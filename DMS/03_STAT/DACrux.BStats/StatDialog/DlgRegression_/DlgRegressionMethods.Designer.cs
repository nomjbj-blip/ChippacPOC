namespace DACrux.BStats.StatDialog.DlgRegression_
{
    partial class DlgRegressionMethods
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgRegressionMethods));
            this.pnlBackground = new System.Windows.Forms.Panel();
            this.btnLeft_Initialmodel = new System.Windows.Forms.Button();
            this.btnRight_Initialmodel = new System.Windows.Forms.Button();
            this.btnLeft_Everymodel = new System.Windows.Forms.Button();
            this.btnRight_Everymodel = new System.Windows.Forms.Button();
            this.rdoAll = new System.Windows.Forms.RadioButton();
            this.lblPredictorsIncludeinEverymodel = new System.Windows.Forms.Label();
            this.lvPredictorsinEveryModel = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlBack = new System.Windows.Forms.Panel();
            this.lblBackValue = new System.Windows.Forms.Label();
            this.nudBackDrop = new System.Windows.Forms.NumericUpDown();
            this.pnlForward = new System.Windows.Forms.Panel();
            this.lblForwardValue = new System.Windows.Forms.Label();
            this.nudForwardAdd = new System.Windows.Forms.NumericUpDown();
            this.pnlStepwise = new System.Windows.Forms.Panel();
            this.lblStepValueR = new System.Windows.Forms.Label();
            this.nudStepDrop = new System.Windows.Forms.NumericUpDown();
            this.lblStepValueE = new System.Windows.Forms.Label();
            this.nudStepAdd = new System.Windows.Forms.NumericUpDown();
            this.lblPredictorsinInitialModel = new System.Windows.Forms.Label();
            this.lvPredictorsinInitialModel = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rdoBackward = new System.Windows.Forms.RadioButton();
            this.rdoForward = new System.Windows.Forms.RadioButton();
            this.rdoStepwise = new System.Windows.Forms.RadioButton();
            this.grpCriterion = new System.Windows.Forms.GroupBox();
            this.rdoFvalue = new System.Windows.Forms.RadioButton();
            this.rdoAlphavalue = new System.Windows.Forms.RadioButton();
            this.lblColumns = new System.Windows.Forms.Label();
            this.lvColumns = new System.Windows.Forms.ListView();
            this.colType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.imlColumnType = new System.Windows.Forms.ImageList(this.components);
            this.pnlBackground.SuspendLayout();
            this.pnlBack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBackDrop)).BeginInit();
            this.pnlForward.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudForwardAdd)).BeginInit();
            this.pnlStepwise.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStepDrop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStepAdd)).BeginInit();
            this.grpCriterion.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBackground
            // 
            this.pnlBackground.Controls.Add(this.btnLeft_Initialmodel);
            this.pnlBackground.Controls.Add(this.btnRight_Initialmodel);
            this.pnlBackground.Controls.Add(this.btnLeft_Everymodel);
            this.pnlBackground.Controls.Add(this.btnRight_Everymodel);
            this.pnlBackground.Controls.Add(this.rdoAll);
            this.pnlBackground.Controls.Add(this.lblPredictorsIncludeinEverymodel);
            this.pnlBackground.Controls.Add(this.lvPredictorsinEveryModel);
            this.pnlBackground.Controls.Add(this.pnlBack);
            this.pnlBackground.Controls.Add(this.pnlForward);
            this.pnlBackground.Controls.Add(this.pnlStepwise);
            this.pnlBackground.Controls.Add(this.rdoBackward);
            this.pnlBackground.Controls.Add(this.rdoForward);
            this.pnlBackground.Controls.Add(this.rdoStepwise);
            this.pnlBackground.Controls.Add(this.grpCriterion);
            this.pnlBackground.Controls.Add(this.lblColumns);
            this.pnlBackground.Controls.Add(this.lvColumns);
            this.pnlBackground.Controls.Add(this.btnCancel);
            this.pnlBackground.Controls.Add(this.btnOk);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Size = new System.Drawing.Size(364, 494);
            this.pnlBackground.TabIndex = 1;
            // 
            // btnLeft_Initialmodel
            // 
            this.btnLeft_Initialmodel.Enabled = false;
            this.btnLeft_Initialmodel.Location = new System.Drawing.Point(148, 188);
            this.btnLeft_Initialmodel.Name = "btnLeft_Initialmodel";
            this.btnLeft_Initialmodel.Size = new System.Drawing.Size(17, 20);
            this.btnLeft_Initialmodel.TabIndex = 71;
            this.btnLeft_Initialmodel.Text = "◄";
            this.btnLeft_Initialmodel.UseCompatibleTextRendering = true;
            this.btnLeft_Initialmodel.UseVisualStyleBackColor = true;
            // 
            // btnRight_Initialmodel
            // 
            this.btnRight_Initialmodel.Enabled = false;
            this.btnRight_Initialmodel.Location = new System.Drawing.Point(148, 167);
            this.btnRight_Initialmodel.Name = "btnRight_Initialmodel";
            this.btnRight_Initialmodel.Size = new System.Drawing.Size(17, 20);
            this.btnRight_Initialmodel.TabIndex = 70;
            this.btnRight_Initialmodel.Text = "►";
            this.btnRight_Initialmodel.UseCompatibleTextRendering = true;
            this.btnRight_Initialmodel.UseVisualStyleBackColor = true;
            // 
            // btnLeft_Everymodel
            // 
            this.btnLeft_Everymodel.Location = new System.Drawing.Point(148, 58);
            this.btnLeft_Everymodel.Name = "btnLeft_Everymodel";
            this.btnLeft_Everymodel.Size = new System.Drawing.Size(17, 20);
            this.btnLeft_Everymodel.TabIndex = 69;
            this.btnLeft_Everymodel.Text = "◄";
            this.btnLeft_Everymodel.UseCompatibleTextRendering = true;
            this.btnLeft_Everymodel.UseVisualStyleBackColor = true;
            // 
            // btnRight_Everymodel
            // 
            this.btnRight_Everymodel.Location = new System.Drawing.Point(148, 37);
            this.btnRight_Everymodel.Name = "btnRight_Everymodel";
            this.btnRight_Everymodel.Size = new System.Drawing.Size(17, 20);
            this.btnRight_Everymodel.TabIndex = 68;
            this.btnRight_Everymodel.Text = "►";
            this.btnRight_Everymodel.UseCompatibleTextRendering = true;
            this.btnRight_Everymodel.UseVisualStyleBackColor = true;
            // 
            // rdoAll
            // 
            this.rdoAll.AutoSize = true;
            this.rdoAll.Checked = true;
            this.rdoAll.Location = new System.Drawing.Point(159, 97);
            this.rdoAll.Name = "rdoAll";
            this.rdoAll.Size = new System.Drawing.Size(36, 17);
            this.rdoAll.TabIndex = 67;
            this.rdoAll.TabStop = true;
            this.rdoAll.Text = "A&ll";
            this.rdoAll.UseVisualStyleBackColor = true;
            // 
            // lblPredictorsIncludeinEverymodel
            // 
            this.lblPredictorsIncludeinEverymodel.AutoSize = true;
            this.lblPredictorsIncludeinEverymodel.Location = new System.Drawing.Point(169, 19);
            this.lblPredictorsIncludeinEverymodel.Name = "lblPredictorsIncludeinEverymodel";
            this.lblPredictorsIncludeinEverymodel.Size = new System.Drawing.Size(177, 13);
            this.lblPredictorsIncludeinEverymodel.TabIndex = 64;
            this.lblPredictorsIncludeinEverymodel.Text = "Predictors to include in every model";
            // 
            // lvPredictorsinEveryModel
            // 
            this.lvPredictorsinEveryModel.AllowDrop = true;
            this.lvPredictorsinEveryModel.BackColor = System.Drawing.Color.Lavender;
            this.lvPredictorsinEveryModel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvPredictorsinEveryModel.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.lvPredictorsinEveryModel.FullRowSelect = true;
            this.lvPredictorsinEveryModel.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvPredictorsinEveryModel.Location = new System.Drawing.Point(171, 37);
            this.lvPredictorsinEveryModel.Name = "lvPredictorsinEveryModel";
            this.lvPredictorsinEveryModel.Size = new System.Drawing.Size(174, 41);
            this.lvPredictorsinEveryModel.TabIndex = 63;
            this.lvPredictorsinEveryModel.UseCompatibleStateImageBehavior = false;
            this.lvPredictorsinEveryModel.View = System.Windows.Forms.View.Details;
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
            // pnlBack
            // 
            this.pnlBack.Controls.Add(this.lblBackValue);
            this.pnlBack.Controls.Add(this.nudBackDrop);
            this.pnlBack.Enabled = false;
            this.pnlBack.Location = new System.Drawing.Point(171, 402);
            this.pnlBack.Name = "pnlBack";
            this.pnlBack.Size = new System.Drawing.Size(180, 38);
            this.pnlBack.TabIndex = 38;
            // 
            // lblBackValue
            // 
            this.lblBackValue.AutoSize = true;
            this.lblBackValue.Location = new System.Drawing.Point(4, 9);
            this.lblBackValue.Name = "lblBackValue";
            this.lblBackValue.Size = new System.Drawing.Size(85, 13);
            this.lblBackValue.TabIndex = 2;
            this.lblBackValue.Text = "Value to &remove";
            // 
            // nudBackDrop
            // 
            this.nudBackDrop.DecimalPlaces = 4;
            this.nudBackDrop.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.nudBackDrop.Location = new System.Drawing.Point(114, 7);
            this.nudBackDrop.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.nudBackDrop.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            262144});
            this.nudBackDrop.Name = "nudBackDrop";
            this.nudBackDrop.Size = new System.Drawing.Size(62, 21);
            this.nudBackDrop.TabIndex = 0;
            this.nudBackDrop.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // pnlForward
            // 
            this.pnlForward.Controls.Add(this.lblForwardValue);
            this.pnlForward.Controls.Add(this.nudForwardAdd);
            this.pnlForward.Enabled = false;
            this.pnlForward.Location = new System.Drawing.Point(171, 326);
            this.pnlForward.Name = "pnlForward";
            this.pnlForward.Size = new System.Drawing.Size(180, 38);
            this.pnlForward.TabIndex = 37;
            // 
            // lblForwardValue
            // 
            this.lblForwardValue.AutoSize = true;
            this.lblForwardValue.Location = new System.Drawing.Point(4, 9);
            this.lblForwardValue.Name = "lblForwardValue";
            this.lblForwardValue.Size = new System.Drawing.Size(75, 13);
            this.lblForwardValue.TabIndex = 2;
            this.lblForwardValue.Text = "Value to &enter";
            // 
            // nudForwardAdd
            // 
            this.nudForwardAdd.DecimalPlaces = 4;
            this.nudForwardAdd.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.nudForwardAdd.Location = new System.Drawing.Point(114, 7);
            this.nudForwardAdd.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.nudForwardAdd.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            262144});
            this.nudForwardAdd.Name = "nudForwardAdd";
            this.nudForwardAdd.Size = new System.Drawing.Size(62, 21);
            this.nudForwardAdd.TabIndex = 0;
            this.nudForwardAdd.Value = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            // 
            // pnlStepwise
            // 
            this.pnlStepwise.Controls.Add(this.lblStepValueR);
            this.pnlStepwise.Controls.Add(this.nudStepDrop);
            this.pnlStepwise.Controls.Add(this.lblStepValueE);
            this.pnlStepwise.Controls.Add(this.nudStepAdd);
            this.pnlStepwise.Controls.Add(this.lblPredictorsinInitialModel);
            this.pnlStepwise.Controls.Add(this.lvPredictorsinInitialModel);
            this.pnlStepwise.Enabled = false;
            this.pnlStepwise.Location = new System.Drawing.Point(171, 149);
            this.pnlStepwise.Name = "pnlStepwise";
            this.pnlStepwise.Size = new System.Drawing.Size(180, 138);
            this.pnlStepwise.TabIndex = 36;
            // 
            // lblStepValueR
            // 
            this.lblStepValueR.AutoSize = true;
            this.lblStepValueR.Location = new System.Drawing.Point(3, 111);
            this.lblStepValueR.Name = "lblStepValueR";
            this.lblStepValueR.Size = new System.Drawing.Size(85, 13);
            this.lblStepValueR.TabIndex = 3;
            this.lblStepValueR.Text = "Value to &remove";
            // 
            // nudStepDrop
            // 
            this.nudStepDrop.DecimalPlaces = 4;
            this.nudStepDrop.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.nudStepDrop.Location = new System.Drawing.Point(114, 110);
            this.nudStepDrop.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.nudStepDrop.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            262144});
            this.nudStepDrop.Name = "nudStepDrop";
            this.nudStepDrop.Size = new System.Drawing.Size(62, 21);
            this.nudStepDrop.TabIndex = 1;
            this.nudStepDrop.Value = new decimal(new int[] {
            15,
            0,
            0,
            131072});
            // 
            // lblStepValueE
            // 
            this.lblStepValueE.AutoSize = true;
            this.lblStepValueE.Location = new System.Drawing.Point(3, 78);
            this.lblStepValueE.Name = "lblStepValueE";
            this.lblStepValueE.Size = new System.Drawing.Size(75, 13);
            this.lblStepValueE.TabIndex = 2;
            this.lblStepValueE.Text = "Value to &enter";
            // 
            // nudStepAdd
            // 
            this.nudStepAdd.DecimalPlaces = 4;
            this.nudStepAdd.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.nudStepAdd.Location = new System.Drawing.Point(114, 76);
            this.nudStepAdd.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.nudStepAdd.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            262144});
            this.nudStepAdd.Name = "nudStepAdd";
            this.nudStepAdd.Size = new System.Drawing.Size(62, 21);
            this.nudStepAdd.TabIndex = 0;
            this.nudStepAdd.Value = new decimal(new int[] {
            15,
            0,
            0,
            131072});
            // 
            // lblPredictorsinInitialModel
            // 
            this.lblPredictorsinInitialModel.AutoSize = true;
            this.lblPredictorsinInitialModel.Location = new System.Drawing.Point(2, 2);
            this.lblPredictorsinInitialModel.Name = "lblPredictorsinInitialModel";
            this.lblPredictorsinInitialModel.Size = new System.Drawing.Size(124, 13);
            this.lblPredictorsinInitialModel.TabIndex = 66;
            this.lblPredictorsinInitialModel.Text = "Predictors in initial model";
            // 
            // lvPredictorsinInitialModel
            // 
            this.lvPredictorsinInitialModel.AllowDrop = true;
            this.lvPredictorsinInitialModel.BackColor = System.Drawing.Color.Lavender;
            this.lvPredictorsinInitialModel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvPredictorsinInitialModel.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvPredictorsinInitialModel.FullRowSelect = true;
            this.lvPredictorsinInitialModel.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvPredictorsinInitialModel.Location = new System.Drawing.Point(2, 18);
            this.lvPredictorsinInitialModel.Name = "lvPredictorsinInitialModel";
            this.lvPredictorsinInitialModel.Size = new System.Drawing.Size(174, 56);
            this.lvPredictorsinInitialModel.TabIndex = 65;
            this.lvPredictorsinInitialModel.UseCompatibleStateImageBehavior = false;
            this.lvPredictorsinInitialModel.View = System.Windows.Forms.View.Details;
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
            // rdoBackward
            // 
            this.rdoBackward.AutoSize = true;
            this.rdoBackward.Location = new System.Drawing.Point(159, 379);
            this.rdoBackward.Name = "rdoBackward";
            this.rdoBackward.Size = new System.Drawing.Size(124, 17);
            this.rdoBackward.TabIndex = 35;
            this.rdoBackward.Text = "&Backward elimination";
            this.rdoBackward.UseVisualStyleBackColor = true;
            // 
            // rdoForward
            // 
            this.rdoForward.AutoSize = true;
            this.rdoForward.Location = new System.Drawing.Point(159, 304);
            this.rdoForward.Name = "rdoForward";
            this.rdoForward.Size = new System.Drawing.Size(110, 17);
            this.rdoForward.TabIndex = 34;
            this.rdoForward.Text = "&Forward selection";
            this.rdoForward.UseVisualStyleBackColor = true;
            // 
            // rdoStepwise
            // 
            this.rdoStepwise.AutoSize = true;
            this.rdoStepwise.Location = new System.Drawing.Point(159, 124);
            this.rdoStepwise.Name = "rdoStepwise";
            this.rdoStepwise.Size = new System.Drawing.Size(187, 17);
            this.rdoStepwise.TabIndex = 33;
            this.rdoStepwise.Text = "&Stepwise (forward and backward)";
            this.rdoStepwise.UseVisualStyleBackColor = true;
            // 
            // grpCriterion
            // 
            this.grpCriterion.Controls.Add(this.rdoFvalue);
            this.grpCriterion.Controls.Add(this.rdoAlphavalue);
            this.grpCriterion.Location = new System.Drawing.Point(19, 375);
            this.grpCriterion.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.grpCriterion.Name = "grpCriterion";
            this.grpCriterion.Padding = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.grpCriterion.Size = new System.Drawing.Size(124, 65);
            this.grpCriterion.TabIndex = 32;
            this.grpCriterion.TabStop = false;
            this.grpCriterion.Text = "Criterion";
            // 
            // rdoFvalue
            // 
            this.rdoFvalue.AutoSize = true;
            this.rdoFvalue.Location = new System.Drawing.Point(5, 42);
            this.rdoFvalue.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.rdoFvalue.Name = "rdoFvalue";
            this.rdoFvalue.Size = new System.Drawing.Size(97, 16);
            this.rdoFvalue.TabIndex = 1;
            this.rdoFvalue.Text = "&Use F values";
            this.rdoFvalue.UseVisualStyleBackColor = true;
            // 
            // rdoAlphavalue
            // 
            this.rdoAlphavalue.AutoSize = true;
            this.rdoAlphavalue.Checked = true;
            this.rdoAlphavalue.Location = new System.Drawing.Point(5, 16);
            this.rdoAlphavalue.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.rdoAlphavalue.Name = "rdoAlphavalue";
            this.rdoAlphavalue.Size = new System.Drawing.Size(121, 16);
            this.rdoAlphavalue.TabIndex = 0;
            this.rdoAlphavalue.TabStop = true;
            this.rdoAlphavalue.Text = "Use &alpha values";
            this.rdoAlphavalue.UseVisualStyleBackColor = true;
            // 
            // lblColumns
            // 
            this.lblColumns.AutoSize = true;
            this.lblColumns.Location = new System.Drawing.Point(19, 19);
            this.lblColumns.Name = "lblColumns";
            this.lblColumns.Size = new System.Drawing.Size(47, 13);
            this.lblColumns.TabIndex = 30;
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
            this.lvColumns.Location = new System.Drawing.Point(19, 37);
            this.lvColumns.Name = "lvColumns";
            this.lvColumns.Size = new System.Drawing.Size(125, 328);
            this.lvColumns.TabIndex = 29;
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
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(289, 457);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(59, 22);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(225, 457);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(59, 22);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // imlColumnType
            // 
            this.imlColumnType.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlColumnType.ImageStream")));
            this.imlColumnType.TransparentColor = System.Drawing.Color.Transparent;
            this.imlColumnType.Images.SetKeyName(0, "NUMBER");
            this.imlColumnType.Images.SetKeyName(1, "TEXT");
            this.imlColumnType.Images.SetKeyName(2, "DATETIME");
            // 
            // DlgRegressionMethods
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(364, 494);
            this.Controls.Add(this.pnlBackground);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DlgRegressionMethods";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DlgRegressionMethods";
            this.pnlBackground.ResumeLayout(false);
            this.pnlBackground.PerformLayout();
            this.pnlBack.ResumeLayout(false);
            this.pnlBack.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBackDrop)).EndInit();
            this.pnlForward.ResumeLayout(false);
            this.pnlForward.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudForwardAdd)).EndInit();
            this.pnlStepwise.ResumeLayout(false);
            this.pnlStepwise.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStepDrop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStepAdd)).EndInit();
            this.grpCriterion.ResumeLayout(false);
            this.grpCriterion.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBackground;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblStepValueR;
        private System.Windows.Forms.Label lblStepValueE;
        private System.Windows.Forms.NumericUpDown nudStepDrop;
        private System.Windows.Forms.NumericUpDown nudStepAdd;
        private System.Windows.Forms.Label lblColumns;
        private System.Windows.Forms.ListView lvColumns;
        private System.Windows.Forms.ColumnHeader colType;
        private System.Windows.Forms.ColumnHeader colID;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.Panel pnlStepwise;
        private System.Windows.Forms.RadioButton rdoBackward;
        private System.Windows.Forms.RadioButton rdoForward;
        private System.Windows.Forms.RadioButton rdoStepwise;
        private System.Windows.Forms.GroupBox grpCriterion;
        private System.Windows.Forms.RadioButton rdoFvalue;
        private System.Windows.Forms.RadioButton rdoAlphavalue;
        private System.Windows.Forms.Panel pnlBack;
        private System.Windows.Forms.Label lblBackValue;
        private System.Windows.Forms.NumericUpDown nudBackDrop;
        private System.Windows.Forms.Panel pnlForward;
        private System.Windows.Forms.Label lblForwardValue;
        private System.Windows.Forms.NumericUpDown nudForwardAdd;
        private System.Windows.Forms.Label lblPredictorsIncludeinEverymodel;
        private System.Windows.Forms.ListView lvPredictorsinEveryModel;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Label lblPredictorsinInitialModel;
        private System.Windows.Forms.ListView lvPredictorsinInitialModel;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.RadioButton rdoAll;
        private System.Windows.Forms.Button btnLeft_Initialmodel;
        private System.Windows.Forms.Button btnRight_Initialmodel;
        private System.Windows.Forms.Button btnLeft_Everymodel;
        private System.Windows.Forms.Button btnRight_Everymodel;
        private System.Windows.Forms.ImageList imlColumnType;
    }
}