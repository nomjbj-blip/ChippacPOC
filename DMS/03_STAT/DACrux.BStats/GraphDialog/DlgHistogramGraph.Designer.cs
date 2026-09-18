namespace DACrux.BStats.GraphDialog
{
    partial class DlgHistogramGraph
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgHistogramGraph));
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
            this.lblSample = new System.Windows.Forms.Label();
            this.btnLeft_Ser = new System.Windows.Forms.Button();
            this.btnRight_Ser = new System.Windows.Forms.Button();
            this.imlColumnType = new System.Windows.Forms.ImageList(this.components);
            this.lblLsl = new System.Windows.Forms.Label();
            this.nudLSL = new System.Windows.Forms.NumericUpDown();
            this.lblUSL = new System.Windows.Forms.Label();
            this.nudUSL = new System.Windows.Forms.NumericUpDown();
            this.lblAxisXTitle = new System.Windows.Forms.Label();
            this.lblAxisYTitle = new System.Windows.Forms.Label();
            this.txtAxisXTitle = new System.Windows.Forms.TextBox();
            this.txtAxisYTitle = new System.Windows.Forms.TextBox();
            this.chkFrequence = new System.Windows.Forms.CheckBox();
            this.chkGridLine = new System.Windows.Forms.CheckBox();
            this.chk3SigmaLine = new System.Windows.Forms.CheckBox();
            this.chkSpecLimit = new System.Windows.Forms.CheckBox();
            this.chkNormalLine = new System.Windows.Forms.CheckBox();
            this.grbOption = new System.Windows.Forms.GroupBox();
            this.lblDecimalPlace = new System.Windows.Forms.Label();
            this.nudDecimalPlace = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSample)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLSL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUSL)).BeginInit();
            this.grbOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDecimalPlace)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(522, 363);
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
            this.btnCancel.Location = new System.Drawing.Point(592, 363);
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
            this.pictureBoxSample.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxSample.Location = new System.Drawing.Point(343, 153);
            this.pictureBoxSample.Name = "pictureBoxSample";
            this.pictureBoxSample.Size = new System.Drawing.Size(313, 194);
            this.pictureBoxSample.TabIndex = 15;
            this.pictureBoxSample.TabStop = false;
            this.pictureBoxSample.Click += new System.EventHandler(this.pictureBoxSample_Click);
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
            // lblLsl
            // 
            this.lblLsl.AutoSize = true;
            this.lblLsl.Location = new System.Drawing.Point(243, 26);
            this.lblLsl.Name = "lblLsl";
            this.lblLsl.Size = new System.Drawing.Size(23, 13);
            this.lblLsl.TabIndex = 30;
            this.lblLsl.Text = "LSL";
            // 
            // nudLSL
            // 
            this.nudLSL.Location = new System.Drawing.Point(293, 24);
            this.nudLSL.Maximum = new decimal(new int[] {
            -1981284353,
            -1966660860,
            0,
            0});
            this.nudLSL.Minimum = new decimal(new int[] {
            -1981284353,
            -1966660860,
            0,
            -2147483648});
            this.nudLSL.Name = "nudLSL";
            this.nudLSL.Size = new System.Drawing.Size(77, 21);
            this.nudLSL.TabIndex = 29;
            // 
            // lblUSL
            // 
            this.lblUSL.AutoSize = true;
            this.lblUSL.Location = new System.Drawing.Point(243, 49);
            this.lblUSL.Name = "lblUSL";
            this.lblUSL.Size = new System.Drawing.Size(25, 13);
            this.lblUSL.TabIndex = 32;
            this.lblUSL.Text = "USL";
            // 
            // nudUSL
            // 
            this.nudUSL.Location = new System.Drawing.Point(293, 47);
            this.nudUSL.Maximum = new decimal(new int[] {
            -1981284353,
            -1966660860,
            0,
            0});
            this.nudUSL.Minimum = new decimal(new int[] {
            -1981284353,
            -1966660860,
            0,
            -2147483648});
            this.nudUSL.Name = "nudUSL";
            this.nudUSL.Size = new System.Drawing.Size(77, 21);
            this.nudUSL.TabIndex = 31;
            // 
            // lblAxisXTitle
            // 
            this.lblAxisXTitle.AutoSize = true;
            this.lblAxisXTitle.Location = new System.Drawing.Point(23, 28);
            this.lblAxisXTitle.Name = "lblAxisXTitle";
            this.lblAxisXTitle.Size = new System.Drawing.Size(59, 13);
            this.lblAxisXTitle.TabIndex = 44;
            this.lblAxisXTitle.Text = "X Axis Title";
            // 
            // lblAxisYTitle
            // 
            this.lblAxisYTitle.AutoSize = true;
            this.lblAxisYTitle.Location = new System.Drawing.Point(23, 52);
            this.lblAxisYTitle.Name = "lblAxisYTitle";
            this.lblAxisYTitle.Size = new System.Drawing.Size(59, 13);
            this.lblAxisYTitle.TabIndex = 45;
            this.lblAxisYTitle.Text = "Y Axis Title";
            // 
            // txtAxisXTitle
            // 
            this.txtAxisXTitle.Location = new System.Drawing.Point(102, 25);
            this.txtAxisXTitle.Name = "txtAxisXTitle";
            this.txtAxisXTitle.Size = new System.Drawing.Size(100, 21);
            this.txtAxisXTitle.TabIndex = 46;
            // 
            // txtAxisYTitle
            // 
            this.txtAxisYTitle.Location = new System.Drawing.Point(102, 49);
            this.txtAxisYTitle.Name = "txtAxisYTitle";
            this.txtAxisYTitle.Size = new System.Drawing.Size(100, 21);
            this.txtAxisYTitle.TabIndex = 47;
            // 
            // chkFrequence
            // 
            this.chkFrequence.AutoSize = true;
            this.chkFrequence.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkFrequence.Location = new System.Drawing.Point(530, 24);
            this.chkFrequence.Name = "chkFrequence";
            this.chkFrequence.Size = new System.Drawing.Size(74, 17);
            this.chkFrequence.TabIndex = 59;
            this.chkFrequence.Text = "Frequence";
            this.chkFrequence.UseVisualStyleBackColor = true;
            // 
            // chkGridLine
            // 
            this.chkGridLine.AutoSize = true;
            this.chkGridLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkGridLine.Location = new System.Drawing.Point(530, 47);
            this.chkGridLine.Name = "chkGridLine";
            this.chkGridLine.Size = new System.Drawing.Size(64, 17);
            this.chkGridLine.TabIndex = 58;
            this.chkGridLine.Text = "Grid Line";
            this.chkGridLine.UseVisualStyleBackColor = true;
            // 
            // chk3SigmaLine
            // 
            this.chk3SigmaLine.AutoSize = true;
            this.chk3SigmaLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chk3SigmaLine.Location = new System.Drawing.Point(417, 71);
            this.chk3SigmaLine.Name = "chk3SigmaLine";
            this.chk3SigmaLine.Size = new System.Drawing.Size(82, 17);
            this.chk3SigmaLine.TabIndex = 57;
            this.chk3SigmaLine.Text = "3 Sigma Line";
            this.chk3SigmaLine.UseVisualStyleBackColor = true;
            // 
            // chkSpecLimit
            // 
            this.chkSpecLimit.AutoSize = true;
            this.chkSpecLimit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkSpecLimit.Location = new System.Drawing.Point(417, 47);
            this.chkSpecLimit.Name = "chkSpecLimit";
            this.chkSpecLimit.Size = new System.Drawing.Size(70, 17);
            this.chkSpecLimit.TabIndex = 56;
            this.chkSpecLimit.Text = "Spec Limit";
            this.chkSpecLimit.UseVisualStyleBackColor = true;
            // 
            // chkNormalLine
            // 
            this.chkNormalLine.AutoSize = true;
            this.chkNormalLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkNormalLine.Location = new System.Drawing.Point(417, 24);
            this.chkNormalLine.Name = "chkNormalLine";
            this.chkNormalLine.Size = new System.Drawing.Size(78, 17);
            this.chkNormalLine.TabIndex = 55;
            this.chkNormalLine.Text = "Normal Line";
            this.chkNormalLine.UseVisualStyleBackColor = true;
            // 
            // grbOption
            // 
            this.grbOption.Controls.Add(this.lblDecimalPlace);
            this.grbOption.Controls.Add(this.txtAxisXTitle);
            this.grbOption.Controls.Add(this.nudDecimalPlace);
            this.grbOption.Controls.Add(this.chkFrequence);
            this.grbOption.Controls.Add(this.lblAxisXTitle);
            this.grbOption.Controls.Add(this.chkGridLine);
            this.grbOption.Controls.Add(this.txtAxisYTitle);
            this.grbOption.Controls.Add(this.chk3SigmaLine);
            this.grbOption.Controls.Add(this.lblAxisYTitle);
            this.grbOption.Controls.Add(this.chkNormalLine);
            this.grbOption.Controls.Add(this.chkSpecLimit);
            this.grbOption.Controls.Add(this.lblLsl);
            this.grbOption.Controls.Add(this.nudLSL);
            this.grbOption.Controls.Add(this.nudUSL);
            this.grbOption.Controls.Add(this.lblUSL);
            this.grbOption.Location = new System.Drawing.Point(12, 11);
            this.grbOption.Name = "grbOption";
            this.grbOption.Size = new System.Drawing.Size(644, 110);
            this.grbOption.TabIndex = 60;
            this.grbOption.TabStop = false;
            this.grbOption.Text = "Option";
            // 
            // lblDecimalPlace
            // 
            this.lblDecimalPlace.AutoSize = true;
            this.lblDecimalPlace.Location = new System.Drawing.Point(23, 77);
            this.lblDecimalPlace.Name = "lblDecimalPlace";
            this.lblDecimalPlace.Size = new System.Drawing.Size(71, 13);
            this.lblDecimalPlace.TabIndex = 62;
            this.lblDecimalPlace.Text = "Decimal Place";
            // 
            // nudDecimalPlace
            // 
            this.nudDecimalPlace.Location = new System.Drawing.Point(102, 73);
            this.nudDecimalPlace.Name = "nudDecimalPlace";
            this.nudDecimalPlace.Size = new System.Drawing.Size(100, 21);
            this.nudDecimalPlace.TabIndex = 61;
            this.nudDecimalPlace.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // DlgHistogramGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 400);
            this.Controls.Add(this.grbOption);
            this.Controls.Add(this.btnLeft_Ser);
            this.Controls.Add(this.btnRight_Ser);
            this.Controls.Add(this.pictureBoxSample);
            this.Controls.Add(this.lblSample);
            this.Controls.Add(this.lblColumns);
            this.Controls.Add(this.lvColumns);
            this.Controls.Add(this.lblSeries);
            this.Controls.Add(this.lvSeries);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgHistogramGraph";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSample)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLSL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUSL)).EndInit();
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
        private System.Windows.Forms.Label lblSample;
        private System.Windows.Forms.Button btnLeft_Ser;
        private System.Windows.Forms.Button btnRight_Ser;
        private System.Windows.Forms.ImageList imlColumnType;
        private System.Windows.Forms.ColumnHeader colType_Ser;
        private System.Windows.Forms.ColumnHeader colID_Ser;
        private System.Windows.Forms.ColumnHeader colName_Ser;
        private System.Windows.Forms.Label lblUSL;
        private System.Windows.Forms.NumericUpDown nudUSL;
        private System.Windows.Forms.Label lblLsl;
        private System.Windows.Forms.NumericUpDown nudLSL;
        private System.Windows.Forms.Label lblAxisXTitle;
        private System.Windows.Forms.Label lblAxisYTitle;
        private System.Windows.Forms.TextBox txtAxisXTitle;
        private System.Windows.Forms.TextBox txtAxisYTitle;
        private System.Windows.Forms.CheckBox chkSpecLimit;
        private System.Windows.Forms.CheckBox chkNormalLine;
        private System.Windows.Forms.CheckBox chkFrequence;
        private System.Windows.Forms.CheckBox chkGridLine;
        private System.Windows.Forms.CheckBox chk3SigmaLine;
        private System.Windows.Forms.GroupBox grbOption;
        private System.Windows.Forms.Label lblDecimalPlace;
        private System.Windows.Forms.NumericUpDown nudDecimalPlace;
    }
}