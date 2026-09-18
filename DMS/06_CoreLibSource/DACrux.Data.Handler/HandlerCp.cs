using System;
using System.Collections.Generic;
using System.Text;
using DACrux.Data.Parser;
using System.IO;
using System.Data;
using DACrux.Framework.Server;

namespace DACrux.Data.Handler
{
    public class HandlerCp : Handler
    {
        protected override ParserBase CreateParser(string fileName)
        {
            try
            {
                return new ParserCp(fileName);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Parsing Error: {0}", Path.GetFileName(fileName)), ex);
            }
        }

        public override void Run()
        {
            /*
             * 파일 파싱 처리
             */

            if (FileNames == null || FileNames.Length == 0)
                return;

            int sleepTime = Utility.GetIntValueFromConfig("SLEEP_TIME", 0);

            foreach (string fileName in FileNames)
            {
                // 서버 부하를 줄이기 위해 Sleep
                System.Threading.Thread.Sleep(sleepTime);

                WriteLog(String.Format("'{0}' 파일 처리 시작", fileName));

                ParserCp parser = CreateParser(fileName) as ParserCp;

                if (parser.ErrorFlag)
                {
                    ParserList.Add(parser);
                    continue;
                }

                // 백업 파일명 설정
                parser.BackupDirectoryName = GetFullBackupPath(parser);
                parser.BackupFileName = Path.Combine(parser.BackupDirectoryName, Path.GetFileName(parser.FileName));
                parser.FtpRelativePath = GetImageRelativePath(parser.BackupDirectoryName);

                ParserList.Add(parser);
            }
        }

        /// <summary>
        /// 데이터를 DataTable로 가져옵니다.
        /// </summary>
        public DataTable GetDataToDataTable(ParserCp parser, string[] appendColumnNames = null)
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
    }
}
