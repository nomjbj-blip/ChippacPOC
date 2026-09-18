namespace DACrux.SEMDMS.ENGUI
{
    partial class frmDMDataMaint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDMDataMaint));
            this.pnlSizeDetail = new System.Windows.Forms.Panel();
            this.pnlColorListBySize = new System.Windows.Forms.Panel();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpWafer = new System.Windows.Forms.TabPage();
            this.fpSpreadWafer = new FarPoint.Win.Spread.FpSpread();
            this.fpSpreadWafer_fpSpread = new FarPoint.Win.Spread.SheetView();
            this.tpLot = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.fpSpreadLot = new FarPoint.Win.Spread.FpSpread();
            this.fpSpreadLot_fpSpread = new FarPoint.Win.Spread.SheetView();
            this.fpSpreadLotDetail = new FarPoint.Win.Spread.FpSpread();
            this.fpSpreadLotDetail_fpSpread_Size = new FarPoint.Win.Spread.SheetView();
            this.tpHistory = new System.Windows.Forms.TabPage();
            this.fpSpreadHistory = new FarPoint.Win.Spread.FpSpread();
            this.fpSpreadHistory_fpSpread_Size = new FarPoint.Win.Spread.SheetView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.TxtLotID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BtnView = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.pnlSizeTitle = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtComment = new System.Windows.Forms.RichTextBox();
            this.BtnSave = new System.Windows.Forms.Button();
            this.BtnDelete = new System.Windows.Forms.Button();
            this.pnlSizeDetail.SuspendLayout();
            this.pnlColorListBySize.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.tpWafer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadWafer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadWafer_fpSpread)).BeginInit();
            this.tpLot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadLot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadLot_fpSpread)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadLotDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadLotDetail_fpSpread_Size)).BeginInit();
            this.tpHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadHistory_fpSpread_Size)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlSizeTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSizeDetail
            // 
            this.pnlSizeDetail.Controls.Add(this.pnlColorListBySize);
            this.pnlSizeDetail.Controls.Add(this.pnlSizeTitle);
            this.pnlSizeDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSizeDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlSizeDetail.Name = "pnlSizeDetail";
            this.pnlSizeDetail.Size = new System.Drawing.Size(1181, 552);
            this.pnlSizeDetail.TabIndex = 13;
            // 
            // pnlColorListBySize
            // 
            this.pnlColorListBySize.Controls.Add(this.tcMain);
            this.pnlColorListBySize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlColorListBySize.Location = new System.Drawing.Point(0, 74);
            this.pnlColorListBySize.Name = "pnlColorListBySize";
            this.pnlColorListBySize.Size = new System.Drawing.Size(1181, 478);
            this.pnlColorListBySize.TabIndex = 18;
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tpWafer);
            this.tcMain.Controls.Add(this.tpLot);
            this.tcMain.Controls.Add(this.tpHistory);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Location = new System.Drawing.Point(0, 0);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(1181, 478);
            this.tcMain.TabIndex = 11;
            // 
            // tpWafer
            // 
            this.tpWafer.Controls.Add(this.fpSpreadWafer);
            this.tpWafer.Location = new System.Drawing.Point(4, 22);
            this.tpWafer.Name = "tpWafer";
            this.tpWafer.Padding = new System.Windows.Forms.Padding(3);
            this.tpWafer.Size = new System.Drawing.Size(1173, 452);
            this.tpWafer.TabIndex = 0;
            this.tpWafer.Text = "Wafer Base";
            this.tpWafer.UseVisualStyleBackColor = true;
            // 
            // fpSpreadWafer
            // 
            this.fpSpreadWafer.AccessibleDescription = "fpSpread_Size, fpSpread_Size";
            this.fpSpreadWafer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpreadWafer.Location = new System.Drawing.Point(3, 3);
            this.fpSpreadWafer.Name = "fpSpreadWafer";
            this.fpSpreadWafer.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.fpSpreadWafer.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpreadWafer_fpSpread});
            this.fpSpreadWafer.Size = new System.Drawing.Size(1167, 446);
            this.fpSpreadWafer.TabIndex = 11;
            this.fpSpreadWafer.EditModeOff += new System.EventHandler(this.fpSpreadWafer_EditModeOff);
            // 
            // fpSpreadWafer_fpSpread
            // 
            this.fpSpreadWafer_fpSpread.Reset();
            fpSpreadWafer_fpSpread.SheetName = "fpSpread_Size";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpreadWafer_fpSpread.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpSpreadWafer_fpSpread.ColumnCount = 0;
            fpSpreadWafer_fpSpread.RowCount = 0;
            this.fpSpreadWafer_fpSpread.ActiveColumnIndex = -1;
            this.fpSpreadWafer_fpSpread.ActiveRowIndex = -1;
            this.fpSpreadWafer_fpSpread.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin1", System.Drawing.SystemColors.AppWorkspace, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(223)))), ((int)(((byte)(222))))), FarPoint.Win.Spread.GridLines.Horizontal, System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107))))), System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107))))), System.Drawing.Color.White, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(93)))), ((int)(((byte)(90))))), System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(222))))), System.Drawing.Color.White, true, true, true, true, true, true, false, true, "HeaderDefault", "HeaderDefault", "HeaderDefault", "DataAreaDefault", "HeaderDefault");
            this.fpSpreadWafer_fpSpread.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadWafer_fpSpread.ColumnFooter.Columns.Default.Width = 100F;
            this.fpSpreadWafer_fpSpread.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadWafer_fpSpread.ColumnFooter.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadWafer_fpSpread.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadWafer_fpSpread.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadWafer_fpSpread.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadWafer_fpSpread.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadWafer_fpSpread.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadWafer_fpSpread.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadWafer_fpSpread.ColumnFooterSheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadWafer_fpSpread.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadWafer_fpSpread.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadWafer_fpSpread.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadWafer_fpSpread.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadWafer_fpSpread.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadWafer_fpSpread.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadWafer_fpSpread.ColumnHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadWafer_fpSpread.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadWafer_fpSpread.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadWafer_fpSpread.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadWafer_fpSpread.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadWafer_fpSpread.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadWafer_fpSpread.Columns.Default.Width = 100F;
            this.fpSpreadWafer_fpSpread.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpSpreadWafer_fpSpread.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpSpreadWafer_fpSpread.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadWafer_fpSpread.DefaultStyle.Parent = "DataAreaDefault";
            this.fpSpreadWafer_fpSpread.RowHeader.Columns.Default.Resizable = false;
            this.fpSpreadWafer_fpSpread.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadWafer_fpSpread.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadWafer_fpSpread.RowHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadWafer_fpSpread.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadWafer_fpSpread.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadWafer_fpSpread.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadWafer_fpSpread.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadWafer_fpSpread.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadWafer_fpSpread.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadWafer_fpSpread.SheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadWafer_fpSpread.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadWafer_fpSpread.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadWafer_fpSpread.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadWafer_fpSpread.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadWafer_fpSpread.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // tpLot
            // 
            this.tpLot.Controls.Add(this.splitContainer1);
            this.tpLot.Location = new System.Drawing.Point(4, 22);
            this.tpLot.Name = "tpLot";
            this.tpLot.Padding = new System.Windows.Forms.Padding(3);
            this.tpLot.Size = new System.Drawing.Size(1173, 452);
            this.tpLot.TabIndex = 1;
            this.tpLot.Text = "Lot Base";
            this.tpLot.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.fpSpreadLot);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.fpSpreadLotDetail);
            this.splitContainer1.Size = new System.Drawing.Size(1167, 446);
            this.splitContainer1.SplitterDistance = 225;
            this.splitContainer1.TabIndex = 11;
            // 
            // fpSpreadLot
            // 
            this.fpSpreadLot.AccessibleDescription = "fpSpread_Size, fpSpread_Size";
            this.fpSpreadLot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpreadLot.Location = new System.Drawing.Point(0, 0);
            this.fpSpreadLot.Name = "fpSpreadLot";
            this.fpSpreadLot.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.fpSpreadLot.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpreadLot_fpSpread});
            this.fpSpreadLot.Size = new System.Drawing.Size(1165, 223);
            this.fpSpreadLot.TabIndex = 10;
            this.fpSpreadLot.EditModeOff += new System.EventHandler(this.fpSpreadLot_EditModeOff);
            this.fpSpreadLot.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpSpreadLot_CellClick);
            this.fpSpreadLot.SetActiveViewport(0, -1, -1);
            // 
            // fpSpreadLot_fpSpread
            // 
            this.fpSpreadLot_fpSpread.Reset();
            fpSpreadLot_fpSpread.SheetName = "fpSpread_Size";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpreadLot_fpSpread.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpSpreadLot_fpSpread.ColumnCount = 0;
            fpSpreadLot_fpSpread.RowCount = 0;
            this.fpSpreadLot_fpSpread.ActiveColumnIndex = -1;
            this.fpSpreadLot_fpSpread.ActiveRowIndex = -1;
            this.fpSpreadLot_fpSpread.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin1", System.Drawing.SystemColors.AppWorkspace, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(223)))), ((int)(((byte)(222))))), FarPoint.Win.Spread.GridLines.Horizontal, System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107))))), System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107))))), System.Drawing.Color.White, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(93)))), ((int)(((byte)(90))))), System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(222))))), System.Drawing.Color.White, true, true, true, true, true, true, false, true, "HeaderDefault", "HeaderDefault", "HeaderDefault", "DataAreaDefault", "HeaderDefault");
            this.fpSpreadLot_fpSpread.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadLot_fpSpread.ColumnFooter.Columns.Default.Width = 100F;
            this.fpSpreadLot_fpSpread.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadLot_fpSpread.ColumnFooter.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadLot_fpSpread.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadLot_fpSpread.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadLot_fpSpread.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadLot_fpSpread.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadLot_fpSpread.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadLot_fpSpread.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadLot_fpSpread.ColumnFooterSheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadLot_fpSpread.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadLot_fpSpread.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadLot_fpSpread.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadLot_fpSpread.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadLot_fpSpread.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadLot_fpSpread.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadLot_fpSpread.ColumnHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadLot_fpSpread.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadLot_fpSpread.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadLot_fpSpread.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadLot_fpSpread.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadLot_fpSpread.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadLot_fpSpread.Columns.Default.Width = 100F;
            this.fpSpreadLot_fpSpread.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpSpreadLot_fpSpread.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpSpreadLot_fpSpread.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadLot_fpSpread.DefaultStyle.Parent = "DataAreaDefault";
            this.fpSpreadLot_fpSpread.RowHeader.Columns.Default.Resizable = false;
            this.fpSpreadLot_fpSpread.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadLot_fpSpread.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadLot_fpSpread.RowHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadLot_fpSpread.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadLot_fpSpread.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadLot_fpSpread.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadLot_fpSpread.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadLot_fpSpread.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadLot_fpSpread.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadLot_fpSpread.SheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadLot_fpSpread.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadLot_fpSpread.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadLot_fpSpread.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadLot_fpSpread.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadLot_fpSpread.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // fpSpreadLotDetail
            // 
            this.fpSpreadLotDetail.AccessibleDescription = "fpSpread_Size, fpSpread_Size";
            this.fpSpreadLotDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpreadLotDetail.Location = new System.Drawing.Point(0, 0);
            this.fpSpreadLotDetail.Name = "fpSpreadLotDetail";
            this.fpSpreadLotDetail.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.fpSpreadLotDetail.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpreadLotDetail_fpSpread_Size});
            this.fpSpreadLotDetail.Size = new System.Drawing.Size(1165, 215);
            this.fpSpreadLotDetail.TabIndex = 11;
            // 
            // fpSpreadLotDetail_fpSpread_Size
            // 
            this.fpSpreadLotDetail_fpSpread_Size.Reset();
            fpSpreadLotDetail_fpSpread_Size.SheetName = "fpSpread_Size";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpreadLotDetail_fpSpread_Size.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpSpreadLotDetail_fpSpread_Size.ColumnCount = 0;
            fpSpreadLotDetail_fpSpread_Size.RowCount = 0;
            this.fpSpreadLotDetail_fpSpread_Size.ActiveColumnIndex = -1;
            this.fpSpreadLotDetail_fpSpread_Size.ActiveRowIndex = -1;
            this.fpSpreadLotDetail_fpSpread_Size.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin1", System.Drawing.SystemColors.AppWorkspace, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(223)))), ((int)(((byte)(222))))), FarPoint.Win.Spread.GridLines.Horizontal, System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107))))), System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107))))), System.Drawing.Color.White, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(93)))), ((int)(((byte)(90))))), System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(222))))), System.Drawing.Color.White, true, true, true, true, true, true, false, true, "HeaderDefault", "HeaderDefault", "HeaderDefault", "DataAreaDefault", "HeaderDefault");
            this.fpSpreadLotDetail_fpSpread_Size.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadLotDetail_fpSpread_Size.ColumnFooter.Columns.Default.Width = 100F;
            this.fpSpreadLotDetail_fpSpread_Size.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadLotDetail_fpSpread_Size.ColumnFooter.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadLotDetail_fpSpread_Size.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadLotDetail_fpSpread_Size.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadLotDetail_fpSpread_Size.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadLotDetail_fpSpread_Size.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadLotDetail_fpSpread_Size.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadLotDetail_fpSpread_Size.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadLotDetail_fpSpread_Size.ColumnFooterSheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadLotDetail_fpSpread_Size.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadLotDetail_fpSpread_Size.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadLotDetail_fpSpread_Size.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadLotDetail_fpSpread_Size.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadLotDetail_fpSpread_Size.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadLotDetail_fpSpread_Size.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadLotDetail_fpSpread_Size.ColumnHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadLotDetail_fpSpread_Size.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadLotDetail_fpSpread_Size.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadLotDetail_fpSpread_Size.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadLotDetail_fpSpread_Size.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadLotDetail_fpSpread_Size.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadLotDetail_fpSpread_Size.Columns.Default.Width = 100F;
            this.fpSpreadLotDetail_fpSpread_Size.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpSpreadLotDetail_fpSpread_Size.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpSpreadLotDetail_fpSpread_Size.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadLotDetail_fpSpread_Size.DefaultStyle.Parent = "DataAreaDefault";
            this.fpSpreadLotDetail_fpSpread_Size.RowHeader.Columns.Default.Resizable = false;
            this.fpSpreadLotDetail_fpSpread_Size.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadLotDetail_fpSpread_Size.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadLotDetail_fpSpread_Size.RowHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadLotDetail_fpSpread_Size.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadLotDetail_fpSpread_Size.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadLotDetail_fpSpread_Size.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadLotDetail_fpSpread_Size.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadLotDetail_fpSpread_Size.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadLotDetail_fpSpread_Size.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadLotDetail_fpSpread_Size.SheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadLotDetail_fpSpread_Size.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadLotDetail_fpSpread_Size.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadLotDetail_fpSpread_Size.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadLotDetail_fpSpread_Size.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadLotDetail_fpSpread_Size.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // tpHistory
            // 
            this.tpHistory.Controls.Add(this.fpSpreadHistory);
            this.tpHistory.Controls.Add(this.panel1);
            this.tpHistory.Location = new System.Drawing.Point(4, 22);
            this.tpHistory.Name = "tpHistory";
            this.tpHistory.Padding = new System.Windows.Forms.Padding(3);
            this.tpHistory.Size = new System.Drawing.Size(1173, 452);
            this.tpHistory.TabIndex = 2;
            this.tpHistory.Text = "Maint History";
            this.tpHistory.UseVisualStyleBackColor = true;
            // 
            // fpSpreadHistory
            // 
            this.fpSpreadHistory.AccessibleDescription = "fpSpread_Size, fpSpread_Size";
            this.fpSpreadHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpreadHistory.Location = new System.Drawing.Point(3, 42);
            this.fpSpreadHistory.Name = "fpSpreadHistory";
            this.fpSpreadHistory.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.fpSpreadHistory.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpreadHistory_fpSpread_Size});
            this.fpSpreadHistory.Size = new System.Drawing.Size(1167, 407);
            this.fpSpreadHistory.TabIndex = 11;
            // 
            // fpSpreadHistory_fpSpread_Size
            // 
            this.fpSpreadHistory_fpSpread_Size.Reset();
            fpSpreadHistory_fpSpread_Size.SheetName = "fpSpread_Size";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpreadHistory_fpSpread_Size.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpSpreadHistory_fpSpread_Size.ColumnCount = 0;
            fpSpreadHistory_fpSpread_Size.RowCount = 0;
            this.fpSpreadHistory_fpSpread_Size.ActiveColumnIndex = -1;
            this.fpSpreadHistory_fpSpread_Size.ActiveRowIndex = -1;
            this.fpSpreadHistory_fpSpread_Size.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin1", System.Drawing.SystemColors.AppWorkspace, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(223)))), ((int)(((byte)(222))))), FarPoint.Win.Spread.GridLines.Horizontal, System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107))))), System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107))))), System.Drawing.Color.White, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(93)))), ((int)(((byte)(90))))), System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(222))))), System.Drawing.Color.White, true, true, true, true, true, true, false, true, "HeaderDefault", "HeaderDefault", "HeaderDefault", "DataAreaDefault", "HeaderDefault");
            this.fpSpreadHistory_fpSpread_Size.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadHistory_fpSpread_Size.ColumnFooter.Columns.Default.Width = 100F;
            this.fpSpreadHistory_fpSpread_Size.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadHistory_fpSpread_Size.ColumnFooter.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadHistory_fpSpread_Size.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadHistory_fpSpread_Size.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadHistory_fpSpread_Size.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadHistory_fpSpread_Size.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadHistory_fpSpread_Size.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadHistory_fpSpread_Size.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadHistory_fpSpread_Size.ColumnFooterSheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadHistory_fpSpread_Size.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadHistory_fpSpread_Size.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadHistory_fpSpread_Size.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadHistory_fpSpread_Size.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadHistory_fpSpread_Size.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadHistory_fpSpread_Size.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadHistory_fpSpread_Size.ColumnHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadHistory_fpSpread_Size.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadHistory_fpSpread_Size.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadHistory_fpSpread_Size.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadHistory_fpSpread_Size.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadHistory_fpSpread_Size.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadHistory_fpSpread_Size.Columns.Default.Width = 100F;
            this.fpSpreadHistory_fpSpread_Size.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpSpreadHistory_fpSpread_Size.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpSpreadHistory_fpSpread_Size.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadHistory_fpSpread_Size.DefaultStyle.Parent = "DataAreaDefault";
            this.fpSpreadHistory_fpSpread_Size.RowHeader.Columns.Default.Resizable = false;
            this.fpSpreadHistory_fpSpread_Size.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadHistory_fpSpread_Size.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadHistory_fpSpread_Size.RowHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadHistory_fpSpread_Size.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadHistory_fpSpread_Size.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadHistory_fpSpread_Size.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadHistory_fpSpread_Size.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadHistory_fpSpread_Size.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadHistory_fpSpread_Size.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadHistory_fpSpread_Size.SheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadHistory_fpSpread_Size.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadHistory_fpSpread_Size.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadHistory_fpSpread_Size.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadHistory_fpSpread_Size.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadHistory_fpSpread_Size.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.TxtLotID);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.BtnView);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.dtpStart);
            this.panel1.Controls.Add(this.dtpEnd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1167, 39);
            this.panel1.TabIndex = 12;
            // 
            // TxtLotID
            // 
            this.TxtLotID.Location = new System.Drawing.Point(415, 9);
            this.TxtLotID.Name = "TxtLotID";
            this.TxtLotID.Size = new System.Drawing.Size(175, 21);
            this.TxtLotID.TabIndex = 116;
            this.TxtLotID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtLotID_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(362, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 12);
            this.label2.TabIndex = 115;
            this.label2.Text = "Lot ID";
            // 
            // BtnView
            // 
            this.BtnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnView.Image = ((System.Drawing.Image)(resources.GetObject("BtnView.Image")));
            this.BtnView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnView.Location = new System.Drawing.Point(1028, 4);
            this.BtnView.Name = "BtnView";
            this.BtnView.Size = new System.Drawing.Size(134, 32);
            this.BtnView.TabIndex = 114;
            this.BtnView.Text = "View";
            this.BtnView.UseVisualStyleBackColor = true;
            this.BtnView.Click += new System.EventHandler(this.BtnView_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(182, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 12);
            this.label7.TabIndex = 8;
            this.label7.Text = "~ To";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "From";
            // 
            // dtpStart
            // 
            this.dtpStart.CustomFormat = "yyyy-MM-dd";
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStart.Location = new System.Drawing.Point(67, 9);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(105, 21);
            this.dtpStart.TabIndex = 6;
            // 
            // dtpEnd
            // 
            this.dtpEnd.CustomFormat = "yyyy-MM-dd";
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEnd.Location = new System.Drawing.Point(234, 9);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(105, 21);
            this.dtpEnd.TabIndex = 7;
            // 
            // pnlSizeTitle
            // 
            this.pnlSizeTitle.BackColor = System.Drawing.SystemColors.Control;
            this.pnlSizeTitle.Controls.Add(this.label1);
            this.pnlSizeTitle.Controls.Add(this.TxtComment);
            this.pnlSizeTitle.Controls.Add(this.BtnDelete);
            this.pnlSizeTitle.Controls.Add(this.BtnSave);
            this.pnlSizeTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSizeTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlSizeTitle.Name = "pnlSizeTitle";
            this.pnlSizeTitle.Size = new System.Drawing.Size(1181, 74);
            this.pnlSizeTitle.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 12);
            this.label1.TabIndex = 116;
            this.label1.Text = "Comment";
            // 
            // TxtComment
            // 
            this.TxtComment.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TxtComment.Location = new System.Drawing.Point(0, 44);
            this.TxtComment.Name = "TxtComment";
            this.TxtComment.Size = new System.Drawing.Size(1181, 30);
            this.TxtComment.TabIndex = 115;
            this.TxtComment.Text = "";
            // 
            // BtnSave
            // 
            this.BtnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSave.Image = ((System.Drawing.Image)(resources.GetObject("BtnSave.Image")));
            this.BtnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnSave.Location = new System.Drawing.Point(753, 6);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(211, 32);
            this.BtnSave.TabIndex = 114;
            this.BtnSave.Text = "Save";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnDelete
            // 
            this.BtnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDelete.Image = ((System.Drawing.Image)(resources.GetObject("BtnDelete.Image")));
            this.BtnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnDelete.Location = new System.Drawing.Point(970, 6);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(211, 32);
            this.BtnDelete.TabIndex = 114;
            this.BtnDelete.Text = "Delete";
            this.BtnDelete.UseVisualStyleBackColor = true;
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // frmDMDataMaint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1181, 552);
            this.Controls.Add(this.pnlSizeDetail);
            this.Name = "frmDMDataMaint";
            this.Text = "DM Data Maint";
            this.Load += new System.EventHandler(this.frmDMDataMaint_Load);
            this.pnlSizeDetail.ResumeLayout(false);
            this.pnlColorListBySize.ResumeLayout(false);
            this.tcMain.ResumeLayout(false);
            this.tpWafer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadWafer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadWafer_fpSpread)).EndInit();
            this.tpLot.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadLot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadLot_fpSpread)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadLotDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadLotDetail_fpSpread_Size)).EndInit();
            this.tpHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadHistory_fpSpread_Size)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlSizeTitle.ResumeLayout(false);
            this.pnlSizeTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSizeDetail;
        private FarPoint.Win.Spread.FpSpread fpSpreadLot;
        private FarPoint.Win.Spread.SheetView fpSpreadLot_fpSpread;
        private System.Windows.Forms.Panel pnlSizeTitle;
        private System.Windows.Forms.Panel pnlColorListBySize;
        private System.Windows.Forms.Button BtnSave;
        private FarPoint.Win.Spread.FpSpread fpSpreadWafer;
        private FarPoint.Win.Spread.SheetView fpSpreadWafer_fpSpread;
        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tpWafer;
        private System.Windows.Forms.TabPage tpLot;
        private System.Windows.Forms.TabPage tpHistory;
        private FarPoint.Win.Spread.FpSpread fpSpreadHistory;
        private FarPoint.Win.Spread.SheetView fpSpreadHistory_fpSpread_Size;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Button BtnView;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private FarPoint.Win.Spread.FpSpread fpSpreadLotDetail;
        private FarPoint.Win.Spread.SheetView fpSpreadLotDetail_fpSpread_Size;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox TxtComment;
        private System.Windows.Forms.TextBox TxtLotID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtnDelete;

    }
}