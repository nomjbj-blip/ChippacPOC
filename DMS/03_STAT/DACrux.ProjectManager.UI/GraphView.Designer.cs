namespace DACrux.ProjectManager.UI
{
    partial class GraphView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphView));
            this.pnlSplit = new System.Windows.Forms.SplitContainer();
            this.lvlist = new System.Windows.Forms.ListView();
            this.cmsGraph = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiRenameGraph = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiRemoveGraph = new System.Windows.Forms.ToolStripMenuItem();
            this.graphPanel = new DACrux.ProjectManager.UI.GraphPanel();
            this.imlGraphType = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pnlSplit)).BeginInit();
            this.pnlSplit.Panel1.SuspendLayout();
            this.pnlSplit.Panel2.SuspendLayout();
            this.pnlSplit.SuspendLayout();
            this.cmsGraph.SuspendLayout();
            this.SuspendLayout();
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
            this.pnlSplit.Panel2.Controls.Add(this.graphPanel);
            this.pnlSplit.Size = new System.Drawing.Size(800, 600);
            this.pnlSplit.SplitterDistance = 134;
            this.pnlSplit.SplitterWidth = 3;
            this.pnlSplit.TabIndex = 1;
            // 
            // lvlist
            // 
            this.lvlist.ContextMenuStrip = this.cmsGraph;
            this.lvlist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvlist.LabelEdit = true;
            this.lvlist.Location = new System.Drawing.Point(0, 0);
            this.lvlist.Name = "lvlist";
            this.lvlist.Size = new System.Drawing.Size(134, 600);
            this.lvlist.TabIndex = 0;
            this.lvlist.UseCompatibleStateImageBehavior = false;
            this.lvlist.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lvlist_AfterLabelEdit);
            this.lvlist.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvlist_MouseDoubleClick);
            // 
            // cmsGraph
            // 
            this.cmsGraph.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiRenameGraph,
            this.toolStripSeparator10,
            this.cmiRemoveGraph});
            this.cmsGraph.Name = "cmsProject";
            this.cmsGraph.Size = new System.Drawing.Size(191, 54);
            this.cmsGraph.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsGraph_ItemClicked);
            // 
            // cmiRenameGraph
            // 
            this.cmiRenameGraph.Name = "cmiRenameGraph";
            this.cmiRenameGraph.Size = new System.Drawing.Size(190, 22);
            this.cmiRenameGraph.Text = "Re&name Analysis As...";
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(187, 6);
            // 
            // cmiRemoveGraph
            // 
            this.cmiRemoveGraph.Name = "cmiRemoveGraph";
            this.cmiRemoveGraph.Size = new System.Drawing.Size(190, 22);
            this.cmiRemoveGraph.Text = "&Remove Analysis";
            // 
            // graphPanel
            // 
            this.graphPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.graphPanel.Location = new System.Drawing.Point(0, 0);
            this.graphPanel.Name = "graphPanel";
            this.graphPanel.showpropertybutton = true;
            this.graphPanel.ShowToolBar = true;
            this.graphPanel.Size = new System.Drawing.Size(663, 600);
            this.graphPanel.TabIndex = 0;
            // 
            // imlGraphType
            // 
            this.imlGraphType.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlGraphType.ImageStream")));
            this.imlGraphType.TransparentColor = System.Drawing.Color.White;
            this.imlGraphType.Images.SetKeyName(0, "Scatter");
            this.imlGraphType.Images.SetKeyName(1, "Bar");
            this.imlGraphType.Images.SetKeyName(2, "BoxPlot");
            this.imlGraphType.Images.SetKeyName(3, "Histogram");
            this.imlGraphType.Images.SetKeyName(4, "Line");
            this.imlGraphType.Images.SetKeyName(5, "Pareto");
            this.imlGraphType.Images.SetKeyName(6, "Pie");
            this.imlGraphType.Images.SetKeyName(7, "Line4Taguchi");
            // 
            // GraphView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlSplit);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "GraphView";
            this.Size = new System.Drawing.Size(800, 600);
            this.Load += new System.EventHandler(this.GraphView_Load);
            this.pnlSplit.Panel1.ResumeLayout(false);
            this.pnlSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlSplit)).EndInit();
            this.pnlSplit.ResumeLayout(false);
            this.cmsGraph.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer pnlSplit;
        private System.Windows.Forms.ListView lvlist;
        private GraphPanel graphPanel;
        private System.Windows.Forms.ImageList imlGraphType;
        private System.Windows.Forms.ContextMenuStrip cmsGraph;
        private System.Windows.Forms.ToolStripMenuItem cmiRenameGraph;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem cmiRemoveGraph;
    }
}
