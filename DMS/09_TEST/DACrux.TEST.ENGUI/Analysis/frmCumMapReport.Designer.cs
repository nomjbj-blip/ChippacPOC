namespace DACrux.TEST.ENGUI
{
    partial class frmCumMapReport
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
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer1 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer2 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCumMapReport));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.m_wMap = new DACrux.Map.WaferMap();
            this.fpSpread1 = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.BtnDraw = new System.Windows.Forms.Button();
            this.rbtCount = new System.Windows.Forms.RadioButton();
            this.rbtRate = new System.Windows.Forms.RadioButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.txtBin = new System.Windows.Forms.TextBox();
            this.lbBin = new System.Windows.Forms.Label();
            this.txtYIndex = new System.Windows.Forms.TextBox();
            this.lbY = new System.Windows.Forms.Label();
            this.txtXIndex = new System.Windows.Forms.TextBox();
            this.lbX = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.chartTrend = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label3 = new System.Windows.Forms.Label();
            this.dgCumCount = new System.Windows.Forms.DataGridView();
            this.contextMenuColor = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.colorResetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartTrend)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgCumCount)).BeginInit();
            this.contextMenuColor.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_wMap
            // 
            this.m_wMap.AngleOffSet = 0;
            this.m_wMap.CenterMark = false;
            this.m_wMap.Cursor = System.Windows.Forms.Cursors.Cross;
            this.m_wMap.DataSource = null;
            this.m_wMap.DieBorderColor = System.Drawing.Color.LightGray;
            this.m_wMap.DieFocusingType = DACrux.Map.FocusType.Arraw;
            this.m_wMap.DieMaxX = 0;
            this.m_wMap.DieMaxY = 0;
            this.m_wMap.DieMinX = 0;
            this.m_wMap.DieMinY = 0;
            this.m_wMap.DieSizeX = 0.01D;
            this.m_wMap.DieSizeY = 0.01D;
            this.m_wMap.DisplayDieValue = DACrux.Base.DieDisplayValue.Bin;
            this.m_wMap.DisplayValue = "BIN";
            this.m_wMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_wMap.DrawFirstDie = true;
            this.m_wMap.DrawMarkDie = false;
            this.m_wMap.DrawOriginDie = true;
            this.m_wMap.DrawSkipDie = true;
            this.m_wMap.EdgeColor = System.Drawing.Color.LightGray;
            this.m_wMap.EdgeSize = 1D;
            this.m_wMap.FirstDieBorderColor = System.Drawing.Color.SkyBlue;
            this.m_wMap.FirstDieX = 0;
            this.m_wMap.FirstDieY = 0;
            this.m_wMap.ForeColor = System.Drawing.Color.Red;
            this.m_wMap.FromGradationDieColor = System.Drawing.Color.Lime;
            this.m_wMap.GradationInterval = 5;
            this.m_wMap.GradationMaxValue = double.NaN;
            this.m_wMap.GradationMinValue = double.NaN;
            this.m_wMap.Location = new System.Drawing.Point(0, 21);
            this.m_wMap.MarkDieColor = System.Drawing.Color.LightSkyBlue;
            this.m_wMap.Name = "m_wMap";
            this.m_wMap.NotchAngle = 0;
            this.m_wMap.NotchType = DACrux.Base.Notch.Flat;
            this.m_wMap.OriginDieBorder = System.Drawing.Color.Red;
            this.m_wMap.OriginIndexX = 0;
            this.m_wMap.OriginIndexY = 0;
            this.m_wMap.OriginX = 0D;
            this.m_wMap.OriginY = 0D;
            this.m_wMap.ParaLimit = false;
            this.m_wMap.ParametricColumn = "PCMVALUE";
            this.m_wMap.ParaValueFont = new System.Drawing.Font("굴림", 9F);
            this.m_wMap.PickupDieAlpha = 96;
            this.m_wMap.PickupedDieColor = System.Drawing.Color.Transparent;
            this.m_wMap.PopupMenu = true;
            this.m_wMap.ReferenceDieSetting = 0;
            this.m_wMap.ScaleMark = false;
            this.m_wMap.SelecetedBin = "ALL";
            this.m_wMap.SelectedVI = "ALL";
            this.m_wMap.ShotLineWidth = 2;
            this.m_wMap.Size = new System.Drawing.Size(688, 404);
            this.m_wMap.SkipDieColor = System.Drawing.Color.Yellow;
            this.m_wMap.TabIndex = 0;
            this.m_wMap.ToGradationDieColor = System.Drawing.Color.Red;
            this.m_wMap.TransParent = 255;
            this.m_wMap.ViewAngle = 0;
            this.m_wMap.VIMember = "VIFAIL";
            this.m_wMap.VisibleDieBorder = true;
            this.m_wMap.VisibleDieValue = false;
            this.m_wMap.VisibleFocusDie = false;
            this.m_wMap.VisibleInfomation = true;
            this.m_wMap.VisibleOffDie = false;
            this.m_wMap.VisibleShotAlignPoint = false;
            this.m_wMap.VisibleSignDies = false;
            this.m_wMap.VisibleStringBin = false;
            this.m_wMap.VisibleVIFail = false;
            this.m_wMap.VisibleXY = false;
            this.m_wMap.WaferBorderColor = System.Drawing.Color.LightGray;
            this.m_wMap.WaferColor = System.Drawing.Color.Gray;
            this.m_wMap.WaferDrawMode = DACrux.Map.MapMode.Free;
            this.m_wMap.WaferID = "";
            this.m_wMap.WaferMargin = 0.95D;
            this.m_wMap.WaferSize = 200000D;
            this.m_wMap.XYDirect = DACrux.Base.XYDirection.LeftTop;
            this.m_wMap.OnChangeCurrentDie += new DACrux.Map.ChangeCurrentDie(this.m_wMap_OnChangeCurrentDie);
            // 
            // fpSpread1
            // 
            this.fpSpread1.AccessibleDescription = "";
            this.fpSpread1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpread1.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpSpread1.HorizontalScrollBar.Name = "";
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
            this.fpSpread1.HorizontalScrollBar.Renderer = enhancedScrollBarRenderer1;
            this.fpSpread1.HorizontalScrollBar.TabIndex = 2;
            this.fpSpread1.Location = new System.Drawing.Point(0, 38);
            this.fpSpread1.Name = "fpSpread1";
            this.fpSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread1_Sheet1});
            this.fpSpread1.Size = new System.Drawing.Size(1148, 166);
            this.fpSpread1.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Metallic;
            this.fpSpread1.TabIndex = 1;
            this.fpSpread1.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpSpread1.VerticalScrollBar.Name = "";
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
            this.fpSpread1.VerticalScrollBar.Renderer = enhancedScrollBarRenderer2;
            this.fpSpread1.VerticalScrollBar.TabIndex = 3;
            this.fpSpread1.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpSpread1_CellDoubleClick);
            // 
            // fpSpread1_Sheet1
            // 
            this.fpSpread1_Sheet1.Reset();
            fpSpread1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpSpread1_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread1_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderMetallic";
            this.fpSpread1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerMetallic";
            this.fpSpread1_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread1_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderMetallic";
            this.fpSpread1_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread1_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderMetallic";
            this.fpSpread1_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread1_Sheet1.SheetCornerStyle.Parent = "CornerMetallic";
            this.fpSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.fpSpread1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1148, 204);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.BtnDraw);
            this.panel2.Controls.Add(this.rbtCount);
            this.panel2.Controls.Add(this.rbtRate);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1148, 38);
            this.panel2.TabIndex = 2;
            // 
            // BtnDraw
            // 
            this.BtnDraw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDraw.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDraw.Image = ((System.Drawing.Image)(resources.GetObject("BtnDraw.Image")));
            this.BtnDraw.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnDraw.Location = new System.Drawing.Point(1013, 5);
            this.BtnDraw.Name = "BtnDraw";
            this.BtnDraw.Size = new System.Drawing.Size(126, 28);
            this.BtnDraw.TabIndex = 1;
            this.BtnDraw.Text = "Apply";
            this.BtnDraw.UseVisualStyleBackColor = true;
            this.BtnDraw.Click += new System.EventHandler(this.BtnDraw_Click);
            // 
            // rbtCount
            // 
            this.rbtCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbtCount.AutoSize = true;
            this.rbtCount.Location = new System.Drawing.Point(938, 11);
            this.rbtCount.Name = "rbtCount";
            this.rbtCount.Size = new System.Drawing.Size(56, 16);
            this.rbtCount.TabIndex = 0;
            this.rbtCount.Text = "Count";
            this.rbtCount.UseVisualStyleBackColor = true;
            this.rbtCount.Click += new System.EventHandler(this.rbtType_Click);
            // 
            // rbtRate
            // 
            this.rbtRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbtRate.AutoSize = true;
            this.rbtRate.Checked = true;
            this.rbtRate.Location = new System.Drawing.Point(880, 11);
            this.rbtRate.Name = "rbtRate";
            this.rbtRate.Size = new System.Drawing.Size(48, 16);
            this.rbtRate.TabIndex = 0;
            this.rbtRate.TabStop = true;
            this.rbtRate.Text = "Rate";
            this.rbtRate.UseVisualStyleBackColor = true;
            this.rbtRate.Click += new System.EventHandler(this.rbtType_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 208);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.m_wMap);
            this.splitContainer1.Panel1.Controls.Add(this.pnlInfo);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1148, 427);
            this.splitContainer1.SplitterDistance = 690;
            this.splitContainer1.TabIndex = 3;
            // 
            // pnlInfo
            // 
            this.pnlInfo.BackColor = System.Drawing.Color.CornflowerBlue;
            this.pnlInfo.Controls.Add(this.txtBin);
            this.pnlInfo.Controls.Add(this.lbBin);
            this.pnlInfo.Controls.Add(this.txtYIndex);
            this.pnlInfo.Controls.Add(this.lbY);
            this.pnlInfo.Controls.Add(this.txtXIndex);
            this.pnlInfo.Controls.Add(this.lbX);
            this.pnlInfo.Controls.Add(this.label2);
            this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInfo.Location = new System.Drawing.Point(0, 0);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(688, 21);
            this.pnlInfo.TabIndex = 4;
            // 
            // txtBin
            // 
            this.txtBin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBin.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtBin.Location = new System.Drawing.Point(222, 0);
            this.txtBin.Name = "txtBin";
            this.txtBin.ReadOnly = true;
            this.txtBin.Size = new System.Drawing.Size(77, 21);
            this.txtBin.TabIndex = 0;
            // 
            // lbBin
            // 
            this.lbBin.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbBin.ForeColor = System.Drawing.Color.White;
            this.lbBin.Location = new System.Drawing.Point(166, 0);
            this.lbBin.Name = "lbBin";
            this.lbBin.Size = new System.Drawing.Size(56, 21);
            this.lbBin.TabIndex = 11;
            this.lbBin.Text = "Count";
            this.lbBin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtYIndex
            // 
            this.txtYIndex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtYIndex.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtYIndex.Location = new System.Drawing.Point(105, 0);
            this.txtYIndex.Name = "txtYIndex";
            this.txtYIndex.ReadOnly = true;
            this.txtYIndex.Size = new System.Drawing.Size(61, 21);
            this.txtYIndex.TabIndex = 0;
            // 
            // lbY
            // 
            this.lbY.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbY.ForeColor = System.Drawing.Color.White;
            this.lbY.Location = new System.Drawing.Point(90, 0);
            this.lbY.Name = "lbY";
            this.lbY.Size = new System.Drawing.Size(15, 21);
            this.lbY.TabIndex = 11;
            this.lbY.Text = "Y";
            this.lbY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtXIndex
            // 
            this.txtXIndex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtXIndex.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtXIndex.Location = new System.Drawing.Point(35, 0);
            this.txtXIndex.Name = "txtXIndex";
            this.txtXIndex.ReadOnly = true;
            this.txtXIndex.Size = new System.Drawing.Size(55, 21);
            this.txtXIndex.TabIndex = 0;
            // 
            // lbX
            // 
            this.lbX.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbX.ForeColor = System.Drawing.Color.White;
            this.lbX.Location = new System.Drawing.Point(20, 0);
            this.lbX.Name = "lbX";
            this.lbX.Size = new System.Drawing.Size(15, 21);
            this.lbX.TabIndex = 12;
            this.lbX.Text = "X";
            this.lbX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 21);
            this.label2.TabIndex = 19;
            this.label2.Text = "           ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.chartTrend);
            this.splitContainer2.Panel1.Controls.Add(this.label3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dgCumCount);
            this.splitContainer2.Panel2.Controls.Add(this.label1);
            this.splitContainer2.Size = new System.Drawing.Size(452, 425);
            this.splitContainer2.SplitterDistance = 212;
            this.splitContainer2.TabIndex = 5;
            // 
            // chartTrend
            // 
            chartArea1.Name = "ChartArea1";
            this.chartTrend.ChartAreas.Add(chartArea1);
            this.chartTrend.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chartTrend.Legends.Add(legend1);
            this.chartTrend.Location = new System.Drawing.Point(0, 23);
            this.chartTrend.Name = "chartTrend";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartTrend.Series.Add(series1);
            this.chartTrend.Size = new System.Drawing.Size(452, 189);
            this.chartTrend.TabIndex = 4;
            this.chartTrend.Text = "chart1";
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(452, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Cum Chart";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgCumCount
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgCumCount.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgCumCount.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgCumCount.ContextMenuStrip = this.contextMenuColor;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgCumCount.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgCumCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgCumCount.Location = new System.Drawing.Point(0, 23);
            this.dgCumCount.Name = "dgCumCount";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgCumCount.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgCumCount.RowTemplate.Height = 23;
            this.dgCumCount.Size = new System.Drawing.Size(452, 186);
            this.dgCumCount.TabIndex = 37;
            this.dgCumCount.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgCumCount_CellDoubleClick);
            // 
            // contextMenuColor
            // 
            this.contextMenuColor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.colorResetToolStripMenuItem});
            this.contextMenuColor.Name = "contextMenuColor";
            this.contextMenuColor.Size = new System.Drawing.Size(136, 26);
            // 
            // colorResetToolStripMenuItem
            // 
            this.colorResetToolStripMenuItem.Name = "colorResetToolStripMenuItem";
            this.colorResetToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.colorResetToolStripMenuItem.Text = "Color Reset";
            this.colorResetToolStripMenuItem.Click += new System.EventHandler(this.colorResetToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(452, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cum Count Color Setup";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 204);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1148, 4);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            // 
            // frmCumMapReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1148, 635);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Name = "frmCumMapReport";
            this.Text = "Cumulated Map Analysis";
            this.Load += new System.EventHandler(this.frmCumMapReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartTrend)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgCumCount)).EndInit();
            this.contextMenuColor.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Map.WaferMap m_wMap;
        private FarPoint.Win.Spread.FpSpread fpSpread1;
        private FarPoint.Win.Spread.SheetView fpSpread1_Sheet1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rbtCount;
        private System.Windows.Forms.RadioButton rbtRate;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartTrend;
        private System.Windows.Forms.Button BtnDraw;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.TextBox txtBin;
        private System.Windows.Forms.Label lbBin;
        private System.Windows.Forms.TextBox txtYIndex;
        private System.Windows.Forms.Label lbY;
        private System.Windows.Forms.TextBox txtXIndex;
        private System.Windows.Forms.Label lbX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgCumCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuColor;
        private System.Windows.Forms.ToolStripMenuItem colorResetToolStripMenuItem;
    }
}