using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace NexplantQMS.GdsMap.Chain
{
    /// <summary>유사 도형 묶음의 배치 Element 키와 검토가 필요한 이유를 검색 결과로 전달한다.</summary>
    public sealed class ChainSimilarGroup
    {
        public ChainSimilarGroup(IEnumerable<string> elementKeys, bool isProtected, bool isAmbiguous)
        {
            ElementKeys = elementKeys.OrderBy(key => key, StringComparer.Ordinal).ToArray();
            IsProtected = isProtected;
            IsAmbiguous = isAmbiguous;
        }

        public string[] ElementKeys { get; private set; }
        public bool IsProtected { get; private set; }
        public bool IsAmbiguous { get; internal set; }
    }

    /// <summary>
    /// 선택한 Chain의 Element만 대상으로 예시 묶음의 평행 이동 배치를 찾는다.
    /// Layer/DataType/형상/상대 위치를 모두 비교하고 Chain 소속은 변경하지 않는다.
    /// </summary>
    public sealed class ChainSimilarGroupFinder
    {
        /// <summary>
        /// 기준 Element를 다른 위치로 평행 이동했을 때 예시의 나머지 Element도 대응되는지 확인한다.
        /// X좌표 정렬 인덱스로 주변 객체만 조회하며, 중복 또는 겹친 결과는 검토 대상으로 표시한다.
        /// </summary>
        public IList<ChainSimilarGroup> Find(IEnumerable<ChainTraceElement> chainElements,
            IEnumerable<string> exampleKeys, IEnumerable<string> protectedKeys,
            double tolerance, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (chainElements == null) throw new ArgumentNullException(nameof(chainElements));
            if (exampleKeys == null) throw new ArgumentNullException(nameof(exampleKeys));
            if (tolerance <= 0 || double.IsNaN(tolerance))
                throw new ArgumentOutOfRangeException(nameof(tolerance));

            List<ChainTraceElement> elements = chainElements.ToList();
            var exampleSet = new HashSet<string>(exampleKeys, StringComparer.Ordinal);
            var protectedSet = new HashSet<string>(protectedKeys ?? Enumerable.Empty<string>(),
                StringComparer.Ordinal);
            if (exampleSet.Count == 0)
                throw new ArgumentException("예시 Element를 선택하세요.", nameof(exampleKeys));
            if (elements.Select(element => element.ElementKey).Distinct(StringComparer.Ordinal).Count()
                != elements.Count)
                throw new ArgumentException("Chain 안에 중복 Element Key가 있습니다.", nameof(chainElements));
            List<ChainTraceElement> example = elements
                .Where(element => exampleSet.Contains(element.ElementKey)).ToList();
            if (example.Count != exampleSet.Count || example.Any(element => element.WorldPoints == null))
                throw new ArgumentException("현재 Chain에 없는 예시 Element가 있습니다.", nameof(exampleKeys));
            if (exampleSet.Overlaps(protectedSet))
                throw new ArgumentException("Input/Output은 예시 묶음에 넣을 수 없습니다.", nameof(exampleKeys));

            var index = elements.GroupBy(Signature).ToDictionary(group => group.Key,
                group => group.OrderBy(element => element.Bounds.MinX).ToList());
            ChainTraceElement anchor = example.OrderBy(element => index[Signature(element)].Count)
                .ThenBy(element => element.ElementKey, StringComparer.Ordinal).First();
            var results = new List<ChainSimilarGroup>();
            var seen = new HashSet<string>(StringComparer.Ordinal);

            foreach (ChainTraceElement candidateAnchor in index[Signature(anchor)])
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (!SameShape(anchor, candidateAnchor, tolerance)) continue;
                double dx = candidateAnchor.Bounds.MinX - anchor.Bounds.MinX;
                double dy = candidateAnchor.Bounds.MinY - anchor.Bounds.MinY;
                var matched = new HashSet<string>(StringComparer.Ordinal);
                bool ambiguous = false;
                bool complete = true;
                foreach (ChainTraceElement template in example)
                {
                    List<ChainTraceElement> choices = index[Signature(template)];
                    double expectedX = template.Bounds.MinX + dx;
                    double expectedY = template.Bounds.MinY + dy;
                    int start = LowerBound(choices, expectedX - tolerance);
                    ChainTraceElement selected = null;
                    int matchCount = 0;
                    for (int i = start; i < choices.Count
                        && choices[i].Bounds.MinX <= expectedX + tolerance; i++)
                    {
                        ChainTraceElement choice = choices[i];
                        if (Math.Abs(choice.Bounds.MinY - expectedY) > tolerance
                            || !SameShape(template, choice, tolerance)) continue;
                        if (matched.Contains(choice.ElementKey)) continue;
                        selected = choice;
                        matchCount++;
                    }
                    if (matchCount == 0) { complete = false; break; }
                    if (matchCount > 1) ambiguous = true;
                    matched.Add(selected.ElementKey);
                }
                if (!complete || matched.Count != example.Count) continue;
                string identity = string.Join("|", matched.OrderBy(key => key, StringComparer.Ordinal));
                if (!seen.Add(identity)) continue;
                results.Add(new ChainSimilarGroup(matched, matched.Overlaps(protectedSet), ambiguous));
            }

            // 같은 Element가 둘 이상의 일치 묶음에 들어가면 어느 쪽을 제외할지 엔지니어가 검토한다.
            var owners = new Dictionary<string, ChainSimilarGroup>(StringComparer.Ordinal);
            foreach (ChainSimilarGroup group in results)
                foreach (string key in group.ElementKeys)
                {
                    if (owners.TryGetValue(key, out ChainSimilarGroup earlier))
                        earlier.IsAmbiguous = group.IsAmbiguous = true;
                    else
                        owners.Add(key, group);
                }
            return results.AsReadOnly();
        }

        /// <summary>객체 속성과 꼭짓점 개수로 비교 후보를 먼저 나눈다.</summary>
        private static Tuple<int, int, string, int> Signature(ChainTraceElement element)
        {
            bool closed = !string.Equals(element.ElementType, "PATH", StringComparison.OrdinalIgnoreCase);
            return Tuple.Create(element.LayerId, element.DataType,
                element.ElementType.ToUpperInvariant(),
                element.WorldPoints == null ? -1
                    : EffectivePointCount(element.WorldPoints, closed, 0.00001));
        }

        /// <summary>정렬된 X좌표에서 허용 구간의 첫 후보를 이진 검색한다.</summary>
        private static int LowerBound(IList<ChainTraceElement> elements, double minimumX)
        {
            int low = 0, high = elements.Count;
            while (low < high)
            {
                int middle = low + (high - low) / 2;
                if (elements[middle].Bounds.MinX < minimumX) low = middle + 1;
                else high = middle;
            }
            return low;
        }

        /// <summary>
        /// 도형 위치를 각 Bounding Box의 왼쪽 아래로 맞춰 윤곽을 비교한다.
        /// PATH는 역방향, BOUNDARY는 꼭짓점 시작 위치와 진행 방향 차이를 허용한다.
        /// </summary>
        private static bool SameShape(ChainTraceElement first, ChainTraceElement second, double tolerance)
        {
            if (first.LayerId != second.LayerId || first.DataType != second.DataType
                || !string.Equals(first.ElementType, second.ElementType, StringComparison.OrdinalIgnoreCase)
                || Math.Abs(first.Bounds.Width - second.Bounds.Width) > tolerance
                || Math.Abs(first.Bounds.Height - second.Bounds.Height) > tolerance
                || Math.Abs(first.PathWidth - second.PathWidth) > tolerance
                || first.WorldPoints == null || second.WorldPoints == null)
                return false;

            bool path = string.Equals(first.ElementType, "PATH", StringComparison.OrdinalIgnoreCase);
            int firstCount = EffectivePointCount(first.WorldPoints, !path, tolerance);
            int secondCount = EffectivePointCount(second.WorldPoints, !path, tolerance);
            if (firstCount != secondCount) return false;
            int startCount = path ? 1 : firstCount;
            for (int start = 0; start < startCount; start++)
                for (int direction = -1; direction <= 1; direction += 2)
                {
                    bool equal = true;
                    for (int i = 0; i < firstCount; i++)
                    {
                        int position = path
                            ? (direction == 1 ? i : firstCount - 1 - i)
                            : (start + direction * i + firstCount) % firstCount;
                        if (Math.Abs((first.WorldPoints[i].X - first.Bounds.MinX)
                                - (second.WorldPoints[position].X - second.Bounds.MinX)) > tolerance
                            || Math.Abs((first.WorldPoints[i].Y - first.Bounds.MinY)
                                - (second.WorldPoints[position].Y - second.Bounds.MinY)) > tolerance)
                        { equal = false; break; }
                    }
                    if (equal) return true;
                }
            return false;
        }

        /// <summary>GDS Boundary의 시작점을 끝에 반복한 좌표는 비교에서 한 번만 센다.</summary>
        private static int EffectivePointCount(GPoint[] points, bool closed, double tolerance)
        {
            int count = points.Length;
            if (closed && count > 1
                && Math.Abs(points[0].X - points[count - 1].X) <= tolerance
                && Math.Abs(points[0].Y - points[count - 1].Y) <= tolerance)
                count--;
            return count;
        }
    }
}
