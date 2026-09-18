namespace DACrux.BStats.StatDialog.DlgDescriptiveAnalysis_
{
    partial class DlgGraph
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlBackground = new System.Windows.Forms.Panel();
            this.chkRawDataPlot = new System.Windows.Forms.CheckBox();
            this.chkBoxplot = new System.Windows.Forms.CheckBox();
            this.chkNormalNHistogram = new System.Windows.Forms.CheckBox();
            this.chkHistogram = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.pnlBackground.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBackground
            // 
            this.pnlBackground.Controls.Add(this.chkRawDataPlot);
            this.pnlBackground.Controls.Add(this.chkBoxplot);
            this.pnlBackground.Controls.Add(this.chkNormalNHistogram);
            this.pnlBackground.Controls.Add(this.chkHistogram);
            this.pnlBackground.Controls.Add(this.btnCancel);
            this.pnlBackground.Controls.Add(this.btnOk);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Size = new System.Drawing.Size(283, 151);
            this.pnlBackground.TabIndex = 0;
            // 
            // chkRawDataPlot
            // 
            this.chkRawDataPlot.AutoSize = true;
            this.chkRawDataPlot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkRawDataPlot.Location = new System.Drawing.Point(12, 81);
            this.chkRawDataPlot.Name = "chkRawDataPlot";
            this.chkRawDataPlot.Size = new System.Drawing.Size(91, 17);
            this.chkRawDataPlot.TabIndex = 30;
            this.chkRawDataPlot.Text = "&Raw Data Plot";
            this.chkRawDataPlot.UseVisualStyleBackColor = true;
            // 
            // chkBoxplot
            // 
            this.chkBoxplot.AutoSize = true;
            this.chkBoxplot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkBoxplot.Location = new System.Drawing.Point(12, 58);
            this.chkBoxplot.Name = "chkBoxplot";
            this.chkBoxplot.Size = new System.Drawing.Size(97, 17);
            this.chkBoxplot.TabIndex = 30;
            this.chkBoxplot.Text = "&Boxplot of data";
            this.chkBoxplot.UseVisualStyleBackColor = true;
            // 
            // chkNormalNHistogram
            // 
            this.chkNormalNHistogram.AutoSize = true;
            this.chkNormalNHistogram.Enabled = false;
            this.chkNormalNHistogram.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkNormalNHistogram.Location = new System.Drawing.Point(28, 31);
            this.chkNormalNHistogram.Name = "chkNormalNHistogram";
            this.chkNormalNHistogram.Size = new System.Drawing.Size(201, 17);
            this.chkNormalNHistogram.TabIndex = 29;
            this.chkNormalNHistogram.Text = "Histogram of data, with &normal curve";
            this.chkNormalNHistogram.UseVisualStyleBackColor = true;
            // 
            // chkHistogram
            // 
            this.chkHistogram.AutoSize = true;
            this.chkHistogram.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkHistogram.Location = new System.Drawing.Point(12, 11);
            this.chkHistogram.Name = "chkHistogram";
            this.chkHistogram.Size = new System.Drawing.Size(109, 17);
            this.chkHistogram.TabIndex = 28;
            this.chkHistogram.Text = "&Histogram of data";
            this.chkHistogram.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(195, 117);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 21);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(120, 117);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(69, 21);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // DlgGraph
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(283, 151);
            this.Controls.Add(this.pnlBackground);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DlgGraph";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.pnlBackground.ResumeLayout(false);
            this.pnlBackground.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBackground;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.CheckBox chkHistogram;
        private System.Windows.Forms.CheckBox chkBoxplot;
        private System.Windows.Forms.CheckBox chkNormalNHistogram;
        private System.Windows.Forms.CheckBox chkRawDataPlot;
    }
}