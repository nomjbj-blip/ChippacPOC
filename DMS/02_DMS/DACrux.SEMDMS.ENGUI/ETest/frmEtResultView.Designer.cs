namespace DACrux.SEMDMS.ENGUI
{
    // E-Test 결과 조회 화면의 컨트롤과 배치를 정의한다. 조회 / 삭제 동작은 본문 파일(frmEtResultView.cs)에서 처리한다.
    // 2026-10-15 Upload / View 분리: 업로드 화면의 [등록 파일] 탭과 팝업 조회 창을 이 메뉴 화면 하나로 합쳤다.
    partial class frmEtResultView
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>화면 종료 시 디자이너 구성 요소를 해제한다.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// 위: Factory / Device / 공정 / Lot / [조회] [선택 파일 삭제]. ([Map 좌표 다시 계산] 은 2026-10-15 삭제)
        /// 가운데: 저장된 파일 목록(위) + 선택 파일 결과(아래, 공통 Control: 정보 / 1단계 Die / 2단계 정의 Para 값). 아래: 상태 표시줄 + 진행 막대.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEtResultView));
            this.topBar = new System.Windows.Forms.Panel();
            this.lblFactory = new System.Windows.Forms.Label();
            this.txtFactory = new System.Windows.Forms.TextBox();
            this.lblDevice = new System.Windows.Forms.Label();
            this.cboDevice = new System.Windows.Forms.ComboBox();
            this.lblProgram = new System.Windows.Forms.Label();
            this.cboProgram = new System.Windows.Forms.ComboBox();
            this.lblLot = new System.Windows.Forms.Label();
            this.txtLot = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.splitView = new System.Windows.Forms.SplitContainer();
            this.dgvFiles = new System.Windows.Forms.DataGridView();
            this.resultView = new DACrux.SEMDMS.ENGUI.EtResultDataControl();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.busyBar = new DACrux.SEMDMS.Control.ProgressStatusBar();
            this.topBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitView)).BeginInit();
            this.splitView.Panel1.SuspendLayout();
            this.splitView.Panel2.SuspendLayout();
            this.splitView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).BeginInit();
            this.pnlStatus.SuspendLayout();
            this.SuspendLayout();
            //
            // topBar
            //
            this.topBar.Controls.Add(this.lblFactory);
            this.topBar.Controls.Add(this.txtFactory);
            this.topBar.Controls.Add(this.lblDevice);
            this.topBar.Controls.Add(this.cboDevice);
            this.topBar.Controls.Add(this.lblProgram);
            this.topBar.Controls.Add(this.cboProgram);
            this.topBar.Controls.Add(this.lblLot);
            this.topBar.Controls.Add(this.txtLot);
            this.topBar.Controls.Add(this.btnSearch);
            this.topBar.Controls.Add(this.btnDelete);
            this.topBar.Name = "topBar";
            this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar.Location = new System.Drawing.Point(0, 0);
            this.topBar.Size = new System.Drawing.Size(1300, 40);
            this.topBar.TabIndex = 0;
            //
            // lblFactory
            //
            this.lblFactory.Name = "lblFactory";
            this.lblFactory.AutoSize = true;
            this.lblFactory.Location = new System.Drawing.Point(10, 14);
            this.lblFactory.Size = new System.Drawing.Size(46, 12);
            this.lblFactory.TabIndex = 0;
            this.lblFactory.Text = "Factory";
            //
            // txtFactory
            //
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.Location = new System.Drawing.Point(62, 10);
            this.txtFactory.Size = new System.Drawing.Size(80, 21);
            this.txtFactory.TabIndex = 1;
            this.txtFactory.Leave += new System.EventHandler(this.txtFactory_Leave);
            //
            // lblDevice
            //
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.AutoSize = true;
            this.lblDevice.Location = new System.Drawing.Point(154, 14);
            this.lblDevice.Size = new System.Drawing.Size(42, 12);
            this.lblDevice.TabIndex = 2;
            this.lblDevice.Text = "Device";
            //
            // cboDevice
            //
            this.cboDevice.Name = "cboDevice";
            this.cboDevice.FormattingEnabled = true;
            this.cboDevice.Location = new System.Drawing.Point(202, 10);
            this.cboDevice.Size = new System.Drawing.Size(150, 20);
            this.cboDevice.TabIndex = 3;
            this.cboDevice.SelectedIndexChanged += new System.EventHandler(this.cboDevice_SelectedIndexChanged);
            //
            // lblProgram
            //
            this.lblProgram.Name = "lblProgram";
            this.lblProgram.AutoSize = true;
            this.lblProgram.Location = new System.Drawing.Point(364, 14);
            this.lblProgram.Size = new System.Drawing.Size(29, 12);
            this.lblProgram.TabIndex = 4;
            this.lblProgram.Text = "공정";
            //
            // cboProgram
            //
            this.cboProgram.Name = "cboProgram";
            this.cboProgram.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProgram.FormattingEnabled = true;
            this.cboProgram.Location = new System.Drawing.Point(398, 10);
            this.cboProgram.Size = new System.Drawing.Size(90, 20);
            this.cboProgram.TabIndex = 5;
            //
            // lblLot
            //
            this.lblLot.Name = "lblLot";
            this.lblLot.AutoSize = true;
            this.lblLot.Location = new System.Drawing.Point(500, 14);
            this.lblLot.Size = new System.Drawing.Size(21, 12);
            this.lblLot.TabIndex = 6;
            this.lblLot.Text = "Lot";
            //
            // txtLot
            //
            this.txtLot.Name = "txtLot";
            this.txtLot.Location = new System.Drawing.Point(526, 10);
            this.txtLot.Size = new System.Drawing.Size(120, 21);
            this.txtLot.TabIndex = 7;
            this.txtLot.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLot_KeyDown);
            //
            // btnSearch
            //
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.Location = new System.Drawing.Point(658, 6);
            this.btnSearch.Size = new System.Drawing.Size(88, 28);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            //
            // btnDelete
            //
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.Location = new System.Drawing.Point(1164, 6);
            this.btnDelete.Size = new System.Drawing.Size(124, 28);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "선택 파일 삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            //
            // splitView
            //
            this.splitView.Name = "splitView";
            this.splitView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitView.Location = new System.Drawing.Point(0, 40);
            this.splitView.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitView.Panel1.Controls.Add(this.dgvFiles);
            this.splitView.Panel2.Controls.Add(this.resultView);
            this.splitView.Size = new System.Drawing.Size(1300, 716);
            this.splitView.SplitterDistance = 220;
            this.splitView.TabIndex = 1;
            //
            // dgvFiles
            //
            this.dgvFiles.Name = "dgvFiles";
            this.dgvFiles.AllowUserToAddRows = false;
            this.dgvFiles.AllowUserToDeleteRows = false;
            this.dgvFiles.AllowUserToResizeRows = false;
            this.dgvFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFiles.Location = new System.Drawing.Point(0, 0);
            this.dgvFiles.MultiSelect = false;
            this.dgvFiles.ReadOnly = true;
            this.dgvFiles.RowHeadersVisible = false;
            this.dgvFiles.RowTemplate.Height = 21;
            this.dgvFiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFiles.Size = new System.Drawing.Size(1300, 220);
            this.dgvFiles.TabIndex = 0;
            this.dgvFiles.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvFiles_CellFormatting);
            this.dgvFiles.SelectionChanged += new System.EventHandler(this.dgvFiles_SelectionChanged);
            //
            // resultView
            //
            this.resultView.Name = "resultView";
            this.resultView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultView.Location = new System.Drawing.Point(0, 0);
            this.resultView.Size = new System.Drawing.Size(1300, 492);
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
            this.lblStatus.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lblStatus.Size = new System.Drawing.Size(880, 26);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // frmEtResultView
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 780);
            this.Controls.Add(this.splitView);
            this.Controls.Add(this.pnlStatus);
            this.Controls.Add(this.topBar);
            this.Name = "frmEtResultView";
            this.Text = "E-Test 결과 조회";
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.splitView.Panel1.ResumeLayout(false);
            this.splitView.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitView)).EndInit();
            this.splitView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).EndInit();
            this.pnlStatus.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Label lblFactory;
        private System.Windows.Forms.TextBox txtFactory;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.ComboBox cboDevice;
        private System.Windows.Forms.Label lblProgram;
        private System.Windows.Forms.ComboBox cboProgram;
        private System.Windows.Forms.Label lblLot;
        private System.Windows.Forms.TextBox txtLot;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.SplitContainer splitView;
        private System.Windows.Forms.DataGridView dgvFiles;
        private DACrux.SEMDMS.ENGUI.EtResultDataControl resultView;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Label lblStatus;
        private DACrux.SEMDMS.Control.ProgressStatusBar busyBar;
    }
}
