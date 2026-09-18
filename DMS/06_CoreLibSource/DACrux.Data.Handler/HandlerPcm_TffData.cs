using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DACrux.Data.Parser;
using System.Data;
using System.Diagnostics;
using System.IO;

namespace DACrux.Data.Handler
{
    public class HandlerPcm_TffData : HandlerPcm
    {
        protected override ParserBase CreateParser(string fileName)
        {
            try
            {
                ParserPcm_TffData parser = new ParserPcm_TffData(fileName);
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
