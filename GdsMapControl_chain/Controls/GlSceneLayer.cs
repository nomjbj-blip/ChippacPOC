using NexplantQMS.GdsMap.Gds;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexplantQMS.GdsMap
{
	/// <summary>Layer별 도형과 고정 VBO 구간을 보관하여 체크/색상 변경 시 전체 버퍼를 재구성하지 않는다.</summary>
	internal class GlSceneLayer : IComparable<GlSceneLayer>
	{
		public GlSceneLayer(int layerID)
		{
			LayerID = layerID;
		}

		public Color Color { get; set; }
		public int LayerID { get; set; }
		public bool Visible { get; set; } = true;
		public GlSceneItemList Items { get; set; } = new GlSceneItemList();
		public GBox Bounds = GBox.Empty;
		// 파일 조회 시 모든 Layer를 연속 구간으로 배치한다. 표시/색상 변경 중에는 이 위치를 유지한다.
		public int VertexOffset { get; set; }
		public int VertexCount { get; set; }

		public int CompareTo(GlSceneLayer other)
		{
			return LayerID.CompareTo(other.LayerID);
		}
	}

	internal class GlSceneLayerList : List<GlSceneLayer> 
	{
		public GlSceneLayer GetLayer(int layerID)
		{
			var layer = new GlSceneLayer(layerID);
			int idx = BinarySearch(layer);

			if (idx < 0)
				throw null;

			return this[idx];
		}

		public void SetLayerColor(Dictionary<int, Color> layerColordic)
		{
			foreach (var item in layerColordic)
			{
				int layer = item.Key;

				var layerObj = GetLayer(layer);

				if (layerObj != null)
					layerObj.Color = item.Value;
			}
		}

		public void AddSceneItem(GlSceneItem it)
		{
			var layer = AddLayer(it.LayerID);

			var idx = layer.Items.BinarySearch(it);
			layer.Items.Insert(~idx, it);
			layer.Bounds.Include(it.WorldBounds);
		}

		public GlSceneLayer AddLayer(int layerID)
		{
			var layer = new GlSceneLayer(layerID);
			int idx = BinarySearch(layer);

			if (idx < 0)
				Insert(~idx, layer);
			else
				layer = this[idx];

			return layer;
		}
	}
}
