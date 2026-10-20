using System;
using System.Threading;
using System.Windows.Forms;

namespace MenuFileEditor
{
    /// <summary>
    /// 메뉴 파일 편집기 시작점 (2026-10-15).
    /// 사용: MenuFileEditor.exe [메뉴 파일 경로]. 경로를 주지 않으면 마지막으로 연 파일 또는 DACruxV5\MenuFile.mnu 를 연다.
    /// 처리하지 못한 오류는 창으로 알리고 프로그램은 계속 쓸 수 있게 둔다(편집 중인 내용 보호).
    /// </summary>
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            Application.Run(new MenuEditorForm(args.Length > 0 ? args[0] : null));
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message + Environment.NewLine + Environment.NewLine + e.Exception.StackTrace, "Menu File Editor",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
