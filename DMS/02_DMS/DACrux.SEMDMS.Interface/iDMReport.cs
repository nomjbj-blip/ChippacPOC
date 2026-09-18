using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.SEMDMS.Interface
{
    public interface iDMReport
    {
        /// <summary>
        /// inspection 에 대한 lot 또는 Wafer에 대한 수량
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="fromdate"></param>
        /// <param name="bFlag">true = 'LOT', false = 'WAFER'</param>
        /// <returns></returns>
        DataSet GetInspectionLotOrWaferCnt(string factory, string fromdate, string todate, bool bTypeFlag, bool bAreaFlag);

        DataTable GetInspectionDetail(string factory, string fromDate, string toDate, string model, bool bAreaFlag);

        object[,] GetDmSummaryData(long[] Steps);
        byte[] GetDmSummaryData_Comp(long[] Steps);

        // DM Data Sheet
        byte[] GetDataSheet_Comp(string fromDate, string toDate, string[] products, string[] stepids, string[] lots, string[] wafers, string[] xItem, string[] categorise);
        // DM Data Trend
        byte[] GetDataTrend_Comp(string fromDate, string toDate, string[] products, string[] stepids, string[] lots, string[] wafers, string[] classnumber, string ordered, bool lastInsp);

        // Killing Rate
        byte[] GetKillingRate_Comp(string fromDate, string toDate, string[] layers, string[] lotIDs, string[] waferIDs, string[] defectCodes, bool isNormalized);
    }
}
