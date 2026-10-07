# KLARF 업로드 / DB 적재 검증 스크립트 (CLAUDE-031, F08 구현 1~2단계)
# 목적: BSL KlarfUpload 가 고객 샘플 KLARF(XD106.02) + Defect 이미지 15장을 기존 TQD_* 와 이미지 저장소에 저장하고,
#       기존 Defect 조회 화면이 쓰는 BSL 함수로 그대로 조회되는지 확인한다.
#  1) 입력 검증 (항목 수 / 경로가 섞인 파일 이름 / 잘못된 업로드 키)
#  2) BeginUpload 미리보기 (DMS WaferID 규칙 / 원본 WaferID / SetupID 46자 / Defect 15 / 검사 Die 26 / 이미지 이름 15)
#  3) 이미지 15장 + CommitUpload -> 저장소 파일(원본 / thumb_ / KLARF) + 기존 화면 BSL 로 재조회
#     GetDMSStepWaferList (왼쪽 Wafer 조회, "Include Image" = IMAGES > 0 포함) / GetDefectMapInfo / GetDefectData / GetDefectMapViewer_Info / GetDefectImagePath01
#     값: Defect 1 X = -23229.419 / CLASSNUMBER = ROUGHBIN 54 (D3) / SETUP_ID 46자 (D2) / IMAGE_PATH = FTP 상대 경로 (D4)
#  4) 같은 파일 다시: 미리보기 EXISTS / 건너뜀(N) SKIPPED / 재등록(Y, 이미지 14장만) -> 이전 STEP 삭제, 없는 이미지 행 미생성
#  5) 이미지 저장소 설정(KLARF_IMAGE_ROOT)이 없는데 이미지가 있으면 DB 저장 전에 오류 (기존 데이터 그대로)
#  6) 기존 쿼리 회귀: 쿼리를 추가한 XML 의 기존 쿼리(SELECT_SETUP_SEQ / SELECT_STEP_INFO)가 기존 데이터를 찾는다
#  7) DeleteUploadedStep: 업로드 STEP 만 지운다 (자동 적재 STEP 은 COMMENT 'Parser' - 읽기만 확인)
# Service 를 띄우지 않고 BSL 을 직접 호출한다 (DB: 로컬 docker DMSMGR, TQD_SETUP.SETUP_ID 128 DDL 적용 필요).
# 설정: 101_ServerBin\DACrux.SEMDMS.Service.exe.config 를 %TEMP% 에 복사해 KLARF_IMAGE_ROOT / KLARF_UPLOAD_WORK_PATH 를 넣어 쓴다(운영 config 는 바꾸지 않음).
#       이미지 저장소는 %TEMP%\ChippacKlarfUploadProbe\DMS_FTP\KLARF_UPLOAD (매 실행 시 비움).
# 결과 데이터: 기본은 마지막에 지운다. -Keep 을 주면 마지막 저장 Wafer(이미지 15장)를 남겨 DMS 화면(왼쪽 Wafer 조회)에서 볼 수 있다.
#   SETUP / PRODUCT / LOT / WAFER 행은 실제 업로드와 같이 남는다(다른 STEP 이 함께 쓰는 행).
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File KlarfUploadProbe.ps1 [-BslDir <BSL / Interface 폴더>] [-Keep]
param(
    [string]$BslDir = '',
    [switch]$Keep
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$root = Split-Path $dms -Parent
$serverBin = Join-Path $dms '101_ServerBin'
if ($BslDir -eq '') { $BslDir = $serverBin }
$sample = Join-Path $root 'SampleFile\26_10_06 Sample File\XD106.02'
$imageDir = Join-Path $root 'SampleFile\26_10_06 Sample File\Defect Images'

# ---- 시험용 config: 운영 Service config 복사 + KLARF_IMAGE_ROOT / KLARF_UPLOAD_WORK_PATH ----
$probeRoot = Join-Path $env:TEMP 'ChippacKlarfUploadProbe'
$imageRoot = Join-Path $probeRoot 'DMS_FTP\KLARF_UPLOAD'
if (Test-Path $probeRoot) { Remove-Item $probeRoot -Recurse -Force }
New-Item -ItemType Directory -Force $imageRoot | Out-Null
$baseConfig = Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config'
$probeConfig = Join-Path $probeRoot 'probe.exe.config'
[xml]$x = Get-Content $baseConfig -Encoding UTF8
foreach ($kv in @(@('KLARF_IMAGE_ROOT', $imageRoot), @('KLARF_UPLOAD_WORK_PATH', (Join-Path $probeRoot 'work')))) {
    $old = $x.configuration.appSettings.add | Where-Object { $_.key -eq $kv[0] }
    if ($old) { [void]$x.configuration.appSettings.RemoveChild($old) }
    $n = $x.CreateElement('add'); $n.SetAttribute('key', $kv[0]); $n.SetAttribute('value', $kv[1])
    [void]$x.configuration.appSettings.AppendChild($n)
}
$x.Save($probeConfig)
# 설정 없음 시험용: 운영 config 에 KLARF_IMAGE_ROOT 가 들어 있어도(배포 후) 키를 뺀 사본을 쓴다
$noRootConfig = Join-Path $probeRoot 'noroot.exe.config'
[xml]$y = Get-Content $baseConfig -Encoding UTF8
$r = $y.configuration.appSettings.add | Where-Object { $_.key -eq 'KLARF_IMAGE_ROOT' }
if ($r) { [void]$y.configuration.appSettings.RemoveChild($r) }
$y.Save($noRootConfig)

Add-Type -AssemblyName System.Configuration
function Use-Config($path) {
    [AppDomain]::CurrentDomain.SetData('APP_CONFIG_FILE', $path)
    $flags = [Reflection.BindingFlags]'NonPublic,Static'
    $cm = [Configuration.ConfigurationManager]
    $cm.GetField('s_initState', $flags).SetValue($null, 0)
    $cm.GetField('s_configSystem', $flags).SetValue($null, $null)
}
Use-Config $probeConfig
foreach ($d in 'Oracle.ManagedDataAccess', 'Miracom.Middleware', 'DACrux.Base', 'DACrux.Data.Parser', 'DACrux.SEMDMS.DSL') {
    $p = Join-Path $serverBin "$d.dll"; if (Test-Path $p) { [void][Reflection.Assembly]::LoadFrom($p) }
}
# 새 Interface(iKlarfUpload)가 들어 있는 폴더의 Interface 를 BSL 보다 먼저 읽는다
$ifDll = Join-Path $BslDir 'DACrux.SEMDMS.Interface.dll'
$bslDll = Join-Path $BslDir 'DACrux.SEMDMS.BSL.dll'
[void][Reflection.Assembly]::LoadFrom($ifDll)
[void][Reflection.Assembly]::LoadFrom($bslDll)
Write-Host "BSL: $bslDll"
Write-Host "Sample: $sample"
Write-Host "Image root: $imageRoot"

# Middleware 가 호출 클래스를 스택에서 찾으므로 BSL 호출은 NoInlining 래퍼 안에서 한다.
$helper = @"
using System;
using System.Data;
using System.Runtime.CompilerServices;

public static class KlarfUploadProbeHelper
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static DataTable Begin(string[] info, byte[] klarf) { return new DACrux.SEMDMS.BSL.KlarfUpload().BeginUpload(info, klarf); }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void AddImage(string key, string name, byte[] data) { new DACrux.SEMDMS.BSL.KlarfUpload().AddImage(key, name, data); }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static DataTable Commit(string key, string overwrite) { return new DACrux.SEMDMS.BSL.KlarfUpload().CommitUpload(key, new string[] { overwrite }); }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string CommitError(string key, string overwrite)
    {
        try { Commit(key, overwrite); return ""; } catch (Exception ex) { return ex.Message; }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Cancel(string key) { new DACrux.SEMDMS.BSL.KlarfUpload().CancelUpload(key); }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static int Delete(long step, long wafer) { return new DACrux.SEMDMS.BSL.KlarfUpload().DeleteUploadedStep(step.ToString(), wafer.ToString(), "PROBE"); }

    /// <summary>오류 메시지를 돌려준다(오류가 없으면 빈 문자열).</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string BeginError(string[] info, byte[] klarf)
    {
        try { DataTable dt = Begin(info, klarf); Cancel(dt.Rows[0]["UPLOAD_KEY"].ToString()); return ""; }
        catch (Exception ex) { return ex.Message; }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string AddImageError(string key, string name, byte[] data)
    {
        try { AddImage(key, name, data); return ""; } catch (Exception ex) { return ex.Message; }
    }

    // ---- 기존 Defect 조회 화면이 쓰는 BSL 함수 ----
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static DataSet WaferList(string start, string end, string[] wheres)
    {
        DataSet ds = new DACrux.SEMDMS.BSL.DefectMapAnalysis().GetDMSStepWaferList(start, end, wheres, false, false);
        return ds.Copy();
    }

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

    // ---- 저장 값 / 회귀 확인 (기존 DSL 범용 조회) ----
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static DataTable Setup(long setupSeq) { return new DACrux.SEMDMS.DSL.TQD_SETUP().GetData("GET_SETUP", null, new string[] { setupSeq.ToString() }); }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static long OldSetupDuplicate(string setupId, string stepId, string setupTime) { return new DACrux.SEMDMS.BSL.DefectDefine().GetSetupDuplicate(setupId, stepId, setupTime); }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static long OldStepInfo(string waferSeq, string stepId, string resultTime) { return new DACrux.SEMDMS.BSL.DefectDefine().GetStepInfo(new string[] { waferSeq, stepId, resultTime, "1" }); }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static DataTable StepComment(long step, long wafer)
    {
        return new DACrux.SEMDMS.DSL.TQD_STEP().GetData("SELECT_STEP_KLARF_UPLOAD", null, new string[] { step.ToString(), wafer.ToString() });
    }
}
"@
Add-Type -TypeDefinition $helper -ReferencedAssemblies @($bslDll, $ifDll, (Join-Path $serverBin 'DACrux.SEMDMS.DSL.dll'),
    (Join-Path $serverBin 'Miracom.Middleware.dll'), 'System.Data', 'System.Xml')

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
function Info([string]$cls = 'ROUGHBIN') { return [string[]]@('SCK', 'PROBE', 'XD106.02', '', '', '', $cls) }
function Stop-Probe($msg) { Write-Host "KLARF_UPLOAD_PROBE=FAIL ($msg)"; exit 1 }
function Send-Images($key, [int]$count) {
    $files = @(Get-ChildItem $imageDir -Filter '*.jpg' | Sort-Object Name | Select-Object -First $count)
    foreach ($f in $files) { [KlarfUploadProbeHelper]::AddImage($key, $f.Name, [IO.File]::ReadAllBytes($f.FullName)) }
    return $files
}
function Commit-Checked($key, $overwrite, $label) {
    $t = [KlarfUploadProbeHelper]::Commit($key, $overwrite)
    $row = $t.Rows[0]
    if ($row['RESULT'] -ne 'SAVED') { Check $label $false "$($row['RESULT']) $($row['MESSAGE'])"; Stop-Probe '저장 실패로 중단' }
    return $row
}

$bytes = [IO.File]::ReadAllBytes($sample)
$lot = 'PFIC_RH-HS2615_C4BUMP'
$setupIdFull = 'ROOT-CAI-OMM_Hybrid-OMM_Hybrid_C4Bump_Silterra'
$storeFolder = Join-Path $imageRoot ((Get-Date).ToString('yyyyMMdd') + "\$lot")   # 저장일 yyyyMMdd \ Lot (2026-10-07 사용자 지시)
$expectedRel = ($storeFolder.Substring([IO.Path]::GetPathRoot($storeFolder).Length)).Replace('\', '/')

# ---- 1) 입력 검증 ----
$e = [KlarfUploadProbeHelper]::BeginError([string[]]@('SCK', 'PROBE', 'XD106.02'), $bytes)
Check '입력 항목 수 부족 -> 거부' ($e.Length -gt 0) $e
$e = [KlarfUploadProbeHelper]::BeginError([string[]]@('SCK', 'PROBE', '..\XD106.02', '', '', '', 'ROUGHBIN'), $bytes)
Check '경로가 섞인 파일 이름 -> 거부' ($e.Length -gt 0) $e
$e = [KlarfUploadProbeHelper]::AddImageError('..\..\Windows', 'a.jpg', [byte[]](1, 2, 3))
Check '잘못된 업로드 키 -> 거부' ($e.Length -gt 0) $e

# ---- 2) 미리보기 ----
$pv = [KlarfUploadProbeHelper]::Begin((Info), $bytes)
$r = $pv.Rows[0]
$key = $r['UPLOAD_KEY']
# 사용자 데이터 보호(2026-10-07): 같은 샘플 Wafer 를 사용자가 화면에서 올려 둔 경우(STEP COMMENT 의 USER 가 PROBE / SMOKE 가 아님)
# Probe 의 재등록이 그 데이터를 지우므로, 아무것도 바꾸지 않고 멈춘다. (실제로 사용자 업로드 STEP 454 가 Smoke 재등록으로 지워진 적이 있음)
if ($r['RESULT'] -eq 'EXISTS') {
    $ex = [KlarfUploadProbeHelper]::StepComment([long]$r['EXIST_STEP_SEQ'], [long]$r['WAFER_SEQ'])
    $cm = if ($ex.Rows.Count) { $ex.Rows[0]['STEP_COMMENT'].ToString() } else { '' }
    if (-not ($cm.Contains('USER=PROBE;') -or $cm.Contains('USER=SMOKE;'))) {
        [KlarfUploadProbeHelper]::Cancel($key)
        Write-Host "STOP: 사용자가 올린 데이터가 있습니다 (STEP_SEQ $($r['EXIST_STEP_SEQ']), $cm). Probe 는 재등록으로 이 데이터를 지우므로 실행하지 않습니다."
        Write-Host 'KLARF_UPLOAD_PROBE=SKIPPED (사용자 데이터 보호)'; exit 2
    }
}
Check '미리보기 Wafer 1행' ($pv.Rows.Count -eq 1) "rows=$($pv.Rows.Count) key=$key"
Check 'DMS WaferID / 원본 WaferID' (($r['WAFER_ID'] -eq "$lot-02") -and ($r['RAW_WAFER_ID'] -eq 'XD106.02')) "$($r['WAFER_ID']) / $($r['RAW_WAFER_ID'])"
Check 'SetupID 46자 / Step ID 원문' (($r['SETUP_ID'] -eq $setupIdFull) -and ($r['STEP_ID'] -eq 'OMM_Hybrid_C4Bump_Silterra')) "$($r['SETUP_ID'].Length)자 / $($r['STEP_ID'])"
Check 'Defect 15 / 검사 Die 26 / 이미지 이름 15' (($r['DEFECTS'] -eq 15) -and ($r['INSPECTED_DIE'] -eq 26) -and ($r['IMAGE_FILES'] -eq 15)) "$($r['DEFECTS']) / $($r['INSPECTED_DIE']) / $($r['IMAGE_FILES'])"
Check '미리보기 상태 READY 또는 EXISTS' (@('READY', 'EXISTS') -contains $r['RESULT']) "$($r['RESULT']) $($r['MESSAGE'])"
$e = [KlarfUploadProbeHelper]::AddImageError($key, '..\evil.jpg', [byte[]](1, 2, 3))
Check '이미지 이름에 경로 -> 거부' ($e.Length -gt 0) $e

# ---- 3) 이미지 15장 저장 + 기존 화면 BSL 재조회 ----
$sent = Send-Images $key 15
$c = Commit-Checked $key 'Y' '저장 SAVED (이미지 15)'
$step = [long]$c['STEP_SEQ']; $wafer = [long]$c['WAFER_SEQ']; $setup = [long]$c['SETUP_SEQ']
Check '저장 SAVED / 이미지 15' (($c['RESULT'] -eq 'SAVED') -and ([int]$c['SAVED_IMAGES'] -eq 15)) "STEP_SEQ=$step WAFER_SEQ=$wafer SETUP_SEQ=$setup 이미지=$($c['SAVED_IMAGES']) $($c['ELAPSED_MS'])ms $($c['MESSAGE'])"
Check '저장 경로 = FTP 상대 경로 (D4)' ($c['IMAGE_PATH'] -eq $expectedRel) "$($c['IMAGE_PATH'])"
$jpg = @(Get-ChildItem $storeFolder -Filter '*.jpg' | Where-Object { -not $_.Name.StartsWith('thumb_') }).Count
$thumb = @(Get-ChildItem $storeFolder -Filter 'thumb_*.jpg').Count
Check '저장소 파일: 원본 15 / 썸네일 15 / KLARF' (($jpg -eq 15) -and ($thumb -eq 15) -and (Test-Path (Join-Path $storeFolder 'XD106.02'))) "jpg=$jpg thumb=$thumb klarf=$(Test-Path (Join-Path $storeFolder 'XD106.02'))"

$wl = [KlarfUploadProbeHelper]::WaferList('2026-06-05', '2026-06-06', [string[]]@("LOT_ID = '$lot'"))
$found = @($wl.Tables['STEPLIST'].Rows | Where-Object { [long]$_['STEP_SEQ'] -eq $step })
Check '왼쪽 Wafer 조회(GetDMSStepWaferList)에 표시' ($found.Count -ge 1) "STEPLIST $($wl.Tables['STEPLIST'].Rows.Count)행 중 $($found.Count)"
$wli = [KlarfUploadProbeHelper]::WaferList('2026-06-05', '2026-06-06', [string[]]@("LOT_ID = '$lot'", ' IMAGES > 0 '))
$foundI = @($wli.Tables['STEPLIST'].Rows | Where-Object { [long]$_['STEP_SEQ'] -eq $step })
Check '"Include Image"(IMAGES > 0) 조건에도 표시' ($foundI.Count -ge 1) "$($foundI.Count)"
$mi = [KlarfUploadProbeHelper]::MapInfo($step)
Check 'Map 정보(GetDefectMapInfo) MAP 1 / DIE 26' (($mi.Tables['MAP'].Rows.Count -eq 1) -and ($mi.Tables['DIE'].Rows.Count -eq 26)) "MAP $($mi.Tables['MAP'].Rows.Count) / DIE $($mi.Tables['DIE'].Rows.Count)"
$df = [KlarfUploadProbeHelper]::Defects($step)
$d1 = @($df.Rows | Where-Object { [int]$_['DEFECTID'] -eq 1 })[0]
Check 'Defect 15건(GetDefectData)' ($df.Rows.Count -eq 15) "$($df.Rows.Count)"
Check 'Defect 1 X = -23229.419 / Y = 86770.97' (([math]::Abs([double]$d1['X'] + 23229.419) -lt 0.001) -and ([math]::Abs([double]$d1['Y'] - 86770.97) -lt 0.001)) "X=$($d1['X']) Y=$($d1['Y']) XINDEX=$($d1['XINDEX']) YINDEX=$($d1['YINDEX'])"
Check 'CLASSNUMBER = ROUGHBIN 54 (D3)' (([int]$d1['CLASSNUMBER'] -eq 54) -and ([int]$d1['ROUGHBINNUMBER'] -eq 54)) "CLASS=$($d1['CLASSNUMBER']) ROUGH=$($d1['ROUGHBINNUMBER'])"
Check 'Defect 1 IMAGECOUNT = 1' ([int]$d1['IMAGECOUNT'] -eq 1) "$($d1['IMAGECOUNT'])"
$ip = [KlarfUploadProbeHelper]::ImagePaths($step)
$ip1 = @($ip.Rows | Where-Object { [int]$_['DEFECTID'] -eq 1 })[0]
Check '이미지 경로(GetDefectImagePath01) 15행 / Defect 1' (($ip.Rows.Count -eq 15) -and ($ip1['IMAGE_PATH'] -eq $expectedRel) -and ($ip1['IMAGE_FILENAME'] -eq 'XD106.02_-1_3_0_1_RDLshort_54.jpg') -and ($ip1['THUMB_FILENAME'] -eq 'thumb_XD106.02_-1_3_0_1_RDLshort_54.jpg')) "$($ip.Rows.Count)행 / $($ip1['IMAGE_PATH'])/$($ip1['IMAGE_FILENAME'])"
$e = [KlarfUploadProbeHelper]::ViewerInfoError($step)
Check 'Defect Map Viewer 정보(GetDefectMapViewer_Info) 예외 없음' ($e.Length -eq 0) $e
$su = [KlarfUploadProbeHelper]::Setup($setup)
Check 'TQD_SETUP.SETUP_ID 46자 그대로 (D2)' ($su.Rows.Count -eq 1 -and $su.Rows[0]['SETUP_ID'] -eq $setupIdFull) "$(if ($su.Rows.Count) { $su.Rows[0]['SETUP_ID'] })"
$sc = [KlarfUploadProbeHelper]::StepComment($step, $wafer)
Check 'STEP COMMENT 업로드 구분 / 원본 WaferID' ($sc.Rows.Count -eq 1 -and $sc.Rows[0]['STEP_COMMENT'].ToString().StartsWith('UPLOAD;') -and $sc.Rows[0]['STEP_COMMENT'].ToString().Contains('RAW_WAFER_ID=XD106.02')) "$(if ($sc.Rows.Count) { $sc.Rows[0]['STEP_COMMENT'] })"

# ---- 4) 다시 올리기 ----
$pv2 = [KlarfUploadProbeHelper]::Begin((Info), $bytes)
Check '다시 미리보기 EXISTS (같은 STEP_SEQ)' (($pv2.Rows[0]['RESULT'] -eq 'EXISTS') -and ([long]$pv2.Rows[0]['EXIST_STEP_SEQ'] -eq $step)) "$($pv2.Rows[0]['RESULT']) $($pv2.Rows[0]['EXIST_STEP_SEQ'])"
$cr2 = [KlarfUploadProbeHelper]::Commit($pv2.Rows[0]['UPLOAD_KEY'], 'N')
Check '건너뜀(N) -> SKIPPED' ($cr2.Rows[0]['RESULT'] -eq 'SKIPPED') "$($cr2.Rows[0]['RESULT']) $($cr2.Rows[0]['MESSAGE'])"
$pv3 = [KlarfUploadProbeHelper]::Begin((Info), $bytes)
[void](Send-Images $pv3.Rows[0]['UPLOAD_KEY'] 14)
$c3 = Commit-Checked $pv3.Rows[0]['UPLOAD_KEY'] 'Y' '재등록(Y) 저장'
$step2 = [long]$c3['STEP_SEQ']
Check '재등록(Y) -> 새 STEP / 이전 STEP 삭제 기록' (($step2 -ne $step) -and ([long]$c3['DELETED_STEP_SEQ'] -eq $step)) "new=$step2 deleted=$($c3['DELETED_STEP_SEQ']) $($c3['MESSAGE'])"
Check '이미지 14장만 보냄 -> 이미지 행 14 (없는 파일은 행 없음)' (([int]$c3['SAVED_IMAGES'] -eq 14) -and (([KlarfUploadProbeHelper]::ImagePaths($step2)).Rows.Count -eq 14)) "$($c3['SAVED_IMAGES'])"
Check '이전 STEP Defect / 이미지 0 (삭제됨)' ((([KlarfUploadProbeHelper]::Defects($step)).Rows.Count -eq 0) -and (([KlarfUploadProbeHelper]::ImagePaths($step)).Rows.Count -eq 0)) ''

# ---- 5) 이미지 저장소 설정 없음 -> DB 저장 전 오류 ----
Use-Config $noRootConfig
$pv4 = [KlarfUploadProbeHelper]::Begin((Info), $bytes)
[void](Send-Images $pv4.Rows[0]['UPLOAD_KEY'] 1)
$e = [KlarfUploadProbeHelper]::CommitError($pv4.Rows[0]['UPLOAD_KEY'], 'Y')
Use-Config $probeConfig
Check '저장소 설정 없음 + 이미지 -> 오류' ($e.Contains('KLARF_IMAGE_ROOT')) $e
Check '오류 후 기존 STEP 그대로' ((([KlarfUploadProbeHelper]::Defects($step2)).Rows.Count -eq 15)) "STEP_SEQ=$step2"

# 마지막 상태: 이미지 15장으로 다시 저장 (-Keep 이면 이 Wafer 를 남긴다)
$pv5 = [KlarfUploadProbeHelper]::Begin((Info), $bytes)
[void](Send-Images $pv5.Rows[0]['UPLOAD_KEY'] 15)
$c5 = Commit-Checked $pv5.Rows[0]['UPLOAD_KEY'] 'Y' '최종 저장'
$step3 = [long]$c5['STEP_SEQ']
Check '최종 재등록 이미지 15' ([int]$c5['SAVED_IMAGES'] -eq 15) "STEP_SEQ=$step3"

# ---- 6) 기존 쿼리 회귀 (XML 에 쿼리를 추가한 파일) ----
$old = [KlarfUploadProbeHelper]::OldSetupDuplicate('SW73_TASH_PAD_2', 'TASH_PAD_2', '20200806191135')
Check '기존 SELECT_SETUP_SEQ 가 기존 Setup 261 을 찾음' ($old -eq 261) "$old"
$oldStep = [KlarfUploadProbeHelper]::OldStepInfo('181', 'TASH_PAD_2', '20190507001230')
Check '기존 SELECT_STEP_INFO 가 기존 STEP 401 을 찾음' ($oldStep -eq 401) "$oldStep"

# ---- 7) 삭제 대상 제한 ----
$auto = [KlarfUploadProbeHelper]::StepComment(401, 181)
Check '자동 적재 STEP 401 COMMENT 는 업로드 아님(삭제 대상 아님, 읽기만)' ($auto.Rows.Count -eq 1 -and -not $auto.Rows[0]['STEP_COMMENT'].ToString().StartsWith('UPLOAD;')) "$(if ($auto.Rows.Count) { $auto.Rows[0]['STEP_COMMENT'] })"
Check '없는 STEP 삭제 -> 0' (([KlarfUploadProbeHelper]::Delete(999999999, 999999999)) -eq 0) ''
$leftWork = @(Get-ChildItem (Join-Path $probeRoot 'work') -Directory -ErrorAction SilentlyContinue).Count
Check '작업 폴더 정리 (Commit 후 남은 폴더 0)' ($leftWork -eq 0) "$leftWork"
if (-not $Keep) {
    Check '업로드 STEP 삭제 -> 1' (([KlarfUploadProbeHelper]::Delete($step3, $wafer)) -eq 1) "STEP_SEQ=$step3"
    Check '삭제 후 Defect / 이미지 0' ((([KlarfUploadProbeHelper]::Defects($step3)).Rows.Count -eq 0) -and (([KlarfUploadProbeHelper]::ImagePaths($step3)).Rows.Count -eq 0)) ''
} else {
    Write-Host "KEEP: STEP_SEQ=$step3 WAFER_SEQ=$wafer 를 남김 (DMS 왼쪽 Wafer 조회: Lot $lot / 기간 2026-06-05 포함). 이미지 파일: $storeFolder"
}

$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host "KLARF_UPLOAD_PROBE=PASS ($($results.Count))" } else { Write-Host "KLARF_UPLOAD_PROBE=FAIL ($fail / $($results.Count))"; exit 1 }
