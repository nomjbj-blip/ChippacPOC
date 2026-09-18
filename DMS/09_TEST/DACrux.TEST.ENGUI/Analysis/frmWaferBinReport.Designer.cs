namespace DACrux.TEST.ENGUI
{
    partial class frmWaferBinReport
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer1 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer2 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkRate = new System.Windows.Forms.CheckBox();
            this.rbtLot = new System.Windows.Forms.RadioButton();
            this.rbtWafer = new System.Windows.Forms.RadioButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.BinChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sizeResetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chartConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.legendList1 = new DACrux.TEST.ENGUI.DataSelecter.LegendList();
            this.fpSpread = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread_Sheet = new FarPoint.Win.Spread.SheetView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BinChart)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Sheet)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.chkRate);
            this.panel1.Controls.Add(this.rbtLot);
            this.panel1.Controls.Add(this.rbtWafer);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1038, 28);
            this.panel1.TabIndex = 27;
            // 
            // chkRate
            // 
            this.chkRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkRate.AutoSize = true;
            this.chkRate.Location = new System.Drawing.Point(944, 6);
            this.chkRate.Name = "chkRate";
            this.chkRate.Size = new System.Drawing.Size(85, 16);
            this.chkRate.TabIndex = 15;
            this.chkRate.Text = "Rate Mode";
            this.chkRate.UseVisualStyleBackColor = true;
            this.chkRate.CheckedChanged += new System.EventHandler(this.chkRate_CheckedChanged);
            // 
            // rbtLot
            // 
            this.rbtLot.AutoSize = true;
            this.rbtLot.Location = new System.Drawing.Point(100, 6);
            this.rbtLot.Name = "rbtLot";
            this.rbtLot.Size = new System.Drawing.Size(73, 16);
            this.rbtLot.TabIndex = 14;
            this.rbtLot.Text = "Lot Base";
            this.rbtLot.UseVisualStyleBackColor = true;
            this.rbtLot.CheckedChanged += new System.EventHandler(this.rbtLot_CheckedChanged);
            // 
            // rbtWafer
            // 
            this.rbtWafer.AutoSize = true;
            this.rbtWafer.Checked = true;
            this.rbtWafer.Location = new System.Drawing.Point(7, 6);
            this.rbtWafer.Name = "rbtWafer";
            this.rbtWafer.Size = new System.Drawing.Size(87, 16);
            this.rbtWafer.TabIndex = 14;
            this.rbtWafer.TabStop = true;
            this.rbtWafer.Text = "Wafer Base";
            this.rbtWafer.UseVisualStyleBackColor = true;
            this.rbtWafer.CheckedChanged += new System.EventHandler(this.rbtWafer_CheckedChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 28);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.BinChart);
            this.splitContainer1.Panel1.Controls.Add(this.splitter1);
            this.splitContainer1.Panel1.Controls.Add(this.legendList1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.fpSpread);
            this.splitContainer1.Size = new System.Drawing.Size(1038, 534);
            this.splitContainer1.SplitterDistance = 358;
            this.splitContainer1.TabIndex = 28;
            // 
            // BinChart
            // 
            this.BinChart.BackColor = System.Drawing.Color.WhiteSmoke;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea1.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea1.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.Name = "Default";
            this.BinChart.ChartAreas.Add(chartArea1);
            this.BinChart.ContextMenuStrip = this.contextMenuStrip;
            this.BinChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BinChart.Location = new System.Drawing.Point(0, 0);
            this.BinChart.Name = "BinChart";
            series1.ChartArea = "Default";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn100;
            series1.Name = "Series1";
            this.BinChart.Series.Add(series1);
            this.BinChart.Size = new System.Drawing.Size(804, 356);
            this.BinChart.TabIndex = 5;
            this.BinChart.Text = "chart1";
            this.BinChart.GetToolTipText += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs>(this.BinChart_GetToolTipText);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sizeResetToolStripMenuItem,
            this.chartConfigToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(144, 48);
            // 
            // sizeResetToolStripMenuItem
            // 
            this.sizeResetToolStripMenuItem.Name = "sizeResetToolStripMenuItem";
            this.sizeResetToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.sizeResetToolStripMenuItem.Text = "Size Reset";
            this.sizeResetToolStripMenuItem.Click += new System.EventHandler(this.sizeResetToolStripMenuItem_Click);
            // 
            // chartConfigToolStripMenuItem
            // 
            this.chartConfigToolStripMenuItem.Name = "chartConfigToolStripMenuItem";
            this.chartConfigToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.chartConfigToolStripMenuItem.Text = "Chart Config";
            this.chartConfigToolStripMenuItem.Click += new System.EventHandler(this.chartConfigToolStripMenuItem_Click);
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(804, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(4, 356);
            this.splitter1.TabIndex = 7;
            this.splitter1.TabStop = false;
            // 
            // legendList1
            // 
            this.legendList1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.legendList1.Dock = System.Windows.Forms.DockStyle.Right;
            this.legendList1.Location = new System.Drawing.Point(808, 0);
            this.legendList1.Name = "legendList1";
            this.legendList1.Size = new System.Drawing.Size(228, 356);
            this.legendList1.TabIndex = 6;
            this.legendList1.VisibleCheckedChanged += new DACrux.TEST.ENGUI.DataSelecter.LegendList.ItemCheckedChangedEventHandler(this.legendList1_VisibleCheckedChanged);
            this.legendList1.ShowLabelCheckedChanged += new DACrux.TEST.ENGUI.DataSelecter.LegendList.ItemCheckedChangedEventHandler(this.legendList1_ShowLabelCheckedChanged);
            // 
            // fpSpread
            // 
            this.fpSpread.AccessibleDescription = "";
            this.fpSpread.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fpSpread.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpread.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpSpread.HorizontalScrollBar.Name = "";
            enhancedScrollBarRenderer1.ArrowColor = System.Drawing.Color.Black;
            enhancedScrollBarRenderer1.ArrowHoveredColor = System.Drawing.Color.Black;
            enhancedScrollBarRenderer1.ArrowSelectedColor = System.Drawing.Color.Black;
            enhancedScrollBarRenderer1.ButtonBackgroundColor = System.Drawing.Color.Silver;
            enhancedScrollBarRenderer1.ButtonBorderColor = System.Drawing.Color.DarkGray;
            enhancedScrollBarRenderer1.ButtonHoveredBackgroundColor = System.Drawing.Color.DarkGray;
            enhancedScrollBarRenderer1.ButtonHoveredBorderColor = System.Drawing.Color.DimGray;
            enhancedScrollBarRenderer1.ButtonSelectedBackgroundColor = System.Drawing.Color.DimGray;
            enhancedScrollBarRenderer1.ButtonSelectedBorderColor = System.Drawing.Color.Gray;
            enhancedScrollBarRenderer1.TrackBarBackgroundColor = System.Drawing.Color.DarkGray;
            enhancedScrollBarRenderer1.TrackBarSelectedBackgroundColor = System.Drawing.Color.Gray;
            this.fpSpread.HorizontalScrollBar.Renderer = enhancedScrollBarRenderer1;
            this.fpSpread.HorizontalScrollBar.TabIndex = 4;
            this.fpSpread.Location = new System.Drawing.Point(0, 0);
            this.fpSpread.Name = "fpSpread";
            this.fpSpread.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread_Sheet});
            this.fpSpread.Size = new System.Drawing.Size(1036, 170);
            this.fpSpread.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Metallic;
            this.fpSpread.TabIndex = 2;
            this.fpSpread.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpSpread.VerticalScrollBar.Name = "";
            enhancedScrollBarRenderer2.ArrowColor = System.Drawing.Color.Black;
            enhancedScrollBarRenderer2.ArrowHoveredColor = System.Drawing.Color.Black;
            enhancedScrollBarRenderer2.ArrowSelectedColor = System.Drawing.Color.Black;
            enhancedScrollBarRenderer2.ButtonBackgroundColor = System.Drawing.Color.Silver;
            enhancedScrollBarRenderer2.ButtonBorderColor = System.Drawing.Color.DarkGray;
            enhancedScrollBarRenderer2.ButtonHoveredBackgroundColor = System.Drawing.Color.DarkGray;
            enhancedScrollBarRenderer2.ButtonHoveredBorderColor = System.Drawing.Color.DimGray;
            enhancedScrollBarRenderer2.ButtonSelectedBackgroundColor = System.Drawing.Color.DimGray;
            enhancedScrollBarRenderer2.ButtonSelectedBorderColor = System.Drawing.Color.Gray;
            enhancedScrollBarRenderer2.TrackBarBackgroundColor = System.Drawing.Color.DarkGray;
            enhancedScrollBarRenderer2.TrackBarSelectedBackgroundColor = System.Drawing.Color.Gray;
            this.fpSpread.VerticalScrollBar.Renderer = enhancedScrollBarRenderer2;
            this.fpSpread.VerticalScrollBar.TabIndex = 5;
            // 
            // fpSpread_Sheet
            // 
            this.fpSpread_Sheet.Reset();
            fpSpread_Sheet.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpread_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpSpread_Sheet.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Classic;
            this.fpSpread_Sheet.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.fpSpread_Sheet.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Sheet.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpread_Sheet.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.fpSpread_Sheet.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Sheet.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpread_Sheet.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.fpSpread_Sheet.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Sheet.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpread_Sheet.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.fpSpread_Sheet.RowHeader.Columns.Default.Resizable = false;
            this.fpSpread_Sheet.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.fpSpread_Sheet.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Sheet.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpread_Sheet.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.fpSpread_Sheet.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Sheet.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpread_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmWaferBinReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1038, 562);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Name = "frmWaferBinReport";
            this.Text = "Bin Stack Bar";
            this.Load += new System.EventHandler(this.frmWaferBinReport_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BinChart)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Sheet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataVisualization.Charting.Chart BinChart;
        private FarPoint.Win.Spread.FpSpread fpSpread;
        private FarPoint.Win.Spread.SheetView fpSpread_Sheet;
        private System.Windows.Forms.Splitter splitter1;
        private DataSelecter.LegendList legendList1;
        private System.Windows.Forms.RadioButton rbtLot;
        private System.Windows.Forms.RadioButton rbtWafer;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem sizeResetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chartConfigToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkRate;
    }
}