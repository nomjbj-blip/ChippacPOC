namespace DACrux.TEST.ENGUI
{
    partial class frmPatternSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPatternSearch));
            Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane3 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedLeft, new System.Guid("6d22daa2-b7a5-4833-ab5b-b9178115913c"));
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane6 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("7c827875-54ce-4867-b10e-f0af17cc4a69"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("6d22daa2-b7a5-4833-ab5b-b9178115913c"), -1);
            Infragistics.Win.UltraWinDock.DockableGroupPane dockableGroupPane2 = new Infragistics.Win.UltraWinDock.DockableGroupPane(new System.Guid("33014d21-818f-43aa-af00-29909b149184"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("6d22daa2-b7a5-4833-ab5b-b9178115913c"), -1);
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane7 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("b01beb64-cd80-4cf8-a16a-57785ec7fbf4"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("33014d21-818f-43aa-af00-29909b149184"), -1);
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane8 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("9d317c46-c43c-45d2-a8e8-d5cc7203c9ac"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("33014d21-818f-43aa-af00-29909b149184"), -1);
            Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane4 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedRight, new System.Guid("3868bccc-ee6c-45e7-bb30-8741aeab3679"));
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane9 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("78398a5b-2957-4ae2-bf9d-0b87a8d02e86"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("3868bccc-ee6c-45e7-bb30-8741aeab3679"), 0);
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane10 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("582b1544-38a5-4b91-87cd-e3d9ac97722d"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("3868bccc-ee6c-45e7-bb30-8741aeab3679"), -1);
            this.fpSpreadTargetMap = new FarPoint.Win.Spread.FpSpread();
            this.fpSpreadTargetMap_Sheet = new FarPoint.Win.Spread.SheetView();
            this.pnlBin = new System.Windows.Forms.Panel();
            this.fpSpreadBin = new FarPoint.Win.Spread.FpSpread();
            this.fpSpreadBin_Sheet = new FarPoint.Win.Spread.SheetView();
            this.butApply = new System.Windows.Forms.Button();
            this.fpSpreadDie = new FarPoint.Win.Spread.FpSpread();
            this.fpSpreadDie_Sheet = new FarPoint.Win.Spread.SheetView();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.ultraProgressBar = new Infragistics.Win.UltraWinProgressBar.UltraProgressBar();
            this.tWaferMap = new DACrux.Map.WaferMap();
            this.panel8 = new System.Windows.Forms.Panel();
            this.buttonShowMap = new System.Windows.Forms.Button();
            this.buttonPauseResume = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.fpSpreadFindMap = new FarPoint.Win.Spread.FpSpread();
            this.fpSpreadFindMap_Sheet = new FarPoint.Win.Spread.SheetView();
            this.panelSourceMap = new System.Windows.Forms.Panel();
            this.sWaferMap = new DACrux.Map.WaferMap();
            this.panel4 = new System.Windows.Forms.Panel();
            this.buttonClear = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxMatchingRate = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.frmBimMapPattern_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.gallery = new DACrux.TEST.ENGUI.TPUCGallery();
            this.ultraDockManager = new Infragistics.Win.UltraWinDock.UltraDockManager(this.components);
            this._frmBimMapPatternUnpinnedTabAreaLeft = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._frmBimMapPatternUnpinnedTabAreaRight = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._frmBimMapPatternUnpinnedTabAreaTop = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._frmBimMapPatternUnpinnedTabAreaBottom = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._frmBimMapPatternAutoHideControl = new Infragistics.Win.UltraWinDock.AutoHideControl();
            this.dockableWindow1 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.dockableWindow3 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.dockableWindow4 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.windowDockingArea5 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
            this.dockableWindow5 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.windowDockingArea2 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
            this.dockableWindow2 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.ctxMnu = new System.Windows.Forms.ContextMenu();
            this.mnuAll = new System.Windows.Forms.MenuItem();
            this.mnuNone = new System.Windows.Forms.MenuItem();
            this.mnuInvert = new System.Windows.Forms.MenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadTargetMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadTargetMap_Sheet)).BeginInit();
            this.pnlBin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadBin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadBin_Sheet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadDie)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadDie_Sheet)).BeginInit();
            this.pnlSearch.SuspendLayout();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadFindMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadFindMap_Sheet)).BeginInit();
            this.panelSourceMap.SuspendLayout();
            this.panel4.SuspendLayout();
            this.frmBimMapPattern_Fill_Panel.ClientArea.SuspendLayout();
            this.frmBimMapPattern_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager)).BeginInit();
            this.dockableWindow1.SuspendLayout();
            this.dockableWindow3.SuspendLayout();
            this.dockableWindow4.SuspendLayout();
            this.windowDockingArea5.SuspendLayout();
            this.dockableWindow5.SuspendLayout();
            this.windowDockingArea2.SuspendLayout();
            this.dockableWindow2.SuspendLayout();
            this.SuspendLayout();
            // 
            // fpSpreadTargetMap
            // 
            this.fpSpreadTargetMap.AccessibleDescription = "";
            this.fpSpreadTargetMap.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpSpreadTargetMap.Location = new System.Drawing.Point(0, 20);
            this.fpSpreadTargetMap.Name = "fpSpreadTargetMap";
            this.fpSpreadTargetMap.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpSpreadTargetMap.SelectionBlockOptions = FarPoint.Win.Spread.SelectionBlockOptions.Rows;
            this.fpSpreadTargetMap.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpreadTargetMap_Sheet});
            this.fpSpreadTargetMap.Size = new System.Drawing.Size(189, 239);
            this.fpSpreadTargetMap.TabIndex = 13;
            this.fpSpreadTargetMap.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpSpreadTargetMap_CellDoubleClick);
            // 
            // fpSpreadTargetMap_Sheet
            // 
            this.fpSpreadTargetMap_Sheet.Reset();
            fpSpreadTargetMap_Sheet.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpreadTargetMap_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpSpreadTargetMap_Sheet.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Classic2;
            this.fpSpreadTargetMap_Sheet.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadTargetMap_Sheet.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadTargetMap_Sheet.ColumnFooter.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadTargetMap_Sheet.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadTargetMap_Sheet.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadTargetMap_Sheet.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadTargetMap_Sheet.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadTargetMap_Sheet.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadTargetMap_Sheet.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadTargetMap_Sheet.ColumnFooterSheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadTargetMap_Sheet.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadTargetMap_Sheet.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadTargetMap_Sheet.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadTargetMap_Sheet.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadTargetMap_Sheet.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadTargetMap_Sheet.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadTargetMap_Sheet.ColumnHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadTargetMap_Sheet.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadTargetMap_Sheet.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadTargetMap_Sheet.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadTargetMap_Sheet.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadTargetMap_Sheet.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadTargetMap_Sheet.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpSpreadTargetMap_Sheet.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpSpreadTargetMap_Sheet.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadTargetMap_Sheet.DefaultStyle.Parent = "DataAreaDefault";
            this.fpSpreadTargetMap_Sheet.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;
            this.fpSpreadTargetMap_Sheet.RowHeader.Columns.Default.Resizable = false;
            this.fpSpreadTargetMap_Sheet.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadTargetMap_Sheet.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadTargetMap_Sheet.RowHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadTargetMap_Sheet.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadTargetMap_Sheet.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadTargetMap_Sheet.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadTargetMap_Sheet.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadTargetMap_Sheet.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadTargetMap_Sheet.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.Single;
            this.fpSpreadTargetMap_Sheet.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            this.fpSpreadTargetMap_Sheet.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadTargetMap_Sheet.SheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadTargetMap_Sheet.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadTargetMap_Sheet.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadTargetMap_Sheet.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadTargetMap_Sheet.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadTargetMap_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // pnlBin
            // 
            this.pnlBin.Controls.Add(this.fpSpreadBin);
            this.pnlBin.Controls.Add(this.butApply);
            this.pnlBin.Location = new System.Drawing.Point(0, 20);
            this.pnlBin.Name = "pnlBin";
            this.pnlBin.Size = new System.Drawing.Size(189, 154);
            this.pnlBin.TabIndex = 15;
            // 
            // fpSpreadBin
            // 
            this.fpSpreadBin.AccessibleDescription = "fpSpreadBin, Sheet1, Row 0, Column 0, ";
            this.fpSpreadBin.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpSpreadBin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpreadBin.Location = new System.Drawing.Point(0, 0);
            this.fpSpreadBin.Name = "fpSpreadBin";
            this.fpSpreadBin.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpSpreadBin.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpreadBin_Sheet});
            this.fpSpreadBin.Size = new System.Drawing.Size(189, 114);
            this.fpSpreadBin.TabIndex = 14;
            this.fpSpreadBin.MouseDown += new System.Windows.Forms.MouseEventHandler(this.fpSpreadBin_MouseDown);
            // 
            // fpSpreadBin_Sheet
            // 
            this.fpSpreadBin_Sheet.Reset();
            fpSpreadBin_Sheet.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpreadBin_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpSpreadBin_Sheet.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Classic2;
            this.fpSpreadBin_Sheet.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadBin_Sheet.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadBin_Sheet.ColumnFooter.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadBin_Sheet.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadBin_Sheet.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadBin_Sheet.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadBin_Sheet.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadBin_Sheet.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadBin_Sheet.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadBin_Sheet.ColumnFooterSheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadBin_Sheet.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadBin_Sheet.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadBin_Sheet.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadBin_Sheet.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadBin_Sheet.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadBin_Sheet.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadBin_Sheet.ColumnHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadBin_Sheet.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadBin_Sheet.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadBin_Sheet.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadBin_Sheet.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadBin_Sheet.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadBin_Sheet.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpSpreadBin_Sheet.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpSpreadBin_Sheet.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadBin_Sheet.DefaultStyle.Parent = "DataAreaDefault";
            this.fpSpreadBin_Sheet.RowHeader.Columns.Default.Resizable = false;
            this.fpSpreadBin_Sheet.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadBin_Sheet.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadBin_Sheet.RowHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadBin_Sheet.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadBin_Sheet.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadBin_Sheet.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadBin_Sheet.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadBin_Sheet.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadBin_Sheet.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadBin_Sheet.SheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadBin_Sheet.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadBin_Sheet.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadBin_Sheet.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadBin_Sheet.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadBin_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // butApply
            // 
            this.butApply.BackColor = System.Drawing.Color.Ivory;
            this.butApply.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.butApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butApply.ForeColor = System.Drawing.Color.Ivory;
            this.butApply.Image = ((System.Drawing.Image)(resources.GetObject("butApply.Image")));
            this.butApply.Location = new System.Drawing.Point(0, 114);
            this.butApply.Name = "butApply";
            this.butApply.Size = new System.Drawing.Size(189, 40);
            this.butApply.TabIndex = 31;
            this.butApply.UseVisualStyleBackColor = false;
            this.butApply.Click += new System.EventHandler(this.butApply_Click);
            // 
            // fpSpreadDie
            // 
            this.fpSpreadDie.AccessibleDescription = "fpSpreadDie, Sheet1, Row 0, Column 0, ";
            this.fpSpreadDie.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpSpreadDie.Location = new System.Drawing.Point(0, 20);
            this.fpSpreadDie.Name = "fpSpreadDie";
            this.fpSpreadDie.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpSpreadDie.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpreadDie_Sheet});
            this.fpSpreadDie.Size = new System.Drawing.Size(189, 163);
            this.fpSpreadDie.TabIndex = 15;
            this.fpSpreadDie.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpSpreadTargetMap_CellDoubleClick);
            // 
            // fpSpreadDie_Sheet
            // 
            this.fpSpreadDie_Sheet.Reset();
            fpSpreadDie_Sheet.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpreadDie_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpSpreadDie_Sheet.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Classic2;
            this.fpSpreadDie_Sheet.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDie_Sheet.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadDie_Sheet.ColumnFooter.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadDie_Sheet.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadDie_Sheet.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadDie_Sheet.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadDie_Sheet.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDie_Sheet.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDie_Sheet.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadDie_Sheet.ColumnFooterSheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadDie_Sheet.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadDie_Sheet.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadDie_Sheet.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadDie_Sheet.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDie_Sheet.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDie_Sheet.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadDie_Sheet.ColumnHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadDie_Sheet.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadDie_Sheet.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadDie_Sheet.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadDie_Sheet.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDie_Sheet.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDie_Sheet.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpSpreadDie_Sheet.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpSpreadDie_Sheet.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadDie_Sheet.DefaultStyle.Parent = "DataAreaDefault";
            this.fpSpreadDie_Sheet.RowHeader.Columns.Default.Resizable = false;
            this.fpSpreadDie_Sheet.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDie_Sheet.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadDie_Sheet.RowHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadDie_Sheet.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadDie_Sheet.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadDie_Sheet.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadDie_Sheet.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDie_Sheet.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDie_Sheet.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadDie_Sheet.SheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadDie_Sheet.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadDie_Sheet.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadDie_Sheet.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadDie_Sheet.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadDie_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // pnlSearch
            // 
            this.pnlSearch.Controls.Add(this.ultraProgressBar);
            this.pnlSearch.Controls.Add(this.tWaferMap);
            this.pnlSearch.Controls.Add(this.panel8);
            this.pnlSearch.Location = new System.Drawing.Point(0, 20);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(269, 214);
            this.pnlSearch.TabIndex = 0;
            // 
            // ultraProgressBar
            // 
            this.ultraProgressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraProgressBar.Location = new System.Drawing.Point(0, 198);
            this.ultraProgressBar.Name = "ultraProgressBar";
            this.ultraProgressBar.Size = new System.Drawing.Size(269, 16);
            this.ultraProgressBar.TabIndex = 9;
            this.ultraProgressBar.Text = "[Formatted]";
            // 
            // tWaferMap
            // 
            this.tWaferMap.AngleOffSet = 0;
            this.tWaferMap.CenterMark = false;
            this.tWaferMap.Cursor = System.Windows.Forms.Cursors.Cross;
            this.tWaferMap.DataSource = null;
            this.tWaferMap.DieBorderColor = System.Drawing.Color.LightGray;
            this.tWaferMap.DieFocusingType = DACrux.Map.FocusType.Arraw;
            this.tWaferMap.DieMaxX = 0;
            this.tWaferMap.DieMaxY = 0;
            this.tWaferMap.DieMinX = 0;
            this.tWaferMap.DieMinY = 0;
            this.tWaferMap.DieSizeX = 0.01D;
            this.tWaferMap.DieSizeY = 0.01D;
            this.tWaferMap.DisplayDieValue = DACrux.Base.DieDisplayValue.Bin;
            this.tWaferMap.DisplayValue = "BIN";
            this.tWaferMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tWaferMap.DrawFirstDie = true;
            this.tWaferMap.DrawMarkDie = false;
            this.tWaferMap.DrawOriginDie = true;
            this.tWaferMap.DrawSkipDie = true;
            this.tWaferMap.EdgeColor = System.Drawing.Color.LightGray;
            this.tWaferMap.EdgeSize = 1D;
            this.tWaferMap.FirstDieBorderColor = System.Drawing.Color.SkyBlue;
            this.tWaferMap.FirstDieX = 0;
            this.tWaferMap.FirstDieY = 0;
            this.tWaferMap.ForeColor = System.Drawing.Color.Red;
            this.tWaferMap.FromGradationDieColor = System.Drawing.Color.Lime;
            this.tWaferMap.GradationInterval = 5;
            this.tWaferMap.GradationMaxValue = double.NaN;
            this.tWaferMap.GradationMinValue = double.NaN;
            this.tWaferMap.Location = new System.Drawing.Point(0, 24);
            this.tWaferMap.MarkDieColor = System.Drawing.Color.LightSkyBlue;
            this.tWaferMap.Name = "tWaferMap";
            this.tWaferMap.NotchAngle = 0;
            this.tWaferMap.NotchType = DACrux.Base.Notch.Flat;
            this.tWaferMap.OriginDieBorder = System.Drawing.Color.Red;
            this.tWaferMap.OriginIndexX = 0;
            this.tWaferMap.OriginIndexY = 0;
            this.tWaferMap.OriginX = 0D;
            this.tWaferMap.OriginY = 0D;
            this.tWaferMap.ParaLimit = false;
            this.tWaferMap.ParametricColumn = "PCMVALUE";
            this.tWaferMap.ParaValueFont = new System.Drawing.Font("굴림", 9F);
            this.tWaferMap.PickupDieAlpha = 96;
            this.tWaferMap.PickupedDieColor = System.Drawing.Color.Transparent;
            this.tWaferMap.PopupMenu = true;
            this.tWaferMap.ReferenceDieSetting = 0;
            this.tWaferMap.ScaleMark = false;
            this.tWaferMap.SelecetedBin = "ALL";
            this.tWaferMap.SelectedVI = "ALL";
            this.tWaferMap.Size = new System.Drawing.Size(269, 190);
            this.tWaferMap.SkipDieColor = System.Drawing.Color.Yellow;
            this.tWaferMap.TabIndex = 2;
            this.tWaferMap.ToGradationDieColor = System.Drawing.Color.Red;
            this.tWaferMap.TransParent = 255;
            this.tWaferMap.ViewAngle = 0;
            this.tWaferMap.VIMember = "VIFAIL";
            this.tWaferMap.VisibleDieBorder = true;
            this.tWaferMap.VisibleDieValue = false;
            this.tWaferMap.VisibleFocusDie = false;
            this.tWaferMap.VisibleInfomation = true;
            this.tWaferMap.VisibleOffDie = false;
            this.tWaferMap.VisibleShotAlignPoint = false;
            this.tWaferMap.VisibleSignDies = false;
            this.tWaferMap.VisibleStringBin = false;
            this.tWaferMap.VisibleVIFail = false;
            this.tWaferMap.VisibleXY = false;
            this.tWaferMap.WaferBorderColor = System.Drawing.Color.LightGray;
            this.tWaferMap.WaferColor = System.Drawing.Color.Gray;
            this.tWaferMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
            this.tWaferMap.WaferID = "";
            this.tWaferMap.WaferMargin = 0.95D;
            this.tWaferMap.WaferSize = 200000D;
            this.tWaferMap.XYDirect = DACrux.Base.XYDirection.LeftTop;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.buttonShowMap);
            this.panel8.Controls.Add(this.buttonPauseResume);
            this.panel8.Controls.Add(this.buttonStart);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(269, 24);
            this.panel8.TabIndex = 1;
            // 
            // buttonShowMap
            // 
            this.buttonShowMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonShowMap.Enabled = false;
            this.buttonShowMap.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonShowMap.Location = new System.Drawing.Point(112, 0);
            this.buttonShowMap.Name = "buttonShowMap";
            this.buttonShowMap.Size = new System.Drawing.Size(157, 24);
            this.buttonShowMap.TabIndex = 11;
            this.buttonShowMap.Text = "View Find Map";
            this.buttonShowMap.Click += new System.EventHandler(this.buttonShowMap_Click);
            // 
            // buttonPauseResume
            // 
            this.buttonPauseResume.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonPauseResume.Enabled = false;
            this.buttonPauseResume.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonPauseResume.Location = new System.Drawing.Point(56, 0);
            this.buttonPauseResume.Name = "buttonPauseResume";
            this.buttonPauseResume.Size = new System.Drawing.Size(56, 24);
            this.buttonPauseResume.TabIndex = 11;
            this.buttonPauseResume.Text = "Pause";
            this.buttonPauseResume.Click += new System.EventHandler(this.buttonPauseResume_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonStart.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonStart.Location = new System.Drawing.Point(0, 0);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(56, 24);
            this.buttonStart.TabIndex = 11;
            this.buttonStart.Text = "Start";
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // fpSpreadFindMap
            // 
            this.fpSpreadFindMap.AccessibleDescription = "";
            this.fpSpreadFindMap.Location = new System.Drawing.Point(0, 20);
            this.fpSpreadFindMap.Name = "fpSpreadFindMap";
            this.fpSpreadFindMap.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpreadFindMap_Sheet});
            this.fpSpreadFindMap.Size = new System.Drawing.Size(269, 210);
            this.fpSpreadFindMap.TabIndex = 14;
            // 
            // fpSpreadFindMap_Sheet
            // 
            this.fpSpreadFindMap_Sheet.Reset();
            fpSpreadFindMap_Sheet.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpreadFindMap_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpSpreadFindMap_Sheet.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Classic2;
            this.fpSpreadFindMap_Sheet.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadFindMap_Sheet.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadFindMap_Sheet.ColumnFooter.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadFindMap_Sheet.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadFindMap_Sheet.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadFindMap_Sheet.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadFindMap_Sheet.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadFindMap_Sheet.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadFindMap_Sheet.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadFindMap_Sheet.ColumnFooterSheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadFindMap_Sheet.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadFindMap_Sheet.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadFindMap_Sheet.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadFindMap_Sheet.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadFindMap_Sheet.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadFindMap_Sheet.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadFindMap_Sheet.ColumnHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadFindMap_Sheet.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadFindMap_Sheet.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadFindMap_Sheet.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadFindMap_Sheet.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadFindMap_Sheet.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadFindMap_Sheet.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpSpreadFindMap_Sheet.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpSpreadFindMap_Sheet.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadFindMap_Sheet.DefaultStyle.Parent = "DataAreaDefault";
            this.fpSpreadFindMap_Sheet.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadFindMap_Sheet.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadFindMap_Sheet.RowHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadFindMap_Sheet.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadFindMap_Sheet.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadFindMap_Sheet.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadFindMap_Sheet.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadFindMap_Sheet.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadFindMap_Sheet.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadFindMap_Sheet.SheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadFindMap_Sheet.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadFindMap_Sheet.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadFindMap_Sheet.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadFindMap_Sheet.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadFindMap_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panelSourceMap
            // 
            this.panelSourceMap.Controls.Add(this.sWaferMap);
            this.panelSourceMap.Controls.Add(this.panel4);
            this.panelSourceMap.Location = new System.Drawing.Point(19, 272);
            this.panelSourceMap.Name = "panelSourceMap";
            this.panelSourceMap.Size = new System.Drawing.Size(211, 168);
            this.panelSourceMap.TabIndex = 2;
            // 
            // sWaferMap
            // 
            this.sWaferMap.AngleOffSet = 0;
            this.sWaferMap.CenterMark = false;
            this.sWaferMap.Cursor = System.Windows.Forms.Cursors.Cross;
            this.sWaferMap.DataSource = null;
            this.sWaferMap.DieBorderColor = System.Drawing.Color.LightGray;
            this.sWaferMap.DieFocusingType = DACrux.Map.FocusType.Arraw;
            this.sWaferMap.DieMaxX = 0;
            this.sWaferMap.DieMaxY = 0;
            this.sWaferMap.DieMinX = 0;
            this.sWaferMap.DieMinY = 0;
            this.sWaferMap.DieSizeX = 0.01D;
            this.sWaferMap.DieSizeY = 0.01D;
            this.sWaferMap.DisplayDieValue = DACrux.Base.DieDisplayValue.Bin;
            this.sWaferMap.DisplayValue = "BIN";
            this.sWaferMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sWaferMap.DrawFirstDie = true;
            this.sWaferMap.DrawMarkDie = false;
            this.sWaferMap.DrawOriginDie = true;
            this.sWaferMap.DrawSkipDie = true;
            this.sWaferMap.EdgeColor = System.Drawing.Color.LightGray;
            this.sWaferMap.EdgeSize = 1D;
            this.sWaferMap.FirstDieBorderColor = System.Drawing.Color.SkyBlue;
            this.sWaferMap.FirstDieX = 0;
            this.sWaferMap.FirstDieY = 0;
            this.sWaferMap.ForeColor = System.Drawing.Color.Red;
            this.sWaferMap.FromGradationDieColor = System.Drawing.Color.Lime;
            this.sWaferMap.GradationInterval = 5;
            this.sWaferMap.GradationMaxValue = double.NaN;
            this.sWaferMap.GradationMinValue = double.NaN;
            this.sWaferMap.Location = new System.Drawing.Point(0, 0);
            this.sWaferMap.MarkDieColor = System.Drawing.Color.LightSkyBlue;
            this.sWaferMap.Name = "sWaferMap";
            this.sWaferMap.NotchAngle = 0;
            this.sWaferMap.NotchType = DACrux.Base.Notch.Flat;
            this.sWaferMap.OriginDieBorder = System.Drawing.Color.Red;
            this.sWaferMap.OriginIndexX = 0;
            this.sWaferMap.OriginIndexY = 0;
            this.sWaferMap.OriginX = 0D;
            this.sWaferMap.OriginY = 0D;
            this.sWaferMap.ParaLimit = false;
            this.sWaferMap.ParametricColumn = "PCMVALUE";
            this.sWaferMap.ParaValueFont = new System.Drawing.Font("굴림", 9F);
            this.sWaferMap.PickupDieAlpha = 96;
            this.sWaferMap.PickupedDieColor = System.Drawing.Color.Transparent;
            this.sWaferMap.PopupMenu = true;
            this.sWaferMap.ReferenceDieSetting = 0;
            this.sWaferMap.ScaleMark = false;
            this.sWaferMap.SelecetedBin = "ALL";
            this.sWaferMap.SelectedVI = "ALL";
            this.sWaferMap.Size = new System.Drawing.Size(211, 144);
            this.sWaferMap.SkipDieColor = System.Drawing.Color.Yellow;
            this.sWaferMap.TabIndex = 3;
            this.sWaferMap.ToGradationDieColor = System.Drawing.Color.Red;
            this.sWaferMap.TransParent = 255;
            this.sWaferMap.ViewAngle = 0;
            this.sWaferMap.VIMember = "VIFAIL";
            this.sWaferMap.VisibleDieBorder = true;
            this.sWaferMap.VisibleDieValue = false;
            this.sWaferMap.VisibleFocusDie = false;
            this.sWaferMap.VisibleInfomation = true;
            this.sWaferMap.VisibleOffDie = false;
            this.sWaferMap.VisibleShotAlignPoint = false;
            this.sWaferMap.VisibleSignDies = false;
            this.sWaferMap.VisibleStringBin = false;
            this.sWaferMap.VisibleVIFail = false;
            this.sWaferMap.VisibleXY = false;
            this.sWaferMap.WaferBorderColor = System.Drawing.Color.LightGray;
            this.sWaferMap.WaferColor = System.Drawing.Color.Gray;
            this.sWaferMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
            this.sWaferMap.WaferID = "";
            this.sWaferMap.WaferMargin = 0.95D;
            this.sWaferMap.WaferSize = 200000D;
            this.sWaferMap.XYDirect = DACrux.Base.XYDirection.LeftTop;
            this.sWaferMap.OnSelectDies += new DACrux.Map.SelectDies(this.sWaferMap_OnSelectDies);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.buttonClear);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.textBoxMatchingRate);
            this.panel4.Controls.Add(this.label6);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 144);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(211, 24);
            this.panel4.TabIndex = 0;
            // 
            // buttonClear
            // 
            this.buttonClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonClear.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonClear.Location = new System.Drawing.Point(144, 0);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(67, 24);
            this.buttonClear.TabIndex = 10;
            this.buttonClear.Text = "Clear";
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Left;
            this.label7.Location = new System.Drawing.Point(120, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(24, 24);
            this.label7.TabIndex = 16;
            this.label7.Text = "%";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxMatchingRate
            // 
            this.textBoxMatchingRate.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBoxMatchingRate.Location = new System.Drawing.Point(88, 0);
            this.textBoxMatchingRate.Name = "textBoxMatchingRate";
            this.textBoxMatchingRate.Size = new System.Drawing.Size(32, 21);
            this.textBoxMatchingRate.TabIndex = 15;
            this.textBoxMatchingRate.Text = "70";
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Left;
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 24);
            this.label6.TabIndex = 14;
            this.label6.Text = "Matching Rate";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmBimMapPattern_Fill_Panel
            // 
            // 
            // frmBimMapPattern_Fill_Panel.ClientArea
            // 
            this.frmBimMapPattern_Fill_Panel.ClientArea.Controls.Add(this.gallery);
            this.frmBimMapPattern_Fill_Panel.ClientArea.Controls.Add(this.panelSourceMap);
            this.frmBimMapPattern_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.frmBimMapPattern_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.frmBimMapPattern_Fill_Panel.Location = new System.Drawing.Point(194, 0);
            this.frmBimMapPattern_Fill_Panel.Name = "frmBimMapPattern_Fill_Panel";
            this.frmBimMapPattern_Fill_Panel.Size = new System.Drawing.Size(359, 469);
            this.frmBimMapPattern_Fill_Panel.TabIndex = 0;
            // 
            // gallery
            // 
            this.gallery.CloseButtonVisiable = true;
            this.gallery.Location = new System.Drawing.Point(6, 29);
            this.gallery.Name = "gallery";
            this.gallery.Size = new System.Drawing.Size(347, 180);
            this.gallery.TabIndex = 3;
            this.gallery.OnClose += new DACrux.TEST.ENGUI.TPUCGallery.EventHandlerClose(this.gallery_OnClose);
            // 
            // ultraDockManager
            // 
            this.ultraDockManager.CompressUnpinnedTabs = false;
            dockAreaPane3.DockedBefore = new System.Guid("3868bccc-ee6c-45e7-bb30-8741aeab3679");
            dockableControlPane6.Control = this.fpSpreadTargetMap;
            dockableControlPane6.OriginalControlBounds = new System.Drawing.Rectangle(12, 35, 211, 283);
            dockableControlPane6.Size = new System.Drawing.Size(100, 430);
            dockableControlPane6.Text = "Wafer List";
            dockableGroupPane2.ChildPaneStyle = Infragistics.Win.UltraWinDock.ChildPaneStyle.TabGroup;
            dockableControlPane7.Control = this.pnlBin;
            dockableControlPane7.OriginalControlBounds = new System.Drawing.Rectangle(210, 221, 154, 175);
            dockableControlPane7.Size = new System.Drawing.Size(100, 100);
            dockableControlPane7.Text = "Compare Bin";
            dockableControlPane8.Control = this.fpSpreadDie;
            dockableControlPane8.OriginalControlBounds = new System.Drawing.Rectangle(262, 30, 211, 273);
            dockableControlPane8.Size = new System.Drawing.Size(95, 285);
            dockableControlPane8.Text = "Selected Die";
            dockableGroupPane2.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] {
            dockableControlPane7,
            dockableControlPane8});
            dockableGroupPane2.SelectedTabIndex = 1;
            dockableGroupPane2.Size = new System.Drawing.Size(95, 342);
            dockAreaPane3.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] {
            dockableControlPane6,
            dockableGroupPane2});
            dockAreaPane3.SelectedTabIndex = 1;
            dockAreaPane3.Size = new System.Drawing.Size(189, 469);
            dockableControlPane9.Control = this.pnlSearch;
            dockableControlPane9.OriginalControlBounds = new System.Drawing.Rectangle(579, 90, 245, 295);
            dockableControlPane9.Size = new System.Drawing.Size(100, 100);
            dockableControlPane9.Text = "Search";
            dockableControlPane10.Control = this.fpSpreadFindMap;
            dockableControlPane10.OriginalControlBounds = new System.Drawing.Rectangle(302, 354, 248, 217);
            dockableControlPane10.Size = new System.Drawing.Size(100, 99);
            dockableControlPane10.Text = "Result";
            dockAreaPane4.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] {
            dockableControlPane9,
            dockableControlPane10});
            dockAreaPane4.SelectedTabIndex = 1;
            dockAreaPane4.Size = new System.Drawing.Size(269, 469);
            this.ultraDockManager.DockAreas.AddRange(new Infragistics.Win.UltraWinDock.DockAreaPane[] {
            dockAreaPane3,
            dockAreaPane4});
            this.ultraDockManager.HostControl = this;
            this.ultraDockManager.WindowStyle = Infragistics.Win.UltraWinDock.WindowStyle.VisualStudio2005;
            // 
            // _frmBimMapPatternUnpinnedTabAreaLeft
            // 
            this._frmBimMapPatternUnpinnedTabAreaLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this._frmBimMapPatternUnpinnedTabAreaLeft.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._frmBimMapPatternUnpinnedTabAreaLeft.Location = new System.Drawing.Point(0, 0);
            this._frmBimMapPatternUnpinnedTabAreaLeft.Name = "_frmBimMapPatternUnpinnedTabAreaLeft";
            this._frmBimMapPatternUnpinnedTabAreaLeft.Owner = this.ultraDockManager;
            this._frmBimMapPatternUnpinnedTabAreaLeft.Size = new System.Drawing.Size(0, 469);
            this._frmBimMapPatternUnpinnedTabAreaLeft.TabIndex = 1;
            // 
            // _frmBimMapPatternUnpinnedTabAreaRight
            // 
            this._frmBimMapPatternUnpinnedTabAreaRight.Dock = System.Windows.Forms.DockStyle.Right;
            this._frmBimMapPatternUnpinnedTabAreaRight.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._frmBimMapPatternUnpinnedTabAreaRight.Location = new System.Drawing.Point(827, 0);
            this._frmBimMapPatternUnpinnedTabAreaRight.Name = "_frmBimMapPatternUnpinnedTabAreaRight";
            this._frmBimMapPatternUnpinnedTabAreaRight.Owner = this.ultraDockManager;
            this._frmBimMapPatternUnpinnedTabAreaRight.Size = new System.Drawing.Size(0, 469);
            this._frmBimMapPatternUnpinnedTabAreaRight.TabIndex = 2;
            // 
            // _frmBimMapPatternUnpinnedTabAreaTop
            // 
            this._frmBimMapPatternUnpinnedTabAreaTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._frmBimMapPatternUnpinnedTabAreaTop.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._frmBimMapPatternUnpinnedTabAreaTop.Location = new System.Drawing.Point(0, 0);
            this._frmBimMapPatternUnpinnedTabAreaTop.Name = "_frmBimMapPatternUnpinnedTabAreaTop";
            this._frmBimMapPatternUnpinnedTabAreaTop.Owner = this.ultraDockManager;
            this._frmBimMapPatternUnpinnedTabAreaTop.Size = new System.Drawing.Size(827, 0);
            this._frmBimMapPatternUnpinnedTabAreaTop.TabIndex = 3;
            // 
            // _frmBimMapPatternUnpinnedTabAreaBottom
            // 
            this._frmBimMapPatternUnpinnedTabAreaBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._frmBimMapPatternUnpinnedTabAreaBottom.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._frmBimMapPatternUnpinnedTabAreaBottom.Location = new System.Drawing.Point(0, 469);
            this._frmBimMapPatternUnpinnedTabAreaBottom.Name = "_frmBimMapPatternUnpinnedTabAreaBottom";
            this._frmBimMapPatternUnpinnedTabAreaBottom.Owner = this.ultraDockManager;
            this._frmBimMapPatternUnpinnedTabAreaBottom.Size = new System.Drawing.Size(827, 0);
            this._frmBimMapPatternUnpinnedTabAreaBottom.TabIndex = 4;
            // 
            // _frmBimMapPatternAutoHideControl
            // 
            this._frmBimMapPatternAutoHideControl.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._frmBimMapPatternAutoHideControl.Location = new System.Drawing.Point(0, 0);
            this._frmBimMapPatternAutoHideControl.Name = "_frmBimMapPatternAutoHideControl";
            this._frmBimMapPatternAutoHideControl.Owner = this.ultraDockManager;
            this._frmBimMapPatternAutoHideControl.Size = new System.Drawing.Size(0, 0);
            this._frmBimMapPatternAutoHideControl.TabIndex = 5;
            // 
            // dockableWindow1
            // 
            this.dockableWindow1.Controls.Add(this.fpSpreadTargetMap);
            this.dockableWindow1.Location = new System.Drawing.Point(0, 0);
            this.dockableWindow1.Name = "dockableWindow1";
            this.dockableWindow1.Owner = this.ultraDockManager;
            this.dockableWindow1.Size = new System.Drawing.Size(189, 259);
            this.dockableWindow1.TabIndex = 12;
            // 
            // dockableWindow3
            // 
            this.dockableWindow3.Controls.Add(this.pnlBin);
            this.dockableWindow3.Location = new System.Drawing.Point(-10000, 252);
            this.dockableWindow3.Name = "dockableWindow3";
            this.dockableWindow3.Owner = this.ultraDockManager;
            this.dockableWindow3.Size = new System.Drawing.Size(189, 176);
            this.dockableWindow3.TabIndex = 13;
            // 
            // dockableWindow4
            // 
            this.dockableWindow4.Controls.Add(this.fpSpreadDie);
            this.dockableWindow4.Location = new System.Drawing.Point(0, 264);
            this.dockableWindow4.Name = "dockableWindow4";
            this.dockableWindow4.Owner = this.ultraDockManager;
            this.dockableWindow4.Size = new System.Drawing.Size(189, 185);
            this.dockableWindow4.TabIndex = 14;
            // 
            // windowDockingArea5
            // 
            this.windowDockingArea5.Controls.Add(this.dockableWindow4);
            this.windowDockingArea5.Controls.Add(this.dockableWindow1);
            this.windowDockingArea5.Controls.Add(this.dockableWindow3);
            this.windowDockingArea5.Dock = System.Windows.Forms.DockStyle.Left;
            this.windowDockingArea5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.windowDockingArea5.Location = new System.Drawing.Point(0, 0);
            this.windowDockingArea5.Name = "windowDockingArea5";
            this.windowDockingArea5.Owner = this.ultraDockManager;
            this.windowDockingArea5.Size = new System.Drawing.Size(194, 469);
            this.windowDockingArea5.TabIndex = 10;
            // 
            // dockableWindow5
            // 
            this.dockableWindow5.Controls.Add(this.pnlSearch);
            this.dockableWindow5.Location = new System.Drawing.Point(5, 0);
            this.dockableWindow5.Name = "dockableWindow5";
            this.dockableWindow5.Owner = this.ultraDockManager;
            this.dockableWindow5.Size = new System.Drawing.Size(269, 234);
            this.dockableWindow5.TabIndex = 15;
            // 
            // windowDockingArea2
            // 
            this.windowDockingArea2.Controls.Add(this.dockableWindow2);
            this.windowDockingArea2.Controls.Add(this.dockableWindow5);
            this.windowDockingArea2.Dock = System.Windows.Forms.DockStyle.Right;
            this.windowDockingArea2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.windowDockingArea2.Location = new System.Drawing.Point(553, 0);
            this.windowDockingArea2.Name = "windowDockingArea2";
            this.windowDockingArea2.Owner = this.ultraDockManager;
            this.windowDockingArea2.Size = new System.Drawing.Size(274, 469);
            this.windowDockingArea2.TabIndex = 11;
            // 
            // dockableWindow2
            // 
            this.dockableWindow2.Controls.Add(this.fpSpreadFindMap);
            this.dockableWindow2.Location = new System.Drawing.Point(5, 239);
            this.dockableWindow2.Name = "dockableWindow2";
            this.dockableWindow2.Owner = this.ultraDockManager;
            this.dockableWindow2.Size = new System.Drawing.Size(269, 230);
            this.dockableWindow2.TabIndex = 16;
            // 
            // ctxMnu
            // 
            this.ctxMnu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuAll,
            this.mnuNone,
            this.mnuInvert});
            // 
            // mnuAll
            // 
            this.mnuAll.Index = 0;
            this.mnuAll.Text = "Select All";
            this.mnuAll.Click += new System.EventHandler(this.mnuAll_Click);
            // 
            // mnuNone
            // 
            this.mnuNone.Index = 1;
            this.mnuNone.Text = "Select None";
            this.mnuNone.Click += new System.EventHandler(this.mnuNone_Click);
            // 
            // mnuInvert
            // 
            this.mnuInvert.Index = 2;
            this.mnuInvert.Text = "Select Invert";
            this.mnuInvert.Click += new System.EventHandler(this.mnuInvert_Click);
            // 
            // frmPatternSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 469);
            this.Controls.Add(this._frmBimMapPatternAutoHideControl);
            this.Controls.Add(this.frmBimMapPattern_Fill_Panel);
            this.Controls.Add(this.windowDockingArea2);
            this.Controls.Add(this.windowDockingArea5);
            this.Controls.Add(this._frmBimMapPatternUnpinnedTabAreaBottom);
            this.Controls.Add(this._frmBimMapPatternUnpinnedTabAreaTop);
            this.Controls.Add(this._frmBimMapPatternUnpinnedTabAreaRight);
            this.Controls.Add(this._frmBimMapPatternUnpinnedTabAreaLeft);
            this.Name = "frmPatternSearch";
            this.Text = "Fail Pattern Search";
            this.Load += new System.EventHandler(this.PatternSearch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadTargetMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadTargetMap_Sheet)).EndInit();
            this.pnlBin.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadBin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadBin_Sheet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadDie)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadDie_Sheet)).EndInit();
            this.pnlSearch.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadFindMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadFindMap_Sheet)).EndInit();
            this.panelSourceMap.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.frmBimMapPattern_Fill_Panel.ClientArea.ResumeLayout(false);
            this.frmBimMapPattern_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager)).EndInit();
            this.dockableWindow1.ResumeLayout(false);
            this.dockableWindow3.ResumeLayout(false);
            this.dockableWindow4.ResumeLayout(false);
            this.windowDockingArea5.ResumeLayout(false);
            this.dockableWindow5.ResumeLayout(false);
            this.windowDockingArea2.ResumeLayout(false);
            this.dockableWindow2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSearch;
        private Map.WaferMap tWaferMap;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Button buttonShowMap;
        private System.Windows.Forms.Button buttonPauseResume;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Panel panelSourceMap;
        private Map.WaferMap sWaferMap;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxMatchingRate;
        private System.Windows.Forms.Label label6;
        private Infragistics.Win.Misc.UltraPanel frmBimMapPattern_Fill_Panel;
        private TPUCGallery gallery;
        private FarPoint.Win.Spread.FpSpread fpSpreadTargetMap;
        private FarPoint.Win.Spread.SheetView fpSpreadTargetMap_Sheet;
        private FarPoint.Win.Spread.FpSpread fpSpreadBin;
        private FarPoint.Win.Spread.SheetView fpSpreadBin_Sheet;
        private FarPoint.Win.Spread.FpSpread fpSpreadDie;
        private FarPoint.Win.Spread.SheetView fpSpreadDie_Sheet;
        private Infragistics.Win.UltraWinDock.UltraDockManager ultraDockManager;
        private System.Windows.Forms.Panel pnlBin;
        private System.Windows.Forms.Button butApply;
        private FarPoint.Win.Spread.FpSpread fpSpreadFindMap;
        private FarPoint.Win.Spread.SheetView fpSpreadFindMap_Sheet;
        private Infragistics.Win.UltraWinDock.AutoHideControl _frmBimMapPatternAutoHideControl;
        private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea5;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow4;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow5;
        private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea2;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow3;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow1;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow2;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _frmBimMapPatternUnpinnedTabAreaBottom;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _frmBimMapPatternUnpinnedTabAreaTop;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _frmBimMapPatternUnpinnedTabAreaRight;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _frmBimMapPatternUnpinnedTabAreaLeft;
        private System.Windows.Forms.ContextMenu ctxMnu;
        private System.Windows.Forms.MenuItem mnuAll;
        private System.Windows.Forms.MenuItem mnuNone;
        private System.Windows.Forms.MenuItem mnuInvert;
        private Infragistics.Win.UltraWinProgressBar.UltraProgressBar ultraProgressBar;
    }
}