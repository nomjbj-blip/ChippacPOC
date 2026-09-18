namespace TestDataMigration
{
    partial class FrmMain
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
            this.dtEnd = new System.Windows.Forms.DateTimePicker();
            this.dtStart = new System.Windows.Forms.DateTimePicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnPROGRAM_PARM_DEF = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoFAB2 = new System.Windows.Forms.RadioButton();
            this.rdoFab1 = new System.Windows.Forms.RadioButton();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtEnd
            // 
            this.dtEnd.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtEnd.Location = new System.Drawing.Point(183, 21);
            this.dtEnd.Name = "dtEnd";
            this.dtEnd.Size = new System.Drawing.Size(145, 21);
            this.dtEnd.TabIndex = 13;
            this.dtEnd.Value = new System.DateTime(2019, 2, 15, 0, 0, 0, 0);
            // 
            // dtStart
            // 
            this.dtStart.Checked = false;
            this.dtStart.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtStart.Location = new System.Drawing.Point(12, 21);
            this.dtStart.Name = "dtStart";
            this.dtStart.Size = new System.Drawing.Size(145, 21);
            this.dtStart.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnPROGRAM_PARM_DEF);
            this.panel1.Controls.Add(this.btnTest);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.btnStop);
            this.panel1.Controls.Add(this.btnRun);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.dtStart);
            this.panel1.Controls.Add(this.dtEnd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(637, 103);
            this.panel1.TabIndex = 14;
            // 
            // btnPROGRAM_PARM_DEF
            // 
            this.btnPROGRAM_PARM_DEF.Location = new System.Drawing.Point(12, 74);
            this.btnPROGRAM_PARM_DEF.Name = "btnPROGRAM_PARM_DEF";
            this.btnPROGRAM_PARM_DEF.Size = new System.Drawing.Size(152, 23);
            this.btnPROGRAM_PARM_DEF.TabIndex = 19;
            this.btnPROGRAM_PARM_DEF.Text = "PROGRAM_PARM_DEF";
            this.btnPROGRAM_PARM_DEF.UseVisualStyleBackColor = true;
            this.btnPROGRAM_PARM_DEF.Click += new System.EventHandler(this.btnPROGRAM_PARM_DEF_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(470, 3);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(155, 23);
            this.btnTest.TabIndex = 18;
            this.btnTest.Text = "Connection Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoFAB2);
            this.groupBox1.Controls.Add(this.rdoFab1);
            this.groupBox1.Location = new System.Drawing.Point(334, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(130, 42);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Factory";
            // 
            // rdoFAB2
            // 
            this.rdoFAB2.AutoSize = true;
            this.rdoFAB2.Location = new System.Drawing.Point(64, 20);
            this.rdoFAB2.Name = "rdoFAB2";
            this.rdoFAB2.Size = new System.Drawing.Size(52, 16);
            this.rdoFAB2.TabIndex = 15;
            this.rdoFAB2.TabStop = true;
            this.rdoFAB2.Text = "FAB2";
            this.rdoFAB2.UseVisualStyleBackColor = true;
            // 
            // rdoFab1
            // 
            this.rdoFab1.AutoSize = true;
            this.rdoFab1.Location = new System.Drawing.Point(6, 20);
            this.rdoFab1.Name = "rdoFab1";
            this.rdoFab1.Size = new System.Drawing.Size(52, 16);
            this.rdoFab1.TabIndex = 15;
            this.rdoFab1.TabStop = true;
            this.rdoFab1.Text = "FAB1";
            this.rdoFab1.UseVisualStyleBackColor = true;
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.Location = new System.Drawing.Point(551, 29);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 16;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRun.Location = new System.Drawing.Point(470, 29);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 15;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(163, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "~";
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(0, 103);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(637, 325);
            this.listBox1.TabIndex = 15;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 428);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.panel1);
            this.Name = "FrmMain";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtEnd;
        private System.Windows.Forms.DateTimePicker dtStart;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoFAB2;
        private System.Windows.Forms.RadioButton rdoFab1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnPROGRAM_PARM_DEF;
    }
}

