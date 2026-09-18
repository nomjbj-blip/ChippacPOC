using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DACrux.Framework.DSL;
using DACrux.Framework.Interface;

namespace DACrux.Framework.BSL
{
    public class ServiceLog : Miracom.Middleware.BaseComponent, iServiceLog
    {
        public enum Col
        {
            FACTORY,
            SERVICE_NAME,
            TRAN_KEY,
            SERVER_NAME,
            ACTION,
            FILE_NAME,
            EQUIP_ID,
            LOT_ID,
            WAFER_ID,
            HANDLER,
            EXECUTE_TIME,
            MESSAGE,
            DETAIL_MSG
        }

        public void InsertData(ServiceLogData data)
        {
            List<ServiceLogData> list = new List<ServiceLogData>();
            list.Add(data);

            InsertData(list);
        }

        public void InsertData(List<ServiceLogData> list)
        {
            string[,] arr = new string[list.Count, Enum.GetNames(typeof(Col)).Length];

            for (int i = 0; i < list.Count; i++)
            {
                arr[i, (int)Col.FACTORY] = TrimData(list[i].FACTORY, 20);
                arr[i, (int)Col.SERVICE_NAME] = TrimData(list[i].SERVICE_NAME, 50);
                arr[i, (int)Col.TRAN_KEY] = list[i].TRAN_KEY;
                arr[i, (int)Col.SERVER_NAME] = TrimData(list[i].SERVER_NAME, 50);
                arr[i, (int)Col.ACTION] = TrimData(list[i].ACTION, 50);
                arr[i, (int)Col.FILE_NAME] = TrimData(list[i].FILE_NAME, 500);
                arr[i, (int)Col.EQUIP_ID] = TrimData(list[i].EQUIP_ID, 50);
                arr[i, (int)Col.LOT_ID] = TrimData(list[i].LOT_ID, 50);
                arr[i, (int)Col.WAFER_ID] = TrimData(list[i].WAFER_ID, 500);
                arr[i, (int)Col.HANDLER] = TrimData(list[i].HANDLER, 50);
                arr[i, (int)Col.EXECUTE_TIME] = (list[i].EXECUTE_TIME != null && list[i].EXECUTE_TIME.HasValue) ? list[i].EXECUTE_TIME.ToString() : String.Empty;
                arr[i, (int)Col.MESSAGE] = TrimData(list[i].MESSAGE, 500);
                arr[i, (int)Col.DETAIL_MSG] = TrimData(list[i].DETAIL_MSG, 3000);
            }

            TQC_SERVICE_LOG obj = new TQC_SERVICE_LOG();
            obj.InsertData(arr);
        }

        private string TrimData(string text, int length)
        {
            if (String.IsNullOrEmpty(text))
                return text;

            return text.Substring(0, Math.Min(length, text.Length));
        }

        public string[] GetDataServiceName()
        {
            TQC_SERVICE_LOG obj = new TQC_SERVICE_LOG();
            return obj.GetDataServiceName();
        }

        public string[] GetDataServiceAction()
        {
            TQC_SERVICE_LOG obj = new TQC_SERVICE_LOG();
            return obj.GetDataServiceAction();
        }

        public byte[] GetDataServiceList_Comp(DateTime dtStart, DateTime dtEnd, string factory, object[] names, object[] actions, string equipid, string lotid)
        {
            return DACrux.Base.Util.ObjectToCompressedBytes(
                GetDataServiceList(dtStart, dtEnd, factory, names, actions, equipid, lotid)
                );
        }


        public DataTable GetDataServiceList(DateTime dtStart, DateTime dtEnd, string factory, object[] names, object[] actions, string equipid, string lotid)
        {
            TQC_SERVICE_LOG obj = new TQC_SERVICE_LOG();
            return obj.GetDataServiceList(
                dtStart, dtEnd, factory, Array.ConvertAll(names, x => x.ToString()), Array.ConvertAll(actions, x => x.ToString()), equipid, lotid
                );
        }
    }

    public class ServiceLogData
    {
        public static readonly string TIME_KEY_FORMAT = "yyyyMMddHHmmssffffff";

        private static object LockObject = new object();

        public ServiceLogData()
        {
            lock (LockObject)
            {
                TRAN_KEY = DateTime.Now.ToString(TIME_KEY_FORMAT);
                System.Threading.Thread.Sleep(1);
            }
        }

        public string TRAN_KEY { get; private set; }
        public string FACTORY { get; set; }
        public string SERVICE_NAME { get; set; }
        public string SERVER_NAME { get; set; }
        public string ACTION { get; set; }
        public string FILE_NAME { get; set; }
        public string EQUIP_ID { get; set; }
        public string LOT_ID { get; set; }
        public string WAFER_ID { get; set; }
        public string HANDLER { get; set; }
        public int? EXECUTE_TIME { get; set; }
        public string MESSAGE { get; set; }
        public string DETAIL_MSG { get; set; }
    }
}
