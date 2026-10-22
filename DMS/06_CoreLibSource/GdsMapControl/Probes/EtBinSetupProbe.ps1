# E-Test Bin 설정 서비스 검증 스크립트 (2026-10-22, 설계: 문서/설계/04_ETest/2026-10-22_ETest_Bin_Setup_화면_설계.md 11절)
# 목적: GdsEtResult Bin 설정 메서드(GetEtBinProgramList / GetEtBinList / SaveEtBins / DeleteEtBins)와 수율 다시 계산(RejudgeEtFiles)을
#       실제 업로드 파일로 확인한다. 샘플은 Bin 1 Pass 20 / Bin 5 Fail 4 -> YMS 수율 83.33, Bin 5 를 양품으로 바꾸면 100.
#       저장은 이름 / 양품 / 색 / 설명만 바꾸고 Map 기호 / 표시 / Limit 은 그대로인지, 긴 설명이 잘리지 않는지,
#       틀린 행이 있으면 아무것도 저장하지 않는지, 정의를 지운 Bin 이 다음 YMS 반영 때 다시 생기는지 본다.
#       기존 TQP_BINDESC.xml(frmSetupBin / frmSetupProgram 이 쓰는 MERGE_BIN / SELECT_BIN_EDITABLE)이 긴 설명 / 26자 Program 을 자르지 않는지도 본다
#       (2026-10-22 바인딩 크기 40 / 25 -> 0. 고치기 전 실측: 60자 설명 -> 40자).
#       파일 읽기와 업로드 행 만들기는 화면과 같은 코드(ENGUI EtResultFileParser / EtUploadPlan)를 쓴다(EtYmsServiceProbe 와 같은 방식).
#       DB 접속은 BSL -> DSL -> Miracom.Middleware (Service Remoting 경유 아님).
# 보호: Factory PROBE / Device PRB* 만 허용한다. 서버 설정 파일의 DMS / TEST / QMS_CONNECT_ID 가 다른 DB 여도 사본에서 -ConnectId(기본 DMSMGR = 로컬)로 바꾼다.
#       시험 Map 은 SCK OMM_Hybrid 사용 Die 를 TQP_USEMAP 에 복사했다가 지운다. 끝나면 E-Test 파일 / 등록 Parameter / Spec / 시험 Map / YMS 데이터 / Bin 정의를 모두 지운다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File EtBinSetupProbe.ps1 [-BinDir <TEMP 빌드: SEMDMS / TEST 서버 DLL>] [-EnguiDir <ENGUI DLL 폴더>] [-ConnectId DMSMGR] [-Keep]
#       -BinDir 생략 시 101_ServerBin, -EnguiDir 생략 시 100_ClientBin. -Keep 이면 시험 데이터를 남긴다.
# 결과: 마지막 줄 ET_BIN_SETUP_PROBE=PASS 또는 FAIL (통과 수 / 전체 수)
param(
    [string]$BinDir = '',
    [string]$EnguiDir = '',
    [string]$Factory = 'PROBE',
    [string]$Device = 'PRBBIN',
    [string]$ConnectId = 'DMSMGR',
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
$Slot = '6'

# BSL / DSL 이 ConfigurationManager.AppSettings 의 접속 이름을 읽는다. 서버 설정 사본의 접속 이름만 -ConnectId 로 바꿔 이 프로세스 설정으로 쓴다.
[xml]$x = Get-Content (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config') -Encoding UTF8
$remoting = $x.configuration.'system.runtime.remoting'
if ($remoting) { [void]$x.configuration.RemoveChild($remoting) }
foreach ($k in @($x.configuration.appSettings.add | Where-Object { $_.key -in 'DMS_CONNECT_ID', 'TEST_CONNECT_ID', 'QMS_CONNECT_ID' })) {
    if ($k.value -ne $ConnectId) { Write-Host "DB 연결 이름: $($k.key) $($k.value) -> $ConnectId (사본만, 서버 설정은 그대로)" }
    $k.value = $ConnectId
}
$config = Join-Path $env:TEMP ('chippac_EtBinSetupProbe_' + $ConnectId + '.config')
$x.Save($config)
[AppDomain]::CurrentDomain.SetData('APP_CONFIG_FILE', $config)
Add-Type -AssemblyName System.Configuration
$flags = [Reflection.BindingFlags]'NonPublic,Static'
$cm = [Configuration.ConfigurationManager]
$cm.GetField('s_initState', $flags).SetValue($null, 0)
$cm.GetField('s_configSystem', $flags).SetValue($null, $null)
$probeDirs = @($BinDir, $serverBin, $clientBin)
[AppDomain]::CurrentDomain.add_AssemblyResolve({
    param($s, $e)
    $n = (New-Object Reflection.AssemblyName($e.Name)).Name
    foreach ($d in $probeDirs) { $p = Join-Path $d "$n.dll"; if (Test-Path $p) { return [Reflection.Assembly]::LoadFrom($p) } }
    return $null
})
foreach ($d in 'Oracle.ManagedDataAccess', 'Miracom.Middleware', 'DACrux.Base') {
    $p = Join-Path $serverBin "$d.dll"; if (Test-Path $p) { [void][Reflection.Assembly]::LoadFrom($p) }
}
foreach ($d in 'DACrux.SEMDMS.Interface', 'DACrux.SEMDMS.DSL', 'DACrux.TEST.Interface', 'DACrux.TEST.DSL', 'DACrux.TEST.BSL', 'DACrux.SEMDMS.BSL') {
    [void][Reflection.Assembly]::LoadFrom((Join-Path $BinDir "$d.dll"))
}
foreach ($d in 'DocumentFormat.OpenXml', 'ClosedXML') { [void][Reflection.Assembly]::LoadFrom((Join-Path $clientBin "$d.dll")) }
[void][Reflection.Assembly]::LoadFrom((Join-Path $EnguiDir 'DACrux.SEMDMS.ENGUI.dll'))
Write-Host "BSL: $(Join-Path $BinDir 'DACrux.SEMDMS.BSL.dll') / ENGUI: $(Join-Path $EnguiDir 'DACrux.SEMDMS.ENGUI.dll') / DB 연결 $ConnectId"

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
function Rows([object[]]$rows, [int]$cols) {
    $a = New-Object 'string[,]' $rows.Count, $cols
    for ($i = 0; $i -lt $rows.Count; $i++) { for ($c = 0; $c -lt $cols; $c++) { $a[$i, $c] = [string]$rows[$i][$c] } }
    return ,$a
}
# 로컬 docker DB 에 SQL 실행 (시험 Map / 숨긴 열 값 넣기, 확인 조회).
# PowerShell 이 보내는 글자 앞에 BOM 이 붙어 첫 줄은 sqlplus 가 모르는 명령(SP2-0042 / SP2-0734)이 된다. 첫 줄을 버리는 REM 으로 두고 그 경고 줄은 결과에서 뺀다.
function Sql([string]$text) {
    $script = "REM`nWHENEVER SQLERROR EXIT 1`nSET FEEDBACK OFF`nSET HEADING OFF`nSET PAGESIZE 0`nSET LINESIZE 400`n" + $text + "`nCOMMIT;`nEXIT`n"
    $out = $script | docker exec -i -e NLS_LANG=AMERICAN_AMERICA.AL32UTF8 oracle19c-DMS bash -lc 'sqlplus -s / as sysdba'
    if ($LASTEXITCODE -ne 0) { throw "SQL 실패: $out" }
    return (@($out) | ForEach-Object { "$_".Trim() } | Where-Object { $_ -ne '' -and $_ -notmatch '^SP2-' })
}

$bsl = New-Object DACrux.SEMDMS.BSL.GdsEtResult
$pub = New-Object DACrux.SEMDMS.BSL.EtYmsPublisher
$program = [DACrux.SEMDMS.Interface.EtYmsRule]::ToProgramName($Device, 'TSVET')
$longProgram = $Device + 'LONG_ABCDEFGHIJKLMNO'.Substring(0, 26 - $Device.Length)   # 기존 SELECT_BIN_EDITABLE 바인딩(25)보다 긴 26자 Program
$key = [string[]]@($Factory, $Device, 'TSVET')
$lotDsl = New-Object DACrux.TEST.DSL.TQP_LOT
$waferDsl = New-Object DACrux.TEST.DSL.TQP_WAFER

function YmsLotSeq() { return $lotDsl.GetLotSeq($Factory, 'TSVET', 'RH-HS2615', $program) }
function YmsWafers() {
    $l = YmsLotSeq
    if (-not $l) { return ,([object[]]@()) }
    return ,([object[]]@($waferDsl.GetData('SELECT_WAFER_BY_LOT', $null, [string[]]@($l, $program)).Rows))
}
function YmsState([string]$seq) {
    return @($(New-Object DACrux.SEMDMS.DSL.TQP_GDS_ET_YMS).GetData('SELECT_ET_YMS_FILE', $null, [string[]]@($seq)).Rows)[0]
}
function Yield([string]$seq) {
    $w = [string](YmsState $seq)['YMS_WAFER_SEQ']
    if ($w -eq '') { return -1 }
    return [double]@($pub.GetWaferSummary($w).Rows)[0]['YIELD']
}
function BinRows() { return ,([object[]]@($bsl.GetEtBinList($key).Rows)) }
function BinRow([string]$bin) { return @((BinRows) | Where-Object { [string]$_['BIN'] -eq $bin })[0] }
# 정의 원본(TQP_BINDESC 모든 열)을 한 줄 글자로: 저장 거부 뒤 바뀌지 않았는지 / 숨긴 열 유지 비교용
function DefText() {
    return ((Sql "SELECT BIN || '|' || BIN_NAME || '|' || CHAR_BIN || '|' || HIGH_GEC || '|' || DISPLAY || '|' || UPPER_LIMIT_CNT || '|' || LOWER_LIMIT_CNT || '|' || COLOR || '|' || DESCRIPTION FROM DMSMGR.TQP_BINDESC WHERE PROGRAM = '$program' ORDER BY BIN;") -join ' ; ')
}
function Info([object]$plan) {
    return [string[]]@($Factory, $Device, $f.FileName, $f.LotId, $Slot, 'TSVET', $f.TestTime, '', $f.SheetName,
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
    [void](Sql "DELETE FROM DMSMGR.TQP_USEMAP WHERE FACTORY = '$Factory' AND MAPID = '$Device';`nINSERT INTO DMSMGR.TQP_USEMAP (FACTORY, MAPID, PROGRAM_REV, X, Y, USECODE, DIE_NUM) SELECT '$Factory', '$Device', PROGRAM_REV, X, Y, USECODE, DIE_NUM FROM DMSMGR.TQP_USEMAP WHERE FACTORY = 'SCK' AND MAPID = 'OMM_Hybrid';")
}
function MapOff() { [void](Sql "DELETE FROM DMSMGR.TQP_USEMAP WHERE FACTORY = '$Factory' AND MAPID = '$Device';") }
# 시험 데이터 전부 정리: E-Test 파일(YMS 반영도 같이) -> 남은 YMS Wafer / Lot -> 등록 Parameter(Spec FK CASCADE) -> 시험 Map -> YMS Bin / Program / Product
function Cleanup() {
    foreach ($r in @($bsl.GetEtFileByKey([string[]]@($Factory, 'RH-HS2615', $Slot)).Rows)) { [void]$bsl.DeleteEtFile([string]$r['SRC_FILE_SEQ'], 'probe') }
    foreach ($r in (YmsWafers)) { [void]$pub.DeleteWafer([string]$r['WAFER_SEQ']) }
    $l = YmsLotSeq
    if ($l) { (New-Object DACrux.TEST.DSL.TQP_LOT_SUM).DeleteLot($l); $lotDsl.DeleteLot($l) }
    $names = [string[]]@($bsl.GetEtParameterList($Factory, $Device, 'TSVET').Rows | ForEach-Object { [string]$_['ITEM_NAME'] })
    if ($names.Count -gt 0) { [void]$bsl.DeleteEtParameters($key, $names, 'probe') }
    MapOff
    $tm = New-Object DACrux.TEST.BSL.TestDataManager
    foreach ($t in @((New-Object DACrux.TEST.DSL.TQP_DATA_TABLES).GetTableNames($Factory, $program))) { if ($t) { $tm.CheckTableEmpty($t) } }
    [void](New-Object DACrux.TEST.DSL.TQP_BINDESC).DeleteDataNonQuery('DELETE_PROGRAM_BIN_ALL', $null, [string[]]@($program))
    [void](New-Object DACrux.TEST.DSL.TQP_BINDESC).DeleteDataNonQuery('DELETE_PROGRAM_BIN_ALL', $null, [string[]]@($longProgram))
    [void](New-Object DACrux.TEST.DSL.TQP_PROGRAM).DeleteDataNonQuery('DELETE_PROGRAM', $null, [string[]]@($program))
    (New-Object DACrux.TEST.DSL.TQP_PRODUCT).UpdateDeleteFlag('Y', $Device)
}
function Fails([scriptblock]$call) {
    try { & $call | Out-Null; return '' } catch { return $_.Exception.GetBaseException().Message }
}

# 다른 Program 의 Bin 정의는 건드리지 않는다 (시험 전 / 후 비교)
$otherBefore = (Sql "SELECT COUNT(*) || '|' || NVL(SUM(ORA_HASH(PROGRAM || BIN || BIN_NAME || HIGH_GEC || COLOR || DESCRIPTION)), 0) FROM DMSMGR.TQP_BINDESC WHERE PROGRAM NOT LIKE '$Device%';") -join ''

# 1. 준비: 파일 읽기 / 정리 / TSVET 37개 등록 + Spec / Map / 업로드 -> YMS 반영 83.33
$f = [DACrux.SEMDMS.ENGUI.EtResultFileParser]::Parse($SampleFile)
Cleanup
$spec = @([DACrux.SEMDMS.ENGUI.EtSpecTable]::Parse([IO.File]::ReadAllText($SpecTable)) | Where-Object { $_.TestProgram -eq 'TSVET' })
[void]$bsl.RegisterEtParameters([string[]]@($Factory, $Device, 'TSVET', 'SPEC', ''), (Rows @($spec | ForEach-Object { , @($_.HeaderText, $_.Unit, '', '') }) 4), 'probe')
[void]$bsl.SaveEtLimit($key, (Rows @($spec | ForEach-Object { , @($_.HeaderText, $_.Lower, $_.Upper, 'Y', $_.Unit, $_.SpecText, '') }) 7), 'probe')
$registered = $bsl.GetEtParameterList($Factory, $Device, 'TSVET')
$plan = [DACrux.SEMDMS.ENGUI.EtUploadPlan]::Build($f, $registered)
MapOn
$u = Upload $plan
$seq = $u.Seq
Check '준비: 업로드 -> YMS DONE / 수율 83.33' ($u.Complete['RESULT'] -eq 'OK' -and [string]$u.Complete['YMS_STATUS'] -eq 'DONE' -and (Yield $seq) -eq 83.33) "파일 $seq / YMS $($u.Complete['YMS_STATUS']) / 수율 $(Yield $seq) / Program $program"

# 2. Device + 공정 목록
$pl = @($bsl.GetEtBinProgramList($Factory).Rows | Where-Object { [string]$_['DEVICE_ID'] -eq $Device })
Check 'GetEtBinProgramList: 파일 1 / YMS 반영 1 / YMS Program' ($pl.Count -eq 1 -and [string]$pl[0]['TEST_PROGRAM'] -eq 'TSVET' -and [int]$pl[0]['FILE_COUNT'] -eq 1 -and [int]$pl[0]['YMS_FILE_COUNT'] -eq 1 -and [string]$pl[0]['YMS_PROGRAM'] -eq $program) "행 $($pl.Count) / 파일 $($pl[0]['FILE_COUNT']) / YMS $($pl[0]['YMS_FILE_COUNT']) / $($pl[0]['YMS_PROGRAM'])"

# 3. 정의 + 파일 분포 합치기: 업로드가 만든 정의(설명 51자 그대로) + Software Bin / Die / Pass / Fail / 파일
$rows = BinRows
$b1 = BinRow '1'; $b5 = BinRow '5'
$desc1 = [DACrux.SEMDMS.Interface.EtYmsRule]::BinDescription(1, [int[]]@(1), $true, $false)
Check 'GetEtBinList: Bin 1 양품 Die 20 Pass 20 / Bin 5 불량 SW 5001 Die 4 Fail 4' ($rows.Count -eq 2 -and [string]$b1['HAS_DEF'] -eq 'Y' -and [string]$b1['HIGH_GEC'] -eq 'Y' -and [string]$b1['SW_BINS'] -eq '1' -and [long]$b1['DIE_COUNT'] -eq 20 -and [long]$b1['PASS_COUNT'] -eq 20 -and [long]$b1['FAIL_COUNT'] -eq 0 -and [long]$b1['FILE_COUNT'] -eq 1 -and [string]$b1['IN_RANGE'] -eq 'Y' -and [string]$b5['HIGH_GEC'] -eq 'N' -and [string]$b5['SW_BINS'] -eq '5001' -and [long]$b5['DIE_COUNT'] -eq 4 -and [long]$b5['FAIL_COUNT'] -eq 4 -and [string]$b1['YMS_PROGRAM'] -eq $program) "행 $($rows.Count) / Bin 1 $($b1['BIN_NAME']) $($b1['HIGH_GEC']) SW $($b1['SW_BINS']) Die $($b1['DIE_COUNT']) / Bin 5 $($b5['BIN_NAME']) $($b5['HIGH_GEC']) SW $($b5['SW_BINS']) Fail $($b5['FAIL_COUNT'])"
Check 'GetEtBinList: 업로드가 만든 설명 길이 그대로 (40자 넘음)' ([string]$b1['DESCRIPTION'] -eq $desc1 -and $desc1.Length -gt 40 -and [string]$b1['COLOR'] -eq '#00C000') "$($desc1.Length)자 / $($b1['DESCRIPTION'])"

# 4. 새 Bin 정의 (파일에 없는 Bin 7) -> 정의만 있는 행
$s7 = $bsl.SaveEtBins($key, (Rows @(, @('7', 'MANUAL 7', 'N', '#0000ff', 'probe bin 7')) 5), 'probe').Rows[0]
$b7 = BinRow '7'
Check 'SaveEtBins 새 Bin 7: NEW 1 / 양품 변경 N / 색 대문자 / Die 0' ([int]$s7['SAVED_COUNT'] -eq 1 -and [int]$s7['NEW_COUNT'] -eq 1 -and [string]$s7['GOOD_CHANGED'] -eq 'N' -and [int]$s7['YMS_FILE_COUNT'] -eq 1 -and [string]$b7['HAS_DEF'] -eq 'Y' -and [string]$b7['COLOR'] -eq '#0000FF' -and [long]$b7['DIE_COUNT'] -eq 0 -and [long]$b7['FILE_COUNT'] -eq 0) "저장 $($s7['SAVED_COUNT']) / 새 $($s7['NEW_COUNT']) / 양품 변경 $($s7['GOOD_CHANGED']) / 색 $($b7['COLOR']) / Die $($b7['DIE_COUNT'])"
$sql7 = Sql "SELECT DISPLAY || '|' || NVL(CHAR_BIN, '-') || '|' || NVL(TO_CHAR(UPPER_LIMIT_CNT), '-') FROM DMSMGR.TQP_BINDESC WHERE PROGRAM = '$program' AND BIN = 7;"
Check '새 정의 기본값: 표시 Y / Map 기호 · Limit 없음 (업로드 자동 등록과 같음)' (($sql7 -join '') -eq 'Y|-|-') ($sql7 -join '')

# 5. 저장 거부: 틀린 행이 하나라도 있으면 아무것도 저장하지 않는다
$before = DefText
$long31 = 'N' * 31
$korean = '가나다라마바사아자차카'   # 11자 = 33바이트
$e1 = Fails { $bsl.SaveEtBins($key, (Rows @(@('1', 'OK NAME', 'Y', '', ''), @('5', $long31, 'N', '', '')) 5), 'probe') }
$e2 = Fails { $bsl.SaveEtBins($key, (Rows @(, @('5', 'X', 'N', 'red', '')) 5), 'probe') }
$e3 = Fails { $bsl.SaveEtBins($key, (Rows @(, @('100', 'X', 'N', '', '')) 5), 'probe') }
$e4 = Fails { $bsl.SaveEtBins($key, (Rows @(@('5', 'A', 'N', '', ''), @('5', 'B', 'N', '', '')) 5), 'probe') }
$e5 = Fails { $bsl.SaveEtBins($key, (Rows @(, @('5', $korean, 'N', '', '')) 5), 'probe') }
$e6 = Fails { $bsl.SaveEtBins($key, (Rows @(, @('5', 'X', 'T', '', '')) 5), 'probe') }
$after = DefText
Check '저장 거부: 이름 31자 / 색 red / Bin 100 / 같은 Bin / 한글 33바이트 / 양품 T -> 정의 그대로' ($e1 -match '이름' -and $e2 -match '#RRGGBB' -and $e3 -match '0 ~ 99' -and $e4 -match '두 번' -and $e5 -match '30바이트' -and $e6 -match 'Y 또는 N' -and $before -eq $after) "1 $($e1.Length -gt 0) / 2 $($e2.Length -gt 0) / 3 $($e3.Length -gt 0) / 4 $($e4.Length -gt 0) / 5 $($e5.Length -gt 0) / 6 $($e6.Length -gt 0) / 바뀜 $($before -ne $after)"
$e7 = Fails { $bsl.SaveEtBins([string[]]@($Factory, $Device), (Rows @(, @('5', 'X', 'N', '', '')) 5), 'probe') }
Check '키 거부: 키 2개' ($e7 -match '3개 항목') $e7

# 6. 숨긴 열 유지 + 긴 설명(100바이트) 그대로: Bin 1 에 Map 기호 / 표시 N / Limit 을 넣어 두고 이름 / 설명만 바꾼다
[void](Sql "UPDATE DMSMGR.TQP_BINDESC SET CHAR_BIN = 'A', DISPLAY = 'N', UPPER_LIMIT_CNT = 9, LOWER_LIMIT_CNT = 1 WHERE PROGRAM = '$program' AND BIN = 1;")
$desc100 = 'D' * 100
$s1 = $bsl.SaveEtBins($key, (Rows @(, @('1', 'GOOD ONE', 'Y', '#00C000', $desc100)) 5), 'probe').Rows[0]
$sql1 = (Sql "SELECT BIN_NAME || '|' || CHAR_BIN || '|' || DISPLAY || '|' || UPPER_LIMIT_CNT || '|' || LOWER_LIMIT_CNT || '|' || LENGTH(DESCRIPTION) FROM DMSMGR.TQP_BINDESC WHERE PROGRAM = '$program' AND BIN = 1;") -join ''
Check 'SaveEtBins Bin 1: 이름 / 설명 100자 저장, Map 기호 / 표시 / Limit 그대로, 양품 변경 N' ([int]$s1['NEW_COUNT'] -eq 0 -and [string]$s1['GOOD_CHANGED'] -eq 'N' -and $sql1 -eq 'GOOD ONE|A|N|9|1|100') $sql1

# 7. 기존 TQP_BINDESC.xml (frmSetupBin / frmSetupProgram 이 쓰는 쿼리): MERGE_BIN 60자 설명 / SELECT_BIN_EDITABLE 26자 Program 이 잘리지 않는지
#    (2026-10-22 바인딩 크기 수정. Bin 7 은 다음 단계에서 지우고, 26자 Program 정의는 여기서 지운다)
$desc60 = 'E' * 60
$tb = New-Object DACrux.TEST.DSL.TQP_BINDESC
[void]$tb.MergeBinInfo([string[]]@($program, '7', 'MANUAL 7', '', 'N', 'Y', '', '', '#0000FF', $desc60))
$len7 = (Sql "SELECT LENGTH(DESCRIPTION) FROM DMSMGR.TQP_BINDESC WHERE PROGRAM = '$program' AND BIN = 7;") -join ''
[void]$tb.MergeBinInfo([string[]]@($longProgram, '1', 'LONG PROGRAM', '', 'Y', 'Y', '', '', '#00C000', $desc60))
$editable = @($tb.GetBinListEditable($longProgram).Rows)
$longSaved = (Sql "SELECT LENGTH(PROGRAM) || '|' || LENGTH(DESCRIPTION) FROM DMSMGR.TQP_BINDESC WHERE PROGRAM = '$longProgram';") -join ''
[void]$tb.DeleteBinInfo($longProgram, '1', '')
$eTooLong = Fails { $tb.MergeBinInfo([string[]]@($program, '7', 'MANUAL 7', '', 'N', 'Y', '', '', '#0000FF', ('F' * 101))) }   # 컬럼(100)보다 길면 잘리지 않고 DB 오류
$len7b = (Sql "SELECT LENGTH(DESCRIPTION) FROM DMSMGR.TQP_BINDESC WHERE PROGRAM = '$program' AND BIN = 7;") -join ''
Check '기존 MERGE_BIN 60자 설명 그대로 / 101자는 DB 오류(ORA-12899) + 값 그대로 / SELECT_BIN_EDITABLE 26자 Program 조회 (frmSetupBin 경로)' ($len7 -eq '60' -and $eTooLong -match 'ORA-12899' -and $len7b -eq '60' -and $longSaved -eq '26|60' -and $editable.Count -eq 1 -and ([string]$editable[0]['DESCRIPTION']).Length -eq 60 -and [string]$editable[0]['HIGH_GEC'] -eq 'True') "Bin 7 설명 $len7 / 101자 $(if ($eTooLong -match 'ORA-12899') { 'ORA-12899' } else { $eTooLong }) -> $len7b / 26자 Program 저장 $longSaved / 조회 $($editable.Count)행"

# 8. 양품 바꿈 -> GOOD_CHANGED Y -> 다시 판정(YMS 다시 반영) -> 수율 100 -> 되돌려 83.33
$s5 = $bsl.SaveEtBins($key, (Rows @(, @('5', [string]$b5['BIN_NAME'], 'Y', [string]$b5['COLOR'], [string]$b5['DESCRIPTION'])) 5), 'probe').Rows[0]
$n1 = $bsl.RejudgeEtFiles($key, 'probe')
$y1 = Yield $seq
$s5b = $bsl.SaveEtBins($key, (Rows @(, @('5', [string]$b5['BIN_NAME'], 'N', [string]$b5['COLOR'], [string]$b5['DESCRIPTION'])) 5), 'probe').Rows[0]
[void]$bsl.RejudgeEtFiles($key, 'probe')
$y2 = Yield $seq
Check '양품 Bin 5 -> GOOD_CHANGED Y / YMS 파일 1 / 다시 계산 수율 100 -> 되돌림 83.33' ([string]$s5['GOOD_CHANGED'] -eq 'Y' -and [int]$s5['YMS_FILE_COUNT'] -eq 1 -and $n1 -eq 1 -and $y1 -eq 100 -and [string]$s5b['GOOD_CHANGED'] -eq 'Y' -and $y2 -eq 83.33) "변경 $($s5['GOOD_CHANGED']) / 다시 판정 $n1 / 수율 $y1 -> $y2"

# 9. 삭제: 정의 없는 Bin 이 섞이면 거부, Bin 7 삭제 -> 목록에서 빠짐
$e8 = Fails { $bsl.DeleteEtBins($key, [string[]]@('7', '8'), 'probe') }
$still7 = BinRow '7'
$d7 = $bsl.DeleteEtBins($key, [string[]]@('7'), 'probe')
Check 'DeleteEtBins: 정의 없는 Bin 8 섞이면 거부 / Bin 7 삭제' ($e8 -match '정의가 없는' -and $null -ne $still7 -and $d7 -eq 1 -and $null -eq (BinRow '7')) "거부 $($e8.Length -gt 0) / 삭제 $d7 / 남은 행 $((BinRows).Count)"

# 10. 파일에 있는 Bin 5 정의를 지우면 '정의 없음' 행 -> 다음 YMS 반영(다시 판정) 때 업로드 기본값으로 다시 생김
[void]$bsl.DeleteEtBins($key, [string[]]@('5'), 'probe')
$nodef = BinRow '5'
[void]$bsl.RejudgeEtFiles($key, 'probe')
$redef = BinRow '5'
Check 'Bin 5 정의 삭제 -> 정의 없음(Die 4) -> 다시 반영 때 기본값으로 다시 생김' ([string]$nodef['HAS_DEF'] -eq 'N' -and [long]$nodef['DIE_COUNT'] -eq 4 -and [string]$redef['HAS_DEF'] -eq 'Y' -and [string]$redef['BIN_NAME'] -eq [string]$b5['BIN_NAME'] -and [string]$redef['DESCRIPTION'] -eq [string]$b5['DESCRIPTION']) "삭제 뒤 $($nodef['HAS_DEF']) Die $($nodef['DIE_COUNT']) / 다시 반영 뒤 $($redef['HAS_DEF']) $($redef['BIN_NAME'])"

if ($Keep) {
    Write-Host "Keep: SRC_FILE_SEQ=$seq / YMS Program $program (Factory $Factory, 시험 Map TQP_USEMAP 남김)"
} else {
    Cleanup
    $left = @($bsl.GetEtParameterList($Factory, $Device, 'TSVET').Rows).Count + @($bsl.GetEtFileByKey([string[]]@($Factory, 'RH-HS2615', $Slot)).Rows).Count + (YmsWafers).Count
    $leftBin = (Sql "SELECT COUNT(*) FROM DMSMGR.TQP_BINDESC WHERE PROGRAM = '$program';") -join ''
    Check '끝 정리 (E-Test 파일 / 등록 Parameter / YMS Wafer / Bin 정의)' ($left -eq 0 -and $leftBin -eq '0') "남은 행 $left / Bin 정의 $leftBin"
}
$otherAfter = (Sql "SELECT COUNT(*) || '|' || NVL(SUM(ORA_HASH(PROGRAM || BIN || BIN_NAME || HIGH_GEC || COLOR || DESCRIPTION)), 0) FROM DMSMGR.TQP_BINDESC WHERE PROGRAM NOT LIKE '$Device%';") -join ''
Check '다른 Program Bin 정의는 그대로' ($otherBefore -eq $otherAfter) "$otherBefore -> $otherAfter"

$pass = @($results | Where-Object { $_.StartsWith('PASS') }).Count
$total = $results.Count
Write-Host ''
Write-Host ("ET_BIN_SETUP_PROBE={0} ({1}/{2})" -f $(if ($pass -eq $total) { 'PASS' } else { 'FAIL' }), $pass, $total)
if ($pass -ne $total) { exit 1 }
