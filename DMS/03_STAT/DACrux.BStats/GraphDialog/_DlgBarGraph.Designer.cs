namespace DACrux.BStats.GraphDialog
{
    partial class _DlgBarGraph
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_DlgBarGraph));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lvColumns = new System.Windows.Forms.ListView();
            this.colType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblSeries = new System.Windows.Forms.Label();
            this.lblColumns = new System.Windows.Forms.Label();
            this.lvLabel = new System.Windows.Forms.ListView();
            this.colType_Leg = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colID_Leg = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName_Leg = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblLegend = new System.Windows.Forms.Label();
            this.lvSeries = new System.Windows.Forms.ListView();
            this.colType_Ser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colID_Ser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName_Ser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pictureBoxSample = new System.Windows.Forms.PictureBox();
            this.grbOption = new System.Windows.Forms.GroupBox();
            this.chkPointLabel = new System.Windows.Forms.CheckBox();
            this.cboLabelAngle = new System.Windows.Forms.ComboBox();
            this.lblBarSize = new System.Windows.Forms.Label();
            this.nudBarSize = new System.Windows.Forms.NumericUpDown();
            this.lblDateTimeFormat = new System.Windows.Forms.Label();
            this.lblDecimalPlace = new System.Windows.Forms.Label();
            this.lblLabelAngle = new System.Windows.Forms.Label();
            this.cboDateTimeFormat = new System.Windows.Forms.ComboBox();
            this.nudDecimalPlace = new System.Windows.Forms.NumericUpDown();
            this.chkView3D = new System.Windows.Forms.CheckBox();
            this.txtAxisYTitle = new System.Windows.Forms.TextBox();
            this.txtAxisXTitle = new System.Windows.Forms.TextBox();
            this.lblAxisYTitle = new System.Windows.Forms.Label();
            this.lblAxisXTitle = new System.Windows.Forms.Label();
            this.chkSerLegBox = new System.Windows.Forms.CheckBox();
            this.lblSample = new System.Windows.Forms.Label();
            this.btnLeft_Ser = new System.Windows.Forms.Button();
            this.btnRight_Ser = new System.Windows.Forms.Button();
            this.btnLeft_Leg = new System.Windows.Forms.Button();
            this.btnRight_Leg = new System.Windows.Forms.Button();
            this.imlColumnType = new System.Windows.Forms.ImageList(this.components);
            this.btnLeft_Val = new System.Windows.Forms.Button();
            this.btnRight_Val = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lvValue = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSample)).BeginInit();
            this.grbOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBarSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDecimalPlace)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(522, 383);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(64, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(592, 383);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
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
            this.lvColumns.Location = new System.Drawing.Point(12, 153);
            this.lvColumns.Name = "lvColumns";
            this.lvColumns.Size = new System.Drawing.Size(145, 219);
            this.lvColumns.TabIndex = 7;
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
            // lblSeries
            // 
            this.lblSeries.AutoSize = true;
            this.lblSeries.Location = new System.Drawing.Point(187, 137);
            this.lblSeries.Name = "lblSeries";
            this.lblSeries.Size = new System.Drawing.Size(36, 13);
            this.lblSeries.TabIndex = 11;
            this.lblSeries.Text = "Series";
            // 
            // lblColumns
            // 
            this.lblColumns.AutoSize = true;
            this.lblColumns.Location = new System.Drawing.Point(9, 137);
            this.lblColumns.Name = "lblColumns";
            this.lblColumns.Size = new System.Drawing.Size(47, 13);
            this.lblColumns.TabIndex = 12;
            this.lblColumns.Text = "Columns";
            // 
            // lvLabel
            // 
            this.lvLabel.AllowDrop = true;
            this.lvLabel.BackColor = System.Drawing.Color.Lavender;
            this.lvLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvLabel.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colType_Leg,
            this.colID_Leg,
            this.colName_Leg});
            this.lvLabel.FullRowSelect = true;
            this.lvLabel.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvLabel.Location = new System.Drawing.Point(343, 354);
            this.lvLabel.MultiSelect = false;
            this.lvLabel.Name = "lvLabel";
            this.lvLabel.Scrollable = false;
            this.lvLabel.Size = new System.Drawing.Size(313, 19);
            this.lvLabel.TabIndex = 13;
            this.lvLabel.UseCompatibleStateImageBehavior = false;
            this.lvLabel.View = System.Windows.Forms.View.Details;
            // 
            // colType_Leg
            // 
            this.colType_Leg.Width = 20;
            // 
            // colID_Leg
            // 
            this.colID_Leg.Width = 50;
            // 
            // colName_Leg
            // 
            this.colName_Leg.Width = 170;
            // 
            // lblLegend
            // 
            this.lblLegend.AutoSize = true;
            this.lblLegend.BackColor = System.Drawing.Color.Transparent;
            this.lblLegend.Location = new System.Drawing.Point(187, 357);
            this.lblLegend.Name = "lblLegend";
            this.lblLegend.Size = new System.Drawing.Size(72, 13);
            this.lblLegend.TabIndex = 14;
            this.lblLegend.Text = "Label (X Axis)";
            // 
            // lvSeries
            // 
            this.lvSeries.AllowDrop = true;
            this.lvSeries.BackColor = System.Drawing.Color.Lavender;
            this.lvSeries.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvSeries.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colType_Ser,
            this.colID_Ser,
            this.colName_Ser});
            this.lvSeries.FullRowSelect = true;
            this.lvSeries.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvSeries.Location = new System.Drawing.Point(189, 153);
            this.lvSeries.Name = "lvSeries";
            this.lvSeries.Size = new System.Drawing.Size(145, 44);
            this.lvSeries.TabIndex = 9;
            this.lvSeries.UseCompatibleStateImageBehavior = false;
            this.lvSeries.View = System.Windows.Forms.View.Details;
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
            // pictureBoxSample
            // 
            this.pictureBoxSample.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBoxSample.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxSample.Image")));
            this.pictureBoxSample.Location = new System.Drawing.Point(343, 153);
            this.pictureBoxSample.Name = "pictureBoxSample";
            this.pictureBoxSample.Size = new System.Drawing.Size(313, 194);
            this.pictureBoxSample.TabIndex = 15;
            this.pictureBoxSample.TabStop = false;
            // 
            // grbOption
            // 
            this.grbOption.Controls.Add(this.chkPointLabel);
            this.grbOption.Controls.Add(this.cboLabelAngle);
            this.grbOption.Controls.Add(this.lblBarSize);
            this.grbOption.Controls.Add(this.nudBarSize);
            this.grbOption.Controls.Add(this.lblDateTimeFormat);
            this.grbOption.Controls.Add(this.lblDecimalPlace);
            this.grbOption.Controls.Add(this.lblLabelAngle);
            this.grbOption.Controls.Add(this.cboDateTimeFormat);
            this.grbOption.Controls.Add(this.nudDecimalPlace);
            this.grbOption.Controls.Add(this.chkView3D);
            this.grbOption.Controls.Add(this.txtAxisYTitle);
            this.grbOption.Controls.Add(this.txtAxisXTitle);
            this.grbOption.Controls.Add(this.lblAxisYTitle);
            this.grbOption.Controls.Add(this.lblAxisXTitle);
            this.grbOption.Controls.Add(this.chkSerLegBox);
            this.grbOption.Location = new System.Drawing.Point(12, 11);
            this.grbOption.Name = "grbOption";
            this.grbOption.Size = new System.Drawing.Size(644, 110);
            this.grbOption.TabIndex = 8;
            this.grbOption.TabStop = false;
            this.grbOption.Text = "Option";
            // 
            // chkPointLabel
            // 
            this.chkPointLabel.AutoSize = true;
            this.chkPointLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkPointLabel.Location = new System.Drawing.Point(481, 45);
            this.chkPointLabel.Name = "chkPointLabel";
            this.chkPointLabel.Size = new System.Drawing.Size(84, 16);
            this.chkPointLabel.TabIndex = 27;
            this.chkPointLabel.Text = "Point Label";
            this.chkPointLabel.UseVisualStyleBackColor = true;
            // 
            // cboLabelAngle
            // 
            this.cboLabelAngle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLabelAngle.FormattingEnabled = true;
            this.cboLabelAngle.Items.AddRange(new object[] {
            "Horizontal",
            "Diagonal",
            "Vertical"});
            this.cboLabelAngle.Location = new System.Drawing.Point(344, 19);
            this.cboLabelAngle.Name = "cboLabelAngle";
            this.cboLabelAngle.Size = new System.Drawing.Size(100, 21);
            this.cboLabelAngle.TabIndex = 26;
            // 
            // lblBarSize
            // 
            this.lblBarSize.AutoSize = true;
            this.lblBarSize.Location = new System.Drawing.Point(248, 71);
            this.lblBarSize.Name = "lblBarSize";
            this.lblBarSize.Size = new System.Drawing.Size(45, 13);
            this.lblBarSize.TabIndex = 25;
            this.lblBarSize.Text = "Bar Size";
            // 
            // nudBarSize
            // 
            this.nudBarSize.DecimalPlaces = 1;
            this.nudBarSize.Location = new System.Drawing.Point(344, 69);
            this.nudBarSize.Name = "nudBarSize";
            this.nudBarSize.Size = new System.Drawing.Size(100, 21);
            this.nudBarSize.TabIndex = 24;
            // 
            // lblDateTimeFormat
            // 
            this.lblDateTimeFormat.AutoSize = true;
            this.lblDateTimeFormat.Location = new System.Drawing.Point(22, 70);
            this.lblDateTimeFormat.Name = "lblDateTimeFormat";
            this.lblDateTimeFormat.Size = new System.Drawing.Size(89, 13);
            this.lblDateTimeFormat.TabIndex = 23;
            this.lblDateTimeFormat.Text = "DateTime Format";
            // 
            // lblDecimalPlace
            // 
            this.lblDecimalPlace.AutoSize = true;
            this.lblDecimalPlace.Location = new System.Drawing.Point(248, 46);
            this.lblDecimalPlace.Name = "lblDecimalPlace";
            this.lblDecimalPlace.Size = new System.Drawing.Size(71, 13);
            this.lblDecimalPlace.TabIndex = 22;
            this.lblDecimalPlace.Text = "Decimal Place";
            // 
            // lblLabelAngle
            // 
            this.lblLabelAngle.AutoSize = true;
            this.lblLabelAngle.Location = new System.Drawing.Point(248, 22);
            this.lblLabelAngle.Name = "lblLabelAngle";
            this.lblLabelAngle.Size = new System.Drawing.Size(62, 13);
            this.lblLabelAngle.TabIndex = 21;
            this.lblLabelAngle.Text = "Label Angle";
            // 
            // cboDateTimeFormat
            // 
            this.cboDateTimeFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDateTimeFormat.FormattingEnabled = true;
            this.cboDateTimeFormat.Items.AddRange(new object[] {
            "Full",
            "LongDate",
            "LongTime",
            "ShortDate",
            "ShortTime"});
            this.cboDateTimeFormat.Location = new System.Drawing.Point(120, 67);
            this.cboDateTimeFormat.Name = "cboDateTimeFormat";
            this.cboDateTimeFormat.Size = new System.Drawing.Size(100, 21);
            this.cboDateTimeFormat.TabIndex = 20;
            // 
            // nudDecimalPlace
            // 
            this.nudDecimalPlace.Location = new System.Drawing.Point(344, 45);
            this.nudDecimalPlace.Name = "nudDecimalPlace";
            this.nudDecimalPlace.Size = new System.Drawing.Size(100, 21);
            this.nudDecimalPlace.TabIndex = 18;
            // 
            // chkView3D
            // 
            this.chkView3D.AutoSize = true;
            this.chkView3D.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkView3D.Location = new System.Drawing.Point(481, 71);
            this.chkView3D.Name = "chkView3D";
            this.chkView3D.Size = new System.Drawing.Size(67, 16);
            this.chkView3D.TabIndex = 17;
            this.chkView3D.Text = "View 3D";
            this.chkView3D.UseVisualStyleBackColor = true;
            // 
            // txtAxisYTitle
            // 
            this.txtAxisYTitle.Location = new System.Drawing.Point(120, 44);
            this.txtAxisYTitle.Name = "txtAxisYTitle";
            this.txtAxisYTitle.Size = new System.Drawing.Size(100, 21);
            this.txtAxisYTitle.TabIndex = 16;
            // 
            // txtAxisXTitle
            // 
            this.txtAxisXTitle.Location = new System.Drawing.Point(120, 20);
            this.txtAxisXTitle.Name = "txtAxisXTitle";
            this.txtAxisXTitle.Size = new System.Drawing.Size(100, 21);
            this.txtAxisXTitle.TabIndex = 15;
            // 
            // lblAxisYTitle
            // 
            this.lblAxisYTitle.AutoSize = true;
            this.lblAxisYTitle.Location = new System.Drawing.Point(22, 46);
            this.lblAxisYTitle.Name = "lblAxisYTitle";
            this.lblAxisYTitle.Size = new System.Drawing.Size(59, 13);
            this.lblAxisYTitle.TabIndex = 14;
            this.lblAxisYTitle.Text = "Y Axis Title";
            // 
            // lblAxisXTitle
            // 
            this.lblAxisXTitle.AutoSize = true;
            this.lblAxisXTitle.Location = new System.Drawing.Point(22, 22);
            this.lblAxisXTitle.Name = "lblAxisXTitle";
            this.lblAxisXTitle.Size = new System.Drawing.Size(59, 13);
            this.lblAxisXTitle.TabIndex = 13;
            this.lblAxisXTitle.Text = "X Axis Title";
            // 
            // chkSerLegBox
            // 
            this.chkSerLegBox.AutoSize = true;
            this.chkSerLegBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkSerLegBox.Location = new System.Drawing.Point(481, 19);
            this.chkSerLegBox.Name = "chkSerLegBox";
            this.chkSerLegBox.Size = new System.Drawing.Size(129, 16);
            this.chkSerLegBox.TabIndex = 0;
            this.chkSerLegBox.Text = "Series Legend Box";
            this.chkSerLegBox.UseVisualStyleBackColor = true;
            // 
            // lblSample
            // 
            this.lblSample.AutoSize = true;
            this.lblSample.Location = new System.Drawing.Point(340, 137);
            this.lblSample.Name = "lblSample";
            this.lblSample.Size = new System.Drawing.Size(41, 13);
            this.lblSample.TabIndex = 16;
            this.lblSample.Text = "Sample";
            // 
            // btnLeft_Ser
            // 
            this.btnLeft_Ser.Location = new System.Drawing.Point(163, 178);
            this.btnLeft_Ser.Name = "btnLeft_Ser";
            this.btnLeft_Ser.Size = new System.Drawing.Size(20, 19);
            this.btnLeft_Ser.TabIndex = 18;
            this.btnLeft_Ser.Text = "◄";
            this.btnLeft_Ser.UseCompatibleTextRendering = true;
            this.btnLeft_Ser.UseVisualStyleBackColor = true;
            // 
            // btnRight_Ser
            // 
            this.btnRight_Ser.Location = new System.Drawing.Point(163, 154);
            this.btnRight_Ser.Name = "btnRight_Ser";
            this.btnRight_Ser.Size = new System.Drawing.Size(20, 19);
            this.btnRight_Ser.TabIndex = 17;
            this.btnRight_Ser.Text = "►";
            this.btnRight_Ser.UseCompatibleTextRendering = true;
            this.btnRight_Ser.UseVisualStyleBackColor = true;
            // 
            // btnLeft_Leg
            // 
            this.btnLeft_Leg.Location = new System.Drawing.Point(287, 354);
            this.btnLeft_Leg.Name = "btnLeft_Leg";
            this.btnLeft_Leg.Size = new System.Drawing.Size(20, 19);
            this.btnLeft_Leg.TabIndex = 20;
            this.btnLeft_Leg.Text = "◄";
            this.btnLeft_Leg.UseCompatibleTextRendering = true;
            this.btnLeft_Leg.UseVisualStyleBackColor = true;
            // 
            // btnRight_Leg
            // 
            this.btnRight_Leg.Location = new System.Drawing.Point(313, 354);
            this.btnRight_Leg.Name = "btnRight_Leg";
            this.btnRight_Leg.Size = new System.Drawing.Size(20, 19);
            this.btnRight_Leg.TabIndex = 19;
            this.btnRight_Leg.Text = "►";
            this.btnRight_Leg.UseCompatibleTextRendering = true;
            this.btnRight_Leg.UseVisualStyleBackColor = true;
            // 
            // imlColumnType
            // 
            this.imlColumnType.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlColumnType.ImageStream")));
            this.imlColumnType.TransparentColor = System.Drawing.Color.Transparent;
            this.imlColumnType.Images.SetKeyName(0, "NUMBER");
            this.imlColumnType.Images.SetKeyName(1, "TEXT");
            this.imlColumnType.Images.SetKeyName(2, "DATETIME");
            // 
            // btnLeft_Val
            // 
            this.btnLeft_Val.Location = new System.Drawing.Point(162, 245);
            this.btnLeft_Val.Name = "btnLeft_Val";
            this.btnLeft_Val.Size = new System.Drawing.Size(20, 19);
            this.btnLeft_Val.TabIndex = 24;
            this.btnLeft_Val.Text = "◄";
            this.btnLeft_Val.UseCompatibleTextRendering = true;
            this.btnLeft_Val.UseVisualStyleBackColor = true;
            // 
            // btnRight_Val
            // 
            this.btnRight_Val.Location = new System.Drawing.Point(162, 221);
            this.btnRight_Val.Name = "btnRight_Val";
            this.btnRight_Val.Size = new System.Drawing.Size(20, 19);
            this.btnRight_Val.TabIndex = 23;
            this.btnRight_Val.Text = "►";
            this.btnRight_Val.UseCompatibleTextRendering = true;
            this.btnRight_Val.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(187, 204);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Value";
            // 
            // lvValue
            // 
            this.lvValue.AllowDrop = true;
            this.lvValue.BackColor = System.Drawing.Color.Lavender;
            this.lvValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvValue.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvValue.FullRowSelect = true;
            this.lvValue.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvValue.Location = new System.Drawing.Point(188, 220);
            this.lvValue.Name = "lvValue";
            this.lvValue.Size = new System.Drawing.Size(145, 127);
            this.lvValue.TabIndex = 21;
            this.lvValue.UseCompatibleStateImageBehavior = false;
            this.lvValue.View = System.Windows.Forms.View.Details;
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
            // _DlgBarGraph
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 420);
            this.Controls.Add(this.btnLeft_Val);
            this.Controls.Add(this.btnRight_Val);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lvValue);
            this.Controls.Add(this.btnLeft_Leg);
            this.Controls.Add(this.btnRight_Leg);
            this.Controls.Add(this.btnLeft_Ser);
            this.Controls.Add(this.btnRight_Ser);
            this.Controls.Add(this.lblLegend);
            this.Controls.Add(this.pictureBoxSample);
            this.Controls.Add(this.lblSample);
            this.Controls.Add(this.lblColumns);
            this.Controls.Add(this.lvColumns);
            this.Controls.Add(this.lvLabel);
            this.Controls.Add(this.lblSeries);
            this.Controls.Add(this.lvSeries);
            this.Controls.Add(this.grbOption);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "_DlgBarGraph";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Activated += new System.EventHandler(this._DlgBarGraph_Activated);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this._DlgBarGraph_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSample)).EndInit();
            this.grbOption.ResumeLayout(false);
            this.grbOption.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBarSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDecimalPlace)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListView lvColumns;
        private System.Windows.Forms.ColumnHeader colType;
        private System.Windows.Forms.ColumnHeader colID;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.Label lblSeries;
        private System.Windows.Forms.Label lblColumns;
        private System.Windows.Forms.ListView lvLabel;
        private System.Windows.Forms.Label lblLegend;
        private System.Windows.Forms.ListView lvSeries;
        private System.Windows.Forms.PictureBox pictureBoxSample;
        private System.Windows.Forms.GroupBox grbOption;
        private System.Windows.Forms.Label lblSample;
        private System.Windows.Forms.CheckBox chkSerLegBox;
        private System.Windows.Forms.Button btnLeft_Ser;
        private System.Windows.Forms.Button btnRight_Ser;
        private System.Windows.Forms.Button btnLeft_Leg;
        private System.Windows.Forms.Button btnRight_Leg;
        private System.Windows.Forms.CheckBox chkView3D;
        private System.Windows.Forms.ImageList imlColumnType;
        private System.Windows.Forms.ColumnHeader colType_Ser;
        private System.Windows.Forms.ColumnHeader colID_Ser;
        private System.Windows.Forms.ColumnHeader colName_Ser;
        private System.Windows.Forms.ColumnHeader colType_Leg;
        private System.Windows.Forms.ColumnHeader colID_Leg;
        private System.Windows.Forms.ColumnHeader colName_Leg;
        private System.Windows.Forms.NumericUpDown nudDecimalPlace;
        private System.Windows.Forms.Label lblDateTimeFormat;
        private System.Windows.Forms.Label lblDecimalPlace;
        private System.Windows.Forms.Label lblLabelAngle;
        private System.Windows.Forms.Label lblBarSize;
        private System.Windows.Forms.NumericUpDown nudBarSize;
        private System.Windows.Forms.ComboBox cboDateTimeFormat;
        private System.Windows.Forms.ComboBox cboLabelAngle;
        private System.Windows.Forms.CheckBox chkPointLabel;
        private System.Windows.Forms.TextBox txtAxisYTitle;
        private System.Windows.Forms.TextBox txtAxisXTitle;
        private System.Windows.Forms.Label lblAxisYTitle;
        private System.Windows.Forms.Label lblAxisXTitle;
        private System.Windows.Forms.Button btnLeft_Val;
        private System.Windows.Forms.Button btnRight_Val;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lvValue;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}