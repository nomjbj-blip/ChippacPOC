using System;
using System.Data;
using System.Globalization;
using System.IO;

namespace NexplantQMS.GdsMap.Persistence
{
    /// <summary>
    /// 조회 서비스(iGdsMapView.GetPlacedElementPage)가 돌려준 PLACED 행을 화면 도형 데이터(MapPlacedElementData)로 바꾼다.
    /// 설계: 문서/2026-10-02_GDS_Map_조회화면_설계.md 6절. GdsMapPlacedRowBuilder(저장)의 역변환이다.
    ///  - POINTS_BIN v1 = 월드 X/Y double little-endian 반복(점당 16바이트)
    ///  - PATH가 아닌 행의 PATH_WIDTH / PATH_TYPE, TEXT가 아닌 행의 TEXT 속성은 NULL로 저장되어 있다.
    /// 이 클래스는 DataRow만 다루며 UI 컨트롤에 접근하지 않으므로 백그라운드 스레드에서 호출할 수 있다.
    /// </summary>
    public static class GdsMapPlacedRowReader
    {
        /// <summary>
        /// PLACED 행 한 건을 해석한다.
        /// 처리 순서: 필수 값 확인 -> 좌표 형식 버전 / 바이트 길이 확인 -> 좌표 해석 -> 종류별 선택 값(PATH / TEXT) 채우기.
        /// </summary>
        public static MapPlacedElementData Read(DataRow row)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            string placedId = Text(row, "PLACED_ELEMENT_ID");
            string elementType = Text(row, "ELEMENT_TYPE");
            if (String.IsNullOrEmpty(placedId) || String.IsNullOrEmpty(elementType))
                throw new InvalidDataException("PLACED_ELEMENT_ID / ELEMENT_TYPE이 없는 행입니다.");

            int formatVersion = Int(row, "POINTS_FORMAT_VERSION");
            if (formatVersion != GdsMapPlacedRowBuilder.PointsFormatVersion)
                throw new InvalidDataException("지원하지 않는 좌표 형식 버전입니다: " + formatVersion + " / " + placedId);
            long pointCount = Convert.ToInt64(row["POINT_COUNT"], CultureInfo.InvariantCulture);
            byte[] bin = row["POINTS_BIN"] as byte[];
            if (bin == null || pointCount <= 0 || bin.LongLength != pointCount * 16)
                throw new InvalidDataException("좌표 바이트 길이가 점 수와 다릅니다: " + placedId);

            bool isPath = elementType == "PATH";
            bool isText = elementType == "TEXT";
            return new MapPlacedElementData
            {
                PlacedElementId = placedId,
                SourceElementId = Text(row, "SOURCE_ELEMENT_ID"),
                LayerId = Int(row, "LAYER_ID"),
                DataType = Int(row, "DATA_TYPE"),
                ElementType = elementType,
                Closed = Int(row, "CLOSED_FLAG") == 1,
                // 화면 Flatten은 PATH가 아닌 도형의 폭을 0으로 만든다. 저장 시 NULL로 비운 값을 같은 0으로 되돌린다.
                PathWidth = isPath ? Double(row, "PATH_WIDTH") : 0,
                PathType = isPath ? (byte)Int(row, "PATH_TYPE") : (byte)0,
                Bounds = new GBox(Double(row, "MIN_X"), Double(row, "MIN_Y"), Double(row, "MAX_X"), Double(row, "MAX_Y")),
                Text = isText ? Text(row, "TEXT_VALUE") : null,
                TextType = isText ? NullableInt(row, "TEXT_TYPE") : null,
                TextFont = isText ? NullableInt(row, "TEXT_FONT") : null,
                TextHorizontal = isText ? Text(row, "TEXT_HORIZONTAL") : null,
                TextVertical = isText ? Text(row, "TEXT_VERTICAL") : null,
                WorldPoints = DecodePoints(bin)
            };
        }

        /// <summary>POINTS_BIN v1 바이트를 월드 좌표 배열로 바꾼다. BitConverter는 실행 PC의 바이트 순서를 따르므로 little-endian PC만 허용한다.</summary>
        public static GPoint[] DecodePoints(byte[] bin)
        {
            if (bin == null) throw new ArgumentNullException(nameof(bin));
            if (!BitConverter.IsLittleEndian)
                throw new PlatformNotSupportedException("POINTS_BIN v1은 little-endian 환경에서만 해석합니다.");
            if (bin.Length % 16 != 0)
                throw new InvalidDataException("POINTS_BIN 길이가 16의 배수가 아닙니다: " + bin.Length);
            var points = new GPoint[bin.Length / 16];
            for (int i = 0, offset = 0; i < points.Length; i++, offset += 16)
                points[i] = new GPoint(BitConverter.ToDouble(bin, offset), BitConverter.ToDouble(bin, offset + 8));
            return points;
        }

        private static string Text(DataRow row, string column)
        {
            object value = row[column];
            return value == null || value == DBNull.Value ? null : value.ToString();
        }

        private static int Int(DataRow row, string column)
        {
            object value = row[column];
            if (value == null || value == DBNull.Value)
                throw new InvalidDataException(column + " 값이 없습니다.");
            return Convert.ToInt32(value, CultureInfo.InvariantCulture);
        }

        private static int? NullableInt(DataRow row, string column)
        {
            object value = row[column];
            return value == null || value == DBNull.Value ? (int?)null : Convert.ToInt32(value, CultureInfo.InvariantCulture);
        }

        /// <summary>BINARY_DOUBLE 열은 double로 온다. 다른 숫자 형식으로 와도 같은 값으로 바꾼다.</summary>
        private static double Double(DataRow row, string column)
        {
            object value = row[column];
            if (value == null || value == DBNull.Value)
                throw new InvalidDataException(column + " 값이 없습니다.");
            return Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }
    }
}
