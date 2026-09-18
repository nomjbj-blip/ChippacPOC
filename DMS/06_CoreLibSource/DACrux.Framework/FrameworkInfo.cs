using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DACrux.Framework
{
    public static class FrameworkInfo
    {
        public static string Product = "DACrux";
        public static string Version = "5";
        public static string SubVersion = "000";

        public static string ProductFullName
        {
            get
            {
                return string.Format("{0} V{1}.{2}", Product,Version,SubVersion);
            }
        }
    }
}
