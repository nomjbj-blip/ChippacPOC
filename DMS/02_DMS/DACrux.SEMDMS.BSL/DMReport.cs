using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DACrux.SEMDMS.Interface;
using Miracom.Middleware;
using System.Data;
using DACrux.SEMDMS.DSL;

namespace DACrux.SEMDMS.BSL
{
    public class DMReport
        : BaseComponent, iDMReport
    {
        #region [ Constructor ]
        public DMReport()
        {
        }
        #endregion [ Constructor ]

        #region [ Inspection Lot or Wafer Cnt ]
        public DataSet GetInspectionLotOrWaferCnt(
            string factory,
            string fromdate,
            string todate,
            bool bTypeFlag,
            bool bAreaFlag
            )
        {
            DataSet ds = new DataSet();
            DataTable dt = null;
            TQD_INSP_INFO obj = new TQD_INSP_INFO();
            dt = obj.GetInspectionLotOrWaferCnt(factory, fromdate, todate, bTypeFlag, bAreaFlag);
            dt.TableName = "PIVOT_DATA";
            ds.Tables.Add(dt);

            dt = obj.GetInspectionLotOrWaferRawData(factory, fromdate, todate, bAreaFlag);
            dt.TableName = "RAWDATA";
            ds.Tables.Add(dt);

            return ds;
        }

        public DataTable GetInspectionDetail(
            string factory,
            string fromDate,
            string toDate,
            string model,
            bool bAreaFlag
            )
        {
            TQD_INSP_INFO obj = new TQD_INSP_INFO();
            return obj.GetInspectionDetail(
                factory,
                fromDate,
                toDate,
                model,
                bAreaFlag
                );

        }
        #endregion [ Inspection Lot or Wafer Cnt ]

        #region [ Dm Summary Data ]

        public byte[] GetDmSummaryData_Comp(
            long[] Steps
            )
        {
            return DACrux.Base.Util.ObjectToCompressedBytes(
                GetDmSummaryData(Steps)
                );
        }

        public object[,] GetDmSummaryData(
            long[] Steps
            )
        {
            TQD_INSP_INFO obj = new TQD_INSP_INFO();
            return obj.GetInspectionDefectSummaryData(Steps);
        }
        #endregion [ Dm Summary Data ]

        #region [ Data Sheet ]
        public byte[] GetDataSheet_Comp(
            string fromDate,
            string toDate,
            string[] products,
            string[] stepids,
            string[] lots,
            string[] wafers,
            string[] xItem,
            string[] categorise
            )
        {
            return DACrux.Base.Util.ObjectToCompressedBytes(
                GetDataSheet(fromDate, toDate, products, stepids, lots, wafers, xItem, categorise)
                );
        }

        //--------------------------------------------------------------------------------------------------------------

        private object GetDataSheet(
            string fromDate,
            string toDate,
            string[] products,
            string[] stepids,
            string[] lots,
            string[] wafers,
            string[] xItem,
            string[] categorise
            )
        {
            DataSet ds = new DataSet();
            TQD_REVIEW_SUM oReviewSum = new TQD_REVIEW_SUM();
            TQD_DEFECT_TYPE oDefectType = new TQD_DEFECT_TYPE();
            DataTable dtDefectType = oDefectType.SelectDefectList();
            string[] pivotValue = new string[categorise.Length];
            for (int idx = 0; idx < categorise.Length; idx++)
            {
                DataRow[] rows = dtDefectType.Select(String.Format("[CLASSNUMBER] = '{0}'", categorise[idx]));
                pivotValue[idx] = String.Format("'{0}' AS \"{1}\"", rows[0]["CLASSNUMBER"], rows[0]["NAME"].ToString().Replace(' ', '_').ToUpper());
            }

            DataTable dt = oReviewSum.GetDataSheet01(fromDate, toDate, products, stepids, lots, wafers, xItem, pivotValue);
            dt.TableName = "DEFECT COUNT SHEET";
            AddTotal(dt);
            ds.Tables.Add(dt);

            dt = oReviewSum.GetDataSheet02(fromDate, toDate, products, stepids, lots, wafers, xItem, pivotValue);
            dt.TableName = "DEFECTIVE DIE SHEET";
            AddTotal(dt);
            ds.Tables.Add(dt);

            return ds;
        }

        private void AddTotal(
            DataTable dt
            )
        {
            int stepSeqIdx = dt.Columns["STEP_SEQ"].Ordinal;
            double sumValue = 0;
            double value = double.NaN;
            foreach (DataRow row in dt.Rows)
            {
                sumValue = 0;
                for (int colidx = dt.Columns.Count - 1; colidx > stepSeqIdx; colidx--)
                {
                    if (!double.TryParse(row[dt.Columns[colidx].ColumnName].ToString(), out value))
                        value = 0;

                    sumValue += value;
                }
                row["TOTAL"] = sumValue;
            }
        }
        #endregion [ Data Sheet ]

        #region [ DM Data Trend ]
        public byte[] GetDataTrend_Comp(
            string fromDate,
            string toDate,
            string[] products,
            string[] stepids,
            string[] lots,
            string[] wafers,
            string[] classnumber,
            string ordered,
            bool lastInsp
            )
        {
            return Base.Util.ObjectToCompressedBytes(
                GetDataTrend(fromDate, toDate, products, stepids, lots, wafers, classnumber, ordered, lastInsp)
                );
        }

        //--------------------------------------------------------------------------------------------------------------

        private DataTable GetDataTrend(
            string fromDate,
            string toDate,
            string[] products,
            string[] stepids,
            string[] lots,
            string[] wafers,
            string[] classnumber,
            string ordered,
            bool lastInsp
            )
        {
            TQD_INSP_SUM oInspSum = new TQD_INSP_SUM();
            DataTable dt = oInspSum.GetDataSheet02(fromDate, toDate, products, stepids, lots, wafers, classnumber, ordered);
            if (lastInsp)
            {
                DataRow[] rows = dt.Select("[RN] = 1");
                if (rows != null || rows.Length > 0)
                    dt = rows.CopyToDataTable<DataRow>();
            }

            return dt;
        }
        #endregion

        #region [ Killing Rate ]
        public byte[] GetKillingRate_Comp(
            string fromDate,
            string toDate,
            string[] steps,
            string[] lotIDs,
            string[] waferIDs,
            string[] defectCodes,
            bool isNormalized
            )
        {
            return DACrux.Base.Util.ObjectToCompressedBytes(
                GetKillingRate(fromDate, toDate, steps, lotIDs, waferIDs, defectCodes, isNormalized)
                );
        }

        public DataTable GetKillingRate(
            string fromDate,
            string toDate,
            string[] steps,
            string[] lotIDs,
            string[] waferIDs,
            string[] defectCodes,
            bool isNormalized
            )
        {
            DataTable dt = null;
            DataTable dtHighGec = null;
            TQD_DEFECT_TYPE oDefectType = new TQD_DEFECT_TYPE();
            TEST.DSL.TQD_DEFECT_DIE oDefectDie = new TEST.DSL.TQD_DEFECT_DIE();
            dt = oDefectType.SelectDefectCode();

            List<String> highGecBin = new List<String>();
            TQD_STEP oStep = new TQD_STEP();
            dtHighGec = oStep.GetWaferBinInfo(
                steps,
                lotIDs,
                waferIDs
                );
            if (dtHighGec == null || dtHighGec.Rows.Count <= 0)
            {
                highGecBin.Add("1");
            }
            else
            {
                foreach (DataRow row in dtHighGec.Rows)
                {
                    highGecBin.Add(row["BIN"].ToString());
                }
            }

            List<String> lstDefectivePivotValues = new List<String>();
            List<String> lstFailBinPivotValues = new List<String>();

            List<String> lstDefectiveDieFilter = new List<String>();
            List<String> lstFailBinFilter = new List<String>();
            List<String> lstKillRate = new List<String>();
            List<String> lstYieldLoss = new List<String>();

            DataRow[] rows = dt.Select(String.Format("[CLASSNUMBER] IN ('{0}')", String.Join("','", defectCodes)));
            foreach (DataRow row in rows)
            {
                lstDefectivePivotValues.Add(String.Format("'{0}' AS \"DEFECTIVE_{1}\"", row["CLASSNUMBER"].ToString(), row["NAME"].ToString()));
                lstFailBinPivotValues.Add(String.Format("'{0}' AS \"FAILDIE_{1}\"", row["CLASSNUMBER"].ToString(), row["NAME"].ToString()));

                lstDefectiveDieFilter.Add(String.Format("nvl(\"DEFECTIVE_{0}\", 0) as \"DEFECTIVE_{0}\" ", row["NAME"].ToString()));
                lstFailBinFilter.Add(String.Format("nvl(\"FAILDIE_{0}\", 0) as \"FAILDIE_{0}\"", row["NAME"].ToString()));
                /// 분자: FILE DIE, 분모: Defective die, 분모가 0인 경우에는 0 값을 Display 
                lstKillRate.Add(String.Format("CASE WHEN \"FAILDIE_{0}\" IS NULL AND \"DEFECTIVE_{0}\" IS NOT NULL THEN 0 ELSE ROUND(\"FAILDIE_{0}\" / NULLIF(\"DEFECTIVE_{0}\", 0), 10) END AS \"KILL_{0}\"", row["NAME"].ToString()));
                if (isNormalized)
                    lstYieldLoss.Add(String.Format("CASE WHEN \"FAILDIE_{0}\" IS NULL AND \"DEFECTIVE_{0}\" IS NOT NULL THEN 0 ELSE ROUND(DEFECTIVE_DIE * (\"FAILDIE_{0}\" / CLASSIFIED_DEFECTS) / NULLIF(NETDIE, 0), 10) END AS \"YIELDLOSS_{0}\"", row["NAME"].ToString()));
                else
                    lstYieldLoss.Add(String.Format("CASE WHEN \"FAILDIE_{0}\" IS NULL AND \"DEFECTIVE_{0}\" IS NOT NULL THEN 0 ELSE ROUND(\"FAILDIE_{0}\" / NULLIF(NETDIE, 0), 10) END AS \"YIELDLOSS_{0}\"", row["NAME"].ToString()));
            }
            lstDefectivePivotValues.Add(String.Format("'SUM' AS \"DEFECTIVE_SUM\""));
            lstFailBinPivotValues.Add(String.Format("'SUM' AS \"FAILBIN_SUM\""));
            lstDefectiveDieFilter.Add("NVL(DEFECTIVE_SUM, 0) AS DEFECTIVE_SUM");
            lstFailBinFilter.Add("NVL(FAILBIN_SUM, 0) AS FAILBIN_SUM");

            dt = oDefectDie.GetKillingRate(
                fromDate,
                toDate,
                steps,
                lotIDs,
                waferIDs,
                highGecBin.ToArray(),
                defectCodes,
                String.Format("{0},{1},{2},{3}", String.Join(",", lstDefectiveDieFilter), String.Join(",", lstFailBinFilter), String.Join(",", lstKillRate), String.Join(",", lstYieldLoss)),
                String.Format("{0}", String.Join(",", lstDefectivePivotValues)),
                String.Format("{0}", String.Join(",", lstFailBinPivotValues))
                );
            dt.Columns.AddRange(new DataColumn[] { new DataColumn("KILL_SUM", typeof(double)), new DataColumn("YIELDLOSS_SUM", typeof(double)) });
            dt.TableName = "KILLING_RATE";

            int idx = dt.Columns.IndexOf(String.Format("KILL_{0}", rows[rows.Length - 1]["NAME"]));
            dt.Columns["YIELDLOSS_SUM"].SetOrdinal(idx + 1);

            idx = dt.Columns.IndexOf("FAILBIN_SUM");
            dt.Columns["KILL_SUM"].SetOrdinal(idx + 1);


            foreach (DataRow row in dt.Rows)
            {
                double dKillSum = 0d;
                double dYieldSum = 0d;
                for (int colIdx = idx; colIdx < dt.Columns.Count; colIdx++)
                {
                    if (dt.Columns[colIdx].ColumnName.Contains("_SUM"))
                        continue;

                    if (dt.Columns[colIdx].ColumnName.Contains("KILL_"))
                        dKillSum += Base.Convert.doubleParse(row[colIdx].ToString());

                    if (dt.Columns[colIdx].ColumnName.Contains("YIELDLOSS_"))
                        dYieldSum += Base.Convert.doubleParse(row[colIdx].ToString());
                }
                row["KILL_SUM"] = dKillSum;
                row["YIELDLOSS_SUM"] = dYieldSum;
            }
            return dt;
        }
        #endregion [ Killing Rate ]
    }
}
