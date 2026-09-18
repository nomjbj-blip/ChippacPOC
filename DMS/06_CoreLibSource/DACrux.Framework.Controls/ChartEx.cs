using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Forms.DataVisualization.Charting
{
    public class ChartEx : System.Windows.Forms.DataVisualization.Charting.Chart
    {
        public static readonly int FONT_MAX = 32;
        public static readonly int FONT_MIN = 6;

        public ChartEx()
        {
            SetStyle(ControlStyles.Selectable, true);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            Focus();
        }

        protected override void OnMouseWheel(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            // Ctrl + Mouse Wheel 시 X축 Font 크기 조정 2019.11.22 Taihi,Kim.
            if (System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Control)
            {
                if (ChartAreas == null || ChartAreas.Count == 0)
                    return;

                if (ChartAreas[0].AxisX == null)
                    return;

                System.Drawing.Font currFont = ChartAreas[0].AxisX.LabelStyle.Font;
                float size = currFont.Size + (e.Delta > 0 ? 1f : -1f);
                size = Math.Max(size, FONT_MIN);
                size = Math.Min(size, FONT_MAX);

                ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font(currFont.FontFamily, size);
                currFont.Dispose();
            }
        }
    }
}
