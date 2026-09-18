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
    /// 2009-02-25 Develop New Function By Miracom AndyKuo<br/>
    ///    CheckFunctionCodeExistence<br/>
    ///    CheckFunctionNameExistence<br/>
    ///    Insert Function<br/>
    ///    Update Function<br/>
    ///    Check Attached Function<br/>
    ///    Delete Function<br/>
    ///    Remove Function From Group<br/>
    /// </summary>
    public class FunctionManagement : Miracom.Middleware.BaseComponent, iDACruxFunctionManagement 
    {
        #region LoadFunctionList

        /// <summary>
        /// Get Toal Function Information
        /// </summary>
        /// <param name="">N/A</param>
        /// <returns>Result DataSet</returns>
        public DataTable LoadFunctionList()
        {
            TQC_FUNCTION_LIST oFuncList = null;

            try
            {
                oFuncList = new TQC_FUNCTION_LIST();
                return oFuncList.LoadAllFunction();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CheckFuncCodeExistence

        /// <summary>
        /// Check Function Code's Existence(strFuncCode)
        /// </summary>
        /// <param name="strFuncCode">Function Code</param>
        /// <returns>Result DataSet</returns>
        public DataTable CheckFuncCodeExistence(string strFuncCode)
        {
            TQC_FUNCTION_LIST oFuncList = null;

            try
            {
                oFuncList = new TQC_FUNCTION_LIST();
                return oFuncList.CheckFuncCodeExistence(strFuncCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CheckFuncNameExistence

        /// <summary>
        /// Check Function Name's Existence(strFuncName)
        /// </summary>
        /// <param name="strFuncName">Function Name</param>
        /// <returns>Result DataSet</returns>
        public DataTable CheckFuncNameExistence(string strFuncName)
        {
            TQC_FUNCTION_LIST oFuncList = null;

            try
            {
                oFuncList = new TQC_FUNCTION_LIST();
                return oFuncList.CheckFuncNameExistence(strFuncName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CheckAttachedFunction

        /// <summary>
        /// Check whether Function Already Attached to Security Group or not(strFuncCode)
        /// </summary>
        /// <param name="strFuncCode">Function Code</param>
        /// <returns>Result DataSet</returns>
        public DataTable CheckAttachedFunction(string strFuncCode)
        {
            TQC_FUNCTION_LIST oFuncList = null;

            try
            {
                oFuncList = new TQC_FUNCTION_LIST();
                return oFuncList.CheckAttachedFunction(strFuncCode);
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
        /// <param name="strFuncCode">Function Code</param>
        /// <param name="strFuncName">Function Name</param>
        /// <param name="strHelpUrl">Help Url</param>
        /// <returns>Success Bool</returns>
        public bool InsertFunction(string strFuncCode, string strFuncName, string strHelpUrl)
        {
            bool bResult;
            TQC_FUNCTION_LIST oFuncList = null;

            try
            {
                oFuncList = new TQC_FUNCTION_LIST();

                bResult = oFuncList.InsertFunction(strFuncCode, strFuncName, strHelpUrl);
                
                return bResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oFuncList != null) oFuncList.Dispose();
                oFuncList = null;
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
            bool bResult;
            TQC_FUNCTION_LIST oFuncList = null;

            try
            {
                oFuncList = new TQC_FUNCTION_LIST();

                bResult = oFuncList.UpdateFunction(strFuncCode, strFuncName, strHelpUrl, strOldFuncCode);
                
                return bResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oFuncList != null) oFuncList.Dispose();
                oFuncList = null;
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
            bool bResult;
            TQC_FUNCTION_LIST oFuncList = null;

            try
            {
                oFuncList = new TQC_FUNCTION_LIST();

                bResult = oFuncList.UpdateGroupFunction(strFuncCode, strOldFuncCode);

                return bResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oFuncList != null) oFuncList.Dispose();
                oFuncList = null;
            }
        }

        #endregion

        #region DeleteFunction

        /// <summary>
        /// Delete Function Record(strFuncCode)
        /// </summary>
        /// <param name="strFuncCode">Function Code</param>
        /// <returns>Success Bool</returns>
        public bool DeleteFunction(string strFuncCode)
        {
            bool bResult;
            TQC_FUNCTION_LIST oFuncList = null;

            try
            {
                oFuncList = new TQC_FUNCTION_LIST();

                bResult = oFuncList.DeleteFunction(strFuncCode);
                return bResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oFuncList != null) oFuncList.Dispose();
                oFuncList = null;
            }
        }

        #endregion

        #region RemoveFunctionFromGroup

        /// <summary>
        /// Remove Function From SEC Group(strFuncCode)
        /// </summary>
        /// <param name="strFuncCode">Function Code</param>
        /// <returns>Success Bool</returns>
        public bool DeleteGroupFunction(string strFuncCode)
        {
            bool bResult;
            TQC_FUNCTION_LIST oFuncList = null;

            try
            {
                oFuncList = new TQC_FUNCTION_LIST();

                bResult = oFuncList.DeleteGroupFunction(strFuncCode);
                return bResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oFuncList != null) oFuncList.Dispose();
                oFuncList = null;
            }
        }

        #endregion


    }
}
