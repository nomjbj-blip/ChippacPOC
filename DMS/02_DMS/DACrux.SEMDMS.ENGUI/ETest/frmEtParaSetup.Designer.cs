namespace DACrux.SEMDMS.ENGUI
{
    partial class frmEtParaSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEtParaSetup));
            this.topBar = new System.Windows.Forms.Panel();
            this.lblFactory = new System.Windows.Forms.Label();
            this.txtFactory = new System.Windows.Forms.TextBox();
            this.lblDevice = new System.Windows.Forms.Label();
            this.cboDevice = new System.Windows.Forms.ComboBox();
            this.lblTestProgram = new System.Windows.Forms.Label();
            this.cboTestProgram = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.splitPara = new System.Windows.Forms.SplitContainer();
            this.dgvPara = new System.Windows.Forms.DataGridView();
            this.colPHeader = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPMeas = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPCond = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPChain1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPChain2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPUse = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colPGds = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPLower = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPUpper = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPEnable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colPRemark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPSpecText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPRegFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPUpdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvGds = new System.Windows.Forms.DataGridView();
            this.colGCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGUse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gdsBar = new System.Windows.Forms.Panel();
            this.lblGdsDevice = new System.Windows.Forms.Label();
            this.txtGdsDevice = new System.Windows.Forms.TextBox();
            this.lblGdsInfo = new System.Windows.Forms.Label();
            this.paraBar = new System.Windows.Forms.Panel();
            this.lblParaFind = new System.Windows.Forms.Label();
            this.txtParaFind = new System.Windows.Forms.TextBox();
            this.chkSpecOnly = new System.Windows.Forms.CheckBox();
            this.chkNoGdsOnly = new System.Windows.Forms.CheckBox();
            this.btnPasteSpec = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClearFilter = new System.Windows.Forms.Button();
            this.lblFltMeas = new System.Windows.Forms.Label();
            this.cboFltMeas = new System.Windows.Forms.ComboBox();
            this.lblFltCond = new System.Windows.Forms.Label();
            this.cboFltCond = new System.Windows.Forms.ComboBox();
            this.lblFltChain1 = new System.Windows.Forms.Label();
            this.cboFltChain1 = new System.Windows.Forms.ComboBox();
            this.lblFltGds = new System.Windows.Forms.Label();
            this.cboFltGds = new System.Windows.Forms.ComboBox();
            this.lblFltUse = new System.Windows.Forms.Label();
            this.cboFltUse = new System.Windows.Forms.ComboBox();
            this.lblFltEnable = new System.Windows.Forms.Label();
            this.cboFltEnable = new System.Windows.Forms.ComboBox();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.busyBar = new DACrux.SEMDMS.Control.ProgressStatusBar();
            this.topBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitPara)).BeginInit();
            this.splitPara.Panel1.SuspendLayout();
            this.splitPara.Panel2.SuspendLayout();
            this.splitPara.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPara)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGds)).BeginInit();
            this.gdsBar.SuspendLayout();
            this.paraBar.SuspendLayout();
            this.pnlStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // topBar
            // 
            this.topBar.Controls.Add(this.lblFactory);
            this.topBar.Controls.Add(this.txtFactory);
            this.topBar.Controls.Add(this.lblDevice);
            this.topBar.Controls.Add(this.cboDevice);
            this.topBar.Controls.Add(this.lblTestProgram);
            this.topBar.Controls.Add(this.cboTestProgram);
            this.topBar.Controls.Add(this.btnSearch);
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
            // cboDevice
            // 
            this.cboDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDevice.Location = new System.Drawing.Point(291, 17);
            this.cboDevice.Margin = new System.Windows.Forms.Padding(4);
            this.cboDevice.Name = "cboDevice";
            this.cboDevice.Size = new System.Drawing.Size(213, 26);
            this.cboDevice.TabIndex = 3;
            this.cboDevice.SelectedIndexChanged += new System.EventHandler(this.cboDevice_SelectedIndexChanged);
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
            // cboTestProgram
            // 
            this.cboTestProgram.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTestProgram.Location = new System.Drawing.Point(574, 17);
            this.cboTestProgram.Margin = new System.Windows.Forms.Padding(4);
            this.cboTestProgram.Name = "cboTestProgram";
            this.cboTestProgram.Size = new System.Drawing.Size(152, 26);
            this.cboTestProgram.TabIndex = 5;
            this.cboTestProgram.SelectedIndexChanged += new System.EventHandler(this.cboTestProgram_SelectedIndexChanged);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(740, 9);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.Size = new System.Drawing.Size(126, 42);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // splitPara
            // 
            this.splitPara.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitPara.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitPara.Location = new System.Drawing.Point(0, 120);
            this.splitPara.Margin = new System.Windows.Forms.Padding(4);
            this.splitPara.Name = "splitPara";
            // 
            // splitPara.Panel1
            // 
            this.splitPara.Panel1.Controls.Add(this.dgvPara);
            // 
            // splitPara.Panel2
            // 
            this.splitPara.Panel2.Controls.Add(this.dgvGds);
            this.splitPara.Panel2.Controls.Add(this.gdsBar);
            this.splitPara.Size = new System.Drawing.Size(1857, 929);
            this.splitPara.SplitterDistance = 1483;
            this.splitPara.SplitterWidth = 6;
            this.splitPara.TabIndex = 2;
            // 
            // dgvPara
            // 
            this.dgvPara.AllowUserToAddRows = false;
            this.dgvPara.AllowUserToDeleteRows = false;
            this.dgvPara.AllowUserToResizeRows = false;
            this.dgvPara.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvPara.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPara.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPHeader,
            this.colPName,
            this.colPMeas,
            this.colPCond,
            this.colPChain1,
            this.colPChain2,
            this.colPUnit,
            this.colPUse,
            this.colPGds,
            this.colPLower,
            this.colPUpper,
            this.colPEnable,
            this.colPRemark,
            this.colPSpecText,
            this.colPSource,
            this.colPRegFile,
            this.colPUpdate});
            this.dgvPara.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPara.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvPara.Location = new System.Drawing.Point(0, 0);
            this.dgvPara.Margin = new System.Windows.Forms.Padding(4);
            this.dgvPara.Name = "dgvPara";
            this.dgvPara.RowHeadersWidth = 28;
            this.dgvPara.RowTemplate.Height = 21;
            this.dgvPara.Size = new System.Drawing.Size(1483, 929);
            this.dgvPara.TabIndex = 0;
            this.dgvPara.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPara_CellContentClick);
            this.dgvPara.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPara_CellDoubleClick);
            this.dgvPara.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPara_CellEndEdit);
            this.dgvPara.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grid_CellMouseDown);
            this.dgvPara.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPara_CellValueChanged);
            this.dgvPara.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvPara_KeyDown);
            // 
            // colPHeader
            // 
            this.colPHeader.Frozen = true;
            this.colPHeader.HeaderText = "Parameter (헤더 원문)";
            this.colPHeader.MinimumWidth = 8;
            this.colPHeader.Name = "colPHeader";
            this.colPHeader.ReadOnly = true;
            this.colPHeader.Width = 280;
            // 
            // colPName
            // 
            this.colPName.HeaderText = "저장 이름";
            this.colPName.MinimumWidth = 8;
            this.colPName.Name = "colPName";
            this.colPName.ReadOnly = true;
            this.colPName.Width = 220;
            // 
            // colPMeas
            // 
            this.colPMeas.HeaderText = "측정";
            this.colPMeas.MinimumWidth = 8;
            this.colPMeas.Name = "colPMeas";
            this.colPMeas.ReadOnly = true;
            this.colPMeas.Width = 90;
            // 
            // colPCond
            // 
            this.colPCond.HeaderText = "조건";
            this.colPCond.MinimumWidth = 8;
            this.colPCond.Name = "colPCond";
            this.colPCond.ReadOnly = true;
            this.colPCond.Width = 150;
            // 
            // colPChain1
            // 
            this.colPChain1.HeaderText = "Chain 1";
            this.colPChain1.MinimumWidth = 8;
            this.colPChain1.Name = "colPChain1";
            this.colPChain1.ReadOnly = true;
            this.colPChain1.Width = 140;
            // 
            // colPChain2
            // 
            this.colPChain2.HeaderText = "Chain 2";
            this.colPChain2.MinimumWidth = 8;
            this.colPChain2.Name = "colPChain2";
            this.colPChain2.ReadOnly = true;
            this.colPChain2.Width = 140;
            // 
            // colPUnit
            // 
            this.colPUnit.HeaderText = "단위";
            this.colPUnit.MinimumWidth = 8;
            this.colPUnit.Name = "colPUnit";
            this.colPUnit.ReadOnly = true;
            this.colPUnit.Width = 50;
            // 
            // colPUse
            // 
            this.colPUse.HeaderText = "사용";
            this.colPUse.MinimumWidth = 8;
            this.colPUse.Name = "colPUse";
            this.colPUse.ReadOnly = true;
            this.colPUse.Width = 44;
            // 
            // colPGds
            // 
            this.colPGds.HeaderText = "GDS Chain (Chain 1)";
            this.colPGds.MinimumWidth = 8;
            this.colPGds.Name = "colPGds";
            this.colPGds.Width = 160;
            // 
            // colPLower
            // 
            this.colPLower.HeaderText = "Lower";
            this.colPLower.MinimumWidth = 8;
            this.colPLower.Name = "colPLower";
            this.colPLower.Width = 70;
            // 
            // colPUpper
            // 
            this.colPUpper.HeaderText = "Upper";
            this.colPUpper.MinimumWidth = 8;
            this.colPUpper.Name = "colPUpper";
            this.colPUpper.Width = 70;
            // 
            // colPEnable
            // 
            this.colPEnable.HeaderText = "Enable";
            this.colPEnable.MinimumWidth = 8;
            this.colPEnable.Name = "colPEnable";
            this.colPEnable.ReadOnly = true;
            this.colPEnable.Width = 50;
            // 
            // colPRemark
            // 
            this.colPRemark.HeaderText = "비고";
            this.colPRemark.MinimumWidth = 8;
            this.colPRemark.Name = "colPRemark";
            this.colPRemark.Width = 150;
            // 
            // colPSpecText
            // 
            this.colPSpecText.HeaderText = "Spec 원문";
            this.colPSpecText.MinimumWidth = 8;
            this.colPSpecText.Name = "colPSpecText";
            this.colPSpecText.ReadOnly = true;
            this.colPSpecText.Width = 200;
            // 
            // colPSource
            // 
            this.colPSource.HeaderText = "출처";
            this.colPSource.MinimumWidth = 8;
            this.colPSource.Name = "colPSource";
            this.colPSource.ReadOnly = true;
            this.colPSource.Width = 70;
            // 
            // colPRegFile
            // 
            this.colPRegFile.HeaderText = "등록 파일";
            this.colPRegFile.MinimumWidth = 8;
            this.colPRegFile.Name = "colPRegFile";
            this.colPRegFile.ReadOnly = true;
            this.colPRegFile.Width = 200;
            // 
            // colPUpdate
            // 
            this.colPUpdate.HeaderText = "수정";
            this.colPUpdate.MinimumWidth = 8;
            this.colPUpdate.Name = "colPUpdate";
            this.colPUpdate.ReadOnly = true;
            this.colPUpdate.Width = 150;
            // 
            // dgvGds
            // 
            this.dgvGds.AllowUserToAddRows = false;
            this.dgvGds.AllowUserToDeleteRows = false;
            this.dgvGds.AllowUserToResizeRows = false;
            this.dgvGds.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvGds.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGds.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colGCode,
            this.colGName,
            this.colGUse});
            this.dgvGds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGds.Location = new System.Drawing.Point(0, 84);
            this.dgvGds.Margin = new System.Windows.Forms.Padding(4);
            this.dgvGds.MultiSelect = false;
            this.dgvGds.Name = "dgvGds";
            this.dgvGds.ReadOnly = true;
            this.dgvGds.RowHeadersVisible = false;
            this.dgvGds.RowHeadersWidth = 62;
            this.dgvGds.RowTemplate.Height = 21;
            this.dgvGds.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGds.Size = new System.Drawing.Size(368, 845);
            this.dgvGds.TabIndex = 1;
            this.dgvGds.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGds_CellDoubleClick);
            this.dgvGds.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grid_CellMouseDown);
            this.dgvGds.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvGds_KeyDown);
            // 
            // colGCode
            // 
            this.colGCode.HeaderText = "Code";
            this.colGCode.MinimumWidth = 8;
            this.colGCode.Name = "colGCode";
            this.colGCode.ReadOnly = true;
            this.colGCode.Visible = false;
            this.colGCode.Width = 70;
            // 
            // colGName
            // 
            this.colGName.HeaderText = "GDS Setup Chain";
            this.colGName.MinimumWidth = 8;
            this.colGName.Name = "colGName";
            this.colGName.ReadOnly = true;
            this.colGName.Width = 260;
            // 
            // colGUse
            // 
            this.colGUse.HeaderText = "사용 수";
            this.colGUse.MinimumWidth = 8;
            this.colGUse.Name = "colGUse";
            this.colGUse.ReadOnly = true;
            this.colGUse.Width = 60;
            // 
            // gdsBar
            // 
            this.gdsBar.Controls.Add(this.lblGdsDevice);
            this.gdsBar.Controls.Add(this.txtGdsDevice);
            this.gdsBar.Controls.Add(this.lblGdsInfo);
            this.gdsBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.gdsBar.Location = new System.Drawing.Point(0, 0);
            this.gdsBar.Margin = new System.Windows.Forms.Padding(4);
            this.gdsBar.Name = "gdsBar";
            this.gdsBar.Size = new System.Drawing.Size(368, 84);
            this.gdsBar.TabIndex = 0;
            // 
            // lblGdsDevice
            // 
            this.lblGdsDevice.AutoSize = true;
            this.lblGdsDevice.Location = new System.Drawing.Point(6, 14);
            this.lblGdsDevice.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGdsDevice.Name = "lblGdsDevice";
            this.lblGdsDevice.Size = new System.Drawing.Size(102, 18);
            this.lblGdsDevice.TabIndex = 0;
            this.lblGdsDevice.Text = "GDS Device";
            // 
            // txtGdsDevice
            // 
            this.txtGdsDevice.Location = new System.Drawing.Point(111, 8);
            this.txtGdsDevice.Margin = new System.Windows.Forms.Padding(4);
            this.txtGdsDevice.Name = "txtGdsDevice";
            this.txtGdsDevice.ReadOnly = true;
            this.txtGdsDevice.Size = new System.Drawing.Size(355, 28);
            this.txtGdsDevice.TabIndex = 1;
            this.txtGdsDevice.TabStop = false;
            // 
            // lblGdsInfo
            // 
            this.lblGdsInfo.AutoSize = true;
            this.lblGdsInfo.ForeColor = System.Drawing.Color.DimGray;
            this.lblGdsInfo.Location = new System.Drawing.Point(6, 51);
            this.lblGdsInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGdsInfo.Name = "lblGdsInfo";
            this.lblGdsInfo.Size = new System.Drawing.Size(145, 18);
            this.lblGdsInfo.TabIndex = 2;
            this.lblGdsInfo.Text = "GDS Setup Chain";
            // 
            // paraBar
            // 
            this.paraBar.Controls.Add(this.lblParaFind);
            this.paraBar.Controls.Add(this.txtParaFind);
            this.paraBar.Controls.Add(this.chkSpecOnly);
            this.paraBar.Controls.Add(this.chkNoGdsOnly);
            this.paraBar.Controls.Add(this.btnPasteSpec);
            this.paraBar.Controls.Add(this.btnDelete);
            this.paraBar.Controls.Add(this.btnSave);
            this.paraBar.Controls.Add(this.btnClearFilter);
            this.paraBar.Controls.Add(this.lblFltMeas);
            this.paraBar.Controls.Add(this.cboFltMeas);
            this.paraBar.Controls.Add(this.lblFltCond);
            this.paraBar.Controls.Add(this.cboFltCond);
            this.paraBar.Controls.Add(this.lblFltChain1);
            this.paraBar.Controls.Add(this.cboFltChain1);
            this.paraBar.Controls.Add(this.lblFltGds);
            this.paraBar.Controls.Add(this.cboFltGds);
            this.paraBar.Controls.Add(this.lblFltUse);
            this.paraBar.Controls.Add(this.cboFltUse);
            this.paraBar.Controls.Add(this.lblFltEnable);
            this.paraBar.Controls.Add(this.cboFltEnable);
            this.paraBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.paraBar.Location = new System.Drawing.Point(0, 60);
            this.paraBar.Margin = new System.Windows.Forms.Padding(4);
            this.paraBar.Name = "paraBar";
            this.paraBar.Size = new System.Drawing.Size(1857, 60);
            this.paraBar.TabIndex = 1;
            // 
            // lblParaFind
            // 
            this.lblParaFind.AutoSize = true;
            this.lblParaFind.Location = new System.Drawing.Point(9, 21);
            this.lblParaFind.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblParaFind.Name = "lblParaFind";
            this.lblParaFind.Size = new System.Drawing.Size(44, 18);
            this.lblParaFind.TabIndex = 1;
            this.lblParaFind.Text = "찾기";
            this.lblParaFind.Visible = false;
            // 
            // txtParaFind
            // 
            this.txtParaFind.Location = new System.Drawing.Point(57, 16);
            this.txtParaFind.Margin = new System.Windows.Forms.Padding(4);
            this.txtParaFind.Name = "txtParaFind";
            this.txtParaFind.Size = new System.Drawing.Size(213, 28);
            this.txtParaFind.TabIndex = 2;
            this.txtParaFind.Visible = false;
            this.txtParaFind.TextChanged += new System.EventHandler(this.paraFilter_Changed);
            // 
            // chkSpecOnly
            // 
            this.chkSpecOnly.AutoSize = true;
            this.chkSpecOnly.Location = new System.Drawing.Point(291, 19);
            this.chkSpecOnly.Margin = new System.Windows.Forms.Padding(4);
            this.chkSpecOnly.Name = "chkSpecOnly";
            this.chkSpecOnly.Size = new System.Drawing.Size(159, 22);
            this.chkSpecOnly.TabIndex = 3;
            this.chkSpecOnly.Text = "Spec 있는 것만";
            this.chkSpecOnly.UseVisualStyleBackColor = true;
            this.chkSpecOnly.Visible = false;
            this.chkSpecOnly.CheckedChanged += new System.EventHandler(this.paraFilter_Changed);
            // 
            // chkNoGdsOnly
            // 
            this.chkNoGdsOnly.AutoSize = true;
            this.chkNoGdsOnly.Location = new System.Drawing.Point(457, 19);
            this.chkNoGdsOnly.Margin = new System.Windows.Forms.Padding(4);
            this.chkNoGdsOnly.Name = "chkNoGdsOnly";
            this.chkNoGdsOnly.Size = new System.Drawing.Size(203, 22);
            this.chkNoGdsOnly.TabIndex = 4;
            this.chkNoGdsOnly.Text = "GDS Chain 없는 것만";
            this.chkNoGdsOnly.UseVisualStyleBackColor = true;
            this.chkNoGdsOnly.Visible = false;
            this.chkNoGdsOnly.CheckedChanged += new System.EventHandler(this.paraFilter_Changed);
            // 
            // btnPasteSpec
            // 
            this.btnPasteSpec.Location = new System.Drawing.Point(677, 9);
            this.btnPasteSpec.Margin = new System.Windows.Forms.Padding(4);
            this.btnPasteSpec.Name = "btnPasteSpec";
            this.btnPasteSpec.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPasteSpec.Image = ((System.Drawing.Image)(resources.GetObject("btnPasteSpec.Image")));
            this.btnPasteSpec.Size = new System.Drawing.Size(206, 42);
            this.btnPasteSpec.TabIndex = 5;
            this.btnPasteSpec.Text = "Spec 표 붙여넣기";
            this.btnPasteSpec.UseVisualStyleBackColor = true;
            this.btnPasteSpec.Visible = false;
            this.btnPasteSpec.Click += new System.EventHandler(this.btnPasteSpec_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(892, 9);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.Size = new System.Drawing.Size(137, 42);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "선택 삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Visible = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(1717, 9);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Size = new System.Drawing.Size(126, 42);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // btnClearFilter
            //
            this.btnClearFilter.Enabled = false;
            this.btnClearFilter.Location = new System.Drawing.Point(1485, 9);
            this.btnClearFilter.Margin = new System.Windows.Forms.Padding(4);
            this.btnClearFilter.Name = "btnClearFilter";
            this.btnClearFilter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClearFilter.Image = ((System.Drawing.Image)(resources.GetObject("btnClearFilter.Image")));
            this.btnClearFilter.Size = new System.Drawing.Size(137, 42);
            this.btnClearFilter.TabIndex = 8;
            this.btnClearFilter.Text = "필터 해제";
            this.btnClearFilter.UseVisualStyleBackColor = true;
            this.btnClearFilter.Click += new System.EventHandler(this.btnClearFilter_Click);
            // 
            // lblFltMeas
            // 
            this.lblFltMeas.AutoSize = true;
            this.lblFltMeas.Location = new System.Drawing.Point(9, 21);
            this.lblFltMeas.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFltMeas.Name = "lblFltMeas";
            this.lblFltMeas.TabIndex = 9;
            this.lblFltMeas.Text = "측정";
            // 
            // cboFltMeas
            // 
            this.cboFltMeas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFltMeas.DropDownWidth = 220;
            this.cboFltMeas.Location = new System.Drawing.Point(57, 17);
            this.cboFltMeas.Margin = new System.Windows.Forms.Padding(4);
            this.cboFltMeas.Name = "cboFltMeas";
            this.cboFltMeas.Size = new System.Drawing.Size(140, 26);
            this.cboFltMeas.TabIndex = 10;
            this.cboFltMeas.DropDown += new System.EventHandler(this.cboFlt_DropDown);
            this.cboFltMeas.SelectedIndexChanged += new System.EventHandler(this.cboFlt_SelectedIndexChanged);
            // 
            // lblFltCond
            // 
            this.lblFltCond.AutoSize = true;
            this.lblFltCond.Location = new System.Drawing.Point(211, 21);
            this.lblFltCond.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFltCond.Name = "lblFltCond";
            this.lblFltCond.TabIndex = 11;
            this.lblFltCond.Text = "조건";
            // 
            // cboFltCond
            // 
            this.cboFltCond.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFltCond.DropDownWidth = 260;
            this.cboFltCond.Location = new System.Drawing.Point(259, 17);
            this.cboFltCond.Margin = new System.Windows.Forms.Padding(4);
            this.cboFltCond.Name = "cboFltCond";
            this.cboFltCond.Size = new System.Drawing.Size(200, 26);
            this.cboFltCond.TabIndex = 12;
            this.cboFltCond.DropDown += new System.EventHandler(this.cboFlt_DropDown);
            this.cboFltCond.SelectedIndexChanged += new System.EventHandler(this.cboFlt_SelectedIndexChanged);
            // 
            // lblFltChain1
            // 
            this.lblFltChain1.AutoSize = true;
            this.lblFltChain1.Location = new System.Drawing.Point(473, 21);
            this.lblFltChain1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFltChain1.Name = "lblFltChain1";
            this.lblFltChain1.TabIndex = 13;
            this.lblFltChain1.Text = "Chain 1";
            // 
            // cboFltChain1
            // 
            this.cboFltChain1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFltChain1.DropDownWidth = 320;
            this.cboFltChain1.Location = new System.Drawing.Point(545, 17);
            this.cboFltChain1.Margin = new System.Windows.Forms.Padding(4);
            this.cboFltChain1.Name = "cboFltChain1";
            this.cboFltChain1.Size = new System.Drawing.Size(210, 26);
            this.cboFltChain1.TabIndex = 14;
            this.cboFltChain1.DropDown += new System.EventHandler(this.cboFlt_DropDown);
            this.cboFltChain1.SelectedIndexChanged += new System.EventHandler(this.cboFlt_SelectedIndexChanged);
            // 
            // lblFltGds
            // 
            this.lblFltGds.AutoSize = true;
            this.lblFltGds.Location = new System.Drawing.Point(769, 21);
            this.lblFltGds.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFltGds.Name = "lblFltGds";
            this.lblFltGds.TabIndex = 15;
            this.lblFltGds.Text = "GDS Chain";
            // 
            // cboFltGds
            // 
            this.cboFltGds.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFltGds.DropDownWidth = 320;
            this.cboFltGds.Location = new System.Drawing.Point(867, 17);
            this.cboFltGds.Margin = new System.Windows.Forms.Padding(4);
            this.cboFltGds.Name = "cboFltGds";
            this.cboFltGds.Size = new System.Drawing.Size(210, 26);
            this.cboFltGds.TabIndex = 16;
            this.cboFltGds.DropDown += new System.EventHandler(this.cboFlt_DropDown);
            this.cboFltGds.SelectedIndexChanged += new System.EventHandler(this.cboFlt_SelectedIndexChanged);
            // 
            // lblFltUse
            // 
            this.lblFltUse.AutoSize = true;
            this.lblFltUse.Location = new System.Drawing.Point(1091, 21);
            this.lblFltUse.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFltUse.Name = "lblFltUse";
            this.lblFltUse.TabIndex = 17;
            this.lblFltUse.Text = "사용";
            // 
            // cboFltUse
            // 
            this.cboFltUse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFltUse.DropDownWidth = 120;
            this.cboFltUse.Location = new System.Drawing.Point(1139, 17);
            this.cboFltUse.Margin = new System.Windows.Forms.Padding(4);
            this.cboFltUse.Name = "cboFltUse";
            this.cboFltUse.Size = new System.Drawing.Size(125, 26);
            this.cboFltUse.TabIndex = 18;
            this.cboFltUse.DropDown += new System.EventHandler(this.cboFlt_DropDown);
            this.cboFltUse.SelectedIndexChanged += new System.EventHandler(this.cboFlt_SelectedIndexChanged);
            // 
            // lblFltEnable
            // 
            this.lblFltEnable.AutoSize = true;
            this.lblFltEnable.Location = new System.Drawing.Point(1278, 21);
            this.lblFltEnable.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFltEnable.Name = "lblFltEnable";
            this.lblFltEnable.TabIndex = 19;
            this.lblFltEnable.Text = "Enable";
            // 
            // cboFltEnable
            // 
            this.cboFltEnable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFltEnable.DropDownWidth = 120;
            this.cboFltEnable.Location = new System.Drawing.Point(1346, 17);
            this.cboFltEnable.Margin = new System.Windows.Forms.Padding(4);
            this.cboFltEnable.Name = "cboFltEnable";
            this.cboFltEnable.Size = new System.Drawing.Size(125, 26);
            this.cboFltEnable.TabIndex = 20;
            this.cboFltEnable.DropDown += new System.EventHandler(this.cboFlt_DropDown);
            this.cboFltEnable.SelectedIndexChanged += new System.EventHandler(this.cboFlt_SelectedIndexChanged);
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
            // frmEtParaSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1857, 1080);
            this.Controls.Add(this.splitPara);
            this.Controls.Add(this.paraBar);
            this.Controls.Add(this.pnlStatus);
            this.Controls.Add(this.topBar);
            this.Margin = new System.Windows.Forms.Padding(9);
            this.Name = "frmEtParaSetup";
            this.Text = "E-Test Para 설정";
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.splitPara.Panel1.ResumeLayout(false);
            this.splitPara.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitPara)).EndInit();
            this.splitPara.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPara)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGds)).EndInit();
            this.gdsBar.ResumeLayout(false);
            this.gdsBar.PerformLayout();
            this.paraBar.ResumeLayout(false);
            this.paraBar.PerformLayout();
            this.pnlStatus.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Label lblFactory;
        private System.Windows.Forms.TextBox txtFactory;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.ComboBox cboDevice;
        private System.Windows.Forms.Label lblTestProgram;
        private System.Windows.Forms.ComboBox cboTestProgram;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.SplitContainer splitPara;
        private System.Windows.Forms.DataGridView dgvPara;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPHeader;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPMeas;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPCond;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPChain1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPChain2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPUnit;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colPUse;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPGds;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPLower;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPUpper;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colPEnable;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPRemark;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPSpecText;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPRegFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPUpdate;
        private System.Windows.Forms.DataGridView dgvGds;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGUse;
        private System.Windows.Forms.Panel gdsBar;
        private System.Windows.Forms.Label lblGdsDevice;
        private System.Windows.Forms.TextBox txtGdsDevice;
        private System.Windows.Forms.Label lblGdsInfo;
        private System.Windows.Forms.Panel paraBar;
        private System.Windows.Forms.Label lblParaFind;
        private System.Windows.Forms.TextBox txtParaFind;
        private System.Windows.Forms.CheckBox chkSpecOnly;
        private System.Windows.Forms.CheckBox chkNoGdsOnly;
        private System.Windows.Forms.Button btnPasteSpec;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClearFilter;
        private System.Windows.Forms.Label lblFltMeas;
        private System.Windows.Forms.ComboBox cboFltMeas;
        private System.Windows.Forms.Label lblFltCond;
        private System.Windows.Forms.ComboBox cboFltCond;
        private System.Windows.Forms.Label lblFltChain1;
        private System.Windows.Forms.ComboBox cboFltChain1;
        private System.Windows.Forms.Label lblFltGds;
        private System.Windows.Forms.ComboBox cboFltGds;
        private System.Windows.Forms.Label lblFltUse;
        private System.Windows.Forms.ComboBox cboFltUse;
        private System.Windows.Forms.Label lblFltEnable;
        private System.Windows.Forms.ComboBox cboFltEnable;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Label lblStatus;
        private DACrux.SEMDMS.Control.ProgressStatusBar busyBar;
    }
}
