using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Collections.Specialized;
using DACrux.Base;

namespace DACrux.Data.Parser.Klarf
{
    public class ParserKlarf : ParserBase, ISaveFile
    {
        public static readonly string DATE_FORMAT = "MM-dd-yy HH:mm:ss";
        public static readonly string IMAGE_FILE_NAME = "TiffFileName";
        public static readonly string IMAGE_COUNT_COLUMN = "IMAGECOUNT";

        // 별도 지정한 ClassLookup 이 있는 경우 파일 저장 시 사용
        public static Dictionary<int, string> UserClassLookup;

        public static readonly string[] DEFECT_RECORD_SPEC = { "DEFECTID", "XREL", "YREL", "XINDEX", "YINDEX", "XSIZE", "YSIZE", "DEFECTAREA", "DSIZE", "CLASSNUMBER", "TEST", "IMAGECOUNT" };

        private ParserKlarf()
            : base()
        {
        }

        /// <summary>
        /// Klarf 파일을 이용한 인스턴스 생성
        /// </summary>
        public ParserKlarf(string fileName)
            : base(fileName)
        {
            try
            {
                Wafers = new WaferList();
                TiffFileNames = new List<string>();
                FullPathImageList = new List<string>();
                DataSourceFromFile = true;

                // 파일을 텍스트로 읽기
                string text = ParsingUtil.FileToString(fileName);

                NameValueCollection mainCol;
                List<NameValueCollection> waferColList;
                GetNameValueCollection(text, out mainCol, out waferColList);

                FileVersion = GetString(mainCol, "FileVersion");
                TiffSpec = GetString(mainCol, "TiffSpec");

                FileTimestamp = GetDateTime(GetString(mainCol, "FileTimestamp"));

                // 종류에 따라 메이커,모델,설비 값 순서가 다름
                List<string> list = GetStringList(mainCol["InspectionStationID"]);
                if (list != null && list.Count > 2)
                {
                    int i = 0;
                    if (InspectionStationOrder == InspectionStationValueOrder.MakerModelEquip)
                    {
                        Maker = list[i++];
                        Model = list[i++];
                        Equip = list[i];
                    }
                    else
                    {
                        Equip = list[i++];
                        Model = list[i++];
                        Maker = list[i];
                    }
                }

                SampleType = GetString(mainCol, "SampleType");
                ResultTimestamp = GetDateTime(GetString(mainCol, "ResultTimestamp"));
                LotID = GetString(mainCol, "LotID");
                SampleSize = First(GetIntWithLength(GetString(mainCol, "SampleSize")));
                DeviceID = GetString(mainCol, "DeviceID");

                list = GetStringList(mainCol["SetupID"]);
                if (list != null && list.Count > 0)
                {
                    SetupID = list.Count > 0 ? list[0] : null;
                    // SetupID는 Split하면 3개 배열로 표현됨. "ADPCA_7077MIMA" 11-07-18 03:01:12;
                    SetupTimestamp = list.Count == 3 ? GetDateTime(String.Format("{0} {1}", list[1], list[2])) : DateTime.MinValue;
                }

                StepID = GetString(mainCol, "StepID");
                ResultsID = GetString(mainCol, "ResultsID");
                SampleOrientationMarkType = GetString(mainCol, "SampleOrientationMarkType");
                OrientationMarkLocation = GetString(mainCol, "OrientationMarkLocation");

                list = GetStringList(mainCol["DiePitch"]);
                if (list != null && list.Count > 1)
                {
                    DiePitchX = GetDouble(list[0]);
                    DiePitchY = GetDouble(list[1]);
                }

                List<string> imageNameList = new List<string>();
                AppendImageList(imageNameList, mainCol.GetValues("TiffFileName"));


                InspectionTestList oTestList = null;
                Dictionary<int, string> classLookup = null;

                // Wafer 단위 처리
                foreach (NameValueCollection waferCol in waferColList)
                {
                    Wafer wafer = new Wafer();
                    wafer.Parser = this;

                    list = GetStringList(waferCol["DieOrigin"]);
                    if (list != null && list.Count > 1)
                    {
                        wafer.DieOriginX = GetDouble(list[0]);
                        wafer.DieOriginY = GetDouble(list[1]);
                    }

                    wafer.WaferID = GetString(waferCol, "WaferID");
                    wafer.Slot = GetInt(GetString(waferCol, "Slot"));

                    SetFormattedWaferID(wafer);

                    list = GetStringList(waferCol["SampleCenterLocation"]);
                    if (list != null && list.Count > 1)
                    {
                        wafer.SampleCenterLocationX = GetDouble(list[0]);
                        wafer.SampleCenterLocationY = GetDouble(list[1]);
                    }

                    wafer.ClassLookup = GetDictionaryWithLength(waferCol["ClassLookup"]);

                    if (wafer.ClassLookup == null)
                        wafer.ClassLookup = classLookup;
                    else
                        classLookup = wafer.ClassLookup;

                    // Summary 데이터 처리
                    List<string> summHeaders = GetStringWithLength(waferCol["SummarySpec"]);
                    string[] summArr = waferCol.GetValues("SummaryList");
                    SummaryList summList = new SummaryList();
                    summList.Add(summHeaders, SplitDouble(summArr));

                    // TestParametersSpec 처리
                    List<string> testParaHeaders = GetStringWithLength(waferCol["TestParametersSpec"], false);
                    string[] testParaArr = waferCol.GetValues("TestParametersList");
                    TestParameterSpecList testParaList = new TestParameterSpecList();
                    testParaList.Add(testParaHeaders, SplitDouble(testParaArr));

                    // Test 데이터 처리
                    string[] testNoArr = waferCol.GetValues("InspectionTest");

                    if (testNoArr != null && testNoArr.Length > 0)
                    {
                        for (int i = 0; i < testNoArr.Length; i++)
                        {
                            InspectionTest test = new InspectionTest();
                            test.TestNo = GetInt(GetIndexData(waferCol, "InspectionTest", i));
                            test.SampleTestPlan = ListToArray(GetPointWithLength(GetIndexData(waferCol, "SampleTestPlan", i)));

                            //SampleTestPlan 정보가 없을 경우 처음 Test 정보를 복사하여 넣는다.
                            //Ex) 한 File 안에 SampleTestPlan 는 하나인데 Wafer 의 Defect 정보가 여러개의 경우
                            if (test.SampleTestPlan == null && oTestList != null)
                                test.SampleTestPlan = oTestList[0].SampleTestPlan.Clone() as Point[];

                            test.AreaPerTest = GetDouble(GetIndexData(waferCol, "AreaPerTest", i));
                            test.Summary = summList.GetSummary(test.TestNo);
                            test.TestParameterSpec = testParaList.GetParaSpec(i);

                            wafer.TestList.Add(test);
                        }

                        if (oTestList == null)
                        {
                            oTestList = wafer.TestList;

                            if (oTestList.Count == 0)
                                throw new Exception("InspectionTestList Data Count Zero");
                        }
                    }
                    else if (oTestList != null)
                    {
                        wafer.CopyTestList(oTestList);
                    }

                    // Defect 처리
                    wafer.DefectHeaders = GetStringWithLength(waferCol["DefectRecordSpec"]);
                    string[] defectArr = waferCol.GetValues("DefectList");

                    if (defectArr != null && defectArr.Length > 0)
                    {
                        wafer.DefectList = DefectDataHelper.GetDefectList(wafer.DefectHeaders, defectArr);

                        foreach (Defect d in wafer.DefectList)
                        {
                            if (d.IMAGECOUNT > 0)
                            {
                                wafer.ImageDefectList.Add(d);
                            }

                            if (d.CLASSNUMBER != 0)
                            {
                                wafer.ClassifedDefectList.Add(d);
                            }

                            d.X = (double)(d.XINDEX * DiePitchX + d.XREL) - wafer.SampleCenterLocationX;
                            d.Y = (double)(d.YINDEX * DiePitchY + d.YREL) - wafer.SampleCenterLocationY;
                        }
                    }

                    AppendImageList(imageNameList, waferCol.GetValues("TiffFileName"));

                    Wafers.Add(wafer);
                }

                int allWaferImageDefectCount = 0;

                foreach (Wafer wafer in Wafers)
                {
                    foreach (Defect defect in wafer.ImageDefectList)
                        allWaferImageDefectCount += defect.IMAGECOUNT;
                }

                foreach (Wafer wafer in Wafers)
                {
                    // Die 기준 좌표를 좌하단으로 변경
                    wafer.UpdateDieIndex(DieIndexSort.CenterToLowerLeft);
                }

                // 이미지 매핑
                if (imageNameList.Count > 0)
                {
                    IsTiffImages = true;

                    // imageList 카운트가 Wafer 카운트와 같고, 확장자가 tif 인 경우 별도 처리 (IMAGESEQ 에 INDEX 연결되어 있음)
                    IsTiffImages = IsTiffImages && imageNameList.Count == Wafers.Count;

                    foreach (string imageName in imageNameList)
                        IsTiffImages = IsTiffImages && Path.GetExtension(imageNameList[0]).ToLower().Contains(".tif");

                    // TIF 이미지 true 이면서 모든 Wafer의 ImageDefectList 갯수의 합이 tif Image 개수와 같은 경우 일반 이미지로 처리 되도록 한다.
                    if (IsTiffImages && allWaferImageDefectCount == imageNameList.Count)
                        IsTiffImages = false;

                    int n = 0;

                    foreach (Wafer wf in Wafers)
                    {
                        ImageLoader tifLoader = null;

                        if (IsTiffImages)
                        {
                            string tiffImageFileName = Path.Combine(DirectoryName, imageNameList[Wafers.IndexOf(wf)]);

                            if (!File.Exists(tiffImageFileName))
                            {
                                ErrorFlag = true;
                                ErrorMessage = String.Format("'{0}' 파일을 찾을 수 없습니다.", Path.GetFileName(tiffImageFileName));
                                return;
                            }

                            tifLoader = new ImageLoader(tiffImageFileName);
                            TiffFileNames.Add(tiffImageFileName);
                        }

                        foreach (Defect d in wf.ImageDefectList)
                        {
                            for (int i = 0; i < d.IMAGECOUNT; i++)
                            {
                                // 일반적인 경우 순서대로 매핑
                                if (!IsTiffImages)
                                {
                                    if (imageNameList.Count > n)
                                        d.Images[i] = imageNameList[n++];
                                }
                                else // TIF 인 경우 IMAGESEQ에 맞춰 매핑
                                {
                                    // JPG로 추출
                                    int imageIndex = Math.Max(0, d.Images.GetDefectImage(i).IMAGESEQ - 1);
                                    string fullImageName = tifLoader.SaveImage(imageIndex);
                                    d.Images[i] = Path.GetFileName(fullImageName);
                                }
                            }
                        }

                        if (tifLoader != null)
                            tifLoader.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorFlag = true;
                ErrorMessage = ex.Message + Environment.NewLine + ex.StackTrace;
            }
        }

        /// <summary>
        /// DB 데이터를 이용한 인스턴스 생성
        /// </summary>
        public ParserKlarf(DACrux.Base.Defect[] defectArr, DataSet ds)
        {
            List<ParserKlarf> list = new List<ParserKlarf>();

            if (defectArr == null || defectArr.Length == 0 || ds == null || ds.Tables.Count == 0)
                return;

            DataTable mapDt = ds.Tables["MAP"];
            DataTable classDt = ds.Tables["CLASS"];
            DataTable dieDt = ds.Tables["DIE"];

            if (mapDt.Rows.Count == 0)
                return;

            DataRow mapRow = mapDt.Rows[0];

            FileVersion = "1 2";
            FileTimestamp = DateTime.Now;
            Maker = mapRow["MAKER"].ToString();
            Model = mapRow["MODEL"].ToString();
            Equip = mapRow["EQUIP"].ToString();
            SampleType = "WAFER";
            ResultTimestamp = GetDateTime(mapRow["RESULTTIMESTAMP"].ToString());
            LotID = mapRow["LOT_ID"].ToString();
            SampleSize = 200;
            DeviceID = mapRow["PRODUCT"].ToString();
            SetupID = mapRow["SETUP_ID"].ToString();
            SetupTimestamp = GetDateTime(mapRow["SETUP_TIME"].ToString());
            StepID = mapRow["STEP_ID"].ToString();
            SampleOrientationMarkType = "NOTCH";
            OrientationMarkLocation = "DOWN";
            DiePitchX = GetDouble(mapRow["DIE_PITCH_X"].ToString());
            DiePitchY = GetDouble(mapRow["DIE_PITCH_Y"].ToString());

            Wafer wafer = new Wafer();
            Wafers = new WaferList();
            Wafers.Add(wafer);

            wafer.Parser = this;
            wafer.DieOriginX = GetDouble(mapRow["DIE_ORIGIN_X"].ToString());
            wafer.DieOriginY = GetDouble(mapRow["DIE_ORIGIN_Y"].ToString());
            wafer.WaferID = mapRow["WAFER_ID"].ToString();
            wafer.Slot = GetInt(mapRow["SLOT"].ToString());
            wafer.SampleCenterLocationX = GetDouble(mapRow["ORIGIN_X"].ToString());
            wafer.SampleCenterLocationY = GetDouble(mapRow["ORIGIN_Y"].ToString());

            // ClassLookup
            if (classDt != null && classDt.Rows.Count > 0)
            {
                wafer.ClassLookup = new Dictionary<int, string>();

                foreach (DataRow row in classDt.Rows)
                    wafer.ClassLookup.Add(GetInt(row["CLASSNUMBER"].ToString()), row["NAME"].ToString());
            }

            InspectionTest test = new InspectionTest();
            wafer.TestList.Add(test);
            test.TestNo = GetInt(mapRow["TEST"].ToString());

            // SampleTestPlan
            if (dieDt != null && dieDt.Rows.Count > 0)
            {
                test.SampleTestPlan = new Point[dieDt.Rows.Count];

                for (int i = 0; i < dieDt.Rows.Count; i++)
                    test.SampleTestPlan[i] = new Point(GetInt(dieDt.Rows[i]["X"].ToString()), GetInt(dieDt.Rows[i]["Y"].ToString()));
            }

            test.AreaPerTest = GetDouble(mapRow["SCAN_AREA"].ToString());

            // DefectList
            foreach (DACrux.Base.Defect d in defectArr)
            {
                Defect defect = new Defect();
                defect.DEFECTID = d.DEFECTID;
                defect.XREL = d.XREL;
                defect.YREL = d.YREL;
                defect.XINDEX = d.XINDEX;
                defect.YINDEX = d.YINDEX;
                defect.XSIZE = d.XSIZE;
                defect.YSIZE = d.YSIZE;
                defect.DEFECTAREA = d.DEFECTAREA;
                defect.DSIZE = d.DSIZE;
                defect.CLASSNUMBER = d.CLASSNUMBER;
                defect.TEST = d.TEST;
                defect.IMAGECOUNT = d.IMAGECOUNT;
                defect.CLUSTERNUMBER = d.CLUSTERNUMBER;
                defect.ADDER = d.ADDER;

                wafer.DefectList.Add(defect);

                if (defect.IMAGECOUNT > 0)
                    wafer.ImageDefectList.Add(defect);
            }
        }

        /// <summary>
        /// DmsCache 데이터를 이용한 인스턴스 생성
        /// </summary>
        public ParserKlarf(Defect[] defectArr, DmsWaferDieInfo info, Dictionary<int, string> classLookupDic)
        {
            DmsStepInfo stepInfo = info.StepInfo;

            FileVersion = "1 2";
            FileTimestamp = DateTime.Now;
            Maker = stepInfo.Maker;
            Model = stepInfo.Model;
            Equip = stepInfo.Equip;
            SampleType = "WAFER";
            ResultTimestamp = GetDateTime(stepInfo.ResultTimestamp);
            LotID = stepInfo.LotID;
            SampleSize = stepInfo.SampleSize;
            DeviceID = stepInfo.DeviceID;
            SetupID = stepInfo.SetupID;
            SetupTimestamp = GetDateTime(stepInfo.SetupTimestamp);
            StepID = stepInfo.StepID;
            SampleOrientationMarkType = "NOTCH";
            OrientationMarkLocation = "DOWN";
            DiePitchX = stepInfo.DiePitchX;
            DiePitchY = stepInfo.DiePitchY;

            Wafer wafer = new Wafer();
            Wafers = new WaferList();
            Wafers.Add(wafer);

            wafer.Parser = this;
            wafer.DieOriginX = stepInfo.DieOriginX;
            wafer.DieOriginY = stepInfo.DieOriginY;
            wafer.WaferID = stepInfo.WaferID;
            wafer.Slot = stepInfo.Slot;
            wafer.SampleCenterLocationX = stepInfo.SampleCenterLocationX;
            wafer.SampleCenterLocationY = stepInfo.SampleCenterLocationY;
            wafer.ClassLookup = classLookupDic;

            SetDefect(defectArr);

            InspectionTest test = new InspectionTest();
            wafer.TestList.Add(test);
            test.TestNo = 1;

            test.SampleTestPlan = info.Dies;
            test.AreaPerTest = stepInfo.AreaPerTest;
        }

        /// <summary>
        /// {LOT ID}-{NO} 형태로 표현되는 WAFER ID를 가져옵니다.
        /// </summary>
        private void SetFormattedWaferID(Wafer wafer)
        {
            if (String.IsNullOrEmpty(wafer.WaferID))
                wafer.WaferID = String.Format("{0:00}", wafer.Slot);

            wafer.WaferID = wafer.WaferID.Trim().Trim('@');

            if (wafer.WaferID.Contains(LotID) && wafer.WaferID.Length >= (LotID.Length + 3))
                return;

            if (wafer.WaferID.Length == 2)
                wafer.WaferID = String.Format("{0}-{1}", LotID, wafer.WaferID);
            else
                wafer.WaferID = String.Format("{0}-{1}", LotID, wafer.Slot);
        }

        public override DateTime GetBackupDateTime()
        {
            return ResultTimestamp;
        }

        public void SetDefect(Defect[] defectArr)
        {
            if (defectArr == null)
                return;

            if (Wafers.Count == 0)
                throw new Exception("설정된 Wafer 정보가 없습니다.");

            Wafers[0].DefectList.Clear();

            // DefectList
            foreach (DACrux.Base.Defect d in defectArr)
            {
                Defect defect = new Defect();
                defect.DEFECTID = d.DEFECTID;
                defect.XREL = d.XREL;
                defect.YREL = d.YREL;
                defect.XINDEX = d.XINDEX;
                defect.YINDEX = d.YINDEX;
                defect.XSIZE = d.XSIZE;
                defect.YSIZE = d.YSIZE;
                defect.DEFECTAREA = d.DEFECTAREA;
                defect.DSIZE = d.DSIZE;
                defect.CLASSNUMBER = d.CLASSNUMBER;
                defect.TEST = d.TEST;
                defect.IMAGECOUNT = d.IMAGECOUNT;

                Wafers[0].DefectList.Add(defect);

                if (defect.IMAGECOUNT > 0)
                    Wafers[0].ImageDefectList.Add(defect);
            }
        }

        /// <summary>
        /// Review 설비에서 사용 가능하도록 파일로 저장합니다.
        /// </summary>
        /// <param name="orginDieToZero">Origin Die의 인덱스를 0으로 할지를 나타냅니다.</param>
        public void SaveFile(string fileName)
        {
            SaveFile(fileName, true);
        }

        /// <summary>
        /// Review 설비에서 사용 가능하도록 파일로 저장합니다.
        /// </summary>
        /// <param name="orginDieToZero">Origin Die의 인덱스를 0으로 할지를 나타냅니다.</param>
        public void SaveFile(string fileName, bool originDieToZero)
        {
            if (Wafers.Count == 0)
                throw new Exception("Klarf export error. Not found Wafer data.");

            SaveFile(fileName, originDieToZero, Wafers[0]);
        }

        /// <summary>
        /// Review 설비에서 사용 가능하도록 파일로 저장합니다.
        /// </summary>
        /// <param name="orginDieToZero">Origin Die의 인덱스를 0으로 할지를 나타냅니다.</param>
        private void SaveFile(string fileName, bool originDieToZero, Wafer wafer)
        {
            // 데이터소스가 파일인 경우 Origin Die가 이미 0 이다.
            if (DataSourceFromFile)
                originDieToZero = true;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("FileVersion {0};", FileVersion));
            sb.AppendLine(String.Format("FileTimestamp {0};", FileTimestamp.ToString(DATE_FORMAT)));
            sb.AppendLine(String.Format("InspectionStationID \"{0}\" \"{1}\" \"{2}\";", Maker, Model, Equip));
            sb.AppendLine(String.Format("SampleType {0};", SampleType));
            sb.AppendLine(String.Format("ResultTimestamp {0};", ResultTimestamp.ToString(DATE_FORMAT)));
            sb.AppendLine(String.Format("LotID \"{0}\";", LotID));
            sb.AppendLine(String.Format("SampleSize 1 {0};", 200));// SampleSize));
            sb.AppendLine(String.Format("DeviceID \"{0}\";", DeviceID));
            sb.AppendLine(String.Format("SetupID \"{0}\" {1};", SetupID, SetupTimestamp.ToString(DATE_FORMAT)));
            sb.AppendLine(String.Format("StepID \"{0}\";", StepID));
            sb.AppendLine(String.Format("SampleOrientationMarkType {0};", SampleOrientationMarkType));
            sb.AppendLine(String.Format("OrientationMarkLocation {0};", OrientationMarkLocation));
            sb.AppendLine(String.Format("DiePitch {0:e10} {1:e10};", DiePitchX, DiePitchY));

            double dieOriginX = originDieToZero ? 0 : wafer.DieOriginX;
            double dieOriginY = originDieToZero ? 0 : wafer.DieOriginY;

            sb.AppendLine(String.Format("DieOrigin {0:e10} {1:e10};", dieOriginX, dieOriginY));
            sb.AppendLine(String.Format("WaferID \"{0}\";", wafer.WaferID));
            sb.AppendLine(String.Format("Slot {0};", wafer.Slot));
            sb.AppendLine(String.Format("SampleCenterLocation {0:e10} {1:e10};", wafer.SampleCenterLocationX, wafer.SampleCenterLocationY));

            // 별도 지정한 ClassLookup 이 있는 경우 파일 저장 시 사용
            if (UserClassLookup != null && UserClassLookup.Count > 1)
                wafer.ClassLookup = UserClassLookup;

            // ClassLookup
            sb.AppendLine(GetClassLookupString(wafer.ClassLookup));

            if (wafer.TestList.Count == 0)
                throw new Exception("Klarf export error. Not found Inspection Test info.");

            InspectionTest test = wafer.TestList[0];

            sb.AppendLine(String.Format("InspectionTest {0};", test.TestNo));

            // SampleTestPlan
            if (test.SampleTestPlan != null && test.SampleTestPlan.Length > 0)
            {
                sb.AppendFormat("SampleTestPlan {0}", test.SampleTestPlan.Length);

                foreach (Point pt in test.SampleTestPlan)
                {
                    int x = originDieToZero ? pt.X - (int)wafer.DieOriginX : pt.X;
                    int y = originDieToZero ? pt.Y - (int)wafer.DieOriginY : pt.Y;
                    sb.AppendFormat("{0}  {1} {2}", Environment.NewLine, x, y);
                }

                sb.AppendLine(";");
            }

            sb.AppendLine(String.Format("AreaPerTest {0:e10};", test.AreaPerTest));

            if (wafer.DefectList != null && wafer.DefectList.Count > 0)
            {
                // DefectRecordSpec
                string[] defectHeader = (wafer.DefectHeaders != null && wafer.DefectHeaders.Count > 0) ? wafer.DefectHeaders.ToArray() : DEFECT_RECORD_SPEC;

                sb.AppendFormat("DefectRecordSpec {0}", defectHeader.Length);

                foreach (String header in defectHeader)
                    sb.AppendFormat(" {0}", header);

                sb.AppendLine(";");

                sb.AppendLine("DefectList");

                // DefectList
                for (int i = 0; i < wafer.DefectList.Count; i++)
                {
                    Defect defect = wafer.DefectList[i];
                    Type t = typeof(Defect);

                    foreach (string name in defectHeader)
                    {
                        FieldInfo fi = t.GetField(name);

                        object val = null;

                        if (fi != null)
                            val = fi.GetValue(defect);

                        // origin die index가 0이 되도록 인덱스 값 조정
                        if (originDieToZero)
                        {
                            if (name == "XINDEX")
                                val = (int)(Int32.Parse(val.ToString()) - wafer.DieOriginX);

                            if (name == "YINDEX")
                                val = (int)(Int32.Parse(val.ToString()) - wafer.DieOriginY);
                        }

                        sb.AppendFormat(" {0}", val);
                    }

                    if (i < wafer.DefectList.Count - 1)
                        sb.AppendLine();
                    else
                        sb.AppendLine(";");
                }
            }

            sb.AppendLine("EndOfFile;");

            File.WriteAllText(fileName, sb.ToString());
        }

        private static string GetClassLookupString(Dictionary<int, string> dic)
        {
            StringBuilder sb = new StringBuilder();

            // ClassLookup
            if (dic != null && dic.Count > 0)
            {
                sb.Append(String.Format("ClassLookup {0}", dic.Count));

                foreach (var item in dic)
                    sb.AppendFormat("{0}  {1} \"{2}\"", Environment.NewLine, item.Key, item.Value);

                sb.Append(";");
            }

            return sb.ToString();
        }

        /// <summary>
        /// WAFER 단위로 분리하여 파일을 저장합니다.
        /// </summary>
        public string[] SaveFile_SplitByWafer(string savePath, bool originDieToZero, FileNameRule fileNameRule)
        {
            if (Wafers.Count == 0)
                throw new Exception("Klarf export error. Not found Wafer data.");

            string[] arr = new string[Wafers.Count];

            for (int i = 0; i < Wafers.Count; i++)
            {
                string fileName;
                Wafer wafer = Wafers[i];

                if (fileNameRule == FileNameRule.Lot_Slot_Layer_000)
                    fileName = String.Format("{0}_{1:00}_{2}{3}", LotID, wafer.Slot, StepID, ".000");
                else
                    throw new Exception("FileNameRule에 대한 파일명 규칙이 정의되지 않았습니다");

                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);

                string fullPath = Path.Combine(savePath, RemoveInvalidCharFromFileName(fileName));

                SaveFile(fullPath, originDieToZero, wafer);
                arr[i] = fullPath;
            }

            return arr;
        }

        private string RemoveInvalidCharFromFileName(string fileName)
        {
            foreach (char ch in Path.GetInvalidFileNameChars())
            {
                if (fileName.IndexOf(ch) >= 0)
                    fileName = fileName.Replace(ch, '_');
            }

            return fileName;
        }

        public int GetDefectCount()
        {
            int count = 0;

            foreach (Wafer wafer in Wafers)
            {
                count += wafer.DefectList.Count;
            }

            return count;
        }

        /// <summary>
        /// 이미지 파일명 배열을 가져옵니다.
        /// </summary>
        public string[] GetImageFileNames()
        {
            List<string> list = new List<string>();

            // Wafer 단위 처리
            foreach (var wafer in Wafers)
            {
                foreach (var defect in wafer.DefectList)
                {
                    if (defect.IMAGECOUNT > 0 && defect.Images != null)
                    {
                        foreach (DefectImage image in defect.Images)
                        {
                            list.Add(image.IMAGEPATH);
                        }
                    }
                }
            }

            return list.ToArray();
        }

        private object GetPropertyData(object instance, string fieldName)
        {
            return instance.GetType().GetProperty(fieldName).GetValue(instance, null);
        }

        private void AppendImageList(List<string> imageList, string[] values)
        {
            if (values == null || values.Length == 0)
                return;

            imageList.AddRange(values);
        }

        private List<double> SplitDouble(string[] arr)
        {
            if (arr == null || arr.Length == 0)
                return null;

            List<double> list = new List<double>();

            foreach (string d in arr)
                AppendDefectData(list, d);

            return list;
        }

        /// <summary>
        /// 데이터를 NameValueCollection 형태로 가져옵니다.
        /// 공통영역은 MainDic, Wafer단위 반복 영역은 List형태로, Test단위 반복 영역은 List형태로
        /// 
        /// main
        /// - Wafer
        /// - - Test
        /// - - Test
        /// - Wafer
        /// - - Test
        /// - - Test
        /// </summary>
        private static void GetNameValueCollection(string fileText, out NameValueCollection mainCol, out List<NameValueCollection> waferColList)
        {
            mainCol = new NameValueCollection();
            waferColList = new List<NameValueCollection>();

            if (String.IsNullOrEmpty(fileText))
                return;

            string[] arr = fileText.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            NameValueCollection curr = mainCol;

            foreach (string text in arr)
            {
                string val = text.TrimStart();
                int index = val.IndexOf(' ');
                int index2 = val.IndexOf(Environment.NewLine);

                if (index < 0)
                    index = val.IndexOf('\t');

                if (index > -1 && index2 > -1 && index > index2)
                    index = index2;

                if (index > 0)
                {
                    string key = val.Substring(0, index).Trim();
                    string value = val.Substring(index + 1).Trim();

                    // DieOrigin을 만나면 Wafer 단위로 저장한다.
                    if (key == "DieOrigin")
                    {
                        curr = new NameValueCollection();
                        waferColList.Add(curr);
                    }

                    curr.Add(key, value);
                }
            }
        }

        private void AppendDefectData(List<double> valueList, string data)
        {
            if (String.IsNullOrEmpty(data))
                return;

            List<string> list = new List<string>();

            string[] lines = data.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            // 아래의 1	0 데이터는 필요없으므로 버린다.
            // Ex)
            // 14 15888 225 0 27 10.00 10.00 100.00 10.00 78 0 1 0 1 1
            // 1	0
            foreach (string line in lines)
            {
                if (line.Trim().Length < 10)
                    continue;

                string[] arr = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string val in arr)
                {
                    double d;

                    if (!Double.TryParse(val, out d))
                        throw new Exception(String.Format("Parsing Error: '{0}'", val));

                    valueList.Add(d);
                }
            }
        }

        private string GetIndexData(System.Collections.Specialized.NameValueCollection col, string name, int index)
        {
            string[] arr = col.GetValues(name);

            if (arr == null || arr.Length <= index)
                return null;

            return arr[index];
        }

        public string FileVersion { get; private set; }
        public DateTime FileTimestamp { get; private set; }
        public string TiffSpec { get; private set; }
        public string Equip { get; private set; }
        public string Maker { get; private set; }
        public string Model { get; private set; }
        public InspectionStationValueOrder InspectionStationOrder { get; private set; }
        public string SampleType { get; private set; }
        public DateTime ResultTimestamp { get; private set; }
        public int SampleSize { get; private set; }
        public string DeviceID { get; private set; }
        public string SetupID { get; private set; }
        public DateTime SetupTimestamp { get; private set; }
        public string ResultsID { get; private set; }
        public string StepID { get; private set; }
        public string SampleOrientationMarkType { get; private set; }
        public string OrientationMarkLocation { get; private set; }
        public double DiePitchX { get; private set; }
        public double DiePitchY { get; private set; }
        public bool IsTiffImages { get; private set; }
        public List<string> TiffFileNames { get; private set; }
        public List<string> FullPathImageList { get; private set; }
        private bool DataSourceFromFile { get; set; }

        public WaferList Wafers { get; private set; }

        public int GetAngle()
        {
            if (SampleOrientationMarkType == "DOWN")
                return 0;
            else if (SampleOrientationMarkType == "UP")
                return 180;
            else if (SampleOrientationMarkType == "LEFT")
                return 90;
            else if (SampleOrientationMarkType == "RIGHT")
                return 270;
            else
                return 0;
        }

        /// <summary>
        /// 데이터를 회전 합니다.
        /// </summary>
        public void Rotate(int rotateAngle)
        {
            // 회전대상 데이터는 SampleTestPlan, Defect데이터의 XINDEX, YINDEX, XREL, YREL

            if (rotateAngle == 0)
                return;

            foreach (Wafer wafer in Wafers)
            {
                foreach (InspectionTest test in wafer.TestList)
                {
                    int xmin, xmax, ymin, ymax;
                    test.GetMinMaxDiePoint(out xmin, out xmax, out ymin, out ymax);

                    // SampleTestPlan 변환
                    for (int i = 0; i < test.SampleTestPlan.Length; i++)
                    {
                        int x = test.SampleTestPlan[i].X;
                        int y = test.SampleTestPlan[i].Y;
                        Rotate(rotateAngle, xmin, xmax, ymin, ymax, ref x, ref y);
                        test.SampleTestPlan[i].X = x;
                        test.SampleTestPlan[i].Y = y;
                    }

                    // Defect 데이터를 Test 별로 가져온다.
                    foreach (Defect defect in wafer.DefectList.FindByTest(test.TestNo))
                    {
                        Rotate(rotateAngle, xmin, xmax, ymin, ymax, ref defect.XINDEX, ref defect.YINDEX);
                        RotateDefectRel(rotateAngle, ref defect.XREL, ref defect.YREL);
                    }
                }
            }
        }

        /// <summary>
        /// Defect의 XREL, YREL 값을 회전값에 맞춰 변환합니다.
        /// </summary>
        private void RotateDefectRel(int angle, ref double xrel, ref double yrel)
        {
            double nx = xrel;
            double ny = yrel;

            if (angle == 180)
            {
                nx = DiePitchX - xrel;
                ny = DiePitchY - yrel;
            }
            else if (angle == 90)
            {
                nx = yrel;
                ny = DiePitchX - xrel;
            }
            else if (angle == 90)
            {
                nx = DiePitchY - yrel;
                ny = xrel;
            }

            xrel = Math.Round(nx, 3);
            yrel = Math.Round(ny, 3);
        }

        /// <summary>
        /// ClassLookup 정보를 추가합니다.
        /// </summary>
        /// <param name="p"></param>
        public static void AppendClassLookupInfo(string fileName)
        {
            string search1 = "ClassLookup";
            string search2 = ";";

            string text = ParsingUtil.FileToString(fileName);
            int idx1 = text.IndexOf(search1);

            string newText = null;

            if (idx1 > 0)
            {
                int idx2 = text.IndexOf(search2, idx1);

                if (idx2 < 0)
                    return;

                newText = text.Substring(0, idx1) + GetClassLookupString(ParserKlarf.UserClassLookup) + text.Substring(idx2 + search2.Length);
            }
            else // Klarf 파일에 문제가 있어 ClassLookup 을 못찾는 경우 SampleCenterLocation을 찾아 뒤에 붙여준다.
            {
                string searchEx = "SampleCenterLocation";
                idx1 = text.IndexOf(searchEx);

                if (idx1 < 0)
                    return;

                int idx2 = text.IndexOf(search2, idx1);

                if (idx2 < 0)
                    return;

                newText = text.Substring(0, idx2 + search2.Length) + Environment.NewLine + GetClassLookupString(ParserKlarf.UserClassLookup) + text.Substring(idx2 + search2.Length);
            }

            if (!String.IsNullOrEmpty(newText))
                File.WriteAllText(fileName, newText);
        }
    }

    /// <summary>
    /// InspectionStationID 데이터의 순서를 나타냅니다.
    /// </summary>
    public enum InspectionStationValueOrder
    {
        MakerModelEquip,
        EquipModelMaker
    }

    public enum DataType
    {
        String,
        Int,
        Double,
        Date,
        Array,
        Type,
        IntArrayWithLength,
        StringAndDate,
        StringArrayWithLength,
        XYValue,
        XYValueArrayWithLength,
        ListData,
        IntStringArrayWithLength
    }

    public enum FileNameRule
    {
        Lot_Slot_Layer_000
    }

    #region Wafer 데이터 관련

    public class WaferList : List<Wafer>
    {
        public new void Add(Wafer wafer)
        {
            base.Add(wafer);
            Sort();
        }

        public override string ToString()
        {
            if (Count == 0)
                return "(empty)";

            StringBuilder sb = new StringBuilder();
            sb.Append("(");

            for (int i = 0; i < Count; i++)
            {
                sb.Append(this[i].WaferID);

                if (i < Count - 1)
                    sb.Append(",");
            }

            sb.Append(")");

            return sb.ToString();
        }
    }

    public class Wafer : IComparable<Wafer>
    {
        public Wafer()
        {
            DefectList = new DefectList();
            ClassifedDefectList = new DefectList();
            ImageDefectList = new DefectList();
            TestList = new InspectionTestList();
            //SummaryList = new SummaryList();
        }

        public void UpdateDieIndex(DieIndexSort indexSort)
        {
            int addX = Int32.MaxValue;
            int addY = Int32.MaxValue;

            // 좌표 조정 (Klarf 파일은 Wafer의 가운데 Die가 (0,0)이며, WaferDieCalculator 클래스를 이용하여 
            // 좌하단의 첫번째 Die가 (1,1)이 되도록 좌표를 업데이트 한다) 2019.10.05 Taihi,Kim.
            if (indexSort == DieIndexSort.CenterToLowerLeft)
            {
                //WaferDieCalculator
                foreach (InspectionTest test in TestList)
                {
                    if (test.SampleTestPlan == null)
                        return;

                    foreach (Point pt in test.SampleTestPlan)
                    {
                        addX = Math.Min(addX, pt.X);
                        addY = Math.Min(addY, pt.Y);
                    }
                }

                addX = (int)DieOriginX - addX + 1;
                addY = (int)DieOriginY - addY + 1;
            }
            else // 좌표 조정 (Wafer 좌하단이 (1,1)이며, 가운데가 (0,0)이 되도록 변경한다.)
            {
                addX = -(int)DieOriginX;
                addY = -(int)DieOriginY;
            }

            // Die Origin 업데이트
            DieOriginX += addX;
            DieOriginY += addY;

            if (addX != 0 || addY != 0)
            {
                // Die 정보 업데이트
                foreach (InspectionTest test in TestList)
                {
                    for (int i = 0; i < test.SampleTestPlan.Length; i++)
                    {
                        test.SampleTestPlan[i].X += addX;
                        test.SampleTestPlan[i].Y += addY;
                    }
                }

                // Defect 좌표 업데이트
                foreach (Defect d in DefectList)
                {
                    d.XINDEX += addX;
                    d.YINDEX += addY;
                }
            }
        }

        public void CopyTestList(InspectionTestList otestList)
        {
            TestList = otestList.Clone() as InspectionTestList;
        }

        public double DieOriginX { get; internal set; }
        public double DieOriginY { get; internal set; }
        public string WaferID { get; internal set; }
        public int Slot { get; internal set; }
        public double SampleCenterLocationX { get; internal set; }
        public double SampleCenterLocationY { get; internal set; }
        public Dictionary<int, string> ClassLookup { get; internal set; }
        public DefectList DefectList { get; internal set; }
        public DefectList ClassifedDefectList { get; internal set; }
        public DefectList ImageDefectList { get; internal set; }
        public InspectionTestList TestList { get; private set; }
        public ParserKlarf Parser { get; internal set; }
        public List<string> DefectHeaders { get; internal set; }

        public override string ToString()
        {
            return WaferID;
        }

        public int CompareTo(Wafer wafer)
        {
            return WaferID.CompareTo(wafer.WaferID);
        }
    }




    public enum DieIndexSort
    {
        CenterToLowerLeft,
        LowerLeftToCenter
    }

    #endregion

    #region Summary 데이터 관련

    public class SummaryList : List<Summary>
    {
        public void Add(List<string> headers, List<double> dataArr)
        {
            if (headers == null || headers.Count == 0 || dataArr == null || dataArr.Count == 0)
                return;

            int length = dataArr.Count / headers.Count;

            for (int i = 0; i < length; i++)
            {
                Add(new Summary(headers, dataArr.GetRange(i * headers.Count, headers.Count)));
            }
        }

        public Summary GetSummary(int testNo)
        {
            foreach (Summary summ in this)
            {
                if (summ.TESTNO == testNo)
                    return summ;
            }

            return null;
        }
    }

    public class Summary : ICloneable
    {
        private Summary()
        {
        }

        public Summary(List<string> headers, List<double> data)
        {
            ParsingUtil.SetPropertyData(this, headers, data);
        }

        public object Clone()
        {
            Summary obj = new Summary();
            obj.TESTNO = TESTNO;
            obj.NDEFECT = NDEFECT;
            obj.DEFDENSITY = DEFDENSITY;
            obj.NDIE = NDIE;
            obj.NDEFDIE = NDEFDIE;
            return obj;
        }

        public int TESTNO { get; internal set; }
        public int NDEFECT { get; internal set; }
        public double DEFDENSITY { get; internal set; }
        public int NDIE { get; internal set; }
        public int NDEFDIE { get; internal set; }
    }

    #endregion

    #region Defect 데이터 관련

    public static class DefectDataHelper
    {
        /// <summary>
        /// 줄바꿈 문자 단위로 Row를 나누고, 헤더 갯수를 넘어가는 데이터는 저장하지 않는다.
        /// </summary>
        public static DefectList GetDefectList(List<string> headers, string[] textList)
        {
            DefectList list = new DefectList();

            if (headers == null || headers.Count == 0 || textList == null || textList.Length == 0)
                return list;

            List<string> rowList = new List<string>();

            foreach (string text in textList)
            {
                rowList.AddRange(text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            }

            FieldInfo[] fieldArr = new FieldInfo[headers.Count];

            for (int i = 0; i < fieldArr.Length; i++)
                fieldArr[i] = typeof(Defect).GetField(headers[i]);

            foreach (string row in rowList)
            {
                Defect defect = new Defect();

                double[] rowDataArr = ToDoubleList(row.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries));

                if (fieldArr.Length <= rowDataArr.Length)
                {
                    for (int i = 0; i < fieldArr.Length; i++)
                    {
                        if (fieldArr[i] != null)
                            fieldArr[i].SetValue(defect, System.Convert.ChangeType(rowDataArr[i], fieldArr[i].FieldType));
                    }

                    list.Add(defect);

                    // 헤더 갯수보다 데이터 갯수가 많은 경우는 Review 데이터에 이미지 정보가 붙어오는 경우이다. 2019.09.08
                    // 이미지 1개 당 2개의 데이터가 붙어오는것으로 확인됨
                    if (fieldArr.Length < rowDataArr.Length)
                    {
                        for (int i = fieldArr.Length; i < rowDataArr.Length; i += 2)
                        {
                            defect.Images.Add((int)rowDataArr[i]);
                        }
                    }
                }
                // 헤더 갯수보다 데이터 갯수가 적은 경우
                else if (rowDataArr.Length > 0 && list.Count > 0)
                {
                    /* Defect 이미지가 있을 경우 그 다음 줄 첫째값이 이미지 인덱스 값이다. 따라서 짧은 데이터가 오는 경우 윗쪽 데이터의 imageseq 에 연결
                       이미지가 여러개 오는 경우 여러줄이 생길 수 있다. 2019.09.03 Taihi,Kim.
                     * 
                       예제) 헤더는 DEFECTID XREL YREL XINDEX YINDEX XSIZE YSIZE DEFECTAREA DSIZE CLASSNUMBER TEST IMAGECOUNT IMAGELIST;
                     * 
                       22 13437.400 422.900 2 9 16.000 2.700 43.200001 107.000 0 1 1 1
	                   10 0
                     * 
                       22 13437.400 422.900 2 9 16.000 2.700 43.200001 107.000 0 1 3 3
	                   10 0
                        9 0
                        8 0
                     */
                    Defect prevDefect = list[list.Count - 1];

                    // 이미지 1개 당 2개의 데이터가 붙어오는것으로 확인됨
                    for (int i = 0; i < rowDataArr.Length; i += 2)
                    {
                        prevDefect.Images.Add((int)rowDataArr[i]);
                    }
                }

            }

            return list;
        }

        private static double[] ToDoubleList(string[] arr)
        {
            if (arr == null || arr.Length == 0)
                return null;

            double[] doubleArr = new double[arr.Length];

            for (int i = 0; i < arr.Length; i++)
                doubleArr[i] = DACrux.Base.Convert.doubleParse(arr[i]);

            return doubleArr;
        }

        public static DefectList GetDefectList(List<string> headers, List<double> dataArr)
        {
            if (headers == null || headers.Count == 0 || dataArr == null || dataArr.Count == 0)
                return null;

            DefectList list = new DefectList();

            int length = dataArr.Count / headers.Count;
            Type t = typeof(Defect);

            for (int i = 0; i < length; i++)
            {
                Defect defect = new Defect();
                List<double> rowData = dataArr.GetRange(i * headers.Count, headers.Count);

                for (int j = 0; j < headers.Count; j++)
                {
                    FieldInfo fi = t.GetField(headers[j]);

                    if (fi != null)
                        fi.SetValue(defect, System.Convert.ChangeType(rowData[j], fi.FieldType));
                }

                list.Add(defect);
            }

            return list;
        }
    }

    #endregion

    #region InspectionTest 데이터 관련

    /*
    InspectionTest 1;
    SampleTestPlan 3363
      9 -38 
      8 -38 
      7 -38 
      ...
    AreaPerTest 6.3087e+009;
     */
    public class InspectionTestList : List<InspectionTest>, ICloneable
    {
        public object Clone()
        {
            InspectionTestList obj = new InspectionTestList();

            foreach (InspectionTest test in this)
            {
                obj.Add(test.Clone() as InspectionTest);
            }

            return obj;
        }

        public InspectionTest GetTest(int testNo)
        {
            foreach (var test in this)
            {
                if (test.TestNo == testNo)
                    return test;
            }

            throw new Exception(String.Format("해당하는 TEST 번호({0})를 찾을 수 없습니다.", testNo));
        }
    }

    public class InspectionTest : ICloneable
    {
        public void GetMinMaxDiePoint(out int xmin, out int xmax, out int ymin, out int ymax)
        {
            xmin = Int32.MaxValue;
            xmax = Int32.MinValue;

            ymin = Int32.MaxValue;
            ymax = Int32.MinValue;

            for (int i = 0; i < SampleTestPlan.Length; i++)
            {
                xmin = Math.Min(xmin, SampleTestPlan[i].X);
                xmax = Math.Max(xmax, SampleTestPlan[i].X);

                ymin = Math.Min(ymin, SampleTestPlan[i].Y);
                ymax = Math.Max(ymax, SampleTestPlan[i].Y);
            }
        }

        public int TestNo { get; internal set; }
        public double AreaPerTest { get; internal set; }
        public Point[] SampleTestPlan { get; internal set; }
        public TestParameterSpec TestParameterSpec { get; internal set; }
        public Summary Summary { get; internal set; }

        public object Clone()
        {
            InspectionTest obj = new InspectionTest();
            obj.TestNo = TestNo;
            obj.AreaPerTest = AreaPerTest;

            if (SampleTestPlan != null)
                obj.SampleTestPlan = SampleTestPlan.Clone() as Point[];

            if (TestParameterSpec != null)
                obj.TestParameterSpec = TestParameterSpec.Clone() as TestParameterSpec;

            if (Summary != null)
                obj.Summary = Summary.Clone() as Summary;

            return obj;
        }
    }

    #endregion

    #region TestParameterSpec 데이터 관련

    /*
    TestParametersSpec 2
      PIXELSIZE    SAMPLEPERCENTAGE  ;
    TestParametersList
      0.3906    0  ;
    DefectClusterSpec 3
     */
    public class TestParameterSpecList : List<TestParameterSpec>
    {
        public void Add(List<string> headers, List<double> dataArr)
        {
            if (headers == null || headers.Count == 0 || dataArr == null || dataArr.Count == 0)
                return;

            int length = dataArr.Count / headers.Count;

            for (int i = 0; i < length; i++)
            {
                Add(new TestParameterSpec(headers, dataArr.GetRange(i * headers.Count, headers.Count)));
            }
        }

        public TestParameterSpec GetParaSpec(int index)
        {
            if (Count > index)
                return this[index];

            return null;
        }
    }

    public class TestParameterSpec : ICloneable
    {
        private TestParameterSpec()
        {
        }

        public TestParameterSpec(List<string> headers, List<double> data)
        {
            ParsingUtil.SetPropertyData(this, headers, data);
        }

        public object Clone()
        {
            TestParameterSpec obj = new TestParameterSpec();
            obj.PIXELSIZE = PIXELSIZE;
            obj.SAMPLEPERCENTAGE = SAMPLEPERCENTAGE;
            return obj;
        }

        public double PIXELSIZE { get; internal set; }
        public double SAMPLEPERCENTAGE { get; internal set; }
    }

    #endregion
}
