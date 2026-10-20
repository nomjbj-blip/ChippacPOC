using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using DACrux.SEMDMS.Interface;

namespace DACrux.SEMDMS.BSL
{
    /// <summary>
    /// GDS × KLARF × E-Test 통합 연관 분석 (2026-10-16): Analyze(Wafer 집계) / AnalyzeDie(Die 1개 상세). 결과는 저장하지 않는다.
    /// 설계: 문서/설계/05_연관분석/2026-10-15_GDS_KLARF_ETest_통합분석_데이터_설계.md 4 / 5 / 7 / 9 / 10 / 15 / 16.7 / 18 / 19절
    ///
    /// 처리 흐름
    ///  1) 요청 확인 (Step / 검사 면 / Chain 면 / Defect 영역 / 분류 기준)
    ///  2) Step 마다 KLARF 정보 / Pitch (TQD_INSP_INFO.SELECT_DMS_WAFER_INFO) + 검사 Die (TQD_SETUP_MAP.SELECT_X_Y)
    ///  3) GDS Map / Chain 목록 -> 모양 Cache (GdsKlarfGeometryCache, TQP_GDS_CHAIN_MEMBER.SELECT_CHAIN_MEMBER_SHAPE_ALL)
    ///  4) Step 마다 Defect (TQD_DEFECT.SELECT_DEFECT_FOR_GDS) -> 분류 거르기 / 큰 Defect / 시스템 Die 좌표
    ///  5) E-Test 파일을 골랐으면 Chain 이름 연결 / Die 판정 / (Die, Chain) 판정 (GdsKlarfEtLink.cs, SQL 에서 묶음)
    ///  6) Step 마다 GdsKlarfMatchEngine.MatchShapes. Defect 반전 = (Chain 면 ≠ 검사 면) XOR (Map 저장 면 = BACK)
    ///  7) 시스템 Die 좌표로 Step / E-Test 를 합쳐 결과 표를 만든다. Die Index / GDS 보기 좌표는 Map 을 그린 Step(DisplayStepSeq) 기준
    ///     (Die, Chain) 4분류 = BOTH / FAIL / DEFECT / OK / NA (설계 7.1절), Chain 별 Die 수 / Fail률 2개 (7.2절)
    ///
    /// 시스템 Die 좌표 (설계 6.4절): KLARF XINDEX / YINDEX 를 그대로 쓴다(KLARF / E-Test 는 데이터 그대로, 2026-10-20).
    ///  검사 면은 Defect 반전(위 6)에만 쓴다. KLARF 검사 면은 FS 고정(요청이 비면 FRONT), 고르는 것은 Chain 면만.
    ///  이전(2026-10-16 ~ 10-19): 검사 면 FRONT 면 Die 를 (MinX + MaxX - x, y) 로 바꿨다. 여러 Step(설계 B16, 2차) 때 다시 본다.
    ///
    /// 결과 테이블 (Analyze)
    ///  ANL_INFO     1행: AnalyzeWafer 와 같은 열 + Defect 영역 / 면 / Step / Cache / 단계별 시간 / E-Test 파일
    ///               TOLERANCE_UM = 걸림 계산에 쓴 반폭(중심 사각형이면 한 변 / 2), USE_DEFECT_SIZE = Y(크기 + 허용오차) / N(중심 사각형)
    ///  CHAIN        Chain: AnalyzeWafer 열 + ET_LINK_STATUS / ET_PARA_COUNT / DIE_JUDGED / DIE_DEFECT / DIE_FAIL / DIE_BOTH / FAIL_RATE_DEFECT / FAIL_RATE_CLEAN
    ///  DIE          Die: XINDEX / YINDEX(Map Step Index) / SYS_X / SYS_Y / IN_PLAN / DEFECT_COUNT / IN_CHAIN_COUNT / OUT_CHAIN_COUNT /
    ///               HIT_CHAIN_COUNT / TOP_CHAINS / ET_STATE / ET_FAIL_CHAIN_COUNT / BOTH_CHAIN_COUNT /
    ///               ET_FILE_FLAG(결과 파일 DUT Pass / Fail Flag, E-Test Die 없으면 빈 값, 2026-10-19)
    ///  DIE_CHAIN    걸린 Defect 가 있는 (Die, Chain): DEFECT_COUNT / DEFECT_IDS / MIN_GAP / NEAR_DEFECT_ID / NEAR_STEP_ID / ET_* / CLASS4
    ///  DEFECT       Defect(키 STEP_SEQ + DEFECTID): 위치 / 크기 / 분류 / GDS_X / GDS_Y(Map Step 보기 좌표) / IN_CHAIN / CHAIN_CODES /
    ///               GAP(탐색 거리 안 가장 가까운 Chain 까지, 없으면 빈 값) / NEAR_CHAIN_SEQ / NEAR_CHAIN_CODE / LARGE.
    ///               Defect 가 DefectRowsAll 이하면 전부, 넘으면 탐색 거리 안 Defect 만 (ANL_INFO.DEFECT_ROWS = ALL / NEAR)
    ///  DEFECT_CHAIN 걸린 (Defect, Chain): GAP / NEAR_PLACED_ELEMENT_ID (Member 단위 행 대신)
    ///  TOL_CURVE    허용오차 단계(0 / 2 / 5 / 10 / 20 / 50 + 지금 값)별 Chain 영역 Defect 수 / 걸린 Die 수. 중심 사각형이면 빈 표
    ///  ET_LINK      E-Test Chain 이름 연결 (E-Test 를 고르지 않으면 빈 표)
    /// AnalyzeDie 는 같은 모양을 Die 1개로 만든다: DEFECT = 그 Die 전체, DIE_CHAIN / DEFECT_CHAIN = 탐색 거리 안 전부(역추적 거리),
    ///  DEFECT_HIT = 걸린 Member 단위(Gap ≤ 걸림 기준), ET_VALUE = 그 Die 연결 Parameter 값.
    /// </summary>
    public partial class GdsKlarfAnalysis
    {
        /// <summary>큰 Defect: 한 변이 이 값(µm) 이상 (결정 B5).</summary>
        private const double LargeDefectUm = 1000;
        /// <summary>판정 Defect 상한(Step 합계, 결정 B14). 넘으면 분류 거르기를 안내한다.</summary>
        private const int DefectLimit = 300000;
        /// <summary>DEFECT 표에 모든 Defect 를 보내는 상한. 넘으면 탐색 거리 안 Defect 만 보낸다(결과 축소, 설계 9.2절).</summary>
        private const int DefectRowsAll = 20000;
        private static readonly double[] TolSteps = { 0, 2, 5, 10, 20, 50 };

        #region [요청 / 중간 값]

        private sealed class StepRun
        {
            public long Seq;
            public string Side;
            public DataRow Info;
            public string StepId;
            public double PitchX, PitchY;
            public string PitchSource;
            public readonly List<DiePoint> Plan = new List<DiePoint>();
            public bool Mirror;
            public int DefectTotal, ClassRemoved, LargeCount, LargeExcluded;
            public readonly List<DefectRun> Defects = new List<DefectRun>();
        }

        private sealed class DefectRun
        {
            public StepRun Step;
            public GdsKlarfDefectPoint Point;
            public int RoughBin;
            public bool Large;
            public DiePoint Sys;
            /// <summary>Map 을 그린 Step 의 Die Index</summary>
            public DiePoint Index;
            public GdsKlarfDefectGap Gap;
            /// <summary>Chain 별 가장 가까운 Member (탐색 거리 안)</summary>
            public Dictionary<long, GdsKlarfShapeGap> ByChain;
            /// <summary>걸린 Chain (Gap ≤ 걸림 기준), Chain Code 순서</summary>
            public List<long> HitChains;
            /// <summary>Map Step 보기 기준 GDS X (µm). Y 는 Gap.ViewY 그대로</summary>
            public double GdsX;
        }

        private sealed class AnalysisRun
        {
            public string Factory, DeviceId, Product, ChainSide, MapSide, AreaMode, ClassBasis, WaferKey;
            public double Tolerance, Square, HitGap, Search, OffsetX, OffsetY;
            public bool ExcludeLarge;
            public long EtSrcFileSeq;
            public HashSet<long> ChainFilter;
            public HashSet<int> ClassFilter;
            public readonly List<StepRun> Steps = new List<StepRun>();
            public StepRun Display;
            public DataRow MapInfo;
            /// <summary>분석 Chain (READY + 화면 거르기). SELECT_CHAIN_LIST 순서</summary>
            public readonly Dictionary<long, DataRow> Chains = new Dictionary<long, DataRow>();
            public readonly Dictionary<long, string> ChainCode = new Dictionary<long, string>();
            public readonly Dictionary<long, int> ChainMembers = new Dictionary<long, int>();
            public readonly Dictionary<long, string> ChainLayers = new Dictionary<long, string>();
            public int DraftCount, MemberCount;
            /// <summary>Device 의 READY Chain 전체 (화면 거르기와 상관없이 E-Test 이름 연결 / Cache Stamp 에 쓴다)</summary>
            public List<DataRow> ReadyRows = new List<DataRow>();
            public GdsKlarfShapeIndex Index;
            public bool CacheHit;
            public long InfoMs, GdsMs, DefectMs, MatchMs, EtMs;
            public readonly List<DefectRun> Defects = new List<DefectRun>();
            /// <summary>판정한 Defect (Analyze = 전부, AnalyzeDie = 그 Die)</summary>
            public readonly List<DefectRun> Matched = new List<DefectRun>();
            /// <summary>어느 Step 이든 KLARF 검사 Die 목록에 있는 시스템 Die</summary>
            public readonly HashSet<DiePoint> PlanSys = new HashSet<DiePoint>();
            /// <summary>고른 E-Test 파일 판정 (없으면 null)</summary>
            public EtData Et;
            public readonly Stopwatch Total = Stopwatch.StartNew();
        }

        private sealed class DieAgg
        {
            public DiePoint Sys, Index;
            public bool InPlan;
            public int Total, InChain;
            /// <summary>KLARF 검사 Die(어느 Step 이든 검사 Die 목록에 있거나 Defect 가 있음)</summary>
            public bool Inspected;
            public EtDie Et;
            public readonly Dictionary<long, List<DefectRun>> Hits = new Dictionary<long, List<DefectRun>>();
            public readonly Dictionary<long, NearDefect> Near = new Dictionary<long, NearDefect>();
        }

        private sealed class NearDefect
        {
            public double Gap;
            public DefectRun Defect;
        }

        #endregion

        /// <summary>통합 연관 분석 Wafer 집계. 인자 / 결과 설명은 iGdsKlarfAnalysis / 이 파일 머리 주석 참고.</summary>
        public DataSet Analyze(GdsKlarfAnalysisRequest request)
        {
            AnalysisRun run = Prepare(request);
            Match(run, null);
            var build = Stopwatch.StartNew();
            DataSet ds = BuildAnalysis(run, false, null, null);
            AddInfo(ds, run, build, null, null);
            return ds;
        }

        /// <summary>통합 연관 분석 Die 1개 상세. xIndex / yIndex = Map 을 그린 Step 의 Die Index.</summary>
        public DataSet AnalyzeDie(GdsKlarfAnalysisRequest request, int xIndex, int yIndex, string focusChainSeq)
        {
            AnalysisRun run = Prepare(request);
            DiePoint target = ToSystem(run.Display, xIndex, yIndex);
            Match(run, d => d.Sys.Equals(target));
            long focus;
            string focusText = long.TryParse(focusChainSeq == null ? null : focusChainSeq.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out focus)
                && run.Chains.ContainsKey(focus) ? focus.ToString(CultureInfo.InvariantCulture) : "";
            var build = Stopwatch.StartNew();
            DataSet ds = BuildAnalysis(run, true, target, new DiePoint(xIndex, yIndex));
            AddInfo(ds, run, build, new DiePoint(xIndex, yIndex), focusText);
            return ds;
        }

        #region [준비 / 판정]

        /// <summary>요청 확인 -> Step 정보 / 검사 Die -> GDS Map / Chain / 모양 Cache -> Defect 읽기. 판정은 Match 에서 한다.</summary>
        private static AnalysisRun Prepare(GdsKlarfAnalysisRequest request)
        {
            if (request == null) throw new ArgumentNullException("request");
            var run = new AnalysisRun();
            run.Factory = Required(request.Factory, "Factory", 30);
            if (request.StepSeqs == null || request.StepSeqs.Length == 0) throw new ArgumentException("분석할 KLARF Step 을 1개 이상 고르세요.");
            if (request.StepSeqs.Any(s => s <= 0)) throw new ArgumentException("STEP_SEQ 는 1 이상이어야 합니다.");
            if (request.StepSeqs.Distinct().Count() != request.StepSeqs.Length) throw new ArgumentException("같은 KLARF Step 이 두 번 들어 있습니다.");
            if (request.ViewSides != null && request.ViewSides.Length != request.StepSeqs.Length)
                throw new ArgumentException("검사 면(ViewSides) 수가 Step 수와 다릅니다.");
            // Chain 면 기본 BS (2026-10-20 변경, 이전 FS)
            run.ChainSide = String.IsNullOrEmpty(request.ChainSide == null ? null : request.ChainSide.Trim()) ? SideBack : NormalizeSide(request.ChainSide);
            long displaySeq = request.DisplayStepSeq > 0 ? request.DisplayStepSeq : request.StepSeqs[0];
            if (!request.StepSeqs.Contains(displaySeq)) throw new ArgumentException("Map 을 그린 Step(DisplayStepSeq) 이 분석 Step 에 없습니다: " + displaySeq);
            run.AreaMode = String.Equals((request.AreaMode ?? "").Trim(), GdsKlarfAnalysisRequest.AreaSquare, StringComparison.OrdinalIgnoreCase)
                ? GdsKlarfAnalysisRequest.AreaSquare : GdsKlarfAnalysisRequest.AreaSizeTolerance;
            if (run.AreaMode == GdsKlarfAnalysisRequest.AreaSquare)
            {
                if (double.IsNaN(request.SquareUm) || request.SquareUm <= 0 || request.SquareUm > 20000)
                    throw new ArgumentException("Defect 중심 사각형 한 변은 0 초과 ~ 20,000 µm 로 입력하세요.");
                run.Square = request.SquareUm;
                run.HitGap = 0;
            }
            else
            {
                if (double.IsNaN(request.ToleranceUm) || request.ToleranceUm < 0 || request.ToleranceUm > 10000)
                    throw new ArgumentException("허용오차는 0 ~ 10,000 µm로 입력하세요.");
                run.Tolerance = request.ToleranceUm;
                run.HitGap = request.ToleranceUm;
            }
            run.Search = Math.Max(run.HitGap, TolSteps[TolSteps.Length - 1]);
            if (double.IsNaN(request.OffsetXUm) || double.IsNaN(request.OffsetYUm) || Math.Abs(request.OffsetXUm) > 100000 || Math.Abs(request.OffsetYUm) > 100000)
                throw new ArgumentException("좌표 보정은 ±100,000 µm 안으로 입력하세요.");
            run.OffsetX = request.OffsetXUm;
            run.OffsetY = request.OffsetYUm;
            run.ExcludeLarge = request.ExcludeLargeDefect;
            run.ChainFilter = ToChainFilter(request.ChainSeqs);
            run.ClassBasis = String.Equals((request.ClassBasis ?? "").Trim(), GdsKlarfAnalysisRequest.ClassBasisClass, StringComparison.OrdinalIgnoreCase)
                ? GdsKlarfAnalysisRequest.ClassBasisClass : GdsKlarfAnalysisRequest.ClassBasisRoughBin;
            run.ClassFilter = request.ClassValues == null || request.ClassValues.Length == 0 ? null : new HashSet<int>(request.ClassValues);
            run.WaferKey = (request.WaferKey ?? "").Trim();
            run.EtSrcFileSeq = Math.Max(0, request.EtSrcFileSeq);

            // 2) Step 정보 / 검사 Die
            var watch = Stopwatch.StartNew();
            for (int i = 0; i < request.StepSeqs.Length; i++)
            {
                long seq = request.StepSeqs[i];
                DataTable stepInfo = new DACrux.SEMDMS.DSL.TQD_INSP_INFO().GetData("SELECT_DMS_WAFER_INFO", null,
                    new string[] { seq.ToString(CultureInfo.InvariantCulture) });
                if (stepInfo == null || stepInfo.Rows.Count == 0)
                    throw new ArgumentException("KLARF Wafer 정보가 없습니다. STEP_SEQ=" + seq);
                DataRow info = stepInfo.Rows[0];
                var step = new StepRun
                {
                    Seq = seq,
                    // KLARF 검사 면 기본 FS (2026-10-20 변경, 이전 BS)
                    Side = request.ViewSides == null || String.IsNullOrEmpty(request.ViewSides[i] == null ? null : request.ViewSides[i].Trim())
                        ? SideFront : NormalizeSide(request.ViewSides[i]),
                    Info = info,
                    StepId = Text(info["STEP_ID"]),
                    PitchX = ToDouble(info["DIE_PITCH_X"]),
                    PitchY = ToDouble(info["DIE_PITCH_Y"]),
                    PitchSource = "MAP_SETUP"
                };
                // Pitch 출처(결정 B6): KLARF 업로드가 TQD_STEP.COMMENT 에 남긴 파일 DiePitch(XREL 기준)를 먼저, 없으면 TQD_SETUP(Map Setup) 값
                DataTable comment = new DACrux.SEMDMS.DSL.TQD_STEP().GetData("SELECT_STEP_KLARF_UPLOAD", null,
                    new string[] { seq.ToString(CultureInfo.InvariantCulture), Text(info["WAFER_SEQ"]) });
                double rawX, rawY;
                if (comment != null && comment.Rows.Count > 0 && TryRawPitch(Text(comment.Rows[0]["STEP_COMMENT"]), out rawX, out rawY))
                {
                    step.PitchX = rawX;
                    step.PitchY = rawY;
                    step.PitchSource = "KLARF";
                }
                if (!(step.PitchX > 0) || !(step.PitchY > 0))
                    throw new InvalidOperationException("KLARF Die Pitch가 없습니다(TQD_SETUP.DIE_PITCH_X / Y). STEP_SEQ=" + seq);
                DataTable planRows = new DACrux.SEMDMS.DSL.TQD_SETUP_MAP().GetData("SELECT_X_Y", null,
                    new string[] { Text(info["SETUP_SEQ"]), Text(info["TEST"]) });
                var seen = new HashSet<DiePoint>();
                foreach (DataRow row in planRows.Rows)
                {
                    var p = new DiePoint((int)ToLong(row["INDEX_X"]), (int)ToLong(row["INDEX_Y"]));
                    if (seen.Add(p)) step.Plan.Add(p);
                }
                foreach (DiePoint p in step.Plan) run.PlanSys.Add(ToSystem(step, p.X, p.Y));
                run.Steps.Add(step);
                if (seq == displaySeq) run.Display = step;
            }
            run.Product = Text(run.Display.Info["PRODUCT"]);

            // 3) GDS Map / Chain / 모양 Cache
            string device = request.DeviceId == null ? "" : request.DeviceId.Trim();
            run.DeviceId = Required(device.Length == 0 ? ResolveMapDevice(run.Factory, run.Product) : device, "GDS Device ID", 50);
            DataTable mapInfo = new DACrux.SEMDMS.DSL.TQP_GDS_MAP().GetData("SELECT_MAP_INFO", null, new string[] { run.Factory, run.DeviceId });
            if (mapInfo == null || mapInfo.Rows.Count == 0 || Text(mapInfo.Rows[0]["MAP_STATUS"]) != "READY")
                throw new InvalidOperationException("저장 완료(READY)된 GDS Map이 없습니다: " + run.Factory + " / " + run.DeviceId);
            run.MapInfo = mapInfo.Rows[0];
            run.MapSide = NormalizeSide(Text(run.MapInfo["VIEW_SIDE"]));
            DataTable chainList = new DACrux.SEMDMS.DSL.TQP_GDS_CHAIN().GetData("SELECT_CHAIN_LIST", null, new string[] { run.Factory, run.DeviceId });
            var readyRows = new List<DataRow>();
            foreach (DataRow row in chainList.Rows)
            {
                long seq = ToLong(row["CHAIN_SEQ"]);
                bool ready = Text(row["VERSION_STATUS"]) == "READY";
                if (ready) readyRows.Add(row);
                if (run.ChainFilter != null && !run.ChainFilter.Contains(seq)) continue;
                if (!ready) { run.DraftCount++; continue; }
                run.Chains.Add(seq, row);
                run.ChainCode.Add(seq, Text(row["CHAIN_CODE"]));
            }
            run.ReadyRows = readyRows;
            run.InfoMs = watch.ElapsedMilliseconds;
            watch.Restart();
            if (readyRows.Count > 0)
            {
                var readySeqs = new HashSet<long>(readyRows.Select(r => ToLong(r["CHAIN_SEQ"])));
                string factory = run.Factory, deviceId = run.DeviceId;
                long loadMs;
                run.Index = GdsKlarfGeometryCache.GetOrLoad(factory, deviceId, GdsKlarfGeometryCache.MakeStamp(run.MapInfo, readyRows), readySeqs,
                    () => new DACrux.SEMDMS.DSL.TQP_GDS_CHAIN_MEMBER().GetData("SELECT_CHAIN_MEMBER_SHAPE_ALL", null, new string[] { factory, deviceId }),
                    out run.CacheHit, out loadMs);
            }
            else
            {
                run.Index = new GdsKlarfShapeIndex(new List<GdsKlarfShape>(), GdsKlarfGeometryCache.DefaultCellSize, 0, 0);
            }
            foreach (IGrouping<long, GdsKlarfShape> g in run.Index.Shapes.Where(s => run.Chains.ContainsKey(s.ChainSeq)).GroupBy(s => s.ChainSeq))
            {
                run.ChainMembers[g.Key] = g.Count();
                run.ChainLayers[g.Key] = FormatLayerIds(g.Select(s => new KeyValuePair<int, int>(s.LayerId, s.DataType)));
                run.MemberCount += g.Count();
            }
            run.GdsMs = watch.ElapsedMilliseconds;

            // 4) Defect: Step 마다 읽고 분류 / 큰 Defect 거르기, 시스템 Die / Map Step Index
            watch.Restart();
            foreach (StepRun step in run.Steps)
            {
                step.Mirror = (run.ChainSide != step.Side) ^ (run.MapSide == SideBack);
                DataTable rows = new DACrux.SEMDMS.DSL.TQD_DEFECT().GetData("SELECT_DEFECT_FOR_GDS", null,
                    new string[] { step.Seq.ToString(CultureInfo.InvariantCulture) });
                step.DefectTotal = rows.Rows.Count;
                foreach (DataRow row in rows.Rows)
                {
                    var point = new GdsKlarfDefectPoint
                    {
                        DefectId = ToLong(row["DEFECTID"]),
                        XIndex = (int)ToLong(row["XINDEX"]),
                        YIndex = (int)ToLong(row["YINDEX"]),
                        XRel = ToDouble(row["XREL"]),
                        YRel = ToDouble(row["YREL"]),
                        XSize = ToDouble(row["XSIZE"]),
                        YSize = ToDouble(row["YSIZE"]),
                        ClassNumber = (int)ToLong(row["CLASSNUMBER"])
                    };
                    int roughBin = (int)ToLong(row["ROUGHBINNUMBER"]);
                    int classValue = run.ClassBasis == GdsKlarfAnalysisRequest.ClassBasisClass ? point.ClassNumber : roughBin;
                    if (run.ClassFilter != null && !run.ClassFilter.Contains(classValue)) { step.ClassRemoved++; continue; }
                    bool large = Math.Max(Math.Abs(point.XSize), Math.Abs(point.YSize)) >= LargeDefectUm;
                    if (large) step.LargeCount++;
                    if (large && run.ExcludeLarge) { step.LargeExcluded++; continue; }
                    DiePoint sys = ToSystem(step, point.XIndex, point.YIndex);
                    var d = new DefectRun { Step = step, Point = point, RoughBin = roughBin, Large = large, Sys = sys, Index = FromSystem(run.Display, sys) };
                    step.Defects.Add(d);
                    run.Defects.Add(d);
                }
            }
            run.DefectMs = watch.ElapsedMilliseconds;
            if (run.Defects.Count > DefectLimit)
                throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture,
                    "Defect 가 {0:N0}개로 한 번에 판정할 수 있는 {1:N0}개를 넘습니다. 분류 거르기나 Step 수를 줄여 다시 분석하세요.", run.Defects.Count, DefectLimit));

            // 5) E-Test (고른 파일이 있을 때만): Chain 이름 연결 / Die 판정 / (Die, Chain) 판정
            if (run.EtSrcFileSeq > 0)
            {
                watch.Restart();
                run.Et = LoadEt(run.EtSrcFileSeq, run.Factory, run.Product, run.ReadyRows);
                run.EtMs = watch.ElapsedMilliseconds;
            }
            return run;
        }

        /// <summary>Step 마다 모양 판정을 하고 Defect 별 Chain 거리 / 걸린 Chain 을 채운다. only 가 있으면 그 Defect 만 판정한다.</summary>
        private static void Match(AnalysisRun run, Func<DefectRun, bool> only)
        {
            var watch = Stopwatch.StartNew();
            HashSet<long> filter = run.ChainFilter == null ? null : new HashSet<long>(run.Chains.Keys);
            foreach (StepRun step in run.Steps)
            {
                List<DefectRun> list = only == null ? step.Defects : step.Defects.Where(only).ToList();
                if (list.Count == 0) continue;
                var options = new GdsKlarfMatchOptions
                {
                    PitchX = step.PitchX,
                    PitchY = step.PitchY,
                    OffsetX = run.OffsetX,
                    OffsetY = run.OffsetY,
                    Tolerance = run.Tolerance,
                    UseDefectSize = true,
                    SquareSide = run.Square,
                    SearchDistance = run.Search,
                    Mirror = step.Mirror
                };
                List<GdsKlarfDefectGap> gaps = GdsKlarfMatchEngine.MatchShapes(run.Index, list.Select(d => d.Point).ToList(), options, filter);
                for (int i = 0; i < list.Count; i++)
                {
                    DefectRun d = list[i];
                    d.Gap = gaps[i];
                    d.ByChain = GdsKlarfMatchEngine.NearestByChain(d.Gap, run.Index.Shapes);
                    d.HitChains = OrderSeqs(d.ByChain.Where(p => p.Value.Gap <= run.HitGap).Select(p => p.Key), run.ChainCode).ToList();
                    double stored = step.Mirror ? -d.Gap.ViewX : d.Gap.ViewX;
                    d.GdsX = run.Display.Mirror ? -stored : stored;
                    run.Matched.Add(d);
                }
            }
            run.MatchMs = watch.ElapsedMilliseconds;
        }

        /// <summary>
        /// KLARF Index -> 시스템 Die 좌표. 검사 면과 상관없이 그대로 쓴다(KLARF / E-Test 는 데이터 그대로, 2026-10-20).
        /// KLARF Index = 고객 대응표(U9 76 Die)와 맞는 DieCoordType.KlarfBack 규칙. 면에 따른 Die 변환은 여러 Step(설계 B16, 2차) 때 다시 본다.
        /// </summary>
        private static DiePoint ToSystem(StepRun step, int x, int y)
        {
            return new DiePoint(x, y);
        }

        /// <summary>시스템 Die 좌표 -> 그 Step 의 KLARF Index (그대로).</summary>
        private static DiePoint FromSystem(StepRun step, DiePoint sys)
        {
            return sys;
        }

        #endregion

        #region [결과 표]

        /// <summary>
        /// 판정한 Defect 로 결과 표를 만든다. dieMode = AnalyzeDie(그 Die 1개, 탐색 거리 안 쌍 전부 + Member 단위 DEFECT_HIT).
        /// </summary>
        private static DataSet BuildAnalysis(AnalysisRun run, bool dieMode, DiePoint? targetSys, DiePoint? targetIndex)
        {
            var ds = new DataSet("GDS_KLARF_ANALYSIS");
            ds.RemotingFormat = SerializationFormat.Binary;
            bool multiStep = run.Steps.Count > 1;

            // Die 집계 (시스템 Die 좌표). Wafer = Map Step 검사 Die + Defect Die + E-Test Die, Die 상세 = 그 Die 1개
            var dies = new Dictionary<DiePoint, DieAgg>();
            var dieOrder = new List<DieAgg>();
            Func<DiePoint, DiePoint, DieAgg> get = (sys, index) =>
            {
                DieAgg a;
                if (!dies.TryGetValue(sys, out a))
                {
                    a = new DieAgg { Sys = sys, Index = index };
                    dies.Add(sys, a);
                    dieOrder.Add(a);
                }
                return a;
            };
            var planSys = new HashSet<DiePoint>();
            foreach (DiePoint p in run.Display.Plan)
            {
                DiePoint sys = ToSystem(run.Display, p.X, p.Y);
                planSys.Add(sys);
                if (!dieMode) get(sys, p).InPlan = true;
            }
            if (dieMode) get(targetSys.Value, targetIndex.Value).InPlan = planSys.Contains(targetSys.Value);
            foreach (DefectRun d in run.Matched)
            {
                DieAgg a = get(d.Sys, d.Index);
                a.Total++;
                if (d.HitChains.Count > 0) a.InChain++;
                foreach (long seq in d.HitChains)
                {
                    List<DefectRun> list;
                    if (!a.Hits.TryGetValue(seq, out list)) a.Hits.Add(seq, list = new List<DefectRun>());
                    list.Add(d);
                }
                foreach (KeyValuePair<long, GdsKlarfShapeGap> p in d.ByChain)
                {
                    NearDefect n;
                    if (!a.Near.TryGetValue(p.Key, out n) || p.Value.Gap < n.Gap) a.Near[p.Key] = new NearDefect { Gap = p.Value.Gap, Defect = d };
                }
            }
            if (run.Et != null && !dieMode)
                foreach (EtDie e in run.Et.Dies.Values)
                    get(e.Sys, FromSystem(run.Display, e.Sys));
            foreach (DieAgg a in dieOrder)
            {
                a.Inspected = run.PlanSys.Contains(a.Sys) || a.Total > 0;
                EtDie e;
                if (run.Et != null && run.Et.Dies.TryGetValue(a.Sys, out e)) a.Et = e;
            }

            // DIE_CHAIN: Wafer = 걸린 Defect 가 있거나 E-Test 상태가 F / N 인 칸, Die 상세 = 탐색 거리 안 + E-Test 연결 Chain 전부
            DataTable dieChainTable = NewTable("DIE_CHAIN",
                "XINDEX", typeof(int), "YINDEX", typeof(int), "SYS_X", typeof(int), "SYS_Y", typeof(int), "CHAIN_SEQ", typeof(long), "CHAIN_CODE", typeof(string),
                "DEFECT_COUNT", typeof(int), "DEFECT_IDS", typeof(string), "MIN_GAP", typeof(double), "NEAR_DEFECT_ID", typeof(long), "NEAR_STEP_SEQ", typeof(long),
                "NEAR_STEP_ID", typeof(string), "ET_STATUS", typeof(string), "ET_PARA_COUNT", typeof(int), "ET_FAIL_COUNT", typeof(int),
                "ET_FIRST_FAIL_ITEM", typeof(string), "ET_FIRST_FAIL_VALUE", typeof(string), "CLASS4", typeof(string));
            DataTable dieTable = NewTable("DIE",
                "XINDEX", typeof(int), "YINDEX", typeof(int), "SYS_X", typeof(int), "SYS_Y", typeof(int), "IN_PLAN", typeof(string),
                "DEFECT_COUNT", typeof(int), "IN_CHAIN_COUNT", typeof(int), "OUT_CHAIN_COUNT", typeof(int), "HIT_CHAIN_COUNT", typeof(int),
                "TOP_CHAINS", typeof(string), "ET_STATE", typeof(string), "ET_FAIL_CHAIN_COUNT", typeof(int), "BOTH_CHAIN_COUNT", typeof(int),
                "ET_FILE_FLAG", typeof(string));
            var chainHitDies = new Dictionary<long, int>();
            var chainHitDefects = new Dictionary<long, int>();
            // Chain 별 Die 수 (설계 7.2절): 대상 = KLARF 검사 Die 이면서 그 Chain 의 E-Test 상태가 P / F 인 Die
            var judged = new Dictionary<long, int[]>();   // { 판정, Defect, Fail, 일치 }
            bool etLoaded = run.Et != null;
            foreach (DieAgg a in dieOrder.OrderByDescending(x => x.InChain).ThenByDescending(x => x.Total).ThenByDescending(x => x.Index.Y).ThenBy(x => x.Index.X))
            {
                var perChain = a.Hits.OrderByDescending(p => p.Value.Count).ThenBy(p => run.ChainCode[p.Key].Length)
                    .ThenBy(p => run.ChainCode[p.Key], StringComparer.Ordinal).ToList();
                foreach (KeyValuePair<long, List<DefectRun>> p in perChain)
                {
                    chainHitDies[p.Key] = (chainHitDies.ContainsKey(p.Key) ? chainHitDies[p.Key] : 0) + 1;
                    chainHitDefects[p.Key] = (chainHitDefects.ContainsKey(p.Key) ? chainHitDefects[p.Key] : 0) + p.Value.Count;
                }
                IEnumerable<long> etChains = a.Et == null ? Enumerable.Empty<long>() : a.Et.Chains.Keys.Where(k => run.Chains.ContainsKey(k));
                IEnumerable<long> rowChains = dieMode
                    ? OrderSeqs(a.Near.Keys.Union(a.Hits.Keys).Union(etChains), run.ChainCode)
                    : OrderSeqs(perChain.Select(p => p.Key).Union(etChains.Where(k => a.Et.Chains[k].Status != "P")), run.ChainCode)
                        .OrderByDescending(k => a.Hits.ContainsKey(k) ? a.Hits[k].Count : 0);
                int failChains = 0, bothChains = 0;
                foreach (long seq in etChains)
                {
                    EtChainJudge j = a.Et.Chains[seq];
                    bool hit = a.Hits.ContainsKey(seq);
                    if (j.Status == "F") failChains++;
                    if (j.Status == "F" && hit && a.Inspected) bothChains++;
                    if (!a.Inspected || (j.Status != "P" && j.Status != "F")) continue;
                    int[] c;
                    if (!judged.TryGetValue(seq, out c)) judged.Add(seq, c = new int[4]);
                    c[0]++;
                    if (hit) c[1]++;
                    if (j.Status == "F") c[2]++;
                    if (j.Status == "F" && hit) c[3]++;
                }
                foreach (long seq in rowChains)
                {
                    List<DefectRun> hits;
                    if (!a.Hits.TryGetValue(seq, out hits)) hits = new List<DefectRun>();
                    NearDefect n;
                    a.Near.TryGetValue(seq, out n);
                    EtChainJudge j = null;
                    if (a.Et != null) a.Et.Chains.TryGetValue(seq, out j);
                    dieChainTable.Rows.Add(a.Index.X, a.Index.Y, a.Sys.X, a.Sys.Y, seq, run.ChainCode[seq], hits.Count,
                        String.Join(",", hits.Select(d => DefectLabel(d, multiStep)).ToArray()),
                        n == null ? (object)DBNull.Value : Round(n.Gap), n == null ? (object)DBNull.Value : n.Defect.Point.DefectId,
                        n == null ? (object)DBNull.Value : n.Defect.Step.Seq, n == null ? "" : n.Defect.Step.StepId,
                        j == null ? "" : j.Status, j == null ? (object)DBNull.Value : j.ParaCount, j == null ? (object)DBNull.Value : j.FailCount,
                        j == null ? "" : j.FirstFailItem, j == null ? "" : j.FirstFailValue,
                        Class4(etLoaded, a.Inspected, j, hits.Count > 0));
                }
                string top = String.Join(" ", perChain.Take(3).Select(p => run.ChainCode[p.Key] + "(" + p.Value.Count.ToString(CultureInfo.InvariantCulture) + ")").ToArray());
                dieTable.Rows.Add(a.Index.X, a.Index.Y, a.Sys.X, a.Sys.Y, a.InPlan ? "Y" : "N", a.Total, a.InChain, a.Total - a.InChain, a.Hits.Count, top,
                    a.Et == null ? "" : (a.Et.Judge.Length == 0 ? "N" : a.Et.Judge),
                    etLoaded ? (object)failChains : DBNull.Value, etLoaded ? (object)bothChains : DBNull.Value,
                    a.Et == null || a.Et.FilePass == null ? "" : a.Et.FilePass);
            }

            // CHAIN (+ E-Test 연결 / Chain 별 Die 수 / Fail률 2개)
            DataTable chainTable = NewTable("CHAIN",
                "CHAIN_SEQ", typeof(long), "CHAIN_CODE", typeof(string), "CHAIN_NAME", typeof(string), "COLOR_ARGB", typeof(int),
                "MEMBER_COUNT", typeof(int), "LAYER_IDS", typeof(string), "HIT_DIE_COUNT", typeof(int), "HIT_DEFECT_COUNT", typeof(int),
                "UPDATE_TIME", typeof(string), "ET_LINK_STATUS", typeof(string), "ET_PARA_COUNT", typeof(int), "DIE_JUDGED", typeof(int),
                "DIE_DEFECT", typeof(int), "DIE_FAIL", typeof(int), "DIE_BOTH", typeof(int), "FAIL_RATE_DEFECT", typeof(double), "FAIL_RATE_CLEAN", typeof(double));
            foreach (KeyValuePair<long, DataRow> p in run.Chains)
            {
                object color = p.Value["COLOR_ARGB"];
                int mc, hd, hf;
                string layers;
                object linkStatus = "", paraCount = DBNull.Value, dieJudged = DBNull.Value, dieDefect = DBNull.Value, dieFail = DBNull.Value, dieBoth = DBNull.Value;
                object rateDefect = DBNull.Value, rateClean = DBNull.Value;
                if (etLoaded)
                {
                    List<EtNameLink> links = run.Et.Links.Where(l => l.Status == LinkLinked && l.ChainSeq == p.Key).ToList();
                    linkStatus = links.Count > 0 ? LinkLinked : "NONE";
                    paraCount = links.Sum(l => l.ParaCount);
                    int[] c;
                    if (!judged.TryGetValue(p.Key, out c)) c = new int[4];
                    dieJudged = c[0];
                    dieDefect = c[1];
                    dieFail = c[2];
                    dieBoth = c[3];
                    if (c[1] > 0) rateDefect = Math.Round((double)c[3] / c[1], 4);
                    if (c[0] - c[1] > 0) rateClean = Math.Round((double)(c[2] - c[3]) / (c[0] - c[1]), 4);
                }
                chainTable.Rows.Add(p.Key, run.ChainCode[p.Key], Text(p.Value["CHAIN_NAME"]),
                    color == null || color == DBNull.Value ? 0 : (int)ToLong(color),
                    run.ChainMembers.TryGetValue(p.Key, out mc) ? mc : 0, run.ChainLayers.TryGetValue(p.Key, out layers) ? layers : "",
                    chainHitDies.TryGetValue(p.Key, out hd) ? hd : 0, chainHitDefects.TryGetValue(p.Key, out hf) ? hf : 0,
                    Text(p.Value["UPDATE_TIME"]), linkStatus, paraCount, dieJudged, dieDefect, dieFail, dieBoth, rateDefect, rateClean);
            }

            // DEFECT / DEFECT_CHAIN (+ Die 상세 DEFECT_HIT)
            DataTable defectTable = NewTable("DEFECT",
                "STEP_SEQ", typeof(long), "STEP_ID", typeof(string), "DEFECTID", typeof(long), "XINDEX", typeof(int), "YINDEX", typeof(int),
                "SYS_X", typeof(int), "SYS_Y", typeof(int), "CLASSNUMBER", typeof(int), "ROUGHBINNUMBER", typeof(int),
                "XSIZE", typeof(double), "YSIZE", typeof(double), "XREL", typeof(double), "YREL", typeof(double), "GDS_X", typeof(double), "GDS_Y", typeof(double),
                "IN_CHAIN", typeof(string), "CHAIN_CODES", typeof(string), "HIT_MEMBER_COUNT", typeof(int), "GAP", typeof(double),
                "NEAR_CHAIN_SEQ", typeof(long), "NEAR_CHAIN_CODE", typeof(string), "LARGE", typeof(string));
            DataTable defectChainTable = NewTable("DEFECT_CHAIN",
                "STEP_SEQ", typeof(long), "STEP_ID", typeof(string), "DEFECTID", typeof(long), "CHAIN_SEQ", typeof(long), "CHAIN_CODE", typeof(string),
                "GAP", typeof(double), "NEAR_PLACED_ELEMENT_ID", typeof(string));
            DataTable hitTable = NewTable("DEFECT_HIT",
                "STEP_SEQ", typeof(long), "STEP_ID", typeof(string), "DEFECTID", typeof(long), "CHAIN_SEQ", typeof(long), "CHAIN_CODE", typeof(string),
                "PLACED_ELEMENT_ID", typeof(string), "LAYER_ID", typeof(int), "ROLE_CODE", typeof(string),
                "MIN_X", typeof(double), "MIN_Y", typeof(double), "MAX_X", typeof(double), "MAX_Y", typeof(double), "GAP", typeof(double));
            bool allRows = dieMode || run.Matched.Count <= DefectRowsAll;
            foreach (DefectRun d in run.Matched)
            {
                bool near = d.Gap.Near.Count > 0;
                if (!allRows && !near && d.HitChains.Count == 0) continue;
                long nearChain = near ? run.Index.Shapes[d.Gap.Near[0].ShapeIndex].ChainSeq : 0;
                int hitMembers = d.Gap.Near.Count(g => g.Gap <= run.HitGap);
                GdsKlarfDefectPoint p = d.Point;
                defectTable.Rows.Add(d.Step.Seq, d.Step.StepId, p.DefectId, d.Index.X, d.Index.Y, d.Sys.X, d.Sys.Y, p.ClassNumber, d.RoughBin,
                    p.XSize, p.YSize, p.XRel, p.YRel, Round(d.GdsX), Round(d.Gap.ViewY), d.HitChains.Count > 0 ? "Y" : "N",
                    String.Join(",", d.HitChains.Select(s => run.ChainCode[s]).ToArray()), hitMembers,
                    near ? (object)Round(d.Gap.MinGap) : DBNull.Value, near ? (object)nearChain : DBNull.Value, near ? run.ChainCode[nearChain] : "",
                    d.Large ? "Y" : "N");
                IEnumerable<long> pairChains = dieMode ? OrderSeqs(d.ByChain.Keys, run.ChainCode) : d.HitChains;
                foreach (long seq in pairChains)
                {
                    GdsKlarfShapeGap g = d.ByChain[seq];
                    defectChainTable.Rows.Add(d.Step.Seq, d.Step.StepId, p.DefectId, seq, run.ChainCode[seq], Round(g.Gap), run.Index.Shapes[g.ShapeIndex].PlacedElementId);
                }
                if (!dieMode) continue;
                foreach (GdsKlarfShapeGap g in d.Gap.Near.Where(x => x.Gap <= run.HitGap))
                {
                    GdsKlarfShape s = run.Index.Shapes[g.ShapeIndex];
                    hitTable.Rows.Add(d.Step.Seq, d.Step.StepId, p.DefectId, s.ChainSeq, run.ChainCode[s.ChainSeq], s.PlacedElementId, s.LayerId, s.RoleCode,
                        s.MinX, s.MinY, s.MaxX, s.MaxY, Round(g.Gap));
                }
            }
            ds.ExtendedProperties["DEFECT_ROWS"] = allRows ? "ALL" : "NEAR";

            // TOL_CURVE: 한 번 구한 Gap 으로 단계별로 센다(크기 + 허용오차 방식만)
            DataTable curve = NewTable("TOL_CURVE", "TOL_UM", typeof(double), "IN_CHAIN_DEFECT", typeof(int), "HIT_DIE", typeof(int));
            if (!dieMode && run.AreaMode == GdsKlarfAnalysisRequest.AreaSizeTolerance)
                foreach (double t in TolSteps.Concat(new[] { run.Tolerance }).Distinct().OrderBy(t => t))
                {
                    double tol = t;
                    List<DefectRun> inside = run.Matched.Where(d => d.Gap.MinGap <= tol).ToList();
                    curve.Rows.Add(tol, inside.Count, inside.Select(d => d.Sys).Distinct().Count());
                }

            DataTable etLink = NewTable("ET_LINK", "GDS_CHAIN_KEY", typeof(string), "GDS_CHAIN_NAME", typeof(string), "CHAIN_SEQ", typeof(long),
                "CHAIN_CODE", typeof(string), "STATUS", typeof(string), "PARA_COUNT", typeof(int), "CHAIN2_PARA_COUNT", typeof(int));
            if (etLoaded)
                foreach (EtNameLink l in run.Et.Links)
                    etLink.Rows.Add(l.Key, l.Name, l.Status == LinkLinked ? (object)l.ChainSeq : DBNull.Value, l.ChainCode, l.Status, l.ParaCount, l.Chain2ParaCount);

            ds.Tables.Add(chainTable);
            ds.Tables.Add(dieTable);
            ds.Tables.Add(dieChainTable);
            ds.Tables.Add(defectTable);
            ds.Tables.Add(defectChainTable);
            if (dieMode)
            {
                ds.Tables.Add(hitTable);
                DataTable values = NewTable("ET_VALUE", "XINDEX", typeof(int), "YINDEX", typeof(int), "CHAIN_SEQ", typeof(long), "ITEM_NAME", typeof(string),
                    "HEADER_TEXT", typeof(string), "CHAIN_TEXT", typeof(string), "GDS_CHAIN_NAME", typeof(string), "MEAS_VALUE", typeof(string),
                    "LOWER_LIMIT", typeof(string), "UPPER_LIMIT", typeof(string), "JUDGE_FLAG", typeof(string), "UNIT", typeof(string));
                DieAgg target = dies[targetSys.Value];
                if (target.Et != null)
                    foreach (DataRow r in new DACrux.SEMDMS.DSL.TQP_GDS_ET_VALUE().GetData("SELECT_ET_VALUE_OF_DIE", null, new string[] {
                        run.EtSrcFileSeq.ToString(CultureInfo.InvariantCulture), target.Et.DutNo.ToString(CultureInfo.InvariantCulture) }).Rows)
                    {
                        long chain;
                        bool linked = run.Et.KeyToChain.TryGetValue(Text(r["GDS_CHAIN_KEY"]), out chain) && run.Chains.ContainsKey(chain);
                        string chain2 = Text(r["CHAIN_NAME2"]);
                        values.Rows.Add(target.Index.X, target.Index.Y, linked ? (object)chain : DBNull.Value, Text(r["ITEM_NAME"]), Text(r["HEADER_TEXT"]),
                            Text(r["CHAIN_NAME"]) + (chain2.Length == 0 ? "" : " : " + chain2), Text(r["GDS_CHAIN_NAME"]),
                            ValueText(r["MEAS_VALUE"], Text(r["VALUE_STATUS"])), ValueText(r["LOWER_LIMIT"], ""), ValueText(r["UPPER_LIMIT"], ""),
                            Text(r["JUDGE_FLAG"]), Text(r["UNIT"]));
                    }
                ds.Tables.Add(values);
            }
            else
            {
                ds.Tables.Add(curve);
                ds.Tables.Add(etLink);
            }
            return ds;
        }

        /// <summary>ANL_INFO 1행. AnalyzeWafer 와 같은 열을 먼저 두고 통합 분석 열을 더한다.</summary>
        private static void AddInfo(DataSet ds, AnalysisRun run, Stopwatch build, DiePoint? dieIndex, string focusChainSeq)
        {
            DataRow s = run.Display.Info;
            EtData et = run.Et;
            bool square = run.AreaMode == GdsKlarfAnalysisRequest.AreaSquare;
            string viewSide = run.Display.Mirror ? Opposite(run.MapSide) : run.MapSide;
            DataTable info = NewTable("ANL_INFO",
                "FACTORY", typeof(string), "DEVICE_ID", typeof(string), "STEP_SEQ", typeof(long), "PRODUCT", typeof(string),
                "LOT_ID", typeof(string), "WAFER_ID", typeof(string), "STEP_ID", typeof(string), "SLOT_ID", typeof(string),
                "RESULTTIMESTAMP", typeof(string), "MAP_VIEW_SIDE", typeof(string), "VIEW_SIDE", typeof(string), "MIRROR", typeof(string),
                "TOLERANCE_UM", typeof(double), "USE_DEFECT_SIZE", typeof(string), "OFFSET_X", typeof(double), "OFFSET_Y", typeof(double),
                "PITCH_X", typeof(double), "PITCH_Y", typeof(double), "CHAIN_COUNT", typeof(int), "DRAFT_CHAIN_COUNT", typeof(int),
                "MEMBER_COUNT", typeof(int), "DEFECT_TOTAL", typeof(int), "DEFECT_COUNT", typeof(int), "IN_CHAIN_DEFECT_COUNT", typeof(int),
                "PLAN_DIE_COUNT", typeof(int), "DEFECT_DIE_COUNT", typeof(int), "CHAIN_FILTER", typeof(string), "CLASS_FILTER", typeof(string),
                "CHAIN_LAST_SAVED", typeof(string), "ANALYZED_AT", typeof(string), "QUERY_MS", typeof(long), "MATCH_MS", typeof(long),
                "ELAPSED_MS", typeof(long),
                "AREA_MODE", typeof(string), "SQUARE_UM", typeof(double), "HIT_GAP_UM", typeof(double), "SEARCH_UM", typeof(double),
                "WAFER_KEY", typeof(string), "STEP_SEQS", typeof(string), "STEP_IDS", typeof(string), "STEP_SIDES", typeof(string),
                "CHAIN_SIDE", typeof(string), "DISPLAY_STEP_SEQ", typeof(long), "CLASS_BASIS", typeof(string), "EXCLUDE_LARGE", typeof(string),
                "LARGE_DEFECT_COUNT", typeof(int), "LARGE_EXCLUDED_COUNT", typeof(int), "PITCH_SOURCE", typeof(string), "CACHE_HIT", typeof(string),
                "INFO_MS", typeof(long), "GDS_MS", typeof(long), "DEFECT_MS", typeof(long), "ET_MS", typeof(long), "RESULT_MS", typeof(long), "DEFECT_ROWS", typeof(string),
                "ET_SRC_FILE_SEQ", typeof(long), "ET_WAFER_ID", typeof(string), "ET_TEST_PROGRAM", typeof(string), "ET_JUDGE_TIME", typeof(string),
                "ET_SPEC_CHANGED", typeof(string), "ET_DIE_COUNT", typeof(int), "ET_OUT_OF_MAP_COUNT", typeof(int), "MAP_SHAPE_SAME", typeof(string),
                "DIE_X", typeof(int), "DIE_Y", typeof(int), "FOCUS_CHAIN_SEQ", typeof(string));
            build.Stop();
            run.Total.Stop();
            info.Rows.Add(run.Factory, run.DeviceId, run.Display.Seq, run.Product, Text(s["LOT_ID"]), Text(s["WAFER_ID"]), run.Display.StepId,
                Text(s["SLOT_ID"]), Text(s["RESULTTIMESTAMP"]), run.MapSide, viewSide, run.Display.Mirror ? "Y" : "N",
                square ? run.Square / 2 : run.Tolerance, square ? "N" : "Y", run.OffsetX, run.OffsetY, run.Display.PitchX, run.Display.PitchY,
                run.Chains.Count, run.DraftCount, run.MemberCount, run.Steps.Sum(x => x.DefectTotal), run.Matched.Count,
                run.Matched.Count(d => d.HitChains.Count > 0), run.Display.Plan.Count, run.Matched.Select(d => d.Sys).Distinct().Count(),
                run.ChainFilter == null ? "ALL" : String.Join(",", run.ChainFilter.OrderBy(x => x).Select(x => x.ToString(CultureInfo.InvariantCulture)).ToArray()),
                run.ClassFilter == null ? "ALL" : String.Join(",", run.ClassFilter.OrderBy(x => x).Select(x => x.ToString(CultureInfo.InvariantCulture)).ToArray()),
                run.Chains.Count == 0 ? "" : run.Chains.Values.Select(r => Text(r["UPDATE_TIME"])).Max(),
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                run.InfoMs + run.DefectMs, run.MatchMs, run.Total.ElapsedMilliseconds,
                run.AreaMode, square ? run.Square : 0, run.HitGap, run.Search,
                run.WaferKey, String.Join(",", run.Steps.Select(x => x.Seq.ToString(CultureInfo.InvariantCulture)).ToArray()),
                String.Join(",", run.Steps.Select(x => x.StepId).ToArray()), String.Join(",", run.Steps.Select(x => x.Side).ToArray()),
                run.ChainSide, run.Display.Seq, run.ClassBasis, run.ExcludeLarge ? "Y" : "N",
                run.Steps.Sum(x => x.LargeCount), run.Steps.Sum(x => x.LargeExcluded),
                run.Steps.Any(x => x.PitchSource == "MAP_SETUP") ? "MAP_SETUP" : "KLARF", run.CacheHit ? "Y" : "N",
                run.InfoMs, run.GdsMs, run.DefectMs, run.EtMs, build.ElapsedMilliseconds, Convert.ToString(ds.ExtendedProperties["DEFECT_ROWS"], CultureInfo.InvariantCulture),
                run.EtSrcFileSeq, et == null ? "" : Text(et.File["WAFER_ID"]), et == null ? "" : Text(et.File["TEST_PROGRAM"]),
                et == null ? "" : Text(et.File["JUDGE_TIME"]), et == null ? "" : Text(et.File["SPEC_CHANGED"]),
                et == null ? (object)DBNull.Value : et.Dies.Count, et == null ? (object)DBNull.Value : et.OutOfMap, et == null ? "" : et.MapShapeSame,
                dieIndex.HasValue ? (object)dieIndex.Value.X : DBNull.Value, dieIndex.HasValue ? (object)dieIndex.Value.Y : DBNull.Value, focusChainSeq ?? "");
            ds.Tables.Add(info);
        }

        /// <summary>Defect 표시 이름: Step 이 여러 개면 "Step ID:DefectId" (Step 마다 DEFECTID 가 겹칠 수 있다).</summary>
        private static string DefectLabel(DefectRun d, bool multiStep)
        {
            string id = d.Point.DefectId.ToString(CultureInfo.InvariantCulture);
            return multiStep ? d.Step.StepId + ":" + id : id;
        }

        /// <summary>Chain 번호를 Chain Code 순서(길이 -> 문자)로 정렬한다.</summary>
        private static IEnumerable<long> OrderSeqs(IEnumerable<long> seqs, Dictionary<long, string> chainCode)
        {
            return seqs.Distinct().OrderBy(s => chainCode[s].Length).ThenBy(s => chainCode[s], StringComparer.Ordinal);
        }

        private static string Opposite(string side)
        {
            return side == SideBack ? SideFront : SideBack;
        }

        #endregion
    }
}
