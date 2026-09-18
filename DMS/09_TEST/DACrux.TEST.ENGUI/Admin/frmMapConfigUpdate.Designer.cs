namespace DACrux.TEST.ENGUI
{
    partial class frmMapConfigUpdate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMapConfigUpdate));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer1 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer2 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dlbRevision = new DACrux.Framework.Controls.DUCListBox();
            this.dlbProgram = new DACrux.Framework.Controls.DUCListBox();
            this.dlbTestArea = new DACrux.Framework.Controls.DUCListBox();
            this.dlbProduct = new DACrux.Framework.Controls.DUCListBox();
            this.BtnConfigListUp = new System.Windows.Forms.Button();
            this.BtnView = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.dtEnd = new System.Windows.Forms.DateTimePicker();
            this.dtStart = new System.Windows.Forms.DateTimePicker();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.fsSheet = new FarPoint.Win.Spread.FpSpread();
            this.fsSheet_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbFailCount = new System.Windows.Forms.Label();
            this.lbProcess = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.m_wMap = new DACrux.Map.WaferMap();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.TxtInfo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBin = new System.Windows.Forms.TextBox();
            this.lbBin = new System.Windows.Forms.Label();
            this.txtYIndex = new System.Windows.Forms.TextBox();
            this.lbY = new System.Windows.Forms.Label();
            this.txtXIndex = new System.Windows.Forms.TextBox();
            this.lbX = new System.Windows.Forms.Label();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.TxtLog = new System.Windows.Forms.RichTextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.TxtDir = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.TxtAngle = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.TxtindexX = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.TxtindexY = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fsSheet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fsSheet_Sheet1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TxtDir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtindexX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtindexY)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.TxtindexY);
            this.panel2.Controls.Add(this.TxtindexX);
            this.panel2.Controls.Add(this.TxtAngle);
            this.panel2.Controls.Add(this.TxtDir);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.dlbRevision);
            this.panel2.Controls.Add(this.dlbProgram);
            this.panel2.Controls.Add(this.dlbTestArea);
            this.panel2.Controls.Add(this.dlbProduct);
            this.panel2.Controls.Add(this.BtnConfigListUp);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1098, 132);
            this.panel2.TabIndex = 8;
            // 
            // dlbRevision
            // 
            this.dlbRevision.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dlbRevision.DataSource = null;
            this.dlbRevision.DisplayMember = "";
            this.dlbRevision.Location = new System.Drawing.Point(446, 8);
            this.dlbRevision.Name = "dlbRevision";
            this.dlbRevision.SearchText = "";
            this.dlbRevision.SearchTitle = "Revision No";
            this.dlbRevision.SelectedIndex = -1;
            this.dlbRevision.SelectedItem = null;
            this.dlbRevision.SelectedValue = null;
            this.dlbRevision.Size = new System.Drawing.Size(200, 118);
            this.dlbRevision.TabIndex = 31;
            this.dlbRevision.ValueMember = "";
            this.dlbRevision.OnSelectedIndexChanged += new System.EventHandler(this.dlbRevision_OnSelectedIndexChanged);
            // 
            // dlbProgram
            // 
            this.dlbProgram.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dlbProgram.DataSource = null;
            this.dlbProgram.DisplayMember = "";
            this.dlbProgram.Location = new System.Drawing.Point(270, 8);
            this.dlbProgram.Name = "dlbProgram";
            this.dlbProgram.SearchText = "";
            this.dlbProgram.SearchTitle = "Program";
            this.dlbProgram.SelectedIndex = -1;
            this.dlbProgram.SelectedItem = null;
            this.dlbProgram.SelectedValue = null;
            this.dlbProgram.Size = new System.Drawing.Size(171, 118);
            this.dlbProgram.TabIndex = 32;
            this.dlbProgram.ValueMember = "";
            this.dlbProgram.OnSelectedIndexChanged += new System.EventHandler(this.dlbProgram_OnSelectedIndexChanged);
            // 
            // dlbTestArea
            // 
            this.dlbTestArea.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dlbTestArea.DataSource = null;
            this.dlbTestArea.DisplayMember = "";
            this.dlbTestArea.Location = new System.Drawing.Point(9, 8);
            this.dlbTestArea.Name = "dlbTestArea";
            this.dlbTestArea.SearchText = "";
            this.dlbTestArea.SearchTitle = "Test Area";
            this.dlbTestArea.SelectedIndex = -1;
            this.dlbTestArea.SelectedItem = null;
            this.dlbTestArea.SelectedValue = null;
            this.dlbTestArea.Size = new System.Drawing.Size(132, 118);
            this.dlbTestArea.TabIndex = 29;
            this.dlbTestArea.ValueMember = "";
            this.dlbTestArea.OnSelectedIndexChanged += new System.EventHandler(this.dlbTestArea_OnSelectedIndexChanged);
            // 
            // dlbProduct
            // 
            this.dlbProduct.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dlbProduct.DataSource = null;
            this.dlbProduct.DisplayMember = "";
            this.dlbProduct.Location = new System.Drawing.Point(146, 8);
            this.dlbProduct.Name = "dlbProduct";
            this.dlbProduct.SearchText = "";
            this.dlbProduct.SearchTitle = "Device Alias";
            this.dlbProduct.SelectedIndex = -1;
            this.dlbProduct.SelectedItem = null;
            this.dlbProduct.SelectedValue = null;
            this.dlbProduct.Size = new System.Drawing.Size(119, 118);
            this.dlbProduct.TabIndex = 30;
            this.dlbProduct.ValueMember = "";
            this.dlbProduct.OnSelectedIndexChanged += new System.EventHandler(this.dlbProduct_OnSelectedIndexChanged);
            // 
            // BtnConfigListUp
            // 
            this.BtnConfigListUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnConfigListUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnConfigListUp.Image = ((System.Drawing.Image)(resources.GetObject("BtnConfigListUp.Image")));
            this.BtnConfigListUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnConfigListUp.Location = new System.Drawing.Point(938, 8);
            this.BtnConfigListUp.Name = "BtnConfigListUp";
            this.BtnConfigListUp.Size = new System.Drawing.Size(148, 32);
            this.BtnConfigListUp.TabIndex = 26;
            this.BtnConfigListUp.Text = "Config List Up";
            this.BtnConfigListUp.UseVisualStyleBackColor = true;
            this.BtnConfigListUp.Click += new System.EventHandler(this.BtnConfigListUp_Click);
            // 
            // BtnView
            // 
            this.BtnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnView.Image = ((System.Drawing.Image)(resources.GetObject("BtnView.Image")));
            this.BtnView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnView.Location = new System.Drawing.Point(498, 20);
            this.BtnView.Name = "BtnView";
            this.BtnView.Size = new System.Drawing.Size(125, 32);
            this.BtnView.TabIndex = 26;
            this.BtnView.Text = "Search";
            this.BtnView.UseVisualStyleBackColor = true;
            this.BtnView.Click += new System.EventHandler(this.BtnView_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSave.Image = ((System.Drawing.Image)(resources.GetObject("BtnSave.Image")));
            this.BtnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnSave.Location = new System.Drawing.Point(629, 20);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(125, 32);
            this.BtnSave.TabIndex = 25;
            this.BtnSave.Text = "Save";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // dtEnd
            // 
            this.dtEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtEnd.Location = new System.Drawing.Point(75, 47);
            this.dtEnd.Name = "dtEnd";
            this.dtEnd.Size = new System.Drawing.Size(107, 21);
            this.dtEnd.TabIndex = 22;
            // 
            // dtStart
            // 
            this.dtStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtStart.Location = new System.Drawing.Point(75, 20);
            this.dtStart.Name = "dtStart";
            this.dtStart.Size = new System.Drawing.Size(107, 21);
            this.dtStart.TabIndex = 21;
            // 
            // ultraLabel2
            // 
            appearance1.Image = "data.png";
            appearance1.TextHAlignAsString = "Right";
            appearance1.TextVAlignAsString = "Middle";
            this.ultraLabel2.Appearance = appearance1;
            this.ultraLabel2.ImageList = this.imageList1;
            this.ultraLabel2.Location = new System.Drawing.Point(9, 47);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(60, 23);
            this.ultraLabel2.TabIndex = 18;
            this.ultraLabel2.Text = "END";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "data.png");
            this.imageList1.Images.SetKeyName(1, "find.png");
            this.imageList1.Images.SetKeyName(2, "search.gif");
            this.imageList1.Images.SetKeyName(3, "search.png");
            // 
            // ultraLabel1
            // 
            appearance2.Image = "data.png";
            appearance2.TextHAlignAsString = "Right";
            appearance2.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance2;
            this.ultraLabel1.ImageList = this.imageList1;
            this.ultraLabel1.Location = new System.Drawing.Point(9, 22);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(60, 23);
            this.ultraLabel1.TabIndex = 17;
            this.ultraLabel1.Text = "START";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 132);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.fsSheet);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2.Controls.Add(this.ultraSplitter1);
            this.splitContainer1.Panel2.Controls.Add(this.TxtLog);
            this.splitContainer1.Size = new System.Drawing.Size(1098, 379);
            this.splitContainer1.SplitterDistance = 760;
            this.splitContainer1.TabIndex = 9;
            // 
            // fsSheet
            // 
            this.fsSheet.AccessibleDescription = "";
            this.fsSheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fsSheet.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fsSheet.HorizontalScrollBar.Name = "";
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
            this.fsSheet.HorizontalScrollBar.Renderer = enhancedScrollBarRenderer1;
            this.fsSheet.HorizontalScrollBar.TabIndex = 2;
            this.fsSheet.Location = new System.Drawing.Point(0, 78);
            this.fsSheet.Name = "fsSheet";
            this.fsSheet.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fsSheet_Sheet1});
            this.fsSheet.Size = new System.Drawing.Size(760, 301);
            this.fsSheet.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Metallic;
            this.fsSheet.TabIndex = 7;
            this.fsSheet.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fsSheet.VerticalScrollBar.Name = "";
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
            this.fsSheet.VerticalScrollBar.Renderer = enhancedScrollBarRenderer2;
            this.fsSheet.VerticalScrollBar.TabIndex = 3;
            this.fsSheet.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fsSheet_CellDoubleClick);
            // 
            // fsSheet_Sheet1
            // 
            this.fsSheet_Sheet1.Reset();
            fsSheet_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fsSheet_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fsSheet_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fsSheet_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderMetallic";
            this.fsSheet_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fsSheet_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerMetallic";
            this.fsSheet_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fsSheet_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderMetallic";
            this.fsSheet_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fsSheet_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderMetallic";
            this.fsSheet_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fsSheet_Sheet1.SheetCornerStyle.Parent = "CornerMetallic";
            this.fsSheet_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbFailCount);
            this.groupBox1.Controls.Add(this.lbProcess);
            this.groupBox1.Controls.Add(this.dtEnd);
            this.groupBox1.Controls.Add(this.ultraLabel1);
            this.groupBox1.Controls.Add(this.ultraLabel2);
            this.groupBox1.Controls.Add(this.dtStart);
            this.groupBox1.Controls.Add(this.BtnView);
            this.groupBox1.Controls.Add(this.BtnSave);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(760, 78);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Wafer List";
            // 
            // lbFailCount
            // 
            this.lbFailCount.AutoSize = true;
            this.lbFailCount.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbFailCount.ForeColor = System.Drawing.Color.Red;
            this.lbFailCount.Location = new System.Drawing.Point(199, 47);
            this.lbFailCount.Name = "lbFailCount";
            this.lbFailCount.Size = new System.Drawing.Size(94, 12);
            this.lbFailCount.TabIndex = 1;
            this.lbFailCount.Text = "Fail Count : 0";
            // 
            // lbProcess
            // 
            this.lbProcess.AutoSize = true;
            this.lbProcess.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbProcess.Location = new System.Drawing.Point(199, 26);
            this.lbProcess.Name = "lbProcess";
            this.lbProcess.Size = new System.Drawing.Size(133, 12);
            this.lbProcess.TabIndex = 1;
            this.lbProcess.Text = "Processsing : 0 / 0";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.m_wMap);
            this.panel1.Controls.Add(this.pnlInfo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 183);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(334, 196);
            this.panel1.TabIndex = 7;
            // 
            // m_wMap
            // 
            this.m_wMap.AngleOffSet = 0;
            this.m_wMap.BackColor = System.Drawing.SystemColors.Control;
            this.m_wMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
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
            this.m_wMap.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.m_wMap.MarkDieColor = System.Drawing.Color.LightSkyBlue;
            this.m_wMap.Name = "m_wMap";
            this.m_wMap.NotchAngle = 0;
            this.m_wMap.NotchType = DACrux.Base.Notch.Notch;
            this.m_wMap.OriginDieBorder = System.Drawing.Color.Red;
            this.m_wMap.OriginIndexX = 0;
            this.m_wMap.OriginIndexY = 0;
            this.m_wMap.OriginX = 0D;
            this.m_wMap.OriginY = 0D;
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
            this.m_wMap.Size = new System.Drawing.Size(334, 175);
            this.m_wMap.SkipDieColor = System.Drawing.Color.Yellow;
            this.m_wMap.TabIndex = 5;
            this.m_wMap.ToGradationDieColor = System.Drawing.Color.Red;
            this.m_wMap.TransParent = 255;
            this.m_wMap.ViewAngle = 0;
            this.m_wMap.VIMember = "VIFAIL";
            this.m_wMap.VisibleDieBorder = true;
            this.m_wMap.VisibleDieValue = false;
            this.m_wMap.VisibleFocusDie = false;
            this.m_wMap.VisibleInfomation = true;
            this.m_wMap.VisibleOffDie = false;
            this.m_wMap.VisibleStringBin = false;
            this.m_wMap.VisibleVIFail = true;
            this.m_wMap.VisibleXY = false;
            this.m_wMap.WaferBorderColor = System.Drawing.Color.LightGray;
            this.m_wMap.WaferColor = System.Drawing.Color.Gray;
            this.m_wMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
            this.m_wMap.WaferMargin = 0.95D;
            this.m_wMap.WaferSize = 200D;
            this.m_wMap.XYDirect = DACrux.Base.XYDirection.LeftBottom;
            this.m_wMap.OnChangeCurrentDie += new DACrux.Map.ChangeCurrentDie(this.m_wMap_OnChangeCurrentDie);
            // 
            // pnlInfo
            // 
            this.pnlInfo.BackColor = System.Drawing.Color.CornflowerBlue;
            this.pnlInfo.Controls.Add(this.TxtInfo);
            this.pnlInfo.Controls.Add(this.label2);
            this.pnlInfo.Controls.Add(this.txtBin);
            this.pnlInfo.Controls.Add(this.lbBin);
            this.pnlInfo.Controls.Add(this.txtYIndex);
            this.pnlInfo.Controls.Add(this.lbY);
            this.pnlInfo.Controls.Add(this.txtXIndex);
            this.pnlInfo.Controls.Add(this.lbX);
            this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInfo.Location = new System.Drawing.Point(0, 0);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(334, 21);
            this.pnlInfo.TabIndex = 5;
            // 
            // TxtInfo
            // 
            this.TxtInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TxtInfo.Location = new System.Drawing.Point(252, 0);
            this.TxtInfo.Name = "TxtInfo";
            this.TxtInfo.Size = new System.Drawing.Size(82, 21);
            this.TxtInfo.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(219, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 21);
            this.label2.TabIndex = 16;
            this.label2.Text = "Info";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtBin
            // 
            this.txtBin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBin.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtBin.Location = new System.Drawing.Point(179, 0);
            this.txtBin.Name = "txtBin";
            this.txtBin.ReadOnly = true;
            this.txtBin.Size = new System.Drawing.Size(40, 21);
            this.txtBin.TabIndex = 14;
            // 
            // lbBin
            // 
            this.lbBin.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbBin.ForeColor = System.Drawing.Color.White;
            this.lbBin.Location = new System.Drawing.Point(146, 0);
            this.lbBin.Name = "lbBin";
            this.lbBin.Size = new System.Drawing.Size(33, 21);
            this.lbBin.TabIndex = 15;
            this.lbBin.Text = "BIN";
            this.lbBin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtYIndex
            // 
            this.txtYIndex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtYIndex.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtYIndex.Location = new System.Drawing.Point(85, 0);
            this.txtYIndex.Name = "txtYIndex";
            this.txtYIndex.ReadOnly = true;
            this.txtYIndex.Size = new System.Drawing.Size(61, 21);
            this.txtYIndex.TabIndex = 0;
            // 
            // lbY
            // 
            this.lbY.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbY.ForeColor = System.Drawing.Color.White;
            this.lbY.Location = new System.Drawing.Point(70, 0);
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
            this.txtXIndex.Location = new System.Drawing.Point(15, 0);
            this.txtXIndex.Name = "txtXIndex";
            this.txtXIndex.ReadOnly = true;
            this.txtXIndex.Size = new System.Drawing.Size(55, 21);
            this.txtXIndex.TabIndex = 0;
            // 
            // lbX
            // 
            this.lbX.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbX.ForeColor = System.Drawing.Color.White;
            this.lbX.Location = new System.Drawing.Point(0, 0);
            this.lbX.Name = "lbX";
            this.lbX.Size = new System.Drawing.Size(15, 21);
            this.lbX.TabIndex = 12;
            this.lbX.Text = "X";
            this.lbX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraSplitter1.Location = new System.Drawing.Point(0, 177);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 177;
            this.ultraSplitter1.Size = new System.Drawing.Size(334, 6);
            this.ultraSplitter1.TabIndex = 6;
            // 
            // TxtLog
            // 
            this.TxtLog.Dock = System.Windows.Forms.DockStyle.Top;
            this.TxtLog.Location = new System.Drawing.Point(0, 0);
            this.TxtLog.Name = "TxtLog";
            this.TxtLog.Size = new System.Drawing.Size(334, 177);
            this.TxtLog.TabIndex = 0;
            this.TxtLog.Text = "";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.Location = new System.Drawing.Point(652, 99);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(48, 12);
            this.label11.TabIndex = 34;
            this.label11.Text = "Index Y";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(652, 72);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(48, 12);
            this.label10.TabIndex = 33;
            this.label10.Text = "Index X";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(652, 45);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(37, 12);
            this.label9.TabIndex = 36;
            this.label9.Text = "Angle";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(652, 18);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 12);
            this.label8.TabIndex = 35;
            this.label8.Text = "Direction";
            // 
            // TxtDir
            // 
            this.TxtDir.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.TxtDir.Location = new System.Drawing.Point(713, 14);
            this.TxtDir.Name = "TxtDir";
            this.TxtDir.ReadOnly = true;
            this.TxtDir.Size = new System.Drawing.Size(136, 21);
            this.TxtDir.TabIndex = 37;
            // 
            // TxtAngle
            // 
            this.TxtAngle.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.TxtAngle.Location = new System.Drawing.Point(713, 41);
            this.TxtAngle.Name = "TxtAngle";
            this.TxtAngle.ReadOnly = true;
            this.TxtAngle.Size = new System.Drawing.Size(136, 21);
            this.TxtAngle.TabIndex = 37;
            // 
            // TxtindexX
            // 
            this.TxtindexX.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.TxtindexX.Location = new System.Drawing.Point(713, 68);
            this.TxtindexX.Name = "TxtindexX";
            this.TxtindexX.ReadOnly = true;
            this.TxtindexX.Size = new System.Drawing.Size(136, 21);
            this.TxtindexX.TabIndex = 37;
            // 
            // TxtindexY
            // 
            this.TxtindexY.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.TxtindexY.Location = new System.Drawing.Point(713, 95);
            this.TxtindexY.Name = "TxtindexY";
            this.TxtindexY.ReadOnly = true;
            this.TxtindexY.Size = new System.Drawing.Size(136, 21);
            this.TxtindexY.TabIndex = 37;
            // 
            // frmMapConfigUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1098, 511);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel2);
            this.Name = "frmMapConfigUpdate";
            this.Text = "Map Define Update";
            this.Load += new System.EventHandler(this.frmMapConfigUpdate_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fsSheet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fsSheet_Sheet1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TxtDir)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtindexX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtindexY)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DateTimePicker dtEnd;
        private System.Windows.Forms.DateTimePicker dtStart;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbFailCount;
        private System.Windows.Forms.Label lbProcess;
        private System.Windows.Forms.RichTextBox TxtLog;
        private Map.WaferMap m_wMap;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private Framework.Controls.DUCListBox dlbRevision;
        private Framework.Controls.DUCListBox dlbProgram;
        private Framework.Controls.DUCListBox dlbTestArea;
        private Framework.Controls.DUCListBox dlbProduct;
        private System.Windows.Forms.Button BtnView;
        private System.Windows.Forms.Button BtnSave;
        private FarPoint.Win.Spread.FpSpread fsSheet;
        private FarPoint.Win.Spread.SheetView fsSheet_Sheet1;
        private System.Windows.Forms.Button BtnConfigListUp;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.TextBox TxtInfo;
        private System.Windows.Forms.TextBox txtYIndex;
        private System.Windows.Forms.Label lbY;
        private System.Windows.Forms.TextBox txtXIndex;
        private System.Windows.Forms.Label lbX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBin;
        private System.Windows.Forms.Label lbBin;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor TxtindexY;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor TxtindexX;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor TxtAngle;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor TxtDir;
    }
}