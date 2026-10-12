using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.SEMDMS.Interface
{
    /// <summary>
    /// GDS Map Setup 화면에서 만든 Chain을 DB에 저장 / 조회 / 삭제하는 원격 호출 계약이다.
    /// 설계: 문서/2026-10-06_GDS_Chain_DB저장_설계.md 10.1절
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
        /// aLayers 열: 0 LAYER_ID (탐색 Layer, 1행 이상)
        /// aRules 열: 0 FIRST_LAYER_ID / 1 SECOND_LAYER_ID / 2 TOLERANCE(G17) (없으면 null 또는 0행)
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

        /// <summary>Chain 탐색 Layer. 반환 열: LAYER_ID</summary>
        DataTable GetChainLayers(string strFactory, string strDeviceID, string strChainSeq);

        /// <summary>Chain Layer 연결 규칙. 반환 열: FIRST_LAYER_ID / SECOND_LAYER_ID / TOLERANCE</summary>
        DataTable GetChainRules(string strFactory, string strDeviceID, string strChainSeq);

        /// <summary>Chain 포함 도형. 반환 열: PLACED_ELEMENT_ID / ROLE_CODE / INCLUSION_SOURCE / MARKER_X / MARKER_Y</summary>
        DataTable GetChainMembers(string strFactory, string strDeviceID, string strChainSeq);

        /// <summary>Chain 수동 편집 기록. 반환 열: EDIT_SEQ / PLACED_ELEMENT_ID / ACTION_CODE / BATCH_EDIT_SEQ</summary>
        DataTable GetChainEdits(string strFactory, string strDeviceID, string strChainSeq);

        #endregion
    }
}
