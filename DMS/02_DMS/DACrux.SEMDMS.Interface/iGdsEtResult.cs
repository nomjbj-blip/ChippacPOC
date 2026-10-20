using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.SEMDMS.Interface
{
    /// <summary>
    /// E-Test 결과 파일(xlsx)을 DB에 저장 / 조회 / 삭제하는 원격 호출 계약이다.
    /// 설계: 문서/설계/04_ETest/2026-10-07_ETest_결과파일_Parsing_DB설계.md 9절 / 10절, 문서/설계/04_ETest/2026-10-13_ETest_Upload_재설계.md 9절 / 10절
    /// 파일 Parsing은 클라이언트가 하고, 서버는 받은 행을 검증 / 저장한다.
    /// 저장 순서 (2026-10-14): BeginEtFile(헤더, LOADING) -> CreateEtDieBatch -> CreateEtValueBatch(10,000행씩) -> CompleteEtFile(판정 + Map 좌표 + READY)
    ///   값은 파일 Device + 공정의 등록 Parameter(사용 Y)만 저장한다. 항목 번호는 화면이 GetEtParameterList 에서 받는다(예전 MergeEtItem 은 없앴다).
    /// 같은 Wafer / 같은 공정 파일(Factory + WAFER_ID + 공정 Test Type, 2026-10-12)이 이미 있으면 BeginEtFile이 거부한다. 화면이 GetEtFileByKey로 먼저 묻고 DeleteEtFile 후 다시 저장한다.
    /// 기준 (2026-10-07): Device 중복 허용(같은 Device에 여러 Lot / Wafer), 중복 방지는 WAFER_ID 기준, Slot 번호는 사용자가 입력.
    /// </summary>
    public partial interface iGdsEtResult
    {
        #region [TQP_GDS_SRC_FILE]

        /// <summary>
        /// 같은 Wafer 파일이 있는지 조회한다(UNIQUE 키 Factory + WAFER_ID). 없으면 0행이다.
        /// aKey 순서: 0 Factory / 1 Lot ID / 2 Slot 번호(1~99) [/ 3 공정(Test Type)] -> WAFER_ID = Lot + '-' + 2자리 Slot
        /// 4항목이면 UNIQUE 키(Factory + WAFER_ID + 공정) 1건, 3항목이면 그 Wafer 의 모든 공정 파일.
        /// 반환 열: SRC_FILE_SEQ / FILE_NAME / DEVICE_ID / WAFER_ID / TEST_PROGRAM / TEST_TIME / LOAD_STATUS / DIE_COUNT / ITEM_COUNT / VALUE_COUNT / CREATE_USER / CREATE_TIME
        /// </summary>
        DataTable GetEtFileByKey(string[] aKey);

        /// <summary>
        /// 파일 1건. 반환 열: SRC_FILE_SEQ / FILE_NAME / FACTORY / DEVICE_ID / LOT_ID / SLOT_NO / WAFER_ID / TEST_PROGRAM / TEST_TIME / SHEET_NAME / DIE_COUNT /
        /// ITEM_COUNT(저장 Parameter) / VALUE_COUNT / LOAD_STATUS / 파일명 규칙 값 / PASS_DIE_COUNT · FAIL_DIE_COUNT(Spec 판정) / NOT_TESTED_COUNT / PARSE_RULE_VER /
        /// HEADER_COUNT / DEFINED_COUNT(업로드 때 등록 Parameter 수) / MISSING_COUNT / JUDGE_TIME / MAP_ID / MAP_TIME / 감사 컬럼
        /// </summary>
        DataTable GetEtFile(string strSrcFileSeq);

        /// <summary>Device의 파일 목록(최근 시험 순). 2026-10-14: 헤더 / 등록 / 누락 수, MAP_ID / MAP_DIE_COUNT(좌표를 찾은 Die), 판정 시각 포함.</summary>
        DataTable GetEtFileList(string strFactory, string strDeviceID);

        /// <summary>
        /// E-Test 결과 조회 화면 파일 목록 (2026-10-15). 공정 / Lot 은 빈 값이면 전체, Lot 은 포함 검색(대소문자 무시).
        /// 열: SRC_FILE_SEQ / WAFER_ID / LOT_ID / SLOT_NO / TEST_PROGRAM / TEST_TIME / DEVICE_ID / DIE_COUNT / PASS_DIE_COUNT / FAIL_DIE_COUNT / ITEM_COUNT / VALUE_COUNT /
        ///     ADDED_PARA_COUNT(지금 등록 Parameter 중 이 파일에 없는 수 - 다시 업로드해야 보임) / SPEC_CHANGED(Y = 판정 뒤 Spec 수정) / MAP_ID / MAP_DIE_COUNT /
        ///     JUDGE_TIME / YMS_STATUS / YMS_TIME / YMS_MESSAGE / LOAD_STATUS / CREATE_USER / CREATE_TIME / FILE_NAME
        /// </summary>
        DataTable GetEtFileBrowseList(string strFactory, string strDeviceID, string strTestProgram, string strLot);

        /// <summary>파일 1건과 그 Die / 측정값을 실제로 삭제한다. 측정 항목(등록 Parameter)은 남긴다. 반환: 지운 측정값 수</summary>
        int DeleteEtFile(string strSrcFileSeq, string strUpdateUser);

        /// <summary>
        /// 저장을 시작한다 (2026-10-14: 등록 Parameter 만 저장, PARSE_RULE_VER 3).
        /// FileInfo 21항목: 0 Factory / 1 Device ID / 2 파일 이름 / 3 Lot ID / 4 Slot 번호(1~99) / 5 공정(Test Type) / 6 시험 일시(yyyyMMddHHmmss) /
        ///   7 파일 접두어(쓰지 않음) / 8 시트 이름 / 9 Die 수 / 10 저장할 등록 Parameter 수 / 11 User /
        ///   12 Customer / 13 File Device / 14 Wafer Type / 15 Wafer Rev / 16 Lot-Wafer No / 17 Fab(받지만 저장 안 함) /
        ///   18 파일 전체 헤더 수 / 19 화면이 본 등록 Parameter(사용 Y) 수 / 20 등록됐는데 파일에 없는 수
        /// 등록 Parameter(사용 Y)가 없으면 / 분석 뒤 등록이 바뀌었으면 / 같은 Wafer + 공정 파일이 있으면 거부. 12 / 18항목(이전 방식)도 거부.
        /// 반환: 헤더 1행(SRC_FILE_SEQ / WAFER_ID 포함)
        /// </summary>
        DataTable BeginEtFile(string[] FileInfo);

        /// <summary>
        /// 저장 확정. DB의 Die 수 = 헤더 Die 수, 측정값 수 = Die 수 x 저장 Parameter 수이면 Spec 판정(값 / Die / 파일) + Map 좌표(Map ID = Device) 뒤 READY.
        /// 다르면 LOADING으로 두고 MISMATCH를 반환한다.
        /// 반환 1행: RESULT(OK / MISMATCH) / LOAD_STATUS / EXPECTED_DIE / DB_DIE / FAIL_DIE(Spec 판정) / EXPECTED_VALUE / DB_VALUE / NOT_TESTED / EMPTY / MESSAGE /
        ///          SPEC_FAIL_DIE / MAP_ID(없으면 빈 값) / MAP_DIE / OUT_OF_MAP
        /// </summary>
        DataTable CompleteEtFile(string strSrcFileSeq, string strUpdateUser);

        /// <summary>Device + 공정의 READY 파일을 지금 Spec 으로 다시 판정한다(Spec 화면 저장 뒤). aKey: 0 Factory / 1 Device / 2 공정. 반환: 파일 수</summary>
        int RejudgeEtFiles(string[] aKey, string strUpdateUser);

        // RemapEtFiles(Map 좌표 다시 계산)는 2026-10-15 화면 기능과 함께 원격 호출에서 뺐다. 업로드 때 Map 이 없으면 저장하지 않는다.

        /// <summary>
        /// E-Test 파일 1개를 YMS 테이블에 (다시) 반영한다 (2026-10-14). 저장 확정 / 다시 판정 때는 서버가 자동으로 부른다.
        /// 반환 1행: YMS_STATUS(DONE / WAIT / FAIL) / YMS_STEP / YMS_MESSAGE / YMS_PROGRAM / YMS_TESTAREA / YMS_LOT_SEQ / YMS_WAFER_SEQ /
        /// BIN_COUNTS(Hardware Bin 별 Die 수) / DIE_COUNT / SKIP_DIE_COUNT / PARAM_COUNT / GOOD_DIE_COUNT / FAIL_DIE_COUNT / NETDIE / TESTED_DIE / YIELD / SRC_FILE_SEQ
        /// YMS BIN 은 결과 파일 Hardware Bin, 양품은 파일 DUT 결과 Pass 다 (2026-10-14, E-Test Spec 판정과 별개).
        /// </summary>
        DataTable PublishEtFileToYms(string strSrcFileSeq, string strUpdateUser);

        /// <summary>Map 사용 Die (TQP_USEMAP 가장 큰 PROGRAM_REV). 열: X / Y / USECODE. 업로드 미리보기의 Map 확인용(Map ID = 파일 Device).</summary>
        DataTable GetEtMapDieList(string strFactory, string strMapId);

        #endregion

        #region [TQP_GDS_ET_ITEM]

        /// <summary>Device의 측정 항목 목록(= 등록 Parameter, 모든 공정). 열: ET_ITEM_SEQ / ITEM_NAME / PARA_NAME / COND_VALUE / CHAIN_NAME / CHAIN_NAME2 / UNIT / TEST_NO / DISPLAY_ORDER</summary>
        DataTable GetEtItemList(string strFactory, string strDeviceID);

        #endregion

        #region [TQP_GDS_ET_DIE]

        /// <summary>
        /// Die 정보 배치를 저장한다. 파일이 LOADING이어야 한다.
        /// aParas 열: 0 DUT_NO / 1 DIE_X / 2 DIE_Y / 3 HW_BIN / 4 SW_BIN / 5 PASS_FLAG(P / F, 파일 DUT Flag) / 6 FLAG_CODE / 7 TEST_TIME_MS / 8 TESTS_EXECUTED / 9 HEAD_SITE
        /// 반환: 저장한 행 수
        /// </summary>
        int CreateEtDieBatch(string strSrcFileSeq, string[,] aParas, string strUpdateUser);

        /// <summary>
        /// 파일의 Die 목록. 열: DUT_NO / DIE_X / DIE_Y / HW_BIN / SW_BIN / PASS_FLAG(DUT Flag, 참고) / FLAG_CODE / TEST_TIME_MS / TESTS_EXECUTED / HEAD_SITE /
        /// MAP_X / MAP_Y(시스템 Map 좌표) / JUDGE_FLAG(Spec 판정 P / F) / FAIL_COUNT / NOT_TESTED_COUNT
        /// </summary>
        DataTable GetEtDieList(string strSrcFileSeq);

        #endregion

        #region [TQP_GDS_ET_VALUE]

        /// <summary>
        /// 측정값 배치(최대 10,000행)를 저장한다. 파일이 LOADING이어야 한다.
        /// aParas 열: 0 DUT_NO / 1 ET_ITEM_SEQ(파일 Device + 공정의 등록 Parameter, 사용 Y) / 2 MEAS_VALUE(G17, 미측정 / 빈 값이면 빈 문자열) / 3 VALUE_STATUS(V / N / E)
        /// 반환: 저장한 행 수
        /// </summary>
        int CreateEtValueBatch(string strSrcFileSeq, string[,] aParas, string strUpdateUser);

        /// <summary>
        /// 파일의 측정값 전체. 열: DUT_NO / ET_ITEM_SEQ / ITEM_NAME(저장 이름) / MEAS_VALUE / VALUE_STATUS / JUDGE_FLAG(P / F / 빈 값 = 판정 안 함) /
        /// HEADER_TEXT / PARA_NAME / COND_VALUE / CHAIN_NAME / CHAIN_NAME2 / UNIT / TEST_NO / DISPLAY_ORDER
        /// </summary>
        DataTable GetEtValueList(string strSrcFileSeq);

        #endregion

        #region [TQP_GDS_ET_LIMIT]

        /// <summary>측정 항목이 있는 Device 목록. 열: DEVICE_ID / ITEM_COUNT</summary>
        DataTable GetEtDeviceList(string strFactory);

        /// <summary>Device 의 공정 목록(업로드된 파일 + Spec + 등록 Parameter 가 있는 공정). 열: TEST_PROGRAM</summary>
        DataTable GetEtTestProgramList(string strFactory, string strDeviceID);

        /// <summary>Device + 공정의 저장된 Spec (판정용). 열: ITEM_NAME(저장 이름) / LOWER_LIMIT / UPPER_LIMIT / UNIT / ENABLE_FLAG / SPEC_TEXT / REMARK</summary>
        DataTable GetEtLimit(string strFactory, string strDeviceID, string strTestProgram);

        /// <summary>
        /// Spec 저장(있으면 수정 / 없으면 추가). aKey: 0 Factory / 1 Device ID / 2 공정(Test Type)
        /// aRows 열: 0 Parameter(저장 이름, 또는 헤더 원문 -> 저장 이름으로 바꿈) / 1 LOWER(빈 값 = 없음) / 2 UPPER(빈 값 = 없음) / 3 ENABLE_FLAG(Y/N) /
        ///          4 UNIT(빈 값이면 등록 단위) / 5 SPEC_TEXT / 6 REMARK
        /// Lower / Upper 중 하나는 있어야 하고 둘 다 있으면 Lower &lt;= Upper. 등록되지 않은 Parameter 가 있으면 거부한다(2026-10-14). 반환: 저장한 행 수
        /// </summary>
        int SaveEtLimit(string[] aKey, string[,] aRows, string strUpdateUser);

        /// <summary>Spec 1행 삭제. aKey: 0 Factory / 1 Device ID / 2 공정 / 3 Parameter(저장 이름 또는 헤더 원문). 반환: 지운 행 수(0 또는 1)</summary>
        int DeleteEtLimit(string[] aKey, string strUpdateUser);

        #endregion

        #region [Setup Chain -> E-Test 정보 (인계 API)]

        /// <summary>
        /// Setup Chain(GDS Chain 이름)의 E-Test Parameter / Spec. 다른 화면(GDS 등)도 이 메서드 하나로 찾는다.
        /// 2026-10-14 Para 화면 재구성: 대표 Chain 을 없애고 Parameter 의 GDS Chain(Chain 1, 사용자 입력)을 본다:
        ///   Device(E-Test Device = 같은 이름 GDS Device) -> GDS Chain 이름(대소문자 무시)이 같은 모든 공정 등록 Parameter + Spec (2026-10-15 Sub Device 연결 제거).
        /// 열: DEVICE_ID / TEST_PROGRAM / ITEM_NAME / HEADER_TEXT / CHAIN_NAME / CHAIN_NAME2 / GDS_CHAIN_NAME / UNIT / USE_FLAG /
        ///     HAS_SPEC / LOWER_LIMIT / UPPER_LIMIT / ENABLE_FLAG / SPEC_TEXT. 없으면 0행(열은 있음).
        /// </summary>
        DataTable GetEtChainSpecInfo(string strFactory, string strDeviceID, string strSetupChain);

        #endregion
    }
}
