using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.Framework.Interface
{
    public interface iDACruxHitLog
    {
        void StartLog(string strFunctionCode, string strUserID, string strIPAddress, string strParameter);
        void EndLog(string strFunctionCode, string strUserID, string strIPAddress);
    }
}
