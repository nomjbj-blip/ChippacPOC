using System;
using System.Collections.Generic;
using System.Linq;

namespace NexplantQMS.GdsMap.Chain
{
    /// <summary>
    /// 엔지니어가 클릭 위치에서 직접 고를 수 있는 배치 Element의 화면 조회 정보다.
    /// ElementKey는 현재 GDS 조회 세션에서만 유효하며 DB 저장 식별자로 사용하지 않는다.
    /// </summary>
    public sealed class ChainElementCandidate
    {
        public ChainElementCandidate(string elementKey, int layerId, int dataType,
            string elementType, GBox bounds, int pointCount, double pathWidth)
        {
            ElementKey = elementKey;
            LayerId = layerId;
            DataType = dataType;
            ElementType = elementType;
            Bounds = bounds;
            PointCount = pointCount;
            PathWidth = pathWidth;
        }

        public string ElementKey { get; private set; }
        public int LayerId { get; private set; }
        public int DataType { get; private set; }
        public string ElementType { get; private set; }
        public GBox Bounds { get; private set; }
        public int PointCount { get; private set; }
        public double PathWidth { get; private set; }
    }

    /// <summary>
    /// Chain 후보 추적에 사용하는 GDS 배치 Element 정보다.
    /// 원본 GDS Element가 SREF/AREF로 반복 배치될 수 있으므로 ElementKey는 화면에 배치된 항목마다 달라야 한다.
    /// </summary>
    public sealed class ChainTraceElement
    {
        /// <summary>기존 Bounding Box 기반 Probe에서 사용하는 생성 방식도 유지한다.</summary>
        public ChainTraceElement(string elementKey, int layerId, GBox bounds, string elementType)
            : this(elementKey, layerId, bounds, elementType, null, 0)
        {
        }

        /// <summary>새 화면에서 추출한 실제 배치 좌표와 PATH 폭을 함께 보관한다.</summary>
        public ChainTraceElement(string elementKey, int layerId, GBox bounds, string elementType,
            GPoint[] worldPoints, double pathWidth)
            : this(elementKey, layerId, bounds, elementType, worldPoints, pathWidth, 0)
        {
        }

        /// <summary>반복 도형 비교에서 같은 Layer의 서로 다른 DataType을 구분한다.</summary>
        public ChainTraceElement(string elementKey, int layerId, GBox bounds, string elementType,
            GPoint[] worldPoints, double pathWidth, int dataType)
        {
            if (string.IsNullOrWhiteSpace(elementKey))
                throw new ArgumentException("ElementKey가 필요합니다.", nameof(elementKey));
            if (bounds.IsEmpty)
                throw new ArgumentException("Element 영역이 비어 있습니다.", nameof(bounds));

            ElementKey = elementKey;
            LayerId = layerId;
            DataType = dataType;
            Bounds = bounds;
            ElementType = elementType ?? string.Empty;
            WorldPoints = worldPoints == null ? null : (GPoint[])worldPoints.Clone();
            PathWidth = pathWidth;
        }

        public string ElementKey { get; private set; }
        public int LayerId { get; private set; }
        public int DataType { get; private set; }
        public GBox Bounds { get; private set; }
        public string ElementType { get; private set; }
        public GPoint[] WorldPoints { get; private set; }
        public double PathWidth { get; private set; }
    }

    /// <summary>
    /// 두 Layer 사이에서 Element 연결 후보를 판단할 수 있는 규칙이다.
    /// 같은 Layer 연결도 명시적으로 등록해야 하며, 규칙에 없는 Layer 조합은 연결하지 않는다.
    /// </summary>
    public sealed class ChainLayerConnectionRule
    {
        public ChainLayerConnectionRule(int firstLayerId, int secondLayerId, double tolerance)
        {
            if (tolerance < 0)
                throw new ArgumentOutOfRangeException(nameof(tolerance), "연결 허용 오차는 0 이상이어야 합니다.");

            FirstLayerId = firstLayerId;
            SecondLayerId = secondLayerId;
            Tolerance = tolerance;
        }

        public int FirstLayerId { get; private set; }
        public int SecondLayerId { get; private set; }
        public double Tolerance { get; private set; }

        /// <summary>
        /// Layer 연결은 Input/Output 방향과 별개이므로 두 Layer의 순서를 바꾸어도 같은 규칙으로 판단한다.
        /// </summary>
        public bool Matches(int firstLayerId, int secondLayerId)
        {
            return (FirstLayerId == firstLayerId && SecondLayerId == secondLayerId)
                || (FirstLayerId == secondLayerId && SecondLayerId == firstLayerId);
        }
    }

    /// <summary>
    /// 사용자가 확정한 Input과 선택적 Output, 왼쪽 Layer 필터와 Layer 규칙을 후보 추적기에 전달한다.
    /// WorkArea는 이전 Probe와의 호환을 위해 남겨 두며 새 화면에서는 지정하지 않는다.
    /// </summary>
    public sealed class ChainTraceRequest
    {
        public ChainTraceRequest(
            IEnumerable<ChainTraceElement> elements,
            string inputElementKey,
            string outputElementKey,
            IEnumerable<ChainLayerConnectionRule> layerRules,
            double spatialCellSize)
            : this(elements, new[] { inputElementKey }, new[] { outputElementKey },
                layerRules, spatialCellSize)
        {
        }

        /// <summary>지도에서 확정한 복수 Input/Output을 한 번의 탐색 요청으로 보관한다.</summary>
        public ChainTraceRequest(
            IEnumerable<ChainTraceElement> elements,
            IEnumerable<string> inputElementKeys,
            IEnumerable<string> outputElementKeys,
            IEnumerable<ChainLayerConnectionRule> layerRules,
            double spatialCellSize)
        {
            Elements = elements ?? throw new ArgumentNullException(nameof(elements));
            LayerRules = layerRules ?? throw new ArgumentNullException(nameof(layerRules));
            InputElementKeys = (inputElementKeys ?? throw new ArgumentNullException(nameof(inputElementKeys)))
                .ToList().AsReadOnly();
            OutputElementKeys = (outputElementKeys ?? throw new ArgumentNullException(nameof(outputElementKeys)))
                .ToList().AsReadOnly();
            SpatialCellSize = spatialCellSize;
        }

        public IEnumerable<ChainTraceElement> Elements { get; private set; }
        public IList<string> InputElementKeys { get; private set; }
        public IList<string> OutputElementKeys { get; private set; }
        public IEnumerable<ChainLayerConnectionRule> LayerRules { get; private set; }
        public double SpatialCellSize { get; private set; }
        public GBox? WorkArea { get; set; }
        public ISet<int> SelectedLayerIds { get; set; }
        public bool ApplyLayerRules { get; set; } = true;
    }

    /// <summary>
    /// 자동 추적 결과다. 기본 경로, 직접 겹침, 분기 후보는 모두 엔지니어 검토 대상이다.
    /// </summary>
    public sealed class ChainTraceResult
    {
        internal ChainTraceResult(
            bool isConnected,
            IList<ChainTraceElement> path,
            IList<ChainTraceElement> visitedElements,
            string message,
            int? inputLayerId = null,
            int? outputLayerId = null,
            IList<ChainTraceElement> overlappingElements = null,
            IList<ChainTraceElement> branchCandidates = null,
            IList<ChainTraceElement> endpointElements = null,
            bool isOpenEnded = false)
        {
            IsConnected = isConnected;
            IsOpenEnded = isOpenEnded;
            Path = new List<ChainTraceElement>(path).AsReadOnly();
            VisitedElements = new List<ChainTraceElement>(visitedElements).AsReadOnly();
            OverlappingElements = new List<ChainTraceElement>(
                overlappingElements ?? new List<ChainTraceElement>()).AsReadOnly();
            BranchCandidates = new List<ChainTraceElement>(
                branchCandidates ?? new List<ChainTraceElement>()).AsReadOnly();
            EndpointElements = new List<ChainTraceElement>(
                endpointElements ?? new List<ChainTraceElement>()).AsReadOnly();
            var pathKeys = new HashSet<string>(Path.Select(element => element.ElementKey),
                StringComparer.Ordinal);
            OffPathEndpointElements = EndpointElements
                .Where(element => !pathKeys.Contains(element.ElementKey)).ToList().AsReadOnly();
            var candidateKeys = new HashSet<string>(StringComparer.Ordinal);
            CandidateElements = (IsOpenEnded ? VisitedElements : Path)
                .Concat(EndpointElements).Concat(OverlappingElements)
                .Where(element => candidateKeys.Add(element.ElementKey)).ToList().AsReadOnly();
            Message = message ?? string.Empty;
            InputLayerId = inputLayerId;
            OutputLayerId = outputLayerId;
        }

        public bool IsConnected { get; private set; }
        /// <summary>Output을 지정하지 않고 Input에서 닿는 모든 Element를 탐색한 결과인지 구분한다.</summary>
        public bool IsOpenEnded { get; private set; }
        public IList<ChainTraceElement> Path { get; private set; }
        public IList<ChainTraceElement> VisitedElements { get; private set; }
        /// <summary>기본 경로 또는 지정 단자와 실제 형상이 직접 겹치는 선택 Layer Element다.</summary>
        public IList<ChainTraceElement> OverlappingElements { get; private set; }
        /// <summary>직접 겹침 중 경로 밖의 다음 Element로도 이어지는 검토 후보이며 겹침 목록의 부분집합이다.</summary>
        public IList<ChainTraceElement> BranchCandidates { get; private set; }
        /// <summary>엔지니어가 지정한 모든 Input/Output Element이며 기본 경로 채택 여부와 무관하게 후보에 포함한다.</summary>
        public IList<ChainTraceElement> EndpointElements { get; private set; }
        /// <summary>지정 단자 중 기본 경로에 사용되지 않아 연결 검토가 필요한 Element다.</summary>
        public IList<ChainTraceElement> OffPathEndpointElements { get; private set; }
        /// <summary>기본 경로, 모든 지정 단자, 직접 겹침을 합친 중복 없는 Chain 후보 집합이다.</summary>
        public IList<ChainTraceElement> CandidateElements { get; private set; }
        public string Message { get; private set; }
        public int? InputLayerId { get; private set; }
        public int? OutputLayerId { get; private set; }
    }

    /// <summary>
    /// 수동 편집 후 최종 포함 Element가 첫 Input에서 모두 이어지는지 검사한 결과다.
    /// 도형 접촉을 나타낼 뿐 전기적 연결의 최종 확정은 아니다.
    /// </summary>
    public sealed class ChainConnectivityResult
    {
        public ChainConnectivityResult(IEnumerable<string> disconnectedKeys,
            int reachedOutputCount, int totalOutputCount)
        {
            DisconnectedKeys = new List<string>(disconnectedKeys).AsReadOnly();
            ReachedOutputCount = reachedOutputCount;
            TotalOutputCount = totalOutputCount;
        }

        public IList<string> DisconnectedKeys { get; private set; }
        public int ReachedOutputCount { get; private set; }
        public int TotalOutputCount { get; private set; }
        public bool IsConnected { get { return DisconnectedKeys.Count == 0 && ReachedOutputCount == TotalOutputCount; } }
    }
}
