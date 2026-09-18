namespace DACrux.BStats.StatDialog.DlgDescriptiveAnalysis_
{
    partial class DlgStatistics
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
            this.chkMSSD = new System.Windows.Forms.CheckBox();
            this.chkKurtosis = new System.Windows.Forms.CheckBox();
            this.chkSkewness = new System.Windows.Forms.CheckBox();
            this.chkSumOfSquares = new System.Windows.Forms.CheckBox();
            this.chkMode = new System.Windows.Forms.CheckBox();
            this.chkInterquartileRange = new System.Windows.Forms.CheckBox();
            this.chkQ3 = new System.Windows.Forms.CheckBox();
            this.chkMedian = new System.Windows.Forms.CheckBox();
            this.chkQ1 = new System.Windows.Forms.CheckBox();
            this.chkCumulativePercent = new System.Windows.Forms.CheckBox();
            this.chkPercent = new System.Windows.Forms.CheckBox();
            this.chkCumulativeN = new System.Windows.Forms.CheckBox();
            this.chkNtotal = new System.Windows.Forms.CheckBox();
            this.chkNmissing = new System.Windows.Forms.CheckBox();
            this.chkNnonmissing = new System.Windows.Forms.CheckBox();
            this.chkRange = new System.Windows.Forms.CheckBox();
            this.chkMaximum = new System.Windows.Forms.CheckBox();
            this.chkMinimum = new System.Windows.Forms.CheckBox();
            this.chkSum = new System.Windows.Forms.CheckBox();
            this.chkTrimmedMean = new System.Windows.Forms.CheckBox();
            this.chkCoefficientOfVariation = new System.Windows.Forms.CheckBox();
            this.chkVariance = new System.Windows.Forms.CheckBox();
            this.chkStandardDeviation = new System.Windows.Forms.CheckBox();
            this.chkMeanSE = new System.Windows.Forms.CheckBox();
            this.chkMean = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.pnlBackground.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBackground
            // 
            this.pnlBackground.Controls.Add(this.chkMSSD);
            this.pnlBackground.Controls.Add(this.chkKurtosis);
            this.pnlBackground.Controls.Add(this.chkSkewness);
            this.pnlBackground.Controls.Add(this.chkSumOfSquares);
            this.pnlBackground.Controls.Add(this.chkMode);
            this.pnlBackground.Controls.Add(this.chkInterquartileRange);
            this.pnlBackground.Controls.Add(this.chkQ3);
            this.pnlBackground.Controls.Add(this.chkMedian);
            this.pnlBackground.Controls.Add(this.chkQ1);
            this.pnlBackground.Controls.Add(this.chkCumulativePercent);
            this.pnlBackground.Controls.Add(this.chkPercent);
            this.pnlBackground.Controls.Add(this.chkCumulativeN);
            this.pnlBackground.Controls.Add(this.chkNtotal);
            this.pnlBackground.Controls.Add(this.chkNmissing);
            this.pnlBackground.Controls.Add(this.chkNnonmissing);
            this.pnlBackground.Controls.Add(this.chkRange);
            this.pnlBackground.Controls.Add(this.chkMaximum);
            this.pnlBackground.Controls.Add(this.chkMinimum);
            this.pnlBackground.Controls.Add(this.chkSum);
            this.pnlBackground.Controls.Add(this.chkTrimmedMean);
            this.pnlBackground.Controls.Add(this.chkCoefficientOfVariation);
            this.pnlBackground.Controls.Add(this.chkVariance);
            this.pnlBackground.Controls.Add(this.chkStandardDeviation);
            this.pnlBackground.Controls.Add(this.chkMeanSE);
            this.pnlBackground.Controls.Add(this.chkMean);
            this.pnlBackground.Controls.Add(this.btnCancel);
            this.pnlBackground.Controls.Add(this.btnOk);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Size = new System.Drawing.Size(478, 250);
            this.pnlBackground.TabIndex = 1;
            // 
            // chkMSSD
            // 
            this.chkMSSD.AutoSize = true;
            this.chkMSSD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkMSSD.Location = new System.Drawing.Point(168, 181);
            this.chkMSSD.Name = "chkMSSD";
            this.chkMSSD.Size = new System.Drawing.Size(50, 17);
            this.chkMSSD.TabIndex = 52;
            this.chkMSSD.Text = "MSS&D";
            this.chkMSSD.UseVisualStyleBackColor = true;
            // 
            // chkKurtosis
            // 
            this.chkKurtosis.AutoSize = true;
            this.chkKurtosis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkKurtosis.Location = new System.Drawing.Point(168, 165);
            this.chkKurtosis.Name = "chkKurtosis";
            this.chkKurtosis.Size = new System.Drawing.Size(61, 17);
            this.chkKurtosis.TabIndex = 51;
            this.chkKurtosis.Text = "&Kurtosis";
            this.chkKurtosis.UseVisualStyleBackColor = true;
            // 
            // chkSkewness
            // 
            this.chkSkewness.AutoSize = true;
            this.chkSkewness.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkSkewness.Location = new System.Drawing.Point(168, 149);
            this.chkSkewness.Name = "chkSkewness";
            this.chkSkewness.Size = new System.Drawing.Size(70, 17);
            this.chkSkewness.TabIndex = 50;
            this.chkSkewness.Text = "Ske&wness";
            this.chkSkewness.UseVisualStyleBackColor = true;
            // 
            // chkSumOfSquares
            // 
            this.chkSumOfSquares.AutoSize = true;
            this.chkSumOfSquares.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkSumOfSquares.Location = new System.Drawing.Point(168, 134);
            this.chkSumOfSquares.Name = "chkSumOfSquares";
            this.chkSumOfSquares.Size = new System.Drawing.Size(98, 17);
            this.chkSumOfSquares.TabIndex = 49;
            this.chkSumOfSquares.Text = "Sum of Squares";
            this.chkSumOfSquares.UseVisualStyleBackColor = true;
            // 
            // chkMode
            // 
            this.chkMode.AutoSize = true;
            this.chkMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkMode.Location = new System.Drawing.Point(12, 197);
            this.chkMode.Name = "chkMode";
            this.chkMode.Size = new System.Drawing.Size(49, 17);
            this.chkMode.TabIndex = 48;
            this.chkMode.Text = "Mode";
            this.chkMode.UseVisualStyleBackColor = true;
            // 
            // chkInterquartileRange
            // 
            this.chkInterquartileRange.AutoSize = true;
            this.chkInterquartileRange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkInterquartileRange.Location = new System.Drawing.Point(12, 181);
            this.chkInterquartileRange.Name = "chkInterquartileRange";
            this.chkInterquartileRange.Size = new System.Drawing.Size(114, 17);
            this.chkInterquartileRange.TabIndex = 47;
            this.chkInterquartileRange.Text = "Inter&quartile range";
            this.chkInterquartileRange.UseVisualStyleBackColor = true;
            // 
            // chkQ3
            // 
            this.chkQ3.AutoSize = true;
            this.chkQ3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkQ3.Location = new System.Drawing.Point(12, 165);
            this.chkQ3.Name = "chkQ3";
            this.chkQ3.Size = new System.Drawing.Size(86, 17);
            this.chkQ3.TabIndex = 46;
            this.chkQ3.Text = "&Third quartile";
            this.chkQ3.UseVisualStyleBackColor = true;
            // 
            // chkMedian
            // 
            this.chkMedian.AutoSize = true;
            this.chkMedian.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkMedian.Location = new System.Drawing.Point(12, 149);
            this.chkMedian.Name = "chkMedian";
            this.chkMedian.Size = new System.Drawing.Size(57, 17);
            this.chkMedian.TabIndex = 45;
            this.chkMedian.Text = "M&edian";
            this.chkMedian.UseVisualStyleBackColor = true;
            // 
            // chkQ1
            // 
            this.chkQ1.AutoSize = true;
            this.chkQ1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkQ1.Location = new System.Drawing.Point(12, 134);
            this.chkQ1.Name = "chkQ1";
            this.chkQ1.Size = new System.Drawing.Size(83, 17);
            this.chkQ1.TabIndex = 44;
            this.chkQ1.Text = "&First quartile";
            this.chkQ1.UseVisualStyleBackColor = true;
            // 
            // chkCumulativePercent
            // 
            this.chkCumulativePercent.AutoSize = true;
            this.chkCumulativePercent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkCumulativePercent.Location = new System.Drawing.Point(334, 90);
            this.chkCumulativePercent.Name = "chkCumulativePercent";
            this.chkCumulativePercent.Size = new System.Drawing.Size(116, 17);
            this.chkCumulativePercent.TabIndex = 43;
            this.chkCumulativePercent.Text = "Cumu&lative percent";
            this.chkCumulativePercent.UseVisualStyleBackColor = true;
            // 
            // chkPercent
            // 
            this.chkPercent.AutoSize = true;
            this.chkPercent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkPercent.Location = new System.Drawing.Point(334, 74);
            this.chkPercent.Name = "chkPercent";
            this.chkPercent.Size = new System.Drawing.Size(60, 17);
            this.chkPercent.TabIndex = 42;
            this.chkPercent.Text = "&Percent";
            this.chkPercent.UseVisualStyleBackColor = true;
            // 
            // chkCumulativeN
            // 
            this.chkCumulativeN.AutoSize = true;
            this.chkCumulativeN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkCumulativeN.Location = new System.Drawing.Point(334, 58);
            this.chkCumulativeN.Name = "chkCumulativeN";
            this.chkCumulativeN.Size = new System.Drawing.Size(86, 17);
            this.chkCumulativeN.TabIndex = 41;
            this.chkCumulativeN.Text = "&Cumulative N";
            this.chkCumulativeN.UseVisualStyleBackColor = true;
            // 
            // chkNtotal
            // 
            this.chkNtotal.AutoSize = true;
            this.chkNtotal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkNtotal.Location = new System.Drawing.Point(334, 43);
            this.chkNtotal.Name = "chkNtotal";
            this.chkNtotal.Size = new System.Drawing.Size(55, 17);
            this.chkNtotal.TabIndex = 40;
            this.chkNtotal.Text = "N &total";
            this.chkNtotal.UseVisualStyleBackColor = true;
            // 
            // chkNmissing
            // 
            this.chkNmissing.AutoSize = true;
            this.chkNmissing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkNmissing.Location = new System.Drawing.Point(334, 27);
            this.chkNmissing.Name = "chkNmissing";
            this.chkNmissing.Size = new System.Drawing.Size(67, 17);
            this.chkNmissing.TabIndex = 39;
            this.chkNmissing.Text = "N missin&g";
            this.chkNmissing.UseVisualStyleBackColor = true;
            // 
            // chkNnonmissing
            // 
            this.chkNnonmissing.AutoSize = true;
            this.chkNnonmissing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkNnonmissing.Location = new System.Drawing.Point(334, 11);
            this.chkNnonmissing.Name = "chkNnonmissing";
            this.chkNnonmissing.Size = new System.Drawing.Size(85, 17);
            this.chkNnonmissing.TabIndex = 38;
            this.chkNnonmissing.Text = "&N nonmissing";
            this.chkNnonmissing.UseVisualStyleBackColor = true;
            // 
            // chkRange
            // 
            this.chkRange.AutoSize = true;
            this.chkRange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkRange.Location = new System.Drawing.Point(168, 74);
            this.chkRange.Name = "chkRange";
            this.chkRange.Size = new System.Drawing.Size(54, 17);
            this.chkRange.TabIndex = 37;
            this.chkRange.Text = "&Range";
            this.chkRange.UseVisualStyleBackColor = true;
            // 
            // chkMaximum
            // 
            this.chkMaximum.AutoSize = true;
            this.chkMaximum.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkMaximum.Location = new System.Drawing.Point(168, 58);
            this.chkMaximum.Name = "chkMaximum";
            this.chkMaximum.Size = new System.Drawing.Size(67, 17);
            this.chkMaximum.TabIndex = 36;
            this.chkMaximum.Text = "Ma&ximum";
            this.chkMaximum.UseVisualStyleBackColor = true;
            // 
            // chkMinimum
            // 
            this.chkMinimum.AutoSize = true;
            this.chkMinimum.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkMinimum.Location = new System.Drawing.Point(168, 43);
            this.chkMinimum.Name = "chkMinimum";
            this.chkMinimum.Size = new System.Drawing.Size(63, 17);
            this.chkMinimum.TabIndex = 35;
            this.chkMinimum.Text = "M&inimum";
            this.chkMinimum.UseVisualStyleBackColor = true;
            // 
            // chkSum
            // 
            this.chkSum.AutoSize = true;
            this.chkSum.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkSum.Location = new System.Drawing.Point(168, 27);
            this.chkSum.Name = "chkSum";
            this.chkSum.Size = new System.Drawing.Size(43, 17);
            this.chkSum.TabIndex = 34;
            this.chkSum.Text = "S&um";
            this.chkSum.UseVisualStyleBackColor = true;
            // 
            // chkTrimmedMean
            // 
            this.chkTrimmedMean.AutoSize = true;
            this.chkTrimmedMean.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkTrimmedMean.Location = new System.Drawing.Point(168, 11);
            this.chkTrimmedMean.Name = "chkTrimmedMean";
            this.chkTrimmedMean.Size = new System.Drawing.Size(92, 17);
            this.chkTrimmedMean.TabIndex = 33;
            this.chkTrimmedMean.Text = "Trimmed mean";
            this.chkTrimmedMean.UseVisualStyleBackColor = true;
            // 
            // chkCoefficientOfVariation
            // 
            this.chkCoefficientOfVariation.AutoSize = true;
            this.chkCoefficientOfVariation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkCoefficientOfVariation.Location = new System.Drawing.Point(12, 74);
            this.chkCoefficientOfVariation.Name = "chkCoefficientOfVariation";
            this.chkCoefficientOfVariation.Size = new System.Drawing.Size(133, 17);
            this.chkCoefficientOfVariation.TabIndex = 32;
            this.chkCoefficientOfVariation.Text = "Coefficient of variation";
            this.chkCoefficientOfVariation.UseVisualStyleBackColor = true;
            // 
            // chkVariance
            // 
            this.chkVariance.AutoSize = true;
            this.chkVariance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkVariance.Location = new System.Drawing.Point(12, 58);
            this.chkVariance.Name = "chkVariance";
            this.chkVariance.Size = new System.Drawing.Size(64, 17);
            this.chkVariance.TabIndex = 31;
            this.chkVariance.Text = "&Variance";
            this.chkVariance.UseVisualStyleBackColor = true;
            // 
            // chkStandardDeviation
            // 
            this.chkStandardDeviation.AutoSize = true;
            this.chkStandardDeviation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkStandardDeviation.Location = new System.Drawing.Point(12, 43);
            this.chkStandardDeviation.Name = "chkStandardDeviation";
            this.chkStandardDeviation.Size = new System.Drawing.Size(115, 17);
            this.chkStandardDeviation.TabIndex = 30;
            this.chkStandardDeviation.Text = "&Standard Deviation";
            this.chkStandardDeviation.UseVisualStyleBackColor = true;
            // 
            // chkMeanSE
            // 
            this.chkMeanSE.AutoSize = true;
            this.chkMeanSE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkMeanSE.Location = new System.Drawing.Point(12, 27);
            this.chkMeanSE.Name = "chkMeanSE";
            this.chkMeanSE.Size = new System.Drawing.Size(77, 17);
            this.chkMeanSE.TabIndex = 29;
            this.chkMeanSE.Text = "SE of me&an";
            this.chkMeanSE.UseVisualStyleBackColor = true;
            // 
            // chkMean
            // 
            this.chkMean.AutoSize = true;
            this.chkMean.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkMean.Location = new System.Drawing.Point(12, 11);
            this.chkMean.Name = "chkMean";
            this.chkMean.Size = new System.Drawing.Size(49, 17);
            this.chkMean.TabIndex = 28;
            this.chkMean.Text = "M&ean";
            this.chkMean.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(393, 215);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 21);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(318, 215);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(69, 21);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // DlgStatistics
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(478, 250);
            this.Controls.Add(this.pnlBackground);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DlgStatistics";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Descriptive Statistics - Statistics";
            this.pnlBackground.ResumeLayout(false);
            this.pnlBackground.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBackground;
        private System.Windows.Forms.CheckBox chkMean;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.CheckBox chkMedian;
        private System.Windows.Forms.CheckBox chkQ1;
        private System.Windows.Forms.CheckBox chkCumulativePercent;
        private System.Windows.Forms.CheckBox chkPercent;
        private System.Windows.Forms.CheckBox chkCumulativeN;
        private System.Windows.Forms.CheckBox chkNtotal;
        private System.Windows.Forms.CheckBox chkNmissing;
        private System.Windows.Forms.CheckBox chkNnonmissing;
        private System.Windows.Forms.CheckBox chkRange;
        private System.Windows.Forms.CheckBox chkMaximum;
        private System.Windows.Forms.CheckBox chkMinimum;
        private System.Windows.Forms.CheckBox chkSum;
        private System.Windows.Forms.CheckBox chkTrimmedMean;
        private System.Windows.Forms.CheckBox chkCoefficientOfVariation;
        private System.Windows.Forms.CheckBox chkVariance;
        private System.Windows.Forms.CheckBox chkStandardDeviation;
        private System.Windows.Forms.CheckBox chkMeanSE;
        private System.Windows.Forms.CheckBox chkMSSD;
        private System.Windows.Forms.CheckBox chkKurtosis;
        private System.Windows.Forms.CheckBox chkSkewness;
        private System.Windows.Forms.CheckBox chkSumOfSquares;
        private System.Windows.Forms.CheckBox chkMode;
        private System.Windows.Forms.CheckBox chkInterquartileRange;
        private System.Windows.Forms.CheckBox chkQ3;

    }
}