namespace DACrux.TEST.ENGUI
{
    // File Map Setup(KLARF 기준) 화면의 컨트롤과 배치를 정의한다. 파일 읽기 / 원형 생성 / 저장 동작은 본문 파일에서 처리한다.
    partial class frmStandardMapSetup
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>화면 종료 시 디자이너 구성 요소를 해제한다.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>파일 선택 / 저장 설정 / 형상 미리보기를 WinForms 디자이너 구성으로 만든다.</summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStandardMapSetup));
            this.fileBar = new System.Windows.Forms.Panel();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnParse = new System.Windows.Forms.Button();
            this.settings = new System.Windows.Forms.Panel();
            this.lblMapID = new System.Windows.Forms.Label();
            this.txtMapID = new System.Windows.Forms.TextBox();
            this.lblKlarfWafer = new System.Windows.Forms.Label();
            this.cmbKlarfWafer = new System.Windows.Forms.ComboBox();
            this.lblWafer = new System.Windows.Forms.Label();
            this.cmbWaferSize = new System.Windows.Forms.ComboBox();
            this.lblEdge = new System.Windows.Forms.Label();
            this.numEdge = new System.Windows.Forms.NumericUpDown();
            this.lblDirection = new System.Windows.Forms.Label();
            this.cmbXYDirection = new System.Windows.Forms.ComboBox();
            this.lblFlatZone = new System.Windows.Forms.Label();
            this.cmbFlatZone = new System.Windows.Forms.ComboBox();
            this.lblMarkType = new System.Windows.Forms.Label();
            this.cmbMarkType = new System.Windows.Forms.ComboBox();
            this.lblOrigin = new System.Windows.Forms.Label();
            this.lblOriginX = new System.Windows.Forms.Label();
            this.lblOriginY = new System.Windows.Forms.Label();
            this.numOriginX = new System.Windows.Forms.NumericUpDown();
            this.numOriginY = new System.Windows.Forms.NumericUpDown();
            this.lblPitch = new System.Windows.Forms.Label();
            this.lblPitchX = new System.Windows.Forms.Label();
            this.lblPitchY = new System.Windows.Forms.Label();
            this.numPitchX = new System.Windows.Forms.NumericUpDown();
            this.numPitchY = new System.Windows.Forms.NumericUpDown();
            this.btnPitchKlarf = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.chkDieNumber = new System.Windows.Forms.CheckBox();
            this.lblEditMode = new System.Windows.Forms.Label();
            this.rdoView = new System.Windows.Forms.RadioButton();
            this.rdoRemove = new System.Windows.Forms.RadioButton();
            this.rdoRestore = new System.Windows.Forms.RadioButton();
            this.btnRemoveNotInspected = new System.Windows.Forms.Button();
            this.btnRemoveOutside = new System.Windows.Forms.Button();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnOpenMap = new System.Windows.Forms.Button();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.busyBar = new DACrux.SEMDMS.Control.ProgressStatusBar();
            this.lblCoordinates = new System.Windows.Forms.Label();
            this.lblDieCount = new System.Windows.Forms.Label();
            this.m_wMap = new DACrux.TEST.ENGUI.StandardMapEditMap();
            this.fileBar.SuspendLayout();
            this.settings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEdge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOriginX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOriginY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPitchX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPitchY)).BeginInit();
            this.pnlStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileBar
            // 
            this.fileBar.Controls.Add(this.txtFile);
            this.fileBar.Controls.Add(this.btnBrowse);
            this.fileBar.Controls.Add(this.btnParse);
            this.fileBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.fileBar.Location = new System.Drawing.Point(0, 0);
            this.fileBar.Name = "fileBar";
            this.fileBar.Size = new System.Drawing.Size(1634, 52);
            this.fileBar.TabIndex = 2;
            // 
            // txtFile
            // 
            this.txtFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFile.Location = new System.Drawing.Point(12, 14);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(1402, 23);
            this.txtFile.TabIndex = 0;
            this.txtFile.TextChanged += new System.EventHandler(this.txtFile_TextChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowse.Image")));
            this.btnBrowse.Location = new System.Drawing.Point(1424, 12);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBrowse.Size = new System.Drawing.Size(96, 28);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "파일 선택";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnParse
            // 
            this.btnParse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnParse.Image = ((System.Drawing.Image)(resources.GetObject("btnParse.Image")));
            this.btnParse.Location = new System.Drawing.Point(1526, 12);
            this.btnParse.Name = "btnParse";
            this.btnParse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnParse.Size = new System.Drawing.Size(96, 28);
            this.btnParse.TabIndex = 2;
            this.btnParse.Text = "다시 읽기";
            this.btnParse.UseVisualStyleBackColor = true;
            this.btnParse.Click += new System.EventHandler(this.btnParse_Click);
            // 
            // settings
            // 
            this.settings.AutoScroll = true;
            this.settings.Controls.Add(this.lblMapID);
            this.settings.Controls.Add(this.txtMapID);
            this.settings.Controls.Add(this.lblKlarfWafer);
            this.settings.Controls.Add(this.cmbKlarfWafer);
            this.settings.Controls.Add(this.lblWafer);
            this.settings.Controls.Add(this.cmbWaferSize);
            this.settings.Controls.Add(this.lblEdge);
            this.settings.Controls.Add(this.numEdge);
            this.settings.Controls.Add(this.lblDirection);
            this.settings.Controls.Add(this.cmbXYDirection);
            this.settings.Controls.Add(this.lblFlatZone);
            this.settings.Controls.Add(this.cmbFlatZone);
            this.settings.Controls.Add(this.lblMarkType);
            this.settings.Controls.Add(this.cmbMarkType);
            this.settings.Controls.Add(this.lblOrigin);
            this.settings.Controls.Add(this.lblOriginX);
            this.settings.Controls.Add(this.lblOriginY);
            this.settings.Controls.Add(this.numOriginX);
            this.settings.Controls.Add(this.numOriginY);
            this.settings.Controls.Add(this.lblPitch);
            this.settings.Controls.Add(this.lblPitchX);
            this.settings.Controls.Add(this.lblPitchY);
            this.settings.Controls.Add(this.numPitchX);
            this.settings.Controls.Add(this.numPitchY);
            this.settings.Controls.Add(this.btnPitchKlarf);
            this.settings.Controls.Add(this.btnGenerate);
            this.settings.Controls.Add(this.chkDieNumber);
            this.settings.Controls.Add(this.lblEditMode);
            this.settings.Controls.Add(this.rdoView);
            this.settings.Controls.Add(this.rdoRemove);
            this.settings.Controls.Add(this.rdoRestore);
            this.settings.Controls.Add(this.btnRemoveNotInspected);
            this.settings.Controls.Add(this.btnRemoveOutside);
            this.settings.Controls.Add(this.txtInfo);
            this.settings.Controls.Add(this.btnSave);
            this.settings.Controls.Add(this.btnOpenMap);
            this.settings.Dock = System.Windows.Forms.DockStyle.Left;
            this.settings.Location = new System.Drawing.Point(0, 52);
            this.settings.Name = "settings";
            this.settings.Size = new System.Drawing.Size(310, 874);
            this.settings.TabIndex = 1;
            // 
            // lblMapID
            // 
            this.lblMapID.Location = new System.Drawing.Point(12, 10);
            this.lblMapID.Name = "lblMapID";
            this.lblMapID.Size = new System.Drawing.Size(280, 22);
            this.lblMapID.TabIndex = 0;
            this.lblMapID.Text = "새 Map ID";
            // 
            // txtMapID
            // 
            this.txtMapID.Location = new System.Drawing.Point(12, 34);
            this.txtMapID.Name = "txtMapID";
            this.txtMapID.Size = new System.Drawing.Size(280, 31);
            this.txtMapID.TabIndex = 0;
            // 
            // lblKlarfWafer
            // 
            this.lblKlarfWafer.Location = new System.Drawing.Point(12, 68);
            this.lblKlarfWafer.Name = "lblKlarfWafer";
            this.lblKlarfWafer.Size = new System.Drawing.Size(280, 22);
            this.lblKlarfWafer.TabIndex = 10;
            this.lblKlarfWafer.Text = "KLARF Wafer";
            // 
            // cmbKlarfWafer
            // 
            this.cmbKlarfWafer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKlarfWafer.Enabled = false;
            this.cmbKlarfWafer.Location = new System.Drawing.Point(12, 92);
            this.cmbKlarfWafer.Name = "cmbKlarfWafer";
            this.cmbKlarfWafer.Size = new System.Drawing.Size(280, 33);
            this.cmbKlarfWafer.TabIndex = 1;
            this.cmbKlarfWafer.SelectedIndexChanged += new System.EventHandler(this.cmbKlarfWafer_SelectedIndexChanged);
            // 
            // lblWafer
            // 
            this.lblWafer.Location = new System.Drawing.Point(12, 130);
            this.lblWafer.Name = "lblWafer";
            this.lblWafer.Size = new System.Drawing.Size(280, 22);
            this.lblWafer.TabIndex = 1;
            this.lblWafer.Text = "Wafer 크기 (mm)";
            // 
            // cmbWaferSize
            // 
            this.cmbWaferSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWaferSize.Items.AddRange(new object[] {
            "75",
            "100",
            "125",
            "150",
            "200",
            "300"});
            this.cmbWaferSize.Location = new System.Drawing.Point(12, 154);
            this.cmbWaferSize.Name = "cmbWaferSize";
            this.cmbWaferSize.Size = new System.Drawing.Size(280, 33);
            this.cmbWaferSize.TabIndex = 2;
            this.cmbWaferSize.SelectedIndexChanged += new System.EventHandler(this.MapSettingsChanged);
            // 
            // lblEdge
            // 
            this.lblEdge.Location = new System.Drawing.Point(12, 192);
            this.lblEdge.Name = "lblEdge";
            this.lblEdge.Size = new System.Drawing.Size(135, 22);
            this.lblEdge.TabIndex = 11;
            this.lblEdge.Text = "Edge (µm)";
            // 
            // numEdge
            // 
            this.numEdge.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numEdge.Location = new System.Drawing.Point(12, 216);
            this.numEdge.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numEdge.Name = "numEdge";
            this.numEdge.Size = new System.Drawing.Size(135, 31);
            this.numEdge.TabIndex = 3;
            this.numEdge.ValueChanged += new System.EventHandler(this.MapSettingsChanged);
            // 
            // lblDirection
            // 
            this.lblDirection.Location = new System.Drawing.Point(157, 192);
            this.lblDirection.Name = "lblDirection";
            this.lblDirection.Size = new System.Drawing.Size(135, 22);
            this.lblDirection.TabIndex = 12;
            this.lblDirection.Text = "XY 방향";
            // 
            // cmbXYDirection
            // 
            this.cmbXYDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbXYDirection.Items.AddRange(new object[] {
            "LeftBottom",
            "LeftTop",
            "RightBottom",
            "RightTop"});
            this.cmbXYDirection.Location = new System.Drawing.Point(157, 216);
            this.cmbXYDirection.Name = "cmbXYDirection";
            this.cmbXYDirection.Size = new System.Drawing.Size(135, 33);
            this.cmbXYDirection.TabIndex = 4;
            this.cmbXYDirection.SelectedIndexChanged += new System.EventHandler(this.MapSettingsChanged);
            // 
            // lblFlatZone
            // 
            this.lblFlatZone.Location = new System.Drawing.Point(12, 256);
            this.lblFlatZone.Name = "lblFlatZone";
            this.lblFlatZone.Size = new System.Drawing.Size(135, 22);
            this.lblFlatZone.TabIndex = 19;
            this.lblFlatZone.Text = "Flat Zone 위치";
            // 
            // cmbFlatZone
            // 
            this.cmbFlatZone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFlatZone.Items.AddRange(new object[] {
            "Bottom",
            "Top",
            "Left",
            "Right"});
            this.cmbFlatZone.Location = new System.Drawing.Point(12, 280);
            this.cmbFlatZone.Name = "cmbFlatZone";
            this.cmbFlatZone.Size = new System.Drawing.Size(135, 33);
            this.cmbFlatZone.TabIndex = 20;
            this.cmbFlatZone.SelectedIndexChanged += new System.EventHandler(this.MapSettingsChanged);
            // 
            // lblMarkType
            // 
            this.lblMarkType.Location = new System.Drawing.Point(157, 256);
            this.lblMarkType.Name = "lblMarkType";
            this.lblMarkType.Size = new System.Drawing.Size(135, 22);
            this.lblMarkType.TabIndex = 21;
            this.lblMarkType.Text = "형태 (Notch / Flat)";
            // 
            // cmbMarkType
            // 
            this.cmbMarkType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMarkType.Items.AddRange(new object[] {
            "Notch",
            "Flat"});
            this.cmbMarkType.Location = new System.Drawing.Point(157, 280);
            this.cmbMarkType.Name = "cmbMarkType";
            this.cmbMarkType.Size = new System.Drawing.Size(135, 33);
            this.cmbMarkType.TabIndex = 22;
            this.cmbMarkType.SelectedIndexChanged += new System.EventHandler(this.MapSettingsChanged);
            // 
            // lblOrigin
            // 
            this.lblOrigin.Location = new System.Drawing.Point(12, 320);
            this.lblOrigin.Name = "lblOrigin";
            this.lblOrigin.Size = new System.Drawing.Size(290, 22);
            this.lblOrigin.TabIndex = 2;
            this.lblOrigin.Text = "원점 (µm)";
            // 
            // lblOriginX
            // 
            this.lblOriginX.Location = new System.Drawing.Point(12, 352);
            this.lblOriginX.Name = "lblOriginX";
            this.lblOriginX.Size = new System.Drawing.Size(20, 22);
            this.lblOriginX.TabIndex = 3;
            this.lblOriginX.Text = "X";
            // 
            // lblOriginY
            // 
            this.lblOriginY.Location = new System.Drawing.Point(157, 352);
            this.lblOriginY.Name = "lblOriginY";
            this.lblOriginY.Size = new System.Drawing.Size(20, 22);
            this.lblOriginY.TabIndex = 4;
            this.lblOriginY.Text = "Y";
            // 
            // numOriginX
            // 
            this.numOriginX.DecimalPlaces = 3;
            this.numOriginX.Location = new System.Drawing.Point(32, 349);
            this.numOriginX.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numOriginX.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.numOriginX.Name = "numOriginX";
            this.numOriginX.Size = new System.Drawing.Size(115, 31);
            this.numOriginX.TabIndex = 5;
            this.numOriginX.ValueChanged += new System.EventHandler(this.MapSettingsChanged);
            // 
            // numOriginY
            // 
            this.numOriginY.DecimalPlaces = 3;
            this.numOriginY.Location = new System.Drawing.Point(177, 349);
            this.numOriginY.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numOriginY.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.numOriginY.Name = "numOriginY";
            this.numOriginY.Size = new System.Drawing.Size(115, 31);
            this.numOriginY.TabIndex = 6;
            this.numOriginY.ValueChanged += new System.EventHandler(this.MapSettingsChanged);
            // 
            // lblPitch
            // 
            this.lblPitch.Location = new System.Drawing.Point(12, 388);
            this.lblPitch.Name = "lblPitch";
            this.lblPitch.Size = new System.Drawing.Size(290, 22);
            this.lblPitch.TabIndex = 23;
            this.lblPitch.Text = "Die Pitch (µm)";
            // 
            // lblPitchX
            // 
            this.lblPitchX.Location = new System.Drawing.Point(12, 420);
            this.lblPitchX.Name = "lblPitchX";
            this.lblPitchX.Size = new System.Drawing.Size(20, 22);
            this.lblPitchX.TabIndex = 24;
            this.lblPitchX.Text = "X";
            // 
            // lblPitchY
            // 
            this.lblPitchY.Location = new System.Drawing.Point(157, 420);
            this.lblPitchY.Name = "lblPitchY";
            this.lblPitchY.Size = new System.Drawing.Size(20, 22);
            this.lblPitchY.TabIndex = 25;
            this.lblPitchY.Text = "Y";
            // 
            // numPitchX
            // 
            this.numPitchX.DecimalPlaces = 3;
            this.numPitchX.Location = new System.Drawing.Point(32, 417);
            this.numPitchX.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numPitchX.Name = "numPitchX";
            this.numPitchX.Size = new System.Drawing.Size(115, 31);
            this.numPitchX.TabIndex = 26;
            this.numPitchX.ThousandsSeparator = true;
            this.numPitchX.ValueChanged += new System.EventHandler(this.MapSettingsChanged);
            // 
            // numPitchY
            // 
            this.numPitchY.DecimalPlaces = 3;
            this.numPitchY.Location = new System.Drawing.Point(177, 417);
            this.numPitchY.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numPitchY.Name = "numPitchY";
            this.numPitchY.Size = new System.Drawing.Size(115, 31);
            this.numPitchY.TabIndex = 27;
            this.numPitchY.ThousandsSeparator = true;
            this.numPitchY.ValueChanged += new System.EventHandler(this.MapSettingsChanged);
            // 
            // btnPitchKlarf
            // 
            this.btnPitchKlarf.Enabled = false;
            this.btnPitchKlarf.Image = ((System.Drawing.Image)(resources.GetObject("btnPitchKlarf.Image")));
            this.btnPitchKlarf.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPitchKlarf.Location = new System.Drawing.Point(12, 856);
            this.btnPitchKlarf.Name = "btnPitchKlarf";
            this.btnPitchKlarf.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnPitchKlarf.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPitchKlarf.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPitchKlarf.Size = new System.Drawing.Size(170, 28);
            this.btnPitchKlarf.TabIndex = 28;
            this.btnPitchKlarf.Text = "KLARF 값으로";
            this.btnPitchKlarf.UseVisualStyleBackColor = true;
            this.btnPitchKlarf.Visible = false;
            this.btnPitchKlarf.Click += new System.EventHandler(this.btnPitchKlarf_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Enabled = false;
            this.btnGenerate.Image = ((System.Drawing.Image)(resources.GetObject("btnGenerate.Image")));
            this.btnGenerate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGenerate.Location = new System.Drawing.Point(12, 456);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnGenerate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGenerate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGenerate.Size = new System.Drawing.Size(170, 28);
            this.btnGenerate.TabIndex = 7;
            this.btnGenerate.Text = "다시 그리기";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // chkDieNumber
            // 
            this.chkDieNumber.Checked = true;
            this.chkDieNumber.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDieNumber.Location = new System.Drawing.Point(192, 456);
            this.chkDieNumber.Name = "chkDieNumber";
            this.chkDieNumber.Size = new System.Drawing.Size(100, 28);
            this.chkDieNumber.TabIndex = 19;
            this.chkDieNumber.Text = "Die 번호";
            this.chkDieNumber.UseVisualStyleBackColor = true;
            this.chkDieNumber.CheckedChanged += new System.EventHandler(this.chkDieNumber_CheckedChanged);
            // 
            // lblEditMode
            // 
            this.lblEditMode.Location = new System.Drawing.Point(12, 570);
            this.lblEditMode.Name = "lblEditMode";
            this.lblEditMode.Size = new System.Drawing.Size(280, 22);
            this.lblEditMode.TabIndex = 13;
            this.lblEditMode.Text = "Die 편집";
            this.lblEditMode.Visible = false;
            // 
            // rdoView
            // 
            this.rdoView.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoView.Checked = true;
            this.rdoView.Image = ((System.Drawing.Image)(resources.GetObject("rdoView.Image")));
            this.rdoView.Location = new System.Drawing.Point(12, 594);
            this.rdoView.Name = "rdoView";
            this.rdoView.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rdoView.Size = new System.Drawing.Size(88, 28);
            this.rdoView.TabIndex = 14;
            this.rdoView.TabStop = true;
            this.rdoView.Text = "보기";
            this.rdoView.UseVisualStyleBackColor = true;
            this.rdoView.Visible = false;
            this.rdoView.CheckedChanged += new System.EventHandler(this.EditModeChanged);
            // 
            // rdoRemove
            // 
            this.rdoRemove.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoRemove.Image = ((System.Drawing.Image)(resources.GetObject("rdoRemove.Image")));
            this.rdoRemove.Location = new System.Drawing.Point(108, 594);
            this.rdoRemove.Name = "rdoRemove";
            this.rdoRemove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rdoRemove.Size = new System.Drawing.Size(88, 28);
            this.rdoRemove.TabIndex = 15;
            this.rdoRemove.Text = "제거";
            this.rdoRemove.UseVisualStyleBackColor = true;
            this.rdoRemove.Visible = false;
            this.rdoRemove.CheckedChanged += new System.EventHandler(this.EditModeChanged);
            // 
            // rdoRestore
            // 
            this.rdoRestore.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoRestore.Image = ((System.Drawing.Image)(resources.GetObject("rdoRestore.Image")));
            this.rdoRestore.Location = new System.Drawing.Point(204, 594);
            this.rdoRestore.Name = "rdoRestore";
            this.rdoRestore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoRestore.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rdoRestore.Size = new System.Drawing.Size(88, 28);
            this.rdoRestore.TabIndex = 16;
            this.rdoRestore.Text = "복원";
            this.rdoRestore.UseVisualStyleBackColor = true;
            this.rdoRestore.Visible = false;
            this.rdoRestore.CheckedChanged += new System.EventHandler(this.EditModeChanged);
            // 
            // btnRemoveNotInspected
            // 
            this.btnRemoveNotInspected.Enabled = false;
            this.btnRemoveNotInspected.Image = ((System.Drawing.Image)(resources.GetObject("btnRemoveNotInspected.Image")));
            this.btnRemoveNotInspected.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRemoveNotInspected.Location = new System.Drawing.Point(12, 628);
            this.btnRemoveNotInspected.Name = "btnRemoveNotInspected";
            this.btnRemoveNotInspected.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnRemoveNotInspected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRemoveNotInspected.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRemoveNotInspected.Size = new System.Drawing.Size(280, 28);
            this.btnRemoveNotInspected.TabIndex = 17;
            this.btnRemoveNotInspected.Text = "검사 Die 외 제거";
            this.btnRemoveNotInspected.UseVisualStyleBackColor = true;
            this.btnRemoveNotInspected.Visible = false;
            this.btnRemoveNotInspected.Click += new System.EventHandler(this.btnRemoveNotInspected_Click);
            // 
            // btnRemoveOutside
            // 
            this.btnRemoveOutside.Enabled = false;
            this.btnRemoveOutside.Image = ((System.Drawing.Image)(resources.GetObject("btnRemoveOutside.Image")));
            this.btnRemoveOutside.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRemoveOutside.Location = new System.Drawing.Point(12, 662);
            this.btnRemoveOutside.Name = "btnRemoveOutside";
            this.btnRemoveOutside.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnRemoveOutside.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRemoveOutside.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRemoveOutside.Size = new System.Drawing.Size(280, 28);
            this.btnRemoveOutside.TabIndex = 18;
            this.btnRemoveOutside.Text = "Wafer 밖 제거";
            this.btnRemoveOutside.UseVisualStyleBackColor = true;
            this.btnRemoveOutside.Visible = false;
            this.btnRemoveOutside.Click += new System.EventHandler(this.btnRemoveOutside_Click);
            // 
            // txtInfo
            // 
            this.txtInfo.BackColor = System.Drawing.SystemColors.Window;
            this.txtInfo.Location = new System.Drawing.Point(12, 698);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            this.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInfo.Size = new System.Drawing.Size(280, 150);
            this.txtInfo.TabIndex = 8;
            this.txtInfo.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(12, 494);
            this.btnSave.Name = "btnSave";
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.Size = new System.Drawing.Size(280, 28);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Map 저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnOpenMap
            // 
            this.btnOpenMap.Enabled = false;
            this.btnOpenMap.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenMap.Image")));
            this.btnOpenMap.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenMap.Location = new System.Drawing.Point(12, 528);
            this.btnOpenMap.Name = "btnOpenMap";
            this.btnOpenMap.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnOpenMap.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenMap.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOpenMap.Size = new System.Drawing.Size(280, 28);
            this.btnOpenMap.TabIndex = 10;
            this.btnOpenMap.Text = "저장 Map 열기";
            this.btnOpenMap.UseVisualStyleBackColor = true;
            this.btnOpenMap.Visible = false;
            this.btnOpenMap.Click += new System.EventHandler(this.btnOpenMap_Click);
            // 
            // pnlStatus
            // 
            this.pnlStatus.Controls.Add(this.lblStatus);
            this.pnlStatus.Controls.Add(this.busyBar);
            this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlStatus.Location = new System.Drawing.Point(0, 952);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(1634, 32);
            this.pnlStatus.TabIndex = 90;
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Location = new System.Drawing.Point(0, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.lblStatus.Size = new System.Drawing.Size(1214, 32);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // busyBar
            // 
            this.busyBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.busyBar.Location = new System.Drawing.Point(1214, 0);
            this.busyBar.Name = "busyBar";
            this.busyBar.Padding = new System.Windows.Forms.Padding(0, 3, 6, 3);
            this.busyBar.Size = new System.Drawing.Size(420, 32);
            this.busyBar.TabIndex = 1;
            // 
            // lblCoordinates
            // 
            this.lblCoordinates.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblCoordinates.Location = new System.Drawing.Point(0, 926);
            this.lblCoordinates.Name = "lblCoordinates";
            this.lblCoordinates.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.lblCoordinates.Size = new System.Drawing.Size(1634, 26);
            this.lblCoordinates.TabIndex = 3;
            this.lblCoordinates.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDieCount
            // 
            this.lblDieCount.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblDieCount.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDieCount.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lblDieCount.Location = new System.Drawing.Point(310, 52);
            this.lblDieCount.Name = "lblDieCount";
            this.lblDieCount.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.lblDieCount.Size = new System.Drawing.Size(1324, 36);
            this.lblDieCount.TabIndex = 4;
            this.lblDieCount.Text = "Net Die: - / 생성 Die: -";
            this.lblDieCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_wMap
            // 
            this.m_wMap.AngleOffSet = 0;
            this.m_wMap.AutoFocus = false;
            this.m_wMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(234)))), ((int)(((byte)(216)))));
            this.m_wMap.CenterMark = true;
            this.m_wMap.CurrentFailNumber = 0;
            this.m_wMap.Cursor = System.Windows.Forms.Cursors.Cross;
            this.m_wMap.DataSource = null;
            this.m_wMap.DieBorderColor = System.Drawing.Color.White;
            this.m_wMap.DieFocusingType = DACrux.Map.FocusType.Arraw;
            this.m_wMap.DieMaxX = -65536;
            this.m_wMap.DieMaxY = -65536;
            this.m_wMap.DieMinX = 65536;
            this.m_wMap.DieMinY = 65536;
            this.m_wMap.DieSizeX = 1D;
            this.m_wMap.DieSizeY = 1D;
            this.m_wMap.DisplayDieValue = DACrux.Base.DieDisplayValue.Bin;
            this.m_wMap.DisplayValue = "BIN";
            this.m_wMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_wMap.DrawFirstDie = false;
            this.m_wMap.DrawMarkDie = false;
            this.m_wMap.DrawOriginDie = false;
            this.m_wMap.DrawSkipDie = false;
            this.m_wMap.EdgeColor = System.Drawing.Color.LightGray;
            this.m_wMap.EdgeSize = 0D;
            this.m_wMap.EditMenuVisible = false;
            this.m_wMap.EditMethod = DACrux.Map.EditMode.ADD;
            this.m_wMap.FirstDieBorderColor = System.Drawing.Color.SkyBlue;
            this.m_wMap.FirstDieX = 0;
            this.m_wMap.FirstDieY = 0;
            this.m_wMap.ForeColor = System.Drawing.Color.Red;
            this.m_wMap.FromGradationDieColor = System.Drawing.Color.Lime;
            this.m_wMap.GradationInterval = 5;
            this.m_wMap.GradationMaxValue = double.NaN;
            this.m_wMap.GradationMinValue = double.NaN;
            this.m_wMap.ImeMode = System.Windows.Forms.ImeMode.On;
            this.m_wMap.IsVIEdit = false;
            this.m_wMap.Location = new System.Drawing.Point(310, 88);
            this.m_wMap.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.m_wMap.MarkDieColor = System.Drawing.Color.LightSkyBlue;
            this.m_wMap.Name = "m_wMap";
            this.m_wMap.NotchAngle = 0;
            this.m_wMap.NotchType = DACrux.Base.Notch.Notch;
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
            this.m_wMap.Size = new System.Drawing.Size(1324, 838);
            this.m_wMap.SkipDieColor = System.Drawing.Color.Yellow;
            this.m_wMap.TabIndex = 0;
            this.m_wMap.ToGradationDieColor = System.Drawing.Color.Red;
            this.m_wMap.TransParent = 255;
            this.m_wMap.ViewAngle = 0;
            this.m_wMap.VIMember = "VIFAIL";
            this.m_wMap.VisibleDieBorder = true;
            this.m_wMap.VisibleDieValue = false;
            this.m_wMap.VisibleFocusDie = false;
            this.m_wMap.VisibleInfomation = false;
            this.m_wMap.VisibleOffDie = false;
            this.m_wMap.VisibleShotAlignPoint = false;
            this.m_wMap.VisibleSignDies = false;
            this.m_wMap.VisibleStringBin = false;
            this.m_wMap.VisibleVIFail = false;
            this.m_wMap.VisibleXY = false;
            this.m_wMap.WaferBorderColor = System.Drawing.Color.Silver;
            this.m_wMap.WaferColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(234)))), ((int)(((byte)(216)))));
            this.m_wMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
            this.m_wMap.WaferID = "";
            this.m_wMap.WaferMargin = 0.95D;
            this.m_wMap.WaferSize = 300000D;
            this.m_wMap.XYDirect = DACrux.Base.XYDirection.LeftBottom;
            this.m_wMap.OnChangeDieProperty += new DACrux.Map.ChangeDieProperty(this.m_wMap_OnChangeDieProperty);
            this.m_wMap.OnChangeCurrentDie += new DACrux.Map.ChangeCurrentDie(this.m_wMap_OnChangeCurrentDie);
            this.m_wMap.OnChangePosition += new DACrux.Map.ChangePosition(this.m_wMap_OnChangePosition);
            this.m_wMap.MouseLeave += new System.EventHandler(this.m_wMap_MouseLeave);
            this.m_wMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.m_wMap_MouseMove);
            this.m_wMap.MouseUp += new System.Windows.Forms.MouseEventHandler(this.m_wMap_MouseUp);
            // 
            // frmStandardMapSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1634, 984);
            this.Controls.Add(this.m_wMap);
            this.Controls.Add(this.lblDieCount);
            this.Controls.Add(this.settings);
            this.Controls.Add(this.fileBar);
            this.Controls.Add(this.lblCoordinates);
            this.Controls.Add(this.pnlStatus);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.Margin = new System.Windows.Forms.Padding(9);
            this.MinimumSize = new System.Drawing.Size(860, 620);
            this.Name = "frmStandardMapSetup";
            this.Text = "파일 Map Setup";
            this.fileBar.ResumeLayout(false);
            this.fileBar.PerformLayout();
            this.settings.ResumeLayout(false);
            this.settings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEdge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOriginX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOriginY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPitchX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPitchY)).EndInit();
            this.pnlStatus.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel fileBar;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnParse;
        private System.Windows.Forms.Panel settings;
        private System.Windows.Forms.Label lblMapID;
        private System.Windows.Forms.TextBox txtMapID;
        private System.Windows.Forms.Label lblWafer;
        private System.Windows.Forms.ComboBox cmbWaferSize;
        private System.Windows.Forms.Label lblOrigin;
        private System.Windows.Forms.Label lblOriginX;
        private System.Windows.Forms.Label lblOriginY;
        private System.Windows.Forms.NumericUpDown numOriginX;
        private System.Windows.Forms.NumericUpDown numOriginY;
        private System.Windows.Forms.Label lblPitch;
        private System.Windows.Forms.Label lblPitchX;
        private System.Windows.Forms.Label lblPitchY;
        private System.Windows.Forms.NumericUpDown numPitchX;
        private System.Windows.Forms.NumericUpDown numPitchY;
        private System.Windows.Forms.Button btnPitchKlarf;
        private System.Windows.Forms.TextBox txtInfo;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnOpenMap;
        private DACrux.TEST.ENGUI.StandardMapEditMap m_wMap;
        private System.Windows.Forms.Label lblKlarfWafer;
        private System.Windows.Forms.ComboBox cmbKlarfWafer;
        private System.Windows.Forms.Label lblEdge;
        private System.Windows.Forms.NumericUpDown numEdge;
        private System.Windows.Forms.Label lblDirection;
        private System.Windows.Forms.ComboBox cmbXYDirection;
        private System.Windows.Forms.Label lblFlatZone;
        private System.Windows.Forms.ComboBox cmbFlatZone;
        private System.Windows.Forms.Label lblMarkType;
        private System.Windows.Forms.ComboBox cmbMarkType;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.CheckBox chkDieNumber;
        private System.Windows.Forms.Label lblEditMode;
        private System.Windows.Forms.RadioButton rdoView;
        private System.Windows.Forms.RadioButton rdoRemove;
        private System.Windows.Forms.RadioButton rdoRestore;
        private System.Windows.Forms.Button btnRemoveNotInspected;
        private System.Windows.Forms.Button btnRemoveOutside;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Label lblStatus;
        private DACrux.SEMDMS.Control.ProgressStatusBar busyBar;
        private System.Windows.Forms.Label lblCoordinates;
        private System.Windows.Forms.Label lblDieCount;
    }
}
