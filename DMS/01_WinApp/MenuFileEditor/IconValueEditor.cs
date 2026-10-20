using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace MenuFileEditor
{
    /// <summary>
    /// 속성 창의 아이콘 칸 편집기 (2026-10-15).
    /// 목적: 긴 16진수 문자열을 직접 고치지 않고, 칸 왼쪽에 그림을 보이며 [...] 로 편집 창(IconEditForm)을 열어 바꾸게 한다.
    /// 크기: 속성 이름이 Icon16 이면 16 x 16, 그 밖에는 32 x 32 로 맞춘다.
    /// </summary>
    public sealed class IconValueEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var service = provider == null ? null : provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            int size = context != null && context.PropertyDescriptor != null && context.PropertyDescriptor.Name == "Icon16" ? 16 : 32;
            using (var form = new IconEditForm(value as string, size))
            {
                DialogResult result = service != null ? service.ShowDialog(form) : form.ShowDialog();
                return result == DialogResult.OK ? form.HexValue : value;
            }
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>속성 칸 왼쪽 작은 칸에 아이콘을 그린다(못 읽으면 비워 둠).</summary>
        public override void PaintValue(PaintValueEventArgs e)
        {
            using (Image image = MenuDocument.HexToImage(e.Value as string))
            {
                if (image == null) return;
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                e.Graphics.DrawImage(image, e.Bounds);
            }
        }
    }
}
