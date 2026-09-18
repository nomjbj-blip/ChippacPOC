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
    /// Class Name : UserMonitor<br/>
    /// Summary    : User Monitoring Process Business Logic Class<br/>
    /// Author     : YSIM<br/>
    /// First Date : 2009-02-23<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class UserMonitor : Miracom.Middleware.BaseComponent, iDACruxUserMonitor
    {
        #region HitRateByFunction

        /// <summary>
        /// Get Hit Rate By Function
        /// </summary>
        /// <param name="strStartTime">Start Time</param>
        /// <param name="strEndTime">End Time</param>
        /// <returns>Result DataSet</returns>
        public DataTable HitRateByFunction(string strStartTime, string strEndTime)
        {
            TQC_USER_HITLOG oHitLog = null;
            try
            {
                oHitLog = new TQC_USER_HITLOG();
                return oHitLog.HitRateByFunction(strStartTime, strEndTime);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region UserCountByFunction

        /// <summary>
        /// Get User Count By Function
        /// </summary>
        /// <param name="strStartTime">Start Time</param>
        /// <param name="strEndTime">End Time</param>
        /// <returns>Result DataSet</returns>
        public DataTable UserCountByFunction(string strStartTime, string strEndTime)
        {
            TQC_USER_HITLOG oHitLog = null;

            try
            {
                oHitLog = new TQC_USER_HITLOG();
                return oHitLog.UserCountByFunction(strStartTime, strEndTime);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DailyTrend

        /// <summary>
        /// Get Daily Hit Log Trend
        /// </summary>
        /// <param name="strStartTime">Start Time</param>
        /// <param name="strEndTime">End Time</param>
        /// <returns>Result DataSet</returns>
        public DataTable DailyTrend(string strStartTime, string strEndTime)
        {
            TQC_USER_HITLOG oHitLog = null;

            try
            {
                oHitLog = new TQC_USER_HITLOG();
                return oHitLog.DailyTrend(strStartTime, strEndTime);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region MonthlyTrend

        /// <summary>
        /// Get Monthly Hit Log Trend
        /// </summary>
        /// <param name="strStartTime">Start Time</param>
        /// <param name="strEndTime">End Time</param>
        /// <returns>Result DataSet</returns>
        public DataTable MonthlyTrend(string strStartTime, string strEndTime)
        {
            TQC_USER_HITLOG oHitLog = null;

            try
            {
                oHitLog = new TQC_USER_HITLOG();
                return oHitLog.MonthlyTrend(strStartTime, strEndTime);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region HitRateByDepartment

        /// <summary>
        /// Get Hit Rate By Department
        /// </summary>
        /// <param name="strStartTime">Start Time</param>
        /// <param name="strEndTime">End Time</param>
        /// <returns>Result DataSet</returns>
        public DataTable HitRateByDepartment(string strStartTime, string strEndTime)
        {
            TQC_USER_HITLOG oHitLog = null;

            try
            {
                oHitLog = new TQC_USER_HITLOG();
                return oHitLog.HitRateByDepartment(strStartTime, strEndTime);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region HitLogDetail

        /// <summary>
        /// Get Detail HitLog
        /// </summary>
        /// <param name="strStartTime">Start Time</param>
        /// <param name="strEndTime">End Time</param>
        /// <returns>Result DataSet</returns>
        public DataTable HitLogDetail(string strStartTime, string strEndTime)
        {
            TQC_USER_HITLOG oHitLog = null;

            try
            {
                oHitLog = new TQC_USER_HITLOG();
                return oHitLog.HitLogDetail(strStartTime, strEndTime);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
