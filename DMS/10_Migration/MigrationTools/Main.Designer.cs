namespace MigrationTools
{
    partial class TxtWaferNo
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.fpSpread1 = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkFail = new System.Windows.Forms.CheckBox();
            this.lbFailCount = new System.Windows.Forms.Label();
            this.lbProcess = new System.Windows.Forms.Label();
            this.lbCount = new System.Windows.Forms.Label();
            this.TxtLog = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtWafer = new System.Windows.Forms.TextBox();
            this.TxtLotID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.RecipeParse = new System.Windows.Forms.Button();
            this.BtnGetData = new System.Windows.Forms.Button();
            this.BtnStop = new System.Windows.Forms.Button();
            this.btnExecute = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.TxtErrorLog = new System.Windows.Forms.RichTextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1431, 551);
            this.panel1.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 46);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.fpSpread1);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(1431, 505);
            this.splitContainer1.SplitterDistance = 991;
            this.splitContainer1.TabIndex = 8;
            // 
            // fpSpread1
            // 
            this.fpSpread1.AccessibleDescription = "";
            this.fpSpread1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpread1.Location = new System.Drawing.Point(0, 62);
            this.fpSpread1.Name = "fpSpread1";
            this.fpSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread1_Sheet1});
            this.fpSpread1.Size = new System.Drawing.Size(991, 443);
            this.fpSpread1.TabIndex = 1;
            // 
            // fpSpread1_Sheet1
            // 
            this.fpSpread1_Sheet1.Reset();
            fpSpread1_Sheet1.SheetName = "Sheet1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkFail);
            this.groupBox1.Controls.Add(this.lbFailCount);
            this.groupBox1.Controls.Add(this.lbProcess);
            this.groupBox1.Controls.Add(this.lbCount);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(991, 62);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Migration List";
            // 
            // chkFail
            // 
            this.chkFail.AutoSize = true;
            this.chkFail.Location = new System.Drawing.Point(880, 18);
            this.chkFail.Name = "chkFail";
            this.chkFail.Size = new System.Drawing.Size(105, 16);
            this.chkFail.TabIndex = 2;
            this.chkFail.Text = "Fail Data View";
            this.chkFail.UseVisualStyleBackColor = true;
            this.chkFail.CheckedChanged += new System.EventHandler(this.chkFail_CheckedChanged);
            // 
            // lbFailCount
            // 
            this.lbFailCount.AutoSize = true;
            this.lbFailCount.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbFailCount.ForeColor = System.Drawing.Color.Red;
            this.lbFailCount.Location = new System.Drawing.Point(12, 43);
            this.lbFailCount.Name = "lbFailCount";
            this.lbFailCount.Size = new System.Drawing.Size(94, 12);
            this.lbFailCount.TabIndex = 1;
            this.lbFailCount.Text = "Fail Count : 0";
            // 
            // lbProcess
            // 
            this.lbProcess.AutoSize = true;
            this.lbProcess.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbProcess.Location = new System.Drawing.Point(12, 22);
            this.lbProcess.Name = "lbProcess";
            this.lbProcess.Size = new System.Drawing.Size(133, 12);
            this.lbProcess.TabIndex = 1;
            this.lbProcess.Text = "Processsing : 0 / 0";
            // 
            // lbCount
            // 
            this.lbCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbCount.AutoSize = true;
            this.lbCount.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbCount.Location = new System.Drawing.Point(797, 43);
            this.lbCount.Name = "lbCount";
            this.lbCount.Size = new System.Drawing.Size(130, 12);
            this.lbCount.TabIndex = 0;
            this.lbCount.Text = "Get Row Count :  0";
            // 
            // TxtLog
            // 
            this.TxtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TxtLog.Location = new System.Drawing.Point(3, 3);
            this.TxtLog.Name = "TxtLog";
            this.TxtLog.Size = new System.Drawing.Size(422, 473);
            this.TxtLog.TabIndex = 0;
            this.TxtLog.Text = "";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.TxtWafer);
            this.panel2.Controls.Add(this.TxtLotID);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.dtTo);
            this.panel2.Controls.Add(this.dtFrom);
            this.panel2.Controls.Add(this.RecipeParse);
            this.panel2.Controls.Add(this.BtnGetData);
            this.panel2.Controls.Add(this.BtnStop);
            this.panel2.Controls.Add(this.btnExecute);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1431, 46);
            this.panel2.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(423, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "Wafer No";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(268, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "Lot ID";
            // 
            // TxtWafer
            // 
            this.TxtWafer.Location = new System.Drawing.Point(485, 12);
            this.TxtWafer.Name = "TxtWafer";
            this.TxtWafer.Size = new System.Drawing.Size(100, 21);
            this.TxtWafer.TabIndex = 12;
            // 
            // TxtLotID
            // 
            this.TxtLotID.Location = new System.Drawing.Point(312, 12);
            this.TxtLotID.Name = "TxtLotID";
            this.TxtLotID.Size = new System.Drawing.Size(100, 21);
            this.TxtLotID.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(126, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "~";
            // 
            // dtTo
            // 
            this.dtTo.CustomFormat = "yyyyMMdd";
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtTo.Location = new System.Drawing.Point(146, 12);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(111, 21);
            this.dtTo.TabIndex = 10;
            // 
            // dtFrom
            // 
            this.dtFrom.CustomFormat = "yyyyMMdd";
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtFrom.Location = new System.Drawing.Point(9, 12);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(111, 21);
            this.dtFrom.TabIndex = 10;
            // 
            // RecipeParse
            // 
            this.RecipeParse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RecipeParse.Location = new System.Drawing.Point(614, 8);
            this.RecipeParse.Name = "RecipeParse";
            this.RecipeParse.Size = new System.Drawing.Size(144, 32);
            this.RecipeParse.TabIndex = 9;
            this.RecipeParse.Text = "RecipeParse";
            this.RecipeParse.UseVisualStyleBackColor = true;
            this.RecipeParse.Visible = false;
            this.RecipeParse.Click += new System.EventHandler(this.BtnRecipe_Click);
            // 
            // BtnGetData
            // 
            this.BtnGetData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnGetData.Location = new System.Drawing.Point(968, 8);
            this.BtnGetData.Name = "BtnGetData";
            this.BtnGetData.Size = new System.Drawing.Size(144, 32);
            this.BtnGetData.TabIndex = 9;
            this.BtnGetData.Text = "Get Data";
            this.BtnGetData.UseVisualStyleBackColor = true;
            this.BtnGetData.Click += new System.EventHandler(this.BtnGetData_Click);
            // 
            // BtnStop
            // 
            this.BtnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnStop.Enabled = false;
            this.BtnStop.Location = new System.Drawing.Point(1268, 8);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(144, 32);
            this.BtnStop.TabIndex = 9;
            this.BtnStop.Text = "Stop";
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // btnExecute
            // 
            this.btnExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExecute.Location = new System.Drawing.Point(1118, 8);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(144, 32);
            this.btnExecute.TabIndex = 9;
            this.btnExecute.Text = "Exectue";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(436, 505);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.TxtLog);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(428, 479);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Log List";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.TxtErrorLog);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(428, 479);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Fail Log";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // TxtErrorLog
            // 
            this.TxtErrorLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TxtErrorLog.Location = new System.Drawing.Point(3, 3);
            this.TxtErrorLog.Name = "TxtErrorLog";
            this.TxtErrorLog.Size = new System.Drawing.Size(422, 473);
            this.TxtErrorLog.TabIndex = 1;
            this.TxtErrorLog.Text = "";
            // 
            // TxtWaferNo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1431, 551);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "TxtWaferNo";
            this.Text = "Migration Tool";
            this.Load += new System.EventHandler(this.Main_Load);
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private FarPoint.Win.Spread.FpSpread fpSpread1;
        private FarPoint.Win.Spread.SheetView fpSpread1_Sheet1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox TxtLog;
        private System.Windows.Forms.Button BtnGetData;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbCount;
        private System.Windows.Forms.Label lbProcess;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtLotID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TxtWafer;
        private System.Windows.Forms.Button RecipeParse;
        private System.Windows.Forms.Label lbFailCount;
        private System.Windows.Forms.CheckBox chkFail;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox TxtErrorLog;

    }
}

