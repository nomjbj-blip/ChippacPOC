namespace DACruxV5
{
    partial class FrmValidation
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lstDuplicated = new System.Windows.Forms.ListBox();
            this.lblDuplicated = new System.Windows.Forms.Label();
            this.lstNotConnected = new System.Windows.Forms.ListBox();
            this.lblNotConnected = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lstDuplicated);
            this.splitContainer1.Panel1.Controls.Add(this.lblDuplicated);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lstNotConnected);
            this.splitContainer1.Panel2.Controls.Add(this.lblNotConnected);
            this.splitContainer1.Size = new System.Drawing.Size(387, 331);
            this.splitContainer1.SplitterDistance = 155;
            this.splitContainer1.TabIndex = 0;
            // 
            // lstDuplicated
            // 
            this.lstDuplicated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstDuplicated.FormattingEnabled = true;
            this.lstDuplicated.ItemHeight = 12;
            this.lstDuplicated.Location = new System.Drawing.Point(0, 20);
            this.lstDuplicated.Name = "lstDuplicated";
            this.lstDuplicated.Size = new System.Drawing.Size(387, 135);
            this.lstDuplicated.TabIndex = 1;
            // 
            // lblDuplicated
            // 
            this.lblDuplicated.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDuplicated.Location = new System.Drawing.Point(0, 0);
            this.lblDuplicated.Name = "lblDuplicated";
            this.lblDuplicated.Size = new System.Drawing.Size(387, 20);
            this.lblDuplicated.TabIndex = 0;
            this.lblDuplicated.Text = "Duplicated Items : ";
            this.lblDuplicated.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lstNotConnected
            // 
            this.lstNotConnected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstNotConnected.FormattingEnabled = true;
            this.lstNotConnected.ItemHeight = 12;
            this.lstNotConnected.Location = new System.Drawing.Point(0, 20);
            this.lstNotConnected.Name = "lstNotConnected";
            this.lstNotConnected.Size = new System.Drawing.Size(387, 152);
            this.lstNotConnected.TabIndex = 2;
            // 
            // lblNotConnected
            // 
            this.lblNotConnected.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblNotConnected.Location = new System.Drawing.Point(0, 0);
            this.lblNotConnected.Name = "lblNotConnected";
            this.lblNotConnected.Size = new System.Drawing.Size(387, 20);
            this.lblNotConnected.TabIndex = 1;
            this.lblNotConnected.Text = "Not connected Items : ";
            this.lblNotConnected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 331);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(387, 36);
            this.panel1.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(300, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FrmValidation
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(387, 367);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmValidation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Validation";
            this.Load += new System.EventHandler(this.FrmValidation_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label lblDuplicated;
        private System.Windows.Forms.Label lblNotConnected;
        private System.Windows.Forms.ListBox lstDuplicated;
        private System.Windows.Forms.ListBox lstNotConnected;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
    }
}