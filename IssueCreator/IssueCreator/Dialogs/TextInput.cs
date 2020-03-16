using System;
using System.Windows.Forms;

namespace IssueCreator
{
    public partial class TextInput: Form
    {
        public string InputText { get; private set; } = string.Empty;
        public TextInput()
        {
            InitializeComponent();
        }
        public TextInput(string info):this()
        {
            lblInfo.Text = info;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            InputText = txtResult.Text; 

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
