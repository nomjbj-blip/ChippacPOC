using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SCK.EnvCheck
{
    /// <summary>
    /// DACrux SCK POC 클라이언트 설치 점검 도구 시작점.
    /// 인자
    ///   (없음)   : 점검 화면 (시작 메뉴 "설치 점검")
    ///   /setup   : 점검 화면, Setup 이 설치 끝에 띄울 때
    ///   /silent  : 화면 없이 결과 파일만 쓰고 종료 코드로 알림 (무음 설치 / 자동 검사용)
    /// 종료 코드(/silent): 0 = 실패 없음, 1 = 실패 항목 있음, 2 = 점검 도구 오류
    /// 설치 폴더는 이 EXE 가 있는 폴더로 본다(Setup 이 설치 폴더 루트에 넣는다).
    /// </summary>
    internal static class Program
    {
        [STAThread]
        private static int Main(string[] args)
        {
            string appDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (HasArg(args, "/silent")) return RunSilent(appDir);

            Application.Run(new MainForm(appDir, HasArg(args, "/setup")));
            return 0;
        }

        /// <summary>
        /// 화면 없이 점검한다. OpenGL 점검은 화면 밖 작은 창을 잠깐 띄워 확인한다.
        /// </summary>
        private static int RunSilent(string appDir)
        {
            try
            {
                var checker = new EnvChecker(appDir);
                List<CheckResult> results = checker.RunAll(GlProbe.Run());
                checker.WriteReport(results);
                return results.Any(r => r.Level == CheckLevel.Fail) ? 1 : 0;
            }
            catch (Exception ex)
            {
                try
                {
                    string path = Path.Combine(Path.GetTempPath(), "DACrux_SCK_POC_" + EnvChecker.ReportFileName);
                    File.WriteAllText(path, "점검 도구 오류: " + ex, new UTF8Encoding(true));
                }
                catch (Exception)
                {
                    // 결과 파일도 쓸 수 없으면 종료 코드로만 알린다.
                }
                return 2;
            }
        }

        private static bool HasArg(string[] args, string name)
        {
            return args.Any(a => string.Equals(a, name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
