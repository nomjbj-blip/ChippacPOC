using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.InteropServices;
using DACrux.TEST.DSL;
using DACrux.TEST.Interface;
using System.Transactions;

namespace DACrux.TEST.BSL
{
    public class VisualInspection : Miracom.Middleware.BaseComponent, iVisulalInspection
    {
        public long GetNextLotSeq()
        {
            T_AVI_TABLE oDSL = null;
            try
            {
                oDSL = new T_AVI_TABLE();
                return oDSL.GetNextLotSeq();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertLot(string FACILITY, string TESTAREA, string PRODUCT, string PROGRAM, string LOTID, string FABLOT, string START_TIME, string END_TIME)
        {
            T_AVI_TABLE oDSL = null;
            try
            {
                oDSL = new T_AVI_TABLE();
                oDSL.InsertLot(FACILITY, TESTAREA, PRODUCT, PROGRAM, LOTID, FABLOT, START_TIME, END_TIME);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertWafer(string LOT_SEQ, string WAFER_ID, string TESTER, string OPERATOR, string TESTED_DIE, string START_TIME, string LOSS_DIE)
        {
            T_AVI_TABLE oDSL = null;
            try
            {
                oDSL = new T_AVI_TABLE();
                oDSL.InsertWafer(LOT_SEQ, WAFER_ID, TESTER, OPERATOR, TESTED_DIE, START_TIME, LOSS_DIE);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void InsertFailDie(string PRODUCT, string[] strValue)
        {
            T_AVI_TABLE oDSL = null;
            try
            {
                oDSL = new T_AVI_TABLE();
                oDSL.InsertFailDie(PRODUCT, strValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQCFailDie(string PRODUCT, string[] strValue)
        {
            T_AVI_TABLE oDSL = null;
            try
            {
                oDSL = new T_AVI_TABLE();
                oDSL.UpdateFailDie(PRODUCT, strValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetMapData(string TESTAREA, string PRODUCT, string PROGRAM, string LOT_ID, string WAFER_ID)
        {
            T_AVI_TABLE oDSL = null;
            try
            {
                oDSL = new T_AVI_TABLE();
                return oDSL.GetMapData(TESTAREA, PRODUCT, PROGRAM, LOT_ID, WAFER_ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAVIMapData(string TESTAREA, string PRODUCT, string LOT_ID, string WAFER_ID)
        {
            T_AVI_TABLE oDSL = null;
            try
            {
                oDSL = new T_AVI_TABLE();
                return oDSL.GetAVIMapData(TESTAREA, PRODUCT, LOT_ID, WAFER_ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAVIMapData(string TESTAREA, string PRODUCT, string LOT_ID, string WAFER_ID, string PROGRAM)
        {
            T_AVI_TABLE oDSL = null;
            try
            {
                oDSL = new T_AVI_TABLE();
                return oDSL.GetAVIMapData(TESTAREA, PRODUCT, LOT_ID, WAFER_ID, PROGRAM);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferList(string TestArea, string StartTime, string EndTime, string LikeDevice, string LikeLot, string LikeWafer)
        {
            T_AVI_TABLE oDSL = null;
            try
            {
                oDSL = new T_AVI_TABLE();
                return oDSL.GetWaferList(TestArea, StartTime, EndTime, LikeDevice, LikeLot, LikeWafer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public DataSet GetStepListByStepID(string TestArea, string StartTime, string EndTime, string LikeDevice, string LikeLot, string LikeWafer)
        //{
        //    T_DMS_INSP_INFO oDSL = null;
        //    try
        //    {
        //        oDSL = new T_DMS_INSP_INFO();
        //        return oDSL.GetStepListByStepID(TestArea, StartTime, EndTime, LikeDevice, LikeLot, LikeWafer);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataTable GetTestAreaList()
        {
            T_AVI_TABLE oDSL = null;
            try
            {
                oDSL = new T_AVI_TABLE();
                return oDSL.GetTestAreaList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteAVIZero(string PRODUCT, string WAFER_SEQ)
        {
            T_AVI_TABLE oDSL = null;
            try
            {
                oDSL = new T_AVI_TABLE();
                oDSL.DeleteAVIZero(PRODUCT, WAFER_SEQ);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RunWaferSummary(string PRODUCT, string WAFER_SEQ)
        {
            T_AVI_TABLE oDSL = null;
            try
            {
                oDSL = new T_AVI_TABLE();
                oDSL.RunWaferSummary(PRODUCT, WAFER_SEQ);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDies(string MapID, bool isUseDie)
        {
            TQP_USEMAP oDSL = null;
            try
            {
                oDSL = new TQP_USEMAP();
                return oDSL.GetDies(MapID, isUseDie);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateVIFailDie(string PROGRAM, string[] strValue)
        {
            TQP_PROGRAM oDSL = null;
            try
            {
                oDSL = new TQP_PROGRAM();
                oDSL.UpdateVIFailDie(PROGRAM, strValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectLotSumByLotID(string LotID)
        {
            TQP_LOT_SUM oDSL = null;
            try
            {
                oDSL = new TQP_LOT_SUM();
                return oDSL.SelectLotSumByLotID(LotID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferInfo(string TESTAREA, string PRODUCT, string LOT_ID, string WAFER_ID)
        {
            TQP_WAFER_SUM oDSL = null;
            try
            {
                oDSL = new TQP_WAFER_SUM();
                return oDSL.GetWaferInfo(TESTAREA, PRODUCT, LOT_ID, WAFER_ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetOnlyVI(string PROGRAM, string WAFER_SEQ)
        {
            TQP_WAFER_SUM oDSL = null;
            try
            {
                oDSL = new TQP_WAFER_SUM();
                return oDSL.GetOnlyVI(PROGRAM, WAFER_SEQ);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateSummaryFTA(string WAFER_SEQ, int FTA)
        {
            TQP_WAFER_SUM oDSL = null;
            try
            {
                oDSL = new TQP_WAFER_SUM();
                oDSL.UpdateSummaryFTA(WAFER_SEQ, FTA);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferListByAll(string lotId, string product, string testarea)
        {
            TQP_WAFER_SUM oDSL = null;

            try
            {
                oDSL = new TQP_WAFER_SUM();
                return oDSL.GetWaferLitstByAll(lotId, product, testarea);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferListByAll(string lotId, string product, string testarea, string waferList)
        {
            TQP_WAFER_SUM oDSL = null;

            try
            {
                oDSL = new TQP_WAFER_SUM();
                return oDSL.GetWaferListByAll(lotId, product, testarea, waferList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAVISpec(string InspType)
        {
            TQP_VISUALINSPSPEC oDSL = null;
            try
            {
                oDSL = new TQP_VISUALINSPSPEC();
                return oDSL.GetAVISpec(InspType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateRawdata(string TABLE, string VISUALINSP, string WAFER_SEQ, string X, string Y)
        {
            TQP_VISUALINSPSPEC oDSL = null;
            try
            {
                oDSL = new TQP_VISUALINSPSPEC();
                oDSL.UpdateRawdata(TABLE, VISUALINSP, WAFER_SEQ, X, Y);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateRawdataDMS(string VISUALINSP, string STEP_SEQ, string X, string Y)
        {
            TQP_VISUALINSPSPEC oDSL = null;
            try
            {
                oDSL = new TQP_VISUALINSPSPEC();
                oDSL.UpdateRawdataDMS(VISUALINSP, STEP_SEQ, X, Y);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateReturnRawdata(string TABLE, string WAFER_SEQ)
        {
            TQP_VISUALINSPSPEC oDSL = null;
            try
            {
                oDSL = new TQP_VISUALINSPSPEC();
                oDSL.UpdateReturnRawdata(TABLE, WAFER_SEQ);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateReturnRawdataDMS(string STEP_SEQ)
        {
            TQP_VISUALINSPSPEC oDSL = null;
            try
            {
                oDSL = new TQP_VISUALINSPSPEC();
                oDSL.UpdateReturnRawdataDMS(STEP_SEQ);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetEDSBINDesc(string PROGRAM)
        {
            TQP_VISUALINSPSPEC oDSL = null;
            try
            {
                oDSL = new TQP_VISUALINSPSPEC();
                return oDSL.GetEDSBINDesc(PROGRAM);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable SelectCustomerWithMapdef(string product)
        {
            TQP_PRODUCT oDSL = null;
            try
            {
                oDSL = new TQP_PRODUCT();

                return oDSL.SelectCustomerWithMapdef(product);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetIslot(string Device, string strTestArea, string TestProgram, string LotID)
        {
            DataTable dt = null;
            TQP_LOT oDSL = null;
            try
            {
                oDSL = new TQP_LOT();
                dt = oDSL.GetIslot(Device, strTestArea, TestProgram, LotID);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateLot(string lotSeq,
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
                                string EngCmt)
        {
            TQP_LOT oDSL = null;
            try
            {
                oDSL = new TQP_LOT();
                oDSL.CreateLot(lotSeq,Device,TestArea,TestProgram,LotNo, MotherLotNo,LotType, FabLot,Family,Facility,device_alias,StartTime,EndTime,WipStatus,wafers,SpesailCmt,EngCmt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CopyLot(string TestArea, string STime, string ETime, string PreLotSeq)
        {
            TQP_LOT oDSL = null;
            try
            {
                oDSL = new TQP_LOT();
                oDSL.CopyLot(TestArea, STime, ETime, PreLotSeq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCount(string StartTime, string EndTime, string Wafers, string Lot_Seq)
        {
            TQP_LOT oDSL = null;
            try
            {
                oDSL = new TQP_LOT();
                oDSL.UpdateCount(StartTime, EndTime, Wafers, Lot_Seq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SelectNextLotSeq()
        {
            TQP_LOT oDSL = null;
            try
            {
                oDSL = new TQP_LOT();
                return oDSL.SelectNextLotSeq();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectLotInfoByLotID(string LotID)
        {
            TQP_LOT oDSL = null;
            DataTable dt = null;
            try
            {
                oDSL = new TQP_LOT();
                dt = oDSL.SelectLotInfoByLotID(LotID);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetIsWafer(string Lot_Seq, String WaferID)
        {
            DataTable dt = null;
            TQP_WAFER oDSL = null;
            try
            {
                oDSL = new TQP_WAFER();
                dt = oDSL.GetIsWafer(Lot_Seq, WaferID);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable IsFirst(string Wafer_Seq, string StartTime, string EndTime)
        {
            DataTable dt = null;
            TQP_WAFER oDSL = null;
            try
            {
                oDSL = new TQP_WAFER();
                dt = oDSL.IsFirst(Wafer_Seq, StartTime, EndTime);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectTestdie(string Lot_Seq, string WaferID)
        {
            DataTable dt = null;
            TQP_WAFER oDSL = null;
            try
            {
                oDSL = new TQP_WAFER();
                dt =oDSL.SelectTestdie(Lot_Seq, WaferID);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateProbeCnt(string Lot_Seq, string WaferID)
        {
            TQP_WAFER oDSL = null;
            try
            {
                oDSL = new TQP_WAFER();
                oDSL.UpdateProbeCnt(Lot_Seq, WaferID);

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
            TQP_WAFER oDSL = null;
            try
            {
                oDSL = new TQP_WAFER();
                oDSL.CreateWafer(Wafer_Seq
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
                                                                , FviFlag);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferCount(String Lot_Seq)
        {
            DataTable dt = null;
            TQP_WAFER oDSL = null;
            try
            {
                oDSL = new TQP_WAFER();
                dt = oDSL.GetWaferCount(Lot_Seq);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetStartTime(String Lot_Seq)
        {
            DataTable dt = null;
            TQP_WAFER oDSL = null;
            try
            {
                oDSL = new TQP_WAFER();
                dt = oDSL.GetStartTime(Lot_Seq);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetEndTime(String Lot_Seq)
        {
            DataTable dt = null;
            TQP_WAFER oDSL = null;
            try
            {
                oDSL = new TQP_WAFER();
                dt = oDSL.GetEndTime(Lot_Seq);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public void UpdateLossDie(String Wafer_Seq, string LossDie)
        {
            TQP_WAFER oDSL = null;
            try
            {
                oDSL = new TQP_WAFER();
                oDSL.UpdateLossDie(Wafer_Seq, LossDie);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateTestDie(String Wafer_Seq, string TestDie)
        {
            TQP_WAFER oDSL = null;
            try
            {
                oDSL = new TQP_WAFER();
                oDSL.UpdateTestDie(Wafer_Seq, TestDie);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SelectNextWaferSeq()
        {
            TQP_WAFER oDSL = null;
            DataTable dt = null;
            try
            {
                oDSL = new TQP_WAFER();
                return oDSL.SelectNextWaferSeq();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertPrb(string strTable
                                , string Wafer_Seq
                                , string DieId
                                , string X
                                , string Y
                                , string Bin
                                , string hBin
                                , string CharBin
                                , string Site
                                , string VisualInsp
                                , string Avi)
        {
            T_PRB oDSL = null;
            try
            {
                oDSL = new T_PRB();
                oDSL.InsertPrb(strTable, Wafer_Seq
                                        , DieId
                                        , X
                                        , Y
                                        , Bin
                                        , hBin
                                        , CharBin
                                        , Site
                                        , VisualInsp
                                        , Avi);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CallProcedure(string testArea, string product, string program, string waferSeq, string strGecBin)
        {
            T_PRB oDSL = null;
            try
            {
                oDSL = new T_PRB();
                oDSL.CallProcedure(testArea, product, program, waferSeq, strGecBin);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string ColorString(int iBinNumber)
        {
            try
            {
                return utilColor.ColorString(iBinNumber);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int UpdateVI(string strFactory, string TestArea, string Product, string Program, string LotID, string WaferID, object oFailCode)
        {
            string strTableName = string.Empty;
            TQP_LOT oLot = null;
            TQP_WAFER oWafer = null;
            TQP_WAFER_SUM oWaferSum = null;
            T_AVI_TABLE oAviTable = null;

            long lLotSeq = -1;
            long lWaferSeq = -1;
            try
            {
                /// DSL 선언
                /// //////////////////////////////////////////////////////////////////////////////////////////////
                oLot = new TQP_LOT();
                oWafer = new TQP_WAFER();
                oWaferSum = new TQP_WAFER_SUM();
                oAviTable = new T_AVI_TABLE();

                using (TransactionScope ts = new TransactionScope())
                {
                    /// Data 확인/정리
                    /// //////////////////////////////////////////////////////////////////////////////////////////////
                    List<DACrux.Base.Die> oUpdateDie = (List<DACrux.Base.Die>)DACrux.Base.Convert.Deserialize(oFailCode);
                    strTableName = string.Format("T_AVI_{0}", Product);

                    /// Lot Create or Lot Seq Select
                    /// //////////////////////////////////////////////////////////////////////////////////////////////
                    lLotSeq = oLot.CreateLot(TestArea, Product, Program, LotID);

                    /// Wafer Create 
                    /// //////////////////////////////////////////////////////////////////////////////////////////////
                    /// ==> Raw Dat History를 관리학 위해 TQP_WAFER의 기존에 동일한 Wafer[TESTAREA,PRODUCT,PROGRAM,LOT]에 대해 
                    ///     PROBE_CNT를 증가시키고
                    ///     새로운 Wafer를 PROBE_CNT=0으로 Creation한다.
                    lWaferSeq = oWafer.CreateWafer(strFactory, lLotSeq, WaferID);
                    oWaferSum.ProbeCntPlus(lLotSeq, WaferID);
                
                    /// Die Data Update
                    /// //////////////////////////////////////////////////////////////////////////////////////////////
                    /// 신규 Map이므로 USEMAP에서 필요한 MAP을 T_AVI_XXXX로 1Set Copy한다.
                    /// 
                    oAviTable.CreateCleanMap(Product,lWaferSeq);
                    /// Update Fail Die
                    /// 
                    string[,] strFailDie = new string[oUpdateDie.Count,4];
                    
                    for(int i=0; i<oUpdateDie.Count;i++)
                    {
                        strFailDie[i, 0] = oUpdateDie[i].VIFail.ToString();
                        strFailDie[i, 1] = lWaferSeq.ToString();
                        strFailDie[i, 2] = oUpdateDie[i].IndexX.ToString();
                        strFailDie[i, 3] = oUpdateDie[i].IndexY.ToString();
                    }
                    oAviTable.UpdateFailDie(Product, strFailDie);

                    oWaferSum.ProcWaferSum(Product, lWaferSeq);
                    ts.Complete();
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}