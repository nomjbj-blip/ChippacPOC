namespace DACrux.ProjectManager.UI.Dialog
{
    partial class DlgDataViewTranspose
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgDataViewTranspose));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lvColumns = new System.Windows.Forms.ListView();
            this.colType = new System.Windows.Forms.ColumnHeader("(none)");
            this.colID = new System.Windows.Forms.ColumnHeader();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.lblRow = new System.Windows.Forms.Label();
            this.lblColumns = new System.Windows.Forms.Label();
            this.lvRow = new System.Windows.Forms.ListView();
            this.colType_Row = new System.Windows.Forms.ColumnHeader();
            this.colID_Row = new System.Windows.Forms.ColumnHeader();
            this.colName_Row = new System.Windows.Forms.ColumnHeader();
            this.btnLeft_Row = new System.Windows.Forms.Button();
            this.btnRight_Row = new System.Windows.Forms.Button();
            this.imlColumnType = new System.Windows.Forms.ImageList(this.components);
            this.lvColumn = new System.Windows.Forms.ListView();
            this.colType_Col = new System.Windows.Forms.ColumnHeader();
            this.colID_Col = new System.Windows.Forms.ColumnHeader();
            this.colName_Col = new System.Windows.Forms.ColumnHeader();
            this.btnLeft_Col = new System.Windows.Forms.Button();
            this.btnRight_Col = new System.Windows.Forms.Button();
            this.lblColumn = new System.Windows.Forms.Label();
            this.lblTransposeData = new System.Windows.Forms.Label();
            this.lvData = new System.Windows.Forms.ListView();
            this.colType_Data = new System.Windows.Forms.ColumnHeader();
            this.colId_Data = new System.Windows.Forms.ColumnHeader();
            this.colName_Data = new System.Windows.Forms.ColumnHeader();
            this.btnLeft_Data = new System.Windows.Forms.Button();
            this.btnRight_Data = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(233, 254);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(64, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(303, 254);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
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
            this.lvColumns.Location = new System.Drawing.Point(10, 28);
            this.lvColumns.Name = "lvColumns";
            this.lvColumns.Size = new System.Drawing.Size(166, 216);
            this.lvColumns.TabIndex = 7;
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
            // lblRow
            // 
            this.lblRow.AutoSize = true;
            this.lblRow.Location = new System.Drawing.Point(205, 12);
            this.lblRow.Name = "lblRow";
            this.lblRow.Size = new System.Drawing.Size(28, 13);
            this.lblRow.TabIndex = 11;
            this.lblRow.Text = "Row";
            // 
            // lblColumns
            // 
            this.lblColumns.AutoSize = true;
            this.lblColumns.Location = new System.Drawing.Point(10, 12);
            this.lblColumns.Name = "lblColumns";
            this.lblColumns.Size = new System.Drawing.Size(47, 13);
            this.lblColumns.TabIndex = 12;
            this.lblColumns.Text = "Columns";
            // 
            // lvRow
            // 
            this.lvRow.AllowDrop = true;
            this.lvRow.BackColor = System.Drawing.Color.Lavender;
            this.lvRow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvRow.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colType_Row,
            this.colID_Row,
            this.colName_Row});
            this.lvRow.FullRowSelect = true;
            this.lvRow.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvRow.Location = new System.Drawing.Point(208, 28);
            this.lvRow.MultiSelect = false;
            this.lvRow.Name = "lvRow";
            this.lvRow.Size = new System.Drawing.Size(158, 43);
            this.lvRow.TabIndex = 9;
            this.lvRow.UseCompatibleStateImageBehavior = false;
            this.lvRow.View = System.Windows.Forms.View.Details;
            // 
            // colType_Row
            // 
            this.colType_Row.Width = 20;
            // 
            // colID_Row
            // 
            this.colID_Row.Width = 50;
            // 
            // colName_Row
            // 
            this.colName_Row.Width = 70;
            // 
            // btnLeft_Row
            // 
            this.btnLeft_Row.Location = new System.Drawing.Point(182, 52);
            this.btnLeft_Row.Name = "btnLeft_Row";
            this.btnLeft_Row.Size = new System.Drawing.Size(20, 19);
            this.btnLeft_Row.TabIndex = 18;
            this.btnLeft_Row.Text = "◄";
            this.btnLeft_Row.UseCompatibleTextRendering = true;
            this.btnLeft_Row.UseVisualStyleBackColor = true;
            // 
            // btnRight_Row
            // 
            this.btnRight_Row.Location = new System.Drawing.Point(182, 28);
            this.btnRight_Row.Name = "btnRight_Row";
            this.btnRight_Row.Size = new System.Drawing.Size(20, 19);
            this.btnRight_Row.TabIndex = 17;
            this.btnRight_Row.Text = "►";
            this.btnRight_Row.UseCompatibleTextRendering = true;
            this.btnRight_Row.UseVisualStyleBackColor = true;
            // 
            // imlColumnType
            // 
            this.imlColumnType.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlColumnType.ImageStream")));
            this.imlColumnType.TransparentColor = System.Drawing.Color.Transparent;
            this.imlColumnType.Images.SetKeyName(0, "NUMBER");
            this.imlColumnType.Images.SetKeyName(1, "TEXT");
            this.imlColumnType.Images.SetKeyName(2, "DATETIME");
            // 
            // lvColumn
            // 
            this.lvColumn.AllowDrop = true;
            this.lvColumn.BackColor = System.Drawing.Color.Lavender;
            this.lvColumn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvColumn.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colType_Col,
            this.colID_Col,
            this.colName_Col});
            this.lvColumn.FullRowSelect = true;
            this.lvColumn.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvColumn.Location = new System.Drawing.Point(208, 95);
            this.lvColumn.MultiSelect = false;
            this.lvColumn.Name = "lvColumn";
            this.lvColumn.Scrollable = false;
            this.lvColumn.Size = new System.Drawing.Size(158, 43);
            this.lvColumn.TabIndex = 33;
            this.lvColumn.UseCompatibleStateImageBehavior = false;
            this.lvColumn.View = System.Windows.Forms.View.Details;
            // 
            // colType_Col
            // 
            this.colType_Col.Width = 20;
            // 
            // colID_Col
            // 
            this.colID_Col.Width = 50;
            // 
            // colName_Col
            // 
            this.colName_Col.Width = 170;
            // 
            // btnLeft_Col
            // 
            this.btnLeft_Col.Location = new System.Drawing.Point(182, 119);
            this.btnLeft_Col.Name = "btnLeft_Col";
            this.btnLeft_Col.Size = new System.Drawing.Size(20, 19);
            this.btnLeft_Col.TabIndex = 32;
            this.btnLeft_Col.Text = "◄";
            this.btnLeft_Col.UseCompatibleTextRendering = true;
            this.btnLeft_Col.UseVisualStyleBackColor = true;
            // 
            // btnRight_Col
            // 
            this.btnRight_Col.Location = new System.Drawing.Point(182, 95);
            this.btnRight_Col.Name = "btnRight_Col";
            this.btnRight_Col.Size = new System.Drawing.Size(20, 19);
            this.btnRight_Col.TabIndex = 31;
            this.btnRight_Col.Text = "►";
            this.btnRight_Col.UseCompatibleTextRendering = true;
            this.btnRight_Col.UseVisualStyleBackColor = true;
            // 
            // lblColumn
            // 
            this.lblColumn.AutoSize = true;
            this.lblColumn.BackColor = System.Drawing.Color.Transparent;
            this.lblColumn.Location = new System.Drawing.Point(208, 79);
            this.lblColumn.Name = "lblColumn";
            this.lblColumn.Size = new System.Drawing.Size(42, 13);
            this.lblColumn.TabIndex = 30;
            this.lblColumn.Text = "Column";
            // 
            // lblTransposeData
            // 
            this.lblTransposeData.AutoSize = true;
            this.lblTransposeData.Location = new System.Drawing.Point(205, 148);
            this.lblTransposeData.Name = "lblTransposeData";
            this.lblTransposeData.Size = new System.Drawing.Size(83, 13);
            this.lblTransposeData.TabIndex = 35;
            this.lblTransposeData.Text = "Transpose Data";
            // 
            // lvData
            // 
            this.lvData.AllowDrop = true;
            this.lvData.BackColor = System.Drawing.Color.LightYellow;
            this.lvData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colType_Data,
            this.colId_Data,
            this.colName_Data});
            this.lvData.FullRowSelect = true;
            this.lvData.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvData.Location = new System.Drawing.Point(208, 163);
            this.lvData.MultiSelect = false;
            this.lvData.Name = "lvData";
            this.lvData.Size = new System.Drawing.Size(158, 80);
            this.lvData.TabIndex = 34;
            this.lvData.UseCompatibleStateImageBehavior = false;
            this.lvData.View = System.Windows.Forms.View.Details;
            // 
            // colType_Data
            // 
            this.colType_Data.Width = 20;
            // 
            // colId_Data
            // 
            this.colId_Data.Width = 50;
            // 
            // colName_Data
            // 
            this.colName_Data.Width = 70;
            // 
            // btnLeft_Data
            // 
            this.btnLeft_Data.Location = new System.Drawing.Point(182, 188);
            this.btnLeft_Data.Name = "btnLeft_Data";
            this.btnLeft_Data.Size = new System.Drawing.Size(20, 19);
            this.btnLeft_Data.TabIndex = 37;
            this.btnLeft_Data.Text = "◄";
            this.btnLeft_Data.UseCompatibleTextRendering = true;
            this.btnLeft_Data.UseVisualStyleBackColor = true;
            // 
            // btnRight_Data
            // 
            this.btnRight_Data.Location = new System.Drawing.Point(182, 163);
            this.btnRight_Data.Name = "btnRight_Data";
            this.btnRight_Data.Size = new System.Drawing.Size(20, 19);
            this.btnRight_Data.TabIndex = 36;
            this.btnRight_Data.Text = "►";
            this.btnRight_Data.UseCompatibleTextRendering = true;
            this.btnRight_Data.UseVisualStyleBackColor = true;
            // 
            // DlgDataViewTranspose
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 292);
            this.Controls.Add(this.btnLeft_Data);
            this.Controls.Add(this.btnRight_Data);
            this.Controls.Add(this.lblTransposeData);
            this.Controls.Add(this.lvData);
            this.Controls.Add(this.lvColumn);
            this.Controls.Add(this.btnLeft_Col);
            this.Controls.Add(this.btnRight_Col);
            this.Controls.Add(this.lblColumn);
            this.Controls.Add(this.btnLeft_Row);
            this.Controls.Add(this.btnRight_Row);
            this.Controls.Add(this.lblColumns);
            this.Controls.Add(this.lvColumns);
            this.Controls.Add(this.lblRow);
            this.Controls.Add(this.lvRow);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgDataViewTranspose";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListView lvColumns;
        private System.Windows.Forms.ColumnHeader colType;
        private System.Windows.Forms.ColumnHeader colID;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.Label lblRow;
        private System.Windows.Forms.Label lblColumns;
        private System.Windows.Forms.ListView lvRow;
        private System.Windows.Forms.Button btnLeft_Row;
        private System.Windows.Forms.Button btnRight_Row;
        private System.Windows.Forms.ImageList imlColumnType;
        private System.Windows.Forms.ColumnHeader colType_Row;
        private System.Windows.Forms.ColumnHeader colID_Row;
        private System.Windows.Forms.ColumnHeader colName_Row;
        private System.Windows.Forms.ListView lvColumn;
        private System.Windows.Forms.ColumnHeader colType_Col;
        private System.Windows.Forms.ColumnHeader colID_Col;
        private System.Windows.Forms.ColumnHeader colName_Col;
        private System.Windows.Forms.Button btnLeft_Col;
        private System.Windows.Forms.Button btnRight_Col;
        private System.Windows.Forms.Label lblColumn;
        private System.Windows.Forms.Label lblTransposeData;
        private System.Windows.Forms.ListView lvData;
        private System.Windows.Forms.ColumnHeader colType_Data;
        private System.Windows.Forms.ColumnHeader colId_Data;
        private System.Windows.Forms.ColumnHeader colName_Data;
        private System.Windows.Forms.Button btnLeft_Data;
        private System.Windows.Forms.Button btnRight_Data;
    }
}