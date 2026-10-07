# E-Test 결과 파일 DB 저장 서비스 검증 스크립트 (CLAUDE-029, 설계: 문서/2026-10-07_ETest_결과파일_Parsing_DB설계.md 9절 / 10절)
# 목적: 샘플 E-Test xlsx 를 읽어 BSL GdsEtResult 를 같은 프로세스에서 직접 호출하고 저장 / 확정 / 재조회 / 거부 / 삭제를 확인한다.
#       DB 접속은 BSL -> DSL -> Miracom.Middleware 경로다 (Service Remoting 경유 아님).
#       xlsx 읽기는 ClientBin 의 ClosedXML 을 쓴다. 헤더 분해 규칙은 설계 10.2절 정규식과 같다 (화면 Parsing 클래스는 2단계에서 만든다).
# 결과: FACTORY=PROBE / DEVICE_ID=ET_PROBE 에 파일 1건을 만들고, -Keep 이 없으면 마지막에 파일을 삭제한다 (측정 항목은 Device 단위라 남는다).
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsEtResultProbe.ps1 [-BinDir <BSL/DSL/Interface DLL 폴더>] [-Keep]
#       -BinDir 생략 시 101_ServerBin. 배포 전에는 TEMP 빌드 폴더를 넘긴다.
param(
    [string]$BinDir = '',
    [string]$Factory = 'PROBE',
    [string]$Device = 'ET_PROBE',
    [string]$SampleFile = '',
    [switch]$Keep
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$serverBin = Join-Path $dms '101_ServerBin'
$clientBin = Join-Path $dms '100_ClientBin'
if ($BinDir -eq '') { $BinDir = $serverBin }
if ($SampleFile -eq '') { $SampleFile = Join-Path $dms '..\SampleFile\26_10_06 Sample File\CAI_OMM_ATV_A0_TSVET_20260609161940_RH-HS2615_RH-HS2615-02.xlsx' }
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
Write-Host "BSL: $(Join-Path $BinDir 'DACrux.SEMDMS.BSL.dll')"
Write-Host "Sample: $SampleFile"

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
function Throws([scriptblock]$block) {
    try { & $block; return '' } catch { $m = $_.Exception; while ($m.InnerException) { $m = $m.InnerException }; return $m.Message }
}
$inv = [Globalization.CultureInfo]::InvariantCulture
function CellText($cell) {
    if ($cell.IsEmpty()) { return '' }
    $v = $cell.Value
    if ($v -is [double]) { return $v.ToString('R', $inv) }
    return ([string]$v).Trim()
}

# 1. 파일명 분해 (뒤에서부터: Wafer / Lot / 14자리 일시 / 시험 프로그램 / 나머지 접두어)
$sw = [Diagnostics.Stopwatch]::StartNew()
$base = [IO.Path]::GetFileNameWithoutExtension($SampleFile)
$tok = $base.Split('_')
$wafer = $tok[$tok.Length - 1]; $lot = $tok[$tok.Length - 2]; $testTime = $tok[$tok.Length - 3]; $program = $tok[$tok.Length - 4]
$prefix = [string]::Join('_', $tok[0..($tok.Length - 5)])
$slot = ''
if ($wafer.StartsWith($lot + '-')) { $slot = [string][int]$wafer.Substring($lot.Length + 1) }
Check '파일명 분해' ($lot -eq 'RH-HS2615' -and $slot -eq '2' -and $testTime -eq '20260609161940' -and $program -eq 'TSVET' -and $prefix -eq 'CAI_OMM_ATV_A0') "lot=$lot slot=$slot time=$testTime program=$program prefix=$prefix"

# 2. xlsx 읽기 (시트 DUT Summary / 1행 헤더 / A열 Test Number·Unit 행 / #n Die 행)
# Excel 이 파일을 열고 있어도 읽을 수 있게 공유 읽기 스트림으로 연다 (2026-10-07 실측: 파일 경로로 열면 '다른 프로세스가 사용 중' 오류)
$fs = New-Object IO.FileStream($SampleFile, [IO.FileMode]::Open, [IO.FileAccess]::Read, [IO.FileShare]::ReadWrite)
$wb = New-Object ClosedXML.Excel.XLWorkbook($fs)
$ws = $wb.Worksheet('DUT Summary')
$lastCol = $ws.LastColumnUsed().ColumnNumber(); $lastRow = $ws.LastRowUsed().RowNumber()
$fixed = @{}; $measCols = New-Object Collections.Generic.List[int]
for ($c = 1; $c -le $lastCol; $c++) {
    $h = CellText $ws.Cell(1, $c)
    if ($h.Contains('@')) { $measCols.Add($c) } elseif ($h -ne '') { $fixed[$h] = $c }
}
$rowTestNo = 0; $rowUnit = 0; $dieRows = New-Object Collections.Generic.List[int]
for ($r = 2; $r -le $lastRow; $r++) {
    $a = CellText $ws.Cell($r, 1)
    if ($a -eq 'Test Number') { $rowTestNo = $r } elseif ($a -eq 'Unit') { $rowUnit = $r } elseif ($a -match '^#\d+$') { $dieRows.Add($r) }
}
$re = [regex]'^(?<para>[^=@]+)(?:=(?<cond>[^@]+))?@(?<c1>[^:]+)(?::(?<c2>.+))?$'
$items = New-Object 'string[,]' $measCols.Count, 8
for ($k = 0; $k -lt $measCols.Count; $k++) {
    $c = $measCols[$k]; $h = CellText $ws.Cell(1, $c); $m = $re.Match($h)
    if (-not $m.Success) { throw "헤더 분해 실패: $h" }
    $items[$k, 0] = $h; $items[$k, 1] = $m.Groups['para'].Value; $items[$k, 2] = $m.Groups['cond'].Value
    $items[$k, 3] = $m.Groups['c1'].Value; $items[$k, 4] = $m.Groups['c2'].Value
    $items[$k, 5] = CellText $ws.Cell($rowUnit, $c); $items[$k, 6] = CellText $ws.Cell($rowTestNo, $c); $items[$k, 7] = [string]($k + 1)
}
$dies = New-Object 'string[,]' $dieRows.Count, 10
for ($i = 0; $i -lt $dieRows.Count; $i++) {
    $r = $dieRows[$i]
    $xy = (CellText $ws.Cell($r, $fixed['(X, Y)'])) -replace '[()\s]', ''
    $flag = CellText $ws.Cell($r, $fixed['DUT Flag'])
    $dies[$i, 0] = (CellText $ws.Cell($r, 1)).TrimStart('#')
    $dies[$i, 1] = $xy.Split(',')[0]; $dies[$i, 2] = $xy.Split(',')[1]
    $dies[$i, 3] = (CellText $ws.Cell($r, $fixed['Hardware Bin'])) -replace '^Bin\s*', ''
    $dies[$i, 4] = (CellText $ws.Cell($r, $fixed['Software Bin'])) -replace '^Bin\s*', ''
    $dies[$i, 5] = $(if ($flag.StartsWith('Pass')) { 'P' } else { 'F' })
    $dies[$i, 6] = ($flag -split '-\s*')[-1].Trim()
    $dies[$i, 7] = (CellText $ws.Cell($r, $fixed['Test Time'])) -replace '\s*ms$', ''
    $dies[$i, 8] = CellText $ws.Cell($r, $fixed['Tests Executed'])
    $dies[$i, 9] = CellText $ws.Cell($r, $fixed['Test Head - Site'])
}
$readMs = $sw.ElapsedMilliseconds
Check 'xlsx 읽기' ($measCols.Count -eq 2755 -and $dieRows.Count -eq 24 -and $rowUnit -gt 0) "항목 $($measCols.Count) / Die $($dieRows.Count) / Unit 행 $rowUnit / ${readMs}ms"

$bsl = New-Object DACrux.SEMDMS.BSL.GdsEtResult
# 2026-10-07 중복 키 변경: Factory + WAFER_ID (= Lot + '-' + 2자리 Slot). Device / 시험 일시는 키가 아니다.
$fileKey = [string[]]@($Factory, $lot, $slot)
$fileKey3 = [string[]]@($Factory, $lot, '3')

# 이전 실행 잔여 정리 (같은 Factory 의 Wafer -02 / -03, Device 무관)
foreach ($k in @(, $fileKey) + @(, $fileKey3)) { foreach ($r in @($bsl.GetEtFileByKey($k).Rows)) { [void]$bsl.DeleteEtFile([string]$r['SRC_FILE_SEQ'], 'probe') } }
$itemBefore = $bsl.GetEtItemList($Factory, $Device).Rows.Count

# 3. 저장: Begin -> Merge -> Die -> Value(10,000행씩) -> Complete
$sw.Restart()
$info = [string[]]@($Factory, $Device, [IO.Path]::GetFileName($SampleFile), $lot, $slot, $program, $testTime, $prefix, 'DUT Summary',
    [string]$dieRows.Count, [string]$measCols.Count, 'probe')
$head = $bsl.BeginEtFile($info).Rows[0]
$seq = [string]$head['SRC_FILE_SEQ']
Check '헤더 생성 / WAFER_ID' ($head['WAFER_ID'] -eq 'RH-HS2615-02' -and $head['LOAD_STATUS'] -eq 'LOADING') "seq=$seq wafer=$($head['WAFER_ID']) status=$($head['LOAD_STATUS'])"

$itemTable = $bsl.MergeEtItem([string[]]@($Factory, $Device), $items, 'probe')
$itemSeq = @{}; foreach ($r in $itemTable.Rows) { $itemSeq[[string]$r['ITEM_NAME']] = [string]$r['ET_ITEM_SEQ'] }
Check '항목 등록' ($itemTable.Rows.Count -ge 2755 -and ($itemTable.Rows.Count - $itemBefore) -eq (2755 - [Math]::Min($itemBefore, 2755))) "전 $itemBefore -> 후 $($itemTable.Rows.Count)"

$nd = $bsl.CreateEtDieBatch($seq, $dies, 'probe')

$values = New-Object Collections.Generic.List[string[]]
$notTested = 0
foreach ($i in 0..($dieRows.Count - 1)) {
    $r = $dieRows[$i]; $dut = $dies[$i, 0]
    for ($k = 0; $k -lt $measCols.Count; $k++) {
        $cell = $ws.Cell($r, $measCols[$k])
        $t = CellText $cell
        if ($cell.Value -is [double]) { $values.Add([string[]]@($dut, $itemSeq[$items[$k, 0]], $t, 'V')) }
        elseif ($t -eq '-') { $values.Add([string[]]@($dut, $itemSeq[$items[$k, 0]], '', 'N')); $notTested++ }
        elseif ($t -eq '') { $values.Add([string[]]@($dut, $itemSeq[$items[$k, 0]], '', 'E')) }
        else { throw "알 수 없는 값: DUT $dut / $($items[$k, 0]) = $t" }
    }
}
$buildMs = $sw.ElapsedMilliseconds
$sw.Restart()
$nv = 0
for ($s = 0; $s -lt $values.Count; $s += 10000) {
    $n = [Math]::Min(10000, $values.Count - $s)
    $a = New-Object 'string[,]' $n, 4
    for ($j = 0; $j -lt $n; $j++) { $v = $values[$s + $j]; for ($c = 0; $c -lt 4; $c++) { $a[$j, $c] = $v[$c] } }
    $nv += $bsl.CreateEtValueBatch($seq, $a, 'probe')
}
$saveMs = $sw.ElapsedMilliseconds
$c = $bsl.CompleteEtFile($seq, 'probe').Rows[0]
Check '저장 확정 READY' ($c['RESULT'] -eq 'OK' -and $c['LOAD_STATUS'] -eq 'READY' -and $nd -eq 24 -and $nv -eq 66120) "die=$nd value=$nv $($c['MESSAGE']) / 값 행 만들기 ${buildMs}ms / 측정값 저장 ${saveMs}ms"
Check 'DB 건수' ($c['DB_DIE'] -eq 24 -and $c['FAIL_DIE'] -eq 4 -and $c['DB_VALUE'] -eq 66120 -and $c['NOT_TESTED'] -eq 17 -and $c['EMPTY'] -eq 0) "DB Die $($c['DB_DIE']) / Fail $($c['FAIL_DIE']) / 측정값 $($c['DB_VALUE']) / 미측정 $($c['NOT_TESTED']) (파일 $notTested) / 빈 값 $($c['EMPTY'])"

# 4. 재조회 일치
$f = $bsl.GetEtFile($seq).Rows[0]
Check '헤더 재조회' ($f['VALUE_COUNT'] -eq 66120 -and $f['TEST_TIME'] -eq '20260609161940' -and $f['FILE_PREFIX'] -eq 'CAI_OMM_ATV_A0') "value_count=$($f['VALUE_COUNT']) time=$($f['TEST_TIME'])"
$d20 = @($bsl.GetEtDieList($seq).Rows | Where-Object { $_['DUT_NO'] -eq 20 })[0]
Check 'Die 재조회 (#20)' ($d20['DIE_X'] -eq 7 -and $d20['DIE_Y'] -eq 3 -and $d20['SW_BIN'] -eq 5001 -and $d20['PASS_FLAG'] -eq 'F' -and $d20['FLAG_CODE'] -eq '0x08' -and $d20['TEST_TIME_MS'] -eq 9461) "($($d20['DIE_X']),$($d20['DIE_Y'])) bin=$($d20['HW_BIN'])/$($d20['SW_BIN']) $($d20['PASS_FLAG']) $($d20['FLAG_CODE']) $($d20['TEST_TIME_MS'])ms"
$vt = $bsl.GetEtValueList($seq)
$diff = 0
$byKey = @{}; foreach ($r in $vt.Rows) { $byKey[[string]$r['DUT_NO'] + '|' + [string]$r['ET_ITEM_SEQ']] = $r }
foreach ($v in $values) {
    $r = $byKey[$v[0] + '|' + $v[1]]
    if ($r -eq $null) { $diff++; continue }
    if ($v[3] -eq 'V') { if ($r['VALUE_STATUS'] -ne 'V' -or ([double]$r['MEAS_VALUE']).ToString('R', $inv) -ne $v[2]) { $diff++ } }
    elseif ($r['VALUE_STATUS'] -ne $v[3] -or $r['MEAS_VALUE'] -ne [DBNull]::Value) { $diff++ }
}
Check '측정값 재조회 (전체 비교)' ($vt.Rows.Count -eq 66120 -and $diff -eq 0) "rows=$($vt.Rows.Count) 다른 값=$diff"

# 5. 거부 / 재사용
$m = Throws { $bsl.BeginEtFile($info) }
Check '같은 Wafer 재저장 거부' ($m -like '*이미 있습니다*') $m
$infoOtherDevice = [string[]]$info.Clone(); $infoOtherDevice[1] = $Device + '_OTHER'; $infoOtherDevice[6] = '20260610090000'
$m = Throws { $bsl.BeginEtFile($infoOtherDevice) }
Check '같은 Wafer 다른 Device / 다른 시험 일시도 거부' ($m -like '*이미 있습니다*' -and $m -like "*Device $Device*") $m
$infoSlot3 = [string[]]$info.Clone(); $infoSlot3[4] = '3'
$h3 = $bsl.BeginEtFile($infoSlot3).Rows[0]
Check '같은 Device 다른 Slot 은 저장 가능 (WAFER_ID -03)' ($h3['WAFER_ID'] -eq 'RH-HS2615-03') "seq=$($h3['SRC_FILE_SEQ']) wafer=$($h3['WAFER_ID'])"
[void]$bsl.DeleteEtFile([string]$h3['SRC_FILE_SEQ'], 'probe')
$m = Throws { $bsl.CreateEtValueBatch($seq, $a, 'probe') }
Check 'READY 후 배치 거부' ($m -like '*READY*') $m
$again = $bsl.MergeEtItem([string[]]@($Factory, $Device), $items, 'probe')
Check '같은 항목 재등록 시 추가 0' ($again.Rows.Count -eq $itemTable.Rows.Count) "항목 $($again.Rows.Count)"
$bad = New-Object 'string[,]' 1, 8
for ($c2 = 0; $c2 -lt 8; $c2++) { $bad[0, $c2] = $items[0, $c2] }
$bad[0, 5] = 'mV'
$m = Throws { $bsl.MergeEtItem([string[]]@($Factory, $Device), $bad, 'probe') }
Check '단위 다른 항목 거부' ($m -like '*단위가 다릅니다*') $m
$m = Throws { $bsl.BeginEtFile([string[]]@($Factory, $Device, 'x.xlsx', $lot, '100', $program, $testTime, '', '', '1', '1', 'probe')) }
Check 'Slot 범위 거부' ($m -like '*Slot*') $m
$m = Throws { $bsl.BeginEtFile([string[]]@($Factory, $Device, 'x.xlsx', $lot, '3', $program, '2026060916194', '', '', '1', '1', 'probe')) }
Check '시험 일시 형식 거부' ($m -like '*yyyyMMddHHmmss*') $m

# 6. 삭제 (항목은 남음)
if (-not $Keep) {
    $nDel = $bsl.DeleteEtFile($seq, 'probe')
    $left = $bsl.GetEtFileByKey($fileKey).Rows.Count
    Check '삭제' ($nDel -eq 66120 -and $left -eq 0 -and $bsl.GetEtDieList($seq).Rows.Count -eq 0 -and $bsl.GetEtItemList($Factory, $Device).Rows.Count -eq $itemTable.Rows.Count) "측정값 $nDel 삭제 / 파일 $left / 항목 유지 $($itemTable.Rows.Count)"
} else { Write-Host "Keep: SRC_FILE_SEQ=$seq 남김" }
$wb.Dispose(); $fs.Dispose()

$fail = @($results | Where-Object { $_ -like 'FAIL*' }).Count
Write-Host ("ET_RESULT_PROBE=" + $(if ($fail -eq 0) { 'PASS' } else { 'FAIL' }) + " ($($results.Count - $fail)/$($results.Count))")
if ($fail -gt 0) { exit 1 }
