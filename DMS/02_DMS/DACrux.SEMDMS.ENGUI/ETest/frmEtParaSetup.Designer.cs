namespace DACrux.SEMDMS.ENGUI
{
    // E-Test Para Setup 화면의 컨트롤과 배치를 정의한다. 파일 열기 / 등록 / 저장 / 대표 Chain / GDS 연결 동작은 본문 파일(frmEtParaSetup.cs)에서 처리한다. (2026-10-14)
    partial class frmEtParaSetup
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>화면 종료 시 디자이너 구성 요소를 해제한다.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// 위: Factory / Device / 공정 / [조회] [결과 파일 열기] [Spec 표로 고르기] [등록] [저장] [선택 삭제] + 안내 문구.
        /// 가운데 탭: [파일 헤더] 결과 파일 헤더 -> 등록 후보 / [등록 Parameter] 등록된 Parameter (입력 칸 = 사용 / 비고) /
        ///           [대표 Chain] 단자 | 대표 Chain | GDS Setup Chain + 아래 선택 대표의 Parameter / Spec. 아래: 상태 표시줄.
        /// </summary>
        private void InitializeComponent()
        {
            this.topBar = new System.Windows.Forms.Panel();
            this.lblFactory = new System.Windows.Forms.Label();
            this.txtFactory = new System.Windows.Forms.TextBox();
            this.lblDevice = new System.Windows.Forms.Label();
            this.cboDevice = new System.Windows.Forms.ComboBox();
            this.lblTestProgram = new System.Windows.Forms.Label();
            this.cboTestProgram = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.btnSpecPick = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lblGuide = new System.Windows.Forms.Label();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabFile = new System.Windows.Forms.TabPage();
            this.tabPara = new System.Windows.Forms.TabPage();
            this.tabRep = new System.Windows.Forms.TabPage();
            this.fileBar = new System.Windows.Forms.Panel();
            this.lblFileInfo = new System.Windows.Forms.Label();
            this.lblMeasFilter = new System.Windows.Forms.Label();
            this.cboMeasFilter = new System.Windows.Forms.ComboBox();
            this.lblTypeFilter = new System.Windows.Forms.Label();
            this.cboTypeFilter = new System.Windows.Forms.ComboBox();
            this.lblFind = new System.Windows.Forms.Label();
            this.txtFind = new System.Windows.Forms.TextBox();
            this.chkNewOnly = new System.Windows.Forms.CheckBox();
            this.btnCheckVisible = new System.Windows.Forms.Button();
            this.btnUncheckAll = new System.Windows.Forms.Button();
            this.dgvHeader = new System.Windows.Forms.DataGridView();
            this.colHCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colHStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHHeader = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHMeas = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHCond = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHT1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHLeg1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHT2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHLeg2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHTestNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHRep = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvPara = new System.Windows.Forms.DataGridView();
            this.colPHeader = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPMeas = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPCond = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPT1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPT2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPRep = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPGds = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPUse = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colPFiles = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPRegFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPRemark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPUpdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.repBar = new System.Windows.Forms.Panel();
            this.lblGdsDevice = new System.Windows.Forms.Label();
            this.cboGdsDevice = new System.Windows.Forms.ComboBox();
            this.lblRepName = new System.Windows.Forms.Label();
            this.cboRepName = new System.Windows.Forms.ComboBox();
            this.lblRepPreview = new System.Windows.Forms.Label();
            this.btnRepGroup = new System.Windows.Forms.Button();
            this.btnRepRemove = new System.Windows.Forms.Button();
            this.btnRepRename = new System.Windows.Forms.Button();
            this.btnRepDelete = new System.Windows.Forms.Button();
            this.btnLinkName = new System.Windows.Forms.Button();
            this.btnLinkManual = new System.Windows.Forms.Button();
            this.btnLinkClear = new System.Windows.Forms.Button();
            this.chkUnassignedOnly = new System.Windows.Forms.CheckBox();
            this.lblRepFind = new System.Windows.Forms.Label();
            this.txtRepFind = new System.Windows.Forms.TextBox();
            this.lblRepSummary = new System.Windows.Forms.Label();
            this.splitRepMain = new System.Windows.Forms.SplitContainer();
            this.splitRepTop = new System.Windows.Forms.SplitContainer();
            this.splitRepRight = new System.Windows.Forms.SplitContainer();
            this.dgvTerminal = new System.Windows.Forms.DataGridView();
            this.colTName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTLeg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTPrograms = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTPara = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTRep = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvRep = new System.Windows.Forms.DataGridView();
            this.colRName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRTerminals = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRGds = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRCandidates = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvGds = new System.Windows.Forms.DataGridView();
            this.colGName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGRep = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvRepSpec = new System.Windows.Forms.DataGridView();
            this.colSProgram = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSDevice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSHeader = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSLower = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSUpper = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSEnable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblStatus = new System.Windows.Forms.Label();
            this.topBar.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabFile.SuspendLayout();
            this.fileBar.SuspendLayout();
            this.tabPara.SuspendLayout();
            this.tabRep.SuspendLayout();
            this.repBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitRepMain)).BeginInit();
            this.splitRepMain.Panel1.SuspendLayout();
            this.splitRepMain.Panel2.SuspendLayout();
            this.splitRepMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitRepTop)).BeginInit();
            this.splitRepTop.Panel1.SuspendLayout();
            this.splitRepTop.Panel2.SuspendLayout();
            this.splitRepTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitRepRight)).BeginInit();
            this.splitRepRight.Panel1.SuspendLayout();
            this.splitRepRight.Panel2.SuspendLayout();
            this.splitRepRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPara)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTerminal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRepSpec)).BeginInit();
            this.SuspendLayout();
            //
            // topBar
            //
            this.topBar.Controls.Add(this.lblFactory);
            this.topBar.Controls.Add(this.txtFactory);
            this.topBar.Controls.Add(this.lblDevice);
            this.topBar.Controls.Add(this.cboDevice);
            this.topBar.Controls.Add(this.lblTestProgram);
            this.topBar.Controls.Add(this.cboTestProgram);
            this.topBar.Controls.Add(this.btnSearch);
            this.topBar.Controls.Add(this.btnOpenFile);
            this.topBar.Controls.Add(this.btnSpecPick);
            this.topBar.Controls.Add(this.btnRegister);
            this.topBar.Controls.Add(this.btnSave);
            this.topBar.Controls.Add(this.btnDelete);
            this.topBar.Controls.Add(this.lblGuide);
            this.topBar.Name = "topBar";
            this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar.Location = new System.Drawing.Point(0, 0);
            this.topBar.Size = new System.Drawing.Size(1300, 62);
            this.topBar.TabIndex = 0;
            //
            // lblFactory
            //
            this.lblFactory.Name = "lblFactory";
            this.lblFactory.AutoSize = true;
            this.lblFactory.Location = new System.Drawing.Point(10, 13);
            this.lblFactory.Size = new System.Drawing.Size(46, 12);
            this.lblFactory.TabIndex = 0;
            this.lblFactory.Text = "Factory";
            //
            // txtFactory
            //
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.Location = new System.Drawing.Point(62, 9);
            this.txtFactory.Size = new System.Drawing.Size(90, 21);
            this.txtFactory.TabIndex = 1;
            this.txtFactory.Leave += new System.EventHandler(this.txtFactory_Leave);
            //
            // lblDevice
            //
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.AutoSize = true;
            this.lblDevice.Location = new System.Drawing.Point(164, 13);
            this.lblDevice.Size = new System.Drawing.Size(42, 12);
            this.lblDevice.TabIndex = 2;
            this.lblDevice.Text = "Device";
            //
            // cboDevice
            //
            this.cboDevice.Name = "cboDevice";
            this.cboDevice.FormattingEnabled = true;
            this.cboDevice.Location = new System.Drawing.Point(212, 9);
            this.cboDevice.Size = new System.Drawing.Size(150, 20);
            this.cboDevice.TabIndex = 3;
            this.cboDevice.SelectedIndexChanged += new System.EventHandler(this.cboDevice_SelectedIndexChanged);
            //
            // lblTestProgram
            //
            this.lblTestProgram.Name = "lblTestProgram";
            this.lblTestProgram.AutoSize = true;
            this.lblTestProgram.Location = new System.Drawing.Point(374, 13);
            this.lblTestProgram.Size = new System.Drawing.Size(29, 12);
            this.lblTestProgram.TabIndex = 4;
            this.lblTestProgram.Text = "공정";
            //
            // cboTestProgram
            //
            this.cboTestProgram.Name = "cboTestProgram";
            this.cboTestProgram.FormattingEnabled = true;
            this.cboTestProgram.Location = new System.Drawing.Point(409, 9);
            this.cboTestProgram.Size = new System.Drawing.Size(90, 20);
            this.cboTestProgram.TabIndex = 5;
            this.cboTestProgram.SelectedIndexChanged += new System.EventHandler(this.cboTestProgram_SelectedIndexChanged);
            //
            // btnSearch
            //
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Location = new System.Drawing.Point(508, 8);
            this.btnSearch.Size = new System.Drawing.Size(70, 23);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            //
            // btnOpenFile
            //
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Location = new System.Drawing.Point(596, 8);
            this.btnOpenFile.Size = new System.Drawing.Size(120, 23);
            this.btnOpenFile.TabIndex = 7;
            this.btnOpenFile.Text = "결과 파일 열기...";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            //
            // btnSpecPick
            //
            this.btnSpecPick.Name = "btnSpecPick";
            this.btnSpecPick.Location = new System.Drawing.Point(722, 8);
            this.btnSpecPick.Size = new System.Drawing.Size(130, 23);
            this.btnSpecPick.TabIndex = 8;
            this.btnSpecPick.Text = "Spec 표로 고르기";
            this.btnSpecPick.UseVisualStyleBackColor = true;
            this.btnSpecPick.Click += new System.EventHandler(this.btnSpecPick_Click);
            //
            // btnRegister
            //
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Location = new System.Drawing.Point(870, 8);
            this.btnRegister.Size = new System.Drawing.Size(80, 23);
            this.btnRegister.TabIndex = 9;
            this.btnRegister.Text = "등록";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            //
            // btnSave
            //
            this.btnSave.Name = "btnSave";
            this.btnSave.Location = new System.Drawing.Point(956, 8);
            this.btnSave.Size = new System.Drawing.Size(70, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // btnDelete
            //
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Location = new System.Drawing.Point(1032, 8);
            this.btnDelete.Size = new System.Drawing.Size(100, 23);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "선택 삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            //
            // lblGuide
            //
            this.lblGuide.Name = "lblGuide";
            this.lblGuide.AutoSize = true;
            this.lblGuide.ForeColor = System.Drawing.Color.DimGray;
            this.lblGuide.Location = new System.Drawing.Point(10, 40);
            this.lblGuide.Size = new System.Drawing.Size(900, 12);
            this.lblGuide.TabIndex = 12;
            this.lblGuide.Text = "Parameter = 결과 파일 헤더 전체 (Device + 공정 단위). 저장 이름은 DB / 소스에서 쓰도록 바꾼 이름(대문자, '.' -> 'P', 그 밖 특수문자 -> '_')이고 헤더 원문도 함께 보관합니다.";
            //
            // tabMain
            //
            this.tabMain.Controls.Add(this.tabFile);
            this.tabMain.Controls.Add(this.tabPara);
            this.tabMain.Controls.Add(this.tabRep);
            this.tabMain.Name = "tabMain";
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 62);
            this.tabMain.Size = new System.Drawing.Size(1300, 636);
            this.tabMain.TabIndex = 1;
            this.tabMain.SelectedIndex = 0;
            //
            // tabFile
            //
            this.tabFile.Controls.Add(this.dgvHeader);
            this.tabFile.Controls.Add(this.fileBar);
            this.tabFile.Name = "tabFile";
            this.tabFile.Location = new System.Drawing.Point(4, 22);
            this.tabFile.Size = new System.Drawing.Size(1292, 610);
            this.tabFile.TabIndex = 0;
            this.tabFile.Text = "파일 헤더";
            this.tabFile.UseVisualStyleBackColor = true;
            //
            // tabPara
            //
            this.tabPara.Controls.Add(this.dgvPara);
            this.tabPara.Name = "tabPara";
            this.tabPara.Location = new System.Drawing.Point(4, 22);
            this.tabPara.Size = new System.Drawing.Size(1292, 610);
            this.tabPara.TabIndex = 1;
            this.tabPara.Text = "등록 Parameter";
            this.tabPara.UseVisualStyleBackColor = true;
            //
            // tabRep
            //
            this.tabRep.Controls.Add(this.splitRepMain);
            this.tabRep.Controls.Add(this.lblRepSummary);
            this.tabRep.Controls.Add(this.repBar);
            this.tabRep.Name = "tabRep";
            this.tabRep.Location = new System.Drawing.Point(4, 22);
            this.tabRep.Size = new System.Drawing.Size(1292, 610);
            this.tabRep.TabIndex = 2;
            this.tabRep.Text = "대표 Chain";
            this.tabRep.UseVisualStyleBackColor = true;
            //
            // fileBar
            //
            this.fileBar.Controls.Add(this.lblFileInfo);
            this.fileBar.Controls.Add(this.lblMeasFilter);
            this.fileBar.Controls.Add(this.cboMeasFilter);
            this.fileBar.Controls.Add(this.lblTypeFilter);
            this.fileBar.Controls.Add(this.cboTypeFilter);
            this.fileBar.Controls.Add(this.lblFind);
            this.fileBar.Controls.Add(this.txtFind);
            this.fileBar.Controls.Add(this.chkNewOnly);
            this.fileBar.Controls.Add(this.btnCheckVisible);
            this.fileBar.Controls.Add(this.btnUncheckAll);
            this.fileBar.Name = "fileBar";
            this.fileBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.fileBar.Location = new System.Drawing.Point(0, 0);
            this.fileBar.Size = new System.Drawing.Size(1292, 60);
            this.fileBar.TabIndex = 0;
            //
            // lblFileInfo
            //
            this.lblFileInfo.Name = "lblFileInfo";
            this.lblFileInfo.AutoSize = true;
            this.lblFileInfo.Location = new System.Drawing.Point(8, 9);
            this.lblFileInfo.Size = new System.Drawing.Size(300, 12);
            this.lblFileInfo.TabIndex = 0;
            this.lblFileInfo.Text = "[결과 파일 열기] 로 E-Test 결과 파일(xlsx)을 여세요. 헤더가 등록 후보로 나옵니다.";
            //
            // lblMeasFilter
            //
            this.lblMeasFilter.Name = "lblMeasFilter";
            this.lblMeasFilter.AutoSize = true;
            this.lblMeasFilter.Location = new System.Drawing.Point(8, 37);
            this.lblMeasFilter.Size = new System.Drawing.Size(29, 12);
            this.lblMeasFilter.TabIndex = 1;
            this.lblMeasFilter.Text = "측정";
            //
            // cboMeasFilter
            //
            this.cboMeasFilter.Name = "cboMeasFilter";
            this.cboMeasFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMeasFilter.FormattingEnabled = true;
            this.cboMeasFilter.Location = new System.Drawing.Point(44, 33);
            this.cboMeasFilter.Size = new System.Drawing.Size(130, 20);
            this.cboMeasFilter.TabIndex = 2;
            this.cboMeasFilter.SelectedIndexChanged += new System.EventHandler(this.headerFilter_Changed);
            //
            // lblTypeFilter
            //
            this.lblTypeFilter.Name = "lblTypeFilter";
            this.lblTypeFilter.AutoSize = true;
            this.lblTypeFilter.Location = new System.Drawing.Point(186, 37);
            this.lblTypeFilter.Size = new System.Drawing.Size(65, 12);
            this.lblTypeFilter.TabIndex = 3;
            this.lblTypeFilter.Text = "Chain 종류";
            //
            // cboTypeFilter
            //
            this.cboTypeFilter.Name = "cboTypeFilter";
            this.cboTypeFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTypeFilter.FormattingEnabled = true;
            this.cboTypeFilter.Location = new System.Drawing.Point(257, 33);
            this.cboTypeFilter.Size = new System.Drawing.Size(110, 20);
            this.cboTypeFilter.TabIndex = 4;
            this.cboTypeFilter.SelectedIndexChanged += new System.EventHandler(this.headerFilter_Changed);
            //
            // lblFind
            //
            this.lblFind.Name = "lblFind";
            this.lblFind.AutoSize = true;
            this.lblFind.Location = new System.Drawing.Point(379, 37);
            this.lblFind.Size = new System.Drawing.Size(29, 12);
            this.lblFind.TabIndex = 5;
            this.lblFind.Text = "찾기";
            //
            // txtFind
            //
            this.txtFind.Name = "txtFind";
            this.txtFind.Location = new System.Drawing.Point(414, 33);
            this.txtFind.Size = new System.Drawing.Size(200, 21);
            this.txtFind.TabIndex = 6;
            this.txtFind.TextChanged += new System.EventHandler(this.headerFilter_Changed);
            //
            // chkNewOnly
            //
            this.chkNewOnly.Name = "chkNewOnly";
            this.chkNewOnly.AutoSize = true;
            this.chkNewOnly.Location = new System.Drawing.Point(626, 35);
            this.chkNewOnly.Size = new System.Drawing.Size(60, 16);
            this.chkNewOnly.TabIndex = 7;
            this.chkNewOnly.Text = "신규만";
            this.chkNewOnly.UseVisualStyleBackColor = true;
            this.chkNewOnly.CheckedChanged += new System.EventHandler(this.headerFilter_Changed);
            //
            // btnCheckVisible
            //
            this.btnCheckVisible.Name = "btnCheckVisible";
            this.btnCheckVisible.Location = new System.Drawing.Point(700, 31);
            this.btnCheckVisible.Size = new System.Drawing.Size(120, 23);
            this.btnCheckVisible.TabIndex = 8;
            this.btnCheckVisible.Text = "보이는 신규 선택";
            this.btnCheckVisible.UseVisualStyleBackColor = true;
            this.btnCheckVisible.Click += new System.EventHandler(this.btnCheckVisible_Click);
            //
            // btnUncheckAll
            //
            this.btnUncheckAll.Name = "btnUncheckAll";
            this.btnUncheckAll.Location = new System.Drawing.Point(826, 31);
            this.btnUncheckAll.Size = new System.Drawing.Size(90, 23);
            this.btnUncheckAll.TabIndex = 9;
            this.btnUncheckAll.Text = "선택 해제";
            this.btnUncheckAll.UseVisualStyleBackColor = true;
            this.btnUncheckAll.Click += new System.EventHandler(this.btnUncheckAll_Click);
            //
            // dgvHeader
            //
            this.dgvHeader.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colHCheck,
            this.colHStatus,
            this.colHHeader,
            this.colHName,
            this.colHMeas,
            this.colHCond,
            this.colHT1,
            this.colHLeg1,
            this.colHT2,
            this.colHLeg2,
            this.colHUnit,
            this.colHTestNo,
            this.colHOrder,
            this.colHRep,
            this.colHMessage});
            this.dgvHeader.Name = "dgvHeader";
            this.dgvHeader.AllowUserToAddRows = false;
            this.dgvHeader.AllowUserToDeleteRows = false;
            this.dgvHeader.AllowUserToResizeRows = false;
            this.dgvHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHeader.Location = new System.Drawing.Point(0, 60);
            this.dgvHeader.RowHeadersVisible = false;
            this.dgvHeader.RowTemplate.Height = 23;
            this.dgvHeader.Size = new System.Drawing.Size(1292, 550);
            this.dgvHeader.TabIndex = 1;
            this.dgvHeader.CurrentCellDirtyStateChanged += new System.EventHandler(this.grid_CurrentCellDirtyStateChanged);
            //
            // colHCheck
            //
            this.colHCheck.HeaderText = "선택";
            this.colHCheck.Name = "colHCheck";
            this.colHCheck.Width = 40;
            //
            // colHStatus
            //
            this.colHStatus.HeaderText = "상태";
            this.colHStatus.Name = "colHStatus";
            this.colHStatus.ReadOnly = true;
            this.colHStatus.Width = 70;
            //
            // colHHeader
            //
            this.colHHeader.HeaderText = "Parameter (헤더 원문)";
            this.colHHeader.Name = "colHHeader";
            this.colHHeader.ReadOnly = true;
            this.colHHeader.Width = 300;
            //
            // colHName
            //
            this.colHName.HeaderText = "저장 이름";
            this.colHName.Name = "colHName";
            this.colHName.ReadOnly = true;
            this.colHName.Width = 300;
            //
            // colHMeas
            //
            this.colHMeas.HeaderText = "측정";
            this.colHMeas.Name = "colHMeas";
            this.colHMeas.ReadOnly = true;
            this.colHMeas.Width = 90;
            //
            // colHCond
            //
            this.colHCond.HeaderText = "조건";
            this.colHCond.Name = "colHCond";
            this.colHCond.ReadOnly = true;
            this.colHCond.Width = 100;
            //
            // colHT1
            //
            this.colHT1.HeaderText = "단자 1";
            this.colHT1.Name = "colHT1";
            this.colHT1.ReadOnly = true;
            this.colHT1.Width = 150;
            //
            // colHLeg1
            //
            this.colHLeg1.HeaderText = "Leg";
            this.colHLeg1.Name = "colHLeg1";
            this.colHLeg1.ReadOnly = true;
            this.colHLeg1.Width = 40;
            //
            // colHT2
            //
            this.colHT2.HeaderText = "단자 2";
            this.colHT2.Name = "colHT2";
            this.colHT2.ReadOnly = true;
            this.colHT2.Width = 150;
            //
            // colHLeg2
            //
            this.colHLeg2.HeaderText = "Leg";
            this.colHLeg2.Name = "colHLeg2";
            this.colHLeg2.ReadOnly = true;
            this.colHLeg2.Width = 40;
            //
            // colHUnit
            //
            this.colHUnit.HeaderText = "단위";
            this.colHUnit.Name = "colHUnit";
            this.colHUnit.ReadOnly = true;
            this.colHUnit.Width = 50;
            //
            // colHTestNo
            //
            this.colHTestNo.HeaderText = "Test No";
            this.colHTestNo.Name = "colHTestNo";
            this.colHTestNo.ReadOnly = true;
            this.colHTestNo.Width = 60;
            //
            // colHOrder
            //
            this.colHOrder.HeaderText = "열";
            this.colHOrder.Name = "colHOrder";
            this.colHOrder.ReadOnly = true;
            this.colHOrder.Width = 50;
            //
            // colHRep
            //
            this.colHRep.HeaderText = "대표 Chain";
            this.colHRep.Name = "colHRep";
            this.colHRep.ReadOnly = true;
            this.colHRep.Width = 180;
            //
            // colHMessage
            //
            this.colHMessage.HeaderText = "메시지";
            this.colHMessage.Name = "colHMessage";
            this.colHMessage.ReadOnly = true;
            this.colHMessage.Width = 300;
            //
            // dgvPara
            //
            this.dgvPara.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPHeader,
            this.colPName,
            this.colPMeas,
            this.colPCond,
            this.colPT1,
            this.colPT2,
            this.colPRep,
            this.colPGds,
            this.colPUnit,
            this.colPSpec,
            this.colPUse,
            this.colPFiles,
            this.colPSource,
            this.colPRegFile,
            this.colPRemark,
            this.colPUpdate});
            this.dgvPara.Name = "dgvPara";
            this.dgvPara.AllowUserToAddRows = false;
            this.dgvPara.AllowUserToDeleteRows = false;
            this.dgvPara.AllowUserToResizeRows = false;
            this.dgvPara.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPara.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystrokeOrF2;
            this.dgvPara.Location = new System.Drawing.Point(0, 0);
            this.dgvPara.RowHeadersVisible = false;
            this.dgvPara.RowTemplate.Height = 23;
            this.dgvPara.Size = new System.Drawing.Size(1292, 610);
            this.dgvPara.TabIndex = 0;
            this.dgvPara.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPara_CellValueChanged);
            this.dgvPara.CurrentCellDirtyStateChanged += new System.EventHandler(this.grid_CurrentCellDirtyStateChanged);
            //
            // colPHeader
            //
            this.colPHeader.HeaderText = "Parameter (헤더 원문)";
            this.colPHeader.Name = "colPHeader";
            this.colPHeader.ReadOnly = true;
            this.colPHeader.Width = 280;
            //
            // colPName
            //
            this.colPName.HeaderText = "저장 이름";
            this.colPName.Name = "colPName";
            this.colPName.ReadOnly = true;
            this.colPName.Width = 280;
            //
            // colPMeas
            //
            this.colPMeas.HeaderText = "측정";
            this.colPMeas.Name = "colPMeas";
            this.colPMeas.ReadOnly = true;
            this.colPMeas.Width = 90;
            //
            // colPCond
            //
            this.colPCond.HeaderText = "조건";
            this.colPCond.Name = "colPCond";
            this.colPCond.ReadOnly = true;
            this.colPCond.Width = 100;
            //
            // colPT1
            //
            this.colPT1.HeaderText = "단자 1";
            this.colPT1.Name = "colPT1";
            this.colPT1.ReadOnly = true;
            this.colPT1.Width = 150;
            //
            // colPT2
            //
            this.colPT2.HeaderText = "단자 2";
            this.colPT2.Name = "colPT2";
            this.colPT2.ReadOnly = true;
            this.colPT2.Width = 150;
            //
            // colPRep
            //
            this.colPRep.HeaderText = "대표 Chain";
            this.colPRep.Name = "colPRep";
            this.colPRep.ReadOnly = true;
            this.colPRep.Width = 180;
            //
            // colPGds
            //
            this.colPGds.HeaderText = "GDS Chain";
            this.colPGds.Name = "colPGds";
            this.colPGds.ReadOnly = true;
            this.colPGds.Width = 180;
            //
            // colPUnit
            //
            this.colPUnit.HeaderText = "단위";
            this.colPUnit.Name = "colPUnit";
            this.colPUnit.ReadOnly = true;
            this.colPUnit.Width = 50;
            //
            // colPSpec
            //
            this.colPSpec.HeaderText = "Spec (Lower ~ Upper)";
            this.colPSpec.Name = "colPSpec";
            this.colPSpec.ReadOnly = true;
            this.colPSpec.Width = 140;
            //
            // colPUse
            //
            this.colPUse.HeaderText = "사용";
            this.colPUse.Name = "colPUse";
            this.colPUse.Width = 45;
            //
            // colPFiles
            //
            this.colPFiles.HeaderText = "값 파일";
            this.colPFiles.Name = "colPFiles";
            this.colPFiles.ReadOnly = true;
            this.colPFiles.Width = 60;
            //
            // colPSource
            //
            this.colPSource.HeaderText = "출처";
            this.colPSource.Name = "colPSource";
            this.colPSource.ReadOnly = true;
            this.colPSource.Width = 60;
            //
            // colPRegFile
            //
            this.colPRegFile.HeaderText = "등록 파일";
            this.colPRegFile.Name = "colPRegFile";
            this.colPRegFile.ReadOnly = true;
            this.colPRegFile.Width = 200;
            //
            // colPRemark
            //
            this.colPRemark.HeaderText = "비고";
            this.colPRemark.Name = "colPRemark";
            this.colPRemark.Width = 200;
            //
            // colPUpdate
            //
            this.colPUpdate.HeaderText = "수정";
            this.colPUpdate.Name = "colPUpdate";
            this.colPUpdate.ReadOnly = true;
            this.colPUpdate.Width = 170;
            //
            // repBar
            //
            this.repBar.Controls.Add(this.lblGdsDevice);
            this.repBar.Controls.Add(this.cboGdsDevice);
            this.repBar.Controls.Add(this.lblRepName);
            this.repBar.Controls.Add(this.cboRepName);
            this.repBar.Controls.Add(this.lblRepPreview);
            this.repBar.Controls.Add(this.btnRepGroup);
            this.repBar.Controls.Add(this.btnRepRemove);
            this.repBar.Controls.Add(this.btnRepRename);
            this.repBar.Controls.Add(this.btnRepDelete);
            this.repBar.Controls.Add(this.btnLinkName);
            this.repBar.Controls.Add(this.btnLinkManual);
            this.repBar.Controls.Add(this.btnLinkClear);
            this.repBar.Controls.Add(this.chkUnassignedOnly);
            this.repBar.Controls.Add(this.lblRepFind);
            this.repBar.Controls.Add(this.txtRepFind);
            this.repBar.Name = "repBar";
            this.repBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.repBar.Location = new System.Drawing.Point(0, 0);
            this.repBar.Size = new System.Drawing.Size(1292, 62);
            this.repBar.TabIndex = 0;
            //
            // lblGdsDevice
            //
            this.lblGdsDevice.Name = "lblGdsDevice";
            this.lblGdsDevice.AutoSize = true;
            this.lblGdsDevice.Location = new System.Drawing.Point(8, 12);
            this.lblGdsDevice.Size = new System.Drawing.Size(67, 12);
            this.lblGdsDevice.TabIndex = 0;
            this.lblGdsDevice.Text = "GDS Device";
            //
            // cboGdsDevice
            //
            this.cboGdsDevice.Name = "cboGdsDevice";
            this.cboGdsDevice.FormattingEnabled = true;
            this.cboGdsDevice.Location = new System.Drawing.Point(82, 8);
            this.cboGdsDevice.Size = new System.Drawing.Size(150, 20);
            this.cboGdsDevice.TabIndex = 1;
            this.cboGdsDevice.SelectedIndexChanged += new System.EventHandler(this.cboGdsDevice_SelectedIndexChanged);
            this.cboGdsDevice.Leave += new System.EventHandler(this.cboGdsDevice_Leave);
            //
            // lblRepName
            //
            this.lblRepName.Name = "lblRepName";
            this.lblRepName.AutoSize = true;
            this.lblRepName.Location = new System.Drawing.Point(248, 12);
            this.lblRepName.Size = new System.Drawing.Size(57, 12);
            this.lblRepName.TabIndex = 2;
            this.lblRepName.Text = "대표 이름";
            //
            // cboRepName
            //
            this.cboRepName.Name = "cboRepName";
            this.cboRepName.FormattingEnabled = true;
            this.cboRepName.Location = new System.Drawing.Point(312, 8);
            this.cboRepName.Size = new System.Drawing.Size(220, 20);
            this.cboRepName.TabIndex = 3;
            this.cboRepName.DropDown += new System.EventHandler(this.cboRepName_DropDown);
            this.cboRepName.TextChanged += new System.EventHandler(this.cboRepName_TextChanged);
            //
            // lblRepPreview
            //
            this.lblRepPreview.Name = "lblRepPreview";
            this.lblRepPreview.AutoSize = true;
            this.lblRepPreview.ForeColor = System.Drawing.Color.DimGray;
            this.lblRepPreview.Location = new System.Drawing.Point(540, 12);
            this.lblRepPreview.Size = new System.Drawing.Size(200, 12);
            this.lblRepPreview.TabIndex = 4;
            this.lblRepPreview.Text = "저장 이름: -";
            //
            // btnRepGroup
            //
            this.btnRepGroup.Name = "btnRepGroup";
            this.btnRepGroup.Location = new System.Drawing.Point(8, 34);
            this.btnRepGroup.Size = new System.Drawing.Size(110, 23);
            this.btnRepGroup.TabIndex = 5;
            this.btnRepGroup.Text = "선택 단자 묶기";
            this.btnRepGroup.UseVisualStyleBackColor = true;
            this.btnRepGroup.Click += new System.EventHandler(this.btnRepGroup_Click);
            //
            // btnRepRemove
            //
            this.btnRepRemove.Name = "btnRepRemove";
            this.btnRepRemove.Location = new System.Drawing.Point(122, 34);
            this.btnRepRemove.Size = new System.Drawing.Size(105, 23);
            this.btnRepRemove.TabIndex = 6;
            this.btnRepRemove.Text = "대표에서 빼기";
            this.btnRepRemove.UseVisualStyleBackColor = true;
            this.btnRepRemove.Click += new System.EventHandler(this.btnRepRemove_Click);
            //
            // btnRepRename
            //
            this.btnRepRename.Name = "btnRepRename";
            this.btnRepRename.Location = new System.Drawing.Point(231, 34);
            this.btnRepRename.Size = new System.Drawing.Size(115, 23);
            this.btnRepRename.TabIndex = 7;
            this.btnRepRename.Text = "대표 이름 바꾸기";
            this.btnRepRename.UseVisualStyleBackColor = true;
            this.btnRepRename.Click += new System.EventHandler(this.btnRepRename_Click);
            //
            // btnRepDelete
            //
            this.btnRepDelete.Name = "btnRepDelete";
            this.btnRepDelete.Location = new System.Drawing.Point(350, 34);
            this.btnRepDelete.Size = new System.Drawing.Size(85, 23);
            this.btnRepDelete.TabIndex = 8;
            this.btnRepDelete.Text = "대표 삭제";
            this.btnRepDelete.UseVisualStyleBackColor = true;
            this.btnRepDelete.Click += new System.EventHandler(this.btnRepDelete_Click);
            //
            // btnLinkName
            //
            this.btnLinkName.Name = "btnLinkName";
            this.btnLinkName.Location = new System.Drawing.Point(455, 34);
            this.btnLinkName.Size = new System.Drawing.Size(105, 23);
            this.btnLinkName.TabIndex = 9;
            this.btnLinkName.Text = "GDS 이름 연결";
            this.btnLinkName.UseVisualStyleBackColor = true;
            this.btnLinkName.Click += new System.EventHandler(this.btnLinkName_Click);
            //
            // btnLinkManual
            //
            this.btnLinkManual.Name = "btnLinkManual";
            this.btnLinkManual.Location = new System.Drawing.Point(564, 34);
            this.btnLinkManual.Size = new System.Drawing.Size(110, 23);
            this.btnLinkManual.TabIndex = 10;
            this.btnLinkManual.Text = "GDS 선택 연결";
            this.btnLinkManual.UseVisualStyleBackColor = true;
            this.btnLinkManual.Click += new System.EventHandler(this.btnLinkManual_Click);
            //
            // btnLinkClear
            //
            this.btnLinkClear.Name = "btnLinkClear";
            this.btnLinkClear.Location = new System.Drawing.Point(678, 34);
            this.btnLinkClear.Size = new System.Drawing.Size(85, 23);
            this.btnLinkClear.TabIndex = 11;
            this.btnLinkClear.Text = "연결 해제";
            this.btnLinkClear.UseVisualStyleBackColor = true;
            this.btnLinkClear.Click += new System.EventHandler(this.btnLinkClear_Click);
            //
            // chkUnassignedOnly
            //
            this.chkUnassignedOnly.Name = "chkUnassignedOnly";
            this.chkUnassignedOnly.AutoSize = true;
            this.chkUnassignedOnly.Location = new System.Drawing.Point(780, 38);
            this.chkUnassignedOnly.Size = new System.Drawing.Size(100, 16);
            this.chkUnassignedOnly.TabIndex = 12;
            this.chkUnassignedOnly.Text = "대표 미등록만";
            this.chkUnassignedOnly.UseVisualStyleBackColor = true;
            this.chkUnassignedOnly.CheckedChanged += new System.EventHandler(this.terminalFilter_Changed);
            //
            // lblRepFind
            //
            this.lblRepFind.Name = "lblRepFind";
            this.lblRepFind.AutoSize = true;
            this.lblRepFind.Location = new System.Drawing.Point(892, 39);
            this.lblRepFind.Size = new System.Drawing.Size(29, 12);
            this.lblRepFind.TabIndex = 13;
            this.lblRepFind.Text = "찾기";
            //
            // txtRepFind
            //
            this.txtRepFind.Name = "txtRepFind";
            this.txtRepFind.Location = new System.Drawing.Point(926, 35);
            this.txtRepFind.Size = new System.Drawing.Size(160, 21);
            this.txtRepFind.TabIndex = 14;
            this.txtRepFind.TextChanged += new System.EventHandler(this.terminalFilter_Changed);
            //
            // lblRepSummary
            //
            this.lblRepSummary.Name = "lblRepSummary";
            this.lblRepSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRepSummary.ForeColor = System.Drawing.Color.DimGray;
            this.lblRepSummary.Location = new System.Drawing.Point(0, 62);
            this.lblRepSummary.Padding = new System.Windows.Forms.Padding(8, 3, 0, 0);
            this.lblRepSummary.Size = new System.Drawing.Size(1292, 20);
            this.lblRepSummary.TabIndex = 1;
            this.lblRepSummary.Text = "단자를 고르고 대표 이름을 입력한 뒤 [선택 단자 묶기]. GDS Chain 이름이 대표 이름 또는 구성 단자와 같으면 연결됩니다.";
            //
            // splitRepMain
            //
            this.splitRepMain.Name = "splitRepMain";
            this.splitRepMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitRepMain.Location = new System.Drawing.Point(0, 82);
            this.splitRepMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitRepMain.Size = new System.Drawing.Size(1292, 528);
            this.splitRepMain.SplitterDistance = 340;
            this.splitRepMain.TabIndex = 2;
            this.splitRepMain.Panel1.Controls.Add(this.splitRepTop);
            this.splitRepMain.Panel2.Controls.Add(this.dgvRepSpec);
            //
            // splitRepTop
            //
            this.splitRepTop.Name = "splitRepTop";
            this.splitRepTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitRepTop.Location = new System.Drawing.Point(0, 0);
            this.splitRepTop.Size = new System.Drawing.Size(1292, 340);
            this.splitRepTop.SplitterDistance = 470;
            this.splitRepTop.TabIndex = 0;
            this.splitRepTop.Panel1.Controls.Add(this.dgvTerminal);
            this.splitRepTop.Panel2.Controls.Add(this.splitRepRight);
            //
            // splitRepRight
            //
            this.splitRepRight.Name = "splitRepRight";
            this.splitRepRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitRepRight.Location = new System.Drawing.Point(0, 0);
            this.splitRepRight.Size = new System.Drawing.Size(818, 340);
            this.splitRepRight.SplitterDistance = 500;
            this.splitRepRight.TabIndex = 0;
            this.splitRepRight.Panel1.Controls.Add(this.dgvRep);
            this.splitRepRight.Panel2.Controls.Add(this.dgvGds);
            //
            // dgvTerminal
            //
            this.dgvTerminal.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTName,
            this.colTLeg,
            this.colTType,
            this.colTPrograms,
            this.colTPara,
            this.colTSpec,
            this.colTRep});
            this.dgvTerminal.Name = "dgvTerminal";
            this.dgvTerminal.AllowUserToAddRows = false;
            this.dgvTerminal.AllowUserToDeleteRows = false;
            this.dgvTerminal.AllowUserToResizeRows = false;
            this.dgvTerminal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTerminal.Location = new System.Drawing.Point(0, 0);
            this.dgvTerminal.MultiSelect = true;
            this.dgvTerminal.ReadOnly = true;
            this.dgvTerminal.RowHeadersVisible = false;
            this.dgvTerminal.RowTemplate.Height = 23;
            this.dgvTerminal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTerminal.Size = new System.Drawing.Size(470, 340);
            this.dgvTerminal.TabIndex = 0;
            //
            // colTName
            //
            this.colTName.HeaderText = "E-Test 단자";
            this.colTName.Name = "colTName";
            this.colTName.Width = 160;
            //
            // colTLeg
            //
            this.colTLeg.HeaderText = "Leg";
            this.colTLeg.Name = "colTLeg";
            this.colTLeg.Width = 40;
            //
            // colTType
            //
            this.colTType.HeaderText = "종류";
            this.colTType.Name = "colTType";
            this.colTType.Width = 70;
            //
            // colTPrograms
            //
            this.colTPrograms.HeaderText = "공정";
            this.colTPrograms.Name = "colTPrograms";
            this.colTPrograms.Width = 110;
            //
            // colTPara
            //
            this.colTPara.HeaderText = "Para";
            this.colTPara.Name = "colTPara";
            this.colTPara.Width = 45;
            //
            // colTSpec
            //
            this.colTSpec.HeaderText = "Spec";
            this.colTSpec.Name = "colTSpec";
            this.colTSpec.Width = 45;
            //
            // colTRep
            //
            this.colTRep.HeaderText = "대표 Chain";
            this.colTRep.Name = "colTRep";
            this.colTRep.Width = 150;
            //
            // dgvRep
            //
            this.dgvRep.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRName,
            this.colRTerminals,
            this.colRGds,
            this.colRType,
            this.colRStatus,
            this.colRCandidates});
            this.dgvRep.Name = "dgvRep";
            this.dgvRep.AllowUserToAddRows = false;
            this.dgvRep.AllowUserToDeleteRows = false;
            this.dgvRep.AllowUserToResizeRows = false;
            this.dgvRep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRep.Location = new System.Drawing.Point(0, 0);
            this.dgvRep.MultiSelect = false;
            this.dgvRep.ReadOnly = true;
            this.dgvRep.RowHeadersVisible = false;
            this.dgvRep.RowTemplate.Height = 23;
            this.dgvRep.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRep.Size = new System.Drawing.Size(500, 340);
            this.dgvRep.TabIndex = 0;
            this.dgvRep.SelectionChanged += new System.EventHandler(this.dgvRep_SelectionChanged);
            //
            // colRName
            //
            this.colRName.HeaderText = "대표 Chain";
            this.colRName.Name = "colRName";
            this.colRName.Width = 140;
            //
            // colRTerminals
            //
            this.colRTerminals.HeaderText = "구성 단자";
            this.colRTerminals.Name = "colRTerminals";
            this.colRTerminals.Width = 230;
            //
            // colRGds
            //
            this.colRGds.HeaderText = "GDS Chain";
            this.colRGds.Name = "colRGds";
            this.colRGds.Width = 140;
            //
            // colRType
            //
            this.colRType.HeaderText = "방식";
            this.colRType.Name = "colRType";
            this.colRType.Width = 50;
            //
            // colRStatus
            //
            this.colRStatus.HeaderText = "상태";
            this.colRStatus.Name = "colRStatus";
            this.colRStatus.Width = 100;
            //
            // colRCandidates
            //
            this.colRCandidates.HeaderText = "이름이 같은 GDS Chain";
            this.colRCandidates.Name = "colRCandidates";
            this.colRCandidates.Width = 160;
            //
            // dgvGds
            //
            this.dgvGds.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colGName,
            this.colGCode,
            this.colGKey,
            this.colGRep});
            this.dgvGds.Name = "dgvGds";
            this.dgvGds.AllowUserToAddRows = false;
            this.dgvGds.AllowUserToDeleteRows = false;
            this.dgvGds.AllowUserToResizeRows = false;
            this.dgvGds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGds.Location = new System.Drawing.Point(0, 0);
            this.dgvGds.MultiSelect = false;
            this.dgvGds.ReadOnly = true;
            this.dgvGds.RowHeadersVisible = false;
            this.dgvGds.RowTemplate.Height = 23;
            this.dgvGds.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGds.Size = new System.Drawing.Size(314, 340);
            this.dgvGds.TabIndex = 0;
            //
            // colGName
            //
            this.colGName.HeaderText = "GDS Setup Chain";
            this.colGName.Name = "colGName";
            this.colGName.Width = 140;
            //
            // colGCode
            //
            this.colGCode.HeaderText = "Code";
            this.colGCode.Name = "colGCode";
            this.colGCode.Width = 50;
            //
            // colGKey
            //
            this.colGKey.HeaderText = "비교 이름";
            this.colGKey.Name = "colGKey";
            this.colGKey.Width = 140;
            //
            // colGRep
            //
            this.colGRep.HeaderText = "연결 대표";
            this.colGRep.Name = "colGRep";
            this.colGRep.Width = 130;
            //
            // dgvRepSpec
            //
            this.dgvRepSpec.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSProgram,
            this.colSDevice,
            this.colSHeader,
            this.colSName,
            this.colSPosition,
            this.colSLower,
            this.colSUpper,
            this.colSEnable,
            this.colSUnit,
            this.colSSpec});
            this.dgvRepSpec.Name = "dgvRepSpec";
            this.dgvRepSpec.AllowUserToAddRows = false;
            this.dgvRepSpec.AllowUserToDeleteRows = false;
            this.dgvRepSpec.AllowUserToResizeRows = false;
            this.dgvRepSpec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRepSpec.Location = new System.Drawing.Point(0, 0);
            this.dgvRepSpec.ReadOnly = true;
            this.dgvRepSpec.RowHeadersVisible = false;
            this.dgvRepSpec.RowTemplate.Height = 23;
            this.dgvRepSpec.Size = new System.Drawing.Size(1292, 184);
            this.dgvRepSpec.TabIndex = 0;
            //
            // colSProgram
            //
            this.colSProgram.HeaderText = "공정";
            this.colSProgram.Name = "colSProgram";
            this.colSProgram.Width = 60;
            //
            // colSDevice
            //
            this.colSDevice.HeaderText = "Device";
            this.colSDevice.Name = "colSDevice";
            this.colSDevice.Width = 80;
            //
            // colSHeader
            //
            this.colSHeader.HeaderText = "Parameter (헤더 원문)";
            this.colSHeader.Name = "colSHeader";
            this.colSHeader.Width = 300;
            //
            // colSName
            //
            this.colSName.HeaderText = "저장 이름";
            this.colSName.Name = "colSName";
            this.colSName.Width = 280;
            //
            // colSPosition
            //
            this.colSPosition.HeaderText = "단자 위치";
            this.colSPosition.Name = "colSPosition";
            this.colSPosition.Width = 65;
            //
            // colSLower
            //
            this.colSLower.HeaderText = "Lower";
            this.colSLower.Name = "colSLower";
            this.colSLower.Width = 70;
            //
            // colSUpper
            //
            this.colSUpper.HeaderText = "Upper";
            this.colSUpper.Name = "colSUpper";
            this.colSUpper.Width = 70;
            //
            // colSEnable
            //
            this.colSEnable.HeaderText = "Enable";
            this.colSEnable.Name = "colSEnable";
            this.colSEnable.Width = 55;
            //
            // colSUnit
            //
            this.colSUnit.HeaderText = "단위";
            this.colSUnit.Name = "colSUnit";
            this.colSUnit.Width = 50;
            //
            // colSSpec
            //
            this.colSSpec.HeaderText = "Spec 원문";
            this.colSSpec.Name = "colSSpec";
            this.colSSpec.Width = 220;
            //
            // lblStatus
            //
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Location = new System.Drawing.Point(0, 698);
            this.lblStatus.Padding = new System.Windows.Forms.Padding(8, 4, 0, 0);
            this.lblStatus.Size = new System.Drawing.Size(1300, 22);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Factory / Device / 공정을 고르고 [조회] 하거나 [결과 파일 열기] 로 헤더를 등록하세요.";
            //
            // frmEtParaSetup
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 720);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.topBar);
            this.Name = "frmEtParaSetup";
            this.Text = "E-Test Para 설정";
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.tabMain.ResumeLayout(false);
            this.tabFile.ResumeLayout(false);
            this.fileBar.ResumeLayout(false);
            this.fileBar.PerformLayout();
            this.tabPara.ResumeLayout(false);
            this.tabRep.ResumeLayout(false);
            this.repBar.ResumeLayout(false);
            this.repBar.PerformLayout();
            this.splitRepRight.Panel1.ResumeLayout(false);
            this.splitRepRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitRepRight)).EndInit();
            this.splitRepRight.ResumeLayout(false);
            this.splitRepTop.Panel1.ResumeLayout(false);
            this.splitRepTop.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitRepTop)).EndInit();
            this.splitRepTop.ResumeLayout(false);
            this.splitRepMain.Panel1.ResumeLayout(false);
            this.splitRepMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitRepMain)).EndInit();
            this.splitRepMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPara)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTerminal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRepSpec)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Label lblFactory;
        private System.Windows.Forms.TextBox txtFactory;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.ComboBox cboDevice;
        private System.Windows.Forms.Label lblTestProgram;
        private System.Windows.Forms.ComboBox cboTestProgram;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.Button btnSpecPick;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label lblGuide;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabFile;
        private System.Windows.Forms.TabPage tabPara;
        private System.Windows.Forms.TabPage tabRep;
        private System.Windows.Forms.Panel fileBar;
        private System.Windows.Forms.Label lblFileInfo;
        private System.Windows.Forms.Label lblMeasFilter;
        private System.Windows.Forms.ComboBox cboMeasFilter;
        private System.Windows.Forms.Label lblTypeFilter;
        private System.Windows.Forms.ComboBox cboTypeFilter;
        private System.Windows.Forms.Label lblFind;
        private System.Windows.Forms.TextBox txtFind;
        private System.Windows.Forms.CheckBox chkNewOnly;
        private System.Windows.Forms.Button btnCheckVisible;
        private System.Windows.Forms.Button btnUncheckAll;
        private System.Windows.Forms.DataGridView dgvHeader;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colHCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHHeader;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHMeas;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHCond;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHT1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHLeg1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHT2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHLeg2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHTestNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHRep;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHMessage;
        private System.Windows.Forms.DataGridView dgvPara;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPHeader;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPMeas;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPCond;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPT1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPT2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPRep;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPGds;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPSpec;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colPUse;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPFiles;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPRegFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPRemark;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPUpdate;
        private System.Windows.Forms.Panel repBar;
        private System.Windows.Forms.Label lblGdsDevice;
        private System.Windows.Forms.ComboBox cboGdsDevice;
        private System.Windows.Forms.Label lblRepName;
        private System.Windows.Forms.ComboBox cboRepName;
        private System.Windows.Forms.Label lblRepPreview;
        private System.Windows.Forms.Button btnRepGroup;
        private System.Windows.Forms.Button btnRepRemove;
        private System.Windows.Forms.Button btnRepRename;
        private System.Windows.Forms.Button btnRepDelete;
        private System.Windows.Forms.Button btnLinkName;
        private System.Windows.Forms.Button btnLinkManual;
        private System.Windows.Forms.Button btnLinkClear;
        private System.Windows.Forms.CheckBox chkUnassignedOnly;
        private System.Windows.Forms.Label lblRepFind;
        private System.Windows.Forms.TextBox txtRepFind;
        private System.Windows.Forms.Label lblRepSummary;
        private System.Windows.Forms.SplitContainer splitRepMain;
        private System.Windows.Forms.SplitContainer splitRepTop;
        private System.Windows.Forms.SplitContainer splitRepRight;
        private System.Windows.Forms.DataGridView dgvTerminal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTLeg;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTPrograms;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTPara;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTRep;
        private System.Windows.Forms.DataGridView dgvRep;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRTerminals;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRGds;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRCandidates;
        private System.Windows.Forms.DataGridView dgvGds;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGRep;
        private System.Windows.Forms.DataGridView dgvRepSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSProgram;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSDevice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSHeader;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSPosition;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSLower;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSUpper;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSEnable;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSSpec;
        private System.Windows.Forms.Label lblStatus;
    }
}
