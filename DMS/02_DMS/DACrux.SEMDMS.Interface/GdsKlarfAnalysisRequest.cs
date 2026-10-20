using System;

namespace DACrux.SEMDMS.Interface
{
    /// <summary>
    /// GDS × KLARF × E-Test 통합 연관 분석 요청 (2026-10-16). 인자가 많아 한 객체로 묶었다. 서버와 화면이 같이 쓴다.
    /// 설계: 문서/설계/05_연관분석/2026-10-15_GDS_KLARF_ETest_통합분석_데이터_설계.md 10.1 / 16.7 / 18 / 19절
    /// </summary>
    [Serializable]
    public class GdsKlarfAnalysisRequest
    {
        /// <summary>Defect 영역: Defect 크기(XSIZE x YSIZE) + 허용오차. 걸림 = Gap ≤ 허용오차.</summary>
        public const string AreaSizeTolerance = "SIZE_TOL";
        /// <summary>Defect 영역: 중심에서 한 변 SquareUm 정사각형(크기 / 허용오차 안 씀). 걸림 = Gap 0.</summary>
        public const string AreaSquare = "SQUARE";
        public const string ClassBasisRoughBin = "ROUGHBIN";
        public const string ClassBasisClass = "CLASS";
        public const string SideFront = "FRONT";
        public const string SideBack = "BACK";

        public string Factory;
        /// <summary>GDS Map Device. 비면 Map 을 그린 Step 의 KLARF Product 와 이름이 같은 READY Device.</summary>
        public string DeviceId;
        /// <summary>Ready 단계에서 계산한 Wafer Key (기록용, 결과 ANL_INFO 에 남긴다).</summary>
        public string WaferKey;
        /// <summary>함께 분석할 KLARF Step ([준비] 탭에서 체크한 것).</summary>
        public long[] StepSeqs;
        /// <summary>StepSeqs 와 같은 순서의 KLARF 검사 면 "BACK" / "FRONT". null 이면 모두 FRONT(2026-10-20 변경, 화면은 FS 고정). Defect 반전에만 쓰고 Die 좌표는 그대로.</summary>
        public string[] ViewSides;
        /// <summary>GDS Chain 이 그려진 면 "FRONT" / "BACK" (분석에 1개, 비면 BACK = 2026-10-20 변경). Defect 반전 = Chain 면 ≠ 검사 면.</summary>
        public string ChainSide;
        /// <summary>화면 Wafer Map 을 그린 Step. 결과 Die Index / GDS 보기 좌표는 이 Step 기준이다. 0 이면 StepSeqs[0].</summary>
        public long DisplayStepSeq;
        /// <summary>AreaSizeTolerance(기본) / AreaSquare.</summary>
        public string AreaMode;
        public double ToleranceUm;
        public double SquareUm;
        /// <summary>한 변 1,000 µm 이상 Defect 를 판정에서 뺀다(기본 false = 판정하고 LARGE 표시만).</summary>
        public bool ExcludeLargeDefect;
        public double OffsetXUm;
        public double OffsetYUm;
        /// <summary>비교할 Chain 번호. null / 빈 배열 = READY 전체.</summary>
        public string[] ChainSeqs;
        /// <summary>ClassValues 를 어느 값으로 거를지: ClassBasisRoughBin(기본) / ClassBasisClass.</summary>
        public string ClassBasis;
        /// <summary>남길 분류 값. null / 빈 배열 = 전체.</summary>
        public int[] ClassValues;
        /// <summary>E-Test 파일(TQP_GDS_SRC_FILE.SRC_FILE_SEQ). 0 = E-Test 없이.</summary>
        public long EtSrcFileSeq;
    }
}
