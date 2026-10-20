# GDS Map Back side(좌우 반전) 저장 검증 스크립트 (설계: 문서/설계/02_GDS/2026-10-06_GDS_반전_BackSide_설계.md 8절)
# 목적: Setup 화면 DB 저장과 같은 경로(VisitPlacedElements(layer, mirrorX) / GdsMapPlacedRowBuilder / BSL GdsMapImport)로
#       Back side 저장을 실행하고 확인한다.
#       1) 반전 좌표 = GDS 원점 기준 X' = -X(비트 일치), Y / 도형 수 / 좌표 수 동일, Bounds = (-MaxX, MinY, -MinX, MaxY)
#       2) 반전 추출 후에도 화면 도형 좌표(원본 GDS 배열 공유) 불변, 두 번 반전 = 원본(비트 일치)
#       3) DB Back 저장 -> 확정 OK / GetMapInfo VIEW_SIDE = BACK / PLACED 재조회 좌표 = 반전 좌표 (원본 행 SOURCE는 2026-10-12 제거)
#       4) DB(Back) 도형으로 Chain 탐색한 연결 집합 = 파일(Front) 도형 탐색 집합
#       5) 기존 11개 항목 BeginMapImport는 VIEW_SIDE = FRONT(호환)
#       Service를 띄우지 않고 BSL을 직접 호출한다(DB: 로컬 docker DMSMGR, TQP_GDS_MAP.VIEW_SIDE 컬럼 필요).
# 결과: PROBE / BACK_PROBE(Back, Layer 2·6) / PROBE / FRONT_COMPAT_PROBE를 만들었다가 -Keep이 없으면 삭제한다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsImportBackSideProbe.ps1 -GdsExe <EXE> [-BslDir <BSL 폴더>] [-Keep]
param(
    [Parameter(Mandatory = $true)][string]$GdsExe,
    [string]$BslDir = '',
    [string]$GdsPath = '',
    [int[]]$Layers = @(2, 6),
    [switch]$Keep
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$serverBin = Join-Path $dms '101_ServerBin'
if ($BslDir -eq '') { $BslDir = $serverBin }
if ($GdsPath -eq '') { $GdsPath = Join-Path (Split-Path $dms -Parent) 'SampleFile\OMM all layer2.gds\OMM all layer2.gds' }
[AppDomain]::CurrentDomain.SetData('APP_CONFIG_FILE', (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config'))
Add-Type -AssemblyName System.Configuration
$flags = [Reflection.BindingFlags]'NonPublic,Static'
$cm = [Configuration.ConfigurationManager]
$cm.GetField('s_initState', $flags).SetValue($null, 0)
$cm.GetField('s_configSystem', $flags).SetValue($null, $null)
foreach ($d in 'Oracle.ManagedDataAccess', 'Miracom.Middleware', 'DACrux.Base') {
    $p = Join-Path $serverBin "$d.dll"; if (Test-Path $p) { [void][Reflection.Assembly]::LoadFrom($p) }
}
# Interface / DSL 은 BSL 과 같은 폴더에서 읽는다(시험 빌드의 서비스 계약이 운영 폴더와 다를 수 있음, 2026-10-12)
foreach ($d in 'DACrux.SEMDMS.Interface', 'DACrux.SEMDMS.DSL') { [void][Reflection.Assembly]::LoadFrom((Join-Path $BslDir "$d.dll")) }
$bslDll = Join-Path $BslDir 'DACrux.SEMDMS.BSL.dll'
[void][Reflection.Assembly]::LoadFrom($bslDll)
Write-Host "BSL: $bslDll"
[void][Reflection.Assembly]::LoadFrom((Join-Path $dms '100_ClientBin\DACrux.SEMDMS.RO.dll'))
$probeDir = Join-Path $env:TEMP 'chippac\gdsbacksideprobe'
New-Item -ItemType Directory -Force $probeDir | Out-Null
Copy-Item $GdsExe (Join-Path $probeDir 'NexplantQMS.GdsMap.dll') -Force
foreach ($d in 'OpenTK.dll', 'OpenTK.GLControl.dll') { Copy-Item (Join-Path (Split-Path $GdsExe) $d) $probeDir -Force }
$gdsDll = Join-Path $probeDir 'NexplantQMS.GdsMap.dll'
[void][Reflection.Assembly]::LoadFrom((Join-Path $probeDir 'OpenTK.dll'))
[void][Reflection.Assembly]::LoadFrom((Join-Path $probeDir 'OpenTK.GLControl.dll'))
[void][Reflection.Assembly]::LoadFrom($gdsDll)
Add-Type -AssemblyName System.Windows.Forms

$helper = @"
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using NexplantQMS.GdsMap;
using NexplantQMS.GdsMap.Chain;
using NexplantQMS.GdsMap.Persistence;

public static class BackSideProbe
{
    public static GdsLibrary Lib;
    public static GdsMapControl FileMap;

    public static void Load(string path)
    {
        Lib = new GdsReader { LengthUnit = LengthUnit.Micrometer }.Read(path);
        FileMap = new GdsMapControl();
        FileMap.ShowStructure(Lib);
    }

    static string Hash(GdsMapControl map, ISet<int> layers, bool mirror)
    {
        var sb = new StringBuilder();
        map.VisitPlacedElements(layers, mirror, d =>
        {
            sb.Append(d.PlacedElementId).Append(';');
            foreach (GPoint p in d.WorldPoints) sb.Append(BitConverter.DoubleToInt64Bits(p.X)).Append(',').Append(BitConverter.DoubleToInt64Bits(p.Y)).Append(' ');
        });
        using (var sha = SHA256.Create()) return BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString())));
    }

    /// <summary>1·2) 반전식 / 불변 / 두 번 반전. 반환: 도형수|점수|X불일치|Y불일치|Bounds불일치|원본불변|두번반전일치</summary>
    public static string CheckMirror(int[] layerArr)
    {
        var layers = new HashSet<int>(layerArr);
        string before = Hash(FileMap, layers, false);
        var front = new Dictionary<string, MapPlacedElementData>(StringComparer.Ordinal);
        FileMap.VisitPlacedElements(layers, false, d => front[d.PlacedElementId] = d);
        long items = 0, points = 0, badX = 0, badY = 0, badBounds = 0;
        FileMap.VisitPlacedElements(layers, true, d =>
        {
            MapPlacedElementData f = front[d.PlacedElementId];
            items++;
            if (d.WorldPoints.Length != f.WorldPoints.Length) { badX++; return; }
            for (int i = 0; i < d.WorldPoints.Length; i++)
            {
                points++;
                if (BitConverter.DoubleToInt64Bits(d.WorldPoints[i].X) != BitConverter.DoubleToInt64Bits(-f.WorldPoints[i].X)) badX++;
                if (BitConverter.DoubleToInt64Bits(d.WorldPoints[i].Y) != BitConverter.DoubleToInt64Bits(f.WorldPoints[i].Y)) badY++;
            }
            if (d.Bounds.MinX != -f.Bounds.MaxX || d.Bounds.MaxX != -f.Bounds.MinX || d.Bounds.MinY != f.Bounds.MinY || d.Bounds.MaxY != f.Bounds.MaxY) badBounds++;
        });
        string after = Hash(FileMap, layers, false);
        MethodInfo mirror = typeof(GdsMapControl).GetMethod("MirrorPoints", BindingFlags.NonPublic | BindingFlags.Static);
        long twiceBad = 0;
        foreach (MapPlacedElementData f in front.Values)
        {
            var twice = (GPoint[])mirror.Invoke(null, new object[] { mirror.Invoke(null, new object[] { f.WorldPoints }) });
            for (int i = 0; i < twice.Length; i++)
                if (BitConverter.DoubleToInt64Bits(twice[i].X) != BitConverter.DoubleToInt64Bits(f.WorldPoints[i].X)) twiceBad++;
        }
        return items + "|" + points + "|" + badX + "|" + badY + "|" + badBounds + "|" + (before == after) + "|" + (twiceBad == 0);
    }

    static string[,] To2D(List<string[]> rows)
    {
        int cols = rows.Count == 0 ? 0 : rows[0].Length;
        var a = new string[rows.Count, cols];
        for (int i = 0; i < rows.Count; i++) for (int c = 0; c < cols; c++) a[i, c] = rows[i][c];
        return a;
    }

    /// <summary>3) Setup DB 저장과 같은 순서로 Back side 저장. 반환: job|확정결과|VIEW_SIDE|DB PLACED|예상 PLACED</summary>
    public static string SaveBack(string factory, string device, int[] layerArr)
    {
        var layers = new HashSet<int>(layerArr);
        var bsl = new DACrux.SEMDMS.BSL.GdsMapImport();
        if (bsl.GetMapInfo(factory, device).Rows.Count > 0) bsl.DeleteMap(factory, device, "probe");
        long placed = 0, pts = 0;
        FileMap.VisitPlacedElements(layers, true, d => { placed++; pts += d.WorldPoints.Length; });
        string[] info = { factory, device, Lib.Structures[0].Name, "probe.gds", new string('B', 64), "0.001", "1E-09", "probe",
            "", placed.ToString(), pts.ToString(), "BACK" };
        string job = bsl.BeginMapImport(info).Rows[0]["IMPORT_JOB_SEQ"].ToString();
        var colors = FileMap.GetLayerDisplayItems().Where(i => layers.Contains(i.LayerKey)).OrderBy(i => i.LayerKey)
            .Select(i => new KeyValuePair<int, int>(i.LayerKey, i.Color.ToArgb())).ToList();
        bsl.CreateLayerBatch(job, 0, GdsMapLayerRowBuilder.BuildLayerKeyRows(colors), "probe");
        long no = 0;
        var pending = new List<string[]>(); no = 0;
        FileMap.VisitPlacedElements(layers, true, d =>
        {
            pending.Add(GdsMapPlacedRowBuilder.BuildRow(d));
            if (pending.Count == 10000) { bsl.CreatePlacedBatch(job, no++, To2D(pending), "probe"); pending.Clear(); }
        });
        if (pending.Count > 0) bsl.CreatePlacedBatch(job, no++, To2D(pending), "probe");
        DataRow c = bsl.CompleteMapImport(job, "probe").Rows[0];
        DataRow m = bsl.GetMapInfo(factory, device).Rows[0];
        return job + "|" + c["RESULT"] + "|" + m["VIEW_SIDE"] + "|" + c["DB_PLACED"] + "|" + placed;
    }

    public static GdsMapControl DbMap(string factory, string device, int[] layers)
    {
        var view = new DACrux.SEMDMS.BSL.GdsMapView();
        var items = new List<MapPlacedElementData>();
        foreach (int layer in layers)
        {
            string after = string.Empty;
            while (true)
            {
                DataTable page = view.GetPlacedElementPage(factory, device, GdsLayerKey.Format(layer), after, 10000);
                foreach (DataRow row in page.Rows) items.Add(GdsMapPlacedRowReader.Read(row));
                if (page.Rows.Count > 0) after = page.Rows[page.Rows.Count - 1]["PLACED_ELEMENT_ID"].ToString();
                if (page.Rows.Count < 10000) break;
            }
        }
        var map = new GdsMapControl();
        map.ShowPlacedElements("TOP", layers.ToDictionary(id => id, id => Color.Gray), items);
        return map;
    }

    /// <summary>3) DB 재조회 좌표 = 반전 좌표(비트). 반환: DB 도형 수|불일치 수</summary>
    public static string CompareDb(GdsMapControl db, int[] layerArr)
    {
        var layers = new HashSet<int>(layerArr);
        var expected = new Dictionary<string, MapPlacedElementData>(StringComparer.Ordinal);
        FileMap.VisitPlacedElements(layers, true, d => expected[d.PlacedElementId] = d);
        long count = 0, bad = 0;
        db.VisitPlacedElements(layers, false, d =>
        {
            count++;
            MapPlacedElementData e;
            if (!expected.TryGetValue(d.PlacedElementId, out e) || e.WorldPoints.Length != d.WorldPoints.Length) { bad++; return; }
            for (int i = 0; i < d.WorldPoints.Length; i++)
                if (BitConverter.DoubleToInt64Bits(d.WorldPoints[i].X) != BitConverter.DoubleToInt64Bits(e.WorldPoints[i].X)
                    || BitConverter.DoubleToInt64Bits(d.WorldPoints[i].Y) != BitConverter.DoubleToInt64Bits(e.WorldPoints[i].Y)) { bad++; return; }
        });
        return count + "|" + bad;
    }

    /// <summary>4) Front 파일 도형 / Back DB 도형에서 같은 Input으로 탐색한 방문 ID 집합 비교. 반환: Input|Front 수|Back 수|같음</summary>
    public static string TraceCompare(GdsMapControl db, int layer)
    {
        string input = null;
        db.VisitPlacedElements(new HashSet<int> { layer }, false, d => { if (input == null && (d.ElementType == "BOUNDARY" || d.ElementType == "PATH")) input = d.PlacedElementId; });
        Func<GdsMapControl, List<string>> run = m => m.TraceChainCandidate(new[] { input }, new string[0], new[] { layer },
            new ChainLayerConnectionRule[0], false, 10.0).VisitedElements.Select(e => e.ElementKey).OrderBy(k => k, StringComparer.Ordinal).ToList();
        List<string> f = run(FileMap), b = run(db);
        return input + "|" + f.Count + "|" + b.Count + "|" + f.SequenceEqual(b);
    }

    /// <summary>5) 기존 11개 항목 호출은 FRONT. 헤더만 만들고 VIEW_SIDE 확인 후 삭제.</summary>
    public static string Compat(string factory, string device)
    {
        var bsl = new DACrux.SEMDMS.BSL.GdsMapImport();
        if (bsl.GetMapInfo(factory, device).Rows.Count > 0) bsl.DeleteMap(factory, device, "probe");
        bsl.BeginMapImport(new[] { factory, device, "TOP", "compat.gds", new string('C', 64), "0.001", "1E-09", "probe", "", "0", "0" });
        string side = bsl.GetMapInfo(factory, device).Rows[0]["VIEW_SIDE"].ToString();
        bsl.DeleteMap(factory, device, "probe");
        return side;
    }
}
"@
Add-Type -TypeDefinition $helper -ReferencedAssemblies @($gdsDll, $bslDll, (Join-Path $BslDir 'DACrux.SEMDMS.Interface.dll'),
    (Join-Path $serverBin 'Miracom.Middleware.dll'), (Join-Path $probeDir 'OpenTK.GLControl.dll'), (Join-Path $probeDir 'OpenTK.dll'),
    'System.Core', 'System.Data', 'System.Windows.Forms', 'System.Drawing', 'System.Xml')

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
$f = 'PROBE'; $dev = 'BACK_PROBE'

[BackSideProbe]::Load($GdsPath)
# -Layers 는 GDS Layer 번호(DataType 0). 화면 / 저장 / 조회는 Layer 키(번호 + DataType)로 다룬다 (2026-10-20)
$Layers = [int[]]@($Layers | ForEach-Object { [NexplantQMS.GdsMap.GdsLayerKey]::Make($_, 0) })
$m = ([BackSideProbe]::CheckMirror($Layers)).Split('|')
Check '반전식 X'' = -X / Y / Bounds (비트)' ([long]$m[0] -gt 0 -and $m[2] -eq '0' -and $m[3] -eq '0' -and $m[4] -eq '0') "도형 $($m[0]) / 좌표 $($m[1]) / X 불일치 $($m[2]) / Y 불일치 $($m[3]) / Bounds 불일치 $($m[4])"
Check '반전 추출 후 화면 도형(원본 공유 배열) 불변 / 두 번 반전 = 원본' ($m[5] -eq 'True' -and $m[6] -eq 'True') "불변=$($m[5]) 두번반전=$($m[6])"

$sw = [Diagnostics.Stopwatch]::StartNew()
$s = ([BackSideProbe]::SaveBack($f, $dev, $Layers)).Split('|')
Check 'Back 저장 확정 / VIEW_SIDE / 건수' ($s[1] -eq 'OK' -and $s[2] -eq 'BACK' -and $s[3] -eq $s[4]) ("job={0} {1} VIEW_SIDE={2} PLACED {3}/{4} / {5:N0} ms" -f $s[0], $s[1], $s[2], $s[3], $s[4], $sw.ElapsedMilliseconds)
$db = [BackSideProbe]::DbMap($f, $dev, $Layers)
$c = ([BackSideProbe]::CompareDb($db, $Layers)).Split('|')
Check 'DB 재조회 좌표 = 반전 좌표(비트)' ($c[0] -eq $s[4] -and $c[1] -eq '0') "DB 도형 $($c[0]) / 불일치 $($c[1])"
$t = ([BackSideProbe]::TraceCompare($db, $Layers[0])).Split('|')
Check 'Back DB 도형 Chain 탐색 = Front 파일 도형 탐색' ($t[3] -eq 'True' -and [int]$t[1] -gt 0) "Input $($t[0]) / Front $($t[1]) / Back $($t[2])"
$side = [BackSideProbe]::Compat($f, 'FRONT_COMPAT_PROBE')
Check '기존 11개 항목 호출 = FRONT' ($side -eq 'FRONT') "VIEW_SIDE=$side"

if (-not $Keep) { [void](New-Object DACrux.SEMDMS.BSL.GdsMapImport).DeleteMap($f, $dev, 'probe'); Write-Host "정리: $f / $dev 삭제" }
$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host 'GDS_IMPORT_BACKSIDE_PROBE=PASS' } else { Write-Host "GDS_IMPORT_BACKSIDE_PROBE=FAIL ($fail)"; exit 1 }
