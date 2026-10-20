# E-Test -> YMS 테이블 적재(EtYmsPublisher) 검증 스크립트 (2026-10-14, 설계: 문서/설계/04_ETest/2026-10-14_ETest_Upload_YMS적재_설계.md 14절)
# 목적: 샘플 E-Test xlsx + 고객 Spec 표(TSVET) + 로컬 Map 으로 Wafer 1장을 만들어 EtYmsPublisher 로 YMS 테이블에 넣는다.
#       TQP_LOT / TQP_WAFER / TD_<PROGRAM> / TQP_PARASPEC / TQP_BINDESC / TQP_WAFER_SUM 결과,
#       다시 반영 / Map 밖 Die / 입력 오류 / 삭제를 확인한다.
#       DB 접속은 BSL -> DSL -> Miracom.Middleware 경로다 (Service Remoting 경유 아님). 결과 조회 / 정리는 기존 YMS DSL 쿼리만 쓴다.
#       같은 Lot 에 Wafer 2장을 넣고 1장 / 마지막 1장을 지워 기존 TestDataMaint.DeleteWaferData 의 두 경로(Lot 요약 다시 만들기 / 빈 Lot 지우기)도 본다.
#       YMS BIN = 결과 파일 Hardware Bin, 양품 = 파일 DUT 결과 Pass (2026-10-14, E-Test Spec 판정과 별개). 샘플 RH-HS2615-02 = Bin 1 Pass 20 / Bin 5 Fail 4.
#       예전 판정 Bin 정의(1 PASS / 2 SPEC_FAIL / 3 NOT_TESTED)가 남아 있으면 지우고, 사용자가 고친 정의는 남기는지도 본다.
# 보호: 실제 데이터와 섞이지 않게 Factory PROBE / Device PRB* 만 허용한다. 시작할 때와 끝날 때 그 Program 의 YMS 데이터만 지운다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File EtYmsPublishProbe.ps1 [-BinDir <TEMP 빌드 폴더>] [-Keep]
#       -BinDir 생략 시 101_ServerBin. -Keep 이면 마지막 삭제를 하지 않는다 (DMS YMS 화면에서 PRBOMM_TSVET 를 직접 볼 때).
# 결과: 마지막 줄 ET_YMS_PUBLISH_PROBE=PASS 또는 FAIL (통과 수 / 전체 수)
param(
    [string]$BinDir = '',
    [string]$Factory = 'PROBE',
    [string]$Device = 'PRBOMM',
    [string]$MapId = 'OMM_Hybrid',
    [string]$SampleFile = '',
    [string]$SpecFile = '',
    [switch]$Keep
)
$ErrorActionPreference = 'Stop'
if ($Factory -ne 'PROBE' -or -not $Device.StartsWith('PRB')) { throw "보호 규칙: Factory 는 PROBE, Device 는 PRB 로 시작해야 합니다 ($Factory / $Device)" }
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$serverBin = Join-Path $dms '101_ServerBin'
$clientBin = Join-Path $dms '100_ClientBin'
if ($BinDir -eq '') { $BinDir = $serverBin }
if ($SampleFile -eq '') { $SampleFile = Join-Path $dms '..\SampleFile\26_10_06 Sample File\CAI_OMM_ATV_A0_TSVET_20260609161940_RH-HS2615_RH-HS2615-02.xlsx' }
if ($SpecFile -eq '') { $SpecFile = Join-Path $dms '02_DMS\DACrux.SEMDMS.ENGUI\Verification\EtSpecTable_U9_20261012.txt' }
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
Write-Host "BSL: $(Join-Path $BinDir 'DACrux.SEMDMS.BSL.dll')"
Write-Host "Sample: $SampleFile"

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
$inv = [Globalization.CultureInfo]::InvariantCulture
$Rule = [DACrux.SEMDMS.Interface.EtYmsRule]
$program = $Rule::ToProgramName($Device, 'TSVET')
$pub = New-Object DACrux.SEMDMS.BSL.EtYmsPublisher
# 조회 / 정리는 기존 YMS DSL 과 쿼리만 쓴다
$tdDsl = New-Object DACrux.TEST.DSL.TD_TABLE
$paraDsl = New-Object DACrux.TEST.DSL.TQP_PARASPEC
$binDsl = New-Object DACrux.TEST.DSL.TQP_BINDESC
$tableDsl = New-Object DACrux.TEST.DSL.TQP_DATA_TABLES
$lotDsl = New-Object DACrux.TEST.DSL.TQP_LOT
$lotSumDsl = New-Object DACrux.TEST.DSL.TQP_LOT_SUM
$waferDsl = New-Object DACrux.TEST.DSL.TQP_WAFER

# 시험 Lot 은 1개다 (Factory PROBE + 공정 TSVET + Lot RH-HS2615 + Program). 기존 쿼리 TQP_LOT SELECT_LOT_SEQ_01
$probeLot = 'RH-HS2615'
function ProbeLotSeq() { return $lotDsl.GetLotSeq($Factory, 'TSVET', $probeLot, $program) }
# 행이 1개여도 배열로 돌려준다 (PowerShell 이 배열을 풀어 DataRow 하나로 바꾸지 않게 ',' 를 붙인다). 기존 쿼리 TQP_WAFER SELECT_WAFER_BY_LOT
function ProgramWafers() {
    $lotSeq = ProbeLotSeq
    if (-not $lotSeq) { return ,([object[]]@()) }
    return ,([object[]]@($waferDsl.GetData('SELECT_WAFER_BY_LOT', $null, [string[]]@($lotSeq, $program)).Rows))
}
function BinRows() { return ,([object[]]@($binDsl.GetData('SELECT_BIN_DESC', $null, [string[]]@($program)).Rows | Sort-Object { [int]$_['BIN'] })) }
function TdRows([string]$seq) {
    $ds = $tdDsl.GetTableSchema($Factory, $program, $seq)
    if ($ds.Tables.Count -eq 0) { return $null }
    return ,$ds.Tables[0]   # DataTable 도 PowerShell 이 행으로 풀지 않게
}
# 이 Program 의 YMS 데이터만 지운다 (Wafer -> 빈 TD 테이블 -> Bin / Program / Product)
function CleanProgram() {
    foreach ($r in (ProgramWafers)) { [void]$pub.DeleteWafer([string]$r['WAFER_SEQ']) }
    # Wafer 없이 남은 Lot (이전 실행이 중간에 멈춘 경우)
    $lotSeq = ProbeLotSeq
    if ($lotSeq) { $lotSumDsl.DeleteLot($lotSeq); $lotDsl.DeleteLot($lotSeq) }
    $tm = New-Object DACrux.TEST.BSL.TestDataManager
    foreach ($t in @($tableDsl.GetTableNames($Factory, $program))) { if ($t) { $tm.CheckTableEmpty($t) } }
    [void]$binDsl.DeleteDataNonQuery('DELETE_PROGRAM_BIN_ALL', $null, [string[]]@($program))
    $pgm = New-Object DACrux.TEST.DSL.TQP_PROGRAM
    [void]$pgm.DeleteDataNonQuery('DELETE_PROGRAM', $null, [string[]]@($program))
    # TQP_PRODUCT 는 기존 삭제 쿼리가 없어 기존 UPDATE_DELETE_FLAG 로 숨김만 한다 (Probe 전용 Device)
    $prd = New-Object DACrux.TEST.DSL.TQP_PRODUCT
    $prd.UpdateDeleteFlag('Y', $Device)
}
CleanProgram
Check '시작 정리' ((ProgramWafers).Count -eq 0 -and -not (ProbeLotSeq)) "Program $program 남은 Wafer 0 / 시험 Lot 없음"

# 1. Spec 표 (TSVET): Parameter 이름 / 하한 / 상한 / 단위
$spec = New-Object Collections.Generic.List[object]
foreach ($line in [IO.File]::ReadAllLines($SpecFile)) {
    $col = $line.Split("`t")
    if ($col.Length -lt 6 -or $col[1].Trim() -ne 'TSVET') { continue }
    $compact = $col[4] -replace '\s', ''
    $lower = $null; $upper = $null
    if ($compact -match 'abs\(val\)>(?<n>[-+0-9.Ee]+)') { $n = [double]::Parse($Matches['n'], $inv); $lower = -$n; $upper = $n }
    elseif ($compact -match 'val>(?<n>[-+0-9.Ee]+)') { $upper = [double]::Parse($Matches['n'], $inv) }
    elseif ($compact -match 'val<(?<n>[-+0-9.Ee]+)') { $lower = [double]::Parse($Matches['n'], $inv) }
    else { throw "Spec 문장 해석 불가: $($col[4])" }
    $spec.Add([pscustomobject]@{ Name = $Rule::NormalizeParameterName($col[2]); Header = $col[2].Trim(); Unit = $col[5].Trim(); Lower = $lower; Upper = $upper })
}

# 2. 샘플 xlsx: Die (DUT / (X, Y)) + Spec Parameter 값
$fs = New-Object IO.FileStream($SampleFile, [IO.FileMode]::Open, [IO.FileAccess]::Read, [IO.FileShare]::ReadWrite)
try {
    $wb = New-Object ClosedXML.Excel.XLWorkbook($fs)
    $ws = $wb.Worksheet('DUT Summary')
    $lastCol = $ws.LastColumnUsed().ColumnNumber(); $lastRow = $ws.LastRowUsed().RowNumber()
    $colOf = @{}; $xyCol = 0; $hwCol = 0; $swCol = 0; $flagCol = 0
    for ($c = 1; $c -le $lastCol; $c++) {
        $h = [string]$ws.Cell(1, $c).Value
        if ($h.Contains('@')) { $colOf[$Rule::NormalizeParameterName($h)] = $c } elseif ($h.Trim() -eq '(X, Y)') { $xyCol = $c }
        elseif ($h.Trim() -eq 'Hardware Bin') { $hwCol = $c } elseif ($h.Trim() -eq 'Software Bin') { $swCol = $c } elseif ($h.Trim() -eq 'DUT Flag') { $flagCol = $c }
    }
    $rawDies = New-Object Collections.Generic.List[object]
    for ($r = 2; $r -le $lastRow; $r++) {
        $a = ([string]$ws.Cell($r, 1).Value).Trim()
        if ($a -notmatch '^#(\d+)$') { continue }
        $dut = [int]$Matches[1]
        $xy = ([string]$ws.Cell($r, $xyCol).Value) -replace '[()\s]', ''
        $texts = New-Object 'string[]' $spec.Count
        for ($k = 0; $k -lt $spec.Count; $k++) {
            $cell = $ws.Cell($r, $colOf[$spec[$k].Name])
            $v = $cell.Value
            $texts[$k] = $(if ($cell.IsEmpty()) { '' } elseif ($v -is [double]) { $v.ToString('R', $inv) } else { ([string]$v).Trim() })
        }
        $hw = [int](([string]$ws.Cell($r, $hwCol).Value) -replace '[^0-9]', '')
        $sw = [int](([string]$ws.Cell($r, $swCol).Value) -replace '[^0-9]', '')
        $pf = $(if (([string]$ws.Cell($r, $flagCol).Value).Trim().StartsWith('Pass')) { 'P' } else { 'F' })
        $rawDies.Add([pscustomobject]@{ Dut = $dut; X = [int]$xy.Split(',')[0]; Y = [int]$xy.Split(',')[1]; Texts = $texts; Hw = $hw; Sw = $sw; Pass = $pf })
    }
} finally { $fs.Dispose() }
$missing = @($spec | Where-Object { -not $colOf.ContainsKey($_.Name) }).Count
Check '샘플 / Spec 읽기' ($spec.Count -eq 37 -and $missing -eq 0 -and $rawDies.Count -eq 24) "Spec $($spec.Count) / 파일에 없는 Spec $missing / Die $($rawDies.Count)"

# 3. Map: E-Test (X, Y) -> 시스템 좌표 (DieCoordinateMapper ETest), 고객 Die#
$probeAdmin = New-Object DACrux.TEST.BSL.ProbeAdmin
$mapper = [DACrux.SEMDMS.Interface.DieCoordinateMapper]::FromDieTable($probeAdmin.GetDies($MapId), (New-Object DACrux.SEMDMS.Interface.DiePoint(0, 0)))

# 4. 적재 모델 (Die BIN = 파일 Hardware Bin, 양품 = 파일 DUT 결과)
function Build-Wafer() {
    $w = New-Object DACrux.SEMDMS.BSL.EtYmsWafer
    $w.Factory = $Factory; $w.DeviceId = $Device; $w.TestProgram = 'TSVET'; $w.MapId = $MapId
    $w.LotId = 'RH-HS2615'; $w.WaferId = 'RH-HS2615-02'; $w.LotWaferNo = 'RH-HS2615-02'
    $w.FileName = [IO.Path]::GetFileName($SampleFile)
    $w.TestTime = [datetime]::ParseExact('20260609161940', 'yyyyMMddHHmmss', $inv)
    foreach ($s in $spec) {
        $p = New-Object DACrux.SEMDMS.BSL.EtYmsParameter
        $p.ParameterName = $s.Name; $p.YmsName = $Rule::ToParamName($s.Name); $p.Description = $s.Header; $p.Unit = $s.Unit
        if ($null -ne $s.Lower) { $p.Lower = [double]$s.Lower }
        if ($null -ne $s.Upper) { $p.Upper = [double]$s.Upper }
        $w.Parameters.Add($p)
    }
    foreach ($rd in $rawDies) {
        $d = New-Object DACrux.SEMDMS.BSL.EtYmsDie
        $d.DutNo = $rd.Dut
        $sys = New-Object DACrux.SEMDMS.Interface.DiePoint
        if ($mapper.TryToSystem([DACrux.SEMDMS.Interface.DieCoordType]::ETest, (New-Object DACrux.SEMDMS.Interface.DiePoint($rd.X, $rd.Y)), [ref]$sys)) { $d.MapX = $sys.X; $d.MapY = $sys.Y }
        $values = New-Object 'System.Nullable[double][]' $spec.Count
        for ($k = 0; $k -lt $spec.Count; $k++) {
            $t = $rd.Texts[$k]
            if ($t -eq '-' -or $t -eq '') { continue }
            $values[$k] = [double]::Parse($t, [Globalization.NumberStyles]::Float, $inv)
        }
        $d.Values = $values
        $d.HardwareBin = $rd.Hw; $d.SoftwareBin = $rd.Sw; $d.FilePass = $rd.Pass
        $w.Dies.Add($d)
    }
    return $w
}
$wafer = Build-Wafer
$failDies = @($wafer.Dies | Where-Object { $_.FilePass -eq 'F' })
$hw5 = @($wafer.Dies | Where-Object { $_.HardwareBin -eq 5 -and $_.SoftwareBin -eq 5001 }).Count
$inMap = @($wafer.Dies | Where-Object { $_.MapX -ne $null }).Count
$dut20 = $wafer.Dies | Where-Object { $_.DutNo -eq 20 }
$dut44 = $wafer.Dies | Where-Object { $_.DutNo -eq 44 }
Check '파일 Bin / 좌표' ($failDies.Count -eq 4 -and $hw5 -eq 4 -and $inMap -eq 24 -and $dut20.MapX -eq 9 -and $dut20.MapY -eq 2 -and $dut20.HardwareBin -eq 5 -and $dut44.HardwareBin -eq 1 -and $dut44.FilePass -eq 'P') "파일 Fail Die $($failDies.Count) (HW5/SW5001 $hw5) / Map 안 $inMap / DUT #20 -> ($($dut20.MapX), $($dut20.MapY)) HW$($dut20.HardwareBin) / DUT #44 HW$($dut44.HardwareBin) $($dut44.FilePass) (Spec 판정은 Fail 이던 Die)"

# 첫 Die (DIE_NUM 1) 기대값 = 위 행부터 / 왼쪽부터 첫 Die = 고객 Die# 가 가장 작은 Die
$sorted = @($wafer.Dies | Sort-Object @{ Expression = { $_.MapY }; Descending = $true }, @{ Expression = { $_.MapX }; Descending = $false })
$firstDieNo = $mapper.DieNumberOf((New-Object DACrux.SEMDMS.Interface.DiePoint($sorted[0].MapX, $sorted[0].MapY)))
$minDieNo = ($wafer.Dies | ForEach-Object { $mapper.DieNumberOf((New-Object DACrux.SEMDMS.Interface.DiePoint($_.MapX, $_.MapY))) } | Measure-Object -Minimum).Minimum

# 5. 첫 반영. 예전 판정 Bin 정의 1 / 2 (그대로) + 3 (사용자가 설명을 고친 것)을 미리 넣어 둔다.
[void]$binDsl.InsertBinInfo($program, '1', 'PASS', '', 'Y', 'Y', '', '', '#00C000', 'E-Test Spec Pass', '')
[void]$binDsl.InsertBinInfo($program, '2', 'SPEC_FAIL', '', 'N', 'Y', '', '', '#FF0000', 'E-Test Spec Fail (out of limit)', '')
[void]$binDsl.InsertBinInfo($program, '3', 'NOT_TESTED', '', 'N', 'Y', '', '', '#FFA500', 'user edited', '')
$script:createdSeq = ''
$onCreated = [Action[DACrux.SEMDMS.BSL.EtYmsPublishResult]] { param($r) $script:createdSeq = $r.WaferSeq }
$sw = [Diagnostics.Stopwatch]::StartNew()
$res = $pub.Publish($wafer, $null, 'probe', $onCreated)
$ms1 = $sw.ElapsedMilliseconds
Check '반영 결과' ($res.Status -eq 'DONE' -and $res.Program -eq $program -and $res.TestArea -eq 'TSVET' -and $res.DieCount -eq 24 -and $res.SkipDieCount -eq 0 -and $res.ParamCount -eq 37 -and $res.GoodDieCount -eq 20 -and $res.FailDieCount -eq 4 -and $res.BinCounts -eq '1:20, 5:4') "$($res.Status) $($res.Step) $($res.Message) / $($res.Program) / Die $($res.DieCount) Skip $($res.SkipDieCount) / Param $($res.ParamCount) / 양품 $($res.GoodDieCount) 불량 $($res.FailDieCount) / Bin $($res.BinCounts) / ${ms1}ms"
Check '콜백 WAFER_SEQ' ($script:createdSeq -ne '' -and $script:createdSeq -eq $res.WaferSeq) "콜백 $($script:createdSeq) / 결과 $($res.WaferSeq)"
Check '수율 (파일 Bin 기준 20 / 24)' ($res.Yield -eq 83.33 -and $res.TestedDie -eq 24) "YIELD $($res.Yield) / TESTED $($res.TestedDie) / NETDIE $($res.NetDie) (Factory PROBE 라 Map(SCK) NETDIE 조인 안 됨 -> 측정 Die 로 나눔)"
$tbl = $res.ToDataTable()
Check '결과 표' ($tbl.Rows.Count -eq 1 -and [string]$tbl.Rows[0]['YMS_STATUS'] -eq 'DONE' -and [int]$tbl.Rows[0]['FAIL_DIE_COUNT'] -eq 4 -and [string]$tbl.Rows[0]['BIN_COUNTS'] -eq '1:20, 5:4') "열 $($tbl.Columns.Count) / BIN_COUNTS $($tbl.Rows[0]['BIN_COUNTS'])"

# Lot 은 기존 쿼리 GET_WAFER_INFO(TQP_LOT + TQP_WAFER, Lot 컬럼이 앞), Wafer 는 Publisher.GetWafer(GET_WAFER_INFO [WSQ])
$lr = $waferDsl.GetData('GET_WAFER_INFO', $null, [string[]]@($res.WaferSeq)).Rows
$wr = $pub.GetWafer($res.WaferSeq).Rows
Check 'TQP_LOT / TQP_WAFER' ($lr.Count -eq 1 -and $lr[0]['TEST_AREA'] -eq 'TSVET' -and $lr[0]['DEVICE'] -eq $Device -and $lr[0]['DEVICE_ALIAS'] -eq $MapId -and $lr[0]['LOT_ID'] -eq 'RH-HS2615' -and $wr.Count -eq 1 -and $wr[0]['WAFER_ID'] -eq 'RH-HS2615-02' -and [int]$wr[0]['PROBE_CNT'] -eq 0 -and [int]$wr[0]['NUM_DIE'] -eq 24 -and $wr[0]['COMMENTS'] -eq 'RH-HS2615-02' -and $wr[0]['DATA_SOURCE'] -eq $wafer.FileName -and [int]$wr[0]['PROGRAM_REV'] -eq 0) "TestArea $($lr[0]['TEST_AREA']) / Device $($lr[0]['DEVICE']) / Alias $($lr[0]['DEVICE_ALIAS']) / PROBE_CNT $($wr[0]['PROBE_CNT']) / NUM_DIE $($wr[0]['NUM_DIE']) / MAX $($wr[0]['MAX_X']),$($wr[0]['MAX_Y']) / 시간 $($wr[0]['START_TIME'])"

$td = TdRows $res.WaferSeq
$binCounts = @{}; foreach ($row in $td.Rows) { $b = [string]$row['BIN']; $binCounts[$b] = 1 + [int]$binCounts[$b] }
$paramCols = @($wafer.Parameters | ForEach-Object { $_.YmsName })
$minValue = [double]::MaxValue
foreach ($row in $td.Rows) { foreach ($pc in $paramCols) { if ($row[$pc] -ne [DBNull]::Value) { $minValue = [Math]::Min($minValue, [double]$row[$pc]) } } }
$first = @($td.Rows | Where-Object { [int]$_['DIE_NUM'] -eq 1 })[0]
$row20 = @($td.Rows | Where-Object { [int]$_['X'] -eq 9 -and [int]$_['Y'] -eq 2 })
$die44 = $wafer.Dies | Where-Object { $_.DutNo -eq 44 }
$row44 = @($td.Rows | Where-Object { [int]$_['X'] -eq $die44.MapX -and [int]$_['Y'] -eq $die44.MapY })
Check "TD $($td.TableName)" ($td.Rows.Count -eq 24 -and $td.Columns.Count -eq 43 -and $binCounts['1'] -eq 20 -and $binCounts['5'] -eq 4 -and $minValue -gt -1E20) "행 $($td.Rows.Count) / 열 $($td.Columns.Count) / BIN1 $($binCounts['1']) BIN5 $($binCounts['5']) / 최소값 $minValue"
Check 'TD Die 순서 / 좌표 / 값' ([int]$first['X'] -eq $sorted[0].MapX -and [int]$first['Y'] -eq $sorted[0].MapY -and $firstDieNo -eq $minDieNo -and $row20.Count -eq 1 -and [int]$row20[0]['BIN'] -eq 5 -and $row44.Count -eq 1 -and [int]$row44[0]['BIN'] -eq 1) "DIE_NUM 1 = ($($first['X']), $($first['Y'])) Die#$firstDieNo (최소 Die# $minDieNo) / DUT #20 (9,2) BIN $($row20[0]['BIN']) / DUT #44 ($($die44.MapX),$($die44.MapY)) BIN $($row44[0]['BIN'])"
$k44 = 0; for ($k = 0; $k -lt $spec.Count; $k++) { if ($null -ne $die44.Values[$k] -and $die44.Values[$k] -gt 715) { $k44 = $k; break } }
$v44 = [double]$row44[0][$paramCols[$k44]]
Check 'TD 값 일치' ([Math]::Abs($v44 - [double]$die44.Values[$k44]) -lt 1E-9) "$($paramCols[$k44]) = $v44 (파일 $($die44.Values[$k44]))"

$ps = $paraDsl.GetParamSepc($Factory, $program, '0')
$psRows = @($ps.Rows)
$intCount = @($psRows | Where-Object { $_['PARAM_TYPE'] -eq 'INT' }).Count
$floatCount = @($psRows | Where-Object { $_['PARAM_TYPE'] -eq 'FLOAT' }).Count
$resRow = @($psRows | Where-Object { $_['PARAM_NAME'] -eq 'RESISTANCE_IFORCE_8P0MA_TSVDC_TR_FH' })[0]
$lkgRow = @($psRows | Where-Object { $_['PARAM_NAME'] -eq 'LKG_SERIAL_TSVDC_ASIC_HBM_N_FH_TSVDC_ASIC_HBM_P_FL' })[0]
Check 'TQP_PARASPEC' ($psRows.Count -eq 40 -and $intCount -eq 3 -and $floatCount -eq 37 -and [double]$resRow['USL'] -eq 715 -and $resRow['LSL'] -eq [DBNull]::Value -and $resRow['UOM'] -eq 'Ohm' -and $resRow['PARAM_DESC'] -eq 'Resistance_iforce=8.0mA@TSVDC_TR_FH' -and [double]$lkgRow['LSL'] -eq -50 -and [double]$lkgRow['USL'] -eq 50 -and $lkgRow['PARAM_DESC'] -eq 'Leakage_Serial@TSVDC_ASIC_HBM_N_FH:TSVDC_ASIC_HBM_P_FL') "행 $($psRows.Count) (INT $intCount / FLOAT $floatCount) / RES USL $($resRow['USL']) LSL '$($resRow['LSL'])' $($resRow['UOM']) / LKG(줄인 이름) $($lkgRow['LSL']) ~ $($lkgRow['USL'])"

$bd = BinRows
$fail5Color = $Rule::BinColor(5, $false)
Check 'TQP_BINDESC (파일 Bin 1 / 5, 예전 판정 2 삭제, 사용자가 고친 3 유지)' ($bd.Count -eq 3 -and [int]$bd[0]['BIN'] -eq 1 -and $bd[0]['BIN_NAME'] -eq 'HW1 SW1' -and $bd[0]['HIGH_GEC'] -eq 'Y' -and $bd[0]['BIN_COLOR'] -eq '#00C000' -and [int]$bd[1]['BIN'] -eq 3 -and $bd[1]['BIN_NAME'] -eq 'NOT_TESTED' -and [int]$bd[2]['BIN'] -eq 5 -and $bd[2]['BIN_NAME'] -eq 'HW5 SW5001' -and $bd[2]['HIGH_GEC'] -eq 'N' -and $bd[2]['BIN_COLOR'] -eq $fail5Color) (($bd | ForEach-Object { "$($_['BIN']) $($_['BIN_NAME']) $($_['HIGH_GEC']) $($_['BIN_COLOR'])" }) -join ' / ')

$sum = $pub.GetWaferSummary($res.WaferSeq).Rows
Check 'TQP_WAFER_SUM' ($sum.Count -eq 1 -and [int]$sum[0]['TESTED_DIE'] -eq 24 -and [int]$sum[0]['GEC'] -eq 20 -and [double]$sum[0]['YIELD'] -eq 83.33 -and [int]$sum[0]['BIN1'] -eq 20 -and [int]$sum[0]['BIN5'] -eq 4) "TESTED $($sum[0]['TESTED_DIE']) / GEC $($sum[0]['GEC']) / YIELD $($sum[0]['YIELD']) / BIN1 $($sum[0]['BIN1']) BIN5 $($sum[0]['BIN5']) BIN2 '$($sum[0]['BIN2'])'"

# 6. 다시 반영: 이전 Wafer 를 지우고 새로 넣는다. Bin Define 에서 고친 색은 유지한다.
[void]$binDsl.MergeBinInfo($program, '5', 'HW5 SW5001', '', 'N', 'Y', '', '', '#123456', 'changed by probe', '')
$seq1 = $res.WaferSeq
$res2 = $pub.Publish($wafer, $seq1, 'probe', $null)
$wafers2 = ProgramWafers
$td2 = TdRows $res2.WaferSeq
$old = TdRows $seq1
$bin5Color = @((BinRows) | Where-Object { [int]$_['BIN'] -eq 5 })[0]['BIN_COLOR']
$probeCnt2 = $pub.GetWafer($res2.WaferSeq).Rows[0]['PROBE_CNT']
Check '다시 반영' ($res2.Status -eq 'DONE' -and $res2.WaferSeq -ne $seq1 -and $pub.GetWafer($seq1).Rows.Count -eq 0 -and $wafers2.Count -eq 1 -and [int]$probeCnt2 -eq 0 -and $td2.Rows.Count -eq 24 -and ($null -eq $old -or $old.Rows.Count -eq 0) -and $res2.Yield -eq 83.33) "$($res2.Status) / WAFER_SEQ $seq1 -> $($res2.WaferSeq) / Program Wafer $($wafers2.Count) (PROBE_CNT $probeCnt2) / TD $($td2.Rows.Count) / 이전 TD $(if ($null -eq $old) { '테이블 다시 만듦' } else { $old.Rows.Count })"
Check 'Bin 정의 유지' ($bin5Color -eq '#123456') "BIN5 COLOR $bin5Color"

# 6-1. 같은 Lot 에 Wafer 2장 -> 1장 삭제: 기존 TestDataMaint.DeleteWaferData 의 "Wafer 가 남은 Lot 요약 다시 만들기" 경로
$wafer.WaferId = 'RH-HS2615-03'; $wafer.LotWaferNo = 'RH-HS2615-03'
$resB = $pub.Publish($wafer, $null, 'probe', $null)
$wafer.WaferId = 'RH-HS2615-02'; $wafer.LotWaferNo = 'RH-HS2615-02'
$lotSeqA = [string]$pub.GetWafer($res2.WaferSeq).Rows[0]['LOT_SEQ']
$twoWafers = (ProgramWafers).Count
$delB = $pub.DeleteWafer($resB.WaferSeq)
$afterB = (ProgramWafers).Count
$lotSumKept = $lotSumDsl.GetIsLot($lotSeqA)
Check '같은 Lot 2장 중 1장 삭제' ($resB.Status -eq 'DONE' -and [string]$resB.LotSeq -eq $lotSeqA -and $twoWafers -eq 2 -and $delB -and $afterB -eq 1 -and $lotSumKept -and (ProbeLotSeq) -eq $lotSeqA) "반영 $($resB.Status) (Lot $($resB.LotSeq) = $lotSeqA) / Wafer $twoWafers -> $afterB / Lot 요약 유지 $lotSumKept"

# 7. Map 밖 Die: 좌표가 없는 Die 는 넣지 않고 센다
$skipDie = $wafer.Dies[0]; $sx = $skipDie.MapX; $sy = $skipDie.MapY
$skipDie.MapX = $null; $skipDie.MapY = $null
$res3 = $pub.Publish($wafer, $res2.WaferSeq, 'probe', $null)
$skipDie.MapX = $sx; $skipDie.MapY = $sy
$td3 = TdRows $res3.WaferSeq
Check 'Map 밖 Die' ($res3.Status -eq 'DONE' -and $res3.DieCount -eq 23 -and $res3.SkipDieCount -eq 1 -and $td3.Rows.Count -eq 23 -and (ProgramWafers).Count -eq 1) "$($res3.Status) / Die $($res3.DieCount) / Skip $($res3.SkipDieCount) / TD $($td3.Rows.Count) / YIELD $($res3.Yield)"

# 8. 입력 오류: 확인 단계에서 멈추고 이전 반영은 그대로 둔다
$keepName = $wafer.Parameters[0].YmsName
$wafer.Parameters[0].YmsName = '1BAD'
$res4 = $pub.Publish($wafer, $res3.WaferSeq, 'probe', $null)
$wafer.Parameters[0].YmsName = $keepName
$afterBad = ProgramWafers
Check '입력 오류' ($res4.Status -eq 'FAIL' -and $res4.Step -eq '입력 확인' -and $afterBad.Count -eq 1 -and [string]$afterBad[0]['WAFER_SEQ'] -eq $res3.WaferSeq) "$($res4.Status) / $($res4.Message)"
$wafer.MapId = ''
$res5 = $pub.Publish($wafer, $res3.WaferSeq, 'probe', $null)
$wafer.MapId = $MapId
Check 'Map ID 없음' ($res5.Status -eq 'FAIL' -and $res5.Step -eq '입력 확인' -and (ProgramWafers).Count -eq 1) $res5.Message
$keepHw = $wafer.Dies[1].HardwareBin
$wafer.Dies[1].HardwareBin = $null
$res6 = $pub.Publish($wafer, $res3.WaferSeq, 'probe', $null)
$wafer.Dies[1].HardwareBin = 100
$res7 = $pub.Publish($wafer, $res3.WaferSeq, 'probe', $null)
$wafer.Dies[1].HardwareBin = $keepHw
Check 'Hardware Bin 없음 / 100 이상' ($res6.Status -eq 'FAIL' -and $res6.Step -eq '입력 확인' -and $res6.Message.Contains("DUT #$($wafer.Dies[1].DutNo)") -and $res7.Status -eq 'FAIL' -and $res7.Step -eq '입력 확인' -and (ProgramWafers).Count -eq 1) "$($res6.Message) / $($res7.Message)"

if ($Keep) {
    $resKeep = $pub.Publish($wafer, $res3.WaferSeq, 'probe', $null)
    Check '-Keep: 전체 Die 로 다시 반영' ($resKeep.Status -eq 'DONE' -and $resKeep.DieCount -eq 24) "WAFER_SEQ $($resKeep.WaferSeq) / Program $program / Factory $Factory (YMS Wafer 목록은 Factory 조건 없음, 기간 2026-06-09 포함)"
} else {
    # 9. Lot 의 마지막 Wafer 삭제: 기존 TestDataMaint.DeleteWaferData 의 "빈 Lot 지우기" 경로
    #    Wafer / 요약 / TD 행 + 빈 TD 테이블 / PARASPEC / DATA_TABLES 정리 + Lot / Lot 요약 삭제 (2026-10-14 수정 전에는 ORA-01400)
    $lotSeqLast = [string]$pub.GetWafer($res3.WaferSeq).Rows[0]['LOT_SEQ']
    $deleted = $pub.DeleteWafer($res3.WaferSeq)
    $again = $pub.DeleteWafer($res3.WaferSeq)
    $tables = @($tableDsl.GetTableNames($Factory, $program) | Where-Object { $_ })
    $paraLeft = $paraDsl.GetParamSepc($Factory, $program, '0').Rows.Count
    $lotLeft = ProbeLotSeq
    $lotSumLeft = $lotSumDsl.GetIsLot($lotSeqLast)
    Check '마지막 Wafer 삭제' ($deleted -and -not $again -and (ProgramWafers).Count -eq 0 -and $pub.GetWafer($res3.WaferSeq).Rows.Count -eq 0 -and $pub.GetWaferSummary($res3.WaferSeq).Rows.Count -eq 0 -and $tables.Count -eq 0 -and $paraLeft -eq 0 -and -not $lotLeft -and -not $lotSumLeft) "삭제 $deleted / 다시 삭제 $again / 남은 TD 테이블 $($tables.Count) / PARASPEC $paraLeft / Lot $(if ($lotLeft) { $lotLeft } else { '없음' }) / Lot 요약 $lotSumLeft"
    CleanProgram
    $binLeft = (BinRows).Count
    Check '끝 정리' ((ProgramWafers).Count -eq 0 -and $binLeft -eq 0) "Wafer 0 / BINDESC $binLeft"
}

$pass = @($results | Where-Object { $_.StartsWith('PASS') }).Count
$total = $results.Count
Write-Host ''
Write-Host ("ET_YMS_PUBLISH_PROBE={0} ({1}/{2})" -f $(if ($pass -eq $total) { 'PASS' } else { 'FAIL' }), $pass, $total)
if ($pass -ne $total) { exit 1 }
