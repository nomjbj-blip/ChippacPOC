using NexplantQMS.GdsMap.Gds;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexplantQMS.GdsMap
{
	public partial class GdsMapControl
	{
		private readonly GlSceneLayerList _layerList = new GlSceneLayerList();
		private readonly GlDefectItemList _defectList = new GlDefectItemList();

		private Dictionary<int, System.Drawing.Color> _layerColor;

		private IEnumerable<GlSceneItem> AllItems => _layerList.SelectMany(v => v.Items);

		public void UpdateVisibleLayer()
		{
			if (Structure == null)
				return;

			foreach (var layer in Structure.Layers)
				_layerList.GetLayer(layer.LayerID).Visible = layer.Visible;

			BuildGpuBuffers();
			Invalidate();
		}

		public void VisibleAllLayer(bool visible)
		{
			foreach (var layer in _layerList)
				layer.Visible = visible;

			BuildGpuBuffers();
			Invalidate();
		}

		public void SetLayerColor(Dictionary<int, System.Drawing.Color> dic)
		{
			_layerColor = dic;
		}

		/// <summary>
		/// Square 항목만 선택이 가능하도록 설정
		/// </summary>
		[DefaultValue(false)]
		public bool OnlySelectSquareItems { get; set; } = false;
	}
}
