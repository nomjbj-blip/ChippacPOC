using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DACrux.Data.Parser
{
    public class ParserTest : ParserBase
    {
        public ParserTest(string fileName)
            : base(fileName)
        {
        }

        public string EquipID { get; set; }
        public string ProgramName { get; protected set; }
        public string ProbeID { get; protected set; }
        public string Operator { get; set; }
        public string ProbeCard { get; set; }
        public string OverDrive { get; set; }
        public string Temp { get; set; }

        public DateTime StartTime { get; protected set; }
        public DateTime EndTime { get; protected set; }
    }
}
