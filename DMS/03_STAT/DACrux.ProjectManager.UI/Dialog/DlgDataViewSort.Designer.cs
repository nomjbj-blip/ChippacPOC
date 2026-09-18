namespace DACrux.ProjectManager.UI.Dialog
{
    partial class DlgDataViewSort
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgDataViewSort));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lvLeft = new System.Windows.Forms.ListView();
            this.colMode = new System.Windows.Forms.ColumnHeader("sort_asc.gif");
            this.colID = new System.Windows.Forms.ColumnHeader();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.imlSortMode = new System.Windows.Forms.ImageList(this.components);
            this.chkNewWorksheet = new System.Windows.Forms.CheckBox();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnSortMode = new System.Windows.Forms.Button();
            this.lvRight = new System.Windows.Forms.ListView();
            this.colMode_ = new System.Windows.Forms.ColumnHeader();
            this.colID_ = new System.Windows.Forms.ColumnHeader();
            this.colName_ = new System.Windows.Forms.ColumnHeader();
            this.chkSelectedAreaOnly = new System.Windows.Forms.CheckBox();
            this.lblBaseColumn = new System.Windows.Forms.Label();
            this.lblTargetColumn = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(244, 253);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(64, 25);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(314, 253);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 25);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lvLeft
            // 
            this.lvLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvLeft.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colMode,
            this.colID,
            this.colName});
            this.lvLeft.FullRowSelect = true;
            this.lvLeft.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvLeft.Location = new System.Drawing.Point(12, 24);
            this.lvLeft.MultiSelect = false;
            this.lvLeft.Name = "lvLeft";
            this.lvLeft.Size = new System.Drawing.Size(166, 212);
            this.lvLeft.SmallImageList = this.imlSortMode;
            this.lvLeft.TabIndex = 6;
            this.lvLeft.UseCompatibleStateImageBehavior = false;
            this.lvLeft.View = System.Windows.Forms.View.Details;
            this.lvLeft.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvLeft_MouseDoubleClick);
            // 
            // colMode
            // 
            this.colMode.Text = "Mode";
            this.colMode.Width = 0;
            // 
            // colID
            // 
            this.colID.Text = "ID";
            this.colID.Width = 50;
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 110;
            // 
            // imlSortMode
            // 
            this.imlSortMode.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlSortMode.ImageStream")));
            this.imlSortMode.TransparentColor = System.Drawing.Color.Transparent;
            this.imlSortMode.Images.SetKeyName(0, "sort_asc.gif");
            this.imlSortMode.Images.SetKeyName(1, "sort_desc.gif");
            // 
            // chkNewWorksheet
            // 
            this.chkNewWorksheet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkNewWorksheet.AutoSize = true;
            this.chkNewWorksheet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkNewWorksheet.Location = new System.Drawing.Point(13, 245);
            this.chkNewWorksheet.Name = "chkNewWorksheet";
            this.chkNewWorksheet.Size = new System.Drawing.Size(135, 17);
            this.chkNewWorksheet.TabIndex = 8;
            this.chkNewWorksheet.Text = "Create New Worksheet";
            this.chkNewWorksheet.UseVisualStyleBackColor = true;
            // 
            // btnRight
            // 
            this.btnRight.Location = new System.Drawing.Point(184, 24);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(20, 19);
            this.btnRight.TabIndex = 9;
            this.btnRight.Text = "►";
            this.btnRight.UseCompatibleTextRendering = true;
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // btnLeft
            // 
            this.btnLeft.Location = new System.Drawing.Point(184, 48);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(20, 19);
            this.btnLeft.TabIndex = 10;
            this.btnLeft.Text = "◄";
            this.btnLeft.UseCompatibleTextRendering = true;
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(184, 90);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(20, 19);
            this.btnUp.TabIndex = 11;
            this.btnUp.Text = "▲";
            this.btnUp.UseCompatibleTextRendering = true;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(184, 114);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(20, 19);
            this.btnDown.TabIndex = 12;
            this.btnDown.Text = "▼";
            this.btnDown.UseCompatibleTextRendering = true;
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnSortMode
            // 
            this.btnSortMode.Location = new System.Drawing.Point(298, 6);
            this.btnSortMode.Name = "btnSortMode";
            this.btnSortMode.Size = new System.Drawing.Size(80, 19);
            this.btnSortMode.TabIndex = 13;
            this.btnSortMode.Text = "ASC / DESC";
            this.btnSortMode.UseCompatibleTextRendering = true;
            this.btnSortMode.UseVisualStyleBackColor = true;
            this.btnSortMode.Click += new System.EventHandler(this.btnSortMode_Click);
            // 
            // lvRight
            // 
            this.lvRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvRight.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colMode_,
            this.colID_,
            this.colName_});
            this.lvRight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvRight.FullRowSelect = true;
            this.lvRight.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvRight.Location = new System.Drawing.Point(210, 24);
            this.lvRight.MultiSelect = false;
            this.lvRight.Name = "lvRight";
            this.lvRight.Size = new System.Drawing.Size(168, 212);
            this.lvRight.SmallImageList = this.imlSortMode;
            this.lvRight.TabIndex = 14;
            this.lvRight.UseCompatibleStateImageBehavior = false;
            this.lvRight.View = System.Windows.Forms.View.Details;
            this.lvRight.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvRight_MouseDoubleClick);
            // 
            // colMode_
            // 
            this.colMode_.Text = "";
            this.colMode_.Width = 22;
            // 
            // colID_
            // 
            this.colID_.Text = "ID";
            this.colID_.Width = 50;
            // 
            // colName_
            // 
            this.colName_.Text = "Name";
            this.colName_.Width = 90;
            // 
            // chkSelectedAreaOnly
            // 
            this.chkSelectedAreaOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSelectedAreaOnly.AutoSize = true;
            this.chkSelectedAreaOnly.Checked = true;
            this.chkSelectedAreaOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSelectedAreaOnly.Enabled = false;
            this.chkSelectedAreaOnly.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkSelectedAreaOnly.Location = new System.Drawing.Point(12, 262);
            this.chkSelectedAreaOnly.Name = "chkSelectedAreaOnly";
            this.chkSelectedAreaOnly.Size = new System.Drawing.Size(153, 17);
            this.chkSelectedAreaOnly.TabIndex = 15;
            this.chkSelectedAreaOnly.Text = "Sort the selected area only";
            this.chkSelectedAreaOnly.UseVisualStyleBackColor = true;
            // 
            // lblBaseColumn
            // 
            this.lblBaseColumn.AutoSize = true;
            this.lblBaseColumn.Location = new System.Drawing.Point(12, 8);
            this.lblBaseColumn.Name = "lblBaseColumn";
            this.lblBaseColumn.Size = new System.Drawing.Size(68, 13);
            this.lblBaseColumn.TabIndex = 16;
            this.lblBaseColumn.Text = "Base Column";
            // 
            // lblTargetColumn
            // 
            this.lblTargetColumn.AutoSize = true;
            this.lblTargetColumn.Location = new System.Drawing.Point(207, 8);
            this.lblTargetColumn.Name = "lblTargetColumn";
            this.lblTargetColumn.Size = new System.Drawing.Size(68, 13);
            this.lblTargetColumn.TabIndex = 17;
            this.lblTargetColumn.Text = "Base Column";
            // 
            // DlgDataViewSort
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 295);
            this.Controls.Add(this.lblTargetColumn);
            this.Controls.Add(this.lblBaseColumn);
            this.Controls.Add(this.chkSelectedAreaOnly);
            this.Controls.Add(this.lvRight);
            this.Controls.Add(this.btnSortMode);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.btnLeft);
            this.Controls.Add(this.btnRight);
            this.Controls.Add(this.chkNewWorksheet);
            this.Controls.Add(this.lvLeft);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgDataViewSort";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sort";
            this.Load += new System.EventHandler(this.DlgDataViewSort_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListView lvLeft;
        private System.Windows.Forms.CheckBox chkNewWorksheet;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnSortMode;
        private System.Windows.Forms.ColumnHeader colID;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ListView lvRight;
        private System.Windows.Forms.ColumnHeader colID_;
        private System.Windows.Forms.ColumnHeader colName_;
        private System.Windows.Forms.ColumnHeader colMode;
        private System.Windows.Forms.ImageList imlSortMode;
        private System.Windows.Forms.ColumnHeader colMode_;
        private System.Windows.Forms.CheckBox chkSelectedAreaOnly;
        private System.Windows.Forms.Label lblBaseColumn;
        private System.Windows.Forms.Label lblTargetColumn;
    }
}