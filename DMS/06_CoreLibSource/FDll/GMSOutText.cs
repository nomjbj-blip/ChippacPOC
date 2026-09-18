using System;
using System.IO;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;

namespace FDll
{
    /// <summary>
    /// TSKOutText에 대한 요약 설명입니다.
    /// </summary>
    public class GMSOutText : FOutText
    {
        public GMSOutText()
        {
        }
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="formatFile">Format 형식 File Full Path</param>
        /// <param name="OutFile">Out File Full Path</param>
        /// <param name="ds">DataSet</param>
        public GMSOutText(string formatFile, string OutFile, ref DataSet ds)
            : base(formatFile, OutFile, ref ds)
        {
            m_FormatFile = formatFile;
            m_OutFile = OutFile;
            m_Ds = ds;
        }

        /// <summary>
        /// 소멸자
        /// </summary>
        ~GMSOutText()
        {
        }

        #region -------------------------------------------------------------------------------- Act

        /// <summary>
        /// Act
        /// </summary>
        public override void Act()
        {
            File.Copy(m_FormatFile, m_OutFile, true);

            // Dat Psting ===========
            InsertOutFile();
            InsertTable();
            // Dat Psting ===========
        }

        public override DataSet Act1()
        {
            DataSet ds = null;
            File.Copy(m_FormatFile, m_OutFile, true);

            // Dat Psting ===========
            InsertOutFile();
            ds = InsertTable();
            // Dat Psting ===========

            return ds;
        }

        #endregion -------------------------------------------------------------------------------- Act

        #region -------------------------------------------------------------------------------- Process

        #region ' Dat Parsing '

        /// <summary>
        /// 출력 Text File을 만든다.
        /// </summary>
        private void InsertOutFile()
        {
            string strHead;

            FileStream fs = null;
            StreamReader sr = null;
            StreamWriter sw = null;

            try
            {
                fs = new FileStream(m_OutFile, System.IO.FileMode.Open);
                sr = new StreamReader(fs);

                strHead = sr.ReadToEnd();

                strHead = InsertHeaderData(strHead);

                sr.Close();
                fs.Close();
                sr = null;
                fs = null;

                fs = new FileStream(m_OutFile, System.IO.FileMode.Create);
                sw = new StreamWriter(fs);

                sw.Write(strHead);

                InsertTsetData(ref sw);


                sw.Close();
                fs.Close();
                sw = null;
                fs = null;
            }
            catch
            {
            }
            finally
            {
                if (sr != null) sr.Close();
                if (sw != null) sr.Close();
                if (fs != null) sr.Close();

                sr = null;
                sw = null;
                fs = null;
            }
        }

        /// <summary>
        /// Header Txet를 구성한다
        /// </summary>
        /// <param name="strHead">File에서 읽은 String</param>
        /// <returns>변환된 String</returns>
        public string InsertHeaderData(string strHead)
        {
            string str;
            double dou;

            strHead = strHead.Replace("%LotNo%", GetHeaderValue("LotNo"));
            strHead = strHead.Replace("%CardNo%", GetHeaderValue("CardNo"));
            strHead = strHead.Replace("%Operator%", GetHeaderValue("Operator"));	// Operator
            strHead = strHead.Replace("%MachineNo%", GetHeaderValue("MachineNo"));
            strHead = strHead.Replace("%WaferNoPlus%", GetHeaderValue("WaferNoPlus"));
            strHead = strHead.Replace("%WaferNoMinus%", GetHeaderValue("WaferNoMinus"));
            strHead = strHead.Replace("%WaferName%", GetHeaderValue("WaferName"));

            str = GetHeaderValue("WaferSize");		// Wafer Size
            switch (str)
            {
                case "40": str = "4"; break;
                case "45": str = "4.5"; break;
                case "50": str = "5"; break;
                case "60": str = "6"; break;
                case "80": str = "8"; break;
            }
            strHead = strHead.Replace("%WaferSize%", str);
            strHead = strHead.Replace("%Angle%", GetHeaderValue("Angle"));

            str = GetHeaderValue("ChipSizeX");			// Index Size X, Chip Size X
            dou = Convert.ToDouble(str) * 0.00001;
            str = Convert.ToString(dou);
            strHead = strHead.Replace("%ChipSizeX%", str);

            str = GetHeaderValue("ChipSizeY");			// Index Size Y, Chip Size Y
            dou = Convert.ToDouble(str) * 0.00001;
            str = Convert.ToString(dou);

            strHead = strHead.Replace("%ChipSizeY%", str);
            strHead = strHead.Replace("%StartPosition%", GetHeaderValue("StartPosition"));
            strHead = strHead.Replace("%MicroPosition%", GetHeaderValue("MicroPosition"));
            strHead = strHead.Replace("%AlignmentAxis%", GetHeaderValue("AlignmentAxis"));
            strHead = strHead.Replace("%LightingMode%", GetHeaderValue("LightingMode"));
            strHead = strHead.Replace("%StartPosition%", GetHeaderValue("StartPosition"));
            strHead = strHead.Replace("%MicroPosition%", GetHeaderValue("MicroPosition"));
            strHead = strHead.Replace("%AlignmentAxis%", GetHeaderValue("AlignmentAxis"));
            strHead = strHead.Replace("%AutoFocus%", GetHeaderValue("AutoFocus"));
            strHead = strHead.Replace("%AlignmentX%", GetHeaderValue("AlignmentX"));
            strHead = strHead.Replace("%AlignmentY%", GetHeaderValue("AlignmentY"));
            strHead = strHead.Replace("%ProbeSize%", GetHeaderValue("ProbeSize"));
            strHead = strHead.Replace("%TargetMode%", GetHeaderValue("TargetMode"));
            strHead = strHead.Replace("%TargetPositionX%", GetHeaderValue("TargetPositionX"));
            strHead = strHead.Replace("%TargetPositionY%", GetHeaderValue("TargetPositionY"));
            strHead = strHead.Replace("%StdPositionX%", GetHeaderValue("StdPositionX"));
            strHead = strHead.Replace("%StdPositionY%", GetHeaderValue("StdPositionY"));
            strHead = strHead.Replace("%OrientFlatSelect%", GetHeaderValue("OrientFlatSelect"));
            strHead = strHead.Replace("%NumOrientationFlat%", GetHeaderValue("NumOrientationFlat"));
            strHead = strHead.Replace("%OrientFlatPosition%", GetHeaderValue("OrientFlatPosition"));
            strHead = strHead.Replace("%ProbeAreaSel%", GetHeaderValue("ProbeAreaSel"));
            strHead = strHead.Replace("%InkerOffset%", GetHeaderValue("InkerOffset"));
            strHead = strHead.Replace("%SampleProbeMode%", GetHeaderValue("SampleProbeMode"));
            strHead = strHead.Replace("%SampleStep1X%", GetHeaderValue("SampleStep1X"));
            strHead = strHead.Replace("%SampleStep1Y%", GetHeaderValue("SampleStep1Y"));
            strHead = strHead.Replace("%SampleStep2X%", GetHeaderValue("SampleStep2X"));
            strHead = strHead.Replace("%SampleStep2Y%", GetHeaderValue("SampleStep2Y"));
            strHead = strHead.Replace("%SampleStep3X%", GetHeaderValue("SampleStep3X"));
            strHead = strHead.Replace("%SampleStep3Y%", GetHeaderValue("SampleStep3Y"));
            strHead = strHead.Replace("%SampleStep4X%", GetHeaderValue("SampleStep4X"));
            strHead = strHead.Replace("%SampleStep4Y%", GetHeaderValue("SampleStep4Y"));
            strHead = strHead.Replace("%SampleStep5X%", GetHeaderValue("SampleStep5X"));
            strHead = strHead.Replace("%SampleStep5Y%", GetHeaderValue("SampleStep5Y"));
            strHead = strHead.Replace("%SampleStep6X%", GetHeaderValue("SampleStep6X"));
            strHead = strHead.Replace("%SampleStep6Y%", GetHeaderValue("SampleStep6Y"));
            strHead = strHead.Replace("%SampleStep7X%", GetHeaderValue("SampleStep7X"));
            strHead = strHead.Replace("%SampleStep7Y%", GetHeaderValue("SampleStep7Y"));
            strHead = strHead.Replace("%SampleStep8X%", GetHeaderValue("SampleStep8X"));
            strHead = strHead.Replace("%SampleStep8Y%", GetHeaderValue("SampleStep8Y"));
            strHead = strHead.Replace("%SampleStep9X%", GetHeaderValue("SampleStep9X"));
            strHead = strHead.Replace("%SampleStep9Y%", GetHeaderValue("SampleStep9Y"));
            strHead = strHead.Replace("%SampleStep10X%", GetHeaderValue("SampleStep10X"));
            strHead = strHead.Replace("%SampleStep10Y%", GetHeaderValue("SampleStep10Y"));
            strHead = strHead.Replace("%MonitorDieX%", GetHeaderValue("MonitorDieX"));
            strHead = strHead.Replace("%MonitorDieY%", GetHeaderValue("MonitorDieY"));
            strHead = strHead.Replace("%MonitorDieSizeX%", GetHeaderValue("MonitorDieSizeX"));
            strHead = strHead.Replace("%MonitorDieSizeY%", GetHeaderValue("MonitorDieSizeY"));
            strHead = strHead.Replace("%MultiDieMode%", GetHeaderValue("MultiDieMode"));
            strHead = strHead.Replace("%MultiDieLocation%", GetHeaderValue("MultiDieLocation"));
            strHead = strHead.Replace("%ContinuousFail%", GetHeaderValue("ContinuousFail"));
            strHead = strHead.Replace("%CheckBack%", GetHeaderValue("CheckBack"));
            strHead = strHead.Replace("%ContinuousFailCnt%", GetHeaderValue("ContinuousFailCnt"));
            strHead = strHead.Replace("%SkipDieLine%", GetHeaderValue("SkipDieLine"));
            strHead = strHead.Replace("%CheckBackCnt%", GetHeaderValue("CheckBackCnt"));
            strHead = strHead.Replace("%InkerOffset%", GetHeaderValue("InkerOffset"));
            strHead = strHead.Replace("%ChkBackNeedlePolish%", GetHeaderValue("ChkBackNeedlePolish"));
            strHead = strHead.Replace("%ProbeNeedlePolish%", GetHeaderValue("ProbeNeedlePolish"));
            strHead = strHead.Replace("%ZCount%", GetHeaderValue("ZCount"));
            strHead = strHead.Replace("%DieCount%", GetHeaderValue("DieCount"));
            strHead = strHead.Replace("%WaferCount%", GetHeaderValue("WaferCount"));
            strHead = strHead.Replace("%Overdrive%", GetHeaderValue("Overdrive"));
            strHead = strHead.Replace("%ExecutionCnt%", GetHeaderValue("ExecutionCnt"));
            strHead = strHead.Replace("%SettingTemperature%", GetHeaderValue("SettingTemperature"));
            strHead = strHead.Replace("%PresetAddressX%", GetHeaderValue("PresetAddressX"));
            strHead = strHead.Replace("%PresetAddressY%", GetHeaderValue("PresetAddressY"));
            strHead = strHead.Replace("%MarkingMode%", GetHeaderValue("MarkingMode"));
            strHead = strHead.Replace("%PassDies%", GetHeaderValue("PassDies"));
            strHead = strHead.Replace("%FailDies%", GetHeaderValue("FailDies"));
            strHead = strHead.Replace("%TestDies%", GetHeaderValue("TestDies"));

            str = GetHeaderValue("LotStartTime");		// Wafer Testing Start Time Data  05/11/22 11:10  0511221110
            try
            {
                str = str.Trim();
                str = str.Substring(0, 2) + "/" + str.Substring(2, 2) + "/" + str.Substring(4, 2) + " " + str.Substring(6, 2) + ":" + str.Substring(8, 2);
            }
            catch
            {
                str = "";
            }
            strHead = strHead.Replace("%LotStartTime%", str);

            str = GetHeaderValue("LotEndTime");		// Wafer Testing Finish Time Data
            try
            {
                str = str.Trim();
                str = str.Substring(0, 2) + "/" + str.Substring(2, 2) + "/" + str.Substring(4, 2) + " " + str.Substring(6, 2) + ":" + str.Substring(8, 2);
            }
            catch
            {
                str = "";
            }
            strHead = strHead.Replace("%LotEndTime%", str);

            strHead = strHead.Replace("%CassetteNo%", GetHeaderValue("CassetteNo"));
            strHead = strHead.Replace("%SlotNo%", GetHeaderValue("SlotNo"));
            strHead = strHead.Replace("%TestCount%", GetHeaderValue("TestCount"));
            strHead = strHead.Replace("%CassetteSetInfo%", GetHeaderValue("CassetteSetInfo"));
            strHead = strHead.Replace("%ModelInfo%", GetHeaderValue("ModelInfo"));

            return strHead;
        }

        /// <summary>
        /// Test Data를 txt 파일에 기록한다
        /// </summary>
        /// <param name="sw">기록될 File의 Stream</param>
        private void InsertTsetData(ref StreamWriter sw)
        {
            string inputStr;

            UInt16 iVal;
            UInt16 iData1;
            UInt16 iData2;
            UInt16 iData3;

            foreach (DataRow dr in m_Ds.Tables["DataData"].Rows)
            {
                inputStr = string.Empty;

                iData1 = Convert.ToUInt16(dr[1]);
                iData2 = Convert.ToUInt16(dr[2]);
                iData3 = Convert.ToUInt16(dr[3]);

                iVal = (UInt16)((iData2 >> 11) & 0x01);
                if (iVal == 1) inputStr = inputStr + "-";
                iVal = (UInt16)(iData1 & 0x1FF);
                inputStr = inputStr + iVal.ToString() + "\t";

                iVal = (UInt16)((iData2 >> 10) & 0x01);
                if (iVal == 1) inputStr = inputStr + "-";
                iVal = (UInt16)(iData2 & 0x1FF);
                inputStr = inputStr + iVal.ToString() + "\t";

                iVal = (UInt16)((iData1 >> 14) & 0x03);
                switch (iVal)
                {
                    case 0: inputStr = inputStr + "NoTest\t"; break;
                    case 1: inputStr = inputStr + "PASS\t"; break;
                    case 2: inputStr = inputStr + "FAIL-1\t"; break;
                    case 3: inputStr = inputStr + "FAIL-2\t"; break;
                }

                iVal = (UInt16)((iData2 >> 14) & 0x03);
                switch (iVal)
                {
                    case 0: inputStr = inputStr + "Skip\t"; break;
                    case 1: inputStr = inputStr + "Test\t"; break;
                    case 2: inputStr = inputStr + "Making\t"; break;
                }

                iVal = (UInt16)(iData3 & 0x3F);
                inputStr = inputStr + iVal.ToString() + "\t";

                iVal = (UInt16)((iData3 >> 8) & 0x3F);
                inputStr = inputStr + iVal.ToString() + "\r\n";

                sw.Write(inputStr);
            }

            sw.Write("\r\n");
        }

        public DataSet InsertTable()
        {
            DataSet newDs = null;
            string strHead = string.Empty;

            FileStream fs = null;
            StreamReader sr = null;
            StreamWriter sw = null;


            try
            {
                newDs = new DataSet();
                fs = new FileStream(m_OutFile, System.IO.FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                sr = new StreamReader(fs);

                strHead = sr.ReadToEnd();
                strHead = InsertStringHead(strHead);

                sr.Close();
                fs.Close();
                sr = null;
                fs = null;

                fs = new FileStream(m_OutFile, System.IO.FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                sw = new StreamWriter(fs);

                sw.Write(strHead);

                newDs = InsertTestData(ref sw);


                sw.Close();
                fs.Close();
                sw = null;
                fs = null;

                return newDs;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Test Data를 txt 파일에 기록한다
        /// </summary>
        public DataSet InsertTestData(ref StreamWriter sw)
        {
            string inputStr;

            UInt16 iVal;
            UInt16 iData1;
            UInt16 iData2;
            UInt16 iData3;
            DataSet newDs = null;
            string[] inputData = null;
            int j = 0;

            try
            {
                inputStr = string.Empty;
                newDs = new DataSet();
                newDs.Tables.Add();
                newDs.Tables[0].Columns.Add(new DataColumn("WAFER_SEQ", System.Type.GetType("System.String")));
                newDs.Tables[0].Columns.Add(new DataColumn("DIEID", System.Type.GetType("System.Int32")));
                newDs.Tables[0].Columns.Add(new DataColumn("X", System.Type.GetType("System.Int32")));
                newDs.Tables[0].Columns.Add(new DataColumn("Y", System.Type.GetType("System.Int32")));
                newDs.Tables[0].Columns.Add(new DataColumn("BIN", System.Type.GetType("System.Int32")));
                newDs.Tables[0].Columns.Add(new DataColumn("HBIN", System.Type.GetType("System.Int32")));
                newDs.Tables[0].Columns.Add(new DataColumn("CHARBIN", System.Type.GetType("System.String")));
                newDs.Tables[0].Columns.Add(new DataColumn("SITE", System.Type.GetType("System.Int32")));
                newDs.Tables[0].Columns.Add(new DataColumn("VISUALINSP", System.Type.GetType("System.Int32")));
                newDs.Tables[0].Columns.Add(new DataColumn("AVI", System.Type.GetType("System.Int32")));

                foreach (DataRow dr in m_Ds.Tables["DataData"].Rows)
                {
                    iData1 = Convert.ToUInt16(dr[1]);
                    iData2 = Convert.ToUInt16(dr[2]);
                    iData3 = Convert.ToUInt16(dr[3]);

                    iVal = (UInt16)((iData2 >> 11) & 0x01);
                    if (iVal == 1) inputStr = inputStr + "-";
                    iVal = (UInt16)(iData1 & 0x1FF);
                    inputStr = inputStr + iVal.ToString() + "\t";

                    iVal = (UInt16)((iData2 >> 10) & 0x01);
                    if (iVal == 1) inputStr = inputStr + "-";
                    iVal = (UInt16)(iData2 & 0x1FF);
                    inputStr = inputStr + iVal.ToString() + "\t";

                    iVal = (UInt16)((iData1 >> 14) & 0x03);
                    switch (iVal)
                    {
                        case 0: inputStr = inputStr + "NoTest\t"; break;
                        case 1: inputStr = inputStr + "PASS\t"; break;
                        case 2: inputStr = inputStr + "FAIL-1\t"; break;
                        case 3: inputStr = inputStr + "FAIL-2\t"; break;
                    }

                    iVal = (UInt16)((iData2 >> 14) & 0x03);
                    switch (iVal)
                    {
                        case 0: inputStr = inputStr + "Skip\t"; break;
                        case 1: inputStr = inputStr + "Test\t"; break;
                        case 2: inputStr = inputStr + "Making\t"; break;
                    }

                    iVal = (UInt16)(iData3 & 0x3F);
                    inputStr = inputStr + iVal.ToString() + "\t";

                    iVal = (UInt16)((iData3 >> 8) & 0x3F);
                    inputStr = inputStr + iVal.ToString() + "\n";

                    inputData = inputStr.Split(new char[] { '\t' });

                    for (int i = 0; i < inputData.Length; i++)
                    {
                        if (inputData[i].Contains("NoTest")) inputData[i] = "0";
                        else if (inputData[i].Contains("PASS")) inputData[i] = "1";
                        else if (inputData[i].Contains("FAIL-1")) inputData[i] = "2";

                        if (inputData[i].Contains("Skip")) inputData[i] = "0";
                        else if (inputData[i].Contains("Test")) inputData[i] = "1";
                        else if (inputData[i].Contains("Making")) inputData[i] = "2";
                        else continue;
                    }

                    newDs.Tables[0].Rows.Add(new object[] { j, j, inputData[0], inputData[1], inputData[2], 0, 0, inputData[3], 0, 0 });
                    inputStr = string.Empty;
                    j++;

                }
                return newDs;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public string InsertOutString()
        {
            string strHead;

            FileStream fs = null;
            StreamReader sr = null;
            string[] str = null;

            try
            {
                fs = new FileStream(m_OutFile, System.IO.FileMode.Open);
                sr = new StreamReader(fs);

                strHead = sr.ReadToEnd();
                str = strHead.Split(new char[] { '=', '\r', '\n', '\t' });
                strHead = str[4].Trim();

                return strHead;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sr != null) sr.Close();
                if (fs != null) sr.Close();

                sr = null;
                fs = null;
            }
        }

        /// <summary>
        /// Header Txet를 구성한다
        /// </summary>
        /// <param name="strHead">File에서 읽은 String</param>
        /// <returns>변환된 String</returns>
        public string InsertProgram(string strHead)
        {
            string str;
            double dou;

            strHead = strHead.Replace("%Operator%", GetHeaderValue("Operator"));	// Operator
            strHead = strHead.Replace("%Device%", GetHeaderValue("Device"));	// Device 

            str = GetHeaderValue("WaferSize");		// Wafer Size
            switch (str)
            {
                case "40": str = "4"; break;
                case "45": str = "4.5"; break;
                case "50": str = "5"; break;
                case "60": str = "6"; break;
                case "80": str = "8"; break;
            }
            strHead = strHead.Replace("%WaferSize%", str);

            str = GetHeaderValue("ChipSizeX");			// Index Size X, Chip Size X
            dou = Convert.ToDouble(str) * 0.00001;
            str = Convert.ToString(dou);
            strHead = strHead.Replace("%ChipSizeX%", str);

            str = GetHeaderValue("ChipSizeY");			// Index Size Y, Chip Size Y
            dou = Convert.ToDouble(str) * 0.00001;
            str = Convert.ToString(dou);
            strHead = strHead.Replace("%ChipSizeY%", str);

            strHead = strHead.Replace("%Angle%", GetHeaderValue("Angle"));		// Reference Flat Direction, Angle
            strHead = strHead.Replace("%XDies%", GetHeaderValue("XDies"));		// Map Column Size
            strHead = strHead.Replace("%YDies%", GetHeaderValue("YDies"));		// Map Row Size	
            strHead = strHead.Replace("%WaferID%", GetHeaderValue("WaferID"));				// Wafer ID
            strHead = strHead.Replace("%LotNo%", GetHeaderValue("LotNo"));					// Lot No
            strHead = strHead.Replace("%CassetteNo%", GetHeaderValue("CassetteNo"));		// Cassette No	
            strHead = strHead.Replace("%SlotNo%", GetHeaderValue("SlotNo"));				// Slot No

            str = GetHeaderValue("XDirection");		// X Coordinate Increase Direction, X Direction
            switch (str)
            {
                case "1": str = "Leftward"; break;
                case "2": str = "Rightward"; break;
            }
            strHead = strHead.Replace("%XDirection%", str);

            str = GetHeaderValue("YDirection");		// Y Coordinate Increase Direction, Y Direction
            switch (str)
            {
                case "1": str = "Forward"; break;
                case "2": str = "Inward"; break;
            }
            strHead = strHead.Replace("%YDirection%", str);

            str = GetHeaderValue("ReferenceDieSetting");		// Reference Die Setting
            switch (str)
            {
                case "1": str = "Wafer Center Die"; break;
                case "2": str = "Die selected by teaching"; break;
                case "3": str = "Target Sense Die"; break;
            }
            strHead = strHead.Replace("%ReferenceDieSetting%", str);

            strHead = strHead.Replace("%TargetX%", GetHeaderValue("TargetX"));
            strHead = strHead.Replace("%TargetY%", GetHeaderValue("TargetY"));
            strHead = strHead.Replace("%TargetDieX%", GetHeaderValue("TargetDieX"));
            strHead = strHead.Replace("%TargetDieY%", GetHeaderValue("TargetDieY"));

            str = GetHeaderValue("ProbingStartPosition");		// Probing Start Position
            switch (str)
            {
                case "1": str = "Upper-Left"; break;
                case "2": str = "Upper-Right"; break;
                case "3": str = "Bottom-Left"; break;
                case "4": str = "Bottom-Right"; break;
            }
            strHead = strHead.Replace("%ProbingStartPosition%", str);

            str = GetHeaderValue("ProbingDirection");			// Probing Direction
            switch (str)
            {
                case "1": str = "Leftward"; break;
                case "2": str = "Rightward"; break;
                case "3": str = "Upper"; break;
                case "4": str = "Bottom"; break;
            }
            strHead = strHead.Replace("%ProbingDirection%", str);

            strHead = strHead.Replace("%OriginX%", GetHeaderValue("OriginX"));			// Distance Between Wafer Center And Center Die X, Center Die X	
            strHead = strHead.Replace("%OriginY%", GetHeaderValue("OriginY"));			// Distance Between Wafer Center And Center Die Y, Center Die Y	
            strHead = strHead.Replace("%OriginDieX%", GetHeaderValue("OriginDieX"));	// Center Die Address X 
            strHead = strHead.Replace("%OriginDieY%", GetHeaderValue("OriginDieY"));	// Center Die Address Y

            str = GetHeaderValue("StartTime");		// Wafer Testing Start Time Data  05/11/22 11:10  0511221110
            try
            {
                str = str.Trim();
                str = str.Substring(0, 2) + "/" + str.Substring(2, 2) + "/" + str.Substring(4, 2) + " " + str.Substring(6, 2) + ":" + str.Substring(8, 2);
            }
            catch
            {
                str = "";
            }
            strHead = strHead.Replace("%StartTime%", str);

            str = GetHeaderValue("EndTime");		// Wafer Testing Finish Time Data
            try
            {
                str = str.Trim();
                str = str.Substring(0, 2) + "/" + str.Substring(2, 2) + "/" + str.Substring(4, 2) + " " + str.Substring(6, 2) + ":" + str.Substring(8, 2);
            }
            catch
            {
                str = "";
            }
            strHead = strHead.Replace("%EndTime%", str);

            str = GetHeaderValue("WaferLoadingStartTime");		// Wafer Loading Start Time Data
            try
            {
                str = str.Trim();
                str = str.Substring(0, 2) + "/" + str.Substring(2, 2) + "/" + str.Substring(4, 2) + " " + str.Substring(6, 2) + ":" + str.Substring(8, 2);
            }
            catch
            {
                str = str.Trim();
                str = "";
            }
            strHead = strHead.Replace("%WaferLoadingStartTime%", str);

            str = GetHeaderValue("WaferUnloadingStartTime");	// Wafer Unloading Start Time Data
            try
            {
                str = str.Substring(0, 2) + "/" + str.Substring(2, 2) + "/" + str.Substring(4, 2) + " " + str.Substring(6, 2) + ":" + str.Substring(8, 2);
            }
            catch
            {
                str = "";
            }
            strHead = strHead.Replace("%WaferUnloadingStartTime%", str);

            str = GetHeaderValue("TestingFinishStatus");	// Testing Finish Status
            switch (str)
            {
                case "0": str = "Testing correct finish"; break;
                case "1": str = "Yield NG"; break;
                case "2": str = "Continuity Fail NG"; break;
            }
            strHead = strHead.Replace("%TestingFinishStatus%", str);

            strHead = strHead.Replace("%TestDies%", GetHeaderValue("TestDies"));	// Total Tested Dies
            strHead = strHead.Replace("%PassDies%", GetHeaderValue("PassDies"));	// Total Pass Dies	
            strHead = strHead.Replace("%FailDies%", GetHeaderValue("FailDies"));	// Total Fail Dies	
            strHead = strHead.Replace("%AreaStartAdress%", GetHeaderValue("AreaStartAdress"));	// Testing Result Storage Area Start Adress

            return strHead;
        }

        public string InsertStringHead(string strHead)
        {
            return strHead;
        }

        #endregion ---------------------------------------------------------------

        #region ' Txt Parsing '

        public string[,] WriteRowData(ref DataTable dtBin, ref HeaderData hd, ref RowData[] rd)
        {
                     
            string[,] arrBin = null;
            int xmax = 0;
            int ymax = 0;

            try
            {
                xmax = Convert.ToInt32(dtBin.Compute("MAX(X)", null).ToString());
                ymax = Convert.ToInt32(dtBin.Compute("MAX(Y)", null).ToString());

                arrBin = new string[xmax + 1, ymax + 1];

                rd = new RowData[hd.TestDies];
                int h = 0;

                while (h < rd.Length)
                {
                    foreach (DataRow dr in dtBin.Rows)
                    {
                        rd[h].DieX = Int32.Parse(dr["X"].ToString());
                        rd[h].DieY = Int32.Parse(dr["Y"].ToString());
                        rd[h].Bin = Int32.Parse(dr["BIN"].ToString());
                        rd[h].HBin = Int32.Parse(dr["HBIN"].ToString());
                        rd[h].CharBin = dr["CHARBIN"].ToString();

                        arrBin[rd[h].DieX, rd[h].DieY] = rd[h].Bin.ToString();

                        h++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return arrBin;
        }

        #endregion -------------------------------------------------------------------------------------

        #endregion -------------------------------------------------------------------------------- Process
    }
}
