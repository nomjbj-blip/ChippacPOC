namespace NexplantQMS.GdsMap
{
    partial class GdsMapViewU8Form
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

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// GDS Map View U8 배치 (2026-10-20, 설계: 문서/설계/02_GDS/2026-10-20_GDS_Map_View_U8_화면_설계.md 4절).
        ///  위: 도구 막대(Device / 조회 / 중지 | 보기 기준 | Label 검색).
        ///  가운데: U8 작업 줄(도면 조회 후: 예시 / 예시 지정 / 같은 형태 찾기 / 중지) + 도면.
        ///  오른쪽: 탭 [찾기 결과](요약 / 찾기 / 분류 / 목록에서 빼기 / Chain 생성(DB) / 가상 목록) / [저장된 Chain] / [Layer].
        ///  아래: 진행률 / 상태 / 마우스 좌표.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GdsMapViewU8Form));
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
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.map = new NexplantQMS.GdsMap.GdsMapControl();
            this.pnlU8Bar = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTemplate = new System.Windows.Forms.Label();
            this.btnTemplate = new System.Windows.Forms.Button();
            this.btnFind = new System.Windows.Forms.Button();
            this.btnFindStop = new System.Windows.Forms.Button();
            this.tabRight = new System.Windows.Forms.TabControl();
            this.tabResult = new System.Windows.Forms.TabPage();
            this.dgvResult = new System.Windows.Forms.DataGridView();
            this.colResultName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResultClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResultDb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResultPos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlResultTop = new System.Windows.Forms.Panel();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnResultRemove = new System.Windows.Forms.Button();
            this.cboResultClass = new System.Windows.Forms.ComboBox();
            this.txtResultFind = new System.Windows.Forms.TextBox();
            this.lblResultFind = new System.Windows.Forms.Label();
            this.lblResultSummary = new System.Windows.Forms.Label();
            this.tabSaved = new System.Windows.Forms.TabPage();
            this.dgvSaved = new System.Windows.Forms.DataGridView();
            this.colSavedShow = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colSavedColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSavedName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSavedCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSavedStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlSavedTop = new System.Windows.Forms.Panel();
            this.lblSavedSummary = new System.Windows.Forms.Label();
            this.txtSavedFind = new System.Windows.Forms.TextBox();
            this.lblSavedFind = new System.Windows.Forms.Label();
            this.btnSavedDeleteShown = new System.Windows.Forms.Button();
            this.btnSavedDelete = new System.Windows.Forms.Button();
            this.btnSavedRefresh = new System.Windows.Forms.Button();
            this.btnSavedCheckAll = new System.Windows.Forms.Button();
            this.btnSavedCheckNone = new System.Windows.Forms.Button();
            this.btnSavedColor = new System.Windows.Forms.Button();
            this.tabLayer = new System.Windows.Forms.TabPage();
            this.chkLayerItems = new NexplantQMS.GdsMap.LayerColorCheckedListBox();
            this.pnlLayerButtons = new System.Windows.Forms.Panel();
            this.btnLayerNone = new System.Windows.Forms.Button();
            this.btnLayerAll = new System.Windows.Forms.Button();
            this.lblLayerTitle = new System.Windows.Forms.Label();
            this.statusStripView = new System.Windows.Forms.StatusStrip();
            this.progressLoad = new System.Windows.Forms.ToolStripProgressBar();
            this.lblElapsed = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCoordinate = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTipView = new System.Windows.Forms.ToolTip(this.components);
            this.pnlCondition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.pnlU8Bar.SuspendLayout();
            this.tabRight.SuspendLayout();
            this.tabResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).BeginInit();
            this.pnlResultTop.SuspendLayout();
            this.tabSaved.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSaved)).BeginInit();
            this.pnlSavedTop.SuspendLayout();
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
            this.pnlCondition.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCondition.Location = new System.Drawing.Point(0, 0);
            this.pnlCondition.Name = "pnlCondition";
            this.pnlCondition.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pnlCondition.Size = new System.Drawing.Size(1714, 54);
            this.pnlCondition.TabIndex = 0;
            // 
            // lblDevice
            // 
            this.lblDevice.AutoSize = true;
            this.lblDevice.Location = new System.Drawing.Point(10, 18);
            this.lblDevice.Margin = new System.Windows.Forms.Padding(6, 18, 3, 3);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(62, 18);
            this.lblDevice.TabIndex = 0;
            this.lblDevice.Text = "Device";
            // 
            // cboDevice
            // 
            this.cboDevice.FormattingEnabled = true;
            this.cboDevice.Location = new System.Drawing.Point(78, 14);
            this.cboDevice.Margin = new System.Windows.Forms.Padding(3, 14, 3, 3);
            this.cboDevice.Name = "cboDevice";
            this.cboDevice.Size = new System.Drawing.Size(190, 26);
            this.cboDevice.TabIndex = 1;
            this.cboDevice.SelectedIndexChanged += new System.EventHandler(this.cboDevice_SelectedIndexChanged);
            this.cboDevice.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboDevice_KeyDown);
            // 
            // btnSearch
            // 
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.Location = new System.Drawing.Point(274, 6);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(126, 42);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "조회";
            this.btnSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.Location = new System.Drawing.Point(406, 6);
            this.btnStop.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(126, 42);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "중지";
            this.btnStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // sepTool1
            // 
            this.sepTool1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.sepTool1.Location = new System.Drawing.Point(541, 13);
            this.sepTool1.Margin = new System.Windows.Forms.Padding(6, 13, 6, 13);
            this.sepTool1.Name = "sepTool1";
            this.sepTool1.Size = new System.Drawing.Size(1, 28);
            this.sepTool1.TabIndex = 4;
            this.sepTool1.Visible = false;
            // 
            // lblViewSide
            // 
            this.lblViewSide.AutoSize = true;
            this.lblViewSide.Location = new System.Drawing.Point(551, 18);
            this.lblViewSide.Margin = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.lblViewSide.Name = "lblViewSide";
            this.lblViewSide.Size = new System.Drawing.Size(44, 18);
            this.lblViewSide.TabIndex = 5;
            this.lblViewSide.Text = "보기";
            this.lblViewSide.Visible = false;
            // 
            // cboViewSide
            // 
            this.cboViewSide.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboViewSide.FormattingEnabled = true;
            this.cboViewSide.Items.AddRange(new object[] {
            "Front side(원본)",
            "Back side(반전)"});
            this.cboViewSide.Location = new System.Drawing.Point(601, 14);
            this.cboViewSide.Margin = new System.Windows.Forms.Padding(3, 14, 3, 3);
            this.cboViewSide.Name = "cboViewSide";
            this.cboViewSide.Size = new System.Drawing.Size(190, 26);
            this.cboViewSide.TabIndex = 6;
            this.cboViewSide.Visible = false;
            this.cboViewSide.SelectedIndexChanged += new System.EventHandler(this.cboViewSide_SelectedIndexChanged);
            // 
            // sepTool2
            // 
            this.sepTool2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.sepTool2.Location = new System.Drawing.Point(800, 13);
            this.sepTool2.Margin = new System.Windows.Forms.Padding(6, 13, 6, 13);
            this.sepTool2.Name = "sepTool2";
            this.sepTool2.Size = new System.Drawing.Size(1, 28);
            this.sepTool2.TabIndex = 7;
            this.sepTool2.Visible = false;
            // 
            // lblLabel
            // 
            this.lblLabel.AutoSize = true;
            this.lblLabel.Location = new System.Drawing.Point(810, 18);
            this.lblLabel.Margin = new System.Windows.Forms.Padding(3, 18, 3, 3);
            this.lblLabel.Name = "lblLabel";
            this.lblLabel.Size = new System.Drawing.Size(50, 18);
            this.lblLabel.TabIndex = 8;
            this.lblLabel.Text = "Label";
            this.lblLabel.Visible = false;
            // 
            // txtLabel
            // 
            this.txtLabel.Location = new System.Drawing.Point(866, 13);
            this.txtLabel.Margin = new System.Windows.Forms.Padding(3, 13, 3, 3);
            this.txtLabel.Name = "txtLabel";
            this.txtLabel.Size = new System.Drawing.Size(180, 28);
            this.txtLabel.TabIndex = 9;
            this.txtLabel.Visible = false;
            this.txtLabel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLabel_KeyDown);
            // 
            // btnLabelSearch
            // 
            this.btnLabelSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnLabelSearch.Image")));
            this.btnLabelSearch.Location = new System.Drawing.Point(1052, 6);
            this.btnLabelSearch.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnLabelSearch.Name = "btnLabelSearch";
            this.btnLabelSearch.Size = new System.Drawing.Size(108, 42);
            this.btnLabelSearch.TabIndex = 10;
            this.btnLabelSearch.Text = "검색";
            this.btnLabelSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLabelSearch.UseVisualStyleBackColor = true;
            this.btnLabelSearch.Visible = false;
            this.btnLabelSearch.Click += new System.EventHandler(this.btnLabelSearch_Click);
            // 
            // btnLabelAll
            // 
            this.btnLabelAll.Image = ((System.Drawing.Image)(resources.GetObject("btnLabelAll.Image")));
            this.btnLabelAll.Location = new System.Drawing.Point(1166, 6);
            this.btnLabelAll.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnLabelAll.Name = "btnLabelAll";
            this.btnLabelAll.Size = new System.Drawing.Size(138, 42);
            this.btnLabelAll.TabIndex = 11;
            this.btnLabelAll.Text = "전체 보기";
            this.btnLabelAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLabelAll.UseVisualStyleBackColor = true;
            this.btnLabelAll.Visible = false;
            this.btnLabelAll.Click += new System.EventHandler(this.btnLabelAll_Click);
            // 
            // cboLabelResults
            // 
            this.cboLabelResults.DropDownHeight = 400;
            this.cboLabelResults.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLabelResults.DropDownWidth = 520;
            this.cboLabelResults.FormattingEnabled = true;
            this.cboLabelResults.IntegralHeight = false;
            this.cboLabelResults.Location = new System.Drawing.Point(1310, 14);
            this.cboLabelResults.Margin = new System.Windows.Forms.Padding(3, 14, 3, 3);
            this.cboLabelResults.Name = "cboLabelResults";
            this.cboLabelResults.Size = new System.Drawing.Size(250, 26);
            this.cboLabelResults.TabIndex = 12;
            this.cboLabelResults.Visible = false;
            this.cboLabelResults.SelectedIndexChanged += new System.EventHandler(this.cboLabelResults_SelectedIndexChanged);
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitMain.Location = new System.Drawing.Point(0, 54);
            this.splitMain.Margin = new System.Windows.Forms.Padding(4);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.map);
            this.splitMain.Panel1.Controls.Add(this.pnlU8Bar);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.tabRight);
            this.splitMain.Size = new System.Drawing.Size(1714, 1065);
            this.splitMain.SplitterDistance = 1186;
            this.splitMain.SplitterWidth = 6;
            this.splitMain.TabIndex = 1;
            // 
            // map
            // 
            this.map.BackColor = System.Drawing.Color.Black;
            this.map.ColorAlpha = 50;
            this.map.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.Location = new System.Drawing.Point(0, 54);
            this.map.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.map.Mode = NexplantQMS.GdsMap.GdsMapControl.ViewMode.View;
            this.map.Name = "map";
            this.map.Size = new System.Drawing.Size(1186, 1011);
            this.map.TabIndex = 1;
            this.map.VSync = false;
            this.map.MouseWorldPositionChanged += new System.EventHandler<System.Drawing.PointF>(this.map_MouseWorldPositionChanged);
            this.map.RenderProgressChanged += new System.EventHandler<NexplantQMS.GdsMap.GdsMapRenderProgressChangedEventArgs>(this.map_RenderProgressChanged);
            this.map.FirstFrameMeasured += new System.EventHandler<NexplantQMS.GdsMap.GdsMapLoadMetrics>(this.map_FirstFrameMeasured);
            this.map.MouseClick += new System.Windows.Forms.MouseEventHandler(this.map_MouseClick);
            this.map.MouseDown += new System.Windows.Forms.MouseEventHandler(this.map_MouseDown);
            // 
            // pnlU8Bar
            // 
            this.pnlU8Bar.AutoSize = true;
            this.pnlU8Bar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlU8Bar.BackColor = System.Drawing.Color.AliceBlue;
            this.pnlU8Bar.Controls.Add(this.lblTemplate);
            this.pnlU8Bar.Controls.Add(this.btnTemplate);
            this.pnlU8Bar.Controls.Add(this.btnFind);
            this.pnlU8Bar.Controls.Add(this.btnFindStop);
            this.pnlU8Bar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlU8Bar.Location = new System.Drawing.Point(0, 0);
            this.pnlU8Bar.Name = "pnlU8Bar";
            this.pnlU8Bar.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pnlU8Bar.Size = new System.Drawing.Size(1186, 54);
            this.pnlU8Bar.TabIndex = 0;
            this.pnlU8Bar.Visible = false;
            // 
            // lblTemplate
            // 
            this.lblTemplate.AutoSize = true;
            this.lblTemplate.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblTemplate.Location = new System.Drawing.Point(10, 17);
            this.lblTemplate.Margin = new System.Windows.Forms.Padding(6, 17, 6, 3);
            this.lblTemplate.Name = "lblTemplate";
            this.lblTemplate.Size = new System.Drawing.Size(95, 25);
            this.lblTemplate.TabIndex = 0;
            this.lblTemplate.Text = "예시: 없음";
            // 
            // btnTemplate
            // 
            this.btnTemplate.Image = ((System.Drawing.Image)(resources.GetObject("btnTemplate.Image")));
            this.btnTemplate.Location = new System.Drawing.Point(114, 6);
            this.btnTemplate.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnTemplate.Name = "btnTemplate";
            this.btnTemplate.Size = new System.Drawing.Size(150, 42);
            this.btnTemplate.TabIndex = 1;
            this.btnTemplate.Text = "예시 지정";
            this.btnTemplate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTemplate.UseVisualStyleBackColor = true;
            this.btnTemplate.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // btnFind
            // 
            this.btnFind.Enabled = false;
            this.btnFind.Image = ((System.Drawing.Image)(resources.GetObject("btnFind.Image")));
            this.btnFind.Location = new System.Drawing.Point(270, 6);
            this.btnFind.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(196, 42);
            this.btnFind.TabIndex = 2;
            this.btnFind.Text = "같은 형태 찾기";
            this.btnFind.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // btnFindStop
            // 
            this.btnFindStop.Enabled = false;
            this.btnFindStop.Image = ((System.Drawing.Image)(resources.GetObject("btnFindStop.Image")));
            this.btnFindStop.Location = new System.Drawing.Point(472, 6);
            this.btnFindStop.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnFindStop.Name = "btnFindStop";
            this.btnFindStop.Size = new System.Drawing.Size(108, 42);
            this.btnFindStop.TabIndex = 3;
            this.btnFindStop.Text = "중지";
            this.btnFindStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFindStop.UseVisualStyleBackColor = true;
            this.btnFindStop.Click += new System.EventHandler(this.btnFindStop_Click);
            // 
            // tabRight
            // 
            this.tabRight.Controls.Add(this.tabResult);
            this.tabRight.Controls.Add(this.tabSaved);
            this.tabRight.Controls.Add(this.tabLayer);
            this.tabRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabRight.Location = new System.Drawing.Point(0, 0);
            this.tabRight.Name = "tabRight";
            this.tabRight.SelectedIndex = 0;
            this.tabRight.Size = new System.Drawing.Size(522, 1065);
            this.tabRight.TabIndex = 0;
            // 
            // tabResult
            // 
            this.tabResult.Controls.Add(this.dgvResult);
            this.tabResult.Controls.Add(this.pnlResultTop);
            this.tabResult.Location = new System.Drawing.Point(4, 28);
            this.tabResult.Name = "tabResult";
            this.tabResult.Padding = new System.Windows.Forms.Padding(3);
            this.tabResult.Size = new System.Drawing.Size(514, 1033);
            this.tabResult.TabIndex = 0;
            this.tabResult.Text = "찾기 결과";
            this.tabResult.UseVisualStyleBackColor = true;
            // 
            // dgvResult
            // 
            this.dgvResult.AllowUserToAddRows = false;
            this.dgvResult.AllowUserToDeleteRows = false;
            this.dgvResult.AllowUserToResizeRows = false;
            this.dgvResult.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colResultName,
            this.colResultClass,
            this.colResultDb,
            this.colResultPos});
            this.dgvResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResult.Location = new System.Drawing.Point(3, 147);
            this.dgvResult.Name = "dgvResult";
            this.dgvResult.ReadOnly = true;
            this.dgvResult.RowHeadersVisible = false;
            this.dgvResult.RowHeadersWidth = 62;
            this.dgvResult.RowTemplate.Height = 27;
            this.dgvResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResult.Size = new System.Drawing.Size(508, 883);
            this.dgvResult.TabIndex = 1;
            this.dgvResult.VirtualMode = true;
            this.dgvResult.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvResult_CellValueNeeded);
            this.dgvResult.SelectionChanged += new System.EventHandler(this.dgvResult_SelectionChanged);
            this.dgvResult.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvResult_KeyDown);
            // 
            // colResultName
            // 
            this.colResultName.HeaderText = "이름";
            this.colResultName.MinimumWidth = 8;
            this.colResultName.Name = "colResultName";
            this.colResultName.ReadOnly = true;
            this.colResultName.Width = 170;
            // 
            // colResultClass
            // 
            this.colResultClass.HeaderText = "분류";
            this.colResultClass.MinimumWidth = 8;
            this.colResultClass.Name = "colResultClass";
            this.colResultClass.ReadOnly = true;
            this.colResultClass.Width = 120;
            // 
            // colResultDb
            // 
            this.colResultDb.HeaderText = "DB";
            this.colResultDb.MinimumWidth = 8;
            this.colResultDb.Name = "colResultDb";
            this.colResultDb.ReadOnly = true;
            this.colResultDb.Width = 70;
            // 
            // colResultPos
            // 
            this.colResultPos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colResultPos.HeaderText = "위치";
            this.colResultPos.MinimumWidth = 8;
            this.colResultPos.Name = "colResultPos";
            this.colResultPos.ReadOnly = true;
            // 
            // pnlResultTop
            // 
            this.pnlResultTop.Controls.Add(this.btnCreate);
            this.pnlResultTop.Controls.Add(this.btnResultRemove);
            this.pnlResultTop.Controls.Add(this.cboResultClass);
            this.pnlResultTop.Controls.Add(this.txtResultFind);
            this.pnlResultTop.Controls.Add(this.lblResultFind);
            this.pnlResultTop.Controls.Add(this.lblResultSummary);
            this.pnlResultTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlResultTop.Location = new System.Drawing.Point(3, 3);
            this.pnlResultTop.Name = "pnlResultTop";
            this.pnlResultTop.Size = new System.Drawing.Size(508, 144);
            this.pnlResultTop.TabIndex = 0;
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreate.Enabled = false;
            this.btnCreate.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnCreate.Image = ((System.Drawing.Image)(resources.GetObject("btnCreate.Image")));
            this.btnCreate.Location = new System.Drawing.Point(298, 94);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(206, 42);
            this.btnCreate.TabIndex = 5;
            this.btnCreate.Text = "Chain 생성";
            this.btnCreate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnResultRemove
            // 
            this.btnResultRemove.Enabled = false;
            this.btnResultRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnResultRemove.Image")));
            this.btnResultRemove.Location = new System.Drawing.Point(6, 94);
            this.btnResultRemove.Name = "btnResultRemove";
            this.btnResultRemove.Size = new System.Drawing.Size(166, 42);
            this.btnResultRemove.TabIndex = 4;
            this.btnResultRemove.Text = "목록에서 빼기";
            this.btnResultRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnResultRemove.UseVisualStyleBackColor = true;
            this.btnResultRemove.Click += new System.EventHandler(this.btnResultRemove_Click);
            // 
            // cboResultClass
            // 
            this.cboResultClass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboResultClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboResultClass.FormattingEnabled = true;
            this.cboResultClass.Items.AddRange(new object[] {
            "전체",
            "일치",
            "검토"});
            this.cboResultClass.Location = new System.Drawing.Point(354, 58);
            this.cboResultClass.Name = "cboResultClass";
            this.cboResultClass.Size = new System.Drawing.Size(150, 26);
            this.cboResultClass.TabIndex = 3;
            this.cboResultClass.SelectedIndexChanged += new System.EventHandler(this.ResultFilter_Changed);
            // 
            // txtResultFind
            // 
            this.txtResultFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResultFind.Location = new System.Drawing.Point(62, 57);
            this.txtResultFind.Name = "txtResultFind";
            this.txtResultFind.Size = new System.Drawing.Size(284, 28);
            this.txtResultFind.TabIndex = 2;
            this.txtResultFind.TextChanged += new System.EventHandler(this.ResultFilter_Changed);
            // 
            // lblResultFind
            // 
            this.lblResultFind.AutoSize = true;
            this.lblResultFind.Location = new System.Drawing.Point(6, 61);
            this.lblResultFind.Name = "lblResultFind";
            this.lblResultFind.Size = new System.Drawing.Size(44, 18);
            this.lblResultFind.TabIndex = 1;
            this.lblResultFind.Text = "찾기";
            // 
            // lblResultSummary
            // 
            this.lblResultSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblResultSummary.Location = new System.Drawing.Point(0, 0);
            this.lblResultSummary.Name = "lblResultSummary";
            this.lblResultSummary.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblResultSummary.Size = new System.Drawing.Size(508, 48);
            this.lblResultSummary.TabIndex = 0;
            this.lblResultSummary.Text = "예시를 지정한 뒤 [같은 형태 찾기]";
            this.lblResultSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabSaved
            // 
            this.tabSaved.Controls.Add(this.dgvSaved);
            this.tabSaved.Controls.Add(this.pnlSavedTop);
            this.tabSaved.Location = new System.Drawing.Point(4, 28);
            this.tabSaved.Name = "tabSaved";
            this.tabSaved.Padding = new System.Windows.Forms.Padding(3);
            this.tabSaved.Size = new System.Drawing.Size(514, 1033);
            this.tabSaved.TabIndex = 1;
            this.tabSaved.Text = "저장된 Chain";
            this.tabSaved.UseVisualStyleBackColor = true;
            // 
            // dgvSaved
            // 
            this.dgvSaved.AllowUserToAddRows = false;
            this.dgvSaved.AllowUserToDeleteRows = false;
            this.dgvSaved.AllowUserToResizeRows = false;
            this.dgvSaved.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvSaved.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSaved.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSavedShow,
            this.colSavedColor,
            this.colSavedName,
            this.colSavedCode,
            this.colSavedStatus});
            this.dgvSaved.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSaved.Location = new System.Drawing.Point(3, 195);
            this.dgvSaved.Name = "dgvSaved";
            this.dgvSaved.ReadOnly = true;
            this.dgvSaved.RowHeadersVisible = false;
            this.dgvSaved.RowHeadersWidth = 62;
            this.dgvSaved.RowTemplate.Height = 27;
            this.dgvSaved.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSaved.Size = new System.Drawing.Size(508, 835);
            this.dgvSaved.TabIndex = 1;
            this.dgvSaved.VirtualMode = true;
            this.dgvSaved.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSaved_CellContentClick);
            this.dgvSaved.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSaved_CellContentDoubleClick);
            this.dgvSaved.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvSaved_CellValueNeeded);
            this.dgvSaved.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSaved_CellFormatting);
            this.dgvSaved.SelectionChanged += new System.EventHandler(this.dgvSaved_SelectionChanged);
            // 
            // colSavedShow
            // 
            this.colSavedShow.HeaderText = "표시";
            this.colSavedShow.MinimumWidth = 8;
            this.colSavedShow.Name = "colSavedShow";
            this.colSavedShow.ReadOnly = true;
            this.colSavedShow.Width = 56;
            // 
            // colSavedColor
            // 
            this.colSavedColor.HeaderText = "색";
            this.colSavedColor.MinimumWidth = 8;
            this.colSavedColor.Name = "colSavedColor";
            this.colSavedColor.ReadOnly = true;
            this.colSavedColor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colSavedColor.Width = 40;
            // 
            // colSavedName
            // 
            this.colSavedName.HeaderText = "이름";
            this.colSavedName.MinimumWidth = 8;
            this.colSavedName.Name = "colSavedName";
            this.colSavedName.ReadOnly = true;
            this.colSavedName.Width = 170;
            // 
            // colSavedCode
            // 
            this.colSavedCode.HeaderText = "Code";
            this.colSavedCode.MinimumWidth = 8;
            this.colSavedCode.Name = "colSavedCode";
            this.colSavedCode.ReadOnly = true;
            this.colSavedCode.Width = 130;
            // 
            // colSavedStatus
            // 
            this.colSavedStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSavedStatus.HeaderText = "상태";
            this.colSavedStatus.MinimumWidth = 8;
            this.colSavedStatus.Name = "colSavedStatus";
            this.colSavedStatus.ReadOnly = true;
            // 
            // pnlSavedTop
            // 
            this.pnlSavedTop.Controls.Add(this.lblSavedSummary);
            this.pnlSavedTop.Controls.Add(this.txtSavedFind);
            this.pnlSavedTop.Controls.Add(this.lblSavedFind);
            this.pnlSavedTop.Controls.Add(this.btnSavedDeleteShown);
            this.pnlSavedTop.Controls.Add(this.btnSavedDelete);
            this.pnlSavedTop.Controls.Add(this.btnSavedRefresh);
            this.pnlSavedTop.Controls.Add(this.btnSavedCheckAll);
            this.pnlSavedTop.Controls.Add(this.btnSavedCheckNone);
            this.pnlSavedTop.Controls.Add(this.btnSavedColor);
            this.pnlSavedTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSavedTop.Location = new System.Drawing.Point(3, 3);
            this.pnlSavedTop.Name = "pnlSavedTop";
            this.pnlSavedTop.Size = new System.Drawing.Size(508, 192);
            this.pnlSavedTop.TabIndex = 0;
            // 
            // lblSavedSummary
            // 
            this.lblSavedSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSavedSummary.Location = new System.Drawing.Point(6, 152);
            this.lblSavedSummary.Name = "lblSavedSummary";
            this.lblSavedSummary.Size = new System.Drawing.Size(498, 34);
            this.lblSavedSummary.TabIndex = 5;
            this.lblSavedSummary.Text = "Chain 0개";
            this.lblSavedSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSavedFind
            // 
            this.txtSavedFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSavedFind.Location = new System.Drawing.Point(62, 114);
            this.txtSavedFind.Name = "txtSavedFind";
            this.txtSavedFind.Size = new System.Drawing.Size(442, 28);
            this.txtSavedFind.TabIndex = 4;
            this.txtSavedFind.TextChanged += new System.EventHandler(this.txtSavedFind_TextChanged);
            // 
            // lblSavedFind
            // 
            this.lblSavedFind.AutoSize = true;
            this.lblSavedFind.Location = new System.Drawing.Point(6, 118);
            this.lblSavedFind.Name = "lblSavedFind";
            this.lblSavedFind.Size = new System.Drawing.Size(44, 18);
            this.lblSavedFind.TabIndex = 3;
            this.lblSavedFind.Text = "찾기";
            // 
            // btnSavedDeleteShown
            // 
            this.btnSavedDeleteShown.Image = ((System.Drawing.Image)(resources.GetObject("btnSavedDeleteShown.Image")));
            this.btnSavedDeleteShown.Location = new System.Drawing.Point(290, 8);
            this.btnSavedDeleteShown.Name = "btnSavedDeleteShown";
            this.btnSavedDeleteShown.Size = new System.Drawing.Size(196, 42);
            this.btnSavedDeleteShown.TabIndex = 2;
            this.btnSavedDeleteShown.Text = "목록 모두 삭제";
            this.btnSavedDeleteShown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSavedDeleteShown.UseVisualStyleBackColor = true;
            this.btnSavedDeleteShown.Click += new System.EventHandler(this.btnSavedDeleteShown_Click);
            // 
            // btnSavedDelete
            // 
            this.btnSavedDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnSavedDelete.Image")));
            this.btnSavedDelete.Location = new System.Drawing.Point(148, 8);
            this.btnSavedDelete.Name = "btnSavedDelete";
            this.btnSavedDelete.Size = new System.Drawing.Size(136, 42);
            this.btnSavedDelete.TabIndex = 1;
            this.btnSavedDelete.Text = "선택 삭제";
            this.btnSavedDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSavedDelete.UseVisualStyleBackColor = true;
            this.btnSavedDelete.Click += new System.EventHandler(this.btnSavedDelete_Click);
            // 
            // btnSavedRefresh
            // 
            this.btnSavedRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnSavedRefresh.Image")));
            this.btnSavedRefresh.Location = new System.Drawing.Point(6, 8);
            this.btnSavedRefresh.Name = "btnSavedRefresh";
            this.btnSavedRefresh.Size = new System.Drawing.Size(136, 42);
            this.btnSavedRefresh.TabIndex = 0;
            this.btnSavedRefresh.Text = "다시 읽기";
            this.btnSavedRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSavedRefresh.UseVisualStyleBackColor = true;
            this.btnSavedRefresh.Click += new System.EventHandler(this.btnSavedRefresh_Click);
            // 
            // btnSavedCheckAll
            // 
            this.btnSavedCheckAll.Image = ((System.Drawing.Image)(resources.GetObject("btnSavedCheckAll.Image")));
            this.btnSavedCheckAll.Location = new System.Drawing.Point(6, 56);
            this.btnSavedCheckAll.Name = "btnSavedCheckAll";
            this.btnSavedCheckAll.Size = new System.Drawing.Size(160, 42);
            this.btnSavedCheckAll.TabIndex = 6;
            this.btnSavedCheckAll.Text = "전체 체크";
            this.btnSavedCheckAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSavedCheckAll.UseVisualStyleBackColor = true;
            this.btnSavedCheckAll.Click += new System.EventHandler(this.btnSavedCheckAll_Click);
            // 
            // btnSavedCheckNone
            // 
            this.btnSavedCheckNone.Image = ((System.Drawing.Image)(resources.GetObject("btnSavedCheckNone.Image")));
            this.btnSavedCheckNone.Location = new System.Drawing.Point(172, 56);
            this.btnSavedCheckNone.Name = "btnSavedCheckNone";
            this.btnSavedCheckNone.Size = new System.Drawing.Size(160, 42);
            this.btnSavedCheckNone.TabIndex = 7;
            this.btnSavedCheckNone.Text = "전체 미체크";
            this.btnSavedCheckNone.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSavedCheckNone.UseVisualStyleBackColor = true;
            this.btnSavedCheckNone.Click += new System.EventHandler(this.btnSavedCheckNone_Click);
            // 
            // btnSavedColor
            // 
            this.btnSavedColor.Image = ((System.Drawing.Image)(resources.GetObject("btnSavedColor.Image")));
            this.btnSavedColor.Location = new System.Drawing.Point(338, 56);
            this.btnSavedColor.Name = "btnSavedColor";
            this.btnSavedColor.Size = new System.Drawing.Size(148, 42);
            this.btnSavedColor.TabIndex = 8;
            this.btnSavedColor.Text = "색 변경";
            this.btnSavedColor.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSavedColor.UseVisualStyleBackColor = true;
            this.btnSavedColor.Click += new System.EventHandler(this.btnSavedColor_Click);
            // 
            // tabLayer
            // 
            this.tabLayer.Controls.Add(this.chkLayerItems);
            this.tabLayer.Controls.Add(this.pnlLayerButtons);
            this.tabLayer.Location = new System.Drawing.Point(4, 28);
            this.tabLayer.Name = "tabLayer";
            this.tabLayer.Padding = new System.Windows.Forms.Padding(3);
            this.tabLayer.Size = new System.Drawing.Size(514, 1033);
            this.tabLayer.TabIndex = 2;
            this.tabLayer.Text = "Layer";
            this.tabLayer.UseVisualStyleBackColor = true;
            // 
            // chkLayerItems
            // 
            this.chkLayerItems.CheckOnCheckBoxOnly = true;
            this.chkLayerItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkLayerItems.FormattingEnabled = true;
            this.chkLayerItems.IntegralHeight = false;
            this.chkLayerItems.Location = new System.Drawing.Point(3, 57);
            this.chkLayerItems.Margin = new System.Windows.Forms.Padding(4);
            this.chkLayerItems.Name = "chkLayerItems";
            this.chkLayerItems.Size = new System.Drawing.Size(508, 973);
            this.chkLayerItems.TabIndex = 1;
            this.chkLayerItems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkLayerItems_ItemCheck);
            this.chkLayerItems.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.chkLayerItems_Format);
            // 
            // pnlLayerButtons
            // 
            this.pnlLayerButtons.Controls.Add(this.btnLayerNone);
            this.pnlLayerButtons.Controls.Add(this.btnLayerAll);
            this.pnlLayerButtons.Controls.Add(this.lblLayerTitle);
            this.pnlLayerButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLayerButtons.Location = new System.Drawing.Point(3, 3);
            this.pnlLayerButtons.Name = "pnlLayerButtons";
            this.pnlLayerButtons.Size = new System.Drawing.Size(508, 54);
            this.pnlLayerButtons.TabIndex = 0;
            // 
            // btnLayerNone
            // 
            this.btnLayerNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLayerNone.Image = ((System.Drawing.Image)(resources.GetObject("btnLayerNone.Image")));
            this.btnLayerNone.Location = new System.Drawing.Point(415, 6);
            this.btnLayerNone.Name = "btnLayerNone";
            this.btnLayerNone.Size = new System.Drawing.Size(90, 42);
            this.btnLayerNone.TabIndex = 2;
            this.btnLayerNone.Text = "해제";
            this.btnLayerNone.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLayerNone.UseVisualStyleBackColor = true;
            this.btnLayerNone.Click += new System.EventHandler(this.btnLayerNone_Click);
            // 
            // btnLayerAll
            // 
            this.btnLayerAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLayerAll.Image = ((System.Drawing.Image)(resources.GetObject("btnLayerAll.Image")));
            this.btnLayerAll.Location = new System.Drawing.Point(319, 6);
            this.btnLayerAll.Name = "btnLayerAll";
            this.btnLayerAll.Size = new System.Drawing.Size(90, 42);
            this.btnLayerAll.TabIndex = 1;
            this.btnLayerAll.Text = "전체";
            this.btnLayerAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLayerAll.UseVisualStyleBackColor = true;
            this.btnLayerAll.Click += new System.EventHandler(this.btnLayerAll_Click);
            // 
            // lblLayerTitle
            // 
            this.lblLayerTitle.AutoSize = true;
            this.lblLayerTitle.Location = new System.Drawing.Point(4, 18);
            this.lblLayerTitle.Name = "lblLayerTitle";
            this.lblLayerTitle.Size = new System.Drawing.Size(131, 18);
            this.lblLayerTitle.TabIndex = 0;
            this.lblLayerTitle.Text = "Layer";
            // 
            // statusStripView
            // 
            this.statusStripView.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStripView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressLoad,
            this.lblElapsed,
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
            // lblElapsed
            // 
            this.lblElapsed.ForeColor = System.Drawing.Color.DimGray;
            this.lblElapsed.Name = "lblElapsed";
            this.lblElapsed.Size = new System.Drawing.Size(0, 29);
            this.lblElapsed.Visible = false;
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
            // GdsMapViewU8Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1714, 1155);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.pnlCondition);
            this.Controls.Add(this.statusStripView);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GdsMapViewU8Form";
            this.Text = "GDS Map 조회 U8";
            this.Shown += new System.EventHandler(this.GdsMapViewU8Form_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GdsMapViewU8Form_KeyDown);
            this.pnlCondition.ResumeLayout(false);
            this.pnlCondition.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel1.PerformLayout();
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.pnlU8Bar.ResumeLayout(false);
            this.pnlU8Bar.PerformLayout();
            this.tabRight.ResumeLayout(false);
            this.tabResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).EndInit();
            this.pnlResultTop.ResumeLayout(false);
            this.pnlResultTop.PerformLayout();
            this.tabSaved.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSaved)).EndInit();
            this.pnlSavedTop.ResumeLayout(false);
            this.pnlSavedTop.PerformLayout();
            this.tabLayer.ResumeLayout(false);
            this.pnlLayerButtons.ResumeLayout(false);
            this.pnlLayerButtons.PerformLayout();
            this.statusStripView.ResumeLayout(false);
            this.statusStripView.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

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
        private System.Windows.Forms.SplitContainer splitMain;
        private NexplantQMS.GdsMap.GdsMapControl map;
        private System.Windows.Forms.FlowLayoutPanel pnlU8Bar;
        private System.Windows.Forms.Label lblTemplate;
        private System.Windows.Forms.Button btnTemplate;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Button btnFindStop;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnResultRemove;
        private System.Windows.Forms.TabControl tabRight;
        private System.Windows.Forms.TabPage tabResult;
        private System.Windows.Forms.DataGridView dgvResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResultName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResultClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResultDb;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResultPos;
        private System.Windows.Forms.Panel pnlResultTop;
        private System.Windows.Forms.ComboBox cboResultClass;
        private System.Windows.Forms.TextBox txtResultFind;
        private System.Windows.Forms.Label lblResultFind;
        private System.Windows.Forms.Label lblResultSummary;
        private System.Windows.Forms.TabPage tabSaved;
        private System.Windows.Forms.DataGridView dgvSaved;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSavedShow;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSavedColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSavedName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSavedCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSavedStatus;
        private System.Windows.Forms.Panel pnlSavedTop;
        private System.Windows.Forms.Label lblSavedSummary;
        private System.Windows.Forms.TextBox txtSavedFind;
        private System.Windows.Forms.Label lblSavedFind;
        private System.Windows.Forms.Button btnSavedDeleteShown;
        private System.Windows.Forms.Button btnSavedDelete;
        private System.Windows.Forms.Button btnSavedRefresh;
        private System.Windows.Forms.Button btnSavedCheckAll;
        private System.Windows.Forms.Button btnSavedCheckNone;
        private System.Windows.Forms.Button btnSavedColor;
        private System.Windows.Forms.TabPage tabLayer;
        private NexplantQMS.GdsMap.LayerColorCheckedListBox chkLayerItems;
        private System.Windows.Forms.Panel pnlLayerButtons;
        private System.Windows.Forms.Button btnLayerNone;
        private System.Windows.Forms.Button btnLayerAll;
        private System.Windows.Forms.Label lblLayerTitle;
        private System.Windows.Forms.StatusStrip statusStripView;
        private System.Windows.Forms.ToolStripProgressBar progressLoad;
        private System.Windows.Forms.ToolStripStatusLabel lblElapsed;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblCoordinate;
        private System.Windows.Forms.ToolTip toolTipView;
    }
}
