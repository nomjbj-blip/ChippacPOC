using System;
using System.Collections.Generic;
using DACrux.Base;
using System.Reflection;
using System.Drawing;
using System.IO;

namespace DACrux.Data.Parser.Klarf
{
    // 2020.01.12 Taihi,Kim.
    /// <summary>
    /// Klarf file Ver1.8
    /// </summary>
    public class ParserKlarf_18 : ParserKlarf
    {
        public static readonly char OPEN = '{';
        public static readonly char CLOSE = '}';

        public ParserKlarf_18(string fileName)
            : base(fileName, false)
        {
            Wafers = new WaferList();
            DataSourceFromFile = true;
            ImageManagerList = new ImageManagerList();

            // 파일을 텍스트로 읽기
            string text = ParsingUtil.FileToString(fileName);

            KeyDataList list = GetKeyDataList(text); //Record FileRecord  "1.8"
            FileVersion = list[0].Value;

            list = GetKeyDataList(list[0].Data); //Record LotRecord "14BX44"

            KeyData dat = list["FileTimestamp"];
            string[] arr = dat.GetDataToArray();
            FileTimestamp = GetDateTime(String.Format("{0} {1}", arr[0], arr[1]));

            dat = list["LotRecord"];
            LotID = dat.Value;
            list = GetKeyDataList(dat.Data);

            DeviceID = list["DeviceID"].GetDataToArray()[0];

            arr = list["DiePitch"].GetDataToArray();
            DiePitchX = GetDouble(arr[0]) / 1000; // nm -> um
            DiePitchY = GetDouble(arr[1]) / 1000; // nm -> um

            Equip = list["InspectionStationID"].GetDataToArray()[2];
            OrientationMarkLocation = AngleToOrientationMarkLocation(GetInt(list["OrientationMarkLocation"].GetDataToArray()[0]));

            arr = list["RecipeID"].GetDataToArray();
            SetupID = arr[0];
            SetupTimestamp = GetDateTime(String.Format("{0} {1}", arr[1], arr[2]));

            arr = list["ResultTimestamp"].GetDataToArray();
            ResultTimestamp = GetDateTime(String.Format("{0} {1}", arr[0], arr[1]));
            SampleOrientationMarkType = list["SampleOrientationMarkType"].GetDataToArray()[0];
            SampleSize = GetInt(list["SampleSize"].GetDataToArray()[0]) / 1000 / 1000; // nm -> mm
            SampleType = list["SampleType"].GetDataToArray()[0];
            StepID = list["StepID"].GetDataToArray()[0];

            foreach (var waferRecord in list.GetAllData("WaferRecord"))
            {
                Wafer wafer = new Wafer();
                wafer.Parser = this;
                wafer.WaferID = waferRecord.Value;

                list = GetKeyDataList(waferRecord.Data);

                arr = list["DieOrigin"].GetDataToArray();
                wafer.DieOriginX = GetDouble(arr[0]);
                wafer.DieOriginY = GetDouble(arr[1]);

                arr = list["ResultTimestamp"].GetDataToArray();
                ResultTimestamp = GetDateTime(String.Format("{0} {1}", arr[0], arr[1]));

                arr = list["SampleCenterLocation"].GetDataToArray();
                wafer.SampleCenterLocationX = GetDouble(arr[0]) / 1000;  // nm -> um
                wafer.SampleCenterLocationY = GetDouble(arr[1]) / 1000;  // nm -> um

                wafer.Slot = GetInt(list["SlotNumber"].GetDataToArray()[0]);

                wafer.SetDefect(GetDefectArray(list["DefectList"].Data));

                foreach (Defect defect in wafer.DefectList)
                {
                    defect.X = (double)(defect.XINDEX * DiePitchX + defect.XREL) - wafer.SampleCenterLocationX;
                    defect.Y = (double)(defect.YINDEX * DiePitchY + defect.YREL) - wafer.SampleCenterLocationY;
                }

                foreach (var testRecord in list.GetAllData("TestRecord"))
                {
                    var subList = GetKeyDataList(testRecord.Data);

                    InspectionTest test = new InspectionTest();
                    wafer.TestList.Add(test);
                    test.TestNo = GetInt(testRecord.Value);

                    if (subList["AreaPerTest"] != null)
                        test.AreaPerTest = GetDouble(subList["AreaPerTest"].GetDataToArray()[0]) / 1000 / 1000; // nm2 -> um2

                    if (subList["SampleTestPlanList"] != null)
                    {
                        string[] data = GetKeyDataList(subList["SampleTestPlanList"].Data).GetDataKind("Data").Data.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        List<Point> pointList = new List<Point>();

                        for (int i = 0; i < data.Length; i++)
                        {
                            arr = data[i].Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            pointList.Add(new Point(GetInt(arr[0]), GetInt(arr[1])));
                        }

                        test.SampleTestPlan = pointList.ToArray();
                    }
                }

                if (list["SummaryRecord"] != null)
                {
                    SummaryList summList = new SummaryList();

                    var subList = GetKeyDataList(GetKeyDataList(list["SummaryRecord"].Data)[0].Data);
                    List<string> summaryColumnList = ColumnsToList(subList.GetDataKind("Columns").Data);

                    KeyData data = subList.GetDataKind("Data");
                    arr = data.Data.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < arr.Length; i++)
                    {
                        string row = arr[i].Trim();

                        if (String.IsNullOrEmpty(row))
                            continue;

                        // 줄바꿈 문자 제거
                        row = row.Replace(Environment.NewLine, String.Empty);
                        row = row.Replace("\n", String.Empty);

                        string[] rowDataArr = row.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        double[] rowValArr = new double[rowDataArr.Length];

                        for (int j = 0; j < rowDataArr.Length; j++)
                            rowValArr[j] = GetDouble(rowDataArr[j]);

                        summList.Add(new Summary(summaryColumnList, new List<double>(rowValArr)));
                    }

                    foreach (var test in wafer.TestList)
                        test.Summary = summList.GetSummary(test.TestNo);
                }

                // Test 데이터 검증 - AREAPERTEST 없는 경우 삭제
                for (int i = wafer.TestList.Count - 1; i >= 0; i--)
                {
                    if (wafer.TestList[i].AreaPerTest == 0)
                        wafer.TestList.RemoveAt(i);
                }

                SetFormattedWaferID(wafer);
                Wafers.Add(wafer);
            }

            foreach (Wafer wafer in Wafers)
            {
                // Die 기준 좌표를 좌하단으로 변경
                wafer.UpdateDieIndex(DieIndexSort.CenterToLowerLeft);
            }
        }

        private SearchResult FindBracket(string text, int startIndex)
        {
            int idx1 = text.IndexOf(OPEN, startIndex);
            int idx2 = text.IndexOf(CLOSE, startIndex);

            if (idx1 < 0 && idx2 < 0)
                return SearchResult.Empty;

            Bracket bracket;
            int index;

            if (idx1 < 0)
            {
                bracket = Bracket.Close;
                index = idx2;
            }
            else if (idx2 < 0)
            {
                bracket = Bracket.Open;
                index = idx1;
            }
            else if (idx1 < idx2)
            {
                bracket = Bracket.Open;
                index = idx1;
            }
            else
            {
                bracket = Bracket.Close;
                index = idx2;
            }

            return new SearchResult(bracket, index);
        }

        private string Substring(string text, char ch, int startIndex, out int searchIndex)
        {
            searchIndex = text.IndexOf(ch, startIndex);

            if (searchIndex < 0)
                return null;
            else
                return text.Substring(startIndex, searchIndex - startIndex);
        }

        private KeyDataList GetKeyDataList(string text)
        {
            KeyDataList list = new KeyDataList();

            int start = 0;
            Bracket br = Bracket.None;
            string key = null;
            string value = null;

            while (true)
            {
                if (br == Bracket.None)
                {
                    int idx;
                    key = Substring(text, OPEN, start, out idx);

                    if (String.IsNullOrEmpty(key))
                        break;

                    br = Bracket.Open;
                    start = idx + 1;
                }
                else if (br == Bracket.Open)
                {
                    int cnt = 1;
                    int openIndex = start;

                    while (true)
                    {
                        SearchResult result = FindBracket(text, start);

                        if (!result.Success)
                            throw new Exception("계산 로직 에러");

                        cnt += result.Bracket == Bracket.Open ? +1 : -1;
                        start = result.Index + 1;

                        if (cnt == 0)
                        {
                            value = text.Substring(openIndex, result.Index - openIndex);
                            list.Add(key.Trim(), value.Trim());
                            br = Bracket.None;
                            break;
                        }
                    }
                }
            }

            return list;
        }

        private List<string> ColumnsToList(string text)
        {
            string[] arr = text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            List<string> headers = new List<string>();

            for (int i = 0; i < arr.Length; i++)
            {
                string column = arr[i].Trim();

                if (String.IsNullOrEmpty(column))
                    continue;

                string[] values = column.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (values != null && values.Length > 1)
                    headers.Add(values[1]);
            }

            return headers;
        }

        private Defect[] GetDefectArray(string text)
        {
            KeyDataList list = GetKeyDataList(text);

            //
            // 헤더 처리
            //
            List<string> headers = ColumnsToList(list.GetDataKind("Columns").Data);

            // 매핑할 Defect Field 
            FieldInfo[] fieldArr = new FieldInfo[headers.Count];

            for (int i = 0; i < fieldArr.Length; i++)
                fieldArr[i] = typeof(Defect).GetField(headers[i]);

            //
            // Data 처리
            //
            KeyData data = list.GetDataKind("Data");
            string[] rowArr = data.Data.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            DefectList defectList = new DefectList();
            ImageManager tifImageManager = null;

            for (int i = 0; i < rowArr.Length; i++)
            {
                string row = rowArr[i].Trim();

                if (String.IsNullOrEmpty(row))
                    continue;

                // 줄바꿈 문자 제거
                row = row.Replace(Environment.NewLine, String.Empty);
                row = row.Replace("\n", String.Empty);

                DefectImageList defectImgList = null;

                // 이미지 정보 있는 경우 처리
                if (row.Contains("Field ImageFileName")) // 여러 이미지를 포함하는 Tiff 인 경우
                {
                    KeyDataList subList = GetKeyDataList(row);
                    string imageFile = subList[0].GetDataToArray()[0];
                    ImageManagerList.Add(Path.Combine(DirectoryName, imageFile));
                    tifImageManager = ImageManagerList[imageFile];

                    row = row.Substring(row.IndexOf(CLOSE) + 1).Trim();
                }

                if (row.Contains("Images"))
                {
                    int idx1 = row.IndexOf("Images");
                    int idx2 = row.IndexOf(CLOSE);

                    var img = GetKeyDataList(row.Substring(idx1))[0];
                    int imageCount = GetInt(img.Name);
                    string[] arr = img.GetDataToArray();

                    defectImgList = new DefectImageList();

                    if (tifImageManager != null) // 다중 Tiff에 대한 이미지는 이미지 1개당 데이터 2개 ex) Images 1 { 1 "0" }
                    {
                        foreach (string val in arr)
                        {
                            int seq = GetInt(KeyData.GetDataToArray(val, ' ')[0]);
                            string imageFile = Path.GetFileName(tifImageManager.GetImageFileName(seq));

                            defectImgList.Add(seq, imageFile);

                            if (!tifImageManager.FrameDictionary.ContainsKey(seq))
                                tifImageManager.FrameDictionary.Add(seq, imageFile);
                        }
                    }
                    else // 단일 이미지는 이미지 1개당 데이터 4개이고 ex) Images 2 { "24046-FAB1-172P24-7-667-I1-001.jpg" "JPG" 1 "UNK",  "24046-FAB1-172P24-7-667-I2-001.jpg" "JPG" 2 "UNK" } 
                    {
                        foreach (string val in arr)
                        {
                            string imageFile = KeyData.GetDataToArray(val, ' ')[0];
                            ImageManagerList.Add(Path.Combine(DirectoryName, imageFile));
                            defectImgList.Add(imageFile);
                        }
                    }

                    // row 데이터에서 이미지 부분 삭제
                    row = row.Substring(0, idx1) + row.Substring(idx2 + 1);
                }

                // Defect 데이터 처리
                string[] rowDataArr = row.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                Defect defect = new Defect();

                for (int f = 0; f < fieldArr.Length; f++)
                {
                    if (f >= rowDataArr.Length || fieldArr[f] == null)
                        continue;

                    fieldArr[f].SetValue(defect, System.Convert.ChangeType(rowDataArr[f], fieldArr[f].FieldType));
                }

                // 단위 변경 nm -> um
                defect.XREL = defect.XREL / 1000;
                defect.YREL = defect.YREL / 1000;
                defect.XSIZE = defect.XSIZE / 1000;
                defect.YSIZE = defect.YSIZE / 1000;
                defect.DEFECTAREA = defect.DEFECTAREA / 1000 / 1000;
                defect.DSIZE = defect.DSIZE / 100000;

                int idx = defectList.BinarySearch(defect);

                // 이미지가 여러개인 경우 같은 DEFECTID 데이터가 여러번 올라오므로 최초 추가된 Defect에 이미지를 연결한다.
                if (idx < 0)
                    defectList.Insert(~idx, defect);
                else
                    defect = defectList[idx];

                if (defectImgList != null)
                {
                    defect.Images.AddRange(defectImgList);
                    defect.IMAGECOUNT = defect.IMAGELIST = defectImgList.Count;
                }
            }

            return defectList.ToArray();
        }
    }

    internal enum Bracket
    {
        None,
        Open,
        Close
    }

    internal class SearchResult
    {
        public SearchResult(Bracket bracket, int index)
        {
            Bracket = bracket;
            Index = index;
        }

        static SearchResult()
        {
            Empty = new SearchResult(Bracket.None, -1);
        }

        public static readonly SearchResult Empty;

        public Bracket Bracket { get; private set; }
        public int Index { get; private set; }
        public bool Success { get { return Bracket != Bracket.None; } }
    }

    internal class KeyData
    {
        enum Col { DataKind, Name, Value }

        public static readonly char[] SPLITTER = new char[] { ' ' };
        public static readonly string DataKind_Record = "Record";
        public static readonly string DataKind_Field = "Field";
        public static readonly string DataKind_List = "List";

        public KeyData(string key, string value)
        {
            ArrayDataLength = 1;

            string[] arr = key.Split(SPLITTER, StringSplitOptions.RemoveEmptyEntries);

            DataKind = arr[(int)Col.DataKind];
            Name = arr[(int)Col.Name];

            if (DataKind == DataKind_Record) // Record 인 경우 3번째는 값 
                Value = GetValue(arr, Col.Value);
            else if (DataKind == DataKind_Field) // Field 인 경우 3번째는 데이터 길이
                ArrayDataLength = GetInt(arr, Col.Value);

            Data = value;
        }

        private string GetValue(string[] array, Col col)
        {
            if (array == null || array.Length <= (int)col)
                return null;

            string text = array[(int)col];

            if (String.IsNullOrEmpty(text))
                return null;

            return text.Trim().Trim('"');
        }

        public static string[] GetDataToArray(string text, char ch)
        {
            if (String.IsNullOrEmpty(text))
                return null;

            string[] arr = text.Split(new char[] { ch }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = arr[i].Trim().Trim('"');
            }

            return arr;
        }

        public string[] GetDataToArray()
        {
            return GetDataToArray(Data, ',');
        }

        private int GetInt(string[] array, Col col)
        {
            if (array == null || array.Length <= (int)col)
                return 0;

            string text = array[(int)col];

            if (String.IsNullOrEmpty(text))
                return 0;

            int num;

            if (Int32.TryParse(text.Trim(), out num))
                return num;

            return 0;
        }

        public override string ToString()
        {
            return String.Format("{0} ({1}){2}", Name, DataKind, (DataKind == DataKind_List) ? null : " : " + ((DataKind == DataKind_Record) ? Value : ArrayDataLength.ToString()));
        }

        public string DataKind { get; private set; }
        public string Name { get; private set; }
        public int ArrayDataLength { get; private set; }
        public string Value { get; private set; }
        public string Data { get; private set; }
    }

    internal class KeyDataList : List<KeyData>
    {
        private Dictionary<string, int> m_dic = new Dictionary<string, int>();

        public new void Add(KeyData item)
        {
            base.Add(item);

            int index = Count - 1;
            m_dic[item.Name] = index;
        }

        public void Add(string key, string value)
        {
            Add(new KeyData(key, value));
        }

        public KeyDataList GetAllData(string name)
        {
            KeyDataList list = new KeyDataList();

            foreach (var item in this)
            {
                if (item.Name == name)
                    list.Add(item);
            }

            return list;
        }

        public KeyData this[string name]
        {
            get
            {
                if (!m_dic.ContainsKey(name))
                    return null;

                return this[m_dic[name]];
            }
        }

        public KeyData GetDataKind(string dataKind)
        {
            foreach (var item in this)
            {
                if (item.DataKind == dataKind)
                    return item;
            }

            return null;
        }
    }
}