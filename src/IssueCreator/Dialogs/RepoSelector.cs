using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IssueCreator
{
    public partial class RepoSelector : Form
    {
        private IssueManager _issueManager;

        public RepoSelector()
        {
            InitializeComponent();
        }

        internal RepoSelector(IssueManager issueMgr) : this()
        {
            _issueManager = issueMgr;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private async void BtnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOrg.Text) ||
                string.IsNullOrEmpty(txtRepo.Text))
            {
                MessageBox.Show($"Both organization and repo are required.{Environment.NewLine}Example: Organization:Azure, Repository:azure-sdk");
                return;
            }

            if (!await ValidateRepo(txtOrg.Text, txtRepo.Text))
            {
                MessageBox.Show($"Invalid repo and/or organization specified.{Environment.NewLine} {txtOrg.Text}\\{txtRepo.Text} could not be found");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private async Task<bool> ValidateRepo(string organization, string repository)
        {
            try
            {
                var repo = await _issueManager.GetRepositoryAsync(organization, repository);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string Repo
        {
            get
            {
                return txtOrg.Text + "\\" + txtRepo.Text;
            }
        }
    }
}
