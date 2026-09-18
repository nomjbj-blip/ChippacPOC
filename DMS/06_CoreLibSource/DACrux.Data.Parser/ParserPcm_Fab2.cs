using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;
using System.Diagnostics;

namespace DACrux.Data.Parser
{
    /*
     * PCM Parser for FAB2
     * 
     * 검사항목이 row로, 테스트 DIE 단위가 column으로 표현
     * 
     *  Wafer ID: 1 	 #Slot: 1
        Item Name                                  1( 2,-1)   2( 4, 2)   3( 2, 2)   4(-1, 2)   5( 2, 5)    Avg        Std        Min        Max    
        Rc_M1C_NAA_0p15x0p15                     +4.961e+04 +4.829e+04 +4.815e+04 +4.726e+04 +4.945e+04 +4.855e+04 +8.767e+02 +4.726e+04 +4.961e+04
        Rc_M1C_NAA_0p16x0p16                     +4.045e+04 +3.988e+04 +3.991e+04 +3.895e+04 +4.037e+04 +3.991e+04 +5.312e+02 +3.895e+04 +4.045e+04
        Rc_M1C_NGC_0p15x0p15                     +4.199e+04 +4.198e+04 +4.305e+04 +3.927e+04 +4.275e+04 +4.181e+04 +1.338e+03 +3.927e+04 +4.305e+04
        Rc_M1C_NGC_0p16x0p16                     +3.408e+04 +3.433e+04 +3.537e+04 +3.243e+04 +3.472e+04 +3.418e+04 +9.806e+02 +3.243e+04 +3.537e+04
        Rc_M1C_PAA_0p15x0p15                     +4.856e+04 +4.802e+04 +4.726e+04 +4.461e+04 +4.911e+04 +4.751e+04 +1.575e+03 +4.461e+04 +4.911e+04
        Rc_M1C_PAA_0p16x0p16                     +3.968e+04 +3.989e+04 +3.941e+04 +3.716e+04 +3.984e+04 +3.920e+04 +1.034e+03 +3.716e+04 +3.989e+04
     */
    public class ParserPcm_Fab2 : ParserPcm
    {
        public static readonly string DATETIME_FORMAT = "MM/dd/yyyy/HH:mm:ss";
        private static readonly string MointorValue = "9.999e+25";
        //private static readonly string DefaultValue = "DEFAULT";
        enum ITEM { NAME = 0, SITE }

        private static Dictionary<string, string[]> dicPcmProgram = null;
        private static readonly Dictionary<string, Point> dicPcmMappingDic = null;
        private static readonly Dictionary<string, string> dicConvertItem = null;

        static ParserPcm_Fab2()
        {
            dicPcmMappingDic = new Dictionary<string, Point>();
            dicPcmMappingDic.Add("$L1", new Point() { X = 0, Y = 0 });
            dicPcmMappingDic.Add("$L2", new Point() { X = 0, Y = 1 });
            dicPcmMappingDic.Add("$L3", new Point() { X = 0, Y = 2 });
            dicPcmMappingDic.Add("$C1", new Point() { X = 1, Y = 0 });
            dicPcmMappingDic.Add("$C2", new Point() { X = 1, Y = 1 });
            dicPcmMappingDic.Add("$C3", new Point() { X = 1, Y = 2 });
            dicPcmMappingDic.Add("$R1", new Point() { X = 2, Y = 0 });
            dicPcmMappingDic.Add("$R2", new Point() { X = 2, Y = 1 });
            dicPcmMappingDic.Add("$R3", new Point() { X = 2, Y = 2 });
            dicPcmMappingDic.Add("$T1", new Point() { X = 0, Y = 0 });
            dicPcmMappingDic.Add("$T2", new Point() { X = 0, Y = 1 });
            dicPcmMappingDic.Add("$T3", new Point() { X = 0, Y = 2 });
            dicPcmMappingDic.Add("$B1", new Point() { X = 2, Y = 0 });
            dicPcmMappingDic.Add("$B2", new Point() { X = 2, Y = 1 });
            dicPcmMappingDic.Add("$B3", new Point() { X = 2, Y = 2 });

            dicConvertItem = new Dictionary<string, string>();
            dicConvertItem.Add(" ", "_");
            dicConvertItem.Add(".", "P");
            dicConvertItem.Add("/", "X");
            dicConvertItem.Add("-", "_");
            dicConvertItem.Add("+", "");
            dicConvertItem.Add(",", "_");
            dicConvertItem.Add("(", "_");
            dicConvertItem.Add(")", "");
        }

        public ParserPcm_Fab2(string fileName)
            : base(fileName)
        {
            try
            {
                GetXml();
                WaferDataList = new WaferDataList();

                string text = ParsingUtil.FileToString(fileName);
                string[] arr = text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                PcmWaferData currentWafer = null;
                Dictionary<string, string[]> dataDictionary = new Dictionary<string, string[]>();

                string[] headerArr = null;
                int dataCount = 0;

                foreach (string line in arr)
                {
                    if (line.Contains("Lot Summary"))
                        break;

                    if (line.Contains("Wafer ID:")) // Keysight 4072A
                    {
                        // Wafer 단위 End 인 경우 빈줄 올라옴
                        AppendData(currentWafer, dataDictionary, headerArr, dataCount);

                        //특정 Wafer ID: 상에 "\t" 가 없을때가 있어 예외 처리 함.
                        int slotID = int.Parse(GetValue(line, "Wafer ID", ":", line.IndexOf("\t") > -1 ? "\t" : "  ").Trim());

                        //// 새로운 WaferData 생성
                        currentWafer = new PcmWaferData(
                            string.Format("{0}-{1,2:D2}", LotID.Substring(0, 6), slotID)
                            );
                        WaferDataList.Add(currentWafer);
                    }
                    else if (line.Contains("Lot ID"))
                    {
                        LotID = GetValue(line, "Lot ID", " : ", "_");
                        if(String.IsNullOrEmpty(LotID))
                            LotID = GetValue(line, "Lot ID", " : ");
                        LotID = LotID.ToUpper();
                    }
                    else if (line.Contains("ProbeCard"))
                    {
                        ProbeCard = GetValue(line, "ProbeCard", " : ", " ");
                        OverDrive = GetValue(line, "OverDrive", " : ", " ");
                        Temp = GetValue(line, "Temp", " : ");
                    }
                    else if (line.Contains("Test Id"))
                    {
                        // FAB2는 프로그램명이 CP 이름과 중복되므로 P_ 를 붙인다.
                        ProgramName = GetValue(line, "Test Id", " : ").ToUpper();
                        ProgramName = string.Format("P_{0}", ProgramName);
                    }
                    else if (line.Contains("Test Time"))
                    {
                        string start = GetValue(line, "Test Time", " : ", "-");
                        string end = GetValue(line, "Test Time", "-");

                        StartTime = GetDateTime(start, DATETIME_FORMAT);
                        EndTime = GetDateTime(end, DATETIME_FORMAT);
                    }
                    else if (line.Contains("Equipment"))
                    {
                        EquipID = GetValue(line, "Equipment", " : ", " ");
                        ProbeID = GetValue(line, "Probe ID", " : ", " ");
                        Notch = GetValue(line, "Notch", " : ");
                        int notch = 0;
                        int.TryParse(Notch, out notch);
                        Notch = ((360 - (180 - notch)) % 360).ToString();
                    }
                    else if (line.Contains("Item Name"))
                    {
                        headerArr = line.Split(new string[] { "  " }, StringSplitOptions.RemoveEmptyEntries);
                        dataCount = headerArr.Length - 5; // 5 : Item Name, Avg, Std, Min, Max 제외
                    }
                    else if (currentWafer != null && dataCount > 0 && !String.IsNullOrWhiteSpace(line))
                    {
                        List<String> dataLst = new List<String>();
                        string[] dataArr = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int idx = 0; idx < dataArr.Length - 4; idx++)
                        {
                            if (dataArr[idx].Contains(MointorValue))
                                continue;

                            dataLst.Add(dataArr[idx]);
                        }
                        dataDictionary.Add(dataArr[0], dataLst.ToArray());
                    }
                }

                AppendData(currentWafer, dataDictionary, headerArr, dataCount);
            }
            catch (System.IO.IOException ioex)
            {
                //IO Error 의 경우 삭제 하지 않고 다음 주기에 다시 작업 한다.
                ErrorFlag = true;
                ErrorBackupFlag = false;
                ErrorMessage = ioex.Message + Environment.NewLine + ioex.StackTrace;
            }
            catch (Exception ex)
            {
                //Parsing Error 발생 시 Error Backup 에 넣는다.
                ErrorFlag = true;
                ErrorBackupFlag = true;
                ErrorMessage = ex.Message + Environment.NewLine + ex.StackTrace;
            }
        }

        private void AppendData(PcmWaferData currentWafer, Dictionary<string, string[]> dataDictionary, string[] headerArr, int dataCount)
        {
            if (currentWafer != null && dataDictionary.Count > 0)
            {
                List<string> dicKeys = new List<string>(dataDictionary.Keys);
                /// "FAB2_PCM_PROGRAM_LIST에 등록 또는 첫번째 ItemName 에서 '$'가 확인 된 경우
                if (dicPcmProgram.ContainsKey(ProgramName) || dicKeys[0].Contains("$"))
                {
                    String[] sites = null;
                    if (dicPcmProgram.ContainsKey(ProgramName))
                        sites = dicPcmProgram[ProgramName];
                    else sites = new String[] { "$L1", "$L2", "$L3", "$R1", "$R2", "$R3" }; // Defeulat Site Values

                    string[] items = null;
                    string itemName = string.Empty;
                    string site = string.Empty;
                    foreach (string key in dataDictionary.Keys)
                    {
                        items = key.Split(new char[] { '$' }, StringSplitOptions.RemoveEmptyEntries);
                        //itemName = items[(int)ITEM.NAME].Replace('.', 'P');
                        itemName = GetItem(items[(int)ITEM.NAME]);
                        /// Split 하면서 '$'가 치환되어 버림, 
                        /// 생성자에서 Setting 한 부분은 '$'가 포함되어 있어 키값을 맞춰주기 위해 포함
                        site = String.Format("${0}", items[(int)ITEM.SITE]);

                        Point p = dicPcmMappingDic[site];
                        int dieNum = Array.IndexOf(sites, site);
                        PcmShotData shotData = currentWafer.GetShotData(dieNum + 1, p.X, p.Y);
                        shotData.Add(new PcmTestData(itemName, dataDictionary[key][1]));
                    }
                }
                else // Default
                {
                    // 첫번째는 키, 두번째부터 데이터
                    for (int i = 1; i <= dataCount; i++)
                    {
                        PcmShotData shotData = new PcmShotData(headerArr[i]);
                        string itemName = string.Empty;
                        foreach (string key in dataDictionary.Keys)
                        {
                            if (i >= dataDictionary[key].Length)
                                break;

                            itemName = GetItem(key);
                            shotData.Add(new PcmTestData(itemName, dataDictionary[key][i]));
                        }

                        currentWafer.Add(shotData);
                    }
                }

                // 데이터 keys Clear
                dicKeys.Clear();

                // 데이터 Clear
                dataDictionary.Clear();
            }
        }

        private void GetXml()
        {
            dicPcmProgram = new Dictionary<string, string[]>();

            XmlDocument xDoc = new XmlDocument();
#if DEBUG
            xDoc.Load("FAB2_PCM_PROGRAM_INFO.XML");
#else
            /// 운영서버 서비스 절대경로
            xDoc.Load(@"D:\Miracom\DACrux\DACrux.TEST.PCM.DataService\FAB2_PCM_PROGRAM_INFO.XML");
#endif


            XmlNodeList nodes = xDoc.SelectNodes("/PROGRAM_LIST");

            String ID = String.Empty;
            String Site = String.Empty;
            foreach (XmlNode node1 in nodes)
            {
                foreach (XmlNode node2 in node1)
                {
                    ID = node2.Attributes["ID"].Value;
                    Site = node2.Attributes["SITE"].Value;
                    if (!dicPcmProgram.ContainsKey(ID))
                    {
                        dicPcmProgram.Add(ID, Site.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                    }
                }
            }

            xDoc = null;
        }

        private String GetItem(
            String item
            )
        {
            foreach (KeyValuePair<string, string> pv in dicConvertItem)
            {
                item = item.Replace(pv.Key, pv.Value);
            }
            return item;
        }


        public override void SetCorrection(
            int rotationAngle
            )
        {
            if (rotationAngle == 0)
                return;

            foreach (PcmWaferData wafer in WaferDataList)
            {
                for (int idx = 0; idx < wafer.Count; idx++)
                {
                    int x = Int32.Parse(wafer[idx].X);
                    int y = Int32.Parse(wafer[idx].Y);

                    Rotation(rotationAngle, ref x, ref y);

                    wafer[idx].X = x.ToString();
                    wafer[idx].Y = y.ToString();
                }
            }
        }

        private void Rotation(
            int angle,
            ref int x,
            ref int y
            )
        {
            if (angle == 0)
                return;

            while (angle < 0)
                angle += 360;

            int nx, ny;
            if (angle == 90)
            {
                nx = y;
                ny = -x;
            }
            else if (angle == 180)
            {
                nx = -x;
                ny = -y;
            }
            else if (angle == 270)
            {
                nx = -y;
                ny = x;
            }
            else
            {
                throw new Exception(String.Format("처리할 수 없는 각도({0}) 입니다.", angle));
            }

            x = nx;
            y = ny;
        }
    }

    public class PcmDieMapping : IComparable<PcmDieMapping>
    {
        public string Key { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int DieIndex { get; set; }

        public int CompareTo(PcmDieMapping obj)
        {
            if (DieIndex > obj.DieIndex)
                return 1;
            else if (DieIndex == obj.DieIndex)
                return 0;
            else
                return -1;
        }
    }

    public class PcmDieMappingList : List<PcmDieMapping>
    {
        public void Add(string key, int x, int y, int order)
        {
            Add(new PcmDieMapping() { Key = key, X = x, Y = y, DieIndex = order });
        }

        public bool Parse(string value, out string itemName, out int x, out int y, out int dieIndex)
        {
            itemName = null;
            x = y = dieIndex = 0;

            foreach (PcmDieMapping mapping in this)
            {
                int idx = value.LastIndexOf(mapping.Key);

                if (idx > 0 && idx == (value.Length - mapping.Key.Length))
                {
                    itemName = value.Substring(0, idx);
                    x = mapping.X;
                    y = mapping.Y;
                    dieIndex = mapping.DieIndex;

                    return true;
                }
            }

            return false;
        }
    }
}
