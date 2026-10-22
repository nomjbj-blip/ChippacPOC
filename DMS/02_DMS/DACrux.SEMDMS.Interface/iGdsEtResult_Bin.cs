using System;
using System.Data;

namespace DACrux.SEMDMS.Interface
{
    /// <summary>
    /// E-Test Bin 설정 원격 호출 계약 (2026-10-22).
    /// E-Test Device + 공정의 YMS Program(Device_공정, EtYmsRule.ToProgramName) Bin 정의(TQP_BINDESC)를 이름 / 양품 / 색 / 설명만 고친다.
    /// 업로드 파일 Die 의 Hardware Bin 분포를 같이 돌려줘 양품을 정하는 근거로 보인다.
    /// 설계: 문서/설계/04_ETest/2026-10-22_ETest_Bin_Setup_화면_설계.md
    /// </summary>
    public partial interface iGdsEtResult
    {
        #region [Bin 설정 (TQP_BINDESC)]

        /// <summary>Factory 의 READY 파일이 있는 Device + 공정. 열: DEVICE_ID / TEST_PROGRAM / FILE_COUNT / YMS_FILE_COUNT(YMS 반영 DONE) / YMS_PROGRAM</summary>
        DataTable GetEtBinProgramList(string strFactory);

        /// <summary>
        /// Bin 정의 + 파일 Bin 분포(Bin 별 한 행, Bin 순). aKey: 0 Factory / 1 Device / 2 공정.
        /// 열: BIN(Hardware Bin, 파일 Die 에 Bin 이 없으면 빈 값) / BIN_NAME / HIGH_GEC(Y / N) / COLOR / DESCRIPTION / HAS_DEF(정의 있음 Y / N) /
        ///     SW_BINS(Software Bin, ',' 로 이음) / DIE_COUNT / PASS_COUNT / FAIL_COUNT(파일 DUT 결과) / FILE_COUNT / IN_RANGE(0 ~ 99 이면 Y) / YMS_PROGRAM
        /// </summary>
        DataTable GetEtBinList(string[] aKey);

        /// <summary>
        /// Bin 정의 저장(바뀐 행 / 새 행만). aKey: 0 Factory / 1 Device / 2 공정.
        /// aRows 열: 0 BIN(0 ~ 99) / 1 이름(30바이트) / 2 양품(Y / N) / 3 색(빈 값 또는 #RRGGBB) / 4 설명(100바이트).
        /// 틀린 행이 하나라도 있으면 아무것도 저장하지 않는다. Map 기호 / 표시 / Limit 수는 바꾸지 않는다(새 정의는 표시 Y).
        /// 반환 1행: SAVED_COUNT / NEW_COUNT / GOOD_CHANGED(Y / N) / YMS_FILE_COUNT
        /// </summary>
        DataTable SaveEtBins(string[] aKey, string[,] aRows, string strUpdateUser);

        /// <summary>Bin 정의 삭제. aKey: 0 Factory / 1 Device / 2 공정. 정의가 없는 Bin 이 있으면 아무것도 지우지 않는다. 반환: 지운 정의 수</summary>
        int DeleteEtBins(string[] aKey, string[] aBins, string strUpdateUser);

        #endregion
    }
}
