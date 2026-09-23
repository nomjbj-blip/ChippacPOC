using System;
using System.Collections.Generic;

namespace NexplantQMS.GdsMap.Chain
{
    /// <summary>
    /// Chain 후보 추적에 사용하는 GDS 배치 Element 정보다.
    /// 원본 GDS Element가 SREF/AREF로 반복 배치될 수 있으므로 ElementKey는 화면에 배치된 항목마다 달라야 한다.
    /// </summary>
    public sealed class ChainTraceElement
    {
        public ChainTraceElement(string elementKey, int layerId, GBox bounds, string elementType)
        {
            if (string.IsNullOrWhiteSpace(elementKey))
                throw new ArgumentException("ElementKey가 필요합니다.", nameof(elementKey));
            if (bounds.IsEmpty)
                throw new ArgumentException("Element 영역이 비어 있습니다.", nameof(bounds));

            ElementKey = elementKey;
            LayerId = layerId;
            Bounds = bounds;
            ElementType = elementType ?? string.Empty;
        }

        public string ElementKey { get; private set; }
        public int LayerId { get; private set; }
        public GBox Bounds { get; private set; }
        public string ElementType { get; private set; }
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
    /// 사용자가 지정한 Input, Output, 작업 영역과 Device별 Layer 규칙을 후보 추적기에 전달한다.
    /// </summary>
    public sealed class ChainTraceRequest
    {
        public ChainTraceRequest(
            IEnumerable<ChainTraceElement> elements,
            string inputElementKey,
            string outputElementKey,
            IEnumerable<ChainLayerConnectionRule> layerRules,
            double spatialCellSize)
        {
            Elements = elements ?? throw new ArgumentNullException(nameof(elements));
            LayerRules = layerRules ?? throw new ArgumentNullException(nameof(layerRules));
            InputElementKey = inputElementKey;
            OutputElementKey = outputElementKey;
            SpatialCellSize = spatialCellSize;
        }

        public IEnumerable<ChainTraceElement> Elements { get; private set; }
        public string InputElementKey { get; private set; }
        public string OutputElementKey { get; private set; }
        public IEnumerable<ChainLayerConnectionRule> LayerRules { get; private set; }
        public double SpatialCellSize { get; private set; }
        public GBox? WorkArea { get; set; }
    }

    /// <summary>
    /// 자동 추적 결과다. Path는 확정된 전기 경로가 아니라 Layer 규칙과 영역 접촉으로 찾은 검토 대상 후보다.
    /// </summary>
    public sealed class ChainTraceResult
    {
        internal ChainTraceResult(
            bool isConnected,
            IList<ChainTraceElement> path,
            IList<ChainTraceElement> visitedElements,
            string message)
        {
            IsConnected = isConnected;
            Path = new List<ChainTraceElement>(path).AsReadOnly();
            VisitedElements = new List<ChainTraceElement>(visitedElements).AsReadOnly();
            Message = message ?? string.Empty;
        }

        public bool IsConnected { get; private set; }
        public IList<ChainTraceElement> Path { get; private set; }
        public IList<ChainTraceElement> VisitedElements { get; private set; }
        public string Message { get; private set; }
    }
}
