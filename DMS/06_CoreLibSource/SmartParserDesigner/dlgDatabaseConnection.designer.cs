namespace SmartParser.Designer
{
    partial class dlgDatabaseConnection
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
            this.lblPort = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblHost = new System.Windows.Forms.Label();
            this.lblUserID = new System.Windows.Forms.Label();
            this.lblDatabaseName = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtDatabaseName = new System.Windows.Forms.TextBox();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.rbtIP = new System.Windows.Forms.RadioButton();
            this.rbtName = new System.Windows.Forms.RadioButton();
            this.ipaHost = new IPAddressControlLib.IPAddressControl();
            this.grbConnection = new System.Windows.Forms.GroupBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.lblServerType = new System.Windows.Forms.Label();
            this.cboServer = new System.Windows.Forms.ComboBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grbList = new System.Windows.Forms.GroupBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtConnectionName = new System.Windows.Forms.TextBox();
            this.lvConnection = new System.Windows.Forms.ListView();
            this.grbConnection.SuspendLayout();
            this.grbList.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(10, 93);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(27, 13);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = DACrux.SP.Common.MultiLang.SelectLang["Port"];
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(10, 165);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = DACrux.SP.Common.MultiLang.SelectLang["Password"];
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = true;
            this.lblHost.Location = new System.Drawing.Point(10, 46);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(29, 13);
            this.lblHost.TabIndex = 5;
            this.lblHost.Text = DACrux.SP.Common.MultiLang.SelectLang["Host"];
            // 
            // lblUserID
            // 
            this.lblUserID.AutoSize = true;
            this.lblUserID.Location = new System.Drawing.Point(10, 141);
            this.lblUserID.Name = "lblUserID";
            this.lblUserID.Size = new System.Drawing.Size(43, 13);
            this.lblUserID.TabIndex = 6;
            this.lblUserID.Text = DACrux.SP.Common.MultiLang.SelectLang["User ID"];
            // 
            // lblDatabaseName
            // 
            this.lblDatabaseName.AutoSize = true;
            this.lblDatabaseName.Location = new System.Drawing.Point(10, 117);
            this.lblDatabaseName.Name = "lblDatabaseName";
            this.lblDatabaseName.Size = new System.Drawing.Size(83, 13);
            this.lblDatabaseName.TabIndex = 7;
            this.lblDatabaseName.Text = DACrux.SP.Common.MultiLang.SelectLang["Database Name"];
            // 
            // txtPort
            // 
            this.txtPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPort.Location = new System.Drawing.Point(109, 91);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(108, 21);
            this.txtPort.TabIndex = 5;
            // 
            // txtDatabaseName
            // 
            this.txtDatabaseName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDatabaseName.Location = new System.Drawing.Point(109, 115);
            this.txtDatabaseName.Name = "txtDatabaseName";
            this.txtDatabaseName.Size = new System.Drawing.Size(108, 21);
            this.txtDatabaseName.TabIndex = 6;
            // 
            // txtUserID
            // 
            this.txtUserID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserID.Location = new System.Drawing.Point(109, 139);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(108, 21);
            this.txtUserID.TabIndex = 7;
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Location = new System.Drawing.Point(109, 163);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(108, 21);
            this.txtPassword.TabIndex = 8;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // txtHost
            // 
            this.txtHost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHost.Location = new System.Drawing.Point(109, 67);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(108, 21);
            this.txtHost.TabIndex = 4;
            // 
            // rbtIP
            // 
            this.rbtIP.AutoSize = true;
            this.rbtIP.Checked = true;
            this.rbtIP.Location = new System.Drawing.Point(51, 45);
            this.rbtIP.Name = "rbtIP";
            this.rbtIP.Size = new System.Drawing.Size(35, 17);
            this.rbtIP.TabIndex = 1;
            this.rbtIP.TabStop = true;
            this.rbtIP.Text = "IP";
            this.rbtIP.UseVisualStyleBackColor = true;
            this.rbtIP.CheckedChanged += new System.EventHandler(this.rbtIP_CheckedChanged);
            // 
            // rbtName
            // 
            this.rbtName.AutoSize = true;
            this.rbtName.Location = new System.Drawing.Point(51, 69);
            this.rbtName.Name = "rbtName";
            this.rbtName.Size = new System.Drawing.Size(52, 17);
            this.rbtName.TabIndex = 3;
            this.rbtName.Text = DACrux.SP.Common.MultiLang.SelectLang["Name"];
            this.rbtName.UseVisualStyleBackColor = true;
            // 
            // ipaHost
            // 
            this.ipaHost.AllowInternalTab = false;
            this.ipaHost.AutoHeight = true;
            this.ipaHost.BackColor = System.Drawing.SystemColors.Window;
            this.ipaHost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ipaHost.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ipaHost.Location = new System.Drawing.Point(109, 45);
            this.ipaHost.MinimumSize = new System.Drawing.Size(88, 21);
            this.ipaHost.Name = "ipaHost";
            this.ipaHost.ReadOnly = false;
            this.ipaHost.Size = new System.Drawing.Size(108, 21);
            this.ipaHost.TabIndex = 2;
            this.ipaHost.Text = "...";
            // 
            // grbConnection
            // 
            this.grbConnection.Controls.Add(this.btnTest);
            this.grbConnection.Controls.Add(this.lblServerType);
            this.grbConnection.Controls.Add(this.ipaHost);
            this.grbConnection.Controls.Add(this.lblPort);
            this.grbConnection.Controls.Add(this.cboServer);
            this.grbConnection.Controls.Add(this.rbtName);
            this.grbConnection.Controls.Add(this.lblPassword);
            this.grbConnection.Controls.Add(this.rbtIP);
            this.grbConnection.Controls.Add(this.lblHost);
            this.grbConnection.Controls.Add(this.lblUserID);
            this.grbConnection.Controls.Add(this.txtHost);
            this.grbConnection.Controls.Add(this.lblDatabaseName);
            this.grbConnection.Controls.Add(this.txtPassword);
            this.grbConnection.Controls.Add(this.txtPort);
            this.grbConnection.Controls.Add(this.txtUserID);
            this.grbConnection.Controls.Add(this.txtDatabaseName);
            this.grbConnection.Location = new System.Drawing.Point(12, 11);
            this.grbConnection.Name = "grbConnection";
            this.grbConnection.Size = new System.Drawing.Size(229, 217);
            this.grbConnection.TabIndex = 21;
            this.grbConnection.TabStop = false;
            this.grbConnection.Text = DACrux.SP.Common.MultiLang.SelectLang["Connection Information"];
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(13, 188);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(204, 19);
            this.btnTest.TabIndex = 13;
            this.btnTest.Text = DACrux.SP.Common.MultiLang.SelectLang["Connection Test"];
            this.btnTest.UseCompatibleTextRendering = true;
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // lblServerType
            // 
            this.lblServerType.AutoSize = true;
            this.lblServerType.Location = new System.Drawing.Point(10, 20);
            this.lblServerType.Name = "lblServerType";
            this.lblServerType.Size = new System.Drawing.Size(66, 13);
            this.lblServerType.TabIndex = 23;
            this.lblServerType.Text = DACrux.SP.Common.MultiLang.SelectLang["Server Type"];
            // 
            // cboServer
            // 
            this.cboServer.FormattingEnabled = true;
            this.cboServer.Location = new System.Drawing.Point(109, 18);
            this.cboServer.Name = "cboServer";
            this.cboServer.Size = new System.Drawing.Size(108, 21);
            this.cboServer.TabIndex = 0;
            this.cboServer.SelectedIndexChanged += new System.EventHandler(this.cboServer_SelectedIndexChanged);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(263, 234);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 28);
            this.btnOk.TabIndex = 14;
            this.btnOk.Text = DACrux.SP.Common.MultiLang.SelectLang["OK"];
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(344, 234);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 28);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = DACrux.SP.Common.MultiLang.SelectLang["Cancel"];
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // grbList
            // 
            this.grbList.Controls.Add(this.btnDelete);
            this.grbList.Controls.Add(this.btnSave);
            this.grbList.Controls.Add(this.txtConnectionName);
            this.grbList.Controls.Add(this.lvConnection);
            this.grbList.Location = new System.Drawing.Point(247, 11);
            this.grbList.Name = "grbList";
            this.grbList.Size = new System.Drawing.Size(172, 217);
            this.grbList.TabIndex = 24;
            this.grbList.TabStop = false;
            this.grbList.Text = DACrux.SP.Common.MultiLang.SelectLang["Connection List"];
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(123, 18);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(38, 19);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = DACrux.SP.Common.MultiLang.SelectLang["Delete"];
            this.btnDelete.UseCompatibleTextRendering = true;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(90, 18);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(33, 19);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = DACrux.SP.Common.MultiLang.SelectLang["Save"];
            this.btnSave.UseCompatibleTextRendering = true;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtConnectionName
            // 
            this.txtConnectionName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConnectionName.Location = new System.Drawing.Point(13, 18);
            this.txtConnectionName.Name = "txtConnectionName";
            this.txtConnectionName.Size = new System.Drawing.Size(76, 21);
            this.txtConnectionName.TabIndex = 9;
            // 
            // lvConnection
            // 
            this.lvConnection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvConnection.Location = new System.Drawing.Point(13, 40);
            this.lvConnection.Name = "lvConnection";
            this.lvConnection.Size = new System.Drawing.Size(148, 167);
            this.lvConnection.TabIndex = 12;
            this.lvConnection.UseCompatibleStateImageBehavior = false;
            this.lvConnection.View = System.Windows.Forms.View.List;
            this.lvConnection.SelectedIndexChanged += new System.EventHandler(this.lvConnection_SelectedIndexChanged);
            // 
            // dlgDatabaseConnection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 268);
            this.Controls.Add(this.grbList);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.grbConnection);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "dlgDatabaseConnection";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = DACrux.SP.Common.MultiLang.SelectLang["Database Connection Test"];
            this.Load += new System.EventHandler(this.DlgOracleConnection_Load);
            this.grbConnection.ResumeLayout(false);
            this.grbConnection.PerformLayout();
            this.grbList.ResumeLayout(false);
            this.grbList.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.Label lblUserID;
        private System.Windows.Forms.Label lblDatabaseName;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtDatabaseName;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.RadioButton rbtIP;
        private System.Windows.Forms.RadioButton rbtName;
        private IPAddressControlLib.IPAddressControl ipaHost;
        private System.Windows.Forms.GroupBox grbConnection;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox grbList;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtConnectionName;
        private System.Windows.Forms.ListView lvConnection;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ComboBox cboServer;
        private System.Windows.Forms.Label lblServerType;
        private System.Windows.Forms.Button btnTest;
    }
}