using CsvHelper;
using CsvHelper.Configuration;
using IssueCreator.Logging;
using IssueCreator.Models;
using Octokit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IssueCreator.Dialogs
{
    public partial class BulkCreateEpicTree : Form
    {
        protected IssueManager _issueManager;
        protected FileLogger _logger;
        protected Settings _settings;

        public BulkCreateEpicTree()
        {
            InitializeComponent();
        }

        public BulkCreateEpicTree(IssueManager issueManager, Settings settings, FileLogger logger) : this()
        {
            _issueManager = issueManager;
            _logger = logger;
            _settings = settings;
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                LoadDataFromFile(ofd.FileName);
            }
        }

        private DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Organization", typeof(string));
            dt.Columns.Add("Repository", typeof(string));
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("AssignedTo", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Milestone", typeof(string));
            dt.Columns.Add("Estimate", typeof(string));
            dt.Columns.Add("Labels", typeof(string));
            dt.Columns.Add("CreateAsEpic", typeof(string));
            dt.Columns.Add("EpicTitle", typeof(string));
            dt.Columns.Add("EpicOrganization", typeof(string));
            dt.Columns.Add("EpicRepository", typeof(string));
            return dt;
        }

        private void LoadDataFromFile(string fileName)
        {
            // This loads the data from a CSV file with this format:
            // Org,Repo,Title,Description,AssignedTo,Milestone,Estimate,Labels
            using IDisposable scope = _logger.CreateScope($"Loading issues from {fileName}");

            DataTable fileContentAsTable = CreateDataTable();

            using (StreamReader sr = new StreamReader(fileName))
            using (CsvReader csv = new CsvReader(sr, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    csv.Configuration.MissingFieldFound = null;

                    List<object> row = new List<object>();
                    foreach (DataColumn header in fileContentAsTable.Columns)
                    {
                        if (csv.TryGetField(header.DataType, header.ColumnName, out object data))
                        {
                            row.Add(data);
                        }
                        else
                        {
                            row.Add(string.Empty);
                        }
                    }
                    fileContentAsTable.Rows.Add(row.ToArray());
                }
            }

            dgBulkIssues.DataSource = fileContentAsTable;
        }

        private void btnCreateIssues_Click(object sender, EventArgs e)
        {



        }

        private async void button1_Click(object sender, EventArgs e)
        {
            // Ensure that the milestones that are there are valid
            // Identify which milestones need to be created/missing
            // Ensure that the epics that are there are valid

            List<IssueToCreate> issues = new List<IssueToCreate>();

            // Validate milestones
            foreach (DataRow row in ((DataTable)dgBulkIssues.DataSource).Rows)
            {
                IssueToCreate ic = new IssueToCreate();
                ic.Title = row.Field<string>("Title");
                ic.Description = row.Field<string>("Description");
                ic.AssignedTo = row.Field<string>("AssignedTo");
                ic.Labels = new List<string>(row.Field<string>("Labels").Split(','));
                ic.Estimate = row.Field<string>("Estimate");
                if (bool.TryParse(row.Field<string>("CreateAsEpic"), out bool isEpic))
                {
                    ic.CreateAsEpic = isEpic;
                }

//                string milestoneName = row.Field<string>("Milestone").Trim();
//                await _issueManager.GetMilestoneAsync(issue.Organization, issue.Repository, milestone);
            }
        }
    }
}
