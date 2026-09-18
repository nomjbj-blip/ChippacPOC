using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.Framework.Interface
{
    public interface iDACruxUserEnviroment
    {
        DataTable GetMyInfo(string strUserID);
        DataTable GetUserGroup (string strUserID);
        void UpdateMyInfo(string strUserID, string strPasswd, string strPhoneOffice, string strPhoneMobile, string strPhoneHome, string strPhoneEtc, string strEmail);
    }
}
