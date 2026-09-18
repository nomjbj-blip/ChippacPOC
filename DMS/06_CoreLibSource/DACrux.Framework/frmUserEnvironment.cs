using System;
using System.Data;
using System.Windows.Forms;
using DACrux.Base;

using DACrux.Framework.RO;

namespace DACrux.Framework
{
    /// <summary>
    /// Class Name : frmUserEnvironment<br/>
    /// Summary    : My Information Management WinForm Class<br/>
    /// Author     : YSIM<br/>
    /// First Date : 2009-02-24<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public partial class frmUserEnvironment :  DACrux.Framework.Base.DACruxUXBasic00
    {

        #region Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public frmUserEnvironment()
        {
            InitializeComponent();
        }

        private void frmUserEnvironment_Load(object sender, EventArgs e)
        {
            try
            {
                GetMyInfo();
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
        }

        #endregion

        #region GetMyInfo

        /// <summary>
        /// Get Current User Information
        /// </summary>
        private void GetMyInfo()
        {
            UserEnvironment oUser = null;
            DataTable dt = null;

            try
            {
                oUser = new UserEnvironment();
                dt = oUser.GetMyInfo(DACrux.Base.GlobalVariable.UserID.ToUpper().Trim());

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Your information is not founded.");
                }
                else
                {
                    #region User Information

                    txtUserID.Text = dt.Rows[0]["USER_ID"].ToString();
                    txtUserName.Text = dt.Rows[0]["USER_NAME"].ToString();

                    txtPassword1.Text = DACrux.Base.Crypt.GetDecoding(dt.Rows[0]["PASSWORD"].ToString());
                    txtPassword2.Text = DACrux.Base.Crypt.GetDecoding(dt.Rows[0]["PASSWORD"].ToString());

                    txtPhoneOffice.Text = dt.Rows[0]["PHONE_OFFICE"].ToString();
                    txtPhoneMobile.Text = dt.Rows[0]["PHONE_MOBILE"].ToString();

                    if (dt.Rows[0]["PHONE_HOME"] == DBNull.Value)
                        txtPhoneHome.Text = string.Empty;
                    else
                        txtPhoneHome.Text = dt.Rows[0]["PHONE_HOME"].ToString();


                    if (dt.Rows[0]["PHONE_OTHER"] == DBNull.Value)
                        txtPhoneETC.Text = string.Empty;
                    else
                        txtPhoneETC.Text = dt.Rows[0]["PHONE_OTHER"].ToString();

                    txtEmail.Text = dt.Rows[0]["EMAIL_ID"].ToString();

                    #endregion

                    #region User Group List

                    if (dt != null) dt.Dispose();
                    dt = null;

                    dt = oUser.GetUserGroup(DACrux.Base.GlobalVariable.UserID.ToUpper().Trim());

                    lstGroup.ValueMember = "SEC_GRP_ID";
                    lstGroup.DisplayMember = "SEC_GRP_DESC";
                    lstGroup.DataSource = dt;

                    #endregion
                }
            }
            catch (Exception ex)
            {
                DspError(ex);
            }
            finally
            {
                oUser = null;
            }
        }

        #endregion

        #region Button Click Event

        #region btnSave_Click

        /// <summary>
        /// Save User Information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            DACrux.Framework.RO.UserEnvironment oUser = null;

            try
            {
                oUser = new DACrux.Framework.RO.UserEnvironment();
                #region Check Field

                if (txtPassword1.Text.Length < 4)
                {
                    MessageBox.Show("Password is too short. It should be longer than 3 digits.");
                    txtPassword1.Focus();
                    return;
                }

                if (txtPassword1.Text != txtPassword2.Text)
                {
                    MessageBox.Show("Password is incorrect. Please Confirm the password");
                    txtPassword2.Focus();
                    return;
                }

                if (txtPhoneOffice.Text.Length < 1)
                {
                    MessageBox.Show("Office phone number field is empty.");
                    txtPhoneOffice.Focus();
                    return;
                }

                if (txtPhoneMobile.Text.Length < 1)
                {
                    MessageBox.Show("Mobile phone number field is empty.");
                    txtPhoneMobile.Focus();
                    return;
                }

                if (txtEmail.Text.Length < 5)
                {
                    MessageBox.Show("E Mail field is empty.");
                    txtEmail.Focus();
                    return;
                }

                #endregion

                oUser.UpdateMyInfo(DACrux.Base.GlobalVariable.UserID, DACrux.Base.Crypt.GetEncoding(txtPassword1.Text), txtPhoneOffice.Text
                , txtPhoneMobile.Text, txtPhoneHome.Text, txtPhoneETC.Text, txtEmail.Text);

                MessageBox.Show("Your information has saved successfuly");
                DialogResult = DialogResult.OK;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oUser = null;
            }
        }

        #endregion

        #region btnClose_Click

        /// <summary>
        /// Exit Function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        #endregion

        #endregion
    }
}