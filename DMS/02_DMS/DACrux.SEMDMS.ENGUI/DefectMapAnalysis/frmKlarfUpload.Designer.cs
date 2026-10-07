namespace DACrux.SEMDMS.ENGUI
{
    // KLARF 업로드 화면의 컨트롤과 배치를 정의한다. 분석 / 저장 동작은 본문 파일(frmKlarfUpload.cs)에서 처리한다.
    partial class frmKlarfUpload
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>화면 종료 시 디자이너 구성 요소를 해제한다.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// 위: KLARF 파일 / 이미지 폴더 / 저장 값(Product / Step ID / 설비 / 분류 기준) / [분석] [DB 저장]
        /// 오른쪽: Wafer 목록(상태 / STEP_SEQ) + 처리 로그, 가운데: 미리보기 Map(기존 DefectMap 기반 KlarfDefectMap), 아래: 상태 표시줄.
        /// </summary>
        private void InitializeComponent()
        {
            this.topBar = new System.Windows.Forms.Panel();
            this.lblFile = new System.Windows.Forms.Label();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnAnalyze = new System.Windows.Forms.Button();
            this.lblImageDir = new System.Windows.Forms.Label();
            this.txtImageDir = new System.Windows.Forms.TextBox();
            this.btnImageDir = new System.Windows.Forms.Button();
            this.lblProduct = new System.Windows.Forms.Label();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.lblStepId = new System.Windows.Forms.Label();
            this.txtStepId = new System.Windows.Forms.TextBox();
            this.lblEquip = new System.Windows.Forms.Label();
            this.txtEquip = new System.Windows.Forms.TextBox();
            this.lblClassSource = new System.Windows.Forms.Label();
            this.cmbClassSource = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.rightPanel = new System.Windows.Forms.Panel();
            this.dgvWafer = new System.Windows.Forms.DataGridView();
            this.colRawWaferId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWaferId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSlot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDefects = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDies = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colImages = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStepSeq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblWaferList = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.lblLog = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.m_dMap = new DACrux.SEMDMS.ENGUI.KlarfDefectMap();
            this.topBar.SuspendLayout();
            this.rightPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWafer)).BeginInit();
            this.SuspendLayout();
            //
            // topBar
            //
            this.topBar.Controls.Add(this.lblFile);
            this.topBar.Controls.Add(this.txtFile);
            this.topBar.Controls.Add(this.btnBrowse);
            this.topBar.Controls.Add(this.btnAnalyze);
            this.topBar.Controls.Add(this.lblImageDir);
            this.topBar.Controls.Add(this.txtImageDir);
            this.topBar.Controls.Add(this.btnImageDir);
            this.topBar.Controls.Add(this.lblProduct);
            this.topBar.Controls.Add(this.txtProduct);
            this.topBar.Controls.Add(this.lblStepId);
            this.topBar.Controls.Add(this.txtStepId);
            this.topBar.Controls.Add(this.lblEquip);
            this.topBar.Controls.Add(this.txtEquip);
            this.topBar.Controls.Add(this.lblClassSource);
            this.topBar.Controls.Add(this.cmbClassSource);
            this.topBar.Controls.Add(this.btnSave);
            this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar.Location = new System.Drawing.Point(0, 0);
            this.topBar.Name = "topBar";
            this.topBar.Size = new System.Drawing.Size(1400, 118);
            this.topBar.TabIndex = 0;
            //
            // lblFile
            //
            this.lblFile.Location = new System.Drawing.Point(12, 12);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(86, 23);
            this.lblFile.TabIndex = 0;
            this.lblFile.Text = "KLARF 파일";
            this.lblFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtFile
            //
            this.txtFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFile.Location = new System.Drawing.Point(100, 12);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(1064, 23);
            this.txtFile.TabIndex = 1;
            //
            // btnBrowse
            //
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(1174, 9);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(102, 30);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "파일 선택";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            //
            // btnAnalyze
            //
            this.btnAnalyze.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAnalyze.Location = new System.Drawing.Point(1284, 9);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Size = new System.Drawing.Size(102, 30);
            this.btnAnalyze.TabIndex = 3;
            this.btnAnalyze.Text = "분석";
            this.btnAnalyze.Click += new System.EventHandler(this.btnAnalyze_Click);
            //
            // lblImageDir
            //
            this.lblImageDir.Location = new System.Drawing.Point(12, 46);
            this.lblImageDir.Name = "lblImageDir";
            this.lblImageDir.Size = new System.Drawing.Size(86, 23);
            this.lblImageDir.TabIndex = 4;
            this.lblImageDir.Text = "이미지 폴더";
            this.lblImageDir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtImageDir
            //
            this.txtImageDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtImageDir.Location = new System.Drawing.Point(100, 46);
            this.txtImageDir.Name = "txtImageDir";
            this.txtImageDir.Size = new System.Drawing.Size(1064, 23);
            this.txtImageDir.TabIndex = 5;
            //
            // btnImageDir
            //
            this.btnImageDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImageDir.Location = new System.Drawing.Point(1174, 43);
            this.btnImageDir.Name = "btnImageDir";
            this.btnImageDir.Size = new System.Drawing.Size(102, 30);
            this.btnImageDir.TabIndex = 6;
            this.btnImageDir.Text = "폴더 선택";
            this.btnImageDir.Click += new System.EventHandler(this.btnImageDir_Click);
            //
            // lblProduct
            //
            this.lblProduct.Location = new System.Drawing.Point(12, 82);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(86, 23);
            this.lblProduct.TabIndex = 7;
            this.lblProduct.Text = "Product";
            this.lblProduct.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtProduct
            //
            this.txtProduct.Location = new System.Drawing.Point(100, 82);
            this.txtProduct.MaxLength = 40;
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Size = new System.Drawing.Size(180, 23);
            this.txtProduct.TabIndex = 8;
            //
            // lblStepId
            //
            this.lblStepId.Location = new System.Drawing.Point(296, 82);
            this.lblStepId.Name = "lblStepId";
            this.lblStepId.Size = new System.Drawing.Size(58, 23);
            this.lblStepId.TabIndex = 9;
            this.lblStepId.Text = "Step ID";
            this.lblStepId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtStepId
            //
            this.txtStepId.Location = new System.Drawing.Point(356, 82);
            this.txtStepId.MaxLength = 40;
            this.txtStepId.Name = "txtStepId";
            this.txtStepId.Size = new System.Drawing.Size(240, 23);
            this.txtStepId.TabIndex = 10;
            //
            // lblEquip
            //
            this.lblEquip.Location = new System.Drawing.Point(612, 82);
            this.lblEquip.Name = "lblEquip";
            this.lblEquip.Size = new System.Drawing.Size(40, 23);
            this.lblEquip.TabIndex = 11;
            this.lblEquip.Text = "설비";
            this.lblEquip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtEquip
            //
            this.txtEquip.Location = new System.Drawing.Point(654, 82);
            this.txtEquip.MaxLength = 40;
            this.txtEquip.Name = "txtEquip";
            this.txtEquip.Size = new System.Drawing.Size(140, 23);
            this.txtEquip.TabIndex = 12;
            //
            // lblClassSource
            //
            this.lblClassSource.Location = new System.Drawing.Point(810, 82);
            this.lblClassSource.Name = "lblClassSource";
            this.lblClassSource.Size = new System.Drawing.Size(70, 23);
            this.lblClassSource.TabIndex = 13;
            this.lblClassSource.Text = "분류 기준";
            this.lblClassSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // cmbClassSource
            //
            this.cmbClassSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClassSource.Items.AddRange(new object[] {
            "RoughBin",
            "FineBin",
            "Class"});
            this.cmbClassSource.Location = new System.Drawing.Point(882, 82);
            this.cmbClassSource.Name = "cmbClassSource";
            this.cmbClassSource.Size = new System.Drawing.Size(120, 23);
            this.cmbClassSource.TabIndex = 14;
            this.cmbClassSource.SelectedIndexChanged += new System.EventHandler(this.cmbClassSource_SelectedIndexChanged);
            //
            // btnSave
            //
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(1284, 78);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(102, 30);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "DB 저장";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // rightPanel
            //
            this.rightPanel.Controls.Add(this.txtLog);
            this.rightPanel.Controls.Add(this.lblLog);
            this.rightPanel.Controls.Add(this.dgvWafer);
            this.rightPanel.Controls.Add(this.lblWaferList);
            this.rightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightPanel.Location = new System.Drawing.Point(740, 118);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Padding = new System.Windows.Forms.Padding(6);
            this.rightPanel.Size = new System.Drawing.Size(660, 744);
            this.rightPanel.TabIndex = 1;
            //
            // lblWaferList
            //
            this.lblWaferList.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblWaferList.Location = new System.Drawing.Point(6, 6);
            this.lblWaferList.Name = "lblWaferList";
            this.lblWaferList.Size = new System.Drawing.Size(548, 22);
            this.lblWaferList.TabIndex = 0;
            this.lblWaferList.Text = "Wafer 목록 (선택하면 Map 미리보기)";
            //
            // dgvWafer
            //
            this.dgvWafer.AllowUserToAddRows = false;
            this.dgvWafer.AllowUserToDeleteRows = false;
            this.dgvWafer.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvWafer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWafer.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRawWaferId,
            this.colWaferId,
            this.colSlot,
            this.colDefects,
            this.colDies,
            this.colImages,
            this.colState,
            this.colStepSeq,
            this.colMessage});
            this.dgvWafer.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvWafer.Location = new System.Drawing.Point(6, 28);
            this.dgvWafer.MultiSelect = false;
            this.dgvWafer.Name = "dgvWafer";
            this.dgvWafer.ReadOnly = true;
            this.dgvWafer.RowHeadersVisible = false;
            this.dgvWafer.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvWafer.Size = new System.Drawing.Size(548, 300);
            this.dgvWafer.TabIndex = 1;
            this.dgvWafer.SelectionChanged += new System.EventHandler(this.dgvWafer_SelectionChanged);
            //
            // colRawWaferId
            //
            this.colRawWaferId.HeaderText = "원본 Wafer";
            this.colRawWaferId.Name = "colRawWaferId";
            //
            // colWaferId
            //
            this.colWaferId.HeaderText = "DMS Wafer ID";
            this.colWaferId.Name = "colWaferId";
            //
            // colSlot
            //
            this.colSlot.HeaderText = "Slot";
            this.colSlot.Name = "colSlot";
            //
            // colDefects
            //
            this.colDefects.HeaderText = "Defect";
            this.colDefects.Name = "colDefects";
            //
            // colDies
            //
            this.colDies.HeaderText = "검사 Die";
            this.colDies.Name = "colDies";
            //
            // colImages
            //
            this.colImages.HeaderText = "이미지(찾음/참조)";
            this.colImages.Name = "colImages";
            //
            // colState
            //
            this.colState.HeaderText = "상태";
            this.colState.Name = "colState";
            //
            // colStepSeq
            //
            this.colStepSeq.HeaderText = "STEP_SEQ";
            this.colStepSeq.Name = "colStepSeq";
            //
            // colMessage
            //
            this.colMessage.HeaderText = "메시지";
            this.colMessage.Name = "colMessage";
            //
            // lblLog
            //
            this.lblLog.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLog.Location = new System.Drawing.Point(6, 328);
            this.lblLog.Name = "lblLog";
            this.lblLog.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.lblLog.Size = new System.Drawing.Size(548, 30);
            this.lblLog.TabIndex = 2;
            this.lblLog.Text = "처리 기록";
            //
            // txtLog
            //
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(6, 358);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(548, 380);
            this.txtLog.TabIndex = 3;
            this.txtLog.WordWrap = false;
            //
            // lblStatus
            //
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Location = new System.Drawing.Point(0, 862);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lblStatus.Size = new System.Drawing.Size(1400, 26);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "KLARF 파일을 선택하세요.";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // m_dMap (frmKlarfFileViewer 와 같은 기본값)
            //
            this.m_dMap.AngleOffSet = 0;
            this.m_dMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_dMap.CenterMark = false;
            this.m_dMap.Cursor = System.Windows.Forms.Cursors.Cross;
            this.m_dMap.DataSource = null;
            this.m_dMap.DefectSize = 7F;
            this.m_dMap.DieBackgroundColor = System.Drawing.Color.White;
            this.m_dMap.DieBorderColor = System.Drawing.Color.Gray;
            this.m_dMap.DieDefectColor = System.Drawing.Color.Empty;
            this.m_dMap.DieFocusingType = DACrux.Map.FocusType.Arraw;
            this.m_dMap.DieSizeX = 10000D;
            this.m_dMap.DieSizeY = 10000D;
            this.m_dMap.DisplayDieValue = DACrux.Base.DieDisplayValue.Bin;
            this.m_dMap.DisplayValue = "BIN";
            this.m_dMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dMap.DrawDefectImage = null;
            this.m_dMap.DrawDefects = "ALL";
            this.m_dMap.DrawFirstDie = false;
            this.m_dMap.DrawMarkDie = false;
            this.m_dMap.DrawOriginDie = false;
            this.m_dMap.DrawSkipDie = true;
            this.m_dMap.EdgeColor = System.Drawing.Color.DarkGray;
            this.m_dMap.EdgeSize = 0D;
            this.m_dMap.ForeColor = System.Drawing.Color.Red;
            this.m_dMap.IndexOffset = new System.Drawing.Point(0, 0);
            this.m_dMap.Location = new System.Drawing.Point(0, 118);
            this.m_dMap.MapType = DACrux.Base.MAP_TYPE.ROUGHBIN;
            this.m_dMap.Name = "m_dMap";
            this.m_dMap.NotchAngle = 0;
            this.m_dMap.NotchType = DACrux.Base.Notch.Notch;
            this.m_dMap.OriginDieBorder = System.Drawing.Color.Red;
            this.m_dMap.PopupMenu = true;
            this.m_dMap.SelecetedBin = "ALL";
            this.m_dMap.SelectedVI = "ALL";
            this.m_dMap.ShowDieIndexLabel = true;
            this.m_dMap.Size = new System.Drawing.Size(740, 744);
            this.m_dMap.TabIndex = 2;
            this.m_dMap.TransParent = 255;
            this.m_dMap.VisibleDieBorder = true;
            this.m_dMap.VisibleInfomation = true;
            this.m_dMap.WaferBorderColor = System.Drawing.Color.DimGray;
            this.m_dMap.WaferColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.m_dMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
            this.m_dMap.WaferID = "";
            this.m_dMap.WaferMargin = 0.95D;
            this.m_dMap.WaferSize = 300000D;
            this.m_dMap.XYDirect = DACrux.Base.XYDirection.LeftBottom;
            //
            // frmKlarfUpload
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 888);
            this.Controls.Add(this.m_dMap);
            this.Controls.Add(this.rightPanel);
            this.Controls.Add(this.topBar);
            this.Controls.Add(this.lblStatus);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.MinimumSize = new System.Drawing.Size(1000, 680);
            this.Name = "frmKlarfUpload";
            this.Text = "KLARF Upload";
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.rightPanel.ResumeLayout(false);
            this.rightPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWafer)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnAnalyze;
        private System.Windows.Forms.Label lblImageDir;
        private System.Windows.Forms.TextBox txtImageDir;
        private System.Windows.Forms.Button btnImageDir;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.Label lblStepId;
        private System.Windows.Forms.TextBox txtStepId;
        private System.Windows.Forms.Label lblEquip;
        private System.Windows.Forms.TextBox txtEquip;
        private System.Windows.Forms.Label lblClassSource;
        private System.Windows.Forms.ComboBox cmbClassSource;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel rightPanel;
        private System.Windows.Forms.Label lblWaferList;
        private System.Windows.Forms.DataGridView dgvWafer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRawWaferId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWaferId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSlot;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDefects;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDies;
        private System.Windows.Forms.DataGridViewTextBoxColumn colImages;
        private System.Windows.Forms.DataGridViewTextBoxColumn colState;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStepSeq;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMessage;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label lblStatus;
        private KlarfDefectMap m_dMap;
    }
}
