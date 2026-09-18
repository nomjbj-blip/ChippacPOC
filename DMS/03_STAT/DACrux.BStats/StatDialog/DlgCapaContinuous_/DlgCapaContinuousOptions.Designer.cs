namespace DACrux.BStats.StatDialog.DlgCapaContinuous_
{
    partial class DlgCapaContinuousOptions
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
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlConfidenceIntervals = new System.Windows.Forms.Panel();
            this.cboConfidenceIntervals = new System.Windows.Forms.ComboBox();
            this.txtConfidenceLevel = new System.Windows.Forms.TextBox();
            this.lblConfidenceIntervals = new System.Windows.Forms.Label();
            this.lblConfidenceLevel = new System.Windows.Forms.Label();
            this.chkIncludeConfidenceIntervals = new System.Windows.Forms.CheckBox();
            this.pnlStatistics = new System.Windows.Forms.Panel();
            this.rdoBenchmarkZ = new System.Windows.Forms.RadioButton();
            this.rdoCapabilityStats = new System.Windows.Forms.RadioButton();
            this.pnlUnit = new System.Windows.Forms.Panel();
            this.rdoPercents = new System.Windows.Forms.RadioButton();
            this.rdoPartsPerMillion = new System.Windows.Forms.RadioButton();
            this.lblDisplay = new System.Windows.Forms.Label();
            this.chkOverallAnalysis = new System.Windows.Forms.CheckBox();
            this.chkBetweenWithinAnalysis = new System.Windows.Forms.CheckBox();
            this.lblPerformAnalysis = new System.Windows.Forms.Label();
            this.txtK = new System.Windows.Forms.TextBox();
            this.txtTarget = new System.Windows.Forms.TextBox();
            this.lblK = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblTarget = new System.Windows.Forms.Label();
            this.pnlBackground.SuspendLayout();
            this.pnlConfidenceIntervals.SuspendLayout();
            this.pnlStatistics.SuspendLayout();
            this.pnlUnit.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBackground
            // 
            this.pnlBackground.Controls.Add(this.txtTitle);
            this.pnlBackground.Controls.Add(this.lblTitle);
            this.pnlBackground.Controls.Add(this.pnlConfidenceIntervals);
            this.pnlBackground.Controls.Add(this.pnlStatistics);
            this.pnlBackground.Controls.Add(this.pnlUnit);
            this.pnlBackground.Controls.Add(this.lblDisplay);
            this.pnlBackground.Controls.Add(this.chkOverallAnalysis);
            this.pnlBackground.Controls.Add(this.chkBetweenWithinAnalysis);
            this.pnlBackground.Controls.Add(this.lblPerformAnalysis);
            this.pnlBackground.Controls.Add(this.txtK);
            this.pnlBackground.Controls.Add(this.txtTarget);
            this.pnlBackground.Controls.Add(this.lblK);
            this.pnlBackground.Controls.Add(this.btnCancel);
            this.pnlBackground.Controls.Add(this.btnOk);
            this.pnlBackground.Controls.Add(this.lblTarget);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Size = new System.Drawing.Size(429, 269);
            this.pnlBackground.TabIndex = 2;
            // 
            // txtTitle
            // 
            this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtTitle.Location = new System.Drawing.Point(69, 210);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(354, 21);
            this.txtTitle.TabIndex = 55;
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(19, 213);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(27, 13);
            this.lblTitle.TabIndex = 54;
            this.lblTitle.Text = "T&itle";
            // 
            // pnlConfidenceIntervals
            // 
            this.pnlConfidenceIntervals.Controls.Add(this.cboConfidenceIntervals);
            this.pnlConfidenceIntervals.Controls.Add(this.txtConfidenceLevel);
            this.pnlConfidenceIntervals.Controls.Add(this.lblConfidenceIntervals);
            this.pnlConfidenceIntervals.Controls.Add(this.lblConfidenceLevel);
            this.pnlConfidenceIntervals.Controls.Add(this.chkIncludeConfidenceIntervals);
            this.pnlConfidenceIntervals.Location = new System.Drawing.Point(194, 218);
            this.pnlConfidenceIntervals.Name = "pnlConfidenceIntervals";
            this.pnlConfidenceIntervals.Size = new System.Drawing.Size(243, 75);
            this.pnlConfidenceIntervals.TabIndex = 50;
            this.pnlConfidenceIntervals.Visible = false;
            // 
            // cboConfidenceIntervals
            // 
            this.cboConfidenceIntervals.Enabled = false;
            this.cboConfidenceIntervals.FormattingEnabled = true;
            this.cboConfidenceIntervals.Location = new System.Drawing.Point(149, 48);
            this.cboConfidenceIntervals.Name = "cboConfidenceIntervals";
            this.cboConfidenceIntervals.Size = new System.Drawing.Size(80, 21);
            this.cboConfidenceIntervals.TabIndex = 53;
            // 
            // txtConfidenceLevel
            // 
            this.txtConfidenceLevel.Enabled = false;
            this.txtConfidenceLevel.Location = new System.Drawing.Point(149, 26);
            this.txtConfidenceLevel.Name = "txtConfidenceLevel";
            this.txtConfidenceLevel.Size = new System.Drawing.Size(80, 21);
            this.txtConfidenceLevel.TabIndex = 51;
            this.txtConfidenceLevel.Text = "95.0";
            // 
            // lblConfidenceIntervals
            // 
            this.lblConfidenceIntervals.AutoSize = true;
            this.lblConfidenceIntervals.Enabled = false;
            this.lblConfidenceIntervals.Location = new System.Drawing.Point(34, 51);
            this.lblConfidenceIntervals.Name = "lblConfidenceIntervals";
            this.lblConfidenceIntervals.Size = new System.Drawing.Size(105, 13);
            this.lblConfidenceIntervals.TabIndex = 52;
            this.lblConfidenceIntervals.Text = "Confi&dence intervals";
            // 
            // lblConfidenceLevel
            // 
            this.lblConfidenceLevel.AutoSize = true;
            this.lblConfidenceLevel.Enabled = false;
            this.lblConfidenceLevel.Location = new System.Drawing.Point(34, 29);
            this.lblConfidenceLevel.Name = "lblConfidenceLevel";
            this.lblConfidenceLevel.Size = new System.Drawing.Size(86, 13);
            this.lblConfidenceLevel.TabIndex = 50;
            this.lblConfidenceLevel.Text = "Con&fidence level";
            // 
            // chkIncludeConfidenceIntervals
            // 
            this.chkIncludeConfidenceIntervals.AutoSize = true;
            this.chkIncludeConfidenceIntervals.Location = new System.Drawing.Point(16, 3);
            this.chkIncludeConfidenceIntervals.Name = "chkIncludeConfidenceIntervals";
            this.chkIncludeConfidenceIntervals.Size = new System.Drawing.Size(160, 17);
            this.chkIncludeConfidenceIntervals.TabIndex = 49;
            this.chkIncludeConfidenceIntervals.Text = "I&nclude confidence intervals";
            this.chkIncludeConfidenceIntervals.UseVisualStyleBackColor = true;
            // 
            // pnlStatistics
            // 
            this.pnlStatistics.Controls.Add(this.rdoBenchmarkZ);
            this.pnlStatistics.Controls.Add(this.rdoCapabilityStats);
            this.pnlStatistics.Location = new System.Drawing.Point(195, 157);
            this.pnlStatistics.Name = "pnlStatistics";
            this.pnlStatistics.Size = new System.Drawing.Size(166, 47);
            this.pnlStatistics.TabIndex = 48;
            // 
            // rdoBenchmarkZ
            // 
            this.rdoBenchmarkZ.AutoSize = true;
            this.rdoBenchmarkZ.Location = new System.Drawing.Point(15, 26);
            this.rdoBenchmarkZ.Name = "rdoBenchmarkZ";
            this.rdoBenchmarkZ.Size = new System.Drawing.Size(93, 17);
            this.rdoBenchmarkZ.TabIndex = 3;
            this.rdoBenchmarkZ.Text = "B&enchmark Z\'s";
            this.rdoBenchmarkZ.UseVisualStyleBackColor = true;
            // 
            // rdoCapabilityStats
            // 
            this.rdoCapabilityStats.AutoSize = true;
            this.rdoCapabilityStats.Checked = true;
            this.rdoCapabilityStats.Location = new System.Drawing.Point(15, 3);
            this.rdoCapabilityStats.Name = "rdoCapabilityStats";
            this.rdoCapabilityStats.Size = new System.Drawing.Size(142, 17);
            this.rdoCapabilityStats.TabIndex = 2;
            this.rdoCapabilityStats.TabStop = true;
            this.rdoCapabilityStats.Text = "Capabi&lity stats (Cp, Pp)";
            this.rdoCapabilityStats.UseVisualStyleBackColor = true;
            // 
            // pnlUnit
            // 
            this.pnlUnit.Controls.Add(this.rdoPercents);
            this.pnlUnit.Controls.Add(this.rdoPartsPerMillion);
            this.pnlUnit.Location = new System.Drawing.Point(195, 89);
            this.pnlUnit.Name = "pnlUnit";
            this.pnlUnit.Size = new System.Drawing.Size(166, 52);
            this.pnlUnit.TabIndex = 47;
            // 
            // rdoPercents
            // 
            this.rdoPercents.AutoSize = true;
            this.rdoPercents.Location = new System.Drawing.Point(15, 30);
            this.rdoPercents.Name = "rdoPercents";
            this.rdoPercents.Size = new System.Drawing.Size(67, 17);
            this.rdoPercents.TabIndex = 1;
            this.rdoPercents.Text = "Pe&rcents";
            this.rdoPercents.UseVisualStyleBackColor = true;
            // 
            // rdoPartsPerMillion
            // 
            this.rdoPartsPerMillion.AutoSize = true;
            this.rdoPartsPerMillion.Checked = true;
            this.rdoPartsPerMillion.Location = new System.Drawing.Point(15, 7);
            this.rdoPartsPerMillion.Name = "rdoPartsPerMillion";
            this.rdoPartsPerMillion.Size = new System.Drawing.Size(100, 17);
            this.rdoPartsPerMillion.TabIndex = 0;
            this.rdoPartsPerMillion.TabStop = true;
            this.rdoPartsPerMillion.Text = "&Parts per million";
            this.rdoPartsPerMillion.UseVisualStyleBackColor = true;
            // 
            // lblDisplay
            // 
            this.lblDisplay.AutoSize = true;
            this.lblDisplay.Location = new System.Drawing.Point(207, 73);
            this.lblDisplay.Name = "lblDisplay";
            this.lblDisplay.Size = new System.Drawing.Size(41, 13);
            this.lblDisplay.TabIndex = 46;
            this.lblDisplay.Text = "Display";
            // 
            // chkOverallAnalysis
            // 
            this.chkOverallAnalysis.AutoSize = true;
            this.chkOverallAnalysis.Checked = true;
            this.chkOverallAnalysis.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOverallAnalysis.Location = new System.Drawing.Point(22, 120);
            this.chkOverallAnalysis.Name = "chkOverallAnalysis";
            this.chkOverallAnalysis.Size = new System.Drawing.Size(101, 17);
            this.chkOverallAnalysis.TabIndex = 45;
            this.chkOverallAnalysis.Text = "O&verall analysis";
            this.chkOverallAnalysis.UseVisualStyleBackColor = true;
            this.chkOverallAnalysis.Visible = false;
            // 
            // chkBetweenWithinAnalysis
            // 
            this.chkBetweenWithinAnalysis.AutoSize = true;
            this.chkBetweenWithinAnalysis.Checked = true;
            this.chkBetweenWithinAnalysis.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBetweenWithinAnalysis.Location = new System.Drawing.Point(22, 97);
            this.chkBetweenWithinAnalysis.Name = "chkBetweenWithinAnalysis";
            this.chkBetweenWithinAnalysis.Size = new System.Drawing.Size(141, 17);
            this.chkBetweenWithinAnalysis.TabIndex = 44;
            this.chkBetweenWithinAnalysis.Text = "Bet&ween/within analysis";
            this.chkBetweenWithinAnalysis.UseVisualStyleBackColor = true;
            this.chkBetweenWithinAnalysis.Visible = false;
            // 
            // lblPerformAnalysis
            // 
            this.lblPerformAnalysis.AutoSize = true;
            this.lblPerformAnalysis.Location = new System.Drawing.Point(19, 73);
            this.lblPerformAnalysis.Name = "lblPerformAnalysis";
            this.lblPerformAnalysis.Size = new System.Drawing.Size(87, 13);
            this.lblPerformAnalysis.TabIndex = 7;
            this.lblPerformAnalysis.Text = "Perform Analysis";
            this.lblPerformAnalysis.Visible = false;
            // 
            // txtK
            // 
            this.txtK.Location = new System.Drawing.Point(281, 35);
            this.txtK.Name = "txtK";
            this.txtK.Size = new System.Drawing.Size(80, 21);
            this.txtK.TabIndex = 5;
            this.txtK.Text = "6";
            // 
            // txtTarget
            // 
            this.txtTarget.Location = new System.Drawing.Point(172, 9);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.Size = new System.Drawing.Size(80, 21);
            this.txtTarget.TabIndex = 3;
            // 
            // lblK
            // 
            this.lblK.AutoSize = true;
            this.lblK.Location = new System.Drawing.Point(19, 38);
            this.lblK.Name = "lblK";
            this.lblK.Size = new System.Drawing.Size(264, 13);
            this.lblK.TabIndex = 4;
            this.lblK.Text = "Use toleran&ce of K*sigma for capability statistics  K = ";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(341, 239);
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
            this.btnOk.Location = new System.Drawing.Point(264, 239);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(69, 21);
            this.btnOk.TabIndex = 37;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // lblTarget
            // 
            this.lblTarget.AutoSize = true;
            this.lblTarget.Location = new System.Drawing.Point(19, 12);
            this.lblTarget.Name = "lblTarget";
            this.lblTarget.Size = new System.Drawing.Size(137, 13);
            this.lblTarget.TabIndex = 2;
            this.lblTarget.Text = "&Target (adds Cpm to table)";
            // 
            // DlgCapaContinuousOptions
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(429, 269);
            this.Controls.Add(this.pnlBackground);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DlgCapaContinuousOptions";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Capability Analysis - Options";
            this.pnlBackground.ResumeLayout(false);
            this.pnlBackground.PerformLayout();
            this.pnlConfidenceIntervals.ResumeLayout(false);
            this.pnlConfidenceIntervals.PerformLayout();
            this.pnlStatistics.ResumeLayout(false);
            this.pnlStatistics.PerformLayout();
            this.pnlUnit.ResumeLayout(false);
            this.pnlUnit.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBackground;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblTarget;
        private System.Windows.Forms.TextBox txtTarget;
        private System.Windows.Forms.Label lblK;
        private System.Windows.Forms.CheckBox chkOverallAnalysis;
        private System.Windows.Forms.CheckBox chkBetweenWithinAnalysis;
        private System.Windows.Forms.Label lblPerformAnalysis;
        private System.Windows.Forms.TextBox txtK;
        private System.Windows.Forms.Panel pnlStatistics;
        private System.Windows.Forms.Panel pnlUnit;
        private System.Windows.Forms.RadioButton rdoPercents;
        private System.Windows.Forms.RadioButton rdoPartsPerMillion;
        private System.Windows.Forms.Label lblDisplay;
        private System.Windows.Forms.RadioButton rdoBenchmarkZ;
        private System.Windows.Forms.RadioButton rdoCapabilityStats;
        private System.Windows.Forms.Panel pnlConfidenceIntervals;
        private System.Windows.Forms.CheckBox chkIncludeConfidenceIntervals;
        private System.Windows.Forms.ComboBox cboConfidenceIntervals;
        private System.Windows.Forms.TextBox txtConfidenceLevel;
        private System.Windows.Forms.Label lblConfidenceIntervals;
        private System.Windows.Forms.Label lblConfidenceLevel;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblTitle;
    }
}