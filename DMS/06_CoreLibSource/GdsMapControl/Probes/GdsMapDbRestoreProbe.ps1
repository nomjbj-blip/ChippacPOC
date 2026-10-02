# GDS Map DB 복원 검증 스크립트 (CLAUDE-015 작성 / CLAUDE-018 Factory + Device 키로 변경)
# 목적: 같은 GDS 파일을 (1) 파일 Flatten(ShowStructure)과 (2) DB 조회(GdsMapView 페이지 -> GdsMapPlacedRowReader -> ShowPlacedElements)로
#       각각 GdsMapControl에 올린 뒤, 화면 도형을 VisitPlacedElements로 꺼내 도형 단위로 대조한다.
#       ID / Layer / DataType / 종류 / 닫힘 / PATH 폭·타입 / Bounds / TEXT 속성 / 좌표(double 비트 단위)가 모두 같아야 PASS.
#       단계별 시간(DB 조회 / 행 해석 / 도면 구성)도 측정한다.
# 대상: BSL GdsMapView / GdsMapImport(Service 없이 직접 호출) + GdsMapControl\bin\Debug EXE / 로컬 docker DMSMGR
# 결과: FACTORY / DEVICE_ID(기본 PROBE / VIEW_PROBE)에 READY Map이 없거나 다른 GDS면 지우고 이 GDS로 저장한다(시험 DB DML).
#       READY Map이 같은 GDS(SHA-256)로 있으면 저장하지 않고 조회만 한다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsMapDbRestoreProbe.ps1 [-GdsPath 경로] [-Factory PROBE] [-Device VIEW_PROBE] [-BslDir 폴더] [-PageRows 10000]
param(
    [string]$GdsPath = '',
    [string]$Factory = 'PROBE',
    [string]$Device = 'VIEW_PROBE',
    [string]$BslDir = '',
    [int]$PageRows = 10000,
    [int]$BatchRows = 2000
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$serverBin = Join-Path $dms '101_ServerBin'
if ($BslDir -eq '') { $BslDir = $serverBin }
$gdsExe = Join-Path $dms '06_CoreLibSource\GdsMapControl\bin\Debug\NexplantQMS.GdsMap.exe'
if ($GdsPath -eq '') { $GdsPath = Join-Path (Split-Path $dms -Parent) 'SampleFile\OMM all layer2.gds\OMM all layer2.gds' }

[AppDomain]::CurrentDomain.SetData('APP_CONFIG_FILE', (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config'))
Add-Type -AssemblyName System.Configuration
$flags = [Reflection.BindingFlags]'NonPublic,Static'
$cm = [Configuration.ConfigurationManager]
$cm.GetField('s_initState', $flags).SetValue($null, 0)
$cm.GetField('s_configSystem', $flags).SetValue($null, $null)
foreach ($d in 'Oracle.ManagedDataAccess', 'Miracom.Middleware') { [void][Reflection.Assembly]::LoadFrom((Join-Path $serverBin "$d.dll")) }
foreach ($d in 'DACrux.SEMDMS.Interface', 'DACrux.SEMDMS.DSL', 'DACrux.SEMDMS.BSL') { [void][Reflection.Assembly]::LoadFrom((Join-Path $BslDir "$d.dll")) }
# Add-Type(CodeDom)은 .exe를 참조로 받지 못하므로 시험용 임시 폴더에 같은 어셈블리를 .dll 이름으로 복사해 참조 / 로드한다.
$probeDir = Join-Path $env:TEMP 'claude\gdsdbrestoreprobe'
New-Item -ItemType Directory -Force $probeDir | Out-Null
Copy-Item $gdsExe (Join-Path $probeDir 'NexplantQMS.GdsMap.dll') -Force
foreach ($d in 'OpenTK.dll', 'OpenTK.GLControl.dll') { Copy-Item (Join-Path (Split-Path $gdsExe) $d) $probeDir -Force }
$gdsDll = Join-Path $probeDir 'NexplantQMS.GdsMap.dll'
[void][Reflection.Assembly]::LoadFrom((Join-Path $probeDir 'OpenTK.dll'))
[void][Reflection.Assembly]::LoadFrom((Join-Path $probeDir 'OpenTK.GLControl.dll'))
[void][Reflection.Assembly]::LoadFrom($gdsDll)
Add-Type -AssemblyName System.Windows.Forms

# 60만 건 저장 / 비교는 C#에서 처리한다(PowerShell 5.1 CodeDom = C# 5 문법).
$helper = @"
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using NexplantQMS.GdsMap;
using NexplantQMS.GdsMap.Persistence;

public static class GdsDbRestoreProbe
{
    public static GdsLibrary Parse(string path)
    {
        var reader = new GdsReader { LengthUnit = LengthUnit.Micrometer };
        return reader.Read(path);
    }

    /// <summary>
    /// 화면 저장(GdsMapForm_DbSave)과 같은 순서로 저장한다: Begin -> LAYER -> SOURCE -> PLACED -> Complete.
    /// LAYER 색상은 화면 Layer 색상, 원본에만 있는 Layer는 회색. 반환: 확정 결과 문자열
    /// </summary>
    public static string Import(DACrux.SEMDMS.BSL.GdsMapImport bsl, string[] info, GdsLibrary lib, GdsMapControl map, int batchRows)
    {
        var sw = Stopwatch.StartNew();
        string job = bsl.BeginMapImport(info).Rows[0]["IMPORT_JOB_SEQ"].ToString();
        var colors = new Dictionary<int, int>();
        foreach (var item in map.GetLayerDisplayItems()) colors[item.LayerId] = item.Color.ToArgb();
        var pairs = new List<KeyValuePair<int, int>>();
        foreach (int id in GdsMapSourceRowBuilder.CollectLayerIds(lib))
            pairs.Add(new KeyValuePair<int, int>(id, colors.ContainsKey(id) ? colors[id] : Color.Gray.ToArgb()));
        bsl.CreateLayerBatch(job, 0, GdsMapSourceRowBuilder.BuildLayerRows(pairs), "probe");
        long no = 0;
        foreach (string[,] batch in GdsMapSourceRowBuilder.BuildSourceBatches(lib, batchRows))
            bsl.CreateSourceBatch(job, no++, batch, "probe");
        no = 0;
        var pending = new List<string[]>(batchRows);
        Action flush = () =>
        {
            var arr = new string[pending.Count, GdsMapPlacedRowBuilder.ColumnCount];
            for (int i = 0; i < pending.Count; i++)
                for (int c = 0; c < GdsMapPlacedRowBuilder.ColumnCount; c++) arr[i, c] = pending[i][c];
            bsl.CreatePlacedBatch(job, no++, arr, "probe");
            pending.Clear();
        };
        map.VisitPlacedElements(d => { pending.Add(GdsMapPlacedRowBuilder.BuildRow(d)); if (pending.Count == batchRows) flush(); });
        if (pending.Count > 0) flush();
        DataRow r = bsl.CompleteMapImport(job, "probe").Rows[0];
        return r["RESULT"] + " / job=" + job + " / " + r["MESSAGE"] + " / " + (sw.ElapsedMilliseconds / 1000.0).ToString("0.0") + "초";
    }

    /// <summary>도형 한 건의 모든 저장 대상 값을 비교용 문자열로 만든다. double은 비트 값(Int64)으로 비교한다.</summary>
    static string Signature(MapPlacedElementData d)
    {
        var sb = new StringBuilder(64 + d.WorldPoints.Length * 34);
        sb.Append(d.SourceElementId).Append('|').Append(d.LayerId).Append('|').Append(d.DataType).Append('|')
          .Append(d.ElementType).Append('|').Append(d.Closed ? 1 : 0).Append('|')
          .Append(Bits(d.PathWidth)).Append('|').Append(d.PathType).Append('|')
          .Append(Bits(d.Bounds.MinX)).Append(',').Append(Bits(d.Bounds.MinY)).Append(',')
          .Append(Bits(d.Bounds.MaxX)).Append(',').Append(Bits(d.Bounds.MaxY)).Append('|')
          .Append(d.Text).Append('|').Append(d.TextType).Append('|').Append(d.TextFont).Append('|')
          .Append(d.TextHorizontal).Append('|').Append(d.TextVertical).Append('|');
        foreach (GPoint p in d.WorldPoints) sb.Append(Bits(p.X)).Append(',').Append(Bits(p.Y)).Append(';');
        return sb.ToString();
    }

    static long Bits(double v) { return BitConverter.DoubleToInt64Bits(v); }

    /// <summary>현재 화면 도형을 ID -> 서명 사전으로 꺼낸다. 좌표 배열은 보관하지 않아 두 도면을 동시에 들고 있지 않는다.</summary>
    public static Dictionary<string, string> Capture(GdsMapControl map, out long points)
    {
        var dic = new Dictionary<string, string>(StringComparer.Ordinal);
        long p = 0;
        map.VisitPlacedElements(d => { dic.Add(d.PlacedElementId, Signature(d)); p += d.WorldPoints.Length; });
        points = p;
        return dic;
    }

    /// <summary>
    /// 화면 조회와 같은 순서로 DB 도면을 읽는다: Layer 목록 -> Layer별 페이지 -> 행 해석.
    /// 반환: [0] DB 조회 ms / [1] 행 해석 ms / [2] 페이지 수
    /// </summary>
    public static double[] LoadFromDb(string factory, string device, int pageRows, Dictionary<int, Color> layerColors, List<MapPlacedElementData> items)
    {
        var view = new DACrux.SEMDMS.BSL.GdsMapView();
        double dbMs = 0, readMs = 0; int pages = 0;
        var sw = Stopwatch.StartNew();
        DataTable layers = view.GetMapLayerList(factory, device);
        dbMs += sw.Elapsed.TotalMilliseconds;
        foreach (DataRow l in layers.Rows)
        {
            int layerId = Convert.ToInt32(l["LAYER_ID"]);
            layerColors[layerId] = Color.FromArgb(Convert.ToInt32(l["COLOR_ARGB"]));
            string after = "";
            while (true)
            {
                sw.Restart();
                DataTable page = view.GetPlacedElementPage(factory, device, layerId.ToString(), after, pageRows);
                dbMs += sw.Elapsed.TotalMilliseconds; pages++;
                sw.Restart();
                foreach (DataRow row in page.Rows) items.Add(GdsMapPlacedRowReader.Read(row));
                readMs += sw.Elapsed.TotalMilliseconds;
                int n = page.Rows.Count;
                if (n > 0) after = page.Rows[n - 1]["PLACED_ELEMENT_ID"].ToString();
                page.Dispose();
                if (n < pageRows) break;
            }
        }
        return new double[] { dbMs, readMs, pages };
    }

    /// <summary>두 사전을 비교한다. 반환: [0] 파일에만 / [1] DB에만 / [2] 값 다름, first: 첫 차이 설명</summary>
    public static int[] Compare(Dictionary<string, string> file, Dictionary<string, string> db, out string first)
    {
        int onlyFile = 0, onlyDb = 0, diff = 0; first = "";
        foreach (var kv in file)
        {
            string other;
            if (!db.TryGetValue(kv.Key, out other)) { onlyFile++; if (first == "") first = "DB에 없음: " + kv.Key; }
            else if (other != kv.Value) { diff++; if (first == "") first = "값 다름: " + kv.Key + " file=" + Cut(kv.Value) + " db=" + Cut(other); }
        }
        foreach (var k in db.Keys) if (!file.ContainsKey(k)) { onlyDb++; if (first == "") first = "파일에 없음: " + k; }
        return new int[] { onlyFile, onlyDb, diff };
    }

    static string Cut(string s) { return s.Length > 160 ? s.Substring(0, 160) + "..." : s; }
}
"@
Add-Type -TypeDefinition $helper -ReferencedAssemblies @($gdsDll, (Join-Path $BslDir 'DACrux.SEMDMS.BSL.dll'), (Join-Path $BslDir 'DACrux.SEMDMS.Interface.dll'), (Join-Path $serverBin 'Miracom.Middleware.dll'),
    (Join-Path $probeDir 'OpenTK.GLControl.dll'), (Join-Path $probeDir 'OpenTK.dll'), 'System.Data', 'System.Windows.Forms', 'System.Drawing', 'System.Xml')

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
Write-Host "GDS: $GdsPath / Map: $Factory / $Device / BSL: $BslDir / 페이지 행 수: $PageRows"

# 1. 파일 경로
$sw = [Diagnostics.Stopwatch]::StartNew()
$lib = [GdsDbRestoreProbe]::Parse($GdsPath)
$map = New-Object NexplantQMS.GdsMap.GdsMapControl
$map.ShowStructure($lib)
$fileMs = $sw.ElapsedMilliseconds
$filePoints = 0L
$fileSig = [GdsDbRestoreProbe]::Capture($map, [ref]$filePoints)
$fileLayers = @($map.GetLayerDisplayItems() | ForEach-Object { $_.LayerId }) -join ','
$fileTexts = $map.GetTextLabelDiagnostics().Count
Write-Host ("파일: 도형 {0:N0} / 좌표 {1:N0} / Layer {2} / TEXT 라벨 {3} / 파싱+Flatten+버퍼 {4:N1}초" -f $fileSig.Count, $filePoints, $fileLayers, $fileTexts, ($fileMs / 1000.0))

# 2. 시험 Map 준비: 같은 GDS의 READY Map이 없으면 지우고 저장한다.
$sha = (Get-FileHash -Algorithm SHA256 -LiteralPath $GdsPath).Hash
$import = New-Object DACrux.SEMDMS.BSL.GdsMapImport
$info = $import.GetMapInfo($Factory, $Device)
$ready = $info.Rows.Count -eq 1 -and $info.Rows[0]['MAP_STATUS'] -eq 'READY' -and ([string]$info.Rows[0]['GDS_SHA256']).Trim() -ieq $sha
if (-not $ready) {
    if ($info.Rows.Count -gt 0) { Write-Host ("기존 Map 삭제: 도형 {0:N0}건" -f $import.DeleteMap($Factory, $Device, 'probe')) }
    $expectedSource = [NexplantQMS.GdsMap.Persistence.GdsMapSourceRowBuilder]::CountSourceRows($lib)
    $topName = $lib.Structures[0].Name
    $importInfo = [string[]]@($Factory, $Device, $topName, (Split-Path $GdsPath -Leaf), $sha,
        $lib.UserUnit.ToString('G17', [Globalization.CultureInfo]::InvariantCulture), $lib.DatabaseUnit.ToString('G17', [Globalization.CultureInfo]::InvariantCulture), 'probe',
        "$expectedSource", "$($fileSig.Count)", "$filePoints")
    $r = [GdsDbRestoreProbe]::Import($import, $importInfo, $lib, $map, $BatchRows)
    Check '시험 Map 저장' ($r.StartsWith('OK')) $r
} else {
    Write-Host '같은 GDS의 READY Map이 있어 저장을 건너뜁니다.'
}
$lib = $null

# 3. DB 경로 (화면과 같은 순서)
$colors = New-Object 'Collections.Generic.Dictionary[int,Drawing.Color]'
$items = New-Object 'Collections.Generic.List[NexplantQMS.GdsMap.Persistence.MapPlacedElementData]'
$t = [GdsDbRestoreProbe]::LoadFromDb($Factory, $Device, $PageRows, $colors, $items)
$sw.Restart()
$map.ShowPlacedElements('TOP', $colors, $items)
$showMs = $sw.ElapsedMilliseconds
$items = $null
$dbPoints = 0L
$dbSig = [GdsDbRestoreProbe]::Capture($map, [ref]$dbPoints)
$dbLayers = @($map.GetLayerDisplayItems() | ForEach-Object { $_.LayerId }) -join ','
$dbTexts = $map.GetTextLabelDiagnostics().Count
Write-Host ("DB  : 도형 {0:N0} / 좌표 {1:N0} / Layer {2} / TEXT 라벨 {3}" -f $dbSig.Count, $dbPoints, $dbLayers, $dbTexts)
Write-Host ("시간: DB 조회 {0:N1}초 ({1}페이지) / 행 해석 {2:N1}초 / 도면 구성(ShowPlacedElements) {3:N1}초 / 합계 {4:N1}초" -f ($t[0] / 1000), $t[2], ($t[1] / 1000), ($showMs / 1000.0), (($t[0] + $t[1] + $showMs) / 1000))

# 4. 대조
$first = ''
$c = [GdsDbRestoreProbe]::Compare($fileSig, $dbSig, [ref]$first)
Check '도형 수 / 좌표 수 일치' ($fileSig.Count -eq $dbSig.Count -and $filePoints -eq $dbPoints) "file=$($fileSig.Count)/$filePoints db=$($dbSig.Count)/$dbPoints"
Check '도형 단위 전체 값 일치 (ID / 속성 / Bounds / 좌표 비트)' ($c[0] -eq 0 -and $c[1] -eq 0 -and $c[2] -eq 0) "파일에만=$($c[0]) DB에만=$($c[1]) 다름=$($c[2]) $first"
Check 'Layer 목록 일치' ($fileLayers -eq $dbLayers) "file=$fileLayers db=$dbLayers"
Check 'TEXT 라벨 수 일치' ($fileTexts -eq $dbTexts) "file=$fileTexts db=$dbTexts"
Check '도면 Structure 지정 (Layer 표시 / Chain 탐색 전제)' ($map.Structure -ne $null -and $map.Structure.Layers.Count -eq $colors.Count) "layers=$($map.Structure.Layers.Count)"
$colorOk = $true
foreach ($li in $map.GetLayerDisplayItems()) { if ($colors.ContainsKey($li.LayerId) -and $li.Color.ToArgb() -ne $colors[$li.LayerId].ToArgb()) { $colorOk = $false } }
Check 'Layer 색상 = 저장 색상' $colorOk ''

$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host 'GDS_MAP_DB_RESTORE_PROBE=PASS' } else { Write-Host "GDS_MAP_DB_RESTORE_PROBE=FAIL ($fail)"; exit 1 }
