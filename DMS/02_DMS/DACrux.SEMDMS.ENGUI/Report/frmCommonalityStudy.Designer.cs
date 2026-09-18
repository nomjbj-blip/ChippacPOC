namespace DACrux.SEMDMS.ENGUI
{
    partial class frmCommonalityStudy
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCommonalityStudy));
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer3 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer4 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            this.fpsCommon = new FarPoint.Win.Spread.FpSpread();
            this.fpsCommon_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtBadLot = new System.Windows.Forms.TextBox();
            this.txtGoodLot = new System.Windows.Forms.TextBox();
            this.TabItem = new System.Windows.Forms.TabControl();
            this.tpLot = new System.Windows.Forms.TabPage();
            this.BtnDownLot = new System.Windows.Forms.Button();
            this.lsSelectionLot = new System.Windows.Forms.ListView();
            this.BtnAddLot = new System.Windows.Forms.Button();
            this.BtnUpLot = new System.Windows.Forms.Button();
            this.lsAvailableLot = new System.Windows.Forms.ListView();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.BtnDelLot = new System.Windows.Forms.Button();
            this.tpWafer = new System.Windows.Forms.TabPage();
            this.BtnDownWafer = new System.Windows.Forms.Button();
            this.lsSelectionWafer = new System.Windows.Forms.ListView();
            this.BtnAddWafer = new System.Windows.Forms.Button();
            this.BtnUpWafer = new System.Windows.Forms.Button();
            this.lsAvailableWafer = new System.Windows.Forms.ListView();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.BtnDelWafer = new System.Windows.Forms.Button();
            this.chkInspectionDM = new System.Windows.Forms.CheckBox();
            this.btnView = new System.Windows.Forms.Button();
            this.lsBadLot = new System.Windows.Forms.ListBox();
            this.contextMenuSelect = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.allSelectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lsGoodLot = new System.Windows.Forms.ListBox();
            this.lsTotal = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.BtnMoveBad = new System.Windows.Forms.Button();
            this.BtnMoveGood = new System.Windows.Forms.Button();
            this.BtnSearchLotList = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.fpsWaferDetail = new FarPoint.Win.Spread.FpSpread();
            this.fpsWaferDetail_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.spDetail = new Infragistics.Win.Misc.UltraSplitter();
            this.pnDetail = new System.Windows.Forms.Panel();
            this.BtnDetailView = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fpsCommon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsCommon_Sheet1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.TabItem.SuspendLayout();
            this.tpLot.SuspendLayout();
            this.tpWafer.SuspendLayout();
            this.contextMenuSelect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpsWaferDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsWaferDetail_Sheet1)).BeginInit();
            this.pnDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // fpsCommon
            // 
            this.fpsCommon.AccessibleDescription = "";
            this.fpsCommon.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.fpsCommon.Location = new System.Drawing.Point(0, 288);
            this.fpsCommon.Name = "fpsCommon";
            this.fpsCommon.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.fpsCommon.ScrollTipPolicy = FarPoint.Win.Spread.ScrollTipPolicy.Both;
            this.fpsCommon.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpsCommon_Sheet1});
            this.fpsCommon.Size = new System.Drawing.Size(1177, 181);
            this.fpsCommon.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Metallic;
            this.fpsCommon.TabIndex = 60;
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
            this.fpsCommon.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpsCommon_CellDoubleClick);
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtBadLot);
            this.groupBox1.Controls.Add(this.txtGoodLot);
            this.groupBox1.Controls.Add(this.TabItem);
            this.groupBox1.Controls.Add(this.chkInspectionDM);
            this.groupBox1.Controls.Add(this.btnView);
            this.groupBox1.Controls.Add(this.lsBadLot);
            this.groupBox1.Controls.Add(this.lsGoodLot);
            this.groupBox1.Controls.Add(this.lsTotal);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.BtnMoveBad);
            this.groupBox1.Controls.Add(this.BtnMoveGood);
            this.groupBox1.Controls.Add(this.BtnSearchLotList);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1177, 280);
            this.groupBox1.TabIndex = 61;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Lot Setup";
            // 
            // txtBadLot
            // 
            this.txtBadLot.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBadLot.Location = new System.Drawing.Point(876, 36);
            this.txtBadLot.Name = "txtBadLot";
            this.txtBadLot.Size = new System.Drawing.Size(152, 21);
            this.txtBadLot.TabIndex = 66;
            this.txtBadLot.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBadLot_KeyDown);
            // 
            // txtGoodLot
            // 
            this.txtGoodLot.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtGoodLot.Location = new System.Drawing.Point(714, 36);
            this.txtGoodLot.Name = "txtGoodLot";
            this.txtGoodLot.Size = new System.Drawing.Size(152, 21);
            this.txtGoodLot.TabIndex = 66;
            this.txtGoodLot.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtGoodLot_KeyDown);
            // 
            // TabItem
            // 
            this.TabItem.Controls.Add(this.tpLot);
            this.TabItem.Controls.Add(this.tpWafer);
            this.TabItem.Dock = System.Windows.Forms.DockStyle.Left;
            this.TabItem.Location = new System.Drawing.Point(3, 17);
            this.TabItem.Name = "TabItem";
            this.TabItem.SelectedIndex = 0;
            this.TabItem.Size = new System.Drawing.Size(489, 260);
            this.TabItem.TabIndex = 65;
            this.TabItem.SelectedIndexChanged += new System.EventHandler(this.TabItem_SelectedIndexChanged);
            // 
            // tpLot
            // 
            this.tpLot.Controls.Add(this.BtnDownLot);
            this.tpLot.Controls.Add(this.lsSelectionLot);
            this.tpLot.Controls.Add(this.BtnAddLot);
            this.tpLot.Controls.Add(this.BtnUpLot);
            this.tpLot.Controls.Add(this.lsAvailableLot);
            this.tpLot.Controls.Add(this.label5);
            this.tpLot.Controls.Add(this.label4);
            this.tpLot.Controls.Add(this.BtnDelLot);
            this.tpLot.Location = new System.Drawing.Point(4, 22);
            this.tpLot.Name = "tpLot";
            this.tpLot.Padding = new System.Windows.Forms.Padding(3);
            this.tpLot.Size = new System.Drawing.Size(481, 234);
            this.tpLot.TabIndex = 0;
            this.tpLot.Text = "LEH (Lot Base)";
            this.tpLot.UseVisualStyleBackColor = true;
            // 
            // BtnDownLot
            // 
            this.BtnDownLot.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnDownLot.Image = ((System.Drawing.Image)(resources.GetObject("BtnDownLot.Image")));
            this.BtnDownLot.Location = new System.Drawing.Point(357, 75);
            this.BtnDownLot.Name = "BtnDownLot";
            this.BtnDownLot.Size = new System.Drawing.Size(18, 36);
            this.BtnDownLot.TabIndex = 69;
            this.BtnDownLot.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnDownLot.UseVisualStyleBackColor = true;
            this.BtnDownLot.Click += new System.EventHandler(this.BtnDown_Click);
            // 
            // lsSelectionLot
            // 
            this.lsSelectionLot.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lsSelectionLot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lsSelectionLot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lsSelectionLot.LabelWrap = false;
            this.lsSelectionLot.Location = new System.Drawing.Point(212, 35);
            this.lsSelectionLot.Name = "lsSelectionLot";
            this.lsSelectionLot.Size = new System.Drawing.Size(142, 193);
            this.lsSelectionLot.TabIndex = 65;
            this.lsSelectionLot.UseCompatibleStateImageBehavior = false;
            this.lsSelectionLot.View = System.Windows.Forms.View.List;
            // 
            // BtnAddLot
            // 
            this.BtnAddLot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAddLot.Image = ((System.Drawing.Image)(resources.GetObject("BtnAddLot.Image")));
            this.BtnAddLot.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnAddLot.Location = new System.Drawing.Point(154, 39);
            this.BtnAddLot.Name = "BtnAddLot";
            this.BtnAddLot.Size = new System.Drawing.Size(52, 34);
            this.BtnAddLot.TabIndex = 66;
            this.BtnAddLot.Text = "Add";
            this.BtnAddLot.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnAddLot.UseVisualStyleBackColor = true;
            this.BtnAddLot.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // BtnUpLot
            // 
            this.BtnUpLot.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnUpLot.Image = ((System.Drawing.Image)(resources.GetObject("BtnUpLot.Image")));
            this.BtnUpLot.Location = new System.Drawing.Point(357, 35);
            this.BtnUpLot.Name = "BtnUpLot";
            this.BtnUpLot.Size = new System.Drawing.Size(18, 36);
            this.BtnUpLot.TabIndex = 68;
            this.BtnUpLot.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnUpLot.UseVisualStyleBackColor = true;
            this.BtnUpLot.Click += new System.EventHandler(this.BtnUp_Click);
            // 
            // lsAvailableLot
            // 
            this.lsAvailableLot.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lsAvailableLot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lsAvailableLot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lsAvailableLot.LabelWrap = false;
            this.lsAvailableLot.Location = new System.Drawing.Point(6, 35);
            this.lsAvailableLot.Name = "lsAvailableLot";
            this.lsAvailableLot.Size = new System.Drawing.Size(142, 193);
            this.lsAvailableLot.TabIndex = 65;
            this.lsAvailableLot.UseCompatibleStateImageBehavior = false;
            this.lsAvailableLot.View = System.Windows.Forms.View.List;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(210, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "Selection Item";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "Available Item List";
            // 
            // BtnDelLot
            // 
            this.BtnDelLot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDelLot.Image = ((System.Drawing.Image)(resources.GetObject("BtnDelLot.Image")));
            this.BtnDelLot.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnDelLot.Location = new System.Drawing.Point(154, 75);
            this.BtnDelLot.Name = "BtnDelLot";
            this.BtnDelLot.Size = new System.Drawing.Size(52, 34);
            this.BtnDelLot.TabIndex = 67;
            this.BtnDelLot.Text = "Del";
            this.BtnDelLot.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnDelLot.UseVisualStyleBackColor = true;
            this.BtnDelLot.Click += new System.EventHandler(this.BtnDel_Click);
            // 
            // tpWafer
            // 
            this.tpWafer.Controls.Add(this.BtnDownWafer);
            this.tpWafer.Controls.Add(this.lsSelectionWafer);
            this.tpWafer.Controls.Add(this.BtnAddWafer);
            this.tpWafer.Controls.Add(this.BtnUpWafer);
            this.tpWafer.Controls.Add(this.lsAvailableWafer);
            this.tpWafer.Controls.Add(this.label6);
            this.tpWafer.Controls.Add(this.label7);
            this.tpWafer.Controls.Add(this.BtnDelWafer);
            this.tpWafer.Location = new System.Drawing.Point(4, 22);
            this.tpWafer.Name = "tpWafer";
            this.tpWafer.Padding = new System.Windows.Forms.Padding(3);
            this.tpWafer.Size = new System.Drawing.Size(481, 234);
            this.tpWafer.TabIndex = 1;
            this.tpWafer.Text = "WEH (Wafer Base)";
            this.tpWafer.UseVisualStyleBackColor = true;
            // 
            // BtnDownWafer
            // 
            this.BtnDownWafer.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnDownWafer.Image = ((System.Drawing.Image)(resources.GetObject("BtnDownWafer.Image")));
            this.BtnDownWafer.Location = new System.Drawing.Point(357, 75);
            this.BtnDownWafer.Name = "BtnDownWafer";
            this.BtnDownWafer.Size = new System.Drawing.Size(18, 36);
            this.BtnDownWafer.TabIndex = 77;
            this.BtnDownWafer.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnDownWafer.UseVisualStyleBackColor = true;
            this.BtnDownWafer.Click += new System.EventHandler(this.BtnDownWafer_Click);
            // 
            // lsSelectionWafer
            // 
            this.lsSelectionWafer.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lsSelectionWafer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lsSelectionWafer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lsSelectionWafer.LabelWrap = false;
            this.lsSelectionWafer.Location = new System.Drawing.Point(212, 35);
            this.lsSelectionWafer.Name = "lsSelectionWafer";
            this.lsSelectionWafer.Size = new System.Drawing.Size(142, 193);
            this.lsSelectionWafer.TabIndex = 72;
            this.lsSelectionWafer.UseCompatibleStateImageBehavior = false;
            this.lsSelectionWafer.View = System.Windows.Forms.View.List;
            // 
            // BtnAddWafer
            // 
            this.BtnAddWafer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAddWafer.Image = ((System.Drawing.Image)(resources.GetObject("BtnAddWafer.Image")));
            this.BtnAddWafer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnAddWafer.Location = new System.Drawing.Point(154, 39);
            this.BtnAddWafer.Name = "BtnAddWafer";
            this.BtnAddWafer.Size = new System.Drawing.Size(52, 34);
            this.BtnAddWafer.TabIndex = 74;
            this.BtnAddWafer.Text = "Add";
            this.BtnAddWafer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnAddWafer.UseVisualStyleBackColor = true;
            this.BtnAddWafer.Click += new System.EventHandler(this.BtnAddWafer_Click);
            // 
            // BtnUpWafer
            // 
            this.BtnUpWafer.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnUpWafer.Image = ((System.Drawing.Image)(resources.GetObject("BtnUpWafer.Image")));
            this.BtnUpWafer.Location = new System.Drawing.Point(357, 35);
            this.BtnUpWafer.Name = "BtnUpWafer";
            this.BtnUpWafer.Size = new System.Drawing.Size(18, 36);
            this.BtnUpWafer.TabIndex = 76;
            this.BtnUpWafer.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnUpWafer.UseVisualStyleBackColor = true;
            this.BtnUpWafer.Click += new System.EventHandler(this.BtnUpWafer_Click);
            // 
            // lsAvailableWafer
            // 
            this.lsAvailableWafer.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lsAvailableWafer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lsAvailableWafer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lsAvailableWafer.LabelWrap = false;
            this.lsAvailableWafer.Location = new System.Drawing.Point(6, 35);
            this.lsAvailableWafer.Name = "lsAvailableWafer";
            this.lsAvailableWafer.Size = new System.Drawing.Size(142, 193);
            this.lsAvailableWafer.TabIndex = 73;
            this.lsAvailableWafer.UseCompatibleStateImageBehavior = false;
            this.lsAvailableWafer.View = System.Windows.Forms.View.List;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(210, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 12);
            this.label6.TabIndex = 71;
            this.label6.Text = "Selection Item";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(108, 12);
            this.label7.TabIndex = 70;
            this.label7.Text = "Available Item List";
            // 
            // BtnDelWafer
            // 
            this.BtnDelWafer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDelWafer.Image = ((System.Drawing.Image)(resources.GetObject("BtnDelWafer.Image")));
            this.BtnDelWafer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnDelWafer.Location = new System.Drawing.Point(154, 75);
            this.BtnDelWafer.Name = "BtnDelWafer";
            this.BtnDelWafer.Size = new System.Drawing.Size(52, 34);
            this.BtnDelWafer.TabIndex = 75;
            this.BtnDelWafer.Text = "Del";
            this.BtnDelWafer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnDelWafer.UseVisualStyleBackColor = true;
            this.BtnDelWafer.Click += new System.EventHandler(this.BtnDelWafer_Click);
            // 
            // chkInspectionDM
            // 
            this.chkInspectionDM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkInspectionDM.AutoSize = true;
            this.chkInspectionDM.Location = new System.Drawing.Point(1032, 256);
            this.chkInspectionDM.Name = "chkInspectionDM";
            this.chkInspectionDM.Size = new System.Drawing.Size(143, 16);
            this.chkInspectionDM.TabIndex = 0;
            this.chkInspectionDM.Text = "Inspection / DM 제외";
            this.chkInspectionDM.UseVisualStyleBackColor = true;
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnView.BackColor = System.Drawing.Color.White;
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnView.Image = ((System.Drawing.Image)(resources.GetObject("btnView.Image")));
            this.btnView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnView.Location = new System.Drawing.Point(1038, 13);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(133, 31);
            this.btnView.TabIndex = 64;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = false;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // lsBadLot
            // 
            this.lsBadLot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lsBadLot.ContextMenuStrip = this.contextMenuSelect;
            this.lsBadLot.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lsBadLot.FormattingEnabled = true;
            this.lsBadLot.ItemHeight = 12;
            this.lsBadLot.Location = new System.Drawing.Point(876, 64);
            this.lsBadLot.Name = "lsBadLot";
            this.lsBadLot.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lsBadLot.Size = new System.Drawing.Size(152, 208);
            this.lsBadLot.TabIndex = 63;
            this.lsBadLot.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lsBadLot_KeyDown);
            // 
            // contextMenuSelect
            // 
            this.contextMenuSelect.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allSelectToolStripMenuItem,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.clearToolStripMenuItem});
            this.contextMenuSelect.Name = "contextMenuSelect";
            this.contextMenuSelect.Size = new System.Drawing.Size(125, 114);
            // 
            // allSelectToolStripMenuItem
            // 
            this.allSelectToolStripMenuItem.Name = "allSelectToolStripMenuItem";
            this.allSelectToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.allSelectToolStripMenuItem.Text = "All Select";
            this.allSelectToolStripMenuItem.Click += new System.EventHandler(this.allSelectToolStripMenuItem_Click);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.cutToolStripMenuItem.Text = "Cut";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // lsGoodLot
            // 
            this.lsGoodLot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lsGoodLot.ContextMenuStrip = this.contextMenuSelect;
            this.lsGoodLot.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lsGoodLot.FormattingEnabled = true;
            this.lsGoodLot.ItemHeight = 12;
            this.lsGoodLot.Location = new System.Drawing.Point(714, 64);
            this.lsGoodLot.Name = "lsGoodLot";
            this.lsGoodLot.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lsGoodLot.Size = new System.Drawing.Size(152, 208);
            this.lsGoodLot.TabIndex = 63;
            this.lsGoodLot.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lsGoodLot_KeyDown);
            // 
            // lsTotal
            // 
            this.lsTotal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lsTotal.ContextMenuStrip = this.contextMenuSelect;
            this.lsTotal.FormattingEnabled = true;
            this.lsTotal.ItemHeight = 12;
            this.lsTotal.Location = new System.Drawing.Point(498, 40);
            this.lsTotal.Name = "lsTotal";
            this.lsTotal.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lsTotal.Size = new System.Drawing.Size(152, 232);
            this.lsTotal.TabIndex = 63;
            this.lsTotal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lsTotal_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.label3.Location = new System.Drawing.Point(874, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "Bad Lot List";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.label2.Location = new System.Drawing.Point(714, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "Good Lot List";
            // 
            // BtnMoveBad
            // 
            this.BtnMoveBad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnMoveBad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.BtnMoveBad.Image = ((System.Drawing.Image)(resources.GetObject("BtnMoveBad.Image")));
            this.BtnMoveBad.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BtnMoveBad.Location = new System.Drawing.Point(654, 101);
            this.BtnMoveBad.Name = "BtnMoveBad";
            this.BtnMoveBad.Size = new System.Drawing.Size(57, 50);
            this.BtnMoveBad.TabIndex = 7;
            this.BtnMoveBad.Text = "Bad";
            this.BtnMoveBad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnMoveBad.UseVisualStyleBackColor = true;
            this.BtnMoveBad.Click += new System.EventHandler(this.BtnMoveBad_Click);
            // 
            // BtnMoveGood
            // 
            this.BtnMoveGood.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnMoveGood.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.BtnMoveGood.Image = ((System.Drawing.Image)(resources.GetObject("BtnMoveGood.Image")));
            this.BtnMoveGood.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BtnMoveGood.Location = new System.Drawing.Point(654, 43);
            this.BtnMoveGood.Name = "BtnMoveGood";
            this.BtnMoveGood.Size = new System.Drawing.Size(57, 50);
            this.BtnMoveGood.TabIndex = 7;
            this.BtnMoveGood.Text = "Good";
            this.BtnMoveGood.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnMoveGood.UseVisualStyleBackColor = true;
            this.BtnMoveGood.Click += new System.EventHandler(this.BtnMoveGood_Click);
            // 
            // BtnSearchLotList
            // 
            this.BtnSearchLotList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSearchLotList.Image = ((System.Drawing.Image)(resources.GetObject("BtnSearchLotList.Image")));
            this.BtnSearchLotList.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnSearchLotList.Location = new System.Drawing.Point(582, 13);
            this.BtnSearchLotList.Name = "BtnSearchLotList";
            this.BtnSearchLotList.Size = new System.Drawing.Size(68, 22);
            this.BtnSearchLotList.TabIndex = 7;
            this.BtnSearchLotList.Text = "    Query";
            this.BtnSearchLotList.UseVisualStyleBackColor = true;
            this.BtnSearchLotList.Click += new System.EventHandler(this.btnSearchLotList);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(496, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "Total Lot List";
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter1.BorderStyle = Infragistics.Win.UIElementBorderStyle.TwoColor;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraSplitter1.Location = new System.Drawing.Point(0, 280);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 209;
            this.ultraSplitter1.Size = new System.Drawing.Size(1177, 8);
            this.ultraSplitter1.TabIndex = 62;
            // 
            // fpsWaferDetail
            // 
            this.fpsWaferDetail.AccessibleDescription = "";
            this.fpsWaferDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpsWaferDetail.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpsWaferDetail.HorizontalScrollBar.Name = "";
            enhancedScrollBarRenderer3.ArrowColor = System.Drawing.Color.Black;
            enhancedScrollBarRenderer3.ArrowHoveredColor = System.Drawing.Color.Black;
            enhancedScrollBarRenderer3.ArrowSelectedColor = System.Drawing.Color.Black;
            enhancedScrollBarRenderer3.ButtonBackgroundColor = System.Drawing.Color.Silver;
            enhancedScrollBarRenderer3.ButtonBorderColor = System.Drawing.Color.DarkGray;
            enhancedScrollBarRenderer3.ButtonHoveredBackgroundColor = System.Drawing.Color.DarkGray;
            enhancedScrollBarRenderer3.ButtonHoveredBorderColor = System.Drawing.Color.DimGray;
            enhancedScrollBarRenderer3.ButtonSelectedBackgroundColor = System.Drawing.Color.DimGray;
            enhancedScrollBarRenderer3.ButtonSelectedBorderColor = System.Drawing.Color.Gray;
            enhancedScrollBarRenderer3.TrackBarBackgroundColor = System.Drawing.Color.DarkGray;
            enhancedScrollBarRenderer3.TrackBarSelectedBackgroundColor = System.Drawing.Color.Gray;
            this.fpsWaferDetail.HorizontalScrollBar.Renderer = enhancedScrollBarRenderer3;
            this.fpsWaferDetail.HorizontalScrollBar.TabIndex = 2;
            this.fpsWaferDetail.Location = new System.Drawing.Point(0, 30);
            this.fpsWaferDetail.Name = "fpsWaferDetail";
            this.fpsWaferDetail.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.fpsWaferDetail.ScrollTipPolicy = FarPoint.Win.Spread.ScrollTipPolicy.Both;
            this.fpsWaferDetail.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpsWaferDetail_Sheet1});
            this.fpsWaferDetail.Size = new System.Drawing.Size(1177, 218);
            this.fpsWaferDetail.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Metallic;
            this.fpsWaferDetail.TabIndex = 63;
            this.fpsWaferDetail.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpsWaferDetail.VerticalScrollBar.Name = "";
            enhancedScrollBarRenderer4.ArrowColor = System.Drawing.Color.Black;
            enhancedScrollBarRenderer4.ArrowHoveredColor = System.Drawing.Color.Black;
            enhancedScrollBarRenderer4.ArrowSelectedColor = System.Drawing.Color.Black;
            enhancedScrollBarRenderer4.ButtonBackgroundColor = System.Drawing.Color.Silver;
            enhancedScrollBarRenderer4.ButtonBorderColor = System.Drawing.Color.DarkGray;
            enhancedScrollBarRenderer4.ButtonHoveredBackgroundColor = System.Drawing.Color.DarkGray;
            enhancedScrollBarRenderer4.ButtonHoveredBorderColor = System.Drawing.Color.DimGray;
            enhancedScrollBarRenderer4.ButtonSelectedBackgroundColor = System.Drawing.Color.DimGray;
            enhancedScrollBarRenderer4.ButtonSelectedBorderColor = System.Drawing.Color.Gray;
            enhancedScrollBarRenderer4.TrackBarBackgroundColor = System.Drawing.Color.DarkGray;
            enhancedScrollBarRenderer4.TrackBarSelectedBackgroundColor = System.Drawing.Color.Gray;
            this.fpsWaferDetail.VerticalScrollBar.Renderer = enhancedScrollBarRenderer4;
            this.fpsWaferDetail.VerticalScrollBar.TabIndex = 3;
            // 
            // fpsWaferDetail_Sheet1
            // 
            this.fpsWaferDetail_Sheet1.Reset();
            fpsWaferDetail_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpsWaferDetail_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpsWaferDetail_Sheet1.ColumnCount = 0;
            fpsWaferDetail_Sheet1.RowCount = 0;
            this.fpsWaferDetail_Sheet1.ActiveColumnIndex = -1;
            this.fpsWaferDetail_Sheet1.ActiveRowIndex = -1;
            this.fpsWaferDetail_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpsWaferDetail_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderMetallic";
            this.fpsWaferDetail_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpsWaferDetail_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerMetallic";
            this.fpsWaferDetail_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpsWaferDetail_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderMetallic";
            this.fpsWaferDetail_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpsWaferDetail_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpsWaferDetail_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderMetallic";
            this.fpsWaferDetail_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpsWaferDetail_Sheet1.SheetCornerStyle.Parent = "CornerMetallic";
            this.fpsWaferDetail_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // spDetail
            // 
            this.spDetail.BackColor = System.Drawing.SystemColors.Control;
            this.spDetail.CollapseUIType = Infragistics.Win.Misc.CollapseUIType.None;
            this.spDetail.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.spDetail.Location = new System.Drawing.Point(0, 469);
            this.spDetail.Name = "spDetail";
            this.spDetail.RestoreExtent = 269;
            this.spDetail.Size = new System.Drawing.Size(1177, 6);
            this.spDetail.TabIndex = 64;
            // 
            // pnDetail
            // 
            this.pnDetail.Controls.Add(this.BtnDetailView);
            this.pnDetail.Controls.Add(this.fpsWaferDetail);
            this.pnDetail.Controls.Add(this.label8);
            this.pnDetail.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnDetail.Location = new System.Drawing.Point(0, 475);
            this.pnDetail.Name = "pnDetail";
            this.pnDetail.Size = new System.Drawing.Size(1177, 248);
            this.pnDetail.TabIndex = 70;
            this.pnDetail.Visible = false;
            // 
            // BtnDetailView
            // 
            this.BtnDetailView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDetailView.BackColor = System.Drawing.Color.White;
            this.BtnDetailView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDetailView.Image = ((System.Drawing.Image)(resources.GetObject("BtnDetailView.Image")));
            this.BtnDetailView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnDetailView.Location = new System.Drawing.Point(1039, 3);
            this.BtnDetailView.Name = "BtnDetailView";
            this.BtnDetailView.Size = new System.Drawing.Size(133, 24);
            this.BtnDetailView.TabIndex = 66;
            this.BtnDetailView.Text = "Detail View";
            this.BtnDetailView.UseVisualStyleBackColor = false;
            this.BtnDetailView.Visible = false;
            this.BtnDetailView.Click += new System.EventHandler(this.BtnDetailView_Click);
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Dock = System.Windows.Forms.DockStyle.Top;
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(1177, 30);
            this.label8.TabIndex = 64;
            this.label8.Text = "Detail View";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmCommonalityStudy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1177, 723);
            this.Controls.Add(this.fpsCommon);
            this.Controls.Add(this.spDetail);
            this.Controls.Add(this.pnDetail);
            this.Controls.Add(this.ultraSplitter1);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(1193, 648);
            this.Name = "frmCommonalityStudy";
            this.Text = "Commonality Study";
            this.Load += new System.EventHandler(this.frmCommonalityStudy_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fpsCommon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsCommon_Sheet1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.TabItem.ResumeLayout(false);
            this.tpLot.ResumeLayout(false);
            this.tpLot.PerformLayout();
            this.tpWafer.ResumeLayout(false);
            this.tpWafer.PerformLayout();
            this.contextMenuSelect.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpsWaferDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsWaferDetail_Sheet1)).EndInit();
            this.pnDetail.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FarPoint.Win.Spread.FpSpread fpsCommon;
        private FarPoint.Win.Spread.SheetView fpsCommon_Sheet1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BtnSearchLotList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkInspectionDM;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtnMoveBad;
        private System.Windows.Forms.Button BtnMoveGood;
        private System.Windows.Forms.ListBox lsBadLot;
        private System.Windows.Forms.ListBox lsGoodLot;
        private System.Windows.Forms.ListBox lsTotal;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.ContextMenuStrip contextMenuSelect;
        private System.Windows.Forms.ToolStripMenuItem allSelectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.ListView lsSelectionLot;
        public System.Windows.Forms.ListView lsAvailableLot;
        private System.Windows.Forms.Button BtnDelLot;
        private System.Windows.Forms.Button BtnAddLot;
        private System.Windows.Forms.Button BtnDownLot;
        private System.Windows.Forms.Button BtnUpLot;
        private System.Windows.Forms.TabControl TabItem;
        private System.Windows.Forms.TabPage tpLot;
        private System.Windows.Forms.TabPage tpWafer;
        private System.Windows.Forms.Button BtnDownWafer;
        public System.Windows.Forms.ListView lsSelectionWafer;
        private System.Windows.Forms.Button BtnAddWafer;
        private System.Windows.Forms.Button BtnUpWafer;
        public System.Windows.Forms.ListView lsAvailableWafer;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button BtnDelWafer;
        private FarPoint.Win.Spread.FpSpread fpsWaferDetail;
        private FarPoint.Win.Spread.SheetView fpsWaferDetail_Sheet1;
        private Infragistics.Win.Misc.UltraSplitter spDetail;
        private System.Windows.Forms.Panel pnDetail;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button BtnDetailView;
        private System.Windows.Forms.TextBox txtGoodLot;
        private System.Windows.Forms.TextBox txtBadLot;

    }
}