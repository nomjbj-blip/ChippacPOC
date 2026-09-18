namespace DACrux.SEMDMS.ENGUI
{
    partial class frmDefectTypeDefine_Property
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDefectTypeDefine_Property));
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem1 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem2 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem3 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem4 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem5 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem6 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem7 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem8 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem9 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem10 = new DACrux.Framework.PropertyGrid.PropertyItem();
            this.fpsDefectType = new FarPoint.Win.Spread.FpSpread();
            this.fpsDefectType_Sheet = new FarPoint.Win.Spread.SheetView();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnReSearch = new System.Windows.Forms.Button();
            this.picBoxTypeColor = new System.Windows.Forms.PictureBox();
            this.lblTypeColor = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grid = new DACrux.Framework.PropertyGrid.DucPropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.fpsDefectType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsDefectType_Sheet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxTypeColor)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fpsDefectType
            // 
            this.fpsDefectType.AccessibleDescription = "";
            this.fpsDefectType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpsDefectType.Location = new System.Drawing.Point(0, 61);
            this.fpsDefectType.Name = "fpsDefectType";
            this.fpsDefectType.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpsDefectType_Sheet});
            this.fpsDefectType.Size = new System.Drawing.Size(628, 466);
            this.fpsDefectType.TabIndex = 59;
            this.fpsDefectType.SelectionChanged += new FarPoint.Win.Spread.SelectionChangedEventHandler(this.fpsDefectType_SelectionChanged);
            // 
            // fpsDefectType_Sheet
            // 
            this.fpsDefectType_Sheet.Reset();
            fpsDefectType_Sheet.SheetName = "Sheet1";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightSlateGray;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(896, 20);
            this.label1.TabIndex = 56;
            this.label1.Text = "    Defect Type Item";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BtnReSearch
            // 
            this.BtnReSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnReSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnReSearch.Image = ((System.Drawing.Image)(resources.GetObject("BtnReSearch.Image")));
            this.BtnReSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnReSearch.Location = new System.Drawing.Point(781, 26);
            this.BtnReSearch.Name = "BtnReSearch";
            this.BtnReSearch.Size = new System.Drawing.Size(103, 23);
            this.BtnReSearch.TabIndex = 62;
            this.BtnReSearch.Text = "Search";
            this.BtnReSearch.UseVisualStyleBackColor = true;
            this.BtnReSearch.Click += new System.EventHandler(this.BtnReSearch_Click);
            // 
            // picBoxTypeColor
            // 
            this.picBoxTypeColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picBoxTypeColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picBoxTypeColor.Location = new System.Drawing.Point(659, 26);
            this.picBoxTypeColor.Margin = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.picBoxTypeColor.Name = "picBoxTypeColor";
            this.picBoxTypeColor.Size = new System.Drawing.Size(107, 23);
            this.picBoxTypeColor.TabIndex = 64;
            this.picBoxTypeColor.TabStop = false;
            this.picBoxTypeColor.Click += new System.EventHandler(this.picBoxTypeColor_Click);
            // 
            // lblTypeColor
            // 
            this.lblTypeColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTypeColor.AutoSize = true;
            this.lblTypeColor.Location = new System.Drawing.Point(603, 28);
            this.lblTypeColor.Name = "lblTypeColor";
            this.lblTypeColor.Size = new System.Drawing.Size(35, 12);
            this.lblTypeColor.TabIndex = 63;
            this.lblTypeColor.Text = "Color";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblTypeColor);
            this.panel2.Controls.Add(this.picBoxTypeColor);
            this.panel2.Controls.Add(this.BtnReSearch);
            this.panel2.Controls.Add(this.btnSearch);
            this.panel2.Controls.Add(this.txtSearch);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(896, 61);
            this.panel2.TabIndex = 58;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.SystemColors.Menu;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.Location = new System.Drawing.Point(187, 25);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(25, 23);
            this.btnSearch.TabIndex = 61;
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.Button_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(12, 27);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(169, 21);
            this.txtSearch.TabIndex = 60;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grid);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(628, 61);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(268, 466);
            this.panel1.TabIndex = 60;
            // 
            // grid
            // 
            this.grid.ColorValueType = DACrux.Framework.PropertyGrid.ColorValueType.HtmlColor;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.Name = "grid";
            propertyItem1.Category = "Defect Class";
            propertyItem1.ColumnName = "CLASSNUMBER";
            propertyItem1.DisplayName = "Number";
            propertyItem1.IsReadOnlyAtInsertMode = true;
            propertyItem1.IsRequiredField = true;
            propertyItem2.Category = "Defect Class";
            propertyItem2.ColumnName = "NAME";
            propertyItem2.DisplayName = "Name";
            propertyItem2.IsRequiredField = true;
            propertyItem3.Category = "Defect Class";
            propertyItem3.ColumnName = "DESCRIPTION";
            propertyItem3.DisplayName = "Description";
            propertyItem4.Category = "Defect Class";
            propertyItem4.ColumnName = "GROUP_ID";
            propertyItem4.DisplayName = "Group ID";
            propertyItem4.InputStyle = DACrux.Framework.PropertyGrid.InputStyle.ComboBox;
            propertyItem5.Category = "Defect Class";
            propertyItem5.ColumnName = "DELETE_FLAG";
            propertyItem5.DisplayName = "Delete Flag";
            propertyItem5.Visible = false;
            propertyItem6.Category = "Defect Class";
            propertyItem6.ColumnName = "DEFECT_COLOR";
            propertyItem6.DisplayName = "Color";
            propertyItem6.InputStyle = DACrux.Framework.PropertyGrid.InputStyle.Color;
            propertyItem7.Category = "Information";
            propertyItem7.ColumnName = "CREATE_TIME";
            propertyItem7.DisplayName = "Create Time";
            propertyItem7.IsReadOnly = true;
            propertyItem7.IsReadOnlyAtInsertMode = true;
            propertyItem8.Category = "Information";
            propertyItem8.ColumnName = "CREATE_USER";
            propertyItem8.DisplayName = "Create User";
            propertyItem8.IsReadOnly = true;
            propertyItem8.IsReadOnlyAtInsertMode = true;
            propertyItem9.Category = "Information";
            propertyItem9.ColumnName = "UPDATE_TIME";
            propertyItem9.DisplayName = "Update Time";
            propertyItem9.IsReadOnly = true;
            propertyItem9.IsReadOnlyAtInsertMode = true;
            propertyItem10.Category = "Information";
            propertyItem10.ColumnName = "UPDATE_USER";
            propertyItem10.DisplayName = "Update User";
            propertyItem10.IsReadOnly = true;
            propertyItem10.IsReadOnlyAtInsertMode = true;
            this.grid.PropertyList.Add(propertyItem1);
            this.grid.PropertyList.Add(propertyItem2);
            this.grid.PropertyList.Add(propertyItem3);
            this.grid.PropertyList.Add(propertyItem4);
            this.grid.PropertyList.Add(propertyItem5);
            this.grid.PropertyList.Add(propertyItem6);
            this.grid.PropertyList.Add(propertyItem7);
            this.grid.PropertyList.Add(propertyItem8);
            this.grid.PropertyList.Add(propertyItem9);
            this.grid.PropertyList.Add(propertyItem10);
            this.grid.Size = new System.Drawing.Size(268, 466);
            this.grid.TabIndex = 0;
            this.grid.DropDownComboBox += new DACrux.Framework.PropertyGrid.DucPropertyGrid.DropDownComboBoxEventHandler(this.grid_DropDownComboBox);
            this.grid.CommandButtonClick += new DACrux.Framework.PropertyGrid.DucPropertyGrid.SaveEventHandler(this.grid_CommandButtonClick);
            this.grid.CommandComplete += new DACrux.Framework.PropertyGrid.DucPropertyGrid.SaveEventHandler(this.grid_CommandComplete);
            this.grid.StateChanged += new DACrux.Framework.PropertyGrid.DucPropertyGrid.StatechangedEventHandler(this.grid_StateChanged);
            // 
            // frmDefectTypeDefine_Property
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(896, 527);
            this.Controls.Add(this.fpsDefectType);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "frmDefectTypeDefine_Property";
            this.Text = "Define By Defect Type";
            this.Load += new System.EventHandler(this.frmDefectTypeDefine_Property_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fpsDefectType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsDefectType_Sheet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxTypeColor)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FarPoint.Win.Spread.FpSpread fpsDefectType;
        private FarPoint.Win.Spread.SheetView fpsDefectType_Sheet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnReSearch;
        private System.Windows.Forms.PictureBox picBoxTypeColor;
        private System.Windows.Forms.Label lblTypeColor;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Panel panel1;
        private Framework.PropertyGrid.DucPropertyGrid grid;
    }
}