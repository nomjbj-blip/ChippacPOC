using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace NexplantQMS.GdsMap.Chain
{
    /// <summary>덩어리에 붙일 Label 한 개(화면 좌표). Layer 키 = 번호 + TEXTTYPE.</summary>
    public sealed class ChainComponentLabel
    {
        public ChainComponentLabel(string text, int layerKey, GPoint point)
        {
            Text = text ?? string.Empty;
            LayerKey = layerKey;
            Point = point;
        }

        public string Text { get; private set; }
        public int LayerKey { get; private set; }
        public GPoint Point { get; private set; }
    }

    /// <summary>서로 닿는 도형 묶음(연결 덩어리) 하나와 그 안에 들어 있는 Label.</summary>
    public sealed class ChainComponent
    {
        internal ChainComponent(int index, IList<ChainTraceElement> elements)
        {
            Index = index;
            Elements = elements;
            Labels = new List<ChainComponentLabel>();
            GBox bounds = GBox.Empty;
            foreach (ChainTraceElement element in elements)
            {
                bounds.Include(element.Bounds.MinX, element.Bounds.MinY);
                bounds.Include(element.Bounds.MaxX, element.Bounds.MaxY);
            }
            Bounds = bounds;
        }

        public int Index { get; internal set; }
        public IList<ChainTraceElement> Elements { get; private set; }
        public GBox Bounds { get; private set; }
        public List<ChainComponentLabel> Labels { get; private set; }
    }

    /// <summary>
    /// 도형을 공간 격자에 넣고 서로 닿는 도형을 묶어 연결 덩어리를 만든다 (2026-10-20, GDS Map View U8).
    /// 닿음 판정은 [후보 경로 찾기]와 같다: 외곽 상자 접촉 -> ChainGeometryOverlap.Intersects (Layer 규칙 없음).
    ///  - ExtractFrom: 도형 하나에서 닿는 도형을 따라가 덩어리 하나(예시 지정).
    ///  - ExtractAll: 도면 전체 덩어리(Union-Find). 같은 격자 칸의 두 도형만 비교하고, 이미 같은 덩어리면 형상 검사를 건너뛴다.
    /// 설계: 문서/설계/02_GDS/2026-10-20_GDS_Map_View_U8_화면_설계.md 5절. 검증: Probes/GdsChainAutoExtractProbe.ps1
    /// </summary>
    public sealed class ChainComponentExtractor
    {
        /// <summary>격자 칸 한 변(µm). U8 묶음(약 150 x 116 µm)의 도형이 1~4칸에 걸치는 크기.</summary>
        public const double DefaultCellSize = 100;

        private readonly List<ChainTraceElement> _elements;
        private readonly Dictionary<string, int> _indexByKey;
        private readonly Dictionary<long, List<int>> _grid = new Dictionary<long, List<int>>();
        private readonly double _cellSize;

        public ChainComponentExtractor(IEnumerable<ChainTraceElement> elements, double cellSize = DefaultCellSize)
        {
            if (elements == null) throw new ArgumentNullException(nameof(elements));
            if (cellSize <= 0) throw new ArgumentOutOfRangeException(nameof(cellSize));
            _cellSize = cellSize;
            _elements = elements.Where(e => e.WorldPoints != null && e.WorldPoints.Length > 0 && !e.Bounds.IsEmpty).ToList();
            _indexByKey = new Dictionary<string, int>(StringComparer.Ordinal);
            for (int i = 0; i < _elements.Count; i++)
            {
                if (_indexByKey.ContainsKey(_elements[i].ElementKey))
                    throw new ArgumentException("중복된 ElementKey가 있습니다: " + _elements[i].ElementKey, nameof(elements));
                _indexByKey.Add(_elements[i].ElementKey, i);
                GBox b = _elements[i].Bounds;
                for (long cx = Cell(b.MinX); cx <= Cell(b.MaxX); cx++)
                    for (long cy = Cell(b.MinY); cy <= Cell(b.MaxY); cy++)
                    {
                        List<int> list;
                        long key = CellKey(cx, cy);
                        if (!_grid.TryGetValue(key, out list)) _grid[key] = list = new List<int>();
                        list.Add(i);
                    }
            }
        }

        public int ElementCount { get { return _elements.Count; } }

        private long Cell(double value) { return (long)Math.Floor(value / _cellSize); }
        private static long CellKey(long cx, long cy) { return (cx << 32) ^ (cy & 0xFFFFFFFFL); }

        private static bool BoundsTouch(GBox a, GBox b)
        {
            return a.MaxX >= b.MinX && b.MaxX >= a.MinX && a.MaxY >= b.MinY && b.MaxY >= a.MinY;
        }

        private bool Connected(int i, int j)
        {
            return BoundsTouch(_elements[i].Bounds, _elements[j].Bounds) && ChainGeometryOverlap.Intersects(_elements[i], _elements[j]);
        }

        private IEnumerable<int> Nearby(GBox bounds)
        {
            var seen = new HashSet<int>();
            for (long cx = Cell(bounds.MinX); cx <= Cell(bounds.MaxX); cx++)
                for (long cy = Cell(bounds.MinY); cy <= Cell(bounds.MaxY); cy++)
                {
                    List<int> list;
                    if (!_grid.TryGetValue(CellKey(cx, cy), out list)) continue;
                    foreach (int i in list)
                        if (seen.Add(i)) yield return i;
                }
        }

        /// <summary>도형 하나에서 닿는 도형을 따라가 덩어리 하나를 만든다. 도형이 없으면 null.</summary>
        public ChainComponent ExtractFrom(string elementKey)
        {
            int start;
            if (elementKey == null || !_indexByKey.TryGetValue(elementKey, out start)) return null;
            var visited = new HashSet<int> { start };
            var queue = new Queue<int>();
            queue.Enqueue(start);
            var members = new List<ChainTraceElement>();
            while (queue.Count > 0)
            {
                int current = queue.Dequeue();
                members.Add(_elements[current]);
                foreach (int next in Nearby(_elements[current].Bounds))
                {
                    if (visited.Contains(next) || !Connected(current, next)) continue;
                    visited.Add(next);
                    queue.Enqueue(next);
                }
            }
            return new ChainComponent(0, members);
        }

        /// <summary>
        /// 도면 전체 덩어리. 진행률(0~100)은 격자 칸 처리 비율이다. 취소하면 OperationCanceledException.
        /// 결과는 덩어리 외곽 아래 -> 위, 왼쪽 -> 오른쪽 순서다.
        /// </summary>
        public IList<ChainComponent> ExtractAll(IProgress<int> progress, CancellationToken token)
        {
            int n = _elements.Count;
            var parent = new int[n];
            for (int i = 0; i < n; i++) parent[i] = i;
            Func<int, int> find = x =>
            {
                while (parent[x] != x) { parent[x] = parent[parent[x]]; x = parent[x]; }
                return x;
            };
            int cells = _grid.Count, done = 0, lastPercent = -1;
            foreach (List<int> list in _grid.Values)
            {
                for (int a = 0; a < list.Count; a++)
                    for (int c = a + 1; c < list.Count; c++)
                    {
                        int ra = find(list[a]), rc = find(list[c]);
                        if (ra == rc || !Connected(list[a], list[c])) continue;
                        parent[ra] = rc;
                    }
                done++;
                int percent = (int)(done * 100L / Math.Max(1, cells));
                if (percent != lastPercent)
                {
                    token.ThrowIfCancellationRequested();
                    if (progress != null) progress.Report(percent);
                    lastPercent = percent;
                }
            }
            var groups = new Dictionary<int, List<ChainTraceElement>>();
            for (int i = 0; i < n; i++)
            {
                int root = find(i);
                List<ChainTraceElement> members;
                if (!groups.TryGetValue(root, out members)) groups[root] = members = new List<ChainTraceElement>();
                members.Add(_elements[i]);
            }
            var result = groups.Values.Select(members => new ChainComponent(0, members))
                .OrderBy(c => c.Bounds.MinY).ThenBy(c => c.Bounds.MinX).ToList();
            for (int i = 0; i < result.Count; i++) result[i].Index = i;
            return result;
        }

        /// <summary>
        /// Label 이 들어 있는 도형의 덩어리에 Label 을 붙인다(같은 덩어리 / 같은 Layer / 같은 글자는 하나만).
        /// components 는 이 추출기가 만든 덩어리여야 한다. 반환: 덩어리에 붙은 Label 수(중복 제외 전).
        /// </summary>
        public int AttachLabels(IList<ChainComponent> components, IEnumerable<ChainComponentLabel> labels)
        {
            var owner = new Dictionary<string, ChainComponent>(StringComparer.Ordinal);
            foreach (ChainComponent component in components)
                foreach (ChainTraceElement element in component.Elements)
                    owner[element.ElementKey] = component;
            int attached = 0;
            foreach (ChainComponentLabel label in labels)
            {
                ChainTraceElement element = FindElementAt(label.Point);
                ChainComponent component;
                if (element == null || !owner.TryGetValue(element.ElementKey, out component)) continue;
                attached++;
                if (!component.Labels.Any(l => l.LayerKey == label.LayerKey && l.Text == label.Text))
                    component.Labels.Add(label);
            }
            return attached;
        }

        /// <summary>점이 들어 있는 도형(BOUNDARY 내부 / PATH 폭 안). 없으면 null.</summary>
        public ChainTraceElement FindElementAt(GPoint point)
        {
            List<int> list;
            if (!_grid.TryGetValue(CellKey(Cell(point.X), Cell(point.Y)), out list)) return null;
            foreach (int i in list)
            {
                ChainTraceElement e = _elements[i];
                if (point.X < e.Bounds.MinX || point.X > e.Bounds.MaxX || point.Y < e.Bounds.MinY || point.Y > e.Bounds.MaxY) continue;
                bool path = string.Equals(e.ElementType, "PATH", StringComparison.OrdinalIgnoreCase);
                if (ChainGeometryOverlap.ContainsPoint(point, e.WorldPoints, path, e.PathWidth, 0)) return e;
            }
            return null;
        }
    }
}
