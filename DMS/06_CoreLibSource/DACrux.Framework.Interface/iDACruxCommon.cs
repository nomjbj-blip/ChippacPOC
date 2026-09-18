using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.Framework.Interface
{
    public interface iDACruxCommon
    {
        DataTable GetDynamic(string strSQL);
        void ExcuteDynamic(string strSQL);
        string GetConfigValue(string strCategory, string strName);
        DataTable GetConfigList(string strCategory);
    }
}
