using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;
using System.Drawing;

namespace DACrux.TEST.RO
{
    public class ProbeMapAnalysis
    {
        DACrux.TEST.Interface.iProbeMapAnalysis m_OBJ;
        public ProbeMapAnalysis()
        {

            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_DACRUX_PRB);
            object obj = Activator.GetObject(typeof(DACrux.TEST.Interface.iProbeMapAnalysis)
                , strUrl + "/DACrux.TEST.BSL.ProbeMapAnalysis.bin");
            m_OBJ = obj as DACrux.TEST.Interface.iProbeMapAnalysis;
        }

        public ProbeMapAnalysis(string sServer, int nPort)
        {
            if (string.IsNullOrEmpty(sServer))
                sServer = "localhost";
            string strUrl = string.Format("tcp://{0}:{1}", sServer, nPort);
            object obj = Activator.GetObject(typeof(DACrux.TEST.Interface.iProbeMapAnalysis),
                strUrl + "/DACrux.TEST.BSL.ProbeMapAnalysis.bin");
            m_OBJ = obj as DACrux.TEST.Interface.iProbeMapAnalysis;
            //System.Configuration.ConfigurationManager.GetSection("System.Diagnostics");
        }

        public DataSet GetWaferMap(long WaferSeq)
        {
            return m_OBJ.GetWaferMap(WaferSeq);
        }

        public DataSet GetWaferMap(string TestArea, string Product, string LotID, string WaferID, string Program = null, int ProbeCnt = 0, bool IsMaxOper = false)
        {
            return m_OBJ.GetWaferMap(TestArea, Product, Program, LotID, WaferID, ProbeCnt, IsMaxOper);
        }

        public DataSet GetCumMap(long[] WaferSeqs)
        {
            try
            {
                return m_OBJ.GetCumMap(WaferSeqs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetCumRawData(string Program, long[] WaferSeq, string[] SelBin)
        {
            return m_OBJ.GetCumRawData(Program, WaferSeq, SelBin);
        }

        public DataTable GetBinDistribution(string program, long waferSeq)
        {
            return m_OBJ.GetBinDistribution(program, waferSeq);
        }

        #region ▣ GetMapData
        public DataTable GetMapData(string program, long[] WaferSeq, string[] BINS)
        {
            return m_OBJ.GetMapData(program, WaferSeq, BINS);
        }

        public DataTable GetMapData(string Program, long WaferSeq, string paraItem, int cutCnt)
        {
            return m_OBJ.GetMapData(Program, WaferSeq, paraItem, cutCnt);
        }
        #endregion

        public DataTable GetWaferInfo(long[] WaferSeq)
        {
            return m_OBJ.GetWaferInfo(WaferSeq);
        }

        public DataTable GetYieldByDay(string[] WaferSeq)
        {
            return m_OBJ.GetYieldByDay(WaferSeq);
        }

        public DataTable GetMapDataForDefectOverlay(string PROGRAM, string WaferSeq)
        {
            return m_OBJ.GetMapDataForDefectOverlay(PROGRAM, WaferSeq);
        }

        public DataTable GetWaferInfo(long WaferSeq)
        {
            return m_OBJ.GetWaferInfo(WaferSeq);
        }


        public void CreateParaSpec(string[,] ParaInfo)
        {
            m_OBJ.CreateParaSpec(ParaInfo);
        }

        public DataTable GetParaSpecListEditable(
            string factory,
            string Program
            )
        {
            return m_OBJ.GetParaSpecListEditable(factory, Program);
        }

        public void UpdateProgramParaSpec(string Program, string[,] ParaInfo)
        {
            m_OBJ.UpdateProgramParaSpec(Program, ParaInfo);
        }

        public void DeleteProgramParaSpec(string Program)
        {
            m_OBJ.DeleteProgramParaSpec(Program);
        }



        public DataTable GetMapDataCnt(string Program, long WaferSeq, string paraItem, int cutCnt)
        {
            return m_OBJ.GetMapDataCnt(Program, WaferSeq, paraItem, cutCnt);
        }

        public DataTable GetWaferInfoByLotSlot(string lot_id, string slot_id, string testarea)
        {
            return m_OBJ.GetWaferInfoByLotSlot(lot_id, slot_id, testarea);
        }

        public DataTable GetWaferInfoByWaferId(string wafer_id, string testarea)
        {
            return m_OBJ.GetWaferInfoByWaferId(wafer_id, testarea);
        }

        public DataTable GetMapDefByLotID(string LotID)
        {
            return m_OBJ.GetMapDefByLotID(LotID);
        }

        public DataTable GetWaferInfo(string TestArea, string Product, string LotID, string WaferID)
        {
            return m_OBJ.GetWaferInfo(TestArea, Product, LotID, WaferID);
        }

        public DataTable GetBinDistribution(string PROGRAM, string[] WaferSeq, bool bMulti, bool bVI)
        {
            return m_OBJ.GetBinDistribution(PROGRAM, WaferSeq, bMulti, bVI);
        }

        public DataTable GetWaferInfo(string TestArea, string LotID, string WaferID, bool bDummy)
        {
            return m_OBJ.GetWaferInfo(TestArea, LotID, WaferID, bDummy);
        }


        public void InsertFailDie(string PRODUCT, string[] strValue)
        {
            m_OBJ.InsertFailDie(PRODUCT, strValue);
        }

        public long InsertLot(string FACILITY, string TESTAREA, string PRODUCT, string PROGRAM, string LOTID, string FABLOT, string START_TIME, string END_TIME)
        {
            return m_OBJ.InsertLot(FACILITY, TESTAREA, PRODUCT, PROGRAM, LOTID, FABLOT, START_TIME, END_TIME);
        }

        public long InsertWafer(string LOT_SEQ
                            , string WAFER_ID
                            , string TESTER
                            , string OPERATOR
                            , string TESTED_DIE
                            , string START_TIME
                            , string LOSS_DIE)
        {
            return m_OBJ.InsertWafer(LOT_SEQ, WAFER_ID, TESTER, OPERATOR, TESTED_DIE, START_TIME, LOSS_DIE);
        }



        public DataTable GetWaferInfo(string[] WaferIDs)
        {
            return m_OBJ.GetWaferInfo(WaferIDs);
        }


        #region [동부 하이텍 추가]

        public DataSet SelectWaferMapDrawData(long WaferSeq)
        {
            return m_OBJ.SelectWaferMapDrawData(WaferSeq);
        }

        public DataSet SelectWaferMapDrawData(long[] WaferSeq)
        {
            return m_OBJ.SelectWaferMapDrawData(WaferSeq);
        }

        public DataTable SelectWaferParaItem(long WaferSeq, string strParaItem, int decimalLength)
        {
            return m_OBJ.SelectWaferParaItem(WaferSeq, strParaItem, decimalLength);
        }

        public DataTable SelectWaferParaItem(long[] WaferSeq, string strParaItem, int decimalLength)
        {
            return m_OBJ.SelectWaferParaItem(WaferSeq, strParaItem, decimalLength);
        }


        public DataSet GetCumMapReport(long[] WaferSeqs)
        {
            return m_OBJ.GetCumMapReport(WaferSeqs);
        }

        public System.Data.DataSet SelectWaferMapBasic(long WaferSeq)
        {
            return m_OBJ.SelectWaferMapBasic(WaferSeq);
        }

        public System.Data.DataSet SelectWaferMapConfigMap(long WaferSeq)
        {
            return m_OBJ.SelectWaferMapConfigMap(WaferSeq);
        }

        public System.Data.DataSet SelectWaferShotMapBasic(string strDevice)
        {
            return m_OBJ.SelectWaferShotMapBasic(strDevice);
        }

        public System.Data.DataSet SelectWaferParaData(long WaferSeq, string[] strParaItem)
        {
            return m_OBJ.SelectWaferParaData(WaferSeq, strParaItem);
        }

        public System.Data.DataTable SelectWaferParaDataMulti(long[] WaferSeqList, string[] strParaItem)
        {
            return m_OBJ.SelectWaferParaDataMulti(WaferSeqList, strParaItem);
        }

        public System.Data.DataSet SelectWaferMapConfig(long WaferSeq)
        {
            return m_OBJ.SelectWaferMapConfig(WaferSeq);
        }

        public System.Data.DataTable SelectWaferShotMap(string strMapID)
        {
            return m_OBJ.SelectWaferShotMap(strMapID);
        }

        public void UpdateMapConfig(string strTestArea, string strProduct, string strProgram, string strDir, string strAngle, string strIndexX, string strIndexY, string strUser, string strcomment)
        {
            m_OBJ.UpdateMapConfig(strTestArea, strProduct, strProgram, strDir,strAngle, strIndexX, strIndexY, strUser, strcomment);
        }


        public DataTable SelectMapConfigList(string strProduct)
        {
            return m_OBJ.SelectMapConfigList(strProduct);
        }

        public DataTable SelectMapConfigView(bool bDeleteFlag)
        {
            return m_OBJ.SelectMapConfigView(bDeleteFlag);
        }

        public void DeleteMapConfig(string strSequence, string strUser, string strcomment)
        {
            m_OBJ.DeleteMapConfig(strSequence, strUser, strcomment);
        }

        public DataSet GetCumWaferReport(long[] WaferSeqs)
        {
            return m_OBJ.GetCumWaferReport(WaferSeqs);
        }

        public DataSet GetWaferBinReport(long[] WaferSeq, bool bWaferBase)
        {
            return m_OBJ.GetWaferBinReport(WaferSeq, bWaferBase);
        }

        //public System.Data.DataTable GetUserMapConfig(string strUserID, string strCategory = "")
        //{
        //    return m_OBJ.GetUserMapConfig(strUserID, strCategory);
        //}

        public DataTable GetMapConfigWaferList(string strStartTime, string strEndTime, string strTestArea, string strDevice, string strProgram)
        {
            return m_OBJ.GetMapConfigWaferList(strStartTime, strEndTime, strTestArea, strDevice, strProgram);
        }

        public void WaferMapConfigUpdate(string strWaferSeq, string strMapConfigSeq)
        {
            m_OBJ.WaferMapConfigUpdate(strWaferSeq, strMapConfigSeq);
        }

        public System.Data.DataSet SelectParaReportData(long[] WaferSeqList)
        {
            return m_OBJ.SelectParaReportData(WaferSeqList);
        }

        public System.Data.DataSet SelectAVICumData(long[] WaferSeq)
        {
            return m_OBJ.SelectAVICumData(WaferSeq);
        }

        #endregion

        #region Pattern Search

        public DataTable GetPatternSearch_WaferList(long[] waferSeqArr)
        {
            return m_OBJ.GetPatternSearch_WaferList(waferSeqArr);
        }

        public List<int> GetPatternSearch_DieList(long waferSeq)
        {
            return m_OBJ.GetPatternSearch_DieList(waferSeq);
        }

        #endregion
    }
}
