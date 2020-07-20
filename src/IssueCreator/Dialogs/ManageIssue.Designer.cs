namespace IssueCreator.Dialogs
{
    partial class ManageIssue
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.cboAvailableRepos = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnBrowseEpic = new System.Windows.Forms.Button();
            this.cboEpics = new IssueCreator.Controls.ComboBoxWithSearch();
            this.txtIssueNumber = new IssueCreator.Controls.TextBoxEx();
            this.lblIssueTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 12F);
            this.label1.Location = new System.Drawing.Point(64, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Epic";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 12F);
            this.label2.Location = new System.Drawing.Point(4, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "Issue number";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Calibri", 12F);
            this.btnClose.Location = new System.Drawing.Point(598, 109);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(76, 30);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cboAvailableRepos
            // 
            this.cboAvailableRepos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboAvailableRepos.CausesValidation = false;
            this.cboAvailableRepos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAvailableRepos.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboAvailableRepos.FormattingEnabled = true;
            this.cboAvailableRepos.Location = new System.Drawing.Point(106, 43);
            this.cboAvailableRepos.Name = "cboAvailableRepos";
            this.cboAvailableRepos.Size = new System.Drawing.Size(568, 31);
            this.cboAvailableRepos.Sorted = true;
            this.cboAvailableRepos.TabIndex = 6;
            this.cboAvailableRepos.Validating += new System.ComponentModel.CancelEventHandler(this.cboAvailableRepos_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(25, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 23);
            this.label3.TabIndex = 7;
            this.label3.Text = "Repository";
            // 
            // btnBrowseEpic
            // 
            this.btnBrowseEpic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseEpic.Font = new System.Drawing.Font("Calibri", 12F);
            this.btnBrowseEpic.Location = new System.Drawing.Point(598, 7);
            this.btnBrowseEpic.Name = "btnBrowseEpic";
            this.btnBrowseEpic.Size = new System.Drawing.Size(76, 30);
            this.btnBrowseEpic.TabIndex = 10;
            this.btnBrowseEpic.Text = "Browse";
            this.btnBrowseEpic.UseVisualStyleBackColor = true;
            this.btnBrowseEpic.Click += new System.EventHandler(this.btnBrowseEpic_Click);
            // 
            // cboEpics
            // 
            this.cboEpics.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboEpics.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboEpics.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboEpics.CausesValidation = false;
            this.cboEpics.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboEpics.FormattingEnabled = true;
            this.cboEpics.IntegralHeight = false;
            this.cboEpics.Location = new System.Drawing.Point(106, 8);
            this.cboEpics.Name = "cboEpics";
            this.cboEpics.Size = new System.Drawing.Size(486, 31);
            this.cboEpics.Sorted = true;
            this.cboEpics.TabIndex = 8;
            this.cboEpics.Validating += new System.ComponentModel.CancelEventHandler(this.cboEpics_Validating);
            // 
            // txtIssueNumber
            // 
            this.txtIssueNumber.CausesValidation = false;
            this.txtIssueNumber.Font = new System.Drawing.Font("Calibri", 12F);
            this.txtIssueNumber.Location = new System.Drawing.Point(106, 75);
            this.txtIssueNumber.Name = "txtIssueNumber";
            this.txtIssueNumber.Size = new System.Drawing.Size(55, 32);
            this.txtIssueNumber.TabIndex = 2;
            this.txtIssueNumber.TextChanged += new System.EventHandler(this.txtIssueNumber_TextChanged);
            this.txtIssueNumber.Leave += new System.EventHandler(this.txtIssueNumber_Leave);
            this.txtIssueNumber.Validating += new System.ComponentModel.CancelEventHandler(this.txtIssueNumber_Validating);
            // 
            // lblIssueTitle
            // 
            this.lblIssueTitle.AutoSize = true;
            this.lblIssueTitle.Font = new System.Drawing.Font("Calibri", 12F);
            this.lblIssueTitle.Location = new System.Drawing.Point(167, 78);
            this.lblIssueTitle.Name = "lblIssueTitle";
            this.lblIssueTitle.Size = new System.Drawing.Size(0, 24);
            this.lblIssueTitle.TabIndex = 11;
            // 
            // ManageIssue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(686, 156);
            this.Controls.Add(this.lblIssueTitle);
            this.Controls.Add(this.btnBrowseEpic);
            this.Controls.Add(this.cboEpics);
            this.Controls.Add(this.cboAvailableRepos);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtIssueNumber);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(649, 195);
            this.Name = "ManageIssue";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manage issue";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Label label1;
        protected Controls.TextBoxEx txtIssueNumber;
        protected System.Windows.Forms.Label label2;
        protected System.Windows.Forms.Button btnClose;
        protected System.Windows.Forms.ComboBox cboAvailableRepos;
        protected System.Windows.Forms.Label label3;
        protected Controls.ComboBoxWithSearch cboEpics;
        protected System.Windows.Forms.Button btnBrowseEpic;
        protected System.Windows.Forms.Label lblIssueTitle;
    }
}
