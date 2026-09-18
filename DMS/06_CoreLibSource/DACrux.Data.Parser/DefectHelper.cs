using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DACrux.Data.Parser.Klarf;
using DACrux.Base;
using System.Drawing;

namespace DACrux.Data.Parser
{
    public static class DefectHelper
    {
        /// <summary>
        /// 현재 Defect이 새로운 Defect인지를 계산합니다. 새로운 Defect인 경우 ADDER = 1 입니다.
        /// </summary>
        public static void CalculateNewDefect(DefectList defectList, List<PointD> prevXYList, double tolerance)
        {
            if (defectList == null || defectList.Count == 0 || prevXYList == null || prevXYList.Count == 0)
                return;

            foreach (Defect defect in defectList)
            {
                defect.ADDER = 1;

                double x1 = defect.X - tolerance;
                double x2 = defect.X + tolerance;
                double y1 = defect.Y - tolerance;
                double y2 = defect.Y + tolerance;

                foreach (PointD prevXY in prevXYList)
                {
                    if (x1 <= prevXY.X && x2 >= prevXY.X && y1 <= prevXY.Y && y2 >= prevXY.Y)
                    {
                        defect.ADDER = 0;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Defect이 Cluster Defect 인지를 계산합니다.
        /// </summary>
        /// <param name="defectList"></param>
        public static void CalculateCluster(DefectList defectList, int clusterCount, double tolerance)
        {
            if (defectList == null || defectList.Count == 0)
                return;

            List<DefectItem> allList = new List<DefectItem>();
            List<DefectItem> rdList = new List<DefectItem>();

            // ALL LIST
            foreach (Defect defect in defectList)
            {
                // CLUSTERNUMBER 초기화
                defect.CLUSTERNUMBER = 0;

                allList.Add(new DefectItem(
                    (float)defect.X,
                    (float)defect.Y,
                    new RectangleF(
                    (float)(defect.X - tolerance),
                    (float)(defect.Y - tolerance),
                    (float)(2 * tolerance),
                    (float)(2 * tolerance)),
                    defect));
            }

            int clusterNo = 1;
            Dictionary<int, List<DefectItem>> dic = new Dictionary<int, List<DefectItem>>();

            for (int i = 0; i < allList.Count; i++)
            {
                DefectItem item = allList[i];

                for (int j = i + 1; j < allList.Count; j++)
                {
                    DefectItem cmp = allList[j];

                    if (item.IntersectsWith(cmp))
                    {
                        // 둘 다 없는 경우는 새 번호 할당
                        if (item.Defect.CLUSTERNUMBER == 0 && cmp.Defect.CLUSTERNUMBER == 0)
                        {
                            item.Defect.CLUSTERNUMBER = cmp.Defect.CLUSTERNUMBER = clusterNo++;
                            Append(dic, item.Defect.CLUSTERNUMBER, item);
                            Append(dic, item.Defect.CLUSTERNUMBER, cmp);
                        }
                        // 왼쪽에만 있는 경우는 오른쪽에 같은 번호 할당
                        else if (item.Defect.CLUSTERNUMBER > 0 && cmp.Defect.CLUSTERNUMBER == 0)
                        {
                            cmp.Defect.CLUSTERNUMBER = item.Defect.CLUSTERNUMBER;
                            Append(dic, item.Defect.CLUSTERNUMBER, cmp);
                        }
                        // 오른쪽에만 있는 경우는 왼쪽에 같은 번호 할당
                        else if (item.Defect.CLUSTERNUMBER == 0 && cmp.Defect.CLUSTERNUMBER > 0)
                        {
                            item.Defect.CLUSTERNUMBER = cmp.Defect.CLUSTERNUMBER;
                            Append(dic, item.Defect.CLUSTERNUMBER, item);
                        }
                        // 둘 다 번호가 있는 경우는 작은 번호쪽으로 모두 이동
                        else
                        {
                            if (dic[item.Defect.CLUSTERNUMBER].Count > dic[cmp.Defect.CLUSTERNUMBER].Count)
                                Move(dic, cmp.Defect.CLUSTERNUMBER, item.Defect.CLUSTERNUMBER);
                            else if (dic[item.Defect.CLUSTERNUMBER].Count < dic[cmp.Defect.CLUSTERNUMBER].Count)
                                Move(dic, item.Defect.CLUSTERNUMBER, cmp.Defect.CLUSTERNUMBER);
                            else if (item.Defect.CLUSTERNUMBER != cmp.Defect.CLUSTERNUMBER)
                                Move(dic, Math.Max(item.Defect.CLUSTERNUMBER, cmp.Defect.CLUSTERNUMBER), Math.Min(item.Defect.CLUSTERNUMBER, cmp.Defect.CLUSTERNUMBER));
                        }
                    }
                }
            }

            foreach (var item in dic)
            {
                // 지정된 CLUSTER COUNT 이하인 경우 0으로 변경
                if (item.Value.Count < clusterCount)
                {
                    foreach (var val in item.Value)
                        val.Defect.CLUSTERNUMBER = 0;
                }

                item.Value.Clear();
            }

            dic.Clear();
        }

        private static void Append(Dictionary<int, List<DefectItem>> dic, int clusterNo, DefectItem item)
        {
            if (!dic.ContainsKey(clusterNo))
                dic.Add(clusterNo, new List<DefectItem>());

            dic[clusterNo].Add(item);
        }

        private static void Move(Dictionary<int, List<DefectItem>> dic, int fromNo, int toNo)
        {
            foreach (DefectItem item in dic[fromNo])
            {
                item.Defect.CLUSTERNUMBER = toNo;
                dic[toNo].Add(item);
            }

            dic.Remove(fromNo);
        }

        /// <summary>
        /// Defect과 영역(Rectangle)을 표시
        /// </summary>
        internal class DefectItem
        {
            public DefectItem(float x, float y, RectangleF rect, Defect defect)
            {
                X = x;
                Y = y;
                Rect = rect;
                Defect = defect;
            }

            public bool IntersectsWith(DefectItem item)
            {
                if (Rect.Left > item.X || Rect.Right < item.X || Rect.Top > item.Y || Rect.Bottom < item.Y)
                    return false;

                return true;
            }

            public float X;
            public float Y;
            public RectangleF Rect;
            public Defect Defect;
            public int No;
        }
    }
}
