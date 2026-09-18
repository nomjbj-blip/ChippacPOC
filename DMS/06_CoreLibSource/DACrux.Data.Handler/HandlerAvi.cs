using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DACrux.Base;
using DACrux.Data.Parser;

namespace DACrux.Data.Handler
{
    public class HandlerAvi : Handler
    {
        public static readonly string AVI_IMAGE_EXT = ".JPEG;.BMP;.JPG";
        public static readonly string AVI_ALL_EXTENTION = ".00;.01;.02;.03;.04;.05;.06;.07;.08;.09;.10;.11;.12;.13;.14;.15;.16;.17;.18;.19;.20;.21;.22;.23;.24;.25;.000;.001;.002;.003;.004;.005;.006;.007;.008;.009;.010;.011;.012;.013;.014;.015;.016;.017;.018;.019;.020;.021;.022;.023;.024;.025;.TXT;.JPEG;.BMP;.JPG";

        protected List<string> m_AVIList = new List<string>();
        protected List<string> m_ImageList = new List<string>();
        protected List<string> m_mappingImageList = new List<string>();
        public string FACTORY { get; set; }

        protected override ParserBase CreateParser(string fileName)
        {
            try
            {
                return new ParserAviMapFile(fileName);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Parsing Error: {0}", Path.GetFileName(fileName)), ex);
            }
        }

        /// <summary>
        /// 데이터 파일 파싱 처리를 실행합니다.
        /// </summary>
        public override void Run()
        {
            FileInfo oAVIFile = null;
            FileInfo oImage = null;
            FileInfo[] oImages = null;
            DirectoryInfo oDir;

            try
            {
                if (FileNames == null || FileNames.Length == 0)
                    return;

                string[] AllExtentions = AVI_ALL_EXTENTION.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                string[] ImageExtentions = AVI_IMAGE_EXT.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string fileName in FileNames)
                {
                    WriteLog(String.Format("'{0}' 파일 처리 시작", fileName));

                    oAVIFile = new FileInfo(fileName);

                    if (oAVIFile.Exists == false)
                        continue;

                    if (Array.IndexOf(AllExtentions, oAVIFile.Extension.ToUpper()) < 0)
                    {
                        try
                        {
                            oAVIFile.Delete();
                        }
                        catch { }

                        continue;
                    }

                    if(Array.IndexOf(ImageExtentions, oAVIFile.Extension.ToUpper()) > -1)
                    {
                        m_ImageList.Add(fileName);
                    }
                    else
                    {
                        m_AVIList.Add(fileName);
                    }
                }

                // Avi 파일 파싱
                foreach (string AVI in m_AVIList)
                {
                    // 파일 파싱 
                    ParserAviMapFile parser = CreateParser(AVI) as ParserAviMapFile;

                    if (parser.ErrorFlag)
                    {
                        ParserList.Add(parser);
                        continue;
                    }

                    oAVIFile = new FileInfo(AVI);
                    oAVIFile.IsReadOnly = false;
                    oDir = new DirectoryInfo(oAVIFile.Directory.ToString());

                    //각 Index 별 Image 정보 저장
                    for (int iDie = 0; iDie < parser.DieList.Count; iDie++)
                    {
                        //Image 의 경우 원본 X, Y 의 값을 기준으로 Mapping 해준다. 
                        if (FACTORY == FAB1)
                        {
                            //ex)QPE110$AU09001Q_2X_HY$9127010$AK9127010-16$2$24$1849.767$3141.579$5$ALL$1.jpg
                            oImages = oDir.GetFiles(string.Format("*{0}${1}${2}${3}${4}*.jpg", parser.DEVICE, parser.LotID, Path.GetFileNameWithoutExtension(oAVIFile.FullName), (parser.DieList[iDie].X - 1), (parser.DieList[iDie].Y - 1)));
                        }
                        else
                        {
                            //ex)P_D136BY33001U_191S52_01_7,16_A.jpg
                            oImages = oDir.GetFiles(string.Format("{0}_{1}_{2}_{3},{4}*.jpg", parser.DEVICE, parser.LotID, parser.WAFER, (parser.DieList[iDie].X - 1), (parser.DieList[iDie].Y - 1)));
                        }

                        parser.DieList[iDie].IMAGE = null;
                        if (oImages != null && oImages.Length > 0)
                            parser.DieList[iDie].IMAGE = oImages;

                        //Image 가 있을 경우 Backup 될 경로에 대해 미리 정의해 놓는다.
                        if (parser.DieList[iDie].IMAGE != null && parser.DieList[iDie].IMAGE.Length > 0)
                        {
                            //여러 Image 중 하나의 Image 만 저장한다.
                            oImage = new FileInfo(parser.DieList[iDie].IMAGE[0].FullName);
                            oImage.IsReadOnly = false;
                            string strBacupFileName = string.Format("{0}_{1}_{2}_{3},{4}.jpg", parser.DEVICE, parser.LotID, parser.WAFER, parser.DieList[iDie].X, parser.DieList[iDie].Y);
                            string strBacupThumbnail = ImageLoader.GetThumbnailImageName(strBacupFileName);

                            if (oImage.Exists)
                            {
                                // 원본 File Backup 경로
                                parser.DieList[iDie].IMAGE_BACKUP = Path.Combine(GetFullBackupPath(parser), strBacupFileName);

                                if (!Directory.Exists(GetFullBackupPath(parser)))
                                    Directory.CreateDirectory(GetFullBackupPath(parser));

                                string strThumeNailFile = ImageLoader.GetThumeNail(100, 100, oImage.FullName);
                                if (!string.IsNullOrEmpty(strThumeNailFile))
                                {
                                    oImage = new FileInfo(strThumeNailFile);

                                    if (oImage.Exists)
                                    {
                                        parser.DieList[iDie].THUMENAIL = oImage.FullName;
                                        parser.DieList[iDie].THUMENAIL_BACKUP = Path.Combine(GetFullBackupPath(parser), strBacupThumbnail);
                                    }
                                }
                            }
                        }
                    }

                    //// 백업 파일명 설정
                    parser.BackupDirectoryName = GetFullBackupPath(parser);
                    parser.BackupFileName = Path.Combine(parser.BackupDirectoryName, Path.GetFileName(parser.FileName));
                    parser.FtpRelativePath = GetImageRelativePath(parser.BackupDirectoryName);

                    //// 파서 리스트에 추가
                    ParserList.Add(parser);
                }
            }
            finally
            {
                oAVIFile = null;
                oImage = null;
                oImages = null;
                oDir = null;
            }
        }
    }
}
