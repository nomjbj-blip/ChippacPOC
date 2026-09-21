using System.Drawing;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// 레이어 목록에 표시할 현재 색상과 표시 상태의 복사본이다.
    /// 실제 도면 상태는 GdsMapControl에서 관리하고, 목록을 갱신할 때 이 객체로 전달한다.
    /// </summary>
    public sealed class LayerDisplayItem
    {
        public int LayerId { get; private set; }
        public Color Color { get; set; }
        public bool Visible { get; private set; }

        /// <summary>도면의 레이어 상태를 목록에서 사용할 항목으로 만든다.</summary>
        public LayerDisplayItem(int layerId, Color color, bool visible)
        {
            LayerId = layerId;
            Color = color;
            Visible = visible;
        }

        /// <summary>기본 체크박스 그리기와 접근성 읽기에 사용할 레이어 이름을 제공한다.</summary>
        public override string ToString()
        {
            return "Layer " + LayerId;
        }
    }

    /// <summary>
    /// 기본 CheckedListBox의 체크/키보드 동작을 유지하면서 항목 오른쪽에 색상 견본을 그린다.
    /// CheckedListBox는 DrawItem 이벤트를 지원하지 않으므로 OnDrawItem을 재정의한다.
    /// </summary>
    public class LayerColorCheckedListBox : CheckedListBox
    {
        /// <summary>
        /// 체크박스와 이름은 기본 컨트롤에 맡기고, 별도로 확보한 오른쪽 영역에 색상을 표시한다.
        /// 항목 높이를 기준으로 크기를 계산하여 글꼴/DPI 변경에 맞춘다.
        /// </summary>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= Items.Count || !(Items[e.Index] is LayerDisplayItem))
            {
                base.OnDrawItem(e);
                return;
            }

            var item = (LayerDisplayItem)Items[e.Index];
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
            using (var brush = new SolidBrush(item.Color))
                e.Graphics.FillRectangle(brush, swatch);
            e.Graphics.DrawRectangle(SystemPens.WindowText, swatch);
        }

        /// <summary>우클릭한 행을 선택하여 체크 상태를 바꾸지 않고 색상 변경 대상으로 지정한다.</summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                SelectedIndex = IndexFromPoint(e.Location);
            base.OnMouseDown(e);
        }
    }
}
