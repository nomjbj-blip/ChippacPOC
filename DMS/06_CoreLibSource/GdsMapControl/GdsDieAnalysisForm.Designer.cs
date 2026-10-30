namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// Die 상세 분석 화면 배치. 설계: 문서/설계/05_연관분석/2026-10-12_GDS_KLARF_연관분석_화면_설계.md 5.1절,
    ///       문서/설계/05_연관분석/2026-10-15_GDS_KLARF_ETest_통합분석_데이터_설계.md 8.2 / 15.3절 (2026-10-16 E-Test 값 / 역추적),
    ///       문서/설계/05_연관분석/2026-10-19_화면A_레이아웃_후보.md 10절 (2026-10-19 화면 A 와 같은 기능 탭)
    ///       문서/설계/05_연관분석/2026-10-20_상세보기_GDS_KLARF_합친탭_설계.md (2026-10-20 GDS / KLARF 탭 합침)
    ///       문서/설계/05_연관분석/2026-10-22_상세보기_오른쪽_한패널_설계.md (2026-10-22 오른쪽 탭 3개 -> 한 패널)
    /// </summary>
    partial class GdsDieAnalysisForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null) components.Dispose();
                if (_gridBoldFont != null) _gridBoldFont.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 위: 1줄 Wafer / 분석 기준 요약(GDS · KLARF · E-Test), 2줄 Die 정보 + 역추적 Chain + [역추적 해제]
        /// 가운데: 왼쪽 GDS 도면(머리 = 보기 면 / 전체 도면 / 원점, 도면 칸이 좁으면 다음 줄로) / 오른쪽 한 패널 (2026-10-22, 탭 3개 합침)
        ///   위부터 기준 묶음(GDS / KLARF 2줄 + E-Test 1줄) / 4분류 수 칸 / 머리 줄(체크 3개 + 수) /
        ///   가로 나누기 위 = GDS / KLARF 표(Chain 칸 + 그 Chain 에 걸린 Defect 칸, 맨 아래 Chain 밖 Defect 묶음), 아래 = 고른 Chain 의 E-Test 값 표
        /// 아래: 상태줄(진행 / 좌표)
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GdsDieAnalysisForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.flowCriteria = new System.Windows.Forms.FlowLayoutPanel();
            this.lblWaferInfo = new System.Windows.Forms.Label();
            this.lblSumTitle = new System.Windows.Forms.Label();
            this.lblSumGds = new System.Windows.Forms.Label();
            this.lblSumKlarf = new System.Windows.Forms.Label();
            this.lblSumEt = new System.Windows.Forms.Label();
            this.lblDieInfo = new System.Windows.Forms.Label();
            this.btnReanalyze = new System.Windows.Forms.Button();
            this.btnClearFocus = new System.Windows.Forms.Button();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.map = new NexplantQMS.GdsMap.GdsMapControl();
            this.pnlMapHead = new System.Windows.Forms.FlowLayoutPanel();
            this.lblMapTitle = new System.Windows.Forms.Label();
            this.lblMapSide = new System.Windows.Forms.Label();
            this.chkBackground = new System.Windows.Forms.CheckBox();
            this.chkOriginCross = new System.Windows.Forms.CheckBox();
            this.splitRight = new System.Windows.Forms.SplitContainer();
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
            this.dgvEtValues = new System.Windows.Forms.DataGridView();
            this.pnlEtHead = new System.Windows.Forms.Panel();
            this.lblEtCount = new System.Windows.Forms.Label();
            this.pnlChainHead = new System.Windows.Forms.Panel();
            this.chkHitOnly = new System.Windows.Forms.CheckBox();
            this.chkShowAll = new System.Windows.Forms.CheckBox();
            this.lblChainTitle = new System.Windows.Forms.Label();
            this.pnlGdsCriteria = new System.Windows.Forms.Panel();
            this.lblGdsCriteria = new System.Windows.Forms.Label();
            this.lblGdsText = new System.Windows.Forms.Label();
            this.lblEtText = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.progressLoad = new System.Windows.Forms.ToolStripProgressBar();
            this.lblElapsed = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCoordinate = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.mnuDefect = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuDefectImage = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlTop.SuspendLayout();
            this.flowCriteria.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.pnlMapHead.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitRight)).BeginInit();
            this.splitRight.Panel1.SuspendLayout();
            this.splitRight.Panel2.SuspendLayout();
            this.splitRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChainDefects)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEtValues)).BeginInit();
            this.pnlEtHead.SuspendLayout();
            this.pnlChainHead.SuspendLayout();
            this.pnlGdsCriteria.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.mnuDefect.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(242)))), ((int)(((byte)(250)))));
            this.pnlTop.Controls.Add(this.flowCriteria);
            this.pnlTop.Controls.Add(this.lblDieInfo);
            this.pnlTop.Controls.Add(this.btnReanalyze);
            this.pnlTop.Controls.Add(this.btnClearFocus);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(4);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(2000, 105);
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
            this.flowCriteria.Location = new System.Drawing.Point(6, 3);
            this.flowCriteria.Margin = new System.Windows.Forms.Padding(0);
            this.flowCriteria.Name = "flowCriteria";
            this.flowCriteria.Size = new System.Drawing.Size(1797, 48);
            this.flowCriteria.TabIndex = 0;
            this.flowCriteria.WrapContents = false;
            // 
            // lblWaferInfo
            // 
            this.lblWaferInfo.AutoSize = true;
            this.lblWaferInfo.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblWaferInfo.Location = new System.Drawing.Point(6, 12);
            this.lblWaferInfo.Margin = new System.Windows.Forms.Padding(6, 12, 23, 0);
            this.lblWaferInfo.Name = "lblWaferInfo";
            this.lblWaferInfo.Size = new System.Drawing.Size(0, 28);
            this.lblWaferInfo.TabIndex = 0;
            // 
            // lblSumTitle
            // 
            this.lblSumTitle.AutoSize = true;
            this.lblSumTitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblSumTitle.Location = new System.Drawing.Point(35, 15);
            this.lblSumTitle.Margin = new System.Windows.Forms.Padding(6, 15, 3, 0);
            this.lblSumTitle.Name = "lblSumTitle";
            this.lblSumTitle.Size = new System.Drawing.Size(86, 18);
            this.lblSumTitle.TabIndex = 1;
            this.lblSumTitle.Text = "분석 기준";
            // 
            // lblSumGds
            // 
            this.lblSumGds.AutoSize = true;
            this.lblSumGds.BackColor = System.Drawing.Color.White;
            this.lblSumGds.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSumGds.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(87)))), ((int)(((byte)(154)))));
            this.lblSumGds.Location = new System.Drawing.Point(130, 8);
            this.lblSumGds.Margin = new System.Windows.Forms.Padding(6, 8, 6, 0);
            this.lblSumGds.Name = "lblSumGds";
            this.lblSumGds.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.lblSumGds.Size = new System.Drawing.Size(84, 28);
            this.lblSumGds.TabIndex = 2;
            this.lblSumGds.Text = "GDS · -";
            // 
            // lblSumKlarf
            // 
            this.lblSumKlarf.AutoSize = true;
            this.lblSumKlarf.BackColor = System.Drawing.Color.White;
            this.lblSumKlarf.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSumKlarf.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.lblSumKlarf.Location = new System.Drawing.Point(226, 8);
            this.lblSumKlarf.Margin = new System.Windows.Forms.Padding(6, 8, 6, 0);
            this.lblSumKlarf.Name = "lblSumKlarf";
            this.lblSumKlarf.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.lblSumKlarf.Size = new System.Drawing.Size(102, 28);
            this.lblSumKlarf.TabIndex = 3;
            this.lblSumKlarf.Text = "KLARF · -";
            // 
            // lblSumEt
            // 
            this.lblSumEt.AutoSize = true;
            this.lblSumEt.BackColor = System.Drawing.Color.White;
            this.lblSumEt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSumEt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(95)))), ((int)(((byte)(0)))));
            this.lblSumEt.Location = new System.Drawing.Point(340, 8);
            this.lblSumEt.Margin = new System.Windows.Forms.Padding(6, 8, 6, 0);
            this.lblSumEt.Name = "lblSumEt";
            this.lblSumEt.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.lblSumEt.Size = new System.Drawing.Size(105, 28);
            this.lblSumEt.TabIndex = 4;
            this.lblSumEt.Text = "E-Test · -";
            // 
            // lblDieInfo
            // 
            this.lblDieInfo.AutoSize = true;
            this.lblDieInfo.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblDieInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(60)))), ((int)(((byte)(130)))));
            this.lblDieInfo.Location = new System.Drawing.Point(14, 62);
            this.lblDieInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDieInfo.Name = "lblDieInfo";
            this.lblDieInfo.Size = new System.Drawing.Size(108, 31);
            this.lblDieInfo.TabIndex = 1;
            this.lblDieInfo.Text = "Die (-, -)";
            // 
            // btnReanalyze
            // 
            this.btnReanalyze.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReanalyze.Image = ((System.Drawing.Image)(resources.GetObject("btnReanalyze.Image")));
            this.btnReanalyze.Location = new System.Drawing.Point(1814, 6);
            this.btnReanalyze.Margin = new System.Windows.Forms.Padding(4);
            this.btnReanalyze.Name = "btnReanalyze";
            this.btnReanalyze.Size = new System.Drawing.Size(171, 45);
            this.btnReanalyze.TabIndex = 4;
            this.btnReanalyze.Text = "다시 분석";
            this.btnReanalyze.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip.SetToolTip(this.btnReanalyze, "이 상세보기를 연 분석 화면의 지금 설정으로 다시 분석");
            this.btnReanalyze.UseVisualStyleBackColor = true;
            this.btnReanalyze.Click += new System.EventHandler(this.btnReanalyze_Click);
            // 
            // btnClearFocus
            // 
            this.btnClearFocus.Image = ((System.Drawing.Image)(resources.GetObject("btnClearFocus.Image")));
            this.btnClearFocus.Location = new System.Drawing.Point(1825, 55);
            this.btnClearFocus.Margin = new System.Windows.Forms.Padding(4);
            this.btnClearFocus.Name = "btnClearFocus";
            this.btnClearFocus.Size = new System.Drawing.Size(160, 42);
            this.btnClearFocus.TabIndex = 3;
            this.btnClearFocus.Text = "역추적 해제";
            this.btnClearFocus.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClearFocus.UseVisualStyleBackColor = true;
            this.btnClearFocus.Visible = false;
            this.btnClearFocus.Click += new System.EventHandler(this.btnClearFocus_Click);
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitMain.Location = new System.Drawing.Point(0, 105);
            this.splitMain.Margin = new System.Windows.Forms.Padding(4);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.map);
            this.splitMain.Panel1.Controls.Add(this.pnlMapHead);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.splitRight);
            this.splitMain.Panel2.Controls.Add(this.pnlChainHead);
            this.splitMain.Panel2.Controls.Add(this.pnlGdsCriteria);
            this.splitMain.Size = new System.Drawing.Size(2000, 1209);
            this.splitMain.SplitterDistance = 1290;
            this.splitMain.SplitterWidth = 6;
            this.splitMain.TabIndex = 1;
            // 
            // map
            // 
            this.map.BackColor = System.Drawing.Color.Black;
            this.map.ColorAlpha = 50;
            this.map.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.map.DefectStyle = NexplantQMS.GdsMap.DefectOverlayStyle.Standard;
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.Location = new System.Drawing.Point(0, 37);
            this.map.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.map.Mode = NexplantQMS.GdsMap.GdsMapControl.ViewMode.View;
            this.map.Name = "map";
            this.map.Size = new System.Drawing.Size(1290, 1172);
            this.map.TabIndex = 1;
            this.map.VSync = false;
            this.map.MouseWorldPositionChanged += new System.EventHandler<System.Drawing.PointF>(this.map_MouseWorldPositionChanged);
            this.map.MouseClick += new System.Windows.Forms.MouseEventHandler(this.map_MouseClick);
            // 
            // pnlMapHead
            // 
            this.pnlMapHead.AutoSize = true;
            this.pnlMapHead.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlMapHead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.pnlMapHead.Controls.Add(this.lblMapTitle);
            this.pnlMapHead.Controls.Add(this.lblMapSide);
            this.pnlMapHead.Controls.Add(this.chkBackground);
            this.pnlMapHead.Controls.Add(this.chkOriginCross);
            this.pnlMapHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMapHead.Location = new System.Drawing.Point(0, 0);
            this.pnlMapHead.Margin = new System.Windows.Forms.Padding(4);
            this.pnlMapHead.Name = "pnlMapHead";
            this.pnlMapHead.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pnlMapHead.Size = new System.Drawing.Size(1290, 37);
            this.pnlMapHead.TabIndex = 0;
            // 
            // lblMapTitle
            // 
            this.lblMapTitle.AutoSize = true;
            this.lblMapTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblMapTitle.Location = new System.Drawing.Point(8, 12);
            this.lblMapTitle.Margin = new System.Windows.Forms.Padding(4, 12, 4, 0);
            this.lblMapTitle.Name = "lblMapTitle";
            this.lblMapTitle.Size = new System.Drawing.Size(90, 25);
            this.lblMapTitle.TabIndex = 0;
            this.lblMapTitle.Text = "GDS 도면";
            // 
            // lblMapSide
            // 
            this.lblMapSide.AutoSize = true;
            this.lblMapSide.ForeColor = System.Drawing.Color.DimGray;
            this.lblMapSide.Location = new System.Drawing.Point(106, 15);
            this.lblMapSide.Margin = new System.Windows.Forms.Padding(4, 15, 17, 0);
            this.lblMapSide.Name = "lblMapSide";
            this.lblMapSide.Size = new System.Drawing.Size(74, 18);
            this.lblMapSide.TabIndex = 1;
            this.lblMapSide.Text = "보기 = -";
            // 
            // chkBackground
            // 
            this.chkBackground.AutoSize = true;
            this.chkBackground.Location = new System.Drawing.Point(201, 12);
            this.chkBackground.Margin = new System.Windows.Forms.Padding(4, 12, 4, 0);
            this.chkBackground.Name = "chkBackground";
            this.chkBackground.Size = new System.Drawing.Size(112, 22);
            this.chkBackground.TabIndex = 2;
            this.chkBackground.Text = "전체 도면";
            this.chkBackground.UseVisualStyleBackColor = true;
            this.chkBackground.CheckedChanged += new System.EventHandler(this.chkBackground_CheckedChanged);
            // 
            // chkOriginCross
            // 
            this.chkOriginCross.AutoSize = true;
            this.chkOriginCross.Checked = true;
            this.chkOriginCross.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOriginCross.Location = new System.Drawing.Point(321, 12);
            this.chkOriginCross.Margin = new System.Windows.Forms.Padding(4, 12, 4, 0);
            this.chkOriginCross.Name = "chkOriginCross";
            this.chkOriginCross.Size = new System.Drawing.Size(112, 22);
            this.chkOriginCross.TabIndex = 3;
            this.chkOriginCross.Text = "원점 표시";
            this.chkOriginCross.UseVisualStyleBackColor = true;
            this.chkOriginCross.CheckedChanged += new System.EventHandler(this.chkOriginCross_CheckedChanged);
            // 
            // splitRight
            // 
            this.splitRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitRight.Location = new System.Drawing.Point(0, 177);
            this.splitRight.Margin = new System.Windows.Forms.Padding(4);
            this.splitRight.Name = "splitRight";
            this.splitRight.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitRight.Panel1
            // 
            this.splitRight.Panel1.Controls.Add(this.dgvChainDefects);
            this.splitRight.Panel1MinSize = 80;
            // 
            // splitRight.Panel2
            // 
            this.splitRight.Panel2.Controls.Add(this.dgvEtValues);
            this.splitRight.Panel2.Controls.Add(this.pnlEtHead);
            this.splitRight.Panel2MinSize = 80;
            this.splitRight.Size = new System.Drawing.Size(704, 1032);
            this.splitRight.SplitterDistance = 677;
            this.splitRight.SplitterWidth = 6;
            this.splitRight.TabIndex = 3;
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
            this.colRoughBin,
            this.colClassNo,
            this.colSize,
            this.colGdsX,
            this.colGdsY,
            this.colXRel,
            this.colYRel});
            this.dgvChainDefects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvChainDefects.Location = new System.Drawing.Point(0, 0);
            this.dgvChainDefects.Margin = new System.Windows.Forms.Padding(4);
            this.dgvChainDefects.MultiSelect = false;
            this.dgvChainDefects.Name = "dgvChainDefects";
            this.dgvChainDefects.RowHeadersVisible = false;
            this.dgvChainDefects.RowHeadersWidth = 62;
            this.dgvChainDefects.RowTemplate.Height = 23;
            this.dgvChainDefects.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvChainDefects.Size = new System.Drawing.Size(704, 677);
            this.dgvChainDefects.TabIndex = 2;
            this.dgvChainDefects.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChainDefects_CellContentClick);
            this.dgvChainDefects.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvChainDefects_CellMouseClick);
            this.dgvChainDefects.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvChainDefects_CellPainting);
            this.dgvChainDefects.SelectionChanged += new System.EventHandler(this.dgvChainDefects_SelectionChanged);
            // 
            // colShow
            // 
            this.colShow.HeaderText = "표시";
            this.colShow.MinimumWidth = 8;
            this.colShow.Name = "colShow";
            this.colShow.Width = 40;
            // 
            // colColor
            // 
            this.colColor.HeaderText = "색";
            this.colColor.MinimumWidth = 8;
            this.colColor.Name = "colColor";
            this.colColor.ReadOnly = true;
            this.colColor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colColor.Width = 30;
            // 
            // colCode
            // 
            this.colCode.HeaderText = "Chain";
            this.colCode.MinimumWidth = 8;
            this.colCode.Name = "colCode";
            this.colCode.ReadOnly = true;
            this.colCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCode.Width = 140;
            // 
            // colCount
            // 
            this.colCount.HeaderText = "Defect 수";
            this.colCount.MinimumWidth = 8;
            this.colCount.Name = "colCount";
            this.colCount.ReadOnly = true;
            this.colCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCount.Width = 68;
            // 
            // colClass
            // 
            this.colClass.HeaderText = "분류";
            this.colClass.MinimumWidth = 8;
            this.colClass.Name = "colClass";
            this.colClass.ReadOnly = true;
            this.colClass.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colClass.Width = 64;
            // 
            // colEt
            // 
            this.colEt.HeaderText = "ET";
            this.colEt.MinimumWidth = 8;
            this.colEt.Name = "colEt";
            this.colEt.ReadOnly = true;
            this.colEt.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colEt.Width = 34;
            // 
            // colFailPara
            // 
            this.colFailPara.HeaderText = "Fail Para";
            this.colFailPara.MinimumWidth = 8;
            this.colFailPara.Name = "colFailPara";
            this.colFailPara.ReadOnly = true;
            this.colFailPara.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colFailPara.Width = 64;
            // 
            // colMember
            // 
            this.colMember.HeaderText = "Member";
            this.colMember.MinimumWidth = 8;
            this.colMember.Name = "colMember";
            this.colMember.ReadOnly = true;
            this.colMember.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colMember.Visible = false;
            this.colMember.Width = 56;
            // 
            // colDefectId
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colDefectId.DefaultCellStyle = dataGridViewCellStyle9;
            this.colDefectId.HeaderText = "Defect ID";
            this.colDefectId.MinimumWidth = 8;
            this.colDefectId.Name = "colDefectId";
            this.colDefectId.ReadOnly = true;
            this.colDefectId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDefectId.Width = 64;
            // 
            // colGap
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colGap.DefaultCellStyle = dataGridViewCellStyle10;
            this.colGap.HeaderText = "Gap";
            this.colGap.MinimumWidth = 8;
            this.colGap.Name = "colGap";
            this.colGap.ReadOnly = true;
            this.colGap.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colGap.Visible = false;
            this.colGap.Width = 70;
            // 
            // colRoughBin
            // 
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colRoughBin.DefaultCellStyle = dataGridViewCellStyle11;
            this.colRoughBin.HeaderText = "RoughBin";
            this.colRoughBin.MinimumWidth = 8;
            this.colRoughBin.Name = "colRoughBin";
            this.colRoughBin.ReadOnly = true;
            this.colRoughBin.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colRoughBin.Width = 64;
            // 
            // colClassNo
            // 
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colClassNo.DefaultCellStyle = dataGridViewCellStyle12;
            this.colClassNo.HeaderText = "Class";
            this.colClassNo.MinimumWidth = 8;
            this.colClassNo.Name = "colClassNo";
            this.colClassNo.ReadOnly = true;
            this.colClassNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colClassNo.Width = 44;
            // 
            // colSize
            // 
            this.colSize.HeaderText = "크기";
            this.colSize.MinimumWidth = 8;
            this.colSize.Name = "colSize";
            this.colSize.ReadOnly = true;
            this.colSize.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colSize.Width = 92;
            // 
            // colGdsX
            // 
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colGdsX.DefaultCellStyle = dataGridViewCellStyle13;
            this.colGdsX.HeaderText = "GDS X";
            this.colGdsX.MinimumWidth = 8;
            this.colGdsX.Name = "colGdsX";
            this.colGdsX.ReadOnly = true;
            this.colGdsX.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colGdsX.Visible = false;
            this.colGdsX.Width = 78;
            // 
            // colGdsY
            // 
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colGdsY.DefaultCellStyle = dataGridViewCellStyle14;
            this.colGdsY.HeaderText = "GDS Y";
            this.colGdsY.MinimumWidth = 8;
            this.colGdsY.Name = "colGdsY";
            this.colGdsY.ReadOnly = true;
            this.colGdsY.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colGdsY.Visible = false;
            this.colGdsY.Width = 78;
            // 
            // colXRel
            // 
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colXRel.DefaultCellStyle = dataGridViewCellStyle15;
            this.colXRel.HeaderText = "XREL";
            this.colXRel.MinimumWidth = 8;
            this.colXRel.Name = "colXRel";
            this.colXRel.ReadOnly = true;
            this.colXRel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colXRel.Width = 72;
            // 
            // colYRel
            // 
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colYRel.DefaultCellStyle = dataGridViewCellStyle16;
            this.colYRel.HeaderText = "YREL";
            this.colYRel.MinimumWidth = 8;
            this.colYRel.Name = "colYRel";
            this.colYRel.ReadOnly = true;
            this.colYRel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colYRel.Width = 72;
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
            this.dgvEtValues.Location = new System.Drawing.Point(0, 42);
            this.dgvEtValues.Margin = new System.Windows.Forms.Padding(4);
            this.dgvEtValues.MultiSelect = false;
            this.dgvEtValues.Name = "dgvEtValues";
            this.dgvEtValues.ReadOnly = true;
            this.dgvEtValues.RowHeadersVisible = false;
            this.dgvEtValues.RowHeadersWidth = 62;
            this.dgvEtValues.RowTemplate.Height = 23;
            this.dgvEtValues.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEtValues.Size = new System.Drawing.Size(704, 307);
            this.dgvEtValues.TabIndex = 2;
            this.dgvEtValues.SelectionChanged += new System.EventHandler(this.dgvEtValues_SelectionChanged);
            // 
            // pnlEtHead
            // 
            this.pnlEtHead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(247)))), ((int)(((byte)(240)))));
            this.pnlEtHead.Controls.Add(this.lblEtCount);
            this.pnlEtHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEtHead.Location = new System.Drawing.Point(0, 0);
            this.pnlEtHead.Margin = new System.Windows.Forms.Padding(4);
            this.pnlEtHead.Name = "pnlEtHead";
            this.pnlEtHead.Size = new System.Drawing.Size(704, 42);
            this.pnlEtHead.TabIndex = 1;
            // 
            // lblEtCount
            // 
            this.lblEtCount.AutoSize = true;
            this.lblEtCount.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblEtCount.Location = new System.Drawing.Point(6, 10);
            this.lblEtCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEtCount.Name = "lblEtCount";
            this.lblEtCount.Size = new System.Drawing.Size(88, 25);
            this.lblEtCount.TabIndex = 0;
            this.lblEtCount.Text = "E-Test 값";
            // 
            // pnlChainHead
            // 
            this.pnlChainHead.Controls.Add(this.chkHitOnly);
            this.pnlChainHead.Controls.Add(this.chkShowAll);
            this.pnlChainHead.Controls.Add(this.lblChainTitle);
            this.pnlChainHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlChainHead.Location = new System.Drawing.Point(0, 132);
            this.pnlChainHead.Margin = new System.Windows.Forms.Padding(4);
            this.pnlChainHead.Name = "pnlChainHead";
            this.pnlChainHead.Size = new System.Drawing.Size(704, 45);
            this.pnlChainHead.TabIndex = 2;
            // 
            // chkHitOnly
            // 
            this.chkHitOnly.AutoSize = true;
            this.chkHitOnly.Checked = true;
            this.chkHitOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHitOnly.Location = new System.Drawing.Point(9, 9);
            this.chkHitOnly.Margin = new System.Windows.Forms.Padding(4);
            this.chkHitOnly.Name = "chkHitOnly";
            this.chkHitOnly.Size = new System.Drawing.Size(190, 22);
            this.chkHitOnly.TabIndex = 0;
            this.chkHitOnly.Text = "걸린 / Fail Chain 만";
            this.toolTip.SetToolTip(this.chkHitOnly, "Defect 가 걸린 Chain 과 E-Test Fail Chain 만 표와 도면에 표시");
            this.chkHitOnly.UseVisualStyleBackColor = true;
            this.chkHitOnly.CheckedChanged += new System.EventHandler(this.chkHitOnly_CheckedChanged);
            // 
            // chkShowAll
            // 
            this.chkShowAll.AutoSize = true;
            this.chkShowAll.Checked = true;
            this.chkShowAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowAll.Location = new System.Drawing.Point(214, 9);
            this.chkShowAll.Margin = new System.Windows.Forms.Padding(4);
            this.chkShowAll.Name = "chkShowAll";
            this.chkShowAll.Size = new System.Drawing.Size(107, 22);
            this.chkShowAll.TabIndex = 2;
            this.chkShowAll.Text = "전체 표시";
            this.toolTip.SetToolTip(this.chkShowAll, "모든 Chain 의 표시 체크를 한 번에 켜거나 끔");
            this.chkShowAll.UseVisualStyleBackColor = true;
            this.chkShowAll.CheckedChanged += new System.EventHandler(this.chkShowAll_CheckedChanged);
            // 
            // lblChainTitle
            // 
            this.lblChainTitle.AutoSize = true;
            this.lblChainTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblChainTitle.Location = new System.Drawing.Point(503, 10);
            this.lblChainTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblChainTitle.Name = "lblChainTitle";
            this.lblChainTitle.Size = new System.Drawing.Size(60, 25);
            this.lblChainTitle.TabIndex = 2;
            this.lblChainTitle.Text = "Chain";
            this.lblChainTitle.Visible = false;
            // 
            // pnlGdsCriteria
            // 
            this.pnlGdsCriteria.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(247)))), ((int)(((byte)(252)))));
            this.pnlGdsCriteria.Controls.Add(this.lblGdsCriteria);
            this.pnlGdsCriteria.Controls.Add(this.lblGdsText);
            this.pnlGdsCriteria.Controls.Add(this.lblEtText);
            this.pnlGdsCriteria.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlGdsCriteria.Location = new System.Drawing.Point(0, 0);
            this.pnlGdsCriteria.Margin = new System.Windows.Forms.Padding(4);
            this.pnlGdsCriteria.Name = "pnlGdsCriteria";
            this.pnlGdsCriteria.Size = new System.Drawing.Size(704, 132);
            this.pnlGdsCriteria.TabIndex = 0;
            // 
            // lblGdsCriteria
            // 
            this.lblGdsCriteria.AutoSize = true;
            this.lblGdsCriteria.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblGdsCriteria.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(87)))), ((int)(((byte)(154)))));
            this.lblGdsCriteria.Location = new System.Drawing.Point(9, 6);
            this.lblGdsCriteria.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGdsCriteria.Name = "lblGdsCriteria";
            this.lblGdsCriteria.Size = new System.Drawing.Size(165, 25);
            this.lblGdsCriteria.TabIndex = 0;
            this.lblGdsCriteria.Text = "GDS / KLARF 기준";
            // 
            // lblGdsText
            // 
            this.lblGdsText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGdsText.Location = new System.Drawing.Point(11, 36);
            this.lblGdsText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGdsText.Name = "lblGdsText";
            this.lblGdsText.Size = new System.Drawing.Size(687, 60);
            this.lblGdsText.TabIndex = 1;
            this.lblGdsText.Text = "-";
            // 
            // lblEtText
            // 
            this.lblEtText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEtText.AutoEllipsis = true;
            this.lblEtText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(95)))), ((int)(((byte)(0)))));
            this.lblEtText.Location = new System.Drawing.Point(11, 99);
            this.lblEtText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEtText.Name = "lblEtText";
            this.lblEtText.Size = new System.Drawing.Size(687, 27);
            this.lblEtText.TabIndex = 2;
            this.lblEtText.Text = "-";
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressLoad,
            this.lblElapsed,
            this.lblStatus,
            this.lblCoordinate});
            this.statusStrip.Location = new System.Drawing.Point(0, 1314);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 20, 0);
            this.statusStrip.ShowItemToolTips = true;
            this.statusStrip.Size = new System.Drawing.Size(2000, 36);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 2;
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
            this.lblStatus.Size = new System.Drawing.Size(1889, 29);
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
            // mnuDefect
            // 
            this.mnuDefect.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.mnuDefect.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDefectImage});
            this.mnuDefect.Name = "mnuDefect";
            this.mnuDefect.Size = new System.Drawing.Size(241, 36);
            // 
            // mnuDefectImage
            // 
            this.mnuDefectImage.Name = "mnuDefectImage";
            this.mnuDefectImage.Size = new System.Drawing.Size(240, 32);
            this.mnuDefectImage.Text = "Defect 이미지 보기";
            this.mnuDefectImage.Click += new System.EventHandler(this.mnuDefectImage_Click);
            // 
            // GdsDieAnalysisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2000, 1350);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.statusStrip);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GdsDieAnalysisForm";
            this.Text = "Die 상세 분석";
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.flowCriteria.ResumeLayout(false);
            this.flowCriteria.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel1.PerformLayout();
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.pnlMapHead.ResumeLayout(false);
            this.pnlMapHead.PerformLayout();
            this.splitRight.Panel1.ResumeLayout(false);
            this.splitRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitRight)).EndInit();
            this.splitRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChainDefects)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEtValues)).EndInit();
            this.pnlEtHead.ResumeLayout(false);
            this.pnlEtHead.PerformLayout();
            this.pnlChainHead.ResumeLayout(false);
            this.pnlChainHead.PerformLayout();
            this.pnlGdsCriteria.ResumeLayout(false);
            this.pnlGdsCriteria.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.mnuDefect.ResumeLayout(false);
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
        private System.Windows.Forms.Button btnClearFocus;
        private System.Windows.Forms.Button btnReanalyze;
        private System.Windows.Forms.SplitContainer splitMain;
        private NexplantQMS.GdsMap.GdsMapControl map;
        private System.Windows.Forms.FlowLayoutPanel pnlMapHead;
        private System.Windows.Forms.Label lblMapTitle;
        private System.Windows.Forms.Label lblMapSide;
        private System.Windows.Forms.CheckBox chkBackground;
        private System.Windows.Forms.CheckBox chkOriginCross;
        private System.Windows.Forms.SplitContainer splitRight;
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
        private System.Windows.Forms.Panel pnlChainHead;
        private System.Windows.Forms.Label lblChainTitle;
        private System.Windows.Forms.CheckBox chkHitOnly;
        private System.Windows.Forms.CheckBox chkShowAll;
        private System.Windows.Forms.Panel pnlGdsCriteria;
        private System.Windows.Forms.Label lblGdsCriteria;
        private System.Windows.Forms.Label lblGdsText;
        private System.Windows.Forms.Label lblEtText;
        private System.Windows.Forms.DataGridView dgvEtValues;
        private System.Windows.Forms.Panel pnlEtHead;
        private System.Windows.Forms.Label lblEtCount;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripProgressBar progressLoad;
        private System.Windows.Forms.ToolStripStatusLabel lblElapsed;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblCoordinate;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip mnuDefect;
        private System.Windows.Forms.ToolStripMenuItem mnuDefectImage;
    }
}
