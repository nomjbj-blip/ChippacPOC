using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.Framework.Interface
{
    public interface iServiceLog
    {
        string[] GetDataServiceName();
        string[] GetDataServiceAction();
        byte[] GetDataServiceList_Comp(DateTime dtStart, DateTime dtEnd, string fractory, object[] names, object[] actions, string equipid, string lotid);
        DataTable GetDataServiceList(DateTime dtStart, DateTime dtEnd, string fractory, object[] names, object[] actions, string equipid, string lotid);

    }
}
