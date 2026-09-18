using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace DACrux.Map
{
    // 2019.10.03 Taihi,Kim
    /// <summary>
    /// Die 크기를 기준으로 좌하단 (1, 1)로 시작하는 Reference Die 인덱스를 계산합니다.
    /// </summary>
    public class WaferDieCalculator
    {
#if DEBUG
        public static StringBuilder Log = new StringBuilder();
#endif
        public WaferDieCalculator()
        {
            RowArrayList = new List<int[]>();
        }

        /// <summary>
        /// 계산을 실행합니다.
        /// </summary>
        public void Calculate()
        {
            TotalDieCount = 0;
            OriginIndexX = OriginIndexY = -1;
            RowArrayList.Clear();

            double radius = WaferSize / 2 - WaferMargin;
            double w0 = -1;

            if (DiePitchX >= radius || DiePitchY >= radius)
            {
                OriginIndexX = OriginIndexY = 0;
                TotalDieCount = 1;
#if DEBUG
                Log.AppendLine(String.Format("{0}\t{1}", OriginIndexX, OriginIndexY));
#endif
                return;
            }

            //
            // 가운데에서 위로 계산
            //
            for (double height = -OriginY; height < radius - TopMargin; height += DiePitchY)
            {
                // die가 들어갈 수 있는 폭
                double w = radius * Math.Cos(Math.Asin(height / radius));

                if (w0 < 0)
                {
                    w0 = w;
                    continue;
                }

                int leftDie = 0;
                int rightDie = 0;

                // 오른쪽 폭
                double width = Math.Min(w0, w) + OriginX;

                for (double i = DiePitchX; i < width; i += DiePitchX)
                    rightDie++;

                // 왼쪽 폭
                width = Math.Min(w0, w) - OriginX;

                // 왼쪽에 들어갈 수 있는 die 개수
                for (double i = DiePitchX; i < width; i += DiePitchX)
                    leftDie++;

                // Origin Die 계산
                if (OriginIndexX < 0)
                {
                    OriginIndexX = leftDie + 1;
                }

                int[] rowArr = new int[leftDie + rightDie];

                for (int i = 0; i < leftDie; i++)
                    rowArr[i] = OriginIndexX - leftDie + i;

                for (int i = 0; i < rightDie; i++)
                    rowArr[leftDie + i] = OriginIndexX + i;

                if (rowArr.Length > 0)
                    RowArrayList.Add(rowArr);

                w0 = w;
            }

            w0 = -1;
            RowArrayList.Reverse();
            OriginIndexY = RowArrayList.Count;

            //
            // 가운데에서 아래로 계산
            //
            for (double height = -OriginY; height > -radius + NotchMargin; height -= DiePitchY)
            {
                // die가 들어갈 수 있는 폭
                double w = radius * Math.Cos(Math.Asin(height / radius));

                if (w0 < 0)
                {
                    w0 = w;
                    continue;
                }

                int leftDie = 0;
                int rightDie = 0;

                // 오른쪽 폭
                double width = Math.Min(w0, w) + OriginX;

                // 오른쪽에 들어갈 수 있는 die 개수
                for (double i = DiePitchX; i < width; i += DiePitchX)
                    rightDie++;

                // 왼쪽 폭
                width = Math.Min(w0, w) - OriginX;

                // 왼쪽에 들어갈 수 있는 die 개수
                for (double i = DiePitchX; i < width; i += DiePitchX)
                    leftDie++;

                int[] rowArr = new int[leftDie + rightDie];

                for (int i = 0; i < leftDie; i++)
                    rowArr[i] = OriginIndexX - leftDie + i;

                for (int i = 0; i < rightDie; i++)
                    rowArr[leftDie + i] = OriginIndexX + i;

                if (rowArr.Length > 0)
                    RowArrayList.Add(rowArr);

                w0 = w;
            }

            int min = Int32.MaxValue;

            foreach (int[] row in RowArrayList)
                min = Math.Min(row[0], min);

            if (min != 1)
            {
                int add = 1 - min;

                for (int y = 0; y < RowArrayList.Count; y++)
                {
                    for (int x = 0; x < RowArrayList[y].Length; x++)
                        RowArrayList[y][x] += add;
                }
            }

            foreach (int[] arr in RowArrayList)
            {
                TotalDieCount += arr.Length;

                if (arr.Length > 0)
                    IndexXMax = Math.Max(IndexXMax, arr[arr.Length - 1]);
            }

            OriginIndexY = RowArrayList.Count - OriginIndexY + 1;

            IndexXMin = IndexYMin = 1;
            IndexYMax = RowArrayList.Count;

#if DEBUG
            Log.AppendLine(String.Format("{0}\t{1}", OriginIndexX, OriginIndexY));
#endif
        }

        /// <summary>
        /// 계산된 Die x,y 인덱스에 대한 배열을 가져옵니다.
        /// </summary>
        /// <returns></returns>
        public Point[] GetDiePointArray()
        {
            if (RowArrayList == null || RowArrayList.Count == 0)
                return new Point[] { };

            List<Point> list = new List<Point>();

            for (int y = 0; y < RowArrayList.Count; y++)
            {
                for (int x = 0; x < RowArrayList[y].Length; x++)
                {
                    list.Add(new Point(RowArrayList[y][x], RowArrayList.Count - y));
                }
            }

            return list.ToArray();
        }

        /// <summary>Wafer 크기</summary>
        public double WaferSize { get; set; }

        /// <summary>Wafer 테두리 마진</summary>
        public double WaferMargin { get; set; }
        /// <summary>Wafer 상단부 마진</summary>
        public double TopMargin { get; set; }
        /// <summary>Wafer 노치부 마진</summary>
        public double NotchMargin { get; set; }

        public double DiePitchX { get; set; }
        public double DiePitchY { get; set; }
        public double OriginX { get; set; }
        public double OriginY { get; set; }
        public string Name { get; set; }

        public int OriginIndexX { get; private set; }
        public int OriginIndexY { get; private set; }
        public int TotalDieCount { get; private set; }
        public List<int[]> RowArrayList { get; private set; }
        public int IndexXMin { get; private set; }
        public int IndexYMin { get; private set; }
        public int IndexXMax { get; private set; }
        public int IndexYMax { get; private set; }
    }
}
