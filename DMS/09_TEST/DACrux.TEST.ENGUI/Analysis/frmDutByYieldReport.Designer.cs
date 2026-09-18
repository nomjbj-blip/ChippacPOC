namespace DACrux.TEST.ENGUI
{
    partial class frmDutByYieldReport
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDutByYieldReport));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.chartTrend = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartTrend2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.fpSpread1 = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.btnToExcel = new System.Windows.Forms.Button();
            this.btnAnalysis = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlTitle = new System.Windows.Forms.Panel();
            this.btnRedraw = new System.Windows.Forms.Button();
            this.numToDut = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numFromDut = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.pbCollapse1 = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartTrend)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartTrend2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).BeginInit();
            this.pnlTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numToDut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFromDut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCollapse1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 30);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.fpSpread1);
            this.splitContainer1.Panel2.Controls.Add(this.btnToExcel);
            this.splitContainer1.Panel2.Controls.Add(this.btnAnalysis);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(714, 432);
            this.splitContainer1.SplitterDistance = 299;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.chartTrend);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.chartTrend2);
            this.splitContainer2.Size = new System.Drawing.Size(714, 299);
            this.splitContainer2.SplitterDistance = 181;
            this.splitContainer2.TabIndex = 0;
            // 
            // chartTrend
            // 
            this.chartTrend.BackColor = System.Drawing.Color.LightYellow;
            chartArea1.BackColor = System.Drawing.Color.LightYellow;
            chartArea1.Name = "Default";
            this.chartTrend.ChartAreas.Add(chartArea1);
            this.chartTrend.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.BackColor = System.Drawing.Color.LightYellow;
            legend1.BorderColor = System.Drawing.Color.DarkKhaki;
            legend1.BorderWidth = 2;
            legend1.Name = "Legend1";
            this.chartTrend.Legends.Add(legend1);
            this.chartTrend.Location = new System.Drawing.Point(0, 0);
            this.chartTrend.Name = "chartTrend";
            this.chartTrend.Size = new System.Drawing.Size(181, 299);
            this.chartTrend.TabIndex = 19;
            this.chartTrend.Text = "Dut By Yield";
            title1.Name = "Title1";
            title1.Text = "Dut By Yield";
            this.chartTrend.Titles.Add(title1);
            this.chartTrend.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.chartTrend_MouseDoubleClick);
            // 
            // chartTrend2
            // 
            this.chartTrend2.BackColor = System.Drawing.Color.LightYellow;
            chartArea2.BackColor = System.Drawing.Color.LightYellow;
            chartArea2.Name = "Default";
            this.chartTrend2.ChartAreas.Add(chartArea2);
            this.chartTrend2.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.BackColor = System.Drawing.Color.LightYellow;
            legend2.BorderColor = System.Drawing.Color.DarkKhaki;
            legend2.BorderWidth = 2;
            legend2.Name = "Legend1";
            this.chartTrend2.Legends.Add(legend2);
            this.chartTrend2.Location = new System.Drawing.Point(0, 0);
            this.chartTrend2.Name = "chartTrend2";
            this.chartTrend2.Size = new System.Drawing.Size(529, 299);
            this.chartTrend2.TabIndex = 19;
            this.chartTrend2.Text = "Dut By Bin Portion";
            title2.Name = "Title1";
            title2.Text = "Dut By Bin Portion";
            this.chartTrend2.Titles.Add(title2);
            this.chartTrend2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.chartTrend2_MouseDoubleClick);
            // 
            // fpSpread1
            // 
            this.fpSpread1.AccessibleDescription = "";
            this.fpSpread1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpread1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpSpread1.Location = new System.Drawing.Point(0, 30);
            this.fpSpread1.Name = "fpSpread1";
            this.fpSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread1_Sheet1});
            this.fpSpread1.Size = new System.Drawing.Size(714, 99);
            this.fpSpread1.TabIndex = 71;
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
            // btnToExcel
            // 
            this.btnToExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToExcel.Image = ((System.Drawing.Image)(resources.GetObject("btnToExcel.Image")));
            this.btnToExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnToExcel.Location = new System.Drawing.Point(537, 3);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(81, 23);
            this.btnToExcel.TabIndex = 70;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.btnToExcel_Click);
            // 
            // btnAnalysis
            // 
            this.btnAnalysis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAnalysis.Image = ((System.Drawing.Image)(resources.GetObject("btnAnalysis.Image")));
            this.btnAnalysis.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAnalysis.Location = new System.Drawing.Point(624, 3);
            this.btnAnalysis.Name = "btnAnalysis";
            this.btnAnalysis.Size = new System.Drawing.Size(81, 23);
            this.btnAnalysis.TabIndex = 69;
            this.btnAnalysis.Text = "Analysis";
            this.btnAnalysis.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAnalysis.UseVisualStyleBackColor = true;
            this.btnAnalysis.Visible = false;
            this.btnAnalysis.Click += new System.EventHandler(this.btnAnalysis_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightSlateGray;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(714, 30);
            this.label1.TabIndex = 68;
            this.label1.Text = "     Shot Test Time Raw Data";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlTitle
            // 
            this.pnlTitle.BackColor = System.Drawing.SystemColors.Menu;
            this.pnlTitle.Controls.Add(this.btnRedraw);
            this.pnlTitle.Controls.Add(this.numToDut);
            this.pnlTitle.Controls.Add(this.label3);
            this.pnlTitle.Controls.Add(this.numFromDut);
            this.pnlTitle.Controls.Add(this.label2);
            this.pnlTitle.Controls.Add(this.pbCollapse1);
            this.pnlTitle.Controls.Add(this.label6);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(714, 30);
            this.pnlTitle.TabIndex = 2;
            // 
            // btnRedraw
            // 
            this.btnRedraw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRedraw.Image = ((System.Drawing.Image)(resources.GetObject("btnRedraw.Image")));
            this.btnRedraw.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRedraw.Location = new System.Drawing.Point(601, 3);
            this.btnRedraw.Name = "btnRedraw";
            this.btnRedraw.Size = new System.Drawing.Size(81, 23);
            this.btnRedraw.TabIndex = 70;
            this.btnRedraw.Text = "Redraw";
            this.btnRedraw.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRedraw.UseVisualStyleBackColor = true;
            this.btnRedraw.Click += new System.EventHandler(this.btnRedraw_Click);
            // 
            // numToDut
            // 
            this.numToDut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numToDut.Location = new System.Drawing.Point(535, 4);
            this.numToDut.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numToDut.Name = "numToDut";
            this.numToDut.Size = new System.Drawing.Size(57, 21);
            this.numToDut.TabIndex = 63;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.LightSlateGray;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(487, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 15);
            this.label3.TabIndex = 62;
            this.label3.Text = "To Dut";
            // 
            // numFromDut
            // 
            this.numFromDut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numFromDut.Location = new System.Drawing.Point(424, 4);
            this.numFromDut.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numFromDut.Name = "numFromDut";
            this.numFromDut.Size = new System.Drawing.Size(57, 21);
            this.numFromDut.TabIndex = 61;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.LightSlateGray;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(360, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 15);
            this.label2.TabIndex = 20;
            this.label2.Text = "From Dut";
            // 
            // pbCollapse1
            // 
            this.pbCollapse1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbCollapse1.BackColor = System.Drawing.Color.Transparent;
            this.pbCollapse1.Location = new System.Drawing.Point(688, 6);
            this.pbCollapse1.Name = "pbCollapse1";
            this.pbCollapse1.Size = new System.Drawing.Size(17, 17);
            this.pbCollapse1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbCollapse1.TabIndex = 60;
            this.pbCollapse1.TabStop = false;
            this.pbCollapse1.Click += new System.EventHandler(this.pbCollapse1_Click);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.LightSlateGray;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Image = ((System.Drawing.Image)(resources.GetObject("label6.Image")));
            this.label6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(714, 30);
            this.label6.TabIndex = 59;
            this.label6.Text = "     Dut By Yield && Bin Portion";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmDutByYieldReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 462);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.pnlTitle);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmDutByYieldReport";
            this.Text = "Dut By Yield Report";
            this.Load += new System.EventHandler(this.frmDutByYieldReport_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartTrend)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartTrend2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).EndInit();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numToDut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFromDut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCollapse1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel pnlTitle;
        private System.Windows.Forms.PictureBox pbCollapse1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartTrend;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartTrend2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnToExcel;
        private System.Windows.Forms.Button btnAnalysis;
        private FarPoint.Win.Spread.FpSpread fpSpread1;
        private FarPoint.Win.Spread.SheetView fpSpread1_Sheet1;
        private System.Windows.Forms.NumericUpDown numToDut;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numFromDut;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRedraw;
    }
}