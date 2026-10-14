using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DACrux.SEMDMS.Interface;

/// <summary>
/// Die 좌표 변환(DieCoordinateMapper) 검사 (2026-10-13).
/// 입력: [0] Map 스냅샷 (X / Y / USECODE 탭 구분, 로컬 DB SCK / OMM_Hybrid 의 TQP_USEMAP)
///       [1] 고객 대응표 (E-Test (X, Y) / Back-side Klarf (X, Y) / Front-side Klarf (X, Y) / Die# / Silterra)
///       [2] E-Test 결과 파일 (TSVET 샘플)
/// 확인: 고객 대응표 76행 x 4종류가 모두 같은 시스템 좌표로 모이는지 / 역변환 / 샘플 파일 Die 가 모두 사용 Die 인지 / 범위 밖 처리.
/// </summary>
internal static class DieCoordinateProbe
{
    private static readonly Regex XyRegex = new Regex(@"\(\s*(-?\d+)\s*,\s*(-?\d+)\s*\)");
    private static int failures;
    private static int checks;

    private static int Main(string[] args)
    {
        try { return Run(args); } catch (Exception ex) { Console.WriteLine("FAIL: " + ex); return 1; }
    }

    private static int Run(string[] args)
    {
        // ---- 1) Map 스냅샷 -> 변환기 ----
        DataTable dies = new DataTable();
        dies.Columns.Add("X", typeof(int));
        dies.Columns.Add("Y", typeof(int));
        dies.Columns.Add("USECODE", typeof(int));
        foreach (string line in File.ReadAllLines(args[0]).Skip(1))
        {
            if (line.Trim().Length == 0) continue;
            string[] c = line.Split('\t');
            dies.Rows.Add(int.Parse(c[0], CultureInfo.InvariantCulture), int.Parse(c[1], CultureInfo.InvariantCulture), int.Parse(c[2], CultureInfo.InvariantCulture));
        }
        DieCoordinateMapper map = DieCoordinateMapper.FromDieTable(dies, new DiePoint(7, 5));
        Check("Map: 사용 Die 76 / X 1~12 / Y 1~9 (전체 " + dies.Rows.Count + "칸)", map.DieCount == 76 && map.MinX == 1 && map.MaxX == 12 && map.MinY == 1 && map.MaxY == 9,
            map.DieCount + " / " + map.MinX + "~" + map.MaxX + " / " + map.MinY + "~" + map.MaxY);
        foreach (DieCoordType t in new[] { DieCoordType.ETest, DieCoordType.KlarfBack, DieCoordType.KlarfFront, DieCoordType.DieNumber })
            Console.WriteLine("  규칙 " + t + ": " + map.Describe(t));

        // ---- 2) 고객 대응표 76행: 4종류 -> 같은 시스템 좌표 / 역변환 ----
        int rows = 0, bad = 0, badBack = 0;
        foreach (string line in File.ReadAllLines(args[1]).Skip(1))
        {
            if (line.Trim().Length == 0) continue;
            string[] c = line.Split('\t');
            rows++;
            DiePoint et = Xy(c[0]), back = Xy(c[1]), front = Xy(c[2]);
            DiePoint dieNo = new DiePoint(int.Parse(c[3], CultureInfo.InvariantCulture), 0);
            DiePoint sys = map.ToSystem(DieCoordType.DieNumber, dieNo);
            bool same = map.ToSystem(DieCoordType.ETest, et).Equals(sys) && map.ToSystem(DieCoordType.KlarfBack, back).Equals(sys)
                && map.ToSystem(DieCoordType.KlarfFront, front).Equals(sys);
            bool reverse = map.FromSystem(DieCoordType.ETest, sys).Equals(et) && map.FromSystem(DieCoordType.KlarfBack, sys).Equals(back)
                && map.FromSystem(DieCoordType.KlarfFront, sys).Equals(front) && map.FromSystem(DieCoordType.DieNumber, sys).Equals(dieNo);
            if (!same) { bad++; Console.WriteLine("  다름: Die#" + dieNo.X + " 시스템 " + sys); }
            if (!reverse) badBack++;
        }
        Check("고객 대응표 " + rows + "행: E-Test / KLARF Back / KLARF Front / Die# -> 같은 시스템 좌표", rows == 76 && bad == 0, "다른 행 " + bad);
        Check("고객 대응표 역변환 (시스템 -> 4종류) 일치", rows == 76 && badBack == 0, "다른 행 " + badBack);

        // ---- 3) 대표 값 ----
        Check("Die#39 = 시스템 (7, 5) = E-Test (4, 5) = KLARF Back (0, 0) = KLARF Front (-1, 0)",
            map.ToSystem(DieCoordType.DieNumber, new DiePoint(39, 0)).Equals(new DiePoint(7, 5))
            && map.Convert(DieCoordType.KlarfBack, new DiePoint(0, 0), DieCoordType.ETest).Equals(new DiePoint(4, 5))
            && map.Convert(DieCoordType.KlarfBack, new DiePoint(0, 0), DieCoordType.KlarfFront).Equals(new DiePoint(-1, 0)), "");
        Check("KLARF Front (0, 0) = Die#38", map.Convert(DieCoordType.KlarfFront, new DiePoint(0, 0), DieCoordType.DieNumber).X == 38,
            map.Convert(DieCoordType.KlarfFront, new DiePoint(0, 0), DieCoordType.DieNumber).ToString());

        // ---- 4) 범위 밖 ----
        DiePoint outSys;
        Check("E-Test (0, 0) = 시스템 (12, 9) 모서리 -> 사용 Die 아님 (TryToSystem false)",
            !map.TryToSystem(DieCoordType.ETest, new DiePoint(0, 0), out outSys) && outSys.Equals(new DiePoint(12, 9)), outSys.ToString());
        Check("Die# 0 / 77 -> TryToSystem false", !map.TryToSystem(DieCoordType.DieNumber, new DiePoint(0, 0), out outSys)
            && !map.TryToSystem(DieCoordType.DieNumber, new DiePoint(77, 0), out outSys), "");
        Check("사용 Die 아닌 시스템 좌표의 Die# = 0", map.DieNumberOf(new DiePoint(1, 1)) == 0, map.DieNumberOf(new DiePoint(1, 1)).ToString());
        Check("Die# 77 ToSystem -> 범위 오류", Throws(() => map.ToSystem(DieCoordType.DieNumber, new DiePoint(77, 0))).Contains("1 ~ 76"), "");

        // ---- 5) E-Test 샘플 파일 Die -> 모두 사용 Die / DUT #20 = Die#71 ----
        var file = DACrux.SEMDMS.ENGUI.EtResultFileParser.Parse(args[2]);
        int dieRows = file.Dies.GetLength(0), notUsed = 0, dut20 = 0;
        var numbers = new List<int>();
        for (int i = 0; i < dieRows; i++)
        {
            DiePoint et = new DiePoint(int.Parse(file.Dies[i, 1], CultureInfo.InvariantCulture), int.Parse(file.Dies[i, 2], CultureInfo.InvariantCulture));
            DiePoint sys;
            if (!map.TryToSystem(DieCoordType.ETest, et, out sys)) { notUsed++; continue; }
            int n = map.DieNumberOf(sys);
            numbers.Add(n);
            if (file.Dies[i, 0] == "20") dut20 = n;
        }
        Check("E-Test 샘플 " + dieRows + " Die 모두 사용 Die", dieRows == 24 && notUsed == 0, "사용 Die 아님 " + notUsed);
        Check("E-Test 샘플 DUT #20 (7, 3) = Die#71", dut20 == 71, dut20.ToString());
        Console.WriteLine("  샘플 Die#: " + string.Join(", ", numbers.OrderBy(x => x).Select(x => x.ToString(CultureInfo.InvariantCulture)).ToArray()));

        Console.WriteLine(failures == 0 ? "DIE_COORD_PROBE=PASS (" + checks + "/" + checks + ")" : "DIE_COORD_PROBE=FAIL (" + failures + "/" + checks + ")");
        return failures == 0 ? 0 : 1;
    }

    private static DiePoint Xy(string text)
    {
        Match m = XyRegex.Match(text);
        if (!m.Success) throw new FormatException("(X, Y) 형식이 아닙니다: " + text);
        return new DiePoint(int.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture), int.Parse(m.Groups[2].Value, CultureInfo.InvariantCulture));
    }

    private static string Throws(Action action)
    {
        try { action(); return ""; } catch (Exception ex) { return ex.Message; }
    }

    private static void Check(string name, bool ok, string detail)
    {
        checks++;
        if (!ok) failures++;
        Console.WriteLine((ok ? "PASS" : "FAIL") + " / " + name + " / " + detail);
    }
}
