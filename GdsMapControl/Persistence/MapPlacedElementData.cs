using System;

namespace NexplantQMS.GdsMap.Persistence
{
    /// <summary>
    /// DB 저장 경계에서 사용하는 배치 도형 한 건이다.
    /// 원본 Grid 행과 실제 Map 도형의 ID를 함께 전달해 Chain 소속을 배치 도형에 연결한다.
    /// </summary>
    public sealed class MapPlacedElementData
    {
        public string PlacedElementId { get; set; }
        public string SourceElementId { get; set; }
        public int LayerId { get; set; }
        public int DataType { get; set; }
        public string ElementType { get; set; }
        public bool Closed { get; set; }
        public double PathWidth { get; set; }
        /// <summary>PATH 원본 타입 바이트다. 화면은 1/2를 특수 처리하고 그 외는 기본 끝 모양으로 그린다.</summary>
        public byte PathType { get; set; }
        public GBox Bounds { get; set; }
        public string Text { get; set; }
        public int? TextType { get; set; }
        public int? TextFont { get; set; }
        public string TextHorizontal { get; set; }
        public string TextVertical { get; set; }

        /// <summary>
        /// 동기 방문에서는 Flatten 결과를 빌려 쓰고, 비동기 배치에 추가할 때는 배열을 복사한다.
        /// 동기 방문 콜백 밖에서는 원본 배열을 수정하거나 보관하지 않는다.
        /// </summary>
        public GPoint[] WorldPoints { get; set; }

        /// <summary>비동기 배치가 현재 Map의 좌표 배열을 보유하지 않도록 현재 도형 한 건만 복사한다.</summary>
        internal MapPlacedElementData CloneForBatch()
        {
            var copy = (MapPlacedElementData)MemberwiseClone();
            copy.WorldPoints = (GPoint[])WorldPoints.Clone();
            return copy;
        }
    }
}
