using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DACrux.Data.Parser;
using System.IO;

namespace DACrux.Data.Handler
{
    public class HandlerPcm_Fab2_MEMS : HandlerPcm
    {
        protected override ParserBase CreateParser(string fileName)
        {
            try
            {
                ParserPcm_Fab2_MEMS parser = new ParserPcm_Fab2_MEMS(fileName);
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
