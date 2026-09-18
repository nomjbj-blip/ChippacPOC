using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DACrux.Base;
using DACrux.Framework.Interface;

namespace DACrux.Framework.RO
{
    /// <summary>
    /// Class Name : UserGroup<br/>
    /// Summary    : User Group Management Remoting Object Class<br/>
    /// Author     : YSIM<br/>
    /// First Date : 2009-02-23<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class UserGroup
    {
        #region Class Member

        DACrux.Framework.Interface.iDACruxUserGroup m_OBJ = null;

        #endregion

        #region Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public UserGroup()
        {
            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_QMS_COMMON);
            object obj = Activator.GetObject(typeof(DACrux.Framework.Interface.iDACruxUserGroup),
                strUrl + "/DACrux.Framework.BSL.UserGroup.bin");
            m_OBJ = obj as DACrux.Framework.Interface.iDACruxUserGroup;
            System.Configuration.ConfigurationManager.GetSection("System.Diagnostics");
        }

        #endregion

        #region LoadGroupList

        /// <summary>
        /// Get Group List
        /// </summary>
        /// <param name="strGrpCode">Group Code</param>
        /// <returns>Result DataTable</returns>
        public DataTable LoadGroupList(string strGrpCode)
        {
            return m_OBJ.LoadGroupList(strGrpCode);
        }
        #endregion

        #region LoadGroupInfo

        /// <summary>
        /// Get Group Information
        /// </summary>
        /// <param name="strSelectedGroup">Group Code</param>
        /// <returns>Result DataSet</returns>
        public DataTable LoadGroupInfo(string strSelectedValue)
        {
            return m_OBJ.LoadGroupInfo(strSelectedValue);
        }

        #endregion

        #region LoadGroupFunction

        /// <summary>
        /// Get Function List for Selected Group Code
        /// </summary>
        /// <param name="strSelectedGroup"></param>
        /// <returns>Result DataSet</returns>
        public DataTable LoadGroupFunction(string strSelectedValue)
        {
            return m_OBJ.LoadGroupFunction(strSelectedValue);
        }

        #endregion

        #region LoadOutFunction

        /// <summary>
        /// Missing Function List for Selected Group Code
        /// </summary>
        /// <param name="strSelectedGroup">Selected Group Code</param>
        /// <returns>Result DataSet</returns>
        public DataTable LoadOutFunction(string strSelectedValue)
        {
            return m_OBJ.LoadOutFunction(strSelectedValue);
        }

        #endregion

        #region LoadMaxGroup

        /// <summary>
        /// Get Max Group Code to making a new Group Code
        /// </summary>
        /// <returns>Result DataSet</returns>
        public DataTable LoadMaxGroup()
        {
            return m_OBJ.LoadMaxGroup();
        }

        #endregion

        #region LoadAllFunction

        /// <summary>
        /// All Function List
        /// </summary>
        /// <returns></returns>
        public DataTable LoadAllFunction()
        {
            return m_OBJ.LoadAllFunction();
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
        public bool InsertGroupInfo(string strGroupCode, string strGroupName, string strCaption)
        {
            return m_OBJ.InsertGroupInfo(strGroupCode, strGroupName, strCaption);
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
        public bool UpdateGroupInfo(string strGroupCode, string strGroupName, string strCaption)
        {
            return m_OBJ.UpdateGroupInfo(strGroupCode, strGroupName, strCaption);
        }

        #endregion

        #region DeleteGroupAllFunction

        /// <summary>
        /// Delete All Available Function on Selected Group
        /// </summary>
        /// <param name="strGrpCode">Selected Group Code</param>
        /// <returns>Success Bool</returns>
        public bool DeleteGroupAllFunction(string strGroupCode)
        {
            return m_OBJ.DeleteGroupAllFunction(strGroupCode);
        }

        #endregion

        #region InsertGroupFunction

        /// <summary>
        /// Insert Group Function
        /// </summary>
        /// <param name="strGrpCode">Group Code</param>
        /// <param name="strFunCode">Function Code</param>
        /// <returns>Success Code</returns>
        public bool InsertGroupFunction(string strGroupCode, string strFunctionCode)
        {
            return m_OBJ.InsertGroupFunction(strGroupCode, strFunctionCode);
        }

        #endregion

        #region LoadGroupUser

        /// <summary>
        /// Get User List on Selected Group
        /// </summary>
        /// <param name="strSelectedGroup">Group ID</param>
        /// <returns>Result DataSet</returns>
        public DataTable LoadGroupUser(string strGroupCode)
        {
            return m_OBJ.LoadGroupUser(strGroupCode);
        }

        #endregion

        #region DeleteGroup

        /// <summary>
        /// Delete Group
        /// </summary>
        /// <param name="strGrpCode">Group</param>
        /// <returns>Success Bool</returns>
        public bool DeleteGroup(string strGroupCode)
        {
            return m_OBJ.DeleteGroup(strGroupCode);
        }

        #endregion

        #region CountByGroup

        /// <summary>
        /// Get User Count By Group
        /// </summary>
        /// <returns>Result DataSet</returns>
        public DataTable CountByGroup()
        {
            return m_OBJ.CountByGroup();
        }

        #endregion

        #region LoadJoinUserName

        /// <summary>
        /// Get User List By Group
        /// </summary>
        /// <returns>Result DataSet</returns>
        public DataTable LoadJoinUserName()
        {
            return m_OBJ.LoadJoinUserName();
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
            return m_OBJ.SearchGroupUser(strSearchLike);
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
            return m_OBJ.SearchGroupList(strSearchLike);
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
        public bool InsertUser(string strUserID, string strUserName, string strPass, string strGrp, string strPnOffice, string strPnMobile, string strPnHome, string strPnOther, string strEmail)
        {
            return m_OBJ.InsertUser(strUserID, strUserName, strPass, strGrp, strPnOffice, strPnMobile, strPnHome, strPnOther, strEmail);
        }

        //public bool InsertUser(string strUserID, string strUserName, string strPass, string strGrp, string strArea, string strPnOffice, string strPnMobile, string strPnHome, string strPnOther, string strEmail)
        //{
        //    bool bReturn;

        //    try
        //    {
        //        bReturn = m_OBJ.InsertUser(strUserID, strUserName, strPass, strGrp, strArea, strPnOffice, strPnMobile, strPnHome, strPnOther, strEmail);

        //        return bReturn;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #endregion

        #region InsertGroupUser

        /// <summary>
        /// Insert Group User
        /// </summary>
        /// <param name="strGrpCode">Group Code</param>
        /// <param name="strUserID">User ID</param>
        /// <returns>Success Bool</returns>
        public bool InsertGroupUser(string strGroupCode, string strUserID)
        {
            return m_OBJ.InsertGroupUser(strGroupCode, strUserID);
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
            return m_OBJ.DeleteGroupUser(strID);
        }

        #endregion

        #region DeleteUser

        /// <summary>
        /// Delete Selected User
        /// </summary>
        /// <param name="strUserID">User ID</param>
        /// <returns>Success Bool</returns>
        public bool DeleteUser(string strID)
        {
            return m_OBJ.DeleteUser(strID);
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
            return m_OBJ.SearchDepartmentUser(strSearchLike);
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
            return m_OBJ.SearchDepartmentList(strSearchLike);
        }

        #endregion

        #region CountByDepartment

        /// <summary>
        /// Get User Count By Department
        /// </summary>
        /// <returns>Result DataSet</returns>
        public DataTable CountByDepartment()
        {
            return m_OBJ.CountByDepartment();
        }

        #endregion

        #region LoadSecurityUser
        /// <summary>
        /// Load Security User
        /// </summary>
        /// <returns>Result DataSet</returns>
        public DataTable LoadSecurityUser()
        {
            return m_OBJ.LoadSecurityUser();
        }

        #endregion

        #region LoadJoinUserName2

        /// <summary>
        /// Get User List
        /// </summary>
        /// <returns>Result DataSet</returns>
        public DataTable LoadJoinUserName2()
        {
            return m_OBJ.LoadJoinUserName2();
        }

        #endregion

        #region LoadJoinAllGroup

        /// <summary>
        /// Get All Group User List
        /// </summary>
        /// <returns>Result DataSet</returns>
        public DataTable LoadJoinAllGroup()
        {
            return m_OBJ.LoadJoinAllGroup();
        }

        #endregion

        #region CheckFunction

        /// <summary>
        /// User Access Check for the Function
        /// </summary>
        /// <param name="strUserID">User ID</param>
        /// <param name="strFuncCode">Function Code</param>
        /// <returns>Result DataSet</returns>
        public bool CheckFunction(string strRegID, string strFuncCode)
        {
            return m_OBJ.CheckFunction(
                strRegID,
                strFuncCode
                );
        }

        #endregion
    }
}
