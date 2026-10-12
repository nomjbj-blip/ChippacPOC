# GDS Map DB 저장 / 화면에 표시된 Layer만 저장 검증 스크립트 (설계 21절)
# 목적: 실제 GDS를 파싱 -> GdsMapControl로 Flatten -> 일부 Layer만 선택해 화면 저장과 같은 오버로드
#       (CountSourceRows / BuildSourceBatches / VisitPlacedElements에 Layer 조건)로 LAYER / SOURCE / PLACED를 저장하고 확정까지 확인한다.
#       확인: 필터 건수 < 전체 건수 / SREF·AREF 원본 행은 모두 유지 / 저장 SOURCE ID가 전체 저장 때와 같은 ID·내용(순번 유지)
#             / 배치 도형의 SOURCE가 모두 저장 범위 안 / DB 확정 OK / DB의 Layer·도형 Layer가 선택 Layer와 같음
#       Service를 띄우지 않고 같은 프로세스에서 BSL을 직접 호출한다. DB 접속은 BSL -> DSL -> Miracom.Middleware 경로다.
# 대상: DACrux.SEMDMS.Service.exe.config의 DMS_CONNECT_ID(DMSMGR) -> 레지스트리 Middleware 연결 (로컬 docker 시험 DB)
# 결과: FACTORY=PROBE / DEVICE_ID=LAYER_FILTER_PROBE 를 시작 전 삭제 후 저장하고, -Keep이 없으면 마지막에 삭제한다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsImportLayerFilterProbe.ps1 [-GdsPath 경로] [-Layers 1,2] [-Keep]
#       -Layers를 생략하면 배치 도형이 적은 순서로 Layer 3개를 고른다.
param(
    [string]$GdsPath = '',
    [int[]]$Layers = @(),
    [int]$BatchRows = 2000,
    [switch]$Keep
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$serverBin = Join-Path $dms '101_ServerBin'
$gdsExe = Join-Path $dms '06_CoreLibSource\GdsMapControl\bin\Debug\NexplantQMS.GdsMap.exe'
if ($GdsPath -eq '') { $GdsPath = Join-Path (Split-Path $dms -Parent) 'SampleFile\OMM all layer2.gds\OMM all layer2.gds' }
$factory = 'PROBE'; $device = 'LAYER_FILTER_PROBE'

# BSL / DSL이 ConfigurationManager.AppSettings["DMS_CONNECT_ID"]를 읽으므로 Service 설정 파일을 이 프로세스 설정으로 지정한다.
[AppDomain]::CurrentDomain.SetData('APP_CONFIG_FILE', (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config'))
Add-Type -AssemblyName System.Configuration
$flags = [Reflection.BindingFlags]'NonPublic,Static'
$cm = [Configuration.ConfigurationManager]
$cm.GetField('s_initState', $flags).SetValue($null, 0)
$cm.GetField('s_configSystem', $flags).SetValue($null, $null)
foreach ($d in 'Oracle.ManagedDataAccess', 'Miracom.Middleware', 'DACrux.SEMDMS.Interface', 'DACrux.SEMDMS.DSL', 'DACrux.SEMDMS.BSL') {
    [void][Reflection.Assembly]::LoadFrom((Join-Path $serverBin "$d.dll"))
}
# Add-Type(CodeDom)은 .exe를 참조로 받지 못하므로 시험용 임시 폴더에 같은 어셈블리를 .dll 이름으로 복사해 참조 / 로드한다.
$probeDir = Join-Path $env:TEMP 'chippac\gdslayerfilterprobe'
New-Item -ItemType Directory -Force $probeDir | Out-Null
Copy-Item $gdsExe (Join-Path $probeDir 'NexplantQMS.GdsMap.dll') -Force
foreach ($d in 'OpenTK.dll', 'OpenTK.GLControl.dll') { Copy-Item (Join-Path (Split-Path $gdsExe) $d) $probeDir -Force }
$gdsExe = Join-Path $probeDir 'NexplantQMS.GdsMap.dll'
[void][Reflection.Assembly]::LoadFrom((Join-Path $probeDir 'OpenTK.dll'))
[void][Reflection.Assembly]::LoadFrom((Join-Path $probeDir 'OpenTK.GLControl.dll'))
[void][Reflection.Assembly]::LoadFrom($gdsExe)
Add-Type -AssemblyName System.Windows.Forms

# 대량 순회 / 비교는 C#에서 처리한다. 화면(EXE)과 같은 Builder / 방문 메서드를 그대로 사용한다.
$helper = @"
using System;
using System.Collections.Generic;
using System.Linq;
using NexplantQMS.GdsMap;
using NexplantQMS.GdsMap.Persistence;

public static class GdsLayerFilterProbe
{
    public static GdsLibrary Parse(string path)
    {
        var reader = new GdsReader { LengthUnit = LengthUnit.Micrometer };
        return reader.Read(path);
    }

    /// <summary>Layer별 배치 도형 수. 기본 선택(도형이 적은 Layer)에 사용한다.</summary>
    public static Dictionary<int, long> PlacedByLayer(GdsMapControl map)
    {
        var counts = new Dictionary<int, long>();
        foreach (var item in map.GetLayerDisplayItems()) counts[item.LayerId] = 0;
        map.VisitPlacedElements(d => { long c; counts.TryGetValue(d.LayerId, out c); counts[d.LayerId] = c + 1; });
        return counts;
    }

    /// <summary>Layer 조건 배치 도형 수 / 좌표 수 / SOURCE ID 집합 / 도형 Layer 집합.</summary>
    public static long[] CountPlaced(GdsMapControl map, ISet<int> layers, HashSet<string> sourceIds, SortedSet<int> placedLayers)
    {
        long points = 0;
        int count = map.VisitPlacedElements(layers, d => { points += d.WorldPoints.Length; sourceIds.Add(d.SourceElementId); placedLayers.Add(d.LayerId); });
        return new long[] { count, points };
    }

    /// <summary>SOURCE 행을 ID -> 행 내용(열을 0x1F로 연결)으로 모은다. 비교용이라 Payload까지 포함한다.</summary>
    public static Dictionary<string, string> SourceRows(GdsLibrary lib, ISet<int> layers, int batchRows)
    {
        var rows = new Dictionary<string, string>();
        foreach (string[,] batch in GdsMapSourceRowBuilder.BuildSourceBatches(lib, batchRows, layers))
            for (int i = 0; i < batch.GetLength(0); i++)
            {
                var cols = new string[batch.GetLength(1)];
                for (int c = 0; c < cols.Length; c++) cols[c] = batch[i, c];
                rows.Add(cols[0], string.Join("\u001F", cols));
            }
        return rows;
    }

    /// <summary>원본 SREF / AREF 개수(도면 목록 행 + 파서 중복 행).</summary>
    public static long CountReferences(GdsLibrary lib)
    {
        long count = 0;
        foreach (var structure in lib.Structures)
        {
            foreach (var layer in structure.Layers)
                count += layer.Elements.Count(e => e is GdsSRef || e is GdsARef);
            count += structure.Layers.DuplicatedItems.Count(e => e is GdsSRef || e is GdsARef);
        }
        return count;
    }

    public static long UploadSource(DACrux.SEMDMS.BSL.GdsMapImport bsl, string job, GdsLibrary lib, ISet<int> layers, int batchRows)
    {
        long no = 0, rows = 0;
        foreach (string[,] batch in GdsMapSourceRowBuilder.BuildSourceBatches(lib, batchRows, layers))
            rows += bsl.CreateSourceBatch(job, no++, batch, "probe");
        return rows;
    }

    public static long UploadPlaced(DACrux.SEMDMS.BSL.GdsMapImport bsl, string job, GdsMapControl map, ISet<int> layers, int batchRows)
    {
        long no = 0, rows = 0;
        var pending = new List<string[]>(batchRows);
        Action flush = () =>
        {
            var arr = new string[pending.Count, GdsMapPlacedRowBuilder.ColumnCount];
            for (int i = 0; i < pending.Count; i++)
                for (int c = 0; c < GdsMapPlacedRowBuilder.ColumnCount; c++) arr[i, c] = pending[i][c];
            rows += bsl.CreatePlacedBatch(job, no++, arr, "probe");
            pending.Clear();
        };
        map.VisitPlacedElements(layers, d =>
        {
            pending.Add(GdsMapPlacedRowBuilder.BuildRow(d));
            if (pending.Count == batchRows) flush();
        });
        if (pending.Count > 0) flush();
        return rows;
    }
}
"@
Add-Type -TypeDefinition $helper -ReferencedAssemblies @($gdsExe, (Join-Path $serverBin 'DACrux.SEMDMS.BSL.dll'), (Join-Path $serverBin 'DACrux.SEMDMS.Interface.dll'), (Join-Path $serverBin 'Miracom.Middleware.dll'),
    (Join-Path (Split-Path $gdsExe) 'OpenTK.GLControl.dll'), (Join-Path (Split-Path $gdsExe) 'OpenTK.dll'), 'System.Core', 'System.Data', 'System.Windows.Forms', 'System.Drawing', 'System.Xml')

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
$builder = [NexplantQMS.GdsMap.Persistence.GdsMapSourceRowBuilder]

# 1. 파싱 + Flatten + 저장 Layer 선택
$lib = [GdsLayerFilterProbe]::Parse($GdsPath)
$map = New-Object NexplantQMS.GdsMap.GdsMapControl
$map.ShowStructure($lib)
$byLayer = [GdsLayerFilterProbe]::PlacedByLayer($map)
if ($Layers.Count -eq 0) {
    $Layers = @($byLayer.GetEnumerator() | Where-Object { $_.Value -gt 0 } | Sort-Object Value, Key | Select-Object -First 3 | ForEach-Object { $_.Key })
}
$sel = New-Object 'Collections.Generic.HashSet[int]'
foreach ($id in $Layers) { [void]$sel.Add($id) }
Check '저장 Layer 선택' ($sel.Count -gt 0 -and $sel.Count -lt $byLayer.Count) ("전체 {0}개 중 선택 {1}" -f $byLayer.Count, (($sel | Sort-Object) -join ','))

# 2. 필터 건수 / 전체 건수 / SREF·AREF 유지
$fullSource = $builder::CountSourceRows($lib)
$fullPlaced = [GdsLayerFilterProbe]::CountPlaced($map, $null, (New-Object 'Collections.Generic.HashSet[string]'), (New-Object 'Collections.Generic.SortedSet[int]'))
$placedSourceIds = New-Object 'Collections.Generic.HashSet[string]'
$placedLayers = New-Object 'Collections.Generic.SortedSet[int]'
$counts = [GdsLayerFilterProbe]::CountPlaced($map, $sel, $placedSourceIds, $placedLayers)
$expectedSource = $builder::CountSourceRows($lib, $sel)
Check '필터 건수 < 전체' ($expectedSource -lt $fullSource -and $counts[0] -lt $fullPlaced[0] -and $counts[0] -gt 0) ("SOURCE {0}/{1} PLACED {2}/{3} 좌표 {4}/{5}" -f $expectedSource, $fullSource, $counts[0], $fullPlaced[0], $counts[1], $fullPlaced[1])
$sameLayers = (@($placedLayers | Where-Object { -not $sel.Contains($_) }).Count -eq 0)
Check '배치 도형 Layer가 선택 Layer 안' $sameLayers ("도형 Layer {0}" -f ($placedLayers -join ','))

# 3. SOURCE ID / 내용: 필터 행은 전체 저장 때와 같은 ID·내용이고, 제외 행은 번호만 건너뛴다
$fullRows = [GdsLayerFilterProbe]::SourceRows($lib, $null, $BatchRows)
$filterRows = [GdsLayerFilterProbe]::SourceRows($lib, $sel, $BatchRows)
$diff = 0
foreach ($kv in $filterRows.GetEnumerator()) { if (-not $fullRows.ContainsKey($kv.Key) -or $fullRows[$kv.Key] -ne $kv.Value) { $diff++ } }
Check 'SOURCE ID / 내용이 전체 저장과 같음(순번 유지)' ($diff -eq 0 -and $filterRows.Count -eq $expectedSource) ("필터 행 {0} / 불일치 {1}" -f $filterRows.Count, $diff)
$refCount = [GdsLayerFilterProbe]::CountReferences($lib)
$refKept = @($filterRows.Values | Where-Object { $t = $_.Split([char]0x1F)[3]; $t -eq 'SREF' -or $t -eq 'AREF' }).Count
Check 'SREF / AREF 원본 행 모두 유지' ($refKept -eq $refCount) "원본 $refCount / 저장 $refKept"
$missing = @($placedSourceIds | Where-Object { -not $filterRows.ContainsKey($_) }).Count
Check '배치 도형의 SOURCE가 모두 저장 범위 안(FK)' ($missing -eq 0) "도형 SOURCE ID $($placedSourceIds.Count)개 / 누락 $missing"

# 4. DB 저장 (기존 데이터 삭제 -> 시작 -> LAYER(선택만) -> SOURCE -> PLACED -> 확정)
$sha = (Get-FileHash -Algorithm SHA256 -LiteralPath $GdsPath).Hash
$bsl = New-Object DACrux.SEMDMS.BSL.GdsMapImport
if ($bsl.GetMapInfo($factory, $device).Rows.Count -gt 0) { [void]$bsl.DeleteMap($factory, $device, 'probe') }
$sw = [Diagnostics.Stopwatch]::StartNew()
$job = [string]$bsl.BeginMapImport([string[]]@($factory, $device, $lib.Structures[0].Name, (Split-Path $GdsPath -Leaf), $sha, '0.001', '1E-09', 'probe',
    "$expectedSource", "$($counts[0])", "$($counts[1])")).Rows[0]['IMPORT_JOB_SEQ']
$pairs = New-Object 'Collections.Generic.List[Collections.Generic.KeyValuePair[int,int]]'
foreach ($id in ($sel | Sort-Object)) { $pairs.Add((New-Object 'Collections.Generic.KeyValuePair[int,int]'($id, -16777216))) }
$nLayer = $bsl.CreateLayerBatch($job, 0, $builder::BuildLayerRows($pairs), 'probe')
$nSource = [GdsLayerFilterProbe]::UploadSource($bsl, $job, $lib, $sel, $BatchRows)
$nPlaced = [GdsLayerFilterProbe]::UploadPlaced($bsl, $job, $map, $sel, $BatchRows)
$c = $bsl.CompleteMapImport($job, 'probe').Rows[0]
Check 'DB 저장 / 확정 OK' ($c['RESULT'] -eq 'OK' -and $nLayer -eq $sel.Count -and $nSource -eq $expectedSource -and $nPlaced -eq $counts[0]) ("job={0} {1} / LAYER {2} SOURCE {3} PLACED {4} / DB source={5} placed={6} point={7} / {8:N0} ms" -f $job, $c['RESULT'], $nLayer, $nSource, $nPlaced, $c['DB_SOURCE'], $c['DB_PLACED'], $c['DB_POINT'], $sw.ElapsedMilliseconds)

# 5. 조회 서비스 기준 Layer 목록 = 선택 Layer (조회 화면 / Chain 재활용 범위)
$view = New-Object DACrux.SEMDMS.BSL.GdsMapView
$lt = $view.GetMapLayerList($factory, $device)
$dbLayers = @($lt.Rows | ForEach-Object { [int]$_['LAYER_ID'] } | Sort-Object)
Check '조회 서비스 Layer = 선택 Layer' ((($dbLayers -join ',') -eq (($sel | Sort-Object) -join ','))) ("DB Layer {0}" -f ($dbLayers -join ','))

if (-not $Keep) { [void]$bsl.DeleteMap($factory, $device, 'probe'); Write-Host "정리: $factory / $device 삭제" }

$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host 'GDS_IMPORT_LAYER_FILTER_PROBE=PASS' } else { Write-Host "GDS_IMPORT_LAYER_FILTER_PROBE=FAIL ($fail)"; exit 1 }
