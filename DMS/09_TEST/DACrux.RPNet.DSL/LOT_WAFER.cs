using System;
using System.Data;

namespace DACrux.RPNet.DSL
{
    public class LOT_WAFER : Miracom.Middleware.QueryComponent
    {
        public LOT_WAFER()
        {
            string connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];

            if (String.IsNullOrEmpty(connectID))
            {
                throw new Exception("The connect ID nothing. Please, check app.config.");
            }

            this.InitQueryComponent(connectID, "RPNet.LOT_WAFER.xml");
        }

        public DataTable GetWaferList(string StartTime, string EndTime, string[] strFields, string[] strWhere)
        {
            string str1 = string.Join(",", strFields);
            string str2 = string.Empty;
            if (strWhere != null && strWhere.Length > 0)
            {
                for (int index = 0; index < strWhere.Length; ++index)
                    str2 = str2 + " AND " + strWhere[index];
            }

            DataTable dataTable = this.GetDataTable("GET_WAFER_LIST", new string[2]
            {
                str1.Replace("YIELD", "ROUND(YIELD,2) AS YIELD"),
                str2
            }, new string[2] { StartTime, EndTime });
            return dataTable;
        }

        public DataTable GetWaferInfo(string WaferSeq)
        {
            DataTable dataTable = (DataTable)null;

            dataTable = this.GetDataTable("GET_WAFER_INFO", (string[])null, new string[1]
            {
                WaferSeq
            });
            return dataTable;
        }

        public DataTable GetProgramLotSeq(string Device, string Lot)
        {
            DataTable dataTable = (DataTable)null;
            dataTable = this.GetDataTable("GET_PROGRAM", (string[])null, new string[2]
            {
              Device,
              Lot
            });
            return dataTable;
        }

        public DataTable GetWaferSeq(string Lot, string WaferId, string ProbeCnt)
        {
            DataTable dataTable = (DataTable)null;
            dataTable = this.GetDataTable("GET_WAFER_SEQ", (string[])null, new string[3]
            {
              Lot,
              WaferId,
              ProbeCnt
            });
            return dataTable;
        }

        public DataTable GetDistTestArea()
        {
            DataTable dataTable = (DataTable)null;
            dataTable = this.GetDataTable("GET_DIST_TESTAREA", (string[])null, (string[])null);
            return dataTable;
        }

        public DataTable GetDistProgram(string TESTAREA, string WHERE)
        {
            DataTable dataTable = (DataTable)null;
            string empty = string.Empty;
            dataTable = this.GetDataTable("GET_DIST_PROGRAM", new string[1]
            {
              WHERE.Trim().Length <= 0 ? "" : string.Format(" AND PROGRAM LIKE '%{0}%' ", (object) WHERE)
            }, new string[1] { TESTAREA });
            return dataTable;
        }

        public DataTable GetDistLot(string TESTAREA, string PROGRAM, string WHERE)
        {
            DataTable dataTable = (DataTable)null;
            string empty = string.Empty;
            dataTable = this.GetDataTable("GET_DIST_LOT", new string[1]
            {
              WHERE.Trim().Length <= 0 ? "" : string.Format(" AND LOT LIKE '%{0}%' ", (object) WHERE)
            }, new string[2] { TESTAREA, PROGRAM });
            return dataTable;
        }

        public DataTable GetDistWafer(string TESTAREA, string PROGRAM, string LOT, string WHERE)
        {
            DataTable dataTable = (DataTable)null;
            string empty = string.Empty;
            dataTable = this.GetDataTable("GET_DIST_WAFER", new string[1]
            {
              WHERE.Trim().Length <= 0 ? "" : string.Format(" AND WAFER_ID LIKE '%{0}%' ", (object) WHERE)
            }, new string[3] { TESTAREA, PROGRAM, LOT });
            return dataTable;
        }

        public void SaveMapShift(string device, string xShift, string yShift)
        {
            if (String.IsNullOrEmpty(device) || device == "NONE")
                return;

            Execute("SAVE_MAP_SHIFT", null, new string[] { device, xShift, yShift });
        }
    }
}
