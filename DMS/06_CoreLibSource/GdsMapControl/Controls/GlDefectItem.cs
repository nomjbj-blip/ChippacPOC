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

		// 결함 GPU 버퍼 안의 위치 (2026-10-20 Layer 별 버퍼로 변경)
		public int VertexOffset;
		public int VertexCount;
		public int FillIndexOffset;
		public int FillIndexCount;

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
