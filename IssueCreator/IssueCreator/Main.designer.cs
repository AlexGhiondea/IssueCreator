using IssueCreator.Controls;
using System.Windows.Forms;

namespace IssueCreator
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.label1 = new System.Windows.Forms.Label();
            this.txtIssueTitle = new IssueCreator.Controls.TextBoxEx();
            this.label2 = new System.Windows.Forms.Label();
            this.cboAvailableRepos = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDescription = new IssueCreator.Controls.TextBoxEx();
            this.btnAddTag = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnRemoveTag = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.cboAssignees = new IssueCreator.Controls.ComboBoxWithSearch();
            this.btnCreateIssue = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tssVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblEpic = new System.Windows.Forms.Label();
            this.btnRefreshEpics = new System.Windows.Forms.Button();
            this.txtEstimate = new IssueCreator.Controls.TextBoxEx();
            this.label8 = new System.Windows.Forms.Label();
            this.cboEpics = new IssueCreator.Controls.ComboBoxWithSearch();
            this.lstAvailableTags = new IssueCreator.Controls.ListBoxWithSearch();
            this.lstSelectedTags = new IssueCreator.Controls.ListBoxWithSearch();
            this.chkMakeEpic = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cboMilestones = new IssueCreator.Controls.ComboBoxWithSearch();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(54, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Title";
            // 
            // txtIssueTitle
            // 
            this.txtIssueTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIssueTitle.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIssueTitle.Location = new System.Drawing.Point(93, 108);
            this.txtIssueTitle.Name = "txtIssueTitle";
            this.txtIssueTitle.Size = new System.Drawing.Size(497, 26);
            this.txtIssueTitle.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(15, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Repository";
            // 
            // cboAvailableRepos
            // 
            this.cboAvailableRepos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboAvailableRepos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAvailableRepos.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboAvailableRepos.FormattingEnabled = true;
            this.cboAvailableRepos.Location = new System.Drawing.Point(93, 40);
            this.cboAvailableRepos.Name = "cboAvailableRepos";
            this.cboAvailableRepos.Size = new System.Drawing.Size(497, 26);
            this.cboAvailableRepos.Sorted = true;
            this.cboAvailableRepos.TabIndex = 1;
            this.cboAvailableRepos.SelectedValueChanged += new System.EventHandler(this.CboAvailableRepos_SelectedValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 140);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "Description";
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtDescription.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescription.Location = new System.Drawing.Point(93, 140);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(497, 151);
            this.txtDescription.TabIndex = 4;
            // 
            // btnAddTag
            // 
            this.btnAddTag.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAddTag.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddTag.Location = new System.Drawing.Point(325, 340);
            this.btnAddTag.Name = "btnAddTag";
            this.btnAddTag.Size = new System.Drawing.Size(39, 26);
            this.btnAddTag.TabIndex = 8;
            this.btnAddTag.TabStop = false;
            this.btnAddTag.Text = ">";
            this.btnAddTag.UseVisualStyleBackColor = true;
            this.btnAddTag.Click += new System.EventHandler(this.BtnAddTag_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(56, 299);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 18);
            this.label4.TabIndex = 9;
            this.label4.Text = "Tags";
            // 
            // btnRemoveTag
            // 
            this.btnRemoveTag.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnRemoveTag.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveTag.Location = new System.Drawing.Point(325, 372);
            this.btnRemoveTag.Name = "btnRemoveTag";
            this.btnRemoveTag.Size = new System.Drawing.Size(39, 28);
            this.btnRemoveTag.TabIndex = 11;
            this.btnRemoveTag.TabStop = false;
            this.btnRemoveTag.Text = "<";
            this.btnRemoveTag.UseVisualStyleBackColor = true;
            this.btnRemoveTag.Click += new System.EventHandler(this.BtnRemoveTag_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(10, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 18);
            this.label5.TabIndex = 12;
            this.label5.Text = "Assigned to";
            // 
            // cboAssignees
            // 
            this.cboAssignees.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboAssignees.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboAssignees.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboAssignees.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboAssignees.FormattingEnabled = true;
            this.cboAssignees.Location = new System.Drawing.Point(93, 76);
            this.cboAssignees.Name = "cboAssignees";
            this.cboAssignees.Size = new System.Drawing.Size(497, 26);
            this.cboAssignees.Sorted = true;
            this.cboAssignees.TabIndex = 2;
            // 
            // btnCreateIssue
            // 
            this.btnCreateIssue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateIssue.Font = new System.Drawing.Font("Calibri", 16F);
            this.btnCreateIssue.Location = new System.Drawing.Point(93, 532);
            this.btnCreateIssue.Name = "btnCreateIssue";
            this.btnCreateIssue.Size = new System.Drawing.Size(497, 71);
            this.btnCreateIssue.TabIndex = 8;
            this.btnCreateIssue.Text = "Create issue";
            this.btnCreateIssue.UseVisualStyleBackColor = true;
            this.btnCreateIssue.Click += new System.EventHandler(this.BtnCreateIssue_Click);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(90, 299);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 18);
            this.label6.TabIndex = 14;
            this.label6.Text = "Available";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(531, 299);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 18);
            this.label7.TabIndex = 15;
            this.label7.Text = "Selected";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssVersion,
            this.tssStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 624);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(605, 22);
            this.statusStrip1.TabIndex = 18;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tssVersion
            // 
            this.tssVersion.Name = "tssVersion";
            this.tssVersion.Size = new System.Drawing.Size(0, 17);
            // 
            // tssStatus
            // 
            this.tssStatus.Name = "tssStatus";
            this.tssStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(605, 24);
            this.menuStrip1.TabIndex = 19;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.preferencesToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.preferencesToolStripMenuItem.Text = "Preferences";
            this.preferencesToolStripMenuItem.Click += new System.EventHandler(this.PreferencesToolStripMenuItem_Click);
            // 
            // lblEpic
            // 
            this.lblEpic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblEpic.AutoSize = true;
            this.lblEpic.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEpic.Location = new System.Drawing.Point(6, 443);
            this.lblEpic.Name = "lblEpic";
            this.lblEpic.Size = new System.Drawing.Size(84, 18);
            this.lblEpic.TabIndex = 21;
            this.lblEpic.Text = "ZenHub Epic";
            // 
            // btnRefreshEpics
            // 
            this.btnRefreshEpics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshEpics.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefreshEpics.Location = new System.Drawing.Point(499, 439);
            this.btnRefreshEpics.Name = "btnRefreshEpics";
            this.btnRefreshEpics.Size = new System.Drawing.Size(91, 27);
            this.btnRefreshEpics.TabIndex = 22;
            this.btnRefreshEpics.Text = "Refresh";
            this.btnRefreshEpics.UseVisualStyleBackColor = true;
            this.btnRefreshEpics.Click += new System.EventHandler(this.BtnRefreshEpics_Click);
            // 
            // txtEstimate
            // 
            this.txtEstimate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtEstimate.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEstimate.Location = new System.Drawing.Point(93, 472);
            this.txtEstimate.Name = "txtEstimate";
            this.txtEstimate.Size = new System.Drawing.Size(63, 26);
            this.txtEstimate.TabIndex = 24;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(29, 475);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 18);
            this.label8.TabIndex = 23;
            this.label8.Text = "Estimate";
            // 
            // cboEpics
            // 
            this.cboEpics.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboEpics.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboEpics.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboEpics.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboEpics.FormattingEnabled = true;
            this.cboEpics.IntegralHeight = false;
            this.cboEpics.Location = new System.Drawing.Point(93, 440);
            this.cboEpics.Name = "cboEpics";
            this.cboEpics.Size = new System.Drawing.Size(400, 26);
            this.cboEpics.Sorted = true;
            this.cboEpics.TabIndex = 7;
            // 
            // lstAvailableTags
            // 
            this.lstAvailableTags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lstAvailableTags.Font = new System.Drawing.Font("Calibri", 11.25F);
            this.lstAvailableTags.FormattingEnabled = true;
            this.lstAvailableTags.IntegralHeight = false;
            this.lstAvailableTags.ItemHeight = 18;
            this.lstAvailableTags.Location = new System.Drawing.Point(93, 320);
            this.lstAvailableTags.Name = "lstAvailableTags";
            this.lstAvailableTags.Size = new System.Drawing.Size(226, 114);
            this.lstAvailableTags.Sorted = true;
            this.lstAvailableTags.TabIndex = 5;
            this.lstAvailableTags.KeyUp += new System.Windows.Forms.KeyEventHandler(this.LstAvailableTags_KeyUp);
            this.lstAvailableTags.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LstAvailableTags_MouseDoubleClick);
            // 
            // lstSelectedTags
            // 
            this.lstSelectedTags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lstSelectedTags.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstSelectedTags.FormattingEnabled = true;
            this.lstSelectedTags.IntegralHeight = false;
            this.lstSelectedTags.ItemHeight = 18;
            this.lstSelectedTags.Location = new System.Drawing.Point(370, 320);
            this.lstSelectedTags.Name = "lstSelectedTags";
            this.lstSelectedTags.Size = new System.Drawing.Size(220, 114);
            this.lstSelectedTags.Sorted = true;
            this.lstSelectedTags.TabIndex = 6;
            this.lstSelectedTags.KeyUp += new System.Windows.Forms.KeyEventHandler(this.LstSelectedTags_KeyUp);
            this.lstSelectedTags.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LstSelectedTags_MouseDoubleClick);
            // 
            // chkMakeEpic
            // 
            this.chkMakeEpic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkMakeEpic.AutoSize = true;
            this.chkMakeEpic.Font = new System.Drawing.Font("Calibri", 11.25F);
            this.chkMakeEpic.Location = new System.Drawing.Point(93, 504);
            this.chkMakeEpic.Name = "chkMakeEpic";
            this.chkMakeEpic.Size = new System.Drawing.Size(112, 22);
            this.chkMakeEpic.TabIndex = 25;
            this.chkMakeEpic.Text = "Create as Epic";
            this.chkMakeEpic.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(175, 475);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 18);
            this.label9.TabIndex = 26;
            this.label9.Text = "Milestone";
            // 
            // cboMilestones
            // 
            this.cboMilestones.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cboMilestones.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboMilestones.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboMilestones.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboMilestones.FormattingEnabled = true;
            this.cboMilestones.Location = new System.Drawing.Point(252, 472);
            this.cboMilestones.Name = "cboMilestones";
            this.cboMilestones.Size = new System.Drawing.Size(338, 26);
            this.cboMilestones.Sorted = true;
            this.cboMilestones.TabIndex = 27;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 646);
            this.Controls.Add(this.cboMilestones);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.chkMakeEpic);
            this.Controls.Add(this.txtEstimate);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnRefreshEpics);
            this.Controls.Add(this.cboEpics);
            this.Controls.Add(this.lblEpic);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.lstAvailableTags);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnCreateIssue);
            this.Controls.Add(this.cboAssignees);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnRemoveTag);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnAddTag);
            this.Controls.Add(this.lstSelectedTags);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboAvailableRepos);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtIssueTitle);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(621, 538);
            this.Name = "frmMain";
            this.Text = "Issue Creator UI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private TextBoxEx txtIssueTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboAvailableRepos;
        private System.Windows.Forms.Label label3;
        private TextBoxEx txtDescription;
        private Controls.ListBoxWithSearch lstSelectedTags;
        private System.Windows.Forms.Button btnAddTag;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnRemoveTag;
        private System.Windows.Forms.Label label5;
        private ComboBoxWithSearch cboAssignees;
        private System.Windows.Forms.Button btnCreateIssue;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private Controls.ListBoxWithSearch lstAvailableTags;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tssVersion;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private Controls.ComboBoxWithSearch cboEpics;
        private System.Windows.Forms.Label lblEpic;
        private System.Windows.Forms.ToolStripStatusLabel tssStatus;
        private Button btnRefreshEpics;
        private TextBoxEx txtEstimate;
        private Label label8;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem preferencesToolStripMenuItem;
        private CheckBox chkMakeEpic;
        private Label label9;
        private ComboBoxWithSearch cboMilestones;
    }
}

