using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DACrux.SEMDMS.BSL
{
    /// <summary>
    /// GDS × KLARF 연관 분석(읽기 전용) 서비스다. KLARF Wafer 1장의 Defect가 GDS Chain Member 영역에 걸리는지 계산해
    /// Die / (Die, Chain) / Defect 단위로 집계한다. 결과는 저장하지 않는다(설계 결정 A7).
    /// 설계: 문서/2026-10-12_GDS_KLARF_연관분석_화면_설계.md 6 / 7절
    ///
    /// 처리 흐름 (AnalyzeWafer)
    ///  1) KLARF Step 정보 / Die Pitch (TQD_INSP_INFO.SELECT_DMS_WAFER_INFO, 기존 쿼리)
    ///  2) GDS Map 헤더 READY 확인 / 저장 기준 면 (TQP_GDS_MAP.SELECT_MAP_INFO, 기존 쿼리)
    ///  3) Chain 목록 (TQP_GDS_CHAIN.SELECT_CHAIN_LIST, 기존) -> 저장 완료(READY) + 화면 선택 Chain만
    ///  4) Member + Bounds (TQP_GDS_CHAIN_MEMBER.SELECT_CHAIN_MEMBER_BOUNDS_ALL, 신규 1개)
    ///  5) 검사 Die (TQD_SETUP_MAP.SELECT_X_Y, 기존) / Defect (TQD_DEFECT.GET_DEFECT, 기존)
    ///  6) GdsKlarfMatchEngine으로 판정 -> 결과 테이블 6개
    ///
    /// 결과 테이블
    ///  ANL_INFO   1행: 분석 기준 / 건수 / 시간 (화면 상태줄 / Excel 머리말)
    ///  CHAIN      Chain: CHAIN_SEQ / CHAIN_CODE / CHAIN_NAME / COLOR_ARGB / MEMBER_COUNT / LAYER_IDS / HIT_DIE_COUNT / HIT_DEFECT_COUNT / UPDATE_TIME
    ///  DIE        Die: XINDEX / YINDEX / IN_PLAN(Y/N) / DEFECT_COUNT / IN_CHAIN_COUNT / OUT_CHAIN_COUNT / HIT_CHAIN_COUNT / TOP_CHAINS
    ///  DIE_CHAIN  (Die, Chain): XINDEX / YINDEX / CHAIN_SEQ / CHAIN_CODE / DEFECT_COUNT / DEFECT_IDS  ← 판정 단위, E-Test 확장 키
    ///  DEFECT     Defect: DEFECTID / XINDEX / YINDEX / CLASSNUMBER / XSIZE / YSIZE / XREL / YREL / GDS_X / GDS_Y(보기 기준) / IN_CHAIN / CHAIN_CODES / HIT_MEMBER_COUNT
    ///  DEFECT_HIT (Defect, Member): DEFECTID / CHAIN_SEQ / CHAIN_CODE / PLACED_ELEMENT_ID / LAYER_ID / ROLE_CODE / MIN_X / MIN_Y / MAX_X / MAX_Y(저장 기준)
    ///
    /// IN_CHAIN_COUNT는 Chain이 1개 이상 걸린 Defect 수다. 한 Defect가 Chain 2개에 걸려도 1로 센다(설계 결정 A1).
    /// </summary>
    public class GdsKlarfAnalysis : Miracom.Middleware.BaseComponent, DACrux.SEMDMS.Interface.iGdsKlarfAnalysis
    {
        private const string SideBack = "BACK";
        private const string SideFront = "FRONT";

        public GdsKlarfAnalysis()
        {
        }

        /// <summary>KLARF Wafer 1장을 GDS Chain 영역과 비교한다. 인자 설명은 iGdsKlarfAnalysis 참고.</summary>
        public DataSet AnalyzeWafer(string strFactory, string strDeviceID, long lStepSeq, string strViewSide,
            double dToleranceUm, bool bUseDefectSize, double dOffsetXUm, double dOffsetYUm,
            string[] aChainSeqs, int[] aClassNumbers)
        {
            Stopwatch total = Stopwatch.StartNew();
            string factory = Required(strFactory, "Factory", 30);
            if (lStepSeq <= 0) throw new ArgumentException("STEP_SEQ는 1 이상이어야 합니다.", "lStepSeq");
            if (double.IsNaN(dToleranceUm) || dToleranceUm < 0 || dToleranceUm > 10000)
                throw new ArgumentException("허용오차는 0 ~ 10,000 µm로 입력하세요.", "dToleranceUm");
            if (double.IsNaN(dOffsetXUm) || double.IsNaN(dOffsetYUm) || Math.Abs(dOffsetXUm) > 100000 || Math.Abs(dOffsetYUm) > 100000)
                throw new ArgumentException("좌표 보정은 ±100,000 µm 안으로 입력하세요.");
            string viewSide = NormalizeSide(strViewSide);

            // 1) KLARF Step / Die Pitch
            Stopwatch query = Stopwatch.StartNew();
            DataTable stepInfo = new DACrux.SEMDMS.DSL.TQD_INSP_INFO().GetData("SELECT_DMS_WAFER_INFO", null,
                new string[] { lStepSeq.ToString(CultureInfo.InvariantCulture) });
            if (stepInfo == null || stepInfo.Rows.Count == 0)
                throw new ArgumentException("KLARF Wafer 정보가 없습니다. STEP_SEQ=" + lStepSeq, "lStepSeq");
            DataRow step = stepInfo.Rows[0];
            double pitchX = ToDouble(step["DIE_PITCH_X"]), pitchY = ToDouble(step["DIE_PITCH_Y"]);
            if (!(pitchX > 0) || !(pitchY > 0))
                throw new InvalidOperationException("KLARF Die Pitch가 없습니다(TQD_SETUP.DIE_PITCH_X / Y). STEP_SEQ=" + lStepSeq);
            string product = Text(step["PRODUCT"]);
            // Device 인자가 비면 Product 로 GDS 대표 Device 를 찾는다(2026-10-12: Sub Device 등록 -> 같은 이름 Map 순, 없으면 Product 그대로).
            string deviceId = String.IsNullOrEmpty(strDeviceID == null ? null : strDeviceID.Trim()) ? ResolveMapDevice(factory, product) : strDeviceID;
            deviceId = Required(deviceId, "GDS Device ID", 50);

            // 2) GDS Map
            DataTable mapInfo = new DACrux.SEMDMS.DSL.TQP_GDS_MAP().GetData("SELECT_MAP_INFO", null, new string[] { factory, deviceId });
            if (mapInfo == null || mapInfo.Rows.Count == 0 || Text(mapInfo.Rows[0]["MAP_STATUS"]) != "READY")
                throw new InvalidOperationException("저장 완료(READY)된 GDS Map이 없습니다: " + factory + " / " + deviceId);
            string mapSide = NormalizeSide(Text(mapInfo.Rows[0]["VIEW_SIDE"]));

            // 3) Chain 목록: READY만, 화면에서 고른 Chain만
            HashSet<long> chainFilter = ToChainFilter(aChainSeqs);
            DataTable chainList = new DACrux.SEMDMS.DSL.TQP_GDS_CHAIN().GetData("SELECT_CHAIN_LIST", null, new string[] { factory, deviceId });
            var chains = new Dictionary<long, DataRow>();
            int draftCount = 0;
            foreach (DataRow row in chainList.Rows)
            {
                long seq = ToLong(row["CHAIN_SEQ"]);
                if (chainFilter != null && !chainFilter.Contains(seq)) continue;
                if (Text(row["VERSION_STATUS"]) != "READY") { draftCount++; continue; }
                chains.Add(seq, row);
            }

            // 4) Member + Bounds (Device 전체를 한 번에 읽고 위 Chain만 남긴다)
            DataTable memberRows = new DACrux.SEMDMS.DSL.TQP_GDS_CHAIN_MEMBER().GetData("SELECT_CHAIN_MEMBER_BOUNDS_ALL", null,
                new string[] { factory, deviceId });
            var members = new List<GdsKlarfMemberBox>(memberRows.Rows.Count);
            foreach (DataRow row in memberRows.Rows)
            {
                long seq = ToLong(row["CHAIN_SEQ"]);
                if (!chains.ContainsKey(seq)) continue;
                members.Add(new GdsKlarfMemberBox
                {
                    ChainSeq = seq,
                    PlacedElementId = Text(row["PLACED_ELEMENT_ID"]),
                    LayerId = (int)ToLong(row["LAYER_ID"]),
                    RoleCode = Text(row["ROLE_CODE"]),
                    MinX = ToDouble(row["MIN_X"]),
                    MinY = ToDouble(row["MIN_Y"]),
                    MaxX = ToDouble(row["MAX_X"]),
                    MaxY = ToDouble(row["MAX_Y"])
                });
            }

            // 5) 검사 Die / Defect
            DataTable planRows = new DACrux.SEMDMS.DSL.TQD_SETUP_MAP().GetData("SELECT_X_Y", null,
                new string[] { Text(step["SETUP_SEQ"]), Text(step["TEST"]) });
            DataTable defectRows = new DACrux.SEMDMS.DSL.TQD_DEFECT().GetDefect(lStepSeq);
            query.Stop();

            HashSet<int> classFilter = aClassNumbers == null || aClassNumbers.Length == 0 ? null : new HashSet<int>(aClassNumbers);
            var defects = new List<GdsKlarfDefectPoint>(defectRows.Rows.Count);
            foreach (DataRow row in defectRows.Rows)
            {
                var defect = new GdsKlarfDefectPoint
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
                if (classFilter == null || classFilter.Contains(defect.ClassNumber))
                    defects.Add(defect);
            }
            defects.Sort((a, b) => a.DefectId.CompareTo(b.DefectId));

            // 6) 판정
            var options = new GdsKlarfMatchOptions
            {
                PitchX = pitchX,
                PitchY = pitchY,
                OffsetX = dOffsetXUm,
                OffsetY = dOffsetYUm,
                Tolerance = dToleranceUm,
                UseDefectSize = bUseDefectSize,
                Mirror = mapSide != viewSide
            };
            Stopwatch match = Stopwatch.StartNew();
            List<GdsKlarfDefectMatch> matches = GdsKlarfMatchEngine.Match(members, defects, options);
            match.Stop();

            DataSet result = BuildResult(chains, members, planRows, matches);
            total.Stop();

            DataTable info = NewTable("ANL_INFO",
                "FACTORY", typeof(string), "DEVICE_ID", typeof(string), "STEP_SEQ", typeof(long), "PRODUCT", typeof(string),
                "LOT_ID", typeof(string), "WAFER_ID", typeof(string), "STEP_ID", typeof(string), "SLOT_ID", typeof(string),
                "RESULTTIMESTAMP", typeof(string), "MAP_VIEW_SIDE", typeof(string), "VIEW_SIDE", typeof(string), "MIRROR", typeof(string),
                "TOLERANCE_UM", typeof(double), "USE_DEFECT_SIZE", typeof(string), "OFFSET_X", typeof(double), "OFFSET_Y", typeof(double),
                "PITCH_X", typeof(double), "PITCH_Y", typeof(double), "CHAIN_COUNT", typeof(int), "DRAFT_CHAIN_COUNT", typeof(int),
                "MEMBER_COUNT", typeof(int), "DEFECT_TOTAL", typeof(int), "DEFECT_COUNT", typeof(int), "IN_CHAIN_DEFECT_COUNT", typeof(int),
                "PLAN_DIE_COUNT", typeof(int), "DEFECT_DIE_COUNT", typeof(int), "CHAIN_FILTER", typeof(string), "CLASS_FILTER", typeof(string),
                "CHAIN_LAST_SAVED", typeof(string), "ANALYZED_AT", typeof(string), "QUERY_MS", typeof(long), "MATCH_MS", typeof(long),
                "ELAPSED_MS", typeof(long));
            info.Rows.Add(factory, deviceId, lStepSeq, product, Text(step["LOT_ID"]), Text(step["WAFER_ID"]), Text(step["STEP_ID"]),
                Text(step["SLOT_ID"]), Text(step["RESULTTIMESTAMP"]), mapSide, viewSide, options.Mirror ? "Y" : "N",
                dToleranceUm, bUseDefectSize ? "Y" : "N", dOffsetXUm, dOffsetYUm, pitchX, pitchY, chains.Count, draftCount,
                members.Count, defectRows.Rows.Count, defects.Count, matches.Count(m => m.MemberIndexes.Count > 0),
                planRows.Rows.Count, matches.Select(m => DieKey(m.Defect.XIndex, m.Defect.YIndex)).Distinct().Count(),
                chainFilter == null ? "ALL" : String.Join(",", chainFilter.OrderBy(s => s).Select(s => s.ToString(CultureInfo.InvariantCulture)).ToArray()),
                classFilter == null ? "ALL" : String.Join(",", classFilter.OrderBy(c => c).Select(c => c.ToString(CultureInfo.InvariantCulture)).ToArray()),
                chains.Count == 0 ? "" : chains.Values.Select(r => Text(r["UPDATE_TIME"])).Max(),
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                query.ElapsedMilliseconds, match.ElapsedMilliseconds, total.ElapsedMilliseconds);
            result.Tables.Add(info);
            return result;
        }

        /// <summary>판정 결과로 CHAIN / DIE / DIE_CHAIN / DEFECT / DEFECT_HIT 테이블을 만든다.</summary>
        private static DataSet BuildResult(Dictionary<long, DataRow> chains, List<GdsKlarfMemberBox> members,
            DataTable planRows, List<GdsKlarfDefectMatch> matches)
        {
            var ds = new DataSet("GDS_KLARF_ANALYSIS");
            var chainCode = chains.ToDictionary(p => p.Key, p => Text(p.Value["CHAIN_CODE"]));

            // DEFECT / DEFECT_HIT + (Die, Chain)별 Defect 목록
            DataTable defectTable = NewTable("DEFECT",
                "DEFECTID", typeof(long), "XINDEX", typeof(int), "YINDEX", typeof(int), "CLASSNUMBER", typeof(int),
                "XSIZE", typeof(double), "YSIZE", typeof(double), "XREL", typeof(double), "YREL", typeof(double),
                "GDS_X", typeof(double), "GDS_Y", typeof(double), "IN_CHAIN", typeof(string), "CHAIN_CODES", typeof(string),
                "HIT_MEMBER_COUNT", typeof(int));
            DataTable hitTable = NewTable("DEFECT_HIT",
                "DEFECTID", typeof(long), "CHAIN_SEQ", typeof(long), "CHAIN_CODE", typeof(string), "PLACED_ELEMENT_ID", typeof(string),
                "LAYER_ID", typeof(int), "ROLE_CODE", typeof(string), "MIN_X", typeof(double), "MIN_Y", typeof(double),
                "MAX_X", typeof(double), "MAX_Y", typeof(double));
            var dieChainDefects = new Dictionary<string, List<long>>();      // "x,y|chainSeq" -> DefectId 목록
            var chainDefects = new Dictionary<long, HashSet<long>>();
            var chainDies = new Dictionary<long, HashSet<string>>();
            foreach (GdsKlarfDefectMatch m in matches)
            {
                var hitChains = new SortedSet<long>(m.MemberIndexes.Select(i => members[i].ChainSeq));
                string die = DieKey(m.Defect.XIndex, m.Defect.YIndex);
                foreach (long seq in hitChains)
                {
                    string key = die + "|" + seq.ToString(CultureInfo.InvariantCulture);
                    List<long> list;
                    if (!dieChainDefects.TryGetValue(key, out list)) dieChainDefects.Add(key, list = new List<long>());
                    list.Add(m.Defect.DefectId);
                    AddTo(chainDefects, seq, m.Defect.DefectId);
                    AddTo(chainDies, seq, die);
                }
                defectTable.Rows.Add(m.Defect.DefectId, m.Defect.XIndex, m.Defect.YIndex, m.Defect.ClassNumber,
                    m.Defect.XSize, m.Defect.YSize, m.Defect.XRel, m.Defect.YRel, Round(m.ViewX), Round(m.ViewY),
                    hitChains.Count > 0 ? "Y" : "N", String.Join(",", OrderCodes(hitChains, chainCode)), m.MemberIndexes.Count);
                foreach (int i in m.MemberIndexes)
                {
                    GdsKlarfMemberBox b = members[i];
                    hitTable.Rows.Add(m.Defect.DefectId, b.ChainSeq, chainCode[b.ChainSeq], b.PlacedElementId, b.LayerId, b.RoleCode,
                        b.MinX, b.MinY, b.MaxX, b.MaxY);
                }
            }

            // CHAIN
            DataTable chainTable = NewTable("CHAIN",
                "CHAIN_SEQ", typeof(long), "CHAIN_CODE", typeof(string), "CHAIN_NAME", typeof(string), "COLOR_ARGB", typeof(int),
                "MEMBER_COUNT", typeof(int), "LAYER_IDS", typeof(string), "HIT_DIE_COUNT", typeof(int), "HIT_DEFECT_COUNT", typeof(int),
                "UPDATE_TIME", typeof(string));
            var memberCount = members.GroupBy(b => b.ChainSeq).ToDictionary(g => g.Key, g => g.Count());
            var layerIds = members.GroupBy(b => b.ChainSeq).ToDictionary(g => g.Key,
                g => String.Join(",", g.Select(b => b.LayerId).Distinct().OrderBy(l => l).Select(l => l.ToString(CultureInfo.InvariantCulture)).ToArray()));
            foreach (KeyValuePair<long, DataRow> p in chains)
            {
                int mc, colorArgb = 0;
                HashSet<string> hitDies;
                HashSet<long> defs;
                object color = p.Value["COLOR_ARGB"];
                if (color != null && color != DBNull.Value) colorArgb = (int)ToLong(color);
                chainTable.Rows.Add(p.Key, chainCode[p.Key], Text(p.Value["CHAIN_NAME"]), colorArgb,
                    memberCount.TryGetValue(p.Key, out mc) ? mc : 0, layerIds.ContainsKey(p.Key) ? layerIds[p.Key] : "",
                    chainDies.TryGetValue(p.Key, out hitDies) ? hitDies.Count : 0, chainDefects.TryGetValue(p.Key, out defs) ? defs.Count : 0,
                    Text(p.Value["UPDATE_TIME"]));
            }

            // DIE: 검사 Die 전체 + 검사 계획 밖인데 Defect가 있는 Die
            DataTable dieTable = NewTable("DIE",
                "XINDEX", typeof(int), "YINDEX", typeof(int), "IN_PLAN", typeof(string), "DEFECT_COUNT", typeof(int),
                "IN_CHAIN_COUNT", typeof(int), "OUT_CHAIN_COUNT", typeof(int), "HIT_CHAIN_COUNT", typeof(int), "TOP_CHAINS", typeof(string));
            var plan = new HashSet<string>();
            var dies = new List<int[]>();
            foreach (DataRow row in planRows.Rows)
            {
                int x = (int)ToLong(row["INDEX_X"]), y = (int)ToLong(row["INDEX_Y"]);
                if (plan.Add(DieKey(x, y))) dies.Add(new[] { x, y });
            }
            foreach (GdsKlarfDefectMatch m in matches)
                if (!plan.Contains(DieKey(m.Defect.XIndex, m.Defect.YIndex)) && !dies.Any(d => d[0] == m.Defect.XIndex && d[1] == m.Defect.YIndex))
                    dies.Add(new[] { m.Defect.XIndex, m.Defect.YIndex });
            var byDie = matches.GroupBy(m => DieKey(m.Defect.XIndex, m.Defect.YIndex)).ToDictionary(g => g.Key, g => g.ToList());
            DataTable dieChainTable = NewTable("DIE_CHAIN",
                "XINDEX", typeof(int), "YINDEX", typeof(int), "CHAIN_SEQ", typeof(long), "CHAIN_CODE", typeof(string),
                "DEFECT_COUNT", typeof(int), "DEFECT_IDS", typeof(string));
            var dieRows = new List<object[]>();
            foreach (int[] d in dies)
            {
                string die = DieKey(d[0], d[1]);
                List<GdsKlarfDefectMatch> list;
                if (!byDie.TryGetValue(die, out list)) list = new List<GdsKlarfDefectMatch>();
                int inChain = list.Count(m => m.MemberIndexes.Count > 0);
                var perChain = new List<KeyValuePair<long, List<long>>>();
                foreach (long seq in chains.Keys)
                {
                    List<long> ids;
                    if (dieChainDefects.TryGetValue(die + "|" + seq.ToString(CultureInfo.InvariantCulture), out ids))
                        perChain.Add(new KeyValuePair<long, List<long>>(seq, ids));
                }
                perChain = perChain.OrderByDescending(p => p.Value.Count).ThenBy(p => chainCode[p.Key].Length).ThenBy(p => chainCode[p.Key], StringComparer.Ordinal).ToList();
                foreach (KeyValuePair<long, List<long>> p in perChain)
                    dieChainTable.Rows.Add(d[0], d[1], p.Key, chainCode[p.Key], p.Value.Count,
                        String.Join(",", p.Value.Select(id => id.ToString(CultureInfo.InvariantCulture)).ToArray()));
                string top = String.Join(" ", perChain.Take(3).Select(p => chainCode[p.Key] + "(" + p.Value.Count.ToString(CultureInfo.InvariantCulture) + ")").ToArray());
                dieRows.Add(new object[] { d[0], d[1], plan.Contains(die) ? "Y" : "N", list.Count, inChain, list.Count - inChain, perChain.Count, top });
            }
            // 화면 Die 목록 기본 정렬: Chain 영역 Defect 수 내림차순 -> 전체 Defect 수 -> 위에서 아래 / 왼쪽에서 오른쪽
            foreach (object[] r in dieRows.OrderByDescending(r => (int)r[4]).ThenByDescending(r => (int)r[3]).ThenByDescending(r => (int)r[1]).ThenBy(r => (int)r[0]))
                dieTable.Rows.Add(r);

            ds.Tables.Add(chainTable);
            ds.Tables.Add(dieTable);
            ds.Tables.Add(dieChainTable);
            ds.Tables.Add(defectTable);
            ds.Tables.Add(hitTable);
            return ds;
        }

        #region [Helper]

        /// <summary>Chain Code를 화면 Chain 목록 순서(길이 -> 문자)로 정렬한다. A, B, ..., Z, AA.</summary>
        private static string[] OrderCodes(IEnumerable<long> seqs, Dictionary<long, string> chainCode)
        {
            return seqs.Select(s => chainCode[s]).OrderBy(c => c.Length).ThenBy(c => c, StringComparer.Ordinal).ToArray();
        }

        private static void AddTo<TKey, TValue>(Dictionary<TKey, HashSet<TValue>> map, TKey key, TValue value)
        {
            HashSet<TValue> set;
            if (!map.TryGetValue(key, out set)) map.Add(key, set = new HashSet<TValue>());
            set.Add(value);
        }

        private static string DieKey(int x, int y)
        {
            return x.ToString(CultureInfo.InvariantCulture) + "," + y.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>표시 좌표는 0.001 µm(1 nm, GDS DB 단위)까지만 남긴다.</summary>
        private static double Round(double value)
        {
            return Math.Round(value, 3);
        }

        /// <summary>열 이름 / 형식을 번갈아 받아 빈 테이블을 만든다.</summary>
        private static DataTable NewTable(string name, params object[] columns)
        {
            var table = new DataTable(name);
            for (int i = 0; i < columns.Length; i += 2)
                table.Columns.Add((string)columns[i], (Type)columns[i + 1]);
            return table;
        }

        /// <summary>화면 Chain 선택을 번호 집합으로 바꾼다. 없으면 null(= 전체).</summary>
        private static HashSet<long> ToChainFilter(string[] aChainSeqs)
        {
            if (aChainSeqs == null || aChainSeqs.Length == 0) return null;
            var set = new HashSet<long>();
            foreach (string s in aChainSeqs)
            {
                long seq;
                if (!long.TryParse(s == null ? null : s.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out seq) || seq <= 0)
                    throw new ArgumentException("Chain 번호는 1 이상의 정수여야 합니다: " + s, "aChainSeqs");
                set.Add(seq);
            }
            return set;
        }

        /// <summary>BACK 외의 값(빈 값 포함)은 FRONT. GdsMapControl의 GdsSideTransform.Normalize와 같은 규칙이다.</summary>
        private static string NormalizeSide(string side)
        {
            return String.Equals(side == null ? null : side.Trim(), SideBack, StringComparison.OrdinalIgnoreCase) ? SideBack : SideFront;
        }

        private static string Text(object value)
        {
            return value == null || value == DBNull.Value ? "" : Convert.ToString(value, CultureInfo.InvariantCulture).Trim();
        }

        private static double ToDouble(object value)
        {
            return value == null || value == DBNull.Value ? 0 : Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }

        private static long ToLong(object value)
        {
            return value == null || value == DBNull.Value ? 0 : Convert.ToInt64(value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// KLARF Product 이름으로 GDS 대표 Device 를 찾는다(2026-10-12, TQP_GDS_MAP_SUB_DEVICE.SELECT_MAP_DEVICE_BY_SUB).
        /// 없으면 Product 를 그대로 돌려준다(이후 READY Map 확인에서 오류 안내).
        /// </summary>
        private static string ResolveMapDevice(string factory, string product)
        {
            if (String.IsNullOrEmpty(product)) return product;
            DataTable found = new DACrux.SEMDMS.DSL.TQP_GDS_MAP_SUB_DEVICE().GetData("SELECT_MAP_DEVICE_BY_SUB", null,
                new string[] { factory, product, factory, product });
            return found.Rows.Count == 0 ? product : Text(found.Rows[0]["DEVICE_ID"]);
        }

        /// <summary>Oracle 컬럼 바이트 길이에 맞는 필수 문자열인지 확인한다(GdsChain과 같은 규칙).</summary>
        private static string Required(string strValue, string strName, int iMaxBytes)
        {
            string strResult = strValue == null ? string.Empty : strValue.Trim();
            int iLength = Encoding.UTF8.GetByteCount(strResult);
            if (iLength == 0 || iLength > iMaxBytes)
                throw new ArgumentException(strName + "은(는) 1~" + iMaxBytes + "바이트로 입력하세요.", strName);
            return strResult;
        }

        #endregion
    }
}
