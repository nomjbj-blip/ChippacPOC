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
    public class HandlerPcm_Fab1 : HandlerPcm
    {
        protected override ParserBase CreateParser(string fileName)
        {
            try
            {
                ParserPcm_Fab1 parser = new ParserPcm_Fab1(fileName);
                /// Output File 이름에 Tester에 대한 장비명이 존재하지 않는 경우
                if (String.IsNullOrEmpty(parser.EquipID))
                    parser.EquipID = EquipInfo.EquipID;
                parser.ProbeCard = EquipInfo.Sts05;
                // 백업 파일명 설정
                parser.BackupDirectoryName = GetFullBackupPath(parser);
                return parser;
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Parsing Error: {0}", Path.GetFileName(fileName)), ex);
            }
        }

        protected override string GetFullBackupPath(ParserBase parser)
        {
            return Path.Combine(BackupPath, parser.GetPathForBackup());
        }
    }
}
