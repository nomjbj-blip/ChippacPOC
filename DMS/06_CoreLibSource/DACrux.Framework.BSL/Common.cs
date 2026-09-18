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
    /// Class Name : Common<br/>
    /// Summary    : QMS Common Business Logic Class<br/>
    /// Author     : YSIM<br/>
    /// First Date : 2009-03-30<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class Common : Miracom.Middleware.BaseComponent, iDACruxCommon
    {
        #region GetDynamic

        /// <summary>
        /// Get Dynamic DSL (For Select Query)
        /// </summary>
        /// <param name="strSQL">SQL</param>
        /// <returns>Result Data Table</returns>
        public DataTable GetDynamic(string strSQL)
        {
            DataTable dt = null;
            TQC_COMMON oDSL = null;

            try
            {
                oDSL = new TQC_COMMON();
                dt = oDSL.GetDynamic(strSQL);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
                if (oDSL != null) oDSL.Dispose();
                oDSL = null;
            }
        }

        #endregion

        #region ExcuteDynamic

        /// <summary>
        /// Excute Dynamic DSL (For Insert, Delete, Update Query)
        /// </summary>
        /// <param name="strSQL">SQL</param>
        public void ExcuteDynamic(string strSQL)
        {
            TQC_COMMON oDSL = null;

            try
            {
                oDSL = new TQC_COMMON();

                oDSL.ExcuteDynamic(strSQL);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oDSL != null) oDSL.Dispose();
                oDSL = null;
            }
        }

        #endregion

        #region GetConfigValue

        /// <summary>
        /// Get Config Value 
        /// </summary>
        /// <param name="strCategory">Category String</param>
        /// <param name="strName">Item Name</param>
        /// <returns>Result Data Table</returns>
        public string GetConfigValue(string strCategory, string strName)
        {
            TQC_CONFIG oDSL = null;

            try
            {
                oDSL = new TQC_CONFIG();
                return oDSL.GetConfigValue(strCategory, strName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oDSL != null) oDSL.Dispose();
                oDSL = null;
            }
        }

        public  DataTable GetConfigList(string strCategory)
        {
            DataTable oDSL = null;
            return  oDSL;
         }

        #endregion
    }
}
