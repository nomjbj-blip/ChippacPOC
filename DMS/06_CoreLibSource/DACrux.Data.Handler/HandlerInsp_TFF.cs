using System;
using System.Text;
using System.Diagnostics;
using System.IO;
using DACrux.Data.Parser;

namespace DACrux.Data.Handler
{
    public class HandlerInsp_TFF : HandlerInsp
    {
        public static readonly string PATH_TFF2KLA_CONVERTER = @"TFF2KLA\TFF2KLA.EXE";

        public static readonly string TFF_FILE_EXT = ".TFF";

        /// <summary>
        /// TFF 파일인지를 가져옵니다.
        /// </summary>
        private bool IsTffFile(string fileName)
        {
            return Path.GetExtension(fileName).ToUpper() == TFF_FILE_EXT;
        }

        /// <summary>
        /// TFF2KLA 를 실행합니다.
        /// </summary>
        private string Execute_TFF2KLA(string sourceFile)
        {
            string targetFile = Path.ChangeExtension(sourceFile, KLARF_FILE_DEFAULT_EXT);

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PATH_TFF2KLA_CONVERTER);
            start.Arguments = String.Format("{0} {1}", sourceFile, targetFile);
            start.WindowStyle = ProcessWindowStyle.Hidden;
            start.CreateNoWindow = true;

            Process.Start(start).WaitForExit();

            if (!File.Exists(targetFile))
                throw new Exception("TFF2KLA에서 변환후의 출력 파일을 찾을 수 없습니다. " + targetFile);

            return targetFile;
        }

        public override void Run()
        {
            // TFF 파일이 있는 경우 KLARF 파일로 변환한다.
            for (int i = 0; i < FileNames.Length; i++)
            {
                if (!IsTffFile(FileNames[i]))
                    continue;

                // TFF 를 KLARF 파일로 변환 후 FileName 속성값을 업데이트
                string newFile = Execute_TFF2KLA(FileNames[i]);
                // TFF 파일 삭제
                File.Delete(FileNames[i]);
                // KLARF 파일 경로로 업데이트
                FileNames[i] = newFile;
            }

            base.Run();
        }
    }
}
