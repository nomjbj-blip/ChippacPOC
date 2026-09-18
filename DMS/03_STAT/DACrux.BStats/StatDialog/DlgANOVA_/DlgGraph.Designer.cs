namespace DACrux.BStats.StatDialog.DlgANOVA_
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgGraph));
            this.pnlBackground = new System.Windows.Forms.Panel();
            this.chkResidualsVsFits = new System.Windows.Forms.CheckBox();
            this.chkBoxplot = new System.Windows.Forms.CheckBox();
            this.chkNormalplot = new System.Windows.Forms.CheckBox();
            this.chkHistogramofResiduals = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.imlColumnType = new System.Windows.Forms.ImageList(this.components);
            this.pnlBackground.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBackground
            // 
            this.pnlBackground.Controls.Add(this.chkResidualsVsFits);
            this.pnlBackground.Controls.Add(this.chkBoxplot);
            this.pnlBackground.Controls.Add(this.chkNormalplot);
            this.pnlBackground.Controls.Add(this.chkHistogramofResiduals);
            this.pnlBackground.Controls.Add(this.btnCancel);
            this.pnlBackground.Controls.Add(this.btnOk);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Size = new System.Drawing.Size(291, 146);
            this.pnlBackground.TabIndex = 1;
            // 
            // chkResidualsVsFits
            // 
            this.chkResidualsVsFits.AutoSize = true;
            this.chkResidualsVsFits.Checked = true;
            this.chkResidualsVsFits.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkResidualsVsFits.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkResidualsVsFits.Location = new System.Drawing.Point(12, 83);
            this.chkResidualsVsFits.Name = "chkResidualsVsFits";
            this.chkResidualsVsFits.Size = new System.Drawing.Size(121, 17);
            this.chkResidualsVsFits.TabIndex = 81;
            this.chkResidualsVsFits.Text = "Residuals versus &fits";
            this.chkResidualsVsFits.UseVisualStyleBackColor = true;
            // 
            // chkBoxplot
            // 
            this.chkBoxplot.AutoSize = true;
            this.chkBoxplot.Checked = true;
            this.chkBoxplot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxplot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkBoxplot.Location = new System.Drawing.Point(12, 12);
            this.chkBoxplot.Name = "chkBoxplot";
            this.chkBoxplot.Size = new System.Drawing.Size(102, 17);
            this.chkBoxplot.TabIndex = 85;
            this.chkBoxplot.Text = "&Boxplots of data";
            this.chkBoxplot.UseVisualStyleBackColor = true;
            // 
            // chkNormalplot
            // 
            this.chkNormalplot.AutoSize = true;
            this.chkNormalplot.Checked = true;
            this.chkNormalplot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNormalplot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkNormalplot.Location = new System.Drawing.Point(12, 59);
            this.chkNormalplot.Name = "chkNormalplot";
            this.chkNormalplot.Size = new System.Drawing.Size(135, 17);
            this.chkNormalplot.TabIndex = 83;
            this.chkNormalplot.Text = "&Normal plot of residuals";
            this.chkNormalplot.UseVisualStyleBackColor = true;
            // 
            // chkHistogramofResiduals
            // 
            this.chkHistogramofResiduals.AutoSize = true;
            this.chkHistogramofResiduals.Checked = true;
            this.chkHistogramofResiduals.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHistogramofResiduals.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkHistogramofResiduals.Location = new System.Drawing.Point(12, 35);
            this.chkHistogramofResiduals.Name = "chkHistogramofResiduals";
            this.chkHistogramofResiduals.Size = new System.Drawing.Size(129, 17);
            this.chkHistogramofResiduals.TabIndex = 80;
            this.chkHistogramofResiduals.Text = "&Histogram of residuals";
            this.chkHistogramofResiduals.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(216, 110);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(59, 22);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(152, 110);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(59, 22);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // imlColumnType
            // 
            this.imlColumnType.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlColumnType.ImageStream")));
            this.imlColumnType.TransparentColor = System.Drawing.Color.Transparent;
            this.imlColumnType.Images.SetKeyName(0, "NUMBER");
            this.imlColumnType.Images.SetKeyName(1, "TEXT");
            this.imlColumnType.Images.SetKeyName(2, "DATETIME");
            // 
            // DlgGraph
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(291, 146);
            this.Controls.Add(this.pnlBackground);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DlgGraph";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Analysis of Variance - Graphs";
            this.pnlBackground.ResumeLayout(false);
            this.pnlBackground.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBackground;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ImageList imlColumnType;
        private System.Windows.Forms.CheckBox chkBoxplot;
        private System.Windows.Forms.CheckBox chkNormalplot;
        private System.Windows.Forms.CheckBox chkHistogramofResiduals;
        private System.Windows.Forms.CheckBox chkResidualsVsFits;
    }
}