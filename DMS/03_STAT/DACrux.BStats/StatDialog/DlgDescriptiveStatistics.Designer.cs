namespace DACrux.BStats.StatDialog
{
    partial class DlgDescriptiveStatistics
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgDescriptiveStatistics));
            this.pnlBackground = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnGraph = new System.Windows.Forms.Button();
            this.btnStatistics = new System.Windows.Forms.Button();
            this.btnLeft_ByVal = new System.Windows.Forms.Button();
            this.btnRight_ByVal = new System.Windows.Forms.Button();
            this.lblByVar = new System.Windows.Forms.Label();
            this.lvByVar = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            this.pnlBackground.Controls.Add(this.btnCancel);
            this.pnlBackground.Controls.Add(this.btnOk);
            this.pnlBackground.Controls.Add(this.btnGraph);
            this.pnlBackground.Controls.Add(this.btnStatistics);
            this.pnlBackground.Controls.Add(this.btnLeft_ByVal);
            this.pnlBackground.Controls.Add(this.btnRight_ByVal);
            this.pnlBackground.Controls.Add(this.lblByVar);
            this.pnlBackground.Controls.Add(this.lvByVar);
            this.pnlBackground.Controls.Add(this.btnLeft_Var);
            this.pnlBackground.Controls.Add(this.btnRight_Var);
            this.pnlBackground.Controls.Add(this.lblColumns);
            this.pnlBackground.Controls.Add(this.lvColumns);
            this.pnlBackground.Controls.Add(this.lblVariables);
            this.pnlBackground.Controls.Add(this.lvVariables);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Size = new System.Drawing.Size(365, 306);
            this.pnlBackground.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(275, 271);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 21);
            this.btnCancel.TabIndex = 38;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(198, 271);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(69, 21);
            this.btnOk.TabIndex = 37;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnGraph
            // 
            this.btnGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGraph.Location = new System.Drawing.Point(275, 239);
            this.btnGraph.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnGraph.Name = "btnGraph";
            this.btnGraph.Size = new System.Drawing.Size(69, 21);
            this.btnGraph.TabIndex = 36;
            this.btnGraph.Text = "G&raphs...";
            this.btnGraph.UseVisualStyleBackColor = true;
            this.btnGraph.Visible = false;
            // 
            // btnStatistics
            // 
            this.btnStatistics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStatistics.Location = new System.Drawing.Point(198, 239);
            this.btnStatistics.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnStatistics.Name = "btnStatistics";
            this.btnStatistics.Size = new System.Drawing.Size(69, 21);
            this.btnStatistics.TabIndex = 35;
            this.btnStatistics.Text = "&Statistics...";
            this.btnStatistics.UseVisualStyleBackColor = true;
            // 
            // btnLeft_ByVal
            // 
            this.btnLeft_ByVal.Location = new System.Drawing.Point(170, 212);
            this.btnLeft_ByVal.Name = "btnLeft_ByVal";
            this.btnLeft_ByVal.Size = new System.Drawing.Size(20, 19);
            this.btnLeft_ByVal.TabIndex = 34;
            this.btnLeft_ByVal.Text = "◄";
            this.btnLeft_ByVal.UseCompatibleTextRendering = true;
            this.btnLeft_ByVal.UseVisualStyleBackColor = true;
            // 
            // btnRight_ByVal
            // 
            this.btnRight_ByVal.Location = new System.Drawing.Point(170, 188);
            this.btnRight_ByVal.Name = "btnRight_ByVal";
            this.btnRight_ByVal.Size = new System.Drawing.Size(20, 19);
            this.btnRight_ByVal.TabIndex = 33;
            this.btnRight_ByVal.Text = "►";
            this.btnRight_ByVal.UseCompatibleTextRendering = true;
            this.btnRight_ByVal.UseVisualStyleBackColor = true;
            // 
            // lblByVar
            // 
            this.lblByVar.AutoSize = true;
            this.lblByVar.Location = new System.Drawing.Point(197, 172);
            this.lblByVar.Name = "lblByVar";
            this.lblByVar.Size = new System.Drawing.Size(124, 13);
            this.lblByVar.TabIndex = 32;
            this.lblByVar.Text = "Class Variables(optional)";
            // 
            // lvByVar
            // 
            this.lvByVar.AllowDrop = true;
            this.lvByVar.BackColor = System.Drawing.Color.Lavender;
            this.lvByVar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvByVar.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvByVar.FullRowSelect = true;
            this.lvByVar.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvByVar.Location = new System.Drawing.Point(198, 188);
            this.lvByVar.Name = "lvByVar";
            this.lvByVar.Size = new System.Drawing.Size(145, 43);
            this.lvByVar.TabIndex = 31;
            this.lvByVar.UseCompatibleStateImageBehavior = false;
            this.lvByVar.View = System.Windows.Forms.View.Details;
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
            // btnLeft_Var
            // 
            this.btnLeft_Var.Location = new System.Drawing.Point(171, 61);
            this.btnLeft_Var.Name = "btnLeft_Var";
            this.btnLeft_Var.Size = new System.Drawing.Size(20, 19);
            this.btnLeft_Var.TabIndex = 30;
            this.btnLeft_Var.Text = "◄";
            this.btnLeft_Var.UseCompatibleTextRendering = true;
            this.btnLeft_Var.UseVisualStyleBackColor = true;
            // 
            // btnRight_Var
            // 
            this.btnRight_Var.Location = new System.Drawing.Point(171, 37);
            this.btnRight_Var.Name = "btnRight_Var";
            this.btnRight_Var.Size = new System.Drawing.Size(20, 19);
            this.btnRight_Var.TabIndex = 29;
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
            this.lvColumns.Location = new System.Drawing.Point(20, 36);
            this.lvColumns.Name = "lvColumns";
            this.lvColumns.Size = new System.Drawing.Size(145, 258);
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
            // lblVariables
            // 
            this.lblVariables.AutoSize = true;
            this.lblVariables.Location = new System.Drawing.Point(197, 20);
            this.lblVariables.Name = "lblVariables";
            this.lblVariables.Size = new System.Drawing.Size(50, 13);
            this.lblVariables.TabIndex = 27;
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
            this.lvVariables.Size = new System.Drawing.Size(145, 121);
            this.lvVariables.TabIndex = 26;
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
            // DlgDescriptiveStatistics
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(365, 306);
            this.Controls.Add(this.pnlBackground);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "DlgDescriptiveStatistics";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Display Descriptive Statistics";
            this.pnlBackground.ResumeLayout(false);
            this.pnlBackground.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBackground;
        private System.Windows.Forms.Button btnLeft_ByVal;
        private System.Windows.Forms.Button btnRight_ByVal;
        private System.Windows.Forms.Label lblByVar;
        private System.Windows.Forms.ListView lvByVar;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
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
        private System.Windows.Forms.ImageList imlColumnType;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnGraph;
        private System.Windows.Forms.Button btnStatistics;

    }
}