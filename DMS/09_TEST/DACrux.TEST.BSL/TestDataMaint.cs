using System;
using DACrux.TEST.DSL;
using System.Data;
using DACrux.TEST.Interface;

namespace DACrux.TEST.BSL
{
    public class TestDataMaint : Miracom.Middleware.BaseComponent, iTestDataMaint
    {
        /// <summary>
        /// WAFER의 LOT ID를 업데이트 합니다.
        /// </summary>
        public void UpdateWaferLotID(string factory, string lotSeq, string waferSeq, string newLotID)
        {
            // 새로 바뀐 LOT에 대한 LOT_SEQ 가져오기(없으면 신규 채번)
            string newLotSeq = GetNewLotSeqByNewLotID(lotSeq, newLotID);
         
            TQP_LOT lot = new TQP_LOT();
            DataRow currLotRow = lot.GetLotInfo(lotSeq);
            DataRow newLotRow = lot.GetLotInfo(newLotSeq);

            string program = currLotRow["PROGRAM"].ToString();

            // 신규 LOT과 현재 LOT의 PROGRAM 이 다르면 데이터 옮기는 작업을 진행한다.
            if (program != newLotRow["PROGRAM"].ToString())
            {
                program = newLotRow["PROGRAM"].ToString();

                // RAW데이터 전체 가져오기
                TD_TABLE td = new TD_TABLE();
                DataTable rawDt = td.GetAllRawDataOnlyPara(waferSeq);

                // 데이터가 있는 경우 새 PROGRAM으로 데이터 추가
                if (rawDt != null && rawDt.Rows.Count > 0)
                    TestDataManager.SaveData(factory, program, rawDt, waferSeq);

                // 기존 TD 테이블 목록 가져오기
                TQP_DATA_TABLES tbl = new TQP_DATA_TABLES();
                string[] tableNames = tbl.GetTableNameByWafer(waferSeq);

                // 기존 TD 테이블의 데이터 삭제
                if (tableNames != null && tableNames.Length > 0)
                {
                    foreach (string tableName in tableNames)
                    {
                        td.DeleteData(waferSeq, tableName);

                        // 빈 테이블인지 확인 후 조치
                        TestDataManager tst = new TestDataManager();
                        tst.CheckTableEmpty(tableName);
                    }
                }
            }

            TQP_WAFER waf = new TQP_WAFER();
            TQP_LOT_SUM lotSum = new TQP_LOT_SUM();
            TQP_WAFER_SUM wafSum = new TQP_WAFER_SUM();

            // 해당 WAFER의 LOT_SEQ, PROGRAM 업데이트
            waf.UpdateProgram(waferSeq, newLotSeq, program, -1);
            // PROBE_CNT 1씩 증가
            waf.IncreaseProbeCount(newLotSeq, waferSeq);

            // WAFER SUM의 LOT_SEQ, PROGRAM, PROBE_COUNT 업데이트
            wafSum.UpdateProgram(waferSeq, newLotSeq, program, -1);
            // PROBE_CNT 1씩 증가
            wafSum.IncreaseProbeCount(newLotSeq, waferSeq);

            // 기존 LOT 삭제여부 체크
            DeleteLotSeqIfEmpty(lotSeq);

            // LOT SUM 삭제 후 새로 생성
            lotSum.DeleteLot(newLotSeq);
            lotSum.InsertLotSummary(newLotSeq);
        }

        /// <summary>
        /// WAFER ID를 업데이트 합니다.
        /// </summary>
        public void UpdateWaferID(string waferSeq, string waferID, int probeCnt)
        {
            TQP_WAFER waf = new TQP_WAFER();
            waf.UpdateWaferID(waferSeq, waferID, probeCnt);

            TQP_WAFER_SUM sum = new TQP_WAFER_SUM();
            sum.UpdateWaferID(waferSeq, waferID, probeCnt);
        }

        /// <summary>
        /// WAFER의 PROGRAM 명을 변경 합니다.
        /// </summary>
        public void UpdateWaferProgram(string factory, string lotSeq, string waferSeq, string program)
        {
            // RAW데이터 전체 가져오기
            TD_TABLE td = new TD_TABLE();
            DataTable rawDt = td.GetAllRawDataOnlyPara(waferSeq);
            
            // 데이터가 있는 경우 새 PROGRAM으로 데이터 추가
            if (rawDt != null && rawDt.Rows.Count > 0)
                TestDataManager.SaveData(factory, program, rawDt, waferSeq);

            // 기존 TD 테이블 목록 가져오기
            TQP_DATA_TABLES tbl = new TQP_DATA_TABLES();
            string[] tableNames = tbl.GetTableNameByWafer(waferSeq);

            // TD 테이블의 데이터 삭제
            if (tableNames != null && tableNames.Length > 0)
            {
                foreach (string tableName in tableNames)
                {
                    td.DeleteData(waferSeq, tableName);

                    // 빈 테이블인지 확인 후 조치
                    TestDataManager tst = new TestDataManager();
                    tst.CheckTableEmpty(tableName);
                }
            }

            TQP_LOT lot = new TQP_LOT();
            TQP_WAFER waf = new TQP_WAFER();
            TQP_LOT_SUM lotSum = new TQP_LOT_SUM();
            TQP_WAFER_SUM wafSum = new TQP_WAFER_SUM();

            // 새로 바뀐 PROGRAM에 대한 LOT_SEQ 가져오기(없으면 신규 채번)
            string newLotSeq = GetNewLotSeqByProgram(program, lotSeq);
            
            // 해당 WAFER의 LOT_SEQ, PROGRAM 업데이트
            waf.UpdateProgram(waferSeq, newLotSeq, program, -1);
            // PROBE_CNT 1씩 증가
            waf.IncreaseProbeCount(newLotSeq, waferSeq);

            // WAFER SUM의 LOT_SEQ, PROGRAM, PROBE_COUNT 업데이트
            wafSum.UpdateProgram(waferSeq, newLotSeq, program, -1);
            // PROBE_CNT 1씩 증가
            wafSum.IncreaseProbeCount(newLotSeq, waferSeq);

            // 기존 LOT 삭제여부 체크
            DeleteLotSeqIfEmpty(lotSeq);

            // LOT SUM 삭제 후 새로 생성
            lotSum.DeleteLot(newLotSeq);
            lotSum.InsertLotSummary(newLotSeq);
        }

        /// <summary>
        /// 새로 바뀐 PROGRAM에 대한 Lot Seq 가져오기(없으면 신규 채번)
        /// </summary>
        private string GetNewLotSeqByProgram(string program, string currLotSeq)
        {
            TQP_LOT lot = new TQP_LOT();

            // 현재 LOT_SEQ 로 기본 정보 조회
            DataRow currLotRow = lot.GetLotInfo(currLotSeq);

            // 해당 LOT_ID, TEST_AREA, PROGRAM 으로 LOT_SEQ 가 존재하는지 체크
            DataRow newLotRow = lot.GetLotInfo(currLotRow["LOT_ID"].ToString(), currLotRow["TEST_AREA"].ToString(), program);

            // 없으면 신규로 추가
            if (newLotRow == null)
            {
                // 신규 LOT_SEQ 채번
                string newLotSeq = lot.GetNewLotSeq();

                // 기존 LOT 데이터 기준으로 INSERT 한다.
                DataTable newLotDt = currLotRow.Table.Copy();
                newLotDt.TableName = "TQP_LOT";

                newLotRow = newLotDt.Rows[0];
                newLotRow["LOT_SEQ"] = newLotSeq;
                newLotRow["PROGRAM"] = program;
                lot.InsertData(newLotDt);
            }

            return newLotRow["LOT_SEQ"].ToString();
        }

        /// <summary>
        /// 새로 바뀐 PROGRAM에 대한 Lot Seq 가져오기(없으면 신규 채번)
        /// </summary>
        private string GetNewLotSeqByNewLotID(string currLotSeq, string newLotID)
        {
            TQP_LOT lot = new TQP_LOT();

            // 현재 LOT_SEQ 로 기본 정보 조회
            DataRow currLotRow = lot.GetLotInfo(currLotSeq);

            // 해당 LOT_ID, TEST_AREA, PROGRAM 으로 LOT_SEQ 가 존재하는지 체크
            DataRow newLotRow = lot.GetLotInfo(newLotID, currLotRow["TEST_AREA"].ToString(), currLotRow["PROGRAM"].ToString());

            // 없으면 신규로 추가
            if (newLotRow == null)
            {
                // 신규 LOT_SEQ 채번
                string newLotSeq = lot.GetNewLotSeq();

                // 기존 LOT 데이터 기준으로 INSERT 한다.
                DataTable newLotDt = currLotRow.Table.Copy();
                newLotDt.TableName = "TQP_LOT";

                newLotRow = newLotDt.Rows[0];
                newLotRow["LOT_SEQ"] = newLotSeq;
                newLotRow["LOT_ID"] = newLotID;
                lot.InsertData(newLotDt);
            }

            return newLotRow["LOT_SEQ"].ToString();
        }

        /// <summary>
        /// WAFER 데이터를 삭제합니다.
        /// </summary>
        public void DeleteWaferData(string waferSeq)
        {
            TQP_WAFER waf = new TQP_WAFER();

            DataRow waferRow = waf.GetWaferInfo(waferSeq);
            waf.DeleteWaferData(waferSeq);

            TQP_WAFER_SUM sum = new TQP_WAFER_SUM();
            waf.DeleteWaferData(waferSeq);

            // TD 테이블 목록 가져오기
            TQP_DATA_TABLES tbl = new TQP_DATA_TABLES();
            string[] tableNames = tbl.GetTableNameByWafer(waferSeq);

            if (tableNames == null || tableNames.Length == 0)
                return;

            TD_TABLE td = new TD_TABLE();

            // TD 테이블의 데이터 삭제
            foreach (string tableName in tableNames)
            {
                td.DeleteData(waferSeq, tableName);

                // 빈 테이블인지 확인 후 조치
                TestDataManager tst = new TestDataManager();
                tst.CheckTableEmpty(tableName);
            }

            TQP_LOT_SUM lotSum = new TQP_LOT_SUM();
            string lotSeq = waferRow["LOT_SEQ"].ToString();

            // LOT SUM 삭제 후 새로 생성
            lotSum.DeleteLot(lotSeq);
            lotSum.InsertLotSummary(lotSeq);

            // LOT 데이터 삭제여부 체크
            DeleteLotSeqIfEmpty(lotSeq);
        }

        /// <summary>
        /// 해당 LotSeq에 대한 Wafer 데이터가 한건도 없는 경우 삭제 처리
        /// </summary>
        private void DeleteLotSeqIfEmpty(string lotSeq)
        {
            // Get Wafer Count
            TQP_WAFER waf = new TQP_WAFER();
            int count = Int32.Parse(waf.GetWaferCount(lotSeq).Rows[0][0].ToString());

            if (count > 0)
                return;

            // TQP_LOT_SUM 삭제
            TQP_LOT_SUM sum = new TQP_LOT_SUM();
            sum.DeleteLotData(lotSeq);

            // TQP_LOT 삭제
            TQP_LOT lot = new TQP_LOT();
            lot.DeleteLot(lotSeq);
        }

        /// <summary>
        /// LOT ID를 업데이트 합니다.
        /// </summary>
        public void UpdateLotID(string lotSeq, string lotID)
        {
            // TQC_LOT_STS 에서 4자리 DEVICE 가져오기
            string deviceAlias = null;
            TestCommon cmn = new TestCommon();
            DataTable dt = cmn.GetLotStatusInfoToTable(lotID);

            if (dt != null && dt.Rows.Count > 0)
                deviceAlias = dt.Rows[0]["MASK_ID"].ToString();

            if (String.IsNullOrEmpty(deviceAlias))
                deviceAlias = "NONE";

            TQP_LOT lot = new TQP_LOT();
            lot.UpdateLotID(lotSeq, lotID, deviceAlias);

            TQP_LOT_SUM lotSum = new TQP_LOT_SUM();
            lotSum.UpdateLotID(lotSeq, lotID, deviceAlias);

            TQP_WAFER waf = new TQP_WAFER();
            waf.UpdateLotID(lotSeq, lotID);

            TQP_WAFER_SUM wafSum = new TQP_WAFER_SUM();
            wafSum.UpdateLotID(lotSeq, lotID, deviceAlias);
        }

        /// <summary>
        /// LOT의 PROGRAM 명을 변경 합니다.
        /// </summary>
        public void UpdateLotProgram(string factory, string lotSeq, string currProgram, string newProgram)
        {
            TQP_WAFER waf = new TQP_WAFER();
            string[] waferSeqArr = waf.GetWaferSeqByLot(lotSeq, currProgram);

            foreach (string waferSeq in waferSeqArr)
            {
                UpdateWaferProgram(factory, lotSeq, waferSeq, newProgram);
            }
        }

        /// <summary>
        /// 해당 lot seq의 데이터를 모두 삭제 합니다.
        /// </summary>
        public void DeleteLotData(string lotSeq, string program)
        {
            // TD 테이블 목록 가져오기
            TQP_DATA_TABLES tbl = new TQP_DATA_TABLES();
            string[] tableNames = tbl.GetTableNameByLot(lotSeq, program);

            TQP_LOT lot = new TQP_LOT();
            TQP_LOT_SUM lotSum = new TQP_LOT_SUM();
            TQP_WAFER waf = new TQP_WAFER();
            TQP_WAFER_SUM wafSum = new TQP_WAFER_SUM();
            TD_TABLE td = new TD_TABLE();

            string[] waferSeqArr = waf.GetWaferSeqByLot(lotSeq, program);

            // TD 테이블의 데이터 삭제
            if (tableNames != null && tableNames.Length > 0)
            {
                foreach (string tableName in tableNames)
                {
                    td.DeleteData(waferSeqArr, tableName);

                    // 빈 테이블인지 확인
                    TestDataManager tst = new TestDataManager();
                    tst.CheckTableEmpty(tableName);
                }
            }

            wafSum.DeleteLotData(lotSeq);
            waf.DeleteLotData(lotSeq);
            lotSum.DeleteLot(lotSeq);
            lot.DeleteLot(lotSeq);
        }
    }
}
