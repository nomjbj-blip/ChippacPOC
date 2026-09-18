using System;
using System.Data;

namespace DACrux.Framework.DSL
{
    /// <summary>
    /// Class Name : TQC_GROUP_USER<br/>
    /// Summary    : Access for TQC_GROUP_USER Table<br/>
    /// Author     : YSIM<br/>
    /// First Date : 2009-02-23<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class TQC_GROUP_USER : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public TQC_GROUP_USER()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["QMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQC_GROUP_USER.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region LoadGroupUser

        /// <summary>
        /// Get User List on Selected Group
        /// </summary>
        /// <param name="strFacility"></param>
        /// <param name="strSelectedGroup"></param>
        /// <returns></returns>
        public DataTable LoadGroupUser(string strSelectedGroup)
        {
            DataTable dt;
            try
            {
                dt = this.GetDataTable("LOADGROUPUSER", null, new string[] { strSelectedGroup });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CountByGroup

        /// <summary>
        /// Get User Count By Group
        /// </summary>
        /// <returns></returns>
        public DataTable CountByGroup()
        {
            DataTable dt;
            try
            {
                dt = this.GetDataTable("COUNTBYGROUP", null, null);
                return dt;
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
        /// <returns></returns>
        public DataTable LoadJoinUserName()
        {
            DataTable dt;
            try
            {
                dt = this.GetDataTable("LOADJOINUSERNAME", null, null);
                return dt;
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
        /// <returns>Result DataTable</returns>
        public DataTable SearchGroupUser(string strSearchLike)
        {
            DataTable dt;
            try
            {
                dt = this.GetDataTable("SEARCHGROUPUSER", null, new string[] { strSearchLike });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region SearchDepartmentUser

        /// <summary>
        /// Search Department User List with keyword
        /// </summary>
        /// <param name="strSearchLike">Keyword</param>
        /// <returns>Result DataTable</returns>
        public DataTable SearchDepartmentUser(string strSearchLike)
        {
            DataTable dt;
            try
            {
                dt = this.GetDataTable("SEARCHDEPARTMENTUSER", null, new string[] { strSearchLike });
                return dt;
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
        /// <returns></returns>
        public DataTable SearchGroupList(string strSearchLike)
        {
            DataTable dt;
            try
            {
                dt = this.GetDataTable("SEARCHGROUPLIST", null, new string[] { strSearchLike });
                return dt;
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
        /// <param name="strSearchLike"></param>
        /// <returns></returns>
        public DataTable SearchDepartmentList(string strSearchLike)
        {
            DataTable dt;
            try
            {
                dt = this.GetDataTable("SEARCHDEPARTMENTLIST", null, new string[] { strSearchLike });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region LoadJoinAllGroup

        /// <summary>
        /// Get All Group User List
        /// </summary>
        /// <returns></returns>
        public DataTable LoadJoinAllGroup()
        {
            DataTable dt;
            try
            {
                dt = this.GetDataTable("LOADJOINALLGROUP", null, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DeleteGroupUser

        /// <summary>
        /// Delete Group User
        /// </summary>
        /// <param name="strID"></param>
        /// <returns></returns>
        public bool DeleteGroupUser(string strID)
        {
            try
            {
                this.GetDataTable("DELETEGROUPUSER", null, new string[] { strID });

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region InsertGroupUser

        /// <summary>
        /// Insert Group User
        /// </summary>
        /// <param name="strGrpCode">Group Code</param>
        /// <param name="strUserID">User ID</param>
        /// <returns></returns>
        public bool InsertGroupUser(string strGrpCode, string strUserID)
        {
            try
            {
                this.GetDataTable("INSERTGROUPUSER", null, new string[] { strGrpCode, strUserID });

                return true;
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
        /// <returns>Result DataTable</returns>
        public object CheckFunction(
            string strUserID, 
            string strFuncCode
            )
        {
            return this.ExecuteScalar(
                "CHECKFUNCTION", 
                null, 
                new string[] { strUserID, strFuncCode }
                );
        }

        #endregion
    }
}
