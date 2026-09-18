namespace DACrux.TEST.Control
{
    partial class ucParameterTrend
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panel1 = new System.Windows.Forms.Panel();
            this.nudInterval = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.chkSpecLimit = new System.Windows.Forms.CheckBox();
            this.chkAverageLabel = new System.Windows.Forms.CheckBox();
            this.chkBoxPlot = new System.Windows.Forms.CheckBox();
            this.chkRawData = new System.Windows.Forms.CheckBox();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chkAverageVisible = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.chkSpecLimit);
            this.panel1.Controls.Add(this.chkAverageVisible);
            this.panel1.Controls.Add(this.nudInterval);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.chkAverageLabel);
            this.panel1.Controls.Add(this.chkBoxPlot);
            this.panel1.Controls.Add(this.chkRawData);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 30);
            this.panel1.TabIndex = 0;
            // 
            // nudInterval
            // 
            this.nudInterval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudInterval.Location = new System.Drawing.Point(764, 4);
            this.nudInterval.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudInterval.Name = "nudInterval";
            this.nudInterval.Size = new System.Drawing.Size(33, 21);
            this.nudInterval.TabIndex = 6;
            this.nudInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(713, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "Interval";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBox1.Location = new System.Drawing.Point(388, 0);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(95, 30);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Control Limit";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // chkSpecLimit
            // 
            this.chkSpecLimit.AutoSize = true;
            this.chkSpecLimit.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkSpecLimit.Location = new System.Drawing.Point(261, 0);
            this.chkSpecLimit.Name = "chkSpecLimit";
            this.chkSpecLimit.Size = new System.Drawing.Size(127, 30);
            this.chkSpecLimit.TabIndex = 3;
            this.chkSpecLimit.Text = "Specification Limit";
            this.chkSpecLimit.UseVisualStyleBackColor = true;
            // 
            // chkAverageLabel
            // 
            this.chkAverageLabel.AutoSize = true;
            this.chkAverageLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkAverageLabel.Location = new System.Drawing.Point(176, 0);
            this.chkAverageLabel.Name = "chkAverageLabel";
            this.chkAverageLabel.Size = new System.Drawing.Size(15, 30);
            this.chkAverageLabel.TabIndex = 2;
            this.chkAverageLabel.UseVisualStyleBackColor = true;
            // 
            // chkBoxPlot
            // 
            this.chkBoxPlot.AutoSize = true;
            this.chkBoxPlot.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkBoxPlot.Location = new System.Drawing.Point(109, 0);
            this.chkBoxPlot.Name = "chkBoxPlot";
            this.chkBoxPlot.Size = new System.Drawing.Size(67, 30);
            this.chkBoxPlot.TabIndex = 1;
            this.chkBoxPlot.Text = "BoxPlot";
            this.chkBoxPlot.UseVisualStyleBackColor = true;
            // 
            // chkRawData
            // 
            this.chkRawData.AutoSize = true;
            this.chkRawData.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkRawData.Location = new System.Drawing.Point(0, 0);
            this.chkRawData.Name = "chkRawData";
            this.chkRawData.Size = new System.Drawing.Size(109, 30);
            this.chkRawData.TabIndex = 0;
            this.chkRawData.Text = "RawData Label";
            this.chkRawData.UseVisualStyleBackColor = true;
            // 
            // chart1
            // 
            chartArea2.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea2);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(0, 30);
            this.chart1.Name = "chart1";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(800, 570);
            this.chart1.TabIndex = 1;
            this.chart1.Text = "chart1";
            // 
            // chkAverageVisible
            // 
            this.chkAverageVisible.AutoSize = true;
            this.chkAverageVisible.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkAverageVisible.Location = new System.Drawing.Point(191, 0);
            this.chkAverageVisible.Name = "chkAverageVisible";
            this.chkAverageVisible.Size = new System.Drawing.Size(70, 30);
            this.chkAverageVisible.TabIndex = 7;
            this.chkAverageVisible.Text = "Average";
            this.chkAverageVisible.UseVisualStyleBackColor = true;
            // 
            // ucParameterTrend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.panel1);
            this.Name = "ucParameterTrend";
            this.Size = new System.Drawing.Size(800, 600);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkSpecLimit;
        private System.Windows.Forms.CheckBox chkAverageLabel;
        private System.Windows.Forms.CheckBox chkBoxPlot;
        private System.Windows.Forms.CheckBox chkRawData;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.NumericUpDown nudInterval;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.CheckBox chkAverageVisible;
    }
}
