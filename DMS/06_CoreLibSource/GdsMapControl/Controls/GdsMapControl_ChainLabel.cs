using NexplantQMS.GdsMap.Chain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// Chain Input 근처 Label(GDS TEXT) 찾기 결과. Text가 null이면 범위 안에 Label이 없다.
    /// Hops: 1순위(연결)로 찾았으면 Input에서 몇 단계 연결된 도형 안의 Label인지(0 = Input 도형 안), 2순위(가까움)면 -1.
    /// </summary>
    public sealed class ChainEndpointLabelResult
    {
        public string Text { get; internal set; }
        public int Hops { get; internal set; } = -1;
        public double Distance { get; internal set; }
        /// <summary>점수가 같은 단계에서 함께 찾은 다른 Label 문자열(중복 제외). 상태 줄 안내용.</summary>
        public IList<string> OtherTexts { get; internal set; } = new List<string>();
        public int CandidateCount { get; internal set; }
        /// <summary>범위 안 도형이 상한을 넘어 연결 따라가기를 건너뛰었는지.</summary>
        public bool CandidateLimitHit { get; internal set; }
        public double ElapsedMs { get; internal set; }
        public bool Connected { get { return Text != null && Hops >= 0; } }
    }

    /// <summary>
    /// Chain Input 근처 Label로 Chain 이름을 정하기 위한 탐색(2026-10-08, 설계: 문서/설계/02_GDS/2026-10-08_Chain_Input_Label_자동이름_설계.md).
    /// 전체 도면을 탐색하지 않고 Input 주변 작은 창 안에서만 계산해 Input 확정 때 1회, 수 ms~수십 ms 안에 끝나게 한다.
    /// </summary>
    public partial class GdsMapControl
    {
        /// <summary>
        /// Input(여러 개 가능)마다 근처 Label을 찾아 가장 좋은 하나를 돌려준다. 도면이 없거나 Input 도형을 못 찾으면 null.
        /// 처리 흐름:
        ///  1) 창: Input 도형 Bounds를 (도형 가로 / 세로 중 큰 값 x windowFactor)만큼 넓힌 사각형
        ///  2) 후보 도형: 지금 보이는 Layer의 BOUNDARY / PATH 중 Bounds가 창과 겹치는 것(전체 1회 사각형 비교)
        ///  3) 연결 따라가기: Input 도형에서 실제로 겹치는 도형으로 maxHops 단계까지(BFS, 후보 안에서만)
        ///  4) 1순위: 창 안 Label 중 도달한 도형 안에 있는 것 -> (단계 수, Input 클릭점 거리) 순
        ///  5) 2순위: 1순위가 없으면 창 안에서 Input 클릭점과 가장 가까운 Label
        /// 후보가 maxCandidates를 넘으면 3)을 건너뛰고 2순위만 쓴다(큰 Plane 같은 Input에서 화면이 멈추지 않게).
        /// Label 좌표는 화면 도형과 같은 기준(Front / Back 반전 반영)이다.
        /// </summary>
        /// <param name="endpoints">Input 도형 키와 클릭 위치(월드 좌표)</param>
        public ChainEndpointLabelResult FindChainEndpointLabel(IEnumerable<KeyValuePair<string, GPoint>> endpoints,
            int maxHops = 3, double windowFactor = 3.0, int maxCandidates = 2000)
        {
            if (Structure == null || endpoints == null) return null;
            var picks = endpoints.Where(pair => !String.IsNullOrEmpty(pair.Key)).ToList();
            if (picks.Count == 0) return null;
            var watch = Stopwatch.StartNew();
            var keys = new HashSet<string>(picks.Select(pair => pair.Key), StringComparer.Ordinal);
            var visibleLayers = new HashSet<int>(_layerList.Where(layer => layer.Visible).Select(layer => layer.LayerKey));

            // 1) Input 도형 찾기 + 창 계산
            var seeds = new Dictionary<string, GlSceneItem>(StringComparer.Ordinal);
            foreach (GlSceneItem item in AllItems)
                if (keys.Contains(item.PlacedElementId) && !seeds.ContainsKey(item.PlacedElementId))
                    seeds.Add(item.PlacedElementId, item);
            if (seeds.Count == 0) return null;
            GBox window = GBox.Empty;
            foreach (GlSceneItem seed in seeds.Values)
            {
                GBox b = seed.WorldBounds;
                double margin = Math.Max(Math.Max(b.MaxX - b.MinX, b.MaxY - b.MinY), 1e-9) * windowFactor;
                window.Include(new GBox(b.MinX - margin, b.MinY - margin, b.MaxX + margin, b.MaxY + margin));
            }

            // 2) 창 안 후보 도형 (보이는 Layer의 BOUNDARY / PATH)
            var candidates = new List<GlSceneItem>();
            foreach (GlSceneItem item in AllItems)
            {
                if (!(item.Source is GdsBoundary) && !(item.Source is GdsPath)) continue;
                if (!visibleLayers.Contains(item.LayerKey) && !seeds.ContainsKey(item.PlacedElementId)) continue;
                if (item.WorldBounds.IntersectsWith(window)) candidates.Add(item);
            }
            var result = new ChainEndpointLabelResult { CandidateCount = candidates.Count };

            // 3) 연결 따라가기: 도형 -> 단계 수
            var hops = new Dictionary<GlSceneItem, int>();
            if (candidates.Count <= maxCandidates)
            {
                var traces = candidates.ToDictionary(item => item, ToTraceElement);
                var queue = new Queue<GlSceneItem>();
                foreach (GlSceneItem seed in seeds.Values)
                {
                    hops[seed] = 0;
                    queue.Enqueue(seed);
                    if (!traces.ContainsKey(seed)) traces[seed] = ToTraceElement(seed);
                }
                while (queue.Count > 0)
                {
                    GlSceneItem current = queue.Dequeue();
                    int level = hops[current];
                    if (level >= maxHops) continue;
                    ChainTraceElement currentTrace = traces[current];
                    foreach (GlSceneItem next in candidates)
                    {
                        if (hops.ContainsKey(next)) continue;
                        if (!ChainGeometryOverlap.Intersects(currentTrace, traces[next])) continue;
                        hops[next] = level + 1;
                        queue.Enqueue(next);
                    }
                }
            }
            else
                result.CandidateLimitHit = true;

            // 4) / 5) 창 안 Label 평가
            int bestHops = int.MaxValue;
            double bestDistance = double.MaxValue;
            string bestText = null;
            var sameLevelTexts = new List<string>();
            double nearestDistance = double.MaxValue;
            string nearestText = null;
            foreach (LayerTextLabel label in _textLabels)
            {
                if (String.IsNullOrWhiteSpace(label.Text)) continue;
                GPoint p = label.Position;
                if (p.X < window.MinX || p.X > window.MaxX || p.Y < window.MinY || p.Y > window.MaxY) continue;
                double distance = picks.Min(pick => Distance(pick.Value, p));
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestText = label.Text.Trim();
                }
                int level = int.MaxValue;
                foreach (KeyValuePair<GlSceneItem, int> pair in hops)
                {
                    if (pair.Value >= level) continue;
                    GlSceneItem item = pair.Key;
                    if (p.X < item.WorldBounds.MinX || p.X > item.WorldBounds.MaxX
                        || p.Y < item.WorldBounds.MinY || p.Y > item.WorldBounds.MaxY) continue;
                    if (ChainGeometryOverlap.ContainsPoint(p, item.WorldPoints, item.Source is GdsPath, item.Width, 0))
                        level = pair.Value;
                }
                if (level == int.MaxValue) continue;
                string text = label.Text.Trim();
                if (level < bestHops || level == bestHops && distance < bestDistance)
                {
                    if (level < bestHops) sameLevelTexts.Clear();
                    bestHops = level;
                    bestDistance = distance;
                    bestText = text;
                }
                if (level == bestHops && !sameLevelTexts.Contains(text, StringComparer.Ordinal))
                    sameLevelTexts.Add(text);
            }

            if (bestText != null)
            {
                result.Text = bestText;
                result.Hops = bestHops;
                result.Distance = bestDistance;
                result.OtherTexts = sameLevelTexts.Where(text => !String.Equals(text, bestText, StringComparison.Ordinal)).ToList();
            }
            else if (nearestText != null)
            {
                result.Text = nearestText;
                result.Hops = -1;
                result.Distance = nearestDistance;
            }
            result.ElapsedMs = watch.Elapsed.TotalMilliseconds;
            return result;
        }

        /// <summary>화면 도형을 Chain 겹침 판정용 요소로 바꾼다(CreateSceneTracePairs와 같은 값).</summary>
        private static ChainTraceElement ToTraceElement(GlSceneItem item)
        {
            return new ChainTraceElement(item.PlacedElementId, item.LayerID, item.WorldBounds, item.Source.ElementName,
                item.WorldPoints, item.Source is GdsPath ? item.Width : 0, item.Source.DataType);
        }

        private static double Distance(GPoint a, GPoint b)
        {
            double dx = a.X - b.X, dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
