namespace DACrux.SEMDMS.ENGUI
{
    partial class frmZipTest
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
            this.btnMakeZip = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLot = new System.Windows.Forms.TextBox();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPath = new System.Windows.Forms.Button();
            this.chkLastInspection = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnMakeZip
            // 
            this.btnMakeZip.Location = new System.Drawing.Point(195, 55);
            this.btnMakeZip.Name = "btnMakeZip";
            this.btnMakeZip.Size = new System.Drawing.Size(75, 23);
            this.btnMakeZip.TabIndex = 5;
            this.btnMakeZip.Text = "Make Zip";
            this.btnMakeZip.UseVisualStyleBackColor = true;
            this.btnMakeZip.Click += new System.EventHandler(this.btnMakeZip_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "Lot ID :";
            // 
            // txtLot
            // 
            this.txtLot.Location = new System.Drawing.Point(89, 55);
            this.txtLot.Name = "txtLot";
            this.txtLot.Size = new System.Drawing.Size(100, 21);
            this.txtLot.TabIndex = 4;
            // 
            // txtPath
            // 
            this.txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPath.Location = new System.Drawing.Point(89, 28);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(495, 21);
            this.txtPath.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Save Path :";
            // 
            // btnPath
            // 
            this.btnPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPath.Location = new System.Drawing.Point(590, 26);
            this.btnPath.Name = "btnPath";
            this.btnPath.Size = new System.Drawing.Size(37, 23);
            this.btnPath.TabIndex = 2;
            this.btnPath.Text = "...";
            this.btnPath.UseVisualStyleBackColor = true;
            this.btnPath.Click += new System.EventHandler(this.btnPath_Click);
            // 
            // chkLastInspection
            // 
            this.chkLastInspection.AutoSize = true;
            this.chkLastInspection.Location = new System.Drawing.Point(89, 6);
            this.chkLastInspection.Name = "chkLastInspection";
            this.chkLastInspection.Size = new System.Drawing.Size(110, 16);
            this.chkLastInspection.TabIndex = 6;
            this.chkLastInspection.Text = "Last Inspection";
            this.chkLastInspection.UseVisualStyleBackColor = true;
            // 
            // frmZipTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 318);
            this.Controls.Add(this.chkLastInspection);
            this.Controls.Add(this.btnPath);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtLot);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnMakeZip);
            this.Name = "frmZipTest";
            this.Text = "frmZipTest";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMakeZip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLot;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnPath;
        private System.Windows.Forms.CheckBox chkLastInspection;
    }
}