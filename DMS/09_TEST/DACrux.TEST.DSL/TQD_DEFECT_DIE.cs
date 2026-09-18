using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.TEST.DSL
{
    public class TQD_DEFECT_DIE : Miracom.Middleware.QueryComponent
    {
        public TQD_DEFECT_DIE()
        {
            string connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];

            if (String.IsNullOrEmpty(connectID))
                throw new Exception("The connect ID nothing. Please, check app.config.");

            this.InitQueryComponent(connectID, "TQD_DEFECT_DIE.xml");
        }

        public int InsertData(string tableName, string test_wafer_seq)
        {
            return ExecuteNonQuery("INSERT_DATA", new string[] { tableName }, new string[] { test_wafer_seq });
        }

        /// <summary>
        /// BIN,X,Y 필드가 있는 PROGRAM 테이블을 가져옵니다.
        /// </summary>
        public string GetProgramBinTable(string program)
        {
            object obj = ExecuteScalar("SELECT_PROGRAM_BIN_TABLE", null, new string[] { program });

            if (obj == null)
                return null;

            return obj.ToString();
        }

        public DataTable GetKillingRate(
            string fromDate,
            string toDate,
            string[] steps,
            string[] lotIDs,
            string[] waferIDs,
            string[] highGecBinInfo,
            string[] defectCodes,
            string selectCodition,
            string defectiveDieCode,
            string failBinCode
            )
        {
            return GetDataTable_Numeric_IgnoreException("SELECT_DEFECT_DIE",
                Miracom.Middleware.ConvertBy.String,
                new string[] { 
                    selectCodition,
                    String.Format("'{0}'", String.Join("','", lotIDs)), 
                    String.Format("'{0}'", String.Join("','", waferIDs)), 
                    String.Format("'{0}'", String.Join("','", steps)), 
                    String.Format("'{0}'", String.Join("','", defectCodes)), 
                    defectiveDieCode, 
                    String.Format("'{0}'", String.Join("','", lotIDs)), 
                    String.Format("'{0}'", String.Join("','", waferIDs)), 
                    String.Format("'{0}'", String.Join("','", steps)), 
                    String.Format("'{0}'", String.Join("','", highGecBinInfo)), 
                    String.Format("'{0}'", String.Join("','", defectCodes)), 
                    failBinCode
                },
                new string[] { fromDate, toDate }
                );
        }

        public void DeleteData(string test_wafer_seq)
        {
            ExecuteNonQuery("DELETE_DATA", null, new string[] { test_wafer_seq });
        }
    }
}
