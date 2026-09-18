using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DACrux.Base;
using DACrux.Framework.Interface;

namespace DACrux.Framework.RO
{
    /// <summary>
    /// Class Name : UserManagement<br/>
    /// Summary    : User Management Remoting Object Class<br/>
    /// Author     : YSIM<br/>
    /// First Date : 2009-02-23<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class UserManagement
    {
        #region Class Member

        DACrux.Framework.Interface.iDACruxUserManagement m_OBJ = null;

        #endregion

        #region Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public UserManagement()
        {
            m_OBJ = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_QMS_COMMON
                , "DACrux.Framework.BSL.UserManagement.bin"
                , typeof(DACrux.Framework.Interface.iDACruxUserManagement)) as DACrux.Framework.Interface.iDACruxUserManagement;
        }

        #endregion

        #region DeleteUser

        /// <summary>
        /// Delete Selected User
        /// </summary>
        /// <param name="strUserID">User ID to Delete</param>
        public void DeleteUser(string strUserID)
        {
            m_OBJ.DeleteUser(strUserID);
        }

        #endregion

        #region ReadUserList

        /// <summary>
        /// Get All User List
        /// </summary>
        /// <returns>Result DataSet</returns>
        public DataTable ReadUserList()
        {
            return m_OBJ.ReadUserList();
        }

        #endregion

        #region CheckID

        /// <summary>
        /// Get Count for User ID
        /// </summary>
        /// <param name="strUserID">User ID</param>
        /// <returns>ResultDataSet</returns>
        public bool CheckID(string strUserID)
        {
            return m_OBJ.CheckID(strUserID);
        }

        #endregion

        #region CheckAccount

        /// <summary>
        /// Check User Account
        /// </summary>
        /// <param name="strUserID">User ID</param>
        /// <param name="strUserPW">User Password</param>
        /// <returns>Result DataSet</returns>
        public DataTable CheckAccount(
            string strUserID,
            string strUserPW
            )
        {
            return m_OBJ.CheckAccount(strUserID, strUserPW);
        }

        public string GetUserName(string userID)
        {
            return m_OBJ.GetUserName(userID);
        }

        #endregion

        #region CheckAccount with MES Sync

        /// <summary>
        /// Check User Account
        /// </summary>
        /// <param name="strFactory">Factory</param>
        /// <param name="strUserID">User ID</param>
        /// <param name="strUserPW">User Password</param>
        /// <returns>Result DataSet</returns>
        public DataTable CheckAccount(string strFactory, string strUserID, string strUserPW, bool withMESSync)
        {
            return m_OBJ.CheckAccount(strFactory, strUserID, strUserPW, withMESSync);
        }

        #endregion

        #region UpdateUser

        /// <summary>
        /// Update User Information
        /// </summary>
        /// <param name="strSQL">SQL</param>
        /// <param name="strUserID">User ID</param>
        /// <returns>Success bool</returns>
        public bool UpdateUser(string strSQL, string strUserID)
        {
            return m_OBJ.UpdateUser(strSQL, strUserID);
        }

        #endregion

        #region CheckLicense

        /// <summary>
        /// Check License
        /// </summary>
        /// <param name="IPAddress">Client IP or Client Mac Address</param>
        /// <returns>Success bool</returns>
        public bool GetLicense(string IPAddress)
        {
            return m_OBJ.GetLicense(IPAddress);
        }

        #endregion
    }
}
