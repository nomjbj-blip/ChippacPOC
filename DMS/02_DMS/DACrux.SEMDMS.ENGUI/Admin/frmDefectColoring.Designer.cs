namespace DACrux.SEMDMS.ENGUI
{
    partial class frmDefectColoring
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDefectColoring));
            this.pnlSizeDetail = new System.Windows.Forms.Panel();
            this.pnlColorListBySize = new System.Windows.Forms.Panel();
            this.fpSpread_Size = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread_Size_Sheet = new FarPoint.Win.Spread.SheetView();
            this.pnlSizeInfo = new System.Windows.Forms.Panel();
            this.pnlSizeColor = new System.Windows.Forms.Panel();
            this.btnSizeSave = new System.Windows.Forms.Button();
            this.lblSizeColor = new System.Windows.Forms.Label();
            this.picBoxSizeColor = new System.Windows.Forms.PictureBox();
            this.btnView = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.txtSizeTo = new System.Windows.Forms.TextBox();
            this.lblSizeTo = new System.Windows.Forms.Label();
            this.txtSizeFrom = new System.Windows.Forms.TextBox();
            this.lblSizeFrom = new System.Windows.Forms.Label();
            this.pnlSizeTitle = new System.Windows.Forms.Panel();
            this.lblSizeList = new System.Windows.Forms.Label();
            this.pnlSizeDetail.SuspendLayout();
            this.pnlColorListBySize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Size)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Size_Sheet)).BeginInit();
            this.pnlSizeInfo.SuspendLayout();
            this.pnlSizeColor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSizeColor)).BeginInit();
            this.pnlSizeTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSizeDetail
            // 
            this.pnlSizeDetail.Controls.Add(this.pnlColorListBySize);
            this.pnlSizeDetail.Controls.Add(this.pnlSizeTitle);
            this.pnlSizeDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSizeDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlSizeDetail.Name = "pnlSizeDetail";
            this.pnlSizeDetail.Size = new System.Drawing.Size(896, 527);
            this.pnlSizeDetail.TabIndex = 13;
            // 
            // pnlColorListBySize
            // 
            this.pnlColorListBySize.Controls.Add(this.fpSpread_Size);
            this.pnlColorListBySize.Controls.Add(this.pnlSizeInfo);
            this.pnlColorListBySize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlColorListBySize.Location = new System.Drawing.Point(0, 42);
            this.pnlColorListBySize.Name = "pnlColorListBySize";
            this.pnlColorListBySize.Size = new System.Drawing.Size(896, 485);
            this.pnlColorListBySize.TabIndex = 18;
            // 
            // fpSpread_Size
            // 
            this.fpSpread_Size.AccessibleDescription = "fpSpread_Size, fpSpread_Size";
            this.fpSpread_Size.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpread_Size.Location = new System.Drawing.Point(0, 130);
            this.fpSpread_Size.Name = "fpSpread_Size";
            this.fpSpread_Size.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.fpSpread_Size.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread_Size_Sheet});
            this.fpSpread_Size.Size = new System.Drawing.Size(896, 355);
            this.fpSpread_Size.TabIndex = 10;
            this.fpSpread_Size.SelectionChanged += new FarPoint.Win.Spread.SelectionChangedEventHandler(this.fpSpread_Size_SelectionChanged);
            this.fpSpread_Size.SetActiveViewport(0, -1, -1);
            // 
            // fpSpread_Size_Sheet
            // 
            this.fpSpread_Size_Sheet.Reset();
            fpSpread_Size_Sheet.SheetName = "fpSpread_Size";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpread_Size_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpSpread_Size_Sheet.ColumnCount = 0;
            fpSpread_Size_Sheet.RowCount = 0;
            this.fpSpread_Size_Sheet.ActiveColumnIndex = -1;
            this.fpSpread_Size_Sheet.ActiveRowIndex = -1;
            this.fpSpread_Size_Sheet.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin1", System.Drawing.SystemColors.AppWorkspace, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(223)))), ((int)(((byte)(222))))), FarPoint.Win.Spread.GridLines.Horizontal, System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107))))), System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107))))), System.Drawing.Color.White, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(93)))), ((int)(((byte)(90))))), System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(222))))), System.Drawing.Color.White, true, true, true, true, true, true, false, true, "HeaderDefault", "HeaderDefault", "HeaderDefault", "DataAreaDefault", "HeaderDefault");
            this.fpSpread_Size_Sheet.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Size_Sheet.ColumnFooter.Columns.Default.Width = 100F;
            this.fpSpread_Size_Sheet.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Size_Sheet.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Size_Sheet.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Size_Sheet.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpread_Size_Sheet.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Size_Sheet.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Size_Sheet.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Size_Sheet.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Size_Sheet.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Size_Sheet.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpread_Size_Sheet.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Size_Sheet.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Size_Sheet.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Size_Sheet.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Size_Sheet.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Size_Sheet.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpread_Size_Sheet.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Size_Sheet.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Size_Sheet.Columns.Default.Width = 100F;
            this.fpSpread_Size_Sheet.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpSpread_Size_Sheet.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpSpread_Size_Sheet.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Size_Sheet.DefaultStyle.Parent = "DataAreaDefault";
            this.fpSpread_Size_Sheet.RowHeader.Columns.Default.Resizable = false;
            this.fpSpread_Size_Sheet.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Size_Sheet.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Size_Sheet.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Size_Sheet.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Size_Sheet.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpread_Size_Sheet.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Size_Sheet.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Size_Sheet.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Size_Sheet.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Size_Sheet.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Size_Sheet.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpread_Size_Sheet.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Size_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // pnlSizeInfo
            // 
            this.pnlSizeInfo.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pnlSizeInfo.Controls.Add(this.pnlSizeColor);
            this.pnlSizeInfo.Controls.Add(this.btnView);
            this.pnlSizeInfo.Controls.Add(this.btnDelete);
            this.pnlSizeInfo.Controls.Add(this.btnUpdate);
            this.pnlSizeInfo.Controls.Add(this.btnCreate);
            this.pnlSizeInfo.Controls.Add(this.txtSizeTo);
            this.pnlSizeInfo.Controls.Add(this.lblSizeTo);
            this.pnlSizeInfo.Controls.Add(this.txtSizeFrom);
            this.pnlSizeInfo.Controls.Add(this.lblSizeFrom);
            this.pnlSizeInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSizeInfo.Location = new System.Drawing.Point(0, 0);
            this.pnlSizeInfo.Name = "pnlSizeInfo";
            this.pnlSizeInfo.Size = new System.Drawing.Size(896, 130);
            this.pnlSizeInfo.TabIndex = 12;
            // 
            // pnlSizeColor
            // 
            this.pnlSizeColor.Controls.Add(this.btnSizeSave);
            this.pnlSizeColor.Controls.Add(this.lblSizeColor);
            this.pnlSizeColor.Controls.Add(this.picBoxSizeColor);
            this.pnlSizeColor.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSizeColor.Location = new System.Drawing.Point(751, 0);
            this.pnlSizeColor.Name = "pnlSizeColor";
            this.pnlSizeColor.Size = new System.Drawing.Size(145, 130);
            this.pnlSizeColor.TabIndex = 19;
            // 
            // btnSizeSave
            // 
            this.btnSizeSave.Location = new System.Drawing.Point(30, 101);
            this.btnSizeSave.Name = "btnSizeSave";
            this.btnSizeSave.Size = new System.Drawing.Size(82, 23);
            this.btnSizeSave.TabIndex = 20;
            this.btnSizeSave.Text = "Save";
            this.btnSizeSave.UseVisualStyleBackColor = true;
            this.btnSizeSave.Click += new System.EventHandler(this.btnSizeSave_Click);
            // 
            // lblSizeColor
            // 
            this.lblSizeColor.AutoSize = true;
            this.lblSizeColor.Location = new System.Drawing.Point(48, 9);
            this.lblSizeColor.Name = "lblSizeColor";
            this.lblSizeColor.Size = new System.Drawing.Size(41, 15);
            this.lblSizeColor.TabIndex = 11;
            this.lblSizeColor.Text = "Color";
            // 
            // picBoxSizeColor
            // 
            this.picBoxSizeColor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picBoxSizeColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picBoxSizeColor.Location = new System.Drawing.Point(19, 32);
            this.picBoxSizeColor.Margin = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.picBoxSizeColor.Name = "picBoxSizeColor";
            this.picBoxSizeColor.Size = new System.Drawing.Size(107, 61);
            this.picBoxSizeColor.TabIndex = 18;
            this.picBoxSizeColor.TabStop = false;
            this.picBoxSizeColor.Click += new System.EventHandler(this.picBoxSizeColor_Click);
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(279, 101);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(82, 23);
            this.btnView.TabIndex = 17;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(190, 101);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(82, 23);
            this.btnDelete.TabIndex = 16;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(99, 101);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(82, 23);
            this.btnUpdate.TabIndex = 15;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(8, 101);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(82, 23);
            this.btnCreate.TabIndex = 0;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // txtSizeTo
            // 
            this.txtSizeTo.Location = new System.Drawing.Point(7, 70);
            this.txtSizeTo.Name = "txtSizeTo";
            this.txtSizeTo.Size = new System.Drawing.Size(265, 21);
            this.txtSizeTo.TabIndex = 14;
            // 
            // lblSizeTo
            // 
            this.lblSizeTo.AutoSize = true;
            this.lblSizeTo.Location = new System.Drawing.Point(3, 49);
            this.lblSizeTo.Name = "lblSizeTo";
            this.lblSizeTo.Size = new System.Drawing.Size(55, 15);
            this.lblSizeTo.TabIndex = 13;
            this.lblSizeTo.Text = "Size To";
            // 
            // txtSizeFrom
            // 
            this.txtSizeFrom.Location = new System.Drawing.Point(7, 26);
            this.txtSizeFrom.Name = "txtSizeFrom";
            this.txtSizeFrom.Size = new System.Drawing.Size(265, 21);
            this.txtSizeFrom.TabIndex = 12;
            // 
            // lblSizeFrom
            // 
            this.lblSizeFrom.AutoSize = true;
            this.lblSizeFrom.Location = new System.Drawing.Point(3, 6);
            this.lblSizeFrom.Name = "lblSizeFrom";
            this.lblSizeFrom.Size = new System.Drawing.Size(72, 15);
            this.lblSizeFrom.TabIndex = 11;
            this.lblSizeFrom.Text = "Size From";
            // 
            // pnlSizeTitle
            // 
            this.pnlSizeTitle.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pnlSizeTitle.Controls.Add(this.lblSizeList);
            this.pnlSizeTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSizeTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlSizeTitle.Name = "pnlSizeTitle";
            this.pnlSizeTitle.Size = new System.Drawing.Size(896, 42);
            this.pnlSizeTitle.TabIndex = 11;
            // 
            // lblSizeList
            // 
            this.lblSizeList.AutoSize = true;
            this.lblSizeList.Image = ((System.Drawing.Image)(resources.GetObject("lblSizeList.Image")));
            this.lblSizeList.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSizeList.Location = new System.Drawing.Point(3, 4);
            this.lblSizeList.Name = "lblSizeList";
            this.lblSizeList.Size = new System.Drawing.Size(144, 15);
            this.lblSizeList.TabIndex = 11;
            this.lblSizeList.Text = "  Color by Defect Size";
            // 
            // frmDefectColoring
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(896, 527);
            this.Controls.Add(this.pnlSizeDetail);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDefectColoring";
            this.Text = "Defect Coloring";
            this.Load += new System.EventHandler(this.frmDefectColoring_Load);
            this.pnlSizeDetail.ResumeLayout(false);
            this.pnlColorListBySize.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Size)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Size_Sheet)).EndInit();
            this.pnlSizeInfo.ResumeLayout(false);
            this.pnlSizeInfo.PerformLayout();
            this.pnlSizeColor.ResumeLayout(false);
            this.pnlSizeColor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSizeColor)).EndInit();
            this.pnlSizeTitle.ResumeLayout(false);
            this.pnlSizeTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSizeDetail;
        private FarPoint.Win.Spread.FpSpread fpSpread_Size;
        private FarPoint.Win.Spread.SheetView fpSpread_Size_Sheet;
        private System.Windows.Forms.Panel pnlSizeInfo;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.TextBox txtSizeTo;
        private System.Windows.Forms.Label lblSizeTo;
        private System.Windows.Forms.TextBox txtSizeFrom;
        private System.Windows.Forms.Label lblSizeFrom;
        private System.Windows.Forms.Panel pnlSizeTitle;
        private System.Windows.Forms.Label lblSizeList;
        private System.Windows.Forms.Label lblSizeColor;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Panel pnlSizeColor;
        private System.Windows.Forms.PictureBox picBoxSizeColor;
        private System.Windows.Forms.Panel pnlColorListBySize;
        private System.Windows.Forms.Button btnSizeSave;

    }
}