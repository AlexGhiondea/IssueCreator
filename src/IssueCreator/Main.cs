using IssueCreator.Models;
using IssueCreator.Controls;
using IssueCreator.Dialogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Deployment.Application;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Octokit;
using IssueCreator.Logging;

namespace IssueCreator
{
    public partial class frmMain : Form
    {
        private static Settings s_settings;
        private static IssueManager s_issueManager;
        private static IssueToCreate s_previouslyCreatedIssue;
        private static FileLogger s_logger;

        private static string SettingsFolder;
        private static string SettingsFile;

        private static string CacheFolder
        {
            get
            {
                // if this is deployed via Click-once, use the data folder
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    return Path.Combine(ApplicationDeployment.CurrentDeployment.DataDirectory, "Cache");
                }
                else
                {
                    // otherwise, use a different folder.
                    return Path.Combine(SettingsFolder, "Cache");
                }
            }
        }


        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            s_logger.Log((e.ExceptionObject as Exception)?.ToString());
        }

        public frmMain()
        {
            SettingsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "IssueCreator");
            SettingsFile = Path.Combine(SettingsFolder, "issueCreator.settings");

            // The directories need to exist prior to anything else
            if (!Directory.Exists(SettingsFolder))
            {
                Directory.CreateDirectory(SettingsFolder);
            }

            if (!Directory.Exists(CacheFolder))
            {
                Directory.CreateDirectory(CacheFolder);
            }

            // Log the launch of the app
            s_logger = new FileLogger(Path.Combine(SettingsFolder, "issueCreator.log"));
            s_logger.Log($"=====>>>>  IssueCreator started  <<<<=====");

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            InitializeComponent();
        }

#pragma warning disable 1998 //We want a fire and forget async method here.
        private async void FrmMain_Load(object sender, EventArgs e)
        {
#pragma warning restore 1998

            using IDisposable scope = s_logger.CreateScope("Loading main form");

            s_settings = Settings.Deserialize(SettingsFile, s_logger);

            s_issueManager = IssueManager.Create(s_settings, CacheFolder, s_logger);

            // if we don't have a github token, prompt settings.
            if (s_issueManager == null)
            {
                s_logger.Log("Opening preferences dialog");
                ShowPreferencesDialog();

                s_issueManager = IssueManager.Create(s_settings, CacheFolder, s_logger);

                if (s_issueManager == null)
                {
                    MessageBox.Show("The application requires a GitHub token in order to work.", "Cannot continue", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(-1);
                }
            }
            else
            {
                s_logger.Log("Loading form settings");
                LoadFormFromSettings();
            }

            // load the version
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                tssVersion.Text = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            else
            {
                tssVersion.Text = "dev";
            }
        }

        private async void LoadFormFromSettings()
        {
            if (s_issueManager == null)
            {
                // nothing can be done.
                return;
            }
            cboAvailableRepos.Items.Clear();
            foreach (string item in s_settings.Repositories)
            {
                cboAvailableRepos.Items.Add(item);
            }

            if (s_settings.DefaultTitle != null)
            {
                txtIssueTitle.Text = s_settings.DefaultTitle;
            }

            s_issueManager.DeserializeCacheDataFromFolder(CacheFolder);

            // Start populating the epics from ZenHub
            await UpdateEpicListAsync();
        }

        private async void BtnCreateIssue_Click(object sender, EventArgs e)
        {
            (string owner, string repo) = GetRepoOwner();

            // build up the list of labels to appen
            List<string> selectedLabels = new List<string>();
            foreach (object item in lstSelectedTags.Items)
            {
                selectedLabels.Add(item.ToString());
            }

            IssueToCreate issueToCreate = new IssueToCreate()
            {
                Repository = repo,
                Organization = owner,
                AssignedTo = cboAssignees.Text,
                Title = txtIssueTitle.Text,
                Description = txtDescription.Text,
                Labels = selectedLabels,
                Epic = (cboEpics.SelectedItem as IssueDescription),
                Milestone = (cboMilestones.SelectedItem as IssueMilestone),
                Estimate = txtEstimate.Text,
                CreateAsEpic = chkMakeEpic.Checked
            };

            // validate that the title is not empty and not the default.
            if (string.IsNullOrEmpty(issueToCreate.Title) || StringComparer.OrdinalIgnoreCase.Equals(issueToCreate.Title, s_settings.DefaultTitle))
            {
                MessageBox.Show(this, "Cannot create an issue with an empty or default title.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // have we created this issue again?
            if (issueToCreate.Equals(s_previouslyCreatedIssue))
            {
                if (MessageBox.Show(this, "This issue has already been created, re-create?", "Create duplicate?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    // if we don't want to re-create the issue
                    return;
                }
            }

            // Try to create the issue
            (bool success, string errorMessage) = await s_issueManager.TryCreateNewIssueAsync(issueToCreate);

            // if an error happened, show the message.
            if (!success)
            {
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // store the current issue as the previous issue.
            s_previouslyCreatedIssue = issueToCreate;
        }

        private void CboAvailableRepos_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdateLabelListAsync();
            UpdateMilestoneListAsync();
            UpdateAssigneesListAsync();
        }

        private async Task UpdateEpicListAsync()
        {
            using IDisposable scope = s_logger.CreateScope("Updating the list of epics");
            if (string.IsNullOrEmpty(s_settings.ZenHubToken))
            {
                return;
            }

            btnRefreshEpics.Enabled = false;
            cboEpics.Enabled = false;
            assignIssueToEpicToolStripMenuItem.Enabled = false;
            preferencesToolStripMenuItem.Enabled = false;
            tssStatus.Text = "Loading ZenHub Epics...";

            // retrieve the issues.
            List<IssueDescription> issues = await s_issueManager.GetEpicsAsync(s_settings.Repositories);

            // add just the open issues
            cboEpics.Items.Clear();
            cboEpics.Items.AddRange(issues.Where((issue) => StringComparer.OrdinalIgnoreCase.Equals(issue.Issue.State, Octokit.ItemState.Open.ToString())).ToArray());

            assignIssueToEpicToolStripMenuItem.Enabled = true;
            preferencesToolStripMenuItem.Enabled = true;
            cboEpics.Enabled = true;
            btnRefreshEpics.Enabled = true;

            tssStatus.Text = $"Loaded {cboEpics.Items.Count} Epics from {s_settings.Repositories.Count} repositories.";
        }


        #region Helpers
        private void MoveItem(ListBoxWithSearch source, ListBoxWithSearch dest)
        {
            if (source.SelectedItem != null)
            {
                dest.Items.Add(source.SelectedItem);
                source.Items.Remove(source.SelectedItem);
            }
        }

        private async void UpdateMilestoneListAsync()
        {
            (string owner, string repository) = GetRepoOwner();

            IEnumerable<IssueMilestone> milestones = await s_issueManager.GetMilestonesAsync(owner, repository);

            cboMilestones.Enabled = false;

            // keep track of the old selected milestone and try to re-apply it in the new repo
            IssueMilestone previousMilestone = cboMilestones.SelectedItem as IssueMilestone;
            IssueMilestone newMilestoneInUI = null;
            cboMilestones.Items.Clear();

            foreach (IssueMilestone item in milestones)
            {
                IssueMilestone newMilestone = item;
                cboMilestones.Items.Add(newMilestone);

                // if a milestone was selected previously and we did have a milestone in the new repo that has the same title
                // then use that in the UI.
                if (previousMilestone != null && previousMilestone.Title == newMilestone.Title)
                {
                    newMilestoneInUI = newMilestone;
                }
            }

            cboMilestones.SelectedItem = newMilestoneInUI;

            // if we did not find a matching milestone in the new repo, clear the text.
            if (newMilestoneInUI == null)
            {
                cboMilestones.Text = string.Empty;
            }

            cboMilestones.Enabled = true;
        }

        private async void UpdateLabelListAsync()
        {
            (string owner, string repository) = GetRepoOwner();

            List<RepoLabel> labels = await s_issueManager.GetLabelsAsync(owner, repository);

            if (labels == null)
            {
                return;
            }

            // Want to only keep in the selected list the labels that are common.
            HashSet<string> inSelectedList = new HashSet<string>(GetItemsAsList(lstSelectedTags));

            // add to the list of selected labels the default ones
            foreach (string item in s_settings.DefaultLabels)
            {
                inSelectedList.Add(item);
            }

            lstAvailableTags.Items.Clear();
            lstSelectedTags.Items.Clear();

            foreach (RepoLabel item in labels)
            {
                if (inSelectedList.Contains(item.Name))
                {
                    lstSelectedTags.Items.Add(item.Name);
                }
                else
                {
                    lstAvailableTags.Items.Add(item.Name);
                }
            }
        }

        private IEnumerable<string> GetItemsAsList(ListBox lstSelectedTags)
        {
            foreach (object item in lstSelectedTags.Items)
            {
                yield return item.ToString();
            }
        }

        private (string, string) GetRepoOwner()
        {
            if (cboAvailableRepos.SelectedItem == null)
                return (string.Empty, string.Empty);

            return GetOwnerAndRepoFromString(cboAvailableRepos.SelectedItem.ToString());
        }

        private (string, string) GetOwnerAndRepoFromString(string input)
        {
            string[] parts = input.Split('\\');
            return (parts[0], parts[1]);
        }

        private async void UpdateAssigneesListAsync()
        {
            (string owner, string repo) = GetRepoOwner();

            cboAssignees.Enabled = false;

            List<Models.GitHubContributor> contributors = await s_issueManager.GetContributorsAsync(owner, repo);

            cboAssignees.Items.Clear();
            foreach (Models.GitHubContributor item in contributors)
            {
                cboAssignees.Items.Add(item.Login);
            }
            cboAssignees.Enabled = true;
        }
        #endregion

        private void ClearDataFromForm()
        {
            lstSelectedTags.Items.Clear();
            lstAvailableTags.Items.Clear();
            cboAssignees.Items.Clear();
        }

        #region Label manipulation
        private void LstAvailableTags_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                MoveItem(lstAvailableTags, lstSelectedTags);
            }
        }

        private void LstSelectedTags_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                MoveItem(lstSelectedTags, lstAvailableTags);
            }
        }

        private void BtnAddTag_Click(object sender, EventArgs e)
        {
            MoveItem(lstAvailableTags, lstSelectedTags);
        }

        private void BtnRemoveTag_Click(object sender, EventArgs e)
        {
            MoveItem(lstSelectedTags, lstAvailableTags);
        }

        private void LstAvailableTags_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MoveItem(lstAvailableTags, lstSelectedTags);
        }

        private void LstSelectedTags_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MoveItem(lstSelectedTags, lstAvailableTags);
        }
        #endregion

        private async void BtnRefreshEpics_Click(object sender, EventArgs e)
        {
            using IDisposable scope = s_logger.CreateScope("Refresing the list of epics");

            //clear the repositories from the cache and then force a refresh
            foreach (string item in s_settings.Repositories)
            {
                (string owner, string repo) = GetOwnerAndRepoFromString(item);
                s_issueManager.RemoveEpicFromCache(owner, repo);
            }
            await UpdateEpicListAsync();
        }

        private void PreferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPreferencesDialog();
        }

        private void ShowPreferencesDialog()
        {
            using IDisposable scope = s_logger.CreateScope("Show preferences dialog");
            Preferences p = new Preferences(s_settings.Clone(), s_issueManager, SettingsFile, s_logger);
            if (p.ShowDialog() == DialogResult.OK)
            {
                s_settings = p.NewSettings;

                // the issue manager was not successfully created, that is why we showed the preferences.
                if (s_issueManager == null)
                {
                    // this will ensure the GH token is valid
                    s_issueManager = IssueManager.Create(s_settings, CacheFolder, s_logger);
                }
                else
                {
                    // refresh the ZenHub token
                    s_issueManager.RefreshZenHubToken(s_settings.ZenHubToken);

                    // refresh the GitHub token
                    s_issueManager.RefreshGitHubToken(s_settings.GitHubToken);
                }

                // save the settings to disk
                s_settings.Serialize(SettingsFile, s_logger);

                // refresh the page
                LoadFormFromSettings();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Close();

        private void assignIssueToEpicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageIssue addTo = new ManageIssue(s_issueManager, s_settings, s_logger);
            addTo.ShowDialog(this);
        }
    }
}
