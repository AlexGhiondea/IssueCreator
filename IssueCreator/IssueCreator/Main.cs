using IssueCreator.Models;
using IssueCreator.Controls;
using IssueCreator.Dialogs;
using Octokit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Deployment.Application;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IssueCreator
{
    public partial class frmMain : Form
    {
        private static Settings s_settings;
        private static IssueManager s_issueManager;
        private static IssueToCreate s_previouslyCreatedIssue;

        private static string SettingsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "issueCreator.settings");

        public frmMain()
        {
            InitializeComponent();
        }

        private async void FrmMain_Load(object sender, EventArgs e)
        {
            s_settings = Settings.Deserialize(SettingsFile);

            s_issueManager = IssueManager.Create(s_settings);

            // if we don't have a github token, prompt settings.
            if (s_issueManager == null)
            {
                ShowPreferencesDialog();

                s_issueManager = IssueManager.Create(s_settings);

                if (s_issueManager == null)
                {
                    MessageBox.Show("The application requires a GitHub token in order to work.", "Cannot continue", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(-1);
                }
            }
            else
            {
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

        private void LoadFormFromSettings()
        {
            if (s_issueManager == null)
            {
                // nothing can be done.
                return;
            }
            cboAvailableRepos.Items.Clear();
            foreach (var item in s_settings.Repositories)
            {
                cboAvailableRepos.Items.Add(item);
            }

            if (!string.IsNullOrEmpty(s_settings.DefaultTitle))
            {
                txtIssueTitle.Text = s_settings.DefaultTitle;
            }

            // Start populating the epics from ZenHub
            UpdateEpicListAsync();
        }

        private async void BtnCreateIssue_Click(object sender, EventArgs e)
        {
            (string org, string repo) = GetRepoOwner();

            // build up the list of labels to appen
            List<string> selectedLabels = new List<string>();
            foreach (var item in lstSelectedTags.Items)
            {
                selectedLabels.Add(item.ToString());
            }

            IssueToCreate issueToCreate = new IssueToCreate()
            {
                Repository = repo,
                Organization = org,
                AssignedTo = cboAssignees.Text,
                Title = txtIssueTitle.Text,
                Description = txtDescription.Text,
                Labels = selectedLabels,
                Epic = (cboEpics.SelectedItem as IssueDescription),
                Milestone = (cboMilestones.SelectedItem as IssueCreator.Models.IssueMilestone),
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

        private async void UpdateEpicListAsync()
        {
            if (string.IsNullOrEmpty(s_settings.ZenHubToken))
            {
                return;
            }

            btnRefreshEpics.Enabled = false;
            cboEpics.Enabled = false;
            preferencesToolStripMenuItem.Enabled = false;
            tssStatus.Text = "Loading ZenHub Epics...";

            // retrieve the issues.
            var issues = await s_issueManager.GetEpicsAsync(s_settings.Repositories);

            // add just the open issues
            cboEpics.Items.Clear();
            cboEpics.Items.AddRange(issues.Where((issue) => issue.Issue.State == ItemState.Open).ToArray());

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

            IReadOnlyList<Octokit.Milestone> milestones = await s_issueManager.GetMilestonesAsync(owner, repository);

            cboMilestones.Enabled = false;

            cboMilestones.Items.Clear();
            foreach (var item in milestones)
            {
                cboMilestones.Items.Add(new IssueMilestone(item));
            }
            cboMilestones.Enabled = true;
        }

        private async void UpdateLabelListAsync()
        {
            (string owner, string repository) = GetRepoOwner();

            IReadOnlyList<Octokit.Label> labels = await s_issueManager.GetLabelsAsync(owner, repository);

            if (labels == null)
            {
                return;
            }

            // Want to only keep in the selected list the labels that are common.
            var inSelectedList = new HashSet<string>(GetItemsAsList(lstSelectedTags));

            // add to the list of selected labels the default ones
            foreach (var item in s_settings.DefaultLabels)
            {
                inSelectedList.Add(item);
            }

            lstAvailableTags.Items.Clear();
            lstSelectedTags.Items.Clear();

            foreach (var item in labels)
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
            foreach (var item in lstSelectedTags.Items)
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
            (string org, string repo) = GetRepoOwner();

            cboAssignees.Enabled = false;

            IReadOnlyList<Octokit.RepositoryContributor> contributors = await s_issueManager.GetContributorsAsync(org, repo);

            cboAssignees.Items.Clear();
            foreach (var item in contributors)
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

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            s_settings.Serialize(SettingsFile);
        }
        #endregion

        private void BtnRefreshEpics_Click(object sender, EventArgs e)
        {
            //clear the repositories from the cache and then force a refresh

            foreach (var item in s_settings.Repositories)
            {
                (string org, string repo) = GetOwnerAndRepoFromString(item);
                s_issueManager.RemoveEpicFromCache(org, repo);
            }

            UpdateEpicListAsync();
        }

        private void PreferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPreferencesDialog();
        }

        private void ShowPreferencesDialog()
        {
            Preferences p = new Preferences(s_settings.Clone(), s_issueManager, SettingsFile);
            if (p.ShowDialog() == DialogResult.OK)
            {
                s_settings = p.NewSettings;

                // the issue manager was not successfully created, that is why we showed the preferences.
                if (s_issueManager == null)
                {
                    // this will ensure the GH token is valid
                    s_issueManager = IssueManager.Create(s_settings);
                }
                else
                {
                    // refresh the ZenHub token
                    s_issueManager.RefreshZenHubToken(s_settings.ZenHubToken);

                    // refresh the GitHub token
                    s_issueManager.RefreshGitHubToken(s_settings.GitHubToken);
                }

                // refresh the page
                LoadFormFromSettings();
            }
        }
    }
}
