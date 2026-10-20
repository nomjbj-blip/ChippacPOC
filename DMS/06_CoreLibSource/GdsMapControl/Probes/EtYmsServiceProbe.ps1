# E-Test 업로드 -> YMS 테이블 자동 반영 검증 스크립트 (2026-10-14, 설계: 문서/설계/04_ETest/2026-10-14_ETest_Upload_YMS적재_설계.md 16절)
# 목적: 업로드 서비스(GdsEtResult: Begin -> Die -> 값 -> Complete)만 호출해도 YMS 테이블(TQP_LOT / TQP_WAFER / TD_<PROGRAM> /
#       TQP_WAFER_SUM)에 들어가는지 확인한다. Map 없음 -> 보류(WAIT), Map 생성 + 좌표 다시 계산 -> 반영(DONE),
#       다시 반영 / 다시 판정(Spec 변경) / 파일 삭제 / Map 삭제를 YMS 쪽 결과로 본다.
#       YMS BIN = 결과 파일 Hardware Bin, 양품 = 파일 DUT 결과 (2026-10-14). 샘플은 Bin 1 Pass 20 / Bin 5 Fail 4 -> 수율 83.33,
#       Spec 을 바꿔 다시 판정해도 YMS Bin / 수율은 그대로다 (E-Test Spec 판정과 별개).
#       파일 읽기와 업로드 행 만들기는 화면과 같은 코드(ENGUI EtResultFileParser / EtUploadPlan)를 쓴다(GdsEtResultProbe 와 같은 방식).
#       DB 접속은 BSL -> DSL -> Miracom.Middleware (Service Remoting 경유 아님).
# 보호: Factory PROBE / Device PRB* 만 허용한다. 시험 Map 은 SCK OMM_Hybrid 사용 Die 를 TQP_USEMAP 에 복사했다가 지운다(TQP_MAPDEF 는 만들지 않음 ->
#       YMS Map 은 데이터 범위로 그리고 NETDIE 는 없음). 끝나면 E-Test 파일 / 등록 Parameter / Spec / 시험 Map / YMS 데이터를 모두 지운다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File EtYmsServiceProbe.ps1 [-BinDir <TEMP 빌드: SEMDMS / TEST 서버 DLL>] [-EnguiDir <ENGUI DLL 폴더>] [-Keep]
#       -BinDir 생략 시 101_ServerBin, -EnguiDir 생략 시 100_ClientBin. -Keep 이면 마지막 반영(DONE) 상태로 남긴다.
# 결과: 마지막 줄 ET_YMS_SERVICE_PROBE=PASS 또는 FAIL (통과 수 / 전체 수)
param(
    [string]$BinDir = '',
    [string]$EnguiDir = '',
    [string]$Factory = 'PROBE',
    [string]$Device = 'PRBOMM',
    [string]$SampleFile = '',
    [string]$SpecTable = '',
    [switch]$Keep
)
$ErrorActionPreference = 'Stop'
if ($Factory -ne 'PROBE' -or -not $Device.StartsWith('PRB')) { throw "보호 규칙: Factory 는 PROBE, Device 는 PRB 로 시작해야 합니다 ($Factory / $Device)" }
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$serverBin = Join-Path $dms '101_ServerBin'
$clientBin = Join-Path $dms '100_ClientBin'
if ($BinDir -eq '') { $BinDir = $serverBin }
if ($EnguiDir -eq '') { $EnguiDir = $clientBin }
if ($SampleFile -eq '') { $SampleFile = Join-Path $dms '..\SampleFile\26_10_06 Sample File\CAI_OMM_ATV_A0_TSVET_20260609161940_RH-HS2615_RH-HS2615-02.xlsx' }
if ($SpecTable -eq '') { $SpecTable = Join-Path $dms '02_DMS\DACrux.SEMDMS.ENGUI\Verification\EtSpecTable_U9_20261012.txt' }
$SampleFile = (Resolve-Path $SampleFile).Path

# BSL / DSL 이 ConfigurationManager.AppSettings["DMS_CONNECT_ID"] / ["TEST_CONNECT_ID"] 를 읽으므로 SEMDMS Service 설정 파일을 이 프로세스 설정으로 지정한다.
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
foreach ($d in 'DocumentFormat.OpenXml', 'ClosedXML') { [void][Reflection.Assembly]::LoadFrom((Join-Path $clientBin "$d.dll")) }
[void][Reflection.Assembly]::LoadFrom((Join-Path $EnguiDir 'DACrux.SEMDMS.ENGUI.dll'))
Write-Host "BSL: $(Join-Path $BinDir 'DACrux.SEMDMS.BSL.dll') / ENGUI: $(Join-Path $EnguiDir 'DACrux.SEMDMS.ENGUI.dll')"

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
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

$bsl = New-Object DACrux.SEMDMS.BSL.GdsEtResult
$pub = New-Object DACrux.SEMDMS.BSL.EtYmsPublisher
$program = [DACrux.SEMDMS.Interface.EtYmsRule]::ToProgramName($Device, 'TSVET')
$lotDsl = New-Object DACrux.TEST.DSL.TQP_LOT
$waferDsl = New-Object DACrux.TEST.DSL.TQP_WAFER
$tdDsl = New-Object DACrux.TEST.DSL.TD_TABLE

function YmsLotSeq() { return $lotDsl.GetLotSeq($Factory, 'TSVET', 'RH-HS2615', $program) }
# 행이 1개여도 배열로 (PowerShell 이 풀지 않게)
function YmsWafers() {
    $l = YmsLotSeq
    if (-not $l) { return ,([object[]]@()) }
    return ,([object[]]@($waferDsl.GetData('SELECT_WAFER_BY_LOT', $null, [string[]]@($l, $program)).Rows))
}
function TdRowCount([string]$waferSeq) {
    $ds = $tdDsl.GetTableSchema($Factory, $program, $waferSeq)
    if ($ds.Tables.Count -eq 0) { return 0 }
    return $ds.Tables[0].Rows.Count
}
function FileRow([string]$seq) { return $bsl.GetEtFile($seq).Rows[0] }
function YmsState([string]$seq) {
    return @($(New-Object DACrux.SEMDMS.DSL.TQP_GDS_ET_YMS).GetData('SELECT_ET_YMS_FILE', $null, [string[]]@($seq)).Rows)[0]
}
function Info([object]$plan) {
    return [string[]]@($Factory, $Device, $f.FileName, $f.LotId, '2', 'TSVET', $f.TestTime, '', $f.SheetName,
        [string]$f.DieCount, [string]$plan.Matched.Count, 'probe', $f.Customer, $f.FileDevice, $f.WaferType, $f.WaferRev, $f.WaferId, $f.FabName,
        [string]$f.ItemCount, [string]$plan.RegisteredCount, [string]$plan.Missing.Count)
}
function Upload([object]$plan) {
    $head = $bsl.BeginEtFile((Info $plan)).Rows[0]
    $s = [string]$head['SRC_FILE_SEQ']
    [void]$bsl.CreateEtDieBatch($s, $f.Dies, 'probe')
    foreach ($batch in $plan.BuildValueBatches($f, 10000)) { [void]$bsl.CreateEtValueBatch($s, $batch, 'probe') }
    return @{ Seq = $s; Complete = $bsl.CompleteEtFile($s, 'probe').Rows[0] }
}
function MapOn() {
    Sql "DELETE FROM DMSMGR.TQP_USEMAP WHERE FACTORY = '$Factory' AND MAPID = '$Device';`nINSERT INTO DMSMGR.TQP_USEMAP (FACTORY, MAPID, PROGRAM_REV, X, Y, USECODE, DIE_NUM) SELECT '$Factory', '$Device', PROGRAM_REV, X, Y, USECODE, DIE_NUM FROM DMSMGR.TQP_USEMAP WHERE FACTORY = 'SCK' AND MAPID = 'OMM_Hybrid';"
}
function MapOff() { Sql "DELETE FROM DMSMGR.TQP_USEMAP WHERE FACTORY = '$Factory' AND MAPID = '$Device';" }
# 시험 데이터 전부 정리: E-Test 파일(YMS 반영도 같이) -> 남은 YMS Wafer / Lot -> 등록 Parameter(Spec FK CASCADE) -> 시험 Map -> YMS Bin / Program / Product
function Cleanup() {
    foreach ($s in '2', '3') { foreach ($r in @($bsl.GetEtFileByKey([string[]]@($Factory, 'RH-HS2615', $s)).Rows)) { [void]$bsl.DeleteEtFile([string]$r['SRC_FILE_SEQ'], 'probe') } }
    foreach ($r in (YmsWafers)) { [void]$pub.DeleteWafer([string]$r['WAFER_SEQ']) }
    $l = YmsLotSeq
    if ($l) { (New-Object DACrux.TEST.DSL.TQP_LOT_SUM).DeleteLot($l); $lotDsl.DeleteLot($l) }
    $names = [string[]]@($bsl.GetEtParameterList($Factory, $Device, 'TSVET').Rows | ForEach-Object { [string]$_['ITEM_NAME'] })
    if ($names.Count -gt 0) { [void]$bsl.DeleteEtParameters([string[]]@($Factory, $Device, 'TSVET'), $names, 'probe') }
    MapOff
    $tm = New-Object DACrux.TEST.BSL.TestDataManager
    foreach ($t in @((New-Object DACrux.TEST.DSL.TQP_DATA_TABLES).GetTableNames($Factory, $program))) { if ($t) { $tm.CheckTableEmpty($t) } }
    [void](New-Object DACrux.TEST.DSL.TQP_BINDESC).DeleteDataNonQuery('DELETE_PROGRAM_BIN_ALL', $null, [string[]]@($program))
    [void](New-Object DACrux.TEST.DSL.TQP_PROGRAM).DeleteDataNonQuery('DELETE_PROGRAM', $null, [string[]]@($program))
    (New-Object DACrux.TEST.DSL.TQP_PRODUCT).UpdateDeleteFlag('Y', $Device)
}

# 1. 준비: 파일 읽기 / 정리 / TSVET 37개 등록 + Spec
$f = [DACrux.SEMDMS.ENGUI.EtResultFileParser]::Parse($SampleFile)
Cleanup
$spec = @([DACrux.SEMDMS.ENGUI.EtSpecTable]::Parse([IO.File]::ReadAllText($SpecTable)) | Where-Object { $_.TestProgram -eq 'TSVET' })
[void]$bsl.RegisterEtParameters([string[]]@($Factory, $Device, 'TSVET', 'SPEC', ''), (Rows @($spec | ForEach-Object { , @($_.HeaderText, $_.Unit, '', '') }) 4), 'probe')
[void]$bsl.SaveEtLimit([string[]]@($Factory, $Device, 'TSVET'), (Rows @($spec | ForEach-Object { , @($_.HeaderText, $_.Lower, $_.Upper, 'Y', $_.Unit, $_.SpecText, '') }) 7), 'probe')
$registered = $bsl.GetEtParameterList($Factory, $Device, 'TSVET')
$plan = [DACrux.SEMDMS.ENGUI.EtUploadPlan]::Build($f, $registered)
Check '준비 (헤더 2,755 / 등록 37 / 저장 계획 37 / YMS 데이터 없음)' ($f.ItemCount -eq 2755 -and $registered.Rows.Count -eq 37 -and $plan.Matched.Count -eq 37 -and (YmsWafers).Count -eq 0) "Program $program"

# 2. Map 없이 업로드 -> 확정 결과에 YMS WAIT, YMS 데이터 없음
$u1 = Upload $plan
$seq = $u1.Seq; $c1 = $u1.Complete; $st1 = YmsState $seq
Check '업로드 (Map 없음) -> 확정 READY + YMS 보류' ($c1['RESULT'] -eq 'OK' -and $c1['LOAD_STATUS'] -eq 'READY' -and [string]$c1['YMS_STATUS'] -eq 'WAIT' -and [string]$st1['YMS_STATUS'] -eq 'WAIT' -and $st1['YMS_WAFER_SEQ'] -eq [DBNull]::Value -and (YmsWafers).Count -eq 0) "확정 $($c1['RESULT']) / YMS $($c1['YMS_STATUS']) / $($c1['YMS_MESSAGE'])"

# 3. Map 생성 + 좌표 다시 계산 -> YMS 반영
MapOn
$sw = [Diagnostics.Stopwatch]::StartNew()
$rm = $bsl.RemapEtFiles([string[]]@($Factory, $Device), 'probe').Rows[0]
$msRemap = $sw.ElapsedMilliseconds
$st2 = YmsState $seq
$w2 = [string]$st2['YMS_WAFER_SEQ']
$sum2 = @($pub.GetWaferSummary($w2).Rows)
$lot2 = @($waferDsl.GetData('GET_WAFER_INFO', $null, [string[]]@($w2)).Rows)
Check '좌표 다시 계산 -> YMS DONE (TD 24 / 파일 Bin 1:20 5:4 / 수율 83.33 / TESTAREA TSVET / Map ID)' ([int]$rm['FILE_COUNT'] -eq 1 -and [long]$rm['MAP_DIE'] -eq 24 -and [int]$rm['YMS_FAIL'] -eq 0 -and [string]$st2['YMS_STATUS'] -eq 'DONE' -and [string]$st2['YMS_PROGRAM'] -eq $program -and $w2 -ne '' -and (YmsWafers).Count -eq 1 -and (TdRowCount $w2) -eq 24 -and $sum2.Count -eq 1 -and [int]$sum2[0]['BIN1'] -eq 20 -and [int]$sum2[0]['BIN5'] -eq 4 -and [double]$sum2[0]['YIELD'] -eq 83.33 -and $lot2[0]['TEST_AREA'] -eq 'TSVET' -and $lot2[0]['DEVICE_ALIAS'] -eq $Device) "YMS $($st2['YMS_STATUS']) / Wafer $w2 / TD $(TdRowCount $w2) / BIN1 $($sum2[0]['BIN1']) BIN5 $($sum2[0]['BIN5']) / 수율 $($sum2[0]['YIELD']) / ${msRemap}ms"

# 4. YMS 다시 반영 (원격 메서드) -> 새 Wafer 1장, 이전 Wafer 없음
$rp = $bsl.PublishEtFileToYms($seq, 'probe').Rows[0]
$st3 = YmsState $seq
$w3 = [string]$st3['YMS_WAFER_SEQ']
Check 'PublishEtFileToYms 다시 반영' ([string]$rp['YMS_STATUS'] -eq 'DONE' -and [string]$rp['SRC_FILE_SEQ'] -eq $seq -and $w3 -ne $w2 -and [string]$rp['YMS_WAFER_SEQ'] -eq $w3 -and $pub.GetWafer($w2).Rows.Count -eq 0 -and (YmsWafers).Count -eq 1 -and [int]$rp['DIE_COUNT'] -eq 24 -and [int]$rp['PARAM_COUNT'] -eq 37 -and [string]$rp['BIN_COUNTS'] -eq '1:20, 5:4' -and [int]$rp['GOOD_DIE_COUNT'] -eq 20) "Wafer $w2 -> $w3 / Die $($rp['DIE_COUNT']) / Param $($rp['PARAM_COUNT']) / Bin $($rp['BIN_COUNTS']) / 수율 $($rp['YIELD'])"

# 5. Spec 을 바꾸고 다시 판정 -> E-Test 판정은 바뀌지만 YMS BIN / 수율은 파일 Bin 그대로 (Fail 6개 Parameter 상한 715 -> 800)
$failNames = @($bsl.GetEtValueList($seq).Rows | Where-Object { [string]$_['JUDGE_FLAG'] -eq 'F' } | ForEach-Object { [string]$_['ITEM_NAME'] } | Sort-Object -Unique)
[void]$bsl.SaveEtLimit([string[]]@($Factory, $Device, 'TSVET'), (Rows @($failNames | ForEach-Object { , @($_, '', '800', 'Y', '', 'relaxed', '') }) 7), 'probe')
$n1 = $bsl.RejudgeEtFiles([string[]]@($Factory, $Device, 'TSVET'), 'probe')
$failAfter = @($bsl.GetEtValueList($seq).Rows | Where-Object { [string]$_['JUDGE_FLAG'] -eq 'F' }).Count
$w4 = [string](YmsState $seq)['YMS_WAFER_SEQ']
$sum4 = @($pub.GetWaferSummary($w4).Rows)
[void]$bsl.SaveEtLimit([string[]]@($Factory, $Device, 'TSVET'), (Rows @($failNames | ForEach-Object { , @($_, '', '715', 'Y', '', '', '') }) 7), 'probe')
[void]$bsl.RejudgeEtFiles([string[]]@($Factory, $Device, 'TSVET'), 'probe')
$w5 = [string](YmsState $seq)['YMS_WAFER_SEQ']
$sum5 = @($pub.GetWaferSummary($w5).Rows)
Check '다시 판정 -> E-Test Fail 값 0, YMS 는 파일 Bin 그대로 (83.33)' ($failNames.Count -ge 1 -and $n1 -eq 1 -and $failAfter -eq 0 -and $w4 -ne $w3 -and [int]$sum4[0]['BIN1'] -eq 20 -and [int]$sum4[0]['BIN5'] -eq 4 -and [double]$sum4[0]['YIELD'] -eq 83.33 -and [double]$sum5[0]['YIELD'] -eq 83.33 -and (YmsWafers).Count -eq 1) "Fail Parameter $($failNames.Count) -> 완화 후 Fail 값 $failAfter / YMS 수율 $($sum4[0]['YIELD']) -> 되돌림 $($sum5[0]['YIELD'])"

# 6. 파일 삭제 -> YMS Wafer / Lot 도 삭제 (Lot 의 마지막 Wafer)
[void]$bsl.DeleteEtFile($seq, 'probe')
Check '파일 삭제 -> YMS Wafer / Lot 삭제' ($bsl.GetEtFile($seq).Rows.Count -eq 0 -and $pub.GetWafer($w5).Rows.Count -eq 0 -and -not (YmsLotSeq) -and (YmsWafers).Count -eq 0) "E-Test 파일 0 / YMS Wafer 0 / Lot $(if (YmsLotSeq) { '남음' } else { '없음' })"

# 7. Map 이 있는 상태에서 다시 업로드 -> 확정 때 바로 YMS DONE
$u2 = Upload $plan
$seq2 = $u2.Seq; $c2 = $u2.Complete; $st6 = YmsState $seq2
Check '업로드 (Map 있음) -> 확정 때 바로 YMS DONE' ($c2['RESULT'] -eq 'OK' -and [string]$c2['MAP_ID'] -eq $Device -and [string]$c2['YMS_STATUS'] -eq 'DONE' -and [string]$c2['YMS_PROGRAM'] -eq $program -and [double]$c2['YMS_YIELD'] -eq 83.33 -and [string]$st6['YMS_STATUS'] -eq 'DONE' -and (YmsWafers).Count -eq 1) "YMS $($c2['YMS_STATUS']) / Wafer $($c2['YMS_WAFER_SEQ']) / 수율 $($c2['YMS_YIELD'])"

if ($Keep) {
    Write-Host "Keep: SRC_FILE_SEQ=$seq2 / YMS Program $program / Wafer $($st6['YMS_WAFER_SEQ']) (Factory $Factory, 시험 Map TQP_USEMAP 남김)"
} else {
    # 8. Map 을 지우고 좌표 다시 계산 -> YMS 보류 + 이전 반영 삭제
    MapOff
    $rm2 = $bsl.RemapEtFiles([string[]]@($Factory, $Device), 'probe').Rows[0]
    $st7 = YmsState $seq2
    Check 'Map 삭제 후 다시 계산 -> YMS WAIT + YMS 데이터 삭제' ([string]$rm2['MAP_ID'] -eq '' -and [string]$st7['YMS_STATUS'] -eq 'WAIT' -and $st7['YMS_WAFER_SEQ'] -eq [DBNull]::Value -and (YmsWafers).Count -eq 0 -and -not (YmsLotSeq)) "YMS $($st7['YMS_STATUS']) / $($st7['YMS_MESSAGE'])"
    Cleanup
    $left = @($bsl.GetEtParameterList($Factory, $Device, 'TSVET').Rows).Count + @($bsl.GetEtFileByKey([string[]]@($Factory, 'RH-HS2615', '2')).Rows).Count + (YmsWafers).Count
    Check '끝 정리 (E-Test 파일 / 등록 Parameter / YMS Wafer)' ($left -eq 0) "남은 행 $left"
}

$pass = @($results | Where-Object { $_.StartsWith('PASS') }).Count
$total = $results.Count
Write-Host ''
Write-Host ("ET_YMS_SERVICE_PROBE={0} ({1}/{2})" -f $(if ($pass -eq $total) { 'PASS' } else { 'FAIL' }), $pass, $total)
if ($pass -ne $total) { exit 1 }
