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

        /// <summary>우클릭한 행을 선택하여 체크 상태를 바꾸지 않고 색상 변경 대상으로 지정한다(SelectOnRightClick 일 때).</summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && SelectOnRightClick)
                SelectedIndex = IndexFromPoint(e.Location);
            base.OnMouseDown(e);
        }
    }
}
