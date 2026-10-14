namespace NexplantQMS.GdsMap
{
    /// <summary>GDS Map 조회 화면의 컨트롤 생성과 배치를 정의한다. (설계: 문서/2026-10-02_GDS_Map_조회화면_설계.md 3.2절)</summary>
    partial class GdsMapViewForm
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>화면이 닫힐 때 컨트롤과 OpenGL 자원을 함께 해제한다.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 레이아웃 정리(2026-10-12, 컨셉 A "도구 막대 한 줄 + 오른쪽 탭 패널", Map 정보 Label 제거).
        ///  위: 도구 막대 한 줄(Device / 조회 / 중지 | 보기 기준 | Label 검색(포함, 대소문자 무시) | [Map ▾] = Sub Device / 원점 표시 / Map 삭제). 창이 좁으면 다음 줄로 넘어간다.
        ///  가운데: Chain 작업 줄(도면을 조회한 뒤에만 보임. 왼쪽 = Input / 선택 적용·취소 / 후보 경로 / 예시·유사 묶음, 오른쪽 끝 = 경로 추가·제외) + 도면. 상태 문구는 아래 상태 줄 한 곳에만 보인다 (2026-10-13).
        ///  Label 일치 방식 콤보 제거(포함 고정) / 버튼 순서 번호 제거 / Output 지정 숨김(기능 유지) / 경로 추가·제외 오른쪽 끝(2026-10-12).
        ///  도면 우클릭 메뉴(전체 보기)는 제거했다(2026-10-13).
        ///  오른쪽: 탭 [Chain](목록 + 유사 묶음은 찾았을 때만) / [Layer]. 아래: 진행률 / 상태 / 마우스 좌표.
        ///  Map 정보(파일 / 건수 / 저장 기준 / Sub Device)는 Device 콤보 툴팁으로 옮겼다.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlCondition = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDevice = new System.Windows.Forms.Label();
            this.cboDevice = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.sepTool1 = new System.Windows.Forms.Label();
            this.lblViewSide = new System.Windows.Forms.Label();
            this.cboViewSide = new System.Windows.Forms.ComboBox();
            this.sepTool2 = new System.Windows.Forms.Label();
            this.lblLabel = new System.Windows.Forms.Label();
            this.txtLabel = new System.Windows.Forms.TextBox();
            this.btnLabelSearch = new System.Windows.Forms.Button();
            this.btnLabelAll = new System.Windows.Forms.Button();
            this.cboLabelResults = new System.Windows.Forms.ComboBox();
            this.sepTool3 = new System.Windows.Forms.Label();
            this.btnMapMenu = new System.Windows.Forms.Button();
            this.mnuMap = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuSubDevice = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOriginCross = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMapSep = new System.Windows.Forms.ToolStripSeparator();
            this.mnuDeleteMap = new System.Windows.Forms.ToolStripMenuItem();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.tblChainBar = new System.Windows.Forms.TableLayoutPanel();
            this.chainFirstRow = new System.Windows.Forms.FlowLayoutPanel();
            this.lblChainBarTitle = new System.Windows.Forms.Label();
            this._btnChainInput = new System.Windows.Forms.Button();
            this._btnChainOutput = new System.Windows.Forms.Button();
            this._btnChainApply = new System.Windows.Forms.Button();
            this._btnChainCancel = new System.Windows.Forms.Button();
            this.sepChain1 = new System.Windows.Forms.Label();
            this.chainCellSizeLabel = new System.Windows.Forms.Label();
            this._numChainCellSize = new System.Windows.Forms.NumericUpDown();
            this._btnChainTrace = new System.Windows.Forms.Button();
            this.sepChain2 = new System.Windows.Forms.Label();
            this._btnChainExample = new System.Windows.Forms.Button();
            this._btnChainFindSimilar = new System.Windows.Forms.Button();
            this.chainEditRow = new System.Windows.Forms.FlowLayoutPanel();
            this._btnChainAdd = new System.Windows.Forms.Button();
            this._btnChainRemove = new System.Windows.Forms.Button();
            this.tabRight = new System.Windows.Forms.TabControl();
            this.tabChain = new System.Windows.Forms.TabPage();
            this.chainListPanel = new System.Windows.Forms.Panel();
            this._chainListMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._chainColorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chainListButtons = new System.Windows.Forms.FlowLayoutPanel();
            this._btnChainNew = new System.Windows.Forms.Button();
            this._btnChainRename = new System.Windows.Forms.Button();
            this._btnChainDelete = new System.Windows.Forms.Button();
            this._btnChainDbSave = new System.Windows.Forms.Button();
            this.chainSimilarListPanel = new System.Windows.Forms.Panel();
            this._chainSimilarList = new System.Windows.Forms.CheckedListBox();
            this.chainSimilarButtons = new System.Windows.Forms.FlowLayoutPanel();
            this._btnChainApplySimilar = new System.Windows.Forms.Button();
            this._btnChainClearSimilar = new System.Windows.Forms.Button();
            this._btnChainUndoSimilar = new System.Windows.Forms.Button();
            this.chainSimilarListTitle = new System.Windows.Forms.Label();
            this.tabLayer = new System.Windows.Forms.TabPage();
            this.pnlLayerButtons = new System.Windows.Forms.Panel();
            this.btnLayerNone = new System.Windows.Forms.Button();
            this.btnLayerAll = new System.Windows.Forms.Button();
            this.lblLayerTitle = new System.Windows.Forms.Label();
            this.statusStripView = new System.Windows.Forms.StatusStrip();
            this.progressLoad = new System.Windows.Forms.ToolStripProgressBar();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCoordinate = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTipView = new System.Windows.Forms.ToolTip(this.components);
            this.map = new NexplantQMS.GdsMap.GdsMapControl();
            this._chainList = new NexplantQMS.GdsMap.LayerColorCheckedListBox();
            this.chkLayerItems = new NexplantQMS.GdsMap.LayerColorCheckedListBox();
            this.pnlCondition.SuspendLayout();
            this.mnuMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.tblChainBar.SuspendLayout();
            this.chainFirstRow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numChainCellSize)).BeginInit();
            this.chainEditRow.SuspendLayout();
            this.tabRight.SuspendLayout();
            this.tabChain.SuspendLayout();
            this.chainListPanel.SuspendLayout();
            this._chainListMenu.SuspendLayout();
            this.chainListButtons.SuspendLayout();
            this.chainSimilarListPanel.SuspendLayout();
            this.chainSimilarButtons.SuspendLayout();
            this.tabLayer.SuspendLayout();
            this.pnlLayerButtons.SuspendLayout();
            this.statusStripView.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCondition
            // 
            this.pnlCondition.AutoSize = true;
            this.pnlCondition.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlCondition.Controls.Add(this.lblDevice);
            this.pnlCondition.Controls.Add(this.cboDevice);
            this.pnlCondition.Controls.Add(this.btnSearch);
            this.pnlCondition.Controls.Add(this.btnStop);
            this.pnlCondition.Controls.Add(this.sepTool1);
            this.pnlCondition.Controls.Add(this.lblViewSide);
            this.pnlCondition.Controls.Add(this.cboViewSide);
            this.pnlCondition.Controls.Add(this.sepTool2);
            this.pnlCondition.Controls.Add(this.lblLabel);
            this.pnlCondition.Controls.Add(this.txtLabel);
            this.pnlCondition.Controls.Add(this.btnLabelSearch);
            this.pnlCondition.Controls.Add(this.btnLabelAll);
            this.pnlCondition.Controls.Add(this.cboLabelResults);
            this.pnlCondition.Controls.Add(this.sepTool3);
            this.pnlCondition.Controls.Add(this.btnMapMenu);
            this.pnlCondition.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCondition.Location = new System.Drawing.Point(0, 0);
            this.pnlCondition.Name = "pnlCondition";
            this.pnlCondition.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pnlCondition.Size = new System.Drawing.Size(1714, 48);
            this.pnlCondition.TabIndex = 0;
            // 
            // lblDevice
            // 
            this.lblDevice.AutoSize = true;
            this.lblDevice.Location = new System.Drawing.Point(10, 15);
            this.lblDevice.Margin = new System.Windows.Forms.Padding(6, 15, 3, 3);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(62, 18);
            this.lblDevice.TabIndex = 0;
            this.lblDevice.Text = "Device";
            // 
            // cboDevice
            // 
            this.cboDevice.FormattingEnabled = true;
            this.cboDevice.Location = new System.Drawing.Point(78, 11);
            this.cboDevice.Margin = new System.Windows.Forms.Padding(3, 11, 3, 3);
            this.cboDevice.Name = "cboDevice";
            this.cboDevice.Size = new System.Drawing.Size(190, 26);
            this.cboDevice.TabIndex = 1;
            this.toolTipView.SetToolTip(this.cboDevice, "Device를 선택하세요.");
            this.cboDevice.SelectedIndexChanged += new System.EventHandler(this.cboDevice_SelectedIndexChanged);
            this.cboDevice.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboDevice_KeyDown);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(274, 6);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(90, 36);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(370, 6);
            this.btnStop.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(80, 36);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "중지";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // sepTool1
            // 
            this.sepTool1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.sepTool1.Location = new System.Drawing.Point(459, 10);
            this.sepTool1.Margin = new System.Windows.Forms.Padding(6, 10, 6, 10);
            this.sepTool1.Name = "sepTool1";
            this.sepTool1.Size = new System.Drawing.Size(1, 28);
            this.sepTool1.TabIndex = 4;
            // 
            // lblViewSide
            // 
            this.lblViewSide.AutoSize = true;
            this.lblViewSide.Location = new System.Drawing.Point(469, 15);
            this.lblViewSide.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.lblViewSide.Name = "lblViewSide";
            this.lblViewSide.Size = new System.Drawing.Size(44, 18);
            this.lblViewSide.TabIndex = 5;
            this.lblViewSide.Text = "보기";
            // 
            // cboViewSide
            // 
            this.cboViewSide.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboViewSide.FormattingEnabled = true;
            this.cboViewSide.Items.AddRange(new object[] {
            "Front side(원본)",
            "Back side(좌우 반전)"});
            this.cboViewSide.Location = new System.Drawing.Point(519, 11);
            this.cboViewSide.Margin = new System.Windows.Forms.Padding(3, 11, 3, 3);
            this.cboViewSide.Name = "cboViewSide";
            this.cboViewSide.Size = new System.Drawing.Size(200, 26);
            this.cboViewSide.TabIndex = 6;
            this.cboViewSide.SelectedIndexChanged += new System.EventHandler(this.cboViewSide_SelectedIndexChanged);
            // 
            // sepTool2
            // 
            this.sepTool2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.sepTool2.Location = new System.Drawing.Point(728, 10);
            this.sepTool2.Margin = new System.Windows.Forms.Padding(6, 10, 6, 10);
            this.sepTool2.Name = "sepTool2";
            this.sepTool2.Size = new System.Drawing.Size(1, 28);
            this.sepTool2.TabIndex = 7;
            // 
            // lblLabel
            // 
            this.lblLabel.AutoSize = true;
            this.lblLabel.Location = new System.Drawing.Point(738, 15);
            this.lblLabel.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.lblLabel.Name = "lblLabel";
            this.lblLabel.Size = new System.Drawing.Size(50, 18);
            this.lblLabel.TabIndex = 8;
            this.lblLabel.Text = "Label";
            // 
            // txtLabel
            // 
            this.txtLabel.Location = new System.Drawing.Point(794, 10);
            this.txtLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.txtLabel.Name = "txtLabel";
            this.txtLabel.Size = new System.Drawing.Size(180, 28);
            this.txtLabel.TabIndex = 9;
            this.txtLabel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLabel_KeyDown);
            // 
            // btnLabelSearch
            // 
            this.btnLabelSearch.Location = new System.Drawing.Point(980, 6);
            this.btnLabelSearch.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnLabelSearch.Name = "btnLabelSearch";
            this.btnLabelSearch.Size = new System.Drawing.Size(76, 36);
            this.btnLabelSearch.TabIndex = 11;
            this.btnLabelSearch.Text = "검색";
            this.btnLabelSearch.UseVisualStyleBackColor = true;
            this.btnLabelSearch.Click += new System.EventHandler(this.btnLabelSearch_Click);
            // 
            // btnLabelAll
            // 
            this.btnLabelAll.Location = new System.Drawing.Point(1062, 6);
            this.btnLabelAll.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnLabelAll.Name = "btnLabelAll";
            this.btnLabelAll.Size = new System.Drawing.Size(96, 36);
            this.btnLabelAll.TabIndex = 12;
            this.btnLabelAll.Text = "전체 보기";
            this.btnLabelAll.UseVisualStyleBackColor = true;
            this.btnLabelAll.Click += new System.EventHandler(this.btnLabelAll_Click);
            // 
            // cboLabelResults
            // 
            this.cboLabelResults.DropDownHeight = 400;
            this.cboLabelResults.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLabelResults.DropDownWidth = 520;
            this.cboLabelResults.FormattingEnabled = true;
            this.cboLabelResults.IntegralHeight = false;
            this.cboLabelResults.Location = new System.Drawing.Point(1164, 11);
            this.cboLabelResults.Margin = new System.Windows.Forms.Padding(3, 11, 3, 3);
            this.cboLabelResults.Name = "cboLabelResults";
            this.cboLabelResults.Size = new System.Drawing.Size(250, 26);
            this.cboLabelResults.TabIndex = 13;
            this.cboLabelResults.SelectedIndexChanged += new System.EventHandler(this.cboLabelResults_SelectedIndexChanged);
            // 
            // sepTool3
            // 
            this.sepTool3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.sepTool3.Location = new System.Drawing.Point(1423, 10);
            this.sepTool3.Margin = new System.Windows.Forms.Padding(6, 10, 6, 10);
            this.sepTool3.Name = "sepTool3";
            this.sepTool3.Size = new System.Drawing.Size(1, 28);
            this.sepTool3.TabIndex = 15;
            // 
            // btnMapMenu
            // 
            this.btnMapMenu.Location = new System.Drawing.Point(1433, 6);
            this.btnMapMenu.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnMapMenu.Name = "btnMapMenu";
            this.btnMapMenu.Size = new System.Drawing.Size(133, 36);
            this.btnMapMenu.TabIndex = 16;
            this.btnMapMenu.Text = "Map ▾";
            this.btnMapMenu.UseVisualStyleBackColor = true;
            this.btnMapMenu.Click += new System.EventHandler(this.btnMapMenu_Click);
            // 
            // mnuMap
            // 
            this.mnuMap.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.mnuMap.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSubDevice,
            this.mnuOriginCross,
            this.mnuMapSep,
            this.mnuDeleteMap});
            this.mnuMap.Name = "mnuMap";
            this.mnuMap.Size = new System.Drawing.Size(231, 106);
            // 
            // mnuSubDevice
            // 
            this.mnuSubDevice.Enabled = false;
            this.mnuSubDevice.Name = "mnuSubDevice";
            this.mnuSubDevice.Size = new System.Drawing.Size(230, 32);
            this.mnuSubDevice.Text = "Sub Device 설정...";
            this.mnuSubDevice.Visible = false;
            this.mnuSubDevice.Click += new System.EventHandler(this.mnuSubDevice_Click);
            // 
            // mnuOriginCross
            // 
            this.mnuOriginCross.Checked = true;
            this.mnuOriginCross.CheckOnClick = true;
            this.mnuOriginCross.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnuOriginCross.Name = "mnuOriginCross";
            this.mnuOriginCross.Size = new System.Drawing.Size(230, 32);
            this.mnuOriginCross.Text = "원점 표시";
            this.mnuOriginCross.CheckedChanged += new System.EventHandler(this.mnuOriginCross_CheckedChanged);
            // 
            // mnuMapSep
            // 
            this.mnuMapSep.Name = "mnuMapSep";
            this.mnuMapSep.Size = new System.Drawing.Size(227, 6);
            this.mnuMapSep.Visible = false;
            // 
            // mnuDeleteMap
            // 
            this.mnuDeleteMap.ForeColor = System.Drawing.Color.Firebrick;
            this.mnuDeleteMap.Name = "mnuDeleteMap";
            this.mnuDeleteMap.Size = new System.Drawing.Size(230, 32);
            this.mnuDeleteMap.Text = "Map 삭제...";
            this.mnuDeleteMap.Visible = false;
            this.mnuDeleteMap.Click += new System.EventHandler(this.mnuDeleteMap_Click);
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitMain.Location = new System.Drawing.Point(0, 48);
            this.splitMain.Margin = new System.Windows.Forms.Padding(4);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.map);
            this.splitMain.Panel1.Controls.Add(this.tblChainBar);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.tabRight);
            this.splitMain.Size = new System.Drawing.Size(1714, 1071);
            this.splitMain.SplitterDistance = 1366;
            this.splitMain.SplitterWidth = 6;
            this.splitMain.TabIndex = 1;
            // 
            // tblChainBar
            // 
            this.tblChainBar.AutoSize = true;
            this.tblChainBar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblChainBar.BackColor = System.Drawing.Color.AliceBlue;
            this.tblChainBar.ColumnCount = 2;
            this.tblChainBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblChainBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblChainBar.Controls.Add(this.chainFirstRow, 0, 0);
            this.tblChainBar.Controls.Add(this.chainEditRow, 1, 0);
            this.tblChainBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblChainBar.Location = new System.Drawing.Point(0, 0);
            this.tblChainBar.Name = "tblChainBar";
            this.tblChainBar.RowCount = 1;
            this.tblChainBar.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblChainBar.Size = new System.Drawing.Size(1366, 96);
            this.tblChainBar.TabIndex = 0;
            this.tblChainBar.Visible = false;
            // 
            // chainFirstRow
            // 
            this.chainFirstRow.AutoSize = true;
            this.chainFirstRow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.chainFirstRow.Controls.Add(this.lblChainBarTitle);
            this.chainFirstRow.Controls.Add(this._btnChainInput);
            this.chainFirstRow.Controls.Add(this._btnChainOutput);
            this.chainFirstRow.Controls.Add(this._btnChainApply);
            this.chainFirstRow.Controls.Add(this._btnChainCancel);
            this.chainFirstRow.Controls.Add(this.sepChain1);
            this.chainFirstRow.Controls.Add(this.chainCellSizeLabel);
            this.chainFirstRow.Controls.Add(this._numChainCellSize);
            this.chainFirstRow.Controls.Add(this._btnChainTrace);
            this.chainFirstRow.Controls.Add(this.sepChain2);
            this.chainFirstRow.Controls.Add(this._btnChainExample);
            this.chainFirstRow.Controls.Add(this._btnChainFindSimilar);
            this.chainFirstRow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chainFirstRow.Location = new System.Drawing.Point(0, 0);
            this.chainFirstRow.Margin = new System.Windows.Forms.Padding(0);
            this.chainFirstRow.Name = "chainFirstRow";
            this.chainFirstRow.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.chainFirstRow.Size = new System.Drawing.Size(1156, 96);
            this.chainFirstRow.TabIndex = 0;
            // 
            // lblChainBarTitle
            // 
            this.lblChainBarTitle.AutoSize = true;
            this.lblChainBarTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblChainBarTitle.Location = new System.Drawing.Point(10, 14);
            this.lblChainBarTitle.Margin = new System.Windows.Forms.Padding(6, 14, 10, 3);
            this.lblChainBarTitle.MinimumSize = new System.Drawing.Size(130, 0);
            this.lblChainBarTitle.Name = "lblChainBarTitle";
            this.lblChainBarTitle.Size = new System.Drawing.Size(144, 25);
            this.lblChainBarTitle.TabIndex = 0;
            this.lblChainBarTitle.Text = "Chain 선택 필요";
            // 
            // _btnChainInput
            // 
            this._btnChainInput.AutoSize = true;
            this._btnChainInput.Location = new System.Drawing.Point(167, 6);
            this._btnChainInput.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this._btnChainInput.Name = "_btnChainInput";
            this._btnChainInput.Size = new System.Drawing.Size(98, 36);
            this._btnChainInput.TabIndex = 1;
            this._btnChainInput.Text = "Input 지정";
            this._btnChainInput.UseVisualStyleBackColor = true;
            this._btnChainInput.Click += new System.EventHandler(this.BtnChainInput_Click);
            // 
            // _btnChainOutput
            // 
            this._btnChainOutput.AutoSize = true;
            this._btnChainOutput.Location = new System.Drawing.Point(271, 6);
            this._btnChainOutput.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this._btnChainOutput.Name = "_btnChainOutput";
            this._btnChainOutput.Size = new System.Drawing.Size(113, 36);
            this._btnChainOutput.TabIndex = 2;
            this._btnChainOutput.Text = "Output 지정";
            this._btnChainOutput.UseVisualStyleBackColor = true;
            this._btnChainOutput.Visible = false;
            this._btnChainOutput.Click += new System.EventHandler(this.BtnChainOutput_Click);
            // 
            // _btnChainApply
            // 
            this._btnChainApply.AutoSize = true;
            this._btnChainApply.Location = new System.Drawing.Point(390, 6);
            this._btnChainApply.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this._btnChainApply.Name = "_btnChainApply";
            this._btnChainApply.Size = new System.Drawing.Size(96, 36);
            this._btnChainApply.TabIndex = 3;
            this._btnChainApply.Text = "선택 적용";
            this._btnChainApply.UseVisualStyleBackColor = true;
            this._btnChainApply.Click += new System.EventHandler(this.BtnChainApply_Click);
            // 
            // _btnChainCancel
            // 
            this._btnChainCancel.AutoSize = true;
            this._btnChainCancel.Location = new System.Drawing.Point(492, 6);
            this._btnChainCancel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this._btnChainCancel.Name = "_btnChainCancel";
            this._btnChainCancel.Size = new System.Drawing.Size(96, 36);
            this._btnChainCancel.TabIndex = 4;
            this._btnChainCancel.Text = "선택 취소";
            this._btnChainCancel.UseVisualStyleBackColor = true;
            this._btnChainCancel.Click += new System.EventHandler(this.BtnChainCancel_Click);
            // 
            // sepChain1
            // 
            this.sepChain1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.sepChain1.Location = new System.Drawing.Point(597, 10);
            this.sepChain1.Margin = new System.Windows.Forms.Padding(6, 10, 6, 10);
            this.sepChain1.Name = "sepChain1";
            this.sepChain1.Size = new System.Drawing.Size(1, 28);
            this.sepChain1.TabIndex = 5;
            // 
            // chainCellSizeLabel
            // 
            this.chainCellSizeLabel.AutoSize = true;
            this.chainCellSizeLabel.Location = new System.Drawing.Point(607, 15);
            this.chainCellSizeLabel.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.chainCellSizeLabel.Name = "chainCellSizeLabel";
            this.chainCellSizeLabel.Size = new System.Drawing.Size(86, 18);
            this.chainCellSizeLabel.TabIndex = 6;
            this.chainCellSizeLabel.Text = "격자 크기";
            this.chainCellSizeLabel.Visible = false;
            // 
            // _numChainCellSize
            // 
            this._numChainCellSize.DecimalPlaces = 3;
            this._numChainCellSize.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this._numChainCellSize.Location = new System.Drawing.Point(699, 10);
            this._numChainCellSize.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this._numChainCellSize.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this._numChainCellSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this._numChainCellSize.Name = "_numChainCellSize";
            this._numChainCellSize.Size = new System.Drawing.Size(114, 28);
            this._numChainCellSize.TabIndex = 7;
            this._numChainCellSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this._numChainCellSize.Visible = false;
            // 
            // _btnChainTrace
            // 
            this._btnChainTrace.AutoSize = true;
            this._btnChainTrace.Location = new System.Drawing.Point(819, 6);
            this._btnChainTrace.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this._btnChainTrace.Name = "_btnChainTrace";
            this._btnChainTrace.Size = new System.Drawing.Size(138, 36);
            this._btnChainTrace.TabIndex = 8;
            this._btnChainTrace.Text = "후보 경로 찾기";
            this._btnChainTrace.UseVisualStyleBackColor = true;
            this._btnChainTrace.Click += new System.EventHandler(this.BtnChainTrace_Click);
            // 
            // sepChain2
            // 
            this.sepChain2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.sepChain2.Location = new System.Drawing.Point(966, 10);
            this.sepChain2.Margin = new System.Windows.Forms.Padding(6, 10, 6, 10);
            this.sepChain2.Name = "sepChain2";
            this.sepChain2.Size = new System.Drawing.Size(1, 28);
            this.sepChain2.TabIndex = 11;
            // 
            // _btnChainExample
            // 
            this._btnChainExample.AutoSize = true;
            this._btnChainExample.Location = new System.Drawing.Point(976, 6);
            this._btnChainExample.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this._btnChainExample.Name = "_btnChainExample";
            this._btnChainExample.Size = new System.Drawing.Size(138, 36);
            this._btnChainExample.TabIndex = 12;
            this._btnChainExample.Text = "예시 묶음 지정";
            this._btnChainExample.UseVisualStyleBackColor = true;
            this._btnChainExample.Click += new System.EventHandler(this.BtnChainExample_Click);
            // 
            // _btnChainFindSimilar
            // 
            this._btnChainFindSimilar.AutoSize = true;
            this._btnChainFindSimilar.Location = new System.Drawing.Point(7, 54);
            this._btnChainFindSimilar.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this._btnChainFindSimilar.Name = "_btnChainFindSimilar";
            this._btnChainFindSimilar.Size = new System.Drawing.Size(138, 36);
            this._btnChainFindSimilar.TabIndex = 13;
            this._btnChainFindSimilar.Text = "유사 묶음 찾기";
            this._btnChainFindSimilar.UseVisualStyleBackColor = true;
            this._btnChainFindSimilar.Click += new System.EventHandler(this.BtnChainFindSimilar_Click);
            // 
            // chainEditRow
            // 
            this.chainEditRow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chainEditRow.AutoSize = true;
            this.chainEditRow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.chainEditRow.Controls.Add(this._btnChainAdd);
            this.chainEditRow.Controls.Add(this._btnChainRemove);
            this.chainEditRow.Location = new System.Drawing.Point(1156, 0);
            this.chainEditRow.Margin = new System.Windows.Forms.Padding(0);
            this.chainEditRow.Name = "chainEditRow";
            this.chainEditRow.Padding = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.chainEditRow.Size = new System.Drawing.Size(210, 48);
            this.chainEditRow.TabIndex = 1;
            this.chainEditRow.WrapContents = false;
            // 
            // _btnChainAdd
            // 
            this._btnChainAdd.AutoSize = true;
            this._btnChainAdd.Location = new System.Drawing.Point(3, 6);
            this._btnChainAdd.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this._btnChainAdd.Name = "_btnChainAdd";
            this._btnChainAdd.Size = new System.Drawing.Size(96, 36);
            this._btnChainAdd.TabIndex = 9;
            this._btnChainAdd.Text = "경로 추가";
            this._btnChainAdd.UseVisualStyleBackColor = true;
            this._btnChainAdd.Click += new System.EventHandler(this.BtnChainAdd_Click);
            // 
            // _btnChainRemove
            // 
            this._btnChainRemove.AutoSize = true;
            this._btnChainRemove.Location = new System.Drawing.Point(105, 6);
            this._btnChainRemove.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this._btnChainRemove.Name = "_btnChainRemove";
            this._btnChainRemove.Size = new System.Drawing.Size(96, 36);
            this._btnChainRemove.TabIndex = 10;
            this._btnChainRemove.Text = "경로 제외";
            this._btnChainRemove.UseVisualStyleBackColor = true;
            this._btnChainRemove.Click += new System.EventHandler(this.BtnChainRemove_Click);
            // 
            // tabRight
            // 
            this.tabRight.Controls.Add(this.tabChain);
            this.tabRight.Controls.Add(this.tabLayer);
            this.tabRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabRight.Location = new System.Drawing.Point(0, 0);
            this.tabRight.Name = "tabRight";
            this.tabRight.SelectedIndex = 0;
            this.tabRight.Size = new System.Drawing.Size(342, 1071);
            this.tabRight.TabIndex = 0;
            // 
            // tabChain
            // 
            this.tabChain.Controls.Add(this.chainListPanel);
            this.tabChain.Controls.Add(this.chainSimilarListPanel);
            this.tabChain.Location = new System.Drawing.Point(4, 28);
            this.tabChain.Name = "tabChain";
            this.tabChain.Padding = new System.Windows.Forms.Padding(3);
            this.tabChain.Size = new System.Drawing.Size(334, 1039);
            this.tabChain.TabIndex = 0;
            this.tabChain.Text = "Chain";
            this.tabChain.UseVisualStyleBackColor = true;
            // 
            // chainListPanel
            // 
            this.chainListPanel.Controls.Add(this._chainList);
            this.chainListPanel.Controls.Add(this.chainListButtons);
            this.chainListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chainListPanel.Location = new System.Drawing.Point(3, 3);
            this.chainListPanel.Name = "chainListPanel";
            this.chainListPanel.Size = new System.Drawing.Size(328, 733);
            this.chainListPanel.TabIndex = 0;
            // 
            // _chainListMenu
            // 
            this._chainListMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this._chainListMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._chainColorMenuItem});
            this._chainListMenu.Name = "_chainListMenu";
            this._chainListMenu.Size = new System.Drawing.Size(175, 36);
            this._chainListMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ChainListMenu_Opening);
            // 
            // _chainColorMenuItem
            // 
            this._chainColorMenuItem.Name = "_chainColorMenuItem";
            this._chainColorMenuItem.Size = new System.Drawing.Size(174, 32);
            this._chainColorMenuItem.Text = "색상 변경...";
            this._chainColorMenuItem.Click += new System.EventHandler(this.ChainColorMenu_Click);
            // 
            // chainListButtons
            // 
            this.chainListButtons.Controls.Add(this._btnChainNew);
            this.chainListButtons.Controls.Add(this._btnChainRename);
            this.chainListButtons.Controls.Add(this._btnChainDelete);
            this.chainListButtons.Controls.Add(this._btnChainDbSave);
            this.chainListButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.chainListButtons.Location = new System.Drawing.Point(0, 0);
            this.chainListButtons.Name = "chainListButtons";
            this.chainListButtons.Size = new System.Drawing.Size(328, 93);
            this.chainListButtons.TabIndex = 1;
            // 
            // _btnChainNew
            // 
            this._btnChainNew.Location = new System.Drawing.Point(3, 3);
            this._btnChainNew.Name = "_btnChainNew";
            this._btnChainNew.Size = new System.Drawing.Size(156, 36);
            this._btnChainNew.TabIndex = 0;
            this._btnChainNew.Text = "새 Chain";
            this._btnChainNew.UseVisualStyleBackColor = true;
            this._btnChainNew.Click += new System.EventHandler(this.BtnChainNew_Click);
            // 
            // _btnChainRename
            // 
            this._btnChainRename.Location = new System.Drawing.Point(165, 3);
            this._btnChainRename.Name = "_btnChainRename";
            this._btnChainRename.Size = new System.Drawing.Size(156, 36);
            this._btnChainRename.TabIndex = 1;
            this._btnChainRename.Text = "이름 수정";
            this._btnChainRename.UseVisualStyleBackColor = true;
            this._btnChainRename.Click += new System.EventHandler(this.BtnChainRename_Click);
            // 
            // _btnChainDelete
            // 
            this._btnChainDelete.Location = new System.Drawing.Point(3, 45);
            this._btnChainDelete.Name = "_btnChainDelete";
            this._btnChainDelete.Size = new System.Drawing.Size(156, 36);
            this._btnChainDelete.TabIndex = 2;
            this._btnChainDelete.Text = "삭제";
            this._btnChainDelete.UseVisualStyleBackColor = true;
            this._btnChainDelete.Click += new System.EventHandler(this.BtnChainDelete_Click);
            // 
            // _btnChainDbSave
            // 
            this._btnChainDbSave.Location = new System.Drawing.Point(165, 45);
            this._btnChainDbSave.Name = "_btnChainDbSave";
            this._btnChainDbSave.Size = new System.Drawing.Size(156, 36);
            this._btnChainDbSave.TabIndex = 3;
            this._btnChainDbSave.Text = "Chain 저장(DB)";
            this._btnChainDbSave.UseVisualStyleBackColor = true;
            this._btnChainDbSave.Click += new System.EventHandler(this.BtnChainDbSave_Click);
            // 
            // chainSimilarListPanel
            // 
            this.chainSimilarListPanel.Controls.Add(this._chainSimilarList);
            this.chainSimilarListPanel.Controls.Add(this.chainSimilarButtons);
            this.chainSimilarListPanel.Controls.Add(this.chainSimilarListTitle);
            this.chainSimilarListPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.chainSimilarListPanel.Location = new System.Drawing.Point(3, 736);
            this.chainSimilarListPanel.Name = "chainSimilarListPanel";
            this.chainSimilarListPanel.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.chainSimilarListPanel.Size = new System.Drawing.Size(328, 300);
            this.chainSimilarListPanel.TabIndex = 1;
            this.chainSimilarListPanel.Visible = false;
            // 
            // _chainSimilarList
            // 
            this._chainSimilarList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._chainSimilarList.FormattingEnabled = true;
            this._chainSimilarList.HorizontalScrollbar = true;
            this._chainSimilarList.IntegralHeight = false;
            this._chainSimilarList.Location = new System.Drawing.Point(0, 33);
            this._chainSimilarList.Margin = new System.Windows.Forms.Padding(4);
            this._chainSimilarList.Name = "_chainSimilarList";
            this._chainSimilarList.Size = new System.Drawing.Size(328, 183);
            this._chainSimilarList.TabIndex = 1;
            this._chainSimilarList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ChainSimilarList_ItemCheck);
            this._chainSimilarList.SelectedIndexChanged += new System.EventHandler(this.ChainSimilarList_SelectedIndexChanged);
            this._chainSimilarList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ChainSimilarList_MouseDoubleClick);
            // 
            // chainSimilarButtons
            // 
            this.chainSimilarButtons.Controls.Add(this._btnChainApplySimilar);
            this.chainSimilarButtons.Controls.Add(this._btnChainClearSimilar);
            this.chainSimilarButtons.Controls.Add(this._btnChainUndoSimilar);
            this.chainSimilarButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.chainSimilarButtons.Location = new System.Drawing.Point(0, 216);
            this.chainSimilarButtons.Name = "chainSimilarButtons";
            this.chainSimilarButtons.Size = new System.Drawing.Size(328, 84);
            this.chainSimilarButtons.TabIndex = 2;
            // 
            // _btnChainApplySimilar
            // 
            this._btnChainApplySimilar.Location = new System.Drawing.Point(3, 3);
            this._btnChainApplySimilar.Name = "_btnChainApplySimilar";
            this._btnChainApplySimilar.Size = new System.Drawing.Size(156, 36);
            this._btnChainApplySimilar.TabIndex = 0;
            this._btnChainApplySimilar.Text = "체크 묶음 제외";
            this._btnChainApplySimilar.UseVisualStyleBackColor = true;
            this._btnChainApplySimilar.Click += new System.EventHandler(this.BtnChainApplySimilar_Click);
            // 
            // _btnChainClearSimilar
            // 
            this._btnChainClearSimilar.Location = new System.Drawing.Point(165, 3);
            this._btnChainClearSimilar.Name = "_btnChainClearSimilar";
            this._btnChainClearSimilar.Size = new System.Drawing.Size(156, 36);
            this._btnChainClearSimilar.TabIndex = 1;
            this._btnChainClearSimilar.Text = "미리보기 취소";
            this._btnChainClearSimilar.UseVisualStyleBackColor = true;
            this._btnChainClearSimilar.Click += new System.EventHandler(this.BtnChainClearSimilar_Click);
            // 
            // _btnChainUndoSimilar
            // 
            this._btnChainUndoSimilar.Location = new System.Drawing.Point(3, 45);
            this._btnChainUndoSimilar.Name = "_btnChainUndoSimilar";
            this._btnChainUndoSimilar.Size = new System.Drawing.Size(318, 36);
            this._btnChainUndoSimilar.TabIndex = 2;
            this._btnChainUndoSimilar.Text = "직전 제외 되돌리기";
            this._btnChainUndoSimilar.UseVisualStyleBackColor = true;
            this._btnChainUndoSimilar.Click += new System.EventHandler(this.BtnChainUndoSimilar_Click);
            // 
            // chainSimilarListTitle
            // 
            this.chainSimilarListTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.chainSimilarListTitle.Location = new System.Drawing.Point(0, 6);
            this.chainSimilarListTitle.Name = "chainSimilarListTitle";
            this.chainSimilarListTitle.Size = new System.Drawing.Size(328, 27);
            this.chainSimilarListTitle.TabIndex = 0;
            this.chainSimilarListTitle.Text = "유사 묶음";
            this.chainSimilarListTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabLayer
            // 
            this.tabLayer.Controls.Add(this.chkLayerItems);
            this.tabLayer.Controls.Add(this.pnlLayerButtons);
            this.tabLayer.Location = new System.Drawing.Point(4, 28);
            this.tabLayer.Name = "tabLayer";
            this.tabLayer.Padding = new System.Windows.Forms.Padding(3);
            this.tabLayer.Size = new System.Drawing.Size(334, 1039);
            this.tabLayer.TabIndex = 1;
            this.tabLayer.Text = "Layer";
            this.tabLayer.UseVisualStyleBackColor = true;
            // 
            // pnlLayerButtons
            // 
            this.pnlLayerButtons.Controls.Add(this.btnLayerNone);
            this.pnlLayerButtons.Controls.Add(this.btnLayerAll);
            this.pnlLayerButtons.Controls.Add(this.lblLayerTitle);
            this.pnlLayerButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLayerButtons.Location = new System.Drawing.Point(3, 3);
            this.pnlLayerButtons.Name = "pnlLayerButtons";
            this.pnlLayerButtons.Size = new System.Drawing.Size(328, 45);
            this.pnlLayerButtons.TabIndex = 0;
            // 
            // btnLayerNone
            // 
            this.btnLayerNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLayerNone.Location = new System.Drawing.Point(244, 5);
            this.btnLayerNone.Name = "btnLayerNone";
            this.btnLayerNone.Size = new System.Drawing.Size(80, 34);
            this.btnLayerNone.TabIndex = 2;
            this.btnLayerNone.Text = "해제";
            this.btnLayerNone.UseVisualStyleBackColor = true;
            this.btnLayerNone.Click += new System.EventHandler(this.btnLayerNone_Click);
            // 
            // btnLayerAll
            // 
            this.btnLayerAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLayerAll.Location = new System.Drawing.Point(158, 5);
            this.btnLayerAll.Name = "btnLayerAll";
            this.btnLayerAll.Size = new System.Drawing.Size(80, 34);
            this.btnLayerAll.TabIndex = 1;
            this.btnLayerAll.Text = "전체";
            this.btnLayerAll.UseVisualStyleBackColor = true;
            this.btnLayerAll.Click += new System.EventHandler(this.btnLayerAll_Click);
            // 
            // lblLayerTitle
            // 
            this.lblLayerTitle.AutoSize = true;
            this.lblLayerTitle.Location = new System.Drawing.Point(4, 13);
            this.lblLayerTitle.Name = "lblLayerTitle";
            this.lblLayerTitle.Size = new System.Drawing.Size(131, 18);
            this.lblLayerTitle.TabIndex = 0;
            this.lblLayerTitle.Text = "Layer (도형 수)";
            // 
            // statusStripView
            // 
            this.statusStripView.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStripView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressLoad,
            this.lblStatus,
            this.lblCoordinate});
            this.statusStripView.Location = new System.Drawing.Point(0, 1119);
            this.statusStripView.Name = "statusStripView";
            this.statusStripView.Padding = new System.Windows.Forms.Padding(1, 0, 20, 0);
            this.statusStripView.ShowItemToolTips = true;
            this.statusStripView.Size = new System.Drawing.Size(1714, 36);
            this.statusStripView.SizingGrip = false;
            this.statusStripView.TabIndex = 2;
            // 
            // progressLoad
            // 
            this.progressLoad.Name = "progressLoad";
            this.progressLoad.Size = new System.Drawing.Size(229, 28);
            this.progressLoad.Visible = false;
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(1603, 29);
            this.lblStatus.Spring = true;
            this.lblStatus.Text = "대기";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCoordinate
            // 
            this.lblCoordinate.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblCoordinate.Name = "lblCoordinate";
            this.lblCoordinate.Size = new System.Drawing.Size(90, 29);
            this.lblCoordinate.Text = "X: - / Y: -";
            // 
            // toolTipView
            // 
            this.toolTipView.AutoPopDelay = 30000;
            this.toolTipView.InitialDelay = 400;
            this.toolTipView.ReshowDelay = 100;
            // 
            // map
            // 
            this.map.BackColor = System.Drawing.Color.Black;
            this.map.ColorAlpha = 50;
            this.map.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.Location = new System.Drawing.Point(0, 96);
            this.map.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.map.Mode = NexplantQMS.GdsMap.GdsMapControl.ViewMode.View;
            this.map.Name = "map";
            this.map.Size = new System.Drawing.Size(1366, 975);
            this.map.TabIndex = 2;
            this.map.VSync = false;
            this.map.MouseWorldPositionChanged += new System.EventHandler<System.Drawing.PointF>(this.map_MouseWorldPositionChanged);
            this.map.RenderProgressChanged += new System.EventHandler<NexplantQMS.GdsMap.GdsMapRenderProgressChangedEventArgs>(this.map_RenderProgressChanged);
            this.map.FirstFrameMeasured += new System.EventHandler<NexplantQMS.GdsMap.GdsMapLoadMetrics>(this.map_FirstFrameMeasured);
            this.map.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Map_ChainMouseClick);
            this.map.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Map_ChainMouseDown);
            // 
            // _chainList
            // 
            this._chainList.ContextMenuStrip = this._chainListMenu;
            this._chainList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._chainList.HorizontalScrollbar = true;
            this._chainList.IntegralHeight = false;
            this._chainList.Location = new System.Drawing.Point(0, 93);
            this._chainList.Margin = new System.Windows.Forms.Padding(4);
            this._chainList.Name = "_chainList";
            this._chainList.SelectOnRightClick = false;
            this._chainList.Size = new System.Drawing.Size(328, 640);
            this._chainList.TabIndex = 2;
            this._chainList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ChainList_ItemCheck);
            this._chainList.SelectedIndexChanged += new System.EventHandler(this.ChainList_SelectedIndexChanged);
            this._chainList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ChainList_MouseDoubleClick);
            // 
            // chkLayerItems
            // 
            this.chkLayerItems.CheckOnClick = true;
            this.chkLayerItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkLayerItems.FormattingEnabled = true;
            this.chkLayerItems.IntegralHeight = false;
            this.chkLayerItems.Location = new System.Drawing.Point(3, 48);
            this.chkLayerItems.Margin = new System.Windows.Forms.Padding(4);
            this.chkLayerItems.Name = "chkLayerItems";
            this.chkLayerItems.Size = new System.Drawing.Size(328, 988);
            this.chkLayerItems.TabIndex = 1;
            this.chkLayerItems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkLayerItems_ItemCheck);
            this.chkLayerItems.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.chkLayerItems_Format);
            // 
            // GdsMapViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1714, 1155);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.pnlCondition);
            this.Controls.Add(this.statusStripView);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GdsMapViewForm";
            this.Text = "GDS Map 조회";
            this.Shown += new System.EventHandler(this.GdsMapViewForm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GdsMapViewForm_ChainKeyDown);
            this.pnlCondition.ResumeLayout(false);
            this.pnlCondition.PerformLayout();
            this.mnuMap.ResumeLayout(false);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel1.PerformLayout();
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.tblChainBar.ResumeLayout(false);
            this.tblChainBar.PerformLayout();
            this.chainFirstRow.ResumeLayout(false);
            this.chainFirstRow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numChainCellSize)).EndInit();
            this.chainEditRow.ResumeLayout(false);
            this.chainEditRow.PerformLayout();
            this.tabRight.ResumeLayout(false);
            this.tabChain.ResumeLayout(false);
            this.chainListPanel.ResumeLayout(false);
            this._chainListMenu.ResumeLayout(false);
            this.chainListButtons.ResumeLayout(false);
            this.chainSimilarListPanel.ResumeLayout(false);
            this.chainSimilarButtons.ResumeLayout(false);
            this.tabLayer.ResumeLayout(false);
            this.pnlLayerButtons.ResumeLayout(false);
            this.pnlLayerButtons.PerformLayout();
            this.statusStripView.ResumeLayout(false);
            this.statusStripView.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        // 위 도구 막대
        private System.Windows.Forms.FlowLayoutPanel pnlCondition;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.ComboBox cboDevice;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label sepTool1;
        private System.Windows.Forms.Label lblViewSide;
        private System.Windows.Forms.ComboBox cboViewSide;
        private System.Windows.Forms.Label sepTool2;
        private System.Windows.Forms.Label lblLabel;
        private System.Windows.Forms.TextBox txtLabel;
        private System.Windows.Forms.Button btnLabelSearch;
        private System.Windows.Forms.Button btnLabelAll;
        private System.Windows.Forms.ComboBox cboLabelResults;
        private System.Windows.Forms.Label sepTool3;
        private System.Windows.Forms.Button btnMapMenu;
        private System.Windows.Forms.ContextMenuStrip mnuMap;
        private System.Windows.Forms.ToolStripMenuItem mnuSubDevice;
        private System.Windows.Forms.ToolStripMenuItem mnuOriginCross;
        private System.Windows.Forms.ToolStripSeparator mnuMapSep;
        private System.Windows.Forms.ToolStripMenuItem mnuDeleteMap;
        private System.Windows.Forms.ToolTip toolTipView;
        // 가운데: Chain 작업 줄 + 안내 + 도면 / 오른쪽 탭
        private System.Windows.Forms.SplitContainer splitMain;
        private NexplantQMS.GdsMap.GdsMapControl map;
        private System.Windows.Forms.TabControl tabRight;
        private System.Windows.Forms.TabPage tabChain;
        private System.Windows.Forms.TabPage tabLayer;
        private System.Windows.Forms.Panel pnlLayerButtons;
        private System.Windows.Forms.Label lblLayerTitle;
        private System.Windows.Forms.Button btnLayerAll;
        private System.Windows.Forms.Button btnLayerNone;
        private NexplantQMS.GdsMap.LayerColorCheckedListBox chkLayerItems;
        private System.Windows.Forms.StatusStrip statusStripView;
        private System.Windows.Forms.ToolStripProgressBar progressLoad;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblCoordinate;
        // Chain 설정 컨트롤(2026-10-06 Setup 화면에서 이동). 동작은 GdsMapViewForm_Chain*.cs partial에서 연결한다.
        private System.Windows.Forms.Panel chainListPanel;
        private System.Windows.Forms.FlowLayoutPanel chainListButtons;
        private NexplantQMS.GdsMap.LayerColorCheckedListBox _chainList;
        private System.Windows.Forms.ContextMenuStrip _chainListMenu;
        private System.Windows.Forms.ToolStripMenuItem _chainColorMenuItem;
        private System.Windows.Forms.Panel chainSimilarListPanel;
        private System.Windows.Forms.Label chainSimilarListTitle;
        private System.Windows.Forms.CheckedListBox _chainSimilarList;
        private System.Windows.Forms.FlowLayoutPanel chainSimilarButtons;
        private System.Windows.Forms.TableLayoutPanel tblChainBar;
        private System.Windows.Forms.FlowLayoutPanel chainFirstRow;
        private System.Windows.Forms.FlowLayoutPanel chainEditRow;
        private System.Windows.Forms.Label lblChainBarTitle;
        private System.Windows.Forms.Label sepChain1;
        private System.Windows.Forms.Label sepChain2;
        private System.Windows.Forms.Label chainCellSizeLabel;
        private System.Windows.Forms.NumericUpDown _numChainCellSize;
        private System.Windows.Forms.Button _btnChainInput;
        private System.Windows.Forms.Button _btnChainOutput;
        private System.Windows.Forms.Button _btnChainApply;
        private System.Windows.Forms.Button _btnChainCancel;
        private System.Windows.Forms.Button _btnChainTrace;
        private System.Windows.Forms.Button _btnChainAdd;
        private System.Windows.Forms.Button _btnChainRemove;
        private System.Windows.Forms.Button _btnChainExample;
        private System.Windows.Forms.Button _btnChainFindSimilar;
        private System.Windows.Forms.Button _btnChainClearSimilar;
        private System.Windows.Forms.Button _btnChainApplySimilar;
        private System.Windows.Forms.Button _btnChainUndoSimilar;
        private System.Windows.Forms.Button _btnChainNew;
        private System.Windows.Forms.Button _btnChainRename;
        private System.Windows.Forms.Button _btnChainDelete;
        private System.Windows.Forms.Button _btnChainDbSave;
    }
}
