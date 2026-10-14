namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>GDS × KLARF 연관 분석 화면 배치. 설계: 문서/2026-10-12_GDS_KLARF_연관분석_화면_설계.md 4.2절 (CLAUDE-053)</summary>
    partial class frmGdsKlarfAnalysis
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>화면이 닫힐 때 컨트롤과 메뉴 / 이미지 목록을 함께 해제한다.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 위: 조건 줄(Wafer / GDS Device / 보기 기준 / 허용오차 / Defect 크기 / Class / Chain / 좌표 보정 + [분석])
        /// 상세 화면은 Map 에서 Chip 선택 후 우클릭 [Detail Analysis] 또는 더블클릭으로 연다(2026-10-12 사용자 지시, [상세 보기] 버튼 제거)
        /// 가운데: 왼쪽 Wafer Map / 오른쪽 위 색 구간 표, 오른쪽 아래 Die · Chain · Defect · Image 탭
        /// 아래: 상태줄(분석 기준 / 마우스 Die 정보)
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGdsKlarfAnalysis));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblWafer = new System.Windows.Forms.Label();
            this.cboWafer = new System.Windows.Forms.ComboBox();
            this.lblDevice = new System.Windows.Forms.Label();
            this.cboDevice = new System.Windows.Forms.ComboBox();
            this.lblSide = new System.Windows.Forms.Label();
            this.cboSide = new System.Windows.Forms.ComboBox();
            this.chkDieIndex = new System.Windows.Forms.CheckBox();
            this.lblTolerance = new System.Windows.Forms.Label();
            this.numTolerance = new System.Windows.Forms.NumericUpDown();
            this.chkDefectSize = new System.Windows.Forms.CheckBox();
            this.btnClass = new System.Windows.Forms.Button();
            this.btnChain = new System.Windows.Forms.Button();
            this.lblOffset = new System.Windows.Forms.Label();
            this.numOffsetX = new System.Windows.Forms.NumericUpDown();
            this.numOffsetY = new System.Windows.Forms.NumericUpDown();
            this.btnAnalyze = new System.Windows.Forms.Button();
            this.menuClass = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuChain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.map = new DACrux.SEMDMS.ENGUI.GdsKlarfAnalysisMap();
            this.splitRight = new System.Windows.Forms.SplitContainer();
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
            this.tabList = new System.Windows.Forms.TabControl();
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
            this.tabImage = new System.Windows.Forms.TabPage();
            this.flowImage = new System.Windows.Forms.FlowLayoutPanel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTolerance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOffsetX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOffsetY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitRight)).BeginInit();
            this.splitRight.Panel1.SuspendLayout();
            this.splitRight.Panel2.SuspendLayout();
            this.splitRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBand)).BeginInit();
            this.pnlBandHead.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numThreshold)).BeginInit();
            this.tabList.SuspendLayout();
            this.tabDie.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDie)).BeginInit();
            this.tabChain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChain)).BeginInit();
            this.pnlChainHead.SuspendLayout();
            this.tabDefect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDefect)).BeginInit();
            this.pnlDefectHead.SuspendLayout();
            this.tabImage.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlTop
            //
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(242)))), ((int)(((byte)(250)))));
            this.pnlTop.Controls.Add(this.lblWafer);
            this.pnlTop.Controls.Add(this.cboWafer);
            this.pnlTop.Controls.Add(this.lblDevice);
            this.pnlTop.Controls.Add(this.cboDevice);
            this.pnlTop.Controls.Add(this.lblSide);
            this.pnlTop.Controls.Add(this.cboSide);
            this.pnlTop.Controls.Add(this.chkDieIndex);
            this.pnlTop.Controls.Add(this.lblTolerance);
            this.pnlTop.Controls.Add(this.numTolerance);
            this.pnlTop.Controls.Add(this.chkDefectSize);
            this.pnlTop.Controls.Add(this.btnClass);
            this.pnlTop.Controls.Add(this.btnChain);
            this.pnlTop.Controls.Add(this.lblOffset);
            this.pnlTop.Controls.Add(this.numOffsetX);
            this.pnlTop.Controls.Add(this.numOffsetY);
            this.pnlTop.Controls.Add(this.btnAnalyze);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1400, 78);
            this.pnlTop.TabIndex = 0;
            //
            // lblWafer
            //
            this.lblWafer.AutoSize = true;
            this.lblWafer.Location = new System.Drawing.Point(10, 14);
            this.lblWafer.Name = "lblWafer";
            this.lblWafer.Size = new System.Drawing.Size(39, 12);
            this.lblWafer.TabIndex = 0;
            this.lblWafer.Text = "Wafer";
            //
            // cboWafer
            //
            this.cboWafer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWafer.Location = new System.Drawing.Point(58, 10);
            this.cboWafer.Name = "cboWafer";
            this.cboWafer.Size = new System.Drawing.Size(340, 20);
            this.cboWafer.TabIndex = 1;
            this.toolTip.SetToolTip(this.cboWafer, "왼쪽 Wafer 조회에서 [Search] 한 Wafer 목록. 1장씩 분석한다.");
            this.cboWafer.SelectedIndexChanged += new System.EventHandler(this.cboWafer_SelectedIndexChanged);
            //
            // lblDevice
            //
            this.lblDevice.AutoSize = true;
            this.lblDevice.Location = new System.Drawing.Point(412, 14);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(70, 12);
            this.lblDevice.TabIndex = 2;
            this.lblDevice.Text = "GDS Device";
            //
            // cboDevice
            //
            this.cboDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDevice.Location = new System.Drawing.Point(486, 10);
            this.cboDevice.Name = "cboDevice";
            this.cboDevice.Size = new System.Drawing.Size(160, 20);
            this.cboDevice.TabIndex = 3;
            this.toolTip.SetToolTip(this.cboDevice, "저장 완료(READY)된 GDS Map. KLARF Product 와 같은 이름이 있으면 자동 선택한다.");
            this.cboDevice.SelectedIndexChanged += new System.EventHandler(this.cboDevice_SelectedIndexChanged);
            //
            // lblSide
            //
            this.lblSide.AutoSize = true;
            this.lblSide.Location = new System.Drawing.Point(660, 14);
            this.lblSide.Name = "lblSide";
            this.lblSide.Size = new System.Drawing.Size(53, 12);
            this.lblSide.TabIndex = 4;
            this.lblSide.Text = "보기 기준";
            //
            // cboSide
            //
            this.cboSide.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSide.Items.AddRange(new object[] {
            "Back side",
            "Front side"});
            this.cboSide.Location = new System.Drawing.Point(718, 10);
            this.cboSide.Name = "cboSide";
            this.cboSide.Size = new System.Drawing.Size(110, 20);
            this.cboSide.TabIndex = 5;
            this.toolTip.SetToolTip(this.cboSide, "GDS 를 이 면 기준으로 맞춘다(Back = GDS 좌우 반전). KLARF 좌표는 바꾸지 않는다.");
            this.cboSide.SelectedIndexChanged += new System.EventHandler(this.Condition_Changed);
            //
            // chkDieIndex
            //
            this.chkDieIndex.AutoSize = true;
            this.chkDieIndex.Location = new System.Drawing.Point(844, 12);
            this.chkDieIndex.Name = "chkDieIndex";
            this.chkDieIndex.Size = new System.Drawing.Size(104, 16);
            this.chkDieIndex.TabIndex = 6;
            this.chkDieIndex.Text = "Die Index 표시";
            this.chkDieIndex.UseVisualStyleBackColor = true;
            this.chkDieIndex.CheckedChanged += new System.EventHandler(this.chkDieIndex_CheckedChanged);
            //
            // lblTolerance
            //
            this.lblTolerance.AutoSize = true;
            this.lblTolerance.Location = new System.Drawing.Point(10, 49);
            this.lblTolerance.Name = "lblTolerance";
            this.lblTolerance.Size = new System.Drawing.Size(80, 12);
            this.lblTolerance.TabIndex = 7;
            this.lblTolerance.Text = "허용오차(µm)";
            //
            // numTolerance
            //
            this.numTolerance.DecimalPlaces = 1;
            this.numTolerance.Location = new System.Drawing.Point(96, 45);
            this.numTolerance.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numTolerance.Name = "numTolerance";
            this.numTolerance.Size = new System.Drawing.Size(70, 21);
            this.numTolerance.TabIndex = 8;
            this.numTolerance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip.SetToolTip(this.numTolerance, "Defect 사각형을 상하좌우로 넓히는 값(회의록의 Offset). 고객 값 확정 전 기본 5 µm.");
            this.numTolerance.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numTolerance.ValueChanged += new System.EventHandler(this.Condition_Changed);
            //
            // chkDefectSize
            //
            this.chkDefectSize.AutoSize = true;
            this.chkDefectSize.Checked = true;
            this.chkDefectSize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDefectSize.Location = new System.Drawing.Point(180, 47);
            this.chkDefectSize.Name = "chkDefectSize";
            this.chkDefectSize.Size = new System.Drawing.Size(112, 16);
            this.chkDefectSize.TabIndex = 9;
            this.chkDefectSize.Text = "Defect 크기 반영";
            this.toolTip.SetToolTip(this.chkDefectSize, "체크: XSIZE x YSIZE 사각형 / 해제: Defect 중심점만");
            this.chkDefectSize.UseVisualStyleBackColor = true;
            this.chkDefectSize.CheckedChanged += new System.EventHandler(this.Condition_Changed);
            //
            // btnClass
            //
            this.btnClass.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClass.BackColor = System.Drawing.Color.White;
            this.btnClass.Location = new System.Drawing.Point(304, 43);
            this.btnClass.Name = "btnClass";
            this.btnClass.Size = new System.Drawing.Size(170, 25);
            this.btnClass.TabIndex = 10;
            this.btnClass.Text = "Class: 전체 ▼";
            this.btnClass.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.btnClass, "분석할 Defect Class 선택");
            this.btnClass.UseVisualStyleBackColor = false;
            this.btnClass.Click += new System.EventHandler(this.btnClass_Click);
            //
            // btnChain
            //
            this.btnChain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChain.BackColor = System.Drawing.Color.White;
            this.btnChain.Location = new System.Drawing.Point(484, 43);
            this.btnChain.Name = "btnChain";
            this.btnChain.Size = new System.Drawing.Size(210, 25);
            this.btnChain.TabIndex = 11;
            this.btnChain.Text = "Chain: 전체 ▼";
            this.btnChain.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.btnChain, "비교할 Chain 선택 (저장 완료 READY Chain 만)");
            this.btnChain.UseVisualStyleBackColor = false;
            this.btnChain.Click += new System.EventHandler(this.btnChain_Click);
            //
            // lblOffset
            //
            this.lblOffset.AutoSize = true;
            this.lblOffset.Location = new System.Drawing.Point(708, 49);
            this.lblOffset.Name = "lblOffset";
            this.lblOffset.Size = new System.Drawing.Size(117, 12);
            this.lblOffset.TabIndex = 12;
            this.lblOffset.Text = "좌표 보정 X / Y(µm)";
            //
            // numOffsetX
            //
            this.numOffsetX.DecimalPlaces = 1;
            this.numOffsetX.Location = new System.Drawing.Point(830, 45);
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
            this.numOffsetX.Size = new System.Drawing.Size(70, 21);
            this.numOffsetX.TabIndex = 13;
            this.numOffsetX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip.SetToolTip(this.numOffsetX, "KLARF -> GDS 좌표 보정(보기 기준 좌표에 더함). 연구소 좌표 대응표 전까지 0.");
            this.numOffsetX.ValueChanged += new System.EventHandler(this.Condition_Changed);
            //
            // numOffsetY
            //
            this.numOffsetY.DecimalPlaces = 1;
            this.numOffsetY.Location = new System.Drawing.Point(904, 45);
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
            this.numOffsetY.Size = new System.Drawing.Size(70, 21);
            this.numOffsetY.TabIndex = 14;
            this.numOffsetY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip.SetToolTip(this.numOffsetY, "KLARF -> GDS 좌표 보정(보기 기준 좌표에 더함). 연구소 좌표 대응표 전까지 0.");
            this.numOffsetY.ValueChanged += new System.EventHandler(this.Condition_Changed);
            //
            // btnAnalyze
            //
            this.btnAnalyze.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAnalyze.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(238)))), ((int)(((byte)(252)))));
            this.btnAnalyze.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(110)))), ((int)(((byte)(170)))));
            this.btnAnalyze.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnalyze.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnAnalyze.Image = ((System.Drawing.Image)(resources.GetObject("btnAnalyze.Image")));
            this.btnAnalyze.Location = new System.Drawing.Point(1274, 8);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Size = new System.Drawing.Size(116, 62);
            this.btnAnalyze.TabIndex = 15;
            this.btnAnalyze.Text = "분석";
            this.btnAnalyze.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolTip.SetToolTip(this.btnAnalyze, "선택한 Wafer 를 GDS Chain 영역과 비교해 Die 별로 색을 칠한다");
            this.btnAnalyze.UseVisualStyleBackColor = false;
            this.btnAnalyze.Click += new System.EventHandler(this.btnAnalyze_Click);
            //
            // menuClass
            //
            this.menuClass.Name = "menuClass";
            this.menuClass.Size = new System.Drawing.Size(61, 4);
            this.menuClass.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.CheckMenu_Closing);
            //
            // menuChain
            //
            this.menuChain.Name = "menuChain";
            this.menuChain.Size = new System.Drawing.Size(61, 4);
            this.menuChain.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.CheckMenu_Closing);
            //
            // splitMain
            //
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 78);
            this.splitMain.Name = "splitMain";
            //
            // splitMain.Panel1
            //
            this.splitMain.Panel1.Controls.Add(this.map);
            //
            // splitMain.Panel2
            //
            this.splitMain.Panel2.Controls.Add(this.splitRight);
            this.splitMain.Size = new System.Drawing.Size(1400, 776);
            this.splitMain.SplitterDistance = 820;
            this.splitMain.TabIndex = 1;
            //
            // map
            //
            this.map.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.map.Cursor = System.Windows.Forms.Cursors.Cross;
            this.map.DefectSize = 7F;
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.DrawDefects = "ALL";
            this.map.Location = new System.Drawing.Point(0, 0);
            this.map.Name = "map";
            this.map.PopupMenu = true;
            this.map.ShowDieIndexLabel = false;
            this.map.Size = new System.Drawing.Size(820, 776);
            this.map.TabIndex = 0;
            this.map.DieClicked += new System.EventHandler<DACrux.SEMDMS.ENGUI.GdsKlarfDieEventArgs>(this.map_DieClicked);
            this.map.DieDoubleClicked += new System.EventHandler<DACrux.SEMDMS.ENGUI.GdsKlarfDieEventArgs>(this.map_DieDoubleClicked);
            this.map.DetailAnalysisRequested += new System.EventHandler<DACrux.SEMDMS.ENGUI.GdsKlarfDieEventArgs>(this.map_DetailAnalysisRequested);
            //
            // splitRight
            //
            this.splitRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitRight.Location = new System.Drawing.Point(0, 0);
            this.splitRight.Name = "splitRight";
            this.splitRight.Orientation = System.Windows.Forms.Orientation.Horizontal;
            //
            // splitRight.Panel1
            //
            this.splitRight.Panel1.Controls.Add(this.dgvBand);
            this.splitRight.Panel1.Controls.Add(this.pnlBandHead);
            //
            // splitRight.Panel2
            //
            this.splitRight.Panel2.Controls.Add(this.tabList);
            this.splitRight.Size = new System.Drawing.Size(576, 776);
            this.splitRight.SplitterDistance = 250;
            this.splitRight.TabIndex = 0;
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
            this.dgvBand.Location = new System.Drawing.Point(0, 30);
            this.dgvBand.MultiSelect = false;
            this.dgvBand.Name = "dgvBand";
            this.dgvBand.RowHeadersVisible = false;
            this.dgvBand.RowTemplate.Height = 23;
            this.dgvBand.Size = new System.Drawing.Size(576, 180);
            this.dgvBand.TabIndex = 1;
            this.toolTip.SetToolTip(this.dgvBand, "FROM / TO 를 고치거나 색 칸을 더블클릭하면 사용자 구간이 된다(Wafer 를 바꿔도 유지)");
            this.dgvBand.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBand_CellDoubleClick);
            this.dgvBand.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBand_CellEndEdit);
            //
            // colBandFrom
            //
            this.colBandFrom.HeaderText = "FROM";
            this.colBandFrom.Name = "colBandFrom";
            //
            // colBandTo
            //
            this.colBandTo.HeaderText = "TO";
            this.colBandTo.Name = "colBandTo";
            //
            // colBandColor
            //
            this.colBandColor.HeaderText = "색";
            this.colBandColor.Name = "colBandColor";
            this.colBandColor.ReadOnly = true;
            //
            // colBandDies
            //
            this.colBandDies.HeaderText = "Die 수";
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
            this.pnlBandHead.Name = "pnlBandHead";
            this.pnlBandHead.Size = new System.Drawing.Size(576, 30);
            this.pnlBandHead.TabIndex = 0;
            //
            // lblBandTitle
            //
            this.lblBandTitle.AutoSize = true;
            this.lblBandTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblBandTitle.Location = new System.Drawing.Point(4, 8);
            this.lblBandTitle.Name = "lblBandTitle";
            this.lblBandTitle.Size = new System.Drawing.Size(178, 15);
            this.lblBandTitle.TabIndex = 0;
            this.lblBandTitle.Text = "색 구간 (Chain 영역 Defect 수)";
            //
            // chkAutoBand
            //
            this.chkAutoBand.AutoSize = true;
            this.chkAutoBand.Checked = true;
            this.chkAutoBand.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoBand.Location = new System.Drawing.Point(222, 7);
            this.chkAutoBand.Name = "chkAutoBand";
            this.chkAutoBand.Size = new System.Drawing.Size(76, 16);
            this.chkAutoBand.TabIndex = 1;
            this.chkAutoBand.Text = "자동 구간";
            this.toolTip.SetToolTip(this.chkAutoBand, "체크: 분석할 때마다 이 Wafer 최대값으로 5구간 이내 자동 나누기");
            this.chkAutoBand.UseVisualStyleBackColor = true;
            this.chkAutoBand.CheckedChanged += new System.EventHandler(this.chkAutoBand_CheckedChanged);
            //
            // lblThreshold
            //
            this.lblThreshold.AutoSize = true;
            this.lblThreshold.Location = new System.Drawing.Point(312, 9);
            this.lblThreshold.Name = "lblThreshold";
            this.lblThreshold.Size = new System.Drawing.Size(53, 12);
            this.lblThreshold.TabIndex = 2;
            this.lblThreshold.Text = "표시 기준";
            //
            // numThreshold
            //
            this.numThreshold.Location = new System.Drawing.Point(370, 5);
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
            this.numThreshold.Size = new System.Drawing.Size(56, 21);
            this.numThreshold.TabIndex = 3;
            this.numThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip.SetToolTip(this.numThreshold, "Chain 영역 Defect 가 이 개수 이상인 Die 만 구간 색, 미만은 연회색");
            this.numThreshold.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numThreshold.ValueChanged += new System.EventHandler(this.numThreshold_ValueChanged);
            //
            // tabList
            //
            this.tabList.Controls.Add(this.tabDie);
            this.tabList.Controls.Add(this.tabChain);
            this.tabList.Controls.Add(this.tabDefect);
            this.tabList.Controls.Add(this.tabImage);
            this.tabList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabList.Location = new System.Drawing.Point(0, 0);
            this.tabList.Name = "tabList";
            this.tabList.SelectedIndex = 0;
            this.tabList.Size = new System.Drawing.Size(576, 562);
            this.tabList.TabIndex = 0;
            this.tabList.SelectedIndexChanged += new System.EventHandler(this.tabList_SelectedIndexChanged);
            //
            // tabDie
            //
            this.tabDie.Controls.Add(this.dgvDie);
            this.tabDie.Location = new System.Drawing.Point(4, 22);
            this.tabDie.Name = "tabDie";
            this.tabDie.Size = new System.Drawing.Size(568, 536);
            this.tabDie.TabIndex = 0;
            this.tabDie.Text = "Die";
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
            this.dgvDie.MultiSelect = false;
            this.dgvDie.Name = "dgvDie";
            this.dgvDie.ReadOnly = true;
            this.dgvDie.RowHeadersVisible = false;
            this.dgvDie.RowTemplate.Height = 23;
            this.dgvDie.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDie.Size = new System.Drawing.Size(568, 536);
            this.dgvDie.TabIndex = 0;
            this.dgvDie.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDie_CellClick);
            this.dgvDie.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDie_CellDoubleClick);
            //
            // tabChain
            //
            this.tabChain.Controls.Add(this.dgvChain);
            this.tabChain.Controls.Add(this.pnlChainHead);
            this.tabChain.Location = new System.Drawing.Point(4, 22);
            this.tabChain.Name = "tabChain";
            this.tabChain.Size = new System.Drawing.Size(568, 536);
            this.tabChain.TabIndex = 1;
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
            this.dgvChain.Location = new System.Drawing.Point(0, 28);
            this.dgvChain.MultiSelect = false;
            this.dgvChain.Name = "dgvChain";
            this.dgvChain.ReadOnly = true;
            this.dgvChain.RowHeadersVisible = false;
            this.dgvChain.RowTemplate.Height = 23;
            this.dgvChain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvChain.Size = new System.Drawing.Size(568, 508);
            this.dgvChain.TabIndex = 1;
            //
            // pnlChainHead
            //
            this.pnlChainHead.Controls.Add(this.chkChainAll);
            this.pnlChainHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlChainHead.Location = new System.Drawing.Point(0, 0);
            this.pnlChainHead.Name = "pnlChainHead";
            this.pnlChainHead.Size = new System.Drawing.Size(568, 28);
            this.pnlChainHead.TabIndex = 0;
            //
            // chkChainAll
            //
            this.chkChainAll.AutoSize = true;
            this.chkChainAll.Location = new System.Drawing.Point(6, 6);
            this.chkChainAll.Name = "chkChainAll";
            this.chkChainAll.Size = new System.Drawing.Size(240, 16);
            this.chkChainAll.TabIndex = 0;
            this.chkChainAll.Text = "Wafer 전체 (해제하면 선택 Die 만 표시)";
            this.chkChainAll.UseVisualStyleBackColor = true;
            this.chkChainAll.CheckedChanged += new System.EventHandler(this.ListFilter_Changed);
            //
            // tabDefect
            //
            this.tabDefect.Controls.Add(this.dgvDefect);
            this.tabDefect.Controls.Add(this.pnlDefectHead);
            this.tabDefect.Location = new System.Drawing.Point(4, 22);
            this.tabDefect.Name = "tabDefect";
            this.tabDefect.Size = new System.Drawing.Size(568, 536);
            this.tabDefect.TabIndex = 2;
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
            this.dgvDefect.Location = new System.Drawing.Point(0, 28);
            this.dgvDefect.MultiSelect = false;
            this.dgvDefect.Name = "dgvDefect";
            this.dgvDefect.ReadOnly = true;
            this.dgvDefect.RowHeadersVisible = false;
            this.dgvDefect.RowTemplate.Height = 23;
            this.dgvDefect.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDefect.Size = new System.Drawing.Size(568, 508);
            this.dgvDefect.TabIndex = 1;
            this.dgvDefect.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDefect_CellDoubleClick);
            //
            // pnlDefectHead
            //
            this.pnlDefectHead.Controls.Add(this.cboDefectScope);
            this.pnlDefectHead.Controls.Add(this.chkDefectAll);
            this.pnlDefectHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDefectHead.Location = new System.Drawing.Point(0, 0);
            this.pnlDefectHead.Name = "pnlDefectHead";
            this.pnlDefectHead.Size = new System.Drawing.Size(568, 28);
            this.pnlDefectHead.TabIndex = 0;
            //
            // cboDefectScope
            //
            this.cboDefectScope.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDefectScope.Items.AddRange(new object[] {
            "Chain 영역",
            "Chain 밖",
            "전체"});
            this.cboDefectScope.Location = new System.Drawing.Point(6, 4);
            this.cboDefectScope.Name = "cboDefectScope";
            this.cboDefectScope.Size = new System.Drawing.Size(110, 20);
            this.cboDefectScope.TabIndex = 0;
            this.cboDefectScope.SelectedIndexChanged += new System.EventHandler(this.ListFilter_Changed);
            //
            // chkDefectAll
            //
            this.chkDefectAll.AutoSize = true;
            this.chkDefectAll.Location = new System.Drawing.Point(128, 6);
            this.chkDefectAll.Name = "chkDefectAll";
            this.chkDefectAll.Size = new System.Drawing.Size(240, 16);
            this.chkDefectAll.TabIndex = 1;
            this.chkDefectAll.Text = "Wafer 전체 (해제하면 선택 Die 만 표시)";
            this.chkDefectAll.UseVisualStyleBackColor = true;
            this.chkDefectAll.CheckedChanged += new System.EventHandler(this.ListFilter_Changed);
            //
            // tabImage
            //
            this.tabImage.Controls.Add(this.flowImage);
            this.tabImage.Location = new System.Drawing.Point(4, 22);
            this.tabImage.Name = "tabImage";
            this.tabImage.Size = new System.Drawing.Size(568, 536);
            this.tabImage.TabIndex = 3;
            this.tabImage.Text = "Image";
            this.tabImage.UseVisualStyleBackColor = true;
            //
            // flowImage
            //
            this.flowImage.AutoScroll = true;
            this.flowImage.BackColor = System.Drawing.Color.White;
            this.flowImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowImage.Location = new System.Drawing.Point(0, 0);
            this.flowImage.Name = "flowImage";
            this.flowImage.Size = new System.Drawing.Size(568, 536);
            this.flowImage.TabIndex = 0;
            //
            // lblStatus
            //
            this.lblStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(242)))), ((int)(((byte)(250)))));
            this.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Location = new System.Drawing.Point(0, 854);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.lblStatus.Size = new System.Drawing.Size(1400, 46);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "왼쪽 Wafer 조회에서 Wafer 를 [Search] 한 뒤 GDS Device / 보기 기준을 고르고 [분석] 하세요.";
            //
            // colorDialog
            //
            this.colorDialog.FullOpen = true;
            //
            // frmGdsKlarfAnalysis
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 900);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pnlTop);
            this.Name = "frmGdsKlarfAnalysis";
            this.Text = "GDS-KLARF 연관 분석";
            this.Load += new System.EventHandler(this.frmGdsKlarfAnalysis_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTolerance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOffsetX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOffsetY)).EndInit();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.splitRight.Panel1.ResumeLayout(false);
            this.splitRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitRight)).EndInit();
            this.splitRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBand)).EndInit();
            this.pnlBandHead.ResumeLayout(false);
            this.pnlBandHead.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numThreshold)).EndInit();
            this.tabList.ResumeLayout(false);
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
            this.tabImage.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblWafer;
        private System.Windows.Forms.ComboBox cboWafer;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.ComboBox cboDevice;
        private System.Windows.Forms.Label lblSide;
        private System.Windows.Forms.ComboBox cboSide;
        private System.Windows.Forms.CheckBox chkDieIndex;
        private System.Windows.Forms.Label lblTolerance;
        private System.Windows.Forms.NumericUpDown numTolerance;
        private System.Windows.Forms.CheckBox chkDefectSize;
        private System.Windows.Forms.Button btnClass;
        private System.Windows.Forms.Button btnChain;
        private System.Windows.Forms.Label lblOffset;
        private System.Windows.Forms.NumericUpDown numOffsetX;
        private System.Windows.Forms.NumericUpDown numOffsetY;
        private System.Windows.Forms.Button btnAnalyze;
        private System.Windows.Forms.ContextMenuStrip menuClass;
        private System.Windows.Forms.ContextMenuStrip menuChain;
        private System.Windows.Forms.SplitContainer splitMain;
        private DACrux.SEMDMS.ENGUI.GdsKlarfAnalysisMap map;
        private System.Windows.Forms.SplitContainer splitRight;
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
        private System.Windows.Forms.TabControl tabList;
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
        private System.Windows.Forms.TabPage tabImage;
        private System.Windows.Forms.FlowLayoutPanel flowImage;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
