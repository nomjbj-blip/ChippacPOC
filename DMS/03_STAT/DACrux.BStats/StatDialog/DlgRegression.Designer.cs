namespace DACrux.BStats.StatDialog
{
    partial class DlgRegression
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgRegression));
            this.pnlBackground = new System.Windows.Forms.Panel();
            this.chkIntercept = new System.Windows.Forms.CheckBox();
            this.lblPredictors = new System.Windows.Forms.Label();
            this.btnLeft_Predictors = new System.Windows.Forms.Button();
            this.btnRight_Predictors = new System.Windows.Forms.Button();
            this.lvPredictors = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnMethods = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnOptions = new System.Windows.Forms.Button();
            this.btnLeft_Response = new System.Windows.Forms.Button();
            this.btnRight_Response = new System.Windows.Forms.Button();
            this.lblColumns = new System.Windows.Forms.Label();
            this.lvColumns = new System.Windows.Forms.ListView();
            this.colType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblResponse = new System.Windows.Forms.Label();
            this.lvRespense = new System.Windows.Forms.ListView();
            this.colType_Ser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colID_Ser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName_Ser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imlColumnType = new System.Windows.Forms.ImageList(this.components);
            this.pnlBackground.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBackground
            // 
            this.pnlBackground.Controls.Add(this.chkIntercept);
            this.pnlBackground.Controls.Add(this.lblPredictors);
            this.pnlBackground.Controls.Add(this.btnLeft_Predictors);
            this.pnlBackground.Controls.Add(this.btnRight_Predictors);
            this.pnlBackground.Controls.Add(this.lvPredictors);
            this.pnlBackground.Controls.Add(this.btnMethods);
            this.pnlBackground.Controls.Add(this.btnCancel);
            this.pnlBackground.Controls.Add(this.btnOk);
            this.pnlBackground.Controls.Add(this.btnOptions);
            this.pnlBackground.Controls.Add(this.btnLeft_Response);
            this.pnlBackground.Controls.Add(this.btnRight_Response);
            this.pnlBackground.Controls.Add(this.lblColumns);
            this.pnlBackground.Controls.Add(this.lvColumns);
            this.pnlBackground.Controls.Add(this.lblResponse);
            this.pnlBackground.Controls.Add(this.lvRespense);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Size = new System.Drawing.Size(431, 339);
            this.pnlBackground.TabIndex = 2;
            // 
            // chkIntercept
            // 
            this.chkIntercept.AutoSize = true;
            this.chkIntercept.Location = new System.Drawing.Point(341, 79);
            this.chkIntercept.Name = "chkIntercept";
            this.chkIntercept.Size = new System.Drawing.Size(84, 17);
            this.chkIntercept.TabIndex = 67;
            this.chkIntercept.Text = "Fi&t intercept";
            this.chkIntercept.UseVisualStyleBackColor = true;
            // 
            // lblPredictors
            // 
            this.lblPredictors.AutoSize = true;
            this.lblPredictors.Location = new System.Drawing.Point(192, 84);
            this.lblPredictors.Name = "lblPredictors";
            this.lblPredictors.Size = new System.Drawing.Size(55, 13);
            this.lblPredictors.TabIndex = 62;
            this.lblPredictors.Text = "Predictors";
            // 
            // btnLeft_Predictors
            // 
            this.btnLeft_Predictors.Location = new System.Drawing.Point(169, 120);
            this.btnLeft_Predictors.Name = "btnLeft_Predictors";
            this.btnLeft_Predictors.Size = new System.Drawing.Size(17, 20);
            this.btnLeft_Predictors.TabIndex = 61;
            this.btnLeft_Predictors.Text = "◄";
            this.btnLeft_Predictors.UseCompatibleTextRendering = true;
            this.btnLeft_Predictors.UseVisualStyleBackColor = true;
            // 
            // btnRight_Predictors
            // 
            this.btnRight_Predictors.Location = new System.Drawing.Point(169, 99);
            this.btnRight_Predictors.Name = "btnRight_Predictors";
            this.btnRight_Predictors.Size = new System.Drawing.Size(17, 20);
            this.btnRight_Predictors.TabIndex = 60;
            this.btnRight_Predictors.Text = "►";
            this.btnRight_Predictors.UseCompatibleTextRendering = true;
            this.btnRight_Predictors.UseVisualStyleBackColor = true;
            // 
            // lvPredictors
            // 
            this.lvPredictors.AllowDrop = true;
            this.lvPredictors.BackColor = System.Drawing.Color.Lavender;
            this.lvPredictors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvPredictors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.lvPredictors.FullRowSelect = true;
            this.lvPredictors.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvPredictors.Location = new System.Drawing.Point(194, 99);
            this.lvPredictors.Name = "lvPredictors";
            this.lvPredictors.Size = new System.Drawing.Size(221, 156);
            this.lvPredictors.TabIndex = 45;
            this.lvPredictors.UseCompatibleStateImageBehavior = false;
            this.lvPredictors.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Width = 20;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Width = 50;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Width = 70;
            // 
            // btnMethods
            // 
            this.btnMethods.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMethods.Location = new System.Drawing.Point(290, 268);
            this.btnMethods.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnMethods.Name = "btnMethods";
            this.btnMethods.Size = new System.Drawing.Size(59, 22);
            this.btnMethods.TabIndex = 39;
            this.btnMethods.Text = "&Methods";
            this.btnMethods.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(356, 302);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(59, 22);
            this.btnCancel.TabIndex = 38;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(290, 302);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(59, 22);
            this.btnOk.TabIndex = 37;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnOptions
            // 
            this.btnOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOptions.Location = new System.Drawing.Point(356, 268);
            this.btnOptions.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Size = new System.Drawing.Size(59, 22);
            this.btnOptions.TabIndex = 36;
            this.btnOptions.Text = "O&ptions";
            this.btnOptions.UseVisualStyleBackColor = true;
            // 
            // btnLeft_Response
            // 
            this.btnLeft_Response.Location = new System.Drawing.Point(249, 35);
            this.btnLeft_Response.Name = "btnLeft_Response";
            this.btnLeft_Response.Size = new System.Drawing.Size(17, 20);
            this.btnLeft_Response.TabIndex = 30;
            this.btnLeft_Response.Text = "◄";
            this.btnLeft_Response.UseCompatibleTextRendering = true;
            this.btnLeft_Response.UseVisualStyleBackColor = true;
            // 
            // btnRight_Response
            // 
            this.btnRight_Response.Location = new System.Drawing.Point(266, 35);
            this.btnRight_Response.Name = "btnRight_Response";
            this.btnRight_Response.Size = new System.Drawing.Size(17, 20);
            this.btnRight_Response.TabIndex = 29;
            this.btnRight_Response.Text = "►";
            this.btnRight_Response.UseCompatibleTextRendering = true;
            this.btnRight_Response.UseVisualStyleBackColor = true;
            // 
            // lblColumns
            // 
            this.lblColumns.AutoSize = true;
            this.lblColumns.Location = new System.Drawing.Point(17, 21);
            this.lblColumns.Name = "lblColumns";
            this.lblColumns.Size = new System.Drawing.Size(47, 13);
            this.lblColumns.TabIndex = 28;
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
            this.lvColumns.Location = new System.Drawing.Point(17, 39);
            this.lvColumns.Name = "lvColumns";
            this.lvColumns.Size = new System.Drawing.Size(145, 284);
            this.lvColumns.TabIndex = 25;
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
            // lblResponse
            // 
            this.lblResponse.AutoSize = true;
            this.lblResponse.Location = new System.Drawing.Point(192, 39);
            this.lblResponse.Name = "lblResponse";
            this.lblResponse.Size = new System.Drawing.Size(54, 13);
            this.lblResponse.TabIndex = 27;
            this.lblResponse.Text = "Response";
            // 
            // lvRespense
            // 
            this.lvRespense.AllowDrop = true;
            this.lvRespense.BackColor = System.Drawing.Color.Lavender;
            this.lvRespense.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvRespense.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colType_Ser,
            this.colID_Ser,
            this.colName_Ser});
            this.lvRespense.FullRowSelect = true;
            this.lvRespense.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvRespense.Location = new System.Drawing.Point(288, 34);
            this.lvRespense.MultiSelect = false;
            this.lvRespense.Name = "lvRespense";
            this.lvRespense.Scrollable = false;
            this.lvRespense.Size = new System.Drawing.Size(127, 21);
            this.lvRespense.TabIndex = 26;
            this.lvRespense.UseCompatibleStateImageBehavior = false;
            this.lvRespense.View = System.Windows.Forms.View.Details;
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
            // DlgRegression
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(431, 339);
            this.Controls.Add(this.pnlBackground);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DlgRegression";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DlgRegression";
            this.pnlBackground.ResumeLayout(false);
            this.pnlBackground.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBackground;
        private System.Windows.Forms.Button btnLeft_Predictors;
        private System.Windows.Forms.Button btnRight_Predictors;
        private System.Windows.Forms.ListView lvPredictors;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Button btnMethods;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnOptions;
        private System.Windows.Forms.Button btnLeft_Response;
        private System.Windows.Forms.Button btnRight_Response;
        private System.Windows.Forms.Label lblColumns;
        private System.Windows.Forms.ListView lvColumns;
        private System.Windows.Forms.ColumnHeader colType;
        private System.Windows.Forms.ColumnHeader colID;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.Label lblResponse;
        private System.Windows.Forms.ListView lvRespense;
        private System.Windows.Forms.ColumnHeader colType_Ser;
        private System.Windows.Forms.ColumnHeader colID_Ser;
        private System.Windows.Forms.ColumnHeader colName_Ser;
        private System.Windows.Forms.Label lblPredictors;
        private System.Windows.Forms.CheckBox chkIntercept;
        private System.Windows.Forms.ImageList imlColumnType;
    }
}