using OpenTK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
	public partial class GdsMapControl
	{
		private bool _isPanning;
		private PointF _panStartScreen;
		private Vector2 _panStartOffset;

		private bool _isDragTracking;
		private PointF _rubberStartScreen;
		private PointF _rubberCurrentScreen;
		//private HashSet<GlSceneItem> _dragInitialSelection; // 드래그 시작 시의 선택 상태 스냅샷
		private List<GlSceneItem> _selectedItems = new List<GlSceneItem>();

		protected override void OnMouseDown(MouseEventArgs e)
		{
			Focus();

			if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Middle)
			{
				if (_mode == ViewMode.View || e.Button == MouseButtons.Middle)
				{
					_isPanning = true;
					_panStartScreen = e.Location;
					_panStartOffset = _offset;
				}
				else
				{
					_isDragTracking = true;
					_rubberStartScreen = e.Location;
					_rubberCurrentScreen = e.Location;

					// 드래그 시작 시 현재 선택 상태를 캡처
					//_dragInitialSelection = new HashSet<GlSceneItem>(_selectedItems);// AllItems.Where(i => i.Selected));
				}
			}
			base.OnMouseDown(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (_isPanning)
			{
				float dx = (float)((e.X - _panStartScreen.X) / _scale);
				float dy = (float)((e.Y - _panStartScreen.Y) / _scale);
				_offset = new Vector2(_panStartOffset.X - dx, _panStartOffset.Y + dy);
				Invalidate();
			}
			else if (_isDragTracking)
			{
				_rubberCurrentScreen = e.Location;
				Invalidate();
			}

			var world = ScreenToWorld(e.Location);
			MouseWorldPositionChanged?.Invoke(this, new PointF((float)world.X, (float)world.Y));
			base.OnMouseMove(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (_isPanning)
			{
				_isPanning = false;
			}
			else if (_isDragTracking && e.Button == MouseButtons.Left)
			{
				_isDragTracking = false;
				// 선택 상태가 바뀐 Item만 VBO에 다시 전송한다.
				var changedItems = new List<GlSceneItem>();

				// 현재 러버밴드의 월드 박스 계산
				var wPt1 = ScreenToWorld(_rubberStartScreen);
				var wPt2 = ScreenToWorld(_rubberCurrentScreen);
				GBox selBox = new GBox(Math.Min(wPt1.X, wPt2.X), Math.Min(wPt1.Y, wPt2.Y), Math.Max(wPt1.X, wPt2.X), Math.Max(wPt1.Y, wPt2.Y));

				//if (_dragInitialSelection == null) _dragInitialSelection = new HashSet<GlSceneItem>();

				// 항목을 클릭한 경우 전체가 포함되지 않아도 선택 처리
				if (selBox.Width == 0 && selBox.Height == 0)
				{
					var list = new List<GlSceneItem>();

					foreach (var layer in _layerList)
					{
						if (!layer.Visible)
							continue;

						foreach (var it in layer.Items)
						{
							if (it.WorldBounds.IntersectsWith(selBox) && CheckSelectable(it))
								list.Add(it);
						}
					}

					if (list.Count > 0)
					{
						// 제일 작은 항목으로 선택
						var it = list.OrderBy(a => a.WorldBounds.Width == a.WorldBounds.Height).OrderBy(a => a.WorldBounds.Width).First();

						//it.Selected = !_dragInitialSelection.Contains(it);
						it.Selected = !ContainsSelectedItem(it);
						changedItems.Add(it);

						if (it.Selected)
							AppendSelectedItem(it);
						else
							RemoveSelectedItem(it);
					}
				}
				else // 드래그 하여 선택한 경우 완전 포함
				{
					foreach (var layer in _layerList)
					{
						if (!layer.Visible)
							continue;

						foreach (var it in layer.Items)
						{
							// 완전 포함(fully contained) 검사
							bool inside = IsBoxFullyContained(selBox, it.WorldBounds) && CheckSelectable(it);

							if (inside)
							{
								bool contains = ContainsSelectedItem(it);
								//bool desired = inside ? !_dragInitialSelection.Contains(it) : _dragInitialSelection.Contains(it);

								it.Selected = !contains;
								changedItems.Add(it);

								if (contains)
									RemoveSelectedItem(it);
								else
									AppendSelectedItem(it);
							}
						}
					}
				}

				// GPU에 반영 및 후처리
				UpdateSelectionVertices(changedItems);
				RaiseSelectionChanged();
				Invalidate();

				//_dragInitialSelection = null;
			}
			base.OnMouseUp(e);
		}

		private void AppendSelectedItem(GlSceneItem it)
		{
			var idx = _selectedItems.BinarySearch(it);
			_selectedItems.Insert(~idx, it);
		}

		private void RemoveSelectedItem(GlSceneItem it)
		{
			var idx = _selectedItems.BinarySearch(it);
			_selectedItems.RemoveAt(idx);
		}

		private bool ContainsSelectedItem(GlSceneItem it)
		{
			return _selectedItems.BinarySearch(it) >= 0;
		}

		private bool CheckSelectable(GlSceneItem item, int decimals = 1)
		{
			if (!OnlySelectSquareItems)
				return true;

			return Math.Round(item.WorldBounds.Width, decimals) == Math.Round(item.WorldBounds.Height, decimals);
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			ZoomAt(e.Location, e.Delta > 0 ? 1.15 : 1 / 1.15);
			base.OnMouseWheel(e);
		}

		private static bool IsBoxFullyContained(GBox container, GBox inner)
		{
			return container.MinX <= inner.MinX &&
				   container.MinY <= inner.MinY &&
				   container.MaxX >= inner.MaxX &&
				   container.MaxY >= inner.MaxY;
		}
	}
}
