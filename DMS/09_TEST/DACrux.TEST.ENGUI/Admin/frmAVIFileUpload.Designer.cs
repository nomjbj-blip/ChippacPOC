namespace DACrux.TEST.ENGUI
{
    partial class frmAVIFileUpload
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAVIFileUpload));
            this.BtnFolderOpen = new System.Windows.Forms.Button();
            this.lsFiles = new System.Windows.Forms.ListView();
            this.CFileList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.TxtExtension = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkFileGet = new System.Windows.Forms.CheckBox();
            this.BtnSave = new System.Windows.Forms.Button();
            this.BtnClear = new System.Windows.Forms.Button();
            this.TxtFolderPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BtnFindFolder = new System.Windows.Forms.Button();
            this.FileImageList = new System.Windows.Forms.ImageList(this.components);
            this.CMenuStripFolder = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.getFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.trFolder = new System.Windows.Forms.TreeView();
            this.FolderImageList = new System.Windows.Forms.ImageList(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkAllLevelDir = new System.Windows.Forms.CheckBox();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.CFileList.SuspendLayout();
            this.panel1.SuspendLayout();
            this.CMenuStripFolder.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnFolderOpen
            // 
            this.BtnFolderOpen.BackColor = System.Drawing.Color.White;
            this.BtnFolderOpen.Image = ((System.Drawing.Image)(resources.GetObject("BtnFolderOpen.Image")));
            this.BtnFolderOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnFolderOpen.Location = new System.Drawing.Point(12, 12);
            this.BtnFolderOpen.Name = "BtnFolderOpen";
            this.BtnFolderOpen.Size = new System.Drawing.Size(134, 28);
            this.BtnFolderOpen.TabIndex = 110;
            this.BtnFolderOpen.Text = "  Search Files";
            this.BtnFolderOpen.UseVisualStyleBackColor = false;
            this.BtnFolderOpen.Visible = false;
            this.BtnFolderOpen.Click += new System.EventHandler(this.BtnFolderOpen_Click);
            // 
            // lsFiles
            // 
            this.lsFiles.ContextMenuStrip = this.CFileList;
            this.lsFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsFiles.Location = new System.Drawing.Point(316, 74);
            this.lsFiles.Name = "lsFiles";
            this.lsFiles.Size = new System.Drawing.Size(782, 437);
            this.lsFiles.TabIndex = 113;
            this.lsFiles.UseCompatibleStateImageBehavior = false;
            this.lsFiles.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lsFiles_ColumnClick);
            this.lsFiles.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lsFiles_KeyUp);
            // 
            // CFileList
            // 
            this.CFileList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearListToolStripMenuItem});
            this.CFileList.Name = "CMenuStripFolder";
            this.CFileList.Size = new System.Drawing.Size(124, 26);
            // 
            // clearListToolStripMenuItem
            // 
            this.clearListToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("clearListToolStripMenuItem.Image")));
            this.clearListToolStripMenuItem.Name = "clearListToolStripMenuItem";
            this.clearListToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.clearListToolStripMenuItem.Text = "Clear List";
            this.clearListToolStripMenuItem.Click += new System.EventHandler(this.clearListToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.TxtExtension);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.chkFileGet);
            this.panel1.Controls.Add(this.BtnSave);
            this.panel1.Controls.Add(this.BtnClear);
            this.panel1.Controls.Add(this.BtnFolderOpen);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1098, 74);
            this.panel1.TabIndex = 114;
            // 
            // TxtExtension
            // 
            this.TxtExtension.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtExtension.Location = new System.Drawing.Point(108, 46);
            this.TxtExtension.Name = "TxtExtension";
            this.TxtExtension.ReadOnly = true;
            this.TxtExtension.Size = new System.Drawing.Size(978, 21);
            this.TxtExtension.TabIndex = 116;
            this.TxtExtension.Text = resources.GetString("TxtExtension.Text");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 12);
            this.label1.TabIndex = 115;
            this.label1.Text = "Extensions";
            // 
            // chkFileGet
            // 
            this.chkFileGet.AutoSize = true;
            this.chkFileGet.Location = new System.Drawing.Point(249, 19);
            this.chkFileGet.Name = "chkFileGet";
            this.chkFileGet.Size = new System.Drawing.Size(119, 16);
            this.chkFileGet.TabIndex = 114;
            this.chkFileGet.Text = "File Select Mode";
            this.chkFileGet.UseVisualStyleBackColor = true;
            this.chkFileGet.Visible = false;
            // 
            // BtnSave
            // 
            this.BtnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSave.Image = ((System.Drawing.Image)(resources.GetObject("BtnSave.Image")));
            this.BtnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnSave.Location = new System.Drawing.Point(875, 8);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(211, 32);
            this.BtnSave.TabIndex = 113;
            this.BtnSave.Text = "File Upload";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnClear
            // 
            this.BtnClear.BackColor = System.Drawing.Color.White;
            this.BtnClear.Image = ((System.Drawing.Image)(resources.GetObject("BtnClear.Image")));
            this.BtnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnClear.Location = new System.Drawing.Point(152, 12);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(91, 28);
            this.BtnClear.TabIndex = 110;
            this.BtnClear.Text = "  Clear";
            this.BtnClear.UseVisualStyleBackColor = false;
            this.BtnClear.Visible = false;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // TxtFolderPath
            // 
            this.TxtFolderPath.Location = new System.Drawing.Point(6, 34);
            this.TxtFolderPath.Name = "TxtFolderPath";
            this.TxtFolderPath.Size = new System.Drawing.Size(224, 21);
            this.TxtFolderPath.TabIndex = 116;
            this.TxtFolderPath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtFolderPath_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 12);
            this.label2.TabIndex = 115;
            this.label2.Text = "File Search";
            // 
            // BtnFindFolder
            // 
            this.BtnFindFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnFindFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnFindFolder.Image = ((System.Drawing.Image)(resources.GetObject("BtnFindFolder.Image")));
            this.BtnFindFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnFindFolder.Location = new System.Drawing.Point(236, 34);
            this.BtnFindFolder.Name = "BtnFindFolder";
            this.BtnFindFolder.Size = new System.Drawing.Size(68, 21);
            this.BtnFindFolder.TabIndex = 113;
            this.BtnFindFolder.Text = "Find";
            this.BtnFindFolder.UseVisualStyleBackColor = true;
            this.BtnFindFolder.Click += new System.EventHandler(this.BtnFindFolder_Click);
            // 
            // FileImageList
            // 
            this.FileImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("FileImageList.ImageStream")));
            this.FileImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.FileImageList.Images.SetKeyName(0, "Map");
            this.FileImageList.Images.SetKeyName(1, "Image");
            // 
            // CMenuStripFolder
            // 
            this.CMenuStripFolder.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.getFilesToolStripMenuItem,
            this.refreshListToolStripMenuItem});
            this.CMenuStripFolder.Name = "CMenuStripFolder";
            this.CMenuStripFolder.Size = new System.Drawing.Size(189, 48);
            // 
            // getFilesToolStripMenuItem
            // 
            this.getFilesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("getFilesToolStripMenuItem.Image")));
            this.getFilesToolStripMenuItem.Name = "getFilesToolStripMenuItem";
            this.getFilesToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.getFilesToolStripMenuItem.Text = "Get Files";
            this.getFilesToolStripMenuItem.Click += new System.EventHandler(this.getFilesToolStripMenuItem_Click);
            // 
            // refreshListToolStripMenuItem
            // 
            this.refreshListToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("refreshListToolStripMenuItem.Image")));
            this.refreshListToolStripMenuItem.Name = "refreshListToolStripMenuItem";
            this.refreshListToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.refreshListToolStripMenuItem.Text = "Refresh Directory List";
            this.refreshListToolStripMenuItem.Click += new System.EventHandler(this.refreshListToolStripMenuItem_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.trFolder);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 74);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(310, 437);
            this.panel2.TabIndex = 116;
            // 
            // trFolder
            // 
            this.trFolder.ContextMenuStrip = this.CMenuStripFolder;
            this.trFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trFolder.ImageIndex = 0;
            this.trFolder.ImageList = this.FolderImageList;
            this.trFolder.Location = new System.Drawing.Point(0, 62);
            this.trFolder.Name = "trFolder";
            this.trFolder.SelectedImageIndex = 0;
            this.trFolder.Size = new System.Drawing.Size(310, 375);
            this.trFolder.TabIndex = 117;
            this.trFolder.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.trFolder_BeforeCollapse);
            this.trFolder.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.trFolder_BeforeExpand);
            this.trFolder.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.trFolder_NodeMouseClick);
            // 
            // FolderImageList
            // 
            this.FolderImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("FolderImageList.ImageStream")));
            this.FolderImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.FolderImageList.Images.SetKeyName(0, "drive.png");
            this.FolderImageList.Images.SetKeyName(1, "drive_network.png");
            this.FolderImageList.Images.SetKeyName(2, "folder.gif");
            this.FolderImageList.Images.SetKeyName(3, "folder_open.gif");
            this.FolderImageList.Images.SetKeyName(4, "desktop.png");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TxtFolderPath);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.chkAllLevelDir);
            this.groupBox1.Controls.Add(this.BtnFindFolder);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(310, 62);
            this.groupBox1.TabIndex = 116;
            this.groupBox1.TabStop = false;
            // 
            // chkAllLevelDir
            // 
            this.chkAllLevelDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAllLevelDir.AutoSize = true;
            this.chkAllLevelDir.Checked = true;
            this.chkAllLevelDir.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllLevelDir.Location = new System.Drawing.Point(177, 13);
            this.chkAllLevelDir.Name = "chkAllLevelDir";
            this.chkAllLevelDir.Size = new System.Drawing.Size(128, 16);
            this.chkAllLevelDir.TabIndex = 114;
            this.chkAllLevelDir.Text = "하위 디렉토리 포함";
            this.chkAllLevelDir.UseVisualStyleBackColor = true;
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.CollapseUIType = Infragistics.Win.Misc.CollapseUIType.None;
            this.ultraSplitter1.Location = new System.Drawing.Point(310, 74);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 0;
            this.ultraSplitter1.Size = new System.Drawing.Size(6, 437);
            this.ultraSplitter1.TabIndex = 117;
            // 
            // frmAVIFileUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1098, 511);
            this.Controls.Add(this.lsFiles);
            this.Controls.Add(this.ultraSplitter1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "frmAVIFileUpload";
            this.Text = "AVI Map File Upload";
            this.Load += new System.EventHandler(this.frmAVIFileUpload_Load);
            this.CFileList.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.CMenuStripFolder.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnFolderOpen;
        private System.Windows.Forms.ListView lsFiles;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.ImageList FileImageList;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.CheckBox chkFileGet;
        private System.Windows.Forms.TextBox TxtExtension;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtFolderPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtnFindFolder;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private System.Windows.Forms.ContextMenuStrip CMenuStripFolder;
        private System.Windows.Forms.ToolStripMenuItem getFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshListToolStripMenuItem;
        private System.Windows.Forms.TreeView trFolder;
        private System.Windows.Forms.ImageList FolderImageList;
        private System.Windows.Forms.CheckBox chkAllLevelDir;
        private System.Windows.Forms.ContextMenuStrip CFileList;
        private System.Windows.Forms.ToolStripMenuItem clearListToolStripMenuItem;

    }
}