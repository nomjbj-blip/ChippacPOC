namespace DACrux.SEMDMS.ENGUI
{
    // E-Test Bin 설정 화면의 컨트롤과 배치를 정의한다. 조회 / 편집 / 저장 / 삭제 동작은 본문 파일(frmEtBinSetup.cs)에서 처리한다. (2026-10-22)
    partial class frmEtBinSetup
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>화면 종료 시 디자이너 구성 요소를 해제한다.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// 위 막대 1: Factory / Device / 공정 / YMS Program / [조회] ... [저장](오른쪽 끝).
        /// 위 막대 2: [Bin 추가] / [선택 삭제] / 편집 안내.
        /// 가운데: Bin 표(정의 + 파일 Bin 분포). 아래: 상태 표시줄 + 진행 막대.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEtBinSetup));
            this.topBar = new System.Windows.Forms.Panel();
            this.lblFactory = new System.Windows.Forms.Label();
            this.txtFactory = new System.Windows.Forms.TextBox();
            this.lblDevice = new System.Windows.Forms.Label();
            this.cboDevice = new System.Windows.Forms.ComboBox();
            this.lblTestProgram = new System.Windows.Forms.Label();
            this.cboTestProgram = new System.Windows.Forms.ComboBox();
            this.lblYmsProgram = new System.Windows.Forms.Label();
            this.txtYmsProgram = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.editBar = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lblHint = new System.Windows.Forms.Label();
            this.dgvBin = new System.Windows.Forms.DataGridView();
            this.colBin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGood = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoft = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFiles = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.busyBar = new DACrux.SEMDMS.Control.ProgressStatusBar();
            this.topBar.SuspendLayout();
            this.editBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBin)).BeginInit();
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
            this.topBar.Controls.Add(this.lblYmsProgram);
            this.topBar.Controls.Add(this.txtYmsProgram);
            this.topBar.Controls.Add(this.btnSearch);
            this.topBar.Controls.Add(this.btnSave);
            this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar.Location = new System.Drawing.Point(0, 0);
            this.topBar.Name = "topBar";
            this.topBar.Size = new System.Drawing.Size(1100, 40);
            this.topBar.TabIndex = 0;
            //
            // lblFactory
            //
            this.lblFactory.AutoSize = true;
            this.lblFactory.Location = new System.Drawing.Point(10, 14);
            this.lblFactory.Name = "lblFactory";
            this.lblFactory.Size = new System.Drawing.Size(46, 12);
            this.lblFactory.TabIndex = 0;
            this.lblFactory.Text = "Factory";
            //
            // txtFactory
            //
            this.txtFactory.Location = new System.Drawing.Point(62, 10);
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.ReadOnly = true;
            this.txtFactory.Size = new System.Drawing.Size(80, 21);
            this.txtFactory.TabIndex = 1;
            this.txtFactory.TabStop = false;
            //
            // lblDevice
            //
            this.lblDevice.AutoSize = true;
            this.lblDevice.Location = new System.Drawing.Point(154, 14);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(42, 12);
            this.lblDevice.TabIndex = 2;
            this.lblDevice.Text = "Device";
            //
            // cboDevice
            //
            this.cboDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDevice.FormattingEnabled = true;
            this.cboDevice.Location = new System.Drawing.Point(202, 10);
            this.cboDevice.Name = "cboDevice";
            this.cboDevice.Size = new System.Drawing.Size(150, 20);
            this.cboDevice.TabIndex = 3;
            this.cboDevice.SelectedIndexChanged += new System.EventHandler(this.cboDevice_SelectedIndexChanged);
            //
            // lblTestProgram
            //
            this.lblTestProgram.AutoSize = true;
            this.lblTestProgram.Location = new System.Drawing.Point(364, 14);
            this.lblTestProgram.Name = "lblTestProgram";
            this.lblTestProgram.Size = new System.Drawing.Size(29, 12);
            this.lblTestProgram.TabIndex = 4;
            this.lblTestProgram.Text = "공정";
            //
            // cboTestProgram
            //
            this.cboTestProgram.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTestProgram.FormattingEnabled = true;
            this.cboTestProgram.Location = new System.Drawing.Point(398, 10);
            this.cboTestProgram.Name = "cboTestProgram";
            this.cboTestProgram.Size = new System.Drawing.Size(90, 20);
            this.cboTestProgram.TabIndex = 5;
            this.cboTestProgram.SelectedIndexChanged += new System.EventHandler(this.cboTestProgram_SelectedIndexChanged);
            //
            // lblYmsProgram
            //
            this.lblYmsProgram.AutoSize = true;
            this.lblYmsProgram.Location = new System.Drawing.Point(500, 14);
            this.lblYmsProgram.Name = "lblYmsProgram";
            this.lblYmsProgram.Size = new System.Drawing.Size(77, 12);
            this.lblYmsProgram.TabIndex = 6;
            this.lblYmsProgram.Text = "YMS Program";
            //
            // txtYmsProgram
            //
            this.txtYmsProgram.Location = new System.Drawing.Point(584, 10);
            this.txtYmsProgram.Name = "txtYmsProgram";
            this.txtYmsProgram.ReadOnly = true;
            this.txtYmsProgram.Size = new System.Drawing.Size(180, 21);
            this.txtYmsProgram.TabIndex = 7;
            this.txtYmsProgram.TabStop = false;
            //
            // btnSearch
            //
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.Location = new System.Drawing.Point(776, 6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(88, 28);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "조회";
            this.btnSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            //
            // btnSave
            //
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(1000, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 28);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "저장";
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // editBar
            //
            this.editBar.Controls.Add(this.btnAdd);
            this.editBar.Controls.Add(this.btnDelete);
            this.editBar.Controls.Add(this.lblHint);
            this.editBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.editBar.Location = new System.Drawing.Point(0, 40);
            this.editBar.Name = "editBar";
            this.editBar.Size = new System.Drawing.Size(1100, 40);
            this.editBar.TabIndex = 1;
            //
            // btnAdd
            //
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.Location = new System.Drawing.Point(10, 6);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(96, 28);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Bin 추가";
            this.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            //
            // btnDelete
            //
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.Location = new System.Drawing.Point(130, 6);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(104, 28);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "선택 삭제";
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            //
            // lblHint
            //
            this.lblHint.AutoSize = true;
            this.lblHint.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblHint.Location = new System.Drawing.Point(252, 14);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(400, 12);
            this.lblHint.TabIndex = 2;
            this.lblHint.Text = "이름 / 색 / 설명 = 더블클릭, 양품 = 클릭. 양품 Bin 의 Die 가 YMS 수율에 들어갑니다.";
            //
            // dgvBin
            //
            this.dgvBin.AllowUserToAddRows = false;
            this.dgvBin.AllowUserToDeleteRows = false;
            this.dgvBin.AllowUserToResizeRows = false;
            this.dgvBin.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvBin.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dgvBin.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBin.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBin,
            this.colName,
            this.colGood,
            this.colColor,
            this.colDesc,
            this.colSoft,
            this.colDie,
            this.colPass,
            this.colFail,
            this.colFiles,
            this.colState});
            this.dgvBin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBin.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvBin.Location = new System.Drawing.Point(0, 80);
            this.dgvBin.Name = "dgvBin";
            this.dgvBin.RowHeadersWidth = 28;
            this.dgvBin.RowTemplate.Height = 21;
            this.dgvBin.Size = new System.Drawing.Size(1100, 534);
            this.dgvBin.TabIndex = 2;
            this.dgvBin.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBin_CellContentClick);
            this.dgvBin.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBin_CellDoubleClick);
            this.dgvBin.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBin_CellEndEdit);
            this.dgvBin.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvBin_CellMouseDown);
            this.dgvBin.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBin_CellValueChanged);
            this.dgvBin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvBin_KeyDown);
            //
            // colBin
            //
            this.colBin.HeaderText = "Bin";
            this.colBin.Name = "colBin";
            this.colBin.Width = 56;
            //
            // colName
            //
            this.colName.HeaderText = "이름";
            this.colName.Name = "colName";
            this.colName.Width = 150;
            //
            // colGood
            //
            this.colGood.HeaderText = "양품";
            this.colGood.Name = "colGood";
            this.colGood.ReadOnly = true;
            this.colGood.Width = 44;
            //
            // colColor
            //
            this.colColor.HeaderText = "색";
            this.colColor.Name = "colColor";
            this.colColor.ReadOnly = true;
            this.colColor.Width = 80;
            //
            // colDesc
            //
            this.colDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDesc.HeaderText = "설명";
            this.colDesc.MinimumWidth = 200;
            this.colDesc.Name = "colDesc";
            //
            // colSoft
            //
            this.colSoft.HeaderText = "Software Bin";
            this.colSoft.Name = "colSoft";
            this.colSoft.ReadOnly = true;
            this.colSoft.Width = 110;
            //
            // colDie
            //
            this.colDie.HeaderText = "Die";
            this.colDie.Name = "colDie";
            this.colDie.ReadOnly = true;
            this.colDie.Width = 56;
            //
            // colPass
            //
            this.colPass.HeaderText = "Pass";
            this.colPass.Name = "colPass";
            this.colPass.ReadOnly = true;
            this.colPass.Width = 56;
            //
            // colFail
            //
            this.colFail.HeaderText = "Fail";
            this.colFail.Name = "colFail";
            this.colFail.ReadOnly = true;
            this.colFail.Width = 56;
            //
            // colFiles
            //
            this.colFiles.HeaderText = "파일";
            this.colFiles.Name = "colFiles";
            this.colFiles.ReadOnly = true;
            this.colFiles.Width = 56;
            //
            // colState
            //
            this.colState.HeaderText = "상태";
            this.colState.Name = "colState";
            this.colState.ReadOnly = true;
            this.colState.Width = 140;
            //
            // pnlStatus
            //
            this.pnlStatus.Controls.Add(this.lblStatus);
            this.pnlStatus.Controls.Add(this.busyBar);
            this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlStatus.Location = new System.Drawing.Point(0, 614);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(1100, 26);
            this.pnlStatus.TabIndex = 3;
            //
            // lblStatus
            //
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Location = new System.Drawing.Point(0, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lblStatus.Size = new System.Drawing.Size(680, 26);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // busyBar
            //
            this.busyBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.busyBar.Location = new System.Drawing.Point(680, 0);
            this.busyBar.Name = "busyBar";
            this.busyBar.Size = new System.Drawing.Size(420, 26);
            this.busyBar.TabIndex = 1;
            //
            // frmEtBinSetup
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 640);
            this.Controls.Add(this.dgvBin);
            this.Controls.Add(this.pnlStatus);
            this.Controls.Add(this.editBar);
            this.Controls.Add(this.topBar);
            this.Name = "frmEtBinSetup";
            this.Text = "E-Test Bin 설정";
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.editBar.ResumeLayout(false);
            this.editBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBin)).EndInit();
            this.pnlStatus.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Label lblFactory;
        private System.Windows.Forms.TextBox txtFactory;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.ComboBox cboDevice;
        private System.Windows.Forms.Label lblTestProgram;
        private System.Windows.Forms.ComboBox cboTestProgram;
        private System.Windows.Forms.Label lblYmsProgram;
        private System.Windows.Forms.TextBox txtYmsProgram;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel editBar;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.DataGridView dgvBin;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBin;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colGood;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoft;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDie;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPass;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFiles;
        private System.Windows.Forms.DataGridViewTextBoxColumn colState;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Label lblStatus;
        private DACrux.SEMDMS.Control.ProgressStatusBar busyBar;
    }
}
