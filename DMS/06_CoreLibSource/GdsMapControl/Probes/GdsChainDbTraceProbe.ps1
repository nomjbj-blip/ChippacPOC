# GDS Map View 화면의 Chain 탐색 전제 검증 스크립트 (1단계, 설계: 문서/설계/02_GDS/2026-10-06_GDS_Map_화면역할_재정의_설계.md 3절)
# 목적: Chain 설정을 View(DB 도형) 화면으로 옮겨도 탐색 결과가 같은지 확인한다.
#       같은 GDS를 (1) 파일 Flatten(ShowStructure)과 (2) DB 페이지 조회 + ShowPlacedElements로 각각 그린 뒤
#       같은 Input / Layer / 격자로 TraceChainCandidate를 실행해 방문 / 후보 도형 ID 집합을 비교한다.
#       또한 두 화면을 생성해 Designer 구성이 실행 시 오류 없이 만들어지는지(Setup에 Chain 컨트롤 없음 / View에 있음) 확인한다.
# 전제: FACTORY=PROBE / DEVICE_ID=LAYER_FILTER_PROBE READY Map(Layer 1, 2, 6)이 있어야 한다(GdsImportLayerFilterProbe.ps1 -Keep).
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsChainDbTraceProbe.ps1 -GdsExe <빌드한 NexplantQMS.GdsMap.exe> [-GdsPath 경로]
param(
    [Parameter(Mandatory = $true)][string]$GdsExe,
    [string]$GdsPath = '',
    [string]$Factory = 'PROBE',
    [string]$Device = 'LAYER_FILTER_PROBE',
    [int[]]$Layers = @(1, 2, 6),
    [string]$BinDir = ''
)
# -Layers 는 GDS Layer 번호(DataType 0), -BinDir 생략 시 101_ServerBin. 화면 / 탐색은 Layer 키(번호 + DataType)로 다룬다 (2026-10-20)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$serverBin = Join-Path $dms '101_ServerBin'
if ($BinDir -eq '') { $BinDir = $serverBin }
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
foreach ($d in 'DACrux.SEMDMS.Interface', 'DACrux.SEMDMS.DSL', 'DACrux.SEMDMS.BSL') { [void][Reflection.Assembly]::LoadFrom((Join-Path $BinDir "$d.dll")) }
Write-Host "BSL: $(Join-Path $BinDir 'DACrux.SEMDMS.BSL.dll')"
# RO(화면 참조)는 ClientBin에서 로드한다. 이 Probe는 RO를 호출하지 않지만 Form 형식 로드에 필요하다.
foreach ($d in 'DACrux.SEMDMS.RO') { [void][Reflection.Assembly]::LoadFrom((Join-Path $dms "100_ClientBin\$d.dll")) }
$probeDir = Join-Path $env:TEMP 'chippac\gdschaindbtraceprobe'
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
using System.Linq;
using NexplantQMS.GdsMap;
using NexplantQMS.GdsMap.Chain;
using NexplantQMS.GdsMap.Persistence;

public static class ChainDbTraceProbe
{
    public static GdsMapControl FileMap(string path)
    {
        var lib = new GdsReader { LengthUnit = LengthUnit.Micrometer }.Read(path);
        var map = new GdsMapControl();
        map.ShowStructure(lib);
        return map;
    }

    /// <summary>View 화면과 같은 방식: Layer별 PLACED 페이지 조회 -> 행 해석 -> ShowPlacedElements.</summary>
    public static GdsMapControl DbMap(string factory, string device, int[] layers)
    {
        var bsl = new DACrux.SEMDMS.BSL.GdsMapView();
        var items = new List<MapPlacedElementData>();
        foreach (int layer in layers)
        {
            string after = string.Empty;
            while (true)
            {
                DataTable page = bsl.GetPlacedElementPage(factory, device, GdsLayerKey.Format(layer), after, 10000);
                foreach (DataRow row in page.Rows) items.Add(GdsMapPlacedRowReader.Read(row));
                if (page.Rows.Count > 0) after = page.Rows[page.Rows.Count - 1]["PLACED_ELEMENT_ID"].ToString();
                if (page.Rows.Count < 10000) break;
            }
        }
        var colors = layers.ToDictionary(id => id, id => Color.Gray);
        var map = new GdsMapControl();
        map.ShowPlacedElements("TOP", colors, items);
        return map;
    }

    /// <summary>Layer마다 BOUNDARY / PATH 도형 하나를 Input 후보로 고른다(파일 / DB 공통 ID).</summary>
    public static List<string> PickInputs(GdsMapControl map, int[] layers)
    {
        var picked = new Dictionary<int, string>();
        map.VisitPlacedElements(new HashSet<int>(layers), d =>
        {
            if (!picked.ContainsKey(d.LayerId) && (d.ElementType == "BOUNDARY" || d.ElementType == "PATH"))
                picked[d.LayerId] = d.PlacedElementId;
        });
        return picked.OrderBy(p => p.Key).Select(p => p.Value).ToList();
    }

    /// <summary>
    /// 2단계: 같은 Map을 다른 Layer로 다시 그린 뒤 RebuildChainTraceItems로 이전 Chain 도형 ID가 새 도형에 연결되는지 확인한다.
    /// keys는 첫 조회 Layer들에서 고른 ID. 반환: 다시 그리기 전 연결 수 | 다시 그린 뒤(재연결 전) | 재연결 후 | 재연결 후 기대 수(다시 그린 Layer에 있는 ID 수)
    /// </summary>
    public static string Reattach(string factory, string device, int[] first, int[] second)
    {
        GdsMapControl map = DbMap(factory, device, first);
        var keys = new List<string>();
        var layerOf = new Dictionary<string, int>();
        map.VisitPlacedElements(new HashSet<int>(first), d =>
        {
            int layerKey = GdsLayerKey.Make(d.LayerId, d.DataType);
            if ((d.ElementType == "BOUNDARY" || d.ElementType == "PATH") && keys.Count(k => layerOf[k] == layerKey) < 5)
            { keys.Add(d.PlacedElementId); layerOf[d.PlacedElementId] = layerKey; }
        });
        map.RebuildChainTraceItems();
        int before = map.GetChainTraceElements(keys).Count;
        GdsMapControl again = DbMap(factory, device, second);
        // 화면과 같은 순서: 새 지도(다른 Layer 조합) -> 오버레이 초기화 -> 재연결
        int afterReload = again.GetChainTraceElements(keys).Count;
        again.ResetChainSetupOverlay();
        again.RebuildChainTraceItems();
        int afterReattach = again.GetChainTraceElements(keys).Count;
        int expected = keys.Count(k => second.Contains(layerOf[k]));
        return before + "|" + afterReload + "|" + afterReattach + "|" + expected + "|" + keys.Count;
    }

    public static string Trace(GdsMapControl map, string input, int[] layers)
    {
        ChainTraceResult r = map.TraceChainCandidate(new[] { input }, new string[0], layers, new ChainLayerConnectionRule[0], false, 10.0);
        var visited = r.VisitedElements.Select(e => e.ElementKey).OrderBy(k => k, StringComparer.Ordinal);
        var candidate = r.CandidateElements.Select(e => e.ElementKey).OrderBy(k => k, StringComparer.Ordinal);
        return r.IsConnected + "|" + r.VisitedElements.Count + "|" + r.CandidateElements.Count + "|"
            + string.Join(",", visited) + "#" + string.Join(",", candidate);
    }
}
"@
Add-Type -TypeDefinition $helper -ReferencedAssemblies @($gdsDll, (Join-Path $BinDir 'DACrux.SEMDMS.BSL.dll'), (Join-Path $BinDir 'DACrux.SEMDMS.Interface.dll'),
    (Join-Path $serverBin 'Miracom.Middleware.dll'), (Join-Path $probeDir 'OpenTK.GLControl.dll'), (Join-Path $probeDir 'OpenTK.dll'),
    'System.Core', 'System.Data', 'System.Windows.Forms', 'System.Drawing', 'System.Xml')

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}

# 1. 화면 생성(Designer 실행) 스모크
$inst = [Reflection.BindingFlags]'NonPublic,Instance'
$setup = New-Object NexplantQMS.GdsMap.GdsMapForm($false)
$view = New-Object NexplantQMS.GdsMap.GdsMapViewForm($false)
$setupChain = [NexplantQMS.GdsMap.GdsMapForm].GetField('_chainList', $inst)
$viewChain = [NexplantQMS.GdsMap.GdsMapViewForm].GetField('_chainList', $inst)
$viewSave = [NexplantQMS.GdsMap.GdsMapViewForm].GetField('_btnChainDbSave', $inst).GetValue($view)
Check '화면 생성: Setup에 Chain 없음 / View에 Chain 있음' ($setupChain -eq $null -and $viewChain -ne $null -and $viewChain.GetValue($view) -ne $null -and -not $viewSave.Visible) "View Chain 저장 버튼 단독 실행 숨김=$(-not $viewSave.Visible)"
$setup.Dispose(); $view.Dispose()

# 2. 파일 도형 vs DB 도형 탐색 결과 비교
$Layers = [int[]]@($Layers | ForEach-Object { [NexplantQMS.GdsMap.GdsLayerKey]::Make($_, 0) })
$sw = [Diagnostics.Stopwatch]::StartNew()
$fileMap = [ChainDbTraceProbe]::FileMap($GdsPath)
$fileMs = $sw.ElapsedMilliseconds; $sw.Restart()
$dbMap = [ChainDbTraceProbe]::DbMap($Factory, $Device, $Layers)
$dbMs = $sw.ElapsedMilliseconds
$inputs = [ChainDbTraceProbe]::PickInputs($dbMap, $Layers)
Check 'Input 후보' ($inputs.Count -ge 1) ("{0}개 / 파일 {1:N0} ms / DB {2:N0} ms" -f $inputs.Count, $fileMs, $dbMs)
foreach ($input in $inputs) {
    $a = [ChainDbTraceProbe]::Trace($fileMap, $input, $Layers)
    $b = [ChainDbTraceProbe]::Trace($dbMap, $input, $Layers)
    $pa = $a.Split('|');
    Check "탐색 결과 일치 ($input)" ($a -eq $b) ("파일 visited={0} candidate={1} / DB {2}" -f $pa[1], $pa[2], ($(if ($a -eq $b) { '동일' } else { $b.Split('|')[1] + '/' + $b.Split('|')[2] })))
}

# 3. (2단계) 다른 Layer 조합으로 다시 조회한 뒤 Chain 도형 ID 재연결
if ($Layers.Count -ge 2) {
    $first = [int[]]@($Layers[0], $Layers[1]); $second = [int[]]@($Layers[0]) + @($Layers | Select-Object -Skip 2)
    $r = ([ChainDbTraceProbe]::Reattach($Factory, $Device, $first, $second)).Split('|')
    Check '다시 조회 후 Chain 도형 재연결' ([int]$r[0] -eq [int]$r[4] -and [int]$r[2] -eq [int]$r[3] -and [int]$r[3] -gt 0) ("첫 조회 Layer {0} 연결 {1}/{5} -> Layer {2} 다시 조회 직후(새 지도) {3} -> 재연결 {4} (기대 {6})" -f (($first | ForEach-Object { [NexplantQMS.GdsMap.GdsLayerKey]::Format($_) }) -join ','), $r[0], (($second | ForEach-Object { [NexplantQMS.GdsMap.GdsLayerKey]::Format($_) }) -join ','), $r[1], $r[2], $r[4], $r[3])
}

$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host 'GDS_CHAIN_DB_TRACE_PROBE=PASS' } else { Write-Host "GDS_CHAIN_DB_TRACE_PROBE=FAIL ($fail)"; exit 1 }
