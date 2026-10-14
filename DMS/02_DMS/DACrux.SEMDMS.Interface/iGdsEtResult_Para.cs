using System;
using System.Data;

namespace DACrux.SEMDMS.Interface
{
    /// <summary>
    /// E-Test Para Setup 원격 호출 계약 (2026-10-14).
    /// 결과 파일 헤더를 Parameter 로 등록하고(TQP_GDS_ET_ITEM, Device + 공정 + Parameter),
    /// 단자(Chain) 이름을 사용자가 입력한 대표 Chain 으로 묶고(TQP_GDS_ET_REP_CHAIN),
    /// 대표 Chain 을 GDS Setup Chain 과 1:1 로 잇는다(TQP_GDS_ET_CHAIN_LINK).
    /// 설계: 문서/2026-10-14_ETest_Para_Setup_설계.md. 이름 분해 / 변환 규칙은 EtParameterNameRule.
    /// 대표 Chain / 연결은 GDS 대표 Device(예 U9) 단위다. E-Test Device(예 OMM)는 Sub Device 연결로 찾는다.
    /// </summary>
    public partial interface iGdsEtResult
    {
        #region [Parameter 등록 (TQP_GDS_ET_ITEM)]

        /// <summary>Factory 의 등록 Parameter 가 있는 Device + 공정 목록. 열: DEVICE_ID / TEST_PROGRAM / PARA_COUNT</summary>
        DataTable GetEtParaProgramList(string strFactory);

        /// <summary>
        /// Device + 공정의 등록 Parameter 목록.
        /// 열: ET_ITEM_SEQ / ITEM_NAME(변환 이름) / HEADER_TEXT(원문) / MEAS_TYPE / PARA_NAME / COND_VALUE / CHAIN_NAME / CHAIN_NAME2 / FORCE_LEG / FORCE_LEG2 /
        ///     CHAIN_TYPE / UNIT / TEST_NO / DISPLAY_ORDER / USE_FLAG / REG_SOURCE / REG_FILE_NAME / REMARK /
        ///     REP_CHAIN_NAME / REP_CHAIN_NAME2 / GDS_CHAIN_NAME / GDS_CHAIN_NAME2 / HAS_SPEC / LOWER_LIMIT / UPPER_LIMIT / ENABLE_FLAG / FILE_COUNT / UPDATE_USER / UPDATE_TIME
        /// 대표 Chain / GDS 열은 E-Test Device 의 GDS 대표 Device(Sub Device 연결, 없으면 같은 이름) 기준이다.
        /// </summary>
        DataTable GetEtParameterList(string strFactory, string strDeviceID, string strTestProgram);

        /// <summary>
        /// 헤더를 Parameter 로 등록한다. 서버가 EtParameterNameRule 로 다시 분해 / 변환해 저장한다.
        /// aKey: 0 Factory / 1 Device ID / 2 공정(Test Type) / 3 출처(FILE / SPEC) / 4 등록 파일 이름(선택)
        /// aRows 열: 0 헤더 원문 / 1 단위 / 2 Test No / 3 열 순서 (1 ~ 3 빈 값 허용)
        /// 이미 같은 헤더가 등록돼 있으면 건너뛴다. 분해 실패 / 같은 헤더 두 번 / 다른 헤더가 같은 Parameter 이름 / 단위 다름이 하나라도 있으면 전체 거부.
        /// 반환 1행: REGISTERED / SKIPPED
        /// </summary>
        DataTable RegisterEtParameters(string[] aKey, string[,] aRows, string strUpdateUser);

        /// <summary>사용 여부 / 비고 저장. aKey: 0 Factory / 1 Device / 2 공정. aRows 열: 0 Parameter(ITEM_NAME) / 1 USE_FLAG(Y/N) / 2 비고. 반환: 저장한 행 수</summary>
        int SaveEtParameters(string[] aKey, string[,] aRows, string strUpdateUser);

        /// <summary>
        /// Parameter 삭제. aKey: 0 Factory / 1 Device / 2 공정. 측정값이 저장된 Parameter 는 거부한다(사용 안 함으로 바꾼다).
        /// 같은 키의 Spec 도 함께 지운다. 반환: 지운 Parameter 수
        /// </summary>
        int DeleteEtParameters(string[] aKey, string[] aParameters, string strUpdateUser);

        #endregion

        #region [대표 Chain (TQP_GDS_ET_REP_CHAIN) / GDS 연결 (TQP_GDS_ET_CHAIN_LINK)]

        /// <summary>
        /// 단자 목록: GDS 대표 Device 의 Sub Device(와 같은 이름 Device)에 등록된 Parameter(사용 Y)의 단자 1 / 2 + 대표 Chain.
        /// 등록 Parameter 에 없는데 대표 Chain 에 남아 있는 단자도 나온다(PARA_COUNT 0).
        /// 열: TERMINAL_NAME / FORCE_LEG / CHAIN_TYPE / TEST_PROGRAMS / PARA_COUNT / SPEC_COUNT / REP_CHAIN_NAME / REMARK / UPDATE_USER / UPDATE_TIME
        /// </summary>
        DataTable GetEtTerminalList(string strFactory, string strGdsDeviceID);

        /// <summary>
        /// 대표 Chain 목록 + GDS 연결 상태.
        /// 열: REP_CHAIN_NAME / TERMINALS(", ") / TERMINAL_COUNT / CHAIN_SEQ / GDS_CHAIN_NAME(지금 이름) / LINK_TYPE(NAME / MANUAL) /
        ///     LINK_STATUS(NAME / MANUAL / NAME_CHANGED / READY / TAKEN / MULTIPLE / NONE) / CANDIDATES(이름이 같은 GDS Chain, ", ")
        /// READY = 이름이 같은 GDS Chain 1개, 아직 연결 전 / TAKEN = 그 GDS Chain 이 다른 대표에 연결됨
        /// </summary>
        DataTable GetEtRepChainList(string strFactory, string strGdsDeviceID);

        /// <summary>GDS Setup Chain 목록(ACTIVE + 현재 READY 버전) + 연결된 대표. 열: CHAIN_SEQ / CHAIN_CODE / CHAIN_NAME / NAME_KEY(비교 이름) / REP_CHAIN_NAME / LINK_TYPE</summary>
        DataTable GetEtGdsChainLinkList(string strFactory, string strGdsDeviceID);

        /// <summary>
        /// 단자를 대표 Chain 으로 묶는다. aKey: 0 Factory / 1 GDS Device / 2 대표 이름(입력값, 이름 규칙으로 바꿔 저장).
        /// 이미 다른 대표에 있던 단자는 옮긴다. 같은 대표 이름이 이미 있으면 그 대표에 더한다. 저장 뒤 이름 연결(LinkEtRepChainsByName)을 한다.
        /// 반환: 저장한 단자 수
        /// </summary>
        int SaveEtRepChain(string[] aKey, string[] aTerminals, string strUpdateUser);

        /// <summary>대표 이름 바꾸기. aKey: 0 Factory / 1 GDS Device / 2 이전 이름 / 3 새 이름(이름 규칙으로 바꿈). 새 이름이 다른 대표와 같으면 거부. 반환: 바꾼 단자 수</summary>
        int RenameEtRepChain(string[] aKey, string strUpdateUser);

        /// <summary>단자를 대표에서 뺀다. aKey: 0 Factory / 1 GDS Device. 단자가 0 이 된 대표의 연결도 지운다. 반환: 뺀 단자 수</summary>
        int RemoveEtRepChainMembers(string[] aKey, string[] aTerminals, string strUpdateUser);

        /// <summary>대표 Chain 삭제(단자 + 연결). aKey: 0 Factory / 1 GDS Device / 2 대표 이름. 반환: 지운 단자 수</summary>
        int DeleteEtRepChain(string[] aKey, string strUpdateUser);

        /// <summary>
        /// 이름 일치 연결: GDS Chain 이름(비교 이름)이 대표 이름 또는 구성 단자 이름과 똑같은 GDS Chain 이 1개이면 연결(NAME).
        /// MANUAL 연결은 건드리지 않는다. 지금 이름이 맞지 않는 NAME 연결은 지운다.
        /// 반환 1행: LINKED / UPDATED / REMOVED / NONE / MULTIPLE / TAKEN
        /// </summary>
        DataTable LinkEtRepChainsByName(string strFactory, string strGdsDeviceID, string strUpdateUser);

        /// <summary>사용자 연결(MANUAL). aKey: 0 Factory / 1 GDS Device / 2 대표 이름 / 3 GDS CHAIN_SEQ. 그 GDS Chain 의 다른 대표 연결은 지운다(1:1). 반환 1</summary>
        int SaveEtChainLink(string[] aKey, string strUpdateUser);

        /// <summary>연결 해제. aKey: 0 Factory / 1 GDS Device / 2 대표 이름. 반환: 지운 행 수</summary>
        int DeleteEtChainLink(string[] aKey, string strUpdateUser);

        /// <summary>
        /// 대표 Chain 의 단자가 단자 1 / 2 로 들어간 모든 공정의 등록 Parameter + Spec.
        /// 열: DEVICE_ID / TEST_PROGRAM / ITEM_NAME / HEADER_TEXT / CHAIN_NAME / CHAIN_NAME2 / MATCH_POSITION(1 / 2 / 1,2) / UNIT / USE_FLAG /
        ///     HAS_SPEC / LOWER_LIMIT / UPPER_LIMIT / ENABLE_FLAG / SPEC_TEXT
        /// </summary>
        DataTable GetEtRepChainSpecInfo(string strFactory, string strGdsDeviceID, string strRepChainName);

        #endregion
    }
}
