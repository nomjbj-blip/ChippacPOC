namespace DACrux.Framework
{
    partial class frmUserEnvironment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserEnvironment));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblGroupList = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblPhoneETC = new System.Windows.Forms.Label();
            this.lblPhoneHome = new System.Windows.Forms.Label();
            this.lblPhoneMobile = new System.Windows.Forms.Label();
            this.lblPhoneOffice = new System.Windows.Forms.Label();
            this.lblPassword2 = new System.Windows.Forms.Label();
            this.lblPassword1 = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblUserID = new System.Windows.Forms.Label();
            this.lstGroup = new System.Windows.Forms.ListBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtPhoneETC = new System.Windows.Forms.TextBox();
            this.txtPhoneHome = new System.Windows.Forms.TextBox();
            this.txtPhoneMobile = new System.Windows.Forms.TextBox();
            this.txtPhoneOffice = new System.Windows.Forms.TextBox();
            this.txtPassword2 = new System.Windows.Forms.TextBox();
            this.txtPassword1 = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.PictureBox();
            this.btnClose = new System.Windows.Forms.PictureBox();
            this.picLine = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLine)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlTop.BackgroundImage")));
            this.pnlTop.Controls.Add(this.lblGroupList);
            this.pnlTop.Controls.Add(this.lblEmail);
            this.pnlTop.Controls.Add(this.lblPhoneETC);
            this.pnlTop.Controls.Add(this.lblPhoneHome);
            this.pnlTop.Controls.Add(this.lblPhoneMobile);
            this.pnlTop.Controls.Add(this.lblPhoneOffice);
            this.pnlTop.Controls.Add(this.lblPassword2);
            this.pnlTop.Controls.Add(this.lblPassword1);
            this.pnlTop.Controls.Add(this.lblUserName);
            this.pnlTop.Controls.Add(this.lblUserID);
            this.pnlTop.Controls.Add(this.lstGroup);
            this.pnlTop.Controls.Add(this.txtEmail);
            this.pnlTop.Controls.Add(this.txtPhoneETC);
            this.pnlTop.Controls.Add(this.txtPhoneHome);
            this.pnlTop.Controls.Add(this.txtPhoneMobile);
            this.pnlTop.Controls.Add(this.txtPhoneOffice);
            this.pnlTop.Controls.Add(this.txtPassword2);
            this.pnlTop.Controls.Add(this.txtPassword1);
            this.pnlTop.Controls.Add(this.txtUserName);
            this.pnlTop.Controls.Add(this.txtUserID);
            this.pnlTop.Controls.Add(this.btnSave);
            this.pnlTop.Controls.Add(this.btnClose);
            this.pnlTop.Controls.Add(this.picLine);
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(406, 374);
            this.pnlTop.TabIndex = 7;
            // 
            // lblGroupList
            // 
            this.lblGroupList.AutoSize = true;
            this.lblGroupList.BackColor = System.Drawing.Color.Transparent;
            this.lblGroupList.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGroupList.Image = ((System.Drawing.Image)(resources.GetObject("lblGroupList.Image")));
            this.lblGroupList.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblGroupList.Location = new System.Drawing.Point(15, 306);
            this.lblGroupList.Name = "lblGroupList";
            this.lblGroupList.Size = new System.Drawing.Size(101, 14);
            this.lblGroupList.TabIndex = 138;
            this.lblGroupList.Text = "     My Group List";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.BackColor = System.Drawing.Color.Transparent;
            this.lblEmail.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.Image = ((System.Drawing.Image)(resources.GetObject("lblEmail.Image")));
            this.lblEmail.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblEmail.Location = new System.Drawing.Point(15, 277);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(104, 14);
            this.lblEmail.TabIndex = 137;
            this.lblEmail.Text = "     E-Mail Address";
            // 
            // lblPhoneETC
            // 
            this.lblPhoneETC.AutoSize = true;
            this.lblPhoneETC.BackColor = System.Drawing.Color.Transparent;
            this.lblPhoneETC.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhoneETC.Image = ((System.Drawing.Image)(resources.GetObject("lblPhoneETC.Image")));
            this.lblPhoneETC.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPhoneETC.Location = new System.Drawing.Point(15, 249);
            this.lblPhoneETC.Name = "lblPhoneETC";
            this.lblPhoneETC.Size = new System.Drawing.Size(111, 14);
            this.lblPhoneETC.TabIndex = 136;
            this.lblPhoneETC.Text = "     Phone No - Etc";
            // 
            // lblPhoneHome
            // 
            this.lblPhoneHome.AutoSize = true;
            this.lblPhoneHome.BackColor = System.Drawing.Color.Transparent;
            this.lblPhoneHome.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhoneHome.Image = ((System.Drawing.Image)(resources.GetObject("lblPhoneHome.Image")));
            this.lblPhoneHome.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPhoneHome.Location = new System.Drawing.Point(15, 222);
            this.lblPhoneHome.Name = "lblPhoneHome";
            this.lblPhoneHome.Size = new System.Drawing.Size(125, 14);
            this.lblPhoneHome.TabIndex = 135;
            this.lblPhoneHome.Text = "     Phone No - Home";
            // 
            // lblPhoneMobile
            // 
            this.lblPhoneMobile.AutoSize = true;
            this.lblPhoneMobile.BackColor = System.Drawing.Color.Transparent;
            this.lblPhoneMobile.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhoneMobile.Image = ((System.Drawing.Image)(resources.GetObject("lblPhoneMobile.Image")));
            this.lblPhoneMobile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPhoneMobile.Location = new System.Drawing.Point(15, 195);
            this.lblPhoneMobile.Name = "lblPhoneMobile";
            this.lblPhoneMobile.Size = new System.Drawing.Size(127, 14);
            this.lblPhoneMobile.TabIndex = 134;
            this.lblPhoneMobile.Text = "     Phone No - Mobile";
            // 
            // lblPhoneOffice
            // 
            this.lblPhoneOffice.AutoSize = true;
            this.lblPhoneOffice.BackColor = System.Drawing.Color.Transparent;
            this.lblPhoneOffice.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhoneOffice.Image = ((System.Drawing.Image)(resources.GetObject("lblPhoneOffice.Image")));
            this.lblPhoneOffice.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPhoneOffice.Location = new System.Drawing.Point(15, 168);
            this.lblPhoneOffice.Name = "lblPhoneOffice";
            this.lblPhoneOffice.Size = new System.Drawing.Size(125, 14);
            this.lblPhoneOffice.TabIndex = 133;
            this.lblPhoneOffice.Text = "     Phone No - Office";
            // 
            // lblPassword2
            // 
            this.lblPassword2.AutoSize = true;
            this.lblPassword2.BackColor = System.Drawing.Color.Transparent;
            this.lblPassword2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword2.Image = ((System.Drawing.Image)(resources.GetObject("lblPassword2.Image")));
            this.lblPassword2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPassword2.Location = new System.Drawing.Point(15, 140);
            this.lblPassword2.Name = "lblPassword2";
            this.lblPassword2.Size = new System.Drawing.Size(123, 14);
            this.lblPassword2.TabIndex = 132;
            this.lblPassword2.Text = "     Confirm Password";
            // 
            // lblPassword1
            // 
            this.lblPassword1.AutoSize = true;
            this.lblPassword1.BackColor = System.Drawing.Color.Transparent;
            this.lblPassword1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword1.Image = ((System.Drawing.Image)(resources.GetObject("lblPassword1.Image")));
            this.lblPassword1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPassword1.Location = new System.Drawing.Point(15, 113);
            this.lblPassword1.Name = "lblPassword1";
            this.lblPassword1.Size = new System.Drawing.Size(78, 14);
            this.lblPassword1.TabIndex = 131;
            this.lblPassword1.Text = "     Password";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.BackColor = System.Drawing.Color.Transparent;
            this.lblUserName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserName.Image = ((System.Drawing.Image)(resources.GetObject("lblUserName.Image")));
            this.lblUserName.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblUserName.Location = new System.Drawing.Point(15, 86);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(86, 14);
            this.lblUserName.TabIndex = 130;
            this.lblUserName.Text = "     User Name";
            // 
            // lblUserID
            // 
            this.lblUserID.AutoSize = true;
            this.lblUserID.BackColor = System.Drawing.Color.Transparent;
            this.lblUserID.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserID.Image = ((System.Drawing.Image)(resources.GetObject("lblUserID.Image")));
            this.lblUserID.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblUserID.Location = new System.Drawing.Point(15, 59);
            this.lblUserID.Name = "lblUserID";
            this.lblUserID.Size = new System.Drawing.Size(67, 14);
            this.lblUserID.TabIndex = 129;
            this.lblUserID.Text = "     User ID";
            // 
            // lstGroup
            // 
            this.lstGroup.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lstGroup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstGroup.FormattingEnabled = true;
            this.lstGroup.ItemHeight = 14;
            this.lstGroup.Location = new System.Drawing.Point(157, 303);
            this.lstGroup.Name = "lstGroup";
            this.lstGroup.Size = new System.Drawing.Size(235, 58);
            this.lstGroup.TabIndex = 92;
            // 
            // txtEmail
            // 
            this.txtEmail.BackColor = System.Drawing.Color.Ivory;
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmail.Location = new System.Drawing.Point(157, 275);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(235, 22);
            this.txtEmail.TabIndex = 91;
            // 
            // txtPhoneETC
            // 
            this.txtPhoneETC.BackColor = System.Drawing.Color.Ivory;
            this.txtPhoneETC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPhoneETC.Location = new System.Drawing.Point(157, 247);
            this.txtPhoneETC.Name = "txtPhoneETC";
            this.txtPhoneETC.Size = new System.Drawing.Size(235, 22);
            this.txtPhoneETC.TabIndex = 89;
            // 
            // txtPhoneHome
            // 
            this.txtPhoneHome.BackColor = System.Drawing.Color.Ivory;
            this.txtPhoneHome.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPhoneHome.Location = new System.Drawing.Point(157, 220);
            this.txtPhoneHome.Name = "txtPhoneHome";
            this.txtPhoneHome.Size = new System.Drawing.Size(235, 22);
            this.txtPhoneHome.TabIndex = 87;
            // 
            // txtPhoneMobile
            // 
            this.txtPhoneMobile.BackColor = System.Drawing.Color.Ivory;
            this.txtPhoneMobile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPhoneMobile.Location = new System.Drawing.Point(157, 193);
            this.txtPhoneMobile.Name = "txtPhoneMobile";
            this.txtPhoneMobile.Size = new System.Drawing.Size(235, 22);
            this.txtPhoneMobile.TabIndex = 85;
            // 
            // txtPhoneOffice
            // 
            this.txtPhoneOffice.BackColor = System.Drawing.Color.Ivory;
            this.txtPhoneOffice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPhoneOffice.Location = new System.Drawing.Point(157, 166);
            this.txtPhoneOffice.Name = "txtPhoneOffice";
            this.txtPhoneOffice.Size = new System.Drawing.Size(235, 22);
            this.txtPhoneOffice.TabIndex = 83;
            // 
            // txtPassword2
            // 
            this.txtPassword2.BackColor = System.Drawing.Color.Ivory;
            this.txtPassword2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword2.Location = new System.Drawing.Point(157, 138);
            this.txtPassword2.Name = "txtPassword2";
            this.txtPassword2.PasswordChar = '*';
            this.txtPassword2.Size = new System.Drawing.Size(137, 22);
            this.txtPassword2.TabIndex = 79;
            // 
            // txtPassword1
            // 
            this.txtPassword1.BackColor = System.Drawing.Color.Ivory;
            this.txtPassword1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword1.Location = new System.Drawing.Point(157, 111);
            this.txtPassword1.Name = "txtPassword1";
            this.txtPassword1.PasswordChar = '*';
            this.txtPassword1.Size = new System.Drawing.Size(137, 22);
            this.txtPassword1.TabIndex = 77;
            // 
            // txtUserName
            // 
            this.txtUserName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserName.Location = new System.Drawing.Point(157, 84);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.ReadOnly = true;
            this.txtUserName.Size = new System.Drawing.Size(137, 22);
            this.txtUserName.TabIndex = 76;
            // 
            // txtUserID
            // 
            this.txtUserID.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtUserID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserID.Location = new System.Drawing.Point(157, 57);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.ReadOnly = true;
            this.txtUserID.Size = new System.Drawing.Size(137, 22);
            this.txtUserID.TabIndex = 72;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(251, 13);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(70, 22);
            this.btnSave.TabIndex = 66;
            this.btnSave.TabStop = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(327, 13);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 22);
            this.btnClose.TabIndex = 63;
            this.btnClose.TabStop = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // picLine
            // 
            this.picLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picLine.Image = ((System.Drawing.Image)(resources.GetObject("picLine.Image")));
            this.picLine.Location = new System.Drawing.Point(8, 42);
            this.picLine.Name = "picLine";
            this.picLine.Size = new System.Drawing.Size(391, 5);
            this.picLine.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picLine.TabIndex = 34;
            this.picLine.TabStop = false;
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Image = ((System.Drawing.Image)(resources.GetObject("lblTitle.Image")));
            this.lblTitle.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblTitle.Location = new System.Drawing.Point(12, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(306, 28);
            this.lblTitle.TabIndex = 33;
            this.lblTitle.Text = "     My Information";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmUserEnvironment
            // 
            this.ClientSize = new System.Drawing.Size(406, 374);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUserEnvironment";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " My Information";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmUserEnvironment_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLine)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.PictureBox btnSave;
        private System.Windows.Forms.PictureBox btnClose;
        private System.Windows.Forms.PictureBox picLine;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtPhoneOffice;
        private System.Windows.Forms.TextBox txtPassword2;
        private System.Windows.Forms.TextBox txtPassword1;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtPhoneETC;
        private System.Windows.Forms.TextBox txtPhoneHome;
        private System.Windows.Forms.TextBox txtPhoneMobile;
        private System.Windows.Forms.ListBox lstGroup;
        private System.Windows.Forms.Label lblGroupList;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblPhoneETC;
        private System.Windows.Forms.Label lblPhoneHome;
        private System.Windows.Forms.Label lblPhoneMobile;
        private System.Windows.Forms.Label lblPhoneOffice;
        private System.Windows.Forms.Label lblPassword2;
        private System.Windows.Forms.Label lblPassword1;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblUserID;
    }
}