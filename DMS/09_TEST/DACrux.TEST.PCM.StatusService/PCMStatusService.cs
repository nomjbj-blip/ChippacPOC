using System;
using System.Data;
using System.IO;
using DACrux.Common.BSL;
using DACrux.Data.Handler;
using DACrux.Data.Parser;
using DACrux.TEST.BSL;
using System.Collections.Generic;

namespace DACrux.TEST.PCM.StatusService
{
    public partial class PCMStatusService : DACrux.Framework.Server.DataServiceBase
    {
        enum Data
        {
            STATUS, 
            LotID, 
            WaferID,
            PROGRAM
        };

        //public static readonly string TYPE_LOT = "LOT";
        //public static readonly string TYPE_WAFER = "WAFER";
        //public static readonly string STATUS_END = "END";
        public static readonly string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

        public PCMStatusService()
        {
            InitializeComponent();
        }

        protected override void Execute()
        {
            WriteLog("Execute()");

            string[] files = Directory.GetFiles(DataPath);

            if (files == null || files.Length == 0)
                return;

            TestCommon tst = new TestCommon();
            List<string> eqList = new List<string>();
            PcmEquipInfoList infoList = new PcmEquipInfoList();

            foreach (string file in files)
            {
                try
                {
                    // 확장자가 없는 파일만 처리
                    if (!String.IsNullOrEmpty(Path.GetExtension(file)))
                        continue;

                    string equipID = Path.GetFileName(file).ToUpper().Trim();
                    PcmEquipInfo info = tst.GetPcmEquipInfo(Factory, equipID);

                    // 없는 설비인 경우 RETURN
                    if (info == null)
                        continue;

                    eqList.Add(equipID);

                    string data = File.ReadAllText(file);

                    if (data == null)
                        continue;

                    string[] arr = data.Split(',');

                    string status = arr[0].Trim();

                    if (status == info.Status)
                        continue;

                    info.Status = status;
                    info.LotID = arr.Length > (int)Data.LotID ? arr[(int)Data.LotID].ToUpper().Trim() : String.Empty;
                    info.WaferID = arr.Length > (int)Data.WaferID ? arr[(int)Data.WaferID].ToUpper().Trim() : String.Empty;
                    info.Program = arr.Length > (int)Data.PROGRAM ? arr[(int)Data.PROGRAM].ToUpper().Trim() : String.Empty;

                    infoList.Add(info);
                }
                catch (Exception ex)
                {
                    WriteLog("ERROR 발생 : " + file);
                    WriteLog(ex);

                    AppendServiceLog(ex);
                }
            }

            tst.UpdatePcmEquipInfo(infoList);

            //if (infoList.Count > 0)
            //    AppendServiceLog(ActionType.SUCCESS, infoList.ToString());

            WriteLog("UPDATE", String.Join(",", eqList));
        }
    }
}
