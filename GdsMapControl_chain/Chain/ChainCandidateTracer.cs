using System;
using System.Collections.Generic;
using System.Linq;

namespace NexplantQMS.GdsMap.Chain
{
    /// <summary>
    /// 모든 Input Element에서 시작하여 선택 Layer의 실제 도형 접촉 관계를 따라 어느 Output까지 탐색한다.
    /// Layer 규칙은 요청에 따라 추가 적용하고 결과는 엔지니어 검토 대상으로 반환한다.
    /// </summary>
    public sealed class ChainCandidateTracer
    {
        /// <summary>
        /// 선택 Layer의 Element를 공간 격자로 색인한 뒤 다중 시작점 BFS로 가장 먼저 닿는 후보 경로를 찾는다.
        /// 공간 격자는 전체 Element를 매번 비교하지 않도록 주변 Element만 연결 후보로 조회하는 역할을 한다.
        /// </summary>
        public ChainTraceResult Trace(ChainTraceRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (request.InputElementKeys.Count == 0 || request.InputElementKeys.Any(string.IsNullOrWhiteSpace))
                throw new ArgumentException("Input ElementKey가 한 개 이상 필요합니다.", nameof(request));
            if (request.OutputElementKeys.Count == 0 || request.OutputElementKeys.Any(string.IsNullOrWhiteSpace))
                throw new ArgumentException("Output ElementKey가 한 개 이상 필요합니다.", nameof(request));
            if (request.InputElementKeys.Intersect(request.OutputElementKeys, StringComparer.Ordinal).Any())
                throw new ArgumentException("같은 Element를 Input과 Output에 함께 지정할 수 없습니다.", nameof(request));
            if (request.SpatialCellSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(request), "SpatialCellSize는 0보다 커야 합니다.");

            List<ChainTraceElement> elements = request.Elements
                .Where(element => request.SelectedLayerIds == null || request.SelectedLayerIds.Contains(element.LayerId))
                .Where(element => !request.WorkArea.HasValue || request.WorkArea.Value.IntersectsWith(element.Bounds))
                .ToList();
            Dictionary<string, ChainTraceElement> elementsByKey = BuildElementDictionary(elements);

            if (request.InputElementKeys.Any(key => !elementsByKey.ContainsKey(key)))
                return Failed("선택한 Layer에서 Input Element 일부를 찾을 수 없습니다.");
            if (request.OutputElementKeys.Any(key => !elementsByKey.ContainsKey(key)))
                return Failed("선택한 Layer에서 Output Element 일부를 찾을 수 없습니다.");

            var inputs = request.InputElementKeys.Distinct(StringComparer.Ordinal)
                .Select(key => elementsByKey[key]).ToList();
            var outputs = new HashSet<string>(request.OutputElementKeys, StringComparer.Ordinal);
            var endpoints = inputs.Concat(request.OutputElementKeys.Select(key => elementsByKey[key]))
                .GroupBy(element => element.ElementKey, StringComparer.Ordinal)
                .Select(group => group.First()).ToList();

            List<ChainLayerConnectionRule> rules = request.LayerRules.ToList();
            if (request.ApplyLayerRules && rules.Count == 0)
                return Failed("Layer 연결 규칙이 없습니다.", endpoints);

            // 실제 형상이 있으면 허용 오차와 무관하게 겹치는 도형만 연결하므로 격자를 넓혀 조회하지 않는다.
            double maximumTolerance = elements.All(element => element.WorldPoints != null)
                ? 0 : request.ApplyLayerRules ? rules.Max(rule => rule.Tolerance) : 0;
            var spatialIndex = new ChainSpatialIndex(elements, request.SpatialCellSize, maximumTolerance);
            var queue = new Queue<ChainTraceElement>();
            var visitedKeys = new HashSet<string>(StringComparer.Ordinal);
            var parents = new Dictionary<string, string>(StringComparer.Ordinal);
            var visitedElements = new List<ChainTraceElement>();

            foreach (ChainTraceElement input in inputs)
            {
                queue.Enqueue(input);
                visitedKeys.Add(input.ElementKey);
            }

            while (queue.Count > 0)
            {
                ChainTraceElement current = queue.Dequeue();
                visitedElements.Add(current);

                if (outputs.Contains(current.ElementKey))
                {
                    IList<ChainTraceElement> path = BuildPath(current, parents, elementsByKey);
                    List<ChainTraceElement> overlapping;
                    List<ChainTraceElement> branches;
                    CollectDirectOverlaps(path, endpoints, spatialIndex, rules, request.ApplyLayerRules,
                        out overlapping, out branches);
                    return new ChainTraceResult(
                        true,
                        path,
                        visitedElements,
                        "선택한 Input 중 하나에서 Output 중 하나까지 후보 경로를 찾았습니다.",
                        path[0].LayerId,
                        current.LayerId,
                        overlapping,
                        branches,
                        endpoints);
                }

                foreach (ChainTraceElement candidate in spatialIndex.FindNearby(current.Bounds))
                {
                    if (candidate.ElementKey == current.ElementKey || visitedKeys.Contains(candidate.ElementKey))
                        continue;

                    if (!AreElementsConnected(current, candidate, rules, request.ApplyLayerRules))
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
                "선택한 모든 Input에서 탐색했지만 어느 Output에도 도달하지 못했습니다.",
                inputs.Count == 1 ? (int?)inputs[0].LayerId : null,
                request.OutputElementKeys.Count == 1
                    ? (int?)elementsByKey[request.OutputElementKeys[0]].LayerId : null,
                null,
                null,
                endpoints);
        }

        /// <summary>
        /// 수동 추가/제외를 적용한 최종 후보만 공간 색인에 넣고 첫 Input에서 탐색을 끝까지 진행한다.
        /// 첫 Output에서 멈추는 후보 탐색과 달리 모든 지정 단자와 후보의 단절을 찾아낸다.
        /// </summary>
        public ChainConnectivityResult CheckSelectionConnectivity(ChainTraceRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (request.SpatialCellSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(request), "SpatialCellSize는 0보다 커야 합니다.");

            List<ChainTraceElement> elements = request.Elements
                .Where(element => request.SelectedLayerIds == null || request.SelectedLayerIds.Contains(element.LayerId))
                .ToList();
            Dictionary<string, ChainTraceElement> byKey = BuildElementDictionary(elements);
            if (request.InputElementKeys.Count == 0 || request.OutputElementKeys.Count == 0
                || request.InputElementKeys.Any(key => !byKey.ContainsKey(key))
                || request.OutputElementKeys.Any(key => !byKey.ContainsKey(key)))
                throw new ArgumentException("최종 후보에 지정한 Input/Output이 모두 있어야 합니다.", nameof(request));

            List<ChainLayerConnectionRule> rules = request.LayerRules.ToList();
            if (request.ApplyLayerRules && rules.Count == 0)
                throw new ArgumentException("적용할 Layer 규칙이 없습니다.", nameof(request));
            double maximumTolerance = elements.All(element => element.WorldPoints != null)
                ? 0 : request.ApplyLayerRules ? rules.Max(rule => rule.Tolerance) : 0;
            var index = new ChainSpatialIndex(elements, request.SpatialCellSize, maximumTolerance);
            var queue = new Queue<ChainTraceElement>();
            var reached = new HashSet<string>(StringComparer.Ordinal);
            ChainTraceElement start = byKey[request.InputElementKeys[0]];
            queue.Enqueue(start);
            reached.Add(start.ElementKey);

            while (queue.Count > 0)
            {
                ChainTraceElement current = queue.Dequeue();
                foreach (ChainTraceElement next in index.FindNearby(current.Bounds))
                {
                    if (reached.Contains(next.ElementKey)
                        || !AreElementsConnected(current, next, rules, request.ApplyLayerRules))
                        continue;
                    reached.Add(next.ElementKey);
                    queue.Enqueue(next);
                }
            }

            List<string> disconnected = elements.Select(element => element.ElementKey)
                .Where(key => !reached.Contains(key)).ToList();
            int reachedOutputs = request.OutputElementKeys.Count(key => reached.Contains(key));
            return new ChainConnectivityResult(disconnected, reachedOutputs, request.OutputElementKeys.Count);
        }

        /// <summary>
        /// 기본 경로와 모든 지정 단자에 한 단계로 직접 닿는 Element를 중복 없이 모은다.
        /// 그중 경로 밖으로 한 번 더 이어지는 항목만 분기 후보로 표시하며 자동 확장은 하지 않는다.
        /// </summary>
        private static void CollectDirectOverlaps(
            IEnumerable<ChainTraceElement> path,
            IEnumerable<ChainTraceElement> endpoints,
            ChainSpatialIndex spatialIndex,
            IList<ChainLayerConnectionRule> rules,
            bool applyLayerRules,
            out List<ChainTraceElement> overlapping,
            out List<ChainTraceElement> branches)
        {
            var anchors = path.Concat(endpoints)
                .GroupBy(element => element.ElementKey, StringComparer.Ordinal)
                .Select(group => group.First()).ToList();
            var anchorKeys = new HashSet<string>(anchors.Select(element => element.ElementKey),
                StringComparer.Ordinal);
            var overlapKeys = new HashSet<string>(StringComparer.Ordinal);
            overlapping = new List<ChainTraceElement>();

            foreach (ChainTraceElement anchor in anchors)
                foreach (ChainTraceElement candidate in spatialIndex.FindNearby(anchor.Bounds))
                {
                    if (anchorKeys.Contains(candidate.ElementKey)
                        || !overlapKeys.Add(candidate.ElementKey))
                        continue;
                    if (AreElementsConnected(anchor, candidate, rules, false))
                        overlapping.Add(candidate);
                    else
                        overlapKeys.Remove(candidate.ElementKey);
                }

            branches = new List<ChainTraceElement>();
            foreach (ChainTraceElement overlap in overlapping)
                foreach (ChainTraceElement next in spatialIndex.FindNearby(overlap.Bounds))
                {
                    if (next.ElementKey == overlap.ElementKey
                        || anchorKeys.Contains(next.ElementKey)
                        || !AreElementsConnected(overlap, next, rules, applyLayerRules))
                        continue;
                    branches.Add(overlap);
                    break;
                }
        }

        /// <summary>Layer 규칙이 켜진 경우 조합을 확인하고 Bounds와 실제 형상의 접촉을 검사한다.</summary>
        private static bool AreElementsConnected(ChainTraceElement first,
            ChainTraceElement second, IList<ChainLayerConnectionRule> rules, bool applyLayerRules)
        {
            ChainLayerConnectionRule rule = applyLayerRules
                ? FindRule(rules, first.LayerId, second.LayerId) : null;
            if (applyLayerRules && rule == null)
                return false;
            double tolerance = rule == null ? 0 : rule.Tolerance;
            if (!AreBoundsConnected(first.Bounds, second.Bounds, tolerance))
                return false;
            return first.WorldPoints == null || second.WorldPoints == null
                || ChainGeometryOverlap.Intersects(first, second);
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
        /// Bounding Box의 X/Y 간격으로 주변 후보를 먼저 걸러낸다.
        /// 실제 형상이 있으면 이후 ChainGeometryOverlap으로 접촉을 별도 확인한다.
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
        /// BFS가 기록한 부모를 Output부터 역추적하고 부모가 없는 시작 Input에서 멈춘다.
        /// </summary>
        private static IList<ChainTraceElement> BuildPath(
            ChainTraceElement output,
            IDictionary<string, string> parents,
            IDictionary<string, ChainTraceElement> elementsByKey)
        {
            var path = new List<ChainTraceElement>();
            string currentKey = output.ElementKey;

            while (true)
            {
                path.Add(elementsByKey[currentKey]);
                if (!parents.TryGetValue(currentKey, out string parentKey))
                    break;
                currentKey = parentKey;
            }

            path.Reverse();
            return path;
        }

        /// <summary>
        /// 입력 검증 단계에서 경로 탐색을 시작할 수 없을 때 일관된 실패 결과를 만든다.
        /// </summary>
        private static ChainTraceResult Failed(string message,
            IList<ChainTraceElement> endpointElements = null)
        {
            return new ChainTraceResult(
                false,
                new List<ChainTraceElement>(),
                new List<ChainTraceElement>(),
                message,
                endpointElements: endpointElements);
        }

        /// <summary>
        /// 대량 GDS Element에서 현재 Element 주변 후보만 조회하기 위한 균일 공간 격자다.
        /// Element가 여러 Cell에 걸치면 각 Cell에 등록하고, 조회 결과는 ElementKey로 중복을 제거한다.
        /// </summary>
        private sealed class ChainSpatialIndex
        {
            private const long MaximumCellsPerItem = 4096;
            private readonly Dictionary<CellKey, List<ChainTraceElement>> _cells =
                new Dictionary<CellKey, List<ChainTraceElement>>();
            private readonly List<ChainTraceElement> _largeItems = new List<ChainTraceElement>();
            private readonly List<ChainTraceElement> _allItems = new List<ChainTraceElement>();
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
                {
                    _allItems.Add(element);
                    Add(element);
                }
            }

            /// <summary>
            /// Element의 영역이 걸치는 모든 Cell에 등록하여 길이가 긴 배선도 교차 위치에서 조회되도록 한다.
            /// </summary>
            private void Add(ChainTraceElement element)
            {
                if (ExceedsCellLimit(element.Bounds))
                {
                    _largeItems.Add(element);
                    return;
                }
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

                // 넓은 현재 도형은 전체와 비교하고, 일반 도형은 격자와 넓은 도형만 검사한다.
                if (ExceedsCellLimit(expanded))
                    return _allItems;

                foreach (ChainTraceElement largeItem in _largeItems)
                    if (foundKeys.Add(largeItem.ElementKey))
                        result.Add(largeItem);

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

            /// <summary>큰 도형 하나가 수백만 Cell을 만드는 것을 막기 위해 등록 방식을 분기한다.</summary>
            private bool ExceedsCellLimit(GBox bounds)
            {
                double widthCells = Math.Floor(bounds.MaxX / _cellSize) - Math.Floor(bounds.MinX / _cellSize) + 1;
                double heightCells = Math.Floor(bounds.MaxY / _cellSize) - Math.Floor(bounds.MinY / _cellSize) + 1;
                return widthCells * heightCells > MaximumCellsPerItem;
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
