using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Globalization;
using DACrux.SEMDMS.DSL;

namespace DACrux.SEMDMS.BSL
{
    /// <summary>
    /// E-Test 결과 파일 저장의 업무 검증과 DSL 호출을 담당한다.
    /// 설계: 문서/설계/04_ETest/2026-10-07_ETest_결과파일_Parsing_DB설계.md 9절 / 10절
    /// 파일 Parsing은 클라이언트가 하고, 서버는 받은 행을 검증해 DefectDefine.CreateDefect와 같은 Bulk Insert(InsertExecuteMultiple)로 저장한다.
    /// 문장마다 자동 Commit이다. 중간에 끊기면 헤더가 LOADING으로 남고, 다음 저장 때 화면이 삭제 여부를 묻는다.
    /// </summary>
    public partial class GdsEtResult : Miracom.Middleware.BaseComponent, DACrux.SEMDMS.Interface.iGdsEtResult
    {
        public GdsEtResult()
        {
        }

        #region [TQP_GDS_SRC_FILE]

        /// <summary>
        /// 같은 Wafer 파일이 있는지 조회한다. 없으면 0행이다.
        /// aKey: 0 Factory / 1 Lot ID / 2 Slot 번호 [/ 3 공정(Test Type)].
        ///  - 4항목: UNIQUE 키(Factory + WAFER_ID + 공정)로 1건 조회. TSVET -> COWET -> FT 가 공정이라 Wafer 와 공정을 조합해 키로 쓴다 (2026-10-12).
        ///  - 3항목: 공정과 관계없이 그 Wafer 의 파일 전체(TSVET / COWET / FT). 정리 / 이전 호출 호환용.
        /// Device / 시험 일시는 키가 아니다 (2026-10-07).
        /// </summary>
        public DataTable GetEtFileByKey(string[] aKey)
        {
            if (aKey == null || (aKey.Length != 3 && aKey.Length != 4))
                throw new ArgumentException("파일 키는 3개(Factory / Lot / Slot) 또는 4개(+ 공정) 항목이어야 합니다.", "aKey");
            string strFactory = Required(aKey[0], "Factory", 30);
            string strWafer = WaferId(Required(aKey[1], "Lot ID", 50), RequiredSlot(aKey[2]));
            DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE oTQP_GDS_SRC_FILE = new DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE();
            if (aKey.Length == 4)
                return oTQP_GDS_SRC_FILE.GetData("SELECT_SRC_FILE_BY_WAFER_TEST", null,
                    new string[] { strFactory, strWafer, Required(aKey[3], "공정(Test Type)", 50) });
            return oTQP_GDS_SRC_FILE.GetData("SELECT_SRC_FILE_BY_WAFER", null, new string[] { strFactory, strWafer });
        }

        /// <summary>WAFER_ID 가상 컬럼과 같은 규칙(Lot + '-' + 2자리 Slot)으로 만든다.</summary>
        private static string WaferId(string strLot, string strSlot)
        {
            return strLot + "-" + int.Parse(strSlot, CultureInfo.InvariantCulture).ToString("00", CultureInfo.InvariantCulture);
        }

        /// <summary>파일 1건을 조회한다. 없으면 0행이다.</summary>
        public DataTable GetEtFile(string strSrcFileSeq)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE oTQP_GDS_SRC_FILE = new DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE();
            return oTQP_GDS_SRC_FILE.GetData("SELECT_SRC_FILE", null, new string[] { RequiredSeq(strSrcFileSeq, "파일 번호") });
        }

        /// <summary>Device의 파일 목록을 최근 시험 순으로 조회한다(2026-10-14: 헤더 / 등록 / 누락 수, Map ID / 좌표 Die 수, 판정 시각 포함).</summary>
        public DataTable GetEtFileList(string strFactory, string strDeviceID)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE oTQP_GDS_SRC_FILE = new DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE();
            return oTQP_GDS_SRC_FILE.GetData("SELECT_SRC_FILE_LIST", null,
                new string[] { Required(strFactory, "Factory", 30), Required(strDeviceID, "Device ID", 50) });
        }

        /// <summary>
        /// E-Test 결과 조회 화면 파일 목록 (2026-10-15). 공정은 대문자로 맞추고, Lot 은 대문자 포함 검색. 빈 값이면 조건을 걸지 않는다(NULL 바인딩).
        /// 쿼리 순서: factory / device_id / p1 / p2 / l1 / l2 (같은 값을 이름을 나눠 두 번 넘긴다).
        /// </summary>
        public DataTable GetEtFileBrowseList(string strFactory, string strDeviceID, string strTestProgram, string strLot)
        {
            string strProgram = OptionalChars(strTestProgram, "공정(Test Type)", 50).ToUpperInvariant();
            string strLotText = OptionalChars(strLot, "Lot", 50).ToUpperInvariant();
            DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE oTQP_GDS_SRC_FILE = new DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE();
            return oTQP_GDS_SRC_FILE.GetData("SELECT_SRC_FILE_BROWSE", null, new string[] {
                Required(strFactory, "Factory", 30), Required(strDeviceID, "Device ID", 50), strProgram, strProgram, strLotText, strLotText });
        }

        /// <summary>
        /// 파일 1건을 실제로 삭제한다(기존 데이터 삭제 후 재등록).
        /// 처리 순서: 측정값 -> Die -> 헤더. FK가 CASCADE이지만 부모 행마다 내부 DELETE가 도는 것을 피하려고 자식부터 직접 지운다(GdsMapImport.DeleteMap과 같은 이유).
        /// 측정 항목(TQP_GDS_ET_ITEM = 등록 Parameter)은 지우지 않는다. 반환: 지운 측정값 수
        /// </summary>
        public int DeleteEtFile(string strSrcFileSeq, string strUpdateUser)
        {
            string[] aKey = new string[] { RequiredSeq(strSrcFileSeq, "파일 번호") };
            Required(strUpdateUser, "Update User", 30);
            // YMS 반영을 먼저 지운다. 실패하면 E-Test 파일도 지우지 않는다 (2026-10-14, GdsEtResult_Yms.cs)
            DeleteYmsOfFile(aKey[0]);
            int iValues = new DACrux.SEMDMS.DSL.TQP_GDS_ET_VALUE().DeleteDataNonQuery("DELETE_ET_VALUE", null, aKey);
            new DACrux.SEMDMS.DSL.TQP_GDS_ET_DIE().DeleteDataNonQuery("DELETE_ET_DIE", null, aKey);
            new DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE().DeleteDataNonQuery("DELETE_SRC_FILE", null, aKey);
            return iValues;
        }

        /// <summary>
        /// 저장을 시작한다 (2026-10-14 Upload 재설계: 등록 Parameter 만 저장, PARSE_RULE_VER 3).
        /// FileInfo 21항목: 0 Factory / 1 Device ID / 2 파일 이름 / 3 Lot ID / 4 Slot 번호 / 5 공정(Test Type) / 6 시험 일시(yyyyMMddHHmmss) /
        ///   7 파일 접두어(쓰지 않음) / 8 시트 이름 / 9 Die 수 / 10 저장할 등록 Parameter 수 / 11 User /
        ///   12 Customer / 13 File Device / 14 Wafer Type / 15 Wafer Rev / 16 Lot-Wafer No / 17 Fab(받지만 저장하지 않음) /
        ///   18 파일 전체 헤더 수 / 19 화면이 본 등록 Parameter(사용 Y) 수 / 20 등록됐는데 파일에 없는 수
        /// 처리 흐름:
        ///  1) 입력 검증. 12 / 18항목(헤더 전체를 저장하던 이전 방식)은 거부한다.
        ///  2) Device + 공정의 등록 Parameter(사용 Y)를 센다. 0건이면 거부(Setup 우선, 재설계 S1 / 10.1). 화면이 본 수와 다르면 다시 분석하라고 거부한다.
        ///  3) 같은 Wafer + 공정 파일이 있으면 거부(화면이 먼저 묻고 DeleteEtFile 호출).
        ///  4) 번호 채번 -> 헤더 생성(LOADING, FAB_NAME / VIEW_SIDE 없음) -> 헤더 1행 반환
        /// </summary>
        public DataTable BeginEtFile(string[] FileInfo)
        {
            if (FileInfo == null || FileInfo.Length != 21)
                throw new ArgumentException("저장 시작 정보는 21개 항목이어야 합니다(2026-10-14 등록 Parameter 저장 방식). 업로드 화면을 새 버전으로 바꾸세요.", "FileInfo");
            string strFactory = Required(FileInfo[0], "Factory", 30);
            string strDevice = Required(FileInfo[1], "Device ID", 50);
            string strFileName = RequiredChars(FileInfo[2], "파일 이름", 260);
            string strLot = Required(FileInfo[3], "Lot ID", 50);
            string strSlot = RequiredSlot(FileInfo[4]);
            string strProgram = RequiredTestProgram(FileInfo[5]);
            string strTestTime = RequiredTestTime(FileInfo[6]);
            string strSheet = OptionalChars(FileInfo[8], "시트 이름", 50);
            long lDieCount = ParseCount(FileInfo[9], "Die 수");
            long lItemCount = ParseCount(FileInfo[10], "저장 Parameter 수");
            string strUser = Required(FileInfo[11], "Create User", 30);
            string[] aRule = new string[] {
                RequiredChars(FileInfo[12], "Customer", 30), RequiredChars(FileInfo[13], "File Device", 50),
                RequiredChars(FileInfo[14], "Wafer Type", 30), RequiredChars(FileInfo[15], "Wafer Rev", 20),
                RequiredChars(FileInfo[16], "Lot-Wafer No", 60) };
            long lHeader = ParseCount(FileInfo[18], "헤더 수");
            long lDefined = ParseCount(FileInfo[19], "등록 Parameter 수");
            long lMissing = ParseCount(FileInfo[20], "누락 Parameter 수");

            int iUsed = CountUsedParameters(strFactory, strDevice, strProgram);
            if (iUsed == 0)
                throw new InvalidOperationException("Device " + strDevice + " / 공정 " + strProgram
                    + " 에 등록 Parameter(사용 Y)가 없습니다. E-Test Para 설정에서 먼저 등록하세요.");
            if (lDieCount == 0 || lItemCount == 0)
                throw new ArgumentException("Die 수와 저장 Parameter 수는 1 이상이어야 합니다(파일에 등록 Parameter 가 하나도 없으면 저장할 수 없습니다).", "FileInfo");
            if (lDefined != iUsed || lItemCount > iUsed)
                throw new InvalidOperationException("Device " + strDevice + " / 공정 " + strProgram + " 의 등록 Parameter 가 분석한 뒤 바뀌었습니다(분석 "
                    + lDefined + " / 지금 " + iUsed + "). 파일을 다시 분석하세요.");

            using (DataTable dtExist = GetEtFileByKey(new string[] { strFactory, strLot, strSlot, strProgram }))
            {
                if (dtExist.Rows.Count > 0)
                    throw new InvalidOperationException("같은 Wafer / 같은 공정 파일이 이미 있습니다(WAFER_ID " + dtExist.Rows[0]["WAFER_ID"]
                        + " / 공정 " + dtExist.Rows[0]["TEST_PROGRAM"] + " / Device " + dtExist.Rows[0]["DEVICE_ID"]
                        + " / 파일 번호 " + dtExist.Rows[0]["SRC_FILE_SEQ"] + "). 삭제한 뒤 다시 저장하세요.");
            }

            DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE oTQP_GDS_SRC_FILE = new DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE();
            string strSrcFileSeq;
            using (DataTable dtSeq = oTQP_GDS_SRC_FILE.GetData("SELECT_NEXT_SRC_FILE_SEQ", null, null))
                strSrcFileSeq = dtSeq.Rows[0][0].ToString();
            int iRows = oTQP_GDS_SRC_FILE.InsertDataNonQuery("CREATE_SRC_FILE_V3", null, new string[] {
                strSrcFileSeq, strFileName, strFactory, strDevice, strLot, strSlot, strProgram, strTestTime, strSheet,
                lDieCount.ToString(CultureInfo.InvariantCulture), lItemCount.ToString(CultureInfo.InvariantCulture),
                aRule[0], aRule[1], aRule[2], aRule[3], aRule[4],
                lHeader.ToString(CultureInfo.InvariantCulture), iUsed.ToString(CultureInfo.InvariantCulture), lMissing.ToString(CultureInfo.InvariantCulture),
                strUser, strUser });
            if (iRows != 1)
                throw new DataException("E-Test 파일 헤더가 한 건 생성되지 않았습니다.");
            return GetEtFile(strSrcFileSeq);
        }

        /// <summary>Device + 공정의 등록 Parameter 중 사용(Y) 수</summary>
        private static int CountUsedParameters(string strFactory, string strDevice, string strProgram)
        {
            using (DataTable dt = new DACrux.SEMDMS.DSL.TQP_GDS_ET_ITEM().GetData("SELECT_ET_PARA_BY_PROGRAM", null, new string[] { strFactory, strDevice, strProgram }))
                return dt.Rows.Cast<DataRow>().Count(r => r["USE_FLAG"].ToString() == "Y");
        }

        /// <summary>
        /// 저장 완료 확정 (2026-10-14: 판정 + Map 좌표 추가).
        /// 처리 흐름:
        ///  1) 헤더 조회. 이미 READY 면 그 요약으로 OK 를 돌려준다.
        ///  2) DB 에서 Die 수 / 측정값 수를 다시 센다. 배치 저장은 Bulk Insert 만 하므로 이 DB 건수가 유일한 확인이다.
        ///     Die 수 = 헤더 Die 수, 측정값 수 = Die 수 x 저장 Parameter 수가 아니면 LOADING 그대로 두고 MISMATCH 반환.
        ///  3) Spec 판정(값 -> Die -> 파일, Upload 재설계 9.2절) -> Map 좌표(9.5절, Map ID = Device) -> READY.
        /// 반환 1행: RESULT / LOAD_STATUS / EXPECTED_DIE / DB_DIE / FAIL_DIE(Spec 판정) / EXPECTED_VALUE / DB_VALUE / NOT_TESTED / EMPTY / MESSAGE /
        ///          SPEC_FAIL_DIE / MAP_ID(없으면 빈 값) / MAP_DIE(좌표를 찾은 Die) / OUT_OF_MAP(Map 사용 Die 밖)
        /// </summary>
        public DataTable CompleteEtFile(string strSrcFileSeq, string strUpdateUser)
        {
            string strSeq = RequiredSeq(strSrcFileSeq, "파일 번호");
            string strUser = Required(strUpdateUser, "Update User", 30);
            DataRow drFile;
            using (DataTable dtFile = GetEtFile(strSeq))
            {
                if (dtFile.Rows.Count == 0)
                    throw new InvalidOperationException("E-Test 파일을 찾을 수 없습니다: " + strSeq);
                drFile = dtFile.Rows[0];
            }
            string strStatus = drFile["LOAD_STATUS"].ToString();
            long lExpectedDie = DACrux.Base.Convert.longParse(drFile["DIE_COUNT"].ToString());
            long lExpectedValue = lExpectedDie * DACrux.Base.Convert.longParse(drFile["ITEM_COUNT"].ToString());
            DieSummary sum = ReadDieSummary(strSeq);

            if (string.Equals(strStatus, "READY", StringComparison.Ordinal))
                return CompleteResult("OK", "READY", lExpectedDie, lExpectedValue, sum, drFile["MAP_ID"].ToString(), "이미 확정된 파일입니다.");

            StringBuilder sbMessage = new StringBuilder();
            if (sum.Die != lExpectedDie) sbMessage.Append("Die 건수 불일치 / ");
            if (sum.Value != lExpectedValue) sbMessage.Append("측정값 건수 불일치 / ");
            if (sbMessage.Length > 0)
                return CompleteResult("MISMATCH", strStatus, lExpectedDie, lExpectedValue, sum, string.Empty, sbMessage.ToString().TrimEnd(' ', '/'));

            JudgeFile(strSeq, strUser);
            string strMapId = ApplyFileMap(strSeq, drFile["FACTORY"].ToString(), drFile["DEVICE_ID"].ToString(), strUser);
            int iRows = new DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE().UpdateDataNonQuery("UPDATE_SRC_FILE_READY", null, new string[] {
                sum.Value.ToString(CultureInfo.InvariantCulture), strUser, strSeq });
            if (iRows != 1)
                throw new InvalidOperationException("파일 상태가 바뀌어 확정하지 못했습니다. 다시 조회하세요.");
            DataTable dtDone = CompleteResult("OK", "READY", lExpectedDie, lExpectedValue, ReadDieSummary(strSeq), strMapId, "저장이 확정되었습니다.");
            // YMS 테이블 반영 (2026-10-14, GdsEtResult_Yms.cs). 실패해도 확정은 그대로 두고 결과 열(YMS_STATUS 등)로만 알린다.
            AppendYmsResult(dtDone, PublishToYms(strSeq, strUser));
            return dtDone;
        }

        /// <summary>
        /// Device + 공정의 READY 파일을 지금 Spec 으로 다시 판정한다(Spec 화면 저장 뒤). aKey: 0 Factory / 1 Device / 2 공정.
        /// Spec 에 새로 넣은 Parameter 는 저장된 값이 있을 때만 판정된다(값이 없으면 파일을 다시 올려야 한다). 반환: 다시 판정한 파일 수
        /// </summary>
        public int RejudgeEtFiles(string[] aKey, string strUpdateUser)
        {
            if (aKey == null || aKey.Length != 3)
                throw new ArgumentException("다시 판정 키는 3개 항목(Factory / Device / 공정)이어야 합니다.", "aKey");
            string f = Required(aKey[0], "Factory", 30);
            string d = Required(aKey[1], "Device ID", 50);
            string t = RequiredTestProgram(aKey[2]);
            string strUser = Required(strUpdateUser, "Update User", 30);
            int iFiles = 0;
            foreach (DataRow dr in ReadyFiles(f, d))
            {
                if (!string.Equals(dr["TEST_PROGRAM"].ToString(), t, StringComparison.Ordinal))
                    continue;
                JudgeFile(dr["SRC_FILE_SEQ"].ToString(), strUser);
                PublishToYms(dr["SRC_FILE_SEQ"].ToString(), strUser);   // 판정이 바뀌면 YMS BIN / 수율도 다시 반영 (2026-10-14)
                iFiles++;
            }
            return iFiles;
        }

        /// <summary>
        /// Device 의 READY 파일 Map 좌표를 다시 계산한다(File Map Setup 에 Map 을 나중에 만들었을 때). aKey: 0 Factory / 1 Device.
        /// 반환 1행: FILE_COUNT / MAP_ID(없으면 빈 값) / MAP_DIE(좌표를 찾은 Die 합) / OUT_OF_MAP(Map 사용 Die 밖 Die 합)
        /// 2026-10-15: 화면 기능([Map 좌표 다시 계산])과 원격 호출(iGdsEtResult)은 삭제했다(업로드 때 Map 필수).
        ///   서버 검사 스크립트(GdsEtResultProbe / EtYmsServiceProbe)가 Map 좌표 계산 · YMS 다시 반영 확인에만 BSL 을 직접 불러 쓴다.
        /// </summary>
        public DataTable RemapEtFiles(string[] aKey, string strUpdateUser)
        {
            if (aKey == null || aKey.Length != 2)
                throw new ArgumentException("좌표 다시 계산 키는 2개 항목(Factory / Device)이어야 합니다.", "aKey");
            string f = Required(aKey[0], "Factory", 30);
            string d = Required(aKey[1], "Device ID", 50);
            string strUser = Required(strUpdateUser, "Update User", 30);
            int iFiles = 0, iYmsFail = 0;
            long lMapDie = 0, lOut = 0;
            string strMapId = string.Empty;
            foreach (DataRow dr in ReadyFiles(f, d))
            {
                string strSeq = dr["SRC_FILE_SEQ"].ToString();
                strMapId = ApplyFileMap(strSeq, f, d, strUser);
                DieSummary sum = ReadDieSummary(strSeq);
                lMapDie += sum.MapDie;
                if (strMapId.Length > 0)
                    lOut += sum.Die - sum.MapDie;
                // 좌표가 바뀌면 YMS 도 다시 반영 (Map 이 새로 생겨 보류가 풀린 파일 포함, 2026-10-14)
                if (string.Equals(PublishToYms(strSeq, strUser).Status, EtYmsPublisher.StatusFail, StringComparison.Ordinal))
                    iYmsFail++;
                iFiles++;
            }
            DataTable dt = new DataTable("ET_REMAP_RESULT");
            dt.Columns.Add("FILE_COUNT", typeof(int));
            dt.Columns.Add("MAP_ID", typeof(string));
            dt.Columns.Add("MAP_DIE", typeof(long));
            dt.Columns.Add("OUT_OF_MAP", typeof(long));
            dt.Columns.Add("YMS_FAIL", typeof(int));
            dt.Rows.Add(iFiles, strMapId, lMapDie, lOut, iYmsFail);
            return dt;
        }

        /// <summary>
        /// Map 사용 Die 목록 (File Map Setup 의 TQP_USEMAP, 가장 큰 PROGRAM_REV). 열: X / Y / USECODE(1 = 사용 Die). 없으면 0행.
        /// 업로드 화면이 저장 전에 Map 밖 Die 를 미리 보는 데 쓴다(Map ID = E-Test 파일 Device).
        /// </summary>
        public DataTable GetEtMapDieList(string strFactory, string strMapId)
        {
            string f = Required(strFactory, "Factory", 30);
            string m = Required(strMapId, "Map ID", 50);
            return new DACrux.SEMDMS.DSL.TQP_GDS_ET_DIE().GetData("SELECT_ET_MAP_DIE", null, new string[] { f, m, f, m });
        }

        /// <summary>Spec 판정: 값 -> Die -> 파일 요약 (Upload 재설계 9.2절, 업로드 확정 / 다시 판정 공용). 문장마다 자동 Commit.</summary>
        private static void JudgeFile(string strSeq, string strUser)
        {
            string[] aKey = new string[] { strSeq };
            new DACrux.SEMDMS.DSL.TQP_GDS_ET_VALUE().UpdateDataNonQuery("UPDATE_ET_VALUE_JUDGE", null, aKey);
            new DACrux.SEMDMS.DSL.TQP_GDS_ET_DIE().UpdateDataNonQuery("UPDATE_ET_DIE_JUDGE", null, aKey);
            new DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE().UpdateDataNonQuery("UPDATE_SRC_FILE_JUDGE", null, new string[] { strUser, strSeq });
        }

        /// <summary>
        /// Die 시스템 Map 좌표 (Upload 재설계 9.5절): Map ID = Device 의 사용 Die 로 DieCoordinateMapper(ETest 종류, KLARF 원점 안 씀)를 만들어
        /// 파일 (X, Y) -> 시스템 좌표. Map 사용 Die 가 아니면 NULL. Map 이 없으면 모든 Die 좌표를 비운다. 반환: 쓴 Map ID(없으면 빈 문자열)
        /// </summary>
        private string ApplyFileMap(string strSeq, string strFactory, string strDevice, string strUser)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_ET_DIE oDie = new DACrux.SEMDMS.DSL.TQP_GDS_ET_DIE();
            DACrux.SEMDMS.Interface.DieCoordinateMapper mapper = null;
            using (DataTable dtMap = GetEtMapDieList(strFactory, strDevice))
            {
                if (dtMap.Rows.Cast<DataRow>().Any(r => r["USECODE"] != DBNull.Value
                    && Convert.ToInt32(r["USECODE"], CultureInfo.InvariantCulture) == DACrux.SEMDMS.Interface.DieCoordinateMapper.UsedDieCode))
                    mapper = DACrux.SEMDMS.Interface.DieCoordinateMapper.FromDieTable(dtMap, new DACrux.SEMDMS.Interface.DiePoint(0, 0));
            }
            DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE oFile = new DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE();
            if (mapper == null)
            {
                oDie.UpdateDataNonQuery("UPDATE_ET_DIE_MAP_CLEAR", null, new string[] { strSeq });
                oFile.UpdateDataNonQuery("UPDATE_SRC_FILE_MAP", null, new string[] { string.Empty, strUser, strSeq });
                return string.Empty;
            }
            using (DataTable dtDie = GetEtDieList(strSeq))
            {
                if (dtDie.Rows.Count > 0)
                {
                    string[,] aRows = new string[dtDie.Rows.Count, 4];
                    for (int i = 0; i < dtDie.Rows.Count; i++)
                    {
                        DataRow dr = dtDie.Rows[i];
                        DACrux.SEMDMS.Interface.DiePoint sys;
                        bool bIn = mapper.TryToSystem(DACrux.SEMDMS.Interface.DieCoordType.ETest,
                            new DACrux.SEMDMS.Interface.DiePoint(Convert.ToInt32(dr["DIE_X"], CultureInfo.InvariantCulture), Convert.ToInt32(dr["DIE_Y"], CultureInfo.InvariantCulture)), out sys);
                        aRows[i, 0] = bIn ? sys.X.ToString(CultureInfo.InvariantCulture) : string.Empty;
                        aRows[i, 1] = bIn ? sys.Y.ToString(CultureInfo.InvariantCulture) : string.Empty;
                        aRows[i, 2] = strSeq;
                        aRows[i, 3] = dr["DUT_NO"].ToString();
                    }
                    oDie.UpdateExecuteMultiple("UPDATE_ET_DIE_MAP", aRows);
                }
            }
            oFile.UpdateDataNonQuery("UPDATE_SRC_FILE_MAP", null, new string[] { strDevice, strUser, strSeq });
            return strDevice;
        }

        /// <summary>Device 의 READY 파일 (파일 목록 조회에서 거른다)</summary>
        private List<DataRow> ReadyFiles(string strFactory, string strDevice)
        {
            DataTable dt = GetEtFileList(strFactory, strDevice);
            return dt.Rows.Cast<DataRow>().Where(r => r["LOAD_STATUS"].ToString() == "READY").ToList();
        }

        /// <summary>파일의 Die / 값 건수와 판정 / 좌표 요약</summary>
        private sealed class DieSummary
        {
            public long Die;
            public long SpecFailDie;
            public long MapDie;
            public long Value;
            public long NotTested;
            public long Empty;
        }

        private static DieSummary ReadDieSummary(string strSeq)
        {
            string[] aKey = new string[] { strSeq };
            DieSummary sum = new DieSummary();
            using (DataTable dtDie = new DACrux.SEMDMS.DSL.TQP_GDS_ET_DIE().GetData("SELECT_ET_DIE_COUNT", null, aKey))
            {
                sum.Die = DACrux.Base.Convert.longParse(dtDie.Rows[0]["DIE_COUNT"].ToString());
                sum.SpecFailDie = DACrux.Base.Convert.longParse(dtDie.Rows[0]["SPEC_FAIL_DIE_COUNT"].ToString());
                sum.MapDie = DACrux.Base.Convert.longParse(dtDie.Rows[0]["MAP_DIE_COUNT"].ToString());
            }
            using (DataTable dtValue = new DACrux.SEMDMS.DSL.TQP_GDS_ET_VALUE().GetData("SELECT_ET_VALUE_COUNT", null, aKey))
            {
                sum.Value = DACrux.Base.Convert.longParse(dtValue.Rows[0]["VALUE_COUNT"].ToString());
                sum.NotTested = DACrux.Base.Convert.longParse(dtValue.Rows[0]["NOT_TESTED_COUNT"].ToString());
                sum.Empty = DACrux.Base.Convert.longParse(dtValue.Rows[0]["EMPTY_COUNT"].ToString());
            }
            return sum;
        }

        /// <summary>CompleteEtFile 반환 1행을 만든다.</summary>
        private static DataTable CompleteResult(string strResult, string strStatus, long lExpectedDie, long lExpectedValue, DieSummary sum, string strMapId, string strMessage)
        {
            DataTable dt = new DataTable("COMPLETE_RESULT");
            dt.Columns.Add("RESULT", typeof(string));
            dt.Columns.Add("LOAD_STATUS", typeof(string));
            dt.Columns.Add("EXPECTED_DIE", typeof(long));
            dt.Columns.Add("DB_DIE", typeof(long));
            dt.Columns.Add("FAIL_DIE", typeof(long));
            dt.Columns.Add("EXPECTED_VALUE", typeof(long));
            dt.Columns.Add("DB_VALUE", typeof(long));
            dt.Columns.Add("NOT_TESTED", typeof(long));
            dt.Columns.Add("EMPTY", typeof(long));
            dt.Columns.Add("MESSAGE", typeof(string));
            dt.Columns.Add("SPEC_FAIL_DIE", typeof(long));
            dt.Columns.Add("MAP_ID", typeof(string));
            dt.Columns.Add("MAP_DIE", typeof(long));
            dt.Columns.Add("OUT_OF_MAP", typeof(long));
            long lOut = strMapId.Length > 0 ? sum.Die - sum.MapDie : 0;
            dt.Rows.Add(strResult, strStatus, lExpectedDie, sum.Die, sum.SpecFailDie, lExpectedValue, sum.Value, sum.NotTested, sum.Empty, strMessage,
                sum.SpecFailDie, strMapId, sum.MapDie, lOut);
            return dt;
        }

        /// <summary>파일이 있고 LOADING 상태인지 확인한 뒤 정리한 파일 번호를 돌려준다.</summary>
        private string GetLoadingFileSeq(string strSrcFileSeq)
        {
            string strSeq = RequiredSeq(strSrcFileSeq, "파일 번호");
            DataTable dt = null;
            try
            {
                dt = GetEtFile(strSeq);
                if (dt.Rows.Count == 0)
                    throw new InvalidOperationException("E-Test 파일을 찾을 수 없습니다: " + strSeq);
                string strStatus = dt.Rows[0]["LOAD_STATUS"].ToString();
                if (!string.Equals(strStatus, "LOADING", StringComparison.Ordinal))
                    throw new InvalidOperationException("E-Test 파일이 " + strStatus + " 상태라서 배치를 받을 수 없습니다.");
                return strSeq;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
        }

        #endregion

        #region [TQP_GDS_ET_ITEM]

        /// <summary>
        /// Device 의 측정 항목 목록을 조회한다. 2026-10-14 부터 측정 항목 = 등록 Parameter(모든 공정)다.
        /// 업로드는 측정 항목을 만들지 않는다(예전 MergeEtItem 삭제). 등록은 E-Test Para 설정(RegisterEtParameters)에서만 한다.
        /// </summary>
        public DataTable GetEtItemList(string strFactory, string strDeviceID)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_ET_ITEM oTQP_GDS_ET_ITEM = new DACrux.SEMDMS.DSL.TQP_GDS_ET_ITEM();
            return oTQP_GDS_ET_ITEM.GetData("SELECT_ET_ITEM", null,
                new string[] { Required(strFactory, "Factory", 30), Required(strDeviceID, "Device ID", 50) });
        }

        #endregion

        #region [TQP_GDS_ET_DIE]

        /// <summary>
        /// Die 정보 배치를 저장한다. 처리 흐름: 파일 LOADING 확인 -> 열 수 / 필수 값 확인 -> InsertExecuteMultiple 1회.
        /// 같은 DUT 번호 / 같은 (X, Y)가 섞이면 DB 제약으로 배치 전체가 거부된다. PASS_FLAG 는 파일 DUT Flag(참고)이고 판정은 CompleteEtFile 이 한다.
        /// </summary>
        public int CreateEtDieBatch(string strSrcFileSeq, string[,] aParas, string strUpdateUser)
        {
            string strSeq = GetLoadingFileSeq(strSrcFileSeq);
            Required(strUpdateUser, "Update User", 30);
            CheckBatchParas(aParas, 10);
            int iRows = aParas.GetLength(0);

            string[,] aDie = new string[iRows, 11];
            for (int i = 0; i < iRows; i++)
            {
                string strPass = Clean(aParas[i, 5]);
                if (strPass != "P" && strPass != "F")
                    throw new ArgumentException("PASS_FLAG는 P 또는 F여야 합니다: " + aParas[i, 5], "aParas");
                aDie[i, 0] = strSeq;
                aDie[i, 1] = ParseInt(aParas[i, 0], "DUT_NO").ToString(CultureInfo.InvariantCulture);
                aDie[i, 2] = ParseInt(aParas[i, 1], "DIE_X").ToString(CultureInfo.InvariantCulture);
                aDie[i, 3] = ParseInt(aParas[i, 2], "DIE_Y").ToString(CultureInfo.InvariantCulture);
                aDie[i, 4] = OptionalNumber(aParas[i, 3], "HW_BIN");
                aDie[i, 5] = OptionalNumber(aParas[i, 4], "SW_BIN");
                aDie[i, 6] = strPass;
                aDie[i, 7] = OptionalChars(aParas[i, 6], "FLAG_CODE", 10);
                aDie[i, 8] = OptionalNumber(aParas[i, 7], "TEST_TIME_MS");
                aDie[i, 9] = OptionalNumber(aParas[i, 8], "TESTS_EXECUTED");
                aDie[i, 10] = OptionalChars(aParas[i, 9], "HEAD_SITE", 30);
            }
            DACrux.SEMDMS.DSL.TQP_GDS_ET_DIE oTQP_GDS_ET_DIE = new DACrux.SEMDMS.DSL.TQP_GDS_ET_DIE();
            oTQP_GDS_ET_DIE.InsertExecuteMultiple("CREATE_ET_DIE", aDie);
            return iRows;
        }

        /// <summary>파일의 Die 목록을 조회한다. 2026-10-14: MAP_X / MAP_Y / JUDGE_FLAG / FAIL_COUNT / NOT_TESTED_COUNT 포함 (Map 표시용 Die 데이터).</summary>
        public DataTable GetEtDieList(string strSrcFileSeq)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_ET_DIE oTQP_GDS_ET_DIE = new DACrux.SEMDMS.DSL.TQP_GDS_ET_DIE();
            return oTQP_GDS_ET_DIE.GetData("SELECT_ET_DIE", null, new string[] { RequiredSeq(strSrcFileSeq, "파일 번호") });
        }

        #endregion

        #region [TQP_GDS_ET_VALUE]

        /// <summary>
        /// 측정값 배치를 저장한다. 처리 흐름: 파일 LOADING 확인 -> 열 수 / 상태 값 / 항목 번호 확인 -> InsertExecuteMultiple 1회.
        /// 값은 클라이언트가 G17 문자열로 보내고 XML이 TO_BINARY_DOUBLE로 바꾼다. 미측정(N) / 빈 값(E)은 값을 빈 문자열로 보낸다(NULL 저장).
        /// 2026-10-14: 항목 번호는 파일 Device + 공정의 등록 Parameter(사용 Y)만 받는다(아니면 배치 거부). 건수는 CompleteEtFile이 DB에서 확인한다.
        /// </summary>
        public int CreateEtValueBatch(string strSrcFileSeq, string[,] aParas, string strUpdateUser)
        {
            string strSeq = GetLoadingFileSeq(strSrcFileSeq);
            Required(strUpdateUser, "Update User", 30);
            CheckBatchParas(aParas, 4);
            int iRows = aParas.GetLength(0);

            HashSet<string> setItem = new HashSet<string>(StringComparer.Ordinal);
            using (DataTable dtItem = new DACrux.SEMDMS.DSL.TQP_GDS_ET_VALUE().GetData("SELECT_ET_UPLOAD_ITEM_SEQ", null, new string[] { strSeq }))
            {
                foreach (DataRow dr in dtItem.Rows)
                    setItem.Add(Convert.ToInt64(dr["ET_ITEM_SEQ"], CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture));
            }

            string[,] aValue = new string[iRows, 5];
            for (int i = 0; i < iRows; i++)
            {
                string strStatus = Clean(aParas[i, 3]);
                string strValue = Clean(aParas[i, 2]);
                if (strStatus == "V")
                {
                    double dValue;
                    if (!double.TryParse(strValue, NumberStyles.Float, CultureInfo.InvariantCulture, out dValue) || double.IsNaN(dValue) || double.IsInfinity(dValue))
                        throw new ArgumentException("측정값이 숫자가 아닙니다(DUT " + aParas[i, 0] + " / 항목 " + aParas[i, 1] + "): " + strValue, "aParas");
                }
                else if (strStatus == "N" || strStatus == "E")
                    strValue = string.Empty;
                else
                    throw new ArgumentException("VALUE_STATUS는 V / N / E여야 합니다: " + aParas[i, 3], "aParas");
                string strItem = Clean(aParas[i, 1]);
                if (!setItem.Contains(strItem))
                    throw new ArgumentException("파일 Device + 공정의 등록 Parameter(사용 Y)가 아닌 측정 항목 번호입니다: " + aParas[i, 1], "aParas");
                aValue[i, 0] = strSeq;
                aValue[i, 1] = Clean(aParas[i, 0]);
                aValue[i, 2] = strItem;
                aValue[i, 3] = strValue;
                aValue[i, 4] = strStatus;
            }
            DACrux.SEMDMS.DSL.TQP_GDS_ET_VALUE oTQP_GDS_ET_VALUE = new DACrux.SEMDMS.DSL.TQP_GDS_ET_VALUE();
            oTQP_GDS_ET_VALUE.InsertExecuteMultiple("CREATE_ET_VALUE", aValue);
            return iRows;
        }

        /// <summary>파일의 측정값 전체를 조회한다. 2026-10-14: JUDGE_FLAG + 등록 Parameter 정보(헤더 원문 / Para / 조건 / Chain / 단위 / 순서) 포함.</summary>
        public DataTable GetEtValueList(string strSrcFileSeq)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_ET_VALUE oTQP_GDS_ET_VALUE = new DACrux.SEMDMS.DSL.TQP_GDS_ET_VALUE();
            return oTQP_GDS_ET_VALUE.GetData("SELECT_ET_VALUE", null, new string[] { RequiredSeq(strSrcFileSeq, "파일 번호") });
        }

        #endregion

        #region [TQP_GDS_ET_LIMIT]

        /// <summary>측정 항목이 있는 Device 목록을 조회한다.</summary>
        public DataTable GetEtDeviceList(string strFactory)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_ET_ITEM oTQP_GDS_ET_ITEM = new DACrux.SEMDMS.DSL.TQP_GDS_ET_ITEM();
            return oTQP_GDS_ET_ITEM.GetData("SELECT_ET_ITEM_DEVICE_LIST", null, new string[] { Required(strFactory, "Factory", 30) });
        }

        /// <summary>Device 의 공정 목록(업로드된 파일 + Spec + 등록 Parameter 가 있는 공정)을 조회한다.</summary>
        public DataTable GetEtTestProgramList(string strFactory, string strDeviceID)
        {
            string strCleanFactory = Required(strFactory, "Factory", 30);
            string strCleanDevice = Required(strDeviceID, "Device ID", 50);
            DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT oTQP_GDS_ET_LIMIT = new DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT();
            return oTQP_GDS_ET_LIMIT.GetData("SELECT_ET_TEST_PROGRAM_LIST", null, new string[] {
                strCleanFactory, strCleanDevice, strCleanFactory, strCleanDevice, strCleanFactory, strCleanDevice });
        }

        /// <summary>Device + 공정의 저장된 Spec 을 조회한다(판정용).</summary>
        public DataTable GetEtLimit(string strFactory, string strDeviceID, string strTestProgram)
        {
            DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT oTQP_GDS_ET_LIMIT = new DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT();
            return oTQP_GDS_ET_LIMIT.GetData("SELECT_ET_LIMIT", null, new string[] {
                Required(strFactory, "Factory", 30), Required(strDeviceID, "Device ID", 50), RequiredTestProgram(strTestProgram) });
        }

        /// <summary>
        /// Spec 을 저장한다(있으면 수정 / 없으면 추가).
        /// 기준 (2026-10-14): 키 = Device + 공정 + 등록 Parameter 저장 이름. Spec 은 등록 Parameter 에만 둔다(DB FK, Para Setup 설계 4.5절).
        /// 처리 흐름:
        ///  1) 입력 검증: Parameter 는 저장 이름 또는 헤더 원문('@' 포함 -> EtParameterNameRule 로 저장 이름). 같은 Parameter 가 두 번 오면 거부.
        ///     Lower / Upper 는 숫자 또는 빈 값, 하나는 있어야 하고 둘 다 있으면 Lower &lt;= Upper. Enable 은 Y / N.
        ///  2) 등록되지 않은 Parameter 가 하나라도 있으면 모아서 거부한다(Para 설정 화면에서 등록하거나 Spec 표 붙여넣기가 먼저 등록한다).
        ///     단위가 비어 있으면 등록 Parameter 단위로 채운다.
        ///  3) MERGE 로 한 번에 저장한다(InsertExecuteMultiple).
        /// </summary>
        public int SaveEtLimit(string[] aKey, string[,] aRows, string strUpdateUser)
        {
            if (aKey == null || aKey.Length != 3)
                throw new ArgumentException("Spec 키는 3개 항목(Factory / Device / 공정)이어야 합니다.", "aKey");
            string strFactory = Required(aKey[0], "Factory", 30);
            string strDevice = Required(aKey[1], "Device ID", 50);
            string strProgram = RequiredTestProgram(aKey[2]);
            string strUser = Required(strUpdateUser, "Update User", 30);
            if (aRows == null || aRows.GetLength(0) == 0)
                throw new ArgumentException("저장할 Spec 이 없습니다.", "aRows");
            if (aRows.GetLength(0) > MaxBatchRows)
                throw new ArgumentException("Spec 은 한 번에 " + MaxBatchRows + "건 이하로 저장하세요.", "aRows");
            if (aRows.GetLength(1) != 7)
                throw new ArgumentException("Spec 열 수는 7개여야 합니다.", "aRows");

            Dictionary<string, string> dicUnit = new Dictionary<string, string>(StringComparer.Ordinal);
            using (DataTable dtItem = new DACrux.SEMDMS.DSL.TQP_GDS_ET_ITEM().GetData("SELECT_ET_PARA_BY_PROGRAM", null, new string[] { strFactory, strDevice, strProgram }))
            {
                foreach (DataRow dr in dtItem.Rows)
                    dicUnit[dr["ITEM_NAME"].ToString()] = dr["UNIT"].ToString();
            }

            int iRows = aRows.GetLength(0);
            string[,] aMerge = new string[iRows, 11];
            HashSet<string> setItem = new HashSet<string>(StringComparer.Ordinal);
            List<string> lstMissing = new List<string>();
            for (int i = 0; i < iRows; i++)
            {
                string strItem = RequiredSpecParameterName(aRows[i, 0]);
                if (!setItem.Add(strItem))
                    throw new ArgumentException("같은 Parameter 가 두 번 있습니다: " + strItem, "aRows");
                string strItemUnit;
                if (!dicUnit.TryGetValue(strItem, out strItemUnit))
                {
                    lstMissing.Add(strItem);
                    continue;
                }
                string strLower = OptionalDouble(aRows[i, 1], strItem + " Lower");
                string strUpper = OptionalDouble(aRows[i, 2], strItem + " Upper");
                if (strLower.Length == 0 && strUpper.Length == 0)
                    throw new ArgumentException(strItem + " 의 Lower / Upper 중 하나는 입력해야 합니다.", "aRows");
                if (strLower.Length > 0 && strUpper.Length > 0
                    && double.Parse(strLower, CultureInfo.InvariantCulture) > double.Parse(strUpper, CultureInfo.InvariantCulture))
                    throw new ArgumentException(strItem + " 의 Lower(" + strLower + ") 가 Upper(" + strUpper + ") 보다 큽니다.", "aRows");
                string strEnable = Clean(aRows[i, 3]).ToUpperInvariant();
                if (strEnable != "Y" && strEnable != "N")
                    throw new ArgumentException("Enable 은 Y 또는 N 이어야 합니다: " + aRows[i, 3], "aRows");
                string strUnit = OptionalChars(aRows[i, 4], strItem + " 단위", 10);
                if (strUnit.Length == 0)
                    strUnit = strItemUnit;
                aMerge[i, 0] = strFactory;
                aMerge[i, 1] = strDevice;
                aMerge[i, 2] = strProgram;
                aMerge[i, 3] = strItem;
                aMerge[i, 4] = strLower;
                aMerge[i, 5] = strUpper;
                aMerge[i, 6] = strUnit;
                aMerge[i, 7] = strEnable;
                aMerge[i, 8] = OptionalChars(aRows[i, 5], "Spec 원문", 200);
                aMerge[i, 9] = OptionalChars(aRows[i, 6], "비고", 200);
                aMerge[i, 10] = strUser;
            }
            if (lstMissing.Count > 0)
                throw new ArgumentException("등록되지 않은 Parameter 에는 Spec 을 저장할 수 없습니다(" + lstMissing.Count + "건, E-Test Para 설정에서 등록하세요): "
                    + string.Join(", ", lstMissing.Take(5).ToArray()) + (lstMissing.Count > 5 ? " ..." : string.Empty), "aRows");
            DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT oTQP_GDS_ET_LIMIT = new DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT();
            oTQP_GDS_ET_LIMIT.InsertExecuteMultiple("MERGE_ET_LIMIT", aMerge);
            return iRows;
        }

        /// <summary>Spec 1행을 삭제한다. aKey: 0 Factory / 1 Device / 2 공정 / 3 Parameter(저장 이름 또는 헤더 원문, 저장 때와 같은 규칙으로 찾는다).</summary>
        public int DeleteEtLimit(string[] aKey, string strUpdateUser)
        {
            if (aKey == null || aKey.Length != 4)
                throw new ArgumentException("Spec 삭제 키는 4개 항목(Factory / Device / 공정 / Parameter)이어야 합니다.", "aKey");
            Required(strUpdateUser, "Update User", 30);
            DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT oTQP_GDS_ET_LIMIT = new DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT();
            return oTQP_GDS_ET_LIMIT.DeleteDataNonQuery("DELETE_ET_LIMIT", null, new string[] {
                Required(aKey[0], "Factory", 30), Required(aKey[1], "Device ID", 50), RequiredTestProgram(aKey[2]), RequiredSpecParameterName(aKey[3]) });
        }

        /// <summary>빈 값이면 빈 문자열(NULL), 아니면 유한한 숫자인지 확인해 R 형식(왕복 가능) 문자열로 돌려준다.</summary>
        private static string OptionalDouble(string strValue, string strName)
        {
            string strClean = Clean(strValue);
            if (strClean.Length == 0)
                return string.Empty;
            double dValue;
            if (!double.TryParse(strClean, NumberStyles.Float, CultureInfo.InvariantCulture, out dValue) || double.IsNaN(dValue) || double.IsInfinity(dValue))
                throw new ArgumentException(strName + " 값이 숫자가 아닙니다: " + strValue, strName);
            return dValue.ToString("R", CultureInfo.InvariantCulture);
        }

        #endregion

        #region [Batch Common]

        /// <summary>한 번에 받을 수 있는 배치 행 수. Remoting 요청 크기와 DB 배열 바인딩 크기를 제한한다(GdsMapImport와 같음).</summary>
        private const int MaxBatchRows = 10000;

        /// <summary>행 수 / 열 수를 확인한다.</summary>
        private static void CheckBatchParas(string[,] aParas, int iColumns)
        {
            if (aParas == null || aParas.GetLength(0) == 0)
                throw new ArgumentException("배치에 행이 없습니다.", "aParas");
            if (aParas.GetLength(0) > MaxBatchRows)
                throw new ArgumentException("배치 행 수는 " + MaxBatchRows + "건 이하여야 합니다.", "aParas");
            if (aParas.GetLength(1) != iColumns)
                throw new ArgumentException("배치 열 수는 " + iColumns + "개여야 합니다.", "aParas");
        }

        #endregion

        #region [Validation]

        /// <summary>null을 빈 문자열로 바꾸고 앞뒤 공백을 지운다. 빈 문자열은 Oracle에서 NULL로 저장된다.</summary>
        private static string Clean(string strValue)
        {
            return strValue == null ? string.Empty : strValue.Trim();
        }

        /// <summary>정수 문자열을 int로 바꾼다.</summary>
        private static int ParseInt(string strValue, string strName)
        {
            int iValue;
            if (!int.TryParse(Clean(strValue), NumberStyles.Integer, CultureInfo.InvariantCulture, out iValue))
                throw new ArgumentException(strName + " 값이 정수가 아닙니다: " + strValue, strName);
            return iValue;
        }

        /// <summary>빈 값이면 빈 문자열(NULL), 아니면 정수인지 확인한 값을 돌려준다.</summary>
        private static string OptionalNumber(string strValue, string strName)
        {
            string strClean = Clean(strValue);
            if (strClean.Length == 0)
                return string.Empty;
            long lValue;
            if (!long.TryParse(strClean, NumberStyles.Integer, CultureInfo.InvariantCulture, out lValue))
                throw new ArgumentException(strName + " 값이 정수가 아닙니다: " + strValue, strName);
            return lValue.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>건수 문자열을 0 이상의 정수로 바꾼다.</summary>
        private static long ParseCount(string strValue, string strName)
        {
            long lValue;
            if (!long.TryParse(Clean(strValue), NumberStyles.Integer, CultureInfo.InvariantCulture, out lValue) || lValue < 0)
                throw new ArgumentException(strName + "는 0 이상의 정수여야 합니다.", strName);
            return lValue;
        }

        /// <summary>Slot 번호가 1~99 정수인지 확인한다(WAFER_ID = Lot + '-' + 2자리 Slot).</summary>
        private static string RequiredSlot(string strValue)
        {
            int iSlot;
            if (!int.TryParse(Clean(strValue), NumberStyles.Integer, CultureInfo.InvariantCulture, out iSlot) || iSlot < 1 || iSlot > 99)
                throw new ArgumentException("Slot 번호는 1~99 정수여야 합니다: " + strValue, "Slot");
            return iSlot.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>시험 일시가 yyyyMMddHHmmss 14자리 날짜인지 확인한다.</summary>
        private static string RequiredTestTime(string strValue)
        {
            string strClean = Clean(strValue);
            DateTime dtValue;
            if (!DateTime.TryParseExact(strClean, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtValue))
                throw new ArgumentException("시험 일시는 yyyyMMddHHmmss 14자리여야 합니다: " + strValue, "TestTime");
            return strClean;
        }

        /// <summary>시퀀스 번호(파일 번호) 문자열이 1 이상의 정수인지 확인하고 정리한 값을 돌려준다.</summary>
        private static string RequiredSeq(string strValue, string strName)
        {
            long lValue;
            if (!long.TryParse(Clean(strValue), NumberStyles.Integer, CultureInfo.InvariantCulture, out lValue) || lValue <= 0)
                throw new ArgumentException(strName + "는 1 이상의 정수여야 합니다: " + strValue, strName);
            return lValue.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>바이트 길이 컬럼(VARCHAR2(n) / n CHAR 중 짧은 쪽 기준)에 맞는 필수 문자열인지 확인한다.</summary>
        private static string Required(string strValue, string strName, int iMaxBytes)
        {
            string strResult = Clean(strValue);
            int iLength = Encoding.UTF8.GetByteCount(strResult);
            if (iLength == 0 || iLength > iMaxBytes)
                throw new ArgumentException(strName + "은(는) 1~" + iMaxBytes + "바이트로 입력하세요.", strName);
            return strResult;
        }

        /// <summary>CHAR 길이 컬럼(VARCHAR2(n CHAR))에 맞는 필수 문자열인지 확인한다.</summary>
        private static string RequiredChars(string strValue, string strName, int iMaxChars)
        {
            string strResult = Clean(strValue);
            if (strResult.Length == 0 || strResult.Length > iMaxChars)
                throw new ArgumentException(strName + "은(는) 1~" + iMaxChars + "자로 입력하세요: " + strValue, strName);
            return strResult;
        }

        /// <summary>선택 문자열(빈 값이면 NULL 저장)이 CHAR 길이를 넘지 않는지 확인한다.</summary>
        private static string OptionalChars(string strValue, string strName, int iMaxChars)
        {
            string strResult = Clean(strValue);
            if (strResult.Length > iMaxChars)
                throw new ArgumentException(strName + "은(는) " + iMaxChars + "자 이하로 입력하세요: " + strValue, strName);
            return strResult;
        }

        #endregion
    }
}
