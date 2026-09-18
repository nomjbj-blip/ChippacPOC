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
        public static readonly int CR = 2;
        public static readonly string DATE_FORMAT = "MM-dd-yy HH:mm:ss";
        public static readonly string IMAGE_FILE_NAME = "TiffFileName";
        public static readonly string IMAGE_COUNT_COLUMN = "IMAGECOUNT";
        public static readonly string END_OF_FILE = "EndOfFile;";
        public static readonly int MINIMUM_LENGTH = 20;

        // 별도 지정한 ClassLookup 이 있는 경우 파일 저장 시 사용
        public static Dictionary<int, string> UserClassLookup;

        /// <summary>
        /// 저장된 Map 정보가 있는 경우 해당 ORIGIN X,Y 인덱스를 가져옵니다.
        /// </summary>
        public delegate Point GetOriginDieIndexDelegate(string setupID, string stepID, DateTime setupTime);

        public static readonly string[] DEFECT_RECORD_SPEC =
        { 
            "DEFECTID", 
            "XREL", 
            "YREL", 
            "XINDEX", 
            "YINDEX",
            "XSIZE", 
            "YSIZE", 
            "DEFECTAREA",
            "DSIZE", 
            "CLASSNUMBER", 
            "TEST", 
            "CLUSTERNUMBER",
            "ROUGHBINNUMBER",
            "FINEBINNUMBER",
            "REVIEWSAMPLE", 
            "IMAGECOUNT", 
            "IMAGELIST" 
        };

        protected ParserKlarf(string fileName, bool useBaseLogic)
            : base(fileName)
        {
            if (useBaseLogic)
                RunFileParsing(fileName);
        }

        /// <summary>
        /// Klarf 파일을 이용한 인스턴스 생성
        /// </summary>
        public ParserKlarf(string fileName)
            : base(fileName)
        {
            RunFileParsing(fileName);
        }

        private void RunFileParsing(string fileName)
        {
            try
            {
                Wafers = new WaferList();
                DataSourceFromFile = true;

                // 파일을 텍스트로 읽기
                string text = ParsingUtil.FileToString(fileName);

                // END 문자열이 있는지 확인 
                if (text.Length < MINIMUM_LENGTH)
                {
                    ErrorFlag = true;
                    ErrorMessage = "파일 내용이 없습니다.";
                    return;
                }

                if (!text.Substring(text.Length - MINIMUM_LENGTH).Contains(END_OF_FILE))
                {
                    ErrorFlag = true;
                    ErrorMessage = String.Format("파일의 끝({0})이 없습니다.", END_OF_FILE);
                    return;
                }

                NameValueCollection mainCol;
                List<NameValueCollection> waferColList;
                GetNameValueCollection(text, "DieOrigin", out mainCol, out waferColList);

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
                LotID = GetString(mainCol, "LotID").ToUpper();
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

                    if (!String.IsNullOrEmpty(GetString(waferCol, "Slot")))
                        wafer.Slot = GetInt(GetString(waferCol, "Slot"));

                    if (String.IsNullOrEmpty(wafer.WaferID) || wafer.Slot == 0)
                        continue;

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
                            if (d.IMAGECOUNT > 0 && d.IMAGELIST > 0)
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

                    Wafers.Add(wafer);
                }

                foreach (Wafer wafer in Wafers)
                {
                    // Die 기준 좌표를 좌하단으로 변경
                    wafer.UpdateDieIndex(DieIndexSort.CenterToLowerLeft);
                }

                // 이미지 매핑
                ImageManagerList = ImageMapper.Mapping(this, text);
            }
            catch (Exception ex)
            {
                ErrorFlag = true;
                ErrorMessage = ex.Message + Environment.NewLine + ex.StackTrace;
            }
        }

        /// <summary>
        /// Defect를 회전하고 모든 Defect에 대해 X, Y, XREL, YREL, XINDEX, YINDEX 를 재계산 합니다.
        /// </summary>
        public void RotateAndRecalculate(int angle, DieIndexSort indexSort)
        {
            if (angle == 0)
                return;

            while (angle < 0)
                angle += 360;

            angle = angle % 360;

            if (angle == 0)
                return;

            foreach (Wafer wafer in Wafers)
            {
                int originX = (int)wafer.DieOriginX;
                int originY = (int)wafer.DieOriginY;

                if (originX != 0 || originY != 0)
                {
                    // 계산을 쉽게 하기 위해 Origin 좌표를 0,0으로 바꾼 후 계산한다.
                    wafer.AddIndex(-originX, -originY);
                }

                double dieX = DiePitchX;
                double dieY = DiePitchY;

                // SampleCenterLocation, DiePitch 회전
                for (int a = 0; a < angle; a += 90)
                {
                    double x = wafer.SampleCenterLocationY;
                    double y = dieX - wafer.SampleCenterLocationX;
                    wafer.SampleCenterLocationX = x;
                    wafer.SampleCenterLocationY = y;

                    double tmp = dieX;
                    dieX = dieY;
                    dieY = tmp;
                }

                foreach (InspectionTest test in wafer.TestList)
                {
                    // 90도씩 회전 (X,Y) -> (Y,-X)
                    for (int a = 0; a < angle; a += 90)
                    {
                        // SampleTestPlan 회전
                        for (int i = 0; i < test.SampleTestPlan.Length; i++)
                        {
                            int x = test.SampleTestPlan[i].X;
                            int y = test.SampleTestPlan[i].Y;

                            test.SampleTestPlan[i].X = y;
                            test.SampleTestPlan[i].Y = -x;
                        }
                    }
                }

                // Defect X,Y 는 wafer center 기준이므로 각도에 맞게 회전
                foreach (Defect defect in wafer.DefectList)
                {
                    RotatePoint(angle, ref defect.X, ref defect.Y);
                }

                dieX = DiePitchX;
                dieY = DiePitchY;

                // 90도씩 회전 (X,Y) -> (Y,-X)
                for (int a = 0; a < angle; a += 90)
                {
                    // XINDEX, YINDEX, XREL, YREL
                    foreach (Defect defect in wafer.DefectList)
                    {
                        int x = defect.XINDEX;
                        int y = defect.YINDEX;
                        
                        defect.XINDEX = y;
                        defect.YINDEX = -x;

                        double xrel = defect.XREL;
                        double yrel = defect.YREL;

                        defect.XREL = yrel;
                        defect.YREL = dieX - xrel;
                    }

                    double tmp = dieX;
                    dieX = dieY;
                    dieY = tmp;
                }

                if (angle % 180 == 90)
                {
                    int tmp = originX;
                    originX = originY;
                    originY = tmp;
                }

                if (indexSort == DieIndexSort.CenterToLowerLeft)
                {
                    wafer.DieOriginX = originX;
                    wafer.DieOriginY = originY;

                    // 가운데 (0,0) 인 좌표에서 좌하단으로 변경
                    wafer.AddIndex(originX, originY);
                }
                else if (indexSort == DieIndexSort.LowerLeftToCenter)
                {
                    wafer.DieOriginX = 0;
                    wafer.DieOriginY = 0;
                }
            }

            // Die Size 회전
            if (angle % 180 == 90)
            {
                double tmp = DiePitchX;
                DiePitchX = DiePitchY;
                DiePitchY = tmp;
            }
        }

        private void RotatePoint(double clockwiseAngle, ref double x, ref double y)
        {
            double a = clockwiseAngle / 180d * Math.PI;

            double tmpX = x * Math.Cos(a) + y * Math.Sin(a);
            double tmpY = -x * Math.Sin(a) + y * Math.Cos(a);

            x = tmpX;
            y = tmpY;
        }

        public void ChangeLotID(string newLotID)
        {
            if (String.IsNullOrEmpty(newLotID))
                return;

            LotID = newLotID;

            foreach (Wafer wafer in Wafers)
            {
                SetFormattedWaferID(wafer);
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
        public ParserKlarf(Defect[] defectArr, DmsWaferDieInfo info, Dictionary<int, string> classLookupDic = null)
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

            if (classLookupDic == null)
                classLookupDic = ParserKlarf.UserClassLookup;

            wafer.ClassLookup = classLookupDic;

            wafer.SetDefect(defectArr);

            InspectionTest test = new InspectionTest();
            wafer.TestList.Add(test);
            test.TestNo = 1;

            test.SampleTestPlan = info.Dies;
            test.AreaPerTest = stepInfo.AreaPerTest;
        }

        /// <summary>
        /// {LOT ID}-{NO} 형태로 표현되는 WAFER ID를 가져옵니다.
        /// </summary>
        protected void SetFormattedWaferID(Wafer wafer)
        {
            // Wafer ID값이 없으면 Slot으로 설정한다.
            if (String.IsNullOrEmpty(wafer.WaferID))
                wafer.WaferID = String.Format("{0}-{1:D2}", LotID, wafer.Slot);

            // @ trim
            wafer.WaferID = wafer.WaferID.Trim().Trim('@').Trim();

            int no;

            //정상적인 Wafer ID 형식이면 그대로 사용하고 아니면 아래에서 Lot-SlotID조합하여 Wafer ID 를 만든다
            string strCheckWaferID = wafer.WaferID;

            if (strCheckWaferID.Contains(LotID))
            {
                strCheckWaferID = strCheckWaferID.Replace(LotID, "");

                if (strCheckWaferID.Contains("-"))
                {
                    strCheckWaferID = strCheckWaferID.Replace("-", "");

                    if (int.TryParse(strCheckWaferID, out no) == true && strCheckWaferID.Length == 2)
                        return;
                }
            }

            if (wafer.WaferID.Length <= 2 && Int32.TryParse(wafer.WaferID, out no))
                wafer.WaferID = String.Format("{0}-{1:D2}", LotID, no);
            else
                wafer.WaferID = String.Format("{0}-{1:D2}", LotID, wafer.Slot);
        }

        public override DateTime GetBackupDateTime()
        {
            return ResultTimestamp;
        }

        /// <summary>
        /// Review 설비에서 사용 가능하도록 파일로 저장합니다.
        /// </summary>
        /// <param name="orginDieToZero">Origin Die의 인덱스를 0으로 할지를 나타냅니다.</param>
        public void SaveFile(string fileName)
        {
            SaveFile(fileName, true);
        }

        private string GetFirstDefectImageName()
        {
            foreach (Wafer wafer in Wafers)
            {
                foreach (Defect defect in wafer.DefectList)
                {
                    if (defect.Images.Count > 0)
                        return defect.Images.GetDefectImage(0).IMAGEPATH;
                }
            }

            return null;
        }

        /// <summary>
        /// Review 설비에서 사용 가능하도록 파일로 저장합니다.
        /// </summary>
        /// <param name="orginDieToZero">Origin Die의 인덱스를 0으로 할지를 나타냅니다.</param>
        /// <param name="saveWafer">특정 Wafer 데이터만 저장할 경우 지정. 기본값은 전체 저장</param>
        /// <param name="includeImagePath">Defect 이미지에 대한 경로를 포함할지 여부. 기본값은 포함하지 않음</param>
        public void SaveFile(string fileName, bool originDieToZero, Wafer saveWafer = null, bool includeImagePath = false)
        {
            if (Wafers.Count == 0)
                throw new Exception("Klarf export error. Not found Wafer data.");

            // 데이터소스가 파일인 경우 Origin Die가 이미 0 이다.
            if (DataSourceFromFile)
                originDieToZero = true;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("FileVersion {0};", FileVersion));
            sb.AppendLine(String.Format("FileTimestamp {0};", FileTimestamp.ToString(DATE_FORMAT)));

            string firstDefectImageName = null;

            if (includeImagePath)
            {
                sb.AppendLine("TiffSpec 6.0 R NA;");
                firstDefectImageName = GetFirstDefectImageName();
                
                // Defect 이미지가 있는 경우 TiffSpec 다음에 넣어준다.
                if (!String.IsNullOrEmpty(firstDefectImageName))
                    sb.AppendLine(String.Format("TiffFileName {0};", firstDefectImageName));
            }

            sb.AppendLine(String.Format("InspectionStationID \"{0}\" \"{1}\" \"{2}\";", Maker, Model, Equip));
            sb.AppendLine(String.Format("SampleType {0};", SampleType));
            sb.AppendLine(String.Format("ResultTimestamp {0};", ResultTimestamp.ToString(DATE_FORMAT)));
            sb.AppendLine(String.Format("LotID \"{0}\";", LotID));
            sb.AppendLine(String.Format("SampleSize 1 {0};", 200));// SampleSize));
            sb.AppendLine(String.Format("SetupID \"{0}\" {1};", SetupID, SetupTimestamp.ToString(DATE_FORMAT)));
            sb.AppendLine(String.Format("StepID \"{0}\";", StepID));
            sb.AppendLine(String.Format("DeviceID \"{0}\";", DeviceID));
            sb.AppendLine(String.Format("SampleOrientationMarkType {0};", SampleOrientationMarkType));
            sb.AppendLine(String.Format("OrientationMarkLocation {0};", OrientationMarkLocation));
            sb.AppendLine(String.Format("DiePitch {0:e10} {1:e10};", DiePitchX, DiePitchY));

            foreach (Wafer wafer in Wafers)
            {
                if (saveWafer != null && wafer != saveWafer)
                    continue;

                double dieOriginX = originDieToZero ? 0 : wafer.DieOriginX;
                double dieOriginY = originDieToZero ? 0 : wafer.DieOriginY;

                sb.AppendLine(String.Format("DieOrigin {0:e10} {1:e10};", dieOriginX, dieOriginY));
                sb.AppendLine(String.Format("WaferID \"{0}\";", wafer.WaferID));
                sb.AppendLine(String.Format("Slot {0};", wafer.Slot));
                sb.AppendLine(String.Format("SampleCenterLocation {0:e10} {1:e10};", wafer.SampleCenterLocationX, wafer.SampleCenterLocationY));

                // ClassLookup 정보는 최초 한번만 기록
                if (Wafers.IndexOf(wafer) == 0)
                {
                    // 별도 지정한 ClassLookup 이 있는 경우 파일 저장 시 사용
                    if (UserClassLookup != null && UserClassLookup.Count > 1)
                        wafer.ClassLookup = UserClassLookup;

                    // ClassLookup
                    sb.AppendLine(GetClassLookupString(wafer.ClassLookup));
                }

                if (wafer.TestList.Count == 0)
                    throw new Exception("Klarf export error. Not found Inspection Test info.");

                foreach (InspectionTest test in wafer.TestList)
                {
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

                    // 파일로 저장 시 10의 8승을 곱한다.
                    sb.AppendLine(String.Format("AreaPerTest {0:e10};", test.AreaPerTest * 100000000));
                }

                // Defect 데이터 기록
                if (wafer.DefectList != null && wafer.DefectList.Count > 0)
                {
                    // DefectRecordSpec
                    string[] defectHeader = DEFECT_RECORD_SPEC; // (wafer.DefectHeaders != null && wafer.DefectHeaders.Count > 0) ? wafer.DefectHeaders.ToArray() : DEFECT_RECORD_SPEC;

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

                        StringBuilder rowVal = new StringBuilder();

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

                            // DACrux TQD_DEFECT 테이블에는 IMAGELIST 필드가 없어서 값이 0으로 들어가게 되므로 IMAGECOUNT 값과 같게 한다. 2020.01.05 Taihi,Kim.
                            if (includeImagePath && name == "IMAGELIST" && (fi = t.GetField("IMAGECOUNT")) != null)
                                val = fi.GetValue(defect);

                            rowVal.AppendFormat(" {0}", val);
                        }

                        if (includeImagePath && defect.Images.Count > 0)
                        {
                            foreach (DefectImage image in defect.Images)
                            {
                                if (image.IMAGEPATH != firstDefectImageName)
                                {
                                    // 이전 줄에 줄바꿈 문자 제거 후 세미콜론을 넣어준다.
                                    sb.Remove(sb.Length - CR, CR);
                                    sb.AppendLine(";");

                                    sb.AppendLine(String.Format("TiffFileName {0};", Path.GetFileName(image.IMAGEPATH)));
                                    sb.AppendLine("DefectList");
                                }

                                sb.AppendLine(rowVal.ToString());
                                sb.Append("\t1\t0");
                            }
                        }
                        else
                        {
                            sb.Append(rowVal.ToString());
                        }

                        if (i < wafer.DefectList.Count - 1)
                            sb.AppendLine();
                        else
                            sb.AppendLine(";");
                    }
                }
            }

            sb.AppendLine("EndOfFile;");

            string path = Path.GetDirectoryName(fileName);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

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
                string fileName = GetGeneralFileName(FileNameRule.Lot_Slot_Layer_000, i);

                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);

                string fullPath = Path.Combine(savePath, RemoveInvalidCharFromFileName(fileName));

                SaveFile(fullPath, originDieToZero, Wafers[i]);
                arr[i] = fullPath;
            }

            return arr;
        }

        public string GetGeneralFileName(FileNameRule fileNameRule, int waferIndex)
        {
            int slot = Wafers.Count > waferIndex ? Wafers[waferIndex].Slot : 0;

            if (fileNameRule == FileNameRule.Lot_Slot_Layer_000)
                return String.Format("{0}_{1:00}_{2}{3}", LotID, slot, StepID, ".001");
            else
                throw new Exception("FileNameRule에 대한 파일명 규칙이 정의되지 않았습니다");
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
        public static void GetNameValueCollection(string fileText, string waferSplitText, out NameValueCollection mainCol, out List<NameValueCollection> waferColList)
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
                    if (key == waferSplitText)
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

        public string FileVersion { get; protected set; }
        public DateTime FileTimestamp { get; protected set; }
        public string TiffSpec { get; protected set; }
        public string Equip { get; set; }
        public string Maker { get; set; }
        public string Model { get; set; }
        public InspectionStationValueOrder InspectionStationOrder { get; protected set; }
        public string SampleType { get; protected set; }
        public DateTime ResultTimestamp { get; protected set; }
        public int SampleSize { get; protected set; }
        public string DeviceID { get; protected set; }
        public string SetupID { get; protected set; }
        public DateTime SetupTimestamp { get; protected set; }
        public string ResultsID { get; protected set; }
        public string StepID { get; protected set; }
        public string SampleOrientationMarkType { get; protected set; }
        public string OrientationMarkLocation { get; set; }
        public double DiePitchX { get; protected set; }
        public double DiePitchY { get; protected set; }
        public bool MapOriginFromDB { get; set; }
        public bool DataSourceFromFile { get; set; }

        public WaferList Wafers { get; protected set; }

        public enum OrientationMark { DOWN = 0, LEFT = 90, UP = 180, RIGHT = 270 }

        public static string AngleToOrientationMarkLocation(int angle)
        {
            return ((OrientationMark)angle).ToString();
        }

        public static int OrientationMarkLocationToAngle(string location)
        {
            OrientationMark angle;

            if (Enum.TryParse<OrientationMark>(location, out angle))
                return (int)angle;

            return (int)OrientationMark.DOWN;
        }

        public int GetAngle()
        {
            return OrientationMarkLocationToAngle(OrientationMarkLocation);
        }

        /// <summary>
        /// ClassLookup 정보를 추가합니다.
        /// </summary>
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
            {
                try
                {
                    // 간혹 파일의 속성이 ReadOnly 인 경우 IOException 이 발생하므로 속성을 Normal로 바꾼다.
                    File.SetAttributes(fileName, FileAttributes.Normal);
                }
                catch { }

                File.WriteAllText(fileName, newText);
            }
        }

        public ImageManagerList ImageManagerList
        {
            get;
            protected set;
        }

        public static GetOriginDieIndexDelegate GetOriginDieIndex
        {
            get;
            set;
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

    public enum DieIndexSort
    {
        /// <summary>
        /// 가운데 (0,0) 기준인 좌표를 좌하단 기준으로 변경
        /// </summary>
        CenterToLowerLeft,
        /// <summary>
        /// 좌하단 기준인 좌표를 가운데 (0,0) 기준으로 변경
        /// </summary>
        LowerLeftToCenter
    }

    public enum FileNameRule
    {
        Lot_Slot_Layer_000
    }
}
