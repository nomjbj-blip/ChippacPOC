using System;
using System.Data;

namespace DACrux.Framework.DSL
{
    /// <summary>
    /// Class Name : TQC_USER_HITLOG<br/>
    /// Summary    : Access for TQ_USER_HITLOG Table<br/>
    /// Author     : YSIM<br/>
    /// First Date : 2009-02-23<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class TQC_USER_HITLOG : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public TQC_USER_HITLOG()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["QMS_CONNECT_ID"];
                if (connectID == null || connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQC_USER_HITLOG.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            try
            {
                this.Execute("StartLog", null, new string[] { strFunctionCode, strUserID, strIPAddress, strParameter });
            }
            catch (Exception ex)
            {
                throw ex;
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
            try
            {
                this.Execute("EndLog", null, new string[] { strFunctionCode, strUserID, strIPAddress });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region HitRateByFunction

        /// <summary>
        /// Get Hit Rate By Function
        /// </summary>
        /// <param name="strStartTime">Start Time</param>
        /// <param name="strEndTime">End Time</param>
        /// <returns>Result DataTable</returns>
        public DataTable HitRateByFunction(string strStartTime, string strEndTime)
        {
            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("HitRateByFunction", null, new string[] { strStartTime, strEndTime });
                return dt;
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
        /// <returns>Result DataTable</returns>
        public DataTable UserCountByFunction(string strStartTime, string strEndTime)
        {
            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("UserCountByFunction", null, new string[] { strStartTime, strEndTime });
                return dt;
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
        /// <returns>Result DataTable</returns>
        public DataTable DailyTrend(string strStartTime, string strEndTime)
        {
            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("DailyTrend", null, new string[] { strStartTime, strEndTime });
                return dt;
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
        /// <returns>Result DataTable</returns>
        public DataTable MonthlyTrend(string strStartTime, string strEndTime)
        {
            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("MonthlyTrend", null, new string[] { strStartTime, strEndTime });
                return dt;
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
        /// <returns>Result DataTable</returns>
        public DataTable HitRateByDepartment(string strStartTime, string strEndTime)
        {
            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("HitRateByDepartment", null, new string[] { strStartTime, strEndTime });
                return dt;
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
        /// <returns>Result DataTable</returns>
        public DataTable HitLogDetail(string strStartTime, string strEndTime)
        {
            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("HitLogDetail", null, new string[] { strStartTime, strEndTime });
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
    }
}
