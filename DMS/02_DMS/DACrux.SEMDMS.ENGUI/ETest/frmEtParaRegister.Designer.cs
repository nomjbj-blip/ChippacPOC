namespace DACrux.SEMDMS.ENGUI
{
    partial class frmEtParaRegister
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

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEtParaRegister));
            this.components = new System.ComponentModel.Container();
            this.topBar = new System.Windows.Forms.Panel();
            this.lblFactory = new System.Windows.Forms.Label();
            this.txtFactory = new System.Windows.Forms.TextBox();
            this.lblDevice = new System.Windows.Forms.Label();
            this.txtDevice = new System.Windows.Forms.TextBox();
            this.lblTestProgram = new System.Windows.Forms.Label();
            this.txtTestProgram = new System.Windows.Forms.TextBox();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.lblFileName = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.fileBar = new System.Windows.Forms.Panel();
            this.lblMeasFilter = new System.Windows.Forms.Label();
            this.cboMeasFilter = new System.Windows.Forms.ComboBox();
            this.lblTypeFilter = new System.Windows.Forms.Label();
            this.cboTypeFilter = new System.Windows.Forms.ComboBox();
            this.lblFind = new System.Windows.Forms.Label();
            this.txtFind = new System.Windows.Forms.TextBox();
            this.chkNewOnly = new System.Windows.Forms.CheckBox();
            this.btnCheckVisible = new System.Windows.Forms.Button();
            this.btnUncheckAll = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.dgvHeader = new System.Windows.Forms.DataGridView();
            this.colHCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colHStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHHeader = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHMeas = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHCond = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHChain1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHLeg1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHChain2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHLeg2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHTestNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHGds = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ctxHeader = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuHeaderDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHeaderRestore = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.busyBar = new DACrux.SEMDMS.Control.ProgressStatusBar();
            this.topBar.SuspendLayout();
            this.fileBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHeader)).BeginInit();
            this.ctxHeader.SuspendLayout();
            this.pnlStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // topBar
            // 
            this.topBar.Controls.Add(this.lblFactory);
            this.topBar.Controls.Add(this.txtFactory);
            this.topBar.Controls.Add(this.lblDevice);
            this.topBar.Controls.Add(this.txtDevice);
            this.topBar.Controls.Add(this.lblTestProgram);
            this.topBar.Controls.Add(this.txtTestProgram);
            this.topBar.Controls.Add(this.btnOpenFile);
            this.topBar.Controls.Add(this.lblFileName);
            this.topBar.Controls.Add(this.txtFileName);
            this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar.Location = new System.Drawing.Point(0, 0);
            this.topBar.Margin = new System.Windows.Forms.Padding(4);
            this.topBar.Name = "topBar";
            this.topBar.Size = new System.Drawing.Size(1857, 60);
            this.topBar.TabIndex = 0;
            // 
            // lblFactory
            // 
            this.lblFactory.AutoSize = true;
            this.lblFactory.Location = new System.Drawing.Point(14, 21);
            this.lblFactory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFactory.Name = "lblFactory";
            this.lblFactory.Size = new System.Drawing.Size(69, 18);
            this.lblFactory.TabIndex = 0;
            this.lblFactory.Text = "Factory";
            // 
            // txtFactory
            // 
            this.txtFactory.Location = new System.Drawing.Point(89, 16);
            this.txtFactory.Margin = new System.Windows.Forms.Padding(4);
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.ReadOnly = true;
            this.txtFactory.Size = new System.Drawing.Size(113, 28);
            this.txtFactory.TabIndex = 1;
            this.txtFactory.Leave += new System.EventHandler(this.txtFactory_Leave);
            // 
            // lblDevice
            // 
            this.lblDevice.AutoSize = true;
            this.lblDevice.Location = new System.Drawing.Point(223, 21);
            this.lblDevice.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(62, 18);
            this.lblDevice.TabIndex = 2;
            this.lblDevice.Text = "Device";
            // 
            // txtDevice
            // 
            this.txtDevice.Location = new System.Drawing.Point(291, 16);
            this.txtDevice.Margin = new System.Windows.Forms.Padding(4);
            this.txtDevice.Name = "txtDevice";
            this.txtDevice.ReadOnly = true;
            this.txtDevice.Size = new System.Drawing.Size(213, 28);
            this.txtDevice.TabIndex = 3;
            this.txtDevice.TabStop = false;
            // 
            // lblTestProgram
            // 
            this.lblTestProgram.AutoSize = true;
            this.lblTestProgram.Location = new System.Drawing.Point(526, 21);
            this.lblTestProgram.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTestProgram.Name = "lblTestProgram";
            this.lblTestProgram.Size = new System.Drawing.Size(44, 18);
            this.lblTestProgram.TabIndex = 4;
            this.lblTestProgram.Text = "공정";
            // 
            // txtTestProgram
            // 
            this.txtTestProgram.Location = new System.Drawing.Point(574, 16);
            this.txtTestProgram.Margin = new System.Windows.Forms.Padding(4);
            this.txtTestProgram.Name = "txtTestProgram";
            this.txtTestProgram.ReadOnly = true;
            this.txtTestProgram.Size = new System.Drawing.Size(113, 28);
            this.txtTestProgram.TabIndex = 5;
            this.txtTestProgram.TabStop = false;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.btnOpenFile.Location = new System.Drawing.Point(709, 9);
            this.btnOpenFile.Margin = new System.Windows.Forms.Padding(4);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOpenFile.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenFile.Image")));
            this.btnOpenFile.Size = new System.Drawing.Size(200, 42);
            this.btnOpenFile.TabIndex = 6;
            this.btnOpenFile.Text = "결과 파일 열기...";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(927, 21);
            this.lblFileName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(44, 18);
            this.lblFileName.TabIndex = 7;
            this.lblFileName.Text = "파일";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(972, 16);
            this.txtFileName.Margin = new System.Windows.Forms.Padding(4);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.ReadOnly = true;
            this.txtFileName.Size = new System.Drawing.Size(792, 28);
            this.txtFileName.TabIndex = 8;
            this.txtFileName.TabStop = false;
            // 
            // fileBar
            // 
            this.fileBar.Controls.Add(this.lblMeasFilter);
            this.fileBar.Controls.Add(this.cboMeasFilter);
            this.fileBar.Controls.Add(this.lblTypeFilter);
            this.fileBar.Controls.Add(this.cboTypeFilter);
            this.fileBar.Controls.Add(this.lblFind);
            this.fileBar.Controls.Add(this.txtFind);
            this.fileBar.Controls.Add(this.chkNewOnly);
            this.fileBar.Controls.Add(this.btnCheckVisible);
            this.fileBar.Controls.Add(this.btnUncheckAll);
            this.fileBar.Controls.Add(this.btnRegister);
            this.fileBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.fileBar.Location = new System.Drawing.Point(0, 60);
            this.fileBar.Margin = new System.Windows.Forms.Padding(4);
            this.fileBar.Name = "fileBar";
            this.fileBar.Size = new System.Drawing.Size(1857, 60);
            this.fileBar.TabIndex = 1;
            // 
            // lblMeasFilter
            // 
            this.lblMeasFilter.AutoSize = true;
            this.lblMeasFilter.Location = new System.Drawing.Point(12, 21);
            this.lblMeasFilter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMeasFilter.Name = "lblMeasFilter";
            this.lblMeasFilter.Size = new System.Drawing.Size(44, 18);
            this.lblMeasFilter.TabIndex = 1;
            this.lblMeasFilter.Text = "측정";
            // 
            // cboMeasFilter
            // 
            this.cboMeasFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMeasFilter.Location = new System.Drawing.Point(61, 17);
            this.cboMeasFilter.Margin = new System.Windows.Forms.Padding(4);
            this.cboMeasFilter.Name = "cboMeasFilter";
            this.cboMeasFilter.Size = new System.Drawing.Size(183, 26);
            this.cboMeasFilter.TabIndex = 2;
            this.cboMeasFilter.SelectedIndexChanged += new System.EventHandler(this.headerFilter_Changed);
            // 
            // lblTypeFilter
            // 
            this.lblTypeFilter.AutoSize = true;
            this.lblTypeFilter.Location = new System.Drawing.Point(252, 21);
            this.lblTypeFilter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTypeFilter.Name = "lblTypeFilter";
            this.lblTypeFilter.Size = new System.Drawing.Size(95, 18);
            this.lblTypeFilter.TabIndex = 3;
            this.lblTypeFilter.Text = "Chain 종류";
            // 
            // cboTypeFilter
            // 
            this.cboTypeFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTypeFilter.Location = new System.Drawing.Point(349, 17);
            this.cboTypeFilter.Margin = new System.Windows.Forms.Padding(4);
            this.cboTypeFilter.Name = "cboTypeFilter";
            this.cboTypeFilter.Size = new System.Drawing.Size(180, 26);
            this.cboTypeFilter.TabIndex = 4;
            this.cboTypeFilter.SelectedIndexChanged += new System.EventHandler(this.headerFilter_Changed);
            // 
            // lblFind
            // 
            this.lblFind.AutoSize = true;
            this.lblFind.Location = new System.Drawing.Point(1088, 21);
            this.lblFind.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFind.Name = "lblFind";
            this.lblFind.Size = new System.Drawing.Size(44, 18);
            this.lblFind.TabIndex = 5;
            this.lblFind.Text = "찾기";
            this.lblFind.Visible = false;
            // 
            // txtFind
            // 
            this.txtFind.Location = new System.Drawing.Point(1136, 16);
            this.txtFind.Margin = new System.Windows.Forms.Padding(4);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(213, 28);
            this.txtFind.TabIndex = 6;
            this.txtFind.Visible = false;
            this.txtFind.TextChanged += new System.EventHandler(this.headerFilter_Changed);
            // 
            // chkNewOnly
            // 
            this.chkNewOnly.AutoSize = true;
            this.chkNewOnly.Location = new System.Drawing.Point(992, 19);
            this.chkNewOnly.Margin = new System.Windows.Forms.Padding(4);
            this.chkNewOnly.Name = "chkNewOnly";
            this.chkNewOnly.Size = new System.Drawing.Size(88, 22);
            this.chkNewOnly.TabIndex = 7;
            this.chkNewOnly.Text = "신규만";
            this.chkNewOnly.UseVisualStyleBackColor = true;
            this.chkNewOnly.Visible = false;
            this.chkNewOnly.CheckedChanged += new System.EventHandler(this.headerFilter_Changed);
            // 
            // btnCheckVisible
            // 
            this.btnCheckVisible.Location = new System.Drawing.Point(549, 9);
            this.btnCheckVisible.Margin = new System.Windows.Forms.Padding(4);
            this.btnCheckVisible.Name = "btnCheckVisible";
            this.btnCheckVisible.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCheckVisible.Image = ((System.Drawing.Image)(resources.GetObject("btnCheckVisible.Image")));
            this.btnCheckVisible.Size = new System.Drawing.Size(177, 42);
            this.btnCheckVisible.TabIndex = 8;
            this.btnCheckVisible.Text = "신규 전체 선택";
            this.btnCheckVisible.UseVisualStyleBackColor = true;
            this.btnCheckVisible.Click += new System.EventHandler(this.btnCheckVisible_Click);
            // 
            // btnUncheckAll
            // 
            this.btnUncheckAll.Location = new System.Drawing.Point(735, 9);
            this.btnUncheckAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnUncheckAll.Name = "btnUncheckAll";
            this.btnUncheckAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUncheckAll.Image = ((System.Drawing.Image)(resources.GetObject("btnUncheckAll.Image")));
            this.btnUncheckAll.Size = new System.Drawing.Size(177, 42);
            this.btnUncheckAll.TabIndex = 9;
            this.btnUncheckAll.Text = "선택 전체 해제";
            this.btnUncheckAll.UseVisualStyleBackColor = true;
            this.btnUncheckAll.Click += new System.EventHandler(this.btnUncheckAll_Click);
            // 
            // btnRegister
            // 
            this.btnRegister.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRegister.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.btnRegister.Location = new System.Drawing.Point(1717, 9);
            this.btnRegister.Margin = new System.Windows.Forms.Padding(4);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRegister.Image = ((System.Drawing.Image)(resources.GetObject("btnRegister.Image")));
            this.btnRegister.Size = new System.Drawing.Size(126, 42);
            this.btnRegister.TabIndex = 10;
            this.btnRegister.Text = "저장";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // dgvHeader
            // 
            this.dgvHeader.AllowUserToAddRows = false;
            this.dgvHeader.AllowUserToDeleteRows = false;
            this.dgvHeader.AllowUserToResizeRows = false;
            this.dgvHeader.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHeader.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colHCheck,
            this.colHStatus,
            this.colHHeader,
            this.colHName,
            this.colHMeas,
            this.colHCond,
            this.colHChain1,
            this.colHLeg1,
            this.colHChain2,
            this.colHLeg2,
            this.colHUnit,
            this.colHTestNo,
            this.colHGds,
            this.colHSpec,
            this.colHMessage});
            this.dgvHeader.ContextMenuStrip = this.ctxHeader;
            this.dgvHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHeader.Location = new System.Drawing.Point(0, 120);
            this.dgvHeader.Margin = new System.Windows.Forms.Padding(4);
            this.dgvHeader.Name = "dgvHeader";
            this.dgvHeader.RowHeadersWidth = 56;
            this.dgvHeader.RowTemplate.Height = 21;
            this.dgvHeader.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHeader.Size = new System.Drawing.Size(1857, 926);
            this.dgvHeader.TabIndex = 2;
            this.dgvHeader.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvHeader_CellMouseDown);
            this.dgvHeader.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvHeader_CurrentCellDirtyStateChanged);
            this.dgvHeader.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvHeader_KeyDown);
            // 
            // colHCheck
            // 
            this.colHCheck.HeaderText = "선택";
            this.colHCheck.MinimumWidth = 8;
            this.colHCheck.Name = "colHCheck";
            this.colHCheck.Width = 40;
            // 
            // colHStatus
            // 
            this.colHStatus.HeaderText = "상태";
            this.colHStatus.MinimumWidth = 8;
            this.colHStatus.Name = "colHStatus";
            this.colHStatus.ReadOnly = true;
            this.colHStatus.Width = 70;
            // 
            // colHHeader
            // 
            this.colHHeader.HeaderText = "Parameter (헤더 원문)";
            this.colHHeader.MinimumWidth = 8;
            this.colHHeader.Name = "colHHeader";
            this.colHHeader.ReadOnly = true;
            this.colHHeader.Width = 300;
            // 
            // colHName
            // 
            this.colHName.HeaderText = "저장 이름";
            this.colHName.MinimumWidth = 8;
            this.colHName.Name = "colHName";
            this.colHName.ReadOnly = true;
            this.colHName.Width = 260;
            // 
            // colHMeas
            // 
            this.colHMeas.HeaderText = "측정";
            this.colHMeas.MinimumWidth = 8;
            this.colHMeas.Name = "colHMeas";
            this.colHMeas.ReadOnly = true;
            this.colHMeas.Width = 90;
            // 
            // colHCond
            // 
            this.colHCond.HeaderText = "조건";
            this.colHCond.MinimumWidth = 8;
            this.colHCond.Name = "colHCond";
            this.colHCond.ReadOnly = true;
            this.colHCond.Width = 150;
            // 
            // colHChain1
            // 
            this.colHChain1.HeaderText = "Chain 1";
            this.colHChain1.MinimumWidth = 8;
            this.colHChain1.Name = "colHChain1";
            this.colHChain1.ReadOnly = true;
            this.colHChain1.Width = 140;
            // 
            // colHLeg1
            // 
            this.colHLeg1.HeaderText = "Leg";
            this.colHLeg1.MinimumWidth = 8;
            this.colHLeg1.Name = "colHLeg1";
            this.colHLeg1.ReadOnly = true;
            this.colHLeg1.Width = 36;
            // 
            // colHChain2
            // 
            this.colHChain2.HeaderText = "Chain 2";
            this.colHChain2.MinimumWidth = 8;
            this.colHChain2.Name = "colHChain2";
            this.colHChain2.ReadOnly = true;
            this.colHChain2.Width = 140;
            // 
            // colHLeg2
            // 
            this.colHLeg2.HeaderText = "Leg";
            this.colHLeg2.MinimumWidth = 8;
            this.colHLeg2.Name = "colHLeg2";
            this.colHLeg2.ReadOnly = true;
            this.colHLeg2.Width = 36;
            // 
            // colHUnit
            // 
            this.colHUnit.HeaderText = "단위";
            this.colHUnit.MinimumWidth = 8;
            this.colHUnit.Name = "colHUnit";
            this.colHUnit.ReadOnly = true;
            this.colHUnit.Width = 50;
            // 
            // colHTestNo
            // 
            this.colHTestNo.HeaderText = "Test No";
            this.colHTestNo.MinimumWidth = 8;
            this.colHTestNo.Name = "colHTestNo";
            this.colHTestNo.ReadOnly = true;
            this.colHTestNo.Width = 60;
            // 
            // colHGds
            // 
            this.colHGds.HeaderText = "GDS Chain";
            this.colHGds.MinimumWidth = 8;
            this.colHGds.Name = "colHGds";
            this.colHGds.ReadOnly = true;
            this.colHGds.Width = 140;
            // 
            // colHSpec
            // 
            this.colHSpec.HeaderText = "Spec";
            this.colHSpec.MinimumWidth = 8;
            this.colHSpec.Name = "colHSpec";
            this.colHSpec.ReadOnly = true;
            this.colHSpec.Width = 110;
            // 
            // colHMessage
            // 
            this.colHMessage.HeaderText = "메시지";
            this.colHMessage.MinimumWidth = 8;
            this.colHMessage.Name = "colHMessage";
            this.colHMessage.ReadOnly = true;
            this.colHMessage.Width = 300;
            // 
            // ctxHeader
            // 
            this.ctxHeader.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.ctxHeader.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHeaderDelete,
            this.mnuHeaderRestore});
            this.ctxHeader.Name = "ctxHeader";
            this.ctxHeader.Size = new System.Drawing.Size(283, 68);
            this.ctxHeader.Opening += new System.ComponentModel.CancelEventHandler(this.ctxHeader_Opening);
            // 
            // mnuHeaderDelete
            // 
            this.mnuHeaderDelete.Name = "mnuHeaderDelete";
            this.mnuHeaderDelete.Size = new System.Drawing.Size(282, 32);
            this.mnuHeaderDelete.Text = "선택 행 목록에서 삭제";
            this.mnuHeaderDelete.Click += new System.EventHandler(this.mnuHeaderDelete_Click);
            // 
            // mnuHeaderRestore
            // 
            this.mnuHeaderRestore.Name = "mnuHeaderRestore";
            this.mnuHeaderRestore.Size = new System.Drawing.Size(282, 32);
            this.mnuHeaderRestore.Text = "삭제한 행 모두 되살리기";
            this.mnuHeaderRestore.Click += new System.EventHandler(this.mnuHeaderRestore_Click);
            // 
            // pnlStatus
            // 
            this.pnlStatus.Controls.Add(this.lblStatus);
            this.pnlStatus.Controls.Add(this.busyBar);
            this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlStatus.Location = new System.Drawing.Point(0, 1041);
            this.pnlStatus.Margin = new System.Windows.Forms.Padding(4);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(1857, 39);
            this.pnlStatus.TabIndex = 3;
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Location = new System.Drawing.Point(0, 0);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(11, 0, 0, 0);
            this.lblStatus.Size = new System.Drawing.Size(1257, 39);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // busyBar
            // 
            this.busyBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.busyBar.Location = new System.Drawing.Point(1257, 0);
            this.busyBar.Margin = new System.Windows.Forms.Padding(4);
            this.busyBar.Name = "busyBar";
            this.busyBar.Padding = new System.Windows.Forms.Padding(0, 4, 9, 4);
            this.busyBar.Size = new System.Drawing.Size(600, 39);
            this.busyBar.TabIndex = 1;
            // 
            // frmEtParaRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1857, 1080);
            this.Controls.Add(this.dgvHeader);
            this.Controls.Add(this.fileBar);
            this.Controls.Add(this.pnlStatus);
            this.Controls.Add(this.topBar);
            this.Margin = new System.Windows.Forms.Padding(9);
            this.Name = "frmEtParaRegister";
            this.Text = "E-Test Para 등록";
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.fileBar.ResumeLayout(false);
            this.fileBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHeader)).EndInit();
            this.ctxHeader.ResumeLayout(false);
            this.pnlStatus.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Label lblFactory;
        private System.Windows.Forms.TextBox txtFactory;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.TextBox txtDevice;
        private System.Windows.Forms.Label lblTestProgram;
        private System.Windows.Forms.TextBox txtTestProgram;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Panel fileBar;
        private System.Windows.Forms.Label lblMeasFilter;
        private System.Windows.Forms.ComboBox cboMeasFilter;
        private System.Windows.Forms.Label lblTypeFilter;
        private System.Windows.Forms.ComboBox cboTypeFilter;
        private System.Windows.Forms.Label lblFind;
        private System.Windows.Forms.TextBox txtFind;
        private System.Windows.Forms.CheckBox chkNewOnly;
        private System.Windows.Forms.Button btnCheckVisible;
        private System.Windows.Forms.Button btnUncheckAll;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.DataGridView dgvHeader;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colHCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHHeader;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHMeas;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHCond;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHChain1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHLeg1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHChain2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHLeg2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHTestNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHGds;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHMessage;
        private System.Windows.Forms.ContextMenuStrip ctxHeader;
        private System.Windows.Forms.ToolStripMenuItem mnuHeaderDelete;
        private System.Windows.Forms.ToolStripMenuItem mnuHeaderRestore;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Label lblStatus;
        private DACrux.SEMDMS.Control.ProgressStatusBar busyBar;
    }
}
