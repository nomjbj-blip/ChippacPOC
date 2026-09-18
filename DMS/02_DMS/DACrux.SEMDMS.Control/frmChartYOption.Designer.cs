namespace DACrux.SEMDMS.Control
{
    partial class frmChartYOption
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.chkNew = new System.Windows.Forms.CheckBox();
            this.chkCarryOver = new System.Windows.Forms.CheckBox();
            this.chkRandom = new System.Windows.Forms.CheckBox();
            this.chkCluster = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 61);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(216, 54);
            this.panel1.TabIndex = 7;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(129, 19);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(48, 19);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // chkNew
            // 
            this.chkNew.AutoSize = true;
            this.chkNew.Checked = true;
            this.chkNew.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNew.Location = new System.Drawing.Point(12, 12);
            this.chkNew.Name = "chkNew";
            this.chkNew.Size = new System.Drawing.Size(50, 16);
            this.chkNew.TabIndex = 8;
            this.chkNew.Text = "New";
            this.chkNew.UseVisualStyleBackColor = true;
            // 
            // chkCarryOver
            // 
            this.chkCarryOver.AutoSize = true;
            this.chkCarryOver.Checked = true;
            this.chkCarryOver.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCarryOver.Location = new System.Drawing.Point(12, 34);
            this.chkCarryOver.Name = "chkCarryOver";
            this.chkCarryOver.Size = new System.Drawing.Size(85, 16);
            this.chkCarryOver.TabIndex = 9;
            this.chkCarryOver.Text = "Carry Over";
            this.chkCarryOver.UseVisualStyleBackColor = true;
            // 
            // chkRandom
            // 
            this.chkRandom.AutoSize = true;
            this.chkRandom.Checked = true;
            this.chkRandom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRandom.Location = new System.Drawing.Point(117, 34);
            this.chkRandom.Name = "chkRandom";
            this.chkRandom.Size = new System.Drawing.Size(71, 16);
            this.chkRandom.TabIndex = 11;
            this.chkRandom.Text = "Random";
            this.chkRandom.UseVisualStyleBackColor = true;
            // 
            // chkCluster
            // 
            this.chkCluster.AutoSize = true;
            this.chkCluster.Checked = true;
            this.chkCluster.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCluster.Location = new System.Drawing.Point(117, 12);
            this.chkCluster.Name = "chkCluster";
            this.chkCluster.Size = new System.Drawing.Size(64, 16);
            this.chkCluster.TabIndex = 10;
            this.chkCluster.Text = "Cluster";
            this.chkCluster.UseVisualStyleBackColor = true;
            // 
            // frmChartYOption
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(216, 115);
            this.Controls.Add(this.chkRandom);
            this.Controls.Add(this.chkCluster);
            this.Controls.Add(this.chkCarryOver);
            this.Controls.Add(this.chkNew);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmChartYOption";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Setup by defect type";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.CheckBox chkNew;
        private System.Windows.Forms.CheckBox chkCarryOver;
        private System.Windows.Forms.CheckBox chkRandom;
        private System.Windows.Forms.CheckBox chkCluster;
    }
}