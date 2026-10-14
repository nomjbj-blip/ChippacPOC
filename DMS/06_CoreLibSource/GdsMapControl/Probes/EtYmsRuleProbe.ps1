# E-Test -> YMS 적재 규칙(EtYmsRule) 검증 스크립트 (2026-10-14, 설계: 문서/2026-10-14_ETest_Upload_YMS적재_설계.md 14절)
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

# 4. BIN
$bins = @($R::ToBin('P', 0, 0), $R::ToBin('F', 2, 0), $R::ToBin('F', 0, 3), $R::ToBin('F', 1, 1), $R::ToBin(' p ', 0, 0))
Check 'ToBin' ([string]::Join(',', $bins) -eq '1,2,3,2,1') ([string]::Join(',', $bins))
$err = Throws { $R::ToBin('X', 0, 0) }
Check 'ToBin 잘못된 판정' ($err -ne '') $err
Check 'Bin 이름 / 색' (($R::BinName(1) -eq 'PASS') -and ($R::BinName(2) -eq 'SPEC_FAIL') -and ($R::BinName(3) -eq 'NOT_TESTED') -and ($R::BinColor(1) -eq '#00C000')) "$($R::BinName(1)) $($R::BinColor(1)) / $($R::BinName(2)) $($R::BinColor(2)) / $($R::BinName(3)) $($R::BinColor(3))"

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

$pass = @($results | Where-Object { $_.StartsWith('PASS') }).Count
$total = $results.Count
Write-Host ''
Write-Host ("ET_YMS_RULE_PROBE={0} ({1}/{2})" -f $(if ($pass -eq $total) { 'PASS' } else { 'FAIL' }), $pass, $total)
if ($pass -ne $total) { exit 1 }
