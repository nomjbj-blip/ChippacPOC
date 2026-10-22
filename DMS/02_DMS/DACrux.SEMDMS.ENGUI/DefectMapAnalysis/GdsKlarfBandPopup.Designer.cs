namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// 색 구간 설정 팝업 배치. 연관 분석 화면 A 의 색 구간 표(pnlBand)와 같은 모양이다.
    /// 설계: 문서/설계/05_연관분석/2026-10-22_GDS_KLARF_연관분석_Simple_화면_설계.md 3.5절
    /// </summary>
    partial class GdsKlarfBandPopup
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
        /// 위: 제목 / 자동 구간 / 표시 기준, 아래: 색 구간 표(FROM / TO / 색 / Die 수).
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlHead = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.chkAutoBand = new System.Windows.Forms.CheckBox();
            this.lblThreshold = new System.Windows.Forms.Label();
            this.numThreshold = new System.Windows.Forms.NumericUpDown();
            this.dgvBand = new System.Windows.Forms.DataGridView();
            this.colBandFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBandTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBandColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBandDies = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.pnlHead.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBand)).BeginInit();
            this.SuspendLayout();
            //
            // pnlHead
            //
            this.pnlHead.Controls.Add(this.lblTitle);
            this.pnlHead.Controls.Add(this.chkAutoBand);
            this.pnlHead.Controls.Add(this.lblThreshold);
            this.pnlHead.Controls.Add(this.numThreshold);
            this.pnlHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHead.Location = new System.Drawing.Point(0, 0);
            this.pnlHead.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlHead.Name = "pnlHead";
            this.pnlHead.Size = new System.Drawing.Size(640, 54);
            this.pnlHead.TabIndex = 0;
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(6, 12);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(250, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "색 구간 - Chain 위 Defect 수";
            //
            // chkAutoBand
            //
            this.chkAutoBand.AutoSize = true;
            this.chkAutoBand.Checked = true;
            this.chkAutoBand.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoBand.Location = new System.Drawing.Point(300, 14);
            this.chkAutoBand.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkAutoBand.Name = "chkAutoBand";
            this.chkAutoBand.Size = new System.Drawing.Size(112, 22);
            this.chkAutoBand.TabIndex = 1;
            this.chkAutoBand.Text = "자동 구간";
            this.chkAutoBand.UseVisualStyleBackColor = true;
            this.chkAutoBand.CheckedChanged += new System.EventHandler(this.chkAutoBand_CheckedChanged);
            //
            // lblThreshold
            //
            this.lblThreshold.AutoSize = true;
            this.lblThreshold.Location = new System.Drawing.Point(430, 18);
            this.lblThreshold.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblThreshold.Name = "lblThreshold";
            this.lblThreshold.Size = new System.Drawing.Size(86, 18);
            this.lblThreshold.TabIndex = 2;
            this.lblThreshold.Text = "표시 기준";
            //
            // numThreshold
            //
            this.numThreshold.Location = new System.Drawing.Point(516, 12);
            this.numThreshold.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numThreshold.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numThreshold.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numThreshold.Name = "numThreshold";
            this.numThreshold.Size = new System.Drawing.Size(80, 28);
            this.numThreshold.TabIndex = 3;
            this.numThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numThreshold.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numThreshold.ValueChanged += new System.EventHandler(this.numThreshold_ValueChanged);
            //
            // dgvBand
            //
            this.dgvBand.AllowUserToAddRows = false;
            this.dgvBand.AllowUserToDeleteRows = false;
            this.dgvBand.AllowUserToResizeRows = false;
            this.dgvBand.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBand.BackgroundColor = System.Drawing.Color.White;
            this.dgvBand.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBand.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBandFrom,
            this.colBandTo,
            this.colBandColor,
            this.colBandDies});
            this.dgvBand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBand.Location = new System.Drawing.Point(0, 54);
            this.dgvBand.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvBand.MultiSelect = false;
            this.dgvBand.Name = "dgvBand";
            this.dgvBand.RowHeadersVisible = false;
            this.dgvBand.RowHeadersWidth = 62;
            this.dgvBand.RowTemplate.Height = 23;
            this.dgvBand.Size = new System.Drawing.Size(640, 366);
            this.dgvBand.TabIndex = 1;
            this.dgvBand.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBand_CellDoubleClick);
            this.dgvBand.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBand_CellEndEdit);
            //
            // colBandFrom
            //
            this.colBandFrom.HeaderText = "FROM";
            this.colBandFrom.MinimumWidth = 8;
            this.colBandFrom.Name = "colBandFrom";
            //
            // colBandTo
            //
            this.colBandTo.HeaderText = "TO";
            this.colBandTo.MinimumWidth = 8;
            this.colBandTo.Name = "colBandTo";
            //
            // colBandColor
            //
            this.colBandColor.HeaderText = "색";
            this.colBandColor.MinimumWidth = 8;
            this.colBandColor.Name = "colBandColor";
            this.colBandColor.ReadOnly = true;
            //
            // colBandDies
            //
            this.colBandDies.HeaderText = "Die 수";
            this.colBandDies.MinimumWidth = 8;
            this.colBandDies.Name = "colBandDies";
            this.colBandDies.ReadOnly = true;
            //
            // colorDialog
            //
            this.colorDialog.FullOpen = true;
            //
            // GdsKlarfBandPopup
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 420);
            this.Controls.Add(this.dgvBand);
            this.Controls.Add(this.pnlHead);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "GdsKlarfBandPopup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "색 구간 설정";
            this.pnlHead.ResumeLayout(false);
            this.pnlHead.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBand)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHead;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.CheckBox chkAutoBand;
        private System.Windows.Forms.Label lblThreshold;
        private System.Windows.Forms.NumericUpDown numThreshold;
        private System.Windows.Forms.DataGridView dgvBand;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBandFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBandTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBandColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBandDies;
        private System.Windows.Forms.ColorDialog colorDialog;
    }
}
