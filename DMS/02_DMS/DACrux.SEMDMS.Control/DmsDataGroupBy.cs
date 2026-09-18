using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DACrux.Base;
using System.Reflection;

namespace DACrux.SEMDMS.Control
{
    public class DmsDataGroupBy
    {
        public static readonly string EMPTY = " ";

        public static Dictionary<ColRow, DefectInfo> GroupBy(DefectList defectList, DPWafer[] dpWaferArr, GalleryColumnItem col, GalleryRowItem row)
        {
            Dictionary<string, DefectInfo> dic;

            // COL
            if (col == GalleryColumnItem.None)
                dic = GroupByColumn(defectList, dpWaferArr, "WaferID,StepSeq", true);
            else if (col == GalleryColumnItem.LotID)
                dic = GroupByColumn(defectList, dpWaferArr, "LotID");
            else if (col == GalleryColumnItem.WaferID)
                dic = GroupByColumn(defectList, dpWaferArr, "WaferID");
            else if (col == GalleryColumnItem.DeviceID)
                dic = GroupByColumn(defectList, dpWaferArr, "DeviceID");
            else if (col == GalleryColumnItem.Inspector)
                dic = GroupByColumn(defectList, dpWaferArr, "Inspector");
            else if (col == GalleryColumnItem.StepID)
                dic = GroupByColumn(defectList, dpWaferArr, "StepID");
            else if (col == GalleryColumnItem.ResultTime)
                dic = GroupByColumn(defectList, dpWaferArr, "ResultTimestamp");
            else
                throw new Exception("정의되지 않았습니다.");

            // ROW
            Dictionary<ColRow, DefectInfo> result = new Dictionary<ColRow, DefectInfo>();
            if (row == GalleryRowItem.DefectClass)
                result = GroupByRow_DefectClass(dic, "CLASSNAME");
            else if (row == GalleryRowItem.StepID)
                result = GroupByRow(dic, dpWaferArr, "StepID");
            else if (row == GalleryRowItem.LotID)
                result = GroupByRow(dic, dpWaferArr, "LotID");
            else if (row == GalleryRowItem.WaferID)
                result = GroupByRow(dic, dpWaferArr, "WaferID");
            else if (row == GalleryRowItem.DeviceID)
                result = GroupByRow(dic, dpWaferArr, "DeviceID");
            else if (row == GalleryRowItem.Inspector)
                result = GroupByRow(dic, dpWaferArr, "Inspector");
            else if (row == GalleryRowItem.ResultTime)
                result = GroupByRow(dic, dpWaferArr, "ResultTimestamp");
            else if (row == GalleryRowItem.None)
                result = GroupByRow_None(dic);
            else
                throw new Exception("정의되지 않았습니다.");

            if (col == GalleryColumnItem.None)
            {
                int n = 1;
                foreach (var item in result)
                    item.Key.ChangeCol(new string(' ', n));
            }

            return result;
        }

        public static Dictionary<ColRow, DefectInfo> GroupBy(Dictionary<ColRow, DefectInfo> dic, int col)
        {
            Dictionary<ColRow, DefectInfo> newDic = new Dictionary<ColRow, DefectInfo>();

            int i = 0;

            foreach (var item in dic)
            {
                string colHeader = String.Empty.PadRight(1 + i % col);
                string rowHeader = String.Empty.PadRight(1 + i / col);

                newDic.Add(new ColRow(colHeader, rowHeader), item.Value);

                i++;
            }

            return newDic;
        }

        private static Dictionary<string, DefectInfo> GroupByLot(DefectList defectList)
        {
            Dictionary<string, DefectInfo> dic = new Dictionary<string, DefectInfo>();

            foreach (Defect defect in defectList)
            {
                string lotID = DmsCache.Instance[defect.STEP_SEQ].StepInfo.LotID;

                if (!dic.ContainsKey(lotID))
                    dic.Add(lotID, new DefectInfo(defect.STEP_SEQ));

                dic[lotID].DefectList.Add(defect);
            }

            return dic;
        }

        private static Dictionary<string, DefectInfo> GroupByColumn(DefectList defectList, DPWafer[] dpWaferArr, string dmsStepInfoProperty, bool appendEmptyWafer = false)
        {
            Dictionary<string, DefectInfo> dic = new Dictionary<string, DefectInfo>();
            List<string> valList = new List<string>();

            // None / None 인 경우 Defect가 없는 Wafer 표현을 위해 로직 추가 2019.12.01 Taihi,Kim.
            if (appendEmptyWafer && dpWaferArr != null)
            {
                foreach (var wafer in dpWaferArr)
                {
                    long stepSeq = Int64.Parse(wafer.StepSeq);
                    DmsStepInfo info = DmsCache.Instance[stepSeq].StepInfo;

                    string val = GetValue(info, dmsStepInfoProperty);

                    if (!dic.ContainsKey(val))
                        dic.Add(val, new DefectInfo(stepSeq));
                }
            }

            foreach (Defect defect in defectList)
            {
                DmsStepInfo info = DmsCache.Instance[defect.STEP_SEQ].StepInfo;

                string val = GetValue(info, dmsStepInfoProperty);

                if (!dic.ContainsKey(val))
                    dic.Add(val, new DefectInfo(defect.STEP_SEQ));

                dic[val].DefectList.Add(defect);
            }

            string[] arr = new string[dic.Count];
            dic.Keys.CopyTo(arr, 0);
            Array.Sort(arr);

            Dictionary<string, DefectInfo> newDic = new Dictionary<string, DefectInfo>();

            // 정렬하여 리턴
            foreach (string val in arr)
                newDic[val] = dic[val];

            dic.Clear();

            return newDic;
        }

        private static string GetValue(DmsStepInfo info, string names)
        {
            string value = null;

            foreach (string name in names.Split(','))
            {
                PropertyInfo prop = info.GetType().GetProperty(name);

                if (prop == null)
                    throw new Exception(String.Format("'{0}.{1}' 라는 프로퍼티를 찾을 수 없습니다.", info.GetType().Name, name));

                object obj = prop.GetValue(info, null);

                if (obj != null)
                    value += obj.ToString();
            }

            if (String.IsNullOrEmpty(value))
                value = EMPTY;

            return value;
        }

        private static Dictionary<ColRow, DefectInfo> GroupByRow(Dictionary<string, DefectInfo> dic, DPWafer[] dpWaferArr, string dmsStepInfoProperty)
        {
            Dictionary<ColRow, DefectInfo> newDic = new Dictionary<ColRow, DefectInfo>();

            foreach (var item in dic)
            {
                string parent = item.Key;
                DefectList defectList = item.Value.DefectList;

                Dictionary<string, DefectInfo> subDic = GroupByColumn(defectList, dpWaferArr, dmsStepInfoProperty);

                foreach (var subItem in subDic)
                    newDic.Add(new ColRow(parent, subItem.Key), subItem.Value);
            }

            return newDic;
        }

        private static Dictionary<ColRow, DefectInfo> GroupByRow_DefectClass(Dictionary<string, DefectInfo> dic, string defectMember)
        {
            Dictionary<ColRow, DefectInfo> newDic = new Dictionary<ColRow, DefectInfo>();

            foreach (var item in dic)
            {
                string parent = item.Key;
                DefectList defectList = item.Value.DefectList;

                Dictionary<string, DefectInfo> subDic = new Dictionary<string, DefectInfo>();

                foreach (Defect defect in defectList)
                {
                    FieldInfo field = defect.GetType().GetField(defectMember);

                    object obj = field.GetValue(defect);
                    string val;

                    if (obj == null)
                        val = EMPTY;
                    else
                        val = obj.ToString();

                    if (!subDic.ContainsKey(val))
                        subDic.Add(val, new DefectInfo(defect.STEP_SEQ));

                    subDic[val].DefectList.Add(defect);
                }

                foreach (var subItem in subDic)
                    newDic.Add(new ColRow(parent, subItem.Key), subItem.Value);
            }

            return newDic;
        }

        private static Dictionary<ColRow, DefectInfo> GroupByRow_None(Dictionary<string, DefectInfo> dic)
        {
            Dictionary<ColRow, DefectInfo> newDic = new Dictionary<ColRow, DefectInfo>();

            foreach (var item in dic)
            {
                newDic.Add(new ColRow(item.Key, EMPTY), item.Value);
            }

            return newDic;
        }

        public class ColRow
        {
            public ColRow(string col, string row, long stepSeq = 0)
            {
                Col = col;
                Row = row;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is ColRow))
                    return false;

                ColRow key = obj as ColRow;

                return Col == key.Col && Row == key.Row;
            }

            public static bool operator ==(ColRow a, ColRow b)
            {
                return a.Equals(b);
            }

            public static bool operator !=(ColRow a, ColRow b)
            {
                return !a.Equals(b);
            }

            internal void ChangeCol(string newCol)
            {
                Col = newCol;
            }

            public string Col { get; private set; }
            public string Row { get; private set; }
            public long StepSeq { get; private set; }
        }
    }

    /// <summary>
    /// Defect Map Gallery, Die Stack Gallery의 Column 항목을 나타냅니다.
    /// </summary>
    public class GalleryColumnItem
    {
        private GalleryColumnItem(string value)
        {
            Value = value;
        }

        public static GalleryColumnItem[] GetItems()
        {
            return new GalleryColumnItem[]
            {
                GalleryColumnItem.None,
                GalleryColumnItem.LotID,
                GalleryColumnItem.WaferID,
                GalleryColumnItem.StepID,
                GalleryColumnItem.DeviceID,
                GalleryColumnItem.Inspector,
                GalleryColumnItem.ResultTime
            };
        }

        public static string[] ToArray()
        {
            GalleryColumnItem[] items = GetItems();
            string[] arr = new string[items.Length];

            for (int i = 0; i < items.Length; i++)
                arr[i] = items[i].Value;

            return arr;
        }

        public static GalleryColumnItem Parse(string text)
        {
            foreach (GalleryColumnItem item in GetItems())
            {
                if (item.Value == text)
                    return item;
            }

            return GalleryColumnItem.None;
        }

        public static readonly GalleryColumnItem None = new GalleryColumnItem("None");
        public static readonly GalleryColumnItem LotID = new GalleryColumnItem("Lot ID");
        public static readonly GalleryColumnItem WaferID = new GalleryColumnItem("Wafer ID");
        public static readonly GalleryColumnItem StepID = new GalleryColumnItem("Step ID");
        public static readonly GalleryColumnItem DeviceID = new GalleryColumnItem("Device ID");
        public static readonly GalleryColumnItem Inspector = new GalleryColumnItem("Inspector");
        public static readonly GalleryColumnItem ResultTime = new GalleryColumnItem("Result Time");

        public string Value { get; set; }
    }

    /// <summary>
    /// Defect Map Gallery, Die Stack Gallery의 Row 항목을 나타냅니다.
    /// </summary>
    public class GalleryRowItem
    {
        private GalleryRowItem(string value)
        {
            Value = value;
        }

        public static GalleryRowItem[] GetItems()
        {
            return new GalleryRowItem[]
            {
                GalleryRowItem.None,
                GalleryRowItem.LotID,
                GalleryRowItem.WaferID,
                GalleryRowItem.StepID,
                GalleryRowItem.DeviceID,
                GalleryRowItem.Inspector,
                GalleryRowItem.ResultTime,
                GalleryRowItem.DefectClass
            };
        }

        public static string[] ToArray()
        {
            GalleryRowItem[] items = GetItems();
            string[] arr = new string[items.Length];

            for (int i = 0; i < items.Length; i++)
                arr[i] = items[i].Value;

            return arr;
        }

        public static GalleryRowItem Parse(string text)
        {
            foreach (GalleryRowItem item in GetItems())
            {
                if (item.Value == text)
                    return item;
            }

            return GalleryRowItem.None;
        }

        public static readonly GalleryRowItem None = new GalleryRowItem("None");
        public static readonly GalleryRowItem LotID = new GalleryRowItem("Lot ID");
        public static readonly GalleryRowItem WaferID = new GalleryRowItem("Wafer ID");
        public static readonly GalleryRowItem StepID = new GalleryRowItem("Step ID");
        public static readonly GalleryRowItem DeviceID = new GalleryRowItem("Device ID");
        public static readonly GalleryRowItem Inspector = new GalleryRowItem("Inspector");
        public static readonly GalleryRowItem ResultTime = new GalleryRowItem("Result Time");
        public static readonly GalleryRowItem DefectClass = new GalleryRowItem("Defect Class");

        public string Value { get; set; }
    }

    public class DefectInfo
    {
        public DefectInfo(long stepSeq)
        {
            StepSeq = stepSeq;
            DefectList = new DefectList();
        }

        public long StepSeq { get; private set; }
        public DefectList DefectList { get; private set; }
    }
}
