namespace IssueCreator.Dialogs
{
    partial class IssueTemplates
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
            this.label4 = new System.Windows.Forms.Label();
            this.btnLoad = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(64, 313);
            this.label1.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(598, 409);
            // 
            // cboEpics
            // 
            this.cboEpics.Location = new System.Drawing.Point(106, 308);
            this.cboEpics.Visible = false;
            // 
            // btnBrowseEpic
            // 
            this.btnBrowseEpic.Location = new System.Drawing.Point(598, 307);
            this.btnBrowseEpic.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(106, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(310, 24);
            this.label4.TabIndex = 12;
            this.label4.Text = "Load an existing issue as a template";
            // 
            // btnLoad
            // 
            this.btnLoad.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnLoad.Font = new System.Drawing.Font("Calibri", 12F);
            this.btnLoad.Location = new System.Drawing.Point(598, 109);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 35);
            this.btnLoad.TabIndex = 13;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.loadIssue_Click);
            // 
            // IssueTemplates
            // 
            this.AcceptButton = this.btnLoad;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(686, 156);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.label4);
            this.Name = "IssueTemplates";
            this.Text = "Issue Templates";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtIssueNumber, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.cboAvailableRepos, 0);
            this.Controls.SetChildIndex(this.cboEpics, 0);
            this.Controls.SetChildIndex(this.btnBrowseEpic, 0);
            this.Controls.SetChildIndex(this.lblIssueTitle, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.btnLoad, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnLoad;
    }
}
