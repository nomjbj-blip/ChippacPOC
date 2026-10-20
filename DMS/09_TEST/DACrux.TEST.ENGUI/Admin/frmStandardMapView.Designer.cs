namespace DACrux.TEST.ENGUI
{
    // Wafer Map 조회 화면의 컨트롤과 배치를 정의한다. 목록 조회 / Map 표시 / 확인 표 동작은 본문 파일에서 처리한다.
    partial class frmStandardMapView
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>화면 종료 시 디자이너 구성 요소를 해제한다.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>위쪽 조회 줄 / 왼쪽 Map 목록 / 가운데 Map / 오른쪽 탭(Map 정보)을 WinForms 디자이너 구성으로 만든다.</summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStandardMapView));
            System.Windows.Forms.DataGridViewCellStyle numberStyle = new System.Windows.Forms.DataGridViewCellStyle();
            this.topBar = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblListCount = new System.Windows.Forms.Label();
            this.chkDieNumber = new System.Windows.Forms.CheckBox();
            this.pnlList = new System.Windows.Forms.Panel();
            this.grdMapList = new System.Windows.Forms.DataGridView();
            this.colMapId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWafer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNetDie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDirection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabRight = new System.Windows.Forms.TabControl();
            this.tabInfo = new System.Windows.Forms.TabPage();
            this.grdInfo = new System.Windows.Forms.DataGridView();
            this.colInfoName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInfoValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblDieCount = new System.Windows.Forms.Label();
            this.lblCoordinates = new System.Windows.Forms.Label();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.busyBar = new DACrux.SEMDMS.Control.ProgressStatusBar();
            this.m_wMap = new DACrux.TEST.ENGUI.StandardMapEditMap();
            this.topBar.SuspendLayout();
            this.pnlList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMapList)).BeginInit();
            this.tabRight.SuspendLayout();
            this.tabInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdInfo)).BeginInit();
            this.pnlStatus.SuspendLayout();
            this.SuspendLayout();
            //
            // numberStyle
            //
            numberStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            //
            // topBar
            //
            this.topBar.Controls.Add(this.btnSearch);
            this.topBar.Controls.Add(this.lblListCount);
            this.topBar.Controls.Add(this.chkDieNumber);
            this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar.Location = new System.Drawing.Point(0, 0);
            this.topBar.Name = "topBar";
            this.topBar.Size = new System.Drawing.Size(1634, 52);
            this.topBar.TabIndex = 0;
            //
            // btnSearch
            //
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.Location = new System.Drawing.Point(12, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSearch.Size = new System.Drawing.Size(88, 28);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            //
            // lblListCount
            //
            this.lblListCount.Location = new System.Drawing.Point(110, 14);
            this.lblListCount.Name = "lblListCount";
            this.lblListCount.Size = new System.Drawing.Size(110, 24);
            this.lblListCount.TabIndex = 1;
            this.lblListCount.Text = "0 건";
            this.lblListCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // chkDieNumber
            //
            this.chkDieNumber.Checked = true;
            this.chkDieNumber.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDieNumber.Location = new System.Drawing.Point(230, 12);
            this.chkDieNumber.Name = "chkDieNumber";
            this.chkDieNumber.Size = new System.Drawing.Size(150, 28);
            this.chkDieNumber.TabIndex = 2;
            this.chkDieNumber.Text = "Die 번호 표시";
            this.chkDieNumber.UseVisualStyleBackColor = true;
            this.chkDieNumber.CheckedChanged += new System.EventHandler(this.chkDieNumber_CheckedChanged);
            //
            // pnlList
            //
            this.pnlList.Controls.Add(this.grdMapList);
            this.pnlList.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlList.Location = new System.Drawing.Point(0, 52);
            this.pnlList.Name = "pnlList";
            this.pnlList.Padding = new System.Windows.Forms.Padding(8, 0, 4, 0);
            this.pnlList.Size = new System.Drawing.Size(440, 848);
            this.pnlList.TabIndex = 1;
            //
            // grdMapList
            //
            this.grdMapList.AllowUserToAddRows = false;
            this.grdMapList.AllowUserToDeleteRows = false;
            this.grdMapList.AllowUserToResizeRows = false;
            this.grdMapList.BackgroundColor = System.Drawing.Color.White;
            this.grdMapList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdMapList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMapId,
            this.colWafer,
            this.colNetDie,
            this.colDirection});
            this.grdMapList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdMapList.Location = new System.Drawing.Point(8, 0);
            this.grdMapList.MultiSelect = false;
            this.grdMapList.Name = "grdMapList";
            this.grdMapList.ReadOnly = true;
            this.grdMapList.RowHeadersVisible = false;
            this.grdMapList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdMapList.Size = new System.Drawing.Size(428, 848);
            this.grdMapList.TabIndex = 0;
            this.grdMapList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.grdMapList_DataBindingComplete);
            this.grdMapList.SelectionChanged += new System.EventHandler(this.grdMapList_SelectionChanged);
            //
            // colMapId
            //
            this.colMapId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colMapId.DataPropertyName = "MAPID";
            this.colMapId.HeaderText = "Device (Map ID)";
            this.colMapId.MinimumWidth = 120;
            this.colMapId.Name = "colMapId";
            this.colMapId.ReadOnly = true;
            //
            // colWafer
            //
            this.colWafer.DataPropertyName = "WAFER_MM";
            this.colWafer.DefaultCellStyle = numberStyle;
            this.colWafer.HeaderText = "Wafer (mm)";
            this.colWafer.Name = "colWafer";
            this.colWafer.ReadOnly = true;
            this.colWafer.Width = 70;
            //
            // colNetDie
            //
            this.colNetDie.DataPropertyName = "NETDIE";
            this.colNetDie.DefaultCellStyle = numberStyle;
            this.colNetDie.HeaderText = "Net Die";
            this.colNetDie.Name = "colNetDie";
            this.colNetDie.ReadOnly = true;
            this.colNetDie.Width = 70;
            //
            // colDirection
            //
            this.colDirection.DataPropertyName = "XY_NAME";
            this.colDirection.HeaderText = "XY 방향";
            this.colDirection.Name = "colDirection";
            this.colDirection.ReadOnly = true;
            this.colDirection.Width = 100;
            //
            // tabRight
            //
            this.tabRight.Controls.Add(this.tabInfo);
            this.tabRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.tabRight.Location = new System.Drawing.Point(1154, 52);
            this.tabRight.Name = "tabRight";
            this.tabRight.SelectedIndex = 0;
            this.tabRight.Size = new System.Drawing.Size(480, 848);
            this.tabRight.TabIndex = 4;
            //
            // tabInfo
            //
            this.tabInfo.Controls.Add(this.grdInfo);
            this.tabInfo.Location = new System.Drawing.Point(4, 34);
            this.tabInfo.Name = "tabInfo";
            this.tabInfo.Size = new System.Drawing.Size(472, 810);
            this.tabInfo.TabIndex = 0;
            this.tabInfo.Text = "Map 정보";
            this.tabInfo.UseVisualStyleBackColor = true;
            //
            // grdInfo
            //
            this.grdInfo.AllowUserToAddRows = false;
            this.grdInfo.AllowUserToDeleteRows = false;
            this.grdInfo.AllowUserToResizeRows = false;
            this.grdInfo.BackgroundColor = System.Drawing.Color.White;
            this.grdInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colInfoName,
            this.colInfoValue});
            this.grdInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdInfo.Location = new System.Drawing.Point(0, 0);
            this.grdInfo.Name = "grdInfo";
            this.grdInfo.ReadOnly = true;
            this.grdInfo.RowHeadersVisible = false;
            this.grdInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdInfo.Size = new System.Drawing.Size(472, 810);
            this.grdInfo.TabIndex = 0;
            this.grdInfo.VisibleChanged += new System.EventHandler(this.InfoGrid_VisibleChanged);
            //
            // colInfoName
            //
            this.colInfoName.HeaderText = "항목";
            this.colInfoName.Name = "colInfoName";
            this.colInfoName.ReadOnly = true;
            this.colInfoName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colInfoName.Width = 180;
            //
            // colInfoValue
            //
            this.colInfoValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colInfoValue.HeaderText = "값";
            this.colInfoValue.Name = "colInfoValue";
            this.colInfoValue.ReadOnly = true;
            this.colInfoValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            //
            // lblDieCount
            //
            this.lblDieCount.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblDieCount.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDieCount.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lblDieCount.Location = new System.Drawing.Point(440, 52);
            this.lblDieCount.Name = "lblDieCount";
            this.lblDieCount.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.lblDieCount.Size = new System.Drawing.Size(714, 36);
            this.lblDieCount.TabIndex = 3;
            this.lblDieCount.Text = "Map: -";
            this.lblDieCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblCoordinates
            //
            this.lblCoordinates.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblCoordinates.Location = new System.Drawing.Point(0, 900);
            this.lblCoordinates.Name = "lblCoordinates";
            this.lblCoordinates.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.lblCoordinates.Size = new System.Drawing.Size(1634, 26);
            this.lblCoordinates.TabIndex = 5;
            this.lblCoordinates.Text = "";
            this.lblCoordinates.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // pnlStatus
            //
            this.pnlStatus.Controls.Add(this.lblStatus);
            this.pnlStatus.Controls.Add(this.busyBar);
            this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(1300, 32);
            this.pnlStatus.TabIndex = 2;
            //
            // busyBar
            //
            this.busyBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.busyBar.Name = "busyBar";
            this.busyBar.Size = new System.Drawing.Size(420, 32);
            this.busyBar.TabIndex = 1;
            //
            // lblStatus
            //
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Location = new System.Drawing.Point(0, 952);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.lblStatus.Size = new System.Drawing.Size(1634, 32);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // m_wMap
            //
            this.m_wMap.AngleOffSet = 0;
            this.m_wMap.AutoFocus = false;
            this.m_wMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(234)))), ((int)(((byte)(216)))));
            this.m_wMap.CenterMark = true;
            this.m_wMap.CurrentFailNumber = 0;
            this.m_wMap.Cursor = System.Windows.Forms.Cursors.Cross;
            this.m_wMap.DataSource = null;
            this.m_wMap.DieBorderColor = System.Drawing.Color.White;
            this.m_wMap.DieFocusingType = DACrux.Map.FocusType.Arraw;
            this.m_wMap.DieMaxX = -65536;
            this.m_wMap.DieMaxY = -65536;
            this.m_wMap.DieMinX = 65536;
            this.m_wMap.DieMinY = 65536;
            this.m_wMap.DieSizeX = 1D;
            this.m_wMap.DieSizeY = 1D;
            this.m_wMap.DisplayDieValue = DACrux.Base.DieDisplayValue.Bin;
            this.m_wMap.DisplayValue = "BIN";
            this.m_wMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_wMap.DrawFirstDie = false;
            this.m_wMap.DrawMarkDie = false;
            this.m_wMap.DrawOriginDie = false;
            this.m_wMap.DrawSkipDie = false;
            this.m_wMap.EdgeColor = System.Drawing.Color.LightGray;
            this.m_wMap.EdgeSize = 0D;
            this.m_wMap.EditMenuVisible = false;
            this.m_wMap.EditMethod = DACrux.Map.EditMode.ADD;
            this.m_wMap.FirstDieBorderColor = System.Drawing.Color.SkyBlue;
            this.m_wMap.FirstDieX = 0;
            this.m_wMap.FirstDieY = 0;
            this.m_wMap.ForeColor = System.Drawing.Color.Red;
            this.m_wMap.FromGradationDieColor = System.Drawing.Color.Lime;
            this.m_wMap.GradationInterval = 5;
            this.m_wMap.GradationMaxValue = double.NaN;
            this.m_wMap.GradationMinValue = double.NaN;
            this.m_wMap.ImeMode = System.Windows.Forms.ImeMode.On;
            this.m_wMap.IsVIEdit = false;
            this.m_wMap.Location = new System.Drawing.Point(440, 88);
            this.m_wMap.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.m_wMap.MarkDieColor = System.Drawing.Color.LightSkyBlue;
            this.m_wMap.Name = "m_wMap";
            this.m_wMap.NotchAngle = 0;
            this.m_wMap.NotchType = DACrux.Base.Notch.Notch;
            this.m_wMap.OriginDieBorder = System.Drawing.Color.Red;
            this.m_wMap.OriginIndexX = 0;
            this.m_wMap.OriginIndexY = 0;
            this.m_wMap.OriginX = 0D;
            this.m_wMap.OriginY = 0D;
            this.m_wMap.ParaLimit = false;
            this.m_wMap.ParametricColumn = "PCMVALUE";
            this.m_wMap.ParaValueFont = new System.Drawing.Font("굴림", 9F);
            this.m_wMap.PickupDieAlpha = 96;
            this.m_wMap.PickupedDieColor = System.Drawing.Color.Transparent;
            this.m_wMap.PopupMenu = true;
            this.m_wMap.ReferenceDieSetting = 0;
            this.m_wMap.ScaleMark = false;
            this.m_wMap.SelecetedBin = "ALL";
            this.m_wMap.SelectedVI = "ALL";
            this.m_wMap.Size = new System.Drawing.Size(714, 812);
            this.m_wMap.SkipDieColor = System.Drawing.Color.Yellow;
            this.m_wMap.TabIndex = 2;
            this.m_wMap.ToGradationDieColor = System.Drawing.Color.Red;
            this.m_wMap.TransParent = 255;
            this.m_wMap.ViewAngle = 0;
            this.m_wMap.VIMember = "VIFAIL";
            this.m_wMap.VisibleDieBorder = true;
            this.m_wMap.VisibleDieValue = false;
            this.m_wMap.VisibleFocusDie = false;
            this.m_wMap.VisibleInfomation = false;
            this.m_wMap.VisibleOffDie = false;
            this.m_wMap.VisibleShotAlignPoint = false;
            this.m_wMap.VisibleSignDies = false;
            this.m_wMap.VisibleStringBin = false;
            this.m_wMap.VisibleVIFail = false;
            this.m_wMap.VisibleXY = false;
            this.m_wMap.WaferBorderColor = System.Drawing.Color.Silver;
            this.m_wMap.WaferColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(234)))), ((int)(((byte)(216)))));
            this.m_wMap.WaferDrawMode = DACrux.Map.MapMode.Fit;
            this.m_wMap.WaferID = "";
            this.m_wMap.WaferMargin = 0.95D;
            this.m_wMap.WaferSize = 300000D;
            this.m_wMap.XYDirect = DACrux.Base.XYDirection.LeftBottom;
            this.m_wMap.OnChangeCurrentDie += new DACrux.Map.ChangeCurrentDie(this.m_wMap_OnChangeCurrentDie);
            this.m_wMap.OnChangePosition += new DACrux.Map.ChangePosition(this.m_wMap_OnChangePosition);
            this.m_wMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.m_wMap_MouseMove);
            this.m_wMap.MouseLeave += new System.EventHandler(this.m_wMap_MouseLeave);
            //
            // frmStandardMapView
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1634, 984);
            this.Controls.Add(this.m_wMap);
            this.Controls.Add(this.lblDieCount);
            this.Controls.Add(this.tabRight);
            this.Controls.Add(this.pnlList);
            this.Controls.Add(this.topBar);
            this.Controls.Add(this.lblCoordinates);
            this.Controls.Add(this.pnlStatus);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.Margin = new System.Windows.Forms.Padding(9);
            this.MinimumSize = new System.Drawing.Size(1100, 620);
            this.Name = "frmStandardMapView";
            this.Text = "Wafer Map 조회";
            this.Load += new System.EventHandler(this.frmStandardMapView_Load);
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.pnlList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdMapList)).EndInit();
            this.tabRight.ResumeLayout(false);
            this.tabInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdInfo)).EndInit();
            this.pnlStatus.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblListCount;
        private System.Windows.Forms.Panel pnlList;
        private System.Windows.Forms.DataGridView grdMapList;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMapId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWafer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNetDie;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDirection;
        private System.Windows.Forms.TabControl tabRight;
        private System.Windows.Forms.TabPage tabInfo;
        private System.Windows.Forms.DataGridView grdInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInfoName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInfoValue;
        private System.Windows.Forms.CheckBox chkDieNumber;
        private System.Windows.Forms.Label lblDieCount;
        private System.Windows.Forms.Label lblCoordinates;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Label lblStatus;
        private DACrux.SEMDMS.Control.ProgressStatusBar busyBar;
        private DACrux.TEST.ENGUI.StandardMapEditMap m_wMap;
    }
}
