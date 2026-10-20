using System.Reflection;
using System.Windows.Forms;

namespace DACrux.SEMDMS.Control
{
    /// <summary>
    /// DataGridView 이중 버퍼 켜기 (2026-10-15, POC 화면 공통).
    /// 목적: 행이 많은 표를 마우스 휠로 스크롤할 때 잔상이 남는 현상을 없앤다.
    /// 원인: 기본 DataGridView 는 이중 버퍼가 꺼져 있어 바탕을 지운 뒤 칸을 하나씩 화면에 바로 그린다. 행 2,000 x 열 10 측정에서
    ///       스크롤 한 번 다시 그리기가 평균 44ms(화면 갱신 16.7ms 의 2 ~ 3배)라 그리는 중간 상태가 잔상으로 보였다. 이중 버퍼를 켜면 4ms.
    /// 처리: DoubleBuffered 는 protected 속성이라 Designer 에서 바꿀 수 없어 리플렉션으로 켠다.
    ///       화면 생성자에서 InitializeComponent() 뒤에 Enable(this) 한 번 부르면 그 화면 안의 표가 모두 적용된다.
    /// </summary>
    public static class GridBuffer
    {
        private static readonly PropertyInfo DoubleBufferedProperty =
            typeof(System.Windows.Forms.Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>root 와 그 아래(탭 / 분할 창 / 패널 안 포함)의 DataGridView 를 모두 찾아 이중 버퍼를 켠다.</summary>
        public static void Enable(System.Windows.Forms.Control root)
        {
            if (root == null) return;
            DataGridView grid = root as DataGridView;
            if (grid != null) EnableGrid(grid);
            foreach (System.Windows.Forms.Control child in root.Controls)
                Enable(child);
        }

        /// <summary>표 하나의 이중 버퍼를 켠다. 코드로 나중에 만드는 표에 쓴다.</summary>
        public static void EnableGrid(DataGridView grid)
        {
            if (grid == null || DoubleBufferedProperty == null) return;
            DoubleBufferedProperty.SetValue(grid, true, null);
        }

        /// <summary>표의 이중 버퍼가 켜져 있는지(자동 검사용).</summary>
        public static bool IsEnabled(DataGridView grid)
        {
            return grid != null && DoubleBufferedProperty != null && (bool)DoubleBufferedProperty.GetValue(grid, null);
        }
    }
}
