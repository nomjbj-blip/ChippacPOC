namespace DACrux.SEMDMS.ENGUI
{
    // E-Test 결과 업로드 화면의 컨트롤과 배치를 정의한다. 분석 / 저장 / 삭제 동작은 본문 파일(frmEtResultUpload.cs)에서 처리한다.
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
        /// 위: E-Test 파일 / Factory / Device(파일명, 읽기 전용) / Slot No / [찾아보기] [목록 조회] [DB 저장], 그 아래: 파일 정보(항목별 읽기 전용 칸 + 경고).
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
            this.txtDevice = new System.Windows.Forms.TextBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblSlot = new System.Windows.Forms.Label();
            this.txtSlot = new System.Windows.Forms.TextBox();
            this.lblWafer = new System.Windows.Forms.Label();
            this.grpInfo = new System.Windows.Forms.GroupBox();
            this.tlpInfo = new System.Windows.Forms.TableLayoutPanel();
            this.txtWarning = new System.Windows.Forms.TextBox();
            this.lblInfoCustomer = new System.Windows.Forms.Label();
            this.txtInfoCustomer = new System.Windows.Forms.TextBox();
            this.lblInfoWaferType = new System.Windows.Forms.Label();
            this.txtInfoWaferType = new System.Windows.Forms.TextBox();
            this.lblInfoWaferRev = new System.Windows.Forms.Label();
            this.txtInfoWaferRev = new System.Windows.Forms.TextBox();
            this.lblInfoFab = new System.Windows.Forms.Label();
            this.txtInfoFab = new System.Windows.Forms.TextBox();
            this.lblInfoTestType = new System.Windows.Forms.Label();
            this.txtInfoTestType = new System.Windows.Forms.TextBox();
            this.lblInfoTestDate = new System.Windows.Forms.Label();
            this.txtInfoTestDate = new System.Windows.Forms.TextBox();
            this.lblInfoLot = new System.Windows.Forms.Label();
            this.txtInfoLot = new System.Windows.Forms.TextBox();
            this.lblInfoLotWafer = new System.Windows.Forms.Label();
            this.txtInfoLotWafer = new System.Windows.Forms.TextBox();
            this.lblInfoDie = new System.Windows.Forms.Label();
            this.txtInfoDie = new System.Windows.Forms.TextBox();
            this.lblInfoPass = new System.Windows.Forms.Label();
            this.txtInfoPass = new System.Windows.Forms.TextBox();
            this.lblInfoFail = new System.Windows.Forms.Label();
            this.txtInfoFail = new System.Windows.Forms.TextBox();
            this.lblInfoSheet = new System.Windows.Forms.Label();
            this.txtInfoSheet = new System.Windows.Forms.TextBox();
            this.lblInfoItems = new System.Windows.Forms.Label();
            this.txtInfoItems = new System.Windows.Forms.TextBox();
            this.lblInfoValues = new System.Windows.Forms.Label();
            this.txtInfoValues = new System.Windows.Forms.TextBox();
            this.lblInfoNotTested = new System.Windows.Forms.Label();
            this.txtInfoNotTested = new System.Windows.Forms.TextBox();
            this.lblInfoEmpty = new System.Windows.Forms.Label();
            this.txtInfoEmpty = new System.Windows.Forms.TextBox();
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
            this.grpInfo.SuspendLayout();
            this.tlpInfo.SuspendLayout();
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
            this.topBar.Controls.Add(this.txtDevice);
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
            // txtDevice: Device 는 선택하지 않고 파일명 2칸(Device)을 그대로 쓴다(읽기 전용, 2026-10-12)
            //
            this.txtDevice.Location = new System.Drawing.Point(262, 38);
            this.txtDevice.Name = "txtDevice";
            this.txtDevice.ReadOnly = true;
            this.txtDevice.Size = new System.Drawing.Size(230, 21);
            this.txtDevice.TabIndex = 6;
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
            // grpInfo: 파일 분석 결과를 항목별 읽기 전용 칸으로 보여 준다(문자열 한 줄 대신 TextBox, 2026-10-12)
            this.grpInfo.Controls.Add(this.tlpInfo);
            this.grpInfo.Controls.Add(this.txtWarning);
            this.grpInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpInfo.Location = new System.Drawing.Point(0, 94);
            this.grpInfo.Name = "grpInfo";
            this.grpInfo.Size = new System.Drawing.Size(1100, 160);
            this.grpInfo.TabIndex = 1;
            this.grpInfo.TabStop = false;
            this.grpInfo.Text = "파일 정보 (파일의 Upper / Lower Limit 은 저장하지 않음 - 판정은 E-Test Limit 설정 화면)";
            // tlpInfo: 라벨 / 값 4쌍 x 4줄
            this.tlpInfo.ColumnCount = 8;
            this.tlpInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            this.tlpInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            this.tlpInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            this.tlpInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            this.tlpInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpInfo.RowCount = 4;
            this.tlpInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tlpInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tlpInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tlpInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tlpInfo.Controls.Add(this.lblInfoCustomer, 0, 0);
            this.tlpInfo.Controls.Add(this.txtInfoCustomer, 1, 0);
            this.tlpInfo.Controls.Add(this.lblInfoWaferType, 2, 0);
            this.tlpInfo.Controls.Add(this.txtInfoWaferType, 3, 0);
            this.tlpInfo.Controls.Add(this.lblInfoWaferRev, 4, 0);
            this.tlpInfo.Controls.Add(this.txtInfoWaferRev, 5, 0);
            this.tlpInfo.Controls.Add(this.lblInfoFab, 6, 0);
            this.tlpInfo.Controls.Add(this.txtInfoFab, 7, 0);
            this.tlpInfo.Controls.Add(this.lblInfoTestType, 0, 1);
            this.tlpInfo.Controls.Add(this.txtInfoTestType, 1, 1);
            this.tlpInfo.Controls.Add(this.lblInfoTestDate, 2, 1);
            this.tlpInfo.Controls.Add(this.txtInfoTestDate, 3, 1);
            this.tlpInfo.Controls.Add(this.lblInfoLot, 4, 1);
            this.tlpInfo.Controls.Add(this.txtInfoLot, 5, 1);
            this.tlpInfo.Controls.Add(this.lblInfoLotWafer, 6, 1);
            this.tlpInfo.Controls.Add(this.txtInfoLotWafer, 7, 1);
            this.tlpInfo.Controls.Add(this.lblInfoDie, 0, 2);
            this.tlpInfo.Controls.Add(this.txtInfoDie, 1, 2);
            this.tlpInfo.Controls.Add(this.lblInfoPass, 2, 2);
            this.tlpInfo.Controls.Add(this.txtInfoPass, 3, 2);
            this.tlpInfo.Controls.Add(this.lblInfoFail, 4, 2);
            this.tlpInfo.Controls.Add(this.txtInfoFail, 5, 2);
            this.tlpInfo.Controls.Add(this.lblInfoSheet, 6, 2);
            this.tlpInfo.Controls.Add(this.txtInfoSheet, 7, 2);
            this.tlpInfo.Controls.Add(this.lblInfoItems, 0, 3);
            this.tlpInfo.Controls.Add(this.txtInfoItems, 1, 3);
            this.tlpInfo.Controls.Add(this.lblInfoValues, 2, 3);
            this.tlpInfo.Controls.Add(this.txtInfoValues, 3, 3);
            this.tlpInfo.Controls.Add(this.lblInfoNotTested, 4, 3);
            this.tlpInfo.Controls.Add(this.txtInfoNotTested, 5, 3);
            this.tlpInfo.Controls.Add(this.lblInfoEmpty, 6, 3);
            this.tlpInfo.Controls.Add(this.txtInfoEmpty, 7, 3);
            this.tlpInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpInfo.Location = new System.Drawing.Point(3, 17);
            this.tlpInfo.Name = "tlpInfo";
            this.tlpInfo.Size = new System.Drawing.Size(1094, 108);
            this.tlpInfo.TabIndex = 0;
            // lblInfoCustomer / txtInfoCustomer
            this.lblInfoCustomer.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblInfoCustomer.AutoSize = true;
            this.lblInfoCustomer.Name = "lblInfoCustomer";
            this.lblInfoCustomer.TabIndex = 0;
            this.lblInfoCustomer.Text = "Customer";
            this.txtInfoCustomer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfoCustomer.Name = "txtInfoCustomer";
            this.txtInfoCustomer.ReadOnly = true;
            this.txtInfoCustomer.TabIndex = 1;
            // lblInfoWaferType / txtInfoWaferType
            this.lblInfoWaferType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblInfoWaferType.AutoSize = true;
            this.lblInfoWaferType.Name = "lblInfoWaferType";
            this.lblInfoWaferType.TabIndex = 2;
            this.lblInfoWaferType.Text = "Wafer Type";
            this.txtInfoWaferType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfoWaferType.Name = "txtInfoWaferType";
            this.txtInfoWaferType.ReadOnly = true;
            this.txtInfoWaferType.TabIndex = 3;
            // lblInfoWaferRev / txtInfoWaferRev
            this.lblInfoWaferRev.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblInfoWaferRev.AutoSize = true;
            this.lblInfoWaferRev.Name = "lblInfoWaferRev";
            this.lblInfoWaferRev.TabIndex = 4;
            this.lblInfoWaferRev.Text = "Wafer Rev";
            this.txtInfoWaferRev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfoWaferRev.Name = "txtInfoWaferRev";
            this.txtInfoWaferRev.ReadOnly = true;
            this.txtInfoWaferRev.TabIndex = 5;
            // lblInfoFab / txtInfoFab
            this.lblInfoFab.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblInfoFab.AutoSize = true;
            this.lblInfoFab.Name = "lblInfoFab";
            this.lblInfoFab.TabIndex = 6;
            this.lblInfoFab.Text = "Fab";
            this.txtInfoFab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfoFab.Name = "txtInfoFab";
            this.txtInfoFab.ReadOnly = true;
            this.txtInfoFab.TabIndex = 7;
            // lblInfoTestType / txtInfoTestType
            this.lblInfoTestType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblInfoTestType.AutoSize = true;
            this.lblInfoTestType.Name = "lblInfoTestType";
            this.lblInfoTestType.TabIndex = 8;
            this.lblInfoTestType.Text = "공정 (Test Type)";
            this.txtInfoTestType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfoTestType.Name = "txtInfoTestType";
            this.txtInfoTestType.ReadOnly = true;
            this.txtInfoTestType.TabIndex = 9;
            // lblInfoTestDate / txtInfoTestDate
            this.lblInfoTestDate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblInfoTestDate.AutoSize = true;
            this.lblInfoTestDate.Name = "lblInfoTestDate";
            this.lblInfoTestDate.TabIndex = 10;
            this.lblInfoTestDate.Text = "Test Date";
            this.txtInfoTestDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfoTestDate.Name = "txtInfoTestDate";
            this.txtInfoTestDate.ReadOnly = true;
            this.txtInfoTestDate.TabIndex = 11;
            // lblInfoLot / txtInfoLot
            this.lblInfoLot.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblInfoLot.AutoSize = true;
            this.lblInfoLot.Name = "lblInfoLot";
            this.lblInfoLot.TabIndex = 12;
            this.lblInfoLot.Text = "Lot";
            this.txtInfoLot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfoLot.Name = "txtInfoLot";
            this.txtInfoLot.ReadOnly = true;
            this.txtInfoLot.TabIndex = 13;
            // lblInfoLotWafer / txtInfoLotWafer
            this.lblInfoLotWafer.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblInfoLotWafer.AutoSize = true;
            this.lblInfoLotWafer.Name = "lblInfoLotWafer";
            this.lblInfoLotWafer.TabIndex = 14;
            this.lblInfoLotWafer.Text = "Lot-Wafer No";
            this.txtInfoLotWafer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfoLotWafer.Name = "txtInfoLotWafer";
            this.txtInfoLotWafer.ReadOnly = true;
            this.txtInfoLotWafer.TabIndex = 15;
            // lblInfoDie / txtInfoDie
            this.lblInfoDie.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblInfoDie.AutoSize = true;
            this.lblInfoDie.Name = "lblInfoDie";
            this.lblInfoDie.TabIndex = 16;
            this.lblInfoDie.Text = "Die";
            this.txtInfoDie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfoDie.Name = "txtInfoDie";
            this.txtInfoDie.ReadOnly = true;
            this.txtInfoDie.TabIndex = 17;
            // lblInfoPass / txtInfoPass
            this.lblInfoPass.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblInfoPass.AutoSize = true;
            this.lblInfoPass.Name = "lblInfoPass";
            this.lblInfoPass.TabIndex = 18;
            this.lblInfoPass.Text = "Pass Die";
            this.txtInfoPass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfoPass.Name = "txtInfoPass";
            this.txtInfoPass.ReadOnly = true;
            this.txtInfoPass.TabIndex = 19;
            // lblInfoFail / txtInfoFail
            this.lblInfoFail.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblInfoFail.AutoSize = true;
            this.lblInfoFail.Name = "lblInfoFail";
            this.lblInfoFail.TabIndex = 20;
            this.lblInfoFail.Text = "Fail Die";
            this.txtInfoFail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfoFail.Name = "txtInfoFail";
            this.txtInfoFail.ReadOnly = true;
            this.txtInfoFail.TabIndex = 21;
            // lblInfoSheet / txtInfoSheet
            this.lblInfoSheet.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblInfoSheet.AutoSize = true;
            this.lblInfoSheet.Name = "lblInfoSheet";
            this.lblInfoSheet.TabIndex = 22;
            this.lblInfoSheet.Text = "시트";
            this.txtInfoSheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfoSheet.Name = "txtInfoSheet";
            this.txtInfoSheet.ReadOnly = true;
            this.txtInfoSheet.TabIndex = 23;
            // lblInfoItems / txtInfoItems
            this.lblInfoItems.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblInfoItems.AutoSize = true;
            this.lblInfoItems.Name = "lblInfoItems";
            this.lblInfoItems.TabIndex = 24;
            this.lblInfoItems.Text = "측정 항목";
            this.txtInfoItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfoItems.Name = "txtInfoItems";
            this.txtInfoItems.ReadOnly = true;
            this.txtInfoItems.TabIndex = 25;
            // lblInfoValues / txtInfoValues
            this.lblInfoValues.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblInfoValues.AutoSize = true;
            this.lblInfoValues.Name = "lblInfoValues";
            this.lblInfoValues.TabIndex = 26;
            this.lblInfoValues.Text = "측정값";
            this.txtInfoValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfoValues.Name = "txtInfoValues";
            this.txtInfoValues.ReadOnly = true;
            this.txtInfoValues.TabIndex = 27;
            // lblInfoNotTested / txtInfoNotTested
            this.lblInfoNotTested.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblInfoNotTested.AutoSize = true;
            this.lblInfoNotTested.Name = "lblInfoNotTested";
            this.lblInfoNotTested.TabIndex = 28;
            this.lblInfoNotTested.Text = "'-' 값";
            this.txtInfoNotTested.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfoNotTested.Name = "txtInfoNotTested";
            this.txtInfoNotTested.ReadOnly = true;
            this.txtInfoNotTested.TabIndex = 29;
            // lblInfoEmpty / txtInfoEmpty
            this.lblInfoEmpty.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblInfoEmpty.AutoSize = true;
            this.lblInfoEmpty.Name = "lblInfoEmpty";
            this.lblInfoEmpty.TabIndex = 30;
            this.lblInfoEmpty.Text = "빈 칸";
            this.txtInfoEmpty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfoEmpty.Name = "txtInfoEmpty";
            this.txtInfoEmpty.ReadOnly = true;
            this.txtInfoEmpty.TabIndex = 31;
            // txtWarning: 분석 경고 (없으면 "경고 없음")
            this.txtWarning.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtWarning.ForeColor = System.Drawing.Color.Firebrick;
            this.txtWarning.Location = new System.Drawing.Point(3, 125);
            this.txtWarning.Multiline = true;
            this.txtWarning.Name = "txtWarning";
            this.txtWarning.ReadOnly = true;
            this.txtWarning.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtWarning.Size = new System.Drawing.Size(1094, 32);
            this.txtWarning.TabIndex = 1;
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
            this.Controls.Add(this.grpInfo);
            this.Controls.Add(this.topBar);
            this.Name = "frmEtResultUpload";
            this.Text = "E-Test 결과 업로드";
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.tlpInfo.ResumeLayout(false);
            this.tlpInfo.PerformLayout();
            this.grpInfo.ResumeLayout(false);
            this.grpInfo.PerformLayout();
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
        private System.Windows.Forms.TextBox txtDevice;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblSlot;
        private System.Windows.Forms.TextBox txtSlot;
        private System.Windows.Forms.Label lblWafer;
        private System.Windows.Forms.GroupBox grpInfo;
        private System.Windows.Forms.TableLayoutPanel tlpInfo;
        private System.Windows.Forms.TextBox txtWarning;
        private System.Windows.Forms.Label lblInfoCustomer;
        private System.Windows.Forms.TextBox txtInfoCustomer;
        private System.Windows.Forms.Label lblInfoWaferType;
        private System.Windows.Forms.TextBox txtInfoWaferType;
        private System.Windows.Forms.Label lblInfoWaferRev;
        private System.Windows.Forms.TextBox txtInfoWaferRev;
        private System.Windows.Forms.Label lblInfoFab;
        private System.Windows.Forms.TextBox txtInfoFab;
        private System.Windows.Forms.Label lblInfoTestType;
        private System.Windows.Forms.TextBox txtInfoTestType;
        private System.Windows.Forms.Label lblInfoTestDate;
        private System.Windows.Forms.TextBox txtInfoTestDate;
        private System.Windows.Forms.Label lblInfoLot;
        private System.Windows.Forms.TextBox txtInfoLot;
        private System.Windows.Forms.Label lblInfoLotWafer;
        private System.Windows.Forms.TextBox txtInfoLotWafer;
        private System.Windows.Forms.Label lblInfoDie;
        private System.Windows.Forms.TextBox txtInfoDie;
        private System.Windows.Forms.Label lblInfoPass;
        private System.Windows.Forms.TextBox txtInfoPass;
        private System.Windows.Forms.Label lblInfoFail;
        private System.Windows.Forms.TextBox txtInfoFail;
        private System.Windows.Forms.Label lblInfoSheet;
        private System.Windows.Forms.TextBox txtInfoSheet;
        private System.Windows.Forms.Label lblInfoItems;
        private System.Windows.Forms.TextBox txtInfoItems;
        private System.Windows.Forms.Label lblInfoValues;
        private System.Windows.Forms.TextBox txtInfoValues;
        private System.Windows.Forms.Label lblInfoNotTested;
        private System.Windows.Forms.TextBox txtInfoNotTested;
        private System.Windows.Forms.Label lblInfoEmpty;
        private System.Windows.Forms.TextBox txtInfoEmpty;
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
