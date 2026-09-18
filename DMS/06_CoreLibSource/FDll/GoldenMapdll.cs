using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace FDll
{
    /// <summary>
    /// GoldenMapdll
    /// </summary>

    public class GoldenMapdll
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
        public int ChipSizeX = 1;
        public int ChipSizeY = 1;
        public int XDies = 0;
        public int YDies = 0;
        public int angle = 0;
        public int netDie = 0;
        public int waferSize = 0;
        public double dieSizeX = 1;
        public double dieSizeY = 0;
        public int ReferenceDieSetting = 0;
        public int testDies = 0;
        public int notchType = 0;
        public int TargetDieX = 1;
        public int TargetDieY = 1;
        public int EdgeSize = 0;

        public DataTable dtRowData = null;

        /// <summary>
		///  생성자
		/// </summary>
        /// 
        public GoldenMapdll()
        {
        }

		/// <summary>
		/// 소멸자
		/// </summary>
        ~GoldenMapdll()
		{
        }

        //#region -------------------------------------------------------------------------------- Interface Function

        /// <summary>
        /// InitData
        /// </summary>
        // Map Define 시 사용
        public void GetData(string FilePath)
        {
            string strFileText = string.Empty;
            string[] splitTxt = null;
            string strHeader = string.Empty;
            string[] arrBin = null;

            try
            {
                // File Read
                Encoding encode = System.Text.Encoding.GetEncoding("UTF-8");
                FileStream file = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(file, encode);
                strFileText = sr.ReadToEnd();

                splitTxt = strFileText.Split(new string[] { "RowData:" }, StringSplitOptions.RemoveEmptyEntries);

                strHeader = splitTxt[0];
                string[] arrHeder = strHeader.Split(new string[]{"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
                
                // Header Info Setting
                this.productName = arrHeder[0].Split(':')[1];
                this.lotID = arrHeder[1].Split(':')[1];
                this.waferID = arrHeder[2].Split(':')[1];
                this.angle = Int32.Parse(arrHeder[3].Split(':')[1]);
                this.xmax = Int32.Parse(arrHeder[5].Split(':')[1]);
                this.ymax = Int32.Parse(arrHeder[4].Split(':')[1]);
                this.targetDieX = Int32.Parse(arrHeder[7].Split(':')[1]);
                this.targetDieY = Int32.Parse(arrHeder[8].Split(':')[1]);
                this.dieSizeX = Double.Parse(arrHeder[10].Split(':')[1]);
                this.dieSizeY = Double.Parse(arrHeder[11].Split(':')[1]);
                this.XDies = this.xmax - this.xmin + 1;
                this.YDies = this.ymax - this.ymin + 1;
                this.waferSize = GetWaferSize();

                arrBin = new string[splitTxt.Length - 1];
                for (int i = 0; i < arrBin.Length; i++)
                {
                    arrBin[i] = splitTxt[i + 1];
                }

                // Bin Info Setting
                this.dtRowData = new DataTable();

                this.dtRowData.Columns.Add(new DataColumn("X", typeof(int)));
                this.dtRowData.Columns.Add(new DataColumn("Y", typeof(int)));
                this.dtRowData.Columns.Add(new DataColumn("USE_CODE", typeof(int)));
                
                int binSize = 3;
                for (int Y = 0; Y < arrBin.Length; Y++)
                {
                    arrBin[Y] = arrBin[Y].Replace(" ", "").Replace("\r\n", "");

                    for (int X = 0, XStep = 0; XStep < arrBin[Y].Length ; X++, XStep += binSize)
                    {
                        DataRow dr = this.dtRowData.NewRow();
                        string strBin = arrBin[Y].Substring(XStep, binSize);
                        if (strBin != "___")
                        {
                            dr["X"] = X + 1;
                            dr["Y"] = Y + 1;
                            dr["USE_CODE"] = 1;

                            this.dtRowData.Rows.Add(dr);
                        }
                    }
                }

                // TestDie Count
                this.testDies = this.dtRowData.Rows.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int GetWaferSize()
        {
            int finalWaferSize = 0;

            try
            {
                Double xSize = this.dieSizeX * (this.xmax - 1);
                Double ySize = this.dieSizeY * (this.ymax - 1);

                // X축 Size 와 Y축 Size 중 큰 값 사용 (mm)
                Double waferSize = xSize >= ySize ? xSize : ySize;

                if (waferSize <= 400)
                {
                    finalWaferSize = 400;

                    if (waferSize <= 300)
                    {
                        finalWaferSize = 300;

                        if (waferSize <= 200)
                        {
                            finalWaferSize = 200;

                            if (waferSize <= 150)
                            {
                                finalWaferSize = 150;

                                if (waferSize <= 100)
                                    finalWaferSize = 100;
                            }
                        }
                    }
                }
                return finalWaferSize;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Parsing 시 사용
        public void WriteGoldenRowData(ref DataTable dt, ref HeaderData hd, ref RowData[] rd)
        {
            rd = new RowData[hd.TestDies];
            int h = 0;

            while (h < rd.Length)
            {
                foreach(DataRow dr in dt.Rows)
                {
                    rd[h].DieX = Int32.Parse(dr["X"].ToString());
                    rd[h].DieY = Int32.Parse(dr["Y"].ToString());
                    rd[h].Bin = Int32.Parse(dr["BIN"].ToString());
                    rd[h].HBin = Int32.Parse(dr["HBIN"].ToString());
                    rd[h].CharBin = dr["CHARBIN"].ToString();
                    h++;
                }
            }
        }
    }
}

