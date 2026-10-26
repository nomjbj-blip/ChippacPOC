namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// GDS-KLARF 연관 분석 Simple 화면 배치.
    /// 설계: 문서/설계/05_연관분석/2026-10-22_GDS_KLARF_연관분석_Simple_화면_설계.md 3절
    /// </summary>
    partial class frmGdsKlarfSimple
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>화면이 닫힐 때 컨트롤과 색 구간 팝업을 함께 해제한다.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (disposing)
            {
                _bandPopup.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 위: 분석 Wafer / GDS Device (읽기 전용, DMSearch 에서 Wafer 1장, Device 는 Wafer Product 를 따른다) + 오른쪽 진행 표시(작업 중에만)
        /// 왼쪽: Wafer Map (위 막대 [색 구간 설정] = 색 구간 팝업)
        /// 오른쪽: 탭 없이 한 페이지(pnlSetting) = 분석 정보 / Defect 영역 설정 + Chain 면 / KLARF 위치 조정 + [다시 분석] / 종합(Die 표) · Defect 이미지(위아래 나누기),
        ///         기능 사이에는 구분선(lineArea / lineShift / lineDie / lineImage). Chain 목록은 2026-10-22 에 뺐다
        /// 아래 상태줄 / Map 위 색 기준 글자는 두지 않는다 (2026-10-22 레이아웃 정리). 분석 정보는 제목 줄(설정 바뀜 / 알림)만 남겼다(2026-10-22)
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGdsKlarfSimple));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblWafer = new System.Windows.Forms.Label();
            this.lblWaferInfo = new System.Windows.Forms.Label();
            this.lblDevice = new System.Windows.Forms.Label();
            this.lblDeviceValue = new System.Windows.Forms.Label();
            this.busyBar = new DACrux.SEMDMS.Control.ProgressStatusBar();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.map = new DACrux.SEMDMS.ENGUI.GdsKlarfAnalysisMap();
            this.pnlMapHead = new System.Windows.Forms.Panel();
            this.lblMapTitle = new System.Windows.Forms.Label();
            this.lblMapDie = new System.Windows.Forms.Label();
            this.btnBand = new System.Windows.Forms.Button();
            this.pnlSetting = new System.Windows.Forms.Panel();
            this.splitSetting = new System.Windows.Forms.SplitContainer();
            this.flowImage = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlImageHead = new System.Windows.Forms.Panel();
            this.lblImageInfo = new System.Windows.Forms.Label();
            this.lblImageTitle = new System.Windows.Forms.Label();
            this.lineImage = new System.Windows.Forms.Panel();
            this.lineDie = new System.Windows.Forms.Panel();
            this.pnlShift = new System.Windows.Forms.Panel();
            this.lblShiftTitle = new System.Windows.Forms.Label();
            this.lblShiftX = new System.Windows.Forms.Label();
            this.numShiftX = new System.Windows.Forms.NumericUpDown();
            this.lblShiftY = new System.Windows.Forms.Label();
            this.numShiftY = new System.Windows.Forms.NumericUpDown();
            this.btnShiftLeft = new System.Windows.Forms.Button();
            this.btnShiftUp = new System.Windows.Forms.Button();
            this.btnShiftDown = new System.Windows.Forms.Button();
            this.btnShiftRight = new System.Windows.Forms.Button();
            this.btnShiftReset = new System.Windows.Forms.Button();
            this.btnAnalyze = new System.Windows.Forms.Button();
            this.lineShift = new System.Windows.Forms.Panel();
            this.pnlArea = new System.Windows.Forms.Panel();
            this.lblAreaTitle = new System.Windows.Forms.Label();
            this.rdoDefectSize = new System.Windows.Forms.RadioButton();
            this.numTolerance = new System.Windows.Forms.NumericUpDown();
            this.rdoSquare = new System.Windows.Forms.RadioButton();
            this.numSquare = new System.Windows.Forms.NumericUpDown();
            this.lblChainSide = new System.Windows.Forms.Label();
            this.cboChainSide = new System.Windows.Forms.ComboBox();
            this.lineArea = new System.Windows.Forms.Panel();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.lblInfoTitle = new System.Windows.Forms.Label();
            this.lblChanged = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.dgvDie = new System.Windows.Forms.DataGridView();
            this.pnlDieHead = new System.Windows.Forms.Panel();
            this.lblDieCount = new System.Windows.Forms.Label();
            this.lblDieTitle = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.pnlMapHead.SuspendLayout();
            this.pnlSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitSetting)).BeginInit();
            this.splitSetting.Panel1.SuspendLayout();
            this.splitSetting.Panel2.SuspendLayout();
            this.splitSetting.SuspendLayout();
            this.pnlImageHead.SuspendLayout();
            this.pnlShift.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numShiftX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numShiftY)).BeginInit();
            this.pnlArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTolerance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSquare)).BeginInit();
            this.pnlInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDie)).BeginInit();
            this.pnlDieHead.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(242)))), ((int)(((byte)(250)))));
            this.pnlTop.Controls.Add(this.lblWafer);
            this.pnlTop.Controls.Add(this.lblWaferInfo);
            this.pnlTop.Controls.Add(this.lblDevice);
            this.pnlTop.Controls.Add(this.lblDeviceValue);
            this.pnlTop.Controls.Add(this.busyBar);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(4);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(2000, 60);
            this.pnlTop.TabIndex = 0;
            // 
            // lblWafer
            // 
            this.lblWafer.AutoSize = true;
            this.lblWafer.Location = new System.Drawing.Point(14, 21);
            this.lblWafer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWafer.Name = "lblWafer";
            this.lblWafer.Size = new System.Drawing.Size(96, 18);
            this.lblWafer.TabIndex = 0;
            this.lblWafer.Text = "분석 Wafer";
            // 
            // lblWaferInfo
            // 
            this.lblWaferInfo.AutoSize = true;
            this.lblWaferInfo.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblWaferInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(60)))), ((int)(((byte)(130)))));
            this.lblWaferInfo.Location = new System.Drawing.Point(109, 16);
            this.lblWaferInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWaferInfo.Name = "lblWaferInfo";
            this.lblWaferInfo.Size = new System.Drawing.Size(20, 28);
            this.lblWaferInfo.TabIndex = 1;
            this.lblWaferInfo.Text = "-";
            // 
            // lblDevice
            // 
            this.lblDevice.AutoSize = true;
            this.lblDevice.Location = new System.Drawing.Point(1086, 21);
            this.lblDevice.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(102, 18);
            this.lblDevice.TabIndex = 2;
            this.lblDevice.Text = "GDS Device";
            // 
            // lblDeviceValue
            // 
            this.lblDeviceValue.AutoSize = true;
            this.lblDeviceValue.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblDeviceValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(60)))), ((int)(((byte)(130)))));
            this.lblDeviceValue.Location = new System.Drawing.Point(1191, 16);
            this.lblDeviceValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDeviceValue.Name = "lblDeviceValue";
            this.lblDeviceValue.Size = new System.Drawing.Size(20, 28);
            this.lblDeviceValue.TabIndex = 3;
            this.lblDeviceValue.Text = "-";
            // 
            // busyBar
            // 
            this.busyBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.busyBar.Location = new System.Drawing.Point(1500, 0);
            this.busyBar.Margin = new System.Windows.Forms.Padding(4);
            this.busyBar.Name = "busyBar";
            this.busyBar.Padding = new System.Windows.Forms.Padding(0, 4, 9, 4);
            this.busyBar.Size = new System.Drawing.Size(500, 60);
            this.busyBar.TabIndex = 4;
            this.busyBar.Visible = false;
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 60);
            this.splitMain.Margin = new System.Windows.Forms.Padding(4);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.map);
            this.splitMain.Panel1.Controls.Add(this.pnlMapHead);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.pnlSetting);
            this.splitMain.Size = new System.Drawing.Size(2000, 1290);
            this.splitMain.SplitterDistance = 1085;
            this.splitMain.SplitterWidth = 6;
            this.splitMain.TabIndex = 1;
            // 
            // map
            // 
            this.map.AngleOffSet = 0;
            this.map.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.map.CenterMark = false;
            this.map.Cursor = System.Windows.Forms.Cursors.Cross;
            this.map.DataSource = null;
            this.map.DefectSize = 7F;
            this.map.DieBackgroundColor = System.Drawing.Color.Black;
            this.map.DieBorderColor = System.Drawing.Color.LightGray;
            this.map.DieDefectColor = System.Drawing.Color.Empty;
            this.map.DieFocusingType = DACrux.Map.FocusType.Arraw;
            this.map.DieMaxX = -65536;
            this.map.DieMaxY = -65536;
            this.map.DieMinX = 65536;
            this.map.DieMinY = 65536;
            this.map.DieSizeX = 0.01D;
            this.map.DieSizeY = 0.01D;
            this.map.DisplayDieValue = DACrux.Base.DieDisplayValue.Bin;
            this.map.DisplayValue = "BIN";
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.DrawDefectImage = null;
            this.map.DrawDefects = "ALL";
            this.map.DrawFirstDie = true;
            this.map.DrawMarkDie = true;
            this.map.DrawOriginDie = true;
            this.map.DrawSkipDie = true;
            this.map.EdgeColor = System.Drawing.Color.LightGray;
            this.map.EdgeSize = 1D;
            this.map.FirstDieBorderColor = System.Drawing.Color.SkyBlue;
            this.map.FirstDieX = 0;
            this.map.FirstDieY = 0;
            this.map.ForeColor = System.Drawing.Color.Red;
            this.map.FromGradationDieColor = System.Drawing.Color.Lime;
            this.map.GradationInterval = 5;
            this.map.GradationMaxValue = double.NaN;
            this.map.GradationMinValue = double.NaN;
            this.map.IndexOffset = new System.Drawing.Point(0, 0);
            this.map.Location = new System.Drawing.Point(0, 54);
            this.map.MapType = DACrux.Base.MAP_TYPE.CLASS;
            this.map.Margin = new System.Windows.Forms.Padding(6);
            this.map.MarkDieColor = System.Drawing.Color.LightSkyBlue;
            this.map.Name = "map";
            this.map.NotchAngle = 0;
            this.map.NotchType = DACrux.Base.Notch.Flat;
            this.map.OriginDieBorder = System.Drawing.Color.Red;
            this.map.OriginIndexX = 0;
            this.map.OriginIndexY = 0;
            this.map.OriginX = 0D;
            this.map.OriginY = 0D;
            this.map.ParaLimit = false;
            this.map.ParametricColumn = "PCMVALUE";
            this.map.ParaValueFont = new System.Drawing.Font("굴림", 9F);
            this.map.PickupDieAlpha = 96;
            this.map.PickupedDieColor = System.Drawing.Color.Transparent;
            this.map.PopupMenu = true;
            this.map.ReferenceDieSetting = 0;
            this.map.ScaleMark = false;
            this.map.SelecetedBin = "ALL";
            this.map.SelectedVI = "ALL";
            this.map.ShowDieCount = true;
            this.map.ShowDieIndexLabel = false;
            this.map.Size = new System.Drawing.Size(1085, 1236);
            this.map.SizeColor = null;
            this.map.SkipDieColor = System.Drawing.Color.Yellow;
            this.map.TabIndex = 0;
            this.map.ToGradationDieColor = System.Drawing.Color.Red;
            this.map.TransParent = 255;
            this.map.TypeColor = null;
            this.map.ViewAngle = 0;
            this.map.VIMember = "VIFAIL";
            this.map.VisibleDieBorder = true;
            this.map.VisibleDieValue = false;
            this.map.VisibleFocusDie = false;
            this.map.VisibleImageMark = false;
            this.map.VisibleInfomation = true;
            this.map.VisibleOffDie = false;
            this.map.VisibleProbeOverlay = false;
            this.map.VisibleShotAlignPoint = false;
            this.map.VisibleSignDies = false;
            this.map.VisibleStringBin = false;
            this.map.VisibleVIFail = false;
            this.map.VisibleXY = false;
            this.map.WaferBorderColor = System.Drawing.Color.LightGray;
            this.map.WaferColor = System.Drawing.Color.Gray;
            this.map.WaferDrawMode = DACrux.Map.MapMode.Free;
            this.map.WaferID = "";
            this.map.WaferMargin = 0.95D;
            this.map.WaferSize = 200D;
            this.map.XYDirect = DACrux.Base.XYDirection.LeftTop;
            this.map.DieClicked += new System.EventHandler<DACrux.SEMDMS.ENGUI.GdsKlarfDieEventArgs>(this.map_DieClicked);
            this.map.DieDoubleClicked += new System.EventHandler<DACrux.SEMDMS.ENGUI.GdsKlarfDieEventArgs>(this.map_DieDoubleClicked);
            this.map.DetailAnalysisRequested += new System.EventHandler<DACrux.SEMDMS.ENGUI.GdsKlarfDieEventArgs>(this.map_DetailAnalysisRequested);
            this.map.OnChangeCurrentDie += new DACrux.Map.ChangeCurrentDie(this.map_OnChangeCurrentDie);
            this.map.MousePositionCleared += new System.EventHandler(this.map_MousePositionCleared);
            //
            // pnlMapHead
            //
            this.pnlMapHead.Controls.Add(this.lblMapTitle);
            this.pnlMapHead.Controls.Add(this.lblMapDie);
            this.pnlMapHead.Controls.Add(this.btnBand);
            this.pnlMapHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMapHead.Location = new System.Drawing.Point(0, 0);
            this.pnlMapHead.Margin = new System.Windows.Forms.Padding(0);
            this.pnlMapHead.Name = "pnlMapHead";
            this.pnlMapHead.Size = new System.Drawing.Size(1085, 54);
            this.pnlMapHead.TabIndex = 1;
            // 
            // lblMapTitle
            // 
            this.lblMapTitle.AutoSize = true;
            this.lblMapTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblMapTitle.Location = new System.Drawing.Point(8, 15);
            this.lblMapTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMapTitle.Name = "lblMapTitle";
            this.lblMapTitle.Size = new System.Drawing.Size(108, 25);
            this.lblMapTitle.TabIndex = 0;
            this.lblMapTitle.Text = "Wafer Map";
            //
            // lblMapDie
            //
            this.lblMapDie.AutoSize = true;
            this.lblMapDie.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(60)))), ((int)(((byte)(130)))));
            this.lblMapDie.Location = new System.Drawing.Point(150, 15);
            this.lblMapDie.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMapDie.Name = "lblMapDie";
            this.lblMapDie.Size = new System.Drawing.Size(0, 18);
            this.lblMapDie.TabIndex = 1;
            //
            // btnBand
            // 
            this.btnBand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBand.Image = ((System.Drawing.Image)(resources.GetObject("btnBand.Image")));
            this.btnBand.Location = new System.Drawing.Point(896, 6);
            this.btnBand.Margin = new System.Windows.Forms.Padding(4);
            this.btnBand.Name = "btnBand";
            this.btnBand.Size = new System.Drawing.Size(180, 42);
            this.btnBand.TabIndex = 2;
            this.btnBand.Text = "색 구간 설정";
            this.btnBand.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBand.UseVisualStyleBackColor = true;
            this.btnBand.Click += new System.EventHandler(this.btnBand_Click);
            //
            // pnlSetting
            //
            this.pnlSetting.BackColor = System.Drawing.Color.White;
            this.pnlSetting.Controls.Add(this.splitSetting);
            this.pnlSetting.Controls.Add(this.pnlShift);
            this.pnlSetting.Controls.Add(this.lineShift);
            this.pnlSetting.Controls.Add(this.pnlArea);
            this.pnlSetting.Controls.Add(this.lineArea);
            this.pnlSetting.Controls.Add(this.pnlInfo);
            this.pnlSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSetting.Location = new System.Drawing.Point(0, 0);
            this.pnlSetting.Margin = new System.Windows.Forms.Padding(4);
            this.pnlSetting.Name = "pnlSetting";
            this.pnlSetting.Size = new System.Drawing.Size(909, 1290);
            this.pnlSetting.TabIndex = 0;
            //
            // splitSetting
            //
            this.splitSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitSetting.Location = new System.Drawing.Point(0, 460);
            this.splitSetting.Margin = new System.Windows.Forms.Padding(4);
            this.splitSetting.Name = "splitSetting";
            this.splitSetting.Orientation = System.Windows.Forms.Orientation.Horizontal;
            //
            // splitSetting.Panel1
            //
            this.splitSetting.Panel1.Controls.Add(this.dgvDie);
            this.splitSetting.Panel1.Controls.Add(this.pnlDieHead);
            this.splitSetting.Panel1.Controls.Add(this.lineDie);
            //
            // splitSetting.Panel2
            //
            this.splitSetting.Panel2.Controls.Add(this.flowImage);
            this.splitSetting.Panel2.Controls.Add(this.pnlImageHead);
            this.splitSetting.Panel2.Controls.Add(this.lineImage);
            this.splitSetting.Size = new System.Drawing.Size(909, 830);
            this.splitSetting.SplitterDistance = 471;
            this.splitSetting.SplitterWidth = 6;
            this.splitSetting.TabIndex = 2;
            // 
            // flowImage
            // 
            this.flowImage.AutoScroll = true;
            this.flowImage.BackColor = System.Drawing.Color.White;
            this.flowImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowImage.Location = new System.Drawing.Point(0, 44);
            this.flowImage.Margin = new System.Windows.Forms.Padding(4);
            this.flowImage.Name = "flowImage";
            this.flowImage.Size = new System.Drawing.Size(901, 427);
            this.flowImage.TabIndex = 1;
            // 
            // pnlImageHead
            // 
            this.pnlImageHead.Controls.Add(this.lblImageInfo);
            this.pnlImageHead.Controls.Add(this.lblImageTitle);
            this.pnlImageHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlImageHead.Location = new System.Drawing.Point(0, 2);
            this.pnlImageHead.Margin = new System.Windows.Forms.Padding(4);
            this.pnlImageHead.Name = "pnlImageHead";
            this.pnlImageHead.Padding = new System.Windows.Forms.Padding(8, 0, 12, 0);
            this.pnlImageHead.Size = new System.Drawing.Size(901, 42);
            this.pnlImageHead.TabIndex = 0;
            // 
            // lblImageInfo
            // 
            this.lblImageInfo.AutoEllipsis = true;
            this.lblImageInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblImageInfo.ForeColor = System.Drawing.Color.DimGray;
            this.lblImageInfo.Location = new System.Drawing.Point(136, 0);
            this.lblImageInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblImageInfo.Name = "lblImageInfo";
            this.lblImageInfo.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.lblImageInfo.Size = new System.Drawing.Size(753, 42);
            this.lblImageInfo.TabIndex = 1;
            this.lblImageInfo.Text = "선택 Die 없음";
            this.lblImageInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblImageTitle
            // 
            this.lblImageTitle.AutoSize = true;
            this.lblImageTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblImageTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblImageTitle.Location = new System.Drawing.Point(8, 0);
            this.lblImageTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblImageTitle.Name = "lblImageTitle";
            this.lblImageTitle.Size = new System.Drawing.Size(128, 25);
            this.lblImageTitle.TabIndex = 0;
            this.lblImageTitle.Text = "Defect 이미지";
            this.lblImageTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lineImage
            // 
            this.lineImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(200)))), ((int)(((byte)(214)))));
            this.lineImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.lineImage.Location = new System.Drawing.Point(0, 0);
            this.lineImage.Margin = new System.Windows.Forms.Padding(0);
            this.lineImage.Name = "lineImage";
            this.lineImage.Size = new System.Drawing.Size(901, 2);
            this.lineImage.TabIndex = 2;
            //
            // lineDie
            //
            this.lineDie.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(200)))), ((int)(((byte)(214)))));
            this.lineDie.Dock = System.Windows.Forms.DockStyle.Top;
            this.lineDie.Location = new System.Drawing.Point(0, 0);
            this.lineDie.Margin = new System.Windows.Forms.Padding(0);
            this.lineDie.Name = "lineDie";
            this.lineDie.Size = new System.Drawing.Size(909, 2);
            this.lineDie.TabIndex = 2;
            // 
            // pnlShift
            // 
            this.pnlShift.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(253)))));
            this.pnlShift.Controls.Add(this.lblShiftTitle);
            this.pnlShift.Controls.Add(this.lblShiftX);
            this.pnlShift.Controls.Add(this.numShiftX);
            this.pnlShift.Controls.Add(this.lblShiftY);
            this.pnlShift.Controls.Add(this.numShiftY);
            this.pnlShift.Controls.Add(this.btnShiftLeft);
            this.pnlShift.Controls.Add(this.btnShiftUp);
            this.pnlShift.Controls.Add(this.btnShiftDown);
            this.pnlShift.Controls.Add(this.btnShiftRight);
            this.pnlShift.Controls.Add(this.btnShiftReset);
            this.pnlShift.Controls.Add(this.btnAnalyze);
            this.pnlShift.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlShift.Location = new System.Drawing.Point(0, 360);
            this.pnlShift.Margin = new System.Windows.Forms.Padding(4);
            this.pnlShift.Name = "pnlShift";
            this.pnlShift.Size = new System.Drawing.Size(901, 100);
            this.pnlShift.TabIndex = 5;
            // 
            // lblShiftTitle
            // 
            this.lblShiftTitle.AutoSize = true;
            this.lblShiftTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblShiftTitle.Location = new System.Drawing.Point(8, 9);
            this.lblShiftTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblShiftTitle.Name = "lblShiftTitle";
            this.lblShiftTitle.Size = new System.Drawing.Size(151, 25);
            this.lblShiftTitle.TabIndex = 0;
            this.lblShiftTitle.Text = "KLARF 위치 조정";
            // 
            // lblShiftX
            // 
            this.lblShiftX.AutoSize = true;
            this.lblShiftX.Location = new System.Drawing.Point(18, 54);
            this.lblShiftX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblShiftX.Name = "lblShiftX";
            this.lblShiftX.Size = new System.Drawing.Size(19, 18);
            this.lblShiftX.TabIndex = 1;
            this.lblShiftX.Text = "X";
            // 
            // numShiftX
            // 
            this.numShiftX.DecimalPlaces = 1;
            this.numShiftX.Location = new System.Drawing.Point(44, 48);
            this.numShiftX.Margin = new System.Windows.Forms.Padding(4);
            this.numShiftX.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numShiftX.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.numShiftX.Name = "numShiftX";
            this.numShiftX.Size = new System.Drawing.Size(110, 28);
            this.numShiftX.TabIndex = 2;
            this.numShiftX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numShiftX.ValueChanged += new System.EventHandler(this.Shift_ValueChanged);
            // 
            // lblShiftY
            // 
            this.lblShiftY.AutoSize = true;
            this.lblShiftY.Location = new System.Drawing.Point(176, 54);
            this.lblShiftY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblShiftY.Name = "lblShiftY";
            this.lblShiftY.Size = new System.Drawing.Size(19, 18);
            this.lblShiftY.TabIndex = 3;
            this.lblShiftY.Text = "Y";
            // 
            // numShiftY
            // 
            this.numShiftY.DecimalPlaces = 1;
            this.numShiftY.Location = new System.Drawing.Point(202, 48);
            this.numShiftY.Margin = new System.Windows.Forms.Padding(4);
            this.numShiftY.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numShiftY.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.numShiftY.Name = "numShiftY";
            this.numShiftY.Size = new System.Drawing.Size(110, 28);
            this.numShiftY.TabIndex = 4;
            this.numShiftY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numShiftY.ValueChanged += new System.EventHandler(this.Shift_ValueChanged);
            // 
            // btnShiftLeft
            // 
            this.btnShiftLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftLeft.Image")));
            this.btnShiftLeft.Location = new System.Drawing.Point(340, 46);
            this.btnShiftLeft.Margin = new System.Windows.Forms.Padding(4);
            this.btnShiftLeft.Name = "btnShiftLeft";
            this.btnShiftLeft.Size = new System.Drawing.Size(42, 42);
            this.btnShiftLeft.TabIndex = 5;
            this.btnShiftLeft.UseVisualStyleBackColor = true;
            this.btnShiftLeft.Click += new System.EventHandler(this.ShiftButton_Click);
            // 
            // btnShiftUp
            // 
            this.btnShiftUp.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftUp.Image")));
            this.btnShiftUp.Location = new System.Drawing.Point(386, 46);
            this.btnShiftUp.Margin = new System.Windows.Forms.Padding(4);
            this.btnShiftUp.Name = "btnShiftUp";
            this.btnShiftUp.Size = new System.Drawing.Size(42, 42);
            this.btnShiftUp.TabIndex = 6;
            this.btnShiftUp.UseVisualStyleBackColor = true;
            this.btnShiftUp.Click += new System.EventHandler(this.ShiftButton_Click);
            // 
            // btnShiftDown
            // 
            this.btnShiftDown.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftDown.Image")));
            this.btnShiftDown.Location = new System.Drawing.Point(432, 46);
            this.btnShiftDown.Margin = new System.Windows.Forms.Padding(4);
            this.btnShiftDown.Name = "btnShiftDown";
            this.btnShiftDown.Size = new System.Drawing.Size(42, 42);
            this.btnShiftDown.TabIndex = 7;
            this.btnShiftDown.UseVisualStyleBackColor = true;
            this.btnShiftDown.Click += new System.EventHandler(this.ShiftButton_Click);
            // 
            // btnShiftRight
            // 
            this.btnShiftRight.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftRight.Image")));
            this.btnShiftRight.Location = new System.Drawing.Point(478, 46);
            this.btnShiftRight.Margin = new System.Windows.Forms.Padding(4);
            this.btnShiftRight.Name = "btnShiftRight";
            this.btnShiftRight.Size = new System.Drawing.Size(42, 42);
            this.btnShiftRight.TabIndex = 8;
            this.btnShiftRight.UseVisualStyleBackColor = true;
            this.btnShiftRight.Click += new System.EventHandler(this.ShiftButton_Click);
            // 
            // btnShiftReset
            // 
            this.btnShiftReset.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftReset.Image")));
            this.btnShiftReset.Location = new System.Drawing.Point(540, 46);
            this.btnShiftReset.Margin = new System.Windows.Forms.Padding(4);
            this.btnShiftReset.Name = "btnShiftReset";
            this.btnShiftReset.Size = new System.Drawing.Size(120, 42);
            this.btnShiftReset.TabIndex = 9;
            this.btnShiftReset.Text = "초기화";
            this.btnShiftReset.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnShiftReset.UseVisualStyleBackColor = true;
            this.btnShiftReset.Click += new System.EventHandler(this.btnShiftReset_Click);
            // 
            // btnAnalyze
            // 
            this.btnAnalyze.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAnalyze.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.btnAnalyze.Image = ((System.Drawing.Image)(resources.GetObject("btnAnalyze.Image")));
            this.btnAnalyze.Location = new System.Drawing.Point(737, 46);
            this.btnAnalyze.Margin = new System.Windows.Forms.Padding(4);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Size = new System.Drawing.Size(150, 42);
            this.btnAnalyze.TabIndex = 11;
            this.btnAnalyze.Text = "다시 분석";
            this.btnAnalyze.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAnalyze.UseVisualStyleBackColor = true;
            this.btnAnalyze.Click += new System.EventHandler(this.btnAnalyze_Click);
            // 
            // lineShift
            // 
            this.lineShift.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(200)))), ((int)(((byte)(214)))));
            this.lineShift.Dock = System.Windows.Forms.DockStyle.Top;
            this.lineShift.Location = new System.Drawing.Point(0, 358);
            this.lineShift.Margin = new System.Windows.Forms.Padding(0);
            this.lineShift.Name = "lineShift";
            this.lineShift.Size = new System.Drawing.Size(901, 2);
            this.lineShift.TabIndex = 4;
            // 
            // pnlArea
            // 
            this.pnlArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(253)))));
            this.pnlArea.Controls.Add(this.lblAreaTitle);
            this.pnlArea.Controls.Add(this.rdoDefectSize);
            this.pnlArea.Controls.Add(this.numTolerance);
            this.pnlArea.Controls.Add(this.rdoSquare);
            this.pnlArea.Controls.Add(this.numSquare);
            this.pnlArea.Controls.Add(this.lblChainSide);
            this.pnlArea.Controls.Add(this.cboChainSide);
            this.pnlArea.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlArea.Location = new System.Drawing.Point(0, 182);
            this.pnlArea.Margin = new System.Windows.Forms.Padding(4);
            this.pnlArea.Name = "pnlArea";
            this.pnlArea.Size = new System.Drawing.Size(901, 176);
            this.pnlArea.TabIndex = 1;
            // 
            // lblAreaTitle
            // 
            this.lblAreaTitle.AutoSize = true;
            this.lblAreaTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblAreaTitle.Location = new System.Drawing.Point(8, 9);
            this.lblAreaTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAreaTitle.Name = "lblAreaTitle";
            this.lblAreaTitle.Size = new System.Drawing.Size(152, 25);
            this.lblAreaTitle.TabIndex = 0;
            this.lblAreaTitle.Text = "Defect 영역 설정";
            // 
            // rdoDefectSize
            // 
            this.rdoDefectSize.AutoSize = true;
            this.rdoDefectSize.Checked = true;
            this.rdoDefectSize.Location = new System.Drawing.Point(18, 48);
            this.rdoDefectSize.Margin = new System.Windows.Forms.Padding(4);
            this.rdoDefectSize.Name = "rdoDefectSize";
            this.rdoDefectSize.Size = new System.Drawing.Size(219, 22);
            this.rdoDefectSize.TabIndex = 1;
            this.rdoDefectSize.TabStop = true;
            this.rdoDefectSize.Text = "Defect 크기 + 허용오차";
            this.rdoDefectSize.UseVisualStyleBackColor = true;
            this.rdoDefectSize.CheckedChanged += new System.EventHandler(this.DefectArea_CheckedChanged);
            // 
            // numTolerance
            // 
            this.numTolerance.DecimalPlaces = 1;
            this.numTolerance.Location = new System.Drawing.Point(260, 45);
            this.numTolerance.Margin = new System.Windows.Forms.Padding(4);
            this.numTolerance.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numTolerance.Name = "numTolerance";
            this.numTolerance.Size = new System.Drawing.Size(100, 28);
            this.numTolerance.TabIndex = 2;
            this.numTolerance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numTolerance.ValueChanged += new System.EventHandler(this.Condition_Changed);
            // 
            // rdoSquare
            // 
            this.rdoSquare.AutoSize = true;
            this.rdoSquare.Location = new System.Drawing.Point(18, 90);
            this.rdoSquare.Margin = new System.Windows.Forms.Padding(4);
            this.rdoSquare.Name = "rdoSquare";
            this.rdoSquare.Size = new System.Drawing.Size(204, 22);
            this.rdoSquare.TabIndex = 4;
            this.rdoSquare.Text = "Defect 중심 정사각형";
            this.rdoSquare.UseVisualStyleBackColor = true;
            this.rdoSquare.CheckedChanged += new System.EventHandler(this.DefectArea_CheckedChanged);
            // 
            // numSquare
            // 
            this.numSquare.DecimalPlaces = 1;
            this.numSquare.Enabled = false;
            this.numSquare.Location = new System.Drawing.Point(260, 87);
            this.numSquare.Margin = new System.Windows.Forms.Padding(4);
            this.numSquare.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numSquare.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numSquare.Name = "numSquare";
            this.numSquare.Size = new System.Drawing.Size(100, 28);
            this.numSquare.TabIndex = 5;
            this.numSquare.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numSquare.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numSquare.ValueChanged += new System.EventHandler(this.Condition_Changed);
            // 
            // lblChainSide
            // 
            this.lblChainSide.AutoSize = true;
            this.lblChainSide.Location = new System.Drawing.Point(18, 140);
            this.lblChainSide.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblChainSide.Name = "lblChainSide";
            this.lblChainSide.Size = new System.Drawing.Size(117, 18);
            this.lblChainSide.TabIndex = 7;
            this.lblChainSide.Text = "GDS Chain 면";
            // 
            // cboChainSide
            // 
            this.cboChainSide.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboChainSide.FormattingEnabled = true;
            this.cboChainSide.Items.AddRange(new object[] {
            "FS",
            "BS"});
            this.cboChainSide.Location = new System.Drawing.Point(150, 135);
            this.cboChainSide.Margin = new System.Windows.Forms.Padding(4);
            this.cboChainSide.Name = "cboChainSide";
            this.cboChainSide.Size = new System.Drawing.Size(90, 26);
            this.cboChainSide.TabIndex = 8;
            this.cboChainSide.SelectedIndexChanged += new System.EventHandler(this.Condition_Changed);
            // 
            // lineArea
            // 
            this.lineArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(200)))), ((int)(((byte)(214)))));
            this.lineArea.Dock = System.Windows.Forms.DockStyle.Top;
            this.lineArea.Location = new System.Drawing.Point(0, 42);
            this.lineArea.Margin = new System.Windows.Forms.Padding(0);
            this.lineArea.Name = "lineArea";
            this.lineArea.Size = new System.Drawing.Size(901, 2);
            this.lineArea.TabIndex = 3;
            // 
            // pnlInfo
            // 
            this.pnlInfo.Controls.Add(this.lblInfoTitle);
            this.pnlInfo.Controls.Add(this.lblChanged);
            this.pnlInfo.Controls.Add(this.lblInfo);
            this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInfo.Location = new System.Drawing.Point(0, 0);
            this.pnlInfo.Margin = new System.Windows.Forms.Padding(4);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(901, 42);
            this.pnlInfo.TabIndex = 0;
            // 
            // lblInfoTitle
            // 
            this.lblInfoTitle.AutoSize = true;
            this.lblInfoTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblInfoTitle.Location = new System.Drawing.Point(8, 9);
            this.lblInfoTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInfoTitle.Name = "lblInfoTitle";
            this.lblInfoTitle.Size = new System.Drawing.Size(90, 25);
            this.lblInfoTitle.TabIndex = 0;
            this.lblInfoTitle.Text = "분석 정보";
            // 
            // lblChanged
            // 
            this.lblChanged.AutoSize = true;
            this.lblChanged.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(123)))), ((int)(((byte)(0)))));
            this.lblChanged.Location = new System.Drawing.Point(150, 12);
            this.lblChanged.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblChanged.Name = "lblChanged";
            this.lblChanged.Size = new System.Drawing.Size(286, 18);
            this.lblChanged.TabIndex = 1;
            this.lblChanged.Text = "설정 바뀜 / 지금 결과는 이전 설정";
            this.lblChanged.Visible = false;
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo.AutoEllipsis = true;
            this.lblInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(123)))), ((int)(((byte)(0)))));
            this.lblInfo.Location = new System.Drawing.Point(150, 12);
            this.lblInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(737, 22);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.Visible = false;
            //
            // dgvDie
            // 
            this.dgvDie.AllowUserToAddRows = false;
            this.dgvDie.AllowUserToDeleteRows = false;
            this.dgvDie.AllowUserToResizeRows = false;
            this.dgvDie.BackgroundColor = System.Drawing.Color.White;
            this.dgvDie.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDie.Location = new System.Drawing.Point(0, 44);
            this.dgvDie.Margin = new System.Windows.Forms.Padding(4);
            this.dgvDie.MultiSelect = false;
            this.dgvDie.Name = "dgvDie";
            this.dgvDie.ReadOnly = true;
            this.dgvDie.RowHeadersVisible = false;
            this.dgvDie.RowHeadersWidth = 62;
            this.dgvDie.RowTemplate.Height = 23;
            this.dgvDie.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDie.Size = new System.Drawing.Size(909, 427);
            this.dgvDie.TabIndex = 1;
            this.dgvDie.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDie_CellClick);
            this.dgvDie.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDie_CellDoubleClick);
            // 
            // pnlDieHead
            // 
            this.pnlDieHead.Controls.Add(this.lblDieCount);
            this.pnlDieHead.Controls.Add(this.lblDieTitle);
            this.pnlDieHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDieHead.Location = new System.Drawing.Point(0, 2);
            this.pnlDieHead.Margin = new System.Windows.Forms.Padding(4);
            this.pnlDieHead.Name = "pnlDieHead";
            this.pnlDieHead.Padding = new System.Windows.Forms.Padding(8, 0, 12, 0);
            this.pnlDieHead.Size = new System.Drawing.Size(909, 42);
            this.pnlDieHead.TabIndex = 0;
            //
            // lblDieCount
            //
            this.lblDieCount.AutoEllipsis = true;
            this.lblDieCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDieCount.ForeColor = System.Drawing.Color.DimGray;
            this.lblDieCount.Location = new System.Drawing.Point(62, 0);
            this.lblDieCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDieCount.Name = "lblDieCount";
            this.lblDieCount.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.lblDieCount.Size = new System.Drawing.Size(835, 42);
            this.lblDieCount.TabIndex = 1;
            this.lblDieCount.Text = "Die 0";
            this.lblDieCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblDieTitle
            //
            this.lblDieTitle.AutoSize = true;
            this.lblDieTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblDieTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblDieTitle.Location = new System.Drawing.Point(8, 0);
            this.lblDieTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDieTitle.Name = "lblDieTitle";
            this.lblDieTitle.Size = new System.Drawing.Size(54, 25);
            this.lblDieTitle.TabIndex = 0;
            this.lblDieTitle.Text = "종합";
            this.lblDieTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmGdsKlarfSimple
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2000, 1350);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.pnlTop);
            this.Margin = new System.Windows.Forms.Padding(9);
            this.Name = "frmGdsKlarfSimple";
            this.Text = "GDS-KLARF 연관 분석 (Simple)";
            this.Load += new System.EventHandler(this.frmGdsKlarfSimple_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.pnlMapHead.ResumeLayout(false);
            this.pnlMapHead.PerformLayout();
            this.pnlSetting.ResumeLayout(false);
            this.splitSetting.Panel1.ResumeLayout(false);
            this.splitSetting.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitSetting)).EndInit();
            this.splitSetting.ResumeLayout(false);
            this.pnlImageHead.ResumeLayout(false);
            this.pnlImageHead.PerformLayout();
            this.pnlShift.ResumeLayout(false);
            this.pnlShift.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numShiftX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numShiftY)).EndInit();
            this.pnlArea.ResumeLayout(false);
            this.pnlArea.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTolerance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSquare)).EndInit();
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDie)).EndInit();
            this.pnlDieHead.ResumeLayout(false);
            this.pnlDieHead.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblWafer;
        private System.Windows.Forms.Label lblWaferInfo;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.Label lblDeviceValue;
        private System.Windows.Forms.SplitContainer splitMain;
        private GdsKlarfAnalysisMap map;
        private System.Windows.Forms.Panel pnlMapHead;
        private System.Windows.Forms.Label lblMapTitle;
        private System.Windows.Forms.Label lblMapDie;
        private System.Windows.Forms.Button btnBand;
        private System.Windows.Forms.Panel pnlSetting;
        private System.Windows.Forms.SplitContainer splitSetting;
        private System.Windows.Forms.FlowLayoutPanel flowImage;
        private System.Windows.Forms.Panel pnlImageHead;
        private System.Windows.Forms.Panel lineArea;
        private System.Windows.Forms.Panel lineImage;
        private System.Windows.Forms.Panel lineDie;
        private System.Windows.Forms.Label lblImageTitle;
        private System.Windows.Forms.Label lblImageInfo;
        private System.Windows.Forms.Panel pnlArea;
        private System.Windows.Forms.Label lblAreaTitle;
        private System.Windows.Forms.RadioButton rdoDefectSize;
        private System.Windows.Forms.NumericUpDown numTolerance;
        private System.Windows.Forms.RadioButton rdoSquare;
        private System.Windows.Forms.NumericUpDown numSquare;
        private System.Windows.Forms.Label lblChainSide;
        private System.Windows.Forms.ComboBox cboChainSide;
        private System.Windows.Forms.Button btnAnalyze;
        private System.Windows.Forms.Panel lineShift;
        private System.Windows.Forms.Panel pnlShift;
        private System.Windows.Forms.Label lblShiftTitle;
        private System.Windows.Forms.Label lblShiftX;
        private System.Windows.Forms.NumericUpDown numShiftX;
        private System.Windows.Forms.Label lblShiftY;
        private System.Windows.Forms.NumericUpDown numShiftY;
        private System.Windows.Forms.Button btnShiftLeft;
        private System.Windows.Forms.Button btnShiftUp;
        private System.Windows.Forms.Button btnShiftDown;
        private System.Windows.Forms.Button btnShiftRight;
        private System.Windows.Forms.Button btnShiftReset;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.Label lblInfoTitle;
        private System.Windows.Forms.Label lblChanged;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.DataGridView dgvDie;
        private System.Windows.Forms.Panel pnlDieHead;
        private System.Windows.Forms.Label lblDieCount;
        private System.Windows.Forms.Label lblDieTitle;
        private DACrux.SEMDMS.Control.ProgressStatusBar busyBar;
    }
}
