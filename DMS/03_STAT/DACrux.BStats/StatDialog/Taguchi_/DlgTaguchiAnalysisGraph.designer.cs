namespace DACrux.BStats.StatDialog.DlgTaguchi_
{
    partial class DlgTaguchiAnalysisGraph
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
            this.chkSNRatio = new System.Windows.Forms.CheckBox();
            this.chkAverage = new System.Windows.Forms.CheckBox();
            this.butOk = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.chkStdev = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // chkSNRatio
            // 
            this.chkSNRatio.AutoSize = true;
            this.chkSNRatio.Location = new System.Drawing.Point(12, 12);
            this.chkSNRatio.Name = "chkSNRatio";
            this.chkSNRatio.Size = new System.Drawing.Size(108, 16);
            this.chkSNRatio.TabIndex = 0;
            this.chkSNRatio.Text = "신호 대 잡음 비";
            this.chkSNRatio.UseVisualStyleBackColor = true;
            // 
            // chkAverage
            // 
            this.chkAverage.AutoSize = true;
            this.chkAverage.Location = new System.Drawing.Point(12, 34);
            this.chkAverage.Name = "chkAverage";
            this.chkAverage.Size = new System.Drawing.Size(48, 16);
            this.chkAverage.TabIndex = 0;
            this.chkAverage.Text = "평균";
            this.chkAverage.UseVisualStyleBackColor = true;
            // 
            // butOk
            // 
            this.butOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butOk.Location = new System.Drawing.Point(101, 114);
            this.butOk.Name = "butOk";
            this.butOk.Size = new System.Drawing.Size(93, 35);
            this.butOk.TabIndex = 1;
            this.butOk.Text = "Ok";
            this.butOk.UseVisualStyleBackColor = true;
            this.butOk.Click += new System.EventHandler(this.butOk_Click);
            // 
            // butCancel
            // 
            this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(200, 114);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(93, 35);
            this.butCancel.TabIndex = 1;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            // 
            // chkStdev
            // 
            this.chkStdev.AutoSize = true;
            this.chkStdev.Location = new System.Drawing.Point(12, 56);
            this.chkStdev.Name = "chkStdev";
            this.chkStdev.Size = new System.Drawing.Size(72, 16);
            this.chkStdev.TabIndex = 0;
            this.chkStdev.Text = "표준편차";
            this.chkStdev.UseVisualStyleBackColor = true;
            // 
            // DlgTaguchiAnalysisGraph
            // 
            this.AcceptButton = this.butOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(305, 161);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOk);
            this.Controls.Add(this.chkStdev);
            this.Controls.Add(this.chkAverage);
            this.Controls.Add(this.chkSNRatio);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgTaguchiAnalysisGraph";
            this.ShowInTaskbar = false;
            this.Text = "Taguchi 분석 - 그래프";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butOk;
        private System.Windows.Forms.Button butCancel;
        public System.Windows.Forms.CheckBox chkSNRatio;
        public System.Windows.Forms.CheckBox chkAverage;
        public System.Windows.Forms.CheckBox chkStdev;
    }
}