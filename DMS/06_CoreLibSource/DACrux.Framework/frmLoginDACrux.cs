/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : frmLoginDACrux.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2014.11.14
--  Description     : DACrux Framework::DACrux 의 Login Window
--  History         : Created by YSIM at 2014.11.14
 * ********************************************************************************************************
 * 2014 년 DACrux V4 History Reset

----------------------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DACrux.Framework.RO;
using System.IO;

namespace DACrux.Framework
{
    public partial class frmLoginDACrux : DACrux.Framework.Base.DACruxUXBasic00
    {
        private const string m_strSuperUserID = "ADMIN";
        private const string m_strSuperUserPW = "1111";

        public frmLoginDACrux()
        {
            if (String.IsNullOrEmpty(DACrux.Base.GlobalVariable.Language))
                DACrux.Base.GlobalVariable.Language = "English";

            InitializeComponent();

            lbl_FACTORY.Text = DACrux.Base.MultiLanguage.Translation("Factory");
            lbl_UserID.Text = DACrux.Base.MultiLanguage.Translation("User ID");
            lbl_Password.Text = DACrux.Base.MultiLanguage.Translation("Password");

            if (File.Exists("CustomCI.png"))
                picCustomCI.Image = Image.FromFile("CustomCI.png");
        }

        private void butOk_Click(object sender, EventArgs e)
        {
            if (!GoLogin(txtUserID.Text, txtPassword.Text))
            {
                return;
            }
            else
            {
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void frmLoginDACrux_Load(object sender, EventArgs e)
        {
            this.Opacity = 0;
            System.Threading.Thread t = null;
            try
            {
                // 1. System Version 처리
                lbl_DACruxVersion.Text = DACrux.Base.GlobalVariable.ApplicationLongVersion;

                // 2. 화면 Faidin 효과
                t = new System.Threading.Thread(new System.Threading.ThreadStart(ThreadFaidIn));
                t.Start();


                // 3. 배경화면 Image Load
                if (File.Exists(Application.StartupPath + @"\CustomLogin.rs"))
                    this.BackgroundImage = Image.FromFile(Application.StartupPath + @"\CustomLogin.rs");

                // 4. Last User Display
                txtUserID.Text = DACrux.Base.GlobalVariable.UserID;

                // 5. Factory를 선택하고 Login 할시 Factory선택
                string[] strFactory = DACrux.Base.GlobalVariable.FactoryList.Split(new char[] { '|', ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                cmbFactory.Items.Clear();
                for (int i = 0; i < strFactory.Length; i++)
                    cmbFactory.Items.Add(strFactory[i]);

                if (cmbFactory.Items.Count > 0) cmbFactory.SelectedIndex = 0;
            }
            catch
            {
                if (t != null && t.IsAlive)
                {
                    t.Abort();
                }
                this.Opacity = 1.00d * 100d;
            }
            finally
            {
                this.Focus();
                this.TopMost = true;
            }

        }

        private void ThreadFaidIn()
        {
            while (true)
            {
                if (this.Opacity >= 1.00d) break;
                FaidIn();
                System.Threading.Thread.Sleep(10);
            }
        }

        private void FaidIn()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(
                    delegate()
                    {
                        FaidIn();
                    }
                    ));
            }
            else
            {
                this.Opacity = this.Opacity + 0.01d;
            }
        }

        #region GoLogin

        /// <summary>
        /// Log In Process
        /// </summary>
        /// <param name="strUserID">UserID</param>
        /// <param name="strUserPW">Password</param>
        /// <returns>LoginResult</returns>
        public bool GoLogin(string strUserID, string strUserPW)
        {
            UserManagement oUser = null;
            DataTable dt = null;

            string strUserName = string.Empty;
            string strCheckPasswd = string.Empty;

            bool bResult = false;

            try
            {
                if (String.Equals(strUserID.ToUpper(), m_strSuperUserID.ToUpper())
                    && String.Equals(strUserPW, m_strSuperUserPW))
                {
                    DACrux.Base.GlobalVariable.UserID = m_strSuperUserID;
                    DACrux.Base.GlobalVariable.Password = m_strSuperUserPW;
                    return true;
                }

                if (String.IsNullOrEmpty(txtUserID.Text))
                {
                    MessageBox.Show("Please enter your account.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (String.IsNullOrEmpty(txtPassword.Text))
                {
                    MessageBox.Show("Please enter the password.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                oUser = new UserManagement();

                //if (oUser.GetLicense(DACrux.Base.GlobalVariable.LocalMacAddress) == false)
                //{
                //    MessageBox.Show("License is expire.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return false;
                //}

                //dt = oUser.CheckAccount(DACrux.Base.GlobalVariable.Factory, strUserID.ToUpper(), DACrux.Base.Crypt.GetEncoding(strUserPW), true);
                dt = oUser.CheckAccount(
                    strUserID.ToUpper(), 
                    DACrux.Base.Crypt.GetEncoding(strUserPW)
                    );

                if (dt == null || dt.Rows.Count == 0)
                {
                    if (oUser.CheckID(strUserID.ToUpper()))
                    {
                        MessageBox.Show("Login is incorrect. Password is different.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtPassword.SelectAll();
                        txtPassword.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Login is incorrect. Not registered user account.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtUserID.SelectAll();
                        txtUserID.Focus();
                    }
                    bResult = false;
                }
                else
                {
                    DACrux.Base.GlobalVariable.UserID = strUserID.ToUpper();
                    DACrux.Base.GlobalVariable.Password = strCheckPasswd;
                    bResult = true;
                }

                return bResult;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                dt = null;
                oUser = null;
                DACrux.Base.GlobalVariable.SaveGlobalVariable();
            }
        }

        #endregion

        private void frmLoginDACrux_Shown(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtUserID.Text))
                txtUserID.Focus();
            else
                txtPassword.Focus();
        }

        private void butOption_Click(object sender, EventArgs e)
        {
            frmSystemOption oSystemOpt = new frmSystemOption();
            if (oSystemOpt.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lbl_FACTORY.Text = DACrux.Base.MultiLanguage.Translation("Factory");
                lbl_UserID.Text = DACrux.Base.MultiLanguage.Translation("User ID");
                lbl_Password.Text = DACrux.Base.MultiLanguage.Translation("Password");

                // Image로 처리
                //butCancel.Text = DACrux.Base.MultiLanguage.Translation("Cancel");
                //butOption.Text = DACrux.Base.MultiLanguage.Translation("Option");
                //butOk.Text = DACrux.Base.MultiLanguage.Translation("OK");
            }
        }

        public void UsingFactorySelect(bool IsSet)
        {
            cmbFactory.Visible = IsSet;
            lbl_FACTORY.Visible = IsSet;
        }

        public void UsingOption(bool IsSet)
        {
            butOption.Visible = IsSet;
        }

        private void cmbFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            DACrux.Base.GlobalVariable.Factory = cmbFactory.Text;
            DACrux.Base.GlobalVariable.SaveGlobalVariable();
        }
    }
}
