using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using DACrux.TEST.ENGUI;

/// <summary>실제 샘플 형상/좌표/저장 데이터와 잘못된 파일 거부를 DB 접속 없이 확인한다.</summary>
internal static class StandardMapFileTests
{
    /// <summary>실제 Parser를 같은 어셈블리로 컴파일하여 실행하는 독립 회귀 검사 진입점이다.</summary>
    private static int Main(string[] args)
    {
        string sample = File.ReadAllText(args[0]);
        var map = StandardMapFile.Parse(sample);
        Check(map.Rows == 7 && map.Columns == 10 && map.Dies.Count == 26, "7 x 10 / 26 dies");
        Check(map.Dies.Contains(new Point(4, 1)) && map.Dies.Contains(new Point(5, 1)), "000 is present in first row");
        Check(!map.Dies.Any(p => p.Y == 2 || p.Y == 6), "empty rows preserved");
        Check(map.Dies.Contains(new Point(1, 5)) && map.Dies.Contains(new Point(10, 3)), "asymmetric coordinates");
        Check(!map.Dies.Contains(new Point(1, 1)), "empty origin cell excluded");
        Check(map.CenterDie == new Point(5, 4), "nearest used center die");
        Check(Math.Abs(map.PitchX - 23.834) < 1e-9 && Math.Abs(map.CenterX - 95.336) < 1e-9
            && Math.Abs(map.CenterY + 75.15) < 1e-9, "millimetres and LeftTop center");
        Check(StandardMapFile.Parse(sample.Replace("025", "000")).Dies.SequenceEqual(map.Dies), "Bin independent shape");
        Check(StandardMapFile.Parse(sample.Replace(" ", "  \t")).Dies.SequenceEqual(map.Dies), "whitespace");
        Check(StandardMapFile.Parse(sample.Replace("___", "__")).Dies.SequenceEqual(map.Dies), "two underscore empty die");
        Check(StandardMapFile.Parse(sample.Replace("FNLOC:180", "FNLOC:90")).Dies.SequenceEqual(map.Dies), "no implicit rotation");
        var originalCulture = CultureInfo.CurrentCulture;
        try
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("de-DE");
            Check(Math.Abs(StandardMapFile.Parse(sample).PitchY - 30.060) < 1e-9, "invariant decimal parsing");
        }
        finally { System.Threading.Thread.CurrentThread.CurrentCulture = originalCulture; }
        Check(Math.Abs(StandardMapFile.Parse(sample.Replace("DUTMS:mm", "DUTMS:mil")).PitchX - 23.834 * 0.0254) < 1e-9, "mil conversion");
        Reject(sample.Replace("ROWCT:7", "ROWCT:8"), "row count");
        Reject(sample.Replace("COLCT:10", "COLCT:9"), "column count");
        Reject(sample.Replace("025", "BAD"), "invalid token");
        Reject(sample.Replace("XDIES:23.834", "XDIES:NaN"), "NaN pitch");
        Reject(sample.Replace("YDIES:30.060", "YDIES:0"), "zero pitch");
        Reject(sample.Replace("DUTMS:mm", "DUTMS:unknown"), "unknown unit");
        Reject(sample.Replace("FNLOC:180", "FNLOC:45"), "invalid angle");
        Reject(sample.Replace("REFPX:1", "REFPX:11"), "invalid reference");
        Reject(sample + "\nROWCT:7", "duplicate header");
        Reject(sample.Replace("025", "___").Replace(" 000", " ___"), "empty map");
        Console.WriteLine("PASS: 22 parser checks");
        return 0;
    }

    /// <summary>기대 결과와 다르면 검사 실패로 처리한다.</summary>
    private static void Check(bool condition, string name)
    {
        if (!condition) throw new Exception("FAIL: " + name);
    }

    /// <summary>형식이 잘못된 입력이 부분 Map으로 반환되지 않는지 확인한다.</summary>
    private static void Reject(string input, string name)
    {
        try { StandardMapFile.Parse(input); }
        catch (FormatException) { return; }
        throw new Exception("FAIL: accepted " + name);
    }
}
