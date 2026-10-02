namespace NexplantQMS.GdsMap
{
    partial class GdsMapForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
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
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chainCellSizeLabel = new System.Windows.Forms.Label();
            this._numChainCellSize = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.lblDbFactory = new System.Windows.Forms.Label();
            this.txtDbFactory = new System.Windows.Forms.TextBox();
            this.lblDbDevice = new System.Windows.Forms.Label();
            this.txtDbDevice = new System.Windows.Forms.TextBox();
            this.btnDbSave = new System.Windows.Forms.Button();
            this.btnDbStop = new System.Windows.Forms.Button();
            this.dataGrid = new System.Windows.Forms.DataGridView();
            this.btnLayer = new System.Windows.Forms.Button();
            this.btnLayerCheckNone = new System.Windows.Forms.Button();
            this.btnLayerCheckAll = new System.Windows.Forms.Button();
            this.statusStripMap = new System.Windows.Forms.StatusStrip();
            this.progressMapLoad = new System.Windows.Forms.ToolStripProgressBar();
            this.lblMapStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblMapCoordinate = new System.Windows.Forms.ToolStripStatusLabel();
            this.GDSContainer = new System.Windows.Forms.SplitContainer();
            this.chkLayerItems = new NexplantQMS.GdsMap.LayerColorCheckedListBox();
            this.chainSimilarListPanel = new System.Windows.Forms.Panel();
            this._chainSimilarList = new System.Windows.Forms.CheckedListBox();
            this.chainSimilarListTitle = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chainListPanel = new System.Windows.Forms.Panel();
            this._chainList = new System.Windows.Forms.CheckedListBox();
            this.chainListButtons = new System.Windows.Forms.FlowLayoutPanel();
            this._btnChainNew = new System.Windows.Forms.Button();
            this._btnChainRename = new System.Windows.Forms.Button();
            this._btnChainDelete = new System.Windows.Forms.Button();
            this.chainListTitle = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
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
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numChainCellSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.statusStripMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GDSContainer)).BeginInit();
            this.GDSContainer.Panel1.SuspendLayout();
            this.GDSContainer.Panel2.SuspendLayout();
            this.GDSContainer.SuspendLayout();
            this.chainSimilarListPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.chainListPanel.SuspendLayout();
            this.chainListButtons.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.chainSimilarRow.SuspendLayout();
            this.chainFirstRow.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chainCellSizeLabel);
            this.panel1.Controls.Add(this._numChainCellSize);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.lblDbFactory);
            this.panel1.Controls.Add(this.txtDbFactory);
            this.panel1.Controls.Add(this.lblDbDevice);
            this.panel1.Controls.Add(this.txtDbDevice);
            this.panel1.Controls.Add(this.btnDbSave);
            this.panel1.Controls.Add(this.btnDbStop);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1478, 59);
            this.panel1.TabIndex = 2;
            // 
            // chainCellSizeLabel
            // 
            this.chainCellSizeLabel.AutoSize = true;
            this.chainCellSizeLabel.Location = new System.Drawing.Point(450, 24);
            this.chainCellSizeLabel.Margin = new System.Windows.Forms.Padding(8, 8, 3, 0);
            this.chainCellSizeLabel.Name = "chainCellSizeLabel";
            this.chainCellSizeLabel.Size = new System.Drawing.Size(86, 18);
            this.chainCellSizeLabel.TabIndex = 0;
            this.chainCellSizeLabel.Text = "격자 크기";
            // 
            // _numChainCellSize
            // 
            this._numChainCellSize.DecimalPlaces = 3;
            this._numChainCellSize.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this._numChainCellSize.Location = new System.Drawing.Point(542, 19);
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
            this._numChainCellSize.Size = new System.Drawing.Size(92, 28);
            this._numChainCellSize.TabIndex = 1;
            this._numChainCellSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(18, 13);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(144, 34);
            this.button1.TabIndex = 2;
            this.button1.Text = "Load from File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblDbFactory
            // 
            this.lblDbFactory.AutoSize = true;
            this.lblDbFactory.Location = new System.Drawing.Point(660, 24);
            this.lblDbFactory.Name = "lblDbFactory";
            this.lblDbFactory.Size = new System.Drawing.Size(69, 18);
            this.lblDbFactory.TabIndex = 10;
            this.lblDbFactory.Text = "Factory";
            // 
            // txtDbFactory
            // 
            this.txtDbFactory.Location = new System.Drawing.Point(744, 19);
            this.txtDbFactory.MaxLength = 30;
            this.txtDbFactory.Name = "txtDbFactory";
            this.txtDbFactory.Size = new System.Drawing.Size(80, 28);
            this.txtDbFactory.TabIndex = 11;
            this.txtDbFactory.Text = "CHIPPAC";
            // 
            // lblDbDevice
            // 
            this.lblDbDevice.AutoSize = true;
            this.lblDbDevice.Location = new System.Drawing.Point(832, 24);
            this.lblDbDevice.Name = "lblDbDevice";
            this.lblDbDevice.Size = new System.Drawing.Size(62, 18);
            this.lblDbDevice.TabIndex = 12;
            this.lblDbDevice.Text = "Device";
            // 
            // txtDbDevice
            // 
            this.txtDbDevice.Location = new System.Drawing.Point(894, 19);
            this.txtDbDevice.MaxLength = 50;
            this.txtDbDevice.Name = "txtDbDevice";
            this.txtDbDevice.Size = new System.Drawing.Size(120, 28);
            this.txtDbDevice.TabIndex = 13;
            this.txtDbDevice.Text = "TEST_DEVICE";
            // 
            // btnDbSave
            // 
            this.btnDbSave.Location = new System.Drawing.Point(1024, 13);
            this.btnDbSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnDbSave.Name = "btnDbSave";
            this.btnDbSave.Size = new System.Drawing.Size(104, 34);
            this.btnDbSave.TabIndex = 16;
            this.btnDbSave.Text = "DB 저장";
            this.btnDbSave.UseVisualStyleBackColor = true;
            this.btnDbSave.Click += new System.EventHandler(this.btnDbSave_Click);
            // 
            // btnDbStop
            // 
            this.btnDbStop.Location = new System.Drawing.Point(1134, 13);
            this.btnDbStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnDbStop.Name = "btnDbStop";
            this.btnDbStop.Size = new System.Drawing.Size(104, 34);
            this.btnDbStop.TabIndex = 17;
            this.btnDbStop.Text = "저장 중지";
            this.btnDbStop.UseVisualStyleBackColor = true;
            this.btnDbStop.Click += new System.EventHandler(this.btnDbStop_Click);
            // 
            // dataGrid
            // 
            this.dataGrid.AllowUserToAddRows = false;
            this.dataGrid.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid.Location = new System.Drawing.Point(3, 3);
            this.dataGrid.Margin = new System.Windows.Forms.Padding(4);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGrid.RowHeadersWidth = 62;
            this.dataGrid.RowTemplate.Height = 23;
            this.dataGrid.Size = new System.Drawing.Size(1238, 1635);
            this.dataGrid.TabIndex = 3;
            // 
            // btnLayer
            // 
            this.btnLayer.Enabled = false;
            this.btnLayer.Location = new System.Drawing.Point(7, 77);
            this.btnLayer.Margin = new System.Windows.Forms.Padding(4);
            this.btnLayer.Name = "btnLayer";
            this.btnLayer.Size = new System.Drawing.Size(204, 27);
            this.btnLayer.TabIndex = 1;
            this.btnLayer.Text = "Redraw";
            this.btnLayer.UseVisualStyleBackColor = true;
            this.btnLayer.Click += new System.EventHandler(this.btnLayer_Click);
            // 
            // btnLayerCheckNone
            // 
            this.btnLayerCheckNone.Enabled = false;
            this.btnLayerCheckNone.Location = new System.Drawing.Point(7, 42);
            this.btnLayerCheckNone.Margin = new System.Windows.Forms.Padding(4);
            this.btnLayerCheckNone.Name = "btnLayerCheckNone";
            this.btnLayerCheckNone.Size = new System.Drawing.Size(204, 27);
            this.btnLayerCheckNone.TabIndex = 3;
            this.btnLayerCheckNone.Text = "전체 미체크";
            this.btnLayerCheckNone.UseVisualStyleBackColor = true;
            this.btnLayerCheckNone.Click += new System.EventHandler(this.btnLayerCheckNone_Click);
            // 
            // btnLayerCheckAll
            // 
            this.btnLayerCheckAll.Enabled = false;
            this.btnLayerCheckAll.Location = new System.Drawing.Point(7, 7);
            this.btnLayerCheckAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnLayerCheckAll.Name = "btnLayerCheckAll";
            this.btnLayerCheckAll.Size = new System.Drawing.Size(204, 27);
            this.btnLayerCheckAll.TabIndex = 2;
            this.btnLayerCheckAll.Text = "전체 체크";
            this.btnLayerCheckAll.UseVisualStyleBackColor = true;
            this.btnLayerCheckAll.Click += new System.EventHandler(this.btnLayerCheckAll_Click);
            // 
            // statusStripMap
            // 
            this.statusStripMap.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStripMap.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressMapLoad,
            this.lblMapStatus,
            this.lblMapCoordinate});
            this.statusStripMap.Location = new System.Drawing.Point(0, 1673);
            this.statusStripMap.Name = "statusStripMap";
            this.statusStripMap.Size = new System.Drawing.Size(1252, 36);
            this.statusStripMap.SizingGrip = false;
            this.statusStripMap.TabIndex = 0;
            // 
            // progressMapLoad
            // 
            this.progressMapLoad.Name = "progressMapLoad";
            this.progressMapLoad.Size = new System.Drawing.Size(160, 28);
            this.progressMapLoad.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressMapLoad.Visible = false;
            // 
            // lblMapStatus
            // 
            this.lblMapStatus.Name = "lblMapStatus";
            this.lblMapStatus.Size = new System.Drawing.Size(1125, 29);
            this.lblMapStatus.Spring = true;
            this.lblMapStatus.Text = "대기";
            this.lblMapStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMapCoordinate
            // 
            this.lblMapCoordinate.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblMapCoordinate.Name = "lblMapCoordinate";
            this.lblMapCoordinate.Size = new System.Drawing.Size(112, 29);
            this.lblMapCoordinate.Text = "X: — / Y: —";
            // 
            // GDSContainer
            // 
            this.GDSContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GDSContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GDSContainer.Location = new System.Drawing.Point(0, 59);
            this.GDSContainer.Name = "GDSContainer";
            // 
            // GDSContainer.Panel1
            // 
            this.GDSContainer.Panel1.Controls.Add(this.chkLayerItems);
            this.GDSContainer.Panel1.Controls.Add(this.chainSimilarListPanel);
            this.GDSContainer.Panel1.Controls.Add(this.panel2);
            this.GDSContainer.Panel1.Controls.Add(this.chainListPanel);
            this.GDSContainer.Panel1MinSize = 220;
            // 
            // GDSContainer.Panel2
            // 
            this.GDSContainer.Panel2.Controls.Add(this.tabControl1);
            this.GDSContainer.Panel2.Controls.Add(this.statusStripMap);
            this.GDSContainer.Panel2MinSize = 400;
            this.GDSContainer.Size = new System.Drawing.Size(1478, 1711);
            this.GDSContainer.SplitterDistance = 220;
            this.GDSContainer.TabIndex = 5;
            // 
            // chkLayerItems
            // 
            this.chkLayerItems.CheckOnClick = true;
            this.chkLayerItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkLayerItems.FormattingEnabled = true;
            this.chkLayerItems.Location = new System.Drawing.Point(0, 619);
            this.chkLayerItems.Name = "chkLayerItems";
            this.chkLayerItems.Size = new System.Drawing.Size(218, 1090);
            this.chkLayerItems.TabIndex = 4;
            this.chkLayerItems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ChkLayerItems_ItemCheck);
            // 
            // chainSimilarListPanel
            // 
            this.chainSimilarListPanel.Controls.Add(this._chainSimilarList);
            this.chainSimilarListPanel.Controls.Add(this.chainSimilarListTitle);
            this.chainSimilarListPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.chainSimilarListPanel.Location = new System.Drawing.Point(0, 379);
            this.chainSimilarListPanel.Name = "chainSimilarListPanel";
            this.chainSimilarListPanel.Padding = new System.Windows.Forms.Padding(4);
            this.chainSimilarListPanel.Size = new System.Drawing.Size(218, 240);
            this.chainSimilarListPanel.TabIndex = 7;
            // 
            // _chainSimilarList
            // 
            this._chainSimilarList.CheckOnClick = true;
            this._chainSimilarList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._chainSimilarList.FormattingEnabled = true;
            this._chainSimilarList.HorizontalScrollbar = true;
            this._chainSimilarList.Location = new System.Drawing.Point(4, 29);
            this._chainSimilarList.Name = "_chainSimilarList";
            this._chainSimilarList.Size = new System.Drawing.Size(210, 207);
            this._chainSimilarList.TabIndex = 1;
            this._chainSimilarList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ChainSimilarList_ItemCheck);
            this._chainSimilarList.SelectedIndexChanged += new System.EventHandler(this.ChainSimilarList_SelectedIndexChanged);
            // 
            // chainSimilarListTitle
            // 
            this.chainSimilarListTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.chainSimilarListTitle.Location = new System.Drawing.Point(4, 4);
            this.chainSimilarListTitle.Name = "chainSimilarListTitle";
            this.chainSimilarListTitle.Size = new System.Drawing.Size(210, 25);
            this.chainSimilarListTitle.TabIndex = 0;
            this.chainSimilarListTitle.Text = "유사 묶음 (체크: 제외 / 행: 위치)";
            this.chainSimilarListTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnLayerCheckAll);
            this.panel2.Controls.Add(this.btnLayer);
            this.panel2.Controls.Add(this.btnLayerCheckNone);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 260);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(218, 119);
            this.panel2.TabIndex = 0;
            // 
            // chainListPanel
            // 
            this.chainListPanel.Controls.Add(this._chainList);
            this.chainListPanel.Controls.Add(this.chainListButtons);
            this.chainListPanel.Controls.Add(this.chainListTitle);
            this.chainListPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.chainListPanel.Location = new System.Drawing.Point(0, 0);
            this.chainListPanel.Name = "chainListPanel";
            this.chainListPanel.Padding = new System.Windows.Forms.Padding(4);
            this.chainListPanel.Size = new System.Drawing.Size(218, 260);
            this.chainListPanel.TabIndex = 6;
            // 
            // _chainList
            // 
            this._chainList.CheckOnClick = true;
            this._chainList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._chainList.HorizontalScrollbar = true;
            this._chainList.Location = new System.Drawing.Point(4, 133);
            this._chainList.Name = "_chainList";
            this._chainList.Size = new System.Drawing.Size(210, 123);
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
            this.chainListButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.chainListButtons.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.chainListButtons.Location = new System.Drawing.Point(4, 29);
            this.chainListButtons.Name = "chainListButtons";
            this.chainListButtons.Size = new System.Drawing.Size(210, 104);
            this.chainListButtons.TabIndex = 3;
            this.chainListButtons.WrapContents = false;
            // 
            // _btnChainNew
            // 
            this._btnChainNew.Location = new System.Drawing.Point(3, 3);
            this._btnChainNew.Name = "_btnChainNew";
            this._btnChainNew.Size = new System.Drawing.Size(204, 28);
            this._btnChainNew.TabIndex = 0;
            this._btnChainNew.Text = "새 Chain";
            this._btnChainNew.Click += new System.EventHandler(this.BtnChainNew_Click);
            // 
            // _btnChainRename
            // 
            this._btnChainRename.Location = new System.Drawing.Point(3, 37);
            this._btnChainRename.Name = "_btnChainRename";
            this._btnChainRename.Size = new System.Drawing.Size(204, 28);
            this._btnChainRename.TabIndex = 1;
            this._btnChainRename.Text = "이름 수정";
            this._btnChainRename.Click += new System.EventHandler(this.BtnChainRename_Click);
            // 
            // _btnChainDelete
            // 
            this._btnChainDelete.Location = new System.Drawing.Point(3, 71);
            this._btnChainDelete.Name = "_btnChainDelete";
            this._btnChainDelete.Size = new System.Drawing.Size(204, 28);
            this._btnChainDelete.TabIndex = 2;
            this._btnChainDelete.Text = "삭제";
            this._btnChainDelete.Click += new System.EventHandler(this.BtnChainDelete_Click);
            // 
            // chainListTitle
            // 
            this.chainListTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.chainListTitle.Location = new System.Drawing.Point(4, 4);
            this.chainListTitle.Name = "chainListTitle";
            this.chainListTitle.Size = new System.Drawing.Size(210, 25);
            this.chainListTitle.TabIndex = 4;
            this.chainListTitle.Text = "Chain 목록 (체크: 표시 / 행: 편집)";
            this.chainListTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1252, 1673);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1244, 1641);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Map";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._lblChainSetupStatus);
            this.splitContainer1.Panel1.Controls.Add(this.chainSimilarRow);
            this.splitContainer1.Panel1.Controls.Add(this.chainFirstRow);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.map);
            this.splitContainer1.Size = new System.Drawing.Size(1238, 1635);
            this.splitContainer1.SplitterDistance = 168;
            this.splitContainer1.TabIndex = 5;
            // 
            // _lblChainSetupStatus
            // 
            this._lblChainSetupStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lblChainSetupStatus.Location = new System.Drawing.Point(0, 84);
            this._lblChainSetupStatus.Name = "_lblChainSetupStatus";
            this._lblChainSetupStatus.Size = new System.Drawing.Size(1236, 82);
            this._lblChainSetupStatus.TabIndex = 3;
            this._lblChainSetupStatus.Text = "GDS 조회 후 Input부터 지정하세요.";
            // 
            // chainSimilarRow
            // 
            this.chainSimilarRow.Controls.Add(this._btnChainExample);
            this.chainSimilarRow.Controls.Add(this._btnChainFindSimilar);
            this.chainSimilarRow.Controls.Add(this._btnChainClearSimilar);
            this.chainSimilarRow.Controls.Add(this._btnChainApplySimilar);
            this.chainSimilarRow.Controls.Add(this._btnChainUndoSimilar);
            this.chainSimilarRow.Dock = System.Windows.Forms.DockStyle.Top;
            this.chainSimilarRow.Location = new System.Drawing.Point(0, 42);
            this.chainSimilarRow.Name = "chainSimilarRow";
            this.chainSimilarRow.Size = new System.Drawing.Size(1236, 42);
            this.chainSimilarRow.TabIndex = 2;
            this.chainSimilarRow.WrapContents = false;
            // 
            // _btnChainExample
            // 
            this._btnChainExample.Location = new System.Drawing.Point(3, 3);
            this._btnChainExample.Name = "_btnChainExample";
            this._btnChainExample.Size = new System.Drawing.Size(150, 28);
            this._btnChainExample.TabIndex = 0;
            this._btnChainExample.Text = "예시 묶음 지정";
            this._btnChainExample.Click += new System.EventHandler(this.BtnChainExample_Click);
            // 
            // _btnChainFindSimilar
            // 
            this._btnChainFindSimilar.Location = new System.Drawing.Point(159, 3);
            this._btnChainFindSimilar.Name = "_btnChainFindSimilar";
            this._btnChainFindSimilar.Size = new System.Drawing.Size(150, 28);
            this._btnChainFindSimilar.TabIndex = 1;
            this._btnChainFindSimilar.Text = "유사 묶음 찾기";
            this._btnChainFindSimilar.Click += new System.EventHandler(this.BtnChainFindSimilar_Click);
            // 
            // _btnChainClearSimilar
            // 
            this._btnChainClearSimilar.Location = new System.Drawing.Point(315, 3);
            this._btnChainClearSimilar.Name = "_btnChainClearSimilar";
            this._btnChainClearSimilar.Size = new System.Drawing.Size(150, 28);
            this._btnChainClearSimilar.TabIndex = 2;
            this._btnChainClearSimilar.Text = "미리보기 취소";
            this._btnChainClearSimilar.Click += new System.EventHandler(this.BtnChainClearSimilar_Click);
            // 
            // _btnChainApplySimilar
            // 
            this._btnChainApplySimilar.Location = new System.Drawing.Point(471, 3);
            this._btnChainApplySimilar.Name = "_btnChainApplySimilar";
            this._btnChainApplySimilar.Size = new System.Drawing.Size(140, 28);
            this._btnChainApplySimilar.TabIndex = 3;
            this._btnChainApplySimilar.Text = "체크 묶음 제외";
            this._btnChainApplySimilar.Click += new System.EventHandler(this.BtnChainApplySimilar_Click);
            // 
            // _btnChainUndoSimilar
            // 
            this._btnChainUndoSimilar.Location = new System.Drawing.Point(617, 3);
            this._btnChainUndoSimilar.Name = "_btnChainUndoSimilar";
            this._btnChainUndoSimilar.Size = new System.Drawing.Size(180, 28);
            this._btnChainUndoSimilar.TabIndex = 4;
            this._btnChainUndoSimilar.Text = "직전 제외 되돌리기";
            this._btnChainUndoSimilar.Click += new System.EventHandler(this.BtnChainUndoSimilar_Click);
            // 
            // chainFirstRow
            // 
            this.chainFirstRow.AutoScroll = true;
            this.chainFirstRow.Controls.Add(this._btnChainInput);
            this.chainFirstRow.Controls.Add(this._btnChainOutput);
            this.chainFirstRow.Controls.Add(this._btnChainApply);
            this.chainFirstRow.Controls.Add(this._btnChainCancel);
            this.chainFirstRow.Controls.Add(this._btnChainTrace);
            this.chainFirstRow.Controls.Add(this._btnChainAdd);
            this.chainFirstRow.Controls.Add(this._btnChainRemove);
            this.chainFirstRow.Dock = System.Windows.Forms.DockStyle.Top;
            this.chainFirstRow.Location = new System.Drawing.Point(0, 0);
            this.chainFirstRow.Name = "chainFirstRow";
            this.chainFirstRow.Size = new System.Drawing.Size(1236, 42);
            this.chainFirstRow.TabIndex = 0;
            this.chainFirstRow.WrapContents = false;
            // 
            // _btnChainInput
            // 
            this._btnChainInput.Location = new System.Drawing.Point(3, 3);
            this._btnChainInput.Name = "_btnChainInput";
            this._btnChainInput.Size = new System.Drawing.Size(168, 28);
            this._btnChainInput.TabIndex = 0;
            this._btnChainInput.Text = "Input 지정";
            this._btnChainInput.Click += new System.EventHandler(this.BtnChainInput_Click);
            // 
            // _btnChainOutput
            // 
            this._btnChainOutput.Location = new System.Drawing.Point(177, 3);
            this._btnChainOutput.Name = "_btnChainOutput";
            this._btnChainOutput.Size = new System.Drawing.Size(169, 28);
            this._btnChainOutput.TabIndex = 1;
            this._btnChainOutput.Text = "Output 지정";
            this._btnChainOutput.Click += new System.EventHandler(this.BtnChainOutput_Click);
            // 
            // _btnChainApply
            // 
            this._btnChainApply.Location = new System.Drawing.Point(352, 3);
            this._btnChainApply.Name = "_btnChainApply";
            this._btnChainApply.Size = new System.Drawing.Size(126, 28);
            this._btnChainApply.TabIndex = 2;
            this._btnChainApply.Text = "선택 적용";
            this._btnChainApply.Click += new System.EventHandler(this.BtnChainApply_Click);
            // 
            // _btnChainCancel
            // 
            this._btnChainCancel.Location = new System.Drawing.Point(484, 3);
            this._btnChainCancel.Name = "_btnChainCancel";
            this._btnChainCancel.Size = new System.Drawing.Size(154, 28);
            this._btnChainCancel.TabIndex = 3;
            this._btnChainCancel.Text = "선택 취소";
            this._btnChainCancel.Click += new System.EventHandler(this.BtnChainCancel_Click);
            // 
            // _btnChainTrace
            // 
            this._btnChainTrace.Location = new System.Drawing.Point(644, 3);
            this._btnChainTrace.Name = "_btnChainTrace";
            this._btnChainTrace.Size = new System.Drawing.Size(164, 28);
            this._btnChainTrace.TabIndex = 4;
            this._btnChainTrace.Text = "후보 경로 찾기";
            this._btnChainTrace.Click += new System.EventHandler(this.BtnChainTrace_Click);
            // 
            // _btnChainAdd
            // 
            this._btnChainAdd.Location = new System.Drawing.Point(814, 3);
            this._btnChainAdd.Name = "_btnChainAdd";
            this._btnChainAdd.Size = new System.Drawing.Size(162, 28);
            this._btnChainAdd.TabIndex = 5;
            this._btnChainAdd.Text = "경로 추가";
            this._btnChainAdd.Click += new System.EventHandler(this.BtnChainAdd_Click);
            // 
            // _btnChainRemove
            // 
            this._btnChainRemove.Location = new System.Drawing.Point(982, 3);
            this._btnChainRemove.Name = "_btnChainRemove";
            this._btnChainRemove.Size = new System.Drawing.Size(144, 28);
            this._btnChainRemove.TabIndex = 6;
            this._btnChainRemove.Text = "경로 제외";
            this._btnChainRemove.Click += new System.EventHandler(this.BtnChainRemove_Click);
            // 
            // map
            // 
            this.map.BackColor = System.Drawing.Color.Black;
            this.map.ColorAlpha = 50;
            this.map.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.Location = new System.Drawing.Point(0, 0);
            this.map.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.map.Mode = NexplantQMS.GdsMap.GdsMapControl.ViewMode.View;
            this.map.Name = "map";
            this.map.Size = new System.Drawing.Size(1236, 1461);
            this.map.TabIndex = 4;
            this.map.VSync = false;
            this.map.MouseWorldPositionChanged += new System.EventHandler<System.Drawing.PointF>(this.Map_MouseWorldPositionChanged);
            this.map.RenderProgressChanged += new System.EventHandler<NexplantQMS.GdsMap.GdsMapRenderProgressChangedEventArgs>(this.Map_RenderProgressChanged);
            this.map.FirstFrameMeasured += new System.EventHandler<NexplantQMS.GdsMap.GdsMapLoadMetrics>(this.Map_FirstFrameMeasured);
            this.map.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Map_ChainMouseClick);
            this.map.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Map_ChainMouseDown);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGrid);
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1244, 1641);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Grid";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // GdsMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1478, 1770);
            this.Controls.Add(this.GDSContainer);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "GdsMapForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GDS Map Control";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GdsMapTestForm_ChainKeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numChainCellSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.statusStripMap.ResumeLayout(false);
            this.statusStripMap.PerformLayout();
            this.GDSContainer.Panel1.ResumeLayout(false);
            this.GDSContainer.Panel2.ResumeLayout(false);
            this.GDSContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GDSContainer)).EndInit();
            this.GDSContainer.ResumeLayout(false);
            this.chainSimilarListPanel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.chainListPanel.ResumeLayout(false);
            this.chainListButtons.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.chainSimilarRow.ResumeLayout(false);
            this.chainFirstRow.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

		#endregion
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.DataGridView dataGrid;
		private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblDbFactory;
        private System.Windows.Forms.TextBox txtDbFactory;
        private System.Windows.Forms.Label lblDbDevice;
        private System.Windows.Forms.TextBox txtDbDevice;
        private System.Windows.Forms.Button btnDbSave;
        private System.Windows.Forms.Button btnDbStop;
		private NexplantQMS.GdsMap.GdsMapControl map;
		private System.Windows.Forms.Button btnLayer;
        private System.Windows.Forms.Button btnLayerCheckNone;
        private System.Windows.Forms.Button btnLayerCheckAll;
        private System.Windows.Forms.StatusStrip statusStripMap;
        private System.Windows.Forms.ToolStripProgressBar progressMapLoad;
        private System.Windows.Forms.ToolStripStatusLabel lblMapCoordinate;
        private System.Windows.Forms.SplitContainer GDSContainer;
        private System.Windows.Forms.Panel panel2;
        private NexplantQMS.GdsMap.LayerColorCheckedListBox chkLayerItems;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        // Chain 화면 컨트롤은 디자이너에서 생성하고 Chain partial 파일에서 동작을 연결한다.
        private System.Windows.Forms.Panel chainListPanel;
        private System.Windows.Forms.Panel chainSimilarListPanel;
        private System.Windows.Forms.CheckedListBox _chainSimilarList;
        private System.Windows.Forms.Label chainSimilarListTitle;
        private System.Windows.Forms.CheckedListBox _chainList;
        private System.Windows.Forms.FlowLayoutPanel chainListButtons;
        private System.Windows.Forms.Button _btnChainNew;
        private System.Windows.Forms.Button _btnChainRename;
        private System.Windows.Forms.Button _btnChainDelete;
        private System.Windows.Forms.Label chainListTitle;
        private System.Windows.Forms.FlowLayoutPanel chainFirstRow;
        private System.Windows.Forms.FlowLayoutPanel chainSimilarRow;
        private System.Windows.Forms.Button _btnChainInput;
        private System.Windows.Forms.Button _btnChainOutput;
        private System.Windows.Forms.Button _btnChainApply;
        private System.Windows.Forms.Button _btnChainCancel;
        private System.Windows.Forms.Button _btnChainTrace;
        private System.Windows.Forms.Button _btnChainAdd;
        private System.Windows.Forms.Button _btnChainRemove;
        private System.Windows.Forms.Label chainCellSizeLabel;
        private System.Windows.Forms.NumericUpDown _numChainCellSize;
        private System.Windows.Forms.Button _btnChainExample;
        private System.Windows.Forms.Button _btnChainFindSimilar;
        private System.Windows.Forms.Button _btnChainClearSimilar;
        private System.Windows.Forms.Button _btnChainApplySimilar;
        private System.Windows.Forms.Button _btnChainUndoSimilar;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox _lblChainSetupStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblMapStatus;
    }
}

