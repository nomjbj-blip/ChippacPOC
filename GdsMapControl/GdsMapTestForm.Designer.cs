namespace NexplantQMS.GdsMap
{
    partial class GdsMapTestForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnLayer = new System.Windows.Forms.Button();
            this.btnLayerCheckNone = new System.Windows.Forms.Button();
            this.btnLayerCheckAll = new System.Windows.Forms.Button();
            this.map = new NexplantQMS.GdsMap.GdsMapControl();
            this.statusStripMap = new System.Windows.Forms.StatusStrip();
            this.progressMapLoad = new System.Windows.Forms.ToolStripProgressBar();
            this.lblMapStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblMapCoordinate = new System.Windows.Forms.ToolStripStatusLabel();
            this.GDSContainer = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkLayerItems = new NexplantQMS.GdsMap.LayerColorCheckedListBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.statusStripMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GDSContainer)).BeginInit();
            this.GDSContainer.Panel1.SuspendLayout();
            this.GDSContainer.Panel2.SuspendLayout();
            this.GDSContainer.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnLoad);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1785, 60);
            this.panel1.TabIndex = 2;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(324, 18);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(144, 34);
            this.btnLoad.TabIndex = 4;
            this.btnLoad.Text = "Load DB";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(170, 18);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(144, 34);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save DB";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(17, 18);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(144, 34);
            this.button1.TabIndex = 2;
            this.button1.Text = "Load from File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.Location = new System.Drawing.Point(0, 870);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1785, 240);
            this.dataGridView1.TabIndex = 3;
            // 
            // btnLayer
            // 
            this.btnLayer.Enabled = false;
            this.btnLayer.Location = new System.Drawing.Point(12, 92);
            this.btnLayer.Margin = new System.Windows.Forms.Padding(4);
            this.btnLayer.Name = "btnLayer";
            this.btnLayer.Size = new System.Drawing.Size(194, 34);
            this.btnLayer.TabIndex = 1;
            this.btnLayer.Text = "Redraw";
            this.btnLayer.UseVisualStyleBackColor = true;
            this.btnLayer.Click += new System.EventHandler(this.btnLayer_Click);
            // 
            // btnLayerCheckNone
            // 
            this.btnLayerCheckNone.Enabled = false;
            this.btnLayerCheckNone.Location = new System.Drawing.Point(12, 52);
            this.btnLayerCheckNone.Margin = new System.Windows.Forms.Padding(4);
            this.btnLayerCheckNone.Name = "btnLayerCheckNone";
            this.btnLayerCheckNone.Size = new System.Drawing.Size(194, 34);
            this.btnLayerCheckNone.TabIndex = 3;
            this.btnLayerCheckNone.Text = "전체 미체크";
            this.btnLayerCheckNone.UseVisualStyleBackColor = true;
            this.btnLayerCheckNone.Click += new System.EventHandler(this.btnLayerCheckNone_Click);
            // 
            // btnLayerCheckAll
            // 
            this.btnLayerCheckAll.Enabled = false;
            this.btnLayerCheckAll.Location = new System.Drawing.Point(12, 7);
            this.btnLayerCheckAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnLayerCheckAll.Name = "btnLayerCheckAll";
            this.btnLayerCheckAll.Size = new System.Drawing.Size(194, 37);
            this.btnLayerCheckAll.TabIndex = 2;
            this.btnLayerCheckAll.Text = "전체 체크";
            this.btnLayerCheckAll.UseVisualStyleBackColor = true;
            this.btnLayerCheckAll.Click += new System.EventHandler(this.btnLayerCheckAll_Click);
            // 
            // map
            // 
            this.map.BackColor = System.Drawing.Color.Black;
            this.map.ColorAlpha = 50;
            this.map.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.Location = new System.Drawing.Point(0, 0);
            this.map.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.map.Mode = NexplantQMS.GdsMap.GdsMapControl.ViewMode.View;
            this.map.Name = "map";
            this.map.Size = new System.Drawing.Size(1549, 772);
            this.map.TabIndex = 4;
            this.map.VSync = false;
            // 
            // statusStripMap
            // 
            this.statusStripMap.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStripMap.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressMapLoad,
            this.lblMapStatus,
            this.lblMapCoordinate});
            this.statusStripMap.Location = new System.Drawing.Point(0, 772);
            this.statusStripMap.Name = "statusStripMap";
            this.statusStripMap.Size = new System.Drawing.Size(1549, 36);
            this.statusStripMap.SizingGrip = false;
            this.statusStripMap.TabIndex = 0;
            // 
            // progressMapLoad
            // 
            this.progressMapLoad.Name = "progressMapLoad";
            this.progressMapLoad.Size = new System.Drawing.Size(160, 28);
            this.progressMapLoad.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressMapLoad.Visible = false;
            // 
            // lblMapStatus
            // 
            this.lblMapStatus.Name = "lblMapStatus";
            this.lblMapStatus.Size = new System.Drawing.Size(1422, 29);
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
            // GDSContainer
            // 
            this.GDSContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GDSContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GDSContainer.Location = new System.Drawing.Point(0, 60);
            this.GDSContainer.Name = "GDSContainer";
            // 
            // GDSContainer.Panel1
            // 
            this.GDSContainer.Panel1.Controls.Add(this.panel2);
            this.GDSContainer.Panel1MinSize = 220;
            // 
            // GDSContainer.Panel2
            // 
            this.GDSContainer.Panel2.Controls.Add(this.map);
            this.GDSContainer.Panel2.Controls.Add(this.statusStripMap);
            this.GDSContainer.Panel2MinSize = 400;
            this.GDSContainer.Size = new System.Drawing.Size(1785, 810);
            this.GDSContainer.SplitterDistance = 230;
            this.GDSContainer.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnLayerCheckAll);
            this.panel2.Controls.Add(this.btnLayer);
            this.panel2.Controls.Add(this.btnLayerCheckNone);
            this.panel2.Controls.Add(this.chkLayerItems);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(228, 808);
            this.panel2.TabIndex = 0;
            // 
            // chkLayerItems
            // 
            this.chkLayerItems.CheckOnClick = true;
            this.chkLayerItems.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.chkLayerItems.FormattingEnabled = true;
            this.chkLayerItems.Location = new System.Drawing.Point(0, 179);
            this.chkLayerItems.Name = "chkLayerItems";
            this.chkLayerItems.Size = new System.Drawing.Size(228, 629);
            this.chkLayerItems.TabIndex = 4;
            // 
            // GdsMapTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1785, 1110);
            this.Controls.Add(this.GDSContainer);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "GdsMapTestForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GDS Map Test";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.statusStripMap.ResumeLayout(false);
            this.statusStripMap.PerformLayout();
            this.GDSContainer.Panel1.ResumeLayout(false);
            this.GDSContainer.Panel2.ResumeLayout(false);
            this.GDSContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GDSContainer)).EndInit();
            this.GDSContainer.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

		#endregion
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button btnSave;
		private NexplantQMS.GdsMap.GdsMapControl map;
		private System.Windows.Forms.Button btnLoad;
		private System.Windows.Forms.Button btnLayer;
        private System.Windows.Forms.Button btnLayerCheckNone;
        private System.Windows.Forms.Button btnLayerCheckAll;
        private System.Windows.Forms.StatusStrip statusStripMap;
        private System.Windows.Forms.ToolStripProgressBar progressMapLoad;
        private System.Windows.Forms.ToolStripStatusLabel lblMapStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblMapCoordinate;
        private System.Windows.Forms.SplitContainer GDSContainer;
        private System.Windows.Forms.Panel panel2;
        private NexplantQMS.GdsMap.LayerColorCheckedListBox chkLayerItems;
    }
}

