// 연관 분석 3 / 4단계 확인: Wafer Key / Ready 조회(GetWaferReady) / E-Test 판정 연결(Analyze / AnalyzeDie)
// 설계: 문서/설계/05_연관분석/2026-10-15_GDS_KLARF_ETest_통합분석_데이터_설계.md 6 / 7 / 15.3 / 16절
// 빌드 / 실행은 GdsKlarfEtProbe.ps1 이 한다(시험 Map PROBE / OMM_Hybrid 를 넣고 끝나면 지운다).
// 시험 데이터: FACTORY=PROBE / E-Test Device OMM_Hybrid / TSVET 등록 Parameter 6개(SCK 등록 헤더 원문 사용) + GDS Chain + Spec,
//             E-Test 파일 1개(Lot RH-HS2615 Slot 2 = KLARF STEP 528 과 같은 Wafer Key, Die = OMM_Hybrid Map 사용 Die 전부). 끝나면 지운다.
// 값은 Die / Chain 마다 기대 결과가 정해지도록 만든다:
//  Die T1(Chain A 에 Defect 가 걸린 Die) : A 의 Parameter 1 = 150 (Spec 0 ~ 100 밖 -> F)  => (T1, A) = 일치(BOTH)
//  Die T2(Chain B 에 Defect 없음)         : B 의 Parameter = '-' (미측정 -> F)            => (T2, B) = Fail만(FAIL), 첫 Fail 값 '-'
//  Die T3                                 : C 의 Parameter = 빈 값(판정 없음)              => (T3, C) = N -> 비교 불가(NA)
//  나머지 값은 50 (P). Chain A 에 Defect 가 걸린 다른 Die 는 Defect만(DEFECT).
//  결과 파일 DUT Flag(PASS_FLAG)는 T3 만 F, 나머지 P (Spec 판정과 별개인 DIE.ET_FILE_FLAG 확인, 2026-10-19).
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using DACrux.SEMDMS.BSL;
using DACrux.SEMDMS.Interface;

internal static class GdsKlarfEtProbe
{
    private const string EtFactory = "PROBE";
    private const string EtDevice = "OMM_Hybrid";
    private const string Program = "TSVET";
    private const string Lot = "RH-HS2615";
    private const string User = "PROBE";
    private static int _pass, _fail;
    private static string _factory, _device;
    private static long _step;

    private static int Main(string[] args)
    {
        _factory = args.Length > 0 ? args[0] : "SCK";
        _device = args.Length > 1 ? args[1] : "OMM_Hybrid";
        _step = args.Length > 2 ? Int64.Parse(args[2], CultureInfo.InvariantCulture) : 528;
        var et = new GdsEtResult();
        try
        {
            KeyChecks();
            StepRuleChecks();
            ReadyChecks();
            Cleanup(et);
            EtChecks(et);
        }
        catch (Exception ex)
        {
            Check("예외 없이 끝까지", false, ex.ToString());
        }
        finally
        {
            try { Cleanup(et); Console.WriteLine("정리: PROBE 시험 E-Test 파일 / 등록 Parameter 삭제"); }
            catch (Exception ex) { Check("시험 데이터 정리", false, ex.Message); }
        }
        Console.WriteLine();
        Console.WriteLine("GDS_KLARF_ET_PROBE=" + (_fail == 0 ? "PASS" : "FAIL") + " (" + _pass + "/" + (_pass + _fail) + ")");
        return _fail == 0 ? 0 : 1;
    }

    // ===== R1 Wafer Key 규칙 (설계 16.2절 샘플 기대값) =====
    private static void KeyChecks()
    {
        string k1 = WaferKeyRule.FromKlarf("PFIC_RH-HS2615_C4Bump", "", "2", "");
        string e1 = WaferKeyRule.FromETest("RH-HS2615", "2", "");
        string k2 = WaferKeyRule.FromKlarf("PFIC_U9-FS2635_C4Bump", "", "1", "");
        string e2 = WaferKeyRule.FromETest("U9-HS2635", "1", "");
        string k3 = WaferKeyRule.FromKlarf("HS2617B_TBDB", "", "01", "");
        WaferKeySearch s = WaferKeyRule.SearchOf(k1);
        Check("R1 Wafer Key 잠정 규칙: 1쌍 맞음 / 2쌍 다름(직접 찾기 대상) / 2조각 Lot 전체 / Slot 없음 null / 후보 조건 / 잠정",
            k1 == "RH-HS2615-02" && e1 == k1 && k2 == "U9-FS2635-01" && e2 == "U9-HS2635-01" && k2 != e2 && k3 == "HS2617B_TBDB-01"
            && WaferKeyRule.FromKlarf("A_B_C", "", "", "") == null && WaferKeyRule.FromETest("", "1", "") == null
            && s != null && s.LotLike == "%RH-HS2615%" && s.Slot == "2" && WaferKeyRule.SearchOf("NOSLOT") == null && !WaferKeyRule.Confirmed,
            k1 + " = " + e1 + " / " + k2 + " ≠ " + e2 + " / " + k3 + " / " + (s == null ? "-" : s.LotLike + "," + s.Slot));
    }

    // ===== R2 ~ R4 Ready 조회 (SCK, E-Test 시험 데이터 없이) =====
    private static void ReadyChecks()
    {
        var bsl = new GdsKlarfAnalysis();
        GdsKlarfGeometryCache.Clear();
        DataSet ready = bsl.GetWaferReady(_factory, _step, "");
        DataRow key = ready.Tables["WAFER_KEY"].Rows[0], gds = ready.Tables["READY_GDS"].Rows[0];
        List<DataRow> steps = Rows(ready, "READY_KLARF").ToList();
        string[] codes = Rows(ready, "READY_CHECK").Select(r => S(r, "CODE")).ToArray();
        bool tables = new[] { "WAFER_KEY", "READY_GDS", "READY_KLARF", "READY_ET", "READY_CHECK" }.All(t => ready.Tables.Contains(t))
            && ready.Tables.Cast<DataTable>().All(t => t.Columns.Cast<DataColumn>().All(c => c.DataType == typeof(string)));
        DataRow picked = steps.FirstOrDefault(r => S(r, "STEP_SEQ") == _step.ToString(CultureInfo.InvariantCulture));
        Check("R2 Ready(STEP " + _step + "): 표 5개(문자열 열) / Wafer Key / GDS = Product 같은 이름 READY + Chain 면 BS(2026-10-20 기본 변경) / 고른 Step 사용 · 검사 면 FS(2026-10-20 고정) / 점검(규칙 잠정 · Pitch 출처)",
            tables && S(key, "WAFER_KEY") == "RH-HS2615-02" && S(key, "CONFIRMED") == "N" && S(key, "RULE_NAME") == WaferKeyRule.RuleName
            && S(gds, "DEVICE_ID") == _device && S(gds, "MAP_STATUS") == "READY" && S(gds, "READY_CHAIN") == "15" && S(gds, "CHAIN_SIDE") == "BACK"
            && picked != null && S(picked, "USE_FLAG") == "Y" && S(picked, "VIEW_SIDE") == "FRONT" && S(picked, "PITCH_SOURCE") == "MAP_SETUP"
            && codes.Contains("RULE_TENTATIVE") && codes.Contains("PITCH_SOURCE") && !codes.Contains("GDS_NO_MAP"),
            String.Format("Key {0} / GDS {1} {2} Chain {3} DRAFT {4} Cache {5} / Step {6}개 [{7}] / E-Test {8}개 / 점검 {9}",
                S(key, "WAFER_KEY"), S(gds, "DEVICE_ID"), S(gds, "MAP_STATUS"), S(gds, "READY_CHAIN"), S(gds, "DRAFT_CHAIN"), S(gds, "CACHE_HIT"),
                steps.Count, string.Join(",", steps.Select(r => S(r, "STEP_SEQ") + ":" + S(r, "STEP_ID")).ToArray()), ready.Tables["READY_ET"].Rows.Count,
                string.Join(",", codes)));

        // Cache 여부: 분석 뒤 다시 Ready 하면 Y
        DataSet sideBase = bsl.Analyze(Req(_step, 0));
        string cacheAfter = S(bsl.GetWaferReady(_factory, _step, "").Tables["READY_GDS"].Rows[0], "CACHE_HIT");
        Check("R3 Ready 의 Cache 여부: 분석 전 N -> 분석 뒤 Y", S(gds, "CACHE_HIT") == "N" && cacheAfter == "Y", S(gds, "CACHE_HIT") + " -> " + cacheAfter);
        // 요청 Chain 면 / 검사 면이 비면 Chain BS(2026-10-20, 이전 FS) · 검사 FS(2026-10-20, 이전 BS) -> 반전.
        // 검사 면은 반전에만 쓰고 KLARF Die 좌표는 그대로라 (Index, 시스템 Die) 는 검사 면 BS 로 분석한 결과와 같다
        GdsKlarfAnalysisRequest noSide = Req(_step, 0);
        noSide.ChainSide = null;
        noSide.ViewSides = null;
        DataSet nsDs = bsl.Analyze(noSide);
        DataRow ns = nsDs.Tables["ANL_INFO"].Rows[0];
        Func<DataSet, string> dieKeys = d => string.Join(";", Rows(d, "DIE").Select(r => S(r, "XINDEX") + "," + S(r, "YINDEX") + "=" + S(r, "SYS_X") + "," + S(r, "SYS_Y"))
            .OrderBy(x => x, StringComparer.Ordinal).ToArray());
        bool sameDie = dieKeys(nsDs) == dieKeys(sideBase);
        Check("R3-1 Analyze 요청 Chain 면 · 검사 면이 비면 Chain BS · 검사 FS -> 반전 / KLARF Die 좌표는 검사 면 BS 결과와 같음(그대로)",
            S(ns, "CHAIN_SIDE") == "BACK" && S(ns, "STEP_SIDES") == "FRONT" && S(ns, "MIRROR") == "Y" && sameDie,
            "CHAIN_SIDE " + S(ns, "CHAIN_SIDE") + " / 검사 " + S(ns, "STEP_SIDES") + " / 반전 " + S(ns, "MIRROR") + " / Chain In " + S(ns, "IN_CHAIN_DEFECT_COUNT")
            + " / Die " + nsDs.Tables["DIE"].Rows.Count + " 같음 " + sameDie);

        DataSet noMap = bsl.GetWaferReady(_factory, _step, "NO_SUCH_DEVICE_PROBE");
        string e1 = Throws(() => bsl.GetWaferReady(_factory, 999999999, ""));
        string e2 = Throws(() => bsl.GetWaferReady("", _step, ""));
        Check("R4 Ready 오류: 없는 Device = 점검 오류(GDS_NO_MAP) / 없는 Step / Factory 빈 값",
            Rows(noMap, "READY_CHECK").Any(r => S(r, "CODE") == "GDS_NO_MAP" && S(r, "LEVEL") == "오류") && e1.Contains("KLARF Wafer") && e2.Contains("Factory"),
            S(noMap.Tables["READY_GDS"].Rows[0], "MAP_STATUS") + " | " + e1 + " | " + e2);

        // R5 같은 Product 만 (결정 B26, 2026-10-16): 같은 KLARF(1107236219)를 Product 만 바꿔 두 번 올린 Step(OMM_TCB / OMMTCB)
        // Step 번호는 재업로드 때 바뀌므로(2026-10-19 OMM_TCB 541 -> 602) Lot / Slot / Product 로 찾는다 (2026-10-20)
        DataTable u9 = new DACrux.SEMDMS.DSL.TQD_INSP_INFO().GetData("SELECT_KLARF_STEP_BY_WAFER", null,
            new[] { _factory, "%U9-FS2635%", "1", "20000101000000", "21000101000000" });
        long stepTcb = u9.Rows.Cast<DataRow>().Where(r => S(r, "PRODUCT") == "OMM_TCB").Select(r => L(r, "STEP_SEQ")).DefaultIfEmpty(0).Max();
        long stepTcb2 = u9.Rows.Cast<DataRow>().Where(r => S(r, "PRODUCT") == "OMMTCB").Select(r => L(r, "STEP_SEQ")).DefaultIfEmpty(0).Max();
        string[] fromA = stepTcb == 0 ? new string[0]
            : Rows(bsl.GetWaferReady(_factory, stepTcb, ""), "READY_KLARF").Select(r => S(r, "STEP_SEQ") + ":" + S(r, "DEFECTS") + ":" + S(r, "USE_FLAG")).ToArray();
        string[] fromB = stepTcb2 == 0 ? new string[0]
            : Rows(bsl.GetWaferReady(_factory, stepTcb2, ""), "READY_KLARF").Select(r => S(r, "STEP_SEQ") + ":" + S(r, "DEFECTS") + ":" + S(r, "USE_FLAG")).ToArray();
        Check("R5 Ready KLARF 는 고른 Step 과 같은 Product 만: OMM_TCB Step -> 그 Step 한 줄 / OMMTCB Step -> 그 Step 한 줄 (Defect 1122, 사용 Y)",
            stepTcb > 0 && stepTcb2 > 0 && fromA.Length == 1 && fromA[0] == stepTcb + ":1122:Y" && fromB.Length == 1 && fromB[0] == stepTcb2 + ":1122:Y",
            "OMM_TCB " + stepTcb + " -> [" + string.Join(",", fromA) + "] / OMMTCB " + stepTcb2 + " -> [" + string.Join(",", fromB) + "]");
    }

    // ===== R6 같은 Step 재검사 정리 (결정 B27, 2026-10-16) - DB 없이 규칙만 =====
    private static void StepRuleChecks()
    {
        Func<long, string, int?, string, GdsKlarfReadyStepRule.Entry> e = (seq, id, order, time) =>
            new GdsKlarfReadyStepRule.Entry(seq, id, order, "2026-10-16 " + time, new object[10]);
        Func<List<GdsKlarfReadyStepRule.Entry>, string> text = list =>
            string.Join(",", list.Select(x => x.StepSeq + ":" + x.AddedBy + ":" + x.UseFlag).ToArray());
        var input = new List<GdsKlarfReadyStepRule.Entry> { e(10, "STEP_A", 2, "09:00:00"), e(12, "STEP_B", 1, "08:00:00"), e(11, "STEP_A", 1, "10:00:00") };
        string latest = text(GdsKlarfReadyStepRule.Arrange(input, 12));
        string pickedOld = text(GdsKlarfReadyStepRule.Arrange(input, 10));
        string noOrder = text(GdsKlarfReadyStepRule.Arrange(new List<GdsKlarfReadyStepRule.Entry> { e(20, "STEP_C", null, "07:00:00"), e(21, "step_c ", null, "09:00:00") }, 99));
        bool parse = GdsKlarfReadyStepRule.ParseOrder("1") == 1 && GdsKlarfReadyStepRule.ParseOrder(2m) == 2
            && GdsKlarfReadyStepRule.ParseOrder(DBNull.Value) == null && GdsKlarfReadyStepRule.ParseOrder("x") == null && GdsKlarfReadyStepRule.ParseOrder(null) == null;
        Check("R6 재검사 정리: Step ID 순 / 같은 Step ID 는 최신(TEST_ORDER 작은 값)만 사용, 이전 검사 PREV / 고른 Step 이 예전 검사면 그것 사용, 최신은 NEXT / TEST_ORDER 없으면 늦은 시각 / Step ID 대소문자, 공백 무시 / 값 해석",
            latest == "11:RULE:Y,10:PREV:N,12:RULE:Y" && pickedOld == "11:NEXT:N,10:RULE:Y,12:RULE:Y" && noOrder == "21:RULE:Y,20:PREV:N" && parse
            && input.Select(x => x.StepSeq).SequenceEqual(new long[] { 10, 12, 11 }),
            latest + " | " + pickedOld + " | " + noOrder + " | 값 해석 " + parse);
    }

    // ===== T E-Test 판정 연결 =====
    private static void EtChecks(GdsEtResult et)
    {
        var bsl = new GdsKlarfAnalysis();
        // 기준: E-Test 없이 (BS, BS) 분석 -> Chain A = Defect 가 가장 많이 걸린 Chain, B / C = 걸린 Defect 가 없는 Chain
        DataSet baseDs = bsl.Analyze(Req(_step, 0));
        List<DataRow> chains = Rows(baseDs, "CHAIN").ToList();
        DataRow chainA = chains.OrderByDescending(r => I(r, "HIT_DEFECT_COUNT")).First();
        List<DataRow> clean = chains.Where(r => I(r, "HIT_DEFECT_COUNT") == 0).Take(2).ToList();
        if (I(chainA, "HIT_DEFECT_COUNT") == 0 || clean.Count < 2) throw new InvalidOperationException("Chain 영역 Defect 가 있는 Chain / 없는 Chain 2개가 필요합니다.");
        DataRow chainB = clean[0], chainC = clean[1];
        long seqA = L(chainA, "CHAIN_SEQ"), seqB = L(chainB, "CHAIN_SEQ"), seqC = L(chainC, "CHAIN_SEQ");
        List<DataRow> aDies = Rows(baseDs, "DIE_CHAIN").Where(r => L(r, "CHAIN_SEQ") == seqA).ToList();
        DataRow t1Row = aDies.First();
        var t1 = new DiePoint(I(t1Row, "SYS_X"), I(t1Row, "SYS_Y"));
        var aHitDies = new HashSet<DiePoint>(aDies.Select(r => new DiePoint(I(r, "SYS_X"), I(r, "SYS_Y"))));
        var bHitDies = new HashSet<DiePoint>(Rows(baseDs, "DIE_CHAIN").Where(r => L(r, "CHAIN_SEQ") == seqB).Select(r => new DiePoint(I(r, "SYS_X"), I(r, "SYS_Y"))));
        var inspected = new HashSet<DiePoint>(Rows(baseDs, "DIE").Where(r => S(r, "IN_PLAN") == "Y" || I(r, "DEFECT_COUNT") > 0).Select(r => new DiePoint(I(r, "SYS_X"), I(r, "SYS_Y"))));

        // 시험 Parameter 6개: SCK 에 등록된 헤더 원문을 쓴다(이름 분해 규칙 통과 보장)
        DataTable source = null;
        foreach (DataRow p in et.GetEtParaProgramList(_factory).Rows)
        {
            DataTable list = et.GetEtParameterList(_factory, S(p, "DEVICE_ID"), S(p, "TEST_PROGRAM"));
            if (list.Rows.Count >= 6) { source = list; break; }
        }
        if (source == null) throw new InvalidOperationException(_factory + " 에 등록 Parameter 6개 이상인 Device + 공정이 없습니다.");
        List<DataRow> picks = source.Rows.Cast<DataRow>().GroupBy(r => S(r, "ITEM_NAME")).Select(g => g.First()).Take(6).ToList();
        var reg = new string[6, 4];
        for (int i = 0; i < 6; i++) { reg[i, 0] = S(picks[i], "HEADER_TEXT"); reg[i, 1] = S(picks[i], "UNIT"); reg[i, 2] = ""; reg[i, 3] = (i + 1).ToString(CultureInfo.InvariantCulture); }
        et.RegisterEtParameters(new[] { EtFactory, EtDevice, Program, "SPEC", "" }, reg, User);
        List<DataRow> paras = et.GetEtParameterList(EtFactory, EtDevice, Program).Rows.Cast<DataRow>().OrderBy(r => I(r, "DISPLAY_ORDER")).ThenBy(r => S(r, "ITEM_NAME")).ToList();
        if (paras.Count != 6) throw new InvalidOperationException("시험 Parameter 등록 수가 6 이 아닙니다: " + paras.Count);
        // GDS Chain: P1 / P2 = A(소문자 + 공백, 이름 비교는 UPPER(TRIM)), P3 = B, P4 = C, P5 = GDS 에 없는 이름, P6 = 없음
        string nameA = S(chainA, "CHAIN_NAME");
        string[] gdsNames = { "  " + nameA.ToLowerInvariant() + " ", nameA, S(chainB, "CHAIN_NAME"), S(chainC, "CHAIN_NAME"), "ZZ_PROBE_NOT_IN_GDS", "" };
        var setup = new string[6, 7];
        for (int i = 0; i < 6; i++) { setup[i, 0] = S(paras[i], "ITEM_NAME"); setup[i, 1] = "Y"; setup[i, 2] = ""; setup[i, 3] = gdsNames[i]; setup[i, 4] = "0"; setup[i, 5] = "100"; setup[i, 6] = "Y"; }
        et.SaveEtParameterSetup(new[] { EtFactory, EtDevice, Program }, setup, User);

        // Die = SCK OMM_Hybrid Map 사용 Die 전부, E-Test 좌표는 DieCoordinateMapper.ETest 역변환
        DataTable mapDies = et.GetEtMapDieList(_factory, _device);
        DieCoordinateMapper mapper = DieCoordinateMapper.FromDieTable(mapDies, new DiePoint(0, 0));
        List<DiePoint> used = mapDies.Rows.Cast<DataRow>().Where(r => I(r, "USECODE") == 1).Select(r => new DiePoint(I(r, "X"), I(r, "Y"))).Distinct().ToList();
        DiePoint t2 = used.First(p => !p.Equals(t1) && !bHitDies.Contains(p) && inspected.Contains(p));
        DiePoint t3 = used.First(p => !p.Equals(t1) && !p.Equals(t2) && inspected.Contains(p));
        var dies = new string[used.Count, 10];
        var values = new string[used.Count * 6, 4];
        int v = 0;
        for (int i = 0; i < used.Count; i++)
        {
            DiePoint e = mapper.FromSystem(DieCoordType.ETest, used[i]);
            string dut = (i + 1).ToString(CultureInfo.InvariantCulture);
            string[] dieRow = { dut, e.X.ToString(CultureInfo.InvariantCulture), e.Y.ToString(CultureInfo.InvariantCulture), "1", "1", used[i].Equals(t3) ? "F" : "P", "", "0", "6", "1" };
            for (int c = 0; c < 10; c++) dies[i, c] = dieRow[c];
            for (int p = 0; p < 6; p++)
            {
                string value = "50", status = "V";
                if (used[i].Equals(t1) && p == 0) value = "150";
                if (used[i].Equals(t2) && p == 2) { value = ""; status = "N"; }
                if (used[i].Equals(t3) && p == 3) { value = ""; status = "E"; }
                values[v, 0] = dut; values[v, 1] = S(paras[p], "ET_ITEM_SEQ"); values[v, 2] = value; values[v, 3] = status;
                v++;
            }
        }
        DataRow head = et.BeginEtFile(new[] { EtFactory, EtDevice, "PROBE_GDS_KLARF_ET.xlsx", Lot, "2", Program, "20261016100000", "", "PROBE",
            used.Count.ToString(CultureInfo.InvariantCulture), "6", User, "PROBE", EtDevice, "W", "A0", Lot + "-02", "", "6", "6", "0" }).Rows[0];
        string fileSeq = S(head, "SRC_FILE_SEQ");
        et.CreateEtDieBatch(fileSeq, dies, User);
        et.CreateEtValueBatch(fileSeq, values, User);
        DataRow done = et.CompleteEtFile(fileSeq, User).Rows[0];
        Check("T0 시험 E-Test 파일 저장 확정 / Map 좌표 / Wafer Key 같음",
            S(done, "RESULT") == "OK" && S(done, "MAP_ID") == EtDevice && S(done, "OUT_OF_MAP") == "0" && S(head, "WAFER_ID") == "RH-HS2615-02",
            "SRC_FILE_SEQ " + fileSeq + " / " + S(done, "RESULT") + " / Map " + S(done, "MAP_ID") + " Die " + S(done, "MAP_DIE") + " 밖 " + S(done, "OUT_OF_MAP")
            + " / Fail Die " + S(done, "SPEC_FAIL_DIE") + " / Chain A " + S(chainA, "CHAIN_CODE") + " B " + S(chainB, "CHAIN_CODE") + " C " + S(chainC, "CHAIN_CODE")
            + " / T1 " + t1 + " T2 " + t2 + " T3 " + t3);

        // Analyze + E-Test
        DataSet ds = bsl.Analyze(Req(_step, long.Parse(fileSeq, CultureInfo.InvariantCulture)));
        DataRow info = ds.Tables["ANL_INFO"].Rows[0];
        Check("T1 ANL_INFO E-Test: Wafer / 공정 / 판정 시각 / Die 수 / Map 밖 0 / Map 형상 같음",
            S(info, "ET_SRC_FILE_SEQ") == fileSeq && S(info, "ET_WAFER_ID") == "RH-HS2615-02" && S(info, "ET_TEST_PROGRAM") == Program && S(info, "ET_JUDGE_TIME").Length > 0
            && I(info, "ET_DIE_COUNT") == used.Count && I(info, "ET_OUT_OF_MAP_COUNT") == 0 && S(info, "MAP_SHAPE_SAME") == "Y" && S(info, "ET_SPEC_CHANGED") == "N",
            String.Format("{0} {1} 판정 {2} / Die {3} 밖 {4} / 형상 {5} / E-Test {6}ms", S(info, "ET_WAFER_ID"), S(info, "ET_TEST_PROGRAM"), S(info, "ET_JUDGE_TIME"),
                S(info, "ET_DIE_COUNT"), S(info, "ET_OUT_OF_MAP_COUNT"), S(info, "MAP_SHAPE_SAME"), S(info, "ET_MS")));

        Dictionary<string, DataRow> link = Rows(ds, "ET_LINK").ToDictionary(r => S(r, "GDS_CHAIN_KEY"));
        DataRow la, lb, lc, lz;
        link.TryGetValue(nameA.Trim().ToUpperInvariant(), out la);
        link.TryGetValue(S(chainB, "CHAIN_NAME").Trim().ToUpperInvariant(), out lb);
        link.TryGetValue(S(chainC, "CHAIN_NAME").Trim().ToUpperInvariant(), out lc);
        link.TryGetValue("ZZ_PROBE_NOT_IN_GDS", out lz);
        Dictionary<long, DataRow> chainRows = Rows(ds, "CHAIN").ToDictionary(r => L(r, "CHAIN_SEQ"));
        Check("T2 이름 연결: A(소문자 / 공백 포함 2개) / B / C 연결, GDS 에 없는 이름, Chain 표 연결 상태 / Parameter 수",
            link.Count == 4 && la != null && S(la, "STATUS") == "LINKED" && L(la, "CHAIN_SEQ") == seqA && I(la, "PARA_COUNT") == 2
            && lb != null && L(lb, "CHAIN_SEQ") == seqB && lc != null && L(lc, "CHAIN_SEQ") == seqC && lz != null && S(lz, "STATUS") == "NOT_IN_GDS"
            && S(chainRows[seqA], "ET_LINK_STATUS") == "LINKED" && I(chainRows[seqA], "ET_PARA_COUNT") == 2
            && chainRows.Values.Count(r => S(r, "ET_LINK_STATUS") == "LINKED") == 3 && chainRows.Values.Count(r => S(r, "ET_LINK_STATUS") == "NONE") == chainRows.Count - 3,
            string.Join(" | ", Rows(ds, "ET_LINK").Select(r => S(r, "GDS_CHAIN_NAME") + "->" + S(r, "CHAIN_CODE") + " " + S(r, "STATUS") + " (" + S(r, "PARA_COUNT") + ")").ToArray()));

        Dictionary<DiePoint, DataRow> dieRows = Rows(ds, "DIE").ToDictionary(r => new DiePoint(I(r, "SYS_X"), I(r, "SYS_Y")));
        bool othersPass = used.Where(p => !p.Equals(t1) && !p.Equals(t2)).All(p => dieRows.ContainsKey(p) && S(dieRows[p], "ET_STATE") == "P");
        Check("T3 Die 판정: T1 / T2 = F, 나머지 P (T3 은 값 없음이라 P) / Fail Chain 수 / 일치 Chain 수",
            S(dieRows[t1], "ET_STATE") == "F" && S(dieRows[t2], "ET_STATE") == "F" && othersPass
            && I(dieRows[t1], "ET_FAIL_CHAIN_COUNT") == 1 && I(dieRows[t1], "BOTH_CHAIN_COUNT") == 1
            && I(dieRows[t2], "ET_FAIL_CHAIN_COUNT") == 1 && I(dieRows[t2], "BOTH_CHAIN_COUNT") == 0 && I(dieRows[t3], "ET_FAIL_CHAIN_COUNT") == 0,
            "T1 " + S(dieRows[t1], "ET_STATE") + "/" + S(dieRows[t1], "ET_FAIL_CHAIN_COUNT") + "/" + S(dieRows[t1], "BOTH_CHAIN_COUNT")
            + " T2 " + S(dieRows[t2], "ET_STATE") + "/" + S(dieRows[t2], "ET_FAIL_CHAIN_COUNT") + " T3 " + S(dieRows[t3], "ET_STATE") + " / Die 행 " + dieRows.Count);
        // 결과 파일 DUT Flag (2026-10-19): ET_FILE_FLAG 는 Spec 판정(ET_STATE)과 별개로 파일 값 그대로. 화면 빨간 삼각형 "Fail Die" 기준
        bool fileFlagOk = S(dieRows[t3], "ET_FILE_FLAG") == "F" && S(dieRows[t3], "ET_STATE") == "P" && S(dieRows[t1], "ET_FILE_FLAG") == "P"
            && used.Where(p => !p.Equals(t3)).All(p => dieRows.ContainsKey(p) && S(dieRows[p], "ET_FILE_FLAG") == "P");
        Check("T3-1 결과 파일 DUT Flag 열 ET_FILE_FLAG: Spec 판정과 별개(T3 파일 F / Spec P, T1 Spec F / 파일 P, 나머지 P)", fileFlagOk,
            "T1 " + S(dieRows[t1], "ET_STATE") + "/" + S(dieRows[t1], "ET_FILE_FLAG") + " T3 " + S(dieRows[t3], "ET_STATE") + "/" + S(dieRows[t3], "ET_FILE_FLAG")
            + " / 파일 F " + Rows(ds, "DIE").Count(r => S(r, "ET_FILE_FLAG") == "F"));

        Func<DiePoint, long, DataRow> pair = (d, seq) => Rows(ds, "DIE_CHAIN").FirstOrDefault(r => I(r, "SYS_X") == d.X && I(r, "SYS_Y") == d.Y && L(r, "CHAIN_SEQ") == seq);
        DataRow p1 = pair(t1, seqA), p2 = pair(t2, seqB), p3 = pair(t3, seqC);
        var expectDefect = aHitDies.Where(d => !d.Equals(t1)).ToList();
        bool defectOnly = expectDefect.All(d => { DataRow r = pair(d, seqA); return r != null && S(r, "CLASS4") == (inspected.Contains(d) ? "DEFECT" : "NA") && S(r, "ET_STATUS") == "P"; });
        bool noPassRows = Rows(ds, "DIE_CHAIN").All(r => I(r, "DEFECT_COUNT") > 0 || S(r, "ET_STATUS") == "F" || S(r, "ET_STATUS") == "N");
        Check("T4 (Die, Chain): T1·A 일치(첫 Fail 항목 / 값 150) / T2·B Fail만(값 '-') / T3·C 비교 불가(N) / A 의 다른 Die Defect만 / Defect 0 + P 칸은 안 보냄",
            p1 != null && S(p1, "ET_STATUS") == "F" && S(p1, "CLASS4") == "BOTH" && S(p1, "ET_FIRST_FAIL_ITEM") == S(paras[0], "ITEM_NAME") && S(p1, "ET_FIRST_FAIL_VALUE") == "150"
            && I(p1, "ET_PARA_COUNT") == 2 && I(p1, "ET_FAIL_COUNT") == 1
            && p2 != null && S(p2, "ET_STATUS") == "F" && S(p2, "CLASS4") == "FAIL" && S(p2, "ET_FIRST_FAIL_VALUE") == "-" && I(p2, "DEFECT_COUNT") == 0
            && p3 != null && S(p3, "ET_STATUS") == "N" && S(p3, "CLASS4") == "NA" && defectOnly && noPassRows,
            (p1 == null ? "T1 행 없음" : S(p1, "CLASS4") + " " + S(p1, "ET_FIRST_FAIL_ITEM") + "=" + S(p1, "ET_FIRST_FAIL_VALUE"))
            + " / " + (p2 == null ? "T2 행 없음" : S(p2, "CLASS4") + " " + S(p2, "ET_FIRST_FAIL_VALUE") + " MIN_GAP " + S(p2, "MIN_GAP"))
            + " / " + (p3 == null ? "T3 행 없음" : S(p3, "CLASS4")) + " / A Defect만 Die " + expectDefect.Count + " / 행 " + ds.Tables["DIE_CHAIN"].Rows.Count);

        // Chain 별 Die 수 (설계 7.2): 대상 = 검사 Die 이면서 그 Chain E-Test P / F
        List<DiePoint> judgedA = used.Where(p => inspected.Contains(p)).ToList();
        int expDefect = judgedA.Count(p => aHitDies.Contains(p));
        DataRow ca = chainRows[seqA], cb = chainRows[seqB], cc = chainRows[seqC];
        double rateDefect = expDefect == 0 ? double.NaN : Math.Round(1.0 / expDefect, 4);
        Check("T5 Chain 별 Die 수 / Fail률: A(판정 / Defect / Fail 1 / 일치 1, Defect 있음 Fail률 = 1 / Defect Die) / B(Fail 1, Defect 없음 Fail률) / C(값 없음 Die 제외)",
            I(ca, "DIE_JUDGED") == judgedA.Count && I(ca, "DIE_DEFECT") == expDefect && I(ca, "DIE_FAIL") == 1 && I(ca, "DIE_BOTH") == 1
            && Math.Abs(D(ca, "FAIL_RATE_DEFECT") - rateDefect) < 1e-9 && D(ca, "FAIL_RATE_CLEAN") == 0
            && I(cb, "DIE_FAIL") == 1 && I(cb, "DIE_BOTH") == 0 && Math.Abs(D(cb, "FAIL_RATE_CLEAN") - Math.Round(1.0 / (I(cb, "DIE_JUDGED") - I(cb, "DIE_DEFECT")), 4)) < 1e-9
            && I(cc, "DIE_JUDGED") == judgedA.Count - 1 && I(cc, "DIE_FAIL") == 0,
            String.Format("A 판정 {0} Defect {1} Fail {2} 일치 {3} 률 {4}/{5} | B 판정 {6} Fail {7} 률 {8} | C 판정 {9}",
                S(ca, "DIE_JUDGED"), S(ca, "DIE_DEFECT"), S(ca, "DIE_FAIL"), S(ca, "DIE_BOTH"), S(ca, "FAIL_RATE_DEFECT"), S(ca, "FAIL_RATE_CLEAN"),
                S(cb, "DIE_JUDGED"), S(cb, "DIE_FAIL"), S(cb, "FAIL_RATE_CLEAN"), S(cc, "DIE_JUDGED")));

        // Die 상세 + E-Test 값 / 역추적
        DataSet die = bsl.AnalyzeDie(Req(_step, long.Parse(fileSeq, CultureInfo.InvariantCulture)), I(t1Row, "XINDEX"), I(t1Row, "YINDEX"), seqA.ToString(CultureInfo.InvariantCulture));
        List<DataRow> vals = Rows(die, "ET_VALUE").ToList();
        DataRow v1 = vals.FirstOrDefault(r => S(r, "ITEM_NAME") == S(paras[0], "ITEM_NAME"));
        DataRow dA = Rows(die, "DIE_CHAIN").FirstOrDefault(r => L(r, "CHAIN_SEQ") == seqA);
        Check("T6 Die 상세 T1: E-Test 값 5행(GDS Chain 있는 Parameter) / 연결 4행 Chain 번호 / P1 = 150 F, Spec 0~100 / (Die, A) 일치 / E-Test Chain 행 포함",
            vals.Count == 5 && vals.Count(r => S(r, "CHAIN_SEQ").Length > 0) == 4 && v1 != null && S(v1, "MEAS_VALUE") == "150" && S(v1, "JUDGE_FLAG") == "F"
            && S(v1, "LOWER_LIMIT") == "0" && S(v1, "UPPER_LIMIT") == "100" && L(v1, "CHAIN_SEQ") == seqA
            && dA != null && S(dA, "CLASS4") == "BOTH" && Rows(die, "DIE_CHAIN").Any(r => L(r, "CHAIN_SEQ") == seqB && S(r, "ET_STATUS") == "P"),
            string.Join(" | ", vals.Select(r => S(r, "ITEM_NAME") + "=" + S(r, "MEAS_VALUE") + " " + S(r, "JUDGE_FLAG") + " [" + S(r, "GDS_CHAIN_NAME").Trim() + "]").ToArray()));

        // Ready: E-Test 파일 Factory(PROBE) + 같은 Device 로 찾으면 같은 Wafer Key 파일이 보임(Test 시각 최신 = 선택)
        // 2026-10-20: Device 가 분석 GDS Device 와 같은 파일만 목록에 넣는다(PROBE 에는 GDS 가 없어 Device 를 직접 넘긴다)
        DataSet ready = bsl.GetWaferReady(EtFactory, _step, EtDevice);
        DataRow etRow = Rows(ready, "READY_ET").FirstOrDefault(r => S(r, "SRC_FILE_SEQ") == fileSeq);
        Check("T7 Ready E-Test: 같은 Wafer Key + 같은 Device 파일 / 선택 Y / Step / Map 형상 / (PROBE 에 GDS 없음 -> GDS_NO_MAP)",
            etRow != null && S(etRow, "SELECTED") == "Y" && S(etRow, "TEST_PROGRAM") == Program && S(etRow, "WAFER_ID") == "RH-HS2615-02" && S(etRow, "DEVICE_ID") == EtDevice
            && S(etRow, "MAP_SHAPE_SAME") == "Y" && Rows(ready, "READY_CHECK").Any(r => S(r, "CODE") == "GDS_NO_MAP") && !Rows(ready, "READY_CHECK").Any(r => S(r, "CODE") == "ET_OTHER_DEVICE"),
            etRow == null ? "파일 행 없음" : S(etRow, "TEST_PROGRAM") + " " + S(etRow, "WAFER_ID") + " " + S(etRow, "DEVICE_ID") + " Die " + S(etRow, "DIE_COUNT") + " Fail " + S(etRow, "FAIL_DIE_COUNT")
                + " 판정 " + S(etRow, "JUDGE_TIME") + " 형상 " + S(etRow, "MAP_SHAPE_SAME"));
        // Device 가 다르면 같은 Wafer Key 라도 목록에서 빠지고 점검 정보 ET_OTHER_DEVICE 로만 알린다.
        // Device 를 비워 보내면 서버가 KLARF Product 이름(STEP 528 = OMM_Hybrid)을 분석 Device 로 쓰므로 같은 Device 파일이 그대로 보인다
        DataSet otherReady = bsl.GetWaferReady(EtFactory, _step, "PROBE_OTHER_DEVICE");
        DataSet productReady = bsl.GetWaferReady(EtFactory, _step, "");
        DataRow otherCheck = Rows(otherReady, "READY_CHECK").FirstOrDefault(r => S(r, "CODE") == "ET_OTHER_DEVICE");
        Check("T7-1 Ready E-Test Device 다름: 목록 0 / 점검 정보 ET_OTHER_DEVICE(파일 Device + Step) / Device 빈 값 = Product 이름 OMM_Hybrid 기준 1행",
            Rows(otherReady, "READY_ET").Count() == 0 && otherCheck != null && S(otherCheck, "LEVEL") == "정보" && S(otherCheck, "MESSAGE").Contains(EtDevice + " " + Program)
            && Rows(productReady, "READY_ET").Count(r => S(r, "SRC_FILE_SEQ") == fileSeq) == 1 && S(productReady.Tables["READY_GDS"].Rows[0], "DEVICE_ID") == EtDevice
            && !Rows(productReady, "READY_CHECK").Any(r => S(r, "CODE") == "ET_OTHER_DEVICE"),
            "다른 Device " + Rows(otherReady, "READY_ET").Count() + "행 / 빈 Device(GDS " + S(productReady.Tables["READY_GDS"].Rows[0], "DEVICE_ID") + ") "
            + Rows(productReady, "READY_ET").Count() + "행 / " + (otherCheck == null ? "점검 없음" : S(otherCheck, "MESSAGE")));

        string bad = Throws(() => bsl.Analyze(Req(_step, 999999999)));
        Check("T8 없는 E-Test 파일 오류", bad.Contains("E-Test 파일이 없습니다"), bad);
    }

    /// <summary>PROBE 시험 E-Test 파일(Lot RH-HS2615 Slot 2) / 등록 Parameter(Spec 포함) 삭제.</summary>
    private static void Cleanup(GdsEtResult et)
    {
        foreach (DataRow r in et.GetEtFileByKey(new[] { EtFactory, Lot, "2" }).Rows)
            et.DeleteEtFile(S(r, "SRC_FILE_SEQ"), User);
        string[] names = et.GetEtParameterList(EtFactory, EtDevice, Program).Rows.Cast<DataRow>().Select(r => S(r, "ITEM_NAME")).ToArray();
        if (names.Length > 0) et.DeleteEtParameters(new[] { EtFactory, EtDevice, Program }, names, User);
    }

    private static GdsKlarfAnalysisRequest Req(long step, long etFile)
    {
        return new GdsKlarfAnalysisRequest
        {
            Factory = _factory,
            DeviceId = _device,
            WaferKey = "RH-HS2615-02",
            StepSeqs = new[] { step },
            ViewSides = new[] { "BACK" },
            ChainSide = "BACK",
            DisplayStepSeq = step,
            AreaMode = GdsKlarfAnalysisRequest.AreaSizeTolerance,
            ToleranceUm = 5,
            EtSrcFileSeq = etFile
        };
    }

    private static IEnumerable<DataRow> Rows(DataSet ds, string table)
    {
        return ds.Tables.Contains(table) ? ds.Tables[table].Rows.Cast<DataRow>() : Enumerable.Empty<DataRow>();
    }

    private static string S(DataRow r, string c) { return r == null || !r.Table.Columns.Contains(c) || r[c] == DBNull.Value ? "" : Convert.ToString(r[c], CultureInfo.InvariantCulture); }

    private static int I(DataRow r, string c) { string s = S(r, c); return s.Length == 0 ? 0 : Convert.ToInt32(Convert.ToDouble(s, CultureInfo.InvariantCulture)); }

    private static long L(DataRow r, string c) { string s = S(r, c); return s.Length == 0 ? 0 : Convert.ToInt64(Convert.ToDouble(s, CultureInfo.InvariantCulture)); }

    private static double D(DataRow r, string c) { string s = S(r, c); return s.Length == 0 ? double.NaN : Convert.ToDouble(s, CultureInfo.InvariantCulture); }

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
