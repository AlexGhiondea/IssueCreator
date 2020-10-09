namespace IssueCreator.Dialogs
{
    partial class BulkCreateIssues
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
            this.btnBrowseEpic = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dgBulkIssues = new System.Windows.Forms.DataGridView();
            this.organizationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.repositoryDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.titleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assignedToDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Milestone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.estimateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.issueToCreateBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnLoadData = new System.Windows.Forms.Button();
            this.cboEpics = new IssueCreator.Controls.ComboBoxWithSearch();
            this.btnCreateIssues = new System.Windows.Forms.Button();
            this.LabelsCollection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgBulkIssues)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.issueToCreateBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBrowseEpic
            // 
            this.btnBrowseEpic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseEpic.Font = new System.Drawing.Font("Calibri", 12F);
            this.btnBrowseEpic.Location = new System.Drawing.Point(910, 11);
            this.btnBrowseEpic.Name = "btnBrowseEpic";
            this.btnBrowseEpic.Size = new System.Drawing.Size(70, 30);
            this.btnBrowseEpic.TabIndex = 13;
            this.btnBrowseEpic.Text = "Browse";
            this.btnBrowseEpic.UseVisualStyleBackColor = true;
            this.btnBrowseEpic.Click += new System.EventHandler(this.btnBrowseEpic_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 12F);
            this.label1.Location = new System.Drawing.Point(13, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 19);
            this.label1.TabIndex = 11;
            this.label1.Text = "Epic";
            // 
            // dgBulkIssues
            // 
            this.dgBulkIssues.AllowUserToAddRows = false;
            this.dgBulkIssues.AllowUserToDeleteRows = false;
            this.dgBulkIssues.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgBulkIssues.AutoGenerateColumns = false;
            this.dgBulkIssues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgBulkIssues.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.organizationDataGridViewTextBoxColumn,
            this.repositoryDataGridViewTextBoxColumn,
            this.titleDataGridViewTextBoxColumn,
            this.assignedToDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.Milestone,
            this.estimateDataGridViewTextBoxColumn,
            this.LabelsCollection});
            this.dgBulkIssues.DataSource = this.issueToCreateBindingSource;
            this.dgBulkIssues.Location = new System.Drawing.Point(12, 47);
            this.dgBulkIssues.Name = "dgBulkIssues";
            this.dgBulkIssues.ReadOnly = true;
            this.dgBulkIssues.Size = new System.Drawing.Size(971, 314);
            this.dgBulkIssues.TabIndex = 14;
            // 
            // organizationDataGridViewTextBoxColumn
            // 
            this.organizationDataGridViewTextBoxColumn.DataPropertyName = "Organization";
            this.organizationDataGridViewTextBoxColumn.HeaderText = "Organization";
            this.organizationDataGridViewTextBoxColumn.Name = "organizationDataGridViewTextBoxColumn";
            this.organizationDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // repositoryDataGridViewTextBoxColumn
            // 
            this.repositoryDataGridViewTextBoxColumn.DataPropertyName = "Repository";
            this.repositoryDataGridViewTextBoxColumn.HeaderText = "Repository";
            this.repositoryDataGridViewTextBoxColumn.Name = "repositoryDataGridViewTextBoxColumn";
            this.repositoryDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // titleDataGridViewTextBoxColumn
            // 
            this.titleDataGridViewTextBoxColumn.DataPropertyName = "Title";
            this.titleDataGridViewTextBoxColumn.HeaderText = "Title";
            this.titleDataGridViewTextBoxColumn.Name = "titleDataGridViewTextBoxColumn";
            this.titleDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // assignedToDataGridViewTextBoxColumn
            // 
            this.assignedToDataGridViewTextBoxColumn.DataPropertyName = "AssignedTo";
            this.assignedToDataGridViewTextBoxColumn.HeaderText = "AssignedTo";
            this.assignedToDataGridViewTextBoxColumn.Name = "assignedToDataGridViewTextBoxColumn";
            this.assignedToDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // Milestone
            // 
            this.Milestone.DataPropertyName = "Milestone";
            this.Milestone.HeaderText = "Milestone";
            this.Milestone.Name = "Milestone";
            this.Milestone.ReadOnly = true;
            // 
            // estimateDataGridViewTextBoxColumn
            // 
            this.estimateDataGridViewTextBoxColumn.DataPropertyName = "Estimate";
            this.estimateDataGridViewTextBoxColumn.HeaderText = "Estimate";
            this.estimateDataGridViewTextBoxColumn.Name = "estimateDataGridViewTextBoxColumn";
            this.estimateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // issueToCreateBindingSource
            // 
            this.issueToCreateBindingSource.DataSource = typeof(IssueCreator.Models.IssueToCreate);
            // 
            // btnLoadData
            // 
            this.btnLoadData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoadData.Font = new System.Drawing.Font("Calibri", 12F);
            this.btnLoadData.Location = new System.Drawing.Point(12, 370);
            this.btnLoadData.Name = "btnLoadData";
            this.btnLoadData.Size = new System.Drawing.Size(126, 30);
            this.btnLoadData.TabIndex = 15;
            this.btnLoadData.Text = "Load Issues";
            this.btnLoadData.UseVisualStyleBackColor = true;
            this.btnLoadData.Click += new System.EventHandler(this.btnLoadData_Click);
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
            this.cboEpics.Location = new System.Drawing.Point(55, 12);
            this.cboEpics.Name = "cboEpics";
            this.cboEpics.Size = new System.Drawing.Size(849, 26);
            this.cboEpics.Sorted = true;
            this.cboEpics.TabIndex = 12;
            // 
            // btnCreateIssues
            // 
            this.btnCreateIssues.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateIssues.Font = new System.Drawing.Font("Calibri", 12F);
            this.btnCreateIssues.Location = new System.Drawing.Point(854, 370);
            this.btnCreateIssues.Name = "btnCreateIssues";
            this.btnCreateIssues.Size = new System.Drawing.Size(126, 30);
            this.btnCreateIssues.TabIndex = 16;
            this.btnCreateIssues.Text = "Create Issues";
            this.btnCreateIssues.UseVisualStyleBackColor = true;
            this.btnCreateIssues.Click += new System.EventHandler(this.btnCreateIssues_Click);
            // 
            // LabelsCollection
            // 
            this.LabelsCollection.DataPropertyName = "LabelsCollection";
            this.LabelsCollection.HeaderText = "Labels";
            this.LabelsCollection.Name = "LabelsCollection";
            this.LabelsCollection.ReadOnly = true;
            // 
            // BulkCreateIssues
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(995, 412);
            this.Controls.Add(this.btnCreateIssues);
            this.Controls.Add(this.btnLoadData);
            this.Controls.Add(this.dgBulkIssues);
            this.Controls.Add(this.btnBrowseEpic);
            this.Controls.Add(this.cboEpics);
            this.Controls.Add(this.label1);
            this.Name = "BulkCreateIssues";
            this.Text = "BulkCreateIssues";
            ((System.ComponentModel.ISupportInitialize)(this.dgBulkIssues)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.issueToCreateBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Button btnBrowseEpic;
        protected Controls.ComboBoxWithSearch cboEpics;
        protected System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgBulkIssues;
        private System.Windows.Forms.BindingSource issueToCreateBindingSource;
        protected System.Windows.Forms.Button btnLoadData;
        private System.Windows.Forms.DataGridViewTextBoxColumn organizationDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn repositoryDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn titleDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn assignedToDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Milestone;
        private System.Windows.Forms.DataGridViewTextBoxColumn estimateDataGridViewTextBoxColumn;
        protected System.Windows.Forms.Button btnCreateIssues;
        private System.Windows.Forms.DataGridViewTextBoxColumn LabelsCollection;
    }
}