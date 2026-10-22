# E-Test -> YMS 적재 규칙(EtYmsRule) 검증 스크립트 (2026-10-14, 설계: 문서/설계/04_ETest/2026-10-14_ETest_Upload_YMS적재_설계.md 14절)
# 목적: Interface DLL 의 EtYmsRule 을 직접 불러 YMS 컬럼 이름 / Program / Bin / 값 / 수율 규칙을 확인한다. DB 는 쓰지 않는다.
#       샘플 E-Test xlsx 의 측정 헤더 전체와 고객 Spec 표(TSVET)로 이름 겹침 / 길이 초과가 없는지도 본다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File EtYmsRuleProbe.ps1 [-BinDir <DACrux.SEMDMS.Interface.dll 폴더>]
#       -BinDir 생략 시 100_ClientBin. 배포 전에는 TEMP 빌드 폴더를 넘긴다.
# 결과: 마지막 줄 ET_YMS_RULE_PROBE=PASS 또는 FAIL (통과 수 / 전체 수)
param(
    [string]$BinDir = '',
    [string]$SampleFile = '',
    [string]$SpecFile = ''
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$clientBin = Join-Path $dms '100_ClientBin'
if ($BinDir -eq '') { $BinDir = $clientBin }
if ($SampleFile -eq '') { $SampleFile = Join-Path $dms '..\SampleFile\26_10_06 Sample File\CAI_OMM_ATV_A0_TSVET_20260609161940_RH-HS2615_RH-HS2615-02.xlsx' }
if ($SpecFile -eq '') { $SpecFile = Join-Path $dms '02_DMS\DACrux.SEMDMS.ENGUI\Verification\EtSpecTable_U9_20261012.txt' }
[void][Reflection.Assembly]::LoadFrom((Join-Path $BinDir 'DACrux.SEMDMS.Interface.dll'))
foreach ($d in 'DocumentFormat.OpenXml', 'ClosedXML') { [void][Reflection.Assembly]::LoadFrom((Join-Path $clientBin "$d.dll")) }
Write-Host "Interface: $(Join-Path $BinDir 'DACrux.SEMDMS.Interface.dll')"

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
function Throws([scriptblock]$block) {
    try { & $block; return '' } catch { $m = $_.Exception; while ($m.InnerException) { $m = $m.InnerException }; return $m.Message }
}
$R = [DACrux.SEMDMS.Interface.EtYmsRule]

# 1. 컬럼 이름 규칙 (설계 4.3)
$cases = @(
    @('Resistance_iforce=8.0mA@C4DC_28_37_FH', 'RESISTANCE_IFORCE_8P0MA_C4DC_28_37_FH'),
    @('Leakage_Serial@TSVDC_ASIC_HBM_N_FH:TSVDC_ASIC_HBM_P_FL', 'LKG_SERIAL_TSVDC_ASIC_HBM_N_FH_TSVDC_ASIC_HBM_P_FL'),
    @('Leakage_Serial@A_FH:B_FL', 'LEAKAGE_SERIAL_A_FH_B_FL'),
    @('  resistance_iforce=8.0mA@TSVDC_TR_FH  #3 ', 'RESISTANCE_IFORCE_8P0MA_TSVDC_TR_FH'),
    @('Leakage_Parallel@C4DC_138_37_FL', 'LEAKAGE_PARALLEL_C4DC_138_37_FL'),
    @('1st_param', 'P_1ST_PARAM'),
    @('bin', 'P_BIN'),
    @('Die_Num', 'P_DIE_NUM'),
    @('@@ ##', '')
)
foreach ($c in $cases) {
    $got = $R::ToParamName($c[0])
    Check "이름 $($c[0].Trim())" ($got -eq $c[1]) "$got (기대 $($c[1]), $($got.Length)자)"
}
Check '줄인 이름 길이' ($R::ToParamName('Leakage_Serial@TSVDC_ASIC_HBM_N_FH:TSVDC_ASIC_HBM_P_FL').Length -eq 50) '54자 -> LEAKAGE 를 LKG 로 줄여 50자'

# 약어로도 50자 안으로 들지 않으면 41자 + '_' + 해시 8자. 같은 입력은 같은 값, 다른 입력은 다른 값.
$long1 = 'Leakage_Parallel@ABCDEFGHIJKLMNOPQRSTUVWXYZ_ABCDEFGHIJKLMNOPQRST_FH'
$long2 = 'Leakage_Parallel@ABCDEFGHIJKLMNOPQRSTUVWXYZ_ABCDEFGHIJKLMNOPQRST_FL'
$h1 = $R::ToParamName($long1); $h1b = $R::ToParamName($long1); $h2 = $R::ToParamName($long2)
Check '해시 이름' ($h1.Length -le 50 -and $h1 -match '^LKG_PAR_[A-Z0-9_]+_[0-9A-F]{8}$' -and $h1 -eq $h1b -and $h1 -ne $h2) "$h1 / $h2"
Check '해시 이름 사용 가능' ($R::IsValidParamName($h1) -and $R::IsValidParamName($h2)) 'IsValidParamName'

# 2. 이름 검사 (사용 가능 / 겹침)
Check 'IsValidParamName' (($R::IsValidParamName('RES_A1')) -and -not ($R::IsValidParamName('1RES')) -and -not ($R::IsValidParamName('BIN')) -and -not ($R::IsValidParamName('res_a')) -and -not ($R::IsValidParamName('A' * 51)) -and -not ($R::IsValidParamName(''))) '정상 / 숫자 시작 / 예약 / 소문자 / 51자 / 빈 값'
$p1 = $R::FindNameProblems([string[]]@('Res@X-1', 'Res@X_1', 'Res@X-1', 'Res@Y'))
Check '겹침 검사' ($p1.Length -eq 1 -and $p1[0].Contains('RES_X_1')) ([string]::Join(' | ', $p1))
$p2 = $R::FindNameProblems([string[]]@('Res@A', '@@'))
Check '만들 수 없는 이름' ($p2.Length -eq 1) ([string]::Join(' | ', $p2))

# 3. Program / TestArea
Check 'ToProgramName' (($R::ToProgramName('OMM', 'TSVET') -eq 'OMM_TSVET') -and ($R::ToProgramName(' omm-1 ', 'tsvet') -eq 'OMM_1_TSVET') -and ($R::ToProgramName('', 'TSVET') -eq '')) "$($R::ToProgramName('OMM', 'TSVET')) / $($R::ToProgramName(' omm-1 ', 'tsvet'))"
Check 'ToTestArea' ($R::ToTestArea(' tsvet ') -eq 'TSVET') $R::ToTestArea(' tsvet ')

# 4. BIN (2026-10-14: YMS BIN = 결과 파일 Hardware Bin 0 ~ 99, E-Test Spec 판정과 별개)
$bins = @($R::ToBin(1), $R::ToBin(5), $R::ToBin(0), $R::ToBin(99))
Check 'ToBin = Hardware Bin' ([string]::Join(',', $bins) -eq '1,5,0,99') ([string]::Join(',', $bins))
$errs = @((Throws { $R::ToBin($null) }), (Throws { $R::ToBin(100) }), (Throws { $R::ToBin(2001) }), (Throws { $R::ToBin(-1) }))
Check 'ToBin 없음 / 100 이상 / 음수는 오류' (@($errs | Where-Object { $_ -eq '' }).Count -eq 0) ([string]::Join(' | ', $errs))
$names = @($R::BinName(2, [int[]]@(2001)), $R::BinName(2, [int[]]@(2002, 2001, 2001)), $R::BinName(1, [int[]]@()), $R::BinName(3, [int[]]@(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13)))
Check 'Bin 이름 (Software Bin 포함, 30자 이하)' ($names[0] -eq 'HW2 SW2001' -and $names[1] -eq 'HW2 SW2001,2002' -and $names[2] -eq 'HW1' -and $names[3].Length -eq 30) ([string]::Join(' / ', $names))
$descs = @($R::BinDescription(2, [int[]]@(2001), $false, $false), $R::BinDescription(1, [int[]]@(1), $true, $false), $R::BinDescription(4, [int[]]@(4001), $false, $true))
Check 'Bin 설명' ($descs[0] -eq 'E-Test file Hardware Bin 2 / Software Bin 2001 / Fail' -and $descs[1].EndsWith('/ Pass') -and $descs[2].EndsWith('Pass and Fail mixed')) ([string]::Join(' / ', $descs))
Check 'Bin 색 (양품 초록 / 불량은 초록 아님)' ($R::BinColor(1, $true) -eq '#00C000' -and $R::BinColor(1, $false) -ne '#00C000' -and $R::BinColor(2, $false) -ne $R::BinColor(5, $false)) "$($R::BinColor(1, $true)) / $($R::BinColor(2, $false)) / $($R::BinColor(5, $false))"
Check '예전 판정 Bin 정의 구분' ($R::IsLegacyJudgeBin('1', 'PASS', 'E-Test Spec Pass') -and $R::IsLegacyJudgeBin('3', 'NOT_TESTED', 'E-Test Fail by not tested value only') -and -not $R::IsLegacyJudgeBin('3', 'NOT_TESTED', 'user edited') -and -not $R::IsLegacyJudgeBin('2', 'PASS', 'E-Test Spec Pass')) 'BIN / 이름 / 설명 모두 같을 때만'

# 5. 값 (빈 값은 '' -> TD NULL, 숫자는 InvariantCulture R)
$vals = @($R::ToYmsValue(715.0), $R::ToYmsValue(0.1), $R::ToYmsValue(-2.5E-12), $R::ToYmsValue($null), $R::ToYmsValue([double]::NaN))
Check 'ToYmsValue' ($vals[0] -eq '715' -and $vals[1] -eq '0.1' -and $vals[2] -eq '-2.5E-12' -and $vals[3] -eq '' -and $vals[4] -eq '') ([string]::Join(' | ', $vals))

# 6. 예상 수율 (YMS SummaryData 분모 규칙)
$y = @($R::ExpectedYield(23, 24, $null), $R::ExpectedYield(23, 24, 76), $R::ExpectedYield(70, 76, 76), $R::ExpectedYield(55, 60, 76), $R::ExpectedYield(0, 0, 76))
Check 'ExpectedYield' ($y[0] -eq 95.83 -and $y[1] -eq 95.83 -and $y[2] -eq 92.11 -and $y[3] -eq 72.37 -and $y[4] -eq 0) ([string]::Join(' / ', $y))

# 7. 고객 Spec 표 TSVET Parameter (37) : 이름 겹침 / 길이 초과 없음, 줄인 이름 1개
$specNames = New-Object Collections.Generic.List[string]
foreach ($line in [IO.File]::ReadAllLines($SpecFile)) {
    $col = $line.Split("`t")
    if ($col.Length -ge 6 -and $col[1].Trim() -eq 'TSVET') { $specNames.Add($col[2]) }
}
$specProblems = $R::FindNameProblems([string[]]$specNames.ToArray())
$specYms = @($specNames | ForEach-Object { $R::ToParamName($_) })
$maxLen = ($specYms | Measure-Object -Property Length -Maximum).Maximum
$shortened = @($specYms | Where-Object { $_.StartsWith('LKG_') -or $_.StartsWith('RES_') }).Count
Check 'Spec 표 TSVET 이름' ($specNames.Count -eq 37 -and $specProblems.Length -eq 0 -and $maxLen -le 50 -and $shortened -eq 1) "Parameter $($specNames.Count) / 문제 $($specProblems.Length) / 최대 $maxLen 자 / 줄인 이름 $shortened"

# 8. 샘플 E-Test 파일 측정 헤더 전체 : 이름 겹침 / 길이 초과 없음
$fs = New-Object IO.FileStream((Resolve-Path $SampleFile).Path, [IO.FileMode]::Open, [IO.FileAccess]::Read, [IO.FileShare]::ReadWrite)
try {
    $wb = New-Object ClosedXML.Excel.XLWorkbook($fs)
    $ws = $wb.Worksheet('DUT Summary')
    $headers = New-Object Collections.Generic.List[string]
    $lastCol = $ws.LastColumnUsed().ColumnNumber()
    for ($c = 1; $c -le $lastCol; $c++) {
        $h = [string]$ws.Cell(1, $c).Value
        if ($h.Contains('@')) { $headers.Add($h) }
    }
} finally { $fs.Dispose() }
$fileProblems = $R::FindNameProblems([string[]]$headers.ToArray())
$fileYms = @($headers | ForEach-Object { $R::ToParamName($_) })
$fileMax = ($fileYms | Measure-Object -Property Length -Maximum).Maximum
$fileDistinct = @($fileYms | Sort-Object -Unique).Count
Check '샘플 파일 헤더 전체' ($headers.Count -eq 2755 -and $fileProblems.Length -eq 0 -and $fileMax -le 50 -and $fileDistinct -eq 2755) "헤더 $($headers.Count) / 문제 $($fileProblems.Length) / 최대 $fileMax 자 / 서로 다른 이름 $fileDistinct"

# 9. YMS 컬럼 이름 30자 (2026-10-21, 설계: 문서/설계/04_ETest/2026-10-21_ETest_YMS_컬럼이름_30자_설계.md 5절)
$N = [DACrux.SEMDMS.Interface.EtParameterNameRule]
$ymsCases = @(
    @('Resistance_iforce=8.0mA@TSVDC_ASIC_HBM_N_FH', 'RES_8P0MA_TSVDC_ASIC_HBM_N_FH'),
    @('Connectivity_iforce=8.0mA@TSVDC_ASIC_HBM_P_FH', 'CONN_8P0MA_TSVDC_ASIC_HBM_P_FH'),
    @('Connectivity_iforce=1.0mA@PIC_SERP1_M2_FH', 'CONN_1P0MA_PIC_SERP1_M2_FH'),
    @('Resistance_iforce=1.0mA@PIC_EDM_R_FH', 'RES_1P0MA_PIC_EDM_R_FH'),
    @('Leakage_Parallel@TSVDC_ASIC_HBM_N_FH', 'LKG_PAR_TSVDC_ASIC_HBM_N_FH'),
    @('Leakage_Serial@TSVDC_ASIC1_N_FH:TSVDC_ASIC1_P_FL', 'LKG_SER_TSVDC_ASIC1_N_FH_P_FL'),
    @('Leakage_Serial@PIC_SERP1_M2_FL:PIC_SERP1_M3_FL', 'LKG_SER_PIC_SERP1_M2_FL_M3_FL'),
    @('Leakage_Serial@TSVDC_ASIC_HBM_N_FH:TSVDC_ASIC_HBM_P_FL', 'LKG_SER_TSVDC_ASIC_HB_39C6541C'),
    @('Resistance@C4DC_1_FH', 'RESISTANCE_C4DC_1_FH')
)
foreach ($c in $ymsCases) {
    $p = $N::Parse($c[0])
    Check "YMS 이름 $($c[0])" ($p.IsValid -and $p.YmsName -eq $c[1]) "$($p.YmsName) (기대 $($c[1]), $($p.YmsName.Length)자 / Parameter $($p.ParameterName))"
}
$short = $N::Parse('Res@X_1')
Check '30자 이하는 Parameter 이름 그대로' ($short.ParameterName -eq 'RES_X_1' -and $short.YmsName -eq 'RES_X_1') "$($short.ParameterName) -> $($short.YmsName)"
$plain = $R::ToYmsName('RESISTANCE_SOMETHING_VERY_LONG_NAME_WITHOUT_CHAIN_PARTS', '', '', '', '', '')
Check '분해 정보 없음 -> 기존 규칙 30자' ($plain.Length -le 30 -and $R::IsValidYmsName($plain) -and $plain -eq $R::ToYmsName('RESISTANCE_SOMETHING_VERY_LONG_NAME_WITHOUT_CHAIN_PARTS', '', '', '', '', '')) "$plain ($($plain.Length)자)"
Check 'IsValidYmsName' (($R::IsValidYmsName('A' * 30)) -and -not ($R::IsValidYmsName('A' * 31)) -and -not ($R::IsValidYmsName('BIN')) -and -not ($R::IsValidYmsName('1A')) -and -not ($R::IsValidYmsName(''))) '30자 / 31자 / 예약 / 숫자 시작 / 빈 값'
Check 'IsValidProgramName (26자)' (($R::IsValidProgramName('OMM_TSVET')) -and ($R::IsValidProgramName('08EYP1')) -and ($R::IsValidProgramName('A' * 26)) -and -not ($R::IsValidProgramName('A' * 27)) -and -not ($R::IsValidProgramName('omm')) -and $R::MaxProgramLength -eq 26) 'OMM_TSVET / 08EYP1 / 26자 / 27자 / 소문자'
$yp = $R::FindYmsNameProblems([string[]]@('A_ITEM', 'B_ITEM', 'A_ITEM', 'C_ITEM'), [string[]]@('X_1', 'X_1', 'X_1', 'bad'))
Check 'YMS 이름 겹침 / 형식 검사' ($yp.Length -eq 2) ([string]::Join(' | ', $yp))
$hp = $N::FindNameProblems([string[]]@('Leakage_Serial@A_1_N_FH:A_1_P_FL', 'Leakage_Serial@A_1_N_FH:B_1_P_FL'))
Check '헤더 검사: 서로 다른 YMS 이름이면 통과' ($hp.Length -eq 0) ([string]::Join(' | ', $hp))

# 샘플 파일 측정 헤더 전체 / Spec 표: YMS 이름 30자 이하, 겹침 없음
$fileParts = @($headers | ForEach-Object { $N::Parse($_) })
$bad = @($fileParts | Where-Object { -not $_.IsValid })
$fileYmsNames = @($fileParts | Where-Object { $_.IsValid } | ForEach-Object { $_.YmsName })
$ymsMax = ($fileYmsNames | Measure-Object -Property Length -Maximum).Maximum
$ymsDistinct = @($fileYmsNames | Sort-Object -Unique).Count
$ymsHashed = @($fileYmsNames | Where-Object { $_ -match '_[0-9A-F]{8}$' }).Count
$headerProblems = $N::FindNameProblems([string[]]$headers.ToArray())
Check '샘플 파일 헤더 YMS 이름' ($bad.Count -eq 0 -and $ymsMax -le 30 -and $ymsDistinct -eq $headers.Count -and $headerProblems.Length -eq 0) "헤더 $($headers.Count) / 분해 실패 $($bad.Count) / 최대 $ymsMax 자 / 서로 다른 이름 $ymsDistinct / 해시 $ymsHashed / 검사 문제 $($headerProblems.Length)"
$specParts = @($specNames | ForEach-Object { $N::Parse($_) } | Where-Object { $_.IsValid })
$specYmsNames = @($specParts | ForEach-Object { $_.YmsName })
$specYmsMax = ($specYmsNames | Measure-Object -Property Length -Maximum).Maximum
Check 'Spec 표 TSVET YMS 이름' ($specParts.Count -eq 37 -and $specYmsMax -le 30 -and @($specYmsNames | Sort-Object -Unique).Count -eq 37) "Parameter $($specParts.Count) / 최대 $specYmsMax 자 / 해시 $(@($specYmsNames | Where-Object { $_ -match '_[0-9A-F]{8}$' }).Count)"

$pass = @($results | Where-Object { $_.StartsWith('PASS') }).Count
$total = $results.Count
Write-Host ''
Write-Host ("ET_YMS_RULE_PROBE={0} ({1}/{2})" -f $(if ($pass -eq $total) { 'PASS' } else { 'FAIL' }), $pass, $total)
if ($pass -ne $total) { exit 1 }
