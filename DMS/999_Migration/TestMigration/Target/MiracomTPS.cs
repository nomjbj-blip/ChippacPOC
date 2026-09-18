using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Infragistics.Win.UltraWinGrid;
using System.Collections;
using FabTwoToMigrationTools;

namespace TestDataMigration_RemotingService
{
    /// <summary>
    /// Database Instance: Oracle 11g
    /// Client information: Oracle ManagedDataAccess Client
    /// </summary>
    public class MiracomTPS : BaseInfo, IMiracomTPS
    {
        public static string TABLESPACE_DATA = "TS_TEST_TBL";
        public static string TABLESPACE_INDEX = "TS_TEST_IDX";
        
        public MiracomTPS()
        {
        }

        public MiracomTPS(string factory)
        {
            Factory = factory;
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

        #region [ CREATE TABLE & ALTER TABLE ]
        public int CreateTable(
            TwResultValue rv
            )
        {
            int iResult = 0;
            StringBuilder sQuery = new StringBuilder();
            List<string> lsItem = new List<string>();
            List<string> lsDuplicate = new List<string>();

            for (int idx = 0; idx < rv.TableName.Count; idx++)
            {
                if (CheckedDataTable(rv.TableName[idx].TableName) > 0)
                    continue;

                //--

                sQuery.Clear();
                sQuery.AppendLine(string.Format(" CREATE TABLE TPSMGR.{0} ", rv.TableName[idx].TableName));
                sQuery.AppendLine(" ( ");
                sQuery.AppendLine("    WAFER_SEQ      NUMBER NOT NULL, ");
                sQuery.AppendLine("    DIE_NUM        NUMBER NOT NULL, ");
                sQuery.AppendLine("    DIEPROBE_CNT   NUMBER,  ");

                for (int pidx = 0; pidx < rv.TableName[idx].Params.Count; pidx++)
                {
                    if (lsDuplicate.IndexOf(rv.TableName[idx].Params[pidx].Name) > -1)
                        continue;

                    lsDuplicate.Add(rv.TableName[idx].Params[pidx].Name);
                    lsItem.Add(string.Format("    {0}   {1}  ", rv.TableName[idx].Params[pidx].Name, ConvertStringToOracleType(rv.TableName[idx].Params[pidx].Type)));
                }

                sQuery.AppendLine(string.Join(",", lsItem.ToArray()));
                sQuery.AppendLine(" ) ");
                sQuery.AppendLine(" TABLESPACE TS_TEST_DATA ");

                iResult = base.ExecuteNonQuery(
                    sQuery.ToString()
                    );
                
                sQuery.Clear();
                sQuery.AppendLine(string.Format(" ALTER TABLE TPSMGR.{0} ADD ( ", rv.TableName[idx].TableName));
                sQuery.AppendLine("   PRIMARY KEY ");
                sQuery.AppendLine("   (WAFER_SEQ, DIE_NUM) ");
                sQuery.AppendLine("   USING INDEX ");
                sQuery.AppendLine("     TABLESPACE TS_TEST_IDX ");
                sQuery.AppendLine("     PCTFREE    10 ");
                sQuery.AppendLine("     INITRANS   2 ");
                sQuery.AppendLine("     MAXTRANS   255 ");
                sQuery.AppendLine("     STORAGE    ( ");
                sQuery.AppendLine("                 INITIAL          64K ");
                sQuery.AppendLine("                 NEXT             1M ");
                sQuery.AppendLine("                 MAXSIZE          UNLIMITED ");
                sQuery.AppendLine("                 MINEXTENTS       1 ");
                sQuery.AppendLine("                 MAXEXTENTS       UNLIMITED ");
                sQuery.AppendLine("                 PCTINCREASE      0 ");
                sQuery.AppendLine("                 FREELISTS        1 ");
                sQuery.AppendLine("                 FREELIST GROUPS  1 ");
                sQuery.AppendLine("                 BUFFER_POOL      DEFAULT ");
                sQuery.AppendLine("                 FLASH_CACHE      DEFAULT ");
                sQuery.AppendLine("                 CELL_FLASH_CACHE DEFAULT ");
                sQuery.AppendLine("                ) ");
                sQuery.AppendLine("   ENABLE VALIDATE) ");

                iResult = base.ExecuteNonQuery(
                    sQuery.ToString()
                    );
            }

            return iResult;
        }

        #endregion [ CREATE TABLE & ALTER TABLE ]
        #endregion [ Checked TPS DataTable ]

        //-----------------------------------------------------------------------------------

        #region [ TPQ_DATA_TABLES ]

        public int CheckedDataTables(
            ref TwResultValue rv
            )
        {
            Dictionary<string, Parameter> dicParams = null;
            StringBuilder sQuery = new StringBuilder();

            //--
            /// Sql Query
            //--

            sQuery.AppendLine("SELECT FACTORY, PROGRAM, TABLE_NAME FROM TQP_DATA_TABLES ");
            sQuery.AppendLine(" WHERE 1=1");
            sQuery.AppendLine("       AND FACTORY = :factory");
            sQuery.AppendLine("       AND PROGRAM = :program");

            //--

            dicParams = new Dictionary<string, Parameter>();
            #region [ Setting Parameter ]

            Parameter p = new Parameter();
            p.Type = DbType.String;
            p.Value = rv.Factory;
            dicParams.Add("factory", p);

            //--

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = rv.Program;
            dicParams.Add("program", p);

            #endregion [ Setting Parameter ]

            //--

            DataTable checkedDt = base.ExecuteQuery(
                sQuery.ToString(),
                dicParams
                );

            if (checkedDt != null && checkedDt.Rows.Count > 0)
            {
                if (rv.TableName == null)
                    rv.TableName = new List<TwDataTable>();

                foreach (DataRow row in checkedDt.Rows)
                {
                    rv.TableName.Add(new TwDataTable(row["TABLE_NAME"].ToString()));
                }
            }

            return checkedDt.Rows.Count;
        }

        //-----------------------------------------------------------------------------------

        public int SetDataTable(
            TwResultValue rv,
            DataTable dt
            )
        {
            int iRet = 0;
            int iBindCnt = 0;
            Dictionary<string, DataTableParameters> dicParams = null;
            StringBuilder sQuery = new StringBuilder();

            //--
            /// Sql Query
            //--

            sQuery.AppendLine(" INSERT INTO TQP_DATA_TABLES(FACTORY, PROGRAM, TABLE_NAME) ");
            sQuery.AppendLine("        VALUES (:FACTORY, :PROGRAM, :TABLE_NAME)");

            //--

            dicParams = new Dictionary<string, DataTableParameters>();
            #region [ Setting Parameter ]

            #region [ Setting Parameter ]
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    DataTableParameters param = new DataTableParameters();

                    //--

                    if (dicParams.ContainsKey(col.ColumnName))
                    {
                        param = dicParams[col.ColumnName];
                        param.Values.Add(row[col]);
                        dicParams[col.ColumnName] = param;
                    }
                    else
                    {
                        if (param.Values == null)
                            param.Values = new ArrayList();

                        param.DataTableColumnType = base.GetDbTypeByName(col.DataType);
                        param.Values.Add(row[col]);
                        dicParams.Add(col.ColumnName, param);
                    }
                    iBindCnt = param.Values.Count;
                }
            }
            #endregion [ Setting Parameter ]

            //--

            #endregion [ Setting Parameter ]

            //--


            iRet =  base.ExecuteNonQuery(
                sQuery.ToString(),
                iBindCnt,
                dicParams
                );

            CheckedDataTables(ref rv);

            return iRet;

        }

        #endregion [ TPQ_DATA_TABLES ]

        //-----------------------------------------------------------------------------------

        #region [ TQP_LOT ]

        public int CheckedLot(
            ref TwLotInfo lotinfo
            )
        {
            Dictionary<string, Parameter> dicParams = null;
            StringBuilder sQuery = new StringBuilder();

            //--
            /// Sql Query
            //--

            sQuery.AppendLine(" SELECT * ");
            sQuery.AppendLine("   FROM TQP_LOT ");
            sQuery.AppendLine("  WHERE 1 = 1 ");
            sQuery.AppendLine("        AND FACTORY = :factory ");
            sQuery.AppendLine("        AND LOT_ID = :lotid ");
            sQuery.AppendLine("        AND TEST_AREA = :testarea ");

            //--

            dicParams = new Dictionary<string, Parameter>();
            #region [ Setting Parameter ]

            Parameter p = new Parameter();
            p.Type = DbType.String;
            p.Value = lotinfo.Factory;
            dicParams.Add("factory", p);

            //--

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = lotinfo.LotID;
            dicParams.Add("lotid", p);

            //--

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = lotinfo.TestArea;
            dicParams.Add("testarea", p);

            #endregion [ Setting Parameter ]

            //--

            DataTable checkedDt = base.ExecuteQuery(
                sQuery.ToString(),
                dicParams
                );

            if (checkedDt != null && checkedDt.Rows.Count > 0)
            {
                lotinfo.Device = checkedDt.Rows[0]["DEVICE"].ToString();
                lotinfo.LotSequence_Next = long.Parse(checkedDt.Rows[0]["LOT_SEQ"].ToString());
            }
            else
            {
                lotinfo.LotSequence_Next = -1;
            }
            return checkedDt.Rows.Count;
        }

        //-----------------------------------------------------------------------------------

        public int SetLot(
            ref TwLotInfo lotinfo,
            UltraGridRow row
            )
        {
            int iResult = -1;
            Dictionary<string, Parameter> dicParams = null;
            StringBuilder sQuery = new StringBuilder();

            sQuery.AppendLine(" INSERT INTO TQP_LOT (FACTORY, ");
            sQuery.AppendLine("                      LOT_SEQ, ");
            sQuery.AppendLine("                      LOT_ID, ");
            sQuery.AppendLine("                      LOT_TYPE, ");
            sQuery.AppendLine("                      FABLOT, ");
            sQuery.AppendLine("                      TEST_AREA, ");
            sQuery.AppendLine("                      TEST_TYPE, ");
            sQuery.AppendLine("                      LOG_POINT, ");
            sQuery.AppendLine("                      OPERATION, ");
            sQuery.AppendLine("                      PROGRAM, ");
            sQuery.AppendLine("                      DEVICE, ");
            sQuery.AppendLine("                      SMS_DEVICE, ");
            sQuery.AppendLine("                      SMS_ITEM_ID, ");
            sQuery.AppendLine("                      DEVICE_ALIAS, ");
            sQuery.AppendLine("                      FAMILY, ");
            sQuery.AppendLine("                      START_TIME, ");
            sQuery.AppendLine("                      END_TIME, ");
            sQuery.AppendLine("                      WAFER_DIAM, ");
            sQuery.AppendLine("                      COMMENTS, ");
            sQuery.AppendLine("                      MERGE_FLAG, ");
            sQuery.AppendLine("                      CMF_FIELD01, ");
            sQuery.AppendLine("                      CMF_FIELD02, ");
            sQuery.AppendLine("                      CMF_FIELD03, ");
            sQuery.AppendLine("                      CMF_FIELD04, ");
            sQuery.AppendLine("                      CMF_FIELD05) ");
            sQuery.AppendLine("      VALUES (:factory, ");
            sQuery.AppendLine("              LOT_SEQ.NEXTVAL, ");
            sQuery.AppendLine("              :lotid, ");
            sQuery.AppendLine("              :lottype, ");
            sQuery.AppendLine("              :fablot, ");
            sQuery.AppendLine("              :testarea, ");
            sQuery.AppendLine("              :testtype, ");
            sQuery.AppendLine("              :logpoint, ");
            sQuery.AppendLine("              :operation, ");
            sQuery.AppendLine("              :program, ");
            sQuery.AppendLine("              :device, ");
            sQuery.AppendLine("              :smsdevice, ");
            sQuery.AppendLine("              :smsitemid, ");
            sQuery.AppendLine("              :devicealias, ");
            sQuery.AppendLine("              :family, ");
            sQuery.AppendLine("              TO_DATE(:starttime, 'YYYYMMDDHH24MISS'), ");
            sQuery.AppendLine("              TO_DATE(:endtime, 'YYYYMMDDHH24MISS'), ");
            sQuery.AppendLine("              :waferdiam, ");
            sQuery.AppendLine("              :comments, ");
            sQuery.AppendLine("              :mergeflag, ");
            sQuery.AppendLine("              :cmffield01, ");
            sQuery.AppendLine("              :cmffield02, ");
            sQuery.AppendLine("              :cmffield03, ");
            sQuery.AppendLine("              :cmffield04, ");
            sQuery.AppendLine("              :cmffield05) ");

            dicParams = new Dictionary<string, Parameter>();
            #region [ Settring Parameter ]
            Parameter p = new Parameter();
            p.Type = DbType.String;
            p.Value = lotinfo.Factory;
            dicParams.Add("factory", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = lotinfo.LotID;
            dicParams.Add("lotid", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = lotinfo.LotType;
            dicParams.Add("lottype", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = row.Cells["FABLOT"].Text;
            dicParams.Add("fablot", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = lotinfo.TestArea;
            dicParams.Add("testarea", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = row.Cells["TEST_TYPE"].Text;
            dicParams.Add("testtype", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = row.Cells["LOGPOINT"].Text;
            dicParams.Add("logpoint", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = row.Cells["OPERATION"].Text;
            dicParams.Add("operation", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = lotinfo.Program;
            dicParams.Add("program", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = lotinfo.Device;
            dicParams.Add("device", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = row.Cells["SMS_DEVICE"].Text;
            dicParams.Add("smsdevice", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = row.Cells["SMS_ITEM_ID"].Text;
            dicParams.Add("smsitemid", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = row.Cells["DEVICE_ALIAS"].Text;
            dicParams.Add("devicealiaS", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = row.Cells["FAMILY"].Text;
            dicParams.Add("family", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = row.Cells["START_TIME"].Text;
            dicParams.Add("starttime", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = row.Cells["END_TIME"].Text;
            dicParams.Add("endtime", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = row.Cells["WAFER_DIAM"].Text;
            dicParams.Add("waferdiam", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = row.Cells["COMMENTS"].Text;
            dicParams.Add("comments", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = row.Cells["MERGE_FLAG"].Text;
            dicParams.Add("mergeflag", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = string.Empty;
            dicParams.Add("cmffield01", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = string.Empty;
            dicParams.Add("cmffield02", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = string.Empty;
            dicParams.Add("cmffield03", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = string.Empty;
            dicParams.Add("cmffield04", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = string.Empty;
            dicParams.Add("cmffield05", p);

            #endregion [ Setting Parameter ]


            iResult = ExecuteNonQuery(
                Constract.FUNCATION_NAME_LOT,
                sQuery.ToString(),
                dicParams
                );

 
            CheckedLot(ref lotinfo);


            return iResult;
        }

        #endregion [ TQP_LOT ]

        //-----------------------------------------------------------------------------------

        #region [ TQP_WAFER ]
        public int CheckedWafer(
            ref TwResultValue rv
            )
        {
            Dictionary<string, Parameter> dicParams = null;
            StringBuilder sQuery = new StringBuilder();

            //--
            /// Sql Query
            //--

            sQuery.AppendLine(" SELECT * ");
            sQuery.AppendLine("   FROM TQP_WAFER ");
            sQuery.AppendLine("  WHERE 1 = 1 ");
            sQuery.AppendLine("        AND FACTORY = :factory ");
            sQuery.AppendLine("        AND LOT_SEQ = :lotseq ");
            sQuery.AppendLine("        AND WAFER_ID = :waferid ");

            //--

            dicParams = new Dictionary<string, Parameter>();
            #region [ Setting Parameter ]

            Parameter p = new Parameter();
            p.Type = DbType.String;
            p.Value = rv.Factory;
            dicParams.Add("factory", p);

            //--

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = rv.LotSequence_Next;
            dicParams.Add("lotseq", p);

            //--

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = rv.WaferID;
            dicParams.Add("waferid", p);

            #endregion [ Setting Parameter ]

            //--

            DataTable checkedDt = base.ExecuteQuery(
                sQuery.ToString(),
                dicParams
                );

            if (checkedDt != null && checkedDt.Rows.Count > 0)
            {
                rv.WaferSequence_Next = long.Parse(checkedDt.Rows[0]["WAFER_SEQ"].ToString());
            }

            return checkedDt.Rows.Count;

        }

        public int SetWafer(
            ref TwResultValue rv,
            DataRow row
            )
        {
            int iResult = -1;
            Dictionary<string, Parameter> dicParams = null;
            StringBuilder sQuery = new StringBuilder();

            sQuery.AppendLine(" INSERT INTO TQP_WAFER (FACTORY, ");
            sQuery.AppendLine("                        WAFER_SEQ, ");
            sQuery.AppendLine("                        LOT_SEQ, ");
            sQuery.AppendLine("                        WAFER_ID, ");
            sQuery.AppendLine("                        PROBE_CNT, ");
            sQuery.AppendLine("                        START_TIME, ");
            sQuery.AppendLine("                        END_TIME, ");
            sQuery.AppendLine("                        LOAD_TIME, ");
            sQuery.AppendLine("                        TESTER, ");
            sQuery.AppendLine("                        PROBER, ");
            sQuery.AppendLine("                        PROBE_CARD, ");
            sQuery.AppendLine("                        HANDLER_ID, ");
            sQuery.AppendLine("                        OPERATOR, ");
            sQuery.AppendLine("                        PROGRAM, ");
            sQuery.AppendLine("                        PROGRAM_REV, ");
            sQuery.AppendLine("                        START_QTY, ");
            sQuery.AppendLine("                        PARENT, ");
            sQuery.AppendLine("                        FLAGS, ");
            sQuery.AppendLine("                        SCREEN_FLAG, ");
            sQuery.AppendLine("                        SCRAP_FLAG, ");
            sQuery.AppendLine("                        DGRADE_FLAG, ");
            sQuery.AppendLine("                        VENDOR_ID, ");
            sQuery.AppendLine("                        LOADBD, ");
            sQuery.AppendLine("                        HOSTNAME, ");
            sQuery.AppendLine("                        TESTHEAD, ");
            sQuery.AppendLine("                        SW_REV, ");
            sQuery.AppendLine("                        USP_REV, ");
            sQuery.AppendLine("                        MAX_X, ");
            sQuery.AppendLine("                        MAX_Y, ");
            sQuery.AppendLine("                        WAF_FLAT, ");
            sQuery.AppendLine("                        COMMENTS, ");
            sQuery.AppendLine("                        SCRIBE_LOT, ");
            sQuery.AppendLine("                        TEMP, ");
            sQuery.AppendLine("                        TEMP_TYPE, ");
            sQuery.AppendLine("                        TESTER_TYPE, ");
            sQuery.AppendLine("                        PGMPATH, ");
            sQuery.AppendLine("                        SPC_FLAG, ");
            sQuery.AppendLine("                        SPC_RULE, ");
            sQuery.AppendLine("                        DATA_SOURCE, ");
            sQuery.AppendLine("                        LOT_MODE, ");
            sQuery.AppendLine("                        IMMED_REPROBE, ");
            sQuery.AppendLine("                        DELAYED_REPROBE, ");
            sQuery.AppendLine("                        TRUNCATION, ");
            sQuery.AppendLine("                        REF_DIE_X, ");
            sQuery.AppendLine("                        REF_DIE_Y, ");
            sQuery.AppendLine("                        T7SCRIBE, ");
            sQuery.AppendLine("                        M12SCRIBE, ");
            sQuery.AppendLine("                        MERGE_FLAG, ");
            sQuery.AppendLine("                        TWFLAGS, ");
            sQuery.AppendLine("                        TFVERSION, ");
            sQuery.AppendLine("                        ESDA_ID, ");
            sQuery.AppendLine("                        WAF_DIAM_MEAS, ");
            sQuery.AppendLine("                        XREF_VECTOR_MEAS, ");
            sQuery.AppendLine("                        YREF_VECTOR_MEAS, ");
            sQuery.AppendLine("                        SETUP_TIMESIZE, ");
            sQuery.AppendLine("                        LOTDEF_DATABASE, ");
            sQuery.AppendLine("                        MAXSITE, ");
            sQuery.AppendLine("                        AC_RULE_SEQ, ");
            sQuery.AppendLine("                        PROBECARD_TOUCHDOWNS, ");
            sQuery.AppendLine("                        STEPSIZE_X, ");
            sQuery.AppendLine("                        STEPSIZE_Y, ");
            sQuery.AppendLine("                        BURNIN_TYPE, ");
            sQuery.AppendLine("                        PROGRAM_MODE, ");
            sQuery.AppendLine("                        LOTCONTROL_MODE, ");
            sQuery.AppendLine("                        LC_FLAGS, ");
            sQuery.AppendLine("                        AC_FLAGS, ");
            sQuery.AppendLine("                        SMSFAMILY_NAME, ");
            sQuery.AppendLine("                        LOT_CLOSE, ");
            sQuery.AppendLine("                        SPC_ENABLE, ");
            sQuery.AppendLine("                        SPC_STATUS, ");
            sQuery.AppendLine("                        SCREEN_TYPE, ");
            sQuery.AppendLine("                        NUM_DIE, ");
            sQuery.AppendLine("                        TW_VALID_DATA_CNT, ");
            sQuery.AppendLine("                        DATATYPE_FLAG, ");
            sQuery.AppendLine("                        FORCE_SPCFAIL, ");
            sQuery.AppendLine("                        LOTINFO_SEQ, ");
            sQuery.AppendLine("                        AC_DIE_CNT, ");
            sQuery.AppendLine("                        MOD_TIME) ");
            sQuery.AppendLine("      VALUES (:FACTORY, ");
            sQuery.AppendLine("              WAFER_SEQ.NEXTVAL, ");
            sQuery.AppendLine("              :LOT_SEQ, ");
            sQuery.AppendLine("              :WAFER_ID, ");
            sQuery.AppendLine("              :PROBE_CNT, ");
            sQuery.AppendLine("              TO_DATE(:START_TIME, 'YYYYMMDDHH24MISS'), ");
            sQuery.AppendLine("              TO_DATE(:END_TIME, 'YYYYMMDDHH24MISS'), ");
            sQuery.AppendLine("              TO_DATE(:LOAD_TIME, 'YYYYMMDDHH24MISS'), ");
            sQuery.AppendLine("              :TESTER, ");
            sQuery.AppendLine("              :PROBER, ");
            sQuery.AppendLine("              :PROBE_CARD, ");
            sQuery.AppendLine("              :HANDLER_ID, ");
            sQuery.AppendLine("              :OPERATOR, ");
            sQuery.AppendLine("              :PROGRAM, ");
            sQuery.AppendLine("              :PROGRAM_REV, ");
            sQuery.AppendLine("              :START_QTY, ");
            sQuery.AppendLine("              :PARENT, ");
            sQuery.AppendLine("              :FLAGS, ");
            sQuery.AppendLine("              :SCREEN_FLAG, ");
            sQuery.AppendLine("              :SCRAP_FLAG, ");
            sQuery.AppendLine("              :DGRADE_FLAG, ");
            sQuery.AppendLine("              :VENDOR_ID, ");
            sQuery.AppendLine("              :LOADBD, ");
            sQuery.AppendLine("              :HOSTNAME, ");
            sQuery.AppendLine("              :TESTHEAD, ");
            sQuery.AppendLine("              :SW_REV, ");
            sQuery.AppendLine("              :USP_REV, ");
            sQuery.AppendLine("              :MAX_X, ");
            sQuery.AppendLine("              :MAX_Y, ");
            sQuery.AppendLine("              :WAF_FLAT, ");
            sQuery.AppendLine("              :COMMENTS, ");
            sQuery.AppendLine("              :SCRIBE_LOT, ");
            sQuery.AppendLine("              :TEMP, ");
            sQuery.AppendLine("              :TEMP_TYPE, ");
            sQuery.AppendLine("              :TESTER_TYPE, ");
            sQuery.AppendLine("              :PGMPATH, ");
            sQuery.AppendLine("              :SPC_FLAG, ");
            sQuery.AppendLine("              :SPC_RULE, ");
            sQuery.AppendLine("              :DATA_SOURCE, ");
            sQuery.AppendLine("              :LOT_MODE, ");
            sQuery.AppendLine("              :IMMED_REPROBE, ");
            sQuery.AppendLine("              :DELAYED_REPROBE, ");
            sQuery.AppendLine("              :TRUNCATION, ");
            sQuery.AppendLine("              :REF_DIE_X, ");
            sQuery.AppendLine("              :REF_DIE_Y, ");
            sQuery.AppendLine("              :T7SCRIBE, ");
            sQuery.AppendLine("              :M12SCRIBE, ");
            sQuery.AppendLine("              :MERGE_FLAG, ");
            sQuery.AppendLine("              :TWFLAGS, ");
            sQuery.AppendLine("              :TFVERSION, ");
            sQuery.AppendLine("              :ESDA_ID, ");
            sQuery.AppendLine("              :WAF_DIAM_MEAS, ");
            sQuery.AppendLine("              :XREF_VECTOR_MEAS, ");
            sQuery.AppendLine("              :YREF_VECTOR_MEAS, ");
            sQuery.AppendLine("              :SETUP_TIMESIZE, ");
            sQuery.AppendLine("              :LOTDEF_DATABASE, ");
            sQuery.AppendLine("              :MAXSITE, ");
            sQuery.AppendLine("              :AC_RULE_SEQ, ");
            sQuery.AppendLine("              :PROBECARD_TOUCHDOWNS, ");
            sQuery.AppendLine("              :STEPSIZE_X, ");
            sQuery.AppendLine("              :STEPSIZE_Y, ");
            sQuery.AppendLine("              :BURNIN_TYPE, ");
            sQuery.AppendLine("              :PROGRAM_MODE, ");
            sQuery.AppendLine("              :LOTCONTROL_MODE, ");
            sQuery.AppendLine("              :LC_FLAGS, ");
            sQuery.AppendLine("              :AC_FLAGS, ");
            sQuery.AppendLine("              :SMSFAMILY_NAME, ");
            sQuery.AppendLine("              :LOT_CLOSE, ");
            sQuery.AppendLine("              :SPC_ENABLE, ");
            sQuery.AppendLine("              :SPC_STATUS, ");
            sQuery.AppendLine("              :SCREEN_TYPE, ");
            sQuery.AppendLine("              :NUM_DIE, ");
            sQuery.AppendLine("              :TW_VALID_DATA_CNT, ");
            sQuery.AppendLine("              :DATATYPE_FLAG, ");
            sQuery.AppendLine("              :FORCE_SPCFAIL, ");
            sQuery.AppendLine("              :LOTINFO_SEQ, ");
            sQuery.AppendLine("              :AC_DIE_CNT, ");
            sQuery.AppendLine("              :MOD_TIME) ");

            //--

            dicParams = new Dictionary<string, Parameter>();
            #region [ Setting Parameter ]
            Parameter p = new Parameter(); p.Type = DbType.String; p.Value = row["FACTORY"]; dicParams.Add("FACTORY", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = rv.LotSequence_Next; dicParams.Add("LOT_SEQ", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["WAFER_ID"]; dicParams.Add("WAFER_ID", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["PROBE_CNT"]; dicParams.Add("PROBE_CNT", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["START_TIME"]; dicParams.Add("START_TIME", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["END_TIME"]; dicParams.Add("END_TIME", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["LOAD_TIME"]; dicParams.Add("LOAD_TIME", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["TESTER"]; dicParams.Add("TESTER", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["PROBER"]; dicParams.Add("PROBER", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["PROBE_CARD"]; dicParams.Add("PROBE_CARD", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["HANDLER_ID"]; dicParams.Add("HANDLER_ID", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["OPERATOR"]; dicParams.Add("OPERATOR", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = rv.Program; dicParams.Add("PROGRAM", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = rv.ProgramRevision; dicParams.Add("PROGRAM_REV", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["START_QTY"]; dicParams.Add("START_QTY", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["PARENT"]; dicParams.Add("PARENT", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["FLAGS"]; dicParams.Add("FLAGS", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["SCREEN_FLAG"]; dicParams.Add("SCREEN_FLAG", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["SCRAP_FLAG"]; dicParams.Add("SCRAP_FLAG", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["DGRADE_FLAG"]; dicParams.Add("DGRADE_FLAG", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["VENDOR_ID"]; dicParams.Add("VENDOR_ID", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["LOADBD"]; dicParams.Add("LOADBD", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["HOSTNAME"]; dicParams.Add("HOSTNAME", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["TESTHEAD"]; dicParams.Add("TESTHEAD", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["SW_REV"]; dicParams.Add("SW_REV", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["USP_REV"]; dicParams.Add("USP_REV", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["MAX_X"]; dicParams.Add("MAX_X", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["MAX_Y"]; dicParams.Add("MAX_Y", p);
            //WAF_FLAT 정보는 무시하고 Bottom 으로 사용한다.(전의택 수석 확인)
            p = new Parameter(); p.Type = DbType.String; p.Value = 0; dicParams.Add("WAF_FLAT", p); //row["WAF_FLAT"]; dicParams.Add("WAF_FLAT", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["COMMENTS"]; dicParams.Add("COMMENTS", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["SCRIBE_LOT"]; dicParams.Add("SCRIBE_LOT", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["TEMP"]; dicParams.Add("TEMP", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["TEMP_TYPE"]; dicParams.Add("TEMP_TYPE", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["TESTER_TYPE"]; dicParams.Add("TESTER_TYPE", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["PGMPATH"]; dicParams.Add("PGMPATH", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["SPC_FLAG"]; dicParams.Add("SPC_FLAG", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["SPC_RULE"]; dicParams.Add("SPC_RULE", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["DATA_SOURCE"]; dicParams.Add("DATA_SOURCE", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["LOT_MODE"]; dicParams.Add("LOT_MODE", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["IMMED_REPROBE"]; dicParams.Add("IMMED_REPROBE", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["DELAYED_REPROBE"]; dicParams.Add("DELAYED_REPROBE", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["TRUNCATION"]; dicParams.Add("TRUNCATION", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["REF_DIE_X"]; dicParams.Add("REF_DIE_X", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["REF_DIE_Y"]; dicParams.Add("REF_DIE_Y", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["T7SCRIBE"]; dicParams.Add("T7SCRIBE", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["M12SCRIBE"]; dicParams.Add("M12SCRIBE", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["MERGE_FLAG"]; dicParams.Add("MERGE_FLAG", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["TWFLAGS"]; dicParams.Add("TWFLAGS", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["TFVERSION"]; dicParams.Add("TFVERSION", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["ESDA_ID"]; dicParams.Add("ESDA_ID", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["WAF_DIAM_MEAS"]; dicParams.Add("WAF_DIAM_MEAS", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["XREF_VECTOR_MEAS"]; dicParams.Add("XREF_VECTOR_MEAS", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["YREF_VECTOR_MEAS"]; dicParams.Add("YREF_VECTOR_MEAS", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["SETUP_TIMESIZE"]; dicParams.Add("SETUP_TIMESIZE", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["LOTDEF_DATABASE"]; dicParams.Add("LOTDEF_DATABASE", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["MAXSITE"]; dicParams.Add("MAXSITE", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["AC_RULE_SEQ"]; dicParams.Add("AC_RULE_SEQ", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["PROBECARD_TOUCHDOWNS"]; dicParams.Add("PROBECARD_TOUCHDOWNS", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["STEPSIZE_X"]; dicParams.Add("STEPSIZE_X", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["STEPSIZE_Y"]; dicParams.Add("STEPSIZE_Y", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["BURNIN_TYPE"]; dicParams.Add("BURNIN_TYPE", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["PROGRAM_MODE"]; dicParams.Add("PROGRAM_MODE", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["LOTCONTROL_MODE"]; dicParams.Add("LOTCONTROL_MODE", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["LC_FLAGS"]; dicParams.Add("LC_FLAGS", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["AC_FLAGS"]; dicParams.Add("AC_FLAGS", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["SMSFAMILY_NAME"]; dicParams.Add("SMSFAMILY_NAME", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["LOT_CLOSE"]; dicParams.Add("LOT_CLOSE", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["SPC_ENABLE"]; dicParams.Add("SPC_ENABLE", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["SPC_STATUS"]; dicParams.Add("SPC_STATUS", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["SCREEN_TYPE"]; dicParams.Add("SCREEN_TYPE", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["NUM_DIE"]; dicParams.Add("NUM_DIE", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["TW_VALID_DATA_CNT"]; dicParams.Add("TW_VALID_DATA_CNT", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["DATATYPE_FLAG"]; dicParams.Add("DATATYPE_FLAG", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["FORCE_SPCFAIL"]; dicParams.Add("FORCE_SPCFAIL", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["LOTINFO_SEQ"]; dicParams.Add("LOTINFO_SEQ", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["AC_DIE_CNT"]; dicParams.Add("AC_DIE_CNT", p);
            p = new Parameter(); p.Type = DbType.String; p.Value = row["MOD_TIME"]; dicParams.Add("MOD_TIME", p);
            #endregion [ Setting Parameter ]

            iResult = base.ExecuteNonQuery(
                sQuery.ToString(),
                dicParams
                );

            CheckedWafer(ref rv);

            return iResult;
        }
        #endregion [ TQP_WAFER ]

        //-----------------------------------------------------------------------------------

        #region [ TQP_PROGRAM ]

        public int CheckedProgram(
            TwResultValue rv
            )
        {
            Dictionary<string, Parameter> dicParams = null;
            StringBuilder sQuery = new StringBuilder();

            //--
            /// Sql Query
            //--

            sQuery.AppendLine(" SELECT * ");
            sQuery.AppendLine("   FROM TQP_PROGRAM ");
            sQuery.AppendLine("  WHERE 1 = 1 ");
            sQuery.AppendLine("        AND FACTORY = :factory ");
            sQuery.AppendLine("        AND PROGRAM = :program ");
            sQuery.AppendLine("        AND TESTAREA = :testarea ");
            sQuery.AppendLine("        AND DEVICE = :device ");

            //--

            dicParams = new Dictionary<string, Parameter>();
            #region [ Setting Parameter ]

            Parameter p = new Parameter();
            p.Type = DbType.String;
            p.Value = rv.Factory;
            dicParams.Add("factory", p);

            //--

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = rv.Program;
            dicParams.Add("program", p);

            //--

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = rv.TestArea;
            dicParams.Add("testarea", p);

            //--

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = rv.Device;
            dicParams.Add("device", p);

            #endregion [ Setting Parameter ]

            //--

            DataTable checkedDt = base.ExecuteQuery(
                sQuery.ToString(),
                dicParams
                );

            return checkedDt.Rows.Count;
        }

        //-----------------------------------------------------------------------------------

        public int SetProgram(
            TwResultValue rv,
            DataTable dt
            )
        {
            int iResult = -1;
            int iBindCnt = -1;
            Dictionary<string, DataTableParameters> dicParams = null;
            StringBuilder sQuery = new StringBuilder();

            sQuery.AppendLine(" INSERT INTO TQP_PROGRAM(FACTORY, PROGRAM, TESTAREA, DEVICE, CREATE_TIME, CREATE_USER, UPDATE_TIME, UPDATE_USER) ");
            sQuery.AppendLine("        VALUES (:FACTORY, :PROGRAM, :TEST_AREA, :DEVICE, TO_DATE(:CREATETIME, 'YYYYMMDDHH24MISS'), :CREATEUSER, TO_DATE(:UPDATETIME, 'YYYYMMDDHH24MISS'), :UPDATEUSER) ");

            //--

            dicParams = new Dictionary<string, DataTableParameters>();
            #region [ Setting Parameter ]
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    DataTableParameters param = new DataTableParameters();
                    if (dicParams.ContainsKey(col.ColumnName))
                    {
                        param = dicParams[col.ColumnName];
                        param.Values.Add(row[col]);
                        dicParams[col.ColumnName] = param;
                    }
                    else
                    {
                        if (param.Values == null)
                            param.Values = new ArrayList();

                        param.DataTableColumnType = base.GetDbTypeByName(col.DataType);
                        param.Values.Add(row[col]);
                        dicParams.Add(col.ColumnName, param);
                    }
                    iBindCnt = param.Values.Count;
                }
            }
            #endregion [ Setting Parameter ]

            iResult = base.ExecuteNonQuery(
                sQuery.ToString(),
                iBindCnt,
                dicParams
                );

            CheckedProgram(rv);

            return iResult;
        }

        #endregion [ TQP_PROGRAM ]

        //-----------------------------------------------------------------------------------

        #region [ TQP_PROGRAM_PARAMETER_SPEC ]

        public bool ExistsProgramParameter(
            string program
            )
        {
            StringBuilder sQuery = new StringBuilder();

            sQuery.AppendLine("  SELECT COUNT(*) ");
            sQuery.AppendLine("  FROM TQP_PARASPEC ");
            sQuery.AppendLine("  WHERE 1 = 1  ");
            sQuery.AppendLine("    AND PROGRAM = :program ");

            Dictionary<string, Parameter> dicParams = new Dictionary<string, Parameter>();

            #region [ Setting Parameter ]

            Parameter p = new Parameter();
            p.Type = DbType.String;
            p.Value = program;
            dicParams.Add("program", p);

            //--

            #endregion [ Setting Parameter ]

            object obj = ExecuteScalar(
                sQuery.ToString(),
                dicParams
                );

            return Int32.Parse(obj.ToString()) > 0;
        }

        public int CheckedProgramParameter(
            ref TwResultValue rv
            )
        {
            Dictionary<string, Parameter> dicParams = null;
            StringBuilder sQuery = new StringBuilder();

            //--
            /// Sql Query
            //--

            sQuery.AppendLine(" SELECT * ");
            sQuery.AppendLine("   FROM TQP_PARASPEC ");
            sQuery.AppendLine("  WHERE 1 = 1 ");
            sQuery.AppendLine("        AND FACTORY = :factory ");
            sQuery.AppendLine("        AND PROGRAM = :program ");
            sQuery.AppendLine("  ORDER BY PARAM_INDEX ");

            //--

            dicParams = new Dictionary<string, Parameter>();
            #region [ Setting Parameter ]

            Parameter p = new Parameter();
            p.Type = DbType.String;
            p.Value = rv.Factory;
            dicParams.Add("factory", p);

            //--

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = rv.Program;
            dicParams.Add("program", p);

            #endregion [ Setting Parameter ]

            //--

            DataTable checkedDt = base.ExecuteQuery(
                sQuery.ToString(),
                dicParams
                );

            if (checkedDt != null && checkedDt.Rows.Count > 0)
            {
                string name = string.Empty;
                string type = string.Empty;
                foreach (DataRow row in checkedDt.Rows)
                {
                    name = row["PARAM_NAME"].ToString();
                    type = row["PARAM_TYPE"].ToString();

                    int idx = rv.TableName.FindIndex(delegate(TwDataTable table)
                    {
                        return table.TableName.Equals(row["TABLE_NAME"].ToString());
                    });

                    if(idx >= 0)
                        rv.TableName[idx].Params.Add(new TwParameter(name, type));
                }
            }

            return checkedDt.Rows.Count;

        }

        public int SetProgramParameter(
            ref TwResultValue rv,
            DataTable dt
            )
        {
            int iResult = -1;
            int iBindCnt = -1;
            Dictionary<string, DataTableParameters> dicInsertParams = null;
            Dictionary<string, Parameter> dicUpdateParams = null;
            StringBuilder sQuery = new StringBuilder();

            sQuery.AppendLine(" INSERT INTO TQP_PARASPEC (FACTORY, PROGRAM, PROGRAM_REV, PARAM_INDEX, PARAM_NAME, PARAM_TYPE, PARAM_DESC, CREATE_TIME, CREATE_USER, UPDATE_TIME, UPDATE_USER, TABLE_NAME, DECIMAL_PLACES, RUNTIME_DEFINED, UOM, LSL, TARGET, USL, LCL, UCL, TEST_LOW, TEST_HIGH) ");
            sQuery.AppendLine("                    VALUES (:FACTORY, :PROGRAM, :PROGRAM_REV, :PARAM_INDEX, :PARAM_NAME, :PARAM_TYPE, :PARAM_DESC, TO_DATE(:CREATE_TIME, 'YYYYMMDDHH24MISS'), :CREATE_USER, TO_DATE(:UPDATE_TIME, 'YYYYMMDDHH24MISS'), :UPDATE_USER, :TABLE_NAME, :DECIMAL_PLACES, :RUNTIME_DEFINED, :UOM, :LSL, :TARGET, :USL, :LCL, :UCL, :TEST_LOW, :TEST_HIGH) ");

            //--

            dicInsertParams = new Dictionary<string, DataTableParameters>();
            #region [ Setting Parameter ]
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    DataTableParameters param = new DataTableParameters();
                    if (dicInsertParams.ContainsKey(col.ColumnName))
                    {
                        param = dicInsertParams[col.ColumnName];
                        if (string.Equals(col.ColumnName, "FACTORY"))
                            param.Values.Add(rv.Factory);
                        else
                            param.Values.Add(row[col]);
                        dicInsertParams[col.ColumnName] = param;
                    }
                    else
                    {
                        if (param.Values == null)
                            param.Values = new ArrayList();

                        param.DataTableColumnType = base.GetDbTypeByName(col.DataType);
                        if (string.Equals(col.ColumnName, "FACTORY"))
                            param.Values.Add(rv.Factory);
                        else
                            param.Values.Add(row[col]);
                        dicInsertParams.Add(col.ColumnName, param);
                    }
                    iBindCnt = param.Values.Count;
                }
            }
            #endregion [ Setting Parameter ]

            iResult = base.ExecuteNonQuery(
                sQuery.ToString(),
                iBindCnt,
                dicInsertParams
                );

            //--

            sQuery.Clear();
            sQuery.AppendLine(" UPDATE TQP_PARASPEC ");
            sQuery.AppendLine("    SET PARAM_INDEX = PARAM_INDEX + 2 ");
            sQuery.AppendLine("  WHERE 1=1  ");
            sQuery.AppendLine("    AND FACTORY = :factory ");
            sQuery.AppendLine("    AND PROGRAM = :program ");

            //--

            #region [ Setting Parameter ]

            //--

            dicUpdateParams = new Dictionary<string, Parameter>();
            Parameter p = new Parameter();
            p.Type = DbType.String;
            p.Value = rv.Factory;
            dicUpdateParams.Add("factory", p);

            //--

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = rv.Program;
            dicUpdateParams.Add("program", p);

            //--

            #endregion [ Setting Parameter ]

            //--

            base.ExecuteNonQuery(
                sQuery.ToString(),
                dicUpdateParams
                );

            CheckedProgramParameter(ref rv);

            return iResult;
        }
        #endregion [ TQP_PROGRAM_PARAMETER_SPEC ]

        //-----------------------------------------------------------------------------------


        #region [ Die Info ]

        public int CheckedDieInfo(
            TwResultValue rv
            )
        {
            Dictionary<string, Parameter> dicParams = null;
            StringBuilder sQuery = new StringBuilder();

            //--
            /// Sql Query
            //--

            sQuery.AppendLine(" SELECT * ");
            sQuery.AppendLine("   FROM TQP_USEMAP ");
            sQuery.AppendLine("  WHERE 1 = 1 ");
            sQuery.AppendLine("        AND FACTORY = :factory ");
            sQuery.AppendLine("        AND MAPID = :program ");

            //--

            dicParams = new Dictionary<string, Parameter>();
            #region [ Setting Parameter ]

            Parameter p = new Parameter();
            p.Type = DbType.String;
            p.Value = rv.Factory;
            dicParams.Add("factory", p);

            //--

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = rv.Program;
            dicParams.Add("program", p);

            #endregion [ Setting Parameter ]

            //--

            DataTable checkedDt = base.ExecuteQuery(
                sQuery.ToString(),
                dicParams
                );

            return checkedDt.Rows.Count;
        }

        public int SetDieInfo(
            TwResultValue rv,
            DataTable dt
            )
        {
            int iResult = -1;
            int iBindCnt = -1;
            Dictionary<string, DataTableParameters> dicParams = null;
            StringBuilder sQuery = new StringBuilder();

            sQuery.AppendLine(" INSERT INTO TQP_USEMAP(FACTORY, MAPID, PROGRAM_REV, DIE_NUM, X, Y, TW_DIETYPE, DIETYPE_DESC) ");
            sQuery.AppendLine("   VALUES (:FACTORY, :PROGRAM, :PROGRAM_REV, :DIE_NUM, :X, :Y, :TW_DIETYPE, :DIETYPE_DESC) ");

            //--

            dicParams = new Dictionary<string, DataTableParameters>();
            #region [ Setting Parameter ]
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    DataTableParameters param = new DataTableParameters();
                    if (dicParams.ContainsKey(col.ColumnName))
                    {
                        param = dicParams[col.ColumnName];
                        if (string.Equals(col.ColumnName, "FACTORY"))
                            param.Values.Add(rv.Factory);
                        else
                            param.Values.Add(row[col]);
                        dicParams[col.ColumnName] = param;
                    }
                    else
                    {
                        if (param.Values == null)
                            param.Values = new ArrayList();

                        param.DataTableColumnType = base.GetDbTypeByName(col.DataType);
                        if (string.Equals(col.ColumnName, "FACTORY"))
                            param.Values.Add(rv.Factory);
                        else
                            param.Values.Add(row[col]);
                        dicParams.Add(col.ColumnName, param);
                    }
                    iBindCnt = param.Values.Count;
                }
            }
            #endregion [ Setting Parameter ]


            iResult = base.ExecuteNonQuery(
                sQuery.ToString(),
                iBindCnt,
                dicParams
                );

            CheckedProgram(rv);

            return iResult;
        }

        #endregion [ Die Info ]

        //-----------------------------------------------------------------------------------

        #region [ Raw Data Table ]
        public int CheckedRawData(
            TwResultValue rv
            )
        {
            DataTable dt = null;
            DataTable checkedDt = null;
            Dictionary<string, Parameter> dicParams = null;
            StringBuilder sQuery = new StringBuilder();

            //--
            /// Sql Query
            //--

            foreach (TwDataTable d in rv.TableName)
            {
                sQuery.AppendLine(" SELECT * ");
                sQuery.AppendLine(string.Format("   FROM {0} ", d.TableName));
                sQuery.AppendLine("  WHERE 1 = 1 ");
                sQuery.AppendLine("        AND WAFER_SEQ = :waferseq ");

                //--

                dicParams = new Dictionary<string, Parameter>();
                #region [ Setting Parameter ]

                Parameter p = new Parameter();
                p.Type = DbType.Int64;
                p.Value = rv.WaferSequence_Next;
                dicParams.Add("waferseq", p);

                #endregion [ Setting Parameter ]

                //--

                checkedDt = base.ExecuteQuery(
                    sQuery.ToString(),
                    dicParams
                    );

                if (dt == null)
                    dt = new DataTable();
                dt.Merge(checkedDt);
            }

            return dt.Rows.Count;
        }

        public void SetRawData(
            TwResultValue rv,
            DataSet dsRawData
            )
        {
            Dictionary<string, DataTableParameters> dicParams = null;
            StringBuilder sQuery = new StringBuilder();
            StringBuilder sQValue = new StringBuilder();

            foreach (TwDataTable d in rv.TableName)
            {
                int iBindCnt = -1;
                int iResult = -1;
                DataTable dt = dsRawData.Tables[d.TableName];
                sQuery.Clear();
                sQValue.Clear();

                //--

                sQuery.AppendLine(string.Format(" INSERT INTO TPSMGR.{0} ", d.TableName));
                sQuery.AppendLine("  ( ");
                sQValue.AppendLine(" VALUES ( ");

                for (int icol = 0; icol < dt.Columns.Count; icol++)
                {
                    DataColumn col = dt.Columns[icol];
                    if (string.Equals(col.ColumnName, "XY")) 
                        continue;

                    if (icol == dt.Columns.Count - 1)
                    {
                        sQuery.AppendLine(string.Format("   {0} ", col.ColumnName));
                        sQValue.AppendLine(string.Format("   :{0} ", col.ColumnName));
                    }
                    else
                    {
                        sQuery.AppendLine(string.Format("   {0}, ", col.ColumnName));
                        sQValue.AppendLine(string.Format("   :{0}, ", col.ColumnName));
                    }
                }
                sQuery.AppendLine("  ) ");
                sQValue.AppendLine("  ) ");

                sQuery.AppendLine(sQValue.ToString());

                dicParams = new Dictionary<string, DataTableParameters>();

                #region [ Setting Parameter ]
                long tmp = long.MinValue;
                long x = long.MinValue;
                long y = long.MaxValue;
                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (string.Equals(col.ColumnName, "XY"))
                            continue;

                        DataTableParameters param = new DataTableParameters();
                        if (dicParams.ContainsKey(col.ColumnName))
                        {
                            param = dicParams[col.ColumnName];
                            if (string.Equals(col.ColumnName, "WAFER_SEQ"))
                                param.Values.Add(rv.WaferSequence_Next);
                            else
                                param.Values.Add(row[col]);

                            dicParams[col.ColumnName] = param;

                        }
                        else
                        {
                            if (param.Values == null)
                                param.Values = new ArrayList();

                            param.DataTableColumnType = base.GetDbTypeByName(col.DataType);
                            if (string.Equals(col.ColumnName, "WAFER_SEQ"))
                                param.Values.Add(rv.WaferSequence_Next);
                            else
                                param.Values.Add(row[col]);
                            dicParams.Add(col.ColumnName, param);
                        }
                        iBindCnt = param.Values.Count;
                    }
                }
                #endregion [ Setting Parameter ]


                iResult = base.ExecuteNonQuery(
                    sQuery.ToString(),
                    iBindCnt,
                    dicParams
                    );

                //2019-05-10-정병주 : 프로시저를 실행 한다.

                sQuery.Clear();
                sQuery.AppendLine(" CALL TPSMGR.PROC_CREATE_TQP_WAFER_SUM(:TESTAREA, :PRODUCT, :PROGRAM, :WAFER_SEQ, :GEC_BIN) ");


                Dictionary<string, Parameter> dicParam = new Dictionary<string, Parameter>();
                Parameter p = new Parameter();
                #region [ Setting Parameter ]
                p = new Parameter(); p.Type = DbType.String; p.Value = rv.TestArea; dicParam.Add("TESTAREA", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = rv.Device; dicParam.Add("PRODUCT", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = rv.Program; dicParam.Add("PROGRAM", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = rv.WaferSequence_Next; dicParam.Add("WAFER_SEQ", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = null; dicParam.Add("GEC_BIN", p);
                #endregion [ Setting Parameter ]

                iResult = base.ExecuteNonQuery(
                    sQuery.ToString(),
                    dicParam
                    );

            }



        }
        #endregion [ Raw Data Table ]

        //-----------------------------------------------------------------------------------

        #region [ TQP_PRODUCT ]

        public DataTable GetTQP_PRODUCT(
         string strFactory,
         string strDevice
         )
        {
            Dictionary<string, Parameter> dicParams = null;
            StringBuilder sQuery = new StringBuilder();

            //--

            sQuery.AppendLine(" SELECT * FROM TQP_PRODUCT WHERE 1= 1 AND  FACTORY = :FACILITY AND PRODUCT = :PRODUCT ");

            //--

            dicParams = new Dictionary<string, Parameter>();

            //--

            Parameter p = new Parameter();
            p.Type = DbType.String;
            p.Value = strFactory;
            dicParams.Add("FACILITY", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = strDevice;
            dicParams.Add("PRODUCT", p);

            //--

            DataTable dt =  base.ExecuteQuery(
                   sQuery.ToString(),
                   dicParams
                   );

            return dt;
        }

        public int SetTQP_PRODUCT(
            TwResultValue rv
            )
        {
            int iResult = -1;
            Dictionary<string, Parameter> dicParams = null;
            StringBuilder sQuery = new StringBuilder();

            sQuery.AppendLine(" INSERT INTO TQP_PRODUCT VALUES(:facility, :product, :customer_id, :custprod, :mapid, 'N') ");

            dicParams = new Dictionary<string, Parameter>();
            #region [ Settring Parameter ]

            Parameter p = new Parameter();
            p.Type = DbType.String;
            p.Value = rv.Factory;
            dicParams.Add("factory", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = rv.Device;
            dicParams.Add("product", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = "DB";
            dicParams.Add("customer_id", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = rv.Device;
            dicParams.Add("custprod", p);

            p = new Parameter();
            p.Type = DbType.String;
            p.Value = rv.Program;
            dicParams.Add("mapid", p);

            #endregion [ Setting Parameter ]


            iResult = ExecuteNonQuery(
                Constract.FUNCATION_NAME_LOT,
                sQuery.ToString(),
                dicParams
                );

            return iResult;
        }

        #endregion [ TQP_PRODUCT ]

        //-----------------------------------------------------------------------------------

        #region [ TQP_MAPDEF ]

        public bool ContainsMAPDEF(string deviceAlias)
        {
            DataTable dt = GetTQP_MAPDEF(deviceAlias);

            return (dt != null && dt.Rows.Count > 0);
        }

        public DataTable GetTQP_MAPDEF(
          string strProgram
          )
        {
            Dictionary<string, Parameter> dicParams = null;
            StringBuilder sQuery = new StringBuilder();

            //--

            sQuery.AppendLine(" SELECT * FROM TQP_MAPDEF WHERE MAPID = :MAPID ");

            //--

            dicParams = new Dictionary<string, Parameter>();

            //--

            Parameter p = new Parameter();
            p.Type = DbType.String;
            p.Value = strProgram;
            dicParams.Add("MAPID", p);

            //--

            DataTable dt = base.ExecuteQuery(
                   sQuery.ToString(),
                   dicParams
                   );

            return dt;
        }


        public int SetTQP_MAPDEF(string program, int flatZone, int xmax, int ymax, int xmin, int ymin, int netDie, TwResultValue rv )
        {
            return SetTQP_MAPDEF(program, flatZone, xmax, ymax, xmin, ymin, netDie, null);
        }

        public int SetTQP_MAPDEF(string deviceAlias, int flatZone, int xmax, int ymax, int xmin, int ymin, int netDie)
        {
            DataTable dt = null;

            decimal OriginX = 0;
            decimal OriginY = 0;

            double OriginIndexX = 0;
            double OriginIndexY = 0;

            double edge = 0.3d;
            double margin = 0.975d;
            double wsize = 200d; // Wafer Size 는 200mm

            int iResult = -1;

            try
            {

                //int xcnt = xmax - xmin + 1;
                //int ycnt = ymax - ymin + 1;

                int xcnt = xmax;
                int ycnt = ymax;

                //wmargin = 97.5 / 100d;

                double dDieSizeX = Math.Round(((wsize - edge * 2) * margin) / xcnt, 5);
                double dDieSizeY = Math.Round(((wsize - edge * 2) * margin) / ycnt, 5);


                OriginX = xmin + (int)(xcnt / 2) - ((xcnt + 1) % 2) + 1;
                OriginY = ymin + (int)(ycnt / 2) - ((ycnt + 1) % 2) + 1;

                OriginIndexX = (-dDieSizeX * 0.5 * (xcnt % 2));
                OriginIndexY = (-dDieSizeY * 0.5 * (ycnt % 2));
                
                Dictionary<string, Parameter> dicParams = null;
                StringBuilder sQuery = new StringBuilder();

                sQuery.AppendLine(" INSERT INTO TQP_MAPDEF  ");
                sQuery.AppendLine(" (MAPID  , ");
                sQuery.AppendLine("  WAFER_SIZE  , ");
                sQuery.AppendLine("  CHIP_SIZE_X , ");
                sQuery.AppendLine("  CHIP_SIZE_Y , ");
                sQuery.AppendLine("  ORIGIN_MICRO_X  , ");
                sQuery.AppendLine("  ORIGIN_MICRO_Y  , ");
                sQuery.AppendLine("  ORIGIN_INDEX_X  , ");
                sQuery.AppendLine("  ORIGIN_INDEX_Y  , ");
                sQuery.AppendLine("  FIRST_MICRO_X   , ");
                sQuery.AppendLine("  FIRST_MICRO_Y   , ");
                sQuery.AppendLine("  FIRST_INDEX_X   , ");
                sQuery.AppendLine("  FIRST_INDEX_Y   , ");
                sQuery.AppendLine("  EDGE_SIZE   , ");
                sQuery.AppendLine("  ANGLE   , ");
                sQuery.AppendLine("  NETDIE  , ");
                sQuery.AppendLine("  NOTCH_TYPE  , ");
                sQuery.AppendLine("  ST_START    , ");
                sQuery.AppendLine("  ST_INTYPE   , ");
                sQuery.AppendLine("  ST_XCNT , ");
                sQuery.AppendLine("  ST_YCNT , ");
                sQuery.AppendLine("  ST_START_X  , ");
                sQuery.AppendLine("  ST_START_Y  , ");
                sQuery.AppendLine("  DIE_INDEX_MIN_X , ");
                sQuery.AppendLine("  DIE_INDEX_MAX_X , ");
                sQuery.AppendLine("  DIE_INDEX_MIN_Y , ");
                sQuery.AppendLine("  DIE_INDEX_MAX_Y , ");
                sQuery.AppendLine("  XY_DIRECTION    , ");
                sQuery.AppendLine("  REFERENCEDIE_SETTING    , ");
                sQuery.AppendLine("  DELETE_FLAG  ");
                sQuery.AppendLine(" ) ");
                sQuery.AppendLine(" VALUES(:mapid ");
                sQuery.AppendLine(" ,:wafer_size ");
                sQuery.AppendLine(" ,:chip_size_x ");
                sQuery.AppendLine(" ,:chip_size_y ");
                sQuery.AppendLine(" ,:origin_micro_x ");
                sQuery.AppendLine(" ,:origin_micro_y ");
                sQuery.AppendLine(" ,:origin_index_x ");
                sQuery.AppendLine(" ,:origin_index_y ");
                sQuery.AppendLine(" ,:first_micro_x ");
                sQuery.AppendLine(" ,:first_micro_y ");
                sQuery.AppendLine(" ,:first_index_x ");
                sQuery.AppendLine(" ,:first_index_y ");
                sQuery.AppendLine(" ,:edge_size ");
                sQuery.AppendLine(" ,:angle ");
                sQuery.AppendLine(" ,:netdie ");
                sQuery.AppendLine(" ,:notch_type ");
                sQuery.AppendLine(" ,:st_start ");
                sQuery.AppendLine(" ,:st_intype ");
                sQuery.AppendLine(" ,:st_xcnt     ");
                sQuery.AppendLine(" ,:st_ycnt     ");
                sQuery.AppendLine(" ,:st_start_x ");
                sQuery.AppendLine(" ,:st_start_y ");
                sQuery.AppendLine(" ,:die_index_min_x ");
                sQuery.AppendLine(" ,:die_index_max_x ");
                sQuery.AppendLine(" ,:die_index_min_y ");
                sQuery.AppendLine(" ,:die_index_max_y ");
                sQuery.AppendLine(" ,:xy_direction ");
                sQuery.AppendLine(" ,:referance_setting ");
                sQuery.AppendLine(" , 'N') ");

                //--

                dicParams = new Dictionary<string, Parameter>();
                #region [ Setting Parameter ]
                Parameter p = new Parameter(); p.Type = DbType.String; p.Value = deviceAlias; dicParams.Add("mapid", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = wsize; dicParams.Add("wafer_size", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = dDieSizeX; dicParams.Add("chip_size_x", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = dDieSizeY; dicParams.Add("chip_size_y", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = OriginIndexX; dicParams.Add("origin_micro_x", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = OriginIndexY; dicParams.Add("origin_micro_y", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = OriginX; dicParams.Add("origin_index_x", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = OriginY; dicParams.Add("origin_index_y", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = dDieSizeX; dicParams.Add("first_micro_x", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = dDieSizeY; dicParams.Add("first_micro_y", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = 1; dicParams.Add("first_index_x", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = 1; dicParams.Add("first_index_y", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = edge; dicParams.Add("edge_size", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = 0; dicParams.Add("angle", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = netDie; dicParams.Add("netdie", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = 0; dicParams.Add("notch_type", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = 0; dicParams.Add("st_start", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = 0; dicParams.Add("st_intype", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = 0; dicParams.Add("st_xcnt", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = 0; dicParams.Add("st_ycnt", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = 0; dicParams.Add("st_start_x", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = 0; dicParams.Add("st_start_y", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = xmin; dicParams.Add("die_index_min_x", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = xmax; dicParams.Add("die_index_max_x", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = ymin; dicParams.Add("die_index_min_y", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = ymax; dicParams.Add("die_index_max_y", p);
                p = new Parameter(); p.Type = DbType.String; p.Value = 1; dicParams.Add("xy_direction", p); //LeftTop = 0, LeftBottom = 1, RightBottom = 2, RightTop = 3
                p = new Parameter(); p.Type = DbType.String; p.Value = 0; dicParams.Add("referance_setting", p);
               
                #endregion [ Setting Parameter ]


                iResult = base.ExecuteNonQuery(
                    sQuery.ToString(),
                    dicParams
                    );


                return iResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion [ TQP_MAPDEF Table  ]

        #region Bulk Inser & Execute

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

                    //do
                    //{
                    //    try
                    //    {
                    oConnection.Open();
                    comm.ExecuteNonQuery();
                    return;
                    //    }
                    //    catch (Oracle.ManagedDataAccess.Client.OracleException oex)
                    //    {
                    //        o = oex;
                    //        if (oex.Number == 904) //ex) "CK_POLY1": 부적합한 식별자
                    //        {
                    //            string tmp = comm.CommandText;
                    //            string colName = oex.Message.Substring(oex.Message.IndexOf('"') + 1, oex.Message.LastIndexOf('"') - oex.Message.IndexOf('"') - 1);
                    //            comm.CommandText = String.Format("ALTER TABLE {0} ADD {1} NUMBER", dt.TableName, colName);
                    //            comm.ExecuteNonQuery();
                    //            comm.CommandText = tmp;
                    //        }
                    //    }
                    //}
                    //while (tr++ < maxtr);

                    //if (tr >= maxtr)
                    //    throw o;
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
        }

        private bool ContainsTable(string tableName)
        {
            DataTable dt = ExecuteQuery(String.Format(
            @"
SELECT COUNT(*) FROM ALL_TABLES
WHERE TABLE_NAME = '{0}'
", tableName));

            return Int32.Parse(dt.Rows[0][0].ToString()) > 0;
        }

        public void DropTable(string tableName)
        {
            ExecuteNonQuery(String.Format("ALTER TABLE TPSMGR.{0} DROP PRIMARY KEY CASCADE", tableName));
            ExecuteNonQuery(String.Format("DROP TABLE TPSMGR.{0} CASCADE CONSTRAINTS", tableName));
        }

        public bool ContainsView(string viewName)
        {
            DataTable dt = ExecuteQuery(String.Format(
@"
SELECT COUNT(*) FROM ALL_VIEWS
WHERE VIEW_NAME = '{0}'
", viewName));

            return Int32.Parse(dt.Rows[0][0].ToString()) > 0;
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
            return (decimal)ExecuteScalar("SELECT WAFER_SEQ.NEXTVAL FROM DUAL");
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

        public DataTable GetDeviceAliasList()
        {
            return ExecuteQuery("SELECT DISTINCT DEVICE_ALIAS FROM TQP_LOT ORDER BY 1");
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

        private void AppendColumnAtTDTable(string tableName, List<string> columnList)
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
    'BIN'||BIN BIN, COUNT(*) CNT
FROM {0}
WHERE WAFER_SEQ = {1}
AND BIN < 100
GROUP BY BIN
ORDER BY BIN
", tableName, waferSeq));

            if (dt == null || dt.Rows.Count == 0)
                return null;

            Dictionary<string, decimal> dic = new Dictionary<string, decimal>();

            foreach (DataRow row in dt.Rows)
            {
                dic.Add((string)row[0], (decimal)row[1]);
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
AND PROBE_CNT = 0
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
    }
}
