using System;

namespace NexplantQMS.GdsMap.Persistence
{
    /// <summary>
    /// 저장소에서 복원한 PLACED 도형을 기존 OpenGL/Chain 경로가 사용하는 GlSceneItem으로 바꾼다.
    /// 원본 GDS 파일 없이도 동일한 도형 종류, PATH 끝 모양, TEXT 표시 속성을 사용할 수 있게 한다.
    /// </summary>
    internal static class MapPlacedSceneItemFactory
    {
        /// <summary>
        /// 복원 데이터 한 건의 ID/좌표/종류를 확인하고 화면용 GDS 도형을 만든다.
        /// 반환된 SceneItem은 입력 WorldPoints 배열을 공유하므로 호출자는 입력 배열을 변경하지 않는다.
        /// </summary>
        internal static GlSceneItem Create(MapPlacedElementData data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (String.IsNullOrEmpty(data.PlacedElementId) || String.IsNullOrEmpty(data.SourceElementId)
                || data.WorldPoints == null || data.WorldPoints.Length == 0)
                throw new ArgumentException("ID와 좌표가 있는 배치 도형이 필요합니다.", nameof(data));

            GdsElement source;
            switch (data.ElementType)
            {
                case "BOUNDARY":
                    source = new GdsBoundary { Points = data.WorldPoints };
                    break;
                case "PATH":
                    if (Double.IsNaN(data.PathWidth) || Double.IsInfinity(data.PathWidth)
                        || data.PathWidth < 0)
                        throw new ArgumentException("PATH 폭이 잘못되었습니다.", nameof(data));
                    source = new GdsPath
                    {
                        Points = data.WorldPoints,
                        Width = data.PathWidth,
                        PathType = data.PathType
                    };
                    break;
                case "TEXT":
                    if (!Enum.TryParse(data.TextHorizontal, out GdsTextHorizontalPresentation horizontal)
                        || !Enum.TryParse(data.TextVertical, out GdsTextVerticalPresentation vertical)
                        || !Enum.IsDefined(typeof(GdsTextHorizontalPresentation), horizontal)
                        || !Enum.IsDefined(typeof(GdsTextVerticalPresentation), vertical))
                        throw new ArgumentException("TEXT 표시 기준점이 잘못되었습니다.", nameof(data));
                    source = new GdsText
                    {
                        Text = data.Text,
                        TextType = data.TextType ?? 0,
                        FontNumber = data.TextFont ?? 0,
                        HorizontalPresentation = horizontal,
                        VerticalPresentation = vertical,
                        // 저장 본문에는 월드 좌표만 있으므로 TEXT 기준 좌표도 월드 위치로 둔다.
                        Position = data.WorldPoints[0]
                    };
                    break;
                default:
                    throw new ArgumentException("지원하지 않는 배치 도형 종류입니다.", nameof(data));
            }

            source.LayerID = data.LayerId;
            source.DataType = data.DataType;
            source.ElementName = data.ElementType;
            source.Bounds = data.Bounds;
            return new GlSceneItem(source, data.WorldPoints, data.Closed, data.PathWidth,
                data.Text, data.PlacedElementId, data.SourceElementId);
        }
    }
}
