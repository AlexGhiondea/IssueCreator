namespace IssueCreator.Dialogs
{
    partial class BulkCreateEpicTree
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
            this.dgBulkIssues = new System.Windows.Forms.DataGridView();
            this.btnLoadData = new System.Windows.Forms.Button();
            this.btnCreateIssues = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgBulkIssues)).BeginInit();
            this.SuspendLayout();
            // 
            // dgBulkIssues
            // 
            this.dgBulkIssues.AllowUserToAddRows = false;
            this.dgBulkIssues.AllowUserToDeleteRows = false;
            this.dgBulkIssues.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgBulkIssues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgBulkIssues.Location = new System.Drawing.Point(12, 12);
            this.dgBulkIssues.Name = "dgBulkIssues";
            this.dgBulkIssues.ReadOnly = true;
            this.dgBulkIssues.Size = new System.Drawing.Size(1500, 261);
            this.dgBulkIssues.TabIndex = 14;
            // 
            // btnLoadData
            // 
            this.btnLoadData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoadData.Font = new System.Drawing.Font("Calibri", 12F);
            this.btnLoadData.Location = new System.Drawing.Point(12, 279);
            this.btnLoadData.Name = "btnLoadData";
            this.btnLoadData.Size = new System.Drawing.Size(126, 30);
            this.btnLoadData.TabIndex = 15;
            this.btnLoadData.Text = "Load Issues";
            this.btnLoadData.UseVisualStyleBackColor = true;
            this.btnLoadData.Click += new System.EventHandler(this.btnLoadData_Click);
            // 
            // btnCreateIssues
            // 
            this.btnCreateIssues.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateIssues.Font = new System.Drawing.Font("Calibri", 12F);
            this.btnCreateIssues.Location = new System.Drawing.Point(1386, 279);
            this.btnCreateIssues.Name = "btnCreateIssues";
            this.btnCreateIssues.Size = new System.Drawing.Size(126, 30);
            this.btnCreateIssues.TabIndex = 16;
            this.btnCreateIssues.Text = "Create Issues";
            this.btnCreateIssues.UseVisualStyleBackColor = true;
            this.btnCreateIssues.Click += new System.EventHandler(this.btnCreateIssues_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(13, 316);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(1499, 234);
            this.textBox1.TabIndex = 17;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Font = new System.Drawing.Font("Calibri", 12F);
            this.button1.Location = new System.Drawing.Point(1254, 280);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(126, 30);
            this.button1.TabIndex = 18;
            this.button1.Text = "Validate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // BulkCreateEpicTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1524, 562);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnCreateIssues);
            this.Controls.Add(this.btnLoadData);
            this.Controls.Add(this.dgBulkIssues);
            this.Name = "BulkCreateEpicTree";
            this.Text = "BulkCreateEpicTree";
            ((System.ComponentModel.ISupportInitialize)(this.dgBulkIssues)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dgBulkIssues;
        protected System.Windows.Forms.Button btnLoadData;
        protected System.Windows.Forms.Button btnCreateIssues;
        private System.Windows.Forms.TextBox textBox1;
        protected System.Windows.Forms.Button button1;
    }
}
