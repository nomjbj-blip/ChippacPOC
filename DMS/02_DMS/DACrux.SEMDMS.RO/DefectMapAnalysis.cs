using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using DACrux.Base;
using System.Diagnostics;
using System.IO;

namespace DACrux.SEMDMS.RO
{
    public class DefectMapAnalysis
    {
        DACrux.SEMDMS.Interface.iDefectMapAnalysis m_OBJ;

        public DefectMapAnalysis()
        {
            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_DACRUX_DMS);
            object obj = Activator.GetObject(typeof(DACrux.SEMDMS.Interface.iDefectMapAnalysis),
                        strUrl + "/DACrux.SEMDMS.BSL.DefectMapAnalysis.bin");
            m_OBJ = obj as DACrux.SEMDMS.Interface.iDefectMapAnalysis;
        }

        public System.Data.DataTable GetStepInfo(long[] step_seq)
        {
            return m_OBJ.GetStepInfo(step_seq);
        }

        public System.Data.DataTable GetStepInfo(string[] step_seq)
        {
            return m_OBJ.GetStepInfo(step_seq);
        }

        public System.Data.DataTable GetStepInfoByStepSeq(string step_seq)
        {
            return m_OBJ.GetStepInfoByStepSeq(step_seq);
        }

        public System.Data.DataTable GetStepSum(long step_seq)
        {
            return m_OBJ.GetStepSum(step_seq);
        }

        public System.Data.DataTable GetStepList(string start, string end, string[] fieldsort, string[] where, string sort)
        {
            return m_OBJ.GetStepList(start, end, fieldsort, where, sort);
        }

        //public System.Data.DataTable GetScanSample(long step_seq)
        //{
        //    try
        //    {
        //        return m_OBJ.GetScanSample(step_seq);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public System.Data.DataTable GetSetupMap(long Setup_seq)
        {
            return m_OBJ.GetSetupMap(Setup_seq);
        }

        public System.Data.DataTable GetSetupRecipe(long setup_seq)
        {
            return m_OBJ.GetSetupRecipe(setup_seq);
        }

        public System.Data.DataTable GetDefectData(long step_seq)
        {
            return m_OBJ.GetDefectData(step_seq);
        }

        public DACrux.Base.DefectList GetDefectData(
            long[] stepseq
            )
        {
            return DataTableToDefectList(m_OBJ.GetDefectData(stepseq));
        }

        public DefectList GetDefectDataToObjectArray_Comp(long[] steps)
        {
            byte[] bytes = m_OBJ.GetDefectDataToObjectArray_Comp(steps);
            object[,] obj = Base.Util.CompressedBytesToObject(bytes) as object[,];
            return ObjectToDefectList(obj);
        }

        public object[,] GetDefectDataToObjectArray(long[] steps)
        {
            return m_OBJ.GetDefectDataToObjectArray(steps);
        }

        public static DefectList ObjectToDefectList(object[,] arr)
        {
            DefectList defects = new DefectList();

            int xLen = arr.GetLength(1);
            int yLen = arr.GetLength(0);

            Dictionary<string, int> headers = new Dictionary<string, int>();

            for (int x = 0; x < xLen; x++)
                headers.Add(arr[0, x].ToString(), x);

            //bool containsImagePath = headers.ContainsKey("IMAGE_PATH");
            //bool containsRepeatXRel = headers.ContainsKey("REPEAT_XREL");
            //bool containsRepeatYRel = headers.ContainsKey("REPEAT_YREL");

            for (int y = 1; y < yLen; y++)
            {
                Defect defect = new Defect();

                defect.STEP_SEQ = DACrux.Base.Convert.intParse(GetData(arr, headers, "STEP_SEQ", y));
                defect.DEFECTID = DACrux.Base.Convert.intParse(GetData(arr, headers, "DEFECTID", y));
                defect.WAFER_SEQ = DACrux.Base.Convert.intParse(GetData(arr, headers, "WAFER_SEQ", y));
                defect.X = DACrux.Base.Convert.doubleParse(GetData(arr, headers, "X", y));
                defect.Y = DACrux.Base.Convert.doubleParse(GetData(arr, headers, "Y", y));
                defect.XREL = DACrux.Base.Convert.doubleParse(GetData(arr, headers, "XREL", y));
                defect.YREL = DACrux.Base.Convert.doubleParse(GetData(arr, headers, "YREL", y));
                defect.XINDEX = DACrux.Base.Convert.intParse(GetData(arr, headers, "XINDEX", y));
                defect.YINDEX = DACrux.Base.Convert.intParse(GetData(arr, headers, "YINDEX", y));
                defect.XSIZE = DACrux.Base.Convert.doubleParse(GetData(arr, headers, "XSIZE", y));
                defect.YSIZE = DACrux.Base.Convert.doubleParse(GetData(arr, headers, "YSIZE", y));
                defect.DEFECTAREA = DACrux.Base.Convert.doubleParse(GetData(arr, headers, "DEFECTAREA", y));
                defect.DSIZE = DACrux.Base.Convert.doubleParse(GetData(arr, headers, "DSIZE", y));
                defect.CLASSNUMBER = DACrux.Base.Convert.intParse(GetData(arr, headers, "CLASSNUMBER", y));
                defect.TEST = DACrux.Base.Convert.intParse(GetData(arr, headers, "TEST", y));
                defect.CLUSTERNUMBER = DACrux.Base.Convert.intParse(GetData(arr, headers, "CLUSTERNUMBER", y));
                //defect.ROUGHBINNUMBER = DACrux.Base.Convert.intParse(GetData(arr, headers, "ROUGHBINNUMBER", y));
                //defect.FINEBINNUMBER = DACrux.Base.Convert.intParse(GetData(arr, headers, "FINEBINNUMBER", y));
                //defect.REVIEWSAMPLE = DACrux.Base.Convert.intParse(GetData(arr, headers, "REVIEWSAMPLE", y));
                defect.IMAGECOUNT = DACrux.Base.Convert.intParse(GetData(arr, headers, "IMAGECOUNT", y));
                defect.ADDER = DACrux.Base.Convert.intParse(GetData(arr, headers, "ADDER", y));
                //defect.FIRST_STEP = GetData(arr, headers, "FIRST_STEP", y);
                //defect.RETICLE_REPEAT_ID = DACrux.Base.Convert.intParse(GetData(arr, headers, "RETICLE_REPEAT_ID", y));
                //defect.DIE_REPEAT_ID = DACrux.Base.Convert.intParse(GetData(arr, headers, "DIE_REPEAT_ID", y));
                //defect.MAN_OPT_CLASS = DACrux.Base.Convert.intParse(GetData(arr, headers, "MAN_OPT_CLASS", y));
                //defect.AUTO_OPT_CLASS = DACrux.Base.Convert.intParse(GetData(arr, headers, "AUTO_OPT_CLASS", y));
                //defect.MAN_SEM_CLASS = DACrux.Base.Convert.intParse(GetData(arr, headers, "MAN_SEM_CLASS", y));
                //defect.AUTO_SEM_CLASS = DACrux.Base.Convert.intParse(GetData(arr, headers, "AUTO_SEM_CLASS", y));

                defects.Add(defect);
            }

            return defects;
        }

        public static DefectList DataTableToDefectList(DataTable dt)
        {
            DefectList defects = new DefectList();

            if (dt == null || dt.Rows.Count == 0)
                return defects;

            //bool containsImagePath = dt.Columns.Contains("IMAGE_PATH");
            //bool containsRepeatXRel = dt.Columns.Contains("REPEAT_XREL");
            //bool containsRepeatYRel = dt.Columns.Contains("REPEAT_YREL");

            foreach (DataRow r in dt.Rows)
            {
                Defect defect = new Defect();
                defect.STEP_SEQ = DACrux.Base.Convert.intParse(r["STEP_SEQ"].ToString());
                defect.DEFECTID = DACrux.Base.Convert.intParse(r["DEFECTID"].ToString());
                defect.WAFER_SEQ = DACrux.Base.Convert.intParse(r["WAFER_SEQ"].ToString());
                defect.X = DACrux.Base.Convert.doubleParse(r["X"].ToString());
                defect.Y = DACrux.Base.Convert.doubleParse(r["Y"].ToString());
                defect.XREL = DACrux.Base.Convert.doubleParse(r["XREL"].ToString());
                defect.YREL = DACrux.Base.Convert.doubleParse(r["YREL"].ToString());
                defect.XINDEX = DACrux.Base.Convert.intParse(r["XINDEX"].ToString());
                defect.YINDEX = DACrux.Base.Convert.intParse(r["YINDEX"].ToString());
                defect.XSIZE = DACrux.Base.Convert.doubleParse(r["XSIZE"].ToString());
                defect.YSIZE = DACrux.Base.Convert.doubleParse(r["YSIZE"].ToString());
                //defect.DEFECTAREA = DACrux.Base.Convert.doubleParse(r["DEFECTAREA"].ToString());
                defect.DSIZE = DACrux.Base.Convert.doubleParse(r["DSIZE"].ToString());
                defect.CLASSNUMBER = DACrux.Base.Convert.intParse(r["CLASSNUMBER"].ToString());
                defect.TEST = DACrux.Base.Convert.intParse(r["TEST"].ToString());
                defect.CLUSTERNUMBER = DACrux.Base.Convert.intParse(r["CLUSTERNUMBER"].ToString());
                //defect.ROUGHBINNUMBER = DACrux.Base.Convert.intParse(r["ROUGHBINNUMBER"].ToString());
                //defect.FINEBINNUMBER = DACrux.Base.Convert.intParse(r["FINEBINNUMBER"].ToString());
                //defect.REVIEWSAMPLE = DACrux.Base.Convert.intParse(r["REVIEWSAMPLE"].ToString());
                defect.IMAGECOUNT = DACrux.Base.Convert.intParse(r["IMAGECOUNT"].ToString());
                defect.ADDER = DACrux.Base.Convert.intParse(r["ADDER"].ToString());
                //defect.FIRST_STEP = r["FIRST_STEP"].ToString();
                //defect.RETICLE_REPEAT_ID = DACrux.Base.Convert.intParse(r["RETICLE_REPEAT_ID"].ToString());
                //defect.DIE_REPEAT_ID = DACrux.Base.Convert.intParse(r["DIE_REPEAT_ID"].ToString());
                //defect.MAN_OPT_CLASS = DACrux.Base.Convert.intParse(r["MAN_OPT_CLASS"].ToString());
                //defect.AUTO_OPT_CLASS = DACrux.Base.Convert.intParse(r["AUTO_OPT_CLASS"].ToString());
                //defect.MAN_SEM_CLASS = DACrux.Base.Convert.intParse(r["MAN_SEM_CLASS"].ToString());
                //defect.AUTO_SEM_CLASS = DACrux.Base.Convert.intParse(r["AUTO_SEM_CLASS"].ToString());

                //if (containsImagePath && r["IMAGE_PATH"] != DBNull.Value)
                //{
                //    defect.IMAGEURL = string.Format("{0}/{1}", r["IMAGE_PATH"].ToString().Trim(), r["IMAGE_FILENAME"].ToString().Trim()).Trim();
                //}

                //try catch 로 하면 속도가 너무 느리다.. 
                //if (containsRepeatXRel)
                //    defect.REPEAT_XREL = DACrux.Base.Convert.intParse(r["REPEAT_XREL"].ToString());

                //if (containsRepeatYRel)
                //    defect.REPEAT_YREL = DACrux.Base.Convert.intParse(r["REPEAT_YREL"].ToString());
                defects.Add(defect);
            }

            return defects;
        }

        private static string GetData(string[,] arr, Dictionary<string, int> header, string column, int row)
        {
            return arr[row, header[column]];
        }

        private static string GetData(object[,] arr, Dictionary<string, int> header, string column, int row)
        {
            return arr[row, header[column]].ToString();
        }

        public System.Data.DataTable GetDensity(long[] step_seq, string SelDefects = "ALL")
        {
            return m_OBJ.GetDensity(step_seq, SelDefects);
        }

        public System.Data.DataTable GetDefectDataCnt(long[] step_seqs, string column)
        {
            return m_OBJ.GetDefectCount(step_seqs, column);
        }

        public System.Data.DataTable GetDefectImage(long step_seq, int[] defectid = null)
        {
            return m_OBJ.GetDefectImage(step_seq, defectid);
        }

        public System.Data.DataTable GetShotRepeatDefect(long step_seq, string product, float die_size_x, float die_size_y, float tolerance)
        {
            return m_OBJ.GetShotRepeatDefect(step_seq, product, die_size_x, die_size_y, tolerance);
        }

        public System.Data.DataTable GetDieRepeatDefect(long step_seq, float tolerance)
        {
            return m_OBJ.GetDieRepeatDefect(step_seq, tolerance);
        }

        public System.Data.DataTable GetSize(string userid)
        {
            return m_OBJ.GetSize(userid);
        }

        public System.Data.DataTable SelectColorByDefectSize(string userid)
        {
            return m_OBJ.SelectColorByDefectSize(userid);
        }

        public Dictionary<int, Color> GetDefectSizeColor(
            string userid)
        {
            return m_OBJ.GetDefectSizeColor(userid);
        }

        public void GetFirstStepCnt(long step_seq, ref int newCnt, ref int carryOverCnt, ref int missingCnt)
        {
            m_OBJ.GetFirstStepCnt(step_seq, ref  newCnt, ref  carryOverCnt, ref  missingCnt);
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
            m_OBJ.GetDsaCnt(
                first_step_seq,
                step_seq,
                tolerance,
                ref  newCnt,
                ref  carryOverCnt,
                ref  missingCnt
                );
        }

        public System.Data.DataTable GetFirstStep(long step_seq)
        {
            return m_OBJ.GetFirstStep(step_seq);
        }

        public System.Data.DataTable GetDsa(long first_step_seq, long step_seq, float tolerance)
        {
            return m_OBJ.GetDsa(first_step_seq, step_seq, tolerance);
        }

        public System.Data.DataTable GetDefectImageInfo(long step_seq)
        {
            return m_OBJ.GetDefectImageInfo(step_seq);
        }

        public DataTable GetStepListByStepID(string TestArea, string StartTime, string EndTime, string LikeDevice, string LikeLot, string LikeWafer)
        {
            return m_OBJ.GetStepListByStepID(TestArea, StartTime, EndTime, LikeDevice, LikeLot, LikeWafer);
        }

        public System.Data.DataTable GetInspInfoByLotId(string LotId)
        {
            return m_OBJ.GetInspInfoByLotId(LotId);
        }

        public System.Data.DataTable SelectMapParsingResultList()
        {
            return m_OBJ.SelectMapParsingResultList();
        }

        public System.Data.DataTable SelectMapParsingResultLotList()
        {
            return m_OBJ.SelectParsingResultLotList();
        }

        public System.Data.DataTable SelectMapParsingResultWaferList()
        {
            return m_OBJ.SelectParsingResultWaferList();
        }

        public System.Data.DataTable SelectMapParsingResultStepList()
        {
            return m_OBJ.SelectParsingResultStepList();
        }

        public System.Data.DataTable SelectMapParsingResultInspEqList()
        {
            return m_OBJ.SelectParsingResultInspEqList();
        }

        public System.Data.DataTable SelectMapParsingResultByItem(string lotId, string waferId, string stepId, string inspEq)
        {
            return m_OBJ.SelectParsingResultByItem(lotId, waferId, stepId, inspEq);
        }

        #region ' Repeat Defect Prepare '
        public DataTable GetDieRepeatDefect(int stepSeq, int tolerance)
        {
            return m_OBJ.GetDieRepeatDefect(stepSeq, tolerance);
        }

        public DataTable GetShotRepeatDefect(int stepSeq, string product, int dieSizeX, int dieSizeY, int tolerance)
        {
            return m_OBJ.GetShotRepeatDefect(stepSeq, product, dieSizeX, dieSizeY, tolerance);
        }

        public DataTable GetRepeatListByShot(int dieSizeX, int dieSizeY, int tolerance, int stepSeq, string product, int repeatCnt)
        {
            return m_OBJ.GetRepeatListByShot(dieSizeX, dieSizeY, tolerance, stepSeq, product, repeatCnt);
        }

        public DataTable GetRepeatListByDie(int tolerance, int stepSeq, int repeatCnt)
        {
            return m_OBJ.GetRepeatListByDie(tolerance, stepSeq, repeatCnt);
        }
        #endregion ===============================================================

        public DataTable GetWaferDefectOverlay(string program, string[] waferSeq)
        {
            return m_OBJ.GetWaferDefectOverlay(program, waferSeq);
        }

        public DataTable GetStepInfoOverlay(string lotId, string waferId)
        {
            return m_OBJ.GetStepInfoOverlay(lotId, waferId);
        }

        public DataTable SelectProduct(string LotID, string TestArea, string p)
        {
            throw new NotImplementedException();
        }

        public DataSet GetStepListByAll(string m_TestArea, string m_Device, string m_CusLotID, string waferList)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable GetImageGallery(long[] step_seqs)
        {
            return m_OBJ.GetImageGallery(step_seqs);
        }

        #region 동부 하이텍

        public System.Data.DataTable GetColorByDefectType(
            )
        {
            return m_OBJ.GetColorByDefectType();
        }


        public System.Data.DataSet GetDMSStepWaferList(string start, string end, string[] wheres, bool TimeNotCheck, bool bLastInspection)
        {
            try
            {
                return m_OBJ.GetDMSStepWaferList(start, end, wheres, TimeNotCheck, bLastInspection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Obsolete("out of memory 문제로 사용금지", true)]
        public System.Data.DataSet GetDefectMapViewer(long[] step_seqs)
        {
            byte[] bytes = m_OBJ.GetDefectMapViewer(step_seqs);
            return DACrux.Base.Util.CompressedBytesToObject(bytes) as DataSet;
        }

        public DataSet GetBareDefectMapViewer(long[] step_seqs, string strMapID = "")
        {
            return m_OBJ.GetBareDefectMapViewer(step_seqs, strMapID);
        }

        public System.Data.DataTable GetShotDeviceList()
        {
            return m_OBJ.GetShotDeviceList();
        }

        public System.Data.DataTable GetMapIDListNotDelete()
        {
            return m_OBJ.GetMapIDListNotDelete();
        }

        public System.Data.DataSet GetDefectMapInfo(long step_seq)
        {
            return m_OBJ.GetDefectMapInfo(step_seq);
        }

        public DataTable GetDefectClassInfo()
        {
            return m_OBJ.GetDefectClassInfo();
        }

        public Dictionary<string, Color> GetDefectClassColor(
            )
        {
            return m_OBJ.GetDefectClassColor();
        }

        public DataTable GetDSAReport(long[] step_seqs, string strTol)
        {
            return m_OBJ.GetDSAReport(step_seqs, strTol);
        }

        public System.Data.DataSet GetBasicMapViewer(long[] step_seqs)
        {
            return m_OBJ.GetBasicMapViewer(step_seqs);
        }

        //--

        public DataSet GetChartAnalysis(
            long[] steps
            )
        {
            return m_OBJ.GetChartAnalysis(
                steps
                );
        }

        public DefectList GetDefectImages(
            long[] steps
            )
        {
            return GetDefectImages(m_OBJ.GetDefectImages(
                steps
                ));
        }

        public DefectList GetDefectImages(
            DataTable dt
            )
        {
            DefectList defects = new DefectList();
            foreach (DataRow r in dt.Rows)
            {
                int findIndex = defects.BinarySearch(DACrux.Base.Convert.longParse(r["STEP_SEQ"].ToString()), DACrux.Base.Convert.intParse(r["DEFECTID"].ToString()));
                if (findIndex >= 0)
                {
                    defects[findIndex].Images.Add(DACrux.Base.Convert.intParse(r["IMAGE_ID"].ToString()), String.Format("{0}/{1}", r["THUMB_PATH"], r["THUMB_FILENAME"]));
                    defects[findIndex].IMAGECOUNT = defects[findIndex].Images.Count;
                    continue;
                }

                Defect defect = new Defect();
                defect.STEP_SEQ = DACrux.Base.Convert.intParse(r["STEP_SEQ"].ToString());
                defect.DEFECTID = DACrux.Base.Convert.intParse(r["DEFECTID"].ToString());
                defect.WAFER_SEQ = DACrux.Base.Convert.intParse(r["WAFER_SEQ"].ToString());
                defect.X = DACrux.Base.Convert.doubleParse(r["X"].ToString());
                defect.Y = DACrux.Base.Convert.doubleParse(r["Y"].ToString());
                defect.XREL = DACrux.Base.Convert.doubleParse(r["XREL"].ToString());
                defect.YREL = DACrux.Base.Convert.doubleParse(r["YREL"].ToString());
                defect.XINDEX = DACrux.Base.Convert.intParse(r["XINDEX"].ToString());
                defect.YINDEX = DACrux.Base.Convert.intParse(r["YINDEX"].ToString());
                defect.XSIZE = DACrux.Base.Convert.doubleParse(r["XSIZE"].ToString());
                defect.YSIZE = DACrux.Base.Convert.doubleParse(r["YSIZE"].ToString());
                defect.DEFECTAREA = DACrux.Base.Convert.doubleParse(r["DEFECTAREA"].ToString());
                defect.DSIZE = DACrux.Base.Convert.doubleParse(r["DSIZE"].ToString());
                defect.CLASSNUMBER = DACrux.Base.Convert.intParse(r["CLASSNUMBER"].ToString());
                defect.TEST = DACrux.Base.Convert.intParse(r["TEST"].ToString());
                defect.CLUSTERNUMBER = DACrux.Base.Convert.intParse(r["CLUSTERNUMBER"].ToString());
                defect.ROUGHBINNUMBER = DACrux.Base.Convert.intParse(r["ROUGHBINNUMBER"].ToString());
                defect.FINEBINNUMBER = DACrux.Base.Convert.intParse(r["FINEBINNUMBER"].ToString());
                defect.REVIEWSAMPLE = DACrux.Base.Convert.intParse(r["REVIEWSAMPLE"].ToString());
                defect.ADDER = DACrux.Base.Convert.intParse(r["ADDER"].ToString());
                defect.FIRST_STEP = r["FIRST_STEP"].ToString();
                defect.RETICLE_REPEAT_ID = DACrux.Base.Convert.intParse(r["RETICLE_REPEAT_ID"].ToString());
                defect.DIE_REPEAT_ID = DACrux.Base.Convert.intParse(r["DIE_REPEAT_ID"].ToString());
                defect.MAN_OPT_CLASS = DACrux.Base.Convert.intParse(r["MAN_OPT_CLASS"].ToString());
                defect.AUTO_OPT_CLASS = DACrux.Base.Convert.intParse(r["AUTO_OPT_CLASS"].ToString());
                defect.MAN_SEM_CLASS = DACrux.Base.Convert.intParse(r["MAN_SEM_CLASS"].ToString());
                defect.AUTO_SEM_CLASS = DACrux.Base.Convert.intParse(r["AUTO_SEM_CLASS"].ToString());
                defect.IMAGEURL = r["IMAGE_PATH"].ToString();

                //--

                defect.Images.Add(DACrux.Base.Convert.intParse(r["IMAGE_ID"].ToString()), String.Format("{0}/{1}", r["THUMB_PATH"], r["THUMB_FILENAME"]));
                //defect.IMAGECOUNT = DACrux.Base.Convert.intParse(r["IMAGECOUNT"].ToString());
                defect.IMAGECOUNT = defect.Images.Count;

                //--

                defects.Add(defect);
            }

            return defects;
        }

        public DefectList SetReclassifyDefectImageList(
            long[] steps,
            string[,] Params,
            string userId,
            string ipAddr
            )
        {
            return this.GetDefectImages(
                m_OBJ.SetReclassifyDefectImageList(steps, Params, userId, ipAddr)
                );
        }

        public DefectList SetReclassifyDefectList_Comp(long[] steps, string[,] Params, string userId, string ipAddr)
        {
            byte[] bytes = m_OBJ.SetReclassifyDefectList_Comp(steps, Params, userId, ipAddr);
            object[,] data = Base.Util.CompressedBytesToObject(bytes) as object[,];
            return ObjectToDefectList(data);
        }

        public DefectList SetReclassifyDefectList(long[] steps, string[,] Params, string userId, string ipAddr)
        {
            object[,] data = m_OBJ.SetReclassifyDefectList(steps, Params, userId, ipAddr);
            return ObjectToDefectList(data);
        }

        public object SetRemoveDefectImageList(long[] steps, string[,] Params, string userId, string ipAddr)
        {
            return m_OBJ.SetRemoveDefectImageList(steps, Params, userId, ipAddr);
        }

        //public Color GetDieBackColor(string factory, string userID)
        //{
        //    return m_OBJ.GetDieBackColor(factory, userID);
        //}

        public DataTable GetWaferInfo(long[] stepSeqArr)
        {
            return m_OBJ.GetWaferInfo(stepSeqArr);
        }

        public DataSet GetDefectMapViewer_Info(long[] step_seqs)
        {
            return m_OBJ.GetDefectMapViewer_Info(step_seqs);
        }

        public DataTable GetDefectMapViewer_Map(long[] step_seqs, string[] setupseq, string[] test)
        {
            byte[] bytes = m_OBJ.GetDefectMapViewer_Map(step_seqs, setupseq, test);
            return DACrux.Base.Util.CompressedBytesToObject(bytes) as DataTable;
        }

        public DataTable GetDefectMapViewer_Defects(long[] step_seqs, bool bAdder = false)
        {
            byte[] bytes = m_OBJ.GetDefectMapViewer_Defects3_Comp(step_seqs, bAdder);
            object[,] arr = DACrux.Base.Util.CompressedBytesToObject(bytes) as object[,];

            DataTable dt = ArrayToDataTable(arr);

            DataTable infoDt = GetInspInfo01(step_seqs);
            Dictionary<decimal, string> waferDic = new Dictionary<decimal, string>();
            Dictionary<decimal, string> stepDic = new Dictionary<decimal, string>();
            Dictionary<decimal, DateTime> timeDic = new Dictionary<decimal, DateTime>();

            foreach (DataRow row in infoDt.Rows)
            {
                decimal stepSeq = (decimal)row["STEP_SEQ"];
                waferDic[stepSeq] = row["WAFER_ID"].ToString();
                stepDic[stepSeq] = row["STEP_ID"].ToString();
                timeDic[stepSeq] = (DateTime)row["RESULTTIMESTAMP"];
            }

            // 테이블에 D.WAFER_ID, D.STEP_ID, D.RESULTTIMESTAMP 데이터 추가
            DataColumn waferCol = dt.Columns.Add("WAFER_ID", typeof(string));
            DataColumn stepCol = dt.Columns.Add("STEP_ID", typeof(string));
            DataColumn timeCol = dt.Columns.Add("RESULTTIMESTAMP", typeof(DateTime));

            foreach (DataRow row in dt.Rows)
            {
                decimal stepSeq = (decimal)row["STEP_SEQ"];
                row[waferCol] = waferDic[stepSeq];
                row[stepCol] = stepDic[stepSeq];
                row[timeCol] = timeDic[stepSeq];
            }

            return dt;

            //byte[] bytes = m_OBJ.GetDefectMapViewer_Defects(step_seqs, bAdder);
            //return DACrux.Base.Util.CompressedBytesToObject(bytes) as DataTable;
        }

        public Defect[] GetDefectMapViewer_DefectArray(long[] step_seqs, bool bAdder = false)
        {
            byte[] bytes = m_OBJ.GetDefectMapViewer_Defects3_Comp(step_seqs, bAdder);
            object[,] arr = DACrux.Base.Util.CompressedBytesToObject(bytes) as object[,];

            DataTable infoDt = GetInspInfo01(step_seqs);
            Dictionary<decimal, string> waferDic = new Dictionary<decimal, string>();
            Dictionary<decimal, string> stepDic = new Dictionary<decimal, string>();
            Dictionary<decimal, DateTime> timeDic = new Dictionary<decimal, DateTime>();

            // 헤더만 있는 경우
            if (arr == null || arr.Length == 1)
                return new Defect[] { };

            int xLen = arr.GetLength(1);
            int yLen = arr.GetLength(0);

            Defect[] defectArr = new Defect[yLen - 1];

            foreach (DataRow row in infoDt.Rows)
            {
                decimal stepSeq = (decimal)row["STEP_SEQ"];
                waferDic[stepSeq] = row["WAFER_ID"].ToString();
                stepDic[stepSeq] = row["STEP_ID"].ToString();
                timeDic[stepSeq] = (DateTime)row["RESULTTIMESTAMP"];
            }

            Dictionary<string, int> headers = new Dictionary<string, int>();

            for (int x = 0; x < xLen; x++)
                headers.Add(arr[0, x].ToString(), x);

            for (int y = 1; y < yLen; y++)
            {
                DefectEx defect = new DefectEx();

                defect.STEP_SEQ = DACrux.Base.Convert.intParse(GetData(arr, headers, "STEP_SEQ", y));
                defect.DEFECTID = DACrux.Base.Convert.intParse(GetData(arr, headers, "DEFECTID", y));
                defect.WAFER_SEQ = DACrux.Base.Convert.intParse(GetData(arr, headers, "WAFER_SEQ", y));
                defect.X = DACrux.Base.Convert.doubleParse(GetData(arr, headers, "X", y));
                defect.Y = DACrux.Base.Convert.doubleParse(GetData(arr, headers, "Y", y));
                defect.XREL = DACrux.Base.Convert.doubleParse(GetData(arr, headers, "XREL", y));
                defect.YREL = DACrux.Base.Convert.doubleParse(GetData(arr, headers, "YREL", y));
                defect.XINDEX = DACrux.Base.Convert.intParse(GetData(arr, headers, "XINDEX", y));
                defect.YINDEX = DACrux.Base.Convert.intParse(GetData(arr, headers, "YINDEX", y));
                defect.XSIZE = DACrux.Base.Convert.doubleParse(GetData(arr, headers, "XSIZE", y));
                defect.YSIZE = DACrux.Base.Convert.doubleParse(GetData(arr, headers, "YSIZE", y));
                defect.DEFECTAREA = DACrux.Base.Convert.doubleParse(GetData(arr, headers, "DEFECTAREA", y));
                defect.DSIZE = DACrux.Base.Convert.doubleParse(GetData(arr, headers, "DSIZE", y));
                defect.CLASSNUMBER = DACrux.Base.Convert.intParse(GetData(arr, headers, "CLASSNUMBER", y));
                defect.TEST = DACrux.Base.Convert.intParse(GetData(arr, headers, "TEST", y));
                defect.CLUSTERNUMBER = DACrux.Base.Convert.intParse(GetData(arr, headers, "CLUSTERNUMBER", y));
                //defect.ROUGHBINNUMBER = DACrux.Base.Convert.intParse(GetData(arr, headers, "ROUGHBINNUMBER", y));
                //defect.FINEBINNUMBER = DACrux.Base.Convert.intParse(GetData(arr, headers, "FINEBINNUMBER", y));
                //defect.REVIEWSAMPLE = DACrux.Base.Convert.intParse(GetData(arr, headers, "REVIEWSAMPLE", y));
                defect.IMAGECOUNT = DACrux.Base.Convert.intParse(GetData(arr, headers, "IMAGECOUNT", y));
                defect.ADDER = DACrux.Base.Convert.intParse(GetData(arr, headers, "ADDER", y));
                //defect.FIRST_STEP = GetData(arr, headers, "FIRST_STEP", y);
                //defect.RETICLE_REPEAT_ID = DACrux.Base.Convert.intParse(GetData(arr, headers, "RETICLE_REPEAT_ID", y));
                //defect.DIE_REPEAT_ID = DACrux.Base.Convert.intParse(GetData(arr, headers, "DIE_REPEAT_ID", y));
                //defect.MAN_OPT_CLASS = DACrux.Base.Convert.intParse(GetData(arr, headers, "MAN_OPT_CLASS", y));
                //defect.AUTO_OPT_CLASS = DACrux.Base.Convert.intParse(GetData(arr, headers, "AUTO_OPT_CLASS", y));
                //defect.MAN_SEM_CLASS = DACrux.Base.Convert.intParse(GetData(arr, headers, "MAN_SEM_CLASS", y));
                //defect.AUTO_SEM_CLASS = DACrux.Base.Convert.intParse(GetData(arr, headers, "AUTO_SEM_CLASS", y));

                defect.WAFER_ID = waferDic[defect.STEP_SEQ];
                defect.STEP_ID = stepDic[defect.STEP_SEQ];
                defect.RESULTTIMESTAMP = timeDic[defect.STEP_SEQ];

                defectArr[y - 1] = defect;
            }

            return defectArr;
        }

        public List<object[]> GetDefectMapViewer_Defects2(long[] step_seqs, bool bAdder)
        {
            return m_OBJ.GetDefectMapViewer_Defects2(step_seqs, bAdder);
        }

        public List<object[]> GetDefectMapViewer_Defects2_Comp(long[] step_seqs, bool bAdder)
        {
            byte[] bytes = m_OBJ.GetDefectMapViewer_Defects2_Comp(step_seqs, bAdder);
            return DACrux.Base.Util.CompressedBytesToObject(bytes) as List<object[]>;
        }

        public object[,] GetDefectMapViewer_Defects3(long[] step_seqs, bool bAdder)
        {
            return m_OBJ.GetDefectMapViewer_Defects3(step_seqs, bAdder);
        }

        public object[,] GetDefectMapViewer_Defects3_Comp(long[] step_seqs, bool bAdder)
        {
            byte[] bytes = m_OBJ.GetDefectMapViewer_Defects3_Comp(step_seqs, bAdder);
            return DACrux.Base.Util.CompressedBytesToObject(bytes) as object[,];
        }

        public DataTable GetInspInfo01(long[] stepSeqArr)
        {
            return m_OBJ.GetInspInfo01(stepSeqArr);
        }

        public DataTable GetInspInfo02(long[] stepSeqArr)
        {
            return m_OBJ.GetInspInfo02(stepSeqArr);
        }

        public DataTable GetDefectImagePath01(long[] stepSeqArr)
        {
            return m_OBJ.GetDefectImagePath01(stepSeqArr);
        }

        public DataTable GetDefectMapViewer_Chart(long[] step_seqs, bool bAdder)
        {
            byte[] bytes = m_OBJ.GetDefectMapViewer_Chart(step_seqs, bAdder);
            return DACrux.Base.Util.CompressedBytesToObject(bytes) as DataTable;
        }

        public DataTable ArrayToDataTable(object[,] arr)
        {
            if (arr == null || arr.Length == 0)
                return null;

            int xLen = arr.GetLength(1);
            int yLen = arr.GetLength(0);

            // 첫줄은 헤더
            DataTable dt = new DataTable();

            for (int i = 0; i < xLen; i++)
            {
                Type t = yLen > 1 ? arr[1, i].GetType() : typeof(string);

                dt.Columns.Add(arr[0, i].ToString(), t);
            }

            // 헤더만 있고 데이터는 없는 경우
            if (arr.Length == 1)
                return dt;

            for (int y = 1; y < yLen; y++)
            {
                DataRow row = dt.NewRow();

                for (int x = 0; x < xLen; x++)
                    row[x] = arr[y, x];

                dt.Rows.Add(row);
            }

            dt.AcceptChanges();
            arr = null;
            return dt;
        }

        #endregion


        public string GetImagePath(
            long stepseq,
            long waferseq,
            int defectid,
            int imageid
            )
        {
            return m_OBJ.GetImagePath(
                stepseq,
                waferseq,
                defectid,
                imageid
                );
        }

        #region DM Data Maint

        public System.Data.DataSet GetDMMaintDataList(long[] step_seqs)
        {
            return m_OBJ.GetDMMaintDataList(step_seqs);
        }

        public void SetDMMaintDataUpdateWafer(string strStepSeq, string strWaferID, string strLotID, int iSlotID, string strStepID, string strProduct, string strUser, string strComment, string strLocalIP)
        {
            m_OBJ.SetDMMaintDataUpdateWafer(strStepSeq, strWaferID, strLotID, iSlotID, strStepID, strProduct, strUser, strComment, strLocalIP);
        }

        public void SetDMMaintDataUpdateLot(string[] strStepSeq, string strLotseq, string strLotID, string strOriLotID, string strStepID, string strProduct, string strUser, string strComment, string strLocalIP)
        {
            m_OBJ.SetDMMaintDataUpdateLot(strStepSeq, strLotseq, strLotID, strOriLotID, strStepID, strProduct, strUser, strComment, strLocalIP);
        }


        public void InsertMaintHis(string[] strValue)
        {
            m_OBJ.InsertMaintHis(strValue);
        }

        public DataTable GetMaintHisTime(string strStarttime, string strEndTime)
        {
            return m_OBJ.GetMaintHisTime(strStarttime, strEndTime);
        }

        public DataTable GetMaintHisLotID(string strLotID)
        {
            return m_OBJ.GetMaintHisLotID(strLotID);
        }


        public void SetDMMaintDataDeleteWafer(string strStepSeq, string strUser, string strComment, string strLocalIP)
        {
            m_OBJ.SetDMMaintDataDeleteWafer(strStepSeq, strUser, strComment, strLocalIP);
        }

        public void SetDMMaintDataDeleteLot(string[] strStepSeqList, string strUser, string strComment, string strLocalIP)
        {
            m_OBJ.SetDMMaintDataDeleteLot(strStepSeqList, strUser, strComment, strLocalIP);
        }
        #endregion  DM Data Maint

        #region Pattern Search 관련 메서드

        public DataTable GetPatternSearch_WaferList(long[] stepSeqArr)
        {
            return m_OBJ.GetPatternSearch_WaferList(stepSeqArr);
        }

        public List<PointF> GetPatternSearch_XYList(long stepSeq)
        {
            return m_OBJ.GetPatternSearch_XYList(stepSeq);
        }

        public Dictionary<long, List<PointF>> GetPatternSearch_XYList(long[] stepSeqArr)
        {
            return m_OBJ.GetPatternSearch_XYList(stepSeqArr);
        }

        public Dictionary<long, List<PointF>> GetPatternSearch_XYList_Comp(long[] stepSeqArr)
        {
            byte[] bytes = m_OBJ.GetPatternSearch_XYList_Comp(stepSeqArr);
            return DACrux.Base.Util.CompressedBytesToObject(bytes) as Dictionary<long, List<PointF>>;
        }

        #endregion

        #region Lot 별 압축 파일 생성 관련 메서드

        /// <summary>
        /// Lot 별 Zip 파일을 생성합니다.
        /// </summary>
        /// <param name="lotID">Lot ID</param>
        /// <param name="saveFolder">압축 파일을 저장할 디렉토리</param>
        /// <param name="onlyLastInspection">마지막 Inspection만을 포함할지 여부. 기본값은 false</param>
        /// <param name="statusMessage">진행 상태를 나타내는 메시지를 수신 할 때. 기본값은 수신하지 않음</param>
        public void MakeZipFile(string lotID, string saveFolder, bool onlyLastInspection = false, Action<string> statusMessage = null)
        {
            if (String.IsNullOrEmpty(lotID))
                throw new Exception("Lot ID가 null 입니다.");

            if (String.IsNullOrEmpty(saveFolder))
                throw new Exception("압축파일을 저장할 폴더명이 없습니다.");

            DataTable dt = m_OBJ.GetLotInspInfo(lotID, onlyLastInspection);
            string tempPath = null;

            if (dt == null || dt.Rows.Count == 0)
                return;

            DmsCache.Instance.ClassLookupDel += DefectMapAnalysis.GetClassLookup;
            DmsCache.Instance.WaferDieInfoDel += DefectMapAnalysis.GetWaferDieInfo;

            try
            {
                // 임시 디렉토리 생성
                tempPath = Path.GetTempFileName();
                File.Delete(tempPath);
                Directory.CreateDirectory(tempPath);

                DACrux.Common.RO.ComConfiguration con = new DACrux.Common.RO.ComConfiguration();

                DACrux.Utility.HFtpClient ftp;

                // FTP 접속 정보 설정
                using (DataTable dtConfig = con.GetDefectFTP())
                {
                    if (dtConfig == null || dtConfig.Rows.Count == 0)
                        return;

                    string server = dtConfig.Rows[0]["IP"].ToString();
                    int port = DACrux.Base.Convert.intParse(dtConfig.Rows[0]["PORT"].ToString());
                    string id = dtConfig.Rows[0]["ID"].ToString();
                    string password = dtConfig.Rows[0]["PASS"].ToString();

                    ftp = new Utility.HFtpClient(server, port, id, password);
                }

                // step_seq 별 처리
                foreach (DataRow row in dt.Rows)
                {
                    if (statusMessage != null)
                        statusMessage(String.Format("Downloading data ({0}/{1})...", dt.Rows.IndexOf(row) + 1, dt.Rows.Count));

                    string stepID = row["STEP_ID"].ToString();
                    string waferID = row["WAFER_ID"].ToString();
                    long stepSeq = DACrux.Base.Convert.longParse(row["STEP_SEQ"].ToString());

                    // defect 정보
                    Defect[] defectArr = GetDefectMapViewer_DefectArray(new long[] { stepSeq });
                    Array.Sort(defectArr);

                    if (defectArr == null || defectArr.Length == 0)
                        continue;

                    // defect image 정보 가져오기
                    DataTable imgDt = m_OBJ.GetDefectImagePath02(stepSeq.ToString());

                    // defect image 정보 Add
                    foreach (DataRow imgRow in imgDt.Rows)
                    {
                        int defectID = DACrux.Base.Convert.intParse(imgRow["DEFECTID"].ToString());
                        string imagePath = imgRow["PATH"].ToString();

                        if (String.IsNullOrEmpty(imagePath))
                            continue;

                        int idx = Array.BinarySearch<Defect>(defectArr, new Defect() { STEP_SEQ = stepSeq, DEFECTID = defectID });

                        if (idx < 0)
                            continue;

                        defectArr[idx].Images.Add(Path.GetFileName(imagePath));

                        try
                        {
                            string fileName = Path.Combine(tempPath, stepID, waferID, Path.GetFileName(imagePath));

                            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                                Directory.CreateDirectory(Path.GetDirectoryName(fileName));

                            // defect 이미지 다운로드
                            ftp.Down(imagePath, fileName);
                        }
                        catch
                        {
                            // 다운로드 실패 시 무시
                        }
                    }

                    // KLARF 파일 생성
                    DACrux.Data.Parser.Klarf.ParserKlarf parser = new Data.Parser.Klarf.ParserKlarf(defectArr, DmsCache.Instance[stepSeq], DmsCache.Instance.ClassLookup);

                    string klarfFile = null;

                    for (int i = 0; i < 999; i++)
                    {
                        klarfFile = Path.Combine(tempPath, stepID, waferID, String.Format("{0}_{1}.{2:000}", waferID, stepID, i));

                        if (!File.Exists(klarfFile))
                            break;
                    }

                    parser.SaveFile(klarfFile, true, null, true);
                }

                // 전체 디렉토리를 압축
                string zipFileName = Path.Combine(saveFolder, String.Format("{0}.zip", lotID));

                if (statusMessage != null)
                    statusMessage(String.Format("{0} 압축 파일을 생성중입니다...", Path.GetFileName(zipFileName)));

                SevenZipUtil.ZipFolder(zipFileName, tempPath);
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();

                if (!String.IsNullOrEmpty(tempPath))
                    Directory.Delete(tempPath, true);
            }
        }

        private void SaveFtpImage(string server, int port, string id, string password, string ftpFilePath, string localFilePath)
        {
            using (DACrux.Utility.ServerCommunicationFtp oFTP = new Utility.ServerCommunicationFtp())
            {
                oFTP.Server = server;
                oFTP.Port = port;
                oFTP.UserID = id;
                oFTP.Password = password;
                oFTP.ChmodValue = 777;
                oFTP.ReceiveFile(ftpFilePath, localFilePath);
            }
        }

        public static DefectClassDictionary GetClassLookup()
        {
            DefectClassDictionary dic = new DefectClassDictionary();

            DefectMapAnalysis obj = new DefectMapAnalysis();
            DataTable dt = obj.GetDefectClassInfo();

            if (dt == null || dt.Rows.Count == 0)
            {
                dic.Add(0, "0");
            }
            else
            {
                foreach (DataRow row in dt.Rows)
                    dic.Add(Int32.Parse(row["CLASSNUMBER"].ToString()), row["NAME"].ToString());
            }

            return dic;
        }

        public static DmsWaferDieInfo GetWaferDieInfo(long stepSeq)
        {
            DefectMapAnalysis oMap = new DefectMapAnalysis();
            DataSet ds = oMap.GetDefectMapInfo(stepSeq);

            if (ds == null || ds.Tables.IndexOf("MAP") < 0 || ds.Tables.IndexOf("DIE") < 0)
                return null;

            if (ds.Tables["MAP"].Rows.Count == 0)
                return null;

            DataRow mapRow = ds.Tables["MAP"].Rows[0];
            DmsStepInfo step = new DmsStepInfo();

            step.Maker = mapRow["MAKER"].ToString();
            step.Model = mapRow["MODEL"].ToString();
            step.Equip = mapRow["EQUIP"].ToString();
            step.ResultTimestamp = mapRow["RESULTTIMESTAMP"].ToString();
            step.LotID = mapRow["LOT_ID"].ToString();
            step.WaferID = mapRow["WAFER_ID"].ToString();
            step.WaferSize = DACrux.Base.Convert.intParse(mapRow["WAFER_SIZE"].ToString());
            step.SampleSize = step.WaferSize / 1000 / 1000;
            step.DeviceID = mapRow["PRODUCT"].ToString();
            step.SetupID = mapRow["SETUP_ID"].ToString();
            step.SetupTimestamp = mapRow["SETUP_TIME"].ToString();
            step.StepID = mapRow["STEP_ID"].ToString();
            step.SampleOrientationMarkType = "NOTCH";
            step.DiePitchX = DACrux.Base.Convert.doubleParse(mapRow["DIE_PITCH_X"].ToString());
            step.DiePitchY = DACrux.Base.Convert.doubleParse(mapRow["DIE_PITCH_Y"].ToString());
            step.DieOriginX = DACrux.Base.Convert.intParse(mapRow["DIE_ORIGIN_X"].ToString());
            step.DieOriginY = DACrux.Base.Convert.intParse(mapRow["DIE_ORIGIN_Y"].ToString());
            step.Slot = DACrux.Base.Convert.intParse(mapRow["SLOT_ID"].ToString());
            step.Angle = DACrux.Base.Convert.intParse(mapRow["ANGLE"].ToString());
            step.SampleCenterLocationX = DACrux.Base.Convert.doubleParse(mapRow["ORIGIN_X"].ToString());
            step.SampleCenterLocationY = DACrux.Base.Convert.doubleParse(mapRow["ORIGIN_Y"].ToString());
            step.AreaPerTest = DACrux.Base.Convert.doubleParse(mapRow["SCAN_AREA"].ToString());
            step.Inspector = mapRow["INSPECTION_EQ"].ToString();
            step.ShotArrayX = DACrux.Base.Convert.intParse(mapRow["ST_XCNT"].ToString(), 1);
            step.ShotArrayY = DACrux.Base.Convert.intParse(mapRow["ST_YCNT"].ToString(), 1);
            step.ShotStartX = DACrux.Base.Convert.intParse(mapRow["ST_START_X"].ToString(), 1);
            step.ShotStartY = DACrux.Base.Convert.intParse(mapRow["ST_START_Y"].ToString(), 1);
            step.StepSeq = stepSeq.ToString();

            DataTable dieDt = ds.Tables["DIE"];
            Point[] dies = new Point[dieDt.Rows.Count];

            for (int i = 0; i < dieDt.Rows.Count; i++)
            {
                dies[i] = new Point(DACrux.Base.Convert.intParse(dieDt.Rows[i]["INDEX_X"].ToString()), DACrux.Base.Convert.intParse(dieDt.Rows[i]["INDEX_Y"].ToString()));
            }

            return new DmsWaferDieInfo(stepSeq, step, dies);
        }

        #endregion
    }

    public class DefectEx : Defect
    {
        public string WAFER_ID;
        public string STEP_ID;
        public DateTime RESULTTIMESTAMP;
        public string CLASSNAME;
    }
}
