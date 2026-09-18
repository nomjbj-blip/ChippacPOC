using System;
using System.Collections.Generic;
using System.Text;
using DACrux.TEST.Interface;
using System.Data;
using System.IO;

namespace DACrux.TEST.RO
{
    public class DataSelect
    {
        iDataSelect m_OBJ;

        public DataSelect()
        {
           string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_DACRUX_PRB);
           object obj = Activator.GetObject(typeof(DACrux.TEST.Interface.iDataSelect),
                        strUrl + "/DACrux.TEST.BSL.DataSelect.bin");
            m_OBJ = obj as DACrux.TEST.Interface.iDataSelect;
        }

        public DataTable GetLotList(string StartTime, string EndTime, string[] strFields, string[] strWhere, string strSort)
        {
            DataTable dt = null;
            try
            {
                dt = m_OBJ.GetLotList(StartTime, EndTime, strFields, strWhere, strSort);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        //public DataTable GetWaferInfo(string WAFERSEQ)
        //{
        //    try
        //    {
        //        return m_OBJ.GetWaferInfo(WAFERSEQ);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataTable GetWaferList(DateTime stime, DateTime etime, string fieldOrder)
        {
            try
            {
                return m_OBJ.GetWaferList(stime, etime, fieldOrder);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferList(string StartTime, string EndTime, string[] strFields, string[] strWhere, string strSort)
        {
            try
            {
                return m_OBJ.GetWaferList(StartTime, EndTime, strFields, strWhere, strSort);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetBinSum(string BINLIST, string WAFER_SEQ)
        {
            try
            {
                return m_OBJ.GetBinSum(BINLIST, WAFER_SEQ);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAVIWaferSum(string PROGRAM, string[] WaferSeq)
        {
            try
            {
                return m_OBJ.GetAVIWaferSum(PROGRAM, WaferSeq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferInfo(long[] WaferSeq)
        {
            try
            {
                return m_OBJ.GetWaferInfo(WaferSeq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public DataTable GetWaferInfo()
        {
            try
            {
                return m_OBJ.GetWaferInfo();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 동부하이텍

        public DataTable GetTESTWaferList(string StartTime, string EndTime, string[] strWhere, string strSort, bool TimeNotCheck, bool LastTest)
        {
            try
            {
                return m_OBJ.GetTESTWaferList(StartTime, EndTime, strWhere, strSort, TimeNotCheck, LastTest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// TESTAREA를 가져옵니다.
        /// </summary>
        /// <returns></returns>
        public DataTable GetConditionTestArea()
        {
            return m_OBJ.GetConditionTestArea();
        }
        

        /// <summary>
        /// 해당 TESTAREA의 DEVICE 리스트를 가져옵니다.
        /// </summary>
        public DataTable GetConditionDevice(string fromDate, string toDate, string[] testAreaArr)
        {
            return m_OBJ.GetConditionDevice(fromDate, toDate, testAreaArr);
        }

        /// <summary>
        /// 해당 TESTAREA, DEVICE의 PROGRAM 리스트를 가져옵니다.
        /// </summary>
        public DataTable GetConditionProgram(string fromDate, string toDate, string[] testAreaArr, string[] deviceArr)
        {
            return m_OBJ.GetConditionProgram(fromDate, toDate, testAreaArr, deviceArr);
        }

        /// <summary>
        /// 해당 기간에 대한 LOT 정보를 가져옵니다.
        /// </summary>
        public DataTable GetLotInfo(string testArea, string fromDate, string toDate, string lotID)
        {
            return m_OBJ.GetLotInfo(testArea, fromDate, toDate, lotID);
        }

        public DataTable GetLotInfo(string[] testArea, string fromDate, string toDate, string lotID)
        {
            return m_OBJ.GetLotInfo(testArea, fromDate, toDate, lotID);
        }

        /// <summary>
        /// 해당 PROGRAM의 주어진 기간에 대한 LOT 리스트를 가져옵니다.
        /// </summary>
        public DataTable GetConditionLot(string fromDate, string toDate, string[] programArr)
        {
            return m_OBJ.GetConditionLot(fromDate, toDate, programArr);
        }

        /// <summary>
        /// 해당 PROGRAM의 주어진 기간에 대한 LOT 리스트를 가져옵니다.
        /// </summary>
        public DataTable GetConditionLot(string fromDate, string toDate, string[] testareaArr, string[] productArr, string[] programArr)
        {
            return m_OBJ.GetConditionLot(fromDate, toDate, testareaArr, productArr, programArr);
        }

        /// <summary>
        /// 해당 LOT의 WAFER 리스트를 가져옵니다.
        /// </summary>
        public DataTable GetConditionWafer(string program, string[] lotSeqArr)
        {
            return m_OBJ.GetConditionWafer(program, lotSeqArr);
        }

        /// <summary>
        /// 해당 LOT의 PROGRAM 리스트를 가져옵니다.
        /// </summary>
        //public string[] GetProgramByWafer(string[] waferSeqArr)
        //{
        //    return m_OBJ.GetProgramByWafer(waferSeqArr);
        //}

        /// <summary>
        /// 해당 LOT의 PROGRAM 리스트를 가져옵니다.
        /// </summary>
        public string[] GetProgramByLot(string[] lotSeqArr)
        {
            return m_OBJ.GetProgramByLot(lotSeqArr);
        }

        /// <summary>
        /// WAFER_SEQ에 대한 RAW DATA를 가져옵니다. 여러 테이블인 경우 모든 데이터를 가져옵니다.
        /// </summary>
        public DataTable GetAllRawData(string[] waferSeqArr)
        {
            return m_OBJ.GetAllRawData(waferSeqArr);
        }

        /// <summary>
        /// WAFER_SEQ에 대한 RAW DATA를 가져옵니다. 여러 테이블인 경우 모든 데이터를 가져옵니다. (압축 적용 버전)
        /// </summary>
        public DataTable GetAllRawData_Comp(string[] waferSeqArr)
        {
            byte[] bytes = m_OBJ.GetAllRawData_Comp(waferSeqArr);
            return DACrux.Base.Util.CompressedBytesToObject(bytes) as DataTable;
        }
         ///  WAFER_SEQ에 대한 AVI RAW DATA를 가져옵니다. 여러 테이블인 경우 모든 데이터를 가져옵니다.
        /// </summary>
        /// <param name="waferSeqArr"></param>
        /// <returns></returns>
        public DataTable GetAVIRawData(string[] waferSeqArr)
        {
            return m_OBJ.GetAVIRawData(waferSeqArr);
        }

        #endregion
    }
}
