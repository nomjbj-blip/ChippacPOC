using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace NexplantQMS.GdsMap.Persistence
{
    /// <summary>
    /// GDS 원본(파서 결과)을 DB 저장 서비스(iGdsMapImport)에 보낼 Layer / SOURCE 배치 행으로 바꾼다.
    /// 설계: 문서/2026-10-02_GDS_Map_DB저장_설계.md 6.1 / 6.2절.
    /// 서비스는 기존 DMS 방식대로 string[,]만 받으므로 BLOB Payload는 16진수 문자열로 보낸다.
    ///
    /// SOURCE_ELEMENT_ID 규칙
    ///  - 도면 목록 행: Structure명 + "/E" + 순번. Flatten / Grid와 같은 순서(Structure의 Layer 순 -> Element 순)로 0부터 매긴다.
    ///  - 파서 중복 제외 행(GdsLayerList.DuplicatedItems): Structure명 + "/D" + 순번(0부터). 배치 도형에서 참조되지 않는다.
    ///
    /// SOURCE_PAYLOAD v1 (little-endian, 문자열은 Int32 UTF-8 바이트 수 + 본문 / null은 -1)
    ///  공통: Byte 종류(1 BOUNDARY / 2 PATH / 3 TEXT / 4 SREF / 5 AREF), String Name, String ElementName,
    ///        Double Rotation, Double Magnification, Byte MirrorX(0/1)
    ///  BOUNDARY: Int32 점 수, (Double X, Double Y) 반복
    ///  PATH    : Double Width, Byte PathType, Int32 점 수, (X, Y) 반복
    ///  TEXT    : String Text, Double X, Double Y, Int32 TextType, Int32 FontNumber, Byte 가로 정렬, Byte 세로 정렬
    ///  SREF    : String StructureName, Double OriginX, Double OriginY
    ///  AREF    : String StructureName, Int32 Columns, Int32 Rows, (X, Y) Origin / ColVector / RowVector
    ///  좌표는 파서가 만든 값(GdsReader.LengthUnit 변환 / 소수 5자리 반올림 후)이다.
    /// </summary>
    public static class GdsMapSourceRowBuilder
    {
        public const int SourcePayloadVersion = 1;

        /// <summary>Layer 배치 행(LAYER_ID / COLOR_ARGB / DISPLAY_NAME)을 만든다. 배치 도형 FK 대상이므로 화면의 전체 Layer를 넘긴다.</summary>
        public static string[,] BuildLayerRows(IList<KeyValuePair<int, int>> layerColors)
        {
            if (layerColors == null) throw new ArgumentNullException(nameof(layerColors));
            var rows = new string[layerColors.Count, 3];
            for (int i = 0; i < layerColors.Count; i++)
            {
                rows[i, 0] = layerColors[i].Key.ToString(CultureInfo.InvariantCulture);
                rows[i, 1] = layerColors[i].Value.ToString(CultureInfo.InvariantCulture);
                rows[i, 2] = null;
            }
            return rows;
        }

        /// <summary>원본 Element 전체에서 Layer ID 목록을 모은다. 화면 없이 저장할 때(Probe) 사용한다.</summary>
        public static List<int> CollectLayerIds(GdsLibrary library)
        {
            var ids = new SortedSet<int>();
            foreach (var structure in library.Structures)
                foreach (var layer in structure.Layers)
                    ids.Add(layer.LayerID);
            return ids.ToList();
        }

        /// <summary>저장할 SOURCE 행 수(도면 목록 행 + 파서 중복 행). 저장 시작 시 예상 건수로 보낸다.</summary>
        public static long CountSourceRows(GdsLibrary library)
        {
            long count = 0;
            foreach (var structure in library.Structures)
                count += structure.Layers.Sum(layer => (long)layer.Elements.Count) + structure.Layers.DuplicatedItems.Count;
            return count;
        }

        /// <summary>
        /// SOURCE 행을 최대 maxRows건씩 나눠 돌려준다. 열 순서는 iGdsMapImport.CreateSourceBatch 설명과 같다.
        /// 한 번에 전체를 만들지 않고 배치 단위로 만들어 대형 GDS에서도 메모리를 배치 크기로 제한한다.
        /// </summary>
        public static IEnumerable<string[,]> BuildSourceBatches(GdsLibrary library, int maxRows)
        {
            if (library == null) throw new ArgumentNullException(nameof(library));
            if (maxRows <= 0) throw new ArgumentOutOfRangeException(nameof(maxRows));
            var pending = new List<string[]>(maxRows);
            foreach (var structure in library.Structures)
            {
                int ordinal = 0;
                foreach (var layer in structure.Layers)
                    foreach (var element in layer.Elements)
                    {
                        pending.Add(BuildSourceRow(structure.Name, "/E", ordinal++, element, false));
                        if (pending.Count == maxRows) { yield return ToArray(pending); pending.Clear(); }
                    }
                int duplicateOrdinal = 0;
                foreach (var duplicate in structure.Layers.DuplicatedItems)
                {
                    pending.Add(BuildSourceRow(structure.Name, "/D", duplicateOrdinal++, duplicate, true));
                    if (pending.Count == maxRows) { yield return ToArray(pending); pending.Clear(); }
                }
            }
            if (pending.Count > 0) yield return ToArray(pending);
        }

        /// <summary>원본 Element 한 건을 SOURCE 행 9열로 바꾼다.</summary>
        private static string[] BuildSourceRow(string structureName, string prefix, int ordinal, GdsElement element, bool duplicate)
        {
            return new[]
            {
                structureName + prefix + ordinal.ToString(CultureInfo.InvariantCulture),
                structureName,
                ordinal.ToString(CultureInfo.InvariantCulture),
                ElementTypeName(element),
                element.LayerID.ToString(CultureInfo.InvariantCulture),
                element.DataType.ToString(CultureInfo.InvariantCulture),
                duplicate ? "1" : "0",
                SourcePayloadVersion.ToString(CultureInfo.InvariantCulture),
                ToHex(EncodeSourcePayload(element))
            };
        }

        /// <summary>DB ELEMENT_TYPE 값. 서버 검증 목록(BOUNDARY / PATH / TEXT / SREF / AREF)과 같아야 한다.</summary>
        public static string ElementTypeName(GdsElement element)
        {
            if (element is GdsBoundary) return "BOUNDARY";
            if (element is GdsPath) return "PATH";
            if (element is GdsText) return "TEXT";
            if (element is GdsSRef) return "SREF";
            if (element is GdsARef) return "AREF";
            throw new NotSupportedException("저장할 수 없는 Element 종류입니다: " + element.GetType().Name);
        }

        /// <summary>원본 Element 한 건을 SOURCE_PAYLOAD v1 바이너리로 만든다. 형식은 클래스 설명 참고.</summary>
        public static byte[] EncodeSourcePayload(GdsElement element)
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false)))
            {
                writer.Write(ElementKind(element));
                WriteString(writer, element.Name);
                WriteString(writer, element.ElementName);
                writer.Write(element.Transform.Rotation);
                writer.Write(element.Transform.Magnification);
                writer.Write((byte)(element.Transform.MirrorX ? 1 : 0));

                if (element is GdsBoundary boundary)
                    WritePoints(writer, boundary.Points);
                else if (element is GdsPath path)
                {
                    writer.Write(path.Width);
                    writer.Write(path.PathType);
                    WritePoints(writer, path.Points);
                }
                else if (element is GdsText text)
                {
                    WriteString(writer, text.Text);
                    writer.Write(text.Position.X);
                    writer.Write(text.Position.Y);
                    writer.Write(text.TextType);
                    writer.Write(text.FontNumber);
                    writer.Write((byte)text.HorizontalPresentation);
                    writer.Write((byte)text.VerticalPresentation);
                }
                else if (element is GdsSRef sref)
                {
                    WriteString(writer, sref.StructureName);
                    writer.Write(sref.Origin.X);
                    writer.Write(sref.Origin.Y);
                }
                else if (element is GdsARef aref)
                {
                    WriteString(writer, aref.StructureName);
                    writer.Write(aref.Columns);
                    writer.Write(aref.Rows);
                    writer.Write(aref.Origin.X); writer.Write(aref.Origin.Y);
                    writer.Write(aref.ColVector.X); writer.Write(aref.ColVector.Y);
                    writer.Write(aref.RowVector.X); writer.Write(aref.RowVector.Y);
                }
                writer.Flush();
                return stream.ToArray();
            }
        }

        private static byte ElementKind(GdsElement element)
        {
            if (element is GdsBoundary) return 1;
            if (element is GdsPath) return 2;
            if (element is GdsText) return 3;
            if (element is GdsSRef) return 4;
            if (element is GdsARef) return 5;
            throw new NotSupportedException("저장할 수 없는 Element 종류입니다: " + element.GetType().Name);
        }

        private static void WritePoints(BinaryWriter writer, GPoint[] points)
        {
            int count = points == null ? 0 : points.Length;
            writer.Write(count);
            for (int i = 0; i < count; i++)
            {
                writer.Write(points[i].X);
                writer.Write(points[i].Y);
            }
        }

        private static void WriteString(BinaryWriter writer, string value)
        {
            if (value == null) { writer.Write(-1); return; }
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            writer.Write(bytes.Length);
            writer.Write(bytes);
        }

        /// <summary>byte[]를 대문자 16진수 문자열로 바꾼다. 서버는 2,000바이트 이하면 HEXTORAW 다건 저장, 넘으면 BLOB 한 건 저장으로 처리한다.</summary>
        public static string ToHex(byte[] bytes)
        {
            const string digits = "0123456789ABCDEF";
            var chars = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                chars[i * 2] = digits[bytes[i] >> 4];
                chars[i * 2 + 1] = digits[bytes[i] & 0x0F];
            }
            return new string(chars);
        }

        private static string[,] ToArray(List<string[]> rows)
        {
            var result = new string[rows.Count, 9];
            for (int i = 0; i < rows.Count; i++)
                for (int c = 0; c < 9; c++)
                    result[i, c] = rows[i][c];
            return result;
        }
    }
}
