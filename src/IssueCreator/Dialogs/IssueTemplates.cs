using IssueCreator.Logging;
using System;
using System.Windows.Forms;

namespace IssueCreator.Dialogs
{
    public partial class IssueTemplates : IssueCreator.Dialogs.ManageIssue
    {
        public IssueTemplates() : base()
        {
            InitializeComponent();
        }

        public IssueTemplates(IssueManager issueManager, Settings settings, FileLogger logger)
            : base(issueManager, settings, logger, null)
        {
            InitializeComponent();
        }

        private void IssueTemplates_Load(object sender, EventArgs e)
        {

        }

        private async void loadIssue_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                return;
            }

            await LoadIssueTemplateDetailsAsync();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
