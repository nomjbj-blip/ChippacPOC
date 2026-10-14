namespace DACrux.SEMDMS.ENGUI
{
    // E-Test 업로드 데이터 조회 창의 컨트롤과 배치를 정의한다. 조회 / 거르기 동작은 본문 파일(frmEtResultView.cs)에서 처리한다.
    partial class frmEtResultView
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>창 종료 시 디자이너 구성 요소를 해제한다.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// 위: 파일 정보 / 거르기(Para / Chain 포함 / '-' 있는 항목만 / 표시 건수).
        /// 왼쪽: Die 목록(더블클릭하면 오른쪽 표가 그 Die 열로 이동), 오른쪽: 측정값 표(행 = 측정 항목, 열 = Die).
        /// </summary>
        private void InitializeComponent()
        {
            this.lblInfo = new System.Windows.Forms.Label();
            this.filterBar = new System.Windows.Forms.Panel();
            this.lblPara = new System.Windows.Forms.Label();
            this.cboPara = new System.Windows.Forms.ComboBox();
            this.lblChain = new System.Windows.Forms.Label();
            this.txtChain = new System.Windows.Forms.TextBox();
            this.chkNotTested = new System.Windows.Forms.CheckBox();
            this.lblCount = new System.Windows.Forms.Label();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.dgvDie = new System.Windows.Forms.DataGridView();
            this.dgvValue = new System.Windows.Forms.DataGridView();
            this.filterBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDie)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvValue)).BeginInit();
            this.SuspendLayout();
            //
            // lblInfo
            //
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblInfo.Location = new System.Drawing.Point(0, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Padding = new System.Windows.Forms.Padding(8, 6, 8, 0);
            this.lblInfo.Size = new System.Drawing.Size(1200, 60);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "조회 중...";
            //
            // filterBar
            //
            this.filterBar.Controls.Add(this.lblPara);
            this.filterBar.Controls.Add(this.cboPara);
            this.filterBar.Controls.Add(this.lblChain);
            this.filterBar.Controls.Add(this.txtChain);
            this.filterBar.Controls.Add(this.chkNotTested);
            this.filterBar.Controls.Add(this.lblCount);
            this.filterBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.filterBar.Location = new System.Drawing.Point(0, 60);
            this.filterBar.Name = "filterBar";
            this.filterBar.Size = new System.Drawing.Size(1200, 34);
            this.filterBar.TabIndex = 1;
            //
            // lblPara
            //
            this.lblPara.AutoSize = true;
            this.lblPara.Location = new System.Drawing.Point(10, 10);
            this.lblPara.Name = "lblPara";
            this.lblPara.Size = new System.Drawing.Size(31, 12);
            this.lblPara.TabIndex = 0;
            this.lblPara.Text = "Para";
            //
            // cboPara
            //
            this.cboPara.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPara.FormattingEnabled = true;
            this.cboPara.Location = new System.Drawing.Point(48, 6);
            this.cboPara.Name = "cboPara";
            this.cboPara.Size = new System.Drawing.Size(240, 20);
            this.cboPara.TabIndex = 1;
            this.cboPara.SelectedIndexChanged += new System.EventHandler(this.Filter_Changed);
            //
            // lblChain
            //
            this.lblChain.AutoSize = true;
            this.lblChain.Location = new System.Drawing.Point(302, 10);
            this.lblChain.Name = "lblChain";
            this.lblChain.Size = new System.Drawing.Size(65, 12);
            this.lblChain.TabIndex = 2;
            this.lblChain.Text = "Chain 포함";
            //
            // txtChain
            //
            this.txtChain.Location = new System.Drawing.Point(372, 6);
            this.txtChain.Name = "txtChain";
            this.txtChain.Size = new System.Drawing.Size(200, 21);
            this.txtChain.TabIndex = 3;
            this.txtChain.TextChanged += new System.EventHandler(this.Filter_Changed);
            //
            // chkNotTested
            //
            this.chkNotTested.AutoSize = true;
            this.chkNotTested.Location = new System.Drawing.Point(588, 8);
            this.chkNotTested.Name = "chkNotTested";
            this.chkNotTested.Size = new System.Drawing.Size(144, 16);
            this.chkNotTested.TabIndex = 4;
            this.chkNotTested.Text = "'-' 있는 항목만";
            this.chkNotTested.UseVisualStyleBackColor = true;
            this.chkNotTested.CheckedChanged += new System.EventHandler(this.Filter_Changed);
            //
            // lblCount
            //
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(750, 10);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(0, 12);
            this.lblCount.TabIndex = 5;
            //
            // splitMain
            //
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 94);
            this.splitMain.Name = "splitMain";
            //
            // splitMain.Panel1
            //
            this.splitMain.Panel1.Controls.Add(this.dgvDie);
            //
            // splitMain.Panel2
            //
            this.splitMain.Panel2.Controls.Add(this.dgvValue);
            this.splitMain.Size = new System.Drawing.Size(1200, 656);
            this.splitMain.SplitterDistance = 430;
            this.splitMain.TabIndex = 2;
            //
            // dgvDie
            //
            this.dgvDie.AllowUserToAddRows = false;
            this.dgvDie.AllowUserToDeleteRows = false;
            this.dgvDie.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDie.Location = new System.Drawing.Point(0, 0);
            this.dgvDie.MultiSelect = false;
            this.dgvDie.Name = "dgvDie";
            this.dgvDie.ReadOnly = true;
            this.dgvDie.RowHeadersVisible = false;
            this.dgvDie.RowTemplate.Height = 23;
            this.dgvDie.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDie.Size = new System.Drawing.Size(430, 672);
            this.dgvDie.TabIndex = 0;
            this.dgvDie.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDie_CellDoubleClick);
            //
            // dgvValue
            //
            this.dgvValue.AllowUserToAddRows = false;
            this.dgvValue.AllowUserToDeleteRows = false;
            this.dgvValue.AllowUserToResizeRows = false;
            this.dgvValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvValue.EnableHeadersVisualStyles = false;
            this.dgvValue.Location = new System.Drawing.Point(0, 0);
            this.dgvValue.Name = "dgvValue";
            this.dgvValue.ReadOnly = true;
            this.dgvValue.RowHeadersVisible = false;
            this.dgvValue.RowTemplate.Height = 21;
            this.dgvValue.Size = new System.Drawing.Size(766, 672);
            this.dgvValue.TabIndex = 0;
            this.dgvValue.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvValue_CellFormatting);
            //
            // frmEtResultView
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 750);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.filterBar);
            this.Controls.Add(this.lblInfo);
            this.Name = "frmEtResultView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "E-Test 업로드 데이터 조회";
            this.filterBar.ResumeLayout(false);
            this.filterBar.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDie)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvValue)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Panel filterBar;
        private System.Windows.Forms.Label lblPara;
        private System.Windows.Forms.ComboBox cboPara;
        private System.Windows.Forms.Label lblChain;
        private System.Windows.Forms.TextBox txtChain;
        private System.Windows.Forms.CheckBox chkNotTested;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.DataGridView dgvDie;
        private System.Windows.Forms.DataGridView dgvValue;
    }
}
