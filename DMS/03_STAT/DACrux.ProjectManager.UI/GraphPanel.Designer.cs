namespace DACrux.ProjectManager.UI
{
    partial class GraphPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphPanel));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolBtnImageCopy = new System.Windows.Forms.ToolStripButton();
            this.toolSepLabelAngle = new System.Windows.Forms.ToolStripSeparator();
            this.toolLblAngle = new System.Windows.Forms.ToolStripLabel();
            this.toolCboAngle = new System.Windows.Forms.ToolStripComboBox();
            this.toolSepPointLabel = new System.Windows.Forms.ToolStripSeparator();
            this.toolBtnPointLabel = new System.Windows.Forms.ToolStripButton();
            this.toolBtnLegendBox = new System.Windows.Forms.ToolStripButton();
            this.toolBtn3D = new System.Windows.Forms.ToolStripButton();
            this.toolSepZoom = new System.Windows.Forms.ToolStripSeparator();
            this.toolBtnZoom = new System.Windows.Forms.ToolStripButton();
            this.toolSepProperty = new System.Windows.Forms.ToolStripSeparator();
            this.toolBtnProperty = new System.Windows.Forms.ToolStripButton();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.spcHistogram = new DACrux.SPC.Visualization.Histogram();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.AutoSize = false;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBtnImageCopy,
            this.toolSepLabelAngle,
            this.toolLblAngle,
            this.toolCboAngle,
            this.toolSepPointLabel,
            this.toolBtnPointLabel,
            this.toolBtnLegendBox,
            this.toolBtn3D,
            this.toolSepZoom,
            this.toolBtnZoom,
            this.toolSepProperty,
            this.toolBtnProperty});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(800, 19);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolBtnImageCopy
            // 
            this.toolBtnImageCopy.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnImageCopy.Image")));
            this.toolBtnImageCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnImageCopy.Name = "toolBtnImageCopy";
            this.toolBtnImageCopy.Size = new System.Drawing.Size(92, 16);
            this.toolBtnImageCopy.Text = "Image Copy";
            this.toolBtnImageCopy.Click += new System.EventHandler(this.toolBtnImageCopy_Click);
            // 
            // toolSepLabelAngle
            // 
            this.toolSepLabelAngle.Name = "toolSepLabelAngle";
            this.toolSepLabelAngle.Size = new System.Drawing.Size(6, 19);
            // 
            // toolLblAngle
            // 
            this.toolLblAngle.Image = ((System.Drawing.Image)(resources.GetObject("toolLblAngle.Image")));
            this.toolLblAngle.Name = "toolLblAngle";
            this.toolLblAngle.Size = new System.Drawing.Size(86, 16);
            this.toolLblAngle.Text = "Label Angle";
            // 
            // toolCboAngle
            // 
            this.toolCboAngle.AutoSize = false;
            this.toolCboAngle.Items.AddRange(new object[] {
            "Horizontal",
            "Diagonal",
            "Vertical"});
            this.toolCboAngle.Name = "toolCboAngle";
            this.toolCboAngle.Size = new System.Drawing.Size(85, 23);
            this.toolCboAngle.SelectedIndexChanged += new System.EventHandler(this.toolCboAngle_SelectedIndexChanged);
            // 
            // toolSepPointLabel
            // 
            this.toolSepPointLabel.Name = "toolSepPointLabel";
            this.toolSepPointLabel.Size = new System.Drawing.Size(6, 19);
            // 
            // toolBtnPointLabel
            // 
            this.toolBtnPointLabel.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnPointLabel.Image")));
            this.toolBtnPointLabel.ImageTransparentColor = System.Drawing.Color.White;
            this.toolBtnPointLabel.Name = "toolBtnPointLabel";
            this.toolBtnPointLabel.Size = new System.Drawing.Size(87, 16);
            this.toolBtnPointLabel.Text = "Point Label";
            this.toolBtnPointLabel.Click += new System.EventHandler(this.toolBtnPointLabel_Click);
            // 
            // toolBtnLegendBox
            // 
            this.toolBtnLegendBox.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnLegendBox.Image")));
            this.toolBtnLegendBox.ImageTransparentColor = System.Drawing.Color.AliceBlue;
            this.toolBtnLegendBox.Name = "toolBtnLegendBox";
            this.toolBtnLegendBox.Size = new System.Drawing.Size(90, 16);
            this.toolBtnLegendBox.Text = "Legend Box";
            this.toolBtnLegendBox.Click += new System.EventHandler(this.toolBtnLegendBox_Click);
            // 
            // toolBtn3D
            // 
            this.toolBtn3D.Image = ((System.Drawing.Image)(resources.GetObject("toolBtn3D.Image")));
            this.toolBtn3D.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtn3D.Name = "toolBtn3D";
            this.toolBtn3D.Size = new System.Drawing.Size(43, 16);
            this.toolBtn3D.Text = "3D";
            this.toolBtn3D.Click += new System.EventHandler(this.toolBtn3D_Click);
            // 
            // toolSepZoom
            // 
            this.toolSepZoom.Name = "toolSepZoom";
            this.toolSepZoom.Size = new System.Drawing.Size(6, 19);
            // 
            // toolBtnZoom
            // 
            this.toolBtnZoom.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnZoom.Image")));
            this.toolBtnZoom.ImageTransparentColor = System.Drawing.Color.White;
            this.toolBtnZoom.Name = "toolBtnZoom";
            this.toolBtnZoom.Size = new System.Drawing.Size(59, 16);
            this.toolBtnZoom.Text = "Zoom";
            this.toolBtnZoom.Click += new System.EventHandler(this.toolBtnZoom_Click);
            // 
            // toolSepProperty
            // 
            this.toolSepProperty.Name = "toolSepProperty";
            this.toolSepProperty.Size = new System.Drawing.Size(6, 19);
            // 
            // toolBtnProperty
            // 
            this.toolBtnProperty.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnProperty.Image")));
            this.toolBtnProperty.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnProperty.Name = "toolBtnProperty";
            this.toolBtnProperty.Size = new System.Drawing.Size(72, 16);
            this.toolBtnProperty.Text = "Property";
            this.toolBtnProperty.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.toolBtnProperty.Click += new System.EventHandler(this.toolBtnProperty_Click);
            // 
            // chart
            // 
            chartArea1.Area3DStyle.Enable3D = true;
            chartArea1.CursorX.IsUserSelectionEnabled = true;
            chartArea1.CursorY.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.CursorY.IsUserEnabled = true;
            chartArea1.CursorY.IsUserSelectionEnabled = true;
            chartArea1.Name = "ChartArea1";
            chartArea1.Visible = false;
            this.chart.ChartAreas.Add(chartArea1);
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.BackColor = System.Drawing.Color.Cornsilk;
            legend1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            legend1.Name = "Legend1";
            this.chart.Legends.Add(legend1);
            this.chart.Location = new System.Drawing.Point(0, 19);
            this.chart.Name = "chart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
            series1.IsValueShownAsLabel = true;
            series1.IsXValueIndexed = true;
            series1.Legend = "Legend1";
            series1.MarkerSize = 10;
            series1.Name = "Series1";
            this.chart.Series.Add(series1);
            this.chart.Size = new System.Drawing.Size(800, 581);
            this.chart.TabIndex = 3;
            this.chart.Text = "chart1";
            this.chart.PostPaint += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ChartPaintEventArgs>(this.chart_PostPaint);
            // 
            // spcHistogram
            // 
            this.spcHistogram.DataSource = null;
            this.spcHistogram.Location = new System.Drawing.Point(413, 118);
            this.spcHistogram.LSL = double.NaN;
            this.spcHistogram.MainTitle = "Histogram";
            this.spcHistogram.Name = "spcHistogram";
            this.spcHistogram.SegmentIntervalNumber = 10;
            this.spcHistogram.SegmentIntervalWidth = 15D;
            this.spcHistogram.ShowPercentOnSecondaryYAxis = true;
            this.spcHistogram.Size = new System.Drawing.Size(329, 326);
            this.spcHistogram.TabIndex = 4;
            this.spcHistogram.Target = double.NaN;
            this.spcHistogram.USL = double.NaN;
            // 
            // GraphPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.spcHistogram);
            this.Controls.Add(this.chart);
            this.Controls.Add(this.toolStrip);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "GraphPanel";
            this.Size = new System.Drawing.Size(800, 600);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolBtnImageCopy;
        private System.Windows.Forms.ToolStripSeparator toolSepLabelAngle;
        private System.Windows.Forms.ToolStripLabel toolLblAngle;
        private System.Windows.Forms.ToolStripSeparator toolSepPointLabel;
        private System.Windows.Forms.ToolStripComboBox toolCboAngle;
        private System.Windows.Forms.ToolStripSeparator toolSepZoom;
        private System.Windows.Forms.ToolStripButton toolBtn3D;
        private System.Windows.Forms.ToolStripSeparator toolSepProperty;
        private System.Windows.Forms.ToolStripButton toolBtnZoom;
        private System.Windows.Forms.ToolStripButton toolBtnLegendBox;
        private System.Windows.Forms.ToolStripButton toolBtnProperty;
        private System.Windows.Forms.ToolStripButton toolBtnPointLabel;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private SPC.Visualization.Histogram spcHistogram;

    }
}
