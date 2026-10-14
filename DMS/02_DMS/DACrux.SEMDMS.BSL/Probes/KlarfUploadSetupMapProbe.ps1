# KLARF 업로드 Map Setup 기준 저장 검증 스크립트 (F08, 2026-10-13)
# 목적: 업로드가 Defect 조회 화면용 TQD_SETUP / TQD_SETUP_MAP 을 KLARF 검사 Die 가 아니라 Map Setup(TQP_MAPDEF / TQP_USEMAP) 기준으로 저장하는지 확인한다.
#  1) 미리보기: Map Setup 사용 Die 수(MAP_DIES) / 원점 번호 / Map 밖 Defect 없음 / Map 없는 Device 는 INVALID
#  2) 저장: TQD_SETUP 값 = Map Setup 값, Defect Map Viewer 가 읽는 GetDefectMapViewer_Map = Map Setup 사용 Die 전체,
#     GetDefectMapViewer_Info 오류 없음, Defect 번호가 모두 Map 안
#  3) Defect 번호 독립 확인: 사용자가 예전 규칙(KLARF 원점)으로 올린 같은 Wafer(샘플 원본)의 Defect 번호와 같다 (읽기만)
#  4) 재업로드 교체: Setup / Setup Map 을 예전 규칙 값(검사 Die 26 / KLARF 와 다른 Pitch)으로 바꿔 둔 뒤 재등록하면 Map Setup 값으로 돌아온다
#  5) 다시 올리기: 미리보기 EXISTS / 건너뜀(N) SKIPPED
# 사용자 데이터 보호: 샘플 KLARF 사본의 LotID / SetupID 시각 / ResultTimestamp 만 바꿔 별도 Setup / Lot / Wafer 로 저장한다.
#   사용자가 올린 Setup(같은 SetupID, 원래 시각)과 Wafer 는 조회만 한다.
# 결과 데이터: 기본은 마지막에 STEP 을 지운다(DeleteUploadedStep). SETUP / SETUP_MAP / LOT / WAFER 는 실제 업로드처럼 남는다(다음 실행에서 재사용).
#   -Keep 을 주면 STEP 도 남겨 DMS 왼쪽 Wafer 조회(Lot PROBE_SETUPMAP, 기간 2020-01-02)에서 Defect Map Viewer 로 볼 수 있다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File KlarfUploadSetupMapProbe.ps1 [-BslDir <BSL / Interface 폴더>] [-Keep]
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

# ---- 시험용 config: 운영 Service config 복사 + KLARF_IMAGE_ROOT / KLARF_UPLOAD_WORK_PATH (운영 config 는 바꾸지 않음) ----
$probeRoot = Join-Path $env:TEMP 'ChippacKlarfSetupMapProbe'
$imageRoot = Join-Path $probeRoot 'DMS_FTP\KLARF_UPLOAD'
if (Test-Path $probeRoot) { Remove-Item $probeRoot -Recurse -Force }
New-Item -ItemType Directory -Force $imageRoot | Out-Null
$probeConfig = Join-Path $probeRoot 'probe.exe.config'
[xml]$x = Get-Content (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config') -Encoding UTF8
foreach ($kv in @(@('KLARF_IMAGE_ROOT', $imageRoot), @('KLARF_UPLOAD_WORK_PATH', (Join-Path $probeRoot 'work')))) {
    $old = $x.configuration.appSettings.add | Where-Object { $_.key -eq $kv[0] }
    if ($old) { [void]$x.configuration.appSettings.RemoveChild($old) }
    $n = $x.CreateElement('add'); $n.SetAttribute('key', $kv[0]); $n.SetAttribute('value', $kv[1])
    [void]$x.configuration.appSettings.AppendChild($n)
}
$x.Save($probeConfig)

Add-Type -AssemblyName System.Configuration
[AppDomain]::CurrentDomain.SetData('APP_CONFIG_FILE', $probeConfig)
$flags = [Reflection.BindingFlags]'NonPublic,Static'
[Configuration.ConfigurationManager].GetField('s_initState', $flags).SetValue($null, 0)
[Configuration.ConfigurationManager].GetField('s_configSystem', $flags).SetValue($null, $null)
foreach ($d in 'Oracle.ManagedDataAccess', 'Miracom.Middleware', 'DACrux.Base', 'DACrux.Data.Parser', 'DACrux.SEMDMS.DSL', 'DACrux.TEST.DSL') {
    $p = Join-Path $serverBin "$d.dll"; if (Test-Path $p) { [void][Reflection.Assembly]::LoadFrom($p) }
}
$ifDll = Join-Path $BslDir 'DACrux.SEMDMS.Interface.dll'
if (-not (Test-Path $ifDll)) { $ifDll = Join-Path $serverBin 'DACrux.SEMDMS.Interface.dll' }
$bslDll = Join-Path $BslDir 'DACrux.SEMDMS.BSL.dll'
[void][Reflection.Assembly]::LoadFrom($ifDll)
[void][Reflection.Assembly]::LoadFrom($bslDll)
Write-Host "BSL: $bslDll"

# Middleware 가 호출 클래스를 스택에서 찾으므로 BSL / DSL 호출은 NoInlining 래퍼 안에서 한다.
$helper = @"
using System;
using System.Data;
using System.Runtime.CompilerServices;

public static class SetupMapProbeHelper
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static DataTable Begin(string[] info, byte[] klarf) { return new DACrux.SEMDMS.BSL.KlarfUpload().BeginUpload(info, klarf); }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static DataTable Commit(string key, string overwrite) { return new DACrux.SEMDMS.BSL.KlarfUpload().CommitUpload(key, new string[] { overwrite }); }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Cancel(string key) { new DACrux.SEMDMS.BSL.KlarfUpload().CancelUpload(key); }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static int Delete(long step, long wafer) { return new DACrux.SEMDMS.BSL.KlarfUpload().DeleteUploadedStep(step.ToString(), wafer.ToString(), "PROBE"); }

    // ---- 기존 Defect Map Viewer 가 쓰는 BSL 함수 (RO 와 같이 압축을 푼다) ----
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static DataTable ViewerMap(long step, string setup, string test)
    {
        byte[] b = new DACrux.SEMDMS.BSL.DefectMapAnalysis().GetDefectMapViewer_Map(new long[] { step }, new string[] { setup }, new string[] { test });
        return DACrux.Base.Util.CompressedBytesToObject(b) as DataTable;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static DataSet ViewerInfo(long step) { return new DACrux.SEMDMS.BSL.DefectMapAnalysis().GetDefectMapViewer_Info(new long[] { step }); }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static DataTable Defects(long step) { return new DACrux.SEMDMS.BSL.DefectMapAnalysis().GetDefectData(step); }

    // ---- 저장 값 확인 / 예전 규칙 데이터 만들기 (기존 DSL 범용 함수 + 이번에 추가한 쿼리) ----
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static DataTable Setup(long setupSeq) { return new DACrux.SEMDMS.DSL.TQD_SETUP().GetData("GET_SETUP", null, new string[] { setupSeq.ToString() }); }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static DataTable SetupMap(long setupSeq, string test) { return new DACrux.SEMDMS.DSL.TQD_SETUP_MAP().GetData("SELECT_MAP_TEST_DATA", null, new string[] { setupSeq.ToString(), test }); }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void MakeOldRule(long setupSeq, string test, string[,] dies)
    {
        new DACrux.SEMDMS.DSL.TQD_SETUP_MAP().DeleteDataNonQuery("DELETE_SETUP_MAP_KLARF_UPLOAD", null, new string[] { setupSeq.ToString(), test });
        new DACrux.SEMDMS.DSL.TQD_SETUP_MAP().InsertExecuteMultiple("CREATE_TQD_SETUP_MAP", dies);
        new DACrux.SEMDMS.DSL.TQD_SETUP().UpdateDataNonQuery("UPDATE_SETUP_INFO_KLARF_UPLOAD", null,
            new string[] { "300000", "23000", "30000", "6", "4", "0", "0", setupSeq.ToString() });
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static DataTable MapDef(string mapId) { return new DACrux.TEST.DSL.TQP_MAPDEF().GetMapDef(mapId); }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static DataTable UseMap(string mapId) { return new DACrux.TEST.DSL.TQP_USEMAP().GetDies(mapId); }
}
"@
Add-Type -TypeDefinition $helper -ReferencedAssemblies @($bslDll, $ifDll, (Join-Path $serverBin 'DACrux.SEMDMS.DSL.dll'),
    (Join-Path $serverBin 'DACrux.TEST.DSL.dll'), (Join-Path $serverBin 'DACrux.Base.dll'),
    (Join-Path $serverBin 'Miracom.Middleware.dll'), 'System.Data', 'System.Xml')

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
function Info([string]$file, [string]$product = '') { return [string[]]@('SCK', 'PROBE', $file, $product, '', '', 'ROUGHBIN') }
function Key-Set($table, $xCol, $yCol) {
    $s = New-Object 'Collections.Generic.HashSet[string]'
    foreach ($r in $table.Rows) { [void]$s.Add("$([int]$r[$xCol]),$([int]$r[$yCol])") }
    return ,$s
}

# ---- 기대값: Map Setup(SCK / OMM_Hybrid) 을 저장 방향에서 LeftBottom 으로 (File Map Setup 과 같은 식, 독립 계산) ----
$def = @([SetupMapProbeHelper]::MapDef('OMM_Hybrid').Rows | Where-Object { $_['FACTORY'] -eq 'SCK' })[0]
if (-not $def) { Write-Host 'KLARF_SETUPMAP_PROBE=FAIL (Map Setup SCK / OMM_Hybrid 없음)'; exit 1 }
$dir = [int]$def['XY_DIRECTION']; $sumX = [int]$def['DIE_INDEX_MIN_X'] + [int]$def['DIE_INDEX_MAX_X']; $sumY = [int]$def['DIE_INDEX_MIN_Y'] + [int]$def['DIE_INDEX_MAX_Y']
$expected = New-Object 'Collections.Generic.HashSet[string]'
foreach ($u in [SetupMapProbeHelper]::UseMap('OMM_Hybrid').Rows) {
    if ($u['FACTORY'] -ne 'SCK' -or [int]$u['USECODE'] -ne 1) { continue }
    $ux = [int]$u['X']; $uy = [int]$u['Y']
    if ($dir -eq 2 -or $dir -eq 3) { $ux = $sumX - $ux }   # RightBottom / RightTop
    if ($dir -eq 0 -or $dir -eq 3) { $uy = $sumY - $uy }   # LeftTop / RightTop
    [void]$expected.Add("$ux,$uy")
}
$ox = [int]$def['ORIGIN_INDEX_X']; $oy = [int]$def['ORIGIN_INDEX_Y']
switch ($dir) { 0 { $eo = @(($ox + 1), ($sumY - $oy)) } 3 { $eo = @(($sumX - $ox), ($sumY - $oy)) } 2 { $eo = @(($sumX - $ox), $oy) } default { $eo = @($ox, $oy) } }
Write-Host "Map Setup OMM_Hybrid: 방향 $dir / 사용 Die $($expected.Count) / 원점 LeftBottom ($($eo[0]), $($eo[1])) / Pitch $($def['CHIP_SIZE_X']) x $($def['CHIP_SIZE_Y'])"

# ---- 사본 KLARF: LotID / SetupID 시각 / ResultTimestamp 만 바꿈 ----
$text = [IO.File]::ReadAllText($sample)
$text = $text.Replace('LotID "PFIC_RH-HS2615_C4Bump";', 'LotID "PROBE_SETUPMAP";')
$text = [Text.RegularExpressions.Regex]::Replace($text, '(SetupID "[^"]*") [0-9-]+ [0-9:]+;', '$1 01-02-20 03:04:05;')
$text = [Text.RegularExpressions.Regex]::Replace($text, 'ResultTimestamp [0-9-]+ [0-9:]+;', 'ResultTimestamp 01-02-20 03:04:05;')
$probeFile = 'XD106.02'
$bytes = [Text.Encoding]::ASCII.GetBytes($text)
if (-not $text.Contains('PROBE_SETUPMAP')) { Write-Host 'KLARF_SETUPMAP_PROBE=FAIL (사본 만들기 실패)'; exit 1 }

# ---- 1) 미리보기 ----
$pv = [SetupMapProbeHelper]::Begin((Info $probeFile), $bytes)
$r = $pv.Rows[0]
Check '미리보기 READY 또는 EXISTS(이전 Probe)' (@('READY', 'EXISTS') -contains $r['RESULT']) "$($r['RESULT']) $($r['MESSAGE'])"
Check 'MAP_DIES = Map Setup 사용 Die' ([int]$r['MAP_DIES'] -eq $expected.Count) "MAP_DIES=$($r['MAP_DIES']) 기대 $($expected.Count) / 검사 Die $($r['INSPECTED_DIE'])"
Check '원점 번호 = Map Setup 원점(LeftBottom)' (([int]$r['DIE_ORIGIN_X'] -eq $eo[0]) -and ([int]$r['DIE_ORIGIN_Y'] -eq $eo[1])) "($($r['DIE_ORIGIN_X']), $($r['DIE_ORIGIN_Y']))"
Check 'Map Setup 참고 문구 없음 (Pitch / 원점 같음, Map 밖 Die / Defect 없음)' ($r['MAP_NOTE'].ToString() -eq '') "'$($r['MAP_NOTE'])'"
$key = $r['UPLOAD_KEY']
$noMap = [SetupMapProbeHelper]::Begin((Info $probeFile 'NO_MAP_PROBE_DEVICE'), $bytes)
Check 'Map 없는 Device -> INVALID' (($noMap.Rows[0]['RESULT'] -eq 'INVALID') -and $noMap.Rows[0]['MESSAGE'].ToString().Contains('Map')) "$($noMap.Rows[0]['MESSAGE'])"
[SetupMapProbeHelper]::Cancel($noMap.Rows[0]['UPLOAD_KEY'])

# ---- 2) 저장 + 기존 Defect Map Viewer 함수로 재조회 ----
$c = [SetupMapProbeHelper]::Commit($key, 'Y').Rows[0]
if ($c['RESULT'] -ne 'SAVED') { Check '저장 SAVED' $false "$($c['RESULT']) $($c['MESSAGE'])"; Write-Host 'KLARF_SETUPMAP_PROBE=FAIL'; exit 1 }
$step = [long]$c['STEP_SEQ']; $wafer = [long]$c['WAFER_SEQ']; $setup = [long]$c['SETUP_SEQ']
Check '저장 SAVED' $true "STEP_SEQ=$step WAFER_SEQ=$wafer SETUP_SEQ=$setup $($c['ELAPSED_MS'])ms $($c['MESSAGE'])"

function Check-Saved($label) {
    $s = [SetupMapProbeHelper]::Setup($setup).Rows[0]
    $ok = ([double]$s['WAFER_SIZE'] -eq [double]$def['WAFER_SIZE']) -and ([double]$s['DIE_PITCH_X'] -eq [double]$def['CHIP_SIZE_X']) -and ([double]$s['DIE_PITCH_Y'] -eq [double]$def['CHIP_SIZE_Y']) `
        -and ([int]$s['DIE_ORIGIN_X'] -eq $eo[0]) -and ([int]$s['DIE_ORIGIN_Y'] -eq $eo[1]) `
        -and ([double]$s['ORIGIN_X'] -eq [double]$def['ORIGIN_MICRO_X']) -and ([double]$s['ORIGIN_Y'] -eq [double]$def['ORIGIN_MICRO_Y'])
    Check "$label TQD_SETUP = Map Setup 값" $ok ("Wafer {0} / Pitch {1} x {2} / 원점번호 ({3}, {4}) / 원점 {5} / {6}" -f $s['WAFER_SIZE'], $s['DIE_PITCH_X'], $s['DIE_PITCH_Y'], $s['DIE_ORIGIN_X'], $s['DIE_ORIGIN_Y'], $s['ORIGIN_X'], $s['ORIGIN_Y'])
    # Viewer 쿼리는 Setup 의 모든 Test 를 돌려준다. 행 수 / 위치 / TEST(= KLARF Test 번호 하나) 모두 본다.
    $vm = [SetupMapProbeHelper]::ViewerMap($step, "$setup", $script:test)
    $got = Key-Set $vm 'INDEX_X' 'INDEX_Y'
    $tests = @($vm.Rows | ForEach-Object { "$($_['TEST'])" } | Sort-Object -Unique) -join ','
    Check "$label Defect Map Viewer Map(GetDefectMapViewer_Map) = Map Setup 사용 Die" (($vm.Rows.Count -eq $expected.Count) -and $got.SetEquals($expected) -and ($tests -eq $script:test)) "행 $($vm.Rows.Count) / 기대 $($expected.Count) / 같음 $($got.SetEquals($expected)) / TEST $tests"
}
# Defect Map Viewer 는 STEP_INFO 의 TEST 로 Map 을 찾는다. 이 KLARF 의 Test 번호(샘플은 3)를 쓴다.
$err = ''
$test = ''
try { $vi = [SetupMapProbeHelper]::ViewerInfo($step); $test = "$($vi.Tables['STEP_INFO'].Rows[0]['TEST'])" } catch { $err = $_.Exception.Message }
Check 'Defect Map Viewer 정보(GetDefectMapViewer_Info) 오류 없음' ($err -eq '') "$err TEST=$test"
Check-Saved '저장 후'
$df = [SetupMapProbeHelper]::Defects($step)
$xc = @('XINDEX', 'INDEX_X') | Where-Object { $df.Columns.Contains($_) } | Select-Object -First 1
$yc = @('YINDEX', 'INDEX_Y') | Where-Object { $df.Columns.Contains($_) } | Select-Object -First 1
$out = @($df.Rows | Where-Object { -not $expected.Contains("$([int]$_[$xc]),$([int]$_[$yc])") }).Count
Check 'Defect 15 / 모두 Map Setup Die 안' (($df.Rows.Count -eq 15) -and ($out -eq 0)) "Defect $($df.Rows.Count) / Map 밖 $out ($xc / $yc)"

# ---- 3) Defect 번호 독립 확인: 사용자가 예전 규칙으로 올린 같은 Wafer (읽기만) ----
$orig = [SetupMapProbeHelper]::Begin((Info 'XD106.02'), [IO.File]::ReadAllBytes($sample)).Rows[0]
[SetupMapProbeHelper]::Cancel($orig['UPLOAD_KEY'])
if ($orig['RESULT'] -eq 'EXISTS') {
    $udf = [SetupMapProbeHelper]::Defects([long]$orig['EXIST_STEP_SEQ'])
    $a = @($df.Rows | Sort-Object { [int]$_['DEFECTID'] } | ForEach-Object { "$($_['DEFECTID']):$($_[$xc]),$($_[$yc])" }) -join ';'
    $b = @($udf.Rows | Sort-Object { [int]$_['DEFECTID'] } | ForEach-Object { "$($_['DEFECTID']):$($_[$xc]),$($_[$yc])" }) -join ';'
    Check 'Defect 번호 = 예전 규칙(KLARF 원점 7,5) 업로드와 같음' ($a -eq $b) "사용자 STEP_SEQ $($orig['EXIST_STEP_SEQ'])"
} else {
    Check 'Defect 번호 독립 확인 (사용자 업로드 없음 -> 생략)' $true "원본 미리보기 $($orig['RESULT'])"
}

# ---- 4) 재업로드 교체: Setup / Setup Map 을 예전 규칙 값으로 바꿔 둔 뒤 재등록 ----
$old = New-Object 'string[,]' 26, 4
$i = 0
foreach ($p in @($expected | Select-Object -First 26)) { $xy = $p.Split(','); $old[$i, 0] = "$setup"; $old[$i, 1] = $test; $old[$i, 2] = $xy[0]; $old[$i, 3] = $xy[1]; $i++ }
[SetupMapProbeHelper]::MakeOldRule($setup, $test, $old)
$before = [SetupMapProbeHelper]::SetupMap($setup, $test).Rows.Count
$ps = [SetupMapProbeHelper]::Setup($setup).Rows[0]
Check '예전 규칙 데이터 만들기 (Setup Map 26 / Pitch 23000)' (($before -eq 26) -and ([double]$ps['DIE_PITCH_X'] -eq 23000)) "Setup Map $before / Pitch $($ps['DIE_PITCH_X'])"
$pv2 = [SetupMapProbeHelper]::Begin((Info $probeFile), $bytes).Rows[0]
Check '다시 올리기 미리보기 EXISTS' ($pv2['RESULT'] -eq 'EXISTS') "$($pv2['RESULT']) $($pv2['MESSAGE'])"
$c2 = [SetupMapProbeHelper]::Commit($pv2['UPLOAD_KEY'], 'Y').Rows[0]
Check '재등록 SAVED (이전 STEP 삭제)' (($c2['RESULT'] -eq 'SAVED') -and ([long]$c2['DELETED_STEP_SEQ'] -eq $step) -and ([long]$c2['SETUP_SEQ'] -eq $setup)) "$($c2['RESULT']) STEP_SEQ=$($c2['STEP_SEQ']) $($c2['MESSAGE'])"
$step = [long]$c2['STEP_SEQ']; $wafer = [long]$c2['WAFER_SEQ']
Check-Saved '재등록 후'

# ---- 5) 건너뜀 ----
$pv3 = [SetupMapProbeHelper]::Begin((Info $probeFile), $bytes).Rows[0]
$c3 = [SetupMapProbeHelper]::Commit($pv3['UPLOAD_KEY'], 'N').Rows[0]
Check '건너뜀(N) SKIPPED / Setup Map 그대로' (($c3['RESULT'] -eq 'SKIPPED') -and ([SetupMapProbeHelper]::SetupMap($setup, $test).Rows.Count -eq $expected.Count)) "$($c3['RESULT']) $($c3['MESSAGE'])"

# ---- 정리 ----
if ($Keep) {
    Write-Host "남김: STEP_SEQ $step / WAFER_SEQ $wafer / SETUP_SEQ $setup (Lot PROBE_SETUPMAP, ResultTimestamp 2020-01-02 03:04:05)"
} else {
    $n = [SetupMapProbeHelper]::Delete($step, $wafer)
    Check 'Probe STEP 삭제' ($n -eq 1) "STEP_SEQ $step"
}

$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host "KLARF_SETUPMAP_PROBE=PASS ($($results.Count))"; exit 0 }
Write-Host "KLARF_SETUPMAP_PROBE=FAIL ($fail / $($results.Count))"; exit 1
