using System.Reflection;
using System.Windows.Forms;

namespace NexplantQMS.GdsMap
{
    /// <summary>
    /// DataGridView 이중 버퍼 켜기 (2026-10-15). DACrux 쪽 DACrux.SEMDMS.Control.GridBuffer 와 같은 내용이다.
    /// 목적: 행이 많은 표를 마우스 휠로 스크롤할 때 잔상이 남는 현상을 없앤다(기본 DataGridView 는 이중 버퍼가 꺼져 있다).
    /// 처리: DoubleBuffered 는 protected 속성이라 리플렉션으로 켠다. 화면 생성자에서 InitializeComponent() 뒤에 Enable(this) 한 번 부른다.
    /// </summary>
    internal static class GridBuffer
    {
        private static readonly PropertyInfo DoubleBufferedProperty =
            typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>root 와 그 아래(탭 / 분할 창 / 패널 안 포함)의 DataGridView 를 모두 찾아 이중 버퍼를 켠다.</summary>
        internal static void Enable(Control root)
        {
            if (root == null) return;
            DataGridView grid = root as DataGridView;
            if (grid != null && DoubleBufferedProperty != null) DoubleBufferedProperty.SetValue(grid, true, null);
            foreach (Control child in root.Controls)
                Enable(child);
        }

        /// <summary>표의 이중 버퍼가 켜져 있는지(자동 검사용).</summary>
        internal static bool IsEnabled(DataGridView grid)
        {
            return grid != null && DoubleBufferedProperty != null && (bool)DoubleBufferedProperty.GetValue(grid, null);
        }
    }
}
