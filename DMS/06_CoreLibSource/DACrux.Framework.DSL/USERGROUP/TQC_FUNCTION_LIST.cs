using System;
using System.Data;

namespace DACrux.Framework.DSL
{
    /// <summary>
    /// Class Name : TQ_FUNCTION_LIST<br/>
    /// Summary    : Access for TQ_FUNCTION_LIST Table<br/>
    /// Author     : YSIM<br/>
    /// First Date : 2009-02-23<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// 2009-02-25 Develop New Function By AndyKuo<br/>
    ///    CheckFunctionCodeExistence<br/>
    ///    CheckFunctionNameExistence<br/>
    ///    Insert Function<br/>
    ///    Update Function<br/>
    ///    Check Attached Function<br/>
    ///    Delete Function<br/>
    ///    Remove Function From Group<br/>
    /// </summary>
    public class TQC_FUNCTION_LIST : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public TQC_FUNCTION_LIST()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["QMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQC_FUNCTION_LIST.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region LoadGroupFunction

        /// <summary>
        /// Get Function Information By selected Group(strSelectedGroup)
        /// </summary>
        /// <param name="strSelectedGroup">Group Code</param>
        /// <returns>Result DataTable</returns>
        public DataTable LoadGroupFunction(string strSelectedGroup)
        {
            DataTable dt;
            try
            {
                dt = this.GetDataTable("LOADGROUPFUNCTION", null, new string[] { strSelectedGroup });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region LoadOutFunction

        /// <summary>
        /// Get Function List witch are not using.(strSelectedGroup)
        /// </summary>
        /// <param name="strSelectedGroup">Group Code</param>
        /// <returns>Result DataTable</returns>
        public DataTable LoadOutFunction(string strSelectedGroup)
        {
            DataTable dt;
            try
            {
                dt = this.GetDataTable("LOADOUTFUNCTION", null, new string[] { strSelectedGroup });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region LoadAllFunction

        /// <summary>
        /// Get Total Function Information
        /// </summary>
        /// <param name="">N/A</param>
        /// <returns>Result DataTable</returns>
        public DataTable LoadAllFunction()
        {
            DataTable dt;
            try
            {
                dt = this.GetDataTable("LOADALLFUNCTION", null, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CheckFunctionCodeExistence

        /// <summary>
        /// Check Function Code's Existence(strFuncCode)
        /// </summary>
        /// <param name="strFuncCode">Function Code</param>
        /// <returns>Result DataTable</returns>
        public DataTable CheckFuncCodeExistence(string strFuncCode)
        {
            DataTable dt;
            try
            {
                dt = this.GetDataTable("CHECKFUNCTIONCODE", null, new string[] { strFuncCode });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CheckFunctionNameExistence

        /// <summary>
        /// Check Function Name's Existence
        /// </summary>
        /// <param name="strFuncName">Function Name</param>
        /// <returns>Result DataTable</returns>
        public DataTable CheckFuncNameExistence(string strFuncName)
        {
            DataTable dt;
            try
            {
                dt = this.GetDataTable("CHECKFUNCTIONNAME", null, new string[] { strFuncName });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Insert Function

        /// <summary>
        /// Insert new Function Record(strFuncCode,strFuncName,strHelpUrl)
        /// </summary>
        /// <param name="strFuncCode">Function Code</param>
        /// <param name="strFuncName">Function Name</param>
        /// <param name="strHelpUrl">Help Url</param>
        /// <returns>Success Bool</returns>
        public bool InsertFunction(string strFuncCode, string strFuncName, string strHelpUrl)
        {
            try
            {
                this.GetDataTable("INSERTFUNCTION", null, new string[] { strFuncCode,strFuncName,strHelpUrl });
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region UpdateFunction

        /// <summary>
        /// Update Function Information(strFuncCode,strFuncName,strHelpUrl,strOldFuncCode)
        /// </summary>
        /// <param name="strFuncCode">Function Code</param>
        /// <param name="strFuncName">Function Name</param>
        /// <param name="strHelpUrl">Help Url</param>
        /// <param name="strOldFuncCode">Old Function Code</param>
        /// <returns>Success Bool</returns>
        public bool UpdateFunction(string strFuncCode, string strFuncName,string strHelpUrl,string strOldFuncCode)
        {
            try
            {
                this.GetDataTable("UPDATEFUNCTION", null, new string[] { strFuncCode, strFuncName, strHelpUrl, strOldFuncCode });

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region UpdateGroupFunction

        /// <summary>
        /// Update Group Function(strFuncCode,strOldFuncCode)
        /// </summary>
        /// <param name="strFuncCode">Function Code</param>
        /// <param name="strOldFuncCode">Old Function Code</param>
        /// <returns>Success Bool</returns>
        public bool UpdateGroupFunction(string strFuncCode, string strOldFuncCode)
        {
            try
            {
                this.GetDataTable("UPDATE_GROUP_FUNCTION", null, new string[] {strFuncCode, strOldFuncCode });

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region CheckAttachedFunction

        /// <summary>
        /// Check whether Function Already Attached to SEC Group or not(strFuncCode)
        /// </summary>
        /// <param name="strFuncCode">Function Code</param>
        /// <returns>ResultDataTable</returns>
        public DataTable CheckAttachedFunction(string strFuncCode)
        {
            DataTable dt;
            try
            {
                dt = this.GetDataTable("CHECKGROUPFUNCTION", null, new string[] { strFuncCode });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region DeleteFunction

        /// <summary>
        /// Delete Function Record(strFuncCode)
        /// </summary>
        /// <param name="strFuncCode">FunctionCode</param>
        /// <returns>ResultBool</returns>
        public bool DeleteFunction(string strFuncCode)
        {
            try
            {
                this.GetDataTable("DELETEFUNCTION", null, new string[] { strFuncCode });
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region RemoveFunctionFromGroup

        /// <summary>
        /// Remove Function From SEC Group(strFuncCode)
        /// </summary>
        /// <param name="strFuncCode">FunctionCode</param>
        /// <returns>ResultBool</returns>
        public bool DeleteGroupFunction(string strFuncCode)
        {
            try
            {
                this.GetDataTable("DELETEGROUPFUNCTION", null, new string[] { strFuncCode });
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


    }
}
