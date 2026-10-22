using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
    /// <summary>목록 행 오른쪽에 색 견본을 그릴 항목. Layer / Chain 목록이 함께 쓴다 (2026-10-13).</summary>
    public interface IColorListItem
    {
        Color ListColor { get; }
    }

    /// <summary>
    /// 레이어 목록에 표시할 현재 색상과 표시 상태의 복사본이다.
    /// 실제 도면 상태는 GdsMapControl에서 관리하고, 목록을 갱신할 때 이 객체로 전달한다.
    /// </summary>
    public sealed class LayerDisplayItem : IColorListItem
    {
        Color IColorListItem.ListColor { get { return Color; } }

        /// <summary>Layer 번호 + DataType 키(GdsLayerKey). 18/0 / 18/3 / 18/4 는 다른 항목이다 (2026-10-20)</summary>
        public int LayerKey { get; private set; }
        public int Layer { get { return GdsLayerKey.Layer(LayerKey); } }
        public int DataType { get { return GdsLayerKey.DataType(LayerKey); } }
        public Color Color { get; set; }
        public bool Visible { get; private set; }

        /// <summary>도면의 레이어 상태를 목록에서 사용할 항목으로 만든다.</summary>
        public LayerDisplayItem(int layerKey, Color color, bool visible)
        {
            LayerKey = layerKey;
            Color = color;
            Visible = visible;
        }

        /// <summary>기본 체크박스 그리기와 접근성 읽기에 사용할 레이어 이름을 제공한다.</summary>
        public override string ToString()
        {
            return "Layer " + GdsLayerKey.Format(LayerKey);
        }
    }

    /// <summary>
    /// 기본 CheckedListBox의 체크/키보드 동작을 유지하면서 항목 오른쪽에 색상 견본을 그린다.
    /// CheckedListBox는 DrawItem 이벤트를 지원하지 않으므로 OnDrawItem을 재정의한다.
    /// </summary>
    public class LayerColorCheckedListBox : CheckedListBox
    {
        /// <summary>
        /// 우클릭하면 그 행을 선택할지. Layer 목록은 선택(색 변경 대상 지정), Chain 목록은 우클릭이 편집 대상을 바꾸지 않도록 false.
        /// </summary>
        [DefaultValue(true)]
        public bool SelectOnRightClick { get; set; } = true;

        /// <summary>
        /// 체크는 행 왼쪽 체크 박스를 눌렀을 때만 바꾼다(2026-10-20, Setup / View 화면 Layer 목록).
        /// 행 글자 클릭 / 선택된 행 다시 클릭 / 더블클릭 / Space 키는 행 선택만 한다. 코드에서 바꾸는 체크(전체 체크 등)는 그대로 된다.
        /// 체크 박스를 빠르게 두 번 누르면 두 번 바뀐다(일반 체크 박스와 같음).
        /// </summary>
        [DefaultValue(false)]
        public bool CheckOnCheckBoxOnly { get; set; }

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_CHAR = 0x0102;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONDBLCLK = 0x0203;
        /// <summary>목록 선택 변경 알림(LBN_SELCHANGE)이 부모에서 되돌아오는 메시지. 기본 목록은 여기서 체크를 바꾼다.</summary>
        private const int WM_REFLECT_COMMAND = 0x2000 + 0x0111;

        /// <summary>마우스 / 키 입력을 처리하는 중(기본 목록이 체크를 바꾸려는 때).</summary>
        private int _userInput;
        /// <summary>체크 박스를 눌러 체크를 바꾸는 중.</summary>
        private bool _checkBoxClick;

        /// <summary>
        /// 체크박스와 이름은 기본 컨트롤에 맡기고, 별도로 확보한 오른쪽 영역에 색상을 표시한다.
        /// 항목 높이를 기준으로 크기를 계산하여 글꼴/DPI 변경에 맞춘다.
        /// </summary>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= Items.Count || !(Items[e.Index] is IColorListItem))
            {
                base.OnDrawItem(e);
                return;
            }

            var item = (IColorListItem)Items[e.Index];
            int swatchSize = System.Math.Max(6, e.Bounds.Height - 4);
            int reservedWidth = swatchSize + 10;
            e.DrawBackground();
            var textBounds = e.Bounds;
            textBounds.Width = System.Math.Max(0, textBounds.Width - reservedWidth);
            var textArgs = new DrawItemEventArgs(e.Graphics, e.Font, textBounds,
                e.Index, e.State, e.ForeColor, e.BackColor);
            base.OnDrawItem(textArgs);

            var swatch = new Rectangle(e.Bounds.Right - reservedWidth + 3,
                e.Bounds.Top + (e.Bounds.Height - swatchSize) / 2, swatchSize, swatchSize);
            using (var brush = new SolidBrush(item.ListColor))
                e.Graphics.FillRectangle(brush, swatch);
            e.Graphics.DrawRectangle(SystemPens.WindowText, swatch);
        }

        /// <summary>
        /// 우클릭한 행을 선택하여 체크 상태를 바꾸지 않고 색상 변경 대상으로 지정한다(SelectOnRightClick 일 때).
        /// CheckOnCheckBoxOnly 이면 왼쪽 버튼으로 체크 박스를 누를 때만 체크를 바꾼다.
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && SelectOnRightClick)
                SelectedIndex = IndexFromPoint(e.Location);
            base.OnMouseDown(e);
            if (CheckOnCheckBoxOnly && e.Button == MouseButtons.Left)
            {
                int index = IndexFromPoint(e.Location);
                if (IsOnCheckBox(index, e.Location))
                {
                    _checkBoxClick = true;
                    try
                    {
                        SetItemChecked(index, !GetItemChecked(index));
                    }
                    finally
                    {
                        _checkBoxClick = false;
                    }
                }
            }
        }

        /// <summary>
        /// 기본 목록은 클릭 / 선택된 행 다시 클릭 / 더블클릭 / Space 키에서 체크를 바꾼다.
        /// CheckOnCheckBoxOnly 이면 그 입력을 처리하는 동안 생기는 체크 변경을 막는다(체크 박스를 누른 변경만 받는다).
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            bool input = CheckOnCheckBoxOnly && (m.Msg == WM_LBUTTONDOWN || m.Msg == WM_LBUTTONDBLCLK
                || m.Msg == WM_REFLECT_COMMAND || m.Msg == WM_KEYDOWN || m.Msg == WM_CHAR);
            if (input) _userInput++;
            try
            {
                base.WndProc(ref m);
            }
            finally
            {
                if (input) _userInput--;
            }
        }

        /// <summary>막는 체크 변경은 ItemCheck 이벤트를 올리지 않고 그대로 둔다.</summary>
        protected override void OnItemCheck(ItemCheckEventArgs ice)
        {
            if (CheckOnCheckBoxOnly && _userInput > 0 && !_checkBoxClick)
            {
                ice.NewValue = ice.CurrentValue;
                return;
            }
            base.OnItemCheck(ice);
        }

        /// <summary>
        /// 위치가 그 행의 체크 박스 칸인지. 기본 CheckedListBox 가 그리는 체크 그림 크기(+ 여백)를 쓴다.
        /// </summary>
        private bool IsOnCheckBox(int index, Point location)
        {
            if (index < 0 || index >= Items.Count) return false;
            Rectangle row = GetItemRectangle(index);
            if (!row.Contains(location)) return false;
            int glyph;
            if (Application.RenderWithVisualStyles)
            {
                using (Graphics g = CreateGraphics())
                    glyph = CheckBoxRenderer.GetGlyphSize(g, System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal).Width;
            }
            else
                glyph = 13 * DeviceDpi / 96;
            return location.X < row.Left + glyph + 4;
        }
    }
}
