using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexplantQMS.GdsMap
{
	internal class GlDefectItem
	{
		public Defect Source { get; }
		public GPoint[] WorldPoints { get; }
		public GBox WorldBounds { get; }

		// Index offset and count in GPU VBO
		public int FillVertexOffset;
		public int FillVertexCount;
		public int LineVertexOffset;
		public int LineVertexCount;

		public GlDefectItem(Defect source, GPoint[] worldPoints)
		{
			Source = source;
			WorldPoints = worldPoints;

			GBox b = GBox.Empty;

			foreach (var p in worldPoints)
				b.Include(p);

			WorldBounds = b;
		}
	}

	internal class GlDefectItemList : List<GlDefectItem>
	{
	}
}
