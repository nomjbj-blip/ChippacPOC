namespace MenuFileEditor
{
    partial class MenuEditorForm
    {
        /// <summary>필수 디자이너 변수입니다.</summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>사용 중인 모든 리소스를 정리합니다.</summary>
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
        /// 화면 배치(위 -> 아래):
        ///  - pnlTop: 파일 경로 / [...] 열기 / [다시 읽기] / [저장] / [다른 이름으로 저장]
        ///  - splitMain 왼쪽: 찾기 줄(pnlSearch) + 메뉴 트리(treeMenu) + 위 / 아래 이동 버튼(pnlMove), 오른쪽: 속성 창(propertyGrid)
        ///  - pnlProblems: 검증 결과 목록(검증할 때만 보임)
        ///  - pnlBottom: [같은 수준 추가] [하위 추가] [복제] [삭제] [검증] / [닫기]
        ///  - statusStrip: 파일 경로 / 메뉴 수 / 저장 상태
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuEditorForm));
            this.components = new System.ComponentModel.Container();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblPath = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.treeMenu = new System.Windows.Forms.TreeView();
            this.ctxTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuAddSibling = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAddChild = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDuplicate = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuMoveUp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMoveDown = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlMove = new System.Windows.Forms.Panel();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.btnExpandAll = new System.Windows.Forms.Button();
            this.btnCollapseAll = new System.Windows.Forms.Button();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.pnlProblems = new System.Windows.Forms.Panel();
            this.lstProblems = new System.Windows.Forms.ListBox();
            this.pnlProblemsHead = new System.Windows.Forms.Panel();
            this.lblProblems = new System.Windows.Forms.Label();
            this.btnCloseProblems = new System.Windows.Forms.Button();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnAddSibling = new System.Windows.Forms.Button();
            this.btnAddChild = new System.Windows.Forms.Button();
            this.btnDuplicate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnValidate = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatusFile = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusDirty = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.ctxTree.SuspendLayout();
            this.pnlMove.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlProblems.SuspendLayout();
            this.pnlProblemsHead.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlTop
            //
            this.pnlTop.Controls.Add(this.lblPath);
            this.pnlTop.Controls.Add(this.txtPath);
            this.pnlTop.Controls.Add(this.btnBrowse);
            this.pnlTop.Controls.Add(this.btnReload);
            this.pnlTop.Controls.Add(this.btnSave);
            this.pnlTop.Controls.Add(this.btnSaveAs);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1184, 40);
            this.pnlTop.TabIndex = 0;
            //
            // lblPath
            //
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(10, 14);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(29, 12);
            this.lblPath.TabIndex = 0;
            this.lblPath.Text = "파일";
            //
            // txtPath
            //
            this.txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPath.Location = new System.Drawing.Point(46, 10);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(732, 21);
            this.txtPath.TabIndex = 1;
            this.txtPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPath_KeyDown);
            //
            // btnBrowse
            //
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(784, 6);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowse.Image")));
            this.btnBrowse.Size = new System.Drawing.Size(28, 28);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            //
            // btnReload
            //
            this.btnReload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReload.Location = new System.Drawing.Point(818, 6);
            this.btnReload.Name = "btnReload";
            this.btnReload.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnReload.Image = ((System.Drawing.Image)(resources.GetObject("btnReload.Image")));
            this.btnReload.Size = new System.Drawing.Size(96, 28);
            this.btnReload.TabIndex = 3;
            this.btnReload.Text = "다시 읽기";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            //
            // btnSave
            //
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(920, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Size = new System.Drawing.Size(88, 28);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // btnSaveAs
            //
            this.btnSaveAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAs.Location = new System.Drawing.Point(1014, 6);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveAs.Image")));
            this.btnSaveAs.Size = new System.Drawing.Size(160, 28);
            this.btnSaveAs.TabIndex = 5;
            this.btnSaveAs.Text = "다른 이름으로 저장...";
            this.btnSaveAs.UseVisualStyleBackColor = true;
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            //
            // splitMain
            //
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 40);
            this.splitMain.Name = "splitMain";
            //
            // splitMain.Panel1
            //
            this.splitMain.Panel1.Controls.Add(this.treeMenu);
            this.splitMain.Panel1.Controls.Add(this.pnlMove);
            this.splitMain.Panel1.Controls.Add(this.pnlSearch);
            //
            // splitMain.Panel2
            //
            this.splitMain.Panel2.Controls.Add(this.propertyGrid);
            this.splitMain.Size = new System.Drawing.Size(1184, 567);
            this.splitMain.SplitterDistance = 640;
            this.splitMain.Panel1MinSize = 420;
            this.splitMain.Panel2MinSize = 260;
            this.splitMain.TabIndex = 1;
            //
            // treeMenu
            //
            this.treeMenu.ContextMenuStrip = this.ctxTree;
            this.treeMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeMenu.HideSelection = false;
            this.treeMenu.Location = new System.Drawing.Point(0, 32);
            this.treeMenu.Name = "treeMenu";
            this.treeMenu.Size = new System.Drawing.Size(588, 535);
            this.treeMenu.TabIndex = 1;
            this.treeMenu.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeMenu_AfterSelect);
            this.treeMenu.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeMenu_NodeMouseClick);
            this.treeMenu.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeMenu_KeyDown);
            //
            // ctxTree
            //
            this.ctxTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAddSibling,
            this.mnuAddChild,
            this.mnuDuplicate,
            this.mnuSep1,
            this.mnuMoveUp,
            this.mnuMoveDown,
            this.mnuSep2,
            this.mnuDelete});
            this.ctxTree.Name = "ctxTree";
            this.ctxTree.Size = new System.Drawing.Size(181, 148);
            this.ctxTree.Opening += new System.ComponentModel.CancelEventHandler(this.ctxTree_Opening);
            //
            // mnuAddSibling
            //
            this.mnuAddSibling.Name = "mnuAddSibling";
            this.mnuAddSibling.Size = new System.Drawing.Size(180, 22);
            this.mnuAddSibling.Text = "같은 수준 추가";
            this.mnuAddSibling.Click += new System.EventHandler(this.btnAddSibling_Click);
            //
            // mnuAddChild
            //
            this.mnuAddChild.Name = "mnuAddChild";
            this.mnuAddChild.Size = new System.Drawing.Size(180, 22);
            this.mnuAddChild.Text = "하위 추가";
            this.mnuAddChild.Click += new System.EventHandler(this.btnAddChild_Click);
            //
            // mnuDuplicate
            //
            this.mnuDuplicate.Name = "mnuDuplicate";
            this.mnuDuplicate.Size = new System.Drawing.Size(180, 22);
            this.mnuDuplicate.Text = "복제";
            this.mnuDuplicate.Click += new System.EventHandler(this.btnDuplicate_Click);
            //
            // mnuSep1
            //
            this.mnuSep1.Name = "mnuSep1";
            this.mnuSep1.Size = new System.Drawing.Size(177, 6);
            //
            // mnuMoveUp
            //
            this.mnuMoveUp.Name = "mnuMoveUp";
            this.mnuMoveUp.ShortcutKeyDisplayString = "Ctrl+Up";
            this.mnuMoveUp.Size = new System.Drawing.Size(180, 22);
            this.mnuMoveUp.Text = "위로";
            this.mnuMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            //
            // mnuMoveDown
            //
            this.mnuMoveDown.Name = "mnuMoveDown";
            this.mnuMoveDown.ShortcutKeyDisplayString = "Ctrl+Down";
            this.mnuMoveDown.Size = new System.Drawing.Size(180, 22);
            this.mnuMoveDown.Text = "아래로";
            this.mnuMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            //
            // mnuSep2
            //
            this.mnuSep2.Name = "mnuSep2";
            this.mnuSep2.Size = new System.Drawing.Size(177, 6);
            //
            // mnuDelete
            //
            this.mnuDelete.Name = "mnuDelete";
            this.mnuDelete.ShortcutKeyDisplayString = "Del";
            this.mnuDelete.Size = new System.Drawing.Size(180, 22);
            this.mnuDelete.Text = "삭제";
            this.mnuDelete.Click += new System.EventHandler(this.btnDelete_Click);
            //
            // pnlMove
            //
            this.pnlMove.Controls.Add(this.btnMoveUp);
            this.pnlMove.Controls.Add(this.btnMoveDown);
            this.pnlMove.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlMove.Location = new System.Drawing.Point(588, 32);
            this.pnlMove.Name = "pnlMove";
            this.pnlMove.Size = new System.Drawing.Size(52, 535);
            this.pnlMove.TabIndex = 2;
            //
            // btnMoveUp
            //
            this.btnMoveUp.Location = new System.Drawing.Point(12, 8);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMoveUp.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveUp.Image")));
            this.btnMoveUp.Size = new System.Drawing.Size(28, 28);
            this.btnMoveUp.TabIndex = 0;
            this.btnMoveUp.Text = "";
            this.btnMoveUp.UseVisualStyleBackColor = true;
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            //
            // btnMoveDown
            //
            this.btnMoveDown.Location = new System.Drawing.Point(12, 42);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMoveDown.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveDown.Image")));
            this.btnMoveDown.Size = new System.Drawing.Size(28, 28);
            this.btnMoveDown.TabIndex = 1;
            this.btnMoveDown.Text = "";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            //
            // pnlSearch
            //
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.btnFind);
            this.pnlSearch.Controls.Add(this.btnExpandAll);
            this.pnlSearch.Controls.Add(this.btnCollapseAll);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(640, 40);
            this.pnlSearch.TabIndex = 0;
            //
            // txtSearch
            //
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(4, 10);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(322, 21);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            //
            // btnFind
            //
            this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFind.Location = new System.Drawing.Point(332, 6);
            this.btnFind.Name = "btnFind";
            this.btnFind.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFind.Image = ((System.Drawing.Image)(resources.GetObject("btnFind.Image")));
            this.btnFind.Size = new System.Drawing.Size(88, 28);
            this.btnFind.TabIndex = 1;
            this.btnFind.Text = "찾기";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            //
            // btnExpandAll
            //
            this.btnExpandAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExpandAll.Location = new System.Drawing.Point(426, 6);
            this.btnExpandAll.Name = "btnExpandAll";
            this.btnExpandAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnExpandAll.Image = ((System.Drawing.Image)(resources.GetObject("btnExpandAll.Image")));
            this.btnExpandAll.Size = new System.Drawing.Size(108, 28);
            this.btnExpandAll.TabIndex = 2;
            this.btnExpandAll.Text = "모두 펼치기";
            this.btnExpandAll.UseVisualStyleBackColor = true;
            this.btnExpandAll.Click += new System.EventHandler(this.btnExpandAll_Click);
            //
            // btnCollapseAll
            //
            this.btnCollapseAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCollapseAll.Location = new System.Drawing.Point(540, 6);
            this.btnCollapseAll.Name = "btnCollapseAll";
            this.btnCollapseAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCollapseAll.Image = ((System.Drawing.Image)(resources.GetObject("btnCollapseAll.Image")));
            this.btnCollapseAll.Size = new System.Drawing.Size(96, 28);
            this.btnCollapseAll.TabIndex = 3;
            this.btnCollapseAll.Text = "모두 접기";
            this.btnCollapseAll.UseVisualStyleBackColor = true;
            this.btnCollapseAll.Click += new System.EventHandler(this.btnCollapseAll_Click);
            //
            // propertyGrid
            //
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGrid.Size = new System.Drawing.Size(540, 567);
            this.propertyGrid.TabIndex = 0;
            this.propertyGrid.ToolbarVisible = false;
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            //
            // pnlProblems
            //
            this.pnlProblems.Controls.Add(this.lstProblems);
            this.pnlProblems.Controls.Add(this.pnlProblemsHead);
            this.pnlProblems.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlProblems.Location = new System.Drawing.Point(0, 607);
            this.pnlProblems.Name = "pnlProblems";
            this.pnlProblems.Size = new System.Drawing.Size(1184, 140);
            this.pnlProblems.TabIndex = 2;
            this.pnlProblems.Visible = false;
            //
            // lstProblems
            //
            this.lstProblems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstProblems.HorizontalScrollbar = true;
            this.lstProblems.IntegralHeight = false;
            this.lstProblems.ItemHeight = 12;
            this.lstProblems.Location = new System.Drawing.Point(0, 26);
            this.lstProblems.Name = "lstProblems";
            this.lstProblems.Size = new System.Drawing.Size(1184, 114);
            this.lstProblems.TabIndex = 1;
            this.lstProblems.DoubleClick += new System.EventHandler(this.lstProblems_DoubleClick);
            //
            // pnlProblemsHead
            //
            this.pnlProblemsHead.Controls.Add(this.lblProblems);
            this.pnlProblemsHead.Controls.Add(this.btnCloseProblems);
            this.pnlProblemsHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlProblemsHead.Location = new System.Drawing.Point(0, 0);
            this.pnlProblemsHead.Name = "pnlProblemsHead";
            this.pnlProblemsHead.Size = new System.Drawing.Size(1184, 34);
            this.pnlProblemsHead.TabIndex = 0;
            //
            // lblProblems
            //
            this.lblProblems.AutoSize = true;
            this.lblProblems.Location = new System.Drawing.Point(8, 11);
            this.lblProblems.Name = "lblProblems";
            this.lblProblems.Size = new System.Drawing.Size(57, 12);
            this.lblProblems.TabIndex = 0;
            this.lblProblems.Text = "검증 결과";
            //
            // btnCloseProblems
            //
            this.btnCloseProblems.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCloseProblems.Location = new System.Drawing.Point(1090, 3);
            this.btnCloseProblems.Name = "btnCloseProblems";
            this.btnCloseProblems.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCloseProblems.Image = ((System.Drawing.Image)(resources.GetObject("btnCloseProblems.Image")));
            this.btnCloseProblems.Size = new System.Drawing.Size(88, 28);
            this.btnCloseProblems.TabIndex = 1;
            this.btnCloseProblems.Text = "닫기";
            this.btnCloseProblems.UseVisualStyleBackColor = true;
            this.btnCloseProblems.Click += new System.EventHandler(this.btnCloseProblems_Click);
            //
            // pnlBottom
            //
            this.pnlBottom.Controls.Add(this.btnAddSibling);
            this.pnlBottom.Controls.Add(this.btnAddChild);
            this.pnlBottom.Controls.Add(this.btnDuplicate);
            this.pnlBottom.Controls.Add(this.btnDelete);
            this.pnlBottom.Controls.Add(this.btnValidate);
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 747);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1184, 44);
            this.pnlBottom.TabIndex = 3;
            //
            // btnAddSibling
            //
            this.btnAddSibling.Location = new System.Drawing.Point(10, 8);
            this.btnAddSibling.Name = "btnAddSibling";
            this.btnAddSibling.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddSibling.Image = ((System.Drawing.Image)(resources.GetObject("btnAddSibling.Image")));
            this.btnAddSibling.Size = new System.Drawing.Size(124, 28);
            this.btnAddSibling.TabIndex = 0;
            this.btnAddSibling.Text = "같은 수준 추가";
            this.btnAddSibling.UseVisualStyleBackColor = true;
            this.btnAddSibling.Click += new System.EventHandler(this.btnAddSibling_Click);
            //
            // btnAddChild
            //
            this.btnAddChild.Location = new System.Drawing.Point(140, 8);
            this.btnAddChild.Name = "btnAddChild";
            this.btnAddChild.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddChild.Image = ((System.Drawing.Image)(resources.GetObject("btnAddChild.Image")));
            this.btnAddChild.Size = new System.Drawing.Size(96, 28);
            this.btnAddChild.TabIndex = 1;
            this.btnAddChild.Text = "하위 추가";
            this.btnAddChild.UseVisualStyleBackColor = true;
            this.btnAddChild.Click += new System.EventHandler(this.btnAddChild_Click);
            //
            // btnDuplicate
            //
            this.btnDuplicate.Location = new System.Drawing.Point(242, 8);
            this.btnDuplicate.Name = "btnDuplicate";
            this.btnDuplicate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDuplicate.Image = ((System.Drawing.Image)(resources.GetObject("btnDuplicate.Image")));
            this.btnDuplicate.Size = new System.Drawing.Size(88, 28);
            this.btnDuplicate.TabIndex = 2;
            this.btnDuplicate.Text = "복제";
            this.btnDuplicate.UseVisualStyleBackColor = true;
            this.btnDuplicate.Click += new System.EventHandler(this.btnDuplicate_Click);
            //
            // btnDelete
            //
            this.btnDelete.Location = new System.Drawing.Point(336, 8);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.Size = new System.Drawing.Size(88, 28);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            //
            // btnValidate
            //
            this.btnValidate.Location = new System.Drawing.Point(440, 8);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnValidate.Image = ((System.Drawing.Image)(resources.GetObject("btnValidate.Image")));
            this.btnValidate.Size = new System.Drawing.Size(88, 28);
            this.btnValidate.TabIndex = 4;
            this.btnValidate.Text = "검증";
            this.btnValidate.UseVisualStyleBackColor = true;
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            //
            // btnClose
            //
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(1086, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Size = new System.Drawing.Size(88, 28);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            //
            // statusStrip
            //
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatusFile,
            this.lblStatusCount,
            this.lblStatusDirty});
            this.statusStrip.Location = new System.Drawing.Point(0, 791);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1184, 22);
            this.statusStrip.TabIndex = 4;
            //
            // lblStatusFile
            //
            this.lblStatusFile.Name = "lblStatusFile";
            this.lblStatusFile.Size = new System.Drawing.Size(1069, 17);
            this.lblStatusFile.Spring = true;
            this.lblStatusFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblStatusCount
            //
            this.lblStatusCount.Name = "lblStatusCount";
            this.lblStatusCount.Size = new System.Drawing.Size(50, 17);
            //
            // lblStatusDirty
            //
            this.lblStatusDirty.ForeColor = System.Drawing.Color.Firebrick;
            this.lblStatusDirty.Name = "lblStatusDirty";
            this.lblStatusDirty.Size = new System.Drawing.Size(50, 17);
            //
            // MenuEditorForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 813);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.pnlProblems);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.statusStrip);
            this.MinimumSize = new System.Drawing.Size(900, 500);
            this.Name = "MenuEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Menu File Editor";
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.ctxTree.ResumeLayout(false);
            this.pnlMove.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlProblems.ResumeLayout(false);
            this.pnlProblemsHead.ResumeLayout(false);
            this.pnlProblemsHead.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSaveAs;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.TreeView treeMenu;
        private System.Windows.Forms.ContextMenuStrip ctxTree;
        private System.Windows.Forms.ToolStripMenuItem mnuAddSibling;
        private System.Windows.Forms.ToolStripMenuItem mnuAddChild;
        private System.Windows.Forms.ToolStripMenuItem mnuDuplicate;
        private System.Windows.Forms.ToolStripSeparator mnuSep1;
        private System.Windows.Forms.ToolStripMenuItem mnuMoveUp;
        private System.Windows.Forms.ToolStripMenuItem mnuMoveDown;
        private System.Windows.Forms.ToolStripSeparator mnuSep2;
        private System.Windows.Forms.ToolStripMenuItem mnuDelete;
        private System.Windows.Forms.Panel pnlMove;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Button btnExpandAll;
        private System.Windows.Forms.Button btnCollapseAll;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Panel pnlProblems;
        private System.Windows.Forms.ListBox lstProblems;
        private System.Windows.Forms.Panel pnlProblemsHead;
        private System.Windows.Forms.Label lblProblems;
        private System.Windows.Forms.Button btnCloseProblems;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnAddSibling;
        private System.Windows.Forms.Button btnAddChild;
        private System.Windows.Forms.Button btnDuplicate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnValidate;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusFile;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusCount;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusDirty;
    }
}
