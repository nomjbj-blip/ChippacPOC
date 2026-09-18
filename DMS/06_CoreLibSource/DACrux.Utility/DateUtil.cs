using System;

namespace DACrux.Utility
{
    /// <summary>
    /// Class Name : DateUtil<br/>
    /// Summary    : Date Manupulation Class<br/>
    /// Author     : Miracom David, Kwak<br/>
    /// First Date : 2009-02-23<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class DateUtil
    {
        #region GetWeekStartDate
        /// <summary>
		/// Return Start day of week (Base 07:00)
		/// </summary>
        /// <returns>Week Start Day</returns>
        public static string GetWeekStartDate()
		{
			DateTime dtmStartDay;
			string strStartDateTime = string.Empty;

			for(int i=0; i< 7; i++)
			{
				if(DateTime.Now.AddDays(-i).DayOfWeek == DayOfWeek.Monday)
				{
					dtmStartDay = DateTime.Now.AddDays(-i);
					strStartDateTime = dtmStartDay.Year.ToString()
						+ dtmStartDay.Month.ToString().PadLeft(2,'0')
						+ dtmStartDay.Day.ToString().PadLeft(2,'0')
						+ "070000";
					break;
				}
			}

			return strStartDateTime;
		}

		#endregion

        #region GetMonthStartDate

        /// <summary>
        /// Return Start day of month (Base 07:00)
        /// </summary>
        /// <returns>Month Start Day</returns>
		public static string GetMonthStartDate()
		{
			string strStartDateTime = DateTime.Now.Year.ToString()
						+ DateTime.Now.Month.ToString().PadLeft(2,'0')
						+ "01070000";
			return strStartDateTime;
		}

		#endregion

	}
}
