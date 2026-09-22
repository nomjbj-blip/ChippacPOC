using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NexplantQMS.GdsMap
{
	/// <summary>
	/// 모든 Layer의 GDS TEXT를 도형 렌더링 뒤에 표시하는 화면 오버레이다.
	/// OpenGL 도형 버퍼와 분리하여 문자열 데이터가 GPU 버퍼를 불필요하게 늘리지 않도록 한다.
	/// </summary>
	public partial class GdsMapControl
	{
		private const float LabelMarginPixels = 6.0f;
		private const int LabelCollisionCellPixels = 64;
		// 인접 라벨 사이 간격의 일부만 글씨에 사용해 작은 도형에서 라벨이 과하게 커지지 않게 한다.
		private const float LabelSpaceUseRatio = 0.4f;
		private const float LabelBaseFontPixels = 10.0f;
		private const float LabelMinimumFontPixels = 6.0f;
		// 간격에 맞춘 크기 조정을 우선하고, 극단적인 줌에서만 최대 글꼴 크기를 제한한다.
		private const float LabelMaximumFontPixels = 64.0f;
		private readonly List<LayerTextLabel> _textLabels = new List<LayerTextLabel>();
		private double _labelFitScale = 1.0;
		public int LastVisibleTextLabelCount { get; private set; }

		/// <summary>
		/// 전체 보기에서 정한 배율을 라벨의 10px 기준으로 보관한다.
		/// 이후 줌 배율과의 비율로 글자를 키워 도형과 함께 확대되게 한다.
		/// </summary>
		private void SetTextLabelFitScale()
		{
			_labelFitScale = _scale > 0 && !Double.IsNaN(_scale) && !Double.IsInfinity(_scale) ? _scale : 1.0;
		}

		/// <summary>현재 줌 비율로 화면 글꼴 크기를 정하고 과도한 확대와 축소를 제한한다.</summary>
		private float GetTextLabelFontPixels()
		{
			double pixels = LabelBaseFontPixels * (_scale / _labelFitScale);
			if (Double.IsNaN(pixels) || Double.IsInfinity(pixels)) return LabelBaseFontPixels;
			return (float)Clamp(pixels, LabelMinimumFontPixels, LabelMaximumFontPixels);
		}

		/// <summary>TEXT의 실제 Layer 번호와 구조 변환 완료 좌표를 함께 보관한다.</summary>
		private void AddTextLabel(GdsText text, GPoint[] worldPoints, string structurePath)
		{
			if (String.IsNullOrWhiteSpace(text.Text) || worldPoints == null || worldPoints.Length == 0)
				return;
			_textLabels.Add(new LayerTextLabel(
				text.LayerID,
				text.Text,
				text.TextType,
				text.FontNumber,
				text.HorizontalPresentation,
				text.VerticalPresentation,
				text.Position,
				worldPoints[0],
				structurePath));
		}

		/// <summary>모든 Layer의 TEXT 원본 좌표와 변환 결과를 비교할 수 있는 진단 목록을 반환한다.</summary>
		public List<GdsTextLabelDiagnostic> GetTextLabelDiagnostics()
		{
			return _textLabels.Select(label => label.Diagnostic).ToList();
		}

		/// <summary>
		/// 각 Layer에서 실제로 같은 X/Y 좌표에 있는 TEXT만 묶어 인접 간격을 계산한다.
		/// GPoint가 소수 다섯째 자리까지 정규화되므로 셋째 자리 반올림은 서로 다른 행/열을 합쳐 버린다.
		/// </summary>
		private void CalculateTextLabelDisplayAreas()
		{
			foreach (var label in _textLabels)
			{
				label.AvailableWidth = 0;
				label.AvailableHeight = 0;
			}
			foreach (var row in _textLabels.GroupBy(label => new { label.LayerId, label.Position.Y }))
				AssignAvailableSpace(row.OrderBy(label => label.Position.X).ToList(), true);
			foreach (var column in _textLabels.GroupBy(label => new { label.LayerId, label.Position.X }))
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
		/// 현재 화면 안에 있고 해당 TEXT Layer가 표시 중인 Label만 GDI 오버레이로 그린다.
		/// 축소 상태에서는 문자 수가 많아도 반복 DrawString 비용이 발생하지 않도록 표시를 건너뛴다.
		/// </summary>
		private void DrawTextLabels()
		{
			LastVisibleTextLabelCount = 0;
			if (_textLabels.Count == 0)
				return;

			var visibleLayers = new HashSet<int>(_layerList.Where(layer => layer.Visible).Select(layer => layer.LayerID));
			if (visibleLayers.Count == 0)
				return;

			using (var graphics = CreateGraphics())
			using (var brush = new SolidBrush(Color.White))
			using (var maximumFont = new Font("Segoe UI", GetTextLabelFontPixels(), FontStyle.Regular, GraphicsUnit.Pixel))
			{
				graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
				var occupiedRegions = new Dictionary<long, List<RectangleF>>();
				var fittedFonts = new Dictionary<int, Font>();
				try
				{
					foreach (var label in _textLabels)
					{
						if (!visibleLayers.Contains(label.LayerId)) continue;
						// 이웃이 없는 방향은 화면 크기를 사용한다. 확대 글씨는 인접 간격의 40%에 맞춰 줄인다.
						PointF screen = WorldToScreen(label.Position);
						// 화면 밖 기준점은 최종 문자열도 화면 안에 완전히 들어올 수 없어 측정 전에 제외한다.
						if (screen.X < 0 || screen.Y < 0 || screen.X > ClientSize.Width || screen.Y > ClientSize.Height) continue;
						float allowedWidth = label.AvailableWidth > 0
							? (float)(label.AvailableWidth * _scale) - LabelMarginPixels
							: ClientSize.Width - LabelMarginPixels * 2;
						float allowedHeight = label.AvailableHeight > 0
							? (float)(label.AvailableHeight * _scale) - LabelMarginPixels
							: ClientSize.Height - LabelMarginPixels * 2;
						if (allowedWidth <= 0 || allowedHeight <= 0) continue;
						SizeF maximumSize = graphics.MeasureString(label.Text, maximumFont);
						float fitRatio = Math.Min(1f, Math.Min(
							allowedWidth * LabelSpaceUseRatio / maximumSize.Width,
							allowedHeight * LabelSpaceUseRatio / maximumSize.Height));
						int fontPixels = (int)Math.Floor(maximumFont.Size * fitRatio);
						if (fontPixels < LabelMinimumFontPixels) continue;
						if (!fittedFonts.TryGetValue(fontPixels, out var labelFont))
						{
							labelFont = new Font("Segoe UI", fontPixels, FontStyle.Regular, GraphicsUnit.Pixel);
							fittedFonts.Add(fontPixels, labelFont);
						}
						SizeF textSize = graphics.MeasureString(label.Text, labelFont);
						if (textSize.Width > allowedWidth || textSize.Height > allowedHeight) continue;
						PointF drawPoint = GetTextLabelDrawPoint(label, screen, textSize);
						var textBounds = new RectangleF(drawPoint.X, drawPoint.Y, textSize.Width, textSize.Height);
						if (textBounds.Left < 0 || textBounds.Top < 0 || textBounds.Right > ClientSize.Width || textBounds.Bottom > ClientSize.Height ||
							HasLabelCollision(textBounds, occupiedRegions))
							continue;
						graphics.DrawString(label.Text, labelFont, brush, drawPoint);
						ReserveLabelCells(textBounds, occupiedRegions);
						LastVisibleTextLabelCount++;
					}
				}
				finally
				{
					foreach (var font in fittedFonts.Values) font.Dispose();
				}
			}
		}

		/// <summary>
		/// GDS TEXT 삽입 좌표를 PRESENTATION에 지정된 문자 기준점으로 해석해 실제 그리기 좌표를 계산한다.
		/// 예를 들어 가로 Center는 삽입 X에서 문자 너비의 절반을 빼어 정중앙에 배치한다.
		/// </summary>
		private static PointF GetTextLabelDrawPoint(LayerTextLabel label, PointF anchor, SizeF textSize)
		{
			float x = anchor.X;
			if (label.HorizontalPresentation == GdsTextHorizontalPresentation.Center) x -= textSize.Width / 2.0f;
			else if (label.HorizontalPresentation == GdsTextHorizontalPresentation.Right) x -= textSize.Width;

			float y = anchor.Y;
			if (label.VerticalPresentation == GdsTextVerticalPresentation.Middle) y -= textSize.Height / 2.0f;
			else if (label.VerticalPresentation == GdsTextVerticalPresentation.Bottom) y -= textSize.Height;
			return new PointF(x, y);
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
			public int LayerId { get; private set; }
			public string Text { get; private set; }
			public GPoint Position { get; private set; }
			public GdsTextHorizontalPresentation HorizontalPresentation { get; private set; }
			public GdsTextVerticalPresentation VerticalPresentation { get; private set; }
			public double AvailableWidth { get; set; }
			public double AvailableHeight { get; set; }
			public GdsTextLabelDiagnostic Diagnostic { get; private set; }
			public LayerTextLabel(int layerId, string text, int textType, int fontNumber,
				GdsTextHorizontalPresentation horizontalPresentation, GdsTextVerticalPresentation verticalPresentation,
				GPoint rawPosition, GPoint position, string structurePath)
			{
				LayerId = layerId;
				Text = text;
				Position = position;
				HorizontalPresentation = horizontalPresentation;
				VerticalPresentation = verticalPresentation;
				Diagnostic = new GdsTextLabelDiagnostic(layerId, text, textType, fontNumber,
					horizontalPresentation.ToString(), verticalPresentation.ToString(), rawPosition, position, structurePath);
			}
		}

	/// <summary>GDS TEXT의 Layer 번호와 SREF 변환 전/후 좌표를 조회 전용으로 제공한다.</summary>
	public sealed class GdsTextLabelDiagnostic
	{
		public int LayerId { get; private set; }
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
		public GdsTextLabelDiagnostic(int layerId, string text, int textType, int fontNumber,
			string horizontalPresentation, string verticalPresentation,
			GPoint rawPosition, GPoint transformedPosition, string structurePath)
		{
			LayerId = layerId;
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
