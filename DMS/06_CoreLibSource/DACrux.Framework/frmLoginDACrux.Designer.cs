namespace DACrux.Framework
{
    partial class frmLoginDACrux
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLoginDACrux));
            this.cmbFactory = new System.Windows.Forms.ComboBox();
            this.lbl_FACTORY = new System.Windows.Forms.Label();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.lbl_UserID = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lbl_Password = new System.Windows.Forms.Label();
            this.butCancel = new System.Windows.Forms.Button();
            this.butOk = new System.Windows.Forms.Button();
            this.butOption = new System.Windows.Forms.Button();
            this.lbl_SystemName = new System.Windows.Forms.Label();
            this.lbl_SubTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbFactory
            // 
            this.cmbFactory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cmbFactory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbFactory.ForeColor = System.Drawing.Color.White;
            this.cmbFactory.FormattingEnabled = true;
            this.cmbFactory.Location = new System.Drawing.Point(510, 414);
            this.cmbFactory.Margin = new System.Windows.Forms.Padding(4);
            this.cmbFactory.Name = "cmbFactory";
            this.cmbFactory.Size = new System.Drawing.Size(257, 26);
            this.cmbFactory.TabIndex = 5;
            this.cmbFactory.Visible = false;
            this.cmbFactory.SelectedIndexChanged += new System.EventHandler(this.cmbFactory_SelectedIndexChanged);
            // 
            // lbl_FACTORY
            // 
            this.lbl_FACTORY.BackColor = System.Drawing.Color.Transparent;
            this.lbl_FACTORY.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_FACTORY.ForeColor = System.Drawing.Color.White;
            this.lbl_FACTORY.Location = new System.Drawing.Point(327, 414);
            this.lbl_FACTORY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_FACTORY.Name = "lbl_FACTORY";
            this.lbl_FACTORY.Size = new System.Drawing.Size(174, 30);
            this.lbl_FACTORY.TabIndex = 3;
            this.lbl_FACTORY.Text = "Factory";
            this.lbl_FACTORY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl_FACTORY.Visible = false;
            // 
            // txtUserID
            // 
            this.txtUserID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtUserID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserID.ForeColor = System.Drawing.SystemColors.Window;
            this.txtUserID.Location = new System.Drawing.Point(510, 347);
            this.txtUserID.Margin = new System.Windows.Forms.Padding(4);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(258, 28);
            this.txtUserID.TabIndex = 0;
            // 
            // lbl_UserID
            // 
            this.lbl_UserID.BackColor = System.Drawing.Color.Transparent;
            this.lbl_UserID.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_UserID.ForeColor = System.Drawing.Color.White;
            this.lbl_UserID.Location = new System.Drawing.Point(327, 347);
            this.lbl_UserID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_UserID.Name = "lbl_UserID";
            this.lbl_UserID.Size = new System.Drawing.Size(174, 30);
            this.lbl_UserID.TabIndex = 3;
            this.lbl_UserID.Text = "User ID";
            this.lbl_UserID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.ForeColor = System.Drawing.SystemColors.Window;
            this.txtPassword.Location = new System.Drawing.Point(510, 384);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(258, 28);
            this.txtPassword.TabIndex = 1;
            // 
            // lbl_Password
            // 
            this.lbl_Password.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Password.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Password.ForeColor = System.Drawing.Color.White;
            this.lbl_Password.Location = new System.Drawing.Point(327, 385);
            this.lbl_Password.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_Password.Name = "lbl_Password";
            this.lbl_Password.Size = new System.Drawing.Size(174, 30);
            this.lbl_Password.TabIndex = 3;
            this.lbl_Password.Text = "Password";
            this.lbl_Password.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // butCancel
            // 
            this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butCancel.BackColor = System.Drawing.Color.Transparent;
            this.butCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.butCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.butCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.butCancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.butCancel.Image = ((System.Drawing.Image)(resources.GetObject("butCancel.Image")));
            this.butCancel.Location = new System.Drawing.Point(668, 475);
            this.butCancel.Margin = new System.Windows.Forms.Padding(4);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(77, 28);
            this.butCancel.TabIndex = 3;
            this.butCancel.UseVisualStyleBackColor = false;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // butOk
            // 
            this.butOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butOk.BackColor = System.Drawing.Color.Transparent;
            this.butOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.butOk.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.butOk.ForeColor = System.Drawing.Color.Transparent;
            this.butOk.Image = ((System.Drawing.Image)(resources.GetObject("butOk.Image")));
            this.butOk.Location = new System.Drawing.Point(583, 475);
            this.butOk.Margin = new System.Windows.Forms.Padding(4);
            this.butOk.Name = "butOk";
            this.butOk.Size = new System.Drawing.Size(77, 28);
            this.butOk.TabIndex = 2;
            this.butOk.UseVisualStyleBackColor = false;
            this.butOk.Click += new System.EventHandler(this.butOk_Click);
            // 
            // butOption
            // 
            this.butOption.BackColor = System.Drawing.Color.Transparent;
            this.butOption.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.butOption.Image = ((System.Drawing.Image)(resources.GetObject("butOption.Image")));
            this.butOption.Location = new System.Drawing.Point(495, 475);
            this.butOption.Margin = new System.Windows.Forms.Padding(4);
            this.butOption.Name = "butOption";
            this.butOption.Size = new System.Drawing.Size(80, 28);
            this.butOption.TabIndex = 4;
            this.butOption.UseVisualStyleBackColor = false;
            this.butOption.Visible = false;
            this.butOption.Click += new System.EventHandler(this.butOption_Click);
            // 
            // lbl_SystemName
            // 
            this.lbl_SystemName.AutoSize = true;
            this.lbl_SystemName.BackColor = System.Drawing.Color.Transparent;
            this.lbl_SystemName.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_SystemName.ForeColor = System.Drawing.Color.White;
            this.lbl_SystemName.Location = new System.Drawing.Point(12, 9);
            this.lbl_SystemName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_SystemName.Name = "lbl_SystemName";
            this.lbl_SystemName.Size = new System.Drawing.Size(437, 72);
            this.lbl_SystemName.TabIndex = 6;
            this.lbl_SystemName.Text = "SCK E-TEST POC";
            // 
            // lbl_SubTitle
            // 
            this.lbl_SubTitle.AutoSize = true;
            this.lbl_SubTitle.BackColor = System.Drawing.Color.Transparent;
            this.lbl_SubTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_SubTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(58)))), ((int)(((byte)(104)))));
            this.lbl_SubTitle.Location = new System.Drawing.Point(14, 77);
            this.lbl_SubTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_SubTitle.Name = "lbl_SubTitle";
            this.lbl_SubTitle.Size = new System.Drawing.Size(297, 25);
            this.lbl_SubTitle.TabIndex = 8;
            this.lbl_SubTitle.Text = "Chain · Defect · E-Test Correlation";
            // 
            // frmLoginDACrux
            // 
            this.AcceptButton = this.butOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlText;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(786, 525);
            this.ControlBox = false;
            this.Controls.Add(this.lbl_SubTitle);
            this.Controls.Add(this.lbl_SystemName);
            this.Controls.Add(this.butOk);
            this.Controls.Add(this.butOption);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.lbl_Password);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lbl_UserID);
            this.Controls.Add(this.txtUserID);
            this.Controls.Add(this.lbl_FACTORY);
            this.Controls.Add(this.cmbFactory);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmLoginDACrux";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmLoginDACrux_Load);
            this.Shown += new System.EventHandler(this.frmLoginDACrux_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbFactory;
        private System.Windows.Forms.Label lbl_FACTORY;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.Label lbl_UserID;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lbl_Password;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Button butOk;
        private System.Windows.Forms.Button butOption;
        private System.Windows.Forms.Label lbl_SystemName;
        private System.Windows.Forms.Label lbl_SubTitle;

    }
}