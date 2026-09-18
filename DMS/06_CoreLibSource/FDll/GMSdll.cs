using System;
using System.IO;
using System.Text;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;

namespace FDll
{
    public class GMSdll : Fdll
    {
        string productName = null;
        //int xmin = 0;
        //int ymin = 0;
        //int xmax = 0;
        //int ymax = 0;
        int angle = 0;
        int netDie = 0;
        //int tmpCnt = 0;

        /// <summary>
        ///  생성자
        /// </summary>
        public GMSdll()
        {
            
        }

        /// <summary>
        /// 소멸자
        /// </summary>
        ~GMSdll()
        {
        }


        #region -------------------------------------------------------------------------------- Interface Function

        /// <summary>
        /// InitData
        /// </summary>
        public void InitData()
        {
            SetDataSetXmlFile(sFormatDataPath + "gms.xml", true);
            DataSetToStruct();
        }

        /// <summary>
        /// GMS Binary File -> DataSet
        /// </summary>
        /// <param name="binaryFile">GMS Data File</param>
        public void ReadLotBinaryFile(string binaryFile)
        {
            SetDataSetXmlFile(sFormatDataPath + "gms_lot.xml", true);
            ReadBinaryDataFileGMS(binaryFile, "LOT");
            DataSetToStruct("LOT");
        }

        public DataSet ReadWaferBinaryFile(ref HeaderData headerData, string binaryFile)
        {
            SetDataSetXmlFile(sFormatDataPath + "gms_wafer.xml", true);
            ReadBinaryDataFileGMS(binaryFile,"WAFER");

            DataSetToStruct("WAFER");
            m_HeaderData.TestArea = headerData.TestArea;
            angle = headerData.Angle = m_HeaderData.Angle;
            
            headerData = m_HeaderData;

            return GetGMSDataSet();
        }

        /// <summary>
        /// DataSet -> GMS Binary File 
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
            SetDataSetXmlFile(sFormatDataPath + "gms.xml", true);
            ReadBinaryDataFileGMS(binaryFile, "WAFER");
            OutTxtFile(sFormatDataPath + "gms.tf", outFile);
        }

        public DataSet OutTextFile1(string binaryFile, string outFile)
        {
            DataSet ds = null;
            SetDataSetXmlFile(sFormatDataPath + "gms.xml", true);
            ReadBinaryDataFileGMS(binaryFile,"WAFER");
            ds = OutTxtFile1(sFormatDataPath + "gms.tf", outFile);
            return ds;
        }

        /// <summary>
        /// DataSet -> Text로 출력
        /// </summary>
        /// <param name="tskBinaryFile">TSK Data File</param>
        /// <param name="outFilePath">Text File</param>
        public void OutTextFile(string outFile)
        {
            OutTxtFile(sFormatDataPath + "gms.tf", outFile);
        }

        /// <summary>
        /// Txt file -> Header Data, Bin data
        /// </summary>
        /// <param name="binaryFile">TSK Data File</param>
        public string[] ReadTxtFile(string filePath, ref HeaderData hd)
        {
            FileStream fs = null;
            StreamReader sr = null;
            DataTable binDt = null;
            string str = string.Empty;
            int[] binItem = null;
            string[] splitTxt = null;
            string[] binChr = null;
            string[] arrBin = null;

            try
            {
                binItem = new int[100];
                binChr = new string[100];
                binDt = new DataTable();

                //File Read
                Encoding encode = System.Text.Encoding.GetEncoding("UTF-8");
                fs = new FileStream(filePath, System.IO.FileMode.Open, FileAccess.Read);	// File Open
                sr = new StreamReader(fs, encode);
                str = sr.ReadToEnd();
                sr.Close();

                splitTxt = str.Split(new string[] { "[ Wafer Map ]" }, StringSplitOptions.RemoveEmptyEntries);

                // Header Info
                string[] arrHeder = splitTxt[0].Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                hd.Device = arrHeder[0].Split('=')[1].Replace(" ", "");
                hd.LotNo = arrHeder[1].Split('=')[1].Replace(" ", "");
                hd.WaferID = arrHeder[2].Split('=')[1].Replace(" ", "");
                hd.StartTime = arrHeder[3].Split('=')[1].TrimStart();
                hd.EndTime = arrHeder[4].Split('=')[1].TrimStart();
                //dieSizeX = Convert.ToDouble(arrHeder[10].Split(':')[1]);
                //dieSizeY = Convert.ToDouble(arrHeder[11].Split(':')[1]);
                angle = GetAngle(arrHeder[7].Split('=')[1]);

                // Bin Data
                string strBins = splitTxt[1].Split(new string[] {"[ Wafer Bin Summary ]"}, StringSplitOptions.RemoveEmptyEntries)[0];
                arrBin = strBins.Split(new string[]{"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

                return arrBin;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }

        #endregion -------------------------------------------------------------------------------- Interface Function

        #region -------------------------------------------------------------------------------- DataConvert

        protected override void DataSetToStruct()
        {
        }

        protected override void DataSetToStruct(string LotWafer)
        {
            try
            {
                if (LotWafer == "LOT")
                {
                    m_HeaderData.LotNo = GetHeaderValue("LotNo").Replace(" ", "");
                    m_HeaderData.CardNo = GetHeaderValue("CardNo");
                    m_HeaderData.Operator = GetHeaderValue("OperatorName");
                    m_HeaderData.MachineNo = GetHeaderValue("MachineNo");
                    m_HeaderData.WaferNoList = GetHeaderValueInt("WaferNoList");
                    m_HeaderData.WaferName = GetHeaderValue("WaferName").Replace(" ", "");
                    m_HeaderData.WaferSize = GetHeaderValueInt("WaferSize") * 2.5;
                    m_HeaderData.Angle = GetHeaderValueInt("OrientationFlat");
                    m_HeaderData.ChipSizeX = GetHeaderValueDouble("DieSizeX") / 100000;
                    m_HeaderData.ChipSizeY = GetHeaderValueDouble("DieSizeY") / 100000;
                    m_HeaderData.LightingMode = GetHeaderValueInt("LightingMode");
                    m_HeaderData.StartPosition = GetHeaderValueInt("StartPosition");
                    m_HeaderData.MicroPosition = GetHeaderValueInt("MicroPosition");
                    m_HeaderData.AlignmentAxis = GetHeaderValue("AlignmentAxis");
                    m_HeaderData.AutoFocus = GetHeaderValueInt("AutoFocus");
                    m_HeaderData.AlignmentX = GetHeaderValueInt("AlignmentSizeX");
                    m_HeaderData.AlignmentY = GetHeaderValueInt("AlignmentSizeY");
                    GetHeaderValue("Reserved", 15);
                    m_HeaderData.ProbeSize = GetHeaderValueDouble("ProbeSize");
                    m_HeaderData.TargetMode = GetHeaderValueInt("TargetMode");
                    m_HeaderData.TargetPositionX = GetHeaderValueInt("TargetPositionX");
                    m_HeaderData.TargetPositionY = GetHeaderValueInt("TargetPositionY");
                    GetHeaderValue("Reserved", 19);
                    m_HeaderData.StdPositionX = GetHeaderValueInt("StandardDiePositionX");
                    m_HeaderData.StdPositionY = GetHeaderValueInt("StandardDiePositionX");
                    GetHeaderValue("Reserved", 7);
                    m_HeaderData.OrientFlatSelect = GetHeaderValue("OrientationFlatSelection");
                    m_HeaderData.NumOrientationFlat = GetHeaderValueInt("NumberOfOrientationFlats");
                    m_HeaderData.OrientFlatPosition = GetHeaderValueInt("OrientationFlatPosition");
                    m_HeaderData.ProbeAreaSel = GetHeaderValue("ProbeAreaSelection");
                    m_HeaderData.InkerOffset = GetHeaderValueInt("InkerOffset");
                    m_HeaderData.SampleProbeMode = GetHeaderValueInt("SampleProbeMode");
                    m_HeaderData.SampleStep1X = GetHeaderValueInt("SampleStep1X");
                    m_HeaderData.SampleStep1Y = GetHeaderValueInt("SampleStep1Y");
                    m_HeaderData.SampleStep2X = GetHeaderValueInt("SampleStep2X");
                    m_HeaderData.SampleStep2Y = GetHeaderValueInt("SampleStep2Y");
                    m_HeaderData.SampleStep3X = GetHeaderValueInt("SampleStep3X");
                    m_HeaderData.SampleStep3Y = GetHeaderValueInt("SampleStep3Y");
                    m_HeaderData.SampleStep4X = GetHeaderValueInt("SampleStep4X");
                    m_HeaderData.SampleStep4Y = GetHeaderValueInt("SampleStep4Y");
                    m_HeaderData.SampleStep5X = GetHeaderValueInt("SampleStep5X");
                    m_HeaderData.SampleStep5Y = GetHeaderValueInt("SampleStep5Y");
                    m_HeaderData.SampleStep6X = GetHeaderValueInt("SampleStep6X");
                    m_HeaderData.SampleStep6Y = GetHeaderValueInt("SampleStep6Y");
                    m_HeaderData.SampleStep7X = GetHeaderValueInt("SampleStep7X");
                    m_HeaderData.SampleStep7Y = GetHeaderValueInt("SampleStep7Y");
                    m_HeaderData.SampleStep8X = GetHeaderValueInt("SampleStep8X");
                    m_HeaderData.SampleStep8Y = GetHeaderValueInt("SampleStep8Y");
                    m_HeaderData.SampleStep9X = GetHeaderValueInt("SampleStep9X");
                    m_HeaderData.SampleStep9Y = GetHeaderValueInt("SampleStep9Y");
                    m_HeaderData.SampleStep10X = GetHeaderValueInt("SampleStep10X");
                    m_HeaderData.SampleStep10Y = GetHeaderValueInt("SampleStep10Y");
                    m_HeaderData.MonitorDieX = GetHeaderValueInt("MonitorDieX");
                    m_HeaderData.MonitorDieY = GetHeaderValueInt("MonitorDieY");
                    m_HeaderData.MonitorDieSizeX = GetHeaderValueInt("MonitorDieSizeX");
                    m_HeaderData.MonitorDieSizeY = GetHeaderValueInt("MonitorDieSizeY");
                    m_HeaderData.MultiDieMode = GetHeaderValueInt("MultiDieMode");
                    m_HeaderData.MultiDieLocation = GetHeaderValueInt("MultiDieLocation");
                    m_HeaderData.ContinuousFail = GetHeaderValueInt("ContinuousFail");
                    m_HeaderData.CheckBack = GetHeaderValueInt("CheckBack");
                    m_HeaderData.ContinuousFailCnt = GetHeaderValueInt("ContinuousFailCount");
                    m_HeaderData.SkipDieLine = GetHeaderValueInt("SkipDieLine");
                    m_HeaderData.CheckBackCnt = GetHeaderValueInt("CheckBackCount");
                    m_HeaderData.RejectWaferCnt = GetHeaderValueInt("RejectWaferCount");
                    m_HeaderData.ChkBackNeedlePolish = GetHeaderValueInt("CheckBackAfterNeedlePolish");
                    m_HeaderData.ProbeNeedlePolish = GetHeaderValueInt("ProbeNeedlePolish");
                    m_HeaderData.ZCount = GetHeaderValueInt("ZCount");
                    m_HeaderData.DieCount = GetHeaderValueInt("DieCount");
                    m_HeaderData.WaferCount = GetHeaderValueInt("WaferCount");
                    m_HeaderData.Overdrive = GetHeaderValueInt("Overdrive");
                    m_HeaderData.ExecutionCnt = GetHeaderValueInt("ExecutionCount");
                    m_HeaderData.SettingTemperature = GetHeaderValueInt("SettingTemperature");
                    m_HeaderData.PresetAddressX = GetHeaderValueInt("PresetAddressX");
                    m_HeaderData.PresetAddressY = GetHeaderValueInt("PresetAddressY");
                    m_HeaderData.MarkingMode = GetHeaderValueInt("MarkingMode");
                    GetHeaderValue("Reserved", 59);
                    m_HeaderData.PassDies = GetHeaderValueInt("PassTotal");
                    m_HeaderData.FailDies = GetHeaderValueInt("FailTotal");
                    m_HeaderData.TestDies = GetHeaderValueInt("TestTotal");
                    m_HeaderData.LotStartTime = GetHeaderValueInt("LotStartTime").ToString();
                    m_HeaderData.LotEndTime = GetHeaderValueInt("LotEndTime").ToString();
                    m_HeaderData.CassetteNo = GetHeaderValueInt("CassetteNo");
                    m_HeaderData.SlotNo = GetHeaderValueInt("SlotNo");
                    m_HeaderData.TestCount = GetHeaderValueInt("TestCount");
                    m_HeaderData.CassetteSetInfo = GetHeaderValueInt("CassetteSetInformation");
                    m_HeaderData.ModelInfo = GetHeaderValue("Modelinformation");
                }

                else if (LotWafer == "WAFER")
                {
                    GetHeaderValue("dummy");
                    m_HeaderData.WaferNo = GetHeaderValueInt("WaferNo");
                    m_HeaderData.CassetteNo = GetHeaderValueInt("CassetteNo");
                    m_HeaderData.SlotNo = GetHeaderValueInt("SlotNo");
                    m_HeaderData.TestCount = GetHeaderValueInt("TestCount");
                    m_HeaderData.PassDies = GetHeaderValueInt("PassTotal");
                    m_HeaderData.TestDies = GetHeaderValueInt("TestTotal");
                    m_HeaderData.StartTime = GetHeaderValue("StartTime");
                    m_HeaderData.EndTime = GetHeaderValue("EndTime");
                    m_HeaderData.Records = GetHeaderValue("Records");
                    m_HeaderData.OriginDieX = GetHeaderValueInt("OriginX");
                    m_HeaderData.OriginDieY = GetHeaderValueInt("OriginY");
                    m_HeaderData.FirstDieX = GetHeaderValueInt("FirstX");
                    m_HeaderData.FirstDieY = GetHeaderValueInt("FirstY");
                    
                }
                //int i;

                //i = m_Ds.Tables["DataData"].Rows.Count;

                //if (m_RowDatas != null) m_RowDatas = null;
                //m_RowDatas = new RowData[i];

                //int val = 0;
                //UInt16 iVal;
                //UInt16 iData1;
                //UInt16 iData2;
                //UInt16 iData3;
                //string str = string.Empty;

                //i = 0;

                //foreach (DataRow dr in m_Ds.Tables["DataData"].Rows)
                //{

                //    iData1 = Convert.ToUInt16(dr[1]);
                //    iData2 = Convert.ToUInt16(dr[2]);
                //    iData3 = Convert.ToUInt16(dr[3]);

                //    iVal = (UInt16)((iData1 >> 14) & 0x03);
                //    m_RowDatas[i].DieTestResult = Convert.ToInt32(iVal);

                //    iVal = (UInt16)((iData1 >> 13) & 0x01);
                //    m_RowDatas[i].Marking = Convert.ToInt32(iVal);

                //    iVal = (UInt16)((iData1 >> 12) & 0x01);
                //    m_RowDatas[i].FailMarkInsp = Convert.ToInt32(iVal);

                //    iVal = (UInt16)((iData1 >> 11) & 0x03);
                //    m_RowDatas[i].ReProbing = Convert.ToInt32(iVal);

                //    iVal = (UInt16)((iData1 >> 9) & 0x01);
                //    m_RowDatas[i].NeddleInspResult = Convert.ToInt32(iVal);

                //    iVal = (UInt16)(iData1 & 0x1FF);
                //    val = iVal;
                //    if (((iData2 >> 11) & 0x01) == 1) val = val * -1;
                //    m_RowDatas[i].DieX = Convert.ToInt32(val);

                //    iVal = (UInt16)(iData2 >> 14 & 0x03);
                //    m_RowDatas[i].DieAttribute = Convert.ToInt32(iVal);

                //    iVal = (UInt16)((iData2 >> 13) & 0x01);
                //    m_RowDatas[i].NeddleInspTarget = Convert.ToInt32(iVal);

                //    iVal = (UInt16)((iData2 >> 12) & 0x01);
                //    m_RowDatas[i].SamplingDie = Convert.ToInt32(iVal);

                //    iVal = (UInt16)((iData2 >> 9) & 0x01);
                //    m_RowDatas[i].DummyData = Convert.ToInt32(iVal);

                //    iVal = (UInt16)(iData2 & 0x1FF);
                //    val = iVal;
                //    if (((iData2 >> 10) & 0x01) == 1) val = val * -1;
                //    m_RowDatas[i].DieY = Convert.ToInt32(val);

                //    iVal = (UInt16)((iData3 >> 15) & 0x01);
                //    m_RowDatas[i].NotOutput = Convert.ToInt32(iVal);

                //    iVal = (UInt16)((iData3 >> 14) & 0x01);
                //    m_RowDatas[i].ProbingDie = Convert.ToInt32(iVal);

                //    iVal = (UInt16)((iData3 >> 13) & 0x3F);
                //    m_RowDatas[i].TestSiteNo = Convert.ToInt32(iVal);

                //    iVal = (UInt16)((iData3 >> 7) & 0x03);
                //    m_RowDatas[i].BlockArea = Convert.ToInt32(iVal);

                //    iVal = (UInt16)(iData3 & 0x3F);
                //    m_RowDatas[i].Bin = Convert.ToInt32(iVal);

                //    i++;
                //}
            }
            catch
            {
            }
            finally
            {
            }
        }

        protected override void StructToDataSet()
        {
            try
            {
                SetHeaderValue("LotNo", m_HeaderData.LotNo);
                SetHeaderValue("CardNo", m_HeaderData.CardNo);
                SetHeaderValue("Operator", m_HeaderData.Operator);
                SetHeaderValue("MachineNo", Convert.ToString(m_HeaderData.MachineNo));
                SetHeaderValue("WaferNoPlus", Convert.ToString(m_HeaderData.WaferNoPlus));
                SetHeaderValue("WaferNoMinus", Convert.ToString(m_HeaderData.WaferNoMinus));
                SetHeaderValue("WaferName", m_HeaderData.WaferName);
                SetHeaderValue("WaferSize", Convert.ToString(m_HeaderData.WaferSize / 2.5));
                SetHeaderValue("Angle", Convert.ToString(m_HeaderData.Angle));
                SetHeaderValue("ChipSizeX", Convert.ToString(m_HeaderData.ChipSizeX * 100000));
                SetHeaderValue("ChipSizeY", Convert.ToString(m_HeaderData.ChipSizeY * 100000));
                SetHeaderValue("LightingMode", Convert.ToString(m_HeaderData.LightingMode));
                SetHeaderValue("StartPosition", Convert.ToString(m_HeaderData.StartPosition));
                SetHeaderValue("MicroPosition", Convert.ToString(m_HeaderData.MicroPosition));
                SetHeaderValue("AlignmentAxis", m_HeaderData.AlignmentAxis);
                SetHeaderValue("AutoFocus", Convert.ToString(m_HeaderData.AutoFocus));
                SetHeaderValue("AlignmentX", Convert.ToString(m_HeaderData.AlignmentX));
                SetHeaderValue("AlignmentY", Convert.ToString(m_HeaderData.AlignmentY));
                SetHeaderValue("ProbeSize", Convert.ToString(m_HeaderData.ProbeSize));
                SetHeaderValue("TargetMode", Convert.ToString(m_HeaderData.TargetMode));
                SetHeaderValue("TargetPositionX", Convert.ToString(m_HeaderData.TargetPositionX));
                SetHeaderValue("TargetPositionY", Convert.ToString(m_HeaderData.TargetPositionY));
                SetHeaderValue("StdPositionX", Convert.ToString(m_HeaderData.StdPositionX));
                SetHeaderValue("StdPositionY", Convert.ToString(m_HeaderData.StdPositionY));
                SetHeaderValue("OrientFlatSelect", m_HeaderData.OrientFlatSelect);
                SetHeaderValue("NumOrientationFlat", Convert.ToString(m_HeaderData.NumOrientationFlat));
                SetHeaderValue("OrientFlatPosition", Convert.ToString(m_HeaderData.OrientFlatPosition));
                SetHeaderValue("ProbeAreaSel", m_HeaderData.ProbeAreaSel);
                SetHeaderValue("InkerOffset", Convert.ToString(m_HeaderData.InkerOffset));
                SetHeaderValue("SampleProbeMode", Convert.ToString(m_HeaderData.SampleProbeMode));
                SetHeaderValue("SampleStep1X", Convert.ToString(m_HeaderData.SampleStep1X));
                SetHeaderValue("SampleStep1Y", Convert.ToString(m_HeaderData.SampleStep1Y));
                SetHeaderValue("SampleStep2X", Convert.ToString(m_HeaderData.SampleStep2X));
                SetHeaderValue("SampleStep2Y", Convert.ToString(m_HeaderData.SampleStep2Y));
                SetHeaderValue("SampleStep3X", Convert.ToString(m_HeaderData.SampleStep3X));
                SetHeaderValue("SampleStep3Y", Convert.ToString(m_HeaderData.SampleStep3Y));
                SetHeaderValue("SampleStep4X", Convert.ToString(m_HeaderData.SampleStep4Y));
                SetHeaderValue("SampleStep4Y", Convert.ToString(m_HeaderData.SampleStep4Y));
                SetHeaderValue("SampleStep5X", Convert.ToString(m_HeaderData.SampleStep5X));
                SetHeaderValue("SampleStep5Y", Convert.ToString(m_HeaderData.SampleStep5Y));
                SetHeaderValue("SampleStep6X", Convert.ToString(m_HeaderData.SampleStep6X));
                SetHeaderValue("SampleStep6Y", Convert.ToString(m_HeaderData.SampleStep6Y));
                SetHeaderValue("SampleStep7X", Convert.ToString(m_HeaderData.SampleStep7X));
                SetHeaderValue("SampleStep7Y", Convert.ToString(m_HeaderData.SampleStep7Y));
                SetHeaderValue("SampleStep8X", Convert.ToString(m_HeaderData.SampleStep8X));
                SetHeaderValue("SampleStep8Y", Convert.ToString(m_HeaderData.SampleStep8Y));
                SetHeaderValue("SampleStep9X", Convert.ToString(m_HeaderData.SampleStep9X));
                SetHeaderValue("SampleStep9Y", Convert.ToString(m_HeaderData.SampleStep9Y));
                SetHeaderValue("SampleStep10X", Convert.ToString(m_HeaderData.SampleStep10X));
                SetHeaderValue("SampleStep10Y", Convert.ToString(m_HeaderData.SampleStep10Y));
                SetHeaderValue("MonitorDieX", Convert.ToString(m_HeaderData.MonitorDieX));
                SetHeaderValue("MonitorDieY", Convert.ToString(m_HeaderData.MonitorDieY));
                SetHeaderValue("MonitorDieSizeX", Convert.ToString(m_HeaderData.MonitorDieSizeX));
                SetHeaderValue("MonitorDieSizeY", Convert.ToString(m_HeaderData.MonitorDieSizeY));
                SetHeaderValue("MultiDieMode", Convert.ToString(m_HeaderData.MultiDieMode));
                SetHeaderValue("MultiDieLocation", Convert.ToString(m_HeaderData.MultiDieLocation));
                SetHeaderValue("ContinuousFail", Convert.ToString(m_HeaderData.ContinuousFail));
                SetHeaderValue("CheckBack", Convert.ToString(m_HeaderData.CheckBack));
                SetHeaderValue("ContinuousFailCnt", Convert.ToString(m_HeaderData.ContinuousFailCnt));
                SetHeaderValue("SkipDieLine", Convert.ToString(m_HeaderData.SkipDieLine));
                SetHeaderValue("CheckBackCnt", Convert.ToString(m_HeaderData.CheckBackCnt));
                SetHeaderValue("RejectWaferCnt", Convert.ToString(m_HeaderData.InkerOffset));
                SetHeaderValue("ChkBackNeedlePolish", Convert.ToString(m_HeaderData.ChkBackNeedlePolish));
                SetHeaderValue("ProbeNeedlePolish", Convert.ToString(m_HeaderData.ProbeNeedlePolish));
                SetHeaderValue("ZCount", Convert.ToString(m_HeaderData.ZCount));
                SetHeaderValue("DieCount", Convert.ToString(m_HeaderData.DieCount));
                SetHeaderValue("WaferCount", Convert.ToString(m_HeaderData.WaferCount));
                SetHeaderValue("Overdrive", Convert.ToString(m_HeaderData.Overdrive));
                SetHeaderValue("ExecutionCnt", Convert.ToString(m_HeaderData.ExecutionCnt));
                SetHeaderValue("SettingTemperature", Convert.ToString(m_HeaderData.SettingTemperature));
                SetHeaderValue("PresetAddressX", Convert.ToString(m_HeaderData.PresetAddressX));
                SetHeaderValue("PresetAddressY", Convert.ToString(m_HeaderData.PresetAddressY));
                SetHeaderValue("MarkingMode", Convert.ToString(m_HeaderData.MarkingMode));
                SetHeaderValue("PassDies", Convert.ToString(m_HeaderData.PassDies));
                SetHeaderValue("FailDies", Convert.ToString(m_HeaderData.FailDies));
                SetHeaderValue("TestDies", Convert.ToString(m_HeaderData.TestDies));
                SetHeaderValue("LotStartTime", m_HeaderData.LotStartTime);
                SetHeaderValue("LotEndTime", m_HeaderData.LotEndTime);
                SetHeaderValue("CassetteNo", Convert.ToString(m_HeaderData.CassetteNo));
                SetHeaderValue("SlotNo", Convert.ToString(m_HeaderData.SlotNo));
                SetHeaderValue("TestCount", Convert.ToString(m_HeaderData.TestCount));
                SetHeaderValue("CassetteSetInfo", Convert.ToString(m_HeaderData.CassetteSetInfo));
                SetHeaderValue("ModelInfo", m_HeaderData.ModelInfo);

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
            GMSOutText tot = new GMSOutText(formatFilePath, outFilePath, ref m_Ds);
            tot.Act1();
            return 1;
        }

        protected override DataSet OutTxtFile1(string formatFilePath, string outFilePath)
        {
            DataSet ds = null;
            GMSOutText tot = new GMSOutText(formatFilePath, outFilePath, ref m_Ds);
            ds = tot.Act1();
            return ds;
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

        #endregion -------------------------------------------------------------------------------- Text File Read, Write

        #region -------------------------------------------------------------------------------- Map Setup

        //public void BinCreate(int cnt, int[] bin, string[] binChr, ref HeaderData hd)
        //{
        //    string[,] strBinInfo = null;
        //    ADMIN_DATA oAdmin = null;
        //    try
        //    {
        //        oAdmin = new ADMIN_DATA();
        //        strBinInfo = new string[cnt, 13];

        //        for (int j = 0; j < cnt; j++)
        //        {

        //            strBinInfo[j, 0] = productName;
        //            strBinInfo[j, 1] = productName;
        //            strBinInfo[j, 2] = "MULTIPROBE";
        //            strBinInfo[j, 3] = bin[j].ToString();
        //            strBinInfo[j, 4] = bin[j].ToString();
        //            strBinInfo[j, 5] = "BIN" + bin[j].ToString();
        //            strBinInfo[j, 6] = binChr[j].ToString();
        //            if (binChr[j].ToString() == "1" || binChr[j].ToString() == "Y" || binChr[j].ToString() == "X" && tmpCnt == hd.PassDies)
        //            {
        //                strBinInfo[j, 7] = "T";
        //            }
        //            else strBinInfo[j, 7] = "F";
        //            strBinInfo[j, 8] = "T";
        //            if (binChr[j].ToString() == "1" || binChr[j].ToString() == "Y" || binChr[j].ToString() == "X" && tmpCnt == hd.PassDies)
        //            {
        //                strBinInfo[j, 9] = Convert.ToString(-1);
        //                strBinInfo[j, 10] = Convert.ToString(10);
        //                strBinInfo[j, 11] = oAdmin.ColorString(1);
        //                strBinInfo[j, 12] = "Good Electrical Chip";
        //            }
        //            else
        //            {
        //                strBinInfo[j, 9] = Convert.ToString(10);
        //                strBinInfo[j, 10] = Convert.ToString(0);
        //                strBinInfo[j, 11] = oAdmin.ColorString(bin[j]);
        //                strBinInfo[j, 12] = "Fail Chip";
        //            }
        //        }
        //        oAdmin.CreateBins(strBinInfo);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #endregion -------------------------------------------------------------------------------- Map Setup

        #region ------------------------------------------------------------------------ Return Data

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

        #endregion --------------------------------------------------------------------------------

        public DataTable ConvertToArrayMapdata(DataSet dsMapData, ref HeaderData hd, DataTable dtBinDesc)
        {
            DataTable dtBin;
            DataTable dtRowData;
            string[] arrBin;
            string[] arrGecBin;
            int Good = 0;
            int X_StratPoint = 1000;
            int Y_StartPoint = 0;
            //bool bFindX = false;
            bool bFindY = false;

            try
            {
                // Get Map Data from DataSet
                dtBin = dsMapData.Tables["DataData"];

                // Bin Info Setting
                dtRowData = new DataTable();
                dtRowData.Columns.Add(new DataColumn("X", typeof(int)));
                dtRowData.Columns.Add(new DataColumn("Y", typeof(int)));
                dtRowData.Columns.Add(new DataColumn("BIN", typeof(int)));
                dtRowData.Columns.Add(new DataColumn("HBIN", typeof(int)));
                dtRowData.Columns.Add(new DataColumn("CHARBIN", typeof(string)));

                // Row Data put from DataTable to array
                arrBin = new string[dtBin.Rows.Count];
                for (int i = 0; i < arrBin.Length; i++)
                {
                    arrBin[i] = dtBin.Rows[i]["VALUE"].ToString();
                }
                productName = hd.Device;

                // Bin Desc Select
                DataRow[] drGecBin = null;
                if(dtBinDesc != null && dtBinDesc.Rows.Count > 0)
                    drGecBin = dtBinDesc.Select("HIGH_GEC = 'T'");

                arrGecBin = new string[drGecBin.Length];
                for (int i = 0; i < drGecBin.Length; i++)
                {
                    arrGecBin[i] = drGecBin[i]["BIN"].ToString();
                }

                //DataTable dtGecBin = oMapData.Select
                if (dtBinDesc == null || dtBinDesc.Rows.Count < 1)
                    throw new Exception(string.Format("The Default Bin is not exist. Customer [{0}], Product [{1}], TestArea [{2}], Format[{3}]", hd.Customer, hd.Device, hd.TestArea, hd.Format));

                // BinDesc Array
                int iBinInfoSize = dtBinDesc.Rows.Count;
                string[,] arrBinInfo = new string[iBinInfoSize, 2];
                for (int i = 0; i < iBinInfoSize; i++)
                {
                    arrBinInfo[i, 0] = dtBinDesc.Rows[i]["BIN"].ToString(); // BIN
                    arrBinInfo[i, 1] = dtBinDesc.Rows[i]["IN_CHAR_BIN"].ToString(); // CHAR_BIN
                }

                string strBinAll = string.Empty;

                for (int Y = 0; Y < arrBin.Length; Y++)
                {
                    strBinAll += (arrBin[Y] + "\n");
                }

                // Start Point Find 
                for (int Y = 0; Y < arrBin.Length; Y++)
                {
                    for (int X = 0; X < arrBin[Y].Length; X++)
                    {
                        string strBin = arrBin[Y].Substring(X, 1);

                        // Y Start Point Find
                        if (strBin != " " && strBin != "#" && strBin != "X" && !bFindY)
                        {
                            Y_StartPoint = Y;
                            bFindY = true;
                        }

                        if (strBin != " " && strBin != "#" && strBin != "X" )
                        {
                            if (X_StratPoint > X)
                                X_StratPoint = X;
                        }
                    }
                }

                // Bin Data put from Row Data
                for (int Y = Y_StartPoint; Y < arrBin.Length; Y++)
                {
                    for (int X = X_StratPoint ; X < arrBin[Y].Length; X++)
                    {
                        DataRow dr = dtRowData.NewRow();
                        string strBin = arrBin[Y].Substring(X, 1);

                        // Bin : " " == NULL BIN, "#" == SKIP, "X" == SKIP
                        if (strBin != " " && strBin != "#" && strBin != "X")
                        {
                            dr["X"] = X - X_StratPoint + 1;
                            dr["Y"] = Y - Y_StartPoint + 1;

                            int binNum_Index = -1;
                            for (int i = 0; i < iBinInfoSize; i++)
                            {
                                if (arrBinInfo[i, 1] == strBin)
                                    binNum_Index = i;
                            }
                            if (binNum_Index < 0)
                            {
                                throw new Exception(string.Format("LotID : {0}, WaferID : {1}, CharBin : {2} is not defined on BINDESC", hd.LotNo, hd.WaferID, strBin));
                            }

                            dr["BIN"] = arrBinInfo[binNum_Index, 0];
                            dr["HBIN"] = arrBinInfo[binNum_Index, 0];
                            dr["CHARBIN"] = strBin;
                            dtRowData.Rows.Add(dr);

                            foreach (string bin in arrGecBin)
                            {
                                if (bin == dr["BIN"].ToString())
                                {
                                    Good++;
                                    continue;
                                }
                            }
                        }
                    }
                }

                // TestDie Count
                hd.TestDies = netDie = dtRowData.Rows.Count;
                hd.FailDies = hd.TestDies - Good;
                hd.iMinX = Convert.ToInt32(dtRowData.Compute("MIN(X)", null));
                hd.iMinY = Convert.ToInt32(dtRowData.Compute("MIN(Y)", null));
                hd.iMaxX = Convert.ToInt32(dtRowData.Compute("MAX(X)", null));
                hd.iMaxY = Convert.ToInt32(dtRowData.Compute("MAX(Y)", null));

                string replaceBinAll = strBinAll.Replace("#", " ").Replace("X", " ").Replace(" ", "");
                string[] splitAll = replaceBinAll.Split("\n\r".ToCharArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dtRowData;
        }

        public static bool CheckNumber(string letter)
        {
            bool IsCheck = true;

            Regex numRegex = new Regex(@"[0-9]?[0-9]");
            Boolean isMatch = numRegex.IsMatch(letter);

            if (!isMatch)
            {
                IsCheck = false;
            }
            return IsCheck;
        }

        public int GetAngle(string strAngle)
        {
            int iAngle = 180;

            try
            {
                switch (strAngle)
                {

                    case "TOP":
                        iAngle = 0;
                        break;

                    case "LEFT":
                        iAngle = 90;
                        break;

                    case "BOTTOM":
                        iAngle = 180;
                        break;

                    case "RIGHT":
                        iAngle = 270;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return iAngle;
        }
    }
}
