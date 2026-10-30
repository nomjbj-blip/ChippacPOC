# 데이터 생성 / 수정 작업 기간 확인(WorkRule) 검증 스크립트 (2026-10-23)
# 목적: 저장 함수 첫 단계의 작업 기간 확인이 빠짐없이 들어 있고, DB 일시(한국 시간)로 판정하는지 확인한다.
#  1) 소스 검사: 대상 저장 함수 31곳의 첫 줄이 WorkRule().Confirm 이다. 파일 삭제(DeleteEtFile)와 조회 함수에는 없다.
#  2) 기간 판정: IsOpen / DaysLeft 경계(마지막 날 23:59:59 통과, 다음 날 0시 차단, 하한 2026-10-01).
#  3) DB 일시: WorkRule.GetState / BSL GdsMapView.GetWorkState 가 한국 시간과 기간 상태를 돌려준다(로컬 DB OS 시간이 UTC 여도 PC 시각과 같아야 함).
#  4) -ExpectClosed: 마지막 날을 과거로 바꾼 임시 DSL 빌드에서 대상 31개 함수가 모두 첫 단계에서 기간 문장으로 멈추고,
#     조회 함수(GdsMapView.GetMapDeviceList)는 그대로 동작한다. 기간 안 빌드에서는 저장 함수를 부르지 않는다(DB 를 바꾸지 않기 위해).
# Service 를 띄우지 않고 BSL 을 직접 부른다. 설정은 101_ServerBin\DACrux.SEMDMS.Service.exe.config 사본을 쓴다(운영 config 는 바꾸지 않음).
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File WorkRuleProbe.ps1 [-BslDir <DSL / BSL / TEST.BSL / Interface 폴더>] [-ExpectClosed]
param(
    [string]$BslDir = '',
    [switch]$ExpectClosed
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$serverBin = Join-Path $dms '101_ServerBin'
if ($BslDir -eq '') { $BslDir = $serverBin }

$results = New-Object System.Collections.Generic.List[object]
function Check($name, [bool]$ok, $detail) {
    $results.Add([pscustomobject]@{ Name = $name; Ok = $ok; Detail = $detail })
    Write-Host ("[{0}] {1} {2}" -f $(if ($ok) { 'PASS' } else { 'FAIL' }), $name, $detail)
}

# ---- 1) 소스 검사 ----
$targets = @(
    @('02_DMS\DACrux.SEMDMS.BSL\GdsMapImport.cs', @('DeleteMap', 'CopyMap', 'BeginMapImport', 'CompleteMapImport')),
    @('02_DMS\DACrux.SEMDMS.BSL\GdsChain.cs', @('DeleteChain', 'BeginChainSave', 'CompleteChainSave', 'CreateChainBulk', 'DeleteChainBulk', 'UpdateChainColorBulk')),
    @('02_DMS\DACrux.SEMDMS.BSL\GdsEtResult.cs', @('BeginEtFile', 'CompleteEtFile', 'RejudgeEtFiles', 'SaveEtLimit', 'DeleteEtLimit')),
    @('02_DMS\DACrux.SEMDMS.BSL\GdsEtResult_Yms.cs', @('PublishEtFileToYms')),
    @('02_DMS\DACrux.SEMDMS.BSL\GdsEtResult_Para.cs', @('RegisterEtParameters', 'SaveEtParameterSetup', 'DeleteEtParameters')),
    @('02_DMS\DACrux.SEMDMS.BSL\GdsEtResult_Bin.cs', @('SaveEtBins', 'DeleteEtBins')),
    @('02_DMS\DACrux.SEMDMS.BSL\KlarfUpload.cs', @('BeginUpload', 'CommitUpload', 'DeleteUploadedStep')),
    @('02_DMS\DACrux.SEMDMS.BSL\SEMConfiguration.cs', @('InsertDefectType', 'UpdateDefectType', 'DeleteDefectType')),
    @('09_TEST\DACrux.TEST.BSL\ProbeAdmin.cs', @('SetMapDef', 'UpdateMapDef', 'CreateUseMap'))
)
function Get-FirstStatements($lines, $name) {
    # 함수 선언마다(오버로드 포함) 여는 괄호 다음 두 줄을 돌려준다
    $out = @()
    for ($i = 0; $i -lt $lines.Count; $i++) {
        if ($lines[$i] -match ('^\s*public\s+[\w\.\[\]<>, ]+\s+' + [regex]::Escape($name) + '\s*\(')) {
            for ($j = $i; $j -lt [Math]::Min($i + 30, $lines.Count); $j++) {
                if ($lines[$j].Trim() -eq '{') { $out += ,(($lines[$j + 1] + ' ' + $lines[$j + 2]).Trim()); break }
            }
        }
    }
    return ,$out
}
$found = 0; $missing = @()
foreach ($t in $targets) {
    $lines = [IO.File]::ReadAllLines((Join-Path $dms $t[0]), [Text.Encoding]::UTF8)
    foreach ($m in $t[1]) {
        $heads = Get-FirstStatements $lines $m
        if ($heads.Count -eq 0) { $missing += "$m(선언 없음)"; continue }
        foreach ($h in $heads) { if ($h -match 'new (DACrux\.SEMDMS\.DSL\.)?WorkRule\(\)\.Confirm\(') { $found++ } else { $missing += $m } }
    }
}
Check '소스: 대상 함수 첫 줄 WorkRule().Confirm' ($found -eq 31 -and $missing.Count -eq 0) ("있음 $found / 31" + $(if ($missing.Count) { ", 없음: " + ($missing -join ', ') } else { '' }))

$etLines = [IO.File]::ReadAllLines((Join-Path $dms '02_DMS\DACrux.SEMDMS.BSL\GdsEtResult.cs'), [Text.Encoding]::UTF8)
$delHead = Get-FirstStatements $etLines 'DeleteEtFile'
Check '소스: 파일 삭제(DeleteEtFile)는 확인하지 않음' ($delHead.Count -eq 1 -and $delHead[0] -notmatch 'WorkRule') $delHead[0]
$viewText = [IO.File]::ReadAllText((Join-Path $dms '02_DMS\DACrux.SEMDMS.BSL\GdsMapView.cs'), [Text.Encoding]::UTF8)
$confirmInView = ([regex]::Matches($viewText, 'WorkRule\(\)\.Confirm')).Count
Check '소스: 조회 서비스(GdsMapView)에는 Confirm 없음' ($confirmInView -eq 0) "Confirm $confirmInView 곳"

# ---- 설정 / DLL ----
$probeRoot = Join-Path $env:TEMP 'ChippacWorkRuleProbe'
New-Item -ItemType Directory -Force $probeRoot | Out-Null
$probeConfig = Join-Path $probeRoot 'probe.exe.config'
Copy-Item (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config') $probeConfig -Force
Add-Type -AssemblyName System.Configuration
[AppDomain]::CurrentDomain.SetData('APP_CONFIG_FILE', $probeConfig)
$flags = [Reflection.BindingFlags]'NonPublic,Static'
$cm = [Configuration.ConfigurationManager]
$cm.GetField('s_initState', $flags).SetValue($null, 0)
$cm.GetField('s_configSystem', $flags).SetValue($null, $null)

foreach ($d in 'Oracle.ManagedDataAccess', 'Miracom.Middleware', 'DACrux.Base', 'DACrux.Data.Parser', 'DACrux.Common.DSL', 'DACrux.TEST.DSL', 'DACrux.TEST.Interface') {
    $p = Join-Path $serverBin "$d.dll"; if (Test-Path $p) { [void][Reflection.Assembly]::LoadFrom($p) }
}
$dsl = [Reflection.Assembly]::LoadFrom((Join-Path $BslDir 'DACrux.SEMDMS.DSL.dll'))
[void][Reflection.Assembly]::LoadFrom((Join-Path $BslDir 'DACrux.SEMDMS.Interface.dll'))
$testBsl = [Reflection.Assembly]::LoadFrom((Join-Path $BslDir 'DACrux.TEST.BSL.dll'))
$bsl = [Reflection.Assembly]::LoadFrom((Join-Path $BslDir 'DACrux.SEMDMS.BSL.dll'))
Write-Host "BSL / DSL: $BslDir"

$ruleType = $dsl.GetType('DACrux.SEMDMS.DSL.WorkRule', $true)
$sflags = [Reflection.BindingFlags]'NonPublic,Static'
$lastDay = [DateTime]$ruleType.GetField('LastWorkDay', $sflags).GetValue($null)
$firstDay = [DateTime]$ruleType.GetField('FirstValidDay', $sflags).GetValue($null)
$isOpen = $ruleType.GetMethod('IsOpen', $sflags)
$daysLeft = $ruleType.GetMethod('DaysLeft', $sflags)
function Open-At([DateTime]$t) { return [bool]$isOpen.Invoke($null, @([object]$t)) }

# ---- 2) 기간 판정 ----
if (-not $ExpectClosed) {
    Check '상수: 마지막 날 2027-01-31 / 하한 2026-10-01' ($lastDay -eq [DateTime]'2027-01-31' -and $firstDay -eq [DateTime]'2026-10-01') ("{0:yyyy-MM-dd} / {1:yyyy-MM-dd}" -f $lastDay, $firstDay)
}
$b = @(
    @(($firstDay.AddSeconds(-1)), $false), @($firstDay, $true),
    @(($lastDay.AddDays(1).AddSeconds(-1)), $true), @(($lastDay.AddDays(1)), $false)
)
$bOk = $true; $bText = @()
foreach ($x in $b) { $r = Open-At $x[0]; if ($r -ne $x[1]) { $bOk = $false }; $bText += ("{0:yyyy-MM-dd HH:mm:ss}={1}" -f $x[0], $(if ($r) { '통과' } else { '차단' })) }
Check '판정: 기간 경계' $bOk ($bText -join ' / ')
$d1 = [int]$daysLeft.Invoke($null, @([object]$lastDay)); $d0 = [int]$daysLeft.Invoke($null, @([object]$lastDay.AddDays(1))); $d14 = [int]$daysLeft.Invoke($null, @([object]$lastDay.AddDays(-13)))
Check '판정: 남은 일수' ($d1 -eq 1 -and $d0 -eq 0 -and $d14 -eq 14) "마지막 날 $d1 / 다음 날 $d0 / 13일 전 $d14"

# ---- 3) DB 일시 / 상태 ----
$rule = [Activator]::CreateInstance($ruleType)
$state = $ruleType.GetMethod('GetState').Invoke($rule, $null)
$row = $state.Rows[0]
$dbNow = [DateTime]::ParseExact($row['DB_TIME'], 'yyyy-MM-dd HH:mm:ss', [Globalization.CultureInfo]::InvariantCulture)
$gap = [Math]::Abs(($dbNow - (Get-Date)).TotalMinutes)
Check 'DB 일시: 한국 시간(PC 시각과 5분 이내)' ($gap -lt 5) ("DB {0} / PC {1:yyyy-MM-dd HH:mm:ss} / 차이 {2:N1}분" -f $row['DB_TIME'], (Get-Date), $gap)
$expectOpen = if ($ExpectClosed) { 'N' } else { 'Y' }
$expectDays = ($lastDay.Date - $dbNow.Date).Days + 1
Check 'GetState: 기간 상태' ($row['WORK_OPEN'] -eq $expectOpen -and [int]$row['DAYS_LEFT'] -eq $expectDays -and $row['LAST_DAY'] -eq $lastDay.ToString('yyyy-MM-dd')) ("WORK_OPEN={0} LAST_DAY={1} DAYS_LEFT={2}" -f $row['WORK_OPEN'], $row['LAST_DAY'], $row['DAYS_LEFT'])

$viewType = $bsl.GetType('DACrux.SEMDMS.BSL.GdsMapView', $true)
$view = [Activator]::CreateInstance($viewType)
$vs = $viewType.GetMethod('GetWorkState').Invoke($view, $null)
Check 'BSL GdsMapView.GetWorkState (클라이언트 메뉴용)' ($vs.TableName -eq 'WORK_STATE' -and $vs.Rows[0]['WORK_OPEN'] -eq $expectOpen) ("WORK_OPEN={0}" -f $vs.Rows[0]['WORK_OPEN'])

function Inner-Message($e) {
    $x = $e
    while ($x.InnerException -ne $null) { $x = $x.InnerException }
    return $x.Message
}
$confirm = $ruleType.GetMethod('Confirm')
$mod = [Enum]::Parse($dsl.GetType('DACrux.SEMDMS.DSL.WorkModule'), 'Gds')
$kind = [Enum]::Parse($dsl.GetType('DACrux.SEMDMS.DSL.WorkKind'), 'Setup')
$confirmMsg = ''
try { [void]$confirm.Invoke($rule, @($mod, $kind)) } catch { $confirmMsg = Inner-Message $_.Exception }
if ($ExpectClosed) {
    Check 'Confirm: 기간 문장으로 막음' ($confirmMsg -like '*POC 사용 기간*') $confirmMsg
} else {
    Check 'Confirm: 통과' ($confirmMsg -eq '') $(if ($confirmMsg) { $confirmMsg } else { '예외 없음' })
}

# ---- 4) 기간이 지난 빌드: 대상 함수가 첫 단계에서 멈추는지 ----
# 저장 함수를 실제로 부르므로, 서버가 기간 밖(WORK_OPEN=N)이라고 답할 때만 실행한다.
# 기간 안 빌드에 -ExpectClosed 를 주면 첫 줄이 통과해 함수 본문(DB 문)까지 가기 때문이다 (2026-10-29).
if ($ExpectClosed -and $row['WORK_OPEN'] -ne 'N') {
    Check '기간 후: 대상 함수 31개 첫 단계에서 멈춤' $false '기간 안 빌드라 저장 함수를 부르지 않음(DB 보호)'
}
elseif ($ExpectClosed) {
    $types = @(
        @('DACrux.SEMDMS.BSL.GdsMapImport', $bsl, @('DeleteMap', 'CopyMap', 'BeginMapImport', 'CompleteMapImport')),
        @('DACrux.SEMDMS.BSL.GdsChain', $bsl, @('DeleteChain', 'BeginChainSave', 'CompleteChainSave', 'CreateChainBulk', 'DeleteChainBulk', 'UpdateChainColorBulk')),
        @('DACrux.SEMDMS.BSL.GdsEtResult', $bsl, @('BeginEtFile', 'CompleteEtFile', 'RejudgeEtFiles', 'SaveEtLimit', 'DeleteEtLimit', 'PublishEtFileToYms', 'RegisterEtParameters', 'SaveEtParameterSetup', 'DeleteEtParameters', 'SaveEtBins', 'DeleteEtBins')),
        @('DACrux.SEMDMS.BSL.KlarfUpload', $bsl, @('BeginUpload', 'CommitUpload', 'DeleteUploadedStep')),
        @('DACrux.SEMDMS.BSL.SEMConfiguration', $bsl, @('InsertDefectType', 'UpdateDefectType', 'DeleteDefectType')),
        @('DACrux.TEST.BSL.ProbeAdmin', $testBsl, @('SetMapDef', 'UpdateMapDef', 'CreateUseMap'))
    )
    $blocked = 0; $notBlocked = @()
    foreach ($t in $types) {
        $type = $t[1].GetType($t[0], $true)
        $obj = [Activator]::CreateInstance($type)
        foreach ($name in $t[2]) {
            foreach ($mi in ($type.GetMethods([Reflection.BindingFlags]'Public,Instance,DeclaredOnly') | Where-Object { $_.Name -eq $name })) {
                $ps = $mi.GetParameters()
                $argv = New-Object object[] $ps.Count
                for ($k = 0; $k -lt $ps.Count; $k++) { if ($ps[$k].ParameterType.IsValueType) { $argv[$k] = [Activator]::CreateInstance($ps[$k].ParameterType) } }
                $msg = ''
                try { [void]$mi.Invoke($obj, $argv) } catch { $msg = Inner-Message $_.Exception }
                if ($msg -like '*POC 사용 기간*') { $blocked++ } else { $notBlocked += ("{0}.{1}({2}): {3}" -f $type.Name, $name, $ps.Count, $(if ($msg) { $msg } else { '예외 없음' })) }
            }
        }
    }
    Check '기간 후: 대상 함수 31개 첫 단계에서 멈춤' ($blocked -eq 31 -and $notBlocked.Count -eq 0) ("멈춤 $blocked / 31" + $(if ($notBlocked.Count) { ' / 안 멈춤: ' + ($notBlocked -join ' | ') } else { '' }))

    $devMsg = ''
    try { $devices = $viewType.GetMethod('GetMapDeviceList').Invoke($view, $null) } catch { $devMsg = Inner-Message $_.Exception }
    Check '기간 후: 조회(GetMapDeviceList)는 그대로' ($devMsg -eq '') $(if ($devMsg) { $devMsg } else { "Device $($devices.Rows.Count)건" })
}

$fail = @($results | Where-Object { -not $_.Ok }).Count
if ($fail -eq 0) { Write-Host "WORK_RULE_PROBE=PASS ($($results.Count))" } else { Write-Host "WORK_RULE_PROBE=FAIL ($fail / $($results.Count))"; exit 1 }
