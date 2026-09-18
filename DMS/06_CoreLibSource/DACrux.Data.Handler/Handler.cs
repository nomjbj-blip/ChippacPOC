using System;
using System.Collections.Generic;
using DACrux.Data.Parser;
using System.IO;

namespace DACrux.Data.Handler
{
    public abstract class Handler
    {
        #region 멤버 변수

        public delegate void LogDelegate(string message);
        public static readonly string FAB1 = "FAB1";
        public static readonly string FAB2 = "FAB2";

        /// <summary>
        /// 한번에 처리할 DATA FILE의 개수
        /// </summary>
        public static readonly int DATA_FILE_LIMIT_COUNT = 100;

        public static readonly string THUMBNAIL_PREFIX = "thumb_";

        public static readonly string TO_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

        public static readonly string BACKUP_PATH = "BACKUP_PATH";

        private static string m_backupPath;
        
        #endregion

        #region 생성자

        protected Handler()
        {
            ParserList = new ParserList();
        }
        
        #endregion

        #region 사용자 정의 메서드

        protected void WriteLog(string message)
        {
            if (LogMethod != null)
                LogMethod(message);
        }

        /// <summary>
        /// 데이터 파일 파싱 처리를 실행합니다.
        /// </summary>
        public virtual void Run()
        {
            throw new NotImplementedException("Run() 메서드는 자식 클래스에서 반드시 구현하여야 합니다.");
        }

        /// <summary>
        /// 데이터를 백업 위치에 저장 합니다.
        /// </summary>
        public virtual void Backup(ParserBase parser)
        {
            // 에러 발생한 데이터 파일은 처리하지 않음
            if (parser.ErrorFlag)
                return;

            if (String.IsNullOrEmpty(parser.BackupFileName))
                throw new Exception("BackupFileName 이 null 입니다.");

            // 경로

            if (!Directory.Exists(parser.BackupDirectoryName))
                Directory.CreateDirectory(parser.BackupDirectoryName);

            FileCopy(parser.FileName, parser.BackupFileName);
        }

        /// <summary>
        /// 원본 데이터 파일을 추가 복사 합니다.
        /// </summary>
        public virtual void AddtionalCopy(ParserBase parserBase, string copyPath)
        {
            throw new NotImplementedException("AddtionalCopy() 메서드는 자식 클래스에서 별도로 구현하여야 합니다.");
        }
        
        /// <summary>
        /// 원본 데이터 파일을 삭제합니다.
        /// </summary>
        public virtual void Remove(ParserBase parser)
        {
            // 에러 발생한 데이터 파일은 처리하지 않음
            if (parser.ErrorFlag)
                return;

            FileInfo oDelFile = new FileInfo(parser.FileName);
            if (oDelFile != null && oDelFile.Exists)
            {
                try
                {
                    oDelFile.IsReadOnly = false;
                }
                catch { }

                // 데이터 파일 삭제
                oDelFile.Delete();
            }
        }

        /// <summary>
        /// Backup 전에 Backup 경로에 동일한 File 이 있을 경우 삭제 후 Copy 한다.
        /// </summary>
        public void FileCopy(string strOriFile, string strBackupFile)
        {
            string dir = Path.GetDirectoryName(strBackupFile);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            FileInfo oDelFile = new FileInfo(strBackupFile);
            
            if (oDelFile.Exists)
            {
                try
                {
                    oDelFile.IsReadOnly = false;
                }
                catch { }
                File.Delete(strBackupFile);
            }

            // 데이터 파일 복사
            File.Copy(strOriFile, strBackupFile, true);
        }

        protected virtual ParserBase CreateParser(string fileName)
        {
            throw new NotImplementedException("GetParser() 메서드는 자식 클래스에서 반드시 구현하여야 합니다.");
        }

        /// <summary>
        /// 전체 Backup 경로를 가져옵니다.<para/>
        /// ex) X:\BACKUP\DM\INSP\2019\07\05\{LOTID}
        /// </summary>
        protected virtual string GetFullBackupPath(ParserBase parser)
        {
            return Path.Combine(BackupPath, parser.GetPathForBackup());
        }

        /// <summary>
        /// 디렉토리 경로를 FTP 상대 경로로 가져옵니다.<para/>
        /// ex) X:\BACKUP\DM\INSP\2019\07\05\{LOTID} ==> BACKUP/DM/INSP/2019/07/05/{LOTID}
        /// </summary>
        protected string GetImageRelativePath(string path)
        {
            path = path.Replace(Path.GetPathRoot(path), String.Empty);
            return path.Replace('\\', '/');
        }
        
        #endregion

        #region 프로퍼티

        public ParserList ParserList
        {
            get;
            private set;
        }

        public string[] FileNames
        {
            get;
            internal set;
        }

        public EquipInfo EquipInfo
        {
            get;
            internal set;
        }

        public static string BackupPath
        {
            get
            {
                if (String.IsNullOrEmpty(m_backupPath))
                {
                    if (Array.IndexOf(System.Configuration.ConfigurationManager.AppSettings.AllKeys, BACKUP_PATH) < 0)
                        throw new Exception(String.Format("서비스.exe.config 파일에 '{0}' 값이 설정되지 않았습니다.", BACKUP_PATH));

                    m_backupPath = System.Configuration.ConfigurationManager.AppSettings[BACKUP_PATH];
                }

                return m_backupPath;
            }
        }

        public string Name
        {
            get { return GetType().Name; }
        }

        public LogDelegate LogMethod
        {
            get;
            set;
        }

        #endregion
    }

    public class HandlerList : List<Handler>
    {
    }

    public class EquipInfo
    {
        public static readonly string YES = "Y";

        public override string ToString()
        {
            return String.Format("{0} / {1} / {2} / {3}", EquipID, Area, Oper, EquipModel);
        }

        public string GetRelativePath()
        {
            return String.Format(@"{0}\{1}", Area, Oper);
        }

        public string Factory { get; set; }
        public string EquipID { get; set; }
        public string Area { get; set; }
        public string Oper { get; set; }
        public string EquipModel { get; set; }
        public string Handler { get; set; }

        public string Grp01 { get; set; }
        public string Grp02 { get; set; }
        public string Grp03 { get; set; }
        public string Grp04 { get; set; }
        public string Grp05 { get; set; }
        public string Grp06 { get; set; }
        public string Grp07 { get; set; }
        public string Grp08 { get; set; }
        public string Grp09 { get; set; }


        // 설비에 대한 상태정보 값
        public string Sts01 { get; set; }
        public string Sts02 { get; set; }
        public string Sts03 { get; set; }
        public string Sts04 { get; set; }
        public string Sts05 { get; set; }
        public string Sts06 { get; set; }
        public string Sts07 { get; set; }
        public string Sts08 { get; set; }
        public string Sts09 { get; set; }

        public string Cmf01 { get; set; }
        public string Cmf02 { get; set; }
        public string Cmf03 { get; set; }
        public string Cmf04 { get; set; }
        public string Cmf05 { get; set; }
        public string Cmf06 { get; set; }
        public string Cmf07 { get; set; }
        public string Cmf08 { get; set; }
        public string Cmf09 { get; set; }
        public string Cmf10 { get; set; }

        /// <summary>
        /// Review 파일 생성을 LOTEND 파일때마다 생성할지를 나타냅니다.
        /// </summary>
        public bool IsCreateReviewFileAtLotEnd()
        {
            return Cmf07 == YES;
        }
    }
}
