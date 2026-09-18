using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DACrux.Base.Zone
{
    /// <summary>
    /// Zone의 도넛 방향의 크기 모드를 나타냅니다.
    /// </summary>
    public enum ZoneAnnularSizeMode
    {
        SameRadius,
        SameArea
    }

    /// <summary>
    /// Zone 기준점을 나타냅니다. 기본값은 Down 입니다.
    /// </summary>
    public enum ZoneOrientation
    {
        Down,
        Up,
        Left,
        Right
    }

    /// <summary>
    /// Zone 설정값을 나타냅니다.
    /// </summary>
    public class ZoneConfig //: ICloneable
    {
        public event EventHandler ZoneItemListChanged;

        public static readonly int DEFAULT_WAFER_SIZE = 200 * 1000;

        public ZoneConfig()
        {
            ZoneItemList = new Zone.ZoneItemList();
        }

        protected virtual void OnZoneItemListChanged(EventArgs e)
        {
            if (ZoneItemListChanged != null)
                ZoneItemListChanged(this, e);
        }

        public void Read()
        {
            SettingData setting = new SettingData(typeof(ZoneConfig));
            Orientation = setting.GetValue<ZoneOrientation>("Orientation", ZoneOrientation.Down);
            AnnularSizeMode = setting.GetValue<ZoneAnnularSizeMode>("AnnularSizeMode", ZoneAnnularSizeMode.SameRadius);
            AnnularZoneCount = setting.GetValue<int>("AnnularZoneCount", 4);
            RadialZoneCount = setting.GetValue<int>("RadialZoneCount", 6);
            Angle = setting.GetValue<int>("Angle", 0);
            WaferEdgeSize = setting.GetValue<int>("WaferEdgeSize", 0);
        }
        
        public void Save()
        {
            SettingData setting = new SettingData(typeof(ZoneConfig));
            setting.SetValue("Orientation", Orientation);
            setting.SetValue("AnnularSizeMode", AnnularSizeMode);
            setting.SetValue("AnnularZoneCount", AnnularZoneCount);
            setting.SetValue("RadialZoneCount", RadialZoneCount);
            setting.SetValue("Angle", Angle);
            setting.SetValue("WaferEdgeSize", WaferEdgeSize);
            setting.Save();
        }

        //public object Clone()
        //{
        //    ZoneConfig obj = new ZoneConfig();
        //    obj.Orientation = Orientation;
        //    obj.AnnularSizeMode = AnnularSizeMode;
        //    obj.AnnularZoneCount = AnnularZoneCount;
        //    obj.RadialZoneCount = RadialZoneCount;
        //    obj.Angle = Angle;
        //    obj.ZoneItemList = ZoneItemList;
        //    return obj;
        //}

        public ZoneOrientation Orientation
        {
            get;
            set;
        }

        public ZoneAnnularSizeMode AnnularSizeMode
        {
            get;
            set;
        }

        public int AnnularZoneCount
        {
            get;
            set;
        }

        public int RadialZoneCount
        {
            get;
            set;
        }

        public float Angle
        {
            get;
            set;
        }

        public ZoneItemList ZoneItemList
        {
            get;
            private set;
        }

        /// <summary>
        /// Wafer의 가장자리 폭을 나타냅니다.
        /// </summary>
        public float WaferEdgeSize
        {
            get;
            set;
        }

        /// <summary>
        /// 입력한 값 유효성을 검사합니다.
        /// </summary>
        /// <returns></returns>
        public bool CheckValid()
        {
            if (AnnularZoneCount <= 0)
                throw new Exception("AnnularZoneCount 값이 없습니다.");

            if (RadialZoneCount <= 0)
                throw new Exception("RadialZoneCount 값이 없습니다.");

            if (Angle < 0)
                throw new Exception("Angle 값은 0 보다 커야 합니다.");

            if (WaferEdgeSize < 0)
                throw new Exception("WaferEdgeSize 값은 0 보다 커야 합니다.");

            return true;
        }

        /// <summary>
        /// 설정된 값에 따라 ZoneItem 항목들을 계산합니다.
        /// </summary>
        public void CalculateZoneItem()
        {
            CheckValid();

            ZoneItemList.Clear();
            double[] annuArr = new double[AnnularZoneCount + 1];

            // 도넛 방향으로 너비 계산
            if (AnnularSizeMode == ZoneAnnularSizeMode.SameRadius)
            {
                double width = 1d / AnnularZoneCount;

                for (int i = 1; i < annuArr.Length; i++)
                    annuArr[i] = width * i;
            }
            else if (AnnularSizeMode == ZoneAnnularSizeMode.SameArea)
            {
                double unitArea = Math.PI / AnnularZoneCount;

                for (int i = 1; i < annuArr.Length; i++)
                    annuArr[i] = Math.Sqrt(i * unitArea / Math.PI);
            }

            // ZoneItem 추가
            for (int a = 0; a < AnnularZoneCount; a++)
            {
                double unitAngle = 360f / RadialZoneCount;

                for (int r = 0; r < RadialZoneCount; r++)
                {
                    ZoneItem item = new ZoneItem(a, r);
                    item.Zone = this;
                    
                    item.StartAngle = (float)(unitAngle * r + Angle);
                    item.EndAngle = (float)(item.StartAngle + unitAngle);

                    item.StartAngle = (GetOrientationAngle() + item.StartAngle);
                    item.EndAngle = (GetOrientationAngle() + item.EndAngle);

                    item.StartRadius = (float)annuArr[a];
                    item.EndRadius = (float)annuArr[a + 1];

                    ZoneItemList.Add(item);
                }
            }

            OnZoneItemListChanged(EventArgs.Empty);
        }

        private float GetOrientationAngle()
        {
            switch (Orientation)
            {
                case ZoneOrientation.Down: return 0;
                case ZoneOrientation.Up: return 180;
                case ZoneOrientation.Left: return 90;
                case ZoneOrientation.Right: return 270;
                default: return 0;
            }
        }
    }

    /// <summary>
    /// Zone 항목을 나타냅니다.
    /// </summary>
    public class ZoneItem : IComparable<ZoneItem>, IComparer<ZoneItem>
    {
        public ZoneItem(int annularIndex, int radialIndex)
        {
            ZoneID = String.Format("{0}:{1}", annularIndex + 1, radialIndex + 1);
            Visible = true;
        }

        public string ZoneID
        {
            get;
            private set;
        }

        public float StartAngle
        {
            get;
            set;
        }

        public float EndAngle
        {
            get;
            set;
        }

        public double StartRadius
        {
            get;
            set;
        }

        public double EndRadius   
        {
            get;
            set;
        }

        public PointF LabelPoint
        {
            get;
            private set;
        }

        public ZoneConfig Zone
        {
            get;
            set;
        }

        public Region Region
        {
            get;
            private set;
        }

        public bool Visible
        {
            get;
            set;
        }

        /// <summary>
        /// Zone의 영역을 가져옵니다.
        /// </summary>
        internal void SetRegion(float pointX, float pointY, float waferSize, float zoomRatio)
        {
            float radius = waferSize / 2f - Zone.WaferEdgeSize;
            radius = radius * zoomRatio;
            pointX = pointX * zoomRatio;
            pointY = pointY * zoomRatio;

            Region region = new Region();
            region.Intersect(GetCircle(pointX, pointY, (float)(radius * EndRadius)));
            region.Xor(GetCircle(pointX, pointY, (float)(radius * StartRadius)));
            region.Intersect(GetPie(pointX, pointY, radius));
            Region = region;
        }

        private GraphicsPath GetPie(float pointX, float pointY, float radius)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddPie(pointX - radius, pointY - radius, 2 * radius, 2 * radius, 90 + StartAngle, EndAngle - StartAngle);
            return path;
        }

        private GraphicsPath GetCircle(float pointX, float pointY, float radius)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(pointX - radius, pointY - radius, 2 * radius, 2 * radius);
            return path;
        }

        public DefectList GetDefect()
        {
            return null;
        }

        public int CompareTo(ZoneItem other)
        {
            return ZoneID.CompareTo(other.ZoneID);
        }

        public int Compare(ZoneItem x, ZoneItem y)
        {
            return x.CompareTo(y);
        }

        public override string ToString()
        {
            return String.Format("{0},[{1}~{2}],[{3}~{4}]", ZoneID, StartAngle, EndAngle, StartRadius, EndRadius);
        }
    }

    /// <summary>
    /// Zone 항목 리스트를 나타냅니다.
    /// </summary>
    public class ZoneItemList : List<ZoneItem>
    {
        public ZoneItem this[string zoneID]
        {
            get
            {
                foreach (ZoneItem item in this)
                {
                    if (item.ZoneID == zoneID)
                        return item;
                }

                return null;
            }
        }

        public void CalculateRegion(float pointX, float pointY, float waferSize, float zoomRatio)
        {
            foreach (ZoneItem item in this)
            {
                item.SetRegion(pointX, pointY, waferSize, zoomRatio);
            }
        }

        public string[] ToZoneIDArray()
        {
            string[] arr = new string[Count];

            for (int i = 0; i < Count; i++)
                arr[i] = this[i].ZoneID;

            return arr;
        }
    }
}
