namespace DACrux.BStats.StatDialog.DlgHypothesisTesting_
{
    partial class DlgOneSampleT
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
            this.pnlBack = new System.Windows.Forms.Panel();
            this.cboAlternative = new System.Windows.Forms.ComboBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.txtTestMean = new System.Windows.Forms.TextBox();
            this.lblAlternative = new System.Windows.Forms.Label();
            this.lblConfidenceLevel = new System.Windows.Forms.Label();
            this.lblMean = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblPercent = new System.Windows.Forms.Label();
            this.pnlBack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBack
            // 
            this.pnlBack.Controls.Add(this.lblPercent);
            this.pnlBack.Controls.Add(this.cboAlternative);
            this.pnlBack.Controls.Add(this.numericUpDown1);
            this.pnlBack.Controls.Add(this.txtTestMean);
            this.pnlBack.Controls.Add(this.lblAlternative);
            this.pnlBack.Controls.Add(this.lblConfidenceLevel);
            this.pnlBack.Controls.Add(this.lblMean);
            this.pnlBack.Controls.Add(this.btnCancel);
            this.pnlBack.Controls.Add(this.btnOk);
            this.pnlBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBack.Location = new System.Drawing.Point(0, 0);
            this.pnlBack.Name = "pnlBack";
            this.pnlBack.Size = new System.Drawing.Size(228, 164);
            this.pnlBack.TabIndex = 2;
            // 
            // cboAlternative
            // 
            this.cboAlternative.FormattingEnabled = true;
            this.cboAlternative.Items.AddRange(new object[] {
            "less than",
            "not equal",
            "greater than"});
            this.cboAlternative.Location = new System.Drawing.Point(117, 96);
            this.cboAlternative.Name = "cboAlternative";
            this.cboAlternative.Size = new System.Drawing.Size(95, 21);
            this.cboAlternative.TabIndex = 7;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DecimalPlaces = 2;
            this.numericUpDown1.Location = new System.Drawing.Point(117, 64);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            131072});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(68, 21);
            this.numericUpDown1.TabIndex = 5;
            this.numericUpDown1.Value = new decimal(new int[] {
            9500,
            0,
            0,
            131072});
            // 
            // txtTestMean
            // 
            this.txtTestMean.Location = new System.Drawing.Point(117, 26);
            this.txtTestMean.Name = "txtTestMean";
            this.txtTestMean.Size = new System.Drawing.Size(95, 21);
            this.txtTestMean.TabIndex = 3;
            // 
            // lblAlternative
            // 
            this.lblAlternative.AutoSize = true;
            this.lblAlternative.Location = new System.Drawing.Point(13, 99);
            this.lblAlternative.Name = "lblAlternative";
            this.lblAlternative.Size = new System.Drawing.Size(64, 13);
            this.lblAlternative.TabIndex = 6;
            this.lblAlternative.Text = "&Alternative:";
            // 
            // lblConfidenceLevel
            // 
            this.lblConfidenceLevel.AutoSize = true;
            this.lblConfidenceLevel.Location = new System.Drawing.Point(13, 66);
            this.lblConfidenceLevel.Name = "lblConfidenceLevel";
            this.lblConfidenceLevel.Size = new System.Drawing.Size(90, 13);
            this.lblConfidenceLevel.TabIndex = 4;
            this.lblConfidenceLevel.Text = "&Confidence level:";
            // 
            // lblMean
            // 
            this.lblMean.AutoSize = true;
            this.lblMean.Location = new System.Drawing.Point(13, 29);
            this.lblMean.Name = "lblMean";
            this.lblMean.Size = new System.Drawing.Size(61, 13);
            this.lblMean.TabIndex = 2;
            this.lblMean.Text = "&Test mean:";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(155, 128);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(59, 22);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(90, 128);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(59, 22);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // lblPercent
            // 
            this.lblPercent.AutoSize = true;
            this.lblPercent.Location = new System.Drawing.Point(188, 66);
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.Size = new System.Drawing.Size(26, 13);
            this.lblPercent.TabIndex = 10;
            this.lblPercent.Text = "(%)";
            // 
            // DlgOneSampleT
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(228, 164);
            this.Controls.Add(this.pnlBack);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DlgOneSampleT";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "One sample t test";
            this.pnlBack.ResumeLayout(false);
            this.pnlBack.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBack;
        private System.Windows.Forms.ComboBox cboAlternative;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.TextBox txtTestMean;
        private System.Windows.Forms.Label lblAlternative;
        private System.Windows.Forms.Label lblConfidenceLevel;
        private System.Windows.Forms.Label lblMean;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblPercent;
    }
}