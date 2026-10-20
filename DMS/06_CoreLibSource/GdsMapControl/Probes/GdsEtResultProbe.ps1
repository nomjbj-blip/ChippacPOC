# E-Test 결과 파일 DB 저장 서비스 검증 스크립트 (2026-10-07 작성, 2026-10-14 Upload 재설계로 다시 작성)
# 설계: 문서/설계/04_ETest/2026-10-13_ETest_Upload_재설계.md 9절 / 10절 (저장 대상 = 등록 Parameter, Spec 판정 저장, Map 좌표)
# 목적: 샘플 E-Test xlsx 를 화면과 같은 코드(ENGUI EtResultFileParser / EtUploadPlan)로 읽고, BSL GdsEtResult 를 같은 프로세스에서 직접 호출해
#       등록 Parameter 확인 / 저장 / 판정 / 거부 / 다시 판정 / Map 좌표 / 삭제를 확인한다. DB 접속은 BSL -> DSL -> Miracom.Middleware (Service Remoting 경유 아님).
# 준비: FACTORY=PROBE / DEVICE_ID=ET_PROBE 에 고객 Spec 표 TSVET 37개를 Parameter 로 등록 + Spec 저장.
#       Map 시험은 로컬 docker oracle19c-DMS 에 시험 Map(PROBE / MAPID = ET_PROBE, SCK OMM_Hybrid 사용 Die 복사)을 넣었다가 지운다.
# 결과: -Keep 이 없으면 파일 / 등록 Parameter / Spec / 시험 Map 을 모두 지운다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsEtResultProbe.ps1 [-BinDir <BSL/DSL/Interface/ENGUI DLL 폴더>] [-Keep]
#       -BinDir 생략 시 101_ServerBin (ENGUI 는 100_ClientBin). 배포 전에는 TEMP 빌드 폴더를 넘긴다.
param(
    [string]$BinDir = '',
    [string]$Factory = 'PROBE',
    [string]$Device = 'ET_PROBE',
    [string]$SampleFile = '',
    [string]$SpecTable = '',
    [switch]$Keep
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$serverBin = Join-Path $dms '101_ServerBin'
$clientBin = Join-Path $dms '100_ClientBin'
if ($BinDir -eq '') { $BinDir = $serverBin }
if ($SampleFile -eq '') { $SampleFile = Join-Path $dms '..\SampleFile\26_10_06 Sample File\CAI_OMM_ATV_A0_TSVET_20260609161940_RH-HS2615_RH-HS2615-02.xlsx' }
if ($SpecTable -eq '') { $SpecTable = Join-Path $dms '02_DMS\DACrux.SEMDMS.ENGUI\Verification\EtSpecTable_U9_20261012.txt' }
$SampleFile = (Resolve-Path $SampleFile).Path

# BSL / DSL 이 ConfigurationManager.AppSettings["DMS_CONNECT_ID"] 를 읽으므로 Service 설정 파일을 이 프로세스 설정으로 지정한다.
[AppDomain]::CurrentDomain.SetData('APP_CONFIG_FILE', (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config'))
Add-Type -AssemblyName System.Configuration
$flags = [Reflection.BindingFlags]'NonPublic,Static'
$cm = [Configuration.ConfigurationManager]
$cm.GetField('s_initState', $flags).SetValue($null, 0)
$cm.GetField('s_configSystem', $flags).SetValue($null, $null)
foreach ($d in 'Oracle.ManagedDataAccess', 'Miracom.Middleware', 'DACrux.Base') {
    $p = Join-Path $serverBin "$d.dll"; if (Test-Path $p) { [void][Reflection.Assembly]::LoadFrom($p) }
}
foreach ($d in 'DACrux.SEMDMS.Interface', 'DACrux.SEMDMS.DSL', 'DACrux.SEMDMS.BSL') {
    [void][Reflection.Assembly]::LoadFrom((Join-Path $BinDir "$d.dll"))
}
foreach ($d in 'DocumentFormat.OpenXml', 'ClosedXML') { [void][Reflection.Assembly]::LoadFrom((Join-Path $clientBin "$d.dll")) }
$engui = Join-Path $BinDir 'DACrux.SEMDMS.ENGUI.dll'
if (-not (Test-Path $engui)) { $engui = Join-Path $clientBin 'DACrux.SEMDMS.ENGUI.dll' }
[void][Reflection.Assembly]::LoadFrom($engui)
Write-Host "BSL: $(Join-Path $BinDir 'DACrux.SEMDMS.BSL.dll') / ENGUI: $engui"
Write-Host "Sample: $SampleFile"

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
function Throws([scriptblock]$block) {
    try { & $block; return '' } catch { $m = $_.Exception; while ($m.InnerException) { $m = $m.InnerException }; return $m.Message }
}
function Rows([object[]]$rows, [int]$cols) {
    $a = New-Object 'string[,]' $rows.Count, $cols
    for ($i = 0; $i -lt $rows.Count; $i++) { for ($c = 0; $c -lt $cols; $c++) { $a[$i, $c] = [string]$rows[$i][$c] } }
    return ,$a
}
# 로컬 docker DB 에 SQL 실행 (시험 Map 넣기 / 지우기)
function Sql([string]$text) {
    $script = "WHENEVER SQLERROR EXIT 1`nSET FEEDBACK OFF`n" + $text + "`nCOMMIT;`nEXIT`n"
    $out = $script | docker exec -i -e NLS_LANG=AMERICAN_AMERICA.AL32UTF8 oracle19c-DMS bash -lc 'sqlplus -s / as sysdba'
    if ($LASTEXITCODE -ne 0) { throw "SQL 실패: $out" }
}
$inv = [Globalization.CultureInfo]::InvariantCulture
$bsl = New-Object DACrux.SEMDMS.BSL.GdsEtResult
$rule = [DACrux.SEMDMS.Interface.EtParameterNameRule]

function Info([string]$program, [object]$plan, [string]$slot, [int]$defined) {
    return [string[]]@($Factory, $Device, $f.FileName, $f.LotId, $slot, $program, $f.TestTime, '', $f.SheetName,
        [string]$f.DieCount, [string]$plan.Matched.Count, 'probe', $f.Customer, $f.FileDevice, $f.WaferType, $f.WaferRev, $f.WaferId, $f.FabName,
        [string]$f.ItemCount, [string]$defined, [string]$plan.Missing.Count)
}
function Cleanup {
    foreach ($s in '2', '3') { foreach ($r in @($bsl.GetEtFileByKey([string[]]@($Factory, $f.LotId, $s)).Rows)) { [void]$bsl.DeleteEtFile([string]$r['SRC_FILE_SEQ'], 'probe') } }
    foreach ($r in @($bsl.GetEtParaProgramList($Factory).Rows | Where-Object { [string]$_['DEVICE_ID'] -eq $Device })) {
        $t = [string]$r['TEST_PROGRAM']
        $names = [string[]]@($bsl.GetEtParameterList($Factory, $Device, $t).Rows | ForEach-Object { [string]$_['ITEM_NAME'] })
        if ($names.Count -gt 0) { [void]$bsl.DeleteEtParameters([string[]]@($Factory, $Device, $t), $names, 'probe') }
    }
    Sql "DELETE FROM DMSMGR.TQP_USEMAP WHERE FACTORY = '$Factory' AND MAPID = '$Device';"
}

# ---------------------------------------------------------------------------------------------------
# 1. 파일 읽기 (화면과 같은 파서)
# ---------------------------------------------------------------------------------------------------
$sw = [Diagnostics.Stopwatch]::StartNew()
$f = [DACrux.SEMDMS.ENGUI.EtResultFileParser]::Parse($SampleFile)
Check '파일 읽기 (헤더 2,755 / Die 24 / 파일명 규칙)' ($f.ItemCount -eq 2755 -and $f.DieCount -eq 24 -and $f.LotId -eq 'RH-HS2615' -and $f.SlotNo -eq 2 -and $f.TestProgram -eq 'TSVET' -and $f.FileDevice -eq 'OMM') "헤더 $($f.ItemCount) / Die $($f.DieCount) / Lot $($f.LotId) / Slot $($f.SlotNo) / $($sw.ElapsedMilliseconds)ms"
Cleanup

# ---------------------------------------------------------------------------------------------------
# 2. 등록 Parameter 없으면 거부 / 이전 방식 거부
# ---------------------------------------------------------------------------------------------------
$plan0 = [DACrux.SEMDMS.ENGUI.EtUploadPlan]::Build($f, $bsl.GetEtParameterList($Factory, $Device, 'TSVET'))
$m1 = Throws { $bsl.BeginEtFile((Info 'TSVET' $plan0 '2' 0)) }
$old = [string[]]@($Factory, $Device, $f.FileName, $f.LotId, '2', 'TSVET', $f.TestTime, '', $f.SheetName, '24', '2755', 'probe', $f.Customer, $f.FileDevice, $f.WaferType, $f.WaferRev, $f.WaferId, $f.FabName)
$m2 = Throws { $bsl.BeginEtFile($old) }
Check '등록 Parameter 없음 -> 저장 거부 / 18항목(이전 방식) 거부' ($plan0.RegisteredCount -eq 0 -and $m1 -match '등록 Parameter\(사용 Y\)가 없습니다' -and $m2 -match '21개') "$m1 | $m2"

# ---------------------------------------------------------------------------------------------------
# 3. 고객 Spec 표 TSVET 37개 등록 + Spec
# ---------------------------------------------------------------------------------------------------
$spec = @([DACrux.SEMDMS.ENGUI.EtSpecTable]::Parse([IO.File]::ReadAllText($SpecTable)) | Where-Object { $_.TestProgram -eq 'TSVET' })
$reg = $bsl.RegisterEtParameters([string[]]@($Factory, $Device, 'TSVET', 'SPEC', ''), (Rows @($spec | ForEach-Object { , @($_.HeaderText, $_.Unit, '', '') }) 4), 'probe').Rows[0]
[void]$bsl.SaveEtLimit([string[]]@($Factory, $Device, 'TSVET'), (Rows @($spec | ForEach-Object { , @($_.HeaderText, $_.Lower, $_.Upper, 'Y', $_.Unit, $_.SpecText, '') }) 7), 'probe')
$registered = $bsl.GetEtParameterList($Factory, $Device, 'TSVET')
Check 'TSVET 37개 등록 + Spec 37' ([int]$reg['REGISTERED'] -eq 37 -and $registered.Rows.Count -eq 37 -and @($registered.Rows | Where-Object { [string]$_['HAS_SPEC'] -eq 'Y' }).Count -eq 37) "등록 $($reg['REGISTERED']) / 목록 $($registered.Rows.Count)"

# ---------------------------------------------------------------------------------------------------
# 4. 업로드 계획 (화면 미리보기와 같은 코드)
# ---------------------------------------------------------------------------------------------------
$plan = [DACrux.SEMDMS.ENGUI.EtUploadPlan]::Build($f, $registered)
$previewFailDie = @(0..23 | Where-Object { $plan.DieFail[$_] -gt 0 })
Check '업로드 계획: 저장 37 / 누락 0 / 건너뜀 2,718 / 미리보기 Fail Die 1 (#44, Fail 6)' ($plan.Matched.Count -eq 37 -and $plan.Missing.Count -eq 0 -and $plan.Skipped -eq 2718 -and $plan.RegisteredCount -eq 37 -and $plan.JudgedCount -eq 37 -and $plan.SpecFailDie -eq 1 -and $previewFailDie.Count -eq 1 -and $f.Dies[$previewFailDie[0], 0] -eq '44' -and $plan.DieFail[$previewFailDie[0]] -eq 6) "저장 $($plan.Matched.Count) / 건너뜀 $($plan.Skipped) / Fail Die $($plan.SpecFailDie) (DUT #$($f.Dies[$previewFailDie[0], 0]) Fail $($plan.DieFail[$previewFailDie[0]]))"

# ---------------------------------------------------------------------------------------------------
# 5. 저장: Begin(21) -> Die -> 값(등록 Parameter 만) -> Complete(판정 + Map 좌표)
# ---------------------------------------------------------------------------------------------------
$sw.Restart()
$head = $bsl.BeginEtFile((Info 'TSVET' $plan '2' $plan.RegisteredCount)).Rows[0]
$seq = [string]$head['SRC_FILE_SEQ']
$nd = $bsl.CreateEtDieBatch($seq, $f.Dies, 'probe')
$nv = 0
foreach ($batch in $plan.BuildValueBatches($f, 10000)) { $nv += $bsl.CreateEtValueBatch($seq, $batch, 'probe') }
$c = $bsl.CompleteEtFile($seq, 'probe').Rows[0]
Check '저장 확정 READY: 값 888 (37 x 24) / Spec Fail Die 1 / Map 없음' ($c['RESULT'] -eq 'OK' -and $c['LOAD_STATUS'] -eq 'READY' -and $nd -eq 24 -and $nv -eq 888 -and [long]$c['DB_VALUE'] -eq 888 -and [long]$c['SPEC_FAIL_DIE'] -eq 1 -and [long]$c['FAIL_DIE'] -eq 1 -and [string]$c['MAP_ID'] -eq '' -and [long]$c['MAP_DIE'] -eq 0) "$($c['MESSAGE']) / 값 $($c['DB_VALUE']) / Fail Die $($c['SPEC_FAIL_DIE']) / Map '$($c['MAP_ID'])' / $($sw.ElapsedMilliseconds)ms"
$fr = $bsl.GetEtFile($seq).Rows[0]
Check '파일 요약: 헤더 2,755 / 등록 37 / 저장 37 / 누락 0 / Pass 23 · Fail 1 / 규칙 3 / Fab · 면 없음 / 판정 시각' ([int]$fr['HEADER_COUNT'] -eq 2755 -and [int]$fr['DEFINED_COUNT'] -eq 37 -and [int]$fr['ITEM_COUNT'] -eq 37 -and [int]$fr['MISSING_COUNT'] -eq 0 -and [int]$fr['PASS_DIE_COUNT'] -eq 23 -and [int]$fr['FAIL_DIE_COUNT'] -eq 1 -and [int]$fr['PARSE_RULE_VER'] -eq 3 -and $fr['FAB_NAME'] -eq [DBNull]::Value -and $fr['VIEW_SIDE'] -eq [DBNull]::Value -and $fr['JUDGE_TIME'] -ne [DBNull]::Value) "WAFER_ID $($fr['WAFER_ID']) / Pass $($fr['PASS_DIE_COUNT']) Fail $($fr['FAIL_DIE_COUNT']) / 값 $($fr['VALUE_COUNT'])"
$dies = $bsl.GetEtDieList($seq)
$d44 = @($dies.Rows | Where-Object { [int]$_['DUT_NO'] -eq 44 })[0]
$d20 = @($dies.Rows | Where-Object { [int]$_['DUT_NO'] -eq 20 })[0]
Check 'Die 판정: #44 F (Fail 6) / 나머지 P / DUT Flag 는 원본 그대로(#20 F)' ([string]$d44['JUDGE_FLAG'] -eq 'F' -and [int]$d44['FAIL_COUNT'] -eq 6 -and @($dies.Rows | Where-Object { [string]$_['JUDGE_FLAG'] -eq 'P' }).Count -eq 23 -and [string]$d20['PASS_FLAG'] -eq 'F' -and [string]$d20['JUDGE_FLAG'] -eq 'P') "#44 $($d44['JUDGE_FLAG']) $($d44['FAIL_COUNT']) / #20 DUT Flag $($d20['PASS_FLAG']) 판정 $($d20['JUDGE_FLAG'])"
$values = $bsl.GetEtValueList($seq)
$col = @{}; for ($m = 0; $m -lt $plan.Matched.Count; $m++) { $col[$plan.Matched[$m].ItemSeq] = $m }
$dieIndex = @{}; for ($i = 0; $i -lt $f.DieCount; $i++) { $dieIndex[[string]$f.Dies[$i, 0]] = $i }
$diff = 0
foreach ($r in $values.Rows) {
    $expect = $plan.Judge[$dieIndex[[string]$r['DUT_NO']], $col[[string]$r['ET_ITEM_SEQ']]]
    $actual = [string]$r['JUDGE_FLAG']
    if (($expect -eq [char]0 -and $actual -ne '') -or ($expect -ne [char]0 -and $actual -ne [string]$expect)) { $diff++ }
}
Check '값 판정: 서버 SQL = 화면 미리보기 (888개 모두 같음) / Fail 값 6 / 등록 정보 열' ($values.Rows.Count -eq 888 -and $diff -eq 0 -and @($values.Rows | Where-Object { [string]$_['JUDGE_FLAG'] -eq 'F' }).Count -eq 6 -and $values.Columns.Contains('HEADER_TEXT') -and $values.Columns.Contains('CHAIN_NAME')) "다른 판정 $diff / F $(@($values.Rows | Where-Object { [string]$_['JUDGE_FLAG'] -eq 'F' }).Count)"

# ---------------------------------------------------------------------------------------------------
# 6. 거부
# ---------------------------------------------------------------------------------------------------
$m = Throws { $bsl.BeginEtFile((Info 'TSVET' $plan '2' 37)) }
Check '같은 Wafer + 공정 재저장 거부' ($m -match '이미 있습니다') $m
$m = Throws { $bsl.BeginEtFile((Info 'TSVET' $plan '3' 36)) }
Check '분석 뒤 등록 Parameter 가 바뀜(화면 36 / 지금 37) -> 거부' ($m -match '바뀌었습니다') $m
$m = Throws { $bsl.BeginEtFile((Info 'COWET' $plan '2' 37)) }
Check '같은 Wafer 다른 공정(COWET): 등록 Parameter 없음 -> 거부' ($m -match '등록 Parameter\(사용 Y\)가 없습니다') $m
$h3 = $bsl.BeginEtFile((Info 'TSVET' $plan '3' 37)).Rows[0]
[void]$bsl.CreateEtDieBatch([string]$h3['SRC_FILE_SEQ'], $f.Dies, 'probe')
$bad = New-Object 'string[,]' 1, 4; $bad[0, 0] = '1'; $bad[0, 1] = '999999999'; $bad[0, 2] = '1'; $bad[0, 3] = 'V'
$m = Throws { $bsl.CreateEtValueBatch([string]$h3['SRC_FILE_SEQ'], $bad, 'probe') }
Check '등록 Parameter 가 아닌 항목 번호 값 거부 (다른 Slot -03 은 저장 시작 가능)' ([string]$h3['WAFER_ID'] -eq 'RH-HS2615-03' -and $m -match '등록 Parameter\(사용 Y\)가 아닌') $m
[void]$bsl.DeleteEtFile([string]$h3['SRC_FILE_SEQ'], 'probe')
$m = Throws { $bsl.CreateEtValueBatch($seq, $bad, 'probe') }
Check 'READY 후 배치 거부' ($m -match 'READY') $m

# ---------------------------------------------------------------------------------------------------
# 7. Spec 변경 -> 다시 판정
# ---------------------------------------------------------------------------------------------------
$failNames = @($values.Rows | Where-Object { [string]$_['JUDGE_FLAG'] -eq 'F' } | ForEach-Object { [string]$_['ITEM_NAME'] })
[void]$bsl.SaveEtLimit([string[]]@($Factory, $Device, 'TSVET'), (Rows @($failNames | ForEach-Object { , @($_, '', '800', 'Y', '', '완화', '') }) 7), 'probe')
$n1 = $bsl.RejudgeEtFiles([string[]]@($Factory, $Device, 'tsvet'), 'probe')
$fr1 = $bsl.GetEtFile($seq).Rows[0]
$d44b = @($bsl.GetEtDieList($seq).Rows | Where-Object { [int]$_['DUT_NO'] -eq 44 })[0]
[void]$bsl.SaveEtLimit([string[]]@($Factory, $Device, 'TSVET'), (Rows @(, @($failNames[0], '', '715', 'N', '', 'Enable N', '')) 7), 'probe')
[void]$bsl.SaveEtLimit([string[]]@($Factory, $Device, 'TSVET'), (Rows @($failNames | Select-Object -Skip 1 | ForEach-Object { , @($_, '', '715', 'Y', '', '', '') }) 7), 'probe')
$n2 = $bsl.RejudgeEtFiles([string[]]@($Factory, $Device, 'TSVET'), 'probe')
$fr2 = $bsl.GetEtFile($seq).Rows[0]
$v0 = @($bsl.GetEtValueList($seq).Rows | Where-Object { [string]$_['ITEM_NAME'] -eq $failNames[0] -and [int]$_['DUT_NO'] -eq 44 })[0]
$d44c = @($bsl.GetEtDieList($seq).Rows | Where-Object { [int]$_['DUT_NO'] -eq 44 })[0]
Check 'Spec 완화 -> 다시 판정: Fail Die 0 / 되돌리고 1개 Enable N -> #44 Fail 5, 그 값 판정 없음(NULL)' ($n1 -eq 1 -and [int]$fr1['FAIL_DIE_COUNT'] -eq 0 -and [string]$d44b['JUDGE_FLAG'] -eq 'P' -and $n2 -eq 1 -and [int]$fr2['FAIL_DIE_COUNT'] -eq 1 -and [int]$d44c['FAIL_COUNT'] -eq 5 -and [string]$v0['JUDGE_FLAG'] -eq '') "완화 후 Fail Die $($fr1['FAIL_DIE_COUNT']) (#44 $($d44b['JUDGE_FLAG'])) / 되돌린 뒤 $($fr2['FAIL_DIE_COUNT']) (#44 Fail $($d44c['FAIL_COUNT']), Enable N 값 '$($v0['JUDGE_FLAG'])')"

# ---------------------------------------------------------------------------------------------------
# 8. Map 좌표: 시험 Map(MAPID = Device) 만들고 다시 계산 -> 지우고 다시 계산
# ---------------------------------------------------------------------------------------------------
Sql "INSERT INTO DMSMGR.TQP_USEMAP (FACTORY, MAPID, PROGRAM_REV, X, Y, USECODE, DIE_NUM) SELECT '$Factory', '$Device', PROGRAM_REV, X, Y, USECODE, DIE_NUM FROM DMSMGR.TQP_USEMAP WHERE FACTORY = 'SCK' AND MAPID = 'OMM_Hybrid';"
$mapDies = $bsl.GetEtMapDieList($Factory, $Device)
$rm = $bsl.RemapEtFiles([string[]]@($Factory, $Device), 'probe').Rows[0]
$d20m = @($bsl.GetEtDieList($seq).Rows | Where-Object { [int]$_['DUT_NO'] -eq 20 })[0]
$list = @($bsl.GetEtFileList($Factory, $Device).Rows | Where-Object { [string]$_['SRC_FILE_SEQ'] -eq $seq })[0]
Check 'Map 다시 계산: 사용 Die 76 / 파일 1 / 좌표 Die 24 / 밖 0 / DUT #20 E-Test (7,3) -> 시스템 (9,2) / 목록 MAP_ID · 좌표 수' ($mapDies.Rows.Count -eq 108 -and @($mapDies.Rows | Where-Object { [int]$_['USECODE'] -eq 1 }).Count -eq 76 -and [int]$rm['FILE_COUNT'] -eq 1 -and [string]$rm['MAP_ID'] -eq $Device -and [long]$rm['MAP_DIE'] -eq 24 -and [long]$rm['OUT_OF_MAP'] -eq 0 -and [int]$d20m['MAP_X'] -eq 9 -and [int]$d20m['MAP_Y'] -eq 2 -and [string]$list['MAP_ID'] -eq $Device -and [int]$list['MAP_DIE_COUNT'] -eq 24) "Map Die $($mapDies.Rows.Count) / 좌표 $($rm['MAP_DIE']) / #20 ($($d20m['MAP_X']),$($d20m['MAP_Y']))"
Sql "DELETE FROM DMSMGR.TQP_USEMAP WHERE FACTORY = '$Factory' AND MAPID = '$Device';"
$rm2 = $bsl.RemapEtFiles([string[]]@($Factory, $Device), 'probe').Rows[0]
$d20n = @($bsl.GetEtDieList($seq).Rows | Where-Object { [int]$_['DUT_NO'] -eq 20 })[0]
Check 'Map 을 지우고 다시 계산 -> 좌표 비움 / MAP_ID 없음' ([string]$rm2['MAP_ID'] -eq '' -and [long]$rm2['MAP_DIE'] -eq 0 -and $d20n['MAP_X'] -eq [DBNull]::Value -and $bsl.GetEtFile($seq).Rows[0]['MAP_ID'] -eq [DBNull]::Value) "MAP_ID '$($rm2['MAP_ID'])' / #20 '$($d20n['MAP_X'])'"

# ---------------------------------------------------------------------------------------------------
# 9. 삭제 (등록 Parameter 는 남음) / 정리
# ---------------------------------------------------------------------------------------------------
if (-not $Keep) {
    $nDel = $bsl.DeleteEtFile($seq, 'probe')
    $leftPara = $bsl.GetEtParameterList($Factory, $Device, 'TSVET').Rows.Count
    Check '파일 삭제: 값 888 / Die 0 / 등록 Parameter 37 남음' ($nDel -eq 888 -and $bsl.GetEtDieList($seq).Rows.Count -eq 0 -and $leftPara -eq 37) "값 $nDel / 등록 Parameter $leftPara"
    Cleanup
    $left = @($bsl.GetEtParameterList($Factory, $Device, 'TSVET').Rows).Count + @($bsl.GetEtFileByKey([string[]]@($Factory, $f.LotId, '2')).Rows).Count
    Check '시험 데이터 정리 (파일 / 등록 Parameter / Spec / 시험 Map)' ($left -eq 0) "남은 행 $left"
} else { Write-Host "Keep: SRC_FILE_SEQ=$seq / $Factory $Device TSVET 등록 Parameter 남김" }

$fail = @($results | Where-Object { $_ -like 'FAIL*' }).Count
Write-Host ("ET_RESULT_PROBE=" + $(if ($fail -eq 0) { 'PASS' } else { 'FAIL' }) + " ($($results.Count - $fail)/$($results.Count))")
if ($fail -gt 0) { exit 1 }
