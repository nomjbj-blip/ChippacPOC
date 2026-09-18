namespace DACrux.ProjectManager.UI
{
    partial class StatView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatView));
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.pnlSplit = new System.Windows.Forms.SplitContainer();
            this.lvlist = new System.Windows.Forms.ListView();
            this.cmsStat = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiExportStat = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiRenameStat = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiRemoveStat = new System.Windows.Forms.ToolStripMenuItem();
            this.imlStatType = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pnlSplit)).BeginInit();
            this.pnlSplit.Panel1.SuspendLayout();
            this.pnlSplit.Panel2.SuspendLayout();
            this.pnlSplit.SuspendLayout();
            this.cmsStat.SuspendLayout();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(0, 0);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 23);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(513, 576);
            this.webBrowser.TabIndex = 0;
            // 
            // pnlSplit
            // 
            this.pnlSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSplit.Location = new System.Drawing.Point(0, 0);
            this.pnlSplit.Name = "pnlSplit";
            // 
            // pnlSplit.Panel1
            // 
            this.pnlSplit.Panel1.Controls.Add(this.lvlist);
            // 
            // pnlSplit.Panel2
            // 
            this.pnlSplit.Panel2.Controls.Add(this.webBrowser);
            this.pnlSplit.Size = new System.Drawing.Size(627, 576);
            this.pnlSplit.SplitterDistance = 110;
            this.pnlSplit.TabIndex = 1;
            // 
            // lvlist
            // 
            this.lvlist.ContextMenuStrip = this.cmsStat;
            this.lvlist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvlist.LabelEdit = true;
            this.lvlist.Location = new System.Drawing.Point(0, 0);
            this.lvlist.Name = "lvlist";
            this.lvlist.Size = new System.Drawing.Size(110, 576);
            this.lvlist.TabIndex = 1;
            this.lvlist.UseCompatibleStateImageBehavior = false;
            this.lvlist.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lvlist_AfterLabelEdit);
            this.lvlist.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvlist_MouseDoubleClick);
            // 
            // cmsStat
            // 
            this.cmsStat.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiExportStat,
            this.cmiRenameStat,
            this.toolStripSeparator10,
            this.cmiRemoveStat});
            this.cmsStat.Name = "cmsProject";
            this.cmsStat.Size = new System.Drawing.Size(191, 76);
            this.cmsStat.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsStat_ItemClicked);
            // 
            // cmiExportStat
            // 
            this.cmiExportStat.Name = "cmiExportStat";
            this.cmiExportStat.Size = new System.Drawing.Size(190, 22);
            this.cmiExportStat.Text = "&Export Analysis";
            // 
            // cmiRenameStat
            // 
            this.cmiRenameStat.Name = "cmiRenameStat";
            this.cmiRenameStat.Size = new System.Drawing.Size(190, 22);
            this.cmiRenameStat.Text = "Re&name Analysis As...";
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(187, 6);
            // 
            // cmiRemoveStat
            // 
            this.cmiRemoveStat.Name = "cmiRemoveStat";
            this.cmiRemoveStat.Size = new System.Drawing.Size(190, 22);
            this.cmiRemoveStat.Text = "&Remove Analysis";
            // 
            // imlStatType
            // 
            this.imlStatType.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlStatType.ImageStream")));
            this.imlStatType.TransparentColor = System.Drawing.Color.White;
            this.imlStatType.Images.SetKeyName(0, "ANOVA");
            this.imlStatType.Images.SetKeyName(1, "CorrelationAnalysis");
            this.imlStatType.Images.SetKeyName(2, "HypothesisTesting");
            this.imlStatType.Images.SetKeyName(3, "CpCpkAnalysis_Continuous");
            this.imlStatType.Images.SetKeyName(4, "RegressionAnalysis");
            this.imlStatType.Images.SetKeyName(5, "DescriptiveAnalysis");
            this.imlStatType.Images.SetKeyName(6, "DOE_TaguchiAnalysis");
            // 
            // StatView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlSplit);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "StatView";
            this.Size = new System.Drawing.Size(627, 576);
            this.pnlSplit.Panel1.ResumeLayout(false);
            this.pnlSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlSplit)).EndInit();
            this.pnlSplit.ResumeLayout(false);
            this.cmsStat.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.SplitContainer pnlSplit;
        private System.Windows.Forms.ListView lvlist;
        private System.Windows.Forms.ContextMenuStrip cmsStat;
        private System.Windows.Forms.ToolStripMenuItem cmiRenameStat;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem cmiRemoveStat;
        private System.Windows.Forms.ImageList imlStatType;
        private System.Windows.Forms.ToolStripMenuItem cmiExportStat;
    }
}
