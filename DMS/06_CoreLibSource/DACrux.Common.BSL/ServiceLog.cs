using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DACrux.Common.DSL;

namespace DACrux.Common.BSL
{
    public class ServiceLog : Miracom.Middleware.BaseComponent
    {
        public enum Col
        {
            FACTORY,
            SERVICE_NAME,
            TRAN_KEY,
            SERVER_NAME,
            ACTION,
            EQUIP_ID,
            LOT_ID,
            WAFER_ID,
            HANDLER,
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
                arr[i, (int)Col.FACTORY] =      list[i].FACTORY;
                arr[i, (int)Col.SERVICE_NAME] = list[i].SERVICE_NAME;
                arr[i, (int)Col.TRAN_KEY] =     list[i].TRAN_KEY;
                arr[i, (int)Col.SERVER_NAME] =  list[i].SERVER_NAME;
                arr[i, (int)Col.ACTION] =       list[i].ACTION;
                arr[i, (int)Col.EQUIP_ID] =     list[i].EQUIP_ID;
                arr[i, (int)Col.LOT_ID] =       list[i].LOT_ID;
                arr[i, (int)Col.WAFER_ID] =     list[i].WAFER_ID;
                arr[i, (int)Col.HANDLER] =      list[i].HANDLER;
                arr[i, (int)Col.MESSAGE] =      list[i].MESSAGE;
                arr[i, (int)Col.DETAIL_MSG] =   list[i].DETAIL_MSG;
            }

            TQC_SERVICE_LOG obj = new TQC_SERVICE_LOG();
            obj.InsertData(arr);
        }
    }

    public class ServiceLogData
    {
        public string FACTORY { get; set; }
        public string SERVICE_NAME { get; set; }
        public string TRAN_KEY { get; set; }
        public string SERVER_NAME { get; set; }
        public string ACTION { get; set; }
        public string EQUIP_ID { get; set; }
        public string LOT_ID { get; set; }
        public string WAFER_ID { get; set; }
        public string HANDLER { get; set; }
        public string MESSAGE { get; set; }
        public string DETAIL_MSG { get; set; }
    }
}
