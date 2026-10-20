using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace DACrux.SEMDMS.BSL
{
    /// <summary>
    /// E-Test 파일 -> YMS 테이블 반영 (2026-10-14).
    /// 설계: 문서/설계/04_ETest/2026-10-14_ETest_Upload_YMS적재_설계.md 14절 / 16절
    ///  - 저장 확정(CompleteEtFile) / 다시 판정(RejudgeEtFiles) / Map 좌표 다시 계산(RemapEtFiles, 2026-10-15 부터 검사 스크립트 전용) 끝에서 자동으로 부른다.
    ///    업로드 화면을 바꾸지 않아도 올린 파일이 YMS 화면(TEST Map View / Gallery / Yield 등)에 나온다.
    ///  - E-Test 테이블(파일 / 판정·Map 좌표가 들어간 Die / 값 / 등록 Parameter / Spec)을 읽어 EtYmsWafer 로 만들고
    ///    EtYmsPublisher(기존 YMS 적재 함수 호출)에 넘긴다. Parameter 저장 이름이 곧 YMS 컬럼 이름이다(EtYmsRule.ToParamName).
    ///  - Map 이 없는 파일(MAP_ID 없음)은 좌표가 없어 YMS Map 에 그릴 수 없으므로 WAIT 로 두고 이전 반영을 지운다.
    ///    2026-10-15 부터 업로드 화면이 Map 없는 파일을 저장하지 않으므로 그 전에 올린 파일만 해당한다. File Map Setup 에 Map 을 만든 뒤 파일을 다시 업로드하면 반영된다.
    ///  - 결과는 TQP_GDS_SRC_FILE.YMS_* 에 남긴다. YMS 반영이 실패해도 예외를 던지지 않는다(E-Test 저장 / 판정은 그대로 둔다).
    ///  - 파일 삭제(DeleteEtFile) 때는 YMS 반영도 먼저 지운다. 이것이 실패하면 E-Test 파일도 지우지 않는다(YMS 에 남는 데이터 방지).
    /// </summary>
    public partial class GdsEtResult
    {
        /// <summary>YMS 반영 보류 상태 (Map 없음)</summary>
        public const string YmsStatusWait = "WAIT";

        private const int MaxYmsMessageChars = 500;

        /// <summary>
        /// E-Test 파일 1개를 YMS 테이블에 (다시) 반영한다. 화면의 [YMS 다시 반영] / 검증용.
        /// 반환 1행: EtYmsPublishResult.ToDataTable() 열(YMS_STATUS / YMS_STEP / YMS_MESSAGE / YMS_PROGRAM / YMS_TESTAREA / YMS_LOT_SEQ / YMS_WAFER_SEQ /
        /// BIN_COUNTS / DIE_COUNT / SKIP_DIE_COUNT / PARAM_COUNT / GOOD_DIE_COUNT / FAIL_DIE_COUNT / NETDIE / TESTED_DIE / YIELD) + SRC_FILE_SEQ
        /// </summary>
        public DataTable PublishEtFileToYms(string strSrcFileSeq, string strUpdateUser)
        {
            string strSeq = RequiredSeq(strSrcFileSeq, "파일 번호");
            string strUser = Required(strUpdateUser, "Update User", 30);
            DataTable dt = PublishToYms(strSeq, strUser).ToDataTable();
            dt.Columns.Add("SRC_FILE_SEQ", typeof(string));
            dt.Rows[0]["SRC_FILE_SEQ"] = strSeq;
            return dt;
        }

        /// <summary>
        /// YMS 반영 본체. 처리 흐름:
        ///  1) 파일 + 반영 상태 조회. READY 가 아니면 FAIL.
        ///  2) MAP_ID 가 없으면 이전 반영을 지우고 WAIT.
        ///  3) E-Test 데이터로 EtYmsWafer 를 만들어 EtYmsPublisher.Publish (이전 반영 Wafer 는 Publisher 가 먼저 지운다).
        ///     Lot / Wafer 를 만든 직후 키를 바로 기록한다(뒤 단계가 실패해도 다음 반영 때 지울 수 있게).
        ///  4) 상태 / 메시지 기록. 어느 단계에서 실패해도 예외를 던지지 않는다.
        /// </summary>
        private static EtYmsPublishResult PublishToYms(string strSeq, string strUser)
        {
            EtYmsPublishResult result;
            DACrux.SEMDMS.DSL.TQP_GDS_ET_YMS oYms = null;
            try
            {
                oYms = new DACrux.SEMDMS.DSL.TQP_GDS_ET_YMS();
                DataRow drFile;
                using (DataTable dtFile = oYms.GetData("SELECT_ET_YMS_FILE", null, new string[] { strSeq }))
                {
                    if (dtFile.Rows.Count == 0)
                        return YmsFailed("파일 확인", "E-Test 파일을 찾을 수 없습니다: " + strSeq);
                    drFile = dtFile.Rows[0];
                }

                EtYmsPublisher publisher = new EtYmsPublisher();
                string strPrevious = YmsText(drFile["YMS_WAFER_SEQ"]);
                string strStatus = YmsText(drFile["LOAD_STATUS"]);
                if (!string.Equals(strStatus, "READY", StringComparison.Ordinal))
                {
                    result = YmsFailed("파일 확인", "READY 가 아닌 파일은 반영하지 않습니다 (" + strStatus + ")");
                }
                else if (YmsText(drFile["MAP_ID"]).Length == 0)
                {
                    publisher.DeleteWafer(strPrevious);
                    oYms.UpdateDataNonQuery("UPDATE_SRC_FILE_YMS_KEY", null, new string[] { string.Empty, string.Empty, string.Empty, strSeq });
                    result = new EtYmsPublishResult();
                    result.Status = YmsStatusWait;
                    result.Step = "Map 확인";
                    result.Message = "Map 없음 (Map ID = Device " + YmsText(drFile["DEVICE_ID"]) + "). File Map Setup 에 Map 을 만든 뒤 파일을 다시 업로드하면 반영됩니다.";
                }
                else
                {
                    EtYmsWafer wafer = BuildYmsWafer(oYms, strSeq, drFile);
                    DACrux.SEMDMS.DSL.TQP_GDS_ET_YMS oKey = oYms;
                    result = publisher.Publish(wafer, strPrevious, strUser, delegate (EtYmsPublishResult created)
                    {
                        oKey.UpdateDataNonQuery("UPDATE_SRC_FILE_YMS_KEY", null, new string[] { created.Program, created.LotSeq, created.WaferSeq, strSeq });
                    });
                }
            }
            catch (Exception ex)
            {
                result = YmsFailed("E-Test 읽기", YmsInner(ex).Message);
            }

            try
            {
                if (oYms == null)
                    oYms = new DACrux.SEMDMS.DSL.TQP_GDS_ET_YMS();
                oYms.UpdateDataNonQuery("UPDATE_SRC_FILE_YMS", null, new string[] {
                    result.Status, result.Program ?? string.Empty, result.LotSeq ?? string.Empty, result.WaferSeq ?? string.Empty,
                    YmsCut(result.Message ?? string.Empty, MaxYmsMessageChars), strSeq });
            }
            catch (Exception ex)
            {
                result.Message = YmsCut((result.Message ?? string.Empty) + " / 반영 상태 기록 실패: " + YmsInner(ex).Message, MaxYmsMessageChars);
            }
            return result;
        }

        /// <summary>
        /// E-Test 테이블 -> EtYmsWafer.
        /// Parameter = 그 파일에 값이 저장된 등록 Parameter (YMS 컬럼 이름 = 저장 이름, 설명 = 헤더 원문, Spec = 사용 Y 인 것만).
        /// Die = 시스템 Map 좌표(없으면 Map 밖 -> Publisher 가 제외) / 파일 Hardware · Software Bin / 파일 DUT 결과(P / F).
        /// YMS BIN 은 파일 Bin 이며 E-Test Spec 판정(JUDGE_FLAG)은 쓰지 않는다 (2026-10-14). 값이 없는('-' / 빈 칸) Parameter 는 null.
        /// </summary>
        private static EtYmsWafer BuildYmsWafer(DACrux.SEMDMS.DSL.TQP_GDS_ET_YMS oYms, string strSeq, DataRow drFile)
        {
            EtYmsWafer wafer = new EtYmsWafer();
            wafer.Factory = YmsText(drFile["FACTORY"]);
            wafer.DeviceId = YmsText(drFile["DEVICE_ID"]);
            wafer.TestProgram = YmsText(drFile["TEST_PROGRAM"]);
            wafer.MapId = YmsText(drFile["MAP_ID"]);
            wafer.LotId = YmsText(drFile["LOT_ID"]);
            wafer.WaferId = YmsText(drFile["WAFER_ID"]);
            wafer.LotWaferNo = YmsText(drFile["LOT_WAFER_NO"]);
            wafer.FileName = YmsText(drFile["FILE_NAME"]);
            wafer.TestTime = Convert.ToDateTime(drFile["TEST_TIME"], CultureInfo.InvariantCulture);
            if (drFile["WAFER_SIZE"] != DBNull.Value)
            {
                // TQP_MAPDEF.WAFER_SIZE 는 µm (300000). TQP_LOT.WAFER_DIAM 은 mm.
                int iMm = (int)Math.Round(Convert.ToDouble(drFile["WAFER_SIZE"], CultureInfo.InvariantCulture) / 1000d);
                if (iMm > 0)
                    wafer.WaferDiameterMm = iMm;
            }

            Dictionary<long, int> paramIndex = new Dictionary<long, int>();
            using (DataTable dtParam = oYms.GetData("SELECT_ET_YMS_PARAM", null, new string[] { strSeq }))
            {
                foreach (DataRow dr in dtParam.Rows)
                {
                    EtYmsParameter parameter = new EtYmsParameter();
                    parameter.ParameterName = YmsText(dr["ITEM_NAME"]);
                    parameter.YmsName = parameter.ParameterName;
                    parameter.Description = YmsText(dr["HEADER_TEXT"]);
                    parameter.Unit = YmsText(dr["UNIT"]);
                    parameter.Lower = YmsDouble(dr["LOWER_LIMIT"]);
                    parameter.Upper = YmsDouble(dr["UPPER_LIMIT"]);
                    paramIndex[Convert.ToInt64(dr["ET_ITEM_SEQ"], CultureInfo.InvariantCulture)] = wafer.Parameters.Count;
                    wafer.Parameters.Add(parameter);
                }
            }

            Dictionary<long, EtYmsDie> dies = new Dictionary<long, EtYmsDie>();
            using (DataTable dtDie = oYms.GetData("SELECT_ET_YMS_DIE", null, new string[] { strSeq }))
            {
                foreach (DataRow dr in dtDie.Rows)
                {
                    EtYmsDie die = new EtYmsDie();
                    die.DutNo = Convert.ToInt32(dr["DUT_NO"], CultureInfo.InvariantCulture);
                    if (dr["MAP_X"] != DBNull.Value && dr["MAP_Y"] != DBNull.Value)
                    {
                        die.MapX = Convert.ToInt32(dr["MAP_X"], CultureInfo.InvariantCulture);
                        die.MapY = Convert.ToInt32(dr["MAP_Y"], CultureInfo.InvariantCulture);
                    }
                    if (dr["HW_BIN"] != DBNull.Value)
                        die.HardwareBin = Convert.ToInt32(dr["HW_BIN"], CultureInfo.InvariantCulture);
                    if (dr["SW_BIN"] != DBNull.Value)
                        die.SoftwareBin = Convert.ToInt32(dr["SW_BIN"], CultureInfo.InvariantCulture);
                    die.FilePass = YmsText(dr["PASS_FLAG"]);
                    die.Values = new double?[wafer.Parameters.Count];
                    dies[die.DutNo] = die;
                }
            }

            using (DataTable dtValue = oYms.GetData("SELECT_ET_YMS_VALUE", null, new string[] { strSeq }))
            {
                foreach (DataRow dr in dtValue.Rows)
                {
                    if (dr["MEAS_VALUE"] == DBNull.Value || !string.Equals(YmsText(dr["VALUE_STATUS"]), "V", StringComparison.Ordinal))
                        continue;
                    EtYmsDie die;
                    int iParam;
                    if (dies.TryGetValue(Convert.ToInt64(dr["DUT_NO"], CultureInfo.InvariantCulture), out die)
                        && paramIndex.TryGetValue(Convert.ToInt64(dr["ET_ITEM_SEQ"], CultureInfo.InvariantCulture), out iParam))
                        die.Values[iParam] = Convert.ToDouble(dr["MEAS_VALUE"], CultureInfo.InvariantCulture);
                }
            }
            wafer.Dies.AddRange(dies.Values.OrderBy(d => d.DutNo));
            return wafer;
        }

        /// <summary>파일을 지우기 전에 YMS 반영을 지운다. 실패하면 예외 -> 호출한 DeleteEtFile 도 멈춘다.</summary>
        private static void DeleteYmsOfFile(string strSeq)
        {
            using (DataTable dt = new DACrux.SEMDMS.DSL.TQP_GDS_ET_YMS().GetData("SELECT_ET_YMS_FILE", null, new string[] { strSeq }))
            {
                if (dt.Rows.Count == 0)
                    return;
                string strWafer = YmsText(dt.Rows[0]["YMS_WAFER_SEQ"]);
                if (strWafer.Length > 0)
                    new EtYmsPublisher().DeleteWafer(strWafer);
            }
        }

        /// <summary>CompleteEtFile 결과 1행에 YMS 반영 결과 열을 붙인다 (화면이 쓰지 않으면 그냥 남는다).</summary>
        private static void AppendYmsResult(DataTable dt, EtYmsPublishResult result)
        {
            dt.Columns.Add("YMS_STATUS", typeof(string));
            dt.Columns.Add("YMS_MESSAGE", typeof(string));
            dt.Columns.Add("YMS_PROGRAM", typeof(string));
            dt.Columns.Add("YMS_WAFER_SEQ", typeof(string));
            dt.Columns.Add("YMS_YIELD", typeof(decimal));
            DataRow row = dt.Rows[0];
            row["YMS_STATUS"] = result.Status;
            row["YMS_MESSAGE"] = result.Message;
            row["YMS_PROGRAM"] = result.Program;
            row["YMS_WAFER_SEQ"] = result.WaferSeq;
            row["YMS_YIELD"] = result.Yield.HasValue ? (object)result.Yield.Value : DBNull.Value;
        }

        private static EtYmsPublishResult YmsFailed(string strStep, string strMessage)
        {
            EtYmsPublishResult result = new EtYmsPublishResult();
            result.Status = EtYmsPublisher.StatusFail;
            result.Step = strStep;
            result.Message = YmsCut(strStep + ": " + strMessage, MaxYmsMessageChars);
            return result;
        }

        private static string YmsText(object value)
        {
            return value == null || value == DBNull.Value ? string.Empty : Convert.ToString(value, CultureInfo.InvariantCulture).Trim();
        }

        private static double? YmsDouble(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;
            return Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }

        private static Exception YmsInner(Exception ex)
        {
            while (ex.InnerException != null)
                ex = ex.InnerException;
            return ex;
        }

        private static string YmsCut(string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars);
        }
    }
}
