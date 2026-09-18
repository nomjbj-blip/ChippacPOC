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
    /// Class Name : UserHitLog<br/>
    /// Summary    : User Hit Log Process Business Logic Class<br/>
    /// Author     : YSIM<br/>
    /// First Date : 2009-02-23<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class UserHitLog : Miracom.Middleware.BaseComponent, iDACruxHitLog
    {
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
            TQC_USER_HITLOG oUserHitLog = null;

            try
            {
                oUserHitLog = new TQC_USER_HITLOG();

                oUserHitLog.StartLog(strFunctionCode, strUserID, strIPAddress, strParameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oUserHitLog != null) oUserHitLog.Dispose();
            }
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
            TQC_USER_HITLOG oUserHitLog = null;

            try
            {
                oUserHitLog = new TQC_USER_HITLOG();
                oUserHitLog.EndLog(strFunctionCode, strUserID, strIPAddress);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oUserHitLog != null) oUserHitLog.Dispose();
            }
        }

        #endregion
    }
}
