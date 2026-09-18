using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DACrux.TEST.DSL;
using DACrux.TEST.Interface;
using System.Diagnostics;
using DACrux.Common.DSL;
using System.Text;

namespace DACrux.TEST.BSL
{
    public class TestCommon : Miracom.Middleware.BaseComponent, iTestCommon
    {
        public static string[] m_tbl = new string[] {
                                     "A","A","B","C","D","E","F","G","H","I","J"
                                    ,"K","L","M","N","O","P","Q","R","S","T"
                                    ,"U","V","W","X","Y","Z"};

        public void InsertDataMulti(
            DataTable dt
            )
        {
            TB_COMMON obj = new TB_COMMON();
            obj.InsertDataMulti(dt);
        }

        //--

        public void ExecuteProcedureMulti(
            DataTable dt
            )
        {
            TB_COMMON obj = new TB_COMMON();
            obj.ExecuteProcedureMulti(dt);
        }

        //--

        public DataTable GetStandardReport(
            DateTime dtStart,
            DateTime dtEnd,
            String factory,
            String testarea,
            String product,
            String program,
            String[] lot
            )
        {
            DataSet ds = null;
            DataTable dt = null;
            DataRow[] drs = null;

            TQP_WAFER oWafer = null;
            TQP_DATA_TABLES oTables = null;
            TQP_PARASPEC oParams = null;
            //T_PCM oPcm = null;
            TD_TABLE oTable = null;

            Dictionary<string, string> dicWafer = null;
            List<string> tmp = null;

            try
            {
#if DEBUG
                Stopwatch st = System.Diagnostics.Stopwatch.StartNew();
#endif
                ds = new DataSet();
                oWafer = new TQP_WAFER();
                oTables = new TQP_DATA_TABLES();
                oParams = new TQP_PARASPEC();
                oTable = new TD_TABLE();

                //--

                #region [ Step 1. Search TQP_WAFER ]
                dt = oWafer.GetData(
                    "SELECT_WAFER_INFO_02",
                    new string[] { String.Format("'{0}'", String.Join("','", lot)) },
                    new string[] { dtStart.ToString("yyyyMMddHHmmss"), dtEnd.ToString("yyyyMMddHHmmss"), testarea, product, program }
                    );
                dt.TableName = "WAFER_INFO";
                if (dt == null || dt.Rows.Count <= 0)
                    return dt;

                ds.Tables.Add(dt);
                #endregion [ Step 1. Search TQP_WAFER ]

                //--

                #region [ Step 2. Search TQP_PARASPEC ]
                dt = dt.DefaultView.ToTable(true, "PROGRAM", "PROGRAM_REV");
                dicWafer = new Dictionary<string, string>();
                tmp = new List<string>();
                foreach (DataRow r1 in dt.Rows)
                {
                    drs = ds.Tables["WAFER_INFO"].Select(String.Format("[PROGRAM] = '{0}' AND [PROGRAM_REV] = '{1}'", r1["PROGRAM"], r1["PROGRAM_REV"]));
                    foreach (DataRow r2 in drs)
                    {
                        tmp.Add(r2["WAFER_SEQ"].ToString());
                    }
                    dicWafer.Add(r1["PROGRAM_REV"].ToString(), String.Format("'{0}'", String.Join("','", tmp.ToArray())));
                }
                tmp = new List<string>(dicWafer.Keys);

                dt = oParams.GetData(
                    "SELECT_PARA_DATA",
                    new string[] { String.Format("'{0}'", String.Join("','", tmp.ToArray())) },
                    new string[] { factory, program }
                    );
                dt.TableName = "PARAM_SPEC";
                ds.Tables.Add(dt);
                #endregion [ Step 2. Search TQP_PARASPEC ]

                //--

                #region [ Step 3. Search Program Data Table ]
                dt = oTables.GetData("SELECT_TABLE_LIST", null, new string[] { program });
                dt.TableName = "DATA_TABLES";
                ds.Tables.Add(dt);
                #endregion [ Step 3. Search Program Data Table ]

                //--

                #region [ Step 4. Get RawData ]
                foreach (KeyValuePair<string, string> p in dicWafer)
                {
                    foreach (DataRow r in ds.Tables["DATA_TABLES"].Rows)
                    {
                        dt = oTable.GetTestRawData(
                            r["TABLE_NAME"].ToString(), p.Value
                            );
                        dt.TableName = String.Format("{0}|{1}", r["TABLE_NAME"], p.Key);
                        ds.Tables.Add(dt);
                    }
                }
                #endregion [ Step 4. Get RawData ]

                //--

                #region [ Step 5. Summary Data ]
                dt = CreateStandardRptDataTable();
                double min = double.NaN;
                double max = double.NaN;
                double avg = double.NaN;
                double median = double.NaN;
                double stddev = double.NaN;
                double q1 = double.NaN;
                double q3 = double.NaN;
                String tablekey = String.Empty;

                foreach (KeyValuePair<String, String> p in dicWafer)
                {
                    foreach (DataRow r1 in ds.Tables["DATA_TABLES"].Rows)
                    {
                        foreach (DataRow r2 in ds.Tables["PARAM_SPEC"].Select(String.Format("[TABLE_NAME] = '{0}'", r1["TABLE_NAME"])))
                        {
                            tablekey = String.Format("{0}|{1}", r2["TABLE_NAME"], p.Key);
                            if (String.Equals(r2["PARAM_NAME"].ToString(), "BIN")
                                || String.Equals(r2["PARAM_NAME"].ToString(), "X")
                                || String.Equals(r2["PARAM_NAME"].ToString(), "Y"))
                                continue;

                            DataRow row = dt.NewRow();
                            row["PROGRAM"] = program;
                            row["REV"] = p.Key;
                            row["PARAM_NAME"] = r2["PARAM_NAME"];
                            row["INDEX"] = r2["PARAM_INDEX"];
                            row["LSL"] = r2["LSL"];
                            row["TARGET"] = r2["TARGET"];
                            row["USL"] = r2["USL"];
                            row["LCL"] = r2["LCL"];
                            row["UCL"] = r2["UCL"];

                            drs = ds.Tables[tablekey].Select(String.Format("[{0}] IS NOT NULL", r2["PARAM_NAME"]));
                            if (drs.Length <= 0)
                                continue;

                            GetMedianFromDataRows(
                                drs,
                                r2["PARAM_NAME"] as String,
                                out min,
                                out max,
                                out avg,
                                out median,
                                out stddev,
                                out q1,
                                out q3
                                );

                            //--

                            row["COUNT"] = drs.Count();
                            row["MININUM"] = (double)min;
                            row["Q1"] = (double)q1;
                            row["AVERAGE"] = (double)avg;
                            row["MEDIAN"] = (double)median;
                            row["STDDEV"] = (double)stddev;
                            row["Q3"] = (double)q3;
                            row["MAXINUM"] = (double)max;

                            dt.Rows.Add(row);
                        }
                    }
                }

#if DEBUG
                Debug.WriteLine(String.Format("ElapsedMilliseconds: {0}", st.ElapsedMilliseconds));
                st.Stop();
#endif

                dt.AcceptChanges();
                dt.TableName = "SUMMARY";
                ds.Tables.Add(dt);

                dt = ds.Tables["SUMMARY"];
                #endregion [ Step 5. Summary Data ]
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();

                if (oWafer != null)
                    oWafer.Dispose();

                if (oTables != null)
                    oTables.Dispose();

                if (oParams != null)
                    oParams.Dispose();

                if (oTable != null)
                    oTable.Dispose();

                if (dicWafer != null)
                    dicWafer = null;

                if (tmp != null)
                    tmp = null;
            }

            return dt;
        }

        //--

        public byte[] GetTestReportStatistics_Comp(
            string fromDate, 
            string toDate, 
            string factory,
            string[] testarea, 
            string product, 
            string program,
            string[] lotids, 
            string pgmParam
            )
        {
            return DACrux.Base.Util.ObjectToCompressedBytes(
                GetTestReportStatistics(fromDate, toDate, factory, testarea, product, program, lotids, pgmParam)
                );
        }

        public DataTable GetTestReportStatistics(
            string fromDate,
            string toDate,
            string factory,
            string[] testarea,
            string product,
            string program,
            string[] lotids, 
            string pgmParam
            )
        {
            DataTable dtWafer = null;
            DataTable dt = null;

            //--

            TQP_WAFER oWafer = null;
            TQP_WAFER_SUM oWaferSum = null;
            TQP_PARASPEC oPgmParam = null;
            TD_TABLE oTdTable = null;

            //--

            List<String> lstTmp = null;
            String tablename = String.Empty;

            //--

            oWafer = new TQP_WAFER();
            oWaferSum = new TQP_WAFER_SUM();
            oPgmParam = new TQP_PARASPEC();
            oTdTable = new TD_TABLE();

            //--

            #region [ Step 1. Search Wafer Information ]
            dtWafer = oWafer.GetWaferInfo08(testarea, product, program, lotids);
            dtWafer.TableName = "WAFER_INFO";
            if (dtWafer == null || dtWafer.Rows.Count <= 0)
                return null;

            #endregion [ Step 1. Search Wafer Information ]

            //--

            #region [ Step 2. Search Parameter Information and RawData Table name ]
            if (lstTmp == null)
                lstTmp = new List<String>();
            dt = dtWafer.DefaultView.ToTable(true, "PROGRAM", "PROGRAM_REV");
            foreach (DataRow r in dt.Rows)
            {
                lstTmp.Add(r["PROGRAM_REV"].ToString());
            }

            dt = oPgmParam.GetProgramParamsData(factory, program, lstTmp.ToArray());
            dt.TableName = "PARAM_SPEC";
            DataRow[] drParam = dt.Select(String.Format("[PARAM_NAME] ='{0}'", pgmParam));
            if(drParam == null || drParam.Length <= 0)
                return null;

            tablename = drParam[0]["TABLE_NAME"].ToString();
            #endregion [ Step 2. Search Parameter Information and RawData Table name ]

            #region [ Step 3. Statistics Data ]
            lstTmp = new List<string>();
            lstTmp.Clear();
            foreach (DataRow dr in dtWafer.Rows)
            {
                lstTmp.Add(dr["WAFER_SEQ"].ToString());
            }
            dt = oTdTable.GetTestStatistics(factory, lotids, tablename, pgmParam, lstTmp.ToArray());
            #endregion [ Step 3. Statistics Data ]

            return dt;
        }

        public byte[] GetTestReportRawData_Comp(
            string fromDate,
            string toDate,
            string factory,
            string[] testarea,
            string product,
            string program,
            string[] lotids,
            string pgmParam
            )
        {
            return DACrux.Base.Util.ObjectToCompressedBytes(
                GetTestReportRawData(fromDate, toDate, factory, testarea, product, program, lotids, pgmParam)
                );
        }

        public DataTable GetTestReportRawData(
            string fromDate,
            string toDate,
            string factory,
            string[] testarea,
            string product,
            string program,
            string[] lotids,
            string pgmParam
            )
        {
            DataTable dtWafer = null;
            DataTable dt = null;

            //--

            TQP_WAFER oWafer = null;
            TQP_WAFER_SUM oWaferSum = null;
            TQP_PARASPEC oPgmParam = null;
            TD_TABLE oTdTable = null;

            //--

            List<String> lstTmp = null;
            String[] selectCondition = null;
            String[] tablename = null;
            String[] joinCondition = null;

            //--

            oWafer = new TQP_WAFER();
            oWaferSum = new TQP_WAFER_SUM();
            oPgmParam = new TQP_PARASPEC();
            oTdTable = new TD_TABLE();

            //--

            #region [ Step 1. Search Wafer Information ]
            dtWafer = oWafer.GetWaferInfo08(testarea, product, program, lotids);
            dtWafer.TableName = "WAFER_INFO";
            if (dtWafer == null || dtWafer.Rows.Count <= 0)
                return null;

            #endregion [ Step 1. Search Wafer Information ]

            //--

            #region [ Step 2. Search Parameter Information and RawData Table name ]
            if (lstTmp == null)
                lstTmp = new List<String>();
            dt = dtWafer.DefaultView.ToTable(true, "PROGRAM", "PROGRAM_REV");
            foreach (DataRow r in dt.Rows)
            {
                lstTmp.Add(r["PROGRAM_REV"].ToString());
            }

            dt = oPgmParam.GetProgramParamsData(factory, program, lstTmp.ToArray());
            DataTable dtTmp = dt.DefaultView.ToTable(true, "TABLE_NAME");
            selectCondition = new String[dtTmp.Rows.Count];
            tablename = new String[dtTmp.Rows.Count];
            joinCondition = new String[dtTmp.Rows.Count];
            for (int rowIdx = 0; rowIdx < dtTmp.Rows.Count; rowIdx++)
            {
                selectCondition[rowIdx] = String.Format(" A{0}.*", rowIdx);
                tablename[rowIdx] = String.Format("{0} A{1}", dtTmp.Rows[rowIdx]["TABLE_NAME"].ToString(), rowIdx);
                joinCondition[rowIdx] = String.Format("T2.WAFER_SEQ = A{0}.WAFER_SEQ", rowIdx);
            }
            #endregion [ Step 2. Search Parameter Information and RawData Table name ]

            #region [ Step 3. Statistics Data ]
            lstTmp = new List<string>();
            lstTmp.Clear();
            foreach (DataRow dr in dtWafer.Rows)
            {
                lstTmp.Add(dr["WAFER_SEQ"].ToString());
            }

            dt = oTdTable.GetTestRawData(factory, selectCondition, tablename, joinCondition, lotids, lstTmp.ToArray());
            #endregion [ Step 3. Statistics Data ]

            return dt;
        }


        //--

        public byte[] GetPCMReport_Comp(
            DateTime dtStart,
            DateTime dtEnd,
            String factory,
            String[] testarea,
            String product,
            String program,
            String parameter,
            String[] lotids
            )
        {
            return DACrux.Base.Util.ObjectToCompressedBytes(
                GetPCMReport(dtStart, dtEnd, factory, testarea, product, program, parameter, lotids)
                );
        }

        public DataSet GetPCMReport(
            DateTime dtStart,
            DateTime dtEnd,
            String factory,
            String[] testarea,
            String product,
            String program,
            String parameter,
            String[] lotids
            )
        {
            DataSet ds = null;
            DataTable dt = null;

            //--

            TQP_WAFER oWafer = null;
            TQP_WAFER_SUM oWaferSum = null;
            TQP_PARASPEC oPgmParam = null;
            TD_TABLE oTdTable = null;

            //--

            List<String> lstTmp = null;
            String tablename = String.Empty;

            //--

            ds = new DataSet();
            oWafer = new TQP_WAFER();
            oWaferSum = new TQP_WAFER_SUM();
            oPgmParam = new TQP_PARASPEC();
            oTdTable = new TD_TABLE();

            //--

            #region [ Step 1. Search Wafer Information ]
            dt = oWafer.GetWaferInfo08(testarea, product, program, lotids);
            dt.TableName = "WAFER_INFO";
            if (dt == null || dt.Rows.Count <= 0)
                return null;

            ds.Tables.Add(dt);
            #endregion [ Step 1. Search Wafer Information ]

            //--

            #region [ Step 2. Search Parameter Information ]
            if (lstTmp == null)
                lstTmp = new List<String>();
            dt = dt.DefaultView.ToTable(true, "PROGRAM", "PROGRAM_REV");
            foreach (DataRow r in dt.Rows)
            {
                lstTmp.Add(r["PROGRAM_REV"].ToString());
            }

            dt = oPgmParam.GetProgramParamsData(factory, program, lstTmp.ToArray());
            dt.TableName = "PARAM_SPEC";
            ds.Tables.Add(dt);
            #endregion [ Step 2. Search Parameter Information  ]

            //--

            #region [ Step 3. Get PCM Data ]
            lstTmp.Clear();
            foreach (DataRow r in ds.Tables["WAFER_INFO"].Rows)
            {
                lstTmp.Add(r["WAFER_SEQ"].ToString());
            }

            dt = oTdTable.GetPCMData(
                factory,
                lotids,
                lstTmp.ToArray() // Wafer Seq
                );
            dt.TableName = "RAW_DATA";
            ds.Tables.Add(dt);
            #endregion [ Step 3. Get Pcm Data ]

            return ds;
        }

        //--

        #region [ Standard Report Sub Method ]

        private DataTable CreateStandardRptDataTable(
            )
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] { 
                new DataColumn("PROGRAM", typeof(string)), 
                new DataColumn("REV", typeof(int)), 
                new DataColumn("PARAM_NAME", typeof(string)), 
                new DataColumn("INDEX", typeof(int)), 
                new DataColumn("BOX_PLOT", typeof(string)), 
                new DataColumn("LSL", typeof(double)), 
                new DataColumn("TARGET", typeof(double)), 
                new DataColumn("USL", typeof(double)), 
                new DataColumn("LCL", typeof(double)), 
                new DataColumn("UCL", typeof(double)), 
                new DataColumn("COUNT", typeof(int)), 
                new DataColumn("MININUM", typeof(double)), 
                new DataColumn("Q1", typeof(double)), 
                new DataColumn("MEDIAN", typeof(double)), 
                new DataColumn("Q3", typeof(double)), 
                new DataColumn("MAXINUM", typeof(double)),
                new DataColumn("AVERAGE", typeof(double)),
                new DataColumn("STDDEV", typeof(double)) 
            });
            return dt;
        }

        private void GetMedianFromDataRows(
            DataRow[] rows,
            String columnName,
            out double min,
            out double max,
            out double avg,
            out double median,
            out double stddev,
            out double q1,
            out double q3
            )
        {
            double[] values = Array.ConvertAll<DataRow, double>(
                rows,
                delegate(DataRow r)
                {
                    return double.Parse(r[columnName].ToString());
                });

            min = values.Min() < double.MinValue ? double.MinValue : values.Min();
            max = values.Max() > double.MaxValue ? double.MaxValue : values.Max();
            avg = values.Average();
            median = GetMedianFromArray(values);
            stddev = GetStddevFromArray(values, values.Count() - 1);
            q1 = percentile(values, 25);
            q3 = percentile(values, 75);
        }

        private double GetMedianFromArray(
            double[] values
            )
        {
            double median = double.NaN;
            Array.Sort(values);
            if (values.Length % 2 != 0)
            {
                median = values[values.Length / 2];
            }
            else
            {
                int middle = values.Length / 2;
                double first = values[middle];
                double second = values[middle - 1];
                median = (first + second) / 2;
            }

            return median;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="divisor">divisor 의 값에 의해서 Population 과 Sample로 나뉨</param>
        /// Population: values.count() 
        /// Sample: values.count() - 1
        /// <returns></returns>
        private double GetStddevFromArray(
            double[] values,
            double divisor
            )
        {
            double avg = values.Average();
            return Math.Sqrt(values.Sum(x => Math.Pow(x - avg, 2)) / divisor);
        }

        private double percentile(
            double[] values,
            double p
            )
        {
            if (p >= 100.0d) return values[values.Length - 1];

            double position = (values.Length + 1) * p / 100.0;
            double leftnumber = 0.0d, rightnumber = 0.0d;

            double n = p / 100.0d * (values.Length - 1) + 1.0d;

            if (position >= 1)
            {
                leftnumber = values[(int)Math.Floor(n) - 1];
                rightnumber = values[(int)Math.Floor(n)];
            }
            else
            {
                leftnumber = values[0];
                rightnumber = values[1];
            }

            if (Equals(leftnumber, rightnumber))
                return leftnumber;
            double part = n - Math.Floor(n);
            return leftnumber + part * (rightnumber - leftnumber);
        }

        #endregion [ Standard Report Sub Mehtod ]

        #region [ Pcm Report Sub method ]
        private DataTable CreatePcmRptDataTable(
            )
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(
                new DataColumn[] {
                    new DataColumn("WAFER_SEQ", typeof(decimal)),
                    new DataColumn("WAFER_ID", typeof(String)),
                    new DataColumn("YIELD", typeof(decimal)),
                    new DataColumn("LSL", typeof(decimal)),
                    new DataColumn("TARGET", typeof(decimal)),
                    new DataColumn("USL", typeof(decimal)),
                    new DataColumn("LCL", typeof(decimal)),
                    new DataColumn("UCL", typeof(decimal)),
                    new DataColumn("COUNT", typeof(decimal)),
                    new DataColumn("MININUM", typeof(decimal)),
                    new DataColumn("Q1", typeof(decimal)),
                    new DataColumn("MEDIAN", typeof(decimal)),
                    new DataColumn("Q3", typeof(decimal)),
                    new DataColumn("MAXINUM", typeof(decimal)),
                    new DataColumn("AVERAGE", typeof(decimal)),
                    new DataColumn("STDDEV", typeof(decimal))
                });

            return dt;
        }
        #endregion [ Pcm Report Sub method ]

        /// <summary>
        /// TQP_PRODUCT 데이터가 존재하지 않는 경우 INSERT 합니다.
        /// </summary>
        public void InsertProductInfo(string factory, string product, string mapid)
        {
            TQP_PRODUCT obj = new TQP_PRODUCT();
            obj.MergeData(factory, product, mapid);
        }

        /// <summary>
        /// TQP_PROGRAM 데이터가 존재하지 않는 경우 INSERT 합니다.
        /// </summary>
        public void InsertProgramInfo(string factory, string program, string testarea, string device)
        {
            TQP_PROGRAM obj = new TQP_PROGRAM();
            obj.MergeData(factory, program, testarea, device);
        }

        /// <summary>
        /// WaferSeq를 가져옵니다.
        /// </summary>
        public string GetNewWaferSeq()
        {
            TQP_WAFER obj = new TQP_WAFER();
            return obj.SelectNextWaferSeq();
        }

        public void GetCorrectionData(string testArea, string deviceAlias, string program, out string Dir, out int rotate, out int shiftX, out int shiftY, out int ConfigSeq, bool bPST)
        {
            rotate = shiftX = shiftY = ConfigSeq = 0;
            Dir = "";

            TQP_MAPCFG obj = new TQP_MAPCFG();
            DataTable dt = obj.GetCorrectionData(testArea, deviceAlias, program);

            if (dt == null || dt.Rows.Count == 0)
            {
                int iChar = 0;

                //Program 첫글자의 숫자 여부에 따라 Default shift
                //bOrigin 은 Setup 정보가 없을 경우 그래도 적용할지에 대한 여부
                if (bPST == true)
                {
                    if (int.TryParse(program[0].ToString(), out iChar) == true)
                    {
                        Dir = "TL";
                        shiftX = 1;
                    }
                    else
                    {
                        shiftX = 1;
                        shiftY = 1;
                    }
                }

            }
            else
            {
                Dir = dt.Rows[0]["ALTER_DIRECTION"].ToString();
                Int32.TryParse(dt.Rows[0]["ALTER_ANGLE"].ToString(), out rotate);
                Int32.TryParse(dt.Rows[0]["ALTER_INDEX_X"].ToString(), out shiftX);
                Int32.TryParse(dt.Rows[0]["ALTER_INDEX_Y"].ToString(), out shiftY);
                Int32.TryParse(dt.Rows[0]["MAP_CFG_SEQ"].ToString(), out ConfigSeq);
            }
        }

        public int GetMaxProgramRev(string factory, string program)
        {
            TQP_PARASPEC obj = new TQP_PARASPEC();
            return obj.GetMaxProgramRev(factory, program);
        }


        /// <summary>
        /// TEST Wafer 데이터를 저장합니다.
        /// </summary>
        public void SaveWaferData(
            string FACTORY,
            string TEST_AREA,
            string DEVICE,
            string DEVICE_ALIAS,
            string LOT_ID,
            string WAFER_ID,
            string START_TIME,
            string END_TIME,
            int WAFER_DIAM,
            string TESTER,
            string PROBER,
            string PROBE_CARD,
            string OPERATOR,
            string PROGRAM,
            int PROGRAM_REV,
            int START_QTY,
            string VENDOR_ID,
            int MAX_X,
            int MAX_Y,
            string WAF_FLAT,
            string COMMENTS,
            string TEMP,
            string PGMPATH,
            string DATA_SOURCE,
            string TFVERSION,
            string PROBECARD_TOUCHDOWNS,
            string SMSFAMILY_NAME,
            int NUM_DIE,
            out string lotSeq,
            out string waferSeq
            )
        {
            SaveWaferData(
                FACTORY,
                TEST_AREA,
                DEVICE,
                DEVICE_ALIAS,
                LOT_ID,
                WAFER_ID,
                START_TIME,
                END_TIME,
                WAFER_DIAM,
                TESTER,
                PROBER,
                PROBE_CARD,
                OPERATOR,
                PROGRAM,
                PROGRAM_REV,
                START_QTY,
                VENDOR_ID,
                MAX_X,
                MAX_Y,
                WAF_FLAT,
                COMMENTS,
                null, // Over Drive
                TEMP,
                PGMPATH,
                DATA_SOURCE,
                null, // max site
                TFVERSION,
                PROBECARD_TOUCHDOWNS,
                SMSFAMILY_NAME,
                NUM_DIE,
                true,
                out lotSeq,
                out waferSeq
                );
        }

        /// <summary>
        /// TEST Wafer 데이터를 저장합니다.
        /// </summary>
        public void SaveWaferData(
            string FACTORY,
            string TEST_AREA,
            string DEVICE,
            string DEVICE_ALIAS,
            string LOT_ID,
            string WAFER_ID,
            string START_TIME,
            string END_TIME,
            int WAFER_DIAM,
            string TESTER,
            string PROBER,
            string PROBE_CARD,
            string OPERATOR,
            string PROGRAM,
            int PROGRAM_REV,
            int START_QTY,
            string VENDOR_ID,
            int MAX_X,
            int MAX_Y,
            string WAF_FLAT,
            string COMMENTS,
            string OVERDRIVE,
            string TEMP,
            string PGMPATH,
            string DATA_SOURCE,
            string MAXSITE,
            string TFVERSION,
            string PROBECARD_TOUCHDOWNS,
            string SMSFAMILY_NAME,
            int NUM_DIE,
            bool bIncreaseProbeCnt,
            out string lotSeq,
            out string waferSeq
            )
        {
            // LOT SEQ
            TQP_LOT lot = new TQP_LOT();
            TQP_WAFER waf = new TQP_WAFER();
            TQP_FOI_DIE foi = new TQP_FOI_DIE();

            //if (String.Equals(TEST_AREA, "AVI"))
            //{
            //    //AVI Map 의 경우 기존 Lot 및 Wafer 에 대한 정보를 삭제 하고 다시 만든다.
            //    //Retest 에 대한 개념보다는 잘못 올린 File 에 대한 재 Upload 개념
            //    //기존 Lot 및 Wafer 에 대한 정보를 가지고 온다.
            //    lotSeq = lot.GetLotSeq(FACTORY, TEST_AREA, LOT_ID);

            //    if (!String.IsNullOrEmpty(lotSeq))
            //    {
            //        lot.DeleteLot(lotSeq);
            //        waf.DeleteWaferDataAVI(FACTORY, lotSeq, WAFER_ID);
            //    }
            //}
            lotSeq = lot.GetLotSeq(FACTORY, TEST_AREA, LOT_ID, PROGRAM);

            // LOT 데이터가 없는 경우 TQP_LOT 추가
            if (String.IsNullOrEmpty(lotSeq))
                lotSeq = lot.GetNewLotSeq();

            // LOT 데이터 INSERT OR UPDATE
            lot.MergeData(
                FACTORY,
                lotSeq,
                LOT_ID,
                TEST_AREA,
                PROGRAM,
                DEVICE,
                DEVICE_ALIAS,
                START_TIME,
                END_TIME,
                WAFER_DIAM.ToString());

            // PROBE_CNT
            DataTable wafDt = null;

            /// FAB2 PCM 의 경우 Probe Cnt 를 증가 시키지 않기 때문에 기존에 등록된 이력으로 진행한다.
            /// FACTORY/LOT_SEQ/WAFER_ID/START_TIME 동일한것이 없는 경우에는 신규로 Wafer Seq 생성
            if (!bIncreaseProbeCnt)
                wafDt = waf.GetWaferInfo(FACTORY, lotSeq, WAFER_ID);
            else
                wafDt = waf.GetWaferInfo(FACTORY, lotSeq, WAFER_ID, START_TIME);

            bool existsWaferData = wafDt != null && wafDt.Rows.Count > 0;
            int probeCnt;

            if (existsWaferData) // WAFER 관련 데이터가 있는 경우
            {
                waferSeq = wafDt.Rows[0]["WAFER_SEQ"].ToString();
                probeCnt = Int32.Parse(wafDt.Rows[0]["PROBE_CNT"].ToString());

                //Scope 의 경우 FOI 상의 Map 정보를 삭제 한다.
                //if ((String.Equals(TEST_AREA, "AVI") && String.Equals(PROGRAM, "SCOPE")))
                //{
                //    foi.DeleteFOIDiesMulti(waferSeq);
                //}
            }
            else // WAF 관련 데이터가 없는 경우
            {
                waferSeq = waf.SelectNextWaferSeq();
                probeCnt = -1;
            }

            // WAFER 데이터 INSERT OR UPDATE
            waf.MergeData(
                FACTORY,
                waferSeq,
                probeCnt.ToString(),
                lotSeq,
                WAFER_ID,
                START_TIME,
                END_TIME,
                TESTER,
                PROBER,
                PROBE_CARD,
                OPERATOR,
                PROGRAM,
                PROGRAM_REV.ToString(),
                START_QTY.ToString(),
                VENDOR_ID,
                MAX_X.ToString(),
                MAX_Y.ToString(),
                WAF_FLAT,
                COMMENTS,
                LOT_ID,
                TEMP,
                OVERDRIVE,
                PGMPATH,
                DATA_SOURCE,
                MAXSITE,
                TFVERSION,
                PROBECARD_TOUCHDOWNS,
                SMSFAMILY_NAME,
                NUM_DIE.ToString());

            if (!existsWaferData)
            {
                // INSERT 인 경우 현재 데이터가 최신이 되도록 (PROBE_CNT=0) PROBE_CNT 값을 1씩 증가시킨다.
                waf.IncreaseProbeCnt(FACTORY, lotSeq, WAFER_ID);
            }
        }

        /// <summary>
        /// Rework 개념없이 Test 는 중복제거
        /// </summary>
        //public void SaveWaferDataNotRework(
        //    string FACTORY,
        //    string TEST_AREA,
        //    string DEVICE,
        //    string DEVICE_ALIAS,
        //    string LOT_ID,
        //    string WAFER_ID,
        //    string START_TIME,
        //    string END_TIME,
        //    int WAFER_DIAM,
        //    string TESTER,
        //    string PROBER,
        //    string PROBE_CARD,
        //    string OPERATOR,
        //    string PROGRAM,
        //    int PROGRAM_REV,
        //    int START_QTY,
        //    string VENDOR_ID,
        //    int MAX_X,
        //    int MAX_Y,
        //    string WAF_FLAT,
        //    string COMMENTS,
        //    string PGMPATH,
        //    string DATA_SOURCE,
        //    string TFVERSION,
        //    string PROBECARD_TOUCHDOWNS,
        //    string SMSFAMILY_NAME,
        //    int NUM_DIE,
        //    out string lotSeq,
        //    out string waferSeq
        //    )
        //{
        //    int probeCnt;
        //    DataTable wafDt;

        //    // LOT SEQ
        //    TQP_LOT lot = new TQP_LOT();
        //    TQP_WAFER waf = new TQP_WAFER();
        //    TQP_LOT_SUM lot_sum = new TQP_LOT_SUM();
        //    TQP_WAFER_SUM waf_sum = new TQP_WAFER_SUM();
        //    TQP_FOI_DIE foi = new TQP_FOI_DIE();

        //    //Lot 정보는 없으면 생성 한다.
        //    lotSeq = lot.GetAVILotSeq(FACTORY, TEST_AREA, LOT_ID, PROGRAM);
        //    if (String.IsNullOrEmpty(lotSeq))
        //    {
        //        lotSeq = lot.GetNewLotSeq();
        //        // LOT 데이터 INSERT OR UPDATE
        //        lot.MergeData(
        //            FACTORY,
        //            lotSeq,
        //            LOT_ID,
        //            TEST_AREA,
        //            PROGRAM,
        //            DEVICE,
        //            DEVICE_ALIAS,
        //            START_TIME,
        //            END_TIME,
        //            WAFER_DIAM.ToString());
        //    }

        //    //Wafer 관련 Data 중복 Data 는 삭제 한다.
        //    //Wafer 는 재 Upload 시 Retest 로 간주하지 않는다.
        //    wafDt = waf.GetWaferInfo(FACTORY, lotSeq, WAFER_ID);
        //    if (wafDt != null && wafDt.Rows.Count > 0)
        //    {
        //        waferSeq = wafDt.Rows[0]["WAFER_SEQ"].ToString();
        //        foi.DeleteFOIDiesMulti(waferSeq);
        //        waf.DeleteWaferData(waferSeq);
        //        waf_sum.DeleteWaferData(waferSeq);
        //    }

        //    waferSeq = waf.SelectNextWaferSeq();
        //    probeCnt = -1;

        //    // WAFER 데이터 INSERT OR UPDATE
        //    waf.MergeData(
        //        FACTORY,
        //        waferSeq,
        //        probeCnt.ToString(),
        //        lotSeq,
        //        WAFER_ID,
        //        START_TIME,
        //        END_TIME,
        //        TESTER,
        //        PROBER,
        //        PROBE_CARD,
        //        OPERATOR,
        //        PROGRAM,
        //        PROGRAM_REV.ToString(),
        //        START_QTY.ToString(),
        //        VENDOR_ID,
        //        MAX_X.ToString(),
        //        MAX_Y.ToString(),
        //        WAF_FLAT,
        //        COMMENTS,
        //        LOT_ID,
        //        PGMPATH,
        //        DATA_SOURCE,
        //        TFVERSION,
        //        PROBECARD_TOUCHDOWNS,
        //        SMSFAMILY_NAME,
        //        NUM_DIE.ToString());


        //    // INSERT 인 경우 현재 데이터가 최신이 되도록 (PROBE_CNT=0) PROBE_CNT 값을 1씩 증가시킨다.
        //    waf.IncreaseProbeCnt(FACTORY, lotSeq, WAFER_ID);

        //}

        public DataTable GetLotInfo(string lotid)
        {
            TQC_LOT_STS obj = new TQC_LOT_STS();
            return obj.GetLotInfo(
                lotid
                );
        }

        /// <summary>
        /// Lot ID 7자리로 확인 후 없으면 6자리로 확인 한다.
        /// </summary>
        /// <param name="lotid"></param>
        /// <returns></returns>
        public string GetLotStatusInfo(string LOT_ID)
        {
            TQC_LOT_STS oTQC_LOT_STS = new TQC_LOT_STS();

            DataTable dtTemp = null;
            DataTable dtTempFilter = null;
            string strLotID = string.Empty;
            string strDevice = string.Empty;

            //Lot 이 6자리 이하는 나올 수 없다.
            if (LOT_ID.Length < 6)
                return "NONE";

            //6자리로 먼저 Data Get
            strLotID = string.Format("{0}%", LOT_ID.Substring(0, 6));
            dtTemp = oTQC_LOT_STS.GetLotInfoLength(strLotID);
            if (dtTemp != null && dtTemp.Rows.Count > 0)
            {
                //원본 ID 로 Filter -> 7자리 -> 6자리
                if (dtTemp.Select(string.Format("LOT_NO = '{0}'", LOT_ID)).Length > 0)
                    dtTempFilter = dtTemp.Select(string.Format("LOT_NO = '{0}'", LOT_ID)).CopyToDataTable<DataRow>();
                else if (LOT_ID.Length >= 7 && dtTemp.Select(string.Format("LOT_NO = '{0}'", LOT_ID.Substring(0, 7))).Length > 0)
                    dtTempFilter = dtTemp.Select(string.Format("LOT_NO = '{0}'", LOT_ID.Substring(0, 7))).CopyToDataTable<DataRow>();
                else if (LOT_ID.Length >= 6 && dtTemp.Select(string.Format("LOT_NO = '{0}'", LOT_ID.Substring(0, 6))).Length > 0)
                    dtTempFilter = dtTemp.Select(string.Format("LOT_NO = '{0}'", LOT_ID.Substring(0, 6))).CopyToDataTable<DataRow>();
                else
                    dtTempFilter = dtTemp.Copy();

                strDevice = dtTempFilter.Rows[0]["MASK_ID"].ToString();
            }
            else
                strDevice = "NONE";

            return strDevice;
        }

        public string GetProductInfo(string LOT_ID)
        {

            TQC_LOT_STS oTQC_LOT_STS = new TQC_LOT_STS();

            DataTable dtTemp = null;
            DataTable dtTempFilter = null;
            string strLotID = string.Empty;
            string strDevice = string.Empty;

            //Lot 이 6자리 이하는 나올 수 없다.
            if (LOT_ID.Length < 6)
                return null;

            //6자리로 먼저 Data Get
            strLotID = string.Format("{0}%", LOT_ID.Substring(0, 6));
            dtTemp = oTQC_LOT_STS.GetLotInfoLength(strLotID);
            if (dtTemp != null && dtTemp.Rows.Count > 0)
            {
                //원본 ID 로 Filter -> 7자리 -> 6자리
                if (dtTemp.Select(string.Format("LOT_NO = '{0}'", LOT_ID)).Length > 0)
                    dtTempFilter = dtTemp.Select(string.Format("LOT_NO = '{0}'", LOT_ID)).CopyToDataTable<DataRow>();
                else if (LOT_ID.Length >= 7 && dtTemp.Select(string.Format("LOT_NO = '{0}'", LOT_ID.Substring(0, 7))).Length > 0)
                    dtTempFilter = dtTemp.Select(string.Format("LOT_NO = '{0}'", LOT_ID.Substring(0, 7))).CopyToDataTable<DataRow>();
                else if (LOT_ID.Length >= 6 && dtTemp.Select(string.Format("LOT_NO = '{0}'", LOT_ID.Substring(0, 6))).Length > 0)
                    dtTempFilter = dtTemp.Select(string.Format("LOT_NO = '{0}'", LOT_ID.Substring(0, 6))).CopyToDataTable<DataRow>();
                else
                    dtTempFilter = dtTemp.Copy();

                strDevice = dtTempFilter.Rows[0]["MAT_ID"].ToString();
            }
            else
                strDevice = "NONE";

            return strDevice;
        }

        public DataTable GetLotStatusInfoToTable(string LOT_ID)
        {
            TQC_LOT_STS oTQC_LOT_STS = new TQC_LOT_STS();

            DataTable dtTemp = null;
            DataTable dtTempFilter = null;
            string strLotID = string.Empty;
            string strDevice = string.Empty;

            //Lot 이 6자리 이하는 나올 수 없다.
            if (LOT_ID.Length < 6)
                return null;

            //6자리로 먼저 Data Get
            strLotID = string.Format("{0}%", LOT_ID.Substring(0, 6));
            dtTemp = oTQC_LOT_STS.GetLotInfoLength(strLotID);
            if (dtTemp != null && dtTemp.Rows.Count > 0)
            {
                //원본 ID 로 Filter -> 7자리 -> 6자리
                if (dtTemp.Select(string.Format("LOT_NO = '{0}'", LOT_ID)).Length > 0)
                    dtTempFilter = dtTemp.Select(string.Format("LOT_NO = '{0}'", LOT_ID)).CopyToDataTable<DataRow>();
                else if (LOT_ID.Length >= 7 && dtTemp.Select(string.Format("LOT_NO = '{0}'", LOT_ID.Substring(0, 7))).Length > 0)
                    dtTempFilter = dtTemp.Select(string.Format("LOT_NO = '{0}'", LOT_ID.Substring(0, 7))).CopyToDataTable<DataRow>();
                else if (LOT_ID.Length >= 6 && dtTemp.Select(string.Format("LOT_NO = '{0}'", LOT_ID.Substring(0, 6))).Length > 0)
                    dtTempFilter = dtTemp.Select(string.Format("LOT_NO = '{0}'", LOT_ID.Substring(0, 6))).CopyToDataTable<DataRow>();
                else
                    dtTempFilter = dtTemp.Copy();
            }

            return dtTempFilter;
        }

        public void CreateFOIDiesMulti(string[,] Paras)
        {
            TQP_FOI_DIE obj = new TQP_FOI_DIE();
            obj.CreateFOIDiesMulti(Paras);
        }
        public void DeleteFOIDiesMulti(string strWaferSeq)
        {
            TQP_FOI_DIE obj = new TQP_FOI_DIE();
            obj.DeleteFOIDiesMulti(strWaferSeq);
        }

        public DataTable SelectScopeRawData(string strLotID, string strWaferID)
        {
            TQP_FOI_DIE obj = new TQP_FOI_DIE();
            return obj.SelectScopeRawData(strLotID, strWaferID);
        }

        public DataTable SelectFOIBinList(string strWaferSeq)
        {
            TQP_FOI_DIE obj = new TQP_FOI_DIE();
            return obj.SelectFOIBinList(strWaferSeq);
        }

        public void SetBinDESC_BinInfo(string strProgram, string strBin, string strCharBin, string strDESC)
        {
            TQP_BINDESC obj = new TQP_BINDESC();
            DataTable dt = obj.SelectProgramBin(strProgram, strBin);
            if (dt == null || dt.Rows.Count <= 0)
                obj.SetBinDESC(strProgram, strBin, strCharBin, strDESC);
        }

        public void SetAVIWaferSum(string strWaferSeq)
        {
            TQP_WAFER_SUM obj = new TQP_WAFER_SUM();
            obj.SetAVIWaferSum(strWaferSeq);

        }

        public void SetAVIWaferSummary(string strWaferSeq, string strLotSeq, string strWaferID)
        {
            TQP_WAFER_SUM obj = new TQP_WAFER_SUM();

            //WaferSeq 기준 Wafer sum 삭제후 다시 생성
            obj.DelAVIWaferSummary(strWaferSeq);
            obj.SetAVIWaferSummary(strWaferSeq);
            obj.ProbeCntPlus(long.Parse(strLotSeq), strWaferID);
        }


        public void SetAVILotSummary(string strLotSeq)
        {
            TQP_LOT_SUM obj = new TQP_LOT_SUM();

            //WaferSeq 기준 Wafer sum 삭제후 다시 생성
            obj.DelAVILotSummary(strLotSeq);
            obj.SetAVILotSummary(strLotSeq);
        }


        #region [ DMSMGR SUMMARY DATA ]
        /// <summary>
        /// PCM or CP 데이터 처리 후 Summary 진행
        /// </summary>
        /// <param name="lotSeq"></param>
        /// <param name="waferSeq"></param>
        /// 
        private readonly string DefaultGetBin = "BIN1";
        public void SummaryData(
            string factory, 
            string lotSeq,
            string waferSeq,
            string waferID
            )
        {
            TD_TABLE oTdTable = new TD_TABLE();
            TQP_BINDESC oBinDesc = new TQP_BINDESC();
            TQP_LOT oLot = new TQP_LOT();
            TQP_LOT_SUM oLotSum = new TQP_LOT_SUM();
            TQP_PARASPEC oParaSpec = new TQP_PARASPEC();
            TQP_WAFER oWafer = new TQP_WAFER();
            TQP_WAFER_SUM oWaferSum = new TQP_WAFER_SUM();

            //--

            DataTable dtBinDesc = null;
            DataTable dtLotSum = null;
            DataTable dtWafer = null;
            DataTable dtWaferSum = null;

            //--

            string ttable = string.Empty;
            string[] gecBinCode = null;

            // Step 1. 기존 데이터 삭제
            if (oLotSum.GetIsLot(lotSeq))
                oLotSum.DeleteLot(lotSeq);

            //--

            if (oWaferSum.GetIsWafer(waferSeq))
                oWaferSum.DeleteWaferData(waferSeq);

            // 기존 probe count 
            oWaferSum.ProbeCntPlus(lotSeq, waferID);

            //--

            // Step 2. Summary Table 생성
            DataTable dtNewLotSum = EmptyLotSumTable();
            DataTable dtNewWaferSum = EmptyWaferSumTable();

            //--

            dtWafer = oWafer.GetWaferInfoByWSEQ(waferSeq);

            // Step 3. Summary Data 생성
            foreach (DataRow row in dtWafer.Rows)
            {
                decimal dWaferSeq = decimal.Parse(row["WAFER_SEQ"].ToString());
                string program = row["PROGRAM"].ToString();
                string programRev = row["PROGRAM_REV"].ToString();

                ttable = oParaSpec.GetBinTTable(
                    dWaferSeq
                    );

                Dictionary<string, decimal> dic = null;
                if (!String.IsNullOrEmpty(ttable))
                {
                    dic = oTdTable.GetBinCount(
                        ttable, dWaferSeq
                        );
                }

                dtWaferSum = oWafer.GetWaferSummaryInfo(
                    lotSeq,
                    dWaferSeq
                    );

                if (dtWaferSum == null || dtWaferSum.Rows.Count == 0)
                    continue;

                //--

                /// GEC BIN에 대한 정보를 가져옴
                dtBinDesc = oBinDesc.SelectGecBin(
                    program
                    );

                if (dtBinDesc != null && dtBinDesc.Rows.Count > 0)
                {
                    gecBinCode = new string[dtBinDesc.Rows.Count];
                    for (int idx = 0; idx < dtBinDesc.Rows.Count; idx++)
                    {
                        gecBinCode[idx] = dtBinDesc.Rows[idx]["BIN_CODE"].ToString();
                    }
                }

                //--

                DataRow newWafer = dtNewWaferSum.NewRow();
                newWafer["CUSTOMER"] = dtWaferSum.Rows[0]["CUSTOMER"];
                newWafer["FACILITY"] = dtWaferSum.Rows[0]["FACILITY"];
                newWafer["PRODUCT"] = dtWaferSum.Rows[0]["PRODUCT"];
                newWafer["DEVICE_ALIAS"] = dtWaferSum.Rows[0]["DEVICE_ALIAS"];
                newWafer["TESTAREA"] = dtWaferSum.Rows[0]["TESTAREA"];
                newWafer["PROGRAM"] = dtWaferSum.Rows[0]["PROGRAM"];
                newWafer["MOTHER_LOT_ID"] = dtWaferSum.Rows[0]["MOTHER_LOT_ID"];
                newWafer["LOT_ID"] = dtWaferSum.Rows[0]["LOT_ID"];
                newWafer["LOT_SEQ"] = dtWaferSum.Rows[0]["LOT_SEQ"];
                newWafer["WAFER_ID"] = dtWaferSum.Rows[0]["WAFER_ID"];
                newWafer["WAFER_SEQ"] = dtWaferSum.Rows[0]["WAFER_SEQ"];
                newWafer["TESTER"] = dtWaferSum.Rows[0]["TESTER"];
                newWafer["PROBE_CARD"] = dtWaferSum.Rows[0]["PROBE_CARD"];
                newWafer["OPERATOR"] = dtWaferSum.Rows[0]["OPERATOR"];
                newWafer["PROBE_CNT"] = dtWaferSum.Rows[0]["PROBE_CNT"];
                newWafer["START_TIME"] = dtWaferSum.Rows[0]["START_TIME"];
                newWafer["END_TIME"] = dtWaferSum.Rows[0]["END_TIME"];
                newWafer["WAFER_CAT"] = dtWaferSum.Rows[0]["WAFER_CAT"];
                newWafer["PROBE_CNT"] = dtWaferSum.Rows[0]["PROBE_CNT"];
                newWafer["NETDIE"] = dtWaferSum.Rows[0]["NETDIE"];

                if (dic != null)
                {
                    double netDie = 0, totalDie = 0, tmpDie = 0, good = 0;

                    foreach (var item in dic)
                    {
                        if (gecBinCode == null || gecBinCode.Length <= 0)
                        {
                            if (string.Equals(item.Key, DefaultGetBin))
                                good += (int)item.Value;
                        }
                        else
                        {
                            string key = Array.Find(
                                gecBinCode,
                                x => String.Equals(item.Key, x)
                                );

                            if (!string.IsNullOrEmpty(key))
                                good += (int)item.Value;
                        }
                        newWafer[item.Key] = item.Value;
                        totalDie += (int)item.Value;
                    }

                    // TQP_MAPDEF 테이블에 ShotMap 에 대한 정보가 존재할 경우 
                    // NETDIE로 YIELD를 계산, 값이 존재하지 않는 경우에는 TestDie로 계산한다.
                    if (String.Equals(factory, "FAB2") && !String.IsNullOrEmpty(dtWaferSum.Rows[0]["NETDIE"].ToString()))
                    {
                        netDie = DACrux.Base.Convert.doubleParse(dtWaferSum.Rows[0]["NETDIE"].ToString());
                        if (netDie / totalDie > 1.5 || totalDie / netDie > 1.5)
                            tmpDie = totalDie;
                        else tmpDie = netDie;
                    }
                    else
                    {
                        tmpDie = totalDie;
                    }

                    newWafer["TESTED_DIE"] = totalDie;
                    newWafer["GEC"] = good;
                    newWafer["YIELD"] = Math.Round(100 * good / tmpDie, 2);
                }

                dtNewWaferSum.Rows.Add(newWafer);
            }

            /// Step 4. TQP_WAFER_SUM 저장
            if (dtNewWaferSum != null && dtNewWaferSum.Rows.Count > 0)
                oWaferSum.CreateDataWafer(
                    dtNewWaferSum
                    );

            //--

            /// Step 5. Lot Summary 에 대한 데이터 조회
            dtLotSum = oWaferSum.GetWaferSummaryInfo(
                lotSeq
                );

            if (dtLotSum == null || dtLotSum.Rows.Count <= 0)
                return;

            /// Step 6. Lot Summary 에 대한 데이터 생성
            DataRow newLot = dtNewLotSum.NewRow();
            newLot["CUSTOMER"] = dtLotSum.Rows[0]["CUSTOMER"];
            newLot["FACILITY"] = dtLotSum.Rows[0]["FACILITY"];
            newLot["PRODUCT"] = dtLotSum.Rows[0]["PRODUCT"];
            newLot["DEVICE_ALIAS"] = dtLotSum.Rows[0]["DEVICE_ALIAS"];
            newLot["TESTAREA"] = dtLotSum.Rows[0]["TESTAREA"];
            newLot["PROGRAM"] = dtLotSum.Rows[0]["PROGRAM"];
            newLot["MOTHER_LOT_ID"] = dtLotSum.Rows[0]["MOTHER_LOT_ID"];
            newLot["LOT_ID"] = dtLotSum.Rows[0]["LOT_ID"];
            newLot["LOT_SEQ"] = dtLotSum.Rows[0]["LOT_SEQ"];
            newLot["START_TIME"] = dtLotSum.Rows[0]["START_TIME"];
            newLot["END_TIME"] = dtLotSum.Rows[0]["END_TIME"];
            newLot["LOSS_DIE"] = dtLotSum.Rows[0]["LOSS_DIE"];
            newLot["TESTED_DIE"] = dtLotSum.Rows[0]["TESTED_DIE"];
            newLot["YIELD"] = dtLotSum.Rows[0]["YIELD"];
            newLot["BIN1"] = dtLotSum.Rows[0]["BIN1"];
            newLot["GEC"] = dtLotSum.Rows[0]["GEC"];
            newLot["WAFERS"] = dtLotSum.Rows[0]["WAFERS"];

            for (int i = 0; i < 100; i++)
            {
                string name = String.Format("BIN{0}", i);
                newLot[name] = dtLotSum.Rows[0][name];
            }

            dtNewLotSum.Rows.Add(newLot);

            /// Step 7. TQP_LOT_SUM 저장
            oLotSum.CreateDataLot(
                dtNewLotSum
                );
        }

        //public void SummaryWaferData(
        //   string waferSeq
        //   )
        //{
        //    TQP_BINDESC oBinDesc = new TQP_BINDESC();
        //    TD_TABLE oTdTable = new TD_TABLE();
        //    TQP_WAFER oWafer = new TQP_WAFER();
        //    TQP_WAFER_SUM oWaferSum = new TQP_WAFER_SUM();
        //    TQP_PARASPEC oParaSpec = new TQP_PARASPEC();

        //    //--

        //    DataTable dtWafer = null;
        //    DataTable dtWaferSum = null;

        //    //--

        //    string ttable = string.Empty;
        //    string[] gecBinCode = null;

        //    //--

        //    if (oWaferSum.GetIsWafer(waferSeq))
        //        oWaferSum.DeleteWaferData(waferSeq);

        //    //--

        //    // Step 2. Summary Table 생성
        //    DataTable dtNewWaferSum = EmptyWaferSumTable();

        //    //--

        //    dtWafer = oWafer.GetWaferInfoByWSEQ(waferSeq);

        //    // Step 3. Summary Data 생성
        //    foreach (DataRow row in dtWafer.Rows)
        //    {
        //        decimal dWaferSeq = decimal.Parse(row["WAFER_SEQ"].ToString());
        //        string program = row["PROGRAM"].ToString();
        //        string programRev = row["PROGRAM_REV"].ToString();

        //        ttable = oParaSpec.GetBinTTable(
        //                dWaferSeq
        //                );

        //        Dictionary<string, decimal> dic = null;
        //        if (!String.IsNullOrEmpty(ttable))
        //        {
        //            dic = oTdTable.GetBinCount(
        //                ttable, dWaferSeq
        //                );
        //        }

        //        dtWaferSum = oWafer.GetWaferSummaryInfo(
        //            dWaferSeq
        //            );

        //        if (dtWaferSum == null || dtWaferSum.Rows.Count == 0)
        //            continue;

        //        //--

        //        DataRow newWafer = dtNewWaferSum.NewRow();
        //        newWafer["CUSTOMER"] = dtWaferSum.Rows[0]["CUSTOMER"];
        //        newWafer["FACILITY"] = dtWaferSum.Rows[0]["FACILITY"];
        //        newWafer["PRODUCT"] = dtWaferSum.Rows[0]["PRODUCT"];
        //        newWafer["DEVICE_ALIAS"] = dtWaferSum.Rows[0]["DEVICE_ALIAS"];
        //        newWafer["TESTAREA"] = dtWaferSum.Rows[0]["TESTAREA"];
        //        newWafer["PROGRAM"] = dtWaferSum.Rows[0]["PROGRAM"];
        //        newWafer["MOTHER_LOT_ID"] = dtWaferSum.Rows[0]["MOTHER_LOT_ID"];
        //        newWafer["LOT_ID"] = dtWaferSum.Rows[0]["LOT_ID"];
        //        newWafer["LOT_SEQ"] = dtWaferSum.Rows[0]["LOT_SEQ"];
        //        newWafer["WAFER_ID"] = dtWaferSum.Rows[0]["WAFER_ID"];
        //        newWafer["WAFER_SEQ"] = dtWaferSum.Rows[0]["WAFER_SEQ"];
        //        newWafer["TESTER"] = dtWaferSum.Rows[0]["TESTER"];
        //        newWafer["PROBE_CARD"] = dtWaferSum.Rows[0]["PROBE_CARD"];
        //        newWafer["OPERATOR"] = dtWaferSum.Rows[0]["OPERATOR"];
        //        newWafer["PROBE_CNT"] = dtWaferSum.Rows[0]["PROBE_CNT"];
        //        newWafer["START_TIME"] = dtWaferSum.Rows[0]["START_TIME"];
        //        newWafer["END_TIME"] = dtWaferSum.Rows[0]["END_TIME"];
        //        newWafer["WAFER_CAT"] = dtWaferSum.Rows[0]["WAFER_CAT"];
        //        newWafer["PROBE_CNT"] = dtWaferSum.Rows[0]["PROBE_CNT"];

        //        if (dic != null)
        //        {
        //            double sum = 0, good = 0;

        //            foreach (var item in dic)
        //            {
        //                if (gecBinCode == null || gecBinCode.Length <= 0)
        //                {
        //                    if (string.Equals(item.Key, DefaultGetBin))
        //                        good += (int)item.Value;
        //                }
        //                else
        //                {
        //                    string key = Array.Find(
        //                        gecBinCode,
        //                        x => String.Equals(item.Key, x)
        //                        );

        //                    if (!string.IsNullOrEmpty(key))
        //                        good += (int)item.Value;
        //                }
        //                newWafer[item.Key] = item.Value;
        //                sum += (int)item.Value;
        //            }

        //            newWafer["TESTED_DIE"] = sum;
        //            newWafer["YIELD"] = Math.Round(100 * good / sum, 2);
        //        }

        //        dtNewWaferSum.Rows.Add(newWafer);
        //    }

        //    /// Step 4. TQP_WAFER_SUM 저장
        //    if (dtNewWaferSum != null && dtNewWaferSum.Rows.Count > 0)
        //        oWaferSum.CreateDataWafer(
        //            dtNewWaferSum
        //            );
        //}

        //--------------------------------------------------------------------------------

        private DataTable EmptyLotSumTable(
            )
        {
            DataTable dt = new DataTable("TQP_LOT_SUM");
            dt.Columns.Add("CUSTOMER", typeof(string));
            dt.Columns.Add("FACILITY", typeof(string));
            dt.Columns.Add("PRODUCT", typeof(string));
            dt.Columns.Add("DEVICE_ALIAS", typeof(string));
            dt.Columns.Add("TESTAREA", typeof(string));
            dt.Columns.Add("PROGRAM", typeof(string));
            dt.Columns.Add("MOTHER_LOT_ID", typeof(string));
            dt.Columns.Add("LOT_ID", typeof(string));
            dt.Columns.Add("LOT_SEQ", typeof(decimal));
            dt.Columns.Add("START_TIME", typeof(DateTime));
            dt.Columns.Add("END_TIME", typeof(DateTime));
            dt.Columns.Add("LOSS_DIE", typeof(decimal));
            dt.Columns.Add("TESTED_DIE", typeof(decimal));
            dt.Columns.Add("YIELD", typeof(decimal));
            dt.Columns.Add("FTA", typeof(decimal));
            dt.Columns.Add("GEC", typeof(decimal));
            dt.Columns.Add("WAFERS", typeof(decimal));
            dt.Columns.Add("BIN0", typeof(decimal));
            dt.Columns.Add("BIN1", typeof(decimal));
            dt.Columns.Add("BIN2", typeof(decimal));
            dt.Columns.Add("BIN3", typeof(decimal));
            dt.Columns.Add("BIN4", typeof(decimal));
            dt.Columns.Add("BIN5", typeof(decimal));
            dt.Columns.Add("BIN6", typeof(decimal));
            dt.Columns.Add("BIN7", typeof(decimal));
            dt.Columns.Add("BIN8", typeof(decimal));
            dt.Columns.Add("BIN9", typeof(decimal));
            dt.Columns.Add("BIN10", typeof(decimal));
            dt.Columns.Add("BIN11", typeof(decimal));
            dt.Columns.Add("BIN12", typeof(decimal));
            dt.Columns.Add("BIN13", typeof(decimal));
            dt.Columns.Add("BIN14", typeof(decimal));
            dt.Columns.Add("BIN15", typeof(decimal));
            dt.Columns.Add("BIN16", typeof(decimal));
            dt.Columns.Add("BIN17", typeof(decimal));
            dt.Columns.Add("BIN18", typeof(decimal));
            dt.Columns.Add("BIN19", typeof(decimal));
            dt.Columns.Add("BIN20", typeof(decimal));
            dt.Columns.Add("BIN21", typeof(decimal));
            dt.Columns.Add("BIN22", typeof(decimal));
            dt.Columns.Add("BIN23", typeof(decimal));
            dt.Columns.Add("BIN24", typeof(decimal));
            dt.Columns.Add("BIN25", typeof(decimal));
            dt.Columns.Add("BIN26", typeof(decimal));
            dt.Columns.Add("BIN27", typeof(decimal));
            dt.Columns.Add("BIN28", typeof(decimal));
            dt.Columns.Add("BIN29", typeof(decimal));
            dt.Columns.Add("BIN30", typeof(decimal));
            dt.Columns.Add("BIN31", typeof(decimal));
            dt.Columns.Add("BIN32", typeof(decimal));
            dt.Columns.Add("BIN33", typeof(decimal));
            dt.Columns.Add("BIN34", typeof(decimal));
            dt.Columns.Add("BIN35", typeof(decimal));
            dt.Columns.Add("BIN36", typeof(decimal));
            dt.Columns.Add("BIN37", typeof(decimal));
            dt.Columns.Add("BIN38", typeof(decimal));
            dt.Columns.Add("BIN39", typeof(decimal));
            dt.Columns.Add("BIN40", typeof(decimal));
            dt.Columns.Add("BIN41", typeof(decimal));
            dt.Columns.Add("BIN42", typeof(decimal));
            dt.Columns.Add("BIN43", typeof(decimal));
            dt.Columns.Add("BIN44", typeof(decimal));
            dt.Columns.Add("BIN45", typeof(decimal));
            dt.Columns.Add("BIN46", typeof(decimal));
            dt.Columns.Add("BIN47", typeof(decimal));
            dt.Columns.Add("BIN48", typeof(decimal));
            dt.Columns.Add("BIN49", typeof(decimal));
            dt.Columns.Add("BIN50", typeof(decimal));
            dt.Columns.Add("BIN51", typeof(decimal));
            dt.Columns.Add("BIN52", typeof(decimal));
            dt.Columns.Add("BIN53", typeof(decimal));
            dt.Columns.Add("BIN54", typeof(decimal));
            dt.Columns.Add("BIN55", typeof(decimal));
            dt.Columns.Add("BIN56", typeof(decimal));
            dt.Columns.Add("BIN57", typeof(decimal));
            dt.Columns.Add("BIN58", typeof(decimal));
            dt.Columns.Add("BIN59", typeof(decimal));
            dt.Columns.Add("BIN60", typeof(decimal));
            dt.Columns.Add("BIN61", typeof(decimal));
            dt.Columns.Add("BIN62", typeof(decimal));
            dt.Columns.Add("BIN63", typeof(decimal));
            dt.Columns.Add("BIN64", typeof(decimal));
            dt.Columns.Add("BIN65", typeof(decimal));
            dt.Columns.Add("BIN66", typeof(decimal));
            dt.Columns.Add("BIN67", typeof(decimal));
            dt.Columns.Add("BIN68", typeof(decimal));
            dt.Columns.Add("BIN69", typeof(decimal));
            dt.Columns.Add("BIN70", typeof(decimal));
            dt.Columns.Add("BIN71", typeof(decimal));
            dt.Columns.Add("BIN72", typeof(decimal));
            dt.Columns.Add("BIN73", typeof(decimal));
            dt.Columns.Add("BIN74", typeof(decimal));
            dt.Columns.Add("BIN75", typeof(decimal));
            dt.Columns.Add("BIN76", typeof(decimal));
            dt.Columns.Add("BIN77", typeof(decimal));
            dt.Columns.Add("BIN78", typeof(decimal));
            dt.Columns.Add("BIN79", typeof(decimal));
            dt.Columns.Add("BIN80", typeof(decimal));
            dt.Columns.Add("BIN81", typeof(decimal));
            dt.Columns.Add("BIN82", typeof(decimal));
            dt.Columns.Add("BIN83", typeof(decimal));
            dt.Columns.Add("BIN84", typeof(decimal));
            dt.Columns.Add("BIN85", typeof(decimal));
            dt.Columns.Add("BIN86", typeof(decimal));
            dt.Columns.Add("BIN87", typeof(decimal));
            dt.Columns.Add("BIN88", typeof(decimal));
            dt.Columns.Add("BIN89", typeof(decimal));
            dt.Columns.Add("BIN90", typeof(decimal));
            dt.Columns.Add("BIN91", typeof(decimal));
            dt.Columns.Add("BIN92", typeof(decimal));
            dt.Columns.Add("BIN93", typeof(decimal));
            dt.Columns.Add("BIN94", typeof(decimal));
            dt.Columns.Add("BIN95", typeof(decimal));
            dt.Columns.Add("BIN96", typeof(decimal));
            dt.Columns.Add("BIN97", typeof(decimal));
            dt.Columns.Add("BIN98", typeof(decimal));
            dt.Columns.Add("BIN99", typeof(decimal));
            dt.Columns.Add("BIN100", typeof(decimal));
            dt.Columns.Add("BIN101", typeof(decimal));
            dt.Columns.Add("BIN102", typeof(decimal));
            dt.Columns.Add("BIN103", typeof(decimal));
            dt.Columns.Add("BIN104", typeof(decimal));
            dt.Columns.Add("BIN105", typeof(decimal));
            dt.Columns.Add("BIN106", typeof(decimal));
            dt.Columns.Add("BIN107", typeof(decimal));
            dt.Columns.Add("BIN108", typeof(decimal));
            dt.Columns.Add("BIN109", typeof(decimal));
            dt.Columns.Add("BIN110", typeof(decimal));
            dt.Columns.Add("BIN111", typeof(decimal));
            dt.Columns.Add("BIN112", typeof(decimal));
            dt.Columns.Add("BIN113", typeof(decimal));
            dt.Columns.Add("BIN114", typeof(decimal));
            dt.Columns.Add("BIN115", typeof(decimal));
            dt.Columns.Add("BIN116", typeof(decimal));
            dt.Columns.Add("BIN117", typeof(decimal));
            dt.Columns.Add("BIN118", typeof(decimal));
            dt.Columns.Add("BIN119", typeof(decimal));
            dt.Columns.Add("BIN120", typeof(decimal));
            dt.Columns.Add("BIN121", typeof(decimal));
            dt.Columns.Add("BIN122", typeof(decimal));
            dt.Columns.Add("BIN123", typeof(decimal));
            dt.Columns.Add("BIN124", typeof(decimal));
            dt.Columns.Add("BIN125", typeof(decimal));
            dt.Columns.Add("BIN126", typeof(decimal));
            dt.Columns.Add("BIN127", typeof(decimal));
            dt.Columns.Add("BIN128", typeof(decimal));
            dt.Columns.Add("BIN129", typeof(decimal));
            dt.Columns.Add("BIN130", typeof(decimal));
            dt.Columns.Add("BIN131", typeof(decimal));
            dt.Columns.Add("BIN132", typeof(decimal));
            dt.Columns.Add("BIN133", typeof(decimal));
            dt.Columns.Add("BIN134", typeof(decimal));
            dt.Columns.Add("BIN135", typeof(decimal));
            dt.Columns.Add("BIN136", typeof(decimal));
            dt.Columns.Add("BIN137", typeof(decimal));
            dt.Columns.Add("BIN138", typeof(decimal));
            dt.Columns.Add("BIN139", typeof(decimal));
            dt.Columns.Add("BIN140", typeof(decimal));
            dt.Columns.Add("BIN141", typeof(decimal));
            dt.Columns.Add("BIN142", typeof(decimal));
            dt.Columns.Add("BIN143", typeof(decimal));
            dt.Columns.Add("BIN144", typeof(decimal));
            dt.Columns.Add("BIN145", typeof(decimal));
            dt.Columns.Add("BIN146", typeof(decimal));
            dt.Columns.Add("BIN147", typeof(decimal));
            dt.Columns.Add("BIN148", typeof(decimal));
            dt.Columns.Add("BIN149", typeof(decimal));
            dt.Columns.Add("BIN150", typeof(decimal));
            return dt;
        }

        //--------------------------------------------------------------------------------

        private DataTable EmptyWaferSumTable(
            )
        {
            DataTable dt = new DataTable("TQP_WAFER_SUM");
            dt.Columns.Add("CUSTOMER", typeof(string));
            dt.Columns.Add("FACILITY", typeof(string));
            dt.Columns.Add("PRODUCT", typeof(string));
            dt.Columns.Add("DEVICE_ALIAS", typeof(string));
            dt.Columns.Add("TESTAREA", typeof(string));
            dt.Columns.Add("PROGRAM", typeof(string));
            dt.Columns.Add("MOTHER_LOT_ID", typeof(string));
            dt.Columns.Add("LOT_ID", typeof(string));
            dt.Columns.Add("LOT_SEQ", typeof(decimal));
            dt.Columns.Add("WAFER_ID", typeof(string));
            dt.Columns.Add("WAFER_SEQ", typeof(decimal));
            dt.Columns.Add("TESTER", typeof(string));
            dt.Columns.Add("PROBE_CARD", typeof(string));
            dt.Columns.Add("OPERATOR", typeof(string));
            dt.Columns.Add("PROBE_CNT", typeof(decimal));
            dt.Columns.Add("NETDIE", typeof(decimal));
            dt.Columns.Add("START_TIME", typeof(DateTime));
            dt.Columns.Add("END_TIME", typeof(DateTime));
            dt.Columns.Add("WAFER_CAT", typeof(decimal));
            dt.Columns.Add("LOSS_DIE", typeof(decimal));
            dt.Columns.Add("TESTED_DIE", typeof(decimal));
            dt.Columns.Add("YIELD", typeof(decimal));
            dt.Columns.Add("FTA", typeof(decimal));
            dt.Columns.Add("GEC", typeof(decimal));
            dt.Columns.Add("BIN0", typeof(decimal));
            dt.Columns.Add("BIN1", typeof(decimal));
            dt.Columns.Add("BIN2", typeof(decimal));
            dt.Columns.Add("BIN3", typeof(decimal));
            dt.Columns.Add("BIN4", typeof(decimal));
            dt.Columns.Add("BIN5", typeof(decimal));
            dt.Columns.Add("BIN6", typeof(decimal));
            dt.Columns.Add("BIN7", typeof(decimal));
            dt.Columns.Add("BIN8", typeof(decimal));
            dt.Columns.Add("BIN9", typeof(decimal));
            dt.Columns.Add("BIN10", typeof(decimal));
            dt.Columns.Add("BIN11", typeof(decimal));
            dt.Columns.Add("BIN12", typeof(decimal));
            dt.Columns.Add("BIN13", typeof(decimal));
            dt.Columns.Add("BIN14", typeof(decimal));
            dt.Columns.Add("BIN15", typeof(decimal));
            dt.Columns.Add("BIN16", typeof(decimal));
            dt.Columns.Add("BIN17", typeof(decimal));
            dt.Columns.Add("BIN18", typeof(decimal));
            dt.Columns.Add("BIN19", typeof(decimal));
            dt.Columns.Add("BIN20", typeof(decimal));
            dt.Columns.Add("BIN21", typeof(decimal));
            dt.Columns.Add("BIN22", typeof(decimal));
            dt.Columns.Add("BIN23", typeof(decimal));
            dt.Columns.Add("BIN24", typeof(decimal));
            dt.Columns.Add("BIN25", typeof(decimal));
            dt.Columns.Add("BIN26", typeof(decimal));
            dt.Columns.Add("BIN27", typeof(decimal));
            dt.Columns.Add("BIN28", typeof(decimal));
            dt.Columns.Add("BIN29", typeof(decimal));
            dt.Columns.Add("BIN30", typeof(decimal));
            dt.Columns.Add("BIN31", typeof(decimal));
            dt.Columns.Add("BIN32", typeof(decimal));
            dt.Columns.Add("BIN33", typeof(decimal));
            dt.Columns.Add("BIN34", typeof(decimal));
            dt.Columns.Add("BIN35", typeof(decimal));
            dt.Columns.Add("BIN36", typeof(decimal));
            dt.Columns.Add("BIN37", typeof(decimal));
            dt.Columns.Add("BIN38", typeof(decimal));
            dt.Columns.Add("BIN39", typeof(decimal));
            dt.Columns.Add("BIN40", typeof(decimal));
            dt.Columns.Add("BIN41", typeof(decimal));
            dt.Columns.Add("BIN42", typeof(decimal));
            dt.Columns.Add("BIN43", typeof(decimal));
            dt.Columns.Add("BIN44", typeof(decimal));
            dt.Columns.Add("BIN45", typeof(decimal));
            dt.Columns.Add("BIN46", typeof(decimal));
            dt.Columns.Add("BIN47", typeof(decimal));
            dt.Columns.Add("BIN48", typeof(decimal));
            dt.Columns.Add("BIN49", typeof(decimal));
            dt.Columns.Add("BIN50", typeof(decimal));
            dt.Columns.Add("BIN51", typeof(decimal));
            dt.Columns.Add("BIN52", typeof(decimal));
            dt.Columns.Add("BIN53", typeof(decimal));
            dt.Columns.Add("BIN54", typeof(decimal));
            dt.Columns.Add("BIN55", typeof(decimal));
            dt.Columns.Add("BIN56", typeof(decimal));
            dt.Columns.Add("BIN57", typeof(decimal));
            dt.Columns.Add("BIN58", typeof(decimal));
            dt.Columns.Add("BIN59", typeof(decimal));
            dt.Columns.Add("BIN60", typeof(decimal));
            dt.Columns.Add("BIN61", typeof(decimal));
            dt.Columns.Add("BIN62", typeof(decimal));
            dt.Columns.Add("BIN63", typeof(decimal));
            dt.Columns.Add("BIN64", typeof(decimal));
            dt.Columns.Add("BIN65", typeof(decimal));
            dt.Columns.Add("BIN66", typeof(decimal));
            dt.Columns.Add("BIN67", typeof(decimal));
            dt.Columns.Add("BIN68", typeof(decimal));
            dt.Columns.Add("BIN69", typeof(decimal));
            dt.Columns.Add("BIN70", typeof(decimal));
            dt.Columns.Add("BIN71", typeof(decimal));
            dt.Columns.Add("BIN72", typeof(decimal));
            dt.Columns.Add("BIN73", typeof(decimal));
            dt.Columns.Add("BIN74", typeof(decimal));
            dt.Columns.Add("BIN75", typeof(decimal));
            dt.Columns.Add("BIN76", typeof(decimal));
            dt.Columns.Add("BIN77", typeof(decimal));
            dt.Columns.Add("BIN78", typeof(decimal));
            dt.Columns.Add("BIN79", typeof(decimal));
            dt.Columns.Add("BIN80", typeof(decimal));
            dt.Columns.Add("BIN81", typeof(decimal));
            dt.Columns.Add("BIN82", typeof(decimal));
            dt.Columns.Add("BIN83", typeof(decimal));
            dt.Columns.Add("BIN84", typeof(decimal));
            dt.Columns.Add("BIN85", typeof(decimal));
            dt.Columns.Add("BIN86", typeof(decimal));
            dt.Columns.Add("BIN87", typeof(decimal));
            dt.Columns.Add("BIN88", typeof(decimal));
            dt.Columns.Add("BIN89", typeof(decimal));
            dt.Columns.Add("BIN90", typeof(decimal));
            dt.Columns.Add("BIN91", typeof(decimal));
            dt.Columns.Add("BIN92", typeof(decimal));
            dt.Columns.Add("BIN93", typeof(decimal));
            dt.Columns.Add("BIN94", typeof(decimal));
            dt.Columns.Add("BIN95", typeof(decimal));
            dt.Columns.Add("BIN96", typeof(decimal));
            dt.Columns.Add("BIN97", typeof(decimal));
            dt.Columns.Add("BIN98", typeof(decimal));
            dt.Columns.Add("BIN99", typeof(decimal));
            dt.Columns.Add("BIN100", typeof(decimal));
            dt.Columns.Add("BIN101", typeof(decimal));
            dt.Columns.Add("BIN102", typeof(decimal));
            dt.Columns.Add("BIN103", typeof(decimal));
            dt.Columns.Add("BIN104", typeof(decimal));
            dt.Columns.Add("BIN105", typeof(decimal));
            dt.Columns.Add("BIN106", typeof(decimal));
            dt.Columns.Add("BIN107", typeof(decimal));
            dt.Columns.Add("BIN108", typeof(decimal));
            dt.Columns.Add("BIN109", typeof(decimal));
            dt.Columns.Add("BIN110", typeof(decimal));
            dt.Columns.Add("BIN111", typeof(decimal));
            dt.Columns.Add("BIN112", typeof(decimal));
            dt.Columns.Add("BIN113", typeof(decimal));
            dt.Columns.Add("BIN114", typeof(decimal));
            dt.Columns.Add("BIN115", typeof(decimal));
            dt.Columns.Add("BIN116", typeof(decimal));
            dt.Columns.Add("BIN117", typeof(decimal));
            dt.Columns.Add("BIN118", typeof(decimal));
            dt.Columns.Add("BIN119", typeof(decimal));
            dt.Columns.Add("BIN120", typeof(decimal));
            dt.Columns.Add("BIN121", typeof(decimal));
            dt.Columns.Add("BIN122", typeof(decimal));
            dt.Columns.Add("BIN123", typeof(decimal));
            dt.Columns.Add("BIN124", typeof(decimal));
            dt.Columns.Add("BIN125", typeof(decimal));
            dt.Columns.Add("BIN126", typeof(decimal));
            dt.Columns.Add("BIN127", typeof(decimal));
            dt.Columns.Add("BIN128", typeof(decimal));
            dt.Columns.Add("BIN129", typeof(decimal));
            dt.Columns.Add("BIN130", typeof(decimal));
            dt.Columns.Add("BIN131", typeof(decimal));
            dt.Columns.Add("BIN132", typeof(decimal));
            dt.Columns.Add("BIN133", typeof(decimal));
            dt.Columns.Add("BIN134", typeof(decimal));
            dt.Columns.Add("BIN135", typeof(decimal));
            dt.Columns.Add("BIN136", typeof(decimal));
            dt.Columns.Add("BIN137", typeof(decimal));
            dt.Columns.Add("BIN138", typeof(decimal));
            dt.Columns.Add("BIN139", typeof(decimal));
            dt.Columns.Add("BIN140", typeof(decimal));
            dt.Columns.Add("BIN141", typeof(decimal));
            dt.Columns.Add("BIN142", typeof(decimal));
            dt.Columns.Add("BIN143", typeof(decimal));
            dt.Columns.Add("BIN144", typeof(decimal));
            dt.Columns.Add("BIN145", typeof(decimal));
            dt.Columns.Add("BIN146", typeof(decimal));
            dt.Columns.Add("BIN147", typeof(decimal));
            dt.Columns.Add("BIN148", typeof(decimal));
            dt.Columns.Add("BIN149", typeof(decimal));
            dt.Columns.Add("BIN150", typeof(decimal));
            return dt;
        }


        /// <summary>
        /// Scope Data 저장 시 사용
        /// </summary>
        /// <param name="strWaferSeq"></param>
        /// <param name="DiesList"></param>
        /// <param name="iTotalDies"></param>
        /// <param name="iINDEX_XMAX"></param>
        /// <param name="iINDEX_YMAX"></param>
        /// <param name="strUser"></param>
        /// <param name="strFTPPath"></param>
        public void ScopeDataSet(string strWaferSeq, DACrux.Base.DieList oDiesList, int iTotalDies, int iINDEX_XMAX, int iINDEX_YMAX, string strUser, bool bAlterInfo, string strAlterLotID, string strAlterWaferID, DACrux.Base.TestImageList oTestImageList)
        {
            DACrux.TEST.BSL.TestCommon oTest = new DACrux.TEST.BSL.TestCommon();

            DACrux.TEST.DSL.TQP_WAFER_SUM oWafer = new DACrux.TEST.DSL.TQP_WAFER_SUM();
            DACrux.TEST.DSL.TQP_FOI_IMAGES oFOIImage = new DACrux.TEST.DSL.TQP_FOI_IMAGES();

            DataTable dtBaseWaferInfo = null;
            DataTable dtTemp = null;

            string lotSeq = string.Empty;
            string waferSeq = string.Empty;
            string TO_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
            int WAFERDIAMETER = 200;

            DateTime dtTime = DateTime.Now;

            try
            {
                dtBaseWaferInfo = oWafer.GetWaferLotInfo(DACrux.Base.Convert.longParse(strWaferSeq));

                string sCUSTOMER = dtBaseWaferInfo.Rows[0]["CUSTOMER"].ToString();
                string sFACILITY = dtBaseWaferInfo.Rows[0]["FACILITY"].ToString();
                string sPRODUCT = dtBaseWaferInfo.Rows[0]["PRODUCT"].ToString();
                string sDEVICE_ALIAS = dtBaseWaferInfo.Rows[0]["DEVICE_ALIAS"].ToString();
                string sTESTAREA = "AVI";
                string sPROGRAM = "SCOPE";
                string sMOTHER_LOT_ID = dtBaseWaferInfo.Rows[0]["MOTHER_LOT_ID"].ToString();
                string sLOT_ID = dtBaseWaferInfo.Rows[0]["LOT_ID"].ToString();
                string sWAFER_ID = dtBaseWaferInfo.Rows[0]["WAFER_ID"].ToString();
                string sTESTER = "SCOPE";

                if (bAlterInfo)
                {
                    sLOT_ID = strAlterLotID;
                    sWAFER_ID = strAlterWaferID;
                }

                //////////////////////////////////////////////////////////////////////////////////////////////
                //Step 01 : TQP_PRODUCT 확인 생성
                //////////////////////////////////////////////////////////////////////////////////////////////
                oTest.InsertProductInfo(sFACILITY, sPRODUCT, sDEVICE_ALIAS);


                //////////////////////////////////////////////////////////////////////////////////////////////
                //Step 02 : TQP_PROGRAM 확인 생성
                //////////////////////////////////////////////////////////////////////////////////////////////

                //Scope 생성 시 Fix
                oTest.InsertProgramInfo(sFACILITY, sPROGRAM, sTESTAREA, sPROGRAM);

                //////////////////////////////////////////////////////////////////////////////////////////////
                //Step 03 : TQP_WAFER 확인 생성
                //////////////////////////////////////////////////////////////////////////////////////////////
                oTest.SaveWaferData(
                    sFACILITY,
                    sTESTAREA,
                    sPRODUCT,
                    sDEVICE_ALIAS,
                    sLOT_ID,
                    sWAFER_ID,
                    dtTime.ToString(TO_DATE_FORMAT),
                    dtTime.ToString(TO_DATE_FORMAT),
                    WAFERDIAMETER,
                    sTESTER,
                    "Unknown",
                    "Unknown",
                    strUser,
                    sPROGRAM,
                    0,
                    iTotalDies,
                    "Unknown",
                    iINDEX_XMAX,
                    iINDEX_YMAX,
                    0.ToString(),
                    "Scope Manual Upload",
                    null,
                     "Unknown",
                     "Unknown",
                    "-1",
                    "-1",
                    "Unknown",
                    iTotalDies,
                    out lotSeq,
                    out waferSeq
                    );

                //////////////////////////////////////////////////////////////////////////////////////////////
                //Step 04 : TQP_FOI_DIE 확인 생성
                //////////////////////////////////////////////////////////////////////////////////////////////
                //TQP_FOI_DIE Table Data 생성

                string[,] para = new string[oDiesList.Count, 12];
                for (int iDie = 0; iDie < oDiesList.Count; iDie++)
                {
                    para[iDie, 0] = waferSeq.ToString();                                                            //WAFER_SEQ
                    para[iDie, 1] = (iDie + 1).ToString();                                                          //DIE_NUM
                    para[iDie, 2] = "AVI";                                                                          //TEST_AREA
                    para[iDie, 3] = 0.ToString();                                                                   //DIEPROBE_CNT
                    para[iDie, 4] = oDiesList[iDie].IndexX.ToString();                                              //X
                    para[iDie, 5] = oDiesList[iDie].IndexY.ToString();                                              //Y
                    para[iDie, 6] = oDiesList[iDie].BinNumber.ToString();                                           //BinNumber
                    para[iDie, 7] = oDiesList[iDie].BinNumber.ToString();                                           //BIN_CHAR
                    para[iDie, 8] = oDiesList[iDie].ScopeImagePath == null ? "" : oDiesList[iDie].ScopeImagePath;   //IMAGE_PATH
                    para[iDie, 9] = "";                                                                             //THUMB_PATH
                    para[iDie, 10] = oDiesList[iDie].ScopeImage == null ? "" : oDiesList[iDie].ScopeImage;          //IMAGE_FILE_NAME
                    para[iDie, 11] = "";                                                                            //THUMB_FILENAME
                }

                oTest.DeleteFOIDiesMulti(waferSeq.ToString());

                if(oDiesList.Count > 0 )
                    oTest.CreateFOIDiesMulti(para);

                para = new string[oTestImageList.Count, 8];
                for (int iImage = 0; iImage < oTestImageList.Count; iImage++)
                {
                    para[iImage, 0] = waferSeq.ToString();                                                                          //WAFER_SEQ
                    para[iImage, 1] = oTestImageList[iImage].IndexX;                                                                //X
                    para[iImage, 2] = oTestImageList[iImage].IndexY;                                                                //Y
                    para[iImage, 3] = oTestImageList[iImage].BinNumber.ToString();                                                  //BinNumber
                    para[iImage, 4] = oTestImageList[iImage].ServerImagePath == null ? "" : oTestImageList[iImage].ServerImagePath; //IMAGE_PATH
                    para[iImage, 5] = "";                                                                                           //THUMB_PATH
                    para[iImage, 6] = oTestImageList[iImage].LocalImageName == null ? "" : oTestImageList[iImage].LocalImageName;   //IMAGE_FILE_NAME
                    para[iImage, 7] = "";                
                }

                oFOIImage.DeleteFOIImageList(waferSeq.ToString());

                if(oTestImageList.Count > 0)
                    oFOIImage.CreateFOIImagesMulti(para);

                //////////////////////////////////////////////////////////////////////////////////////////////
                //Step 05 : TQP_BINDESC 확인 생성
                //////////////////////////////////////////////////////////////////////////////////////////////
                //TQP_BINDESC 에 정의되지 않을 경우 임의로 생성 해준다.
                dtTemp = oTest.SelectFOIBinList(waferSeq);
                foreach (DataRow dr in dtTemp.Rows)
                    oTest.SetBinDESC_BinInfo(sPROGRAM, dr["BIN"].ToString(), dr["BIN_CHAR"].ToString(), "Scope Manual Create");

                //////////////////////////////////////////////////////////////////////////////////////////////
                //Step 06 : TQP_WAFER_SUM 확인 생성
                //////////////////////////////////////////////////////////////////////////////////////////////
                oTest.SetAVIWaferSummary(waferSeq, lotSeq, sWAFER_ID);

                //////////////////////////////////////////////////////////////////////////////////////////////
                //Step 07 : TQP_LOT_SUM 확인 생성
                //////////////////////////////////////////////////////////////////////////////////////////////
                oTest.SetAVILotSummary(lotSeq);

            }
            finally
            {
            }

        }

        /// <summary>
        /// Shot Map 기준으로 새로 생성 한다.
        /// </summary>
        /// <param name="strFactory"></param>
        /// <param name="strUser"></param>
        /// <param name="strAlterLotID"></param>
        /// <param name="strAlterWaferID"></param>
        /// <param name="AlterDevice"></param>
        /// <param name="oDiesList"></param>
        /// <param name="iTotalDies"></param>
        /// <param name="iINDEX_XMAX"></param>
        /// <param name="iINDEX_YMAX"></param>
        public void ScopeNewLotDataSet(string strFactory, string strUser, string strAlterLotID, string strAlterWaferID, string AlterDevice, DACrux.Base.DieList oDiesList, int iTotalDies, int iINDEX_XMAX, int iINDEX_YMAX, DACrux.Base.TestImageList oTestImageList)
        {
            DACrux.TEST.BSL.TestCommon oTest = new DACrux.TEST.BSL.TestCommon();

            DACrux.TEST.DSL.TQP_WAFER_SUM oWafer = new DACrux.TEST.DSL.TQP_WAFER_SUM();
            DACrux.TEST.DSL.TQP_FOI_IMAGES oFOIImage = new DACrux.TEST.DSL.TQP_FOI_IMAGES();

            DataTable dtTemp = null;

            string lotSeq = string.Empty;
            string waferSeq = string.Empty;
            string TO_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
            int WAFERDIAMETER = 200;

            DateTime dtTime = DateTime.Now;

            try
            {
                string sFACILITY = strFactory;
                string sPRODUCT = AlterDevice;
                string sDEVICE_ALIAS = AlterDevice;
                string sTESTAREA = "AVI";
                string sPROGRAM = "SCOPE";
                string sMOTHER_LOT_ID = strAlterLotID;
                string sLOT_ID = strAlterLotID;
                string sWAFER_ID = strAlterWaferID;
                string sTESTER = "SCOPE";

                //////////////////////////////////////////////////////////////////////////////////////////////
                //Step 01 : TQP_PRODUCT 확인 생성
                //////////////////////////////////////////////////////////////////////////////////////////////
                oTest.InsertProductInfo(sFACILITY, sPRODUCT, sDEVICE_ALIAS);


                //////////////////////////////////////////////////////////////////////////////////////////////
                //Step 02 : TQP_PROGRAM 확인 생성
                //////////////////////////////////////////////////////////////////////////////////////////////

                //Scope 생성 시 Fix
                oTest.InsertProgramInfo(sFACILITY, sPROGRAM, sTESTAREA, sPROGRAM);

                //////////////////////////////////////////////////////////////////////////////////////////////
                //Step 03 : TQP_WAFER 확인 생성
                //////////////////////////////////////////////////////////////////////////////////////////////
                oTest.SaveWaferData(
                    sFACILITY,
                    sTESTAREA,
                    sPRODUCT,
                    sDEVICE_ALIAS,
                    sLOT_ID,
                    sWAFER_ID,
                    dtTime.ToString(TO_DATE_FORMAT),
                    dtTime.ToString(TO_DATE_FORMAT),
                    WAFERDIAMETER,
                    sTESTER,
                    "Unknown",
                    "Unknown",
                    strUser,
                    sPROGRAM,
                    0,
                    iTotalDies,
                    "Unknown",
                    iINDEX_XMAX,
                    iINDEX_YMAX,
                    0.ToString(),
                    "Scope Manual Upload",
                    null,
                     "Unknown",
                     "Unknown",
                    "-1",
                    "-1",
                    "Unknown",
                    iTotalDies,
                    out lotSeq,
                    out waferSeq
                    );

                //////////////////////////////////////////////////////////////////////////////////////////////
                //Step 04 : TQP_FOI_DIE 확인 생성
                //////////////////////////////////////////////////////////////////////////////////////////////
                //TQP_FOI_DIE Table Data 생성

                string[,] para = new string[oDiesList.Count, 12];
                for (int iDie = 0; iDie < oDiesList.Count; iDie++)
                {
                    para[iDie, 0] = waferSeq.ToString();                                                            //WAFER_SEQ
                    para[iDie, 1] = (iDie + 1).ToString();                                                          //DIE_NUM
                    para[iDie, 2] = "AVI";                                                                          //TEST_AREA
                    para[iDie, 3] = 0.ToString();                                                                   //DIEPROBE_CNT
                    para[iDie, 4] = oDiesList[iDie].IndexX.ToString();                                              //X
                    para[iDie, 5] = oDiesList[iDie].IndexY.ToString();                                              //Y
                    para[iDie, 6] = oDiesList[iDie].BinNumber.ToString();                                           //BinNumber
                    para[iDie, 7] = oDiesList[iDie].BinNumber.ToString();                                           //BIN_CHAR
                    para[iDie, 8] = oDiesList[iDie].ScopeImagePath == null ? "" : oDiesList[iDie].ScopeImagePath;   //IMAGE_PATH
                    para[iDie, 9] = "";                                                                             //THUMB_PATH
                    para[iDie, 10] = oDiesList[iDie].ScopeImage == null ? "" : oDiesList[iDie].ScopeImage;          //IMAGE_FILE_NAME
                    para[iDie, 11] = "";                                                                            //THUMB_FILENAME
                }

                oTest.DeleteFOIDiesMulti(waferSeq.ToString());

                if(oDiesList.Count > 0)
                    oTest.CreateFOIDiesMulti(para);

                para = new string[oTestImageList.Count, 8];
                for (int iImage = 0; iImage < oTestImageList.Count; iImage++)
                {
                    para[iImage, 0] = waferSeq.ToString();                                                                          //WAFER_SEQ
                    para[iImage, 1] = oTestImageList[iImage].IndexX;                                                                //X
                    para[iImage, 2] = oTestImageList[iImage].IndexY;                                                                //Y
                    para[iImage, 3] = oTestImageList[iImage].BinNumber.ToString();                                                  //BinNumber
                    para[iImage, 4] = oTestImageList[iImage].ServerImagePath == null ? "" : oTestImageList[iImage].ServerImagePath; //IMAGE_PATH
                    para[iImage, 5] = "";                                                                                           //THUMB_PATH
                    para[iImage, 6] = oTestImageList[iImage].LocalImageName == null ? "" : oTestImageList[iImage].LocalImageName;  //IMAGE_FILE_NAME
                    para[iImage, 7] = "";
                }

                oFOIImage.DeleteFOIImageList(waferSeq.ToString());

                if (oTestImageList.Count > 0)
                    oFOIImage.CreateFOIImagesMulti(para);

                //////////////////////////////////////////////////////////////////////////////////////////////
                //Step 05 : TQP_BINDESC 확인 생성
                //////////////////////////////////////////////////////////////////////////////////////////////
                //TQP_BINDESC 에 정의되지 않을 경우 임의로 생성 해준다.
                dtTemp = oTest.SelectFOIBinList(waferSeq);
                foreach (DataRow dr in dtTemp.Rows)
                    oTest.SetBinDESC_BinInfo(sPROGRAM, dr["BIN"].ToString(), dr["BIN_CHAR"].ToString(), "Scope Manual Create");

                //////////////////////////////////////////////////////////////////////////////////////////////
                //Step 06 : TQP_WAFER_SUM 확인 생성
                //////////////////////////////////////////////////////////////////////////////////////////////
                oTest.SetAVIWaferSummary(waferSeq, lotSeq, sWAFER_ID);

                //////////////////////////////////////////////////////////////////////////////////////////////
                //Step 07 : TQP_LOT_SUM 확인 생성
                //////////////////////////////////////////////////////////////////////////////////////////////
                oTest.SetAVILotSummary(lotSeq);
            }
            finally
            {
            }

        }

        /// <summary>
        /// Summary Data Update
        /// </summary>
        /// <param name="strLotID"></param>
        /// <param name="strWaferSeq"></param>
        /// <param name="strLotSeq"></param>
        public void SummaryUpdateLotStatus(string strLotID, string strWaferSeq, string strLotSeq)
        {
            DACrux.TEST.DSL.TQP_LOT_SUM oLot = new DACrux.TEST.DSL.TQP_LOT_SUM();
            DACrux.TEST.DSL.TQP_WAFER_SUM oWafer = new DACrux.TEST.DSL.TQP_WAFER_SUM();
            DataTable dtLot = GetLotStatusInfoToTable(strLotID);
            if (dtLot != null && dtLot.Rows.Count > 0)
            {
                string strCustomer = dtLot.Rows[0]["CUSTOMER"].ToString();
                string strDeviceAlias = dtLot.Rows[0]["MASK_ID"].ToString();

                if (string.IsNullOrEmpty(strCustomer) == false && string.IsNullOrEmpty(strDeviceAlias) == false)
                {
                    oLot.UpdateData("UPDATE_LOT_STS", null, new string[] { strCustomer, strDeviceAlias, strLotSeq });
                    oWafer.UpdateData("UPDATE_LOT_STS", null, new string[] { strCustomer, strDeviceAlias, strWaferSeq });
                }
            }
        }


        public void WaferConfigSeqUpdate(string wafer_seq, string config_seq)
        {
            DACrux.TEST.DSL.TQP_WAFER_SUM oWaferSum = new DACrux.TEST.DSL.TQP_WAFER_SUM();
            DACrux.TEST.DSL.TQP_WAFER oWafer = new DACrux.TEST.DSL.TQP_WAFER();

            oWaferSum.WaferConfigSeqUpdate(wafer_seq, config_seq);
            oWafer.WaferConfigSeqUpdate(wafer_seq, config_seq);
        }

        #endregion [ DMSMGR SUMMARY DATA ]

        /// <summary>
        /// TD 테이블의 데이터를 MERGE 구문으로 처리 합니다.
        /// </summary>
        public void MergeTdTableData(string factory, DataTable source)
        {
            if (source == null || source.Rows.Count == 0)
                return;

            // Wafer Seq 별 처리
            DataTable wafSeqDt = source.DefaultView.ToTable(true, "WAFER_SEQ");

            foreach (DataRow row in wafSeqDt.Rows)
            {
                string waferSeq = row[0].ToString();

                TQP_WAFER waf = new TQP_WAFER();
                string[] arr = waf.GetProgram(new string[] { waferSeq });

                if (arr == null || arr.Length == 0)
                    continue;

                // UPDATE를 위한 테이블 스키마 가져오기
                TD_TABLE td = new TD_TABLE();
                DataSet ds = td.GetTableSchema(factory, arr[0]);

                // 프로그램 테이블 단위로 처리
                foreach (DataTable dt in ds.Tables)
                {
                    // Row 갯수만큼 추가
                    for (int i = 0; i < source.Rows.Count; i++)
                        dt.Rows.Add(dt.NewRow());

                    // 필수 컬럼값 복사
                    foreach (string name in new string[] { "WAFER_SEQ", "DIE_NUM" })
                    {
                        for (int i = 0; i < source.Rows.Count; i++)
                            dt.Rows[i][name] = source.Rows[i][name];
                    }

                    // 컬럼 별 데이터 처리
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (source.Columns.Contains(col.ColumnName))
                        {
                            DataColumn sourceCol = source.Columns[col.ColumnName];

                            for (int i = 0; i < source.Rows.Count; i++)
                                dt.Rows[i][col] = source.Rows[i][sourceCol];
                        }
                    }

                    // 데이터에 대해 MERGE INTO 실행
                    TD_TABLE obj = new TD_TABLE();
                    obj.ExecuteMerge(dt);
                }
            }
        }

        /// <summary>
        /// AVI 테이블의 데이터를 MERGE 구문으로 처리 합니다.
        /// </summary>
        public void MergeAVITableData(string factory, DataTable source)
        {
            if (source == null || source.Rows.Count == 0)
                return;

            TQP_FOI_DIE oDie = new TQP_FOI_DIE();
            TQP_WAFER_SUM oWafer = new TQP_WAFER_SUM();

            for (int ir = 0; source.Rows.Count > ir; ir++)
            {
                DataTable dtWafer = oWafer.GetWaferInfo(long.Parse(source.Rows[ir]["WAFER_SEQ"].ToString()));
                if (dtWafer != null && dtWafer.Rows.Count > 0)
                {
                    oDie.UpdateAVIBinValue(source.Rows[ir]["BIN"].ToString(), source.Rows[ir]["WAFER_SEQ"].ToString(), source.Rows[ir]["DIE_NUM"].ToString());

                    //////////////////////////////////////////////////////////////////////////////////////////////
                    //TQP_WAFER_SUM 확인 생성
                    SetAVIWaferSummary(source.Rows[ir]["WAFER_SEQ"].ToString(), dtWafer.Rows[0]["LOT_SEQ"].ToString(), dtWafer.Rows[0]["WAFER_ID"].ToString());

                    //////////////////////////////////////////////////////////////////////////////////////////////
                    //TQP_LOT_SUM 확인 생성
                    SetAVILotSummary(dtWafer.Rows[0]["LOT_SEQ"].ToString());
                }
            }
        }

        /// <summary>
        /// 데이터 변경 이력 정보를 추가합니다.
        /// </summary>
        public void InsertChangeHis(string factory, string testArea, string product, string program, string lotID, string waferID,
            string tranUser, string tranUserIP, string category, string tranType, int tranCount, string prevValue, string currValue, string userComment)
        {
            TQP_CHANGE_HIS obj = new TQP_CHANGE_HIS();
            obj.InsertData(factory, testArea, product, program, lotID, waferID,
                tranUser, tranUserIP, category, tranType, tranCount, prevValue, currValue, userComment);
        }

        /// <summary>
        /// 데이터 변경 이력 데이터를 가져옵니다.
        /// </summary>
        public DataTable GetChangeHisData(string factory, string testArea, string fromDate, string toDate)
        {
            TQP_CHANGE_HIS obj = new TQP_CHANGE_HIS();
            return obj.GetData(factory, testArea, fromDate, toDate);
        }

        /// <summary>
        /// PCM 설비 상태를 가져옵니다.
        /// </summary>
        public DataTable GetPcmEquipStatus(string factory)
        {
            TQC_EQUIP obj = new TQC_EQUIP();
            return obj.GetPcmEquipStatus(factory);
        }

        /// <summary>
        /// Input Data 이력을 가져옵니다.
        /// </summary>
        public DataTable GetPcmInputDataHistory(string factory, string equipID, string fromDate, string toDate)
        {
            TQP_PCM_INPUT_HIS obj = new TQP_PCM_INPUT_HIS();
            return obj.GetData(factory, equipID, fromDate, toDate);
        }

        /// <summary>
        /// PCM Input 데이터를 추가 합니다.
        /// </summary>
        public void InsertPcmInputData(string factory, string equipID, string probeCard, string worker, string loginUser)
        {
            TQP_PCM_INPUT_HIS his = new TQP_PCM_INPUT_HIS();
            his.InsertData(factory, equipID, probeCard, worker, loginUser);

            // TQC_EQUIP 업데이트
            TQC_EQUIP obj = new TQC_EQUIP();
            obj.UpdateEquipData01(factory, equipID, probeCard, worker);
        }

        public PcmEquipInfo GetPcmEquipInfo(string factory, string equipID)
        {
            TQC_EQUIP obj = new TQC_EQUIP();
            DataTable dt = obj.GetEquipInfo(factory, equipID);

            if (dt == null || dt.Rows.Count == 0)
                return null;

            DataRow row = dt.Rows[0];

            return new PcmEquipInfo()
            {
                Factory = factory,
                EquipID = equipID,
                Status = row["EQUIP_STS_1"].ToString(),
                LotID = row["EQUIP_STS_3"].ToString(),
                WaferID = row["EQUIP_STS_4"].ToString(),
                ProbeCard = row["EQUIP_STS_5"].ToString(),
                Operator = row["EQUIP_STS_6"].ToString(),
                Program = row["EQUIP_STS_7"].ToString(),
                ParsingWaferID = row["EQUIP_STS_8"].ToString(),
                ParsingEndTime = row["EQUIP_STS_9"].ToString()
            };
        }

        public void UpdatePcmEquipInfo(PcmEquipInfo info)
        {
            TQC_EQUIP obj = new TQC_EQUIP();

            obj.UpdateEquipData02(
                info.Factory,
                info.EquipID,
                info.Status,
                info.LotID,
                info.WaferID);
        }

        public void UpdatePcmEquipInfo(PcmEquipInfoList list)
        {
            if (list == null || list.Count == 0)
                return;

            string[,] arr = new string[list.Count, Enum.GetNames(typeof(PcmEquipCol)).Length];

            for (int i = 0; i < list.Count; i++)
            {
                arr[i, (int)PcmEquipCol.Factory] = list[i].Factory;
                arr[i, (int)PcmEquipCol.EquipID] = list[i].EquipID;
                arr[i, (int)PcmEquipCol.Status] = list[i].Status;
                arr[i, (int)PcmEquipCol.LotID] = list[i].LotID;
                arr[i, (int)PcmEquipCol.WaferID] = list[i].WaferID;
                arr[i, (int)PcmEquipCol.Program] = list[i].Program;
            }

            TQC_EQUIP obj = new TQC_EQUIP();
            obj.UpdateEquipData02(arr);
        }

        public void UpdateParsingInfo(
            string factory,
            string equipID,
            string equip_sts_8
            )
        {
            TQC_EQUIP obj = new TQC_EQUIP();
            /// equip_sts_9 컬럼은 Parsing이 완료된 시간을 업데이트 한다.
            obj.UpdateEquipData04(factory, equipID, equip_sts_8);
        }

        #region Test DM Wafer Map Matching
        public System.Data.DataSet SelectWaferMapBasic(long WaferSeq)
        {
            DataSet dsData = null;
            DataTable dtTemp = null;

            string strTestArea = string.Empty;
            string strProduct = string.Empty;
            string strProgram = string.Empty;

            try
            {
                DACrux.TEST.DSL.TQP_PROGRAM oProgram = new DACrux.TEST.DSL.TQP_PROGRAM();
                DACrux.TEST.DSL.TQP_WAFER_SUM oWafer = new DACrux.TEST.DSL.TQP_WAFER_SUM();
                DACrux.TEST.DSL.TQP_BINDESC oBin = new DACrux.TEST.DSL.TQP_BINDESC();
                DACrux.TEST.DSL.TQP_MAPDEF oMapDef = new DACrux.TEST.DSL.TQP_MAPDEF();
                DACrux.TEST.DSL.TQP_FOI_DIE oMapFoi = new DACrux.TEST.DSL.TQP_FOI_DIE();

                dsData = new DataSet();

                //TQP_WAFER_SUM Table 상에서 기준 정보를 불러 온다.
                dtTemp = oWafer.GetWaferLotInfo(WaferSeq);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("Wafer Seq 에 대한 정보가 없습니다.");

                strTestArea = dtTemp.Rows[0]["TESTAREA"].ToString();
                strProduct = dtTemp.Rows[0]["DEVICE_ALIAS"].ToString();
                strProgram = dtTemp.Rows[0]["PROGRAM"].ToString();

                dtTemp.TableName = "WAFER_INFO";
                dsData.Tables.Add(dtTemp.Copy());

                //===============================================================================================

                //AVI 와 이외의 공정의 Data Map 을 가져오는 방식이 다르다.
                if (strTestArea == "AVI")
                {
                    dtTemp = oMapFoi.SelectFOIMap(WaferSeq.ToString(), "0");
                    if (dtTemp == null || dtTemp.Rows.Count <= 0)
                        throw new Exception("TEST Map Data를 불러오지 못했습니다.");
                }
                else
                {
                    // Table 및 Colums 정보를 기준으로 Test Data 를 가져온다.
                    dtTemp = oProgram.GetData("GET_MAP_TABLES", null, new string[] { strProgram });
                    if (dtTemp == null || dtTemp.Rows.Count <= 0)
                        throw new Exception("해당 Program 에 해당되는 Column 정보가 없습니다.");

                    StringBuilder sQuery = new StringBuilder();
                    Dictionary<string, string> dicAlias = new Dictionary<string, string>();

                    string[] strTable = new string[dtTemp.Rows.Count];

                    for (int it = 0; it < dtTemp.Rows.Count; it++)
                    {
                        strTable[it] = string.Format("{0} {1}", dtTemp.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTemp.Rows[it]["RN"].ToString())]);
                        dicAlias.Add(dtTemp.Rows[it]["TABLE_NAME"].ToString(), m_tbl[DACrux.Base.Convert.intParse(dtTemp.Rows[it]["RN"].ToString())]);
                    }

                    sQuery.AppendLine(string.Join(",", strTable));
                    sQuery.AppendLine(" WHERE 1 = 1 ");

                    int ic = 0;
                    string[] arrTemp = new string[2];
                    foreach (KeyValuePair<string, string> var in dicAlias)
                    {
                        if (ic == 0)
                        {
                            sQuery.AppendLine(string.Format(" AND T1.WAFER_SEQ = {0}.WAFER_SEQ", var.Value));
                            sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ = {1} ", var.Value, WaferSeq));
                        }
                        else
                        {
                            sQuery.AppendLine(string.Format(" AND {0}.WAFER_SEQ = {1}.WAFER_SEQ ", arrTemp[1], var.Value));
                            sQuery.AppendLine(string.Format(" AND {0}.DIE_NUM = {1}.DIE_NUM ", arrTemp[1], var.Value));
                        }
                        arrTemp[0] = var.Key;
                        arrTemp[1] = var.Value;
                        ic++;
                    }

                    //===============================================================================================

                    dtTemp = oProgram.GetData("GET_BIN_SUMMARY", new string[] { sQuery.ToString() }, new string[] { strProgram });
                    if (dtTemp != null && dtTemp.Rows.Count > 0)
                    {
                        dtTemp.TableName = "BINSUM";
                        dsData.Tables.Add(dtTemp.Copy());
                    }

                    dtTemp = oProgram.GetData("GET_DRAW_TEST_MAP_DATA", new string[] { "", sQuery.ToString() }, null);
                    if (dtTemp == null || dtTemp.Rows.Count <= 0)
                        throw new Exception("TEST Map Data를 불러오지 못했습니다.");
                }

                dtTemp.TableName = "MAPDATA";
                dsData.Tables.Add(dtTemp.Copy());

                //===============================================================================================
                //Map 에 대한 Recipe 정보를 불러 온다.

                dtTemp = oMapDef.GetData("GET_MAPDEF", null, new string[] { strProduct });
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("Shot Map 정보가 없습니다.");

                //Map Direction 은 LeftBottom 고정이다.
                dtTemp.Rows[0]["XY_DIRECTION"] = 1;
                dtTemp.AcceptChanges();

                dtTemp.TableName = "RECIPE";
                dsData.Tables.Add(dtTemp.Copy());

                //===============================================================================================

                //Bin 정보
                dtTemp = oBin.GetData("SELECT_BIN_DATA", null, new string[] { strProgram });
                dtTemp.TableName = "MASTER_BIN";
                dsData.Tables.Add(dtTemp.Copy());

                //===============================================================================================

                if (strTestArea != "PARAMETRIC" && strTestArea != "INLINE")
                {
                    dtTemp = oMapDef.GetData("GET_MAPDEF", null, new string[] { strProduct });
                    dtTemp.TableName = "SHOT_DEF";
                    dsData.Tables.Add(dtTemp.Copy());

                    dtTemp = oMapDef.GetData("GET_MAPDEF_DIE", null, new string[] { strProduct });
                    dtTemp.TableName = "MARKDATA";
                    dsData.Tables.Add(dtTemp.Copy());
                }

                //===============================================================================================

                return dsData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dtTemp != null)
                    dtTemp.Dispose();

                if (dsData != null)
                    dsData.Dispose();
            }
        }

        public System.Data.DataSet GetMatchDefectData(string strLotID, string strSlotID, string strUser)
        {
            DataSet dsData = null;
            DataTable dtTemp = null;

            string strSetupSeq = string.Empty;

            List<string> arrStepSeq;
            List<string> arrSetupSeq;
            List<string> arrTest;

            DACrux.SEMDMS.DSL.TQD_INSP_INFO oInspInfo = null;
            DACrux.SEMDMS.DSL.TQD_SETUP oSetup = null;
            DACrux.SEMDMS.DSL.TQD_DEFECT oDefect = null;
            DACrux.SEMDMS.DSL.TQD_COLORBYSIZE oTQD_COLORBYSIZE = null;
            DACrux.SEMDMS.DSL.TQD_DEFECT_TYPE oDefectType = null;

            try
            {
                arrStepSeq = new List<string>();
                arrSetupSeq = new List<string>();
                arrTest = new List<string>();
                dsData = new DataSet();
                oInspInfo = new DACrux.SEMDMS.DSL.TQD_INSP_INFO();
                oSetup = new DACrux.SEMDMS.DSL.TQD_SETUP();
                oDefect = new DACrux.SEMDMS.DSL.TQD_DEFECT();
                oTQD_COLORBYSIZE = new DACrux.SEMDMS.DSL.TQD_COLORBYSIZE();
                oDefectType = new DACrux.SEMDMS.DSL.TQD_DEFECT_TYPE();


                //=================================================================================================================================
                //Step Seq 정보를 가져 온다.
                //=================================================================================================================================
                dtTemp = oInspInfo.GetData("SELECT_MATCH_WAFER", null, new string[] { strLotID, strSlotID });
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception(string.Format("DM 상에 Wafer 정보가 없습니다. Lot ID : {0}, Slot : {1}", strLotID, strSlotID));

                dtTemp.TableName = "STEP_INFO";
                dsData.Tables.Add(dtTemp.Copy());

                for (int ir = 0; ir < dtTemp.Rows.Count; ir++)
                {
                    if (arrSetupSeq.IndexOf(dtTemp.Rows[ir]["SETUP_SEQ"].ToString()) < 0)
                        arrSetupSeq.Add(dtTemp.Rows[ir]["SETUP_SEQ"].ToString());

                    if (arrTest.IndexOf(dtTemp.Rows[ir]["TEST"].ToString()) < 0)
                        arrTest.Add(dtTemp.Rows[ir]["TEST"].ToString());

                    if (arrStepSeq.IndexOf(dtTemp.Rows[ir]["STEP_SEQ"].ToString()) < 0)
                        arrStepSeq.Add(dtTemp.Rows[ir]["STEP_SEQ"].ToString());

                    if (ir == 0)
                        strSetupSeq = dtTemp.Rows[ir]["SETUP_SEQ"].ToString();
                }

                //=================================================================================================================================
                //Setup Seq 정보를 가져 온다.
                //=================================================================================================================================
                dtTemp = oSetup.GetData("GET_SETUP", null, new string[] { strSetupSeq });
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("정의된 Setup 정보가 없습니다. TQD_SETUP Empty");

                dtTemp.TableName = "SETUP_INFO";
                dsData.Tables.Add(dtTemp.Copy());

                //=================================================================================================================================
                //Defect Map 정보를 가져 온다.
                //=================================================================================================================================
                //dtTemp = oDefect.GetData("SELECT_DEFECT_MULTI", new string[] { string.Join(",", arrStepSeq.ToArray()) }, null);
                //if (dtTemp == null)
                //    throw new Exception("정의된 Defect 정보가 없습니다. TQD_DEFECT Empty");

                //dtTemp.TableName = "DEFECT_INFO";
                //dsData.Tables.Add(dtTemp.Copy());

                dtTemp = oTQD_COLORBYSIZE.SelectColorListByDefectSize(strUser);
                if (dtTemp != null && dtTemp.Rows.Count > 0)
                {
                    dtTemp.TableName = "COLOR_SIZE";
                    dsData.Tables.Add(dtTemp.Copy());
                }

                dtTemp = oDefectType.GetData("SELECT_DEFECT_COLOR", null, null);
                if (dtTemp != null && dtTemp.Rows.Count > 0)
                {
                    dtTemp.TableName = "DEFECT_COLOR";
                    dsData.Tables.Add(dtTemp.Copy());
                }

                return dsData;
            }
            finally
            {
                if (dtTemp != null)
                    dtTemp.Dispose();

                if (dsData != null)
                    dsData.Dispose();
            }
        }

        /// <summary>
        /// TQC_LOSS_YIELD_SUM 생성
        /// </summary>
        /// <param name="strWaferSeq"></param>
        public void InsertMatchSum(string strWaferSeq)
        {
            DACrux.SEMDMS.DSL.TQC_LOSS_YIELD_SUM oLossSum = null;

            try
            {
                oLossSum = new SEMDMS.DSL.TQC_LOSS_YIELD_SUM();

                oLossSum.DeleteMatchSum(strWaferSeq);

                oLossSum.InsertMatchSum(strWaferSeq);

            }
            finally
            {
            }
        }

        #endregion Test DM Wafer Map Matching
    }

    public enum PcmEquipCol
    {
        Factory,
        EquipID,
        Status,
        LotID,
        WaferID,
        Program
    }

    public class PcmEquipInfoList : List<PcmEquipInfo>
    {
        public override string ToString()
        {
            if (Count == 0)
                return "(empty)";

            StringBuilder sb = new StringBuilder();
            sb.Append("(");

            for (int i = 0; i < Count; i++)
            {
                sb.Append(this[i].EquipID);

                if (i < Count - 1)
                    sb.Append(",");
            }

            sb.Append(")");

            return sb.ToString();
        }
    }

    public class PcmEquipInfo
    {
        public string Factory { get; set; }
        public string EquipID { get; set; }
        public string Status { get; set; }
        public string LotID { get; set; }
        public string WaferID { get; set; }
        public string ProbeCard { get; set; }
        public string Operator { get; set; }
        public string Program { get; set; }
        public string ParsingWaferID { get; set; }
        public string ParsingEndTime { get; set; }

        public override string ToString()
        {
            return EquipID;
        }
    }
}
