#define WINDOWS2008
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.InteropServices;
using DACrux.Base;
using DACrux.Framework.Interface;
using DACrux.Framework.DSL;
using System.Transactions;

namespace DACrux.Framework.BSL
{
    /// <summary>
    /// Class Name : UserEnvironment<br/>
    /// Summary    : User Environment Business Logic Class<br/>
    /// Author     : YSIM<br/>
    /// First Date : 2009-02-23<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class UserEnvironment : Miracom.Middleware.BaseComponent, iDACruxUserEnviroment
    {
        #region GetMyInfo

        /// <summary>
        /// Get My Information
        /// </summary>
        /// <param name="strUserID">User ID</param>
        /// <returns>Result DataSet</returns>
        public DataTable GetMyInfo(string strUserID)
        {
            DataTable dt = null;
            TQC_USER oUser = null;

            try
            {
                oUser = new TQC_USER();
                dt = oUser.GetMyInfo(strUserID);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
                if (oUser != null) oUser.Dispose();
                oUser = null;
            }
        }

        #endregion

        #region GetUserGroup

        /// <summary>
        /// Get My Security Group List
        /// </summary>
        /// <param name="strUserID">User ID</param>
        /// <returns>Result DataSet</returns>
        public DataTable GetUserGroup(string strUserID)
        {
            TQC_SEC_GROUP oGroup = null;

            try
            {
                oGroup = new TQC_SEC_GROUP();
                return oGroup.GetUserGroup(strUserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region UpdateMyInfo

        /// <summary>
        /// Update My Information
        /// </summary>
        /// <param name="strUserID">User ID</param>
        /// <param name="strPasswd">Password</param>
        /// <param name="strPhoneOffice">Office No</param>
        /// <param name="strPhoneMobile">Mobile No</param>
        /// <param name="strPhoneHome">Home No</param>
        /// <param name="strPhoneEtc">Etc No</param>
        /// <param name="strEmail">E Mail Address</param>
        public void UpdateMyInfo(string strUserID, string strPasswd, string strPhoneOffice, string strPhoneMobile, string strPhoneHome, string strPhoneEtc, string strEmail)
        {
            TQC_USER oUser = null;

            try
            {
                oUser = new TQC_USER();

                oUser.UpdateMyInfo(strUserID, strPasswd, strPhoneOffice, strPhoneMobile, strPhoneHome, strPhoneEtc, strEmail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oUser != null) oUser.Dispose();
            }
        }

        #endregion
    }
}
