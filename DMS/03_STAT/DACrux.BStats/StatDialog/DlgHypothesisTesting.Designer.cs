namespace DACrux.BStats.StatDialog
{
    partial class DlgHypothesisTesting
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
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgHypothesisTesting));
            this.imlColumnType = new System.Windows.Forms.ImageList(this.components);
            this.pnlBackground = new System.Windows.Forms.Panel();
            this.rdoPairedT = new System.Windows.Forms.RadioButton();
            this.rdo2SampleT = new System.Windows.Forms.RadioButton();
            this.rdo1SampleT = new System.Windows.Forms.RadioButton();
            this.rdo1SampleZ = new System.Windows.Forms.RadioButton();
            this.btnMethods = new System.Windows.Forms.Button();
            this.btnOptions = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnLeft_Var = new System.Windows.Forms.Button();
            this.btnRight_Var = new System.Windows.Forms.Button();
            this.lblColumns = new System.Windows.Forms.Label();
            this.lvColumns = new System.Windows.Forms.ListView();
            this.colType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblVariables = new System.Windows.Forms.Label();
            this.lvVariables = new System.Windows.Forms.ListView();
            this.colType_Ser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colID_Ser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName_Ser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlBackground.SuspendLayout();
            this.SuspendLayout();
            // 
            // imlColumnType
            // 
            this.imlColumnType.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlColumnType.ImageStream")));
            this.imlColumnType.TransparentColor = System.Drawing.Color.Transparent;
            this.imlColumnType.Images.SetKeyName(0, "NUMBER");
            this.imlColumnType.Images.SetKeyName(1, "TEXT");
            this.imlColumnType.Images.SetKeyName(2, "DATETIME");
            // 
            // pnlBackground
            // 
            this.pnlBackground.Controls.Add(this.rdoPairedT);
            this.pnlBackground.Controls.Add(this.rdo2SampleT);
            this.pnlBackground.Controls.Add(this.rdo1SampleT);
            this.pnlBackground.Controls.Add(this.rdo1SampleZ);
            this.pnlBackground.Controls.Add(this.btnMethods);
            this.pnlBackground.Controls.Add(this.btnOptions);
            this.pnlBackground.Controls.Add(this.btnCancel);
            this.pnlBackground.Controls.Add(this.btnOk);
            this.pnlBackground.Controls.Add(this.btnLeft_Var);
            this.pnlBackground.Controls.Add(this.btnRight_Var);
            this.pnlBackground.Controls.Add(this.lblColumns);
            this.pnlBackground.Controls.Add(this.lvColumns);
            this.pnlBackground.Controls.Add(this.lblVariables);
            this.pnlBackground.Controls.Add(this.lvVariables);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Size = new System.Drawing.Size(358, 315);
            this.pnlBackground.TabIndex = 2;
            // 
            // rdoPairedT
            // 
            this.rdoPairedT.AutoSize = true;
            this.rdoPairedT.Location = new System.Drawing.Point(200, 217);
            this.rdoPairedT.Name = "rdoPairedT";
            this.rdoPairedT.Size = new System.Drawing.Size(74, 17);
            this.rdoPairedT.TabIndex = 72;
            this.rdoPairedT.Text = "&Paired t...";
            this.rdoPairedT.UseVisualStyleBackColor = true;
            // 
            // rdo2SampleT
            // 
            this.rdo2SampleT.AutoSize = true;
            this.rdo2SampleT.Location = new System.Drawing.Point(199, 194);
            this.rdo2SampleT.Name = "rdo2SampleT";
            this.rdo2SampleT.Size = new System.Drawing.Size(88, 17);
            this.rdo2SampleT.TabIndex = 71;
            this.rdo2SampleT.Text = "&2-Sample t...";
            this.rdo2SampleT.UseVisualStyleBackColor = true;
            // 
            // rdo1SampleT
            // 
            this.rdo1SampleT.AutoSize = true;
            this.rdo1SampleT.Location = new System.Drawing.Point(199, 171);
            this.rdo1SampleT.Name = "rdo1SampleT";
            this.rdo1SampleT.Size = new System.Drawing.Size(88, 17);
            this.rdo1SampleT.TabIndex = 70;
            this.rdo1SampleT.Text = "&1-Sample t...";
            this.rdo1SampleT.UseVisualStyleBackColor = true;
            // 
            // rdo1SampleZ
            // 
            this.rdo1SampleZ.AutoSize = true;
            this.rdo1SampleZ.Checked = true;
            this.rdo1SampleZ.Location = new System.Drawing.Point(199, 148);
            this.rdo1SampleZ.Name = "rdo1SampleZ";
            this.rdo1SampleZ.Size = new System.Drawing.Size(90, 17);
            this.rdo1SampleZ.TabIndex = 69;
            this.rdo1SampleZ.TabStop = true;
            this.rdo1SampleZ.Text = "1-Sample &Z...";
            this.rdo1SampleZ.UseVisualStyleBackColor = true;
            // 
            // btnMethods
            // 
            this.btnMethods.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMethods.Location = new System.Drawing.Point(220, 247);
            this.btnMethods.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnMethods.Name = "btnMethods";
            this.btnMethods.Size = new System.Drawing.Size(59, 22);
            this.btnMethods.TabIndex = 41;
            this.btnMethods.Text = "&Methods";
            this.btnMethods.UseVisualStyleBackColor = true;
            // 
            // btnOptions
            // 
            this.btnOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOptions.Location = new System.Drawing.Point(285, 247);
            this.btnOptions.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Size = new System.Drawing.Size(59, 22);
            this.btnOptions.TabIndex = 40;
            this.btnOptions.Text = "O&ptions";
            this.btnOptions.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(285, 280);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(59, 22);
            this.btnCancel.TabIndex = 38;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(220, 280);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(59, 22);
            this.btnOk.TabIndex = 37;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnLeft_Var
            // 
            this.btnLeft_Var.Location = new System.Drawing.Point(172, 61);
            this.btnLeft_Var.Name = "btnLeft_Var";
            this.btnLeft_Var.Size = new System.Drawing.Size(20, 19);
            this.btnLeft_Var.TabIndex = 5;
            this.btnLeft_Var.Text = "◄";
            this.btnLeft_Var.UseCompatibleTextRendering = true;
            this.btnLeft_Var.UseVisualStyleBackColor = true;
            // 
            // btnRight_Var
            // 
            this.btnRight_Var.Location = new System.Drawing.Point(172, 37);
            this.btnRight_Var.Name = "btnRight_Var";
            this.btnRight_Var.Size = new System.Drawing.Size(20, 19);
            this.btnRight_Var.TabIndex = 4;
            this.btnRight_Var.Text = "►";
            this.btnRight_Var.UseCompatibleTextRendering = true;
            this.btnRight_Var.UseVisualStyleBackColor = true;
            // 
            // lblColumns
            // 
            this.lblColumns.AutoSize = true;
            this.lblColumns.Location = new System.Drawing.Point(20, 20);
            this.lblColumns.Name = "lblColumns";
            this.lblColumns.Size = new System.Drawing.Size(47, 13);
            this.lblColumns.TabIndex = 2;
            this.lblColumns.Text = "Columns";
            // 
            // lvColumns
            // 
            this.lvColumns.AllowDrop = true;
            this.lvColumns.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvColumns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colType,
            this.colID,
            this.colName});
            this.lvColumns.FullRowSelect = true;
            this.lvColumns.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvColumns.Location = new System.Drawing.Point(20, 36);
            this.lvColumns.Name = "lvColumns";
            this.lvColumns.Size = new System.Drawing.Size(145, 265);
            this.lvColumns.TabIndex = 3;
            this.lvColumns.UseCompatibleStateImageBehavior = false;
            this.lvColumns.View = System.Windows.Forms.View.Details;
            // 
            // colType
            // 
            this.colType.Text = "Type";
            this.colType.Width = 20;
            // 
            // colID
            // 
            this.colID.Text = "ID";
            this.colID.Width = 50;
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 70;
            // 
            // lblVariables
            // 
            this.lblVariables.AutoSize = true;
            this.lblVariables.Location = new System.Drawing.Point(197, 20);
            this.lblVariables.Name = "lblVariables";
            this.lblVariables.Size = new System.Drawing.Size(50, 13);
            this.lblVariables.TabIndex = 6;
            this.lblVariables.Text = "Variables";
            // 
            // lvVariables
            // 
            this.lvVariables.AllowDrop = true;
            this.lvVariables.BackColor = System.Drawing.Color.Lavender;
            this.lvVariables.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvVariables.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colType_Ser,
            this.colID_Ser,
            this.colName_Ser});
            this.lvVariables.FullRowSelect = true;
            this.lvVariables.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvVariables.Location = new System.Drawing.Point(199, 37);
            this.lvVariables.Name = "lvVariables";
            this.lvVariables.Size = new System.Drawing.Size(145, 102);
            this.lvVariables.TabIndex = 7;
            this.lvVariables.UseCompatibleStateImageBehavior = false;
            this.lvVariables.View = System.Windows.Forms.View.Details;
            // 
            // colType_Ser
            // 
            this.colType_Ser.Width = 20;
            // 
            // colID_Ser
            // 
            this.colID_Ser.Width = 50;
            // 
            // colName_Ser
            // 
            this.colName_Ser.Width = 70;
            // 
            // DlgHypothesisTesting
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(358, 315);
            this.Controls.Add(this.pnlBackground);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DlgHypothesisTesting";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Hypothesis Testing";
            this.pnlBackground.ResumeLayout(false);
            this.pnlBackground.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imlColumnType;
        private System.Windows.Forms.Panel pnlBackground;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnLeft_Var;
        private System.Windows.Forms.Button btnRight_Var;
        private System.Windows.Forms.Label lblColumns;
        private System.Windows.Forms.ListView lvColumns;
        private System.Windows.Forms.ColumnHeader colType;
        private System.Windows.Forms.ColumnHeader colID;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.Label lblVariables;
        private System.Windows.Forms.ListView lvVariables;
        private System.Windows.Forms.ColumnHeader colType_Ser;
        private System.Windows.Forms.ColumnHeader colID_Ser;
        private System.Windows.Forms.ColumnHeader colName_Ser;
        private System.Windows.Forms.Button btnMethods;
        private System.Windows.Forms.Button btnOptions;
        private System.Windows.Forms.RadioButton rdoPairedT;
        private System.Windows.Forms.RadioButton rdo2SampleT;
        private System.Windows.Forms.RadioButton rdo1SampleT;
        private System.Windows.Forms.RadioButton rdo1SampleZ;


    }
}