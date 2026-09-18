using System;
using System.Collections.Generic;
using System.Text;
using DACrux.Base;
using System.Data;

namespace DACrux.Framework.RO
{
    /// <summary>
    /// Class Name : UserMonitor<br/>
    /// Summary    : User Hit Log Monitoring Remoting Object Class<br/>
    /// Author     : YSIM<br/>
    /// First Date : 2009-02-23<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class UserMonitor
    {
        #region Class Member

        DACrux.Framework.Interface.iDACruxUserMonitor m_OBJ = null;

        #endregion

        #region Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public UserMonitor()
        {
            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_QMS_COMMON);
            object obj = Activator.GetObject(typeof(DACrux.Framework.Interface.iDACruxUserMonitor),
                        strUrl + "/DACrux.Framework.BSL.UserMonitor.bin");
            m_OBJ = obj as DACrux.Framework.Interface.iDACruxUserMonitor;
            System.Configuration.ConfigurationManager.GetSection("System.Diagnostics");
        }

        #endregion

        #region HitRateByFunction

        /// <summary>
        /// Get Hit Rate By Function
        /// </summary>
        /// <param name="strStartTime">Start Time</param>
        /// <param name="strEndTime">End Time</param>
        /// <returns>Result DataSet</returns>
        public DataTable HitRateByFunction(string strStartTime, string strEndTime)
        {
            return m_OBJ.HitRateByFunction(strStartTime, strEndTime);
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
            return m_OBJ.UserCountByFunction(strStartTime, strEndTime);
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
            return m_OBJ.DailyTrend(strStartTime, strEndTime);
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
            return m_OBJ.MonthlyTrend(strStartTime, strEndTime);
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
            return m_OBJ.HitRateByDepartment(strStartTime, strEndTime);
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
            return m_OBJ.HitLogDetail(strStartTime, strEndTime);
        }

        #endregion
    }
}
