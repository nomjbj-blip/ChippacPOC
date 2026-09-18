using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;

namespace FabTwoToMigrationTools.Source
{
    /// <summary>
    /// DataBase Instance: Oracle 8i
    /// Client information: Oracle Instance Client 8i
    /// </summary>
    public class TestData : IDisposable
    {
        private static readonly string ConnectionString = "Server={0};User ID={1};password={2}";
        private static readonly string DataSource = "ANAMNTDB";
        private static readonly string UserID = "t_test";
        private static readonly string Password = "t_test";
        private static readonly string m_ESDA_ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.147.13.13)(PORT=1521)))(CONNECT_DATA =(SERVICE_NAME=anamntdb)));User ID=t_test;Password=t_test;";
        private static readonly string strTableName = "TD_";

        public TestData(
            )
        {
        }


        #region [ Method ]

        public void Dispose(
            )
        {
        }

        //-----------------------------------------------------------------------------------

        public static object ExecuteScalar(
            string sQuery,
            Dictionary<string, Parameter> dicParams = null
            )
        {
            using (OracleCommand oCommand = new OracleCommand())
            {
                using (OracleConnection oConnection = new OracleConnection())
                {
                    oCommand.Connection = oConnection;
                    oConnection.ConnectionString = m_ESDA_ConnectionString;

                    oCommand.CommandText = sQuery;
                    oCommand.CommandTimeout = 60;
                    oCommand.CommandType = CommandType.Text;

                    if (dicParams != null)
                    {
                        foreach (KeyValuePair<string, Parameter> pk in dicParams)
                        {
                            OracleParameter oParams = new OracleParameter();
                            Parameter p = pk.Value;
                            oParams.ParameterName = pk.Key;
                            oParams.OracleType = GetOracleType(p.Type);
                            oParams.Value = p.Value;

                            oCommand.Parameters.Add(oParams);
                        }
                    }

                    oConnection.Open();
                    return oCommand.ExecuteScalar();
                }
            }
        }

        public static DataTable ExecuteQuery(
            string sQuery,
            Dictionary<string, Parameter> dicParams = null
            )
        {
            using (OracleCommand oCommand = new OracleCommand())
            {
                using (OracleConnection oConnection = new OracleConnection())
                {
                    oCommand.Connection = oConnection;
                    oConnection.ConnectionString = m_ESDA_ConnectionString;

                    oCommand.CommandText = sQuery;
                    oCommand.CommandTimeout = 60;
                    oCommand.CommandType = CommandType.Text;

                    if (dicParams != null)
                    {
                        foreach (KeyValuePair<string, Parameter> pk in dicParams)
                        {
                            OracleParameter oParams = new OracleParameter();
                            Parameter p = pk.Value;
                            oParams.ParameterName = pk.Key;
                            oParams.OracleType = GetOracleType(p.Type);
                            oParams.Value = p.Value;

                            oCommand.Parameters.Add(oParams);
                        }
                    }

                    DataTable dt = new DataTable();
                    OracleDataAdapter oAdapter = new OracleDataAdapter(oCommand);
                    oAdapter.Fill(dt);
                    return dt;
                }
            }
        }

        public static OracleType GetOracleType(
            DbType dbType
            )
        {
            OracleType oraType = OracleType.VarChar;
            switch (dbType)
            {
                case DbType.Date:
                case DbType.DateTime:
                    oraType = OracleType.DateTime;
                    break;
                case DbType.Decimal:
                    oraType = OracleType.Number;
                    break;
                case DbType.Double:
                    oraType = OracleType.Double;
                    break;
                case DbType.Object:
                    oraType = OracleType.Blob;
                    break;
                case DbType.Int16:
                    oraType = OracleType.Int16;
                    break;
                case DbType.Int32:
                    oraType = OracleType.Int32;
                    break;
                case DbType.UInt16:
                    oraType = OracleType.UInt16;
                    break;
                case DbType.UInt32:
                    oraType = OracleType.UInt32;
                    break;
                case DbType.Int64:
                case DbType.UInt64:
                    oraType = OracleType.Float;
                    break;
                case DbType.Time:
                    oraType = OracleType.Timestamp;
                    break;
                default:
                    oraType = OracleType.VarChar;
                    break;
            }

            return oraType;
        }


        //--

        #endregion [ Method ]

        //-----------------------------------------------------------------------------------

        #region [ Data Tables ]

        public DataTable GetDataTable(
            string lotid,
            string program
            )
        {
            Dictionary<string, Parameter> dicParams = null;
            StringBuilder sQuery = new StringBuilder();

            sQuery.AppendLine(" SELECT DISTINCT DECODE(T1.FACILITY, 'PROBE1', 'FAB2', 'FAB1') AS \"FACTORY\", T2.PROGRAM, ");
            sQuery.AppendLine(string.Format("  '{0}' ||  T1.PROGRAM AS TABLE_NAME ", strTableName));
            sQuery.AppendLine("   FROM LOT T1, DATA_TABLES T2 ");
            sQuery.AppendLine("  WHERE     1 = 1 ");
            sQuery.AppendLine("        AND T1.PROGRAM = T2.PROGRAM ");
            sQuery.AppendLine("        AND T1.LOT = :lot ");
            sQuery.AppendLine("        AND T1.PROGRAM = :program ");


            //--

            dicParams = new Dictionary<string, Parameter>();

            //--

            #region [ Setting Parameters ]

            //--

            Parameter p = new Parameter();
            p.Type = DbType.String;
            p.Value = lotid;
            dicParams.Add("lot", p);

            //--

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = program;
            dicParams.Add("program", p);

            //--

            #endregion [ Setting Parameters ]

            //--

            DataTable dt = ExecuteQuery(
                sQuery.ToString(),
                dicParams
                );

            return dt;
        }

        #endregion [ Data Tables]

        //-----------------------------------------------------------------------------------

        #region [ Lot ]

        public DataTable GetLot(
            DateTime dStartDate,
            DateTime dEndDate
            )
        {
            Dictionary<string, Parameter> dicParams = null;
            StringBuilder sQuery = new StringBuilder();

            sQuery.AppendLine(" SELECT LOT_SEQ, ");
            sQuery.AppendLine("        LOT, ");
            sQuery.AppendLine("        LOT_TYPE, ");
            sQuery.AppendLine("        FABLOT, ");
            sQuery.AppendLine("        TEST_AREA, ");
            sQuery.AppendLine("        TEST_TYPE, ");
            sQuery.AppendLine("        LOGPOINT, ");
            sQuery.AppendLine("        OPERATION, ");
            sQuery.AppendLine("        PROGRAM, ");
            sQuery.AppendLine("        DEVICE, ");
            sQuery.AppendLine("        SMS_DEVICE, ");
            sQuery.AppendLine("        DEVICE_ALIAS, ");
            sQuery.AppendLine("        SMS_ITEM_ID, ");
            sQuery.AppendLine("        FAMILY, ");
            sQuery.AppendLine("        DECODE(FACILITY, 'PROBE1', 'FAB2', 'FAB1') AS \"FACTORY\", ");
            sQuery.AppendLine("        SRCFAB, ");
            sQuery.AppendLine("        TO_CHAR(START_TIME, 'YYYYMMDDHH24MISS') AS \"START_TIME\", ");
            sQuery.AppendLine("        TO_CHAR(END_TIME, 'YYYYMMDDHH24MISS') AS \"END_TIME\", ");
            sQuery.AppendLine("        WAFER_DIAM, ");
            sQuery.AppendLine("        COMMENTS, ");
            sQuery.AppendLine("        MERGE_FLAG ");
            sQuery.AppendLine("   FROM LOT ");
            sQuery.AppendLine("  WHERE 1 = 1  ");
            //sQuery.AppendLine("        AND FACILITY = :factory ");
            sQuery.AppendLine("        AND END_TIME >= TO_DATE(:starttime, 'YYYYMMDDHH24MISS') ");
            sQuery.AppendLine("        AND END_TIME <= TO_DATE(:endtime, 'YYYYMMDDHH24MISS') ");

            //--

            dicParams = new Dictionary<string, Parameter>();
            Parameter p = new Parameter();
            p.Type = DbType.String;
            p.Value = string.Format("{0}000000", dStartDate.ToString(Constract.DEFAULT_DATE_STRING));
            dicParams.Add("starttime", p);

            //--

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = string.Format("{0}000000", dEndDate.ToString(Constract.DEFAULT_DATE_STRING));
            dicParams.Add("endtime", p);

            //--

            DataTable dt = ExecuteQuery(
                sQuery.ToString(),
                dicParams
                );

            return dt;
        }

        public DataTable GetLot(
           string factory,
           DateTime dStartDate,
           DateTime dEndDate
           )
        {
            Dictionary<string, Parameter> dicParams = null;
            StringBuilder sQuery = new StringBuilder();

            sQuery.AppendLine(" SELECT LOT_SEQ, ");
            sQuery.AppendLine("        LOT_SEQ OLD_LOT_SEQ, ");
            sQuery.AppendLine("        LOT LOT_ID, ");
            sQuery.AppendLine("        LOT_TYPE, ");
            sQuery.AppendLine("        FABLOT, ");
            sQuery.AppendLine("        TEST_AREA, ");
            sQuery.AppendLine("        TEST_TYPE, ");
            sQuery.AppendLine("        LOGPOINT LOG_POINT, ");
            sQuery.AppendLine("        OPERATION, ");
            sQuery.AppendLine("        PROGRAM, ");
            sQuery.AppendLine("        DEVICE, ");
            sQuery.AppendLine("        SMS_DEVICE, ");
            sQuery.AppendLine("        DEVICE_ALIAS, ");
            sQuery.AppendLine("        SMS_ITEM_ID, ");
            sQuery.AppendLine("        FAMILY, ");
            sQuery.AppendLine("        DECODE(FACILITY, 'PROBE1', 'FAB2', 'FAB1') AS \"FACTORY\", ");
            //sQuery.AppendLine("        SRCFAB, ");
            sQuery.AppendLine("        START_TIME, ");
            sQuery.AppendLine("        END_TIME, ");
            sQuery.AppendLine("        WAFER_DIAM, ");
            sQuery.AppendLine("        COMMENTS, ");
            sQuery.AppendLine("        MERGE_FLAG ");
            sQuery.AppendLine("   FROM LOT ");
            sQuery.AppendLine("  WHERE 1 = 1  ");
            //sQuery.AppendLine("        AND FACILITY = :factory ");
            sQuery.AppendLine("        AND END_TIME >= TO_DATE(:starttime, 'YYYYMMDDHH24MISS') ");
            sQuery.AppendLine("        AND END_TIME < TO_DATE(:endtime, 'YYYYMMDDHH24MISS') ");

            if (factory == "FAB2")
                sQuery.AppendLine("    AND FACILITY = 'PROBE1'");
            else
                sQuery.AppendLine("    AND FACILITY <> 'PROBE1'");

            sQuery.AppendLine("ORDER BY LOT_SEQ");

            //--

            dicParams = new Dictionary<string, Parameter>();
            Parameter p = new Parameter();
            p.Type = DbType.String;
            p.Value = string.Format("{0}000000", dStartDate.ToString(Constract.DEFAULT_DATE_STRING));
            dicParams.Add("starttime", p);

            //--

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = string.Format("{0}000000", dEndDate.ToString(Constract.DEFAULT_DATE_STRING));
            dicParams.Add("endtime", p);

            //--

            DataTable dt = ExecuteQuery(
                sQuery.ToString(),
                dicParams
                );

            return dt;
        }

        #endregion [ Lot ]

        //-----------------------------------------------------------------------------------

        #region [ Wafer ]

        public DataTable GetWafer(
            string lot,
            decimal lotseq
            )
        {
            Dictionary<string, Parameter> dicParams = null;
            StringBuilder sQuery = new StringBuilder();

            sQuery.AppendLine(" SELECT DECODE(T1.FACILITY, 'PROBE1', 'FAB2', 'FAB1') AS \"FACTORY\", ");
            sQuery.AppendLine("        T2.WAFER_SEQ, ");
            sQuery.AppendLine("        T1.LOT_SEQ, ");
            sQuery.AppendLine("        T2.WAFER_SEQ OLD_WAFER_SEQ, ");
            sQuery.AppendLine("        T1.LOT_SEQ OLD_LOT_SEQ, ");
            sQuery.AppendLine("        T2.WAFER_ID, ");
            sQuery.AppendLine("        T2.PROBE_CNT, ");
            sQuery.AppendLine("        T2.START_TIME,");
            sQuery.AppendLine("        T2.END_TIME,");
            sQuery.AppendLine("        T2.LOAD_TIME,");
            //sQuery.AppendLine("        TO_CHAR(T2.START_TIME, 'YYYYMMDDHH24MISS') AS \"START_TIME\", ");
            //sQuery.AppendLine("        TO_CHAR(T2.END_TIME, 'YYYYMMDDHH24MISS') AS \"END_TIME\", ");
            //sQuery.AppendLine("        TO_CHAR(T2.LOAD_TIME, 'YYYYMMDDHH24MISS') AS \"LOAD_TIME\", ");
            sQuery.AppendLine("        T2.TESTER, ");
            sQuery.AppendLine("        T2.PROBER, ");
            sQuery.AppendLine("        T2.PROBE_CARD, ");
            sQuery.AppendLine("        T2.HANDLER_ID, ");
            sQuery.AppendLine("        T2.OPERATOR, ");
            sQuery.AppendLine("        '' AS \"PROGRAM\", ");
            sQuery.AppendLine("        PPD_SEQ AS \"PROGRAM_REV\", ");
            sQuery.AppendLine("        T2.START_QTY, ");
            sQuery.AppendLine("        T2.PARENT, ");
            sQuery.AppendLine("        T2.FLAGS, ");
            sQuery.AppendLine("        T2.SCREEN_FLAG, ");
            sQuery.AppendLine("        T2.SCRAP_FLAG, ");
            sQuery.AppendLine("        T2.DGRADE_FLAG, ");
            sQuery.AppendLine("        T2.VENDOR_ID, ");
            sQuery.AppendLine("        T2.LOADBD, ");
            sQuery.AppendLine("        T2.HOSTNAME, ");
            sQuery.AppendLine("        T2.TESTHEAD, ");
            sQuery.AppendLine("        T2.SW_REV, ");
            sQuery.AppendLine("        T2.USP_REV, ");
            sQuery.AppendLine("        T2.MAXX AS \"MAX_X\", ");
            sQuery.AppendLine("        T2.MAXY AS \"MAX_Y\", ");
            sQuery.AppendLine("        T2.WAF_FLAT, ");
            sQuery.AppendLine("        T2.COMMENTS, ");
            sQuery.AppendLine("        T2.SCRIBE_LOT, ");
            sQuery.AppendLine("        T2.TEMP, ");
            sQuery.AppendLine("        T2.TEMP_TYPE, ");
            sQuery.AppendLine("        T2.TESTER_TYPE, ");
            sQuery.AppendLine("        T2.PGMPATH, ");
            sQuery.AppendLine("        T2.SPC_FLAG, ");
            sQuery.AppendLine("        T2.SPC_RULE, ");
            sQuery.AppendLine("        T2.DATA_SOURCE, ");
            sQuery.AppendLine("        T2.LOT_MODE, ");
            sQuery.AppendLine("        T2.IMMED_REPROBE, ");
            sQuery.AppendLine("        T2.DELAYED_REPROBE, ");
            sQuery.AppendLine("        T2.TRUNCATION, ");
            sQuery.AppendLine("        T2.REF_DIEX AS \"REF_DIE_X\", ");
            sQuery.AppendLine("        T2.REF_DIEY AS \"REF_DIE_Y\", ");
            sQuery.AppendLine("        T2.T7SCRIBE, ");
            sQuery.AppendLine("        T2.M12SCRIBE, ");
            sQuery.AppendLine("        T2.MERGE_FLAG, ");
            sQuery.AppendLine("        T2.TWFLAGS, ");
            sQuery.AppendLine("        T2.TFVERSION, ");
            sQuery.AppendLine("        T2.ESDA_ID, ");
            sQuery.AppendLine("        T2.WAF_DIAM_MEAS, ");
            sQuery.AppendLine("        T2.XREF_VECTOR_MEAS, ");
            sQuery.AppendLine("        T2.YREF_VECTOR_MEAS, ");
            sQuery.AppendLine("        T2.SETUP_TIMESIZE, ");
            sQuery.AppendLine("        T2.LOTDEF_DATABASE, ");
            sQuery.AppendLine("        T2.MAXSITE, ");
            sQuery.AppendLine("        T2.AC_RULE_SEQ, ");
            sQuery.AppendLine("        T2.PROBECARD_TOUCHDOWNS, ");
            sQuery.AppendLine("        T2.STEPSIZE_X, ");
            sQuery.AppendLine("        T2.STEPSIZE_Y, ");
            sQuery.AppendLine("        T2.BURNIN_TYPE, ");
            sQuery.AppendLine("        T2.PROGRAM_MODE, ");
            sQuery.AppendLine("        T2.LOTCONTROL_MODE, ");
            sQuery.AppendLine("        T2.LC_FLAGS, ");
            sQuery.AppendLine("        T2.AC_FLAGS, ");
            sQuery.AppendLine("        T2.SMSFAMILY_NAME, ");
            sQuery.AppendLine("        T2.LOT_CLOSE, ");
            sQuery.AppendLine("        T2.SPC_ENABLE, ");
            sQuery.AppendLine("        T2.SPC_STATUS, ");
            sQuery.AppendLine("        T2.SCREEN_TYPE, ");
            sQuery.AppendLine("        T2.NUM_DIE, ");
            sQuery.AppendLine("        T2.TW_VALID_DATA_CNT, ");
            sQuery.AppendLine("        T2.DATATYPE_FLAG, ");
            sQuery.AppendLine("        T2.FORCE_SPCFAIL, ");
            sQuery.AppendLine("        T2.LOTINFO_SEQ, ");
            sQuery.AppendLine("        T2.AC_DIE_CNT, ");
            sQuery.AppendLine("        T2.MOD_TIME, ");
            sQuery.AppendLine("        T2.PPD_SEQ ");
            //sQuery.AppendLine("        T2.PDM_SEQ ");
            sQuery.AppendLine("   FROM LOT T1, WAFER T2");
            sQuery.AppendLine("  WHERE 1 = 1 ");
            sQuery.AppendLine("        AND T1.LOT_SEQ = T2.LOT_SEQ ");
            sQuery.AppendLine("        AND T1.LOT = :lot ");
            sQuery.AppendLine("        AND T1.LOT_SEQ = :lotseq ");
            sQuery.AppendLine("  ORDER BY WAFER_SEQ ");

            //--

            dicParams = new Dictionary<string, Parameter>();

            #region [ Setting Parameter ]

            Parameter p = new Parameter();
            p.Type = DbType.String;
            p.Value = lot;
            dicParams.Add("lot", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = lotseq;
            dicParams.Add("lotseq", p);

            #endregion [ Setting Parameter ]
            //--

            DataTable dt = ExecuteQuery(
                sQuery.ToString(),
                dicParams
                );

            return dt;
        }

        #endregion [ Wafer ]

        //-----------------------------------------------------------------------------------

        #region [ Program ]

        public DataTable GetProgram(
            string program,
            string testarea,
            string device
            )
        {
            Dictionary<string, Parameter> dicParams = null;
            StringBuilder sQuery = new StringBuilder();

            //--

            sQuery.AppendLine(" SELECT FACTORY, ");
            sQuery.AppendLine("        PROGRAM, ");
            sQuery.AppendLine("        TEST_AREA, ");
            sQuery.AppendLine("        DEVICE, ");
            sQuery.AppendLine("        TO_CHAR(CREATE_TIME, 'YYYYMMDDHH24MISS') AS \"CREATETIME\", ");
            sQuery.AppendLine("        'MIGRATION' AS CREATEUSER, ");
            sQuery.AppendLine("        TO_CHAR(UPDATE_TIME, 'YYYYMMDDHH24MISS') AS \"UPDATETIME\", ");
            sQuery.AppendLine("        'MIGRATION' AS UPDATEUSER ");
            sQuery.AppendLine("   FROM (  SELECT DECODE(FACILITY, 'PROBE1', 'FAB2', 'FAB1') AS \"FACTORY\", ");
            sQuery.AppendLine("                  PROGRAM, ");
            sQuery.AppendLine("                  TEST_AREA, ");
            sQuery.AppendLine("                  DEVICE, ");
            sQuery.AppendLine("                  MIN (START_TIME) AS \"CREATE_TIME\", ");
            sQuery.AppendLine("                  MAX (START_TIME) AS \"UPDATE_TIME\" ");
            sQuery.AppendLine("             FROM LOT ");
            sQuery.AppendLine("            WHERE 1 = 1 ");
            sQuery.AppendLine("                  AND FACILITY IN ('ANAMNTFAB', 'PROBE1') ");
            sQuery.AppendLine("                  AND PROGRAM = :program ");
            sQuery.AppendLine("                  AND TEST_AREA = :testarea ");
            sQuery.AppendLine("                  AND DEVICE = :device ");
            sQuery.AppendLine("                  AND START_TIME >= SYSDATE - 730 ");
            sQuery.AppendLine("                  AND END_TIME < SYSDATE ");
            sQuery.AppendLine("         GROUP BY DECODE(FACILITY, 'PROBE1', 'FAB2', 'FAB1'), ");
            sQuery.AppendLine("                  PROGRAM, ");
            sQuery.AppendLine("                  TEST_AREA, ");
            sQuery.AppendLine("                  DEVICE) ");
            sQuery.AppendLine("  WHERE 1 = 1 ");

            //--

            dicParams = new Dictionary<string, Parameter>();

            #region [ Setting Parameter ]

            Parameter p = new Parameter();
            p.Type = DbType.String;
            p.Value = program;
            dicParams.Add("program", p);

            //--

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = testarea;
            dicParams.Add("testarea", p);

            //--

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = device;
            dicParams.Add("device", p);

            #endregion [ Setting Parameter ]

            //--

            DataTable dt = ExecuteQuery(
                sQuery.ToString(),
                dicParams
                );

            return dt;
        }

        #endregion [ Program ]

        //-----------------------------------------------------------------------------------

        #region [ Program Parameter ]

        public DataTable GetProgramParameter(
            string factory,
            string program
            )
        {
            Dictionary<string, Parameter> dicParams = null;
            StringBuilder sQuery = new StringBuilder();

            //--

            sQuery.AppendLine(" SELECT :factory AS \"FACTORY\", ");
            sQuery.AppendLine("        PROGRAM AS \"PROGRAM\", ");
            sQuery.AppendLine("        0 PROGRAM_REV,");// DENSE_RANK () OVER (PARTITION BY PROGRAM ORDER BY PPD_SEQ) AS \"PROGRAM_REV\", ");
            sQuery.AppendLine("        PPD_SEQ, ");
            sQuery.AppendLine("        ROW_NUMBER () OVER (PARTITION BY PPD_SEQ ORDER BY PARM) AS \"PARAM_INDEX\", ");
            sQuery.AppendLine("        PARM AS \"PARAM_NAME\", ");
            sQuery.AppendLine("        PARM_TYPE AS \"PARAM_TYPE\", ");
            sQuery.AppendLine("        PARM_DESC AS \"PARAM_DESC\", ");
            sQuery.AppendLine("        SYSDATE CREATE_TIME, ");// TO_CHAR(SYSDATE, 'YYYYMMDDHH24MISS') AS \"CREATE_TIME\", ");
            sQuery.AppendLine("        'MIGRATION' AS \"CREATE_USER\", ");
            sQuery.AppendLine("        SYSDATE UPDATE_TIME, ");// TO_CHAR(SYSDATE, 'YYYYMMDDHH24MISS') AS \"UPDATE_TIME\", ");
            sQuery.AppendLine("        'MIGRATION' AS \"UPDATE_USER\", ");
            sQuery.AppendLine(string.Format("        '{0}'||SUBSTR(TABLE_NAME,2) AS \"TABLE_NAME\", ", strTableName));
            //sQuery.AppendLine(string.Format("        '{0}' || PROGRAM AS \"TABLE_NAME\", ", strTableName));
            sQuery.AppendLine("        0 AS \"DECIMAL_PLACES\", ");
            sQuery.AppendLine("        RUNTIME_DEFINED AS \"RUNTIME_DEFINED\", ");
            sQuery.AppendLine("        UOM, ");
            sQuery.AppendLine("        SPEC_LOW AS \"LSL\", ");
            sQuery.AppendLine("        TARGET AS \"TARGET\", ");
            sQuery.AppendLine("        SPEC_HI AS \"USL\", ");
            sQuery.AppendLine("        LCL, ");
            sQuery.AppendLine("        UCL, ");
            sQuery.AppendLine("        TEST_LOW, ");
            sQuery.AppendLine("        TEST_HIGH ");
            sQuery.AppendLine("   FROM PROGRAM_PARM_DEF");
            sQuery.AppendLine("   WHERE 1 = 1  ");
            sQuery.AppendLine("    AND PROGRAM = :program ");
            sQuery.AppendLine("    AND PPD_SEQ = (");
            sQuery.AppendLine("        SELECT MAX(PPD_SEQ)");
            sQuery.AppendLine("        FROM PROGRAM_PARM_DEF");
            sQuery.AppendLine("        WHERE PROGRAM = :program");
            sQuery.AppendLine("    )");
            sQuery.AppendLine("    AND TABLE_NAME IS NOT NULL ");

            //--

            dicParams = new Dictionary<string, Parameter>();

            #region [ Setting Parameter ]

            //--

            Parameter p = new Parameter();
            p.Type = DbType.String;
            p.Value = program;
            dicParams.Add("program", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = factory;
            dicParams.Add("factory", p);

            //--

            #endregion [ Setting Parameter ]

            DataTable dt = ExecuteQuery(
                sQuery.ToString(),
                dicParams
                );

            return dt;
        }

        public DataTable GetProgramParameterByPpdSeq(
            string factory,
            string ppdSeq
            )
        {
            StringBuilder sQuery = new StringBuilder();

            //--

            sQuery.AppendLine(" SELECT '" + factory + "' AS \"FACTORY\", ");
            sQuery.AppendLine("        PROGRAM AS \"PROGRAM\", ");
            sQuery.AppendLine("        0 PROGRAM_REV,");// DENSE_RANK () OVER (PARTITION BY PROGRAM ORDER BY PPD_SEQ) AS \"PROGRAM_REV\", ");
            sQuery.AppendLine("        PPD_SEQ, ");
            sQuery.AppendLine("        ROW_NUMBER () OVER (PARTITION BY PPD_SEQ ORDER BY PARM) AS \"PARAM_INDEX\", ");
            sQuery.AppendLine("        PARM AS \"PARAM_NAME\", ");
            sQuery.AppendLine("        PARM_TYPE AS \"PARAM_TYPE\", ");
            sQuery.AppendLine("        PARM_DESC AS \"PARAM_DESC\", ");
            sQuery.AppendLine("        SYSDATE CREATE_TIME, ");// TO_CHAR(SYSDATE, 'YYYYMMDDHH24MISS') AS \"CREATE_TIME\", ");
            sQuery.AppendLine("        'MIGRATION' AS \"CREATE_USER\", ");
            sQuery.AppendLine("        SYSDATE UPDATE_TIME, ");// TO_CHAR(SYSDATE, 'YYYYMMDDHH24MISS') AS \"UPDATE_TIME\", ");
            sQuery.AppendLine("        'MIGRATION' AS \"UPDATE_USER\", ");
            sQuery.AppendLine(string.Format("        '{0}'||SUBSTR(TABLE_NAME,2) AS \"TABLE_NAME\", ", strTableName));
            //sQuery.AppendLine(string.Format("        '{0}' || PROGRAM AS \"TABLE_NAME\", ", strTableName));
            sQuery.AppendLine("        0 AS \"DECIMAL_PLACES\", ");
            sQuery.AppendLine("        RUNTIME_DEFINED AS \"RUNTIME_DEFINED\", ");
            sQuery.AppendLine("        UOM, ");
            sQuery.AppendLine("        SPEC_LOW AS \"LSL\", ");
            sQuery.AppendLine("        TARGET AS \"TARGET\", ");
            sQuery.AppendLine("        SPEC_HI AS \"USL\", ");
            sQuery.AppendLine("        LCL, ");
            sQuery.AppendLine("        UCL, ");
            sQuery.AppendLine("        TEST_LOW, ");
            sQuery.AppendLine("        TEST_HIGH ");
            sQuery.AppendLine("   FROM PROGRAM_PARM_DEF");
            sQuery.AppendLine("   WHERE 1 = 1  ");
            sQuery.AppendLine("    AND PPD_SEQ= " + ppdSeq);

            return ExecuteQuery(
                sQuery.ToString()
                );
        }

        #endregion [ Program Parameter ]

        public DataTable GetTableList(
            string program
            )
        {
            Dictionary<string, Parameter> dicParams = null;
            StringBuilder sQuery = new StringBuilder();

            //--

            sQuery.AppendLine(" SELECT TABLE_NAME FROM DATA_TABLES");
            sQuery.AppendLine(" WHERE PROGRAM = :PROGRAM");

            //--

            dicParams = new Dictionary<string, Parameter>();

            #region [ Setting Parameter ]

            //--

            Parameter p = new Parameter();
            p.Type = DbType.String;
            p.Value = program;
            dicParams.Add("PROGRAM", p);

            //--

            #endregion [ Setting Parameter ]

            DataTable dt = ExecuteQuery(
                sQuery.ToString(),
                dicParams
                );

            return dt;
        }

        //-----------------------------------------------------------------------------------

        #region [ Die Info ]
        public DataTable GetMapInfo(
            string factory,
            string program,
            string pdm_seq
            )
        {
            Dictionary<string, Parameter> dicParams = null;
            StringBuilder sQuery = new StringBuilder();

            //--

            sQuery.AppendLine(" SELECT :factory AS FACTORY, ");
            sQuery.AppendLine("        PROGRAM, ");
            sQuery.AppendLine("        0 AS PROGRAM_REV, ");
            sQuery.AppendLine("        RANK() OVER (ORDER BY X,Y DESC) AS DIE_NUM, ");
            sQuery.AppendLine("        X,  ");
            sQuery.AppendLine("        Y,  ");
            sQuery.AppendLine("        MAX(TW_DIETYPE) AS TW_DIETYPE, ");
            sQuery.AppendLine("        MAX(DIETYPE_DESC) AS DIETYPE_DESC ");
            sQuery.AppendLine("   FROM PROGRAM_DIEMAP ");
            sQuery.AppendLine("  WHERE 1 = 1 ");
            sQuery.AppendLine("    AND PROGRAM = :program ");
            sQuery.AppendLine("    AND PDM_SEQ = :pdm_seq ");
            sQuery.AppendLine("    GROUP BY PROGRAM, X, Y ");


            //중복 오류 발생 TPSMGR.SYS_C0016790
            //sQuery.AppendLine(" SELECT '' AS \"FACTORY\", ");
            //sQuery.AppendLine("        PROGRAM, ");
            //sQuery.AppendLine("        DENSE_RANK() OVER(PARTITION BY PDM_SEQ ORDER BY PDM_SEQ) AS \"PROGRAM_REV\", ");
            //sQuery.AppendLine("        DIE_NUM, ");
            //sQuery.AppendLine("        X,  ");
            //sQuery.AppendLine("        Y,  ");
            //sQuery.AppendLine("        TW_DIETYPE, ");
            //sQuery.AppendLine("        DIETYPE_DESC ");
            //sQuery.AppendLine("   FROM PROGRAM_DIEMAP ");
            //sQuery.AppendLine("  WHERE 1 = 1 ");
            //sQuery.AppendLine("    AND PROGRAM = :program ");

            //--

            dicParams = new Dictionary<string, Parameter>();

            #region [ Setting Parameter ]

            //--

            Parameter p = new Parameter();
            p.Type = DbType.String;
            p.Value = program;
            dicParams.Add("program", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = factory;
            dicParams.Add("factory", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = pdm_seq;
            dicParams.Add("pdm_seq", p);

            //--

            #endregion [ Setting Parameter ]

            DataTable dt = ExecuteQuery(
                sQuery.ToString(),
                dicParams
                );

            return dt;
        }
        #endregion [ Die Info ]

        //-----------------------------------------------------------------------------------

        #region [ RawData Table ]

        public DataSet GetRawData(
            long waferseq,
            List<TwDataTable> tableinfo
            )
        {
            DataSet ds = new DataSet();
            Dictionary<string, Parameter> dicParams = null;
            StringBuilder sQuery = new StringBuilder();

            //--

            foreach (TwDataTable d in tableinfo)
            {
                //TEST 의 경우 DM 과 다르게 X 는 TRUNC, Y 는 MOD 를 사용 한다.
                sQuery.Clear();
                sQuery.AppendLine(string.Format(" SELECT t1.*, TRUNC(T1.XY / 65536) AS \"X\", MOD(T1.XY, 65536) AS \"Y\" FROM  {0} T1", d.TableName.Replace(strTableName, "T")));
                sQuery.AppendLine("   WHERE 1=1 AND WAFER_SEQ = :waferseq ");

                //--

                dicParams = new Dictionary<string, Parameter>();
                Parameter p = new Parameter();
                p.Type = DbType.String;
                p.Value = waferseq;
                dicParams.Add("waferseq", p);

                //--

                DataTable dt = ExecuteQuery(
                    sQuery.ToString(),
                    dicParams
                    );
                dt.TableName = d.TableName;
                ds.Tables.Add(dt);
            }

            return ds;
        }

        #endregion [ RawData Table ]


        //-----------------------------------------------------------------------------------

        public DataTable GetProgramList()
        {
            return ExecuteQuery(
@"
SELECT DISTINCT CASE WHEN FACILITY = 'PROBE1' THEN 'FAB2' ELSE 'FAB1' END FACTORY, PROGRAM, 'TD_' || PROGRAM TABLE_NAME
FROM LOT
WHERE END_TIME > TO_DATE('20171231235959','YYYYMMDDHH24MISS')
AND FACILITY <> 'ITEST'
ORDER BY 1,2
");
        }

        public DataTable GetProgramListWithABCD()
        {
            return ExecuteQuery(
@"
SELECT FACTORY, A.PROGRAM, 'TD_'||SUBSTR(TABLE_NAME,2) TABLE_NAME
FROM
DATA_TABLES A,
(
    SELECT DISTINCT CASE WHEN FACILITY = 'PROBE1' THEN 'FAB2' ELSE 'FAB1' END FACTORY, PROGRAM
    FROM LOT
    WHERE END_TIME > TO_DATE('20171231235959','YYYYMMDDHH24MISS')
    AND FACILITY <> 'ITEST'
) B
WHERE A.PROGRAM = B.PROGRAM
ORDER BY 1,2,3
");
        }



        public DataTable GetProgramList2(string factory)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format(
@"
SELECT
    '{0}' FACTORY, PROGRAM, TEST_AREA TESTAREA,
    DEVICE,
    SYSDATE CREATE_TIME,
    'AUTO' CREATE_USER
FROM LOT
WHERE END_TIME > TO_DATE('20171231235959','YYYYMMDDHH24MISS')
", factory));
            if (factory == "FAB2")
                sb.AppendLine("AND FACILITY = 'PROBE1'");
            else
                sb.AppendLine("AND FACILITY <> 'PROBE1'");

            sb.AppendLine("ORDER BY PROGRAM");

            return ExecuteQuery(sb.ToString());
        }

        public DataTable GetProgramList3(string factory)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format(
@"
SELECT
    distinct PROGRAM
FROM LOT
WHERE END_TIME > TO_DATE('20171231235959','YYYYMMDDHH24MISS')
", factory));
            if (factory == "FAB2")
                sb.AppendLine("AND FACILITY = 'PROBE1'");
            else
                sb.AppendLine("AND FACILITY <> 'PROBE1'");

            sb.AppendLine("ORDER BY PROGRAM");

            return ExecuteQuery(sb.ToString());
        }

        public DataTable GetProgramTable(string program)
        {
            return ExecuteQuery(String.Format(
@"
SELECT TABLE_NAME FROM DATA_TABLES
WHERE PROGRAM = '{0}'
ORDER BY 1
", program));
        }

        public DataTable GetParameterFromTable(string tableName)
        {
            return ExecuteQuery(String.Format(
@"
SELECT COLUMN_NAME
FROM ALL_TAB_COLUMNS
WHERE OWNER = 'TWROOT'
AND TABLE_NAME = '{0}'
AND COLUMN_ID > 3
ORDER BY COLUMN_ID
", tableName));
        }

        public DataTable GetTTableData(string tTableName, decimal[] waferSeqArr)
        {
            return ExecuteQueryByTable(tTableName, waferSeqArr);
        }

        public static DataTable ExecuteQueryByTable(string tableName, decimal[] waferSeqArr)
        {
            using (OracleCommand comm = new OracleCommand())
            {
                using (OracleConnection conn = new OracleConnection())
                {
                    conn.ConnectionString = m_ESDA_ConnectionString;
                    comm.Connection = conn;
                    comm.CommandText = "SELECT * FROM " + tableName;

                    DataTable dt = new DataTable();
                    OracleDataAdapter da = new OracleDataAdapter(comm);

                    da.FillSchema(dt, SchemaType.Source);
                    da.Dispose();

                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT ");

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (dt.Columns[i].ColumnName != "UNIT_START_TIME")
                            sb.AppendFormat("TRUNC({0},20){0},", dt.Columns[i].ColumnName);
                        else
                            sb.AppendFormat("{0},", dt.Columns[i].ColumnName);
                    }

                    sb.Remove(sb.Length - 1, 1);
                    sb.AppendFormat(" FROM {0} ", tableName);

                    if (waferSeqArr != null && waferSeqArr.Length > 0)
                    {
                        if (waferSeqArr.Length == 1)
                        {
                            sb.Append("WHERE WAFER_SEQ = " + waferSeqArr[0]);
                        }
                        else
                        {
                            sb.Append("WHERE WAFER_SEQ IN ");
                            sb.AppendFormat("({0})", String.Join<decimal>(",", waferSeqArr));
                        }
                    }

                    comm.CommandText = sb.ToString();

                    comm.Connection.Open();
                    OracleDataReader reader = comm.ExecuteReader();

                    if (!reader.HasRows)
                        return dt;

                    while (reader.Read())
                    {
                        DataRow newRow = dt.NewRow();

                        for (int c = 0; c < dt.Columns.Count; c++)
                        {
                            object val = DBNull.Value;

                            try
                            {
                                val = reader.GetValue(c);
                            }
                            catch (Exception ex)
                            {
                                if (dt.Columns[c].DataType == typeof(decimal))
                                    val = Decimal.MaxValue;
                            }

                            newRow[c] = val;
                        }

                        dt.Rows.Add(newRow);
                    }

                    comm.Connection.Close();
                    
                    return dt;
                } 
            }
        }

        public string GetMaxPsSeq(string program)
        {
            return ExecuteScalar(String.Format(
@"
SELECT MAX(PS_SEQ) PS_SEQ
FROM PROGRAM_SETUP_BIN_TAGS
WHERE PROGRAM = '{0}'
", program)).ToString();
        }

        public DataTable GetBin(string program)
        {
            return ExecuteQuery(String.Format(
@"
SELECT BIN, BIN_LABEL, A.BIN_COLOR, GECBINS, A.PS_SEQ
FROM PROGRAM_SETUP_BIN_TAGS A,
(
    SELECT PROGRAM, MAX(PS_SEQ) PS_SEQ
    FROM PROGRAM_SETUP_BIN_TAGS
    GROUP BY PROGRAM
) B
WHERE A.PROGRAM = B.PROGRAM AND A.PS_SEQ = B.PS_SEQ
AND A.PROGRAM = '{0}'
ORDER BY BIN
", program));
        }

        public string Sysdate()
        {
            return ExecuteScalar(
@"
SELECT SYSDATE FROM DUAL
").ToString();
        }

        public DataTable GetProgramMaxPpdSeq(string factory)
        {

            return ExecuteQuery(String.Format(
@"
SELECT MAX(PPD_SEQ) PPD_SEQ
FROM PROGRAM_PARM_DEF
WHERE PROGRAM IN (
    SELECT DISTINCT PROGRAM FROM LOT
    WHERE FACILITY {0} 'PROBE1'
    AND END_TIME > TO_DATE('20180101','YYYYMMDD')
)
GROUP BY PROGRAM
", factory == "FAB2" ? "=" : "<>"));
        }
    }
}
