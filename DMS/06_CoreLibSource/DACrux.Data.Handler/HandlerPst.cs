using System;
using System.Collections.Generic;
using System.Text;
using DACrux.Data.Parser;
using System.IO;
using System.Data;
using System.Net;

namespace DACrux.Data.Handler
{
    /// <summary>
    /// pst 형식 및 cp3 형식에 대한 Parsing
    /// </summary>
    public class HandlerPst : HandlerCp
    {
        protected override ParserBase CreateParser(string fileName)
        {
            try
            {
                return new ParserPst(fileName);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Parsing Error: {0}", Path.GetFileName(fileName)), ex);
            }
        }
    }
}
