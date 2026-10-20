using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DACrux.SEMDMS.BSL
{
    /// <summary>
    /// Device 하나의 READY Chain Member 모양 + 균일 격자 색인. 만든 뒤에는 읽기만 하므로 여러 호출이 함께 써도 된다.
    /// 설계: 문서/설계/05_연관분석/2026-10-15_GDS_KLARF_ETest_통합분석_데이터_설계.md 4.2 / 4.5절
    /// </summary>
    public sealed class GdsKlarfShapeIndex
    {
        public readonly List<GdsKlarfShape> Shapes;
        public readonly double CellSize;
        /// <summary>좌표 점 수 합계(Cache 상한 판단용).</summary>
        public readonly long PointCount;
        /// <summary>판정에서 뺀 도형 수(TEXT).</summary>
        public readonly int SkippedCount;
        /// <summary>모양을 만드는 데 걸린 시간(ms, DB 읽기 제외).</summary>
        public long BuildMs;

        internal readonly Dictionary<long, int[]> Grid = new Dictionary<long, int[]>();
        /// <summary>칸을 너무 많이 차지하는 도형(긴 배선 / 큰 Pad). 모든 Defect가 검사한다.</summary>
        internal readonly int[] Large;

        /// <summary>격자 칸 수(Member 가 있는 칸만).</summary>
        public int CellCount
        {
            get { return Grid.Count; }
        }

        /// <summary>격자 대신 모든 Defect 가 검사하는 큰 도형 수.</summary>
        public int LargeCount
        {
            get { return Large.Length; }
        }

        public GdsKlarfShapeIndex(List<GdsKlarfShape> shapes, double cellSize, long pointCount, int skippedCount)
        {
            if (shapes == null) throw new ArgumentNullException("shapes");
            if (!(cellSize > 0)) throw new ArgumentException("격자 칸 크기는 0보다 커야 합니다.", "cellSize");
            Shapes = shapes;
            CellSize = cellSize;
            PointCount = pointCount;
            SkippedCount = skippedCount;
            var grid = new Dictionary<long, List<int>>();
            var large = new List<int>();
            for (int i = 0; i < shapes.Count; i++)
            {
                GdsKlarfShape s = shapes[i];
                long x1 = GdsKlarfMatchEngine.CellIndex(s.MinX, cellSize), x2 = GdsKlarfMatchEngine.CellIndex(s.MaxX, cellSize);
                long y1 = GdsKlarfMatchEngine.CellIndex(s.MinY, cellSize), y2 = GdsKlarfMatchEngine.CellIndex(s.MaxY, cellSize);
                if ((x2 - x1 + 1) * (y2 - y1 + 1) > GdsKlarfMatchEngine.MaxCellsPerBox)
                {
                    large.Add(i);
                    continue;
                }
                for (long cx = x1; cx <= x2; cx++)
                    for (long cy = y1; cy <= y2; cy++)
                    {
                        long key = GdsKlarfMatchEngine.CellKey(cx, cy);
                        List<int> list;
                        if (!grid.TryGetValue(key, out list))
                        {
                            list = new List<int>();
                            grid.Add(key, list);
                        }
                        list.Add(i);
                    }
            }
            foreach (KeyValuePair<long, List<int>> pair in grid) Grid.Add(pair.Key, pair.Value.ToArray());
            Large = large.ToArray();
        }

        /// <summary>
        /// SELECT_CHAIN_MEMBER_SHAPE_ALL 결과로 모양을 만든다. readyChains 에 있는 Chain 의 Member 만 쓴다(null 이면 전부).
        /// 좌표는 POINTS_BIN v1(점마다 X, Y double little-endian 16 B). 길이 검사는 GdsMapPlacedRowReader 와 같다.
        /// </summary>
        public static GdsKlarfShapeIndex Build(DataTable rows, HashSet<long> readyChains, double cellSize)
        {
            if (rows == null) throw new ArgumentNullException("rows");
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var shapes = new List<GdsKlarfShape>(rows.Rows.Count);
            long points = 0;
            int skipped = 0;
            foreach (DataRow row in rows.Rows)
            {
                long chainSeq = Convert.ToInt64(row["CHAIN_SEQ"], CultureInfo.InvariantCulture);
                if (readyChains != null && !readyChains.Contains(chainSeq)) continue;
                string placedId = Convert.ToString(row["PLACED_ELEMENT_ID"], CultureInfo.InvariantCulture);
                string type = Convert.ToString(row["ELEMENT_TYPE"], CultureInfo.InvariantCulture).Trim().ToUpperInvariant();
                if (type == "TEXT") { skipped++; continue; }
                int version = ToInt(row["POINTS_FORMAT_VERSION"]);
                if (version != 1)
                    throw new InvalidOperationException("지원하지 않는 좌표 형식 버전입니다: " + version + " / " + placedId);
                long count = Convert.ToInt64(row["POINT_COUNT"], CultureInfo.InvariantCulture);
                byte[] bin = row["POINTS_BIN"] as byte[];
                if (bin == null || count <= 0 || bin.LongLength != count * 16)
                    throw new InvalidOperationException("좌표 바이트 길이가 점 수와 다릅니다: " + placedId);
                var xs = new double[count];
                var ys = new double[count];
                for (int i = 0, offset = 0; i < count; i++, offset += 16)
                {
                    xs[i] = BitConverter.ToDouble(bin, offset);
                    ys[i] = BitConverter.ToDouble(bin, offset + 8);
                }
                double width = row["PATH_WIDTH"] == DBNull.Value ? 0 : Convert.ToDouble(row["PATH_WIDTH"], CultureInfo.InvariantCulture);
                int pathType = ToInt(row["PATH_TYPE"]);
                GdsKlarfShape shape = GdsKlarfShapeGeometry.Create(chainSeq, placedId, ToInt(row["LAYER_ID"]),
                    Convert.ToString(row["ROLE_CODE"], CultureInfo.InvariantCulture), type, width, pathType, xs, ys);
                shape.DataType = rows.Columns.Contains("DATA_TYPE") ? ToInt(row["DATA_TYPE"]) : 0;
                shapes.Add(shape);
                points += count;
            }
            var index = new GdsKlarfShapeIndex(shapes, cellSize, points, skipped);
            index.BuildMs = watch.ElapsedMilliseconds;
            return index;
        }

        private static int ToInt(object value)
        {
            return value == null || value == DBNull.Value ? 0 : Convert.ToInt32(value, CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// Device 별 Chain Member 모양 Cache (서비스 프로세스 static). SingleCall BSL 객체가 바뀌어도 유지된다.
    /// 설계: 문서/설계/05_연관분석/2026-10-15_GDS_KLARF_ETest_통합분석_데이터_설계.md 4.5절
    ///  - 키 = FACTORY|DEVICE_ID, 바뀜 확인 = MakeStamp(Map 저장 시각 / 작업 번호 / 저장 면 + READY Chain 별 저장 시각 / Member 수)
    ///  - 최근 3 Device(LRU). 점 수 합계가 MaxPoints 를 넘으면 Cache 에 두지 않고 매번 읽는다.
    ///  - 같은 키는 키별 lock 으로 한 번만 만든다. 다른 Device 는 서로 기다리지 않는다.
    /// </summary>
    public static class GdsKlarfGeometryCache
    {
        public const int MaxDevices = 3;
        public const long MaxPoints = 4000000;
        public const double DefaultCellSize = 200;

        private sealed class Entry
        {
            public string Key;
            public string Stamp;
            public GdsKlarfShapeIndex Index;
        }

        private static readonly object Sync = new object();
        /// <summary>앞 = 최근에 쓴 Device.</summary>
        private static readonly List<Entry> Entries = new List<Entry>();
        private static readonly Dictionary<string, object> BuildLocks = new Dictionary<string, object>(StringComparer.Ordinal);

        /// <summary>
        /// Cache 에 같은 Stamp 가 있으면 돌려주고, 없으면 load() 로 읽어 만든다.
        /// load 는 SELECT_CHAIN_MEMBER_SHAPE_ALL 결과를 돌려준다(DB 와 분리해 Probe 가 합성 표로 검사한다).
        /// </summary>
        public static GdsKlarfShapeIndex GetOrLoad(string factory, string deviceId, string stamp, HashSet<long> readyChains,
            Func<DataTable> load, out bool cacheHit, out long loadMs)
        {
            if (load == null) throw new ArgumentNullException("load");
            string key = (factory ?? "") + "|" + (deviceId ?? "");
            loadMs = 0;
            GdsKlarfShapeIndex found = Find(key, stamp);
            if (found != null)
            {
                cacheHit = true;
                return found;
            }
            object buildLock;
            lock (Sync)
            {
                if (!BuildLocks.TryGetValue(key, out buildLock))
                {
                    buildLock = new object();
                    BuildLocks.Add(key, buildLock);
                }
            }
            lock (buildLock)
            {
                found = Find(key, stamp);   // 기다리는 동안 다른 호출이 만들었으면 그것을 쓴다
                if (found != null)
                {
                    cacheHit = true;
                    return found;
                }
                var watch = System.Diagnostics.Stopwatch.StartNew();
                DataTable rows = load();
                loadMs = watch.ElapsedMilliseconds;
                GdsKlarfShapeIndex index = GdsKlarfShapeIndex.Build(rows, readyChains, DefaultCellSize);
                lock (Sync)
                {
                    Entries.RemoveAll(e => e.Key == key);
                    if (index.PointCount <= MaxPoints)
                    {
                        Entries.Insert(0, new Entry { Key = key, Stamp = stamp, Index = index });
                        while (Entries.Count > MaxDevices) Entries.RemoveAt(Entries.Count - 1);
                    }
                }
                cacheHit = false;
                return index;
            }
        }

        private static GdsKlarfShapeIndex Find(string key, string stamp)
        {
            lock (Sync)
            {
                for (int i = 0; i < Entries.Count; i++)
                {
                    Entry e = Entries[i];
                    if (e.Key != key) continue;
                    if (!String.Equals(e.Stamp, stamp, StringComparison.Ordinal)) return null;
                    if (i > 0)
                    {
                        Entries.RemoveAt(i);
                        Entries.Insert(0, e);
                    }
                    return e.Index;
                }
                return null;
            }
        }

        /// <summary>
        /// 바뀜 확인 문자열. 분석이 이미 읽는 SELECT_MAP_INFO 1행 + SELECT_CHAIN_LIST 의 READY Chain 으로 만든다(쿼리 추가 없음).
        /// Map 을 다시 올리면 CREATE_TIME / IMPORT_JOB_SEQ 가, Chain 을 다시 저장하면 UPDATE_TIME / MEMBER_COUNT 가 바뀐다.
        /// </summary>
        public static string MakeStamp(DataRow mapInfo, IEnumerable<DataRow> readyChains)
        {
            var sb = new StringBuilder();
            sb.Append(StampText(mapInfo["CREATE_TIME"])).Append('|').Append(StampText(mapInfo["IMPORT_JOB_SEQ"]))
                .Append('|').Append(StampText(mapInfo["VIEW_SIDE"]));
            foreach (DataRow c in readyChains.OrderBy(r => Convert.ToInt64(r["CHAIN_SEQ"], CultureInfo.InvariantCulture)))
                sb.Append('|').Append(StampText(c["CHAIN_SEQ"])).Append(':').Append(StampText(c["UPDATE_TIME"]))
                    .Append(':').Append(StampText(c["MEMBER_COUNT"]));
            return sb.ToString();
        }

        private static string StampText(object value)
        {
            if (value == null || value == DBNull.Value) return "";
            if (value is DateTime) return ((DateTime)value).ToString("yyyyMMddHHmmss.fff", CultureInfo.InvariantCulture);
            return Convert.ToString(value, CultureInfo.InvariantCulture).Trim();
        }

        /// <summary>같은 Stamp 의 Cache 가 이미 있는지(LRU 순서는 바꾸지 않는다). Ready 조회 표시용 (2026-10-16).</summary>
        public static bool Contains(string factory, string deviceId, string stamp)
        {
            string key = (factory ?? "") + "|" + (deviceId ?? "");
            lock (Sync) return Entries.Any(e => e.Key == key && String.Equals(e.Stamp, stamp, StringComparison.Ordinal));
        }

        /// <summary>지금 Cache 에 있는 키(앞 = 최근). Probe / 상태 확인용.</summary>
        public static string[] Keys()
        {
            lock (Sync) return Entries.Select(e => e.Key).ToArray();
        }

        public static void Clear()
        {
            lock (Sync) Entries.Clear();
        }
    }
}
