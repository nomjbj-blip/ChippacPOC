using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.TEST.Interface
{
    public interface iDataSelect
    {
        DataTable GetLotList(string StartTime, string EndTime, string[] strFields, string[] strWhere, string strSort);
        //DataTable GetWaferInfo(string WAFERSEQ);
        DataTable GetWaferList(DateTime stime, DateTime etime, string fieldOrder);
        DataTable GetWaferList(string StartTime, string EndTime, string[] strFields, string[] strWhere, string strSort);
        DataTable GetBinSum(string BINLIST, string WAFER_SEQ);
        DataTable GetAVIWaferSum(string PROGRAM, string[] WaferSeq);
        DataTable GetWaferInfo(long[] WaferSeq);
        DataTable GetWaferInfo();
       
        #region 동부하이텍

        DataTable GetTESTWaferList(string StartTime, string EndTime, string[] strWhere, string strSort, bool TimeNotCheck, bool LastTest);

        /// <summary>
        /// TESTAREA를 가져옵니다.
        /// </summary>
        /// <returns></returns>
        DataTable GetConditionTestArea();
        /// <summary>
        /// 해당 TESTAREA의 DEVICE 리스트를 가져옵니다.
        /// </summary>
        DataTable GetConditionDevice(string fromDate, string toDate, string[] testAreaArr);
        /// <summary>
        /// 해당 TESTAREA, DEVICE의 PROGRAM 리스트를 가져옵니다.
        /// </summary>
        DataTable GetConditionProgram(string fromDate, string toDate, string[] testAreaArr, string[] deviceArr);
        /// <summary>
        /// 해당 기간에 대한 LOT 정보를 가져옵니다.
        /// </summary>
        DataTable GetLotInfo(string testArea, string fromDate, string toDate, string lotID);
        DataTable GetLotInfo(string[] testArea, string fromDate, string toDate, string lotID);

        /// <summary>
        /// 해당 PROGRAM의 주어진 기간에 대한 LOT 리스트를 가져옵니다.
        /// </summary>
        DataTable GetConditionLot(string fromDate, string toDate, string[] programArr);
        DataTable GetConditionLot(string fromDate, string toDate, string[] testareaArr, string[] productArr, string[] programArr);

        /// <summary>
        /// 해당 LOT의 WAFER 리스트를 가져옵니다.
        /// </summary>
        DataTable GetConditionWafer(string program, string[] lotSeqArr);
        /// <summary>
        /// 해당 WAFER의 PROGRAM 리스트를 가져옵니다.
        /// </summary>
        //string[] GetProgramByWafer(string[] waferSeqArr);
        /// <summary>
        /// 해당 LOT의 PROGRAM 리스트를 가져옵니다.
        /// </summary>
        string[] GetProgramByLot(string[] lotSeqArr);
        /// <summary>
        /// WAFER_SEQ에 대한 RAW DATA를 가져옵니다. 여러 테이블인 경우 모든 데이터를 가져옵니다.
        /// </summary>
        DataTable GetAllRawData(string[] waferSeqArr);
        /// <summary>
        /// WAFER_SEQ에 대한 RAW DATA를 가져옵니다. 여러 테이블인 경우 모든 데이터를 가져옵니다. (압축 적용 버전)
        /// </summary>
        byte[] GetAllRawData_Comp(string[] waferSeqArr);
        ///  WAFER_SEQ에 대한 AVI RAW DATA를 가져옵니다. 여러 테이블인 경우 모든 데이터를 가져옵니다.
        /// </summary>
        /// <param name="waferSeqArr"></param>
        /// <returns></returns>
        DataTable GetAVIRawData(string[] waferSeqArr);

        #endregion
    }
}
