using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SevenZip;
using System.IO;

namespace DACrux.SEMDMS.RO
{
    public static class SevenZipUtil
    {
        private static void LoadLibrary()
        {
            string zipFile;

            if (Environment.Is64BitProcess)
                zipFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "7z64.dll");
            else
                zipFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "7z.dll");

            if (!File.Exists(zipFile))
                throw new Exception(String.Format("'{0}' 파일이 없어서 압축을 진행할 수 없습니다.", zipFile));

            SevenZip.SevenZipBase.SetLibraryPath(zipFile);
        }

        public static void Zip(string zipFileName, params string[] sourceFiles)
        {
            LoadLibrary();

            SevenZipCompressor myCompressor = new SevenZipCompressor();
            myCompressor.ArchiveFormat = OutArchiveFormat.Zip;
            myCompressor.IncludeEmptyDirectories = true;
            myCompressor.CompressFilesEncrypted(zipFileName, null, sourceFiles);
        }

        public static void ZipFolder(string zipFileName, string sourceFolder)
        {
            LoadLibrary();

            SevenZipCompressor myCompressor = new SevenZipCompressor();
            myCompressor.ArchiveFormat = OutArchiveFormat.Zip;
            myCompressor.IncludeEmptyDirectories = true;
            myCompressor.CompressDirectory(sourceFolder, zipFileName);
        }

        public static void Unzip(string zipFileName, string unzipFolder)
        {
            LoadLibrary();

            using (SevenZip.SevenZipExtractor zip = new SevenZip.SevenZipExtractor(zipFileName))
            {
                zip.ExtractArchive(unzipFolder);
            }
        }
    }
}
