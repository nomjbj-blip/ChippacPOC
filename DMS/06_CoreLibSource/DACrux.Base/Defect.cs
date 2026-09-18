using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.Base
{
    [Serializable]
    public class Defect : IComparable, ICloneable
    {
        public Defect()
        {
            Images = new DefectImageList();
            Visible = true;
        }

        public long STEP_SEQ;
        public int DEFECTID;
        public int WAFER_SEQ;
        public double X;
        public double Y;
        public double XREL;
        public double YREL;
        public int XINDEX;
        public int YINDEX;
        public double XSIZE;
        public double YSIZE;
        public double DEFECTAREA;
        public double DSIZE;
        public int CLASSNUMBER;
        public int TEST;
        public int CLUSTERNUMBER;
        public int ROUGHBINNUMBER;
        public int FINEBINNUMBER;
        public int REVIEWSAMPLE;
        public int IMAGECOUNT;
        public int IMAGELIST;
        public int ADDER;
        public string FIRST_STEP;
        public int RETICLE_REPEAT_ID;
        public int DIE_REPEAT_ID;
        public int MAN_OPT_CLASS;
        public int AUTO_OPT_CLASS;
        public int MAN_SEM_CLASS;
        public int AUTO_SEM_CLASS;
        public int REPEAT_XREL;
        public int REPEAT_YREL;
        public int RD;
        public string DSA;

        public string DEFECT_TYPE;
        public bool EmptyShape;
        public string IMAGEURL;
        public double PEAK;
        public double MEAN_GRAY;
        public DefectImageList Images { get; private set; }

        public bool Visible { get; set; }
        public bool IncludeInZone { get; set; }

        public override string ToString()
        {
            return String.Format("ID:{0}, X:{1}, Y:{2}, ImageCount:{3}", DEFECTID, XINDEX, YINDEX, IMAGECOUNT);
        }

        public int CompareTo(object obj)
        {
            Defect d = obj as Defect;

            int val = STEP_SEQ.CompareTo(d.STEP_SEQ);

            if (val == 0)
                return DEFECTID.CompareTo(d.DEFECTID);
            else
                return val;
        }

        public object Clone()
        {
            Defect defect = new Defect();
            defect.STEP_SEQ = STEP_SEQ;
            defect.DEFECTID = DEFECTID;
            defect.WAFER_SEQ = WAFER_SEQ;
            defect.X = X;
            defect.Y = Y;
            defect.XREL = XREL;
            defect.YREL = YREL;
            defect.XINDEX = XINDEX;
            defect.YINDEX = YINDEX;
            defect.XSIZE = XSIZE;
            defect.YSIZE = YSIZE;
            defect.DEFECTAREA = DEFECTAREA;
            defect.DSIZE = DSIZE;
            defect.CLASSNUMBER = CLASSNUMBER;
            defect.TEST = TEST;
            defect.CLUSTERNUMBER = CLUSTERNUMBER;
            defect.ROUGHBINNUMBER = ROUGHBINNUMBER;
            defect.FINEBINNUMBER = FINEBINNUMBER;
            defect.REVIEWSAMPLE = REVIEWSAMPLE;
            defect.IMAGECOUNT = IMAGECOUNT;
            defect.IMAGELIST = IMAGELIST;
            defect.ADDER = ADDER;
            defect.FIRST_STEP = FIRST_STEP;
            defect.RETICLE_REPEAT_ID = RETICLE_REPEAT_ID;
            defect.DIE_REPEAT_ID = DIE_REPEAT_ID;
            defect.MAN_OPT_CLASS = MAN_OPT_CLASS;
            defect.AUTO_OPT_CLASS = AUTO_OPT_CLASS;
            defect.MAN_SEM_CLASS = MAN_SEM_CLASS;
            defect.AUTO_SEM_CLASS = AUTO_SEM_CLASS;
            defect.REPEAT_XREL = REPEAT_XREL;
            defect.REPEAT_YREL = REPEAT_YREL;
            defect.DSA = DSA;
            defect.RD = RD;

            defect.DEFECT_TYPE = DEFECT_TYPE;
            defect.EmptyShape = EmptyShape;
            defect.IMAGEURL = IMAGEURL;
            defect.PEAK = PEAK;
            defect.MEAN_GRAY = MEAN_GRAY;

            foreach (DefectImage image in Images)
            {
                DefectImage newImg = new DefectImage();
                newImg.IMAGESEQ = image.IMAGESEQ;
                newImg.IMAGEPATH = image.IMAGEPATH;
                defect.Images.Add(newImg);
            }

            return defect;
        }
    }

    public class DefectList : List<Defect>
    {
        public const string DEFECT_TYPE_NEW = "NEW";
        public const string DEFECT_TYPE_RANDOM = "RANDOM";
        public const string DEFECT_TYPE_CLUSTER = "CLUSTER";

        public event EventHandler CountChanged;

        protected virtual void OnCountChanged(EventArgs e)
        {
            if (CountChanged != null)
                CountChanged(this, e);
        }

        /*
        public new void Add(Defect defect)
        {
            // 이미 해당 DEFECTID 데이터가 존재하는 경우 이미지 COUNT와 이미지리스트 업데이트 2019.09.25 Taihi,Kim.
            Defect oldDefect = GetDefect(defect.STEP_SEQ, defect.DEFECTID);

            if (oldDefect == null)
            {
                base.Add(defect);
            }
            else
            {
                oldDefect.IMAGECOUNT += defect.IMAGECOUNT;
                oldDefect.IMAGELIST += defect.IMAGELIST;
                oldDefect.Images.AddRange(defect.Images);
            }

            OnCountChanged(EventArgs.Empty);
        }
         */

        public new bool Remove(Defect defect)
        {
            try
            {
                return base.Remove(defect);
            }
            finally
            {
                OnCountChanged(EventArgs.Empty);
            }
        }

        public int GetRDCount()
        {
            int count = 0;

            foreach (Defect d in this)
                count += d.RD;

            return count;
        }

        public Defect GetDefect(int defectid)
        {
            foreach (Defect defect in this)
            {
                if (defect.DEFECTID == defectid)
                    return defect;
            }

            return null;
        }

        public Defect[] GetDefectInDie(int xIndex, int yIndex)
        {
            List<Defect> list = new List<Defect>();

            foreach (Defect defect in this)
            {
                if (defect.XINDEX == xIndex && defect.YINDEX == yIndex)
                    list.Add(defect);
            }

            return list.ToArray();
        }

        public Defect GetDefect(long stepSeq, int defectid)
        {
            foreach (Defect defect in this)
            {
                if (defect.STEP_SEQ == stepSeq && defect.DEFECTID == defectid)
                    return defect;
            }

            return null;
        }

        public long[] GetStepSeqArray()
        {
            List<long> list = new List<long>();

            foreach (Defect defect in this)
            {
                if (!list.Contains(defect.STEP_SEQ))
                    list.Add(defect.STEP_SEQ);
            }

            return list.ToArray();
        }

        public Defect[] GetDefectArray(long stepSeq)
        {
            List<Defect> list = new List<Defect>();

            foreach (Defect defect in this)
            {
                if (defect.STEP_SEQ == stepSeq)
                    list.Add(defect);
            }

            return list.ToArray();
        }

        public DefectStat GetDefectStat()
        {
            DefectStat stat = new DefectStat();

            List<int> groupList = new List<int>();

            foreach (Defect defect in this)
            {
                if (defect.ADDER > 0)
                {
                    stat.New++;
                    stat.AreaNew += defect.DEFECTAREA;
                }

                if (defect.CLUSTERNUMBER == 0)
                {
                    stat.Random++;
                    stat.AreaRandom += defect.DEFECTAREA;

                    if (defect.ADDER > 0)
                    {
                        stat.NewRandom++;
                        stat.AreaNewRandom += defect.DEFECTAREA;
                    }
                }
                else // CLUSTER
                {
                    stat.Cluster++;
                    stat.AreaCluster += defect.DEFECTAREA;

                    if (defect.ADDER > 0)
                    {
                        stat.NewCluster++;
                        stat.AreaNewCluster += defect.DEFECTAREA;
                    }

                    int idx = groupList.BinarySearch(defect.CLUSTERNUMBER);

                    if (idx < 0)
                        groupList.Insert(~idx, defect.CLUSTERNUMBER);
                }
            }

            stat.ClusterGroup = groupList.Count;

            return stat;
        }

        public new void Clear()
        {
            base.Clear();
            OnCountChanged(EventArgs.Empty);
        }

        /// <summary>
        /// 해당 멤버값을 GroupBy한 Count를 가져옵니다.
        /// </summary>
        public Dictionary<T, int> GroupByCount<T>(string name, bool allowSort = false, bool clusterGroupAsOneDefect = false)
        {
            List<T> list = new List<T>();
            Dictionary<T, int> dic = new Dictionary<T, int>();
            System.Reflection.FieldInfo field = null;
            List<string> clusterList = new List<string>();

            foreach (var defect in this)
            {
                if (field == null)
                {
                    field = defect.GetType().GetField(name);

                    if (field == null)
                        throw new Exception(String.Format("멤버('{0}')를 찾을 수 없습니다.", name));
                }

                T val = (T)field.GetValue(defect);

                if (val == null)
                   continue;

                if (clusterGroupAsOneDefect && defect.CLUSTERNUMBER > 0)
                {
                    string str = String.Format("{0}_{1}_{2}", defect.STEP_SEQ, val, defect.CLUSTERNUMBER);

                    int clusterIdx = clusterList.BinarySearch(str);

                    if (clusterIdx >= 0)
                        continue;
                    else
                        clusterList.Insert(~clusterIdx, str);
                }

                int idx = list.BinarySearch(val);

                if (idx < 0)
                {
                    list.Insert(~idx, val);
                    dic.Add(val, 0);
                }

                dic[val]++;
            }

            if (allowSort)
            {
                list.Sort();

                Dictionary<T, int> newDic = new Dictionary<T, int>();

                foreach (T item in list)
                    newDic.Add(item, dic[item]);

                dic.Clear();
                dic = newDic;
            }

            return dic;
        }

        /// <summary>
        /// 각 Type별로 GroupBy한 Count를 가져옵니다.
        /// (Type: All, New, Random, Cluster)
        /// </summary>
        public Dictionary<string, int> GroupByType()
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            dic.Add(DEFECT_TYPE_NEW, 0);
            dic.Add(DEFECT_TYPE_RANDOM, 0);
            dic.Add(DEFECT_TYPE_CLUSTER, 0);

            foreach (var defect in this)
            {
                if (defect.ADDER > 0)
                    dic[DEFECT_TYPE_NEW]++;

                if (defect.CLUSTERNUMBER == 0)
                    dic[DEFECT_TYPE_RANDOM]++;
                else
                    dic[DEFECT_TYPE_CLUSTER]++;
            }

            return dic;
        }

        public Dictionary<Tuple<double, double>, int> GroupBySizeCount(List<Tuple<double, double>> fromToList)
        {
            Dictionary<Tuple<double, double>, int> dic = new Dictionary<Tuple<double,double>,int>();

            foreach (var fromTo in fromToList)
                dic.Add(fromTo, 0);

            foreach (Defect defect in this)
            {
                foreach (var fromTo in fromToList)
                {
                    if (fromTo.Item1 <= defect.DSIZE && fromTo.Item2 >= defect.DSIZE)
                    {
                        dic[fromTo]++;
                        break;
                    }
                }
            }

            return dic;
        }

        public List<Defect> FindByTest(int testNo)
        {
            List<Defect> list = new List<Defect>();

            foreach (Defect defect in this)
            {
                if (defect.TEST == testNo)
                    list.Add(defect);
            }

            return list;
        }

        public int BinarySearch(long stepSeq, int defectid)
        {
            Defect defect = new Defect();
            defect.STEP_SEQ = stepSeq;
            defect.DEFECTID = defectid;
            return base.BinarySearch(defect);
        }

        public void SetVisibleAll(bool visible)
        {
            foreach (Defect defect in this)
                defect.Visible = visible;
        }

        /// <summary>
        /// 해당 값의 Defect를 보여지도록 합니다.
        /// </summary>
        public void SetVisible<T>(string name, List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                SetVisibleAll(true);
                return;
            }

            list.Sort();

            System.Reflection.FieldInfo field = null;

            foreach (var defect in this)
            {
                if (field == null)
                {
                    field = defect.GetType().GetField(name);

                    if (field == null)
                        throw new Exception(String.Format("멤버('{0}')를 찾을 수 없습니다.", name));
                }

                T val = (T)field.GetValue(defect);

                if (val == null)
                    continue;

                defect.Visible = list.BinarySearch(val) >= 0;
            }
        }

        public void SetVisibleType(DefectType defectType)
        {
            if (defectType == DefectType.None)
            {
                SetVisibleAll(false);
                return;
            }
            else if (defectType == DefectType.All)
            {
                SetVisibleAll(true);
                return;
            }

            foreach (var defect in this)
            {
                defect.Visible = false;

                if ((defectType & DefectType.New) == DefectType.New && defect.ADDER > 0)
                    defect.Visible = true;

                if ((defectType & DefectType.Random) == DefectType.Random && defect.CLUSTERNUMBER == 0)
                    defect.Visible = true;

                if ((defectType & DefectType.Cluster) == DefectType.Cluster && defect.CLUSTERNUMBER > 0)
                    defect.Visible = true;
            }
        }

        /// <summary>
        /// 해당 값의 Defect를 보여지도록 합니다.
        /// </summary>
        public void SetVisibleSize(List<Tuple<double, double>> values)
        {
            if (values == null || values.Count == 0)
            {
                SetVisibleAll(true);
                return;
            }

            foreach (var defect in this)
            {
                defect.Visible = false;

                foreach (var item in values)
                {
                    if (item.Item1 <= defect.DSIZE && item.Item2 >= defect.DSIZE)
                    {
                        defect.Visible = true;
                        break;
                    }
                }
            }
        }

        public int[] GetAllClassNumber()
        {
            List<int> list = new List<int>();

            foreach (Defect defect in this)
            {
                int idx = list.BinarySearch(defect.CLASSNUMBER);

                if (idx < 0)
                    list.Insert(~idx, defect.CLASSNUMBER);
            }

            return list.ToArray();
        }

        public DataTable ToDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("STEP_SEQ", typeof(string));
            dt.Columns.Add("DEFECTID", typeof(decimal));
            dt.Columns.Add("WAFER_SEQ", typeof(string));
            dt.Columns.Add("X", typeof(decimal));
            dt.Columns.Add("Y", typeof(decimal));
            dt.Columns.Add("XREL", typeof(decimal));
            dt.Columns.Add("YREL", typeof(decimal));
            dt.Columns.Add("XINDEX", typeof(decimal));
            dt.Columns.Add("YINDEX", typeof(decimal));
            dt.Columns.Add("XSIZE", typeof(decimal));
            dt.Columns.Add("YSIZE", typeof(decimal));
            dt.Columns.Add("DSIZE", typeof(decimal));
            dt.Columns.Add("CLASSNUMBER", typeof(decimal));
            dt.Columns.Add("TEST", typeof(decimal));
            dt.Columns.Add("CLUSTERNUMBER", typeof(decimal));
            dt.Columns.Add("IMAGECOUNT", typeof(int));
            dt.Columns.Add("NEW", typeof(int));

            foreach (Defect defect in this)
            {
                dt.Rows.Add(
                    defect.STEP_SEQ,
                    defect.DEFECTID,
                    defect.WAFER_SEQ,
                    defect.X,
                    defect.Y,
                    defect.XREL,
                    defect.YREL,
                    defect.XINDEX,
                    defect.YINDEX,
                    defect.XSIZE,
                    defect.YSIZE,
                    defect.DSIZE,
                    defect.CLASSNUMBER,
                    defect.TEST,
                    defect.CLUSTERNUMBER,
                    defect.IMAGECOUNT,
                    defect.ADDER
                    );
            }

            return dt;
        }
    }

    [Flags]
    public enum DefectType
    {
        None     = 0x0000,
        New      = 0x0001,
        Random   = 0x0002,
        Cluster  = 0x0004,
        All      = 0x0008
    }

    public class DefectImageList : List<DefectImage>
    {
        public void Add(int imageSequence, string imageFileName)
        {
            Add(new DefectImage() { IMAGESEQ = imageSequence, IMAGEPATH = imageFileName });
        }

        public void Add(string imageFileName)
        {
            Add(Count, imageFileName);
        }

        public void Add(int imageSequence)
        {
            Add(imageSequence, null);
        }

        public DefectImage GetDefectImage(int index)
        {
            if (Count <= index)
                return null;

            return base[index];
        }

        public new string this[int index]
        {
            get { return base[index].IMAGEPATH; }
            set { base[index].IMAGEPATH = value; }
        }
    }

    public class DefectImage
    {
        public int IMAGESEQ;
        public string IMAGEPATH;
    }

    public class DefectStat
    {
        public int New;
        public int Random;
        public int Cluster;
        public int ClusterGroup;
        public int NewRandom;
        public int NewCluster;
        public double AreaNew;
        public double AreaRandom;
        public double AreaCluster;
        public double AreaNewRandom;
        public double AreaNewCluster;
    }
}
