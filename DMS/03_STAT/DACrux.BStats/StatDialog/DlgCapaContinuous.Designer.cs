namespace DACrux.BStats.StatDialog
{
    partial class DlgCapaContinuous
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgCapaContinuous));
            this.pnlBackground = new System.Windows.Forms.Panel();
            this.btnLeft_SubgroupsAcrossRows = new System.Windows.Forms.Button();
            this.btnRight_SubgroupsAcrossRows = new System.Windows.Forms.Button();
            this.txtBetweenSubgroups = new System.Windows.Forms.TextBox();
            this.txtWithinSubgroup = new System.Windows.Forms.TextBox();
            this.txtHistoricalMean = new System.Windows.Forms.TextBox();
            this.txtUpperSpec = new System.Windows.Forms.TextBox();
            this.txtLowerSpec = new System.Windows.Forms.TextBox();
            this.lblBetweenOptional = new System.Windows.Forms.Label();
            this.lblWithinOptional = new System.Windows.Forms.Label();
            this.lblHistoricalMeanOptional = new System.Windows.Forms.Label();
            this.lblBetweenSubgroups = new System.Windows.Forms.Label();
            this.lblWithinSubgroup = new System.Windows.Forms.Label();
            this.lblHistoricalStandardDeviations = new System.Windows.Forms.Label();
            this.lblHistoricalMean = new System.Windows.Forms.Label();
            this.lblUpperSpec = new System.Windows.Forms.Label();
            this.lblLowerSpec = new System.Windows.Forms.Label();
            this.lvSubgroupAcrossRows = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rdoSubgroupAcrossRows = new System.Windows.Forms.RadioButton();
            this.grpSubgroupSize = new System.Windows.Forms.GroupBox();
            this.nudConstant = new System.Windows.Forms.NumericUpDown();
            this.lblIDcolumn = new System.Windows.Forms.Label();
            this.chkConstant = new System.Windows.Forms.CheckBox();
            this.btnLeft_SubgroupColumn = new System.Windows.Forms.Button();
            this.lvSubgroupColumn = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRight_SubgroupColumn = new System.Windows.Forms.Button();
            this.rdoSigleColumn = new System.Windows.Forms.RadioButton();
            this.btnEstimate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnOptions = new System.Windows.Forms.Button();
            this.btnLeft_SingleVar = new System.Windows.Forms.Button();
            this.btnRight_SingleVar = new System.Windows.Forms.Button();
            this.lblColumns = new System.Windows.Forms.Label();
            this.lvColumns = new System.Windows.Forms.ListView();
            this.colType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblDataArrangedAs = new System.Windows.Forms.Label();
            this.lvSingleVariables = new System.Windows.Forms.ListView();
            this.colType_Ser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colID_Ser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName_Ser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imlColumnType = new System.Windows.Forms.ImageList(this.components);
            this.pnlBackground.SuspendLayout();
            this.grpSubgroupSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudConstant)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBackground
            // 
            this.pnlBackground.Controls.Add(this.btnLeft_SubgroupsAcrossRows);
            this.pnlBackground.Controls.Add(this.btnRight_SubgroupsAcrossRows);
            this.pnlBackground.Controls.Add(this.txtBetweenSubgroups);
            this.pnlBackground.Controls.Add(this.txtWithinSubgroup);
            this.pnlBackground.Controls.Add(this.txtHistoricalMean);
            this.pnlBackground.Controls.Add(this.txtUpperSpec);
            this.pnlBackground.Controls.Add(this.txtLowerSpec);
            this.pnlBackground.Controls.Add(this.lblBetweenOptional);
            this.pnlBackground.Controls.Add(this.lblWithinOptional);
            this.pnlBackground.Controls.Add(this.lblHistoricalMeanOptional);
            this.pnlBackground.Controls.Add(this.lblBetweenSubgroups);
            this.pnlBackground.Controls.Add(this.lblWithinSubgroup);
            this.pnlBackground.Controls.Add(this.lblHistoricalStandardDeviations);
            this.pnlBackground.Controls.Add(this.lblHistoricalMean);
            this.pnlBackground.Controls.Add(this.lblUpperSpec);
            this.pnlBackground.Controls.Add(this.lblLowerSpec);
            this.pnlBackground.Controls.Add(this.lvSubgroupAcrossRows);
            this.pnlBackground.Controls.Add(this.rdoSubgroupAcrossRows);
            this.pnlBackground.Controls.Add(this.grpSubgroupSize);
            this.pnlBackground.Controls.Add(this.rdoSigleColumn);
            this.pnlBackground.Controls.Add(this.btnEstimate);
            this.pnlBackground.Controls.Add(this.btnCancel);
            this.pnlBackground.Controls.Add(this.btnOk);
            this.pnlBackground.Controls.Add(this.btnOptions);
            this.pnlBackground.Controls.Add(this.btnLeft_SingleVar);
            this.pnlBackground.Controls.Add(this.btnRight_SingleVar);
            this.pnlBackground.Controls.Add(this.lblColumns);
            this.pnlBackground.Controls.Add(this.lvColumns);
            this.pnlBackground.Controls.Add(this.lblDataArrangedAs);
            this.pnlBackground.Controls.Add(this.lvSingleVariables);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Size = new System.Drawing.Size(565, 335);
            this.pnlBackground.TabIndex = 1;
            // 
            // btnLeft_SubgroupsAcrossRows
            // 
            this.btnLeft_SubgroupsAcrossRows.Enabled = false;
            this.btnLeft_SubgroupsAcrossRows.Location = new System.Drawing.Point(182, 199);
            this.btnLeft_SubgroupsAcrossRows.Name = "btnLeft_SubgroupsAcrossRows";
            this.btnLeft_SubgroupsAcrossRows.Size = new System.Drawing.Size(20, 19);
            this.btnLeft_SubgroupsAcrossRows.TabIndex = 61;
            this.btnLeft_SubgroupsAcrossRows.Text = "◄";
            this.btnLeft_SubgroupsAcrossRows.UseCompatibleTextRendering = true;
            this.btnLeft_SubgroupsAcrossRows.UseVisualStyleBackColor = true;
            // 
            // btnRight_SubgroupsAcrossRows
            // 
            this.btnRight_SubgroupsAcrossRows.Enabled = false;
            this.btnRight_SubgroupsAcrossRows.Location = new System.Drawing.Point(182, 180);
            this.btnRight_SubgroupsAcrossRows.Name = "btnRight_SubgroupsAcrossRows";
            this.btnRight_SubgroupsAcrossRows.Size = new System.Drawing.Size(20, 19);
            this.btnRight_SubgroupsAcrossRows.TabIndex = 60;
            this.btnRight_SubgroupsAcrossRows.Text = "►";
            this.btnRight_SubgroupsAcrossRows.UseCompatibleTextRendering = true;
            this.btnRight_SubgroupsAcrossRows.UseVisualStyleBackColor = true;
            // 
            // txtBetweenSubgroups
            // 
            this.txtBetweenSubgroups.Location = new System.Drawing.Point(359, 377);
            this.txtBetweenSubgroups.Name = "txtBetweenSubgroups";
            this.txtBetweenSubgroups.Size = new System.Drawing.Size(105, 21);
            this.txtBetweenSubgroups.TabIndex = 58;
            this.txtBetweenSubgroups.Visible = false;
            // 
            // txtWithinSubgroup
            // 
            this.txtWithinSubgroup.Location = new System.Drawing.Point(359, 352);
            this.txtWithinSubgroup.Name = "txtWithinSubgroup";
            this.txtWithinSubgroup.Size = new System.Drawing.Size(104, 21);
            this.txtWithinSubgroup.TabIndex = 55;
            this.txtWithinSubgroup.Visible = false;
            // 
            // txtHistoricalMean
            // 
            this.txtHistoricalMean.Location = new System.Drawing.Point(359, 299);
            this.txtHistoricalMean.Name = "txtHistoricalMean";
            this.txtHistoricalMean.Size = new System.Drawing.Size(106, 21);
            this.txtHistoricalMean.TabIndex = 51;
            this.txtHistoricalMean.Visible = false;
            // 
            // txtUpperSpec
            // 
            this.txtUpperSpec.Location = new System.Drawing.Point(359, 275);
            this.txtUpperSpec.Name = "txtUpperSpec";
            this.txtUpperSpec.Size = new System.Drawing.Size(106, 21);
            this.txtUpperSpec.TabIndex = 49;
            // 
            // txtLowerSpec
            // 
            this.txtLowerSpec.Location = new System.Drawing.Point(359, 251);
            this.txtLowerSpec.Name = "txtLowerSpec";
            this.txtLowerSpec.Size = new System.Drawing.Size(105, 21);
            this.txtLowerSpec.TabIndex = 47;
            // 
            // lblBetweenOptional
            // 
            this.lblBetweenOptional.AutoSize = true;
            this.lblBetweenOptional.Location = new System.Drawing.Point(466, 378);
            this.lblBetweenOptional.Name = "lblBetweenOptional";
            this.lblBetweenOptional.Size = new System.Drawing.Size(53, 13);
            this.lblBetweenOptional.TabIndex = 59;
            this.lblBetweenOptional.Text = "(optional)";
            this.lblBetweenOptional.Visible = false;
            // 
            // lblWithinOptional
            // 
            this.lblWithinOptional.AutoSize = true;
            this.lblWithinOptional.Location = new System.Drawing.Point(466, 355);
            this.lblWithinOptional.Name = "lblWithinOptional";
            this.lblWithinOptional.Size = new System.Drawing.Size(53, 13);
            this.lblWithinOptional.TabIndex = 56;
            this.lblWithinOptional.Text = "(optional)";
            this.lblWithinOptional.Visible = false;
            // 
            // lblHistoricalMeanOptional
            // 
            this.lblHistoricalMeanOptional.AutoSize = true;
            this.lblHistoricalMeanOptional.Location = new System.Drawing.Point(467, 302);
            this.lblHistoricalMeanOptional.Name = "lblHistoricalMeanOptional";
            this.lblHistoricalMeanOptional.Size = new System.Drawing.Size(53, 13);
            this.lblHistoricalMeanOptional.TabIndex = 52;
            this.lblHistoricalMeanOptional.Text = "(optional)";
            this.lblHistoricalMeanOptional.Visible = false;
            // 
            // lblBetweenSubgroups
            // 
            this.lblBetweenSubgroups.AutoSize = true;
            this.lblBetweenSubgroups.Location = new System.Drawing.Point(198, 377);
            this.lblBetweenSubgroups.Name = "lblBetweenSubgroups";
            this.lblBetweenSubgroups.Size = new System.Drawing.Size(102, 13);
            this.lblBetweenSubgroups.TabIndex = 57;
            this.lblBetweenSubgroups.Text = "Betwee&n subgroups";
            this.lblBetweenSubgroups.Visible = false;
            // 
            // lblWithinSubgroup
            // 
            this.lblWithinSubgroup.AutoSize = true;
            this.lblWithinSubgroup.Location = new System.Drawing.Point(198, 355);
            this.lblWithinSubgroup.Name = "lblWithinSubgroup";
            this.lblWithinSubgroup.Size = new System.Drawing.Size(85, 13);
            this.lblWithinSubgroup.TabIndex = 54;
            this.lblWithinSubgroup.Text = "&Within subgroup";
            this.lblWithinSubgroup.Visible = false;
            // 
            // lblHistoricalStandardDeviations
            // 
            this.lblHistoricalStandardDeviations.AutoSize = true;
            this.lblHistoricalStandardDeviations.Location = new System.Drawing.Point(184, 332);
            this.lblHistoricalStandardDeviations.Name = "lblHistoricalStandardDeviations";
            this.lblHistoricalStandardDeviations.Size = new System.Drawing.Size(148, 13);
            this.lblHistoricalStandardDeviations.TabIndex = 53;
            this.lblHistoricalStandardDeviations.Text = "Historical standard deviations";
            this.lblHistoricalStandardDeviations.Visible = false;
            // 
            // lblHistoricalMean
            // 
            this.lblHistoricalMean.AutoSize = true;
            this.lblHistoricalMean.Location = new System.Drawing.Point(184, 302);
            this.lblHistoricalMean.Name = "lblHistoricalMean";
            this.lblHistoricalMean.Size = new System.Drawing.Size(79, 13);
            this.lblHistoricalMean.TabIndex = 50;
            this.lblHistoricalMean.Text = "Historical &mean";
            this.lblHistoricalMean.Visible = false;
            // 
            // lblUpperSpec
            // 
            this.lblUpperSpec.AutoSize = true;
            this.lblUpperSpec.Location = new System.Drawing.Point(184, 278);
            this.lblUpperSpec.Name = "lblUpperSpec";
            this.lblUpperSpec.Size = new System.Drawing.Size(61, 13);
            this.lblUpperSpec.TabIndex = 48;
            this.lblUpperSpec.Text = "&Upper spec";
            // 
            // lblLowerSpec
            // 
            this.lblLowerSpec.AutoSize = true;
            this.lblLowerSpec.Location = new System.Drawing.Point(184, 254);
            this.lblLowerSpec.Name = "lblLowerSpec";
            this.lblLowerSpec.Size = new System.Drawing.Size(61, 13);
            this.lblLowerSpec.TabIndex = 46;
            this.lblLowerSpec.Text = "&Lower spec";
            // 
            // lvSubgroupAcrossRows
            // 
            this.lvSubgroupAcrossRows.AllowDrop = true;
            this.lvSubgroupAcrossRows.BackColor = System.Drawing.Color.Lavender;
            this.lvSubgroupAcrossRows.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvSubgroupAcrossRows.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.lvSubgroupAcrossRows.Enabled = false;
            this.lvSubgroupAcrossRows.FullRowSelect = true;
            this.lvSubgroupAcrossRows.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvSubgroupAcrossRows.Location = new System.Drawing.Point(208, 180);
            this.lvSubgroupAcrossRows.Name = "lvSubgroupAcrossRows";
            this.lvSubgroupAcrossRows.Size = new System.Drawing.Size(256, 58);
            this.lvSubgroupAcrossRows.TabIndex = 45;
            this.lvSubgroupAcrossRows.UseCompatibleStateImageBehavior = false;
            this.lvSubgroupAcrossRows.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Width = 20;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Width = 50;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Width = 70;
            // 
            // rdoSubgroupAcrossRows
            // 
            this.rdoSubgroupAcrossRows.AutoSize = true;
            this.rdoSubgroupAcrossRows.Location = new System.Drawing.Point(186, 157);
            this.rdoSubgroupAcrossRows.Name = "rdoSubgroupAcrossRows";
            this.rdoSubgroupAcrossRows.Size = new System.Drawing.Size(149, 17);
            this.rdoSubgroupAcrossRows.TabIndex = 44;
            this.rdoSubgroupAcrossRows.Text = "Su&bgroups across rows of";
            this.rdoSubgroupAcrossRows.UseVisualStyleBackColor = true;
            // 
            // grpSubgroupSize
            // 
            this.grpSubgroupSize.Controls.Add(this.nudConstant);
            this.grpSubgroupSize.Controls.Add(this.lblIDcolumn);
            this.grpSubgroupSize.Controls.Add(this.chkConstant);
            this.grpSubgroupSize.Controls.Add(this.btnLeft_SubgroupColumn);
            this.grpSubgroupSize.Controls.Add(this.lvSubgroupColumn);
            this.grpSubgroupSize.Controls.Add(this.btnRight_SubgroupColumn);
            this.grpSubgroupSize.Location = new System.Drawing.Point(208, 68);
            this.grpSubgroupSize.Name = "grpSubgroupSize";
            this.grpSubgroupSize.Size = new System.Drawing.Size(262, 72);
            this.grpSubgroupSize.TabIndex = 43;
            this.grpSubgroupSize.TabStop = false;
            this.grpSubgroupSize.Text = "Subgroup size";
            // 
            // nudConstant
            // 
            this.nudConstant.Location = new System.Drawing.Point(167, 47);
            this.nudConstant.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudConstant.Name = "nudConstant";
            this.nudConstant.Size = new System.Drawing.Size(68, 21);
            this.nudConstant.TabIndex = 43;
            this.nudConstant.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblIDcolumn
            // 
            this.lblIDcolumn.AutoSize = true;
            this.lblIDcolumn.Location = new System.Drawing.Point(13, 21);
            this.lblIDcolumn.Name = "lblIDcolumn";
            this.lblIDcolumn.Size = new System.Drawing.Size(54, 13);
            this.lblIDcolumn.TabIndex = 41;
            this.lblIDcolumn.Text = "ID column";
            // 
            // chkConstant
            // 
            this.chkConstant.AutoSize = true;
            this.chkConstant.Checked = true;
            this.chkConstant.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkConstant.Location = new System.Drawing.Point(63, 49);
            this.chkConstant.Name = "chkConstant";
            this.chkConstant.Size = new System.Drawing.Size(109, 16);
            this.chkConstant.TabIndex = 42;
            this.chkConstant.Text = "U&se a constant";
            this.chkConstant.UseVisualStyleBackColor = true;
            // 
            // btnLeft_SubgroupColumn
            // 
            this.btnLeft_SubgroupColumn.Location = new System.Drawing.Point(80, 20);
            this.btnLeft_SubgroupColumn.Name = "btnLeft_SubgroupColumn";
            this.btnLeft_SubgroupColumn.Size = new System.Drawing.Size(20, 19);
            this.btnLeft_SubgroupColumn.TabIndex = 34;
            this.btnLeft_SubgroupColumn.Text = "◄";
            this.btnLeft_SubgroupColumn.UseCompatibleTextRendering = true;
            this.btnLeft_SubgroupColumn.UseVisualStyleBackColor = true;
            // 
            // lvSubgroupColumn
            // 
            this.lvSubgroupColumn.AllowDrop = true;
            this.lvSubgroupColumn.BackColor = System.Drawing.Color.Lavender;
            this.lvSubgroupColumn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvSubgroupColumn.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvSubgroupColumn.FullRowSelect = true;
            this.lvSubgroupColumn.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvSubgroupColumn.Location = new System.Drawing.Point(126, 20);
            this.lvSubgroupColumn.MultiSelect = false;
            this.lvSubgroupColumn.Name = "lvSubgroupColumn";
            this.lvSubgroupColumn.Scrollable = false;
            this.lvSubgroupColumn.Size = new System.Drawing.Size(130, 19);
            this.lvSubgroupColumn.TabIndex = 31;
            this.lvSubgroupColumn.UseCompatibleStateImageBehavior = false;
            this.lvSubgroupColumn.View = System.Windows.Forms.View.Details;
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
            // btnRight_SubgroupColumn
            // 
            this.btnRight_SubgroupColumn.Location = new System.Drawing.Point(100, 20);
            this.btnRight_SubgroupColumn.Name = "btnRight_SubgroupColumn";
            this.btnRight_SubgroupColumn.Size = new System.Drawing.Size(20, 19);
            this.btnRight_SubgroupColumn.TabIndex = 33;
            this.btnRight_SubgroupColumn.Text = "►";
            this.btnRight_SubgroupColumn.UseCompatibleTextRendering = true;
            this.btnRight_SubgroupColumn.UseVisualStyleBackColor = true;
            // 
            // rdoSigleColumn
            // 
            this.rdoSigleColumn.AutoSize = true;
            this.rdoSigleColumn.Checked = true;
            this.rdoSigleColumn.Location = new System.Drawing.Point(186, 45);
            this.rdoSigleColumn.Name = "rdoSigleColumn";
            this.rdoSigleColumn.Size = new System.Drawing.Size(89, 17);
            this.rdoSigleColumn.TabIndex = 40;
            this.rdoSigleColumn.TabStop = true;
            this.rdoSigleColumn.Text = "Single &column";
            this.rdoSigleColumn.UseVisualStyleBackColor = true;
            // 
            // btnEstimate
            // 
            this.btnEstimate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEstimate.Location = new System.Drawing.Point(484, 41);
            this.btnEstimate.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnEstimate.Name = "btnEstimate";
            this.btnEstimate.Size = new System.Drawing.Size(69, 21);
            this.btnEstimate.TabIndex = 39;
            this.btnEstimate.Text = "&Estimate";
            this.btnEstimate.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(478, 301);
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
            this.btnOk.Location = new System.Drawing.Point(401, 301);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(69, 21);
            this.btnOk.TabIndex = 37;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnOptions
            // 
            this.btnOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOptions.Location = new System.Drawing.Point(484, 72);
            this.btnOptions.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Size = new System.Drawing.Size(69, 21);
            this.btnOptions.TabIndex = 36;
            this.btnOptions.Text = "O&ptions";
            this.btnOptions.UseVisualStyleBackColor = true;
            // 
            // btnLeft_SingleVar
            // 
            this.btnLeft_SingleVar.Location = new System.Drawing.Point(288, 43);
            this.btnLeft_SingleVar.Name = "btnLeft_SingleVar";
            this.btnLeft_SingleVar.Size = new System.Drawing.Size(20, 19);
            this.btnLeft_SingleVar.TabIndex = 30;
            this.btnLeft_SingleVar.Text = "◄";
            this.btnLeft_SingleVar.UseCompatibleTextRendering = true;
            this.btnLeft_SingleVar.UseVisualStyleBackColor = true;
            // 
            // btnRight_SingleVar
            // 
            this.btnRight_SingleVar.Location = new System.Drawing.Point(308, 43);
            this.btnRight_SingleVar.Name = "btnRight_SingleVar";
            this.btnRight_SingleVar.Size = new System.Drawing.Size(20, 19);
            this.btnRight_SingleVar.TabIndex = 29;
            this.btnRight_SingleVar.Text = "►";
            this.btnRight_SingleVar.UseCompatibleTextRendering = true;
            this.btnRight_SingleVar.UseVisualStyleBackColor = true;
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
            this.lvColumns.Size = new System.Drawing.Size(145, 260);
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
            // lblDataArrangedAs
            // 
            this.lblDataArrangedAs.AutoSize = true;
            this.lblDataArrangedAs.Location = new System.Drawing.Point(184, 20);
            this.lblDataArrangedAs.Name = "lblDataArrangedAs";
            this.lblDataArrangedAs.Size = new System.Drawing.Size(110, 13);
            this.lblDataArrangedAs.TabIndex = 27;
            this.lblDataArrangedAs.Text = "Data are arranged as";
            // 
            // lvSingleVariables
            // 
            this.lvSingleVariables.AllowDrop = true;
            this.lvSingleVariables.BackColor = System.Drawing.Color.Lavender;
            this.lvSingleVariables.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvSingleVariables.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colType_Ser,
            this.colID_Ser,
            this.colName_Ser});
            this.lvSingleVariables.FullRowSelect = true;
            this.lvSingleVariables.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvSingleVariables.Location = new System.Drawing.Point(334, 42);
            this.lvSingleVariables.MultiSelect = false;
            this.lvSingleVariables.Name = "lvSingleVariables";
            this.lvSingleVariables.Scrollable = false;
            this.lvSingleVariables.Size = new System.Drawing.Size(130, 20);
            this.lvSingleVariables.TabIndex = 26;
            this.lvSingleVariables.UseCompatibleStateImageBehavior = false;
            this.lvSingleVariables.View = System.Windows.Forms.View.Details;
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
            // DlgCapaContinuous
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(565, 335);
            this.Controls.Add(this.pnlBackground);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DlgCapaContinuous";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Process Capability Analysis";
            this.pnlBackground.ResumeLayout(false);
            this.pnlBackground.PerformLayout();
            this.grpSubgroupSize.ResumeLayout(false);
            this.grpSubgroupSize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudConstant)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBackground;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnOptions;
        private System.Windows.Forms.Button btnLeft_SubgroupColumn;
        private System.Windows.Forms.Button btnRight_SubgroupColumn;
        private System.Windows.Forms.ListView lvSubgroupColumn;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button btnLeft_SingleVar;
        private System.Windows.Forms.Button btnRight_SingleVar;
        private System.Windows.Forms.Label lblColumns;
        private System.Windows.Forms.ListView lvColumns;
        private System.Windows.Forms.ColumnHeader colType;
        private System.Windows.Forms.ColumnHeader colID;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.Label lblDataArrangedAs;
        private System.Windows.Forms.ListView lvSingleVariables;
        private System.Windows.Forms.ColumnHeader colType_Ser;
        private System.Windows.Forms.ColumnHeader colID_Ser;
        private System.Windows.Forms.ColumnHeader colName_Ser;
        private System.Windows.Forms.ImageList imlColumnType;
        private System.Windows.Forms.Button btnEstimate;
        private System.Windows.Forms.RadioButton rdoSigleColumn;
        private System.Windows.Forms.Label lblIDcolumn;
        private System.Windows.Forms.GroupBox grpSubgroupSize;
        private System.Windows.Forms.NumericUpDown nudConstant;
        private System.Windows.Forms.CheckBox chkConstant;
        private System.Windows.Forms.RadioButton rdoSubgroupAcrossRows;
        private System.Windows.Forms.ListView lvSubgroupAcrossRows;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.TextBox txtBetweenSubgroups;
        private System.Windows.Forms.TextBox txtWithinSubgroup;
        private System.Windows.Forms.TextBox txtHistoricalMean;
        private System.Windows.Forms.TextBox txtUpperSpec;
        private System.Windows.Forms.TextBox txtLowerSpec;
        private System.Windows.Forms.Label lblBetweenOptional;
        private System.Windows.Forms.Label lblWithinOptional;
        private System.Windows.Forms.Label lblHistoricalMeanOptional;
        private System.Windows.Forms.Label lblBetweenSubgroups;
        private System.Windows.Forms.Label lblWithinSubgroup;
        private System.Windows.Forms.Label lblHistoricalStandardDeviations;
        private System.Windows.Forms.Label lblHistoricalMean;
        private System.Windows.Forms.Label lblUpperSpec;
        private System.Windows.Forms.Label lblLowerSpec;
        private System.Windows.Forms.Button btnLeft_SubgroupsAcrossRows;
        private System.Windows.Forms.Button btnRight_SubgroupsAcrossRows;


    }
}