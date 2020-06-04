using IssueCreator.Helpers;
using IssueCreator.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace IssueCreator.Dialogs
{
    public partial class Preferences : Form
    {
        private const string TokenProvided = "<token provided>";
        private IssueManager _issueManager;
        public Settings NewSettings { get; }
        private string _settingsFile;
        private FileLogger _logger;

        public Preferences()
        {
            InitializeComponent();
        }

        public Preferences(Settings settings, IssueManager issueManager, string settingsFile, FileLogger logger) : this()
        {
            _logger = logger;
            NewSettings = settings;
            _issueManager = issueManager;
            _settingsFile = settingsFile;

            foreach (string item in NewSettings.Repositories)
            {
                lstAvailableRepos.Items.Add(item);
            }

            foreach (string item in NewSettings.DefaultLabels)
            {
                lstDefaultLabels.Items.Add(item);
            }

            if (!string.IsNullOrEmpty(NewSettings.ZenHubToken))
            {
                txtToken.Text = TokenProvided;
            }

            if (!string.IsNullOrEmpty(NewSettings.GitHubToken))
            {
                txtGitHubToken.Text = TokenProvided;
            }

            if (!string.IsNullOrEmpty(NewSettings.DefaultTitle))
            {
                txtDefaultTitle.Text = NewSettings.DefaultTitle;
            }
        }

        private void BtnAddRepository_Click(object sender, EventArgs e)
        {
            RepoSelector input = new RepoSelector(_issueManager);
            if (input.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            if (string.IsNullOrEmpty(input.txtOrg.Text))
            {
                return;
            }
            string text = input.Repo;

            if (NewSettings.Repositories.Contains(text, StringComparer.InvariantCultureIgnoreCase))
            {
                return;
            }

            NewSettings.Repositories.Add(text);
            lstAvailableRepos.Items.Add(text);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (txtToken.Text != TokenProvided)
            {
                NewSettings.ZenHubToken = txtToken.Text;
            }

            if (txtGitHubToken.Text != TokenProvided)
            {
                NewSettings.GitHubToken = txtGitHubToken.Text;
            }

            NewSettings.DefaultTitle = txtDefaultTitle.Text;

            NewSettings.Serialize(_settingsFile, _logger);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void BtnRemoveRepository_Click(object sender, EventArgs e)
        {
            if (lstAvailableRepos.SelectedItem == null)
            {
                return;
            }

            string text = lstAvailableRepos.SelectedItem.ToString();
            (string owner, string repo) = StringHelpers.GetOwnerAndRepoFromString(text);
            if (MessageBox.Show(this, $"Remove the repository '{text}' ?", "Are you sure?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                NewSettings.Repositories.Remove(text);
                lstAvailableRepos.Items.Remove(text);

                //remove it from the cache
                _issueManager.RemoveRepoFromCache(owner, repo);
            }
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start(linkLabel1.Text);

        private void BtnAddLabel_Click(object sender, EventArgs e)
        {
            TextInput ti = new TextInput("Please enter the label name");
            if (ti.ShowDialog() == DialogResult.OK)
            {
                if (!NewSettings.DefaultLabels.Contains(ti.InputText))
                {
                    NewSettings.DefaultLabels.Add(ti.InputText);
                    lstDefaultLabels.Items.Add(ti.InputText);
                }
            }
        }

        private void BtnRemoveLabel_Click(object sender, EventArgs e)
        {
            if (lstDefaultLabels.SelectedItem == null)
            {
                return;
            }

            string text = lstDefaultLabels.SelectedItem.ToString();

            if (MessageBox.Show(this, $"Remove default label '{text}' ?", "Are you sure?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                NewSettings.DefaultLabels.Remove(text);
                lstDefaultLabels.Items.Remove(text);
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start(linkLabel2.Text);
    }
}
