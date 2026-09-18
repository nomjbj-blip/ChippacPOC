namespace DACrux.BStats.StatDialog.DlgCapaContinuous_
{
    partial class DlgEstimationofStandardDeviation
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
            this.chkUseUnbiasingConstantsOverall = new System.Windows.Forms.CheckBox();
            this.grpBetweenSubgroups = new System.Windows.Forms.GroupBox();
            this.nudMovingRange = new System.Windows.Forms.NumericUpDown();
            this.rdoSquarerootOfMSSD = new System.Windows.Forms.RadioButton();
            this.rdoMedianMovingRange = new System.Windows.Forms.RadioButton();
            this.rdoAverageMovingRange = new System.Windows.Forms.RadioButton();
            this.lblUseMovingRangeofLength = new System.Windows.Forms.Label();
            this.grpWithinSubgroup = new System.Windows.Forms.GroupBox();
            this.chkUseUnbiasingConstants = new System.Windows.Forms.CheckBox();
            this.rdoPooledStd = new System.Windows.Forms.RadioButton();
            this.rdoSbar = new System.Windows.Forms.RadioButton();
            this.rdoRbar = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblMethodsOfEstimatingStandarddeviation = new System.Windows.Forms.Label();
            this.pnlBackground.SuspendLayout();
            this.grpBetweenSubgroups.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMovingRange)).BeginInit();
            this.grpWithinSubgroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBackground
            // 
            this.pnlBackground.Controls.Add(this.chkUseUnbiasingConstantsOverall);
            this.pnlBackground.Controls.Add(this.grpBetweenSubgroups);
            this.pnlBackground.Controls.Add(this.grpWithinSubgroup);
            this.pnlBackground.Controls.Add(this.btnCancel);
            this.pnlBackground.Controls.Add(this.btnOk);
            this.pnlBackground.Controls.Add(this.lblMethodsOfEstimatingStandarddeviation);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Size = new System.Drawing.Size(357, 333);
            this.pnlBackground.TabIndex = 1;
            // 
            // chkUseUnbiasingConstantsOverall
            // 
            this.chkUseUnbiasingConstantsOverall.AutoSize = true;
            this.chkUseUnbiasingConstantsOverall.Checked = true;
            this.chkUseUnbiasingConstantsOverall.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseUnbiasingConstantsOverall.Location = new System.Drawing.Point(22, 278);
            this.chkUseUnbiasingConstantsOverall.Name = "chkUseUnbiasingConstantsOverall";
            this.chkUseUnbiasingConstantsOverall.Size = new System.Drawing.Size(328, 17);
            this.chkUseUnbiasingConstantsOverall.TabIndex = 42;
            this.chkUseUnbiasingConstantsOverall.Text = "Use unbiasing constants to calculate ov&erall standard deviation";
            this.chkUseUnbiasingConstantsOverall.UseVisualStyleBackColor = true;
            // 
            // grpBetweenSubgroups
            // 
            this.grpBetweenSubgroups.Controls.Add(this.nudMovingRange);
            this.grpBetweenSubgroups.Controls.Add(this.rdoSquarerootOfMSSD);
            this.grpBetweenSubgroups.Controls.Add(this.rdoMedianMovingRange);
            this.grpBetweenSubgroups.Controls.Add(this.rdoAverageMovingRange);
            this.grpBetweenSubgroups.Controls.Add(this.lblUseMovingRangeofLength);
            this.grpBetweenSubgroups.Location = new System.Drawing.Point(22, 155);
            this.grpBetweenSubgroups.Name = "grpBetweenSubgroups";
            this.grpBetweenSubgroups.Size = new System.Drawing.Size(319, 117);
            this.grpBetweenSubgroups.TabIndex = 40;
            this.grpBetweenSubgroups.TabStop = false;
            this.grpBetweenSubgroups.Text = "Between subgroups";
            // 
            // nudMovingRange
            // 
            this.nudMovingRange.Location = new System.Drawing.Point(164, 23);
            this.nudMovingRange.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudMovingRange.Name = "nudMovingRange";
            this.nudMovingRange.Size = new System.Drawing.Size(55, 21);
            this.nudMovingRange.TabIndex = 28;
            this.nudMovingRange.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // rdoSquarerootOfMSSD
            // 
            this.rdoSquarerootOfMSSD.AutoSize = true;
            this.rdoSquarerootOfMSSD.Location = new System.Drawing.Point(15, 91);
            this.rdoSquarerootOfMSSD.Name = "rdoSquarerootOfMSSD";
            this.rdoSquarerootOfMSSD.Size = new System.Drawing.Size(141, 16);
            this.rdoSquarerootOfMSSD.TabIndex = 3;
            this.rdoSquarerootOfMSSD.Text = "S&quare root of MSSD";
            this.rdoSquarerootOfMSSD.UseVisualStyleBackColor = true;
            // 
            // rdoMedianMovingRange
            // 
            this.rdoMedianMovingRange.AutoSize = true;
            this.rdoMedianMovingRange.Location = new System.Drawing.Point(15, 68);
            this.rdoMedianMovingRange.Name = "rdoMedianMovingRange";
            this.rdoMedianMovingRange.Size = new System.Drawing.Size(146, 16);
            this.rdoMedianMovingRange.TabIndex = 2;
            this.rdoMedianMovingRange.Text = "Me&dian moving range";
            this.rdoMedianMovingRange.UseVisualStyleBackColor = true;
            // 
            // rdoAverageMovingRange
            // 
            this.rdoAverageMovingRange.AutoSize = true;
            this.rdoAverageMovingRange.Checked = true;
            this.rdoAverageMovingRange.Location = new System.Drawing.Point(15, 45);
            this.rdoAverageMovingRange.Name = "rdoAverageMovingRange";
            this.rdoAverageMovingRange.Size = new System.Drawing.Size(150, 16);
            this.rdoAverageMovingRange.TabIndex = 1;
            this.rdoAverageMovingRange.TabStop = true;
            this.rdoAverageMovingRange.Text = "&Average moving range";
            this.rdoAverageMovingRange.UseVisualStyleBackColor = true;
            // 
            // lblUseMovingRangeofLength
            // 
            this.lblUseMovingRangeofLength.AutoSize = true;
            this.lblUseMovingRangeofLength.Location = new System.Drawing.Point(12, 25);
            this.lblUseMovingRangeofLength.Name = "lblUseMovingRangeofLength";
            this.lblUseMovingRangeofLength.Size = new System.Drawing.Size(139, 13);
            this.lblUseMovingRangeofLength.TabIndex = 27;
            this.lblUseMovingRangeofLength.Text = "Use mo&ving range of length";
            // 
            // grpWithinSubgroup
            // 
            this.grpWithinSubgroup.Controls.Add(this.chkUseUnbiasingConstants);
            this.grpWithinSubgroup.Controls.Add(this.rdoPooledStd);
            this.grpWithinSubgroup.Controls.Add(this.rdoSbar);
            this.grpWithinSubgroup.Controls.Add(this.rdoRbar);
            this.grpWithinSubgroup.Location = new System.Drawing.Point(22, 41);
            this.grpWithinSubgroup.Name = "grpWithinSubgroup";
            this.grpWithinSubgroup.Size = new System.Drawing.Size(319, 108);
            this.grpWithinSubgroup.TabIndex = 39;
            this.grpWithinSubgroup.TabStop = false;
            this.grpWithinSubgroup.Text = "Within subgroup";
            // 
            // chkUseUnbiasingConstants
            // 
            this.chkUseUnbiasingConstants.AutoSize = true;
            this.chkUseUnbiasingConstants.Checked = true;
            this.chkUseUnbiasingConstants.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseUnbiasingConstants.Location = new System.Drawing.Point(15, 17);
            this.chkUseUnbiasingConstants.Name = "chkUseUnbiasingConstants";
            this.chkUseUnbiasingConstants.Size = new System.Drawing.Size(164, 16);
            this.chkUseUnbiasingConstants.TabIndex = 41;
            this.chkUseUnbiasingConstants.Text = "&Use unbiasing constants";
            this.chkUseUnbiasingConstants.UseVisualStyleBackColor = true;
            // 
            // rdoPooledStd
            // 
            this.rdoPooledStd.AutoSize = true;
            this.rdoPooledStd.Checked = true;
            this.rdoPooledStd.Location = new System.Drawing.Point(15, 82);
            this.rdoPooledStd.Name = "rdoPooledStd";
            this.rdoPooledStd.Size = new System.Drawing.Size(169, 16);
            this.rdoPooledStd.TabIndex = 2;
            this.rdoPooledStd.TabStop = true;
            this.rdoPooledStd.Text = "&Pooled standard deviation";
            this.rdoPooledStd.UseVisualStyleBackColor = true;
            // 
            // rdoSbar
            // 
            this.rdoSbar.AutoSize = true;
            this.rdoSbar.Location = new System.Drawing.Point(15, 59);
            this.rdoSbar.Name = "rdoSbar";
            this.rdoSbar.Size = new System.Drawing.Size(49, 16);
            this.rdoSbar.TabIndex = 1;
            this.rdoSbar.Text = "&Sbar";
            this.rdoSbar.UseVisualStyleBackColor = true;
            // 
            // rdoRbar
            // 
            this.rdoRbar.AutoSize = true;
            this.rdoRbar.Location = new System.Drawing.Point(15, 36);
            this.rdoRbar.Name = "rdoRbar";
            this.rdoRbar.Size = new System.Drawing.Size(49, 16);
            this.rdoRbar.TabIndex = 0;
            this.rdoRbar.Text = "&Rbar";
            this.rdoRbar.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(269, 303);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 21);
            this.btnCancel.TabIndex = 38;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(192, 303);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(69, 21);
            this.btnOk.TabIndex = 37;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // lblMethodsOfEstimatingStandarddeviation
            // 
            this.lblMethodsOfEstimatingStandarddeviation.AutoSize = true;
            this.lblMethodsOfEstimatingStandarddeviation.Location = new System.Drawing.Point(19, 12);
            this.lblMethodsOfEstimatingStandarddeviation.Name = "lblMethodsOfEstimatingStandarddeviation";
            this.lblMethodsOfEstimatingStandarddeviation.Size = new System.Drawing.Size(206, 13);
            this.lblMethodsOfEstimatingStandarddeviation.TabIndex = 28;
            this.lblMethodsOfEstimatingStandarddeviation.Text = "Methods of estimating standard deviation";
            // 
            // DlgEstimationofStandardDeviation
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(357, 333);
            this.Controls.Add(this.pnlBackground);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DlgEstimationofStandardDeviation";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Estimation of Standard Deviation";
            this.pnlBackground.ResumeLayout(false);
            this.pnlBackground.PerformLayout();
            this.grpBetweenSubgroups.ResumeLayout(false);
            this.grpBetweenSubgroups.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMovingRange)).EndInit();
            this.grpWithinSubgroup.ResumeLayout(false);
            this.grpWithinSubgroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBackground;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblMethodsOfEstimatingStandarddeviation;
        private System.Windows.Forms.CheckBox chkUseUnbiasingConstants;
        private System.Windows.Forms.GroupBox grpBetweenSubgroups;
        private System.Windows.Forms.RadioButton rdoSquarerootOfMSSD;
        private System.Windows.Forms.RadioButton rdoMedianMovingRange;
        private System.Windows.Forms.RadioButton rdoAverageMovingRange;
        private System.Windows.Forms.Label lblUseMovingRangeofLength;
        private System.Windows.Forms.GroupBox grpWithinSubgroup;
        private System.Windows.Forms.RadioButton rdoPooledStd;
        private System.Windows.Forms.RadioButton rdoSbar;
        private System.Windows.Forms.RadioButton rdoRbar;
        private System.Windows.Forms.CheckBox chkUseUnbiasingConstantsOverall;
        private System.Windows.Forms.NumericUpDown nudMovingRange;
    }
}