// 연관 분석 1단계 확인: 실제 모양 Gap / 모양 판정 엔진 / 서버 Cache
// 설계: 문서/설계/05_연관분석/2026-10-15_GDS_KLARF_ETest_통합분석_데이터_설계.md 4.2 / 4.3 / 4.5 / 5.5절
// 빌드 / 실행은 GdsKlarfShapeProbe.ps1 이 한다(BSL TEMP 빌드 DLL 참조). DB 는 조회만 한다.
//  A. 도형 거리: 손으로 계산한 값과 비교 (BOUNDARY 볼록 / 오목 / 원형 Pad, PATH Type 0 / 1 / 2 / 4, 꺾임 Miter / 급꺾임, 폭 0, 점 1개)
//  B. 판정 엔진 MatchShapes: 좌표 변환 / 반전 / 허용오차(둥근 넓힘) / 크기 / 중심 사각형 / Chain 거르기 / 무작위 전수 비교
//  C. Cache: 합성 표로 모양 만들기 / 좌표 바이트 검사 / Stamp / LRU 3 / 같은 키 동시 호출 1회 읽기
//  D. 실제 DB: Device 전체 모양 읽기 시간, DB Bounds 와 비교, 실제 KLARF Step 으로 Bounds 판정 vs 모양 판정 수
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using DACrux.SEMDMS.BSL;

internal static class GdsKlarfShapeProbe
{
    private static int _pass, _fail;

    private static int Main(string[] args)
    {
        string factory = args.Length > 0 ? args[0] : "SCK";
        string device = args.Length > 1 ? args[1] : "OMM_Hybrid";
        long stepSeq = args.Length > 2 ? Int64.Parse(args[2], CultureInfo.InvariantCulture) : 528;
        try
        {
            GeometryChecks();
            EngineChecks();
            CacheChecks();
            DbChecks(factory, device, stepSeq);
        }
        catch (Exception ex)
        {
            Check("예외 없이 끝까지", false, ex.ToString());
        }
        Console.WriteLine();
        Console.WriteLine("GDS_KLARF_SHAPE_PROBE=" + (_fail == 0 ? "PASS" : "FAIL") + " (" + _pass + "/" + (_pass + _fail) + ")");
        return _fail == 0 ? 0 : 1;
    }

    // ===== A. 도형 거리 (손 계산) =====
    private static void GeometryChecks()
    {
        GdsKlarfShape sq = Shape("BOUNDARY", 0, 0, 0, 0, 10, 0, 10, 10, 0, 10, 0, 0);
        Check("A1 BOUNDARY 정사각형: 닫는 점 제거 / 바깥 점 5 / 안쪽 사각형 0 / 감싸는 사각형 0 / 사각형-사각형 (3,4)->5 / 가로지름 0",
            sq.Pieces.Count == 1 && sq.Pieces[0][0].Length == 4 && Eq(At(sq, 15, 5), 5) && Eq(Gap(sq, 2, 2, 3, 3), 0) && Eq(Gap(sq, -5, -5, 20, 20), 0)
            && Eq(Gap(sq, 13, 14, 20, 20), 5) && Eq(Gap(sq, -2, 4, 12, 6), 0) && Bounds(sq, 0, 0, 10, 10),
            "점 " + sq.Pieces[0][0].Length + " / " + F(At(sq, 15, 5)) + " / " + F(Gap(sq, 13, 14, 20, 20)));

        GdsKlarfShape ell = Shape("BOUNDARY", 0, 0, 0, 0, 20, 0, 20, 5, 5, 5, 5, 20, 0, 20, 0, 0);
        Check("A2 오목 L 자: 오목한 곳 (10,10) -> 5 (안쪽 아님) / 안쪽 (3,3) -> 0", Eq(At(ell, 10, 10), 5) && Eq(At(ell, 3, 3), 0),
            F(At(ell, 10, 10)) + " / " + F(At(ell, 3, 3)));

        var circle = new List<double>();
        for (int k = 0; k <= 64; k++)
        {
            double a = 2 * Math.PI * (k % 64) / 64;
            circle.Add(10 * Math.Cos(a));
            circle.Add(10 * Math.Sin(a));
        }
        GdsKlarfShape pad = Shape("BOUNDARY", 0, 0, circle.ToArray());
        Check("A3 원형 Pad(65점, 반지름 10): (20,0) -> 10 / (0,25) -> 15 / 중심 0", Eq(At(pad, 20, 0), 10) && Eq(At(pad, 0, 25), 15) && Eq(At(pad, 0, 0), 0),
            F(At(pad, 20, 0)) + " / " + F(At(pad, 0, 25)));

        GdsKlarfShape p0 = Shape("PATH", 10, 0, 0, 0, 100, 0);
        Check("A4 PATH Type 0 폭 10: 끝 밖 (105,0) -> 5 / 옆 (50,8) -> 3 / 시작 밖 (-3,0) -> 3 / Bounds 끝 연장 없음",
            Eq(At(p0, 105, 0), 5) && Eq(At(p0, 50, 8), 3) && Eq(At(p0, -3, 0), 3) && Eq(At(p0, 50, 0), 0) && Bounds(p0, 0, -5, 100, 5),
            F(At(p0, 105, 0)) + " / " + F(At(p0, 50, 8)) + " / " + F(At(p0, -3, 0)) + " / X " + F(p0.MinX) + "~" + F(p0.MaxX));

        GdsKlarfShape p2 = Shape("PATH", 10, 2, 0, 0, 100, 0);
        GdsKlarfShape p4 = Shape("PATH", 10, 4, 0, 0, 100, 0);
        Check("A5 PATH Type 2 (W/2 연장): (105,0) -> 0 / (108,0) -> 3 / (-8,0) -> 3 / Type 4 = Type 2",
            Eq(At(p2, 105, 0), 0) && Eq(At(p2, 108, 0), 3) && Eq(At(p2, -8, 0), 3) && Bounds(p2, -5, -5, 105, 5) && Eq(At(p4, 108, 0), 3),
            F(At(p2, 108, 0)) + " / " + F(At(p4, 108, 0)));

        GdsKlarfShape p1 = Shape("PATH", 10, 1, 0, 0, 100, 0);
        Check("A6 PATH Type 1 (둥근 끝): (108,0) -> 3 / (104,4) -> √32-5 = 0.656854 (Type 0 은 4)",
            Eq(At(p1, 108, 0), 3) && Eq(At(p1, 104, 4), Math.Sqrt(32) - 5) && Eq(At(p0, 104, 4), 4) && Bounds(p1, -5, -5, 105, 5),
            F(At(p1, 108, 0)) + " / " + F(At(p1, 104, 4)) + " / Type0 " + F(At(p0, 104, 4)));

        GdsKlarfShape left = Shape("PATH", 10, 0, 0, 0, 100, 0, 100, 100);
        GdsKlarfShape right = Shape("PATH", 10, 0, 0, 0, 100, 0, 100, -100);
        Check("A7 직각 꺾임 Miter: 바깥 모서리 (107,-7) -> √8 / 오른쪽 꺾임 (107,7) -> √8 / 안쪽 (90,10) -> 5",
            Eq(At(left, 107, -7), Math.Sqrt(8)) && Eq(At(right, 107, 7), Math.Sqrt(8)) && Eq(At(left, 90, 10), 5) && Eq(left.MaxX, 105) && Eq(left.MinY, -5),
            F(At(left, 107, -7)) + " / " + F(At(right, 107, 7)) + " / " + F(At(left, 90, 10)));

        GdsKlarfShape sharp = Shape("PATH", 10, 0, 0, 0, 100, 0, 0, 10);
        Check("A8 급꺾임(약 174도): Miter 대신 삼각형 -> MaxX 100.4975 / (106,0) -> 5.743483",
            Math.Abs(sharp.MaxX - 100.497519) < 1e-5 && Math.Abs(At(sharp, 106, 0) - 5.743483) < 1e-5,
            "MaxX " + F(sharp.MaxX) + " / " + F(At(sharp, 106, 0)));

        GdsKlarfShape line = Shape("PATH", 0, 0, 0, 0, 10, 0);
        GdsKlarfShape cross = Shape("PATH", 0, 0, -10, 5, 30, 5);
        Check("A9 폭 0 PATH: (5,3) -> 3 / (13,4) -> 5 / 사각형을 가로지르는 선 -> 0",
            Eq(At(line, 5, 3), 3) && Eq(At(line, 13, 4), 5) && Eq(Gap(cross, 0, 0, 10, 10), 0), F(At(line, 13, 4)));

        GdsKlarfShape dot0 = Shape("PATH", 10, 0, 0, 0);
        GdsKlarfShape dot1 = Shape("PATH", 10, 1, 0, 0);
        Check("A10 PATH 점 1개: Type 0 정사각형 (8,0) -> 3, (6,6) -> √2 / Type 1 원 (6,6) -> √72-5",
            Eq(At(dot0, 8, 0), 3) && Eq(At(dot0, 6, 6), Math.Sqrt(2)) && Eq(At(dot1, 6, 6), Math.Sqrt(72) - 5),
            F(At(dot0, 6, 6)) + " / " + F(At(dot1, 6, 6)));

        GdsKlarfShape tri = Shape("BOUNDARY", 0, 0, -10, 4, 30, 4, -10, 6, -10, 4);
        GdsKlarfShape dup = Shape("PATH", 10, 0, 0, 0, 0, 0, 100, 0);
        Check("A11 꼭짓점이 사각형 밖이고 사각형 중심도 밖인데 변이 지나감 -> 0 / 사각형 Defect (101,-1)~(103,1) -> 1 / 연속 중복 점 제거",
            Eq(Gap(tri, 0, -10, 10, 4.5), 0) && Eq(Gap(p0, 101, -1, 103, 1), 1) && Eq(At(dup, 105, 0), 5) && dup.Pieces.Count == 1,
            F(Gap(tri, 0, -10, 10, 4.5)) + " / " + F(Gap(p0, 101, -1, 103, 1)));
    }

    // ===== B. 판정 엔진 =====
    private static void EngineChecks()
    {
        // Pitch 1000 x 2000 -> XREL 500 / YREL 1000 이 Chip 중심(0, 0)
        GdsKlarfShapeIndex center = Index(Shape("BOUNDARY", 0, 0, -1, -1, 1, -1, 1, 1, -1, 1, -1, -1));
        GdsKlarfDefectGap g = One(center, Defect(1, 500, 1000, 0, 0), Opt(0, true, false), null);
        Check("B1 좌표: Pitch 칸 중심 = GDS 원점, Gap 0", Eq(g.ViewX, 0) && Eq(g.ViewY, 0) && Eq(g.MinGap, 0) && g.Near.Count == 1,
            "view=(" + F(g.ViewX) + ", " + F(g.ViewY) + ") gap=" + F(g.MinGap));

        GdsKlarfShapeIndex right = Index(Rect(1, 100, -5, 110, 5));
        GdsKlarfDefectGap gm = One(right, Defect(1, 395, 1000, 0, 0), Opt(0, true, true), null);
        GdsKlarfDefectGap gn = One(right, Defect(1, 395, 1000, 0, 0), Opt(0, true, false), null);
        Check("B2 반전: Mirror 면 저장 기준 +105 -> 0 / 아니면 -105 -> 탐색 거리(50) 밖", Eq(gm.MinGap, 0) && gn.Near.Count == 0 && Eq(gm.ViewX, -105),
            F(gm.MinGap) + " / near " + gn.Near.Count);

        GdsKlarfShapeIndex bar = Index(Rect(1, 10, -5, 20, 5));
        GdsKlarfMatchOptions o5 = Opt(5, true, false), o4 = Opt(4.999, true, false);
        double gap5 = One(bar, Defect(1, 505, 1000, 0, 0), o5, null).MinGap;
        double corner = One(bar, Defect(2, 505, 1010, 0, 0), o5, null).MinGap;
        var oldHit = GdsKlarfMatchEngine.Match(new List<GdsKlarfMemberBox> { Box(1, 10, -5, 20, 5) },
            new List<GdsKlarfDefectPoint> { Defect(2, 505, 1010, 0, 0) }, o5)[0].MemberIndexes.Count;
        Check("B3 허용오차: Gap 5 는 5 에서 걸림 / 4.999 안 걸림 / 모서리 방향 √50 은 둥근 넓힘이라 5 에서 안 걸림(Bounds 네모 넓힘은 걸림)",
            Eq(gap5, 5) && gap5 <= o5.HitGap && gap5 > o4.HitGap && Eq(corner, Math.Sqrt(50)) && corner > o5.HitGap && oldHit == 1,
            F(gap5) + " / corner " + F(corner) + " / Bounds 판정 " + oldHit);

        double sized = One(bar, Defect(1, 505, 1000, 10, 2), Opt(0, true, false), null).MinGap;
        double point = One(bar, Defect(1, 505, 1000, 10, 2), Opt(0, false, false), null).MinGap;
        GdsKlarfMatchOptions sq10 = Opt(5, true, false); sq10.SquareSide = 10;
        GdsKlarfMatchOptions sq6 = Opt(5, true, false); sq6.SquareSide = 6;
        double s10 = One(bar, Defect(1, 505, 1000, 300, 300), sq10, null).MinGap;
        double s6 = One(bar, Defect(1, 505, 1000, 300, 300), sq6, null).MinGap;
        Check("B4 Defect 크기 10 -> Gap 0 / 크기 끔 -> 5 / 중심 사각형 한 변 10 -> 0, 6 -> 2 (크기 300 무시, 걸림 기준 0)",
            Eq(sized, 0) && Eq(point, 5) && Eq(s10, 0) && Eq(s6, 2) && sq6.HitGap == 0,
            F(sized) + " / " + F(point) + " / " + F(s10) + " / " + F(s6));

        var twoChains = new List<GdsKlarfShape> { Rect(1, -2, -2, 2, 2), Rect(1, 3, -1, 4, 1), Rect(2, -1, 4, 1, 6) };
        GdsKlarfShapeIndex idx2 = new GdsKlarfShapeIndex(twoChains, 200, 0, 0);
        GdsKlarfDefectGap all = One(idx2, Defect(1, 500, 1000, 0, 0), Opt(0, true, false), null);
        GdsKlarfDefectGap only2 = One(idx2, Defect(1, 500, 1000, 0, 0), Opt(0, true, false), new HashSet<long> { 2 });
        Dictionary<long, GdsKlarfShapeGap> byChain = GdsKlarfMatchEngine.NearestByChain(all, twoChains);
        Check("B5 Gap 오름차순 / Chain 별 최소 Gap + 가장 가까운 Member / Chain 거르기",
            all.Near.Count == 3 && Eq(all.Near[0].Gap, 0) && Eq(all.Near[1].Gap, 3) && Eq(all.Near[2].Gap, 4)
            && byChain.Count == 2 && byChain[1].ShapeIndex == 0 && Eq(byChain[2].Gap, 4) && only2.Near.Count == 1 && only2.Near[0].ShapeIndex == 2,
            string.Join(",", all.Near.Select(n => n.ShapeIndex + ":" + F(n.Gap)).ToArray()) + " / filter " + only2.Near.Count);

        var bigList = new List<GdsKlarfShape> { Rect(7, -10000, -10000, 10000, 10000), Shape("PATH", 2, 0, -900, 0, 900, 0), Rect(8, 5000, 5000, 5001, 5001) };
        GdsKlarfShapeIndex idxBig = new GdsKlarfShapeIndex(bigList, 200, 0, 0);
        GdsKlarfDefectGap gb = One(idxBig, Defect(1, 500, 1000, 400, 4), Opt(0, true, false), null);
        Check("B6 큰 도형(격자 상한 초과) 따로 검사 / 긴 도형 1회만 / 먼 도형 제외", idxBig.LargeCount == 1 && gb.Near.Count == 2
            && gb.Near.Select(n => n.ShapeIndex).OrderBy(i => i).SequenceEqual(new[] { 0, 1 }), "large " + idxBig.LargeCount + " / near " + gb.Near.Count);

        // 무작위 전수 비교: 격자 색인 + Bounds 건너뛰기 = 모든 Member Gap 계산
        var rnd = new Random(53);
        var shapes = new List<GdsKlarfShape>();
        for (int i = 0; i < 800; i++)
        {
            double x = rnd.NextDouble() * 1000 - 500, y = rnd.NextDouble() * 2000 - 1000;
            long chain = i % 7 + 1;
            if (i % 2 == 0)
            {
                int n = 3 + rnd.Next(6);
                double r = 2 + rnd.NextDouble() * (i % 40 == 0 ? 300 : 15);
                var pts = new List<double>();
                for (int k = 0; k < n; k++)
                {
                    double a = 2 * Math.PI * k / n + rnd.NextDouble() * 0.5, rr = r * (0.4 + rnd.NextDouble() * 0.6);
                    pts.Add(x + rr * Math.Cos(a)); pts.Add(y + rr * Math.Sin(a));
                }
                pts.Add(pts[0]); pts.Add(pts[1]);
                shapes.Add(ShapeOf(chain, "BOUNDARY", 0, 0, pts.ToArray()));
            }
            else
            {
                int n = 1 + rnd.Next(4);
                var pts = new List<double> { x, y };
                for (int k = 1; k < n; k++)
                {
                    x += (rnd.NextDouble() - 0.5) * (i % 30 == 1 ? 900 : 120);
                    y += (rnd.NextDouble() - 0.5) * 120;
                    pts.Add(x); pts.Add(y);
                }
                int[] types = { 0, 1, 2, 4 };
                shapes.Add(ShapeOf(chain, "PATH", i % 25 == 3 ? 0 : 5 + rnd.NextDouble() * 40, types[rnd.Next(4)], pts.ToArray()));
            }
        }
        var index = new GdsKlarfShapeIndex(shapes, 200, 0, 0);
        var defects = new List<GdsKlarfDefectPoint>();
        for (int i = 0; i < 600; i++)
            defects.Add(Defect(i + 1, rnd.NextDouble() * 1000, rnd.NextDouble() * 2000, rnd.NextDouble() * (i % 50 == 0 ? 600 : 30), rnd.NextDouble() * 30));
        var combos = new List<Tuple<GdsKlarfMatchOptions, HashSet<long>, string>>();
        combos.Add(Tuple.Create(Opt(0, true, false), (HashSet<long>)null, "tol0 size"));
        combos.Add(Tuple.Create(Opt(5, true, true), (HashSet<long>)null, "tol5 size mirror"));
        combos.Add(Tuple.Create(Opt(3, false, false), new HashSet<long> { 2, 5 }, "tol3 point chain2,5"));
        combos.Add(Tuple.Create(Opt(80, true, false), (HashSet<long>)null, "tol80(>50) size"));
        GdsKlarfMatchOptions sq = Opt(5, true, true); sq.SquareSide = 10;
        combos.Add(Tuple.Create(sq, new HashSet<long> { 1, 3, 4 }, "square10 mirror chain1,3,4"));
        bool same = true;
        var detail = new List<string>();
        foreach (var c in combos)
        {
            var watch = Stopwatch.StartNew();
            List<GdsKlarfDefectGap> eng = GdsKlarfMatchEngine.MatchShapes(index, defects, c.Item1, c.Item2);
            long ms = watch.ElapsedMilliseconds;
            double search = Math.Max(c.Item1.HitGap, c.Item1.SearchDistance);
            int pairs = 0, diff = 0;
            for (int d = 0; d < defects.Count; d++)
            {
                GdsKlarfDefectGap e = eng[d];
                var brute = new List<string>();
                for (int s = 0; s < shapes.Count; s++)
                {
                    if (c.Item2 != null && !c.Item2.Contains(shapes[s].ChainSeq)) continue;
                    double gap = GdsKlarfShapeGeometry.Gap(shapes[s], e.MinX, e.MinY, e.MaxX, e.MaxY);
                    if (gap <= search) brute.Add(s + ":" + gap.ToString("R", CultureInfo.InvariantCulture));
                }
                var mine = e.Near.Select(n => n.ShapeIndex + ":" + n.Gap.ToString("R", CultureInfo.InvariantCulture)).ToList();
                brute.Sort(StringComparer.Ordinal); mine.Sort(StringComparer.Ordinal);
                if (!brute.SequenceEqual(mine)) diff++;
                for (int k = 1; k < e.Near.Count; k++) if (e.Near[k].Gap < e.Near[k - 1].Gap) diff++;
                pairs += mine.Count;
            }
            if (diff != 0) same = false;
            int hits = eng.Count(e => e.MinGap <= c.Item1.HitGap);
            detail.Add(c.Item3 + " near=" + pairs + " hit=" + hits + " diff=" + diff + " " + ms + "ms");
        }
        Check("B7 무작위 800 Member(다각형 / PATH 4 종류 / 폭 0 / 긴 배선) x 600 Defect: 엔진 = 전수 Gap 계산 (5조합)", same, string.Join(" | ", detail.ToArray()));

        List<GdsKlarfDefectGap> curveSrc = GdsKlarfMatchEngine.MatchShapes(index, defects, Opt(5, true, false), null);
        double[] steps = { 0, 2, 5, 10, 20, 50 };
        int[] counts = steps.Select(t => curveSrc.Count(e => e.MinGap <= t)).ToArray();
        bool mono = true;
        for (int i = 1; i < counts.Length; i++) if (counts[i] < counts[i - 1]) mono = false;
        Check("B8 허용오차 표(0/2/5/10/20/50) 한 번 계산으로 세기 / 단조 증가", mono && counts[5] >= counts[0], string.Join(" / ", counts.Select(x => x.ToString()).ToArray()));

        string e1 = Throws(() => GdsKlarfMatchEngine.MatchShapes(index, defects, Opt(-1, true, false), null));
        GdsKlarfMatchOptions bad = Opt(0, true, false); bad.SquareSide = -1;
        string e2 = Throws(() => GdsKlarfMatchEngine.MatchShapes(index, defects, bad, null));
        Check("B9 음수 허용오차 / 음수 사각형 거부", e1.Contains("허용오차") && e2.Contains("한 변"), e1 + " / " + e2);
    }

    // ===== C. Cache =====
    private static void CacheChecks()
    {
        DataTable t = ShapeTable();
        AddRow(t, 1, "B1", "BOUNDARY", null, null, 1, Bin(0, 0, 10, 0, 10, 10, 0, 10, 0, 0), 5);
        AddRow(t, 1, "P1", "PATH", 10.0, 2, 1, Bin(0, 0, 100, 0), 2);
        AddRow(t, 2, "P2", "PATH", 10.0, 0, 1, Bin(0, 50, 100, 50), 2);
        AddRow(t, 1, "T1", "TEXT", null, null, 1, Bin(5, 5), 1);
        GdsKlarfShapeIndex built = GdsKlarfShapeIndex.Build(t, new HashSet<long> { 1 }, 200);
        Check("C1 합성 표 -> 모양: READY Chain 만 / TEXT 제외 / 점 수 / PATH Type 2 Bounds",
            built.Shapes.Count == 2 && built.SkippedCount == 1 && built.PointCount == 7 && built.Shapes[1].IsPath && Bounds(built.Shapes[1], -5, -5, 105, 5),
            "shapes " + built.Shapes.Count + " skipped " + built.SkippedCount + " points " + built.PointCount);

        DataTable badLen = ShapeTable();
        AddRow(badLen, 1, "BAD", "BOUNDARY", null, null, 1, Bin(0, 0, 1, 1), 3);
        DataTable badVer = ShapeTable();
        AddRow(badVer, 1, "VER", "BOUNDARY", null, null, 2, Bin(0, 0, 1, 1), 2);
        string l = Throws(() => GdsKlarfShapeIndex.Build(badLen, null, 200));
        string v = Throws(() => GdsKlarfShapeIndex.Build(badVer, null, 200));
        Check("C2 좌표 바이트 길이 / 형식 버전 검사", l.Contains("좌표 바이트 길이") && v.Contains("형식 버전"), l + " / " + v);

        GdsKlarfGeometryCache.Clear();
        int loads = 0;
        Func<DataTable> load = () => { loads++; return t; };
        bool h1, h2, h3;
        long ms;
        var ready = new HashSet<long> { 1 };
        GdsKlarfShapeIndex a = GdsKlarfGeometryCache.GetOrLoad("F", "D1", "s1", ready, load, out h1, out ms);
        GdsKlarfShapeIndex b = GdsKlarfGeometryCache.GetOrLoad("F", "D1", "s1", ready, load, out h2, out ms);
        GdsKlarfShapeIndex c = GdsKlarfGeometryCache.GetOrLoad("F", "D1", "s2", ready, load, out h3, out ms);
        Check("C3 처음 읽기 / 같은 Stamp 는 Cache / Stamp 바뀌면 다시 읽기", !h1 && h2 && !h3 && ReferenceEquals(a, b) && !ReferenceEquals(a, c) && loads == 2,
            "hit " + h1 + "," + h2 + "," + h3 + " loads " + loads);

        foreach (string d in new[] { "D2", "D3", "D4" }) GdsKlarfGeometryCache.GetOrLoad("F", d, "s", ready, load, out h1, out ms);
        string afterFour = string.Join(",", GdsKlarfGeometryCache.Keys());
        GdsKlarfGeometryCache.GetOrLoad("F", "D2", "s", ready, load, out h2, out ms);
        string afterTouch = string.Join(",", GdsKlarfGeometryCache.Keys());
        Check("C4 최근 3 Device(LRU): D1 밀려남 / 다시 쓴 D2 가 앞으로", afterFour == "F|D4,F|D3,F|D2" && h2 && afterTouch == "F|D2,F|D4,F|D3",
            afterFour + " -> " + afterTouch);

        GdsKlarfGeometryCache.Clear();
        int slowLoads = 0;
        var got = new GdsKlarfShapeIndex[6];
        var threads = new Thread[6];
        for (int i = 0; i < threads.Length; i++)
        {
            int n = i;
            threads[i] = new Thread(() =>
            {
                bool hit;
                long m;
                got[n] = GdsKlarfGeometryCache.GetOrLoad("F", "SAME", "s", ready, () => { Interlocked.Increment(ref slowLoads); Thread.Sleep(300); return t; }, out hit, out m);
            });
        }
        foreach (Thread th in threads) th.Start();
        foreach (Thread th in threads) th.Join();
        Check("C5 같은 Device 동시 6 호출 -> 한 번만 읽고 같은 Cache", slowLoads == 1 && got.All(x => ReferenceEquals(x, got[0])), "loads " + slowLoads);

        DataTable map = new DataTable();
        foreach (string col in new[] { "CREATE_TIME", "IMPORT_JOB_SEQ", "VIEW_SIDE" }) map.Columns.Add(col, typeof(object));
        map.Rows.Add(new DateTime(2026, 10, 16, 9, 0, 0), 12, "FRONT");
        DataTable chains = new DataTable();
        foreach (string col in new[] { "CHAIN_SEQ", "UPDATE_TIME", "MEMBER_COUNT" }) chains.Columns.Add(col, typeof(object));
        chains.Rows.Add(2, new DateTime(2026, 10, 16, 8, 0, 0), 10);
        chains.Rows.Add(1, new DateTime(2026, 10, 16, 7, 0, 0), 20);
        string st1 = GdsKlarfGeometryCache.MakeStamp(map.Rows[0], chains.Rows.Cast<DataRow>());
        string st2 = GdsKlarfGeometryCache.MakeStamp(map.Rows[0], chains.Rows.Cast<DataRow>().Reverse());
        chains.Rows[0]["UPDATE_TIME"] = new DateTime(2026, 10, 16, 8, 0, 1);
        string st3 = GdsKlarfGeometryCache.MakeStamp(map.Rows[0], chains.Rows.Cast<DataRow>());
        Check("C6 Stamp: Chain 순서와 무관 / Chain 저장 시각 바뀌면 다름", st1 == st2 && st1 != st3, st1);
        GdsKlarfGeometryCache.Clear();
    }

    // ===== D. 실제 DB (조회만) =====
    private static void DbChecks(string factory, string device, long stepSeq)
    {
        DataTable mapInfo = new DACrux.SEMDMS.DSL.TQP_GDS_MAP().GetData("SELECT_MAP_INFO", null, new[] { factory, device });
        if (mapInfo.Rows.Count == 0) throw new InvalidOperationException("GDS Map 이 없습니다: " + factory + " / " + device);
        DataTable chainList = new DACrux.SEMDMS.DSL.TQP_GDS_CHAIN().GetData("SELECT_CHAIN_LIST", null, new[] { factory, device });
        List<DataRow> readyRows = chainList.Rows.Cast<DataRow>().Where(r => Convert.ToString(r["VERSION_STATUS"]) == "READY").ToList();
        var ready = new HashSet<long>(readyRows.Select(r => Convert.ToInt64(r["CHAIN_SEQ"])));
        string stamp = GdsKlarfGeometryCache.MakeStamp(mapInfo.Rows[0], readyRows);
        DataTable rows = null;
        Func<DataTable> load = () =>
        {
            rows = new DACrux.SEMDMS.DSL.TQP_GDS_CHAIN_MEMBER().GetData("SELECT_CHAIN_MEMBER_SHAPE_ALL", null, new[] { factory, device });
            return rows;
        };
        GdsKlarfGeometryCache.Clear();
        bool hit1, hit2;
        long loadMs, loadMs2;
        var watch = Stopwatch.StartNew();
        GdsKlarfShapeIndex index = GdsKlarfGeometryCache.GetOrLoad(factory, device, stamp, ready, load, out hit1, out loadMs);
        long first = watch.ElapsedMilliseconds;
        watch.Restart();
        GdsKlarfShapeIndex again = GdsKlarfGeometryCache.GetOrLoad(factory, device, stamp, ready, load, out hit2, out loadMs2);
        long second = watch.ElapsedMilliseconds;
        int readyRowsCount = rows.Rows.Cast<DataRow>().Count(r => ready.Contains(Convert.ToInt64(r["CHAIN_SEQ"])));
        Check("D1 Device 전체 모양 Cache: 처음 읽기 / 두 번째 Cache", !hit1 && hit2 && ReferenceEquals(index, again) && index.Shapes.Count + index.SkippedCount == readyRowsCount,
            String.Format(CultureInfo.InvariantCulture, "READY Chain {0} / Member {1:N0} (TEXT {2}) / 점 {3:N0} / 조회 {4}ms + 모양 {5}ms = {6}ms / Cache {7}ms / 격자 칸 {8:N0} / 큰 도형 {9}",
                ready.Count, index.Shapes.Count, index.SkippedCount, index.PointCount, loadMs, index.BuildMs, first, second, index.CellCount, index.LargeCount));

        // DB Bounds 와 비교 (DB = 꼭짓점 ± W/2, PATH_TYPE 무시)
        var dbBox = new Dictionary<string, double[]>();
        foreach (DataRow r in rows.Rows)
        {
            string key = Convert.ToString(r["CHAIN_SEQ"]) + "|" + Convert.ToString(r["PLACED_ELEMENT_ID"]);
            if (!dbBox.ContainsKey(key))
                dbBox.Add(key, new[] { Convert.ToDouble(r["MIN_X"]), Convert.ToDouble(r["MIN_Y"]), Convert.ToDouble(r["MAX_X"]), Convert.ToDouble(r["MAX_Y"]) });
        }
        double polyDiff = 0, pathOver = 0, pathShrinkArea = 0, pathDbArea = 0;
        int polyN = 0, pathN = 0, pathShrunk = 0, startNonZero = 0;
        foreach (GdsKlarfShape s in index.Shapes)
        {
            double[] b = dbBox[s.ChainSeq + "|" + s.PlacedElementId];
            if (!s.IsPath)
            {
                polyN++;
                polyDiff = Math.Max(polyDiff, Math.Max(Math.Max(Math.Abs(s.MinX - b[0]), Math.Abs(s.MinY - b[1])), Math.Max(Math.Abs(s.MaxX - b[2]), Math.Abs(s.MaxY - b[3]))));
                double[][] p = s.Pieces[0];
                if (GdsKlarfShapeGeometry.Gap(s, p[0][0], p[1][0], p[0][0], p[1][0]) != 0) startNonZero++;
            }
            else
            {
                pathN++;
                pathOver = Math.Max(pathOver, Math.Max(Math.Max(b[0] - s.MinX, b[1] - s.MinY), Math.Max(s.MaxX - b[2], s.MaxY - b[3])));
                double dbArea = (b[2] - b[0]) * (b[3] - b[1]), myArea = (s.MaxX - s.MinX) * (s.MaxY - s.MinY);
                pathDbArea += dbArea;
                if (myArea < dbArea - 1e-6) { pathShrunk++; pathShrinkArea += dbArea - myArea; }
            }
        }
        Check("D2 BOUNDARY Bounds = DB Bounds / PATH 는 끝 연장 없는 Type 0 만큼 줄어듦",
            polyN > 0 && polyDiff < 1e-6 && startNonZero == 0,
            String.Format(CultureInfo.InvariantCulture, "BOUNDARY {0:N0} 최대 차이 {1:0.######} / PATH {2:N0} 중 Bounds 줄어듦 {3:N0} (면적 {4:0.0}%) / DB Bounds 밖으로 나간 최대 {5:0.###} µm (꺾임 Miter)",
                polyN, polyDiff, pathN, pathShrunk, pathDbArea > 0 ? 100 * pathShrinkArea / pathDbArea : 0, pathOver));

        // 실제 KLARF Step: Bounds 판정(지금 AnalyzeWafer) vs 모양 판정
        var bsl = new GdsKlarfAnalysis();
        DataTable defectRows = new DACrux.SEMDMS.DSL.TQD_DEFECT().GetDefect(stepSeq);
        var defects = defectRows.Rows.Cast<DataRow>().Select(r => new GdsKlarfDefectPoint
        {
            DefectId = Convert.ToInt64(r["DEFECTID"]),
            XIndex = Convert.ToInt32(r["XINDEX"]),
            YIndex = Convert.ToInt32(r["YINDEX"]),
            XRel = Convert.ToDouble(r["XREL"]),
            YRel = Convert.ToDouble(r["YREL"]),
            XSize = Convert.ToDouble(r["XSIZE"]),
            YSize = Convert.ToDouble(r["YSIZE"])
        }).ToList();
        var boxes = index.Shapes.Select(s =>
        {
            double[] b = dbBox[s.ChainSeq + "|" + s.PlacedElementId];
            return Box(s.ChainSeq, b[0], b[1], b[2], b[3]);
        }).ToList();
        foreach (string side in new[] { "BACK", "FRONT" })
        {
            DataRow info = bsl.AnalyzeWafer(factory, device, stepSeq, side, 5, true, 0, 0, null, null).Tables["ANL_INFO"].Rows[0];
            var opt = Opt(5, true, Convert.ToString(info["MIRROR"]) == "Y");
            opt.PitchX = Convert.ToDouble(info["PITCH_X"]);
            opt.PitchY = Convert.ToDouble(info["PITCH_Y"]);
            int oldIn = GdsKlarfMatchEngine.Match(boxes, defects, opt).Count(m => m.MemberIndexes.Count > 0);
            watch.Restart();
            List<GdsKlarfDefectGap> gaps = GdsKlarfMatchEngine.MatchShapes(index, defects, opt, null);
            long matchMs = watch.ElapsedMilliseconds;
            int[] curve = new double[] { 0, 2, 5, 10, 20, 50 }.Select(tol => gaps.Count(e => e.MinGap <= tol)).ToArray();
            string near = string.Join(", ", gaps.Where(e => e.Near.Count > 0).OrderBy(e => e.MinGap).Take(5)
                .Select(e => e.Defect.DefectId + ":" + e.MinGap.ToString("0.0", CultureInfo.InvariantCulture) + "µm").ToArray());
            Check("D3 STEP " + stepSeq + " 보기 기준 " + side + ": Bounds 판정 = AnalyzeWafer / 모양 판정은 Bounds 판정 이하",
                oldIn == Convert.ToInt32(info["IN_CHAIN_DEFECT_COUNT"]) && curve[2] <= oldIn,
                String.Format(CultureInfo.InvariantCulture, "Defect {0} / 반전 {1} / Bounds 판정 {2} -> 모양 판정(Gap≤5) {3} / 허용오차 표 0:{4} 2:{5} 5:{6} 10:{7} 20:{8} 50:{9} / 판정 {10}ms / 가까운 순 {11}",
                    defects.Count, info["MIRROR"], oldIn, curve[2], curve[0], curve[1], curve[2], curve[3], curve[4], curve[5], matchMs, near));
        }

        // D4 대량 합성 (설계 9.3절, 2026-10-16): 실제 Device 모양 색인에 합성 Defect 1만 / 10만 / 30만 개 (절반은 Chip 범위 무작위, 절반은 Member 꼭짓점 위)
        double minX = index.Shapes.Min(s => s.MinX), maxX = index.Shapes.Max(s => s.MaxX), minY = index.Shapes.Min(s => s.MinY), maxY = index.Shapes.Max(s => s.MaxY);
        var rnd = new Random(7);
        foreach (int n in new[] { 10000, 100000, 300000 })
        {
            var many = new List<GdsKlarfDefectPoint>(n);
            for (int i = 0; i < n; i++)
            {
                double x, y;
                if (i % 2 == 0)
                {
                    x = minX + rnd.NextDouble() * (maxX - minX);
                    y = minY + rnd.NextDouble() * (maxY - minY);
                }
                else
                {
                    GdsKlarfShape s = index.Shapes[rnd.Next(index.Shapes.Count)];
                    double[][] piece = s.Pieces.Count > 0 ? s.Pieces[0] : null;
                    x = piece != null ? piece[0][0] : s.MinX;
                    y = piece != null ? piece[1][0] : s.MinY;
                }
                many.Add(new GdsKlarfDefectPoint { DefectId = i + 1, XRel = x + 23832 / 2.0, YRel = y + 30060 / 2.0, XSize = rnd.NextDouble() * 20, YSize = rnd.NextDouble() * 20 });
            }
            var parts = new List<string>();
            bool ok = true;
            foreach (double tol in new[] { 5.0, 50.0 })
            {
                var o = new GdsKlarfMatchOptions { PitchX = 23832, PitchY = 30060, Tolerance = tol, UseDefectSize = true };
                GC.Collect();
                long before = GC.GetTotalMemory(true);
                watch.Restart();
                List<GdsKlarfDefectGap> r = GdsKlarfMatchEngine.MatchShapes(index, many, o, null);
                long ms = watch.ElapsedMilliseconds;
                long mb = (GC.GetTotalMemory(false) - before) / (1024 * 1024);
                int hits = r.Count(g => g.MinGap <= tol);
                long pairs = r.Sum(g => (long)g.Near.Count);
                int chainPairs = r.Sum(g => GdsKlarfMatchEngine.NearestByChain(g, index.Shapes).Count(p => p.Value.Gap <= tol));
                if (hits < n / 2) ok = false;
                parts.Add(String.Format(CultureInfo.InvariantCulture, "허용오차 {0}: {1:N0}ms / 걸림 {2:N0} / (Defect,Chain) 걸림 {3:N0} / 탐색 거리 안 Member 쌍 {4:N0} / 결과 메모리 약 {5} MB",
                    tol, ms, hits, chainPairs, pairs, mb));
            }
            Check("D4 대량 합성 Defect " + n.ToString("N0", CultureInfo.InvariantCulture) + "개 판정(절반은 Member 위 -> 걸림 ≥ 절반)", ok, string.Join(" | ", parts.ToArray()));
        }
        GdsKlarfGeometryCache.Clear();
    }

    // ===== 도우미 =====
    private static void Check(string name, bool ok, string detail)
    {
        if (ok) _pass++; else _fail++;
        Console.WriteLine((ok ? "PASS" : "FAIL") + " / " + name + " / " + detail);
    }

    private static string Throws(Action action)
    {
        try { action(); return ""; }
        catch (Exception ex)
        {
            while (ex.InnerException != null) ex = ex.InnerException;
            return ex.Message;
        }
    }

    private static bool Eq(double a, double b) { return Math.Abs(a - b) < 1e-6; }

    private static string F(double v) { return v.ToString("0.######", CultureInfo.InvariantCulture); }

    private static bool Bounds(GdsKlarfShape s, double x1, double y1, double x2, double y2)
    {
        return Eq(s.MinX, x1) && Eq(s.MinY, y1) && Eq(s.MaxX, x2) && Eq(s.MaxY, y2);
    }

    private static double At(GdsKlarfShape s, double x, double y) { return GdsKlarfShapeGeometry.Gap(s, x, y, x, y); }

    private static double Gap(GdsKlarfShape s, double x1, double y1, double x2, double y2) { return GdsKlarfShapeGeometry.Gap(s, x1, y1, x2, y2); }

    private static GdsKlarfShape Shape(string type, double width, int pathType, params double[] xy) { return ShapeOf(1, type, width, pathType, xy); }

    private static GdsKlarfShape ShapeOf(long chain, string type, double width, int pathType, double[] xy)
    {
        var xs = new double[xy.Length / 2];
        var ys = new double[xy.Length / 2];
        for (int i = 0; i < xs.Length; i++) { xs[i] = xy[2 * i]; ys[i] = xy[2 * i + 1]; }
        return GdsKlarfShapeGeometry.Create(chain, "S" + Guid.NewGuid().ToString("N").Substring(0, 6), 1, "MEMBER", type, width, pathType, xs, ys);
    }

    private static GdsKlarfShape Rect(long chain, double x1, double y1, double x2, double y2)
    {
        return ShapeOf(chain, "BOUNDARY", 0, 0, new[] { x1, y1, x2, y1, x2, y2, x1, y2, x1, y1 });
    }

    private static GdsKlarfMemberBox Box(long chain, double x1, double y1, double x2, double y2)
    {
        return new GdsKlarfMemberBox { ChainSeq = chain, PlacedElementId = "B", LayerId = 1, RoleCode = "MEMBER", MinX = x1, MinY = y1, MaxX = x2, MaxY = y2 };
    }

    private static GdsKlarfShapeIndex Index(GdsKlarfShape shape)
    {
        return new GdsKlarfShapeIndex(new List<GdsKlarfShape> { shape }, 200, 0, 0);
    }

    private static GdsKlarfDefectPoint Defect(long id, double xrel, double yrel, double xs, double ys)
    {
        return new GdsKlarfDefectPoint { DefectId = id, XIndex = 1, YIndex = 1, XRel = xrel, YRel = yrel, XSize = xs, YSize = ys };
    }

    private static GdsKlarfMatchOptions Opt(double tol, bool useSize, bool mirror)
    {
        return new GdsKlarfMatchOptions { PitchX = 1000, PitchY = 2000, Tolerance = tol, UseDefectSize = useSize, Mirror = mirror };
    }

    private static GdsKlarfDefectGap One(GdsKlarfShapeIndex index, GdsKlarfDefectPoint defect, GdsKlarfMatchOptions o, HashSet<long> filter)
    {
        return GdsKlarfMatchEngine.MatchShapes(index, new List<GdsKlarfDefectPoint> { defect }, o, filter)[0];
    }

    private static DataTable ShapeTable()
    {
        var t = new DataTable();
        t.Columns.Add("CHAIN_SEQ", typeof(decimal));
        t.Columns.Add("PLACED_ELEMENT_ID", typeof(string));
        t.Columns.Add("ROLE_CODE", typeof(string));
        t.Columns.Add("LAYER_ID", typeof(decimal));
        t.Columns.Add("ELEMENT_TYPE", typeof(string));
        t.Columns.Add("CLOSED_FLAG", typeof(string));
        t.Columns.Add("PATH_WIDTH", typeof(decimal));
        t.Columns.Add("PATH_TYPE", typeof(decimal));
        t.Columns.Add("POINTS_FORMAT_VERSION", typeof(decimal));
        t.Columns.Add("POINT_COUNT", typeof(decimal));
        t.Columns.Add("POINTS_BIN", typeof(byte[]));
        return t;
    }

    private static void AddRow(DataTable t, long chain, string id, string type, double? width, int? pathType, int version, byte[] bin, int count)
    {
        t.Rows.Add(chain, id, "MEMBER", 1, type, type == "BOUNDARY" ? "1" : "0",
            width.HasValue ? (object)(decimal)width.Value : DBNull.Value, pathType.HasValue ? (object)pathType.Value : DBNull.Value, version, count, bin);
    }

    private static byte[] Bin(params double[] xy)
    {
        var bin = new byte[xy.Length * 8];
        for (int i = 0; i < xy.Length; i++) Buffer.BlockCopy(BitConverter.GetBytes(xy[i]), 0, bin, i * 8, 8);
        return bin;
    }
}
