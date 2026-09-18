namespace DACrux.SEMDMS.ENGUI
{
    partial class frmDefectTypeGroup_Property
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDefectTypeGroup_Property));
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem1 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem2 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem3 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem4 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem5 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem6 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem7 = new DACrux.Framework.PropertyGrid.PropertyItem();
            this.fpsDefectGroup = new FarPoint.Win.Spread.FpSpread();
            this.fpsDefectGroup_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnReSearch = new System.Windows.Forms.Button();
            this.picBoxTypeColor = new System.Windows.Forms.PictureBox();
            this.lblTypeColor = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grid = new DACrux.Framework.PropertyGrid.DucPropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.fpsDefectGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsDefectGroup_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxTypeColor)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fpsDefectGroup
            // 
            this.fpsDefectGroup.AccessibleDescription = "";
            this.fpsDefectGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpsDefectGroup.Location = new System.Drawing.Point(0, 61);
            this.fpsDefectGroup.Name = "fpsDefectGroup";
            this.fpsDefectGroup.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpsDefectGroup_Sheet1});
            this.fpsDefectGroup.Size = new System.Drawing.Size(628, 466);
            this.fpsDefectGroup.TabIndex = 59;
            this.fpsDefectGroup.SelectionChanged += new FarPoint.Win.Spread.SelectionChangedEventHandler(this.fpsDefectType_SelectionChanged);
            // 
            // fpsDefectGroup_Sheet1
            // 
            this.fpsDefectGroup_Sheet1.Reset();
            fpsDefectGroup_Sheet1.SheetName = "Sheet1";
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
            propertyItem1.Category = "";
            propertyItem1.ColumnName = "GROUP_ID";
            propertyItem1.DisplayName = "Group ID";
            propertyItem1.IsReadOnlyAtInsertMode = true;
            propertyItem1.IsRequiredField = true;
            propertyItem2.ColumnName = "GROUP_NAME";
            propertyItem2.DisplayName = "Group Name";
            propertyItem3.Category = "";
            propertyItem3.ColumnName = "DELETE_FLAG";
            propertyItem3.DisplayName = "Delete Flag";
            propertyItem3.IsReadOnly = true;
            propertyItem3.IsReadOnlyAtInsertMode = true;
            propertyItem3.Visible = false;
            propertyItem4.Category = "Information";
            propertyItem4.ColumnName = "CREATE_TIME";
            propertyItem4.DisplayName = "Create Time";
            propertyItem4.IsReadOnly = true;
            propertyItem4.IsReadOnlyAtInsertMode = true;
            propertyItem5.Category = "Information";
            propertyItem5.ColumnName = "CREATE_USER";
            propertyItem5.DisplayName = "Create User";
            propertyItem5.IsReadOnly = true;
            propertyItem5.IsReadOnlyAtInsertMode = true;
            propertyItem6.Category = "Information";
            propertyItem6.ColumnName = "UPDATE_TIME";
            propertyItem6.DisplayName = "Update Time";
            propertyItem6.IsReadOnly = true;
            propertyItem6.IsReadOnlyAtInsertMode = true;
            propertyItem7.Category = "Information";
            propertyItem7.ColumnName = "UPDATE_USER";
            propertyItem7.DisplayName = "Update User";
            propertyItem7.IsReadOnly = true;
            propertyItem7.IsReadOnlyAtInsertMode = true;
            this.grid.PropertyList.Add(propertyItem1);
            this.grid.PropertyList.Add(propertyItem2);
            this.grid.PropertyList.Add(propertyItem3);
            this.grid.PropertyList.Add(propertyItem4);
            this.grid.PropertyList.Add(propertyItem5);
            this.grid.PropertyList.Add(propertyItem6);
            this.grid.PropertyList.Add(propertyItem7);
            this.grid.Size = new System.Drawing.Size(268, 466);
            this.grid.TabIndex = 0;
            this.grid.CommandButtonClick += new DACrux.Framework.PropertyGrid.DucPropertyGrid.SaveEventHandler(this.grid_CommandButtonClick);
            this.grid.CommandComplete += new DACrux.Framework.PropertyGrid.DucPropertyGrid.SaveEventHandler(this.grid_CommandComplete);
            this.grid.StateChanged += new DACrux.Framework.PropertyGrid.DucPropertyGrid.StatechangedEventHandler(this.grid_StateChanged);
            // 
            // frmDefectTypeGroup_Property
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(896, 527);
            this.Controls.Add(this.fpsDefectGroup);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "frmDefectTypeGroup_Property";
            this.Text = "Define By Defect Type";
            this.Load += new System.EventHandler(this.frmDefectTypeGroup_Property_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fpsDefectGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsDefectGroup_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxTypeColor)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FarPoint.Win.Spread.FpSpread fpsDefectGroup;
        private FarPoint.Win.Spread.SheetView fpsDefectGroup_Sheet1;
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