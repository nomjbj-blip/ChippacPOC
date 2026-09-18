using System;
using System.Data;

namespace DACrux.TEST.RO
{
    public class TestCommon
    {
        DACrux.TEST.Interface.iTestCommon m_OBJ;

        public TestCommon()
        {
            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_DACRUX_PRB);
            object obj = Activator.GetObject(typeof(DACrux.TEST.Interface.iTestCommon)
                , strUrl + "/DACrux.TEST.BSL.TestCommon.bin");
            m_OBJ = obj as DACrux.TEST.Interface.iTestCommon;
        }

        public void InsertDataMulti(DataTable dt)
        {
            m_OBJ.InsertDataMulti(dt);
        }

        public void ExecuteProcedureMulti(DataTable dt)
        {
            m_OBJ.ExecuteProcedureMulti(dt);
        }

        public DataTable GetStandardReport(
            DateTime dtStart,
            DateTime dtEnd,
            String factory,
            String testarea,
            String product,
            String program,
            String[] lot
            )
        {
            return m_OBJ.GetStandardReport(
                dtStart,
                dtEnd,
                factory,
                testarea,
                product,
                program,
                lot
                );
        }

        public DataSet GetPCMReport_Comp(
            DateTime dtStart,
            DateTime dtEnd,
            String factory,
            String[] testarea,
            String product,
            String program,
            String parameter,
            String[] lotids
            )
        {
            byte[] bytes = m_OBJ.GetPCMReport_Comp(
                dtStart,
                dtEnd,
                factory,
                testarea,
                product,
                program,
                parameter,
                lotids
                );

            return DACrux.Base.Util.CompressedBytesToObject(bytes) as DataSet;
        }
        
        public DataTable GetTestReportRawData(
            string fromDate,
            string toDate,
            string factory,
            string[] testarea,
            string product,
            string program,
            string[] lotid,
            string pgmParam
            )
        {
            byte[] bytes = m_OBJ.GetTestReportRawData_Comp(
                fromDate,
                toDate,
                factory,
                testarea,
                product,
                program,
                lotid,
                pgmParam
                );

            return DACrux.Base.Util.CompressedBytesToObject(bytes) as DataTable;
        }

        public DataTable GetTestReportStatistics(
            string fromDate,
            string toDate,
            string factory,
            string[] testarea,
            string product,
            string program,
            string[] lotid, 
            string pgmParam
            )
        {
            byte[] bytes = m_OBJ.GetTestReportStatistics_Comp(
                fromDate,
                toDate,
                factory,
                testarea,
                product,
                program,
                lotid,
                pgmParam
                );

            return DACrux.Base.Util.CompressedBytesToObject(bytes) as DataTable;
        }


        public void ScopeDataSet(string strWaferSeq, DACrux.Base.DieList DiesList, int iTotalDies, int iINDEX_XMAX, int iINDEX_YMAX, string strUser, bool bAlterInfo, string strAlterLotID, string strAlterWaferID, DACrux.Base.TestImageList oTestImageList)
        {
            m_OBJ.ScopeDataSet(strWaferSeq, DiesList, iTotalDies, iINDEX_XMAX, iINDEX_YMAX, strUser, bAlterInfo, strAlterLotID, strAlterWaferID, oTestImageList);
        }

        public void ScopeNewLotDataSet(string strFactory, string strUser, string strAlterLotID, string strAlterWaferID, string AlterDevice, DACrux.Base.DieList oDiesList, int iTotalDies, int iINDEX_XMAX, int iINDEX_YMAX, DACrux.Base.TestImageList oTestImageList)
        {
            m_OBJ.ScopeNewLotDataSet(strFactory, strUser, strAlterLotID, strAlterWaferID, AlterDevice, oDiesList, iTotalDies, iINDEX_XMAX, iINDEX_YMAX, oTestImageList);
        }

        /// <summary>
        /// TD 테이블의 데이터를 MERGE 구문으로 저장 합니다.
        /// </summary>
        public void MergeTdTableData(string factory, DataTable source)
        {
            m_OBJ.MergeTdTableData(factory, source);
        }

         /// <summary>
        /// AVI 테이블의 데이터를 MERGE 구문으로 처리 합니다.
        /// </summary>
        public void MergeAVITableData(string factory, DataTable source)
        {
            m_OBJ.MergeAVITableData(factory, source);
        }

        /// <summary>
        /// 데이터 변경 이력 정보를 추가합니다.
        /// </summary>
        public void InsertChangeHis(string factory, string testArea, string product, string program, string lotID, string waferID,
            string tranUser, string tranUserIP, string category, string tranType, int tranCount, string prevValue, string currValue, string userComment)
        {
            m_OBJ.InsertChangeHis(factory, testArea, product, program, lotID, waferID,
                tranUser, tranUserIP, category, tranType, tranCount, prevValue, currValue, userComment);
        }

        /// <summary>
        /// 데이터 변경 이력 데이터를 가져옵니다.
        /// </summary>
        public DataTable GetChangeHisData(string factory, string testArea, string fromDate, string toDate)
        {
            return m_OBJ.GetChangeHisData(factory, testArea, fromDate, toDate);
        }

        /// <summary>
        /// PCM 설비 상태를 가져옵니다.
        /// </summary>
        public DataTable GetPcmEquipStatus(string factory)
        {
            return m_OBJ.GetPcmEquipStatus(factory);
        }

        /// <summary>
        /// Input Data 이력을 가져옵니다.
        /// </summary>
        public DataTable GetPcmInputDataHistory(string factory, string equipID, string fromDate, string toDate)
        {
            return m_OBJ.GetPcmInputDataHistory(factory, equipID, fromDate, toDate);
        }

        /// <summary>
        /// PCM Input 데이터를 추가 합니다.
        /// </summary>
        public void InsertPcmInputData(string factory, string equipID, string probeCard, string worker, string loginUser)
        {
            m_OBJ.InsertPcmInputData(factory, equipID, probeCard, worker, loginUser);
        }

        #region Test DM Wafer Map Matching

        public DataSet SelectWaferMapBasic(long WaferSeq)
        {
            return m_OBJ.SelectWaferMapBasic(WaferSeq);
        }

        public DataSet GetMatchDefectData(string strLotID, string strSlotID, string strUser)
        {
            return m_OBJ.GetMatchDefectData(strLotID, strSlotID, strUser);
        }

        #endregion Test DM Wafer Map Matching
    }
}
