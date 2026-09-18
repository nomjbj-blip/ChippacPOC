namespace DACrux.Framework.Controls
{
    partial class DUCLotGroup
    {
		/// <summary> 
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;

        /// <summary> 
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

        #region 구성 요소 디자이너에서 생성한 코드
		/// <summary> 
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DUCLotGroup));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.butAddGroup = new System.Windows.Forms.Button();
            this.txtGroupName = new System.Windows.Forms.TextBox();
            this.ctxMenu = new System.Windows.Forms.ContextMenu();
            this.mnuAddLot = new System.Windows.Forms.MenuItem();
            this.mnuDelLot = new System.Windows.Forms.MenuItem();
            this.mnuDelGroup = new System.Windows.Forms.MenuItem();
            this.mnuResetAll = new System.Windows.Forms.MenuItem();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.HideSelection = false;
            this.treeView1.Location = new System.Drawing.Point(0, 27);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(208, 357);
            this.treeView1.TabIndex = 2;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView1_DragDrop);
            this.treeView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView1_DragEnter);
            this.treeView1.DragOver += new System.Windows.Forms.DragEventHandler(this.treeView1_DragOver);
            this.treeView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDown);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.butAddGroup);
            this.panel2.Controls.Add(this.txtGroupName);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(208, 27);
            this.panel2.TabIndex = 3;
            // 
            // butAddGroup
            // 
            this.butAddGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butAddGroup.Image = ((System.Drawing.Image)(resources.GetObject("butAddGroup.Image")));
            this.butAddGroup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butAddGroup.Location = new System.Drawing.Point(99, 1);
            this.butAddGroup.Name = "butAddGroup";
            this.butAddGroup.Size = new System.Drawing.Size(107, 24);
            this.butAddGroup.TabIndex = 0;
            this.butAddGroup.Text = "    Add Group";
            this.butAddGroup.Click += new System.EventHandler(this.butAddGroup_Click);
            // 
            // txtGroupName
            // 
            this.txtGroupName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGroupName.Location = new System.Drawing.Point(2, 3);
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(91, 21);
            this.txtGroupName.TabIndex = 2;
            this.txtGroupName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtGroupName_KeyUp);
            // 
            // ctxMenu
            // 
            this.ctxMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuAddLot,
            this.mnuDelLot,
            this.mnuDelGroup,
            this.mnuResetAll});
            // 
            // mnuAddLot
            // 
            this.mnuAddLot.Index = 0;
            this.mnuAddLot.Text = "Add Lot";
            this.mnuAddLot.Click += new System.EventHandler(this.mnuAddLot_Click);
            // 
            // mnuDelLot
            // 
            this.mnuDelLot.Index = 1;
            this.mnuDelLot.Text = "Delete Lot";
            this.mnuDelLot.Click += new System.EventHandler(this.mnuDelLot_Click);
            // 
            // mnuDelGroup
            // 
            this.mnuDelGroup.Index = 2;
            this.mnuDelGroup.Text = "Delete Group";
            this.mnuDelGroup.Click += new System.EventHandler(this.mnuDelGroup_Click);
            // 
            // mnuResetAll
            // 
            this.mnuResetAll.Index = 3;
            this.mnuResetAll.Text = "Reset All";
            this.mnuResetAll.Click += new System.EventHandler(this.mnuResetAll_Click);
            // 
            // uclLotGroup
            // 
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.panel2);
            this.Name = "uclLotGroup";
            this.Size = new System.Drawing.Size(208, 384);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

        private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.TextBox txtGroupName;
		private System.Windows.Forms.Button butAddGroup;
		private string m_strSelectedGroup = string.Empty;
		private int m_iSelectedGroupNodeIndex = -1;
		private System.Windows.Forms.ContextMenu ctxMenu;
		private System.Windows.Forms.MenuItem mnuDelLot;
		private System.Windows.Forms.MenuItem mnuDelGroup;
		private System.Windows.Forms.MenuItem mnuAddLot;
		private System.Windows.Forms.MenuItem mnuResetAll;
		private string m_strSelectedLot = string.Empty;
    }
}
