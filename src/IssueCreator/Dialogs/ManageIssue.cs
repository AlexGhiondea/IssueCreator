using IssueCreator.Helpers;
using IssueCreator.Logging;
using IssueCreator.Models;
using Octokit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IssueCreator.Dialogs
{
    public partial class ManageIssue : Form
    {
        private IssueManager _issueManager;
        private FileLogger _logger;
        private Settings _settings;

        public ManageIssue()
        {
            InitializeComponent();
        }

        public ManageIssue(IssueManager issueManager, Settings settings, FileLogger logger) : this()
        {
            _issueManager = issueManager;
            _logger = logger;
            _settings = settings;

            LoadFormSettings();
        }

        private async void LoadFormSettings()
        {
            cboAvailableRepos.Items.Clear();
            foreach (string item in _settings.Repositories)
            {
                cboAvailableRepos.Items.Add(item);
            }

            // load the list of epics
            await UpdateEpicListAsync();
        }

        private async Task UpdateEpicListAsync()
        {
            using IDisposable scope = _logger.CreateScope("Updating the list of epics");
            if (string.IsNullOrEmpty(_settings.ZenHubToken))
            {
                return;
            }

            cboEpics.Enabled = false;

            // retrieve the issues.
            List<IssueDescription> issues = await _issueManager.GetEpicsAsync(_settings.Repositories);

            // add just the open issues
            cboEpics.Items.Clear();
            cboEpics.Items.AddRange(issues.Where((issue) => StringComparer.OrdinalIgnoreCase.Equals(issue.Issue.State, Octokit.ItemState.Open.ToString())).ToArray());

            cboEpics.Enabled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            await ExecuteIssueOperation();
        }

        private async Task ExecuteIssueOperation(Func<long, int,long,int, Task<bool>> action)
        {
            if (!int.TryParse(txtIssueNumber.Text, out int issueNumber))
            {
                MessageBox.Show("Please specify a number for the issue");
            }

            if (cboAvailableRepos.SelectedItem == null)
            {
                MessageBox.Show("Please select a repository");
            }

            if (cboEpics.SelectedItem == null)
            {
                MessageBox.Show("Please select an epic");
            }

            // get the repository
            (string owner, string repo) = UIHelpers.GetRepoOwner(cboAvailableRepos.SelectedItem);

            RepositoryInfo repoFromGH = await _issueManager.GetRepositoryAsync(owner, repo);
            IssueDescription epicInfo = cboEpics.SelectedItem as IssueDescription;

            bool result = await action(epicInfo.Repo.Id, epicInfo.Issue.Number, repoFromGH.Id, issueNumber);

            if (!result)
            {
                MessageBox.Show("Coult not associate the issue with the epic");
            }
            else
            {
                MessageBox.Show("Done!");
            }
        }

        private async void btnRemove_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtIssueNumber.Text, out int issueNumber))
            {
                MessageBox.Show("Please specify a number for the issue");
            }

            if (cboAvailableRepos.SelectedItem == null)
            {
                MessageBox.Show("Please select a repository");
            }

            if (cboEpics.SelectedItem == null)
            {
                MessageBox.Show("Please select an epic");
            }

            // get the repository
            (string owner, string repo) = UIHelpers.GetRepoOwner(cboAvailableRepos.SelectedItem);

            RepositoryInfo repoFromGH = await _issueManager.GetRepositoryAsync(owner, repo);
            IssueDescription epicInfo = cboEpics.SelectedItem as IssueDescription;

            bool result = await _issueManager.RemoveIssueFromEpicAsync(epicInfo.Repo.Id, epicInfo.Issue.Number, repoFromGH.Id, issueNumber);

            if (!result)
            {
                MessageBox.Show(this, "Coult not associate the issue with the epic");
            }
            else
            {
                MessageBox.Show(this, "Done!");
            }
        }

        private async void btnBrowseEpic_Click(object sender, EventArgs e)
        {
            if (cboEpics.SelectedItem != null)
            {
                IssueDescription issue = cboEpics.SelectedItem as IssueDescription;

                string link = (await _issueManager.GetIssueAsync(issue.Repo.Id, issue.Issue.Number)).HtmlUrl;

                // this is a point-in-time check until the updated cache catches up.
                if (link != null)
                {
                    Process.Start(link);
                }
            }
        }

        private async void txtIssueNumber_Leave(object sender, EventArgs e)
        {
            // if we can't parse the number, don't show it.
            if (!int.TryParse(txtIssueNumber.Text, out int issueNumber))
            {
                return;
            }

            // try to load the issue from the github and show the title.
            try
            {
                // get the repository
                (string owner, string repo) = UIHelpers.GetRepoOwner(cboAvailableRepos.SelectedItem);

                RepositoryInfo repoFromGH = await _issueManager.GetRepositoryAsync(owner, repo);

                IssueObject issue = await _issueManager.GetIssueAsync(repoFromGH.Id, issueNumber);

                lblIssueTitle.Text = issue.Title;
            }
            catch{
                lblIssueTitle.Text = "<issueNotFound>";
            }
        }
    }
}
