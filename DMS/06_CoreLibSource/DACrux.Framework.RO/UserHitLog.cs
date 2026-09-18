using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DACrux.Base;
using DACrux.Framework.Interface;

namespace DACrux.Framework.RO
{
    /// <summary>
    /// Class Name : UserHitLog<br/>
    /// Summary    : User Hit Log Remoting Object Class<br/>
    /// Author     : YSIM<br/>
    /// First Date : 2009-02-23<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class UserHitLog
    {
        #region Class Member

        DACrux.Framework.Interface.iDACruxHitLog m_OBJ = null;

        #endregion

        #region Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public UserHitLog()
        {
            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_QMS_COMMON);
            object obj = Activator.GetObject(typeof(DACrux.Framework.Interface.iDACruxHitLog),
                strUrl + "/DACrux.Framework.BSL.UserHitLog.bin");
            m_OBJ = obj as DACrux.Framework.Interface.iDACruxHitLog;
            System.Configuration.ConfigurationManager.GetSection("System.Diagnostics");
        }

        #endregion

        #region StartLog

        /// <summary>
        /// Start Hit Log
        /// </summary>
        /// <param name="strFunctionCode">Function Code</param>
        /// <param name="strUserID">User ID</param>
        /// <param name="strIPAddress">Client IP Address</param>
        /// <param name="strParameter">Parameter for Function</param>
        public void StartLog(string strFunctionCode, string strUserID, string strIPAddress, string strParameter)
        {
            m_OBJ.StartLog(strFunctionCode, strUserID, strIPAddress, strParameter);
        }

        public static void StartFunction(string strFunctionCode, string strUserID, string strIPAddress, string strParameter)
        {
            try
            {
                string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_QMS_COMMON);
                DACrux.Framework.Interface.iDACruxHitLog obj = (DACrux.Framework.Interface.iDACruxHitLog)Activator.GetObject(typeof(DACrux.Framework.Interface.iDACruxHitLog), strUrl + "/DACrux.Framework.BSL.UserHitLog.bin");
                obj.StartLog(strFunctionCode, strUserID, strIPAddress, strParameter);
            }
            catch { }
        }

        #endregion

        #region EndLog

        /// <summary>
        /// End Hit Log
        /// </summary>
        /// <param name="strFunctionCode">Function Code</param>
        /// <param name="strUserID">User ID</param>
        /// <param name="strIPAddress">Client IP Address</param>
        public void EndLog(string strFunctionCode, string strUserID, string strIPAddress)
        {
            m_OBJ.EndLog(strFunctionCode, strUserID, strIPAddress);
        }

        public static void EndFunction(string strFunctionCode, string strUserID, string strIPAddress)
        {
            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_QMS_COMMON);
            DACrux.Framework.Interface.iDACruxHitLog obj = (DACrux.Framework.Interface.iDACruxHitLog)Activator.GetObject(typeof(DACrux.Framework.Interface.iDACruxHitLog), strUrl + "/DACrux.Framework.BSL.UserHitLog.bin");
            obj.EndLog(strFunctionCode, strUserID, strIPAddress);
        }

        #endregion
    }
}
