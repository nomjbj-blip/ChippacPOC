using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.TEST.Interface
{
    public interface iVisulalInspection
    {
        long GetNextLotSeq();
        void InsertLot(string FACILITY, string TESTAREA, string PRODUCT, string PROGRAM, string LOTID, string FABLOT, string START_TIME, string END_TIME);
        void InsertWafer(string LOT_SEQ, string WAFER_ID, string TESTER, string OPERATOR, string TESTED_DIE, string START_TIME, string LOSS_DIE);
        void InsertFailDie(string PRODUCT, string[] strValue);
        void UpdateQCFailDie(string PRODUCT, string[] strValue);
        DataTable GetMapData(string TESTAREA, string PRODUCT, string PROGRAM, string LOT_ID, string WAFER_ID);
        DataTable GetWaferList(string TestArea, string StartTime, string EndTime, string LikeDevice, string LikeLot, string LikeWafer);
        DataTable GetTestAreaList();
        void DeleteAVIZero(string PRODUCT, string WAFER_SEQ);
        void RunWaferSummary(string PRODUCT, string WAFER_SEQ);
        DataTable GetDies(string MapID, bool isUseDie);
        void UpdateVIFailDie(string PROGRAM, string[] strValue);
        DataTable GetAVISpec(string InspType);
        DataTable GetEDSBINDesc(string PROGRAM);
        DataTable GetWaferInfo(string TESTAREA, string PRODUCT, string LOT_ID, string WAFER_ID);
        void UpdateRawdata(string TABLE, string VISUALINSP, string WAFER_SEQ, string X, string Y);
        void UpdateReturnRawdata(string TABLE, string WAFER_SEQ);
        int GetOnlyVI(string PROGRAM, string WAFER_SEQ);
        void UpdateSummaryFTA(string WAFER_SEQ, int FTA);
        //DataTable SelectAreaGroup(string TestArea);
        //DataSet GetStepListByStepID(string TestArea, string StartTime, string EndTime, string LikeDevice, string LikeLot, string LikeWafer);
        //DataSet GetStepListByStepIDRecent(string TestArea, string StartTime, string EndTime, string LikeDevice, string LikeLot, string LikeWafer);
        //DataTable SelectDMSStepSeq(string LotID, string Product, string WaferID, string TestAera);
        //void UpdateReturnRawdataDMS(string STEP_SEQ);
        //void UpdateRawdataDMS(string VISUALINSP, string STEP_SEQ, string X, string Y);
        DataTable SelectLotSumByLotID(string LotID);
        DataTable SelectCustomerWithMapdef(string product);
        DataTable GetWaferListByAll(string lotId, string product, string testarea);
        DataTable GetWaferListByAll(string lotId, string product, string testarea, string WaferList);
        //DataTable SelectProductByCusLotID(string CustRunId);
        DataTable GetIslot(string Device, string strTestArea, string TestProgram, string LotID);
        void CreateLot(string lotSeq,
                                string Device,
                                string TestArea,
                                string TestProgram,
                                string LotNo,
                                string MotherLotNo,
                                string LotType,
                                string FabLot,
                                string Family,
                                string Facility,
                                string device_alias,
                                string StartTime,
                                string EndTime,
                                string WipStatus,
                                string wafers,
                                string SpesailCmt,
                                string EngCmt);
        void CopyLot(string TestArea, string STime, string ETime, string PreLotSeq);
        void UpdateCount(string StartTime, string EndTime, string Wafers, string Lot_Seq);
        string SelectNextLotSeq();
        DataTable SelectLotInfoByLotID(string LotID);
        DataTable GetIsWafer(string Lot_Seq, String WaferID);
        DataTable IsFirst(string Wafer_Seq, string StartTime, string EndTime);
        DataTable SelectTestdie(string Lot_Seq, string WaferID);
        void UpdateProbeCnt(string Lot_Seq, string WaferID);
        void CreateWafer(string Wafer_Seq
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
                                    , string FviFlag);
        DataTable GetWaferCount(String Lot_Seq);
        DataTable GetStartTime(String Lot_Seq);
        DataTable GetEndTime(String Lot_Seq);
        void UpdateLossDie(String Wafer_Seq, string LossDie);
        void UpdateTestDie(String Wafer_Seq, string TestDie);
        string SelectNextWaferSeq();
        void InsertPrb(string strTable, string Wafer_Seq, string DieId, string X, string Y, string Bin, string hBin, string CharBin, string Site, string VisualInsp, string Avi);
        void CallProcedure(string testArea, string product, string program, string waferSeq, string strGecBin);
        string ColorString(int iBinNumber);


        /// <summary>
        /// 2015.04.15
        /// </summary>
        /// <param name="m_TestArea"></param>
        /// <param name="m_Product"></param>
        /// <param name="m_Program"></param>
        /// <param name="m_LotID"></param>
        /// <param name="m_WaferID"></param>
        /// <param name="oUpdateDie"></param>
        /// <returns></returns>
        int UpdateVI(string strFactory, string TestArea, string Product, string Program, string LotID, string WaferID, object oFailCode);

        DataTable GetAVIMapData(string TESTAREA, string PRODUCT, string LOT_ID, string WAFER_ID);
        DataTable GetAVIMapData(string TESTAREA, string PRODUCT, string LOT_ID, string WAFER_ID, string PROGRAM);


    }
}
