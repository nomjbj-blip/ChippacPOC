using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using DACrux.SEMDMS.Interface;

namespace DACrux.SEMDMS.BSL
{
    /// <summary>
    /// 통합 연관 분석의 E-Test 연결 (2026-10-16).
    /// 설계: 문서/설계/05_연관분석/2026-10-15_GDS_KLARF_ETest_통합분석_데이터_설계.md 6.4 / 6.5 / 16.3 / 16.5절
    ///  - Chain 연결(결정 B10): E-Test 등록 Parameter 의 GDS_CHAIN_NAME 을 UPPER(TRIM) 으로 분석 GDS Device 의 READY Chain 이름과 맞춘다.
    ///    E-Test Device(예: OMM) 와 분석 Device(예: OMM_Hybrid, Map 복사본)는 Chain 이름이 같고 CHAIN_SEQ 만 다르기 때문이다.
    ///    이름이 GDS 에 없으면 NOT_IN_GDS, READY Chain 2개 이상이 같은 이름이면 DUPLICATE(연결하지 않음).
    ///  - Map 형상 비교: KLARF Product Map 과 E-Test Map(MAP_ID) 의 사용 Die 범위 / 수가 같은지.
    /// </summary>
    public partial class GdsKlarfAnalysis
    {
        public const string LinkLinked = "LINKED";
        public const string LinkNotInGds = "NOT_IN_GDS";
        public const string LinkDuplicate = "DUPLICATE";

        private sealed class EtNameLink
        {
            public string Key;
            public string Name;
            public string Status;
            public long ChainSeq;
            public string ChainCode;
            public int ParaCount;
            public int Chain2ParaCount;
        }

        /// <summary>
        /// E-Test 파일 Device / 공정의 등록 Parameter(사용 N 제외, GDS_CHAIN_NAME 있는 것)를 READY Chain 이름과 맞춘다.
        /// paraList = SELECT_ET_PARA_LIST 결과(뒤에서 값 / Spec 을 붙일 때 다시 쓴다).
        /// </summary>
        private static List<EtNameLink> LinkChainNames(string etFactory, string etDevice, string testProgram, IEnumerable<DataRow> readyChains,
            out DataTable paraList)
        {
            paraList = new DACrux.SEMDMS.DSL.TQP_GDS_ET_ITEM().GetData("SELECT_ET_PARA_LIST", null, new string[] { etFactory, etDevice, testProgram });
            Dictionary<string, List<DataRow>> byName = readyChains.GroupBy(r => NameKey(r["CHAIN_NAME"]))
                .Where(g => g.Key.Length > 0).ToDictionary(g => g.Key, g => g.ToList(), StringComparer.Ordinal);
            var links = new Dictionary<string, EtNameLink>(StringComparer.Ordinal);
            foreach (DataRow p in paraList.Rows)
            {
                if (Text(p["USE_FLAG"]) == "N") continue;
                string key = NameKey(p["GDS_CHAIN_NAME"]);
                if (key.Length == 0) continue;
                EtNameLink link;
                if (!links.TryGetValue(key, out link))
                {
                    link = new EtNameLink { Key = key, Name = Text(p["GDS_CHAIN_NAME"]), ChainCode = "" };
                    List<DataRow> chains;
                    if (!byName.TryGetValue(key, out chains)) link.Status = LinkNotInGds;
                    else if (chains.Count > 1)
                    {
                        link.Status = LinkDuplicate;
                        link.ChainCode = String.Join(",", chains.Select(c => Text(c["CHAIN_CODE"])).ToArray());
                    }
                    else
                    {
                        link.Status = LinkLinked;
                        link.ChainSeq = ToLong(chains[0]["CHAIN_SEQ"]);
                        link.ChainCode = Text(chains[0]["CHAIN_CODE"]);
                    }
                    links.Add(key, link);
                }
                link.ParaCount++;
                if (Text(p["CHAIN_NAME2"]).Length > 0) link.Chain2ParaCount++;
            }
            return links.Values.OrderBy(l => l.Status == LinkLinked ? 0 : 1).ThenBy(l => l.Key, StringComparer.Ordinal).ToList();
        }

        /// <summary>분석에 쓰는 E-Test 파일 1개의 판정 (시스템 Die 좌표 기준).</summary>
        private sealed class EtData
        {
            public DataRow File;
            public List<EtNameLink> Links;
            /// <summary>연결된 이름 키 -> 분석 Device Chain 번호</summary>
            public readonly Dictionary<string, long> KeyToChain = new Dictionary<string, long>(StringComparer.Ordinal);
            public readonly Dictionary<DiePoint, EtDie> Dies = new Dictionary<DiePoint, EtDie>();
            /// <summary>Map 좌표가 없는(MAP_X NULL = Map 밖) E-Test Die 수</summary>
            public int OutOfMap;
            public string MapShapeSame;
        }

        private sealed class EtDie
        {
            public long DutNo;
            public DiePoint Sys;
            /// <summary>Die 판정 P / F (빈 값 = 판정 없음)</summary>
            public string Judge;
            /// <summary>결과 파일의 DUT Pass / Fail Flag (P / F, 빈 값 = 없음). Spec 판정과 별개로 화면이 고를 수 있게 둔다 (2026-10-19).</summary>
            public string FilePass;
            public readonly Dictionary<long, EtChainJudge> Chains = new Dictionary<long, EtChainJudge>();
        }

        /// <summary>(Die, Chain) E-Test 상태: F = Fail 1개 이상, P = Fail 없고 Pass 1개 이상, N = 판정 값 없음 (설계 6.2절).</summary>
        private sealed class EtChainJudge
        {
            public string Status;
            public int ParaCount, FailCount;
            public string FirstFailItem, FirstFailValue;
        }

        /// <summary>
        /// E-Test 파일 1개를 읽어 Chain 이름 연결 / Die 판정 / (Die, Chain) 판정을 만든다.
        /// Die 는 업로드 때 E-Test Map 으로 계산해 저장한 시스템 좌표(MAP_X / MAP_Y)를 쓰고, 없으면 Map 밖으로 센다(설계 6.4절).
        /// </summary>
        private static EtData LoadEt(long srcFileSeq, string klarfFactory, string product, IEnumerable<DataRow> readyChains)
        {
            string seq = srcFileSeq.ToString(CultureInfo.InvariantCulture);
            DataTable file = new DACrux.SEMDMS.DSL.TQP_GDS_SRC_FILE().GetData("SELECT_ET_FILE_FOR_GDS", null, new string[] { seq });
            if (file == null || file.Rows.Count == 0) throw new ArgumentException("E-Test 파일이 없습니다. SRC_FILE_SEQ=" + seq);
            if (Text(file.Rows[0]["LOAD_STATUS"]) != "READY") throw new InvalidOperationException("저장 완료(READY)된 E-Test 파일이 아닙니다. SRC_FILE_SEQ=" + seq);
            var et = new EtData { File = file.Rows[0] };
            DataTable paraList;
            et.Links = LinkChainNames(Text(et.File["FACTORY"]), Text(et.File["DEVICE_ID"]), Text(et.File["TEST_PROGRAM"]), readyChains, out paraList);
            foreach (EtNameLink l in et.Links)
                if (l.Status == LinkLinked) et.KeyToChain[l.Key] = l.ChainSeq;
            foreach (DataRow r in new DACrux.SEMDMS.DSL.TQP_GDS_ET_DIE().GetData("SELECT_ET_DIE", null, new string[] { seq }).Rows)
            {
                if (r["MAP_X"] == DBNull.Value || r["MAP_Y"] == DBNull.Value) { et.OutOfMap++; continue; }
                var sys = new DiePoint((int)ToLong(r["MAP_X"]), (int)ToLong(r["MAP_Y"]));
                if (!et.Dies.ContainsKey(sys)) et.Dies.Add(sys, new EtDie { DutNo = ToLong(r["DUT_NO"]), Sys = sys, Judge = Text(r["JUDGE_FLAG"]), FilePass = Text(r["PASS_FLAG"]) });
            }
            foreach (DataRow r in new DACrux.SEMDMS.DSL.TQP_GDS_ET_VALUE().GetData("SELECT_ET_DIE_CHAIN_JUDGE", null, new string[] { seq }).Rows)
            {
                long chain;
                EtDie die;
                if (r["MAP_X"] == DBNull.Value || r["MAP_Y"] == DBNull.Value) continue;
                if (!et.KeyToChain.TryGetValue(Text(r["GDS_CHAIN_KEY"]), out chain)) continue;
                if (!et.Dies.TryGetValue(new DiePoint((int)ToLong(r["MAP_X"]), (int)ToLong(r["MAP_Y"])), out die)) continue;
                int fail = (int)ToLong(r["FAIL_COUNT"]), pass = (int)ToLong(r["PASS_COUNT"]);
                die.Chains[chain] = new EtChainJudge
                {
                    Status = fail > 0 ? "F" : pass > 0 ? "P" : "N",
                    ParaCount = (int)ToLong(r["PARA_COUNT"]),
                    FailCount = fail,
                    FirstFailItem = Text(r["FIRST_FAIL_ITEM"]),
                    FirstFailValue = ValueText(r["FIRST_FAIL_VALUE"], Text(r["FIRST_FAIL_STATUS"]))
                };
            }
            et.MapShapeSame = MapShapeSame(MapUsedDies(klarfFactory, product), MapUsedDies(Text(et.File["FACTORY"]), Text(et.File["MAP_ID"])));
            return et;
        }

        /// <summary>
        /// (Die, Chain) 4분류 (설계 7.1절). E-Test 없음 = 빈 값, KLARF 검사 안 한 Die 또는 E-Test 상태가 P / F 가 아니면 NA.
        /// BOTH = Defect + Fail, FAIL = Fail 만, DEFECT = Defect 만(Pass), OK = 둘 다 없음(Pass).
        /// </summary>
        private static string Class4(bool etLoaded, bool inspected, EtChainJudge judge, bool hit)
        {
            if (!etLoaded) return "";
            if (!inspected || judge == null || (judge.Status != "P" && judge.Status != "F")) return "NA";
            if (judge.Status == "F") return hit ? "BOTH" : "FAIL";
            return hit ? "DEFECT" : "OK";
        }

        /// <summary>
        /// 측정값 / Limit 문구: 값 상태 N('-' 측정)은 "-", 값이 없으면 빈 값, 숫자는 일반 표기(150, 0.012). DB 는 BINARY_DOUBLE 이라
        /// SQL TO_CHAR 를 쓰면 1.5E+002 처럼 나오므로 숫자로 받아 여기서 바꾼다.
        /// </summary>
        private static string ValueText(object value, string valueStatus)
        {
            if (valueStatus == "N") return "-";
            if (value == null || value == DBNull.Value) return "";
            return Convert.ToDouble(value, CultureInfo.InvariantCulture).ToString("G", CultureInfo.InvariantCulture);
        }

        /// <summary>Chain 이름 비교 키: 앞뒤 공백 제거 + 대문자.</summary>
        private static string NameKey(object value)
        {
            return Text(value).ToUpperInvariant();
        }

        /// <summary>미연결 이름 문구: "2 (DC_P9, BR_N3)". 없으면 빈 값.</summary>
        private static string UnlinkedText(List<EtNameLink> links)
        {
            List<EtNameLink> bad = links.Where(l => l.Status != LinkLinked).ToList();
            return bad.Count == 0 ? "" : bad.Count.ToString(CultureInfo.InvariantCulture) + " (" + String.Join(", ", bad.Select(l => l.Name).ToArray()) + ")";
        }

        /// <summary>Map 사용 Die (TQP_USEMAP, USECODE 1, 가장 큰 PROGRAM_REV). 없으면 빈 목록.</summary>
        private static List<DiePoint> MapUsedDies(string factory, string mapId)
        {
            var dies = new List<DiePoint>();
            if (String.IsNullOrEmpty(factory) || String.IsNullOrEmpty(mapId)) return dies;
            DataTable rows = new DACrux.SEMDMS.DSL.TQP_GDS_ET_DIE().GetData("SELECT_ET_MAP_DIE", null, new string[] { factory, mapId, factory, mapId });
            foreach (DataRow r in rows.Rows)
                if (ToLong(r["USECODE"]) == DieCoordinateMapper.UsedDieCode)
                    dies.Add(new DiePoint((int)ToLong(r["X"]), (int)ToLong(r["Y"])));
            return dies;
        }

        /// <summary>두 Map 의 사용 Die 범위 / 수가 같으면 "Y", 다르면 "N", 한쪽이라도 없으면 "" (비교 못 함).</summary>
        private static string MapShapeSame(List<DiePoint> a, List<DiePoint> b)
        {
            if (a.Count == 0 || b.Count == 0) return "";
            bool same = a.Count == b.Count && a.Min(p => p.X) == b.Min(p => p.X) && a.Max(p => p.X) == b.Max(p => p.X)
                && a.Min(p => p.Y) == b.Min(p => p.Y) && a.Max(p => p.Y) == b.Max(p => p.Y);
            return same ? "Y" : "N";
        }
    }
}
