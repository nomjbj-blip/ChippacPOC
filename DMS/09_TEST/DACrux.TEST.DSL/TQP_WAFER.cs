using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
//using System.Transactions;
using Miracom.Middleware;
using System.Transactions;

namespace DACrux.TEST.DSL
{
    public class TQP_WAFER : Miracom.Middleware.QueryComponent
    {
        string connectID = string.Empty;
        public TQP_WAFER()
        {
            connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];
            if (string.IsNullOrEmpty(connectID))
            {
                throw new Exception("The connect ID nothing. Please, check app.config.");
            }

            this.InitQueryComponent(connectID, "TQP_WAFER.xml");
        }

        #region [Common Select]
        public DataTable GetData(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                return this.GetDataTable(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [Common Insert]
        public void InsertData(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                this.GetDataTable(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertDataNonQuery(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                return this.ExecuteNonQuery(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertExecuteMultiple(string sqlName, string[,] paras)
        {
            try
            {
                this.ExecuteMultiple(sqlName, null, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [Common Update]
        public void UpdateData(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                this.GetDataTable(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateDataNonQuery(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                return this.ExecuteNonQuery(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [Common Delete]
        public void DeleteData(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                this.GetDataTable(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteDataNonQuery(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                return this.ExecuteNonQuery(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// 주어진 Wafer Seq에 의해 Wafer 정보를 조회한다.
        /// LOT_SEQ, PRODUCT, TESTAREA, PROGRAM, LOT_ID, MOTHER_LOT_ID, START_TIME, END_TIME, WIP_STATUS, WAFERS, WAFER_SEQ, LOT_SEQ_1, WAFER_ID, TESTER, OPERATOR,  PROBE_CNT, TESTED_DIE, START_TIME_1, END_TIME_1, WAFER_CAT, LOSS_DIE, ISP_INITEM, VISUAL_ITEM, ISP_OUTITEM
        /// </summary>
        /// <param name="WaferSeq">WaferSeq</param>
        /// <returns>LOT_SEQ, PRODUCT, TESTAREA, PROGRAM, LOT_ID, MOTHER_LOT_ID, START_TIME, END_TIME, WIP_STATUS, WAFERS, WAFER_SEQ, LOT_SEQ_1, WAFER_ID, TESTER, OPERATOR,  PROBE_CNT, TESTED_DIE, START_TIME_1, END_TIME_1, WAFER_CAT, LOSS_DIE, ISP_INITEM, VISUAL_ITEM, ISP_OUTITEM</returns>
        //public DataTable GetWaferInfo(long WaferSeq)
        //{
        //    return this.GetDataTable("GET_WAFER_INFO", null, new string[] { WaferSeq.ToString() });
        //}

        public DataTable GetWaferInfoByWSEQ(
            string waferSeq
            )
        {
            return this.GetDataTable(
                "GET_WAFER_INFO [WSQ]",
                null,
                new string[] { waferSeq }
                );
        }

        public DataTable GetWaferInfoByLSEQ(
            string lotSeq
            )
        {
            return this.GetDataTable(
                "GET_WAFER_INFO [LSQ]",
                null,
                new string[] { lotSeq }
                );
        }

        public DataTable GetWaferSummaryInfo(
            string lotSeq,
            decimal waferSeq
            )
        {
            return this.GetDataTable(
                "GET_WAFER_SUMMARY_INFO [WSEQ]",
                null,
                new string[] { lotSeq, waferSeq.ToString() }
                );
        }

        /// <summary>
        /// 주어진 Wafer ID 에 의해 Wafer 정보를 조회한다.
        /// LOT_SEQ, PRODUCT, TESTAREA, PROGRAM, LOT_ID, MOTHER_LOT_ID, START_TIME, END_TIME, WIP_STATUS, WAFERS, WAFER_SEQ, LOT_SEQ_1, WAFER_ID, TESTER, OPERATOR,  PROBE_CNT, TESTED_DIE, START_TIME_1, END_TIME_1, WAFER_CAT, LOSS_DIE, ISP_INITEM, VISUAL_ITEM, ISP_OUTITEM
        /// </summary>
        /// <param name="WaferSeq">WaferSeq</param>
        /// <returns>LOT_SEQ, PRODUCT, TESTAREA, PROGRAM, LOT_ID, MOTHER_LOT_ID, START_TIME, END_TIME, WIP_STATUS, WAFERS, WAFER_SEQ, LOT_SEQ_1, WAFER_ID, TESTER, OPERATOR,  PROBE_CNT, TESTED_DIE, START_TIME_1, END_TIME_1, WAFER_CAT, LOSS_DIE, ISP_INITEM, VISUAL_ITEM, ISP_OUTITEM</returns>
        //public DataTable GetWaferInfo(string Waferid)
        //{
        //    return this.GetDataTable("GET_WAFER_INFO [WID]", null, new string[] { Waferid });
        //}

        public DataTable GetWaferList(DateTime stime, DateTime etime, string fieldOrder)
        {
            return this.GetDataTable(
                "WAFER_LIST",
                new string[] { fieldOrder },
                new string[] { stime.ToString("yyyy-MM-dd"), etime.ToString("yyyy-MM-dd") }
                );
        }

        public void CopyWafer(string LotSeq, string STime, string ETime, string strLossDie, string PreWaferSeq)
        {
            try
            {
                this.Execute("COPY_WAFER", null, new string[] { LotSeq, STime, ETime, strLossDie, PreWaferSeq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public DataTable GetStandardPCMReport(string PROGRAM, string WAFERSEQ)
        //{

        //    DataTable dtpara = null;
        //    DataTable dt = null;

        //    DataTable dtRet = null;

        //    double Range = 0.0d;
        //    double Gap = 0.0d;
        //    int Pos = 0;
        //    try
        //    {
        //        TQP_PARASPEC oParaSpec = new TQP_PARASPEC();
        //        dtpara = oParaSpec.GetParaSpec(PROGRAM);

        //        DataRow[] drs = dtpara.Select();

        //        foreach (DataRow dr in drs)
        //        {
        //            dt = this.GetDataTable("GET_STDREPORT"
        //                    , new string[]{utilParametric.REP( dr["ITEM"].ToString().ToUpper() ),
        //                        utilParametric.REP( dr["ITEM"].ToString().ToUpper() ),
        //                        utilParametric.REP( dr["ITEM"].ToString().ToUpper() ),
        //                        utilParametric.REP( dr["ITEM"].ToString().ToUpper() ),
        //                        PROGRAM,
        //                        utilParametric.REP( dr["ITEM"].ToString().ToUpper() ),
        //                                     PROGRAM,
        //                        utilParametric.REP( dr["ITEM"].ToString().ToUpper() ),
        //                                     PROGRAM,
        //                        utilParametric.REP( dr["ITEM"].ToString().ToUpper() ),
        //                                     PROGRAM,
        //                        utilParametric.REP( dr["ITEM"].ToString().ToUpper() ),
        //                                     PROGRAM,
        //                        utilParametric.REP( dr["ITEM"].ToString().ToUpper() ),
        //                                 PROGRAM}
        //                , new string[] { PROGRAM, dr["ITEM"].ToString(), WAFERSEQ });

        //            if (dtRet == null)
        //            {
        //                dtRet = dt.Copy();
        //            }
        //            else
        //            {
        //                if (dt != null && dt.Rows.Count > 0)
        //                {
        //                    Range = DACrux.Base.Convert.doubleParse(dt.Rows[0]["USL"].ToString()) - DACrux.Base.Convert.doubleParse(dt.Rows[0]["LSL"].ToString());
        //                    Gap = Range / 20.0;
        //                    Pos = (int)((DACrux.Base.Convert.doubleParse(dt.Rows[0]["AVGV"].ToString()) - DACrux.Base.Convert.doubleParse(dt.Rows[0]["LSL"].ToString())) / Gap);

        //                    if (Pos > 0)
        //                    {
        //                        //dt.Rows[0]["VALUEPOS"] = string.Format("{0}O{1}", "<".PadRight(Pos,'-'),">".PadLeft(20-Pos,'-'));
        //                        dt.Rows[0]["VALUEPOS"] = "                          ";
        //                        dt.AcceptChanges();
        //                    }
        //                    dtRet.Rows.Add(dt.Rows[0].ItemArray);
        //                }
        //            }
        //        }

        //        return dtRet;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (dt != null) dt.Dispose();
        //    }
        //}

        public DataTable GetMaintWaferList(string LOTSEQ, string PROBE_CNT)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_MAINT_WAFER_LIST", new string[] { LOTSEQ, PROBE_CNT }, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteWaferData(string WaferSeq)
        {
            try
            {
                this.Execute("DELETE_WAFER_DATA", null, new string[] { WaferSeq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteWaferDataAVI(string strFactory, string strLotSeq, string strWaferID)
        {
            try
            {
                this.Execute("DELETE_WAFER_AVI", null, new string[] { strFactory, strLotSeq, strWaferID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateProbeCount(string WaferSeq, string ProbeCnt)
        {
            try
            {
                this.Execute("UPDATE_PROBE_CNT", null, new string[] { ProbeCnt, WaferSeq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetMapWaferList(string LOTSEQ, string PROBE_CNT)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_MAP_WAFER_LIST", new string[] { LOTSEQ }, new string[] { PROBE_CNT });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Map Parser에서 쓰이는 DSL---------------------------------------------------

        public DataTable GetIsWafer(string Lot_Seq, String WaferID)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_ISWAFER", null, new string[] { Lot_Seq, WaferID });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        public DataTable IsFirst(string Wafer_Seq, string StartTime, string EndTime)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("IS_FIRST", null, new string[] { Wafer_Seq, StartTime, EndTime });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        public DataTable SelectTestdie(string Lot_Seq, string WaferID)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_TESTDIE", null, new string[] { Lot_Seq, WaferID });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        public void UpdateProbeCnt(string Lot_Seq, string WaferID)
        {
            try
            {
                this.Execute("UPDATE_PROBE_CNT", null, new string[] { Lot_Seq, WaferID });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateWafer(string Wafer_Seq
                                    , string Lot_Seq
                                    , string WaferID
                                    , string TesterID
                                    , string ProberCard
                                    , string Operator
                                    , string ProbeCnt
                                    , string TestedDie
                                    , string StartTime
                                    , string EndTime
                                    , string WaferCat
                                    , string FailDies
                                    , string IspInitem
                                    , string VisualItem
                                    , string IspOutitem
                                    , string IspIncmt
                                    , string VisualCmt
                                    , string IspOutcmt
                                    , string FviFlag)
        {
            try
            {
                this.Execute("CREATE_WAFER", null, new string[] {Wafer_Seq
                                                                , Lot_Seq
                                                                , WaferID
                                                                , TesterID
                                                                , ProberCard
                                                                , Operator
                                                                , ProbeCnt
                                                                , TestedDie
                                                                , StartTime
                                                                , EndTime
                                                                , WaferCat
                                                                , FailDies
                                                                , IspInitem
                                                                , VisualItem
                                                                , IspOutitem
                                                                , IspIncmt
                                                                , VisualCmt
                                                                , IspOutcmt
                                                                , FviFlag});
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferCount(String Lot_Seq)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_WAFERCOUNT", null, new string[] { Lot_Seq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
            return dt;
        }

        public DataTable GetStartTime(String Lot_Seq)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_STARTTIME", null, new string[] { Lot_Seq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
            return dt;
        }

        public DataTable GetEndTime(String Lot_Seq)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_ENDTIME", null, new string[] { Lot_Seq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
            return dt;
        }

        public void UpdateLossDie(String Wafer_Seq, string LossDie)
        {
            try
            {
                this.Execute("UPDATE_LOSSDIE", null, new string[] { Wafer_Seq, LossDie });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateTestDie(String Wafer_Seq, string TestDie)
        {
            try
            {
                this.Execute("UPDATE_TESTDIE", null, new string[] { Wafer_Seq, TestDie });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SelectNextWaferSeq()
        {
            try
            {
                return ExecuteScalar("SELECT_NEXT_WAFERSEQ", null, null).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion--------------------------------------------------------------------------


        /// <summary>
        /// YSIM 
        /// 2015-04-16
        /// Wafer Creation Function
        /// </summary>
        /// <param name="lLotSeq">LOT_SEQ [Key : TESTAREA,PRODUCT,PROGRAM]</param>
        /// <param name="WaferID">Unique Wafer ID</param>
        /// <param name="Slot">Slot Number [Default : -1]</param>
        /// <returns>Wafer Seq</returns>
        public long CreateWafer(string strFactory, long lLotSeq, string WaferID, int Slot = -1)
        {
            DataTable dt = null;
            long lWaferSeq = -1;
            try
            {
                /// 1. Slot Number 가져옴
                /// 
                if (Slot == -1)
                    int.TryParse(WaferID.Substring(WaferID.Length - 2), out Slot);

                /// 3. Probe Count 정리
                /// ==> 기존의 Wafer에 대해 Probe Count 1씩 증가
                this.ExecuteNonQuery("UPDATE_PROBE_CNT", null, new string[] { lLotSeq.ToString(), WaferID });

                /// 3. Wafer를 Creation 함
                /// ==> 신규는 Probe Count = 0
                this.ExecuteNonQuery("CREATE_WAFER_02", null, new string[] { strFactory, lLotSeq.ToString(), WaferID });

                /// 4. 신규로 만들어진 Wafer Seq를 가져옴
                dt = this.GetWaferInfo(lLotSeq, WaferID, 0);
                if (dt == null || dt.Rows.Count == 0) throw new Exception("TQP_WAFER.Createwafer.Step:04.1");
                if (long.TryParse(dt.Rows[0]["WAFER_SEQ"].ToString(), out lWaferSeq) == false) throw new Exception("TQP_WAFER.Createwafer.Step:04.2");

                return lWaferSeq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferInfo(long lLotSeq, string WaferID, int ProbeCnt = 0)
        {
            try
            {
                return this.GetDataTable("SELECT_WAFER_INFO_01", null, new string[] { lLotSeq.ToString(), WaferID, ProbeCnt.ToString() });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// PROBE_CNT 값을 가져옵니다. START_TIME 기준으로 신규인 경우 새로운 PROBE_CNT 값을, 기존에 존재하는 경우 해당 PROBE_CNT 값을 리턴합니다.
        /// </summary>
        public DataTable GetWaferInfo(string factory, string lotSeq, string waferID, string startTime)
        {
            return GetDataTable("SELECT_WAFER_INFO_03", null, new string[] { factory, lotSeq, waferID, startTime });
        }

        //--

        public DataTable GetWaferInfo(
            string factory,
            string lotid
            )
        {
            return this.GetDataTable(
                "SELECT_WAFER_INFO_05",
                null,
                new string[] { factory, lotid }
                );
        }

        //--

        public DataTable GetWaferInfo(
            string factory,
            string lotid,
            string[] waferid,
            string product,
            string mapid
            )
        {
            return this.GetDataTable(
                "SELECT_WAFER_INFO_06",
                new string[] { string.Format("'{0}'", string.Join("','", waferid)) },
                new string[] { factory, lotid, product, mapid }
                );
        }


        public DataTable GetWaferInfo07(
            string dtStart,
            string dtEnd,
            string[] testarea,
            string product,
            string program,
            string[] lots
            )
        {
            return this.GetDataTable(
                "SELECT_WAFER_INFO_07", 
                new string[] { string.Format("'{0}'", string.Join("','", testarea)), string.Format("'{0}'", string.Join("','", lots)) }, 
                new string[] { dtStart, dtEnd, product, program }
                );
        }

        public DataTable GetWaferInfo08(
            string[] testarea,
            string product,
            string program,
            string[] lots
            )
        {
            return this.GetDataTable(
                "SELECT_WAFER_INFO_08", 
                new string[] { string.Format("'{0}'", string.Join("','", testarea)), string.Format("'{0}'", string.Join("','", lots)) }, 
                new string[] { product, program }
                );
        }


        //--

        /// <summary>
        /// Scope 관련 Wafer 정보를 가져 온다.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="lotSeq"></param>
        /// <param name="waferID"></param>
        /// <returns></returns>
        public DataTable GetWaferInfo(string factory, string lotSeq, string waferID)
        {
            return GetDataTable("SELECT_WAFER_INFO_04", null, new string[] { factory, lotSeq, waferID });
        }

        /// <summary>
        /// FACTORY, WAFER_SEQ
        /// </summary>
        public void MergeData(
            string FACTORY,
            string WAFER_SEQ,
            string PROBE_CNT,
            string LOT_SEQ,
            string WAFER_ID,
            string START_TIME,
            string END_TIME,
            string TESTER,
            string PROBER,
            string PROBE_CARD,
            string OPERATOR,
            string PROGRAM,
            string PROGRAM_REV,
            string START_QTY,
            string VENDOR_ID,
            string MAX_X,
            string MAX_Y,
            string WAF_FLAT,
            string COMMENTS,
            string SCRIBE_LOT,
            string TEMP,
            string OVERDRIVE,
            string PGMPATH,
            string DATA_SOURCE,
            string MAXSITE,
            string TFVERSION,
            string PROBECARD_TOUCHDOWNS,
            string SMSFAMILY_NAME,
            string NUM_DIE
            )
        {
            ExecuteNonQuery("MERGE_DATA", null, new string[]
            {
                FACTORY,
                WAFER_SEQ,
                PROBE_CNT,
                LOT_SEQ,
                WAFER_ID,
                START_TIME,
                END_TIME,
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
                SCRIBE_LOT,
                TEMP,
                OVERDRIVE,
                PGMPATH,
                DATA_SOURCE,
                MAXSITE,
                TFVERSION,
                PROBECARD_TOUCHDOWNS,
                SMSFAMILY_NAME,
                NUM_DIE
            });
        }

        public void IncreaseProbeCnt(string FACTORY, string LOT_SEQ, string WAFER_ID)
        {
            ExecuteNonQuery("INCREASE_PROBE_CNT", null, new string[] { FACTORY, LOT_SEQ, WAFER_ID });
        }

        /// <summary>
        /// 해당 LOT의 WAFER 리스트를 가져옵니다.
        /// </summary>
        public DataTable GetConditionWafer(string program, string[] lotSeqArr)
        {
            if (lotSeqArr == null || lotSeqArr.Length == 0)
                return null;

            string lotId = String.Format("'{0}'", String.Join("','", lotSeqArr));
            return GetDataTable("SELECT_CONDITION_WAFER", new string[] { lotId }, new string[] { program });
        }

        /// <summary>
        /// 해당 WAFER의 PROGRAM 리스트를 가져옵니다.
        /// </summary>
        public string GetProgram(string waferSeq)
        {
            object obj = ExecuteScalar("SELECT_DISTINCT_PROGRAM", new string[] { waferSeq }, null);

            if (obj == null)
                return null;

            return obj.ToString();
        }

        /// <summary>
        /// 해당 WAFER의 PROGRAM 리스트를 가져옵니다.
        /// </summary>
        public string[] GetProgram(string[] waferSeqArr)
        {
            if (waferSeqArr == null || waferSeqArr.Length == 0)
                return null;

            string waferSeq = String.Join(",", waferSeqArr);

            DataTable dt = GetDataTable("SELECT_DISTINCT_PROGRAM", new string[] { waferSeq }, null);

            if (dt == null | dt.Rows.Count == 0)
                return null;

            string[] arr = new string[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
                arr[i] = dt.Rows[i][0].ToString();

            return arr;
        }

        /// <summary>
        /// WAFER ID를 업데이트 합니다.
        /// </summary>
        public void UpdateWaferID(string waferSeq, string waferID, int probeCnt)
        {
            ExecuteNonQuery("UPDATE_WAFER_ID", null, new string[] { waferSeq, waferID, probeCnt.ToString() });
        }

        public string[] GetWaferSeqByLot(string lotSeq, string program)
        {
            DataTable dt = GetDataTable("SELECT_WAFER_BY_LOT", null, new string[] { lotSeq, program });

            if (dt == null | dt.Rows.Count == 0)
                return null;

            string[] arr = new string[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
                arr[i] = dt.Rows[i][0].ToString();

            return arr;
        }

        public void UpdateLotID(string lotSeq, string lotID)
        {
            ExecuteNonQuery("UPDATE_LOT_ID", null, new string[] { lotSeq, lotID });
        }

        public void DeleteLotData(string lotSeq)
        {
            ExecuteNonQuery("DELETE_LOT_DATA", null, new string[] { lotSeq });
        }

        public DataRow GetWaferInfo(string waferSeq)
        {
            DataTable dt = GetDataTable("SELECT_WAFER_INFO_09", null, new string[] { waferSeq });

            if (dt == null || dt.Rows.Count == 0)
                throw new Exception(String.Format("Not found Wafer info.(wafer_seq = {0})", waferSeq));

            return dt.Rows[0];
        }

        /// <summary>
        /// PROGRAM 정보를 업데이트 합니다.
        /// </summary>
        public void UpdateProgram(string waferSeq, string lotSeq, string newProgram, int probeCnt)
        {
            ExecuteNonQuery("UPDATE_PROGRAM", null, new string[] { waferSeq, lotSeq, newProgram, probeCnt.ToString() });
        }

        public void IncreaseProbeCount(string lotSeq, string waferSeq)
        {
            ExecuteNonQuery("INCREASE_PROBE_COUNT", null, new string[] { lotSeq, waferSeq });
        }

        public void WaferConfigSeqUpdate(string wafer_seq, string config_seq)
        {
            ExecuteNonQuery("UPDATE_CONFIG_SEQ", null, new string[] { config_seq, wafer_seq });
        }
    }
}
