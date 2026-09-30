using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NexplantQMS.GdsMap.Persistence
{
    /// <summary>
    /// Service의 PLACED 배치 업로드 요청에 필요한 메타데이터와 본문이다.
    /// JobId/BatchNo/해시를 함께 보내 재전송 시 같은 배치인지 판별한다.
    /// </summary>
    public sealed class MapPlacedBatchPayload
    {
        public const int FormatVersion = 1;
        private static readonly Encoding Utf8 = new UTF8Encoding(false, true);
        public string JobId { get; private set; }
        public string MapRevisionId { get; private set; }
        public string StreamKind { get { return "PLACED"; } }
        public int BatchNo { get; private set; }
        public int ElementCount { get; private set; }
        public long PointCount { get; private set; }
        public string PayloadSha256 { get; private set; }
        public byte[] Payload { get; private set; }

        private MapPlacedBatchPayload() { }

        /// <summary>
        /// 형식 v1로 한 배치를 직렬화하고 실제 요청 본문 크기를 검사한다.
        /// 반환된 본문은 압축이나 Base64 변환 없이 바이너리 그대로 전송하는 계약이다.
        /// </summary>
        public static MapPlacedBatchPayload Create(MapPlacedElementBatch batch,
            string jobId, string mapRevisionId, int maxPayloadBytes)
        {
            if (batch == null) throw new ArgumentNullException(nameof(batch));
            if (!Guid.TryParse(jobId, out Guid job))
                throw new ArgumentException("유효한 JobId가 필요합니다.", nameof(jobId));
            if (!Guid.TryParse(mapRevisionId, out Guid revision))
                throw new ArgumentException("유효한 MapRevisionId가 필요합니다.", nameof(mapRevisionId));
            if (maxPayloadBytes <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxPayloadBytes));
            if (!BitConverter.IsLittleEndian)
                throw new PlatformNotSupportedException("좌표 형식 v1은 little-endian 환경이 필요합니다.");

            byte[] bytes;
            using (var stream = new MemoryStream((int)Math.Min(batch.EstimatedBytes, maxPayloadBytes)))
            using (var writer = new BinaryWriter(stream, Utf8))
            {
                // GMP1 / 버전 / 배치 번호 / 도형 수 / 좌표 수 뒤에 도형 데이터를 순서대로 기록한다.
                writer.Write(new byte[] { (byte)'G', (byte)'M', (byte)'P', (byte)'1' });
                writer.Write(FormatVersion);
                writer.Write(batch.BatchNo);
                writer.Write(batch.Elements.Count);
                writer.Write(batch.PointCount);

                long pointCount = 0;
                foreach (MapPlacedElementData element in batch.Elements)
                {
                    WriteElement(writer, element);
                    pointCount += element.WorldPoints.Length;
                    if (stream.Length > maxPayloadBytes)
                        throw new InvalidOperationException("직렬화한 배치가 실제 요청 크기 한도를 넘었습니다.");
                }
                if (pointCount != batch.PointCount)
                    throw new InvalidOperationException("배치 좌표 수가 직렬화 결과와 다릅니다.");
                writer.Flush();
                bytes = stream.ToArray();
            }

            string sha256;
            using (var hash = SHA256.Create())
                sha256 = BitConverter.ToString(hash.ComputeHash(bytes)).Replace("-", "").ToLowerInvariant();

            return new MapPlacedBatchPayload
            {
                JobId = job.ToString("D"),
                MapRevisionId = revision.ToString("D"),
                BatchNo = batch.BatchNo,
                ElementCount = batch.Elements.Count,
                PointCount = batch.PointCount,
                PayloadSha256 = sha256,
                Payload = bytes
            };
        }

        /// <summary>도형의 식별자/표시 속성/Bounds/월드 좌표를 순서가 고정된 형식으로 기록한다.</summary>
        private static void WriteElement(BinaryWriter writer, MapPlacedElementData element)
        {
            if (element == null || element.WorldPoints == null || element.WorldPoints.Length == 0)
                throw new InvalidOperationException("좌표가 없는 도형은 직렬화할 수 없습니다.");
            if (String.IsNullOrEmpty(element.PlacedElementId)
                || String.IsNullOrEmpty(element.SourceElementId))
                throw new InvalidOperationException("ID가 없는 도형은 직렬화할 수 없습니다.");
            WriteString(writer, element.PlacedElementId);
            WriteString(writer, element.SourceElementId);
            writer.Write(element.LayerId);
            writer.Write(element.DataType);
            WriteString(writer, element.ElementType);
            writer.Write(element.Closed);
            writer.Write(element.PathWidth);
            writer.Write(element.Bounds.MinX);
            writer.Write(element.Bounds.MinY);
            writer.Write(element.Bounds.MaxX);
            writer.Write(element.Bounds.MaxY);
            WriteString(writer, element.Text);
            WriteNullableInt(writer, element.TextType);
            WriteNullableInt(writer, element.TextFont);
            WriteString(writer, element.TextHorizontal);
            WriteString(writer, element.TextVertical);
            writer.Write(element.WorldPoints.Length);
            foreach (GPoint point in element.WorldPoints)
            {
                if (Double.IsNaN(point.X) || Double.IsInfinity(point.X)
                    || Double.IsNaN(point.Y) || Double.IsInfinity(point.Y))
                    throw new InvalidOperationException("유효하지 않은 도형 좌표가 있습니다.");
                writer.Write(point.X);
                writer.Write(point.Y);
            }
        }

        /// <summary>null은 길이 -1, 나머지는 UTF-8 바이트 길이와 본문으로 기록한다.</summary>
        private static void WriteString(BinaryWriter writer, string value)
        {
            if (value == null) { writer.Write(-1); return; }
            byte[] bytes = Utf8.GetBytes(value);
            writer.Write(bytes.Length);
            writer.Write(bytes);
        }

        /// <summary>TEXT 전용 선택 숫자는 존재 여부를 먼저 기록해 0과 null을 구분한다.</summary>
        private static void WriteNullableInt(BinaryWriter writer, int? value)
        {
            writer.Write(value.HasValue);
            if (value.HasValue) writer.Write(value.Value);
        }
    }
}
