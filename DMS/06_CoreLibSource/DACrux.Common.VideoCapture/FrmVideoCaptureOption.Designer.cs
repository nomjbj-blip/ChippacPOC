namespace DACrux.Common.VideoCapture
{
    partial class FrmVideoCaptureOption
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblDeviceName = new System.Windows.Forms.Label();
            this.lblDisplayResolution = new System.Windows.Forms.Label();
            this.lblImageResolution = new System.Windows.Forms.Label();
            this.lblImageFormat = new System.Windows.Forms.Label();
            this.cboDevices = new System.Windows.Forms.ComboBox();
            this.cboDisplayResolution = new System.Windows.Forms.ComboBox();
            this.cboImageResolution = new System.Windows.Forms.ComboBox();
            this.cboImageFormat = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkShowCrosshair = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 146);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(383, 37);
            this.panel1.TabIndex = 10;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(296, 7);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(205, 7);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblDeviceName
            // 
            this.lblDeviceName.AutoSize = true;
            this.lblDeviceName.Location = new System.Drawing.Point(53, 9);
            this.lblDeviceName.Name = "lblDeviceName";
            this.lblDeviceName.Size = new System.Drawing.Size(77, 12);
            this.lblDeviceName.TabIndex = 0;
            this.lblDeviceName.Text = "비디오 장치 :";
            // 
            // lblDisplayResolution
            // 
            this.lblDisplayResolution.AutoSize = true;
            this.lblDisplayResolution.Location = new System.Drawing.Point(24, 35);
            this.lblDisplayResolution.Name = "lblDisplayResolution";
            this.lblDisplayResolution.Size = new System.Drawing.Size(106, 12);
            this.lblDisplayResolution.TabIndex = 2;
            this.lblDisplayResolution.Text = "화면 해상도(DPI) :";
            // 
            // lblImageResolution
            // 
            this.lblImageResolution.AutoSize = true;
            this.lblImageResolution.Location = new System.Drawing.Point(12, 61);
            this.lblImageResolution.Name = "lblImageResolution";
            this.lblImageResolution.Size = new System.Drawing.Size(118, 12);
            this.lblImageResolution.TabIndex = 4;
            this.lblImageResolution.Text = "이미지 해상도(DPI) :";
            // 
            // lblImageFormat
            // 
            this.lblImageFormat.AutoSize = true;
            this.lblImageFormat.Location = new System.Drawing.Point(53, 88);
            this.lblImageFormat.Name = "lblImageFormat";
            this.lblImageFormat.Size = new System.Drawing.Size(77, 12);
            this.lblImageFormat.TabIndex = 6;
            this.lblImageFormat.Text = "이미지 포맷 :";
            // 
            // cboDevices
            // 
            this.cboDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDevices.Location = new System.Drawing.Point(136, 6);
            this.cboDevices.Name = "cboDevices";
            this.cboDevices.Size = new System.Drawing.Size(235, 20);
            this.cboDevices.TabIndex = 1;
            this.cboDevices.SelectedIndexChanged += new System.EventHandler(this.cboDevices_SelectedIndexChanged);
            // 
            // cboDisplayResolution
            // 
            this.cboDisplayResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDisplayResolution.Location = new System.Drawing.Point(136, 32);
            this.cboDisplayResolution.Name = "cboDisplayResolution";
            this.cboDisplayResolution.Size = new System.Drawing.Size(111, 20);
            this.cboDisplayResolution.TabIndex = 3;
            this.cboDisplayResolution.SelectedIndexChanged += new System.EventHandler(this.cboDisplayResolution_SelectedIndexChanged);
            // 
            // cboImageResolution
            // 
            this.cboImageResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboImageResolution.Location = new System.Drawing.Point(136, 58);
            this.cboImageResolution.Name = "cboImageResolution";
            this.cboImageResolution.Size = new System.Drawing.Size(111, 20);
            this.cboImageResolution.TabIndex = 5;
            this.cboImageResolution.SelectedIndexChanged += new System.EventHandler(this.cboImageResolution_SelectedIndexChanged);
            // 
            // cboImageFormat
            // 
            this.cboImageFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboImageFormat.Location = new System.Drawing.Point(136, 85);
            this.cboImageFormat.Name = "cboImageFormat";
            this.cboImageFormat.Size = new System.Drawing.Size(111, 20);
            this.cboImageFormat.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "십자 마크 표시 :";
            // 
            // chkShowCrosshair
            // 
            this.chkShowCrosshair.AutoSize = true;
            this.chkShowCrosshair.Location = new System.Drawing.Point(136, 113);
            this.chkShowCrosshair.Name = "chkShowCrosshair";
            this.chkShowCrosshair.Size = new System.Drawing.Size(15, 14);
            this.chkShowCrosshair.TabIndex = 9;
            this.chkShowCrosshair.UseVisualStyleBackColor = true;
            // 
            // FrmVideoCaptureOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(383, 183);
            this.Controls.Add(this.chkShowCrosshair);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboImageFormat);
            this.Controls.Add(this.cboImageResolution);
            this.Controls.Add(this.cboDisplayResolution);
            this.Controls.Add(this.cboDevices);
            this.Controls.Add(this.lblImageFormat);
            this.Controls.Add(this.lblImageResolution);
            this.Controls.Add(this.lblDisplayResolution);
            this.Controls.Add(this.lblDeviceName);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmVideoCaptureOption";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Option";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblDeviceName;
        private System.Windows.Forms.Label lblDisplayResolution;
        private System.Windows.Forms.Label lblImageResolution;
        private System.Windows.Forms.Label lblImageFormat;
        private System.Windows.Forms.ComboBox cboDevices;
        private System.Windows.Forms.ComboBox cboDisplayResolution;
        private System.Windows.Forms.ComboBox cboImageResolution;
        private System.Windows.Forms.ComboBox cboImageFormat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkShowCrosshair;
    }
}