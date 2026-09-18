using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Collections;
using TestDataMigrationRemotingService;

namespace TestDataMigrationRemotingService
{
    /// <summary>
    /// Database Instance: Oracle 11g
    /// Client information: Oracle ManagedDataAccess Client
    /// </summary>
    public class MiracomTPS : BaseInfo, IMiracomTPS
    {
        public static readonly int FAB1_PORT = 3333;
        public static readonly int FAB2_PORT = 3334;

        public static string TABLESPACE_DATA = "TS_TEST_TBL";
        public static string TABLESPACE_INDEX = "TS_TEST_IDX";

        static object LockObject = new object();

        public MiracomTPS()
        {
        }

        public MiracomTPS(string factory)
        {
            Factory = factory;
        }

        public static int GetPort(string factory)
        {
            return factory == "FAB1" ? FAB1_PORT : factory == "FAB2" ? FAB2_PORT : 9999;
        }

        #region [ Method ]
        public string ConvertStringToOracleType(
            string type
            )
        {
            string rValue = string.Empty;
            switch (type)
            {
                case "INT":
                case "FLOAT":
                    rValue = "NUMBER";
                    break;

                case "DATE":
                    rValue = "DATE";
                    break;

                case "STRING":
                    rValue = "VARCHAR(256 byte)";
                    break;
            }

            return rValue;
        }

        #endregion [ Method ]

        //-----------------------------------------------------------------------------------

        #region [ Checked TPS DataTable ]

        /// <summary>
        /// test program 에 대한 테이블이 존재하는지 체크.
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public int CheckedDataTable(
            string tablename
            )
        {
            Dictionary<string, Parameter> dicParams = null;
            StringBuilder sQuery = new StringBuilder();

            //--
            /// Sql Query
            //--

            sQuery.AppendLine("SELECT * FROM DBA_TABLES ");
            sQuery.AppendLine(" WHERE 1=1");
            sQuery.AppendLine("       AND TABLE_NAME = :tablename");

            //--

            dicParams = new Dictionary<string, Parameter>();
            #region [ Setting Parameter ]

            Parameter p = new Parameter();
            p.Type = DbType.String;
            p.Value = tablename;
            dicParams.Add("tablename", p);

            #endregion [ Setting Parameter ]

            //--

            DataTable checkedDt = base.ExecuteQuery(
                Constract.FUNCATION_NAME_LOT,
                sQuery.ToString(),
                dicParams
                );
            return checkedDt.Rows.Count;
        }

        #endregion [ Checked TPS DataTable ]

        #region Bulk Inser & Execute

        public void ExecuteTable(DataTable schemaTable, List<object[]> dataList)
        {
            if (schemaTable == null || schemaTable.Rows.Count == 0 || dataList == null || dataList[0] == null || dataList[0].Length == 0)
                return;

            int dataCount = dataList[0].Length;

            using (OracleConnection oConnection = CreateConnection())
            {
                using (OracleCommand comm = oConnection.CreateCommand())
                {
                    StringBuilder sb1 = new StringBuilder();
                    StringBuilder sb2 = new StringBuilder();

                    // Parameter 추가
                    for (int c = 0; c < schemaTable.Rows.Count; c++)
                    {
                        string columnName = (string)schemaTable.Rows[c][0];

                        sb1.Append(columnName);
                        sb2.Append(":" + columnName);

                        if (c < schemaTable.Rows.Count - 1)
                        {
                            sb1.Append(", ");
                            sb2.Append(", ");
                        }

                        OracleParameter para = new OracleParameter();
                        para.ParameterName = ":" + columnName;
                        para.SourceColumn = columnName;
                        para.OracleDbType = GetOracleDbType((Type)schemaTable.Rows[c]["DataType"]);
                        para.Size = 50;

                        para.Value = dataList[c];
                        comm.Parameters.Add(para);
                    }

                    comm.CommandText = String.Format("INSERT INTO {0} ({1}) VALUES ({2})", schemaTable.TableName, sb1.ToString(), sb2.ToString());
                    comm.ArrayBindCount = dataCount;
                    comm.CommandType = CommandType.Text;

                    oConnection.Open();
                    comm.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// DataTable을 기반으로 Bulk Insert를 실행합니다.
        /// </summary>
        public void ExecuteTable(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return;

            using (OracleConnection oConnection = CreateConnection())
            {
                using (OracleCommand comm = oConnection.CreateCommand())
                {
                    StringBuilder sb1 = new StringBuilder();
                    StringBuilder sb2 = new StringBuilder();

                    // Parameter 추가
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string columnName = dt.Columns[c].ColumnName;

                        sb1.Append(columnName);
                        sb2.Append(":" + columnName);

                        if (c < dt.Columns.Count - 1)
                        {
                            sb1.Append(", ");
                            sb2.Append(", ");
                        }

                        OracleParameter para = new OracleParameter();
                        para.ParameterName = ":" + columnName;
                        para.SourceColumn = columnName;
                        para.OracleDbType = GetOracleDbType(dt.Columns[c].DataType);
                        para.Size = 50;

                        object[] values = new object[dt.Rows.Count];

                        for (int r = 0; r < dt.Rows.Count; r++)
                            values[r] = dt.Rows[r][c];

                        para.Value = values;

                        comm.Parameters.Add(para);
                    }

                    comm.CommandText = String.Format("INSERT INTO {0} ({1}) VALUES ({2})", dt.TableName, sb1.ToString(), sb2.ToString());
                    comm.ArrayBindCount = dt.Rows.Count;
                    comm.CommandType = CommandType.Text;

                    int tr = 0, maxtr = 10;
                    Oracle.ManagedDataAccess.Client.OracleException o;

                    oConnection.Open();
                    comm.ExecuteNonQuery();
                }
            }
        }

        public static readonly int MAX_UPDATE_COUNT = 100000;

        /// <summary>
        /// DataTable을 기반으로 Bulk Update를 실행합니다.
        /// </summary>
        public void UpdateTable(DataTable dt, string[] updateArr, string[] whereArr)
        {
            if (dt == null || dt.Rows.Count == 0)
                return;

            using (OracleConnection oConnection = CreateConnection())
            {
                using (OracleCommand comm = oConnection.CreateCommand())
                {
                    StringBuilder sb1 = new StringBuilder();
                    StringBuilder sb2 = new StringBuilder();

                    // update
                    for (int c = 0; c < updateArr.Length; c++)
                    {
                        string columnName = updateArr[c];
                        sb1.Append(String.Format("{0} = :{0}", columnName));

                        if (c < updateArr.Length - 1)
                            sb1.Append(",");

                        OracleParameter para = new OracleParameter();
                        para.ParameterName = ":" + columnName;
                        para.SourceColumn = columnName;
                        para.OracleDbType = GetOracleDbType(dt.Columns[c].DataType);
                        para.Size = 50;

                        comm.Parameters.Add(para);
                    }

                    // where
                    for (int c = 0; c < whereArr.Length; c++)
                    {
                        string columnName = whereArr[c];
                        sb2.Append(String.Format("{0} = :{0}", columnName));

                        if (c < whereArr.Length - 1)
                            sb2.Append(" AND ");

                        OracleParameter para = new OracleParameter();
                        para.ParameterName = ":" + columnName;
                        para.SourceColumn = columnName;
                        para.OracleDbType = GetOracleDbType(dt.Columns[c].DataType);
                        para.Size = 50;

                        comm.Parameters.Add(para);
                    }

                    comm.CommandType = CommandType.Text;
                    comm.CommandText = String.Format("UPDATE {0} SET {1} WHERE {2}", dt.TableName, sb1.ToString(), sb2.ToString());

                    // 건수가 많은 경우 나누어서 처리
                    int num = 0;

                    while (num < dt.Rows.Count)
                    {
                        int length = Math.Min(MAX_UPDATE_COUNT, dt.Rows.Count - num);

                        comm.ArrayBindCount = length;

                        int c = 0;

                        foreach (string colName in updateArr)
                        {
                            object[] values = new object[length];

                            for (int r = 0; r < length; r++)
                                values[r] = dt.Rows[num + r][colName];

                            comm.Parameters[c].Value = values;
                            c++;
                        }

                        foreach (string colName in whereArr)
                        {
                            object[] values = new object[length];

                            for (int r = 0; r < length; r++)
                                values[r] = dt.Rows[num + r][colName];

                            comm.Parameters[c].Value = values;
                            c++;
                        }

                        num += length;

                        oConnection.Open();
                        comm.ExecuteNonQuery();
                        oConnection.Close();
                    }
                }
            }
        }

        public void DeleteTable(DataTable dt, string[] whereArr)
        {
            if (dt == null || dt.Rows.Count == 0)
                return;

            using (OracleConnection oConnection = CreateConnection())
            {
                using (OracleCommand comm = oConnection.CreateCommand())
                {
                    StringBuilder sb1 = new StringBuilder();

                    // where
                    for (int c = 0; c < whereArr.Length; c++)
                    {
                        string columnName = whereArr[c];
                        sb1.Append(String.Format("{0} = :{0}", columnName));

                        if (c < whereArr.Length - 1)
                            sb1.Append(" AND ");

                        OracleParameter para = new OracleParameter();
                        para.ParameterName = ":" + columnName;
                        para.SourceColumn = columnName;
                        para.OracleDbType = GetOracleDbType(dt.Columns[c].DataType);
                        para.Size = 50;

                        object[] values = new object[dt.Rows.Count];

                        for (int r = 0; r < dt.Rows.Count; r++)
                            values[r] = dt.Rows[r][columnName];

                        para.Value = values;

                        comm.Parameters.Add(para);
                    }

                    comm.CommandText = String.Format("DELETE FROM {0} WHERE {1}", dt.TableName, sb1.ToString());
                    comm.ArrayBindCount = dt.Rows.Count;
                    comm.CommandType = CommandType.Text;

                    oConnection.Open();
                    comm.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// DataTable을 기반으로 여러개의 프로시저를 실행합니다.
        /// </summary>
        public static void ExecuteProcedure(DataTable dt, OracleConnection oConnection)
        {
            OracleCommand comm = null;

            try
            {
                comm = new OracleCommand();

                // Parameter 추가
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    string columnName = dt.Columns[c].ColumnName;

                    OracleParameter para = new OracleParameter();
                    para.ParameterName = ":" + columnName;
                    para.SourceColumn = columnName;
                    para.OracleDbType = GetOracleDbType(dt.Columns[c].DataType);
                    para.Size = 38;

                    object[] values = new object[dt.Rows.Count];

                    for (int r = 0; r < dt.Rows.Count; r++)
                        values[r] = dt.Rows[r][c];

                    para.Value = values;

                    comm.Parameters.Add(para);
                }

                comm.CommandText = dt.TableName;
                comm.ArrayBindCount = dt.Rows.Count;
                comm.CommandType = CommandType.StoredProcedure;
                comm.Connection = oConnection;

                comm.ExecuteNonQuery();
            }
            finally
            {
                if (comm != null)
                    comm.Dispose();
            }
        }

        private static OracleCommand GetInsertCommand(DataTable dt)
        {
            OracleCommand comm = new OracleCommand();
            comm.ArrayBindCount = dt.Rows.Count;

            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();

            for (int c = 0; c < dt.Columns.Count; c++)
            {
                string columnName = dt.Columns[c].ColumnName;

                sb1.Append(columnName);
                sb2.Append(":" + columnName);

                if (c < dt.Columns.Count - 1)
                {
                    sb1.Append(", ");
                    sb2.Append(", ");
                }

                OracleParameter para = new OracleParameter();
                para.ParameterName = ":" + columnName;
                para.SourceColumn = columnName;
                para.OracleDbType = GetOracleDbType(dt.Columns[c].DataType);
                para.Size = 38;

                object[] values = new object[dt.Rows.Count];

                for (int r = 0; r < dt.Rows.Count; r++)
                    values[r] = dt.Rows[r][c];

                para.Value = values;

                comm.Parameters.Add(para);
            }

            comm.CommandText = String.Format("INSERT INTO {0} ({1}) VALUES ({2})", dt.TableName, sb1.ToString(), sb2.ToString());
            return comm;
        }

        public static string[] DATE_COLUMNS = { "LOG_TIME", "END_TIME", "UNIT_START_TIME" };

        private static OracleDbType GetOracleDbType(Type t)
        {
            if (t == typeof(string)) return OracleDbType.Varchar2;
            if (t == typeof(DateTime)) return OracleDbType.Date;
            if (t == typeof(Int64)) return OracleDbType.Int64;
            if (t == typeof(Int32)) return OracleDbType.Int32;
            if (t == typeof(Int16)) return OracleDbType.Int16;
            if (t == typeof(sbyte)) return OracleDbType.Byte;
            if (t == typeof(byte)) return OracleDbType.Int16;
            if (t == typeof(decimal)) return OracleDbType.Decimal;
            if (t == typeof(float)) return OracleDbType.Single;
            if (t == typeof(double)) return OracleDbType.Double;
            if (t == typeof(byte[])) return OracleDbType.Blob;

            return OracleDbType.Varchar2;
        }

        #endregion

        public void CreateTableAndView(
            string tableName,
            List<string> columnList
            )
        {
            if (tableName.IndexOf("TD_") != 0)
                throw new Exception("테이블명에 TD_ 가 없습니다.");

            string viewName = "VW_" + tableName.Substring(3);

            if (!ContainsTable(tableName))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(
    @"
CREATE TABLE TPSMGR.{0}
(
  WAFER_SEQ     NUMBER,
  DIE_NUM       NUMBER,
  DIEPROBE_CNT  NUMBER,
", tableName);

                foreach (string column in columnList)
                {
                    string dataType = "NUMBER";

                    if (Array.IndexOf<string>(DATE_COLUMNS, column) >= 0)
                        dataType = "DATE";

                    sb.Append(String.Format("{0} {1},", column, dataType));
                }

                // 쉼표 제거
                sb.Remove(sb.Length - 1, 1);
                sb.AppendLine(") TABLESPACE " + TABLESPACE_DATA);
                ExecuteNonQuery(sb.ToString());

                // PK INDEX 생성
                ExecuteNonQuery(String.Format(
@"
CREATE UNIQUE INDEX TPSMGR.PK_{0} ON TPSMGR.{0}
(WAFER_SEQ, DIE_NUM)
TABLESPACE {1}
", tableName, TABLESPACE_INDEX));


                // PK 생성
                ExecuteNonQuery(
                    String.Format(
    @"
ALTER TABLE TPSMGR.{0} ADD (
  CONSTRAINT PK_{0}
  PRIMARY KEY
  (WAFER_SEQ, DIE_NUM)
  USING INDEX
    TABLESPACE {1})
", tableName, TABLESPACE_INDEX));
            }

            /*
            if (!ContainsView(viewName))
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendFormat(
    @"
CREATE OR REPLACE FORCE VIEW TPSMGR.{0}
(
   TEST_AREA,
   PROGRAM,
   LOT,
   LOT_END_TIME,
   LOT_SEQ,
   WAFER_ID,
   PROBE_CNT,
   WAFER_SEQ,
   DIE_NUM,
   DIEPROBE_CNT,
", viewName);

                foreach (string column in columnList)
                {
                    sb.Append(String.Format("{0},", column));
                }

                // 쉼표 제거
                sb.Remove(sb.Length - 1, 1);
                sb.AppendFormat(
    @"
)
AS
    SELECT L.TEST_AREA,
        L.PROGRAM,
        L.LOT_ID,
        L.END_TIME LOT_END_TIME,
        L.LOT_SEQ,
        W.WAFER_ID,
        W.PROBE_CNT,
        A.*
    FROM TQP_LOT L, TQP_WAFER W, {0} A
    WHERE L.LOT_SEQ = W.LOT_SEQ
        AND W.WAFER_SEQ = A.WAFER_SEQ
        AND W.MERGE_FLAG = 'Y'
        AND A.DIEPROBE_CNT = 0
", tableName);

                ExecuteNonQuery(sb.ToString());
            }
            //*/
        }

        private bool ContainsTable(string tableName)
        {
            object obj = ExecuteScalar(String.Format(
            @"
SELECT 1 FROM ALL_TABLES
WHERE TABLE_NAME = '{0}'
", tableName));

            return obj != null && obj != DBNull.Value && obj.ToString() == "1";
        }

        public void DropTable(string tableName)
        {
            //ExecuteNonQuery(String.Format("CREATE TABLE {0}_BAK1911 AS SELECT * FROM {0}", tableName));
            ExecuteNonQuery(String.Format("ALTER TABLE TPSMGR.{0} DROP PRIMARY KEY CASCADE", tableName));
            ExecuteNonQuery(String.Format("DROP TABLE TPSMGR.{0} CASCADE CONSTRAINTS", tableName));
        }

        public bool ContainsView(string viewName)
        {
            object obj = ExecuteScalar(String.Format(
@"
SELECT 1 FROM ALL_VIEWS
WHERE VIEW_NAME = '{0}'
", viewName));

            return obj != null && obj != DBNull.Value && obj.ToString() == "1";
        }

        public void DropView(string viewName)
        {
            ExecuteNonQuery(String.Format("DROP VIEW TPSMGR.{0}", viewName));
        }

        public void DeletePara(string factory, string program)
        {
            // 과거 데이터 삭제
            ExecuteNonQuery(String.Format(
@"
DELETE FROM TQP_PARASPEC
WHERE FACTORY = '{0}' AND PROGRAM = '{1}'
", factory, program));
        }

        public bool ContainsPara(string factory, string program)
        {
            DataTable dt = ExecuteQuery(String.Format(
@"
SELECT COUNT(*) FROM TQP_PARASPEC
WHERE FACTORY = '{0}' AND PROGRAM = '{1}'
", factory, program));

            return Int32.Parse(dt.Rows[0][0].ToString()) > 0;
        }

        public void InsertBulk(DataTable paraDt)
        {
            ExecuteTable(paraDt);
        }

        public void ExecuteProcedure(DataTable procDt)
        {
            ExecuteProcedure(procDt);
        }

        public DataTable GetProgramList(string factory, string program)
        {
            return ExecuteQuery(String.Format(
@"
SELECT TABLE_NAME, 'T'||SUBSTR(TABLE_NAME,4) OLD_TABLE_NAME
FROM TQP_DATA_TABLES
WHERE FACTORY = '{0}' 
AND PROGRAM = '{1}'
ORDER BY 1
", factory, program));
        }

        public void UpdateWaferProgramRev(decimal lotSeq)
        {
            ExecuteNonQuery(String.Format(
@"
UPDATE TQP_WAFER A
SET PROGRAM_REV = (SELECT PROGRAM_REV FROM TQP_PARASPEC
    WHERE PROGRAM = A.PROGRAM AND PPD_SEQ = A.PROGRAM_REV AND ROWNUM = 1) 
WHERE LOT_SEQ = {0}
", lotSeq));
        }

        public decimal NewLotSeq()
        {
            return (decimal)ExecuteScalar("SELECT LOT_SEQ.NEXTVAL FROM DUAL");
        }

        public decimal NewWaferSeq()
        {
            lock (LockObject)
            {
                return (decimal)ExecuteScalar("SELECT WAFER_SEQ.NEXTVAL FROM DUAL");
            }
        }

        public void CheckProduct(string factory, string deviceAlias)
        {
            ExecuteNonQuery(String.Format(
@"
MERGE INTO TQP_PRODUCT
USING DUAL
ON (FACTORY = '{0}' AND PRODUCT = '{1}')
WHEN NOT MATCHED THEN
    INSERT (FACTORY, PRODUCT, MAPID) VALUES ('{0}', '{1}', '{2}')
", factory, deviceAlias, deviceAlias));

        }

        public DataTable GetTTableList(string factory)
        {
            return ExecuteQuery("SELECT DISTINCT TABLE_NAME FROM TQP_DATA_TABLES ORDER BY 1");
        }

        public DataTable GetWaferSeq(string tableName)
        {
            return ExecuteQuery(String.Format(
@"
SELECT OLD_WAFER_SEQ, WAFER_SEQ
FROM 
TQP_WAFER A,
TQP_DATA_TABLES B
WHERE A.PROGRAM = B.PROGRAM
AND B.TABLE_NAME = '{0}'", tableName));
        }

        public void TruncateTTable(string tableName)
        {
            ExecuteNonQuery(String.Format("TRUNCATE TABLE {0}", tableName));
        }

        public bool ExistsData(string tableName)
        {
            DataTable dt = ExecuteQuery(String.Format(
@"
SELECT 1 FROM DUAL
WHERE EXISTS (
SELECT * FROM {0}
)", tableName));

            if (dt == null || dt.Rows.Count == 0)
                return false;

            return true;
        }

        public string GetDeviceAlias(string lotID)
        {
            return (string)ExecuteScalar(String.Format("SELECT MASK_ID FROM TQC_LOT_STS WHERE LOT_NO = '{0}'", lotID));
        }

        public DataTable GetDeviceAliasList()
        {
            return ExecuteQuery("SELECT DISTINCT DEVICE_ALIAS FROM TQP_LOT ORDER BY 1");
        }

        private string[] TableToArray(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return null;

            string[] arr = new string[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
                arr[i] = dt.Rows[i][0].ToString();

            return arr;
        }

        public string[] GetTableNameFormProgram(string program)
        {
            DataTable dt = ExecuteQuery(String.Format(@"
select distinct table_name
from tqp_data_tables
where program = '{0}'", program));

            return TableToArray(dt);
        }

        public DataTable GetTableName(string deviceAlias)
        {
            return ExecuteQuery(String.Format(
@"
SELECT TABLE_NAME FROM ALL_TAB_COLUMNS
WHERE TABLE_NAME IN (
    SELECT TABLE_NAME 
    FROM TQP_DATA_TABLES
    WHERE PROGRAM IN (
        SELECT PROGRAM
        FROM TQP_LOT
        WHERE DEVICE_ALIAS = '{0}'
    )
)
AND COLUMN_NAME = 'X'
", deviceAlias));
        }


        public bool ExistsTable(string mTableName)
        {
            object obj = ExecuteScalar(String.Format(
@"
SELECT 1
FROM DUAL
WHERE EXISTS (
    SELECT *
    FROM ALL_TAB_COLUMNS
    WHERE OWNER = 'TPSMGR'
    AND TABLE_NAME = '{0}'
)", mTableName));

            return (obj != null && obj.ToString() == "1");
        }

        public DataTable GetParameterFromTable(string tableName)
        {
            return ExecuteQuery(String.Format(
@"
SELECT COLUMN_NAME
FROM ALL_TAB_COLUMNS
WHERE OWNER = 'TPSMGR'
AND TABLE_NAME = '{0}'
ORDER BY COLUMN_ID
", tableName));
        }

        public void AppendColumnAtTDTable(string tableName, List<string> columnList)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"ALTER TABLE TPSMGR.{0} ADD (", tableName);

            foreach (string column in columnList)
            {
                sb.Append(String.Format("{0} NUMBER,", column));
            }

            // 쉼표 제거
            sb.Remove(sb.Length - 1, 1);
            sb.AppendLine(")");

            ExecuteNonQuery(sb.ToString());
        }

        public DataTable GetDieInfo(string tableName, string deviceAlias)
        {
            return ExecuteQuery(String.Format(
@"
SELECT * FROM (
SELECT COUNT(*) CNT,MIN(X) MINX, MIN(Y) MINY, MAX(X) MAXX, MAX(Y) MAXY 
FROM {0}
WHERE WAFER_SEQ IN (
    SELECT WAFER_SEQ
    FROM TQP_WAFER
    WHERE LOT_SEQ IN (
        SELECT LOT_SEQ 
        FROM TQP_LOT
        WHERE DEVICE_ALIAS = '{1}'
            AND TEST_AREA IN ('MULTIPROBE', 'PRELASER', 'POSTLASER')
    )
)
GROUP BY WAFER_SEQ
)
WHERE ROWNUM = 1
", tableName, deviceAlias));
        }

        // BIN 필드가 있는 테이블 명
        public string GetBinTTable(decimal waferSeq)
        {
            return (string)ExecuteScalar(String.Format(
@"
SELECT TABLE_NAME 
FROM TQP_PARASPEC
WHERE PROGRAM
IN (
    SELECT PROGRAM
    FROM TQP_WAFER
    WHERE WAFER_SEQ = {0}
)
AND PARAM_NAME = 'BIN'
AND ROWNUM = 1
", waferSeq));
        }

        public Dictionary<string, decimal> GetBinCount(string tableName, decimal waferSeq)
        {
            DataTable dt = ExecuteQuery(String.Format(
@"
SELECT
    'BIN' || BIN BIN, COUNT(*) CNT
FROM {0}
WHERE WAFER_SEQ = {1}
AND BIN < 100
AND BIN >= 0
GROUP BY BIN
ORDER BY BIN
", tableName, waferSeq));

            if (dt == null || dt.Rows.Count == 0)
                return null;

            Dictionary<string, decimal> dic = new Dictionary<string, decimal>();

            foreach (DataRow row in dt.Rows)
            {
                dic.Add(row[0].ToString(), (decimal)row[1]);
            }

            return dic;
        }

        public DataTable GetWaferSum(decimal waferSeq)
        {
            return ExecuteQuery(String.Format(
@"
SELECT
    'DB' CUSTOMER,
    A.FACTORY FACILITY,
    A.DEVICE PRODUCT,
    TEST_AREA TESTAREA,
    B.PROGRAM,
    A.LOT_ID MOTHER_LOT_ID,
    A.DEVICE_ALIAS,
    A.LOT_ID,
    A.LOT_SEQ,
    B.WAFER_ID,
    B.WAFER_SEQ,
    B.TESTER,
    B.PROBE_CARD,
    B.OPERATOR,
    B.PROBE_CNT,
    B.START_TIME,
    B.END_TIME,
    0 WAFER_CAT,
    0 GEC,
    0 YIELD
FROM TQP_LOT A,
TQP_WAFER B
WHERE A.LOT_SEQ = B.LOT_SEQ
AND B.WAFER_SEQ = {0}
", waferSeq));
        }

        public DataTable GetLotSum(decimal lotSeq)
        {
            return ExecuteQuery(String.Format(
@"
SELECT
    CUSTOMER,
    FACILITY,
    PRODUCT,
    DEVICE_ALIAS,
    TESTAREA,
    PROGRAM,
    MOTHER_LOT_ID,
    LOT_ID,
    LOT_SEQ,
    MIN(START_TIME) START_TIME,
    MAX(END_TIME) END_TIME,
    0 LOSS_DIE,
    SUM(TESTED_DIE) TESTED_DIE,
    ROUND(AVG(YIELD),2) YIELD,
    SUM(BIN1) BIN1,
    SUM(GEC) GEC,
    COUNT(*) WAFERS,
    SUM(BIN0) BIN0,
    SUM(BIN1) BIN1,
    SUM(BIN2) BIN2,
    SUM(BIN3) BIN3,
    SUM(BIN4) BIN4,
    SUM(BIN5) BIN5,
    SUM(BIN6) BIN6,
    SUM(BIN7) BIN7,
    SUM(BIN8) BIN8,
    SUM(BIN9) BIN9,
    SUM(BIN10) BIN10,
    SUM(BIN11) BIN11,
    SUM(BIN12) BIN12,
    SUM(BIN13) BIN13,
    SUM(BIN14) BIN14,
    SUM(BIN15) BIN15,
    SUM(BIN16) BIN16,
    SUM(BIN17) BIN17,
    SUM(BIN18) BIN18,
    SUM(BIN19) BIN19,
    SUM(BIN20) BIN20,
    SUM(BIN21) BIN21,
    SUM(BIN22) BIN22,
    SUM(BIN23) BIN23,
    SUM(BIN24) BIN24,
    SUM(BIN25) BIN25,
    SUM(BIN26) BIN26,
    SUM(BIN27) BIN27,
    SUM(BIN28) BIN28,
    SUM(BIN29) BIN29,
    SUM(BIN30) BIN30,
    SUM(BIN31) BIN31,
    SUM(BIN32) BIN32,
    SUM(BIN33) BIN33,
    SUM(BIN34) BIN34,
    SUM(BIN35) BIN35,
    SUM(BIN36) BIN36,
    SUM(BIN37) BIN37,
    SUM(BIN38) BIN38,
    SUM(BIN39) BIN39,
    SUM(BIN40) BIN40,
    SUM(BIN41) BIN41,
    SUM(BIN42) BIN42,
    SUM(BIN43) BIN43,
    SUM(BIN44) BIN44,
    SUM(BIN45) BIN45,
    SUM(BIN46) BIN46,
    SUM(BIN47) BIN47,
    SUM(BIN48) BIN48,
    SUM(BIN49) BIN49,
    SUM(BIN50) BIN50,
    SUM(BIN51) BIN51,
    SUM(BIN52) BIN52,
    SUM(BIN53) BIN53,
    SUM(BIN54) BIN54,
    SUM(BIN55) BIN55,
    SUM(BIN56) BIN56,
    SUM(BIN57) BIN57,
    SUM(BIN58) BIN58,
    SUM(BIN59) BIN59,
    SUM(BIN60) BIN60,
    SUM(BIN61) BIN61,
    SUM(BIN62) BIN62,
    SUM(BIN63) BIN63,
    SUM(BIN64) BIN64,
    SUM(BIN65) BIN65,
    SUM(BIN66) BIN66,
    SUM(BIN67) BIN67,
    SUM(BIN68) BIN68,
    SUM(BIN69) BIN69,
    SUM(BIN70) BIN70,
    SUM(BIN71) BIN71,
    SUM(BIN72) BIN72,
    SUM(BIN73) BIN73,
    SUM(BIN74) BIN74,
    SUM(BIN75) BIN75,
    SUM(BIN76) BIN76,
    SUM(BIN77) BIN77,
    SUM(BIN78) BIN78,
    SUM(BIN79) BIN79,
    SUM(BIN80) BIN80,
    SUM(BIN81) BIN81,
    SUM(BIN82) BIN82,
    SUM(BIN83) BIN83,
    SUM(BIN84) BIN84,
    SUM(BIN85) BIN85,
    SUM(BIN86) BIN86,
    SUM(BIN87) BIN87,
    SUM(BIN88) BIN88,
    SUM(BIN89) BIN89,
    SUM(BIN90) BIN90,
    SUM(BIN91) BIN91,
    SUM(BIN92) BIN92,
    SUM(BIN93) BIN93,
    SUM(BIN94) BIN94,
    SUM(BIN95) BIN95,
    SUM(BIN96) BIN96,
    SUM(BIN97) BIN97,
    SUM(BIN98) BIN98,
    SUM(BIN99) BIN99
FROM TQP_WAFER_SUM
WHERE LOT_SEQ = LOT_SEQ
AND LOT_SEQ = {0}
AND PROBE_CNT = 0
GROUP BY CUSTOMER, FACILITY, PRODUCT, DEVICE_ALIAS, TESTAREA, PROGRAM, MOTHER_LOT_ID, LOT_ID, LOT_SEQ
", lotSeq));
        }

        public DataTable GetAllLotSeq()
        {
            return ExecuteQuery("SELECT LOT_SEQ FROM TQP_LOT ORDER BY 1");
        }

        public DataTable GetWaferSeq(decimal lotSeq)
        {
            return ExecuteQuery(String.Format(
@"SELECT WAFER_SEQ 
FROM TQP_WAFER 
WHERE LOT_SEQ = {0} 
AND PROBE_CNT > 0
ORDER BY 1", lotSeq));
        }

        public void DeleteWaferSum(decimal lotSeq)
        {
            ExecuteNonQuery(String.Format(
@"
DELETE FROM TQP_WAFER_SUM
WHERE LOT_SEQ = {0}
", lotSeq));
        }

        public void DeleteLotSum(decimal lotSeq)
        {
            ExecuteNonQuery(String.Format(
@"
DELETE FROM TQP_LOT_SUM
WHERE LOT_SEQ = {0}
", lotSeq));
        }

        public bool ExistsData(decimal lotSeq)
        {
            DataTable dt = ExecuteQuery(String.Format(
@"
SELECT 1 FROM TQP_LOT_SUM
WHERE LOT_SEQ = {0}
", lotSeq));

            if (dt == null || dt.Rows.Count == 0)
                return false;

            return true;
        }

        public void DeleteMapdef(string deviceAlias)
        {
            ExecuteNonQuery(String.Format(
@"
DELETE FROM TQP_MAPDEF
WHERE MAPID = '{0}'
", deviceAlias));
        }

        public void MergeIntoProgram(string Factory, string program, string testarea, string device)
        {
            ExecuteNonQuery(String.Format(
@"
MERGE INTO TQP_PROGRAM
USING DUAL
ON (FACTORY = '{0}' AND PROGRAM = '{1}')
WHEN NOT MATCHED THEN
    INSERT (FACTORY, PROGRAM, TESTAREA, DEVICE, CREATE_TIME, CREATE_USER)
    VALUES ('{0}', '{1}', '{2}', '{3}', SYSDATE, 'AUTO')
", Factory, program, testarea, device));
        }

        public void DeleteBin(string program)
        {
            ExecuteNonQuery(String.Format(
@"
DELETE FROM TQP_BINDESC
WHERE PROGRAM = '{0}'
", program));
        }

        public string GetMaxPsSeq(string program)
        {
            return (string)ExecuteScalar(String.Format(
@"
SELECT MAX(DESCRIPTION) PS_SEQ
FROM TQP_BINDESC
WHERE PROGRAM = '{0}'
", program));
        }

        public string GetMaxProgramRev(string program, string ppdSeq)
        {
            return (string)ExecuteScalar(String.Format(
@"
SELECT PROGRAM_REV 
FROM TQP_PARASPEC
WHERE PROGRAM = '{0}'
AND PPD_SEQ = {1}
AND ROWNUM = 1
", program, ppdSeq));
        }

        public void InsertProgramTable(string factory, string program, string[] tTableNames)
        {
            foreach (string tTableName in tTableNames)
            {
                ExecuteNonQuery(String.Format(
    @"
MERGE INTO TQP_DATA_TABLES
USING DUAL
ON (FACTORY = '{0}' AND PROGRAM = '{1}' AND TABLE_NAME = '{2}')
WHEN NOT MATCHED THEN
    INSERT (FACTORY, PROGRAM, TABLE_NAME)
    VALUES ('{0}', '{1}', '{2}')
", factory, program, "TD_" + tTableName.Substring(1)));
            }
        }

        public string Sysdate()
        {
            return ExecuteScalar(
@"
SELECT SYSDATE FROM DUAL
").ToString();
        }

        public DataTable Getdata_PARA_XY(string factory)
        {
            return ExecuteQuery(String.Format(
@"
SELECT *
FROM TQP_PARASPEC
WHERE FACTORY = '{0}' AND PARAM_NAME = 'XY'
", factory));
        }

        public DataTable GetLot(string Factory, DateTime dStartDate, DateTime dEndDate)
        {
            Dictionary<string, Parameter> dicParams = null;
            StringBuilder sQuery = new StringBuilder();

            sQuery.AppendLine("SELECT LOT_SEQ, ");
            sQuery.AppendLine("        LOT_SEQ OLD_LOT_SEQ, ");
            sQuery.AppendLine("        LOT_ID, ");
            sQuery.AppendLine("        LOT_TYPE, ");
            sQuery.AppendLine("        FABLOT, ");
            sQuery.AppendLine("        TEST_AREA, ");
            sQuery.AppendLine("        TEST_TYPE, ");
            sQuery.AppendLine("        LOG_POINT, ");
            sQuery.AppendLine("        OPERATION, ");
            sQuery.AppendLine("        PROGRAM, ");
            sQuery.AppendLine("        DEVICE, ");
            sQuery.AppendLine("        SMS_DEVICE, ");
            sQuery.AppendLine("        DEVICE_ALIAS, ");
            sQuery.AppendLine("        SMS_ITEM_ID, ");
            sQuery.AppendLine("        FAMILY, ");
            sQuery.AppendLine("        FACTORY, ");
            sQuery.AppendLine("        START_TIME, ");
            sQuery.AppendLine("        END_TIME, ");
            sQuery.AppendLine("        WAFER_DIAM, ");
            sQuery.AppendLine("        COMMENTS, ");
            sQuery.AppendLine("        MERGE_FLAG ");
            sQuery.AppendLine("FROM TQP_LOT ");
            sQuery.AppendLine("WHERE 1 = 1  ");
            sQuery.AppendLine("    AND END_TIME >= TO_DATE(:starttime, 'YYYYMMDDHH24MISS') ");
            sQuery.AppendLine("    AND END_TIME < TO_DATE(:endtime, 'YYYYMMDDHH24MISS') ");
            sQuery.AppendLine("    AND FACTORY = :factory");
            sQuery.AppendLine("ORDER BY LOT_SEQ");

            //--

            dicParams = new Dictionary<string, Parameter>();
            Parameter p = new Parameter();
            p.Type = DbType.String;
            p.Value = dStartDate.ToString("yyyyMMddHHmmss");
            dicParams.Add("starttime", p);

            //--

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = dEndDate.ToString("yyyyMMddHHmmss");
            dicParams.Add("endtime", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = Factory;
            dicParams.Add("factory", p);

            //--

            DataTable dt = ExecuteQuery(
                sQuery.ToString(),
                dicParams
                );

            return dt;
        }

        public DataTable GetAllTableName()
        {
            return ExecuteQuery(
@"
SELECT TABLE_NAME 
FROM TQP_DATA_TABLES");
        }

        // 삭제할 일자 데이터 가져오기
        public DataTable GetData01(string date)
        {
            return ExecuteQuery(String.Format(
@"
SELECT
    DISTINCT A.LOT_SEQ, C.TABLE_NAME
FROM TQP_LOT A,
TQP_DATA_TABLES C
WHERE A.PROGRAM = C.PROGRAM
    AND TO_CHAR(A.END_TIME, 'YYYY-MM-DD') = '{0}'  
", date));
        }

        public void DeleteTableData(string tableName, string[] lotSeqArr)
        {
            ExecuteNonQuery(String.Format(
@"
            DELETE
FROM {0}
WHERE WAFER_SEQ IN (
    SELECT WAFER_SEQ
    FROM TQP_WAFER
    WHERE LOT_SEQ IN ({1})
)
", tableName, String.Join(",", lotSeqArr)));
        }

        public void DeleteWaferAndLotData(string date)
        {
            ExecuteNonQuery(String.Format(
@"
DELETE
FROM TQP_WAFER 
WHERE LOT_SEQ IN (
    SELECT LOT_SEQ
    FROM TQP_LOT
    WHERE TO_CHAR(END_TIME, 'YYYY-MM-DD') = '{0}'
)
", date));

            ExecuteNonQuery(String.Format(
@"
DELETE
FROM TQP_LOT
WHERE TO_CHAR(END_TIME, 'YYYY-MM-DD') = '{0}'
", date));
        }

        public string[] GetGoodBinNames(decimal lotSeq)
        {
            DataTable dt = ExecuteQuery(String.Format(
@"
SELECT 'BIN' || BIN BIN FROM TQP_BINDESC
WHERE PROGRAM IN (
    SELECT PROGRAM FROM TQP_LOT
    WHERE LOT_SEQ = {0}
)
AND HIGH_GEC = 'Y'
", lotSeq));

            string[] arr = new string[dt.Rows.Count];

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = dt.Rows[i][0].ToString();
            }

            if (arr.Length == 0)
                return new string[] { "BIN1" };

            return arr;
        }

        public bool ExistsLotData(decimal tLotSeq)
        {
            object obj = ExecuteScalar(String.Format(
@"
SELECT 1
FROM TQP_LOT
WHERE OLD_LOT_SEQ = {0}
", tLotSeq));

            return (obj != null && obj.ToString() == "1");
        }

        public decimal GetLotSeq(string lotId)
        {
            object obj = ExecuteScalar(String.Format(
@"
SELECT LOT_SEQ
FROM TQP_LOT
WHERE LOT_ID = '{0}'
", lotId));

            return decimal.Parse(obj.ToString());
        }

        public decimal GetLotSeq(decimal tLotSeq)
        {
            object obj = ExecuteScalar(String.Format(
@"
SELECT LOT_SEQ
FROM TQP_LOT
WHERE OLD_LOT_SEQ = {0}
", tLotSeq));

            return decimal.Parse(obj.ToString());
        }

        public Dictionary<string, decimal> GetWaferSeq02(decimal mLotSeq)
        {
            Dictionary<string, Parameter> para = new Dictionary<string, Parameter>();
            para.Add(":LOT_SEQ", new Parameter(DbType.Decimal, mLotSeq));

            DataTable dt = ExecuteQuery(
@"SELECT WAFER_SEQ, OLD_WAFER_SEQ
FROM TQP_WAFER WHERE LOT_SEQ = :LOT_SEQ
", para);

            Dictionary<string, decimal> dic = new Dictionary<string, decimal>();

            foreach (DataRow row in dt.Rows)
                dic.Add(row["OLD_WAFER_SEQ"].ToString(), decimal.Parse(row["WAFER_SEQ"].ToString()));

            return dic;
        }

        public List<string> GetTableNames()
        {
            return GetStringList(
@"
SELECT TABLE_NAME
FROM TQP_DATA_TABLES
");
        }

        public DataTable GetUnlinkedWaferSeq(string tableName)
        {
            return ExecuteQuery(String.Format(
@"
SELECT DISTINCT WAFER_SEQ FROM {0} A
WHERE NOT EXISTS (
    SELECT WAFER_SEQ FROM TQP_WAFER B WHERE A.WAFER_SEQ = B.WAFER_SEQ
)
", tableName));
        }

        public Dictionary<decimal, int> GetLotAndWafCnt(DateTime dStartDate, DateTime dEndDate)
        {
            DataTable dt = ExecuteQuery(String.Format(
@"
SELECT OLD_LOT_SEQ, COUNT(*)
FROM TQP_WAFER
WHERE LOT_SEQ IN (
    SELECT LOT_SEQ FROM TQP_LOT
    WHERE END_TIME >= TO_DATE('{0}', 'YYYY-MM-DD')
    AND END_TIME < TO_DATE('{1}', 'YYYY-MM-DD')
)
AND PROBE_CNT = 0
GROUP BY OLD_LOT_SEQ
", dStartDate.ToString("yyyy-MM-dd"), dEndDate.ToString("yyyy-MM-dd")));

            Dictionary<decimal, int> dic = new Dictionary<decimal, int>();

            foreach (DataRow row in dt.Rows)
            {
                dic.Add(decimal.Parse(row[0].ToString()), int.Parse(row[1].ToString()));
            }

            return dic;
        }

        public decimal GetDataCount(string tableName)
        {
            return (decimal)ExecuteScalar(
@"
SELECT COUNT(*) CNT
FROM " + tableName);
        }

        public DataTable GetLot(string[] lotSeqArr)
        {
            if (lotSeqArr == null || lotSeqArr.Length == 0)
                return null;

            return ExecuteQuery(string.Format(
@"
SELECT LOT_ID, PROGRAM, OLD_LOT_SEQ
FROM TQP_LOT
WHERE LOT_SEQ IN ({0})", String.Join(",", lotSeqArr)));
        }

        public static bool TryXYtoXandY(object xy, out int x, out int y)
        {
            x = y = 0;

            if (xy == null || xy == DBNull.Value)
                return false;

            int val;

            if (!Int32.TryParse(xy.ToString(), out val))
                return false;

            x = val >> 16;
            y = val & 0xFFFF;

            return true;
        }

        public DataTable Test()
        {
            OracleCommand comm = null;
            OracleConnection conn = null;

            try
            {
                conn = CreateConnection();
                comm = new OracleCommand("SELECT V1C_NOM*V1C_NOM*V1C_NOM*V1C_NOM*V1C_NOM V1C_NOM, V5C_NOM FROM TD_HYT012CRA");
                comm.Connection = conn;

                DataTable schemaDt;
                conn.Open();

                using (OracleDataReader reader = comm.ExecuteReader(CommandBehavior.SchemaOnly))
                {
                    schemaDt = reader.GetSchemaTable();
                }

                DataTable dt = new DataTable();

                foreach (DataRow row in schemaDt.Rows)
                {
                    dt.Columns.Add(row["ColumnName"].ToString(), typeof(string));// Type.GetType(row["DataType"].ToString()));
                }

                schemaDt.Dispose();
                schemaDt = null;

                using (OracleDataReader reader = comm.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        DataRow newRow = dt.NewRow();

                        for (int c = 0; c < dt.Columns.Count; c++)
                        {
                            try
                            {
                                newRow[c] = reader.GetValue(c);
                            }
                            catch (InvalidCastException)
                            {
                                //if (dt.Columns[c].DataType == typeof(decimal))
                                //    newRow[c] = Decimal.MaxValue;

                                newRow[c] = reader.GetOracleValue(c).ToString();
                            }
                            catch
                            {
                            }
                        }

                        dt.Rows.Add(newRow);
                    }
                }

                return dt;
            }
            finally
            {
                if (comm != null)
                    comm.Dispose();

                if (conn != null)
                    conn.Dispose();
            }
        }

        public void DeleteOldTdData(string mTableName, decimal waferSeq)
        {
            ExecuteNonQuery(String.Format("DELETE FROM {0} WHERE WAFER_SEQ = {1}", mTableName, waferSeq));
        }

        public void DeleteProgramPara(string program)
        {
            ExecuteNonQuery(String.Format("DELETE FROM TQP_PARASPEC WHERE PROGRAM = '{0}'", program));
        }

        public DataTable GetDupColumn(string program)
        {
            return ExecuteQuery(string.Format(@"
SELECT '{0}' PROGRAM, COLUMN_NAME
FROM ALL_TAB_COLUMNS
WHERE OWNER = 'TPSMGR'
AND TABLE_NAME LIKE 'TD_' || '{0}%'
AND COLUMN_NAME NOT IN ('WAFER_SEQ','DIE_NUM','DIEPROBE_CNT')
GROUP BY COLUMN_NAME
HAVING COUNT(*) > 1
", program));
        }

        public string[] GetDistinctProgram()
        {
            DataTable dt = ExecuteQuery(@"
SELECT DISTINCT PROGRAM
FROM TQP_DATA_TABLES
ORDER BY 1
");
            return TableToArray(dt);
        }

        public string[] GetTableWhereField(string program, string dupField)
        {
            DataTable dt = ExecuteQuery(String.Format(@"
SELECT TABLE_NAME FROM
TQP_PARASPEC
WHERE PROGRAM = '{0}'
AND PARAM_NAME = '{1}'", program, dupField));

            return TableToArray(dt);
        }

        public string[] GetParaList(string program)
        {
            DataTable dt = ExecuteQuery(String.Format(@"
SELECT PARAM_NAME FROM
TQP_PARASPEC
WHERE PROGRAM = '{0}'", program));

            return TableToArray(dt);
        }

        public void DeleteTdData(string tableName, string lotSeq)
        {
            ExecuteNonQuery(String.Format(@"
DELETE
FROM {0}
WHERE WAFER_SEQ IN (
    SELECT DISTINCT WAFER_SEQ
    FROM TQP_WAFER
    WHERE LOT_SEQ = {1}
)", tableName, lotSeq));
        }

        public void DeleteWaferByLotSeq(string lotSeq)
        {
            ExecuteNonQuery(String.Format(@"
DELETE
FROM TQP_WAFER
WHERE WAFER_SEQ IN (
    SELECT DISTINCT WAFER_SEQ
    FROM TQP_WAFER
    WHERE LOT_SEQ = {0}
)", lotSeq));
        }

        public void DeleteWaferSumByLotSeq(string lotSeq)
        {
            ExecuteNonQuery(String.Format(@"
DELETE
FROM TQP_WAFER_SUM
WHERE WAFER_SEQ IN (
    SELECT DISTINCT WAFER_SEQ
    FROM TQP_WAFER
    WHERE LOT_SEQ = {0}
)", lotSeq));
        }

        public void DeleteLotByLotSeq(string lotSeq)
        {
            ExecuteNonQuery(String.Format(@"
DELETE
FROM TQP_LOT
WHERE LOT_SEQ = {0}
", lotSeq));
        }

        public void DeleteLotSumByLotSeq(string lotSeq)
        {
            ExecuteNonQuery(String.Format(@"
DELETE
FROM TQP_LOT_SUM
WHERE LOT_SEQ = {0}
", lotSeq));
        }

        public Dictionary<string, string> TTableSeqArr(string program)
        {
            DataTable dt = ExecuteQuery(String.Format(@"
SELECT DISTINCT 
    (SELECT WAFER_ID FROM TQP_WAFER WHERE WAFER_SEQ = A.WAFER_SEQ) WAFER_ID,
    WAFER_SEQ
FROM TD_{0} A", program));

            Dictionary<string, string> dic = new Dictionary<string, string>();

            if (dt == null || dt.Rows.Count == 0)
                return dic;

            foreach (DataRow row in dt.Rows)
                dic.Add(row["WAFER_SEQ"].ToString(), row["WAFER_ID"].ToString());

            return dic;

        }

        public void DeleteDataTables(string program)
        {
            ExecuteNonQuery(String.Format(@"
DELETE
FROM TQP_DATA_TABLES
WHERE PROGRAM = '{0}'
", program));
        }

        public void InsertDataTables(string factory, string program, string[] tTableArr)
        {
            DataTable dt = new DataTable("TQP_DATA_TABLES");
            dt.Columns.Add("FACTORY", typeof(string));
            dt.Columns.Add("PROGRAM", typeof(string));
            dt.Columns.Add("TABLE_NAME", typeof(string));

            foreach (string tTable in tTableArr)
                dt.Rows.Add(factory, program, ToMiracomTable(tTable));

            ExecuteTable(dt);
        }

        private string ToMiracomTable(string tTableName)
        {
            return "TD_" + tTableName.Substring(1);
        }

        public void UpdateParaspecTable(string program, string para, string tTableName)
        {
            ExecuteNonQuery(String.Format(@"
UPDATE TQP_PARASPEC
SET TABLE_NAME = '{2}'
WHERE PROGRAM = '{0}' AND PARAM_NAME = '{1}'
", program, para, ToMiracomTable(tTableName)));
        }

        public DataTable GetRawDataCountByWaferSeq(string program, decimal lotSeq)
        {
            return ExecuteQuery(String.Format(@"
SELECT B.OLD_WAFER_SEQ, A.WAFER_SEQ, A.CNT
FROM (
    SELECT A.WAFER_SEQ, COUNT(B.WAFER_SEQ) CNT 
    FROM TQP_WAFER A,
    TD_{0} B
    WHERE A.WAFER_SEQ = B.WAFER_SEQ(+)
    AND A.LOT_SEQ = {1}
    GROUP BY A.WAFER_ID, A.WAFER_SEQ
) A,
TQP_WAFER B
WHERE A.WAFER_SEQ = B.WAFER_SEQ
AND A.CNT = 0
", program, lotSeq));
        }

        public decimal GetMaxWaferSeq()
        {
            object obj = ExecuteScalar("SELECT max(wafer_seq) FROM tqp_wafer");
            return decimal.Parse(obj.ToString());
        }

        public bool ExistsWaferSum(decimal waferSeq)
        {
            object obj = ExecuteScalar("SELECT 1 FROM tqp_wafer where wafer_seq = " + waferSeq.ToString());

            if (obj == null || obj == DBNull.Value)
                return false;

            return true;
        }

        public string[] GetEmptyWaferSum(decimal from, decimal to)
        {
            DataTable dt = ExecuteQuery(String.Format(@"
select a.wafer_seq from tqp_wafer a,
tqp_wafer_sum b
where a.wafer_seq = b.wafer_seq(+)
and b.wafer_seq is null
and a.wafer_seq between {0} and {1}
", from, to));

            if (dt == null || dt.Rows.Count == 0)
                return new string[0];

            return TableToArray(dt);
        }
    }
}