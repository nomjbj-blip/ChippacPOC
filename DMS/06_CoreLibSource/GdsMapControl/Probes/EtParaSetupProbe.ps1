# E-Test Para 설정 서비스 검증 스크립트 (2026-10-14)
# 설계: 문서/설계/04_ETest/2026-10-14_ETest_Para_Setup_설계.md (등록 / 이름 변환), 문서/설계/04_ETest/2026-10-14_ETest_Para_화면_재구성_설계.md (GDS Chain / Spec 편집)
# 목적: 이름 분해 / 변환 규칙(EtParameterNameRule)과 BSL GdsEtResult 의 Parameter 등록 / [등록 Parameter] 탭 저장(수정 행만, GDS Chain + Spec) /
#       GDS Chain 참고 목록 / 인계 API 를 같은 프로세스에서 직접 호출해 확인한다. DB 접속은 BSL -> DSL -> Miracom.Middleware 경로다(Service Remoting 경유 아님).
# 전제: FACTORY=PROBE / DEVICE_ID=LAYER_FILTER_PROBE 에 READY GDS Map 이 있어야 한다(GdsChainProbe 와 같은 전제, Layer 1 도형 사용).
# 결과: E-Test Device PRB_PARA 에 Parameter 를 등록하고, LAYER_FILTER_PROBE 를 E-Test Device 이름(PRB_PARA)으로 복사한 GDS Map 에
#       시험 Chain(Code ZZETP*)을 만들었다가 -Keep 이 없으면 마지막에 모두 지운다(GDS Device = E-Test Device 와 같은 이름, 2026-10-15).
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File EtParaSetupProbe.ps1 [-BinDir <BSL/DSL/Interface DLL 폴더>] [-Keep]
#       -BinDir 생략 시 101_ServerBin. 배포 전에는 TEMP 빌드 폴더를 넘긴다.
param(
    [string]$BinDir = '',
    [string]$Factory = 'PROBE',
    [string]$EtDevice = 'PRB_PARA',
    [string]$GdsSource = 'LAYER_FILTER_PROBE',
    [string]$SampleFile = '',
    [string]$SpecTable = '',
    [switch]$Keep
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$serverBin = Join-Path $dms '101_ServerBin'
$clientBin = Join-Path $dms '100_ClientBin'
if ($BinDir -eq '') { $BinDir = $serverBin }
# GDS Device = E-Test Device 와 같은 이름의 Map (시험 중에 GdsSource 를 복사해 만든다, 2026-10-15)
$GdsDevice = $EtDevice
if ($SampleFile -eq '') { $SampleFile = Join-Path $dms '..\SampleFile\26_10_06 Sample File\CAI_OMM_ATV_A0_TSVET_20260609161940_RH-HS2615_RH-HS2615-02.xlsx' }
if ($SpecTable -eq '') { $SpecTable = Join-Path $dms '02_DMS\DACrux.SEMDMS.ENGUI\Verification\EtSpecTable_U9_20261012.txt' }
$SampleFile = (Resolve-Path $SampleFile).Path
$SpecTable = (Resolve-Path $SpecTable).Path

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
$rule = [DACrux.SEMDMS.Interface.EtParameterNameRule]
$inv = [Globalization.CultureInfo]::InvariantCulture

# ---------------------------------------------------------------------------------------------------
# 1. 이름 규칙 (DB 없음)
# ---------------------------------------------------------------------------------------------------
$p = $rule::Parse('Resistance_iforce=8.0mA@TSVDC_TR_FH')
Check '분해: Resistance' ($p.IsValid -and $p.ParameterName -eq 'RESISTANCE_IFORCE_8P0MA_TSVDC_TR_FH' -and $p.HeaderText -eq 'Resistance_iforce=8.0mA@TSVDC_TR_FH' -and $p.ParaName -eq 'RESISTANCE_IFORCE' -and $p.CondValue -eq '8.0MA' -and $p.MeasType -eq 'RESISTANCE' -and $p.Condition -eq 'IFORCE=8.0MA' -and $p.Chain1 -eq 'TSVDC_TR_FH' -and $p.ForceLeg1 -eq 'FH' -and $p.ChainType -eq 'TSVDC') "$($p.ParameterName) / $($p.Condition) / $($p.Chain1)"
$p = $rule::Parse('  Leakage_Serial@TSVDC_ASIC1_N_FL:TSVDC_ASIC1_P_FH #421 ')
Check '분해: FT Leakage_Serial (#번호 제거 / Chain 2개)' ($p.IsValid -and $p.HeaderText -eq 'Leakage_Serial@TSVDC_ASIC1_N_FL:TSVDC_ASIC1_P_FH' -and $p.ParameterName -eq 'LEAKAGE_SERIAL_TSVDC_ASIC1_N_FL_TSVDC_ASIC1_P_FH' -and $p.Chain1 -eq 'TSVDC_ASIC1_N_FL' -and $p.Chain2 -eq 'TSVDC_ASIC1_P_FH' -and $p.ForceLeg1 -eq 'FL' -and $p.ForceLeg2 -eq 'FH' -and $p.Condition -eq 'SERIAL') "$($p.HeaderText) -> $($p.ParameterName)"
$p = $rule::Parse('Leakage_Serial@TSVDC_ASIC_HBM_N_FH:TSVDC_ASIC_HBM_P_FL')
Check '분해: 50자 초과 -> 고객 약어(LKG)' ($p.IsValid -and $p.ParameterName -eq 'LKG_SERIAL_TSVDC_ASIC_HBM_N_FH_TSVDC_ASIC_HBM_P_FL' -and $p.ParameterName.Length -le 50) "$($p.ParameterName) ($($p.ParameterName.Length)자)"
$p = $rule::Parse('Leakage_Serial@PIC_SERP1_M2_FL:PIC_SERP1_M3_FL')
Check '분해: Chain type PIC_SERP' ($p.IsValid -and $p.ChainType -eq 'PIC_SERP' -and $p.Chain2 -eq 'PIC_SERP1_M3_FL') $p.ChainType
$p = $rule::Parse('Resistance_iforce=8.0mA TSVDC_TR_FH')
$p2 = $rule::Parse('Resistance@TSVDC_TR')
Check '분해: @ 없음 오류 / Force leg 없음 경고' ((-not $p.IsValid) -and $p2.IsValid -and $p2.Warning -ne '') "$($p.Error) / $($p2.Warning)"
Check '이름 변환: 대표 / Chain' ($rule::ToSafeName('tsvdc top1') -eq 'TSVDC_TOP1' -and $rule::ToSafeName(' TSVDC_TR_FH ') -eq 'TSVDC_TR_FH' -and $rule::ToSafeName('a.b@c:d') -eq 'APB_C_D' -and -not $rule::IsSafeName($rule::ToSafeName('1abc'), 64) -and $rule::IsSafeName('TSVDC_TOP1', 64)) "tsvdc top1 -> $($rule::ToSafeName('tsvdc top1'))"
$cl = $rule::FindNameProblems([string[]]@('Resistance_iforce=8.0mA@TSVDC_TR_FH', 'Resistance_iforce_8.0mA@TSVDC_TR_FH', 'resistance_iforce=8.0mA@TSVDC_TR_FH'))
Check '충돌 검사: 다른 헤더가 같은 이름 / 같은 헤더 두 번' ($cl.Length -eq 2) ($cl -join ' | ')

# 샘플 파일 헤더 전부 (DUT Summary 1행 '@' 열, 5행 Unit, 2행 Test Number)
$fs = New-Object IO.FileStream($SampleFile, [IO.FileMode]::Open, [IO.FileAccess]::Read, [IO.FileShare]::ReadWrite)
$wb = New-Object ClosedXML.Excel.XLWorkbook($fs)
$ws = $wb.Worksheet('DUT Summary')
$lastCol = $ws.LastColumnUsed().ColumnNumber(); $lastRow = $ws.LastRowUsed().RowNumber()
$rowTestNo = 0; $rowUnit = 0
for ($r = 2; $r -le $lastRow; $r++) { $a = ([string]$ws.Cell($r, 1).Value).Trim(); if ($a -eq 'Test Number') { $rowTestNo = $r } elseif ($a -eq 'Unit') { $rowUnit = $r } }
$headers = New-Object Collections.Generic.List[object]
for ($c = 1; $c -le $lastCol; $c++) {
    $h = ([string]$ws.Cell(1, $c).Value).Trim()
    if ($h.Contains('@')) { $headers.Add(@($h, ([string]$ws.Cell($rowUnit, $c).Value).Trim(), ([string]$ws.Cell($rowTestNo, $c).Value).Trim(), [string]$c)) }
}
$wb.Dispose(); $fs.Dispose()
$parts = @($headers | ForEach-Object { $rule::Parse($_[0]) })
$terms = New-Object 'Collections.Generic.HashSet[string]'
foreach ($x in $parts) { [void]$terms.Add($x.Chain1); if ($x.Chain2 -ne '') { [void]$terms.Add($x.Chain2) } }
$bad = @($parts | Where-Object { -not $_.IsValid }).Count
$legless = @($terms | Where-Object { $rule::ForceLegOf($_) -eq '' }).Count
$maxLen = ($parts | ForEach-Object { $_.ParameterName.Length } | Measure-Object -Maximum).Maximum
$abbr = @($parts | Where-Object { $_.ParameterName.StartsWith('LKG_') -or $_.ParameterName.StartsWith('RES_') }).Count
$prob = $rule::FindNameProblems([string[]]@($headers | ForEach-Object { $_[0] }))
Check '샘플 헤더 2,755: 오류 0 / 충돌 0 / Chain 1,306 / Force leg 없음 0 / 최대 50자 / 약어 1' ($headers.Count -eq 2755 -and $bad -eq 0 -and $prob.Length -eq 0 -and $terms.Count -eq 1306 -and $legless -eq 0 -and $maxLen -le 50 -and $abbr -eq 1) "헤더 $($headers.Count) / 오류 $bad / 충돌 $($prob.Length) / Chain $($terms.Count) / leg 없음 $legless / 최대 $maxLen / 약어 $abbr"

# 고객 Spec 표 119행 (Device / Test Type / Chain name / Parameter / Spec / Unit)
$spec = @(Get-Content -Path $SpecTable -Encoding UTF8 | Select-Object -Skip 1 | Where-Object { $_.Trim() -ne '' } | ForEach-Object { , ($_ -split "`t") })
$specChains = New-Object 'Collections.Generic.HashSet[string]'
$specBad = 0; $specProb = 0
foreach ($tt in @('TSVET', 'COWET', 'FT')) {
    $hs = @($spec | Where-Object { $_[1].Trim() -eq $tt } | ForEach-Object { $_[2] })
    $specProb += $rule::FindNameProblems([string[]]$hs).Length
}
foreach ($row in $spec) {
    $x = $rule::Parse($row[2]); if (-not $x.IsValid) { $specBad++ ; continue }
    [void]$specChains.Add($x.Chain1); if ($x.Chain2 -ne '') { [void]$specChains.Add($x.Chain2) }
}
Check 'Spec 표 119: 오류 0 / 공정별 충돌 0 / Chain 52' ($spec.Count -eq 119 -and $specBad -eq 0 -and $specProb -eq 0 -and $specChains.Count -eq 52) "행 $($spec.Count) / 오류 $specBad / 충돌 $specProb / Chain $($specChains.Count)"

# ---------------------------------------------------------------------------------------------------
# 2. 서비스 (DB)
# ---------------------------------------------------------------------------------------------------
$bsl = New-Object DACrux.SEMDMS.BSL.GdsEtResult
$chain = New-Object DACrux.SEMDMS.BSL.GdsChain
$import = New-Object DACrux.SEMDMS.BSL.GdsMapImport
$info = $import.GetMapInfo($Factory, $GdsSource)
if ($info.Rows.Count -eq 0 -or $info.Rows[0]['MAP_STATUS'] -ne 'READY') { Write-Host "전제 실패: $Factory / $GdsSource READY Map 없음 (GdsImportLayerFilterProbe.ps1 -Keep 먼저 실행)"; exit 2 }
$placed = New-Object DACrux.SEMDMS.DSL.TQP_GDS_PLACED_EL
$ids = @($placed.GetData('SELECT_PLACED_EL_PAGE', $null, [string[]]@($Factory, $GdsSource, '1', ' ', '6')).Rows | ForEach-Object { [string]$_['PLACED_ELEMENT_ID'] })

function Remove-ProbeData {
    foreach ($r in @($chain.GetChainList($Factory, $GdsDevice).Rows)) { if ([string]$r['CHAIN_CODE'] -like 'ZZETP*') { [void]$chain.DeleteChain($Factory, $GdsDevice, [string]$r['CHAIN_SEQ'], 'probe') } }
    foreach ($t in @('TSVET', 'COWET')) {
        $names = @($bsl.GetEtParameterList($Factory, $EtDevice, $t).Rows | ForEach-Object { [string]$_['ITEM_NAME'] })
        if ($names.Count -gt 0) { [void]$bsl.DeleteEtParameters([string[]]@($Factory, $EtDevice, $t), [string[]]$names, 'probe') }
    }
    if ($import.GetMapInfo($Factory, $GdsDevice).Rows.Count -gt 0) { [void]$import.DeleteMap($Factory, $GdsDevice, 'probe') }
}
function New-ProbeChain($code, $name, $seq = '') {
    $s = [string]$chain.BeginChainSave([string[]]@($Factory, $GdsDevice, $seq, $code, $name, '-16711936', '0', '10', 'probe'), (Rows @(,@('1')) 1), $null).Rows[0]['CHAIN_SEQ']
    [void]$chain.CreateChainMemberBatch([string[]]@($Factory, $GdsDevice, $s), (Rows @(,@($ids[0], 'INPUT', 'ENDPOINT', '0', '0')) 5), 'probe')
    $r = $chain.CompleteChainSave([string[]]@($Factory, $GdsDevice, $s, '1', '0'), 'probe').Rows[0]
    if ($r['RESULT'] -ne 'OK') { throw "GDS 시험 Chain 저장 실패: $code $($r['MESSAGE'])" }
    return $s
}
function SetupRow($t, $name) { return @($bsl.GetEtParameterSetupList($Factory, $EtDevice, $t).Rows | Where-Object { [string]$_['ITEM_NAME'] -eq $name })[0] }
function SaveSetup($t, $rows, $user) { return $bsl.SaveEtParameterSetup([string[]]@($Factory, $EtDevice, $t), (Rows $rows 7), $user).Rows[0] }

Remove-ProbeData
[void]$import.CopyMap($Factory, $GdsSource, $GdsDevice, 'probe')

# 2-1. Parameter 등록: 샘플 파일에서 Spec 표 TSVET 헤더 37개 (원문 / 단위 / Test No / 열 순서)
$specTsvet = New-Object 'Collections.Generic.HashSet[string]'
foreach ($row in $spec) { if ($row[1].Trim() -eq 'TSVET') { [void]$specTsvet.Add($rule::Parse($row[2]).ParameterName) } }
$pick = @($headers | Where-Object { $specTsvet.Contains($rule::Parse($_[0]).ParameterName) })
$reg = $bsl.RegisterEtParameters([string[]]@($Factory, $EtDevice, 'tsvet', 'FILE', [IO.Path]::GetFileName($SampleFile)), (Rows $pick 4), 'probe').Rows[0]
$list = $bsl.GetEtParameterList($Factory, $EtDevice, 'TSVET')
$r0 = @($list.Rows | Where-Object { [string]$_['ITEM_NAME'] -eq 'RESISTANCE_IFORCE_8P0MA_TSVDC_TR_FH' })[0]
Check '등록: TSVET 37 (공정 소문자 -> TSVET, 변환 이름 / 원문 / 단위 / 출처)' ($pick.Count -eq 37 -and [int]$reg['REGISTERED'] -eq 37 -and [int]$reg['SKIPPED'] -eq 0 -and $list.Rows.Count -eq 37 -and $r0 -ne $null -and [string]$r0['HEADER_TEXT'] -eq 'Resistance_iforce=8.0mA@TSVDC_TR_FH' -and [string]$r0['UNIT'] -eq 'Ohm' -and [string]$r0['REG_SOURCE'] -eq 'FILE' -and [string]$r0['USE_FLAG'] -eq 'Y' -and [string]$r0['CHAIN_NAME'] -eq 'TSVDC_TR_FH' -and [string]$r0['FORCE_LEG'] -eq 'FH' -and [string]$r0['MEAS_TYPE'] -eq 'RESISTANCE' -and [int]$r0['FILE_COUNT'] -eq 0) "등록 $($reg['REGISTERED']) / 목록 $($list.Rows.Count) / 단위 $($r0['UNIT'])"
$reg2 = $bsl.RegisterEtParameters([string[]]@($Factory, $EtDevice, 'TSVET', 'FILE', ''), (Rows $pick 4), 'probe').Rows[0]
Check '다시 등록: 37 건너뜀' ([int]$reg2['REGISTERED'] -eq 0 -and [int]$reg2['SKIPPED'] -eq 37) "등록 $($reg2['REGISTERED']) / 건너뜀 $($reg2['SKIPPED'])"
$prog = @($bsl.GetEtParaProgramList($Factory).Rows | Where-Object { [string]$_['DEVICE_ID'] -eq $EtDevice })
Check 'Device + 공정 목록' ($prog.Count -eq 1 -and [string]$prog[0]['TEST_PROGRAM'] -eq 'TSVET' -and [int]$prog[0]['PARA_COUNT'] -eq 37) "$($prog.Count)행"

$e1 = Throws { $bsl.RegisterEtParameters([string[]]@($Factory, $EtDevice, 'COWET', 'SPEC', ''), (Rows @(,@('Resistance_iforce=8.0mA TSVDC_TR_FH', '', '', '')) 4), 'probe') }
$e2 = Throws { $bsl.RegisterEtParameters([string[]]@($Factory, $EtDevice, 'COWET', 'SPEC', ''), (Rows @(@('Resistance_iforce=8.0mA@TSVDC_TR_FH', '', '', ''), @('Resistance_iforce_8.0mA@TSVDC_TR_FH', '', '', '')) 4), 'probe') }
$e3 = Throws { $bsl.RegisterEtParameters([string[]]@($Factory, $EtDevice, 'TSVET', 'FILE', ''), (Rows @(,@('Resistance_iforce=8.0mA@TSVDC_TR_FH', 'V', '', '')) 4), 'probe') }
$e4 = Throws { $bsl.RegisterEtParameters([string[]]@($Factory, $EtDevice, 'TSVET', 'WEB', ''), (Rows @(,@('Resistance_iforce=8.0mA@TSVDC_TR_FH', '', '', '')) 4), 'probe') }
$cowet = @($bsl.GetEtParameterList($Factory, $EtDevice, 'COWET').Rows).Count
Check '등록 거부: @ 없음 / 같은 이름 충돌 / 단위 다름 / 출처 / (전체 거부라 COWET 0행)' ($e1 -ne '' -and $e2 -match '같은 Parameter 이름' -and $e3 -match '단위' -and $e4 -ne '' -and $cowet -eq 0) "$e1 | $e2 | $e3"
$regSpec = $bsl.RegisterEtParameters([string[]]@($Factory, $EtDevice, 'COWET', 'SPEC', 'EtSpecTable_U9_20261012.txt'), (Rows @(,@('Resistance_iforce=8.0mA@TSVDC_TOP1_P_FH  ', 'Ohm', '', '')) 4), 'probe').Rows[0]
Check '같은 헤더를 다른 공정(COWET, 출처 SPEC)에 따로 등록' ([int]$regSpec['REGISTERED'] -eq 1 -and @($bsl.GetEtParameterList($Factory, $EtDevice, 'COWET').Rows).Count -eq 1) "COWET $($regSpec['REGISTERED'])"

# 2-2. [등록 Parameter] 탭 저장(SaveEtParameterSetup): 수정된 행만 / 사용 · 비고 · GDS Chain · Spec 같이 / 틀리면 전체 거부
$TR = 'RESISTANCE_IFORCE_8P0MA_TSVDC_TR_FH'; $RN = 'RESISTANCE_IFORCE_8P0MA_TSVDC_R_N_FH'; $LP = 'RESISTANCE_IFORCE_8P0MA_TSVDC_L_P_FH'
$sv = SaveSetup 'TSVET' @(@($TR, 'N', '시험 비고', ' tsvdc_tr_fh ', '', '715', 'Y'), @($RN, 'Y', '', '', '', '', 'N')) 'probe2'
$st = $bsl.GetEtParameterSetupList($Factory, $EtDevice, 'TSVET')
$tr0 = @($st.Rows | Where-Object { [string]$_['ITEM_NAME'] -eq $TR })[0]
$by2 = @($st.Rows | Where-Object { [string]$_['UPDATE_USER'] -eq 'probe2' }).Count
Check '저장: 2행만 (사용 N / 비고 / GDS Chain 앞뒤 공백 제거 / Spec 새로 저장, 나머지 35행 그대로)' ([int]$sv['UPDATED'] -eq 2 -and [int]$sv['SPEC_SAVED'] -eq 1 -and [int]$sv['SPEC_DELETED'] -eq 0 -and [string]$tr0['USE_FLAG'] -eq 'N' -and [string]$tr0['REMARK'] -eq '시험 비고' -and [string]$tr0['GDS_CHAIN_NAME'] -eq 'tsvdc_tr_fh' -and [string]$tr0['HAS_SPEC'] -eq 'Y' -and [double]$tr0['UPPER_LIMIT'] -eq 715 -and [string]$tr0['UNIT'] -eq 'Ohm' -and $by2 -eq 2 -and $st.Rows.Count -eq 37) "UPDATED $($sv['UPDATED']) / SPEC $($sv['SPEC_SAVED']) / probe2 행 $by2 / GDS '$($tr0['GDS_CHAIN_NAME'])'"
Check '목록 열: Spec 원문 + 출처 / 등록 파일, 값 통계 열 없음(설정 화면에서 쓰지 않음)' ($st.Columns.Contains('SPEC_TEXT') -and $st.Columns.Contains('REG_FILE_NAME') -and $st.Columns.Contains('GDS_CHAIN_NAME') -and -not $st.Columns.Contains('FILE_COUNT') -and -not $st.Columns.Contains('FAIL_COUNT')) "$($st.Columns.Count)열"
$e5 = Throws { SaveSetup 'TSVET' @(@($LP, 'Y', '', '', '800', '700', 'Y'), @('NOT_REGISTERED_NAME', 'Y', '', '', '', '', 'N'), @($RN, 'Y', '', '', '', '', 'Y'), @($TR, 'X', '', '', '', '', 'N')) 'probe' }
$lp0 = SetupRow 'TSVET' $LP
Check '틀린 행이 있으면 아무것도 저장 안 함 (Lower > Upper / 등록 안 됨 / Enable 인데 Limit 없음 / 사용 값)' ($e5 -match '4건' -and $e5 -match '보다 큽니다' -and $e5 -match '등록되지 않은' -and $e5 -match 'Enable 인데' -and [string]$lp0['HAS_SPEC'] -eq 'N') $e5
[void]$bsl.SaveEtLimit([string[]]@($Factory, $EtDevice, 'TSVET'), (Rows @(,@('Resistance_iforce=8.0mA@TSVDC_TR_FH', '', '715', 'Y', 'Ohm', 'If val > 715, Fail', '표 비고')) 7), 'probe')
$sv = SaveSetup 'TSVET' @(,@($TR, 'Y', '', 'tsvdc_tr_fh', '', '720', 'N')) 'probe'
$tr0 = SetupRow 'TSVET' $TR
$lim0 = @($bsl.GetEtLimit($Factory, $EtDevice, 'TSVET').Rows | Where-Object { [string]$_['ITEM_NAME'] -eq $TR })[0]
Check 'Spec 수정: Upper 720 / Enable N, Spec 원문 · Spec 비고는 그대로' ([int]$sv['SPEC_SAVED'] -eq 1 -and [double]$tr0['UPPER_LIMIT'] -eq 720 -and [string]$tr0['ENABLE_FLAG'] -eq 'N' -and [string]$tr0['SPEC_TEXT'] -eq 'If val > 715, Fail' -and [string]$lim0['REMARK'] -eq '표 비고') "$($tr0['UPPER_LIMIT']) / $($tr0['ENABLE_FLAG']) / $($tr0['SPEC_TEXT'])"
$sv = SaveSetup 'TSVET' @(,@($TR, 'Y', '', 'TSVDC_TR_FL', '', '720.0', 'N')) 'probe3'
$tr0 = SetupRow 'TSVET' $TR
Check 'Spec 값이 같으면(720 = 720.0 / Enable N) Spec 은 다시 쓰지 않음 - GDS Chain 만 바뀜' ([int]$sv['UPDATED'] -eq 1 -and [int]$sv['SPEC_SAVED'] -eq 0 -and [int]$sv['SPEC_DELETED'] -eq 0 -and [string]$tr0['GDS_CHAIN_NAME'] -eq 'TSVDC_TR_FL' -and [double]$tr0['UPPER_LIMIT'] -eq 720) "UPDATED $($sv['UPDATED']) / SPEC $($sv['SPEC_SAVED']) / GDS $($tr0['GDS_CHAIN_NAME'])"
$sv = SaveSetup 'TSVET' @(,@($TR, 'Y', '', 'tsvdc_tr_fh', '', '', 'N')) 'probe'
$tr0 = SetupRow 'TSVET' $TR
Check 'Lower / Upper 모두 비움 -> Spec 삭제 (Parameter / GDS Chain 은 남음)' ([int]$sv['SPEC_DELETED'] -eq 1 -and [string]$tr0['HAS_SPEC'] -eq 'N' -and [string]$tr0['GDS_CHAIN_NAME'] -eq 'tsvdc_tr_fh') "삭제 $($sv['SPEC_DELETED'])"
[void]$bsl.SaveEtLimit([string[]]@($Factory, $EtDevice, 'TSVET'), (Rows @(,@('Resistance_iforce=8.0mA@TSVDC_TR_FH', '', '715', 'Y', 'Ohm', 'If val > 715, Fail', '')) 7), 'probe')
$limKey = @($bsl.GetEtLimit($Factory, $EtDevice, 'TSVET').Rows | ForEach-Object { [string]$_['ITEM_NAME'] })
$e9 = Throws { $bsl.SaveEtLimit([string[]]@($Factory, $EtDevice, 'TSVET'), (Rows @(,@('Resistance_iforce=8.0mA@ZZ_NOT_REG_FH', '', '1', 'Y', '', '', '')) 7), 'probe') }
$fk = Throws { (New-Object DACrux.SEMDMS.DSL.TQP_GDS_ET_LIMIT).InsertExecuteMultiple('MERGE_ET_LIMIT', (Rows @(,@($Factory, $EtDevice, 'TSVET', 'ZZ_NOT_REGISTERED', '', '1', '', 'Y', '', '', 'probe')) 11)) }
Check '헤더 원문으로 Spec 저장(붙여넣기 경로) -> 저장 이름 키 / 등록 안 된 Parameter: 서비스 거부 · DB FK 거부(ORA-02291)' (($limKey -join ',') -eq $TR -and $e9 -match '등록되지 않은' -and $fk -match 'ORA-02291') "$($limKey -join ',') | $e9 | $fk"

# 2-3. GDS Chain 참고 목록 / GDS Device(E-Test Device 와 같은 이름) / 인계 API(GDS Chain 이름 -> 모든 공정 Parameter + Spec)
$sTr = New-ProbeChain 'ZZETP1' 'TSVDC_TR_FH'
$gl = $bsl.GetEtGdsChainList($Factory, $GdsDevice)
$gTr = @($gl.Rows | Where-Object { [string]$_['CHAIN_SEQ'] -eq $sTr })[0]
Check 'GDS Device = 같은 이름 READY Map(대소문자 무시, 없으면 그대로) / GDS Setup Chain 목록(Code / 이름)' ($bsl.GetEtGdsDevice($Factory, $EtDevice.ToLowerInvariant()) -eq $GdsDevice -and $bsl.GetEtGdsDevice($Factory, 'ZZ_NO_DEVICE') -eq 'ZZ_NO_DEVICE' -and $gTr -ne $null -and [string]$gTr['CHAIN_CODE'] -eq 'ZZETP1' -and [string]$gTr['CHAIN_NAME'] -eq 'TSVDC_TR_FH') "GDS Device $($bsl.GetEtGdsDevice($Factory, $EtDevice)) / 목록 $($gl.Rows.Count)"
$cowetName = [string]@($bsl.GetEtParameterList($Factory, $EtDevice, 'COWET').Rows)[0]['ITEM_NAME']
[void]$bsl.SaveEtParameterSetup([string[]]@($Factory, $EtDevice, 'COWET'), (Rows @(,@($cowetName, 'Y', '', 'TSVDC_TR_FH', '', '', 'N')) 7), 'probe')
$ci = $bsl.GetEtChainSpecInfo($Factory, $EtDevice, 'TSVDC_TR_FH')
$ciGds = $bsl.GetEtChainSpecInfo($Factory, $GdsDevice.ToLowerInvariant(), 'tsvdc_tr_fh')
$ciNone = $bsl.GetEtChainSpecInfo($Factory, $EtDevice, 'ZZ_NO_SUCH_CHAIN')
$ciTr = @($ci.Rows | Where-Object { [string]$_['ITEM_NAME'] -eq $TR })[0]
Check '인계 API: GDS Chain 이름(대소문자 무시) -> TSVET + COWET Parameter + Spec / Device 소문자로 불러도 같음 / 없는 이름 0행(열 있음)' ($ci.Rows.Count -eq 2 -and (@($ci.Rows | ForEach-Object { [string]$_['TEST_PROGRAM'] }) -join ',') -eq 'COWET,TSVET' -and [string]$ciTr['HAS_SPEC'] -eq 'Y' -and $ciGds.Rows.Count -eq 2 -and $ciNone.Rows.Count -eq 0 -and $ciNone.Columns.Contains('GDS_CHAIN_NAME') -and -not $ci.Columns.Contains('REP_CHAIN_NAME')) "$($ci.Rows.Count)행 / GDS Device $($ciGds.Rows.Count)행 / 없는 이름 $($ciNone.Rows.Count)행"
$r0 = @($bsl.GetEtParameterList($Factory, $EtDevice, 'TSVET').Rows | Where-Object { [string]$_['ITEM_NAME'] -eq $TR })[0]
Check 'Parameter 목록(업로드 계획용)에 GDS Chain / 대표 Chain 열 없음' ([string]$r0['GDS_CHAIN_NAME'] -eq 'tsvdc_tr_fh' -and -not $r0.Table.Columns.Contains('REP_CHAIN_NAME') -and -not $r0.Table.Columns.Contains('GDS_CHAIN_NAME2')) "$($r0['GDS_CHAIN_NAME'])"

# Parameter 삭제: Spec 같이 삭제 / 없는 이름 거부(아무것도 지우지 않음)
$e8 = Throws { $bsl.DeleteEtParameters([string[]]@($Factory, $EtDevice, 'TSVET'), [string[]]@('RESISTANCE_IFORCE_8P0MA_TSVDC_TR_FH', 'NOT_REGISTERED_NAME'), 'probe') }
$still = @($bsl.GetEtParameterList($Factory, $EtDevice, 'TSVET').Rows).Count
$nDel = $bsl.DeleteEtParameters([string[]]@($Factory, $EtDevice, 'TSVET'), [string[]]@('RESISTANCE_IFORCE_8P0MA_TSVDC_TR_FH'), 'probe')
$lim = @($bsl.GetEtLimit($Factory, $EtDevice, 'TSVET').Rows).Count
Check 'Parameter 삭제: 없는 이름이 섞이면 전체 거부 / 삭제 시 Spec 같이 삭제' ($e8 -match '등록되지' -and $still -eq 37 -and $nDel -eq 1 -and $lim -eq 0 -and @($bsl.GetEtParameterList($Factory, $EtDevice, 'TSVET').Rows).Count -eq 36) "거부 후 $still / 삭제 $nDel / Spec $lim"

if (-not $Keep) {
    Remove-ProbeData
    $left = @($bsl.GetEtParameterList($Factory, $EtDevice, 'TSVET').Rows).Count + @($bsl.GetEtParameterList($Factory, $EtDevice, 'COWET').Rows).Count + @($chain.GetChainList($Factory, $GdsDevice).Rows | Where-Object { [string]$_['CHAIN_CODE'] -like 'ZZETP*' }).Count + $import.GetMapInfo($Factory, $GdsDevice).Rows.Count
    Check '시험 데이터 정리' ($left -eq 0) "남은 행 $left"
}

$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
Write-Host ''
Write-Host ("ET_PARA_SETUP_PROBE=" + $(if ($fail -eq 0) { 'PASS' } else { 'FAIL' }) + " ($($results.Count - $fail)/$($results.Count))")
if ($fail -gt 0) { exit 1 }
