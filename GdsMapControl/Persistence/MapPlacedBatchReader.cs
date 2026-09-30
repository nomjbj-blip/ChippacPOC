using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NexplantQMS.GdsMap.Persistence
{
    /// <summary>
    /// 저장된 PLACED 배치 본문을 Map 도형 데이터로 되돌린다.
    /// 형식/해시/건수/좌표 범위를 먼저 검사해 다른 도면의 잘못된 데이터를 그리지 않게 한다.
    /// </summary>
    public static class MapPlacedBatchReader
    {
        private static readonly Encoding Utf8 = new UTF8Encoding(false, true);

        /// <summary>형식 v1 배치를 검증하고 도형 목록을 복원한다. 한 번에 한 배치만 메모리에 올린다.</summary>
        public static MapPlacedElementBatch Read(byte[] payload, string expectedSha256,
            int maxPayloadBytes = 8 * 1024 * 1024, int maxElements = 2000)
        {
            if (payload == null) throw new ArgumentNullException(nameof(payload));
            if (expectedSha256 == null) throw new ArgumentNullException(nameof(expectedSha256));
            if (maxPayloadBytes <= 0) throw new ArgumentOutOfRangeException(nameof(maxPayloadBytes));
            if (maxElements <= 0) throw new ArgumentOutOfRangeException(nameof(maxElements));
            if (payload.Length > maxPayloadBytes)
                throw new InvalidDataException("배치 본문이 허용 크기를 넘었습니다.");
            if (!BitConverter.IsLittleEndian)
                throw new PlatformNotSupportedException("좌표 형식 v1은 little-endian 환경이 필요합니다.");

            string actualHash;
            using (var sha = SHA256.Create())
                actualHash = BitConverter.ToString(sha.ComputeHash(payload)).Replace("-", "").ToLowerInvariant();
            if (!String.Equals(actualHash, expectedSha256, StringComparison.OrdinalIgnoreCase))
                throw new InvalidDataException("배치 본문의 SHA-256이 다릅니다.");

            try
            {
                using (var stream = new MemoryStream(payload, false))
                using (var reader = new BinaryReader(stream, Utf8))
                {
                    byte[] magic = reader.ReadBytes(4);
                    if (magic.Length != 4 || magic[0] != 'G' || magic[1] != 'M'
                        || magic[2] != 'P' || magic[3] != '1')
                        throw new InvalidDataException("PLACED 배치 형식 식별자가 다릅니다.");
                    if (reader.ReadInt32() != MapPlacedBatchPayload.FormatVersion)
                        throw new InvalidDataException("지원하지 않는 PLACED 배치 형식 버전입니다.");
                    int batchNo = reader.ReadInt32();
                    int count = reader.ReadInt32();
                    long expectedPoints = reader.ReadInt64();
                    if (batchNo < 0 || count <= 0 || count > maxElements
                        || expectedPoints < count || expectedPoints > payload.Length / 16L)
                        throw new InvalidDataException("배치 헤더의 건수/좌표 수가 잘못되었습니다.");

                    var elements = new List<MapPlacedElementData>(count);
                    var placedIds = new HashSet<string>(StringComparer.Ordinal);
                    long points = 0;
                    long estimatedBytes = 0;
                    for (int index = 0; index < count; index++)
                    {
                        MapPlacedElementData element = ReadElement(reader);
                        if (!placedIds.Add(element.PlacedElementId))
                            throw new InvalidDataException("한 배치 안에 중복된 배치 Element ID가 있습니다.");
                        elements.Add(element);
                        points = checked(points + element.WorldPoints.Length);
                        estimatedBytes = checked(estimatedBytes
                            + MapPlacedElementBatchBuilder.EstimateBytes(element));
                    }
                    if (points != expectedPoints || stream.Position != stream.Length)
                        throw new InvalidDataException("배치 좌표 수 또는 본문 끝 위치가 다릅니다.");
                    return new MapPlacedElementBatch(batchNo, elements, points, estimatedBytes);
                }
            }
            catch (EndOfStreamException error)
            {
                throw new InvalidDataException("배치 본문이 중간에서 끊겼습니다.", error);
            }
            catch (DecoderFallbackException error)
            {
                throw new InvalidDataException("배치 문자열이 올바른 UTF-8이 아닙니다.", error);
            }
        }

        /// <summary>직렬화 코드와 같은 필드 순서로 도형 한 건을 읽고 DB 키/좌표를 검사한다.</summary>
        private static MapPlacedElementData ReadElement(BinaryReader reader)
        {
            string placedId = ReadString(reader);
            string sourceId = ReadString(reader);
            int layerId = reader.ReadInt32();
            int dataType = reader.ReadInt32();
            string elementType = ReadString(reader);
            byte closed = reader.ReadByte();
            double width = reader.ReadDouble();
            double minX = reader.ReadDouble();
            double minY = reader.ReadDouble();
            double maxX = reader.ReadDouble();
            double maxY = reader.ReadDouble();
            string text = ReadString(reader);
            int? textType = ReadNullableInt(reader);
            int? textFont = ReadNullableInt(reader);
            string textHorizontal = ReadString(reader);
            string textVertical = ReadString(reader);
            int pointCount = reader.ReadInt32();

            if (String.IsNullOrEmpty(placedId) || placedId.Length > 1024
                || String.IsNullOrEmpty(sourceId) || sourceId.Length > 512
                || (elementType != "BOUNDARY" && elementType != "PATH" && elementType != "TEXT")
                || closed > 1 || !IsFinite(width) || width < 0
                || !IsFinite(minX) || !IsFinite(minY) || !IsFinite(maxX) || !IsFinite(maxY)
                || minX > maxX || minY > maxY
                || pointCount <= 0 || pointCount > (reader.BaseStream.Length - reader.BaseStream.Position) / 16L)
                throw new InvalidDataException("배치 도형의 ID/종류/좌표 범위가 잘못되었습니다.");

            var worldPoints = new GPoint[pointCount];
            for (int index = 0; index < pointCount; index++)
            {
                double x = reader.ReadDouble();
                double y = reader.ReadDouble();
                if (!IsFinite(x) || !IsFinite(y))
                    throw new InvalidDataException("유효하지 않은 월드 좌표가 있습니다.");
                // GPoint 생성자는 소수 자리를 반올림하므로 저장된 double 값을 그대로 복원한다.
                worldPoints[index] = new GPoint { X = x, Y = y };
            }
            return new MapPlacedElementData
            {
                PlacedElementId = placedId,
                SourceElementId = sourceId,
                LayerId = layerId,
                DataType = dataType,
                ElementType = elementType,
                Closed = closed == 1,
                PathWidth = width,
                Bounds = new GBox(minX, minY, maxX, maxY),
                Text = text,
                TextType = textType,
                TextFont = textFont,
                TextHorizontal = textHorizontal,
                TextVertical = textVertical,
                WorldPoints = worldPoints
            };
        }

        /// <summary>null/빈 문자열을 구분하고 남은 본문 길이를 넘는 문자열을 거부한다.</summary>
        private static string ReadString(BinaryReader reader)
        {
            int length = reader.ReadInt32();
            if (length == -1) return null;
            if (length < 0 || length > reader.BaseStream.Length - reader.BaseStream.Position)
                throw new InvalidDataException("배치 문자열 길이가 잘못되었습니다.");
            byte[] bytes = reader.ReadBytes(length);
            return Utf8.GetString(bytes);
        }

        /// <summary>TEXT 선택 숫자의 존재 플래그가 0/1인지 확인한 뒤 값을 읽는다.</summary>
        private static int? ReadNullableInt(BinaryReader reader)
        {
            byte exists = reader.ReadByte();
            if (exists == 0) return null;
            if (exists != 1) throw new InvalidDataException("선택 숫자 플래그가 잘못되었습니다.");
            return reader.ReadInt32();
        }

        private static bool IsFinite(double value)
        {
            return !Double.IsNaN(value) && !Double.IsInfinity(value);
        }
    }
}
