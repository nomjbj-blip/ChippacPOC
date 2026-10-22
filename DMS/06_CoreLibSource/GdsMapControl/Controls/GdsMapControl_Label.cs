using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace NexplantQMS.GdsMap
{
	/// <summary>
	/// 모든 Layer의 GDS TEXT를 도형 렌더링 뒤에 표시하는 화면 오버레이다.
	/// OpenGL 도형 버퍼와 분리하여 문자열 데이터가 GPU 버퍼를 불필요하게 늘리지 않도록 한다.
	/// 글자 크기는 배율마다 화면 전체에서 하나다. Label 마다 이웃 거리로 크기를 바꾸면 빈 자리 옆 / 한 자리 여러 Label 에서 크기가 제각각이 되기 때문이다 (2026-10-20).
	/// 한 자리에 Label 이 여러 개 겹친 도면(U8: Bump 마다 핀 번호 + Net 이름, 같은 내용 2벌)은 같은 좌표 묶음(자리) 단위로 줄을 나눠 그린다 (2026-10-20).
	/// 설계: 문서/설계/02_GDS/2026-10-20_U8_Label_표시크기_설계.md
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
		// 보통 이름 폭을 잴 때 쓰는 이름 수와 글꼴 크기. 도면을 읽은 뒤 첫 그리기에서 한 번만 잰다 (2026-10-20)
		private const int LabelReferenceSampleCount = 32;
		private const float LabelMeasureFontPixels = 100.0f;
		// 번호형 이름(접두어_숫자, 예: C4_7444 / TSV_19144). 같은 자리에 이름과 함께 있으면 아래 줄에 둔다 (2026-10-20)
		private static readonly Regex LabelNumberNamePattern = new Regex(@"^[A-Za-z][A-Za-z0-9]*_\d+$", RegexOptions.Compiled);
		private readonly List<LayerTextLabel> _textLabels = new List<LayerTextLabel>();
		// 같은 좌표 Label 묶음(그리기 단위)과 Layer 키별 글자 크기 기준. CalculateTextLabelDisplayAreas 에서 만든다 (2026-10-20)
		private readonly List<TextLabelSpot> _textLabelSpots = new List<TextLabelSpot>();
		private readonly Dictionary<int, TextLabelKeyInfo> _textLabelKeyInfos = new Dictionary<int, TextLabelKeyInfo>();
		private int _textLabelLayoutCount = -1;
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
		/// 각 Layer 키(Layer 번호 + TEXTTYPE)에서 실제로 같은 X/Y 좌표에 있는 TEXT만 묶어 인접 간격을 계산한다.
		/// GPoint가 소수 다섯째 자리까지 정규화되므로 셋째 자리 반올림은 서로 다른 행/열을 합쳐 버린다.
		/// 화면에서 18/3 / 18/4 를 따로 켜고 끄므로 간격도 같은 키 안에서 구한다. 간격은 키별 대표 칸(중앙값)을 구하는 데 쓴다 (2026-10-20).
		/// 이어서 같은 내용 사본 표시 / 키별 글자 크기 기준 / 같은 좌표 묶음(자리)을 만든다 (2026-10-20).
		/// </summary>
		private void CalculateTextLabelDisplayAreas()
		{
			foreach (var label in _textLabels)
			{
				label.AvailableWidth = 0;
				label.AvailableHeight = 0;
				label.SkipDraw = false;
			}
			foreach (var row in _textLabels.GroupBy(label => new { label.LayerKey, label.Position.Y }))
				AssignAvailableSpace(row.OrderBy(label => label.Position.X).ToList(), true);
			foreach (var column in _textLabels.GroupBy(label => new { label.LayerKey, label.Position.X }))
				AssignAvailableSpace(column.OrderBy(label => label.Position.Y).ToList(), false);
			// 2026-10-07 결정 A: 한쪽 방향에만 이웃이 있으면 그 간격을 다른 방향 제한에도 쓴다.
			// 이웃이 없는 방향은 화면 크기가 제한이 되어 확대할수록 최대 글꼴(64px)까지 커지기 때문이다(예: C4DC_HBM0_N_FH).
			foreach (var label in _textLabels)
			{
				if (label.AvailableWidth <= 0 && label.AvailableHeight > 0) label.AvailableWidth = label.AvailableHeight;
				else if (label.AvailableHeight <= 0 && label.AvailableWidth > 0) label.AvailableHeight = label.AvailableWidth;
			}
			MarkDuplicateTextLabels();
			BuildTextLabelKeyInfos();
			BuildTextLabelSpots();
			_textLabelLayoutCount = _textLabels.Count;
		}

		/// <summary>
		/// 가장 가까운 양쪽 이웃 간격을 안전 영역으로 사용한다.
		/// 배열 경계의 TEXT는 이웃이 한쪽만 있어도 그 간격으로 표시 가능 여부를 판단한다.
		/// 같은 좌표 Label(한 자리 여러 Label / 같은 내용 2벌)은 건너뛰고 다른 좌표의 첫 이웃까지 본다.
		/// 바로 앞 / 뒤만 보면 같은 좌표가 3개 이상 이어질 때 가운데 Label 의 간격이 0(= 화면 크기)이 되었다 (2026-10-20).
		/// </summary>
		private static void AssignAvailableSpace(List<LayerTextLabel> labels, bool horizontal)
		{
			for (int index = 0; index < labels.Count; index++)
			{
				double current = horizontal ? labels[index].Position.X : labels[index].Position.Y;
				double available = Double.MaxValue;
				for (int previousIndex = index - 1; previousIndex >= 0; previousIndex--)
				{
					double previous = horizontal ? labels[previousIndex].Position.X : labels[previousIndex].Position.Y;
					if (current > previous)
					{
						available = Math.Min(available, current - previous);
						break;
					}
				}
				for (int nextIndex = index + 1; nextIndex < labels.Count; nextIndex++)
				{
					double next = horizontal ? labels[nextIndex].Position.X : labels[nextIndex].Position.Y;
					if (next > current)
					{
						available = Math.Min(available, next - current);
						break;
					}
				}
				if (available == Double.MaxValue) continue;
				if (horizontal) labels[index].AvailableWidth = available;
				else labels[index].AvailableHeight = available;
			}
		}

		/// <summary>
		/// 같은 Layer 키 / 글자 / 좌표의 두 번째 이후 Label 은 그리지 않는다. 목록(진단 / Chain 이름 찾기)에는 그대로 둔다.
		/// U8 은 제품 Cell 의 TEXT 와 같은 내용의 하위 Cell 이 (0, 0)에 한 번 더 배치되어 Label 이 2벌 펼쳐진다 (2026-10-20).
		/// </summary>
		private void MarkDuplicateTextLabels()
		{
			var seen = new HashSet<(int, string, double, double)>();
			foreach (var label in _textLabels)
				label.SkipDraw = !seen.Add((label.LayerKey, label.Text, label.Position.X, label.Position.Y));
		}

		/// <summary>
		/// Layer 키별 글자 크기 기준을 만든다.
		/// - 대표 칸: Label 칸 가로 / 세로의 중앙값(배열 피치). 빈 자리 옆처럼 넓은 칸이 크기를 키우지 않게 Label 하나의 칸 대신 쓴다.
		/// - 보통 이름: 이름 길이 중앙값의 이름들. 그리기 때 폭을 재서 대표 칸의 40% 에 들어가는 크기를 그 키의 글자 크기로 본다.
		/// </summary>
		private void BuildTextLabelKeyInfos()
		{
			_textLabelKeyInfos.Clear();
			foreach (var key in _textLabels.Where(label => !label.SkipDraw).GroupBy(label => label.LayerKey))
			{
				var lengths = key.Select(label => label.Text.Length).OrderBy(length => length).ToList();
				int typicalLength = lengths[lengths.Count / 2];
				_textLabelKeyInfos.Add(key.Key, new TextLabelKeyInfo
				{
					MedianWidth = MedianOfPositive(key.Select(label => label.AvailableWidth)),
					MedianHeight = MedianOfPositive(key.Select(label => label.AvailableHeight)),
					ReferenceTexts = key.Select(label => label.Text).Where(text => text.Length == typicalLength)
						.Distinct().Take(LabelReferenceSampleCount).ToList()
				});
			}
		}

		/// <summary>0 보다 큰 값의 중앙값. 값이 없으면 0.</summary>
		private static double MedianOfPositive(IEnumerable<double> values)
		{
			var sorted = values.Where(value => value > 0).OrderBy(value => value).ToList();
			return sorted.Count == 0 ? 0 : sorted[sorted.Count / 2];
		}

		/// <summary>
		/// 같은 좌표의 Label 을 그리기 단위(자리)로 묶는다. 자리 순서는 처음 나온 순서(파일 순서)다.
		/// 자리 안 순서: 이름 -> 번호형 이름(C4_7444 / TSV_19144) -> Layer 번호 -> TEXTTYPE 큰 쪽 -> 파일 순서. 그릴 때 이 순서로 줄을 나눈다(Net 위 / 핀 번호 아래).
		/// U8 Layer 18 은 C4 자리의 핀 번호와 Net 이름이 같은 TEXTTYPE(18/3)이라 TEXTTYPE 만으로는 줄 순서를 정할 수 없다.
		/// </summary>
		private void BuildTextLabelSpots()
		{
			_textLabelSpots.Clear();
			var spots = new Dictionary<(double, double), TextLabelSpot>();
			foreach (var label in _textLabels)
			{
				if (label.SkipDraw) continue;
				var position = (label.Position.X, label.Position.Y);
				if (!spots.TryGetValue(position, out var spot))
				{
					spot = new TextLabelSpot(label.Position);
					spots.Add(position, spot);
					_textLabelSpots.Add(spot);
				}
				spot.Labels.Add(label);
			}
			foreach (var spot in _textLabelSpots)
			{
				if (spot.Labels.Count > 1)
					spot.Labels = spot.Labels.OrderBy(label => LabelNumberNamePattern.IsMatch(label.Text))
						.ThenBy(label => label.LayerId).ThenByDescending(label => label.TextType).ToList();
			}
		}

		/// <summary>
		/// 현재 화면 안에 있고 해당 TEXT Layer가 표시 중인 Label만 GDI 오버레이로 그린다.
		/// 축소 상태에서는 문자 수가 많아도 반복 DrawString 비용이 발생하지 않도록 표시를 건너뛴다.
		/// 배치 계산(LayoutTextLabels)과 그리기를 나눠 Probe 에서 같은 계산을 비트맵으로 확인한다 (2026-10-20).
		/// </summary>
		private void DrawTextLabels()
		{
			LastVisibleTextLabelCount = 0;
			if (_textLabels.Count == 0)
				return;

			using (var graphics = CreateGraphics())
			using (var brush = new SolidBrush(Color.White))
			{
				graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
				var placements = LayoutTextLabels(graphics, ClientSize.Width, ClientSize.Height);
				var fonts = new Dictionary<int, Font>();
				try
				{
					foreach (var placement in placements)
					{
						if (!fonts.TryGetValue(placement.FontPixels, out var font))
						{
							font = new Font("Segoe UI", placement.FontPixels, FontStyle.Regular, GraphicsUnit.Pixel);
							fonts.Add(placement.FontPixels, font);
						}
						graphics.DrawString(placement.Text, font, brush, placement.Location);
					}
				}
				finally
				{
					foreach (var font in fonts.Values) font.Dispose();
				}
				LastVisibleTextLabelCount = placements.Count;
			}
		}

		/// <summary>
		/// 그릴 Label 의 글자 / 위치 / 크기를 계산한다(그리지는 않음).
		/// 처리 흐름:
		///  1) 글자 크기를 한 번 정한다 = 켜진 Label Layer 키마다 "대표 칸 40% 에 보통 이름이 들어가는 크기" 중 가장 작은 값,
		///     최대 글꼴(10px x 배율 비율, 64px) 이하. 6px 미만이면 아무것도 그리지 않는다(멀리서는 안 보임)
		///  2) 자리마다: 기준점이 화면 밖이면 제외 -> 켜진 Layer 키의 Label 만 모아 같은 글자는 한 번, 다른 글자는 줄을 나눈다(이름 위 / 번호 아래)
		///  3) 모든 자리를 같은 크기로 재서 PRESENTATION 기준점에 놓고, 화면 밖 / 이미 그린 글자와 겹치면 제외
		/// </summary>
		internal List<TextLabelPlacement> LayoutTextLabels(Graphics graphics, int clientWidth, int clientHeight)
		{
			var placements = new List<TextLabelPlacement>();
			if (_textLabels.Count == 0)
				return placements;
			if (_textLabelLayoutCount != _textLabels.Count)
				CalculateTextLabelDisplayAreas();

			// 표시 Layer 는 Layer 번호 + TEXTTYPE 키로 비교한다(KLayout 처럼 18/3 / 18/4 를 따로 켜고 끔) (2026-10-20)
			var visibleLayers = new HashSet<int>(_layerList.Where(layer => layer.Visible).Select(layer => layer.LayerKey));
			if (visibleLayers.Count == 0)
				return placements;

			double fontLimit = GetTextLabelFontPixels();
			foreach (int layerKey in visibleLayers)
			{
				if (_textLabelKeyInfos.TryGetValue(layerKey, out var info))
					fontLimit = Math.Min(fontLimit, GetKeyFontPixels(info, graphics));
			}
			if (Double.IsNaN(fontLimit) || fontLimit < LabelMinimumFontPixels)
				return placements;
			int fontPixels = (int)Math.Floor(fontLimit);

			var occupiedRegions = new Dictionary<long, List<RectangleF>>();
			var lines = new List<string>();
			using (var labelFont = new Font("Segoe UI", fontPixels, FontStyle.Regular, GraphicsUnit.Pixel))
			{
				foreach (var spot in _textLabelSpots)
				{
					PointF screen = WorldToScreen(spot.Position);
					// 화면 밖 기준점은 최종 문자열도 화면 안에 완전히 들어올 수 없어 측정 전에 제외한다.
					if (screen.X < 0 || screen.Y < 0 || screen.X > clientWidth || screen.Y > clientHeight) continue;

					LayerTextLabel first = null;
					lines.Clear();
					foreach (var label in spot.Labels)
					{
						if (!visibleLayers.Contains(label.LayerKey)) continue;
						if (first == null) first = label;
						if (!lines.Contains(label.Text)) lines.Add(label.Text);
					}
					if (first == null) continue;
					string text = lines.Count == 1 ? lines[0] : String.Join("\n", lines);

					SizeF textSize = graphics.MeasureString(text, labelFont);
					PointF drawPoint = GetTextLabelDrawPoint(first, screen, textSize);
					var textBounds = new RectangleF(drawPoint.X, drawPoint.Y, textSize.Width, textSize.Height);
					if (textBounds.Left < 0 || textBounds.Top < 0 || textBounds.Right > clientWidth || textBounds.Bottom > clientHeight ||
						HasLabelCollision(textBounds, occupiedRegions))
						continue;
					placements.Add(new TextLabelPlacement(text, drawPoint, textSize, fontPixels));
					ReserveLabelCells(textBounds, occupiedRegions);
				}
			}
			return placements;
		}

		/// <summary>
		/// 키 하나의 이번 배율 글자 크기(px) = 보통 이름이 대표 칸 가로 / 세로의 40% 에 들어가는 크기.
		/// 보통 이름 폭은 도면을 읽은 뒤 처음 한 번 100px 글꼴로 재서 중앙값을 쓴다. 대표 칸이 없으면(이웃 없음) 제한하지 않는다.
		/// </summary>
		private double GetKeyFontPixels(TextLabelKeyInfo info, Graphics graphics)
		{
			if (info.LineHeight <= 0)
			{
				using (var measureFont = new Font("Segoe UI", LabelMeasureFontPixels, FontStyle.Regular, GraphicsUnit.Pixel))
				{
					var widths = info.ReferenceTexts.Select(text => (double)graphics.MeasureString(text, measureFont).Width).OrderBy(width => width).ToList();
					info.ReferenceWidth = widths.Count == 0 ? 0 : widths[widths.Count / 2];
					info.LineHeight = graphics.MeasureString("Ag", measureFont).Height;
				}
			}
			double pixels = Double.MaxValue;
			if (info.MedianWidth > 0 && info.ReferenceWidth > 0)
				pixels = Math.Min(pixels, ((info.MedianWidth * _scale) - LabelMarginPixels) * LabelSpaceUseRatio * LabelMeasureFontPixels / info.ReferenceWidth);
			if (info.MedianHeight > 0 && info.LineHeight > 0)
				pixels = Math.Min(pixels, ((info.MedianHeight * _scale) - LabelMarginPixels) * LabelSpaceUseRatio * LabelMeasureFontPixels / info.LineHeight);
			return pixels;
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

		/// <summary>화면에 그릴 Label 하나(자리 묶음)의 글자 / 위치 / 크기 (2026-10-20).</summary>
		internal sealed class TextLabelPlacement
		{
			public TextLabelPlacement(string text, PointF location, SizeF size, int fontPixels)
			{
				Text = text;
				Location = location;
				Size = size;
				FontPixels = fontPixels;
			}
			public string Text { get; private set; }
			public PointF Location { get; private set; }
			public SizeF Size { get; private set; }
			public int FontPixels { get; private set; }
		}

		/// <summary>같은 좌표의 Label 묶음(그리기 단위) (2026-10-20).</summary>
		private sealed class TextLabelSpot
		{
			public TextLabelSpot(GPoint position)
			{
				Position = position;
				Labels = new List<LayerTextLabel>();
			}
			public GPoint Position { get; private set; }
			public List<LayerTextLabel> Labels { get; set; }
		}

		/// <summary>Layer 키의 글자 크기 기준(대표 칸 / 보통 이름 폭) (2026-10-20).</summary>
		private sealed class TextLabelKeyInfo
		{
			public double MedianWidth { get; set; }
			public double MedianHeight { get; set; }
			public List<string> ReferenceTexts { get; set; }
			public double ReferenceWidth { get; set; }
			public double LineHeight { get; set; }
		}

		/// <summary>GDS 변환이 반영된 TEXT와 기준 좌표를 한 쌍으로 보관한다.</summary>
		private sealed class LayerTextLabel
		{
			public int LayerId { get; private set; }
			// 화면 Layer 키(Layer 번호 + TEXTTYPE)와 TEXTTYPE. 간격 계산 / 표시 여부 / 자리 안 순서에 쓴다 (2026-10-20)
			public int LayerKey { get; private set; }
			public int TextType { get; private set; }
			public string Text { get; private set; }
			public GPoint Position { get; private set; }
			public GdsTextHorizontalPresentation HorizontalPresentation { get; private set; }
			public GdsTextVerticalPresentation VerticalPresentation { get; private set; }
			public double AvailableWidth { get; set; }
			public double AvailableHeight { get; set; }
			// 같은 키 / 글자 / 좌표의 두 번째 이후 사본이면 그리지 않는다 (2026-10-20)
			public bool SkipDraw { get; set; }
			public GdsTextLabelDiagnostic Diagnostic { get; private set; }
			public LayerTextLabel(int layerId, string text, int textType, int fontNumber,
				GdsTextHorizontalPresentation horizontalPresentation, GdsTextVerticalPresentation verticalPresentation,
				GPoint rawPosition, GPoint position, string structurePath)
			{
				LayerId = layerId;
				LayerKey = GdsLayerKey.Make(layerId, textType);
				TextType = textType;
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
