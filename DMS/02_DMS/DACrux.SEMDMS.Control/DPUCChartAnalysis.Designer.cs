namespace DACrux.SEMDMS.Control
{
    partial class DPUCChartAnalysis : DACrux.Framework.Base.DACruxCTLBasic01
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DPUCChartAnalysis));
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            DACrux.SEMDMS.Control.OptionInformation optionInformation1 = new DACrux.SEMDMS.Control.OptionInformation();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraGroupBox3 = new Infragistics.Win.Misc.UltraGroupBox();
            this.rbDesc = new System.Windows.Forms.RadioButton();
            this.rbAsc = new System.Windows.Forms.RadioButton();
            this.ultraGroupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this.rbXChronological = new System.Windows.Forms.RadioButton();
            this.rbXLabel = new System.Windows.Forms.RadioButton();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.ucmYValueMember = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ucmXValueMember = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ucmType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.uchkNormalize = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ulblYAxis = new Infragistics.Win.Misc.UltraLabel();
            this.ulblXAxis = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel5 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.ultraPanel3 = new Infragistics.Win.Misc.UltraPanel();
            this.dpucOption1 = new DACrux.SEMDMS.Control.DPUCOption();
            this.ultraPanel4 = new Infragistics.Win.Misc.UltraPanel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmReset = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).BeginInit();
            this.ultraGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ucmYValueMember)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ucmXValueMember)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ucmType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uchkNormalize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.ultraPanel2.ClientArea.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            this.ultraPanel5.ClientArea.SuspendLayout();
            this.ultraPanel5.SuspendLayout();
            this.ultraPanel3.ClientArea.SuspendLayout();
            this.ultraPanel3.SuspendLayout();
            this.ultraPanel4.ClientArea.SuspendLayout();
            this.ultraPanel4.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.ultraGroupBox3);
            this.ultraGroupBox1.Controls.Add(this.ultraGroupBox2);
            this.ultraGroupBox1.Controls.Add(this.btnSearch);
            this.ultraGroupBox1.Controls.Add(this.ucmYValueMember);
            this.ultraGroupBox1.Controls.Add(this.ucmXValueMember);
            this.ultraGroupBox1.Controls.Add(this.ucmType);
            this.ultraGroupBox1.Controls.Add(this.uchkNormalize);
            this.ultraGroupBox1.Controls.Add(this.ulblYAxis);
            this.ultraGroupBox1.Controls.Add(this.ulblXAxis);
            this.ultraGroupBox1.Controls.Add(this.ultraLabel1);
            this.ultraGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(988, 106);
            this.ultraGroupBox1.TabIndex = 1;
            this.ultraGroupBox1.Text = "Option";
            this.ultraGroupBox1.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.VisualStudio2005;
            // 
            // ultraGroupBox3
            // 
            this.ultraGroupBox3.Controls.Add(this.rbDesc);
            this.ultraGroupBox3.Controls.Add(this.rbAsc);
            this.ultraGroupBox3.Location = new System.Drawing.Point(701, 25);
            this.ultraGroupBox3.Name = "ultraGroupBox3";
            this.ultraGroupBox3.Size = new System.Drawing.Size(176, 76);
            this.ultraGroupBox3.TabIndex = 12;
            // 
            // rbDesc
            // 
            this.rbDesc.AutoSize = true;
            this.rbDesc.Location = new System.Drawing.Point(6, 30);
            this.rbDesc.Name = "rbDesc";
            this.rbDesc.Size = new System.Drawing.Size(90, 16);
            this.rbDesc.TabIndex = 3;
            this.rbDesc.Text = "Descending";
            this.rbDesc.UseVisualStyleBackColor = true;
            // 
            // rbAsc
            // 
            this.rbAsc.AutoSize = true;
            this.rbAsc.Checked = true;
            this.rbAsc.Location = new System.Drawing.Point(6, 6);
            this.rbAsc.Name = "rbAsc";
            this.rbAsc.Size = new System.Drawing.Size(83, 16);
            this.rbAsc.TabIndex = 2;
            this.rbAsc.TabStop = true;
            this.rbAsc.Text = "Ascending";
            this.rbAsc.UseVisualStyleBackColor = true;
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Controls.Add(this.rbXChronological);
            this.ultraGroupBox2.Controls.Add(this.rbXLabel);
            this.ultraGroupBox2.Location = new System.Drawing.Point(519, 25);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(176, 76);
            this.ultraGroupBox2.TabIndex = 11;
            // 
            // rbXChronological
            // 
            this.rbXChronological.AutoSize = true;
            this.rbXChronological.Location = new System.Drawing.Point(6, 30);
            this.rbXChronological.Name = "rbXChronological";
            this.rbXChronological.Size = new System.Drawing.Size(113, 16);
            this.rbXChronological.TabIndex = 1;
            this.rbXChronological.Text = "X Chronological";
            this.rbXChronological.UseVisualStyleBackColor = true;
            // 
            // rbXLabel
            // 
            this.rbXLabel.AutoSize = true;
            this.rbXLabel.Checked = true;
            this.rbXLabel.Location = new System.Drawing.Point(6, 6);
            this.rbXLabel.Name = "rbXLabel";
            this.rbXLabel.Size = new System.Drawing.Size(66, 16);
            this.rbXLabel.TabIndex = 0;
            this.rbXLabel.TabStop = true;
            this.rbXLabel.Text = "X Label";
            this.rbXLabel.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.Image = "search.gif";
            this.btnSearch.Appearance = appearance1;
            this.btnSearch.ImageList = this.imageList1;
            this.btnSearch.Location = new System.Drawing.Point(892, 18);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(91, 28);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Text = "Redraw";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "search.gif");
            this.imageList1.Images.SetKeyName(1, "chart_bar.png");
            this.imageList1.Images.SetKeyName(2, "chart_curve.png");
            this.imageList1.Images.SetKeyName(3, "chart_line.png");
            this.imageList1.Images.SetKeyName(4, "chart_organisation.png");
            this.imageList1.Images.SetKeyName(5, "chart_pie.png");
            this.imageList1.Images.SetKeyName(6, "palette.png");
            // 
            // ucmYValueMember
            // 
            this.ucmYValueMember.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.ucmYValueMember.Location = new System.Drawing.Point(368, 49);
            this.ucmYValueMember.Name = "ucmYValueMember";
            this.ucmYValueMember.Size = new System.Drawing.Size(145, 21);
            this.ucmYValueMember.TabIndex = 9;
            this.ucmYValueMember.SelectionChangeCommitted += new System.EventHandler(this.UltraComboEditor_SelectionChangeCommitted);
            // 
            // ucmXValueMember
            // 
            this.ucmXValueMember.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.ucmXValueMember.Location = new System.Drawing.Point(368, 22);
            this.ucmXValueMember.Name = "ucmXValueMember";
            this.ucmXValueMember.Size = new System.Drawing.Size(145, 21);
            this.ucmXValueMember.TabIndex = 8;
            this.ucmXValueMember.SelectionChangeCommitted += new System.EventHandler(this.UltraComboEditor_SelectionChangeCommitted);
            // 
            // ucmType
            // 
            this.ucmType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.ucmType.Location = new System.Drawing.Point(111, 21);
            this.ucmType.Name = "ucmType";
            this.ucmType.Size = new System.Drawing.Size(145, 21);
            this.ucmType.TabIndex = 7;
            this.ucmType.SelectionChangeCommitted += new System.EventHandler(this.UltraComboEditor_SelectionChangeCommitted);
            // 
            // uchkNormalize
            // 
            appearance2.TextHAlignAsString = "Left";
            appearance2.TextVAlignAsString = "Middle";
            this.uchkNormalize.Appearance = appearance2;
            this.uchkNormalize.Enabled = false;
            this.uchkNormalize.Location = new System.Drawing.Point(262, 77);
            this.uchkNormalize.Name = "uchkNormalize";
            this.uchkNormalize.Size = new System.Drawing.Size(100, 20);
            this.uchkNormalize.TabIndex = 6;
            this.uchkNormalize.Text = "Normalize";
            this.uchkNormalize.CheckedChanged += new System.EventHandler(this.uchkNormalize_CheckedChanged);
            // 
            // ulblYAxis
            // 
            appearance3.TextVAlignAsString = "Middle";
            this.ulblYAxis.Appearance = appearance3;
            this.ulblYAxis.ImageList = this.imageList1;
            this.ulblYAxis.Location = new System.Drawing.Point(262, 49);
            this.ulblYAxis.Name = "ulblYAxis";
            this.ulblYAxis.Size = new System.Drawing.Size(100, 22);
            this.ulblYAxis.TabIndex = 4;
            this.ulblYAxis.Text = "Y-Axis";
            // 
            // ulblXAxis
            // 
            appearance4.TextVAlignAsString = "Middle";
            this.ulblXAxis.Appearance = appearance4;
            this.ulblXAxis.ImageList = this.imageList1;
            this.ulblXAxis.Location = new System.Drawing.Point(262, 21);
            this.ulblXAxis.Name = "ulblXAxis";
            this.ulblXAxis.Size = new System.Drawing.Size(100, 22);
            this.ulblXAxis.TabIndex = 2;
            this.ulblXAxis.Text = "X-Axis";
            // 
            // ultraLabel1
            // 
            appearance5.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance5;
            this.ultraLabel1.ImageList = this.imageList1;
            this.ultraLabel1.Location = new System.Drawing.Point(5, 21);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(100, 22);
            this.ultraLabel1.TabIndex = 0;
            this.ultraLabel1.Text = "Chart Type";
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.ContextMenuStrip = this.contextMenuStrip1;
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(788, 480);
            this.chart1.TabIndex = 2;
            this.chart1.Text = "chart1";
            this.chart1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseClick);
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraPanel2);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(988, 586);
            this.ultraPanel1.TabIndex = 3;
            // 
            // ultraPanel2
            // 
            // 
            // ultraPanel2.ClientArea
            // 
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraPanel5);
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraPanel4);
            this.ultraPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel2.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(988, 586);
            this.ultraPanel2.TabIndex = 4;
            // 
            // ultraPanel5
            // 
            // 
            // ultraPanel5.ClientArea
            // 
            this.ultraPanel5.ClientArea.Controls.Add(this.ultraSplitter1);
            this.ultraPanel5.ClientArea.Controls.Add(this.chart1);
            this.ultraPanel5.ClientArea.Controls.Add(this.ultraPanel3);
            this.ultraPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel5.Location = new System.Drawing.Point(0, 106);
            this.ultraPanel5.Name = "ultraPanel5";
            this.ultraPanel5.Size = new System.Drawing.Size(988, 480);
            this.ultraPanel5.TabIndex = 6;
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2007RibbonButton;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.ultraSplitter1.Location = new System.Drawing.Point(778, 0);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 200;
            this.ultraSplitter1.Size = new System.Drawing.Size(10, 480);
            this.ultraSplitter1.TabIndex = 5;
            // 
            // ultraPanel3
            // 
            // 
            // ultraPanel3.ClientArea
            // 
            this.ultraPanel3.ClientArea.Controls.Add(this.dpucOption1);
            this.ultraPanel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.ultraPanel3.Location = new System.Drawing.Point(788, 0);
            this.ultraPanel3.Name = "ultraPanel3";
            this.ultraPanel3.Size = new System.Drawing.Size(200, 480);
            this.ultraPanel3.TabIndex = 4;
            // 
            // dpucOption1
            // 
            this.dpucOption1.DataMember_DefectClass = null;
            this.dpucOption1.DataSource_DefectClass = null;
            this.dpucOption1.DisplayMember_DefectClass = null;
            this.dpucOption1.ValueMember_DefectClass = null;
            this.dpucOption1.Dock = System.Windows.Forms.DockStyle.Fill;
            optionInformation1.Average_Defective_Die_Percentages = null;
            optionInformation1.Defects = null;
            optionInformation1.Insepcted_Wafer = null;
            optionInformation1.Inspected_Area = null;
            optionInformation1.Inspected_Wafer_Average_Defects = null;
            optionInformation1.Inspection_DateTime_From = null;
            optionInformation1.Inspection_DateTime_To = null;
            optionInformation1.Inspections = null;
            optionInformation1.Inspectors = null;
            optionInformation1.Lot_Average_Defects = null;
            optionInformation1.Lots = null;
            optionInformation1.Scan_Tools = null;
            optionInformation1.Steps = null;
            optionInformation1.Total_Classified_Defects = null;
            optionInformation1.Total_Defect_Density = null;
            optionInformation1.Total_Defective_Die_Percentage = null;
            optionInformation1.Total_Defective_Dies = null;
            optionInformation1.Total_Inspected_Dies = null;
            optionInformation1.Wafer_Average_Defects = null;
            optionInformation1.Wafers = null;
            this.dpucOption1.Location = new System.Drawing.Point(0, 0);
            this.dpucOption1.Name = "dpucOption1";
            this.dpucOption1.Size = new System.Drawing.Size(200, 480);
            this.dpucOption1.TabIndex = 0;
            // 
            // ultraPanel4
            // 
            // 
            // ultraPanel4.ClientArea
            // 
            this.ultraPanel4.ClientArea.Controls.Add(this.ultraGroupBox1);
            this.ultraPanel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel4.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel4.Name = "ultraPanel4";
            this.ultraPanel4.Size = new System.Drawing.Size(988, 106);
            this.ultraPanel4.TabIndex = 5;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmReset});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 48);
            // 
            // tsmReset
            // 
            this.tsmReset.Name = "tsmReset";
            this.tsmReset.Size = new System.Drawing.Size(152, 22);
            this.tsmReset.Text = "Reset";
            this.tsmReset.Click += new System.EventHandler(this.tsmReset_Click);
            // 
            // DPUCChartAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraPanel1);
            this.Name = "DPUCChartAnalysis";
            this.Size = new System.Drawing.Size(988, 586);
            this.Load += new System.EventHandler(this.DPUCChartAnalysis_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ultraGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).EndInit();
            this.ultraGroupBox3.ResumeLayout(false);
            this.ultraGroupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            this.ultraGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ucmYValueMember)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ucmXValueMember)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ucmType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uchkNormalize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.ultraPanel2.ClientArea.ResumeLayout(false);
            this.ultraPanel2.ResumeLayout(false);
            this.ultraPanel5.ClientArea.ResumeLayout(false);
            this.ultraPanel5.ResumeLayout(false);
            this.ultraPanel3.ClientArea.ResumeLayout(false);
            this.ultraPanel3.ResumeLayout(false);
            this.ultraPanel4.ClientArea.ResumeLayout(false);
            this.ultraPanel4.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private Infragistics.Win.Misc.UltraLabel ulblYAxis;
        private Infragistics.Win.Misc.UltraLabel ulblXAxis;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor uchkNormalize;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor ucmType;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor ucmYValueMember;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor ucmXValueMember;
        private Infragistics.Win.Misc.UltraButton btnSearch;
        private System.Windows.Forms.ImageList imageList1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.Misc.UltraPanel ultraPanel3;
        private Infragistics.Win.Misc.UltraPanel ultraPanel5;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel4;
        private DPUCOption dpucOption1;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox3;
        private System.Windows.Forms.RadioButton rbDesc;
        private System.Windows.Forms.RadioButton rbAsc;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox2;
        private System.Windows.Forms.RadioButton rbXChronological;
        private System.Windows.Forms.RadioButton rbXLabel;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmReset;
    }
}
