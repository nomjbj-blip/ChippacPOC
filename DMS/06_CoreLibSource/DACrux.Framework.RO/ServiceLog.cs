using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DACrux.Framework.Interface;
using System.Data;

namespace DACrux.Framework.RO
{
    public class ServiceLog
    {
        iServiceLog m_OBJ = null;
        public ServiceLog()
        {
            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_QMS_COMMON);
            object obj = Activator.GetObject(typeof(DACrux.Framework.Interface.iServiceLog),
                strUrl + "/DACrux.Framework.BSL.ServiceLog.bin");
            m_OBJ = obj as DACrux.Framework.Interface.iServiceLog;
            System.Configuration.ConfigurationManager.GetSection("System.Diagnostics");
        }

        public string[] GetDataServiceName()
        {
            return m_OBJ.GetDataServiceName();
        }

        public string[] GetDataServiceAction()
        {
            return m_OBJ.GetDataServiceAction();
        }

        public DataTable GetDataServiceList_Comp(DateTime dtStart, DateTime dtEnd, string factory, object[] names, object[] actions, string equipid, string lotid)
        {
            byte[] obj = m_OBJ.GetDataServiceList_Comp(dtStart, dtEnd, factory, names, actions, equipid, lotid);
            return DACrux.Base.Util.CompressedBytesToObject(obj) as DataTable;
        }

        public DataTable GetDataServiceList(DateTime dtStart, DateTime dtEnd, string factory, object[] names, object[] actions, string equipid, string lotid)
        {
            return GetDataServiceList_Comp(dtStart, dtEnd, factory, names, actions, equipid, lotid);
        }
    }
}
