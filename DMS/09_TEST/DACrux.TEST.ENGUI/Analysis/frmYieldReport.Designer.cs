namespace DACrux.TEST.ENGUI
{
    partial class frmYieldReport
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmYieldReport));
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.utlSelYield = new Infragistics.Win.Misc.UltraLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.utlDieInfo = new Infragistics.Win.Misc.UltraLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkY = new System.Windows.Forms.CheckBox();
            this.chkX = new System.Windows.Forms.CheckBox();
            this.radioButtonBoxPlot = new System.Windows.Forms.RadioButton();
            this.radioButtonStatistical = new System.Windows.Forms.RadioButton();
            this.radioButtonDay = new System.Windows.Forms.RadioButton();
            this.buttonToExcel = new System.Windows.Forms.Button();
            this.radioButtonLot = new System.Windows.Forms.RadioButton();
            this.radioButtonWafer = new System.Windows.Forms.RadioButton();
            this.fpSpread = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread_Sheet = new FarPoint.Win.Spread.SheetView();
            this.ultraPanel2.ClientArea.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Sheet)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraPanel2
            // 
            appearance1.ImageBackground = ((System.Drawing.Image)(resources.GetObject("appearance1.ImageBackground")));
            appearance1.ImageBackgroundStretchMargins = new Infragistics.Win.ImageBackgroundStretchMargins(35, 0, 600, 0);
            this.ultraPanel2.Appearance = appearance1;
            // 
            // ultraPanel2.ClientArea
            // 
            this.ultraPanel2.ClientArea.Controls.Add(this.utlSelYield);
            this.ultraPanel2.ClientArea.Controls.Add(this.label1);
            this.ultraPanel2.ClientArea.Controls.Add(this.utlDieInfo);
            this.ultraPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel2.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(804, 68);
            this.ultraPanel2.TabIndex = 25;
            // 
            // utlSelYield
            // 
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.utlSelYield.Appearance = appearance2;
            this.utlSelYield.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.utlSelYield.Location = new System.Drawing.Point(196, 32);
            this.utlSelYield.Name = "utlSelYield";
            this.utlSelYield.Size = new System.Drawing.Size(162, 35);
            this.utlSelYield.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(23, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(250, 36);
            this.label1.TabIndex = 19;
            this.label1.Text = "Yield Report";
            // 
            // utlDieInfo
            // 
            appearance3.BackColor = System.Drawing.Color.Transparent;
            this.utlDieInfo.Appearance = appearance3;
            this.utlDieInfo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.utlDieInfo.Location = new System.Drawing.Point(28, 32);
            this.utlDieInfo.Name = "utlDieInfo";
            this.utlDieInfo.Size = new System.Drawing.Size(162, 35);
            this.utlDieInfo.TabIndex = 18;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 68);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.chart1);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.fpSpread);
            this.splitContainer1.Size = new System.Drawing.Size(804, 384);
            this.splitContainer1.SplitterDistance = 279;
            this.splitContainer1.TabIndex = 26;
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.WhiteSmoke;
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.LabelStyle.Interval = 0D;
            chartArea1.AxisX.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea1.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.AxisX.MaximumAutoSize = 100F;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea1.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.Name = "ChartArea1";
            chartArea2.Name = "ChartArea2";
            chartArea2.Visible = false;
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.ChartAreas.Add(chartArea2);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart1.Location = new System.Drawing.Point(0, 29);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.IsVisibleInLegend = false;
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(804, 250);
            this.chart1.TabIndex = 14;
            this.chart1.Text = "chart1";
            title1.Name = "Title1";
            title1.Text = "YieldReport";
            this.chart1.Titles.Add(title1);
            this.chart1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseDown);
            this.chart1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseUp);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkY);
            this.panel1.Controls.Add(this.chkX);
            this.panel1.Controls.Add(this.radioButtonBoxPlot);
            this.panel1.Controls.Add(this.radioButtonStatistical);
            this.panel1.Controls.Add(this.radioButtonDay);
            this.panel1.Controls.Add(this.buttonToExcel);
            this.panel1.Controls.Add(this.radioButtonLot);
            this.panel1.Controls.Add(this.radioButtonWafer);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.panel1.Size = new System.Drawing.Size(804, 29);
            this.panel1.TabIndex = 13;
            // 
            // chkY
            // 
            this.chkY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkY.AutoSize = true;
            this.chkY.Location = new System.Drawing.Point(651, 7);
            this.chkY.Name = "chkY";
            this.chkY.Size = new System.Drawing.Size(67, 16);
            this.chkY.TabIndex = 16;
            this.chkY.Text = "Y Label";
            this.chkY.UseVisualStyleBackColor = true;
            this.chkY.CheckedChanged += new System.EventHandler(this.chkY_CheckedChanged);
            // 
            // chkX
            // 
            this.chkX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkX.AutoSize = true;
            this.chkX.Checked = true;
            this.chkX.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkX.Location = new System.Drawing.Point(578, 7);
            this.chkX.Name = "chkX";
            this.chkX.Size = new System.Drawing.Size(67, 16);
            this.chkX.TabIndex = 15;
            this.chkX.Text = "X Label";
            this.chkX.UseVisualStyleBackColor = true;
            this.chkX.CheckedChanged += new System.EventHandler(this.chkX_CheckedChanged);
            // 
            // radioButtonBoxPlot
            // 
            this.radioButtonBoxPlot.Location = new System.Drawing.Point(314, 2);
            this.radioButtonBoxPlot.Name = "radioButtonBoxPlot";
            this.radioButtonBoxPlot.Size = new System.Drawing.Size(70, 24);
            this.radioButtonBoxPlot.TabIndex = 13;
            this.radioButtonBoxPlot.Text = "Box Plot";
            this.radioButtonBoxPlot.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButtonStatistical
            // 
            this.radioButtonStatistical.Location = new System.Drawing.Point(239, 2);
            this.radioButtonStatistical.Name = "radioButtonStatistical";
            this.radioButtonStatistical.Size = new System.Drawing.Size(69, 24);
            this.radioButtonStatistical.TabIndex = 2;
            this.radioButtonStatistical.Text = "Scatter";
            this.radioButtonStatistical.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButtonDay
            // 
            this.radioButtonDay.Location = new System.Drawing.Point(167, 2);
            this.radioButtonDay.Name = "radioButtonDay";
            this.radioButtonDay.Size = new System.Drawing.Size(72, 24);
            this.radioButtonDay.TabIndex = 14;
            this.radioButtonDay.Text = "Day";
            this.radioButtonDay.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // buttonToExcel
            // 
            this.buttonToExcel.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonToExcel.Image = ((System.Drawing.Image)(resources.GetObject("buttonToExcel.Image")));
            this.buttonToExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonToExcel.Location = new System.Drawing.Point(724, 0);
            this.buttonToExcel.Name = "buttonToExcel";
            this.buttonToExcel.Size = new System.Drawing.Size(80, 29);
            this.buttonToExcel.TabIndex = 12;
            this.buttonToExcel.Text = "    ToExcel";
            this.buttonToExcel.Click += new System.EventHandler(this.buttonToExcel_Click);
            // 
            // radioButtonLot
            // 
            this.radioButtonLot.Location = new System.Drawing.Point(89, 2);
            this.radioButtonLot.Name = "radioButtonLot";
            this.radioButtonLot.Size = new System.Drawing.Size(72, 24);
            this.radioButtonLot.TabIndex = 1;
            this.radioButtonLot.Text = "Lot";
            this.radioButtonLot.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButtonWafer
            // 
            this.radioButtonWafer.Location = new System.Drawing.Point(11, 2);
            this.radioButtonWafer.Name = "radioButtonWafer";
            this.radioButtonWafer.Size = new System.Drawing.Size(72, 24);
            this.radioButtonWafer.TabIndex = 0;
            this.radioButtonWafer.Text = "Wafer";
            this.radioButtonWafer.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // fpSpread
            // 
            this.fpSpread.AccessibleDescription = "";
            this.fpSpread.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpread.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpSpread.Location = new System.Drawing.Point(0, 0);
            this.fpSpread.Name = "fpSpread";
            this.fpSpread.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.fpSpread.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread_Sheet});
            this.fpSpread.Size = new System.Drawing.Size(804, 101);
            this.fpSpread.TabIndex = 11;
            this.fpSpread.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // fpSpread_Sheet
            // 
            this.fpSpread_Sheet.Reset();
            fpSpread_Sheet.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpread_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpSpread_Sheet.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Classic2;
            this.fpSpread_Sheet.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Sheet.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Sheet.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Sheet.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Sheet.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpread_Sheet.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Sheet.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Sheet.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Sheet.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Sheet.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Sheet.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpread_Sheet.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Sheet.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Sheet.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Sheet.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Sheet.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Sheet.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpread_Sheet.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Sheet.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Sheet.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpSpread_Sheet.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpSpread_Sheet.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Sheet.DefaultStyle.Parent = "DataAreaDefault";
            this.fpSpread_Sheet.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Sheet.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Sheet.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Sheet.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Sheet.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpread_Sheet.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Sheet.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Sheet.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Sheet.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Sheet.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Sheet.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpread_Sheet.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmYieldReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 452);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.ultraPanel2);
            this.Name = "frmYieldReport";
            this.Text = "Yield Report";
            this.Load += new System.EventHandler(this.frmYieldReport_Load);
            this.ultraPanel2.ClientArea.ResumeLayout(false);
            this.ultraPanel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Sheet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.Misc.UltraLabel utlSelYield;
        private System.Windows.Forms.Label label1;
        private Infragistics.Win.Misc.UltraLabel utlDieInfo;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButtonBoxPlot;
        private System.Windows.Forms.RadioButton radioButtonStatistical;
        private System.Windows.Forms.RadioButton radioButtonDay;
        private System.Windows.Forms.Button buttonToExcel;
        private System.Windows.Forms.RadioButton radioButtonLot;
        private System.Windows.Forms.RadioButton radioButtonWafer;
        private FarPoint.Win.Spread.FpSpread fpSpread;
        private FarPoint.Win.Spread.SheetView fpSpread_Sheet;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.CheckBox chkY;
        private System.Windows.Forms.CheckBox chkX;
    }
}