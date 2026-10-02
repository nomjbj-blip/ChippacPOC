# GDS Map DB 저장 - Device 1:1 / 기존 데이터 삭제 후 재등록 검증 스크립트 (CLAUDE-017)
# 목적: 2026-10-02 사용자 결정(리비전 관리 없음, Factory + Device당 Map 1개, 기존 데이터는 사용자가 삭제 후 재등록)에 맞춰
#       101_ServerBin(또는 -BinDir)의 BSL GdsMapImport가 기존 데이터 조회 / 중복 시작 거부 / 저장 / 확정 / 삭제(전 테이블) / 재등록 /
#       중지(취소)를 올바르게 처리하는지 아주 작은 합성 데이터(LAYER 1 / SOURCE 1 / PLACED 1)로 확인한다.
#       Service를 띄우지 않고 BSL을 직접 호출한다. DB 접속은 BSL -> DSL -> Miracom.Middleware 경로다.
# 대상: DACrux.SEMDMS.Service.exe.config의 DMS_CONNECT_ID(DMSMGR) -> 레지스트리 Middleware 연결 (로컬 docker 시험 DB)
# 결과: FACTORY=PROBE / DEVICE_ID=JOB_PROBE_<시각> 데이터는 마지막에 삭제한다(남지 않음).
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsImportJobProbe.ps1 [-BinDir 시험 빌드 폴더]
param([string]$BinDir = '')
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$serverBin = Join-Path $dms '101_ServerBin'
if ($BinDir -eq '') { $BinDir = $serverBin }

[AppDomain]::CurrentDomain.SetData('APP_CONFIG_FILE', (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config'))
Add-Type -AssemblyName System.Configuration
$flags = [Reflection.BindingFlags]'NonPublic,Static'
$cm = [Configuration.ConfigurationManager]
$cm.GetField('s_initState', $flags).SetValue($null, 0)
$cm.GetField('s_configSystem', $flags).SetValue($null, $null)
foreach ($d in 'Oracle.ManagedDataAccess', 'Miracom.Middleware') { [void][Reflection.Assembly]::LoadFrom((Join-Path $serverBin "$d.dll")) }
foreach ($d in 'DACrux.SEMDMS.Interface', 'DACrux.SEMDMS.DSL', 'DACrux.SEMDMS.BSL') { [void][Reflection.Assembly]::LoadFrom((Join-Path $BinDir "$d.dll")) }

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
function Fails([scriptblock]$action) {
    try { & $action; return $null } catch { $e = $_.Exception; while ($e.InnerException -ne $null) { $e = $e.InnerException }; return $e.Message }
}

$bsl = New-Object DACrux.SEMDMS.BSL.GdsMapImport
$factory = 'PROBE'
$device = 'JOB_PROBE_' + (Get-Date -Format 'HHmmss')
$sha = 'D4' * 32
function Info($s, [long]$src, [long]$pl, [long]$pt) { return ,[string[]]@($factory, $device, 'TOP', 'probe.gds', $s, '0.001', '1E-09', 'probe', "$src", "$pl", "$pt") }
function Upload($job) {
    $layer = New-Object 'string[,]' 1, 3; $layer[0, 0] = '1'; $layer[0, 1] = '-16777216'
    [void]$bsl.CreateLayerBatch($job, 0, $layer, 'probe')
    $src = New-Object 'string[,]' 1, 9
    $v = @('TOP/E0', 'TOP', '0', 'BOUNDARY', '1', '0', '0', '1', '01FFFFFFFF'); for ($c = 0; $c -lt 9; $c++) { $src[0, $c] = $v[$c] }
    [void]$bsl.CreateSourceBatch($job, 0, $src, 'probe')
    $pl = New-Object 'string[,]' 1, 20
    $v = @('R0/E0', 'TOP/E0', '1', '0', 'BOUNDARY', '1', $null, $null, '1', '1', '1', '1', '1', '1', '000000000000F03F000000000000F03F', $null, $null, $null, $null, $null)
    for ($c = 0; $c -lt 20; $c++) { $pl[0, $c] = $v[$c] }
    [void]$bsl.CreatePlacedBatch($job, 0, $pl, 'probe')
}

# 0. 예전 Probe가 남긴 시험 데이터 정리 (FACTORY=PROBE / DEVICE_ID=JOB_PROBE)
if ($bsl.GetMapInfo($factory, 'JOB_PROBE').Rows.Count -gt 0) { [void]$bsl.DeleteMap($factory, 'JOB_PROBE', 'probe') }

# 1. 기존 데이터 없음
Check '기존 데이터 없음' ($bsl.GetMapInfo($factory, $device).Rows.Count -eq 0) "device=$device"

# 2. 저장 시작 -> 작업 번호는 시퀀스 숫자
$r1 = $bsl.BeginMapImport((Info $sha 1 1 1)).Rows[0]
$job1 = [string]$r1['IMPORT_JOB_SEQ']
Check '저장 시작' ($job1 -match '^\d+$' -and $r1['JOB_STATUS'] -eq 'UPLOADING' -and $r1['FACTORY'] -eq $factory -and $r1['DEVICE_ID'] -eq $device) "job=$job1"

# 3. 같은 Device로 다시 시작하면 거부 (화면은 먼저 삭제 여부를 묻는다)
$m3 = Fails { $bsl.BeginMapImport((Info $sha 1 1 1)) }
Check '기존 데이터가 있으면 시작 거부' ($m3 -ne $null -and $m3.Contains('기존 GDS 데이터')) "$m3"

# 4. 저장 미완료 상태 조회
$i4 = $bsl.GetMapInfo($factory, $device).Rows[0]
Check '저장 미완료 표시' ($i4['MAP_STATUS'] -eq 'DRAFT' -and $i4['JOB_STATUS'] -eq 'UPLOADING' -and $i4['GDS_FILE_NAME'] -eq 'probe.gds') "status=$($i4['MAP_STATUS']) job=$($i4['JOB_STATUS'])"

# 5. 저장 / 확정 -> READY
Upload $job1
$c5 = $bsl.CompleteMapImport($job1, 'probe').Rows[0]
$i5 = $bsl.GetMapInfo($factory, $device).Rows[0]
Check '확정 후 READY / 건수' ($c5['RESULT'] -eq 'OK' -and $i5['MAP_STATUS'] -eq 'READY' -and [long]$i5['PLACED_COUNT'] -eq 1 -and [long]$i5['SOURCE_COUNT'] -eq 1) "result=$($c5['RESULT']) status=$($i5['MAP_STATUS'])"

# 6. 삭제 -> 헤더 / 작업 / 배치 / Layer / 원본 / 도형 모두 0건
$deleted = $bsl.DeleteMap($factory, $device, 'probe')
$left = $bsl.GetMapInfo($factory, $device).Rows.Count
$jobLeft = $bsl.GetImportJob($job1).Rows.Count
$batchLeft = $bsl.GetImportBatch($job1).Rows.Count
Add-Type -ReferencedAssemblies @((Join-Path $serverBin 'Miracom.Middleware.dll'), 'System.Data', 'System.Xml') -TypeDefinition @"
using System.Data; using System.Runtime.CompilerServices;
public class GdsDeviceCount : Miracom.Middleware.QueryComponent {
    public GdsDeviceCount(string xml) { InitQueryComponent("DMSMGR", xml); }
    [MethodImpl(MethodImplOptions.NoInlining)]
    public long Count(string id, string f, string d, string col) { return System.Convert.ToInt64(GetDataTable(id, null, new string[] { f, d }).Rows[0][col]); }
}
"@
$srcLeft = (New-Object GdsDeviceCount('TQP_GDS_SOURCE_EL.xml')).Count('SELECT_SOURCE_EL_COUNT', $factory, $device, 'SOURCE_COUNT')
$plLeft = (New-Object GdsDeviceCount('TQP_GDS_PLACED_EL.xml')).Count('SELECT_PLACED_EL_COUNT', $factory, $device, 'PLACED_COUNT')
Check '삭제 후 모든 데이터 0건' ($deleted -eq 1 -and $left -eq 0 -and $jobLeft -eq 0 -and $batchLeft -eq 0 -and $srcLeft -eq 0 -and $plLeft -eq 0) "deleted placed=$deleted map=$left job=$jobLeft batch=$batchLeft source=$srcLeft placed=$plLeft"

# 7. 삭제 후 다시 등록 -> 새 작업 번호
$r7 = $bsl.BeginMapImport((Info ('E5' * 32) 1 1 1)).Rows[0]
$job2 = [string]$r7['IMPORT_JOB_SEQ']
Check '삭제 후 다시 등록' ($job2 -ne $job1 -and $r7['JOB_STATUS'] -eq 'UPLOADING') "job=$job2"

# 8. 중지(취소) -> CANCELLED, 데이터는 남아 다음 저장 때 삭제 여부를 묻는다
$bsl.CancelImportJob($job2, 'probe')
$i8 = $bsl.GetMapInfo($factory, $device).Rows[0]
$m8 = Fails { $bsl.CancelImportJob($job2, 'probe') }
Check '중지 후 CANCELLED / 데이터 남음 / 재취소 거부' ($i8['JOB_STATUS'] -eq 'CANCELLED' -and $i8['MAP_STATUS'] -eq 'DRAFT' -and $m8 -ne $null) "job=$($i8['JOB_STATUS']) map=$($i8['MAP_STATUS'])"

# 9. 입력 검증
$m9 = Fails { $bsl.BeginMapImport([string[]]@('PROBE')) }
Check '입력 항목 수 검증' ($m9 -ne $null) "$m9"
$m10 = Fails { $bsl.GetImportJob('abc') }
Check '작업 번호 형식 검증' ($m10 -ne $null) "$m10"

# 정리
[void]$bsl.DeleteMap($factory, $device, 'probe')
Check '정리 삭제' ($bsl.GetMapInfo($factory, $device).Rows.Count -eq 0) ''

$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host 'GDS_IMPORT_JOB_PROBE=PASS' } else { Write-Host "GDS_IMPORT_JOB_PROBE=FAIL ($fail)"; exit 1 }
