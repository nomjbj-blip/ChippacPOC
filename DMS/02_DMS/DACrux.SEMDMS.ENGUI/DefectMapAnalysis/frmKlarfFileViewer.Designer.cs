namespace DACrux.SEMDMS.ENGUI
{
    // KLARF 파일 조회 화면의 컨트롤과 배치를 정의한다. 파일 읽기/그리기/마우스 정보는 본문 파일에서 처리한다.
    partial class frmKlarfFileViewer
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>화면 종료 시 디자이너 구성 요소를 해제한다.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// 위: 파일 선택 / Wafer / 색상 기준, 오른쪽: 파일 정보 / 범례 / Defect 목록,
        /// 아래: 마우스 위치 정보 / 상태, 가운데: 기존 DefectMap 기반 Wafer Map.
        /// </summary>
        private void InitializeComponent()
        {
            this.fileBar = new System.Windows.Forms.Panel();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.lblWafer = new System.Windows.Forms.Label();
            this.cmbWafer = new System.Windows.Forms.ComboBox();
            this.lblColorBy = new System.Windows.Forms.Label();
            this.cmbColorBy = new System.Windows.Forms.ComboBox();
            this.chkDieLabel = new System.Windows.Forms.CheckBox();
            this.rightPanel = new System.Windows.Forms.Panel();
            this.dgvDefect = new System.Windows.Forms.DataGridView();
            this.colDefectId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIndexX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIndexY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colXRel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colYRel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFineBin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRoughBin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblDefect = new System.Windows.Forms.Label();
            this.dgvLegend = new System.Windows.Forms.DataGridView();
            this.colLegendColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLegendCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLegendName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLegendCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblLegend = new System.Windows.Forms.Label();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.lblMouse = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.m_dMap = new DACrux.SEMDMS.ENGUI.KlarfDefectMap();
            this.fileBar.SuspendLayout();
            this.rightPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDefect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLegend)).BeginInit();
            this.SuspendLayout();
            //
            // fileBar
            //
            this.fileBar.Controls.Add(this.txtFile);
            this.fileBar.Controls.Add(this.btnBrowse);
            this.fileBar.Controls.Add(this.btnReload);
            this.fileBar.Controls.Add(this.lblWafer);
            this.fileBar.Controls.Add(this.cmbWafer);
            this.fileBar.Controls.Add(this.lblColorBy);
            this.fileBar.Controls.Add(this.cmbColorBy);
            this.fileBar.Controls.Add(this.chkDieLabel);
            this.fileBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.fileBar.Location = new System.Drawing.Point(0, 0);
            this.fileBar.Name = "fileBar";
            this.fileBar.Size = new System.Drawing.Size(1400, 84);
            this.fileBar.TabIndex = 2;
            //
            // txtFile
            //
            this.txtFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFile.Location = new System.Drawing.Point(12, 12);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(1152, 23);
            this.txtFile.TabIndex = 0;
            //
            // btnBrowse
            //
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(1174, 9);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(102, 30);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "파일 선택";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            //
            // btnReload
            //
            this.btnReload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReload.Location = new System.Drawing.Point(1284, 9);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(102, 30);
            this.btnReload.TabIndex = 2;
            this.btnReload.Text = "다시 읽기";
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            //
            // lblWafer
            //
            this.lblWafer.Location = new System.Drawing.Point(12, 50);
            this.lblWafer.Name = "lblWafer";
            this.lblWafer.Size = new System.Drawing.Size(50, 23);
            this.lblWafer.TabIndex = 3;
            this.lblWafer.Text = "Wafer";
            this.lblWafer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // cmbWafer
            //
            this.cmbWafer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWafer.Location = new System.Drawing.Point(64, 50);
            this.cmbWafer.Name = "cmbWafer";
            this.cmbWafer.Size = new System.Drawing.Size(240, 23);
            this.cmbWafer.TabIndex = 4;
            this.cmbWafer.SelectedIndexChanged += new System.EventHandler(this.cmbWafer_SelectedIndexChanged);
            //
            // lblColorBy
            //
            this.lblColorBy.Location = new System.Drawing.Point(324, 50);
            this.lblColorBy.Name = "lblColorBy";
            this.lblColorBy.Size = new System.Drawing.Size(70, 23);
            this.lblColorBy.TabIndex = 5;
            this.lblColorBy.Text = "색상 기준";
            this.lblColorBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // cmbColorBy
            //
            this.cmbColorBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbColorBy.Items.AddRange(new object[] {
            "Class",
            "FineBin",
            "RoughBin",
            "Cluster"});
            this.cmbColorBy.Location = new System.Drawing.Point(396, 50);
            this.cmbColorBy.Name = "cmbColorBy";
            this.cmbColorBy.Size = new System.Drawing.Size(130, 23);
            this.cmbColorBy.TabIndex = 6;
            this.cmbColorBy.SelectedIndexChanged += new System.EventHandler(this.cmbColorBy_SelectedIndexChanged);
            //
            // chkDieLabel
            //
            this.chkDieLabel.Checked = true;
            this.chkDieLabel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDieLabel.Location = new System.Drawing.Point(548, 50);
            this.chkDieLabel.Name = "chkDieLabel";
            this.chkDieLabel.Size = new System.Drawing.Size(200, 23);
            this.chkDieLabel.TabIndex = 7;
            this.chkDieLabel.Text = "Die Index 표시 (확대 시)";
            this.chkDieLabel.CheckedChanged += new System.EventHandler(this.chkDieLabel_CheckedChanged);
            //
            // rightPanel
            //
            this.rightPanel.Controls.Add(this.dgvDefect);
            this.rightPanel.Controls.Add(this.lblDefect);
            this.rightPanel.Controls.Add(this.dgvLegend);
            this.rightPanel.Controls.Add(this.lblLegend);
            this.rightPanel.Controls.Add(this.txtInfo);
            this.rightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightPanel.Location = new System.Drawing.Point(1020, 84);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.rightPanel.Size = new System.Drawing.Size(380, 712);
            this.rightPanel.TabIndex = 1;
            //
            // dgvDefect
            //
            this.dgvDefect.AllowUserToAddRows = false;
            this.dgvDefect.AllowUserToDeleteRows = false;
            this.dgvDefect.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvDefect.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDefect.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDefectId,
            this.colIndexX,
            this.colIndexY,
            this.colXRel,
            this.colYRel,
            this.colClass,
            this.colFineBin,
            this.colRoughBin,
            this.colSize});
            this.dgvDefect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDefect.Location = new System.Drawing.Point(6, 503);
            this.dgvDefect.Name = "dgvDefect";
            this.dgvDefect.ReadOnly = true;
            this.dgvDefect.RowHeadersVisible = false;
            this.dgvDefect.RowTemplate.Height = 22;
            this.dgvDefect.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDefect.Size = new System.Drawing.Size(368, 209);
            this.dgvDefect.TabIndex = 4;
            this.dgvDefect.SelectionChanged += new System.EventHandler(this.dgvDefect_SelectionChanged);
            //
            // colDefectId
            //
            this.colDefectId.HeaderText = "ID";
            this.colDefectId.Name = "colDefectId";
            this.colDefectId.ReadOnly = true;
            this.colDefectId.Width = 45;
            //
            // colIndexX
            //
            this.colIndexX.HeaderText = "X Idx";
            this.colIndexX.Name = "colIndexX";
            this.colIndexX.ReadOnly = true;
            this.colIndexX.Width = 45;
            //
            // colIndexY
            //
            this.colIndexY.HeaderText = "Y Idx";
            this.colIndexY.Name = "colIndexY";
            this.colIndexY.ReadOnly = true;
            this.colIndexY.Width = 45;
            //
            // colXRel
            //
            this.colXRel.HeaderText = "XREL";
            this.colXRel.Name = "colXRel";
            this.colXRel.ReadOnly = true;
            this.colXRel.Width = 70;
            //
            // colYRel
            //
            this.colYRel.HeaderText = "YREL";
            this.colYRel.Name = "colYRel";
            this.colYRel.ReadOnly = true;
            this.colYRel.Width = 70;
            //
            // colClass
            //
            this.colClass.HeaderText = "Class";
            this.colClass.Name = "colClass";
            this.colClass.ReadOnly = true;
            this.colClass.Width = 45;
            //
            // colFineBin
            //
            this.colFineBin.HeaderText = "Fine";
            this.colFineBin.Name = "colFineBin";
            this.colFineBin.ReadOnly = true;
            this.colFineBin.Width = 40;
            //
            // colRoughBin
            //
            this.colRoughBin.HeaderText = "Rough";
            this.colRoughBin.Name = "colRoughBin";
            this.colRoughBin.ReadOnly = true;
            this.colRoughBin.Width = 45;
            //
            // colSize
            //
            this.colSize.HeaderText = "Size";
            this.colSize.Name = "colSize";
            this.colSize.ReadOnly = true;
            this.colSize.Width = 60;
            //
            // lblDefect
            //
            this.lblDefect.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDefect.Location = new System.Drawing.Point(6, 477);
            this.lblDefect.Name = "lblDefect";
            this.lblDefect.Size = new System.Drawing.Size(368, 26);
            this.lblDefect.TabIndex = 3;
            this.lblDefect.Text = "Defect 목록 (행 선택 시 Map 강조 / Index 는 KLARF 원본)";
            this.lblDefect.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            //
            // dgvLegend
            //
            this.dgvLegend.AllowUserToAddRows = false;
            this.dgvLegend.AllowUserToDeleteRows = false;
            this.dgvLegend.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvLegend.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLegend.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLegendColor,
            this.colLegendCode,
            this.colLegendName,
            this.colLegendCount});
            this.dgvLegend.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvLegend.Location = new System.Drawing.Point(6, 307);
            this.dgvLegend.Name = "dgvLegend";
            this.dgvLegend.ReadOnly = true;
            this.dgvLegend.RowHeadersVisible = false;
            this.dgvLegend.RowTemplate.Height = 22;
            this.dgvLegend.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLegend.Size = new System.Drawing.Size(368, 170);
            this.dgvLegend.TabIndex = 2;
            //
            // colLegendColor
            //
            this.colLegendColor.HeaderText = "색";
            this.colLegendColor.Name = "colLegendColor";
            this.colLegendColor.ReadOnly = true;
            this.colLegendColor.Width = 36;
            //
            // colLegendCode
            //
            this.colLegendCode.HeaderText = "코드";
            this.colLegendCode.Name = "colLegendCode";
            this.colLegendCode.ReadOnly = true;
            this.colLegendCode.Width = 60;
            //
            // colLegendName
            //
            this.colLegendName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colLegendName.HeaderText = "이름";
            this.colLegendName.Name = "colLegendName";
            this.colLegendName.ReadOnly = true;
            //
            // colLegendCount
            //
            this.colLegendCount.HeaderText = "개수";
            this.colLegendCount.Name = "colLegendCount";
            this.colLegendCount.ReadOnly = true;
            this.colLegendCount.Width = 64;
            //
            // lblLegend
            //
            this.lblLegend.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLegend.Location = new System.Drawing.Point(6, 281);
            this.lblLegend.Name = "lblLegend";
            this.lblLegend.Size = new System.Drawing.Size(368, 26);
            this.lblLegend.TabIndex = 1;
            this.lblLegend.Text = "범례 (색상 기준별 Defect 개수)";
            this.lblLegend.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            //
            // txtInfo
            //
            this.txtInfo.BackColor = System.Drawing.SystemColors.Window;
            this.txtInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtInfo.Location = new System.Drawing.Point(6, 0);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            this.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInfo.Size = new System.Drawing.Size(368, 281);
            this.txtInfo.TabIndex = 0;
            //
            // lblMouse
            //
            this.lblMouse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMouse.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMouse.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblMouse.Location = new System.Drawing.Point(0, 796);
            this.lblMouse.Name = "lblMouse";
            this.lblMouse.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.lblMouse.Size = new System.Drawing.Size(1400, 66);
            this.lblMouse.TabIndex = 3;
            this.lblMouse.Text = "Wafer 좌표: -";
            //
            // lblStatus
            //
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Location = new System.Drawing.Point(0, 862);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lblStatus.Size = new System.Drawing.Size(1400, 26);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "KLARF 파일을 선택하세요.";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.m_dMap.DieMaxX = 0;
            this.m_dMap.DieMaxY = 0;
            this.m_dMap.DieMinX = 0;
            this.m_dMap.DieMinY = 0;
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
            this.m_dMap.Location = new System.Drawing.Point(0, 84);
            this.m_dMap.MapType = DACrux.Base.MAP_TYPE.FINEBIN;
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
            this.m_dMap.Size = new System.Drawing.Size(1020, 712);
            this.m_dMap.SkipDieColor = System.Drawing.Color.Yellow;
            this.m_dMap.TabIndex = 0;
            this.m_dMap.ToGradationDieColor = System.Drawing.Color.Red;
            this.m_dMap.TransParent = 255;
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
            this.m_dMap.MousePositionChanged += new System.EventHandler<DACrux.SEMDMS.ENGUI.KlarfMousePositionEventArgs>(this.m_dMap_MousePositionChanged);
            this.m_dMap.MousePositionCleared += new System.EventHandler(this.m_dMap_MousePositionCleared);
            //
            // frmKlarfFileViewer
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 888);
            this.Controls.Add(this.m_dMap);
            this.Controls.Add(this.rightPanel);
            this.Controls.Add(this.fileBar);
            this.Controls.Add(this.lblMouse);
            this.Controls.Add(this.lblStatus);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.MinimumSize = new System.Drawing.Size(1000, 680);
            this.Name = "frmKlarfFileViewer";
            this.Text = "KLARF File View";
            this.fileBar.ResumeLayout(false);
            this.fileBar.PerformLayout();
            this.rightPanel.ResumeLayout(false);
            this.rightPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDefect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLegend)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel fileBar;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.Label lblWafer;
        private System.Windows.Forms.ComboBox cmbWafer;
        private System.Windows.Forms.Label lblColorBy;
        private System.Windows.Forms.ComboBox cmbColorBy;
        private System.Windows.Forms.CheckBox chkDieLabel;
        private System.Windows.Forms.Panel rightPanel;
        private System.Windows.Forms.TextBox txtInfo;
        private System.Windows.Forms.Label lblLegend;
        private System.Windows.Forms.DataGridView dgvLegend;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLegendColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLegendCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLegendName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLegendCount;
        private System.Windows.Forms.Label lblDefect;
        private System.Windows.Forms.DataGridView dgvDefect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDefectId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIndexX;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIndexY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colXRel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colYRel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFineBin;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRoughBin;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSize;
        private System.Windows.Forms.Label lblMouse;
        private System.Windows.Forms.Label lblStatus;
        private KlarfDefectMap m_dMap;
    }
}
