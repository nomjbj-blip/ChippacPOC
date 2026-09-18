

namespace DACrux.Framework
{
    partial class frmUserMonitor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserMonitor));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea16 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend16 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series16 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea17 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend17 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series17 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea18 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend18 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series18 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea19 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend19 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series19 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea20 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend20 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series20 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.lblTild = new System.Windows.Forms.Label();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.btnSearch = new System.Windows.Forms.PictureBox();
            this.btnClose = new System.Windows.Forms.PictureBox();
            this.splTop = new System.Windows.Forms.Splitter();
            this.tabHitLog = new System.Windows.Forms.TabControl();
            this.tpbHitRateByFunction = new System.Windows.Forms.TabPage();
            this.chart4 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tbpUserCountByFunction = new System.Windows.Forms.TabPage();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tbpDailyTrend = new System.Windows.Forms.TabPage();
            this.chart3 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tbpMonthlyTrend = new System.Windows.Forms.TabPage();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tbpHitRateByDepartment = new System.Windows.Forms.TabPage();
            this.chart5 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tbpHitLogDetail = new System.Windows.Forms.TabPage();
            this.fpSpread1 = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.TexTopLevel = new System.Windows.Forms.TextBox();
            this.topLevelLbl = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            this.tabHitLog.SuspendLayout();
            this.tpbHitRateByFunction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart4)).BeginInit();
            this.tbpUserCountByFunction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            this.tbpDailyTrend.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart3)).BeginInit();
            this.tbpMonthlyTrend.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.tbpHitRateByDepartment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart5)).BeginInit();
            this.tbpHitLogDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTild
            // 
            this.lblTild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTild.AutoSize = true;
            this.lblTild.BackColor = System.Drawing.Color.Transparent;
            this.lblTild.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTild.ForeColor = System.Drawing.Color.White;
            this.lblTild.Location = new System.Drawing.Point(549, 7);
            this.lblTild.Name = "lblTild";
            this.lblTild.Size = new System.Drawing.Size(17, 23);
            this.lblTild.TabIndex = 8;
            this.lblTild.Text = "-";
            // 
            // dtpEnd
            // 
            this.dtpEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEnd.Location = new System.Drawing.Point(567, 7);
            this.dtpEnd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(89, 22);
            this.dtpEnd.TabIndex = 71;
            // 
            // dtpStart
            // 
            this.dtpStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStart.Location = new System.Drawing.Point(455, 7);
            this.dtpStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(89, 22);
            this.dtpStart.TabIndex = 70;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.Location = new System.Drawing.Point(667, 7);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(70, 22);
            this.btnSearch.TabIndex = 69;
            this.btnSearch.TabStop = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(744, 7);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 22);
            this.btnClose.TabIndex = 66;
            this.btnClose.TabStop = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // splTop
            // 
            this.splTop.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.splTop.Location = new System.Drawing.Point(0, 34);
            this.splTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splTop.Name = "splTop";
            this.splTop.Size = new System.Drawing.Size(823, 5);
            this.splTop.TabIndex = 7;
            this.splTop.TabStop = false;
            // 
            // tabHitLog
            // 
            this.tabHitLog.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabHitLog.Controls.Add(this.tpbHitRateByFunction);
            this.tabHitLog.Controls.Add(this.tbpUserCountByFunction);
            this.tabHitLog.Controls.Add(this.tbpDailyTrend);
            this.tabHitLog.Controls.Add(this.tbpMonthlyTrend);
            this.tabHitLog.Controls.Add(this.tbpHitRateByDepartment);
            this.tabHitLog.Controls.Add(this.tbpHitLogDetail);
            this.tabHitLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabHitLog.Location = new System.Drawing.Point(0, 39);
            this.tabHitLog.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabHitLog.Name = "tabHitLog";
            this.tabHitLog.SelectedIndex = 0;
            this.tabHitLog.Size = new System.Drawing.Size(823, 429);
            this.tabHitLog.TabIndex = 8;
            this.tabHitLog.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabHitLog_Selected);
            // 
            // tpbHitRateByFunction
            // 
            this.tpbHitRateByFunction.Controls.Add(this.chart4);
            this.tpbHitRateByFunction.Location = new System.Drawing.Point(4, 26);
            this.tpbHitRateByFunction.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tpbHitRateByFunction.Name = "tpbHitRateByFunction";
            this.tpbHitRateByFunction.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tpbHitRateByFunction.Size = new System.Drawing.Size(815, 399);
            this.tpbHitRateByFunction.TabIndex = 0;
            this.tpbHitRateByFunction.Text = "Hit Rate By Function";
            this.tpbHitRateByFunction.UseVisualStyleBackColor = true;
            // 
            // chart4
            // 
            chartArea16.Name = "ChartArea1";
            this.chart4.ChartAreas.Add(chartArea16);
            this.chart4.Dock = System.Windows.Forms.DockStyle.Fill;
            legend16.Name = "Legend1";
            this.chart4.Legends.Add(legend16);
            this.chart4.Location = new System.Drawing.Point(3, 4);
            this.chart4.Name = "chart4";
            series16.ChartArea = "ChartArea1";
            series16.Legend = "Legend1";
            series16.Name = "Series1";
            this.chart4.Series.Add(series16);
            this.chart4.Size = new System.Drawing.Size(809, 391);
            this.chart4.TabIndex = 0;
            this.chart4.Text = "chart4";
            // 
            // tbpUserCountByFunction
            // 
            this.tbpUserCountByFunction.Controls.Add(this.chart2);
            this.tbpUserCountByFunction.Location = new System.Drawing.Point(4, 26);
            this.tbpUserCountByFunction.Name = "tbpUserCountByFunction";
            this.tbpUserCountByFunction.Size = new System.Drawing.Size(815, 399);
            this.tbpUserCountByFunction.TabIndex = 1;
            this.tbpUserCountByFunction.Text = "User Count By Function";
            this.tbpUserCountByFunction.UseVisualStyleBackColor = true;
            // 
            // chart2
            // 
            chartArea17.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea17);
            this.chart2.Dock = System.Windows.Forms.DockStyle.Fill;
            legend17.Name = "Legend1";
            this.chart2.Legends.Add(legend17);
            this.chart2.Location = new System.Drawing.Point(0, 0);
            this.chart2.Name = "chart2";
            series17.ChartArea = "ChartArea1";
            series17.Legend = "Legend1";
            series17.Name = "Series1";
            this.chart2.Series.Add(series17);
            this.chart2.Size = new System.Drawing.Size(815, 399);
            this.chart2.TabIndex = 1;
            this.chart2.Text = "chart2";
            // 
            // tbpDailyTrend
            // 
            this.tbpDailyTrend.Controls.Add(this.chart3);
            this.tbpDailyTrend.Location = new System.Drawing.Point(4, 26);
            this.tbpDailyTrend.Name = "tbpDailyTrend";
            this.tbpDailyTrend.Size = new System.Drawing.Size(815, 399);
            this.tbpDailyTrend.TabIndex = 2;
            this.tbpDailyTrend.Text = "Daily Trend";
            this.tbpDailyTrend.UseVisualStyleBackColor = true;
            // 
            // chart3
            // 
            chartArea18.Name = "ChartArea1";
            this.chart3.ChartAreas.Add(chartArea18);
            this.chart3.Dock = System.Windows.Forms.DockStyle.Fill;
            legend18.Name = "Legend1";
            this.chart3.Legends.Add(legend18);
            this.chart3.Location = new System.Drawing.Point(0, 0);
            this.chart3.Name = "chart3";
            series18.ChartArea = "ChartArea1";
            series18.Legend = "Legend1";
            series18.Name = "Series1";
            this.chart3.Series.Add(series18);
            this.chart3.Size = new System.Drawing.Size(815, 399);
            this.chart3.TabIndex = 1;
            this.chart3.Text = "chart3";
            // 
            // tbpMonthlyTrend
            // 
            this.tbpMonthlyTrend.Controls.Add(this.chart1);
            this.tbpMonthlyTrend.Location = new System.Drawing.Point(4, 26);
            this.tbpMonthlyTrend.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbpMonthlyTrend.Name = "tbpMonthlyTrend";
            this.tbpMonthlyTrend.Size = new System.Drawing.Size(815, 399);
            this.tbpMonthlyTrend.TabIndex = 3;
            this.tbpMonthlyTrend.Text = "Monthly Hit Log Trend";
            this.tbpMonthlyTrend.UseVisualStyleBackColor = true;
            // 
            // chart1
            // 
            chartArea19.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea19);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend19.Name = "Legend1";
            this.chart1.Legends.Add(legend19);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            series19.ChartArea = "ChartArea1";
            series19.Legend = "Legend1";
            series19.Name = "Series1";
            this.chart1.Series.Add(series19);
            this.chart1.Size = new System.Drawing.Size(815, 399);
            this.chart1.TabIndex = 1;
            this.chart1.Text = "chart1";
            // 
            // tbpHitRateByDepartment
            // 
            this.tbpHitRateByDepartment.Controls.Add(this.chart5);
            this.tbpHitRateByDepartment.Location = new System.Drawing.Point(4, 26);
            this.tbpHitRateByDepartment.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbpHitRateByDepartment.Name = "tbpHitRateByDepartment";
            this.tbpHitRateByDepartment.Size = new System.Drawing.Size(815, 399);
            this.tbpHitRateByDepartment.TabIndex = 4;
            this.tbpHitRateByDepartment.Text = "Hit Rate By Department";
            this.tbpHitRateByDepartment.UseVisualStyleBackColor = true;
            // 
            // chart5
            // 
            chartArea20.Name = "ChartArea1";
            this.chart5.ChartAreas.Add(chartArea20);
            this.chart5.Dock = System.Windows.Forms.DockStyle.Fill;
            legend20.Name = "Legend1";
            this.chart5.Legends.Add(legend20);
            this.chart5.Location = new System.Drawing.Point(0, 0);
            this.chart5.Name = "chart5";
            series20.ChartArea = "ChartArea1";
            series20.Legend = "Legend1";
            series20.Name = "Series1";
            this.chart5.Series.Add(series20);
            this.chart5.Size = new System.Drawing.Size(815, 399);
            this.chart5.TabIndex = 0;
            this.chart5.Text = "chart5";
            // 
            // tbpHitLogDetail
            // 
            this.tbpHitLogDetail.Controls.Add(this.fpSpread1);
            this.tbpHitLogDetail.Location = new System.Drawing.Point(4, 26);
            this.tbpHitLogDetail.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbpHitLogDetail.Name = "tbpHitLogDetail";
            this.tbpHitLogDetail.Size = new System.Drawing.Size(815, 399);
            this.tbpHitLogDetail.TabIndex = 5;
            this.tbpHitLogDetail.Text = "Detail Hit Log";
            this.tbpHitLogDetail.UseVisualStyleBackColor = true;
            // 
            // fpSpread1
            // 
            this.fpSpread1.AccessibleDescription = "fpSpread1, Sheet1, Row 0, Column 0, ";
            this.fpSpread1.BackColor = System.Drawing.SystemColors.Control;
            this.fpSpread1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpread1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpSpread1.Location = new System.Drawing.Point(0, 0);
            this.fpSpread1.Name = "fpSpread1";
            this.fpSpread1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.fpSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread1_Sheet1});
            this.fpSpread1.Size = new System.Drawing.Size(815, 399);
            this.fpSpread1.TabIndex = 0;
            this.fpSpread1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpSpread1.SetActiveViewport(0, -1, -1);
            // 
            // fpSpread1_Sheet1
            // 
            this.fpSpread1_Sheet1.Reset();
            fpSpread1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fpSpread1_Sheet1.ColumnCount = 0;
            fpSpread1_Sheet1.RowCount = 0;
            this.fpSpread1_Sheet1.ActiveColumnIndex = -1;
            this.fpSpread1_Sheet1.ActiveRowIndex = -1;
            this.fpSpread1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(101)))), ((int)(((byte)(126)))));
            this.pnlTop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlTop.BackgroundImage")));
            this.pnlTop.Controls.Add(this.TexTopLevel);
            this.pnlTop.Controls.Add(this.topLevelLbl);
            this.pnlTop.Controls.Add(this.lblTild);
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Controls.Add(this.dtpEnd);
            this.pnlTop.Controls.Add(this.btnClose);
            this.pnlTop.Controls.Add(this.dtpStart);
            this.pnlTop.Controls.Add(this.btnSearch);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(823, 34);
            this.pnlTop.TabIndex = 120;
            // 
            // TexTopLevel
            // 
            this.TexTopLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TexTopLevel.Location = new System.Drawing.Point(340, 7);
            this.TexTopLevel.Name = "TexTopLevel";
            this.TexTopLevel.Size = new System.Drawing.Size(100, 22);
            this.TexTopLevel.TabIndex = 74;
            this.TexTopLevel.Text = "ALL";
            this.TexTopLevel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TexTopLevel_KeyPress);
            // 
            // topLevelLbl
            // 
            this.topLevelLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.topLevelLbl.AutoSize = true;
            this.topLevelLbl.BackColor = System.Drawing.Color.Transparent;
            this.topLevelLbl.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.topLevelLbl.ForeColor = System.Drawing.Color.White;
            this.topLevelLbl.Location = new System.Drawing.Point(269, 10);
            this.topLevelLbl.Name = "topLevelLbl";
            this.topLevelLbl.Size = new System.Drawing.Size(65, 16);
            this.topLevelLbl.TabIndex = 73;
            this.topLevelLbl.Text = "TOP Level";
            this.topLevelLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(259, 34);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "   ● User Hit Log Monitoring";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmUserMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 468);
            this.Controls.Add(this.tabHitLog);
            this.Controls.Add(this.splTop);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmUserMonitor";
            this.Text = "User Monitor";
            this.Load += new System.EventHandler(this.frmUserMonitor_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmUserMonitor_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            this.tabHitLog.ResumeLayout(false);
            this.tpbHitRateByFunction.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart4)).EndInit();
            this.tbpUserCountByFunction.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            this.tbpDailyTrend.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart3)).EndInit();
            this.tbpMonthlyTrend.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.tbpHitRateByDepartment.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart5)).EndInit();
            this.tbpHitLogDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox btnClose;
        private System.Windows.Forms.PictureBox btnSearch;
        private System.Windows.Forms.Splitter splTop;
        private System.Windows.Forms.Label lblTild;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.TabControl tabHitLog;
        private System.Windows.Forms.TabPage tpbHitRateByFunction;
        private System.Windows.Forms.TabPage tbpUserCountByFunction;
        private System.Windows.Forms.TabPage tbpDailyTrend;
        private System.Windows.Forms.TabPage tbpMonthlyTrend;
        private System.Windows.Forms.TabPage tbpHitRateByDepartment;
        private System.Windows.Forms.TabPage tbpHitLogDetail;
        //private ChartFX.WinForms.Chart chart1;
        //private ChartFX.WinForms.Chart chart2;
        // private ChartFX.WinForms.Chart chart3;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart4;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart3;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart5;
        private FarPoint.Win.Spread.FpSpread fpSpread1;
        private FarPoint.Win.Spread.SheetView fpSpread1_Sheet1;
        private System.Windows.Forms.Label topLevelLbl;
        private System.Windows.Forms.TextBox TexTopLevel;
        //private ChartFX.WinForms.Chart chart4;
        //private ChartFX.WinForms.Chart chart5;
    }
}