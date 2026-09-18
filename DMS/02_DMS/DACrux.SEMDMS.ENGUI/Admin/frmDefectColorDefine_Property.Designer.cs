namespace DACrux.SEMDMS.ENGUI
{
    partial class frmDefectColorDefine_Property
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
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem1 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem2 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem3 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem4 = new DACrux.Framework.PropertyGrid.PropertyItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDefectColorDefine_Property));
            this.pnlSizeDetail = new System.Windows.Forms.Panel();
            this.pnlColorListBySize = new System.Windows.Forms.Panel();
            this.fpSpread_Size = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread_Size_Sheet = new FarPoint.Win.Spread.SheetView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grid = new DACrux.Framework.PropertyGrid.DucPropertyGrid();
            this.pnlSizeTitle = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.decimalPlaces = new System.Windows.Forms.NumericUpDown();
            this.pnlSizeDetail.SuspendLayout();
            this.pnlColorListBySize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Size)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Size_Sheet)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlSizeTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.decimalPlaces)).BeginInit();
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
            this.pnlColorListBySize.Controls.Add(this.panel1);
            this.pnlColorListBySize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlColorListBySize.Location = new System.Drawing.Point(0, 36);
            this.pnlColorListBySize.Name = "pnlColorListBySize";
            this.pnlColorListBySize.Size = new System.Drawing.Size(896, 491);
            this.pnlColorListBySize.TabIndex = 18;
            // 
            // fpSpread_Size
            // 
            this.fpSpread_Size.AccessibleDescription = "fpSpread_Size, fpSpread_Size";
            this.fpSpread_Size.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpread_Size.Location = new System.Drawing.Point(0, 0);
            this.fpSpread_Size.Name = "fpSpread_Size";
            this.fpSpread_Size.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.fpSpread_Size.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread_Size_Sheet});
            this.fpSpread_Size.Size = new System.Drawing.Size(625, 491);
            this.fpSpread_Size.TabIndex = 10;
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
            this.fpSpread_Size_Sheet.ColumnFooter.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpread_Size_Sheet.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Size_Sheet.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Size_Sheet.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpread_Size_Sheet.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Size_Sheet.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Size_Sheet.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Size_Sheet.ColumnFooterSheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpread_Size_Sheet.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Size_Sheet.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Size_Sheet.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpread_Size_Sheet.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Size_Sheet.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Size_Sheet.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Size_Sheet.ColumnHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
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
            this.fpSpread_Size_Sheet.RowHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpread_Size_Sheet.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Size_Sheet.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Size_Sheet.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpread_Size_Sheet.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Size_Sheet.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Size_Sheet.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Size_Sheet.SheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpread_Size_Sheet.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Size_Sheet.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Size_Sheet.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpread_Size_Sheet.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Size_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grid);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(625, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(271, 491);
            this.panel1.TabIndex = 11;
            // 
            // grid
            // 
            this.grid.ColorValueType = DACrux.Framework.PropertyGrid.ColorValueType.HtmlColor;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.Name = "grid";
            propertyItem1.Category = "Size";
            propertyItem1.ColumnName = "SIZE_SEQ";
            propertyItem1.DisplayName = "Sequence";
            propertyItem1.IsReadOnly = true;
            propertyItem1.IsReadOnlyAtInsertMode = true;
            propertyItem1.IsRequiredField = true;
            propertyItem2.Category = "Size";
            propertyItem2.ColumnName = "SIZE_FROM";
            propertyItem2.DisplayName = "From";
            propertyItem3.Category = "Size";
            propertyItem3.ColumnName = "SIZE_TO";
            propertyItem3.DisplayName = "To";
            propertyItem4.Category = "Size";
            propertyItem4.ColumnName = "COLOR";
            propertyItem4.DisplayName = "Color";
            propertyItem4.InputStyle = DACrux.Framework.PropertyGrid.InputStyle.Color;
            this.grid.PropertyList.Add(propertyItem1);
            this.grid.PropertyList.Add(propertyItem2);
            this.grid.PropertyList.Add(propertyItem3);
            this.grid.PropertyList.Add(propertyItem4);
            this.grid.Size = new System.Drawing.Size(271, 491);
            this.grid.TabIndex = 0;
            this.grid.CommandButtonClick += new DACrux.Framework.PropertyGrid.DucPropertyGrid.SaveEventHandler(this.grid_CommandButtonClick);
            this.grid.CommandComplete += new DACrux.Framework.PropertyGrid.DucPropertyGrid.SaveEventHandler(this.grid_CommandComplete);
            this.grid.StateChanged += new DACrux.Framework.PropertyGrid.DucPropertyGrid.StatechangedEventHandler(this.grid_StateChanged);
            // 
            // pnlSizeTitle
            // 
            this.pnlSizeTitle.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pnlSizeTitle.Controls.Add(this.label1);
            this.pnlSizeTitle.Controls.Add(this.decimalPlaces);
            this.pnlSizeTitle.Controls.Add(this.btnSearch);
            this.pnlSizeTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSizeTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlSizeTitle.Name = "pnlSizeTitle";
            this.pnlSizeTitle.Size = new System.Drawing.Size(896, 36);
            this.pnlSizeTitle.TabIndex = 11;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(790, 6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(103, 23);
            this.btnSearch.TabIndex = 63;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(623, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 12);
            this.label1.TabIndex = 65;
            this.label1.Text = "Decimal Length";
            // 
            // decimalLength
            // 
            this.decimalPlaces.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.decimalPlaces.Location = new System.Drawing.Point(722, 8);
            this.decimalPlaces.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.decimalPlaces.Name = "decimalLength";
            this.decimalPlaces.Size = new System.Drawing.Size(62, 21);
            this.decimalPlaces.TabIndex = 64;
            this.decimalPlaces.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.decimalPlaces.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // frmDefectColorDefine_Property
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(896, 527);
            this.Controls.Add(this.pnlSizeDetail);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDefectColorDefine_Property";
            this.Text = "Defect Coloring";
            this.Load += new System.EventHandler(this.frmDefectColorDefine_Property_Load);
            this.pnlSizeDetail.ResumeLayout(false);
            this.pnlColorListBySize.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Size)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Size_Sheet)).EndInit();
            this.panel1.ResumeLayout(false);
            this.pnlSizeTitle.ResumeLayout(false);
            this.pnlSizeTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.decimalPlaces)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSizeDetail;
        private FarPoint.Win.Spread.FpSpread fpSpread_Size;
        private FarPoint.Win.Spread.SheetView fpSpread_Size_Sheet;
        private System.Windows.Forms.Panel pnlSizeTitle;
        private System.Windows.Forms.Panel pnlColorListBySize;
        private System.Windows.Forms.Panel panel1;
        private Framework.PropertyGrid.DucPropertyGrid grid;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown decimalPlaces;

    }
}