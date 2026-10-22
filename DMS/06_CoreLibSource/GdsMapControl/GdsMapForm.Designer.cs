namespace NexplantQMS.GdsMap
{
    partial class GdsMapForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
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
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GdsMapForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.chkOriginCross = new System.Windows.Forms.CheckBox();
            this.lblDbDevice = new System.Windows.Forms.Label();
            this.txtDbDevice = new System.Windows.Forms.TextBox();
            this.btnDbSave = new System.Windows.Forms.Button();
            this.btnDbStop = new System.Windows.Forms.Button();
            this.lblTopCell = new System.Windows.Forms.Label();
            this.cboTopCell = new System.Windows.Forms.ComboBox();
            this.dataGrid = new System.Windows.Forms.DataGridView();
            this.GDSContainer = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.map = new NexplantQMS.GdsMap.GdsMapControl();
            this.statusStripMap = new System.Windows.Forms.StatusStrip();
            this.progressMapLoad = new System.Windows.Forms.ToolStripProgressBar();
            this.lblMapElapsed = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblMapStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblMapCoordinate = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chkLayerItems = new NexplantQMS.GdsMap.LayerColorCheckedListBox();
            this.lblLayerSummary = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnLayerCheckAll = new System.Windows.Forms.Button();
            this.btnLayerCheckNone = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GDSContainer)).BeginInit();
            this.GDSContainer.Panel1.SuspendLayout();
            this.GDSContainer.Panel2.SuspendLayout();
            this.GDSContainer.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.statusStripMap.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.chkOriginCross);
            this.panel1.Controls.Add(this.lblDbDevice);
            this.panel1.Controls.Add(this.txtDbDevice);
            this.panel1.Controls.Add(this.btnDbSave);
            this.panel1.Controls.Add(this.btnDbStop);
            this.panel1.Controls.Add(this.lblTopCell);
            this.panel1.Controls.Add(this.cboTopCell);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1488, 60);
            this.panel1.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(18, 9);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(137, 42);
            this.button1.TabIndex = 2;
            this.button1.Text = "파일 열기";
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkOriginCross
            // 
            this.chkOriginCross.AutoSize = true;
            this.chkOriginCross.Checked = true;
            this.chkOriginCross.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOriginCross.Location = new System.Drawing.Point(469, 19);
            this.chkOriginCross.Margin = new System.Windows.Forms.Padding(4);
            this.chkOriginCross.Name = "chkOriginCross";
            this.chkOriginCross.Size = new System.Drawing.Size(112, 22);
            this.chkOriginCross.TabIndex = 3;
            this.chkOriginCross.Text = "원점 표시";
            this.chkOriginCross.UseVisualStyleBackColor = true;
            this.chkOriginCross.CheckedChanged += new System.EventHandler(this.chkOriginCross_CheckedChanged);
            // 
            // lblDbDevice
            // 
            this.lblDbDevice.AutoSize = true;
            this.lblDbDevice.Location = new System.Drawing.Point(170, 21);
            this.lblDbDevice.Name = "lblDbDevice";
            this.lblDbDevice.Size = new System.Drawing.Size(62, 18);
            this.lblDbDevice.TabIndex = 12;
            this.lblDbDevice.Text = "Device";
            // 
            // txtDbDevice
            // 
            this.txtDbDevice.Location = new System.Drawing.Point(238, 16);
            this.txtDbDevice.MaxLength = 50;
            this.txtDbDevice.Name = "txtDbDevice";
            this.txtDbDevice.Size = new System.Drawing.Size(211, 28);
            this.txtDbDevice.TabIndex = 13;
            // 
            // btnDbSave
            // 
            this.btnDbSave.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.btnDbSave.Image = ((System.Drawing.Image)(resources.GetObject("btnDbSave.Image")));
            this.btnDbSave.Location = new System.Drawing.Point(600, 9);
            this.btnDbSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnDbSave.Name = "btnDbSave";
            this.btnDbSave.Size = new System.Drawing.Size(126, 42);
            this.btnDbSave.TabIndex = 16;
            this.btnDbSave.Text = "DB 저장";
            this.btnDbSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDbSave.UseVisualStyleBackColor = true;
            this.btnDbSave.Click += new System.EventHandler(this.btnDbSave_Click);
            // 
            // btnDbStop
            // 
            this.btnDbStop.Image = ((System.Drawing.Image)(resources.GetObject("btnDbStop.Image")));
            this.btnDbStop.Location = new System.Drawing.Point(735, 9);
            this.btnDbStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnDbStop.Name = "btnDbStop";
            this.btnDbStop.Size = new System.Drawing.Size(137, 42);
            this.btnDbStop.TabIndex = 17;
            this.btnDbStop.Text = "저장 중지";
            this.btnDbStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDbStop.UseVisualStyleBackColor = true;
            this.btnDbStop.Click += new System.EventHandler(this.btnDbStop_Click);
            // 
            // lblTopCell
            // 
            this.lblTopCell.AutoSize = true;
            this.lblTopCell.Location = new System.Drawing.Point(892, 21);
            this.lblTopCell.Name = "lblTopCell";
            this.lblTopCell.Size = new System.Drawing.Size(39, 18);
            this.lblTopCell.TabIndex = 18;
            this.lblTopCell.Text = "Cell";
            // 
            // cboTopCell
            // 
            this.cboTopCell.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTopCell.Enabled = false;
            this.cboTopCell.FormattingEnabled = true;
            this.cboTopCell.Location = new System.Drawing.Point(937, 16);
            this.cboTopCell.Name = "cboTopCell";
            this.cboTopCell.Size = new System.Drawing.Size(360, 26);
            this.cboTopCell.TabIndex = 19;
            this.cboTopCell.SelectionChangeCommitted += new System.EventHandler(this.cboTopCell_SelectionChangeCommitted);
            // 
            // dataGrid
            // 
            this.dataGrid.AllowUserToAddRows = false;
            this.dataGrid.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGrid.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid.Location = new System.Drawing.Point(3, 3);
            this.dataGrid.Margin = new System.Windows.Forms.Padding(4);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGrid.RowHeadersWidth = 62;
            this.dataGrid.RowTemplate.Height = 23;
            this.dataGrid.Size = new System.Drawing.Size(1067, 861);
            this.dataGrid.TabIndex = 3;
            // 
            // GDSContainer
            // 
            this.GDSContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GDSContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GDSContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.GDSContainer.Location = new System.Drawing.Point(0, 60);
            this.GDSContainer.Name = "GDSContainer";
            // 
            // GDSContainer.Panel1
            // 
            this.GDSContainer.Panel1.Controls.Add(this.tabControl1);
            this.GDSContainer.Panel1MinSize = 220;
            // 
            // GDSContainer.Panel2
            // 
            this.GDSContainer.Panel2.Controls.Add(this.chkLayerItems);
            this.GDSContainer.Panel2.Controls.Add(this.lblLayerSummary);
            this.GDSContainer.Panel2.Controls.Add(this.panel2);
            this.GDSContainer.Panel2MinSize = 400;
            this.GDSContainer.Size = new System.Drawing.Size(1488, 900);
            this.GDSContainer.SplitterDistance = 1083;
            this.GDSContainer.TabIndex = 5;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1081, 898);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.map);
            this.tabPage1.Controls.Add(this.statusStripMap);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1073, 866);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Map";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // map
            // 
            this.map.BackColor = System.Drawing.Color.Black;
            this.map.ColorAlpha = 50;
            this.map.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.Location = new System.Drawing.Point(3, 3);
            this.map.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.map.Mode = NexplantQMS.GdsMap.GdsMapControl.ViewMode.View;
            this.map.Name = "map";
            this.map.Size = new System.Drawing.Size(1067, 824);
            this.map.TabIndex = 4;
            this.map.VSync = false;
            this.map.MouseWorldPositionChanged += new System.EventHandler<System.Drawing.PointF>(this.Map_MouseWorldPositionChanged);
            this.map.RenderProgressChanged += new System.EventHandler<NexplantQMS.GdsMap.GdsMapRenderProgressChangedEventArgs>(this.Map_RenderProgressChanged);
            // 
            // statusStripMap
            // 
            this.statusStripMap.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStripMap.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressMapLoad,
            this.lblMapElapsed,
            this.lblMapStatus,
            this.lblMapCoordinate});
            this.statusStripMap.Location = new System.Drawing.Point(3, 827);
            this.statusStripMap.Name = "statusStripMap";
            this.statusStripMap.Size = new System.Drawing.Size(1067, 36);
            this.statusStripMap.SizingGrip = false;
            this.statusStripMap.TabIndex = 5;
            // 
            // progressMapLoad
            // 
            this.progressMapLoad.Name = "progressMapLoad";
            this.progressMapLoad.Size = new System.Drawing.Size(160, 28);
            this.progressMapLoad.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressMapLoad.Visible = false;
            // 
            // lblMapElapsed
            // 
            this.lblMapElapsed.ForeColor = System.Drawing.Color.DimGray;
            this.lblMapElapsed.Name = "lblMapElapsed";
            this.lblMapElapsed.Size = new System.Drawing.Size(0, 29);
            this.lblMapElapsed.Visible = false;
            // 
            // lblMapStatus
            // 
            this.lblMapStatus.Name = "lblMapStatus";
            this.lblMapStatus.Size = new System.Drawing.Size(940, 29);
            this.lblMapStatus.Spring = true;
            this.lblMapStatus.Text = "대기";
            this.lblMapStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMapCoordinate
            // 
            this.lblMapCoordinate.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblMapCoordinate.Name = "lblMapCoordinate";
            this.lblMapCoordinate.Size = new System.Drawing.Size(112, 29);
            this.lblMapCoordinate.Text = "X: — / Y: —";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGrid);
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1073, 867);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Grid";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // chkLayerItems
            // 
            this.chkLayerItems.CheckOnCheckBoxOnly = true;
            this.chkLayerItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkLayerItems.FormattingEnabled = true;
            this.chkLayerItems.Location = new System.Drawing.Point(0, 146);
            this.chkLayerItems.Name = "chkLayerItems";
            this.chkLayerItems.Size = new System.Drawing.Size(399, 752);
            this.chkLayerItems.TabIndex = 4;
            this.chkLayerItems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ChkLayerItems_ItemCheck);
            this.chkLayerItems.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.ChkLayerItems_Format);
            // 
            // lblLayerSummary
            // 
            this.lblLayerSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLayerSummary.Location = new System.Drawing.Point(0, 110);
            this.lblLayerSummary.Name = "lblLayerSummary";
            this.lblLayerSummary.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.lblLayerSummary.Size = new System.Drawing.Size(399, 36);
            this.lblLayerSummary.TabIndex = 9;
            this.lblLayerSummary.Text = "Layer";
            this.lblLayerSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnLayerCheckAll);
            this.panel2.Controls.Add(this.btnLayerCheckNone);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(399, 110);
            this.panel2.TabIndex = 2;
            // 
            // btnLayerCheckAll
            // 
            this.btnLayerCheckAll.Enabled = false;
            this.btnLayerCheckAll.Image = ((System.Drawing.Image)(resources.GetObject("btnLayerCheckAll.Image")));
            this.btnLayerCheckAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLayerCheckAll.Location = new System.Drawing.Point(7, 7);
            this.btnLayerCheckAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnLayerCheckAll.Name = "btnLayerCheckAll";
            this.btnLayerCheckAll.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.btnLayerCheckAll.Size = new System.Drawing.Size(204, 42);
            this.btnLayerCheckAll.TabIndex = 2;
            this.btnLayerCheckAll.Text = "전체 체크";
            this.btnLayerCheckAll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLayerCheckAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLayerCheckAll.UseVisualStyleBackColor = true;
            this.btnLayerCheckAll.Click += new System.EventHandler(this.btnLayerCheckAll_Click);
            // 
            // btnLayerCheckNone
            // 
            this.btnLayerCheckNone.Enabled = false;
            this.btnLayerCheckNone.Image = ((System.Drawing.Image)(resources.GetObject("btnLayerCheckNone.Image")));
            this.btnLayerCheckNone.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLayerCheckNone.Location = new System.Drawing.Point(7, 52);
            this.btnLayerCheckNone.Margin = new System.Windows.Forms.Padding(4);
            this.btnLayerCheckNone.Name = "btnLayerCheckNone";
            this.btnLayerCheckNone.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.btnLayerCheckNone.Size = new System.Drawing.Size(204, 42);
            this.btnLayerCheckNone.TabIndex = 3;
            this.btnLayerCheckNone.Text = "전체 미체크";
            this.btnLayerCheckNone.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLayerCheckNone.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLayerCheckNone.UseVisualStyleBackColor = true;
            this.btnLayerCheckNone.Click += new System.EventHandler(this.btnLayerCheckNone_Click);
            // 
            // GdsMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1488, 960);
            this.Controls.Add(this.GDSContainer);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "GdsMapForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GDS Map Control";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.GDSContainer.Panel1.ResumeLayout(false);
            this.GDSContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GDSContainer)).EndInit();
            this.GDSContainer.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.statusStripMap.ResumeLayout(false);
            this.statusStripMap.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

		#endregion
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.DataGridView dataGrid;
		private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox chkOriginCross;
        private System.Windows.Forms.Label lblDbDevice;
        private System.Windows.Forms.TextBox txtDbDevice;
        private System.Windows.Forms.Button btnDbSave;
        private System.Windows.Forms.Button btnDbStop;
        private System.Windows.Forms.Label lblTopCell;
        private System.Windows.Forms.ComboBox cboTopCell;
		private NexplantQMS.GdsMap.GdsMapControl map;
        private System.Windows.Forms.SplitContainer GDSContainer;
        private NexplantQMS.GdsMap.LayerColorCheckedListBox chkLayerItems;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label lblLayerSummary;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnLayerCheckAll;
        private System.Windows.Forms.Button btnLayerCheckNone;
        private System.Windows.Forms.StatusStrip statusStripMap;
        private System.Windows.Forms.ToolStripProgressBar progressMapLoad;
        private System.Windows.Forms.ToolStripStatusLabel lblMapElapsed;
        private System.Windows.Forms.ToolStripStatusLabel lblMapStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblMapCoordinate;
    }
}

