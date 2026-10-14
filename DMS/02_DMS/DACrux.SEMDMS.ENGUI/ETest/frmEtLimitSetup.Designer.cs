namespace DACrux.SEMDMS.ENGUI
{
    // E-Test Spec 설정 화면의 컨트롤과 배치를 정의한다. 조회 / 저장 / 붙여넣기 / Chain 묶기 동작은 본문 파일(frmEtLimitSetup.cs)에서 처리한다. (2026-10-12 Parameter 단위 + Chain 묶기 + Setup Chain 으로 찾기)
    partial class frmEtLimitSetup
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>화면 종료 시 디자이너 구성 요소를 해제한다.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// 위: Factory / Device / 공정 / [조회] [저장] [선택 행 삭제] [Spec 표 붙여넣기] + 안내 문구.
        /// 가운데 탭: [Spec] Parameter 별 Spec 표 (입력 칸 = Lower / Upper / Enable / 비고) / [Chain 묶기] E-Test Chain 묶음 / [Setup Chain 으로 찾기] GDS Setup Chain -> 묶인 Chain 의 Spec. 아래: 상태 표시줄.
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnPaste = new System.Windows.Forms.Button();
            this.lblGuide = new System.Windows.Forms.Label();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabSpec = new System.Windows.Forms.TabPage();
            this.tabChain = new System.Windows.Forms.TabPage();
            this.tabLookup = new System.Windows.Forms.TabPage();
            this.dgvLimit = new System.Windows.Forms.DataGridView();
            this.chainBar = new System.Windows.Forms.Panel();
            this.lblChainFilter = new System.Windows.Forms.Label();
            this.txtChainFilter = new System.Windows.Forms.TextBox();
            this.btnChainGroup = new System.Windows.Forms.Button();
            this.btnChainAuto = new System.Windows.Forms.Button();
            this.btnChainUngroup = new System.Windows.Forms.Button();
            this.lblChainSelect = new System.Windows.Forms.Label();
            this.lblChainInfo = new System.Windows.Forms.Label();
            this.dgvChain = new System.Windows.Forms.DataGridView();
            this.lookupLeft = new System.Windows.Forms.Panel();
            this.lblSetupChain = new System.Windows.Forms.Label();
            this.cboSetupChain = new System.Windows.Forms.ComboBox();
            this.btnLookup = new System.Windows.Forms.Button();
            this.lstSetupChain = new System.Windows.Forms.ListBox();
            this.lblLookupInfo = new System.Windows.Forms.Label();
            this.dgvLookup = new System.Windows.Forms.DataGridView();
            this.lblStatus = new System.Windows.Forms.Label();
            this.colItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValues = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNotTested = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMedian = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colP99 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLower = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUpper = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEnable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colFail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRemark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUpdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChainName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChainItems = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChainGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChainGroupCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChainSetup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChainRemark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChainUpdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLkProgram = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLkItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLkMatch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLkDefined = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLkLower = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLkUpper = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLkUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLkEnable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLkFail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLkFiles = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLkValues = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLkNotTested = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLkMin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLkMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLkSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.topBar.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabSpec.SuspendLayout();
            this.tabChain.SuspendLayout();
            this.chainBar.SuspendLayout();
            this.tabLookup.SuspendLayout();
            this.lookupLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLookup)).BeginInit();
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
            this.topBar.Controls.Add(this.btnSave);
            this.topBar.Controls.Add(this.btnDelete);
            this.topBar.Controls.Add(this.btnPaste);
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
            this.cboDevice.Size = new System.Drawing.Size(180, 20);
            this.cboDevice.TabIndex = 3;
            this.cboDevice.SelectedIndexChanged += new System.EventHandler(this.cboDevice_SelectedIndexChanged);
            //
            // lblTestProgram
            //
            this.lblTestProgram.Name = "lblTestProgram";
            this.lblTestProgram.AutoSize = true;
            this.lblTestProgram.Location = new System.Drawing.Point(404, 13);
            this.lblTestProgram.Size = new System.Drawing.Size(29, 12);
            this.lblTestProgram.TabIndex = 4;
            this.lblTestProgram.Text = "공정";
            //
            // cboTestProgram
            //
            this.cboTestProgram.Name = "cboTestProgram";
            this.cboTestProgram.FormattingEnabled = true;
            this.cboTestProgram.Location = new System.Drawing.Point(439, 9);
            this.cboTestProgram.Size = new System.Drawing.Size(100, 20);
            this.cboTestProgram.TabIndex = 5;
            this.cboTestProgram.SelectedIndexChanged += new System.EventHandler(this.cboTestProgram_SelectedIndexChanged);
            //
            // btnSearch
            //
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Location = new System.Drawing.Point(548, 8);
            this.btnSearch.Size = new System.Drawing.Size(70, 23);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            //
            // btnSave
            //
            this.btnSave.Name = "btnSave";
            this.btnSave.Location = new System.Drawing.Point(624, 8);
            this.btnSave.Size = new System.Drawing.Size(70, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // btnDelete
            //
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Location = new System.Drawing.Point(700, 8);
            this.btnDelete.Size = new System.Drawing.Size(100, 23);
            this.btnDelete.TabIndex = 8;
            this.btnDelete.Text = "선택 행 삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            //
            // btnPaste
            //
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Location = new System.Drawing.Point(806, 8);
            this.btnPaste.Size = new System.Drawing.Size(130, 23);
            this.btnPaste.TabIndex = 9;
            this.btnPaste.Text = "Spec 표 붙여넣기";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            //
            // lblGuide
            //
            this.lblGuide.Name = "lblGuide";
            this.lblGuide.AutoSize = true;
            this.lblGuide.ForeColor = System.Drawing.Color.DimGray;
            this.lblGuide.Location = new System.Drawing.Point(10, 40);
            this.lblGuide.Size = new System.Drawing.Size(900, 12);
            this.lblGuide.TabIndex = 10;
            this.lblGuide.Text = "Spec = Device + 공정 + Parameter(헤더 전체, 끝 '#번호' 제거, 대문자). 고객 Spec 표에 있는 Parameter 만 판정합니다. 값 < Lower 또는 값 > Upper 이면 Fail(경계값 Pass), Enable 인 Spec 은 '-' 값도 Fail. 한쪽 Limit 만 가능.";
            //
            // tabMain
            //
            this.tabMain.Controls.Add(this.tabSpec);
            this.tabMain.Controls.Add(this.tabChain);
            this.tabMain.Controls.Add(this.tabLookup);
            this.tabMain.Name = "tabMain";
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 62);
            this.tabMain.Size = new System.Drawing.Size(1300, 612);
            this.tabMain.TabIndex = 1;
            this.tabMain.SelectedIndex = 0;
            //
            // tabSpec
            //
            this.tabSpec.Controls.Add(this.dgvLimit);
            this.tabSpec.Name = "tabSpec";
            this.tabSpec.Location = new System.Drawing.Point(4, 22);
            this.tabSpec.Size = new System.Drawing.Size(1292, 586);
            this.tabSpec.TabIndex = 0;
            this.tabSpec.Text = "Spec (Parameter)";
            this.tabSpec.UseVisualStyleBackColor = true;
            //
            // tabChain
            //
            this.tabChain.Controls.Add(this.dgvChain);
            this.tabChain.Controls.Add(this.chainBar);
            this.tabChain.Name = "tabChain";
            this.tabChain.Location = new System.Drawing.Point(4, 22);
            this.tabChain.Size = new System.Drawing.Size(1292, 586);
            this.tabChain.TabIndex = 1;
            this.tabChain.Text = "Chain 묶기";
            this.tabChain.UseVisualStyleBackColor = true;
            //
            // tabLookup
            //
            this.tabLookup.Controls.Add(this.dgvLookup);
            this.tabLookup.Controls.Add(this.lblLookupInfo);
            this.tabLookup.Controls.Add(this.lookupLeft);
            this.tabLookup.Name = "tabLookup";
            this.tabLookup.Location = new System.Drawing.Point(4, 22);
            this.tabLookup.Size = new System.Drawing.Size(1292, 586);
            this.tabLookup.TabIndex = 2;
            this.tabLookup.Text = "Setup Chain 으로 찾기";
            this.tabLookup.UseVisualStyleBackColor = true;
            //
            // dgvLimit
            //
            this.dgvLimit.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colItem,
            this.colGroup,
            this.colSetup,
            this.colUnit,
            this.colInData,
            this.colValues,
            this.colNotTested,
            this.colMin,
            this.colMedian,
            this.colP99,
            this.colMax,
            this.colLower,
            this.colUpper,
            this.colEnable,
            this.colFail,
            this.colSpec,
            this.colRemark,
            this.colUpdate});
            this.dgvLimit.Name = "dgvLimit";
            this.dgvLimit.AllowUserToAddRows = false;
            this.dgvLimit.AllowUserToDeleteRows = false;
            this.dgvLimit.AllowUserToResizeRows = false;
            this.dgvLimit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLimit.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystrokeOrF2;
            this.dgvLimit.Location = new System.Drawing.Point(0, 0);
            this.dgvLimit.MultiSelect = true;
            this.dgvLimit.RowHeadersVisible = false;
            this.dgvLimit.RowTemplate.Height = 23;
            this.dgvLimit.Size = new System.Drawing.Size(1292, 586);
            this.dgvLimit.TabIndex = 0;
            this.dgvLimit.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLimit_CellValueChanged);
            this.dgvLimit.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvLimit_CurrentCellDirtyStateChanged);
            //
            // chainBar
            //
            this.chainBar.Controls.Add(this.lblChainFilter);
            this.chainBar.Controls.Add(this.txtChainFilter);
            this.chainBar.Controls.Add(this.btnChainGroup);
            this.chainBar.Controls.Add(this.btnChainAuto);
            this.chainBar.Controls.Add(this.btnChainUngroup);
            this.chainBar.Controls.Add(this.lblChainSelect);
            this.chainBar.Controls.Add(this.lblChainInfo);
            this.chainBar.Name = "chainBar";
            this.chainBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.chainBar.Location = new System.Drawing.Point(0, 0);
            this.chainBar.Size = new System.Drawing.Size(1292, 58);
            this.chainBar.TabIndex = 0;
            //
            // lblChainFilter
            //
            this.lblChainFilter.Name = "lblChainFilter";
            this.lblChainFilter.AutoSize = true;
            this.lblChainFilter.Location = new System.Drawing.Point(8, 12);
            this.lblChainFilter.Size = new System.Drawing.Size(29, 12);
            this.lblChainFilter.TabIndex = 0;
            this.lblChainFilter.Text = "찾기";
            //
            // txtChainFilter
            //
            this.txtChainFilter.Name = "txtChainFilter";
            this.txtChainFilter.Location = new System.Drawing.Point(42, 8);
            this.txtChainFilter.Size = new System.Drawing.Size(170, 21);
            this.txtChainFilter.TabIndex = 1;
            this.txtChainFilter.TextChanged += new System.EventHandler(this.txtChainFilter_TextChanged);
            //
            // btnChainGroup
            //
            this.btnChainGroup.Name = "btnChainGroup";
            this.btnChainGroup.Location = new System.Drawing.Point(222, 7);
            this.btnChainGroup.Size = new System.Drawing.Size(120, 23);
            this.btnChainGroup.TabIndex = 2;
            this.btnChainGroup.Text = "선택 Chain 묶기";
            this.btnChainGroup.UseVisualStyleBackColor = true;
            this.btnChainGroup.Click += new System.EventHandler(this.btnChainGroup_Click);
            //
            // btnChainAuto
            //
            this.btnChainAuto.Name = "btnChainAuto";
            this.btnChainAuto.Location = new System.Drawing.Point(348, 7);
            this.btnChainAuto.Size = new System.Drawing.Size(135, 23);
            this.btnChainAuto.TabIndex = 3;
            this.btnChainAuto.Text = "FH / FL 짝 자동 묶기";
            this.btnChainAuto.UseVisualStyleBackColor = true;
            this.btnChainAuto.Click += new System.EventHandler(this.btnChainAuto_Click);
            //
            // btnChainUngroup
            //
            this.btnChainUngroup.Name = "btnChainUngroup";
            this.btnChainUngroup.Location = new System.Drawing.Point(489, 7);
            this.btnChainUngroup.Size = new System.Drawing.Size(110, 23);
            this.btnChainUngroup.TabIndex = 4;
            this.btnChainUngroup.Text = "선택 묶음 해제";
            this.btnChainUngroup.UseVisualStyleBackColor = true;
            this.btnChainUngroup.Click += new System.EventHandler(this.btnChainUngroup_Click);
            //
            // lblChainSelect
            //
            this.lblChainSelect.Name = "lblChainSelect";
            this.lblChainSelect.AutoSize = true;
            this.lblChainSelect.Location = new System.Drawing.Point(611, 12);
            this.lblChainSelect.Size = new System.Drawing.Size(50, 12);
            this.lblChainSelect.TabIndex = 5;
            this.lblChainSelect.Text = "선택 0개";
            //
            // lblChainInfo
            //
            this.lblChainInfo.Name = "lblChainInfo";
            this.lblChainInfo.AutoSize = true;
            this.lblChainInfo.ForeColor = System.Drawing.Color.DimGray;
            this.lblChainInfo.Location = new System.Drawing.Point(8, 38);
            this.lblChainInfo.Size = new System.Drawing.Size(900, 12);
            this.lblChainInfo.TabIndex = 6;
            this.lblChainInfo.Text = "E-Test Chain(측정 헤더 @ 뒤 / : 뒤)을 묶습니다(보통 _FH + _FL). Setup Chain 으로 찾으면 묶인 Chain 의 Spec 을 모두 찾습니다(Device 단위, 공정 무관).";
            //
            // dgvChain
            //
            this.dgvChain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colChainName,
            this.colChainItems,
            this.colChainGroup,
            this.colChainGroupCount,
            this.colChainSetup,
            this.colChainRemark,
            this.colChainUpdate});
            this.dgvChain.Name = "dgvChain";
            this.dgvChain.AllowUserToAddRows = false;
            this.dgvChain.AllowUserToDeleteRows = false;
            this.dgvChain.AllowUserToResizeRows = false;
            this.dgvChain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvChain.Location = new System.Drawing.Point(0, 58);
            this.dgvChain.MultiSelect = true;
            this.dgvChain.ReadOnly = true;
            this.dgvChain.RowHeadersVisible = false;
            this.dgvChain.RowTemplate.Height = 23;
            this.dgvChain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvChain.Size = new System.Drawing.Size(1292, 528);
            this.dgvChain.TabIndex = 1;
            this.dgvChain.SelectionChanged += new System.EventHandler(this.dgvChain_SelectionChanged);
            //
            // lookupLeft
            //
            this.lookupLeft.Controls.Add(this.lblSetupChain);
            this.lookupLeft.Controls.Add(this.cboSetupChain);
            this.lookupLeft.Controls.Add(this.btnLookup);
            this.lookupLeft.Controls.Add(this.lstSetupChain);
            this.lookupLeft.Name = "lookupLeft";
            this.lookupLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.lookupLeft.Location = new System.Drawing.Point(0, 0);
            this.lookupLeft.Size = new System.Drawing.Size(270, 586);
            this.lookupLeft.TabIndex = 0;
            //
            // lblSetupChain
            //
            this.lblSetupChain.Name = "lblSetupChain";
            this.lblSetupChain.AutoSize = true;
            this.lblSetupChain.Location = new System.Drawing.Point(8, 10);
            this.lblSetupChain.Size = new System.Drawing.Size(80, 12);
            this.lblSetupChain.TabIndex = 0;
            this.lblSetupChain.Text = "Setup Chain (GDS)";
            //
            // cboSetupChain
            //
            this.cboSetupChain.Name = "cboSetupChain";
            this.cboSetupChain.FormattingEnabled = true;
            this.cboSetupChain.Location = new System.Drawing.Point(8, 28);
            this.cboSetupChain.Size = new System.Drawing.Size(180, 20);
            this.cboSetupChain.TabIndex = 1;
            this.cboSetupChain.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboSetupChain_KeyDown);
            //
            // btnLookup
            //
            this.btnLookup.Name = "btnLookup";
            this.btnLookup.Location = new System.Drawing.Point(194, 27);
            this.btnLookup.Size = new System.Drawing.Size(68, 23);
            this.btnLookup.TabIndex = 2;
            this.btnLookup.Text = "찾기";
            this.btnLookup.UseVisualStyleBackColor = true;
            this.btnLookup.Click += new System.EventHandler(this.btnLookup_Click);
            //
            // lstSetupChain
            //
            this.lstSetupChain.Name = "lstSetupChain";
            this.lstSetupChain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lstSetupChain.FormattingEnabled = true;
            this.lstSetupChain.IntegralHeight = false;
            this.lstSetupChain.ItemHeight = 12;
            this.lstSetupChain.Location = new System.Drawing.Point(8, 56);
            this.lstSetupChain.Size = new System.Drawing.Size(254, 522);
            this.lstSetupChain.TabIndex = 3;
            this.lstSetupChain.SelectedIndexChanged += new System.EventHandler(this.lstSetupChain_SelectedIndexChanged);
            //
            // lblLookupInfo
            //
            this.lblLookupInfo.Name = "lblLookupInfo";
            this.lblLookupInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLookupInfo.Location = new System.Drawing.Point(270, 0);
            this.lblLookupInfo.Padding = new System.Windows.Forms.Padding(6, 4, 0, 0);
            this.lblLookupInfo.Size = new System.Drawing.Size(1022, 44);
            this.lblLookupInfo.TabIndex = 1;
            this.lblLookupInfo.Text = "Setup Chain 을 고르면 같은 이름 E-Test Chain(없으면 이름_FH / _FL)과 묶인 Chain 의 Spec / 측정 항목을 모두 보여 줍니다.";
            //
            // dgvLookup
            //
            this.dgvLookup.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLkProgram,
            this.colLkItem,
            this.colLkMatch,
            this.colLkDefined,
            this.colLkLower,
            this.colLkUpper,
            this.colLkUnit,
            this.colLkEnable,
            this.colLkFail,
            this.colLkFiles,
            this.colLkValues,
            this.colLkNotTested,
            this.colLkMin,
            this.colLkMax,
            this.colLkSpec});
            this.dgvLookup.Name = "dgvLookup";
            this.dgvLookup.AllowUserToAddRows = false;
            this.dgvLookup.AllowUserToDeleteRows = false;
            this.dgvLookup.AllowUserToResizeRows = false;
            this.dgvLookup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLookup.Location = new System.Drawing.Point(270, 44);
            this.dgvLookup.MultiSelect = false;
            this.dgvLookup.ReadOnly = true;
            this.dgvLookup.RowHeadersVisible = false;
            this.dgvLookup.RowTemplate.Height = 23;
            this.dgvLookup.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLookup.Size = new System.Drawing.Size(1022, 542);
            this.dgvLookup.TabIndex = 2;
            this.dgvLookup.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLookup_CellDoubleClick);
            //
            // lblStatus
            //
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Location = new System.Drawing.Point(0, 674);
            this.lblStatus.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lblStatus.Size = new System.Drawing.Size(1300, 26);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Device / 공정을 고르고 [조회] 를 누르세요. 고객 Spec 표는 Excel 에서 복사한 뒤 [Spec 표 붙여넣기].";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // colItem
            //
            this.colItem.HeaderText = "Parameter";
            this.colItem.Name = "colItem";
            this.colItem.ReadOnly = true;
            this.colItem.Width = 330;
            //
            // colGroup
            //
            this.colGroup.HeaderText = "묶음";
            this.colGroup.Name = "colGroup";
            this.colGroup.ReadOnly = true;
            this.colGroup.ToolTipText = "[Chain 묶기] 탭의 묶음. Chain 이 2개인 항목은 'Chain1 쪽 : Chain2 쪽'";
            this.colGroup.Width = 200;
            //
            // colSetup
            //
            this.colSetup.HeaderText = "Setup Chain";
            this.colSetup.Name = "colSetup";
            this.colSetup.ReadOnly = true;
            this.colSetup.ToolTipText = "이 항목 Chain(과 묶인 Chain)에 연결되는 GDS Setup Chain (같은 이름 또는 이름 + _FH / _FL)";
            this.colSetup.Width = 130;
            //
            // colUnit
            //
            this.colUnit.HeaderText = "단위";
            this.colUnit.Name = "colUnit";
            this.colUnit.ReadOnly = true;
            this.colUnit.Width = 45;
            //
            // colInData
            //
            this.colInData.HeaderText = "데이터";
            this.colInData.Name = "colInData";
            this.colInData.ReadOnly = true;
            this.colInData.ToolTipText = "Y = 이 공정 READY 파일에 값이 있음";
            this.colInData.Width = 50;
            //
            // colValues
            //
            this.colValues.HeaderText = "값 수";
            this.colValues.Name = "colValues";
            this.colValues.ReadOnly = true;
            this.colValues.ToolTipText = "이 공정 READY 파일의 숫자 값 수";
            this.colValues.Width = 65;
            //
            // colNotTested
            //
            this.colNotTested.HeaderText = "'-' 수";
            this.colNotTested.Name = "colNotTested";
            this.colNotTested.ReadOnly = true;
            this.colNotTested.ToolTipText = "이 공정 READY 파일의 '-' 값 수 (Enable 이면 Fail)";
            this.colNotTested.Width = 55;
            //
            // colMin
            //
            this.colMin.HeaderText = "최소";
            this.colMin.Name = "colMin";
            this.colMin.ReadOnly = true;
            this.colMin.Width = 65;
            //
            // colMedian
            //
            this.colMedian.HeaderText = "중앙값";
            this.colMedian.Name = "colMedian";
            this.colMedian.ReadOnly = true;
            this.colMedian.Width = 65;
            //
            // colP99
            //
            this.colP99.HeaderText = "99%";
            this.colP99.Name = "colP99";
            this.colP99.ReadOnly = true;
            this.colP99.Width = 65;
            //
            // colMax
            //
            this.colMax.HeaderText = "최대";
            this.colMax.Name = "colMax";
            this.colMax.ReadOnly = true;
            this.colMax.Width = 65;
            //
            // colLower
            //
            this.colLower.HeaderText = "Lower";
            this.colLower.Name = "colLower";
            this.colLower.Width = 70;
            //
            // colUpper
            //
            this.colUpper.HeaderText = "Upper";
            this.colUpper.Name = "colUpper";
            this.colUpper.Width = 70;
            //
            // colEnable
            //
            this.colEnable.HeaderText = "Enable";
            this.colEnable.Name = "colEnable";
            this.colEnable.Width = 50;
            //
            // colFail
            //
            this.colFail.HeaderText = "Fail";
            this.colFail.Name = "colFail";
            this.colFail.ReadOnly = true;
            this.colFail.ToolTipText = "Fail 값 수 = Enable Spec 범위 밖 + '-' 값 (이 공정 READY 파일 전체)";
            this.colFail.Width = 55;
            //
            // colSpec
            //
            this.colSpec.HeaderText = "Spec 원문";
            this.colSpec.Name = "colSpec";
            this.colSpec.ReadOnly = true;
            this.colSpec.Width = 220;
            //
            // colRemark
            //
            this.colRemark.HeaderText = "비고";
            this.colRemark.MaxInputLength = 200;
            this.colRemark.Name = "colRemark";
            this.colRemark.Width = 140;
            //
            // colUpdate
            //
            this.colUpdate.HeaderText = "수정";
            this.colUpdate.Name = "colUpdate";
            this.colUpdate.ReadOnly = true;
            this.colUpdate.Width = 160;
            //
            // colChainName
            //
            this.colChainName.HeaderText = "E-Test Chain";
            this.colChainName.Name = "colChainName";
            this.colChainName.ReadOnly = true;
            this.colChainName.Width = 240;
            //
            // colChainItems
            //
            this.colChainItems.HeaderText = "항목 수";
            this.colChainItems.Name = "colChainItems";
            this.colChainItems.ReadOnly = true;
            this.colChainItems.ToolTipText = "이 Chain 이 들어간 측정 항목 수 (Device 전체)";
            this.colChainItems.Width = 60;
            //
            // colChainGroup
            //
            this.colChainGroup.HeaderText = "묶음 Chain";
            this.colChainGroup.Name = "colChainGroup";
            this.colChainGroup.ReadOnly = true;
            this.colChainGroup.ToolTipText = "같은 묶음의 Chain 전부";
            this.colChainGroup.Width = 330;
            //
            // colChainGroupCount
            //
            this.colChainGroupCount.HeaderText = "묶음 수";
            this.colChainGroupCount.Name = "colChainGroupCount";
            this.colChainGroupCount.ReadOnly = true;
            this.colChainGroupCount.ToolTipText = "같은 묶음의 Chain 수";
            this.colChainGroupCount.Width = 60;
            //
            // colChainSetup
            //
            this.colChainSetup.HeaderText = "Setup Chain";
            this.colChainSetup.Name = "colChainSetup";
            this.colChainSetup.ReadOnly = true;
            this.colChainSetup.ToolTipText = "이 Chain(과 묶인 Chain)에 연결되는 GDS Setup Chain (같은 이름 또는 이름 + _FH / _FL)";
            this.colChainSetup.Width = 160;
            //
            // colChainRemark
            //
            this.colChainRemark.HeaderText = "비고";
            this.colChainRemark.Name = "colChainRemark";
            this.colChainRemark.ReadOnly = true;
            this.colChainRemark.Width = 130;
            //
            // colChainUpdate
            //
            this.colChainUpdate.HeaderText = "수정";
            this.colChainUpdate.Name = "colChainUpdate";
            this.colChainUpdate.ReadOnly = true;
            this.colChainUpdate.Width = 160;
            //
            // colLkProgram
            //
            this.colLkProgram.HeaderText = "공정";
            this.colLkProgram.Name = "colLkProgram";
            this.colLkProgram.ReadOnly = true;
            this.colLkProgram.ToolTipText = "빈 값 = Spec 없는 측정 항목 (통계는 모든 공정 파일)";
            this.colLkProgram.Width = 60;
            //
            // colLkItem
            //
            this.colLkItem.HeaderText = "Parameter";
            this.colLkItem.Name = "colLkItem";
            this.colLkItem.ReadOnly = true;
            this.colLkItem.ToolTipText = "Spec 이 있는 행을 더블클릭하면 [Spec] 탭의 그 공정 / Parameter 로 이동";
            this.colLkItem.Width = 320;
            //
            // colLkMatch
            //
            this.colLkMatch.HeaderText = "찾은 Chain";
            this.colLkMatch.Name = "colLkMatch";
            this.colLkMatch.ReadOnly = true;
            this.colLkMatch.ToolTipText = "이 행을 찾게 한 E-Test Chain";
            this.colLkMatch.Width = 160;
            //
            // colLkDefined
            //
            this.colLkDefined.HeaderText = "정의";
            this.colLkDefined.Name = "colLkDefined";
            this.colLkDefined.ReadOnly = true;
            this.colLkDefined.ToolTipText = "Y = Spec 있음 (판정 대상)";
            this.colLkDefined.Width = 40;
            //
            // colLkLower
            //
            this.colLkLower.HeaderText = "Lower";
            this.colLkLower.Name = "colLkLower";
            this.colLkLower.ReadOnly = true;
            this.colLkLower.Width = 60;
            //
            // colLkUpper
            //
            this.colLkUpper.HeaderText = "Upper";
            this.colLkUpper.Name = "colLkUpper";
            this.colLkUpper.ReadOnly = true;
            this.colLkUpper.Width = 60;
            //
            // colLkUnit
            //
            this.colLkUnit.HeaderText = "단위";
            this.colLkUnit.Name = "colLkUnit";
            this.colLkUnit.ReadOnly = true;
            this.colLkUnit.Width = 45;
            //
            // colLkEnable
            //
            this.colLkEnable.HeaderText = "Enable";
            this.colLkEnable.Name = "colLkEnable";
            this.colLkEnable.ReadOnly = true;
            this.colLkEnable.Width = 50;
            //
            // colLkFail
            //
            this.colLkFail.HeaderText = "Fail";
            this.colLkFail.Name = "colLkFail";
            this.colLkFail.ReadOnly = true;
            this.colLkFail.ToolTipText = "Enable Spec 범위 밖 + '-' 값 (그 공정 READY 파일)";
            this.colLkFail.Width = 50;
            //
            // colLkFiles
            //
            this.colLkFiles.HeaderText = "파일";
            this.colLkFiles.Name = "colLkFiles";
            this.colLkFiles.ReadOnly = true;
            this.colLkFiles.ToolTipText = "READY 파일 수";
            this.colLkFiles.Width = 45;
            //
            // colLkValues
            //
            this.colLkValues.HeaderText = "값 수";
            this.colLkValues.Name = "colLkValues";
            this.colLkValues.ReadOnly = true;
            this.colLkValues.Width = 60;
            //
            // colLkNotTested
            //
            this.colLkNotTested.HeaderText = "'-' 수";
            this.colLkNotTested.Name = "colLkNotTested";
            this.colLkNotTested.ReadOnly = true;
            this.colLkNotTested.Width = 50;
            //
            // colLkMin
            //
            this.colLkMin.HeaderText = "최소";
            this.colLkMin.Name = "colLkMin";
            this.colLkMin.ReadOnly = true;
            this.colLkMin.Width = 65;
            //
            // colLkMax
            //
            this.colLkMax.HeaderText = "최대";
            this.colLkMax.Name = "colLkMax";
            this.colLkMax.ReadOnly = true;
            this.colLkMax.Width = 65;
            //
            // colLkSpec
            //
            this.colLkSpec.HeaderText = "Spec 원문";
            this.colLkSpec.Name = "colLkSpec";
            this.colLkSpec.ReadOnly = true;
            this.colLkSpec.Width = 220;
            //
            // frmEtLimitSetup
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 700);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.topBar);
            this.Name = "frmEtLimitSetup";
            this.Text = "E-Test Spec 설정";
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.tabMain.ResumeLayout(false);
            this.tabSpec.ResumeLayout(false);
            this.tabChain.ResumeLayout(false);
            this.chainBar.ResumeLayout(false);
            this.chainBar.PerformLayout();
            this.tabLookup.ResumeLayout(false);
            this.lookupLeft.ResumeLayout(false);
            this.lookupLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLookup)).EndInit();
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
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.Label lblGuide;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabSpec;
        private System.Windows.Forms.TabPage tabChain;
        private System.Windows.Forms.TabPage tabLookup;
        private System.Windows.Forms.DataGridView dgvLimit;
        private System.Windows.Forms.Panel chainBar;
        private System.Windows.Forms.Label lblChainFilter;
        private System.Windows.Forms.TextBox txtChainFilter;
        private System.Windows.Forms.Button btnChainGroup;
        private System.Windows.Forms.Button btnChainAuto;
        private System.Windows.Forms.Button btnChainUngroup;
        private System.Windows.Forms.Label lblChainSelect;
        private System.Windows.Forms.Label lblChainInfo;
        private System.Windows.Forms.DataGridView dgvChain;
        private System.Windows.Forms.Panel lookupLeft;
        private System.Windows.Forms.Label lblSetupChain;
        private System.Windows.Forms.ComboBox cboSetupChain;
        private System.Windows.Forms.Button btnLookup;
        private System.Windows.Forms.ListBox lstSetupChain;
        private System.Windows.Forms.Label lblLookupInfo;
        private System.Windows.Forms.DataGridView dgvLookup;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSetup;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInData;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValues;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNotTested;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMin;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMedian;
        private System.Windows.Forms.DataGridViewTextBoxColumn colP99;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMax;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLower;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUpper;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colEnable;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRemark;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUpdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChainName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChainItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChainGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChainGroupCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChainSetup;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChainRemark;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChainUpdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLkProgram;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLkItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLkMatch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLkDefined;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLkLower;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLkUpper;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLkUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLkEnable;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLkFail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLkFiles;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLkValues;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLkNotTested;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLkMin;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLkMax;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLkSpec;
    }
}
