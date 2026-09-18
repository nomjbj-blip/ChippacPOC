using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DACrux.Data.Parser
{
    // 파일 타입 분석 2019.08.24 Taihi,Kim.
    public class FileAnalyzer
    {
        public static readonly string FILE_HEADER_KLARF = "FileVersion";
        public static readonly string[] FILE_HEADER_TFF_ARR = { "TENCOR SFS 7000", "KLA_TENCOR_AIT" };
        public static readonly string KLARF_VERSION_1_8 = "Record FileRecord  \"1.8\"";

        public static readonly int BUFFER_LENGTH = 30;
        private byte[] _buffer;

        public FileAnalyzer(string fileName)
        {
            string header = ReadHeader(fileName, BUFFER_LENGTH);
            FileType = GetFileType(header);
        }

        private string ReadHeader(string fileName, int length)
        {
            if (length <= 0 || !File.Exists(fileName))
                return null;


            byte[] buffer = new byte[length];

            using (FileStream fs = File.OpenRead(fileName))
            {
                fs.Read(buffer, 0, length);
            }

            return System.Text.Encoding.ASCII.GetString(buffer);
        }

        private FileType GetFileType(string header)
        {
            if (header.IndexOf(FILE_HEADER_KLARF) == 0)
                return FileType.KlarfFile;

            if (header.IndexOf(KLARF_VERSION_1_8) == 0)
                return FileType.KlarfFile_1_8;

            foreach (string tffHeaer in FILE_HEADER_TFF_ARR)
            {
                if (header.IndexOf(tffHeaer) == 0)
                    return Parser.FileType.TffFile;
            }

            return FileType.Unknown;
        }

        public FileType FileType
        {
            get;
            private set;
        }
    }

    public enum FileType
    {
        Unknown,
        KlarfFile,
        KlarfFile_1_8,
        TffFile
    }
}
