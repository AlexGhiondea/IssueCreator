using IssueCreator.Helpers;
using IssueCreator.Logging;
using IssueCreator.Models;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IssueCreator.Dialogs
{
    public partial class ManageIssue : Form
    {
        protected IssueManager _issueManager;
        protected FileLogger _logger;
        protected Settings _settings;
        protected IssueObject LastIssueLoaded { get; private set; }

        public ManageIssue()
        {
            InitializeComponent();
        }

        public ManageIssue(IssueManager issueManager, Settings settings, FileLogger logger, object[] epicList) : this()
        {
            _issueManager = issueManager;
            _logger = logger;
            _settings = settings;
            if(epicList != null)
            {
                cboEpics.Items.AddRange(epicList);
            }
            
            cboAvailableRepos.Items.AddRange(settings.Repositories.ToArray());
            if(cboAvailableRepos.Items.Count == 1)
            {
                cboAvailableRepos.SelectedIndex = 0;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected async Task ExecuteIssueOperation(Func<long, int, long, int, Task<bool>> action, string scopeText)
        {
            using IDisposable scope = _logger.CreateScope(scopeText);
            
            setValidation(true);

            if (!this.ValidateChildren())
            {
                return;
            }
            // get the repository
            (string owner, string repo) = UIHelpers.GetRepoOwner(cboAvailableRepos.SelectedItem);

            RepositoryInfo repoFromGH = await _issueManager.GetRepositoryAsync(owner, repo);
            IssueDescription epicInfo = cboEpics.SelectedItem as IssueDescription;

            bool result = await action(epicInfo.Repo.Id, epicInfo.Issue.Number, repoFromGH.Id, int.Parse(txtIssueNumber.Text));

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

                string link = (await _issueManager.GetIssueAsync(issue.Repo.Id, issue.Issue.Number, IssueLoadScenario.BrowseEpic)).HtmlUrl;
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
            await LoadIssueDetails(IssueLoadScenario.BrowseEpic);
        }

        protected async Task LoadIssueDetails(IssueLoadScenario loadScenario)
        {
            // if we can't parse the number, don't show it.
            if (!txtIssueNumber.CausesValidation || !int.TryParse(txtIssueNumber.Text, out int issueNumber))
            {
                return;
            }

            // try to load the issue from the github and show the title.
            try
            {
                // get the repository
                (string owner, string repo) = UIHelpers.GetRepoOwner(cboAvailableRepos.SelectedItem);

                RepositoryInfo repoFromGH = await _issueManager.GetRepositoryAsync(owner, repo);

                IssueObject issue = await _issueManager.GetIssueAsync(repoFromGH.Id, issueNumber, loadScenario);

                lblIssueTitle.Text = issue.Title;

            }
            catch
            {
                lblIssueTitle.Text = "!!! Could not find issue !!!";
            }
        }

        private void setValidation(bool enabled)
        {
            cboEpics.CausesValidation = enabled;
            cboAvailableRepos.CausesValidation = enabled;
            txtIssueNumber.CausesValidation = enabled;
        }

        private void cboEpics_Validating(object sender, CancelEventArgs e)
        {
            if (cboEpics.SelectedItem == null)
            {
                MessageBox.Show("Please select an epic");
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void cboAvailableRepos_Validating(object sender, CancelEventArgs e)
        {
            if (cboAvailableRepos.SelectedItem == null)
            {
                MessageBox.Show("Please select a repository");
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void txtIssueNumber_Validating(object sender, CancelEventArgs e)
        {
            if (!int.TryParse(txtIssueNumber.Text, out int _))
            {
                MessageBox.Show("Please specify a number for the issue");
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void txtIssueNumber_TextChanged(object sender, EventArgs e)
        {
            txtIssueNumber.CausesValidation = true;
        }
    }
}
