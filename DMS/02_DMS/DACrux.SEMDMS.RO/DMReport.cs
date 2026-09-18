using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.SEMDMS.RO
{
    public class DMReport
    {
        DACrux.SEMDMS.Interface.iDMReport m_OBJ;

        public DMReport(
            )
        {
            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_DACRUX_DMS);
            object obj = Activator.GetObject(typeof(DACrux.SEMDMS.Interface.iDMReport),
                        strUrl + "/DACrux.SEMDMS.BSL.DMReport.bin");
            m_OBJ = obj as DACrux.SEMDMS.Interface.iDMReport;
        }

        //--

        public DataSet GetInspectionLotOrWaferCnt(
            string factory,
            string fromdate,
            string todate,
            bool bTypeFlag, 
            bool bAreaFlag
            )
        {
            return m_OBJ.GetInspectionLotOrWaferCnt(factory, fromdate, todate, bTypeFlag, bAreaFlag);
        }

        public DataTable GetInspectionDetail(string factory, string fromDate, string toDate, string model, bool bAreaFlag)
        {
            return m_OBJ.GetInspectionDetail(factory, fromDate, toDate, model, bAreaFlag);
        }

        public DataTable GetDmSummaryData(long[] Steps)
        {
            object[,] obj = m_OBJ.GetDmSummaryData(Steps);
            return new DefectMapAnalysis().ArrayToDataTable(obj);
        }

        public DataTable GetDmSummaryData_Comp(long[] Steps)
        {
            byte[] bytes = m_OBJ.GetDmSummaryData_Comp(Steps);
            return DACrux.Base.Util.CompressedBytesToObject(bytes) as DataTable;
        }

        public DataSet GetDataSheet(string fromDate, string toDate, string[] products, string[] stepids, string[] lots, string[] wafers, string[] xItem, string[] categorise)
        {
            byte[] bytes = m_OBJ.GetDataSheet_Comp(fromDate, toDate, products, stepids, lots, wafers, xItem, categorise);
            return DACrux.Base.Util.CompressedBytesToObject(bytes) as DataSet;
        }

        public DataTable GetDataTrend(string fromDate, string toDate, string[] products, string[] stepids, string[] lots, string[] wafers, string[] classnumber, string ordered, bool lastInsp)
        {
            byte[] bytes = m_OBJ.GetDataTrend_Comp(fromDate, toDate, products, stepids, lots, wafers, classnumber, ordered, lastInsp);
            return DACrux.Base.Util.CompressedBytesToObject(bytes) as DataTable;
        }

        public DataTable GetKillingRate(string fromDate, string toDate, string[] steps, string[] lotIDs, string[] waferIDs, string[] defectCodes, bool isNormalized)
        {
            byte[] bytes = m_OBJ.GetKillingRate_Comp(fromDate, toDate, steps, lotIDs, waferIDs, defectCodes, isNormalized);
            return DACrux.Base.Util.CompressedBytesToObject(bytes) as DataTable;
        }
    }
}
