using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using DACrux.Base;

namespace DACrux.TEST.ENGUI
{
    /// <summary>
    /// KlarfMapSource / KlarfMapRule 검증 (구현 1단계, DB / 화면 없음).
    /// 사용: KlarfMapRuleTests.exe <KLARF 파일> [같은 Wafer 의 SINF txt]
    /// 샘플(AIBIZ_SCK.KLAF.SINF) 기준 기대값은 설계 문서 2절 계산 결과다.
    /// 출력 마지막 줄: KLARF_MAP_PROBE=PASS / FAIL
    /// </summary>
    internal static class KlarfMapRuleTests
    {
        private static int failures;

        private static int Main(string[] args)
        {
            try
            {
                if (args.Length < 1) throw new ArgumentException("KLARF 파일 경로가 필요합니다.");
                KlarfMapSource source = KlarfMapSource.Load(args[0], null);
                CheckSample(source);
                if (args.Length > 1) CheckSinf(source.Wafers[0], args[1]);
                CheckDirections(source.Wafers[0].Grid);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR " + ex);
                failures++;
            }
            Console.WriteLine(failures == 0 ? "KLARF_MAP_PROBE=PASS" : "KLARF_MAP_PROBE=FAIL (" + failures + ")");
            return failures == 0 ? 0 : 1;
        }

        /// <summary>헤더 값 / 옮긴 양 / 격자 / 검사 Die / Defect 위치와 물리 좌표식을 확인한다.</summary>
        private static void CheckSample(KlarfMapSource s)
        {
            KlarfMapWafer w = s.Wafers[0];
            KlarfMapGrid g = w.Grid;
            Console.WriteLine(string.Format(CultureInfo.InvariantCulture,
                "Device={0} Lot={1} Wafers={2} WaferSize={3} Pitch={4}x{5} Mark={6}/{7} Angle={8} OriginFromDB={9}",
                s.DeviceID, s.LotID, s.Wafers.Count, s.WaferSize, s.PitchX, s.PitchY, s.MarkType, s.MarkLocation, s.NotchAngle, s.OriginFromDB));
            Console.WriteLine(string.Format(CultureInfo.InvariantCulture,
                "Wafer={0} Slot={1} Center=({2},{3}) Shift=({4},{5}) Grid X {6}~{7} Y {8}~{9} CalcDies={10} Inspected={11} Defects={12}",
                w.WaferID, w.Slot, w.CenterX, w.CenterY, w.ShiftX, w.ShiftY, g.MinX, g.MaxX, g.MinY, g.MaxY,
                g.CalculatorDieCount, w.InspectedDies.Count, w.DefectCount));

            Check("Device", s.DeviceID == "Hybrid");
            Check("WaferSize 300000", s.WaferSize == 300000d);
            Check("Pitch", s.PitchX == 23834d && Math.Abs(s.PitchY - 30060.1d) < 1e-6);
            Check("Angle DOWN=0", s.NotchAngle == 0 && !s.IsFlat);
            Check("OriginFromDB false (조회 함수 없음)", !s.OriginFromDB);
            Check("Shift (7,5)", w.ShiftX == 7 && w.ShiftY == 5);
            Check("Grid X 1~12 / Y 1~9", g.MinX == 1 && g.MaxX == 12 && g.MinY == 1 && g.MaxY == 9);
            Check("OriginIndex = Shift", g.OriginIndexX == w.ShiftX && g.OriginIndexY == w.ShiftY);
            Check("Inspected 26", w.InspectedDies.Count == 26);

            HashSet<Point> inspected = new HashSet<Point>(w.InspectedDies);
            bool allInGrid = true;
            foreach (Point p in w.InspectedDies) allInGrid &= g.Contains(p);
            Check("검사 Die 모두 격자 안", allInGrid);

            // Defect: 검사 Die 안 / 물리 좌표 = (원본 인덱스 * Pitch + REL) - Center
            int outside = 0, mismatch = 0;
            foreach (Defect d in w.Defects)
            {
                if (!inspected.Contains(new Point(d.XINDEX, d.YINDEX))) outside++;
                double x = (d.XINDEX - w.ShiftX) * s.PitchX + d.XREL - w.CenterX;
                double y = (d.YINDEX - w.ShiftY) * s.PitchY + d.YREL - w.CenterY;
                if (Math.Abs(x - d.X) > 0.01 || Math.Abs(y - d.Y) > 0.01) mismatch++;
            }
            Check("Defect 109", w.DefectCount == 109);
            Check("Defect 모두 검사 Die 안 (outside=" + outside + ")", outside == 0);
            Check("Defect 물리 좌표식 일치 (mismatch=" + mismatch + ")", mismatch == 0);
        }

        /// <summary>
        /// 같은 Wafer 의 SINF RowData 숫자 칸과 KLARF 검사 Die 가 같은 위치인지 확인한다.
        /// SINF 는 첫 행이 위쪽이므로 LeftBottom 인덱스로 바꾸면 X = 열 + (원본 X 최소 - 1 + Shift) 형태가 된다.
        /// 여기서는 위치 집합만 비교하기 위해 두 집합의 최소값을 맞춰 비교한다.
        /// </summary>
        private static void CheckSinf(KlarfMapWafer w, string sinfPath)
        {
            List<string> rows = new List<string>();
            foreach (string line in File.ReadAllLines(sinfPath))
                if (line.StartsWith("RowData:", StringComparison.OrdinalIgnoreCase)) rows.Add(line.Substring(8).Trim());

            List<Point> sinf = new List<Point>();
            for (int r = 0; r < rows.Count; r++)
            {
                string[] cells = rows[r].Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                for (int c = 0; c < cells.Length; c++)
                    if (!cells[c].StartsWith("_")) sinf.Add(new Point(c + 1, rows.Count - r));
            }

            int minSX = int.MaxValue, minSY = int.MaxValue, minKX = int.MaxValue, minKY = int.MaxValue;
            foreach (Point p in sinf) { minSX = Math.Min(minSX, p.X); minSY = Math.Min(minSY, p.Y); }
            foreach (Point p in w.InspectedDies) { minKX = Math.Min(minKX, p.X); minKY = Math.Min(minKY, p.Y); }

            HashSet<Point> klarf = new HashSet<Point>(w.InspectedDies);
            int missing = 0;
            foreach (Point p in sinf)
                if (!klarf.Contains(new Point(p.X - minSX + minKX, p.Y - minSY + minKY))) missing++;
            Console.WriteLine(string.Format("SINF dies={0} offset=({1},{2}) missing={3}", sinf.Count, minKX - minSX, minKY - minSY, missing));
            Check("SINF 26개 = KLARF 검사 Die", sinf.Count == klarf.Count && missing == 0);
        }

        /// <summary>XY 방향 4종 번호 변환: 모서리 값과 왕복(같은 식 두 번 = 원래 값)을 확인한다.</summary>
        private static void CheckDirections(KlarfMapGrid g)
        {
            Point lb = new Point(g.MinX, g.MinY);
            Check("LeftBottom 그대로", KlarfMapRule.ToDirection(lb, XYDirection.LeftBottom, g) == lb);
            Check("LeftTop 좌하단 -> (MinX, MaxY)", KlarfMapRule.ToDirection(lb, XYDirection.LeftTop, g) == new Point(g.MinX, g.MaxY));
            Check("RightBottom 좌하단 -> (MaxX, MinY)", KlarfMapRule.ToDirection(lb, XYDirection.RightBottom, g) == new Point(g.MaxX, g.MinY));
            Check("RightTop 좌하단 -> (MaxX, MaxY)", KlarfMapRule.ToDirection(lb, XYDirection.RightTop, g) == new Point(g.MaxX, g.MaxY));

            bool roundTrip = true;
            foreach (XYDirection dir in Enum.GetValues(typeof(XYDirection)))
                for (int x = g.MinX; x <= g.MaxX; x++)
                    for (int y = g.MinY; y <= g.MaxY; y++)
                    {
                        Point p = new Point(x, y);
                        Point d = KlarfMapRule.ToDirection(p, dir, g);
                        roundTrip &= g.Contains(d) && KlarfMapRule.FromDirection(d, dir, g) == p;
                    }
            Check("방향 4종 왕복 / 격자 안", roundTrip);
        }

        private static void Check(string name, bool ok)
        {
            Console.WriteLine((ok ? "PASS " : "FAIL ") + name);
            if (!ok) failures++;
        }
    }
}
