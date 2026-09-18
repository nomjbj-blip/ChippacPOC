namespace DACrux.TEST.ENGUI
{
    partial class frmWaferParaAnalysis
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWaferParaAnalysis));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.histogram1 = new DACrux.SPC.Visualization.Histogram();
            this.lbMessage = new System.Windows.Forms.Label();
            this.fpSpread1 = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.btnAnalysis = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlTitle = new System.Windows.Forms.Panel();
            this.rdoCL = new System.Windows.Forms.RadioButton();
            this.ChkLimit = new System.Windows.Forms.CheckBox();
            this.rdoCS = new System.Windows.Forms.RadioButton();
            this.rdoCB = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRedraw = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).BeginInit();
            this.pnlTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.chart1);
            this.splitContainer1.Panel1.Controls.Add(this.splitter1);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.fpSpread1);
            this.splitContainer1.Panel2.Controls.Add(this.btnAnalysis);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(1029, 410);
            this.splitContainer1.SplitterDistance = 283;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 0;
            // 
            // chart1
            // 
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.Name = "Default";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Cursor = System.Windows.Forms.Cursors.Cross;
            this.chart1.DataSource = this.chart1.Series;
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart1.ImeMode = System.Windows.Forms.ImeMode.Katakana;
            this.chart1.Location = new System.Drawing.Point(502, 0);
            this.chart1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chart1.Name = "chart1";
            series1.ChartArea = "Default";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(527, 283);
            this.chart1.TabIndex = 9;
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.Color.LightSlateGray;
            this.splitter1.Location = new System.Drawing.Point(497, 0);
            this.splitter1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(5, 283);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.histogram1);
            this.panel1.Controls.Add(this.lbMessage);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(497, 283);
            this.panel1.TabIndex = 10;
            // 
            // histogram1
            // 
            this.histogram1.CPKLabel = false;
            this.histogram1.DataSource = null;
            this.histogram1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.histogram1.HistogramVisible = true;
            this.histogram1.Location = new System.Drawing.Point(0, 18);
            this.histogram1.LSL = double.NaN;
            this.histogram1.MainTitle = "Histogram";
            this.histogram1.Name = "histogram1";
            this.histogram1.SegmentIntervalNumber = 20;
            this.histogram1.SegmentIntervalWidth = 15D;
            this.histogram1.ShowPercentOnSecondaryYAxis = true;
            this.histogram1.Size = new System.Drawing.Size(497, 265);
            this.histogram1.TabIndex = 0;
            this.histogram1.Target = double.NaN;
            this.histogram1.USL = double.NaN;
            // 
            // lbMessage
            // 
            this.lbMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbMessage.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbMessage.Location = new System.Drawing.Point(0, 0);
            this.lbMessage.Name = "lbMessage";
            this.lbMessage.Size = new System.Drawing.Size(497, 18);
            this.lbMessage.TabIndex = 1;
            this.lbMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // fpSpread1
            // 
            this.fpSpread1.AccessibleDescription = "";
            this.fpSpread1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpread1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpSpread1.Location = new System.Drawing.Point(0, 22);
            this.fpSpread1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.fpSpread1.Name = "fpSpread1";
            this.fpSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread1_Sheet1});
            this.fpSpread1.Size = new System.Drawing.Size(1029, 102);
            this.fpSpread1.TabIndex = 71;
            this.fpSpread1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpSpread1.SetActiveViewport(0, -1, -1);
            // 
            // fpSpread1_Sheet1
            // 
            this.fpSpread1_Sheet1.Reset();
            fpSpread1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpSpread1_Sheet1.ColumnCount = 0;
            fpSpread1_Sheet1.RowCount = 0;
            this.fpSpread1_Sheet1.ActiveColumnIndex = -1;
            this.fpSpread1_Sheet1.ActiveRowIndex = -1;
            this.fpSpread1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // btnAnalysis
            // 
            this.btnAnalysis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAnalysis.Image = ((System.Drawing.Image)(resources.GetObject("btnAnalysis.Image")));
            this.btnAnalysis.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAnalysis.Location = new System.Drawing.Point(939, 4);
            this.btnAnalysis.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAnalysis.Name = "btnAnalysis";
            this.btnAnalysis.Size = new System.Drawing.Size(81, 18);
            this.btnAnalysis.TabIndex = 69;
            this.btnAnalysis.Text = "Analysis";
            this.btnAnalysis.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAnalysis.UseVisualStyleBackColor = true;
            this.btnAnalysis.Visible = false;
            this.btnAnalysis.Click += new System.EventHandler(this.btnAnalysis_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightSlateGray;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1029, 22);
            this.label1.TabIndex = 68;
            this.label1.Text = "     All Para Data";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlTitle
            // 
            this.pnlTitle.BackColor = System.Drawing.SystemColors.Menu;
            this.pnlTitle.Controls.Add(this.rdoCL);
            this.pnlTitle.Controls.Add(this.ChkLimit);
            this.pnlTitle.Controls.Add(this.rdoCS);
            this.pnlTitle.Controls.Add(this.rdoCB);
            this.pnlTitle.Controls.Add(this.label3);
            this.pnlTitle.Controls.Add(this.btnRedraw);
            this.pnlTitle.Controls.Add(this.label6);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(1029, 24);
            this.pnlTitle.TabIndex = 2;
            // 
            // rdoCL
            // 
            this.rdoCL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdoCL.BackColor = System.Drawing.Color.LightSlateGray;
            this.rdoCL.Location = new System.Drawing.Point(518, 2);
            this.rdoCL.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdoCL.Name = "rdoCL";
            this.rdoCL.Size = new System.Drawing.Size(54, 19);
            this.rdoCL.TabIndex = 88;
            this.rdoCL.Text = "Line";
            this.rdoCL.UseVisualStyleBackColor = false;
            this.rdoCL.CheckedChanged += new System.EventHandler(this.rdoChartType_CheckedChanged);
            // 
            // ChkLimit
            // 
            this.ChkLimit.AutoSize = true;
            this.ChkLimit.BackColor = System.Drawing.Color.LightSlateGray;
            this.ChkLimit.Location = new System.Drawing.Point(194, 5);
            this.ChkLimit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ChkLimit.Name = "ChkLimit";
            this.ChkLimit.Size = new System.Drawing.Size(112, 16);
            this.ChkLimit.TabIndex = 90;
            this.ChkLimit.Text = "Histogram Limit";
            this.ChkLimit.UseVisualStyleBackColor = false;
            this.ChkLimit.Visible = false;
            // 
            // rdoCS
            // 
            this.rdoCS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdoCS.BackColor = System.Drawing.Color.LightSlateGray;
            this.rdoCS.Checked = true;
            this.rdoCS.Location = new System.Drawing.Point(584, 2);
            this.rdoCS.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdoCS.Name = "rdoCS";
            this.rdoCS.Size = new System.Drawing.Size(65, 19);
            this.rdoCS.TabIndex = 89;
            this.rdoCS.TabStop = true;
            this.rdoCS.Text = "Scatter";
            this.rdoCS.UseVisualStyleBackColor = false;
            this.rdoCS.CheckedChanged += new System.EventHandler(this.rdoChartType_CheckedChanged);
            // 
            // rdoCB
            // 
            this.rdoCB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdoCB.BackColor = System.Drawing.Color.LightSlateGray;
            this.rdoCB.Location = new System.Drawing.Point(452, 2);
            this.rdoCB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdoCB.Name = "rdoCB";
            this.rdoCB.Size = new System.Drawing.Size(66, 19);
            this.rdoCB.TabIndex = 87;
            this.rdoCB.Text = "Bar";
            this.rdoCB.UseVisualStyleBackColor = false;
            this.rdoCB.CheckedChanged += new System.EventHandler(this.rdoChartType_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.LightSlateGray;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(655, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 12);
            this.label3.TabIndex = 71;
            this.label3.Text = "   Test Item";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnRedraw
            // 
            this.btnRedraw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRedraw.Image = ((System.Drawing.Image)(resources.GetObject("btnRedraw.Image")));
            this.btnRedraw.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRedraw.Location = new System.Drawing.Point(939, 2);
            this.btnRedraw.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRedraw.Name = "btnRedraw";
            this.btnRedraw.Size = new System.Drawing.Size(81, 18);
            this.btnRedraw.TabIndex = 70;
            this.btnRedraw.Text = "Redraw";
            this.btnRedraw.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRedraw.UseVisualStyleBackColor = true;
            this.btnRedraw.Visible = false;
            this.btnRedraw.Click += new System.EventHandler(this.btnRedraw_Click);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.LightSlateGray;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Image = ((System.Drawing.Image)(resources.GetObject("label6.Image")));
            this.label6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(1029, 24);
            this.label6.TabIndex = 59;
            this.label6.Text = "     Measure Para Data Analysis";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmWaferParaAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1029, 434);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.pnlTitle);
            this.Name = "frmWaferParaAnalysis";
            this.Text = "Measure Para Report";
            this.Load += new System.EventHandler(this.frmWaferParaAnalysis_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).EndInit();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel pnlTitle;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAnalysis;
        private FarPoint.Win.Spread.FpSpread fpSpread1;
        private FarPoint.Win.Spread.SheetView fpSpread1_Sheet1;
        private System.Windows.Forms.Button btnRedraw;
        private System.Windows.Forms.Splitter splitter1;
        private SPC.Visualization.Histogram histogram1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rdoCB;
        private System.Windows.Forms.RadioButton rdoCL;
        private System.Windows.Forms.RadioButton rdoCS;
        private System.Windows.Forms.CheckBox ChkLimit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbMessage;
    }
}