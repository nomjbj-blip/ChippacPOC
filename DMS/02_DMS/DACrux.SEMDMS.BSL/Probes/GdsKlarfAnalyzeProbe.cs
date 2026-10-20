// 연관 분석 2단계 확인: 통합 연관 분석 서비스 Analyze / AnalyzeDie (실제 DB 조회만)
// 설계: 문서/설계/05_연관분석/2026-10-15_GDS_KLARF_ETest_통합분석_데이터_설계.md 10.2 / 15.3 / 16.7 / 18 / 19절
// 빌드 / 실행은 GdsKlarfAnalyzeProbe.ps1 이 한다(Interface / DSL / BSL TEMP 빌드 DLL 참조).
//  - 결과 표 / 열 계약, 기존 AnalyzeWafer 보기 기준과 면 규칙 대응, 엔진 직접 호출과 같은 수
//  - 표끼리 합계 일관성, 허용오차 표, 중심 사각형, 여러 Step 합치기(시스템 Die 좌표), Die 상세 / 역추적 거리
//  - Chain / 분류 거르기, 큰 Defect 제외, Cache, 오류 안내, Binary 직렬화
//  - KLARF 원본 Pitch 기록(업로드 COMMENT) / 읽기 (결정 B6)
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using DACrux.SEMDMS.BSL;
using DACrux.SEMDMS.Interface;

internal static class GdsKlarfAnalyzeProbe
{
    private static int _pass, _fail;
    private static string _factory, _device;
    private static long _step;

    private static int Main(string[] args)
    {
        _factory = args.Length > 0 ? args[0] : "SCK";
        _device = args.Length > 1 ? args[1] : "OMM_Hybrid";
        _step = args.Length > 2 ? Int64.Parse(args[2], CultureInfo.InvariantCulture) : 528;
        long[] extra = args.Length > 3 ? args[3].Split(',').Select(s => Int64.Parse(s, CultureInfo.InvariantCulture)).ToArray() : new long[] { 529, 602 };
        string[] extraSides = args.Length > 4 ? args[4].Split(',') : new[] { "FRONT", "BACK" };
        try
        {
            Run(extra, extraSides);
        }
        catch (Exception ex)
        {
            Check("예외 없이 끝까지", false, ex.ToString());
        }
        Console.WriteLine();
        Console.WriteLine("GDS_KLARF_ANALYZE_PROBE=" + (_fail == 0 ? "PASS" : "FAIL") + " (" + _pass + "/" + (_pass + _fail) + ")");
        return _fail == 0 ? 0 : 1;
    }

    private static void Run(long[] extra, string[] extraSides)
    {
        var bsl = new GdsKlarfAnalysis();
        GdsKlarfGeometryCache.Clear();

        // ===== E1 결과 계약 / 기본 조건(Chain FS, 검사 BS -> 반전) =====
        var watch = Stopwatch.StartNew();
        DataSet ds = bsl.Analyze(Req(_step, "BACK", "FRONT"));
        long firstMs = watch.ElapsedMilliseconds;
        DataRow info = ds.Tables["ANL_INFO"].Rows[0];
        string[] tables = { "ANL_INFO", "CHAIN", "DIE", "DIE_CHAIN", "DEFECT", "DEFECT_CHAIN", "TOL_CURVE", "ET_LINK" };
        string[] infoCols = { "WAFER_KEY", "STEP_SEQS", "STEP_SIDES", "CHAIN_SIDE", "DISPLAY_STEP_SEQ", "AREA_MODE", "HIT_GAP_UM", "SEARCH_UM", "CLASS_BASIS",
            "PITCH_SOURCE", "CACHE_HIT", "GDS_MS", "DEFECT_MS", "RESULT_MS", "ET_SRC_FILE_SEQ", "MAP_SHAPE_SAME", "MIRROR", "VIEW_SIDE", "TOLERANCE_UM", "USE_DEFECT_SIZE" };
        string[] dieChainCols = { "MIN_GAP", "NEAR_DEFECT_ID", "NEAR_STEP_ID", "ET_STATUS", "ET_PARA_COUNT", "ET_FAIL_COUNT", "ET_FIRST_FAIL_ITEM", "ET_FIRST_FAIL_VALUE", "CLASS4", "SYS_X" };
        string[] defectCols = { "STEP_SEQ", "STEP_ID", "GAP", "NEAR_CHAIN_SEQ", "ROUGHBINNUMBER", "LARGE", "GDS_X", "GDS_Y", "IN_CHAIN", "CHAIN_CODES" };
        string[] chainCols = { "ET_LINK_STATUS", "ET_PARA_COUNT", "DIE_JUDGED", "DIE_DEFECT", "DIE_FAIL", "DIE_BOTH", "FAIL_RATE_DEFECT", "FAIL_RATE_CLEAN" };
        string[] dieCols = { "SYS_X", "SYS_Y", "ET_STATE", "ET_FAIL_CHAIN_COUNT", "BOTH_CHAIN_COUNT" };
        var missing = new List<string>();
        foreach (string t in tables) if (!ds.Tables.Contains(t)) missing.Add(t);
        Missing(ds, "ANL_INFO", infoCols, missing);
        Missing(ds, "DIE_CHAIN", dieChainCols, missing);
        Missing(ds, "DEFECT", defectCols, missing);
        Missing(ds, "CHAIN", chainCols, missing);
        Missing(ds, "DIE", dieCols, missing);
        Missing(ds, "DEFECT_CHAIN", new[] { "STEP_SEQ", "DEFECTID", "CHAIN_SEQ", "GAP", "NEAR_PLACED_ELEMENT_ID" }, missing);
        Missing(ds, "TOL_CURVE", new[] { "TOL_UM", "IN_CHAIN_DEFECT", "HIT_DIE" }, missing);
        Missing(ds, "ET_LINK", new[] { "GDS_CHAIN_KEY", "CHAIN_SEQ", "CHAIN_CODE", "STATUS", "PARA_COUNT", "CHAIN2_PARA_COUNT" }, missing);
        Check("E1 결과 표 8개 / 설계 10.2 열 / Chain FS · 검사 BS = 반전, 보기 BACK",
            missing.Count == 0 && S(info, "MIRROR") == "Y" && S(info, "VIEW_SIDE") == "BACK" && S(info, "CHAIN_SIDE") == "FRONT" && S(info, "STEP_SIDES") == "BACK"
            && S(info, "USE_DEFECT_SIZE") == "Y" && D(info, "TOLERANCE_UM") == 5 && D(info, "SEARCH_UM") == 50,
            (missing.Count == 0 ? "" : "없음: " + string.Join(",", missing.ToArray()) + " / ") + Summary(info) + " / 첫 호출 " + firstMs + "ms");

        // ===== E2 기존 AnalyzeWafer 와 면 규칙 대응 + 엔진 직접 호출과 같은 수 =====
        DataSet same = bsl.Analyze(Req(_step, "BACK", "BACK"));
        DataRow sameInfo = same.Tables["ANL_INFO"].Rows[0];
        DataRow oldBack = bsl.AnalyzeWafer(_factory, _device, _step, "BACK", 5, true, 0, 0, null, null).Tables["ANL_INFO"].Rows[0];
        DataRow oldFront = bsl.AnalyzeWafer(_factory, _device, _step, "FRONT", 5, true, 0, 0, null, null).Tables["ANL_INFO"].Rows[0];
        int directMirror = DirectInChain(info, true), directSame = DirectInChain(sameInfo, false);
        Check("E2 면 규칙: (FS,BS) = 예전 BACK 보기 / (BS,BS) = 예전 FRONT 보기, Chain 영역 수 = 엔진 직접 판정",
            S(info, "MIRROR") == S(oldBack, "MIRROR") && S(sameInfo, "MIRROR") == S(oldFront, "MIRROR") && S(sameInfo, "VIEW_SIDE") == "FRONT"
            && I(info, "IN_CHAIN_DEFECT_COUNT") == directMirror && I(sameInfo, "IN_CHAIN_DEFECT_COUNT") == directSame,
            String.Format("(FS,BS) 모양 {0} / 직접 {1} / 예전 Bounds {2} | (BS,BS) 모양 {3} / 직접 {4} / 예전 Bounds {5}",
                I(info, "IN_CHAIN_DEFECT_COUNT"), directMirror, I(oldBack, "IN_CHAIN_DEFECT_COUNT"), I(sameInfo, "IN_CHAIN_DEFECT_COUNT"), directSame, I(oldFront, "IN_CHAIN_DEFECT_COUNT")));

        // ===== E3 표끼리 합계 =====
        foreach (DataSet r in new[] { ds, same })
        {
            DataRow ri = r.Tables["ANL_INFO"].Rows[0];
            int inChain = I(ri, "IN_CHAIN_DEFECT_COUNT"), total = I(ri, "DEFECT_COUNT");
            int dieIn = Rows(r, "DIE").Sum(x => I(x, "IN_CHAIN_COUNT")), dieAll = Rows(r, "DIE").Sum(x => I(x, "DEFECT_COUNT"));
            int dcSum = Rows(r, "DIE_CHAIN").Sum(x => I(x, "DEFECT_COUNT")), pairs = r.Tables["DEFECT_CHAIN"].Rows.Count;
            int chainSum = Rows(r, "CHAIN").Sum(x => I(x, "HIT_DEFECT_COUNT"));
            int defectIn = Rows(r, "DEFECT").Count(x => S(x, "IN_CHAIN") == "Y");
            bool gapOk = Rows(r, "DEFECT").All(x => S(x, "IN_CHAIN") != "Y" || D(x, "GAP") <= 5);
            bool pairGapOk = Rows(r, "DEFECT_CHAIN").All(x => D(x, "GAP") <= 5);
            bool nearOk = Rows(r, "DIE_CHAIN").All(x => S(x, "MIN_GAP").Length > 0 && D(x, "MIN_GAP") <= 5);
            Check("E3 합계 (" + S(ri, "CHAIN_SIDE") + "," + S(ri, "STEP_SIDES") + "): Die 합 = Defect 수 / Die Chain 영역 합 = Chain 영역 / (Die,Chain) 합 = Chain 합 = Defect-Chain 쌍 / Gap ≤ 5",
                dieIn == inChain && dieAll == total && r.Tables["DEFECT"].Rows.Count == total && defectIn == inChain && dcSum == pairs && chainSum == pairs
                && gapOk && pairGapOk && nearOk && S(ri, "DEFECT_ROWS") == "ALL",
                String.Format("Defect {0} / Chain 영역 {1} / Die 합 {2}·{3} / 쌍 {4} / (Die,Chain) {5} / Chain 합 {6}", total, inChain, dieAll, dieIn, pairs, dcSum, chainSum));
        }

        // ===== E4 허용오차 표 =====
        List<DataRow> curve = Rows(same, "TOL_CURVE").ToList();
        bool mono = true;
        for (int i = 1; i < curve.Count; i++) if (I(curve[i], "IN_CHAIN_DEFECT") < I(curve[i - 1], "IN_CHAIN_DEFECT") || I(curve[i], "HIT_DIE") < I(curve[i - 1], "HIT_DIE")) mono = false;
        DataRow at5 = curve.FirstOrDefault(x => D(x, "TOL_UM") == 5);
        DataSet tol7 = bsl.Analyze(Req(_step, "BACK", "BACK", 7));
        List<DataRow> curve7 = Rows(tol7, "TOL_CURVE").ToList();
        DataRow at7 = curve7.FirstOrDefault(x => D(x, "TOL_UM") == 7);
        Check("E4 허용오차 표 0/2/5/10/20/50 단조 / 5 행 = Chain 영역 수 / 지금 값 7 은 행 추가 + 그 행 = 7 분석 결과",
            curve.Count == 6 && mono && at5 != null && I(at5, "IN_CHAIN_DEFECT") == I(sameInfo, "IN_CHAIN_DEFECT_COUNT")
            && curve7.Count == 7 && at7 != null && I(at7, "IN_CHAIN_DEFECT") == I(tol7.Tables["ANL_INFO"].Rows[0], "IN_CHAIN_DEFECT_COUNT"),
            string.Join(" / ", curve.Select(x => S(x, "TOL_UM") + ":" + S(x, "IN_CHAIN_DEFECT") + "(" + S(x, "HIT_DIE") + ")").ToArray()));

        // ===== E5 중심 사각형 =====
        GdsKlarfAnalysisRequest sqReq = Req(_step, "BACK", "BACK");
        sqReq.AreaMode = GdsKlarfAnalysisRequest.AreaSquare;
        sqReq.SquareUm = 10;
        DataSet sq = bsl.Analyze(sqReq);
        DataRow sqInfo = sq.Tables["ANL_INFO"].Rows[0];
        int directSquare = DirectInChain(sqInfo, false, 10);
        Check("E5 중심 사각형 한 변 10: USE_DEFECT_SIZE N / TOLERANCE_UM 5(반폭) / 걸림 Gap 0 / 허용오차 표 비움 / 엔진 직접 판정과 같음",
            S(sqInfo, "USE_DEFECT_SIZE") == "N" && D(sqInfo, "TOLERANCE_UM") == 5 && D(sqInfo, "HIT_GAP_UM") == 0 && sq.Tables["TOL_CURVE"].Rows.Count == 0
            && I(sqInfo, "IN_CHAIN_DEFECT_COUNT") == directSquare && Rows(sq, "DEFECT_CHAIN").All(x => D(x, "GAP") == 0),
            "Chain 영역 " + I(sqInfo, "IN_CHAIN_DEFECT_COUNT") + " / 직접 " + directSquare + " / " + S(sqInfo, "AREA_MODE"));

        // ===== E6 여러 Step 합치기 =====
        var seqs = new List<long> { _step };
        var sides = new List<string> { "BACK" };
        for (int i = 0; i < extra.Length; i++)
        {
            DataTable t = new DACrux.SEMDMS.DSL.TQD_INSP_INFO().GetData("SELECT_DMS_WAFER_INFO", null, new[] { extra[i].ToString(CultureInfo.InvariantCulture) });
            if (t.Rows.Count == 0) continue;
            seqs.Add(extra[i]);
            sides.Add(extraSides.Length > i ? extraSides[i] : "BACK");
        }
        if (seqs.Count > 1)
        {
            GdsKlarfAnalysisRequest multiReq = Req(_step, "BACK", "BACK");
            multiReq.StepSeqs = seqs.ToArray();
            multiReq.ViewSides = sides.ToArray();
            multiReq.DisplayStepSeq = _step;
            DataSet multi = bsl.Analyze(multiReq);
            DataRow mi = multi.Tables["ANL_INFO"].Rows[0];
            int sumDefects = 0, sumIn = 0;
            var detail = new List<string>();
            bool sysOk = true, keyOk = true;
            for (int i = 0; i < seqs.Count; i++)
            {
                GdsKlarfAnalysisRequest one = Req(seqs[i], sides[i], "BACK");
                DataSet single = bsl.Analyze(one);
                DataRow si = single.Tables["ANL_INFO"].Rows[0];
                sumDefects += I(si, "DEFECT_COUNT");
                sumIn += I(si, "IN_CHAIN_DEFECT_COUNT");
                int mine = Rows(multi, "DEFECT").Count(x => L(x, "STEP_SEQ") == seqs[i]);
                int mineIn = Rows(multi, "DEFECT").Count(x => L(x, "STEP_SEQ") == seqs[i] && S(x, "IN_CHAIN") == "Y");
                detail.Add(seqs[i] + "(" + sides[i] + ") " + mine + "/" + I(si, "DEFECT_COUNT") + " 영역 " + mineIn + "/" + I(si, "IN_CHAIN_DEFECT_COUNT"));
                if (mine != I(si, "DEFECT_COUNT") || mineIn != I(si, "IN_CHAIN_DEFECT_COUNT")) keyOk = false;
                // 시스템 Die: FS Step 도 KLARF Index 그대로 (2026-10-20, 검사 면은 반전에만 씀. 이전 FS 규칙 MinX + MaxX - x 는 B16 2차 때 다시 본다)
                if (sides[i] == "FRONT")
                {
                    DataSet raw = bsl.Analyze(Req(seqs[i], "BACK", "BACK"));
                    var rawIndex = Rows(raw, "DEFECT").ToDictionary(x => L(x, "DEFECTID"), x => I(x, "XINDEX"));
                    foreach (DataRow x in Rows(multi, "DEFECT").Where(r => L(r, "STEP_SEQ") == seqs[i]))
                        if (I(x, "SYS_X") != rawIndex[L(x, "DEFECTID")] || I(x, "XINDEX") != I(x, "SYS_X")) sysOk = false;
                    detail.Add("FS Step Die 그대로");
                }
            }
            bool uniqueKeys = Rows(multi, "DEFECT").Select(x => S(x, "STEP_SEQ") + "|" + S(x, "DEFECTID")).Distinct().Count() == multi.Tables["DEFECT"].Rows.Count;
            bool dieSum = Rows(multi, "DIE").Sum(x => I(x, "DEFECT_COUNT")) == I(mi, "DEFECT_COUNT");
            Check("E6 여러 Step(" + seqs.Count + "개) 합치기: Defect / Chain 영역 = Step 별 분석 합 / 키(STEP_SEQ, DEFECTID) / FS Step 시스템 Die = KLARF Index 그대로 / Die 합",
                I(mi, "DEFECT_COUNT") == sumDefects && I(mi, "IN_CHAIN_DEFECT_COUNT") == sumIn && keyOk && sysOk && uniqueKeys && dieSum
                && S(mi, "STEP_SEQS") == string.Join(",", seqs.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToArray()),
                "Defect " + I(mi, "DEFECT_COUNT") + " / 영역 " + I(mi, "IN_CHAIN_DEFECT_COUNT") + " / Die " + multi.Tables["DIE"].Rows.Count + " | " + string.Join(" | ", detail.ToArray())
                + " | 판정 " + S(mi, "MATCH_MS") + "ms 전체 " + S(mi, "ELAPSED_MS") + "ms");
        }
        else
        {
            Check("E6 여러 Step 합치기", false, "추가 Step 이 로컬 DB 에 없음: " + string.Join(",", extra.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToArray()));
        }

        // ===== E7 Die 상세 / 역추적 =====
        DataRow topDie = Rows(same, "DIE").OrderByDescending(x => I(x, "IN_CHAIN_COUNT")).ThenByDescending(x => I(x, "DEFECT_COUNT")).First();
        int dx = I(topDie, "XINDEX"), dy = I(topDie, "YINDEX");
        DataRow topPair = Rows(same, "DIE_CHAIN").Where(x => I(x, "XINDEX") == dx && I(x, "YINDEX") == dy).OrderByDescending(x => I(x, "DEFECT_COUNT")).FirstOrDefault();
        string focus = topPair == null ? null : S(topPair, "CHAIN_SEQ");
        DataSet die = bsl.AnalyzeDie(Req(_step, "BACK", "BACK"), dx, dy, focus);
        DataRow dieInfo = die.Tables["ANL_INFO"].Rows[0];
        int dieHitDefects = Rows(die, "DEFECT").Count(x => S(x, "IN_CHAIN") == "Y");
        int hitRowsDefects = Rows(die, "DEFECT_HIT").Select(x => S(x, "STEP_SEQ") + "|" + S(x, "DEFECTID")).Distinct().Count();
        bool focusGaps = focus == null || Rows(die, "DEFECT_CHAIN").Any(x => S(x, "CHAIN_SEQ") == focus);
        bool nearRows = Rows(die, "DIE_CHAIN").Count() >= Rows(same, "DIE_CHAIN").Count(x => I(x, "XINDEX") == dx && I(x, "YINDEX") == dy);
        bool pairsWithin = Rows(die, "DEFECT_CHAIN").All(x => D(x, "GAP") <= 50);
        Check("E7 Die 상세: Die 1행 / Defect = Wafer 결과 Die Defect 수 / DEFECT_HIT Defect = Chain 영역 / 역추적 Chain 거리 / 탐색 거리 안 (Die,Chain) / ET_VALUE 표",
            die.Tables["DIE"].Rows.Count == 1 && die.Tables["DEFECT"].Rows.Count == I(topDie, "DEFECT_COUNT") && dieHitDefects == I(topDie, "IN_CHAIN_COUNT")
            && hitRowsDefects == dieHitDefects && focusGaps && nearRows && pairsWithin && die.Tables.Contains("ET_VALUE")
            && I(dieInfo, "DIE_X") == dx && I(dieInfo, "DIE_Y") == dy && S(dieInfo, "FOCUS_CHAIN_SEQ") == (focus ?? ""),
            String.Format("Die ({0},{1}) Defect {2} / 영역 {3} / Member 행 {4} / (Die,Chain) {5} / Defect-Chain 쌍 {6} / 역추적 {7} / {8}ms",
                dx, dy, die.Tables["DEFECT"].Rows.Count, dieHitDefects, die.Tables["DEFECT_HIT"].Rows.Count, die.Tables["DIE_CHAIN"].Rows.Count,
                die.Tables["DEFECT_CHAIN"].Rows.Count, focus ?? "-", S(dieInfo, "ELAPSED_MS")));

        // ===== E8 Chain / 분류 거르기 =====
        if (focus != null)
        {
            GdsKlarfAnalysisRequest cReq = Req(_step, "BACK", "BACK");
            cReq.ChainSeqs = new[] { focus };
            DataSet ch = bsl.Analyze(cReq);
            Check("E8 Chain 거르기: Chain 1행 / 쌍은 그 Chain 만", ch.Tables["CHAIN"].Rows.Count == 1 && Rows(ch, "DEFECT_CHAIN").All(x => S(x, "CHAIN_SEQ") == focus)
                && I(ch.Tables["ANL_INFO"].Rows[0], "IN_CHAIN_DEFECT_COUNT") == Rows(same, "DEFECT_CHAIN").Where(x => S(x, "CHAIN_SEQ") == focus).Select(x => S(x, "DEFECTID")).Distinct().Count(),
                "Chain " + focus + " / 영역 " + I(ch.Tables["ANL_INFO"].Rows[0], "IN_CHAIN_DEFECT_COUNT"));
        }
        DataRow firstDefect = same.Tables["DEFECT"].Rows[0];
        int rb = I(firstDefect, "ROUGHBINNUMBER"), cl = I(firstDefect, "CLASSNUMBER");
        GdsKlarfAnalysisRequest rbReq = Req(_step, "BACK", "BACK");
        rbReq.ClassValues = new[] { rb };
        GdsKlarfAnalysisRequest clReq = Req(_step, "BACK", "BACK");
        clReq.ClassBasis = GdsKlarfAnalysisRequest.ClassBasisClass;
        clReq.ClassValues = new[] { cl };
        int rbCount = I(bsl.Analyze(rbReq).Tables["ANL_INFO"].Rows[0], "DEFECT_COUNT");
        int clCount = I(bsl.Analyze(clReq).Tables["ANL_INFO"].Rows[0], "DEFECT_COUNT");
        Check("E9 분류 거르기: RoughBin 기준 / Class 기준 각각 그 값 Defect 수",
            rbCount == Rows(same, "DEFECT").Count(x => I(x, "ROUGHBINNUMBER") == rb) && clCount == Rows(same, "DEFECT").Count(x => I(x, "CLASSNUMBER") == cl),
            "RoughBin " + rb + " -> " + rbCount + " / Class " + cl + " -> " + clCount);

        // ===== E10 큰 Defect 제외 (기준 1,000 µm) =====
        long bigStep = seqs.Count > 1 ? seqs.Last() : _step;
        DataSet big = bsl.Analyze(Req(bigStep, "BACK", "BACK"));
        GdsKlarfAnalysisRequest exReq = Req(bigStep, "BACK", "BACK");
        exReq.ExcludeLargeDefect = true;
        DataSet ex = bsl.Analyze(exReq);
        DataRow bi = big.Tables["ANL_INFO"].Rows[0], ei = ex.Tables["ANL_INFO"].Rows[0];
        int largeRows = Rows(big, "DEFECT").Count(x => S(x, "LARGE") == "Y");
        bool largeOk = Rows(big, "DEFECT").All(x => (S(x, "LARGE") == "Y") == (Math.Max(Math.Abs(D(x, "XSIZE")), Math.Abs(D(x, "YSIZE"))) >= 1000));
        Check("E10 큰 Defect: LARGE 표시(한 변 1,000 µm 이상) / 제외하면 그만큼 빠짐",
            largeOk && largeRows == I(bi, "LARGE_DEFECT_COUNT") && I(ei, "LARGE_EXCLUDED_COUNT") == I(bi, "LARGE_DEFECT_COUNT")
            && I(ei, "DEFECT_COUNT") == I(bi, "DEFECT_COUNT") - I(bi, "LARGE_DEFECT_COUNT") && Rows(ex, "DEFECT").All(x => S(x, "LARGE") != "Y"),
            "STEP " + bigStep + " 큰 Defect " + I(bi, "LARGE_DEFECT_COUNT") + " / " + I(bi, "DEFECT_COUNT") + " -> " + I(ei, "DEFECT_COUNT"));

        // ===== E11 Cache / 직렬화 =====
        watch.Restart();
        DataSet again = bsl.Analyze(Req(_step, "BACK", "FRONT"));
        long againMs = watch.ElapsedMilliseconds;
        DataRow ai = again.Tables["ANL_INFO"].Rows[0];
        // 직렬화: 서버는 RemotingFormat = Binary 로 돌려준다(받는 쪽 속성은 기본값으로 돌아오므로 크기로 확인). Defect 가 많은 결과(E10)로 잰다.
        long binBytes, xmlBytes;
        DataSet copy = RoundTrip(big, out binBytes);
        bool binary = big.RemotingFormat == SerializationFormat.Binary;
        big.RemotingFormat = SerializationFormat.Xml;
        RoundTrip(big, out xmlBytes);
        Check("E11 두 번째 분석은 GDS Cache / 결과 Binary 직렬화(Xml 보다 작음) 왕복 같음",
            S(info, "CACHE_HIT") == "N" && S(ai, "CACHE_HIT") == "Y" && binary && binBytes < xmlBytes && copy.Tables.Count == big.Tables.Count
            && copy.Tables["DEFECT"].Rows.Count == big.Tables["DEFECT"].Rows.Count && copy.Tables["DIE"].Rows.Count == big.Tables["DIE"].Rows.Count,
            String.Format("첫 GDS {0}ms / 두 번째 GDS {1}ms, 전체 {2}ms / STEP {3} 결과 Binary {4:N0} B, Xml {5:N0} B",
                S(info, "GDS_MS"), S(ai, "GDS_MS"), againMs, bigStep, binBytes, xmlBytes));

        // ===== E12 오류 안내 =====
        var errors = new List<string>();
        GdsKlarfAnalysisRequest e;
        e = Req(_step, "BACK", "BACK"); e.StepSeqs = new long[0]; errors.Add(Throws(() => bsl.Analyze(e)));
        e = Req(_step, "BACK", "BACK"); e.DisplayStepSeq = 999999999; errors.Add(Throws(() => bsl.Analyze(e)));
        e = Req(_step, "BACK", "BACK"); e.AreaMode = GdsKlarfAnalysisRequest.AreaSquare; e.SquareUm = 0; errors.Add(Throws(() => bsl.Analyze(e)));
        e = Req(_step, "BACK", "BACK"); e.ToleranceUm = -1; errors.Add(Throws(() => bsl.Analyze(e)));
        e = Req(_step, "BACK", "BACK"); e.DeviceId = "NO_SUCH_DEVICE_PROBE"; errors.Add(Throws(() => bsl.Analyze(e)));
        e = Req(999999999, "BACK", "BACK"); errors.Add(Throws(() => bsl.Analyze(e)));
        e = Req(_step, "BACK", "BACK"); e.ViewSides = new[] { "BACK", "FRONT" }; errors.Add(Throws(() => bsl.Analyze(e)));
        string[] expect = { "1개 이상", "DisplayStepSeq", "한 변", "허용오차", "READY", "KLARF Wafer", "검사 면" };
        bool all = true;
        for (int i = 0; i < expect.Length; i++) if (!errors[i].Contains(expect[i])) all = false;
        Check("E12 오류 안내 7가지", all, string.Join(" | ", errors.ToArray()));

        // ===== E13 KLARF 원본 Pitch 기록 / 읽기 (결정 B6): 업로드 COMMENT 만들기 -> 분석 / Ready 가 먼저 쓰는 값 =====
        // 로컬 STEP 은 기록 전에 올린 것이라 DB 로는 확인할 수 없어 두 함수를 직접 부른다(업로드 Probe 는 사용자 데이터 보호로 같은 샘플을 다시 올리지 않음).
        const BindingFlags priv = BindingFlags.NonPublic | BindingFlags.Static;
        Type uploadType = typeof(KlarfUpload);
        Type settingType = uploadType.GetNestedType("UploadSetting", BindingFlags.NonPublic);
        object setting = Activator.CreateInstance(settingType, true);
        settingType.GetField("User").SetValue(setting, "PROBE");
        settingType.GetField("FileName").SetValue(setting, new string('F', 600));
        string comment = (string)uploadType.GetMethod("BuildStepComment", priv).Invoke(null, new object[] { setting, "XD106.02", 23832.0, 30060.1 });
        MethodInfo tryRaw = typeof(GdsKlarfAnalysis).GetMethod("TryRawPitch", priv);
        object[] a1 = { comment, 0.0, 0.0 }, a2 = { "UPLOAD;USER=X;FILE=Y;RAW_WAFER_ID=Z", 0.0, 0.0 }, a3 = { "UPLOAD;RAW_DIE_PITCH=0,30060", 0.0, 0.0 };
        bool r1 = (bool)tryRaw.Invoke(null, a1), r2 = (bool)tryRaw.Invoke(null, a2), r3 = (bool)tryRaw.Invoke(null, a3);
        Check("E13 업로드 COMMENT 에 원본 Pitch(파일 이름이 길어도 500자 안, 원본 WaferID 유지) / 분석이 읽는 값 / 기록 없음·0 은 Map Setup",
            comment.StartsWith("UPLOAD;USER=PROBE;FILE=") && comment.Length <= 500 && comment.EndsWith(";RAW_DIE_PITCH=23832,30060.1;RAW_WAFER_ID=XD106.02")
            && r1 && (double)a1[1] == 23832 && (double)a1[2] == 30060.1 && !r2 && !r3,
            "길이 " + comment.Length + " / 끝 " + comment.Substring(comment.Length - 50) + " / 읽기 " + a1[1] + "," + a1[2] + " / 기록 없음 " + r2 + " / 0 " + r3);
        GdsKlarfGeometryCache.Clear();
    }

    /// <summary>같은 Cache 색인 / 같은 Defect 로 엔진을 직접 불러 Chain 영역 수를 센다(서비스 집계와 별개 경로).</summary>
    private static int DirectInChain(DataRow info, bool mirror, double square = 0)
    {
        DataTable map = new DACrux.SEMDMS.DSL.TQP_GDS_MAP().GetData("SELECT_MAP_INFO", null, new[] { _factory, _device });
        DataTable chains = new DACrux.SEMDMS.DSL.TQP_GDS_CHAIN().GetData("SELECT_CHAIN_LIST", null, new[] { _factory, _device });
        List<DataRow> ready = chains.Rows.Cast<DataRow>().Where(r => Convert.ToString(r["VERSION_STATUS"]) == "READY").ToList();
        bool hit;
        long ms;
        GdsKlarfShapeIndex index = GdsKlarfGeometryCache.GetOrLoad(_factory, _device, GdsKlarfGeometryCache.MakeStamp(map.Rows[0], ready),
            new HashSet<long>(ready.Select(r => Convert.ToInt64(r["CHAIN_SEQ"]))),
            () => new DACrux.SEMDMS.DSL.TQP_GDS_CHAIN_MEMBER().GetData("SELECT_CHAIN_MEMBER_SHAPE_ALL", null, new[] { _factory, _device }), out hit, out ms);
        var defects = new DACrux.SEMDMS.DSL.TQD_DEFECT().GetDefect(_step).Rows.Cast<DataRow>().Select(r => new GdsKlarfDefectPoint
        {
            DefectId = Convert.ToInt64(r["DEFECTID"]),
            XRel = Convert.ToDouble(r["XREL"]),
            YRel = Convert.ToDouble(r["YREL"]),
            XSize = Convert.ToDouble(r["XSIZE"]),
            YSize = Convert.ToDouble(r["YSIZE"])
        }).ToList();
        var o = new GdsKlarfMatchOptions { PitchX = D(info, "PITCH_X"), PitchY = D(info, "PITCH_Y"), Tolerance = 5, UseDefectSize = true, Mirror = mirror, SquareSide = square };
        return GdsKlarfMatchEngine.MatchShapes(index, defects, o, null).Count(g => g.MinGap <= o.HitGap);
    }

    private static GdsKlarfAnalysisRequest Req(long step, string stepSide, string chainSide, double tol = 5)
    {
        return new GdsKlarfAnalysisRequest
        {
            Factory = _factory,
            DeviceId = _device,
            WaferKey = "PROBE-KEY",
            StepSeqs = new[] { step },
            ViewSides = new[] { stepSide },
            ChainSide = chainSide,
            DisplayStepSeq = step,
            AreaMode = GdsKlarfAnalysisRequest.AreaSizeTolerance,
            ToleranceUm = tol,
            ClassBasis = GdsKlarfAnalysisRequest.ClassBasisRoughBin
        };
    }

    private static DataSet RoundTrip(DataSet ds, out long bytes)
    {
        var f = new BinaryFormatter();
        using (var m = new MemoryStream())
        {
            f.Serialize(m, ds);
            bytes = m.Length;
            m.Position = 0;
            return (DataSet)f.Deserialize(m);
        }
    }

    private static void Missing(DataSet ds, string table, string[] cols, List<string> missing)
    {
        if (!ds.Tables.Contains(table)) return;
        foreach (string c in cols) if (!ds.Tables[table].Columns.Contains(c)) missing.Add(table + "." + c);
    }

    private static string Summary(DataRow i)
    {
        return String.Format("Defect {0} / Chain 영역 {1} / Chain {2} / Member {3:N0} / Pitch {4} / 조회 {5}ms GDS {6}ms 판정 {7}ms 결과 {8}ms 전체 {9}ms",
            S(i, "DEFECT_COUNT"), S(i, "IN_CHAIN_DEFECT_COUNT"), S(i, "CHAIN_COUNT"), I(i, "MEMBER_COUNT"), S(i, "PITCH_SOURCE"),
            S(i, "QUERY_MS"), S(i, "GDS_MS"), S(i, "MATCH_MS"), S(i, "RESULT_MS"), S(i, "ELAPSED_MS"));
    }

    private static IEnumerable<DataRow> Rows(DataSet ds, string table)
    {
        return ds.Tables.Contains(table) ? ds.Tables[table].Rows.Cast<DataRow>() : Enumerable.Empty<DataRow>();
    }

    private static string S(DataRow r, string c) { return r[c] == DBNull.Value ? "" : Convert.ToString(r[c], CultureInfo.InvariantCulture); }

    private static int I(DataRow r, string c) { return r[c] == DBNull.Value ? 0 : Convert.ToInt32(r[c], CultureInfo.InvariantCulture); }

    private static long L(DataRow r, string c) { return r[c] == DBNull.Value ? 0 : Convert.ToInt64(r[c], CultureInfo.InvariantCulture); }

    private static double D(DataRow r, string c) { return r[c] == DBNull.Value ? double.NaN : Convert.ToDouble(r[c], CultureInfo.InvariantCulture); }

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
            return ex.Message.Split('\n')[0].Trim();
        }
    }
}
