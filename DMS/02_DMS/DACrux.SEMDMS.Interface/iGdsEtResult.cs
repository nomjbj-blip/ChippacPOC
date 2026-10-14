using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.SEMDMS.Interface
{
    /// <summary>
    /// E-Test 결과 파일(xlsx)을 DB에 저장 / 조회 / 삭제하는 원격 호출 계약이다.
    /// 설계: 문서/2026-10-07_ETest_결과파일_Parsing_DB설계.md 9절 / 10절
    /// 파일 Parsing은 클라이언트가 하고, 서버는 받은 행을 검증 / 저장한다.
    /// 저장 순서: BeginEtFile(헤더, LOADING) -> MergeEtItem(항목 번호 받기) -> CreateEtDieBatch -> CreateEtValueBatch(10,000행씩) -> CompleteEtFile(READY)
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

        /// <summary>파일 1건. 반환 열: SRC_FILE_SEQ / FILE_NAME / FACTORY / DEVICE_ID / LOT_ID / SLOT_NO / WAFER_ID / TEST_PROGRAM / TEST_TIME / FILE_PREFIX / SHEET_NAME / DIE_COUNT / ITEM_COUNT / VALUE_COUNT / LOAD_STATUS / 감사 컬럼</summary>
        DataTable GetEtFile(string strSrcFileSeq);

        /// <summary>Device의 파일 목록(최근 시험 순).</summary>
        DataTable GetEtFileList(string strFactory, string strDeviceID);

        /// <summary>파일 1건과 그 Die / 측정값을 실제로 삭제한다. 측정 항목(Device 단위)은 남긴다. 반환: 지운 측정값 수</summary>
        int DeleteEtFile(string strSrcFileSeq, string strUpdateUser);

        /// <summary>
        /// 저장을 시작한다.
        /// FileInfo 순서: 0 Factory / 1 Device ID / 2 파일 이름 / 3 Lot ID / 4 Slot 번호(1~99, 사용자 입력) / 5 시험 프로그램 /
        ///                6 시험 일시(yyyyMMddHHmmss) / 7 파일 접두어(선택) / 8 시트 이름 / 9 Die 수 / 10 항목 수 / 11 User
        ///                [12 Customer / 13 File Device / 14 Wafer Type / 15 Wafer Rev / 16 Lot-Wafer No / 17 Fab(선택)] - 2026-10-12 파일명 8칸 규칙.
        ///                18항목이면 PARSE_RULE_VER 2 로 저장하고 7 파일 접두어는 쓰지 않는다. 12항목도 계속 받는다(PARSE_RULE_VER 1).
        /// 처리: 입력 검증 -> 같은 Wafer 파일 확인(Factory + WAFER_ID, 있으면 거부) -> 헤더 생성(LOADING) -> 헤더 1행 반환(SRC_FILE_SEQ / WAFER_ID 포함)
        /// </summary>
        DataTable BeginEtFile(string[] FileInfo);

        /// <summary>
        /// 저장 확정. DB의 Die 수 = 헤더 Die 수, 측정값 수 = Die 수 x 항목 수이면 READY로 바꾼다. 다르면 LOADING으로 두고 MISMATCH를 반환한다.
        /// 반환 1행: RESULT(OK / MISMATCH) / LOAD_STATUS / EXPECTED_DIE / DB_DIE / FAIL_DIE / EXPECTED_VALUE / DB_VALUE / NOT_TESTED / EMPTY / MESSAGE
        /// </summary>
        DataTable CompleteEtFile(string strSrcFileSeq, string strUpdateUser);

        #endregion

        #region [TQP_GDS_ET_ITEM]

        /// <summary>
        /// 파일의 측정 항목을 Device 항목 마스터에 맞춘다. 없는 항목만 추가하고, 같은 항목인데 단위가 다르면 거부한다.
        /// aKey: 0 Factory / 1 Device ID
        /// aItems 열: 0 ITEM_NAME(원문 헤더) / 1 PARA_NAME / 2 COND_VALUE / 3 CHAIN_NAME / 4 CHAIN_NAME2 / 5 UNIT / 6 TEST_NO / 7 DISPLAY_ORDER (빈 값 허용: 2 / 4 / 5 / 6 / 7)
        /// 반환: Device 전체 항목. 열: ET_ITEM_SEQ / ITEM_NAME / PARA_NAME / COND_VALUE / CHAIN_NAME / CHAIN_NAME2 / UNIT / TEST_NO / DISPLAY_ORDER
        /// </summary>
        DataTable MergeEtItem(string[] aKey, string[,] aItems, string strUpdateUser);

        /// <summary>Device의 측정 항목 목록. 열은 MergeEtItem 반환과 같다.</summary>
        DataTable GetEtItemList(string strFactory, string strDeviceID);

        #endregion

        #region [TQP_GDS_ET_DIE]

        /// <summary>
        /// Die 정보 배치를 저장한다. 파일이 LOADING이어야 한다.
        /// aParas 열: 0 DUT_NO / 1 DIE_X / 2 DIE_Y / 3 HW_BIN / 4 SW_BIN / 5 PASS_FLAG(P / F) / 6 FLAG_CODE / 7 TEST_TIME_MS / 8 TESTS_EXECUTED / 9 HEAD_SITE
        /// 반환: 저장한 행 수
        /// </summary>
        int CreateEtDieBatch(string strSrcFileSeq, string[,] aParas, string strUpdateUser);

        /// <summary>파일의 Die 목록. 열: DUT_NO / DIE_X / DIE_Y / HW_BIN / SW_BIN / PASS_FLAG / FLAG_CODE / TEST_TIME_MS / TESTS_EXECUTED / HEAD_SITE</summary>
        DataTable GetEtDieList(string strSrcFileSeq);

        #endregion

        #region [TQP_GDS_ET_VALUE]

        /// <summary>
        /// 측정값 배치(최대 10,000행)를 저장한다. 파일이 LOADING이어야 한다.
        /// aParas 열: 0 DUT_NO / 1 ET_ITEM_SEQ / 2 MEAS_VALUE(G17, 미측정 / 빈 값이면 빈 문자열) / 3 VALUE_STATUS(V / N / E)
        /// 반환: 저장한 행 수
        /// </summary>
        int CreateEtValueBatch(string strSrcFileSeq, string[,] aParas, string strUpdateUser);

        /// <summary>파일의 측정값 전체. 열: DUT_NO / ET_ITEM_SEQ / ITEM_NAME / MEAS_VALUE / VALUE_STATUS</summary>
        DataTable GetEtValueList(string strSrcFileSeq);

        #endregion

        #region [TQP_GDS_ET_LIMIT]

        /// <summary>측정 항목이 있는 Device 목록. 열: DEVICE_ID / ITEM_COUNT</summary>
        DataTable GetEtDeviceList(string strFactory);

        /// <summary>Device 의 공정 목록(업로드된 파일 + Spec 이 있는 공정). 열: TEST_PROGRAM</summary>
        DataTable GetEtTestProgramList(string strFactory, string strDeviceID);

        /// <summary>
        /// Spec 화면 목록 (2026-10-12: Device + 공정 + Parameter 헤더 전체 단위). 정의된 Parameter 만 나온다.
        /// 열: ITEM_NAME / CHAIN_NAME / CHAIN_NAME2 / UNIT / ITEM_UNIT / IN_DATA(Y = 그 공정 READY 파일에 값 있음) / VALUE_COUNT / NOT_TESTED_COUNT /
        ///     MIN_VALUE / MEDIAN_VALUE / P99_VALUE / MAX_VALUE / FAIL_COUNT(Enable 이면 범위 밖 + '-') / LOWER_LIMIT / UPPER_LIMIT / ENABLE_FLAG / SPEC_TEXT / REMARK / UPDATE_USER / UPDATE_TIME
        /// 통계는 그 공정의 READY 파일 값이다.
        /// </summary>
        DataTable GetEtLimitList(string strFactory, string strDeviceID, string strTestProgram);

        /// <summary>Device + 공정의 저장된 Spec (판정용). 열: ITEM_NAME / LOWER_LIMIT / UPPER_LIMIT / UNIT / ENABLE_FLAG / SPEC_TEXT / REMARK</summary>
        DataTable GetEtLimit(string strFactory, string strDeviceID, string strTestProgram);

        /// <summary>
        /// Spec 저장(있으면 수정 / 없으면 추가). aKey: 0 Factory / 1 Device ID / 2 공정(Test Type)
        /// aRows 열: 0 Parameter(헤더 전체, 끝 '#번호' 제거 / 대문자로 저장) / 1 LOWER(빈 값 = 없음) / 2 UPPER(빈 값 = 없음) / 3 ENABLE_FLAG(Y/N) / 4 UNIT(빈 값이면 측정 항목 단위) / 5 SPEC_TEXT / 6 REMARK
        /// Lower / Upper 중 하나는 있어야 하고 둘 다 있으면 Lower &lt;= Upper. 아직 업로드되지 않은 Parameter 도 저장한다. 반환: 저장한 행 수
        /// </summary>
        int SaveEtLimit(string[] aKey, string[,] aRows, string strUpdateUser);

        /// <summary>Spec 1행 삭제. aKey: 0 Factory / 1 Device ID / 2 공정 / 3 Parameter. 반환: 지운 행 수(0 또는 1)</summary>
        int DeleteEtLimit(string[] aKey, string strUpdateUser);

        #endregion

        #region [TQP_GDS_ET_CHAIN_GROUP]

        /// <summary>
        /// Chain 묶기 목록: 측정 항목에서 뽑은 E-Test Chain + 묶음 (대표 Chain 이름 없이 묶음 번호만, 2026-10-12).
        /// 열: CHAIN_NAME / ITEM_COUNT / GROUP_SEQ(빈 값 = 묶음 없음) / GROUP_COUNT / GROUP_CHAINS(묶음 Chain 전부, " + ") / REMARK / UPDATE_USER / UPDATE_TIME
        /// </summary>
        DataTable GetEtChainGroupList(string strFactory, string strDeviceID);

        /// <summary>Setup Chain 목록: Factory 의 GDS Chain 이름(현재 READY 버전). 열: DEVICE_ID / CHAIN_NAME</summary>
        DataTable GetEtGdsChainList(string strFactory);

        /// <summary>
        /// Chain 묶기. aKey: 0 Factory / 1 Device ID. aRows 열: 0 묶음 구분값(같은 값끼리 한 묶음) / 1 E-Test Chain / 2 비고.
        /// 묶음마다 Chain 2개 이상. 다른 묶음에 있던 Chain 은 옮기고, 1개만 남은 이전 묶음은 지운다. 반환: 만든 묶음 수
        /// </summary>
        int CreateEtChainGroups(string[] aKey, string[,] aRows, string strUpdateUser);

        /// <summary>묶음 해제(Chain 이 들어 있는 묶음 전체). aKey: 0 Factory / 1 Device ID / 2 E-Test Chain. 반환: 지운 행 수</summary>
        int DeleteEtChainGroup(string[] aKey, string strUpdateUser);

        /// <summary>
        /// Setup Chain(GDS Chain 이름) -> E-Test Chain. 이름이 같으면 EXACT, 없으면 이름_FH / 이름_FL (SUFFIX), 찾은 Chain 의 묶음 나머지는 GROUP.
        /// 열: CHAIN_NAME / MATCH_TYPE / GROUP_SEQ
        /// </summary>
        DataTable GetEtChainMatch(string strFactory, string strDeviceID, string strSetupChain);

        /// <summary>
        /// Setup Chain 으로 찾은 E-Test Chain(묶음 포함)의 Spec(모든 공정) + Spec 없는 측정 항목 + READY 파일 값 통계 / Fail 수.
        /// 열: TEST_PROGRAM / ITEM_NAME / CHAIN_NAME / CHAIN_NAME2 / DEFINED / LOWER_LIMIT / UPPER_LIMIT / UNIT / ENABLE_FLAG / SPEC_TEXT / REMARK /
        ///     FILE_COUNT / VALUE_COUNT / NOT_TESTED_COUNT / MIN_VALUE / MAX_VALUE / FAIL_COUNT / MATCH_CHAIN
        /// GDS 화면 등 다른 화면도 이 메서드 하나로 Setup Chain 의 E-Test 정보를 찾을 수 있다(E-Test Device 는 Sub Device 목록으로 찾는다).
        /// </summary>
        DataTable GetEtChainSpecInfo(string strFactory, string strDeviceID, string strSetupChain);

        #endregion
    }
}
