using System;
using System.Data;
using System.Globalization;

namespace NexplantQMS.GdsMap
{
	/// <summary>
	/// 화면 / Chain / DB 에서 Layer 를 구분하는 키 = Layer 번호 + DataType (2026-10-20).
	/// KLayout Layer 목록과 같은 단위다. TEXT 는 DATATYPE 이 없어 TEXTTYPE 을 DataType 자리에 쓴다.
	/// 예: U8 Layer 18 = 18/0(도형) / 18/3(핀 번호 Label) / 18/4(Net 이름 Label).
	/// int 하나로 다루기 위해 Layer 번호 x 65536 + DataType 으로 만든다(GDS Layer / DataType 은 0 ~ 32767).
	/// </summary>
	public static class GdsLayerKey
	{
		public static int Make(int layer, int dataType)
		{
			return layer * 65536 + (dataType & 0xFFFF);
		}

		/// <summary>요소의 키. TEXT 는 TEXTTYPE, 그 밖에는 DATATYPE.</summary>
		public static int Of(GdsElement element)
		{
			var text = element as GdsText;
			return Make(element.LayerID, text != null ? text.TextType : element.DataType);
		}

		public static int Layer(int key)
		{
			return key >> 16;
		}

		public static int DataType(int key)
		{
			return key & 0xFFFF;
		}

		/// <summary>화면 / 문서 표시 형식 "18/3"</summary>
		public static string Format(int key)
		{
			return Layer(key).ToString(CultureInfo.InvariantCulture) + "/" + DataType(key).ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// DB 행의 Layer 번호 / DataType 열로 키를 만든다. DataType 열이 없거나(이전 서버 응답) 비어 있으면 0 으로 본다.
		/// </summary>
		public static int FromRow(DataRow row, string layerColumn = "LAYER_ID", string dataTypeColumn = "DATA_TYPE")
		{
			int layer = Convert.ToInt32(row[layerColumn], CultureInfo.InvariantCulture);
			int dataType = row.Table.Columns.Contains(dataTypeColumn) && row[dataTypeColumn] != DBNull.Value
				? Convert.ToInt32(row[dataTypeColumn], CultureInfo.InvariantCulture) : 0;
			return Make(layer, dataType);
		}

		/// <summary>"18/3" -> 키. DataType 이 없으면("18") 0 으로 본다.</summary>
		public static int Parse(string text)
		{
			if (String.IsNullOrWhiteSpace(text))
				throw new FormatException("Layer 값이 비어 있습니다.");
			string[] parts = text.Trim().Split('/');
			int layer = Int32.Parse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture);
			int dataType = parts.Length > 1 ? Int32.Parse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture) : 0;
			return Make(layer, dataType);
		}
	}
}
