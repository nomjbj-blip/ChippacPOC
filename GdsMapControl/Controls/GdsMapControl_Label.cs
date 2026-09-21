using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace NexplantQMS.GdsMap
{
	/// <summary>
	/// Layer 775의 GDS TEXT를 도형 렌더링 뒤에 표시하는 화면 오버레이다.
	/// OpenGL 도형 버퍼와 분리하여 문자열 데이터가 GPU 버퍼를 불필요하게 늘리지 않도록 한다.
	/// </summary>
	public partial class GdsMapControl
	{
		private const int LabelLayerId = 775;
		private const float LabelMarginPixels = 6.0f;
		private const int LabelCollisionCellPixels = 64;
		private const float LabelMinimumSpaceMultiplier = 3.0f;
		private const int LabelOwnerGridSize = 32;
		private const int LabelOwnerMaxCellCount = 256;
		private const double LabelMaximumOwnerDistanceRatio = 0.40;
		private readonly List<LayerTextLabel> _layer775Labels = new List<LayerTextLabel>();
		private readonly Font _layerLabelFont = new Font("Segoe UI", 10.0f, FontStyle.Regular, GraphicsUnit.Pixel);
		public int LastVisibleLayer775LabelCount { get; private set; }

		/// <summary>Layer 775 TEXT만 변환 완료 좌표와 함께 보관한다.</summary>
		private void AddLayer775Label(GdsText text, GPoint[] worldPoints, string structurePath)
		{
			if (text.LayerID != LabelLayerId || String.IsNullOrWhiteSpace(text.Text) || worldPoints == null || worldPoints.Length == 0)
				return;
			_layer775Labels.Add(new LayerTextLabel(
				text.Text,
				text.TextType,
				text.FontNumber,
				text.HorizontalPresentation,
				text.VerticalPresentation,
				text.Position,
				worldPoints[0],
				structurePath));
		}

		/// <summary>위치 보정 없이 원본 TEXT와 변환 결과를 비교할 수 있도록 Layer 775 진단 목록을 반환한다.</summary>
		public List<Layer775LabelDiagnostic> GetLayer775LabelDiagnostics()
		{
			return _layer775Labels.Select(label => label.Diagnostic).ToList();
		}

		/// <summary>
		/// Label 기준점과 가장 가까운 비TEXT 도형을 찾는다. GDS TEXT는 도형 사이 기준점일 수 있으므로 내부 포함 조건은 사용하지 않는다.
		/// 대형 배경 도형은 Label 영역을 과도하게 넓히지 않도록 인덱스에서 제외한다.
		/// </summary>
		private void BuildLayer775LabelOwners()
		{
			foreach (var label in _layer775Labels) label.ClearOwner();
			if (_layer775Labels.Count == 0) return;

			GBox allGeometryBounds = GBox.Empty;
			foreach (var item in _layerList.SelectMany(layer => layer.Items).Where(item => !(item.Source is GdsText)))
				allGeometryBounds.Include(item.WorldBounds);
			if (allGeometryBounds.IsEmpty || allGeometryBounds.Width <= 0 || allGeometryBounds.Height <= 0) return;

			var index = new Dictionary<long, List<GlSceneItem>>();
			foreach (var item in _layerList.SelectMany(layer => layer.Items).Where(item => !(item.Source is GdsText)))
			{
				int minX = GetOwnerCell(item.WorldBounds.MinX, allGeometryBounds.MinX, allGeometryBounds.Width);
				int maxX = GetOwnerCell(item.WorldBounds.MaxX, allGeometryBounds.MinX, allGeometryBounds.Width);
				int minY = GetOwnerCell(item.WorldBounds.MinY, allGeometryBounds.MinY, allGeometryBounds.Height);
				int maxY = GetOwnerCell(item.WorldBounds.MaxY, allGeometryBounds.MinY, allGeometryBounds.Height);
				if ((maxX - minX + 1) * (maxY - minY + 1) > LabelOwnerMaxCellCount) continue;
				for (int x = minX; x <= maxX; x++)
					for (int y = minY; y <= maxY; y++)
					{
						long key = GetOwnerCellKey(x, y);
						if (!index.TryGetValue(key, out var items)) { items = new List<GlSceneItem>(); index.Add(key, items); }
						items.Add(item);
					}
			}

			foreach (var label in _layer775Labels)
			{
				int x = GetOwnerCell(label.Position.X, allGeometryBounds.MinX, allGeometryBounds.Width);
				int y = GetOwnerCell(label.Position.Y, allGeometryBounds.MinY, allGeometryBounds.Height);
				var candidates = new List<GlSceneItem>();
				for (int cellX = Math.Max(0, x - 1); cellX <= Math.Min(LabelOwnerGridSize - 1, x + 1); cellX++)
					for (int cellY = Math.Max(0, y - 1); cellY <= Math.Min(LabelOwnerGridSize - 1, y + 1); cellY++)
						if (index.TryGetValue(GetOwnerCellKey(cellX, cellY), out var items)) candidates.AddRange(items);
				if (candidates.Count == 0) continue;
				GlSceneItem owner = null;
				double ownerDistance = Double.MaxValue;
				foreach (var item in candidates)
				{
					double distance = GetDistanceToBounds(item.WorldBounds, label.Position);
					if (distance < ownerDistance || (distance == ownerDistance && owner != null && GetArea(item.WorldBounds) < GetArea(owner.WorldBounds)))
					{
						owner = item;
						ownerDistance = distance;
					}
				}
				if (owner != null) label.SetOwner(owner.LayerID, owner.WorldBounds, ownerDistance);
			}
		}

		/// <summary>월드 좌표를 Label 도형 검색용 격자 번호로 바꾼다.</summary>
		private static int GetOwnerCell(double value, double minimum, double length)
		{
			return Math.Max(0, Math.Min(LabelOwnerGridSize - 1, (int)((value - minimum) / length * LabelOwnerGridSize)));
		}

		private static long GetOwnerCellKey(int x, int y) { return ((long)x << 32) ^ (uint)y; }
		private static double GetDistanceToBounds(GBox bounds, GPoint point)
		{
			double dx = point.X < bounds.MinX ? bounds.MinX - point.X : point.X > bounds.MaxX ? point.X - bounds.MaxX : 0;
			double dy = point.Y < bounds.MinY ? bounds.MinY - point.Y : point.Y > bounds.MaxY ? point.Y - bounds.MaxY : 0;
			return Math.Sqrt(dx * dx + dy * dy);
		}
		private static double GetArea(GBox bounds) { return bounds.Width * bounds.Height; }

		/// <summary>같은 X/Y 축의 인접 TEXT 간격으로 Label별 안전 표시 영역을 계산한다.</summary>
		private void CalculateLayer775LabelDisplayAreas()
		{
			foreach (var label in _layer775Labels)
			{
				label.AvailableWidth = 0;
				label.AvailableHeight = 0;
			}
			foreach (var row in _layer775Labels.GroupBy(label => Math.Round(label.Position.Y, 3)))
				AssignAvailableSpace(row.OrderBy(label => label.Position.X).ToList(), true);
			foreach (var column in _layer775Labels.GroupBy(label => Math.Round(label.Position.X, 3)))
				AssignAvailableSpace(column.OrderBy(label => label.Position.Y).ToList(), false);
		}

		/// <summary>
		/// 가장 가까운 양쪽 이웃 간격을 안전 영역으로 사용한다.
		/// 배열 경계의 TEXT는 이웃이 한쪽만 있어도 그 간격으로 표시 가능 여부를 판단한다.
		/// </summary>
		private static void AssignAvailableSpace(List<LayerTextLabel> labels, bool horizontal)
		{
			for (int index = 0; index < labels.Count; index++)
			{
				double current = horizontal ? labels[index].Position.X : labels[index].Position.Y;
				double available = Double.MaxValue;
				if (index > 0)
				{
					double previous = horizontal ? labels[index - 1].Position.X : labels[index - 1].Position.Y;
					if (current > previous) available = Math.Min(available, current - previous);
				}
				if (index < labels.Count - 1)
				{
					double next = horizontal ? labels[index + 1].Position.X : labels[index + 1].Position.Y;
					if (next > current) available = Math.Min(available, next - current);
				}
				if (available == Double.MaxValue) continue;
				if (horizontal) labels[index].AvailableWidth = available;
				else labels[index].AvailableHeight = available;
			}
		}

		/// <summary>
		/// 현재 화면 안에 있고 Layer 775가 표시 중인 Label만 GDI 오버레이로 그린다.
		/// 축소 상태에서는 문자 수가 많아도 반복 DrawString 비용이 발생하지 않도록 표시를 건너뛴다.
		/// </summary>
		private void DrawLayer775Labels()
		{
			LastVisibleLayer775LabelCount = 0;
			if (_layer775Labels.Count == 0)
				return;

			var layer = _layerList.GetLayer(LabelLayerId);
			if (layer == null || !layer.Visible)
				return;

			using (var graphics = CreateGraphics())
			using (var brush = new SolidBrush(Color.White))
			{
				graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
				var occupiedRegions = new Dictionary<long, List<RectangleF>>();
				foreach (var label in _layer775Labels)
				{
					// TEXT는 독립적인 GDS 요소이므로 특정 도형과의 거리로 숨기지 않고 실제 문자 공간으로만 판단한다.
					if (label.AvailableWidth <= 0 || label.AvailableHeight <= 0) continue;
					PointF screen = WorldToScreen(label.Position);
					SizeF textSize = graphics.MeasureString(label.Text, _layerLabelFont);
					float allowedWidth = (float)(label.AvailableWidth * _scale) - LabelMarginPixels;
					float allowedHeight = (float)(label.AvailableHeight * _scale) - LabelMarginPixels;
					PointF drawPoint = GetLayer775LabelDrawPoint(label, screen, textSize);
					var textBounds = new RectangleF(drawPoint.X, drawPoint.Y, textSize.Width, textSize.Height);
					var safeBounds = new RectangleF(screen.X - allowedWidth / 2, screen.Y - allowedHeight / 2, allowedWidth, allowedHeight);
					if (allowedWidth < textSize.Width * LabelMinimumSpaceMultiplier || allowedHeight < textSize.Height * LabelMinimumSpaceMultiplier ||
						textBounds.Left < 0 || textBounds.Top < 0 || textBounds.Right > ClientSize.Width || textBounds.Bottom > ClientSize.Height ||
						!safeBounds.Contains(textBounds) || HasLabelCollision(textBounds, occupiedRegions))
						continue;
					graphics.DrawString(label.Text, _layerLabelFont, brush, drawPoint);
					ReserveLabelCells(textBounds, occupiedRegions);
					LastVisibleLayer775LabelCount++;
				}
			}
		}

		/// <summary>
		/// GDS TEXT 삽입 좌표를 PRESENTATION에 지정된 문자 기준점으로 해석해 실제 그리기 좌표를 계산한다.
		/// 예를 들어 가로 Center는 삽입 X에서 문자 너비의 절반을 빼어 정중앙에 배치한다.
		/// </summary>
		private static PointF GetLayer775LabelDrawPoint(LayerTextLabel label, PointF anchor, SizeF textSize)
		{
			float x = anchor.X;
			if (label.HorizontalPresentation == GdsTextHorizontalPresentation.Center) x -= textSize.Width / 2.0f;
			else if (label.HorizontalPresentation == GdsTextHorizontalPresentation.Right) x -= textSize.Width;

			float y = anchor.Y;
			if (label.VerticalPresentation == GdsTextVerticalPresentation.Middle) y -= textSize.Height / 2.0f;
			else if (label.VerticalPresentation == GdsTextVerticalPresentation.Bottom) y -= textSize.Height;
			return new PointF(x, y);
		}

		/// <summary>Label이 속한 도형 레이어가 숨겨지면 Label도 숨긴다.</summary>
		private bool IsOwnerLayerVisible(int layerId)
		{
			var layer = _layerList.FirstOrDefault(item => item.LayerID == layerId);
			return layer != null && layer.Visible;
		}

		/// <summary>인접 화면 격자까지 실제 문자열 사각형을 비교해 겹침을 정확히 차단한다.</summary>
		private static bool HasLabelCollision(RectangleF bounds, Dictionary<long, List<RectangleF>> occupiedRegions)
		{
			foreach (long cell in GetLabelCells(bounds, true))
				if (occupiedRegions.TryGetValue(cell, out var regions))
					foreach (var region in regions)
						if (region.IntersectsWith(bounds)) return true;
			return false;
		}

		/// <summary>표시 확정 Label의 화면 점유 격자를 기록한다.</summary>
		private static void ReserveLabelCells(RectangleF bounds, Dictionary<long, List<RectangleF>> occupiedRegions)
		{
			foreach (long cell in GetLabelCells(bounds, false))
			{
				if (!occupiedRegions.TryGetValue(cell, out var regions))
				{
					regions = new List<RectangleF>();
					occupiedRegions.Add(cell, regions);
				}
				regions.Add(bounds);
			}
		}

		/// <summary>문자 영역이 차지하는 화면 격자 키를 만든다.</summary>
		private static IEnumerable<long> GetLabelCells(RectangleF bounds, bool includeNeighborCells)
		{
			int minX = (int)Math.Floor(bounds.Left / LabelCollisionCellPixels);
			int maxX = (int)Math.Floor((bounds.Right - 1) / LabelCollisionCellPixels);
			int minY = (int)Math.Floor(bounds.Top / LabelCollisionCellPixels);
			int maxY = (int)Math.Floor((bounds.Bottom - 1) / LabelCollisionCellPixels);
			if (includeNeighborCells) { minX--; maxX++; minY--; maxY++; }
			for (int x = minX; x <= maxX; x++)
				for (int y = minY; y <= maxY; y++)
					yield return ((long)x << 32) ^ (uint)y;
		}

		/// <summary>GDS 변환이 반영된 TEXT와 기준 좌표를 한 쌍으로 보관한다.</summary>
		private sealed class LayerTextLabel
		{
			public string Text { get; private set; }
			public GPoint Position { get; private set; }
			public GdsTextHorizontalPresentation HorizontalPresentation { get; private set; }
			public GdsTextVerticalPresentation VerticalPresentation { get; private set; }
			public double AvailableWidth { get; set; }
			public double AvailableHeight { get; set; }
			public bool HasOwner { get; private set; }
			public int OwnerLayerId { get; private set; }
			public GBox OwnerBounds { get; private set; }
			public double OwnerDistance { get; private set; }
			public Layer775LabelDiagnostic Diagnostic { get; private set; }
			public LayerTextLabel(string text, int textType, int fontNumber,
				GdsTextHorizontalPresentation horizontalPresentation, GdsTextVerticalPresentation verticalPresentation,
				GPoint rawPosition, GPoint position, string structurePath)
			{
				Text = text;
				Position = position;
				HorizontalPresentation = horizontalPresentation;
				VerticalPresentation = verticalPresentation;
				Diagnostic = new Layer775LabelDiagnostic(text, textType, fontNumber,
					horizontalPresentation.ToString(), verticalPresentation.ToString(), rawPosition, position, structurePath);
			}
			public void ClearOwner() { HasOwner = false; OwnerLayerId = 0; OwnerBounds = GBox.Empty; OwnerDistance = Double.MaxValue; }
			public void SetOwner(int layerId, GBox bounds, double distance) { HasOwner = true; OwnerLayerId = layerId; OwnerBounds = bounds; OwnerDistance = distance; }
		}

	/// <summary>Layer 775 TEXT의 원본 좌표와 SREF 변환 후 좌표를 조회 전용으로 제공한다.</summary>
	public sealed class Layer775LabelDiagnostic
	{
		public string Text { get; private set; }
		public int TextType { get; private set; }
		public int FontNumber { get; private set; }
		public string HorizontalPresentation { get; private set; }
		public string VerticalPresentation { get; private set; }
		public double RawX { get; private set; }
		public double RawY { get; private set; }
		public double TransformedX { get; private set; }
		public double TransformedY { get; private set; }
		public string StructurePath { get; private set; }
		public Layer775LabelDiagnostic(string text, int textType, int fontNumber,
			string horizontalPresentation, string verticalPresentation,
			GPoint rawPosition, GPoint transformedPosition, string structurePath)
		{
			Text = text;
			TextType = textType;
			FontNumber = fontNumber;
			HorizontalPresentation = horizontalPresentation;
			VerticalPresentation = verticalPresentation;
			RawX = rawPosition.X;
			RawY = rawPosition.Y;
			TransformedX = transformedPosition.X;
			TransformedY = transformedPosition.Y;
			StructurePath = structurePath;
		}
	}
}
}
