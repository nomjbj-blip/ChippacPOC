using NexplantQMS.GdsMap.Gds;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexplantQMS.GdsMap
{
	public partial class GdsMapControl
	{
		private readonly GlSceneLayerList _layerList = new GlSceneLayerList();
		private readonly GlDefectItemList _defectList = new GlDefectItemList();

		// 사용자 지정 색상은 실행 중 유지한다. 미지정 레이어는 번호별 기본 팔레트를 사용한다.
		private Dictionary<int, Color> _layerColor = new Dictionary<int, Color>();
		private static readonly Color[] DefaultLayerColors =
		{
			Color.FromArgb(80, 180, 80), Color.FromArgb(80, 140, 220),
			Color.FromArgb(220, 200, 60), Color.FromArgb(200, 90, 200),
			Color.FromArgb(70, 200, 200), Color.FromArgb(230, 140, 60),
			Color.FromArgb(150, 150, 150), Color.FromArgb(220, 80, 80),
			Color.FromArgb(140, 110, 230), Color.FromArgb(190, 220, 110),
			Color.FromArgb(240, 150, 180), Color.FromArgb(100, 190, 170)
		};

		private IEnumerable<GlSceneItem> AllItems => _layerList.SelectMany(v => v.Items);

		public void UpdateVisibleLayer()
		{
			if (Structure == null)
				return;

			foreach (var layer in Structure.Layers)
				_layerList.GetLayer(layer.LayerID).Visible = layer.Visible;

			InvalidateWithDrawProgress();
		}

		public void VisibleAllLayer(bool visible)
		{
			foreach (var layer in _layerList)
				layer.Visible = visible;

			InvalidateWithDrawProgress();
		}

		/// <summary>기존 호출 방식과 호환되는 일괄 색상 지정. 사전을 복사하고 현재 도면에도 반영한다.</summary>
		public void SetLayerColor(Dictionary<int, Color> dic)
		{
			_layerColor = dic == null ? new Dictionary<int, Color>()
				: dic.ToDictionary(p => p.Key, p => Color.FromArgb(255, p.Value));
			foreach (var layer in _layerList)
			{
				Color color = GetLayerColor(layer.LayerID);
				if (layer.Color.ToArgb() == color.ToArgb()) continue;
				layer.Color = color;
				UpdateLayerColorVertices(layer);
			}
			InvalidateWithDrawProgress();
		}

		/// <summary>1~8번의 기존 색상을 유지하고, 다른 번호에도 일정한 기본 색상을 배정한다.</summary>
		public Color GetDefaultLayerColor(int layerId)
		{
			int index = (int)((((long)layerId - 1) % DefaultLayerColors.Length
				+ DefaultLayerColors.Length) % DefaultLayerColors.Length);
			return DefaultLayerColors[index];
		}

		/// <summary>사용자 지정 색상이 없으면 기본 팔레트 색상을 반환한다.</summary>
		public Color GetLayerColor(int layerId)
		{
			return _layerColor.TryGetValue(layerId, out var color) ? color : GetDefaultLayerColor(layerId);
		}

		/// <summary>
		/// 한 레이어의 RGB 색상을 변경하고 GPU 데이터를 갱신한다.
		/// 표시 상태, 선택 상태, 확대/이동 위치는 유지하며 채우기 투명도는 ColorAlpha를 따른다.
		/// </summary>
		public bool SetLayerColor(int layerId, Color color)
		{
			var layer = _layerList.FirstOrDefault(p => p.LayerID == layerId);
			if (layer == null) return false;
			_layerColor[layerId] = Color.FromArgb(255, color);
			if (layer.Color.ToArgb() != _layerColor[layerId].ToArgb())
			{
				layer.Color = _layerColor[layerId];
				UpdateLayerColorVertices(layer);
			}
			InvalidateWithDrawProgress();
			return true;
		}

		/// <summary>현재 그려진 레이어를 목록으로 전달한다. 참조 구조에만 있는 레이어도 포함한다.</summary>
		public List<LayerDisplayItem> GetLayerDisplayItems()
		{
			return _layerList.Select(p => new LayerDisplayItem(p.LayerID, p.Color, p.Visible)).ToList();
		}

		/// <summary>
		/// 목록의 체크 상태를 실제 렌더링 레이어에 일괄 반영한 뒤 한 번만 다시 그린다.
		/// 최상위 Structure에도 같은 번호가 있으면 표시 상태를 함께 맞춘다.
		/// </summary>
		public void SetLayerVisibility(IDictionary<int, bool> visibility)
		{
			foreach (var layer in _layerList)
				if (visibility.TryGetValue(layer.LayerID, out var visible)) layer.Visible = visible;
			if (Structure != null)
				foreach (var layer in Structure.Layers)
					if (visibility.TryGetValue(layer.LayerID, out var visible)) layer.Visible = visible;
			InvalidateWithDrawProgress();
		}

		/// <summary>
		/// Square 항목만 선택이 가능하도록 설정
		/// </summary>
		[DefaultValue(false)]
		public bool OnlySelectSquareItems { get; set; } = false;
	}
}
