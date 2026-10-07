namespace DACrux.SEMDMS.ENGUI
{
    // E-Test Para Limit 설정 화면의 컨트롤과 배치를 정의한다. 조회 / 저장 / 삭제 동작은 본문 파일(frmEtLimitSetup.cs)에서 처리한다. (CLAUDE-029)
    partial class frmEtLimitSetup
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>화면 종료 시 디자이너 구성 요소를 해제한다.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// 위: Factory / Device / [조회] [저장] [선택 Limit 삭제] + 안내 문구.
        /// 가운데: Para 별 Limit 표 (입력 칸 = Lower / Upper / Enable / 비고, 나머지는 업로드 값 통계로 읽기 전용). 아래: 상태 표시줄.
        /// </summary>
        private void InitializeComponent()
        {
            this.topBar = new System.Windows.Forms.Panel();
            this.lblFactory = new System.Windows.Forms.Label();
            this.txtFactory = new System.Windows.Forms.TextBox();
            this.lblDevice = new System.Windows.Forms.Label();
            this.cboDevice = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lblGuide = new System.Windows.Forms.Label();
            this.dgvLimit = new System.Windows.Forms.DataGridView();
            this.colPara = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItems = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValues = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNotTested = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMedian = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colP99 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLower = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUpper = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEnable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colOut = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRemark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUpdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblStatus = new System.Windows.Forms.Label();
            this.topBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLimit)).BeginInit();
            this.SuspendLayout();
            //
            // topBar
            //
            this.topBar.Controls.Add(this.lblFactory);
            this.topBar.Controls.Add(this.txtFactory);
            this.topBar.Controls.Add(this.lblDevice);
            this.topBar.Controls.Add(this.cboDevice);
            this.topBar.Controls.Add(this.btnSearch);
            this.topBar.Controls.Add(this.btnSave);
            this.topBar.Controls.Add(this.btnDelete);
            this.topBar.Controls.Add(this.lblGuide);
            this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar.Location = new System.Drawing.Point(0, 0);
            this.topBar.Name = "topBar";
            this.topBar.Size = new System.Drawing.Size(1200, 62);
            this.topBar.TabIndex = 0;
            //
            // lblFactory
            //
            this.lblFactory.AutoSize = true;
            this.lblFactory.Location = new System.Drawing.Point(10, 13);
            this.lblFactory.Name = "lblFactory";
            this.lblFactory.Size = new System.Drawing.Size(46, 12);
            this.lblFactory.TabIndex = 0;
            this.lblFactory.Text = "Factory";
            //
            // txtFactory
            //
            this.txtFactory.Location = new System.Drawing.Point(62, 9);
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.Size = new System.Drawing.Size(100, 21);
            this.txtFactory.TabIndex = 1;
            this.txtFactory.Leave += new System.EventHandler(this.txtFactory_Leave);
            //
            // lblDevice
            //
            this.lblDevice.AutoSize = true;
            this.lblDevice.Location = new System.Drawing.Point(176, 13);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(42, 12);
            this.lblDevice.TabIndex = 2;
            this.lblDevice.Text = "Device";
            //
            // cboDevice
            //
            this.cboDevice.FormattingEnabled = true;
            this.cboDevice.Location = new System.Drawing.Point(224, 9);
            this.cboDevice.Name = "cboDevice";
            this.cboDevice.Size = new System.Drawing.Size(230, 20);
            this.cboDevice.TabIndex = 3;
            this.cboDevice.SelectedIndexChanged += new System.EventHandler(this.cboDevice_SelectedIndexChanged);
            //
            // btnSearch
            //
            this.btnSearch.Location = new System.Drawing.Point(462, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 23);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            //
            // btnSave
            //
            this.btnSave.Location = new System.Drawing.Point(548, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // btnDelete
            //
            this.btnDelete.Location = new System.Drawing.Point(634, 8);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(120, 23);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "선택 Limit 삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            //
            // lblGuide
            //
            this.lblGuide.AutoSize = true;
            this.lblGuide.ForeColor = System.Drawing.Color.DimGray;
            this.lblGuide.Location = new System.Drawing.Point(10, 40);
            this.lblGuide.Name = "lblGuide";
            this.lblGuide.Size = new System.Drawing.Size(700, 12);
            this.lblGuide.TabIndex = 7;
            this.lblGuide.Text = "Para 별로 Lower / Upper 를 입력합니다 (한쪽만 입력 가능). 값 < Lower 또는 값 > Upper 이면 Fail, 경계값은 Pass. Enable 체크한 Limit 만 판정에 사용합니다. Para 는 대문자로 저장합니다.";
            //
            // dgvLimit
            //
            this.dgvLimit.AllowUserToAddRows = false;
            this.dgvLimit.AllowUserToDeleteRows = false;
            this.dgvLimit.AllowUserToResizeRows = false;
            this.dgvLimit.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPara,
            this.colUnit,
            this.colItems,
            this.colValues,
            this.colNotTested,
            this.colMin,
            this.colMedian,
            this.colP99,
            this.colMax,
            this.colLower,
            this.colUpper,
            this.colEnable,
            this.colOut,
            this.colRemark,
            this.colUpdate});
            this.dgvLimit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLimit.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvLimit.Location = new System.Drawing.Point(0, 62);
            this.dgvLimit.MultiSelect = false;
            this.dgvLimit.Name = "dgvLimit";
            this.dgvLimit.RowHeadersVisible = false;
            this.dgvLimit.RowTemplate.Height = 23;
            this.dgvLimit.Size = new System.Drawing.Size(1200, 612);
            this.dgvLimit.TabIndex = 1;
            this.dgvLimit.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLimit_CellValueChanged);
            this.dgvLimit.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvLimit_CurrentCellDirtyStateChanged);
            //
            // colPara
            //
            this.colPara.HeaderText = "Para";
            this.colPara.Name = "colPara";
            this.colPara.ReadOnly = true;
            this.colPara.Width = 180;
            //
            // colUnit
            //
            this.colUnit.HeaderText = "단위";
            this.colUnit.Name = "colUnit";
            this.colUnit.ReadOnly = true;
            this.colUnit.Width = 50;
            //
            // colItems
            //
            this.colItems.HeaderText = "항목 수";
            this.colItems.Name = "colItems";
            this.colItems.ReadOnly = true;
            this.colItems.Width = 60;
            //
            // colValues
            //
            this.colValues.HeaderText = "값 수";
            this.colValues.Name = "colValues";
            this.colValues.ReadOnly = true;
            this.colValues.Width = 70;
            //
            // colNotTested
            //
            this.colNotTested.HeaderText = "미측정";
            this.colNotTested.Name = "colNotTested";
            this.colNotTested.ReadOnly = true;
            this.colNotTested.Width = 55;
            //
            // colMin
            //
            this.colMin.HeaderText = "최소";
            this.colMin.Name = "colMin";
            this.colMin.ReadOnly = true;
            this.colMin.Width = 70;
            //
            // colMedian
            //
            this.colMedian.HeaderText = "중앙값";
            this.colMedian.Name = "colMedian";
            this.colMedian.ReadOnly = true;
            this.colMedian.Width = 70;
            //
            // colP99
            //
            this.colP99.HeaderText = "99%";
            this.colP99.Name = "colP99";
            this.colP99.ReadOnly = true;
            this.colP99.Width = 70;
            //
            // colMax
            //
            this.colMax.HeaderText = "최대";
            this.colMax.Name = "colMax";
            this.colMax.ReadOnly = true;
            this.colMax.Width = 70;
            //
            // colLower
            //
            this.colLower.HeaderText = "Lower";
            this.colLower.Name = "colLower";
            this.colLower.Width = 80;
            //
            // colUpper
            //
            this.colUpper.HeaderText = "Upper";
            this.colUpper.Name = "colUpper";
            this.colUpper.Width = 80;
            //
            // colEnable
            //
            this.colEnable.HeaderText = "Enable";
            this.colEnable.Name = "colEnable";
            this.colEnable.Width = 55;
            //
            // colOut
            //
            this.colOut.HeaderText = "범위 밖";
            this.colOut.Name = "colOut";
            this.colOut.ReadOnly = true;
            this.colOut.ToolTipText = "저장된 Enable Limit 기준 범위 밖 값 수 (READY 파일 전체)";
            this.colOut.Width = 60;
            //
            // colRemark
            //
            this.colRemark.HeaderText = "비고";
            this.colRemark.MaxInputLength = 200;
            this.colRemark.Name = "colRemark";
            this.colRemark.Width = 160;
            //
            // colUpdate
            //
            this.colUpdate.HeaderText = "수정";
            this.colUpdate.Name = "colUpdate";
            this.colUpdate.ReadOnly = true;
            this.colUpdate.Width = 170;
            //
            // lblStatus
            //
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Location = new System.Drawing.Point(0, 674);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lblStatus.Size = new System.Drawing.Size(1200, 26);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Device 를 고르고 [조회] 를 누르세요.";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // frmEtLimitSetup
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.dgvLimit);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.topBar);
            this.Name = "frmEtLimitSetup";
            this.Text = "E-Test Limit 설정";
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLimit)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Label lblFactory;
        private System.Windows.Forms.TextBox txtFactory;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.ComboBox cboDevice;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label lblGuide;
        private System.Windows.Forms.DataGridView dgvLimit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPara;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValues;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNotTested;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMin;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMedian;
        private System.Windows.Forms.DataGridViewTextBoxColumn colP99;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMax;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLower;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUpper;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colEnable;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOut;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRemark;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUpdate;
        private System.Windows.Forms.Label lblStatus;
    }
}
