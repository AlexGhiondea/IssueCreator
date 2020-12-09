using CsvHelper;
using CsvHelper.Configuration;
using IssueCreator.Logging;
using IssueCreator.Models;
using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IssueCreator.Dialogs
{
    public partial class BulkCreateIssues : Form
    {
        protected IssueManager _issueManager;
        protected FileLogger _logger;
        protected Settings _settings;

        public BulkCreateIssues()
        {
            InitializeComponent();
        }

        public BulkCreateIssues(IssueManager issueManager, Settings settings, FileLogger logger, object[] epicList) : this()
        {
            _issueManager = issueManager;
            _logger = logger;
            _settings = settings;
            if (epicList != null)
            {
                cboEpics.Items.AddRange(epicList);
            }
        }

        private async void btnBrowseEpic_Click(object sender, EventArgs e)
        {
            using IDisposable scope = _logger.CreateScope("Browse epic");
            if (cboEpics.SelectedItem != null)
            {
                IssueDescription issue = cboEpics.SelectedItem as IssueDescription;

                string link = (await _issueManager.GetBrowseableIssueAsync(issue.Repo.Id, issue.Issue.Number)).HtmlUrl;
                _logger.Log($"Found issue html link: {link}");

                // this is a point-in-time check until the updated cache catches up.
                if (link != null)
                {
                    _logger.Log("Launching browser");
                    Process.Start(link);
                }
            }
        }

        private async void btnLoadData_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                issueToCreateBindingSource.DataSource = await LoadDataFromFileAsync(ofd.FileName);
            }

        }

        private async Task<List<IssueToCreate>> LoadDataFromFileAsync(string fileName)
        {
            // This loads the data from a CSV file with this format:
            // Org,Repo,Title,Description,AssignedTo,Milestone,Estimate,Labels
            using IDisposable scope = _logger.CreateScope($"Loading issues from {fileName}");

            List<IssueToCreate> issues = new List<IssueToCreate>();

            using (StreamReader sr = new StreamReader(fileName))
            using (CsvReader csv = new CsvReader(sr, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    csv.Configuration.MissingFieldFound = null;

                    IssueToCreate issue = new IssueToCreate
                    {
                        Organization = csv.GetField<string>("Organization").Trim(),
                        Repository = csv.GetField<string>("Repository").Trim(),
                        Title = csv.GetField<string>("Title").Trim(),
                        Description = csv.GetField<string>("Description").Trim(),
                        AssignedTo = csv.GetField<string>("AssignedTo").Trim(),
                        Estimate = csv.GetField<string>("Estimate").Trim(),
                    };

                    string milestone = csv.GetField<string>("Milestone");
                    if (!string.IsNullOrWhiteSpace(milestone))
                    {
                        milestone = milestone.Trim();
                        issue.Milestone = await _issueManager.GetMilestoneAsync(issue.Organization, issue.Repository, milestone);
                    }

                    string labels = csv.GetField<string>("Labels");
                    if (!string.IsNullOrWhiteSpace(labels))
                    {
                        labels = labels.Trim();
                        issue.Labels = new List<string>(labels.Split(','));
                    }

                    issues.Add(issue);
                }
            }

            return issues;
        }

        private async void btnCreateIssues_Click(object sender, EventArgs e)
        {
            List<IssueToCreate> list = (List<IssueToCreate>)issueToCreateBindingSource.DataSource;

            while (list.Count > 0)
            {
                IssueToCreate issue = list[0];

                // validate that the title is not empty and not the default.
                if (string.IsNullOrEmpty(issue.Title))
                {
                    MessageBox.Show(this, "Cannot create an issue with an empty or default title.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }

                // assign the epic to the issue!
                issue.Epic = (IssueDescription)cboEpics.SelectedItem;

                (bool success, string errorMsg) = await _issueManager.TryCreateNewIssueAsync(issue);
                if (!success)
                {
                    MessageBox.Show($"Could not create issue {issue.Title} in repository {issue.Organization}\\{issue.Repository} {Environment.NewLine} {errorMsg}");
                    continue;
                }
                else
                {
                    // update the datasource
                    list.Remove(issue);
                    issueToCreateBindingSource.DataSource = list;
                    issueToCreateBindingSource.ResetBindings(false);
                }
            }
        }
    }
}
