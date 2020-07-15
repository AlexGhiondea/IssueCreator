namespace IssueCreator.Dialogs
{
    partial class EpicAssociation
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
            this.btnDisassociateEpic = new System.Windows.Forms.Button();
            this.btnAssociateEpic = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(597, 109);
            // 
            // cboAvailableRepos
            // 
            this.cboAvailableRepos.Size = new System.Drawing.Size(567, 31);
            // 
            // cboEpics
            // 
            this.cboEpics.Size = new System.Drawing.Size(485, 31);
            // 
            // btnBrowseEpic
            // 
            this.btnBrowseEpic.Location = new System.Drawing.Point(597, 7);
            // 
            // btnDisassociateEpic
            // 
            this.btnDisassociateEpic.Font = new System.Drawing.Font("Calibri", 12F);
            this.btnDisassociateEpic.Location = new System.Drawing.Point(211, 110);
            this.btnDisassociateEpic.Name = "btnDisassociateEpic";
            this.btnDisassociateEpic.Size = new System.Drawing.Size(99, 30);
            this.btnDisassociateEpic.TabIndex = 13;
            this.btnDisassociateEpic.Text = "Remove";
            this.btnDisassociateEpic.UseVisualStyleBackColor = true;
            this.btnDisassociateEpic.Click += new System.EventHandler(this.btnDisassociateEpic_Click);
            // 
            // btnAssociateEpic
            // 
            this.btnAssociateEpic.Font = new System.Drawing.Font("Calibri", 12F);
            this.btnAssociateEpic.Location = new System.Drawing.Point(106, 110);
            this.btnAssociateEpic.Name = "btnAssociateEpic";
            this.btnAssociateEpic.Size = new System.Drawing.Size(99, 30);
            this.btnAssociateEpic.TabIndex = 12;
            this.btnAssociateEpic.Text = "Add";
            this.btnAssociateEpic.UseVisualStyleBackColor = true;
            this.btnAssociateEpic.Click += new System.EventHandler(this.btnAssociateEpic_Click);
            // 
            // EpicAssociation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(685, 155);
            this.Controls.Add(this.btnDisassociateEpic);
            this.Controls.Add(this.btnAssociateEpic);
            this.Name = "EpicAssociation";
            this.Text = "Associate Epic";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtIssueNumber, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.cboAvailableRepos, 0);
            this.Controls.SetChildIndex(this.cboEpics, 0);
            this.Controls.SetChildIndex(this.btnBrowseEpic, 0);
            this.Controls.SetChildIndex(this.lblIssueTitle, 0);
            this.Controls.SetChildIndex(this.btnAssociateEpic, 0);
            this.Controls.SetChildIndex(this.btnDisassociateEpic, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Button btnDisassociateEpic;
        protected System.Windows.Forms.Button btnAssociateEpic;
    }
}
