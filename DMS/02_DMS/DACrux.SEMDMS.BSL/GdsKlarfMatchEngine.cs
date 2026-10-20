using System;
using System.Collections.Generic;

namespace DACrux.SEMDMS.BSL
{
    /// <summary>
    /// GDS Chain Member 도형 1개의 판정용 영역(저장 기준 좌표, µm). TQP_GDS_PLACED_EL.MIN_X ~ MAX_Y 그대로다.
    /// 설계: 문서/설계/05_연관분석/2026-10-12_GDS_KLARF_연관분석_화면_설계.md 6.3절
    /// </summary>
    public sealed class GdsKlarfMemberBox
    {
        public long ChainSeq;
        public string PlacedElementId;
        public int LayerId;
        /// <summary>GDS DataType(TEXT 는 TEXTTYPE). Layer 는 번호 + DataType 단위다 (2026-10-20).</summary>
        public int DataType;
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
        /// <summary>
        /// 0보다 크면 Defect 중심 사각형 판정: 한 변 = SquareSide(µm). Defect 크기와 허용오차는 쓰지 않고 Gap 0 이면 걸림.
        /// MatchShapes 만 쓴다 (2026-10-16)
        /// </summary>
        public double SquareSide;
        /// <summary>Gap 을 구해 두는 최대 거리(µm, 허용오차 표 최대 단계). 실제 탐색 거리 = max(허용오차, 이 값). MatchShapes 만 쓴다 (2026-10-16)</summary>
        public double SearchDistance = 50;

        /// <summary>걸림 기준 Gap(µm): 중심 사각형이면 0, 아니면 허용오차 (2026-10-16)</summary>
        public double HitGap
        {
            get { return SquareSide > 0 ? 0 : Tolerance; }
        }
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

    /// <summary>Defect 와 Member 모양 1개 사이 거리 (2026-10-16)</summary>
    public sealed class GdsKlarfShapeGap
    {
        /// <summary>GdsKlarfShapeIndex.Shapes 위치.</summary>
        public int ShapeIndex;
        /// <summary>Defect 사각형과 모양 사이 거리(µm). 겹치면 0.</summary>
        public double Gap;
    }

    /// <summary>Defect 1개의 실제 모양 판정 결과 (2026-10-16)</summary>
    public sealed class GdsKlarfDefectGap
    {
        public GdsKlarfDefectPoint Defect;
        /// <summary>보기 기준 GDS 좌표(µm). 화면 표시용.</summary>
        public double ViewX;
        public double ViewY;
        /// <summary>저장 기준 Defect 사각형(µm). 허용오차는 넣지 않은 값이다.</summary>
        public double MinX, MinY, MaxX, MaxY;
        /// <summary>탐색 거리 안의 Member. Gap 오름차순(같으면 Shapes 순서).</summary>
        public List<GdsKlarfShapeGap> Near = new List<GdsKlarfShapeGap>();

        /// <summary>가장 가까운 Member 의 Gap. 탐색 거리 안에 없으면 PositiveInfinity.</summary>
        public double MinGap
        {
            get { return Near.Count == 0 ? double.PositiveInfinity : Near[0].Gap; }
        }
    }

    /// <summary>
    /// KLARF Defect와 GDS Chain Member 영역의 겹침을 계산한다. DB / 화면과 분리해 Probe가 합성 데이터로 검사할 수 있게 했다.
    /// 설계: 문서/설계/05_연관분석/2026-10-12_GDS_KLARF_연관분석_화면_설계.md 6.1 / 6.3절
    ///
    /// 좌표 변환(1차 가정, 결정 A5): GDS 원점 = Chip 중심, KLARF XREL / YREL = Die Pitch 칸 좌하단 기준.
    ///   보기 기준 X = XREL - PitchX / 2 + OffsetX,  Y = YREL - PitchY / 2 + OffsetY
    ///   저장 기준 X = Mirror ? -보기 기준 X : 보기 기준 X  (Y는 그대로, GdsSideTransform과 같은 규칙)
    /// Member는 수만 개라 반전하지 않고, 수가 적은 Defect를 저장 기준으로 바꿔 비교한다.
    ///
    /// 겹침: Defect 사각형 ± Tolerance 와 Member Bounds가 경계 포함 겹치면 "걸림"(1차 Bounds 판정, 정밀 Polygon은 2차).
    /// 처리 흐름: 1) Member Bounds로 균일 격자 색인 2) Defect마다 사각형이 걸치는 칸의 Member만 검사 3) 같은 Member 중복 제거
    ///
    /// 실제 모양 판정 MatchShapes (2026-10-16, 설계 2026-10-15_GDS_KLARF_ETest_통합분석_데이터_설계.md 4.2 / 4.3 / 5.5절)
    ///  - Member = GdsKlarfShape(BOUNDARY 다각형 / PATH 폭 + 끝 처리 + 꺾임). 색인은 Cache 가 한 번 만든 GdsKlarfShapeIndex 를 쓴다.
    ///  - Defect 마다 탐색 거리 S = max(허용오차, SearchDistance) 안의 Member 와 Gap 을 모두 남긴다. 허용오차 표는 다시 계산하지 않고 이 Gap 으로 센다.
    ///  - 걸림 = Gap ≤ HitGap. 허용오차는 Defect 사각형을 둥글게 넓힌 것과 같다(Match 의 네모 넓힘과 모서리 방향만 다름).
    /// </summary>
    public static class GdsKlarfMatchEngine
    {
        /// <summary>한 Member가 차지할 수 있는 최대 칸 수. 더 큰 도형(긴 배선 / 큰 Pad)은 별도 목록에 두고 모든 Defect가 검사한다.</summary>
        internal const long MaxCellsPerBox = 4096;

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

        /// <summary>
        /// 실제 모양 판정 (2026-10-16). 반환 목록은 입력 Defect 와 같은 순서 / 같은 개수다.
        /// chainFilter 가 있으면 그 Chain 의 Member 만 본다(null 이면 색인 전체).
        /// 처리: Defect 사각형 ± S 가 걸치는 칸의 Member 만 보고, Bounds 거리가 S 를 넘으면 모양 계산 없이 건너뛴다.
        /// </summary>
        public static List<GdsKlarfDefectGap> MatchShapes(GdsKlarfShapeIndex index, IList<GdsKlarfDefectPoint> defects,
            GdsKlarfMatchOptions options, HashSet<long> chainFilter)
        {
            if (index == null) throw new ArgumentNullException("index");
            if (defects == null) throw new ArgumentNullException("defects");
            if (options == null) throw new ArgumentNullException("options");
            if (options.Tolerance < 0) throw new ArgumentException("허용오차는 0 이상이어야 합니다.", "options");
            if (options.SquareSide < 0) throw new ArgumentException("Defect 사각형 한 변은 0 이상이어야 합니다.", "options");

            IList<GdsKlarfShape> shapes = index.Shapes;
            double cell = index.CellSize;
            double search = Math.Max(options.HitGap, Math.Max(0, options.SearchDistance));
            bool[] allowed = null;
            if (chainFilter != null)
            {
                allowed = new bool[shapes.Count];
                for (int i = 0; i < shapes.Count; i++) allowed[i] = chainFilter.Contains(shapes[i].ChainSeq);
            }
            var stamp = new int[shapes.Count];
            for (int i = 0; i < stamp.Length; i++) stamp[i] = -1;

            var result = new List<GdsKlarfDefectGap>(defects.Count);
            for (int d = 0; d < defects.Count; d++)
            {
                GdsKlarfDefectPoint defect = defects[d];
                var match = new GdsKlarfDefectGap
                {
                    Defect = defect,
                    ViewX = ToViewX(defect, options),
                    ViewY = ToViewY(defect, options)
                };
                double storedX = options.Mirror ? -match.ViewX : match.ViewX;
                double halfW, halfH;
                if (options.SquareSide > 0) halfW = halfH = options.SquareSide / 2.0;
                else
                {
                    halfW = options.UseDefectSize ? Math.Abs(defect.XSize) / 2.0 : 0;
                    halfH = options.UseDefectSize ? Math.Abs(defect.YSize) / 2.0 : 0;
                }
                match.MinX = storedX - halfW;
                match.MaxX = storedX + halfW;
                match.MinY = match.ViewY - halfH;
                match.MaxY = match.ViewY + halfH;

                long x1 = CellIndex(match.MinX - search, cell), x2 = CellIndex(match.MaxX + search, cell);
                long y1 = CellIndex(match.MinY - search, cell), y2 = CellIndex(match.MaxY + search, cell);
                for (long cx = x1; cx <= x2; cx++)
                    for (long cy = y1; cy <= y2; cy++)
                    {
                        int[] list;
                        if (!index.Grid.TryGetValue(CellKey(cx, cy), out list)) continue;
                        foreach (int i in list)
                            TestShape(shapes, allowed, i, d, stamp, match, search);
                    }
                foreach (int i in index.Large)
                    TestShape(shapes, allowed, i, d, stamp, match, search);

                match.Near.Sort((a, b) => a.Gap != b.Gap ? a.Gap.CompareTo(b.Gap) : a.ShapeIndex.CompareTo(b.ShapeIndex));
                result.Add(match);
            }
            return result;
        }

        /// <summary>이번 Defect 에서 처음 만난 Member 면 Bounds 거리 -> 모양 Gap 순서로 검사해 탐색 거리 안이면 넣는다.</summary>
        private static void TestShape(IList<GdsKlarfShape> shapes, bool[] allowed, int index, int defectNo, int[] stamp,
            GdsKlarfDefectGap match, double search)
        {
            if (stamp[index] == defectNo) return;
            stamp[index] = defectNo;
            if (allowed != null && !allowed[index]) return;
            GdsKlarfShape s = shapes[index];
            double dx = Math.Max(0, Math.Max(s.MinX - match.MaxX, match.MinX - s.MaxX));
            double dy = Math.Max(0, Math.Max(s.MinY - match.MaxY, match.MinY - s.MaxY));
            if (dx * dx + dy * dy > search * search) return;   // 모양은 Bounds 안에 있으므로 Gap ≥ Bounds 거리
            double gap = GdsKlarfShapeGeometry.Gap(s, match.MinX, match.MinY, match.MaxX, match.MaxY);
            if (gap <= search) match.Near.Add(new GdsKlarfShapeGap { ShapeIndex = index, Gap = gap });
        }

        /// <summary>Defect 1개의 Chain 별 가장 가까운 Member (Near 가 Gap 오름차순이라 Chain 마다 처음 만난 것). 2026-10-16</summary>
        public static Dictionary<long, GdsKlarfShapeGap> NearestByChain(GdsKlarfDefectGap match, IList<GdsKlarfShape> shapes)
        {
            var result = new Dictionary<long, GdsKlarfShapeGap>();
            foreach (GdsKlarfShapeGap g in match.Near)
            {
                long chain = shapes[g.ShapeIndex].ChainSeq;
                if (!result.ContainsKey(chain)) result.Add(chain, g);
            }
            return result;
        }

        internal static long CellIndex(double value, double cell)
        {
            return (long)Math.Floor(value / cell);
        }

        /// <summary>칸 번호 두 개를 Dictionary 키 하나로 합친다(칸 번호는 ±20억 안).</summary>
        internal static long CellKey(long cx, long cy)
        {
            return (cx << 32) ^ (cy & 0xFFFFFFFFL);
        }
    }
}
