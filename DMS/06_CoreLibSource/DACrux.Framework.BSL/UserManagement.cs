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
    /// Class Name : UserManagement<br/>
    /// Summary    : User Management Process Business Logic Class<br/>
    /// Author     : YSIM<br/>
    /// First Date : 2009-02-23<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class UserManagement : Miracom.Middleware.BaseComponent, iDACruxUserManagement
    {
        #region DeleteUser

        /// <summary>
        /// Delete Selected User
        /// </summary>
        /// <param name="strUserID">User ID to Delete</param>
        public void DeleteUser(string strUserID)
        {
            TQC_USER oUser = new TQC_USER();

            try
            {
                oUser.DeleteUser(strUserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region ReadUserList

        /// <summary>
        /// Get All User List
        /// </summary>
        /// <returns>Result DataSet</returns>
        public DataTable ReadUserList()
        {
            TQC_USER oUser = new TQC_USER();
            try
            {
                return oUser.ReadAllUserList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CheckAccount (MES Sync)
        public DataTable CheckAccount(
            string strFactory,
            string strUserID,
            string strUserPW,
            bool withMESSync
            )
        {
            TQC_USER oUser = null;
            MSECUSRDEF oMsecusrdef = null;
            TQC_GROUP_USER oGroupUser = null;

            DataTable dtUser = null;
            DataTable dtMESUser = null;

            try
            {
                List<string> lstUpdateValue = new List<string>();

                oUser = new TQC_USER();
                oMsecusrdef = new MSECUSRDEF();

                dtUser = oUser.GetMyInfo(strUserID);
                //dtUser = oUser.CheckAccount(strUserID, strUserPW);
                dtMESUser = oMsecusrdef.GetUserInfo(strFactory, strUserID);

                if (dtMESUser == null || dtMESUser.Rows.Count < 1) // MES에 저장되지 않은 사용자는 접속할 수 없다.
                    return null;

                if (dtUser == null || dtUser.Rows.Count == 0)
                {
                    if (oUser.InsertUser(strUserID
                                    , "USERNAME"
                                    , "PASSWORD"
                                    , "GROUP"
                                    , "PHONE_OFFICE"
                                    , "PHONE_MOBILE"
                                    , "PHONE_HOME"
                                    , "PHONE_OTHER"
                                    , "EMAIL") == false)
                    {
                        throw new Exception(string.Format("Can not find user id [User ID : {0}]", strUserID));
                    }
                    else
                    {
                        oGroupUser = new TQC_GROUP_USER();
                        oGroupUser.InsertGroupUser("G000", strUserID);
                        lstUpdateValue.Add(string.Format("PASSWORD = '{0}'", DACrux.Base.Crypt.GetEncoding("p@ssw0rd"))); // 암호화
                    }
                }

                dtUser = oUser.GetMyInfo(strUserID);
                if (dtUser.Rows[0]["USER_ID"].ToString() != dtMESUser.Rows[0]["USER_ID"].ToString()) lstUpdateValue.Add(string.Format("USER_ID = '{0}'", dtMESUser.Rows[0]["USER_ID"]));
                if (dtUser.Rows[0]["USER_NAME"].ToString() != dtMESUser.Rows[0]["USER_DESC"].ToString()) lstUpdateValue.Add(string.Format("USER_NAME = '{0}'", dtMESUser.Rows[0]["USER_DESC"]));
                if (dtUser.Rows[0]["USER_CMF_1"].ToString() != dtMESUser.Rows[0]["USER_GRP_1"].ToString()) lstUpdateValue.Add(string.Format("USER_CMF_1 = '{0}'", dtMESUser.Rows[0]["USER_GRP_1"]));
                if (dtUser.Rows[0]["USER_CMF_2"].ToString() != dtMESUser.Rows[0]["USER_GRP_2"].ToString()) lstUpdateValue.Add(string.Format("USER_CMF_2 = '{0}'", dtMESUser.Rows[0]["USER_GRP_2"]));
                if (dtUser.Rows[0]["USER_CMF_3"].ToString() != dtMESUser.Rows[0]["USER_GRP_3"].ToString()) lstUpdateValue.Add(string.Format("USER_CMF_3 = '{0}'", dtMESUser.Rows[0]["USER_GRP_3"]));
                if (dtUser.Rows[0]["USER_CMF_4"].ToString() != dtMESUser.Rows[0]["USER_GRP_4"].ToString()) lstUpdateValue.Add(string.Format("USER_CMF_4 = '{0}'", dtMESUser.Rows[0]["USER_GRP_4"]));
                if (dtUser.Rows[0]["USER_CMF_5"].ToString() != dtMESUser.Rows[0]["USER_GRP_5"].ToString()) lstUpdateValue.Add(string.Format("USER_CMF_5 = '{0}'", dtMESUser.Rows[0]["USER_GRP_5"]));
                if (dtUser.Rows[0]["USER_CMF_6"].ToString() != dtMESUser.Rows[0]["USER_GRP_6"].ToString()) lstUpdateValue.Add(string.Format("USER_CMF_6 = '{0}'", dtMESUser.Rows[0]["USER_GRP_6"]));
                if (dtUser.Rows[0]["USER_CMF_7"].ToString() != dtMESUser.Rows[0]["USER_GRP_7"].ToString()) lstUpdateValue.Add(string.Format("USER_CMF_7 = '{0}'", dtMESUser.Rows[0]["USER_GRP_7"]));
                if (dtUser.Rows[0]["USER_CMF_8"].ToString() != dtMESUser.Rows[0]["USER_GRP_8"].ToString()) lstUpdateValue.Add(string.Format("USER_CMF_8 = '{0}'", dtMESUser.Rows[0]["USER_GRP_8"]));
                if (dtUser.Rows[0]["USER_CMF_9"].ToString() != dtMESUser.Rows[0]["USER_GRP_9"].ToString()) lstUpdateValue.Add(string.Format("USER_CMF_9 = '{0}'", dtMESUser.Rows[0]["USER_GRP_9"]));
                if (dtUser.Rows[0]["USER_CMF_10"].ToString() != dtMESUser.Rows[0]["USER_GRP_10"].ToString()) lstUpdateValue.Add(string.Format("USER_CMF_10 = '{0}'", dtMESUser.Rows[0]["USER_GRP_10"]));
                if (dtUser.Rows[0]["SEC_GRP_ID"].ToString() != dtMESUser.Rows[0]["SEC_GRP_ID"].ToString()) lstUpdateValue.Add(string.Format("SEC_GRP_ID = '{0}'", dtMESUser.Rows[0]["SEC_GRP_ID"]));
                if (dtUser.Rows[0]["PHONE_OFFICE"].ToString() != dtMESUser.Rows[0]["PHONE_OFFICE"].ToString()) lstUpdateValue.Add(string.Format("PHONE_OFFICE = '{0}'", dtMESUser.Rows[0]["PHONE_OFFICE"]));
                if (dtUser.Rows[0]["PHONE_MOBILE"].ToString() != dtMESUser.Rows[0]["PHONE_MOBILE"].ToString()) lstUpdateValue.Add(string.Format("PHONE_MOBILE = '{0}'", dtMESUser.Rows[0]["PHONE_MOBILE"]));
                if (dtUser.Rows[0]["PHONE_HOME"].ToString() != dtMESUser.Rows[0]["PHONE_HOME"].ToString()) lstUpdateValue.Add(string.Format("PHONE_HOME = '{0}'", dtMESUser.Rows[0]["PHONE_HOME"]));
                if (dtUser.Rows[0]["PHONE_OTHER"].ToString() != dtMESUser.Rows[0]["PHONE_OTHER"].ToString()) lstUpdateValue.Add(string.Format("PHONE_OTHER = '{0}'", dtMESUser.Rows[0]["PHONE_OTHER"]));
                if (dtUser.Rows[0]["EMAIL_ID"].ToString() != dtMESUser.Rows[0]["EMAIL_ID"].ToString()) lstUpdateValue.Add(string.Format("EMAIL_ID = '{0}'", dtMESUser.Rows[0]["EMAIL_ID"]));
                //if (dtUser.Rows[0]["USER_ID"].ToString() != dtMESUser.Rows[0]["USER_ID"].ToString()) lstUpdateValue.Add(string.Format("USER_ID = '{0}'", dtMESUser.Rows[0]["USER_ID"].ToString()));
                //if (dtUser.Rows[0]["USER_NAME"].ToString() != dtMESUser.Rows[0]["USER_DESC"].ToString()) lstUpdateValue.Add(string.Format("USER_NAME = '{0}'", dtMESUser.Rows[0]["USER_DESC"].ToString()));
                //if (dtUser.Rows[0]["USER_CMF_1"].ToString() != dtMESUser.Rows[0]["USER_GRP_1"].ToString()) lstUpdateValue.Add(string.Format("USER_CMF_1 = '{0}'", dtMESUser.Rows[0]["USER_GRP_1"].ToString()));
                //if (dtUser.Rows[0]["USER_CMF_2"].ToString() != dtMESUser.Rows[0]["USER_GRP_2"].ToString()) lstUpdateValue.Add(string.Format("USER_CMF_2 = '{0}'", dtMESUser.Rows[0]["USER_GRP_2"].ToString()));
                //if (dtUser.Rows[0]["USER_CMF_3"].ToString() != dtMESUser.Rows[0]["USER_GRP_3"].ToString()) lstUpdateValue.Add(string.Format("USER_CMF_3 = '{0}'", dtMESUser.Rows[0]["USER_GRP_3"].ToString()));
                //if (dtUser.Rows[0]["USER_CMF_4"].ToString() != dtMESUser.Rows[0]["USER_GRP_4"].ToString()) lstUpdateValue.Add(string.Format("USER_CMF_4 = '{0}'", dtMESUser.Rows[0]["USER_GRP_4"].ToString()));
                //if (dtUser.Rows[0]["USER_CMF_5"].ToString() != dtMESUser.Rows[0]["USER_GRP_5"].ToString()) lstUpdateValue.Add(string.Format("USER_CMF_5 = '{0}'", dtMESUser.Rows[0]["USER_GRP_5"].ToString()));
                //if (dtUser.Rows[0]["USER_CMF_6"].ToString() != dtMESUser.Rows[0]["USER_GRP_6"].ToString()) lstUpdateValue.Add(string.Format("USER_CMF_6 = '{0}'", dtMESUser.Rows[0]["USER_GRP_6"].ToString()));
                //if (dtUser.Rows[0]["USER_CMF_7"].ToString() != dtMESUser.Rows[0]["USER_GRP_7"].ToString()) lstUpdateValue.Add(string.Format("USER_CMF_7 = '{0}'", dtMESUser.Rows[0]["USER_GRP_7"].ToString()));
                //if (dtUser.Rows[0]["USER_CMF_8"].ToString() != dtMESUser.Rows[0]["USER_GRP_8"].ToString()) lstUpdateValue.Add(string.Format("USER_CMF_8 = '{0}'", dtMESUser.Rows[0]["USER_GRP_8"].ToString()));
                //if (dtUser.Rows[0]["USER_CMF_9"].ToString() != dtMESUser.Rows[0]["USER_GRP_9"].ToString()) lstUpdateValue.Add(string.Format("USER_CMF_9 = '{0}'", dtMESUser.Rows[0]["USER_GRP_9"].ToString()));
                //if (dtUser.Rows[0]["USER_CMF_10"].ToString() != dtMESUser.Rows[0]["USER_GRP_10"].ToString()) lstUpdateValue.Add(string.Format("USER_CMF_10 = '{0}'", dtMESUser.Rows[0]["USER_GRP_10"].ToString()));
                //if (dtUser.Rows[0]["SEC_GRP_ID"].ToString() != dtMESUser.Rows[0]["SEC_GRP_ID"].ToString()) lstUpdateValue.Add(string.Format("SEC_GRP_ID = '{0}'", dtMESUser.Rows[0]["SEC_GRP_ID"].ToString()));
                //if (dtUser.Rows[0]["PHONE_OFFICE"].ToString() != dtMESUser.Rows[0]["PHONE_OFFICE"].ToString()) lstUpdateValue.Add(string.Format("PHONE_OFFICE = '{0}'", dtMESUser.Rows[0]["PHONE_OFFICE"].ToString()));
                //if (dtUser.Rows[0]["PHONE_MOBILE"].ToString() != dtMESUser.Rows[0]["PHONE_MOBILE"].ToString()) lstUpdateValue.Add(string.Format("PHONE_MOBILE = '{0}'", dtMESUser.Rows[0]["PHONE_MOBILE"].ToString()));
                //if (dtUser.Rows[0]["PHONE_HOME"].ToString() != dtMESUser.Rows[0]["PHONE_HOME"].ToString()) lstUpdateValue.Add(string.Format("PHONE_HOME = '{0}'", dtMESUser.Rows[0]["PHONE_HOME"].ToString()));
                //if (dtUser.Rows[0]["PHONE_OTHER"].ToString() != dtMESUser.Rows[0]["PHONE_OTHER"].ToString()) lstUpdateValue.Add(string.Format("PHONE_OTHER = '{0}'", dtMESUser.Rows[0]["PHONE_OTHER"].ToString()));
                //if (dtUser.Rows[0]["EMAIL_ID"].ToString() != dtMESUser.Rows[0]["EMAIL_ID"].ToString()) lstUpdateValue.Add(string.Format("EMAIL_ID = '{0}'", dtMESUser.Rows[0]["EMAIL_ID"].ToString()));

                if (lstUpdateValue.Count > 0)
                {
                    oUser.UpdateUser(lstUpdateValue.ToArray(), strUserID);
                }

                dtUser = oUser.CheckAccount(strUserID, strUserPW);

                return dtUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dtUser != null) dtUser.Dispose();
                dtUser = null;
                if (dtMESUser != null) dtMESUser.Dispose();
                dtMESUser = null;
            }
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
            TQC_USER oUser = new TQC_USER();
            object obj = oUser.CheckID(strUserID);
            if(obj == null || obj == DBNull.Value)
                return false;
            
            return (Int32.Parse(obj.ToString()) > 0);
        }

        #endregion

        #region CheckAccount

        /// <summary>
        /// Check User Account
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="userPw">User Password</param>
        /// <returns>Result DataSet</returns>
        public DataTable CheckAccount(
            string userId, 
            string userPw
            )
        {
            TQC_USER oUser = new TQC_USER();
            return oUser.CheckAccount(
                userId, 
                userPw
                );
        }

        public string GetUserName(string userID)
        {
            TQC_USER obj = new TQC_USER();
            return obj.GetUserName(userID);
        }

        #endregion

        #region UpdateUser

        /// <summary>
        /// Update User Information
        /// </summary>
        /// <param name="strSQL">SQL</param>
        /// <param name="strUserID">User ID</param>
        /// <returns>Success Bool</returns>
        public bool UpdateUser(string strSQL, string strUserID)
        {
            bool bResult;
            TQC_USER oUser = null;

            try
            {
                oUser = new TQC_USER();

                bResult = oUser.UpdateUser(strSQL, strUserID);
                return bResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oUser != null) oUser.Dispose();
                oUser = null;
            }
        }

        #endregion

        public bool GetLicense(string IPAddress)
        {
            License oLicense = new License();
            return oLicense.LicenseCheck(IPAddress);
        }

    }
}
