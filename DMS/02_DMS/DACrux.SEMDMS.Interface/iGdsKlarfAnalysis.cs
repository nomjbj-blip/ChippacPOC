using System;
using System.Data;

namespace DACrux.SEMDMS.Interface
{
    /// <summary>
    /// KLARF Wafer 1장(STEP_SEQ)의 Defect를 GDS Chain 영역과 비교하는 읽기 전용 원격 호출 계약이다.
    /// 설계: 문서/설계/05_연관분석/2026-10-12_GDS_KLARF_연관분석_화면_설계.md 6 / 7절
    /// 결과는 저장하지 않는다(결정 A7). 화면 A(연관 분석)가 결과 DataSet을 화면 B(Die 상세)에 그대로 넘겨
    /// 두 화면이 같은 기준의 숫자를 보여 준다.
    /// </summary>
    public interface iGdsKlarfAnalysis
    {
        /// <summary>
        /// KLARF Wafer 1장을 GDS Chain Member 도형 영역과 비교한다.
        /// strDeviceID: GDS Map Device. 빈 값이면 KLARF PRODUCT와 같은 Device를 쓴다.
        /// strViewSide: "BACK" / "FRONT". GDS를 이 면 기준으로 맞춘다(KLARF 좌표는 그대로). 저장 기준과 다르면 GDS X를 반전한다.
        /// dToleranceUm: Defect 사각형을 상하좌우로 넓히는 값(µm). bUseDefectSize: false면 Defect 중심점만 쓴다.
        /// dOffsetXUm / dOffsetYUm: 좌표 보정(µm, 보기 기준 좌표에 더한다).
        /// aChainSeqs: 비교할 Chain 번호(null / 빈 배열 = 저장 완료 READY 전체). aClassNumbers: Defect Class(null / 빈 배열 = 전체).
        /// 반환 DataSet 테이블: ANL_INFO(1행, 분석 기준) / CHAIN / DIE / DIE_CHAIN / DEFECT / DEFECT_HIT. 열은 BSL 주석 참고.
        /// </summary>
        DataSet AnalyzeWafer(string strFactory, string strDeviceID, long lStepSeq, string strViewSide,
            double dToleranceUm, bool bUseDefectSize, double dOffsetXUm, double dOffsetYUm,
            string[] aChainSeqs, int[] aClassNumbers);

        /// <summary>
        /// 통합 연관 분석 Wafer 집계 (2026-10-16, 설계 2026-10-15_GDS_KLARF_ETest_통합분석_데이터_설계.md 10.2절).
        /// 실제 모양 거리(Gap) 판정, 여러 KLARF Step, Chain 면 / 검사 면, Defect 영역 방식, 허용오차 표.
        /// 반환 테이블: ANL_INFO / CHAIN / DIE / DIE_CHAIN / DEFECT / DEFECT_CHAIN / TOL_CURVE / ET_LINK. 열은 BSL 주석 참고.
        /// </summary>
        DataSet Analyze(GdsKlarfAnalysisRequest request);

        /// <summary>
        /// Die 1개 상세 (2026-10-16). xIndex / yIndex = Map 을 그린 Step(DisplayStepSeq) 의 Die Index.
        /// focusChainSeq = 역추적 기준 Chain(null = 없음). 그 Die 의 Defect 전체 + Member 단위 DEFECT_HIT 를 함께 돌려준다.
        /// </summary>
        DataSet AnalyzeDie(GdsKlarfAnalysisRequest request, int xIndex, int yIndex, string focusChainSeq);

        /// <summary>
        /// 통합 연관 분석 Ready 조회 (2026-10-16, 설계 16.4절). 고른 KLARF Step 의 Wafer Key(WaferKeyRule)로 같은 Wafer 의
        /// KLARF Step / E-Test 파일을 모으고 GDS 상태 / 점검을 돌려준다(목록만). strDeviceID 가 비면 Product 와 같은 이름의 READY Device.
        /// 반환 테이블: WAFER_KEY / READY_GDS / READY_KLARF / READY_ET / READY_CHECK (모든 열 문자열).
        /// </summary>
        DataSet GetWaferReady(string strFactory, long lStepSeq, string strDeviceID);
    }
}
