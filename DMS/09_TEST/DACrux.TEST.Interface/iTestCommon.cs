using System.Data;
using System;

namespace DACrux.TEST.Interface
{
    public interface iTestCommon
    {
        void InsertDataMulti(DataTable dt);
        void ExecuteProcedureMulti(DataTable dt);

        byte[] GetPCMReport_Comp(DateTime dtStart, DateTime dtEnd, String factory, String[] testarea, String product, String program, String parameter, String[] lotids);
        DataSet GetPCMReport(DateTime dtStart, DateTime dtEnd, String factory, String[] testarea, String product, String program, String parameter, String[] lotids);

        byte[] GetTestReportStatistics_Comp(string fromDate, string toDate, string factory, string[] testarea, string product, string program, string[] lotid, string pgmParam);
        byte[] GetTestReportRawData_Comp(string fromDate, string toDate, string factory, string[] testarea, string product, string program, string[] lotid, string pgmParam);

        DataTable GetStandardReport(DateTime dtStart, DateTime dtEnd, String factory, String testarea, String product, String program, String[] lot);

        void ScopeDataSet(string strWaferSeq, DACrux.Base.DieList DiesList, int iTotalDies, int iINDEX_XMAX, int iINDEX_YMAX, string strUser, bool bAlterInfo, string strAlterLotID, string strAlterWaferID, DACrux.Base.TestImageList oTestImageList);
        void ScopeNewLotDataSet(string strFactory, string strUser, string strAlterLotID, string strAlterWaferID, string AlterDevice, DACrux.Base.DieList oDiesList, int iTotalDies, int iINDEX_XMAX, int iINDEX_YMAX, DACrux.Base.TestImageList oTestImageList);

        /// <summary>
        /// TD 테이블의 데이터를 MERGE 구문으로 처리 합니다.
        /// </summary>
        void MergeTdTableData(string factory, DataTable source);

         /// <summary>
        /// AVI 테이블의 데이터를 MERGE 구문으로 처리 합니다.
        /// </summary>
        void MergeAVITableData(string factory, DataTable source);

        /// <summary>
        /// 데이터 변경 이력 정보를 추가합니다.
        /// </summary>
        void InsertChangeHis(string factory, string testArea, string product, string program, string lotID, string waferID,
            string tranUser, string tranUserIP, string category, string tranType, int tranCount, string prevValue, string currValue, string userComment);

        /// <summary>
        /// 데이터 변경 이력 데이터를 가져옵니다.
        /// </summary>
        DataTable GetChangeHisData(string factory, string testArea, string fromDate, string toDate);

        /// <summary>
        /// PCM 설비 상태를 가져옵니다.
        /// </summary>
        DataTable GetPcmEquipStatus(string factory);

        /// <summary>
        /// Input Data 이력을 가져옵니다.
        /// </summary>
        DataTable GetPcmInputDataHistory(string factory, string equipID, string fromDate, string toDate);

        /// <summary>
        /// PCM Input 데이터를 추가 합니다.
        /// </summary>
        void InsertPcmInputData(string factory, string equipID, string probeCard, string worker, string loginUser);

        #region Test DM Wafer Map Matching

        System.Data.DataSet SelectWaferMapBasic(long WaferSeq);
        System.Data.DataSet GetMatchDefectData(string strLotID, string strSlotID, string strUser);

        #endregion Test DM Wafer Map Matching
    }
}
