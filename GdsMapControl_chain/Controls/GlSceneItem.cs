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

		public GlSceneItem(GdsElement source, GPoint[] worldPoints, bool closed, double width, string text = null)
		{
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
