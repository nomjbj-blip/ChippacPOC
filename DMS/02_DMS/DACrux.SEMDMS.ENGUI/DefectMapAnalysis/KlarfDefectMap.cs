using System;
using System.Drawing;
using System.Windows.Forms;
using DACrux.Base;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// KLARF 파일 조회 화면 전용 Defect Map 이다.
    /// Wafer/Die/Defect 그리기와 확대/이동/메뉴는 기존 DACrux.Map.DefectMap 을 그대로 사용하고,
    /// 이 클래스는 아래 두 가지만 추가한다.
    /// 1. 기존 OnChangePosition 은 Die 위에서만 발생하므로 Wafer 어디서든 마우스 좌표(um, Wafer 중심 기준)를 알려주는 이벤트
    /// 2. Die 안에 KLARF 원본 Index "(x,y)" 라벨을 그리는 기능 (화면 확대 배율이 충분할 때만)
    /// </summary>
    public class KlarfDefectMap : DACrux.Map.DefectMap
    {
        /// <summary>마우스 위치가 바뀔 때 Wafer 중심 기준 좌표(um)를 전달한다. 드래그 중에는 발생하지 않는다.</summary>
        public event EventHandler<KlarfMousePositionEventArgs> MousePositionChanged;

        /// <summary>마우스가 Map 밖으로 나가면 좌표 표시를 지우도록 알린다.</summary>
        public event EventHandler MousePositionCleared;

        /// <summary>Die 안에 원본 Index 라벨을 그릴지 여부.</summary>
        public bool ShowDieIndexLabel { get; set; }

        /// <summary>
        /// 화면 Index 에서 빼면 KLARF 원본 Index 가 되는 값.
        /// ParserKlarf 는 Index 를 좌하단 기준(양수)으로 옮기므로 원본 = 화면 Index - IndexOffset 이다.
        /// </summary>
        public Point IndexOffset { get; set; }

        /// <summary>
        /// 기존 마우스 처리(Die 판정/드래그 Zoom 등)를 먼저 실행하고, 버튼을 누르지 않은 이동일 때만 좌표 이벤트를 보낸다.
        /// GetRealPoint 는 기존 WaferMap 의 화면 픽셀 -> Wafer 좌표 변환 함수(확대/이동/회전 반영)이다.
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button != MouseButtons.None || MousePositionChanged == null || WaferSize <= 0) return;
            PointD point = GetRealPoint(e.X, e.Y);
            MousePositionChanged(this, new KlarfMousePositionEventArgs(point.X, point.Y, e.Location));
        }

        /// <summary>Map 밖으로 나가면 마지막 좌표가 남지 않도록 지움 이벤트를 보낸다.</summary>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (MousePositionCleared != null) MousePositionCleared(this, EventArgs.Empty);
        }

        /// <summary>
        /// Wafer 좌표(um)를 현재 화면 픽셀 위치로 바꾼다. 화면에서 가까운 Defect 를 찾을 때 사용한다.
        /// 계산식은 기존 DefectMap.DrawDefect 와 같다 (ViewAngle 회전 후 확대 배율 적용).
        /// </summary>
        public PointF ToScreen(double x, double y)
        {
            if (ViewAngle != 0) RotatePoint(ref x, ref y, ViewAngle);
            return new PointF(
                (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + x) * m_dZoomRatio_X_m_dScale),
                (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - y) * m_dZoomRatio_X_m_dScale));
        }

        /// <summary>
        /// 기존 Map 이 DrawWafer 로 임시 이미지를 다 그린 뒤(e == null) 화면에 복사하기 직전에 라벨을 덧그린다.
        /// OriginPreviewWaferMap 과 같은 방식이며 Paint 이벤트(e != null)에서는 다시 그리지 않는다.
        /// </summary>
        protected override void WaferMap_Paint(object sender, PaintEventArgs e)
        {
            if (e == null && ShowDieIndexLabel && m_gdiTempMap != null && m_arrDies != null)
                DrawDieIndexLabels(m_gdiTempMap);
            base.WaferMap_Paint(sender, e);
        }

        /// <summary>
        /// 각 Die 의 화면 사각형 중심에 KLARF 원본 Index 를 그린다.
        /// Die 가 화면에서 너무 작으면(가로 40px 미만) 글자가 겹치므로 그리지 않는다.
        /// </summary>
        private void DrawDieIndexLabels(Graphics graphics)
        {
            if (m_arrDies.Count == 0) return;
            float dieWidth = (float)(DieSizeX * m_dZoomRatio_X_m_dScale);
            if (dieWidth < 40f) return;

            using (var font = new Font("맑은 고딕", Math.Min(10f, Math.Max(7f, dieWidth / 9f))))
            using (var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                foreach (Die die in m_arrDies)
                {
                    // DieCood 는 Wafer 기준 Die 좌하단 좌표이며 그리기 시 원점(OriginX/Y)을 빼서 사용한다.
                    double centerX = die.DieCood.X + die.DieCood.Width / 2d - OriginX;
                    double centerY = die.DieCood.Y + die.DieCood.Height / 2d - OriginY;
                    PointF center = ToScreen(centerX, centerY);
                    string text = string.Format("({0},{1})", die.IndexX - IndexOffset.X, die.IndexY - IndexOffset.Y);
                    graphics.DrawString(text, font, Brushes.Black, center, format);
                }
            }
        }
    }

    /// <summary>KlarfDefectMap 마우스 좌표 이벤트 데이터. X/Y 는 Wafer 중심 기준 um 이다.</summary>
    public class KlarfMousePositionEventArgs : EventArgs
    {
        public KlarfMousePositionEventArgs(double x, double y, Point screen)
        {
            X = x;
            Y = y;
            Screen = screen;
        }

        public double X { get; private set; }
        public double Y { get; private set; }
        public Point Screen { get; private set; }
    }
}
