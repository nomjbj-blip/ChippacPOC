using System;
using System.IO;
using System.Text;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;

namespace FDll
{

    /// <summary>
    /// TSKdll
    /// </summary>
    public class TSKdll : Fdll
    {
        string productName = string.Empty;
        string waferID = string.Empty;
        //int xmin = 0;
        //int ymin = 0;
        //int xmax = 0;
        //int ymax = 0;
        //int xdie = 0;
        //int ydie = 0;
        //int angle = 0;
        int netDie = 0;
        //int waferSize = 0;
        //double dieSizeX = 0;
        //double dieSizeY = 0;
        /// <summary>
        ///  생성자
        /// </summary>
        /// 
        public TSKdll()
        {
            InitData();
        }

        /// <summary>
        /// 소멸자
        /// </summary>
        ~TSKdll()
        {
        }

        #region -------------------------------------------------------------------------------- Interface Function

        /// <summary>
        /// InitData
        /// </summary>
        public void InitData()
        {
            SetDataSetXmlFile(sFormatDataPath + "tsk.xml", true);
            DataSetToStruct();
        }

        /// <summary>
        /// TSK Binary File -> DataSet
        /// </summary>
        /// <param name="binaryFile">TSK Data File</param>
        public void ReadBinaryFile(string binaryFile)
        {
            SetDataSetXmlFile(sFormatDataPath + "tsk.xml", true);
            ReadBinaryDataFile(binaryFile);
            DataSetToStruct();
        }

        /// <summary>
        /// DataSet -> TSK Binary File 
        /// </summary>
        /// <param name="binaryFile">TSK Data File</param>
        public void WriteBinaryFile(string binaryFile)
        {
            StructToDataSet();
            WriteBinaryDataFile(binaryFile);
        }

        /// <summary>
        /// TSK Binary File -> DataSet -> Text로 출력
        /// </summary>
        /// <param name="tskBinaryFile">TSK Data File</param>
        /// <param name="outFilePath">Text File</param>
        public void OutTextFile(string binaryFile, string outFile)
        {
            SetDataSetXmlFile(sFormatDataPath + "tsk.xml", true);
            ReadBinaryDataFile(binaryFile);
            OutTxtFile(sFormatDataPath + "tsk.tf", outFile);
        }

        public DataSet OutTextFile1(string binaryFile, string outFile)
        {
            DataSet ds = null;
            SetDataSetXmlFile(sFormatDataPath + "tsk.xml", true);
            ReadBinaryDataFile(binaryFile);
            ds = OutTxtFile1(sFormatDataPath + "tsk.tf", outFile);
            return ds;
        }

        /// <summary>
        /// DataSet -> Text로 출력
        /// </summary>
        /// <param name="tskBinaryFile">TSK Data File</param>
        /// <param name="outFilePath">Text File</param>
        public void OutTextFile(string outFile)
        {
            OutTxtFile(sFormatDataPath + "tsk.tf", outFile);
        }

        #endregion -------------------------------------------------------------------------------- Interface Function

        #region -------------------------------------------------------------------------------- DataConvert

        protected override void DataSetToStruct()
        {
            try
            {
                m_HeaderData.Operator = GetHeaderValue("Operator");
                m_HeaderData.Device = GetHeaderValue("Device");
                m_HeaderData.WaferSize = GetHeaderValueDouble("WaferSize") * 2.5;
                m_HeaderData.MachineNo = GetHeaderValue("MachineNo");
                m_HeaderData.ChipSizeX = GetHeaderValueDouble("ChipSizeX") / 100000;
                m_HeaderData.ChipSizeY = GetHeaderValueDouble("ChipSizeY") / 100000;
                m_HeaderData.Angle = GetHeaderValueInt("Angle");
                m_HeaderData.LastEditEquipKind = GetHeaderValueInt("LastEditEquipKind");
                m_HeaderData.MapVersion = GetHeaderValueInt("MapVersion");
                m_HeaderData.XDies = GetHeaderValueInt("XDies");
                m_HeaderData.YDies = GetHeaderValueInt("YDies");
                m_HeaderData.MapDataKind = GetHeaderValueInt("MapDataKind");
                m_HeaderData.WaferID = GetHeaderValue("WaferID");
                m_HeaderData.TestCount = GetHeaderValueInt("TestCount");
                m_HeaderData.LotNo = GetHeaderValue("LotNo");
                m_HeaderData.CassetteNo = GetHeaderValueInt("CassetteNo");
                m_HeaderData.SlotNo = GetHeaderValueInt("SlotNo");
                m_HeaderData.XDirection = GetHeaderValueInt("XDirection");
                m_HeaderData.YDirection = GetHeaderValueInt("YDirection");
                m_HeaderData.ReferenceDieSetting = GetHeaderValueInt("ReferenceDieSetting");
                m_HeaderData.TargetX = GetHeaderValueDouble("TargetX");
                m_HeaderData.TargetY = GetHeaderValueDouble("TargetY");
                m_HeaderData.TargetDieX = GetHeaderValueInt("TargetDieX");
                m_HeaderData.TargetDieY = GetHeaderValueInt("TargetDieY");
                m_HeaderData.ProbingStartPosition = GetHeaderValueInt("ProbingStartPosition");
                m_HeaderData.ProbingDirection = GetHeaderValueInt("ProbingDirection");
                m_HeaderData.OriginX = GetHeaderValueDouble("OriginX") / 1000;
                m_HeaderData.OriginY = GetHeaderValueDouble("OriginY") / 1000;
                m_HeaderData.OriginDieX = GetHeaderValueInt("OriginDieX");
                m_HeaderData.OriginDieY = GetHeaderValueInt("OriginDieY");
                m_HeaderData.FirstDieX = GetHeaderValueInt("FirstDieX");
                m_HeaderData.FirstDieY = GetHeaderValueInt("FirstDieY");
                m_HeaderData.StartTime = GetHeaderValue("StartTime");
                m_HeaderData.EndTime = GetHeaderValue("EndTime");
                m_HeaderData.WaferLoadingStartTime = GetHeaderValue("WaferLoadingStartTime");
                m_HeaderData.WaferUnloadingStartTime = GetHeaderValue("WaferUnloadingStartTime");
                m_HeaderData.MachineNo1 = GetHeaderValueInt("MachineNo1");
                m_HeaderData.MachineNo2 = GetHeaderValueInt("MachineNo2");
                m_HeaderData.SpecialWord = GetHeaderValueInt("SpecialWord");
                m_HeaderData.TestingFinishStatus = GetHeaderValueInt("TestingFinishStatus");
                m_HeaderData.TestDies = GetHeaderValueInt("TestDies");
                m_HeaderData.PassDies = GetHeaderValueInt("PassDies");
                m_HeaderData.FailDies = GetHeaderValueInt("FailDies");
                m_HeaderData.AreaStartAdress = GetHeaderValueInt("AreaStartAdress");
                m_HeaderData.LineCatData = GetHeaderValueInt("LineCatData");
                m_HeaderData.LineCatAddress = GetHeaderValueInt("LineCatAddress");
                m_HeaderData.MapFile = GetHeaderValueInt("MapFile");
                m_HeaderData.MultiSite = GetHeaderValueInt("MultiSite");
                m_HeaderData.Category = GetHeaderValueInt("Category");

                int i;

                i = m_Ds.Tables["DataData"].Rows.Count;

                if (m_RowDatas != null) m_RowDatas = null;
                m_RowDatas = new RowData[i];

                int val = 0;
                UInt16 iVal;
                UInt16 iData1;
                UInt16 iData2;
                UInt16 iData3;
                string str = string.Empty;

                i = 0;

                foreach (DataRow dr in m_Ds.Tables["DataData"].Rows)
                {

                    iData1 = Convert.ToUInt16(dr[1]);
                    iData2 = Convert.ToUInt16(dr[2]);
                    iData3 = Convert.ToUInt16(dr[3]);

                    iVal = (UInt16)((iData1 >> 14) & 0x03);
                    m_RowDatas[i].DieTestResult = Convert.ToInt32(iVal);

                    iVal = (UInt16)((iData1 >> 13) & 0x01);
                    m_RowDatas[i].Marking = Convert.ToInt32(iVal);

                    iVal = (UInt16)((iData1 >> 12) & 0x01);
                    m_RowDatas[i].FailMarkInsp = Convert.ToInt32(iVal);

                    iVal = (UInt16)((iData1 >> 11) & 0x03);
                    m_RowDatas[i].ReProbing = Convert.ToInt32(iVal);

                    iVal = (UInt16)((iData1 >> 9) & 0x01);
                    m_RowDatas[i].NeddleInspResult = Convert.ToInt32(iVal);

                    iVal = (UInt16)(iData1 & 0x1FF);
                    val = iVal;
                    if (((iData2 >> 11) & 0x01) == 1) val = val * -1;
                    m_RowDatas[i].DieX = Convert.ToInt32(val);

                    iVal = (UInt16)(iData2 >> 14 & 0x03);
                    m_RowDatas[i].DieAttribute = Convert.ToInt32(iVal);

                    iVal = (UInt16)((iData2 >> 13) & 0x01);
                    m_RowDatas[i].NeddleInspTarget = Convert.ToInt32(iVal);

                    iVal = (UInt16)((iData2 >> 12) & 0x01);
                    m_RowDatas[i].SamplingDie = Convert.ToInt32(iVal);

                    iVal = (UInt16)((iData2 >> 9) & 0x01);
                    m_RowDatas[i].DummyData = Convert.ToInt32(iVal);

                    iVal = (UInt16)(iData2 & 0x1FF);
                    val = iVal;
                    if (((iData2 >> 10) & 0x01) == 1) val = val * -1;
                    m_RowDatas[i].DieY = Convert.ToInt32(val);

                    iVal = (UInt16)((iData3 >> 15) & 0x01);
                    m_RowDatas[i].NotOutput = Convert.ToInt32(iVal);

                    iVal = (UInt16)((iData3 >> 14) & 0x01);
                    m_RowDatas[i].ProbingDie = Convert.ToInt32(iVal);

                    iVal = (UInt16)((iData3 >> 13) & 0x3F);
                    m_RowDatas[i].TestSiteNo = Convert.ToInt32(iVal);

                    iVal = (UInt16)((iData3 >> 7) & 0x03);
                    m_RowDatas[i].BlockArea = Convert.ToInt32(iVal);

                    iVal = (UInt16)(iData3 & 0x3F);
                    m_RowDatas[i].Bin = Convert.ToInt32(iVal);

                    i++;
                }
            }
            catch
            {
            }
            finally
            {
            }
        }

        protected override void DataSetToStruct(string LotWafer)
        {
        }

        protected override void StructToDataSet()
        {
            try
            {
                SetHeaderValue("Operator", m_HeaderData.Operator);
                SetHeaderValue("Device", m_HeaderData.Device);
                SetHeaderValue("WaferSize", Convert.ToString(m_HeaderData.WaferSize / 2.5));
                SetHeaderValue("MachineNo", 0.ToString());
                SetHeaderValue("ChipSizeX", Convert.ToString(m_HeaderData.ChipSizeX * 100000));
                SetHeaderValue("ChipSizeY", Convert.ToString(m_HeaderData.ChipSizeY * 100000));
                SetHeaderValue("Angle", m_HeaderData.Angle.ToString());
                SetHeaderValue("LastEditEquipKind", m_HeaderData.LastEditEquipKind.ToString());
                SetHeaderValue("MapVersion", m_HeaderData.MapVersion.ToString());
                SetHeaderValue("XDies", m_HeaderData.XDies.ToString());
                SetHeaderValue("YDies", m_HeaderData.YDies.ToString());
                SetHeaderValue("MapDataKind", m_HeaderData.MapDataKind.ToString());
                SetHeaderValue("WaferID", m_HeaderData.LotNo + m_HeaderData.SlotNo.ToString().PadLeft(2, '0'));
                SetHeaderValue("TestCount", m_HeaderData.TestCount.ToString());
                SetHeaderValue("LotNo", m_HeaderData.LotNo);
                SetHeaderValue("CassetteNo", m_HeaderData.CassetteNo.ToString());
                SetHeaderValue("SlotNo", m_HeaderData.SlotNo.ToString());
                SetHeaderValue("XDirection", m_HeaderData.XDirection.ToString());
                SetHeaderValue("YDirection", m_HeaderData.YDirection.ToString());
                SetHeaderValue("ReferenceDieSetting", m_HeaderData.ReferenceDieSetting.ToString());
                SetHeaderValue("TargetX", m_HeaderData.TargetX.ToString());
                SetHeaderValue("TargetY", m_HeaderData.TargetY.ToString());
                SetHeaderValue("TargetDieX", m_HeaderData.TargetDieX.ToString());
                SetHeaderValue("TargetDieY", m_HeaderData.TargetDieY.ToString());
                SetHeaderValue("ProbingStartPosition", m_HeaderData.ProbingStartPosition.ToString());
                SetHeaderValue("ProbingDirection", m_HeaderData.ProbingDirection.ToString());
                SetHeaderValue("OriginX", Convert.ToString(Convert.ToInt32(m_HeaderData.OriginX * 1000)));
                SetHeaderValue("OriginY", Convert.ToString(Convert.ToInt32(m_HeaderData.OriginY * 1000)));
                SetHeaderValue("OriginDieX", m_HeaderData.OriginDieX.ToString());
                SetHeaderValue("OriginDieY", m_HeaderData.OriginDieY.ToString());
                SetHeaderValue("FirstDieX", m_HeaderData.FirstDieX.ToString());
                SetHeaderValue("FirstDieY", m_HeaderData.FirstDieY.ToString());
                SetHeaderValue("StartTime", m_HeaderData.StartTime);
                SetHeaderValue("EndTime", m_HeaderData.EndTime);
                SetHeaderValue("WaferLoadingStartTime", m_HeaderData.WaferLoadingStartTime);
                SetHeaderValue("WaferUnloadingStartTime", m_HeaderData.WaferUnloadingStartTime);
                SetHeaderValue("MachineNo1", m_HeaderData.MachineNo1.ToString());
                SetHeaderValue("MachineNo2", m_HeaderData.MachineNo2.ToString());
                SetHeaderValue("SpecialWord", m_HeaderData.SpecialWord.ToString());
                SetHeaderValue("TestingFinishStatus", m_HeaderData.TestingFinishStatus.ToString());
                SetHeaderValue("TestDies", m_HeaderData.TestDies.ToString());
                SetHeaderValue("PassDies", m_HeaderData.PassDies.ToString());
                SetHeaderValue("FailDies", m_HeaderData.FailDies.ToString());
                SetHeaderValue("AreaStartAdress", m_HeaderData.AreaStartAdress.ToString());
                SetHeaderValue("LineCatData", m_HeaderData.LineCatData.ToString());
                SetHeaderValue("LineCatAddress", m_HeaderData.LineCatAddress.ToString());
                SetHeaderValue("MapFile", m_HeaderData.MapFile.ToString());
                SetHeaderValue("MultiSite", m_HeaderData.MultiSite.ToString());
                SetHeaderValue("Category", m_HeaderData.Category.ToString());

                DataTable dt = m_Ds.Tables["DataData"];
                DataRow dr;
                dt.Rows.Clear();

                int i;
                UInt16 iData1;
                UInt16 iData2;
                UInt16 iData3;
                string str = string.Empty;

                for (i = 0; i < m_RowDatas.Length; i++)
                {
                    dr = dt.NewRow();
                    iData1 = 0;
                    iData2 = 0;
                    iData3 = 0;

                    iData1 = (UInt16)(iData1 + (m_RowDatas[i].DieTestResult << 14));
                    iData1 = (UInt16)(iData1 + (m_RowDatas[i].Marking << 13));
                    iData1 = (UInt16)(iData1 + (m_RowDatas[i].FailMarkInsp << 12));
                    iData1 = (UInt16)(iData1 + (m_RowDatas[i].ReProbing << 11));
                    iData1 = (UInt16)(iData1 + (m_RowDatas[i].NeddleInspResult << 9));

                    if (m_RowDatas[i].DieX < 0)
                    {
                        iData2 = (UInt16)(iData2 + (0x01 << 11));
                        iData1 = (UInt16)(iData1 + (m_RowDatas[i].DieX * -1));
                    }
                    else
                    {
                        iData1 = (UInt16)(iData1 + m_RowDatas[i].DieX);
                    }

                    iData2 = (UInt16)(iData2 + (m_RowDatas[i].DieAttribute << 14));
                    iData2 = (UInt16)(iData2 + (m_RowDatas[i].NeddleInspTarget << 13));
                    iData2 = (UInt16)(iData2 + (m_RowDatas[i].SamplingDie << 12));
                    iData2 = (UInt16)(iData2 + (m_RowDatas[i].DummyData << 9));

                    if (m_RowDatas[i].DieY < 0)
                    {
                        iData2 = (UInt16)(iData2 + (0x01 << 10));
                        iData2 = (UInt16)(iData2 + (m_RowDatas[i].DieY * -1));
                    }
                    else
                    {
                        iData2 = (UInt16)(iData2 + m_RowDatas[i].DieY);
                    }

                    iData3 = (UInt16)(iData3 + (m_RowDatas[i].NotOutput << 15));
                    iData3 = (UInt16)(iData3 + (m_RowDatas[i].ProbingDie << 14));
                    iData3 = (UInt16)(iData3 + (m_RowDatas[i].TestSiteNo << 13));
                    iData3 = (UInt16)(iData3 + (m_RowDatas[i].BlockArea << 7));
                    iData3 = (UInt16)(iData3 + (m_RowDatas[i].Bin));

                    dr["NO"] = Convert.ToString(i + 1);
                    dr["WORD1"] = iData1.ToString();
                    dr["WORD2"] = iData2.ToString();
                    dr["WORD3"] = iData3.ToString();

                    dt.Rows.Add(dr);
                }
            }
            catch
            {
            }
            finally
            {
            }
        }

        #endregion -------------------------------------------------------------------------------- DataConvert

        #region -------------------------------------------------------------------------------- Text File Read, Write

        /// <summary>
        /// DataSet으로 Text File을 만든다
        /// </summary>
        /// <param name="formatFilePath">Text 형식 File</param>
        /// <param name="outFilePath">만들어질 파일의 Full Path</param>
        /// <returns>성공 여부</returns>
        protected override int OutTxtFile(string formatFilePath, string outFilePath)
        {
            TSKOutText tot = new TSKOutText(formatFilePath, outFilePath, ref m_Ds);
            tot.Act1();
            return 1;
        }

        protected override DataSet OutTxtFile1(string formatFilePath, string outFilePath)
        {
            DataSet ds = null;
            TSKOutText tot = new TSKOutText(formatFilePath, outFilePath, ref m_Ds);
            ds = tot.Act1();
            return ds;
        }

        public DataSet getData()
        {
            return m_Ds;
        }

        public void SetHeader(HeaderData hd)
        {
            m_HeaderData = hd;
        }

        public void SetRowData(RowData[] rd)
        {
            m_RowDatas = rd;
        }
        /// <summary>
        /// Text File을 읽는다
        /// </summary>
        /// <param name="regFilePath">정규식 File</param>
        /// <param name="inputFile">읽을 File</param>
        /// <returns>성공 여부</returns>
        protected override int InputTxtFile(string regFilePath, string inputFile)
        {
            return 1;
        }

        public void ReadTxtFile(string filePath, ref HeaderData hd, ref DataSet n_DS)
        {
            FileStream fs = null;
            StreamReader sr = null;
            string str = string.Empty;
            string[] strBuffer = null;
            string[] tmp = null;

            //string[] binData = null;
            //int[] binItem = null;
            //string[] binChr = null;
            //int cnt = 0;
            //char[] binArray = null;
            string strTime = string.Empty;

            fs = new FileStream(filePath, System.IO.FileMode.Open);	// File Open
            sr = new StreamReader(fs);

            try
            {
                str = sr.ReadToEnd();
                sr.Close();
                fs.Close();
                strBuffer = str.Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < strBuffer.Length; i++)
                {
                    if (strBuffer[i].ToString().Split(new char[] { '\t' }).Length == 6) continue;
                    if (strBuffer[i].StartsWith("(Reserved)") || strBuffer[i].StartsWith("Cassette") || strBuffer[i].StartsWith("Slot")
                        || strBuffer[i].StartsWith("X coordinate increase") || strBuffer[i].StartsWith("Y coordinate increase")
                        || strBuffer[i].StartsWith("Reference die setting") || strBuffer[i].StartsWith("Target die") || strBuffer[i].StartsWith("Probing")
                        || strBuffer[i].StartsWith("Distance") || strBuffer[i].StartsWith("Center") || strBuffer[i].StartsWith("Wafer loading")
                        || strBuffer[i].StartsWith("Wafer unloading") || strBuffer[i].StartsWith("Testing") || strBuffer[i].StartsWith("Operator's name")) continue;

                    if (strBuffer[i].StartsWith("Device name"))
                    {
                        tmp = strBuffer[i].Split(new char[] { '=' });
                        productName = tmp[1].TrimStart(' ').TrimEnd('\0').Replace(" ", "");
                        hd.Device = productName;
                        continue;
                    }

                    if (strBuffer[i].StartsWith("Wafer size"))
                    {
                        tmp = strBuffer[i].Split(new char[] { '=' });
                        hd.WaferSize = int.Parse(tmp[1].TrimEnd(new char[] { 'i', 'n', 'c', 'h', '\0' }).ToString().Replace(" ", "")) * 25;
                        continue;
                    }

                    if (strBuffer[i].StartsWith("Index size X"))
                    {
                        tmp = strBuffer[i].Split(new char[] { '=' });
                        hd.ChipSizeX = double.Parse(tmp[1].TrimEnd(new char[] { '\0', 'm' }).ToString().Replace(" ", ""));
                        continue;
                    }

                    if (strBuffer[i].StartsWith("Index size Y"))
                    {
                        tmp = strBuffer[i].Split(new char[] { '=' });
                        hd.ChipSizeY = double.Parse(tmp[1].TrimEnd(new char[] { '\0', 'm' }).ToString().Replace(" ", ""));
                        continue;
                    }


                    if (strBuffer[i].StartsWith("Map column size"))
                    {
                        tmp = strBuffer[i].Split(new char[] { '=' });
                        hd.XDies = int.Parse(tmp[1].TrimEnd('\0').ToString().Replace(" ", ""));
                        continue;
                    }

                    if (strBuffer[i].StartsWith("Map row size"))
                    {
                        tmp = strBuffer[i].Split(new char[] { '=' });
                        hd.YDies = int.Parse(tmp[1].TrimEnd('\0').ToString().Replace(" ", ""));
                        continue;
                    }

                    if (strBuffer[i].StartsWith("Lot No."))
                    {
                        tmp = strBuffer[i].Split(new char[] { '=' });
                        hd.LotNo = tmp[1].TrimEnd('\0').Replace(" ", "").ToString();
                        continue;
                    }

                    if (strBuffer[i].StartsWith("Wafer ID"))
                    {
                        tmp = strBuffer[i].Split(new char[] { '=' });
                        waferID = tmp[1].TrimEnd('\0').Replace(" ", "").Substring(tmp[1].TrimEnd('\0').Replace(" ", "").Length - 2);
                        hd.WaferID = waferID;
                        continue;
                    }

                    if (strBuffer[i].StartsWith("Wafer testing start time data"))
                    {
                        tmp = strBuffer[i].Split(new char[] { '=' });

                        // 13/08/28 14:15 -> 13.08.28 14:15
                        strTime = tmp[1].TrimEnd('\0').ToString().Trim().Replace("/", ".");

                        System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("tr-TR");

                        string pattern = "yy.MM.dd HH:mm";

                        hd.StartTime = DateTime.ParseExact(strTime, pattern, culture).ToString("yyyyMMddHHmmss");

                        continue;
                    }

                    if (strBuffer[i].StartsWith("Wafer testing finish time data"))
                    {

                        tmp = strBuffer[i].Split(new char[] { '=' });

                        strTime = tmp[1].TrimEnd('\0').ToString().Trim().Replace("/", ".");

                        System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("tr-TR");

                        string pattern = "yy.MM.dd HH:mm";

                        hd.EndTime = DateTime.ParseExact(strTime, pattern, culture).ToString("yyyyMMddHHmmss");

                        continue;
                    }

                    if (strBuffer[i].StartsWith("Reference die X coordinate"))
                    {


                        tmp = strBuffer[i].Split(new char[] { '=' });
                        hd.iMinX = int.Parse(tmp[1].TrimEnd('\0').ToString().Replace(" ", ""));
                        hd.iMaxX = hd.XDies + hd.iMinX;
                        continue;

                        //for (int k = 0; k < xmax; k++)
                        //{
                        //    binDt.Columns.Add(Convert.ToString(k));
                        //}
                    }

                    if (strBuffer[i].StartsWith("Reference die Y coordinate"))
                    {
                        tmp = strBuffer[i].Split(new char[] { '=' });
                        hd.iMinY = int.Parse(tmp[1].TrimEnd('\0').ToString().Replace(" ", ""));
                        hd.iMaxY = hd.YDies + hd.iMinY;
                        continue;
                    }

                    if (strBuffer[i].StartsWith("Reference flat direction"))
                    {
                        tmp = strBuffer[i].Split(new char[] { '=' });
                        hd.Angle = int.Parse((tmp[1].Replace(" ", "").Replace("degree", "")));
                        continue;
                    }

                    if (strBuffer[i].StartsWith("Total pass dies"))
                    {
                        tmp = strBuffer[i].Split(new char[] { '=' });
                        hd.PassDies = int.Parse(tmp[1].TrimEnd('\0').ToString().Replace(" ", ""));
                        continue;
                    }

                    if (strBuffer[i].StartsWith("Total fail dies"))
                    {
                        tmp = strBuffer[i].Split(new char[] { '=' });
                        hd.FailDies = int.Parse(tmp[1].TrimEnd('\0').ToString().Replace(" ", ""));
                        continue;
                    }

                    if (strBuffer[i].StartsWith("Total tested dies"))
                    {
                        tmp = strBuffer[i].Split(new char[] { '=' });
                        netDie = int.Parse(tmp[1].TrimEnd('\0').ToString().Replace(" ", ""));
                        hd.TestDies = int.Parse(tmp[1].TrimEnd('\0').ToString().Replace(" ", ""));
                        continue;
                    }
                }

            }
            catch (Exception ex)
            {
                //throw new Exception(string.Format("pos : {0}, string : {1}, err msg : {2}", e, ex.Message));
                throw ex;
            }
        }

        #endregion -------------------------------------------------------------------------------- Text File Read, Write

        public static bool CheckNumber(string letter)
        {
            bool IsCheck = true;

            Regex numRegex = new Regex(@"[0-9]");
            Boolean isMatch = numRegex.IsMatch(letter);

            if (!isMatch)
            {
                IsCheck = false;
            }
            return IsCheck;
        }

        public new DataSet SetDataSetXmlFile(string xmlfilePath, bool valueDataInit)
        {
            try
            {
                if (valueDataInit == true)
                {
                    m_Ds.Clear();
                    m_Ds = new DataSet();
                }

                m_Ds.ReadXml(xmlfilePath);

                return m_Ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
