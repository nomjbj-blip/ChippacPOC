namespace DACrux.Data.Parser
{
    partial class PopupExportToKlarf
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnPath = new System.Windows.Forms.Button();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.txtServerPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.rdoServer = new System.Windows.Forms.RadioButton();
            this.rdoLocal = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnShortcutUnderbar = new DACrux.Data.Parser.ButtonEx();
            this.btnShortcutInspectionTime = new DACrux.Data.Parser.ButtonEx();
            this.btnShortcutWaferID = new DACrux.Data.Parser.ButtonEx();
            this.btnShortcutStepID = new DACrux.Data.Parser.ButtonEx();
            this.btnShortcutSlotID = new DACrux.Data.Parser.ButtonEx();
            this.btnShortcutDeviceID = new DACrux.Data.Parser.ButtonEx();
            this.btnShortcutLotID = new DACrux.Data.Parser.ButtonEx();
            this.grpDataSampling = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.chkDataSampling = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtLocalPath = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.grpDataSampling.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtLocalPath);
            this.groupBox1.Controls.Add(this.btnPath);
            this.groupBox1.Controls.Add(this.txtFileName);
            this.groupBox1.Controls.Add(this.txtServerPath);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.rdoServer);
            this.groupBox1.Controls.Add(this.rdoLocal);
            this.groupBox1.Location = new System.Drawing.Point(12, 142);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(561, 84);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Save Location";
            // 
            // btnPath
            // 
            this.btnPath.Location = new System.Drawing.Point(519, 26);
            this.btnPath.Name = "btnPath";
            this.btnPath.Size = new System.Drawing.Size(27, 23);
            this.btnPath.TabIndex = 4;
            this.btnPath.Text = "...";
            this.btnPath.UseVisualStyleBackColor = true;
            this.btnPath.Click += new System.EventHandler(this.btnPath_Click);
            // 
            // txtFileName
            // 
            this.txtFileName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtFileName.Location = new System.Drawing.Point(190, 49);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(356, 21);
            this.txtFileName.TabIndex = 3;
            // 
            // txtServerPath
            // 
            this.txtServerPath.Location = new System.Drawing.Point(190, 26);
            this.txtServerPath.Name = "txtServerPath";
            this.txtServerPath.ReadOnly = true;
            this.txtServerPath.Size = new System.Drawing.Size(327, 21);
            this.txtServerPath.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(113, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "File Name :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(146, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Path :";
            // 
            // rdoServer
            // 
            this.rdoServer.AutoSize = true;
            this.rdoServer.Location = new System.Drawing.Point(20, 51);
            this.rdoServer.Name = "rdoServer";
            this.rdoServer.Size = new System.Drawing.Size(59, 16);
            this.rdoServer.TabIndex = 1;
            this.rdoServer.TabStop = true;
            this.rdoServer.Text = "Server";
            this.rdoServer.UseVisualStyleBackColor = true;
            this.rdoServer.CheckedChanged += new System.EventHandler(this.rdoLocation_CheckedChanged);
            // 
            // rdoLocal
            // 
            this.rdoLocal.AutoSize = true;
            this.rdoLocal.Location = new System.Drawing.Point(20, 27);
            this.rdoLocal.Name = "rdoLocal";
            this.rdoLocal.Size = new System.Drawing.Size(54, 16);
            this.rdoLocal.TabIndex = 0;
            this.rdoLocal.TabStop = true;
            this.rdoLocal.Text = "Local";
            this.rdoLocal.UseVisualStyleBackColor = true;
            this.rdoLocal.CheckedChanged += new System.EventHandler(this.rdoLocation_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnShortcutUnderbar);
            this.groupBox2.Controls.Add(this.btnShortcutInspectionTime);
            this.groupBox2.Controls.Add(this.btnShortcutWaferID);
            this.groupBox2.Controls.Add(this.btnShortcutStepID);
            this.groupBox2.Controls.Add(this.btnShortcutSlotID);
            this.groupBox2.Controls.Add(this.btnShortcutDeviceID);
            this.groupBox2.Controls.Add(this.btnShortcutLotID);
            this.groupBox2.Location = new System.Drawing.Point(12, 232);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(561, 81);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "File Name Shortcuts:";
            // 
            // btnShortcutUnderbar
            // 
            this.btnShortcutUnderbar.Location = new System.Drawing.Point(92, 49);
            this.btnShortcutUnderbar.Name = "btnShortcutUnderbar";
            this.btnShortcutUnderbar.Size = new System.Drawing.Size(38, 23);
            this.btnShortcutUnderbar.TabIndex = 1;
            this.btnShortcutUnderbar.Text = "_";
            this.btnShortcutUnderbar.UseVisualStyleBackColor = true;
            this.btnShortcutUnderbar.Click += new System.EventHandler(this.ButtonShortcut_Click);
            // 
            // btnShortcutInspectionTime
            // 
            this.btnShortcutInspectionTime.Location = new System.Drawing.Point(394, 20);
            this.btnShortcutInspectionTime.Name = "btnShortcutInspectionTime";
            this.btnShortcutInspectionTime.Size = new System.Drawing.Size(137, 23);
            this.btnShortcutInspectionTime.TabIndex = 0;
            this.btnShortcutInspectionTime.Text = "%I=Inspection Time";
            this.btnShortcutInspectionTime.UseVisualStyleBackColor = true;
            this.btnShortcutInspectionTime.Click += new System.EventHandler(this.ButtonShortcut_Click);
            // 
            // btnShortcutWaferID
            // 
            this.btnShortcutWaferID.Location = new System.Drawing.Point(93, 20);
            this.btnShortcutWaferID.Name = "btnShortcutWaferID";
            this.btnShortcutWaferID.Size = new System.Drawing.Size(91, 23);
            this.btnShortcutWaferID.TabIndex = 0;
            this.btnShortcutWaferID.Text = "%W=Wafer ID";
            this.btnShortcutWaferID.UseVisualStyleBackColor = true;
            this.btnShortcutWaferID.Click += new System.EventHandler(this.ButtonShortcut_Click);
            // 
            // btnShortcutStepID
            // 
            this.btnShortcutStepID.Location = new System.Drawing.Point(190, 20);
            this.btnShortcutStepID.Name = "btnShortcutStepID";
            this.btnShortcutStepID.Size = new System.Drawing.Size(101, 23);
            this.btnShortcutStepID.TabIndex = 0;
            this.btnShortcutStepID.Text = "%S=Step ID";
            this.btnShortcutStepID.UseVisualStyleBackColor = true;
            this.btnShortcutStepID.Click += new System.EventHandler(this.ButtonShortcut_Click);
            // 
            // btnShortcutSlotID
            // 
            this.btnShortcutSlotID.Location = new System.Drawing.Point(6, 49);
            this.btnShortcutSlotID.Name = "btnShortcutSlotID";
            this.btnShortcutSlotID.Size = new System.Drawing.Size(80, 23);
            this.btnShortcutSlotID.TabIndex = 0;
            this.btnShortcutSlotID.Text = "%O=Slot ID";
            this.btnShortcutSlotID.UseVisualStyleBackColor = true;
            this.btnShortcutSlotID.Click += new System.EventHandler(this.ButtonShortcut_Click);
            // 
            // btnShortcutDeviceID
            // 
            this.btnShortcutDeviceID.Location = new System.Drawing.Point(296, 20);
            this.btnShortcutDeviceID.Name = "btnShortcutDeviceID";
            this.btnShortcutDeviceID.Size = new System.Drawing.Size(92, 23);
            this.btnShortcutDeviceID.TabIndex = 0;
            this.btnShortcutDeviceID.Text = "%D=Device ID";
            this.btnShortcutDeviceID.UseVisualStyleBackColor = true;
            this.btnShortcutDeviceID.Click += new System.EventHandler(this.ButtonShortcut_Click);
            // 
            // btnShortcutLotID
            // 
            this.btnShortcutLotID.Location = new System.Drawing.Point(6, 20);
            this.btnShortcutLotID.Name = "btnShortcutLotID";
            this.btnShortcutLotID.Size = new System.Drawing.Size(80, 23);
            this.btnShortcutLotID.TabIndex = 0;
            this.btnShortcutLotID.Text = "%L=Lot ID";
            this.btnShortcutLotID.UseVisualStyleBackColor = true;
            this.btnShortcutLotID.Click += new System.EventHandler(this.ButtonShortcut_Click);
            // 
            // grpDataSampling
            // 
            this.grpDataSampling.Controls.Add(this.flowLayoutPanel1);
            this.grpDataSampling.Controls.Add(this.chkDataSampling);
            this.grpDataSampling.Location = new System.Drawing.Point(12, 12);
            this.grpDataSampling.Name = "grpDataSampling";
            this.grpDataSampling.Size = new System.Drawing.Size(561, 124);
            this.grpDataSampling.TabIndex = 2;
            this.grpDataSampling.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 17);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(555, 104);
            this.flowLayoutPanel1.TabIndex = 5;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // chkDataSampling
            // 
            this.chkDataSampling.AutoSize = true;
            this.chkDataSampling.Location = new System.Drawing.Point(13, 2);
            this.chkDataSampling.Name = "chkDataSampling";
            this.chkDataSampling.Size = new System.Drawing.Size(106, 16);
            this.chkDataSampling.TabIndex = 4;
            this.chkDataSampling.Text = "Data Sampling";
            this.chkDataSampling.UseVisualStyleBackColor = true;
            this.chkDataSampling.CheckedChanged += new System.EventHandler(this.chkDataSampling_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 321);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(585, 35);
            this.panel1.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(498, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(417, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtLocalPath
            // 
            this.txtLocalPath.Location = new System.Drawing.Point(190, 26);
            this.txtLocalPath.Name = "txtLocalPath";
            this.txtLocalPath.ReadOnly = true;
            this.txtLocalPath.Size = new System.Drawing.Size(327, 21);
            this.txtLocalPath.TabIndex = 5;
            // 
            // PopupExportToKlarf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(585, 356);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.grpDataSampling);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PopupExportToKlarf";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Export to Klarf...";
            this.Load += new System.EventHandler(this.PopupExportToKlarf_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.grpDataSampling.ResumeLayout(false);
            this.grpDataSampling.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.TextBox txtServerPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdoServer;
        private System.Windows.Forms.RadioButton rdoLocal;
        private System.Windows.Forms.GroupBox groupBox2;
        private ButtonEx btnShortcutInspectionTime;
        private ButtonEx btnShortcutWaferID;
        private ButtonEx btnShortcutStepID;
        private ButtonEx btnShortcutSlotID;
        private ButtonEx btnShortcutDeviceID;
        private ButtonEx btnShortcutLotID;
        private System.Windows.Forms.GroupBox grpDataSampling;
        private System.Windows.Forms.CheckBox chkDataSampling;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnPath;
        private ButtonEx btnShortcutUnderbar;
        private System.Windows.Forms.TextBox txtLocalPath;
    }
}