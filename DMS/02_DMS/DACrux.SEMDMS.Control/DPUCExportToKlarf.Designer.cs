namespace DACrux.SEMDMS.Control
{
    partial class DPUCExportToKlarf
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
            this.grpSaveLocation = new System.Windows.Forms.GroupBox();
            this.txtLocalPath = new System.Windows.Forms.TextBox();
            this.btnPath = new DACrux.SEMDMS.Control.ButtonEx();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.txtServerPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.rdoServer = new System.Windows.Forms.RadioButton();
            this.rdoLocal = new System.Windows.Forms.RadioButton();
            this.grpShortcut = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnShortcutLotID = new DACrux.SEMDMS.Control.ButtonEx();
            this.btnShortcutDeviceID = new DACrux.SEMDMS.Control.ButtonEx();
            this.btnShortcutSlotID = new DACrux.SEMDMS.Control.ButtonEx();
            this.btnShortcutStepID = new DACrux.SEMDMS.Control.ButtonEx();
            this.btnShortcutWaferID = new DACrux.SEMDMS.Control.ButtonEx();
            this.btnShortcutResultTime = new DACrux.SEMDMS.Control.ButtonEx();
            this.btnShortcutUnderbar = new DACrux.SEMDMS.Control.ButtonEx();
            this.grpSampling = new System.Windows.Forms.GroupBox();
            this.rdoNewDefect = new System.Windows.Forms.RadioButton();
            this.rdoAllDefect = new System.Windows.Forms.RadioButton();
            this.chkCluster = new System.Windows.Forms.CheckBox();
            this.fpSpread1 = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.label3 = new System.Windows.Forms.Label();
            this.numSampleCount = new System.Windows.Forms.NumericUpDown();
            this.chkDefectSampling = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkRandom = new System.Windows.Forms.CheckBox();
            this.chkMergeLot = new System.Windows.Forms.CheckBox();
            this.grpSaveLocation.SuspendLayout();
            this.grpShortcut.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.grpSampling.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSampleCount)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSaveLocation
            // 
            this.grpSaveLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSaveLocation.Controls.Add(this.txtLocalPath);
            this.grpSaveLocation.Controls.Add(this.btnPath);
            this.grpSaveLocation.Controls.Add(this.txtFileName);
            this.grpSaveLocation.Controls.Add(this.txtServerPath);
            this.grpSaveLocation.Controls.Add(this.label2);
            this.grpSaveLocation.Controls.Add(this.label1);
            this.grpSaveLocation.Controls.Add(this.rdoServer);
            this.grpSaveLocation.Controls.Add(this.rdoLocal);
            this.grpSaveLocation.Location = new System.Drawing.Point(12, 204);
            this.grpSaveLocation.Name = "grpSaveLocation";
            this.grpSaveLocation.Size = new System.Drawing.Size(562, 84);
            this.grpSaveLocation.TabIndex = 0;
            this.grpSaveLocation.TabStop = false;
            this.grpSaveLocation.Text = "Save Location";
            // 
            // txtLocalPath
            // 
            this.txtLocalPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLocalPath.Location = new System.Drawing.Point(200, 26);
            this.txtLocalPath.Name = "txtLocalPath";
            this.txtLocalPath.ReadOnly = true;
            this.txtLocalPath.Size = new System.Drawing.Size(318, 21);
            this.txtLocalPath.TabIndex = 5;
            // 
            // btnPath
            // 
            this.btnPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPath.Location = new System.Drawing.Point(520, 26);
            this.btnPath.Name = "btnPath";
            this.btnPath.Size = new System.Drawing.Size(27, 23);
            this.btnPath.TabIndex = 4;
            this.btnPath.Text = "...";
            this.btnPath.UseVisualStyleBackColor = true;
            this.btnPath.Click += new System.EventHandler(this.btnPath_Click);
            // 
            // txtFileName
            // 
            this.txtFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFileName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtFileName.Location = new System.Drawing.Point(200, 49);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(347, 21);
            this.txtFileName.TabIndex = 3;
            this.txtFileName.TextChanged += new System.EventHandler(this.txtFileName_TextChanged);
            // 
            // txtServerPath
            // 
            this.txtServerPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServerPath.Location = new System.Drawing.Point(200, 26);
            this.txtServerPath.Name = "txtServerPath";
            this.txtServerPath.ReadOnly = true;
            this.txtServerPath.Size = new System.Drawing.Size(317, 21);
            this.txtServerPath.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(122, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "File Name :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(155, 29);
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
            // grpShortcut
            // 
            this.grpShortcut.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpShortcut.Controls.Add(this.flowLayoutPanel2);
            this.grpShortcut.Location = new System.Drawing.Point(12, 295);
            this.grpShortcut.Name = "grpShortcut";
            this.grpShortcut.Size = new System.Drawing.Size(562, 81);
            this.grpShortcut.TabIndex = 1;
            this.grpShortcut.TabStop = false;
            this.grpShortcut.Text = "File Name Shortcuts";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.btnShortcutLotID);
            this.flowLayoutPanel2.Controls.Add(this.btnShortcutDeviceID);
            this.flowLayoutPanel2.Controls.Add(this.btnShortcutSlotID);
            this.flowLayoutPanel2.Controls.Add(this.btnShortcutStepID);
            this.flowLayoutPanel2.Controls.Add(this.btnShortcutWaferID);
            this.flowLayoutPanel2.Controls.Add(this.btnShortcutResultTime);
            this.flowLayoutPanel2.Controls.Add(this.btnShortcutUnderbar);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 17);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(556, 61);
            this.flowLayoutPanel2.TabIndex = 6;
            // 
            // btnShortcutLotID
            // 
            this.btnShortcutLotID.Location = new System.Drawing.Point(3, 3);
            this.btnShortcutLotID.Name = "btnShortcutLotID";
            this.btnShortcutLotID.Size = new System.Drawing.Size(80, 23);
            this.btnShortcutLotID.TabIndex = 0;
            this.btnShortcutLotID.Text = "%L=Lot ID";
            this.btnShortcutLotID.UseVisualStyleBackColor = true;
            this.btnShortcutLotID.Click += new System.EventHandler(this.ButtonShortcut_Click);
            // 
            // btnShortcutDeviceID
            // 
            this.btnShortcutDeviceID.Location = new System.Drawing.Point(89, 3);
            this.btnShortcutDeviceID.Name = "btnShortcutDeviceID";
            this.btnShortcutDeviceID.Size = new System.Drawing.Size(92, 23);
            this.btnShortcutDeviceID.TabIndex = 0;
            this.btnShortcutDeviceID.Text = "%D=Device ID";
            this.btnShortcutDeviceID.UseVisualStyleBackColor = true;
            this.btnShortcutDeviceID.Click += new System.EventHandler(this.ButtonShortcut_Click);
            // 
            // btnShortcutSlotID
            // 
            this.btnShortcutSlotID.Location = new System.Drawing.Point(187, 3);
            this.btnShortcutSlotID.Name = "btnShortcutSlotID";
            this.btnShortcutSlotID.Size = new System.Drawing.Size(80, 23);
            this.btnShortcutSlotID.TabIndex = 0;
            this.btnShortcutSlotID.Text = "%O=Slot ID";
            this.btnShortcutSlotID.UseVisualStyleBackColor = true;
            this.btnShortcutSlotID.Click += new System.EventHandler(this.ButtonShortcut_Click);
            // 
            // btnShortcutStepID
            // 
            this.btnShortcutStepID.Location = new System.Drawing.Point(273, 3);
            this.btnShortcutStepID.Name = "btnShortcutStepID";
            this.btnShortcutStepID.Size = new System.Drawing.Size(101, 23);
            this.btnShortcutStepID.TabIndex = 0;
            this.btnShortcutStepID.Text = "%S=Step ID";
            this.btnShortcutStepID.UseVisualStyleBackColor = true;
            this.btnShortcutStepID.Click += new System.EventHandler(this.ButtonShortcut_Click);
            // 
            // btnShortcutWaferID
            // 
            this.btnShortcutWaferID.Location = new System.Drawing.Point(380, 3);
            this.btnShortcutWaferID.Name = "btnShortcutWaferID";
            this.btnShortcutWaferID.Size = new System.Drawing.Size(91, 23);
            this.btnShortcutWaferID.TabIndex = 0;
            this.btnShortcutWaferID.Text = "%W=Wafer ID";
            this.btnShortcutWaferID.UseVisualStyleBackColor = true;
            this.btnShortcutWaferID.Click += new System.EventHandler(this.ButtonShortcut_Click);
            // 
            // btnShortcutResultTime
            // 
            this.btnShortcutResultTime.Location = new System.Drawing.Point(3, 32);
            this.btnShortcutResultTime.Name = "btnShortcutResultTime";
            this.btnShortcutResultTime.Size = new System.Drawing.Size(121, 23);
            this.btnShortcutResultTime.TabIndex = 2;
            this.btnShortcutResultTime.Text = "%R=Result Time";
            this.btnShortcutResultTime.UseVisualStyleBackColor = true;
            this.btnShortcutResultTime.Click += new System.EventHandler(this.ButtonShortcut_Click);
            // 
            // btnShortcutUnderbar
            // 
            this.btnShortcutUnderbar.Location = new System.Drawing.Point(130, 32);
            this.btnShortcutUnderbar.Name = "btnShortcutUnderbar";
            this.btnShortcutUnderbar.Size = new System.Drawing.Size(38, 23);
            this.btnShortcutUnderbar.TabIndex = 1;
            this.btnShortcutUnderbar.Text = "_";
            this.btnShortcutUnderbar.UseVisualStyleBackColor = true;
            this.btnShortcutUnderbar.Click += new System.EventHandler(this.ButtonShortcut_Click);
            // 
            // grpSampling
            // 
            this.grpSampling.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSampling.Controls.Add(this.rdoNewDefect);
            this.grpSampling.Controls.Add(this.rdoAllDefect);
            this.grpSampling.Location = new System.Drawing.Point(12, 387);
            this.grpSampling.Name = "grpSampling";
            this.grpSampling.Size = new System.Drawing.Size(127, 67);
            this.grpSampling.TabIndex = 4;
            this.grpSampling.TabStop = false;
            this.grpSampling.Text = "All / New";
            // 
            // rdoNewDefect
            // 
            this.rdoNewDefect.AutoSize = true;
            this.rdoNewDefect.Location = new System.Drawing.Point(20, 42);
            this.rdoNewDefect.Name = "rdoNewDefect";
            this.rdoNewDefect.Size = new System.Drawing.Size(88, 16);
            this.rdoNewDefect.TabIndex = 1;
            this.rdoNewDefect.Text = "New Defect";
            this.rdoNewDefect.UseVisualStyleBackColor = true;
            this.rdoNewDefect.CheckedChanged += new System.EventHandler(this.DefectSample_CheckedChanged);
            // 
            // rdoAllDefect
            // 
            this.rdoAllDefect.AutoSize = true;
            this.rdoAllDefect.Checked = true;
            this.rdoAllDefect.Location = new System.Drawing.Point(20, 20);
            this.rdoAllDefect.Name = "rdoAllDefect";
            this.rdoAllDefect.Size = new System.Drawing.Size(76, 16);
            this.rdoAllDefect.TabIndex = 2;
            this.rdoAllDefect.TabStop = true;
            this.rdoAllDefect.Text = "All Defect";
            this.rdoAllDefect.UseVisualStyleBackColor = true;
            this.rdoAllDefect.CheckedChanged += new System.EventHandler(this.DefectSample_CheckedChanged);
            // 
            // chkCluster
            // 
            this.chkCluster.AutoSize = true;
            this.chkCluster.Checked = true;
            this.chkCluster.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCluster.Location = new System.Drawing.Point(18, 42);
            this.chkCluster.Name = "chkCluster";
            this.chkCluster.Size = new System.Drawing.Size(131, 16);
            this.chkCluster.TabIndex = 8;
            this.chkCluster.Text = "Cluster Defect 포함";
            this.chkCluster.UseVisualStyleBackColor = true;
            this.chkCluster.CheckedChanged += new System.EventHandler(this.DefectSample_CheckedChanged);
            // 
            // fpSpread1
            // 
            this.fpSpread1.AccessibleDescription = "";
            this.fpSpread1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fpSpread1.Location = new System.Drawing.Point(12, 23);
            this.fpSpread1.Name = "fpSpread1";
            this.fpSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread1_Sheet1});
            this.fpSpread1.Size = new System.Drawing.Size(562, 173);
            this.fpSpread1.TabIndex = 5;
            this.fpSpread1.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpSpread1_CellClick);
            this.fpSpread1.ButtonClicked += new FarPoint.Win.Spread.EditorNotifyEventHandler(this.fpSpread1_ButtonClicked);
            this.fpSpread1.EditChange += new FarPoint.Win.Spread.EditorNotifyEventHandler(this.fpSpread1_EditChange);
            // 
            // fpSpread1_Sheet1
            // 
            this.fpSpread1_Sheet1.Reset();
            fpSpread1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpSpread1_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.MultiRange;
            this.fpSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "Wafer List";
            // 
            // numSampleCount
            // 
            this.numSampleCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numSampleCount.Location = new System.Drawing.Point(152, 460);
            this.numSampleCount.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numSampleCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSampleCount.Name = "numSampleCount";
            this.numSampleCount.Size = new System.Drawing.Size(97, 21);
            this.numSampleCount.TabIndex = 6;
            this.numSampleCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numSampleCount.ThousandsSeparator = true;
            this.numSampleCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSampleCount.ValueChanged += new System.EventHandler(this.numSampleCount_ValueChanged);
            // 
            // chkDefectSampling
            // 
            this.chkDefectSampling.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkDefectSampling.AutoSize = true;
            this.chkDefectSampling.Checked = true;
            this.chkDefectSampling.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDefectSampling.Location = new System.Drawing.Point(31, 464);
            this.chkDefectSampling.Name = "chkDefectSampling";
            this.chkDefectSampling.Size = new System.Drawing.Size(115, 16);
            this.chkDefectSampling.TabIndex = 5;
            this.chkDefectSampling.Text = "샘플 Defect 개수";
            this.chkDefectSampling.UseVisualStyleBackColor = true;
            this.chkDefectSampling.CheckedChanged += new System.EventHandler(this.chkDefectCount_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.chkRandom);
            this.groupBox2.Controls.Add(this.chkCluster);
            this.groupBox2.Location = new System.Drawing.Point(145, 387);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(390, 67);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Defect Include";
            // 
            // chkRandom
            // 
            this.chkRandom.AutoSize = true;
            this.chkRandom.Checked = true;
            this.chkRandom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRandom.Location = new System.Drawing.Point(18, 21);
            this.chkRandom.Name = "chkRandom";
            this.chkRandom.Size = new System.Drawing.Size(138, 16);
            this.chkRandom.TabIndex = 9;
            this.chkRandom.Text = "Random Defect 포함";
            this.chkRandom.UseVisualStyleBackColor = true;
            this.chkRandom.CheckedChanged += new System.EventHandler(this.DefectSample_CheckedChanged);
            // 
            // chkMergeLot
            // 
            this.chkMergeLot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkMergeLot.AutoSize = true;
            this.chkMergeLot.Location = new System.Drawing.Point(31, 485);
            this.chkMergeLot.Name = "chkMergeLot";
            this.chkMergeLot.Size = new System.Drawing.Size(349, 16);
            this.chkMergeLot.TabIndex = 8;
            this.chkMergeLot.Text = "같은 LOT, 같은 STEP의 여러 WAFER는 하나의 파일로 저장";
            this.chkMergeLot.UseVisualStyleBackColor = true;
            this.chkMergeLot.CheckedChanged += new System.EventHandler(this.chkMergeLot_CheckedChanged);
            // 
            // DPUCExportToKlarf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkMergeLot);
            this.Controls.Add(this.numSampleCount);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkDefectSampling);
            this.Controls.Add(this.fpSpread1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grpSampling);
            this.Controls.Add(this.grpShortcut);
            this.Controls.Add(this.grpSaveLocation);
            this.Name = "DPUCExportToKlarf";
            this.Size = new System.Drawing.Size(586, 501);
            this.Load += new System.EventHandler(this.PopupExportToKlarf_Load);
            this.grpSaveLocation.ResumeLayout(false);
            this.grpSaveLocation.PerformLayout();
            this.grpShortcut.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.grpSampling.ResumeLayout(false);
            this.grpSampling.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSampleCount)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSaveLocation;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.TextBox txtServerPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdoServer;
        private System.Windows.Forms.RadioButton rdoLocal;
        private System.Windows.Forms.GroupBox grpShortcut;
        private DACrux.SEMDMS.Control.ButtonEx btnShortcutWaferID;
        private DACrux.SEMDMS.Control.ButtonEx btnShortcutStepID;
        private DACrux.SEMDMS.Control.ButtonEx btnShortcutSlotID;
        private DACrux.SEMDMS.Control.ButtonEx btnShortcutDeviceID;
        private DACrux.SEMDMS.Control.ButtonEx btnShortcutLotID;
        private DACrux.SEMDMS.Control.ButtonEx btnShortcutUnderbar;
        private DACrux.SEMDMS.Control.ButtonEx btnShortcutResultTime;
        private DACrux.SEMDMS.Control.ButtonEx btnPath;
        private System.Windows.Forms.TextBox txtLocalPath;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.GroupBox grpSampling;
        private FarPoint.Win.Spread.FpSpread fpSpread1;
        private FarPoint.Win.Spread.SheetView fpSpread1_Sheet1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rdoNewDefect;
        private System.Windows.Forms.RadioButton rdoAllDefect;
        private System.Windows.Forms.NumericUpDown numSampleCount;
        private System.Windows.Forms.CheckBox chkDefectSampling;
        private System.Windows.Forms.CheckBox chkCluster;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkRandom;
        private System.Windows.Forms.CheckBox chkMergeLot;
    }
}