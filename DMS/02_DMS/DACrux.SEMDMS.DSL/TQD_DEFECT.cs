
using System;
using System.IO;
using System.Data;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Diagnostics;
using System.Collections.Generic;

namespace DACrux.SEMDMS.DSL
{
    public class TQD_DEFECT : Miracom.Middleware.QueryComponent
    {
        public TQD_DEFECT()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["DMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQD_DEFECT.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        public List<object[]> GetDataToArrayList(string sqlName, string[] dynamic, string[] paras)
        {
            return base.GetDataToArrayList(sqlName, dynamic, paras);
        }

        public object[,] GetDataToArray(string sqlName, string[] dynamic, string[] paras)
        {
            return base.GetDataToObjectArray(sqlName, dynamic, paras);
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

        public int UpdateDataNonQuery(
            string sqlName,
            string[,] Params
            )
        {
            return this.ExecuteMultiple(
                sqlName,
                Params
                );

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

        public void Create(string[] DefectInfo)
        {
            DateTime dtLoggingTime = DateTime.Now;
            //성능로그시작
            this.TraceStartPoint();
            try
            {
                //업무 로직 구현
                this.Execute("CREATE_DEFECT", null, DefectInfo);
                //ContextUtil.SetComplete();
                //Tx 명시적 호출
            }
            catch (Exception ex)
            {

                dtLoggingTime = DateTime.Now;
                Debug.WriteLine(dtLoggingTime.ToString("G", DateTimeFormatInfo.InvariantInfo));

                //Tx 명시적 호출
                //ContextUtil.SetAbort();
                //예외처리
                throw this.ProcessErr(ex);
            }
            finally
            {
                //성능로그끝
                this.TraceEndPoint();
            }
        }

        public int GetDefectCount(string[] DefectInfo)
        {
            //성능로그시작
            this.TraceStartPoint();
            DataTable dt = null;
            int idefectcount = 0;
            try
            {
                dt = this.GetDataTable("SELECT_DEFECT_COUNT", null, new string[2] { DefectInfo[2], DefectInfo[0] });
                //ContextUtil.SetComplete();
            }
            catch (Exception ex)
            {
                //ContextUtil.SetAbort();
                //예외처리
                throw this.ProcessErr(ex);
            }
            finally
            {
                //성능로그끝
                this.TraceEndPoint();
            }

            idefectcount = int.Parse(dt.Rows[0][0].ToString());
            return idefectcount;
        }
        public int GetDefectCount(string stepSeq)
        {
            //성능로그시작
            this.TraceStartPoint();
            DataTable dt = null;
            int idefectcount = 0;
            try
            {
                dt = this.GetDataTable("SELECT_DEFECT_COUNT2", null, new string[] { stepSeq });
                //ContextUtil.SetComplete();
            }
            catch (Exception ex)
            {
                //ContextUtil.SetAbort();
                //예외처리
                throw this.ProcessErr(ex);
            }
            finally
            {
                //성능로그끝
                this.TraceEndPoint();
            }

            idefectcount = int.Parse(dt.Rows[0][0].ToString());
            return idefectcount;
        }

        public DataTable GetDefect(string[] where)
        {
            return this.GetDataTable("SELECT_DEFECTS", null, where);
        }

        public DataTable GetDefect(long step_seq)
        {
            return this.GetDataTable("GET_DEFECT", null, new string[] { step_seq.ToString() });
        }

        public DataTable GetDefect(long[] step_seqs)
        {
            return this.GetDataTable("GET_DEFECT_MULTI", new string[] { string.Join(",", step_seqs) }, null);
        }

        public object[,] GetDefectDataToObjectArray(
            long[] steps
            )
        {
            return GetDataToObjectArray("GET_DEFECT_MULTI", new string[] { string.Join(",", steps) }, null);
        }

        public void Import(string filename)
        {
            this.Execute("IMPORT_DEFECT", null, new string[1] { filename });
        }

        public void Update(string[] UpInfo)
        {
            try
            {
                this.Execute("UPDATE_DEFECT", null, UpInfo);
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        public int UpdateDefect01(
            string[,] Params
            )
        {
            return this.ExecuteMultiple(
                "UPDATE_DEFECT_01",
                Params
                );
        }

        public int UpdateDefect(
            string[,] Params
            )
        {
            return this.ExecuteMultiple(
                "UPDATE_DEFECT",
                Params
                );
        }

        public int UpdateReclassifiedDefect(
            string[,] Params
            )
        {
            return this.ExecuteMultipleEx(
                "UPDATE_RECLASSIFY_DEFECT",
                null,
                Params
                );
        }


        public void CreateTempTable(string[] tabinfo)
        {
            try
            {
                //업무 로직 구현
                /// 빌딩블럭이 이상함 
                /// Excute로 하면 Error남
                /// ==================================================================
                this.Execute("CREATE_TEMP_TABLE", tabinfo, null);
                //Tx 명시적 호출
                //ContextUtil.SetComplete();
            }
            catch (Exception ex)
            {
                //Tx 명시적 호출
                //ContextUtil.SetAbort();
                //예외처리
                throw this.ProcessErr(ex);
            }
        }
        public void Create(string dynamicQueryElements)
        {
            DateTime dtLoggingTime = DateTime.Now;
            //성능로그시작
            this.TraceStartPoint();
            DataTable dt = null;
            try
            {
                //업무 로직 구현
                /// 빌딩블럭이 이상함 
                /// Excute로 하면 Error남
                /// ==================================================================
                dt = this.GetDataTable("CREATE_DEFECT2", new string[] { dynamicQueryElements }, null);

            }
            catch (Exception ex)
            {

                dtLoggingTime = DateTime.Now;
                Debug.WriteLine(dtLoggingTime.ToString("G", DateTimeFormatInfo.InvariantInfo));

                throw this.ProcessErr(ex);
            }
        }

        public DataTable GetDensity(long[] step_seqs, string SelDefect = "ALL")
        {
            this.TraceStartPoint();
            DataTable dt = null;
            try
            {
                if (SelDefect == "ALL")
                {
                    dt = this.GetDataTable("GET_DENSITY", null
                                         , new string[] {Miracom.Middleware.Helper.ConvertList(step_seqs,false)
												   , Miracom.Middleware.Helper.ConvertList(step_seqs,false) });
                }
                else
                {
                    dt = this.GetDataTable("GET_DENSITY_02", new string[] { SelDefect }
                                         , new string[] {Miracom.Middleware.Helper.ConvertList(step_seqs,false)
												   , Miracom.Middleware.Helper.ConvertList(step_seqs,false) });
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;

            }
        }

        public DataTable GetDefectCount(long[] step_seqs, string column)
        {
            this.TraceStartPoint();
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_DEFECT_CNT", new string[] {  column
									                                  , column
									                                  , column
									                                  , Miracom.Middleware.Helper.ConvertList(step_seqs)
									                                  , column
									                                  , column }, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }

        public DataTable GetReclassifiedDefectCnt(
            long[] steps
            )
        {
            return this.GetDataTable(
                "GET_RECLASSIFIED_DEFECT_CNT",
                new string[] { Miracom.Middleware.Helper.ConvertList(steps) },
                null
                );
        }

        public DataTable GetNewDefectCnt(long step_seq)
        {
            this.TraceStartPoint();
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_NEW_DEFECT_CNT", null, new string[] { step_seq.ToString() });
                return dt;
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;

            }
        }

        public DataTable GetDSAData(long first_step_seq, long step_seq, float tolerance)
        {
            this.TraceStartPoint();
            DataTable dt = null;

            try
            {
                string strTolerance = string.Format("{0}", tolerance);
                string[] dynamic = new string[12] {strTolerance
												, strTolerance
												, strTolerance
												, strTolerance
												, strTolerance
												, strTolerance
												, strTolerance
												, strTolerance
												, strTolerance
												, strTolerance
												, strTolerance
												, strTolerance  };

                dt = this.GetDataTable("GET_DSA_DATA", dynamic, new string[] { first_step_seq.ToString(), step_seq.ToString() });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;

            }
        }

        public DataTable GetNewDefect(long step_seq)
        {
            try
            {
                return this.GetDataTable("GET_NEW_DEFECT", null, new string[] { step_seq.ToString() });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// result time 기준 최근의 STEP의 Defect X,Y를 가져옵니다.
        /// </summary>
        public DataTable GetLastestStepDefectXY(string waferID, string stepID, string resultTime)
        {
            return GetDataTable("SELECT_LAST_WAFER_DEFECT", null, new string[] { waferID, stepID, resultTime });
        }

        public void SetMigrationData(string[,] strData)
        {
            this.ExecuteMultiple("UPDATE_AREA_ZERO", strData);
        }

        #region ' Repeat Defect '

        public DataTable GetDieRepeatDefect(int stepSeq, int tolerance)
        {
            DataTable dt = null;
            try
            {
                string strTolerance = string.Format("{0}", tolerance);
                string strStepSeq = string.Format("{0}", stepSeq);

                string[] dynamic = new string[] { strTolerance, strTolerance };
                string[] para = new string[] { strStepSeq };

                dt = this.GetDataTable("DIE_REPEAT_DEFECT", dynamic, para);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }

        public DataTable GetShotRepeatDefect(int stepSeq, string product, int dieSizeX, int dieSizeY, int tolerance)
        {
            DataTable dt = null;
            try
            {
                string strTolerance = string.Format("{0}", tolerance);
                string strStepSeq = string.Format("{0}", stepSeq);
                string strDieSizeX = string.Format("{0}", dieSizeX);
                string strDieSizeY = string.Format("{0}", dieSizeY);

                string[] dynamic = new string[] { strDieSizeX, strTolerance, strDieSizeY, strTolerance };
                string[] para = new string[] { strStepSeq, product };

                dt = this.GetDataTable("SHOT_REPEAT_DEFECT", dynamic, para);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }

        public DataTable GetRepeatListByShot(int dieSizeX, int dieSizeY, int tolerance, int stepSeq, string product, int repeatCnt)
        {
            DataTable dt = null;
            try
            {
                string strTolerance = string.Format("{0}", tolerance);
                string strDieSizeX = string.Format("{0}", dieSizeX);
                string strDieSizeY = string.Format("{0}", dieSizeY);
                string strStepSeq = string.Format("{0}", stepSeq);
                string strRepeatCnt = string.Format("{0}", repeatCnt);

                string[] dynamic = new string[] {
													  strDieSizeX
													  , strTolerance
													  , strDieSizeY
													  , strTolerance
													  , strDieSizeX
													  , strTolerance
													  , strDieSizeY
													  , strTolerance
													  , strRepeatCnt};
                string[] para = new string[] {
												   strStepSeq
												   , product
											   };
                dt = this.GetDataTable("REPEAT_LIST_BY_SHOT", dynamic, para);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }

        public DataTable GetRepeatListByDie(int tolerance, int stepSeq, int repeatCnt)
        {
            DataTable dt = null;
            try
            {
                string strTolerance = string.Format("{0}", tolerance);
                string strStepSeq = string.Format("{0}", stepSeq);
                string strRepeatCnt = string.Format("{0}", repeatCnt);

                string[] dynamic = new string[] {
													  strTolerance
													  , strTolerance
													  , strTolerance
													  , strTolerance
													  };
                string[] para = new string[] {
												   strStepSeq
												   , strRepeatCnt
											   };
                dt = this.GetDataTable("REPEAT_LIST_BY_DIE", dynamic, para);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }

        #endregion ========================================================================

        #region [ MapMathcing ]

        public int GetDefectDataCount(string strWaferSeq, string strStepSeq)
        {
            DataTable dtData = null;
            int iDefectCount = 0;

            try
            {
                dtData = this.GetDataTable("SELECT_DEFECT_COUNT", null, new string[] { strWaferSeq, strStepSeq });
                if (dtData != null && dtData.Rows.Count > 0)
                {
                    iDefectCount = int.Parse(dtData.Rows[0][0].ToString());
                }
              
                return iDefectCount;
            }
            finally
            {
                if (dtData != null)
                    dtData.Dispose();
            }
        }

        public void UpdateDefectSeq(string strNewStepSeq, string strNewWaferSeq, string strOriStepSeq, string strOriWaferSeq)
        {
            try
            {
                this.Execute("UPDATE_MAINT_SEQ", null, new string[] {strNewStepSeq, strNewWaferSeq, strOriStepSeq, strOriWaferSeq});
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        public void DeleteDefectSeq(string strStepSeq, string strWaferSeq)
        {
            try
            {
                this.Execute("DELETE_DEFECT", null, new string[] { strWaferSeq, strStepSeq });
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        #endregion [ Map Matching ]

        /// <summary>
        /// IMAGECOUNT 값을 일괄 업데이트 합니다.
        /// </summary>
        public void UpdateImageCount(long stepSeq)
        {
            ExecuteNonQuery("UPDATE_IMAGECOUNT", null, new string[] { stepSeq.ToString() });
        }

        public DataTable GetPatternSearch_XYList(long[] stepSeqArr)
        {
            return GetDataTable("SELECT_XY_LIST", new string[] { String.Join<long>(",", stepSeqArr) }, null);
        }

        public DataTable GetPatternSearch_XYList(long stepSeq)
        {
            return GetDataTable("SELECT_XY_LIST_01", null, new string[] { stepSeq.ToString() });
        }
    }
}
