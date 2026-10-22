namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>DACrux MDI 화면에서 왼쪽 Die 목록과 오른쪽 Die 상세 분석 Form(GDS DLL)을 표시할 영역을 디자이너에 정의한다.</summary>
    partial class frmGdsDieAnalysis
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.Panel pnlListHead;
        private System.Windows.Forms.Label lblListTitle;
        private System.Windows.Forms.Label lblListCount;
        private System.Windows.Forms.DataGridView dgvDies;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colX;
        private System.Windows.Forms.DataGridViewTextBoxColumn colY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDefect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInChain;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEtFail;
        private System.Windows.Forms.Timer tmrSelect;
        private System.Windows.Forms.Panel pnlGdsMap;
        private System.Windows.Forms.Label lblShared;

        /// <summary>호스트 화면이 닫힐 때 공유 GDS 상세 Form 은 먼저 떼어 두고(해제는 공유 규칙에 따름, 2026-10-16) 컨트롤을 해제한다.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing) Detach();
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// 왼쪽 = Die 목록(머리 줄 + 표, 폭 고정), 오른쪽 = GDS 상세 Form 패널(공유 화면이 다른 창에 있을 때는 안내 문구).
        /// 목록 선택은 0.3초 멈춘 뒤 반영한다(tmrSelect).
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.dgvDies = new System.Windows.Forms.DataGridView();
            this.colNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDefect = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInChain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEtFail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlListHead = new System.Windows.Forms.Panel();
            this.lblListCount = new System.Windows.Forms.Label();
            this.lblListTitle = new System.Windows.Forms.Label();
            this.pnlGdsMap = new System.Windows.Forms.Panel();
            this.lblShared = new System.Windows.Forms.Label();
            this.tmrSelect = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDies)).BeginInit();
            this.pnlListHead.SuspendLayout();
            this.pnlGdsMap.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitMain.Location = new System.Drawing.Point(0, 0);
            this.splitMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.dgvDies);
            this.splitMain.Panel1.Controls.Add(this.pnlListHead);
            this.splitMain.Panel1MinSize = 150;
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.pnlGdsMap);
            this.splitMain.Size = new System.Drawing.Size(1714, 1200);
            this.splitMain.SplitterDistance = 300;
            this.splitMain.SplitterWidth = 7;
            this.splitMain.TabIndex = 0;
            // 
            // dgvDies
            // 
            this.dgvDies.AllowUserToAddRows = false;
            this.dgvDies.AllowUserToDeleteRows = false;
            this.dgvDies.AllowUserToResizeRows = false;
            this.dgvDies.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDies.BackgroundColor = System.Drawing.Color.White;
            this.dgvDies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDies.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNo,
            this.colX,
            this.colY,
            this.colDefect,
            this.colInChain,
            this.colEtFail});
            this.dgvDies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDies.Location = new System.Drawing.Point(0, 45);
            this.dgvDies.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvDies.MultiSelect = false;
            this.dgvDies.Name = "dgvDies";
            this.dgvDies.ReadOnly = true;
            this.dgvDies.RowHeadersVisible = false;
            this.dgvDies.RowHeadersWidth = 62;
            this.dgvDies.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDies.Size = new System.Drawing.Size(300, 1155);
            this.dgvDies.TabIndex = 1;
            this.dgvDies.SelectionChanged += new System.EventHandler(this.dgvDies_SelectionChanged);
            // 
            // colNo
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colNo.DefaultCellStyle = dataGridViewCellStyle1;
            this.colNo.FillWeight = 40F;
            this.colNo.HeaderText = "No";
            this.colNo.MinimumWidth = 8;
            this.colNo.Name = "colNo";
            this.colNo.ReadOnly = true;
            this.colNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colX
            // 
            this.colX.DefaultCellStyle = dataGridViewCellStyle1;
            this.colX.FillWeight = 40F;
            this.colX.HeaderText = "X";
            this.colX.MinimumWidth = 8;
            this.colX.Name = "colX";
            this.colX.ReadOnly = true;
            this.colX.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colY
            // 
            this.colY.DefaultCellStyle = dataGridViewCellStyle1;
            this.colY.FillWeight = 40F;
            this.colY.HeaderText = "Y";
            this.colY.MinimumWidth = 8;
            this.colY.Name = "colY";
            this.colY.ReadOnly = true;
            this.colY.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colDefect
            // 
            this.colDefect.DefaultCellStyle = dataGridViewCellStyle1;
            this.colDefect.FillWeight = 55F;
            this.colDefect.HeaderText = "Defect";
            this.colDefect.MinimumWidth = 8;
            this.colDefect.Name = "colDefect";
            this.colDefect.ReadOnly = true;
            this.colDefect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colInChain
            // 
            this.colInChain.DefaultCellStyle = dataGridViewCellStyle1;
            this.colInChain.FillWeight = 60F;
            this.colInChain.HeaderText = "Chain 위";
            this.colInChain.MinimumWidth = 8;
            this.colInChain.Name = "colInChain";
            this.colInChain.ReadOnly = true;
            this.colInChain.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colEtFail
            // 
            this.colEtFail.DefaultCellStyle = dataGridViewCellStyle1;
            this.colEtFail.FillWeight = 65F;
            this.colEtFail.HeaderText = "E-Test Fail";
            this.colEtFail.MinimumWidth = 8;
            this.colEtFail.Name = "colEtFail";
            this.colEtFail.ReadOnly = true;
            this.colEtFail.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colEtFail.Visible = false;
            // 
            // pnlListHead
            // 
            this.pnlListHead.Controls.Add(this.lblListCount);
            this.pnlListHead.Controls.Add(this.lblListTitle);
            this.pnlListHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlListHead.Location = new System.Drawing.Point(0, 0);
            this.pnlListHead.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlListHead.Name = "pnlListHead";
            this.pnlListHead.Padding = new System.Windows.Forms.Padding(9, 0, 9, 0);
            this.pnlListHead.Size = new System.Drawing.Size(300, 45);
            this.pnlListHead.TabIndex = 0;
            // 
            // lblListCount
            // 
            this.lblListCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblListCount.ForeColor = System.Drawing.Color.DimGray;
            this.lblListCount.Location = new System.Drawing.Point(91, 0);
            this.lblListCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblListCount.Name = "lblListCount";
            this.lblListCount.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.lblListCount.Size = new System.Drawing.Size(200, 45);
            this.lblListCount.TabIndex = 1;
            this.lblListCount.Text = "0";
            this.lblListCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblListTitle
            // 
            this.lblListTitle.AutoSize = true;
            this.lblListTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblListTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblListTitle.Location = new System.Drawing.Point(9, 0);
            this.lblListTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblListTitle.Name = "lblListTitle";
            this.lblListTitle.Size = new System.Drawing.Size(82, 25);
            this.lblListTitle.TabIndex = 0;
            this.lblListTitle.Text = "Die 목록";
            this.lblListTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlGdsMap
            // 
            this.pnlGdsMap.Controls.Add(this.lblShared);
            this.pnlGdsMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGdsMap.Location = new System.Drawing.Point(0, 0);
            this.pnlGdsMap.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlGdsMap.Name = "pnlGdsMap";
            this.pnlGdsMap.Size = new System.Drawing.Size(1407, 1200);
            this.pnlGdsMap.TabIndex = 0;
            // 
            // lblShared
            // 
            this.lblShared.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblShared.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblShared.Location = new System.Drawing.Point(0, 0);
            this.lblShared.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblShared.Name = "lblShared";
            this.lblShared.Size = new System.Drawing.Size(1407, 1200);
            this.lblShared.TabIndex = 0;
            this.lblShared.Text = "다른 상세보기 창에 도면을 표시하고 있습니다.\r\n이 창을 누르면 여기에 표시합니다.";
            this.lblShared.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblShared.Visible = false;
            this.lblShared.Click += new System.EventHandler(this.lblShared_Click);
            // 
            // tmrSelect
            // 
            this.tmrSelect.Interval = 300;
            this.tmrSelect.Tick += new System.EventHandler(this.tmrSelect_Tick);
            // 
            // frmGdsDieAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1714, 1200);
            this.Controls.Add(this.splitMain);
            this.Margin = new System.Windows.Forms.Padding(9, 9, 9, 9);
            this.Name = "frmGdsDieAnalysis";
            this.Text = "상세보기";
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDies)).EndInit();
            this.pnlListHead.ResumeLayout(false);
            this.pnlListHead.PerformLayout();
            this.pnlGdsMap.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
