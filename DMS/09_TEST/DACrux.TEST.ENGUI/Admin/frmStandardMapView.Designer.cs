namespace DACrux.TEST.ENGUI
{
    // Wafer Map 조회 화면의 컨트롤과 배치를 정의한다. 목록 조회 / Map 표시 / 확인 표 동작은 본문 파일에서 처리한다.
    partial class frmStandardMapView
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>화면 종료 시 디자이너 구성 요소를 해제한다.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>위쪽 검색 줄 / 왼쪽 Map 목록 / 가운데 Map / 오른쪽 탭(Map 정보 / 데이터 확인 / Die 목록)을 WinForms 디자이너 구성으로 만든다.</summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStandardMapView));
            System.Windows.Forms.DataGridViewCellStyle numberStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle wrapStyle = new System.Windows.Forms.DataGridViewCellStyle();
            this.topBar = new System.Windows.Forms.Panel();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblListCount = new System.Windows.Forms.Label();
            this.chkDieNumber = new System.Windows.Forms.CheckBox();
            this.lblHelp = new System.Windows.Forms.Label();
            this.pnlList = new System.Windows.Forms.Panel();
            this.grdMapList = new System.Windows.Forms.DataGridView();
            this.colMapId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWafer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNetDie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDirection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabRight = new System.Windows.Forms.TabControl();
            this.tabInfo = new System.Windows.Forms.TabPage();
            this.grdInfo = new System.Windows.Forms.DataGridView();
            this.colInfoName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInfoValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabCheck = new System.Windows.Forms.TabPage();
            this.grdCheck = new System.Windows.Forms.DataGridView();
            this.colCheckName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCheckStored = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCheckDie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCheckResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCheckDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabDies = new System.Windows.Forms.TabPage();
            this.grdDies = new System.Windows.Forms.DataGridView();
            this.colDieX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDieY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDieLbX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDieLbY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDieUseCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDieInside = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDieNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDieStoredNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblDieCount = new System.Windows.Forms.Label();
            this.lblCoordinates = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.m_wMap = new DACrux.TEST.ENGUI.StandardMapEditMap();
            this.topBar.SuspendLayout();
            this.pnlList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMapList)).BeginInit();
            this.tabRight.SuspendLayout();
            this.tabInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdInfo)).BeginInit();
            this.tabCheck.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCheck)).BeginInit();
            this.tabDies.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDies)).BeginInit();
            this.SuspendLayout();
            //
            // numberStyle
            //
            numberStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            //
            // wrapStyle
            //
            wrapStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            //
            // topBar
            //
            this.topBar.Controls.Add(this.lblSearch);
            this.topBar.Controls.Add(this.txtSearch);
            this.topBar.Controls.Add(this.btnSearch);
            this.topBar.Controls.Add(this.lblListCount);
            this.topBar.Controls.Add(this.chkDieNumber);
            this.topBar.Controls.Add(this.lblHelp);
            this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar.Location = new System.Drawing.Point(0, 0);
            this.topBar.Name = "topBar";
            this.topBar.Size = new System.Drawing.Size(1634, 52);
            this.topBar.TabIndex = 0;
            //
            // lblSearch
            //
            this.lblSearch.Location = new System.Drawing.Point(12, 14);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(70, 24);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Device";
            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtSearch
            //
            this.txtSearch.Location = new System.Drawing.Point(86, 12);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(260, 31);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            //
            // btnSearch
            //
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(356, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(110, 32);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "  조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            //
            // lblListCount
            //
            this.lblListCount.Location = new System.Drawing.Point(476, 14);
            this.lblListCount.Name = "lblListCount";
            this.lblListCount.Size = new System.Drawing.Size(160, 24);
            this.lblListCount.TabIndex = 3;
            this.lblListCount.Text = "0 / 0 건";
            this.lblListCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // chkDieNumber
            //
            this.chkDieNumber.Checked = true;
            this.chkDieNumber.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDieNumber.Location = new System.Drawing.Point(646, 12);
            this.chkDieNumber.Name = "chkDieNumber";
            this.chkDieNumber.Size = new System.Drawing.Size(150, 28);
            this.chkDieNumber.TabIndex = 5;
            this.chkDieNumber.Text = "Die 번호 표시";
            this.chkDieNumber.UseVisualStyleBackColor = true;
            this.chkDieNumber.CheckedChanged += new System.EventHandler(this.chkDieNumber_CheckedChanged);
            //
            // lblHelp
            //
            this.lblHelp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHelp.ForeColor = System.Drawing.Color.DimGray;
            this.lblHelp.Location = new System.Drawing.Point(806, 14);
            this.lblHelp.Name = "lblHelp";
            this.lblHelp.Size = new System.Drawing.Size(816, 24);
            this.lblHelp.TabIndex = 4;
            this.lblHelp.Text = "보기 전용 / 검색 = Device 포함 (대소문자 무시) / [조회] = 다시 읽기 / Die 번호 = XY 방향 시작 모서리부터 행 우선, 사용 Die 만 / 드래그 = 확대";
            this.lblHelp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // pnlList
            //
            this.pnlList.Controls.Add(this.grdMapList);
            this.pnlList.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlList.Location = new System.Drawing.Point(0, 52);
            this.pnlList.Name = "pnlList";
            this.pnlList.Padding = new System.Windows.Forms.Padding(8, 0, 4, 0);
            this.pnlList.Size = new System.Drawing.Size(440, 848);
            this.pnlList.TabIndex = 1;
            //
            // grdMapList
            //
            this.grdMapList.AllowUserToAddRows = false;
            this.grdMapList.AllowUserToDeleteRows = false;
            this.grdMapList.AllowUserToResizeRows = false;
            this.grdMapList.BackgroundColor = System.Drawing.Color.White;
            this.grdMapList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdMapList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMapId,
            this.colWafer,
            this.colNetDie,
            this.colDirection});
            this.grdMapList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdMapList.Location = new System.Drawing.Point(8, 0);
            this.grdMapList.MultiSelect = false;
            this.grdMapList.Name = "grdMapList";
            this.grdMapList.ReadOnly = true;
            this.grdMapList.RowHeadersVisible = false;
            this.grdMapList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdMapList.Size = new System.Drawing.Size(428, 848);
            this.grdMapList.TabIndex = 0;
            this.grdMapList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.grdMapList_DataBindingComplete);
            this.grdMapList.SelectionChanged += new System.EventHandler(this.grdMapList_SelectionChanged);
            //
            // colMapId
            //
            this.colMapId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colMapId.DataPropertyName = "MAPID";
            this.colMapId.HeaderText = "Device (Map ID)";
            this.colMapId.MinimumWidth = 120;
            this.colMapId.Name = "colMapId";
            this.colMapId.ReadOnly = true;
            //
            // colWafer
            //
            this.colWafer.DataPropertyName = "WAFER_MM";
            this.colWafer.DefaultCellStyle = numberStyle;
            this.colWafer.HeaderText = "Wafer (mm)";
            this.colWafer.Name = "colWafer";
            this.colWafer.ReadOnly = true;
            this.colWafer.Width = 70;
            //
            // colNetDie
            //
            this.colNetDie.DataPropertyName = "NETDIE";
            this.colNetDie.DefaultCellStyle = numberStyle;
            this.colNetDie.HeaderText = "Net Die";
            this.colNetDie.Name = "colNetDie";
            this.colNetDie.ReadOnly = true;
            this.colNetDie.Width = 70;
            //
            // colDirection
            //
            this.colDirection.DataPropertyName = "XY_NAME";
            this.colDirection.HeaderText = "XY 방향";
            this.colDirection.Name = "colDirection";
            this.colDirection.ReadOnly = true;
            this.colDirection.Width = 100;
            //
            // tabRight
            //
            this.tabRight.Controls.Add(this.tabInfo);
            this.tabRight.Controls.Add(this.tabCheck);
            this.tabRight.Controls.Add(this.tabDies);
            this.tabRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.tabRight.Location = new System.Drawing.Point(1154, 52);
            this.tabRight.Name = "tabRight";
            this.tabRight.SelectedIndex = 0;
            this.tabRight.Size = new System.Drawing.Size(480, 848);
            this.tabRight.TabIndex = 4;
            //
            // tabInfo
            //
            this.tabInfo.Controls.Add(this.grdInfo);
            this.tabInfo.Location = new System.Drawing.Point(4, 34);
            this.tabInfo.Name = "tabInfo";
            this.tabInfo.Size = new System.Drawing.Size(472, 810);
            this.tabInfo.TabIndex = 0;
            this.tabInfo.Text = "Map 정보";
            this.tabInfo.UseVisualStyleBackColor = true;
            //
            // grdInfo
            //
            this.grdInfo.AllowUserToAddRows = false;
            this.grdInfo.AllowUserToDeleteRows = false;
            this.grdInfo.AllowUserToResizeRows = false;
            this.grdInfo.BackgroundColor = System.Drawing.Color.White;
            this.grdInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colInfoName,
            this.colInfoValue});
            this.grdInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdInfo.Location = new System.Drawing.Point(0, 0);
            this.grdInfo.Name = "grdInfo";
            this.grdInfo.ReadOnly = true;
            this.grdInfo.RowHeadersVisible = false;
            this.grdInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdInfo.Size = new System.Drawing.Size(472, 810);
            this.grdInfo.TabIndex = 0;
            this.grdInfo.VisibleChanged += new System.EventHandler(this.InfoGrid_VisibleChanged);
            //
            // colInfoName
            //
            this.colInfoName.HeaderText = "항목";
            this.colInfoName.Name = "colInfoName";
            this.colInfoName.ReadOnly = true;
            this.colInfoName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colInfoName.Width = 180;
            //
            // colInfoValue
            //
            this.colInfoValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colInfoValue.HeaderText = "값";
            this.colInfoValue.Name = "colInfoValue";
            this.colInfoValue.ReadOnly = true;
            this.colInfoValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            //
            // tabCheck
            //
            this.tabCheck.Controls.Add(this.grdCheck);
            this.tabCheck.Location = new System.Drawing.Point(4, 34);
            this.tabCheck.Name = "tabCheck";
            this.tabCheck.Size = new System.Drawing.Size(472, 810);
            this.tabCheck.TabIndex = 1;
            this.tabCheck.Text = "데이터 확인";
            this.tabCheck.UseVisualStyleBackColor = true;
            //
            // grdCheck
            //
            this.grdCheck.AllowUserToAddRows = false;
            this.grdCheck.AllowUserToDeleteRows = false;
            this.grdCheck.AllowUserToResizeRows = false;
            this.grdCheck.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.grdCheck.BackgroundColor = System.Drawing.Color.White;
            this.grdCheck.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdCheck.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCheckName,
            this.colCheckStored,
            this.colCheckDie,
            this.colCheckResult,
            this.colCheckDescription});
            this.grdCheck.DefaultCellStyle = wrapStyle;
            this.grdCheck.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdCheck.Location = new System.Drawing.Point(0, 0);
            this.grdCheck.Name = "grdCheck";
            this.grdCheck.ReadOnly = true;
            this.grdCheck.RowHeadersVisible = false;
            this.grdCheck.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdCheck.Size = new System.Drawing.Size(472, 810);
            this.grdCheck.TabIndex = 0;
            this.grdCheck.VisibleChanged += new System.EventHandler(this.InfoGrid_VisibleChanged);
            //
            // colCheckName
            //
            this.colCheckName.HeaderText = "항목";
            this.colCheckName.Name = "colCheckName";
            this.colCheckName.ReadOnly = true;
            this.colCheckName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCheckName.Width = 105;
            //
            // colCheckStored
            //
            this.colCheckStored.HeaderText = "저장값";
            this.colCheckStored.Name = "colCheckStored";
            this.colCheckStored.ReadOnly = true;
            this.colCheckStored.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCheckStored.Width = 90;
            //
            // colCheckDie
            //
            this.colCheckDie.HeaderText = "Die 행 / 기준";
            this.colCheckDie.Name = "colCheckDie";
            this.colCheckDie.ReadOnly = true;
            this.colCheckDie.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCheckDie.Width = 105;
            //
            // colCheckResult
            //
            this.colCheckResult.HeaderText = "결과";
            this.colCheckResult.Name = "colCheckResult";
            this.colCheckResult.ReadOnly = true;
            this.colCheckResult.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCheckResult.Width = 50;
            //
            // colCheckDescription
            //
            this.colCheckDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCheckDescription.HeaderText = "설명";
            this.colCheckDescription.MinimumWidth = 80;
            this.colCheckDescription.Name = "colCheckDescription";
            this.colCheckDescription.ReadOnly = true;
            this.colCheckDescription.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            //
            // tabDies
            //
            this.tabDies.Controls.Add(this.grdDies);
            this.tabDies.Location = new System.Drawing.Point(4, 34);
            this.tabDies.Name = "tabDies";
            this.tabDies.Size = new System.Drawing.Size(472, 810);
            this.tabDies.TabIndex = 2;
            this.tabDies.Text = "Die 목록";
            this.tabDies.UseVisualStyleBackColor = true;
            //
            // grdDies
            //
            this.grdDies.AllowUserToAddRows = false;
            this.grdDies.AllowUserToDeleteRows = false;
            this.grdDies.AllowUserToResizeRows = false;
            this.grdDies.BackgroundColor = System.Drawing.Color.White;
            this.grdDies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdDies.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDieX,
            this.colDieY,
            this.colDieLbX,
            this.colDieLbY,
            this.colDieUseCode,
            this.colDieInside,
            this.colDieNumber,
            this.colDieStoredNum});
            this.grdDies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdDies.Location = new System.Drawing.Point(0, 0);
            this.grdDies.Name = "grdDies";
            this.grdDies.ReadOnly = true;
            this.grdDies.RowHeadersVisible = false;
            this.grdDies.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdDies.Size = new System.Drawing.Size(472, 810);
            this.grdDies.TabIndex = 0;
            //
            // colDieX
            //
            this.colDieX.DataPropertyName = "X";
            this.colDieX.DefaultCellStyle = numberStyle;
            this.colDieX.HeaderText = "X (저장)";
            this.colDieX.Name = "colDieX";
            this.colDieX.ReadOnly = true;
            this.colDieX.Width = 75;
            //
            // colDieY
            //
            this.colDieY.DataPropertyName = "Y";
            this.colDieY.DefaultCellStyle = numberStyle;
            this.colDieY.HeaderText = "Y (저장)";
            this.colDieY.Name = "colDieY";
            this.colDieY.ReadOnly = true;
            this.colDieY.Width = 75;
            //
            // colDieLbX
            //
            this.colDieLbX.DataPropertyName = "LB_X";
            this.colDieLbX.DefaultCellStyle = numberStyle;
            this.colDieLbX.HeaderText = "LB X";
            this.colDieLbX.Name = "colDieLbX";
            this.colDieLbX.ReadOnly = true;
            this.colDieLbX.Width = 65;
            //
            // colDieLbY
            //
            this.colDieLbY.DataPropertyName = "LB_Y";
            this.colDieLbY.DefaultCellStyle = numberStyle;
            this.colDieLbY.HeaderText = "LB Y";
            this.colDieLbY.Name = "colDieLbY";
            this.colDieLbY.ReadOnly = true;
            this.colDieLbY.Width = 65;
            //
            // colDieUseCode
            //
            this.colDieUseCode.DataPropertyName = "USECODE";
            this.colDieUseCode.DefaultCellStyle = numberStyle;
            this.colDieUseCode.HeaderText = "USECODE";
            this.colDieUseCode.Name = "colDieUseCode";
            this.colDieUseCode.ReadOnly = true;
            this.colDieUseCode.Width = 85;
            //
            // colDieInside
            //
            this.colDieInside.DataPropertyName = "INSIDE";
            this.colDieInside.HeaderText = "Wafer 안";
            this.colDieInside.Name = "colDieInside";
            this.colDieInside.ReadOnly = true;
            this.colDieInside.Width = 75;
            //
            // colDieNumber
            //
            this.colDieNumber.DataPropertyName = "DIE_NO_DIR";
            this.colDieNumber.DefaultCellStyle = numberStyle;
            this.colDieNumber.HeaderText = "Die 번호";
            this.colDieNumber.Name = "colDieNumber";
            this.colDieNumber.ReadOnly = true;
            this.colDieNumber.Width = 75;
            //
            // colDieStoredNum
            //
            this.colDieStoredNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDieStoredNum.DataPropertyName = "DIE_NUM";
            this.colDieStoredNum.DefaultCellStyle = numberStyle;
            this.colDieStoredNum.HeaderText = "DIE_NUM (저장)";
            this.colDieStoredNum.MinimumWidth = 60;
            this.colDieStoredNum.Name = "colDieStoredNum";
            this.colDieStoredNum.ReadOnly = true;
            //
            // lblDieCount
            //
            this.lblDieCount.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblDieCount.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDieCount.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lblDieCount.Location = new System.Drawing.Point(440, 52);
            this.lblDieCount.Name = "lblDieCount";
            this.lblDieCount.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.lblDieCount.Size = new System.Drawing.Size(714, 36);
            this.lblDieCount.TabIndex = 3;
            this.lblDieCount.Text = "Map: -";
            this.lblDieCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblCoordinates
            //
            this.lblCoordinates.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblCoordinates.Location = new System.Drawing.Point(0, 900);
            this.lblCoordinates.Name = "lblCoordinates";
            this.lblCoordinates.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.lblCoordinates.Size = new System.Drawing.Size(1634, 52);
            this.lblCoordinates.TabIndex = 5;
            this.lblCoordinates.Text = "Die X/Y: - / LeftBottom: - / USECODE: -\r\n마우스 Wafer 중심 기준: -";
            this.lblCoordinates.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblStatus
            //
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Location = new System.Drawing.Point(0, 952);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.lblStatus.Size = new System.Drawing.Size(1634, 32);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "Device 를 선택하세요.";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.m_wMap.Location = new System.Drawing.Point(440, 88);
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
            this.m_wMap.Size = new System.Drawing.Size(714, 812);
            this.m_wMap.SkipDieColor = System.Drawing.Color.Yellow;
            this.m_wMap.TabIndex = 2;
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
            this.m_wMap.OnChangeCurrentDie += new DACrux.Map.ChangeCurrentDie(this.m_wMap_OnChangeCurrentDie);
            this.m_wMap.OnChangePosition += new DACrux.Map.ChangePosition(this.m_wMap_OnChangePosition);
            this.m_wMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.m_wMap_MouseMove);
            this.m_wMap.MouseLeave += new System.EventHandler(this.m_wMap_MouseLeave);
            //
            // frmStandardMapView
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1634, 984);
            this.Controls.Add(this.m_wMap);
            this.Controls.Add(this.lblDieCount);
            this.Controls.Add(this.tabRight);
            this.Controls.Add(this.pnlList);
            this.Controls.Add(this.topBar);
            this.Controls.Add(this.lblCoordinates);
            this.Controls.Add(this.lblStatus);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.Margin = new System.Windows.Forms.Padding(9);
            this.MinimumSize = new System.Drawing.Size(1100, 620);
            this.Name = "frmStandardMapView";
            this.Text = "Wafer Map 조회";
            this.Load += new System.EventHandler(this.frmStandardMapView_Load);
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.pnlList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdMapList)).EndInit();
            this.tabRight.ResumeLayout(false);
            this.tabInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdInfo)).EndInit();
            this.tabCheck.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdCheck)).EndInit();
            this.tabDies.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDies)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblListCount;
        private System.Windows.Forms.Label lblHelp;
        private System.Windows.Forms.Panel pnlList;
        private System.Windows.Forms.DataGridView grdMapList;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMapId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWafer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNetDie;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDirection;
        private System.Windows.Forms.TabControl tabRight;
        private System.Windows.Forms.TabPage tabInfo;
        private System.Windows.Forms.DataGridView grdInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInfoName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInfoValue;
        private System.Windows.Forms.TabPage tabCheck;
        private System.Windows.Forms.DataGridView grdCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCheckName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCheckStored;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCheckDie;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCheckResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCheckDescription;
        private System.Windows.Forms.TabPage tabDies;
        private System.Windows.Forms.DataGridView grdDies;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDieX;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDieY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDieLbX;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDieLbY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDieUseCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDieInside;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDieNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDieStoredNum;
        private System.Windows.Forms.CheckBox chkDieNumber;
        private System.Windows.Forms.Label lblDieCount;
        private System.Windows.Forms.Label lblCoordinates;
        private System.Windows.Forms.Label lblStatus;
        private DACrux.TEST.ENGUI.StandardMapEditMap m_wMap;
    }
}
