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
                defect.IMAGECOUNT = DACrux.Base.Convert.intParse(GetData(arr, headers, "IMAGECOUNT", y));
                defect.ADDER = DACrux.Base.Convert.intParse(GetData(arr, headers, "ADDER", y));

                defects.Add(defect);
            }

            return defects;
        }

        public static DefectList DataTableToDefectList(DataTable dt)
        {
            DefectList defects = new DefectList();

            if (dt == null || dt.Rows.Count == 0)
                return defects;

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
                defect.DSIZE = DACrux.Base.Convert.doubleParse(r["DSIZE"].ToString());
                defect.CLASSNUMBER = DACrux.Base.Convert.intParse(r["CLASSNUMBER"].ToString());
                defect.TEST = DACrux.Base.Convert.intParse(r["TEST"].ToString());
                defect.CLUSTERNUMBER = DACrux.Base.Convert.intParse(r["CLUSTERNUMBER"].ToString());
                defect.IMAGECOUNT = DACrux.Base.Convert.intParse(r["IMAGECOUNT"].ToString());
                defect.ADDER = DACrux.Base.Convert.intParse(r["ADDER"].ToString());

                //try catch 로 하면 속도가 너무 느리다.. 

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

        public System.Data.DataTable SelectColorByDefectSize(string userid)
        {
            return m_OBJ.SelectColorByDefectSize(userid);
        }

        public Dictionary<int, Color> GetDefectSizeColor(
            string userid)
        {
            return m_OBJ.GetDefectSizeColor(userid);
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

        //--

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
                defect.IMAGECOUNT = DACrux.Base.Convert.intParse(GetData(arr, headers, "IMAGECOUNT", y));
                defect.ADDER = DACrux.Base.Convert.intParse(GetData(arr, headers, "ADDER", y));

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

        #endregion

        #region Lot 별 압축 파일 생성 관련 메서드

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
