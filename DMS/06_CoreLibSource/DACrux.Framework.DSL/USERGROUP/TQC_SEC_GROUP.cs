using System;
using System.Data;

namespace DACrux.Framework.DSL
{
    /// <summary>
    /// Class Name : TQC_SEC_GROUP<br/>
    /// Summary    : Access for TQ_SEC_GROUP Table<br/>
    /// Author     : YSIM<br/>
    /// First Date : 2009-02-23<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class TQC_SEC_GROUP : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public TQC_SEC_GROUP()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["QMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQC_SECURITY_GROUP.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region LoadGroupList

        /// <summary>
        /// Get Security Group List
        /// </summary>
        /// <param name="strGrpCode">Group Code</param>
        /// <returns>Result DataTable</returns>
        public DataTable LoadGroupList(string strGrpCode)
        {
            DataTable dt;
            try
            {
                dt = this.GetDataTable("LOADGROUPLIST", null, new string[] {strGrpCode} );
                return dt;
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
        /// <param name="strSelectedGroup">Selected Group Code</param>
        /// <returns></returns>
        public DataTable LoadGroupInfo(string strSelectedGroup)
        {
            DataTable dt;
            try
            {
                dt = this.GetDataTable("LOADGROUPINFO", null, new string[] { strSelectedGroup });
                return dt;
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
        /// <returns>Result DataTable</returns>
        public DataTable LoadMaxGroup()
        {
            DataTable dt;
            try
            {
                dt = this.GetDataTable("LOADMAXGROUP", null, null);
                return dt;
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
        public bool InsertGroupInfo(string strGroupCode, string strGroupName, string strGroupCaption)
        {
            try
            {
                this.GetDataTable("INSERTGROUPINFO", null, new string[] { strGroupCode, strGroupName, strGroupCaption });

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
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
        public bool UpdateGroupInfo(string strGroupCode, string strGroupName, string strGroupCaption)
        {
            try
            {
                this.GetDataTable("UPDATEGROUPINFO", null, new string[] { strGroupCode, strGroupName, strGroupCaption });

                return true;
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
        public bool DeleteGroup(string strGroupCode)
        {
            try
            {
                this.GetDataTable("DELETEGROUP", null, new string[] { strGroupCode });

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetUserGroup

        /// <summary>
        /// Get My Security Group List
        /// </summary>
        /// <param name="strUserID">User ID</param>
        /// <returns>Result DataTable</returns>
        public DataTable GetUserGroup(string strUserID)
        {
            DataTable dt;
            try
            {
                dt = this.GetDataTable("GETUSERGROUP", null, new string[] { strUserID });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
