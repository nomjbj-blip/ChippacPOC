namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// Die 상세 분석 화면 배치. 설계: 문서/설계/05_연관분석/2026-10-12_GDS_KLARF_연관분석_화면_설계.md 5.1절,
    ///       문서/설계/05_연관분석/2026-10-15_GDS_KLARF_ETest_통합분석_데이터_설계.md 8.2 / 15.3절 (2026-10-16 E-Test 값 / 역추적),
    ///       문서/설계/05_연관분석/2026-10-19_화면A_레이아웃_후보.md 10절 (2026-10-19 화면 A 와 같은 기능 탭)
    ///       문서/설계/05_연관분석/2026-10-20_상세보기_GDS_KLARF_합친탭_설계.md (2026-10-20 GDS / KLARF 탭 합침)
    /// </summary>
    partial class GdsDieAnalysisForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null) components.Dispose();
                if (_tabBoldFont != null) _tabBoldFont.Dispose();
                if (_gridBoldFont != null) _gridBoldFont.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 위: 1줄 Wafer / 분석 기준 요약(GDS · KLARF · E-Test), 2줄 Die 정보 + 역추적 Chain + [역추적 해제]
        /// 가운데: 왼쪽 GDS 도면(머리 = 보기 면 / 배경 도면 / 원점 / 전체 보기) / 오른쪽 기능 탭 [GDS / KLARF] [E-Test] [종합 (Die)]
        ///   탭마다 위에 그 기능의 기준(읽기 전용), 아래에 그 기능의 표 (2026-10-19, 화면 A 와 같은 구조)
        ///   [GDS / KLARF] = Chain 칸 + 그 Chain 에 걸린 Defect 칸을 한 표로, 맨 아래 Chain 밖 Defect 묶음 (2026-10-20, GDS / KLARF 탭 합침)
        /// 아래: 상태줄(진행 / 좌표)
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GdsDieAnalysisForm));
            this.components = new System.ComponentModel.Container();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.flowCriteria = new System.Windows.Forms.FlowLayoutPanel();
            this.lblWaferInfo = new System.Windows.Forms.Label();
            this.lblSumTitle = new System.Windows.Forms.Label();
            this.lblSumGds = new System.Windows.Forms.Label();
            this.lblSumKlarf = new System.Windows.Forms.Label();
            this.lblSumEt = new System.Windows.Forms.Label();
            this.lblDieInfo = new System.Windows.Forms.Label();
            this.lblFocus = new System.Windows.Forms.Label();
            this.btnClearFocus = new System.Windows.Forms.Button();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.map = new NexplantQMS.GdsMap.GdsMapControl();
            this.pnlMapHead = new System.Windows.Forms.Panel();
            this.lblMapTitle = new System.Windows.Forms.Label();
            this.lblMapSide = new System.Windows.Forms.Label();
            this.chkBackground = new System.Windows.Forms.CheckBox();
            this.chkOriginCross = new System.Windows.Forms.CheckBox();
            this.btnFit = new System.Windows.Forms.Button();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabChainDefect = new System.Windows.Forms.TabPage();
            this.dgvChainDefects = new System.Windows.Forms.DataGridView();
            this.colShow = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFailPara = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMember = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDefectId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRoughBin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClassNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGdsX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGdsY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colXRel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colYRel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlChainHead = new System.Windows.Forms.Panel();
            this.lblChainTitle = new System.Windows.Forms.Label();
            this.chkHitOnly = new System.Windows.Forms.CheckBox();
            this.chkOutside = new System.Windows.Forms.CheckBox();
            this.pnlGdsCriteria = new System.Windows.Forms.Panel();
            this.lblGdsCriteria = new System.Windows.Forms.Label();
            this.lblGdsText = new System.Windows.Forms.Label();
            this.tabEt = new System.Windows.Forms.TabPage();
            this.dgvEtValues = new System.Windows.Forms.DataGridView();
            this.pnlEtHead = new System.Windows.Forms.Panel();
            this.lblEtCount = new System.Windows.Forms.Label();
            this.pnlEtCriteria = new System.Windows.Forms.Panel();
            this.lblEtCriteria = new System.Windows.Forms.Label();
            this.lblEtText = new System.Windows.Forms.Label();
            this.tabDie = new System.Windows.Forms.TabPage();
            this.dgvSummary = new System.Windows.Forms.DataGridView();
            this.pnlSummaryHead = new System.Windows.Forms.Panel();
            this.lblSummaryTitle = new System.Windows.Forms.Label();
            this.pnlDieSummary = new System.Windows.Forms.Panel();
            this.lblDieCriteria = new System.Windows.Forms.Label();
            this.lblDieText = new System.Windows.Forms.Label();
            this.flowClass = new System.Windows.Forms.FlowLayoutPanel();
            this.lblClassBoth = new System.Windows.Forms.Label();
            this.lblClassFail = new System.Windows.Forms.Label();
            this.lblClassDefect = new System.Windows.Forms.Label();
            this.lblClassOk = new System.Windows.Forms.Label();
            this.lblClassNa = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.progressLoad = new System.Windows.Forms.ToolStripProgressBar();
            this.lblElapsed = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCoordinate = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.pnlTop.SuspendLayout();
            this.flowCriteria.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.pnlMapHead.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabChainDefect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChainDefects)).BeginInit();
            this.pnlChainHead.SuspendLayout();
            this.pnlGdsCriteria.SuspendLayout();
            this.tabEt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEtValues)).BeginInit();
            this.pnlEtHead.SuspendLayout();
            this.pnlEtCriteria.SuspendLayout();
            this.tabDie.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSummary)).BeginInit();
            this.pnlSummaryHead.SuspendLayout();
            this.pnlDieSummary.SuspendLayout();
            this.flowClass.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlTop
            //
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(242)))), ((int)(((byte)(250)))));
            this.pnlTop.Controls.Add(this.flowCriteria);
            this.pnlTop.Controls.Add(this.lblDieInfo);
            this.pnlTop.Controls.Add(this.lblFocus);
            this.pnlTop.Controls.Add(this.btnClearFocus);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1400, 70);
            this.pnlTop.TabIndex = 0;
            //
            // flowCriteria
            //
            this.flowCriteria.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowCriteria.Controls.Add(this.lblWaferInfo);
            this.flowCriteria.Controls.Add(this.lblSumTitle);
            this.flowCriteria.Controls.Add(this.lblSumGds);
            this.flowCriteria.Controls.Add(this.lblSumKlarf);
            this.flowCriteria.Controls.Add(this.lblSumEt);
            this.flowCriteria.Location = new System.Drawing.Point(4, 2);
            this.flowCriteria.Margin = new System.Windows.Forms.Padding(0);
            this.flowCriteria.Name = "flowCriteria";
            this.flowCriteria.Size = new System.Drawing.Size(1392, 32);
            this.flowCriteria.TabIndex = 0;
            this.flowCriteria.WrapContents = false;
            //
            // lblWaferInfo
            //
            this.lblWaferInfo.AutoSize = true;
            this.lblWaferInfo.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblWaferInfo.Margin = new System.Windows.Forms.Padding(4, 8, 16, 0);
            this.lblWaferInfo.Name = "lblWaferInfo";
            this.lblWaferInfo.Size = new System.Drawing.Size(10, 17);
            this.lblWaferInfo.TabIndex = 0;
            this.lblWaferInfo.Text = "";
            //
            // lblSumTitle
            //
            this.lblSumTitle.AutoSize = true;
            this.lblSumTitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblSumTitle.Margin = new System.Windows.Forms.Padding(4, 10, 2, 0);
            this.lblSumTitle.Name = "lblSumTitle";
            this.lblSumTitle.Size = new System.Drawing.Size(53, 12);
            this.lblSumTitle.TabIndex = 1;
            this.lblSumTitle.Text = "분석 기준";
            //
            // lblSumGds
            //
            this.lblSumGds.AutoSize = true;
            this.lblSumGds.BackColor = System.Drawing.Color.White;
            this.lblSumGds.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSumGds.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(87)))), ((int)(((byte)(154)))));
            this.lblSumGds.Margin = new System.Windows.Forms.Padding(4, 5, 4, 0);
            this.lblSumGds.Name = "lblSumGds";
            this.lblSumGds.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lblSumGds.Size = new System.Drawing.Size(60, 20);
            this.lblSumGds.TabIndex = 2;
            this.lblSumGds.Text = "GDS · -";
            //
            // lblSumKlarf
            //
            this.lblSumKlarf.AutoSize = true;
            this.lblSumKlarf.BackColor = System.Drawing.Color.White;
            this.lblSumKlarf.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSumKlarf.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.lblSumKlarf.Margin = new System.Windows.Forms.Padding(4, 5, 4, 0);
            this.lblSumKlarf.Name = "lblSumKlarf";
            this.lblSumKlarf.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lblSumKlarf.Size = new System.Drawing.Size(60, 20);
            this.lblSumKlarf.TabIndex = 3;
            this.lblSumKlarf.Text = "KLARF · -";
            //
            // lblSumEt
            //
            this.lblSumEt.AutoSize = true;
            this.lblSumEt.BackColor = System.Drawing.Color.White;
            this.lblSumEt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSumEt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(95)))), ((int)(((byte)(0)))));
            this.lblSumEt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 0);
            this.lblSumEt.Name = "lblSumEt";
            this.lblSumEt.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lblSumEt.Size = new System.Drawing.Size(60, 20);
            this.lblSumEt.TabIndex = 4;
            this.lblSumEt.Text = "E-Test · -";
            //
            // lblDieInfo
            //
            this.lblDieInfo.AutoSize = true;
            this.lblDieInfo.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblDieInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(60)))), ((int)(((byte)(130)))));
            this.lblDieInfo.Location = new System.Drawing.Point(10, 41);
            this.lblDieInfo.Name = "lblDieInfo";
            this.lblDieInfo.Size = new System.Drawing.Size(120, 20);
            this.lblDieInfo.TabIndex = 1;
            this.lblDieInfo.Text = "Die (-, -)";
            //
            // lblFocus
            //
            this.lblFocus.AutoSize = true;
            this.lblFocus.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblFocus.ForeColor = System.Drawing.Color.Firebrick;
            this.lblFocus.Location = new System.Drawing.Point(870, 44);
            this.lblFocus.Name = "lblFocus";
            this.lblFocus.Size = new System.Drawing.Size(100, 17);
            this.lblFocus.TabIndex = 2;
            this.lblFocus.Text = "역추적: -";
            this.lblFocus.Visible = false;
            //
            // btnClearFocus
            //
            this.btnClearFocus.Location = new System.Drawing.Point(1180, 37);
            this.btnClearFocus.Name = "btnClearFocus";
            this.btnClearFocus.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClearFocus.Image = ((System.Drawing.Image)(resources.GetObject("btnClearFocus.Image")));
            this.btnClearFocus.Size = new System.Drawing.Size(112, 28);
            this.btnClearFocus.TabIndex = 3;
            this.btnClearFocus.Text = "역추적 해제";
            this.btnClearFocus.UseVisualStyleBackColor = true;
            this.btnClearFocus.Visible = false;
            this.btnClearFocus.Click += new System.EventHandler(this.btnClearFocus_Click);
            //
            // splitMain
            //
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitMain.Location = new System.Drawing.Point(0, 70);
            this.splitMain.Name = "splitMain";
            //
            // splitMain.Panel1
            //
            this.splitMain.Panel1.Controls.Add(this.map);
            this.splitMain.Panel1.Controls.Add(this.pnlMapHead);
            //
            // splitMain.Panel2
            //
            this.splitMain.Panel2.Controls.Add(this.tabMain);
            this.splitMain.Size = new System.Drawing.Size(1400, 808);
            this.splitMain.SplitterDistance = 696;
            this.splitMain.TabIndex = 1;
            //
            // map
            //
            this.map.BackColor = System.Drawing.Color.Black;
            this.map.ColorAlpha = 50;
            this.map.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.Location = new System.Drawing.Point(0, 32);
            this.map.Mode = NexplantQMS.GdsMap.GdsMapControl.ViewMode.View;
            this.map.Name = "map";
            this.map.Size = new System.Drawing.Size(696, 776);
            this.map.TabIndex = 1;
            this.map.VSync = false;
            this.map.MouseWorldPositionChanged += new System.EventHandler<System.Drawing.PointF>(this.map_MouseWorldPositionChanged);
            this.map.MouseClick += new System.Windows.Forms.MouseEventHandler(this.map_MouseClick);
            //
            // pnlMapHead
            //
            this.pnlMapHead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.pnlMapHead.Controls.Add(this.lblMapTitle);
            this.pnlMapHead.Controls.Add(this.lblMapSide);
            this.pnlMapHead.Controls.Add(this.chkBackground);
            this.pnlMapHead.Controls.Add(this.chkOriginCross);
            this.pnlMapHead.Controls.Add(this.btnFit);
            this.pnlMapHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMapHead.Location = new System.Drawing.Point(0, 0);
            this.pnlMapHead.Name = "pnlMapHead";
            this.pnlMapHead.Size = new System.Drawing.Size(696, 32);
            this.pnlMapHead.TabIndex = 0;
            //
            // lblMapTitle
            //
            this.lblMapTitle.AutoSize = true;
            this.lblMapTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblMapTitle.Location = new System.Drawing.Point(6, 8);
            this.lblMapTitle.Name = "lblMapTitle";
            this.lblMapTitle.Size = new System.Drawing.Size(56, 15);
            this.lblMapTitle.TabIndex = 0;
            this.lblMapTitle.Text = "GDS 도면";
            //
            // lblMapSide
            //
            this.lblMapSide.AutoSize = true;
            this.lblMapSide.ForeColor = System.Drawing.Color.DimGray;
            this.lblMapSide.Location = new System.Drawing.Point(72, 10);
            this.lblMapSide.Name = "lblMapSide";
            this.lblMapSide.Size = new System.Drawing.Size(60, 12);
            this.lblMapSide.TabIndex = 1;
            this.lblMapSide.Text = "보기 = -";
            //
            // chkBackground
            //
            this.chkBackground.AutoSize = true;
            this.chkBackground.Location = new System.Drawing.Point(270, 8);
            this.chkBackground.Name = "chkBackground";
            this.chkBackground.Size = new System.Drawing.Size(150, 16);
            this.chkBackground.TabIndex = 2;
            this.chkBackground.Text = "배경 도면(전체 Layer)";
            this.chkBackground.UseVisualStyleBackColor = true;
            this.chkBackground.CheckedChanged += new System.EventHandler(this.chkBackground_CheckedChanged);
            //
            // chkOriginCross
            //
            this.chkOriginCross.AutoSize = true;
            this.chkOriginCross.Checked = true;
            this.chkOriginCross.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOriginCross.Location = new System.Drawing.Point(430, 8);
            this.chkOriginCross.Name = "chkOriginCross";
            this.chkOriginCross.Size = new System.Drawing.Size(76, 16);
            this.chkOriginCross.TabIndex = 3;
            this.chkOriginCross.Text = "원점 표시";
            this.chkOriginCross.UseVisualStyleBackColor = true;
            this.chkOriginCross.CheckedChanged += new System.EventHandler(this.chkOriginCross_CheckedChanged);
            //
            // btnFit
            //
            this.btnFit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFit.Location = new System.Drawing.Point(596, 2);
            this.btnFit.Name = "btnFit";
            this.btnFit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFit.Image = ((System.Drawing.Image)(resources.GetObject("btnFit.Image")));
            this.btnFit.Size = new System.Drawing.Size(96, 28);
            this.btnFit.TabIndex = 4;
            this.btnFit.Text = "전체 보기";
            this.toolTip.SetToolTip(this.btnFit, "Chip 전체 보기");
            this.btnFit.UseVisualStyleBackColor = true;
            this.btnFit.Click += new System.EventHandler(this.btnFit_Click);
            //
            // tabMain
            //
            this.tabMain.Controls.Add(this.tabChainDefect);
            this.tabMain.Controls.Add(this.tabEt);
            this.tabMain.Controls.Add(this.tabDie);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabMain.ItemSize = new System.Drawing.Size(150, 30);
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 2;
            this.tabMain.Size = new System.Drawing.Size(700, 808);
            this.tabMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabMain.TabIndex = 0;
            this.tabMain.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabMain_DrawItem);
            this.tabMain.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabMain_Selecting);
            this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
            //
            // tabChainDefect
            //
            this.tabChainDefect.Controls.Add(this.dgvChainDefects);
            this.tabChainDefect.Controls.Add(this.pnlChainHead);
            this.tabChainDefect.Controls.Add(this.pnlGdsCriteria);
            this.tabChainDefect.Location = new System.Drawing.Point(4, 34);
            this.tabChainDefect.Name = "tabChainDefect";
            this.tabChainDefect.Size = new System.Drawing.Size(692, 770);
            this.tabChainDefect.TabIndex = 0;
            this.tabChainDefect.Text = "GDS / KLARF";
            this.tabChainDefect.UseVisualStyleBackColor = true;
            //
            // dgvChainDefects
            //
            this.dgvChainDefects.AllowUserToAddRows = false;
            this.dgvChainDefects.AllowUserToDeleteRows = false;
            this.dgvChainDefects.AllowUserToResizeRows = false;
            this.dgvChainDefects.BackgroundColor = System.Drawing.Color.White;
            this.dgvChainDefects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChainDefects.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colShow,
            this.colColor,
            this.colCode,
            this.colCount,
            this.colClass,
            this.colEt,
            this.colFailPara,
            this.colMember,
            this.colDefectId,
            this.colGap,
            this.colNear,
            this.colRoughBin,
            this.colClassNo,
            this.colSize,
            this.colGdsX,
            this.colGdsY,
            this.colXRel,
            this.colYRel});
            this.dgvChainDefects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvChainDefects.Location = new System.Drawing.Point(0, 100);
            this.dgvChainDefects.MultiSelect = false;
            this.dgvChainDefects.Name = "dgvChainDefects";
            this.dgvChainDefects.RowHeadersVisible = false;
            this.dgvChainDefects.RowTemplate.Height = 23;
            this.dgvChainDefects.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvChainDefects.Size = new System.Drawing.Size(692, 670);
            this.dgvChainDefects.TabIndex = 2;
            this.dgvChainDefects.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChainDefects_CellContentClick);
            this.dgvChainDefects.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChainDefects_CellDoubleClick);
            this.dgvChainDefects.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvChainDefects_CellPainting);
            this.dgvChainDefects.SelectionChanged += new System.EventHandler(this.dgvChainDefects_SelectionChanged);
            //
            // colShow
            //
            this.colShow.HeaderText = "표시";
            this.colShow.Name = "colShow";
            this.colShow.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colShow.Width = 40;
            //
            // colColor
            //
            this.colColor.HeaderText = "색";
            this.colColor.Name = "colColor";
            this.colColor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colColor.ReadOnly = true;
            this.colColor.Width = 30;
            //
            // colCode
            //
            this.colCode.HeaderText = "Chain";
            this.colCode.Name = "colCode";
            this.colCode.ReadOnly = true;
            this.colCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCode.Width = 140;
            //
            // colCount
            //
            this.colCount.HeaderText = "Defect 수";
            this.colCount.Name = "colCount";
            this.colCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCount.ReadOnly = true;
            this.colCount.Width = 68;
            //
            // colClass
            //
            this.colClass.HeaderText = "분류";
            this.colClass.Name = "colClass";
            this.colClass.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colClass.ReadOnly = true;
            this.colClass.Width = 64;
            //
            // colEt
            //
            this.colEt.HeaderText = "ET";
            this.colEt.Name = "colEt";
            this.colEt.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colEt.ReadOnly = true;
            this.colEt.Width = 34;
            //
            // colFailPara
            //
            this.colFailPara.HeaderText = "Fail Para";
            this.colFailPara.Name = "colFailPara";
            this.colFailPara.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colFailPara.ReadOnly = true;
            this.colFailPara.Width = 64;
            //
            // colMember
            //
            this.colMember.HeaderText = "Member";
            this.colMember.Name = "colMember";
            this.colMember.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colMember.ReadOnly = true;
            this.colMember.Width = 56;
            //
            // colDefectId
            //
            this.colDefectId.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colDefectId.HeaderText = "Defect ID";
            this.colDefectId.Name = "colDefectId";
            this.colDefectId.ReadOnly = true;
            this.colDefectId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDefectId.Width = 64;
            //
            // colGap
            //
            this.colGap.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colGap.HeaderText = "Gap(µm)";
            this.colGap.Name = "colGap";
            this.colGap.ReadOnly = true;
            this.colGap.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colGap.Width = 70;
            //
            // colRoughBin
            //
            this.colRoughBin.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colRoughBin.HeaderText = "RoughBin";
            this.colRoughBin.Name = "colRoughBin";
            this.colRoughBin.ReadOnly = true;
            this.colRoughBin.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colRoughBin.Width = 64;
            //
            // colClassNo
            //
            this.colClassNo.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colClassNo.HeaderText = "Class";
            this.colClassNo.Name = "colClassNo";
            this.colClassNo.ReadOnly = true;
            this.colClassNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colClassNo.Width = 44;
            //
            // colSize
            //
            this.colSize.HeaderText = "크기(µm)";
            this.colSize.Name = "colSize";
            this.colSize.ReadOnly = true;
            this.colSize.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colSize.Width = 92;
            //
            // colGdsX
            //
            this.colGdsX.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colGdsX.HeaderText = "GDS X";
            this.colGdsX.Name = "colGdsX";
            this.colGdsX.ReadOnly = true;
            this.colGdsX.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colGdsX.Width = 78;
            //
            // colGdsY
            //
            this.colGdsY.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colGdsY.HeaderText = "GDS Y";
            this.colGdsY.Name = "colGdsY";
            this.colGdsY.ReadOnly = true;
            this.colGdsY.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colGdsY.Width = 78;
            //
            // colXRel
            //
            this.colXRel.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colXRel.HeaderText = "XREL";
            this.colXRel.Name = "colXRel";
            this.colXRel.ReadOnly = true;
            this.colXRel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colXRel.Width = 72;
            //
            // colYRel
            //
            this.colYRel.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colYRel.HeaderText = "YREL";
            this.colYRel.Name = "colYRel";
            this.colYRel.ReadOnly = true;
            this.colYRel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colYRel.Width = 72;
            //
            // colNear
            //
            this.colNear.HeaderText = "가까운 Chain";
            this.colNear.Name = "colNear";
            this.colNear.ReadOnly = true;
            this.colNear.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colNear.Width = 220;
            //
            // pnlChainHead
            //
            this.pnlChainHead.Controls.Add(this.chkHitOnly);
            this.pnlChainHead.Controls.Add(this.chkOutside);
            this.pnlChainHead.Controls.Add(this.lblChainTitle);
            this.pnlChainHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlChainHead.Location = new System.Drawing.Point(0, 72);
            this.pnlChainHead.Name = "pnlChainHead";
            this.pnlChainHead.Size = new System.Drawing.Size(692, 28);
            this.pnlChainHead.TabIndex = 1;
            //
            // lblChainTitle
            //
            this.lblChainTitle.AutoSize = true;
            this.lblChainTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblChainTitle.Location = new System.Drawing.Point(280, 7);
            this.lblChainTitle.Name = "lblChainTitle";
            this.lblChainTitle.Size = new System.Drawing.Size(40, 15);
            this.lblChainTitle.TabIndex = 2;
            this.lblChainTitle.Text = "Chain";
            //
            // chkHitOnly
            //
            this.chkHitOnly.AutoSize = true;
            this.chkHitOnly.Location = new System.Drawing.Point(6, 6);
            this.chkHitOnly.Name = "chkHitOnly";
            this.chkHitOnly.Size = new System.Drawing.Size(140, 16);
            this.chkHitOnly.TabIndex = 0;
            this.chkHitOnly.Text = "걸린 / Fail Chain 만";
            this.toolTip.SetToolTip(this.chkHitOnly, "Defect 가 걸린 Chain 과 E-Test Fail Chain 만 표와 도면에 표시");
            this.chkHitOnly.UseVisualStyleBackColor = true;
            this.chkHitOnly.CheckedChanged += new System.EventHandler(this.chkHitOnly_CheckedChanged);
            //
            // chkOutside
            //
            this.chkOutside.AutoSize = true;
            this.chkOutside.Checked = true;
            this.chkOutside.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOutside.Location = new System.Drawing.Point(150, 6);
            this.chkOutside.Name = "chkOutside";
            this.chkOutside.Size = new System.Drawing.Size(110, 16);
            this.chkOutside.TabIndex = 1;
            this.chkOutside.Text = "Chain 밖 Defect";
            this.toolTip.SetToolTip(this.chkOutside, "어느 Chain 에도 걸리지 않은 Defect 를 표 맨 아래에 가까운 거리 순으로 표시");
            this.chkOutside.UseVisualStyleBackColor = true;
            this.chkOutside.CheckedChanged += new System.EventHandler(this.chkOutside_CheckedChanged);
            //
            // pnlGdsCriteria
            //
            this.pnlGdsCriteria.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(247)))), ((int)(((byte)(252)))));
            this.pnlGdsCriteria.Controls.Add(this.lblGdsCriteria);
            this.pnlGdsCriteria.Controls.Add(this.lblGdsText);
            this.pnlGdsCriteria.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlGdsCriteria.Location = new System.Drawing.Point(0, 0);
            this.pnlGdsCriteria.Name = "pnlGdsCriteria";
            this.pnlGdsCriteria.Size = new System.Drawing.Size(692, 72);
            this.pnlGdsCriteria.TabIndex = 0;
            //
            // lblGdsCriteria
            //
            this.lblGdsCriteria.AutoSize = true;
            this.lblGdsCriteria.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblGdsCriteria.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(87)))), ((int)(((byte)(154)))));
            this.lblGdsCriteria.Location = new System.Drawing.Point(6, 4);
            this.lblGdsCriteria.Name = "lblGdsCriteria";
            this.lblGdsCriteria.Size = new System.Drawing.Size(104, 15);
            this.lblGdsCriteria.TabIndex = 0;
            this.lblGdsCriteria.Text = "GDS / KLARF 기준";
            //
            // lblGdsText
            //
            this.lblGdsText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGdsText.Location = new System.Drawing.Point(8, 24);
            this.lblGdsText.Name = "lblGdsText";
            this.lblGdsText.Size = new System.Drawing.Size(680, 44);
            this.lblGdsText.TabIndex = 1;
            this.lblGdsText.Text = "-";
            //
            // tabEt
            //
            this.tabEt.Controls.Add(this.dgvEtValues);
            this.tabEt.Controls.Add(this.pnlEtHead);
            this.tabEt.Controls.Add(this.pnlEtCriteria);
            this.tabEt.Location = new System.Drawing.Point(4, 34);
            this.tabEt.Name = "tabEt";
            this.tabEt.Size = new System.Drawing.Size(692, 770);
            this.tabEt.TabIndex = 2;
            this.tabEt.Text = "E-Test";
            this.tabEt.UseVisualStyleBackColor = true;
            //
            // dgvEtValues
            //
            this.dgvEtValues.AllowUserToAddRows = false;
            this.dgvEtValues.AllowUserToDeleteRows = false;
            this.dgvEtValues.AllowUserToResizeRows = false;
            this.dgvEtValues.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvEtValues.BackgroundColor = System.Drawing.Color.White;
            this.dgvEtValues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEtValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEtValues.Location = new System.Drawing.Point(0, 82);
            this.dgvEtValues.MultiSelect = false;
            this.dgvEtValues.Name = "dgvEtValues";
            this.dgvEtValues.ReadOnly = true;
            this.dgvEtValues.RowHeadersVisible = false;
            this.dgvEtValues.RowTemplate.Height = 23;
            this.dgvEtValues.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEtValues.Size = new System.Drawing.Size(692, 688);
            this.dgvEtValues.TabIndex = 2;
            this.dgvEtValues.SelectionChanged += new System.EventHandler(this.dgvEtValues_SelectionChanged);
            //
            // pnlEtHead
            //
            this.pnlEtHead.Controls.Add(this.lblEtCount);
            this.pnlEtHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEtHead.Location = new System.Drawing.Point(0, 54);
            this.pnlEtHead.Name = "pnlEtHead";
            this.pnlEtHead.Size = new System.Drawing.Size(692, 28);
            this.pnlEtHead.TabIndex = 1;
            //
            // lblEtCount
            //
            this.lblEtCount.AutoSize = true;
            this.lblEtCount.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblEtCount.Location = new System.Drawing.Point(4, 7);
            this.lblEtCount.Name = "lblEtCount";
            this.lblEtCount.Size = new System.Drawing.Size(60, 15);
            this.lblEtCount.TabIndex = 0;
            this.lblEtCount.Text = "E-Test 값";
            //
            // pnlEtCriteria
            //
            this.pnlEtCriteria.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(247)))), ((int)(((byte)(240)))));
            this.pnlEtCriteria.Controls.Add(this.lblEtCriteria);
            this.pnlEtCriteria.Controls.Add(this.lblEtText);
            this.pnlEtCriteria.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEtCriteria.Location = new System.Drawing.Point(0, 0);
            this.pnlEtCriteria.Name = "pnlEtCriteria";
            this.pnlEtCriteria.Size = new System.Drawing.Size(692, 64);
            this.pnlEtCriteria.TabIndex = 0;
            //
            // lblEtCriteria
            //
            this.lblEtCriteria.AutoSize = true;
            this.lblEtCriteria.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblEtCriteria.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(95)))), ((int)(((byte)(0)))));
            this.lblEtCriteria.Location = new System.Drawing.Point(6, 4);
            this.lblEtCriteria.Name = "lblEtCriteria";
            this.lblEtCriteria.Size = new System.Drawing.Size(70, 15);
            this.lblEtCriteria.TabIndex = 0;
            this.lblEtCriteria.Text = "E-Test 기준";
            //
            // lblEtText
            //
            this.lblEtText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEtText.Location = new System.Drawing.Point(8, 28);
            this.lblEtText.Name = "lblEtText";
            this.lblEtText.Size = new System.Drawing.Size(680, 32);
            this.lblEtText.TabIndex = 1;
            this.lblEtText.Text = "-";
            //
            // tabDie
            //
            this.tabDie.Controls.Add(this.dgvSummary);
            this.tabDie.Controls.Add(this.pnlSummaryHead);
            this.tabDie.Controls.Add(this.pnlDieSummary);
            this.tabDie.Location = new System.Drawing.Point(4, 34);
            this.tabDie.Name = "tabDie";
            this.tabDie.Size = new System.Drawing.Size(692, 770);
            this.tabDie.TabIndex = 3;
            this.tabDie.Text = "종합 (Die)";
            this.tabDie.UseVisualStyleBackColor = true;
            //
            // dgvSummary
            //
            this.dgvSummary.AllowUserToAddRows = false;
            this.dgvSummary.AllowUserToDeleteRows = false;
            this.dgvSummary.AllowUserToResizeRows = false;
            this.dgvSummary.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvSummary.BackgroundColor = System.Drawing.Color.White;
            this.dgvSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSummary.Location = new System.Drawing.Point(0, 104);
            this.dgvSummary.MultiSelect = false;
            this.dgvSummary.Name = "dgvSummary";
            this.dgvSummary.ReadOnly = true;
            this.dgvSummary.RowHeadersVisible = false;
            this.dgvSummary.RowTemplate.Height = 23;
            this.dgvSummary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSummary.Size = new System.Drawing.Size(692, 666);
            this.dgvSummary.TabIndex = 2;
            this.dgvSummary.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSummary_CellDoubleClick);
            //
            // pnlSummaryHead
            //
            this.pnlSummaryHead.Controls.Add(this.lblSummaryTitle);
            this.pnlSummaryHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSummaryHead.Location = new System.Drawing.Point(0, 76);
            this.pnlSummaryHead.Name = "pnlSummaryHead";
            this.pnlSummaryHead.Size = new System.Drawing.Size(692, 28);
            this.pnlSummaryHead.TabIndex = 1;
            //
            // lblSummaryTitle
            //
            this.lblSummaryTitle.AutoSize = true;
            this.lblSummaryTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblSummaryTitle.Location = new System.Drawing.Point(4, 7);
            this.lblSummaryTitle.Name = "lblSummaryTitle";
            this.lblSummaryTitle.Size = new System.Drawing.Size(80, 15);
            this.lblSummaryTitle.TabIndex = 0;
            this.lblSummaryTitle.Text = "Chain 4분류";
            //
            // pnlDieSummary
            //
            this.pnlDieSummary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(244)))), ((int)(((byte)(252)))));
            this.pnlDieSummary.Controls.Add(this.lblDieCriteria);
            this.pnlDieSummary.Controls.Add(this.lblDieText);
            this.pnlDieSummary.Controls.Add(this.flowClass);
            this.pnlDieSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDieSummary.Location = new System.Drawing.Point(0, 0);
            this.pnlDieSummary.Name = "pnlDieSummary";
            this.pnlDieSummary.Size = new System.Drawing.Size(692, 84);
            this.pnlDieSummary.TabIndex = 0;
            //
            // lblDieCriteria
            //
            this.lblDieCriteria.AutoSize = true;
            this.lblDieCriteria.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblDieCriteria.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(58)))), ((int)(((byte)(145)))));
            this.lblDieCriteria.Location = new System.Drawing.Point(6, 4);
            this.lblDieCriteria.Name = "lblDieCriteria";
            this.lblDieCriteria.Size = new System.Drawing.Size(70, 15);
            this.lblDieCriteria.TabIndex = 0;
            this.lblDieCriteria.Text = "Die 요약";
            //
            // lblDieText
            //
            this.lblDieText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDieText.Location = new System.Drawing.Point(8, 28);
            this.lblDieText.Name = "lblDieText";
            this.lblDieText.Size = new System.Drawing.Size(680, 18);
            this.lblDieText.TabIndex = 1;
            this.lblDieText.Text = "-";
            //
            // flowClass
            //
            this.flowClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowClass.Controls.Add(this.lblClassBoth);
            this.flowClass.Controls.Add(this.lblClassFail);
            this.flowClass.Controls.Add(this.lblClassDefect);
            this.flowClass.Controls.Add(this.lblClassOk);
            this.flowClass.Controls.Add(this.lblClassNa);
            this.flowClass.Location = new System.Drawing.Point(4, 50);
            this.flowClass.Margin = new System.Windows.Forms.Padding(0);
            this.flowClass.Name = "flowClass";
            this.flowClass.Size = new System.Drawing.Size(684, 28);
            this.flowClass.TabIndex = 2;
            this.flowClass.WrapContents = false;
            //
            // lblClassBoth
            //
            this.lblClassBoth.AutoSize = true;
            this.lblClassBoth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblClassBoth.Margin = new System.Windows.Forms.Padding(4, 3, 4, 0);
            this.lblClassBoth.Name = "lblClassBoth";
            this.lblClassBoth.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.lblClassBoth.Size = new System.Drawing.Size(60, 20);
            this.lblClassBoth.TabIndex = 0;
            this.lblClassBoth.Text = "일치 -";
            //
            // lblClassFail
            //
            this.lblClassFail.AutoSize = true;
            this.lblClassFail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblClassFail.Margin = new System.Windows.Forms.Padding(4, 3, 4, 0);
            this.lblClassFail.Name = "lblClassFail";
            this.lblClassFail.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.lblClassFail.Size = new System.Drawing.Size(60, 20);
            this.lblClassFail.TabIndex = 1;
            this.lblClassFail.Text = "Fail만 -";
            //
            // lblClassDefect
            //
            this.lblClassDefect.AutoSize = true;
            this.lblClassDefect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblClassDefect.Margin = new System.Windows.Forms.Padding(4, 3, 4, 0);
            this.lblClassDefect.Name = "lblClassDefect";
            this.lblClassDefect.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.lblClassDefect.Size = new System.Drawing.Size(60, 20);
            this.lblClassDefect.TabIndex = 2;
            this.lblClassDefect.Text = "Defect만 -";
            //
            // lblClassOk
            //
            this.lblClassOk.AutoSize = true;
            this.lblClassOk.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblClassOk.Margin = new System.Windows.Forms.Padding(4, 3, 4, 0);
            this.lblClassOk.Name = "lblClassOk";
            this.lblClassOk.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.lblClassOk.Size = new System.Drawing.Size(60, 20);
            this.lblClassOk.TabIndex = 3;
            this.lblClassOk.Text = "정상 -";
            //
            // lblClassNa
            //
            this.lblClassNa.AutoSize = true;
            this.lblClassNa.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblClassNa.Margin = new System.Windows.Forms.Padding(4, 3, 4, 0);
            this.lblClassNa.Name = "lblClassNa";
            this.lblClassNa.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.lblClassNa.Size = new System.Drawing.Size(60, 20);
            this.lblClassNa.TabIndex = 4;
            this.lblClassNa.Text = "비교 불가 -";
            //
            // statusStrip
            //
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressLoad,
            this.lblElapsed,
            this.lblStatus,
            this.lblCoordinate});
            this.statusStrip.Location = new System.Drawing.Point(0, 878);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.ShowItemToolTips = true;
            this.statusStrip.Size = new System.Drawing.Size(1400, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 2;
            //
            // progressLoad
            //
            this.progressLoad.Name = "progressLoad";
            this.progressLoad.Size = new System.Drawing.Size(160, 16);
            this.progressLoad.Visible = false;
            //
            // lblElapsed
            //
            this.lblElapsed.ForeColor = System.Drawing.Color.DimGray;
            this.lblElapsed.Name = "lblElapsed";
            this.lblElapsed.Size = new System.Drawing.Size(90, 17);
            this.lblElapsed.Visible = false;
            //
            // lblStatus
            //
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(1300, 17);
            this.lblStatus.Spring = true;
            this.lblStatus.Text = "대기";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblCoordinate
            //
            this.lblCoordinate.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblCoordinate.Name = "lblCoordinate";
            this.lblCoordinate.Size = new System.Drawing.Size(90, 17);
            this.lblCoordinate.Text = "X: - / Y: -";
            //
            // GdsDieAnalysisForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 900);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.statusStrip);
            this.Name = "GdsDieAnalysisForm";
            this.Text = "Die 상세 분석";
            this.flowCriteria.ResumeLayout(false);
            this.flowCriteria.PerformLayout();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlMapHead.ResumeLayout(false);
            this.pnlMapHead.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChainDefects)).EndInit();
            this.pnlChainHead.ResumeLayout(false);
            this.pnlChainHead.PerformLayout();
            this.pnlGdsCriteria.ResumeLayout(false);
            this.pnlGdsCriteria.PerformLayout();
            this.tabChainDefect.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEtValues)).EndInit();
            this.pnlEtHead.ResumeLayout(false);
            this.pnlEtHead.PerformLayout();
            this.pnlEtCriteria.ResumeLayout(false);
            this.pnlEtCriteria.PerformLayout();
            this.tabEt.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSummary)).EndInit();
            this.pnlSummaryHead.ResumeLayout(false);
            this.pnlSummaryHead.PerformLayout();
            this.flowClass.ResumeLayout(false);
            this.flowClass.PerformLayout();
            this.pnlDieSummary.ResumeLayout(false);
            this.pnlDieSummary.PerformLayout();
            this.tabDie.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.FlowLayoutPanel flowCriteria;
        private System.Windows.Forms.Label lblWaferInfo;
        private System.Windows.Forms.Label lblSumTitle;
        private System.Windows.Forms.Label lblSumGds;
        private System.Windows.Forms.Label lblSumKlarf;
        private System.Windows.Forms.Label lblSumEt;
        private System.Windows.Forms.Label lblDieInfo;
        private System.Windows.Forms.Label lblFocus;
        private System.Windows.Forms.Button btnClearFocus;
        private System.Windows.Forms.SplitContainer splitMain;
        private NexplantQMS.GdsMap.GdsMapControl map;
        private System.Windows.Forms.Panel pnlMapHead;
        private System.Windows.Forms.Label lblMapTitle;
        private System.Windows.Forms.Label lblMapSide;
        private System.Windows.Forms.CheckBox chkBackground;
        private System.Windows.Forms.CheckBox chkOriginCross;
        private System.Windows.Forms.Button btnFit;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabChainDefect;
        private System.Windows.Forms.DataGridView dgvChainDefects;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colShow;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFailPara;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMember;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDefectId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGap;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRoughBin;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClassNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGdsX;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGdsY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colXRel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colYRel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNear;
        private System.Windows.Forms.Panel pnlChainHead;
        private System.Windows.Forms.Label lblChainTitle;
        private System.Windows.Forms.CheckBox chkHitOnly;
        private System.Windows.Forms.CheckBox chkOutside;
        private System.Windows.Forms.Panel pnlGdsCriteria;
        private System.Windows.Forms.Label lblGdsCriteria;
        private System.Windows.Forms.Label lblGdsText;
        private System.Windows.Forms.TabPage tabEt;
        private System.Windows.Forms.DataGridView dgvEtValues;
        private System.Windows.Forms.Panel pnlEtHead;
        private System.Windows.Forms.Label lblEtCount;
        private System.Windows.Forms.Panel pnlEtCriteria;
        private System.Windows.Forms.Label lblEtCriteria;
        private System.Windows.Forms.Label lblEtText;
        private System.Windows.Forms.TabPage tabDie;
        private System.Windows.Forms.DataGridView dgvSummary;
        private System.Windows.Forms.Panel pnlSummaryHead;
        private System.Windows.Forms.Label lblSummaryTitle;
        private System.Windows.Forms.Panel pnlDieSummary;
        private System.Windows.Forms.Label lblDieCriteria;
        private System.Windows.Forms.Label lblDieText;
        private System.Windows.Forms.FlowLayoutPanel flowClass;
        private System.Windows.Forms.Label lblClassBoth;
        private System.Windows.Forms.Label lblClassFail;
        private System.Windows.Forms.Label lblClassDefect;
        private System.Windows.Forms.Label lblClassOk;
        private System.Windows.Forms.Label lblClassNa;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripProgressBar progressLoad;
        private System.Windows.Forms.ToolStripStatusLabel lblElapsed;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblCoordinate;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
