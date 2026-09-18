namespace DACrux.SEMDMS.ENGUI
{
    partial class frmEquipDefectTrend
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEquipDefectTrend));
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer1 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer2 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.nudDecimalPlaces = new System.Windows.Forms.NumericUpDown();
            this.grbClass = new System.Windows.Forms.GroupBox();
            this.rbDefectiveDie = new System.Windows.Forms.RadioButton();
            this.rbDefects = new System.Windows.Forms.RadioButton();
            this.chkNormalize = new System.Windows.Forms.CheckBox();
            this.dlbClass = new DACrux.Framework.Controls.DUCListBox();
            this.dlbPara = new DACrux.Framework.Controls.DUCListBox();
            this.dlbStep = new DACrux.Framework.Controls.DUCListBox();
            this.dlbLotID = new DACrux.Framework.Controls.DUCListBox();
            this.dlbRoute = new DACrux.Framework.Controls.DUCListBox();
            this.dlbEquip = new DACrux.Framework.Controls.DUCListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dtEnd = new System.Windows.Forms.DateTimePicker();
            this.dtStart = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.rbTimeMode = new System.Windows.Forms.RadioButton();
            this.ckChamber = new System.Windows.Forms.CheckBox();
            this.rbLotMode = new System.Windows.Forms.RadioButton();
            this.rbWaferMode = new System.Windows.Forms.RadioButton();
            this.ckClass = new System.Windows.Forms.CheckBox();
            this.btnView = new System.Windows.Forms.Button();
            this.cmbItem = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nudInterval = new System.Windows.Forms.NumericUpDown();
            this.fpsCommon = new FarPoint.Win.Spread.FpSpread();
            this.fpsCommon_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.ultraSplitter2 = new Infragistics.Win.Misc.UltraSplitter();
            this.chartTrend = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chartConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDecimalPlaces)).BeginInit();
            this.grbClass.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsCommon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsCommon_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartTrend)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.nudDecimalPlaces);
            this.panel1.Controls.Add(this.grbClass);
            this.panel1.Controls.Add(this.dlbClass);
            this.panel1.Controls.Add(this.dlbPara);
            this.panel1.Controls.Add(this.dlbStep);
            this.panel1.Controls.Add(this.dlbLotID);
            this.panel1.Controls.Add(this.dlbRoute);
            this.panel1.Controls.Add(this.dlbEquip);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.btnView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1287, 141);
            this.panel1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1157, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 12);
            this.label3.TabIndex = 73;
            this.label3.Text = "Decimal Places";
            // 
            // nudDecimalPlaces
            // 
            this.nudDecimalPlaces.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudDecimalPlaces.Location = new System.Drawing.Point(1233, 114);
            this.nudDecimalPlaces.Name = "nudDecimalPlaces";
            this.nudDecimalPlaces.Size = new System.Drawing.Size(51, 21);
            this.nudDecimalPlaces.TabIndex = 72;
            this.nudDecimalPlaces.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudDecimalPlaces.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nudDecimalPlaces.ValueChanged += new System.EventHandler(this.nudDecimalPlaces_ValueChanged);
            // 
            // grbClass
            // 
            this.grbClass.Controls.Add(this.rbDefectiveDie);
            this.grbClass.Controls.Add(this.rbDefects);
            this.grbClass.Controls.Add(this.chkNormalize);
            this.grbClass.Dock = System.Windows.Forms.DockStyle.Left;
            this.grbClass.Location = new System.Drawing.Point(1022, 0);
            this.grbClass.Name = "grbClass";
            this.grbClass.Size = new System.Drawing.Size(129, 141);
            this.grbClass.TabIndex = 71;
            this.grbClass.TabStop = false;
            // 
            // rbDefectiveDie
            // 
            this.rbDefectiveDie.AutoSize = true;
            this.rbDefectiveDie.Location = new System.Drawing.Point(6, 67);
            this.rbDefectiveDie.Name = "rbDefectiveDie";
            this.rbDefectiveDie.Size = new System.Drawing.Size(96, 16);
            this.rbDefectiveDie.TabIndex = 71;
            this.rbDefectiveDie.Text = "Defective Die";
            this.rbDefectiveDie.UseVisualStyleBackColor = true;
            // 
            // rbDefects
            // 
            this.rbDefects.AutoSize = true;
            this.rbDefects.Checked = true;
            this.rbDefects.Location = new System.Drawing.Point(6, 45);
            this.rbDefects.Name = "rbDefects";
            this.rbDefects.Size = new System.Drawing.Size(58, 16);
            this.rbDefects.TabIndex = 70;
            this.rbDefects.TabStop = true;
            this.rbDefects.Text = "Defect";
            this.rbDefects.UseVisualStyleBackColor = true;
            // 
            // chkNormalize
            // 
            this.chkNormalize.AutoSize = true;
            this.chkNormalize.Location = new System.Drawing.Point(6, 16);
            this.chkNormalize.Name = "chkNormalize";
            this.chkNormalize.Size = new System.Drawing.Size(82, 16);
            this.chkNormalize.TabIndex = 69;
            this.chkNormalize.Text = "Normalize";
            this.chkNormalize.UseVisualStyleBackColor = true;
            this.chkNormalize.CheckedChanged += new System.EventHandler(this.chkNormalized_CheckedChanged);
            // 
            // dlbClass
            // 
            this.dlbClass.DataSource = null;
            this.dlbClass.DisplayMember = "";
            this.dlbClass.Dock = System.Windows.Forms.DockStyle.Left;
            this.dlbClass.Location = new System.Drawing.Point(837, 0);
            this.dlbClass.Name = "dlbClass";
            this.dlbClass.SearchText = "";
            this.dlbClass.SearchTitle = "Class";
            this.dlbClass.SelectedIndex = -1;
            this.dlbClass.SelectedItem = null;
            this.dlbClass.SelectedValue = null;
            this.dlbClass.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbClass.Size = new System.Drawing.Size(185, 141);
            this.dlbClass.TabIndex = 6;
            this.dlbClass.ValueMember = "";
            // 
            // dlbPara
            // 
            this.dlbPara.DataSource = null;
            this.dlbPara.DisplayMember = "";
            this.dlbPara.Dock = System.Windows.Forms.DockStyle.Left;
            this.dlbPara.Location = new System.Drawing.Point(718, 0);
            this.dlbPara.Name = "dlbPara";
            this.dlbPara.SearchText = "";
            this.dlbPara.SearchTitle = "Chamber Info";
            this.dlbPara.SelectedIndex = -1;
            this.dlbPara.SelectedItem = null;
            this.dlbPara.SelectedValue = null;
            this.dlbPara.Size = new System.Drawing.Size(119, 141);
            this.dlbPara.TabIndex = 6;
            this.dlbPara.ValueMember = "";
            // 
            // dlbStep
            // 
            this.dlbStep.DataSource = null;
            this.dlbStep.DisplayMember = "";
            this.dlbStep.Dock = System.Windows.Forms.DockStyle.Left;
            this.dlbStep.Location = new System.Drawing.Point(606, 0);
            this.dlbStep.Name = "dlbStep";
            this.dlbStep.SearchText = "";
            this.dlbStep.SearchTitle = "Step";
            this.dlbStep.SelectedIndex = -1;
            this.dlbStep.SelectedItem = null;
            this.dlbStep.SelectedValue = null;
            this.dlbStep.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbStep.Size = new System.Drawing.Size(112, 141);
            this.dlbStep.TabIndex = 6;
            this.dlbStep.ValueMember = "";
            this.dlbStep.EnterTextBox += new System.EventHandler(this.dlbStep_EnterTextBox);
            // 
            // dlbLotID
            // 
            this.dlbLotID.DataSource = null;
            this.dlbLotID.DisplayMember = "";
            this.dlbLotID.Dock = System.Windows.Forms.DockStyle.Left;
            this.dlbLotID.Location = new System.Drawing.Point(509, 0);
            this.dlbLotID.Name = "dlbLotID";
            this.dlbLotID.SearchText = "";
            this.dlbLotID.SearchTitle = "Lot ID";
            this.dlbLotID.SelectedIndex = -1;
            this.dlbLotID.SelectedItem = null;
            this.dlbLotID.SelectedValue = null;
            this.dlbLotID.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbLotID.Size = new System.Drawing.Size(97, 141);
            this.dlbLotID.TabIndex = 6;
            this.dlbLotID.ValueMember = "";
            this.dlbLotID.OnSelectedValueChanged += new System.EventHandler(this.dlbLotID_OnSelectedValueDoubleClick);
            this.dlbLotID.EnterTextBox += new System.EventHandler(this.dlbLotID_EnterTextBox);
            // 
            // dlbRoute
            // 
            this.dlbRoute.DataSource = null;
            this.dlbRoute.DisplayMember = "";
            this.dlbRoute.Dock = System.Windows.Forms.DockStyle.Left;
            this.dlbRoute.Location = new System.Drawing.Point(303, 0);
            this.dlbRoute.Name = "dlbRoute";
            this.dlbRoute.SearchText = "";
            this.dlbRoute.SearchTitle = "Route";
            this.dlbRoute.SelectedIndex = -1;
            this.dlbRoute.SelectedItem = null;
            this.dlbRoute.SelectedValue = null;
            this.dlbRoute.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.dlbRoute.Size = new System.Drawing.Size(206, 141);
            this.dlbRoute.TabIndex = 6;
            this.dlbRoute.Tag = "LPT, DESC";
            this.dlbRoute.ValueMember = "";
            this.dlbRoute.OnSelectedValueChanged += new System.EventHandler(this.dlbRoute_OnSelectedValueDoubleClick);
            this.dlbRoute.EnterTextBox += new System.EventHandler(this.dlbRoute_EnterTextBox);
            // 
            // dlbEquip
            // 
            this.dlbEquip.DataSource = null;
            this.dlbEquip.DisplayMember = "";
            this.dlbEquip.Dock = System.Windows.Forms.DockStyle.Left;
            this.dlbEquip.Location = new System.Drawing.Point(206, 0);
            this.dlbEquip.Name = "dlbEquip";
            this.dlbEquip.SearchText = "";
            this.dlbEquip.SearchTitle = "Equip";
            this.dlbEquip.SelectedIndex = -1;
            this.dlbEquip.SelectedItem = null;
            this.dlbEquip.SelectedValue = null;
            this.dlbEquip.Size = new System.Drawing.Size(97, 141);
            this.dlbEquip.TabIndex = 6;
            this.dlbEquip.ValueMember = "";
            this.dlbEquip.OnSelectedValueChanged += new System.EventHandler(this.dlbEquip_OnSelectedValueDoubleClick);
            this.dlbEquip.EnterTextBox += new System.EventHandler(this.dlbEquip_EnterTextBox);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.dtEnd);
            this.groupBox1.Controls.Add(this.dtStart);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.rbTimeMode);
            this.groupBox1.Controls.Add(this.ckChamber);
            this.groupBox1.Controls.Add(this.rbLotMode);
            this.groupBox1.Controls.Add(this.rbWaferMode);
            this.groupBox1.Controls.Add(this.ckClass);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(206, 141);
            this.groupBox1.TabIndex = 70;
            this.groupBox1.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 12);
            this.label6.TabIndex = 13;
            this.label6.Text = "From";
            // 
            // dtEnd
            // 
            this.dtEnd.CustomFormat = "yyyy-MM-dd";
            this.dtEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtEnd.Location = new System.Drawing.Point(56, 40);
            this.dtEnd.Name = "dtEnd";
            this.dtEnd.Size = new System.Drawing.Size(136, 21);
            this.dtEnd.TabIndex = 11;
            this.dtEnd.Value = new System.DateTime(2019, 11, 1, 11, 53, 26, 0);
            // 
            // dtStart
            // 
            this.dtStart.CustomFormat = "yyyy-MM-dd";
            this.dtStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtStart.Location = new System.Drawing.Point(56, 13);
            this.dtStart.Name = "dtStart";
            this.dtStart.Size = new System.Drawing.Size(136, 21);
            this.dtStart.TabIndex = 10;
            this.dtStart.Value = new System.DateTime(2019, 11, 1, 11, 53, 20, 0);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "~ To";
            // 
            // rbTimeMode
            // 
            this.rbTimeMode.AutoSize = true;
            this.rbTimeMode.Checked = true;
            this.rbTimeMode.Location = new System.Drawing.Point(8, 66);
            this.rbTimeMode.Name = "rbTimeMode";
            this.rbTimeMode.Size = new System.Drawing.Size(88, 16);
            this.rbTimeMode.TabIndex = 14;
            this.rbTimeMode.TabStop = true;
            this.rbTimeMode.Text = "Time Mode";
            this.rbTimeMode.UseVisualStyleBackColor = true;
            // 
            // ckChamber
            // 
            this.ckChamber.AutoSize = true;
            this.ckChamber.Checked = true;
            this.ckChamber.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckChamber.Location = new System.Drawing.Point(116, 67);
            this.ckChamber.Name = "ckChamber";
            this.ckChamber.Size = new System.Drawing.Size(76, 16);
            this.ckChamber.TabIndex = 15;
            this.ckChamber.Text = "Chamber";
            this.ckChamber.UseVisualStyleBackColor = true;
            this.ckChamber.CheckedChanged += new System.EventHandler(this.ckChamber_CheckedChanged);
            // 
            // rbLotMode
            // 
            this.rbLotMode.AutoSize = true;
            this.rbLotMode.Location = new System.Drawing.Point(8, 88);
            this.rbLotMode.Name = "rbLotMode";
            this.rbLotMode.Size = new System.Drawing.Size(76, 16);
            this.rbLotMode.TabIndex = 14;
            this.rbLotMode.Text = "Lot Mode";
            this.rbLotMode.UseVisualStyleBackColor = true;
            this.rbLotMode.Visible = false;
            // 
            // rbWaferMode
            // 
            this.rbWaferMode.AutoSize = true;
            this.rbWaferMode.Location = new System.Drawing.Point(8, 109);
            this.rbWaferMode.Name = "rbWaferMode";
            this.rbWaferMode.Size = new System.Drawing.Size(90, 16);
            this.rbWaferMode.TabIndex = 14;
            this.rbWaferMode.Text = "Wafer Mode";
            this.rbWaferMode.UseVisualStyleBackColor = true;
            this.rbWaferMode.Visible = false;
            // 
            // ckClass
            // 
            this.ckClass.AutoSize = true;
            this.ckClass.Location = new System.Drawing.Point(116, 88);
            this.ckClass.Name = "ckClass";
            this.ckClass.Size = new System.Drawing.Size(57, 16);
            this.ckClass.TabIndex = 15;
            this.ckClass.Text = "Class";
            this.ckClass.UseVisualStyleBackColor = true;
            this.ckClass.CheckedChanged += new System.EventHandler(this.ckClass_CheckedChanged);
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnView.BackColor = System.Drawing.Color.White;
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnView.Image = ((System.Drawing.Image)(resources.GetObject("btnView.Image")));
            this.btnView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnView.Location = new System.Drawing.Point(1157, 9);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(127, 31);
            this.btnView.TabIndex = 65;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = false;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // cmbItem
            // 
            this.cmbItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbItem.FormattingEnabled = true;
            this.cmbItem.Location = new System.Drawing.Point(1040, 6);
            this.cmbItem.Name = "cmbItem";
            this.cmbItem.Size = new System.Drawing.Size(124, 20);
            this.cmbItem.TabIndex = 68;
            this.cmbItem.SelectedIndexChanged += new System.EventHandler(this.cmbItem_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1005, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 67;
            this.label1.Text = "Item";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1170, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 12);
            this.label2.TabIndex = 67;
            this.label2.Text = "X Interval";
            // 
            // nudInterval
            // 
            this.nudInterval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nudInterval.Location = new System.Drawing.Point(1233, 6);
            this.nudInterval.Name = "nudInterval";
            this.nudInterval.Size = new System.Drawing.Size(51, 21);
            this.nudInterval.TabIndex = 66;
            this.nudInterval.ValueChanged += new System.EventHandler(this.nudInterval_ValueChanged);
            // 
            // fpsCommon
            // 
            this.fpsCommon.AccessibleDescription = "";
            this.fpsCommon.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.fpsCommon.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpsCommon.HorizontalScrollBar.Name = "";
            enhancedScrollBarRenderer1.ArrowColor = System.Drawing.Color.Black;
            enhancedScrollBarRenderer1.ArrowHoveredColor = System.Drawing.Color.Black;
            enhancedScrollBarRenderer1.ArrowSelectedColor = System.Drawing.Color.Black;
            enhancedScrollBarRenderer1.ButtonBackgroundColor = System.Drawing.Color.Silver;
            enhancedScrollBarRenderer1.ButtonBorderColor = System.Drawing.Color.DarkGray;
            enhancedScrollBarRenderer1.ButtonHoveredBackgroundColor = System.Drawing.Color.DarkGray;
            enhancedScrollBarRenderer1.ButtonHoveredBorderColor = System.Drawing.Color.DimGray;
            enhancedScrollBarRenderer1.ButtonSelectedBackgroundColor = System.Drawing.Color.DimGray;
            enhancedScrollBarRenderer1.ButtonSelectedBorderColor = System.Drawing.Color.Gray;
            enhancedScrollBarRenderer1.TrackBarBackgroundColor = System.Drawing.Color.DarkGray;
            enhancedScrollBarRenderer1.TrackBarSelectedBackgroundColor = System.Drawing.Color.Gray;
            this.fpsCommon.HorizontalScrollBar.Renderer = enhancedScrollBarRenderer1;
            this.fpsCommon.HorizontalScrollBar.TabIndex = 0;
            this.fpsCommon.Location = new System.Drawing.Point(0, 447);
            this.fpsCommon.Name = "fpsCommon";
            this.fpsCommon.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.fpsCommon.ScrollTipPolicy = FarPoint.Win.Spread.ScrollTipPolicy.Both;
            this.fpsCommon.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpsCommon_Sheet1});
            this.fpsCommon.Size = new System.Drawing.Size(1287, 163);
            this.fpsCommon.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Metallic;
            this.fpsCommon.TabIndex = 61;
            this.fpsCommon.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpsCommon.VerticalScrollBar.Name = "";
            enhancedScrollBarRenderer2.ArrowColor = System.Drawing.Color.Black;
            enhancedScrollBarRenderer2.ArrowHoveredColor = System.Drawing.Color.Black;
            enhancedScrollBarRenderer2.ArrowSelectedColor = System.Drawing.Color.Black;
            enhancedScrollBarRenderer2.ButtonBackgroundColor = System.Drawing.Color.Silver;
            enhancedScrollBarRenderer2.ButtonBorderColor = System.Drawing.Color.DarkGray;
            enhancedScrollBarRenderer2.ButtonHoveredBackgroundColor = System.Drawing.Color.DarkGray;
            enhancedScrollBarRenderer2.ButtonHoveredBorderColor = System.Drawing.Color.DimGray;
            enhancedScrollBarRenderer2.ButtonSelectedBackgroundColor = System.Drawing.Color.DimGray;
            enhancedScrollBarRenderer2.ButtonSelectedBorderColor = System.Drawing.Color.Gray;
            enhancedScrollBarRenderer2.TrackBarBackgroundColor = System.Drawing.Color.DarkGray;
            enhancedScrollBarRenderer2.TrackBarSelectedBackgroundColor = System.Drawing.Color.Gray;
            this.fpsCommon.VerticalScrollBar.Renderer = enhancedScrollBarRenderer2;
            this.fpsCommon.VerticalScrollBar.TabIndex = 3;
            // 
            // fpsCommon_Sheet1
            // 
            this.fpsCommon_Sheet1.Reset();
            fpsCommon_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpsCommon_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpsCommon_Sheet1.ColumnCount = 0;
            fpsCommon_Sheet1.RowCount = 0;
            this.fpsCommon_Sheet1.ActiveColumnIndex = -1;
            this.fpsCommon_Sheet1.ActiveRowIndex = -1;
            this.fpsCommon_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpsCommon_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderMetallic";
            this.fpsCommon_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpsCommon_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerMetallic";
            this.fpsCommon_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpsCommon_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderMetallic";
            this.fpsCommon_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpsCommon_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpsCommon_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderMetallic";
            this.fpsCommon_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpsCommon_Sheet1.SheetCornerStyle.Parent = "CornerMetallic";
            this.fpsCommon_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraSplitter1.Location = new System.Drawing.Point(0, 141);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 141;
            this.ultraSplitter1.Size = new System.Drawing.Size(1287, 6);
            this.ultraSplitter1.TabIndex = 62;
            // 
            // ultraSplitter2
            // 
            this.ultraSplitter2.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraSplitter2.Location = new System.Drawing.Point(0, 441);
            this.ultraSplitter2.Name = "ultraSplitter2";
            this.ultraSplitter2.RestoreExtent = 68;
            this.ultraSplitter2.Size = new System.Drawing.Size(1287, 6);
            this.ultraSplitter2.TabIndex = 63;
            // 
            // chartTrend
            // 
            chartArea1.Name = "Default";
            this.chartTrend.ChartAreas.Add(chartArea1);
            this.chartTrend.ContextMenuStrip = this.contextMenuStrip1;
            this.chartTrend.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Default";
            this.chartTrend.Legends.Add(legend1);
            this.chartTrend.Location = new System.Drawing.Point(0, 175);
            this.chartTrend.Name = "chartTrend";
            series1.ChartArea = "Default";
            series1.Legend = "Default";
            series1.Name = "Default";
            this.chartTrend.Series.Add(series1);
            this.chartTrend.Size = new System.Drawing.Size(1287, 266);
            this.chartTrend.TabIndex = 64;
            this.chartTrend.Text = "chart1";
            title1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            title1.Name = "Default";
            title1.Text = "Defect Trend";
            this.chartTrend.Titles.Add(title1);
            this.chartTrend.GetToolTipText += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs>(this.chartTrend_GetToolTipText);
            this.chartTrend.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chartTrend_MouseMove);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetToolStripMenuItem,
            this.chartConfigToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(144, 48);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // chartConfigToolStripMenuItem
            // 
            this.chartConfigToolStripMenuItem.Name = "chartConfigToolStripMenuItem";
            this.chartConfigToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.chartConfigToolStripMenuItem.Text = "Chart Config";
            this.chartConfigToolStripMenuItem.Click += new System.EventHandler(this.chartConfigToolStripMenuItem_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.cmbItem);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.nudInterval);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 147);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1287, 28);
            this.panel2.TabIndex = 65;
            // 
            // frmEquipDefectTrend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1287, 610);
            this.Controls.Add(this.chartTrend);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.ultraSplitter2);
            this.Controls.Add(this.ultraSplitter1);
            this.Controls.Add(this.fpsCommon);
            this.Controls.Add(this.panel1);
            this.Name = "frmEquipDefectTrend";
            this.Text = "Equip Defect Trend";
            this.Load += new System.EventHandler(this.frmEquipDefectTrend_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDecimalPlaces)).EndInit();
            this.grbClass.ResumeLayout(false);
            this.grbClass.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsCommon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsCommon_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartTrend)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread fpsCommon;
        private FarPoint.Win.Spread.SheetView fpsCommon_Sheet1;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartTrend;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtStart;
        private System.Windows.Forms.DateTimePicker dtEnd;
        private System.Windows.Forms.CheckBox ckChamber;
        private System.Windows.Forms.RadioButton rbWaferMode;
        private System.Windows.Forms.RadioButton rbTimeMode;
        private System.Windows.Forms.RadioButton rbLotMode;
        private Framework.Controls.DUCListBox dlbPara;
        private Framework.Controls.DUCListBox dlbLotID;
        private Framework.Controls.DUCListBox dlbStep;
        private Framework.Controls.DUCListBox dlbRoute;
        private Framework.Controls.DUCListBox dlbEquip;
        private System.Windows.Forms.CheckBox ckClass;
        private Framework.Controls.DUCListBox dlbClass;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudInterval;
        private System.Windows.Forms.ToolStripMenuItem chartConfigToolStripMenuItem;
        private System.Windows.Forms.ComboBox cmbItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkNormalize;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox grbClass;
        private System.Windows.Forms.RadioButton rbDefectiveDie;
        private System.Windows.Forms.RadioButton rbDefects;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudDecimalPlaces;


    }
}