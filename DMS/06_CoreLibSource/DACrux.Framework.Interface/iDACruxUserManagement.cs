using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.Framework.Interface
{
    public interface iDACruxUserManagement
    {
        bool CheckID(string strUserID);
        void DeleteUser(string strUserID);
        DataTable ReadUserList();
        DataTable CheckAccount(string strUserID, string strUserPW);
        DataTable CheckAccount(string strFactory, string strUserID, string strUserPW, bool withMESSync);
        string GetUserName(string userID);
        bool UpdateUser(string strSQL, string strUserID);
        bool GetLicense(string IPAddress);
    }
}
