namespace DACrux.SEMDMS.ENGUI
{
    // KLARF 업로드 화면의 컨트롤과 배치를 정의한다. 파일 읽기 / 저장 동작은 본문 파일(frmKlarfUpload.cs)에서 처리한다.
    // 2026-10-08 재배치(요청): 처리 기록 제거 / KLARF Defect Grid 추가 / [분석] 버튼 제거 / 아이콘 버튼.
    // 버튼 아이콘은 DMS/99_Resource/ByModule/POC/BTN_*.png 를 frmKlarfUpload.resx 에 이미지로 넣어 쓴다.
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
        /// 위: [파일] 묶음(KLARF 파일 / 이미지 폴더 + 선택 버튼) / [저장 값] 묶음 / 오른쪽 큰 [DB 저장] 버튼
        /// 그 아래: KLARF 요약 한 줄
        /// 본문(좌우 분할): 왼쪽 Wafer Map 미리보기 / 오른쪽(상하 분할) Wafer 목록 + KLARF Defect 목록
        /// 아래: 상태 표시줄
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmKlarfUpload));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.topBar = new System.Windows.Forms.Panel();
            this.grpFile = new System.Windows.Forms.GroupBox();
            this.lblFile = new System.Windows.Forms.Label();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblImageDir = new System.Windows.Forms.Label();
            this.txtImageDir = new System.Windows.Forms.TextBox();
            this.btnImageDir = new System.Windows.Forms.Button();
            this.grpSaveValue = new System.Windows.Forms.GroupBox();
            this.lblProduct = new System.Windows.Forms.Label();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.lblStepId = new System.Windows.Forms.Label();
            this.txtStepId = new System.Windows.Forms.TextBox();
            this.lblEquip = new System.Windows.Forms.Label();
            this.txtEquip = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblSummary = new System.Windows.Forms.Label();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.grpMap = new System.Windows.Forms.GroupBox();
            this.m_dMap = new DACrux.SEMDMS.ENGUI.KlarfDefectMap();
            this.splitRight = new System.Windows.Forms.SplitContainer();
            this.grpWafer = new System.Windows.Forms.GroupBox();
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
            this.grpDefect = new System.Windows.Forms.GroupBox();
            this.dgvDefect = new System.Windows.Forms.DataGridView();
            this.colDefectId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIndexX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIndexY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClassName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colImageFound = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colXRel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colYRel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRoughBin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFineBin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colImageFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.busyBar = new DACrux.SEMDMS.Control.ProgressStatusBar();
            this.topBar.SuspendLayout();
            this.grpFile.SuspendLayout();
            this.grpSaveValue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.grpMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitRight)).BeginInit();
            this.splitRight.Panel1.SuspendLayout();
            this.splitRight.Panel2.SuspendLayout();
            this.splitRight.SuspendLayout();
            this.grpWafer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWafer)).BeginInit();
            this.grpDefect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDefect)).BeginInit();
            this.pnlStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // topBar
            // 
            this.topBar.Controls.Add(this.grpFile);
            this.topBar.Controls.Add(this.grpSaveValue);
            this.topBar.Controls.Add(this.btnSave);
            this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar.Location = new System.Drawing.Point(0, 0);
            this.topBar.Name = "topBar";
            this.topBar.Size = new System.Drawing.Size(1400, 152);
            this.topBar.TabIndex = 0;
            // 
            // grpFile
            // 
            this.grpFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpFile.Controls.Add(this.lblFile);
            this.grpFile.Controls.Add(this.txtFile);
            this.grpFile.Controls.Add(this.btnBrowse);
            this.grpFile.Controls.Add(this.lblImageDir);
            this.grpFile.Controls.Add(this.txtImageDir);
            this.grpFile.Controls.Add(this.btnImageDir);
            this.grpFile.Location = new System.Drawing.Point(8, 4);
            this.grpFile.Name = "grpFile";
            this.grpFile.Size = new System.Drawing.Size(1384, 88);
            this.grpFile.TabIndex = 0;
            this.grpFile.TabStop = false;
            this.grpFile.Text = "파일";
            // 
            // lblFile
            // 
            this.lblFile.Location = new System.Drawing.Point(12, 22);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(84, 26);
            this.lblFile.TabIndex = 0;
            this.lblFile.Text = "KLARF 파일";
            this.lblFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtFile
            // 
            this.txtFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFile.BackColor = System.Drawing.SystemColors.Window;
            this.txtFile.Location = new System.Drawing.Point(98, 22);
            this.txtFile.Name = "txtFile";
            this.txtFile.ReadOnly = true;
            this.txtFile.Size = new System.Drawing.Size(1024, 23);
            this.txtFile.TabIndex = 1;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowse.Image")));
            this.btnBrowse.Location = new System.Drawing.Point(1278, 19);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(96, 28);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "파일 선택";
            this.btnBrowse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lblImageDir
            // 
            this.lblImageDir.Location = new System.Drawing.Point(12, 54);
            this.lblImageDir.Name = "lblImageDir";
            this.lblImageDir.Size = new System.Drawing.Size(84, 26);
            this.lblImageDir.TabIndex = 3;
            this.lblImageDir.Text = "이미지 폴더";
            this.lblImageDir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtImageDir
            // 
            this.txtImageDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtImageDir.BackColor = System.Drawing.SystemColors.Window;
            this.txtImageDir.Location = new System.Drawing.Point(98, 54);
            this.txtImageDir.Name = "txtImageDir";
            this.txtImageDir.ReadOnly = true;
            this.txtImageDir.Size = new System.Drawing.Size(1024, 23);
            this.txtImageDir.TabIndex = 4;
            // 
            // btnImageDir
            // 
            this.btnImageDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImageDir.Image = ((System.Drawing.Image)(resources.GetObject("btnImageDir.Image")));
            this.btnImageDir.Location = new System.Drawing.Point(1278, 51);
            this.btnImageDir.Name = "btnImageDir";
            this.btnImageDir.Size = new System.Drawing.Size(96, 28);
            this.btnImageDir.TabIndex = 5;
            this.btnImageDir.Text = "폴더 선택";
            this.btnImageDir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnImageDir.UseVisualStyleBackColor = true;
            this.btnImageDir.Click += new System.EventHandler(this.btnImageDir_Click);
            // 
            // grpSaveValue
            // 
            this.grpSaveValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSaveValue.Controls.Add(this.lblProduct);
            this.grpSaveValue.Controls.Add(this.txtProduct);
            this.grpSaveValue.Controls.Add(this.lblStepId);
            this.grpSaveValue.Controls.Add(this.txtStepId);
            this.grpSaveValue.Controls.Add(this.lblEquip);
            this.grpSaveValue.Controls.Add(this.txtEquip);
            this.grpSaveValue.Location = new System.Drawing.Point(8, 94);
            this.grpSaveValue.Name = "grpSaveValue";
            this.grpSaveValue.Size = new System.Drawing.Size(1238, 54);
            this.grpSaveValue.TabIndex = 1;
            this.grpSaveValue.TabStop = false;
            this.grpSaveValue.Text = "저장 값 ";
            // 
            // lblProduct
            // 
            this.lblProduct.Location = new System.Drawing.Point(12, 20);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(84, 26);
            this.lblProduct.TabIndex = 0;
            this.lblProduct.Text = "Product";
            this.lblProduct.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtProduct
            // 
            this.txtProduct.Location = new System.Drawing.Point(98, 22);
            this.txtProduct.MaxLength = 40;
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Size = new System.Drawing.Size(200, 31);
            this.txtProduct.TabIndex = 1;
            // 
            // lblStepId
            // 
            this.lblStepId.Location = new System.Drawing.Point(320, 20);
            this.lblStepId.Name = "lblStepId";
            this.lblStepId.Size = new System.Drawing.Size(60, 26);
            this.lblStepId.TabIndex = 2;
            this.lblStepId.Text = "Step ID";
            this.lblStepId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtStepId
            // 
            this.txtStepId.Location = new System.Drawing.Point(382, 22);
            this.txtStepId.MaxLength = 40;
            this.txtStepId.Name = "txtStepId";
            this.txtStepId.Size = new System.Drawing.Size(260, 31);
            this.txtStepId.TabIndex = 3;
            // 
            // lblEquip
            // 
            this.lblEquip.Location = new System.Drawing.Point(664, 20);
            this.lblEquip.Name = "lblEquip";
            this.lblEquip.Size = new System.Drawing.Size(40, 26);
            this.lblEquip.TabIndex = 4;
            this.lblEquip.Text = "설비";
            this.lblEquip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtEquip
            // 
            this.txtEquip.Location = new System.Drawing.Point(706, 22);
            this.txtEquip.MaxLength = 40;
            this.txtEquip.Name = "txtEquip";
            this.txtEquip.Size = new System.Drawing.Size(150, 31);
            this.txtEquip.TabIndex = 5;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(1252, 113);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(140, 28);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "DB 저장";
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblSummary
            // 
            this.lblSummary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(242)))), ((int)(((byte)(250)))));
            this.lblSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSummary.Font = new System.Drawing.Font("맑은 고딕", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(50)))), ((int)(((byte)(90)))));
            this.lblSummary.Location = new System.Drawing.Point(0, 152);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.lblSummary.Size = new System.Drawing.Size(1400, 30);
            this.lblSummary.TabIndex = 1;
            this.lblSummary.Text = "";
            this.lblSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 182);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.grpMap);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.splitRight);
            this.splitMain.Size = new System.Drawing.Size(1400, 680);
            this.splitMain.SplitterDistance = 560;
            this.splitMain.SplitterWidth = 6;
            this.splitMain.TabIndex = 2;
            // 
            // grpMap
            // 
            this.grpMap.Controls.Add(this.m_dMap);
            this.grpMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpMap.Location = new System.Drawing.Point(0, 0);
            this.grpMap.Name = "grpMap";
            this.grpMap.Size = new System.Drawing.Size(560, 680);
            this.grpMap.TabIndex = 0;
            this.grpMap.TabStop = false;
            this.grpMap.Text = "Wafer Map";
            // 
            // m_dMap
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
            this.m_dMap.DieMaxX = -65536;
            this.m_dMap.DieMaxY = -65536;
            this.m_dMap.DieMinX = 65536;
            this.m_dMap.DieMinY = 65536;
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
            this.m_dMap.FirstDieBorderColor = System.Drawing.Color.SkyBlue;
            this.m_dMap.FirstDieX = 0;
            this.m_dMap.FirstDieY = 0;
            this.m_dMap.ForeColor = System.Drawing.Color.Red;
            this.m_dMap.FromGradationDieColor = System.Drawing.Color.Lime;
            this.m_dMap.GradationInterval = 5;
            this.m_dMap.GradationMaxValue = double.NaN;
            this.m_dMap.GradationMinValue = double.NaN;
            this.m_dMap.IndexOffset = new System.Drawing.Point(0, 0);
            this.m_dMap.Location = new System.Drawing.Point(3, 27);
            this.m_dMap.MapType = DACrux.Base.MAP_TYPE.CLASS;
            this.m_dMap.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.m_dMap.MarkDieColor = System.Drawing.Color.LightSkyBlue;
            this.m_dMap.Name = "m_dMap";
            this.m_dMap.NotchAngle = 0;
            this.m_dMap.NotchType = DACrux.Base.Notch.Notch;
            this.m_dMap.OriginDieBorder = System.Drawing.Color.Red;
            this.m_dMap.OriginIndexX = 0;
            this.m_dMap.OriginIndexY = 0;
            this.m_dMap.OriginX = 0D;
            this.m_dMap.OriginY = 0D;
            this.m_dMap.ParaLimit = false;
            this.m_dMap.ParametricColumn = "PCMVALUE";
            this.m_dMap.ParaValueFont = new System.Drawing.Font("굴림", 9F);
            this.m_dMap.PickupDieAlpha = 96;
            this.m_dMap.PickupedDieColor = System.Drawing.Color.Transparent;
            this.m_dMap.PopupMenu = true;
            this.m_dMap.ReferenceDieSetting = 0;
            this.m_dMap.ScaleMark = false;
            this.m_dMap.SelecetedBin = "ALL";
            this.m_dMap.SelectedVI = "ALL";
            this.m_dMap.ShowDieIndexLabel = true;
            this.m_dMap.Size = new System.Drawing.Size(554, 650);
            this.m_dMap.SizeColor = null;
            this.m_dMap.SkipDieColor = System.Drawing.Color.Yellow;
            this.m_dMap.TabIndex = 0;
            this.m_dMap.ToGradationDieColor = System.Drawing.Color.Red;
            this.m_dMap.TransParent = 255;
            this.m_dMap.TypeColor = null;
            this.m_dMap.ViewAngle = 0;
            this.m_dMap.VIMember = "VIFAIL";
            this.m_dMap.VisibleDieBorder = true;
            this.m_dMap.VisibleDieValue = false;
            this.m_dMap.VisibleFocusDie = false;
            this.m_dMap.VisibleImageMark = false;
            this.m_dMap.VisibleInfomation = true;
            this.m_dMap.VisibleOffDie = false;
            this.m_dMap.VisibleProbeOverlay = false;
            this.m_dMap.VisibleShotAlignPoint = false;
            this.m_dMap.VisibleSignDies = false;
            this.m_dMap.VisibleStringBin = false;
            this.m_dMap.VisibleVIFail = false;
            this.m_dMap.VisibleXY = false;
            this.m_dMap.WaferBorderColor = System.Drawing.Color.DimGray;
            this.m_dMap.WaferColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.m_dMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
            this.m_dMap.WaferID = "";
            this.m_dMap.WaferMargin = 0.95D;
            this.m_dMap.WaferSize = 300000D;
            this.m_dMap.XYDirect = DACrux.Base.XYDirection.LeftBottom;
            // 
            // splitRight
            // 
            this.splitRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitRight.Location = new System.Drawing.Point(0, 0);
            this.splitRight.Name = "splitRight";
            this.splitRight.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitRight.Panel1
            // 
            this.splitRight.Panel1.Controls.Add(this.grpWafer);
            // 
            // splitRight.Panel2
            // 
            this.splitRight.Panel2.Controls.Add(this.grpDefect);
            this.splitRight.Size = new System.Drawing.Size(834, 680);
            this.splitRight.SplitterDistance = 170;
            this.splitRight.SplitterWidth = 6;
            this.splitRight.TabIndex = 0;
            // 
            // grpWafer
            // 
            this.grpWafer.Controls.Add(this.dgvWafer);
            this.grpWafer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpWafer.Location = new System.Drawing.Point(0, 0);
            this.grpWafer.Name = "grpWafer";
            this.grpWafer.Size = new System.Drawing.Size(834, 170);
            this.grpWafer.TabIndex = 0;
            this.grpWafer.TabStop = false;
            this.grpWafer.Text = "Wafer 목록 ";
            // 
            // dgvWafer
            // 
            this.dgvWafer.AllowUserToAddRows = false;
            this.dgvWafer.AllowUserToDeleteRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.dgvWafer.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvWafer.BackgroundColor = System.Drawing.SystemColors.Window;
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
            this.dgvWafer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvWafer.Location = new System.Drawing.Point(3, 27);
            this.dgvWafer.MultiSelect = false;
            this.dgvWafer.Name = "dgvWafer";
            this.dgvWafer.ReadOnly = true;
            this.dgvWafer.RowHeadersVisible = false;
            this.dgvWafer.RowHeadersWidth = 62;
            this.dgvWafer.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvWafer.Size = new System.Drawing.Size(828, 140);
            this.dgvWafer.TabIndex = 0;
            this.dgvWafer.SelectionChanged += new System.EventHandler(this.dgvWafer_SelectionChanged);
            // 
            // colRawWaferId
            // 
            this.colRawWaferId.HeaderText = "원본 Wafer";
            this.colRawWaferId.MinimumWidth = 8;
            this.colRawWaferId.Name = "colRawWaferId";
            this.colRawWaferId.ReadOnly = true;
            this.colRawWaferId.Width = 127;
            // 
            // colWaferId
            // 
            this.colWaferId.HeaderText = "DMS Wafer ID";
            this.colWaferId.MinimumWidth = 8;
            this.colWaferId.Name = "colWaferId";
            this.colWaferId.ReadOnly = true;
            this.colWaferId.Width = 136;
            // 
            // colSlot
            // 
            this.colSlot.HeaderText = "Slot";
            this.colSlot.MinimumWidth = 8;
            this.colSlot.Name = "colSlot";
            this.colSlot.ReadOnly = true;
            this.colSlot.Width = 79;
            // 
            // colDefects
            // 
            this.colDefects.HeaderText = "Defect";
            this.colDefects.MinimumWidth = 8;
            this.colDefects.Name = "colDefects";
            this.colDefects.ReadOnly = true;
            this.colDefects.Width = 102;
            // 
            // colDies
            // 
            this.colDies.HeaderText = "검사 Die";
            this.colDies.MinimumWidth = 8;
            this.colDies.Name = "colDies";
            this.colDies.ReadOnly = true;
            this.colDies.Width = 84;
            // 
            // colImages
            // 
            this.colImages.HeaderText = "이미지";
            this.colImages.MinimumWidth = 8;
            this.colImages.Name = "colImages";
            this.colImages.ReadOnly = true;
            this.colImages.Width = 138;
            // 
            // colState
            // 
            this.colState.HeaderText = "상태";
            this.colState.MinimumWidth = 80;
            this.colState.Name = "colState";
            this.colState.ReadOnly = true;
            this.colState.Width = 80;
            // 
            // colStepSeq
            // 
            this.colStepSeq.HeaderText = "STEP_SEQ";
            this.colStepSeq.MinimumWidth = 70;
            this.colStepSeq.Name = "colStepSeq";
            this.colStepSeq.ReadOnly = true;
            this.colStepSeq.Visible = false;
            this.colStepSeq.Width = 128;
            // 
            // colMessage
            // 
            this.colMessage.HeaderText = "메시지";
            this.colMessage.MinimumWidth = 8;
            this.colMessage.Name = "colMessage";
            this.colMessage.ReadOnly = true;
            this.colMessage.Visible = false;
            this.colMessage.Width = 95;
            // 
            // grpDefect
            // 
            this.grpDefect.Controls.Add(this.dgvDefect);
            this.grpDefect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDefect.Location = new System.Drawing.Point(0, 0);
            this.grpDefect.Name = "grpDefect";
            this.grpDefect.Size = new System.Drawing.Size(834, 504);
            this.grpDefect.TabIndex = 0;
            this.grpDefect.TabStop = false;
            this.grpDefect.Text = "KLARF Defect 목록 ";
            // 
            // dgvDefect
            // 
            this.dgvDefect.AllowUserToAddRows = false;
            this.dgvDefect.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.dgvDefect.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvDefect.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvDefect.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDefect.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDefectId,
            this.colIndexX,
            this.colIndexY,
            this.colClass,
            this.colClassName,
            this.colImageFound,
            this.colX,
            this.colY,
            this.colXRel,
            this.colYRel,
            this.colSize,
            this.colTest,
            this.colRoughBin,
            this.colFineBin,
            this.colImageFile});
            this.dgvDefect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDefect.Location = new System.Drawing.Point(3, 27);
            this.dgvDefect.Name = "dgvDefect";
            this.dgvDefect.ReadOnly = true;
            this.dgvDefect.RowHeadersVisible = false;
            this.dgvDefect.RowHeadersWidth = 62;
            this.dgvDefect.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDefect.Size = new System.Drawing.Size(828, 474);
            this.dgvDefect.TabIndex = 0;
            // 
            // colDefectId
            // 
            this.colDefectId.HeaderText = "Defect ID";
            this.colDefectId.MinimumWidth = 8;
            this.colDefectId.Name = "colDefectId";
            this.colDefectId.ReadOnly = true;
            this.colDefectId.Width = 126;
            // 
            // colIndexX
            // 
            this.colIndexX.HeaderText = "X Index";
            this.colIndexX.MinimumWidth = 8;
            this.colIndexX.Name = "colIndexX";
            this.colIndexX.ReadOnly = true;
            this.colIndexX.Width = 109;
            // 
            // colIndexY
            // 
            this.colIndexY.HeaderText = "Y Index";
            this.colIndexY.MinimumWidth = 8;
            this.colIndexY.Name = "colIndexY";
            this.colIndexY.ReadOnly = true;
            this.colIndexY.Width = 108;
            // 
            // colClassName
            // 
            this.colClassName.HeaderText = "분류명";
            this.colClassName.MinimumWidth = 8;
            this.colClassName.Name = "colClassName";
            this.colClassName.ReadOnly = true;
            this.colClassName.Width = 102;
            // 
            // colImageFound
            // 
            this.colImageFound.HeaderText = "이미지";
            this.colImageFound.MinimumWidth = 8;
            this.colImageFound.Name = "colImageFound";
            this.colImageFound.ReadOnly = true;
            this.colImageFound.Width = 102;
            // 
            // colX
            // 
            this.colX.HeaderText = "X";
            this.colX.MinimumWidth = 8;
            this.colX.Name = "colX";
            this.colX.ReadOnly = true;
            this.colX.Width = 101;
            // 
            // colY
            // 
            this.colY.HeaderText = "Y";
            this.colY.MinimumWidth = 8;
            this.colY.Name = "colY";
            this.colY.ReadOnly = true;
            // 
            // colXRel
            // 
            this.colXRel.HeaderText = "XREL";
            this.colXRel.MinimumWidth = 8;
            this.colXRel.Name = "colXRel";
            this.colXRel.ReadOnly = true;
            this.colXRel.Width = 88;
            // 
            // colYRel
            // 
            this.colYRel.HeaderText = "YREL";
            this.colYRel.MinimumWidth = 8;
            this.colYRel.Name = "colYRel";
            this.colYRel.ReadOnly = true;
            this.colYRel.Width = 87;
            // 
            // colSize
            // 
            this.colSize.HeaderText = "Size";
            this.colSize.MinimumWidth = 8;
            this.colSize.Name = "colSize";
            this.colSize.ReadOnly = true;
            this.colSize.Width = 80;
            // 
            // colTest
            // 
            this.colTest.HeaderText = "Test";
            this.colTest.MinimumWidth = 8;
            this.colTest.Name = "colTest";
            this.colTest.ReadOnly = true;
            this.colTest.Width = 82;
            // 
            // colClass
            // 
            this.colClass.HeaderText = "Class";
            this.colClass.MinimumWidth = 8;
            this.colClass.Name = "colClass";
            this.colClass.ReadOnly = true;
            this.colClass.Width = 88;
            // 
            // colRoughBin
            // 
            this.colRoughBin.HeaderText = "RoughBin";
            this.colRoughBin.MinimumWidth = 8;
            this.colRoughBin.Name = "colRoughBin";
            this.colRoughBin.ReadOnly = true;
            this.colRoughBin.Width = 126;
            // 
            // colFineBin
            // 
            this.colFineBin.HeaderText = "FineBin";
            this.colFineBin.MinimumWidth = 8;
            this.colFineBin.Name = "colFineBin";
            this.colFineBin.ReadOnly = true;
            this.colFineBin.Width = 106;
            // 
            // colImageFile
            // 
            this.colImageFile.HeaderText = "이미지 파일";
            this.colImageFile.MinimumWidth = 8;
            this.colImageFile.Name = "colImageFile";
            this.colImageFile.ReadOnly = true;
            this.colImageFile.Width = 144;
            // 
            // pnlStatus
            // 
            this.pnlStatus.Controls.Add(this.lblStatus);
            this.pnlStatus.Controls.Add(this.busyBar);
            this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(1300, 26);
            this.pnlStatus.TabIndex = 90;
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
            this.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Location = new System.Drawing.Point(0, 862);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lblStatus.Size = new System.Drawing.Size(1400, 26);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmKlarfUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 888);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.lblSummary);
            this.Controls.Add(this.topBar);
            this.Controls.Add(this.pnlStatus);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.MinimumSize = new System.Drawing.Size(1100, 700);
            this.Name = "frmKlarfUpload";
            this.Text = "KLARF Upload";
            this.topBar.ResumeLayout(false);
            this.grpFile.ResumeLayout(false);
            this.grpFile.PerformLayout();
            this.grpSaveValue.ResumeLayout(false);
            this.grpSaveValue.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.grpMap.ResumeLayout(false);
            this.splitRight.Panel1.ResumeLayout(false);
            this.splitRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitRight)).EndInit();
            this.splitRight.ResumeLayout(false);
            this.grpWafer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWafer)).EndInit();
            this.grpDefect.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDefect)).EndInit();
            this.pnlStatus.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.GroupBox grpFile;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblImageDir;
        private System.Windows.Forms.TextBox txtImageDir;
        private System.Windows.Forms.Button btnImageDir;
        private System.Windows.Forms.GroupBox grpSaveValue;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.Label lblStepId;
        private System.Windows.Forms.TextBox txtStepId;
        private System.Windows.Forms.Label lblEquip;
        private System.Windows.Forms.TextBox txtEquip;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.GroupBox grpMap;
        private KlarfDefectMap m_dMap;
        private System.Windows.Forms.SplitContainer splitRight;
        private System.Windows.Forms.GroupBox grpWafer;
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
        private System.Windows.Forms.GroupBox grpDefect;
        private System.Windows.Forms.DataGridView dgvDefect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDefectId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIndexX;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIndexY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colX;
        private System.Windows.Forms.DataGridViewTextBoxColumn colY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colXRel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colYRel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTest;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRoughBin;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFineBin;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClassName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colImageFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn colImageFound;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Label lblStatus;
        private DACrux.SEMDMS.Control.ProgressStatusBar busyBar;
    }
}
