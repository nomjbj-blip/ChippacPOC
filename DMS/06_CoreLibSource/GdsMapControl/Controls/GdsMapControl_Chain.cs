using NexplantQMS.GdsMap.Chain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// 현재 GDS 화면의 배치 Element를 Chain 후보 추적 모델로 변환하는 기능을 제공한다.
    /// 화면 선택 기능과 자동 추적 로직을 분리하여 Layer 규칙과 추적 결과를 독립적으로 검증할 수 있게 한다.
    /// </summary>
    public partial class GdsMapControl
    {
        private Dictionary<string, GlSceneItem> _lastChainTraceItems =
            new Dictionary<string, GlSceneItem>(StringComparer.Ordinal);

        /// <summary>
        /// Chain 설정 화면에서 클릭한 화면 좌표를 GDS 월드 좌표로 변환한다.
        /// 화면 밖의 코드가 내부 카메라 Offset과 Zoom 계산을 중복 구현하지 않게 한다.
        /// </summary>
        public GPoint GetWorldPosition(Point screenPoint)
        {
            return ScreenToWorld(screenPoint);
        }

        /// <summary>목록에서 고른 묶음이 화면 중앙에 충분한 크기로 보이도록 이동하고 확대한다.</summary>
        public void FocusChainBounds(GBox bounds)
        {
            if (bounds.IsEmpty) return;
            double widthScale = ClientSize.Width / Math.Max(bounds.Width * 4, 0.000001);
            double heightScale = ClientSize.Height / Math.Max(bounds.Height * 4, 0.000001);
            _scale = Clamp(Math.Min(widthScale, heightScale), MinScale, MaxScale);
            SetTextLabelFitScale();
            _offset = new OpenTK.Vector2((float)(bounds.MinX + bounds.Width / 2),
                (float)(bounds.MinY + bounds.Height / 2));
            Invalidate();
            ZoomChanged?.Invoke(this, _scale);
        }

        /// <summary>
        /// 클릭 주변의 보이는 BOUNDARY/PATH 중 실제 형상이 클릭점에 닿는 후보를 반환한다.
        /// Bounds는 빠른 사전 검색에만 쓰고 최종 선택은 엔지니어가 한다.
        /// </summary>
        public IList<ChainElementCandidate> GetChainElementCandidates(Point screenPoint, int radiusPixels = 5)
        {
            if (Structure == null)
                throw new InvalidOperationException("GDS 도면을 먼저 조회해야 합니다.");
            if (radiusPixels < 0)
                throw new ArgumentOutOfRangeException(nameof(radiusPixels));

            GPoint first = ScreenToWorld(new Point(screenPoint.X - radiusPixels, screenPoint.Y - radiusPixels));
            GPoint second = ScreenToWorld(new Point(screenPoint.X + radiusPixels, screenPoint.Y + radiusPixels));
            var hitArea = new GBox(Math.Min(first.X, second.X), Math.Min(first.Y, second.Y),
                Math.Max(first.X, second.X), Math.Max(first.Y, second.Y));
            GPoint center = ScreenToWorld(screenPoint);
            double tolerance = Math.Max(hitArea.Width, hitArea.Height) / 2;
            var visibleLayers = new HashSet<int>(_layerList.Where(layer => layer.Visible).Select(layer => layer.LayerID));
            var candidates = new List<ChainElementCandidate>();
            foreach (GlSceneItem item in AllItems)
            {
                if (!(item.Source is GdsBoundary) && !(item.Source is GdsPath))
                    continue;

                if (visibleLayers.Contains(item.LayerID)
                    && item.WorldBounds.IntersectsWith(hitArea)
                    && ChainGeometryOverlap.ContainsPoint(center, item.WorldPoints,
                        item.Source is GdsPath, item.Width, tolerance))
                {
                    var points = item.WorldPoints;
                    candidates.Add(new ChainElementCandidate(
                        item.PlacedElementId,
                        item.LayerID,
                        item.Source.DataType,
                        item.Source.ElementName,
                        item.WorldBounds,
                        points == null ? 0 : points.Length,
                        item.Source is GdsPath ? item.Width : 0));
                }
            }

            return candidates.OrderBy(candidate => GetBoundsArea(candidate.Bounds))
                .ThenBy(candidate => candidate.LayerId).ToList().AsReadOnly();
        }

        /// <summary>
        /// 후보 목록에서 고른 Element와 이미 확정한 Input/Output Element를 기존 GPU 선택 색상으로 강조한다.
        /// 배치 ID로 현재 화면 도형을 찾아 사용한다.
        /// </summary>
        public void HighlightChainElements(IEnumerable<string> elementKeys)
        {
            ClearChainCandidateOverlay();
            var keys = new HashSet<string>(elementKeys ?? Enumerable.Empty<string>(), StringComparer.Ordinal);
            var changedItems = new HashSet<GlSceneItem>(_selectedItems);
            ClearSelectionInternal();
            if (keys.Count > 0)
                foreach (GlSceneItem item in AllItems)
                {
                    if (!(item.Source is GdsBoundary) && !(item.Source is GdsPath))
                        continue;
                    if (keys.Contains(item.PlacedElementId))
                    {
                        _lastChainTraceItems[item.PlacedElementId] = item;
                        item.Selected = true;
                        _selectedItems.Add(item);
                        changedItems.Add(item);
                    }
                }
            _selectedItems.Sort();
            UpdateSelectionVertices(changedItems);
            RaiseSelectionChanged();
            Invalidate();
        }

        /// <summary>
        /// 엔지니어가 확정한 여러 Input/Output Element와 선택 Layer를 기준으로 후보를 추적한다.
        /// 실제 도형의 접촉을 검사하지만 전기적 연결의 최종 확정은 엔지니어가 한다.
        /// </summary>
        /// <param name="inputElementKeys">엔지니어가 확정한 Input의 현재 화면 Element 키 집합</param>
        /// <param name="outputElementKeys">엔지니어가 확정한 Output의 현재 화면 Element 키 집합</param>
        /// <param name="selectedLayerIds">왼쪽 Layer 목록에서 체크한 Layer ID</param>
        /// <param name="layerRules">선택적으로 적용할 Layer 연결 규칙</param>
        /// <param name="applyLayerRules">규칙 추가 검사를 실행할지 여부</param>
        /// <param name="spatialCellSize">주변 Element 검색에 사용할 공간 격자 크기</param>
        public ChainTraceResult TraceChainCandidate(
            IEnumerable<string> inputElementKeys,
            IEnumerable<string> outputElementKeys,
            IEnumerable<int> selectedLayerIds,
            IEnumerable<ChainLayerConnectionRule> layerRules,
            bool applyLayerRules,
            double spatialCellSize)
        {
            if (Structure == null)
                throw new InvalidOperationException("GDS 도면을 먼저 조회해야 합니다.");
            if (selectedLayerIds == null)
                throw new ArgumentNullException(nameof(selectedLayerIds));
            if (layerRules == null)
                throw new ArgumentNullException(nameof(layerRules));
            if (inputElementKeys == null || outputElementKeys == null)
                throw new ArgumentNullException(inputElementKeys == null
                    ? nameof(inputElementKeys) : nameof(outputElementKeys));

            var inputKeys = new HashSet<string>(inputElementKeys, StringComparer.Ordinal);
            var outputKeys = new HashSet<string>(outputElementKeys, StringComparer.Ordinal);
            if (inputKeys.Count == 0)
                throw new ArgumentException("Input Element를 한 개 이상 확정해야 합니다.");
            if (inputKeys.Overlaps(outputKeys))
                throw new ArgumentException("같은 Element를 Input과 Output에 함께 지정할 수 없습니다.");

            var selectedLayers = new HashSet<int>(selectedLayerIds);
            if (selectedLayers.Count == 0)
                throw new ArgumentException("탐색할 Layer를 한 개 이상 선택해야 합니다.", nameof(selectedLayerIds));

            List<SceneTracePair> pairs = CreateSceneTracePairs(selectedLayers);
            // 같은 GDS의 다른 Chain으로 전환해도 이전 Chain의 배치 Element를 다시 찾을 수 있게 누적한다.
            foreach (SceneTracePair pair in pairs)
                _lastChainTraceItems[pair.TraceElement.ElementKey] = pair.SceneItem;
            var availableKeys = new HashSet<string>(
                pairs.Select(pair => pair.TraceElement.ElementKey), StringComparer.Ordinal);
            if (!inputKeys.IsSubsetOf(availableKeys))
                return CreateMissingSeedResult("선택한 Layer에 확정된 Input Element 일부가 없습니다.");
            if (!outputKeys.IsSubsetOf(availableKeys))
                return CreateMissingSeedResult("선택한 Layer에 확정된 Output Element 일부가 없습니다.");

            var request = new ChainTraceRequest(
                pairs.Select(pair => pair.TraceElement),
                inputKeys.OrderBy(key => key, StringComparer.Ordinal),
                outputKeys.OrderBy(key => key, StringComparer.Ordinal),
                layerRules,
                spatialCellSize)
            {
                SelectedLayerIds = selectedLayers,
                ApplyLayerRules = applyLayerRules
            };

            return new ChainCandidateTracer().Trace(request);
        }

        /// <summary>기존 단일 Input/Output 호출을 복수 키 탐색에 위임하여 이전 사용 코드를 유지한다.</summary>
        public ChainTraceResult TraceChainCandidate(
            string inputElementKey,
            string outputElementKey,
            IEnumerable<int> selectedLayerIds,
            IEnumerable<ChainLayerConnectionRule> layerRules,
            bool applyLayerRules,
            double spatialCellSize)
        {
            return TraceChainCandidate(new[] { inputElementKey }, new[] { outputElementKey },
                selectedLayerIds, layerRules, applyLayerRules, spatialCellSize);
        }

        /// <summary>
        /// 기본 경로, 모든 지정 단자와 직접 겹친 Element를 GPU 강조한다.
        /// 화면에 보이는 모든 항목은 엔지니어 검토 대상이며 확정 Chain은 아니다.
        /// </summary>
        public void ShowChainCandidate(ChainTraceResult result)
        {
            ShowChainCandidate(result, null);
        }

        /// <summary>
        /// 자동 추적 결과를 유지하면서 엔지니어가 편집한 최종 후보 키만 강조한다.
        /// null이면 기존 자동 결과를 그대로 표시한다.
        /// </summary>
        public void ShowChainCandidate(ChainTraceResult result, IEnumerable<string> includedElementKeys)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));

            var changedItems = new HashSet<GlSceneItem>(_selectedItems);
            ClearSelectionInternal();

            IEnumerable<ChainTraceElement> displayElements = result.IsConnected
                ? result.CandidateElements
                : result.VisitedElements.Concat(result.EndpointElements);
            var includedKeys = includedElementKeys == null
                ? new HashSet<string>(displayElements.Select(element => element.ElementKey), StringComparer.Ordinal)
                : new HashSet<string>(includedElementKeys, StringComparer.Ordinal);
            foreach (string elementKey in includedKeys)
            {
                if (!_lastChainTraceItems.TryGetValue(elementKey, out GlSceneItem item))
                    continue;
                item.Selected = true;
                _selectedItems.Add(item);
                changedItems.Add(item);
            }

            _selectedItems.Sort();
            UpdateSelectionVertices(changedItems);
            RaiseSelectionChanged();
            SetChainCandidateOverlay(result, includedKeys);
            Invalidate();
        }

        /// <summary>
        /// 화면에서 최종 포함한 Element만 추적 모델로 바꿔 Input/Output 단절 여부를 검사한다.
        /// 전체 GDS 대신 현재 후보만 검사하므로 수동 편집 직후에도 빠르게 결과를 돌려준다.
        /// </summary>
        public ChainConnectivityResult CheckChainSelectionConnectivity(
            IEnumerable<string> includedElementKeys,
            IEnumerable<string> inputElementKeys,
            IEnumerable<string> outputElementKeys,
            IEnumerable<int> selectedLayerIds,
            IEnumerable<ChainLayerConnectionRule> layerRules,
            bool applyLayerRules,
            double spatialCellSize)
        {
            var included = new HashSet<string>(includedElementKeys, StringComparer.Ordinal);
            var selectedLayers = new HashSet<int>(selectedLayerIds);
            var elements = _lastChainTraceItems
                .Where(pair => included.Contains(pair.Key) && selectedLayers.Contains(pair.Value.LayerID))
                .Select(pair => new ChainTraceElement(pair.Key, pair.Value.LayerID,
                    pair.Value.WorldBounds, pair.Value.Source.ElementName,
                    pair.Value.WorldPoints, pair.Value.Source is GdsPath ? pair.Value.Width : 0,
                    pair.Value.Source.DataType))
                .ToList();
            if (elements.Count != included.Count)
                throw new ArgumentException("현재 Layer에 없는 수동 후보가 있습니다.", nameof(includedElementKeys));
            var request = new ChainTraceRequest(elements, inputElementKeys,
                outputElementKeys, layerRules, spatialCellSize)
            {
                ApplyLayerRules = applyLayerRules
            };
            return new ChainCandidateTracer().CheckSelectionConnectivity(request);
        }

        /// <summary>
        /// 이미 탐색한 Chain의 Element 키를 현재 GDS 도형으로 되돌려 Visible Chain 외곽선에 제공한다.
        /// 다른 GDS를 열면 캐시가 초기화되므로 이전 도면과 섞이지 않는다.
        /// </summary>
        public IList<ChainTraceElement> GetChainTraceElements(IEnumerable<string> elementKeys)
        {
            var result = new List<ChainTraceElement>();
            foreach (string key in new HashSet<string>(elementKeys, StringComparer.Ordinal))
            {
                if (!_lastChainTraceItems.TryGetValue(key, out GlSceneItem item))
                    continue;
                result.Add(new ChainTraceElement(key, item.LayerID, item.WorldBounds,
                    item.Source.ElementName, item.WorldPoints,
                    item.Source is GdsPath ? item.Width : 0, item.Source.DataType));
            }
            return result;
        }

        /// <summary>Input 또는 Output이 바뀌면 이전 후보 경로 강조를 제거한다.</summary>
        private void ClearChainCandidate()
        {
            ClearChainCandidateOverlay();
            if (_selectedItems.Count == 0)
                return;

            GlSceneItem[] changedItems = _selectedItems.ToArray();
            ClearSelectionInternal();
            UpdateSelectionVertices(changedItems);
            RaiseSelectionChanged();
        }

        /// <summary>
        /// 현재 화면에 펼쳐진 Boundary와 Path 중 체크한 Layer의 항목만 추적용 Element로 변환한다.
        /// 도면 개정본 안의 배치 ID를 사용해 선택/탐색/DB 도형 참조 기준을 일치시킨다.
        /// </summary>
        private List<SceneTracePair> CreateSceneTracePairs(ISet<int> selectedLayerIds)
        {
            var result = new List<SceneTracePair>();
            foreach (GlSceneItem item in AllItems)
            {
                if (!(item.Source is GdsBoundary) && !(item.Source is GdsPath))
                    continue;
                string key = item.PlacedElementId;
                if (!selectedLayerIds.Contains(item.LayerID))
                    continue;
                var traceElement = new ChainTraceElement(
                    key,
                    item.LayerID,
                    item.WorldBounds,
                    item.Source.ElementName,
                    item.WorldPoints,
                    item.Source is GdsPath ? item.Width : 0,
                    item.Source.DataType);
                result.Add(new SceneTracePair(item, traceElement));
            }

            return result;
        }

        /// <summary>
        /// 클릭 후보 목록에서 작은 도형을 앞에 표시하기 위한 면적을 계산한다.
        /// 정렬 순서와 관계없이 사용자의 확정 동작 없이는 선택하지 않는다.
        /// </summary>
        private static double GetBoundsArea(GBox bounds)
        {
            return bounds.Width * bounds.Height;
        }

        /// <summary>Input 또는 Output 후보를 찾지 못했을 때 화면에 전달할 실패 결과를 만든다.</summary>
        private static ChainTraceResult CreateMissingSeedResult(string message)
        {
            return new ChainTraceResult(
                false,
                new List<ChainTraceElement>(),
                new List<ChainTraceElement>(),
                message);
        }

        /// <summary>화면 Element와 추적 모델의 실행 중 대응 관계를 보관한다.</summary>
        private sealed class SceneTracePair
        {
            public SceneTracePair(GlSceneItem sceneItem, ChainTraceElement traceElement)
            {
                SceneItem = sceneItem;
                TraceElement = traceElement;
            }

            public GlSceneItem SceneItem { get; private set; }
            public ChainTraceElement TraceElement { get; private set; }
        }
    }
}
