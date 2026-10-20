namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>연관 분석 [직접 찾기] 창 배치. 설계: 문서/설계/05_연관분석/2026-10-15_GDS_KLARF_ETest_통합분석_데이터_설계.md 16.6절</summary>
    partial class frmGdsKlarfSourceFind
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
        /// 위: 비교용 KLARF 정보 / 찾기 조건(Device / 공정 / Lot 글자 + [조회])
        /// 가운데: 찾은 목록
        /// 아래: 건수 / [추가] / [닫기]
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGdsKlarfSourceFind));
            this.pnlCompare = new System.Windows.Forms.Panel();
            this.lblCompare = new System.Windows.Forms.Label();
            this.pnlFilter = new System.Windows.Forms.Panel();
            this.lblDevice = new System.Windows.Forms.Label();
            this.txtDevice = new System.Windows.Forms.TextBox();
            this.lblProgram = new System.Windows.Forms.Label();
            this.cboProgram = new System.Windows.Forms.ComboBox();
            this.lblLot = new System.Windows.Forms.Label();
            this.txtLot = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lblCount = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlCompare.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlCompare
            //
            this.pnlCompare.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(242)))), ((int)(((byte)(250)))));
            this.pnlCompare.Controls.Add(this.lblCompare);
            this.pnlCompare.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCompare.Location = new System.Drawing.Point(0, 0);
            this.pnlCompare.Name = "pnlCompare";
            this.pnlCompare.Size = new System.Drawing.Size(860, 30);
            this.pnlCompare.TabIndex = 0;
            //
            // lblCompare
            //
            this.lblCompare.AutoSize = true;
            this.lblCompare.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblCompare.Location = new System.Drawing.Point(8, 8);
            this.lblCompare.Name = "lblCompare";
            this.lblCompare.Size = new System.Drawing.Size(60, 15);
            this.lblCompare.TabIndex = 0;
            this.lblCompare.Text = "비교용: -";
            //
            // pnlFilter
            //
            this.pnlFilter.Controls.Add(this.lblDevice);
            this.pnlFilter.Controls.Add(this.txtDevice);
            this.pnlFilter.Controls.Add(this.lblProgram);
            this.pnlFilter.Controls.Add(this.cboProgram);
            this.pnlFilter.Controls.Add(this.lblLot);
            this.pnlFilter.Controls.Add(this.txtLot);
            this.pnlFilter.Controls.Add(this.btnSearch);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilter.Location = new System.Drawing.Point(0, 30);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(860, 40);
            this.pnlFilter.TabIndex = 1;
            //
            // lblDevice
            //
            this.lblDevice.AutoSize = true;
            this.lblDevice.Location = new System.Drawing.Point(8, 14);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(42, 12);
            this.lblDevice.TabIndex = 0;
            this.lblDevice.Text = "Device";
            //
            // txtDevice
            //
            this.txtDevice.Location = new System.Drawing.Point(56, 10);
            this.txtDevice.Name = "txtDevice";
            this.txtDevice.Size = new System.Drawing.Size(110, 21);
            this.txtDevice.TabIndex = 1;
            //
            // lblProgram
            //
            this.lblProgram.AutoSize = true;
            this.lblProgram.Location = new System.Drawing.Point(180, 14);
            this.lblProgram.Name = "lblProgram";
            this.lblProgram.Size = new System.Drawing.Size(29, 12);
            this.lblProgram.TabIndex = 2;
            this.lblProgram.Text = "Step";
            //
            // cboProgram
            //
            this.cboProgram.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProgram.Items.AddRange(new object[] {
            "전체",
            "TSVET",
            "COWET",
            "FT"});
            this.cboProgram.Location = new System.Drawing.Point(214, 10);
            this.cboProgram.Name = "cboProgram";
            this.cboProgram.Size = new System.Drawing.Size(80, 20);
            this.cboProgram.TabIndex = 3;
            //
            // lblLot
            //
            this.lblLot.AutoSize = true;
            this.lblLot.Location = new System.Drawing.Point(308, 14);
            this.lblLot.Name = "lblLot";
            this.lblLot.Size = new System.Drawing.Size(21, 12);
            this.lblLot.TabIndex = 4;
            this.lblLot.Text = "Lot";
            //
            // txtLot
            //
            this.txtLot.Location = new System.Drawing.Point(334, 10);
            this.txtLot.Name = "txtLot";
            this.txtLot.Size = new System.Drawing.Size(200, 21);
            this.txtLot.TabIndex = 5;
            this.txtLot.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFilter_KeyDown);
            //
            // btnSearch
            //
            this.btnSearch.Location = new System.Drawing.Point(546, 6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.Size = new System.Drawing.Size(88, 28);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            //
            // dgvList
            //
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.AllowUserToResizeRows = false;
            this.dgvList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvList.BackgroundColor = System.Drawing.Color.White;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvList.Location = new System.Drawing.Point(0, 70);
            this.dgvList.Name = "dgvList";
            this.dgvList.ReadOnly = true;
            this.dgvList.RowHeadersVisible = false;
            this.dgvList.RowTemplate.Height = 23;
            this.dgvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvList.Size = new System.Drawing.Size(860, 354);
            this.dgvList.TabIndex = 2;
            this.dgvList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellDoubleClick);
            //
            // pnlBottom
            //
            this.pnlBottom.Controls.Add(this.lblCount);
            this.pnlBottom.Controls.Add(this.btnAdd);
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 420);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(860, 40);
            this.pnlBottom.TabIndex = 3;
            //
            // lblCount
            //
            this.lblCount.AutoSize = true;
            this.lblCount.ForeColor = System.Drawing.Color.DimGray;
            this.lblCount.Location = new System.Drawing.Point(8, 14);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(40, 12);
            this.lblCount.TabIndex = 0;
            this.lblCount.Text = "0 건";
            //
            // btnAdd
            //
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(670, 6);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.Size = new System.Drawing.Size(88, 28);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "추가";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            //
            // btnClose
            //
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(764, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Size = new System.Drawing.Size(88, 28);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = true;
            //
            // frmGdsKlarfSourceFind
            //
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(860, 460);
            this.Controls.Add(this.dgvList);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.pnlCompare);
            this.MinimizeBox = false;
            this.Name = "frmGdsKlarfSourceFind";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "직접 찾기";
            this.pnlCompare.ResumeLayout(false);
            this.pnlCompare.PerformLayout();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlCompare;
        private System.Windows.Forms.Label lblCompare;
        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.TextBox txtDevice;
        private System.Windows.Forms.Label lblProgram;
        private System.Windows.Forms.ComboBox cboProgram;
        private System.Windows.Forms.Label lblLot;
        private System.Windows.Forms.TextBox txtLot;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnClose;
    }
}
