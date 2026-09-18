using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SharpCompress.Archive;
using SharpCompress.Archive.GZip;
using SharpCompress.Compressor.Deflate;
using SharpCompress.Compressor;
using SharpCompress.Reader;
using SharpCompress.Common;
using SharpCompress.Writer;
using System.Diagnostics;



namespace DACrux.Utility
{
	public class ZipUtil
	{
		public enum CompressType { TAR, TGZ };

        public static void UncompressGZipToTar(string gZipFile)
        {
            try
            {
                GZipUncompress(gZipFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static System.IO.FileInfo ProcGZip2Tar(System.IO.FileInfo gzip)
        {
            try
            {
                return GZipUncompress(gzip.FullName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static FileInfo GZipUncompress(string gZipFile)
        {
            string tarFile = string.Empty;
            if (Path.GetExtension(gZipFile).ToUpper() == ".GZ")
            {
                tarFile = Path.GetFullPath(gZipFile).Replace(".gz", "");
                tarFile = Path.GetFullPath(tarFile).Replace(".GZ", "");
            }

            try
            {
                int readCount = 0;
                byte[] buffer = new byte[1024];

                using (Stream stream = File.Open(gZipFile, FileMode.Open, FileAccess.Read))
                using (GZipStream gzipStream = new GZipStream(stream, CompressionMode.Decompress))
                {
                    Stream TarStream = File.Open(tarFile, FileMode.CreateNew, FileAccess.Write);
                    while ((readCount = gzipStream.Read(buffer, 0, 1024)) > 0)
                    {
                        TarStream.Write(buffer, 0, readCount);
                        TarStream.Flush();
                    }
                    TarStream.Close();
                }

                if (File.Exists(tarFile))
                {
                    return new FileInfo(tarFile);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void GZipCompress(string gZipFile, string sourceFile, bool sourceDelete = false)
        {
            try
            {
                int readCount = 0;
                int tot = 0;
                byte[] buffer = new byte[1024];


                using (Stream gzStream = File.Open(gZipFile, FileMode.CreateNew, FileAccess.Write))
                using (GZipStream gzipStream = new GZipStream(gzStream, CompressionMode.Compress))
                {
                    using (Stream stream = File.Open(sourceFile, FileMode.Open, FileAccess.Read))
                    {
                        while ((readCount = stream.Read(buffer, 0, 1024)) > 0)
                        {
                            gzipStream.Write(buffer, 0, readCount);
                            gzipStream.Flush();
                            tot += readCount;
                        }
                    }
                    gzipStream.Close();
                }

                if (sourceDelete)
                {
                    File.Delete(sourceFile);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static System.IO.FileInfo[] UncompressTar(string sourceFile, ref System.IO.FileInfo[] unextractFiles)
        {
            try
            {
                return Uncompress(sourceFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static System.IO.FileInfo[] Uncompress(string sourceFile, string TarPath = "", bool sourceDelete = false)
        {
            if (TarPath == "")
            {
                TarPath = Path.Combine(Path.GetDirectoryName(sourceFile), Path.GetFileNameWithoutExtension(sourceFile));
            }

            System.Collections.ArrayList alUnextractFile = new System.Collections.ArrayList();

            try
            {
                using (Stream stream = File.OpenRead(sourceFile))
                {
                    var reader = ReaderFactory.Open(stream);
                    while (reader.MoveToNextEntry())
                    {
                        if (!reader.Entry.IsDirectory)
                        {
                            Debug.WriteLine(reader.Entry.FilePath);
                            reader.WriteEntryToDirectory(TarPath, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                            alUnextractFile.Add(new System.IO.FileInfo(TarPath));
                        }
                    }
                }

                if (sourceDelete)
                {
                    File.Delete(sourceFile);
                }

                return (System.IO.FileInfo[])alUnextractFile.ToArray(Type.GetType("System.IO.FileInfo"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Compress(string descFile, string sourcePath, string searchPattern = "*", ArchiveType ArcType = ArchiveType.Zip, CompressionType CompType = CompressionType.None)
        {
            try
            {
                using (FileStream zip = File.OpenWrite(descFile))
                using (var zipWriter = WriterFactory.Open(zip, ArcType, CompType))
                {
                    zipWriter.WriteAll(sourcePath, searchPattern);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Compress(string descFile, string sourcePath, string[] sourceFiles, ArchiveType ArcType = ArchiveType.Zip, CompressionType CompType = CompressionType.None)
        {
            try
            {
                using (FileStream zip = File.OpenWrite(descFile))
                using (var zipWriter = WriterFactory.Open(zip, ArcType, CompType))
                {
                    for (int i = 0; i < sourceFiles.Length; i++)
                    {
                        FileInfo fi = new FileInfo(sourceFiles[i]);
                        if (fi.Exists)
                        {
                            zipWriter.Write(fi.FullName, fi);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

	}
}
