namespace DACrux.SEMDMS.ENGUI
{
    partial class frmDefectTypeDefine
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
            this.fpSpread_Group = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread_Group_Sheet = new FarPoint.Win.Spread.SheetView();
            this.pnlGroup = new System.Windows.Forms.Panel();
            this.pnlGroupDetail = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.txtGroupName = new System.Windows.Forms.TextBox();
            this.lblGroupName = new System.Windows.Forms.Label();
            this.txtGroupId = new System.Windows.Forms.TextBox();
            this.lblGroupId = new System.Windows.Forms.Label();
            this.pnlGroupTitle = new System.Windows.Forms.Panel();
            this.lblGroup = new System.Windows.Forms.Label();
            this.pnlType = new System.Windows.Forms.Panel();
            this.fpSpread_Type = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread_Type_Sheet = new FarPoint.Win.Spread.SheetView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmbGroupList = new System.Windows.Forms.ComboBox();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.txtTypeName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.butDeleteType = new System.Windows.Forms.Button();
            this.butUpdateType = new System.Windows.Forms.Button();
            this.butCreateType = new System.Windows.Forms.Button();
            this.txtTypeId = new System.Windows.Forms.TextBox();
            this.lblTypeNumber = new System.Windows.Forms.Label();
            this.lblTypeGroupId = new System.Windows.Forms.Label();
            this.pnlDefectTitle = new System.Windows.Forms.Panel();
            this.lblDefect = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Group)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Group_Sheet)).BeginInit();
            this.pnlGroup.SuspendLayout();
            this.pnlGroupDetail.SuspendLayout();
            this.pnlGroupTitle.SuspendLayout();
            this.pnlType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Type)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Type_Sheet)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlDefectTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // fpSpread_Group
            // 
            this.fpSpread_Group.AccessibleDescription = "fpSpread_Group, fpSpread_Group, Row 0, Column 0, ";
            this.fpSpread_Group.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpread_Group.Location = new System.Drawing.Point(0, 23);
            this.fpSpread_Group.Name = "fpSpread_Group";
            this.fpSpread_Group.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread_Group_Sheet});
            this.fpSpread_Group.Size = new System.Drawing.Size(240, 379);
            this.fpSpread_Group.TabIndex = 10;
            this.fpSpread_Group.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpSpread_Group_CellClick);
            this.fpSpread_Group.SetActiveViewport(0, -1, -1);
            // 
            // fpSpread_Group_Sheet
            // 
            this.fpSpread_Group_Sheet.Reset();
            fpSpread_Group_Sheet.SheetName = "fpSpread_Group";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpread_Group_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpSpread_Group_Sheet.ColumnCount = 0;
            fpSpread_Group_Sheet.RowCount = 0;
            this.fpSpread_Group_Sheet.ActiveColumnIndex = -1;
            this.fpSpread_Group_Sheet.ActiveRowIndex = -1;
            this.fpSpread_Group_Sheet.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin1", System.Drawing.SystemColors.AppWorkspace, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(223)))), ((int)(((byte)(222))))), FarPoint.Win.Spread.GridLines.Horizontal, System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107))))), System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107))))), System.Drawing.Color.White, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(93)))), ((int)(((byte)(90))))), System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(222))))), System.Drawing.Color.White, true, true, true, true, true, true, false, true, "HeaderDefault", "HeaderDefault", "HeaderDefault", "DataAreaDefault", "HeaderDefault");
            this.fpSpread_Group_Sheet.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Group_Sheet.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Group_Sheet.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Group_Sheet.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Group_Sheet.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpread_Group_Sheet.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Group_Sheet.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Group_Sheet.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Group_Sheet.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Group_Sheet.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Group_Sheet.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpread_Group_Sheet.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Group_Sheet.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Group_Sheet.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Group_Sheet.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Group_Sheet.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Group_Sheet.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpread_Group_Sheet.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Group_Sheet.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Group_Sheet.DataAutoCellTypes = false;
            this.fpSpread_Group_Sheet.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpSpread_Group_Sheet.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpSpread_Group_Sheet.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Group_Sheet.DefaultStyle.Parent = "DataAreaDefault";
            this.fpSpread_Group_Sheet.RowHeader.Columns.Default.Resizable = false;
            this.fpSpread_Group_Sheet.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Group_Sheet.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Group_Sheet.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Group_Sheet.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Group_Sheet.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpread_Group_Sheet.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Group_Sheet.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Group_Sheet.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Group_Sheet.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Group_Sheet.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Group_Sheet.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpread_Group_Sheet.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Group_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // pnlGroup
            // 
            this.pnlGroup.Controls.Add(this.fpSpread_Group);
            this.pnlGroup.Controls.Add(this.pnlGroupDetail);
            this.pnlGroup.Controls.Add(this.pnlGroupTitle);
            this.pnlGroup.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlGroup.Location = new System.Drawing.Point(0, 0);
            this.pnlGroup.Name = "pnlGroup";
            this.pnlGroup.Size = new System.Drawing.Size(240, 527);
            this.pnlGroup.TabIndex = 11;
            // 
            // pnlGroupDetail
            // 
            this.pnlGroupDetail.Controls.Add(this.btnDelete);
            this.pnlGroupDetail.Controls.Add(this.btnUpdate);
            this.pnlGroupDetail.Controls.Add(this.btnCreate);
            this.pnlGroupDetail.Controls.Add(this.txtGroupName);
            this.pnlGroupDetail.Controls.Add(this.lblGroupName);
            this.pnlGroupDetail.Controls.Add(this.txtGroupId);
            this.pnlGroupDetail.Controls.Add(this.lblGroupId);
            this.pnlGroupDetail.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlGroupDetail.Location = new System.Drawing.Point(0, 402);
            this.pnlGroupDetail.Name = "pnlGroupDetail";
            this.pnlGroupDetail.Size = new System.Drawing.Size(240, 125);
            this.pnlGroupDetail.TabIndex = 12;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(163, 97);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(70, 23);
            this.btnDelete.TabIndex = 16;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(85, 97);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(70, 23);
            this.btnUpdate.TabIndex = 15;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(7, 97);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(70, 23);
            this.btnCreate.TabIndex = 0;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // txtGroupName
            // 
            this.txtGroupName.Location = new System.Drawing.Point(6, 70);
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(228, 21);
            this.txtGroupName.TabIndex = 14;
            // 
            // lblGroupName
            // 
            this.lblGroupName.AutoSize = true;
            this.lblGroupName.Location = new System.Drawing.Point(3, 49);
            this.lblGroupName.Name = "lblGroupName";
            this.lblGroupName.Size = new System.Drawing.Size(88, 15);
            this.lblGroupName.TabIndex = 13;
            this.lblGroupName.Text = "Group Name";
            // 
            // txtGroupId
            // 
            this.txtGroupId.Location = new System.Drawing.Point(6, 26);
            this.txtGroupId.Name = "txtGroupId";
            this.txtGroupId.Size = new System.Drawing.Size(228, 21);
            this.txtGroupId.TabIndex = 12;
            // 
            // lblGroupId
            // 
            this.lblGroupId.AutoSize = true;
            this.lblGroupId.Location = new System.Drawing.Point(3, 6);
            this.lblGroupId.Name = "lblGroupId";
            this.lblGroupId.Size = new System.Drawing.Size(155, 15);
            this.lblGroupId.TabIndex = 11;
            this.lblGroupId.Text = "Group ID(Only number)";
            // 
            // pnlGroupTitle
            // 
            this.pnlGroupTitle.Controls.Add(this.lblGroup);
            this.pnlGroupTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlGroupTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlGroupTitle.Name = "pnlGroupTitle";
            this.pnlGroupTitle.Size = new System.Drawing.Size(240, 23);
            this.pnlGroupTitle.TabIndex = 11;
            // 
            // lblGroup
            // 
            this.lblGroup.AutoSize = true;
            this.lblGroup.Location = new System.Drawing.Point(3, 4);
            this.lblGroup.Name = "lblGroup";
            this.lblGroup.Size = new System.Drawing.Size(73, 15);
            this.lblGroup.TabIndex = 11;
            this.lblGroup.Text = "Group List";
            // 
            // pnlType
            // 
            this.pnlType.Controls.Add(this.fpSpread_Type);
            this.pnlType.Controls.Add(this.panel1);
            this.pnlType.Controls.Add(this.pnlDefectTitle);
            this.pnlType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlType.Location = new System.Drawing.Point(240, 0);
            this.pnlType.Name = "pnlType";
            this.pnlType.Size = new System.Drawing.Size(528, 527);
            this.pnlType.TabIndex = 12;
            // 
            // fpSpread_Type
            // 
            this.fpSpread_Type.AccessibleDescription = "fpSpread1, fpSpread_Type, Row 0, Column 0, ";
            this.fpSpread_Type.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpread_Type.Location = new System.Drawing.Point(0, 23);
            this.fpSpread_Type.Name = "fpSpread_Type";
            this.fpSpread_Type.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread_Type_Sheet});
            this.fpSpread_Type.Size = new System.Drawing.Size(528, 379);
            this.fpSpread_Type.TabIndex = 13;
            this.fpSpread_Type.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpSpread_Type_CellClick);
            this.fpSpread_Type.SetActiveViewport(0, -1, -1);
            // 
            // fpSpread_Type_Sheet
            // 
            this.fpSpread_Type_Sheet.Reset();
            fpSpread_Type_Sheet.SheetName = "fpSpread_Type";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpread_Type_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpSpread_Type_Sheet.ColumnCount = 0;
            fpSpread_Type_Sheet.RowCount = 0;
            this.fpSpread_Type_Sheet.ActiveColumnIndex = -1;
            this.fpSpread_Type_Sheet.ActiveRowIndex = -1;
            this.fpSpread_Type_Sheet.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin1", System.Drawing.SystemColors.AppWorkspace, System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(223)))), ((int)(((byte)(222))))), FarPoint.Win.Spread.GridLines.Horizontal, System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107))))), System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107))))), System.Drawing.Color.White, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(93)))), ((int)(((byte)(90))))), System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(222))))), System.Drawing.Color.White, true, true, true, true, true, true, false, true, "HeaderDefault", "HeaderDefault", "HeaderDefault", "DataAreaDefault", "HeaderDefault");
            this.fpSpread_Type_Sheet.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Type_Sheet.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Type_Sheet.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Type_Sheet.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Type_Sheet.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpread_Type_Sheet.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Type_Sheet.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Type_Sheet.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Type_Sheet.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Type_Sheet.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Type_Sheet.ColumnFooterSheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpread_Type_Sheet.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Type_Sheet.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Type_Sheet.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Type_Sheet.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Type_Sheet.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Type_Sheet.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpread_Type_Sheet.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Type_Sheet.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Type_Sheet.DataAutoCellTypes = false;
            this.fpSpread_Type_Sheet.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpSpread_Type_Sheet.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpSpread_Type_Sheet.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Type_Sheet.DefaultStyle.Parent = "DataAreaDefault";
            this.fpSpread_Type_Sheet.RowHeader.Columns.Default.Resizable = false;
            this.fpSpread_Type_Sheet.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Type_Sheet.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Type_Sheet.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Type_Sheet.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Type_Sheet.RowHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpSpread_Type_Sheet.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Type_Sheet.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Type_Sheet.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpSpread_Type_Sheet.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpSpread_Type_Sheet.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpSpread_Type_Sheet.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpSpread_Type_Sheet.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpSpread_Type_Sheet.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmbGroupList);
            this.panel1.Controls.Add(this.txtDesc);
            this.panel1.Controls.Add(this.lblDesc);
            this.panel1.Controls.Add(this.txtTypeName);
            this.panel1.Controls.Add(this.lblName);
            this.panel1.Controls.Add(this.butDeleteType);
            this.panel1.Controls.Add(this.butUpdateType);
            this.panel1.Controls.Add(this.butCreateType);
            this.panel1.Controls.Add(this.txtTypeId);
            this.panel1.Controls.Add(this.lblTypeNumber);
            this.panel1.Controls.Add(this.lblTypeGroupId);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 402);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(528, 125);
            this.panel1.TabIndex = 17;
            // 
            // cmbGroupList
            // 
            this.cmbGroupList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGroupList.FormattingEnabled = true;
            this.cmbGroupList.Location = new System.Drawing.Point(199, 10);
            this.cmbGroupList.Name = "cmbGroupList";
            this.cmbGroupList.Size = new System.Drawing.Size(228, 20);
            this.cmbGroupList.TabIndex = 21;
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(199, 94);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(228, 21);
            this.txtDesc.TabIndex = 20;
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.Location = new System.Drawing.Point(3, 97);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(69, 15);
            this.lblDesc.TabIndex = 19;
            this.lblDesc.Text = "Description";
            // 
            // txtTypeName
            // 
            this.txtTypeName.Location = new System.Drawing.Point(199, 65);
            this.txtTypeName.Name = "txtTypeName";
            this.txtTypeName.Size = new System.Drawing.Size(228, 21);
            this.txtTypeName.TabIndex = 18;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(3, 68);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(79, 15);
            this.lblName.TabIndex = 17;
            this.lblName.Text = "Type Name";
            // 
            // butDeleteType
            // 
            this.butDeleteType.Location = new System.Drawing.Point(443, 88);
            this.butDeleteType.Name = "butDeleteType";
            this.butDeleteType.Size = new System.Drawing.Size(70, 23);
            this.butDeleteType.TabIndex = 16;
            this.butDeleteType.Text = "Delete";
            this.butDeleteType.UseVisualStyleBackColor = true;
            this.butDeleteType.Click += new System.EventHandler(this.butDeleteType_Click);
            // 
            // butUpdateType
            // 
            this.butUpdateType.Location = new System.Drawing.Point(443, 57);
            this.butUpdateType.Name = "butUpdateType";
            this.butUpdateType.Size = new System.Drawing.Size(70, 23);
            this.butUpdateType.TabIndex = 15;
            this.butUpdateType.Text = "Update";
            this.butUpdateType.UseVisualStyleBackColor = true;
            this.butUpdateType.Click += new System.EventHandler(this.butUpdateType_Click);
            // 
            // butCreateType
            // 
            this.butCreateType.Location = new System.Drawing.Point(443, 25);
            this.butCreateType.Name = "butCreateType";
            this.butCreateType.Size = new System.Drawing.Size(70, 23);
            this.butCreateType.TabIndex = 0;
            this.butCreateType.Text = "Create";
            this.butCreateType.UseVisualStyleBackColor = true;
            this.butCreateType.Visible = false;
            this.butCreateType.Click += new System.EventHandler(this.butCreateType_Click);
            // 
            // txtTypeId
            // 
            this.txtTypeId.Location = new System.Drawing.Point(199, 36);
            this.txtTypeId.Name = "txtTypeId";
            this.txtTypeId.Size = new System.Drawing.Size(228, 21);
            this.txtTypeId.TabIndex = 14;
            // 
            // lblTypeNumber
            // 
            this.lblTypeNumber.AutoSize = true;
            this.lblTypeNumber.Location = new System.Drawing.Point(3, 39);
            this.lblTypeNumber.Name = "lblTypeNumber";
            this.lblTypeNumber.Size = new System.Drawing.Size(189, 15);
            this.lblTypeNumber.TabIndex = 13;
            this.lblTypeNumber.Text = "Type Number (Only Number)";
            // 
            // lblTypeGroupId
            // 
            this.lblTypeGroupId.AutoSize = true;
            this.lblTypeGroupId.Location = new System.Drawing.Point(3, 10);
            this.lblTypeGroupId.Name = "lblTypeGroupId";
            this.lblTypeGroupId.Size = new System.Drawing.Size(64, 15);
            this.lblTypeGroupId.TabIndex = 11;
            this.lblTypeGroupId.Text = "Group ID";
            // 
            // pnlDefectTitle
            // 
            this.pnlDefectTitle.Controls.Add(this.lblDefect);
            this.pnlDefectTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDefectTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlDefectTitle.Name = "pnlDefectTitle";
            this.pnlDefectTitle.Size = new System.Drawing.Size(528, 23);
            this.pnlDefectTitle.TabIndex = 12;
            // 
            // lblDefect
            // 
            this.lblDefect.AutoSize = true;
            this.lblDefect.Location = new System.Drawing.Point(3, 4);
            this.lblDefect.Name = "lblDefect";
            this.lblDefect.Size = new System.Drawing.Size(75, 15);
            this.lblDefect.TabIndex = 11;
            this.lblDefect.Text = "Defect List";
            // 
            // frmDefectTypeDefine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 527);
            this.Controls.Add(this.pnlType);
            this.Controls.Add(this.pnlGroup);
            this.Name = "frmDefectTypeDefine";
            this.Text = "Defect Type Define";
            this.Load += new System.EventHandler(this.frmDefectTypeDefine_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Group)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Group_Sheet)).EndInit();
            this.pnlGroup.ResumeLayout(false);
            this.pnlGroupDetail.ResumeLayout(false);
            this.pnlGroupDetail.PerformLayout();
            this.pnlGroupTitle.ResumeLayout(false);
            this.pnlGroupTitle.PerformLayout();
            this.pnlType.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Type)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread_Type_Sheet)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlDefectTitle.ResumeLayout(false);
            this.pnlDefectTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private FarPoint.Win.Spread.FpSpread fpSpread_Group;
        private FarPoint.Win.Spread.SheetView fpSpread_Group_Sheet;
        private System.Windows.Forms.Panel pnlGroup;
        private System.Windows.Forms.Panel pnlGroupDetail;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.TextBox txtGroupName;
        private System.Windows.Forms.Label lblGroupName;
        private System.Windows.Forms.TextBox txtGroupId;
        private System.Windows.Forms.Label lblGroupId;
        private System.Windows.Forms.Panel pnlGroupTitle;
        private System.Windows.Forms.Label lblGroup;
        private System.Windows.Forms.Panel pnlType;
        private FarPoint.Win.Spread.FpSpread fpSpread_Type;
        private FarPoint.Win.Spread.SheetView fpSpread_Type_Sheet;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button butDeleteType;
        private System.Windows.Forms.Button butUpdateType;
        private System.Windows.Forms.Button butCreateType;
        private System.Windows.Forms.TextBox txtTypeId;
        private System.Windows.Forms.Label lblTypeNumber;
        private System.Windows.Forms.Label lblTypeGroupId;
        private System.Windows.Forms.Panel pnlDefectTitle;
        private System.Windows.Forms.Label lblDefect;
        private System.Windows.Forms.TextBox txtTypeName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.ComboBox cmbGroupList;

    }
}