using System;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace DACrux.SEMDMS.TFF2KLA
{
    /// <summary>
    /// TFF to KLARF 변환기
    /// 2019.07.01. Taihi,Kim.
    /// </summary>
    class Program
    {
        /// <summary>
        /// 기본 확장자
        /// </summary>
        public static readonly string DEFAULT_EXT = ".000";

        /// <summary>
        /// Main 메서드
        /// </summary>
        static void Main(string[] args)
        {
            StringBuilder sb = new StringBuilder();

            if (args == null || args.Length == 0 || (args.Length == 1 && IsHelp(args[0])))
            {
                Console.WriteLine(GetHelp());
                return;
            }

            string sourceFile = args[0];

            if (!File.Exists(sourceFile))
            {
                Console.WriteLine("원본 파일을 찾을 수 없습니다.");
                return;
            }

            string targetFile = args.Length > 1 ? args[1] : Path.Combine(Path.GetDirectoryName(sourceFile), Path.GetFileNameWithoutExtension(sourceFile) + DEFAULT_EXT);

            if (File.Exists(targetFile))
            {
                while (true)
                {
                    Console.WriteLine("대상 파일을 덮어쓰시겠습니까?(Y/N)");
                    string result = Console.ReadLine().ToUpper().Trim();

                    if (result == "N")
                        return;
                    else if (result == "Y")
                        break;
                }
            }

            XtffToKlarf(sourceFile, targetFile);
        }

        /// <summary>
        /// Help 인지를 가져옵니다.
        /// </summary>
        private static bool IsHelp(string text)
        {
            return text == "?" || text.ToUpper().Trim() == "HELP";
        }

        /// <summary>
        /// Help 정보를 가져옵니다.
        /// </summary>
        /// <returns></returns>
        private static string GetHelp()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("TFF 파일을 KLARF 파일로 변환합니다.");
            sb.AppendLine("Copyright 2019 all rights reserved. (c) Miracom Inc.");
            sb.AppendLine();
            sb.AppendLine("TFF2KLA 원본 [대상]");
            sb.AppendLine();
            sb.AppendLine("원본\t\tTFF 파일을 지정합니다.");
            sb.AppendLine("대상\t\tKLARF 파일명을 지정합니다.");
            sb.AppendLine(String.Format("\t\t지정하지 않으면 자동으로 확장자가 {0} 인 파일로 생성됩니다.", DEFAULT_EXT));
            return sb.ToString();
        }

        /// <summary>
        /// TFF to KLARF
        /// </summary>
        /// <param name="tffFileName">TFF 파일명</param>
        /// <param name="klarfFileName">KLARF 파일명</param>
        [DllImport("xTff2KlarfD.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void XtffToKlarf(string tffFileName, string klarfFileName);
    }
}
