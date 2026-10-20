# GDS SREF 배치 변환 Probe (2026-10-15)
# 목적: GdsMapControl.FlattenStructure 가 계층 GDS(SREF)를 다른 GDS 도구와 같은 월드 좌표로 펼치는지 확인한다.
#   1) 중첩 SREF 행렬 순서: 부모 회전 / 반전 / 배율 + 자식 이동, 부모 이동 + 자식 회전 등 7가지 (도형 21개)
#   2) SREF 배율이 PATH 폭에 반영되는지 (배율 2 / 1.5)
#   3) 같은 원점에 놓인 서로 다른 SREF 가 읽기 단계에서 빠지지 않는지
#   4) 같은 셀을 두 번 배치한 GDS 가 열리고, 한 배치를 선택해도 다른 배치가 선택된 것으로 보이지 않는지
#   5) (샘플이 있으면) 실제 샘플 OMM all layer2.gds 의 화면 / 저장 결과가 변경 전과 같은지
#      (도형 수 / 읽기 중복 수 / 배치 ID + 좌표 + 폭 + Bounds + 문자열 SHA-256)
# 기대 좌표는 같은 합성 GDS 를 KLayout 0.30.12 로 펼친 값이다. 합성 GDS 는 이 스크립트가 만든다.
# 화면은 띄우지 않고 DB / Service 를 쓰지 않는다.
# 실행: powershell -NoProfile -STA -ExecutionPolicy Bypass -File GdsSrefTransformProbe.ps1 -GdsExe <빌드한 NexplantQMS.GdsMap.exe> [-SampleGds <경로>]
param(
    [Parameter(Mandatory = $true)][string]$GdsExe,
    [string]$SampleGds = ''
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
if ($SampleGds -eq '') { $SampleGds = Join-Path (Split-Path $dms -Parent) 'SampleFile\OMM all layer2.gds\OMM all layer2.gds' }
$probeDir = Join-Path $env:TEMP 'chippac\gdssreftransformprobe'
New-Item -ItemType Directory -Force $probeDir | Out-Null
# Add-Type 은 .exe 를 참조로 받지 못하므로 같은 어셈블리를 .dll 이름으로 복사한다.
Copy-Item $GdsExe (Join-Path $probeDir 'NexplantQMS.GdsMap.dll') -Force
foreach ($d in 'OpenTK.dll', 'OpenTK.GLControl.dll') { Copy-Item (Join-Path (Split-Path $GdsExe) $d) $probeDir -Force }
$gdsDll = Join-Path $probeDir 'NexplantQMS.GdsMap.dll'
foreach ($d in 'OpenTK.dll', 'OpenTK.GLControl.dll', 'NexplantQMS.GdsMap.dll') { [void][Reflection.Assembly]::LoadFrom((Join-Path $probeDir $d)) }
Add-Type -AssemblyName System.Windows.Forms, System.Drawing

$helper = @"
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using NexplantQMS.GdsMap;
using NexplantQMS.GdsMap.Persistence;

/// <summary>시험용 GDS 를 만든다. user unit = 1 µm, DB unit = 1 nm. 화면은 첫 구조를 TOP 으로 쓰므로 TOP 을 먼저 쓴다.</summary>
public sealed class SrefGdsWriter
{
    readonly MemoryStream _ms = new MemoryStream();

    void Rec(int type, int dataType, byte[] data)
    {
        if (data == null) data = new byte[0];
        int pad = data.Length % 2;
        int len = 4 + data.Length + pad;
        _ms.WriteByte((byte)(len >> 8)); _ms.WriteByte((byte)len);
        _ms.WriteByte((byte)type); _ms.WriteByte((byte)dataType);
        _ms.Write(data, 0, data.Length);
        if (pad == 1) _ms.WriteByte(0);
    }

    static byte[] I16(params int[] v)
    {
        var b = new byte[v.Length * 2];
        for (int i = 0; i < v.Length; i++) { b[2 * i] = (byte)(v[i] >> 8); b[2 * i + 1] = (byte)v[i]; }
        return b;
    }

    static byte[] I32(params int[] v)
    {
        var b = new byte[v.Length * 4];
        for (int i = 0; i < v.Length; i++)
        {
            b[4 * i] = (byte)(v[i] >> 24); b[4 * i + 1] = (byte)(v[i] >> 16);
            b[4 * i + 2] = (byte)(v[i] >> 8); b[4 * i + 3] = (byte)v[i];
        }
        return b;
    }

    /// <summary>GDS 8바이트 실수(지수 64 초과 16진 지수 + 56비트 가수).</summary>
    static byte[] Real8(double v)
    {
        var b = new byte[8];
        if (v == 0) return b;
        int sign = v < 0 ? 0x80 : 0;
        v = Math.Abs(v);
        int exp = 64;
        while (v >= 1) { v /= 16.0; exp++; }
        while (v < 1.0 / 16) { v *= 16.0; exp--; }
        ulong mant = (ulong)Math.Round(v * 72057594037927936.0);
        if (mant >= (1UL << 56)) { mant >>= 4; exp++; }
        b[0] = (byte)(sign | exp);
        for (int i = 7; i >= 1; i--) { b[i] = (byte)(mant & 0xFF); mant >>= 8; }
        return b;
    }

    static byte[] Ascii(string s) { return Encoding.ASCII.GetBytes(s); }
    static int Nm(double um) { return (int)Math.Round(um * 1000); }
    static byte[] Date() { return I16(2026, 10, 15, 0, 0, 0, 2026, 10, 15, 0, 0, 0); }

    void XY(params double[] xy)
    {
        var v = new int[xy.Length];
        for (int i = 0; i < xy.Length; i++) v[i] = Nm(xy[i]);
        Rec(0x10, 0x03, I32(v));
    }

    public SrefGdsWriter(string libName)
    {
        Rec(0x00, 2, I16(600)); Rec(0x01, 2, Date()); Rec(0x02, 6, Ascii(libName));
        Rec(0x03, 5, Real8(0.001).Concat(Real8(1e-9)).ToArray());
    }

    public void BeginStructure(string name) { Rec(0x05, 2, Date()); Rec(0x06, 6, Ascii(name)); }
    public void EndStructure() { Rec(0x07, 0, null); }

    public void Boundary(int layer, params double[] xy)
    {
        Rec(0x08, 0, null); Rec(0x0D, 2, I16(layer)); Rec(0x0E, 2, I16(0)); XY(xy); Rec(0x11, 0, null);
    }

    public void Path(int layer, double width, params double[] xy)
    {
        Rec(0x09, 0, null); Rec(0x0D, 2, I16(layer)); Rec(0x0E, 2, I16(0)); Rec(0x21, 2, I16(0));
        Rec(0x0F, 3, I32(Nm(width))); XY(xy); Rec(0x11, 0, null);
    }

    public void Text(int layer, double x, double y, string s)
    {
        Rec(0x0C, 0, null); Rec(0x0D, 2, I16(layer)); Rec(0x16, 2, I16(0)); XY(x, y); Rec(0x19, 6, Ascii(s)); Rec(0x11, 0, null);
    }

    public void Sref(string name, double x, double y, double angle, double mag, bool mirror)
    {
        Rec(0x0A, 0, null); Rec(0x12, 6, Ascii(name));
        if (angle != 0 || mag != 1 || mirror)
        {
            Rec(0x1A, 1, new byte[] { (byte)(mirror ? 0x80 : 0), 0 });
            if (mag != 1) Rec(0x1B, 5, Real8(mag));
            if (angle != 0) Rec(0x1C, 5, Real8(angle));
        }
        XY(x, y); Rec(0x11, 0, null);
    }

    /// <summary>셀마다 원본 크기를 다르게 한 시험 셀: BOUNDARY(L1) / PATH 폭 2(L2) / TEXT(L3).</summary>
    public void Leaf(string name, int k)
    {
        BeginStructure(name);
        Boundary(1, 0, 0, 10 + k, 0, 10 + k, 5, 0, 5, 0, 0);
        Path(2, 2.0, 0, 0, 20 + k, 0);
        Text(3, 1, 1 + k, "P" + k);
        EndStructure();
    }

    public byte[] Finish() { Rec(0x04, 0, null); return _ms.ToArray(); }
}

public static class SrefTransformProbe
{
    static string R(double v) { return v.ToString("R", CultureInfo.InvariantCulture); }

    // ---- 합성 GDS ----
    public static void WriteOrderGds(string path)
    {
        var w = new SrefGdsWriter("SREF_ORDER");
        // 사례: 이름 / TOP SREF 원점 / TOP 변환(회전, 배율, 반전) / MID 안 SREF 원점 / MID 안 변환
        var cases = new object[][]
        {
            new object[] { "A", 1000.0, 2000.0, 90.0, 1.0, false, 100.0, 0.0, 0.0, 1.0, false },   // 부모 회전 90 + 자식 이동
            new object[] { "B", 3000.0, 0.0, 0.0, 1.0, true, 100.0, 50.0, 0.0, 1.0, false },      // 부모 반전 + 자식 이동
            new object[] { "C", 5000.0, 0.0, 0.0, 2.0, false, 100.0, 0.0, 0.0, 1.0, false },      // 부모 배율 2 + 자식 이동 (+ PATH 폭)
            new object[] { "D", 7000.0, 1000.0, 0.0, 1.0, false, 100.0, 0.0, 90.0, 1.0, false },  // 부모 이동 + 자식 회전 90
            new object[] { "F", 11000.0, 0.0, 0.0, 1.0, false, 100.0, 0.0, 0.0, 1.0, false },     // 2단 이동만
            new object[] { "G", 13000.0, 0.0, 30.0, 1.5, false, 100.0, 50.0, 45.0, 1.0, true },   // 회전 30 + 배율 1.5 / 자식 반전 + 회전 45
        };
        w.BeginStructure("TOP");
        foreach (var c in cases)
            w.Sref("MID_" + c[0], (double)c[1], (double)c[2], (double)c[3], (double)c[4], (bool)c[5]);
        w.Sref("LEAF_E", 9000, 0, 90, 1, false);   // 깊이 1 회전
        w.EndStructure();
        for (int i = 0; i < cases.Length; i++)
        {
            var c = cases[i];
            w.BeginStructure("MID_" + c[0]);
            w.Sref("LEAF_" + c[0], (double)c[6], (double)c[7], (double)c[8], (double)c[9], (bool)c[10]);
            w.EndStructure();
            w.Leaf("LEAF_" + c[0], i);
        }
        w.Leaf("LEAF_E", 10);
        File.WriteAllBytes(path, w.Finish());
    }

    public static void WriteDupGds(string path)
    {
        var w = new SrefGdsWriter("SREF_DUP");
        w.BeginStructure("TOP");
        w.Sref("LEAF_1", 0, 0, 0, 1, false);
        w.Sref("LEAF_2", 0, 0, 0, 1, false);      // 다른 셀, 같은 원점
        w.Sref("LEAF_3", 500, 0, 0, 1, false);
        w.Sref("LEAF_3", 500, 0, 180, 1, false);  // 같은 셀, 같은 원점, 회전만 다름
        w.EndStructure();
        w.Leaf("LEAF_1", 1); w.Leaf("LEAF_2", 2); w.Leaf("LEAF_3", 3);
        File.WriteAllBytes(path, w.Finish());
    }

    public static void WriteRepeatGds(string path)
    {
        var w = new SrefGdsWriter("SREF_REPEAT");
        w.BeginStructure("TOP");
        w.Sref("LEAF_1", 0, 0, 0, 1, false);
        w.Sref("LEAF_1", 100, 0, 0, 1, false);    // 같은 셀을 두 번 배치
        w.EndStructure();
        w.Leaf("LEAF_1", 1);
        File.WriteAllBytes(path, w.Finish());
    }

    // ---- 읽기 / 펼치기 ----
    public static GdsLibrary Read(string path) { return new GdsReader { LengthUnit = LengthUnit.Micrometer }.Read(path); }

    public static List<MapPlacedElementData> Placed(GdsMapControl map)
    {
        var list = new List<MapPlacedElementData>();
        map.VisitPlacedElements(d => list.Add(d));
        return list;
    }

    public static int DuplicateCount(GdsLibrary lib)
    {
        int n = 0;
        foreach (var s in lib.Structures) n += s.Layers.DuplicatedItems.Count;
        return n;
    }

    // ---- 기대 도형과 대조 ----
    // 기대 형식: "layer|종류|폭|x,y x,y ..." (점 순서 무관, 닫는 점 중복 무관)
    static List<double[]> Points(string s)
    {
        var r = new List<double[]>();
        foreach (var tok in s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
        {
            var xy = tok.Split(',');
            r.Add(new[] { double.Parse(xy[0], CultureInfo.InvariantCulture), double.Parse(xy[1], CultureInfo.InvariantCulture) });
        }
        return r;
    }

    static List<double[]> Unique(IEnumerable<double[]> pts, double tol)
    {
        var r = new List<double[]>();
        foreach (var p in pts)
            if (!r.Any(q => Math.Abs(q[0] - p[0]) <= tol && Math.Abs(q[1] - p[1]) <= tol)) r.Add(p);
        return r;
    }

    static bool SamePoints(List<double[]> a, List<double[]> b, double tol)
    {
        if (a.Count != b.Count) return false;
        var used = new bool[b.Count];
        foreach (var p in a)
        {
            int hit = -1;
            for (int j = 0; j < b.Count; j++)
                if (!used[j] && Math.Abs(p[0] - b[j][0]) <= tol && Math.Abs(p[1] - b[j][1]) <= tol) { hit = j; break; }
            if (hit < 0) return false;
            used[hit] = true;
        }
        return true;
    }

    /// <summary>실제 배치 도형과 기대 도형을 1:1 로 맞춘다. 좌표 허용 0.01 µm(float 계산), 폭 허용 1e-6 µm.</summary>
    public static string Match(List<MapPlacedElementData> actual, string[] expected, out bool ok)
    {
        const double tol = 0.01;
        var exp = expected.Select(e => e.Split('|')).ToList();
        var used = new bool[exp.Count];
        var bad = new List<string>();
        int matched = 0;
        foreach (var d in actual)
        {
            var pts = Unique(d.WorldPoints.Select(p => new[] { p.X, p.Y }), 1e-6);
            int hit = -1;
            for (int i = 0; i < exp.Count; i++)
            {
                if (used[i] || int.Parse(exp[i][0]) != d.LayerId || exp[i][1] != d.ElementType) continue;
                if (!SamePoints(pts, Unique(Points(exp[i][3]), 1e-6), tol)) continue;
                hit = i; break;
            }
            if (hit < 0) { bad.Add(d.PlacedElementId + " " + d.ElementType + " 좌표 (" + R(d.WorldPoints[0].X) + ", " + R(d.WorldPoints[0].Y) + ") 기대에 없음"); continue; }
            used[hit] = true;
            double w = double.Parse(exp[hit][2], CultureInfo.InvariantCulture);
            if (Math.Abs(w - d.PathWidth) > 1e-6) { bad.Add(d.PlacedElementId + " PATH 폭 " + R(d.PathWidth) + " / 기대 " + R(w)); continue; }
            matched++;
        }
        for (int i = 0; i < exp.Count; i++) if (!used[i]) bad.Add("기대 도형 없음: " + string.Join("|", exp[i]));
        ok = bad.Count == 0 && matched == exp.Count && actual.Count == exp.Count;
        return "일치 " + matched + " / 기대 " + exp.Count + " / 실제 " + actual.Count + (bad.Count > 0 ? " / " + string.Join(" ; ", bad.Take(6)) : "");
    }

    /// <summary>같은 셀의 두 배치 중 하나만 선택했을 때 다른 배치가 선택된 것으로 보이지 않는지 본다.</summary>
    public static string SelectionCheck(GdsMapControl map, out bool ok)
    {
        const BindingFlags NP = BindingFlags.NonPublic | BindingFlags.Instance;
        var all = ((System.Collections.IEnumerable)typeof(GdsMapControl).GetProperty("AllItems", NP).GetValue(map, null)).Cast<object>().ToList();
        Type itemType = all[0].GetType();
        PropertyInfo source = itemType.GetProperty("Source"), placed = itemType.GetProperty("PlacedElementId");
        var boundaries = all.Where(o => source.GetValue(o, null) is GdsBoundary).ToList();
        MethodInfo append = typeof(GdsMapControl).GetMethod("AppendSelectedItem", NP);
        MethodInfo contains = typeof(GdsMapControl).GetMethod("ContainsSelectedItem", NP);
        MethodInfo clear = typeof(GdsMapControl).GetMethod("ClearSelectionInternal", NP);
        if (boundaries.Count != 2 || append == null || contains == null)
        {
            ok = false;
            return "BOUNDARY 배치 " + boundaries.Count + "개 / 선택 메서드 " + (append != null && contains != null ? "있음" : "없음");
        }
        if (clear != null) clear.Invoke(map, null);
        append.Invoke(map, new[] { boundaries[0] });
        bool first = (bool)contains.Invoke(map, new[] { boundaries[0] });
        bool second = (bool)contains.Invoke(map, new[] { boundaries[1] });
        ok = first && !second && ReferenceEquals(source.GetValue(boundaries[0], null), source.GetValue(boundaries[1], null));
        return "같은 원본 BOUNDARY 배치 2개(" + placed.GetValue(boundaries[0], null) + " / " + placed.GetValue(boundaries[1], null)
            + ") 하나 선택 -> 선택됨 " + first + " / 다른 배치 선택됨 " + second;
    }

    /// <summary>저장에 쓰는 값(배치 ID / Layer / DataType / 종류 / 폭 / 좌표 / Bounds / 문자열)을 배치 ID 순으로 모아 SHA-256 을 만든다.</summary>
    public static string PlacedHash(GdsMapControl map, out int count)
    {
        var lines = new List<string>();
        map.VisitPlacedElements(d =>
        {
            var sb = new StringBuilder();
            sb.Append(d.PlacedElementId).Append('\t').Append(d.LayerId).Append('\t').Append(d.DataType).Append('\t')
              .Append(d.ElementType).Append('\t').Append(R(d.PathWidth)).Append('\t');
            foreach (var p in d.WorldPoints) sb.Append(R(p.X)).Append(',').Append(R(p.Y)).Append(' ');
            sb.Append('\t').Append(R(d.Bounds.MinX)).Append(',').Append(R(d.Bounds.MinY)).Append(',')
              .Append(R(d.Bounds.MaxX)).Append(',').Append(R(d.Bounds.MaxY)).Append('\t').Append(d.Text);
            lines.Add(sb.ToString());
        });
        lines.Sort(StringComparer.Ordinal);
        count = lines.Count;
        using (var sha = SHA256.Create())
        {
            byte[] h = sha.ComputeHash(Encoding.UTF8.GetBytes(string.Join("\n", lines)));
            return BitConverter.ToString(h).Replace("-", "");
        }
    }
}
"@
Add-Type -TypeDefinition $helper -ReferencedAssemblies @($gdsDll, (Join-Path $probeDir 'OpenTK.dll'), (Join-Path $probeDir 'OpenTK.GLControl.dll'),
    'System.Data', 'System.Windows.Forms', 'System.Drawing', 'System.Xml', 'System.Core')

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}

# 합성 GDS 를 읽어 화면과 같은 GdsMapControl 로 펼친다. 예외는 결과로 돌려준다.
function Show($path) {
    $lib = [SrefTransformProbe]::Read($path)
    $map = New-Object NexplantQMS.GdsMap.GdsMapControl
    try { $map.ShowStructure($lib); return @{ Lib = $lib; Map = $map; Error = $null } }
    catch { $e = $_.Exception; while ($e.InnerException) { $e = $e.InnerException }; $map.Dispose(); return @{ Lib = $lib; Map = $null; Error = $e.GetType().Name + ': ' + $e.Message } }
}

# KLayout 0.30.12 로 같은 파일을 펼친 결과 (layer|종류|폭|좌표)
$orderExpected = @(
    '1|BOUNDARY|0|995,2100 995,2110 1000,2110 1000,2100',
    '1|BOUNDARY|0|3100,-55 3100,-50 3111,-50 3111,-55',
    '1|BOUNDARY|0|5200,0 5200,10 5224,10 5224,0',
    '1|BOUNDARY|0|7095,1000 7095,1013 7100,1013 7100,1000',
    '1|BOUNDARY|0|11100,0 11100,5 11114,5 11114,0',
    '1|BOUNDARY|0|13099.648254,138.010762 13092.403811,139.951905 13098.227239,161.685236 13105.471683,159.744094',
    '1|BOUNDARY|0|8995,0 8995,20 9000,20 9000,0',
    '2|PATH|2|1000,2100 1000,2120',
    '2|PATH|2|3100,-50 3121,-50',
    '2|PATH|4|5200,0 5244,0',
    '2|PATH|2|7100,1000 7100,1023',
    '2|PATH|2|11100,0 11124,0',
    '2|PATH|3|13092.403811,139.951905 13102.109525,176.174124',
    '2|PATH|2|9000,0 9000,30',
    '3|TEXT|0|999,2101',
    '3|TEXT|0|3101,-52',
    '3|TEXT|0|5202,6',
    '3|TEXT|0|7096,1001',
    '3|TEXT|0|11101,5',
    '3|TEXT|0|13101.485372,139.071423',
    '3|TEXT|0|8989,1')
$dupExpected = @(
    '1|BOUNDARY|0|0,0 0,5 11,5 11,0', '1|BOUNDARY|0|0,0 0,5 12,5 12,0',
    '1|BOUNDARY|0|500,0 500,5 513,5 513,0', '1|BOUNDARY|0|487,-5 487,0 500,0 500,-5',
    '2|PATH|2|0,0 21,0', '2|PATH|2|0,0 22,0', '2|PATH|2|500,0 523,0', '2|PATH|2|500,0 477,0',
    '3|TEXT|0|1,2', '3|TEXT|0|1,3', '3|TEXT|0|501,4', '3|TEXT|0|499,-4')
$repeatExpected = @(
    '1|BOUNDARY|0|0,0 0,5 11,5 11,0', '1|BOUNDARY|0|100,0 100,5 111,5 111,0',
    '2|PATH|2|0,0 21,0', '2|PATH|2|100,0 121,0', '3|TEXT|0|1,2', '3|TEXT|0|101,2')

# 1 / 2. 중첩 SREF 행렬 순서 + PATH 폭
$orderGds = Join-Path $probeDir 'sref_order.gds'
[SrefTransformProbe]::WriteOrderGds($orderGds)
$r = Show $orderGds
if ($r.Error) { Check '중첩 SREF 좌표 / PATH 폭 (KLayout 대조)' $false $r.Error }
else {
    $ok = $false
    $detail = [SrefTransformProbe]::Match([SrefTransformProbe]::Placed($r.Map), $orderExpected, [ref]$ok)
    Check '중첩 SREF 좌표 / PATH 폭 (KLayout 대조)' $ok $detail
    $r.Map.Dispose()
}

# 3. 같은 원점 SREF
$dupGds = Join-Path $probeDir 'sref_dup.gds'
[SrefTransformProbe]::WriteDupGds($dupGds)
$r = Show $dupGds
$dupCount = [SrefTransformProbe]::DuplicateCount($r.Lib)
Check '같은 원점 SREF 읽기 중복 제외 없음' ($dupCount -eq 0) "읽기 중복 $dupCount"
if ($r.Error) { Check '같은 원점 SREF 펼치기 (KLayout 대조)' $false $r.Error }
else {
    $ok = $false
    $detail = [SrefTransformProbe]::Match([SrefTransformProbe]::Placed($r.Map), $dupExpected, [ref]$ok)
    Check '같은 원점 SREF 펼치기 (KLayout 대조)' $ok $detail
    $r.Map.Dispose()
}

# 4. 같은 셀 반복 배치
$repeatGds = Join-Path $probeDir 'sref_repeat.gds'
[SrefTransformProbe]::WriteRepeatGds($repeatGds)
$r = Show $repeatGds
Check '같은 셀 반복 배치 열기' ($r.Error -eq $null) $(if ($r.Error) { $r.Error } else { 'ShowStructure 성공' })
if (-not $r.Error) {
    $ok = $false
    $detail = [SrefTransformProbe]::Match([SrefTransformProbe]::Placed($r.Map), $repeatExpected, [ref]$ok)
    Check '같은 셀 반복 배치 펼치기 (KLayout 대조)' $ok $detail
    $ok = $false
    $detail = [SrefTransformProbe]::SelectionCheck($r.Map, [ref]$ok)
    Check '같은 셀 다른 배치 선택 구분' $ok $detail
    $r.Map.Dispose()
}

# 5. 실제 샘플 회귀 (변경 전 기준값: 2026-10-15 15:12 빌드)
$sampleCount = 605080
$sampleDuplicates = 31890
# TEXT 의 DataType 을 TEXTTYPE 으로 저장하게 되어 값이 바뀌었다(이전 0376A87D...6F2D, TEXT DataType 을 0 으로 두면 이전과 같음) (2026-10-20)
$sampleHash = 'D84FE5A9AF8B0B44D27DB6DCE05C758E8F25BE78CCBB563FFC91F15C4D3C86DF'
if (Test-Path -LiteralPath $SampleGds) {
    $r = Show $SampleGds
    if ($r.Error) { Check '실제 샘플 회귀' $false $r.Error }
    else {
        $count = 0
        $hash = [SrefTransformProbe]::PlacedHash($r.Map, [ref]$count)
        $dups = [SrefTransformProbe]::DuplicateCount($r.Lib)
        Check '실제 샘플 도형 수 / 읽기 중복 수' ($count -eq $sampleCount -and $dups -eq $sampleDuplicates) "도형 $count (기준 $sampleCount) / 읽기 중복 $dups (기준 $sampleDuplicates)"
        Check '실제 샘플 배치 ID / 좌표 / 폭 / Bounds 그대로' ($hash -eq $sampleHash) "SHA-256 $hash"
        $r.Map.Dispose()
    }
}
else { Write-Host "SKIP / 실제 샘플 없음: $SampleGds" }

$fail = @($results | Where-Object { $_ -like 'FAIL*' }).Count
if ($fail -eq 0) { Write-Host "GDS_SREF_TRANSFORM_PROBE=PASS ($($results.Count))" }
else { Write-Host "GDS_SREF_TRANSFORM_PROBE=FAIL ($fail / $($results.Count))"; exit 1 }
