using System;
using System.Collections.Generic;
using System.Linq;

namespace NexplantQMS.GdsMap.Chain
{
    /// <summary>
    /// Input Element에서 시작하여 Device별 Layer 연결 규칙에 맞는 Element를 탐색하고 Output까지의 후보 경로를 찾는다.
    /// 현재 단계는 Bounding Box 접촉을 사용하므로 결과를 자동 확정하지 않고 엔지니어 검토 대상으로 반환한다.
    /// </summary>
    public sealed class ChainCandidateTracer
    {
        /// <summary>
        /// 작업 영역 안의 Element를 공간 격자로 색인한 뒤 BFS로 Input에서 Output까지 가장 짧은 후보 경로를 찾는다.
        /// 공간 격자는 전체 Element를 매번 비교하지 않도록 주변 Element만 연결 후보로 조회하는 역할을 한다.
        /// </summary>
        public ChainTraceResult Trace(ChainTraceRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(request.InputElementKey))
                throw new ArgumentException("Input ElementKey가 필요합니다.", nameof(request));
            if (string.IsNullOrWhiteSpace(request.OutputElementKey))
                throw new ArgumentException("Output ElementKey가 필요합니다.", nameof(request));
            if (request.SpatialCellSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(request), "SpatialCellSize는 0보다 커야 합니다.");

            List<ChainTraceElement> elements = request.Elements
                .Where(element => !request.WorkArea.HasValue || request.WorkArea.Value.IntersectsWith(element.Bounds))
                .ToList();
            Dictionary<string, ChainTraceElement> elementsByKey = BuildElementDictionary(elements);

            if (!elementsByKey.TryGetValue(request.InputElementKey, out ChainTraceElement input))
                return Failed("작업 영역에서 Input Element를 찾을 수 없습니다.");
            if (!elementsByKey.TryGetValue(request.OutputElementKey, out ChainTraceElement output))
                return Failed("작업 영역에서 Output Element를 찾을 수 없습니다.");

            List<ChainLayerConnectionRule> rules = request.LayerRules.ToList();
            if (rules.Count == 0)
                return Failed("Layer 연결 규칙이 없습니다.");

            double maximumTolerance = rules.Max(rule => rule.Tolerance);
            var spatialIndex = new ChainSpatialIndex(elements, request.SpatialCellSize, maximumTolerance);
            var queue = new Queue<ChainTraceElement>();
            var visitedKeys = new HashSet<string>(StringComparer.Ordinal);
            var parents = new Dictionary<string, string>(StringComparer.Ordinal);
            var visitedElements = new List<ChainTraceElement>();

            queue.Enqueue(input);
            visitedKeys.Add(input.ElementKey);

            while (queue.Count > 0)
            {
                ChainTraceElement current = queue.Dequeue();
                visitedElements.Add(current);

                if (current.ElementKey == output.ElementKey)
                {
                    IList<ChainTraceElement> path = BuildPath(input, output, parents, elementsByKey);
                    return new ChainTraceResult(true, path, visitedElements, "Input에서 Output까지 후보 경로를 찾았습니다.");
                }

                foreach (ChainTraceElement candidate in spatialIndex.FindNearby(current.Bounds))
                {
                    if (candidate.ElementKey == current.ElementKey || visitedKeys.Contains(candidate.ElementKey))
                        continue;

                    ChainLayerConnectionRule rule = FindRule(rules, current.LayerId, candidate.LayerId);
                    if (rule == null || !AreBoundsConnected(current.Bounds, candidate.Bounds, rule.Tolerance))
                        continue;

                    visitedKeys.Add(candidate.ElementKey);
                    parents[candidate.ElementKey] = current.ElementKey;
                    queue.Enqueue(candidate);
                }
            }

            return new ChainTraceResult(
                false,
                new List<ChainTraceElement>(),
                visitedElements,
                "Input과 연결된 후보는 찾았지만 Output까지 도달하지 못했습니다.");
        }

        /// <summary>
        /// ElementKey는 저장과 화면 복원의 기준이므로 한 요청 안에서 중복을 허용하지 않는다.
        /// </summary>
        private static Dictionary<string, ChainTraceElement> BuildElementDictionary(IEnumerable<ChainTraceElement> elements)
        {
            var result = new Dictionary<string, ChainTraceElement>(StringComparer.Ordinal);
            foreach (ChainTraceElement element in elements)
            {
                if (result.ContainsKey(element.ElementKey))
                    throw new ArgumentException("중복된 ElementKey가 있습니다: " + element.ElementKey);
                result.Add(element.ElementKey, element);
            }
            return result;
        }

        /// <summary>
        /// 두 Layer에 적용할 연결 규칙을 찾는다. 규칙에 없는 Layer 조합은 연결하지 않는다.
        /// </summary>
        private static ChainLayerConnectionRule FindRule(
            IEnumerable<ChainLayerConnectionRule> rules,
            int firstLayerId,
            int secondLayerId)
        {
            return rules.FirstOrDefault(rule => rule.Matches(firstLayerId, secondLayerId));
        }

        /// <summary>
        /// 두 Bounding Box의 X/Y 간격이 모두 허용 오차 이내인지 확인한다.
        /// Polygon 정밀 접촉 판정이 추가되기 전까지 후보를 빠르게 좁히는 1차 조건으로 사용한다.
        /// </summary>
        private static bool AreBoundsConnected(GBox first, GBox second, double tolerance)
        {
            double gapX = AxisGap(first.MinX, first.MaxX, second.MinX, second.MaxX);
            double gapY = AxisGap(first.MinY, first.MaxY, second.MinY, second.MaxY);
            return gapX <= tolerance && gapY <= tolerance;
        }

        /// <summary>
        /// 한 축에서 두 구간이 떨어진 거리를 반환하며, 겹치거나 접촉하면 0을 반환한다.
        /// </summary>
        private static double AxisGap(double firstMin, double firstMax, double secondMin, double secondMax)
        {
            if (firstMax < secondMin)
                return secondMin - firstMax;
            if (secondMax < firstMin)
                return firstMin - secondMax;
            return 0;
        }

        /// <summary>
        /// BFS가 기록한 부모 Element를 Output부터 Input까지 역추적하여 화면 표시 순서의 경로로 만든다.
        /// </summary>
        private static IList<ChainTraceElement> BuildPath(
            ChainTraceElement input,
            ChainTraceElement output,
            IDictionary<string, string> parents,
            IDictionary<string, ChainTraceElement> elementsByKey)
        {
            var path = new List<ChainTraceElement>();
            string currentKey = output.ElementKey;

            while (true)
            {
                path.Add(elementsByKey[currentKey]);
                if (currentKey == input.ElementKey)
                    break;
                currentKey = parents[currentKey];
            }

            path.Reverse();
            return path;
        }

        /// <summary>
        /// 입력 검증 단계에서 경로 탐색을 시작할 수 없을 때 일관된 실패 결과를 만든다.
        /// </summary>
        private static ChainTraceResult Failed(string message)
        {
            return new ChainTraceResult(
                false,
                new List<ChainTraceElement>(),
                new List<ChainTraceElement>(),
                message);
        }

        /// <summary>
        /// 대량 GDS Element에서 현재 Element 주변 후보만 조회하기 위한 균일 공간 격자다.
        /// Element가 여러 Cell에 걸치면 각 Cell에 등록하고, 조회 결과는 ElementKey로 중복을 제거한다.
        /// </summary>
        private sealed class ChainSpatialIndex
        {
            private readonly Dictionary<CellKey, List<ChainTraceElement>> _cells =
                new Dictionary<CellKey, List<ChainTraceElement>>();
            private readonly double _cellSize;
            private readonly double _maximumTolerance;

            public ChainSpatialIndex(
                IEnumerable<ChainTraceElement> elements,
                double cellSize,
                double maximumTolerance)
            {
                _cellSize = cellSize;
                _maximumTolerance = maximumTolerance;

                foreach (ChainTraceElement element in elements)
                    Add(element);
            }

            /// <summary>
            /// Element의 영역이 걸치는 모든 Cell에 등록하여 길이가 긴 배선도 교차 위치에서 조회되도록 한다.
            /// </summary>
            private void Add(ChainTraceElement element)
            {
                VisitCells(element.Bounds, cellKey =>
                {
                    if (!_cells.TryGetValue(cellKey, out List<ChainTraceElement> items))
                    {
                        items = new List<ChainTraceElement>();
                        _cells.Add(cellKey, items);
                    }
                    items.Add(element);
                });
            }

            /// <summary>
            /// 최대 허용 오차만큼 넓힌 영역과 같은 Cell에 있는 Element를 연결 후보로 반환한다.
            /// </summary>
            public IEnumerable<ChainTraceElement> FindNearby(GBox bounds)
            {
                var expanded = new GBox(
                    bounds.MinX - _maximumTolerance,
                    bounds.MinY - _maximumTolerance,
                    bounds.MaxX + _maximumTolerance,
                    bounds.MaxY + _maximumTolerance);
                var foundKeys = new HashSet<string>(StringComparer.Ordinal);
                var result = new List<ChainTraceElement>();

                VisitCells(expanded, cellKey =>
                {
                    if (!_cells.TryGetValue(cellKey, out List<ChainTraceElement> items))
                        return;

                    foreach (ChainTraceElement item in items)
                    {
                        if (foundKeys.Add(item.ElementKey))
                            result.Add(item);
                    }
                });

                return result;
            }

            /// <summary>
            /// 지정 영역이 차지하는 Cell 좌표를 계산하여 등록 또는 조회 작업을 실행한다.
            /// </summary>
            private void VisitCells(GBox bounds, Action<CellKey> action)
            {
                long minX = ToCell(bounds.MinX);
                long maxX = ToCell(bounds.MaxX);
                long minY = ToCell(bounds.MinY);
                long maxY = ToCell(bounds.MaxY);

                for (long x = minX; x <= maxX; x++)
                {
                    for (long y = minY; y <= maxY; y++)
                        action(new CellKey(x, y));
                }
            }

            private long ToCell(double coordinate)
            {
                return (long)Math.Floor(coordinate / _cellSize);
            }

            /// <summary>Dictionary에서 공간 Cell을 식별하기 위한 값 형식이다.</summary>
            private struct CellKey : IEquatable<CellKey>
            {
                private readonly long _x;
                private readonly long _y;

                public CellKey(long x, long y)
                {
                    _x = x;
                    _y = y;
                }

                public bool Equals(CellKey other)
                {
                    return _x == other._x && _y == other._y;
                }

                public override bool Equals(object obj)
                {
                    return obj is CellKey other && Equals(other);
                }

                public override int GetHashCode()
                {
                    unchecked
                    {
                        return (_x.GetHashCode() * 397) ^ _y.GetHashCode();
                    }
                }
            }
        }
    }
}
