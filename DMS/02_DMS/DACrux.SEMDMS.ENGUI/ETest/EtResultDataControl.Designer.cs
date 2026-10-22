namespace DACrux.SEMDMS.ENGUI
{
    // E-Test 결과 공통 Control 의 배치를 정의한다. 채우기 / 거르기 동작은 본문 파일(EtResultDataControl.cs)에서 처리한다. (2026-10-15)
    partial class EtResultDataControl
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>Control 종료 시 디자이너 구성 요소를 해제한다.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// 왼쪽: 정보 표(항목 / 값). 오른쪽 위: 1단계 Die 표. 오른쪽 아래: 2단계 정의 Para 값 표 + 거르기(Fail 만 / '-' 만 / 찾기).
        /// </summary>
        private void InitializeComponent()
        {
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.dgvInfo = new System.Windows.Forms.DataGridView();
            this.colInfoName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInfoValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblInfoTitle = new System.Windows.Forms.Label();
            this.splitData = new System.Windows.Forms.SplitContainer();
            this.dgvDie = new System.Windows.Forms.DataGridView();
            this.colDDut = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDMap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDJudge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDFail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDNotTested = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDFlag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblDieTitle = new System.Windows.Forms.Label();
            this.dgvValue = new System.Windows.Forms.DataGridView();
            this.colVDut = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVXY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVHeader = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVChain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVGds = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVLower = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVUpper = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVJudge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVReason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueBar = new System.Windows.Forms.Panel();
            this.lblValueTitle = new System.Windows.Forms.Label();
            this.chkFailOnly = new System.Windows.Forms.CheckBox();
            this.chkNotTested = new System.Windows.Forms.CheckBox();
            this.lblFind = new System.Windows.Forms.Label();
            this.txtFind = new System.Windows.Forms.TextBox();
            this.lblCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitData)).BeginInit();
            this.splitData.Panel1.SuspendLayout();
            this.splitData.Panel2.SuspendLayout();
            this.splitData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDie)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvValue)).BeginInit();
            this.valueBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 0);
            this.splitMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.dgvInfo);
            this.splitMain.Panel1.Controls.Add(this.lblInfoTitle);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.splitData);
            this.splitMain.Size = new System.Drawing.Size(1857, 630);
            this.splitMain.SplitterDistance = 471;
            this.splitMain.SplitterWidth = 6;
            this.splitMain.TabIndex = 0;
            // 
            // dgvInfo
            // 
            this.dgvInfo.AllowUserToAddRows = false;
            this.dgvInfo.AllowUserToDeleteRows = false;
            this.dgvInfo.AllowUserToResizeRows = false;
            this.dgvInfo.ColumnHeadersHeight = 34;
            this.dgvInfo.ColumnHeadersVisible = false;
            this.dgvInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colInfoName,
            this.colInfoValue});
            this.dgvInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInfo.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvInfo.Location = new System.Drawing.Point(0, 33);
            this.dgvInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvInfo.Name = "dgvInfo";
            this.dgvInfo.RowHeadersVisible = false;
            this.dgvInfo.RowHeadersWidth = 62;
            this.dgvInfo.RowTemplate.Height = 21;
            this.dgvInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvInfo.Size = new System.Drawing.Size(471, 597);
            this.dgvInfo.TabIndex = 1;
            this.dgvInfo.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvInfo_CellDoubleClick);
            this.dgvInfo.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvInfo_CellEndEdit);
            // 
            // colInfoName
            // 
            this.colInfoName.HeaderText = "항목";
            this.colInfoName.MinimumWidth = 8;
            this.colInfoName.Name = "colInfoName";
            this.colInfoName.ReadOnly = true;
            this.colInfoName.Width = 130;
            // 
            // colInfoValue
            // 
            this.colInfoValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colInfoValue.HeaderText = "값";
            this.colInfoValue.MinimumWidth = 8;
            this.colInfoValue.Name = "colInfoValue";
            // 
            // lblInfoTitle
            // 
            this.lblInfoTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblInfoTitle.Location = new System.Drawing.Point(0, 0);
            this.lblInfoTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInfoTitle.Name = "lblInfoTitle";
            this.lblInfoTitle.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lblInfoTitle.Size = new System.Drawing.Size(471, 33);
            this.lblInfoTitle.TabIndex = 0;
            this.lblInfoTitle.Text = "결과 정보";
            this.lblInfoTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitData
            // 
            this.splitData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitData.Location = new System.Drawing.Point(0, 0);
            this.splitData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitData.Name = "splitData";
            this.splitData.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitData.Panel1
            // 
            this.splitData.Panel1.Controls.Add(this.dgvDie);
            this.splitData.Panel1.Controls.Add(this.lblDieTitle);
            // 
            // splitData.Panel2
            // 
            this.splitData.Panel2.Controls.Add(this.dgvValue);
            this.splitData.Panel2.Controls.Add(this.valueBar);
            this.splitData.Size = new System.Drawing.Size(1380, 630);
            this.splitData.SplitterDistance = 255;
            this.splitData.SplitterWidth = 6;
            this.splitData.TabIndex = 0;
            // 
            // dgvDie
            // 
            this.dgvDie.AllowUserToAddRows = false;
            this.dgvDie.AllowUserToDeleteRows = false;
            this.dgvDie.AllowUserToResizeRows = false;
            this.dgvDie.ColumnHeadersHeight = 34;
            this.dgvDie.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDDut,
            this.colDX,
            this.colDY,
            this.colDMap,
            this.colDJudge,
            this.colDFail,
            this.colDNotTested,
            this.colDFlag});
            this.dgvDie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDie.Location = new System.Drawing.Point(0, 33);
            this.dgvDie.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvDie.Name = "dgvDie";
            this.dgvDie.ReadOnly = true;
            this.dgvDie.RowHeadersVisible = false;
            this.dgvDie.RowHeadersWidth = 62;
            this.dgvDie.RowTemplate.Height = 21;
            this.dgvDie.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDie.Size = new System.Drawing.Size(1380, 222);
            this.dgvDie.TabIndex = 1;
            this.dgvDie.SelectionChanged += new System.EventHandler(this.dgvDie_SelectionChanged);
            // 
            // colDDut
            // 
            this.colDDut.HeaderText = "DUT";
            this.colDDut.MinimumWidth = 8;
            this.colDDut.Name = "colDDut";
            this.colDDut.ReadOnly = true;
            this.colDDut.Width = 55;
            // 
            // colDX
            // 
            this.colDX.HeaderText = "X";
            this.colDX.MinimumWidth = 8;
            this.colDX.Name = "colDX";
            this.colDX.ReadOnly = true;
            this.colDX.Width = 40;
            // 
            // colDY
            // 
            this.colDY.HeaderText = "Y";
            this.colDY.MinimumWidth = 8;
            this.colDY.Name = "colDY";
            this.colDY.ReadOnly = true;
            this.colDY.Width = 40;
            // 
            // colDMap
            // 
            this.colDMap.HeaderText = "Map";
            this.colDMap.MinimumWidth = 8;
            this.colDMap.Name = "colDMap";
            this.colDMap.ReadOnly = true;
            this.colDMap.Width = 75;
            // 
            // colDJudge
            // 
            this.colDJudge.HeaderText = "판정";
            this.colDJudge.MinimumWidth = 8;
            this.colDJudge.Name = "colDJudge";
            this.colDJudge.ReadOnly = true;
            this.colDJudge.Width = 70;
            // 
            // colDFail
            // 
            this.colDFail.HeaderText = "Fail 수";
            this.colDFail.MinimumWidth = 8;
            this.colDFail.Name = "colDFail";
            this.colDFail.ReadOnly = true;
            this.colDFail.Width = 55;
            // 
            // colDNotTested
            // 
            this.colDNotTested.HeaderText = "\'-\' 수";
            this.colDNotTested.MinimumWidth = 8;
            this.colDNotTested.Name = "colDNotTested";
            this.colDNotTested.ReadOnly = true;
            this.colDNotTested.Width = 50;
            // 
            // colDFlag
            // 
            this.colDFlag.HeaderText = "DUT Flag";
            this.colDFlag.MinimumWidth = 8;
            this.colDFlag.Name = "colDFlag";
            this.colDFlag.ReadOnly = true;
            this.colDFlag.Width = 120;
            // 
            // lblDieTitle
            // 
            this.lblDieTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDieTitle.Location = new System.Drawing.Point(0, 0);
            this.lblDieTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDieTitle.Name = "lblDieTitle";
            this.lblDieTitle.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lblDieTitle.Size = new System.Drawing.Size(1380, 33);
            this.lblDieTitle.TabIndex = 0;
            this.lblDieTitle.Text = "1단계: Die";
            this.lblDieTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvValue
            // 
            this.dgvValue.AllowUserToAddRows = false;
            this.dgvValue.AllowUserToDeleteRows = false;
            this.dgvValue.AllowUserToResizeRows = false;
            this.dgvValue.ColumnHeadersHeight = 34;
            this.dgvValue.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colVDut,
            this.colVXY,
            this.colVName,
            this.colVHeader,
            this.colVChain,
            this.colVGds,
            this.colVUnit,
            this.colVValue,
            this.colVLower,
            this.colVUpper,
            this.colVJudge,
            this.colVReason});
            this.dgvValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvValue.Location = new System.Drawing.Point(0, 42);
            this.dgvValue.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvValue.Name = "dgvValue";
            this.dgvValue.ReadOnly = true;
            this.dgvValue.RowHeadersVisible = false;
            this.dgvValue.RowHeadersWidth = 62;
            this.dgvValue.RowTemplate.Height = 21;
            this.dgvValue.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvValue.Size = new System.Drawing.Size(1380, 327);
            this.dgvValue.TabIndex = 1;
            this.dgvValue.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvValue_CellFormatting);
            // 
            // colVDut
            // 
            this.colVDut.DataPropertyName = "DUT";
            this.colVDut.HeaderText = "DUT";
            this.colVDut.MinimumWidth = 8;
            this.colVDut.Name = "colVDut";
            this.colVDut.ReadOnly = true;
            this.colVDut.Width = 55;
            // 
            // colVXY
            // 
            this.colVXY.DataPropertyName = "XY";
            this.colVXY.HeaderText = "X,Y";
            this.colVXY.MinimumWidth = 8;
            this.colVXY.Name = "colVXY";
            this.colVXY.ReadOnly = true;
            this.colVXY.Width = 60;
            // 
            // colVName
            // 
            this.colVName.DataPropertyName = "NAME";
            this.colVName.HeaderText = "Parameter";
            this.colVName.MinimumWidth = 8;
            this.colVName.Name = "colVName";
            this.colVName.ReadOnly = true;
            this.colVName.Width = 220;
            // 
            // colVHeader
            // 
            this.colVHeader.DataPropertyName = "HEADER";
            this.colVHeader.HeaderText = "헤더 원문";
            this.colVHeader.MinimumWidth = 8;
            this.colVHeader.Name = "colVHeader";
            this.colVHeader.ReadOnly = true;
            this.colVHeader.Width = 230;
            // 
            // colVChain
            // 
            this.colVChain.DataPropertyName = "CHAIN";
            this.colVChain.HeaderText = "Chain";
            this.colVChain.MinimumWidth = 8;
            this.colVChain.Name = "colVChain";
            this.colVChain.ReadOnly = true;
            this.colVChain.Width = 150;
            // 
            // colVGds
            // 
            this.colVGds.DataPropertyName = "GDS";
            this.colVGds.HeaderText = "GDS Chain";
            this.colVGds.MinimumWidth = 8;
            this.colVGds.Name = "colVGds";
            this.colVGds.ReadOnly = true;
            this.colVGds.Width = 110;
            // 
            // colVUnit
            // 
            this.colVUnit.DataPropertyName = "UNIT";
            this.colVUnit.HeaderText = "단위";
            this.colVUnit.MinimumWidth = 8;
            this.colVUnit.Name = "colVUnit";
            this.colVUnit.ReadOnly = true;
            this.colVUnit.Width = 45;
            // 
            // colVValue
            // 
            this.colVValue.DataPropertyName = "VALUE";
            this.colVValue.HeaderText = "값";
            this.colVValue.MinimumWidth = 8;
            this.colVValue.Name = "colVValue";
            this.colVValue.ReadOnly = true;
            this.colVValue.Width = 80;
            // 
            // colVLower
            // 
            this.colVLower.DataPropertyName = "LOWER";
            this.colVLower.HeaderText = "Lower";
            this.colVLower.MinimumWidth = 8;
            this.colVLower.Name = "colVLower";
            this.colVLower.ReadOnly = true;
            this.colVLower.Width = 60;
            // 
            // colVUpper
            // 
            this.colVUpper.DataPropertyName = "UPPER";
            this.colVUpper.HeaderText = "Upper";
            this.colVUpper.MinimumWidth = 8;
            this.colVUpper.Name = "colVUpper";
            this.colVUpper.ReadOnly = true;
            this.colVUpper.Width = 70;
            // 
            // colVJudge
            // 
            this.colVJudge.DataPropertyName = "JUDGE";
            this.colVJudge.HeaderText = "판정";
            this.colVJudge.MinimumWidth = 8;
            this.colVJudge.Name = "colVJudge";
            this.colVJudge.ReadOnly = true;
            this.colVJudge.Width = 50;
            // 
            // colVReason
            // 
            this.colVReason.DataPropertyName = "REASON";
            this.colVReason.HeaderText = "사유";
            this.colVReason.MinimumWidth = 8;
            this.colVReason.Name = "colVReason";
            this.colVReason.ReadOnly = true;
            // 
            // valueBar
            // 
            this.valueBar.Controls.Add(this.lblValueTitle);
            this.valueBar.Controls.Add(this.chkFailOnly);
            this.valueBar.Controls.Add(this.chkNotTested);
            this.valueBar.Controls.Add(this.lblFind);
            this.valueBar.Controls.Add(this.txtFind);
            this.valueBar.Controls.Add(this.lblCount);
            this.valueBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.valueBar.Location = new System.Drawing.Point(0, 0);
            this.valueBar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.valueBar.Name = "valueBar";
            this.valueBar.Size = new System.Drawing.Size(1380, 42);
            this.valueBar.TabIndex = 0;
            // 
            // lblValueTitle
            // 
            this.lblValueTitle.AutoSize = true;
            this.lblValueTitle.Location = new System.Drawing.Point(6, 12);
            this.lblValueTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblValueTitle.Name = "lblValueTitle";
            this.lblValueTitle.Size = new System.Drawing.Size(169, 18);
            this.lblValueTitle.TabIndex = 0;
            this.lblValueTitle.Text = "2단계: 정의 Para 값";
            // 
            // chkFailOnly
            // 
            this.chkFailOnly.AutoSize = true;
            this.chkFailOnly.Location = new System.Drawing.Point(429, 9);
            this.chkFailOnly.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkFailOnly.Name = "chkFailOnly";
            this.chkFailOnly.Size = new System.Drawing.Size(83, 22);
            this.chkFailOnly.TabIndex = 1;
            this.chkFailOnly.Text = "Fail 만";
            this.chkFailOnly.UseVisualStyleBackColor = true;
            this.chkFailOnly.Visible = false;
            this.chkFailOnly.CheckedChanged += new System.EventHandler(this.Filter_Changed);
            // 
            // chkNotTested
            // 
            this.chkNotTested.AutoSize = true;
            this.chkNotTested.Location = new System.Drawing.Point(537, 9);
            this.chkNotTested.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkNotTested.Name = "chkNotTested";
            this.chkNotTested.Size = new System.Drawing.Size(79, 22);
            this.chkNotTested.TabIndex = 2;
            this.chkNotTested.Text = "\'-\' 만";
            this.chkNotTested.UseVisualStyleBackColor = true;
            this.chkNotTested.Visible = false;
            this.chkNotTested.CheckedChanged += new System.EventHandler(this.Filter_Changed);
            // 
            // lblFind
            // 
            this.lblFind.AutoSize = true;
            this.lblFind.Location = new System.Drawing.Point(643, 12);
            this.lblFind.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFind.Name = "lblFind";
            this.lblFind.Size = new System.Drawing.Size(44, 18);
            this.lblFind.TabIndex = 3;
            this.lblFind.Text = "찾기";
            this.lblFind.Visible = false;
            // 
            // txtFind
            // 
            this.txtFind.Location = new System.Drawing.Point(691, 6);
            this.txtFind.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(284, 28);
            this.txtFind.TabIndex = 4;
            this.txtFind.Visible = false;
            this.txtFind.TextChanged += new System.EventHandler(this.Filter_Changed);
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.ForeColor = System.Drawing.Color.DimGray;
            this.lblCount.Location = new System.Drawing.Point(994, 12);
            this.lblCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(0, 18);
            this.lblCount.TabIndex = 5;
            this.lblCount.Visible = false;
            // 
            // EtResultDataControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitMain);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "EtResultDataControl";
            this.Size = new System.Drawing.Size(1857, 630);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInfo)).EndInit();
            this.splitData.Panel1.ResumeLayout(false);
            this.splitData.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitData)).EndInit();
            this.splitData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDie)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvValue)).EndInit();
            this.valueBar.ResumeLayout(false);
            this.valueBar.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.DataGridView dgvInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInfoName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInfoValue;
        private System.Windows.Forms.Label lblInfoTitle;
        private System.Windows.Forms.SplitContainer splitData;
        private System.Windows.Forms.DataGridView dgvDie;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDDut;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDX;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDMap;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDJudge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDFail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDNotTested;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDFlag;
        private System.Windows.Forms.Label lblDieTitle;
        private System.Windows.Forms.DataGridView dgvValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVDut;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVXY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVHeader;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVChain;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVGds;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVLower;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVUpper;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVJudge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVReason;
        private System.Windows.Forms.Panel valueBar;
        private System.Windows.Forms.Label lblValueTitle;
        private System.Windows.Forms.CheckBox chkFailOnly;
        private System.Windows.Forms.CheckBox chkNotTested;
        private System.Windows.Forms.Label lblFind;
        private System.Windows.Forms.TextBox txtFind;
        private System.Windows.Forms.Label lblCount;
    }
}
