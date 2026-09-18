namespace DACrux.TEST.ENGUI
{
    partial class frmSetupShot
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSetupShot));
            this.panel2 = new System.Windows.Forms.Panel();
            this.butSave = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.butLoad = new System.Windows.Forms.Button();
            this.shotMap1 = new DACrux.Map.ShotMap();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbDevice = new System.Windows.Forms.ComboBox();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.cmbDevice);
            this.panel2.Controls.Add(this.butSave);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.butLoad);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(832, 48);
            this.panel2.TabIndex = 56;
            // 
            // butSave
            // 
            this.butSave.BackColor = System.Drawing.Color.LightSteelBlue;
            this.butSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butSave.ForeColor = System.Drawing.Color.Transparent;
            this.butSave.Image = ((System.Drawing.Image)(resources.GetObject("butSave.Image")));
            this.butSave.Location = new System.Drawing.Point(327, 22);
            this.butSave.Name = "butSave";
            this.butSave.Size = new System.Drawing.Size(61, 23);
            this.butSave.TabIndex = 64;
            this.butSave.UseVisualStyleBackColor = false;
            this.butSave.Click += new System.EventHandler(this.butSave_Click);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.LightSlateGray;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Image = ((System.Drawing.Image)(resources.GetObject("label6.Image")));
            this.label6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(832, 20);
            this.label6.TabIndex = 54;
            this.label6.Text = "    Shot Define";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // butLoad
            // 
            this.butLoad.BackColor = System.Drawing.Color.LightSteelBlue;
            this.butLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butLoad.ForeColor = System.Drawing.Color.Transparent;
            this.butLoad.Image = ((System.Drawing.Image)(resources.GetObject("butLoad.Image")));
            this.butLoad.Location = new System.Drawing.Point(261, 22);
            this.butLoad.Name = "butLoad";
            this.butLoad.Size = new System.Drawing.Size(61, 23);
            this.butLoad.TabIndex = 64;
            this.butLoad.UseVisualStyleBackColor = false;
            this.butLoad.Click += new System.EventHandler(this.butLoad_Click);
            // 
            // shotMap1
            // 
            this.shotMap1.AngleOffSet = 0;
            this.shotMap1.CenterMark = false;
            this.shotMap1.Cursor = System.Windows.Forms.Cursors.Cross;
            this.shotMap1.DataSource = null;
            this.shotMap1.DieBorderColor = System.Drawing.Color.LightGray;
            this.shotMap1.DieFocusingType = DACrux.Map.FocusType.Arraw;
            this.shotMap1.DieMaxX = -65536;
            this.shotMap1.DieMaxY = -65536;
            this.shotMap1.DieMinX = 65536;
            this.shotMap1.DieMinY = 65536;
            this.shotMap1.DieSizeX = 0.01D;
            this.shotMap1.DieSizeY = 0.01D;
            this.shotMap1.DisplayValue = "BIN";
            this.shotMap1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shotMap1.DrawFirstDie = true;
            this.shotMap1.DrawMarkDie = false;
            this.shotMap1.DrawOriginDie = true;
            this.shotMap1.DrawSkipDie = true;
            this.shotMap1.EdgeColor = System.Drawing.Color.LightGray;
            this.shotMap1.EdgeSize = 1D;
            this.shotMap1.FirstDieBorderColor = System.Drawing.Color.SkyBlue;
            this.shotMap1.FirstDieX = 0;
            this.shotMap1.FirstDieY = 0;
            this.shotMap1.ForeColor = System.Drawing.Color.Red;
            this.shotMap1.Location = new System.Drawing.Point(0, 48);
            this.shotMap1.MarkDieColor = System.Drawing.Color.LightSkyBlue;
            this.shotMap1.Name = "shotMap1";
            this.shotMap1.NotchAngle = 0;
            this.shotMap1.NotchType = DACrux.Base.Notch.Flat;
            this.shotMap1.OriginDieBorder = System.Drawing.Color.Red;
            this.shotMap1.OriginIndexX = 0;
            this.shotMap1.OriginIndexY = 0;
            this.shotMap1.OriginX = 0D;
            this.shotMap1.OriginY = 0D;
            this.shotMap1.PickupDieAlpha = 96;
            this.shotMap1.PickupedDieColor = System.Drawing.Color.Transparent;
            this.shotMap1.PopupMenu = true;
            this.shotMap1.ReferenceDieSetting = 0;
            this.shotMap1.ScaleMark = false;
            this.shotMap1.SelecetedBin = "ALL";
            this.shotMap1.SelectedVI = "ALL";
            this.shotMap1.Size = new System.Drawing.Size(832, 542);
            this.shotMap1.SkipDieColor = System.Drawing.Color.Yellow;
            this.shotMap1.TabIndex = 57;
            this.shotMap1.TransParent = 255;
            this.shotMap1.ViewAngle = 0;
            this.shotMap1.VIMember = "VIFAIL";
            this.shotMap1.VisibleDieBorder = true;
            this.shotMap1.VisibleDieValue = false;
            this.shotMap1.VisibleFocusDie = false;
            this.shotMap1.VisibleInfomation = true;
            this.shotMap1.VisibleOffDie = false;
            this.shotMap1.VisibleStringBin = false;
            this.shotMap1.VisibleVIFail = false;
            this.shotMap1.VisibleXY = false;
            this.shotMap1.WaferBorderColor = System.Drawing.Color.LightGray;
            this.shotMap1.WaferColor = System.Drawing.Color.Gray;
            this.shotMap1.WaferDrawMode = DACrux.Map.MapMode.Fit;
            this.shotMap1.WaferMargin = 0.95D;
            this.shotMap1.WaferSize = 200000D;
            this.shotMap1.XYDirect = DACrux.Base.XYDirection.LeftTop;
            // 
            // label3
            // 
            this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
            this.label3.Location = new System.Drawing.Point(4, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 20);
            this.label3.TabIndex = 66;
            // 
            // cmbDevice
            // 
            this.cmbDevice.Location = new System.Drawing.Point(108, 23);
            this.cmbDevice.Name = "cmbDevice";
            this.cmbDevice.Size = new System.Drawing.Size(152, 20);
            this.cmbDevice.TabIndex = 65;
            this.cmbDevice.SelectedIndexChanged += new System.EventHandler(this.cmbDevice_SelectedIndexChanged);
            // 
            // frmSetupShot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 590);
            this.Controls.Add(this.shotMap1);
            this.Controls.Add(this.panel2);
            this.Name = "frmSetupShot";
            this.Text = "frmSetupShot";
            this.Load += new System.EventHandler(this.frmSetupShot_Load);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button butSave;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button butLoad;
        private Map.ShotMap shotMap1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbDevice;
    }
}