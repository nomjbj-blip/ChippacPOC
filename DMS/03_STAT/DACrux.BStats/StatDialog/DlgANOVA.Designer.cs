namespace DACrux.BStats.StatDialog
{
    partial class DlgANOVA
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgANOVA));
            this.pnlBack = new System.Windows.Forms.Panel();
            this.btnComparisons = new System.Windows.Forms.Button();
            this.btnGraph = new System.Windows.Forms.Button();
            this.grpSingleCol = new System.Windows.Forms.GroupBox();
            this.lblResponse = new System.Windows.Forms.Label();
            this.lvSingleCol = new System.Windows.Forms.ListView();
            this.colType_Ser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colID_Ser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName_Ser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblFactor = new System.Windows.Forms.Label();
            this.btnRight_SingleCol = new System.Windows.Forms.Button();
            this.btnLeft_FactorCol = new System.Windows.Forms.Button();
            this.btnLeft_SingleCol = new System.Windows.Forms.Button();
            this.btnRight_FactorCol = new System.Windows.Forms.Button();
            this.lvFactorCol = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnLeft_SeparateCols = new System.Windows.Forms.Button();
            this.btnRight_SeparateCols = new System.Windows.Forms.Button();
            this.lvSeparateCols = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rdoSeparateCols = new System.Windows.Forms.RadioButton();
            this.rdoSigleColumn = new System.Windows.Forms.RadioButton();
            this.lblDataArrangedAs = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblColumns = new System.Windows.Forms.Label();
            this.lvColumns = new System.Windows.Forms.ListView();
            this.colType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imlColumnType = new System.Windows.Forms.ImageList(this.components);
            this.pnlBack.SuspendLayout();
            this.grpSingleCol.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBack
            // 
            this.pnlBack.Controls.Add(this.btnComparisons);
            this.pnlBack.Controls.Add(this.btnGraph);
            this.pnlBack.Controls.Add(this.grpSingleCol);
            this.pnlBack.Controls.Add(this.btnLeft_SeparateCols);
            this.pnlBack.Controls.Add(this.btnRight_SeparateCols);
            this.pnlBack.Controls.Add(this.lvSeparateCols);
            this.pnlBack.Controls.Add(this.rdoSeparateCols);
            this.pnlBack.Controls.Add(this.rdoSigleColumn);
            this.pnlBack.Controls.Add(this.lblDataArrangedAs);
            this.pnlBack.Controls.Add(this.btnCancel);
            this.pnlBack.Controls.Add(this.btnOk);
            this.pnlBack.Controls.Add(this.lblColumns);
            this.pnlBack.Controls.Add(this.lvColumns);
            this.pnlBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBack.Location = new System.Drawing.Point(0, 0);
            this.pnlBack.Name = "pnlBack";
            this.pnlBack.Size = new System.Drawing.Size(486, 289);
            this.pnlBack.TabIndex = 0;
            // 
            // btnComparisons
            // 
            this.btnComparisons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnComparisons.Location = new System.Drawing.Point(332, 223);
            this.btnComparisons.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnComparisons.Name = "btnComparisons";
            this.btnComparisons.Size = new System.Drawing.Size(64, 21);
            this.btnComparisons.TabIndex = 75;
            this.btnComparisons.Text = "&Compare";
            this.btnComparisons.UseVisualStyleBackColor = true;
            this.btnComparisons.Visible = false;
            // 
            // btnGraph
            // 
            this.btnGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGraph.Location = new System.Drawing.Point(406, 223);
            this.btnGraph.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnGraph.Name = "btnGraph";
            this.btnGraph.Size = new System.Drawing.Size(64, 21);
            this.btnGraph.TabIndex = 74;
            this.btnGraph.Text = "G&raphs...";
            this.btnGraph.UseVisualStyleBackColor = true;
            // 
            // grpSingleCol
            // 
            this.grpSingleCol.Controls.Add(this.lblResponse);
            this.grpSingleCol.Controls.Add(this.lvSingleCol);
            this.grpSingleCol.Controls.Add(this.lblFactor);
            this.grpSingleCol.Controls.Add(this.btnRight_SingleCol);
            this.grpSingleCol.Controls.Add(this.btnLeft_FactorCol);
            this.grpSingleCol.Controls.Add(this.btnLeft_SingleCol);
            this.grpSingleCol.Controls.Add(this.btnRight_FactorCol);
            this.grpSingleCol.Controls.Add(this.lvFactorCol);
            this.grpSingleCol.Location = new System.Drawing.Point(214, 40);
            this.grpSingleCol.Name = "grpSingleCol";
            this.grpSingleCol.Size = new System.Drawing.Size(262, 63);
            this.grpSingleCol.TabIndex = 73;
            this.grpSingleCol.TabStop = false;
            // 
            // lblResponse
            // 
            this.lblResponse.AutoSize = true;
            this.lblResponse.Location = new System.Drawing.Point(9, 17);
            this.lblResponse.Name = "lblResponse";
            this.lblResponse.Size = new System.Drawing.Size(54, 13);
            this.lblResponse.TabIndex = 72;
            this.lblResponse.Text = "Response";
            // 
            // lvSingleCol
            // 
            this.lvSingleCol.AllowDrop = true;
            this.lvSingleCol.BackColor = System.Drawing.Color.Lavender;
            this.lvSingleCol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvSingleCol.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colType_Ser,
            this.colID_Ser,
            this.colName_Ser});
            this.lvSingleCol.FullRowSelect = true;
            this.lvSingleCol.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvSingleCol.Location = new System.Drawing.Point(119, 13);
            this.lvSingleCol.MultiSelect = false;
            this.lvSingleCol.Name = "lvSingleCol";
            this.lvSingleCol.Scrollable = false;
            this.lvSingleCol.Size = new System.Drawing.Size(130, 19);
            this.lvSingleCol.TabIndex = 62;
            this.lvSingleCol.UseCompatibleStateImageBehavior = false;
            this.lvSingleCol.View = System.Windows.Forms.View.Details;
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
            // lblFactor
            // 
            this.lblFactor.AutoSize = true;
            this.lblFactor.Location = new System.Drawing.Point(9, 35);
            this.lblFactor.Name = "lblFactor";
            this.lblFactor.Size = new System.Drawing.Size(38, 13);
            this.lblFactor.TabIndex = 41;
            this.lblFactor.Text = "Factor";
            // 
            // btnRight_SingleCol
            // 
            this.btnRight_SingleCol.Location = new System.Drawing.Point(93, 14);
            this.btnRight_SingleCol.Name = "btnRight_SingleCol";
            this.btnRight_SingleCol.Size = new System.Drawing.Size(20, 19);
            this.btnRight_SingleCol.TabIndex = 64;
            this.btnRight_SingleCol.Text = "►";
            this.btnRight_SingleCol.UseCompatibleTextRendering = true;
            this.btnRight_SingleCol.UseVisualStyleBackColor = true;
            // 
            // btnLeft_FactorCol
            // 
            this.btnLeft_FactorCol.Location = new System.Drawing.Point(73, 35);
            this.btnLeft_FactorCol.Name = "btnLeft_FactorCol";
            this.btnLeft_FactorCol.Size = new System.Drawing.Size(20, 19);
            this.btnLeft_FactorCol.TabIndex = 34;
            this.btnLeft_FactorCol.Text = "◄";
            this.btnLeft_FactorCol.UseCompatibleTextRendering = true;
            this.btnLeft_FactorCol.UseVisualStyleBackColor = true;
            // 
            // btnLeft_SingleCol
            // 
            this.btnLeft_SingleCol.Location = new System.Drawing.Point(73, 14);
            this.btnLeft_SingleCol.Name = "btnLeft_SingleCol";
            this.btnLeft_SingleCol.Size = new System.Drawing.Size(20, 19);
            this.btnLeft_SingleCol.TabIndex = 65;
            this.btnLeft_SingleCol.Text = "◄";
            this.btnLeft_SingleCol.UseCompatibleTextRendering = true;
            this.btnLeft_SingleCol.UseVisualStyleBackColor = true;
            // 
            // btnRight_FactorCol
            // 
            this.btnRight_FactorCol.Location = new System.Drawing.Point(93, 35);
            this.btnRight_FactorCol.Name = "btnRight_FactorCol";
            this.btnRight_FactorCol.Size = new System.Drawing.Size(20, 19);
            this.btnRight_FactorCol.TabIndex = 33;
            this.btnRight_FactorCol.Text = "►";
            this.btnRight_FactorCol.UseCompatibleTextRendering = true;
            this.btnRight_FactorCol.UseVisualStyleBackColor = true;
            // 
            // lvFactorCol
            // 
            this.lvFactorCol.AllowDrop = true;
            this.lvFactorCol.BackColor = System.Drawing.Color.Lavender;
            this.lvFactorCol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvFactorCol.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvFactorCol.FullRowSelect = true;
            this.lvFactorCol.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvFactorCol.Location = new System.Drawing.Point(119, 35);
            this.lvFactorCol.MultiSelect = false;
            this.lvFactorCol.Name = "lvFactorCol";
            this.lvFactorCol.Scrollable = false;
            this.lvFactorCol.Size = new System.Drawing.Size(130, 19);
            this.lvFactorCol.TabIndex = 31;
            this.lvFactorCol.UseCompatibleStateImageBehavior = false;
            this.lvFactorCol.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 20;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 50;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Width = 70;
            // 
            // btnLeft_SeparateCols
            // 
            this.btnLeft_SeparateCols.Location = new System.Drawing.Point(188, 79);
            this.btnLeft_SeparateCols.Name = "btnLeft_SeparateCols";
            this.btnLeft_SeparateCols.Size = new System.Drawing.Size(20, 19);
            this.btnLeft_SeparateCols.TabIndex = 71;
            this.btnLeft_SeparateCols.Text = "◄";
            this.btnLeft_SeparateCols.UseCompatibleTextRendering = true;
            this.btnLeft_SeparateCols.UseVisualStyleBackColor = true;
            // 
            // btnRight_SeparateCols
            // 
            this.btnRight_SeparateCols.Location = new System.Drawing.Point(188, 60);
            this.btnRight_SeparateCols.Name = "btnRight_SeparateCols";
            this.btnRight_SeparateCols.Size = new System.Drawing.Size(20, 19);
            this.btnRight_SeparateCols.TabIndex = 70;
            this.btnRight_SeparateCols.Text = "►";
            this.btnRight_SeparateCols.UseCompatibleTextRendering = true;
            this.btnRight_SeparateCols.UseVisualStyleBackColor = true;
            // 
            // lvSeparateCols
            // 
            this.lvSeparateCols.AllowDrop = true;
            this.lvSeparateCols.BackColor = System.Drawing.Color.Lavender;
            this.lvSeparateCols.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvSeparateCols.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.lvSeparateCols.FullRowSelect = true;
            this.lvSeparateCols.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvSeparateCols.Location = new System.Drawing.Point(214, 132);
            this.lvSeparateCols.Name = "lvSeparateCols";
            this.lvSeparateCols.Size = new System.Drawing.Size(262, 78);
            this.lvSeparateCols.TabIndex = 69;
            this.lvSeparateCols.UseCompatibleStateImageBehavior = false;
            this.lvSeparateCols.View = System.Windows.Forms.View.Details;
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
            // rdoSeparateCols
            // 
            this.rdoSeparateCols.AutoSize = true;
            this.rdoSeparateCols.Location = new System.Drawing.Point(214, 109);
            this.rdoSeparateCols.Name = "rdoSeparateCols";
            this.rdoSeparateCols.Size = new System.Drawing.Size(184, 17);
            this.rdoSeparateCols.TabIndex = 68;
            this.rdoSeparateCols.Text = "Responses(in separate columns):";
            this.rdoSeparateCols.UseVisualStyleBackColor = true;
            // 
            // rdoSigleColumn
            // 
            this.rdoSigleColumn.AutoSize = true;
            this.rdoSigleColumn.Checked = true;
            this.rdoSigleColumn.Location = new System.Drawing.Point(188, 25);
            this.rdoSigleColumn.Name = "rdoSigleColumn";
            this.rdoSigleColumn.Size = new System.Drawing.Size(128, 17);
            this.rdoSigleColumn.TabIndex = 66;
            this.rdoSigleColumn.TabStop = true;
            this.rdoSigleColumn.Text = "Data are arranged as";
            this.rdoSigleColumn.UseVisualStyleBackColor = true;
            // 
            // lblDataArrangedAs
            // 
            this.lblDataArrangedAs.AutoSize = true;
            this.lblDataArrangedAs.Location = new System.Drawing.Point(281, 9);
            this.lblDataArrangedAs.Name = "lblDataArrangedAs";
            this.lblDataArrangedAs.Size = new System.Drawing.Size(0, 13);
            this.lblDataArrangedAs.TabIndex = 63;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(406, 254);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 21);
            this.btnCancel.TabIndex = 40;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(332, 254);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(64, 21);
            this.btnOk.TabIndex = 39;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // lblColumns
            // 
            this.lblColumns.AutoSize = true;
            this.lblColumns.Location = new System.Drawing.Point(20, 20);
            this.lblColumns.Name = "lblColumns";
            this.lblColumns.Size = new System.Drawing.Size(47, 13);
            this.lblColumns.TabIndex = 30;
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
            this.lvColumns.Size = new System.Drawing.Size(145, 237);
            this.lvColumns.TabIndex = 29;
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
            // imlColumnType
            // 
            this.imlColumnType.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlColumnType.ImageStream")));
            this.imlColumnType.TransparentColor = System.Drawing.Color.Transparent;
            this.imlColumnType.Images.SetKeyName(0, "NUMBER");
            this.imlColumnType.Images.SetKeyName(1, "TEXT");
            this.imlColumnType.Images.SetKeyName(2, "DATETIME");
            // 
            // DlgANOVA
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(486, 289);
            this.Controls.Add(this.pnlBack);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DlgANOVA";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ANOVA";
            this.pnlBack.ResumeLayout(false);
            this.pnlBack.PerformLayout();
            this.grpSingleCol.ResumeLayout(false);
            this.grpSingleCol.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBack;
        private System.Windows.Forms.Label lblColumns;
        private System.Windows.Forms.ListView lvColumns;
        private System.Windows.Forms.ColumnHeader colType;
        private System.Windows.Forms.ColumnHeader colID;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ImageList imlColumnType;
        private System.Windows.Forms.Button btnLeft_SeparateCols;
        private System.Windows.Forms.Button btnRight_SeparateCols;
        private System.Windows.Forms.ListView lvSeparateCols;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.RadioButton rdoSeparateCols;
        private System.Windows.Forms.Label lblFactor;
        private System.Windows.Forms.Button btnLeft_FactorCol;
        private System.Windows.Forms.ListView lvFactorCol;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button btnRight_FactorCol;
        private System.Windows.Forms.RadioButton rdoSigleColumn;
        private System.Windows.Forms.Button btnLeft_SingleCol;
        private System.Windows.Forms.Button btnRight_SingleCol;
        private System.Windows.Forms.Label lblDataArrangedAs;
        private System.Windows.Forms.ListView lvSingleCol;
        private System.Windows.Forms.ColumnHeader colType_Ser;
        private System.Windows.Forms.ColumnHeader colID_Ser;
        private System.Windows.Forms.ColumnHeader colName_Ser;
        private System.Windows.Forms.GroupBox grpSingleCol;
        private System.Windows.Forms.Label lblResponse;
        private System.Windows.Forms.Button btnGraph;
        private System.Windows.Forms.Button btnComparisons;

    }
}