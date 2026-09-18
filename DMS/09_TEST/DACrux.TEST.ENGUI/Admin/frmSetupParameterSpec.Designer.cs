namespace DACrux.TEST.ENGUI
{
    partial class frmSetupParameterSpec
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
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem5 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem6 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem7 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem8 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem9 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem10 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem11 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem12 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem13 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem14 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem15 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem16 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem17 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem18 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem19 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem20 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem21 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem22 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem23 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem24 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem25 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem26 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem27 = new DACrux.Framework.PropertyGrid.PropertyItem();
            DACrux.Framework.PropertyGrid.PropertyItem propertyItem28 = new DACrux.Framework.PropertyGrid.PropertyItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.duclbProgram = new DACrux.Framework.Controls.DUCListBox();
            this.duclbProduct = new DACrux.Framework.Controls.DUCListBox();
            this.duclbTestArea = new DACrux.Framework.Controls.DUCListBox();
            this.fpsParamSpec = new FarPoint.Win.Spread.FpSpread();
            this.fpsParamSpec_Sheet = new FarPoint.Win.Spread.SheetView();
            this.grid = new DACrux.Framework.PropertyGrid.DucPropertyGrid();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpsParamSpec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsParamSpec_Sheet)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.duclbProgram);
            this.panel1.Controls.Add(this.duclbProduct);
            this.panel1.Controls.Add(this.duclbTestArea);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(955, 157);
            this.panel1.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(852, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 34);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // duclbProgram
            // 
            this.duclbProgram.DataSource = null;
            this.duclbProgram.DisplayMember = "";
            this.duclbProgram.Dock = System.Windows.Forms.DockStyle.Left;
            this.duclbProgram.Location = new System.Drawing.Point(300, 0);
            this.duclbProgram.Name = "duclbProgram";
            this.duclbProgram.SearchText = "";
            this.duclbProgram.SearchTitle = "PROGRAM";
            this.duclbProgram.SelectedIndex = -1;
            this.duclbProgram.SelectedItem = null;
            this.duclbProgram.SelectedValue = null;
            this.duclbProgram.Size = new System.Drawing.Size(150, 157);
            this.duclbProgram.TabIndex = 2;
            this.duclbProgram.ValueMember = "";
            this.duclbProgram.OnSelectedIndexChanged += new System.EventHandler(this.duclbProgram_OnSelectedIndexChanged);
            // 
            // duclbProduct
            // 
            this.duclbProduct.DataSource = null;
            this.duclbProduct.DisplayMember = "";
            this.duclbProduct.Dock = System.Windows.Forms.DockStyle.Left;
            this.duclbProduct.Location = new System.Drawing.Point(150, 0);
            this.duclbProduct.Name = "duclbProduct";
            this.duclbProduct.SearchText = "";
            this.duclbProduct.SearchTitle = "Product";
            this.duclbProduct.SelectedIndex = -1;
            this.duclbProduct.SelectedItem = null;
            this.duclbProduct.SelectedValue = null;
            this.duclbProduct.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.duclbProduct.Size = new System.Drawing.Size(150, 157);
            this.duclbProduct.TabIndex = 1;
            this.duclbProduct.ValueMember = "";
            this.duclbProduct.OnSelectedIndexChanged += new System.EventHandler(this.duclbProduct_OnSelectedIndexChanged);
            // 
            // duclbTestArea
            // 
            this.duclbTestArea.DataSource = null;
            this.duclbTestArea.DisplayMember = "";
            this.duclbTestArea.Dock = System.Windows.Forms.DockStyle.Left;
            this.duclbTestArea.Location = new System.Drawing.Point(0, 0);
            this.duclbTestArea.Name = "duclbTestArea";
            this.duclbTestArea.SearchText = "";
            this.duclbTestArea.SearchTitle = "TESTAREA";
            this.duclbTestArea.SelectedIndex = -1;
            this.duclbTestArea.SelectedItem = null;
            this.duclbTestArea.SelectedValue = null;
            this.duclbTestArea.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.duclbTestArea.Size = new System.Drawing.Size(150, 157);
            this.duclbTestArea.TabIndex = 0;
            this.duclbTestArea.ValueMember = "";
            this.duclbTestArea.OnSelectedIndexChanged += new System.EventHandler(this.duclbTestArea_OnSelectedIndexChanged);
            // 
            // fpsParamSpec
            // 
            this.fpsParamSpec.AccessibleDescription = "";
            this.fpsParamSpec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpsParamSpec.Location = new System.Drawing.Point(0, 163);
            this.fpsParamSpec.Name = "fpsParamSpec";
            this.fpsParamSpec.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpsParamSpec_Sheet});
            this.fpsParamSpec.Size = new System.Drawing.Size(651, 356);
            this.fpsParamSpec.TabIndex = 1;
            // 
            // fpsParamSpec_Sheet
            // 
            this.fpsParamSpec_Sheet.Reset();
            fpsParamSpec_Sheet.SheetName = "Sheet1";
            // 
            // grid
            // 
            this.grid.Dock = System.Windows.Forms.DockStyle.Right;
            this.grid.Location = new System.Drawing.Point(651, 163);
            this.grid.Name = "grid";
            propertyItem1.ColumnName = "FACTORY";
            propertyItem1.DisplayName = "FACTORY";
            propertyItem2.ColumnName = "PROGRAM";
            propertyItem2.DisplayName = "PROGRAM";
            propertyItem2.IsRequiredField = true;
            propertyItem3.ColumnName = "PROGRAM_REV";
            propertyItem3.DisplayName = "Program Revision";
            propertyItem4.ColumnName = "PARAM_INDEX";
            propertyItem4.DisplayName = "Index";
            propertyItem5.ColumnName = "PARAM_NAME";
            propertyItem5.DisplayName = "Name";
            propertyItem6.ColumnName = "USER_PROGRAM_NAME";
            propertyItem6.DisplayName = "User Parameter Name";
            propertyItem7.ColumnName = "PARAM_TYPE";
            propertyItem7.DisplayName = "Type";
            propertyItem8.ColumnName = "PARAM_DESC";
            propertyItem8.DisplayName = "PARAM_DESC";
            propertyItem9.ColumnName = "CREATE_TIME";
            propertyItem9.DisplayName = "Create Time";
            propertyItem10.ColumnName = "CREATE_USER";
            propertyItem10.DisplayName = "Create User";
            propertyItem11.ColumnName = "UPDATE_TIME";
            propertyItem11.DisplayName = "Update Time";
            propertyItem12.ColumnName = "UPDATE_USER";
            propertyItem12.DisplayName = "Update User";
            propertyItem13.ColumnName = "TABLE_NAME";
            propertyItem13.DisplayName = "Table name";
            propertyItem14.ColumnName = "DECIMAL_PLACES";
            propertyItem14.DisplayName = "Decimal Palces";
            propertyItem15.ColumnName = "RUNTIME_DEFINED";
            propertyItem15.DisplayName = "Runtime Define";
            propertyItem16.ColumnName = "USE_FLAG";
            propertyItem16.DisplayName = "Use Flag";
            propertyItem16.InputStyle = DACrux.Framework.PropertyGrid.InputStyle.ComboBox;
            propertyItem17.ColumnName = "UOM";
            propertyItem17.DisplayName = "UOM";
            propertyItem17.InputStyle = DACrux.Framework.PropertyGrid.InputStyle.ComboBox;
            propertyItem18.ColumnName = "LSL";
            propertyItem18.DisplayName = "LSL";
            propertyItem18.IsReadOnly = true;
            propertyItem19.ColumnName = "TARGET";
            propertyItem19.DisplayName = "TARGET";
            propertyItem19.IsReadOnly = true;
            propertyItem20.ColumnName = "USL";
            propertyItem20.DisplayName = "USL";
            propertyItem20.IsReadOnly = true;
            propertyItem21.ColumnName = "LCL";
            propertyItem21.DisplayName = "LCL";
            propertyItem21.IsReadOnly = true;
            propertyItem22.ColumnName = "UCL";
            propertyItem22.DisplayName = "UCL";
            propertyItem22.IsReadOnly = true;
            propertyItem23.ColumnName = "LOL";
            propertyItem23.DisplayName = "LOL";
            propertyItem23.IsReadOnly = true;
            propertyItem24.ColumnName = "UOL";
            propertyItem24.DisplayName = "UOL";
            propertyItem24.IsReadOnly = true;
            propertyItem25.ColumnName = "LTL";
            propertyItem25.DisplayName = "LTL";
            propertyItem25.IsReadOnly = true;
            propertyItem26.ColumnName = "UTL";
            propertyItem26.DisplayName = "UTL";
            propertyItem26.IsReadOnly = true;
            propertyItem27.ColumnName = "TEST_LOW";
            propertyItem27.DisplayName = "TEST_LOW";
            propertyItem27.IsReadOnly = true;
            propertyItem28.ColumnName = "TEST_HIGH";
            propertyItem28.DisplayName = "TEST_HIGH";
            propertyItem28.IsReadOnly = true;
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
            this.grid.PropertyList.Add(propertyItem11);
            this.grid.PropertyList.Add(propertyItem12);
            this.grid.PropertyList.Add(propertyItem13);
            this.grid.PropertyList.Add(propertyItem14);
            this.grid.PropertyList.Add(propertyItem15);
            this.grid.PropertyList.Add(propertyItem16);
            this.grid.PropertyList.Add(propertyItem17);
            this.grid.PropertyList.Add(propertyItem18);
            this.grid.PropertyList.Add(propertyItem19);
            this.grid.PropertyList.Add(propertyItem20);
            this.grid.PropertyList.Add(propertyItem21);
            this.grid.PropertyList.Add(propertyItem22);
            this.grid.PropertyList.Add(propertyItem23);
            this.grid.PropertyList.Add(propertyItem24);
            this.grid.PropertyList.Add(propertyItem25);
            this.grid.PropertyList.Add(propertyItem26);
            this.grid.PropertyList.Add(propertyItem27);
            this.grid.PropertyList.Add(propertyItem28);
            this.grid.Size = new System.Drawing.Size(304, 356);
            this.grid.TabIndex = 2;
            this.grid.VisibleButtonPanel = false;
            this.grid.DropDownComboBox += new DACrux.Framework.PropertyGrid.DucPropertyGrid.DropDownComboBoxEventHandler(this.grid_DropDownComboBox);
            this.grid.CommandButtonClick += new DACrux.Framework.PropertyGrid.DucPropertyGrid.SaveEventHandler(this.grid_CommandButtonClick);
            this.grid.CommandComplete += new DACrux.Framework.PropertyGrid.DucPropertyGrid.SaveEventHandler(this.grid_CommandComplete);
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraSplitter1.Location = new System.Drawing.Point(0, 157);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 157;
            this.ultraSplitter1.Size = new System.Drawing.Size(955, 6);
            this.ultraSplitter1.TabIndex = 3;
            // 
            // frmSetupParameterSpec
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 519);
            this.Controls.Add(this.fpsParamSpec);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.ultraSplitter1);
            this.Controls.Add(this.panel1);
            this.Name = "frmSetupParameterSpec";
            this.Text = "Setup by program parameter spec";
            this.Load += new System.EventHandler(this.frmSetupParameterSpec_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpsParamSpec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsParamSpec_Sheet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Framework.Controls.DUCListBox duclbTestArea;
        private Framework.Controls.DUCListBox duclbProgram;
        private Framework.Controls.DUCListBox duclbProduct;
        private FarPoint.Win.Spread.FpSpread fpsParamSpec;
        private FarPoint.Win.Spread.SheetView fpsParamSpec_Sheet;
        private System.Windows.Forms.Button btnSearch;
        private Framework.PropertyGrid.DucPropertyGrid grid;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;

    }
}