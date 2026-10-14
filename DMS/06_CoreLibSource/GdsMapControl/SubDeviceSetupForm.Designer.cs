namespace NexplantQMS.GdsMap
{
    partial class SubDeviceSetupForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblRepresentative = new System.Windows.Forms.Label();
            this.lblHelp = new System.Windows.Forms.Label();
            this.gridSub = new System.Windows.Forms.DataGridView();
            this.colSubDevice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRemark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnDeleteRow = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridSub)).BeginInit();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            //
            // lblRepresentative (대표 Device = GDS Map 이 있는 Device)
            //
            this.lblRepresentative.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRepresentative.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lblRepresentative.Location = new System.Drawing.Point(12, 12);
            this.lblRepresentative.Name = "lblRepresentative";
            this.lblRepresentative.Size = new System.Drawing.Size(676, 34);
            this.lblRepresentative.TabIndex = 0;
            this.lblRepresentative.Text = "대표 Device: -";
            this.lblRepresentative.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblHelp
            //
            this.lblHelp.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHelp.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblHelp.Location = new System.Drawing.Point(12, 46);
            this.lblHelp.Name = "lblHelp";
            this.lblHelp.Size = new System.Drawing.Size(676, 48);
            this.lblHelp.TabIndex = 1;
            this.lblHelp.Text = "다른 데이터(KLARF Product / E-Test Device 등)의 Device 이름을 등록하면 이 대표 GDS Map으로 연결됩니다.\r\n맨 아래 빈 행에 입력해 추가합니다. 대소문자는 구분하지 않습니다.";
            //
            // gridSub
            //
            this.gridSub.AllowUserToResizeRows = false;
            this.gridSub.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridSub.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridSub.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSub.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSubDevice,
            this.colRemark});
            this.gridSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridSub.Location = new System.Drawing.Point(12, 94);
            this.gridSub.Name = "gridSub";
            this.gridSub.RowHeadersWidth = 30;
            this.gridSub.RowTemplate.Height = 28;
            this.gridSub.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSub.Size = new System.Drawing.Size(676, 334);
            this.gridSub.TabIndex = 2;
            this.gridSub.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridSub_CellValueChanged);
            this.gridSub.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.gridSub_UserDeletingRow);
            this.gridSub.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.gridSub_UserDeletedRow);
            //
            // colSubDevice
            //
            this.colSubDevice.FillWeight = 45F;
            this.colSubDevice.HeaderText = "Sub Device";
            this.colSubDevice.MaxInputLength = 50;
            this.colSubDevice.Name = "colSubDevice";
            //
            // colRemark
            //
            this.colRemark.FillWeight = 55F;
            this.colRemark.HeaderText = "비고";
            this.colRemark.MaxInputLength = 200;
            this.colRemark.Name = "colRemark";
            //
            // pnlButtons
            //
            this.pnlButtons.Controls.Add(this.lblStatus);
            this.pnlButtons.Controls.Add(this.btnDeleteRow);
            this.pnlButtons.Controls.Add(this.btnSave);
            this.pnlButtons.Controls.Add(this.btnClose);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(12, 428);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(676, 90);
            this.pnlButtons.TabIndex = 3;
            //
            // lblStatus
            //
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblStatus.Location = new System.Drawing.Point(0, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(676, 36);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // btnDeleteRow
            //
            this.btnDeleteRow.Location = new System.Drawing.Point(0, 44);
            this.btnDeleteRow.Name = "btnDeleteRow";
            this.btnDeleteRow.Size = new System.Drawing.Size(130, 40);
            this.btnDeleteRow.TabIndex = 1;
            this.btnDeleteRow.Text = "행 삭제";
            this.btnDeleteRow.UseVisualStyleBackColor = true;
            this.btnDeleteRow.Click += new System.EventHandler(this.btnDeleteRow_Click);
            //
            // btnSave
            //
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(410, 44);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(130, 40);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // btnClose
            //
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(546, 44);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(130, 40);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = true;
            //
            // SubDeviceSetupForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(700, 530);
            this.Controls.Add(this.gridSub);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.lblHelp);
            this.Controls.Add(this.lblRepresentative);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(560, 420);
            this.Name = "SubDeviceSetupForm";
            this.Padding = new System.Windows.Forms.Padding(12);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sub Device 설정";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SubDeviceSetupForm_FormClosing);
            this.Shown += new System.EventHandler(this.SubDeviceSetupForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.gridSub)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblRepresentative;
        private System.Windows.Forms.Label lblHelp;
        private System.Windows.Forms.DataGridView gridSub;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubDevice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRemark;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnDeleteRow;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblStatus;
    }
}
