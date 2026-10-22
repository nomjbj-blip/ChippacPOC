namespace DACrux.SEMDMS.ENGUI
{
    // E-Test 결과 업로드 화면의 컨트롤과 배치를 정의한다. 분석 / 저장 동작은 본문 파일(frmEtResultUpload.cs)에서 처리한다.
    // 2026-10-15 Upload / View 분리 + 파일 하나씩 올리기: 위 조건 / 상태 줄 + 연 파일의 결과(공통 EtResultDataControl). 저장된 파일 조회는 E-Test 결과 조회 화면(frmEtResultView).
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
        /// 위 1줄: Factory / [파일 열기] [다시 읽기] / 파일 경로(읽기 전용) / Slot(입력) / [저장].
        /// 위 2줄 상태 + 메시지(lblState / lblMessage)는 숨기고 위 막대를 1줄 높이로 줄였다 (2026-10-22).
        /// 가운데: 연 파일의 결과(공통 Control: 정보 / 1단계 Die / 2단계 정의 Para 값). 아래: 상태 표시줄 + 진행 막대.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEtResultUpload));
            this.topBar = new System.Windows.Forms.Panel();
            this.lblFactory = new System.Windows.Forms.Label();
            this.txtFactory = new System.Windows.Forms.TextBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.lblFile = new System.Windows.Forms.Label();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.lblSlot = new System.Windows.Forms.Label();
            this.txtSlot = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblState = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.resultView = new DACrux.SEMDMS.ENGUI.EtResultDataControl();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.busyBar = new DACrux.SEMDMS.Control.ProgressStatusBar();
            this.topBar.SuspendLayout();
            this.pnlStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // topBar
            // 
            this.topBar.Controls.Add(this.lblFactory);
            this.topBar.Controls.Add(this.txtFactory);
            this.topBar.Controls.Add(this.btnOpen);
            this.topBar.Controls.Add(this.btnReload);
            this.topBar.Controls.Add(this.lblFile);
            this.topBar.Controls.Add(this.txtFile);
            this.topBar.Controls.Add(this.lblSlot);
            this.topBar.Controls.Add(this.txtSlot);
            this.topBar.Controls.Add(this.btnSave);
            this.topBar.Controls.Add(this.lblState);
            this.topBar.Controls.Add(this.lblMessage);
            this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar.Location = new System.Drawing.Point(0, 0);
            this.topBar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.txtFactory.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.Size = new System.Drawing.Size(127, 28);
            this.txtFactory.TabIndex = 1;
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(230, 9);
            this.btnOpen.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnOpen.Image")));
            this.btnOpen.Size = new System.Drawing.Size(154, 42);
            this.btnOpen.TabIndex = 2;
            this.btnOpen.Text = "파일 열기...";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnReload
            // 
            this.btnReload.Location = new System.Drawing.Point(393, 9);
            this.btnReload.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnReload.Name = "btnReload";
            this.btnReload.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnReload.Image = ((System.Drawing.Image)(resources.GetObject("btnReload.Image")));
            this.btnReload.Size = new System.Drawing.Size(137, 42);
            this.btnReload.TabIndex = 3;
            this.btnReload.Text = "다시 읽기";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(548, 21);
            this.lblFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(44, 18);
            this.lblFile.TabIndex = 4;
            this.lblFile.Text = "파일";
            // 
            // txtFile
            // 
            this.txtFile.Location = new System.Drawing.Point(597, 16);
            this.txtFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtFile.Name = "txtFile";
            this.txtFile.ReadOnly = true;
            this.txtFile.Size = new System.Drawing.Size(764, 28);
            this.txtFile.TabIndex = 5;
            this.txtFile.TabStop = false;
            // 
            // lblSlot
            // 
            this.lblSlot.AutoSize = true;
            this.lblSlot.Location = new System.Drawing.Point(1380, 21);
            this.lblSlot.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSlot.Name = "lblSlot";
            this.lblSlot.Size = new System.Drawing.Size(38, 18);
            this.lblSlot.TabIndex = 6;
            this.lblSlot.Text = "Slot";
            this.lblSlot.Visible = false;
            // 
            // txtSlot
            // 
            this.txtSlot.Location = new System.Drawing.Point(1426, 16);
            this.txtSlot.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSlot.MaxLength = 2;
            this.txtSlot.Name = "txtSlot";
            this.txtSlot.Size = new System.Drawing.Size(55, 28);
            this.txtSlot.TabIndex = 7;
            this.txtSlot.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSlot.Visible = false;
            this.txtSlot.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSlot_KeyDown);
            this.txtSlot.Leave += new System.EventHandler(this.txtSlot_Leave);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(1717, 9);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Size = new System.Drawing.Size(126, 42);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblState
            // 
            this.lblState.Location = new System.Drawing.Point(14, 64);
            this.lblState.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(91, 27);
            this.lblState.TabIndex = 9;
            this.lblState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblState.Visible = false;
            // 
            // lblMessage
            // 
            this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMessage.AutoEllipsis = true;
            this.lblMessage.Location = new System.Drawing.Point(111, 64);
            this.lblMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(1731, 27);
            this.lblMessage.TabIndex = 10;
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblMessage.Visible = false;
            // 
            // resultView
            // 
            this.resultView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultView.Location = new System.Drawing.Point(0, 60);
            this.resultView.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.resultView.Name = "resultView";
            this.resultView.Size = new System.Drawing.Size(1857, 1071);
            this.resultView.TabIndex = 1;
            // 
            // pnlStatus
            // 
            this.pnlStatus.Controls.Add(this.lblStatus);
            this.pnlStatus.Controls.Add(this.busyBar);
            this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlStatus.Location = new System.Drawing.Point(0, 1131);
            this.pnlStatus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(1857, 39);
            this.pnlStatus.TabIndex = 2;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoEllipsis = true;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Location = new System.Drawing.Point(0, 0);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.lblStatus.Size = new System.Drawing.Size(1257, 39);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // busyBar
            // 
            this.busyBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.busyBar.Location = new System.Drawing.Point(1257, 0);
            this.busyBar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.busyBar.Name = "busyBar";
            this.busyBar.Padding = new System.Windows.Forms.Padding(0, 4, 9, 4);
            this.busyBar.Size = new System.Drawing.Size(600, 39);
            this.busyBar.TabIndex = 1;
            // 
            // frmEtResultUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1857, 1170);
            this.Controls.Add(this.resultView);
            this.Controls.Add(this.pnlStatus);
            this.Controls.Add(this.topBar);
            this.Margin = new System.Windows.Forms.Padding(9, 9, 9, 9);
            this.Name = "frmEtResultUpload";
            this.Text = "E-Test 결과 업로드";
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.pnlStatus.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Label lblFactory;
        private System.Windows.Forms.TextBox txtFactory;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Label lblSlot;
        private System.Windows.Forms.TextBox txtSlot;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Label lblMessage;
        private DACrux.SEMDMS.ENGUI.EtResultDataControl resultView;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Label lblStatus;
        private DACrux.SEMDMS.Control.ProgressStatusBar busyBar;
    }
}
