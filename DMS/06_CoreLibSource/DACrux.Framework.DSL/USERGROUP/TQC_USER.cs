using System;
using System.Data;

namespace DACrux.Framework.DSL
{
    /// <summary>
    /// Class Name : TQC_USER<br/>
    /// Summary    : Access for TQ_USER Table<br/>
    /// Author     : YSIM<br/>
    /// First Date : 2009-02-23<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class TQC_USER : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public TQC_USER()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["QMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQC_USER.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region ReadAllUserList
        /// <summary>
        /// Receiving of all user information
        /// </summary>
        public DataTable ReadAllUserList()
        {
            try
            {
                return this.GetDataTable("READ_ALLUSERLIST", null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetMyInfo

        /// <summary>
        /// Get My Information
        /// </summary>
        /// <param name="strUserID">User ID</param>
        /// <returns>Result DataTable</returns>
        public DataTable GetMyInfo(string strUserID)
        {
            DataTable dt;
            try
            {
                dt = this.GetDataTable("GETMYINFO", null, new string[] { strUserID });
                return dt;
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
                this.Execute("UPDATEMYINFO", null, new string[] { strUserID, strPasswd, strPhoneOffice, strPhoneMobile, strPhoneHome, strPhoneEtc, strEmail });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region LoadSecurityUser
        /// <summary>
        /// Load Security User
        /// </summary>
        /// <returns></returns>
        public DataTable LoadSecurityUser()
        {
            DataTable dt;
            try
            {
                dt = this.GetDataTable("LOADSECURITYUSER", null, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CountByDepartment

        /// <summary>
        /// Get User Count By Department
        /// </summary>
        /// <returns></returns>
        public DataTable CountByDepartment()
        {
            DataTable dt;
            try
            {
                dt = this.GetDataTable("COUNTBYDEPARTMENT", null, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region LoadJoinUserName2

        /// <summary>
        /// Get User List
        /// </summary>
        /// <returns></returns>
        public DataTable LoadJoinUserName2()
        {
            DataTable dt;
            try
            {
                dt = this.GetDataTable("LOADJOINUSERNAME2", null, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DeleteUser

        /// <summary>
        /// Delete Selected User
        /// </summary>
        /// <param name="strUserID">User ID</param>
        /// <returns></returns>
        public bool DeleteUser(
            string strID
            )
        {
            int iResult = this.ExecuteNonQuery("DELETEUSER", null, new string[] { strID });
            if (iResult <= 0)
                return false;
            else
                return true;
        }

        #endregion

        #region InsertUser

        /// <summary>
        /// Insert New User
        /// </summary>
        /// <param name="strUserID">User ID</param>
        /// <param name="strUserName">User Name</param>
        /// <param name="strPass">User Password</param>
        /// <param name="strGrp">Department</param>
        /// <param name="strPnOffice">Office No</param>
        /// <param name="strPnMobile">Mobile No</param>
        /// <param name="strPnHome">Home No</param>
        /// <param name="strPnOther">ETC No</param>
        /// <param name="strEmail">E Mail Address</param>
        /// <returns>Success Bool</returns>
        public bool InsertUser(
            string strUserID,
            string strUserName,
            string strPass,
            string strGrp,
            string strPnOffice,
            string strPnMobile,
            string strPnHome,
            string strPnOther,
            string strEmail
            )
        {
            int iResult = this.ExecuteNonQuery(
                "INSERTUSER",
                null,
                new string[] { 
                    strUserID, 
                    strUserName, 
                    strPass, 
                    strGrp, 
                    strPnOffice, 
                    strPnMobile, 
                    strPnHome, 
                    strPnOther, 
                    strEmail 
                });

            if (iResult <= 0)
                return false;
            else
                return true;
        }

        #endregion

        #region CheckID

        /// <summary>
        /// Get Count for User ID
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>ResultDataSet</returns>
        public object CheckID(string userID)
        {
            return this.ExecuteScalar(
                "CHECKID",
                null,
                new string[] { userID }
                );
        }

        #endregion

        #region CheckAccount

        /// <summary>
        /// Log in Check
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="userPw">Password</param>
        /// <returns>ResultDataSet</returns>
        public DataTable CheckAccount(
            string userId,
            string userPw
            )
        {
            return this.GetDataTable(
                "CHECKACCOUNT",
                null,
                new string[] { userId, userPw }
                );
        }

        public string GetUserName(string userID)
        {
            object obj = ExecuteScalar("SELECT_USER_NAME", null, new string[] { userID });
            return (string)obj;
        }

        #endregion

        #region UpdateUser

        /// <summary>
        /// Update User Information
        /// </summary>
        /// <param name="strSQL">SQL</param>
        /// <param name="strUserID">User ID</param>
        /// <returns></returns>
        public bool UpdateUser(
            string strSQL, 
            string strUserID
            )
        {
            int iResult = this.ExecuteNonQuery(
                "UPDATEUSER",
                new string[] { strSQL },
                new string[] { strUserID }
                );
            if (iResult <= 0)
                return false;
            else
                return true;
        }

        #endregion


        public void UpdateUser(
            string[] UpdateValue,
            string strUserID
            )
        {
            string strDynamic = string.Join(",", UpdateValue);
            this.ExecuteNonQuery("UPDATE_USER_INFO", new string[] { strDynamic }, new string[] { strUserID });
        }
    }
}
