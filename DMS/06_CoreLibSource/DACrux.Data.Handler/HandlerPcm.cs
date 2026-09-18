using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DACrux.Data.Parser;
using System.Data;
using System.Diagnostics;
using DACrux.Framework.Server;

namespace DACrux.Data.Handler
{
    public abstract class HandlerPcm : Handler
    {
        public static readonly string USER_UPLOAD_EXT = ".user_upload";
        public static readonly string USER_UPLOAD_SEPARATOR = "@@";

        protected HandlerPcm()
        {
        }

        public override void Run()
        {
            /*
             * 파일 파싱 처리
             */

            int sleepTime = Utility.GetIntValueFromConfig("SLEEP_TIME", 0);

            for (int i = 0; i < FileNames.Length; i++)
            {
                // 서버 부하를 줄이기 위해 Sleep
                System.Threading.Thread.Sleep(sleepTime);

                string fileName = FileNames[i];
                WriteLog(String.Format("'{0}' 파일 처리 시작", fileName));

                ParserPcm parser = CreateParser(fileName) as ParserPcm;

                if (parser.ErrorFlag)
                {
                    ParserList.Add(parser);
                    continue;
                }

                // 사용자가 직접 업로드 한 파일인 경우 ProbeCard, Operator 정보를 가져오고 원래 파일명으로 되돌린다.
                if (IsUserUploadFile(fileName))
                {
                    string originalFileName, probeCard, operatorID;

                    if (GetUserUploadFileInformation(fileName, out originalFileName, out probeCard, out operatorID))
                    {
                        fileName = FileNames[i] = originalFileName;
                        parser.ProbeCard = probeCard;
                        //parser.Operator = operatorID;
                    }
                }

                // 백업 파일명 설정
                //parser.BackupDirectoryName = GetFullBackupPath(parser);
                parser.BackupFileName = Path.Combine(parser.BackupDirectoryName, Path.GetFileName(parser.FileName));
                parser.FtpRelativePath = GetImageRelativePath(parser.BackupDirectoryName);

                ParserList.Add(parser);
            }
        }

        protected DataTable AppendColumns(
            string[] appendColumnNames
            )
        {
            DataTable dt = new DataTable();

            if (appendColumnNames != null && appendColumnNames.Length > 0)
            {
                foreach (string cName in appendColumnNames)
                    dt.Columns.Add(cName, typeof(string));
            }

            return dt;
        }

        public DataTable GetTffDataToDataTable(
            ParserPcm_TffData parser, 
            string[] appendColumnNames = null
            )
        {
            DataTable dt = new DataTable();

            if (appendColumnNames != null && appendColumnNames.Length > 0)
            {
                foreach (string columnName in appendColumnNames)
                    dt.Columns.Add(columnName, typeof(string));
            }

            if (parser.ChipDataDef != null && parser.ChipDataDef.Length > 0)
            {
                foreach (string columnName in parser.ChipDataDef)
                    dt.Columns.Add(columnName, typeof(string));
            }

            foreach (var die in parser.DieDataList)
            {
                DataRow row = dt.NewRow();

                foreach (var data in die)
                    row[data.Name] = data.Value;

                dt.Rows.Add(row);
            }

            return dt;
        }

        public DataTable GetDataToDataTable(
            PcmWaferData pcmWaferData,
            string[] appendColumnName = null
            )
        {
            DataTable dt = AppendColumns(
                appendColumnName
                );

            //--

            if (!dt.Columns.Contains("X"))
                dt.Columns.Add(new DataColumn("X", typeof(string)));

            if (!dt.Columns.Contains("Y"))
                dt.Columns.Add(new DataColumn("Y", typeof(string)));

            if (!dt.Columns.Contains("BIN"))
                dt.Columns.Add(new DataColumn("BIN", typeof(string)));

            //--

            string columnName = string.Empty;
            for (int idx = 0; idx < pcmWaferData.Count; idx++)
            {
                /// column 생성
                foreach (PcmTestData tstData in pcmWaferData[idx])
                {
                    columnName = GetColumnName(tstData.Name);
#if DEBUG
                    Debug.WriteLine(String.Format("{0}", columnName));
#endif
                    if (!dt.Columns.Contains(columnName))
                        dt.Columns.Add(columnName);
                }

                /// row 생성
                DataRow dr = dt.NewRow();
                foreach (PcmTestData tstData in pcmWaferData[idx])
                {
                    if (string.IsNullOrEmpty(tstData.Name))
                        continue;

                    columnName = GetColumnName(tstData.Name);
                    dr[columnName] = tstData.Value;
                }

                dr["X"] = pcmWaferData[idx].X;
                dr["Y"] = pcmWaferData[idx].Y;
                dt.Rows.Add(dr);
            }

            return dt;
        }

        protected string GetColumnName(string name)
        {
            return name.Substring(
                name.IndexOf(':') + 1
                );
        }

        /// <summary>
        /// 사용자가 업로드한 파일인지를 가져옵니다.
        /// </summary>
        protected bool IsUserUploadFile(string fileName)
        {
            return (Path.GetExtension(fileName) == USER_UPLOAD_EXT);
        }

        /// <summary>
        /// 사용자가 업로드한 파일명에서 정보를 추출합니다.
        /// </summary>
        public bool GetUserUploadFileInformation(string fileName, out string originalFileName, out string probeCard, out string operatorID)
        {
            originalFileName = probeCard = operatorID = String.Empty;

            if (String.IsNullOrEmpty(fileName) || !Path.GetFileName(fileName).Contains(USER_UPLOAD_SEPARATOR))
                return false;

            fileName = Path.GetFileNameWithoutExtension(fileName);
            originalFileName = Path.GetFileNameWithoutExtension(fileName);
            string ext = Path.GetExtension(fileName).TrimStart('.');

            string[] arr = ext.Split(new string[] { USER_UPLOAD_SEPARATOR }, StringSplitOptions.None);

            if (arr == null || arr.Length != 2)
                return false;

            probeCard = arr[0];
            operatorID = arr[1];

            return true;
        }

        /// <summary>
        /// 사용자가 업로드한 파일인지를 식별할 수 있는 파일명을 가져옵니다.
        /// </summary>
        public static string GetUserUploadFileName(string fileName, string probeCard, string operatorID)
        {
            return String.Format("{0}.{1}{2}{3}{4}", fileName, probeCard, USER_UPLOAD_SEPARATOR, operatorID, USER_UPLOAD_EXT);
        }
    }
}
