using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using DACrux.Base;
using DACrux.TEST.Interface;

namespace DACrux.TEST.RO
{
    public class VisulalInspection
    {
        DACrux.TEST.Interface.iVisulalInspection m_OBJ;
        public VisulalInspection()
        {

            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_DACRUX_PRB);
            object obj = Activator.GetObject(typeof(DACrux.TEST.Interface.iVisulalInspection)
                , strUrl + "/DACrux.TEST.BSL.VisualInspection.bin");
            m_OBJ = obj as DACrux.TEST.Interface.iVisulalInspection;
        }

        public VisulalInspection(string sServer, int nPort)
        {
            if (string.IsNullOrEmpty(sServer))
                sServer = "localhost";
            string strUrl = string.Format("tcp://{0}:{1}", sServer, nPort);
            object obj = Activator.GetObject(typeof(DACrux.TEST.Interface.iVisulalInspection),
                strUrl + "/DACrux.TEST.BSL.VisualInspection.bin");
            m_OBJ = obj as DACrux.TEST.Interface.iVisulalInspection;
        }


        //public DataTable SelectAreaGroup(string TestArea)
        //{
        //    try
        //    {
        //        return m_OBJ.SelectAreaGroup(TestArea);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public long GetNextLotSeq()
        {
            try
            {
                return m_OBJ.GetNextLotSeq();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertLot(string FACILITY, string TESTAREA, string PRODUCT, string PROGRAM, string LOTID, string FABLOT, string START_TIME, string END_TIME)
        {
            try
            {
                m_OBJ.InsertLot(FACILITY, TESTAREA, PRODUCT, PROGRAM, LOTID, FABLOT, START_TIME, END_TIME);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsertWafer(string LOT_SEQ, string WAFER_ID, string TESTER, string OPERATOR, string TESTED_DIE, string START_TIME, string LOSS_DIE)
        {
            try
            {
                m_OBJ.InsertWafer(LOT_SEQ, WAFER_ID, TESTER, OPERATOR, TESTED_DIE, START_TIME, LOSS_DIE);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsertFailDie(string PRODUCT, string[] strValue)
        {
            try
            {
                m_OBJ.InsertFailDie(PRODUCT, strValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateQCFailDie(string PRODUCT, string[] strValue)
        {
            try
            {
                m_OBJ.UpdateQCFailDie(PRODUCT, strValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetMapData(string TESTAREA, string PRODUCT, string PROGRAM, string LOT_ID, string WAFER_ID)
        {
            try
            {
                return m_OBJ.GetMapData(TESTAREA, PRODUCT, PROGRAM, LOT_ID, WAFER_ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetAVIMapData(string TESTAREA, string PRODUCT, string LOT_ID, string WAFER_ID)
        {
            try
            {
                return m_OBJ.GetAVIMapData(TESTAREA, PRODUCT,  LOT_ID, WAFER_ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAVIMapData(string TESTAREA, string PRODUCT, string LOT_ID, string WAFER_ID, string PROGRAM)
        {
            try
            {
                return m_OBJ.GetAVIMapData(TESTAREA, PRODUCT, LOT_ID, WAFER_ID, PROGRAM);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public DataSet GetStepListByStepID(string TestArea, string StartTime, string EndTime, string LikeDevice, string LikeLot, string LikeWafer)
        //{
        //    try
        //    {
        //        return m_OBJ.GetStepListByStepID(TestArea, StartTime, EndTime, LikeDevice, LikeLot, LikeWafer);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataTable GetWaferList(string TestArea, string StartTime, string EndTime, string LikeDevice, string LikeLot, string LikeWafer)
        {
            try
            {
                return m_OBJ.GetWaferList(TestArea, StartTime, EndTime, LikeDevice, LikeLot, LikeWafer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public DataSet GetStepListByStepID(string TestArea, string StartTime, string EndTime, string LikeDevice, string LikeLot, string LikeWafer)
        //{
        //    try
        //    {
        //        return m_OBJ.GetStepListByStepID(TestArea, StartTime, EndTime, LikeDevice, LikeLot, LikeWafer);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public DataSet GetStepListByStepIDRecent(string TestArea, string StartTime, string EndTime, string LikeDevice, string LikeLot, string LikeWafer)
        //{
        //    try
        //    {
        //        return m_OBJ.GetStepListByStepIDRecent(TestArea, StartTime, EndTime, LikeDevice, LikeLot, LikeWafer);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataTable SelectLotSumByLotID(string LotID)
        {
            try
            {
                return m_OBJ.SelectLotSumByLotID(LotID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetTestAreaList()
        {
            try
            {
                return m_OBJ.GetTestAreaList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteAVIZero(string PRODUCT, string WAFER_SEQ)
        {
            try
            {
                m_OBJ.DeleteAVIZero(PRODUCT, WAFER_SEQ);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void RunWaferSummary(string PRODUCT, string WAFER_SEQ)
        {
            try
            {
                m_OBJ.RunWaferSummary(PRODUCT, WAFER_SEQ);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetDies(string MapID, bool isUseDie)
        {
            try
            {
                return m_OBJ.GetDies(MapID, isUseDie);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateVIFailDie(string PROGRAM, string[] strValue)
        {
            try
            {
                m_OBJ.UpdateVIFailDie(PROGRAM, strValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// IQC, OQC, AVI에 해당하는 BIN, BIN_NAME,COLOR,DESCRIPTION,KEYMAP 을 가져온다.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAVISpec(string InspType = "VI")
        {
            try
            {
                return m_OBJ.GetAVISpec(InspType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetEDSBINDesc(string PROGRAM)
        {
            try
            {
                return m_OBJ.GetEDSBINDesc(PROGRAM);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateRawdata(string TABLE, string VISUALINSP, string WAFER_SEQ, string X, string Y)
        {
            try
            {
                m_OBJ.UpdateRawdata(TABLE, VISUALINSP, WAFER_SEQ, X, Y);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public void UpdateRawdataDMS(string VISUALINSP, string STEP_SEQ, string X, string Y)
        //{
        //    try
        //    {
        //        m_OBJ.UpdateRawdataDMS(VISUALINSP, STEP_SEQ, X, Y);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void UpdateReturnRawdata(string TABLE, string WAFER_SEQ)
        {
            try
            {
                m_OBJ.UpdateReturnRawdata(TABLE, WAFER_SEQ);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public void UpdateReturnRawdataDMS(string STEP_SEQ)
        //{
        //    try
        //    {
        //        m_OBJ.UpdateReturnRawdataDMS(STEP_SEQ);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataTable GetWaferInfo(string TESTAREA, string PRODUCT, string LOT_ID, string WAFER_ID)
        {
            try
            {
                return m_OBJ.GetWaferInfo(TESTAREA, PRODUCT, LOT_ID, WAFER_ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetOnlyVI(string PROGRAM, string WAFER_SEQ)
        {
            try
            {
                return m_OBJ.GetOnlyVI(PROGRAM, WAFER_SEQ);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateSummaryFTA(string WAFER_SEQ, int FTA)
        {
            try
            {
                m_OBJ.UpdateSummaryFTA(WAFER_SEQ, FTA);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public DataTable SelectDMSStepSeq(string LotID, string Product, string WaferID, string TestAera)
        //{
        //    try
        //    {
        //        return m_OBJ.SelectDMSStepSeq(LotID, Product, WaferID, TestAera);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataTable SelectCustomerWithMapdef(string product)
        {
            try
            {
                return m_OBJ.SelectCustomerWithMapdef(product);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferListByAll(string lotId, string product, string testarea)
        {
            try
            {
                return m_OBJ.GetWaferListByAll(lotId, product, testarea);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferListByAll(string lotId, string product, string testarea, string WaferList)
        {
            try
            {
                return m_OBJ.GetWaferListByAll(lotId, product, testarea, WaferList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public DataTable SelectProductByCusLotID(string CustRunId)
        //{
        //    try
        //    {
        //        return m_OBJ.SelectProductByCusLotID(CustRunId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataTable GetIslot(string Device, string strTestArea, string TestProgram, string LotID)
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetIslot(Device, strTestArea, TestProgram, LotID);
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
            try
            {
                m_OBJ.CreateLot(lotSeq, Device, TestArea, TestProgram, LotNo, MotherLotNo, LotType, FabLot, Family, Facility, device_alias, StartTime, EndTime, WipStatus, wafers, SpesailCmt, EngCmt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CopyLot(string TestArea, string STime, string ETime, string PreLotSeq)
        {
            try
            {
                m_OBJ.CopyLot(TestArea, STime, ETime, PreLotSeq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCount(string StartTime, string EndTime, string Wafers, string Lot_Seq)
        {
            try
            {
                m_OBJ.UpdateCount(StartTime, EndTime, Wafers, Lot_Seq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SelectNextLotSeq()
        {
            DataTable dt = null;
            try
            {
                return m_OBJ.SelectNextLotSeq();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectLotInfoByLotID(string LotID)
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.SelectLotInfoByLotID(LotID);
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
            try
            {
                dt = m_OBJ.GetIsWafer(Lot_Seq, WaferID);
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
            try
            {
                dt = m_OBJ.IsFirst(Wafer_Seq, StartTime, EndTime);
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
            try
            {
                dt = m_OBJ.SelectTestdie(Lot_Seq, WaferID);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateProbeCnt(string Lot_Seq, string WaferID)
        {
            try
            {
                m_OBJ.UpdateProbeCnt(Lot_Seq, WaferID);

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
                m_OBJ.CreateWafer(Wafer_Seq
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
            try
            {
                dt = m_OBJ.GetWaferCount(Lot_Seq);
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
            try
            {
                dt = m_OBJ.GetStartTime(Lot_Seq);
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
            try
            {
                dt = m_OBJ.GetEndTime(Lot_Seq);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void UpdateLossDie(String Wafer_Seq, string LossDie)
        {
            try
            {
                m_OBJ.UpdateLossDie(Wafer_Seq, LossDie);
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
                m_OBJ.UpdateTestDie(Wafer_Seq, TestDie);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SelectNextWaferSeq()
        {
            DataTable dt = null;
            try
            {
                return m_OBJ.SelectNextWaferSeq();
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
            try
            {
                m_OBJ.InsertPrb(strTable, Wafer_Seq
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
            try
            {
                m_OBJ.CallProcedure(testArea, product, program, waferSeq, strGecBin);
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
                return m_OBJ.ColorString(iBinNumber);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateVI(string strFactory, string m_TestArea, string m_Product, string m_Program, string m_LotID, string m_WaferID, List<DACrux.Base.Die> oUpdateDie)
        {
            try
            {
                object oFailDie = DACrux.Base.Convert.Serialize(oUpdateDie);
                return m_OBJ.UpdateVI(strFactory, m_TestArea, m_Product, m_Program, m_LotID, m_WaferID, oFailDie);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
