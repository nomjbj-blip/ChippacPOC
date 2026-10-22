namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// GDS × KLARF 연관 분석 화면 배치.
    /// 설계: 문서/설계/05_연관분석/2026-10-12_GDS_KLARF_연관분석_화면_설계.md 4.2절,
    ///       문서/설계/05_연관분석/2026-10-15_GDS_KLARF_ETest_통합분석_데이터_설계.md 8.1 / 16.6절 (2026-10-16 Wafer 중심 Ready 배치)
    /// </summary>
    partial class frmGdsKlarfAnalysis
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>화면이 닫힐 때 컨트롤과 메뉴 / 이미지 목록, 색 구간 팝업 / 탭 글꼴을 함께 해제한다.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (disposing)
            {
                if (_bandPopup != null) _bandPopup.Dispose();
                if (_tabBoldFont != null) _tabBoldFont.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 배치: 문서/설계/05_연관분석/2026-10-19_화면A_레이아웃_후보.md 안 1 (2026-10-19 결정: 권장안, 색 구간 설정은 팝업)
        /// 위: 줄 1 = 분석 Wafer / GDS Device (읽기 전용: DMSearch 에서 Wafer 1장, Device 는 Wafer Product 를 따른다) + [분석]
        ///     줄 2 = Wafer Key + [다시 조회] ("지금 기준" 요약 줄은 2026-10-20 삭제)
        ///     분류 기준 · 분류 · Chain 거르기 · Die Index · 큰 Defect 제외 · 색 기준은 지금 기준(RoughBin 전체 / READY Chain 전체 / 끔 / Chain Defect 수)으로 두고 숨긴다 (2026-10-16)
        /// 왼쪽: Wafer Map (위 막대 [색 구간 설정] = 색 구간 표 팝업. Map 범례는 2026-10-20 삭제, 색 구간은 팝업에서만)
        /// 오른쪽: 기능 탭 [GDS] [KLARF] [E-Test] [종합 (Die)]. 탭마다 위에 그 기능의 기준(접기), 아래에 결과 하위 탭
        ///     GDS = GDS Map 표 / Chain 면 -> Chain, KLARF = 고른 Step 정보 + Pitch 출처(Step 표 / 검사 면 콤보는 2026-10-20 삭제: Wafer 1개만 분석, KLARF 검사 면 FS 고정) / Defect 영역 / 좌표 보정 -> Defect · 허용오차 · Image,
        ///     E-Test = E-Test 표 / Map 빨간 삼각형 기준 -> E-Test Fail · E-Test 연결, 종합 = Die 표
        /// 아래: 상태줄(분석 기준 / 마우스 Die 정보) + 진행 표시
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGdsKlarfAnalysis));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblWafer = new System.Windows.Forms.Label();
            this.lblWaferInfo = new System.Windows.Forms.Label();
            this.lblDevice = new System.Windows.Forms.Label();
            this.lblDeviceValue = new System.Windows.Forms.Label();
            this.lblClassBasis = new System.Windows.Forms.Label();
            this.cboClassBasis = new System.Windows.Forms.ComboBox();
            this.btnClass = new System.Windows.Forms.Button();
            this.btnChain = new System.Windows.Forms.Button();
            this.chkDieIndex = new System.Windows.Forms.CheckBox();
            this.rdoDefectSize = new System.Windows.Forms.RadioButton();
            this.numTolerance = new System.Windows.Forms.NumericUpDown();
            this.rdoSquare = new System.Windows.Forms.RadioButton();
            this.numSquare = new System.Windows.Forms.NumericUpDown();
            this.chkLargeDefect = new System.Windows.Forms.CheckBox();
            this.lblOffset = new System.Windows.Forms.Label();
            this.numOffsetX = new System.Windows.Forms.NumericUpDown();
            this.numOffsetY = new System.Windows.Forms.NumericUpDown();
            this.lblColorBasis = new System.Windows.Forms.Label();
            this.cboColorBasis = new System.Windows.Forms.ComboBox();
            this.btnAnalyze = new System.Windows.Forms.Button();
            this.flowSummary = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTolerance = new System.Windows.Forms.Label();
            this.menuClass = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuChain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.map = new DACrux.SEMDMS.ENGUI.GdsKlarfAnalysisMap();
            this.pnlMapHead = new System.Windows.Forms.Panel();
            this.lblMapTitle = new System.Windows.Forms.Label();
            this.lblMapBasis = new System.Windows.Forms.Label();
            this.btnBand = new System.Windows.Forms.Button();
            this.pnlBand = new System.Windows.Forms.Panel();
            this.dgvBand = new System.Windows.Forms.DataGridView();
            this.colBandFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBandTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBandColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBandDies = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlBandHead = new System.Windows.Forms.Panel();
            this.lblBandTitle = new System.Windows.Forms.Label();
            this.chkAutoBand = new System.Windows.Forms.CheckBox();
            this.lblThreshold = new System.Windows.Forms.Label();
            this.numThreshold = new System.Windows.Forms.NumericUpDown();
            this.lblFailMark = new System.Windows.Forms.Label();
            this.chkMarkSpec = new System.Windows.Forms.CheckBox();
            this.chkMarkFile = new System.Windows.Forms.CheckBox();
            this.tabList = new System.Windows.Forms.TabControl();
            this.tabGds = new System.Windows.Forms.TabPage();
            this.pnlGdsHead = new System.Windows.Forms.Panel();
            this.lblGdsCriteria = new System.Windows.Forms.Label();
            this.btnGdsFold = new System.Windows.Forms.Button();
            this.pnlGdsCriteria = new System.Windows.Forms.Panel();
            this.tabGdsResult = new System.Windows.Forms.TabControl();
            this.tabKlarf = new System.Windows.Forms.TabPage();
            this.pnlKlarfHead = new System.Windows.Forms.Panel();
            this.lblKlarfCriteria = new System.Windows.Forms.Label();
            this.btnKlarfFold = new System.Windows.Forms.Button();
            this.pnlKlarfCriteria = new System.Windows.Forms.Panel();
            this.pnlKlarfCond = new System.Windows.Forms.Panel();
            this.lblArea = new System.Windows.Forms.Label();
            this.tabKlarfResult = new System.Windows.Forms.TabControl();
            this.tabEt = new System.Windows.Forms.TabPage();
            this.pnlEtHead = new System.Windows.Forms.Panel();
            this.lblEtCriteria = new System.Windows.Forms.Label();
            this.btnEtFold = new System.Windows.Forms.Button();
            this.pnlEtCriteria = new System.Windows.Forms.Panel();
            this.pnlEtCond = new System.Windows.Forms.Panel();
            this.tabEtResult = new System.Windows.Forms.TabControl();
            this.lblWaferKey = new System.Windows.Forms.Label();
            this.btnReadyRefresh = new System.Windows.Forms.Button();
            this.pnlReadyGdsHead = new System.Windows.Forms.Panel();
            this.lblReadyGds = new System.Windows.Forms.Label();
            this.lblChainSide = new System.Windows.Forms.Label();
            this.cboChainSide = new System.Windows.Forms.ComboBox();
            this.dgvReadyGds = new System.Windows.Forms.DataGridView();
            this.pnlReadyStepHead = new System.Windows.Forms.Panel();
            this.lblReadyStep = new System.Windows.Forms.Label();
            this.pnlStepInfo = new System.Windows.Forms.Panel();
            this.lblStepInfo = new System.Windows.Forms.Label();
            this.lblPitchSource = new System.Windows.Forms.Label();
            this.pnlReadyEtHead = new System.Windows.Forms.Panel();
            this.lblReadyEt = new System.Windows.Forms.Label();
            this.btnFindEt = new System.Windows.Forms.Button();
            this.lblEtStep = new System.Windows.Forms.Label();
            this.cboEtStep = new System.Windows.Forms.ComboBox();
            this.dgvReadyEt = new System.Windows.Forms.DataGridView();
            this.colEtUse = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colEtProgram = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEtWafer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEtDevice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEtTestTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEtDie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEtFail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEtJudge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEtSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEtLink = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEtAdded = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEtSeq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabDie = new System.Windows.Forms.TabPage();
            this.dgvDie = new System.Windows.Forms.DataGridView();
            this.tabChain = new System.Windows.Forms.TabPage();
            this.dgvChain = new System.Windows.Forms.DataGridView();
            this.pnlChainHead = new System.Windows.Forms.Panel();
            this.chkChainAll = new System.Windows.Forms.CheckBox();
            this.tabDefect = new System.Windows.Forms.TabPage();
            this.dgvDefect = new System.Windows.Forms.DataGridView();
            this.pnlDefectHead = new System.Windows.Forms.Panel();
            this.cboDefectScope = new System.Windows.Forms.ComboBox();
            this.chkDefectAll = new System.Windows.Forms.CheckBox();
            this.tabTolerance = new System.Windows.Forms.TabPage();
            this.dgvTolerance = new System.Windows.Forms.DataGridView();
            this.tabEtLink = new System.Windows.Forms.TabPage();
            this.dgvEtLink = new System.Windows.Forms.DataGridView();
            this.tabEtFail = new System.Windows.Forms.TabPage();
            this.dgvEtFail = new System.Windows.Forms.DataGridView();
            this.tabImage = new System.Windows.Forms.TabPage();
            this.flowImage = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.busyBar = new DACrux.SEMDMS.Control.ProgressStatusBar();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.pnlTop.SuspendLayout();
            this.flowSummary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTolerance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSquare)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOffsetX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOffsetY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.pnlMapHead.SuspendLayout();
            this.pnlBand.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBand)).BeginInit();
            this.pnlBandHead.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numThreshold)).BeginInit();
            this.tabList.SuspendLayout();
            this.tabGds.SuspendLayout();
            this.pnlGdsHead.SuspendLayout();
            this.pnlGdsCriteria.SuspendLayout();
            this.tabGdsResult.SuspendLayout();
            this.tabKlarf.SuspendLayout();
            this.pnlKlarfHead.SuspendLayout();
            this.pnlKlarfCriteria.SuspendLayout();
            this.pnlKlarfCond.SuspendLayout();
            this.tabKlarfResult.SuspendLayout();
            this.tabEt.SuspendLayout();
            this.pnlEtHead.SuspendLayout();
            this.pnlEtCriteria.SuspendLayout();
            this.pnlEtCond.SuspendLayout();
            this.tabEtResult.SuspendLayout();
            this.pnlReadyGdsHead.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReadyGds)).BeginInit();
            this.pnlReadyStepHead.SuspendLayout();
            this.pnlStepInfo.SuspendLayout();
            this.pnlReadyEtHead.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReadyEt)).BeginInit();
            this.tabDie.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDie)).BeginInit();
            this.tabChain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChain)).BeginInit();
            this.pnlChainHead.SuspendLayout();
            this.tabDefect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDefect)).BeginInit();
            this.pnlDefectHead.SuspendLayout();
            this.tabTolerance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTolerance)).BeginInit();
            this.tabEtLink.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEtLink)).BeginInit();
            this.tabEtFail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEtFail)).BeginInit();
            this.tabImage.SuspendLayout();
            this.pnlStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(242)))), ((int)(((byte)(250)))));
            this.pnlTop.Controls.Add(this.flowSummary);
            this.pnlTop.Controls.Add(this.lblWafer);
            this.pnlTop.Controls.Add(this.lblWaferInfo);
            this.pnlTop.Controls.Add(this.lblDevice);
            this.pnlTop.Controls.Add(this.lblDeviceValue);
            this.pnlTop.Controls.Add(this.lblClassBasis);
            this.pnlTop.Controls.Add(this.cboClassBasis);
            this.pnlTop.Controls.Add(this.btnClass);
            this.pnlTop.Controls.Add(this.btnChain);
            this.pnlTop.Controls.Add(this.chkDieIndex);
            this.pnlTop.Controls.Add(this.chkLargeDefect);
            this.pnlTop.Controls.Add(this.lblColorBasis);
            this.pnlTop.Controls.Add(this.cboColorBasis);
            this.pnlTop.Controls.Add(this.btnAnalyze);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(2000, 117);
            this.pnlTop.TabIndex = 0;
            // 
            // lblWafer
            // 
            this.lblWafer.AutoSize = true;
            this.lblWafer.Location = new System.Drawing.Point(14, 21);
            this.lblWafer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWafer.Name = "lblWafer";
            this.lblWafer.Size = new System.Drawing.Size(96, 18);
            this.lblWafer.TabIndex = 0;
            this.lblWafer.Text = "분석 Wafer";
            // 
            // lblWaferInfo
            // 
            this.lblWaferInfo.AutoSize = true;
            this.lblWaferInfo.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblWaferInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(60)))), ((int)(((byte)(130)))));
            this.lblWaferInfo.Location = new System.Drawing.Point(109, 16);
            this.lblWaferInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWaferInfo.Name = "lblWaferInfo";
            this.lblWaferInfo.Size = new System.Drawing.Size(20, 28);
            this.lblWaferInfo.TabIndex = 1;
            this.lblWaferInfo.Text = "-";
            // 
            // lblDevice
            // 
            this.lblDevice.AutoSize = true;
            this.lblDevice.Location = new System.Drawing.Point(1086, 21);
            this.lblDevice.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(102, 18);
            this.lblDevice.TabIndex = 2;
            this.lblDevice.Text = "GDS Device";
            // 
            // lblDeviceValue
            // 
            this.lblDeviceValue.AutoSize = true;
            this.lblDeviceValue.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblDeviceValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(60)))), ((int)(((byte)(130)))));
            this.lblDeviceValue.Location = new System.Drawing.Point(1191, 16);
            this.lblDeviceValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDeviceValue.Name = "lblDeviceValue";
            this.lblDeviceValue.Size = new System.Drawing.Size(20, 28);
            this.lblDeviceValue.TabIndex = 3;
            this.lblDeviceValue.Text = "-";
            // 
            // lblClassBasis
            // 
            this.lblClassBasis.AutoSize = true;
            this.lblClassBasis.Location = new System.Drawing.Point(874, 21);
            this.lblClassBasis.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblClassBasis.Name = "lblClassBasis";
            this.lblClassBasis.Size = new System.Drawing.Size(86, 18);
            this.lblClassBasis.TabIndex = 4;
            this.lblClassBasis.Text = "분류 기준";
            this.lblClassBasis.Visible = false;
            // 
            // cboClassBasis
            // 
            this.cboClassBasis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboClassBasis.Items.AddRange(new object[] {
            "RoughBin",
            "Class"});
            this.cboClassBasis.Location = new System.Drawing.Point(957, 15);
            this.cboClassBasis.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboClassBasis.Name = "cboClassBasis";
            this.cboClassBasis.Size = new System.Drawing.Size(118, 26);
            this.cboClassBasis.TabIndex = 5;
            this.cboClassBasis.Visible = false;
            this.cboClassBasis.SelectedIndexChanged += new System.EventHandler(this.cboClassBasis_SelectedIndexChanged);
            // 
            // btnClass
            // 
            this.btnClass.BackColor = System.Drawing.Color.White;
            this.btnClass.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClass.Location = new System.Drawing.Point(1089, 9);
            this.btnClass.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClass.Name = "btnClass";
            this.btnClass.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClass.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClass.Image = ((System.Drawing.Image)(resources.GetObject("btnClass.Image")));
            this.btnClass.Size = new System.Drawing.Size(243, 42);
            this.btnClass.TabIndex = 6;
            this.btnClass.Text = "RoughBin: 전체 ▼";
            this.btnClass.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClass.UseVisualStyleBackColor = false;
            this.btnClass.Visible = false;
            this.btnClass.Click += new System.EventHandler(this.btnClass_Click);
            // 
            // btnChain
            // 
            this.btnChain.BackColor = System.Drawing.Color.White;
            this.btnChain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChain.Location = new System.Drawing.Point(1343, 9);
            this.btnChain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnChain.Name = "btnChain";
            this.btnChain.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnChain.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnChain.Image = ((System.Drawing.Image)(resources.GetObject("btnChain.Image")));
            this.btnChain.Size = new System.Drawing.Size(286, 42);
            this.btnChain.TabIndex = 7;
            this.btnChain.Text = "Chain: 전체 ▼";
            this.btnChain.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnChain.UseVisualStyleBackColor = false;
            this.btnChain.Visible = false;
            this.btnChain.Click += new System.EventHandler(this.btnChain_Click);
            // 
            // chkDieIndex
            // 
            this.chkDieIndex.AutoSize = true;
            this.chkDieIndex.Location = new System.Drawing.Point(1649, 18);
            this.chkDieIndex.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkDieIndex.Name = "chkDieIndex";
            this.chkDieIndex.Size = new System.Drawing.Size(149, 22);
            this.chkDieIndex.TabIndex = 8;
            this.chkDieIndex.Text = "Die Index 표시";
            this.chkDieIndex.UseVisualStyleBackColor = true;
            this.chkDieIndex.Visible = false;
            this.chkDieIndex.CheckedChanged += new System.EventHandler(this.chkDieIndex_CheckedChanged);
            // 
            // rdoDefectSize
            // 
            this.rdoDefectSize.AutoSize = true;
            this.rdoDefectSize.Checked = true;
            this.rdoDefectSize.Location = new System.Drawing.Point(118, 12);
            this.rdoDefectSize.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoDefectSize.Name = "rdoDefectSize";
            this.rdoDefectSize.Size = new System.Drawing.Size(219, 22);
            this.rdoDefectSize.TabIndex = 9;
            this.rdoDefectSize.TabStop = true;
            this.rdoDefectSize.Text = "Defect 크기 + 허용오차";
            this.rdoDefectSize.UseVisualStyleBackColor = true;
            this.rdoDefectSize.CheckedChanged += new System.EventHandler(this.DefectArea_CheckedChanged);
            // 
            // numTolerance
            // 
            this.numTolerance.DecimalPlaces = 1;
            this.numTolerance.Location = new System.Drawing.Point(341, 10);
            this.numTolerance.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numTolerance.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numTolerance.Name = "numTolerance";
            this.numTolerance.Size = new System.Drawing.Size(86, 28);
            this.numTolerance.TabIndex = 10;
            this.numTolerance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numTolerance.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numTolerance.ValueChanged += new System.EventHandler(this.Condition_Changed);
            // 
            // rdoSquare
            // 
            this.rdoSquare.AutoSize = true;
            this.rdoSquare.Location = new System.Drawing.Point(484, 12);
            this.rdoSquare.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoSquare.Name = "rdoSquare";
            this.rdoSquare.Size = new System.Drawing.Size(190, 22);
            this.rdoSquare.TabIndex = 12;
            this.rdoSquare.Text = "Defect 중심 사각형";
            this.rdoSquare.UseVisualStyleBackColor = true;
            this.rdoSquare.CheckedChanged += new System.EventHandler(this.DefectArea_CheckedChanged);
            // 
            // numSquare
            // 
            this.numSquare.DecimalPlaces = 1;
            this.numSquare.Location = new System.Drawing.Point(738, 10);
            this.numSquare.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numSquare.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numSquare.Name = "numSquare";
            this.numSquare.Size = new System.Drawing.Size(86, 28);
            this.numSquare.TabIndex = 13;
            this.numSquare.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numSquare.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numSquare.ValueChanged += new System.EventHandler(this.Condition_Changed);
            // 
            // chkLargeDefect
            // 
            this.chkLargeDefect.AutoSize = true;
            this.chkLargeDefect.Location = new System.Drawing.Point(1649, 70);
            this.chkLargeDefect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkLargeDefect.Name = "chkLargeDefect";
            this.chkLargeDefect.Size = new System.Drawing.Size(151, 22);
            this.chkLargeDefect.TabIndex = 15;
            this.chkLargeDefect.Text = "큰 Defect 제외";
            this.chkLargeDefect.UseVisualStyleBackColor = true;
            this.chkLargeDefect.Visible = false;
            this.chkLargeDefect.CheckedChanged += new System.EventHandler(this.Condition_Changed);
            // 
            // lblOffset
            // 
            this.lblOffset.AutoSize = true;
            this.lblOffset.Location = new System.Drawing.Point(8, 61);
            this.lblOffset.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOffset.Name = "lblOffset";
            this.lblOffset.Size = new System.Drawing.Size(172, 18);
            this.lblOffset.TabIndex = 16;
            this.lblOffset.Text = "좌표 보정 X / Y";
            // 
            // numOffsetX
            // 
            this.numOffsetX.DecimalPlaces = 1;
            this.numOffsetX.Location = new System.Drawing.Point(190, 55);
            this.numOffsetX.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numOffsetX.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numOffsetX.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.numOffsetX.Name = "numOffsetX";
            this.numOffsetX.Size = new System.Drawing.Size(100, 28);
            this.numOffsetX.TabIndex = 17;
            this.numOffsetX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numOffsetX.ValueChanged += new System.EventHandler(this.Condition_Changed);
            // 
            // numOffsetY
            // 
            this.numOffsetY.DecimalPlaces = 1;
            this.numOffsetY.Location = new System.Drawing.Point(296, 55);
            this.numOffsetY.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numOffsetY.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numOffsetY.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.numOffsetY.Name = "numOffsetY";
            this.numOffsetY.Size = new System.Drawing.Size(100, 28);
            this.numOffsetY.TabIndex = 18;
            this.numOffsetY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numOffsetY.ValueChanged += new System.EventHandler(this.Condition_Changed);
            // 
            // lblColorBasis
            // 
            this.lblColorBasis.AutoSize = true;
            this.lblColorBasis.Location = new System.Drawing.Point(1194, 74);
            this.lblColorBasis.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblColorBasis.Name = "lblColorBasis";
            this.lblColorBasis.Size = new System.Drawing.Size(68, 18);
            this.lblColorBasis.TabIndex = 19;
            this.lblColorBasis.Text = "색 기준";
            this.lblColorBasis.Visible = false;
            // 
            // cboColorBasis
            // 
            this.cboColorBasis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboColorBasis.Items.AddRange(new object[] {
            "Chain Defect 수",
            "E-Test Fail Chain 수",
            "일치 Chain 수 (Defect + Fail)",
            "선택 Chain 4분류"});
            this.cboColorBasis.Location = new System.Drawing.Point(1260, 68);
            this.cboColorBasis.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboColorBasis.Name = "cboColorBasis";
            this.cboColorBasis.Size = new System.Drawing.Size(270, 26);
            this.cboColorBasis.TabIndex = 20;
            this.cboColorBasis.Visible = false;
            this.cboColorBasis.SelectedIndexChanged += new System.EventHandler(this.cboColorBasis_SelectedIndexChanged);
            // 
            // btnAnalyze
            // 
            this.btnAnalyze.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAnalyze.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.btnAnalyze.Image = ((System.Drawing.Image)(resources.GetObject("btnAnalyze.Image")));
            this.btnAnalyze.Location = new System.Drawing.Point(1860, 9);
            this.btnAnalyze.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Size = new System.Drawing.Size(126, 42);
            this.btnAnalyze.TabIndex = 21;
            this.btnAnalyze.Text = "분석";
            this.btnAnalyze.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAnalyze.UseVisualStyleBackColor = true;
            this.btnAnalyze.Click += new System.EventHandler(this.btnAnalyze_Click);
            // 
            // flowSummary
            // 
            this.flowSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowSummary.Controls.Add(this.lblWaferKey);
            this.flowSummary.Controls.Add(this.btnReadyRefresh);
            this.flowSummary.Location = new System.Drawing.Point(6, 60);
            this.flowSummary.Margin = new System.Windows.Forms.Padding(0);
            this.flowSummary.Name = "flowSummary";
            this.flowSummary.Size = new System.Drawing.Size(1988, 54);
            this.flowSummary.TabIndex = 22;
            this.flowSummary.WrapContents = false;
            // 
            // lblTolerance
            // 
            this.lblTolerance.Location = new System.Drawing.Point(0, 0);
            this.lblTolerance.Name = "lblTolerance";
            this.lblTolerance.Size = new System.Drawing.Size(100, 23);
            this.lblTolerance.TabIndex = 0;
            // 
            // menuClass
            // 
            this.menuClass.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuClass.Name = "menuClass";
            this.menuClass.Size = new System.Drawing.Size(61, 4);
            this.menuClass.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.CheckMenu_Closing);
            // 
            // menuChain
            // 
            this.menuChain.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuChain.Name = "menuChain";
            this.menuChain.Size = new System.Drawing.Size(61, 4);
            this.menuChain.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.CheckMenu_Closing);
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 117);
            this.splitMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.map);
            this.splitMain.Panel1.Controls.Add(this.pnlMapHead);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.tabList);
            this.splitMain.Size = new System.Drawing.Size(2000, 1164);
            this.splitMain.SplitterDistance = 1085;
            this.splitMain.SplitterWidth = 6;
            this.splitMain.TabIndex = 1;
            // 
            // map
            // 
            this.map.AngleOffSet = 0;
            this.map.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.map.CenterMark = false;
            this.map.Cursor = System.Windows.Forms.Cursors.Cross;
            this.map.DataSource = null;
            this.map.DefectSize = 7F;
            this.map.DieBackgroundColor = System.Drawing.Color.Black;
            this.map.DieBorderColor = System.Drawing.Color.LightGray;
            this.map.DieDefectColor = System.Drawing.Color.Empty;
            this.map.DieFocusingType = DACrux.Map.FocusType.Arraw;
            this.map.DieMaxX = -65536;
            this.map.DieMaxY = -65536;
            this.map.DieMinX = 65536;
            this.map.DieMinY = 65536;
            this.map.DieSizeX = 0.01D;
            this.map.DieSizeY = 0.01D;
            this.map.DisplayDieValue = DACrux.Base.DieDisplayValue.Bin;
            this.map.DisplayValue = "BIN";
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.DrawDefectImage = null;
            this.map.DrawDefects = "ALL";
            this.map.DrawFirstDie = true;
            this.map.DrawMarkDie = true;
            this.map.DrawOriginDie = true;
            this.map.DrawSkipDie = true;
            this.map.EdgeColor = System.Drawing.Color.LightGray;
            this.map.EdgeSize = 1D;
            this.map.FirstDieBorderColor = System.Drawing.Color.SkyBlue;
            this.map.FirstDieX = 0;
            this.map.FirstDieY = 0;
            this.map.ForeColor = System.Drawing.Color.Red;
            this.map.FromGradationDieColor = System.Drawing.Color.Lime;
            this.map.GradationInterval = 5;
            this.map.GradationMaxValue = double.NaN;
            this.map.GradationMinValue = double.NaN;
            this.map.IndexOffset = new System.Drawing.Point(0, 0);
            this.map.Location = new System.Drawing.Point(0, 54);
            this.map.MapType = DACrux.Base.MAP_TYPE.CLASS;
            this.map.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.map.MarkDieColor = System.Drawing.Color.LightSkyBlue;
            this.map.Name = "map";
            this.map.NotchAngle = 0;
            this.map.NotchType = DACrux.Base.Notch.Flat;
            this.map.OriginDieBorder = System.Drawing.Color.Red;
            this.map.OriginIndexX = 0;
            this.map.OriginIndexY = 0;
            this.map.OriginX = 0D;
            this.map.OriginY = 0D;
            this.map.ParaLimit = false;
            this.map.ParametricColumn = "PCMVALUE";
            this.map.ParaValueFont = new System.Drawing.Font("굴림", 9F);
            this.map.PickupDieAlpha = 96;
            this.map.PickupedDieColor = System.Drawing.Color.Transparent;
            this.map.PopupMenu = true;
            this.map.ReferenceDieSetting = 0;
            this.map.ScaleMark = false;
            this.map.SelecetedBin = "ALL";
            this.map.SelectedVI = "ALL";
            this.map.ShowDieCount = true;
            this.map.ShowDieIndexLabel = false;
            this.map.Size = new System.Drawing.Size(1085, 1110);
            this.map.SizeColor = null;
            this.map.SkipDieColor = System.Drawing.Color.Yellow;
            this.map.TabIndex = 0;
            this.map.ToGradationDieColor = System.Drawing.Color.Red;
            this.map.TransParent = 255;
            this.map.TypeColor = null;
            this.map.ViewAngle = 0;
            this.map.VIMember = "VIFAIL";
            this.map.VisibleDieBorder = true;
            this.map.VisibleDieValue = false;
            this.map.VisibleFocusDie = false;
            this.map.VisibleImageMark = false;
            this.map.VisibleInfomation = true;
            this.map.VisibleOffDie = false;
            this.map.VisibleProbeOverlay = false;
            this.map.VisibleShotAlignPoint = false;
            this.map.VisibleSignDies = false;
            this.map.VisibleStringBin = false;
            this.map.VisibleVIFail = false;
            this.map.VisibleXY = false;
            this.map.WaferBorderColor = System.Drawing.Color.LightGray;
            this.map.WaferColor = System.Drawing.Color.Gray;
            this.map.WaferDrawMode = DACrux.Map.MapMode.Free;
            this.map.WaferID = "";
            this.map.WaferMargin = 0.95D;
            this.map.WaferSize = 200D;
            this.map.XYDirect = DACrux.Base.XYDirection.LeftTop;
            this.map.DieClicked += new System.EventHandler<DACrux.SEMDMS.ENGUI.GdsKlarfDieEventArgs>(this.map_DieClicked);
            this.map.DieDoubleClicked += new System.EventHandler<DACrux.SEMDMS.ENGUI.GdsKlarfDieEventArgs>(this.map_DieDoubleClicked);
            this.map.DetailAnalysisRequested += new System.EventHandler<DACrux.SEMDMS.ENGUI.GdsKlarfDieEventArgs>(this.map_DetailAnalysisRequested);
            // 
            // pnlMapHead
            // 
            this.pnlMapHead.Controls.Add(this.lblMapTitle);
            this.pnlMapHead.Controls.Add(this.lblMapBasis);
            this.pnlMapHead.Controls.Add(this.btnBand);
            this.pnlMapHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMapHead.Location = new System.Drawing.Point(0, 0);
            this.pnlMapHead.Margin = new System.Windows.Forms.Padding(0);
            this.pnlMapHead.Name = "pnlMapHead";
            this.pnlMapHead.Size = new System.Drawing.Size(1085, 54);
            this.pnlMapHead.TabIndex = 1;
            // 
            // lblMapTitle
            // 
            this.lblMapTitle.AutoSize = true;
            this.lblMapTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblMapTitle.Location = new System.Drawing.Point(8, 15);
            this.lblMapTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMapTitle.Name = "lblMapTitle";
            this.lblMapTitle.Size = new System.Drawing.Size(103, 25);
            this.lblMapTitle.TabIndex = 0;
            this.lblMapTitle.Text = "Wafer Map";
            // 
            // lblMapBasis
            // 
            this.lblMapBasis.AutoSize = true;
            this.lblMapBasis.ForeColor = System.Drawing.Color.DimGray;
            this.lblMapBasis.Location = new System.Drawing.Point(130, 19);
            this.lblMapBasis.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMapBasis.Name = "lblMapBasis";
            this.lblMapBasis.Size = new System.Drawing.Size(170, 18);
            this.lblMapBasis.TabIndex = 1;
            this.lblMapBasis.Text = "색 = Chain Defect 수";
            // 
            // btnBand
            // 
            this.btnBand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBand.Image = ((System.Drawing.Image)(resources.GetObject("btnBand.Image")));
            this.btnBand.Location = new System.Drawing.Point(896, 6);
            this.btnBand.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBand.Name = "btnBand";
            this.btnBand.Size = new System.Drawing.Size(180, 42);
            this.btnBand.TabIndex = 2;
            this.btnBand.Text = "색 구간 설정";
            this.btnBand.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBand.UseVisualStyleBackColor = true;
            this.btnBand.Click += new System.EventHandler(this.btnBand_Click);
            // 
            // pnlBand
            // 
            this.pnlBand.Controls.Add(this.dgvBand);
            this.pnlBand.Controls.Add(this.pnlBandHead);
            this.pnlBand.Location = new System.Drawing.Point(0, 0);
            this.pnlBand.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBand.Name = "pnlBand";
            this.pnlBand.Size = new System.Drawing.Size(640, 420);
            this.pnlBand.TabIndex = 91;
            this.pnlBand.Visible = false;
            // 
            // dgvBand
            // 
            this.dgvBand.AllowUserToAddRows = false;
            this.dgvBand.AllowUserToDeleteRows = false;
            this.dgvBand.AllowUserToResizeRows = false;
            this.dgvBand.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBand.BackgroundColor = System.Drawing.Color.White;
            this.dgvBand.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBand.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBandFrom,
            this.colBandTo,
            this.colBandColor,
            this.colBandDies});
            this.dgvBand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBand.Location = new System.Drawing.Point(0, 54);
            this.dgvBand.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvBand.MultiSelect = false;
            this.dgvBand.Name = "dgvBand";
            this.dgvBand.RowHeadersVisible = false;
            this.dgvBand.RowHeadersWidth = 62;
            this.dgvBand.RowTemplate.Height = 23;
            this.dgvBand.Size = new System.Drawing.Size(640, 366);
            this.dgvBand.TabIndex = 1;
            this.dgvBand.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBand_CellDoubleClick);
            this.dgvBand.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBand_CellEndEdit);
            // 
            // colBandFrom
            // 
            this.colBandFrom.HeaderText = "FROM";
            this.colBandFrom.MinimumWidth = 8;
            this.colBandFrom.Name = "colBandFrom";
            // 
            // colBandTo
            // 
            this.colBandTo.HeaderText = "TO";
            this.colBandTo.MinimumWidth = 8;
            this.colBandTo.Name = "colBandTo";
            // 
            // colBandColor
            // 
            this.colBandColor.HeaderText = "색";
            this.colBandColor.MinimumWidth = 8;
            this.colBandColor.Name = "colBandColor";
            this.colBandColor.ReadOnly = true;
            // 
            // colBandDies
            // 
            this.colBandDies.HeaderText = "Die 수";
            this.colBandDies.MinimumWidth = 8;
            this.colBandDies.Name = "colBandDies";
            this.colBandDies.ReadOnly = true;
            // 
            // pnlBandHead
            // 
            this.pnlBandHead.Controls.Add(this.lblBandTitle);
            this.pnlBandHead.Controls.Add(this.chkAutoBand);
            this.pnlBandHead.Controls.Add(this.lblThreshold);
            this.pnlBandHead.Controls.Add(this.numThreshold);
            this.pnlBandHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBandHead.Location = new System.Drawing.Point(0, 0);
            this.pnlBandHead.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlBandHead.Name = "pnlBandHead";
            this.pnlBandHead.Size = new System.Drawing.Size(640, 54);
            this.pnlBandHead.TabIndex = 0;
            // 
            // lblBandTitle
            // 
            this.lblBandTitle.AutoSize = true;
            this.lblBandTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblBandTitle.Location = new System.Drawing.Point(6, 12);
            this.lblBandTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBandTitle.Name = "lblBandTitle";
            this.lblBandTitle.Size = new System.Drawing.Size(72, 25);
            this.lblBandTitle.TabIndex = 0;
            this.lblBandTitle.Text = "색 구간";
            // 
            // chkAutoBand
            // 
            this.chkAutoBand.AutoSize = true;
            this.chkAutoBand.Checked = true;
            this.chkAutoBand.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoBand.Location = new System.Drawing.Point(300, 14);
            this.chkAutoBand.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkAutoBand.Name = "chkAutoBand";
            this.chkAutoBand.Size = new System.Drawing.Size(112, 22);
            this.chkAutoBand.TabIndex = 1;
            this.chkAutoBand.Text = "자동 구간";
            this.chkAutoBand.UseVisualStyleBackColor = true;
            this.chkAutoBand.CheckedChanged += new System.EventHandler(this.chkAutoBand_CheckedChanged);
            // 
            // lblThreshold
            // 
            this.lblThreshold.AutoSize = true;
            this.lblThreshold.Location = new System.Drawing.Point(430, 18);
            this.lblThreshold.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblThreshold.Name = "lblThreshold";
            this.lblThreshold.Size = new System.Drawing.Size(86, 18);
            this.lblThreshold.TabIndex = 2;
            this.lblThreshold.Text = "표시 기준";
            // 
            // numThreshold
            // 
            this.numThreshold.Location = new System.Drawing.Point(516, 12);
            this.numThreshold.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numThreshold.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numThreshold.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numThreshold.Name = "numThreshold";
            this.numThreshold.Size = new System.Drawing.Size(80, 28);
            this.numThreshold.TabIndex = 3;
            this.numThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numThreshold.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numThreshold.ValueChanged += new System.EventHandler(this.numThreshold_ValueChanged);
            //
            // lblFailMark
            //
            this.lblFailMark.AutoSize = true;
            this.lblFailMark.Location = new System.Drawing.Point(8, 15);
            this.lblFailMark.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFailMark.Name = "lblFailMark";
            this.lblFailMark.Size = new System.Drawing.Size(144, 18);
            this.lblFailMark.TabIndex = 4;
            this.lblFailMark.Text = "Map E-Test 표식";
            //
            // chkMarkSpec
            //
            this.chkMarkSpec.AutoSize = true;
            this.chkMarkSpec.Checked = true;
            this.chkMarkSpec.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMarkSpec.Location = new System.Drawing.Point(170, 12);
            this.chkMarkSpec.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkMarkSpec.Name = "chkMarkSpec";
            this.chkMarkSpec.Size = new System.Drawing.Size(120, 22);
            this.chkMarkSpec.TabIndex = 5;
            this.chkMarkSpec.Text = "Spec Out";
            this.chkMarkSpec.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.chkMarkSpec.UseVisualStyleBackColor = true;
            this.chkMarkSpec.CheckedChanged += new System.EventHandler(this.chkEtMark_CheckedChanged);
            //
            // chkMarkFile
            //
            this.chkMarkFile.AutoSize = true;
            this.chkMarkFile.Checked = true;
            this.chkMarkFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMarkFile.Location = new System.Drawing.Point(316, 12);
            this.chkMarkFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkMarkFile.Name = "chkMarkFile";
            this.chkMarkFile.Size = new System.Drawing.Size(190, 22);
            this.chkMarkFile.TabIndex = 6;
            this.chkMarkFile.Text = "Fail Die";
            this.chkMarkFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.chkMarkFile.UseVisualStyleBackColor = true;
            this.chkMarkFile.CheckedChanged += new System.EventHandler(this.chkEtMark_CheckedChanged);
            //
            // tabList
            // 
            this.tabList.Controls.Add(this.tabGds);
            this.tabList.Controls.Add(this.tabKlarf);
            this.tabList.Controls.Add(this.tabEt);
            this.tabList.Controls.Add(this.tabDie);
            this.tabList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabList.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabList.ItemSize = new System.Drawing.Size(150, 34);
            this.tabList.Location = new System.Drawing.Point(0, 0);
            this.tabList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabList.Name = "tabList";
            this.tabList.SelectedIndex = 1;
            this.tabList.Size = new System.Drawing.Size(909, 1164);
            this.tabList.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabList.TabIndex = 0;
            this.tabList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabList_DrawItem);
            this.tabList.SelectedIndexChanged += new System.EventHandler(this.tabList_SelectedIndexChanged);
            // 
            // tabGds
            // 
            this.tabGds.Controls.Add(this.tabGdsResult);
            this.tabGds.Controls.Add(this.pnlGdsCriteria);
            this.tabGds.Controls.Add(this.pnlGdsHead);
            this.tabGds.Location = new System.Drawing.Point(4, 38);
            this.tabGds.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabGds.Name = "tabGds";
            this.tabGds.Size = new System.Drawing.Size(901, 1122);
            this.tabGds.TabIndex = 0;
            this.tabGds.Text = "GDS";
            this.tabGds.UseVisualStyleBackColor = true;
            // 
            // pnlGdsHead
            // 
            this.pnlGdsHead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(247)))), ((int)(((byte)(252)))));
            this.pnlGdsHead.Controls.Add(this.lblGdsCriteria);
            this.pnlGdsHead.Controls.Add(this.btnGdsFold);
            this.pnlGdsHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlGdsHead.Location = new System.Drawing.Point(0, 0);
            this.pnlGdsHead.Margin = new System.Windows.Forms.Padding(0);
            this.pnlGdsHead.Name = "pnlGdsHead";
            this.pnlGdsHead.Size = new System.Drawing.Size(901, 54);
            this.pnlGdsHead.TabIndex = 0;
            // 
            // lblGdsCriteria
            // 
            this.lblGdsCriteria.AutoSize = true;
            this.lblGdsCriteria.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblGdsCriteria.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(87)))), ((int)(((byte)(154)))));
            this.lblGdsCriteria.Location = new System.Drawing.Point(6, 15);
            this.lblGdsCriteria.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGdsCriteria.Name = "lblGdsCriteria";
            this.lblGdsCriteria.Size = new System.Drawing.Size(110, 25);
            this.lblGdsCriteria.TabIndex = 0;
            this.lblGdsCriteria.Text = "GDS 기준";
            // 
            // btnGdsFold
            // 
            this.btnGdsFold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGdsFold.Image = ((System.Drawing.Image)(resources.GetObject("btnGdsFold.Image")));
            this.btnGdsFold.Location = new System.Drawing.Point(758, 6);
            this.btnGdsFold.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnGdsFold.Name = "btnGdsFold";
            this.btnGdsFold.Size = new System.Drawing.Size(137, 42);
            this.btnGdsFold.TabIndex = 1;
            this.btnGdsFold.Text = "기준 접기";
            this.btnGdsFold.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGdsFold.UseVisualStyleBackColor = true;
            this.btnGdsFold.Click += new System.EventHandler(this.CriteriaFold_Click);
            // 
            // pnlGdsCriteria
            // 
            this.pnlGdsCriteria.Controls.Add(this.dgvReadyGds);
            this.pnlGdsCriteria.Controls.Add(this.pnlReadyGdsHead);
            this.pnlGdsCriteria.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlGdsCriteria.Location = new System.Drawing.Point(0, 54);
            this.pnlGdsCriteria.Margin = new System.Windows.Forms.Padding(0);
            this.pnlGdsCriteria.Name = "pnlGdsCriteria";
            this.pnlGdsCriteria.Size = new System.Drawing.Size(901, 140);
            this.pnlGdsCriteria.TabIndex = 1;
            // 
            // tabGdsResult
            // 
            this.tabGdsResult.Controls.Add(this.tabChain);
            this.tabGdsResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabGdsResult.Location = new System.Drawing.Point(0, 0);
            this.tabGdsResult.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabGdsResult.Name = "tabGdsResult";
            this.tabGdsResult.SelectedIndex = 0;
            this.tabGdsResult.Size = new System.Drawing.Size(901, 700);
            this.tabGdsResult.TabIndex = 2;
            // 
            // tabKlarf
            // 
            this.tabKlarf.Controls.Add(this.tabKlarfResult);
            this.tabKlarf.Controls.Add(this.pnlKlarfCriteria);
            this.tabKlarf.Controls.Add(this.pnlKlarfHead);
            this.tabKlarf.Location = new System.Drawing.Point(4, 38);
            this.tabKlarf.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabKlarf.Name = "tabKlarf";
            this.tabKlarf.Size = new System.Drawing.Size(901, 1122);
            this.tabKlarf.TabIndex = 1;
            this.tabKlarf.Text = "KLARF";
            this.tabKlarf.UseVisualStyleBackColor = true;
            // 
            // pnlKlarfHead
            // 
            this.pnlKlarfHead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(247)))), ((int)(((byte)(252)))));
            this.pnlKlarfHead.Controls.Add(this.lblKlarfCriteria);
            this.pnlKlarfHead.Controls.Add(this.btnKlarfFold);
            this.pnlKlarfHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlKlarfHead.Location = new System.Drawing.Point(0, 0);
            this.pnlKlarfHead.Margin = new System.Windows.Forms.Padding(0);
            this.pnlKlarfHead.Name = "pnlKlarfHead";
            this.pnlKlarfHead.Size = new System.Drawing.Size(901, 54);
            this.pnlKlarfHead.TabIndex = 0;
            // 
            // lblKlarfCriteria
            // 
            this.lblKlarfCriteria.AutoSize = true;
            this.lblKlarfCriteria.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblKlarfCriteria.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.lblKlarfCriteria.Location = new System.Drawing.Point(6, 15);
            this.lblKlarfCriteria.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblKlarfCriteria.Name = "lblKlarfCriteria";
            this.lblKlarfCriteria.Size = new System.Drawing.Size(110, 25);
            this.lblKlarfCriteria.TabIndex = 0;
            this.lblKlarfCriteria.Text = "KLARF 기준";
            // 
            // btnKlarfFold
            // 
            this.btnKlarfFold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKlarfFold.Image = ((System.Drawing.Image)(resources.GetObject("btnKlarfFold.Image")));
            this.btnKlarfFold.Location = new System.Drawing.Point(758, 6);
            this.btnKlarfFold.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnKlarfFold.Name = "btnKlarfFold";
            this.btnKlarfFold.Size = new System.Drawing.Size(137, 42);
            this.btnKlarfFold.TabIndex = 1;
            this.btnKlarfFold.Text = "기준 접기";
            this.btnKlarfFold.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnKlarfFold.UseVisualStyleBackColor = true;
            this.btnKlarfFold.Click += new System.EventHandler(this.CriteriaFold_Click);
            // 
            // pnlKlarfCriteria
            // 
            this.pnlKlarfCriteria.Controls.Add(this.pnlStepInfo);
            this.pnlKlarfCriteria.Controls.Add(this.pnlKlarfCond);
            this.pnlKlarfCriteria.Controls.Add(this.pnlReadyStepHead);
            this.pnlKlarfCriteria.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlKlarfCriteria.Location = new System.Drawing.Point(0, 54);
            this.pnlKlarfCriteria.Margin = new System.Windows.Forms.Padding(0);
            this.pnlKlarfCriteria.Name = "pnlKlarfCriteria";
            this.pnlKlarfCriteria.Size = new System.Drawing.Size(901, 242);
            this.pnlKlarfCriteria.TabIndex = 1;
            // 
            // pnlKlarfCond
            // 
            this.pnlKlarfCond.Controls.Add(this.lblArea);
            this.pnlKlarfCond.Controls.Add(this.rdoDefectSize);
            this.pnlKlarfCond.Controls.Add(this.numTolerance);
            this.pnlKlarfCond.Controls.Add(this.rdoSquare);
            this.pnlKlarfCond.Controls.Add(this.numSquare);
            this.pnlKlarfCond.Controls.Add(this.lblOffset);
            this.pnlKlarfCond.Controls.Add(this.numOffsetX);
            this.pnlKlarfCond.Controls.Add(this.numOffsetY);
            this.pnlKlarfCond.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlKlarfCond.Location = new System.Drawing.Point(0, 54);
            this.pnlKlarfCond.Margin = new System.Windows.Forms.Padding(0);
            this.pnlKlarfCond.Name = "pnlKlarfCond";
            this.pnlKlarfCond.Size = new System.Drawing.Size(901, 100);
            this.pnlKlarfCond.TabIndex = 2;
            // 
            // lblArea
            // 
            this.lblArea.AutoSize = true;
            this.lblArea.Location = new System.Drawing.Point(8, 16);
            this.lblArea.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblArea.Name = "lblArea";
            this.lblArea.Size = new System.Drawing.Size(98, 18);
            this.lblArea.TabIndex = 0;
            this.lblArea.Text = "Defect 영역";
            // 
            // tabKlarfResult
            // 
            this.tabKlarfResult.Controls.Add(this.tabDefect);
            this.tabKlarfResult.Controls.Add(this.tabTolerance);
            this.tabKlarfResult.Controls.Add(this.tabImage);
            this.tabKlarfResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabKlarfResult.Location = new System.Drawing.Point(0, 0);
            this.tabKlarfResult.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabKlarfResult.Name = "tabKlarfResult";
            this.tabKlarfResult.SelectedIndex = 0;
            this.tabKlarfResult.Size = new System.Drawing.Size(901, 700);
            this.tabKlarfResult.TabIndex = 2;
            this.tabKlarfResult.SelectedIndexChanged += new System.EventHandler(this.tabList_SelectedIndexChanged);
            // 
            // tabEt
            // 
            this.tabEt.Controls.Add(this.tabEtResult);
            this.tabEt.Controls.Add(this.pnlEtCriteria);
            this.tabEt.Controls.Add(this.pnlEtHead);
            this.tabEt.Location = new System.Drawing.Point(4, 38);
            this.tabEt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabEt.Name = "tabEt";
            this.tabEt.Size = new System.Drawing.Size(901, 1122);
            this.tabEt.TabIndex = 2;
            this.tabEt.Text = "E-Test";
            this.tabEt.UseVisualStyleBackColor = true;
            // 
            // pnlEtHead
            // 
            this.pnlEtHead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(247)))), ((int)(((byte)(252)))));
            this.pnlEtHead.Controls.Add(this.lblEtCriteria);
            this.pnlEtHead.Controls.Add(this.btnEtFold);
            this.pnlEtHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEtHead.Location = new System.Drawing.Point(0, 0);
            this.pnlEtHead.Margin = new System.Windows.Forms.Padding(0);
            this.pnlEtHead.Name = "pnlEtHead";
            this.pnlEtHead.Size = new System.Drawing.Size(901, 54);
            this.pnlEtHead.TabIndex = 0;
            // 
            // lblEtCriteria
            // 
            this.lblEtCriteria.AutoSize = true;
            this.lblEtCriteria.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblEtCriteria.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(95)))), ((int)(((byte)(0)))));
            this.lblEtCriteria.Location = new System.Drawing.Point(6, 15);
            this.lblEtCriteria.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEtCriteria.Name = "lblEtCriteria";
            this.lblEtCriteria.Size = new System.Drawing.Size(110, 25);
            this.lblEtCriteria.TabIndex = 0;
            this.lblEtCriteria.Text = "E-Test 기준";
            // 
            // btnEtFold
            // 
            this.btnEtFold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEtFold.Image = ((System.Drawing.Image)(resources.GetObject("btnEtFold.Image")));
            this.btnEtFold.Location = new System.Drawing.Point(758, 6);
            this.btnEtFold.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnEtFold.Name = "btnEtFold";
            this.btnEtFold.Size = new System.Drawing.Size(137, 42);
            this.btnEtFold.TabIndex = 1;
            this.btnEtFold.Text = "기준 접기";
            this.btnEtFold.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEtFold.UseVisualStyleBackColor = true;
            this.btnEtFold.Click += new System.EventHandler(this.CriteriaFold_Click);
            // 
            // pnlEtCriteria
            // 
            this.pnlEtCriteria.Controls.Add(this.dgvReadyEt);
            this.pnlEtCriteria.Controls.Add(this.pnlEtCond);
            this.pnlEtCriteria.Controls.Add(this.pnlReadyEtHead);
            this.pnlEtCriteria.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEtCriteria.Location = new System.Drawing.Point(0, 54);
            this.pnlEtCriteria.Margin = new System.Windows.Forms.Padding(0);
            this.pnlEtCriteria.Name = "pnlEtCriteria";
            this.pnlEtCriteria.Size = new System.Drawing.Size(901, 270);
            this.pnlEtCriteria.TabIndex = 1;
            // 
            // pnlEtCond
            // 
            this.pnlEtCond.Controls.Add(this.lblFailMark);
            this.pnlEtCond.Controls.Add(this.chkMarkSpec);
            this.pnlEtCond.Controls.Add(this.chkMarkFile);
            this.pnlEtCond.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlEtCond.Location = new System.Drawing.Point(0, 54);
            this.pnlEtCond.Margin = new System.Windows.Forms.Padding(0);
            this.pnlEtCond.Name = "pnlEtCond";
            this.pnlEtCond.Size = new System.Drawing.Size(901, 50);
            this.pnlEtCond.TabIndex = 2;
            // 
            // tabEtResult
            // 
            this.tabEtResult.Controls.Add(this.tabEtFail);
            this.tabEtResult.Controls.Add(this.tabEtLink);
            this.tabEtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabEtResult.Location = new System.Drawing.Point(0, 0);
            this.tabEtResult.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabEtResult.Name = "tabEtResult";
            this.tabEtResult.SelectedIndex = 0;
            this.tabEtResult.Size = new System.Drawing.Size(901, 700);
            this.tabEtResult.TabIndex = 2;
            // 
            // lblWaferKey
            // 
            this.lblWaferKey.AutoSize = true;
            this.lblWaferKey.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblWaferKey.Location = new System.Drawing.Point(4, 12);
            this.lblWaferKey.Margin = new System.Windows.Forms.Padding(4, 12, 8, 0);
            this.lblWaferKey.Name = "lblWaferKey";
            this.lblWaferKey.Size = new System.Drawing.Size(131, 28);
            this.lblWaferKey.TabIndex = 0;
            this.lblWaferKey.Text = "Wafer Key: -";
            // 
            // btnReadyRefresh
            // 
            this.btnReadyRefresh.Location = new System.Drawing.Point(147, 6);
            this.btnReadyRefresh.Margin = new System.Windows.Forms.Padding(4, 6, 14, 4);
            this.btnReadyRefresh.Name = "btnReadyRefresh";
            this.btnReadyRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnReadyRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnReadyRefresh.Image")));
            this.btnReadyRefresh.Size = new System.Drawing.Size(137, 42);
            this.btnReadyRefresh.TabIndex = 1;
            this.btnReadyRefresh.Text = "다시 조회";
            this.btnReadyRefresh.UseVisualStyleBackColor = true;
            this.btnReadyRefresh.Click += new System.EventHandler(this.btnReadyRefresh_Click);
            // 
            // pnlReadyGdsHead
            // 
            this.pnlReadyGdsHead.Controls.Add(this.lblReadyGds);
            this.pnlReadyGdsHead.Controls.Add(this.lblChainSide);
            this.pnlReadyGdsHead.Controls.Add(this.cboChainSide);
            this.pnlReadyGdsHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlReadyGdsHead.Location = new System.Drawing.Point(0, 48);
            this.pnlReadyGdsHead.Margin = new System.Windows.Forms.Padding(0);
            this.pnlReadyGdsHead.Name = "pnlReadyGdsHead";
            this.pnlReadyGdsHead.Size = new System.Drawing.Size(901, 45);
            this.pnlReadyGdsHead.TabIndex = 1;
            // 
            // lblReadyGds
            // 
            this.lblReadyGds.AutoSize = true;
            this.lblReadyGds.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblReadyGds.Location = new System.Drawing.Point(6, 15);
            this.lblReadyGds.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReadyGds.Name = "lblReadyGds";
            this.lblReadyGds.Size = new System.Drawing.Size(88, 25);
            this.lblReadyGds.TabIndex = 0;
            this.lblReadyGds.Text = "GDS Map";
            // 
            // lblChainSide
            // 
            this.lblChainSide.AutoSize = true;
            this.lblChainSide.Location = new System.Drawing.Point(300, 15);
            this.lblChainSide.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblChainSide.Name = "lblChainSide";
            this.lblChainSide.Size = new System.Drawing.Size(77, 18);
            this.lblChainSide.TabIndex = 1;
            this.lblChainSide.Text = "Chain 면";
            // 
            // cboChainSide
            // 
            this.cboChainSide.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboChainSide.Items.AddRange(new object[] {
            "FS",
            "BS"});
            this.cboChainSide.Location = new System.Drawing.Point(377, 8);
            this.cboChainSide.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboChainSide.Name = "cboChainSide";
            this.cboChainSide.Size = new System.Drawing.Size(84, 26);
            this.cboChainSide.TabIndex = 2;
            this.cboChainSide.SelectedIndexChanged += new System.EventHandler(this.cboChainSide_SelectedIndexChanged);
            // 
            // dgvReadyGds
            // 
            this.dgvReadyGds.AllowUserToAddRows = false;
            this.dgvReadyGds.AllowUserToDeleteRows = false;
            this.dgvReadyGds.AllowUserToResizeRows = false;
            this.dgvReadyGds.BackgroundColor = System.Drawing.Color.White;
            this.dgvReadyGds.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReadyGds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReadyGds.Location = new System.Drawing.Point(4, 97);
            this.dgvReadyGds.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvReadyGds.MultiSelect = false;
            this.dgvReadyGds.Name = "dgvReadyGds";
            this.dgvReadyGds.ReadOnly = true;
            this.dgvReadyGds.RowHeadersVisible = false;
            this.dgvReadyGds.RowHeadersWidth = 62;
            this.dgvReadyGds.RowTemplate.Height = 23;
            this.dgvReadyGds.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.dgvReadyGds.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReadyGds.Size = new System.Drawing.Size(893, 76);
            this.dgvReadyGds.TabIndex = 2;
            // 
            // pnlReadyStepHead
            // 
            this.pnlReadyStepHead.Controls.Add(this.lblReadyStep);
            this.pnlReadyStepHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlReadyStepHead.Location = new System.Drawing.Point(0, 177);
            this.pnlReadyStepHead.Margin = new System.Windows.Forms.Padding(0);
            this.pnlReadyStepHead.Name = "pnlReadyStepHead";
            this.pnlReadyStepHead.Size = new System.Drawing.Size(901, 54);
            this.pnlReadyStepHead.TabIndex = 3;
            // 
            // lblReadyStep
            // 
            this.lblReadyStep.AutoSize = true;
            this.lblReadyStep.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblReadyStep.Location = new System.Drawing.Point(6, 15);
            this.lblReadyStep.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReadyStep.Name = "lblReadyStep";
            this.lblReadyStep.Size = new System.Drawing.Size(223, 25);
            this.lblReadyStep.TabIndex = 0;
            this.lblReadyStep.Text = "KLARF Step";
            //
            // pnlStepInfo
            //
            this.pnlStepInfo.Controls.Add(this.lblStepInfo);
            this.pnlStepInfo.Controls.Add(this.lblPitchSource);
            this.pnlStepInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStepInfo.Location = new System.Drawing.Point(0, 54);
            this.pnlStepInfo.Margin = new System.Windows.Forms.Padding(0);
            this.pnlStepInfo.Name = "pnlStepInfo";
            this.pnlStepInfo.Size = new System.Drawing.Size(901, 88);
            this.pnlStepInfo.TabIndex = 4;
            //
            // lblStepInfo
            //
            this.lblStepInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStepInfo.AutoEllipsis = true;
            this.lblStepInfo.Location = new System.Drawing.Point(8, 8);
            this.lblStepInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStepInfo.Name = "lblStepInfo";
            this.lblStepInfo.Size = new System.Drawing.Size(885, 46);
            this.lblStepInfo.TabIndex = 0;
            this.lblStepInfo.Text = "-";
            //
            // lblPitchSource
            //
            this.lblPitchSource.AutoSize = true;
            this.lblPitchSource.ForeColor = System.Drawing.Color.DimGray;
            this.lblPitchSource.Location = new System.Drawing.Point(8, 56);
            this.lblPitchSource.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPitchSource.Name = "lblPitchSource";
            this.lblPitchSource.Size = new System.Drawing.Size(105, 18);
            this.lblPitchSource.TabIndex = 1;
            this.lblPitchSource.Text = "Pitch 출처: -";
            // 
            // pnlReadyEtHead
            // 
            this.pnlReadyEtHead.Controls.Add(this.lblReadyEt);
            this.pnlReadyEtHead.Controls.Add(this.lblEtStep);
            this.pnlReadyEtHead.Controls.Add(this.cboEtStep);
            this.pnlReadyEtHead.Controls.Add(this.btnFindEt);
            this.pnlReadyEtHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlReadyEtHead.Location = new System.Drawing.Point(0, 517);
            this.pnlReadyEtHead.Margin = new System.Windows.Forms.Padding(0);
            this.pnlReadyEtHead.Name = "pnlReadyEtHead";
            this.pnlReadyEtHead.Size = new System.Drawing.Size(901, 54);
            this.pnlReadyEtHead.TabIndex = 5;
            // 
            // lblReadyEt
            // 
            this.lblReadyEt.AutoSize = true;
            this.lblReadyEt.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblReadyEt.Location = new System.Drawing.Point(6, 15);
            this.lblReadyEt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReadyEt.Name = "lblReadyEt";
            this.lblReadyEt.Size = new System.Drawing.Size(299, 25);
            this.lblReadyEt.TabIndex = 0;
            this.lblReadyEt.Text = "E-Test";
            //
            // lblEtStep
            //
            this.lblEtStep.AutoSize = true;
            this.lblEtStep.Location = new System.Drawing.Point(430, 15);
            this.lblEtStep.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEtStep.Name = "lblEtStep";
            this.lblEtStep.Size = new System.Drawing.Size(40, 18);
            this.lblEtStep.TabIndex = 2;
            this.lblEtStep.Text = "Step";
            //
            // cboEtStep
            //
            this.cboEtStep.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEtStep.Enabled = false;
            this.cboEtStep.Location = new System.Drawing.Point(476, 8);
            this.cboEtStep.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboEtStep.Name = "cboEtStep";
            this.cboEtStep.Size = new System.Drawing.Size(130, 26);
            this.cboEtStep.TabIndex = 3;
            this.cboEtStep.SelectedIndexChanged += new System.EventHandler(this.cboEtStep_SelectedIndexChanged);
            // 
            // btnFindEt
            // 
            this.btnFindEt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindEt.Location = new System.Drawing.Point(758, 6);
            this.btnFindEt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnFindEt.Name = "btnFindEt";
            this.btnFindEt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFindEt.Image = ((System.Drawing.Image)(resources.GetObject("btnFindEt.Image")));
            this.btnFindEt.Size = new System.Drawing.Size(137, 42);
            this.btnFindEt.TabIndex = 1;
            this.btnFindEt.Text = "직접 찾기";
            this.btnFindEt.UseVisualStyleBackColor = true;
            this.btnFindEt.Click += new System.EventHandler(this.btnFindEt_Click);
            // 
            // dgvReadyEt
            // 
            this.dgvReadyEt.AllowUserToAddRows = false;
            this.dgvReadyEt.AllowUserToDeleteRows = false;
            this.dgvReadyEt.AllowUserToResizeRows = false;
            this.dgvReadyEt.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReadyEt.BackgroundColor = System.Drawing.Color.White;
            this.dgvReadyEt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReadyEt.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colEtUse,
            this.colEtProgram,
            this.colEtWafer,
            this.colEtDevice,
            this.colEtTestTime,
            this.colEtDie,
            this.colEtFail,
            this.colEtJudge,
            this.colEtSpec,
            this.colEtLink,
            this.colEtAdded,
            this.colEtSeq});
            this.dgvReadyEt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReadyEt.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvReadyEt.Location = new System.Drawing.Point(4, 566);
            this.dgvReadyEt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvReadyEt.MultiSelect = false;
            this.dgvReadyEt.Name = "dgvReadyEt";
            this.dgvReadyEt.RowHeadersVisible = false;
            this.dgvReadyEt.RowHeadersWidth = 62;
            this.dgvReadyEt.RowTemplate.Height = 23;
            this.dgvReadyEt.Size = new System.Drawing.Size(893, 256);
            this.dgvReadyEt.TabIndex = 6;
            this.dgvReadyEt.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReadyEt_CellValueChanged);
            this.dgvReadyEt.CurrentCellDirtyStateChanged += new System.EventHandler(this.ReadyGrid_CurrentCellDirtyStateChanged);
            // 
            // colEtUse
            // 
            this.colEtUse.FillWeight = 32F;
            this.colEtUse.HeaderText = "선택";
            this.colEtUse.MinimumWidth = 8;
            this.colEtUse.Name = "colEtUse";
            // 
            // colEtProgram
            // 
            this.colEtProgram.FillWeight = 50F;
            this.colEtProgram.HeaderText = "Step";
            this.colEtProgram.MinimumWidth = 8;
            this.colEtProgram.Name = "colEtProgram";
            this.colEtProgram.ReadOnly = true;
            // 
            // colEtWafer
            // 
            this.colEtWafer.HeaderText = "WAFER_ID";
            this.colEtWafer.MinimumWidth = 8;
            this.colEtWafer.Name = "colEtWafer";
            this.colEtWafer.ReadOnly = true;
            // 
            // colEtDevice
            // 
            this.colEtDevice.FillWeight = 55F;
            this.colEtDevice.HeaderText = "Device";
            this.colEtDevice.MinimumWidth = 8;
            this.colEtDevice.Name = "colEtDevice";
            this.colEtDevice.ReadOnly = true;
            // 
            // colEtTestTime
            // 
            this.colEtTestTime.HeaderText = "Test 시각";
            this.colEtTestTime.MinimumWidth = 8;
            this.colEtTestTime.Name = "colEtTestTime";
            this.colEtTestTime.ReadOnly = true;
            // 
            // colEtDie
            // 
            this.colEtDie.FillWeight = 35F;
            this.colEtDie.HeaderText = "Die";
            this.colEtDie.MinimumWidth = 8;
            this.colEtDie.Name = "colEtDie";
            this.colEtDie.ReadOnly = true;
            // 
            // colEtFail
            // 
            this.colEtFail.FillWeight = 45F;
            this.colEtFail.HeaderText = "Fail Die";
            this.colEtFail.MinimumWidth = 8;
            this.colEtFail.Name = "colEtFail";
            this.colEtFail.ReadOnly = true;
            // 
            // colEtJudge
            // 
            this.colEtJudge.HeaderText = "판정 시각";
            this.colEtJudge.MinimumWidth = 8;
            this.colEtJudge.Name = "colEtJudge";
            this.colEtJudge.ReadOnly = true;
            // 
            // colEtSpec
            // 
            this.colEtSpec.FillWeight = 45F;
            this.colEtSpec.HeaderText = "Spec";
            this.colEtSpec.MinimumWidth = 8;
            this.colEtSpec.Name = "colEtSpec";
            this.colEtSpec.ReadOnly = true;
            // 
            // colEtLink
            // 
            this.colEtLink.FillWeight = 75F;
            this.colEtLink.HeaderText = "Chain 연결";
            this.colEtLink.MinimumWidth = 8;
            this.colEtLink.Name = "colEtLink";
            this.colEtLink.ReadOnly = true;
            // 
            // colEtAdded
            // 
            this.colEtAdded.FillWeight = 40F;
            this.colEtAdded.HeaderText = "추가";
            this.colEtAdded.MinimumWidth = 8;
            this.colEtAdded.Name = "colEtAdded";
            this.colEtAdded.ReadOnly = true;
            // 
            // colEtSeq
            // 
            this.colEtSeq.HeaderText = "SRC_FILE_SEQ";
            this.colEtSeq.MinimumWidth = 8;
            this.colEtSeq.Name = "colEtSeq";
            this.colEtSeq.ReadOnly = true;
            this.colEtSeq.Visible = false;
            // 
            // tabDie
            // 
            this.tabDie.Controls.Add(this.dgvDie);
            this.tabDie.Location = new System.Drawing.Point(4, 28);
            this.tabDie.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabDie.Name = "tabDie";
            this.tabDie.Size = new System.Drawing.Size(901, 826);
            this.tabDie.TabIndex = 3;
            this.tabDie.Text = "종합";
            this.tabDie.UseVisualStyleBackColor = true;
            // 
            // dgvDie
            // 
            this.dgvDie.AllowUserToAddRows = false;
            this.dgvDie.AllowUserToDeleteRows = false;
            this.dgvDie.AllowUserToResizeRows = false;
            this.dgvDie.BackgroundColor = System.Drawing.Color.White;
            this.dgvDie.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDie.Location = new System.Drawing.Point(0, 0);
            this.dgvDie.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvDie.MultiSelect = false;
            this.dgvDie.Name = "dgvDie";
            this.dgvDie.ReadOnly = true;
            this.dgvDie.RowHeadersVisible = false;
            this.dgvDie.RowHeadersWidth = 62;
            this.dgvDie.RowTemplate.Height = 23;
            this.dgvDie.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDie.Size = new System.Drawing.Size(901, 826);
            this.dgvDie.TabIndex = 0;
            this.dgvDie.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDie_CellClick);
            this.dgvDie.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDie_CellDoubleClick);
            // 
            // tabChain
            // 
            this.tabChain.Controls.Add(this.dgvChain);
            this.tabChain.Controls.Add(this.pnlChainHead);
            this.tabChain.Location = new System.Drawing.Point(4, 28);
            this.tabChain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabChain.Name = "tabChain";
            this.tabChain.Size = new System.Drawing.Size(901, 826);
            this.tabChain.TabIndex = 2;
            this.tabChain.Text = "Chain";
            this.tabChain.UseVisualStyleBackColor = true;
            // 
            // dgvChain
            // 
            this.dgvChain.AllowUserToAddRows = false;
            this.dgvChain.AllowUserToDeleteRows = false;
            this.dgvChain.AllowUserToResizeRows = false;
            this.dgvChain.BackgroundColor = System.Drawing.Color.White;
            this.dgvChain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvChain.Location = new System.Drawing.Point(0, 42);
            this.dgvChain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvChain.MultiSelect = false;
            this.dgvChain.Name = "dgvChain";
            this.dgvChain.ReadOnly = true;
            this.dgvChain.RowHeadersVisible = false;
            this.dgvChain.RowHeadersWidth = 62;
            this.dgvChain.RowTemplate.Height = 23;
            this.dgvChain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvChain.Size = new System.Drawing.Size(901, 784);
            this.dgvChain.TabIndex = 1;
            this.dgvChain.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChain_CellClick);
            // 
            // pnlChainHead
            // 
            this.pnlChainHead.Controls.Add(this.chkChainAll);
            this.pnlChainHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlChainHead.Location = new System.Drawing.Point(0, 0);
            this.pnlChainHead.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlChainHead.Name = "pnlChainHead";
            this.pnlChainHead.Size = new System.Drawing.Size(901, 42);
            this.pnlChainHead.TabIndex = 0;
            // 
            // chkChainAll
            // 
            this.chkChainAll.AutoSize = true;
            this.chkChainAll.Location = new System.Drawing.Point(9, 9);
            this.chkChainAll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkChainAll.Name = "chkChainAll";
            this.chkChainAll.Size = new System.Drawing.Size(122, 22);
            this.chkChainAll.TabIndex = 0;
            this.chkChainAll.Text = "Wafer 전체";
            this.chkChainAll.UseVisualStyleBackColor = true;
            this.chkChainAll.CheckedChanged += new System.EventHandler(this.ListFilter_Changed);
            // 
            // tabDefect
            // 
            this.tabDefect.Controls.Add(this.dgvDefect);
            this.tabDefect.Controls.Add(this.pnlDefectHead);
            this.tabDefect.Location = new System.Drawing.Point(4, 28);
            this.tabDefect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabDefect.Name = "tabDefect";
            this.tabDefect.Size = new System.Drawing.Size(901, 826);
            this.tabDefect.TabIndex = 3;
            this.tabDefect.Text = "Defect";
            this.tabDefect.UseVisualStyleBackColor = true;
            // 
            // dgvDefect
            // 
            this.dgvDefect.AllowUserToAddRows = false;
            this.dgvDefect.AllowUserToDeleteRows = false;
            this.dgvDefect.AllowUserToResizeRows = false;
            this.dgvDefect.BackgroundColor = System.Drawing.Color.White;
            this.dgvDefect.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDefect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDefect.Location = new System.Drawing.Point(0, 42);
            this.dgvDefect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvDefect.MultiSelect = false;
            this.dgvDefect.Name = "dgvDefect";
            this.dgvDefect.ReadOnly = true;
            this.dgvDefect.RowHeadersVisible = false;
            this.dgvDefect.RowHeadersWidth = 62;
            this.dgvDefect.RowTemplate.Height = 23;
            this.dgvDefect.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDefect.Size = new System.Drawing.Size(901, 784);
            this.dgvDefect.TabIndex = 1;
            this.dgvDefect.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDefect_CellDoubleClick);
            // 
            // pnlDefectHead
            // 
            this.pnlDefectHead.Controls.Add(this.cboDefectScope);
            this.pnlDefectHead.Controls.Add(this.chkDefectAll);
            this.pnlDefectHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDefectHead.Location = new System.Drawing.Point(0, 0);
            this.pnlDefectHead.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlDefectHead.Name = "pnlDefectHead";
            this.pnlDefectHead.Size = new System.Drawing.Size(901, 42);
            this.pnlDefectHead.TabIndex = 0;
            // 
            // cboDefectScope
            // 
            this.cboDefectScope.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDefectScope.Items.AddRange(new object[] {
            "Chain In",
            "Chain Out",
            "Defect 전체"});
            this.cboDefectScope.Location = new System.Drawing.Point(9, 6);
            this.cboDefectScope.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboDefectScope.Name = "cboDefectScope";
            this.cboDefectScope.Size = new System.Drawing.Size(155, 26);
            this.cboDefectScope.TabIndex = 0;
            this.cboDefectScope.SelectedIndexChanged += new System.EventHandler(this.ListFilter_Changed);
            // 
            // chkDefectAll
            // 
            this.chkDefectAll.AutoSize = true;
            this.chkDefectAll.Location = new System.Drawing.Point(183, 9);
            this.chkDefectAll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkDefectAll.Name = "chkDefectAll";
            this.chkDefectAll.Size = new System.Drawing.Size(122, 22);
            this.chkDefectAll.TabIndex = 1;
            this.chkDefectAll.Text = "Wafer 전체";
            this.chkDefectAll.UseVisualStyleBackColor = true;
            this.chkDefectAll.CheckedChanged += new System.EventHandler(this.ListFilter_Changed);
            // 
            // tabTolerance
            // 
            this.tabTolerance.Controls.Add(this.dgvTolerance);
            this.tabTolerance.Location = new System.Drawing.Point(4, 28);
            this.tabTolerance.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabTolerance.Name = "tabTolerance";
            this.tabTolerance.Size = new System.Drawing.Size(901, 826);
            this.tabTolerance.TabIndex = 4;
            this.tabTolerance.Text = "허용오차";
            this.tabTolerance.UseVisualStyleBackColor = true;
            // 
            // dgvTolerance
            // 
            this.dgvTolerance.AllowUserToAddRows = false;
            this.dgvTolerance.AllowUserToDeleteRows = false;
            this.dgvTolerance.AllowUserToResizeRows = false;
            this.dgvTolerance.BackgroundColor = System.Drawing.Color.White;
            this.dgvTolerance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTolerance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTolerance.Location = new System.Drawing.Point(0, 0);
            this.dgvTolerance.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvTolerance.MultiSelect = false;
            this.dgvTolerance.Name = "dgvTolerance";
            this.dgvTolerance.ReadOnly = true;
            this.dgvTolerance.RowHeadersVisible = false;
            this.dgvTolerance.RowHeadersWidth = 62;
            this.dgvTolerance.RowTemplate.Height = 23;
            this.dgvTolerance.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTolerance.Size = new System.Drawing.Size(901, 826);
            this.dgvTolerance.TabIndex = 0;
            // 
            // tabEtLink
            // 
            this.tabEtLink.Controls.Add(this.dgvEtLink);
            this.tabEtLink.Location = new System.Drawing.Point(4, 28);
            this.tabEtLink.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabEtLink.Name = "tabEtLink";
            this.tabEtLink.Size = new System.Drawing.Size(901, 826);
            this.tabEtLink.TabIndex = 5;
            this.tabEtLink.Text = "E-Test 연결";
            this.tabEtLink.UseVisualStyleBackColor = true;
            // 
            // dgvEtLink
            // 
            this.dgvEtLink.AllowUserToAddRows = false;
            this.dgvEtLink.AllowUserToDeleteRows = false;
            this.dgvEtLink.AllowUserToResizeRows = false;
            this.dgvEtLink.BackgroundColor = System.Drawing.Color.White;
            this.dgvEtLink.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEtLink.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEtLink.Location = new System.Drawing.Point(0, 0);
            this.dgvEtLink.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvEtLink.MultiSelect = false;
            this.dgvEtLink.Name = "dgvEtLink";
            this.dgvEtLink.ReadOnly = true;
            this.dgvEtLink.RowHeadersVisible = false;
            this.dgvEtLink.RowHeadersWidth = 62;
            this.dgvEtLink.RowTemplate.Height = 23;
            this.dgvEtLink.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEtLink.Size = new System.Drawing.Size(901, 826);
            this.dgvEtLink.TabIndex = 0;
            // 
            // tabEtFail
            // 
            this.tabEtFail.Controls.Add(this.dgvEtFail);
            this.tabEtFail.Location = new System.Drawing.Point(4, 28);
            this.tabEtFail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabEtFail.Name = "tabEtFail";
            this.tabEtFail.Size = new System.Drawing.Size(901, 826);
            this.tabEtFail.TabIndex = 6;
            this.tabEtFail.Text = "E-Test Fail";
            this.tabEtFail.UseVisualStyleBackColor = true;
            // 
            // dgvEtFail
            // 
            this.dgvEtFail.AllowUserToAddRows = false;
            this.dgvEtFail.AllowUserToDeleteRows = false;
            this.dgvEtFail.AllowUserToResizeRows = false;
            this.dgvEtFail.BackgroundColor = System.Drawing.Color.White;
            this.dgvEtFail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEtFail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEtFail.Location = new System.Drawing.Point(0, 0);
            this.dgvEtFail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvEtFail.MultiSelect = false;
            this.dgvEtFail.Name = "dgvEtFail";
            this.dgvEtFail.ReadOnly = true;
            this.dgvEtFail.RowHeadersVisible = false;
            this.dgvEtFail.RowHeadersWidth = 62;
            this.dgvEtFail.RowTemplate.Height = 23;
            this.dgvEtFail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEtFail.Size = new System.Drawing.Size(901, 826);
            this.dgvEtFail.TabIndex = 0;
            this.dgvEtFail.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEtFail_CellClick);
            this.dgvEtFail.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEtFail_CellDoubleClick);
            // 
            // tabImage
            // 
            this.tabImage.Controls.Add(this.flowImage);
            this.tabImage.Location = new System.Drawing.Point(4, 28);
            this.tabImage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabImage.Name = "tabImage";
            this.tabImage.Size = new System.Drawing.Size(901, 826);
            this.tabImage.TabIndex = 7;
            this.tabImage.Text = "Image";
            this.tabImage.UseVisualStyleBackColor = true;
            // 
            // flowImage
            // 
            this.flowImage.AutoScroll = true;
            this.flowImage.BackColor = System.Drawing.Color.White;
            this.flowImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowImage.Location = new System.Drawing.Point(0, 0);
            this.flowImage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flowImage.Name = "flowImage";
            this.flowImage.Size = new System.Drawing.Size(901, 826);
            this.flowImage.TabIndex = 0;
            // 
            // pnlStatus
            // 
            this.pnlStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(242)))), ((int)(((byte)(250)))));
            this.pnlStatus.Controls.Add(this.lblStatus);
            this.pnlStatus.Controls.Add(this.busyBar);
            this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlStatus.Location = new System.Drawing.Point(0, 1281);
            this.pnlStatus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(2000, 69);
            this.pnlStatus.TabIndex = 90;
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(242)))), ((int)(((byte)(250)))));
            this.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Location = new System.Drawing.Point(0, 0);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.lblStatus.Size = new System.Drawing.Size(1400, 69);
            this.lblStatus.TabIndex = 2;
            // 
            // busyBar
            // 
            this.busyBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.busyBar.Location = new System.Drawing.Point(1400, 0);
            this.busyBar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.busyBar.Name = "busyBar";
            this.busyBar.Padding = new System.Windows.Forms.Padding(0, 4, 9, 4);
            this.busyBar.Size = new System.Drawing.Size(600, 69);
            this.busyBar.TabIndex = 1;
            // 
            // colorDialog
            // 
            this.colorDialog.FullOpen = true;
            // 
            // frmGdsKlarfAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2000, 1350);
            this.Controls.Add(this.pnlBand);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.pnlStatus);
            this.Controls.Add(this.pnlTop);
            this.Margin = new System.Windows.Forms.Padding(9, 9, 9, 9);
            this.Name = "frmGdsKlarfAnalysis";
            this.Text = "GDS-KLARF 연관 분석";
            this.Load += new System.EventHandler(this.frmGdsKlarfAnalysis_Load);
            this.flowSummary.ResumeLayout(false);
            this.flowSummary.PerformLayout();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTolerance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSquare)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOffsetX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOffsetY)).EndInit();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel1.PerformLayout();
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.pnlMapHead.ResumeLayout(false);
            this.pnlMapHead.PerformLayout();
            this.pnlBand.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBand)).EndInit();
            this.pnlBandHead.ResumeLayout(false);
            this.pnlBandHead.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numThreshold)).EndInit();
            this.tabList.ResumeLayout(false);
            this.tabGds.ResumeLayout(false);
            this.pnlGdsHead.ResumeLayout(false);
            this.pnlGdsHead.PerformLayout();
            this.pnlGdsCriteria.ResumeLayout(false);
            this.tabGdsResult.ResumeLayout(false);
            this.tabKlarf.ResumeLayout(false);
            this.pnlKlarfHead.ResumeLayout(false);
            this.pnlKlarfHead.PerformLayout();
            this.pnlKlarfCriteria.ResumeLayout(false);
            this.pnlKlarfCond.ResumeLayout(false);
            this.pnlKlarfCond.PerformLayout();
            this.tabKlarfResult.ResumeLayout(false);
            this.tabEt.ResumeLayout(false);
            this.pnlEtHead.ResumeLayout(false);
            this.pnlEtHead.PerformLayout();
            this.pnlEtCriteria.ResumeLayout(false);
            this.pnlEtCond.ResumeLayout(false);
            this.pnlEtCond.PerformLayout();
            this.tabEtResult.ResumeLayout(false);
            this.pnlReadyGdsHead.ResumeLayout(false);
            this.pnlReadyGdsHead.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReadyGds)).EndInit();
            this.pnlReadyStepHead.ResumeLayout(false);
            this.pnlReadyStepHead.PerformLayout();
            this.pnlStepInfo.ResumeLayout(false);
            this.pnlStepInfo.PerformLayout();
            this.pnlReadyEtHead.ResumeLayout(false);
            this.pnlReadyEtHead.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReadyEt)).EndInit();
            this.tabDie.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDie)).EndInit();
            this.tabChain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChain)).EndInit();
            this.pnlChainHead.ResumeLayout(false);
            this.pnlChainHead.PerformLayout();
            this.tabDefect.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDefect)).EndInit();
            this.pnlDefectHead.ResumeLayout(false);
            this.pnlDefectHead.PerformLayout();
            this.tabTolerance.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTolerance)).EndInit();
            this.tabEtLink.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEtLink)).EndInit();
            this.tabEtFail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEtFail)).EndInit();
            this.tabImage.ResumeLayout(false);
            this.pnlStatus.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblWafer;
        private System.Windows.Forms.Label lblWaferInfo;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.Label lblDeviceValue;
        private System.Windows.Forms.Label lblClassBasis;
        private System.Windows.Forms.ComboBox cboClassBasis;
        private System.Windows.Forms.Button btnClass;
        private System.Windows.Forms.Button btnChain;
        private System.Windows.Forms.CheckBox chkDieIndex;
        private System.Windows.Forms.Label lblTolerance;
        private System.Windows.Forms.NumericUpDown numTolerance;
        private System.Windows.Forms.RadioButton rdoDefectSize;
        private System.Windows.Forms.RadioButton rdoSquare;
        private System.Windows.Forms.NumericUpDown numSquare;
        private System.Windows.Forms.CheckBox chkLargeDefect;
        private System.Windows.Forms.Label lblOffset;
        private System.Windows.Forms.NumericUpDown numOffsetX;
        private System.Windows.Forms.NumericUpDown numOffsetY;
        private System.Windows.Forms.Label lblColorBasis;
        private System.Windows.Forms.ComboBox cboColorBasis;
        private System.Windows.Forms.Button btnAnalyze;
        private System.Windows.Forms.FlowLayoutPanel flowSummary;
        private System.Windows.Forms.ContextMenuStrip menuClass;
        private System.Windows.Forms.ContextMenuStrip menuChain;
        private System.Windows.Forms.SplitContainer splitMain;
        private DACrux.SEMDMS.ENGUI.GdsKlarfAnalysisMap map;
        private System.Windows.Forms.Panel pnlMapHead;
        private System.Windows.Forms.Label lblMapTitle;
        private System.Windows.Forms.Label lblMapBasis;
        private System.Windows.Forms.Button btnBand;
        private System.Windows.Forms.Panel pnlBand;
        private System.Windows.Forms.DataGridView dgvBand;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBandFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBandTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBandColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBandDies;
        private System.Windows.Forms.Panel pnlBandHead;
        private System.Windows.Forms.Label lblBandTitle;
        private System.Windows.Forms.CheckBox chkAutoBand;
        private System.Windows.Forms.Label lblThreshold;
        private System.Windows.Forms.NumericUpDown numThreshold;
        private System.Windows.Forms.Label lblFailMark;
        private System.Windows.Forms.CheckBox chkMarkSpec;
        private System.Windows.Forms.CheckBox chkMarkFile;
        private System.Windows.Forms.TabControl tabList;
        private System.Windows.Forms.TabPage tabGds;
        private System.Windows.Forms.Panel pnlGdsHead;
        private System.Windows.Forms.Label lblGdsCriteria;
        private System.Windows.Forms.Button btnGdsFold;
        private System.Windows.Forms.Panel pnlGdsCriteria;
        private System.Windows.Forms.TabControl tabGdsResult;
        private System.Windows.Forms.TabPage tabKlarf;
        private System.Windows.Forms.Panel pnlKlarfHead;
        private System.Windows.Forms.Label lblKlarfCriteria;
        private System.Windows.Forms.Button btnKlarfFold;
        private System.Windows.Forms.Panel pnlKlarfCriteria;
        private System.Windows.Forms.Panel pnlKlarfCond;
        private System.Windows.Forms.Label lblArea;
        private System.Windows.Forms.TabControl tabKlarfResult;
        private System.Windows.Forms.TabPage tabEt;
        private System.Windows.Forms.Panel pnlEtHead;
        private System.Windows.Forms.Label lblEtCriteria;
        private System.Windows.Forms.Button btnEtFold;
        private System.Windows.Forms.Panel pnlEtCriteria;
        private System.Windows.Forms.Panel pnlEtCond;
        private System.Windows.Forms.TabControl tabEtResult;
        private System.Windows.Forms.Label lblWaferKey;
        private System.Windows.Forms.Button btnReadyRefresh;
        private System.Windows.Forms.Panel pnlReadyGdsHead;
        private System.Windows.Forms.Label lblReadyGds;
        private System.Windows.Forms.Label lblChainSide;
        private System.Windows.Forms.ComboBox cboChainSide;
        private System.Windows.Forms.DataGridView dgvReadyGds;
        private System.Windows.Forms.Panel pnlReadyStepHead;
        private System.Windows.Forms.Label lblReadyStep;
        private System.Windows.Forms.Panel pnlStepInfo;
        private System.Windows.Forms.Label lblStepInfo;
        private System.Windows.Forms.Label lblPitchSource;
        private System.Windows.Forms.Panel pnlReadyEtHead;
        private System.Windows.Forms.Label lblReadyEt;
        private System.Windows.Forms.Button btnFindEt;
        private System.Windows.Forms.Label lblEtStep;
        private System.Windows.Forms.ComboBox cboEtStep;
        private System.Windows.Forms.DataGridView dgvReadyEt;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colEtUse;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEtProgram;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEtWafer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEtDevice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEtTestTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEtDie;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEtFail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEtJudge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEtSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEtLink;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEtAdded;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEtSeq;
        private System.Windows.Forms.TabPage tabDie;
        private System.Windows.Forms.DataGridView dgvDie;
        private System.Windows.Forms.TabPage tabChain;
        private System.Windows.Forms.DataGridView dgvChain;
        private System.Windows.Forms.Panel pnlChainHead;
        private System.Windows.Forms.CheckBox chkChainAll;
        private System.Windows.Forms.TabPage tabDefect;
        private System.Windows.Forms.DataGridView dgvDefect;
        private System.Windows.Forms.Panel pnlDefectHead;
        private System.Windows.Forms.ComboBox cboDefectScope;
        private System.Windows.Forms.CheckBox chkDefectAll;
        private System.Windows.Forms.TabPage tabTolerance;
        private System.Windows.Forms.DataGridView dgvTolerance;
        private System.Windows.Forms.TabPage tabEtLink;
        private System.Windows.Forms.DataGridView dgvEtLink;
        private System.Windows.Forms.TabPage tabEtFail;
        private System.Windows.Forms.DataGridView dgvEtFail;
        private System.Windows.Forms.TabPage tabImage;
        private System.Windows.Forms.FlowLayoutPanel flowImage;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Label lblStatus;
        private DACrux.SEMDMS.Control.ProgressStatusBar busyBar;
        private System.Windows.Forms.ColorDialog colorDialog;
    }
}
