using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;

namespace DACrux.SEMDMS.Interface
{
    public interface iDefectMapAnalysis
    {
        // TQD_INSP_INFO
        DataTable GetStepInfo(long[] step_seqs);
        DataTable GetStepInfo(string[] step_seqs);
        DataTable GetStepInfoByStepSeq(string step_seq);
        DataTable GetStepSum(long step_seq);
        DataTable GetStepList(string start, string end, string[] fieldsorts, string[] wheres, string sort);
        DataTable GetStepListByStepID(string TestArea, string StartTime, string EndTime, string LikeDevice, string LikeLot, string LikeWafer);
        DataTable GetInspInfoByLotId(string LotId);
        DataTable SelectMapParsingResultList();
        DataTable SelectParsingResultLotList();
        DataTable SelectParsingResultWaferList();
        DataTable SelectParsingResultStepList();
        DataTable SelectParsingResultInspEqList();
        DataTable SelectParsingResultByItem(string lotId, string waferId, string stepId, string inspEq);
        DataTable GetImageGallery(long[] step_seqs);

        // TQD_SETUP
        DataTable GetSetupRecipe(long setup_seq);

        // TQD_DEFECT
        DataTable GetDefectData(long step_seq);
        DataTable GetDefectData(long[] stepseq);
        byte[] GetDefectDataToObjectArray_Comp(long[] steps);
        object[,] GetDefectDataToObjectArray(long[] steps);
        DataTable GetDensity(long[] step_seqs, string SelDefects = "ALL");
        DataTable GetDefectCount(long[] step_seqs, string column);

        // TQD_IMAGE
        DataTable GetDefectImage(long step_seq, int[] defectids);
        DataTable GetDefectImageInfo(long step_seq);


        // REPEAT_DEFECT
        DataTable GetShotRepeatDefect(long step_seq, string product, float die_size_x, float die_size_y, float tolerance);
        DataTable GetDieRepeatDefect(long step_seq, float tolerance);
        DataTable GetRepeatListByShot(int dieSizeX, int dieSizeY, int tolerance, int stepSeq, string product, int repeatCnt);
        DataTable GetRepeatListByDie(int tolerance, int stepSeq, int repeatCnt);


        // TQD_COLORBYSIZE
        DataTable GetSize(string userid);
        Dictionary<int, Color> GetDefectSizeColor(string userid);
        DataTable SelectColorByDefectSize(string userid);
        
        void GetFirstStepCnt(long step_seq, ref int newCnt, ref int carryOverCnt, ref int missingCnt);
        void GetDsaCnt(long first_step_seq, long step_seq, float tolerance, ref int newCnt, ref int carryOverCnt, ref int missingCnt);
        DataTable GetFirstStep(long step_seq);
        DataTable GetDsa(long first_step_seq, long step_seq, float tolerance);

        // OVERRAY
        DataTable GetWaferDefectOverlay(string program, string[] waferSeq);
        DataTable GetStepInfoOverlay(string lotId, string waferId);

        //TQD_SETUP_MAP
        DataTable GetSetupMap(long Setup_seq);
        DataTable GetInspInfo01(long[] stepSeqArr);
        DataTable GetInspInfo02(long[] stepSeqArr);
        DataTable GetDefectImagePath01(long[] stepSeqArr);

        #region DB 하이텍 
        
        DataSet GetChartAnalysis(long[] steps);
        System.Data.DataTable GetColorByDefectType();
        DataSet GetDMSStepWaferList(string start, string end, string[] wheres, bool TimeNotCheck, bool bLastInspection);
        byte[] GetDefectMapViewer(long[] step_seqs);
        DataSet GetBareDefectMapViewer(long[] step_seqs, string strMapID);
        System.Data.DataTable GetShotDeviceList();
        System.Data.DataTable GetMapIDListNotDelete();
        System.Data.DataSet GetDefectMapInfo(long step_seq);
        
        DataTable GetDefectClassInfo();
        Dictionary<string, Color> GetDefectClassColor();

        DataTable GetDSAReport(long[] step_seqs, string strTol);
        System.Data.DataSet GetBasicMapViewer(long[] step_seqs);

        DataTable GetDefectImages(long[] steps);
        // Reclassify 
        DataTable SetReclassifyDefectImageList(long[] steps, string[,] Params, string userId, string ipAddr);
        byte[] SetReclassifyDefectList_Comp(long[] steps, string[,] Params, string userId, string ipAddr);
        object[,] SetReclassifyDefectList(long[] steps, string[,] Params, string userId, string ipAddr);

        // Delete Image
        DataTable SetRemoveDefectImageList(long[] steps, string[,] Params, string userId, string ipAddr);

        string GetImagePath(long stepseq, long waferseq, int defectid, int imageid);
        DataTable GetWaferInfo(long[] stepSeqArr);

        DataSet GetDefectMapViewer_Info(long[] step_seqs);
        byte[] GetDefectMapViewer_Map(long[] step_seqs, string[] setupseq, string[] test);
        byte[] GetDefectMapViewer_Defects(long[] step_seqs, bool bAdder);
        List<object[]> GetDefectMapViewer_Defects2(long[] step_seqs, bool bAdder);
        byte[] GetDefectMapViewer_Defects2_Comp(long[] step_seqs, bool bAdder);
        object[,] GetDefectMapViewer_Defects3(long[] step_seqs, bool bAdder);
        byte[] GetDefectMapViewer_Defects3_Comp(long[] step_seqs, bool bAdder);
        byte[] GetDefectMapViewer_Chart(long[] step_seqs, bool bAdder);
     
        #endregion

        #region DM Data Maint

        System.Data.DataSet GetDMMaintDataList(long[] step_seqs);

        void SetDMMaintDataUpdateWafer(string strStepSeq, string strWaferID, string strLotID, int iSlotID, string strStepID, string strProduct, string strUser, string strComment, string strLocalIP);
        void SetDMMaintDataUpdateLot(string[] strStepSeq, string strLotseq, string strLotID, string strOriLotID, string strStepID, string strProduct, string strUser, string strComment, string strLocalIP);

        void InsertMaintHis(string[] strValue);
        DataTable GetMaintHisTime(string strStarttime, string strEndTime);
        DataTable GetMaintHisLotID(string strLotID);

        void SetDMMaintDataDeleteWafer(string strStepSeq, string strUser, string strComment, string strLocalIP);
        void SetDMMaintDataDeleteLot(string[] strStepSeqList, string strUser, string strComment, string strLocalIP);

        #endregion  DM Data Maint

        #region Pattern Search 관련 메서드

        DataTable GetPatternSearch_WaferList(long[] stepSeqArr);
        Dictionary<long, List<PointF>> GetPatternSearch_XYList(long[] stepSeqArr);
        List<PointF> GetPatternSearch_XYList(long stepSeq);
        byte[] GetPatternSearch_XYList_Comp(long[] stepSeqArr);

        #endregion

        DataTable GetLotInspInfo(string lotID, bool onlyLastInspection);
        DataTable GetDefectImagePath02(string stepSeq);
    }
}
