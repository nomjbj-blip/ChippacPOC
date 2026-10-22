using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DACrux.Base;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// GDS × KLARF 연관 분석 화면(frmGdsKlarfAnalysis) 전용 Wafer Map 이다.
    /// 설계: 문서/설계/05_연관분석/2026-10-12_GDS_KLARF_연관분석_화면_설계.md 4.4 / 4.5절
    ///
    /// Wafer / Die / Defect 그리기, 확대 / 이동 / 기본 메뉴는 기존 DefectMap(KlarfDefectMap)을 그대로 쓰고 아래만 더한다.
    /// 1. Die 채움색: 서버 분석 결과(Die 안 Chain 영역 Defect 수)를 구간 색으로 칠한다.
    ///    기존 Defect Count Color(ItemCountColorList)는 Map 에 올라간 Defect 수로만 세고 구간 밖 Die 의 Defect 를 숨기므로 쓰지 않는다.
    ///    DefectMap.DrawWafer 순서가 DrawDies -> DrawDefect -> DrawDieBorder 이므로 DrawDies 뒤에 칠하면 Defect 점은 그 위에 그려진다.
    /// 2. Die 안 숫자(Chain 영역 Defect 수): Map 을 다 그린 뒤(e == null) 덧그린다(KlarfDefectMap 의 Index 라벨과 같은 방식).
    /// 3. Die 클릭 / 더블클릭 이벤트: 기존 DefectMap 은 Die 더블클릭 이벤트가 없고 Defect 배열만 보내므로 현재 Die Index 로 따로 알린다.
    /// 4. 우클릭 메뉴(설계 14.2 / 14.3절, 2026-10-12 요청): 우클릭하면 커서 아래 Chip 을 먼저 선택하고, 메뉴는
    ///    Free Zoom / Fit Size / Center Cross(2026-10-13) / Detail Analysis(신규) 만 보인다(Defect / Die Select 는 2026-10-16 제거, 기본 Die 선택). 기존 메뉴(WaferMap 의 protected ctxmWaferMap)는
    ///    수정하지 않고 Popup 때 표시 여부만 바꾼다(06_CoreLibSource 수정 금지). DefectMap 항목 필드가 private 이라 Text 로 구분한다.
    /// 5. E-Test Fail Die 표식(2026-10-16): Die 왼쪽 위 모서리 빨간 삼각형. 선택 Die 빨간 테두리와 겹치지 않게 테두리 대신 모서리에 그린다.
    /// </summary>
    public class GdsKlarfAnalysisMap : KlarfDefectMap
    {
        /// <summary>Die 를 클릭했다(드래그 아님). 인자: Die Index (XINDEX, YINDEX).</summary>
        public event EventHandler<GdsKlarfDieEventArgs> DieClicked;

        /// <summary>Die 를 더블클릭했다. 인자: Die Index (XINDEX, YINDEX).</summary>
        public event EventHandler<GdsKlarfDieEventArgs> DieDoubleClicked;

        /// <summary>우클릭 메뉴 [Detail Analysis] 를 눌렀다. 인자: 선택한 Chip(Die Index).</summary>
        public event EventHandler<GdsKlarfDieEventArgs> DetailAnalysisRequested;

        /// <summary>
        /// 우클릭 메뉴에서 보이게 둘 기존 항목(Text).
        /// Defect Select / Die Select 는 2026-10-16 요청으로 뺐다. 화면이 처음부터 Die 선택 모드로 둔다(frmGdsKlarfAnalysis.ShowWafer).
        /// </summary>
        private static readonly HashSet<string> VisibleMenuTexts = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Free Zoom", "Fit Size"
        };

        private readonly MenuItem _detailSeparator;
        private readonly MenuItem _detailMenu;

        /// <summary>중심 십자선 보이기 / 숨기기 항목과 그 앞 구분선 (2026-10-13).</summary>
        private readonly MenuItem _crossSeparator;
        private readonly MenuItem _crossMenu;

        /// <summary>Die 별 채움색. 없으면 기존 Map 색 그대로다.</summary>
        private readonly Dictionary<Point, Color> _dieFill = new Dictionary<Point, Color>();

        /// <summary>Die 안에 쓸 숫자(Chain 영역 Defect 수). Defect 가 있는 Die 만 넣는다.</summary>
        private readonly Dictionary<Point, int> _dieLabel = new Dictionary<Point, int>();

        /// <summary>
        /// E-Test 표식 (2026-10-16 통합 분석 설계 8.1절, 2026-10-20 두 가지로 나눔: 문서/설계/05_연관분석/2026-10-20_Map_ETest_표식_설계.md).
        /// 색 기준과 상관없이 모서리에 그린다. 선택 Die 의 빨간 테두리 / 가운데 숫자와 겹치지 않게 모서리 표식만 쓴다.
        ///  Spec Out(DMS Spec 판정 F) = 왼쪽 위 빨간 삼각형(두 변의 35%)
        ///  Fail Die(결과 파일 DUT Flag F) = 오른쪽 아래 작은 남색 삼각형(22%). 결과 파일 Fail 이 대부분인 Wafer 도 Map 을 덮지 않게 작게 둔다.
        /// 둘 다 흰 1px 테두리를 그려 진한 구간 색 위에서도 보이게 한다.
        /// </summary>
        private readonly HashSet<Point> _specDies = new HashSet<Point>();
        private readonly HashSet<Point> _fileDies = new HashSet<Point>();

        /// <summary>Spec Out 표식 색. 색 구간 팝업 표의 Spec Out Die 행도 같은 색으로 그린다.</summary>
        internal static readonly Color FailMarkColor = Color.FromArgb(0xE2, 0x1B, 0x1B);

        /// <summary>Fail Die(결과 파일) 표식 색. 빨강 / 구간 색과 겹치지 않는 남색.</summary>
        internal static readonly Color FileFailMarkColor = Color.FromArgb(0x24, 0x40, 0x8E);

        /// <summary>표식 크기: Die 두 변을 따라 이 비율까지 삼각형.</summary>
        private const float FailMarkRatio = 0.35f;
        private const float FileFailMarkRatio = 0.22f;

        private Point _mouseDown;

        /// <summary>Die 안 숫자를 그릴지 여부.</summary>
        public bool ShowDieCount { get; set; }

        /// <summary>
        /// 우클릭 [Detail Analysis] 를 보일지 여부(기본 true = 지금 연관 분석 화면 그대로).
        /// Simple 화면(frmGdsKlarfSimple)은 Die 상세를 연결하기 전까지 끈다 (2026-10-22, 설계 문서/설계/05_연관분석/2026-10-22_GDS_KLARF_연관분석_Simple_화면_설계.md 결정 S9).
        /// </summary>
        [System.ComponentModel.DefaultValue(true)]
        public bool ShowDetailMenu { get; set; }

        public GdsKlarfAnalysisMap()
        {
            ShowDieCount = true;
            ShowDetailMenu = true;
            // 기반 클래스 생성자(InitializeComponent)가 ctxmWaferMap 을 만든 뒤라 바로 항목을 붙일 수 있다.
            // Popup 처리기는 WaferMap / DefectMap 처리기 뒤에 등록되므로 그 처리가 끝난 뒤 표시 여부를 정한다.
            _crossSeparator = ctxmWaferMap.MenuItems.Add("-");
            _crossMenu = ctxmWaferMap.MenuItems.Add("Center Cross", CrossMenu_Click);
            _detailSeparator = ctxmWaferMap.MenuItems.Add("-");
            _detailMenu = ctxmWaferMap.MenuItems.Add("Detail Analysis", DetailMenu_Click);
            ctxmWaferMap.Popup += ctxmWaferMap_PopupFilter;
        }

        /// <summary>지금 우클릭 메뉴에 보이는 항목 Text(구분선 "-" 포함). Smoke 확인용.</summary>
        public List<string> VisibleMenuItems()
        {
            var result = new List<string>();
            foreach (MenuItem item in ctxmWaferMap.MenuItems)
                if (item.Visible) result.Add(item.Text);
            return result;
        }

        /// <summary>[Detail Analysis] 를 지금 누를 수 있는지(선택 Chip 있음). Smoke 확인용.</summary>
        public bool DetailMenuEnabled { get { return _detailMenu.Enabled; } }

        /// <summary>
        /// 메뉴가 뜨기 직전: 허용 항목만 보이고, [Detail Analysis] 는 선택 Chip 이 있을 때만 쓸 수 있다.
        /// [Center Cross] 의 체크는 지금 십자선 표시 여부(CenterMark)다 (2026-10-13).
        /// </summary>
        private void ctxmWaferMap_PopupFilter(object sender, EventArgs e)
        {
            foreach (MenuItem item in ctxmWaferMap.MenuItems)
                item.Visible = ((item == _detailMenu || item == _detailSeparator) && ShowDetailMenu) || item == _crossMenu || item == _crossSeparator
                    || VisibleMenuTexts.Contains(item.Text);
            _detailMenu.Enabled = m_SelectedDies != null && m_SelectedDies.Count > 0;
            _crossMenu.Checked = CenterMark;
        }

        /// <summary>
        /// 우클릭 [Center Cross]: Wafer 중심 십자선(CenterMark, 기존 WaferMap 기능)을 켜고 끈다. 기본값은 지금과 같이 끔이다.
        /// CenterMark 는 켤 때 바로 선을 긋지만 끌 때는 이미 그린 선을 지우지 않으므로 다시 그린다.
        /// </summary>
        private void CrossMenu_Click(object sender, EventArgs e)
        {
            CenterMark = !CenterMark;
            Redraw();
        }

        /// <summary>[Detail Analysis]: 선택한 Chip(여러 개면 첫 번째)으로 상세 화면을 요청한다.</summary>
        private void DetailMenu_Click(object sender, EventArgs e)
        {
            if (DetailAnalysisRequested == null || m_SelectedDies == null || m_SelectedDies.Count == 0) return;
            DetailAnalysisRequested(this, new GdsKlarfDieEventArgs(m_SelectedDies[0].X, m_SelectedDies[0].Y));
        }

        /// <summary>
        /// 분석 결과의 Die 채움색 / 숫자를 넣고 다시 그린다. fills 가 비어 있으면 기존 Map 모습으로 돌아간다.
        /// </summary>
        public void SetDieDisplay(IDictionary<Point, Color> fills, IDictionary<Point, int> labels)
        {
            _dieFill.Clear();
            _dieLabel.Clear();
            if (fills != null) foreach (KeyValuePair<Point, Color> p in fills) _dieFill[p.Key] = p.Value;
            if (labels != null) foreach (KeyValuePair<Point, int> p in labels) _dieLabel[p.Key] = p.Value;
            Redraw();
        }

        /// <summary>분석 결과 표시를 지운다(Wafer 변경 / 분석 전).</summary>
        public void ClearDieDisplay()
        {
            _specDies.Clear();
            _fileDies.Clear();
            SetDieDisplay(null, null);
        }

        /// <summary>E-Test 모서리 표식을 바꾼다(Spec Out Die / Fail Die). 다시 그리기는 SetDieDisplay 가 한다(2026-10-20).</summary>
        public void SetEtMarks(IEnumerable<Point> specOutDies, IEnumerable<Point> fileFailDies)
        {
            _specDies.Clear();
            _fileDies.Clear();
            if (specOutDies != null) foreach (Point p in specOutDies) _specDies.Add(p);
            if (fileFailDies != null) foreach (Point p in fileFailDies) _fileDies.Add(p);
        }

        /// <summary>표식이 있는 Spec Out Die / Fail Die 수(Smoke 확인용).</summary>
        public int SpecOutMarkCount { get { return _specDies.Count; } }
        public int FileFailMarkCount { get { return _fileDies.Count; } }

        /// <summary>
        /// 기존 Die 그리기 뒤에 분석 결과 채움색을 덮고 테두리를 다시 그린다.
        /// 좌표 계산은 DefectMap.DrawDies 와 같다(Die 좌하단 좌표 - 원점, 보기 각도 회전, 확대 배율).
        /// </summary>
        protected override void DrawDies(Graphics g)
        {
            base.DrawDies(g);
            if ((_dieFill.Count == 0 && _specDies.Count == 0 && _fileDies.Count == 0) || m_arrDies == null || m_arrDies.Count == 0) return;

            var points = new PointF[4];
            var cood = new PointD[4];
            using (var brush = new SolidBrush(Color.White))
            using (var failBrush = new SolidBrush(FailMarkColor))
            using (var fileBrush = new SolidBrush(FileFailMarkColor))
            using (var markEdge = new Pen(Color.White, 1f))
            using (var border = new Pen(m_colDieBorder))
            using (var selected = new Pen(Color.Red, 3))
            {
                foreach (Die die in m_arrDies)
                {
                    Color fill;
                    var key = new Point(die.IndexX, die.IndexY);
                    bool hasFill = _dieFill.TryGetValue(key, out fill);
                    bool specOut = _specDies.Contains(key);
                    bool fileFail = _fileDies.Contains(key);
                    if (!hasFill && !specOut && !fileFail) continue;
                    if (!UseDie(die.DieCood.X - m_WaferRecipe.ORIGIN_X, die.DieCood.Y - m_WaferRecipe.ORIGIN_Y,
                            m_WaferRecipe.DIE_SIZE_X, m_WaferRecipe.DIE_SIZE_Y)) continue;

                    cood[0].X = die.DieCood.X - m_WaferRecipe.ORIGIN_X;
                    cood[0].Y = die.DieCood.Y - m_WaferRecipe.ORIGIN_Y;
                    cood[1].X = cood[0].X + die.DieCood.Width;
                    cood[1].Y = cood[0].Y;
                    cood[2].X = cood[1].X;
                    cood[2].Y = cood[0].Y + die.DieCood.Height;
                    cood[3].X = cood[0].X;
                    cood[3].Y = cood[2].Y;
                    for (int i = 0; i < 4; i++)
                    {
                        RotatePoint(ref cood[i].X, ref cood[i].Y, m_iViewAngle);
                        points[i].X = (float)((-m_rectdWaferArea.X + m_WaferRecipe.WAFER_RADIUS + cood[i].X) * m_dZoomRatio_X_m_dScale);
                        points[i].Y = (float)((-m_rectdWaferArea.Y + m_WaferRecipe.WAFER_RADIUS - cood[i].Y) * m_dZoomRatio_X_m_dScale);
                    }

                    if (hasFill)
                    {
                        brush.Color = fill;
                        m_gdiTempMap.FillPolygon(brush, points);
                    }
                    // Spec Out: 왼쪽 위 꼭짓점(points[3])에서 두 변을 따라 35% 지점까지 빨간 삼각형
                    if (specOut)
                        FillMark(failBrush, markEdge, points[3], points[2], points[0], FailMarkRatio);
                    // Fail Die(결과 파일): 오른쪽 아래 꼭짓점(points[1])에서 22% 지점까지 남색 삼각형
                    if (fileFail)
                        FillMark(fileBrush, markEdge, points[1], points[0], points[2], FileFailMarkRatio);
                    if (m_SelectedDies.IndexOf(key) > -1)
                        g.DrawPolygon(selected, points);
                    else if (m_bVisibleDieBorder)
                        g.DrawPolygon(border, points);
                }
            }
        }

        /// <summary>꼭짓점 corner 에서 두 이웃 꼭짓점 쪽으로 ratio 만큼 간 삼각형을 칠하고 흰 테두리를 그린다.</summary>
        private void FillMark(Brush brush, Pen edge, PointF corner, PointF a, PointF b, float ratio)
        {
            var mark = new[] { corner, Lerp(corner, a, ratio), Lerp(corner, b, ratio) };
            m_gdiTempMap.FillPolygon(brush, mark);
            m_gdiTempMap.DrawPolygon(edge, mark);
        }

        private static PointF Lerp(PointF a, PointF b, float t)
        {
            return new PointF(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t);
        }

        /// <summary>Map 을 다 그린 뒤(e == null) Die 안 숫자를 덧그린다. Index 라벨(기반 클래스)과 겹치지 않게 아래쪽에 둔다.</summary>
        protected override void WaferMap_Paint(object sender, PaintEventArgs e)
        {
            if (e == null && ShowDieCount && _dieLabel.Count > 0 && m_gdiTempMap != null && m_arrDies != null)
                DrawDieCounts(m_gdiTempMap);
            base.WaferMap_Paint(sender, e);
        }

        /// <summary>Die 화면 폭이 18px 이상일 때만 숫자를 그린다. 진한 색 구간에서는 흰 글자를 쓴다.</summary>
        private void DrawDieCounts(Graphics graphics)
        {
            float dieWidth = (float)(DieSizeX * m_dZoomRatio_X_m_dScale);
            float dieHeight = (float)(DieSizeY * m_dZoomRatio_X_m_dScale);
            if (dieWidth < 18f) return;

            using (var font = new Font("맑은 고딕", Math.Min(16f, Math.Max(7f, dieWidth / 4.5f)), FontStyle.Bold))
            using (var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                foreach (Die die in m_arrDies)
                {
                    int count;
                    var key = new Point(die.IndexX, die.IndexY);
                    if (!_dieLabel.TryGetValue(key, out count)) continue;
                    double centerX = die.DieCood.X + die.DieCood.Width / 2d - OriginX;
                    double centerY = die.DieCood.Y + die.DieCood.Height / 2d - OriginY;
                    PointF center = ToScreen(centerX, centerY);
                    if (ShowDieIndexLabel) center.Y += dieHeight * 0.22f;
                    Color fill;
                    Brush brush = _dieFill.TryGetValue(key, out fill) && fill.GetBrightness() < 0.45f ? Brushes.White : Brushes.Black;
                    graphics.DrawString(count.ToString(), font, brush, center, format);
                }
            }
        }

        /// <summary>누른 위치를 기억한다. 놓을 때 거의 움직이지 않았으면 클릭으로 본다(드래그 확대 / 영역 선택과 구분).</summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            _mouseDown = e.Location;
            // 우클릭 메뉴는 기반 WaferMap.OnMouseDown 에서 바로(모달로) 뜨므로, 그 전에 커서 아래 Chip 을 선택한다(설계 14.2절).
            // m_iCurrentX / Y 는 마우스 이동 때 기반 클래스가 정한 현재 Die 다. Chip 밖이면(-1) 기존 선택을 그대로 둔다.
            if (e.Button == MouseButtons.Right && (m_iCurrentX >= 0 || m_iCurrentY >= 0) && DieClicked != null)
                DieClicked(this, new GdsKlarfDieEventArgs(m_iCurrentX, m_iCurrentY));
            base.OnMouseDown(e);
        }

        /// <summary>
        /// 왼쪽 버튼 클릭이고 현재 Die 가 있으면 DieClicked 를 보낸다.
        /// 기존 DefectMap.OnMouseUp(CalSelectedDie)은 Shift / Ctrl 없이 버튼을 놓으면 오른쪽 버튼이어도 선택 Die 를 모두 지운다.
        /// 우클릭(메뉴)으로 고른 Chip 의 선택(빨간 테두리)이 메뉴를 닫자마자 사라지지 않도록 오른쪽 버튼이면 이전 선택을 되살린다(2026-10-12 Smoke 로 발견).
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            List<Point> before = e.Button == MouseButtons.Right && m_SelectedDies != null ? new List<Point>(m_SelectedDies) : null;
            base.OnMouseUp(e);
            if (before != null)
            {
                m_SelectedDies.Clear();
                m_SelectedDies.AddRange(before);
                Redraw();
                return;
            }
            if (e.Button != MouseButtons.Left || DieClicked == null) return;
            if (Math.Abs(e.X - _mouseDown.X) > 3 || Math.Abs(e.Y - _mouseDown.Y) > 3) return;
            if (m_iCurrentX < 0 && m_iCurrentY < 0) return;
            DieClicked(this, new GdsKlarfDieEventArgs(m_iCurrentX, m_iCurrentY));
        }

        /// <summary>기존 더블클릭 처리(그 Die 의 Defect 배열 이벤트) 뒤에 Die 더블클릭을 알린다.</summary>
        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            if (DieDoubleClicked == null || (m_iCurrentX < 0 && m_iCurrentY < 0)) return;
            DieDoubleClicked(this, new GdsKlarfDieEventArgs(m_iCurrentX, m_iCurrentY));
        }
    }

    /// <summary>GdsKlarfAnalysisMap Die 이벤트 데이터 (Die Index = TQD_DEFECT XINDEX / YINDEX).</summary>
    public class GdsKlarfDieEventArgs : EventArgs
    {
        public GdsKlarfDieEventArgs(int xIndex, int yIndex)
        {
            XIndex = xIndex;
            YIndex = yIndex;
        }

        public int XIndex { get; private set; }
        public int YIndex { get; private set; }
    }
}
