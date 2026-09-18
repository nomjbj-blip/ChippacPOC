using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DACrux.Base;
using DACrux.Data.Parser.Klarf;
using DACrux.Data.Parser;
using DACrux.SP.Common;

namespace DACrux.Data.Handler
{
    /// <summary>
    /// Inspection 설비의 데이터 파일 처리
    /// </summary>
    public class HandlerInsp : Handler
    {
        public enum ImageCol
        {
            STEP_SEQ,
            DEFECTID,
            IMAGE_ID,
            WAFER_SEQ,
            TEST,
            IMAGE_TYPE,
            IMAGE_PATH,
            IMAGE_FILENAME,
            IMAGE_SERVER,
            THUMB_PATH,
            THUMB_FILEANME,
            UPLOAD
        }

        public static readonly string KLARF_FILE_DEFAULT_EXT = ".000";
        public static readonly string KLARF_CONTAINS_EXT = ".0";
        public static readonly string TRF_KLARF_CONTAINS_EXT = ".trf";

        public static readonly int MINIMUM_CLUSTER_COUNT = 5;
        public static readonly double CLUSTER_THRESHOLD = 100;
        public static readonly double NEW_DEFECT_TOLERANCE = 100;

        protected List<string> m_klarfList = new List<string>();
        protected List<string> m_trfKlarfList = new List<string>();

        protected override ParserBase CreateParser(string fileName)
        {
            try
            {
                FileAnalyzer anal = new FileAnalyzer(fileName);

                if (anal.FileType != FileType.KlarfFile_1_8)
                    return new ParserKlarf(fileName);
                else
                    return new ParserKlarf_18(fileName);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Parsing Error: {0}", Path.GetFileName(fileName)), ex);
            }
        }

        /// <summary>
        /// 확장자가 TRF 인지를 가져옵니다.
        /// </summary>
        public static bool IsTrfKlarfFile(string fileName)
        {
            return String.Compare(Path.GetExtension(fileName), TRF_KLARF_CONTAINS_EXT, true) == 0;
        }

        /// <summary>
        /// LOTEND 파일인지를 가져옵니다.
        /// </summary>
        public bool IsTrfKlarfLotEndFile(string fileName)
        {
            return Path.GetFileName(fileName).Contains("LOTEND");
        }

        /// <summary>
        /// 데이터 파일 파싱 처리를 실행합니다.
        /// </summary>
        public override void Run()
        {
            /*
             * 1) FileNames 배열은 Klarf 파일과 Image 파일이 섞여 있다.
             * 2) Klarf 파일은 각각 파싱 처리 한다.
             * 3) Image 파일은 Defect과 연동 시키기 위해 일단 한곳으로 모아둔다.
             * 4) 연동된 이미지는 백업으로 옮기고 삭제하여야 하므로 별도 리스트로 관리한다.
             * 5) 연동되지 않은 이미지4는 그대로 두고 다음번 처리 시 처리될 수 있도록 한다.
             * 6) (검토) 파일의 Last modified Time이 1일을 지난 경우 백업 폴더로 옮긴다.
             */

            if (FileNames == null || FileNames.Length == 0)
                return;

            // KLARF 파일과 Image 파일로 분류
            foreach (string fileName in FileNames)
            {
                if (Path.GetExtension(fileName).Contains(KLARF_CONTAINS_EXT))
                {
                    if (m_klarfList.Count < DATA_FILE_LIMIT_COUNT)
                        m_klarfList.Add(fileName);
                }

                if (IsTrfKlarfFile(fileName))
                {
                    if (m_trfKlarfList.Count < DATA_FILE_LIMIT_COUNT)
                        m_trfKlarfList.Add(fileName);
                }
            }

            // KLARF 파일 파싱
            foreach (string klarf in m_klarfList)
            {
                WriteLog(String.Format("'{0}' 파일 처리 시작", klarf));

                // 파일 파싱
                ParserKlarf parser = CreateParser(klarf) as ParserKlarf;

                // TFF 파일 파싱 후 데이터 없는 빈 파일일 경우 _00_.001로 옴.
                if (parser.ErrorFlag && klarf.Contains("_00_.001"))
                {
                    Remove(parser);
                    continue;
                }

                if (parser.ErrorFlag)
                {
                    // 파서 리스트에 추가
                    ParserList.Add(parser);
                    continue;
                }

                // 백업 파일명 설정
                parser.BackupDirectoryName = GetFullBackupPath(parser);
                parser.BackupFileName = Path.Combine(parser.BackupDirectoryName, Path.GetFileName(parser.FileName));
                parser.FtpRelativePath = GetImageRelativePath(parser.BackupDirectoryName);

                //
                // OrientationMarkLocation 값대로 데이터를 회전시켜 저장한다. FAB1/FAB2 모두 적용 2019.12.06 Taihi,Kim.
                //
                if (parser.GetAngle() > 0)
                {
                    parser.RotateAndRecalculate(360 - parser.GetAngle(), DieIndexSort.CenterToLowerLeft);
                }

                // 설비ID를 Handler 기준으로 변경
                parser.Equip = EquipInfo.EquipID;

                // FAB1은 LOT 앞에 A가 붙으면 제거
                if (EquipInfo.Factory == FAB1 && !String.IsNullOrEmpty(parser.LotID) && (parser.LotID[0] == 'A' || parser.LotID[0] == 'a'))
                    parser.ChangeLotID(parser.LotID.Substring(1));

                // 파서 리스트에 추가
                if (parser.Wafers.Count > 0)
                    ParserList.Add(parser);
                else
                    Remove(parser);
            }

            // TRF KLARF 파일 파싱
            foreach (string trfKlarf in m_trfKlarfList)
            {
                WriteLog(String.Format("'{0}' 파일 처리 시작", trfKlarf));

                // 파일 파싱
                ParserKlarf_Trf parser = new ParserKlarf_Trf(trfKlarf);

                // 백업 파일명 설정
                parser.BackupDirectoryName = GetFullBackupPath(parser);
                parser.BackupFileName = Path.Combine(parser.BackupDirectoryName, Path.GetFileName(parser.FileName));
                parser.FtpRelativePath = GetImageRelativePath(parser.BackupDirectoryName);

                // 설비ID를 Handler 기준으로 변경
                parser.Equip = EquipInfo.EquipID;

                // FAB1은 LOT 앞에 A가 붙으면 제거
                if (EquipInfo.Factory == FAB1 && !String.IsNullOrEmpty(parser.LotID) && (parser.LotID[0] == 'A' || parser.LotID[0] == 'a'))
                    parser.ChangeLotID(parser.LotID.Substring(1));

                // 파서 리스트에 추가
                ParserList.Add(parser);
            }
        }

        /// <summary>
        /// 데이터 파일을 백업 위치에 저장 합니다.
        /// </summary>
        public override void Backup(ParserBase parserBase)
        {
            ParserKlarf parser = parserBase as ParserKlarf;

            if (!Directory.Exists(parser.BackupDirectoryName))
                Directory.CreateDirectory(parser.BackupDirectoryName);

            // Klarf 파일 복사
            FileCopy(parser.FileName, parser.BackupFileName);

            // 이미지가 있는 경우
            if (parser.ImageManagerList != null && parser.ImageManagerList.HasData())
            {
                // 단일 이미지 처리
                foreach (ImageManager mgr in parser.ImageManagerList.SingleList)
                {
                    // 원본 복사
                    FileCopy(mgr.ImageFullName, Path.Combine(parser.BackupDirectoryName, mgr.ImageName));
                    // Thumbnail 복사
                    mgr.SaveThumbnailImage(parser.BackupDirectoryName);
                }

                // 다중 TIF 이미지 처리
                foreach (ImageManager mgr in parser.ImageManagerList.MultiList)
                {
                    foreach (var item in mgr.FrameDictionary)
                    {
                        // Frame 이미지 저장
                        mgr.SaveFrameImageAndThumbnail(item.Key, parser.BackupDirectoryName);
                    }
                }
            }
        }

        /// <summary>
        /// Review 용 Klarf 파일을 복사합니다.
        /// </summary>
        public void CopyReviewKlarfFile(ParserKlarf parser)
        {
            DACrux.Data.Parser.Klarf.ParserKlarf.AppendClassLookupInfo(parser.FileName);

            // EQUIP_CMF_2 에 값을 지정하면 해당 폴더명으로 파일이 들어감
            string folderName = GetReviewSpecialFolderName();

            // 폴더 정보가 없으면 String.Empty 값을 넣는다.
            if (String.IsNullOrEmpty(folderName))
                folderName = String.Empty;

            // REVIEW 폴더에 파일 복사
            FileCopy(parser.FileName, Path.Combine(ReviewCopyPath, folderName, Path.GetFileName(parser.FileName)));
        }

        /// <summary>
        /// Review 용 Klarf 파일을 새로 생성합니다.
        /// </summary>
        public void MakeReviewKlarfFile(ParserKlarf parser)
        {
            // EQUIP_CMF_2 에 값을 지정하면 해당 폴더명으로 파일이 들어감
            string folderName = GetReviewSpecialFolderName();

            // 폴더 정보가 없으면 String.Empty 값을 넣는다.
            if (String.IsNullOrEmpty(folderName))
                folderName = String.Empty;

            // 저장
            parser.DataSourceFromFile = false;
            parser.SaveFile(Path.Combine(ReviewCopyPath, folderName, Path.GetFileName(parser.FileName)), true);
        }

        /// <summary>
        /// 원본 데이터 파일을 추가 복사 합니다.
        /// </summary>
        public override void AddtionalCopy(ParserBase parserBase, string copyPath)
        {
            ParserKlarf parser = parserBase as ParserKlarf;

            if (!Directory.Exists(copyPath))
                Directory.CreateDirectory(copyPath);

            // 이미지가 있는 경우
            if (parser.ImageManagerList != null && parser.ImageManagerList.HasData())
            {
                // 단일 이미지 처리
                foreach (ImageManager mgr in parser.ImageManagerList.SingleList)
                {
                    // 원본 복사
                    if (File.Exists(mgr.ImageFullName))
                        FileCopy(mgr.ImageFullName, Path.Combine(copyPath, mgr.ImageName));
                }

                // 다중 TIF 이미지 처리
                foreach (ImageManager mgr in parser.ImageManagerList.MultiList)
                {
                    // 원본 복사
                    if (File.Exists(mgr.ImageFullName))
                        FileCopy(mgr.ImageFullName, Path.Combine(copyPath, mgr.ImageName));
                }
            }

            // copy klarf
            FileCopy(parser.FileName, Path.Combine(copyPath, Path.GetFileName(parser.FileName)));
        }

        /// <summary>
        /// 원본 데이터 파일을 삭제합니다.
        /// </summary>
        public override void Remove(ParserBase parserBase)
        {
            ParserKlarf parser = parserBase as ParserKlarf;

            // 에러 발생한 데이터 파일은 처리하지 않음
            if (parser.ErrorFlag)
                return;

            // 이미지가 있는 경우 삭제
            if (parser.ImageManagerList != null && parser.ImageManagerList.HasData())
            {
                // 단일 이미지 처리
                foreach (ImageManager mgr in parser.ImageManagerList.SingleList)
                {
                    mgr.Dispose();

                    try
                    {
                        // 원본 삭제
                        File.Delete(mgr.ImageFullName);
                    }
                    catch { }
                }

                // 다중 TIF 이미지 처리
                foreach (ImageManager mgr in parser.ImageManagerList.MultiList)
                {
                    mgr.Dispose();

                    try
                    {
                        // 원본 삭제
                        File.Delete(mgr.ImageFullName);
                    }
                    catch { }
                }

                parser.ImageManagerList.Dispose();
            }

            // Klarf 파일 삭제
            File.Delete(parser.FileName);
        }

        /// <summary>
        /// Defect 데이터를 2차원 배열로 가져옵니다.
        /// </summary>
        public string[,] GetDefect2DArray(Base.DefectList defectList, string[] defectColumnsOrder)
        {
            if (defectList == null || defectList.Count == 0)
                return null;

            if (defectColumnsOrder == null || defectColumnsOrder.Length == 0)
                throw new Exception("Defect 컬럼 순서 데이터는 null 일 수 없습니다.");

            System.Reflection.FieldInfo[] fieldInfoArr = new System.Reflection.FieldInfo[defectColumnsOrder.Length];

            // FieldInfo 배열 생성
            for (int i = 0; i < defectColumnsOrder.Length; i++)
            {
                fieldInfoArr[i] = typeof(Defect).GetField(defectColumnsOrder[i]);

                //if (fieldInfoArr[i] == null)
                //    throw new Exception(String.Format("Defect 클래스에서 '{0}' 라는 필드를 찾을 수 없습니다.", defectColumnsOrder[i]));
            }

            return Create2DArray(defectList, fieldInfoArr);
        }

        /// <summary>
        /// Defect 데이터를 2차원 배열로 가져옵니다.
        /// </summary>
        public string[,] GetImage2DArray(Parser.Klarf.Wafer wafer, string uploadFrom)
        {
            if (wafer.ImageDefectList == null || wafer.ImageDefectList.Count == 0)
                return new string[0, 0];

            List<string[]> list = new List<string[]>();

            // 이미지 파일 존재여부를 검사할지 여부 (Klarf 파일 생성 후 일정 시간이 지난 후에는 이미지 파일 존재 여부 체크하지 않음) 2019.10.16 Taihi,Kim.
            bool checkExistImage = false;// wafer.Parser.RequireErrorCheck();

            foreach (Defect d in wafer.ImageDefectList)
            {
                string imagePath = wafer.Parser.FtpRelativePath;

                if (d.IMAGECOUNT > 0 && d.Images != null && d.Images.Count > 0)
                {
                    for (int i = 0; i < d.Images.Count; i++)
                    {
                        string image = d.Images[i];

                        if (String.IsNullOrEmpty(image))
                            continue;

                        if (checkExistImage && !File.Exists(Path.Combine(wafer.Parser.DirectoryName, Path.GetFileName(image))))
                        {
                            //다음 주기 재시도의 경우 file 을 Error Backup 에 넣지 않는다.
                            wafer.Parser.ErrorBackupFlag = false;
                            throw new Exception(String.Format("이미지 파일({0})이 존재하지 않음. 다음 주기 재시도 예정", image));
                        }
                        
                        string[] data = new string[Enum.GetNames(typeof(ImageCol)).Length];

                        data[(int)ImageCol.STEP_SEQ] = d.STEP_SEQ.ToString();
                        data[(int)ImageCol.DEFECTID] = d.DEFECTID.ToString();
                        data[(int)ImageCol.IMAGE_ID] = i.ToString();
                        data[(int)ImageCol.WAFER_SEQ] = d.WAFER_SEQ.ToString();
                        data[(int)ImageCol.TEST] = d.TEST.ToString();
                        data[(int)ImageCol.IMAGE_TYPE] = System.IO.Path.GetExtension(image);
                        data[(int)ImageCol.IMAGE_PATH] = imagePath;
                        data[(int)ImageCol.IMAGE_FILENAME] = image;
                        data[(int)ImageCol.IMAGE_SERVER] = " ";
                        data[(int)ImageCol.THUMB_PATH] = imagePath;
                        data[(int)ImageCol.THUMB_FILEANME] = THUMBNAIL_PREFIX + image;
                        data[(int)ImageCol.UPLOAD] = uploadFrom;

                        list.Add(data);
                    }
                }
            }

            return Create2DArray(list);
        }

        protected string[,] Create2DArray(Base.DefectList defectList, System.Reflection.FieldInfo[] fieldInfoArr)
        {
            string[,] arr = new string[defectList.Count, fieldInfoArr.Length];

            for (int x = 0; x < defectList.Count; x++)
            {
                for (int y = 0; y < fieldInfoArr.Length; y++)
                {
                    if (fieldInfoArr[y] == null)
                        continue;

                    object val = fieldInfoArr[y].GetValue(defectList[x]);

                    arr[x, y] = (val != null) ? val.ToString() : String.Empty;
                }
            }
            return arr;
        }

        protected string[,] Create2DArray(List<string[]> list)
        {
            if (list.Count == 0)
                return null;

            string[,] arr = new string[list.Count, list[0].Length];

            for (int x = 0; x < list.Count; x++)
                for (int y = 0; y < list[0].Length; y++)
                    arr[x, y] = list[x][y];

            return arr;
        }

        /// <summary>
        /// 데이터를 회전할 각도를 가져옵니다. (EQUIP_CMF_1)
        /// </summary>
        /// <returns></returns>
        private int GetDataRotateAngle()
        {
            int angle;

            if (Int32.TryParse(EquipInfo.Cmf01, out angle))
                return angle;

            return 0;
        }

        /// <summary>
        /// 리뷰 설비에 특별히 지정할 폴더명을 가져옵니다. (EQUIP_CMF_2)
        /// </summary>
        public string GetReviewSpecialFolderName()
        {
            if (!String.IsNullOrEmpty(EquipInfo.Cmf02))
                return EquipInfo.Cmf02;

            return null;
        }

        /// <summary>
        /// CLUSTER 계산이 필요한지를 가져옵니다. (EQUIP_CMF_3)
        /// </summary>
        public bool NeedsCalculateCluster()
        {
            return (EquipInfo.Cmf03 == "Y");
        }

        /// <summary>
        /// CLUSTER로 판별할 최소 CLUSTER 갯수를 가져옵니다. (EQUIP_CMF_4)
        /// </summary>
        public int GetMinimunClusterCount()
        {
            int val;

            if (Int32.TryParse(EquipInfo.Cmf04, out val))
                return val;

            return MINIMUM_CLUSTER_COUNT;
        }

        /// <summary>
        /// CLUSTER로 판단할 한도 거리값 (um) (EQUIP_CMF_5)
        /// </summary>
        public double GetClusterThreadhold()
        {
            double val;

            if (Double.TryParse(EquipInfo.Cmf05, out val))
                return val;

            return CLUSTER_THRESHOLD;
        }

        /// <summary>
        /// NEW DEFECT인지를 판단할 한도 거리값 (um) (EQUIP_CMF_6)
        /// </summary>
        public double GetNewDefectTolerance()
        {
            double val;

            if (Double.TryParse(EquipInfo.Cmf06, out val))
                return val;

            return NEW_DEFECT_TOLERANCE;
        }

        public string ReviewCopyPath
        {
            get { return DACrux.Framework.Server.Utility.GetConfigValue("REVIEW_COPY_PATH"); }
        }
    }
}
