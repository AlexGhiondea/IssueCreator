using IssueCreator.Controls;

namespace IssueCreator.Dialogs
{
    partial class Preferences
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
            this.components = new System.ComponentModel.Container();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtToken = new IssueCreator.Controls.TextBoxEx();
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.lstAvailableRepos = new IssueCreator.Controls.ListBoxWithSearch();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRemoveRepository = new System.Windows.Forms.Button();
            this.btnAddRepository = new System.Windows.Forms.Button();
            this.txtDefaultTitle = new IssueCreator.Controls.TextBoxEx();
            this.label4 = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.lstDefaultLabels = new IssueCreator.Controls.ListBoxWithSearch();
            this.btnRemoveLabel = new System.Windows.Forms.Button();
            this.btnAddLabel = new System.Windows.Forms.Button();
            this.lblTags = new System.Windows.Forms.Label();
            this.txtGitHubToken = new IssueCreator.Controls.TextBoxEx();
            this.label5 = new System.Windows.Forms.Label();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(503, 551);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(136, 30);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(359, 551);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(136, 30);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // txtToken
            // 
            this.txtToken.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtToken.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtToken.Location = new System.Drawing.Point(118, 185);
            this.txtToken.Name = "txtToken";
            this.txtToken.Size = new System.Drawing.Size(521, 27);
            this.txtToken.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 188);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 19);
            this.label2.TabIndex = 11;
            this.label2.Text = "ZenHub Token";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.Location = new System.Drawing.Point(217, 158);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(285, 19);
            this.linkLabel1.TabIndex = 10;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "https://app.zenhub.com/dashboard/tokens";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel1_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(113, 139);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(493, 19);
            this.label1.TabIndex = 9;
            this.label1.Text = "To get a ZenHub token, please access the link below and generate a token.";
            // 
            // lstAvailableRepos
            // 
            this.lstAvailableRepos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstAvailableRepos.FormattingEnabled = true;
            this.lstAvailableRepos.ItemHeight = 19;
            this.lstAvailableRepos.Location = new System.Drawing.Point(118, 229);
            this.lstAvailableRepos.Name = "lstAvailableRepos";
            this.lstAvailableRepos.Size = new System.Drawing.Size(521, 137);
            this.lstAvailableRepos.Sorted = true;
            this.lstAvailableRepos.TabIndex = 13;
            this.toolTip.SetToolTip(this.lstAvailableRepos, "The list of available repositories to load issues from");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 229);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 19);
            this.label3.TabIndex = 14;
            this.label3.Text = "Repositories";
            // 
            // btnRemoveRepository
            // 
            this.btnRemoveRepository.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveRepository.Location = new System.Drawing.Point(40, 305);
            this.btnRemoveRepository.Name = "btnRemoveRepository";
            this.btnRemoveRepository.Size = new System.Drawing.Size(72, 27);
            this.btnRemoveRepository.TabIndex = 19;
            this.btnRemoveRepository.Text = "Remove";
            this.btnRemoveRepository.UseVisualStyleBackColor = true;
            this.btnRemoveRepository.Click += new System.EventHandler(this.BtnRemoveRepository_Click);
            // 
            // btnAddRepository
            // 
            this.btnAddRepository.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddRepository.Location = new System.Drawing.Point(40, 272);
            this.btnAddRepository.Name = "btnAddRepository";
            this.btnAddRepository.Size = new System.Drawing.Size(72, 27);
            this.btnAddRepository.TabIndex = 18;
            this.btnAddRepository.Text = "Add";
            this.btnAddRepository.UseVisualStyleBackColor = true;
            this.btnAddRepository.Click += new System.EventHandler(this.BtnAddRepository_Click);
            // 
            // txtDefaultTitle
            // 
            this.txtDefaultTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDefaultTitle.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDefaultTitle.Location = new System.Drawing.Point(117, 372);
            this.txtDefaultTitle.Name = "txtDefaultTitle";
            this.txtDefaultTitle.Size = new System.Drawing.Size(521, 27);
            this.txtDefaultTitle.TabIndex = 21;
            this.toolTip.SetToolTip(this.txtDefaultTitle, "The value that shows up when the app starts and no other changes have been made");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(22, 375);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 19);
            this.label4.TabIndex = 20;
            this.label4.Text = "Default title";
            // 
            // lstDefaultLabels
            // 
            this.lstDefaultLabels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstDefaultLabels.FormattingEnabled = true;
            this.lstDefaultLabels.IntegralHeight = false;
            this.lstDefaultLabels.ItemHeight = 19;
            this.lstDefaultLabels.Location = new System.Drawing.Point(117, 405);
            this.lstDefaultLabels.Name = "lstDefaultLabels";
            this.lstDefaultLabels.Size = new System.Drawing.Size(521, 118);
            this.lstDefaultLabels.Sorted = true;
            this.lstDefaultLabels.TabIndex = 22;
            this.toolTip.SetToolTip(this.lstDefaultLabels, "The list of labels that will be added by default when you change the repo");
            // 
            // btnRemoveLabel
            // 
            this.btnRemoveLabel.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveLabel.Location = new System.Drawing.Point(39, 481);
            this.btnRemoveLabel.Name = "btnRemoveLabel";
            this.btnRemoveLabel.Size = new System.Drawing.Size(72, 27);
            this.btnRemoveLabel.TabIndex = 25;
            this.btnRemoveLabel.Text = "Remove";
            this.btnRemoveLabel.UseVisualStyleBackColor = true;
            this.btnRemoveLabel.Click += new System.EventHandler(this.BtnRemoveLabel_Click);
            // 
            // btnAddLabel
            // 
            this.btnAddLabel.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddLabel.Location = new System.Drawing.Point(39, 448);
            this.btnAddLabel.Name = "btnAddLabel";
            this.btnAddLabel.Size = new System.Drawing.Size(72, 27);
            this.btnAddLabel.TabIndex = 24;
            this.btnAddLabel.Text = "Add";
            this.btnAddLabel.UseVisualStyleBackColor = true;
            this.btnAddLabel.Click += new System.EventHandler(this.BtnAddLabel_Click);
            // 
            // lblTags
            // 
            this.lblTags.AutoSize = true;
            this.lblTags.Location = new System.Drawing.Point(12, 405);
            this.lblTags.Name = "lblTags";
            this.lblTags.Size = new System.Drawing.Size(100, 19);
            this.lblTags.TabIndex = 23;
            this.lblTags.Text = "Default labels";
            // 
            // txtGitHubToken
            // 
            this.txtGitHubToken.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGitHubToken.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGitHubToken.Location = new System.Drawing.Point(118, 85);
            this.txtGitHubToken.Name = "txtGitHubToken";
            this.txtGitHubToken.Size = new System.Drawing.Size(521, 27);
            this.txtGitHubToken.TabIndex = 29;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 19);
            this.label5.TabIndex = 28;
            this.label5.Text = "GitHub Token";
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel2.Location = new System.Drawing.Point(217, 58);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(269, 19);
            this.linkLabel2.TabIndex = 27;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "https://github.com/settings/tokens/new";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(15, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(628, 19);
            this.label6.TabIndex = 26;
            this.label6.Text = "To get a GitHub token, please access the link below and generate a token with the" +
    " \'repo\' scope.";
            // 
            // Preferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 599);
            this.Controls.Add(this.txtGitHubToken);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnRemoveLabel);
            this.Controls.Add(this.btnAddLabel);
            this.Controls.Add(this.lblTags);
            this.Controls.Add(this.lstDefaultLabels);
            this.Controls.Add(this.txtDefaultTitle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnRemoveRepository);
            this.Controls.Add(this.btnAddRepository);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lstAvailableRepos);
            this.Controls.Add(this.txtToken);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Preferences";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Preferences";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private TextBoxEx txtToken;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label1;
        private Controls.ListBoxWithSearch lstAvailableRepos;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRemoveRepository;
        private System.Windows.Forms.Button btnAddRepository;
        private TextBoxEx txtDefaultTitle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button btnRemoveLabel;
        private System.Windows.Forms.Button btnAddLabel;
        private System.Windows.Forms.Label lblTags;
        private Controls.ListBoxWithSearch lstDefaultLabels;
        private TextBoxEx txtGitHubToken;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.Label label6;
    }
}