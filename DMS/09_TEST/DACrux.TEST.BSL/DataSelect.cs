using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using DACrux.TEST.DSL;
//using System.Transactions;
using DACrux.TEST.Interface;
using DACrux.Common.DSL;

namespace DACrux.TEST.BSL
{
    public class DataSelect : Miracom.Middleware.BaseComponent, iDataSelect
    {
        public DataTable GetLotList(string StartTime, string EndTime, string[] strFields, string[] strWhere, string strSort)
        {
            TQP_INLINE oDSL = null;
            DataTable dt = null;

            try
            {
                oDSL = new TQP_INLINE();
                dt = oDSL.GetLotList(StartTime, EndTime, strFields, strWhere, strSort);
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
        //    TQP_WAFER oDSL = null;
        //    try
        //    {
        //        oDSL = new TQP_WAFER();
        //        return oDSL.GetWaferInfo(DACrux.Base.Convert.longParse(WAFERSEQ));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public DataTable GetWaferInfo(long WaferSeq)
        //{
        //    TQP_WAFER oDSL = null;
        //    try
        //    {
        //        oDSL = new TQP_WAFER();
        //        return oDSL.GetWaferInfo(WaferSeq);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataTable GetWaferList(DateTime stime, DateTime etime, string fieldOrder)
        {
            TQP_WAFER oDSL = null;
            DataTable dt = null;
            try
            {
                oDSL = new TQP_WAFER();
                dt = oDSL.GetWaferList(stime, etime, fieldOrder);
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

        public DataTable GetWaferList(string StartTime, string EndTime, string[] strFields, string[] strWhere, string strSort)
        {
            TQP_WAFER_SUM oDSL = null;
            try
            {
                oDSL = new TQP_WAFER_SUM();
                return oDSL.GetWaferList(StartTime, EndTime, strFields, strWhere, strSort);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetBinSum(string BINLIST, string WAFER_SEQ)
        {
            TQP_WAFER_SUM oDSL = null;

            try
            {
                oDSL = new TQP_WAFER_SUM();
                return oDSL.GetBinSum(BINLIST, WAFER_SEQ);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetAVIWaferSum(string PROGRAM, string[] WaferSeq)
        {
            TQP_WAFER_SUM oDSL = null;
            try
            {
                oDSL = new TQP_WAFER_SUM();
                return oDSL.GetAVIWaferSum(PROGRAM, WaferSeq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferInfo(long[] WaferSeq)
        {
            TQP_WAFER_SUM oDSL = null;
            try
            {
                oDSL = new TQP_WAFER_SUM();
                return oDSL.GetWaferInfo(WaferSeq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWaferInfo()
        {
            TQP_WAFER_SUM oDSL = null;
            try
            {
                oDSL = new TQP_WAFER_SUM();
                return oDSL.GetWaferInfo();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 동부하이텍

        public DataTable GetTESTWaferList(string StartTime, string EndTime, string[] strWhere, string strSort, bool TimeNotCheck, bool LastTest)
        {
            TQP_WAFER_SUM oDSL = null;
            StringBuilder sQuery = new StringBuilder();

            try
            {
                oDSL = new TQP_WAFER_SUM();

                oDSL = new TQP_WAFER_SUM();

                //시간을 지정 하지않을 경우 조건에서 시간을 뺀다.
                if (TimeNotCheck != true)
                {
                    sQuery.AppendLine(" AND END_TIME BETWEEN ");
                    sQuery.AppendLine(string.Format("     TO_DATE ( '{0}', 'YYYY-MM-DD') ", StartTime));
                    sQuery.AppendLine(string.Format(" AND TO_DATE ( '{0}', 'YYYY-MM-DD') ", EndTime));
                }

                if (LastTest == true)
                    sQuery.AppendLine(" AND PROBE_CNT = 0 ");

                foreach (string strVal in strWhere)
                    sQuery.AppendLine(string.Format(" AND {0} ", strVal));

                //if (string.IsNullOrEmpty(strSort) == false)
                //    sQuery.AppendLine(string.Format(" ORDER BY {0} ", strSort));

                return oDSL.GetData("SELECT_TEST_WAFER_LIST", new string[] { sQuery.ToString() }, null);
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
            TQP_PROGRAM obj = new TQP_PROGRAM();
            return obj.GetConditionTestArea();
        }

        /// <summary>
        /// 해당 TESTAREA의 DEVICE 리스트를 가져옵니다.
        /// </summary>
        public DataTable GetConditionDevice(string fromDate, string toDate, string[] testAreaArr)
        {
            TQP_LOT obj = new TQP_LOT();
            return obj.GetConditionDevice(fromDate, toDate, testAreaArr);
        }

        /// <summary>
        /// 해당 TESTAREA, DEVICE의 PROGRAM 리스트를 가져옵니다.
        /// </summary>
        public DataTable GetConditionProgram(string fromDate, string toDate, string[] testAreaArr, string[] deviceArr)
        {
            TQP_LOT obj = new TQP_LOT();
            return obj.GetConditionProgram(fromDate, toDate, testAreaArr, deviceArr);
        }

        /// <summary>
        /// 해당 기간에 대한 LOT 정보를 가져옵니다.
        /// </summary>
        public DataTable GetLotInfo(string testArea, string fromDate, string toDate, string lotID)
        {
            TQP_LOT obj = new TQP_LOT();
            return obj.GetLotInfo(testArea, fromDate, toDate, lotID);
        }

        /// <summary>
        /// 해당 기간에 대한 LOT 정보를 가져옵니다.
        /// </summary>
        public DataTable GetLotInfo(string[] testArea, string fromDate, string toDate, string lotID)
        {
            TQP_LOT obj = new TQP_LOT();
            return obj.GetLotInfo(testArea, fromDate, toDate, lotID);
        }

        /// <summary>
        /// 해당 PROGRAM의 주어진 기간에 대한 LOT 리스트를 가져옵니다.
        /// </summary>
        public DataTable GetConditionLot(string fromDate, string toDate, string[] programArr)
        {
            TQP_LOT obj = new TQP_LOT();
            return obj.GetConditionLot(fromDate, toDate, programArr);
        }

        /// <summary>
        /// 해당 PROGRAM의 주어진 기간에 대한 LOT 리스트를 가져옵니다.
        /// </summary>
        public DataTable GetConditionLot(string fromDate, string toDate, string[] testareaArr, string[] productArr, string[] programArr)
        {
            TQP_LOT obj = new TQP_LOT();
            return obj.GetConditionLot(fromDate, toDate, testareaArr, productArr, programArr);
        }

        /// <summary>
        /// 해당 LOT의 WAFER 리스트를 가져옵니다.
        /// </summary>
        public DataTable GetConditionWafer(string program, string[] lotSeqArr)
        {
            TQP_WAFER obj = new TQP_WAFER();
            return obj.GetConditionWafer(program, lotSeqArr);
        }

        /// <summary>
        /// 해당 WAFER의 PROGRAM 리스트를 가져옵니다.
        /// </summary>
        //public string[] GetProgramByWafer(string[] waferSeqArr)
        //{
        //    TQP_WAFER obj = new TQP_WAFER();
        //    return obj.GetProgram(waferSeqArr);
        //}

        /// <summary>
        /// 해당 LOT의 PROGRAM 리스트를 가져옵니다.
        /// </summary>
        public string[] GetProgramByLot(string[] lotSeqArr)
        {
            TQP_LOT obj = new TQP_LOT();
            return obj.GetProgram(lotSeqArr);
        }

        /// <summary>
        /// WAFER_SEQ에 대한 RAW DATA를 가져옵니다. 여러 테이블인 경우 모든 데이터를 가져옵니다.
        /// </summary>
        public byte[] GetAllRawData_Comp(string[] waferSeqArr)
        {
            return DACrux.Base.Util.ObjectToCompressedBytes(
                GetAllRawData(waferSeqArr));
        }

        /// <summary>
        /// WAFER_SEQ에 대한 RAW DATA를 가져옵니다. 여러 테이블인 경우 모든 데이터를 가져옵니다.
        /// </summary>
        public DataTable GetAllRawData(string[] waferSeqArr)
        {
            TD_TABLE obj = new TD_TABLE();
            return obj.GetAllRawData(waferSeqArr);
        }

        ///  WAFER_SEQ에 대한 AVI RAW DATA를 가져옵니다. 여러 테이블인 경우 모든 데이터를 가져옵니다.
        /// </summary>
        /// <param name="waferSeqArr"></param>
        /// <returns></returns>
        public DataTable GetAVIRawData(string[] waferSeqArr)
        {
            DACrux.TEST.DSL.TQP_FOI_DIE oDie = new TQP_FOI_DIE();
            return oDie.SelectFOIDataMultiIRaw(waferSeqArr);
        }


        #endregion
    }
}
