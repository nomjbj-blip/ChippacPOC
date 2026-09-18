namespace DACrux.TEST.Control
{
    partial class ucTrendChart
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint9 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(2D, "0,0,0,0,0,0");
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint10 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(2D, "0,0,0,0,0,0");
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint11 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(2D, "0,0,0,0,0,0");
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint12 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(2D, "0,0,0,0,0,0");
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint13 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(2D, "0,0,0,0,0,0");
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint14 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(2D, "0,0,0,0,0,0");
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint15 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(2D, "0,0,0,0,0,0");
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint16 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(2D, "0,0,0,0,0,0");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucTrendChart));
            this.labLSL = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.labUSL = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.labSum = new System.Windows.Forms.Label();
            this.labStd = new System.Windows.Forms.Label();
            this.labQ3 = new System.Windows.Forms.Label();
            this.labMax = new System.Windows.Forms.Label();
            this.labMin = new System.Windows.Forms.Label();
            this.labCnt = new System.Windows.Forms.Label();
            this.labQ1 = new System.Windows.Forms.Label();
            this.labMedian = new System.Windows.Forms.Label();
            this.labAvg = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CopyImage = new System.Windows.Forms.ToolStripMenuItem();
            this.chartResetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.yAxisMaxMinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labTitle = new System.Windows.Forms.Label();
            this.chkSpec = new System.Windows.Forms.CheckBox();
            this.chkSigma = new System.Windows.Forms.CheckBox();
            this.CheckTrend = new System.Windows.Forms.CheckBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // labLSL
            // 
            this.labLSL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labLSL.Location = new System.Drawing.Point(76, 222);
            this.labLSL.Name = "labLSL";
            this.labLSL.Size = new System.Drawing.Size(79, 22);
            this.labLSL.TabIndex = 12;
            this.labLSL.Text = "0";
            this.labLSL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label13.Location = new System.Drawing.Point(3, 222);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(73, 22);
            this.label13.TabIndex = 11;
            this.label13.Text = "LSL";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labUSL
            // 
            this.labUSL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labUSL.Location = new System.Drawing.Point(76, 200);
            this.labUSL.Name = "labUSL";
            this.labUSL.Size = new System.Drawing.Size(79, 22);
            this.labUSL.TabIndex = 12;
            this.labUSL.Text = "0";
            this.labUSL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label14.Location = new System.Drawing.Point(3, 200);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(73, 22);
            this.label14.TabIndex = 11;
            this.label14.Text = "USL";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labSum
            // 
            this.labSum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labSum.Location = new System.Drawing.Point(76, 156);
            this.labSum.Name = "labSum";
            this.labSum.Size = new System.Drawing.Size(79, 22);
            this.labSum.TabIndex = 10;
            this.labSum.Text = "0";
            this.labSum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labStd
            // 
            this.labStd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labStd.Location = new System.Drawing.Point(76, 178);
            this.labStd.Name = "labStd";
            this.labStd.Size = new System.Drawing.Size(79, 22);
            this.labStd.TabIndex = 10;
            this.labStd.Text = "0";
            this.labStd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labQ3
            // 
            this.labQ3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labQ3.Location = new System.Drawing.Point(76, 112);
            this.labQ3.Name = "labQ3";
            this.labQ3.Size = new System.Drawing.Size(79, 22);
            this.labQ3.TabIndex = 9;
            this.labQ3.Text = "0";
            this.labQ3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labMax
            // 
            this.labMax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labMax.Location = new System.Drawing.Point(76, 134);
            this.labMax.Name = "labMax";
            this.labMax.Size = new System.Drawing.Size(79, 22);
            this.labMax.TabIndex = 9;
            this.labMax.Text = "0";
            this.labMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labMin
            // 
            this.labMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labMin.Location = new System.Drawing.Point(76, 24);
            this.labMin.Name = "labMin";
            this.labMin.Size = new System.Drawing.Size(79, 22);
            this.labMin.TabIndex = 8;
            this.labMin.Text = "0";
            this.labMin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labCnt
            // 
            this.labCnt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labCnt.Location = new System.Drawing.Point(76, 2);
            this.labCnt.Name = "labCnt";
            this.labCnt.Size = new System.Drawing.Size(79, 22);
            this.labCnt.TabIndex = 7;
            this.labCnt.Text = "0";
            this.labCnt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labQ1
            // 
            this.labQ1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labQ1.Location = new System.Drawing.Point(76, 46);
            this.labQ1.Name = "labQ1";
            this.labQ1.Size = new System.Drawing.Size(79, 22);
            this.labQ1.TabIndex = 7;
            this.labQ1.Text = "0";
            this.labQ1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labMedian
            // 
            this.labMedian.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labMedian.Location = new System.Drawing.Point(76, 68);
            this.labMedian.Name = "labMedian";
            this.labMedian.Size = new System.Drawing.Size(79, 22);
            this.labMedian.TabIndex = 7;
            this.labMedian.Text = "0";
            this.labMedian.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labAvg
            // 
            this.labAvg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labAvg.Location = new System.Drawing.Point(76, 90);
            this.labAvg.Name = "labAvg";
            this.labAvg.Size = new System.Drawing.Size(79, 22);
            this.labAvg.TabIndex = 7;
            this.labAvg.Text = "0";
            this.labAvg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label12.Location = new System.Drawing.Point(3, 112);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(73, 22);
            this.label12.TabIndex = 3;
            this.label12.Text = "Q3";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(3, 156);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 22);
            this.label8.TabIndex = 2;
            this.label8.Text = "SUM";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(3, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 22);
            this.label4.TabIndex = 3;
            this.label4.Text = "MAX";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(3, 178);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 22);
            this.label3.TabIndex = 2;
            this.label3.Text = "STDEV";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(3, 2);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 22);
            this.label6.TabIndex = 0;
            this.label6.Text = "COUNT";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Location = new System.Drawing.Point(3, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 22);
            this.label7.TabIndex = 0;
            this.label7.Text = "Q1";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Location = new System.Drawing.Point(3, 68);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(73, 22);
            this.label10.TabIndex = 0;
            this.label10.Text = "MEDIAN";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(3, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 22);
            this.label2.TabIndex = 1;
            this.label2.Text = "MIN";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(3, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "AVERAGE";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chart);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 33);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1274, 285);
            this.panel2.TabIndex = 3;
            // 
            // chart
            // 
            this.chart.AntiAliasing = System.Windows.Forms.DataVisualization.Charting.AntiAliasingStyles.Graphics;
            this.chart.BackColor = System.Drawing.Color.WhiteSmoke;
            this.chart.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            this.chart.BackSecondaryColor = System.Drawing.Color.White;
            this.chart.BorderlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
            this.chart.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            this.chart.BorderSkin.SkinStyle = System.Windows.Forms.DataVisualization.Charting.BorderSkinStyle.Emboss;
            chartArea3.AxisX.IsStartedFromZero = false;
            chartArea3.AxisX.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea3.AxisX.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea3.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea3.AxisY.IsStartedFromZero = false;
            chartArea3.AxisY.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea3.AxisY.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea3.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea3.AxisY.Maximum = 100D;
            chartArea3.AxisY.Minimum = 0D;
            chartArea3.BackColor = System.Drawing.Color.WhiteSmoke;
            chartArea3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea3.Name = "Data Chart Area";
            chartArea3.Position.Auto = false;
            chartArea3.Position.Height = 82F;
            chartArea3.Position.Width = 88F;
            chartArea3.Position.Y = 12F;
            chartArea4.AlignmentOrientation = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal;
            chartArea4.AlignWithChartArea = "Data Chart Area";
            chartArea4.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea4.AxisX.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea4.AxisX.MajorGrid.Enabled = false;
            chartArea4.AxisX.Maximum = 10D;
            chartArea4.AxisX.Minimum = 0D;
            chartArea4.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea4.AxisY.IsStartedFromZero = false;
            chartArea4.AxisY.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea4.AxisY.MajorGrid.Enabled = false;
            chartArea4.AxisY.Maximum = 100D;
            chartArea4.AxisY.Minimum = 0D;
            chartArea4.BackColor = System.Drawing.Color.Transparent;
            chartArea4.BorderColor = System.Drawing.Color.Empty;
            chartArea4.Name = "Box Chart Area";
            chartArea4.Position.Auto = false;
            chartArea4.Position.Height = 82F;
            chartArea4.Position.Width = 5F;
            chartArea4.Position.X = 92F;
            chartArea4.Position.Y = 12F;
            this.chart.ChartAreas.Add(chartArea3);
            this.chart.ChartAreas.Add(chartArea4);
            this.chart.ContextMenuStrip = this.contextMenuStrip1;
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Enabled = false;
            legend2.Name = "Default";
            this.chart.Legends.Add(legend2);
            this.chart.Location = new System.Drawing.Point(0, 0);
            this.chart.Name = "chart";
            series4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            series4.ChartArea = "Data Chart Area";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series4.Legend = "Default";
            series4.MarkerSize = 8;
            series4.Name = "DataSeries";
            series4.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            series4.ShadowOffset = 1;
            series5.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.VerticalCenter;
            series5.BackSecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(224)))), ((int)(((byte)(64)))), ((int)(((byte)(10)))));
            series5.BorderColor = System.Drawing.Color.Black;
            series5.BorderWidth = 3;
            series5.ChartArea = "Box Chart Area";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.BoxPlot;
            series5.Color = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(64)))), ((int)(((byte)(10)))));
            series5.CustomProperties = "PixelPointWidth=70, BoxPlotSeries=DataSeries, PointWidth=1, BoxPlotShowUnusualVal" +
    "ues=True";
            series5.Legend = "Default";
            series5.Name = "BoxPlotSeries";
            series5.YValuesPerPoint = 6;
            series6.ChartArea = "Box Chart Area";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series6.CustomProperties = "LabelStyle=Right";
            series6.Legend = "Default";
            series6.Name = "BoxPlotLabels";
            dataPoint9.Color = System.Drawing.Color.Transparent;
            dataPoint10.Color = System.Drawing.Color.Transparent;
            dataPoint11.Color = System.Drawing.Color.Transparent;
            dataPoint12.Color = System.Drawing.Color.Transparent;
            dataPoint13.Color = System.Drawing.Color.Transparent;
            dataPoint14.Color = System.Drawing.Color.Transparent;
            dataPoint15.Color = System.Drawing.Color.Transparent;
            dataPoint16.Color = System.Drawing.Color.Transparent;
            series6.Points.Add(dataPoint9);
            series6.Points.Add(dataPoint10);
            series6.Points.Add(dataPoint11);
            series6.Points.Add(dataPoint12);
            series6.Points.Add(dataPoint13);
            series6.Points.Add(dataPoint14);
            series6.Points.Add(dataPoint15);
            series6.Points.Add(dataPoint16);
            series6.SmartLabelStyle.Enabled = false;
            series6.YValuesPerPoint = 6;
            this.chart.Series.Add(series4);
            this.chart.Series.Add(series5);
            this.chart.Series.Add(series6);
            this.chart.Size = new System.Drawing.Size(1116, 285);
            this.chart.TabIndex = 4;
            this.chart.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chart_KeyDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CopyImage,
            this.chartResetToolStripMenuItem,
            this.toolStripSeparator1,
            this.yAxisMaxMinToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(161, 76);
            // 
            // CopyImage
            // 
            this.CopyImage.Image = ((System.Drawing.Image)(resources.GetObject("CopyImage.Image")));
            this.CopyImage.Name = "CopyImage";
            this.CopyImage.Size = new System.Drawing.Size(160, 22);
            this.CopyImage.Text = "CopyImage";
            this.CopyImage.Click += new System.EventHandler(this.CopyImage_Click);
            // 
            // chartResetToolStripMenuItem
            // 
            this.chartResetToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("chartResetToolStripMenuItem.Image")));
            this.chartResetToolStripMenuItem.Name = "chartResetToolStripMenuItem";
            this.chartResetToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.chartResetToolStripMenuItem.Text = "Chart Reset";
            this.chartResetToolStripMenuItem.Click += new System.EventHandler(this.chartResetToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(157, 6);
            // 
            // yAxisMaxMinToolStripMenuItem
            // 
            this.yAxisMaxMinToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("yAxisMaxMinToolStripMenuItem.Image")));
            this.yAxisMaxMinToolStripMenuItem.Name = "yAxisMaxMinToolStripMenuItem";
            this.yAxisMaxMinToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.yAxisMaxMinToolStripMenuItem.Text = "Y Axis Max/Min";
            this.yAxisMaxMinToolStripMenuItem.Click += new System.EventHandler(this.yAxisMaxMinToolStripMenuItem_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.labLSL);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.label13);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.labUSL);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label14);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Controls.Add(this.labSum);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.labStd);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.labQ3);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.labMax);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.labMin);
            this.panel3.Controls.Add(this.label12);
            this.panel3.Controls.Add(this.labCnt);
            this.panel3.Controls.Add(this.labAvg);
            this.panel3.Controls.Add(this.labQ1);
            this.panel3.Controls.Add(this.labMedian);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(1116, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(158, 285);
            this.panel3.TabIndex = 5;
            // 
            // labTitle
            // 
            this.labTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labTitle.Location = new System.Drawing.Point(0, 0);
            this.labTitle.Name = "labTitle";
            this.labTitle.Size = new System.Drawing.Size(1274, 33);
            this.labTitle.TabIndex = 4;
            this.labTitle.Text = "TagID";
            this.labTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkSpec
            // 
            this.chkSpec.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSpec.AutoSize = true;
            this.chkSpec.ForeColor = System.Drawing.Color.Red;
            this.chkSpec.Location = new System.Drawing.Point(1184, 9);
            this.chkSpec.Name = "chkSpec";
            this.chkSpec.Size = new System.Drawing.Size(84, 16);
            this.chkSpec.TabIndex = 222;
            this.chkSpec.Text = "Spec Limit";
            this.chkSpec.UseVisualStyleBackColor = true;
            this.chkSpec.CheckedChanged += new System.EventHandler(this.chkSpec_CheckedChanged);
            // 
            // chkSigma
            // 
            this.chkSigma.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSigma.AutoSize = true;
            this.chkSigma.Checked = true;
            this.chkSigma.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSigma.ForeColor = System.Drawing.Color.DarkGray;
            this.chkSigma.Location = new System.Drawing.Point(1090, 9);
            this.chkSigma.Name = "chkSigma";
            this.chkSigma.Size = new System.Drawing.Size(88, 16);
            this.chkSigma.TabIndex = 222;
            this.chkSigma.Text = "Sigma Line";
            this.chkSigma.UseVisualStyleBackColor = true;
            this.chkSigma.CheckedChanged += new System.EventHandler(this.chkSigma_CheckedChanged);
            // 
            // CheckTrend
            // 
            this.CheckTrend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CheckTrend.AutoSize = true;
            this.CheckTrend.Checked = true;
            this.CheckTrend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckTrend.ForeColor = System.Drawing.Color.Green;
            this.CheckTrend.Location = new System.Drawing.Point(999, 9);
            this.CheckTrend.Name = "CheckTrend";
            this.CheckTrend.Size = new System.Drawing.Size(85, 16);
            this.CheckTrend.TabIndex = 222;
            this.CheckTrend.Text = "Trend Line";
            this.CheckTrend.UseVisualStyleBackColor = true;
            this.CheckTrend.CheckedChanged += new System.EventHandler(this.CheckTrend_CheckedChanged);
            // 
            // ucTrendChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.CheckTrend);
            this.Controls.Add(this.chkSigma);
            this.Controls.Add(this.chkSpec);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.labTitle);
            this.Name = "ucTrendChart";
            this.Size = new System.Drawing.Size(1274, 318);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labStd;
        private System.Windows.Forms.Label labMax;
        private System.Windows.Forms.Label labMin;
        private System.Windows.Forms.Label labAvg;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labSum;
        private System.Windows.Forms.Label labCnt;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labTitle;
        private System.Windows.Forms.Label labQ3;
        private System.Windows.Forms.Label labQ1;
        private System.Windows.Forms.Label labMedian;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem CopyImage;
        private System.Windows.Forms.ToolStripMenuItem chartResetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem yAxisMaxMinToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkSpec;
        private System.Windows.Forms.CheckBox chkSigma;
        private System.Windows.Forms.Label labLSL;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label labUSL;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.CheckBox CheckTrend;
        private System.Windows.Forms.Panel panel3;
    }
}
