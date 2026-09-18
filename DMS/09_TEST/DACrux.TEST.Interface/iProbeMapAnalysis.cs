using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;

namespace DACrux.TEST.Interface
{
    public interface iProbeMapAnalysis
    {
        DataSet GetWaferMap(long WaferSeq);

        // 2015-04-22
        DataSet GetWaferMap(string TestArea, string Product, string Program, string LotID, string WaferID, int ProbeCnt, bool IsMaxOper = false);

        DataSet GetCumMap(long[] WaferSeqs);
        DataTable GetCumRawData(string Program, long[] WaferSeq, string[] SelBin);
        DataTable GetBinDistribution(string program, long waferSeq);
        DataTable GetMapData(string program, long[] WaferSeq, string[] BINS);
        DataTable GetWaferInfo(long[] WaferSeq);
        DataTable GetWaferInfo(string[] WaferIDs);
        DataTable GetYieldByDay(string[] WaferSeq);
        DataTable GetMapDataForDefectOverlay(string PROGRAM, string WaferSeq);
        DataTable GetWaferInfo(long WaferSeq);

        void CreateParaSpec(string[,] ParaInfo);
        DataTable GetParaSpecListEditable(string factory, string Program);
        void UpdateProgramParaSpec(string Program, string[,] ParaInfo);
        void DeleteProgramParaSpec(string Program);

        DataTable GetMapData(string Program, long WaferSeq, string paraItem, int cutCnt);
        DataTable GetMapDataCnt(string Program, long WaferSeq, string paraItem, int cutCnt);
        DataTable GetWaferInfoByLotSlot(string lot_id, string slot_id, string testarea);
        DataTable GetWaferInfoByWaferId(string wafer_id, string testarea);

        //20150330
        DataTable GetMapDefByLotID(string LotID);
        DataTable GetWaferInfo(string TestArea, string Product, string LotID, string WaferID);
        DataTable GetBinDistribution(string PROGRAM, string[] WaferSeq, bool bMulti, bool bVI);
        DataTable GetWaferInfo(string TestArea, string LotID, string WaferID, bool bDummy);
        void InsertFailDie(string PRODUCT, string[] strValue);
        long InsertLot(string FACILITY, string TESTAREA, string PRODUCT, string PROGRAM, string LOTID, string FABLOT, string START_TIME, string END_TIME);
        long InsertWafer(string LOT_SEQ, string WAFER_ID, string TESTER, string OPERATOR, string TESTED_DIE, string START_TIME, string LOSS_DIE);

        #region [동부 하이텍 추가]

        DataSet SelectWaferMapDrawData(long WaferSeq);
        DataSet SelectWaferMapDrawData(long[] WaferSeq);
        DataTable SelectWaferParaItem(long WaferSeq, string strParaItem, int decimalLength);
        DataTable SelectWaferParaItem(long[] WaferSeq, string strParaItem, int decimalLength);
        DataSet GetCumMapReport(long[] WaferSeqs);
        System.Data.DataSet SelectWaferMapBasic(long WaferSeq);
        System.Data.DataSet SelectWaferMapConfigMap(long WaferSeq);
        System.Data.DataSet SelectWaferShotMapBasic(string strDevice);
        System.Data.DataSet SelectWaferParaData(long WaferSeq, string[] strParaItem);
        System.Data.DataTable SelectWaferParaDataMulti(long[] WaferSeqList, string[] strParaItem);
        System.Data.DataSet SelectWaferMapConfig(long WaferSeq);
        System.Data.DataTable SelectWaferShotMap(string strMapID);
        void UpdateMapConfig(string strTestArea, string strProduct, string strProgram, string strDir, string strAngle, string strIndexX, string strIndexY, string strUser, string strcomment);
        DataTable SelectMapConfigList(string strProduct);
        DataTable SelectMapConfigView(bool bDeleteFlag);
        void DeleteMapConfig(string strSequence, string strUser, string strcomment);
        DataSet GetCumWaferReport(long[] WaferSeqs);
        DataSet GetWaferBinReport(long[] WaferSeq, bool bWaferBase);
        //System.Data.DataTable GetUserMapConfig(string strUserID, string strCategory = "");
        DataTable GetMapConfigWaferList(string strStartTime, string strEndTime, string strTestArea, string strDevice, string strProgram);
        void WaferMapConfigUpdate(string strWaferSeq, string strMapConfigSeq);
        System.Data.DataSet SelectParaReportData(long[] WaferSeqList);
        System.Data.DataSet SelectAVICumData(long[] WaferSeq);

        #endregion

        #region Pattern Search

        DataTable GetPatternSearch_WaferList(long[] waferSeqArr);
        List<int> GetPatternSearch_DieList(long waferSeq);

        #endregion


    }
}