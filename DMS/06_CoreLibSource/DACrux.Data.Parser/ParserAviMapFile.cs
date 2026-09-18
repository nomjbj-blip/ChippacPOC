using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DACrux.Data.Parser
{
    public class ParserAviMapFile : ParserBase
    {
        public static readonly int ANGLE_BOTTOM = 180;
        public static readonly char SEPARATOR_KEY_VALUE = ':';
        public static readonly char SEPARATOR_DATA = ' ';
        public static readonly string[] EMPTY_DIE = new string[] {"__"};
        public static readonly string DATA = "RowData";
        private FileInfo oAVIFile = null;

        // 단계로 처리
        public ParserAviMapFile(string fileName)
            : base(fileName)
        {
            try
            {
                oAVIFile = new FileInfo(fileName);
                if (oAVIFile.Exists == false)
                    throw new Exception(string.Format("File Is Not Exists : {0}", fileName));

                if(oAVIFile.Length == 0)
                    throw new Exception(string.Format("File Length is Zero : {0}", fileName));

                string text = ParsingUtil.FileToString(fileName);
                string[] lines = text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                Dictionary<string, string> dic;
                List<string> dataList;
                GetData(lines, out dic, out dataList);

                if (dic == null || dic.Count == 0 || dataList == null || dataList.Count == 0)
                    return;

                DEVICE = GetString(dic, "DEVICE");
                LotID = GetString(dic, "LOT");
                WAFER = GetString(dic, "WAFER");
                FNLOC = GetInt(dic, "FNLOC");
                ROWCT = GetInt(dic, "ROWCT");
                COLCT = GetInt(dic, "COLCT");
                BCEQU = GetString(dic, "BCEQU");
                REFPX = GetInt(dic, "REFPX");
                REFPY = GetInt(dic, "REFPY");
                DUTMS = GetString(dic, "DUTMS");
                XDIES = GetDouble(dic, "XDIES");
                YDIES = GetDouble(dic, "YDIES");

                DieList = new List<Die>();
                BadDieList = new List<Die>();

                if (dataList.Count != ROWCT)
                    throw new Exception(String.Format("ROWCT({0})와 Row개수({1})가 맞지 않습니다. {2}", ROWCT, dataList.Count, ToString()));

                for (int y = 0; y < dataList.Count; y++)
                {
                    string[] arr = dataList[y].Split(SEPARATOR_DATA);

                    if (arr.Length != COLCT)
                        throw new Exception(String.Format("COLCT({0})와 Column개수({1})가 맞지 않습니다. {2}", COLCT, arr.Length, ToString()));

                    for (int x = 0; x < arr.Length; x++)
                    {
                        //Empty Die 를 __ 또는 4D 일 경우로 정의 한다.
                        if (Array.IndexOf(EMPTY_DIE, arr[x]) > -1)
                            continue;

                        int dieX = x + 1;
                        int dieY = ROWCT - y;
                        //2019-07-16 : FAB2의 경우 전부 Bottom 으로 설정되어 있다. 장동윤 책임.
                        //             따라서 Map 은 그대로 Bottom 으로 Map을 저장 한다.
                        //Rotate(ANGLE_BOTTOM - FNLOC, ROWCT, COLCT, ref dieX, ref dieY); 

                        Die die = new Die(dieX, dieY, arr[x]);

                        DieList.Add(die);

                        if (!BCEQU.Contains(arr[x]))
                            BadDieList.Add(die);

                        INDEX_XMAX = Math.Max(INDEX_XMAX, dieX);
                        INDEX_YMAX = Math.Max(INDEX_YMAX, dieY);
                        INDEX_XMIN = Math.Min(INDEX_XMIN, dieX);
                        INDEX_YMIN = Math.Min(INDEX_YMIN, dieY);
                    }
                }
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

        private void GetData(string[] lines, out Dictionary<string, string> dic, out List<string> dataList)
        {
            dic = new Dictionary<string, string>();
            dataList = new List<string>();

            foreach (string line in lines)
            {
                if (String.IsNullOrEmpty(line))
                    continue;

                int index = line.IndexOf(SEPARATOR_KEY_VALUE.ToString());

                if (index < 0)
                    continue;

                string key = line.Substring(0, index);
                string value = line.Substring(index + 1).Trim();

                if (key == DATA)
                    dataList.Add(value);
                else if (dic.ContainsKey(key))
                    throw new Exception(String.Format("동일한 키({0})의 데이터가 이미 존재합니다.", key));
                else
                    dic.Add(key, value);
            }
        }

        public override string ToString()
        {
            return String.Format("LOT:{0}, WAFER:{1}", LotID, WAFER);
        }

        public override DateTime GetBackupDateTime()
        {
            return oAVIFile.CreationTime;
        }


        #region 프로퍼티

        /// <summary>
        /// DEVICE
        /// </summary>
        public string DEVICE { get; private set; }
        
        /// <summary>
        ///  WAFER ID
        /// </summary>
        public string WAFER { get; private set; }
        
        /// <summary>
        /// wafer flat position (0=TOP,90=RIGHT,180=BOT 270=LEFT)
        /// </summary>
        public int FNLOC { get; private set; }

        /// <summary>
        /// number of rows
        /// </summary>
        public int ROWCT { get; private set; }
        
        /// <summary>
        /// number of columns
        /// </summary>
        public int COLCT { get; private set; }
        
        /// <summary>
        /// List of Bin Codes that are good die
        /// </summary>
        public string BCEQU { get; private set; }
        
        /// <summary>
        /// x-coord of reference die (optional)
        /// </summary>
        public int REFPX { get; private set; }
        
        /// <summary>
        /// y-coord of reference die (optional)
        /// </summary>
        public int REFPY { get; private set; }
        
        /// <summary>
        /// die units of measurement (mm or mil)
        /// </summary>
        public string DUTMS { get; private set; }
        
        /// <summary>
        /// step along X
        /// </summary>
        public double XDIES { get; private set; }
        
        /// <summary>
        /// step along Y
        /// </summary>
        public double YDIES { get; private set; }

        public List<Die> DieList { get; private set; }

        public List<Die> BadDieList { get; private set; }

        public int INDEX_XMAX { get; private set; }
        public int INDEX_YMAX { get; private set; }
        public int INDEX_XMIN { get; private set; }
        public int INDEX_YMIN { get; private set; }

        #endregion

        public class Die
        {
            public Die(int x, int y, string bin)
            {
                X = x;
                Y = y;
                BIN = bin;
            }
            
            public override string ToString()
            {
                return String.Format("{0},{1} {2}", X, Y, BIN);
            }

            public int X { get; internal set; }
            public int Y { get; internal set; }
            public string BIN { get; internal set; }
            public FileInfo[] IMAGE { get; set; }
            public string IMAGE_BACKUP { get; set; }
            public string THUMENAIL { get; set; }
            public string THUMENAIL_BACKUP { get; set; }
        }
    }
}
