using System;
using System.Collections.Generic;

namespace DACrux.SEMDMS.BSL
{
    /// <summary>
    /// GDS Chain Member 도형 1개의 판정용 영역(저장 기준 좌표, µm). TQP_GDS_PLACED_EL.MIN_X ~ MAX_Y 그대로다.
    /// 설계: 문서/2026-10-12_GDS_KLARF_연관분석_화면_설계.md 6.3절
    /// </summary>
    public sealed class GdsKlarfMemberBox
    {
        public long ChainSeq;
        public string PlacedElementId;
        public int LayerId;
        public string RoleCode;
        public double MinX;
        public double MinY;
        public double MaxX;
        public double MaxY;
    }

    /// <summary>KLARF Defect 1개. XREL / YREL / XSIZE / YSIZE는 µm, Die = (XINDEX, YINDEX) (TQD_DEFECT 그대로).</summary>
    public sealed class GdsKlarfDefectPoint
    {
        public long DefectId;
        public int XIndex;
        public int YIndex;
        public double XRel;
        public double YRel;
        public double XSize;
        public double YSize;
        public int ClassNumber;
    }

    /// <summary>판정 기준. 화면 조건과 KLARF Die Pitch로 만든다.</summary>
    public sealed class GdsKlarfMatchOptions
    {
        /// <summary>KLARF Die Pitch(µm). Die 안 좌표를 Chip 중심 원점으로 옮길 때 절반을 뺀다.</summary>
        public double PitchX;
        public double PitchY;
        /// <summary>좌표 보정(µm). 보기 기준 좌표에 더한다(결정 A5, 기본 0).</summary>
        public double OffsetX;
        public double OffsetY;
        /// <summary>Defect 사각형을 상하좌우로 넓히는 값(µm, 결정 A3 기본 5).</summary>
        public double Tolerance;
        /// <summary>true면 XSIZE x YSIZE 사각형, false면 중심점만(결정 A4 기본 true).</summary>
        public bool UseDefectSize = true;
        /// <summary>저장 기준(TQP_GDS_MAP.VIEW_SIDE)과 보기 기준이 다르면 true. GDS 대신 Defect X를 반전해 비교한다.</summary>
        public bool Mirror;
        /// <summary>격자 색인 칸 크기(µm). Defect마다 주변 칸의 Member만 검사하기 위한 값이다.</summary>
        public double CellSize = 200;
    }

    /// <summary>Defect 1개의 판정 결과.</summary>
    public sealed class GdsKlarfDefectMatch
    {
        public GdsKlarfDefectPoint Defect;
        /// <summary>보기 기준 GDS 좌표(µm). 화면 표시용.</summary>
        public double ViewX;
        public double ViewY;
        /// <summary>겹친 Member의 목록 위치(입력 목록 순서). 없으면 0개.</summary>
        public List<int> MemberIndexes = new List<int>();
    }

    /// <summary>
    /// KLARF Defect와 GDS Chain Member 영역의 겹침을 계산한다. DB / 화면과 분리해 Probe가 합성 데이터로 검사할 수 있게 했다.
    /// 설계: 문서/2026-10-12_GDS_KLARF_연관분석_화면_설계.md 6.1 / 6.3절
    ///
    /// 좌표 변환(1차 가정, 결정 A5): GDS 원점 = Chip 중심, KLARF XREL / YREL = Die Pitch 칸 좌하단 기준.
    ///   보기 기준 X = XREL - PitchX / 2 + OffsetX,  Y = YREL - PitchY / 2 + OffsetY
    ///   저장 기준 X = Mirror ? -보기 기준 X : 보기 기준 X  (Y는 그대로, GdsSideTransform과 같은 규칙)
    /// Member는 수만 개라 반전하지 않고, 수가 적은 Defect를 저장 기준으로 바꿔 비교한다.
    ///
    /// 겹침: Defect 사각형 ± Tolerance 와 Member Bounds가 경계 포함 겹치면 "걸림"(1차 Bounds 판정, 정밀 Polygon은 2차).
    /// 처리 흐름: 1) Member Bounds로 균일 격자 색인 2) Defect마다 사각형이 걸치는 칸의 Member만 검사 3) 같은 Member 중복 제거
    /// </summary>
    public static class GdsKlarfMatchEngine
    {
        /// <summary>한 Member가 차지할 수 있는 최대 칸 수. 더 큰 도형(긴 배선 / 큰 Pad)은 별도 목록에 두고 모든 Defect가 검사한다.</summary>
        private const long MaxCellsPerBox = 4096;

        /// <summary>보기 기준 X(µm).</summary>
        public static double ToViewX(GdsKlarfDefectPoint defect, GdsKlarfMatchOptions options)
        {
            return defect.XRel - options.PitchX / 2.0 + options.OffsetX;
        }

        /// <summary>보기 기준 Y(µm).</summary>
        public static double ToViewY(GdsKlarfDefectPoint defect, GdsKlarfMatchOptions options)
        {
            return defect.YRel - options.PitchY / 2.0 + options.OffsetY;
        }

        /// <summary>
        /// Defect 전체를 판정한다. 반환 목록은 입력 Defect와 같은 순서 / 같은 개수다.
        /// 처리: 격자 색인을 한 번 만들고, Defect마다 주변 칸 후보만 사각형 겹침 검사를 한다.
        /// </summary>
        public static List<GdsKlarfDefectMatch> Match(IList<GdsKlarfMemberBox> members, IList<GdsKlarfDefectPoint> defects,
            GdsKlarfMatchOptions options)
        {
            if (members == null) throw new ArgumentNullException("members");
            if (defects == null) throw new ArgumentNullException("defects");
            if (options == null) throw new ArgumentNullException("options");
            if (!(options.CellSize > 0)) throw new ArgumentException("격자 칸 크기는 0보다 커야 합니다.", "options");
            if (options.Tolerance < 0) throw new ArgumentException("허용오차는 0 이상이어야 합니다.", "options");

            double cell = options.CellSize;
            var grid = new Dictionary<long, List<int>>();
            var large = new List<int>();
            for (int i = 0; i < members.Count; i++)
            {
                GdsKlarfMemberBox m = members[i];
                long x1 = CellIndex(m.MinX, cell), x2 = CellIndex(m.MaxX, cell);
                long y1 = CellIndex(m.MinY, cell), y2 = CellIndex(m.MaxY, cell);
                if ((x2 - x1 + 1) * (y2 - y1 + 1) > MaxCellsPerBox)
                {
                    large.Add(i);
                    continue;
                }
                for (long cx = x1; cx <= x2; cx++)
                    for (long cy = y1; cy <= y2; cy++)
                    {
                        long key = CellKey(cx, cy);
                        List<int> list;
                        if (!grid.TryGetValue(key, out list))
                        {
                            list = new List<int>();
                            grid.Add(key, list);
                        }
                        list.Add(i);
                    }
            }

            // Defect마다 같은 Member를 여러 칸에서 다시 만나므로 마지막으로 검사한 Defect 번호로 중복을 거른다.
            var stamp = new int[members.Count];
            for (int i = 0; i < stamp.Length; i++) stamp[i] = -1;

            var result = new List<GdsKlarfDefectMatch>(defects.Count);
            for (int d = 0; d < defects.Count; d++)
            {
                GdsKlarfDefectPoint defect = defects[d];
                var match = new GdsKlarfDefectMatch
                {
                    Defect = defect,
                    ViewX = ToViewX(defect, options),
                    ViewY = ToViewY(defect, options)
                };
                double storedX = options.Mirror ? -match.ViewX : match.ViewX;
                double halfW = (options.UseDefectSize ? Math.Abs(defect.XSize) / 2.0 : 0) + options.Tolerance;
                double halfH = (options.UseDefectSize ? Math.Abs(defect.YSize) / 2.0 : 0) + options.Tolerance;
                double minX = storedX - halfW, maxX = storedX + halfW;
                double minY = match.ViewY - halfH, maxY = match.ViewY + halfH;

                long x1 = CellIndex(minX, cell), x2 = CellIndex(maxX, cell);
                long y1 = CellIndex(minY, cell), y2 = CellIndex(maxY, cell);
                for (long cx = x1; cx <= x2; cx++)
                    for (long cy = y1; cy <= y2; cy++)
                    {
                        List<int> list;
                        if (!grid.TryGetValue(CellKey(cx, cy), out list)) continue;
                        foreach (int i in list)
                            TestMember(members, i, d, stamp, minX, minY, maxX, maxY, match.MemberIndexes);
                    }
                foreach (int i in large)
                    TestMember(members, i, d, stamp, minX, minY, maxX, maxY, match.MemberIndexes);

                match.MemberIndexes.Sort();
                result.Add(match);
            }
            return result;
        }

        /// <summary>이번 Defect에서 처음 만난 Member면 사각형 겹침(경계 포함)을 검사해 결과에 넣는다.</summary>
        private static void TestMember(IList<GdsKlarfMemberBox> members, int index, int defectNo, int[] stamp,
            double minX, double minY, double maxX, double maxY, List<int> hits)
        {
            if (stamp[index] == defectNo) return;
            stamp[index] = defectNo;
            GdsKlarfMemberBox m = members[index];
            if (m.MinX <= maxX && m.MaxX >= minX && m.MinY <= maxY && m.MaxY >= minY)
                hits.Add(index);
        }

        private static long CellIndex(double value, double cell)
        {
            return (long)Math.Floor(value / cell);
        }

        /// <summary>칸 번호 두 개를 Dictionary 키 하나로 합친다(칸 번호는 ±20억 안).</summary>
        private static long CellKey(long cx, long cy)
        {
            return (cx << 32) ^ (cy & 0xFFFFFFFFL);
        }
    }
}
