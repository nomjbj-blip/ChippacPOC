using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Collections.Generic;
using System.Linq;
using NexplantQMS.GdsMap.Chain;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// Chain 설정 중 지정한 Input, Output, 작업 영역을 GDS 도형 위에 표시하는 화면 Overlay다.
    /// 월드 좌표만 보관하고 그릴 때마다 현재 Zoom과 Offset을 적용하여 화면 위치를 다시 계산한다.
    /// </summary>
    public partial class GdsMapControl
    {
        private GPoint? _chainOverlayInputPoint;
        private GPoint? _chainOverlayOutputPoint;
        private IList<GPoint> _chainOverlayInputPoints = new List<GPoint>();
        private IList<GPoint> _chainOverlayOutputPoints = new List<GPoint>();
        private IList<ChainTraceElement> _chainOverlayOverlaps = new List<ChainTraceElement>();
        private IList<ChainTraceElement> _chainOverlayBranches = new List<ChainTraceElement>();
        private GPoint? _chainOverlayWorkAreaFirstPoint;
        private GPoint? _chainOverlayWorkAreaSecondPoint;

        /// <summary>Input 월드 좌표를 저장하고 이전 후보 경로를 해제한 뒤 화면을 다시 그린다.</summary>
        public void SetChainInputPoint(GPoint point)
        {
            _chainOverlayInputPoint = point;
            ClearChainCandidate();
            Invalidate();
        }

        /// <summary>Output 월드 좌표를 저장하고 이전 후보 경로를 해제한 뒤 화면을 다시 그린다.</summary>
        public void SetChainOutputPoint(GPoint point)
        {
            _chainOverlayOutputPoint = point;
            ClearChainCandidate();
            Invalidate();
        }

        /// <summary>
        /// 마우스로 고른 여러 Input/Output의 표시 위치를 한 번에 갱신한다.
        /// 선택 중에는 확정 경로 데이터를 지우지 않고 표시만 바꾼다.
        /// </summary>
        public void SetChainEndpointPoints(IEnumerable<GPoint> inputPoints, IEnumerable<GPoint> outputPoints)
        {
            _chainOverlayInputPoint = null;
            _chainOverlayOutputPoint = null;
            _chainOverlayInputPoints = (inputPoints ?? Enumerable.Empty<GPoint>()).ToList();
            _chainOverlayOutputPoints = (outputPoints ?? Enumerable.Empty<GPoint>()).ToList();
            Invalidate();
        }

        /// <summary>경로와 직접 겹친 도형 및 그중 분기 검토 대상을 지도 외곽선 표시용으로 보관한다.</summary>
        private void SetChainCandidateOverlay(ChainTraceResult result, ISet<string> includedKeys)
        {
            _chainOverlayOverlaps = result.OverlappingElements
                .Where(element => includedKeys.Contains(element.ElementKey)).ToList();
            _chainOverlayBranches = result.BranchCandidates
                .Where(element => includedKeys.Contains(element.ElementKey)).ToList();
        }

        /// <summary>단자나 Layer 조건 변경 시 이전 후보의 구분 외곽선을 제거한다.</summary>
        private void ClearChainCandidateOverlay()
        {
            _chainOverlayOverlaps = new List<ChainTraceElement>();
            _chainOverlayBranches = new List<ChainTraceElement>();
        }

        /// <summary>
        /// 작업 영역의 두 모서리를 저장한다. 두 번째 모서리가 없으면 첫 모서리 위치만 표시한다.
        /// 영역이 바뀌면 기존 후보 경로는 더 이상 유효하지 않으므로 강조를 해제한다.
        /// </summary>
        public void SetChainWorkAreaPoints(GPoint? firstPoint, GPoint? secondPoint)
        {
            _chainOverlayWorkAreaFirstPoint = firstPoint;
            _chainOverlayWorkAreaSecondPoint = secondPoint;
            ClearChainCandidate();
            Invalidate();
        }

        /// <summary>새 GDS 조회 시 이전 도면에 설정한 Chain Overlay와 후보 경로를 모두 제거한다.</summary>
        public void ResetChainSetupOverlay()
        {
            _chainOverlayInputPoint = null;
            _chainOverlayOutputPoint = null;
            _chainOverlayInputPoints.Clear();
            _chainOverlayOutputPoints.Clear();
            _chainOverlayWorkAreaFirstPoint = null;
            _chainOverlayWorkAreaSecondPoint = null;
            ClearChainCandidate();
            Invalidate();
        }

        /// <summary>
        /// OpenGL 도형과 GDS TEXT가 그려진 뒤 Chain 설정 상태를 가장 위에 표시한다.
        /// Marker는 확대 배율과 관계없이 일정한 픽셀 크기로 표시하여 축소 화면에서도 찾을 수 있게 한다.
        /// </summary>
        private void DrawChainSetupOverlay()
        {
            if (!_chainOverlayInputPoint.HasValue
                && !_chainOverlayOutputPoint.HasValue
                && _chainOverlayInputPoints.Count == 0
                && _chainOverlayOutputPoints.Count == 0
                && _chainOverlayOverlaps.Count == 0
                && _chainOverlayBranches.Count == 0
                && !_chainOverlayWorkAreaFirstPoint.HasValue)
                return;

            using (Graphics graphics = CreateGraphics())
            using (var markerFont = new Font("Segoe UI", 9f, FontStyle.Bold, GraphicsUnit.Pixel))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                DrawChainWorkArea(graphics);
                foreach (ChainTraceElement element in _chainOverlayOverlaps)
                    DrawChainElementOutline(graphics, element, Color.DeepSkyBlue, DashStyle.Solid);
                foreach (ChainTraceElement element in _chainOverlayBranches)
                    DrawChainElementOutline(graphics, element, Color.OrangeRed, DashStyle.Dash);
                if (_chainOverlayInputPoint.HasValue)
                    DrawChainPointMarker(graphics, _chainOverlayInputPoint.Value, "IN", Color.LimeGreen, markerFont);
                if (_chainOverlayOutputPoint.HasValue)
                    DrawChainPointMarker(graphics, _chainOverlayOutputPoint.Value, "OUT", Color.OrangeRed, markerFont);
                foreach (GPoint point in _chainOverlayInputPoints)
                    DrawChainPointMarker(graphics, point, "IN", Color.LimeGreen, markerFont);
                foreach (GPoint point in _chainOverlayOutputPoints)
                    DrawChainPointMarker(graphics, point, "OUT", Color.OrangeRed, markerFont);
            }
        }

        /// <summary>실제 다각형 또는 PATH 중심선을 화면 좌표로 그려 겹침과 분기를 색으로 구분한다.</summary>
        private void DrawChainElementOutline(Graphics graphics, ChainTraceElement element,
            Color color, DashStyle dashStyle)
        {
            if (element.WorldPoints == null || element.WorldPoints.Length == 0)
                return;
            PointF cornerA = WorldToScreen(new GPoint(element.Bounds.MinX, element.Bounds.MinY));
            PointF cornerB = WorldToScreen(new GPoint(element.Bounds.MaxX, element.Bounds.MaxY));
            var screenBounds = RectangleF.FromLTRB(Math.Min(cornerA.X, cornerB.X),
                Math.Min(cornerA.Y, cornerB.Y), Math.Max(cornerA.X, cornerB.X),
                Math.Max(cornerA.Y, cornerB.Y));
            if (!screenBounds.IntersectsWith(ClientRectangle))
                return;

            PointF[] points = element.WorldPoints.Select(WorldToScreen).ToArray();
            using (var pen = new Pen(color, 2.5f) { DashStyle = dashStyle })
            {
                if (points.Length == 1)
                    graphics.DrawEllipse(pen, points[0].X - 3, points[0].Y - 3, 6, 6);
                else if (string.Equals(element.ElementType, "PATH", StringComparison.OrdinalIgnoreCase))
                    graphics.DrawLines(pen, points);
                else if (points.Length >= 3)
                    graphics.DrawPolygon(pen, points);
                else
                    graphics.DrawLines(pen, points);
            }
        }

        /// <summary>두 모서리가 있으면 작업 영역 사각형을 그리고, 하나만 있으면 지정된 모서리만 표시한다.</summary>
        private void DrawChainWorkArea(Graphics graphics)
        {
            if (!_chainOverlayWorkAreaFirstPoint.HasValue)
                return;

            PointF first = WorldToScreen(_chainOverlayWorkAreaFirstPoint.Value);
            if (!_chainOverlayWorkAreaSecondPoint.HasValue)
            {
                DrawWorkAreaCorner(graphics, first);
                return;
            }

            PointF second = WorldToScreen(_chainOverlayWorkAreaSecondPoint.Value);
            var rectangle = RectangleF.FromLTRB(
                Math.Min(first.X, second.X),
                Math.Min(first.Y, second.Y),
                Math.Max(first.X, second.X),
                Math.Max(first.Y, second.Y));

            using (var fill = new SolidBrush(Color.FromArgb(24, Color.DeepSkyBlue)))
            using (var pen = new Pen(Color.DeepSkyBlue, 2f) { DashStyle = DashStyle.Dash })
            {
                graphics.FillRectangle(fill, rectangle);
                graphics.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            }

            DrawWorkAreaCorner(graphics, first);
            DrawWorkAreaCorner(graphics, second);
        }

        /// <summary>작업 영역 모서리를 확대 상태에서도 확인할 수 있는 작은 사각형으로 표시한다.</summary>
        private static void DrawWorkAreaCorner(Graphics graphics, PointF point)
        {
            const float size = 8f;
            using (var fill = new SolidBrush(Color.DeepSkyBlue))
            using (var pen = new Pen(Color.White, 1f))
            {
                var rectangle = new RectangleF(point.X - size / 2f, point.Y - size / 2f, size, size);
                graphics.FillRectangle(fill, rectangle);
                graphics.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            }
        }

        /// <summary>Input과 Output을 색상 원, 흰색 외곽선, 짧은 문자로 표시한다.</summary>
        private void DrawChainPointMarker(Graphics graphics, GPoint worldPoint, string text, Color color, Font font)
        {
            PointF screen = WorldToScreen(worldPoint);
            const float radius = 13f;
            var circle = new RectangleF(screen.X - radius, screen.Y - radius, radius * 2f, radius * 2f);

            using (var fill = new SolidBrush(Color.FromArgb(220, color)))
            using (var border = new Pen(Color.White, 2f))
            using (var textBrush = new SolidBrush(Color.White))
            {
                graphics.FillEllipse(fill, circle);
                graphics.DrawEllipse(border, circle);
                SizeF textSize = graphics.MeasureString(text, font);
                graphics.DrawString(
                    text,
                    font,
                    textBrush,
                    screen.X - textSize.Width / 2f,
                    screen.Y - textSize.Height / 2f);
            }
        }
    }
}
