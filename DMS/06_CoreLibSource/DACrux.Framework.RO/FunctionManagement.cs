using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DACrux.Base;
using DACrux.Framework.Interface;

namespace DACrux.Framework.RO
{
    /// <summary>
    /// Class Name : FunctionManagement<br/>
    /// Summary    : Function Management Remoting Object Class<br/>
    /// Author     : Miracom AndyKuo<br/>
    /// First Date : 2009-02-25<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class FunctionManagement
    {
        #region Class Member

        DACrux.Framework.Interface.iDACruxFunctionManagement m_OBJ = null;

        #endregion

        #region Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public FunctionManagement()
        {
            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_QMS_COMMON);
            object obj = Activator.GetObject(typeof(DACrux.Framework.Interface.iDACruxFunctionManagement),
                strUrl + "/DACrux.Framework.BSL.FunctionManagement.bin");
            m_OBJ = obj as DACrux.Framework.Interface.iDACruxFunctionManagement;
            System.Configuration.ConfigurationManager.GetSection("System.Diagnostics");
        }

        #endregion

        #region Function Initial

        #region CheckFunctionCodeExistence

        /// <summary>
        /// Check FunctionCode's Existence(strFuncCode)
        /// </summary>
        /// <param name="strFuncCode">strFuncCode</param>
        /// <returns>ResultDataSet</returns>
        public DataTable CheckFuncCodeExistence(string strFuncCode)
        {
            try
            {
                return m_OBJ.CheckFuncCodeExistence(strFuncCode);  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region CheckFunctionNameExistence

        /// <summary>
        /// Check FunctionName's Existence(strFuncName)
        /// </summary>
        /// <param name="strFuncName">strFuncName</param>
        /// <returns>ResultDataSet</returns>
        public DataTable CheckFuncNameExistence(string strFuncName)
        {
            try
            {
                return  m_OBJ.CheckFuncNameExistence(strFuncName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region LoadFunctionList

        /// <summary>
        /// Get Total Function Information
        /// </summary>
        /// <param name="">N/A</param>
        /// <returns>ResultDataSet</returns>
        public DataTable LoadFunctionList()
        {
            try
            {
                return m_OBJ.LoadFunctionList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region CheckAttachedFunction

        /// <summary>
        /// Check whether Function Already Attached to SEC Group or not (strFucnCode)
        /// </summary>
        /// <param name="strFuncCode">Function Code</param>
        /// <returns>ResultDataSet</returns>
        public DataTable CheckAttachedFunction(string strFuncCode)
        {
            try
            {
                return m_OBJ.CheckAttachedFunction(strFuncCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region InsertFunction

        /// <summary>
        /// Insert New Function(strFuncCode,strFuncName,strHelpUrl)
        /// </summary>
        /// <param name="strFuncCode">FunctionCode</param>
        /// <param name="strFuncName">FunctionName</param>
        /// <param name="strHelpUrl">HelpUrl</param>
        /// <returns>ResultBool</returns>
        public bool InsertFunction(string strFuncCode, string strFuncName, string strHelpUrl)
        {
            bool bReturn;
            try
            {
                bReturn = m_OBJ.InsertFunction(strFuncCode, strFuncName, strHelpUrl);
                return bReturn;
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
        /// <returns>ResultBool</returns>
        public bool UpdateFunction(string strFuncCode,string strFuncName,string strHelpUrl,string strOldFuncCode)
        {
            bool bReturn;
            try
            {
                bReturn = m_OBJ.UpdateFunction(strFuncCode, strFuncName, strHelpUrl, strOldFuncCode);
                return bReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region UpdateGroupFunction

        /// <summary>
        /// Update Group Function(strFuncCode,strOldFuncCode)
        /// </summary>
        /// <param name="strFuncCode">Function Code</param>
        /// <param name="strOldFuncCode">Old Function Code</param>
        /// <returns>ResultBool</returns>
        public bool UpdateGroupFunction(string strFuncCode,string strOldFuncCode)
        {
            bool bReturn;
            try
            {
                bReturn = m_OBJ.UpdateGroupFunction(strFuncCode,strOldFuncCode);
                return bReturn;
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
            bool bReturn;
            try
            {
                bReturn = m_OBJ.DeleteFunction(strFuncCode);
                return bReturn;
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
            bool bReturn;
            try
            {
                bReturn = m_OBJ.DeleteGroupFunction(strFuncCode);
                return bReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #endregion


    }
}
