namespace DACrux.Framework
{
    partial class frmUserManagement
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserManagement));
            this.pnlCondition = new System.Windows.Forms.Panel();
            this.rdoByDepartment = new System.Windows.Forms.RadioButton();
            this.rdoBySecGroup = new System.Windows.Forms.RadioButton();
            this.chkExpand = new System.Windows.Forms.CheckBox();
            this.btnSearch = new System.Windows.Forms.PictureBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.PictureBox();
            this.btnUpdate = new System.Windows.Forms.PictureBox();
            this.btnClose = new System.Windows.Forms.PictureBox();
            this.btnDelete = new System.Windows.Forms.PictureBox();
            this.imlFolder = new System.Windows.Forms.ImageList(this.components);
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.pnlTree = new System.Windows.Forms.Panel();
            this.tvUserList = new System.Windows.Forms.TreeView();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.lslSecGroup = new System.Windows.Forms.ListBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtPhoneETC = new System.Windows.Forms.TextBox();
            this.lblPhoneETC = new System.Windows.Forms.Label();
            this.txtPhoneHome = new System.Windows.Forms.TextBox();
            this.lblPhoneHome = new System.Windows.Forms.Label();
            this.txtPhoneMobile = new System.Windows.Forms.TextBox();
            this.lblPhoneMobile = new System.Windows.Forms.Label();
            this.txtPhoneOffice = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.txtPassword1 = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblPassword1 = new System.Windows.Forms.Label();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.lblPhoneOffice = new System.Windows.Forms.Label();
            this.lblGroupList = new System.Windows.Forms.Label();
            this.lblUserID = new System.Windows.Forms.Label();
            this.txtDepartment = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGroupList = new System.Windows.Forms.Button();
            this.btnSelectDel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.PictureBox();
            this.btnReset = new System.Windows.Forms.PictureBox();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlCondition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnUpdate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDelete)).BeginInit();
            this.pnlTree.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnReset)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCondition
            // 
            this.pnlCondition.BackColor = System.Drawing.Color.White;
            this.pnlCondition.Controls.Add(this.rdoByDepartment);
            this.pnlCondition.Controls.Add(this.rdoBySecGroup);
            this.pnlCondition.Controls.Add(this.chkExpand);
            this.pnlCondition.Controls.Add(this.btnSearch);
            this.pnlCondition.Controls.Add(this.txtSearch);
            this.pnlCondition.Controls.Add(this.lblUser);
            this.pnlCondition.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCondition.Location = new System.Drawing.Point(0, 32);
            this.pnlCondition.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlCondition.Name = "pnlCondition";
            this.pnlCondition.Size = new System.Drawing.Size(856, 45);
            this.pnlCondition.TabIndex = 0;
            // 
            // rdoByDepartment
            // 
            this.rdoByDepartment.AutoSize = true;
            this.rdoByDepartment.Location = new System.Drawing.Point(503, 13);
            this.rdoByDepartment.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoByDepartment.Name = "rdoByDepartment";
            this.rdoByDepartment.Size = new System.Drawing.Size(135, 18);
            this.rdoByDepartment.TabIndex = 2;
            this.rdoByDepartment.Text = "Sort By Department";
            this.rdoByDepartment.UseVisualStyleBackColor = true;
            // 
            // rdoBySecGroup
            // 
            this.rdoBySecGroup.AutoSize = true;
            this.rdoBySecGroup.Checked = true;
            this.rdoBySecGroup.Location = new System.Drawing.Point(341, 13);
            this.rdoBySecGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoBySecGroup.Name = "rdoBySecGroup";
            this.rdoBySecGroup.Size = new System.Drawing.Size(150, 18);
            this.rdoBySecGroup.TabIndex = 1;
            this.rdoBySecGroup.TabStop = true;
            this.rdoBySecGroup.Text = "Sort By Security Group";
            this.rdoBySecGroup.UseVisualStyleBackColor = true;
            // 
            // chkExpand
            // 
            this.chkExpand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkExpand.AutoSize = true;
            this.chkExpand.Checked = true;
            this.chkExpand.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExpand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkExpand.Location = new System.Drawing.Point(678, 13);
            this.chkExpand.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkExpand.Name = "chkExpand";
            this.chkExpand.Size = new System.Drawing.Size(93, 18);
            this.chkExpand.TabIndex = 3;
            this.chkExpand.Text = "Expand Tree";
            this.chkExpand.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.Location = new System.Drawing.Point(778, 11);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(70, 22);
            this.btnSearch.TabIndex = 68;
            this.btnSearch.TabStop = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Location = new System.Drawing.Point(110, 11);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(214, 22);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.Text = "All";
            this.txtSearch.Enter += new System.EventHandler(this.txtSearch_Enter);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // lblUser
            // 
            this.lblUser.BackColor = System.Drawing.Color.Transparent;
            this.lblUser.Image = ((System.Drawing.Image)(resources.GetObject("lblUser.Image")));
            this.lblUser.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblUser.Location = new System.Drawing.Point(14, 13);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(100, 19);
            this.lblUser.TabIndex = 66;
            this.lblUser.Text = "    User Name";
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.Location = new System.Drawing.Point(550, 5);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(70, 22);
            this.btnAdd.TabIndex = 65;
            this.btnAdd.TabStop = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdate.Image")));
            this.btnUpdate.Location = new System.Drawing.Point(626, 5);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(70, 22);
            this.btnUpdate.TabIndex = 64;
            this.btnUpdate.TabStop = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(778, 5);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 22);
            this.btnClose.TabIndex = 63;
            this.btnClose.TabStop = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.Location = new System.Drawing.Point(702, 5);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(70, 22);
            this.btnDelete.TabIndex = 62;
            this.btnDelete.TabStop = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // imlFolder
            // 
            this.imlFolder.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlFolder.ImageStream")));
            this.imlFolder.TransparentColor = System.Drawing.Color.Transparent;
            this.imlFolder.Images.SetKeyName(0, "2folderclosed_6.gif");
            this.imlFolder.Images.SetKeyName(1, "2folderopen_6.gif");
            this.imlFolder.Images.SetKeyName(2, "2folderclosed_1.gif");
            this.imlFolder.Images.SetKeyName(3, "2folderopen_1.gif");
            this.imlFolder.Images.SetKeyName(4, "spot1.gif");
            this.imlFolder.Images.SetKeyName(5, "701tel.gif");
            // 
            // splitter1
            // 
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 77);
            this.splitter1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(856, 5);
            this.splitter1.TabIndex = 6;
            this.splitter1.TabStop = false;
            // 
            // pnlTree
            // 
            this.pnlTree.Controls.Add(this.tvUserList);
            this.pnlTree.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlTree.Location = new System.Drawing.Point(0, 82);
            this.pnlTree.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlTree.Name = "pnlTree";
            this.pnlTree.Size = new System.Drawing.Size(315, 407);
            this.pnlTree.TabIndex = 7;
            // 
            // tvUserList
            // 
            this.tvUserList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvUserList.ImageIndex = 0;
            this.tvUserList.ImageList = this.imlFolder;
            this.tvUserList.Location = new System.Drawing.Point(6, 7);
            this.tvUserList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tvUserList.Name = "tvUserList";
            this.tvUserList.SelectedImageIndex = 0;
            this.tvUserList.Size = new System.Drawing.Size(303, 396);
            this.tvUserList.TabIndex = 0;
            this.tvUserList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvUserList_AfterSelect);
            // 
            // splitter2
            // 
            this.splitter2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitter2.Location = new System.Drawing.Point(315, 82);
            this.splitter2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(5, 407);
            this.splitter2.TabIndex = 7;
            this.splitter2.TabStop = false;
            // 
            // lslSecGroup
            // 
            this.lslSecGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lslSecGroup.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lslSecGroup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lslSecGroup.FormattingEnabled = true;
            this.lslSecGroup.ItemHeight = 14;
            this.lslSecGroup.Location = new System.Drawing.Point(475, 380);
            this.lslSecGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lslSecGroup.Name = "lslSecGroup";
            this.lslSecGroup.Size = new System.Drawing.Size(372, 58);
            this.lslSecGroup.TabIndex = 11;
            // 
            // txtEmail
            // 
            this.txtEmail.BackColor = System.Drawing.Color.Ivory;
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmail.Location = new System.Drawing.Point(475, 312);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(221, 22);
            this.txtEmail.TabIndex = 9;
            // 
            // lblEmail
            // 
            this.lblEmail.BackColor = System.Drawing.Color.Transparent;
            this.lblEmail.Image = ((System.Drawing.Image)(resources.GetObject("lblEmail.Image")));
            this.lblEmail.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblEmail.Location = new System.Drawing.Point(342, 311);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(136, 19);
            this.lblEmail.TabIndex = 110;
            this.lblEmail.Text = "    E-Mail Address";
            this.lblEmail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPhoneETC
            // 
            this.txtPhoneETC.BackColor = System.Drawing.Color.Ivory;
            this.txtPhoneETC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPhoneETC.Location = new System.Drawing.Point(475, 273);
            this.txtPhoneETC.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPhoneETC.Name = "txtPhoneETC";
            this.txtPhoneETC.Size = new System.Drawing.Size(221, 22);
            this.txtPhoneETC.TabIndex = 8;
            // 
            // lblPhoneETC
            // 
            this.lblPhoneETC.BackColor = System.Drawing.Color.Transparent;
            this.lblPhoneETC.Image = ((System.Drawing.Image)(resources.GetObject("lblPhoneETC.Image")));
            this.lblPhoneETC.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblPhoneETC.Location = new System.Drawing.Point(342, 273);
            this.lblPhoneETC.Name = "lblPhoneETC";
            this.lblPhoneETC.Size = new System.Drawing.Size(136, 19);
            this.lblPhoneETC.TabIndex = 108;
            this.lblPhoneETC.Text = "    Phone No - Etc";
            this.lblPhoneETC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPhoneHome
            // 
            this.txtPhoneHome.BackColor = System.Drawing.Color.Ivory;
            this.txtPhoneHome.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPhoneHome.Location = new System.Drawing.Point(475, 246);
            this.txtPhoneHome.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPhoneHome.Name = "txtPhoneHome";
            this.txtPhoneHome.Size = new System.Drawing.Size(221, 22);
            this.txtPhoneHome.TabIndex = 7;
            // 
            // lblPhoneHome
            // 
            this.lblPhoneHome.BackColor = System.Drawing.Color.Transparent;
            this.lblPhoneHome.Image = ((System.Drawing.Image)(resources.GetObject("lblPhoneHome.Image")));
            this.lblPhoneHome.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblPhoneHome.Location = new System.Drawing.Point(342, 248);
            this.lblPhoneHome.Name = "lblPhoneHome";
            this.lblPhoneHome.Size = new System.Drawing.Size(136, 19);
            this.lblPhoneHome.TabIndex = 106;
            this.lblPhoneHome.Text = "    Phone No - Home";
            this.lblPhoneHome.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPhoneMobile
            // 
            this.txtPhoneMobile.BackColor = System.Drawing.Color.Ivory;
            this.txtPhoneMobile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPhoneMobile.Location = new System.Drawing.Point(475, 219);
            this.txtPhoneMobile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPhoneMobile.Name = "txtPhoneMobile";
            this.txtPhoneMobile.Size = new System.Drawing.Size(221, 22);
            this.txtPhoneMobile.TabIndex = 6;
            // 
            // lblPhoneMobile
            // 
            this.lblPhoneMobile.BackColor = System.Drawing.Color.Transparent;
            this.lblPhoneMobile.Image = ((System.Drawing.Image)(resources.GetObject("lblPhoneMobile.Image")));
            this.lblPhoneMobile.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblPhoneMobile.Location = new System.Drawing.Point(342, 220);
            this.lblPhoneMobile.Name = "lblPhoneMobile";
            this.lblPhoneMobile.Size = new System.Drawing.Size(136, 19);
            this.lblPhoneMobile.TabIndex = 104;
            this.lblPhoneMobile.Text = "    Phone No - Mobile";
            this.lblPhoneMobile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPhoneOffice
            // 
            this.txtPhoneOffice.BackColor = System.Drawing.Color.Ivory;
            this.txtPhoneOffice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPhoneOffice.Location = new System.Drawing.Point(475, 192);
            this.txtPhoneOffice.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPhoneOffice.Name = "txtPhoneOffice";
            this.txtPhoneOffice.Size = new System.Drawing.Size(221, 22);
            this.txtPhoneOffice.TabIndex = 5;
            // 
            // lblUserName
            // 
            this.lblUserName.BackColor = System.Drawing.Color.Transparent;
            this.lblUserName.Image = ((System.Drawing.Image)(resources.GetObject("lblUserName.Image")));
            this.lblUserName.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblUserName.Location = new System.Drawing.Point(342, 125);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(110, 18);
            this.lblUserName.TabIndex = 102;
            this.lblUserName.Text = "    User Name";
            this.lblUserName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPassword1
            // 
            this.txtPassword1.BackColor = System.Drawing.Color.Ivory;
            this.txtPassword1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword1.Location = new System.Drawing.Point(475, 151);
            this.txtPassword1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPassword1.Name = "txtPassword1";
            this.txtPassword1.PasswordChar = '*';
            this.txtPassword1.Size = new System.Drawing.Size(221, 22);
            this.txtPassword1.TabIndex = 4;
            // 
            // txtUserName
            // 
            this.txtUserName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserName.Location = new System.Drawing.Point(475, 124);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.ReadOnly = true;
            this.txtUserName.Size = new System.Drawing.Size(221, 22);
            this.txtUserName.TabIndex = 3;
            // 
            // lblPassword1
            // 
            this.lblPassword1.BackColor = System.Drawing.Color.Transparent;
            this.lblPassword1.Image = ((System.Drawing.Image)(resources.GetObject("lblPassword1.Image")));
            this.lblPassword1.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblPassword1.Location = new System.Drawing.Point(342, 151);
            this.lblPassword1.Name = "lblPassword1";
            this.lblPassword1.Size = new System.Drawing.Size(110, 19);
            this.lblPassword1.TabIndex = 97;
            this.lblPassword1.Text = "    Password";
            this.lblPassword1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtUserID
            // 
            this.txtUserID.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtUserID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserID.Location = new System.Drawing.Point(475, 98);
            this.txtUserID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.ReadOnly = true;
            this.txtUserID.Size = new System.Drawing.Size(221, 22);
            this.txtUserID.TabIndex = 2;
            // 
            // lblPhoneOffice
            // 
            this.lblPhoneOffice.BackColor = System.Drawing.Color.Transparent;
            this.lblPhoneOffice.Image = ((System.Drawing.Image)(resources.GetObject("lblPhoneOffice.Image")));
            this.lblPhoneOffice.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblPhoneOffice.Location = new System.Drawing.Point(342, 194);
            this.lblPhoneOffice.Name = "lblPhoneOffice";
            this.lblPhoneOffice.Size = new System.Drawing.Size(136, 19);
            this.lblPhoneOffice.TabIndex = 95;
            this.lblPhoneOffice.Text = "    Phone No - Office";
            this.lblPhoneOffice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblGroupList
            // 
            this.lblGroupList.BackColor = System.Drawing.Color.Transparent;
            this.lblGroupList.Image = ((System.Drawing.Image)(resources.GetObject("lblGroupList.Image")));
            this.lblGroupList.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblGroupList.Location = new System.Drawing.Point(342, 378);
            this.lblGroupList.Name = "lblGroupList";
            this.lblGroupList.Size = new System.Drawing.Size(136, 19);
            this.lblGroupList.TabIndex = 94;
            this.lblGroupList.Text = "    User Group List";
            this.lblGroupList.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblUserID
            // 
            this.lblUserID.BackColor = System.Drawing.Color.Transparent;
            this.lblUserID.Image = ((System.Drawing.Image)(resources.GetObject("lblUserID.Image")));
            this.lblUserID.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblUserID.Location = new System.Drawing.Point(342, 97);
            this.lblUserID.Name = "lblUserID";
            this.lblUserID.Size = new System.Drawing.Size(136, 19);
            this.lblUserID.TabIndex = 8;
            this.lblUserID.Text = "    User ID";
            this.lblUserID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDepartment
            // 
            this.txtDepartment.BackColor = System.Drawing.Color.Ivory;
            this.txtDepartment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDepartment.Location = new System.Drawing.Point(475, 339);
            this.txtDepartment.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.Size = new System.Drawing.Size(221, 22);
            this.txtDepartment.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
            this.label2.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.label2.Location = new System.Drawing.Point(342, 339);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 19);
            this.label2.TabIndex = 113;
            this.label2.Text = "    Department";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnGroupList
            // 
            this.btnGroupList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGroupList.Location = new System.Drawing.Point(475, 452);
            this.btnGroupList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnGroupList.Name = "btnGroupList";
            this.btnGroupList.Size = new System.Drawing.Size(191, 30);
            this.btnGroupList.TabIndex = 12;
            this.btnGroupList.Text = "Select From List";
            this.btnGroupList.UseVisualStyleBackColor = true;
            this.btnGroupList.Visible = false;
            this.btnGroupList.Click += new System.EventHandler(this.btnGroupList_Click);
            // 
            // btnSelectDel
            // 
            this.btnSelectDel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectDel.Location = new System.Drawing.Point(672, 452);
            this.btnSelectDel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSelectDel.Name = "btnSelectDel";
            this.btnSelectDel.Size = new System.Drawing.Size(175, 30);
            this.btnSelectDel.TabIndex = 13;
            this.btnSelectDel.Text = "Delete Selection";
            this.btnSelectDel.UseVisualStyleBackColor = true;
            this.btnSelectDel.Visible = false;
            this.btnSelectDel.Click += new System.EventHandler(this.btnSelectDel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(702, 98);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(70, 22);
            this.btnSave.TabIndex = 118;
            this.btnSave.TabStop = false;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReset.Image = ((System.Drawing.Image)(resources.GetObject("btnReset.Image")));
            this.btnReset.Location = new System.Drawing.Point(778, 98);
            this.btnReset.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(70, 22);
            this.btnReset.TabIndex = 117;
            this.btnReset.TabStop = false;
            this.btnReset.Visible = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(101)))), ((int)(((byte)(126)))));
            this.pnlTop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlTop.BackgroundImage")));
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Controls.Add(this.btnClose);
            this.pnlTop.Controls.Add(this.btnDelete);
            this.pnlTop.Controls.Add(this.btnUpdate);
            this.pnlTop.Controls.Add(this.btnAdd);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(856, 32);
            this.pnlTop.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(225, 32);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "  ● User Management";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmUserManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 489);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSelectDel);
            this.Controls.Add(this.btnGroupList);
            this.Controls.Add(this.txtDepartment);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lslSecGroup);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtPhoneETC);
            this.Controls.Add(this.lblPhoneETC);
            this.Controls.Add(this.txtPhoneHome);
            this.Controls.Add(this.lblPhoneHome);
            this.Controls.Add(this.txtPhoneMobile);
            this.Controls.Add(this.lblPhoneMobile);
            this.Controls.Add(this.txtPhoneOffice);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.txtPassword1);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.lblPassword1);
            this.Controls.Add(this.txtUserID);
            this.Controls.Add(this.lblPhoneOffice);
            this.Controls.Add(this.lblGroupList);
            this.Controls.Add(this.lblUserID);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.pnlTree);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pnlCondition);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmUserManagement";
            this.Text = "User Management";
            this.Load += new System.EventHandler(this.frmUserManagement_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmUserManagement_KeyDown);
            this.pnlCondition.ResumeLayout(false);
            this.pnlCondition.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnUpdate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDelete)).EndInit();
            this.pnlTree.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnReset)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlCondition;
        private System.Windows.Forms.PictureBox btnUpdate;
        private System.Windows.Forms.PictureBox btnClose;
        private System.Windows.Forms.PictureBox btnDelete;
        private System.Windows.Forms.ImageList imlFolder;
        private System.Windows.Forms.PictureBox btnAdd;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.CheckBox chkExpand;
        private System.Windows.Forms.PictureBox btnSearch;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel pnlTree;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.TreeView tvUserList;
        private System.Windows.Forms.ListBox lslSecGroup;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtPhoneETC;
        private System.Windows.Forms.Label lblPhoneETC;
        private System.Windows.Forms.TextBox txtPhoneHome;
        private System.Windows.Forms.Label lblPhoneHome;
        private System.Windows.Forms.TextBox txtPhoneMobile;
        private System.Windows.Forms.Label lblPhoneMobile;
        private System.Windows.Forms.TextBox txtPhoneOffice;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.TextBox txtPassword1;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblPassword1;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.Label lblPhoneOffice;
        private System.Windows.Forms.Label lblGroupList;
        private System.Windows.Forms.Label lblUserID;
        private System.Windows.Forms.TextBox txtDepartment;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnGroupList;
        private System.Windows.Forms.Button btnSelectDel;
        private System.Windows.Forms.PictureBox btnSave;
        private System.Windows.Forms.PictureBox btnReset;
        private System.Windows.Forms.RadioButton rdoBySecGroup;
        private System.Windows.Forms.RadioButton rdoByDepartment;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;

    }
}