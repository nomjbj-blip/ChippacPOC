using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;

namespace DACrux.TEST.DSL
{
    public class TD_TABLE
        : Miracom.Middleware.QueryComponent
    {
        public static readonly string TABLESPACE_DATA = "TS_TEST_TBL";
        public static readonly string TABLESPACE_INDEX = "TS_TEST_IDX";

        public enum ParaCol
        {
            FACTORY,
            PROGRAM,
            PROGRAM_REV,
            PARAM_INDEX,
            PARAM_NAME,
            PARAM_TYPE,
            PARAM_DESC,
            CREATE_USER,
            TABLE_NAME,
            DECIMAL_PLACES,
            RUNTIME_DEFINED,
            UOM,
            USE_FLAG
        }

        public TD_TABLE()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TD_TABLE.xml");
                this.ClearAllPools();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 새로운 테이블을 만듭니다. PK 와 VIEW 도 같이 생성됩니다.
        /// </summary>
        public string CreateNewTable(string factory, string programName, int programRev, int tableIndex, List<string> columnList)
        {
            string suffix = GetTableSuffix(tableIndex);
            string tableName = String.Format("TD_{0}{1}", programName, suffix);
            string viewName = String.Format("VW_{0}{1}", programName, suffix);

            // TABLE 생성
            if (ExistTable(tableName) == false)
            {
                CreateTDTable(tableName, columnList);

                // PK 생성
                CreateTDPrimaryKey(tableName);
            }

            // VIEW 생성
            //if (ExistVIEW(viewName) == false)
            //{
            //    CreateTDView(columnList, tableName, viewName);
            //}

            // TQP_DATA_TABLES 에 추가
            TQP_DATA_TABLES obj = new TQP_DATA_TABLES();
            obj.InsertData(factory, programName, tableName);

            // TQP_PARASPEC 에 추가
            int paraColCount = Enum.GetNames(typeof(ParaCol)).Length;
            string[,] arr = new string[columnList.Count, paraColCount];

            for (int i = 0; i < columnList.Count; i++)
            {
                SetParamSpecRowData(arr, factory, programName, programRev, tableName, columnList[i], i, i + 1);
            }

            TQP_PARASPEC par = new TQP_PARASPEC();
            par.CreateParaSpec(arr);

            return tableName;
        }

        private static void SetParamSpecRowData(string[,] arr, string factory, string programName, int programRev, string tableName, string paramName, int rowIndex, int paramIndex)
        {
            arr[rowIndex, (int)ParaCol.FACTORY] = factory;
            arr[rowIndex, (int)ParaCol.PROGRAM] = programName;
            arr[rowIndex, (int)ParaCol.PROGRAM_REV] = programRev.ToString();
            arr[rowIndex, (int)ParaCol.PARAM_INDEX] = paramIndex.ToString();
            arr[rowIndex, (int)ParaCol.PARAM_NAME] = paramName;
            arr[rowIndex, (int)ParaCol.PARAM_TYPE] = "FLOAT";
            arr[rowIndex, (int)ParaCol.PARAM_DESC] = String.Empty;
            arr[rowIndex, (int)ParaCol.CREATE_USER] = "AUTO";
            arr[rowIndex, (int)ParaCol.TABLE_NAME] = tableName;
            arr[rowIndex, (int)ParaCol.DECIMAL_PLACES] = "0";
            arr[rowIndex, (int)ParaCol.RUNTIME_DEFINED] = "1";
            arr[rowIndex, (int)ParaCol.UOM] = String.Empty;
            arr[rowIndex, (int)ParaCol.USE_FLAG] = "Y";
        }

        /// <summary>
        /// 기존 테이블에 컬럼을 추가합니다.
        /// </summary>
        public void AppendColumn(string factory, string programName, int programRev, int tableIndex, List<string> columnList)
        {
            string suffix = GetTableSuffix(tableIndex);
            string tableName = String.Format("TD_{0}{1}", programName, suffix);

            // 컬럼 추가
            AppendColumnAtTDTable(tableName, columnList);

            // TQP_PARASPEC 에 추가
            TQP_PARASPEC par = new TQP_PARASPEC();
            int maxParamIndex = par.GetMaxParamIndex(factory, programName, tableName);

            int paraColCount = Enum.GetNames(typeof(ParaCol)).Length;
            string[,] arr = new string[columnList.Count, paraColCount];

            for (int i = 0; i < columnList.Count; i++)
            {
                SetParamSpecRowData(arr, factory, programName, programRev, tableName, columnList[i], i, maxParamIndex + i + 1);
            }

            par.CreateParaSpec(arr);
        }

        /// <summary>
        /// 데이터가 없는 테이블 리스트를 가져옵니다.
        /// </summary>
        public DataSet GetTableSchema(string factory, string programName)
        {
            TQP_DATA_TABLES obj = new TQP_DATA_TABLES();
            string[] tableNames = obj.GetTableNames(factory, programName);

            DataSet ds = new DataSet();

            foreach (string tableName in tableNames)
            {
                string query = String.Format("SELECT * FROM DMSMGR.{0} WHERE 1=2", tableName);

                DataTable dt = GetDataTable("EXECUTE", new string[] { query }, null);
                dt.TableName = tableName;
                ds.Tables.Add(dt.Copy());
            }

            return ds;
        }

        public DataSet GetTableSchema(
            string factory,
            string programName,
            string waferseq
            )
        {
            TQP_DATA_TABLES obj = new TQP_DATA_TABLES();
            string[] tableNames = obj.GetTableNames(factory, programName);
            DataSet ds = new DataSet();
            DataTable dt = null;

            foreach (string tableName in tableNames)
            {
                string query = String.Format("SELECT * FROM DMSMGR.{0} WHERE WAFER_SEQ = '{1}'", tableName, waferseq);

                dt = GetDataTable("EXECUTE", new string[] { query }, null);
                dt.TableName = tableName;
                ds.Tables.Add(dt.Copy());
            }

            return ds;
        }

        /// <summary>
        /// 데이터를 추가합니다.
        /// </summary>
        public void InsertData(DataTable dt)
        {
            ExecuteTable(dt);
        }

        /// <summary>
        /// 테이블 접미어의 문자열(A,B,C...)을 가져옵니다.<para />
        /// ex) index = 0: 없음, 1: A, 2: B, 3: C, ...
        /// </summary>
        public static string GetTableSuffix(int tableIndex)
        {
            return (tableIndex > 0) ? ((char)('A' + tableIndex - 1)).ToString() : String.Empty;
        }

        private void CreateTDTable(string tableName, List<string> columnList)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(
@"
CREATE TABLE DMSMGR.{0}
(
WAFER_SEQ     NUMBER,
DIE_NUM       NUMBER,
DIEPROBE_CNT  NUMBER,
", tableName);

            foreach (string column in columnList)
            {
                sb.Append(String.Format("{0} NUMBER,", column));
            }

            // 쉼표 제거
            sb.Remove(sb.Length - 1, 1);
            sb.AppendLine(") TABLESPACE " + TABLESPACE_DATA);

            ExecuteNonQuery("EXECUTE", new string[] { sb.ToString() }, null);
        }

        private void AppendColumnAtTDTable(string tableName, List<string> columnList)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"ALTER TABLE DMSMGR.{0} ADD (", tableName);

            foreach (string column in columnList)
            {
                sb.Append(String.Format("{0} NUMBER,", column));
            }

            // 쉼표 제거
            sb.Remove(sb.Length - 1, 1);
            sb.AppendLine(")");

            ExecuteNonQuery("EXECUTE", new string[] { sb.ToString() }, null);
        }

        private void CreateTDPrimaryKey(string tableName)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(String.Format(
@"
ALTER TABLE DMSMGR.{0} ADD (
PRIMARY KEY
(WAFER_SEQ, DIE_NUM)
USING INDEX
TABLESPACE {1})
", tableName, TABLESPACE_INDEX));

            ExecuteNonQuery("EXECUTE", new string[] { sb.ToString() }, null);
        }

//        private void CreateTDView(List<string> columnList, string tableName, string viewName)
//        {
//            StringBuilder sb = new StringBuilder();

//            sb.AppendFormat(
//@"
//CREATE OR REPLACE FORCE VIEW DMSMGR.{0}
//(
//TEST_AREA,
//PROGRAM,
//LOT,
//LOT_END_TIME,
//LOT_SEQ,
//WAFER_ID,
//PROBE_CNT,
//WAFER_SEQ,
//DIE_NUM,
//DIEPROBE_CNT,
//", viewName);

//            foreach (string column in columnList)
//            {
//                sb.Append(String.Format("{0},", column));
//            }

//            // 쉼표 제거
//            sb.Remove(sb.Length - 1, 1);
//            sb.AppendFormat(
//@"
//)
//AS
//SELECT L.TEST_AREA,
//    L.PROGRAM,
//    L.LOT_ID,
//    L.END_TIME LOT_END_TIME,
//    L.LOT_SEQ,
//    W.WAFER_ID,
//    W.PROBE_CNT,
//    A.*
//FROM TQP_LOT L, TQP_WAFER W, {0} A
//WHERE L.LOT_SEQ = W.LOT_SEQ
//    AND W.WAFER_SEQ = A.WAFER_SEQ
//    AND W.MERGE_FLAG = 'Y'
//    AND A.DIEPROBE_CNT = 0
//", tableName);

//            ExecuteNonQuery("EXECUTE", new string[] { sb.ToString() }, null);
//        }

        /// <summary>
        /// 지정된 테이블의 데이터를 삭제 합니다.
        /// </summary>
        public void DeleteData(string waferSeq, List<string> tableList)
        {
            foreach (string tableName in tableList)
                DeleteData(waferSeq, tableName);
        }

        /// <summary>
        /// 지정된 테이블의 데이터를 삭제 합니다.
        /// </summary>
        public void DeleteData(string waferSeq, string tableName)
        {
            ExecuteNonQuery("DELETE_DATA", new string[] { tableName }, new string[] { waferSeq });
        }

        /// <summary>
        /// 지정된 테이블의 데이터를 삭제 합니다.
        /// </summary>
        public void DeleteData(string[] waferSeqArr, List<string> tableList)
        {
            foreach (string tableName in tableList)
                DeleteData(waferSeqArr, tableName);
        }

        /// <summary>
        /// 지정된 테이블의 데이터를 삭제 합니다.
        /// </summary>
        public void DeleteData(string[] waferSeqArr, string tableName)
        {
            if (waferSeqArr == null || waferSeqArr.Length == 0)
                return;
            
            ExecuteNonQuery("DELETE_DATA_01", new string[] { tableName, String.Join(",", waferSeqArr) }, null);
        }

        //------------------------------------------------------------------------------------------

        public Dictionary<string, decimal> GetBinCount(
            string ttable,
            decimal waferSeq
            )
        {
            DataTable dt = this.GetDataTable(
                "GET_RAW_DATA",
                new string[] { ttable },
                new string[] { waferSeq.ToString() }
                );

            if (dt == null || dt.Rows.Count == 0)
                return null;

            Dictionary<string, decimal> dic = new Dictionary<string, decimal>();
            foreach (DataRow row in dt.Rows)
            {
                dic.Add((string)row["BIN"], (decimal)row["CNT"]);
            }

            return dic;
        }

        //--

        public DataTable GetBinXYData(
            string[] ttable,
            string program,
            string wafer, 
            string minX,
            string minY
            )
        {
            if (ttable.Length == 1)
            {
                return GetDataTable(
                    "GET_BIN_XY_DATA_SINGLE",
                    new string[] { ttable[0] },
                    new string[] { wafer, program, minX, minY }
                    );
            }
            else
            {
                return GetDataTable(
                    "GET_BIN_XY_DATA_MULTI",
                    new string[] { ttable[0], ttable[1] },
                    new string[] { wafer, program, minX, minY }
                    );
            }
        }

        public DataTable GetBinXYMinMaxData(
            string[] ttable,
            string wafer
            )
        {
            if (ttable.Length == 1)
            {
                return GetDataTable(
                    "GET_BIN_XY_MINMAX_SINGLE",
                    new string[] { ttable[0] },
                    new string[] { wafer }
                    );
            }
            else
            {
                return GetDataTable(
                    "GET_BIN_XY_MINMAX_MULTI",
                    new string[] { ttable[0], ttable[1] },
                    new string[] { wafer }
                    );
            }
        }

        //--


        public DataTable GetPCMData(
            string factory,
            string[] lots,
            string[] wafers
            )
        {
            if (wafers == null || wafers.Length <= 0)
                return null;

            //TQP_WAFER oWafer = new TQP_WAFER();
            //string[] programArr = oWafer.GetProgram(wafers);
            TQP_LOT oLot = new TQP_LOT();
            string[] programArr = oLot.GetProgram(lots);

            if (programArr == null || programArr.Length <= 0)
                return null;

            TQP_PARASPEC obj = new TQP_PARASPEC();
            DataTable dtPara = obj.GetParaSpec(programArr[0]);
            if (dtPara == null || dtPara.Rows.Count <= 0)
                return null;

            DataTable dtTable = dtPara.DefaultView.ToTable(true, "TABLE_NAME");

            StringBuilder columns = new StringBuilder();
            string tables = null;
            string tableJoin = null;
            string lotIn = null;
            string waferIn = null;

            for (int i = 0; i < dtTable.Rows.Count; i++)
            {
                char alias = (char)('A' + i);
                DataRow[] drs = dtPara.Select(String.Format("[TABLE_NAME] = '{0}'", dtTable.Rows[i][0]));
                for (int idx = 0; idx < drs.Length; idx++)
                {
                    columns.AppendFormat("{0}.{1},",  alias, drs[idx][1]);
                }

                tables += String.Format("{0} {1}", dtTable.Rows[i][0], alias);

                if (i < dtTable.Rows.Count - 1)
                    tables += ",";

                if (i > 0)
                    tableJoin += String.Format("AND {0}.WAFER_SEQ = {1}.WAFER_SEQ(+) AND {0}.DIE_NUM = {1}.DIE_NUM(+)", (char)(alias - 1), alias);
            }
            columns.Remove(columns.Length - 1, 1);

            lotIn = String.Join(",", lots);
            waferIn = String.Join(",", wafers);

            return GetDataTable_Numeric_IgnoreException(
                "GET_PCM_DATA",
                Miracom.Middleware.ConvertBy.String,
                new string[] { columns.ToString(), tables, tableJoin, lotIn, waferIn }, 
                new string[] { factory}
                );
        }

        //--

        public DataTable GetPCMStatistics(
            string factory,
            string[] lotids,
            string tablename,
            string parameter,
            string[] wafers
            )
        {
            return this.GetDataTable(
                "GET_PCM_STATISTICS",
                new string[] { 
                    parameter, parameter, parameter, parameter, parameter, 
                    parameter, parameter, parameter, tablename,
                    String.Format("'{0}'", String.Join("','", lotids)),
                    String.Format("'{0}'", String.Join("','", wafers)),
                    parameter, parameter
                },
                new string[] { factory, parameter }
                );
        }

        //--

        public DataTable GetTestData(
            string factory,
            string tablename,
            string[] lots,
            string[] wafers
            )
        {
            return this.GetDataTable(
                "GET_TEST_DATA",
                new string[] { 
                    tablename, 
                    String.Format("'{0}'", String.Join("','", lots)), 
                    String.Format("'{0}'", String.Join("','", wafers)) 
                },
                new string[] { factory }
                );
        }

        //--

        public DataTable GetTestStatistics(
            string factory,
            string[] lotids,
            string tablename,
            string parameter,
            string[] wafers
            )
        {
            return this.GetDataTable(
                "GET_TEST_STATISTICS",
                new string[] { 
                    parameter, parameter, parameter, parameter, parameter, 
                    parameter, parameter, parameter, tablename,
                    String.Format("'{0}'", String.Join("','", lotids)),
                    String.Format("'{0}'", String.Join("','", wafers)),
                    parameter, parameter
                },
                new string[] { factory, parameter }
                );
        }

        /// <summary>
        /// WAFER_SEQ에 대한 RAW DATA를 가져옵니다. 여러 테이블인 경우 모든 데이터를 가져옵니다.
        /// </summary>
        public DataTable GetAllRawData(params string[] waferSeqArr)
        {
            #region Sample
            /*
                SELECT A.WAFER_SEQ,W.WAFER_ID,A.DIE_NUM,X,Y--?? ALL PARAMETERS
                FROM
                TQP_WAFER W,
                TD_P_TL13_5P A,
                TD_P_TL13_5PA B
                WHERE W.WAFER_SEQ = A.WAFER_SEQ
                AND A.WAFER_SEQ = B.WAFER_SEQ(+) AND A.DIE_NUM = B.DIE_NUM(+)
                AND A.WAFER_SEQ IN (434448)
                ORDER BY A.WAFER_SEQ, A.DIE_NUM
             */
            #endregion

            if (waferSeqArr == null || waferSeqArr.Length == 0)
                return null;

            // PROGRAM이 여러개더라도 첫번째 PROGRAM으로 처리
            TQP_WAFER waf = new TQP_WAFER();
            string[] programArr = waf.GetProgram(waferSeqArr);

            if (programArr == null || programArr.Length == 0)
                return null;

            // PARAMETER 가져오기
            TQP_PARASPEC obj = new TQP_PARASPEC();
            DataTable paraDt = obj.GetParaSpec(programArr[0]);

            if (paraDt == null || paraDt.Rows.Count == 0)
                return null;

            // X,Y는 실행 쿼리에 들어 있으므로 있는 경우 삭제
            DataRow[] rows = paraDt.Select("PARAM_NAME IN ('X','Y')");

            if (rows != null && rows.Length > 0)
            {
                foreach (DataRow row in rows)
                    paraDt.Rows.Remove(row);
            }

            // TABLE 명 가져오기
            DataTable tableDt = paraDt.DefaultView.ToTable(true, "TABLE_NAME");

            StringBuilder columns = new StringBuilder();
            string tables = null; // TD_P_TL13_5P A,
            string tableJoin = null; // AND A.WAFER_SEQ = B.WAFER_SEQ(+) AND A.DIE_NUM = B.DIE_NUM(+)
            string waferIn = null; // wafer seq in query

            Dictionary<string, char> dic = new Dictionary<string, char>();

            for (int i = 0; i < tableDt.Rows.Count; i++)
                dic.Add(tableDt.Rows[i][0].ToString(), (char)('A' + i));

            for (int i = 0; i < paraDt.Rows.Count; i++)
                columns.AppendFormat("{0}.{1},", dic[paraDt.Rows[i][0].ToString()], paraDt.Rows[i][1]);

            columns.Remove(columns.Length - 1, 1);

            for (int i = 0; i < tableDt.Rows.Count; i++)
            {
                string tableName = tableDt.Rows[i][0].ToString();

                char alias = dic[tableName];
                tables += String.Format("{0} {1}", tableDt.Rows[i][0], alias);

                if (i < tableDt.Rows.Count - 1)
                    tables += ",";

                if (i > 0)
                    tableJoin += String.Format("AND {0}.WAFER_SEQ = {1}.WAFER_SEQ(+) AND {0}.DIE_NUM = {1}.DIE_NUM(+)", (char)(alias - 1), alias);
            }

            waferIn = String.Join(",", waferSeqArr);

            return GetDataTable_Numeric_IgnoreException("SELECT_ALL_DATA", Miracom.Middleware.ConvertBy.String, new string[] { columns.ToString(), tables, tableJoin, waferIn }, null);
        }

        /// <summary>
        /// WAFER_SEQ에 대한 RAW DATA를 가져옵니다. 여러 테이블인 경우 모든 데이터를 가져옵니다.
        /// </summary>
        public DataTable GetAllRawDataOnlyPara(params string[] waferSeqArr)
        {
            DataTable dt = GetAllRawData(waferSeqArr);

            if (dt == null)
                return null;

            foreach (string col in new string[] { "CHECKBOX", "WAFER_ID", "NUM", "WAFER_SEQ", "DIE_NUM" })
            {
                if (dt.Columns.Contains(col))
                    dt.Columns.Remove(col);
            }

            return dt;
        }

        /// <summary>
        /// TD_ 테이블에 대해 MERGE 구문을 실행합니다.
        /// </summary>
        /// <param name="dt"></param>
        public void ExecuteMerge(DataTable dt)
        {
            string[] pkArr = { "WAFER_SEQ", "DIE_NUM" };

            // MERGE 쿼리 생성
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" MERGE INTO {0}", dt.TableName);
            sb.AppendFormat(" USING DUAL");
            sb.AppendFormat(" ON (");

            for (int i = 0; i < pkArr.Length; i++)
            {
                if (i == 0)
                    sb.AppendFormat(" {0} = :{0}", pkArr[i]);
                else
                    sb.AppendFormat("   AND {0} = :{0}", pkArr[i]);
            }

            sb.AppendFormat(" )");
            sb.AppendFormat(" WHEN MATCHED THEN");
            sb.AppendFormat(" UPDATE SET");

            for (int c = pkArr.Length; c < dt.Columns.Count; c++)
                sb.AppendFormat(" {0} = :{0},", dt.Columns[c].ColumnName);

            sb.Remove(sb.Length - 1, 1); // 쉼표 제거

            sb.AppendFormat(" WHEN NOT MATCHED THEN");
            sb.AppendFormat(" INSERT (");

            for (int c = 0; c < dt.Columns.Count; c++)
                sb.AppendFormat(" {0},", dt.Columns[c].ColumnName);

            sb.Remove(sb.Length - 1, 1); // 쉼표 제거
            
            sb.AppendFormat(" ) VALUES (");

            for (int c = 0; c < dt.Columns.Count; c++)
                sb.AppendFormat(" :{0},", dt.Columns[c].ColumnName);

            sb.Remove(sb.Length - 1, 1); // 쉼표 제거

            sb.AppendFormat(" )");

            // 쿼리와 DataTable 전송. (OracleParameter는 Column 정보를 이용해서 내부적으로 생성)
            ExecuteTable(sb.ToString(), dt);
        }

        /// <summary>
        /// 해당 테이블에 데이터가 존재하는지를 가져옵니다.
        /// </summary>
        public bool ExistsData(string tableName)
        {
            object obj = ExecuteScalar("EXISTS_DATA", new string[] { tableName }, null);

            if (obj == null || obj == DBNull.Value)
                return false;

            return true;
        }

        /// <summary>
        /// TD 테이블을 삭제 합니다.
        /// </summary>
        public void DropTable(string tableName)
        {
            string viewName = String.Format("VW_{0}", tableName.Substring(3));

            try
            {
                ExecuteNonQuery("DROP_VIEW", new string[] { viewName }, null);
            }
            catch { }

            ExecuteNonQuery("DROP_TABLE_01", new string[] { tableName }, null);
            ExecuteNonQuery("DROP_TABLE_02", new string[] { tableName }, null);
        }

        public DataTable GetTestParameters(
            string parameters, 
            string table, 
            string[] wafers
            )
        {
            return GetDataTable_Numeric_IgnoreException("GET_TEST_PARAMETER", Miracom.Middleware.ConvertBy.String, new string[] { parameters, table, String.Format("'{0}'", String.Join("','", wafers)) }, null);

        }

        public DataTable GetTestRawData(
            string table,
            string wafers
            )
        {
            return GetDataTable("GET_TEST_RAWDATA", new string[] { table, String.Format("{0}", wafers) }, null);

        }

        public DataTable GetTestRawData(
            string factory,
            string[] selectCondition,
            string[] tablename,
            string[] joinCondition,
            string[] lotids,
            string[] wafers
            )
        {
            return GetDataTable(
                "GET_TEST_DATA",
                new string[] { 
                    String.Format("{0}", String.Join(",", selectCondition)), 
                    String.Format("{0}", String.Join(",", tablename)), 
                    String.Format("{0}", String.Join(" AND ", joinCondition)),
                    String.Format("{0}", String.Join(",", lotids)), 
                    String.Format("{0}", String.Join(",", wafers))
                },
                new string[] { factory }
                );

        }

        public bool ExistTable(string strTable)
        {
            DataTable dt = this.GetDataTable("EXISTS_TABLE", null, new string[] { strTable });
            if (dt == null || dt.Rows.Count <= 0)
                return false;
            else
                return true;
        }

        public bool ExistVIEW(string strTable)
        {
            DataTable dt = this.GetDataTable("EXISTS_VIEW", null, new string[] { strTable });
            if (dt == null || dt.Rows.Count <= 0)
                return false;
            else
                return true;
        }

        public DataTable GetDynamic(string strQuery)
        {
            return GetDataTable("SELECT_DYNAMIC", new string[] { strQuery }, null);
        }

        public int SetTransaction(string strQuery)
        {
            return this.ExecuteNonQuery("SELECT_DYNAMIC", new string[] { strQuery }, null);
        }

        public DataTable GetPatternSearch_XYList(long waferSeq)
        {
            if (waferSeq == 0)
                return null;

            TQP_WAFER waf = new TQP_WAFER();
            string program = waf.GetProgram(waferSeq.ToString());

            if (String.IsNullOrEmpty(program))
                return null;

            TQP_DATA_TABLES tbl = new TQP_DATA_TABLES();
            string[] tableNames = tbl.GetTableNames(program);

            string tables = null; // TD_P_TL13_5P A,
            string tableJoin = null; // AND A.WAFER_SEQ = B.WAFER_SEQ(+) AND A.DIE_NUM = B.DIE_NUM(+)

            Dictionary<string, char> dic = new Dictionary<string, char>();

            for (int i = 0; i < tableNames.Length; i++)
                dic.Add(tableNames[i], (char)('A' + i));

            for (int i = 0; i < tableNames.Length; i++)
            {
                char alias = dic[tableNames[i]];
                tables += String.Format("{0} {1}", tableNames[i], alias);

                if (i < tableNames.Length - 1)
                    tables += ", ";

                if (i > 0)
                    tableJoin += String.Format("AND {0}.WAFER_SEQ = {1}.WAFER_SEQ(+) AND {0}.DIE_NUM = {1}.DIE_NUM(+)", (char)(alias - 1), alias);
            }

            StringBuilder query = new StringBuilder();
            query.AppendLine("SELECT X * 65536 + Y XY");
            query.AppendLine("FROM " + tables);
            query.AppendLine("WHERE 1 = 1");
            query.AppendLine(tableJoin);
            query.AppendLine("AND A.WAFER_SEQ = " + waferSeq);
            query.AppendLine("AND NOT EXISTS (");
            query.AppendLine(String.Format("SELECT * FROM TQP_BINDESC WHERE PROGRAM = '{0}' AND HIGH_GEC = 'Y' AND BIN = A.BIN", program)); //GEC=Y 아닌것을 모두 가져온다.
            query.AppendLine(")");

            return GetDataTable("SELECT_DYNAMIC", new string[] { query.ToString() }, null);
        }
    }
}
