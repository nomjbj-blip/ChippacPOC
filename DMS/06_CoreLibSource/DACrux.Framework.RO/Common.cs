using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DACrux.Framework.Interface;

namespace DACrux.Framework.RO
{
    /// <summary>
    /// Class Name : Common<br/>
    /// Summary    : QMS Common Remoting Object Class<br/>
    /// Author     : YSIM<br/>
    /// First Date : 2009-03-30<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class Common
    {
        #region Class Member

        DACrux.Framework.Interface.iDACruxCommon m_OBJ = null;

        #endregion

        #region Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public Common()
        {
            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_QMS_COMMON);
            object obj = Activator.GetObject(typeof(DACrux.Framework.Interface.iDACruxCommon),
                strUrl + "/DACrux.Framework.BSL.Common.bin");
            m_OBJ = obj as DACrux.Framework.Interface.iDACruxCommon;
            System.Configuration.ConfigurationManager.GetSection("System.Diagnostics");
        }

        #endregion

        #region GetDynamic

        /// <summary>
        /// Get Dynamic DSL (For Select Query)
        /// </summary>
        /// <param name="strSQL">SQL</param>
        /// <returns>Result Data Table</returns>
        public DataTable GetDynamic(string strSQL)
        {
            DataTable dt = null;

            try
            {
                dt = m_OBJ.GetDynamic(strSQL);
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
            try
            {
                m_OBJ.ExcuteDynamic(strSQL);
            }
            catch (Exception ex)
            {
                throw ex;
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
            try
            {
                return m_OBJ.GetConfigValue(strCategory, strName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetConfigList

        /// <summary>
        /// Get Config List 
        /// </summary>
        /// <param name="strCategory">Category String</param>
        /// <returns>Result Data Table</returns>
        public DataTable GetConfigList(string strCategory)
        {
            try
            {
                return m_OBJ.GetConfigList(strCategory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
