using System.Collections.Generic;
using System.Drawing;

namespace DACrux.TEST.ENGUI
{
    /// <summary>
    /// Map 의 XY 방향(Direction) 기준 Die 번호. File Map Setup 과 Wafer Map 조회 화면이 같은 규칙으로 Die 위에 번호를 그린다.
    /// 규칙 (2026-10-13 결정):
    /// 1. 사용 Die (USECODE / DieProp = 1) 만 번호를 붙인다. 제거(-1) / Skip(0) / Mark(2) Die 는 번호가 없다.
    /// 2. 행 우선: 방향 번호(시작 모서리에서 1 부터 커지는 X / Y)로 Y 가 작은 행부터, 행 안에서는 X 가 작은 Die 부터 1, 2, 3 ...
    ///    LeftTop = 왼쪽 위가 1번 -> 오른쪽 -> 아래 행 / LeftBottom = 왼쪽 아래 1번 -> 위 행 /
    ///    RightTop = 오른쪽 위 1번 -> 왼쪽 -> 아래 행 / RightBottom = 오른쪽 아래 1번 -> 위 행.
    /// 이 번호는 화면에서 계산한다. 저장된 TQP_USEMAP.DIE_NUM 은 저장 순서(행 번호 i + 1)라서 이 규칙과 다르다
    /// (DSL TQP_USEMAP.CreateUseMap 확인, 2026-10-13).
    /// </summary>
    internal static class DieNumberRule
    {
        /// <summary>
        /// 방향 번호와 USECODE 로 Die 번호를 매긴다. 같은 방향 번호가 두 번 나오면 한 번만 센다 (중복 행은 데이터 확인에서 따로 알림).
        /// </summary>
        /// <param name="dies">Key = 방향 번호 (저장 X / Y), Value = USECODE</param>
        /// <returns>Key = 방향 번호, Value = Die 번호 (1 부터)</returns>
        public static Dictionary<Point, int> Number(IEnumerable<KeyValuePair<Point, int>> dies)
        {
            List<Point> used = new List<Point>();
            HashSet<Point> seen = new HashSet<Point>();
            foreach (KeyValuePair<Point, int> die in dies)
            {
                if (die.Value != 1 || !seen.Add(die.Key)) continue;
                used.Add(die.Key);
            }
            used.Sort(CompareRowFirst);
            Dictionary<Point, int> numbers = new Dictionary<Point, int>(used.Count);
            for (int i = 0; i < used.Count; i++) numbers[used[i]] = i + 1;
            return numbers;
        }

        /// <summary>행(Y) 먼저, 같은 행이면 X 순서.</summary>
        private static int CompareRowFirst(Point a, Point b)
        {
            int y = a.Y.CompareTo(b.Y);
            return y != 0 ? y : a.X.CompareTo(b.X);
        }
    }
}
