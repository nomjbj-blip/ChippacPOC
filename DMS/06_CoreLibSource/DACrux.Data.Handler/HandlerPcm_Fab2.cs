using System;
using System.IO;
using DACrux.Data.Parser;

namespace DACrux.Data.Handler
{
    public class HandlerPcm_Fab2 : HandlerPcm
    {
        protected override ParserBase CreateParser(string fileName)
        {
            try
            {
                ParserPcm_Fab2 parser = new ParserPcm_Fab2(fileName);
                // 백업 파일명 설정
                parser.BackupDirectoryName = GetFullBackupPath(parser);
                return parser;
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Parsing Error: {0}", Path.GetFileName(fileName)), ex);
            }
        }
    }
}
