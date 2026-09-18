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
            this.picCustomCI = new System.Windows.Forms.PictureBox();
            this.lbl_DACruxVersion = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picCustomCI)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbFactory
            // 
            this.cmbFactory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cmbFactory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbFactory.ForeColor = System.Drawing.Color.White;
            this.cmbFactory.FormattingEnabled = true;
            this.cmbFactory.Location = new System.Drawing.Point(357, 289);
            this.cmbFactory.Name = "cmbFactory";
            this.cmbFactory.Size = new System.Drawing.Size(181, 20);
            this.cmbFactory.TabIndex = 5;
            this.cmbFactory.Visible = false;
            this.cmbFactory.SelectedIndexChanged += new System.EventHandler(this.cmbFactory_SelectedIndexChanged);
            // 
            // lbl_FACTORY
            // 
            this.lbl_FACTORY.BackColor = System.Drawing.Color.Transparent;
            this.lbl_FACTORY.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_FACTORY.ForeColor = System.Drawing.Color.White;
            this.lbl_FACTORY.Location = new System.Drawing.Point(229, 289);
            this.lbl_FACTORY.Name = "lbl_FACTORY";
            this.lbl_FACTORY.Size = new System.Drawing.Size(122, 20);
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
            this.txtUserID.Location = new System.Drawing.Point(357, 237);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(181, 21);
            this.txtUserID.TabIndex = 0;
            // 
            // lbl_UserID
            // 
            this.lbl_UserID.BackColor = System.Drawing.Color.Transparent;
            this.lbl_UserID.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_UserID.ForeColor = System.Drawing.Color.White;
            this.lbl_UserID.Location = new System.Drawing.Point(229, 237);
            this.lbl_UserID.Name = "lbl_UserID";
            this.lbl_UserID.Size = new System.Drawing.Size(122, 20);
            this.lbl_UserID.TabIndex = 3;
            this.lbl_UserID.Text = "User ID";
            this.lbl_UserID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.ForeColor = System.Drawing.SystemColors.Window;
            this.txtPassword.Location = new System.Drawing.Point(357, 262);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(181, 21);
            this.txtPassword.TabIndex = 1;
            // 
            // lbl_Password
            // 
            this.lbl_Password.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Password.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Password.ForeColor = System.Drawing.Color.White;
            this.lbl_Password.Location = new System.Drawing.Point(229, 263);
            this.lbl_Password.Name = "lbl_Password";
            this.lbl_Password.Size = new System.Drawing.Size(122, 20);
            this.lbl_Password.TabIndex = 3;
            this.lbl_Password.Text = "Password";
            this.lbl_Password.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // butCancel
            // 
            this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butCancel.BackColor = System.Drawing.SystemColors.ControlText;
            this.butCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.butCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.butCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.butCancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.butCancel.Image = ((System.Drawing.Image)(resources.GetObject("butCancel.Image")));
            this.butCancel.Location = new System.Drawing.Point(466, 315);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(72, 25);
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
            this.butOk.Location = new System.Drawing.Point(388, 315);
            this.butOk.Name = "butOk";
            this.butOk.Size = new System.Drawing.Size(72, 25);
            this.butOk.TabIndex = 2;
            this.butOk.UseVisualStyleBackColor = false;
            this.butOk.Click += new System.EventHandler(this.butOk_Click);
            // 
            // butOption
            // 
            this.butOption.BackColor = System.Drawing.SystemColors.ControlText;
            this.butOption.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.butOption.Image = ((System.Drawing.Image)(resources.GetObject("butOption.Image")));
            this.butOption.Location = new System.Drawing.Point(310, 315);
            this.butOption.Name = "butOption";
            this.butOption.Size = new System.Drawing.Size(72, 25);
            this.butOption.TabIndex = 4;
            this.butOption.UseVisualStyleBackColor = false;
            this.butOption.Visible = false;
            this.butOption.Click += new System.EventHandler(this.butOption_Click);
            // 
            // picCustomCI
            // 
            this.picCustomCI.BackColor = System.Drawing.Color.Transparent;
            this.picCustomCI.Image = ((System.Drawing.Image)(resources.GetObject("picCustomCI.Image")));
            this.picCustomCI.Location = new System.Drawing.Point(70, 319);
            this.picCustomCI.Name = "picCustomCI";
            this.picCustomCI.Size = new System.Drawing.Size(94, 25);
            this.picCustomCI.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picCustomCI.TabIndex = 6;
            this.picCustomCI.TabStop = false;
            // 
            // lbl_DACruxVersion
            // 
            this.lbl_DACruxVersion.AutoSize = true;
            this.lbl_DACruxVersion.BackColor = System.Drawing.Color.Transparent;
            this.lbl_DACruxVersion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_DACruxVersion.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_DACruxVersion.ForeColor = System.Drawing.Color.White;
            this.lbl_DACruxVersion.Location = new System.Drawing.Point(117, 22);
            this.lbl_DACruxVersion.Name = "lbl_DACruxVersion";
            this.lbl_DACruxVersion.Size = new System.Drawing.Size(52, 15);
            this.lbl_DACruxVersion.TabIndex = 7;
            this.lbl_DACruxVersion.Text = "V5.014.1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(10, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 19);
            this.label2.TabIndex = 7;
            this.label2.Text = "__________";
            // 
            // frmLoginDACrux
            // 
            this.AcceptButton = this.butOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlText;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(550, 350);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbl_DACruxVersion);
            this.Controls.Add(this.picCustomCI);
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
            this.Name = "frmLoginDACrux";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmLoginDACrux_Load);
            this.Shown += new System.EventHandler(this.frmLoginDACrux_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.picCustomCI)).EndInit();
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
        private System.Windows.Forms.PictureBox picCustomCI;
        private System.Windows.Forms.Label lbl_DACruxVersion;
        private System.Windows.Forms.Label label2;

    }
}