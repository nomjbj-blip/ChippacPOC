using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.Framework.Interface
{
    public interface iDACruxUserMonitor
    {
        DataTable HitRateByFunction(string strStartTime, string strEndTime);
        DataTable UserCountByFunction(string strStartTime, string strEndTime);
        DataTable DailyTrend(string strStartTime, string strEndTime);
        DataTable MonthlyTrend(string strStartTime, string strEndTime);
        DataTable HitRateByDepartment(string strStartTime, string strEndTime);
        DataTable HitLogDetail(string strStartTime, string strEndTime);
    }
}
