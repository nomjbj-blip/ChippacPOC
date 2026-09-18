namespace DACrux.TEST.ENGUI
{
    partial class frmBinStackBar
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
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBinStackBar));
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.utlSelYield = new Infragistics.Win.Misc.UltraLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.utlDieInfo = new Infragistics.Win.Misc.UltraLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkX = new System.Windows.Forms.CheckBox();
            this.buttonToExcel = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.fpSpread = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread_Sheet = new FarPoint.Win.Spread.SheetView();
            this.ultraPanel2.ClientArea.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Sheet)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraPanel2
            // 
            appearance4.ImageBackground = ((System.Drawing.Image)(resources.GetObject("appearance4.ImageBackground")));
            appearance4.ImageBackgroundStretchMargins = new Infragistics.Win.ImageBackgroundStretchMargins(35, 0, 600, 0);
            this.ultraPanel2.Appearance = appearance4;
            // 
            // ultraPanel2.ClientArea
            // 
            this.ultraPanel2.ClientArea.Controls.Add(this.utlSelYield);
            this.ultraPanel2.ClientArea.Controls.Add(this.label1);
            this.ultraPanel2.ClientArea.Controls.Add(this.utlDieInfo);
            this.ultraPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel2.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(1038, 68);
            this.ultraPanel2.TabIndex = 26;
            // 
            // utlSelYield
            // 
            appearance5.BackColor = System.Drawing.Color.Transparent;
            this.utlSelYield.Appearance = appearance5;
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
            this.label1.Text = "Bin Stack Bar";
            // 
            // utlDieInfo
            // 
            appearance6.BackColor = System.Drawing.Color.Transparent;
            this.utlDieInfo.Appearance = appearance6;
            this.utlDieInfo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.utlDieInfo.Location = new System.Drawing.Point(28, 32);
            this.utlDieInfo.Name = "utlDieInfo";
            this.utlDieInfo.Size = new System.Drawing.Size(162, 35);
            this.utlDieInfo.TabIndex = 18;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkX);
            this.panel1.Controls.Add(this.buttonToExcel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 68);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1038, 28);
            this.panel1.TabIndex = 27;
            // 
            // chkX
            // 
            this.chkX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkX.AutoSize = true;
            this.chkX.Checked = true;
            this.chkX.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkX.Location = new System.Drawing.Point(886, 6);
            this.chkX.Name = "chkX";
            this.chkX.Size = new System.Drawing.Size(67, 16);
            this.chkX.TabIndex = 16;
            this.chkX.Text = "X Label";
            this.chkX.UseVisualStyleBackColor = true;
            this.chkX.CheckedChanged += new System.EventHandler(this.chkX_CheckedChanged);
            // 
            // buttonToExcel
            // 
            this.buttonToExcel.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonToExcel.Image = ((System.Drawing.Image)(resources.GetObject("buttonToExcel.Image")));
            this.buttonToExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonToExcel.Location = new System.Drawing.Point(958, 0);
            this.buttonToExcel.Name = "buttonToExcel";
            this.buttonToExcel.Size = new System.Drawing.Size(80, 28);
            this.buttonToExcel.TabIndex = 13;
            this.buttonToExcel.Text = "    ToExcel";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 96);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.chart1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.fpSpread);
            this.splitContainer1.Size = new System.Drawing.Size(1038, 466);
            this.splitContainer1.SplitterDistance = 313;
            this.splitContainer1.TabIndex = 28;
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.WhiteSmoke;
            chartArea2.AxisX.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea2.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea2.AxisY.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea2.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea2.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea2);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn100;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(1038, 313);
            this.chart1.TabIndex = 5;
            this.chart1.Text = "chart1";
            this.chart1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseDown);
            this.chart1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseMove);
            this.chart1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseUp);
            // 
            // fpSpread
            // 
            this.fpSpread.AccessibleDescription = "";
            this.fpSpread.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpread.Location = new System.Drawing.Point(0, 0);
            this.fpSpread.Name = "fpSpread";
            this.fpSpread.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread_Sheet});
            this.fpSpread.Size = new System.Drawing.Size(1038, 149);
            this.fpSpread.TabIndex = 2;
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
            // frmBinStackBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1038, 562);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ultraPanel2);
            this.Name = "frmBinStackBar";
            this.Text = "Bin Stack Bar";
            this.Load += new System.EventHandler(this.frmBinStackBar_Load);
            this.ultraPanel2.ClientArea.ResumeLayout(false);
            this.ultraPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Sheet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.Misc.UltraLabel utlSelYield;
        private System.Windows.Forms.Label label1;
        private Infragistics.Win.Misc.UltraLabel utlDieInfo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonToExcel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private FarPoint.Win.Spread.FpSpread fpSpread;
        private FarPoint.Win.Spread.SheetView fpSpread_Sheet;
        private System.Windows.Forms.CheckBox chkX;
    }
}