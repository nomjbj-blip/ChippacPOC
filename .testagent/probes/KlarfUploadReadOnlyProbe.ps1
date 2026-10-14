# KlarfUploadReadOnly Probe (2026-10-12, program-verify 파일럿)
# 목적: 샘플 Wafer(XD106.02)에 사용자가 이미 올린 업로드 STEP 이 있어 KlarfUploadProbe.ps1 이 SKIPPED 될 때,
#       DB 를 바꾸지 않고 그 실제 업로드 데이터를 기존 Defect 조회 화면 BSL 로 다시 읽어 저장 규칙(D2 / D3 / D4)을 확인한다.
#       업로드 STEP 은 BeginUpload 미리보기(DB 쓰기 없음, 작업 폴더만 만들고 Cancel)의 EXIST_STEP_SEQ 로 찾는다.
# 전제: 로컬 docker oracle19c-DMS(SID DMS) DMSMGR 기동(healthy), 101_ServerBin 배포본(BSL / Interface 에 KlarfUpload 포함)
# 결과 데이터: DB 쓰기 없음. 작업 폴더는 %TEMP%\TA_KlarfUploadReadOnly\work (매 실행 시 비움). 운영 config 는 읽기만.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File KlarfUploadReadOnlyProbe.ps1 [-BinDir <BSL / Interface 폴더>] [-RedCheck]
#       -RedCheck: Defect 기대값을 일부러 16 으로 바꿔 이 Probe 가 실제로 FAIL 할 수 있는지 확인(red-green)
# 출력: 'PASS / 이름 / 상세' 줄들 + 마지막 KLARF_UPLOAD_READ_ONLY_PROBE=PASS|FAIL (n)|SKIPPED, 종료 코드 0 / 1 / 2
param(
    [string]$BinDir = '',
    [switch]$RedCheck
)
$ErrorActionPreference = 'Stop'
$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
function Skip($reason) { Write-Host "KLARF_UPLOAD_READ_ONLY_PROBE=SKIPPED ($reason)"; exit 2 }

$root = (Resolve-Path (Join-Path $PSScriptRoot '..\..')).Path
$serverBin = Join-Path $root 'DMS\101_ServerBin'
if ($BinDir -eq '') { $BinDir = $serverBin }
$sample = Join-Path $root 'SampleFile\26_10_06 Sample File\XD106.02'

# ---- 시험용 config: 작업 폴더만 TEMP 로 (CleanupOldWorkFolders 가 다른 작업 폴더를 건드리지 않게) ----
$probeRoot = Join-Path $env:TEMP 'TA_KlarfUploadReadOnly'
if (Test-Path $probeRoot) { Remove-Item $probeRoot -Recurse -Force }
New-Item -ItemType Directory -Force (Join-Path $probeRoot 'work') | Out-Null
$probeConfig = Join-Path $probeRoot 'probe.exe.config'
[xml]$x = Get-Content (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config') -Encoding UTF8
$old = $x.configuration.appSettings.add | Where-Object { $_.key -eq 'KLARF_UPLOAD_WORK_PATH' }
if ($old) { [void]$x.configuration.appSettings.RemoveChild($old) }
$n = $x.CreateElement('add'); $n.SetAttribute('key', 'KLARF_UPLOAD_WORK_PATH'); $n.SetAttribute('value', (Join-Path $probeRoot 'work'))
[void]$x.configuration.appSettings.AppendChild($n)
$x.Save($probeConfig)

Add-Type -AssemblyName System.Configuration
[AppDomain]::CurrentDomain.SetData('APP_CONFIG_FILE', $probeConfig)
$flags = [Reflection.BindingFlags]'NonPublic,Static'
$cm = [Configuration.ConfigurationManager]
$cm.GetField('s_initState', $flags).SetValue($null, 0)
$cm.GetField('s_configSystem', $flags).SetValue($null, $null)
foreach ($d in 'Oracle.ManagedDataAccess', 'Miracom.Middleware', 'DACrux.Base', 'DACrux.Data.Parser', 'DACrux.SEMDMS.DSL') {
    $p = Join-Path $serverBin "$d.dll"; if (Test-Path $p) { [void][Reflection.Assembly]::LoadFrom($p) }
}
$ifDll = Join-Path $BinDir 'DACrux.SEMDMS.Interface.dll'
$bslDll = Join-Path $BinDir 'DACrux.SEMDMS.BSL.dll'
[void][Reflection.Assembly]::LoadFrom($ifDll)
[void][Reflection.Assembly]::LoadFrom($bslDll)
Write-Host "BSL: $bslDll"

# 미들웨어가 호출 클래스를 스택에서 찾으므로 NoInlining 래퍼 안에서 호출한다 (KlarfUploadProbe.ps1 과 같은 호출).
$helper = @'
using System;
using System.Data;
using System.Runtime.CompilerServices;
public static class KlarfReadOnlyHelper
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static DataTable Preview(string[] info, byte[] klarf) { return new DACrux.SEMDMS.BSL.KlarfUpload().BeginUpload(info, klarf); }
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Cancel(string key) { new DACrux.SEMDMS.BSL.KlarfUpload().CancelUpload(key); }
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static DataSet WaferList(string start, string end, string[] wheres) { return new DACrux.SEMDMS.BSL.DefectMapAnalysis().GetDMSStepWaferList(start, end, wheres, false, false).Copy(); }
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static DataSet MapInfo(long step) { return new DACrux.SEMDMS.BSL.DefectMapAnalysis().GetDefectMapInfo(step); }
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static DataTable Defects(long step) { return new DACrux.SEMDMS.BSL.DefectMapAnalysis().GetDefectData(step); }
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static DataTable ImagePaths(long step) { return new DACrux.SEMDMS.BSL.DefectMapAnalysis().GetDefectImagePath01(new long[] { step }); }
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string ViewerInfoError(long step)
    {
        try { new DACrux.SEMDMS.BSL.DefectMapAnalysis().GetDefectMapViewer_Info(new long[] { step }); return ""; }
        catch (Exception ex) { return ex.Message; }
    }
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static DataTable Setup(long setupSeq) { return new DACrux.SEMDMS.DSL.TQD_SETUP().GetData("GET_SETUP", null, new string[] { setupSeq.ToString() }); }
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static DataTable StepComment(long step, long wafer) { return new DACrux.SEMDMS.DSL.TQD_STEP().GetData("SELECT_STEP_KLARF_UPLOAD", null, new string[] { step.ToString(), wafer.ToString() }); }
}
'@
Add-Type -TypeDefinition $helper -ReferencedAssemblies @($bslDll, $ifDll, (Join-Path $serverBin 'DACrux.SEMDMS.DSL.dll'),
    (Join-Path $serverBin 'Miracom.Middleware.dll'), 'System.Data', 'System.Xml')

# 기대값: 원본 KLARF XD106.02 / 사용자 결정 D2·D3·D4 / KlarfUploadProbe.ps1 실측값과 같은 리터럴
$lot = 'PFIC_RH-HS2615_C4BUMP'
$setupIdFull = 'ROOT-CAI-OMM_Hybrid-OMM_Hybrid_C4Bump_Silterra'
$expectDefects = if ($RedCheck) { 16 } else { 15 }

# ---- 1) 미리보기로 기존 업로드 STEP 찾기 (DB 쓰기 없음) ----
$pv = [KlarfReadOnlyHelper]::Preview([string[]]@('SCK', 'PROBE', 'XD106.02', '', '', '', 'ROUGHBIN'), [IO.File]::ReadAllBytes($sample))
$r = $pv.Rows[0]
[KlarfReadOnlyHelper]::Cancel($r['UPLOAD_KEY'])
if ($r['RESULT'] -ne 'EXISTS') { Skip "업로드된 STEP 없음(RESULT=$($r['RESULT'])) — 이 경우는 KlarfUploadProbe.ps1 로 검증" }
$step = [long]$r['EXIST_STEP_SEQ']; $wafer = [long]$r['WAFER_SEQ']
Write-Host "STEP_SEQ=$step"
Write-Host "WAFER_SEQ=$wafer"
$leftWork = @(Get-ChildItem (Join-Path $probeRoot 'work') -Directory -ErrorAction SilentlyContinue).Count
Check '미리보기 후 Cancel 로 작업 폴더 0 (DB 쓰기 없는 경로)' ($leftWork -eq 0) "$leftWork"

# ---- 2) 업로드 구분 (STEP COMMENT) ----
$sc = [KlarfReadOnlyHelper]::StepComment($step, $wafer)
$comment = if ($sc.Rows.Count) { $sc.Rows[0]['STEP_COMMENT'].ToString() } else { '' }
Write-Host "STEP_COMMENT=$comment"
Check 'STEP COMMENT 업로드 구분 / 원본 WaferID' ($comment.StartsWith('UPLOAD;') -and $comment.Contains('RAW_WAFER_ID=XD106.02')) $comment
$classSrc = if ($comment -match 'CLASS_SRC=([A-Z]+)') { $Matches[1] } else { '' }

# ---- 3) 기존 Defect 조회 화면 BSL 로 재조회 ----
$wl = [KlarfReadOnlyHelper]::WaferList('2026-06-05', '2026-06-06', [string[]]@("LOT_ID = '$lot'"))
$found = @($wl.Tables['STEPLIST'].Rows | Where-Object { [long]$_['STEP_SEQ'] -eq $step })
Check '왼쪽 Wafer 조회(GetDMSStepWaferList)에 표시' ($found.Count -ge 1) "STEPLIST $($wl.Tables['STEPLIST'].Rows.Count)행 중 $($found.Count)"

$mi = [KlarfReadOnlyHelper]::MapInfo($step)
Check 'Map 정보(GetDefectMapInfo) MAP 1 / DIE 26 (원본 검사 Die 26)' (($mi.Tables['MAP'].Rows.Count -eq 1) -and ($mi.Tables['DIE'].Rows.Count -eq 26)) "MAP $($mi.Tables['MAP'].Rows.Count) / DIE $($mi.Tables['DIE'].Rows.Count)"

$df = [KlarfReadOnlyHelper]::Defects($step)
Check "Defect $expectDefects 건(GetDefectData, 원본 KLARF)" ($df.Rows.Count -eq $expectDefects) "$($df.Rows.Count)"
$d1 = @($df.Rows | Where-Object { [int]$_['DEFECTID'] -eq 1 })[0]
if ($d1) {
    Check 'Defect 1 X = -23229.419 / Y = 86770.97 (원본 KLARF)' (([math]::Abs([double]$d1['X'] + 23229.419) -lt 0.001) -and ([math]::Abs([double]$d1['Y'] - 86770.97) -lt 0.001)) "X=$($d1['X']) Y=$($d1['Y'])"
    if ($classSrc -eq 'ROUGHBIN') {
        Check 'CLASSNUMBER = ROUGHBIN 54 (D3, CLASS_SRC=ROUGHBIN)' (([int]$d1['CLASSNUMBER'] -eq 54) -and ([int]$d1['ROUGHBINNUMBER'] -eq 54)) "CLASS=$($d1['CLASSNUMBER']) ROUGH=$($d1['ROUGHBINNUMBER'])"
    } else {
        Write-Host "INFO / CLASSNUMBER 검사 생략 / 사용자가 분류 기준 $classSrc 로 올림 (D3 기대값은 ROUGHBIN 기준)"
    }
} else { Check 'Defect 1 존재' $false '없음' }

$ip = [KlarfReadOnlyHelper]::ImagePaths($step)
Write-Host "IMAGE_ROWS=$($ip.Rows.Count)"
$absolute = @($ip.Rows | Where-Object { $_['IMAGE_PATH'].ToString() -match '^[A-Za-z]:' -or $_['IMAGE_PATH'].ToString().Contains('\') })
Check '이미지 경로 = 드라이브 없는 / 상대 경로 (D4)' ($absolute.Count -eq 0) "이미지 행 $($ip.Rows.Count) 중 절대/역슬래시 경로 $($absolute.Count)$(if ($ip.Rows.Count) { ' 예: ' + $ip.Rows[0]['IMAGE_PATH'] })"
$imgDefects = @($df.Rows | Where-Object { [int]$_['IMAGECOUNT'] -gt 0 }).Count
Check '이미지 행 수 = IMAGECOUNT>0 Defect 수 (없는 파일 행 미생성)' ($ip.Rows.Count -eq $imgDefects) "이미지 행 $($ip.Rows.Count) / IMAGECOUNT>0 Defect $imgDefects"

$e = [KlarfReadOnlyHelper]::ViewerInfoError($step)
Check 'Defect Map Viewer 정보(GetDefectMapViewer_Info) 예외 없음' ($e.Length -eq 0) $e

# ---- 4) SETUP_ID 46자 (D2) — MAP 결과에 SETUP_SEQ 가 있으면 확인 ----
$map = $mi.Tables['MAP']
if ($map.Columns.Contains('SETUP_SEQ') -and $map.Rows.Count -eq 1) {
    $su = [KlarfReadOnlyHelper]::Setup([long]$map.Rows[0]['SETUP_SEQ'])
    $sid = if ($su.Rows.Count) { $su.Rows[0]['SETUP_ID'].ToString() } else { '' }
    Check 'TQD_SETUP.SETUP_ID 46자 그대로 (D2)' ($sid -eq $setupIdFull) "$($sid.Length)자 $sid"
} else {
    Write-Host ("INFO / SETUP_ID 검사 생략 / GetDefectMapInfo MAP 열에 SETUP_SEQ 없음: {0}" -f (($map.Columns | ForEach-Object { $_.ColumnName }) -join ','))
}

$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($results.Count -eq 0) { Write-Host 'KLARF_UPLOAD_READ_ONLY_PROBE=FAIL (검사 0건)'; exit 1 }
if ($fail -eq 0) { Write-Host "KLARF_UPLOAD_READ_ONLY_PROBE=PASS ($($results.Count))" } else { Write-Host "KLARF_UPLOAD_READ_ONLY_PROBE=FAIL ($fail / $($results.Count))"; exit 1 }
