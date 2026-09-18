namespace DACrux.SEMDMS.ENGUI
{
    partial class frmLossYieldProspect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLossYieldProspect));
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer1 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer2 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnView = new System.Windows.Forms.Button();
            this.dlbProduct = new DACrux.Framework.Controls.DUCListBox();
            this.dlbTestArea = new DACrux.Framework.Controls.DUCListBox();
            this.dlbDevice = new DACrux.Framework.Controls.DUCListBox();
            this.dlbWaferNo = new DACrux.Framework.Controls.DUCListBox();
            this.dlbLotID = new DACrux.Framework.Controls.DUCListBox();
            this.dlbStep = new DACrux.Framework.Controls.DUCListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dtStart = new System.Windows.Forms.DateTimePicker();
            this.dtEnd = new System.Windows.Forms.DateTimePicker();
            this.fpsCommon = new FarPoint.Win.Spread.FpSpread();
            this.fpsCommon_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.ultraSplitter2 = new Infragistics.Win.Misc.UltraSplitter();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chartConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.legendList1 = new DACrux.SEMDMS.ENGUI.DataSelecter.LegendList();
            this.YieldChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.chkScaleBreaks = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpsCommon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsCommon_Sheet1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.YieldChart)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkScaleBreaks);
            this.panel1.Controls.Add(this.btnView);
            this.panel1.Controls.Add(this.dlbProduct);
            this.panel1.Controls.Add(this.dlbTestArea);
            this.panel1.Controls.Add(this.dlbDevice);
            this.panel1.Controls.Add(this.dlbWaferNo);
            this.panel1.Controls.Add(this.dlbLotID);
            this.panel1.Controls.Add(this.dlbStep);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.dtStart);
            this.panel1.Controls.Add(this.dtEnd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1121, 141);
            this.panel1.TabIndex = 0;
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnView.BackColor = System.Drawing.Color.White;
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnView.Image = ((System.Drawing.Image)(resources.GetObject("btnView.Image")));
            this.btnView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnView.Location = new System.Drawing.Point(1017, 9);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(101, 31);
            this.btnView.TabIndex = 65;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = false;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // dlbProduct
            // 
            this.dlbProduct.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dlbProduct.DataSource = null;
            this.dlbProduct.DisplayMember = "";
            this.dlbProduct.Location = new System.Drawing.Point(443, 6);
            this.dlbProduct.Name = "dlbProduct";
            this.dlbProduct.SearchText = "";
            this.dlbProduct.SearchTitle = "Product";
            this.dlbProduct.SelectedIndex = -1;
            this.dlbProduct.SelectedItem = null;
            this.dlbProduct.SelectedValue = null;
            this.dlbProduct.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbProduct.Size = new System.Drawing.Size(146, 132);
            this.dlbProduct.TabIndex = 6;
            this.dlbProduct.ValueMember = "";
            this.dlbProduct.OnSelectedValueChanged += new System.EventHandler(this.dlbProduct_OnSelectedValueChanged);
            // 
            // dlbTestArea
            // 
            this.dlbTestArea.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dlbTestArea.DataSource = null;
            this.dlbTestArea.DisplayMember = "";
            this.dlbTestArea.Location = new System.Drawing.Point(332, 6);
            this.dlbTestArea.Name = "dlbTestArea";
            this.dlbTestArea.SearchText = "";
            this.dlbTestArea.SearchTitle = "Test Area";
            this.dlbTestArea.SelectedIndex = -1;
            this.dlbTestArea.SelectedItem = null;
            this.dlbTestArea.SelectedValue = null;
            this.dlbTestArea.Size = new System.Drawing.Size(111, 132);
            this.dlbTestArea.TabIndex = 6;
            this.dlbTestArea.ValueMember = "";
            this.dlbTestArea.OnSelectedValueChanged += new System.EventHandler(this.dlbTestArea_OnSelectedValueChanged);
            // 
            // dlbDevice
            // 
            this.dlbDevice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dlbDevice.DataSource = null;
            this.dlbDevice.DisplayMember = "";
            this.dlbDevice.Location = new System.Drawing.Point(186, 6);
            this.dlbDevice.Name = "dlbDevice";
            this.dlbDevice.SearchText = "";
            this.dlbDevice.SearchTitle = "Device";
            this.dlbDevice.SelectedIndex = -1;
            this.dlbDevice.SelectedItem = null;
            this.dlbDevice.SelectedValue = null;
            this.dlbDevice.Size = new System.Drawing.Size(146, 132);
            this.dlbDevice.TabIndex = 6;
            this.dlbDevice.ValueMember = "";
            this.dlbDevice.OnSelectedValueChanged += new System.EventHandler(this.dlbDevice_OnSelectedValueChanged);
            // 
            // dlbWaferNo
            // 
            this.dlbWaferNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dlbWaferNo.DataSource = null;
            this.dlbWaferNo.DisplayMember = "";
            this.dlbWaferNo.Location = new System.Drawing.Point(733, 6);
            this.dlbWaferNo.Name = "dlbWaferNo";
            this.dlbWaferNo.SearchText = "";
            this.dlbWaferNo.SearchTitle = "Wafer No";
            this.dlbWaferNo.SelectedIndex = -1;
            this.dlbWaferNo.SelectedItem = null;
            this.dlbWaferNo.SelectedValue = null;
            this.dlbWaferNo.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbWaferNo.Size = new System.Drawing.Size(133, 132);
            this.dlbWaferNo.TabIndex = 6;
            this.dlbWaferNo.ValueMember = "";
            this.dlbWaferNo.OnSelectedValueChanged += new System.EventHandler(this.dlbWaferNo_OnSelectedValueChanged);
            // 
            // dlbLotID
            // 
            this.dlbLotID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dlbLotID.DataSource = null;
            this.dlbLotID.DisplayMember = "";
            this.dlbLotID.Location = new System.Drawing.Point(589, 6);
            this.dlbLotID.Name = "dlbLotID";
            this.dlbLotID.SearchText = "";
            this.dlbLotID.SearchTitle = "Lot ID";
            this.dlbLotID.SelectedIndex = -1;
            this.dlbLotID.SelectedItem = null;
            this.dlbLotID.SelectedValue = null;
            this.dlbLotID.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbLotID.Size = new System.Drawing.Size(144, 132);
            this.dlbLotID.TabIndex = 6;
            this.dlbLotID.ValueMember = "";
            this.dlbLotID.OnSelectedValueChanged += new System.EventHandler(this.dlbLotID_OnSelectedValueChanged);
            // 
            // dlbStep
            // 
            this.dlbStep.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dlbStep.DataSource = null;
            this.dlbStep.DisplayMember = "";
            this.dlbStep.Location = new System.Drawing.Point(866, 6);
            this.dlbStep.Name = "dlbStep";
            this.dlbStep.SearchText = "";
            this.dlbStep.SearchTitle = "Step";
            this.dlbStep.SelectedIndex = -1;
            this.dlbStep.SelectedItem = null;
            this.dlbStep.SelectedValue = null;
            this.dlbStep.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbStep.Size = new System.Drawing.Size(146, 132);
            this.dlbStep.TabIndex = 6;
            this.dlbStep.ValueMember = "";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 43);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "~ To";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 12);
            this.label6.TabIndex = 13;
            this.label6.Text = "From";
            // 
            // dtStart
            // 
            this.dtStart.CustomFormat = "yyyy-MM-dd";
            this.dtStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtStart.Location = new System.Drawing.Point(59, 12);
            this.dtStart.Name = "dtStart";
            this.dtStart.Size = new System.Drawing.Size(121, 21);
            this.dtStart.TabIndex = 10;
            this.dtStart.Value = new System.DateTime(2019, 11, 1, 11, 53, 20, 0);
            this.dtStart.ValueChanged += new System.EventHandler(this.dtStart_ValueChanged);
            // 
            // dtEnd
            // 
            this.dtEnd.CustomFormat = "yyyy-MM-dd";
            this.dtEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtEnd.Location = new System.Drawing.Point(59, 39);
            this.dtEnd.Name = "dtEnd";
            this.dtEnd.Size = new System.Drawing.Size(121, 21);
            this.dtEnd.TabIndex = 11;
            this.dtEnd.Value = new System.DateTime(2019, 11, 1, 11, 53, 26, 0);
            this.dtEnd.ValueChanged += new System.EventHandler(this.dtStart_ValueChanged);
            // 
            // fpsCommon
            // 
            this.fpsCommon.AccessibleDescription = "";
            this.fpsCommon.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.fpsCommon.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpsCommon.HorizontalScrollBar.Name = "";
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
            this.fpsCommon.HorizontalScrollBar.Renderer = enhancedScrollBarRenderer1;
            this.fpsCommon.HorizontalScrollBar.TabIndex = 0;
            this.fpsCommon.Location = new System.Drawing.Point(0, 330);
            this.fpsCommon.Name = "fpsCommon";
            this.fpsCommon.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.fpsCommon.ScrollTipPolicy = FarPoint.Win.Spread.ScrollTipPolicy.Both;
            this.fpsCommon.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpsCommon_Sheet1});
            this.fpsCommon.Size = new System.Drawing.Size(1121, 280);
            this.fpsCommon.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Metallic;
            this.fpsCommon.TabIndex = 61;
            this.fpsCommon.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpsCommon.VerticalScrollBar.Name = "";
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
            this.fpsCommon.VerticalScrollBar.Renderer = enhancedScrollBarRenderer2;
            this.fpsCommon.VerticalScrollBar.TabIndex = 3;
            // 
            // fpsCommon_Sheet1
            // 
            this.fpsCommon_Sheet1.Reset();
            fpsCommon_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpsCommon_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpsCommon_Sheet1.ColumnCount = 0;
            fpsCommon_Sheet1.RowCount = 0;
            this.fpsCommon_Sheet1.ActiveColumnIndex = -1;
            this.fpsCommon_Sheet1.ActiveRowIndex = -1;
            this.fpsCommon_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpsCommon_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderMetallic";
            this.fpsCommon_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpsCommon_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerMetallic";
            this.fpsCommon_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpsCommon_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderMetallic";
            this.fpsCommon_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpsCommon_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpsCommon_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderMetallic";
            this.fpsCommon_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpsCommon_Sheet1.SheetCornerStyle.Parent = "CornerMetallic";
            this.fpsCommon_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraSplitter1.Location = new System.Drawing.Point(0, 141);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 141;
            this.ultraSplitter1.Size = new System.Drawing.Size(1121, 6);
            this.ultraSplitter1.TabIndex = 62;
            // 
            // ultraSplitter2
            // 
            this.ultraSplitter2.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraSplitter2.Location = new System.Drawing.Point(0, 324);
            this.ultraSplitter2.Name = "ultraSplitter2";
            this.ultraSplitter2.RestoreExtent = 68;
            this.ultraSplitter2.Size = new System.Drawing.Size(1121, 6);
            this.ultraSplitter2.TabIndex = 63;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetToolStripMenuItem,
            this.chartConfigToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(144, 48);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // chartConfigToolStripMenuItem
            // 
            this.chartConfigToolStripMenuItem.Name = "chartConfigToolStripMenuItem";
            this.chartConfigToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.chartConfigToolStripMenuItem.Text = "Chart Config";
            this.chartConfigToolStripMenuItem.Click += new System.EventHandler(this.chartConfigToolStripMenuItem_Click);
            // 
            // legendList1
            // 
            this.legendList1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.legendList1.Dock = System.Windows.Forms.DockStyle.Right;
            this.legendList1.Location = new System.Drawing.Point(752, 147);
            this.legendList1.Name = "legendList1";
            this.legendList1.Size = new System.Drawing.Size(369, 177);
            this.legendList1.TabIndex = 64;
            this.legendList1.VisibleCheckedChanged += new DACrux.SEMDMS.ENGUI.DataSelecter.LegendList.ItemCheckedChangedEventHandler(this.legendList1_VisibleCheckedChanged);
            this.legendList1.ShowLabelCheckedChanged += new DACrux.SEMDMS.ENGUI.DataSelecter.LegendList.ItemCheckedChangedEventHandler(this.legendList1_ShowLabelCheckedChanged);
            // 
            // YieldChart
            // 
            this.YieldChart.BackColor = System.Drawing.Color.WhiteSmoke;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea1.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea1.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.Name = "Default";
            this.YieldChart.ChartAreas.Add(chartArea1);
            this.YieldChart.ContextMenuStrip = this.contextMenuStrip1;
            this.YieldChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.YieldChart.Location = new System.Drawing.Point(0, 147);
            this.YieldChart.Name = "YieldChart";
            series1.ChartArea = "Default";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn100;
            series1.Name = "Series1";
            this.YieldChart.Series.Add(series1);
            this.YieldChart.Size = new System.Drawing.Size(748, 177);
            this.YieldChart.TabIndex = 65;
            this.YieldChart.Text = "chart1";
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(748, 147);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(4, 177);
            this.splitter1.TabIndex = 66;
            this.splitter1.TabStop = false;
            // 
            // chkScaleBreak
            // 
            this.chkScaleBreaks.AutoSize = true;
            this.chkScaleBreaks.Location = new System.Drawing.Point(10, 119);
            this.chkScaleBreaks.Name = "chkScaleBreak";
            this.chkScaleBreaks.Size = new System.Drawing.Size(92, 16);
            this.chkScaleBreaks.TabIndex = 66;
            this.chkScaleBreaks.Text = "Scale Break";
            this.chkScaleBreaks.UseVisualStyleBackColor = true;
            this.chkScaleBreaks.CheckedChanged += new System.EventHandler(this.chkScaleBreak_CheckedChanged);
            // 
            // frmLossYieldProspect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1121, 610);
            this.Controls.Add(this.YieldChart);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.legendList1);
            this.Controls.Add(this.ultraSplitter2);
            this.Controls.Add(this.ultraSplitter1);
            this.Controls.Add(this.fpsCommon);
            this.Controls.Add(this.panel1);
            this.Name = "frmLossYieldProspect";
            this.Text = "Yield Prospect Report";
            this.Load += new System.EventHandler(this.frmLossYieldProspect_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpsCommon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsCommon_Sheet1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.YieldChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread fpsCommon;
        private FarPoint.Win.Spread.SheetView fpsCommon_Sheet1;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtStart;
        private System.Windows.Forms.DateTimePicker dtEnd;
        private Framework.Controls.DUCListBox dlbLotID;
        private Framework.Controls.DUCListBox dlbStep;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chartConfigToolStripMenuItem;
        private Framework.Controls.DUCListBox dlbProduct;
        private Framework.Controls.DUCListBox dlbDevice;
        private Framework.Controls.DUCListBox dlbWaferNo;
        private DACrux.SEMDMS.ENGUI.DataSelecter.LegendList legendList1;
        private System.Windows.Forms.DataVisualization.Charting.Chart YieldChart;
        private System.Windows.Forms.Splitter splitter1;
        private Framework.Controls.DUCListBox dlbTestArea;
        private System.Windows.Forms.CheckBox chkScaleBreaks;


    }
}