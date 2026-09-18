namespace DACrux.SEMDMS.ENGUI
{
    partial class frmGlobalConfiguration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGlobalConfiguration));
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem1 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem2 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem3 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem4 = new DACrux.Framework.PropertyGrid.PropertyItem();
            this.pnlSizeTitle = new System.Windows.Forms.Panel();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.pnlColorListBySize = new System.Windows.Forms.Panel();
            this.fpSpreadConfig = new FarPoint.Win.Spread.FpSpread();
            this.fpSpreadConfig_fpSpread = new FarPoint.Win.Spread.SheetView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gridProperty = new DACrux.Framework.PropertyGrid.DucPropertyGrid();
            this.pnlSizeTitle.SuspendLayout();
            this.pnlColorListBySize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadConfig_fpSpread)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSizeTitle
            // 
            this.pnlSizeTitle.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pnlSizeTitle.Controls.Add(this.cmbCategory);
            this.pnlSizeTitle.Controls.Add(this.label1);
            this.pnlSizeTitle.Controls.Add(this.btnSearch);
            this.pnlSizeTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSizeTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlSizeTitle.Name = "pnlSizeTitle";
            this.pnlSizeTitle.Size = new System.Drawing.Size(873, 36);
            this.pnlSizeTitle.TabIndex = 12;
            // 
            // cmbCategory
            // 
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(74, 9);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(246, 20);
            this.cmbCategory.TabIndex = 66;
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.cmbCategory_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 12);
            this.label1.TabIndex = 64;
            this.label1.Text = "Category";
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(743, 6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(127, 23);
            this.btnSearch.TabIndex = 63;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // pnlColorListBySize
            // 
            this.pnlColorListBySize.Controls.Add(this.fpSpreadConfig);
            this.pnlColorListBySize.Controls.Add(this.panel1);
            this.pnlColorListBySize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlColorListBySize.Location = new System.Drawing.Point(0, 36);
            this.pnlColorListBySize.Name = "pnlColorListBySize";
            this.pnlColorListBySize.Size = new System.Drawing.Size(873, 540);
            this.pnlColorListBySize.TabIndex = 19;
            // 
            // fpSpreadConfig
            // 
            this.fpSpreadConfig.AccessibleDescription = "fpSpread_Size, fpSpread_Size";
            this.fpSpreadConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpreadConfig.Location = new System.Drawing.Point(0, 0);
            this.fpSpreadConfig.Name = "fpSpreadConfig";
            this.fpSpreadConfig.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.fpSpreadConfig.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpreadConfig_fpSpread});
            this.fpSpreadConfig.Size = new System.Drawing.Size(602, 540);
            this.fpSpreadConfig.TabIndex = 10;
            this.fpSpreadConfig.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpSpreadConfig_CellClick);
            // 
            // fpSpreadConfig_fpSpread
            // 
            this.fpSpreadConfig_fpSpread.Reset();
            fpSpreadConfig_fpSpread.SheetName = "fpSpread_Size";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpreadConfig_fpSpread.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpSpreadConfig_fpSpread.ColumnCount = 0;
            fpSpreadConfig_fpSpread.RowCount = 0;
            this.fpSpreadConfig_fpSpread.ActiveColumnIndex = -1;
            this.fpSpreadConfig_fpSpread.ActiveRowIndex = -1;
            this.fpSpreadConfig_fpSpread.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin1", System.Drawing.SystemColors.AppWorkspace, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(223)))), ((int)(((byte)(222))))), FarPoint.Win.Spread.GridLines.Horizontal, System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107))))), System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107))))), System.Drawing.Color.White, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(93)))), ((int)(((byte)(90))))), System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(222))))), System.Drawing.Color.White, true, true, true, true, true, true, false, true, "HeaderDefault", "HeaderDefault", "HeaderDefault", "DataAreaDefault", "HeaderDefault");
            this.fpSpreadConfig_fpSpread.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadConfig_fpSpread.ColumnFooter.Columns.Default.Width = 100F;
            this.fpSpreadConfig_fpSpread.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadConfig_fpSpread.ColumnFooter.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadConfig_fpSpread.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadConfig_fpSpread.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadConfig_fpSpread.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadConfig_fpSpread.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadConfig_fpSpread.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadConfig_fpSpread.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadConfig_fpSpread.ColumnFooterSheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadConfig_fpSpread.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadConfig_fpSpread.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadConfig_fpSpread.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadConfig_fpSpread.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadConfig_fpSpread.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadConfig_fpSpread.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadConfig_fpSpread.ColumnHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadConfig_fpSpread.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadConfig_fpSpread.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadConfig_fpSpread.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadConfig_fpSpread.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadConfig_fpSpread.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadConfig_fpSpread.Columns.Default.Width = 100F;
            this.fpSpreadConfig_fpSpread.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpSpreadConfig_fpSpread.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpSpreadConfig_fpSpread.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadConfig_fpSpread.DefaultStyle.Parent = "DataAreaDefault";
            this.fpSpreadConfig_fpSpread.RowHeader.Columns.Default.Resizable = false;
            this.fpSpreadConfig_fpSpread.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadConfig_fpSpread.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadConfig_fpSpread.RowHeader.DefaultStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadConfig_fpSpread.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadConfig_fpSpread.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadConfig_fpSpread.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpreadConfig_fpSpread.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadConfig_fpSpread.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadConfig_fpSpread.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpreadConfig_fpSpread.SheetCornerStyle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.fpSpreadConfig_fpSpread.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpreadConfig_fpSpread.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpreadConfig_fpSpread.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpreadConfig_fpSpread.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpreadConfig_fpSpread.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridProperty);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(602, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(271, 540);
            this.panel1.TabIndex = 11;
            // 
            // gridProperty
            // 
            this.gridProperty.ColorValueType = DACrux.Framework.PropertyGrid.ColorValueType.HtmlColor;
            this.gridProperty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridProperty.Location = new System.Drawing.Point(0, 0);
            this.gridProperty.Name = "gridProperty";
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
            this.gridProperty.PropertyList.Add(propertyItem1);
            this.gridProperty.PropertyList.Add(propertyItem2);
            this.gridProperty.PropertyList.Add(propertyItem3);
            this.gridProperty.PropertyList.Add(propertyItem4);
            this.gridProperty.Size = new System.Drawing.Size(271, 540);
            this.gridProperty.TabIndex = 0;
            this.gridProperty.CommandButtonClick += new DACrux.Framework.PropertyGrid.DucPropertyGrid.SaveEventHandler(this.grid_CommandButtonClick);
            // 
            // frmGlobalConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 576);
            this.Controls.Add(this.pnlColorListBySize);
            this.Controls.Add(this.pnlSizeTitle);
            this.Name = "frmGlobalConfiguration";
            this.Text = "Global Configration";
            this.Load += new System.EventHandler(this.frmGlobalConfiguration_Load);
            this.pnlSizeTitle.ResumeLayout(false);
            this.pnlSizeTitle.PerformLayout();
            this.pnlColorListBySize.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadConfig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpreadConfig_fpSpread)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSizeTitle;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel pnlColorListBySize;
        private FarPoint.Win.Spread.FpSpread fpSpreadConfig;
        private FarPoint.Win.Spread.SheetView fpSpreadConfig_fpSpread;
        private System.Windows.Forms.Panel panel1;
        private Framework.PropertyGrid.DucPropertyGrid gridProperty;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbCategory;


    }
}