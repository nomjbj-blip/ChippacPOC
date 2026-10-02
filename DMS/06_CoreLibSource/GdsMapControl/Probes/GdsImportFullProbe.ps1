# GDS Map DB 저장 구현 4단계 검증 스크립트 (CLAUDE-009)
# 목적: 실제 GDS 한 개를 파싱 -> GdsMapControl로 Flatten -> 저장 작업 시작 -> LAYER / SOURCE / PLACED 배치 저장 -> 완료 확정까지
#       전체 흐름을 BSL GdsMapImport로 실행하고, 건수 / 좌표 재조회 / 확정 상태 / 건수 불일치 감지를 확인한다.
#       Service를 띄우지 않고 같은 프로세스에서 BSL을 직접 호출한다. DB 접속은 BSL -> DSL -> Miracom.Middleware 경로다.
# 대상: DACrux.SEMDMS.Service.exe.config의 DMS_CONNECT_ID(DMSMGR) -> 레지스트리 Middleware 연결 (로컬 docker 시험 DB)
# 결과(CLAUDE-017, Device 1:1): FACTORY=PROBE / DEVICE_ID=FULL_PROBE 의 READY Map 1건(샘플 기준 SOURCE 약 64만 / PLACED 약 60만 행)이 남고,
#       시작 전에 같은 Device의 기존 데이터를 삭제한다. 불일치 확인용 FULLMIS_PROBE는 마지막에 삭제한다. (아래 원래 문구)
# 이전:
#       불일치 확인용 DRAFT 개정본 1건이 남는다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsImportFullProbe.ps1 [-GdsPath 경로] [-BatchRows 2000]
param(
    [string]$GdsPath = '',
    [int]$BatchRows = 2000
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$serverBin = Join-Path $dms '101_ServerBin'
$gdsExe = Join-Path $dms '06_CoreLibSource\GdsMapControl\bin\Debug\NexplantQMS.GdsMap.exe'
if ($GdsPath -eq '') { $GdsPath = Join-Path (Split-Path $dms -Parent) 'SampleFile\OMM all layer2.gds\OMM all layer2.gds' }

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
$probeDir = Join-Path $env:TEMP 'claude\gdsfullprobe'
New-Item -ItemType Directory -Force $probeDir | Out-Null
Copy-Item $gdsExe (Join-Path $probeDir 'NexplantQMS.GdsMap.dll') -Force
foreach ($d in 'OpenTK.dll', 'OpenTK.GLControl.dll') { Copy-Item (Join-Path (Split-Path $gdsExe) $d) $probeDir -Force }
$gdsExe = Join-Path $probeDir 'NexplantQMS.GdsMap.dll'
[void][Reflection.Assembly]::LoadFrom((Join-Path $probeDir 'OpenTK.dll'))
[void][Reflection.Assembly]::LoadFrom((Join-Path $probeDir 'OpenTK.GLControl.dll'))
[void][Reflection.Assembly]::LoadFrom($gdsExe)
Add-Type -AssemblyName System.Windows.Forms

# 60만 건 순회 / 변환 / 업로드는 PowerShell 반복보다 C#에서 처리한다. 화면(EXE)과 같은 Builder와 BSL을 그대로 사용한다.
$helper = @"
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using NexplantQMS.GdsMap;
using NexplantQMS.GdsMap.Persistence;

public static class GdsFullProbe
{
    public static GdsLibrary Parse(string path)
    {
        var reader = new GdsReader { LengthUnit = LengthUnit.Micrometer };
        return reader.Read(path);
    }

    /// <summary>화면 저장과 같이 Flatten 결과의 배치 도형 수 / 좌표 수 / Layer 목록을 센다.</summary>
    public static long[] CountPlaced(GdsMapControl map, SortedSet<int> layers)
    {
        long points = 0;
        int count = map.VisitPlacedElements(d => { points += d.WorldPoints.Length; layers.Add(d.LayerId); });
        return new long[] { count, points };
    }

    public static string UploadSource(DACrux.SEMDMS.BSL.GdsMapImport bsl, string job, GdsLibrary lib, int batchRows)
    {
        var sw = Stopwatch.StartNew(); long no = 0, rows = 0;
        foreach (string[,] batch in GdsMapSourceRowBuilder.BuildSourceBatches(lib, batchRows))
            rows += bsl.CreateSourceBatch(job, no++, batch, "probe");
        return rows + "|" + no + "|" + sw.ElapsedMilliseconds;
    }

    /// <summary>배치 도형을 batchRows건씩 모아 행으로 바꾸고 바로 업로드한다(좌표는 콜백 안에서 즉시 16진수로 변환).</summary>
    public static string UploadPlaced(DACrux.SEMDMS.BSL.GdsMapImport bsl, string job, GdsMapControl map, int batchRows, Dictionary<string, string> sample)
    {
        var sw = Stopwatch.StartNew(); var upload = new Stopwatch();
        long no = 0, rows = 0;
        var pending = new List<string[]>(batchRows);
        Action flush = () =>
        {
            var arr = new string[pending.Count, GdsMapPlacedRowBuilder.ColumnCount];
            for (int i = 0; i < pending.Count; i++)
                for (int c = 0; c < GdsMapPlacedRowBuilder.ColumnCount; c++) arr[i, c] = pending[i][c];
            upload.Start(); rows += bsl.CreatePlacedBatch(job, no++, arr, "probe"); upload.Stop();
            pending.Clear();
        };
        int index = 0;
        map.VisitPlacedElements(d =>
        {
            string[] row = GdsMapPlacedRowBuilder.BuildRow(d);
            if (index == 0 || index % 99991 == 0 || d.ElementType == "TEXT" && !sample.ContainsKey("TEXT"))
                sample[d.ElementType == "TEXT" && !sample.ContainsKey("TEXT") ? "TEXT" : d.PlacedElementId] = d.PlacedElementId + "\u001F" + row[14] + "\u001F" + row[8] + "\u001F" + row[15];
            index++;
            pending.Add(row);
            if (pending.Count == batchRows) flush();
        });
        if (pending.Count > 0) flush();
        return rows + "|" + no + "|" + sw.ElapsedMilliseconds + "|" + upload.ElapsedMilliseconds;
    }
}
"@
Add-Type -TypeDefinition $helper -ReferencedAssemblies @($gdsExe, (Join-Path $serverBin 'DACrux.SEMDMS.BSL.dll'), (Join-Path $serverBin 'DACrux.SEMDMS.Interface.dll'), (Join-Path $serverBin 'Miracom.Middleware.dll'),
    (Join-Path (Split-Path $gdsExe) 'OpenTK.GLControl.dll'), (Join-Path (Split-Path $gdsExe) 'OpenTK.dll'), 'System.Data', 'System.Windows.Forms', 'System.Drawing', 'System.Xml')

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}

# 1. 파싱 + Flatten
$sw = [Diagnostics.Stopwatch]::StartNew()
$lib = [GdsFullProbe]::Parse($GdsPath)
$parseMs = $sw.ElapsedMilliseconds; $sw.Restart()
$map = New-Object NexplantQMS.GdsMap.GdsMapControl
$map.ShowStructure($lib)
$flattenMs = $sw.ElapsedMilliseconds
$layers = New-Object 'Collections.Generic.SortedSet[int]'
foreach ($id in [NexplantQMS.GdsMap.Persistence.GdsMapSourceRowBuilder]::CollectLayerIds($lib)) { [void]$layers.Add($id) }
$counts = [GdsFullProbe]::CountPlaced($map, $layers)
$expectedSource = [NexplantQMS.GdsMap.Persistence.GdsMapSourceRowBuilder]::CountSourceRows($lib)
Check '파싱 / Flatten' ($counts[0] -gt 0) "source=$expectedSource placed=$($counts[0]) points=$($counts[1]) layers=$($layers.Count) parse ms=$parseMs flatten ms=$flattenMs"

# 2. 저장 작업 시작
$sha = (Get-FileHash -Algorithm SHA256 -LiteralPath $GdsPath).Hash
$bsl = New-Object DACrux.SEMDMS.BSL.GdsMapImport
# Device당 Map 1개: 기존 데이터가 있으면 먼저 삭제한다(화면에서는 사용자에게 묻는 단계).
$swDel = [Diagnostics.Stopwatch]::StartNew()
$deletedBefore = 0
if ($bsl.GetMapInfo('PROBE', 'FULL_PROBE').Rows.Count -gt 0) { $deletedBefore = $bsl.DeleteMap('PROBE', 'FULL_PROBE', 'probe') }
$deleteMs = $swDel.ElapsedMilliseconds
$job = [string]$bsl.BeginMapImport([string[]]@('PROBE', 'FULL_PROBE', $lib.Structures[0].Name, (Split-Path $GdsPath -Leaf), $sha, '0.001', '1E-09', 'probe',
    "$expectedSource", "$($counts[0])", "$($counts[1])")).Rows[0]['IMPORT_JOB_SEQ']
Check '기존 데이터 삭제 후 저장 시작' ($job -match '^\d+$') ("job={0} / 이전 데이터 삭제 도형 {1}건 {2:N0} ms" -f $job, $deletedBefore, $deleteMs)

# 3. LAYER
$pairs = New-Object 'Collections.Generic.List[Collections.Generic.KeyValuePair[int,int]]'
foreach ($id in $layers) { $pairs.Add((New-Object 'Collections.Generic.KeyValuePair[int,int]'($id, -16777216))) }
$n = $bsl.CreateLayerBatch($job, 0, [NexplantQMS.GdsMap.Persistence.GdsMapSourceRowBuilder]::BuildLayerRows($pairs), 'probe')
Check 'LAYER 저장' ($n -eq $layers.Count) "rows=$n"

# 4. SOURCE
$s = ([GdsFullProbe]::UploadSource($bsl, $job, $lib, $BatchRows)).Split('|')
Check 'SOURCE 저장' ([long]$s[0] -eq $expectedSource) ("rows={0} batches={1} {2:N0} ms" -f $s[0], $s[1], [long]$s[2])

# 5. PLACED
$sample = New-Object 'Collections.Generic.Dictionary[string,string]'
$p = ([GdsFullProbe]::UploadPlaced($bsl, $job, $map, $BatchRows, $sample)).Split('|')
Check 'PLACED 저장' ([long]$p[0] -eq $counts[0]) ("rows={0} batches={1} 전체 {2:N0} ms / 서버 저장 {3:N0} ms (행당 {4:N3} ms)" -f $p[0], $p[1], [long]$p[2], [long]$p[3], ([long]$p[3] / [Math]::Max(1, [long]$p[0])))

# 6. 완료 확정
$sw.Restart()
$c = $bsl.CompleteMapImport($job, 'probe').Rows[0]
Check '완료 확정 OK' ($c['RESULT'] -eq 'OK' -and $c['JOB_STATUS'] -eq 'READY') ("{0} / DB source={1} placed={2} point={3} / {4:N0} ms / {5}" -f $c['RESULT'], $c['DB_SOURCE'], $c['DB_PLACED'], $c['DB_POINT'], $sw.ElapsedMilliseconds, $c['MESSAGE'])
$jobRow = $bsl.GetImportJob($job).Rows[0]
$mapRow = $bsl.GetMapInfo('PROBE', 'FULL_PROBE').Rows[0]
Check '작업 / Map READY' ($jobRow['JOB_STATUS'] -eq 'READY' -and $mapRow['MAP_STATUS'] -eq 'READY' -and [long]$jobRow['RECEIVED_PLACED_COUNT'] -eq $counts[0]) "job=$($jobRow['JOB_STATUS']) map=$($mapRow['MAP_STATUS']) file=$($mapRow['GDS_FILE_NAME'])"
$c2 = $bsl.CompleteMapImport($job, 'probe').Rows[0]
Check '재확정은 이미 확정 응답' ($c2['RESULT'] -eq 'OK' -and ([string]$c2['MESSAGE']).Contains('이미')) "$($c2['MESSAGE'])"

# 7. 좌표 / TEXT 재조회 비교 (Middleware로 SELECT_PLACED_POINTS)
Add-Type -ReferencedAssemblies @((Join-Path $serverBin 'Miracom.Middleware.dll'), 'System.Data', 'System.Xml') -TypeDefinition @"
using System.Data; using System.Runtime.CompilerServices;
public class GdsFullRead : Miracom.Middleware.QueryComponent {
    public GdsFullRead() { InitQueryComponent("DMSMGR", "TQP_GDS_PLACED_EL.xml"); }
    [MethodImpl(MethodImplOptions.NoInlining)]
    public DataTable Read(string factory, string device, string id) { return GetDataTable("SELECT_PLACED_POINTS", null, new string[] { factory, device, id }); }
}
"@
$reader = New-Object GdsFullRead
$okCount = 0; $detail = @()
foreach ($kv in $sample.GetEnumerator()) {
    $parts = $kv.Value.Split([char]0x1F)
    $row = $reader.Read('PROBE', 'FULL_PROBE', $parts[0]).Rows[0]
    $hex = [NexplantQMS.GdsMap.Persistence.GdsMapSourceRowBuilder]::ToHex([byte[]]$row['POINTS_BIN'])
    $text = if ($row['TEXT_VALUE'] -is [DBNull]) { '' } else { [string]$row['TEXT_VALUE'] }
    $same = ($hex -eq $parts[1]) -and ([double]$row['MIN_X'] -eq [double]::Parse($parts[2], [Globalization.CultureInfo]::InvariantCulture)) -and ($text -eq $parts[3])
    if ($same) { $okCount++ } else { $detail += $parts[0] }
}
Check '좌표 / Bounds / TEXT 재조회 일치' ($okCount -eq $sample.Count) "sample=$($sample.Count) ok=$okCount $($detail -join ',')"

# 8. 건수 불일치 감지: PLACED를 보내지 않은 작업은 MISMATCH이고 UPLOADING으로 남아야 한다
if ($bsl.GetMapInfo('PROBE', 'FULLMIS_PROBE').Rows.Count -gt 0) { [void]$bsl.DeleteMap('PROBE', 'FULLMIS_PROBE', 'probe') }
$job2 = [string]$bsl.BeginMapImport([string[]]@('PROBE', 'FULLMIS_PROBE', 'TOP', 'mismatch.gds', ('F' * 64), '0.001', '1E-09', 'probe', '1', '1', '5')).Rows[0]['IMPORT_JOB_SEQ']
[void]$bsl.CreateLayerBatch($job2, 0, [NexplantQMS.GdsMap.Persistence.GdsMapSourceRowBuilder]::BuildLayerRows($pairs), 'probe')
$firstSource = $null
foreach ($b in [NexplantQMS.GdsMap.Persistence.GdsMapSourceRowBuilder]::BuildSourceBatches($lib, 1)) { $firstSource = $b; break }
[void]$bsl.CreateSourceBatch($job2, 0, $firstSource, 'probe')
$m = $bsl.CompleteMapImport($job2, 'probe').Rows[0]
$st2 = $bsl.GetImportJob($job2).Rows[0]
Check 'PLACED 누락은 MISMATCH / UPLOADING 유지' ($m['RESULT'] -eq 'MISMATCH' -and $st2['JOB_STATUS'] -eq 'UPLOADING') "$($m['MESSAGE']) / job=$($st2['JOB_STATUS'])"
[void]$bsl.DeleteMap('PROBE', 'FULLMIS_PROBE', 'probe')

$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host 'GDS_IMPORT_FULL_PROBE=PASS' } else { Write-Host "GDS_IMPORT_FULL_PROBE=FAIL ($fail)"; exit 1 }
