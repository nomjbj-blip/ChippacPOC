namespace DACrux.BStats.GraphDialog
{
    partial class DlgBoxPlotGraph
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgBoxPlotGraph));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lvColumns = new System.Windows.Forms.ListView();
            this.colType = new System.Windows.Forms.ColumnHeader("(none)");
            this.colID = new System.Windows.Forms.ColumnHeader();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.lblSeries = new System.Windows.Forms.Label();
            this.lblColumns = new System.Windows.Forms.Label();
            this.lvSeries = new System.Windows.Forms.ListView();
            this.colType_Ser = new System.Windows.Forms.ColumnHeader();
            this.colID_Ser = new System.Windows.Forms.ColumnHeader();
            this.colName_Ser = new System.Windows.Forms.ColumnHeader();
            this.pictureBoxSample = new System.Windows.Forms.PictureBox();
            this.grbOption = new System.Windows.Forms.GroupBox();
            this.chkPointLabel = new System.Windows.Forms.CheckBox();
            this.cboLabelAngle = new System.Windows.Forms.ComboBox();
            this.lblDecimalPlace = new System.Windows.Forms.Label();
            this.lblLabelAngle = new System.Windows.Forms.Label();
            this.nudDecimalPlace = new System.Windows.Forms.NumericUpDown();
            this.txtAxisYTitle = new System.Windows.Forms.TextBox();
            this.txtAxisXTitle = new System.Windows.Forms.TextBox();
            this.lblAxisYTitle = new System.Windows.Forms.Label();
            this.lblAxisXTitle = new System.Windows.Forms.Label();
            this.lblSample = new System.Windows.Forms.Label();
            this.btnLeft_Ser = new System.Windows.Forms.Button();
            this.btnRight_Ser = new System.Windows.Forms.Button();
            this.imlColumnType = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSample)).BeginInit();
            this.grbOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDecimalPlace)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(522, 325);
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
            this.btnCancel.Location = new System.Drawing.Point(592, 325);
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
            this.lvColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lvColumns.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvColumns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colType,
            this.colID,
            this.colName});
            this.lvColumns.FullRowSelect = true;
            this.lvColumns.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvColumns.Location = new System.Drawing.Point(12, 115);
            this.lvColumns.Name = "lvColumns";
            this.lvColumns.Size = new System.Drawing.Size(145, 194);
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
            this.lblSeries.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSeries.AutoSize = true;
            this.lblSeries.Location = new System.Drawing.Point(186, 99);
            this.lblSeries.Name = "lblSeries";
            this.lblSeries.Size = new System.Drawing.Size(73, 13);
            this.lblSeries.TabIndex = 11;
            this.lblSeries.Text = "Series (Value)";
            // 
            // lblColumns
            // 
            this.lblColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblColumns.AutoSize = true;
            this.lblColumns.Location = new System.Drawing.Point(9, 99);
            this.lblColumns.Name = "lblColumns";
            this.lblColumns.Size = new System.Drawing.Size(47, 13);
            this.lblColumns.TabIndex = 12;
            this.lblColumns.Text = "Columns";
            // 
            // lvSeries
            // 
            this.lvSeries.AllowDrop = true;
            this.lvSeries.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lvSeries.BackColor = System.Drawing.Color.Lavender;
            this.lvSeries.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvSeries.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colType_Ser,
            this.colID_Ser,
            this.colName_Ser});
            this.lvSeries.FullRowSelect = true;
            this.lvSeries.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvSeries.Location = new System.Drawing.Point(189, 115);
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
            this.pictureBoxSample.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxSample.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBoxSample.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBoxSample.BackgroundImage")));
            this.pictureBoxSample.Location = new System.Drawing.Point(343, 115);
            this.pictureBoxSample.Name = "pictureBoxSample";
            this.pictureBoxSample.Size = new System.Drawing.Size(313, 194);
            this.pictureBoxSample.TabIndex = 15;
            this.pictureBoxSample.TabStop = false;
            // 
            // grbOption
            // 
            this.grbOption.Controls.Add(this.chkPointLabel);
            this.grbOption.Controls.Add(this.cboLabelAngle);
            this.grbOption.Controls.Add(this.lblDecimalPlace);
            this.grbOption.Controls.Add(this.lblLabelAngle);
            this.grbOption.Controls.Add(this.nudDecimalPlace);
            this.grbOption.Controls.Add(this.txtAxisYTitle);
            this.grbOption.Controls.Add(this.txtAxisXTitle);
            this.grbOption.Controls.Add(this.lblAxisYTitle);
            this.grbOption.Controls.Add(this.lblAxisXTitle);
            this.grbOption.Location = new System.Drawing.Point(12, 11);
            this.grbOption.Name = "grbOption";
            this.grbOption.Size = new System.Drawing.Size(644, 76);
            this.grbOption.TabIndex = 8;
            this.grbOption.TabStop = false;
            this.grbOption.Text = "Option";
            // 
            // chkPointLabel
            // 
            this.chkPointLabel.AutoSize = true;
            this.chkPointLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkPointLabel.Location = new System.Drawing.Point(496, 20);
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
            // nudDecimalPlace
            // 
            this.nudDecimalPlace.Location = new System.Drawing.Point(344, 45);
            this.nudDecimalPlace.Name = "nudDecimalPlace";
            this.nudDecimalPlace.Size = new System.Drawing.Size(100, 21);
            this.nudDecimalPlace.TabIndex = 18;
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
            // lblSample
            // 
            this.lblSample.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSample.AutoSize = true;
            this.lblSample.Location = new System.Drawing.Point(340, 99);
            this.lblSample.Name = "lblSample";
            this.lblSample.Size = new System.Drawing.Size(41, 13);
            this.lblSample.TabIndex = 16;
            this.lblSample.Text = "Sample";
            // 
            // btnLeft_Ser
            // 
            this.btnLeft_Ser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLeft_Ser.Location = new System.Drawing.Point(163, 140);
            this.btnLeft_Ser.Name = "btnLeft_Ser";
            this.btnLeft_Ser.Size = new System.Drawing.Size(20, 19);
            this.btnLeft_Ser.TabIndex = 18;
            this.btnLeft_Ser.Text = "◄";
            this.btnLeft_Ser.UseCompatibleTextRendering = true;
            this.btnLeft_Ser.UseVisualStyleBackColor = true;
            // 
            // btnRight_Ser
            // 
            this.btnRight_Ser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRight_Ser.Location = new System.Drawing.Point(163, 116);
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
            // DlgBoxPlotGraph
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 362);
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
            this.Name = "DlgBoxPlotGraph";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSample)).EndInit();
            this.grbOption.ResumeLayout(false);
            this.grbOption.PerformLayout();
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
        private System.Windows.Forms.Button btnLeft_Ser;
        private System.Windows.Forms.Button btnRight_Ser;
        private System.Windows.Forms.TextBox txtAxisYTitle;
        private System.Windows.Forms.TextBox txtAxisXTitle;
        private System.Windows.Forms.Label lblAxisYTitle;
        private System.Windows.Forms.Label lblAxisXTitle;
        private System.Windows.Forms.ImageList imlColumnType;
        private System.Windows.Forms.ColumnHeader colType_Ser;
        private System.Windows.Forms.ColumnHeader colID_Ser;
        private System.Windows.Forms.ColumnHeader colName_Ser;
        private System.Windows.Forms.NumericUpDown nudDecimalPlace;
        private System.Windows.Forms.Label lblDecimalPlace;
        private System.Windows.Forms.Label lblLabelAngle;
        private System.Windows.Forms.ComboBox cboLabelAngle;
        private System.Windows.Forms.CheckBox chkPointLabel;
    }
}