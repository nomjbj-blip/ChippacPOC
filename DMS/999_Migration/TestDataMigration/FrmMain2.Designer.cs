namespace TestDataMigration
{
    partial class FrmMain2
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.dtStart = new System.Windows.Forms.DateTimePicker();
            this.pnlFunc = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.button7 = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.btnBin = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnInsertWaferData = new System.Windows.Forms.Button();
            this.txtInsertWafSeq = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSumLot = new System.Windows.Forms.TextBox();
            this.btnSum = new System.Windows.Forms.Button();
            this.txtMLotSeq = new System.Windows.Forms.TextBox();
            this.btnProcessValid = new System.Windows.Forms.Button();
            this.btnDeleteUnlinked = new System.Windows.Forms.Button();
            this.btnUpdateXY = new System.Windows.Forms.Button();
            this.btnPROGRAM_PARM_DEF = new System.Windows.Forms.Button();
            this.btnDeleteData = new System.Windows.Forms.Button();
            this.btnDATA_TABLES = new System.Windows.Forms.Button();
            this.dtpDeleteDate = new System.Windows.Forms.DateTimePicker();
            this.btnCreateTdTable = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnRunSummary = new System.Windows.Forms.Button();
            this.btnRunTDTableFieldCheckALL = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRunTDTableFieldCheck = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtWafSeq_TDTableFieldCheck = new System.Windows.Forms.TextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoFAB2 = new System.Windows.Forms.RadioButton();
            this.rdoFab1 = new System.Windows.Forms.RadioButton();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.label11 = new System.Windows.Forms.Label();
            this.button9 = new System.Windows.Forms.Button();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.pnlFunc.SuspendLayout();
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
            this.dtEnd.Value = new System.DateTime(2019, 8, 1, 0, 0, 0, 0);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dtStart);
            this.panel1.Controls.Add(this.pnlFunc);
            this.panel1.Controls.Add(this.btnTest);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.btnStop);
            this.panel1.Controls.Add(this.btnRun);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.dtEnd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(841, 457);
            this.panel1.TabIndex = 14;
            // 
            // dtStart
            // 
            this.dtStart.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtStart.Location = new System.Drawing.Point(11, 21);
            this.dtStart.Name = "dtStart";
            this.dtStart.Size = new System.Drawing.Size(145, 21);
            this.dtStart.TabIndex = 31;
            this.dtStart.Value = new System.DateTime(2019, 7, 1, 0, 0, 0, 0);
            // 
            // pnlFunc
            // 
            this.pnlFunc.Controls.Add(this.label11);
            this.pnlFunc.Controls.Add(this.button9);
            this.pnlFunc.Controls.Add(this.textBox5);
            this.pnlFunc.Controls.Add(this.label10);
            this.pnlFunc.Controls.Add(this.button8);
            this.pnlFunc.Controls.Add(this.dateTimePicker1);
            this.pnlFunc.Controls.Add(this.button7);
            this.pnlFunc.Controls.Add(this.textBox4);
            this.pnlFunc.Controls.Add(this.label9);
            this.pnlFunc.Controls.Add(this.button6);
            this.pnlFunc.Controls.Add(this.textBox3);
            this.pnlFunc.Controls.Add(this.textBox2);
            this.pnlFunc.Controls.Add(this.button5);
            this.pnlFunc.Controls.Add(this.button4);
            this.pnlFunc.Controls.Add(this.button3);
            this.pnlFunc.Controls.Add(this.textBox1);
            this.pnlFunc.Controls.Add(this.label8);
            this.pnlFunc.Controls.Add(this.button2);
            this.pnlFunc.Controls.Add(this.btnBin);
            this.pnlFunc.Controls.Add(this.button1);
            this.pnlFunc.Controls.Add(this.btnInsertWaferData);
            this.pnlFunc.Controls.Add(this.txtInsertWafSeq);
            this.pnlFunc.Controls.Add(this.label7);
            this.pnlFunc.Controls.Add(this.label6);
            this.pnlFunc.Controls.Add(this.label5);
            this.pnlFunc.Controls.Add(this.txtSumLot);
            this.pnlFunc.Controls.Add(this.btnSum);
            this.pnlFunc.Controls.Add(this.txtMLotSeq);
            this.pnlFunc.Controls.Add(this.btnProcessValid);
            this.pnlFunc.Controls.Add(this.btnDeleteUnlinked);
            this.pnlFunc.Controls.Add(this.btnUpdateXY);
            this.pnlFunc.Controls.Add(this.btnPROGRAM_PARM_DEF);
            this.pnlFunc.Controls.Add(this.btnDeleteData);
            this.pnlFunc.Controls.Add(this.btnDATA_TABLES);
            this.pnlFunc.Controls.Add(this.dtpDeleteDate);
            this.pnlFunc.Controls.Add(this.btnCreateTdTable);
            this.pnlFunc.Controls.Add(this.label4);
            this.pnlFunc.Controls.Add(this.btnRunSummary);
            this.pnlFunc.Controls.Add(this.btnRunTDTableFieldCheckALL);
            this.pnlFunc.Controls.Add(this.label2);
            this.pnlFunc.Controls.Add(this.btnRunTDTableFieldCheck);
            this.pnlFunc.Controls.Add(this.label3);
            this.pnlFunc.Controls.Add(this.txtWafSeq_TDTableFieldCheck);
            this.pnlFunc.Location = new System.Drawing.Point(0, 57);
            this.pnlFunc.Name = "pnlFunc";
            this.pnlFunc.Size = new System.Drawing.Size(841, 394);
            this.pnlFunc.TabIndex = 30;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(402, 253);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(118, 12);
            this.label10.TabIndex = 55;
            this.label10.Text = "빈 WaferSum 만들기";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(526, 248);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(69, 23);
            this.button8.TabIndex = 54;
            this.button8.Text = "SUM";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(500, 4);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(82, 21);
            this.dateTimePicker1.TabIndex = 32;
            this.dateTimePicker1.Value = new System.DateTime(2019, 7, 1, 0, 0, 0, 0);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(674, 3);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 52;
            this.button7.Text = "LOT RUN";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(588, 4);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(80, 21);
            this.textBox4.TabIndex = 53;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(211, 244);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(142, 12);
            this.label9.TabIndex = 51;
            this.label9.Text = "TTable의 lot 데이터 넣기";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(291, 256);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 50;
            this.button6.Text = "button6";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(213, 256);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(80, 21);
            this.textBox3.TabIndex = 49;
            this.textBox3.Text = "TKA80_FT2";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(213, 209);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 21);
            this.textBox2.TabIndex = 49;
            this.textBox2.Text = "c:\\P_MA65_5P.txt";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(319, 209);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(38, 23);
            this.button5.TabIndex = 48;
            this.button5.Text = "RUN";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(375, 190);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(235, 23);
            this.button4.TabIndex = 47;
            this.button4.Text = "TQP_WAFER있고 데이터 없는 WAFER";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(135, 235);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(51, 21);
            this.button3.TabIndex = 46;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(29, 235);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 45;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(27, 220);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(146, 12);
            this.label8.TabIndex = 44;
            this.label8.Text = "PROGRAM 테이블 재정렬";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(724, 29);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(102, 23);
            this.button2.TabIndex = 43;
            this.button2.Text = "Delete Data";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnBin
            // 
            this.btnBin.Location = new System.Drawing.Point(569, 140);
            this.btnBin.Name = "btnBin";
            this.btnBin.Size = new System.Drawing.Size(75, 23);
            this.btnBin.TabIndex = 42;
            this.btnBin.Text = "BIN";
            this.btnBin.UseVisualStyleBackColor = true;
            this.btnBin.Click += new System.EventHandler(this.btnBin_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(383, 147);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(135, 23);
            this.button1.TabIndex = 40;
            this.button1.Text = "중복 COL 삭제";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // btnInsertWaferData
            // 
            this.btnInsertWaferData.Location = new System.Drawing.Point(319, 140);
            this.btnInsertWaferData.Name = "btnInsertWaferData";
            this.btnInsertWaferData.Size = new System.Drawing.Size(38, 23);
            this.btnInsertWaferData.TabIndex = 39;
            this.btnInsertWaferData.Text = "RUN";
            this.btnInsertWaferData.UseVisualStyleBackColor = true;
            this.btnInsertWaferData.Click += new System.EventHandler(this.btnInsertWaferData_Click);
            // 
            // txtInsertWafSeq
            // 
            this.txtInsertWafSeq.Location = new System.Drawing.Point(145, 142);
            this.txtInsertWafSeq.Multiline = true;
            this.txtInsertWafSeq.Name = "txtInsertWafSeq";
            this.txtInsertWafSeq.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInsertWafSeq.Size = new System.Drawing.Size(168, 61);
            this.txtInsertWafSeq.TabIndex = 38;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(153, 129);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 12);
            this.label7.TabIndex = 37;
            this.label7.Text = "tWafSeq,mWafSeq";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 147);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(132, 12);
            this.label6.TabIndex = 37;
            this.label6.Text = "wafer 데이터 다시 넣기";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(683, 139);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 24);
            this.label5.TabIndex = 36;
            this.label5.Text = "lot,wafer sum\r\n새로 만들기";
            // 
            // txtSumLot
            // 
            this.txtSumLot.Location = new System.Drawing.Point(661, 186);
            this.txtSumLot.Name = "txtSumLot";
            this.txtSumLot.Size = new System.Drawing.Size(93, 21);
            this.txtSumLot.TabIndex = 35;
            // 
            // btnSum
            // 
            this.btnSum.Location = new System.Drawing.Point(685, 162);
            this.btnSum.Name = "btnSum";
            this.btnSum.Size = new System.Drawing.Size(69, 23);
            this.btnSum.TabIndex = 34;
            this.btnSum.Text = "SUM";
            this.btnSum.UseVisualStyleBackColor = true;
            this.btnSum.Click += new System.EventHandler(this.btnSum_Click);
            // 
            // txtMLotSeq
            // 
            this.txtMLotSeq.Location = new System.Drawing.Point(402, 8);
            this.txtMLotSeq.Multiline = true;
            this.txtMLotSeq.Name = "txtMLotSeq";
            this.txtMLotSeq.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMLotSeq.Size = new System.Drawing.Size(134, 88);
            this.txtMLotSeq.TabIndex = 33;
            // 
            // btnProcessValid
            // 
            this.btnProcessValid.Location = new System.Drawing.Point(537, 96);
            this.btnProcessValid.Name = "btnProcessValid";
            this.btnProcessValid.Size = new System.Drawing.Size(107, 23);
            this.btnProcessValid.TabIndex = 32;
            this.btnProcessValid.Text = "Process Valid";
            this.btnProcessValid.UseVisualStyleBackColor = true;
            this.btnProcessValid.Visible = false;
            this.btnProcessValid.Click += new System.EventHandler(this.btnProcessValid_Click);
            // 
            // btnDeleteUnlinked
            // 
            this.btnDeleteUnlinked.Location = new System.Drawing.Point(537, 63);
            this.btnDeleteUnlinked.Name = "btnDeleteUnlinked";
            this.btnDeleteUnlinked.Size = new System.Drawing.Size(133, 23);
            this.btnDeleteUnlinked.TabIndex = 31;
            this.btnDeleteUnlinked.Text = "DELETE Unlinked";
            this.btnDeleteUnlinked.UseVisualStyleBackColor = true;
            this.btnDeleteUnlinked.Visible = false;
            this.btnDeleteUnlinked.Click += new System.EventHandler(this.btnDeleteUnlinked_Click);
            // 
            // btnUpdateXY
            // 
            this.btnUpdateXY.Location = new System.Drawing.Point(383, 96);
            this.btnUpdateXY.Name = "btnUpdateXY";
            this.btnUpdateXY.Size = new System.Drawing.Size(101, 23);
            this.btnUpdateXY.TabIndex = 30;
            this.btnUpdateXY.Text = "XY -> X, Y";
            this.btnUpdateXY.UseVisualStyleBackColor = true;
            this.btnUpdateXY.Click += new System.EventHandler(this.btnUpdateXY_Click);
            // 
            // btnPROGRAM_PARM_DEF
            // 
            this.btnPROGRAM_PARM_DEF.Location = new System.Drawing.Point(11, 6);
            this.btnPROGRAM_PARM_DEF.Name = "btnPROGRAM_PARM_DEF";
            this.btnPROGRAM_PARM_DEF.Size = new System.Drawing.Size(148, 23);
            this.btnPROGRAM_PARM_DEF.TabIndex = 19;
            this.btnPROGRAM_PARM_DEF.Text = "PROGRAM_PARM_DEF";
            this.btnPROGRAM_PARM_DEF.UseVisualStyleBackColor = true;
            this.btnPROGRAM_PARM_DEF.Click += new System.EventHandler(this.btnPROGRAM_PARM_DEF_Click);
            // 
            // btnDeleteData
            // 
            this.btnDeleteData.Location = new System.Drawing.Point(192, 92);
            this.btnDeleteData.Name = "btnDeleteData";
            this.btnDeleteData.Size = new System.Drawing.Size(49, 23);
            this.btnDeleteData.TabIndex = 29;
            this.btnDeleteData.Text = "RUN";
            this.btnDeleteData.UseVisualStyleBackColor = true;
            this.btnDeleteData.Visible = false;
            this.btnDeleteData.Click += new System.EventHandler(this.btnDeleteData_Click);
            // 
            // btnDATA_TABLES
            // 
            this.btnDATA_TABLES.Location = new System.Drawing.Point(165, 6);
            this.btnDATA_TABLES.Name = "btnDATA_TABLES";
            this.btnDATA_TABLES.Size = new System.Drawing.Size(111, 23);
            this.btnDATA_TABLES.TabIndex = 20;
            this.btnDATA_TABLES.Text = "DATA_TABLES";
            this.btnDATA_TABLES.UseVisualStyleBackColor = true;
            this.btnDATA_TABLES.Click += new System.EventHandler(this.btnDATA_TABLES_Click);
            // 
            // dtpDeleteDate
            // 
            this.dtpDeleteDate.Checked = false;
            this.dtpDeleteDate.CustomFormat = "yyyy-MM-dd";
            this.dtpDeleteDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDeleteDate.Location = new System.Drawing.Point(89, 91);
            this.dtpDeleteDate.Name = "dtpDeleteDate";
            this.dtpDeleteDate.Size = new System.Drawing.Size(97, 21);
            this.dtpDeleteDate.TabIndex = 28;
            this.dtpDeleteDate.Visible = false;
            // 
            // btnCreateTdTable
            // 
            this.btnCreateTdTable.Location = new System.Drawing.Point(282, 6);
            this.btnCreateTdTable.Name = "btnCreateTdTable";
            this.btnCreateTdTable.Size = new System.Drawing.Size(116, 23);
            this.btnCreateTdTable.TabIndex = 21;
            this.btnCreateTdTable.Text = "Create TD tables";
            this.btnCreateTdTable.UseVisualStyleBackColor = true;
            this.btnCreateTdTable.Click += new System.EventHandler(this.btnCreateTdTable_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 12);
            this.label4.TabIndex = 27;
            this.label4.Text = "데이터 삭제";
            this.label4.Visible = false;
            // 
            // btnRunSummary
            // 
            this.btnRunSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRunSummary.Location = new System.Drawing.Point(564, 32);
            this.btnRunSummary.Name = "btnRunSummary";
            this.btnRunSummary.Size = new System.Drawing.Size(155, 23);
            this.btnRunSummary.TabIndex = 22;
            this.btnRunSummary.Text = "Run Summary";
            this.btnRunSummary.UseVisualStyleBackColor = true;
            this.btnRunSummary.Click += new System.EventHandler(this.btnRunSummary_Click);
            // 
            // btnRunTDTableFieldCheckALL
            // 
            this.btnRunTDTableFieldCheckALL.Location = new System.Drawing.Point(159, 65);
            this.btnRunTDTableFieldCheckALL.Name = "btnRunTDTableFieldCheckALL";
            this.btnRunTDTableFieldCheckALL.Size = new System.Drawing.Size(49, 23);
            this.btnRunTDTableFieldCheckALL.TabIndex = 26;
            this.btnRunTDTableFieldCheckALL.Text = "RUN";
            this.btnRunTDTableFieldCheckALL.UseVisualStyleBackColor = true;
            this.btnRunTDTableFieldCheckALL.Click += new System.EventHandler(this.btnRunTDTableFieldCheckALL_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(172, 12);
            this.label2.TabIndex = 23;
            this.label2.Text = "TD테이블 필드 추가 (wafseq):";
            // 
            // btnRunTDTableFieldCheck
            // 
            this.btnRunTDTableFieldCheck.Location = new System.Drawing.Point(291, 38);
            this.btnRunTDTableFieldCheck.Name = "btnRunTDTableFieldCheck";
            this.btnRunTDTableFieldCheck.Size = new System.Drawing.Size(49, 23);
            this.btnRunTDTableFieldCheck.TabIndex = 25;
            this.btnRunTDTableFieldCheck.Text = "RUN";
            this.btnRunTDTableFieldCheck.UseVisualStyleBackColor = true;
            this.btnRunTDTableFieldCheck.Click += new System.EventHandler(this.btnRunTDTableFieldCheck_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 12);
            this.label3.TabIndex = 23;
            this.label3.Text = "TD테이블 필드 추가 ALL";
            // 
            // txtWafSeq_TDTableFieldCheck
            // 
            this.txtWafSeq_TDTableFieldCheck.Location = new System.Drawing.Point(185, 40);
            this.txtWafSeq_TDTableFieldCheck.Name = "txtWafSeq_TDTableFieldCheck";
            this.txtWafSeq_TDTableFieldCheck.Size = new System.Drawing.Size(100, 21);
            this.txtWafSeq_TDTableFieldCheck.TabIndex = 24;
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTest.Location = new System.Drawing.Point(675, 3);
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
            this.btnStop.Location = new System.Drawing.Point(755, 29);
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
            this.btnRun.Location = new System.Drawing.Point(674, 29);
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
            this.listBox1.Location = new System.Drawing.Point(0, 460);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(841, 186);
            this.listBox1.TabIndex = 15;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 457);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(841, 3);
            this.splitter1.TabIndex = 16;
            this.splitter1.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(22, 312);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(195, 24);
            this.label11.TabIndex = 56;
            this.label11.Text = "PROGRM에 대한 테이블의\r\nCOLUMN 정보로 PARASPEC 생성";
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(329, 311);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(49, 23);
            this.button9.TabIndex = 58;
            this.button9.Text = "RUN";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(223, 313);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(100, 21);
            this.textBox5.TabIndex = 57;
            // 
            // FrmMain2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 646);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Name = "FrmMain2";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlFunc.ResumeLayout(false);
            this.pnlFunc.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtEnd;
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
        private System.Windows.Forms.Button btnDATA_TABLES;
        private System.Windows.Forms.Button btnCreateTdTable;
        private System.Windows.Forms.Button btnRunSummary;
        private System.Windows.Forms.Button btnRunTDTableFieldCheck;
        private System.Windows.Forms.TextBox txtWafSeq_TDTableFieldCheck;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRunTDTableFieldCheckALL;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnDeleteData;
        private System.Windows.Forms.DateTimePicker dtpDeleteDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlFunc;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Button btnUpdateXY;
        private System.Windows.Forms.DateTimePicker dtStart;
        private System.Windows.Forms.Button btnDeleteUnlinked;
        private System.Windows.Forms.Button btnProcessValid;
        private System.Windows.Forms.TextBox txtMLotSeq;
        private System.Windows.Forms.TextBox txtSumLot;
        private System.Windows.Forms.Button btnSum;
        private System.Windows.Forms.Button btnInsertWaferData;
        private System.Windows.Forms.TextBox txtInsertWafSeq;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnBin;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.TextBox textBox5;
    }
}

