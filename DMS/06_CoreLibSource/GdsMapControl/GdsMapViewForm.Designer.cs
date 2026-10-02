namespace NexplantQMS.GdsMap
{
    /// <summary>GDS Map 조회 화면의 컨트롤 생성과 배치를 정의한다. (설계: 문서/2026-10-02_GDS_Map_조회화면_설계.md 3.2절)</summary>
    partial class GdsMapViewForm
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>화면이 닫힐 때 컨트롤과 OpenGL 자원을 함께 해제한다.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 위: 조회 조건(Factory / Device) / 옵션, 왼쪽: Map 정보 + Layer 목록, 가운데: 도면, 아래: 진행률 / 상태 / 마우스 좌표.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlCondition = new System.Windows.Forms.Panel();
            this.lblOptionNote = new System.Windows.Forms.Label();
            this.rdoBoth = new System.Windows.Forms.RadioButton();
            this.rdoChainOnly = new System.Windows.Forms.RadioButton();
            this.rdoDrawingOnly = new System.Windows.Forms.RadioButton();
            this.lblOption = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cboDevice = new System.Windows.Forms.ComboBox();
            this.lblDevice = new System.Windows.Forms.Label();
            this.cboFactory = new System.Windows.Forms.ComboBox();
            this.lblFactory = new System.Windows.Forms.Label();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.chkLayerItems = new NexplantQMS.GdsMap.LayerColorCheckedListBox();
            this.pnlLayerButtons = new System.Windows.Forms.Panel();
            this.btnLayerNone = new System.Windows.Forms.Button();
            this.btnLayerAll = new System.Windows.Forms.Button();
            this.lblLayerTitle = new System.Windows.Forms.Label();
            this.grpMapInfo = new System.Windows.Forms.GroupBox();
            this.lblMapInfo = new System.Windows.Forms.Label();
            this.map = new NexplantQMS.GdsMap.GdsMapControl();
            this.statusStripView = new System.Windows.Forms.StatusStrip();
            this.progressLoad = new System.Windows.Forms.ToolStripProgressBar();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCoordinate = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlCondition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.pnlLayerButtons.SuspendLayout();
            this.grpMapInfo.SuspendLayout();
            this.statusStripView.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlCondition
            //
            this.pnlCondition.Controls.Add(this.lblOptionNote);
            this.pnlCondition.Controls.Add(this.rdoBoth);
            this.pnlCondition.Controls.Add(this.rdoChainOnly);
            this.pnlCondition.Controls.Add(this.rdoDrawingOnly);
            this.pnlCondition.Controls.Add(this.lblOption);
            this.pnlCondition.Controls.Add(this.btnStop);
            this.pnlCondition.Controls.Add(this.btnSearch);
            this.pnlCondition.Controls.Add(this.cboDevice);
            this.pnlCondition.Controls.Add(this.lblDevice);
            this.pnlCondition.Controls.Add(this.cboFactory);
            this.pnlCondition.Controls.Add(this.lblFactory);
            this.pnlCondition.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCondition.Location = new System.Drawing.Point(0, 0);
            this.pnlCondition.Name = "pnlCondition";
            this.pnlCondition.Size = new System.Drawing.Size(1200, 62);
            this.pnlCondition.TabIndex = 0;
            //
            // lblFactory
            //
            this.lblFactory.AutoSize = true;
            this.lblFactory.Location = new System.Drawing.Point(8, 11);
            this.lblFactory.Name = "lblFactory";
            this.lblFactory.Size = new System.Drawing.Size(45, 12);
            this.lblFactory.TabIndex = 0;
            this.lblFactory.Text = "Factory";
            //
            // cboFactory
            //
            this.cboFactory.FormattingEnabled = true;
            this.cboFactory.Location = new System.Drawing.Point(58, 7);
            this.cboFactory.Name = "cboFactory";
            this.cboFactory.Size = new System.Drawing.Size(120, 20);
            this.cboFactory.TabIndex = 1;
            this.cboFactory.SelectedIndexChanged += new System.EventHandler(this.cboFactory_SelectedIndexChanged);
            this.cboFactory.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboFactory_KeyDown);
            //
            // lblDevice
            //
            this.lblDevice.AutoSize = true;
            this.lblDevice.Location = new System.Drawing.Point(190, 11);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(43, 12);
            this.lblDevice.TabIndex = 2;
            this.lblDevice.Text = "Device";
            //
            // cboDevice
            //
            this.cboDevice.FormattingEnabled = true;
            this.cboDevice.Location = new System.Drawing.Point(238, 7);
            this.cboDevice.Name = "cboDevice";
            this.cboDevice.Size = new System.Drawing.Size(160, 20);
            this.cboDevice.TabIndex = 3;
            this.cboDevice.SelectedIndexChanged += new System.EventHandler(this.cboDevice_SelectedIndexChanged);
            this.cboDevice.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboDevice_KeyDown);
            //
            // btnSearch
            //
            this.btnSearch.Location = new System.Drawing.Point(410, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 24);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            //
            // btnStop
            //
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(496, 5);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(80, 24);
            this.btnStop.TabIndex = 7;
            this.btnStop.Text = "중지";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            //
            // lblOption
            //
            this.lblOption.AutoSize = true;
            this.lblOption.Location = new System.Drawing.Point(8, 39);
            this.lblOption.Name = "lblOption";
            this.lblOption.Size = new System.Drawing.Size(57, 12);
            this.lblOption.TabIndex = 8;
            this.lblOption.Text = "조회 옵션";
            //
            // rdoDrawingOnly
            //
            this.rdoDrawingOnly.AutoSize = true;
            this.rdoDrawingOnly.Checked = true;
            this.rdoDrawingOnly.Location = new System.Drawing.Point(72, 37);
            this.rdoDrawingOnly.Name = "rdoDrawingOnly";
            this.rdoDrawingOnly.Size = new System.Drawing.Size(63, 16);
            this.rdoDrawingOnly.TabIndex = 9;
            this.rdoDrawingOnly.TabStop = true;
            this.rdoDrawingOnly.Text = "도면만";
            this.rdoDrawingOnly.UseVisualStyleBackColor = true;
            //
            // rdoChainOnly
            //
            this.rdoChainOnly.AutoSize = true;
            this.rdoChainOnly.Enabled = false;
            this.rdoChainOnly.Location = new System.Drawing.Point(146, 37);
            this.rdoChainOnly.Name = "rdoChainOnly";
            this.rdoChainOnly.Size = new System.Drawing.Size(73, 16);
            this.rdoChainOnly.TabIndex = 10;
            this.rdoChainOnly.Text = "Chain만";
            this.rdoChainOnly.UseVisualStyleBackColor = true;
            //
            // rdoBoth
            //
            this.rdoBoth.AutoSize = true;
            this.rdoBoth.Enabled = false;
            this.rdoBoth.Location = new System.Drawing.Point(230, 37);
            this.rdoBoth.Name = "rdoBoth";
            this.rdoBoth.Size = new System.Drawing.Size(51, 16);
            this.rdoBoth.TabIndex = 11;
            this.rdoBoth.Text = "둘 다";
            this.rdoBoth.UseVisualStyleBackColor = true;
            //
            // lblOptionNote
            //
            this.lblOptionNote.AutoSize = true;
            this.lblOptionNote.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblOptionNote.Location = new System.Drawing.Point(296, 39);
            this.lblOptionNote.Name = "lblOptionNote";
            this.lblOptionNote.Size = new System.Drawing.Size(285, 12);
            this.lblOptionNote.TabIndex = 12;
            this.lblOptionNote.Text = "※ Chain 옵션은 Chain 저장 기능 구현 후 사용합니다.";
            //
            // splitMain
            //
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitMain.Location = new System.Drawing.Point(0, 62);
            this.splitMain.Name = "splitMain";
            //
            // splitMain.Panel1
            //
            this.splitMain.Panel1.Controls.Add(this.chkLayerItems);
            this.splitMain.Panel1.Controls.Add(this.pnlLayerButtons);
            this.splitMain.Panel1.Controls.Add(this.grpMapInfo);
            //
            // splitMain.Panel2
            //
            this.splitMain.Panel2.Controls.Add(this.map);
            this.splitMain.Size = new System.Drawing.Size(1200, 686);
            this.splitMain.SplitterDistance = 260;
            this.splitMain.TabIndex = 1;
            //
            // grpMapInfo
            //
            this.grpMapInfo.Controls.Add(this.lblMapInfo);
            this.grpMapInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpMapInfo.Location = new System.Drawing.Point(0, 0);
            this.grpMapInfo.Name = "grpMapInfo";
            this.grpMapInfo.Size = new System.Drawing.Size(260, 190);
            this.grpMapInfo.TabIndex = 0;
            this.grpMapInfo.TabStop = false;
            this.grpMapInfo.Text = "Map 정보";
            //
            // lblMapInfo
            //
            this.lblMapInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMapInfo.Location = new System.Drawing.Point(3, 17);
            this.lblMapInfo.Name = "lblMapInfo";
            this.lblMapInfo.Padding = new System.Windows.Forms.Padding(4);
            this.lblMapInfo.Size = new System.Drawing.Size(254, 170);
            this.lblMapInfo.TabIndex = 0;
            this.lblMapInfo.Text = "Device를 선택하세요.";
            //
            // pnlLayerButtons
            //
            this.pnlLayerButtons.Controls.Add(this.btnLayerNone);
            this.pnlLayerButtons.Controls.Add(this.btnLayerAll);
            this.pnlLayerButtons.Controls.Add(this.lblLayerTitle);
            this.pnlLayerButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLayerButtons.Location = new System.Drawing.Point(0, 190);
            this.pnlLayerButtons.Name = "pnlLayerButtons";
            this.pnlLayerButtons.Size = new System.Drawing.Size(260, 30);
            this.pnlLayerButtons.TabIndex = 1;
            //
            // lblLayerTitle
            //
            this.lblLayerTitle.AutoSize = true;
            this.lblLayerTitle.Location = new System.Drawing.Point(4, 9);
            this.lblLayerTitle.Name = "lblLayerTitle";
            this.lblLayerTitle.Size = new System.Drawing.Size(117, 12);
            this.lblLayerTitle.TabIndex = 0;
            this.lblLayerTitle.Text = "Layer (도형 수)";
            //
            // btnLayerAll
            //
            this.btnLayerAll.Location = new System.Drawing.Point(140, 4);
            this.btnLayerAll.Name = "btnLayerAll";
            this.btnLayerAll.Size = new System.Drawing.Size(56, 22);
            this.btnLayerAll.TabIndex = 1;
            this.btnLayerAll.Text = "전체";
            this.btnLayerAll.UseVisualStyleBackColor = true;
            this.btnLayerAll.Click += new System.EventHandler(this.btnLayerAll_Click);
            //
            // btnLayerNone
            //
            this.btnLayerNone.Location = new System.Drawing.Point(200, 4);
            this.btnLayerNone.Name = "btnLayerNone";
            this.btnLayerNone.Size = new System.Drawing.Size(56, 22);
            this.btnLayerNone.TabIndex = 2;
            this.btnLayerNone.Text = "해제";
            this.btnLayerNone.UseVisualStyleBackColor = true;
            this.btnLayerNone.Click += new System.EventHandler(this.btnLayerNone_Click);
            //
            // chkLayerItems
            //
            this.chkLayerItems.CheckOnClick = true;
            this.chkLayerItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkLayerItems.FormattingEnabled = true;
            this.chkLayerItems.Location = new System.Drawing.Point(0, 220);
            this.chkLayerItems.Name = "chkLayerItems";
            this.chkLayerItems.Size = new System.Drawing.Size(260, 466);
            this.chkLayerItems.TabIndex = 2;
            this.chkLayerItems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkLayerItems_ItemCheck);
            this.chkLayerItems.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.chkLayerItems_Format);
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
            this.map.Size = new System.Drawing.Size(936, 686);
            this.map.TabIndex = 0;
            this.map.VSync = false;
            this.map.MouseWorldPositionChanged += new System.EventHandler<System.Drawing.PointF>(this.map_MouseWorldPositionChanged);
            this.map.RenderProgressChanged += new System.EventHandler<NexplantQMS.GdsMap.GdsMapRenderProgressChangedEventArgs>(this.map_RenderProgressChanged);
            this.map.FirstFrameMeasured += new System.EventHandler<NexplantQMS.GdsMap.GdsMapLoadMetrics>(this.map_FirstFrameMeasured);
            //
            // statusStripView
            //
            this.statusStripView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressLoad,
            this.lblStatus,
            this.lblCoordinate});
            this.statusStripView.Location = new System.Drawing.Point(0, 748);
            this.statusStripView.Name = "statusStripView";
            this.statusStripView.ShowItemToolTips = true;
            this.statusStripView.Size = new System.Drawing.Size(1200, 22);
            this.statusStripView.SizingGrip = false;
            this.statusStripView.TabIndex = 2;
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
            this.lblStatus.Size = new System.Drawing.Size(1000, 17);
            this.lblStatus.Spring = true;
            this.lblStatus.Text = "대기";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblCoordinate
            //
            this.lblCoordinate.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblCoordinate.Name = "lblCoordinate";
            this.lblCoordinate.Size = new System.Drawing.Size(120, 17);
            this.lblCoordinate.Text = "X: - / Y: -";
            //
            // GdsMapViewForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 770);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.pnlCondition);
            this.Controls.Add(this.statusStripView);
            this.Name = "GdsMapViewForm";
            this.Text = "GDS Map 조회";
            this.Shown += new System.EventHandler(this.GdsMapViewForm_Shown);
            this.pnlCondition.ResumeLayout(false);
            this.pnlCondition.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.pnlLayerButtons.ResumeLayout(false);
            this.pnlLayerButtons.PerformLayout();
            this.grpMapInfo.ResumeLayout(false);
            this.statusStripView.ResumeLayout(false);
            this.statusStripView.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlCondition;
        private System.Windows.Forms.Label lblFactory;
        private System.Windows.Forms.ComboBox cboFactory;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.ComboBox cboDevice;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblOption;
        private System.Windows.Forms.RadioButton rdoDrawingOnly;
        private System.Windows.Forms.RadioButton rdoChainOnly;
        private System.Windows.Forms.RadioButton rdoBoth;
        private System.Windows.Forms.Label lblOptionNote;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.GroupBox grpMapInfo;
        private System.Windows.Forms.Label lblMapInfo;
        private System.Windows.Forms.Panel pnlLayerButtons;
        private System.Windows.Forms.Label lblLayerTitle;
        private System.Windows.Forms.Button btnLayerAll;
        private System.Windows.Forms.Button btnLayerNone;
        private NexplantQMS.GdsMap.LayerColorCheckedListBox chkLayerItems;
        private NexplantQMS.GdsMap.GdsMapControl map;
        private System.Windows.Forms.StatusStrip statusStripView;
        private System.Windows.Forms.ToolStripProgressBar progressLoad;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblCoordinate;
    }
}
