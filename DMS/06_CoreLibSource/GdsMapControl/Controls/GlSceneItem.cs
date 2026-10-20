using NexplantQMS.GdsMap.Gds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexplantQMS.GdsMap
{
	internal class GlSceneItem : IComparable<GlSceneItem>
	{
		/// <summary>현재 도면 개정본 안에서 배치 위치를 구분하는 ID다. Chain과 DB 도형 행이 같은 값을 사용한다.</summary>
		public string PlacedElementId { get; }
		/// <summary>원본 Grid 행과 여러 배치 인스턴스를 연결할 때 사용하는 원본 Element ID다.</summary>
		public string SourceElementId { get; }
		public GdsElement Source { get; }
		/// <summary>GDS Layer 번호(DB LAYER_ID)</summary>
		public int LayerID { get; }
		/// <summary>화면 Layer 키 = Layer 번호 + DataType(TEXT 는 TEXTTYPE) (GdsLayerKey, 2026-10-20)</summary>
		public int LayerKey { get; }
		public GPoint[] WorldPoints { get; }
		public bool Closed { get; }
		public double Width { get; }
		public string Text { get; }
		public GBox WorldBounds { get; }
		public bool Selected { get; set; }

		// Layer GPU 버퍼 안의 위치 (2026-10-20 Layer 별 버퍼로 변경)
		// 정점: 선택 표시를 바꿀 구간 / 채우기 인덱스: 선택 항목을 위에 다시 그릴 구간 / 외곽선: LineLoop(닫힌 도형) 또는 LineStrip(열린 선)
		public int VertexOffset;
		public int VertexCount;
		public int FillIndexOffset;
		public int FillIndexCount;
		public int LineFirst = -1;
		public int LineCount;
		public bool LineLoop;

		/// <summary>Flatten된 월드 도형과 그 배치 ID를 한 객체에 묶어 Map/Chain 참조가 어긋나지 않게 한다.</summary>
		public GlSceneItem(GdsElement source, GPoint[] worldPoints, bool closed, double width,
			string text = null, string placedElementId = null, string sourceElementId = null)
		{
			PlacedElementId = placedElementId;
			SourceElementId = sourceElementId;
			Source = source;
			LayerID = source.LayerID;
			LayerKey = GdsLayerKey.Of(source);
			WorldPoints = worldPoints;
			Closed = closed;
			Width = width;
			Text = text;

			GBox b = GBox.Empty;
			foreach (var p in worldPoints) b.Include(p);
			if (width > 0)
			{
				double r = width / 2;
				b.MinX -= r; b.MaxX += r; b.MinY -= r; b.MaxY += r;
			}
			WorldBounds = b;
		}

		/// <summary>
		/// 원본 요소 순서로 정렬하고, 원본이 같으면(같은 셀을 여러 번 배치) 배치 ID로 구분한다.
		/// Layer 목록 / 선택 목록의 BinarySearch가 같은 셀의 다른 배치를 같은 도형으로 보지 않게 한다. (2026-10-15)
		/// </summary>
		public int CompareTo(GlSceneItem other)
		{
			// 같은 셀을 여러 번 배치한 도형은 원본 객체가 같아 원본 비교가 좌표 배열 끝까지 간 뒤 0 이 된다. 바로 배치 ID 로 비교한다 (2026-10-20)
			int source = ReferenceEquals(Source, other.Source) ? 0 : Source.CompareTo(other.Source);
			return source != 0 ? source : String.CompareOrdinal(PlacedElementId, other.PlacedElementId);
		}
	}

	internal class GlSceneItemList : List<GlSceneItem>
	{
	}
}
