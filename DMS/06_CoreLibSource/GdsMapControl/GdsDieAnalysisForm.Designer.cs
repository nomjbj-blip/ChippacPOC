namespace NexplantQMS.GdsMap
{
    /// <summary>Die 상세 분석 화면 배치. 설계: 문서/2026-10-12_GDS_KLARF_연관분석_화면_설계.md 5.1절 (CLAUDE-053)</summary>
    partial class GdsDieAnalysisForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 위: Wafer / 분석 기준(읽기 전용) / Chip 좌표 정보(이 Chip 1개만 분석, 2026-10-12 사용자 지시) / 배경 도면 / 원점 / 전체 보기
        /// 가운데: 왼쪽 Chain 목록(체크 = 지도 표시) / 가운데 GDS 도면 / 오른쪽 Defect 목록
        /// 아래: 상태줄(진행 / 좌표)
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblWaferInfo = new System.Windows.Forms.Label();
            this.lblCriteria = new System.Windows.Forms.Label();
            this.lblDieInfo = new System.Windows.Forms.Label();
            this.chkBackground = new System.Windows.Forms.CheckBox();
            this.chkOriginCross = new System.Windows.Forms.CheckBox();
            this.btnFit = new System.Windows.Forms.Button();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.dgvChains = new System.Windows.Forms.DataGridView();
            this.colShow = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlChainHead = new System.Windows.Forms.Panel();
            this.lblChainTitle = new System.Windows.Forms.Label();
            this.chkHitOnly = new System.Windows.Forms.CheckBox();
            this.splitRight = new System.Windows.Forms.SplitContainer();
            this.map = new NexplantQMS.GdsMap.GdsMapControl();
            this.dgvDefects = new System.Windows.Forms.DataGridView();
            this.pnlDefectHead = new System.Windows.Forms.Panel();
            this.cboScope = new System.Windows.Forms.ComboBox();
            this.lblDefectCount = new System.Windows.Forms.Label();
            this.lblHint = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.progressLoad = new System.Windows.Forms.ToolStripProgressBar();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCoordinate = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChains)).BeginInit();
            this.pnlChainHead.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitRight)).BeginInit();
            this.splitRight.Panel1.SuspendLayout();
            this.splitRight.Panel2.SuspendLayout();
            this.splitRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDefects)).BeginInit();
            this.pnlDefectHead.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlTop
            //
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(242)))), ((int)(((byte)(250)))));
            this.pnlTop.Controls.Add(this.lblWaferInfo);
            this.pnlTop.Controls.Add(this.lblCriteria);
            this.pnlTop.Controls.Add(this.lblDieInfo);
            this.pnlTop.Controls.Add(this.chkBackground);
            this.pnlTop.Controls.Add(this.chkOriginCross);
            this.pnlTop.Controls.Add(this.btnFit);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1400, 66);
            this.pnlTop.TabIndex = 0;
            //
            // lblWaferInfo
            //
            this.lblWaferInfo.AutoSize = true;
            this.lblWaferInfo.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblWaferInfo.Location = new System.Drawing.Point(10, 8);
            this.lblWaferInfo.Name = "lblWaferInfo";
            this.lblWaferInfo.Size = new System.Drawing.Size(190, 17);
            this.lblWaferInfo.TabIndex = 0;
            this.lblWaferInfo.Text = "연관 분석 화면에서 Die 를 더블클릭하세요";
            //
            // lblCriteria
            //
            this.lblCriteria.AutoSize = true;
            this.lblCriteria.ForeColor = System.Drawing.Color.DimGray;
            this.lblCriteria.Location = new System.Drawing.Point(520, 11);
            this.lblCriteria.Name = "lblCriteria";
            this.lblCriteria.Size = new System.Drawing.Size(100, 12);
            this.lblCriteria.TabIndex = 1;
            this.lblCriteria.Text = "분석 기준: -";
            //
            // lblDieInfo
            //
            this.lblDieInfo.AutoSize = true;
            this.lblDieInfo.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblDieInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(60)))), ((int)(((byte)(130)))));
            this.lblDieInfo.Location = new System.Drawing.Point(10, 36);
            this.lblDieInfo.Name = "lblDieInfo";
            this.lblDieInfo.Size = new System.Drawing.Size(120, 20);
            this.lblDieInfo.TabIndex = 2;
            this.lblDieInfo.Text = "Die (-, -)";
            this.toolTip.SetToolTip(this.lblDieInfo, "상세 화면은 이 Chip 1개만 분석한다. 다른 Chip 은 연관 분석 화면에서 선택");
            //
            // chkBackground
            //
            this.chkBackground.AutoSize = true;
            this.chkBackground.Location = new System.Drawing.Point(520, 40);
            this.chkBackground.Name = "chkBackground";
            this.chkBackground.Size = new System.Drawing.Size(150, 16);
            this.chkBackground.TabIndex = 8;
            this.chkBackground.Text = "배경 도면(전체 Layer)";
            this.toolTip.SetToolTip(this.chkBackground, "해제: Chain 이 쓰는 Layer 만 읽는다(빠름) / 체크: Map 의 모든 Layer 를 다시 읽는다");
            this.chkBackground.UseVisualStyleBackColor = true;
            this.chkBackground.CheckedChanged += new System.EventHandler(this.chkBackground_CheckedChanged);
            //
            // chkOriginCross
            //
            this.chkOriginCross.AutoSize = true;
            this.chkOriginCross.Checked = true;
            this.chkOriginCross.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOriginCross.Location = new System.Drawing.Point(680, 40);
            this.chkOriginCross.Name = "chkOriginCross";
            this.chkOriginCross.Size = new System.Drawing.Size(76, 16);
            this.chkOriginCross.TabIndex = 9;
            this.chkOriginCross.Text = "원점 표시";
            this.chkOriginCross.UseVisualStyleBackColor = true;
            this.chkOriginCross.CheckedChanged += new System.EventHandler(this.chkOriginCross_CheckedChanged);
            //
            // btnFit
            //
            this.btnFit.Location = new System.Drawing.Point(768, 35);
            this.btnFit.Name = "btnFit";
            this.btnFit.Size = new System.Drawing.Size(80, 24);
            this.btnFit.TabIndex = 10;
            this.btnFit.Text = "전체 보기";
            this.toolTip.SetToolTip(this.btnFit, "Chip 전체 보기");
            this.btnFit.UseVisualStyleBackColor = true;
            this.btnFit.Click += new System.EventHandler(this.btnFit_Click);
            //
            // splitMain
            //
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitMain.Location = new System.Drawing.Point(0, 66);
            this.splitMain.Name = "splitMain";
            //
            // splitMain.Panel1
            //
            this.splitMain.Panel1.Controls.Add(this.dgvChains);
            this.splitMain.Panel1.Controls.Add(this.pnlChainHead);
            //
            // splitMain.Panel2
            //
            this.splitMain.Panel2.Controls.Add(this.splitRight);
            this.splitMain.Size = new System.Drawing.Size(1400, 812);
            this.splitMain.SplitterDistance = 300;
            this.splitMain.TabIndex = 1;
            //
            // dgvChains
            //
            this.dgvChains.AllowUserToAddRows = false;
            this.dgvChains.AllowUserToDeleteRows = false;
            this.dgvChains.AllowUserToResizeRows = false;
            this.dgvChains.BackgroundColor = System.Drawing.Color.White;
            this.dgvChains.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChains.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colShow,
            this.colColor,
            this.colCode,
            this.colCount,
            this.colName});
            this.dgvChains.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvChains.Location = new System.Drawing.Point(0, 28);
            this.dgvChains.MultiSelect = false;
            this.dgvChains.Name = "dgvChains";
            this.dgvChains.RowHeadersVisible = false;
            this.dgvChains.RowTemplate.Height = 23;
            this.dgvChains.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvChains.Size = new System.Drawing.Size(300, 784);
            this.dgvChains.TabIndex = 1;
            this.dgvChains.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChains_CellContentClick);
            this.dgvChains.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChains_CellDoubleClick);
            //
            // colShow
            //
            this.colShow.HeaderText = "표시";
            this.colShow.Name = "colShow";
            this.colShow.Width = 40;
            //
            // colColor
            //
            this.colColor.HeaderText = "색";
            this.colColor.Name = "colColor";
            this.colColor.ReadOnly = true;
            this.colColor.Width = 30;
            //
            // colCode
            //
            this.colCode.HeaderText = "Chain";
            this.colCode.Name = "colCode";
            this.colCode.ReadOnly = true;
            this.colCode.Width = 50;
            //
            // colCount
            //
            this.colCount.HeaderText = "Defect";
            this.colCount.Name = "colCount";
            this.colCount.ReadOnly = true;
            this.colCount.Width = 55;
            //
            // colName
            //
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.HeaderText = "이름";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            //
            // pnlChainHead
            //
            this.pnlChainHead.Controls.Add(this.lblChainTitle);
            this.pnlChainHead.Controls.Add(this.chkHitOnly);
            this.pnlChainHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlChainHead.Location = new System.Drawing.Point(0, 0);
            this.pnlChainHead.Name = "pnlChainHead";
            this.pnlChainHead.Size = new System.Drawing.Size(300, 28);
            this.pnlChainHead.TabIndex = 0;
            //
            // lblChainTitle
            //
            this.lblChainTitle.AutoSize = true;
            this.lblChainTitle.Location = new System.Drawing.Point(4, 8);
            this.lblChainTitle.Name = "lblChainTitle";
            this.lblChainTitle.Size = new System.Drawing.Size(120, 12);
            this.lblChainTitle.TabIndex = 0;
            this.lblChainTitle.Text = "Chain (더블클릭: 위치)";
            //
            // chkHitOnly
            //
            this.chkHitOnly.AutoSize = true;
            this.chkHitOnly.Location = new System.Drawing.Point(168, 6);
            this.chkHitOnly.Name = "chkHitOnly";
            this.chkHitOnly.Size = new System.Drawing.Size(100, 16);
            this.chkHitOnly.TabIndex = 1;
            this.chkHitOnly.Text = "걸린 Chain 만";
            this.toolTip.SetToolTip(this.chkHitOnly, "체크: Defect 가 걸린 Chain 만 지도에 표시 / 해제: 모든 Chain 표시");
            this.chkHitOnly.UseVisualStyleBackColor = true;
            this.chkHitOnly.CheckedChanged += new System.EventHandler(this.chkHitOnly_CheckedChanged);
            //
            // splitRight
            //
            this.splitRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitRight.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitRight.Location = new System.Drawing.Point(0, 0);
            this.splitRight.Name = "splitRight";
            //
            // splitRight.Panel1
            //
            this.splitRight.Panel1.Controls.Add(this.map);
            //
            // splitRight.Panel2
            //
            this.splitRight.Panel2.Controls.Add(this.dgvDefects);
            this.splitRight.Panel2.Controls.Add(this.pnlDefectHead);
            this.splitRight.Panel2.Controls.Add(this.lblHint);
            this.splitRight.Size = new System.Drawing.Size(1096, 812);
            this.splitRight.SplitterDistance = 726;
            this.splitRight.TabIndex = 0;
            //
            // map
            //
            this.map.BackColor = System.Drawing.Color.Black;
            this.map.ColorAlpha = 50;
            this.map.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.Location = new System.Drawing.Point(0, 0);
            this.map.Mode = NexplantQMS.GdsMap.GdsMapControl.ViewMode.View;
            this.map.Name = "map";
            this.map.Size = new System.Drawing.Size(726, 812);
            this.map.TabIndex = 0;
            this.map.VSync = false;
            this.map.MouseWorldPositionChanged += new System.EventHandler<System.Drawing.PointF>(this.map_MouseWorldPositionChanged);
            this.map.MouseClick += new System.Windows.Forms.MouseEventHandler(this.map_MouseClick);
            //
            // dgvDefects
            //
            this.dgvDefects.AllowUserToAddRows = false;
            this.dgvDefects.AllowUserToDeleteRows = false;
            this.dgvDefects.AllowUserToResizeRows = false;
            this.dgvDefects.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvDefects.BackgroundColor = System.Drawing.Color.White;
            this.dgvDefects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDefects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDefects.Location = new System.Drawing.Point(0, 28);
            this.dgvDefects.MultiSelect = false;
            this.dgvDefects.Name = "dgvDefects";
            this.dgvDefects.ReadOnly = true;
            this.dgvDefects.RowHeadersVisible = false;
            this.dgvDefects.RowTemplate.Height = 23;
            this.dgvDefects.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDefects.Size = new System.Drawing.Size(366, 760);
            this.dgvDefects.TabIndex = 1;
            this.dgvDefects.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDefects_CellDoubleClick);
            this.dgvDefects.SelectionChanged += new System.EventHandler(this.dgvDefects_SelectionChanged);
            //
            // pnlDefectHead
            //
            this.pnlDefectHead.Controls.Add(this.cboScope);
            this.pnlDefectHead.Controls.Add(this.lblDefectCount);
            this.pnlDefectHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDefectHead.Location = new System.Drawing.Point(0, 0);
            this.pnlDefectHead.Name = "pnlDefectHead";
            this.pnlDefectHead.Size = new System.Drawing.Size(366, 28);
            this.pnlDefectHead.TabIndex = 0;
            //
            // cboScope
            //
            this.cboScope.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboScope.Items.AddRange(new object[] {
            "Chain 영역",
            "Chain 밖",
            "전체"});
            this.cboScope.Location = new System.Drawing.Point(4, 4);
            this.cboScope.Name = "cboScope";
            this.cboScope.Size = new System.Drawing.Size(100, 20);
            this.cboScope.TabIndex = 0;
            this.cboScope.SelectedIndexChanged += new System.EventHandler(this.cboScope_SelectedIndexChanged);
            //
            // lblDefectCount
            //
            this.lblDefectCount.AutoSize = true;
            this.lblDefectCount.Location = new System.Drawing.Point(112, 8);
            this.lblDefectCount.Name = "lblDefectCount";
            this.lblDefectCount.Size = new System.Drawing.Size(60, 12);
            this.lblDefectCount.TabIndex = 1;
            this.lblDefectCount.Text = "Defect -";
            //
            // lblHint
            //
            this.lblHint.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblHint.ForeColor = System.Drawing.Color.DimGray;
            this.lblHint.Location = new System.Drawing.Point(0, 788);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(366, 24);
            this.lblHint.TabIndex = 2;
            this.lblHint.Text = "행 클릭: 위치 이동 / 더블클릭: 이미지 / 지도 Defect 클릭: 행 선택";
            this.lblHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // statusStrip
            //
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressLoad,
            this.lblStatus,
            this.lblCoordinate});
            this.statusStrip.Location = new System.Drawing.Point(0, 878);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.ShowItemToolTips = true;
            this.statusStrip.Size = new System.Drawing.Size(1400, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 2;
            //
            // progressLoad
            //
            this.progressLoad.Name = "progressLoad";
            this.progressLoad.Size = new System.Drawing.Size(160, 16);
            this.progressLoad.Visible = false;
            //
            // lblStatus
            //
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(1300, 17);
            this.lblStatus.Spring = true;
            this.lblStatus.Text = "대기";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblCoordinate
            //
            this.lblCoordinate.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblCoordinate.Name = "lblCoordinate";
            this.lblCoordinate.Size = new System.Drawing.Size(90, 17);
            this.lblCoordinate.Text = "X: - / Y: -";
            //
            // GdsDieAnalysisForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 900);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.statusStrip);
            this.Name = "GdsDieAnalysisForm";
            this.Text = "Die 상세 분석";
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChains)).EndInit();
            this.pnlChainHead.ResumeLayout(false);
            this.pnlChainHead.PerformLayout();
            this.splitRight.Panel1.ResumeLayout(false);
            this.splitRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitRight)).EndInit();
            this.splitRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDefects)).EndInit();
            this.pnlDefectHead.ResumeLayout(false);
            this.pnlDefectHead.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblWaferInfo;
        private System.Windows.Forms.Label lblCriteria;
        private System.Windows.Forms.Label lblDieInfo;
        private System.Windows.Forms.CheckBox chkBackground;
        private System.Windows.Forms.CheckBox chkOriginCross;
        private System.Windows.Forms.Button btnFit;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.DataGridView dgvChains;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colShow;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.Panel pnlChainHead;
        private System.Windows.Forms.Label lblChainTitle;
        private System.Windows.Forms.CheckBox chkHitOnly;
        private System.Windows.Forms.SplitContainer splitRight;
        private NexplantQMS.GdsMap.GdsMapControl map;
        private System.Windows.Forms.DataGridView dgvDefects;
        private System.Windows.Forms.Panel pnlDefectHead;
        private System.Windows.Forms.ComboBox cboScope;
        private System.Windows.Forms.Label lblDefectCount;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripProgressBar progressLoad;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblCoordinate;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
