namespace DACrux.ProjectManager
{
    partial class ProjectManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectManager));
            this.tstMenu = new System.Windows.Forms.ToolStrip();
            this.tsbNewProject = new System.Windows.Forms.ToolStripButton();
            this.tsbOpenProject = new System.Windows.Forms.ToolStripButton();
            this.tsbNewWorkSheet = new System.Windows.Forms.ToolStripButton();
            this.tsbExpand = new System.Windows.Forms.ToolStripButton();
            this.tsbCollapse = new System.Windows.Forms.ToolStripButton();
            this.tvExplorer = new System.Windows.Forms.TreeView();
            this.imlExplorer = new System.Windows.Forms.ImageList(this.components);
            this.cmsProject = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiExpandProject = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiCollapseProject = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiAddWorkSheet = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiRenameProject = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiSaveProject = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiSaveProjectAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiRemoveProject = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiCloseProject = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsWorkSheet = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiExpandWorkSheet = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiCollapseWorkSheet = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiRenameWorkSheet = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiRemoveWorkSheet = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiCloseWorkSheet = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsAnalysis = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiRenameAnalysis = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiSaveAnalysis = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiSaveAnalysisAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiRemoveAnalysis = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiCloseAnalysis = new System.Windows.Forms.ToolStripMenuItem();
            this.tstMenu.SuspendLayout();
            this.cmsProject.SuspendLayout();
            this.cmsWorkSheet.SuspendLayout();
            this.cmsAnalysis.SuspendLayout();
            this.SuspendLayout();
            // 
            // tstMenu
            // 
            this.tstMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNewProject,
            this.tsbOpenProject,
            this.tsbNewWorkSheet,
            this.tsbExpand,
            this.tsbCollapse});
            this.tstMenu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.tstMenu.Location = new System.Drawing.Point(0, 0);
            this.tstMenu.Name = "tstMenu";
            this.tstMenu.Size = new System.Drawing.Size(189, 23);
            this.tstMenu.TabIndex = 2;
            this.tstMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tstMenu_ItemClicked);
            // 
            // tsbNewProject
            // 
            this.tsbNewProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNewProject.Image = ((System.Drawing.Image)(resources.GetObject("tsbNewProject.Image")));
            this.tsbNewProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNewProject.Name = "tsbNewProject";
            this.tsbNewProject.Size = new System.Drawing.Size(23, 20);
            this.tsbNewProject.Text = "New Project";
            // 
            // tsbOpenProject
            // 
            this.tsbOpenProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOpenProject.Image = ((System.Drawing.Image)(resources.GetObject("tsbOpenProject.Image")));
            this.tsbOpenProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOpenProject.Name = "tsbOpenProject";
            this.tsbOpenProject.Size = new System.Drawing.Size(23, 20);
            this.tsbOpenProject.Text = "Open Project";
            // 
            // tsbNewWorkSheet
            // 
            this.tsbNewWorkSheet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNewWorkSheet.Image = ((System.Drawing.Image)(resources.GetObject("tsbNewWorkSheet.Image")));
            this.tsbNewWorkSheet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNewWorkSheet.Name = "tsbNewWorkSheet";
            this.tsbNewWorkSheet.Size = new System.Drawing.Size(23, 20);
            this.tsbNewWorkSheet.Text = "New Worksheet";
            // 
            // tsbExpand
            // 
            this.tsbExpand.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbExpand.Image = ((System.Drawing.Image)(resources.GetObject("tsbExpand.Image")));
            this.tsbExpand.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExpand.Name = "tsbExpand";
            this.tsbExpand.Size = new System.Drawing.Size(23, 20);
            this.tsbExpand.Text = "Expand";
            // 
            // tsbCollapse
            // 
            this.tsbCollapse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCollapse.Image = ((System.Drawing.Image)(resources.GetObject("tsbCollapse.Image")));
            this.tsbCollapse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCollapse.Name = "tsbCollapse";
            this.tsbCollapse.Size = new System.Drawing.Size(23, 20);
            this.tsbCollapse.Text = "Collapse";
            // 
            // tvExplorer
            // 
            this.tvExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvExplorer.ImageIndex = 0;
            this.tvExplorer.ImageList = this.imlExplorer;
            this.tvExplorer.LabelEdit = true;
            this.tvExplorer.Location = new System.Drawing.Point(0, 23);
            this.tvExplorer.Name = "tvExplorer";
            this.tvExplorer.SelectedImageIndex = 0;
            this.tvExplorer.Size = new System.Drawing.Size(189, 346);
            this.tvExplorer.TabIndex = 3;
            this.tvExplorer.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvExplorer_AfterLabelEdit);
            this.tvExplorer.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvExplorer_NodeMouseClick);
            this.tvExplorer.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvExplorer_NodeMouseDoubleClick);
            this.tvExplorer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvExplorer_KeyDown);
            // 
            // imlExplorer
            // 
            this.imlExplorer.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlExplorer.ImageStream")));
            this.imlExplorer.TransparentColor = System.Drawing.Color.Transparent;
            this.imlExplorer.Images.SetKeyName(0, "002nprjopen.ico");
            this.imlExplorer.Images.SetKeyName(1, "002nprjopen.ico");
            this.imlExplorer.Images.SetKeyName(2, "301colname.ico");
            this.imlExplorer.Images.SetKeyName(3, "301colname.ico");
            this.imlExplorer.Images.SetKeyName(4, "501grpbar.ico");
            this.imlExplorer.Images.SetKeyName(5, "501grpbar.ico");
            // 
            // cmsProject
            // 
            this.cmsProject.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiExpandProject,
            this.cmiCollapseProject,
            this.toolStripSeparator11,
            this.cmiAddWorkSheet,
            this.toolStripSeparator1,
            this.cmiRenameProject,
            this.toolStripSeparator8,
            this.cmiSaveProject,
            this.cmiSaveProjectAs,
            this.toolStripSeparator2,
            this.cmiRemoveProject,
            this.toolStripSeparator3,
            this.cmiCloseProject});
            this.cmsProject.Name = "cmsProject";
            this.cmsProject.Size = new System.Drawing.Size(198, 232);
            this.cmsProject.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsProject_ItemClicked);
            // 
            // cmiExpandProject
            // 
            this.cmiExpandProject.Name = "cmiExpandProject";
            this.cmiExpandProject.Size = new System.Drawing.Size(197, 22);
            this.cmiExpandProject.Text = "E&xpand All";
            // 
            // cmiCollapseProject
            // 
            this.cmiCollapseProject.Name = "cmiCollapseProject";
            this.cmiCollapseProject.Size = new System.Drawing.Size(197, 22);
            this.cmiCollapseProject.Text = "Co&llapse All";
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(194, 6);
            // 
            // cmiAddWorkSheet
            // 
            this.cmiAddWorkSheet.Name = "cmiAddWorkSheet";
            this.cmiAddWorkSheet.Size = new System.Drawing.Size(197, 22);
            this.cmiAddWorkSheet.Text = "Add &WorkSheet";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(194, 6);
            // 
            // cmiRenameProject
            // 
            this.cmiRenameProject.Name = "cmiRenameProject";
            this.cmiRenameProject.Size = new System.Drawing.Size(197, 22);
            this.cmiRenameProject.Text = "Re&name Project As...";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(194, 6);
            // 
            // cmiSaveProject
            // 
            this.cmiSaveProject.Name = "cmiSaveProject";
            this.cmiSaveProject.Size = new System.Drawing.Size(197, 22);
            this.cmiSaveProject.Text = "&Save Project";
            // 
            // cmiSaveProjectAs
            // 
            this.cmiSaveProjectAs.Name = "cmiSaveProjectAs";
            this.cmiSaveProjectAs.Size = new System.Drawing.Size(197, 22);
            this.cmiSaveProjectAs.Text = "Save Project &As...";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(194, 6);
            this.toolStripSeparator2.Visible = false;
            // 
            // cmiRemoveProject
            // 
            this.cmiRemoveProject.Name = "cmiRemoveProject";
            this.cmiRemoveProject.Size = new System.Drawing.Size(197, 22);
            this.cmiRemoveProject.Text = "&Remove Project";
            this.cmiRemoveProject.Visible = false;
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(194, 6);
            this.toolStripSeparator3.Visible = false;
            // 
            // cmiCloseProject
            // 
            this.cmiCloseProject.Name = "cmiCloseProject";
            this.cmiCloseProject.Size = new System.Drawing.Size(197, 22);
            this.cmiCloseProject.Text = "&Close Project";
            // 
            // cmsWorkSheet
            // 
            this.cmsWorkSheet.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiExpandWorkSheet,
            this.cmiCollapseWorkSheet,
            this.toolStripSeparator12,
            this.cmiRenameWorkSheet,
            this.toolStripSeparator9,
            this.cmiRemoveWorkSheet,
            this.toolStripSeparator6,
            this.cmiCloseWorkSheet});
            this.cmsWorkSheet.Name = "cmsProject";
            this.cmsWorkSheet.Size = new System.Drawing.Size(218, 132);
            this.cmsWorkSheet.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsWorkSheet_ItemClicked);
            // 
            // cmiExpandWorkSheet
            // 
            this.cmiExpandWorkSheet.Name = "cmiExpandWorkSheet";
            this.cmiExpandWorkSheet.Size = new System.Drawing.Size(217, 22);
            this.cmiExpandWorkSheet.Text = "E&xpand WorkSheet";
            // 
            // cmiCollapseWorkSheet
            // 
            this.cmiCollapseWorkSheet.Name = "cmiCollapseWorkSheet";
            this.cmiCollapseWorkSheet.Size = new System.Drawing.Size(217, 22);
            this.cmiCollapseWorkSheet.Text = "Co&llapse WorkSheet";
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(214, 6);
            // 
            // cmiRenameWorkSheet
            // 
            this.cmiRenameWorkSheet.Name = "cmiRenameWorkSheet";
            this.cmiRenameWorkSheet.Size = new System.Drawing.Size(217, 22);
            this.cmiRenameWorkSheet.Text = "Re&name WorkSheet As...";
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(214, 6);
            // 
            // cmiRemoveWorkSheet
            // 
            this.cmiRemoveWorkSheet.Name = "cmiRemoveWorkSheet";
            this.cmiRemoveWorkSheet.Size = new System.Drawing.Size(217, 22);
            this.cmiRemoveWorkSheet.Text = "&Remove WorkSheet";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(214, 6);
            // 
            // cmiCloseWorkSheet
            // 
            this.cmiCloseWorkSheet.Name = "cmiCloseWorkSheet";
            this.cmiCloseWorkSheet.Size = new System.Drawing.Size(217, 22);
            this.cmiCloseWorkSheet.Text = "&Close WorkSheet";
            // 
            // cmsAnalysis
            // 
            this.cmsAnalysis.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiRenameAnalysis,
            this.toolStripSeparator10,
            this.cmiSaveAnalysis,
            this.cmiSaveAnalysisAs,
            this.toolStripSeparator4,
            this.cmiRemoveAnalysis,
            this.toolStripSeparator7,
            this.cmiCloseAnalysis});
            this.cmsAnalysis.Name = "cmsProject";
            this.cmsAnalysis.Size = new System.Drawing.Size(208, 132);
            this.cmsAnalysis.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsAnalysis_ItemClicked);
            // 
            // cmiRenameAnalysis
            // 
            this.cmiRenameAnalysis.Name = "cmiRenameAnalysis";
            this.cmiRenameAnalysis.Size = new System.Drawing.Size(207, 22);
            this.cmiRenameAnalysis.Text = "Re&name Analysis As...";
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(204, 6);
            // 
            // cmiSaveAnalysis
            // 
            this.cmiSaveAnalysis.Name = "cmiSaveAnalysis";
            this.cmiSaveAnalysis.Size = new System.Drawing.Size(207, 22);
            this.cmiSaveAnalysis.Text = "&Save Analysis";
            // 
            // cmiSaveAnalysisAs
            // 
            this.cmiSaveAnalysisAs.Name = "cmiSaveAnalysisAs";
            this.cmiSaveAnalysisAs.Size = new System.Drawing.Size(207, 22);
            this.cmiSaveAnalysisAs.Text = "Save Analysis &As...";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(204, 6);
            // 
            // cmiRemoveAnalysis
            // 
            this.cmiRemoveAnalysis.Name = "cmiRemoveAnalysis";
            this.cmiRemoveAnalysis.Size = new System.Drawing.Size(207, 22);
            this.cmiRemoveAnalysis.Text = "&Remove Analysis";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(204, 6);
            // 
            // cmiCloseAnalysis
            // 
            this.cmiCloseAnalysis.Name = "cmiCloseAnalysis";
            this.cmiCloseAnalysis.Size = new System.Drawing.Size(207, 22);
            this.cmiCloseAnalysis.Text = "&Close Analysis";
            // 
            // ProjectManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tvExplorer);
            this.Controls.Add(this.tstMenu);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ProjectManager";
            this.Size = new System.Drawing.Size(189, 369);
            this.Load += new System.EventHandler(this.ProjectManager_Load);
            this.tstMenu.ResumeLayout(false);
            this.tstMenu.PerformLayout();
            this.cmsProject.ResumeLayout(false);
            this.cmsWorkSheet.ResumeLayout(false);
            this.cmsAnalysis.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tstMenu;
        private System.Windows.Forms.ToolStripButton tsbNewProject;
        private System.Windows.Forms.ToolStripButton tsbOpenProject;
        private System.Windows.Forms.ToolStripButton tsbNewWorkSheet;
        private System.Windows.Forms.ToolStripButton tsbExpand;
        private System.Windows.Forms.ToolStripButton tsbCollapse;
        private System.Windows.Forms.TreeView tvExplorer;
        private System.Windows.Forms.ImageList imlExplorer;
        private System.Windows.Forms.ContextMenuStrip cmsProject;
        private System.Windows.Forms.ToolStripMenuItem cmiAddWorkSheet;
        private System.Windows.Forms.ToolStripMenuItem cmiSaveProject;
        private System.Windows.Forms.ToolStripMenuItem cmiSaveProjectAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem cmiRemoveProject;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem cmiCloseProject;
        private System.Windows.Forms.ContextMenuStrip cmsWorkSheet;
        private System.Windows.Forms.ToolStripMenuItem cmiRemoveWorkSheet;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem cmiCloseWorkSheet;
        private System.Windows.Forms.ContextMenuStrip cmsAnalysis;
        private System.Windows.Forms.ToolStripMenuItem cmiSaveAnalysis;
        private System.Windows.Forms.ToolStripMenuItem cmiSaveAnalysisAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem cmiRemoveAnalysis;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem cmiCloseAnalysis;
        private System.Windows.Forms.ToolStripMenuItem cmiRenameProject;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem cmiRenameWorkSheet;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem cmiRenameAnalysis;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem cmiExpandProject;
        private System.Windows.Forms.ToolStripMenuItem cmiCollapseProject;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem cmiExpandWorkSheet;
        private System.Windows.Forms.ToolStripMenuItem cmiCollapseWorkSheet;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;


    }
}
