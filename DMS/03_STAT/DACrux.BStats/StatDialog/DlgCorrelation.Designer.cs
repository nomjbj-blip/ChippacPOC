namespace DACrux.BStats.StatDialog
{
    partial class DlgCorrelation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgCorrelation));
            this.pnlBackground = new System.Windows.Forms.Panel();
            this.chkSpearman = new System.Windows.Forms.CheckBox();
            this.chkRegression = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkScatter = new System.Windows.Forms.CheckBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.chkPvalue = new System.Windows.Forms.CheckBox();
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
            this.imlColumnType = new System.Windows.Forms.ImageList(this.components);
            this.pnlBackground.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBackground
            // 
            this.pnlBackground.Controls.Add(this.chkSpearman);
            this.pnlBackground.Controls.Add(this.chkRegression);
            this.pnlBackground.Controls.Add(this.btnCancel);
            this.pnlBackground.Controls.Add(this.chkScatter);
            this.pnlBackground.Controls.Add(this.btnOk);
            this.pnlBackground.Controls.Add(this.chkPvalue);
            this.pnlBackground.Controls.Add(this.btnLeft_Var);
            this.pnlBackground.Controls.Add(this.btnRight_Var);
            this.pnlBackground.Controls.Add(this.lblColumns);
            this.pnlBackground.Controls.Add(this.lvColumns);
            this.pnlBackground.Controls.Add(this.lblVariables);
            this.pnlBackground.Controls.Add(this.lvVariables);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Size = new System.Drawing.Size(365, 267);
            this.pnlBackground.TabIndex = 1;
            // 
            // chkSpearman
            // 
            this.chkSpearman.AutoSize = true;
            this.chkSpearman.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkSpearman.Location = new System.Drawing.Point(200, 174);
            this.chkSpearman.Name = "chkSpearman";
            this.chkSpearman.Size = new System.Drawing.Size(100, 17);
            this.chkSpearman.TabIndex = 39;
            this.chkSpearman.Text = "Spearman\'s &Rho";
            this.chkSpearman.UseVisualStyleBackColor = true;
            // 
            // chkRegression
            // 
            this.chkRegression.AutoSize = true;
            this.chkRegression.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkRegression.Location = new System.Drawing.Point(200, 208);
            this.chkRegression.Name = "chkRegression";
            this.chkRegression.Size = new System.Drawing.Size(121, 17);
            this.chkRegression.TabIndex = 41;
            this.chkRegression.Text = "&Regression equation";
            this.chkRegression.UseVisualStyleBackColor = true;
            this.chkRegression.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(275, 232);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 21);
            this.btnCancel.TabIndex = 38;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // chkScatter
            // 
            this.chkScatter.AutoSize = true;
            this.chkScatter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkScatter.Location = new System.Drawing.Point(200, 191);
            this.chkScatter.Name = "chkScatter";
            this.chkScatter.Size = new System.Drawing.Size(79, 17);
            this.chkScatter.TabIndex = 40;
            this.chkScatter.Text = "&Scatter plot";
            this.chkScatter.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(198, 232);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(69, 21);
            this.btnOk.TabIndex = 37;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // chkPvalue
            // 
            this.chkPvalue.AutoSize = true;
            this.chkPvalue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkPvalue.Location = new System.Drawing.Point(200, 155);
            this.chkPvalue.Name = "chkPvalue";
            this.chkPvalue.Size = new System.Drawing.Size(101, 17);
            this.chkPvalue.TabIndex = 38;
            this.chkPvalue.Text = "&Display p-values";
            this.chkPvalue.UseVisualStyleBackColor = true;
            // 
            // btnLeft_Var
            // 
            this.btnLeft_Var.Location = new System.Drawing.Point(173, 61);
            this.btnLeft_Var.Name = "btnLeft_Var";
            this.btnLeft_Var.Size = new System.Drawing.Size(20, 19);
            this.btnLeft_Var.TabIndex = 5;
            this.btnLeft_Var.Text = "◄";
            this.btnLeft_Var.UseCompatibleTextRendering = true;
            this.btnLeft_Var.UseVisualStyleBackColor = true;
            // 
            // btnRight_Var
            // 
            this.btnRight_Var.Location = new System.Drawing.Point(173, 37);
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
            this.lvColumns.Size = new System.Drawing.Size(145, 217);
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
            this.lvVariables.Location = new System.Drawing.Point(199, 36);
            this.lvVariables.Name = "lvVariables";
            this.lvVariables.Size = new System.Drawing.Size(145, 113);
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
            // imlColumnType
            // 
            this.imlColumnType.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlColumnType.ImageStream")));
            this.imlColumnType.TransparentColor = System.Drawing.Color.Transparent;
            this.imlColumnType.Images.SetKeyName(0, "NUMBER");
            this.imlColumnType.Images.SetKeyName(1, "TEXT");
            this.imlColumnType.Images.SetKeyName(2, "DATETIME");
            // 
            // DlgCorrelation
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(365, 267);
            this.Controls.Add(this.pnlBackground);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "DlgCorrelation";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Correlation";
            this.pnlBackground.ResumeLayout(false);
            this.pnlBackground.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

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
        private System.Windows.Forms.CheckBox chkRegression;
        private System.Windows.Forms.CheckBox chkScatter;
        private System.Windows.Forms.CheckBox chkPvalue;
        private System.Windows.Forms.ImageList imlColumnType;
        private System.Windows.Forms.CheckBox chkSpearman;

    }
}