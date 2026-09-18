using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;

namespace FDll
{
    public class ISORTdll : Fdll
    {
        public string productName = string.Empty;
        public string waferID = string.Empty;
        public string lotID = string.Empty;
        public int xmin = 1;
        public int ymin = 1;
        public int xmax = 0;
        public int ymax = 0;
        public int xdie = 0;
        public int ydie = 0;
        public int originX = 0;
        public int originY = 0;
        public int targetDieX = 0;
        public int targetDieY = 0;
        //public int ChipSizeX = 1;
        //public int ChipSizeY = 1;
        //public int XDies = 0;
        //public int YDies = 0;
        public int angle = 180;
        public int netDie = 0;
        public int waferSize = 0;
        public double dieSizeX = 1;
        public double dieSizeY = 0;
        //public int ReferenceDieSetting = 0;
        public int testDies = 0;
        public int notchType = 0;
        //public int TargetDieX = 1;
        //public int TargetDieY = 1;
        //public int EdgeSize = 0;

        public DataTable dtRowData = null;

        protected override void DataSetToStruct() { }
        protected override void DataSetToStruct(string LotWafer) { }
        protected override void StructToDataSet() { }
        protected override int OutTxtFile(string formatFilePath, string outFilePath) { return 1; }
        protected override DataSet OutTxtFile1(string formatFilePath, string outFilePath) { return null; }
        protected override int InputTxtFile(string regFilePath, string inputFile) { return 1; }

        public void ReadISORTFile(string filePath, ref HeaderData hd, ref string[] arrBinData, ref string[] arrBinLayout)
        {
            string strFileText = string.Empty;
            string strX_MaxMatch = string.Empty;
            string strY_MaxMatch = string.Empty;
            string strBinMatch = string.Empty;

            try
            {
                // File Read
                Encoding encode = System.Text.Encoding.GetEncoding("UTF-8");
                FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(file, encode);
                strFileText = sr.ReadToEnd();
                sr.Close();

                string BinPattern = @"^(\[START OF INSPECTION \/ INTAPE\][\s]*COLNUM\=[0-9 ]*)(?<value>[A-Z0-9\s\=]*)";
                string XPattern = "X DIES?=[ ]?(?<value>[0-9.]+)";
                string YPattern = "Y DIES?=[ ]?(?<value>[0-9.]+)";
                //string LotPattern = @"(?<value>[a-zA-Z0-9_]*)[\-]*[0-9]*.isort";
                //string WaferPattern = @"[a-zA-Z0-9_]*[\-]*(?<value>[0-9]*).isort";
                string BinPatternShape = @"^(\[ORIGINAL MAP\][\s]*COLNUM\=[0-9 ]*)(?<value>[A-Z0-9\s\=_\*]*)";

                // X Max
                foreach (Match match in Regex.Matches(strFileText, XPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline))
                    strX_MaxMatch = match.Groups["value"].Value;

                // YMax
                foreach (Match match in Regex.Matches(strFileText, YPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline))
                    strY_MaxMatch = match.Groups["value"].Value;

                // Y  줄 개수 만큼만 Split
                // BIN
                List<string> temp = new List<string>();
                foreach (Match match in Regex.Matches(strFileText, BinPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled))
                    strBinMatch = strFileText.Substring(match.Groups["value"].Index, match.Groups["value"].Length);

                string strBinPattern = string.Empty;
                foreach (Match match in Regex.Matches(strFileText, BinPatternShape, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled))
                    strBinPattern = strFileText.Substring(match.Groups["value"].Index, match.Groups["value"].Length);
                
                string colNo = string.Empty;
                for (int i = 1; i <= Convert.ToInt32(strY_MaxMatch); i++)
                {
                    colNo = "ROW" + i.ToString().PadLeft(3, '0') + "=";

                    strBinMatch = strBinMatch.Replace(colNo, string.Empty);
                    strBinPattern = strBinPattern.Replace(colNo, string.Empty);
                }

                if (strBinMatch.StartsWith("\r\n"))
                {
                    strBinMatch = strBinMatch.Substring(2, strBinMatch.Length - 4);
                    strBinPattern = strBinPattern.Substring(2, strBinPattern.Length - 4);
                }

                arrBinData = strBinMatch.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                arrBinLayout = strBinPattern.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                // Header Info Setting
                string[] arrTemp = filePath.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries);
                string[] arrName = arrTemp[arrTemp.Length - 1].Split('_');
                hd.ReelID = arrName[0];
                arrName[1] = arrName[1].Replace(".isort","");
                hd.LotNo = arrName[1].Split('-')[0];
                hd.WaferID = arrName[1].Split('-')[1];
                this.xmax = Int32.Parse(strX_MaxMatch);
                this.ymax = Int32.Parse(strY_MaxMatch);
                this.XDies = this.xmax - this.xmin + 1;
                this.YDies = this.ymax - this.ymin + 1;
                hd.StartTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                hd.EndTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                hd.DieIndexMaxX = xmax;
                hd.DieIndexMaxY = ymax;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[,] WriteRowData(ref DataTable dt, ref HeaderData hd, ref RowData[] rd)
        {
            rd = new RowData[hd.TestDies];
            string[,] arrBin = null;
            int h = 0;

            try
            {
                arrBin = new string[hd.DieIndexMaxX + 1, hd.DieIndexMaxY + 1];

                while (h < rd.Length)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        rd[h].DieX = Int32.Parse(dr["X"].ToString());
                        rd[h].DieY = Int32.Parse(dr["Y"].ToString());
                        rd[h].Bin = Int32.Parse(dr["BIN"].ToString());
                        rd[h].HBin = Int32.Parse(dr["HBIN"].ToString());
                        rd[h].CharBin = dr["CHARBIN"].ToString();
                        rd[h].Reel = Int32.Parse(dr["REEL"].ToString());
                        rd[h].Position = Int32.Parse(dr["POSITION"].ToString());

                        arrBin[rd[h].DieX, rd[h].DieY] = rd[h].Bin.ToString();

                        h++;
                    }
                }

                hd.iMaxX = Int32.Parse(dt.Compute("MAX(X)", null).ToString());
                hd.iMaxY = Int32.Parse(dt.Compute("MAX(Y)", null).ToString());
                hd.iMinX = Int32.Parse(dt.Compute("MIN(X)", null).ToString());
                hd.iMinY = Int32.Parse(dt.Compute("MIN(Y)", null).ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return arrBin;
        }
    }
}
