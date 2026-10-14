using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// GDS 지도 위에 표시할 KLARF Defect 1개. 좌표는 지도와 같은 월드 좌표(µm, 보기 기준)다.
    /// 설계: 문서/2026-10-12_GDS_KLARF_연관분석_화면_설계.md 5.2절
    /// </summary>
    public sealed class DefectOverlayMarker
    {
        public long Id;
        /// <summary>Defect 중심(µm).</summary>
        public double X;
        public double Y;
        /// <summary>Defect 크기(XSIZE / YSIZE, µm). 0 이면 점.</summary>
        public double Width;
        public double Height;
        /// <summary>Chain 영역에 걸렸는지. 걸림 = 빨강 채움 / 밖 = 노랑 외곽선.</summary>
        public bool InChain;
        /// <summary>목록에서 고른 Defect. 허용오차 점선 사각형과 번호를 함께 그린다.</summary>
        public bool Selected;
    }

    /// <summary>
    /// KLARF Defect 표시(Die 상세 분석 화면 전용). 수가 적어(수십 ~ 수백) GPU 버퍼 대신 Chain 단자 Marker 와 같은
    /// GDI+ 오버레이(SwapBuffers 뒤 CreateGraphics)로 그린다. 확대 배율과 관계없이 최소 크기를 둬 Chip 전체 보기에서도 보이게 한다.
    /// 기존 _defectList(GDS 파일 Structure 의 가상 결함)와는 별개다.
    /// </summary>
    public partial class GdsMapControl
    {
        /// <summary>Defect 표시 최소 크기(화면 px). Defect 3 µm 는 Chip 전체 보기에서 1px 도 안 된다.</summary>
        private const float DefectMarkerMinPixels = 7f;

        private readonly List<DefectOverlayMarker> _defectMarkers = new List<DefectOverlayMarker>();
        private double _defectMarkerTolerance;

        private static readonly Color DefectInChainColor = Color.FromArgb(230, 40, 40);
        private static readonly Color DefectOutChainColor = Color.FromArgb(255, 200, 0);

        /// <summary>표시 중인 Defect 수.</summary>
        public int DefectMarkerCount { get { return _defectMarkers.Count; } }

        /// <summary>Defect 표시를 바꾼다. toleranceUm 은 고른 Defect 의 허용오차 점선 사각형에 쓴다.</summary>
        public void SetDefectMarkers(IEnumerable<DefectOverlayMarker> markers, double toleranceUm)
        {
            _defectMarkers.Clear();
            if (markers != null) _defectMarkers.AddRange(markers);
            _defectMarkerTolerance = Math.Max(0, toleranceUm);
            Invalidate();
        }

        /// <summary>고른 Defect 만 바꾼다(목록 행 이동 시 전체 목록을 다시 만들지 않는다).</summary>
        public void SelectDefectMarker(long id)
        {
            foreach (DefectOverlayMarker m in _defectMarkers) m.Selected = m.Id == id;
            Invalidate();
        }

        /// <summary>화면 좌표 근처(반경 radiusPixels)에서 가장 가까운 Defect 번호. 없으면 null.</summary>
        public long? FindDefectMarkerAt(Point screenPoint, int radiusPixels = 8)
        {
            DefectOverlayMarker best = null;
            double bestDistance = double.MaxValue;
            foreach (DefectOverlayMarker m in _defectMarkers)
            {
                PointF p = WorldToScreen(new GPoint(m.X, m.Y));
                double d = Math.Sqrt((p.X - screenPoint.X) * (p.X - screenPoint.X) + (p.Y - screenPoint.Y) * (p.Y - screenPoint.Y));
                if (d <= radiusPixels + DefectMarkerMinPixels / 2 && d < bestDistance)
                {
                    best = m;
                    bestDistance = d;
                }
            }
            return best == null ? (long?)null : best.Id;
        }

        /// <summary>Defect 전체(없으면 빈 영역)의 월드 범위. 화면 확대 대상 계산용.</summary>
        public GBox GetDefectMarkerBounds()
        {
            if (_defectMarkers.Count == 0) return GBox.Empty;
            return new GBox(_defectMarkers.Min(m => m.X - m.Width / 2), _defectMarkers.Min(m => m.Y - m.Height / 2),
                _defectMarkers.Max(m => m.X + m.Width / 2), _defectMarkers.Max(m => m.Y + m.Height / 2));
        }

        /// <summary>
        /// OnPaint 의 SwapBuffers 뒤에 호출된다. Chain 밖 -> Chain 영역 -> 고른 Defect 순서로 그려 중요한 표시가 위에 오게 한다.
        /// </summary>
        private void DrawDefectOverlay()
        {
            if (_defectMarkers.Count == 0) return;
            using (Graphics graphics = CreateGraphics())
            using (var inFill = new SolidBrush(Color.FromArgb(170, DefectInChainColor)))
            using (var inPen = new Pen(Color.FromArgb(255, 120, 0, 0), 1.5f))
            using (var outPen = new Pen(DefectOutChainColor, 2f))
            using (var selectedPen = new Pen(Color.White, 2.5f))
            using (var tolerancePen = new Pen(Color.White, 1f) { DashStyle = DashStyle.Dash })
            using (var font = new Font("Segoe UI", 11f, FontStyle.Bold, GraphicsUnit.Pixel))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                foreach (DefectOverlayMarker m in _defectMarkers.Where(m => !m.InChain && !m.Selected))
                    graphics.DrawRectangle(outPen, ToRectangle(MarkerRect(m, 0)));
                foreach (DefectOverlayMarker m in _defectMarkers.Where(m => m.InChain && !m.Selected))
                {
                    RectangleF r = MarkerRect(m, 0);
                    graphics.FillRectangle(inFill, r);
                    graphics.DrawRectangle(inPen, ToRectangle(r));
                }
                foreach (DefectOverlayMarker m in _defectMarkers.Where(m => m.Selected))
                {
                    RectangleF r = MarkerRect(m, 0);
                    if (m.InChain) graphics.FillRectangle(inFill, r);
                    graphics.DrawRectangle(m.InChain ? selectedPen : outPen, ToRectangle(r));
                    if (_defectMarkerTolerance > 0)
                        graphics.DrawRectangle(tolerancePen, ToRectangle(MarkerRect(m, _defectMarkerTolerance)));
                    string text = "#" + m.Id;
                    graphics.DrawString(text, font, Brushes.Black, r.Right + 5, r.Top - 1);
                    graphics.DrawString(text, font, Brushes.White, r.Right + 4, r.Top - 2);
                }
            }
        }

        /// <summary>Defect 사각형(+ 여유)을 화면 사각형으로 바꾼다. 최소 크기보다 작으면 중심 기준으로 키운다.</summary>
        private RectangleF MarkerRect(DefectOverlayMarker m, double margin)
        {
            PointF a = WorldToScreen(new GPoint(m.X - m.Width / 2 - margin, m.Y - m.Height / 2 - margin));
            PointF b = WorldToScreen(new GPoint(m.X + m.Width / 2 + margin, m.Y + m.Height / 2 + margin));
            float left = Math.Min(a.X, b.X), top = Math.Min(a.Y, b.Y);
            float width = Math.Abs(b.X - a.X), height = Math.Abs(b.Y - a.Y);
            float min = margin > 0 ? DefectMarkerMinPixels + 6 : DefectMarkerMinPixels;
            if (width < min) { left -= (min - width) / 2; width = min; }
            if (height < min) { top -= (min - height) / 2; height = min; }
            return new RectangleF(left, top, width, height);
        }

        private static Rectangle ToRectangle(RectangleF r)
        {
            return Rectangle.Round(r);
        }
    }
}
