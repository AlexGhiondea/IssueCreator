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

                    issue.Milestone = await _issueManager.GetMilestoneAsync(issue.Organization, issue.Repository, csv.GetField<string>("Milestone").Trim());

                    string labels = csv.GetField<string>("Labels").Trim();
                    if (!string.IsNullOrEmpty(labels))
                    {
                        issue.Labels = new List<string>(labels.Split(','));
                    }

                    issues.Add(issue);
                }
            }

            return issues;
        }
    }
}
