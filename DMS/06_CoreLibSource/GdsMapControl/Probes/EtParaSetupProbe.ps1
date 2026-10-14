# E-Test Para Setup 서비스 검증 스크립트 (2026-10-14, 설계: 문서/2026-10-14_ETest_Para_Setup_설계.md)
# 목적: 이름 분해 / 변환 규칙(EtParameterNameRule)과 BSL GdsEtResult 의 Parameter 등록 / 대표 Chain / GDS 연결을
#       같은 프로세스에서 직접 호출해 확인한다. DB 접속은 BSL -> DSL -> Miracom.Middleware 경로다(Service Remoting 경유 아님).
# 전제: FACTORY=PROBE / DEVICE_ID=LAYER_FILTER_PROBE 에 READY GDS Map 이 있어야 한다(GdsChainProbe 와 같은 전제, Layer 1 도형 사용).
# 결과: E-Test Device PRB_PARA 에 Parameter 를 등록하고, GDS 시험 Chain(Code ZZETP*) / 대표 Chain / Sub Device 연결
#       (LAYER_FILTER_PROBE <- PRB_PARA)을 만들었다가 -Keep 이 없으면 마지막에 모두 지운다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File EtParaSetupProbe.ps1 [-BinDir <BSL/DSL/Interface DLL 폴더>] [-Keep]
#       -BinDir 생략 시 101_ServerBin. 배포 전에는 TEMP 빌드 폴더를 넘긴다.
param(
    [string]$BinDir = '',
    [string]$Factory = 'PROBE',
    [string]$EtDevice = 'PRB_PARA',
    [string]$GdsDevice = 'LAYER_FILTER_PROBE',
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
Check '분해: Resistance' ($p.IsValid -and $p.ParameterName -eq 'RESISTANCE_IFORCE_8P0MA_TSVDC_TR_FH' -and $p.HeaderText -eq 'Resistance_iforce=8.0mA@TSVDC_TR_FH' -and $p.ParaName -eq 'RESISTANCE_IFORCE' -and $p.CondValue -eq '8.0MA' -and $p.MeasType -eq 'RESISTANCE' -and $p.Condition -eq 'IFORCE=8.0MA' -and $p.Terminal1 -eq 'TSVDC_TR_FH' -and $p.ForceLeg1 -eq 'FH' -and $p.ChainType -eq 'TSVDC') "$($p.ParameterName) / $($p.Condition) / $($p.Terminal1)"
$p = $rule::Parse('  Leakage_Serial@TSVDC_ASIC1_N_FL:TSVDC_ASIC1_P_FH #421 ')
Check '분해: FT Leakage_Serial (#번호 제거 / 단자 2개)' ($p.IsValid -and $p.HeaderText -eq 'Leakage_Serial@TSVDC_ASIC1_N_FL:TSVDC_ASIC1_P_FH' -and $p.ParameterName -eq 'LEAKAGE_SERIAL_TSVDC_ASIC1_N_FL_TSVDC_ASIC1_P_FH' -and $p.Terminal1 -eq 'TSVDC_ASIC1_N_FL' -and $p.Terminal2 -eq 'TSVDC_ASIC1_P_FH' -and $p.ForceLeg1 -eq 'FL' -and $p.ForceLeg2 -eq 'FH' -and $p.Condition -eq 'SERIAL') "$($p.HeaderText) -> $($p.ParameterName)"
$p = $rule::Parse('Leakage_Serial@TSVDC_ASIC_HBM_N_FH:TSVDC_ASIC_HBM_P_FL')
Check '분해: 50자 초과 -> 고객 약어(LKG)' ($p.IsValid -and $p.ParameterName -eq 'LKG_SERIAL_TSVDC_ASIC_HBM_N_FH_TSVDC_ASIC_HBM_P_FL' -and $p.ParameterName.Length -le 50) "$($p.ParameterName) ($($p.ParameterName.Length)자)"
$p = $rule::Parse('Leakage_Serial@PIC_SERP1_M2_FL:PIC_SERP1_M3_FL')
Check '분해: Chain type PIC_SERP' ($p.IsValid -and $p.ChainType -eq 'PIC_SERP' -and $p.Terminal2 -eq 'PIC_SERP1_M3_FL') $p.ChainType
$p = $rule::Parse('Resistance_iforce=8.0mA TSVDC_TR_FH')
$p2 = $rule::Parse('Resistance@TSVDC_TR')
Check '분해: @ 없음 오류 / Force leg 없음 경고' ((-not $p.IsValid) -and $p2.IsValid -and $p2.Warning -ne '') "$($p.Error) / $($p2.Warning)"
Check '이름 변환: 대표 / 단자' ($rule::ToSafeName('tsvdc top1') -eq 'TSVDC_TOP1' -and $rule::ToSafeName(' TSVDC_TR_FH ') -eq 'TSVDC_TR_FH' -and $rule::ToSafeName('a.b@c:d') -eq 'APB_C_D' -and -not $rule::IsSafeName($rule::ToSafeName('1abc'), 64) -and $rule::IsSafeName('TSVDC_TOP1', 64)) "tsvdc top1 -> $($rule::ToSafeName('tsvdc top1'))"
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
foreach ($x in $parts) { [void]$terms.Add($x.Terminal1); if ($x.Terminal2 -ne '') { [void]$terms.Add($x.Terminal2) } }
$bad = @($parts | Where-Object { -not $_.IsValid }).Count
$legless = @($terms | Where-Object { $rule::ForceLegOf($_) -eq '' }).Count
$maxLen = ($parts | ForEach-Object { $_.ParameterName.Length } | Measure-Object -Maximum).Maximum
$abbr = @($parts | Where-Object { $_.ParameterName.StartsWith('LKG_') -or $_.ParameterName.StartsWith('RES_') }).Count
$prob = $rule::FindNameProblems([string[]]@($headers | ForEach-Object { $_[0] }))
Check '샘플 헤더 2,755: 오류 0 / 충돌 0 / 단자 1,306 / Force leg 없음 0 / 최대 50자 / 약어 1' ($headers.Count -eq 2755 -and $bad -eq 0 -and $prob.Length -eq 0 -and $terms.Count -eq 1306 -and $legless -eq 0 -and $maxLen -le 50 -and $abbr -eq 1) "헤더 $($headers.Count) / 오류 $bad / 충돌 $($prob.Length) / 단자 $($terms.Count) / leg 없음 $legless / 최대 $maxLen / 약어 $abbr"

# 고객 Spec 표 119행 (Device / Test Type / Chain name / Parameter / Spec / Unit)
$spec = @(Get-Content -Path $SpecTable -Encoding UTF8 | Select-Object -Skip 1 | Where-Object { $_.Trim() -ne '' } | ForEach-Object { , ($_ -split "`t") })
$specTerms = New-Object 'Collections.Generic.HashSet[string]'
$specBad = 0; $specProb = 0
foreach ($tt in @('TSVET', 'COWET', 'FT')) {
    $hs = @($spec | Where-Object { $_[1].Trim() -eq $tt } | ForEach-Object { $_[2] })
    $specProb += $rule::FindNameProblems([string[]]$hs).Length
}
foreach ($row in $spec) {
    $x = $rule::Parse($row[2]); if (-not $x.IsValid) { $specBad++ ; continue }
    [void]$specTerms.Add($x.Terminal1); if ($x.Terminal2 -ne '') { [void]$specTerms.Add($x.Terminal2) }
}
Check 'Spec 표 119: 오류 0 / 공정별 충돌 0 / 단자 52' ($spec.Count -eq 119 -and $specBad -eq 0 -and $specProb -eq 0 -and $specTerms.Count -eq 52) "행 $($spec.Count) / 오류 $specBad / 충돌 $specProb / 단자 $($specTerms.Count)"

# ---------------------------------------------------------------------------------------------------
# 2. 서비스 (DB)
# ---------------------------------------------------------------------------------------------------
$bsl = New-Object DACrux.SEMDMS.BSL.GdsEtResult
$chain = New-Object DACrux.SEMDMS.BSL.GdsChain
$import = New-Object DACrux.SEMDMS.BSL.GdsMapImport
$info = $import.GetMapInfo($Factory, $GdsDevice)
if ($info.Rows.Count -eq 0 -or $info.Rows[0]['MAP_STATUS'] -ne 'READY') { Write-Host "전제 실패: $Factory / $GdsDevice READY Map 없음 (GdsImportLayerFilterProbe.ps1 -Keep 먼저 실행)"; exit 2 }
$placed = New-Object DACrux.SEMDMS.DSL.TQP_GDS_PLACED_EL
$ids = @($placed.GetData('SELECT_PLACED_EL_PAGE', $null, [string[]]@($Factory, $GdsDevice, '1', ' ', '6')).Rows | ForEach-Object { [string]$_['PLACED_ELEMENT_ID'] })

function Remove-ProbeData {
    foreach ($r in @($bsl.GetEtRepChainList($Factory, $GdsDevice).Rows)) { [void]$bsl.DeleteEtRepChain([string[]]@($Factory, $GdsDevice, [string]$r['REP_CHAIN_NAME']), 'probe') }
    foreach ($r in @($chain.GetChainList($Factory, $GdsDevice).Rows)) { if ([string]$r['CHAIN_CODE'] -like 'ZZETP*') { [void]$chain.DeleteChain($Factory, $GdsDevice, [string]$r['CHAIN_SEQ'], 'probe') } }
    foreach ($t in @('TSVET', 'COWET')) {
        $names = @($bsl.GetEtParameterList($Factory, $EtDevice, $t).Rows | ForEach-Object { [string]$_['ITEM_NAME'] })
        if ($names.Count -gt 0) { [void]$bsl.DeleteEtParameters([string[]]@($Factory, $EtDevice, $t), [string[]]$names, 'probe') }
    }
    [void]$import.SaveSubDeviceList($Factory, $GdsDevice, [string[]]@(), $null, 'probe')
}
function New-ProbeChain($code, $name, $seq = '') {
    $s = [string]$chain.BeginChainSave([string[]]@($Factory, $GdsDevice, $seq, $code, $name, '-16711936', '0', '10', 'probe'), (Rows @(,@('1')) 1), $null).Rows[0]['CHAIN_SEQ']
    [void]$chain.CreateChainMemberBatch([string[]]@($Factory, $GdsDevice, $s), (Rows @(,@($ids[0], 'INPUT', 'ENDPOINT', '0', '0')) 5), 'probe')
    $r = $chain.CompleteChainSave([string[]]@($Factory, $GdsDevice, $s, '1', '0'), 'probe').Rows[0]
    if ($r['RESULT'] -ne 'OK') { throw "GDS 시험 Chain 저장 실패: $code $($r['MESSAGE'])" }
    return $s
}
function RepRow($name) { return @($bsl.GetEtRepChainList($Factory, $GdsDevice).Rows | Where-Object { [string]$_['REP_CHAIN_NAME'] -eq $name })[0] }

Remove-ProbeData
[void]$import.SaveSubDeviceList($Factory, $GdsDevice, [string[]]@($EtDevice), $null, 'probe')

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

# 2-2. 사용 여부 / 비고, 예전 키 Spec 표시, 삭제(Spec 같이)
[void]$bsl.SaveEtParameters([string[]]@($Factory, $EtDevice, 'TSVET'), (Rows @(,@('RESISTANCE_IFORCE_8P0MA_TSVDC_TR_FH', 'N', '시험 비고')) 3), 'probe')
$r0 = @($bsl.GetEtParameterList($Factory, $EtDevice, 'TSVET').Rows | Where-Object { [string]$_['ITEM_NAME'] -eq 'RESISTANCE_IFORCE_8P0MA_TSVDC_TR_FH' })[0]
$e5 = Throws { $bsl.SaveEtParameters([string[]]@($Factory, $EtDevice, 'TSVET'), (Rows @(,@('NOT_REGISTERED_NAME', 'Y', '')) 3), 'probe') }
Check '사용 안 함 / 비고 저장, 없는 Parameter 거부' ([string]$r0['USE_FLAG'] -eq 'N' -and [string]$r0['REMARK'] -eq '시험 비고' -and $e5 -match '등록되지') "$($r0['USE_FLAG']) / $e5"
[void]$bsl.SaveEtParameters([string[]]@($Factory, $EtDevice, 'TSVET'), (Rows @(,@('RESISTANCE_IFORCE_8P0MA_TSVDC_TR_FH', 'Y', '')) 3), 'probe')
[void]$bsl.SaveEtLimit([string[]]@($Factory, $EtDevice, 'TSVET'), (Rows @(,@('Resistance_iforce=8.0mA@TSVDC_TR_FH', '', '715', 'Y', 'Ohm', 'If val > 715, Fail', '')) 7), 'probe')
$r0 = @($bsl.GetEtParameterList($Factory, $EtDevice, 'TSVET').Rows | Where-Object { [string]$_['ITEM_NAME'] -eq 'RESISTANCE_IFORCE_8P0MA_TSVDC_TR_FH' })[0]
Check '예전 키(헤더 원문)로 저장된 Spec 이 Parameter 목록에 보임' ([string]$r0['HAS_SPEC'] -eq 'Y' -and [double]$r0['UPPER_LIMIT'] -eq 715) "Spec $($r0['HAS_SPEC']) / Upper $($r0['UPPER_LIMIT'])"

# 2-3. 대표 Chain / GDS 연결
$sTop1 = New-ProbeChain 'ZZETP1' 'TSVDC_TOP1_N_FL'      # 사용자 예: GDS 는 한쪽 끝 이름(N_FL)으로 등록
$sTr = New-ProbeChain 'ZZETP2' 'TSVDC_TR_FH'
$sAsic = New-ProbeChain 'ZZETP3' 'tsvdc_asic1_p'         # 사람이 고친 이름(대소문자 섞임)
$sDup1 = New-ProbeChain 'ZZETP4' 'TSVDC_DUP_FH'
$sDup2 = New-ProbeChain 'ZZETP5' 'TSVDC_DUP_FH'

$n1 = $bsl.SaveEtRepChain([string[]]@($Factory, $GdsDevice, 'tsvdc top1'), [string[]]@('TSVDC_TOP1_P_FH', 'tsvdc_top1_n_fl'), 'probe')
$top = RepRow 'TSVDC_TOP1'
Check '사용자 예: 단자 P_FH + N_FL 을 대표 TSVDC_TOP1 로 묶음 -> GDS 이름 N_FL 과 이름 연결' ($n1 -eq 2 -and $top -ne $null -and [string]$top['TERMINALS'] -eq 'TSVDC_TOP1_N_FL, TSVDC_TOP1_P_FH' -and [string]$top['LINK_STATUS'] -eq 'NAME' -and [string]$top['CHAIN_SEQ'] -eq $sTop1) "$($top['TERMINALS']) / $($top['LINK_STATUS']) / $($top['GDS_CHAIN_NAME'])"
[void]$bsl.SaveEtRepChain([string[]]@($Factory, $GdsDevice, 'TSVDC_TR_FH'), [string[]]@('TSVDC_TR_FH', 'TSVDC_TR_FL'), 'probe')
[void]$bsl.SaveEtRepChain([string[]]@($Factory, $GdsDevice, 'ASIC1P'), [string[]]@('TSVDC_ASIC1_P_FH', 'TSVDC_ASIC1_P_FL'), 'probe')
[void]$bsl.SaveEtRepChain([string[]]@($Factory, $GdsDevice, 'DUPREP'), [string[]]@('TSVDC_DUP_FH'), 'probe')
[void]$bsl.SaveEtRepChain([string[]]@($Factory, $GdsDevice, 'TSVDC_TOP1_N_FL'), [string[]]@('ZZ_ONLY_FH'), 'probe')
$st = @{}; foreach ($r in @($bsl.GetEtRepChainList($Factory, $GdsDevice).Rows)) { $st[[string]$r['REP_CHAIN_NAME']] = [string]$r['LINK_STATUS'] }
Check '상태: NAME / NONE / MULTIPLE / TAKEN' ($st['TSVDC_TR_FH'] -eq 'NAME' -and $st['ASIC1P'] -eq 'NONE' -and $st['DUPREP'] -eq 'MULTIPLE' -and $st['TSVDC_TOP1_N_FL'] -eq 'TAKEN') (($st.GetEnumerator() | Sort-Object Name | ForEach-Object { "$($_.Name)=$($_.Value)" }) -join ' ')

[void]$bsl.SaveEtChainLink([string[]]@($Factory, $GdsDevice, 'ASIC1P', $sAsic), 'probe')
$asic = RepRow 'ASIC1P'
Check '사용자 연결(MANUAL): 이름이 다른 GDS Chain' ([string]$asic['LINK_STATUS'] -eq 'MANUAL' -and [string]$asic['GDS_CHAIN_NAME'] -eq 'tsvdc_asic1_p') "$($asic['LINK_STATUS']) / $($asic['GDS_CHAIN_NAME'])"
[void]$bsl.SaveEtChainLink([string[]]@($Factory, $GdsDevice, 'DUPREP', $sTr), 'probe')
$st = @{}; foreach ($r in @($bsl.GetEtRepChainList($Factory, $GdsDevice).Rows)) { $st[[string]$r['REP_CHAIN_NAME']] = [string]$r['LINK_STATUS'] }
$e6 = Throws { $bsl.SaveEtChainLink([string[]]@($Factory, $GdsDevice, 'ASIC1P', '999999999'), 'probe') }
Check '1:1: 이미 연결된 GDS Chain 을 다른 대표로 옮김 -> 이전 대표 TAKEN / 없는 GDS Chain 거부' ($st['DUPREP'] -eq 'MANUAL' -and $st['TSVDC_TR_FH'] -eq 'TAKEN' -and $e6 -ne '') "DUPREP=$($st['DUPREP']) TR=$($st['TSVDC_TR_FH']) / $e6"
[void]$bsl.DeleteEtChainLink([string[]]@($Factory, $GdsDevice, 'DUPREP'), 'probe')
$lk = $bsl.LinkEtRepChainsByName($Factory, $GdsDevice, 'probe').Rows[0]
Check '연결 해제 뒤 이름 연결 다시 계산 (TR 다시 연결)' ([string](RepRow 'TSVDC_TR_FH')['LINK_STATUS'] -eq 'NAME' -and [int]$lk['LINKED'] -eq 1) "LINKED $($lk['LINKED']) NONE $($lk['NONE']) MULTIPLE $($lk['MULTIPLE']) TAKEN $($lk['TAKEN'])"

$e7 = Throws { $bsl.RenameEtRepChain([string[]]@($Factory, $GdsDevice, 'TSVDC_TOP1', 'ASIC1P'), 'probe') }
[void]$bsl.RenameEtRepChain([string[]]@($Factory, $GdsDevice, 'TSVDC_TOP1', 'top1 chain'), 'probe')
$t1 = RepRow 'TOP1_CHAIN'
Check '대표 이름 바꾸기(변환 TOP1_CHAIN, 연결 유지) / 같은 이름 거부' ($t1 -ne $null -and [string]$t1['LINK_STATUS'] -eq 'NAME' -and [string]$t1['CHAIN_SEQ'] -eq $sTop1 -and $null -eq (RepRow 'TSVDC_TOP1') -and $e7 -match '이미') "$($t1['LINK_STATUS']) / $e7"

# GDS 쪽 이름 변경: 다른 끝 이름(P_FH)으로 바꾸면 연결 유지 + 저장 이름 갱신, 관계없는 이름이면 NAME_CHANGED -> 다시 계산하면 해제
[void](New-ProbeChain 'ZZETP1' 'TSVDC_TOP1_P_FH' $sTop1)
$t1 = RepRow 'TOP1_CHAIN'; $lk = $bsl.LinkEtRepChainsByName($Factory, $GdsDevice, 'probe').Rows[0]
Check 'GDS 이름을 다른 끝(P_FH)으로 바꿈 -> 연결 유지, 저장 이름 갱신' ([string]$t1['LINK_STATUS'] -eq 'NAME' -and [string]$t1['GDS_CHAIN_NAME'] -eq 'TSVDC_TOP1_P_FH' -and [int]$lk['UPDATED'] -eq 1) "$($t1['LINK_STATUS']) / UPDATED $($lk['UPDATED'])"
[void](New-ProbeChain 'ZZETP1' 'ZZ_OTHER_NAME' $sTop1)
$before = [string](RepRow 'TOP1_CHAIN')['LINK_STATUS']; $lk = $bsl.LinkEtRepChainsByName($Factory, $GdsDevice, 'probe').Rows[0]
$t1 = RepRow 'TOP1_CHAIN'
Check 'GDS 이름이 관계없게 바뀜 -> NAME_CHANGED -> 다시 계산하면 해제(NONE)' ($before -eq 'NAME_CHANGED' -and [int]$lk['REMOVED'] -eq 1 -and [string]$t1['LINK_STATUS'] -eq 'NONE') "$before -> $($t1['LINK_STATUS'])"

# 단자 목록 / 대표 Chain 의 Parameter / Parameter 목록의 대표·GDS 열
$tl = $bsl.GetEtTerminalList($Factory, $GdsDevice)
$tr = @($tl.Rows | Where-Object { [string]$_['TERMINAL_NAME'] -eq 'TSVDC_TR_FH' })[0]
$zz = @($tl.Rows | Where-Object { [string]$_['TERMINAL_NAME'] -eq 'ZZ_ONLY_FH' })[0]
$p1 = @($tl.Rows | Where-Object { [string]$_['TERMINAL_NAME'] -eq 'TSVDC_TOP1_P_FH' })[0]
Check '단자 목록: 등록 단자 + 대표에만 있는 단자(종류 채움) + 공정 합침' ($tr -ne $null -and [string]$tr['REP_CHAIN_NAME'] -eq 'TSVDC_TR_FH' -and [int]$tr['PARA_COUNT'] -ge 1 -and [int]$tr['SPEC_COUNT'] -ge 1 -and $zz -ne $null -and [int]$zz['PARA_COUNT'] -eq 0 -and [string]$zz['FORCE_LEG'] -eq 'FH' -and [string]$p1['TEST_PROGRAMS'] -eq 'COWET') "단자 $($tl.Rows.Count) / TR para=$($tr['PARA_COUNT']) spec=$($tr['SPEC_COUNT']) / TOP1_P_FH 공정=$($p1['TEST_PROGRAMS'])"
$si = $bsl.GetEtRepChainSpecInfo($Factory, $GdsDevice, 'tsvdc_tr_fh')
$siTr = @($si.Rows | Where-Object { [string]$_['ITEM_NAME'] -eq 'RESISTANCE_IFORCE_8P0MA_TSVDC_TR_FH' })[0]
Check '대표 Chain 의 Parameter / Spec' ($si.Rows.Count -ge 1 -and $siTr -ne $null -and [string]$siTr['HAS_SPEC'] -eq 'Y' -and [string]$siTr['MATCH_POSITION'] -eq '1') "$($si.Rows.Count)행"
$r0 = @($bsl.GetEtParameterList($Factory, $EtDevice, 'TSVET').Rows | Where-Object { [string]$_['ITEM_NAME'] -eq 'RESISTANCE_IFORCE_8P0MA_TSVDC_TR_FH' })[0]
Check 'Parameter 목록에 대표 Chain / GDS Chain (Sub Device 로 GDS Device 찾음)' ([string]$r0['REP_CHAIN_NAME'] -eq 'TSVDC_TR_FH' -and [string]$r0['GDS_CHAIN_NAME'] -eq 'TSVDC_TR_FH') "$($r0['REP_CHAIN_NAME']) / $($r0['GDS_CHAIN_NAME'])"

# 단자 빼기 / CASCADE / 대표 삭제
[void]$bsl.RemoveEtRepChainMembers([string[]]@($Factory, $GdsDevice), [string[]]@('TSVDC_DUP_FH'), 'probe')
Check '단자를 모두 빼면 대표가 없어짐' ($null -eq (RepRow 'DUPREP')) ''
[void]$chain.DeleteChain($Factory, $GdsDevice, $sAsic, 'probe')
$asic = RepRow 'ASIC1P'
Check 'GDS Chain 삭제 -> 연결만 CASCADE 삭제, 대표 Chain 남음' ($asic -ne $null -and [string]$asic['CHAIN_SEQ'] -eq '' -and [string]$asic['LINK_STATUS'] -eq 'NONE') "$($asic['LINK_STATUS'])"
$nd = $bsl.DeleteEtRepChain([string[]]@($Factory, $GdsDevice, 'TSVDC_TR_FH'), 'probe')
Check '대표 Chain 삭제(단자 + 연결)' ($nd -eq 2 -and $null -eq (RepRow 'TSVDC_TR_FH')) "지운 단자 $nd"

# Parameter 삭제: Spec 같이 삭제 / 없는 이름 거부(아무것도 지우지 않음)
$e8 = Throws { $bsl.DeleteEtParameters([string[]]@($Factory, $EtDevice, 'TSVET'), [string[]]@('RESISTANCE_IFORCE_8P0MA_TSVDC_TR_FH', 'NOT_REGISTERED_NAME'), 'probe') }
$still = @($bsl.GetEtParameterList($Factory, $EtDevice, 'TSVET').Rows).Count
$nDel = $bsl.DeleteEtParameters([string[]]@($Factory, $EtDevice, 'TSVET'), [string[]]@('RESISTANCE_IFORCE_8P0MA_TSVDC_TR_FH'), 'probe')
$lim = @($bsl.GetEtLimit($Factory, $EtDevice, 'TSVET').Rows).Count
Check 'Parameter 삭제: 없는 이름이 섞이면 전체 거부 / 삭제 시 Spec(예전 키) 같이 삭제' ($e8 -match '등록되지' -and $still -eq 37 -and $nDel -eq 1 -and $lim -eq 0 -and @($bsl.GetEtParameterList($Factory, $EtDevice, 'TSVET').Rows).Count -eq 36) "거부 후 $still / 삭제 $nDel / Spec $lim"

if (-not $Keep) {
    Remove-ProbeData
    $left = @($bsl.GetEtParameterList($Factory, $EtDevice, 'TSVET').Rows).Count + @($bsl.GetEtRepChainList($Factory, $GdsDevice).Rows).Count + @($chain.GetChainList($Factory, $GdsDevice).Rows | Where-Object { [string]$_['CHAIN_CODE'] -like 'ZZETP*' }).Count
    Check '시험 데이터 정리' ($left -eq 0) "남은 행 $left"
}

$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
Write-Host ''
Write-Host ("ET_PARA_SETUP_PROBE=" + $(if ($fail -eq 0) { 'PASS' } else { 'FAIL' }) + " ($($results.Count - $fail)/$($results.Count))")
if ($fail -gt 0) { exit 1 }
