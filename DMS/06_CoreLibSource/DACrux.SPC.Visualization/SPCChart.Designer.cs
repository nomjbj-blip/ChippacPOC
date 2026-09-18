namespace DACrux.SPC.Visualization
{
    partial class SPCChart
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPCChart));
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.FV_MAIN_MENU = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.zoomBackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fitSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.toggleInternalFlagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.copyToImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.FV_MAIN_MENU.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.ContextMenuStrip = this.FV_MAIN_MENU;
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series1.Legend = "Legend1";
            series1.Name = "XBar";
            series1.YValuesPerPoint = 2;
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(714, 455);
            this.chart1.SuppressExceptions = true;
            this.chart1.TabIndex = 1;
            this.chart1.Text = "chart1";
            this.chart1.PostPaint += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ChartPaintEventArgs>(this.chart1_PostPaint);
            this.chart1.Click += new System.EventHandler(this.chart1_Click);
            this.chart1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chart1_KeyDown);
            // 
            // FV_MAIN_MENU
            // 
            this.FV_MAIN_MENU.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomBackToolStripMenuItem,
            this.fitSizeToolStripMenuItem,
            this.selectDataToolStripMenuItem,
            this.toolStripMenuItem2,
            this.toggleInternalFlagToolStripMenuItem,
            this.propertiesToolStripMenuItem,
            this.toolStripMenuItem1,
            this.copyToImageToolStripMenuItem});
            this.FV_MAIN_MENU.Name = "FV_MAIN_MENU";
            this.FV_MAIN_MENU.Size = new System.Drawing.Size(181, 148);
            // 
            // zoomBackToolStripMenuItem
            // 
            this.zoomBackToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("zoomBackToolStripMenuItem.Image")));
            this.zoomBackToolStripMenuItem.Name = "zoomBackToolStripMenuItem";
            this.zoomBackToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.zoomBackToolStripMenuItem.Text = "Zoom Back";
            this.zoomBackToolStripMenuItem.Click += new System.EventHandler(this.zoomBackToolStripMenuItem_Click);
            // 
            // fitSizeToolStripMenuItem
            // 
            this.fitSizeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("fitSizeToolStripMenuItem.Image")));
            this.fitSizeToolStripMenuItem.Name = "fitSizeToolStripMenuItem";
            this.fitSizeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.fitSizeToolStripMenuItem.Text = "Fit Size";
            this.fitSizeToolStripMenuItem.Click += new System.EventHandler(this.fitSizeToolStripMenuItem_Click);
            // 
            // selectDataToolStripMenuItem
            // 
            this.selectDataToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("selectDataToolStripMenuItem.Image")));
            this.selectDataToolStripMenuItem.Name = "selectDataToolStripMenuItem";
            this.selectDataToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.selectDataToolStripMenuItem.Text = "Select Data";
            this.selectDataToolStripMenuItem.Click += new System.EventHandler(this.selectDataToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(177, 6);
            // 
            // toggleInternalFlagToolStripMenuItem
            // 
            this.toggleInternalFlagToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("toggleInternalFlagToolStripMenuItem.Image")));
            this.toggleInternalFlagToolStripMenuItem.Name = "toggleInternalFlagToolStripMenuItem";
            this.toggleInternalFlagToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.toggleInternalFlagToolStripMenuItem.Text = "Toggle Internal Flag";
            this.toggleInternalFlagToolStripMenuItem.Click += new System.EventHandler(this.toggleInternalFlagToolStripMenuItem_Click);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.propertiesToolStripMenuItem.Text = "Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(177, 6);
            // 
            // copyToImageToolStripMenuItem
            // 
            this.copyToImageToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToImageToolStripMenuItem.Image")));
            this.copyToImageToolStripMenuItem.Name = "copyToImageToolStripMenuItem";
            this.copyToImageToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.copyToImageToolStripMenuItem.Text = "Copy Image";
            this.copyToImageToolStripMenuItem.Click += new System.EventHandler(this.copyToImageToolStripMenuItem_Click);
            // 
            // SPCChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chart1);
            this.Name = "SPCChart";
            this.Size = new System.Drawing.Size(714, 455);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.FV_MAIN_MENU.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ContextMenuStrip FV_MAIN_MENU;
        private System.Windows.Forms.ToolStripMenuItem zoomBackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fitSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toggleInternalFlagToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem copyToImageToolStripMenuItem;

    }
}
