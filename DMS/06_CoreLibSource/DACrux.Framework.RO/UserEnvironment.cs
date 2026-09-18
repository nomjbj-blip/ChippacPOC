using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DACrux.Base;
using DACrux.Framework.Interface;

namespace DACrux.Framework.RO
{
    /// <summary>
    /// Class Name : UserEnvironment<br/>
    /// Summary    : User Environment Remoting Object Class<br/>
    /// Author     : YSIM<br/>
    /// First Date : 2009-02-23<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class UserEnvironment
    {
        #region Class Member

        DACrux.Framework.Interface.iDACruxUserEnviroment m_OBJ = null;

        #endregion

        #region Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public UserEnvironment()
        {
            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_QMS_COMMON);
            object obj = Activator.GetObject(typeof(DACrux.Framework.Interface.iDACruxUserEnviroment),
                strUrl + "/DACrux.Framework.BSL.UserEnvironment.bin");
            m_OBJ = obj as DACrux.Framework.Interface.iDACruxUserEnviroment;
            System.Configuration.ConfigurationManager.GetSection("System.Diagnostics");
        }

        #endregion

        #region GetMyInfo

        /// <summary>
        /// Get My Information
        /// </summary>
        /// <param name="strUserID">User ID</param>
        /// <returns>Result DataSet</returns>
        public DataTable GetMyInfo(string strUserID)
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetMyInfo(strUserID);
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
            try
            {
                return m_OBJ.GetUserGroup(strUserID);
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
            try
            {
                m_OBJ.UpdateMyInfo(strUserID, strPasswd, strPhoneOffice, strPhoneMobile, strPhoneHome, strPhoneEtc, strEmail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
