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

        public ManageIssue(IssueManager issueManager, Settings settings, FileLogger logger, object[] epicList) : this()
        {
            _issueManager = issueManager;
            _logger = logger;
            _settings = settings;

            cboEpics.Items.AddRange(epicList);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            using IDisposable scope = _logger.CreateScope("Add issue to epic");
            await ExecuteIssueOperation(_issueManager.AddIssueToEpicAsync);
        }

        private async void btnRemove_Click(object sender, EventArgs e)
        {
            using IDisposable scope = _logger.CreateScope("Remove issue to epic");
            await ExecuteIssueOperation(_issueManager.RemoveIssueFromEpicAsync);
        }

        private async Task ExecuteIssueOperation(Func<long, int, long, int, Task<bool>> action)
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

        private async void btnBrowseEpic_Click(object sender, EventArgs e)
        {
            using IDisposable scope = _logger.CreateScope("Browse epic");
            if (cboEpics.SelectedItem != null)
            {
                IssueDescription issue = cboEpics.SelectedItem as IssueDescription;

                string link = (await _issueManager.GetIssueAsync(issue.Repo.Id, issue.Issue.Number)).HtmlUrl;
                _logger.Log($"Found issue html link: {link}");

                // this is a point-in-time check until the updated cache catches up.
                if (link != null)
                {
                    _logger.Log("Launching browser");
                    Process.Start(link);
                }
            }
        }

        private async void txtIssueNumber_Leave(object sender, EventArgs e)
        {
            using IDisposable scope = _logger.CreateScope("Focus lost on issue number box. Attempting to retrieve the issue title.");

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
                lblIssueTitle.Text = "!!! Could not find issue !!!";
            }
        }
    }
}
