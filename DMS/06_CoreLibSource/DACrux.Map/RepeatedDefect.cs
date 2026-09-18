//#define RECTANGLE_DOUBLE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DACrux.Base;

namespace DACrux.Map
{
    // 2019.09.27 Taihi,Kim.
    public class RepeatedDefect
    {
        private Point[] _dies;
        private DefectList _defectList;

        public RepeatedDefect()
        {
            ShotList = new List<ShotPanel>();
        }

        public void Calculate(Point[] dies, DefectList defectList)
        {
            if (defectList == null || defectList.Count == 0)
                return;

            if (dies == null || dies.Length == 0)
                throw new Exception("Dies 값이 없는 경우 계산할 수 없습니다.");

            _dies = dies;
            _defectList = defectList;

            if (DieXSize == 0 || DieYSize == 0)
                throw new Exception("Die Size 값이 없는 경우 계산할 수 없습니다.");

            if (Tolerance == 0)
                throw new Exception("Tolerance 값이 없는 경우 계산할 수 없습니다.");
            
            if (RepeatCount == 0)
                throw new Exception("RepeatCount 값이 없는 경우 계산할 수 없습니다.");

            if (ShotArrayX == 0 || ShotArrayY == 0)
                throw new Exception("ShotArra 값이 없는 경우 계산할 수 없습니다.");

            AppendShot();
            CalculateRD();
        }

        private void AppendShot()
        {
            ShotList.Clear();

            int xStart, yStart, xEnd, yEnd;
            DACrux.Map.DefectMap.CalculateShotStartEndIndex(_dies, ShotStartX, ShotStartY, ShotArrayX, ShotArrayY, out xStart, out yStart, out xEnd, out yEnd);

            for (int x = xStart; x <= xEnd; x += ShotArrayX)
            {
                int x2 = x + ShotArrayX;

                for (int y = yStart; y <= yEnd; y += ShotArrayY)
                {
                    int y2 = y + ShotArrayY;
                    ShotPanel panel = new ShotPanel(x, y, x + ShotArrayX, y + ShotArrayY);

                    foreach (Defect defect in _defectList)
                    {
                        if (defect.XINDEX >= x && defect.XINDEX < x2 && defect.YINDEX >= y && defect.YINDEX < y2)
                        {
                            double xrel = defect.XREL + (defect.XINDEX - x) * DieXSize;
                            double yrel = defect.YREL + (defect.YINDEX - y) * DieYSize;

#if RECTANGLE_DOUBLE
                                    RectangleD rect = new RectangleD(
                                        (xrel - Tolerance),
                                        (yrel - Tolerance),
                                        (2 * Tolerance + (CalcBy == Map.CalcBy.Size ? defect.XSIZE : 0)),
                                        (2 * Tolerance + (CalcBy == Map.CalcBy.Size ? defect.YSIZE : 0)));
#else
                            RectangleF rect = new RectangleF(
                                (float)(xrel - Tolerance),
                                (float)(yrel - Tolerance),
                                (float)(2 * Tolerance + (CalcBy == Map.CalcBy.Size ? defect.XSIZE : 0)),
                                (float)(2 * Tolerance + (CalcBy == Map.CalcBy.Size ? defect.YSIZE : 0)));
#endif
                            panel.AllItem.Add(new RDItem((float)xrel, (float)yrel, rect, defect));
                        }
                    }

                    // Defect이 있는 경우만 추가
                    if (panel.AllItem.Count > 0)
                        ShotList.Add(panel);
                }
            }
        }

        private void CalculateRD()
        {
            // ShotPanel을 순환시키면서 체크
            for (int i = 0; i < ShotList.Count; i++)
            {
                // 비교 기준 Panel
                ShotPanel pnlA = ShotList[0];

                foreach (RDItem itemA in pnlA.AllItem)
                {
                    int panelCount = 1;
                    List<RDItem>[] tmpArr = new List<RDItem>[ShotList.Count];

                    // Panel 전체 순환
                    for (int j = 1; j < ShotList.Count; j++)
                    {
                        ShotPanel pnlB = ShotList[j];
                        tmpArr[j] = new List<RDItem>();

                        bool rdFound = false;

                        foreach (RDItem itemB in pnlB.AllItem)
                        {
                            if (itemA.IntersectsWith(itemB))
                            {
                                tmpArr[j].Add(itemB);
                                rdFound = true;
                            }
                        }

                        if (rdFound)
                            panelCount++;
                    }

                    // RD 조건을 만족하는 경우
                    if (panelCount >= RepeatCount)
                    {
                        pnlA.RDItem.Add(itemA);

                        for (int k = 1; k < ShotList.Count; k++)
                        {
                            if (tmpArr[k] != null && tmpArr[k].Count > 0)
                                ShotList[k].RDItem.AddRange(tmpArr[k]);
                        }
                    }
                }

                // 맨 앞의 Panel이 맨뒤로 위치하도록 한다.
                ShotList.RemoveAt(0);
                ShotList.Add(pnlA);
            }

            // RD 인 경우 Defect에 값 설정
            foreach (ShotPanel panel in ShotList)
            {
                foreach (RDItem item in panel.AllItem)
                    item.Defect.RD = 0;

                foreach (RDItem item in panel.RDItem)
                    item.Defect.RD = 1;
            }
        }

        /// <summary>Die의 X 사이즈</summary>
        public double DieXSize { get; set; }
        /// <summary>Die의 Y 사이즈</summary>
        public double DieYSize { get; set; }

        /// <summary>계산 시 사용되는 허용 오차값</summary>
        public double Tolerance {get; set; }
        /// <summary>RD로 판정할 반복 개수</summary>
        public int RepeatCount {get; set; }
        /// <summary></summary>
        public CalcBy CalcBy { get; set; }

        /// <summary>Shot 시작 DIE 의 X 인덱스</summary>
        public int ShotStartX { get; set; }
        /// <summary>Shot 시작 DIE 의 Y 인덱스</summary>
        public int ShotStartY { get; set; }

        /// <summary>Shot 의 X 방향 DIE 개수</summary>
        public int ShotArrayX { get; set; }
        /// <summary>Shot 의 Y 방향 DIE 개수</summary>
        public int ShotArrayY { get; set; }

        public List<ShotPanel> ShotList
        {
            get;
            private set;
        }
    }

    public enum CalcBy
    {
        Point,
        Size
    }

#if RECTANGLE_DOUBLE
    /// <summary>
    /// Repeated Defect 항목을 표현합니다.
    /// </summary>
    public class RDItem
    {
        public RDItem(float x, float y, RectangleD rect, Defect defect)
        {
            X = x;
            Y = y;
            Rect = rect;
            Defect = defect;
        }

        // 영역을 표현
        public float X;
        public float Y;
        public RectangleD Rect;
        // 연결된 Defect
        public Defect Defect;

        internal bool IntersectsWith(RDItem item)
        {
            if (Rect.X > item.X || (Rect.X + Rect.Width) < item.X || Rect.Y > item.Y || (Rect.Y + Rect.Height) < item.Y)
                return false;

            return true;
        }
    }
#else
    /// <summary>
    /// Repeated Defect 항목을 표현합니다.
    /// </summary>
    public class RDItem
    {
        public RDItem(float x, float y, RectangleF rect, Defect defect)
        {
            X = x;
            Y = y;
            Rect = rect;
            Defect = defect;
        }

        // 영역을 표현
        public float X;
        public float Y;
        public RectangleF Rect;
        // 연결된 Defect
        public Defect Defect;
        
        internal bool IntersectsWith(RDItem item)
        {
            if (Rect.Left > item.X || Rect.Right < item.X || Rect.Top > item.Y || Rect.Bottom < item.Y)
                return false;

            return true;
        }
    }
#endif

        // Shot 영역에 대한 표현
    public class ShotPanel
    {
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="xStart">시작 DIE의 X 인덱스</param>
        /// <param name="yStart">시작 DIE의 Y 인덱스</param>
        /// <param name="xEnd">끝 DIE의 X 인덱스</param>
        /// <param name="yEnd">끝 DIE의 Y 인덱스</param>
        public ShotPanel(int xStart, int yStart, int xEnd, int yEnd)
        {
            AllItem = new List<RDItem>();
            RDItem = new List<RDItem>();

            // 이 값들은 특별한 용도로 사용되지 않으며 디버깅 시 단지 좌표값 확인 용도로 사용됩니다.
            XStart = xStart;
            YStart = yStart;
            XEnd = xEnd;
            YEnd = yEnd;
        }

        public override string ToString()
        {
            return String.Format("ALL:{0}, RD:{1}, X:{2}~{3}, Y:{4}~{5}", AllItem.Count, RDItem.Count, XStart, XEnd, YStart, YEnd);
        }

        public List<RDItem> AllItem { get; private set; }
        public List<RDItem> RDItem { get; private set; }

        public int XStart { get; private set; }
        public int YStart { get; private set; }
        public int XEnd { get; private set; }
        public int YEnd { get; private set; }
    }
}
