namespace DACrux.SEMDMS.ENGUI
{
    partial class frmDMDataTrend
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkScaleBreaks = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbAsc = new System.Windows.Forms.RadioButton();
            this.rbDesc = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbStep = new System.Windows.Forms.RadioButton();
            this.rbWaferID = new System.Windows.Forms.RadioButton();
            this.rbResulTime = new System.Windows.Forms.RadioButton();
            this.rbLotID = new System.Windows.Forms.RadioButton();
            this.dlbDefectClass = new DACrux.Framework.Controls.DUCListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.nudDecimalPlaces = new System.Windows.Forms.NumericUpDown();
            this.btnView = new System.Windows.Forms.Button();
            this.dlbWafer = new DACrux.Framework.Controls.DUCListBox();
            this.dlbLot = new DACrux.Framework.Controls.DUCListBox();
            this.dlbStep = new DACrux.Framework.Controls.DUCListBox();
            this.dlbProduct = new DACrux.Framework.Controls.DUCListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lstXItem = new System.Windows.Forms.ListBox();
            this.chkLastInspection = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.toDate = new System.Windows.Forms.DateTimePicker();
            this.fromDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.fpSpread1 = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDecimalPlaces)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkScaleBreaks);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.dlbDefectClass);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.nudDecimalPlaces);
            this.panel1.Controls.Add(this.btnView);
            this.panel1.Controls.Add(this.dlbWafer);
            this.panel1.Controls.Add(this.dlbLot);
            this.panel1.Controls.Add(this.dlbStep);
            this.panel1.Controls.Add(this.dlbProduct);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1353, 161);
            this.panel1.TabIndex = 1;
            // 
            // chkScaleBreaks
            // 
            this.chkScaleBreaks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkScaleBreaks.AutoSize = true;
            this.chkScaleBreaks.Location = new System.Drawing.Point(1254, 120);
            this.chkScaleBreaks.Name = "chkScaleBreaks";
            this.chkScaleBreaks.Size = new System.Drawing.Size(92, 16);
            this.chkScaleBreaks.TabIndex = 25;
            this.chkScaleBreaks.Text = "Scale Break";
            this.chkScaleBreaks.UseVisualStyleBackColor = true;
            this.chkScaleBreaks.CheckedChanged += new System.EventHandler(this.chkScaleBreaks_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbAsc);
            this.groupBox2.Controls.Add(this.rbDesc);
            this.groupBox2.Location = new System.Drawing.Point(956, 100);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(123, 52);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            // 
            // rbAsc
            // 
            this.rbAsc.AutoSize = true;
            this.rbAsc.Checked = true;
            this.rbAsc.Location = new System.Drawing.Point(5, 10);
            this.rbAsc.Name = "rbAsc";
            this.rbAsc.Size = new System.Drawing.Size(83, 16);
            this.rbAsc.TabIndex = 27;
            this.rbAsc.TabStop = true;
            this.rbAsc.Text = "Ascending";
            this.rbAsc.UseVisualStyleBackColor = true;
            // 
            // rbDesc
            // 
            this.rbDesc.AutoSize = true;
            this.rbDesc.Location = new System.Drawing.Point(5, 32);
            this.rbDesc.Name = "rbDesc";
            this.rbDesc.Size = new System.Drawing.Size(90, 16);
            this.rbDesc.TabIndex = 28;
            this.rbDesc.Text = "Descending";
            this.rbDesc.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbStep);
            this.groupBox1.Controls.Add(this.rbWaferID);
            this.groupBox1.Controls.Add(this.rbResulTime);
            this.groupBox1.Controls.Add(this.rbLotID);
            this.groupBox1.Location = new System.Drawing.Point(956, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(123, 104);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            // 
            // rbStep
            // 
            this.rbStep.AutoSize = true;
            this.rbStep.Location = new System.Drawing.Point(5, 82);
            this.rbStep.Name = "rbStep";
            this.rbStep.Size = new System.Drawing.Size(93, 16);
            this.rbStep.TabIndex = 30;
            this.rbStep.Text = "Sort By Step";
            this.rbStep.UseVisualStyleBackColor = true;
            // 
            // rbWaferID
            // 
            this.rbWaferID.AutoSize = true;
            this.rbWaferID.Checked = true;
            this.rbWaferID.Location = new System.Drawing.Point(5, 37);
            this.rbWaferID.Name = "rbWaferID";
            this.rbWaferID.Size = new System.Drawing.Size(99, 16);
            this.rbWaferID.TabIndex = 29;
            this.rbWaferID.TabStop = true;
            this.rbWaferID.Text = "Sort By Wafer";
            this.rbWaferID.UseVisualStyleBackColor = true;
            // 
            // rbResulTime
            // 
            this.rbResulTime.AutoSize = true;
            this.rbResulTime.Location = new System.Drawing.Point(5, 12);
            this.rbResulTime.Name = "rbResulTime";
            this.rbResulTime.Size = new System.Drawing.Size(97, 16);
            this.rbResulTime.TabIndex = 27;
            this.rbResulTime.Text = "Sort By Time";
            this.rbResulTime.UseVisualStyleBackColor = true;
            // 
            // rbLotID
            // 
            this.rbLotID.AutoSize = true;
            this.rbLotID.Location = new System.Drawing.Point(5, 60);
            this.rbLotID.Name = "rbLotID";
            this.rbLotID.Size = new System.Drawing.Size(85, 16);
            this.rbLotID.TabIndex = 28;
            this.rbLotID.Text = "Sort By Lot";
            this.rbLotID.UseVisualStyleBackColor = true;
            // 
            // dlbDefectClass
            // 
            this.dlbDefectClass.DataSource = null;
            this.dlbDefectClass.DisplayMember = "";
            this.dlbDefectClass.Dock = System.Windows.Forms.DockStyle.Left;
            this.dlbDefectClass.Location = new System.Drawing.Point(800, 0);
            this.dlbDefectClass.Name = "dlbDefectClass";
            this.dlbDefectClass.SearchText = "";
            this.dlbDefectClass.SearchTitle = "Defect Class";
            this.dlbDefectClass.SelectedIndex = -1;
            this.dlbDefectClass.SelectedItem = null;
            this.dlbDefectClass.SelectedValue = null;
            this.dlbDefectClass.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbDefectClass.Size = new System.Drawing.Size(150, 161);
            this.dlbDefectClass.TabIndex = 26;
            this.dlbDefectClass.ValueMember = "";
            this.dlbDefectClass.OnSelectedIndexChanged += new System.EventHandler(this.dlbDefectClass_OnSelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1252, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 12);
            this.label4.TabIndex = 25;
            this.label4.Text = "Decimal Places";
            // 
            // nudDecimalPlaces
            // 
            this.nudDecimalPlaces.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudDecimalPlaces.Location = new System.Drawing.Point(1254, 93);
            this.nudDecimalPlaces.Name = "nudDecimalPlaces";
            this.nudDecimalPlaces.Size = new System.Drawing.Size(96, 21);
            this.nudDecimalPlaces.TabIndex = 24;
            this.nudDecimalPlaces.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudDecimalPlaces.ValueChanged += new System.EventHandler(this.nudDecimalPlaces_ValueChanged);
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnView.Location = new System.Drawing.Point(1254, 4);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(96, 49);
            this.btnView.TabIndex = 23;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // dlbWafer
            // 
            this.dlbWafer.DataSource = null;
            this.dlbWafer.DisplayMember = "";
            this.dlbWafer.Dock = System.Windows.Forms.DockStyle.Left;
            this.dlbWafer.Location = new System.Drawing.Point(650, 0);
            this.dlbWafer.Name = "dlbWafer";
            this.dlbWafer.SearchText = "";
            this.dlbWafer.SearchTitle = "Wafer ID";
            this.dlbWafer.SelectedIndex = -1;
            this.dlbWafer.SelectedItem = null;
            this.dlbWafer.SelectedValue = null;
            this.dlbWafer.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbWafer.Size = new System.Drawing.Size(150, 161);
            this.dlbWafer.TabIndex = 22;
            this.dlbWafer.ValueMember = "";
            this.dlbWafer.OnSelectedIndexChanged += new System.EventHandler(this.dlbWafer_OnSelectedIndexChanged);
            // 
            // dlbLot
            // 
            this.dlbLot.DataSource = null;
            this.dlbLot.DisplayMember = "";
            this.dlbLot.Dock = System.Windows.Forms.DockStyle.Left;
            this.dlbLot.Location = new System.Drawing.Point(500, 0);
            this.dlbLot.Name = "dlbLot";
            this.dlbLot.SearchText = "";
            this.dlbLot.SearchTitle = "Lot ID";
            this.dlbLot.SelectedIndex = -1;
            this.dlbLot.SelectedItem = null;
            this.dlbLot.SelectedValue = null;
            this.dlbLot.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbLot.Size = new System.Drawing.Size(150, 161);
            this.dlbLot.TabIndex = 21;
            this.dlbLot.ValueMember = "";
            this.dlbLot.OnSelectedIndexChanged += new System.EventHandler(this.dlbLot_OnSelectedIndexChanged);
            // 
            // dlbStep
            // 
            this.dlbStep.DataSource = null;
            this.dlbStep.DisplayMember = "";
            this.dlbStep.Dock = System.Windows.Forms.DockStyle.Left;
            this.dlbStep.Location = new System.Drawing.Point(350, 0);
            this.dlbStep.Name = "dlbStep";
            this.dlbStep.SearchText = "";
            this.dlbStep.SearchTitle = "Step ID";
            this.dlbStep.SelectedIndex = -1;
            this.dlbStep.SelectedItem = null;
            this.dlbStep.SelectedValue = null;
            this.dlbStep.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbStep.Size = new System.Drawing.Size(150, 161);
            this.dlbStep.TabIndex = 20;
            this.dlbStep.ValueMember = "";
            this.dlbStep.OnSelectedIndexChanged += new System.EventHandler(this.dlbStep_OnSelectedIndexChanged);
            // 
            // dlbProduct
            // 
            this.dlbProduct.DataSource = null;
            this.dlbProduct.DisplayMember = "";
            this.dlbProduct.Dock = System.Windows.Forms.DockStyle.Left;
            this.dlbProduct.Location = new System.Drawing.Point(200, 0);
            this.dlbProduct.Name = "dlbProduct";
            this.dlbProduct.SearchText = "";
            this.dlbProduct.SearchTitle = "Device";
            this.dlbProduct.SelectedIndex = -1;
            this.dlbProduct.SelectedItem = null;
            this.dlbProduct.SelectedValue = null;
            this.dlbProduct.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbProduct.Size = new System.Drawing.Size(150, 161);
            this.dlbProduct.TabIndex = 19;
            this.dlbProduct.ValueMember = "";
            this.dlbProduct.OnSelectedIndexChanged += new System.EventHandler(this.dlbProduct_OnSelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lstXItem);
            this.panel2.Controls.Add(this.chkLastInspection);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.toDate);
            this.panel2.Controls.Add(this.fromDate);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 161);
            this.panel2.TabIndex = 18;
            // 
            // lstXItem
            // 
            this.lstXItem.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lstXItem.FormattingEnabled = true;
            this.lstXItem.ItemHeight = 12;
            this.lstXItem.Items.AddRange(new object[] {
            "TOTALDEFECT",
            "TOTALCLUSTER",
            "TOTALRANDOM",
            "TOTAL0_5UM",
            "TOTAL0_7UM",
            "TOTAL1UM",
            "TOTAL2UM",
            "TOTAL3UM",
            "TOTAL4UM",
            "TOTAL5UM",
            "TOTAL6UM",
            "TOTAL7UM",
            "TOTALOS",
            "TOTALRANDOM0_5UM",
            "TOTALRANDOM0_7UM",
            "TOTALRANDOM1UM",
            "TOTALRANDOM2UM",
            "TOTALRANDOM3UM",
            "TOTALRANDOM4UM",
            "TOTALRANDOM5UM",
            "TOTALRANDOM6UM",
            "TOTALRANDOM7UM",
            "TOTALRANDOMOS",
            "TOTALDEFECTDIE",
            "TOTALRANDOMDIE",
            "TOTALCLUSTERDIE",
            "NEWDEFECT",
            "NEWCLUSTER",
            "NEWRANDOM",
            "NEW0_5UM",
            "NEW0_7UM",
            "NEW1UM",
            "NEW2UM",
            "NEW3UM",
            "NEW4UM",
            "NEW5UM",
            "NEW6UM",
            "NEW7UM",
            "NEWOS",
            "NEWRANDOM0_5UM",
            "NEWRANDOM0_7UM",
            "NEWRANDOM1UM",
            "NEWRANDOM2UM",
            "NEWRANDOM3UM",
            "NEWRANDOM4UM",
            "NEWRANDOM5UM",
            "NEWRANDOM6UM",
            "NEWRANDOM7UM",
            "NEWRANDOMOS",
            "NEWDEFECTDIE",
            "NEWRANDOMDIE",
            "NEWCLUSTERDIE"});
            this.lstXItem.Location = new System.Drawing.Point(0, 97);
            this.lstXItem.Name = "lstXItem";
            this.lstXItem.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstXItem.Size = new System.Drawing.Size(200, 64);
            this.lstXItem.TabIndex = 25;
            // 
            // chkLastInspection
            // 
            this.chkLastInspection.AutoSize = true;
            this.chkLastInspection.Location = new System.Drawing.Point(67, 52);
            this.chkLastInspection.Name = "chkLastInspection";
            this.chkLastInspection.Size = new System.Drawing.Size(110, 16);
            this.chkLastInspection.TabIndex = 24;
            this.chkLastInspection.Text = "Last Inspection";
            this.chkLastInspection.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 22;
            this.label3.Text = "X Item";
            // 
            // toDate
            // 
            this.toDate.CustomFormat = "yyyy-MM-dd";
            this.toDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.toDate.Location = new System.Drawing.Point(67, 25);
            this.toDate.Name = "toDate";
            this.toDate.Size = new System.Drawing.Size(130, 21);
            this.toDate.TabIndex = 21;
            this.toDate.ValueChanged += new System.EventHandler(this.date_ValueChanged);
            // 
            // fromDate
            // 
            this.fromDate.CustomFormat = "yyyy-MM-dd";
            this.fromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.fromDate.Location = new System.Drawing.Point(67, 3);
            this.fromDate.Name = "fromDate";
            this.fromDate.Size = new System.Drawing.Size(130, 21);
            this.fromDate.TabIndex = 20;
            this.fromDate.ValueChanged += new System.EventHandler(this.date_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 12);
            this.label2.TabIndex = 19;
            this.label2.Text = "End Date";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 18;
            this.label1.Text = "Start Date";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.fpSpread1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 317);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1353, 209);
            this.panel3.TabIndex = 3;
            // 
            // fpSpread1
            // 
            this.fpSpread1.AccessibleDescription = "";
            this.fpSpread1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpread1.Location = new System.Drawing.Point(0, 0);
            this.fpSpread1.Name = "fpSpread1";
            this.fpSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread1_Sheet1});
            this.fpSpread1.Size = new System.Drawing.Size(1353, 209);
            this.fpSpread1.TabIndex = 3;
            this.fpSpread1.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpSpread1_CellClick);
            // 
            // fpSpread1_Sheet1
            // 
            this.fpSpread1_Sheet1.Reset();
            fpSpread1_Sheet1.SheetName = "Sheet1";
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraSplitter1.Location = new System.Drawing.Point(0, 311);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 146;
            this.ultraSplitter1.Size = new System.Drawing.Size(1353, 6);
            this.ultraSplitter1.TabIndex = 4;
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.ContextMenuStrip = this.contextMenuStrip1;
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(0, 161);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(1353, 150);
            this.chart1.TabIndex = 5;
            this.chart1.Text = "chart1";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(103, 26);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.tsm_Click);
            // 
            // frmDMDataTrend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1353, 526);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.ultraSplitter1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "frmDMDataTrend";
            this.Text = "frmDMDataTrend";
            this.Load += new System.EventHandler(this.frmDMDataTrend_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDecimalPlaces)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbAsc;
        private System.Windows.Forms.RadioButton rbDesc;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbWaferID;
        private System.Windows.Forms.RadioButton rbResulTime;
        private System.Windows.Forms.RadioButton rbLotID;
        private Framework.Controls.DUCListBox dlbDefectClass;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudDecimalPlaces;
        private System.Windows.Forms.Button btnView;
        private Framework.Controls.DUCListBox dlbWafer;
        private Framework.Controls.DUCListBox dlbLot;
        private Framework.Controls.DUCListBox dlbStep;
        private Framework.Controls.DUCListBox dlbProduct;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker toDate;
        private System.Windows.Forms.DateTimePicker fromDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkLastInspection;
        private System.Windows.Forms.Panel panel3;
        private FarPoint.Win.Spread.FpSpread fpSpread1;
        private FarPoint.Win.Spread.SheetView fpSpread1_Sheet1;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.CheckBox chkScaleBreaks;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.RadioButton rbStep;
        private System.Windows.Forms.ListBox lstXItem;
    }
}