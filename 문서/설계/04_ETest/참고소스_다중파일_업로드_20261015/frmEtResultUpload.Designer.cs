namespace DACrux.SEMDMS.ENGUI
{
    // E-Test 결과 업로드 화면의 컨트롤과 배치를 정의한다. 분석 / 저장 동작은 본문 파일(frmEtResultUpload.cs)에서 처리한다.
    // 2026-10-15 Upload / View 분리: 대기 목록 + 선택 파일 결과(공통 EtResultDataControl). 등록 파일 목록 / 조회는 E-Test 결과 조회 화면(frmEtResultView)으로 옮겼다.
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
        /// 위: Factory / [파일 추가] [폴더 추가] [목록 비우기] / [모두 저장] [선택 저장].
        /// 가운데: 대기 목록(위) + 선택 파일 결과(아래, 공통 Control: 정보 / 1단계 Die / 2단계 정의 Para 값). 아래: 상태 표시줄 + 진행 막대.
        /// </summary>
        private void InitializeComponent()
        {
            this.topBar = new System.Windows.Forms.Panel();
            this.lblFactory = new System.Windows.Forms.Label();
            this.txtFactory = new System.Windows.Forms.TextBox();
            this.btnAddFiles = new System.Windows.Forms.Button();
            this.btnAddFolder = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSaveAll = new System.Windows.Forms.Button();
            this.btnSaveSelected = new System.Windows.Forms.Button();
            this.splitUpload = new System.Windows.Forms.SplitContainer();
            this.dgvQueue = new System.Windows.Forms.DataGridView();
            this.colQFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQDevice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQProgram = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQLot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQLotWafer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQSlot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQWafer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQTestDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQDie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQHeader = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQRegistered = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQSaved = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQMissing = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQFailDie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQMap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQSeq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resultView = new DACrux.SEMDMS.ENGUI.EtResultDataControl();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.busyBar = new DACrux.SEMDMS.Control.ProgressStatusBar();
            this.topBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitUpload)).BeginInit();
            this.splitUpload.Panel1.SuspendLayout();
            this.splitUpload.Panel2.SuspendLayout();
            this.splitUpload.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueue)).BeginInit();
            this.pnlStatus.SuspendLayout();
            this.SuspendLayout();
            //
            // topBar
            //
            this.topBar.Controls.Add(this.lblFactory);
            this.topBar.Controls.Add(this.txtFactory);
            this.topBar.Controls.Add(this.btnAddFiles);
            this.topBar.Controls.Add(this.btnAddFolder);
            this.topBar.Controls.Add(this.btnClear);
            this.topBar.Controls.Add(this.btnSaveAll);
            this.topBar.Controls.Add(this.btnSaveSelected);
            this.topBar.Name = "topBar";
            this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar.Location = new System.Drawing.Point(0, 0);
            this.topBar.Size = new System.Drawing.Size(1300, 38);
            this.topBar.TabIndex = 0;
            //
            // lblFactory
            //
            this.lblFactory.Name = "lblFactory";
            this.lblFactory.AutoSize = true;
            this.lblFactory.Location = new System.Drawing.Point(10, 13);
            this.lblFactory.Size = new System.Drawing.Size(46, 12);
            this.lblFactory.TabIndex = 0;
            this.lblFactory.Text = "Factory";
            //
            // txtFactory
            //
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.Location = new System.Drawing.Point(62, 9);
            this.txtFactory.Size = new System.Drawing.Size(90, 21);
            this.txtFactory.TabIndex = 1;
            //
            // btnAddFiles
            //
            this.btnAddFiles.Name = "btnAddFiles";
            this.btnAddFiles.Location = new System.Drawing.Point(166, 8);
            this.btnAddFiles.Size = new System.Drawing.Size(96, 23);
            this.btnAddFiles.TabIndex = 2;
            this.btnAddFiles.Text = "파일 추가...";
            this.btnAddFiles.UseVisualStyleBackColor = true;
            this.btnAddFiles.Click += new System.EventHandler(this.btnAddFiles_Click);
            //
            // btnAddFolder
            //
            this.btnAddFolder.Name = "btnAddFolder";
            this.btnAddFolder.Location = new System.Drawing.Point(266, 8);
            this.btnAddFolder.Size = new System.Drawing.Size(96, 23);
            this.btnAddFolder.TabIndex = 3;
            this.btnAddFolder.Text = "폴더 추가...";
            this.btnAddFolder.UseVisualStyleBackColor = true;
            this.btnAddFolder.Click += new System.EventHandler(this.btnAddFolder_Click);
            //
            // btnClear
            //
            this.btnClear.Name = "btnClear";
            this.btnClear.Location = new System.Drawing.Point(366, 8);
            this.btnClear.Size = new System.Drawing.Size(90, 23);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "목록 비우기";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            //
            // btnSaveAll
            //
            this.btnSaveAll.Name = "btnSaveAll";
            this.btnSaveAll.Location = new System.Drawing.Point(472, 8);
            this.btnSaveAll.Size = new System.Drawing.Size(90, 23);
            this.btnSaveAll.TabIndex = 5;
            this.btnSaveAll.Text = "모두 저장";
            this.btnSaveAll.UseVisualStyleBackColor = true;
            this.btnSaveAll.Click += new System.EventHandler(this.btnSaveAll_Click);
            //
            // btnSaveSelected
            //
            this.btnSaveSelected.Name = "btnSaveSelected";
            this.btnSaveSelected.Location = new System.Drawing.Point(566, 8);
            this.btnSaveSelected.Size = new System.Drawing.Size(90, 23);
            this.btnSaveSelected.TabIndex = 6;
            this.btnSaveSelected.Text = "선택 저장";
            this.btnSaveSelected.UseVisualStyleBackColor = true;
            this.btnSaveSelected.Click += new System.EventHandler(this.btnSaveSelected_Click);
            //
            // splitUpload
            //
            this.splitUpload.Name = "splitUpload";
            this.splitUpload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitUpload.Location = new System.Drawing.Point(0, 38);
            this.splitUpload.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitUpload.Panel1.Controls.Add(this.dgvQueue);
            this.splitUpload.Panel2.Controls.Add(this.resultView);
            this.splitUpload.Size = new System.Drawing.Size(1300, 716);
            this.splitUpload.SplitterDistance = 240;
            this.splitUpload.TabIndex = 1;
            //
            // dgvQueue
            //
            this.dgvQueue.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colQFile,
            this.colQStatus,
            this.colQDevice,
            this.colQProgram,
            this.colQLot,
            this.colQLotWafer,
            this.colQSlot,
            this.colQWafer,
            this.colQTestDate,
            this.colQDie,
            this.colQHeader,
            this.colQRegistered,
            this.colQSaved,
            this.colQMissing,
            this.colQFailDie,
            this.colQMap,
            this.colQMessage,
            this.colQSeq});
            this.dgvQueue.Name = "dgvQueue";
            this.dgvQueue.AllowUserToAddRows = false;
            this.dgvQueue.AllowUserToDeleteRows = false;
            this.dgvQueue.AllowUserToResizeRows = false;
            this.dgvQueue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvQueue.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystrokeOrF2;
            this.dgvQueue.Location = new System.Drawing.Point(0, 0);
            this.dgvQueue.MultiSelect = true;
            this.dgvQueue.RowHeadersVisible = false;
            this.dgvQueue.RowTemplate.Height = 23;
            this.dgvQueue.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvQueue.Size = new System.Drawing.Size(1292, 260);
            this.dgvQueue.TabIndex = 0;
            this.dgvQueue.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvQueue_CellEndEdit);
            this.dgvQueue.SelectionChanged += new System.EventHandler(this.dgvQueue_SelectionChanged);
            this.dgvQueue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvQueue_KeyDown);
            //
            // colQFile
            //
            this.colQFile.HeaderText = "파일명";
            this.colQFile.Name = "colQFile";
            this.colQFile.ReadOnly = true;
            this.colQFile.Width = 330;
            //
            // colQStatus
            //
            this.colQStatus.HeaderText = "상태";
            this.colQStatus.Name = "colQStatus";
            this.colQStatus.ReadOnly = true;
            this.colQStatus.Width = 70;
            //
            // colQDevice
            //
            this.colQDevice.HeaderText = "Device";
            this.colQDevice.Name = "colQDevice";
            this.colQDevice.ReadOnly = true;
            this.colQDevice.Width = 70;
            //
            // colQProgram
            //
            this.colQProgram.HeaderText = "공정";
            this.colQProgram.Name = "colQProgram";
            this.colQProgram.ReadOnly = true;
            this.colQProgram.Width = 60;
            //
            // colQLot
            //
            this.colQLot.HeaderText = "Lot";
            this.colQLot.Name = "colQLot";
            this.colQLot.ReadOnly = true;
            this.colQLot.Width = 90;
            //
            // colQLotWafer
            //
            this.colQLotWafer.HeaderText = "Lot-Wafer No";
            this.colQLotWafer.Name = "colQLotWafer";
            this.colQLotWafer.ReadOnly = true;
            this.colQLotWafer.Width = 110;
            //
            // colQSlot
            //
            this.colQSlot.HeaderText = "Slot";
            this.colQSlot.MaxInputLength = 2;
            this.colQSlot.Name = "colQSlot";
            this.colQSlot.Width = 45;
            //
            // colQWafer
            //
            this.colQWafer.HeaderText = "WAFER_ID";
            this.colQWafer.Name = "colQWafer";
            this.colQWafer.ReadOnly = true;
            this.colQWafer.Width = 100;
            //
            // colQTestDate
            //
            this.colQTestDate.HeaderText = "Test Date";
            this.colQTestDate.Name = "colQTestDate";
            this.colQTestDate.ReadOnly = true;
            this.colQTestDate.Width = 120;
            //
            // colQDie
            //
            this.colQDie.HeaderText = "Die";
            this.colQDie.Name = "colQDie";
            this.colQDie.ReadOnly = true;
            this.colQDie.Width = 45;
            //
            // colQHeader
            //
            this.colQHeader.HeaderText = "헤더";
            this.colQHeader.Name = "colQHeader";
            this.colQHeader.ReadOnly = true;
            this.colQHeader.Width = 55;
            //
            // colQRegistered
            //
            this.colQRegistered.HeaderText = "등록";
            this.colQRegistered.Name = "colQRegistered";
            this.colQRegistered.ReadOnly = true;
            this.colQRegistered.Width = 45;
            //
            // colQSaved
            //
            this.colQSaved.HeaderText = "저장";
            this.colQSaved.Name = "colQSaved";
            this.colQSaved.ReadOnly = true;
            this.colQSaved.Width = 45;
            //
            // colQMissing
            //
            this.colQMissing.HeaderText = "누락";
            this.colQMissing.Name = "colQMissing";
            this.colQMissing.ReadOnly = true;
            this.colQMissing.Width = 45;
            //
            // colQFailDie
            //
            this.colQFailDie.HeaderText = "Spec Fail Die";
            this.colQFailDie.Name = "colQFailDie";
            this.colQFailDie.ReadOnly = true;
            this.colQFailDie.Width = 85;
            //
            // colQMap
            //
            this.colQMap.HeaderText = "Map";
            this.colQMap.Name = "colQMap";
            this.colQMap.ReadOnly = true;
            this.colQMap.Width = 80;
            //
            // colQMessage
            //
            this.colQMessage.HeaderText = "메시지";
            this.colQMessage.Name = "colQMessage";
            this.colQMessage.ReadOnly = true;
            this.colQMessage.Width = 380;
            //
            // colQSeq
            //
            this.colQSeq.HeaderText = "파일 번호";
            this.colQSeq.Name = "colQSeq";
            this.colQSeq.ReadOnly = true;
            this.colQSeq.Width = 70;
            //
            // resultView
            //
            this.resultView.Name = "resultView";
            this.resultView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultView.Location = new System.Drawing.Point(0, 0);
            this.resultView.Size = new System.Drawing.Size(1300, 472);
            this.resultView.TabIndex = 0;
            //
            // pnlStatus
            //
            this.pnlStatus.Controls.Add(this.lblStatus);
            this.pnlStatus.Controls.Add(this.busyBar);
            this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(1300, 26);
            this.pnlStatus.TabIndex = 2;
            //
            // busyBar
            //
            this.busyBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.busyBar.Name = "busyBar";
            this.busyBar.Size = new System.Drawing.Size(420, 26);
            this.busyBar.TabIndex = 1;
            //
            // lblStatus
            //
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Location = new System.Drawing.Point(0, 754);
            this.lblStatus.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lblStatus.Size = new System.Drawing.Size(1300, 26);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // frmEtResultUpload
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 780);
            this.Controls.Add(this.splitUpload);
            this.Controls.Add(this.pnlStatus);
            this.Controls.Add(this.topBar);
            this.Name = "frmEtResultUpload";
            this.Text = "E-Test 결과 업로드";
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.splitUpload.Panel1.ResumeLayout(false);
            this.splitUpload.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitUpload)).EndInit();
            this.splitUpload.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueue)).EndInit();
            this.pnlStatus.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Label lblFactory;
        private System.Windows.Forms.TextBox txtFactory;
        private System.Windows.Forms.Button btnAddFiles;
        private System.Windows.Forms.Button btnAddFolder;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSaveAll;
        private System.Windows.Forms.Button btnSaveSelected;
        private System.Windows.Forms.SplitContainer splitUpload;
        private System.Windows.Forms.DataGridView dgvQueue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQDevice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQProgram;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQLot;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQLotWafer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQSlot;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQWafer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQTestDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQDie;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQHeader;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQRegistered;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQSaved;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQMissing;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQFailDie;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQMap;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQMessage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQSeq;
        private DACrux.SEMDMS.ENGUI.EtResultDataControl resultView;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Label lblStatus;
        private DACrux.SEMDMS.Control.ProgressStatusBar busyBar;
    }
}
