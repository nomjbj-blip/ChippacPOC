namespace NexplantQMS.GdsMap
{
    partial class Form1
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
			this.btnLayerLabelDiagnostic = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnLayerColor = new System.Windows.Forms.Button();
            this.btnLayer = new System.Windows.Forms.Button();
            this.btnLayerCheckNone = new System.Windows.Forms.Button();
            this.btnLayerCheckAll = new System.Windows.Forms.Button();
            this.panelMapHost = new System.Windows.Forms.Panel();
            this.statusStripMap = new System.Windows.Forms.StatusStrip();
            this.progressMapLoad = new System.Windows.Forms.ToolStripProgressBar();
            this.lblMapStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblMapCoordinate = new System.Windows.Forms.ToolStripStatusLabel();
            this.map = new NexplantQMS.GdsMap.GdsMapControl();
            this.checkedListBox1 = new NexplantQMS.GdsMap.LayerColorCheckedListBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panelMapHost.SuspendLayout();
            this.statusStripMap.SuspendLayout();
            this.SuspendLayout();
            //
            // panel1
            //
            this.panel1.Controls.Add(this.btnLoad);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.button1);
			this.panel1.Controls.Add(this.btnLayerLabelDiagnostic);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1303, 66);
            this.panel1.TabIndex = 2;
            //
            // btnLoad
            //
            this.btnLoad.Location = new System.Drawing.Point(323, 18);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(144, 34);
            this.button1.TabIndex = 2;
            this.button1.Text = "Load from File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            //
            // btnLayerLabelDiagnostic
            //
            this.btnLayerLabelDiagnostic.Enabled = false;
            this.btnLayerLabelDiagnostic.Location = new System.Drawing.Point(476, 18);
            this.btnLayerLabelDiagnostic.Margin = new System.Windows.Forms.Padding(4);
            this.btnLayerLabelDiagnostic.Name = "btnLayerLabelDiagnostic";
            this.btnLayerLabelDiagnostic.Size = new System.Drawing.Size(144, 34);
            this.btnLayerLabelDiagnostic.TabIndex = 5;
            this.btnLayerLabelDiagnostic.Text = "Layer 775 진단";
            this.btnLayerLabelDiagnostic.UseVisualStyleBackColor = true;
            this.btnLayerLabelDiagnostic.Click += new System.EventHandler(this.btnLayerLabelDiagnostic_Click);
            //
            // dataGridView1
            //
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.Location = new System.Drawing.Point(0, 937);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1303, 113);
            this.dataGridView1.TabIndex = 3;
            //
            // panel2
            //
            this.panel2.Controls.Add(this.btnLayerColor);
            this.panel2.Controls.Add(this.btnLayer);
            this.panel2.Controls.Add(this.btnLayerCheckNone);
            this.panel2.Controls.Add(this.btnLayerCheckAll);
            this.panel2.Controls.Add(this.checkedListBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 66);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(220, 871);
            this.panel2.TabIndex = 5;
            //
            // btnLayerColor
            //
            this.btnLayerColor.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLayerColor.Enabled = false;
            this.btnLayerColor.Location = new System.Drawing.Point(0, 263);
            this.btnLayerColor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLayerColor.Name = "btnLayerColor";
            this.btnLayerColor.Size = new System.Drawing.Size(220, 42);
            this.btnLayerColor.TabIndex = 2;
            this.btnLayerColor.Text = "색상 변경";
            this.btnLayerColor.UseVisualStyleBackColor = true;
            this.btnLayerColor.Click += new System.EventHandler(this.ChangeSelectedLayerColor);
            //
            // btnLayer
            //
            this.btnLayer.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLayer.Location = new System.Drawing.Point(0, 229);
            this.btnLayer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLayer.Name = "btnLayer";
            this.btnLayer.Size = new System.Drawing.Size(220, 34);
            this.btnLayer.TabIndex = 1;
            this.btnLayer.Text = "Redraw";
            this.btnLayer.UseVisualStyleBackColor = true;
            this.btnLayer.Click += new System.EventHandler(this.btnLayer_Click);
            // 
            // btnLayerCheckNone
            // 
            this.btnLayerCheckNone.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLayerCheckNone.Location = new System.Drawing.Point(0, 263);
            this.btnLayerCheckNone.Margin = new System.Windows.Forms.Padding(4);
            this.btnLayerCheckNone.Name = "btnLayerCheckNone";
            this.btnLayerCheckNone.Size = new System.Drawing.Size(220, 34);
            this.btnLayerCheckNone.TabIndex = 3;
            this.btnLayerCheckNone.Text = "전체 미체크";
            this.btnLayerCheckNone.UseVisualStyleBackColor = true;
            this.btnLayerCheckNone.Click += new System.EventHandler(this.btnLayerCheckNone_Click);
            // 
            // btnLayerCheckAll
            // 
            this.btnLayerCheckAll.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLayerCheckAll.Location = new System.Drawing.Point(0, 229);
            this.btnLayerCheckAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnLayerCheckAll.Name = "btnLayerCheckAll";
            this.btnLayerCheckAll.Size = new System.Drawing.Size(220, 34);
            this.btnLayerCheckAll.TabIndex = 2;
            this.btnLayerCheckAll.Text = "전체 체크";
            this.btnLayerCheckAll.UseVisualStyleBackColor = true;
            this.btnLayerCheckAll.Click += new System.EventHandler(this.btnLayerCheckAll_Click);
            //
            // panelMapHost
            //
            this.panelMapHost.Controls.Add(this.map);
            this.panelMapHost.Controls.Add(this.statusStripMap);
            this.panelMapHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMapHost.Location = new System.Drawing.Point(220, 66);
            this.panelMapHost.Margin = new System.Windows.Forms.Padding(4);
            this.panelMapHost.Name = "panelMapHost";
            this.panelMapHost.Size = new System.Drawing.Size(1083, 871);
            this.panelMapHost.TabIndex = 6;
            //
            // statusStripMap
            //
            this.statusStripMap.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStripMap.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressMapLoad,
            this.lblMapStatus,
            this.lblMapCoordinate});
            this.statusStripMap.Location = new System.Drawing.Point(0, 845);
            this.statusStripMap.Name = "statusStripMap";
            this.statusStripMap.Size = new System.Drawing.Size(1083, 26);
            this.statusStripMap.SizingGrip = false;
            this.statusStripMap.TabIndex = 0;
            //
            // progressMapLoad
            //
            this.progressMapLoad.Name = "progressMapLoad";
            this.progressMapLoad.Size = new System.Drawing.Size(160, 20);
            this.progressMapLoad.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressMapLoad.Visible = false;
            //
            // lblMapStatus
            //
            this.lblMapStatus.Name = "lblMapStatus";
            this.lblMapStatus.Size = new System.Drawing.Size(488, 20);
            this.lblMapStatus.Spring = true;
            this.lblMapStatus.Text = "대기";
            this.lblMapStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblMapCoordinate
            //
            this.lblMapCoordinate.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblMapCoordinate.Name = "lblMapCoordinate";
            this.lblMapCoordinate.Size = new System.Drawing.Size(190, 20);
            this.lblMapCoordinate.Text = "X: — / Y: —";
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
            this.map.Size = new System.Drawing.Size(1083, 845);
            this.map.TabIndex = 4;
            this.map.VSync = false;
            //
            // checkedListBox1
            //
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(0, 0);
            this.checkedListBox1.Margin = new System.Windows.Forms.Padding(4);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(220, 229);
            this.checkedListBox1.TabIndex = 0;
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1303, 1050);
            this.Controls.Add(this.panelMapHost);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panelMapHost.ResumeLayout(false);
            this.panelMapHost.PerformLayout();
            this.statusStripMap.ResumeLayout(false);
            this.statusStripMap.PerformLayout();
            this.ResumeLayout(false);

        }

		#endregion
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button btnSave;
		private NexplantQMS.GdsMap.GdsMapControl map;
		private System.Windows.Forms.Button btnLoad;
		private System.Windows.Forms.Button btnLayerLabelDiagnostic;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button btnLayer;
		private NexplantQMS.GdsMap.LayerColorCheckedListBox checkedListBox1;
        private System.Windows.Forms.Button btnLayerColor;
        private System.Windows.Forms.Button btnLayerCheckNone;
        private System.Windows.Forms.Button btnLayerCheckAll;
        private System.Windows.Forms.Panel panelMapHost;
        private System.Windows.Forms.StatusStrip statusStripMap;
        private System.Windows.Forms.ToolStripProgressBar progressMapLoad;
        private System.Windows.Forms.ToolStripStatusLabel lblMapStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblMapCoordinate;
	}
}

