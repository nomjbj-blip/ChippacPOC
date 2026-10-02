using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace NexplantQMS.GdsMap.Persistence
{
    /// <summary>
    /// 화면 배치 도형(MapPlacedElementData)을 DB 저장 서비스(iGdsMapImport.CreatePlacedBatch)에 보낼 행으로 바꾼다.
    /// 설계: 문서/2026-10-02_GDS_Map_DB저장_설계.md 6.3절.
    /// 서비스는 기존 DMS 방식대로 string[,]만 받으므로
    ///  - 좌표(POINTS_BIN v1 = 월드 X/Y double little-endian 반복, 점당 16바이트)는 16진수 문자열로,
    ///  - double(Bounds / PATH 폭)은 G17 문자열로(서버 / DB에서 TO_BINARY_DOUBLE로 정확히 복원) 보낸다.
    /// 배치는 GdsMapControl.VisitPlacedElementBatchesAsync가 만든 MapPlacedElementBatch를 그대로 변환한다.
    /// </summary>
    public static class GdsMapPlacedRowBuilder
    {
        public const int PointsFormatVersion = 1;
        public const int ColumnCount = 20;

        /// <summary>배치 하나를 서비스 열 순서(iGdsMapImport.CreatePlacedBatch 설명)의 2차원 배열로 바꾼다.</summary>
        public static string[,] BuildRows(IList<MapPlacedElementData> elements)
        {
            if (elements == null) throw new ArgumentNullException(nameof(elements));
            var rows = new string[elements.Count, ColumnCount];
            for (int i = 0; i < elements.Count; i++)
            {
                string[] row = BuildRow(elements[i]);
                for (int c = 0; c < ColumnCount; c++)
                    rows[i, c] = row[c];
            }
            return rows;
        }

        /// <summary>배치 도형 한 건을 20열로 바꾼다. PATH가 아니면 PATH 폭 / 타입을, TEXT가 아니면 TEXT 속성을 비운다.</summary>
        public static string[] BuildRow(MapPlacedElementData data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (data.WorldPoints == null || data.WorldPoints.Length == 0)
                throw new InvalidOperationException("좌표가 없는 배치 도형은 저장할 수 없습니다: " + data.PlacedElementId);
            bool isPath = data.ElementType == "PATH";
            bool isText = data.ElementType == "TEXT";
            return new[]
            {
                data.PlacedElementId,
                data.SourceElementId,
                data.LayerId.ToString(CultureInfo.InvariantCulture),
                data.DataType.ToString(CultureInfo.InvariantCulture),
                data.ElementType,
                data.Closed ? "1" : "0",
                isPath ? D(data.PathWidth) : null,
                isPath ? data.PathType.ToString(CultureInfo.InvariantCulture) : null,
                D(data.Bounds.MinX),
                D(data.Bounds.MinY),
                D(data.Bounds.MaxX),
                D(data.Bounds.MaxY),
                data.WorldPoints.Length.ToString(CultureInfo.InvariantCulture),
                PointsFormatVersion.ToString(CultureInfo.InvariantCulture),
                EncodePointsHex(data.WorldPoints),
                isText ? data.Text : null,
                isText && data.TextType.HasValue ? data.TextType.Value.ToString(CultureInfo.InvariantCulture) : null,
                isText && data.TextFont.HasValue ? data.TextFont.Value.ToString(CultureInfo.InvariantCulture) : null,
                isText ? data.TextHorizontal : null,
                isText ? data.TextVertical : null
            };
        }

        /// <summary>월드 좌표를 POINTS_BIN v1 바이트(X, Y double little-endian 반복)로 만든 뒤 16진수 문자열로 바꾼다.</summary>
        public static string EncodePointsHex(GPoint[] points)
        {
            using (var stream = new MemoryStream(points.Length * 16))
            using (var writer = new BinaryWriter(stream))
            {
                foreach (GPoint point in points)
                {
                    writer.Write(point.X);
                    writer.Write(point.Y);
                }
                writer.Flush();
                return GdsMapSourceRowBuilder.ToHex(stream.ToArray());
            }
        }

        /// <summary>double을 되돌렸을 때 같은 값이 되는 G17 문자열로 바꾼다. .NET Framework의 "R"은 일부 값에서 왕복이 보장되지 않는다.</summary>
        private static string D(double value)
        {
            return value.ToString("G17", CultureInfo.InvariantCulture);
        }
    }
}
