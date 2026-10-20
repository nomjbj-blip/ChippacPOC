using NexplantQMS.GdsMap.Gds;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexplantQMS.GdsMap
{
	/// <summary>Layer별 도형을 보관한다. GPU 버퍼는 Layer 마다 따로 만든다(GdsMapControl.LayerGpuBuffer, 2026-10-20).</summary>
	internal class GlSceneLayer : IComparable<GlSceneLayer>
	{
		/// <param name="layerKey">Layer 번호 + DataType 키(GdsLayerKey, 2026-10-20)</param>
		public GlSceneLayer(int layerKey)
		{
			LayerKey = layerKey;
		}

		public Color Color { get; set; }
		/// <summary>Layer 번호 + DataType 키(GdsLayerKey). KLayout 처럼 18/0 / 18/3 / 18/4 를 다른 Layer 로 본다 (2026-10-20)</summary>
		public int LayerKey { get; set; }
		public bool Visible { get; set; } = true;
		public GlSceneItemList Items { get; set; } = new GlSceneItemList();
		public GBox Bounds = GBox.Empty;

		public int CompareTo(GlSceneLayer other)
		{
			return LayerKey.CompareTo(other.LayerKey);
		}
	}

	internal class GlSceneLayerList : List<GlSceneLayer> 
	{
		public GlSceneLayer GetLayer(int layerKey)
		{
			var layer = new GlSceneLayer(layerKey);
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
			var layer = AddLayer(it.LayerKey);

			var idx = layer.Items.BinarySearch(it);
			// 같은 순서 값이 이미 있어도(idx >= 0) 음수 위치로 넣지 않고 그 자리에 넣는다. (2026-10-15)
			layer.Items.Insert(idx < 0 ? ~idx : idx, it);
			layer.Bounds.Include(it.WorldBounds);
		}

		/// <summary>
		/// 정렬 없이 뒤에 붙인다. 도면 전체를 펼칠 때 쓰고, 다 넣은 뒤 SortSceneItems 를 한 번 부른다 (2026-10-20).
		/// 하나씩 정렬 위치에 넣으면 U8 제품 Cell(Layer 하나 48.9만 개)에서 20초가 걸렸다.
		/// 정렬 기준(GlSceneItem.CompareTo: 원본 -> 배치 ID)이 전체 순서라 한 번 정렬한 결과는 하나씩 넣은 결과와 같다.
		/// </summary>
		public void AppendSceneItem(GlSceneItem it)
		{
			var layer = AddLayer(it.LayerKey);
			layer.Items.Add(it);
			layer.Bounds.Include(it.WorldBounds);
		}

		/// <summary>AppendSceneItem 으로 넣은 Layer 항목을 정렬한다.</summary>
		public void SortSceneItems()
		{
			foreach (var layer in this)
				layer.Items.Sort();
		}

		public GlSceneLayer AddLayer(int layerKey)
		{
			var layer = new GlSceneLayer(layerKey);
			int idx = BinarySearch(layer);

			if (idx < 0)
				Insert(~idx, layer);
			else
				layer = this[idx];

			return layer;
		}
	}
}
