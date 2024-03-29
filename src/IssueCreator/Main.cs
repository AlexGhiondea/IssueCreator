﻿using IssueCreator.Models;
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
using IssueCreator.Logging;
using IssueCreator.Helpers;
using Octokit;

namespace IssueCreator
{
    public partial class frmMain : Form
    {
        private static Settings s_settings = new Settings();
        private static IssueManager s_issueManager;
        private static IssueToCreate s_model = new IssueToCreate();
        private static IssueToCreate s_previouslyCreatedIssue;
        private static FileLogger s_logger;

        private (string owner, string repo) selectedRepo => UIHelpers.GetRepoOwner(s_settings.SelectedRepository);

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
                    return Path.Combine(Settings.SettingsFolder, "Cache");
                }
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            s_logger.Log((e.ExceptionObject as Exception)?.ToString());
        }

        public frmMain()
        {
            // The directories need to exist prior to anything else
            if (!Directory.Exists(Settings.SettingsFolder))
            {
                Directory.CreateDirectory(Settings.SettingsFolder);
            }

            if (!Directory.Exists(CacheFolder))
            {
                Directory.CreateDirectory(CacheFolder);
            }

            // Log the launch of the app
            s_logger = new FileLogger(Path.Combine(Settings.SettingsFolder, "issueCreator.log"));
            s_logger.Log($"=====>>>>  IssueCreator started  <<<<=====");

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            InitializeComponent();

            txtIssueTitle.DataBindings.Add(nameof(txtIssueTitle.Text), s_model, nameof(s_model.Title), false, DataSourceUpdateMode.OnPropertyChanged);
            txtEstimate.DataBindings.Add(nameof(txtEstimate.Text), s_model, nameof(s_model.Estimate), false, DataSourceUpdateMode.OnPropertyChanged);
            txtDescription.DataBindings.Add(nameof(txtDescription.Text), s_model, nameof(s_model.Description), false, DataSourceUpdateMode.OnPropertyChanged);
            cboAvailableRepos.DataBindings.Add(nameof(cboAvailableRepos.SelectedItem), s_settings, nameof(s_settings.SelectedRepository), false, DataSourceUpdateMode.OnPropertyChanged);
        }

#pragma warning disable 1998 //We want a fire and forget async method here.
        private async void FrmMain_Load(object sender, EventArgs e)
        {
#pragma warning restore 1998

            using IDisposable scope = s_logger.CreateScope("Loading main form");

            s_settings.Initialize(Settings.Deserialize(Settings.SettingsFile, s_logger));

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
            cboAvailableRepos.SelectedItem = s_settings.SelectedRepository;

            if (s_settings.DefaultTitle != null)
            {
                s_model.Title = s_settings.DefaultTitle;
            }

            s_issueManager.DeserializeCacheDataFromFolder(CacheFolder);

            // Start populating the epics from ZenHub
            await UpdateEpicListAsync();
        }

        private async void BtnCreateIssue_Click(object sender, EventArgs e)
        {
            // build up the list of labels to appen
            List<string> selectedLabels = new List<string>();
            foreach (object item in lstSelectedTags.Items)
            {
                selectedLabels.Add(item.ToString());
            }

            IssueToCreate issueToCreate = new IssueToCreate()
            {
                Repository = selectedRepo.repo,
                Organization = selectedRepo.owner,
                AssignedTo = cboAssignees.Text,
                Title = s_model.Title,
                Description = s_model.Description,
                Labels = selectedLabels,
                Epic = (cboEpics.SelectedItem as IssueDescription),
                Milestone = (cboMilestones.SelectedItem as IssueMilestone),
                Estimate = s_model.Estimate,
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
            s_settings.SelectedRepository = cboAvailableRepos.SelectedItem as string;
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
            s_issueManager.IssueLoaded += issueManager_IssueLoadedForLoadAllIssuesEvent;
            // retrieve the issues.
            List<IssueDescription> issues = await s_issueManager.GetEpicsAsync(s_settings.Repositories);

            s_issueManager.IssueLoaded -= issueManager_IssueLoadedForLoadAllIssuesEvent;

            // add just the open issues
            cboEpics.Items.Clear();
            cboEpics.Items.AddRange(issues.Where((issue) => StringComparer.OrdinalIgnoreCase.Equals(issue.Issue.State, Octokit.ItemState.Open.ToString())).ToArray());

            assignIssueToEpicToolStripMenuItem.Enabled = true;
            preferencesToolStripMenuItem.Enabled = true;
            cboEpics.Enabled = true;
            btnRefreshEpics.Enabled = true;

            tssStatus.Text = $"Loaded {cboEpics.Items.Count} Epics from {s_settings.Repositories.Count} repositories.";
        }

        private void issueManager_IssueLoadedForLoadAllIssuesEvent(object sender, IssueObject issue)
        {
            tssStatus.Text = $"Loading ZenHub Epics... Found issue #{issue.Number}";
        }


        #region Helpers
        private void MoveItem(ListBoxWithSearch source, ListBoxWithSearch dest)
        {
            if (source.SelectedItem != null)
            {
                dest.Items.Add(source.SelectedItem);
                source.Items.Remove(source.SelectedItem);
                if(dest == lstSelectedTags)
                {
                    s_model.Labels.Add(source.SelectedItem as string);
                }
                else
                {
                    s_model.Labels.Remove(source.SelectedItem as string);
                }
            }
        }

        private void SetSelectedTags(List<string> tags)
        {
            lstAvailableTags.Items.AddRange(lstSelectedTags.Items);
            foreach (string tag in tags)
            {
                if(tag != null)
                {
                    lstSelectedTags.Items.Add(tag);
                }
            }
            
        }

        private async void UpdateMilestoneListAsync()
        {
            IEnumerable<IssueMilestone> milestones = await s_issueManager.GetMilestonesAsync(selectedRepo.owner, selectedRepo.repo);

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
            List<RepoLabel> labels = await s_issueManager.GetLabelsAsync(selectedRepo.owner, selectedRepo.repo);

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

        private async void UpdateAssigneesListAsync()
        {
            cboAssignees.Enabled = false;

            List<Models.GitHubContributor> contributors = await s_issueManager.GetContributorsAsync(selectedRepo.owner, selectedRepo.repo);

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
                (string owner, string repo) = StringHelpers.GetOwnerAndRepoFromString(item);
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
            Preferences p = new Preferences(s_settings.Clone(), s_issueManager, Settings.SettingsFile, CacheFolder, s_logger);
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
                s_settings.Serialize(Settings.SettingsFile, s_logger);

                // refresh the page
                LoadFormFromSettings();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Close();

        private void assignIssueToEpicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using IDisposable scope = s_logger.CreateScope("Open issue management dialog");

            object[] epics = new object[cboEpics.Items.Count];
            cboEpics.Items.CopyTo(epics, 0);

            ManageIssue addTo = new EpicAssociation(s_issueManager, s_settings, s_logger, epics);
            addTo.ShowDialog(this);
        }

        private void loadIssueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using IDisposable scope = s_logger.CreateScope("Open issue as template dialog");
            IssueTemplates dlg = new IssueTemplates(s_issueManager, s_settings, s_logger);
            s_issueManager.TemplateIssueLoaded += issueManager_IssueLoadedEvent;
            DialogResult result = dlg.ShowDialog(this);  
        }

        private void issueManager_IssueLoadedEvent(object sender, IssueObject issue)
        {
            s_model.Title = issue.Title;
            s_model.Description = issue.Body;
            s_model.Labels = issue.Tags;
            SetSelectedTags(s_model.Labels);
            s_issueManager.TemplateIssueLoaded -= issueManager_IssueLoadedEvent;
        }

        private void cboAvailableRepos_SelectedIndexChanged(object sender, EventArgs e)
        {
            s_settings.SelectedRepository = cboAvailableRepos.SelectedItem as string;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // save the settings to disk
            s_settings.Serialize(Settings.SettingsFile, s_logger);
        }

        private void bulkCreateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using IDisposable scope = s_logger.CreateScope("Open bulk issue assignment dialog");

            object[] epics = new object[cboEpics.Items.Count];
            cboEpics.Items.CopyTo(epics, 0);

            BulkCreateIssues bci = new BulkCreateIssues(s_issueManager, s_settings, s_logger, epics);
            bci.ShowDialog(this);
        }
    }
}
