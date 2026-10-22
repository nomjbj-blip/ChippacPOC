using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.SEMDMS.Interface
{
    /// <summary>
    /// GDS Map Setup 화면에서 만든 Chain을 DB에 저장 / 조회 / 삭제하는 원격 호출 계약이다.
    /// 설계: 문서/설계/02_GDS/2026-10-06_GDS_Chain_DB저장_설계.md 10.1절
    /// 2026-10-06 결정: 버전 이력 없음(VERSION_NO는 항상 1, 저장 = 덮어쓰기) / 삭제는 실제 삭제.
    /// 저장 순서: BeginChainSave(헤더 / Layer / 규칙, 기존 내용 삭제) -> CreateChainMemberBatch / CreateChainEditBatch -> CompleteChainSave
    /// 문장마다 자동 Commit이며, 확정 전에는 VERSION_STATUS = DRAFT로 남아 "저장 미완료"로 구분한다.
    /// </summary>
    public interface iGdsChain
    {
        #region [TQP_GDS_CHAIN]

        /// <summary>
        /// Device의 Chain 목록을 조회한다. 없으면 0행이다.
        /// 반환 열: CHAIN_SEQ / CHAIN_CODE / CHAIN_NAME / COLOR_ARGB / APPLY_LAYER_RULES / CELL_SIZE / VERSION_STATUS(READY / DRAFT) /
        ///          MEMBER_COUNT / EDIT_COUNT / UPDATE_USER / UPDATE_TIME
        /// </summary>
        DataTable GetChainList(string strFactory, string strDeviceID);

        /// <summary>Chain 1개를 실제로 삭제한다(하위 Layer / 규칙 / Member / 편집 기록 포함). 반환: 지운 Chain 수(0 또는 1)</summary>
        int DeleteChain(string strFactory, string strDeviceID, string strChainSeq, string strUpdateUser);

        #endregion

        #region [TQP_GDS_CHAIN_VER]

        /// <summary>
        /// Chain 저장을 시작한다. Device Map이 READY여야 한다.
        /// ChainInfo 순서: 0 Factory / 1 Device ID / 2 Chain Seq(신규면 빈 값) / 3 Chain Code / 4 Chain 이름 / 5 색상 ARGB /
        ///                 6 Layer 규칙 적용(0/1) / 7 격자 크기 / 8 User
        /// aLayers 열: 0 LAYER_ID / 1 DATA_TYPE (탐색 Layer, 1행 이상)
        /// aRules 열: 0 FIRST_LAYER_ID / 1 FIRST_DATA_TYPE / 2 SECOND_LAYER_ID / 3 SECOND_DATA_TYPE / 4 TOLERANCE(G17) (없으면 null 또는 0행)
        ///   Layer 는 번호 + DataType 단위다. 예전 열 수(aLayers 1열 / aRules 3열)는 DataType 0 으로 저장한다 (2026-10-20).
        /// 처리: 신규면 Chain 생성(시퀀스), 기존이면 이전 저장 내용을 지우고 VERSION 1을 DRAFT로 다시 만든 뒤 Layer / 규칙을 저장한다.
        /// 반환 1행: CHAIN_SEQ / CHAIN_CODE
        /// </summary>
        DataTable BeginChainSave(string[] ChainInfo, string[,] aLayers, string[,] aRules);

        /// <summary>
        /// Chain 최종 포함 도형 배치를 저장한다. aKey: 0 Factory / 1 Device ID / 2 Chain Seq
        /// aParas 열: 0 PLACED_ELEMENT_ID / 1 ROLE_CODE(INPUT / OUTPUT / MEMBER) / 2 INCLUSION_SOURCE(TRACE / MANUAL / ENDPOINT) /
        ///            3 MARKER_X / 4 MARKER_Y (G17, INPUT / OUTPUT만, MEMBER는 빈 값)
        /// 반환: 저장한 행 수
        /// </summary>
        int CreateChainMemberBatch(string[] aKey, string[,] aParas, string strUpdateUser);

        /// <summary>
        /// 수동 편집 기록 배치를 저장한다. aKey는 CreateChainMemberBatch와 같다.
        /// aParas 열: 0 EDIT_SEQ(1부터) / 1 PLACED_ELEMENT_ID / 2 ACTION_CODE(ADD / EXCLUDE / RESTORE) / 3 BATCH_EDIT_SEQ(없으면 빈 값)
        /// 반환: 저장한 행 수
        /// </summary>
        int CreateChainEditBatch(string[] aKey, string[,] aParas, string strUpdateUser);

        /// <summary>
        /// Chain 저장을 확정한다. aKey: 0 Factory / 1 Device ID / 2 Chain Seq / 3 예상 Member 수 / 4 예상 편집 수
        /// DB 건수 = 예상 건수, INPUT 1개, OUTPUT 0~1개이면 READY로 바꾼다. 아니면 DRAFT로 두고 MISMATCH를 반환한다.
        /// 반환 1행: RESULT(OK / MISMATCH) / VERSION_STATUS / EXPECTED_MEMBER / DB_MEMBER / EXPECTED_EDIT / DB_EDIT /
        ///           INPUT_COUNT / OUTPUT_COUNT / MESSAGE
        /// </summary>
        DataTable CompleteChainSave(string[] aKey, string strUpdateUser);

        #endregion

        #region [Chain 상세 조회]

        /// <summary>Chain 탐색 Layer. 반환 열: LAYER_ID / DATA_TYPE (2026-10-20 DATA_TYPE 추가)</summary>
        DataTable GetChainLayers(string strFactory, string strDeviceID, string strChainSeq);

        /// <summary>Chain Layer 연결 규칙. 반환 열: FIRST_LAYER_ID / FIRST_DATA_TYPE / SECOND_LAYER_ID / SECOND_DATA_TYPE / TOLERANCE (2026-10-20 DataType 추가)</summary>
        DataTable GetChainRules(string strFactory, string strDeviceID, string strChainSeq);

        /// <summary>Chain 포함 도형. 반환 열: PLACED_ELEMENT_ID / ROLE_CODE / INCLUSION_SOURCE / MARKER_X / MARKER_Y</summary>
        DataTable GetChainMembers(string strFactory, string strDeviceID, string strChainSeq);

        /// <summary>Chain 수동 편집 기록. 반환 열: EDIT_SEQ / PLACED_ELEMENT_ID / ACTION_CODE / BATCH_EDIT_SEQ</summary>
        DataTable GetChainEdits(string strFactory, string strDeviceID, string strChainSeq);

        #endregion

        #region [Chain 상세 범위 조회 (2026-10-21)]

        /// <summary>Chain 번호 범위(시작 ~ 끝)의 탐색 Layer. 반환 열: CHAIN_SEQ / LAYER_ID / DATA_TYPE</summary>
        DataTable GetChainLayersRange(string strFactory, string strDeviceID, string strFromSeq, string strToSeq);

        /// <summary>Chain 번호 범위의 Layer 연결 규칙. 반환 열: CHAIN_SEQ / FIRST_LAYER_ID / FIRST_DATA_TYPE / SECOND_LAYER_ID / SECOND_DATA_TYPE / TOLERANCE</summary>
        DataTable GetChainRulesRange(string strFactory, string strDeviceID, string strFromSeq, string strToSeq);

        /// <summary>Chain 번호 범위의 포함 도형. 반환 열: CHAIN_SEQ / PLACED_ELEMENT_ID / ROLE_CODE / INCLUSION_SOURCE / MARKER_X / MARKER_Y / LAYER_ID / DATA_TYPE</summary>
        DataTable GetChainMembersRange(string strFactory, string strDeviceID, string strFromSeq, string strToSeq);

        /// <summary>Chain 번호 범위의 수동 편집 기록. 반환 열: CHAIN_SEQ / EDIT_SEQ / PLACED_ELEMENT_ID / ACTION_CODE / BATCH_EDIT_SEQ</summary>
        DataTable GetChainEditsRange(string strFactory, string strDeviceID, string strFromSeq, string strToSeq);

        #endregion

        #region [Chain 일괄 저장 (2026-10-20, GDS Map View U8)]

        /// <summary>
        /// Chain 여러 개를 한 번에 저장한다(새 Chain 만, 같은 Code 가 이미 있으면 건너뜀).
        /// aKey: Factory / Device / Layer 규칙 적용(0 / 1) / 격자 크기
        /// aChains 열: CHAIN_CODE / CHAIN_NAME / COLOR_ARGB
        /// aLayers 열: LAYER_ID / DATA_TYPE (모든 Chain 의 탐색 Layer)
        /// aMembers 열: CHAIN_INDEX(aChains 행 번호, 0부터) / PLACED_ELEMENT_ID / ROLE_CODE / INCLUSION_SOURCE / MARKER_X / MARKER_Y (1만 행 이하)
        /// Chain 마다 INPUT 이 1개여야 한다. 중간에 실패하면 이번 호출에서 만든 Chain 을 지우고 오류를 낸다.
        /// 반환 열: CHAIN_CODE / CHAIN_SEQ / RESULT(CREATED / SKIPPED) / MEMBER_COUNT
        /// </summary>
        DataTable CreateChainBulk(string[] aKey, string[,] aChains, string[,] aLayers, string[,] aMembers, string strUpdateUser);

        /// <summary>Device 의 Chain 목록(포함 도형 수 계산 없음). 반환 열: CHAIN_SEQ / CHAIN_CODE / CHAIN_NAME / COLOR_ARGB / VERSION_STATUS / UPDATE_TIME</summary>
        DataTable GetChainCodeList(string strFactory, string strDeviceID);

        /// <summary>Chain 여러 개를 한 번에 실제로 삭제한다(하위 테이블은 FK CASCADE). 반환: 요청한 Chain 수</summary>
        int DeleteChainBulk(string strFactory, string strDeviceID, string[] aChainSeqs, string strUpdateUser);

        /// <summary>
        /// Chain 여러 개의 색을 한 번에 바꾼다(저장 내용 VERSION 1 의 COLOR_ARGB 만, 포함 도형은 그대로) (2026-10-21).
        /// aChainSeqs 1만 개 이하. strColorArgb = Color.ToArgb() 정수 글자. 반환: 요청한 Chain 수
        /// </summary>
        int UpdateChainColorBulk(string strFactory, string strDeviceID, string[] aChainSeqs, string strColorArgb, string strUpdateUser);

        #endregion
    }
}
