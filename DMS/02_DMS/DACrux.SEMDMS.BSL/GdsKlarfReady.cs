using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using DACrux.SEMDMS.Interface;

namespace DACrux.SEMDMS.BSL
{
    /// <summary>
    /// 통합 연관 분석 Ready 조회 GetWaferReady (2026-10-16). 무거운 데이터(GDS 모양 / Defect / E-Test 값)는 읽지 않고 목록만 만든다.
    /// 설계: 문서/설계/05_연관분석/2026-10-15_GDS_KLARF_ETest_통합분석_데이터_설계.md 16.2 ~ 16.6 / 16.8절
    ///
    /// 처리 흐름
    ///  1) 고른 Step 정보(SELECT_DMS_WAFER_INFO) + 원본 Wafer ID(TQD_STEP.COMMENT) -> WaferKeyRule.FromKlarf
    ///  2) 같은 Wafer KLARF Step: SELECT_KLARF_STEP_BY_WAFER_PRODUCT(Lot 글자 / Slot / 검사 시각 ± ReadyDays / 고른 Step 과 같은 Product) -> FromKlarf 가 같은 것만
    ///     -> 같은 Step ID 재검사 정리(GdsKlarfReadyStepRule: Step ID -> TEST_ORDER 순, Step ID 마다 기본 체크 1행) (2026-10-16)
    ///  3) 같은 Wafer E-Test 파일: SELECT_ET_FILE_BY_WAFER(Lot 글자 / Slot) -> FromETest 가 같고 Device 가 분석 GDS Device 와 같은 것만(2026-10-20), Chain 이름 연결 수 / Map 형상
    ///  4) GDS Device / Map / Chain 수 / 저장 시각 / Cache 여부
    ///  5) 점검(READY_CHECK): 오류 / 주의 / 정보
    ///
    /// 결과 테이블(모든 열 문자열, 화면 frmGdsKlarfAnalysis.NewReadySet 과 같은 모양)
    ///  WAFER_KEY   1행: WAFER_KEY / RULE_NAME / CONFIRMED / STEP_SEQ / FACTORY
    ///  READY_GDS   1행: DEVICE_ID / MAP_STATUS / READY_CHAIN / DRAFT_CHAIN / CHAIN_LAST_SAVED / CACHE_HIT / CHAIN_SIDE(기본 BACK, 2026-10-20 변경)
    ///  READY_KLARF Step: STEP_SEQ / STEP_ID / LOT_ID / RESULTTIMESTAMP / DEFECTS / INSPECTED_DIE / VIEW_SIDE(KLARF 검사 면, FRONT 고정 = 2026-10-20 변경, 이전 BACK) / PITCH_SOURCE /
    ///              ADDED_BY(RULE / PREV = 같은 Step 이전 검사 / NEXT = 이후 검사, 화면 [직접 찾기] = HAND) / USE_FLAG
    ///  READY_ET    파일: SRC_FILE_SEQ / TEST_PROGRAM / WAFER_ID / DEVICE_ID / TEST_TIME / DIE_COUNT / FAIL_DIE_COUNT / JUDGE_TIME / SPEC_CHANGED /
    ///              LINKED_CHAIN / UNLINKED_NAME / MAP_SHAPE_SAME / ADDED_BY / SELECTED(Test 시각 최신 1개 = Y, 결정 B19)
    ///  READY_CHECK 점검: CODE / LEVEL(오류 / 주의 / 정보) / MESSAGE
    /// </summary>
    public partial class GdsKlarfAnalysis
    {
        /// <summary>같은 Wafer KLARF Step 을 찾는 검사 시각 범위(고른 Step 기준 ± 일, 설계 16.8절).</summary>
        private const int ReadyDays = 180;
        private const string LevelError = "오류";
        private const string LevelWarn = "주의";
        private const string LevelInfo = "정보";

        /// <summary>통합 연관 분석 Ready 조회. strDeviceID 가 비면 고른 Step 의 Product 와 이름이 같은 READY GDS Device.</summary>
        public DataSet GetWaferReady(string strFactory, long lStepSeq, string strDeviceID)
        {
            string factory = Required(strFactory, "Factory", 30);
            if (lStepSeq <= 0) throw new ArgumentException("STEP_SEQ는 1 이상이어야 합니다.", "lStepSeq");
            DataTable stepInfo = new DACrux.SEMDMS.DSL.TQD_INSP_INFO().GetData("SELECT_DMS_WAFER_INFO", null,
                new string[] { lStepSeq.ToString(CultureInfo.InvariantCulture) });
            if (stepInfo == null || stepInfo.Rows.Count == 0)
                throw new ArgumentException("KLARF Wafer 정보가 없습니다. STEP_SEQ=" + lStepSeq, "lStepSeq");
            DataRow step = stepInfo.Rows[0];
            string product = Text(step["PRODUCT"]);
            string stepComment = "";
            DataTable stepRow = new DACrux.SEMDMS.DSL.TQD_STEP().GetData("SELECT_STEP_KLARF_UPLOAD", null,
                new string[] { lStepSeq.ToString(CultureInfo.InvariantCulture), Text(step["WAFER_SEQ"]) });
            if (stepRow != null && stepRow.Rows.Count > 0) stepComment = Text(stepRow.Rows[0]["STEP_COMMENT"]);
            string key = WaferKeyRule.FromKlarf(Text(step["LOT_ID"]), Text(step["WAFER_ID"]), Text(step["SLOT_ID"]), CommentValue(stepComment, "RAW_WAFER_ID"));
            WaferKeySearch search = key == null ? null : WaferKeyRule.SearchOf(key);

            DataSet ds = NewReadyTables();
            ds.Tables["WAFER_KEY"].Rows.Add(key ?? "", WaferKeyRule.RuleName, WaferKeyRule.Confirmed ? "Y" : "N",
                lStepSeq.ToString(CultureInfo.InvariantCulture), factory);
            DataTable check = ds.Tables["READY_CHECK"];

            // 2) KLARF Step
            // 고른 Step 과 같은 Product 만 모은다(결정 B26). 같은 파일을 다시 올리면 업로드가 지우고 다시 쓰지만 그 키에 Product 가 들어 있어,
            // Product 만 다르게 올린 같은 파일은 둘 다 남는다. 같은 Step ID 재검사는 TEST_ORDER 로 정리한다(결정 B27). (2026-10-16)
            DataTable klarf = ds.Tables["READY_KLARF"];
            var foundSteps = new List<DataRow>();
            DateTime picked;
            bool hasTime = DateTime.TryParseExact(Text(step["RESULTTIMESTAMP"]), "MM-dd-yy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out picked);
            if (search != null && hasTime)
            {
                DataTable rows = new DACrux.SEMDMS.DSL.TQD_INSP_INFO().GetData("SELECT_KLARF_STEP_BY_WAFER_PRODUCT", null, new string[] {
                    factory, search.LotLike, search.Slot,
                    picked.AddDays(-ReadyDays).ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture),
                    picked.AddDays(ReadyDays).ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture), product });
                foreach (DataRow r in rows.Rows)
                    if (WaferKeyRule.FromKlarf(Text(r["LOT_ID"]), Text(r["WAFER_ID"]), Text(r["SLOT_ID"]), CommentValue(Text(r["STEP_COMMENT"]), "RAW_WAFER_ID")) == key)
                        foundSteps.Add(r);
            }
            bool pickedFound = foundSteps.Any(r => ToLong(r["STEP_SEQ"]) == lStepSeq);
            var stepEntries = new List<GdsKlarfReadyStepRule.Entry>();
            if (!pickedFound)
            {
                string pickedTime = hasTime ? picked.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : Text(step["RESULTTIMESTAMP"]);
                stepEntries.Add(new GdsKlarfReadyStepRule.Entry(lStepSeq, Text(step["STEP_ID"]), null, pickedTime, new object[] {
                    lStepSeq.ToString(CultureInfo.InvariantCulture), Text(step["STEP_ID"]), Text(step["LOT_ID"]), pickedTime,
                    "", "", SideFront, PitchSourceOf(stepComment), null, null }));   // KLARF 검사 면 FS 고정 (2026-10-20)
            }
            foreach (DataRow r in foundSteps)
                stepEntries.Add(new GdsKlarfReadyStepRule.Entry(ToLong(r["STEP_SEQ"]), Text(r["STEP_ID"]), GdsKlarfReadyStepRule.ParseOrder(r["TEST_ORDER"]),
                    Text(r["RESULTTIMESTAMP"]), new object[] {
                    Text(r["STEP_SEQ"]), Text(r["STEP_ID"]), Text(r["LOT_ID"]), Text(r["RESULTTIMESTAMP"]), Text(r["DEFECTS"]),
                    Text(r["INSPECTED_DIE"]), SideFront, PitchSourceOf(Text(r["STEP_COMMENT"])), null, null }));
            foreach (GdsKlarfReadyStepRule.Entry e in GdsKlarfReadyStepRule.Arrange(stepEntries, lStepSeq))
            {
                e.Values[8] = e.AddedBy;
                e.Values[9] = e.UseFlag;
                klarf.Rows.Add(e.Values);
            }

            // 4) GDS
            string device = strDeviceID == null ? "" : strDeviceID.Trim();
            if (device.Length == 0) device = ResolveMapDevice(factory, product) ?? "";
            DataTable mapInfo = device.Length == 0 ? null : new DACrux.SEMDMS.DSL.TQP_GDS_MAP().GetData("SELECT_MAP_INFO", null, new string[] { factory, device });
            bool mapReady = mapInfo != null && mapInfo.Rows.Count > 0 && Text(mapInfo.Rows[0]["MAP_STATUS"]) == "READY";
            var readyChains = new List<DataRow>();
            int draft = 0;
            if (mapReady)
                foreach (DataRow r in new DACrux.SEMDMS.DSL.TQP_GDS_CHAIN().GetData("SELECT_CHAIN_LIST", null, new string[] { factory, device }).Rows)
                {
                    if (Text(r["VERSION_STATUS"]) == "READY") readyChains.Add(r);
                    else draft++;
                }
            bool cached = mapReady && readyChains.Count > 0
                && GdsKlarfGeometryCache.Contains(factory, device, GdsKlarfGeometryCache.MakeStamp(mapInfo.Rows[0], readyChains));
            ds.Tables["READY_GDS"].Rows.Add(device, mapReady ? "READY" : mapInfo != null && mapInfo.Rows.Count > 0 ? Text(mapInfo.Rows[0]["MAP_STATUS"]) : "없음",
                readyChains.Count.ToString(CultureInfo.InvariantCulture), draft.ToString(CultureInfo.InvariantCulture),
                readyChains.Count == 0 ? "" : readyChains.Select(r => Text(r["UPDATE_TIME"])).Max(), cached ? "Y" : "N", SideBack);   // Chain 면 기본 BS (2026-10-20)

            // 3) E-Test 파일: 같은 Wafer Key + 분석 GDS Device 와 같은 Device 만 (2026-10-20).
            //    Step(파일 Step ID = TEST_PROGRAM)은 화면 [E-Test] 탭 Step 콤보로 고른다. Device 가 다른 같은 Wafer 파일은 점검 정보(ET_OTHER_DEVICE)로만 알린다
            DataTable et = ds.Tables["READY_ET"];
            var etFiles = new List<DataRow>();
            var otherDevice = new List<string>();
            if (search != null)
            {
                DataTable rows = new DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE().GetData("SELECT_ET_FILE_BY_WAFER", null, new string[] { factory, search.LotLike, search.Slot });
                foreach (DataRow r in rows.Rows)
                {
                    if (WaferKeyRule.FromETest(Text(r["LOT_ID"]), Text(r["SLOT_NO"]), Text(r["LOT_WAFER_NO"])) != key) continue;
                    if (device.Length > 0 && String.Equals(Text(r["DEVICE_ID"]).Trim(), device, StringComparison.OrdinalIgnoreCase)) etFiles.Add(r);
                    else otherDevice.Add(Text(r["DEVICE_ID"]) + " " + Text(r["TEST_PROGRAM"]));
                }
            }
            List<DiePoint> klarfMap = etFiles.Count == 0 ? new List<DiePoint>() : MapUsedDies(factory, product);
            string selectedUnlinked = "", selectedSpec = "", selectedShape = "", selectedName = "";
            for (int i = 0; i < etFiles.Count; i++)
            {
                DataRow r = etFiles[i];
                DataTable paraList;
                List<EtNameLink> links = mapReady
                    ? LinkChainNames(Text(r["FACTORY"]), Text(r["DEVICE_ID"]), Text(r["TEST_PROGRAM"]), readyChains, out paraList)
                    : new List<EtNameLink>();
                string shape = MapShapeSame(klarfMap, MapUsedDies(Text(r["FACTORY"]), Text(r["MAP_ID"])));
                string unlinked = UnlinkedText(links);
                et.Rows.Add(Text(r["SRC_FILE_SEQ"]), Text(r["TEST_PROGRAM"]), Text(r["WAFER_ID"]), Text(r["DEVICE_ID"]), Text(r["TEST_TIME"]),
                    Text(r["DIE_COUNT"]), Text(r["FAIL_DIE_COUNT"]), Text(r["JUDGE_TIME"]), Text(r["SPEC_CHANGED"]),
                    links.Count(l => l.Status == LinkLinked).ToString(CultureInfo.InvariantCulture), unlinked, shape, "RULE", i == 0 ? "Y" : "N");
                if (i == 0)
                {
                    selectedName = Text(r["TEST_PROGRAM"]) + " " + Text(r["WAFER_ID"]);
                    selectedUnlinked = unlinked;
                    selectedSpec = Text(r["SPEC_CHANGED"]);
                    selectedShape = shape;
                }
            }

            // 5) 점검
            if (device.Length == 0 || !mapReady)
                check.Rows.Add("GDS_NO_MAP", LevelError, "Product " + product + " 와 같은 이름의 READY GDS Map 이 없어 분석할 수 없습니다. (GDS Map 조회 화면 [Map 복사])");
            else if (readyChains.Count == 0)
                check.Rows.Add("GDS_NO_CHAIN", LevelError, "GDS Device " + device + " 에 저장 완료(READY) Chain 이 없어 분석할 수 없습니다.");
            if (draft > 0)
                check.Rows.Add("GDS_DRAFT", LevelWarn, "저장 미완료(DRAFT) Chain " + draft + "개는 분석에서 빠집니다.");
            if (key == null)
                check.Rows.Add("WAFER_KEY_NONE", LevelWarn, "Wafer Key 를 만들 수 없어(LOT_ID / Slot) 고른 Step 만 보입니다. 다른 Step / E-Test 는 [직접 찾기]로 더하세요.");
            else if (klarf.Rows.Count == 1)
                check.Rows.Add("KLARF_ONE", LevelInfo, "같은 Wafer 의 KLARF Step 을 1개만 찾았습니다." + (WaferKeyRule.Confirmed ? "" : " 규칙이 잠정이라 [직접 찾기]로 확인을 권장합니다."));
            if (otherDevice.Count > 0)
                check.Rows.Add("ET_OTHER_DEVICE", LevelInfo, "E-Test " + otherDevice.Count + "개는 Device 가 GDS Device " + (device.Length == 0 ? "-" : device)
                    + " 와 달라 목록에서 뺌(" + String.Join(", ", otherDevice.Distinct().ToArray()) + ")");   // 화면 상태줄에 그대로 보인다(사용법 안내 없이)
            if (etFiles.Count == 0)
                check.Rows.Add("ET_NONE", LevelInfo, "같은 Wafer 의 E-Test 파일이 없습니다. Defect 만 분석합니다(E-Test 열은 비움). 있으면 [직접 찾기]로 더하세요.");
            else
            {
                if (selectedSpec == "Y")
                    check.Rows.Add("ET_SPEC_CHANGED", LevelWarn, "E-Test " + selectedName + " 판정 뒤 Spec 이 바뀌었습니다. E-Test 결과 조회 화면에서 다시 판정하세요.");
                if (selectedUnlinked.Length > 0)
                    check.Rows.Add("ET_UNLINKED", LevelWarn, "E-Test GDS Chain 이름 미연결 " + selectedUnlinked + " - 분석 뒤 E-Test 연결 탭에서 확인하세요.");
                if (selectedShape == "N")
                    check.Rows.Add("MAP_SHAPE", LevelWarn, "KLARF Map 과 E-Test Map 의 사용 Die 범위 / 수가 다릅니다. Die 연결 결과를 확인하세요.");
            }
            if (!WaferKeyRule.Confirmed)
                check.Rows.Add("RULE_TENTATIVE", LevelInfo, "Wafer Key 규칙 잠정(" + WaferKeyRule.RuleName + ": " + WaferKeyRule.Describe() + "), 고객 협의 후 확정");
            if (klarf.Rows.Cast<DataRow>().Any(r => Text(r["PITCH_SOURCE"]) == "MAP_SETUP"))
                check.Rows.Add("PITCH_SOURCE", LevelInfo, "KLARF 원본 Die Pitch 기록이 없는 Step 은 Map Setup Pitch 로 분석합니다.");
            return ds;
        }

        /// <summary>Ready 결과 빈 틀 (모든 열 문자열).</summary>
        private static DataSet NewReadyTables()
        {
            var ds = new DataSet("READY");
            ds.RemotingFormat = SerializationFormat.Binary;
            AddStringTable(ds, "WAFER_KEY", "WAFER_KEY", "RULE_NAME", "CONFIRMED", "STEP_SEQ", "FACTORY");
            AddStringTable(ds, "READY_GDS", "DEVICE_ID", "MAP_STATUS", "READY_CHAIN", "DRAFT_CHAIN", "CHAIN_LAST_SAVED", "CACHE_HIT", "CHAIN_SIDE");
            AddStringTable(ds, "READY_KLARF", "STEP_SEQ", "STEP_ID", "LOT_ID", "RESULTTIMESTAMP", "DEFECTS", "INSPECTED_DIE", "VIEW_SIDE", "PITCH_SOURCE", "ADDED_BY", "USE_FLAG");
            AddStringTable(ds, "READY_ET", "SRC_FILE_SEQ", "TEST_PROGRAM", "WAFER_ID", "DEVICE_ID", "TEST_TIME", "DIE_COUNT", "FAIL_DIE_COUNT", "JUDGE_TIME",
                "SPEC_CHANGED", "LINKED_CHAIN", "UNLINKED_NAME", "MAP_SHAPE_SAME", "ADDED_BY", "SELECTED");
            AddStringTable(ds, "READY_CHECK", "CODE", "LEVEL", "MESSAGE");
            return ds;
        }

        private static void AddStringTable(DataSet ds, string name, params string[] columns)
        {
            DataTable t = ds.Tables.Add(name);
            foreach (string c in columns) t.Columns.Add(c, typeof(string));
        }

        /// <summary>TQD_STEP.COMMENT 의 "이름=값" (';' 로 구분). 없으면 빈 값.</summary>
        private static string CommentValue(string comment, string name)
        {
            foreach (string part in (comment ?? "").Split(';'))
            {
                int eq = part.IndexOf('=');
                if (eq > 0 && String.Equals(part.Substring(0, eq).Trim(), name, StringComparison.OrdinalIgnoreCase))
                    return part.Substring(eq + 1).Trim();
            }
            return "";
        }

        /// <summary>Pitch 출처: KLARF 업로드가 원본 Pitch(RAW_DIE_PITCH)를 남겼으면 KLARF, 아니면 Map Setup (결정 B6).</summary>
        private static string PitchSourceOf(string stepComment)
        {
            double x, y;
            return TryRawPitch(stepComment, out x, out y) ? "KLARF" : "MAP_SETUP";
        }

        /// <summary>TQD_STEP.COMMENT 의 RAW_DIE_PITCH=x,y (KLARF 업로드가 남긴 파일 DiePitch, µm). 없거나 0 이하면 false.</summary>
        private static bool TryRawPitch(string stepComment, out double pitchX, out double pitchY)
        {
            pitchX = pitchY = 0;
            string[] parts = CommentValue(stepComment, "RAW_DIE_PITCH").Split(',');
            return parts.Length == 2
                && double.TryParse(parts[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out pitchX) && pitchX > 0
                && double.TryParse(parts[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out pitchY) && pitchY > 0;
        }
    }

    /// <summary>
    /// [준비] 탭 KLARF Step 목록의 같은 Step 재검사 정리 (결정 B27, 2026-10-16).
    /// 같은 Wafer + 같은 Step ID 를 다시 검사해 올리면 KLARF 업로드가 기존 TQD_STEP.TEST_ORDER 를 1 씩 올리므로 가장 작은 값이 최신이다.
    /// 목록은 Step ID -> 최신 먼저 순으로 놓고, 같은 Step ID 에서는 한 행만 기본 체크한다:
    /// 고른 Step(Map 을 그리는 기준)이 그 Step ID 에 있으면 그것, 없으면 최신. 나머지는 체크를 끄고 이전 / 이후 검사로 표시한다.
    /// TEST_ORDER 는 WAFER_SEQ 안에서만 매겨지므로 값이 같거나 없으면 검사 시각이 늦은 쪽을 최신으로 본다.
    /// </summary>
    public static class GdsKlarfReadyStepRule
    {
        public const string AddedRule = "RULE";
        /// <summary>기본 체크한 행보다 먼저 검사한 같은 Step ID.</summary>
        public const string AddedPrevious = "PREV";
        /// <summary>기본 체크한 행보다 나중에 검사한 같은 Step ID(고른 Step 이 예전 검사일 때).</summary>
        public const string AddedNext = "NEXT";

        /// <summary>목록 한 행. Values 는 READY_KLARF 행 값이며 호출자가 ADDED_BY / USE_FLAG 칸을 채운다.</summary>
        public sealed class Entry
        {
            public Entry(long stepSeq, string stepId, int? testOrder, string resultTime, object[] values)
            {
                StepSeq = stepSeq;
                StepId = stepId ?? "";
                TestOrder = testOrder;
                ResultTime = resultTime ?? "";
                Values = values;
            }

            public long StepSeq { get; private set; }
            public string StepId { get; private set; }
            public int? TestOrder { get; private set; }
            /// <summary>검사 시각 yyyy-MM-dd HH:mm:ss (문자열 순서 = 시간 순서).</summary>
            public string ResultTime { get; private set; }
            public object[] Values { get; private set; }
            public string AddedBy { get; set; }
            public string UseFlag { get; set; }
        }

        /// <summary>TQD_STEP.TEST_ORDER 값. 비었거나 숫자가 아니면 null.</summary>
        public static int? ParseOrder(object value)
        {
            if (value == null || value == DBNull.Value) return null;
            int order;
            return int.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture).Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out order)
                ? (int?)order : null;
        }

        /// <summary>Step ID 순으로 묶고 묶음 안은 최신 먼저 놓은 뒤, Step ID 마다 기본 체크 1행을 정한다. 입력 목록은 바꾸지 않는다.</summary>
        public static List<Entry> Arrange(IEnumerable<Entry> entries, long pickedStepSeq)
        {
            var result = new List<Entry>();
            foreach (IGrouping<string, Entry> group in entries
                .GroupBy(e => e.StepId.Trim(), StringComparer.OrdinalIgnoreCase)
                .OrderBy(g => g.Key, StringComparer.OrdinalIgnoreCase))
            {
                List<Entry> ordered = group
                    .OrderBy(e => e.TestOrder.HasValue ? 0 : 1)
                    .ThenBy(e => e.TestOrder ?? 0)
                    .ThenByDescending(e => e.ResultTime, StringComparer.Ordinal)
                    .ThenByDescending(e => e.StepSeq)
                    .ToList();
                int chosen = ordered.FindIndex(e => e.StepSeq == pickedStepSeq);
                if (chosen < 0) chosen = 0;
                for (int i = 0; i < ordered.Count; i++)
                {
                    ordered[i].UseFlag = i == chosen ? "Y" : "N";
                    ordered[i].AddedBy = i == chosen ? AddedRule : i > chosen ? AddedPrevious : AddedNext;
                }
                result.AddRange(ordered);
            }
            return result;
        }
    }
}
