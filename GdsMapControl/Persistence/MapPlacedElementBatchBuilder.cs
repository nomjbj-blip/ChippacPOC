using System;
using System.Collections.Generic;
using System.Text;

namespace NexplantQMS.GdsMap.Persistence
{
    /// <summary>Service 전송 한 번에 담을 배치 도형과 예상 비압축 크기다.</summary>
    public sealed class MapPlacedElementBatch
    {
        public int BatchNo { get; private set; }
        public IList<MapPlacedElementData> Elements { get; private set; }
        public long PointCount { get; private set; }
        public long EstimatedBytes { get; private set; }

        internal MapPlacedElementBatch(int batchNo, List<MapPlacedElementData> elements,
            long pointCount, long estimatedBytes)
        {
            BatchNo = batchNo;
            Elements = elements.AsReadOnly();
            PointCount = pointCount;
            EstimatedBytes = estimatedBytes;
        }
    }

    /// <summary>
    /// 도형 수와 예상 비압축 바이트를 함께 제한한다.
    /// 배치에 넣을 때만 좌표를 복사하므로 전체 Map의 두 번째 좌표 배열을 만들지 않는다.
    /// </summary>
    public sealed class MapPlacedElementBatchBuilder
    {
        private readonly int _maxElements;
        private readonly long _maxBytes;
        private List<MapPlacedElementData> _elements = new List<MapPlacedElementData>();
        private long _pointCount;
        private long _estimatedBytes;

        /// <summary>Service의 한 요청 크기 제한을 생성 시 고정한다.</summary>
        public MapPlacedElementBatchBuilder(int maxElements, long maxBytes)
        {
            if (maxElements <= 0) throw new ArgumentOutOfRangeException(nameof(maxElements));
            if (maxBytes <= 0) throw new ArgumentOutOfRangeException(nameof(maxBytes));
            _maxElements = maxElements;
            _maxBytes = maxBytes;
        }

        public bool IsEmpty { get { return _elements.Count == 0; } }

        /// <summary>
        /// 현재 배치에 추가한다. 한도를 넘지만 단독으로는 담을 수 있으면 false를 반환해 먼저 배치를 보내게 한다.
        /// 도형 하나가 한도를 넘으면 분할 규약 없이 보내지 않도록 예외를 낸다.
        /// </summary>
        public bool TryAdd(MapPlacedElementData element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (element.WorldPoints == null || element.WorldPoints.Length == 0)
                throw new ArgumentException("좌표가 없는 도형은 배치에 넣을 수 없습니다.", nameof(element));

            long size = EstimateBytes(element);
            if (size > _maxBytes)
                throw new ArgumentException("도형 한 개가 배치 바이트 한도를 넘습니다.", nameof(element));
            if (_elements.Count >= _maxElements || _estimatedBytes + size > _maxBytes)
                return false;

            // 소비자가 비동기 전송하는 동안 화면 도형의 좌표 배열을 직접 참조하지 않게 한다.
            var copy = element.CloneForBatch();
            _elements.Add(copy);
            _pointCount += copy.WorldPoints.Length;
            _estimatedBytes += size;
            return true;
        }

        /// <summary>완성된 배치를 넘기고 다음 배치용 저장 공간만 새로 만든다.</summary>
        public MapPlacedElementBatch TakeBatch(int batchNo)
        {
            if (batchNo < 0) throw new ArgumentOutOfRangeException(nameof(batchNo));
            if (IsEmpty) throw new InvalidOperationException("빈 배치는 전송하지 않습니다.");
            var result = new MapPlacedElementBatch(batchNo, _elements, _pointCount, _estimatedBytes);
            _elements = new List<MapPlacedElementData>();
            _pointCount = 0;
            _estimatedBytes = 0;
            return result;
        }

        /// <summary>
        /// 좌표 1개를 X/Y double 16바이트로 계산하고 문자열 및 행 메타데이터의 예상 크기를 더한다.
        /// 실제 Service 직렬화 결과는 전송 직전에 별도로 제한해야 한다.
        /// </summary>
        public static long EstimateBytes(MapPlacedElementData element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (element.WorldPoints == null) throw new ArgumentException("좌표가 없습니다.", nameof(element));
            return checked(128L + (long)element.WorldPoints.Length * 16L
                + Utf8Length(element.PlacedElementId) + Utf8Length(element.SourceElementId)
                + Utf8Length(element.ElementType) + Utf8Length(element.Text)
                + Utf8Length(element.TextHorizontal) + Utf8Length(element.TextVertical));
        }

        private static int Utf8Length(string value)
        {
            return value == null ? 0 : Encoding.UTF8.GetByteCount(value);
        }
    }
}
