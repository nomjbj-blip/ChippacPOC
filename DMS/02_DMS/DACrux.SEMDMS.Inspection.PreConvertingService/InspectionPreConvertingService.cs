using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using DACrux.Data.Parser;
using DACrux.Framework.Server;
using DACrux.Data.Handler;


namespace DACrux.SEMDMS.Inspection.PreConvertingService
{
    /*
     * DB하이텍 DM 데이터 파일 PRE-CONVERTING SERVICE 2019.08.24 Taihi,Kim.
     * 
     * 1) TFF 파일인 경우 TFF2KLA 를 통해 KLARF 파일로 변환한다.
     * 2) 여러 WAFER 데이터가 하나의 파일로 업로드 되는 경우 REVIEW 장비에서 테스트가 가능하도록 각각의 WAFER 단위의 KLARF 파일로 변환
     * 3) 장비에서 파일명을 A라고 입력하여 업로드 하더라도 파일명이 자동으로 LOT_SLOT_LAYER.000 형태로 바뀔 수 있도록 구성
     */
    public partial class InspectionPreConvertingService : DACrux.Framework.Server.DataServiceBase
    {
        public static readonly int TFF2KLA_MAX_TIMEOUT = 10 * 1000; // 10초
        public static readonly string PATH_TFF2KLA_CONVERTER = @"TFF2KLA\TFF2KLA.EXE";
        public static readonly string KLARF_FILE_DEFAULT_EXT = ".KLA";

        public InspectionPreConvertingService()
        {
            InitializeComponent();
        }

        protected override void Execute()
        {
            WriteLog("Execute()");

#if SINGLE_EQUIP
            if (String.IsNullOrEmpty(EquipID))
                throw new Exception("EQUIP_ID 값이 설정되지 않았습니다.");
#endif
            string[] dirs = Directory.GetDirectories(DataPath, "*", SearchOption.AllDirectories);

            foreach (string dir in dirs)
            {
                string[] files = Directory.GetFiles(dir);

                if (files == null || files.Length == 0)
                    continue;

                // 디렉토리 배열
                string[] arr = dir.Split(Path.DirectorySeparatorChar);

                if (arr == null || arr.Length == 0)
                    continue;

                // 마지막 디렉토리가 설비ID
                string equipID = arr[arr.Length - 1];

                if (EquipID != equipID)
                    continue;

                // 원본 데이터 파일을 복사할지를 나타냅니다.
                if (!String.IsNullOrEmpty(OriginalFileBackupPath))
                {
                    if (!Directory.Exists(Path.Combine(OriginalFileBackupPath, equipID)))
                        Directory.CreateDirectory(Path.Combine(OriginalFileBackupPath, equipID));

                    foreach (string file in Directory.GetFiles(dir))
                        File.Copy(file, Path.Combine(OriginalFileBackupPath, equipID, Path.GetFileName(file)), true);
                }

                for (int i = 0; i < files.Length; i++)
                {
                    if (IsReqeustServiceStop)
                        break;

                    try
                    {
                        string fileName = files[i];
                        FileAnalyzer anal = new FileAnalyzer(fileName);

                        WriteLog(String.Format("'{0}' 파일 처리 시작", fileName));

                        string message;
                        StringBuilder sb = new StringBuilder();

                        // TFF 파일인 경우 TFF2KLA 를 통해 KLARF 파일로 변환한다.
                        if (anal.FileType == FileType.TffFile)
                        {
                            string newKlarfFile = Execute_TFF2KLA(fileName);
                            message = String.Format("TFF2KLA : {0} -> {1}", fileName, newKlarfFile);
                            WriteLog(message);
                            sb.AppendLine(message);

                            // TFF 파일 삭제
                            DeleteFile(equipID, fileName);

                            fileName = newKlarfFile;
                            anal = new FileAnalyzer(fileName);
                        }

                        if (anal.FileType == FileType.KlarfFile)
                        {
                            DACrux.Data.Parser.Klarf.ParserKlarf parser;
                            
                            if (!HandlerInsp.IsTrfKlarfFile(fileName))
                                parser = new Data.Parser.Klarf.ParserKlarf(fileName);
                            else
                                parser = new Data.Parser.Klarf.ParserKlarf_Trf(fileName);

                            string backupPath = Path.Combine(BackupPath, equipID);

                            // WAFER 단위로 나누어서 저장할 경우
                            if (parser.Wafers.Count > 1 && SplitByWafer)
                            {
                                    // 여러 WAFER 데이터가 하나의 파일로 업로드 되는 경우 REVIEW 장비에서 테스트가 가능하도록 각각의 WAFER 단위의 KLARF 파일로 변환
                                    // 장비에서 파일명을 A라고 입력하여 업로드 하더라도 파일명이 자동으로 LOT_SLOT_LAYER.000 형태로 바뀔 수 있도록 구성
                                    string[] outputFiles = parser.SaveFile_SplitByWafer(backupPath, true, Data.Parser.Klarf.FileNameRule.Lot_Slot_Layer_000);

                                    foreach (string outputFile in outputFiles)
                                    {
                                        message = String.Format("KLARF : {0} -> {1}", fileName, outputFile);
                                        WriteLog(message);
                                        sb.AppendLine(message);
                                    }
                            }
                            else
                            {
                                string destFileName = Path.GetFileName(fileName);

                                if (!HandlerInsp.IsTrfKlarfFile(fileName))
                                    destFileName = parser.GetGeneralFileName(Data.Parser.Klarf.FileNameRule.Lot_Slot_Layer_000, 0);
                                
                                destFileName = Path.Combine(backupPath, destFileName);

                                CopyFile(fileName, destFileName);

                                message = String.Format("KLARF : {0} -> {1}", fileName, destFileName);
                                WriteLog(message);
                                sb.AppendLine(message);
                            }

                            AppendServiceLog(ActionType.SUCCESS, null, sb.ToString(), fileName);
                        }
                        else // KLARF 파일이 아닌 경우 INSP_PARSE 폴더로 그대로 복사한다. 2019.10.15 Taihi,Kim.
                        {
                            string destFileName = Path.Combine(BackupPath, equipID, Path.GetFileName(fileName));
                            CopyFile(fileName, destFileName);

                            message = String.Format("OTHER : {0} -> {1}", fileName, destFileName);
                            WriteLog(message);
                        }

                        // 파일 삭제
                        DeleteFile(equipID, fileName);
                    }
                    catch (Exception ex)
                    {
                        AppendServiceLog(ex, equipID);

                        WriteLog("ERROR 발생 : " + files[i]);
                        WriteLog(ex);
                    }
                }
            }
        }

        private void CopyFile(string sourceFileName, string destFileName)
        {
            if (!Directory.Exists(Path.GetDirectoryName(destFileName)))
                Directory.CreateDirectory(Path.GetDirectoryName(destFileName));

            File.Copy(sourceFileName, destFileName, true);
        }

        private void DeleteFile(string equipID, string fileName)
        {
            try
            {
                string localBackupPath = GetConfigValue("LOCAL_BACKUP_PATH");

                if (!String.IsNullOrEmpty(localBackupPath))
                {
                    if (!Directory.Exists(localBackupPath))
                        Directory.CreateDirectory(localBackupPath);

                    string backupFileName = String.Format("{0}_{1}_{2}", equipID, DateTime.Now.ToString("yyyyMMddHHmmssFFF"), Path.GetFileName(fileName));
                    WriteLog(String.Format("BACKUP : {0} -> {1}", Path.GetFileName(fileName), backupFileName));
                    File.Copy(fileName, Path.Combine(localBackupPath, backupFileName), true);
                }

                File.Delete(fileName);
            }
            catch (Exception ex)
            {
                WriteLog("파일 삭제 에러 : " + fileName);
                WriteLog(ex);
            }
        }

        private string GetUniqueNewFileName(string[] files, string fileName, string newExt)
        {
            int i = 0;

            fileName = GetFullPathWithoutExt(fileName);
            string newFileName = GetFullPathWithoutExt(fileName) + newExt;

            while (files.Contains(newFileName))
            {
                newFileName = String.Format("{0}({1}){2}", fileName, ++i, newExt); // 18203_02.kla -> 18203_02(1).kla
            }

            return newFileName;
        }

        /// <summary>
        /// 확장자만 제외한 파일명을 포함하는 전체 경로를 가져옵니다.
        /// </summary>
        private string GetFullPathWithoutExt(string fileName)
        {
            int idx = fileName.LastIndexOf('.');

            if (idx < 0)
                return fileName;
            else
                return fileName.Substring(0, idx);
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

            Process proc = null;

            try
            {
                proc = Process.Start(start);

                // TFF2KLA 프로세스 응답 없는 경우 처리 2019.12.04 Taihi,Kim.
                if (!proc.WaitForExit(TFF2KLA_MAX_TIMEOUT))
                    throw new Exception("TFF2KLA 변환 중 응답 없음 발생. 해당 파일은 다음 주기에 처리 예정 입니다." + targetFile);
            }
            finally
            {
                if (proc != null && !proc.HasExited)
                    proc.Kill();
            }

            if (!File.Exists(targetFile))
                throw new Exception("TFF2KLA에서 변환후의 출력 파일을 찾을 수 없습니다. " + targetFile);

            return targetFile;
        }

        /// <summary>
        /// KLARF 파일을 WAFER 단위로 나눌지를 나타냅니다.
        /// </summary>
        public bool SplitByWafer
        {
            get 
            {
                string val = Utility.GetConfigValue("SPLIT_BY_WAFER");
                return String.IsNullOrEmpty(val) ? false : val.ToUpper() == Boolean.TrueString.ToUpper();
            }
        }
    }
}
