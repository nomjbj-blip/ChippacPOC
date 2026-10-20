using System;
using System.Data;

namespace DACrux.SEMDMS.Interface
{
    /// <summary>
    /// E-Test Para 설정 원격 호출 계약 (2026-10-14).
    /// 결과 파일 헤더를 Parameter 로 등록하고(TQP_GDS_ET_ITEM, Device + 공정 + Parameter),
    /// Parameter 마다 GDS Chain(Chain 1 의 GDS Setup Chain 이름, 사용자 입력)과 Spec(TQP_GDS_ET_LIMIT)을 같은 화면에서 편집한다.
    /// 설계: 문서/설계/04_ETest/2026-10-14_ETest_Para_Setup_설계.md (등록 / 이름 변환), 문서/설계/04_ETest/2026-10-14_ETest_Para_화면_재구성_설계.md (화면 / GDS Chain / Spec 편집).
    /// 이름 분해 / 변환 규칙은 EtParameterNameRule. GDS Device 는 E-Test Device 와 이름이 같은(대소문자 무시) READY Map 이다(2026-10-15 Sub Device 연결 제거).
    /// </summary>
    public partial interface iGdsEtResult
    {
        #region [Parameter 등록 (TQP_GDS_ET_ITEM)]

        /// <summary>Factory 의 등록 Parameter 가 있는 Device + 공정 목록. 열: DEVICE_ID / TEST_PROGRAM / PARA_COUNT</summary>
        DataTable GetEtParaProgramList(string strFactory);

        /// <summary>
        /// Device + 공정의 등록 Parameter 목록 (업로드 계획 / 조회 창).
        /// 열: ET_ITEM_SEQ / ITEM_NAME(변환 이름) / HEADER_TEXT(원문) / MEAS_TYPE / PARA_NAME / COND_VALUE / CHAIN_NAME / CHAIN_NAME2 / FORCE_LEG / FORCE_LEG2 /
        ///     CHAIN_TYPE / UNIT / TEST_NO / DISPLAY_ORDER / USE_FLAG / REG_SOURCE / REG_FILE_NAME / REMARK / GDS_CHAIN_NAME /
        ///     HAS_SPEC / LOWER_LIMIT / UPPER_LIMIT / ENABLE_FLAG / FILE_COUNT / UPDATE_USER / UPDATE_TIME
        /// </summary>
        DataTable GetEtParameterList(string strFactory, string strDeviceID, string strTestProgram);

        /// <summary>
        /// E-Test Para 설정 화면 목록: 등록 Parameter + GDS Chain + Spec.
        /// 열: ET_ITEM_SEQ / ITEM_NAME / HEADER_TEXT / MEAS_TYPE / PARA_NAME / COND_VALUE / CHAIN_NAME / CHAIN_NAME2 / ITEM_UNIT / TEST_NO / DISPLAY_ORDER /
        ///     USE_FLAG / REG_SOURCE / REG_FILE_NAME / REMARK / GDS_CHAIN_NAME / HAS_SPEC / LOWER_LIMIT / UPPER_LIMIT / ENABLE_FLAG / UNIT(Spec 단위, 없으면 등록 단위) /
        ///     SPEC_TEXT / UPDATE_USER / UPDATE_TIME (등록 / Spec 중 나중 수정)
        /// </summary>
        DataTable GetEtParameterSetupList(string strFactory, string strDeviceID, string strTestProgram);

        /// <summary>
        /// 헤더를 Parameter 로 등록한다. 서버가 EtParameterNameRule 로 다시 분해 / 변환해 저장한다.
        /// aKey: 0 Factory / 1 Device ID / 2 공정(Test Type) / 3 출처(FILE / SPEC) / 4 등록 파일 이름(선택)
        /// aRows 열: 0 헤더 원문 / 1 단위 / 2 Test No / 3 열 순서 (1 ~ 3 빈 값 허용)
        /// 이미 같은 헤더가 등록돼 있으면 건너뛴다. 분해 실패 / 같은 헤더 두 번 / 다른 헤더가 같은 Parameter 이름 / 단위 다름이 하나라도 있으면 전체 거부.
        /// 반환 1행: REGISTERED / SKIPPED
        /// </summary>
        DataTable RegisterEtParameters(string[] aKey, string[,] aRows, string strUpdateUser);

        /// <summary>
        /// [등록 Parameter] 탭 저장: 화면에서 수정된 행만 받는다. aKey: 0 Factory / 1 Device / 2 공정.
        /// aRows 열: 0 Parameter(저장 이름) / 1 사용(Y/N) / 2 비고 / 3 GDS Chain(빈 값 = 없음) / 4 Lower / 5 Upper(빈 값 = 없음) / 6 Enable(Y/N)
        /// Lower / Upper 가 모두 빈 행은 Spec 을 지운다. 등록되지 않은 Parameter / 같은 행 두 번 / 숫자 아님 / Lower &gt; Upper / Enable 인데 Limit 없음이
        /// 하나라도 있으면 아무것도 저장하지 않는다. Spec 은 값이 바뀐 행만 다시 쓴다. 반환 1행: UPDATED / SPEC_SAVED(새로 만들거나 바뀐 Spec) / SPEC_DELETED
        /// </summary>
        DataTable SaveEtParameterSetup(string[] aKey, string[,] aRows, string strUpdateUser);

        /// <summary>
        /// Parameter 삭제. aKey: 0 Factory / 1 Device / 2 공정. 측정값이 저장된 Parameter 는 거부한다(사용 안 함으로 바꾼다).
        /// 같은 키의 Spec 도 함께 지운다. 반환: 지운 Parameter 수
        /// </summary>
        int DeleteEtParameters(string[] aKey, string[] aParameters, string strUpdateUser);

        #endregion

        #region [GDS Chain 참고 목록]

        /// <summary>E-Test Device 의 GDS Device: 이름이 같은(대소문자 무시) READY Map, 없으면 E-Test Device 이름 그대로.</summary>
        string GetEtGdsDevice(string strFactory, string strDeviceID);

        /// <summary>GDS Device 의 Setup Chain 목록(ACTIVE + 현재 READY 버전). Para 설정 [등록 Parameter] 탭 오른쪽 목록(이름 복사용). 열: CHAIN_SEQ / CHAIN_CODE / CHAIN_NAME</summary>
        DataTable GetEtGdsChainList(string strFactory, string strGdsDeviceID);

        #endregion
    }
}
