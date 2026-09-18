namespace DACrux.BStats.GraphDialog
{
    partial class DlgParetoGraph
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgParetoGraph));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lvColumns = new System.Windows.Forms.ListView();
            this.colType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblSeries = new System.Windows.Forms.Label();
            this.lblColumns = new System.Windows.Forms.Label();
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
            this.imlColumnType = new System.Windows.Forms.ImageList(this.components);
            this.lvLabel = new System.Windows.Forms.ListView();
            this.colType_Leg = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colID_Leg = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName_Leg = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnLeft_Leg = new System.Windows.Forms.Button();
            this.btnRight_Leg = new System.Windows.Forms.Button();
            this.lblLegend = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSample)).BeginInit();
            this.grbOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBarSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDecimalPlace)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(522, 388);
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
            this.btnCancel.Location = new System.Drawing.Point(592, 388);
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
            this.lvColumns.Size = new System.Drawing.Size(145, 218);
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
            this.lblSeries.Location = new System.Drawing.Point(186, 137);
            this.lblSeries.Name = "lblSeries";
            this.lblSeries.Size = new System.Drawing.Size(73, 13);
            this.lblSeries.TabIndex = 11;
            this.lblSeries.Text = "Series (Value)";
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
            this.lvSeries.MultiSelect = false;
            this.lvSeries.Name = "lvSeries";
            this.lvSeries.Size = new System.Drawing.Size(145, 194);
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
            this.pictureBoxSample.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBoxSample.BackgroundImage")));
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
            this.chkPointLabel.Size = new System.Drawing.Size(75, 17);
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
            this.lblBarSize.Location = new System.Drawing.Point(248, 46);
            this.lblBarSize.Name = "lblBarSize";
            this.lblBarSize.Size = new System.Drawing.Size(45, 13);
            this.lblBarSize.TabIndex = 25;
            this.lblBarSize.Text = "Bar Size";
            // 
            // nudBarSize
            // 
            this.nudBarSize.DecimalPlaces = 1;
            this.nudBarSize.Location = new System.Drawing.Point(344, 44);
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
            this.lblDecimalPlace.Location = new System.Drawing.Point(248, 70);
            this.lblDecimalPlace.Name = "lblDecimalPlace";
            this.lblDecimalPlace.Size = new System.Drawing.Size(71, 13);
            this.lblDecimalPlace.TabIndex = 22;
            this.lblDecimalPlace.Text = "Decimal Place";
            this.lblDecimalPlace.Visible = false;
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
            this.nudDecimalPlace.Location = new System.Drawing.Point(344, 67);
            this.nudDecimalPlace.Name = "nudDecimalPlace";
            this.nudDecimalPlace.Size = new System.Drawing.Size(100, 21);
            this.nudDecimalPlace.TabIndex = 18;
            this.nudDecimalPlace.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDecimalPlace.Visible = false;
            // 
            // chkView3D
            // 
            this.chkView3D.AutoSize = true;
            this.chkView3D.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkView3D.Location = new System.Drawing.Point(481, 71);
            this.chkView3D.Name = "chkView3D";
            this.chkView3D.Size = new System.Drawing.Size(61, 17);
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
            this.chkSerLegBox.Size = new System.Drawing.Size(111, 17);
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
            // imlColumnType
            // 
            this.imlColumnType.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlColumnType.ImageStream")));
            this.imlColumnType.TransparentColor = System.Drawing.Color.Transparent;
            this.imlColumnType.Images.SetKeyName(0, "NUMBER");
            this.imlColumnType.Images.SetKeyName(1, "TEXT");
            this.imlColumnType.Images.SetKeyName(2, "DATETIME");
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
            this.lvLabel.Location = new System.Drawing.Point(343, 353);
            this.lvLabel.MultiSelect = false;
            this.lvLabel.Name = "lvLabel";
            this.lvLabel.Scrollable = false;
            this.lvLabel.Size = new System.Drawing.Size(313, 19);
            this.lvLabel.TabIndex = 25;
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
            // btnLeft_Leg
            // 
            this.btnLeft_Leg.Location = new System.Drawing.Point(287, 353);
            this.btnLeft_Leg.Name = "btnLeft_Leg";
            this.btnLeft_Leg.Size = new System.Drawing.Size(20, 19);
            this.btnLeft_Leg.TabIndex = 24;
            this.btnLeft_Leg.Text = "◄";
            this.btnLeft_Leg.UseCompatibleTextRendering = true;
            this.btnLeft_Leg.UseVisualStyleBackColor = true;
            // 
            // btnRight_Leg
            // 
            this.btnRight_Leg.Location = new System.Drawing.Point(314, 353);
            this.btnRight_Leg.Name = "btnRight_Leg";
            this.btnRight_Leg.Size = new System.Drawing.Size(20, 19);
            this.btnRight_Leg.TabIndex = 23;
            this.btnRight_Leg.Text = "►";
            this.btnRight_Leg.UseCompatibleTextRendering = true;
            this.btnRight_Leg.UseVisualStyleBackColor = true;
            // 
            // lblLegend
            // 
            this.lblLegend.AutoSize = true;
            this.lblLegend.BackColor = System.Drawing.Color.Transparent;
            this.lblLegend.Location = new System.Drawing.Point(187, 356);
            this.lblLegend.Name = "lblLegend";
            this.lblLegend.Size = new System.Drawing.Size(72, 13);
            this.lblLegend.TabIndex = 22;
            this.lblLegend.Text = "Label (X Axis)";
            // 
            // DlgParetoGraph
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 425);
            this.Controls.Add(this.lvLabel);
            this.Controls.Add(this.btnLeft_Leg);
            this.Controls.Add(this.btnRight_Leg);
            this.Controls.Add(this.lblLegend);
            this.Controls.Add(this.btnLeft_Ser);
            this.Controls.Add(this.btnRight_Ser);
            this.Controls.Add(this.pictureBoxSample);
            this.Controls.Add(this.lblSample);
            this.Controls.Add(this.lblColumns);
            this.Controls.Add(this.lvColumns);
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
            this.Name = "DlgParetoGraph";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
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
        private System.Windows.Forms.ListView lvSeries;
        private System.Windows.Forms.PictureBox pictureBoxSample;
        private System.Windows.Forms.GroupBox grbOption;
        private System.Windows.Forms.Label lblSample;
        private System.Windows.Forms.CheckBox chkSerLegBox;
        private System.Windows.Forms.Button btnLeft_Ser;
        private System.Windows.Forms.Button btnRight_Ser;
        private System.Windows.Forms.TextBox txtAxisYTitle;
        private System.Windows.Forms.TextBox txtAxisXTitle;
        private System.Windows.Forms.Label lblAxisYTitle;
        private System.Windows.Forms.Label lblAxisXTitle;
        private System.Windows.Forms.CheckBox chkView3D;
        private System.Windows.Forms.ImageList imlColumnType;
        private System.Windows.Forms.ColumnHeader colType_Ser;
        private System.Windows.Forms.ColumnHeader colID_Ser;
        private System.Windows.Forms.ColumnHeader colName_Ser;
        private System.Windows.Forms.NumericUpDown nudDecimalPlace;
        private System.Windows.Forms.Label lblDateTimeFormat;
        private System.Windows.Forms.Label lblDecimalPlace;
        private System.Windows.Forms.Label lblLabelAngle;
        private System.Windows.Forms.Label lblBarSize;
        private System.Windows.Forms.NumericUpDown nudBarSize;
        private System.Windows.Forms.ComboBox cboDateTimeFormat;
        private System.Windows.Forms.ComboBox cboLabelAngle;
        private System.Windows.Forms.CheckBox chkPointLabel;
        private System.Windows.Forms.ListView lvLabel;
        private System.Windows.Forms.ColumnHeader colType_Leg;
        private System.Windows.Forms.ColumnHeader colID_Leg;
        private System.Windows.Forms.ColumnHeader colName_Leg;
        private System.Windows.Forms.Button btnLeft_Leg;
        private System.Windows.Forms.Button btnRight_Leg;
        private System.Windows.Forms.Label lblLegend;
    }
}