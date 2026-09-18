namespace DACrux.MapAnalysis.Control
{
    partial class TPUCMapGallery : DACrux.Framework.Base.DACruxCTLBasic01
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TPUCMapGallery));
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane1 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.Floating, new System.Guid("85d3d877-76e6-4d10-aa19-f45ca13e2507"));
            Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane2 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.Floating, new System.Guid("8ad30bcb-99d9-4934-92ac-5603f0fe3a02"));
            Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane3 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedRight, new System.Guid("ba5b14b4-13f7-4422-827b-49997e7cd755"));
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane1 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("0abafeea-0777-441b-bb8d-a6a9590aa5e0"), new System.Guid("8ad30bcb-99d9-4934-92ac-5603f0fe3a02"), -1, new System.Guid("ba5b14b4-13f7-4422-827b-49997e7cd755"), 0);
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane2 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("aa3139ea-fe00-4065-bec5-39c49cf52cf0"), new System.Guid("85d3d877-76e6-4d10-aa19-f45ca13e2507"), -1, new System.Guid("ba5b14b4-13f7-4422-827b-49997e7cd755"), 1);
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane3 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("42761205-59d1-4e5e-af00-f912b6a08d00"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("ba5b14b4-13f7-4422-827b-49997e7cd755"), -1);
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane4 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("ac66a4ef-3cc5-4ce8-b425-5e85af5ff8a8"), new System.Guid("4e91b0b0-768d-41ee-9ee5-e07866f4c8f2"), -1, new System.Guid("ba5b14b4-13f7-4422-827b-49997e7cd755"), 3);
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane5 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("7026e3b2-18e8-4194-a8dc-300ea6734498"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("ba5b14b4-13f7-4422-827b-49997e7cd755"), -1);
            Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane4 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.Floating, new System.Guid("4e91b0b0-768d-41ee-9ee5-e07866f4c8f2"));
            this.pnlWaferMapInfo = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlParaItem = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lsParaList = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.chkTotalPara = new System.Windows.Forms.CheckBox();
            this.txtItemFilter = new System.Windows.Forms.TextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel5 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.FILTER = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.clbParaCut = new System.Windows.Forms.CheckedListBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cbCutCnt = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SPEC = new System.Windows.Forms.TabPage();
            this.txtLTL = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtUTL = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.chkTightenLimit = new System.Windows.Forms.CheckBox();
            this.txtLCL = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtUCL = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.chkControlLimit = new System.Windows.Forms.CheckBox();
            this.txtLSL = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtUSL = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkSpecificationLimit = new System.Windows.Forms.CheckBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnTestDataDraw = new System.Windows.Forms.Button();
            this.BtnParaAnalysis = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlChart = new System.Windows.Forms.Panel();
            this.BinTrendChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chkGoodBin = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.fpRawData = new FarPoint.Win.Spread.FpSpread();
            this.fpRawData_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.AVIImageList = new System.Windows.Forms.FlowLayoutPanel();
            this.mapContainer = new System.Windows.Forms.FlowLayoutPanel();
            this._TPUCMapViewAutoHideControl = new Infragistics.Win.UltraWinDock.AutoHideControl();
            this.ultraDockManager1 = new Infragistics.Win.UltraWinDock.UltraDockManager(this.components);
            this._TPUCMapViewUnpinnedTabAreaRight = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._TPUCMapViewUnpinnedTabAreaBottom = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._TPUCMapViewUnpinnedTabAreaTop = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._TPUCMapViewUnpinnedTabAreaLeft = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this.windowDockingArea5 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
            this.windowDockingArea1 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
            this.windowDockingArea6 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
            this.dockableWindow2 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.dockableWindow3 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.dockableWindow5 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.dockableWindow1 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.dockableWindow4 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.windowDockingArea9 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
            this.panel8 = new System.Windows.Forms.Panel();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlWaferMapInfo.SuspendLayout();
            this.pnlParaItem.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.panel5.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.FILTER.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SPEC.SuspendLayout();
            this.panel7.SuspendLayout();
            this.pnlChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BinTrendChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpRawData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpRawData_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager1)).BeginInit();
            this.windowDockingArea6.SuspendLayout();
            this.dockableWindow2.SuspendLayout();
            this.dockableWindow3.SuspendLayout();
            this.dockableWindow5.SuspendLayout();
            this.dockableWindow1.SuspendLayout();
            this.dockableWindow4.SuspendLayout();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlWaferMapInfo
            // 
            this.pnlWaferMapInfo.Controls.Add(this.label1);
            this.pnlWaferMapInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlWaferMapInfo.Location = new System.Drawing.Point(0, 20);
            this.pnlWaferMapInfo.Name = "pnlWaferMapInfo";
            this.pnlWaferMapInfo.Padding = new System.Windows.Forms.Padding(2);
            this.pnlWaferMapInfo.Size = new System.Drawing.Size(311, 574);
            this.pnlWaferMapInfo.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightSlateGray;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(2, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(307, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "    Bin Select";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlParaItem
            // 
            this.pnlParaItem.Controls.Add(this.panel6);
            this.pnlParaItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlParaItem.Location = new System.Drawing.Point(0, 20);
            this.pnlParaItem.Name = "pnlParaItem";
            this.pnlParaItem.Padding = new System.Windows.Forms.Padding(2);
            this.pnlParaItem.Size = new System.Drawing.Size(288, 574);
            this.pnlParaItem.TabIndex = 12;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.panel1);
            this.panel6.Controls.Add(this.splitter1);
            this.panel6.Controls.Add(this.panel5);
            this.panel6.Controls.Add(this.label3);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(2, 2);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(284, 570);
            this.panel6.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lsParaList);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.txtItemFilter);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 20);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 277);
            this.panel1.TabIndex = 32;
            // 
            // lsParaList
            // 
            this.lsParaList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsParaList.FormattingEnabled = true;
            this.lsParaList.ItemHeight = 12;
            this.lsParaList.Location = new System.Drawing.Point(0, 21);
            this.lsParaList.Name = "lsParaList";
            this.lsParaList.Size = new System.Drawing.Size(284, 226);
            this.lsParaList.TabIndex = 31;
            this.lsParaList.SelectedIndexChanged += new System.EventHandler(this.lsParaList_SelectedIndexChanged);
            this.lsParaList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lsParaList_KeyDown);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.numericUpDown1);
            this.panel2.Controls.Add(this.chkTotalPara);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 247);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(284, 30);
            this.panel2.TabIndex = 33;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(143, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 12);
            this.label7.TabIndex = 3;
            this.label7.Text = "Decimal Length";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown1.Location = new System.Drawing.Point(242, 5);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(37, 21);
            this.numericUpDown1.TabIndex = 2;
            this.numericUpDown1.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // chkTotalPara
            // 
            this.chkTotalPara.AutoSize = true;
            this.chkTotalPara.Location = new System.Drawing.Point(3, 8);
            this.chkTotalPara.Name = "chkTotalPara";
            this.chkTotalPara.Size = new System.Drawing.Size(82, 16);
            this.chkTotalPara.TabIndex = 1;
            this.chkTotalPara.Text = "Total Para";
            this.chkTotalPara.UseVisualStyleBackColor = true;
            this.chkTotalPara.CheckedChanged += new System.EventHandler(this.chkTotalPara_CheckedChanged);
            // 
            // txtItemFilter
            // 
            this.txtItemFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtItemFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtItemFilter.Location = new System.Drawing.Point(0, 0);
            this.txtItemFilter.Name = "txtItemFilter";
            this.txtItemFilter.Size = new System.Drawing.Size(284, 21);
            this.txtItemFilter.TabIndex = 33;
            this.txtItemFilter.TextChanged += new System.EventHandler(this.txtItemFilter_TextChanged);
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 297);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(284, 5);
            this.splitter1.TabIndex = 35;
            this.splitter1.TabStop = false;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.tabControl1);
            this.panel5.Controls.Add(this.panel7);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 302);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(284, 268);
            this.panel5.TabIndex = 34;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.FILTER);
            this.tabControl1.Controls.Add(this.SPEC);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 62);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(284, 206);
            this.tabControl1.TabIndex = 36;
            // 
            // FILTER
            // 
            this.FILTER.Controls.Add(this.panel4);
            this.FILTER.Location = new System.Drawing.Point(4, 22);
            this.FILTER.Name = "FILTER";
            this.FILTER.Padding = new System.Windows.Forms.Padding(3);
            this.FILTER.Size = new System.Drawing.Size(276, 180);
            this.FILTER.TabIndex = 0;
            this.FILTER.Text = "Section Filter";
            this.FILTER.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.clbParaCut);
            this.panel4.Controls.Add(this.panel3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(270, 174);
            this.panel4.TabIndex = 5;
            // 
            // clbParaCut
            // 
            this.clbParaCut.CheckOnClick = true;
            this.clbParaCut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbParaCut.IntegralHeight = false;
            this.clbParaCut.Location = new System.Drawing.Point(0, 23);
            this.clbParaCut.Name = "clbParaCut";
            this.clbParaCut.Size = new System.Drawing.Size(270, 151);
            this.clbParaCut.TabIndex = 34;
            this.clbParaCut.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbParaCut_ItemCheck);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.cbCutCnt);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(270, 23);
            this.panel3.TabIndex = 33;
            // 
            // cbCutCnt
            // 
            this.cbCutCnt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCutCnt.Items.AddRange(new object[] {
            "1",
            "5",
            "10",
            "25",
            "50",
            "100"});
            this.cbCutCnt.Location = new System.Drawing.Point(85, 0);
            this.cbCutCnt.Name = "cbCutCnt";
            this.cbCutCnt.Size = new System.Drawing.Size(185, 20);
            this.cbCutCnt.TabIndex = 3;
            this.cbCutCnt.Text = "5";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.LightSlateGray;
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label4.Image = ((System.Drawing.Image)(resources.GetObject("label4.Image")));
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 23);
            this.label4.TabIndex = 2;
            this.label4.Text = "   Cut Cnt";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SPEC
            // 
            this.SPEC.Controls.Add(this.txtLTL);
            this.SPEC.Controls.Add(this.label11);
            this.SPEC.Controls.Add(this.txtUTL);
            this.SPEC.Controls.Add(this.label12);
            this.SPEC.Controls.Add(this.chkTightenLimit);
            this.SPEC.Controls.Add(this.txtLCL);
            this.SPEC.Controls.Add(this.label9);
            this.SPEC.Controls.Add(this.txtUCL);
            this.SPEC.Controls.Add(this.label10);
            this.SPEC.Controls.Add(this.chkControlLimit);
            this.SPEC.Controls.Add(this.txtLSL);
            this.SPEC.Controls.Add(this.label8);
            this.SPEC.Controls.Add(this.txtUSL);
            this.SPEC.Controls.Add(this.label6);
            this.SPEC.Controls.Add(this.chkSpecificationLimit);
            this.SPEC.Location = new System.Drawing.Point(4, 22);
            this.SPEC.Name = "SPEC";
            this.SPEC.Padding = new System.Windows.Forms.Padding(3);
            this.SPEC.Size = new System.Drawing.Size(276, 180);
            this.SPEC.TabIndex = 1;
            this.SPEC.Text = "Spec Limit";
            this.SPEC.UseVisualStyleBackColor = true;
            // 
            // txtLTL
            // 
            this.txtLTL.Location = new System.Drawing.Point(166, 134);
            this.txtLTL.Name = "txtLTL";
            this.txtLTL.ReadOnly = true;
            this.txtLTL.Size = new System.Drawing.Size(100, 21);
            this.txtLTL.TabIndex = 14;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(139, 137);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(27, 12);
            this.label11.TabIndex = 13;
            this.label11.Text = "LTL";
            // 
            // txtUTL
            // 
            this.txtUTL.Location = new System.Drawing.Point(33, 134);
            this.txtUTL.Name = "txtUTL";
            this.txtUTL.ReadOnly = true;
            this.txtUTL.Size = new System.Drawing.Size(100, 21);
            this.txtUTL.TabIndex = 12;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 137);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(28, 12);
            this.label12.TabIndex = 11;
            this.label12.Text = "UTL";
            // 
            // chkTightenLimit
            // 
            this.chkTightenLimit.AutoSize = true;
            this.chkTightenLimit.Location = new System.Drawing.Point(6, 114);
            this.chkTightenLimit.Name = "chkTightenLimit";
            this.chkTightenLimit.Size = new System.Drawing.Size(97, 16);
            this.chkTightenLimit.TabIndex = 10;
            this.chkTightenLimit.Tag = "TIGHTEN";
            this.chkTightenLimit.Text = "Tighten Limit";
            this.chkTightenLimit.UseVisualStyleBackColor = true;
            this.chkTightenLimit.CheckedChanged += new System.EventHandler(this.chkToleranceLimit_CheckedChanged);
            // 
            // txtLCL
            // 
            this.txtLCL.Location = new System.Drawing.Point(166, 81);
            this.txtLCL.Name = "txtLCL";
            this.txtLCL.ReadOnly = true;
            this.txtLCL.Size = new System.Drawing.Size(100, 21);
            this.txtLCL.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(139, 84);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(28, 12);
            this.label9.TabIndex = 8;
            this.label9.Text = "LCL";
            // 
            // txtUCL
            // 
            this.txtUCL.Location = new System.Drawing.Point(33, 81);
            this.txtUCL.Name = "txtUCL";
            this.txtUCL.ReadOnly = true;
            this.txtUCL.Size = new System.Drawing.Size(100, 21);
            this.txtUCL.TabIndex = 7;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 84);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 12);
            this.label10.TabIndex = 6;
            this.label10.Text = "UCL";
            // 
            // chkControlLimit
            // 
            this.chkControlLimit.AutoSize = true;
            this.chkControlLimit.Location = new System.Drawing.Point(6, 61);
            this.chkControlLimit.Name = "chkControlLimit";
            this.chkControlLimit.Size = new System.Drawing.Size(95, 16);
            this.chkControlLimit.TabIndex = 5;
            this.chkControlLimit.Tag = "CONTORL";
            this.chkControlLimit.Text = "Control Limit";
            this.chkControlLimit.UseVisualStyleBackColor = true;
            this.chkControlLimit.CheckedChanged += new System.EventHandler(this.chkControlLimit_CheckedChanged);
            // 
            // txtLSL
            // 
            this.txtLSL.Location = new System.Drawing.Point(166, 28);
            this.txtLSL.Name = "txtLSL";
            this.txtLSL.ReadOnly = true;
            this.txtLSL.Size = new System.Drawing.Size(100, 21);
            this.txtLSL.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(139, 31);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(27, 12);
            this.label8.TabIndex = 3;
            this.label8.Text = "LSL";
            // 
            // txtUSL
            // 
            this.txtUSL.Location = new System.Drawing.Point(33, 28);
            this.txtUSL.Name = "txtUSL";
            this.txtUSL.ReadOnly = true;
            this.txtUSL.Size = new System.Drawing.Size(100, 21);
            this.txtUSL.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "USL";
            // 
            // chkSpecificationLimit
            // 
            this.chkSpecificationLimit.AutoSize = true;
            this.chkSpecificationLimit.Location = new System.Drawing.Point(6, 7);
            this.chkSpecificationLimit.Name = "chkSpecificationLimit";
            this.chkSpecificationLimit.Size = new System.Drawing.Size(127, 16);
            this.chkSpecificationLimit.TabIndex = 0;
            this.chkSpecificationLimit.Tag = "SPECIFICATION";
            this.chkSpecificationLimit.Text = "Specification Limit";
            this.chkSpecificationLimit.UseVisualStyleBackColor = true;
            this.chkSpecificationLimit.CheckedChanged += new System.EventHandler(this.chkSpecificationLimit_CheckedChanged);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.btnTestDataDraw);
            this.panel7.Controls.Add(this.BtnParaAnalysis);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(284, 62);
            this.panel7.TabIndex = 35;
            // 
            // btnTestDataDraw
            // 
            this.btnTestDataDraw.BackColor = System.Drawing.Color.Ivory;
            this.btnTestDataDraw.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestDataDraw.ForeColor = System.Drawing.Color.Black;
            this.btnTestDataDraw.Image = ((System.Drawing.Image)(resources.GetObject("btnTestDataDraw.Image")));
            this.btnTestDataDraw.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTestDataDraw.Location = new System.Drawing.Point(109, 3);
            this.btnTestDataDraw.Name = "btnTestDataDraw";
            this.btnTestDataDraw.Size = new System.Drawing.Size(105, 57);
            this.btnTestDataDraw.TabIndex = 32;
            this.btnTestDataDraw.Text = "          Map \r\n          Draw";
            this.btnTestDataDraw.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTestDataDraw.UseVisualStyleBackColor = false;
            this.btnTestDataDraw.Click += new System.EventHandler(this.btnTestDataDraw_Click);
            // 
            // BtnParaAnalysis
            // 
            this.BtnParaAnalysis.BackColor = System.Drawing.Color.Ivory;
            this.BtnParaAnalysis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnParaAnalysis.ForeColor = System.Drawing.Color.Black;
            this.BtnParaAnalysis.Image = ((System.Drawing.Image)(resources.GetObject("BtnParaAnalysis.Image")));
            this.BtnParaAnalysis.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnParaAnalysis.Location = new System.Drawing.Point(3, 3);
            this.BtnParaAnalysis.Name = "BtnParaAnalysis";
            this.BtnParaAnalysis.Size = new System.Drawing.Size(105, 57);
            this.BtnParaAnalysis.TabIndex = 32;
            this.BtnParaAnalysis.Text = "          Para \r\n          Analysis";
            this.BtnParaAnalysis.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnParaAnalysis.UseVisualStyleBackColor = false;
            this.BtnParaAnalysis.Click += new System.EventHandler(this.btnParaAnalysis_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.LightSlateGray;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(284, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "   Test Item";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlChart
            // 
            this.pnlChart.Controls.Add(this.BinTrendChart);
            this.pnlChart.Controls.Add(this.chkGoodBin);
            this.pnlChart.Controls.Add(this.label5);
            this.pnlChart.Location = new System.Drawing.Point(0, 20);
            this.pnlChart.Name = "pnlChart";
            this.pnlChart.Size = new System.Drawing.Size(311, 574);
            this.pnlChart.TabIndex = 15;
            // 
            // BinTrendChart
            // 
            this.BinTrendChart.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            this.BinTrendChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.BinTrendChart.Legends.Add(legend1);
            this.BinTrendChart.Location = new System.Drawing.Point(0, 20);
            this.BinTrendChart.Name = "BinTrendChart";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedBar100;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.BinTrendChart.Series.Add(series1);
            this.BinTrendChart.Size = new System.Drawing.Size(311, 554);
            this.BinTrendChart.TabIndex = 12;
            this.BinTrendChart.Text = "Bin Chart";
            // 
            // chkGoodBin
            // 
            this.chkGoodBin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkGoodBin.AutoSize = true;
            this.chkGoodBin.BackColor = System.Drawing.Color.LightSlateGray;
            this.chkGoodBin.Checked = true;
            this.chkGoodBin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGoodBin.ForeColor = System.Drawing.Color.White;
            this.chkGoodBin.Location = new System.Drawing.Point(339, 1);
            this.chkGoodBin.Name = "chkGoodBin";
            this.chkGoodBin.Size = new System.Drawing.Size(83, 16);
            this.chkGoodBin.TabIndex = 14;
            this.chkGoodBin.Text = "GOOD BIN";
            this.chkGoodBin.UseVisualStyleBackColor = false;
            this.chkGoodBin.Visible = false;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.LightSlateGray;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Image = ((System.Drawing.Image)(resources.GetObject("label5.Image")));
            this.label5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(311, 20);
            this.label5.TabIndex = 18;
            this.label5.Text = "    Option";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // fpRawData
            // 
            this.fpRawData.AccessibleDescription = "";
            this.fpRawData.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpRawData.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpRawData.Location = new System.Drawing.Point(0, 20);
            this.fpRawData.Name = "fpRawData";
            this.fpRawData.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.AsNeeded;
            this.fpRawData.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpRawData_Sheet1});
            this.fpRawData.Size = new System.Drawing.Size(333, 574);
            this.fpRawData.TabIndex = 14;
            this.fpRawData.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // fpRawData_Sheet1
            // 
            this.fpRawData_Sheet1.Reset();
            fpRawData_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpRawData_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpRawData_Sheet1.ColumnCount = 0;
            fpRawData_Sheet1.RowCount = 0;
            this.fpRawData_Sheet1.ActiveColumnIndex = -1;
            this.fpRawData_Sheet1.ActiveRowIndex = -1;
            this.fpRawData_Sheet1.AllowNoteEdit = false;
            this.fpRawData_Sheet1.AutoCalculation = false;
            this.fpRawData_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpRawData_Sheet1.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpRawData_Sheet1.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpRawData_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpRawData_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterEnhanced";
            this.fpRawData_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpRawData_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpRawData_Sheet1.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpRawData_Sheet1.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpRawData_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpRawData_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerEnhanced";
            this.fpRawData_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpRawData_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpRawData_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpRawData_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpRawData_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpRawData_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderEnhanced";
            this.fpRawData_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpRawData_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpRawData_Sheet1.DataAutoCellTypes = false;
            this.fpRawData_Sheet1.DefaultStyle.BackColor = System.Drawing.Color.White;
            this.fpRawData_Sheet1.DefaultStyle.ForeColor = System.Drawing.Color.Black;
            this.fpRawData_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpRawData_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.fpRawData_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ExtendedSelect;
            this.fpRawData_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpRawData_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpRawData_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpRawData_Sheet1.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            this.fpRawData_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpRawData_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderEnhanced";
            this.fpRawData_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpRawData_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpRawData_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.MultiRange;
            this.fpRawData_Sheet1.SelectionStyle = FarPoint.Win.Spread.SelectionStyles.SelectionColors;
            this.fpRawData_Sheet1.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            this.fpRawData_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(105)))), ((int)(((byte)(107)))));
            this.fpRawData_Sheet1.SheetCornerStyle.ForeColor = System.Drawing.Color.White;
            this.fpRawData_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpRawData_Sheet1.SheetCornerStyle.Parent = "CornerEnhanced";
            this.fpRawData_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpRawData_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // AVIImageList
            // 
            this.AVIImageList.AutoScroll = true;
            this.AVIImageList.AutoSize = true;
            this.AVIImageList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.AVIImageList.Location = new System.Drawing.Point(0, 20);
            this.AVIImageList.Name = "AVIImageList";
            this.AVIImageList.Size = new System.Drawing.Size(311, 574);
            this.AVIImageList.TabIndex = 11;
            this.AVIImageList.WrapContents = false;
            // 
            // mapContainer
            // 
            this.mapContainer.AutoScroll = true;
            this.mapContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapContainer.Location = new System.Drawing.Point(0, 36);
            this.mapContainer.Name = "mapContainer";
            this.mapContainer.Size = new System.Drawing.Size(841, 580);
            this.mapContainer.TabIndex = 11;
            this.mapContainer.Resize += new System.EventHandler(this.mapContainer_Resize);
            // 
            // _TPUCMapViewAutoHideControl
            // 
            this._TPUCMapViewAutoHideControl.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._TPUCMapViewAutoHideControl.Location = new System.Drawing.Point(0, 0);
            this._TPUCMapViewAutoHideControl.Name = "_TPUCMapViewAutoHideControl";
            this._TPUCMapViewAutoHideControl.Owner = this.ultraDockManager1;
            this._TPUCMapViewAutoHideControl.Size = new System.Drawing.Size(0, 616);
            this._TPUCMapViewAutoHideControl.TabIndex = 10;
            // 
            // ultraDockManager1
            // 
            this.ultraDockManager1.AnimationSpeed = Infragistics.Win.UltraWinDock.AnimationSpeed.StandardSpeedPlus3;
            dockAreaPane1.ChildPaneStyle = Infragistics.Win.UltraWinDock.ChildPaneStyle.TabGroup;
            dockAreaPane1.DockedBefore = new System.Guid("8ad30bcb-99d9-4934-92ac-5603f0fe3a02");
            dockAreaPane1.FloatingLocation = new System.Drawing.Point(390, 195);
            dockAreaPane1.Size = new System.Drawing.Size(221, 242);
            dockAreaPane2.ChildPaneStyle = Infragistics.Win.UltraWinDock.ChildPaneStyle.TabGroup;
            dockAreaPane2.DockedBefore = new System.Guid("ba5b14b4-13f7-4422-827b-49997e7cd755");
            dockAreaPane2.FloatingLocation = new System.Drawing.Point(425, 312);
            dockAreaPane2.Size = new System.Drawing.Size(221, 242);
            dockAreaPane3.ChildPaneStyle = Infragistics.Win.UltraWinDock.ChildPaneStyle.TabGroup;
            dockAreaPane3.DockedBefore = new System.Guid("4e91b0b0-768d-41ee-9ee5-e07866f4c8f2");
            dockAreaPane3.FloatingLocation = new System.Drawing.Point(379, 148);
            dockableControlPane1.Closed = true;
            dockableControlPane1.Control = this.pnlWaferMapInfo;
            dockableControlPane1.OriginalControlBounds = new System.Drawing.Rectangle(320, 50, 222, 329);
            dockableControlPane1.Size = new System.Drawing.Size(100, 129);
            dockableControlPane1.Text = " Wafer Map Info";
            dockableControlPane2.Control = this.pnlParaItem;
            dockableControlPane2.OriginalControlBounds = new System.Drawing.Rectangle(289, 36, 222, 329);
            dockableControlPane2.Size = new System.Drawing.Size(100, 100);
            dockableControlPane2.Text = " Para Item";
            dockableControlPane3.Closed = true;
            dockableControlPane3.Control = this.pnlChart;
            dockableControlPane3.OriginalControlBounds = new System.Drawing.Rectangle(589, 105, 200, 100);
            dockableControlPane3.Size = new System.Drawing.Size(100, 100);
            dockableControlPane3.Text = "Bin Chart";
            dockableControlPane4.Control = this.fpRawData;
            dockableControlPane4.OriginalControlBounds = new System.Drawing.Rectangle(216, 71, 222, 329);
            dockableControlPane4.Size = new System.Drawing.Size(100, 100);
            dockableControlPane4.Text = " Raw Data";
            dockableControlPane5.Closed = true;
            dockableControlPane5.Control = this.AVIImageList;
            dockableControlPane5.OriginalControlBounds = new System.Drawing.Rectangle(460, 43, 327, 558);
            dockableControlPane5.Size = new System.Drawing.Size(100, 153);
            dockableControlPane5.Text = "AVIImageList";
            dockAreaPane3.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] {
            dockableControlPane1,
            dockableControlPane2,
            dockableControlPane3,
            dockableControlPane4,
            dockableControlPane5});
            dockAreaPane3.SelectedTabIndex = 1;
            dockAreaPane3.Size = new System.Drawing.Size(288, 616);
            dockAreaPane4.ChildPaneStyle = Infragistics.Win.UltraWinDock.ChildPaneStyle.TabGroup;
            dockAreaPane4.FloatingLocation = new System.Drawing.Point(453, 119);
            dockAreaPane4.Size = new System.Drawing.Size(221, 237);
            this.ultraDockManager1.DockAreas.AddRange(new Infragistics.Win.UltraWinDock.DockAreaPane[] {
            dockAreaPane1,
            dockAreaPane2,
            dockAreaPane3,
            dockAreaPane4});
            this.ultraDockManager1.HostControl = this;
            this.ultraDockManager1.WindowStyle = Infragistics.Win.UltraWinDock.WindowStyle.VisualStudio2005;
            // 
            // _TPUCMapViewUnpinnedTabAreaRight
            // 
            this._TPUCMapViewUnpinnedTabAreaRight.Dock = System.Windows.Forms.DockStyle.Right;
            this._TPUCMapViewUnpinnedTabAreaRight.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._TPUCMapViewUnpinnedTabAreaRight.Location = new System.Drawing.Point(1134, 0);
            this._TPUCMapViewUnpinnedTabAreaRight.Name = "_TPUCMapViewUnpinnedTabAreaRight";
            this._TPUCMapViewUnpinnedTabAreaRight.Owner = this.ultraDockManager1;
            this._TPUCMapViewUnpinnedTabAreaRight.Size = new System.Drawing.Size(0, 616);
            this._TPUCMapViewUnpinnedTabAreaRight.TabIndex = 7;
            // 
            // _TPUCMapViewUnpinnedTabAreaBottom
            // 
            this._TPUCMapViewUnpinnedTabAreaBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._TPUCMapViewUnpinnedTabAreaBottom.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._TPUCMapViewUnpinnedTabAreaBottom.Location = new System.Drawing.Point(0, 616);
            this._TPUCMapViewUnpinnedTabAreaBottom.Name = "_TPUCMapViewUnpinnedTabAreaBottom";
            this._TPUCMapViewUnpinnedTabAreaBottom.Owner = this.ultraDockManager1;
            this._TPUCMapViewUnpinnedTabAreaBottom.Size = new System.Drawing.Size(1134, 0);
            this._TPUCMapViewUnpinnedTabAreaBottom.TabIndex = 9;
            // 
            // _TPUCMapViewUnpinnedTabAreaTop
            // 
            this._TPUCMapViewUnpinnedTabAreaTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._TPUCMapViewUnpinnedTabAreaTop.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._TPUCMapViewUnpinnedTabAreaTop.Location = new System.Drawing.Point(0, 0);
            this._TPUCMapViewUnpinnedTabAreaTop.Name = "_TPUCMapViewUnpinnedTabAreaTop";
            this._TPUCMapViewUnpinnedTabAreaTop.Owner = this.ultraDockManager1;
            this._TPUCMapViewUnpinnedTabAreaTop.Size = new System.Drawing.Size(1134, 0);
            this._TPUCMapViewUnpinnedTabAreaTop.TabIndex = 8;
            // 
            // _TPUCMapViewUnpinnedTabAreaLeft
            // 
            this._TPUCMapViewUnpinnedTabAreaLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this._TPUCMapViewUnpinnedTabAreaLeft.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._TPUCMapViewUnpinnedTabAreaLeft.Location = new System.Drawing.Point(0, 0);
            this._TPUCMapViewUnpinnedTabAreaLeft.Name = "_TPUCMapViewUnpinnedTabAreaLeft";
            this._TPUCMapViewUnpinnedTabAreaLeft.Owner = this.ultraDockManager1;
            this._TPUCMapViewUnpinnedTabAreaLeft.Size = new System.Drawing.Size(0, 616);
            this._TPUCMapViewUnpinnedTabAreaLeft.TabIndex = 6;
            // 
            // windowDockingArea5
            // 
            this.windowDockingArea5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.windowDockingArea5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.windowDockingArea5.Location = new System.Drawing.Point(8, 8);
            this.windowDockingArea5.Name = "windowDockingArea5";
            this.windowDockingArea5.Owner = this.ultraDockManager1;
            this.windowDockingArea5.Size = new System.Drawing.Size(221, 242);
            this.windowDockingArea5.TabIndex = 0;
            // 
            // windowDockingArea1
            // 
            this.windowDockingArea1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.windowDockingArea1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.windowDockingArea1.Location = new System.Drawing.Point(8, 8);
            this.windowDockingArea1.Name = "windowDockingArea1";
            this.windowDockingArea1.Owner = this.ultraDockManager1;
            this.windowDockingArea1.Size = new System.Drawing.Size(221, 242);
            this.windowDockingArea1.TabIndex = 0;
            // 
            // windowDockingArea6
            // 
            this.windowDockingArea6.Controls.Add(this.dockableWindow2);
            this.windowDockingArea6.Controls.Add(this.dockableWindow3);
            this.windowDockingArea6.Controls.Add(this.dockableWindow5);
            this.windowDockingArea6.Controls.Add(this.dockableWindow1);
            this.windowDockingArea6.Controls.Add(this.dockableWindow4);
            this.windowDockingArea6.Dock = System.Windows.Forms.DockStyle.Right;
            this.windowDockingArea6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.windowDockingArea6.Location = new System.Drawing.Point(841, 0);
            this.windowDockingArea6.Name = "windowDockingArea6";
            this.windowDockingArea6.Owner = this.ultraDockManager1;
            this.windowDockingArea6.Size = new System.Drawing.Size(293, 616);
            this.windowDockingArea6.TabIndex = 0;
            // 
            // dockableWindow2
            // 
            this.dockableWindow2.Controls.Add(this.pnlWaferMapInfo);
            this.dockableWindow2.Location = new System.Drawing.Point(5, 0);
            this.dockableWindow2.Name = "dockableWindow2";
            this.dockableWindow2.Owner = this.ultraDockManager1;
            this.dockableWindow2.Size = new System.Drawing.Size(311, 596);
            this.dockableWindow2.TabIndex = 13;
            // 
            // dockableWindow3
            // 
            this.dockableWindow3.Controls.Add(this.pnlParaItem);
            this.dockableWindow3.Location = new System.Drawing.Point(5, 0);
            this.dockableWindow3.Name = "dockableWindow3";
            this.dockableWindow3.Owner = this.ultraDockManager1;
            this.dockableWindow3.Size = new System.Drawing.Size(288, 596);
            this.dockableWindow3.TabIndex = 14;
            // 
            // dockableWindow5
            // 
            this.dockableWindow5.Controls.Add(this.pnlChart);
            this.dockableWindow5.Location = new System.Drawing.Point(-10000, 0);
            this.dockableWindow5.Name = "dockableWindow5";
            this.dockableWindow5.Owner = this.ultraDockManager1;
            this.dockableWindow5.Size = new System.Drawing.Size(311, 596);
            this.dockableWindow5.TabIndex = 15;
            // 
            // dockableWindow1
            // 
            this.dockableWindow1.Controls.Add(this.fpRawData);
            this.dockableWindow1.Location = new System.Drawing.Point(-10000, 0);
            this.dockableWindow1.Name = "dockableWindow1";
            this.dockableWindow1.Owner = this.ultraDockManager1;
            this.dockableWindow1.Size = new System.Drawing.Size(333, 596);
            this.dockableWindow1.TabIndex = 16;
            // 
            // dockableWindow4
            // 
            this.dockableWindow4.Controls.Add(this.AVIImageList);
            this.dockableWindow4.Location = new System.Drawing.Point(-10000, 0);
            this.dockableWindow4.Name = "dockableWindow4";
            this.dockableWindow4.Owner = this.ultraDockManager1;
            this.dockableWindow4.Size = new System.Drawing.Size(311, 596);
            this.dockableWindow4.TabIndex = 17;
            // 
            // windowDockingArea9
            // 
            this.windowDockingArea9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.windowDockingArea9.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.windowDockingArea9.Location = new System.Drawing.Point(674, 0);
            this.windowDockingArea9.Name = "windowDockingArea9";
            this.windowDockingArea9.Owner = this.ultraDockManager1;
            this.windowDockingArea9.Size = new System.Drawing.Size(221, 237);
            this.windowDockingArea9.TabIndex = 13;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.numericUpDown2);
            this.panel8.Controls.Add(this.label2);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(841, 36);
            this.panel8.TabIndex = 12;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(95, 9);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(120, 21);
            this.numericUpDown2.TabIndex = 1;
            this.numericUpDown2.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Column Count";
            // 
            // TPUCMapGallery
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this._TPUCMapViewAutoHideControl);
            this.Controls.Add(this.mapContainer);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.windowDockingArea6);
            this.Controls.Add(this._TPUCMapViewUnpinnedTabAreaTop);
            this.Controls.Add(this._TPUCMapViewUnpinnedTabAreaBottom);
            this.Controls.Add(this._TPUCMapViewUnpinnedTabAreaLeft);
            this.Controls.Add(this._TPUCMapViewUnpinnedTabAreaRight);
            this.Name = "TPUCMapGallery";
            this.Size = new System.Drawing.Size(1134, 616);
            this.Load += new System.EventHandler(this.TPUCMapGallery_Load);
            this.pnlWaferMapInfo.ResumeLayout(false);
            this.pnlParaItem.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.panel5.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.FILTER.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.SPEC.ResumeLayout(false);
            this.SPEC.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.pnlChart.ResumeLayout(false);
            this.pnlChart.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BinTrendChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpRawData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpRawData_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager1)).EndInit();
            this.windowDockingArea6.ResumeLayout(false);
            this.dockableWindow2.ResumeLayout(false);
            this.dockableWindow3.ResumeLayout(false);
            this.dockableWindow5.ResumeLayout(false);
            this.dockableWindow1.ResumeLayout(false);
            this.dockableWindow4.ResumeLayout(false);
            this.dockableWindow4.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkGoodBin;
        private System.Windows.Forms.Panel pnlWaferMapInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlParaItem;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataVisualization.Charting.Chart BinTrendChart;
        private FarPoint.Win.Spread.FpSpread fpRawData;
        private FarPoint.Win.Spread.SheetView fpRawData_Sheet1;
        private System.Windows.Forms.Panel pnlChart;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox lsParaList;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnTestDataDraw;
        private System.Windows.Forms.Button BtnParaAnalysis;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox chkTotalPara;
        private System.Windows.Forms.TextBox txtItemFilter;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.FlowLayoutPanel AVIImageList;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.FlowLayoutPanel mapContainer;
        private Infragistics.Win.UltraWinDock.AutoHideControl _TPUCMapViewAutoHideControl;
        private Infragistics.Win.UltraWinDock.UltraDockManager ultraDockManager1;
        private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea6;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow2;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow3;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow5;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow1;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow4;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _TPUCMapViewUnpinnedTabAreaTop;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _TPUCMapViewUnpinnedTabAreaBottom;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _TPUCMapViewUnpinnedTabAreaLeft;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _TPUCMapViewUnpinnedTabAreaRight;
        private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea5;
        private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea1;
        private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea9;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage FILTER;
        private System.Windows.Forms.CheckedListBox clbParaCut;
        private System.Windows.Forms.ComboBox cbCutCnt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage SPEC;
        private System.Windows.Forms.TextBox txtLCL;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtUCL;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkControlLimit;
        private System.Windows.Forms.TextBox txtLSL;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtUSL;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkSpecificationLimit;
        private System.Windows.Forms.TextBox txtLTL;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtUTL;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox chkTightenLimit;
    }
}
