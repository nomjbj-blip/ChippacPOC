namespace DACrux.SEMDMS.Control
{
    partial class DPUCRDSetup
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint1 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 0D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint2 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 0D);
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnApply = new System.Windows.Forms.Button();
            this.chkImageMarker = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtRDTolerance = new System.Windows.Forms.TextBox();
            this.numRDCount = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.grpStart = new System.Windows.Forms.GroupBox();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.numShotStartY = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numShotStartX = new System.Windows.Forms.NumericUpDown();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rdoSize = new System.Windows.Forms.RadioButton();
            this.rdoPoint = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numShotArrayY = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numShotArrayX = new System.Windows.Forms.NumericUpDown();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chk1X1 = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRDCount)).BeginInit();
            this.grpStart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numShotStartY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numShotStartX)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numShotArrayY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numShotArrayX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnApply);
            this.panel1.Controls.Add(this.chkImageMarker);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.grpStart);
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(297, 299);
            this.panel1.TabIndex = 1;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(212, 264);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 3;
            this.btnApply.Text = "&Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // chkImageMarker
            // 
            this.chkImageMarker.AutoSize = true;
            this.chkImageMarker.Checked = true;
            this.chkImageMarker.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkImageMarker.Location = new System.Drawing.Point(13, 268);
            this.chkImageMarker.Name = "chkImageMarker";
            this.chkImageMarker.Size = new System.Drawing.Size(138, 16);
            this.chkImageMarker.TabIndex = 4;
            this.chkImageMarker.Text = "&Show Image Marker";
            this.chkImageMarker.CheckedChanged += new System.EventHandler(this.chkImageMarker_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.txtRDTolerance);
            this.groupBox3.Controls.Add(this.numRDCount);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Location = new System.Drawing.Point(7, 150);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(283, 68);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "RD Option";
            // 
            // txtRDTolerance
            // 
            this.txtRDTolerance.Location = new System.Drawing.Point(157, 17);
            this.txtRDTolerance.Name = "txtRDTolerance";
            this.txtRDTolerance.Size = new System.Drawing.Size(79, 21);
            this.txtRDTolerance.TabIndex = 1;
            this.txtRDTolerance.Text = "50";
            this.txtRDTolerance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numRDCount
            // 
            this.numRDCount.Location = new System.Drawing.Point(202, 43);
            this.numRDCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRDCount.Name = "numRDCount";
            this.numRDCount.Size = new System.Drawing.Size(49, 21);
            this.numRDCount.TabIndex = 4;
            this.numRDCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numRDCount.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(91, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "&Count";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(242, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(23, 12);
            this.label7.TabIndex = 2;
            this.label7.Text = "μm";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(67, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "&Tolerance";
            // 
            // grpStart
            // 
            this.grpStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpStart.Controls.Add(this.btnRight);
            this.grpStart.Controls.Add(this.btnUp);
            this.grpStart.Controls.Add(this.btnDown);
            this.grpStart.Controls.Add(this.btnLeft);
            this.grpStart.Controls.Add(this.numShotStartY);
            this.grpStart.Controls.Add(this.label3);
            this.grpStart.Controls.Add(this.label4);
            this.grpStart.Controls.Add(this.numShotStartX);
            this.grpStart.Location = new System.Drawing.Point(7, 59);
            this.grpStart.Name = "grpStart";
            this.grpStart.Size = new System.Drawing.Size(283, 85);
            this.grpStart.TabIndex = 1;
            this.grpStart.TabStop = false;
            this.grpStart.Text = "Starting Die (Lower Left Die)";
            // 
            // btnRight
            // 
            this.btnRight.Location = new System.Drawing.Point(52, 35);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(24, 23);
            this.btnRight.TabIndex = 10;
            this.btnRight.Text = "→";
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(31, 14);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(24, 23);
            this.btnUp.TabIndex = 11;
            this.btnUp.Text = "↑";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(30, 57);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(24, 23);
            this.btnDown.TabIndex = 8;
            this.btnDown.Text = "↓";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnLeft
            // 
            this.btnLeft.Location = new System.Drawing.Point(9, 35);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(24, 23);
            this.btnLeft.TabIndex = 9;
            this.btnLeft.Text = "←";
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // numShotStartY
            // 
            this.numShotStartY.Location = new System.Drawing.Point(221, 17);
            this.numShotStartY.Name = "numShotStartY";
            this.numShotStartY.Size = new System.Drawing.Size(49, 21);
            this.numShotStartY.TabIndex = 3;
            this.numShotStartY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numShotStartY.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numShotStartY.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(202, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "&Y";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(89, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "&X";
            // 
            // numShotStartX
            // 
            this.numShotStartX.Location = new System.Drawing.Point(108, 17);
            this.numShotStartX.Name = "numShotStartX";
            this.numShotStartX.Size = new System.Drawing.Size(49, 21);
            this.numShotStartX.TabIndex = 1;
            this.numShotStartX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numShotStartX.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numShotStartX.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.rdoSize);
            this.groupBox4.Controls.Add(this.rdoPoint);
            this.groupBox4.Location = new System.Drawing.Point(7, 224);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(283, 33);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Mode";
            // 
            // rdoSize
            // 
            this.rdoSize.AutoSize = true;
            this.rdoSize.Location = new System.Drawing.Point(177, 11);
            this.rdoSize.Name = "rdoSize";
            this.rdoSize.Size = new System.Drawing.Size(48, 16);
            this.rdoSize.TabIndex = 0;
            this.rdoSize.Text = "Size";
            this.rdoSize.UseVisualStyleBackColor = true;
            // 
            // rdoPoint
            // 
            this.rdoPoint.AutoSize = true;
            this.rdoPoint.Checked = true;
            this.rdoPoint.Location = new System.Drawing.Point(73, 11);
            this.rdoPoint.Name = "rdoPoint";
            this.rdoPoint.Size = new System.Drawing.Size(51, 16);
            this.rdoPoint.TabIndex = 0;
            this.rdoPoint.TabStop = true;
            this.rdoPoint.Text = "Point";
            this.rdoPoint.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chk1X1);
            this.groupBox1.Controls.Add(this.numShotArrayY);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numShotArrayX);
            this.groupBox1.Location = new System.Drawing.Point(7, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(283, 49);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Shot Size";
            // 
            // numShotArrayY
            // 
            this.numShotArrayY.Location = new System.Drawing.Point(221, 18);
            this.numShotArrayY.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numShotArrayY.Name = "numShotArrayY";
            this.numShotArrayY.Size = new System.Drawing.Size(49, 21);
            this.numShotArrayY.TabIndex = 3;
            this.numShotArrayY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numShotArrayY.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numShotArrayY.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(175, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "&Height";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(67, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Width";
            // 
            // numShotArrayX
            // 
            this.numShotArrayX.Location = new System.Drawing.Point(108, 18);
            this.numShotArrayX.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numShotArrayX.Name = "numShotArrayX";
            this.numShotArrayX.Size = new System.Drawing.Size(49, 21);
            this.numShotArrayX.TabIndex = 1;
            this.numShotArrayX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numShotArrayX.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numShotArrayX.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // chart
            // 
            chartArea1.AxisX.MajorGrid.Interval = 0D;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.SystemColors.ButtonFace;
            chartArea1.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea1);
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart.Location = new System.Drawing.Point(0, 299);
            this.chart.Name = "chart";
            series1.ChartArea = "ChartArea1";
            series1.IsValueShownAsLabel = true;
            series1.Name = "Series1";
            dataPoint1.AxisLabel = "NonRD";
            dataPoint1.BorderColor = System.Drawing.Color.Empty;
            dataPoint1.IsValueShownAsLabel = true;
            dataPoint1.Label = "";
            dataPoint1.LegendText = "";
            dataPoint2.AxisLabel = "RD";
            dataPoint2.BorderWidth = 1;
            dataPoint2.LabelBorderWidth = 1;
            series1.Points.Add(dataPoint1);
            series1.Points.Add(dataPoint2);
            this.chart.Series.Add(series1);
            this.chart.Size = new System.Drawing.Size(297, 260);
            this.chart.TabIndex = 2;
            this.chart.Text = "chart1";
            this.chart.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ChartDefect_MouseClick);
            // 
            // chk1X1
            // 
            this.chk1X1.AutoSize = true;
            this.chk1X1.Location = new System.Drawing.Point(8, 21);
            this.chk1X1.Name = "chk1X1";
            this.chk1X1.Size = new System.Drawing.Size(44, 16);
            this.chk1X1.TabIndex = 12;
            this.chk1X1.Text = "1X1";
            this.chk1X1.UseVisualStyleBackColor = true;
            this.chk1X1.CheckedChanged += new System.EventHandler(this.chk1X1_CheckedChanged);
            // 
            // DPUCRDSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chart);
            this.Controls.Add(this.panel1);
            this.Name = "DPUCRDSetup";
            this.Size = new System.Drawing.Size(297, 559);
            this.Load += new System.EventHandler(this.DPUCShotSetup_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRDCount)).EndInit();
            this.grpStart.ResumeLayout(false);
            this.grpStart.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numShotStartY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numShotStartX)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numShotArrayY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numShotArrayX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.CheckBox chkImageMarker;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtRDTolerance;
        private System.Windows.Forms.NumericUpDown numRDCount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox grpStart;
        private System.Windows.Forms.NumericUpDown numShotStartY;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numShotStartX;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numShotArrayY;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numShotArrayX;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rdoSize;
        private System.Windows.Forms.RadioButton rdoPoint;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.CheckBox chk1X1;
    }
}
