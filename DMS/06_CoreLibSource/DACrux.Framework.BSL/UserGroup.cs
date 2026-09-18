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
    /// Class Name : UserGroup<br/>
    /// Summary    : User Group Process Business Logic Class<br/>
    /// Author     : YSIM<br/>
    /// First Date : 2009-02-23<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class UserGroup : Miracom.Middleware.BaseComponent, iDACruxUserGroup
    {
        #region LoadGroupList

        /// <summary>
        /// Get Group List
        /// </summary>
        /// <param name="strGrpCode">Group Code</param>
        /// <returns>Result DataSet</returns>
        public DataTable LoadGroupList(string strGrpCode)
        {
            TQC_SEC_GROUP oGroup = null;
            try
            {
                oGroup = new TQC_SEC_GROUP();
                return oGroup.LoadGroupList(strGrpCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region LoadGroupInfo

        /// <summary>
        /// Get Group Information
        /// </summary>
        /// <param name="strSelectedGroup">Group Code</param>
        /// <returns>Result DataSet</returns>
        public DataTable LoadGroupInfo(string strSelectedGroup)
        {
            TQC_SEC_GROUP oGroup = null;
            try
            {
                oGroup = new TQC_SEC_GROUP();
                return oGroup.LoadGroupInfo(strSelectedGroup);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region LoadGroupFunction

        /// <summary>
        /// Get Function List for Selected Group Code
        /// </summary>
        /// <param name="strSelectedGroup"></param>
        /// <returns>Result DataSet</returns>
        public DataTable LoadGroupFunction(string strSelectedGroup)
        {
            TQC_FUNCTION_LIST oFunctionList = null;
            try
            {
                oFunctionList = new TQC_FUNCTION_LIST();
                return oFunctionList.LoadGroupFunction(strSelectedGroup);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region LoadOutFunction

        /// <summary>
        /// Missing Function List for Selected Group Code
        /// </summary>
        /// <param name="strSelectedGroup">Selected Group Code</param>
        /// <returns>Result DataSet</returns>
        public DataTable LoadOutFunction(string strSelectedGroup)
        {
            TQC_FUNCTION_LIST oFunctionList = null;
            try
            {
                oFunctionList = new TQC_FUNCTION_LIST();
                return oFunctionList.LoadOutFunction(strSelectedGroup);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region LoadMaxGroup

        /// <summary>
        /// Get Max Group Code to making a new Group Code
        /// </summary>
        /// <returns>Result DataSet</returns>
        public DataTable LoadMaxGroup()
        {
            TQC_SEC_GROUP oGroup = null;

            try
            {
                oGroup = new TQC_SEC_GROUP();
                return oGroup.LoadMaxGroup();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region LoadAllFunction

        /// <summary>
        /// All Function List
        /// </summary>
        /// <returns></returns>
        public DataTable LoadAllFunction()
        {
            TQC_FUNCTION_LIST oFunction = null;

            try
            {
                oFunction = new TQC_FUNCTION_LIST();
                return oFunction.LoadAllFunction();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region InsertGroupInfo

        /// <summary>
        /// Insert New Group Information
        /// </summary>
        /// <param name="strGrpCode">Group Code</param>
        /// <param name="strGrpName">Group Name</param>
        /// <param name="strGrpCaption">Group Caption</param>
        /// <returns>Success Bool</returns>
        public bool InsertGroupInfo(string strGrpCode, string strGrpName, string strGrpCaption)
        {
            bool bResult;
            TQC_SEC_GROUP oGroup = null;

            try
            {
                oGroup = new TQC_SEC_GROUP();

                bResult = oGroup.InsertGroupInfo(strGrpCode, strGrpName, strGrpCaption);
                return bResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oGroup != null) oGroup.Dispose();
                oGroup = null;
            }
        }

        #endregion

        #region InsertGroupFunction

        /// <summary>
        /// Insert Group Function
        /// </summary>
        /// <param name="strGrpCode">Group Code</param>
        /// <param name="strFunCode">Function Code</param>
        /// <returns>Success Code</returns>
        public bool InsertGroupFunction(string strGrpCode, string strFunCode)
        {
            bool bResult;
            TQC_GROUP_FUNCTION oGroup = null;

            try
            {
                oGroup = new TQC_GROUP_FUNCTION();

                bResult = oGroup.InsertGroupFunction(strGrpCode, strFunCode);
                return bResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oGroup != null) oGroup.Dispose();
                oGroup = null;
            }
        }

        #endregion

        #region UpdateGroupInfo

        /// <summary>
        /// Update Group Information
        /// </summary>
        /// <param name="strGrpCode">Group Code to Edit</param>
        /// <param name="strGrpName">Group Name</param>
        /// <param name="strGrpCaption">Group Caption</param>
        /// <returns>Success Bool</returns>
        public bool UpdateGroupInfo(string strGrpCode, string strGrpName, string strGrpCaption)
        {
            bool bResult;
            TQC_SEC_GROUP oGroup = null;

            try
            {
                oGroup = new TQC_SEC_GROUP();

                bResult = oGroup.UpdateGroupInfo(strGrpCode, strGrpName, strGrpCaption);
                return bResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oGroup != null) oGroup.Dispose();
                oGroup = null;
            }
        }

        #endregion

        #region DeleteGroupAllFunction

        /// <summary>
        /// Delete All AvaiLabel Function on Selected Group
        /// </summary>
        /// <param name="strGrpCode">Selected Group Code</param>
        /// <returns>Success Bool</returns>
        public bool DeleteGroupAllFunction(string strGrpCode)
        {
            bool bResult;
            TQC_GROUP_FUNCTION oGroup = null;

            try
            {
                oGroup = new TQC_GROUP_FUNCTION();

                bResult = oGroup.DeleteGroupAllFunction(strGrpCode);
                return bResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oGroup != null) oGroup.Dispose();
                oGroup = null;
            }
        }

        #endregion

        #region LoadGroupUser

        /// <summary>
        /// Get User List on Selected Group
        /// </summary>
        /// <param name="strSelectedGroup">Group ID</param>
        /// <returns>Result DataSet</returns>
        public DataTable LoadGroupUser(string strSelectedGroup)
        {
            TQC_GROUP_USER oGroup = null;

            try
            {
                oGroup = new TQC_GROUP_USER();
                return oGroup.LoadGroupUser(strSelectedGroup);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DeleteGroup

        /// <summary>
        /// Delete Group
        /// </summary>
        /// <param name="strGrpCode">Group</param>
        /// <returns>Success Bool</returns>
        public bool DeleteGroup(string strGrpCode)
        {
            bool bResult;
            TQC_SEC_GROUP oGroup = null;

            try
            {
                oGroup = new TQC_SEC_GROUP();

                bResult = oGroup.DeleteGroup(strGrpCode);
                return bResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oGroup != null) oGroup.Dispose();
                oGroup = null;
            }
        }

        #endregion

        #region CountByGroup

        /// <summary>
        /// Get User Count By Group
        /// </summary>
        /// <returns>Result DataSet</returns>
        public DataTable CountByGroup()
        {
            TQC_GROUP_USER oGroup = null;
            try
            {
                oGroup = new TQC_GROUP_USER();
                return oGroup.CountByGroup();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region LoadJoinUserName

        /// <summary>
        /// Get User List By Group
        /// </summary>
        /// <returns>Result DataSet</returns>
        public DataTable LoadJoinUserName()
        {
            TQC_GROUP_USER oGroup = null;

            try
            {
                oGroup = new TQC_GROUP_USER();
                return oGroup.LoadJoinUserName();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region SearchGroupUser

        /// <summary>
        /// Search User with keyword (Name or ID)
        /// </summary>
        /// <param name="strSearchLike">Keyword</param>
        /// <returns>Result DataSet</returns>
        public DataTable SearchGroupUser(string strSearchLike)
        {
            TQC_GROUP_USER oGroup = null;

            try
            {
                oGroup = new TQC_GROUP_USER();
                return oGroup.SearchGroupUser(strSearchLike);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region SearchGroupList

        /// <summary>
        /// Search Group User Count with keywrod (name or ID)
        /// </summary>
        /// <param name="strSearchLike">Keyword</param>
        /// <returns>Result DataSet</returns>
        public DataTable SearchGroupList(string strSearchLike)
        {
            TQC_GROUP_USER oGroup = null;

            try
            {
                oGroup = new TQC_GROUP_USER();
                return oGroup.SearchGroupList(strSearchLike);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CheckID

        /// <summary>
        /// Get Count for User ID
        /// </summary>
        /// <param name="strUserID">User ID</param>
        /// <returns>ResultDataSet</returns>
        public bool CheckID(
            string strUserID
            )
        {
            TQC_USER oUser = new TQC_USER();
            object obj = oUser.CheckID(strUserID);

            if (obj == null || obj == DBNull.Value)
                return false;

            return Int32.Parse(obj.ToString()) > 0;
        }

        #endregion

        #region InsertGroupUser

        /// <summary>
        /// Insert Group User
        /// </summary>
        /// <param name="strGrpCode">Group Code</param>
        /// <param name="strUserID">User ID</param>
        /// <returns>Success Bool</returns>
        public bool InsertGroupUser(string strGrpCode, string strUserID)
        {
            bool bResult;
            TQC_GROUP_USER oGroup = null;

            try
            {
                oGroup = new TQC_GROUP_USER();

                bResult = oGroup.InsertGroupUser(strGrpCode, strUserID);
                return bResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oGroup != null) oGroup.Dispose();
                oGroup = null;
            }
        }

        #endregion

        #region DeleteGroupUser

        /// <summary>
        /// Delete Group User
        /// </summary>
        /// <param name="strID">Group ID</param>
        /// <returns>Success Bool</returns>
        public bool DeleteGroupUser(string strID)
        {
            bool bResult;
            TQC_GROUP_USER oGroup = null;

            try
            {
                oGroup = new TQC_GROUP_USER();

                bResult = oGroup.DeleteGroupUser(strID);
                return bResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oGroup != null) oGroup.Dispose();
                oGroup = null;
            }
        }

        #endregion

        #region LoadSecurityUser
        /// <summary>
        /// Load Security User
        /// </summary>
        /// <returns>Result DataSet</returns>
        public DataTable LoadSecurityUser()
        {
            TQC_USER oUser = null;

            try
            {
                oUser = new TQC_USER();
                return oUser.LoadSecurityUser();
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
        public bool InsertUser(string strUserID, string strUserName, string strPass, string strGrp
            , string strPnOffice, string strPnMobile, string strPnHome, string strPnOther, string strEmail)
        {
            bool bResult;
            TQC_USER oUser = null;

            try
            {
                oUser = new TQC_USER();

                bResult = oUser.InsertUser(strUserID, strUserName, strPass, strGrp, strPnOffice, strPnMobile
                        , strPnHome, strPnOther, strEmail);

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

        #region LoadJoinAllGroup

        /// <summary>
        /// Get All Group User List
        /// </summary>
        /// <returns>Result DataSet</returns>
        public DataTable LoadJoinAllGroup()
        {
            TQC_GROUP_USER oGroup = null;

            try
            {
                oGroup = new TQC_GROUP_USER();
                return oGroup.LoadJoinAllGroup();
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
        /// <returns>Success Bool</returns>
        public bool DeleteUser(string strUserID)
        {
            bool bResult;
            TQC_USER oUser = null;

            try
            {
                oUser = new TQC_USER();

                bResult = oUser.DeleteUser(strUserID);
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

        #region SearchDepartmentUser

        /// <summary>
        /// Search Department User List with keyword
        /// </summary>
        /// <param name="strSearchLike">Keyword</param>
        /// <returns>Result DataSet</returns>
        public DataTable SearchDepartmentUser(string strSearchLike)
        {
            TQC_GROUP_USER oGroup = null;

            try
            {
                oGroup = new TQC_GROUP_USER();
                return oGroup.SearchDepartmentUser(strSearchLike);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region SearchDepartmentList

        /// <summary>
        /// Get User Count By Department
        /// </summary>
        /// <param name="strSearchLike">Keyword</param>
        /// <returns>Result DataSet</returns>
        public DataTable SearchDepartmentList(string strSearchLike)
        {
            TQC_GROUP_USER oGroup = null;

            try
            {
                oGroup = new TQC_GROUP_USER();
                return oGroup.SearchDepartmentList(strSearchLike);
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
        /// <returns>Result DataSet</returns>
        public DataTable CountByDepartment()
        {
            TQC_USER oUser = null;

            try
            {
                oUser = new TQC_USER();
                return oUser.CountByDepartment();
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
        /// <returns>Result DataSet</returns>
        public DataTable LoadJoinUserName2()
        {
            TQC_USER oUser = null;

            try
            {
                oUser = new TQC_USER();
                return oUser.LoadJoinUserName2();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CheckFunction

        /// <summary>
        /// User Access Check for the Function
        /// </summary>
        /// <param name="strUserID">User ID</param>
        /// <param name="strFuncCode">Function Code</param>
        /// <returns>Result DataSet</returns>
        public bool CheckFunction(string strUserID, string strFuncCode)
        {
            TQC_GROUP_USER oGroup = new TQC_GROUP_USER();
            object obj = oGroup.CheckFunction(strUserID, strFuncCode);
            if (obj == null || obj == DBNull.Value)
                return false;
            return Int32.Parse(obj.ToString()) > 0;
        }

        #endregion

    }
}
