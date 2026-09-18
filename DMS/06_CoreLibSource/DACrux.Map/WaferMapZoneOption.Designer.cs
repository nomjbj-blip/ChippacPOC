namespace DACrux.Map
{
    partial class WaferMapZoneOption
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WaferMapZoneOption));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.map = new DACrux.Map.DefectMap();
            this.panel3 = new System.Windows.Forms.Panel();
            this.chkDisplayZoneID = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboOrientation = new System.Windows.Forms.ComboBox();
            this.numExcludesWaferEdge = new System.Windows.Forms.NumericUpDown();
            this.numAngle = new System.Windows.Forms.NumericUpDown();
            this.numRadialZone = new System.Windows.Forms.NumericUpDown();
            this.numAnnularZone = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdoSameArea = new System.Windows.Forms.RadioButton();
            this.rdoSameRadius = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numExcludesWaferEdge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRadialZone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAnnularZone)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 354);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(662, 41);
            this.panel1.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(575, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(494, 9);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(662, 354);
            this.panel2.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.map);
            this.groupBox2.Controls.Add(this.panel3);
            this.groupBox2.Location = new System.Drawing.Point(362, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(290, 345);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // map
            // 
            this.map.AngleOffSet = 0;
            this.map.CenterMark = false;
            this.map.Cursor = System.Windows.Forms.Cursors.Cross;
            this.map.DataSource = null;
            this.map.DieBackgroundColor = System.Drawing.Color.Black;
            this.map.DieBorderColor = System.Drawing.Color.LightGray;
            this.map.DieDefectColor = System.Drawing.Color.Empty;
            this.map.DieFocusingType = DACrux.Map.FocusType.Arraw;
            this.map.DieMaxX = -65536;
            this.map.DieMaxY = -65536;
            this.map.DieMinX = 65536;
            this.map.DieMinY = 65536;
            this.map.DieSizeX = 0.01D;
            this.map.DieSizeY = 0.01D;
            this.map.DisplayDieValue = DACrux.Base.DieDisplayValue.Bin;
            this.map.DisplayValue = "BIN";
            this.map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map.DrawDefectImage = null;
            this.map.DrawDefects = "ALL";
            this.map.DrawFirstDie = false;
            this.map.DrawMarkDie = true;
            this.map.DrawOriginDie = true;
            this.map.DrawSkipDie = true;
            this.map.EdgeColor = System.Drawing.Color.LightGray;
            this.map.EdgeSize = 1D;
            this.map.FirstDieBorderColor = System.Drawing.Color.SkyBlue;
            this.map.FirstDieX = 0;
            this.map.FirstDieY = 0;
            this.map.ForeColor = System.Drawing.Color.Red;
            this.map.FromGradationDieColor = System.Drawing.Color.Lime;
            this.map.GradationInterval = 5;
            this.map.GradationMaxValue = double.NaN;
            this.map.GradationMinValue = double.NaN;
            this.map.Location = new System.Drawing.Point(3, 17);
            this.map.MapType = DACrux.Base.MAP_TYPE.CLASS;
            this.map.MarkDieColor = System.Drawing.Color.LightSkyBlue;
            this.map.Name = "map";
            this.map.NotchAngle = 0;
            this.map.NotchType = DACrux.Base.Notch.Notch;
            this.map.OriginDieBorder = System.Drawing.Color.Red;
            this.map.OriginIndexX = 0;
            this.map.OriginIndexY = 0;
            this.map.OriginX = 0D;
            this.map.OriginY = 0D;
            this.map.ParaLimit = false;
            this.map.ParametricColumn = "";
            this.map.ParaValueFont = new System.Drawing.Font("굴림", 9F);
            this.map.PickupDieAlpha = 96;
            this.map.PickupedDieColor = System.Drawing.Color.Transparent;
            this.map.PopupMenu = true;
            this.map.ReferenceDieSetting = 0;
            this.map.ScaleMark = false;
            this.map.SelecetedBin = "ALL";
            this.map.SelectedVI = "ALL";
            this.map.Size = new System.Drawing.Size(284, 297);
            this.map.SkipDieColor = System.Drawing.Color.Yellow;
            this.map.TabIndex = 1;
            this.map.ToGradationDieColor = System.Drawing.Color.Red;
            this.map.TransParent = 255;
            this.map.UseZoneIDSelectControlPopup = false;
            this.map.ViewAngle = 0;
            this.map.VIMember = "VIFAIL";
            this.map.VisibleDieBorder = true;
            this.map.VisibleDieValue = false;
            this.map.VisibleFocusDie = false;
            this.map.VisibleImageMark = false;
            this.map.VisibleInfomation = true;
            this.map.VisibleOffDie = false;
            this.map.VisibleProbeOverlay = false;
            this.map.VisibleShotAlignPoint = false;
            this.map.VisibleSignDies = false;
            this.map.VisibleStringBin = false;
            this.map.VisibleVIFail = false;
            this.map.VisibleXY = false;
            this.map.VisibleZone = true;
            this.map.WaferBorderColor = System.Drawing.Color.LightGray;
            this.map.WaferColor = System.Drawing.Color.Silver;
            this.map.WaferDrawMode = DACrux.Map.MapMode.Free;
            this.map.WaferID = "";
            this.map.WaferMargin = 0.95D;
            this.map.WaferSize = 200000D;
            this.map.XYDirect = DACrux.Base.XYDirection.LeftTop;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.chkDisplayZoneID);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(3, 314);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(284, 28);
            this.panel3.TabIndex = 0;
            // 
            // chkDisplayZoneID
            // 
            this.chkDisplayZoneID.AutoSize = true;
            this.chkDisplayZoneID.Location = new System.Drawing.Point(9, 6);
            this.chkDisplayZoneID.Name = "chkDisplayZoneID";
            this.chkDisplayZoneID.Size = new System.Drawing.Size(114, 16);
            this.chkDisplayZoneID.TabIndex = 5;
            this.chkDisplayZoneID.Text = "Display Zone ID";
            this.chkDisplayZoneID.UseVisualStyleBackColor = true;
            this.chkDisplayZoneID.CheckedChanged += new System.EventHandler(this.chkDisplayZoneID_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.cboOrientation);
            this.groupBox1.Controls.Add(this.numExcludesWaferEdge);
            this.groupBox1.Controls.Add(this.numAngle);
            this.groupBox1.Controls.Add(this.numRadialZone);
            this.groupBox1.Controls.Add(this.numAnnularZone);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Location = new System.Drawing.Point(12, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(344, 345);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // cboOrientation
            // 
            this.cboOrientation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOrientation.FormattingEnabled = true;
            this.cboOrientation.Location = new System.Drawing.Point(202, 285);
            this.cboOrientation.Name = "cboOrientation";
            this.cboOrientation.Size = new System.Drawing.Size(73, 20);
            this.cboOrientation.TabIndex = 4;
            this.cboOrientation.SelectedIndexChanged += new System.EventHandler(this.cboOrientation_SelectedIndexChanged);
            // 
            // numExcludesWaferEdge
            // 
            this.numExcludesWaferEdge.Location = new System.Drawing.Point(202, 241);
            this.numExcludesWaferEdge.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numExcludesWaferEdge.Name = "numExcludesWaferEdge";
            this.numExcludesWaferEdge.Size = new System.Drawing.Size(73, 21);
            this.numExcludesWaferEdge.TabIndex = 3;
            this.numExcludesWaferEdge.ValueChanged += new System.EventHandler(this.NumericUpDown_ValueChanged);
            // 
            // numAngle
            // 
            this.numAngle.Location = new System.Drawing.Point(202, 195);
            this.numAngle.Maximum = new decimal(new int[] {
            359,
            0,
            0,
            0});
            this.numAngle.Name = "numAngle";
            this.numAngle.Size = new System.Drawing.Size(73, 21);
            this.numAngle.TabIndex = 3;
            this.numAngle.ValueChanged += new System.EventHandler(this.NumericUpDown_ValueChanged);
            // 
            // numRadialZone
            // 
            this.numRadialZone.Location = new System.Drawing.Point(202, 152);
            this.numRadialZone.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numRadialZone.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRadialZone.Name = "numRadialZone";
            this.numRadialZone.Size = new System.Drawing.Size(73, 21);
            this.numRadialZone.TabIndex = 3;
            this.numRadialZone.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRadialZone.ValueChanged += new System.EventHandler(this.NumericUpDown_ValueChanged);
            // 
            // numAnnularZone
            // 
            this.numAnnularZone.Location = new System.Drawing.Point(202, 109);
            this.numAnnularZone.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numAnnularZone.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numAnnularZone.Name = "numAnnularZone";
            this.numAnnularZone.Size = new System.Drawing.Size(73, 21);
            this.numAnnularZone.TabIndex = 3;
            this.numAnnularZone.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numAnnularZone.ValueChanged += new System.EventHandler(this.NumericUpDown_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(284, 244);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "um";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(282, 199);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "degrees";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(141, 197);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "Angle:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(113, 291);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "Orientation:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(40, 245);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(126, 12);
            this.label7.TabIndex = 1;
            this.label7.Text = "Excludes Wafer Edge";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 154);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Number of radial zones:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Number of annular zones:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rdoSameArea);
            this.groupBox3.Controls.Add(this.rdoSameRadius);
            this.groupBox3.Location = new System.Drawing.Point(6, 20);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(324, 55);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Annular Size";
            // 
            // rdoSameArea
            // 
            this.rdoSameArea.AutoSize = true;
            this.rdoSameArea.Location = new System.Drawing.Point(183, 23);
            this.rdoSameArea.Name = "rdoSameArea";
            this.rdoSameArea.Size = new System.Drawing.Size(86, 16);
            this.rdoSameArea.TabIndex = 0;
            this.rdoSameArea.TabStop = true;
            this.rdoSameArea.Text = "Same Area";
            this.rdoSameArea.UseVisualStyleBackColor = true;
            this.rdoSameArea.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // rdoSameRadius
            // 
            this.rdoSameRadius.AutoSize = true;
            this.rdoSameRadius.Location = new System.Drawing.Point(25, 23);
            this.rdoSameRadius.Name = "rdoSameRadius";
            this.rdoSameRadius.Size = new System.Drawing.Size(99, 16);
            this.rdoSameRadius.TabIndex = 0;
            this.rdoSameRadius.TabStop = true;
            this.rdoSameRadius.Text = "Same Radius";
            this.rdoSameRadius.UseVisualStyleBackColor = true;
            this.rdoSameRadius.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // WaferMapZoneOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(662, 395);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WaferMapZoneOption";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Zonal Setup";
            this.Load += new System.EventHandler(this.WaferMapZoneOption_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numExcludesWaferEdge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRadialZone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAnnularZone)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdoSameArea;
        private System.Windows.Forms.RadioButton rdoSameRadius;
        private System.Windows.Forms.ComboBox cboOrientation;
        private System.Windows.Forms.NumericUpDown numExcludesWaferEdge;
        private System.Windows.Forms.NumericUpDown numAngle;
        private System.Windows.Forms.NumericUpDown numRadialZone;
        private System.Windows.Forms.NumericUpDown numAnnularZone;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox chkDisplayZoneID;
        private System.Windows.Forms.Label label7;
        private DACrux.Map.DefectMap map;
    }
}