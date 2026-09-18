namespace DACrux.SEMDMS.ENGUI
{
    partial class frmDefectPatternSearchNew
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDefectPatternSearchNew));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabSource = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.sourceMap = new DACrux.Map.DefectMap();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolWaferDieLocation = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTotalDie = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolDefectCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolSelectedDefect = new System.Windows.Forms.ToolStripStatusLabel();
            this.ultraGroupBox3 = new Infragistics.Win.Misc.UltraGroupBox();
            this.fpSelectedDefect = new FarPoint.Win.Spread.FpSpread();
            this.fpSelectedDefect_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnDeleteSelected = new System.Windows.Forms.Button();
            this.tabTarget = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.fpSelectedWafer = new FarPoint.Win.Spread.FpSpread();
            this.fpSelectedWafer_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.numMatchingRate = new System.Windows.Forms.NumericUpDown();
            this.numTolerance = new System.Windows.Forms.NumericUpDown();
            this.txtDefectCount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblProgress = new System.Windows.Forms.Label();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.ultraGroupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this.fpMatchedWafer = new FarPoint.Win.Spread.FpSpread();
            this.fpMatchedWafer_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.targetMap = new DACrux.Map.DefectMap();
            this.tabControl1.SuspendLayout();
            this.tabSource.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).BeginInit();
            this.ultraGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSelectedDefect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSelectedDefect_Sheet1)).BeginInit();
            this.panel3.SuspendLayout();
            this.tabTarget.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSelectedWafer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSelectedWafer_Sheet1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMatchingRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTolerance)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpMatchedWafer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpMatchedWafer_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabSource);
            this.tabControl1.Controls.Add(this.tabTarget);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(853, 526);
            this.tabControl1.TabIndex = 0;
            // 
            // tabSource
            // 
            this.tabSource.Controls.Add(this.splitContainer1);
            this.tabSource.Location = new System.Drawing.Point(4, 22);
            this.tabSource.Name = "tabSource";
            this.tabSource.Padding = new System.Windows.Forms.Padding(3);
            this.tabSource.Size = new System.Drawing.Size(845, 500);
            this.tabSource.TabIndex = 0;
            this.tabSource.Text = "    Source Wafer    ";
            this.tabSource.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.sourceMap);
            this.splitContainer1.Panel1.Controls.Add(this.statusStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ultraGroupBox3);
            this.splitContainer1.Size = new System.Drawing.Size(839, 494);
            this.splitContainer1.SplitterDistance = 569;
            this.splitContainer1.TabIndex = 0;
            // 
            // sourceMap
            // 
            this.sourceMap.AngleOffSet = 0;
            this.sourceMap.CenterMark = false;
            this.sourceMap.Cursor = System.Windows.Forms.Cursors.Cross;
            this.sourceMap.DataSource = null;
            this.sourceMap.DieBackgroundColor = System.Drawing.Color.Black;
            this.sourceMap.DieBorderColor = System.Drawing.Color.LightGray;
            this.sourceMap.DieDefectColor = System.Drawing.Color.Empty;
            this.sourceMap.DieFocusingType = DACrux.Map.FocusType.Arraw;
            this.sourceMap.DieMaxX = 0;
            this.sourceMap.DieMaxY = 0;
            this.sourceMap.DieMinX = 0;
            this.sourceMap.DieMinY = 0;
            this.sourceMap.DieSizeX = 0.01D;
            this.sourceMap.DieSizeY = 0.01D;
            this.sourceMap.DisplayDieValue = DACrux.Base.DieDisplayValue.Bin;
            this.sourceMap.DisplayValue = "BIN";
            this.sourceMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourceMap.DrawDefectImage = null;
            this.sourceMap.DrawDefects = "ALL";
            this.sourceMap.DrawFirstDie = true;
            this.sourceMap.DrawMarkDie = false;
            this.sourceMap.DrawOriginDie = true;
            this.sourceMap.DrawSkipDie = true;
            this.sourceMap.EdgeColor = System.Drawing.Color.LightGray;
            this.sourceMap.EdgeSize = 1D;
            this.sourceMap.FirstDieBorderColor = System.Drawing.Color.SkyBlue;
            this.sourceMap.FirstDieX = 0;
            this.sourceMap.FirstDieY = 0;
            this.sourceMap.ForeColor = System.Drawing.Color.Red;
            this.sourceMap.FromGradationDieColor = System.Drawing.Color.Lime;
            this.sourceMap.GradationInterval = 5;
            this.sourceMap.GradationMaxValue = double.NaN;
            this.sourceMap.GradationMinValue = double.NaN;
            this.sourceMap.Location = new System.Drawing.Point(0, 0);
            this.sourceMap.MapType = DACrux.Base.MAP_TYPE.CLASS;
            this.sourceMap.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.sourceMap.MarkDieColor = System.Drawing.Color.LightSkyBlue;
            this.sourceMap.Name = "sourceMap";
            this.sourceMap.NotchAngle = 0;
            this.sourceMap.NotchType = DACrux.Base.Notch.Flat;
            this.sourceMap.OriginDieBorder = System.Drawing.Color.Red;
            this.sourceMap.OriginIndexX = 0;
            this.sourceMap.OriginIndexY = 0;
            this.sourceMap.OriginX = 0D;
            this.sourceMap.OriginY = 0D;
            this.sourceMap.ParaLimit = false;
            this.sourceMap.ParametricColumn = "PCMVALUE";
            this.sourceMap.ParaValueFont = new System.Drawing.Font("굴림", 9F);
            this.sourceMap.PickupDieAlpha = 96;
            this.sourceMap.PickupedDieColor = System.Drawing.Color.Transparent;
            this.sourceMap.PopupMenu = true;
            this.sourceMap.ReferenceDieSetting = 0;
            this.sourceMap.ScaleMark = false;
            this.sourceMap.SelecetedBin = "ALL";
            this.sourceMap.SelectedVI = "ALL";
            this.sourceMap.Size = new System.Drawing.Size(569, 470);
            this.sourceMap.SizeColor = null;
            this.sourceMap.SkipDieColor = System.Drawing.Color.Yellow;
            this.sourceMap.TabIndex = 10;
            this.sourceMap.ToGradationDieColor = System.Drawing.Color.Red;
            this.sourceMap.TransParent = 255;
            this.sourceMap.TypeColor = null;
            this.sourceMap.ViewAngle = 0;
            this.sourceMap.VIMember = "VIFAIL";
            this.sourceMap.VisibleDieBorder = true;
            this.sourceMap.VisibleDieValue = false;
            this.sourceMap.VisibleFocusDie = false;
            this.sourceMap.VisibleImageMark = true;
            this.sourceMap.VisibleInfomation = true;
            this.sourceMap.VisibleOffDie = false;
            this.sourceMap.VisibleProbeOverlay = false;
            this.sourceMap.VisibleShotAlignPoint = false;
            this.sourceMap.VisibleSignDies = false;
            this.sourceMap.VisibleStringBin = false;
            this.sourceMap.VisibleVIFail = false;
            this.sourceMap.VisibleXY = false;
            this.sourceMap.WaferBorderColor = System.Drawing.Color.LightGray;
            this.sourceMap.WaferColor = System.Drawing.Color.DimGray;
            this.sourceMap.WaferDrawMode = DACrux.Map.MapMode.Free;
            this.sourceMap.WaferID = "";
            this.sourceMap.WaferMargin = 0.95D;
            this.sourceMap.WaferSize = 200000D;
            this.sourceMap.XYDirect = DACrux.Base.XYDirection.LeftBottom;
            this.sourceMap.OnSelectedDefect += new DACrux.Map.SelectedDefect(this.map_OnSelectedDefect);
            this.sourceMap.OnChangeCurrentDie += new DACrux.Map.ChangeCurrentDie(this.map_OnChangeCurrentDie);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolWaferDieLocation,
            this.toolTotalDie,
            this.toolDefectCount,
            this.toolSelectedDefect});
            this.statusStrip1.Location = new System.Drawing.Point(0, 470);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(569, 24);
            this.statusStrip1.TabIndex = 11;
            // 
            // toolWaferDieLocation
            // 
            this.toolWaferDieLocation.AutoSize = false;
            this.toolWaferDieLocation.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolWaferDieLocation.Name = "toolWaferDieLocation";
            this.toolWaferDieLocation.Size = new System.Drawing.Size(110, 19);
            this.toolWaferDieLocation.Text = "X : 0, Y : 0";
            // 
            // toolTotalDie
            // 
            this.toolTotalDie.AutoSize = false;
            this.toolTotalDie.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolTotalDie.Name = "toolTotalDie";
            this.toolTotalDie.Size = new System.Drawing.Size(140, 19);
            this.toolTotalDie.Text = "Total Dies : 0";
            // 
            // toolDefectCount
            // 
            this.toolDefectCount.AutoSize = false;
            this.toolDefectCount.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolDefectCount.Name = "toolDefectCount";
            this.toolDefectCount.Size = new System.Drawing.Size(140, 19);
            this.toolDefectCount.Text = "Total Defects : 0";
            // 
            // toolSelectedDefect
            // 
            this.toolSelectedDefect.AutoSize = false;
            this.toolSelectedDefect.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolSelectedDefect.Name = "toolSelectedDefect";
            this.toolSelectedDefect.Size = new System.Drawing.Size(140, 19);
            this.toolSelectedDefect.Text = "Selected Defects : 0";
            // 
            // ultraGroupBox3
            // 
            this.ultraGroupBox3.Controls.Add(this.fpSelectedDefect);
            this.ultraGroupBox3.Controls.Add(this.panel3);
            this.ultraGroupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox3.Location = new System.Drawing.Point(0, 0);
            this.ultraGroupBox3.Name = "ultraGroupBox3";
            this.ultraGroupBox3.Size = new System.Drawing.Size(266, 494);
            this.ultraGroupBox3.TabIndex = 3;
            this.ultraGroupBox3.Text = "Selected Defect List";
            this.ultraGroupBox3.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // fpSelectedDefect
            // 
            this.fpSelectedDefect.AccessibleDescription = "";
            this.fpSelectedDefect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSelectedDefect.Location = new System.Drawing.Point(3, 16);
            this.fpSelectedDefect.Name = "fpSelectedDefect";
            this.fpSelectedDefect.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSelectedDefect_Sheet1});
            this.fpSelectedDefect.Size = new System.Drawing.Size(260, 442);
            this.fpSelectedDefect.TabIndex = 3;
            // 
            // fpSelectedDefect_Sheet1
            // 
            this.fpSelectedDefect_Sheet1.Reset();
            fpSelectedDefect_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSelectedDefect_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpSelectedDefect_Sheet1.ColumnCount = 0;
            fpSelectedDefect_Sheet1.RowCount = 0;
            this.fpSelectedDefect_Sheet1.ActiveColumnIndex = -1;
            this.fpSelectedDefect_Sheet1.ActiveRowIndex = -1;
            this.fpSelectedDefect_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.fpSelectedDefect_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ExtendedSelect;
            this.fpSelectedDefect_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpSelectedDefect_Sheet1.RowHeader.Columns.Get(0).Width = 56F;
            this.fpSelectedDefect_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.MultiRange;
            this.fpSelectedDefect_Sheet1.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            this.fpSelectedDefect_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnDeleteSelected);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(3, 458);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(260, 33);
            this.panel3.TabIndex = 2;
            // 
            // btnDeleteSelected
            // 
            this.btnDeleteSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteSelected.Location = new System.Drawing.Point(129, 5);
            this.btnDeleteSelected.Name = "btnDeleteSelected";
            this.btnDeleteSelected.Size = new System.Drawing.Size(127, 23);
            this.btnDeleteSelected.TabIndex = 0;
            this.btnDeleteSelected.Text = "Remove Selected";
            this.btnDeleteSelected.UseVisualStyleBackColor = true;
            this.btnDeleteSelected.Click += new System.EventHandler(this.btnDeleteSelected_Click);
            // 
            // tabTarget
            // 
            this.tabTarget.Controls.Add(this.splitContainer2);
            this.tabTarget.Location = new System.Drawing.Point(4, 22);
            this.tabTarget.Name = "tabTarget";
            this.tabTarget.Padding = new System.Windows.Forms.Padding(3);
            this.tabTarget.Size = new System.Drawing.Size(845, 500);
            this.tabTarget.TabIndex = 1;
            this.tabTarget.Text = "    Target Wafers    ";
            this.tabTarget.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            this.splitContainer2.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer2.Size = new System.Drawing.Size(839, 494);
            this.splitContainer2.SplitterDistance = 192;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.ultraGroupBox1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.panel1);
            this.splitContainer3.Size = new System.Drawing.Size(839, 169);
            this.splitContainer3.SplitterDistance = 596;
            this.splitContainer3.TabIndex = 0;
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.fpSelectedWafer);
            this.ultraGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(596, 169);
            this.ultraGroupBox1.TabIndex = 2;
            this.ultraGroupBox1.Text = "Selected Wafer List";
            this.ultraGroupBox1.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // fpSelectedWafer
            // 
            this.fpSelectedWafer.AccessibleDescription = "";
            this.fpSelectedWafer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSelectedWafer.Location = new System.Drawing.Point(3, 16);
            this.fpSelectedWafer.Name = "fpSelectedWafer";
            this.fpSelectedWafer.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSelectedWafer_Sheet1});
            this.fpSelectedWafer.Size = new System.Drawing.Size(590, 150);
            this.fpSelectedWafer.TabIndex = 0;
            // 
            // fpSelectedWafer_Sheet1
            // 
            this.fpSelectedWafer_Sheet1.Reset();
            fpSelectedWafer_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSelectedWafer_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpSelectedWafer_Sheet1.ColumnCount = 0;
            fpSelectedWafer_Sheet1.RowCount = 0;
            this.fpSelectedWafer_Sheet1.ActiveColumnIndex = -1;
            this.fpSelectedWafer_Sheet1.ActiveRowIndex = -1;
            this.fpSelectedWafer_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.fpSelectedWafer_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ExtendedSelect;
            this.fpSelectedWafer_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpSelectedWafer_Sheet1.RowHeader.Columns.Get(0).Width = 56F;
            this.fpSelectedWafer_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.MultiRange;
            this.fpSelectedWafer_Sheet1.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            this.fpSelectedWafer_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnStop);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.numMatchingRate);
            this.panel1.Controls.Add(this.numTolerance);
            this.panel1.Controls.Add(this.txtDefectCount);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(239, 169);
            this.panel1.TabIndex = 0;
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.BackColor = System.Drawing.Color.White;
            this.btnStop.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnStop.Enabled = false;
            this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStop.Location = new System.Drawing.Point(42, 139);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(92, 24);
            this.btnStop.TabIndex = 219;
            this.btnStop.Text = "     Stop";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(140, 139);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(92, 24);
            this.btnSearch.TabIndex = 218;
            this.btnSearch.Text = "     Calculate";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // numMatchingRate
            // 
            this.numMatchingRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numMatchingRate.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numMatchingRate.Location = new System.Drawing.Point(143, 64);
            this.numMatchingRate.Name = "numMatchingRate";
            this.numMatchingRate.Size = new System.Drawing.Size(89, 21);
            this.numMatchingRate.TabIndex = 216;
            this.numMatchingRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numTolerance
            // 
            this.numTolerance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numTolerance.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numTolerance.Location = new System.Drawing.Point(143, 37);
            this.numTolerance.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numTolerance.Name = "numTolerance";
            this.numTolerance.Size = new System.Drawing.Size(89, 21);
            this.numTolerance.TabIndex = 214;
            this.numTolerance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtDefectCount
            // 
            this.txtDefectCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDefectCount.Location = new System.Drawing.Point(143, 10);
            this.txtDefectCount.Name = "txtDefectCount";
            this.txtDefectCount.ReadOnly = true;
            this.txtDefectCount.Size = new System.Drawing.Size(73, 21);
            this.txtDefectCount.TabIndex = 213;
            this.txtDefectCount.Text = "0";
            this.txtDefectCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(13, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 21);
            this.label3.TabIndex = 215;
            this.label3.Text = "    Matching Rate (%)";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Location = new System.Drawing.Point(13, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 21);
            this.label2.TabIndex = 212;
            this.label2.Text = "    Tolerance (um)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(13, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 21);
            this.label1.TabIndex = 212;
            this.label1.Text = "    Selected Defects";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.progressBar1);
            this.panel2.Controls.Add(this.lblProgress);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 169);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(839, 23);
            this.panel2.TabIndex = 1;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar1.Location = new System.Drawing.Point(192, 0);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(647, 23);
            this.progressBar1.TabIndex = 1;
            // 
            // lblProgress
            // 
            this.lblProgress.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblProgress.Location = new System.Drawing.Point(0, 0);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(192, 23);
            this.lblProgress.TabIndex = 0;
            this.lblProgress.Text = "label4";
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.ultraGroupBox2);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.targetMap);
            this.splitContainer4.Size = new System.Drawing.Size(839, 298);
            this.splitContainer4.SplitterDistance = 527;
            this.splitContainer4.TabIndex = 0;
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Controls.Add(this.fpMatchedWafer);
            this.ultraGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox2.Location = new System.Drawing.Point(0, 0);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(527, 298);
            this.ultraGroupBox2.TabIndex = 3;
            this.ultraGroupBox2.Text = "Matched Wafer List";
            this.ultraGroupBox2.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // fpMatchedWafer
            // 
            this.fpMatchedWafer.AccessibleDescription = "";
            this.fpMatchedWafer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpMatchedWafer.Location = new System.Drawing.Point(3, 16);
            this.fpMatchedWafer.Name = "fpMatchedWafer";
            this.fpMatchedWafer.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpMatchedWafer_Sheet1});
            this.fpMatchedWafer.Size = new System.Drawing.Size(521, 279);
            this.fpMatchedWafer.TabIndex = 1;
            this.fpMatchedWafer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.fpMatchedWafer_MouseUp);
            // 
            // fpMatchedWafer_Sheet1
            // 
            this.fpMatchedWafer_Sheet1.Reset();
            fpMatchedWafer_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpMatchedWafer_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpMatchedWafer_Sheet1.ColumnCount = 0;
            fpMatchedWafer_Sheet1.RowCount = 0;
            this.fpMatchedWafer_Sheet1.ActiveColumnIndex = -1;
            this.fpMatchedWafer_Sheet1.ActiveRowIndex = -1;
            this.fpMatchedWafer_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.fpMatchedWafer_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ExtendedSelect;
            this.fpMatchedWafer_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpMatchedWafer_Sheet1.RowHeader.Columns.Get(0).Width = 56F;
            this.fpMatchedWafer_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.MultiRange;
            this.fpMatchedWafer_Sheet1.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            this.fpMatchedWafer_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // targetMap
            // 
            this.targetMap.AngleOffSet = 0;
            this.targetMap.CenterMark = false;
            this.targetMap.Cursor = System.Windows.Forms.Cursors.Cross;
            this.targetMap.DataSource = null;
            this.targetMap.DieBackgroundColor = System.Drawing.Color.Black;
            this.targetMap.DieBorderColor = System.Drawing.Color.LightGray;
            this.targetMap.DieDefectColor = System.Drawing.Color.Empty;
            this.targetMap.DieFocusingType = DACrux.Map.FocusType.Arraw;
            this.targetMap.DieMaxX = 0;
            this.targetMap.DieMaxY = 0;
            this.targetMap.DieMinX = 0;
            this.targetMap.DieMinY = 0;
            this.targetMap.DieSizeX = 0.01D;
            this.targetMap.DieSizeY = 0.01D;
            this.targetMap.DisplayDieValue = DACrux.Base.DieDisplayValue.Bin;
            this.targetMap.DisplayValue = "BIN";
            this.targetMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.targetMap.DrawDefectImage = null;
            this.targetMap.DrawDefects = "ALL";
            this.targetMap.DrawFirstDie = true;
            this.targetMap.DrawMarkDie = false;
            this.targetMap.DrawOriginDie = true;
            this.targetMap.DrawSkipDie = true;
            this.targetMap.EdgeColor = System.Drawing.Color.LightGray;
            this.targetMap.EdgeSize = 1D;
            this.targetMap.FirstDieBorderColor = System.Drawing.Color.SkyBlue;
            this.targetMap.FirstDieX = 0;
            this.targetMap.FirstDieY = 0;
            this.targetMap.ForeColor = System.Drawing.Color.Red;
            this.targetMap.FromGradationDieColor = System.Drawing.Color.Lime;
            this.targetMap.GradationInterval = 5;
            this.targetMap.GradationMaxValue = double.NaN;
            this.targetMap.GradationMinValue = double.NaN;
            this.targetMap.Location = new System.Drawing.Point(0, 0);
            this.targetMap.MapType = DACrux.Base.MAP_TYPE.CLASS;
            this.targetMap.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.targetMap.MarkDieColor = System.Drawing.Color.LightSkyBlue;
            this.targetMap.Name = "targetMap";
            this.targetMap.NotchAngle = 0;
            this.targetMap.NotchType = DACrux.Base.Notch.Flat;
            this.targetMap.OriginDieBorder = System.Drawing.Color.Red;
            this.targetMap.OriginIndexX = 0;
            this.targetMap.OriginIndexY = 0;
            this.targetMap.OriginX = 0D;
            this.targetMap.OriginY = 0D;
            this.targetMap.ParaLimit = false;
            this.targetMap.ParametricColumn = "PCMVALUE";
            this.targetMap.ParaValueFont = new System.Drawing.Font("굴림", 9F);
            this.targetMap.PickupDieAlpha = 96;
            this.targetMap.PickupedDieColor = System.Drawing.Color.Transparent;
            this.targetMap.PopupMenu = true;
            this.targetMap.ReferenceDieSetting = 0;
            this.targetMap.ScaleMark = false;
            this.targetMap.SelecetedBin = "ALL";
            this.targetMap.SelectedVI = "ALL";
            this.targetMap.Size = new System.Drawing.Size(308, 298);
            this.targetMap.SizeColor = null;
            this.targetMap.SkipDieColor = System.Drawing.Color.Yellow;
            this.targetMap.TabIndex = 11;
            this.targetMap.ToGradationDieColor = System.Drawing.Color.Red;
            this.targetMap.TransParent = 255;
            this.targetMap.TypeColor = null;
            this.targetMap.ViewAngle = 0;
            this.targetMap.VIMember = "VIFAIL";
            this.targetMap.VisibleDieBorder = true;
            this.targetMap.VisibleDieValue = false;
            this.targetMap.VisibleFocusDie = false;
            this.targetMap.VisibleImageMark = true;
            this.targetMap.VisibleInfomation = true;
            this.targetMap.VisibleOffDie = false;
            this.targetMap.VisibleProbeOverlay = false;
            this.targetMap.VisibleShotAlignPoint = false;
            this.targetMap.VisibleSignDies = false;
            this.targetMap.VisibleStringBin = false;
            this.targetMap.VisibleVIFail = false;
            this.targetMap.VisibleXY = false;
            this.targetMap.WaferBorderColor = System.Drawing.Color.LightGray;
            this.targetMap.WaferColor = System.Drawing.Color.DimGray;
            this.targetMap.WaferDrawMode = DACrux.Map.MapMode.Free;
            this.targetMap.WaferID = "";
            this.targetMap.WaferMargin = 0.95D;
            this.targetMap.WaferSize = 200000D;
            this.targetMap.XYDirect = DACrux.Base.XYDirection.LeftBottom;
            // 
            // frmDefectPatternSearchNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 526);
            this.Controls.Add(this.tabControl1);
            this.Name = "frmDefectPatternSearchNew";
            this.Text = "Defect Pattern Search";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDefectPatternSearchNew_FormClosing);
            this.Load += new System.EventHandler(this.frmDefectPatternSearchNew_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabSource.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).EndInit();
            this.ultraGroupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSelectedDefect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSelectedDefect_Sheet1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.tabTarget.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSelectedWafer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSelectedWafer_Sheet1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMatchingRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTolerance)).EndInit();
            this.panel2.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpMatchedWafer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpMatchedWafer_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabSource;
        private System.Windows.Forms.TabPage tabTarget;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Map.DefectMap sourceMap;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numTolerance;
        private System.Windows.Forms.TextBox txtDefectCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numMatchingRate;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnSearch;
        private Map.DefectMap targetMap;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolWaferDieLocation;
        private System.Windows.Forms.ToolStripStatusLabel toolTotalDie;
        private System.Windows.Forms.ToolStripStatusLabel toolDefectCount;
        private System.Windows.Forms.ToolStripStatusLabel toolSelectedDefect;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnDeleteSelected;
        private FarPoint.Win.Spread.FpSpread fpSelectedWafer;
        private FarPoint.Win.Spread.SheetView fpSelectedWafer_Sheet1;
        private FarPoint.Win.Spread.FpSpread fpMatchedWafer;
        private FarPoint.Win.Spread.SheetView fpMatchedWafer_Sheet1;
        private FarPoint.Win.Spread.FpSpread fpSelectedDefect;
        private FarPoint.Win.Spread.SheetView fpSelectedDefect_Sheet1;
        private System.Windows.Forms.Button btnStop;
    }
}