# E-Test 파일 YMS 다시 반영 (2026-10-21)
# 목적: YMS 컬럼 이름이 바뀐 뒤(예전 50자 ITEM_NAME -> YMS_PARAM_NAME 30자) 이미 YMS 에 반영된 파일을 지우고 다시 반영한다.
#       Program 의 Wafer 를 모두 지워야 TD 테이블 / Parameter 정의(TQP_PARASPEC)가 지워지고 새 이름으로 다시 만들어진다.
#       하나만 다시 반영하면 남은 Wafer 때문에 TD 테이블이 남아 예전 컬럼 + 새 컬럼이 같이 생긴다.
# 설계: 문서/설계/04_ETest/2026-10-21_ETest_YMS_컬럼이름_30자_설계.md Y8 / 7절 6단계
# 처리: Factory + Device 의 E-Test 파일 목록 -> (1) 파일별 YMS 반영 삭제(EtYmsPublisher.DeleteWafer, 파일 삭제 때와 같은 기능)
#       -> (2) Program 의 Wafer 0 / 남은 빈 TD 테이블 정리(TestDataManager.CheckTableEmpty) -> (3) 파일별 PublishEtFileToYms
#       -> (4) TD 컬럼 / Parameter 정의 이름 30자 이하 확인
#       DB 접속은 BSL -> DSL -> Miracom.Middleware (Service Remoting 경유 아님, SEMDMS Service 설정 파일의 접속 이름 사용)
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File EtYmsRepublish.ps1 [-BinDir <TEMP 빌드 폴더>] [-Factory SCK] [-Device OMM,OMMTCB] [-Apply]
#       -BinDir 생략 시 101_ServerBin. -Apply 가 없으면 대상 파일 / 지금 YMS 상태만 보여 준다.
# 결과: 마지막 줄 ET_YMS_REPUBLISH=PASS 또는 FAIL
param(
    [string]$BinDir = '',
    [string]$Factory = 'SCK',
    [string[]]$Device = @('OMM', 'OMMTCB'),
    [string]$User = 'YMS_REPUBLISH',
    [switch]$Apply
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$serverBin = Join-Path $dms '101_ServerBin'
if ($BinDir -eq '') { $BinDir = $serverBin }
$Device = @($Device | ForEach-Object { $_.Split(',') } | ForEach-Object { $_.Trim() } | Where-Object { $_ -ne '' })

# BSL / DSL 이 ConfigurationManager.AppSettings 를 읽으므로 SEMDMS Service 설정 파일을 이 프로세스 설정으로 지정한다.
[AppDomain]::CurrentDomain.SetData('APP_CONFIG_FILE', (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config'))
Add-Type -AssemblyName System.Configuration
$flags = [Reflection.BindingFlags]'NonPublic,Static'
$cm = [Configuration.ConfigurationManager]
$cm.GetField('s_initState', $flags).SetValue($null, 0)
$cm.GetField('s_configSystem', $flags).SetValue($null, $null)
foreach ($d in 'Oracle.ManagedDataAccess', 'Miracom.Middleware', 'DACrux.Base') {
    $p = Join-Path $serverBin "$d.dll"; if (Test-Path $p) { [void][Reflection.Assembly]::LoadFrom($p) }
}
foreach ($d in 'DACrux.SEMDMS.Interface', 'DACrux.SEMDMS.DSL', 'DACrux.TEST.Interface', 'DACrux.TEST.DSL', 'DACrux.TEST.BSL', 'DACrux.SEMDMS.BSL') {
    [void][Reflection.Assembly]::LoadFrom((Join-Path $BinDir "$d.dll"))
}
Write-Host "BSL: $(Join-Path $BinDir 'DACrux.SEMDMS.BSL.dll') / Factory $Factory / Device $($Device -join ',') / $(if ($Apply) { '실행' } else { '계획만' })"

$Rule = [DACrux.SEMDMS.Interface.EtYmsRule]
$bsl = New-Object DACrux.SEMDMS.BSL.GdsEtResult
$pub = New-Object DACrux.SEMDMS.BSL.EtYmsPublisher
$ymsDsl = New-Object DACrux.SEMDMS.DSL.TQP_GDS_ET_YMS
$tableDsl = New-Object DACrux.TEST.DSL.TQP_DATA_TABLES
$tdDsl = New-Object DACrux.TEST.DSL.TD_TABLE
$paraDsl = New-Object DACrux.TEST.DSL.TQP_PARASPEC
$problems = New-Object Collections.Generic.List[string]

function YmsFile([string]$seq) { return @($ymsDsl.GetData('SELECT_ET_YMS_FILE', $null, [string[]]@($seq)).Rows)[0] }
function TdTables([string]$program) { return @($tableDsl.GetTableNames($Factory, $program) | Where-Object { $_ }) }
function LongParamNames([string]$program) {
    return @($paraDsl.GetParamSepc($Factory, $program, '0').Rows | ForEach-Object { [string]$_['PARAM_NAME'] } | Where-Object { $_.Length -gt $Rule::MaxYmsNameLength })
}

# 0. 대상 파일 (저장 끝난 READY 파일만)
$files = New-Object Collections.Generic.List[object]
foreach ($dev in $Device) {
    foreach ($r in @($bsl.GetEtFileList($Factory, $dev).Rows)) {
        if ([string]$r['LOAD_STATUS'] -ne 'READY') { continue }
        $y = YmsFile ([string]$r['SRC_FILE_SEQ'])
        $files.Add([pscustomobject]@{
            Seq = [string]$r['SRC_FILE_SEQ']; Device = $dev; Test = [string]$r['TEST_PROGRAM']; File = [string]$r['FILE_NAME']
            Program = $Rule::ToProgramName($dev, $Rule::ToTestArea([string]$r['TEST_PROGRAM'])); YmsStatus = [string]$y['YMS_STATUS']; Wafer = [string]$y['YMS_WAFER_SEQ']
        })
    }
}
$programs = @($files | ForEach-Object { $_.Program } | Sort-Object -Unique)
foreach ($f in $files) { Write-Host ("파일 {0} / {1} {2} / YMS {3} Wafer {4} / {5}" -f $f.Seq, $f.Device, $f.Test, $f.YmsStatus, $f.Wafer, $f.File) }
foreach ($pg in $programs) { Write-Host ("Program {0} / TD 테이블 {1} / 30자 넘는 Parameter 정의 {2}" -f $pg, ((TdTables $pg) -join ','), (LongParamNames $pg).Count) }
if ($files.Count -eq 0) { Write-Host 'ET_YMS_REPUBLISH=PASS (대상 파일 없음)'; return }
if (-not $Apply) { Write-Host 'ET_YMS_REPUBLISH=PASS (계획만, -Apply 없음)'; return }

# 1. 파일별 YMS 반영 삭제 (Program 의 Wafer 를 모두 지운다)
foreach ($f in $files) {
    if ($f.Wafer -ne '') { [void]$pub.DeleteWafer($f.Wafer); Write-Host "YMS 삭제: 파일 $($f.Seq) Wafer $($f.Wafer)" }
}
# 2. 빈 TD 테이블 정리 (마지막 Wafer 삭제 때 지워지지 않고 남은 경우)
$tm = New-Object DACrux.TEST.BSL.TestDataManager
foreach ($pg in $programs) {
    foreach ($t in (TdTables $pg)) { $tm.CheckTableEmpty($t) }
    $left = TdTables $pg
    Write-Host ("Program {0} 삭제 뒤 TD 테이블 {1} / Parameter 정의 {2}" -f $pg, $(if ($left.Count -eq 0) { '없음' } else { $left -join ',' }), @($paraDsl.GetParamSepc($Factory, $pg, '0').Rows).Count)
    if ($left.Count -gt 0) { $problems.Add("Program $pg 의 TD 테이블이 남음(다른 Wafer 가 있음): $($left -join ',')") }
}
# 3. 파일별 다시 반영
foreach ($f in $files) {
    $r = $bsl.PublishEtFileToYms($f.Seq, $User).Rows[0]
    Write-Host ("반영: 파일 {0} / {1} {2} / Wafer {3} / Die {4} / Param {5} / 수율 {6} {7}" -f $f.Seq, $r['YMS_STATUS'], $r['YMS_PROGRAM'], $r['YMS_WAFER_SEQ'], $r['DIE_COUNT'], $r['PARAM_COUNT'], $r['YIELD'], $r['YMS_MESSAGE'])
    if ([string]$r['YMS_STATUS'] -ne 'DONE') { $problems.Add("파일 $($f.Seq) 반영 $($r['YMS_STATUS']): $($r['YMS_MESSAGE'])") }
    $f.Wafer = [string]$r['YMS_WAFER_SEQ']
}
# 4. TD 컬럼 / Parameter 정의 이름 30자 이하
foreach ($pg in $programs) {
    $wafer = @($files | Where-Object { $_.Program -eq $pg -and $_.Wafer -ne '' } | Select-Object -First 1)
    $maxCol = 0
    if ($wafer.Count -gt 0) {
        $ds = $tdDsl.GetTableSchema($Factory, $pg, $wafer[0].Wafer)
        if ($ds.Tables.Count -gt 0) { $maxCol = ($ds.Tables[0].Columns | ForEach-Object { $_.ColumnName.Length } | Measure-Object -Maximum).Maximum }
    }
    $long = LongParamNames $pg
    Write-Host ("확인: Program {0} / TD {1} / TD 컬럼 최대 {2}자 / Parameter 정의 {3} (30자 넘는 것 {4})" -f $pg, ((TdTables $pg) -join ','), $maxCol, @($paraDsl.GetParamSepc($Factory, $pg, '0').Rows).Count, $long.Count)
    if ($maxCol -gt $Rule::MaxYmsNameLength -or $long.Count -gt 0) { $problems.Add("Program $pg 에 30자 넘는 이름이 남음") }
}
$problems | ForEach-Object { Write-Host "문제: $_" }
Write-Host ("ET_YMS_REPUBLISH={0} (파일 {1} / Program {2})" -f $(if ($problems.Count -eq 0) { 'PASS' } else { 'FAIL' }), $files.Count, $programs.Count)
if ($problems.Count -gt 0) { exit 1 }
