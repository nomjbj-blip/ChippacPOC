namespace DACrux.SEMDMS.ENGUI
{
    // E-Test 결과 업로드 화면의 컨트롤과 배치를 정의한다. 분석 / 저장 / 삭제 동작은 본문 파일(frmEtResultUpload.cs)에서 처리한다. (CLAUDE-029)
    partial class frmEtResultUpload
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>화면 종료 시 디자이너 구성 요소를 해제한다.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// 위: E-Test 파일 / Factory / Device / [찾아보기] [목록 조회] [DB 저장], 그 아래: 분석 요약.
        /// 가운데 위: 파일 Die 미리보기, 가운데 아래: Device에 등록된 E-Test 파일 목록 + [선택 파일 삭제], 아래: 상태 표시줄 / 진행 막대.
        /// </summary>
        private void InitializeComponent()
        {
            this.topBar = new System.Windows.Forms.Panel();
            this.lblFile = new System.Windows.Forms.Label();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblFactory = new System.Windows.Forms.Label();
            this.txtFactory = new System.Windows.Forms.TextBox();
            this.lblDevice = new System.Windows.Forms.Label();
            this.cboDevice = new System.Windows.Forms.ComboBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblSlot = new System.Windows.Forms.Label();
            this.txtSlot = new System.Windows.Forms.TextBox();
            this.lblWafer = new System.Windows.Forms.Label();
            this.txtSummary = new System.Windows.Forms.TextBox();
            this.statusBar = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.progressSave = new System.Windows.Forms.ProgressBar();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.grpDie = new System.Windows.Forms.GroupBox();
            this.dgvDie = new System.Windows.Forms.DataGridView();
            this.grpFiles = new System.Windows.Forms.GroupBox();
            this.dgvFiles = new System.Windows.Forms.DataGridView();
            this.fileButtons = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.topBar.SuspendLayout();
            this.statusBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.grpDie.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDie)).BeginInit();
            this.grpFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).BeginInit();
            this.fileButtons.SuspendLayout();
            this.SuspendLayout();
            //
            // topBar
            //
            this.topBar.Controls.Add(this.lblFile);
            this.topBar.Controls.Add(this.txtFile);
            this.topBar.Controls.Add(this.btnBrowse);
            this.topBar.Controls.Add(this.lblFactory);
            this.topBar.Controls.Add(this.txtFactory);
            this.topBar.Controls.Add(this.lblDevice);
            this.topBar.Controls.Add(this.cboDevice);
            this.topBar.Controls.Add(this.btnRefresh);
            this.topBar.Controls.Add(this.btnSave);
            this.topBar.Controls.Add(this.lblSlot);
            this.topBar.Controls.Add(this.txtSlot);
            this.topBar.Controls.Add(this.lblWafer);
            this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar.Location = new System.Drawing.Point(0, 0);
            this.topBar.Name = "topBar";
            this.topBar.Size = new System.Drawing.Size(1100, 94);
            this.topBar.TabIndex = 0;
            //
            // lblFile
            //
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(10, 13);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(73, 12);
            this.lblFile.TabIndex = 0;
            this.lblFile.Text = "E-Test 파일";
            //
            // txtFile
            //
            this.txtFile.Location = new System.Drawing.Point(92, 9);
            this.txtFile.Name = "txtFile";
            this.txtFile.ReadOnly = true;
            this.txtFile.Size = new System.Drawing.Size(600, 21);
            this.txtFile.TabIndex = 1;
            //
            // btnBrowse
            //
            this.btnBrowse.Location = new System.Drawing.Point(698, 8);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(96, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "찾아보기...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            //
            // lblFactory
            //
            this.lblFactory.AutoSize = true;
            this.lblFactory.Location = new System.Drawing.Point(10, 42);
            this.lblFactory.Name = "lblFactory";
            this.lblFactory.Size = new System.Drawing.Size(46, 12);
            this.lblFactory.TabIndex = 3;
            this.lblFactory.Text = "Factory";
            //
            // txtFactory
            //
            this.txtFactory.Location = new System.Drawing.Point(92, 38);
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.Size = new System.Drawing.Size(110, 21);
            this.txtFactory.TabIndex = 4;
            this.txtFactory.Leave += new System.EventHandler(this.txtFactory_Leave);
            //
            // lblDevice
            //
            this.lblDevice.AutoSize = true;
            this.lblDevice.Location = new System.Drawing.Point(214, 42);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(42, 12);
            this.lblDevice.TabIndex = 5;
            this.lblDevice.Text = "Device";
            //
            // cboDevice
            //
            this.cboDevice.FormattingEnabled = true;
            this.cboDevice.Location = new System.Drawing.Point(262, 38);
            this.cboDevice.Name = "cboDevice";
            this.cboDevice.Size = new System.Drawing.Size(230, 20);
            this.cboDevice.TabIndex = 6;
            this.cboDevice.SelectedIndexChanged += new System.EventHandler(this.cboDevice_SelectedIndexChanged);
            this.cboDevice.Leave += new System.EventHandler(this.cboDevice_Leave);
            //
            // btnRefresh
            //
            this.btnRefresh.Location = new System.Drawing.Point(498, 37);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(96, 23);
            this.btnRefresh.TabIndex = 7;
            this.btnRefresh.Text = "목록 조회";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            //
            // btnSave
            //
            this.btnSave.Location = new System.Drawing.Point(698, 37);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(96, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "DB 저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // txtSummary
            //
            // lblSlot: Slot 번호는 파일에 없어 사용자가 입력한다(파일명에서 얻으면 기본값)
            this.lblSlot.AutoSize = true;
            this.lblSlot.Location = new System.Drawing.Point(10, 70);
            this.lblSlot.Name = "lblSlot";
            this.lblSlot.Size = new System.Drawing.Size(48, 12);
            this.lblSlot.TabIndex = 9;
            this.lblSlot.Text = "Slot No";
            // txtSlot
            this.txtSlot.Location = new System.Drawing.Point(92, 66);
            this.txtSlot.MaxLength = 2;
            this.txtSlot.Name = "txtSlot";
            this.txtSlot.Size = new System.Drawing.Size(50, 21);
            this.txtSlot.TabIndex = 10;
            this.txtSlot.TextChanged += new System.EventHandler(this.txtSlot_TextChanged);
            // lblWafer: 저장될 WAFER_ID 미리보기 (Lot + '-' + 2자리 Slot)
            this.lblWafer.AutoSize = true;
            this.lblWafer.Font = new System.Drawing.Font(this.Font, System.Drawing.FontStyle.Bold);
            this.lblWafer.Location = new System.Drawing.Point(152, 70);
            this.lblWafer.Name = "lblWafer";
            this.lblWafer.Size = new System.Drawing.Size(100, 12);
            this.lblWafer.TabIndex = 11;
            this.lblWafer.Text = "WAFER_ID -";
            // txtSummary
            this.txtSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSummary.Location = new System.Drawing.Point(0, 94);
            this.txtSummary.Multiline = true;
            this.txtSummary.Name = "txtSummary";
            this.txtSummary.ReadOnly = true;
            this.txtSummary.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSummary.Size = new System.Drawing.Size(1100, 96);
            this.txtSummary.TabIndex = 1;
            //
            // statusBar
            //
            this.statusBar.Controls.Add(this.lblStatus);
            this.statusBar.Controls.Add(this.progressSave);
            this.statusBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusBar.Location = new System.Drawing.Point(0, 674);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(1100, 26);
            this.statusBar.TabIndex = 3;
            //
            // lblStatus
            //
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Location = new System.Drawing.Point(0, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lblStatus.Size = new System.Drawing.Size(880, 26);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "E-Test 결과 파일(xlsx)을 선택하세요.";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // progressSave
            //
            this.progressSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.progressSave.Location = new System.Drawing.Point(880, 0);
            this.progressSave.Name = "progressSave";
            this.progressSave.Size = new System.Drawing.Size(220, 26);
            this.progressSave.TabIndex = 1;
            //
            // splitMain
            //
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 162);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            //
            // splitMain.Panel1
            //
            this.splitMain.Panel1.Controls.Add(this.grpDie);
            //
            // splitMain.Panel2
            //
            this.splitMain.Panel2.Controls.Add(this.grpFiles);
            this.splitMain.Size = new System.Drawing.Size(1100, 512);
            this.splitMain.SplitterDistance = 270;
            this.splitMain.TabIndex = 2;
            //
            // grpDie
            //
            this.grpDie.Controls.Add(this.dgvDie);
            this.grpDie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDie.Location = new System.Drawing.Point(0, 0);
            this.grpDie.Name = "grpDie";
            this.grpDie.Size = new System.Drawing.Size(1100, 270);
            this.grpDie.TabIndex = 0;
            this.grpDie.TabStop = false;
            this.grpDie.Text = "파일 Die 미리보기";
            //
            // dgvDie
            //
            this.dgvDie.AllowUserToAddRows = false;
            this.dgvDie.AllowUserToDeleteRows = false;
            this.dgvDie.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDie.Location = new System.Drawing.Point(3, 17);
            this.dgvDie.Name = "dgvDie";
            this.dgvDie.ReadOnly = true;
            this.dgvDie.RowHeadersVisible = false;
            this.dgvDie.RowTemplate.Height = 23;
            this.dgvDie.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDie.Size = new System.Drawing.Size(1094, 250);
            this.dgvDie.TabIndex = 0;
            //
            // grpFiles
            //
            this.grpFiles.Controls.Add(this.dgvFiles);
            this.grpFiles.Controls.Add(this.fileButtons);
            this.grpFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpFiles.Location = new System.Drawing.Point(0, 0);
            this.grpFiles.Name = "grpFiles";
            this.grpFiles.Size = new System.Drawing.Size(1100, 238);
            this.grpFiles.TabIndex = 0;
            this.grpFiles.TabStop = false;
            this.grpFiles.Text = "등록된 E-Test 파일 (Factory / Device 기준, 더블클릭하면 저장 데이터 조회)";
            //
            // dgvFiles
            //
            this.dgvFiles.AllowUserToAddRows = false;
            this.dgvFiles.AllowUserToDeleteRows = false;
            this.dgvFiles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFiles.Location = new System.Drawing.Point(3, 47);
            this.dgvFiles.MultiSelect = false;
            this.dgvFiles.Name = "dgvFiles";
            this.dgvFiles.ReadOnly = true;
            this.dgvFiles.RowHeadersVisible = false;
            this.dgvFiles.RowTemplate.Height = 23;
            this.dgvFiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFiles.Size = new System.Drawing.Size(1094, 188);
            this.dgvFiles.TabIndex = 1;
            this.dgvFiles.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFiles_CellDoubleClick);
            //
            // fileButtons
            //
            this.fileButtons.Controls.Add(this.btnDelete);
            this.fileButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.fileButtons.Location = new System.Drawing.Point(3, 17);
            this.fileButtons.Name = "fileButtons";
            this.fileButtons.Size = new System.Drawing.Size(1094, 30);
            this.fileButtons.TabIndex = 0;
            //
            // btnDelete
            //
            this.btnDelete.Location = new System.Drawing.Point(4, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(120, 23);
            this.btnDelete.TabIndex = 0;
            this.btnDelete.Text = "선택 파일 삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            //
            // frmEtResultUpload
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 700);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.txtSummary);
            this.Controls.Add(this.topBar);
            this.Name = "frmEtResultUpload";
            this.Text = "E-Test 결과 업로드";
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.statusBar.ResumeLayout(false);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.grpDie.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDie)).EndInit();
            this.grpFiles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).EndInit();
            this.fileButtons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblFactory;
        private System.Windows.Forms.TextBox txtFactory;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.ComboBox cboDevice;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblSlot;
        private System.Windows.Forms.TextBox txtSlot;
        private System.Windows.Forms.Label lblWafer;
        private System.Windows.Forms.TextBox txtSummary;
        private System.Windows.Forms.Panel statusBar;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressSave;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.GroupBox grpDie;
        private System.Windows.Forms.DataGridView dgvDie;
        private System.Windows.Forms.GroupBox grpFiles;
        private System.Windows.Forms.DataGridView dgvFiles;
        private System.Windows.Forms.Panel fileButtons;
        private System.Windows.Forms.Button btnDelete;
    }
}
