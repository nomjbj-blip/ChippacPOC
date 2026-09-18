using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DACrux.SEMDMS.DSL;
using System.Diagnostics;
using System.Drawing;
using DACrux.Common.DSL;

namespace DACrux.SEMDMS.BSL
{
    public class DefectMapAnalysis
        : Miracom.Middleware.BaseComponent, DACrux.SEMDMS.Interface.iDefectMapAnalysis
    {
        public DefectMapAnalysis()
        {
        }

        public System.Data.DataTable GetStepInfo(long[] step_seqs)
        {
            DACrux.SEMDMS.DSL.TQD_INSP_INFO oInspInfo = null;
            oInspInfo = new DACrux.SEMDMS.DSL.TQD_INSP_INFO();
            return oInspInfo.GetStepInfo(step_seqs);

        }

        public System.Data.DataTable GetStepInfo(string[] step_seqs)
        {
            DACrux.SEMDMS.DSL.TQD_INSP_INFO oInspInfo = null;
            oInspInfo = new DACrux.SEMDMS.DSL.TQD_INSP_INFO();
            return oInspInfo.GetStepInfo(step_seqs);

        }

        public DataTable GetStepInfoByStepSeq(string step_seq)
        {
            DACrux.SEMDMS.DSL.TQD_INSP_INFO oInspInfo = null;
            oInspInfo = new DACrux.SEMDMS.DSL.TQD_INSP_INFO();
            return oInspInfo.GetStepInfoByStepSeq(step_seq);

        }

        public System.Data.DataTable GetStepSum(long step_seq)
        {
            DACrux.SEMDMS.DSL.TQD_STEP_SUM oStepSum = null;
            oStepSum = new DACrux.SEMDMS.DSL.TQD_STEP_SUM();
            return oStepSum.GetStepSum(step_seq);

        }

        public System.Data.DataTable GetStepList(string start, string end, string[] fieldsorts, string[] wheres, string sort)
        {
            DACrux.SEMDMS.DSL.TQD_INSP_INFO oInspInfo = null;
            oInspInfo = new DACrux.SEMDMS.DSL.TQD_INSP_INFO();
            return oInspInfo.GetStepList(start, end, fieldsorts, wheres, sort);

        }

        public System.Data.DataTable GetSetupMap(long Setup_seq)
        {
            DACrux.SEMDMS.DSL.TQD_SETUP_MAP oSetupMap = null;
            oSetupMap = new DACrux.SEMDMS.DSL.TQD_SETUP_MAP();
            return oSetupMap.GetData("SELECT_MAP_SAMPLE", null, new string[] { Setup_seq.ToString() });
        }

        public System.Data.DataTable GetSetupRecipe(long setup_seq)
        {
            DACrux.SEMDMS.DSL.TQD_SETUP oSetup = null;
            oSetup = new DACrux.SEMDMS.DSL.TQD_SETUP();
            return oSetup.GetSetupRecipe(setup_seq);
        }

        public System.Data.DataTable GetDefectData(long step_seq)
        {
            DACrux.SEMDMS.DSL.TQD_DEFECT oDefect = null;
            oDefect = new DACrux.SEMDMS.DSL.TQD_DEFECT();
            return oDefect.GetDefect(step_seq);
        }

        public DataTable GetDefectData(
            long[] stepseq
            )
        {
            TQD_DEFECT oDefect = null;
            oDefect = new TQD_DEFECT();
            return oDefect.GetDefect(stepseq);
        }

        public byte[] GetDefectDataToObjectArray_Comp(
            long[] steps
            )
        {
            return DACrux.Base.Util.ObjectToCompressedBytes(GetDefectDataToObjectArray(steps));
        }

        public object[,] GetDefectDataToObjectArray(
            long[] steps
            )
        {
            TQD_DEFECT obj = new TQD_DEFECT();
            return obj.GetDefectDataToObjectArray(steps);
        }

        public System.Data.DataTable GetDensity(long[] step_seqs, string SelDefect = "ALL")
        {
            DACrux.SEMDMS.DSL.TQD_DEFECT oDefect = null;
            oDefect = new DACrux.SEMDMS.DSL.TQD_DEFECT();
            return oDefect.GetDensity(step_seqs, SelDefect);
        }

        public System.Data.DataTable GetDefectImage(long step_seq, int[] defectids)
        {
            DACrux.SEMDMS.DSL.TQD_IMAGES oImage = null;
            oImage = new DACrux.SEMDMS.DSL.TQD_IMAGES();
            return oImage.GetDefectImage(step_seq, defectids);
        }

        public System.Data.DataTable GetShotRepeatDefect(long step_seq, string product, float die_size_x, float die_size_y, float tolerance)
        {
            DACrux.SEMDMS.DSL.TQD_SHOT oImage = null;
            oImage = new DACrux.SEMDMS.DSL.TQD_SHOT();
            return oImage.GetShotRepeatDefect(step_seq, product, die_size_x, die_size_y, tolerance);
        }

        public System.Data.DataTable GetDieRepeatDefect(long step_seq, float tolerance)
        {
            DACrux.SEMDMS.DSL.TQD_SHOT oImage = null;

            try
            {
                oImage = new DACrux.SEMDMS.DSL.TQD_SHOT();
                return oImage.GetDieRepeatDefect(step_seq, tolerance);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public System.Data.DataTable GetSize(string userid)
        {
            DACrux.SEMDMS.DSL.TQD_COLORBYSIZE oImage = null;

            try
            {
                oImage = new DACrux.SEMDMS.DSL.TQD_COLORBYSIZE();
                return oImage.SelectColorListByDefectSize(userid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public System.Data.DataTable SelectColorByDefectSize(string userid)
        {
            DACrux.SEMDMS.DSL.TQD_COLORBYSIZE oImage = null;

            try
            {
                oImage = new DACrux.SEMDMS.DSL.TQD_COLORBYSIZE();
                return oImage.SelectColorByDefectSize(userid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dictionary<int, Color> GetDefectSizeColor(
            string userid
            )
        {
            Dictionary<int, Color> dic = new Dictionary<int, Color>();
            TQD_COLORBYSIZE oDefectSize = new TQD_COLORBYSIZE();
            DataTable dt = oDefectSize.SelectColorListByDefectSize01(userid);

            int seq = 0;
            string strColor = string.Empty;

            foreach (DataRow row in dt.Rows)
            {
                seq = DACrux.Base.Convert.intParse(row["SIZE_SEQ"].ToString());
                strColor = row["COLOR"].ToString();
                dic.Add(seq, ColorTranslator.FromHtml(strColor));
            }
            return dic;
        }


        public System.Data.DataTable GetInspInfoByLotId(string LotId)
        {
            DACrux.SEMDMS.DSL.TQD_INSP_INFO oImage = null;

            try
            {
                oImage = new DSL.TQD_INSP_INFO();
                return oImage.GetInspInfoByLotId(LotId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public System.Data.DataTable GetDefectCount(long[] step_seqs, string column)
        {
            DACrux.SEMDMS.DSL.TQD_DEFECT oDefect = null;
            try
            {
                oDefect = new DACrux.SEMDMS.DSL.TQD_DEFECT();
                return oDefect.GetDefectCount(step_seqs, column);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GetFirstStepCnt(long step_seq, ref int newCnt, ref int carryOverCnt, ref int missingCnt)
        {
            DACrux.SEMDMS.DSL.TQD_DEFECT oDefect = null;
            DataTable dt = null;
            try
            {
                oDefect = new DACrux.SEMDMS.DSL.TQD_DEFECT();

                dt = oDefect.GetNewDefectCnt(step_seq);
                string kind;
                int cnt;
                kind = string.Format("{0}", dt.Rows[0]["NEW_CARRYOVER_MISSING"]);
                cnt = DACrux.Base.Convert.intParse(string.Format("{0}", dt.Rows[0]["CNT"]));
                newCnt = cnt;
                carryOverCnt = 0;
                missingCnt = 0;

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

        public void GetDsaCnt(
            long first_step_seq,
            long step_seq,
            float tolerance,
            ref int newCnt,
            ref int carryOverCnt,
            ref int missingCnt
            )
        {
            DataTable dt = null;
            DataRow[] dr = null;
            DACrux.SEMDMS.DSL.TQD_DEFECT oDefect = null;
            try
            {
                oDefect = new DACrux.SEMDMS.DSL.TQD_DEFECT();
                // DEFECT 의 갯수가 많으면 GROUP BY에서 DELAY 발생
                // rawdata 를 가져와서 count 체크
                //dt = oDefect.GetDSACnt(first_step_seq, step_seq, tolerance);
                dt = oDefect.GetDSAData(first_step_seq, step_seq, tolerance);

                newCnt = 0;
                carryOverCnt = 0;
                missingCnt = 0;

                //string kind;
                //int cnt;
                //for (int a = 0; a < dt.Rows.Count; a++)
                //{
                //    kind = string.Format("{0}", dt.Rows[a]["NEW_CARRYOVER_MISSING"]);
                //    cnt = DACrux.Base.Convert.intParse(string.Format("{0}", dt.Rows[a]["CNT"]));
                //    if (kind.Equals("N")) newCnt = cnt;
                //    else if (kind.Equals("C")) carryOverCnt = cnt;
                //    else if (kind.Equals("M")) missingCnt = -cnt;
                //}
                dr = dt.Select(String.Format("[NEW_CARRYOVER_MISSING] = 'N'"));
                newCnt = dr.Length;

                dr = dt.Select(String.Format("[NEW_CARRYOVER_MISSING] = 'C'"));
                carryOverCnt = dr.Length;

                dr = dt.Select(String.Format("[NEW_CARRYOVER_MISSING] = 'M'"));
                missingCnt = -dr.Length;

            }
            finally
            {
                if (dt != null) dt.Dispose();
                dr = null;
                dt = null;
            }
        }

        public System.Data.DataTable GetFirstStep(long step_seq)
        {
            DACrux.SEMDMS.DSL.TQD_DEFECT oDefect = null;
            DataTable dt = null;
            try
            {
                oDefect = new DACrux.SEMDMS.DSL.TQD_DEFECT();
                dt = oDefect.GetNewDefect(step_seq);
                return dt;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }

        public System.Data.DataTable GetDsa(long first_step_seq, long step_seq, float tolerance)
        {
            DataTable dt = null;
            DACrux.SEMDMS.DSL.TQD_DEFECT oDefect = null;
            try
            {
                oDefect = new DACrux.SEMDMS.DSL.TQD_DEFECT();
                dt = oDefect.GetDSAData(first_step_seq, step_seq, tolerance);
                return dt;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }

        public DataTable GetDefectImageInfo(long step_seq)
        {
            DACrux.SEMDMS.DSL.TQD_IMAGES oDefect = null;
            oDefect = new DSL.TQD_IMAGES();
            return oDefect.GetDefectImageInfo(step_seq);

        }

        public DataTable GetStepListByStepID(string TestArea, string StartTime, string EndTime, string LikeDevice, string LikeLot, string LikeWafer)
        {
            DACrux.SEMDMS.DSL.TQD_INSP_INFO oDSL = null;
            oDSL = new DACrux.SEMDMS.DSL.TQD_INSP_INFO();
            return oDSL.GetStepListByStepID(TestArea, StartTime, EndTime, LikeDevice, LikeLot, LikeWafer);
        }

        public DataTable SelectMapParsingResultList()
        {
            DACrux.SEMDMS.DSL.TQD_INSP_INFO oInspInfo = null;
            oInspInfo = new DSL.TQD_INSP_INFO();
            return oInspInfo.SelectMapParsingResultList();
        }

        public DataTable SelectParsingResultLotList()
        {
            DACrux.SEMDMS.DSL.TQD_INSP_INFO oInspInfo = null;
            oInspInfo = new DSL.TQD_INSP_INFO();
            return oInspInfo.SelectMapParsingResultLotList();
        }

        public DataTable SelectParsingResultWaferList()
        {
            DACrux.SEMDMS.DSL.TQD_INSP_INFO oInspInfo = null;
            oInspInfo = new DSL.TQD_INSP_INFO();
            return oInspInfo.SelectMapParsingResultWaferList();
        }

        public DataTable SelectParsingResultStepList()
        {
            DACrux.SEMDMS.DSL.TQD_INSP_INFO oInspInfo = null;
            oInspInfo = new DSL.TQD_INSP_INFO();
            return oInspInfo.SelectMapParsingResultStepList();
        }

        public DataTable SelectParsingResultInspEqList()
        {
            DACrux.SEMDMS.DSL.TQD_INSP_INFO oInspInfo = null;
            oInspInfo = new DSL.TQD_INSP_INFO();
            return oInspInfo.SelectMapParsingResultInspEqList();
        }

        public DataTable SelectParsingResultByItem(string lotId, string waferId, string stepId, string inspEq)
        {
            DACrux.SEMDMS.DSL.TQD_INSP_INFO oInspInfo = null;
            oInspInfo = new DSL.TQD_INSP_INFO();
            return oInspInfo.SelectMapParsingResultByItem(lotId, waferId, stepId, inspEq);
        }


        public DataTable GetRepeatListByShot(int dieSizeX, int dieSizeY, int tolerance, int stepSeq, string product, int repeatCnt)
        {
            DACrux.SEMDMS.DSL.TQD_DEFECT o = null;
            DataTable dt = null;

            try
            {
                o = new DSL.TQD_DEFECT();
                dt = o.GetRepeatListByShot(dieSizeX, dieSizeY, tolerance, stepSeq, product, repeatCnt);
                return dt;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }

        public DataTable GetRepeatListByDie(int tolerance, int stepSeq, int repeatCnt)
        {
            DACrux.SEMDMS.DSL.TQD_DEFECT o = null;
            DataTable dt = null;

            try
            {
                o = new DSL.TQD_DEFECT();
                dt = o.GetRepeatListByDie(tolerance, stepSeq, repeatCnt);
                return dt;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }

        #region ' Test Overlay '
        public DataTable GetWaferDefectOverlay(string program, string[] waferSeq)
        {
            DACrux.SEMDMS.DSL.TQD_WAFER_SUM o = null;
            DataTable dt = null;
            o = new DACrux.SEMDMS.DSL.TQD_WAFER_SUM();
            dt = o.GetWaferDefectOverlay(program, waferSeq);
            return dt;
        }

        public DataTable GetStepInfoOverlay(string lotId, string waferId)
        {
            DACrux.SEMDMS.DSL.TQD_INSP_INFO o = null;
            DataTable dt = null;
            o = new DSL.TQD_INSP_INFO();
            dt = o.GetStepInfoOverlay(lotId, waferId);
            return dt;

        }
        #endregion ===============================================================

        public System.Data.DataTable GetImageGallery(long[] step_seqs)
        {
            DACrux.SEMDMS.DSL.TQD_INSP_INFO oInspInfo = null;
            string[] str_step_seq = new string[step_seqs.Length];
            for (int i = 0; i < str_step_seq.Length; i++) str_step_seq[i] = step_seqs[i].ToString();

            oInspInfo = new DACrux.SEMDMS.DSL.TQD_INSP_INFO();
            return oInspInfo.GetData("SELECT_IMAGE_GALLERY", new string[] { string.Join(",", str_step_seq) }, null);
        }

        public DataTable GetInspInfo01(long[] stepSeqArr)
        {
            TQD_INSP_INFO obj = new TQD_INSP_INFO();
            return obj.GetInspInfo01(stepSeqArr);
        }

        public DataTable GetInspInfo02(long[] stepSeqArr)
        {
            TQD_INSP_INFO obj = new TQD_INSP_INFO();
            return obj.GetInspInfo02(stepSeqArr);
        }

        public DataTable GetDefectImagePath01(long[] stepSeqArr)
        {
            TQD_IMAGES obj = new TQD_IMAGES();
            return obj.GetDefectImagePath01(stepSeqArr);
        }

        #region DB 하이텍

        public DataSet GetChartAnalysis(
            long[] steps
            )
        {
            DataSet ds = new DataSet();

            //--

            using (TQD_INSP_INFO oInspInfo = new TQD_INSP_INFO())
            {
                //-- .INSPECTION INFORMATION
                DataTable dt = oInspInfo.GetStepInfo(
                    steps
                    );
                dt.TableName = "INSPECTION_INFO";
                ds.Tables.Add(dt);
            }

            //--

            using (TQD_DEFECT oDefect = new TQD_DEFECT())
            {
                DataTable dt = oDefect.GetDefect(
                    steps
                    );
                dt.TableName = "DEFECT_INFO";
                ds.Tables.Add(dt);
            }

            //--

            using (TQD_DEFECT_TYPE oDefectType = new TQD_DEFECT_TYPE())
            {
                DataTable dt = oDefectType.SelectDefectList();
                dt.TableName = "DEFECT_TYPE_INFO";
                ds.Tables.Add(dt);
            }

            return ds;
        }

        public DataTable GetDefectImages(
            long[] steps
            )
        {
            TQD_IMAGES oImages = new TQD_IMAGES();
            return oImages.GetData(
                "GET_DEFECT_IMAGE_ALL",
                new string[] { String.Format("'{0}'", String.Join("','", steps)) },
                null
                );
        }

        public DataTable SetReclassifyDefectImageList(
            long[] steps,
            string[,] Params,
            string userId,
            string ipAddr
            )
        {
            int iReturnValue = 0;

            //--

            DataTable dt = null;
            using (TQD_DEFECT oDefect = new TQD_DEFECT())
            {
                iReturnValue += oDefect.UpdateReclassifiedDefect(
                    Params
                    );
                dt = oDefect.GetReclassifiedDefectCnt(
                    steps
                    );
            }

            //--

            iReturnValue = 0;
            string[,] inspInfoParam = new string[dt.Rows.Count, 3];
            using (TQD_INSP_INFO oInspInfo = new TQD_INSP_INFO())
            {
                for (int idx = 0; idx < dt.Rows.Count; idx++)
                {
                    inspInfoParam[idx, 0] = dt.Rows[idx]["STEP_SEQ"].ToString();
                    inspInfoParam[idx, 1] = dt.Rows[idx]["WAFER_SEQ"].ToString();
                    inspInfoParam[idx, 2] = dt.Rows[idx]["CNT"].ToString();
                }

                iReturnValue += oInspInfo.UpdateReclassifiedDefect(
                    inspInfoParam
                    );
            }

            //--

            //--

            using (TQD_IMAGES oImages = new TQD_IMAGES())
            {
                dt = oImages.GetData(
                    "GET_DEFECT_IMAGE_ALL",
                    new string[] { String.Format("'{0}'", String.Join("','", steps)) },
                    null
                    );
            }

            //--

            return dt;
        }

        public byte[] SetReclassifyDefectList_Comp(
            long[] steps,
            string[,] Params,
            string userId,
            string ipAddr
            )
        {
            return DACrux.Base.Util.ObjectToCompressedBytes(
                SetReclassifyDefectList(steps, Params, userId, ipAddr)
                );
        }

        public object[,] SetReclassifyDefectList(
            long[] steps,
            string[,] Params,
            string userId, 
            string ipAddr
            )
        {
            int iReturnValue = 0;

            //--

            DataTable dt = null;
            using (TQD_DEFECT oDefect = new TQD_DEFECT())
            {
                iReturnValue += oDefect.UpdateReclassifiedDefect(
                    Params
                    );
                dt = oDefect.GetReclassifiedDefectCnt(
                    steps
                    );
            }

            //--

            iReturnValue = 0;
            string[,] inspInfoParam = new string[dt.Rows.Count, 3];
            using (TQD_INSP_INFO oInspInfo = new TQD_INSP_INFO())
            {
                for (int idx = 0; idx < dt.Rows.Count; idx++)
                {
                    inspInfoParam[idx, 0] = dt.Rows[idx]["STEP_SEQ"].ToString();
                    inspInfoParam[idx, 1] = dt.Rows[idx]["WAFER_SEQ"].ToString();
                    inspInfoParam[idx, 2] = dt.Rows[idx]["CNT"].ToString();
                }

                iReturnValue += oInspInfo.UpdateReclassifiedDefect(
                    inspInfoParam
                    );
            }

            object[,] data = null;
            using (TQD_DEFECT oDefect = new TQD_DEFECT())
            {
                data = oDefect.GetDefectDataToObjectArray(steps);
            }

            return data;
        }


        public DataTable SetRemoveDefectImageList(long[] steps, string[,] Params, string userId, string ipAddr)
        {
            int iReturnValue = 0;
            DataTable dt = null;
            TQD_IMAGES oImages = new TQD_IMAGES();
            TQD_INSP_INFO oInspInfo = new TQD_INSP_INFO();
            iReturnValue = oImages.DeleteDefectImage(Params);

            dt = oInspInfo.GetStepInfo(steps);
            for (int idx = 0; idx < Params.GetLength(0); idx++)
            {
                DataRow[] rows = dt.Select(String.Format("[STEP_SEQ] = '{0}' AND [WAFER_SEQ] = '{1}'", Params[idx, 0], Params[idx, 2]));
                string[] oMaint = new string[12];
                oMaint[0] = rows[0]["LOT_ID"].ToString(); //LOT_ID   
                oMaint[1] = rows[0]["PRODUCT"].ToString(); //PRODUCT
                oMaint[2] = rows[0]["STEP_ID"].ToString(); //STEP_ID
                oMaint[3] = rows[0]["SLOT_ID"].ToString(); //SLOT_ID
                oMaint[4] = rows[0]["WAFER_ID"].ToString(); //WAFER_ID
                oMaint[5] = userId; //TRAN_USER
                oMaint[6] = ipAddr; //TRAN_USER_IP
                oMaint[7] = string.Format("{0}, {1}, {2}, {3}", Params[idx, 0], Params[idx, 1], Params[idx, 2], Params[idx, 3]); //PREV_VALUE
                oMaint[8] = "Delete To Defect Image"; //CURR_VALUE
                oMaint[9] = Params[idx, 0]; //PREV_STEP_SEQ
                oMaint[10] = Params[idx, 0]; //CURR_STEP_SEQ
                oMaint[11] = "Delete To Defect Image"; //USER_COMMENT
                InsertMaintHis(oMaint);
            }
            dt = oImages.GetData(
                "GET_DEFECT_IMAGE_ALL",
                new string[] { String.Format("'{0}'", String.Join("','", steps)) },
                null
                );
            return dt;
        }




        public System.Data.DataTable GetColorByDefectType()
        {
            DACrux.SEMDMS.DSL.TQD_DEFECT_TYPE oDefectType = null;
            oDefectType = new DACrux.SEMDMS.DSL.TQD_DEFECT_TYPE();
            return oDefectType.GetData("SELECT_DEFECT_COLOR", null, null);
        }


        public System.Data.DataTable GetStepWaferList(string start, string end, string[] wheres, bool TimeNotCheck)
        {
            DACrux.SEMDMS.DSL.TQD_INSP_INFO oInspInfo = null;
            StringBuilder sQuery = new StringBuilder();
            oInspInfo = new DACrux.SEMDMS.DSL.TQD_INSP_INFO();

            //시간을 지정 하지않을 경우 조건에서 시간을 뺀다.
            if (TimeNotCheck != true)
            {
                sQuery.AppendLine(" AND RESULTTIMESTAMP BETWEEN ");
                sQuery.AppendLine(string.Format("     TO_DATE ( '{0}', 'YYYY-MM-DD') ", start));
                sQuery.AppendLine(string.Format(" AND TO_DATE ( '{0}', 'YYYY-MM-DD') ", end));
            }

            foreach (string strVal in wheres)
                sQuery.AppendLine(string.Format(" AND {0} ", strVal));

            //if(string.IsNullOrEmpty(sort) == false)
            //    sQuery.AppendLine(string.Format(" ORDER BY {0} ", sort));

            //Wafer List 를 가져온다.
            return oInspInfo.GetData("SELECT_DMS_WAFER_LIST", new string[] { sQuery.ToString() }, null);

        }

        public System.Data.DataSet GetDMSStepWaferList(string start, string end, string[] wheres, bool TimeNotCheck, bool bLastInspection)
        {
            DACrux.SEMDMS.DSL.TQD_INSP_INFO oInspInfo = null;
            StringBuilder sQuery = new StringBuilder();
            oInspInfo = new DACrux.SEMDMS.DSL.TQD_INSP_INFO();
            DataSet ds = null;
            DataTable dtTemp = null;
            string strLastInspect = string.Empty;

            try
            {
                ds = new DataSet();
                //시간을 지정 하지않을 경우 조건에서 시간을 뺀다.
                if (TimeNotCheck != true)
                {
                    sQuery.AppendLine(" AND RESULTTIMESTAMP BETWEEN ");
                    sQuery.AppendLine(string.Format("     TO_DATE ( '{0}', 'YYYY-MM-DD') ", start));
                    sQuery.AppendLine(string.Format(" AND TO_DATE ( '{0}', 'YYYY-MM-DD') ", end));
                }

                foreach (string strVal in wheres)
                    sQuery.AppendLine(string.Format(" AND {0} ", strVal));

                //마지막 Test 한 내용만 가져온다.
                if (bLastInspection == true)
                    strLastInspect = " AND PROBE_COUNT = 0";

                //Wafer List 를 가져온다.
                dtTemp = oInspInfo.GetData("SELECT_DMS_WAFER_LIST", new string[] { sQuery.ToString(), strLastInspect }, null);
                dtTemp.TableName = "STEPLIST";
                ds.Tables.Add(dtTemp);


                //Step List 를 가져온다.
                dtTemp = oInspInfo.GetData("SELECT_DMS_WAFER_ORDER", new string[] { sQuery.ToString(), strLastInspect }, null);
                dtTemp.TableName = "STEP_ORDER";
                ds.Tables.Add(dtTemp);
                ds.AcceptChanges();

                return ds;

            }
            finally
            {
                if (ds != null)
                    ds.Dispose();

                if (dtTemp != null)
                    dtTemp.Dispose();
            }

        }

        /// <summary>
        /// Defect Map View 화면에 대한 기준 Data Gathering
        /// </summary>
        /// <param name="step_seqs"></param>
        /// <returns></returns>
        public byte[] GetDefectMapViewer(long[] step_seqs)
        {
            DataSet dsData = null;
            DataTable dtTemp = null;

            string strSetupSeq = string.Empty;

            List<string> arrSetupSeq;
            List<string> arrTest;

            DACrux.SEMDMS.DSL.TQD_INSP_INFO oInspInfo = null;
            DACrux.SEMDMS.DSL.TQD_SETUP oSetup = null;
            DACrux.SEMDMS.DSL.TQD_SETUP_MAP oSetupMap = null;
            DACrux.SEMDMS.DSL.TQD_DEFECT oDefect = null;

            try
            {
                arrSetupSeq = new List<string>();
                arrTest = new List<string>();
                dsData = new DataSet();
                oInspInfo = new DACrux.SEMDMS.DSL.TQD_INSP_INFO();
                oSetup = new DACrux.SEMDMS.DSL.TQD_SETUP();
                oSetupMap = new DACrux.SEMDMS.DSL.TQD_SETUP_MAP();
                oDefect = new TQD_DEFECT();

                //=================================================================================================================================
                //Step Seq 정보를 가져 온다.
                //=================================================================================================================================
                dtTemp = oInspInfo.GetData("GET_INSP_INFO_MULTI_SEQ", new string[] { string.Join(",", step_seqs) }, null);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("정의된 Wafer 정보가 없습니다. TQD_INSP_INFO Empty");

                dtTemp.TableName = "STEP_INFO";
                dsData.Tables.Add(dtTemp.Copy());

                for (int ir = 0; ir < dtTemp.Rows.Count; ir++)
                {
                    if (arrSetupSeq.IndexOf(dtTemp.Rows[ir]["SETUP_SEQ"].ToString()) < 0)
                        arrSetupSeq.Add(dtTemp.Rows[ir]["SETUP_SEQ"].ToString());

                    if (arrTest.IndexOf(dtTemp.Rows[ir]["TEST"].ToString()) < 0)
                        arrTest.Add(dtTemp.Rows[ir]["TEST"].ToString());

                    if (ir == 0)
                        strSetupSeq = dtTemp.Rows[ir]["SETUP_SEQ"].ToString();

                    //Setup Seq 가 다를 경우 Error 처리 한다. 서로 다른 Map 형상일 경우 그릴수 없음.
                    //if (string.IsNullOrEmpty(strSetupSeq) == false && strSetupSeq != dtTemp.Rows[ir]["SETUP_SEQ"].ToString())
                    //    throw new Exception("서로 다른 형상의 Map 은 해당 화면에서 같이 볼 수 없습니다.");
                }

                //형상이 다른 Map 이 있는지 확인 한다.
                //X, Y 를 Group 하여 Count 수가 다른것이 있으면 Row 가 1 이상이 나옴.
                //dtTemp = oSetupMap.GetData("SELECT_MAP_DUPLE", new string[] { string.Join(",", arrSetupSeq.ToArray()) }, null);
                //if(dtTemp == null || dtTemp.Rows.Count != 1) 
                //    throw new Exception("서로 다른 형상의 Map 은 해당 화면에서 같이 볼 수 없습니다.");

                //dtTemp = oInspInfo.GetData("SELECT_DUPLE_PRODUCT", new string[] { string.Join(",", step_seqs) }, null);
                //if (dtTemp.Rows.Count > 1)
                //    throw new Exception("서로 다른 제품의 Map은 해당 화면에서 같이 볼 수 없습니다.");

                //=================================================================================================================================
                //Setup Seq 정보를 가져 온다.
                //=================================================================================================================================
                dtTemp = oSetup.GetData("GET_SETUP", null, new string[] { strSetupSeq });
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("정의된 Setup 정보가 없습니다. TQD_SETUP Empty");

                dtTemp.TableName = "SETUP_INFO";
                dsData.Tables.Add(dtTemp.Copy());


                //=================================================================================================================================
                //Setup Map 정보를 가져 온다.
                //=================================================================================================================================
                //dtTemp = oSetupMap.GetData("SELECT_MAP_SAMPLE", null, new string[] { strSetupSeq });
                //if (dtTemp == null || dtTemp.Rows.Count <= 0)
                //    throw new Exception("정의된 Setup Map 정보가 없습니다. TQD_SETUP_MAP Empty");

                //여러 Step 정보를 보여 준다.
                //TEST 정보 추가
                dtTemp = oSetupMap.GetData("SELECT_MAP_SAMPLE_MULTI", new string[] { string.Join(",", arrSetupSeq.ToArray())}, null);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("정의된 Setup Map 정보가 없습니다. TQD_SETUP_MAP Empty");

                dtTemp.TableName = "SETUP_MAP";
                dsData.Tables.Add(dtTemp.Copy());

                //Size 를 1000으로 버림했을때 다른 Size 가 있을 경우 Map 을 보여 주지 않는다.
                dtTemp = oSetup.GetData("SELECT_SETUP_DUPLE", new string[] { string.Join(",", step_seqs) }, null);
                if (dtTemp.Rows.Count > 1)
                {
                    dsData.Tables["SETUP_MAP"].Rows.Clear();
                    dsData.Tables["SETUP_MAP"].AcceptChanges();
                }

                //=================================================================================================================================
                //Defect Map 정보를 가져 온다.
                //=================================================================================================================================
                dtTemp = oDefect.GetData("SELECT_DEFECT_MULTI", new string[] { string.Join(",", step_seqs) }, null);
                if (dtTemp == null)
                    throw new Exception("정의된 Defect 정보가 없습니다. TQD_DEFECT Empty");

                dtTemp.TableName = "DEFECT_INFO";
                dsData.Tables.Add(dtTemp.Copy());

                dtTemp = oDefect.GetData("SELECT_INDEX_GRP", new string[] { string.Join(",", step_seqs) }, null);
                if (dtTemp == null)
                    throw new Exception("정의된 Defect Group 정보가 없습니다. TQD_DEFECT Group Empty");

                dtTemp.TableName = "DEFECT_GRP";
                dsData.Tables.Add(dtTemp.Copy());

                return DACrux.Base.Util.ObjectToCompressedBytes(dsData);
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
        /// Bare 및 다른 Map 을 기준으로 die 를 Add 할 수 있다.
        /// </summary>
        /// <param name="step_seqs"></param>
        /// <returns></returns>
        public DataSet GetBareDefectMapViewer(long[] step_seqs, string strMapID)
        {
            DataSet dsData = null;
            DataTable dtTemp = null;
            DataTable dtLotsts = null;

            string strSetupSeq = string.Empty;
            string strLotID = string.Empty;
            string strDevice = string.Empty;

            List<string> arrSetupSeq;
            List<string> arrTest;

            TQD_INSP_INFO oInspInfo = null;
            TQD_SETUP oSetup = null;
            TQD_SETUP_MAP oSetupMap = null;
            TQD_DEFECT oDefect = null;
            TQC_LOT_STS oLotsts = null;
            TQP_MAPDEF oMapDie = null;

            try
            {
                arrSetupSeq = new List<string>();
                arrTest = new List<string>();
                dsData = new DataSet();
                oInspInfo = new DACrux.SEMDMS.DSL.TQD_INSP_INFO();
                oSetup = new DACrux.SEMDMS.DSL.TQD_SETUP();
                oSetupMap = new DACrux.SEMDMS.DSL.TQD_SETUP_MAP();
                oDefect = new TQD_DEFECT();
                oLotsts = new TQC_LOT_STS();
                oMapDie = new TQP_MAPDEF();


                //=================================================================================================================================
                //Step Seq 정보를 가져 온다.
                //=================================================================================================================================
                dtTemp = oInspInfo.GetData("GET_INSP_INFO_MULTI_SEQ", new string[] { string.Join(",", step_seqs) }, null);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("정의된 Wafer 정보가 없습니다. TQD_INSP_INFO Empty");

                dtTemp.TableName = "STEP_INFO";
                dsData.Tables.Add(dtTemp.Copy());

                for (int ir = 0; ir < dtTemp.Rows.Count; ir++)
                {
                    if (arrSetupSeq.IndexOf(dtTemp.Rows[ir]["SETUP_SEQ"].ToString()) < 0)
                        arrSetupSeq.Add(dtTemp.Rows[ir]["SETUP_SEQ"].ToString());

                    if (arrTest.IndexOf(dtTemp.Rows[ir]["TEST"].ToString()) < 0)
                        arrTest.Add(dtTemp.Rows[ir]["TEST"].ToString());

                    if (ir == 0)
                    {
                        strSetupSeq = dtTemp.Rows[ir]["SETUP_SEQ"].ToString();
                        strLotID = dtTemp.Rows[ir]["LOT_ID"].ToString();
                    }

                }

                dtLotsts = GetLotStatusInfo(strLotID);
                if ((dtLotsts != null && dtLotsts.Rows.Count > 0) || string.IsNullOrEmpty(strMapID) == false)
                {
                    if (string.IsNullOrEmpty(strMapID) == true)
                        strDevice = dtLotsts.Rows[0]["MASK_ID"].ToString();
                    else
                        strDevice = strMapID;

                    dtTemp = oMapDie.GetData("GET_MAPDEF_DIE_INFO", null, new string[] { strDevice });
                    if (dtTemp != null && dtTemp.Rows.Count > 0)
                    {
                        dtTemp.TableName = "SETUP_INFO";
                        dsData.Tables.Add(dtTemp.Copy());
                    }

                    dtTemp = oMapDie.GetData("GET_MAPDEF_DIE_DM", null, new string[] { strDevice });
                    if (dtTemp != null && dtTemp.Rows.Count > 0)
                    {
                        dtTemp.TableName = "SETUP_MAP";
                        dsData.Tables.Add(dtTemp.Copy());
                    }

                }

                //=================================================================================================================================
                //Defect Map 정보를 가져 온다.
                //=================================================================================================================================
                dtTemp = oDefect.GetData("SELECT_DEFECT_MULTI", new string[] { string.Join(",", step_seqs) }, null);
                if (dtTemp == null)
                    throw new Exception("정의된 Defect 정보가 없습니다. TQD_DEFECT Empty");

                dtTemp.TableName = "DEFECT_INFO";
                dsData.Tables.Add(dtTemp.Copy());

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
        /// Lot ID 7자리로 확인 후 없으면 6자리로 확인 한다.
        /// Data 가 없을 경우 null Return
        /// Like 로 가져와서 data Table 에서 확인 한다.
        /// </summary>
        /// <param name="lotid"></param>
        /// <returns></returns>
        public DataTable GetLotStatusInfo(string LOT_ID)
        {
            TQC_LOT_STS oTQC_LOT_STS = new TQC_LOT_STS();

            DataTable dtTemp = null;
            DataTable dtTempFilter = null;
            string strLotID = string.Empty;

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

        public System.Data.DataTable GetShotDeviceList()
        {
            TQP_MAPDEF oTQP_MAPDEF = new TQP_MAPDEF();
            return oTQP_MAPDEF.GetData("GET_MAPIDLIST", null, null);
        }

        public System.Data.DataTable GetMapIDListNotDelete()
        {
            TQP_MAPDEF oTQP_MAPDEF = new TQP_MAPDEF();
            return oTQP_MAPDEF.GetMapIDListNotDelete();
        }

        /// <summary>
        /// Defect Map 표현에 대한 정보
        /// </summary>
        public System.Data.DataSet GetDefectMapInfo(long step_seq)
        {
            DataSet ds = new DataSet();

            TQD_INSP_INFO oInspInfo = new TQD_INSP_INFO();
            DataTable dt = oInspInfo.GetData("SELECT_DMS_WAFER_INFO", null, new string[] { step_seq.ToString() });
            dt.TableName = "MAP";
            ds.Tables.Add(dt);

            if (dt == null || dt.Rows.Count == 0)
                return ds;

            string setup_seq = dt.Rows[0]["SETUP_SEQ"].ToString();
            string test = dt.Rows[0]["TEST"].ToString();

            TQD_SETUP_MAP oSetupMap = new TQD_SETUP_MAP();
            dt = oSetupMap.GetData("SELECT_X_Y", null, new string[] { setup_seq, test });
            dt.TableName = "DIE";
            ds.Tables.Add(dt);

            return ds;
        }

        /// <summary>
        /// DEFECT CLASS 정의를 가져옵니다.
        /// </summary>
        public DataTable GetDefectClassInfo()
        {
            TQD_DEFECT_TYPE obj = new TQD_DEFECT_TYPE();
            //return obj.GetData("SELECT_CODE_NAME", null, null);
            return obj.SelectDefectCode();
        }

        public Dictionary<string, Color> GetDefectClassColor(
            )
        {
            Dictionary<string, Color> dic = new Dictionary<string, Color>();
            TQD_DEFECT_TYPE obj = new TQD_DEFECT_TYPE();
            DataTable dt = obj.SelectDefectCode();

            string name = string.Empty;
            string color = string.Empty;
            foreach (DataRow row in dt.Rows)
            {
                name = row["NAME"].ToString();
                color = row["DEFECT_COLOR"].ToString();
                dic.Add(name, ColorTranslator.FromHtml(color));
            }

            return dic;
        }


        /// <summary>
        /// WaferMap 관련 Defect 제외한 Map 을 그릴때 사용한다.
        /// </summary>
        /// <param name="step_seqs"></param>
        /// <returns></returns>
        public System.Data.DataSet GetBasicMapViewer(long[] step_seqs)
        {
            DataSet dsData = null;
            DataTable dtTemp = null;

            string strSetupSeq = string.Empty;

            List<string> arrSetupSeq;
            List<string> arrTest;

            TQD_INSP_INFO oInspInfo = null;
            TQD_SETUP oSetup = null;
            TQD_SETUP_MAP oSetupMap = null;

            try
            {
                arrSetupSeq = new List<string>();
                arrTest = new List<string>();
                dsData = new DataSet();
                oInspInfo = new TQD_INSP_INFO();
                oSetup = new TQD_SETUP();
                oSetupMap = new TQD_SETUP_MAP();

                //=================================================================================================================================
                //Step Seq 정보를 가져 온다.
                //=================================================================================================================================
                dtTemp = oInspInfo.GetData("GET_INSP_INFO_MULTI_SEQ", new string[] { string.Join(",", step_seqs) }, null);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("정의된 Wafer 정보가 없습니다. TQD_INSP_INFO Empty");

                dtTemp.TableName = "STEP_INFO";
                dsData.Tables.Add(dtTemp.Copy());

                for (int ir = 0; ir < dtTemp.Rows.Count; ir++)
                {
                    if (arrSetupSeq.IndexOf(dtTemp.Rows[ir]["SETUP_SEQ"].ToString()) < 0)
                        arrSetupSeq.Add(dtTemp.Rows[ir]["SETUP_SEQ"].ToString());

                    if (arrTest.IndexOf(dtTemp.Rows[ir]["TEST"].ToString()) < 0)
                        arrTest.Add(dtTemp.Rows[ir]["TEST"].ToString());

                    if (ir == 0)
                        strSetupSeq = dtTemp.Rows[ir]["SETUP_SEQ"].ToString();

                    //Setup Seq 가 다를 경우 Error 처리 한다. 서로 다른 Map 형상일 경우 그릴수 없음.
                    //if (string.IsNullOrEmpty(strSetupSeq) == false && strSetupSeq != dtTemp.Rows[ir]["SETUP_SEQ"].ToString())
                    //    throw new Exception("서로 다른 형상의 Map 은 해당 화면에서 같이 볼 수 없습니다.");
                }

                //형상이 다른 Map 이 있는지 확인 한다.
                //X, Y 를 Group 하여 Count 수가 다른것이 있으면 Row 가 1 이상이 나옴.
                //dtTemp = oSetupMap.GetData("SELECT_MAP_DUPLE", new string[] { string.Join(",", arrSetupSeq.ToArray()) }, null);
                //if (dtTemp == null || dtTemp.Rows.Count != 1)
                //    throw new Exception("서로 다른 형상의 Map 은 해당 화면에서 같이 볼 수 없습니다.");

                //dtTemp = oInspInfo.GetData("SELECT_DUPLE_PRODUCT", new string[] { string.Join(",", step_seqs) }, null);
                //if (dtTemp.Rows.Count > 1)
                //    throw new Exception("서로 다른 제품의 Map은 해당 화면에서 같이 볼 수 없습니다.");

                //=================================================================================================================================
                //Setup Seq 정보를 가져 온다.
                //=================================================================================================================================
                dtTemp = oSetup.GetData("GET_SETUP", null, new string[] { strSetupSeq });
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("정의된 Setup 정보가 없습니다. TQD_SETUP Empty");

                dtTemp.TableName = "SETUP_INFO";
                dsData.Tables.Add(dtTemp.Copy());


                //=================================================================================================================================
                //Setup Map 정보를 가져 온다.
                //=================================================================================================================================
                //dtTemp = oSetupMap.GetData("SELECT_MAP_SAMPLE", null, new string[] { strSetupSeq });
                //if (dtTemp == null || dtTemp.Rows.Count <= 0)
                //    throw new Exception("정의된 Setup Map 정보가 없습니다. TQD_SETUP_MAP Empty");

                //여러 Step 정보를 보여 준다.
                dtTemp = oSetupMap.GetData("SELECT_MAP_SAMPLE_MULTI", new string[] { string.Join(",", arrSetupSeq.ToArray()) }, null);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("정의된 Setup Map 정보가 없습니다. TQD_SETUP_MAP Empty");

                dtTemp.TableName = "SETUP_MAP";
                dsData.Tables.Add(dtTemp.Copy());

                //Size 를 1000으로 버림했을때 다른 Size 가 있을 경우 Map 을 보여 주지 않는다.
                dtTemp = oSetup.GetData("SELECT_SETUP_DUPLE", new string[] { string.Join(",", step_seqs) }, null);
                if (dtTemp.Rows.Count > 1)
                {
                    dsData.Tables["SETUP_MAP"].Rows.Clear();
                    dsData.Tables["SETUP_MAP"].AcceptChanges();
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
        /// DSA 관련 Report
        /// </summary>
        /// <param name="step_seqs"></param>
        /// <returns></returns>
        public DataTable GetDSAReport(long[] step_seqs, string strTol)
        {
            TQD_INSP_INFO oInspInfo = null;
            DataTable dt = null;
            oInspInfo = new TQD_INSP_INFO();
            dt = oInspInfo.GetData("SELECT_DAS_REPORT_NEW", new string[] { string.Join(",", step_seqs) }, null);
            return dt;

        }

        public string GetImagePath(
            long stepsseq,
            long waferseq,
            int defectid,
            int imageid
            )
        {
            TQD_IMAGES obj = new TQD_IMAGES();
            DataTable dt = obj.GetImagePath(
                stepsseq,
                waferseq,
                defectid,
                imageid
                );

            if (dt == null || dt.Rows.Count <= 0)
                return string.Empty;

            return String.Format(@"{0}\{1}", dt.Rows[0]["IMAGE_PATH"], dt.Rows[0]["IMAGE_FILENAME"]);
        }

        //public Color GetDieBackColor(string factory, string userID)
        //{
        //    TQC_CONFIG_USER obj = new TQC_CONFIG_USER();
        //    string colorStr = obj.SelectTqcConfigUser(factory, "WAFER_DIE_COLOR", "WAFER_MAP_BG", userID);

        //    if (String.IsNullOrEmpty(colorStr))
        //        return Color.Empty;

        //    return ColorTranslator.FromHtml(String.Format("#{0}", colorStr));
        //}

        public DataTable GetWaferInfo(long[] stepSeqArr)
        {
            TQD_INSP_INFO obj = new TQD_INSP_INFO();
            return obj.GetWaferInfo(stepSeqArr);
        }

        /// <summary>
        /// 기존 Data Set 을 각각 정보들로 나눔.
        /// </summary>
        /// <param name="step_seqs"></param>
        /// <returns></returns>
        public DataSet GetDefectMapViewer_Info(long[] step_seqs)
        {
            DataSet dsData = null;
            DataTable dtTemp = null;

            string strSetupSeq = string.Empty;

            DACrux.SEMDMS.DSL.TQD_INSP_INFO oInspInfo = null;
            DACrux.SEMDMS.DSL.TQD_SETUP oSetup = null;
            DACrux.SEMDMS.DSL.TQD_DEFECT oDefect = null;

            try
            {
                dsData = new DataSet();
                oInspInfo = new DACrux.SEMDMS.DSL.TQD_INSP_INFO();
                oSetup = new DACrux.SEMDMS.DSL.TQD_SETUP();
                oDefect = new TQD_DEFECT();

                //=================================================================================================================================
                //Step Seq 정보를 가져 온다.
                //=================================================================================================================================
                dtTemp = oInspInfo.GetData("GET_INSP_INFO_MULTI_SEQ", new string[] { string.Join(",", step_seqs) }, null);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("정의된 Wafer 정보가 없습니다. TQD_INSP_INFO Empty");

                dtTemp.TableName = "STEP_INFO";
                dsData.Tables.Add(dtTemp.Copy());

                strSetupSeq = dtTemp.Rows[0]["SETUP_SEQ"].ToString();

                //=================================================================================================================================
                //Setup Seq 정보를 가져 온다.
                //=================================================================================================================================
                dtTemp = oSetup.GetData("GET_SETUP", null, new string[] { strSetupSeq });
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("정의된 Setup 정보가 없습니다. TQD_SETUP Empty");

                dtTemp.TableName = "SETUP_INFO";
                dsData.Tables.Add(dtTemp.Copy());

                dtTemp = oDefect.GetData("SELECT_INDEX_GRP", new string[] { string.Join(",", step_seqs) }, null);
                if (dtTemp == null)
                    throw new Exception("정의된 Defect Group 정보가 없습니다. TQD_DEFECT Group Empty");

                dtTemp.TableName = "DEFECT_GRP";
                dsData.Tables.Add(dtTemp.Copy());

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
        /// 기존 Data Set 을 각각 정보들로 나눔.
        /// </summary>
        /// <param name="step_seqs"></param>
        /// <param name="setupseq"></param>
        /// <param name="test"></param>
        /// <returns></returns>
        public byte[] GetDefectMapViewer_Map(long[] step_seqs, string[] setupseq, string[] test)
        {
            DataTable dtWaferMap = null;
            DataTable dtTemp = null;

            DACrux.SEMDMS.DSL.TQD_SETUP oSetup = null;
            DACrux.SEMDMS.DSL.TQD_SETUP_MAP oSetupMap = null;

            try
            {
                oSetup = new DACrux.SEMDMS.DSL.TQD_SETUP();
                oSetupMap = new DACrux.SEMDMS.DSL.TQD_SETUP_MAP();

                //Defect Map Data
                dtWaferMap = oSetupMap.GetData("SELECT_MAP_SAMPLE_MULTI", new string[] { string.Join(",", setupseq) }, null);
                if (dtWaferMap == null || dtWaferMap.Rows.Count <= 0)
                    throw new Exception("정의된 Setup Map 정보가 없습니다. TQD_SETUP_MAP Empty");

                dtWaferMap.TableName = "SETUP_MAP";

                //Size 를 1000으로 버림했을때 다른 Size 가 있을 경우 Map 을 보여 주지 않는다.
                dtTemp = oSetup.GetData("SELECT_SETUP_DUPLE", new string[] { string.Join(",", step_seqs) }, null);
                if (dtTemp.Rows.Count > 1)
                {
                    dtWaferMap.Rows.Clear();
                    dtWaferMap.AcceptChanges();
                }

                return DACrux.Base.Util.ObjectToCompressedBytes(dtWaferMap); ;
            }
            finally
            {
                if (dtTemp != null)
                    dtTemp.Dispose();
            }
        }

        /// <summary>
        /// 기존 Data Set 을 각각 정보들로 나눔.
        /// </summary>
        /// <param name="step_seqs"></param>
        /// <returns></returns>
        public byte[] GetDefectMapViewer_Defects(long[] step_seqs, bool bAdder)
        {
            DataTable dtTemp = null;
            DACrux.SEMDMS.DSL.TQD_DEFECT oDefect = null;
            string strDynamic = string.Empty;
            try
            {
                oDefect = new TQD_DEFECT();
                //=================================================================================================================================
                //Defect Map 정보를 가져 온다.
                //=================================================================================================================================

                if (bAdder)
                    strDynamic = " AND A.ADDER = 1 ";

                dtTemp = oDefect.GetData("SELECT_DEFECT_VIEW", new string[] { string.Join(",", step_seqs), strDynamic }, null);
                if (dtTemp == null)
                    throw new Exception("정의된 Defect 정보가 없습니다. TQD_DEFECT Empty");

                dtTemp.TableName = "DEFECT_INFO";
                return DACrux.Base.Util.ObjectToCompressedBytes(dtTemp);
            }
            finally
            {
                if (dtTemp != null)
                    dtTemp.Dispose();
            }
        }

        /// <summary>
        /// 기존 Data Set 을 각각 정보들로 나눔.
        /// </summary>
        /// <param name="step_seqs"></param>
        /// <returns></returns>
        public byte[] GetDefectMapViewer_Defects2_Comp(long[] step_seqs, bool bAdder)
        {
            List<object[]> list = GetDefectMapViewer_Defects2(step_seqs, bAdder);
            return DACrux.Base.Util.ObjectToCompressedBytes(list);
        }

        public object[,] GetDefectMapViewer_Defects3(long[] step_seqs, bool bAdder)
        {
            DACrux.SEMDMS.DSL.TQD_DEFECT oDefect = null;
            string strDynamic = string.Empty;

            oDefect = new TQD_DEFECT();
            //=================================================================================================================================
            //Defect Map 정보를 가져 온다.
            //=================================================================================================================================

            if (bAdder)
                strDynamic = " AND A.ADDER = 1 ";

            return oDefect.GetDataToArray("SELECT_DEFECT_VIEW", new string[] { string.Join(",", step_seqs), strDynamic }, null);
        }

        public byte[] GetDefectMapViewer_Defects3_Comp(long[] step_seqs, bool bAdder)
        {
            object[,] arr = GetDefectMapViewer_Defects3(step_seqs, bAdder);
            return DACrux.Base.Util.ObjectToCompressedBytes(arr);
        }

        /// <summary>
        /// 기존 Data Set 을 각각 정보들로 나눔.
        /// </summary>
        /// <param name="step_seqs"></param>
        /// <returns></returns>
        public List<object[]> GetDefectMapViewer_Defects2(long[] step_seqs, bool bAdder)
        {
            DACrux.SEMDMS.DSL.TQD_DEFECT oDefect = null;
            string strDynamic = string.Empty;

            oDefect = new TQD_DEFECT();
            //=================================================================================================================================
            //Defect Map 정보를 가져 온다.
            //=================================================================================================================================

            if (bAdder)
                strDynamic = " AND A.ADDER = 1 ";

            return oDefect.GetDataToArrayList("SELECT_DEFECT_VIEW", new string[] { string.Join(",", step_seqs), strDynamic }, null);
        }

        /// <summary>
        /// 기존 Data Set 을 각각 정보들로 나눔.
        /// </summary>
        /// <param name="step_seqs"></param>
        /// <param name="bAdder"></param>
        /// <returns></returns>
        public byte[] GetDefectMapViewer_Chart(long[] step_seqs, bool bAdder)
        {
            DataTable dtTemp = null;
            DACrux.SEMDMS.DSL.TQD_DEFECT oDefect = null;
            string strDynamic = string.Empty;
            try
            {
                oDefect = new TQD_DEFECT();
                //=================================================================================================================================
                //Defect Map 정보를 가져 온다.
                //=================================================================================================================================

                if (bAdder)
                    strDynamic = " AND A.ADDER = 1 ";

                dtTemp = oDefect.GetData("SELECT_DEFECT_VIEW_CHART", new string[] { string.Join(",", step_seqs), strDynamic }, null);
                if (dtTemp == null)
                    throw new Exception("정의된 Defect 정보가 없습니다. TQD_DEFECT Empty");

                dtTemp.TableName = "CHART";
                return DACrux.Base.Util.ObjectToCompressedBytes(dtTemp);
            }
            finally
            {
                if (dtTemp != null)
                    dtTemp.Dispose();
            }
        }

        #endregion

        #region DM Data Maint

        /// <summary>
        /// DM Data 수정할 Wafer 및 Lot List 정보를 가져 온다.
        /// </summary>
        /// <param name="step_seqs"></param>
        /// <returns></returns>
        public System.Data.DataSet GetDMMaintDataList(long[] step_seqs)
        {
            DataSet dsData = null;
            DataTable dtTemp = null;
            TQD_INSP_INFO oInspInfo = null;

            try
            {
                dsData = new DataSet();
                oInspInfo = new TQD_INSP_INFO();

                //=================================================================================================================================
                //Wafer List 정보를 가져 온다.
                //=================================================================================================================================
                dtTemp = oInspInfo.GetData("SELECT_DM_MAINT_WAFER", new string[] { string.Join(",", step_seqs) }, null);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("정의된 Wafer 정보가 없습니다. TQD_INSP_INFO Empty");

                dtTemp.TableName = "WAFER";
                dsData.Tables.Add(dtTemp.Copy());

                //=================================================================================================================================
                //Wafer List 정보를 가져 온다.
                //=================================================================================================================================
                dtTemp = oInspInfo.GetData("SELECT_DM_MAINT_LOT", new string[] { string.Join(",", step_seqs) }, null);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                    throw new Exception("정의된 Wafer 정보가 없습니다. TQD_INSP_INFO Empty");

                dtTemp.TableName = "LOT";
                dsData.Tables.Add(dtTemp.Copy());

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
        /// Wafer 별로 기준정보를 Update 한다.
        /// </summary>
        /// <param name="strStepSeq"></param>
        /// <param name="strLotID"></param>
        /// <param name="iSlotID"></param>
        /// <param name="strStepID"></param>
        /// <param name="strProduct"></param>
        public void SetDMMaintDataUpdateWafer(string strStepSeq, string strWaferID, string strLotID, int iSlotID, string strStepID, string strProduct, string strUser, string strComment, string strLocalIP)
        {
            DataTable dtWaferInfo = null;
            DataTable dtTemp = null;
            DACrux.SEMDMS.DSL.TQD_INSP_INFO oInspInfo = null;
            DACrux.SEMDMS.DSL.TQD_PRODUCT oPoduct = null;
            DACrux.SEMDMS.DSL.TQD_LOT oTQD_LOT = null;
            DACrux.SEMDMS.DSL.TQD_STEP oTQD_STEP = null;
            DACrux.SEMDMS.DSL.TQD_WAFER oTQD_WAFER = null;
            DACrux.SEMDMS.DSL.TQD_DEFECT oTQD_DEFECT = null;
            DACrux.SEMDMS.DSL.TQD_IMAGES oTQD_IMAGES = null;

            DMReview oReview;

            string strOriLotID = string.Empty;
            string strOriWaferId = string.Empty;
            int iOriSlotID = -1;
            string strOriStepID = string.Empty;
            string strOriProduct = string.Empty;
            string strSlotID = string.Empty;

            string strLotSeq = string.Empty;
            string strWaferSeq = string.Empty;

            string strNewLotSeq = string.Empty;
            string strNewWaferSeq = string.Empty;
            string strNewStepSeq = string.Empty;

            DACrux.SEMDMS.BSL.DefectDefine oDefectDefine = null;

            try
            {
                oInspInfo = new DACrux.SEMDMS.DSL.TQD_INSP_INFO();
                oPoduct = new DACrux.SEMDMS.DSL.TQD_PRODUCT();
                oTQD_LOT = new DACrux.SEMDMS.DSL.TQD_LOT();
                oTQD_STEP = new DACrux.SEMDMS.DSL.TQD_STEP();
                oTQD_WAFER = new DACrux.SEMDMS.DSL.TQD_WAFER();
                oTQD_DEFECT = new DACrux.SEMDMS.DSL.TQD_DEFECT();
                oTQD_IMAGES = new DACrux.SEMDMS.DSL.TQD_IMAGES();

                oDefectDefine = new DACrux.SEMDMS.BSL.DefectDefine();
                oReview = new DMReview();

                strSlotID = string.Format("{0:00}", iSlotID);
                //strWaferID = string.Format("{0}-{1}", strLotID, strSlotID);
                //=================================================================================================================================
                //Wafer List 정보를 가져 온다.
                //=================================================================================================================================
                dtWaferInfo = oInspInfo.GetInspIncludeLotSeq(strStepSeq);
                if (dtWaferInfo == null || dtWaferInfo.Rows.Count <= 0)
                    throw new Exception("정의된 Wafer 정보가 없습니다. TQD_INSP_INFO Empty");

                strOriLotID = dtWaferInfo.Rows[0]["LOT_ID"].ToString();
                strOriWaferId = dtWaferInfo.Rows[0]["WAFER_ID"].ToString();
                iOriSlotID = int.Parse(dtWaferInfo.Rows[0]["SLOT_ID"].ToString());
                strOriStepID = dtWaferInfo.Rows[0]["STEP_ID"].ToString();
                strOriProduct = dtWaferInfo.Rows[0]["PRODUCT"].ToString();

                strLotSeq = dtWaferInfo.Rows[0]["LOT_SEQ"].ToString();
                strWaferSeq = dtWaferInfo.Rows[0]["WAFER_SEQ"].ToString();

                //신규 Product 의 경우 기준정보는 그대로 하고 이름만 바꿔서 생성한다.
                dtTemp = oDefectDefine.GetProductInfo(strProduct);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                {
                    oPoduct.CopyInsertProduct(strProduct, strOriProduct);
                }

                //Lot Table 상의 정보 확인 
                dtTemp = oTQD_LOT.GetLotData(strProduct, strLotID);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                {
                    //없으면 생성 해준다.
                    oDefectDefine.CreateLotInfo(new string[] { strLotID, strProduct, "Maint Update" });
                    dtTemp = oTQD_LOT.GetLotData(strProduct, strLotID);
                }

                strNewLotSeq = dtTemp.Rows[0]["LOT_SEQ"].ToString();

                //TQD_WAFER Table 상의 정보 확인 
                dtTemp = oTQD_WAFER.GetWaferSeq(strNewLotSeq, strWaferID);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                {
                    //없으면 생성 해준다.
                    oDefectDefine.CreateWaferInfo(new string[] { strNewLotSeq, strWaferID, strSlotID });
                    dtTemp = oTQD_WAFER.GetWaferSeq(strNewLotSeq, strWaferID);
                }

                strNewWaferSeq = dtTemp.Rows[0]["WAFER_SEQ"].ToString();

                //TQD_STEP 정보를 Update 기준 정보로 조회후 없으면 생성 및 기존 Data 를 삭제 한다.
                dtTemp = oTQD_STEP.GetMaintData(strStepSeq, strNewWaferSeq, strStepID, strSlotID);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                {
                    strNewStepSeq = oTQD_STEP.SelectNextStpeSeq();

                    //없으면 기존 Step Seq 기반 재생성 후 기존 Data 는 삭제 한다.
                    oTQD_STEP.CreateMaintStep(strNewStepSeq, strNewWaferSeq, strStepID, strSlotID, strStepSeq);
                    oTQD_STEP.DeleteMaintStep(strStepSeq, strWaferSeq);

                    dtTemp = oTQD_STEP.GetMaintData(strNewStepSeq, strNewWaferSeq, strStepID, strSlotID);
                }

                strNewStepSeq = dtTemp.Rows[0]["STEP_SEQ"].ToString();

                //새로운 Seq 로 TQD_DEFECT Defect 정보가 없을 경우 기존 Defect 상의 Seq 들을 Update 해준다.
                int iDefectCount = oTQD_DEFECT.GetDefectDataCount(strNewWaferSeq, strNewStepSeq);
                if (iDefectCount <= 0)
                {
                    oTQD_DEFECT.UpdateDefectSeq(strNewStepSeq, strNewWaferSeq, strStepSeq, strWaferSeq);
                    oTQD_IMAGES.UpdateDefectSeq(strNewStepSeq, strNewWaferSeq, strStepSeq, strWaferSeq);
                }

                //InspInfo 정보를 조회 후 없으면 생성 및 Update 해준다.
                dtTemp = oInspInfo.GetStepWaferData(strNewStepSeq, strNewWaferSeq);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                {
                    oInspInfo.CreateMaintCopy(strNewStepSeq, strNewWaferSeq, strProduct, strLotID, strWaferID, strStepID, strSlotID, strStepSeq, strWaferSeq);

                    //기존 Data는 삭제 한다.
                    oInspInfo.DeleteInspMaint(strStepSeq, strWaferSeq);
                }
                else
                {
                    oInspInfo.UpdateInspMaint(strProduct, strLotID, strWaferID, strStepID, strSlotID, strNewStepSeq, strNewWaferSeq);
                }

                // TQD_INSP_SUM 테이블에 SUMMARY 데이터를 생성한다.
                oDefectDefine.CreateInspSum(strNewStepSeq);

                // TQD_REVIEW_SUM 데이터 생성
                oReview.InsertReviewSum(long.Parse(strNewStepSeq));

                string[] oMaint = new string[12];
                oMaint[0] = strLotID; //LOT_ID   
                oMaint[1] = strProduct; //PRODUCT
                oMaint[2] = strStepID; //STEP_ID
                oMaint[3] = strSlotID; //SLOT_ID
                oMaint[4] = strWaferID; //WAFER_ID
                oMaint[5] = strUser; //TRAN_USER
                oMaint[6] = strLocalIP; //TRAN_USER_IP
                oMaint[7] = string.Format("{0}, {1}, {2}, {3}, {4}", strOriLotID, iOriSlotID, strOriWaferId, strOriStepID, strOriProduct); //PREV_VALUE
                oMaint[8] = string.Format("{0}, {1}, {2}, {3}, {4}", strLotID, iSlotID, strWaferID, strStepID, strProduct); ;               //CURR_VALUE
                oMaint[9] = strStepSeq; //PREV_STEP_SEQ
                oMaint[10] = strNewStepSeq; //CURR_STEP_SEQ
                oMaint[11] = strComment; //USER_COMMENT
                InsertMaintHis(oMaint);

            }
            finally
            {
                if (dtWaferInfo != null)
                    dtWaferInfo.Dispose();

                if (dtTemp != null)
                    dtTemp.Dispose();
            }
        }

        public void SetDMMaintDataUpdateLot(string[] strStepSeq, string strCurrLotseq, string strLotID, string strOriLotID, string strStepID, string strProduct, string strUser, string strComment, string strLocalIP)
        {
            DataTable dtTemp = null;
            DACrux.SEMDMS.DSL.TQD_INSP_INFO oInspInfo = null;
            DACrux.SEMDMS.DSL.TQD_LOT oTQD_LOT = null;
            DACrux.SEMDMS.DSL.TQD_WAFER oTQD_WAFER = null;

            string strLotSeq = string.Empty;
            try
            {
                oInspInfo = new DACrux.SEMDMS.DSL.TQD_INSP_INFO();
                oTQD_LOT = new DACrux.SEMDMS.DSL.TQD_LOT();
                oTQD_WAFER = new DACrux.SEMDMS.DSL.TQD_WAFER();

                dtTemp = oTQD_LOT.GetLotDataSeq(strLotID, strProduct);
                if (dtTemp == null || dtTemp.Rows.Count <= 0)
                {
                    oTQD_LOT.Create(new string[] { strLotID, strProduct, "Maint Update" });
                    dtTemp = oTQD_LOT.GetLotDataSeq(strLotID, strProduct);
                }

                strLotSeq = dtTemp.Rows[0]["LOT_SEQ"].ToString();

                //Wafer 정보를 Update 한다. Wafer 의 Lot Seq 및 Wafer ID 에 Lot 가 들어가므로 Wafer ID 도 변경
                oTQD_WAFER.UpdateWaferMaint(strLotSeq, strLotID, string.Join(",", strStepSeq));

                //TQD_INSP_INFO 상의 Wafer ID 및 Lot ID 를 변경 한다.
                oInspInfo.UpdateMaintLotID(string.Join(",", strStepSeq), strLotID);

                string[] oMaint = new string[12];
                oMaint[0] = strLotID;                         //LOT_ID   
                oMaint[1] = strProduct;                       //PRODUCT
                oMaint[2] = strStepID;                        //STEP_ID
                oMaint[3] = "";                               //SLOT_ID
                oMaint[4] = "";                               //WAFER_ID
                oMaint[5] = strUser;                          //TRAN_USER
                oMaint[6] = strLocalIP;                       //TRAN_USER_IP
                oMaint[7] = string.Format("{0}", strLotID);   //PREV_VALUE
                oMaint[8] = string.Format("{0}", strOriLotID);//CURR_VALUE
                oMaint[9] = string.Join(",", strStepSeq);     //PREV_STEP_SEQ
                oMaint[10] = string.Join(",", strStepSeq);    //CURR_STEP_SEQ
                oMaint[11] = strComment;                      //USER_COMMENT
                InsertMaintHis(oMaint);

            }
            finally
            {

                if (dtTemp != null)
                    dtTemp.Dispose();
            }
        }

        /// <summary>
        /// LOT_ID, PRODUCT, STEP_ID, SLOT_ID, WAFER_ID, TRAN_USER, TRAN_USER_IP, PREV_VALUE, CURR_VALUE, PREV_STEP_SEQ, CURR_STEP_SEQ, USER_COMMENT
        /// </summary>
        /// <param name="strValue"></param>
        public void InsertMaintHis(string[] strValue)
        {
            DACrux.SEMDMS.DSL.TQD_DATA_MAINT_HIS oDataMaint = new DACrux.SEMDMS.DSL.TQD_DATA_MAINT_HIS();
            oDataMaint.InsertMaintHis(strValue);
        }

        public DataTable GetMaintHisTime(string strStarttime, string strEndTime)
        {
            DACrux.SEMDMS.DSL.TQD_DATA_MAINT_HIS oDataMaint = new DACrux.SEMDMS.DSL.TQD_DATA_MAINT_HIS();
            return oDataMaint.GetMaintHisTime(strStarttime, strEndTime);
        }

        public DataTable GetMaintHisLotID(string strLotID)
        {
            DACrux.SEMDMS.DSL.TQD_DATA_MAINT_HIS oDataMaint = new DACrux.SEMDMS.DSL.TQD_DATA_MAINT_HIS();
            return oDataMaint.GetMaintHisLotID(strLotID);
        }

        public void SetDMMaintDataDeleteWafer(string strStepSeq, string strUser, string strComment, string strLocalIP)
        {
            DataTable dtWaferInfo = null;
            DataTable dtTemp = null;
            DACrux.SEMDMS.DSL.TQD_INSP_INFO oInspInfo = null;
            DACrux.SEMDMS.DSL.TQD_STEP oTQD_STEP = null;
            DACrux.SEMDMS.DSL.TQD_DEFECT oTQD_DEFECT = null;
            DACrux.SEMDMS.DSL.TQD_IMAGES oTQD_IMAGES = null;

            string strLotID = string.Empty;
            string strWaferId = string.Empty;
            string strStepID = string.Empty;
            string strProduct = string.Empty;
            string strSlotID = string.Empty;
            string strWaferID = string.Empty;

            string strLotSeq = string.Empty;
            string strWaferSeq = string.Empty;

            DACrux.SEMDMS.BSL.DefectDefine oDefectDefine = null;

            try
            {
                oInspInfo = new DACrux.SEMDMS.DSL.TQD_INSP_INFO();
                oTQD_STEP = new DACrux.SEMDMS.DSL.TQD_STEP();
                oTQD_DEFECT = new DACrux.SEMDMS.DSL.TQD_DEFECT();
                oTQD_IMAGES = new DACrux.SEMDMS.DSL.TQD_IMAGES();

                oDefectDefine = new DACrux.SEMDMS.BSL.DefectDefine();

                //=================================================================================================================================
                //Wafer List 정보를 가져 온다.
                //=================================================================================================================================
                dtWaferInfo = oInspInfo.GetInspIncludeLotSeq(strStepSeq);
                if (dtWaferInfo == null || dtWaferInfo.Rows.Count <= 0)
                    throw new Exception("정의된 Wafer 정보가 없습니다. TQD_INSP_INFO Empty");

                strLotID = dtWaferInfo.Rows[0]["LOT_ID"].ToString();
                strWaferId = dtWaferInfo.Rows[0]["WAFER_ID"].ToString();
                strSlotID = dtWaferInfo.Rows[0]["SLOT_ID"].ToString();
                strStepID = dtWaferInfo.Rows[0]["STEP_ID"].ToString();
                strProduct = dtWaferInfo.Rows[0]["PRODUCT"].ToString();
                strLotSeq = dtWaferInfo.Rows[0]["LOT_SEQ"].ToString();
                strWaferSeq = dtWaferInfo.Rows[0]["WAFER_SEQ"].ToString();


                //TQD_STEP 삭제
                oTQD_STEP.DeleteMaintStep(strStepSeq, strWaferSeq);

                //TQD_DEFECT 삭제
                oTQD_DEFECT.DeleteDefectSeq(strStepSeq, strWaferSeq);

                //TQD_IMAGES 삭제
                oTQD_IMAGES.DeleteDefectImage(long.Parse(strStepSeq), long.Parse(strWaferSeq));

                //InspInfo 삭제
                oInspInfo.DeleteInspMaint(strStepSeq, strWaferSeq);


                string[] oMaint = new string[12];
                oMaint[0] = strLotID; //LOT_ID   
                oMaint[1] = strProduct; //PRODUCT
                oMaint[2] = strStepID; //STEP_ID
                oMaint[3] = strSlotID; //SLOT_ID
                oMaint[4] = strWaferID; //WAFER_ID
                oMaint[5] = strUser; //TRAN_USER
                oMaint[6] = strLocalIP; //TRAN_USER_IP
                oMaint[7] = string.Format("{0}, {1}, {2}, {3}, {4}", strLotID, strSlotID, strWaferId, strStepID, strProduct); //PREV_VALUE
                oMaint[8] = "Delete Wafer Base"; //CURR_VALUE
                oMaint[9] = strStepSeq; //PREV_STEP_SEQ
                oMaint[10] = strStepSeq; //CURR_STEP_SEQ
                oMaint[11] = "Data Delete :" + strComment; //USER_COMMENT
                InsertMaintHis(oMaint);

            }
            finally
            {
                if (dtWaferInfo != null)
                    dtWaferInfo.Dispose();

                if (dtTemp != null)
                    dtTemp.Dispose();
            }
        }

        public void SetDMMaintDataDeleteLot(string[] strStepSeqList, string strUser, string strComment, string strLocalIP)
        {
            DataTable dtWaferInfo = null;
            DataTable dtTemp = null;
            DACrux.SEMDMS.DSL.TQD_INSP_INFO oInspInfo = null;
            DACrux.SEMDMS.DSL.TQD_STEP oTQD_STEP = null;
            DACrux.SEMDMS.DSL.TQD_DEFECT oTQD_DEFECT = null;
            DACrux.SEMDMS.DSL.TQD_IMAGES oTQD_IMAGES = null;

            string strLotID = string.Empty;
            string strWaferId = string.Empty;
            string strStepID = string.Empty;
            string strProduct = string.Empty;
            string strSlotID = string.Empty;
            string strWaferID = string.Empty;

            string strLotSeq = string.Empty;
            string strWaferSeq = string.Empty;

            DACrux.SEMDMS.BSL.DefectDefine oDefectDefine = null;

            try
            {
                oInspInfo = new DACrux.SEMDMS.DSL.TQD_INSP_INFO();
                oTQD_STEP = new DACrux.SEMDMS.DSL.TQD_STEP();
                oTQD_DEFECT = new DACrux.SEMDMS.DSL.TQD_DEFECT();
                oTQD_IMAGES = new DACrux.SEMDMS.DSL.TQD_IMAGES();

                oDefectDefine = new DACrux.SEMDMS.BSL.DefectDefine();

                for (int ir = 0; ir < strStepSeqList.Length; ir++)
                {
                    string strStepSeq = strStepSeqList[ir];

                    //=================================================================================================================================
                    //Wafer List 정보를 가져 온다.
                    //=================================================================================================================================
                    dtWaferInfo = oInspInfo.GetInspIncludeLotSeq(strStepSeq);
                    if (dtWaferInfo == null || dtWaferInfo.Rows.Count <= 0)
                        throw new Exception("정의된 Wafer 정보가 없습니다. TQD_INSP_INFO Empty");

                    strLotID = dtWaferInfo.Rows[0]["LOT_ID"].ToString();
                    strWaferId = dtWaferInfo.Rows[0]["WAFER_ID"].ToString();
                    strSlotID = dtWaferInfo.Rows[0]["SLOT_ID"].ToString();
                    strStepID = dtWaferInfo.Rows[0]["STEP_ID"].ToString();
                    strProduct = dtWaferInfo.Rows[0]["PRODUCT"].ToString();
                    strLotSeq = dtWaferInfo.Rows[0]["LOT_SEQ"].ToString();
                    strWaferSeq = dtWaferInfo.Rows[0]["WAFER_SEQ"].ToString();


                    //TQD_STEP 삭제
                    oTQD_STEP.DeleteMaintStep(strStepSeq, strWaferSeq);

                    //TQD_DEFECT 삭제
                    oTQD_DEFECT.DeleteDefectSeq(strStepSeq, strWaferSeq);

                    //TQD_IMAGES 삭제
                    oTQD_IMAGES.DeleteDefectImage(long.Parse(strStepSeq), long.Parse(strWaferSeq));

                    //InspInfo 삭제
                    oInspInfo.DeleteInspMaint(strStepSeq, strWaferSeq);

                    string[] oMaint = new string[12];
                    oMaint[0] = strLotID; //LOT_ID   
                    oMaint[1] = strProduct; //PRODUCT
                    oMaint[2] = strStepID; //STEP_ID
                    oMaint[3] = strSlotID; //SLOT_ID
                    oMaint[4] = strWaferID; //WAFER_ID
                    oMaint[5] = strUser; //TRAN_USER
                    oMaint[6] = strLocalIP; //TRAN_USER_IP
                    oMaint[7] = string.Format("{0}, {1}, {2}, {3}, {4}", strLotID, strSlotID, strWaferId, strStepID, strProduct); //PREV_VALUE
                    oMaint[8] = "Delete Lot Base"; //CURR_VALUE
                    oMaint[9] = strStepSeq; //PREV_STEP_SEQ
                    oMaint[10] = strStepSeq; //CURR_STEP_SEQ
                    oMaint[11] = "Data Delete :" + strComment; //USER_COMMENT
                    InsertMaintHis(oMaint);
                }

            }
            finally
            {
                if (dtWaferInfo != null)
                    dtWaferInfo.Dispose();

                if (dtTemp != null)
                    dtTemp.Dispose();
            }
        }

        #endregion  DM Data Maint

        #region Pattern Search 관련 메서드

        public DataTable GetPatternSearch_WaferList(long[] stepSeqArr)
        {
            TQD_INSP_INFO obj = new TQD_INSP_INFO();
            return obj.GetPatternSearch_WaferList(stepSeqArr);
        }

        public List<PointF> GetPatternSearch_XYList(long stepSeq)
        {
            TQD_DEFECT obj = new TQD_DEFECT();
            DataTable dt = obj.GetPatternSearch_XYList(stepSeq);

            DataColumn colX = dt.Columns["X"];
            DataColumn colY = dt.Columns["Y"];
            
            List<PointF> list = new List<PointF>();

            foreach (DataRow row in dt.Rows)
            {
                float x = float.Parse(row[colX].ToString());
                float y = float.Parse(row[colY].ToString());

                list.Add(new PointF(x, y));
            }

            return list;
        }

        public Dictionary<long, List<PointF>> GetPatternSearch_XYList(long[] stepSeqArr)
        {
            TQD_DEFECT obj = new TQD_DEFECT();
            DataTable dt = obj.GetPatternSearch_XYList(stepSeqArr);
            
            // STEP_SEQ 단위로 분류
            Dictionary<long, List<PointF>> dic = new Dictionary<long, List<PointF>>();
            
            List<PointF> list = null;
            long prevStep = 0;

            DataColumn colStepSeq = dt.Columns["STEP_SEQ"];
            DataColumn colX = dt.Columns["X"];
            DataColumn colY = dt.Columns["Y"];

            foreach (DataRow row in dt.Rows)
            {
                long stepSeq = long.Parse(row[colStepSeq].ToString());
                float x = float.Parse(row[colX].ToString());
                float y = float.Parse(row[colY].ToString());

                if (stepSeq != prevStep)
                {
                    list = new List<PointF>();
                    dic[stepSeq] = list;
                    prevStep = stepSeq;
                }

                list.Add(new PointF(x, y));
            }

            return dic;
        }

        public byte[] GetPatternSearch_XYList_Comp(long[] stepSeqArr)
        {
            var obj = GetPatternSearch_XYList(stepSeqArr);
            return DACrux.Base.Util.ObjectToCompressedBytes(obj);
        }

        #endregion

        public DataTable GetLotInspInfo(string lotID, bool onlyLastInspection)
        {
            TQD_INSP_INFO obj = new TQD_INSP_INFO();
            return obj.GetLotinspInfo(lotID, onlyLastInspection);
        }

        public DataTable GetDefectImagePath02(string stepSeq)
        {
            TQD_IMAGES obj = new TQD_IMAGES();
            return obj.GetDefectImagePath02(stepSeq);
        }
    }
}
