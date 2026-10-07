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
        /// 위: 조회 조건(Factory / Device), 왼쪽: Map 정보 + Chain 목록 + 유사 묶음 + Layer 목록,
        /// 오른쪽: Chain 상태 창 / Chain 버튼 2줄 + 도면, 아래: 진행률 / 상태 / 마우스 좌표.
        /// 2026-10-06 CLAUDE-026: Chain 설정 / DB 반영을 이 화면으로 옮겼다(Setup 화면에서 이동, View 7x12 기준 크기로 다시 배치).
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlCondition = new System.Windows.Forms.Panel();
            this.pnlLabelSearch = new System.Windows.Forms.Panel();
            this.lblLabel = new System.Windows.Forms.Label();
            this.txtLabel = new System.Windows.Forms.TextBox();
            this.cboLabelMatch = new System.Windows.Forms.ComboBox();
            this.btnLabelSearch = new System.Windows.Forms.Button();
            this.btnLabelAll = new System.Windows.Forms.Button();
            this.cboLabelResults = new System.Windows.Forms.ComboBox();
            this.lblLabelResult = new System.Windows.Forms.Label();
            this.lblViewSide = new System.Windows.Forms.Label();
            this.cboViewSide = new System.Windows.Forms.ComboBox();
            this.chainCellSizeLabel = new System.Windows.Forms.Label();
            this._numChainCellSize = new System.Windows.Forms.NumericUpDown();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cboDevice = new System.Windows.Forms.ComboBox();
            this.lblDevice = new System.Windows.Forms.Label();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.chkLayerItems = new NexplantQMS.GdsMap.LayerColorCheckedListBox();
            this.pnlLayerButtons = new System.Windows.Forms.Panel();
            this.btnLayerNone = new System.Windows.Forms.Button();
            this.btnLayerAll = new System.Windows.Forms.Button();
            this.lblLayerTitle = new System.Windows.Forms.Label();
            this.chainSimilarListPanel = new System.Windows.Forms.Panel();
            this._chainSimilarList = new System.Windows.Forms.CheckedListBox();
            this.chainSimilarListTitle = new System.Windows.Forms.Label();
            this.chainListPanel = new System.Windows.Forms.Panel();
            this._chainList = new System.Windows.Forms.CheckedListBox();
            this.chainListButtons = new System.Windows.Forms.FlowLayoutPanel();
            this._btnChainNew = new System.Windows.Forms.Button();
            this._btnChainRename = new System.Windows.Forms.Button();
            this._btnChainDelete = new System.Windows.Forms.Button();
            this._btnChainDbSave = new System.Windows.Forms.Button();
            this.chainListTitle = new System.Windows.Forms.Label();
            this.grpMapInfo = new System.Windows.Forms.GroupBox();
            this.lblMapInfo = new System.Windows.Forms.Label();
            this.splitChain = new System.Windows.Forms.SplitContainer();
            this._lblChainSetupStatus = new System.Windows.Forms.RichTextBox();
            this.chainSimilarRow = new System.Windows.Forms.FlowLayoutPanel();
            this._btnChainExample = new System.Windows.Forms.Button();
            this._btnChainFindSimilar = new System.Windows.Forms.Button();
            this._btnChainClearSimilar = new System.Windows.Forms.Button();
            this._btnChainApplySimilar = new System.Windows.Forms.Button();
            this._btnChainUndoSimilar = new System.Windows.Forms.Button();
            this.chainFirstRow = new System.Windows.Forms.FlowLayoutPanel();
            this._btnChainInput = new System.Windows.Forms.Button();
            this._btnChainOutput = new System.Windows.Forms.Button();
            this._btnChainApply = new System.Windows.Forms.Button();
            this._btnChainCancel = new System.Windows.Forms.Button();
            this._btnChainTrace = new System.Windows.Forms.Button();
            this._btnChainAdd = new System.Windows.Forms.Button();
            this._btnChainRemove = new System.Windows.Forms.Button();
            this.map = new NexplantQMS.GdsMap.GdsMapControl();
            this.statusStripView = new System.Windows.Forms.StatusStrip();
            this.progressLoad = new System.Windows.Forms.ToolStripProgressBar();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCoordinate = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlCondition.SuspendLayout();
            this.pnlLabelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numChainCellSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.pnlLayerButtons.SuspendLayout();
            this.chainSimilarListPanel.SuspendLayout();
            this.chainListPanel.SuspendLayout();
            this.chainListButtons.SuspendLayout();
            this.grpMapInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitChain)).BeginInit();
            this.splitChain.Panel1.SuspendLayout();
            this.splitChain.Panel2.SuspendLayout();
            this.splitChain.SuspendLayout();
            this.chainSimilarRow.SuspendLayout();
            this.chainFirstRow.SuspendLayout();
            this.statusStripView.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCondition
            // 
            this.pnlCondition.Controls.Add(this.lblViewSide);
            this.pnlCondition.Controls.Add(this.cboViewSide);
            this.pnlCondition.Controls.Add(this.chainCellSizeLabel);
            this.pnlCondition.Controls.Add(this._numChainCellSize);
            this.pnlCondition.Controls.Add(this.btnStop);
            this.pnlCondition.Controls.Add(this.btnSearch);
            this.pnlCondition.Controls.Add(this.cboDevice);
            this.pnlCondition.Controls.Add(this.lblDevice);
            this.pnlCondition.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCondition.Location = new System.Drawing.Point(0, 0);
            this.pnlCondition.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlCondition.Name = "pnlCondition";
            this.pnlCondition.Size = new System.Drawing.Size(1714, 54);
            this.pnlCondition.TabIndex = 0;
            //
            // pnlLabelSearch (2026-10-07 CLAUDE-032: Label 검색 - 조회 조건 아래 한 줄. 왼쪽 패널은 Chain 목록 / Layer 목록으로 차 있어 결과 목록은 이 줄의 콤보로 둔다)
            //
            this.pnlLabelSearch.Controls.Add(this.lblLabelResult);
            this.pnlLabelSearch.Controls.Add(this.cboLabelResults);
            this.pnlLabelSearch.Controls.Add(this.btnLabelAll);
            this.pnlLabelSearch.Controls.Add(this.btnLabelSearch);
            this.pnlLabelSearch.Controls.Add(this.cboLabelMatch);
            this.pnlLabelSearch.Controls.Add(this.txtLabel);
            this.pnlLabelSearch.Controls.Add(this.lblLabel);
            this.pnlLabelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLabelSearch.Location = new System.Drawing.Point(0, 54);
            this.pnlLabelSearch.Name = "pnlLabelSearch";
            this.pnlLabelSearch.Size = new System.Drawing.Size(1714, 48);
            this.pnlLabelSearch.TabIndex = 3;
            //
            // lblLabel
            //
            this.lblLabel.AutoSize = true;
            this.lblLabel.Location = new System.Drawing.Point(11, 14);
            this.lblLabel.Name = "lblLabel";
            this.lblLabel.Size = new System.Drawing.Size(52, 18);
            this.lblLabel.TabIndex = 0;
            this.lblLabel.Text = "Label";
            //
            // txtLabel
            //
            this.txtLabel.Location = new System.Drawing.Point(83, 9);
            this.txtLabel.Name = "txtLabel";
            this.txtLabel.Size = new System.Drawing.Size(230, 28);
            this.txtLabel.TabIndex = 1;
            this.txtLabel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLabel_KeyDown);
            //
            // cboLabelMatch
            //
            this.cboLabelMatch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLabelMatch.FormattingEnabled = true;
            this.cboLabelMatch.Items.AddRange(new object[] {
            "정확히",
            "시작",
            "포함"});
            this.cboLabelMatch.Location = new System.Drawing.Point(322, 9);
            this.cboLabelMatch.Name = "cboLabelMatch";
            this.cboLabelMatch.Size = new System.Drawing.Size(100, 26);
            this.cboLabelMatch.TabIndex = 2;
            //
            // btnLabelSearch
            //
            this.btnLabelSearch.Location = new System.Drawing.Point(432, 5);
            this.btnLabelSearch.Name = "btnLabelSearch";
            this.btnLabelSearch.Size = new System.Drawing.Size(100, 36);
            this.btnLabelSearch.TabIndex = 3;
            this.btnLabelSearch.Text = "검색";
            this.btnLabelSearch.UseVisualStyleBackColor = true;
            this.btnLabelSearch.Click += new System.EventHandler(this.btnLabelSearch_Click);
            //
            // btnLabelAll
            //
            this.btnLabelAll.Location = new System.Drawing.Point(540, 5);
            this.btnLabelAll.Name = "btnLabelAll";
            this.btnLabelAll.Size = new System.Drawing.Size(110, 36);
            this.btnLabelAll.TabIndex = 4;
            this.btnLabelAll.Text = "전체 보기";
            this.btnLabelAll.UseVisualStyleBackColor = true;
            this.btnLabelAll.Click += new System.EventHandler(this.btnLabelAll_Click);
            //
            // cboLabelResults (검색 결과 목록: 값 / Layer / 화면 좌표. 선택 = 그 위치로 이동)
            //
            this.cboLabelResults.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLabelResults.DropDownHeight = 400;
            this.cboLabelResults.FormattingEnabled = true;
            this.cboLabelResults.IntegralHeight = false;
            this.cboLabelResults.Location = new System.Drawing.Point(660, 9);
            this.cboLabelResults.Name = "cboLabelResults";
            this.cboLabelResults.Size = new System.Drawing.Size(520, 26);
            this.cboLabelResults.TabIndex = 5;
            this.cboLabelResults.SelectedIndexChanged += new System.EventHandler(this.cboLabelResults_SelectedIndexChanged);
            //
            // lblLabelResult
            //
            this.lblLabelResult.AutoSize = true;
            this.lblLabelResult.Location = new System.Drawing.Point(1192, 14);
            this.lblLabelResult.Name = "lblLabelResult";
            this.lblLabelResult.Size = new System.Drawing.Size(0, 18);
            this.lblLabelResult.TabIndex = 6;
            // 
            // chainCellSizeLabel
            // 
            this.chainCellSizeLabel.AutoSize = true;
            this.chainCellSizeLabel.Location = new System.Drawing.Point(597, 16);
            this.chainCellSizeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.chainCellSizeLabel.Name = "chainCellSizeLabel";
            this.chainCellSizeLabel.Size = new System.Drawing.Size(86, 18);
            this.chainCellSizeLabel.TabIndex = 8;
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
            this._numChainCellSize.Location = new System.Drawing.Point(689, 10);
            this._numChainCellSize.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this._numChainCellSize.TabIndex = 9;
            this._numChainCellSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this._numChainCellSize.Visible = false;
            //
            // lblViewSide (2026-10-07 CLAUDE-030: 보기 기준 Front / Back side)
            //
            this.lblViewSide.AutoSize = true;
            this.lblViewSide.Location = new System.Drawing.Point(820, 16);
            this.lblViewSide.Name = "lblViewSide";
            this.lblViewSide.Size = new System.Drawing.Size(80, 18);
            this.lblViewSide.TabIndex = 10;
            this.lblViewSide.Text = "보기 기준";
            //
            // cboViewSide
            //
            this.cboViewSide.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboViewSide.FormattingEnabled = true;
            this.cboViewSide.Items.AddRange(new object[] {
            "Front side(원본)",
            "Back side(좌우 반전)"});
            this.cboViewSide.Location = new System.Drawing.Point(910, 10);
            this.cboViewSide.Name = "cboViewSide";
            this.cboViewSide.Size = new System.Drawing.Size(230, 26);
            this.cboViewSide.TabIndex = 11;
            this.cboViewSide.SelectedIndexChanged += new System.EventHandler(this.cboViewSide_SelectedIndexChanged);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(449, 8);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(114, 36);
            this.btnStop.TabIndex = 7;
            this.btnStop.Text = "중지";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(326, 8);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(114, 36);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cboDevice
            // 
            this.cboDevice.FormattingEnabled = true;
            this.cboDevice.Location = new System.Drawing.Point(80, 10);
            this.cboDevice.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboDevice.Name = "cboDevice";
            this.cboDevice.Size = new System.Drawing.Size(227, 26);
            this.cboDevice.TabIndex = 3;
            this.cboDevice.SelectedIndexChanged += new System.EventHandler(this.cboDevice_SelectedIndexChanged);
            this.cboDevice.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboDevice_KeyDown);
            // 
            // lblDevice
            // 
            this.lblDevice.AutoSize = true;
            this.lblDevice.Location = new System.Drawing.Point(11, 16);
            this.lblDevice.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(62, 18);
            this.lblDevice.TabIndex = 2;
            this.lblDevice.Text = "Device";
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitMain.Location = new System.Drawing.Point(0, 54);
            this.splitMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.chkLayerItems);
            this.splitMain.Panel1.Controls.Add(this.pnlLayerButtons);
            this.splitMain.Panel1.Controls.Add(this.chainSimilarListPanel);
            this.splitMain.Panel1.Controls.Add(this.chainListPanel);
            this.splitMain.Panel1.Controls.Add(this.grpMapInfo);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.splitChain);
            this.splitMain.Size = new System.Drawing.Size(1714, 1065);
            this.splitMain.SplitterDistance = 260;
            this.splitMain.SplitterWidth = 6;
            this.splitMain.TabIndex = 1;
            // 
            // chkLayerItems
            // 
            this.chkLayerItems.CheckOnClick = true;
            this.chkLayerItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkLayerItems.FormattingEnabled = true;
            this.chkLayerItems.Location = new System.Drawing.Point(0, 720);
            this.chkLayerItems.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkLayerItems.Name = "chkLayerItems";
            this.chkLayerItems.Size = new System.Drawing.Size(260, 345);
            this.chkLayerItems.TabIndex = 2;
            this.chkLayerItems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkLayerItems_ItemCheck);
            this.chkLayerItems.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.chkLayerItems_Format);
            // 
            // pnlLayerButtons
            // 
            this.pnlLayerButtons.Controls.Add(this.btnLayerNone);
            this.pnlLayerButtons.Controls.Add(this.btnLayerAll);
            this.pnlLayerButtons.Controls.Add(this.lblLayerTitle);
            this.pnlLayerButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLayerButtons.Location = new System.Drawing.Point(0, 675);
            this.pnlLayerButtons.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlLayerButtons.Name = "pnlLayerButtons";
            this.pnlLayerButtons.Size = new System.Drawing.Size(260, 45);
            this.pnlLayerButtons.TabIndex = 1;
            // 
            // btnLayerNone
            // 
            this.btnLayerNone.Location = new System.Drawing.Point(286, 6);
            this.btnLayerNone.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLayerNone.Name = "btnLayerNone";
            this.btnLayerNone.Size = new System.Drawing.Size(80, 33);
            this.btnLayerNone.TabIndex = 2;
            this.btnLayerNone.Text = "해제";
            this.btnLayerNone.UseVisualStyleBackColor = true;
            this.btnLayerNone.Click += new System.EventHandler(this.btnLayerNone_Click);
            // 
            // btnLayerAll
            // 
            this.btnLayerAll.Location = new System.Drawing.Point(200, 6);
            this.btnLayerAll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLayerAll.Name = "btnLayerAll";
            this.btnLayerAll.Size = new System.Drawing.Size(80, 33);
            this.btnLayerAll.TabIndex = 1;
            this.btnLayerAll.Text = "전체";
            this.btnLayerAll.UseVisualStyleBackColor = true;
            this.btnLayerAll.Click += new System.EventHandler(this.btnLayerAll_Click);
            // 
            // lblLayerTitle
            // 
            this.lblLayerTitle.AutoSize = true;
            this.lblLayerTitle.Location = new System.Drawing.Point(6, 14);
            this.lblLayerTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLayerTitle.Name = "lblLayerTitle";
            this.lblLayerTitle.Size = new System.Drawing.Size(131, 18);
            this.lblLayerTitle.TabIndex = 0;
            this.lblLayerTitle.Text = "Layer (도형 수)";
            // 
            // chainSimilarListPanel
            // 
            this.chainSimilarListPanel.Controls.Add(this._chainSimilarList);
            this.chainSimilarListPanel.Controls.Add(this.chainSimilarListTitle);
            this.chainSimilarListPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.chainSimilarListPanel.Location = new System.Drawing.Point(0, 495);
            this.chainSimilarListPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chainSimilarListPanel.Name = "chainSimilarListPanel";
            this.chainSimilarListPanel.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chainSimilarListPanel.Size = new System.Drawing.Size(260, 180);
            this.chainSimilarListPanel.TabIndex = 4;
            // 
            // _chainSimilarList
            // 
            this._chainSimilarList.CheckOnClick = true;
            this._chainSimilarList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._chainSimilarList.FormattingEnabled = true;
            this._chainSimilarList.HorizontalScrollbar = true;
            this._chainSimilarList.Location = new System.Drawing.Point(4, 31);
            this._chainSimilarList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._chainSimilarList.Name = "_chainSimilarList";
            this._chainSimilarList.Size = new System.Drawing.Size(252, 145);
            this._chainSimilarList.TabIndex = 1;
            this._chainSimilarList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ChainSimilarList_ItemCheck);
            this._chainSimilarList.SelectedIndexChanged += new System.EventHandler(this.ChainSimilarList_SelectedIndexChanged);
            // 
            // chainSimilarListTitle
            // 
            this.chainSimilarListTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.chainSimilarListTitle.Location = new System.Drawing.Point(4, 4);
            this.chainSimilarListTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.chainSimilarListTitle.Name = "chainSimilarListTitle";
            this.chainSimilarListTitle.Size = new System.Drawing.Size(252, 27);
            this.chainSimilarListTitle.TabIndex = 0;
            this.chainSimilarListTitle.Text = "유사 묶음 (체크: 제외 / 행: 위치)";
            this.chainSimilarListTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chainListPanel
            // 
            this.chainListPanel.Controls.Add(this._chainList);
            this.chainListPanel.Controls.Add(this.chainListButtons);
            this.chainListPanel.Controls.Add(this.chainListTitle);
            this.chainListPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.chainListPanel.Location = new System.Drawing.Point(0, 195);
            this.chainListPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chainListPanel.Name = "chainListPanel";
            this.chainListPanel.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chainListPanel.Size = new System.Drawing.Size(260, 300);
            this.chainListPanel.TabIndex = 3;
            // 
            // _chainList
            // 
            this._chainList.CheckOnClick = true;
            this._chainList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._chainList.HorizontalScrollbar = true;
            this._chainList.Location = new System.Drawing.Point(4, 118);
            this._chainList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._chainList.Name = "_chainList";
            this._chainList.Size = new System.Drawing.Size(252, 178);
            this._chainList.TabIndex = 2;
            this._chainList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ChainList_ItemCheck);
            this._chainList.SelectedIndexChanged += new System.EventHandler(this.ChainList_SelectedIndexChanged);
            this._chainList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ChainList_MouseDown);
            this._chainList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ChainList_MouseUp);
            // 
            // chainListButtons
            // 
            this.chainListButtons.Controls.Add(this._btnChainNew);
            this.chainListButtons.Controls.Add(this._btnChainRename);
            this.chainListButtons.Controls.Add(this._btnChainDelete);
            this.chainListButtons.Controls.Add(this._btnChainDbSave);
            this.chainListButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.chainListButtons.Location = new System.Drawing.Point(4, 31);
            this.chainListButtons.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chainListButtons.Name = "chainListButtons";
            this.chainListButtons.Size = new System.Drawing.Size(252, 87);
            this.chainListButtons.TabIndex = 1;
            // 
            // _btnChainNew
            // 
            this._btnChainNew.Location = new System.Drawing.Point(3, 3);
            this._btnChainNew.Name = "_btnChainNew";
            this._btnChainNew.Size = new System.Drawing.Size(174, 36);
            this._btnChainNew.TabIndex = 0;
            this._btnChainNew.Text = "새 Chain";
            this._btnChainNew.UseVisualStyleBackColor = true;
            this._btnChainNew.Click += new System.EventHandler(this.BtnChainNew_Click);
            // 
            // _btnChainRename
            // 
            this._btnChainRename.Location = new System.Drawing.Point(3, 45);
            this._btnChainRename.Name = "_btnChainRename";
            this._btnChainRename.Size = new System.Drawing.Size(174, 36);
            this._btnChainRename.TabIndex = 1;
            this._btnChainRename.Text = "이름 수정";
            this._btnChainRename.UseVisualStyleBackColor = true;
            this._btnChainRename.Click += new System.EventHandler(this.BtnChainRename_Click);
            // 
            // _btnChainDelete
            // 
            this._btnChainDelete.Location = new System.Drawing.Point(3, 87);
            this._btnChainDelete.Name = "_btnChainDelete";
            this._btnChainDelete.Size = new System.Drawing.Size(174, 36);
            this._btnChainDelete.TabIndex = 2;
            this._btnChainDelete.Text = "삭제";
            this._btnChainDelete.UseVisualStyleBackColor = true;
            this._btnChainDelete.Click += new System.EventHandler(this.BtnChainDelete_Click);
            // 
            // _btnChainDbSave
            // 
            this._btnChainDbSave.Location = new System.Drawing.Point(3, 129);
            this._btnChainDbSave.Name = "_btnChainDbSave";
            this._btnChainDbSave.Size = new System.Drawing.Size(174, 36);
            this._btnChainDbSave.TabIndex = 3;
            this._btnChainDbSave.Text = "Chain 저장(DB)";
            this._btnChainDbSave.UseVisualStyleBackColor = true;
            this._btnChainDbSave.Click += new System.EventHandler(this.BtnChainDbSave_Click);
            // 
            // chainListTitle
            // 
            this.chainListTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.chainListTitle.Location = new System.Drawing.Point(4, 4);
            this.chainListTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.chainListTitle.Name = "chainListTitle";
            this.chainListTitle.Size = new System.Drawing.Size(252, 27);
            this.chainListTitle.TabIndex = 0;
            this.chainListTitle.Text = "Chain 목록 (체크: 표시 / 행: 편집)";
            this.chainListTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpMapInfo
            // 
            this.grpMapInfo.Controls.Add(this.lblMapInfo);
            this.grpMapInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpMapInfo.Location = new System.Drawing.Point(0, 0);
            this.grpMapInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpMapInfo.Name = "grpMapInfo";
            this.grpMapInfo.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpMapInfo.Size = new System.Drawing.Size(260, 195);
            this.grpMapInfo.TabIndex = 0;
            this.grpMapInfo.TabStop = false;
            this.grpMapInfo.Text = "Map 정보";
            // 
            // lblMapInfo
            // 
            this.lblMapInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMapInfo.Location = new System.Drawing.Point(4, 25);
            this.lblMapInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMapInfo.Name = "lblMapInfo";
            this.lblMapInfo.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.lblMapInfo.Size = new System.Drawing.Size(252, 166);
            this.lblMapInfo.TabIndex = 0;
            this.lblMapInfo.Text = "Device를 선택하세요.";
            // 
            // splitChain
            // 
            this.splitChain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitChain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitChain.Location = new System.Drawing.Point(0, 0);
            this.splitChain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitChain.Name = "splitChain";
            this.splitChain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitChain.Panel1
            // 
            this.splitChain.Panel1.Controls.Add(this._lblChainSetupStatus);
            this.splitChain.Panel1.Controls.Add(this.chainSimilarRow);
            this.splitChain.Panel1.Controls.Add(this.chainFirstRow);
            // 
            // splitChain.Panel2
            // 
            this.splitChain.Panel2.Controls.Add(this.map);
            this.splitChain.Size = new System.Drawing.Size(1448, 1065);
            this.splitChain.SplitterDistance = 166;
            this.splitChain.SplitterWidth = 6;
            this.splitChain.TabIndex = 0;
            // 
            // _lblChainSetupStatus
            // 
            this._lblChainSetupStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lblChainSetupStatus.Location = new System.Drawing.Point(0, 90);
            this._lblChainSetupStatus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._lblChainSetupStatus.Name = "_lblChainSetupStatus";
            this._lblChainSetupStatus.Size = new System.Drawing.Size(1448, 76);
            this._lblChainSetupStatus.TabIndex = 2;
            this._lblChainSetupStatus.Text = "Map을 조회한 뒤 Chain을 만드세요.";
            // 
            // chainSimilarRow
            // 
            this.chainSimilarRow.Controls.Add(this._btnChainExample);
            this.chainSimilarRow.Controls.Add(this._btnChainFindSimilar);
            this.chainSimilarRow.Controls.Add(this._btnChainClearSimilar);
            this.chainSimilarRow.Controls.Add(this._btnChainApplySimilar);
            this.chainSimilarRow.Controls.Add(this._btnChainUndoSimilar);
            this.chainSimilarRow.Dock = System.Windows.Forms.DockStyle.Top;
            this.chainSimilarRow.Location = new System.Drawing.Point(0, 45);
            this.chainSimilarRow.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chainSimilarRow.Name = "chainSimilarRow";
            this.chainSimilarRow.Size = new System.Drawing.Size(1448, 45);
            this.chainSimilarRow.TabIndex = 1;
            this.chainSimilarRow.WrapContents = false;
            // 
            // _btnChainExample
            // 
            this._btnChainExample.Location = new System.Drawing.Point(3, 3);
            this._btnChainExample.Name = "_btnChainExample";
            this._btnChainExample.Size = new System.Drawing.Size(150, 36);
            this._btnChainExample.TabIndex = 0;
            this._btnChainExample.Text = "예시 묶음 지정";
            this._btnChainExample.UseVisualStyleBackColor = true;
            this._btnChainExample.Click += new System.EventHandler(this.BtnChainExample_Click);
            // 
            // _btnChainFindSimilar
            // 
            this._btnChainFindSimilar.Location = new System.Drawing.Point(159, 3);
            this._btnChainFindSimilar.Name = "_btnChainFindSimilar";
            this._btnChainFindSimilar.Size = new System.Drawing.Size(150, 36);
            this._btnChainFindSimilar.TabIndex = 1;
            this._btnChainFindSimilar.Text = "유사 묶음 찾기";
            this._btnChainFindSimilar.UseVisualStyleBackColor = true;
            this._btnChainFindSimilar.Click += new System.EventHandler(this.BtnChainFindSimilar_Click);
            // 
            // _btnChainClearSimilar
            // 
            this._btnChainClearSimilar.Location = new System.Drawing.Point(315, 3);
            this._btnChainClearSimilar.Name = "_btnChainClearSimilar";
            this._btnChainClearSimilar.Size = new System.Drawing.Size(150, 36);
            this._btnChainClearSimilar.TabIndex = 2;
            this._btnChainClearSimilar.Text = "미리보기 취소";
            this._btnChainClearSimilar.UseVisualStyleBackColor = true;
            this._btnChainClearSimilar.Click += new System.EventHandler(this.BtnChainClearSimilar_Click);
            // 
            // _btnChainApplySimilar
            // 
            this._btnChainApplySimilar.Location = new System.Drawing.Point(471, 3);
            this._btnChainApplySimilar.Name = "_btnChainApplySimilar";
            this._btnChainApplySimilar.Size = new System.Drawing.Size(143, 36);
            this._btnChainApplySimilar.TabIndex = 3;
            this._btnChainApplySimilar.Text = "체크 묶음 제외";
            this._btnChainApplySimilar.UseVisualStyleBackColor = true;
            this._btnChainApplySimilar.Click += new System.EventHandler(this.BtnChainApplySimilar_Click);
            // 
            // _btnChainUndoSimilar
            // 
            this._btnChainUndoSimilar.Location = new System.Drawing.Point(620, 3);
            this._btnChainUndoSimilar.Name = "_btnChainUndoSimilar";
            this._btnChainUndoSimilar.Size = new System.Drawing.Size(179, 36);
            this._btnChainUndoSimilar.TabIndex = 4;
            this._btnChainUndoSimilar.Text = "직전 제외 되돌리기";
            this._btnChainUndoSimilar.UseVisualStyleBackColor = true;
            this._btnChainUndoSimilar.Click += new System.EventHandler(this.BtnChainUndoSimilar_Click);
            // 
            // chainFirstRow
            // 
            this.chainFirstRow.Controls.Add(this._btnChainInput);
            this.chainFirstRow.Controls.Add(this._btnChainOutput);
            this.chainFirstRow.Controls.Add(this._btnChainApply);
            this.chainFirstRow.Controls.Add(this._btnChainCancel);
            this.chainFirstRow.Controls.Add(this._btnChainTrace);
            this.chainFirstRow.Controls.Add(this._btnChainAdd);
            this.chainFirstRow.Controls.Add(this._btnChainRemove);
            this.chainFirstRow.Dock = System.Windows.Forms.DockStyle.Top;
            this.chainFirstRow.Location = new System.Drawing.Point(0, 0);
            this.chainFirstRow.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chainFirstRow.Name = "chainFirstRow";
            this.chainFirstRow.Size = new System.Drawing.Size(1448, 45);
            this.chainFirstRow.TabIndex = 0;
            this.chainFirstRow.WrapContents = false;
            // 
            // _btnChainInput
            // 
            this._btnChainInput.Location = new System.Drawing.Point(3, 3);
            this._btnChainInput.Name = "_btnChainInput";
            this._btnChainInput.Size = new System.Drawing.Size(157, 36);
            this._btnChainInput.TabIndex = 0;
            this._btnChainInput.Text = "Input 지정";
            this._btnChainInput.UseVisualStyleBackColor = true;
            this._btnChainInput.Click += new System.EventHandler(this.BtnChainInput_Click);
            // 
            // _btnChainOutput
            // 
            this._btnChainOutput.Location = new System.Drawing.Point(166, 3);
            this._btnChainOutput.Name = "_btnChainOutput";
            this._btnChainOutput.Size = new System.Drawing.Size(157, 36);
            this._btnChainOutput.TabIndex = 1;
            this._btnChainOutput.Text = "Output 지정";
            this._btnChainOutput.UseVisualStyleBackColor = true;
            this._btnChainOutput.Click += new System.EventHandler(this.BtnChainOutput_Click);
            // 
            // _btnChainApply
            // 
            this._btnChainApply.Location = new System.Drawing.Point(329, 3);
            this._btnChainApply.Name = "_btnChainApply";
            this._btnChainApply.Size = new System.Drawing.Size(114, 36);
            this._btnChainApply.TabIndex = 2;
            this._btnChainApply.Text = "선택 적용";
            this._btnChainApply.UseVisualStyleBackColor = true;
            this._btnChainApply.Click += new System.EventHandler(this.BtnChainApply_Click);
            // 
            // _btnChainCancel
            // 
            this._btnChainCancel.Location = new System.Drawing.Point(449, 3);
            this._btnChainCancel.Name = "_btnChainCancel";
            this._btnChainCancel.Size = new System.Drawing.Size(114, 36);
            this._btnChainCancel.TabIndex = 3;
            this._btnChainCancel.Text = "선택 취소";
            this._btnChainCancel.UseVisualStyleBackColor = true;
            this._btnChainCancel.Click += new System.EventHandler(this.BtnChainCancel_Click);
            // 
            // _btnChainTrace
            // 
            this._btnChainTrace.Location = new System.Drawing.Point(569, 3);
            this._btnChainTrace.Name = "_btnChainTrace";
            this._btnChainTrace.Size = new System.Drawing.Size(157, 36);
            this._btnChainTrace.TabIndex = 4;
            this._btnChainTrace.Text = "후보 경로 찾기";
            this._btnChainTrace.UseVisualStyleBackColor = true;
            this._btnChainTrace.Click += new System.EventHandler(this.BtnChainTrace_Click);
            // 
            // _btnChainAdd
            // 
            this._btnChainAdd.Location = new System.Drawing.Point(732, 3);
            this._btnChainAdd.Name = "_btnChainAdd";
            this._btnChainAdd.Size = new System.Drawing.Size(129, 36);
            this._btnChainAdd.TabIndex = 5;
            this._btnChainAdd.Text = "경로 추가";
            this._btnChainAdd.UseVisualStyleBackColor = true;
            this._btnChainAdd.Click += new System.EventHandler(this.BtnChainAdd_Click);
            // 
            // _btnChainRemove
            // 
            this._btnChainRemove.Location = new System.Drawing.Point(867, 3);
            this._btnChainRemove.Name = "_btnChainRemove";
            this._btnChainRemove.Size = new System.Drawing.Size(129, 36);
            this._btnChainRemove.TabIndex = 6;
            this._btnChainRemove.Text = "경로 제외";
            this._btnChainRemove.UseVisualStyleBackColor = true;
            this._btnChainRemove.Click += new System.EventHandler(this.BtnChainRemove_Click);
            // 
            // map
            // 
            this.map.BackColor = System.Drawing.Color.Black;
            this.map.ColorAlpha = 50;
            this.map.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.Location = new System.Drawing.Point(0, 0);
            this.map.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.map.Mode = NexplantQMS.GdsMap.GdsMapControl.ViewMode.View;
            this.map.Name = "map";
            this.map.Size = new System.Drawing.Size(1448, 893);
            this.map.TabIndex = 0;
            this.map.VSync = false;
            this.map.MouseWorldPositionChanged += new System.EventHandler<System.Drawing.PointF>(this.map_MouseWorldPositionChanged);
            this.map.RenderProgressChanged += new System.EventHandler<NexplantQMS.GdsMap.GdsMapRenderProgressChangedEventArgs>(this.map_RenderProgressChanged);
            this.map.FirstFrameMeasured += new System.EventHandler<NexplantQMS.GdsMap.GdsMapLoadMetrics>(this.map_FirstFrameMeasured);
            this.map.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Map_ChainMouseClick);
            this.map.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Map_ChainMouseDown);
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
            // GdsMapViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1714, 1155);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.pnlLabelSearch);
            this.Controls.Add(this.pnlCondition);
            this.Controls.Add(this.statusStripView);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "GdsMapViewForm";
            this.Text = "GDS Map 조회";
            this.Shown += new System.EventHandler(this.GdsMapViewForm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GdsMapViewForm_ChainKeyDown);
            this.pnlCondition.ResumeLayout(false);
            this.pnlCondition.PerformLayout();
            this.pnlLabelSearch.ResumeLayout(false);
            this.pnlLabelSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numChainCellSize)).EndInit();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.pnlLayerButtons.ResumeLayout(false);
            this.pnlLayerButtons.PerformLayout();
            this.chainSimilarListPanel.ResumeLayout(false);
            this.chainListPanel.ResumeLayout(false);
            this.chainListButtons.ResumeLayout(false);
            this.grpMapInfo.ResumeLayout(false);
            this.splitChain.Panel1.ResumeLayout(false);
            this.splitChain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitChain)).EndInit();
            this.splitChain.ResumeLayout(false);
            this.chainSimilarRow.ResumeLayout(false);
            this.chainFirstRow.ResumeLayout(false);
            this.statusStripView.ResumeLayout(false);
            this.statusStripView.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlCondition;
        private System.Windows.Forms.Panel pnlLabelSearch;
        private System.Windows.Forms.Label lblLabel;
        private System.Windows.Forms.TextBox txtLabel;
        private System.Windows.Forms.ComboBox cboLabelMatch;
        private System.Windows.Forms.Button btnLabelSearch;
        private System.Windows.Forms.Button btnLabelAll;
        private System.Windows.Forms.ComboBox cboLabelResults;
        private System.Windows.Forms.Label lblLabelResult;
        private System.Windows.Forms.Label lblViewSide;
        private System.Windows.Forms.ComboBox cboViewSide;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.ComboBox cboDevice;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.GroupBox grpMapInfo;
        private System.Windows.Forms.Label lblMapInfo;
        private System.Windows.Forms.Panel pnlLayerButtons;
        private System.Windows.Forms.Label lblLayerTitle;
        private System.Windows.Forms.Button btnLayerAll;
        private System.Windows.Forms.Button btnLayerNone;
        private NexplantQMS.GdsMap.LayerColorCheckedListBox chkLayerItems;
        private NexplantQMS.GdsMap.GdsMapControl map;
        private System.Windows.Forms.StatusStrip statusStripView;
        private System.Windows.Forms.ToolStripProgressBar progressLoad;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblCoordinate;
        // Chain 설정 컨트롤(2026-10-06 Setup 화면에서 이동). 동작은 GdsMapViewForm_Chain*.cs partial에서 연결한다.
        private System.Windows.Forms.Panel chainListPanel;
        private System.Windows.Forms.Label chainListTitle;
        private System.Windows.Forms.FlowLayoutPanel chainListButtons;
        private System.Windows.Forms.CheckedListBox _chainList;
        private System.Windows.Forms.Panel chainSimilarListPanel;
        private System.Windows.Forms.Label chainSimilarListTitle;
        private System.Windows.Forms.CheckedListBox _chainSimilarList;
        private System.Windows.Forms.SplitContainer splitChain;
        private System.Windows.Forms.RichTextBox _lblChainSetupStatus;
        private System.Windows.Forms.FlowLayoutPanel chainFirstRow;
        private System.Windows.Forms.FlowLayoutPanel chainSimilarRow;
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
