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
		public int LayerID { get; }
		public GPoint[] WorldPoints { get; }
		public bool Closed { get; }
		public double Width { get; }
		public string Text { get; }
		public GBox WorldBounds { get; }
		public bool Selected { get; set; }

		// Index offset and count in GPU VBO
		public int FillVertexOffset;
		public int FillVertexCount;
		public int LineVertexOffset;
		public int LineVertexCount;

		/// <summary>Flatten된 월드 도형과 그 배치 ID를 한 객체에 묶어 Map/Chain 참조가 어긋나지 않게 한다.</summary>
		public GlSceneItem(GdsElement source, GPoint[] worldPoints, bool closed, double width,
			string text = null, string placedElementId = null, string sourceElementId = null)
		{
			PlacedElementId = placedElementId;
			SourceElementId = sourceElementId;
			Source = source;
			LayerID = source.LayerID;
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

		public int CompareTo(GlSceneItem other)
		{
			return Source.CompareTo(other.Source);
		}
	}

	internal class GlSceneItemList : List<GlSceneItem>
	{
	}
}
