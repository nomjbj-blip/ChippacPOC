using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using DACrux.Base;

namespace DACrux.TEST.ENGUI
{
    /// <summary>
    /// File Map Setup / Wafer Map 조회 화면 전용 Map 컨트롤. 기존 EditWaferMap 을 그대로 상속하고 원 안 판정 함수 하나만 밖으로 연다.
    /// Die 번호(DieNumberRule) 를 Die 가운데에 덧그리는 기능도 여기서 한다 (2026-10-13). 기존 WaferMap 의 Die 글자 표시
    /// (DisplayDieValue) 에는 번호 모드가 없고 Core 는 수정하지 않으므로, GDS-KLARF 연관 분석 Map 과 같이 그린 뒤 덧그리는 방식이다.
    /// 왜: WaferMap.UseDie (Wafer 반지름 - Edge / Flat·Notch 영역 판정) 는 protected 라 화면에서 부를 수 없다.
    ///     06_CoreLibSource 는 수정하지 않으므로 상속해서 같은 판정을 재사용한다.
    /// 주의: WaferMap.DieCalculation(true) 는 LeftBottom 등에서 AddDie 와 Die 위치 계산이 한 칸 다르다.
    ///     그래서 이 화면은 DieCalculation 대신 AddDie 로 격자를 넣고, 이 함수로 Die 마다 원 안 여부를 정한다.
    /// </summary>
    public class StandardMapEditMap : DACrux.Map.EditWaferMap
    {
        /// <summary>우클릭 메뉴에 추가한 중심 십자선 항목과 그 앞 구분선 (2026-10-13).</summary>
        private readonly MenuItem centerMarkSeparator;
        private readonly MenuItem centerMarkMenu;

        /// <summary>
        /// 우클릭 메뉴는 Free Zoom / Fit Size / Move View Center 와 [Center Cross] 만 보인다 (2026-10-13).
        /// 기존 메뉴(WaferMap 의 protected ctxmWaferMap)는 수정하지 않고, 기반 클래스 생성자가 메뉴를 만든 뒤 항목 / Popup 처리기를 붙인다.
        /// </summary>
        public StandardMapEditMap()
        {
            centerMarkSeparator = ctxmWaferMap.MenuItems.Add("-");
            centerMarkMenu = ctxmWaferMap.MenuItems.Add("Center Cross", centerMarkMenu_Click);
            ctxmWaferMap.Popup += ctxmWaferMap_ShowViewItemsOnly;
        }

        /// <summary>
        /// 메뉴가 뜨기 직전: 보기 항목 3개와 [Center Cross] 만 보이고 나머지 항목(Rotate Notch / Zone / Shot / Edit Map 등)과 구분선은 숨긴다.
        /// 기존 처리기(Shot 메뉴 사용 가능 여부 등)가 먼저 실행된 뒤라 표시 여부는 여기서 정한 값이 남는다.
        /// [Center Cross] 의 체크는 지금 십자선 표시 여부(CenterMark)다.
        /// </summary>
        private void ctxmWaferMap_ShowViewItemsOnly(object sender, EventArgs e)
        {
            foreach (MenuItem item in ctxmWaferMap.MenuItems)
                item.Visible = item == mnuitemMAPMODE_FREEZOOM || item == mnuitemMAPMODE_FITSIZE || item == mnuitemMOUSEDRAGMODE_MOVE
                    || item == centerMarkSeparator || item == centerMarkMenu;
            centerMarkMenu.Checked = CenterMark;
        }

        /// <summary>
        /// 우클릭 [Center Cross]: Wafer 중심 십자선(CenterMark, 기존 WaferMap 기능)을 켜고 끈다.
        /// CenterMark 는 켤 때 바로 선을 긋지만 끌 때는 이미 그린 선을 지우지 않으므로 다시 그린다. 기본값은 화면 Designer 값(켬) 그대로다.
        /// </summary>
        private void centerMarkMenu_Click(object sender, EventArgs e)
        {
            CenterMark = !CenterMark;
            Redraw();
        }

        /// <summary>
        /// Die 가 Wafer 안(반지름 - Edge, Notch / Flat 영역 밖)에 완전히 들어가는지 판정한다.
        /// DieCood 는 AddDie 가 계산한 Die 좌하단 좌표(원점 기준)이고, UseDie 는 Wafer 중심 기준 좌표를 받으므로 Origin 을 뺀다.
        /// Notch / Flat 영역은 그리기 중에 계산되므로 설정을 바꾼 뒤 Redraw 를 먼저 한 번 해야 한다.
        /// </summary>
        public bool IsInsideWafer(Die die)
        {
            return UseDie(die.DieCood.X - OriginX, die.DieCood.Y - OriginY, die.DieCood.Width, die.DieCood.Height);
        }

        /// <summary>
        /// 화면의 편집 모드 라디오(보기 / 제거 / 복원)를 Map 컨트롤 동작으로 바꾼다.
        /// 왜: EditWaferMap 은 마우스 모드(m_eoMouseDragMode, protected)가 Normal 일 때만 클릭 / 드래그로 Die 속성을 바꾸고,
        ///     기본값은 Zoom 이다. 이 값을 바꾸는 공개 함수가 없어서 상속 클래스에서 연다 (Core 수정 금지).
        /// - edit = false (보기): Zoom 으로 되돌려 클릭해도 Die 가 바뀌지 않는다 (드래그 = 확대, 기존 기본 동작).
        /// - edit = true: EditMethod 를 지정하고 Normal 로 바꾼다. 제거 = EditMode.DELETE(-1), 복원 = EditMode.ADD(1).
        /// </summary>
        public void SetDieEditMode(bool edit, DACrux.Map.EditMode mode)
        {
            if (edit)
            {
                EditMethod = mode;
                m_eoMouseDragMode = DACrux.Map.MouseDragMode.Normal;
            }
            else
            {
                m_eoMouseDragMode = DACrux.Map.MouseDragMode.Zoom;
            }
        }

        /// <summary>지금 클릭 / 드래그가 Die 속성을 바꾸는 상태인지 (검사 / 화면 확인용).</summary>
        public bool IsDieEditing
        {
            get { return m_eoMouseDragMode == DACrux.Map.MouseDragMode.Normal; }
        }

        private Dictionary<Point, int> dieNumbers = new Dictionary<Point, int>();

        /// <summary>
        /// Die 번호. Key = Map 컨트롤 Die 번호(IndexX, IndexY, 두 화면 모두 LeftBottom) / Value = 표시할 번호.
        /// 화면이 DieNumberRule 로 계산해 넣는다. 바꾼 뒤 Redraw 해야 그림에 반영된다.
        /// 실행 중에만 쓰는 값이라 Designer / resx 에 저장하지 않는다 (2026-10-13).
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dictionary<Point, int> DieNumbers
        {
            get { return dieNumbers; }
            set { dieNumbers = value ?? new Dictionary<Point, int>(); }
        }

        /// <summary>Die 번호를 그릴지. 바꾼 뒤 Redraw 해야 그림에 반영된다. 화면 체크박스 값으로 정하므로 Designer 에 저장하지 않는다.</summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ShowDieNumbers { get; set; }

        /// <summary>
        /// 기존 Map 이 Die 를 그림 버퍼에 다 그린 뒤(e == null) 화면에 복사하기 전에 번호를 덧그린다.
        /// Paint 이벤트(e != null)는 버퍼를 복사만 하므로 다시 그리지 않는다 (번호가 버퍼에 남아 있다).
        /// </summary>
        protected override void WaferMap_Paint(object sender, PaintEventArgs e)
        {
            if (e == null && ShowDieNumbers && dieNumbers.Count > 0 && m_gdiTempMap != null && m_arrDies != null)
                DrawDieNumbers(m_gdiTempMap);
            base.WaferMap_Paint(sender, e);
        }

        /// <summary>
        /// 번호 글자 크기는 Die 화면 크기와 자릿수로 정한다. 글자가 6pt 보다 작아지면(Die 가 작게 보일 때) 그리지 않는다.
        /// 확대하면 다시 그려지므로 큰 Map 은 확대해서 본다.
        /// </summary>
        private void DrawDieNumbers(Graphics graphics)
        {
            float dieWidth = (float)(DieSizeX * m_dZoomRatio_X_m_dScale);
            float dieHeight = (float)(DieSizeY * m_dZoomRatio_X_m_dScale);
            int digits = 1;
            foreach (int number in dieNumbers.Values) digits = Math.Max(digits, number.ToString(CultureInfo.InvariantCulture).Length);
            // 맑은 고딕 숫자 폭은 대략 글자 크기(pt)의 0.75 배 픽셀이다. 앞뒤 여백 1 자리를 둔다.
            float size = Math.Min(14f, Math.Min(dieWidth / (0.75f * (digits + 1)), dieHeight * 0.45f));
            if (size < 6f) return;

            using (Font font = new Font("맑은 고딕", size, FontStyle.Bold, GraphicsUnit.Pixel))
            using (StringFormat format = new StringFormat())
            {
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                foreach (Die die in m_arrDies)
                {
                    int number;
                    if (!dieNumbers.TryGetValue(new Point(die.IndexX, die.IndexY), out number)) continue;
                    double x = die.DieCood.X + die.DieCood.Width / 2d - OriginX;
                    double y = die.DieCood.Y + die.DieCood.Height / 2d - OriginY;
                    graphics.DrawString(number.ToString(CultureInfo.InvariantCulture), font, Brushes.Black, ToScreen(x, y), format);
                }
            }
        }

        /// <summary>Wafer 중심 기준 µm 를 그림 버퍼 픽셀로 바꾼다 (WaferMap.GetViewPoint 가 private 이라 같은 식을 쓴다, ViewAngle 회전 포함).</summary>
        public PointF ToScreen(double x, double y)
        {
            if (ViewAngle != 0) RotatePoint(ref x, ref y, ViewAngle);
            return new PointF(
                (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + x) * m_dZoomRatio_X_m_dScale),
                (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - y) * m_dZoomRatio_X_m_dScale));
        }
    }
}
