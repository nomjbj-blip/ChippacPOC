# 연관 분석 0단계 확인: Chain Member 도형 통계 (설계: 문서/설계/05_연관분석/2026-10-15_GDS_KLARF_ETest_통합분석_데이터_설계.md 4.5 / 4.6절)
# 목적: 서버 Cache 크기와 실제 모양 판정이 필요한 비율을 미리 본다.
#  - 도형 종류(BOUNDARY / PATH / 그 밖) 비율, PATH 폭 분포, PATH_TYPE 분포, Member 당 점 수, 좌표 바이트 합계
#  - 같은 도형이 여러 Chain 에 들어 있는 수(Member 행 수 vs 고유 도형 수)
#  - 조회 시간(SELECT_CHAIN_MEMBER_SHAPE_ALL 한 번)
# DB 는 조회만 한다. 접속은 BSL 과 같은 DSL -> Miracom.Middleware 경로다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsKlarfShapeStatsProbe.ps1 [-BinDir <Interface / DSL DLL 폴더>] [-Factory SCK] [-Device OMM_Hybrid]
param(
    [string]$BinDir = '',
    [string]$Factory = 'SCK',
    [string]$Device = 'OMM_Hybrid'
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$serverBin = Join-Path $dms '101_ServerBin'
if ($BinDir -eq '') { $BinDir = $serverBin }

[AppDomain]::CurrentDomain.SetData('APP_CONFIG_FILE', (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config'))
Add-Type -AssemblyName System.Configuration
$flags = [Reflection.BindingFlags]'NonPublic,Static'
$cm = [Configuration.ConfigurationManager]
$cm.GetField('s_initState', $flags).SetValue($null, 0)
$cm.GetField('s_configSystem', $flags).SetValue($null, $null)
foreach ($d in 'Oracle.ManagedDataAccess', 'Miracom.Middleware', 'DACrux.Base') {
    $p = Join-Path $serverBin "$d.dll"; if (Test-Path $p) { [void][Reflection.Assembly]::LoadFrom($p) }
}
foreach ($d in 'DACrux.SEMDMS.Interface', 'DACrux.SEMDMS.DSL') {
    [void][Reflection.Assembly]::LoadFrom((Join-Path $BinDir "$d.dll"))
}

$watch = [Diagnostics.Stopwatch]::StartNew()
$table = (New-Object DACrux.SEMDMS.DSL.TQP_GDS_CHAIN_MEMBER).GetData('SELECT_CHAIN_MEMBER_SHAPE_ALL', $null, [string[]]@($Factory, $Device))
$seconds = $watch.Elapsed.TotalSeconds
if ($table.Rows.Count -eq 0) { throw "Chain Member 가 없습니다: $Factory / $Device" }

$types = @{}; $pathTypes = @{}; $widths = New-Object Collections.Generic.List[double]; $points = New-Object Collections.Generic.List[int]
$unique = @{}; $bytes = 0L; $closed = 0; $chains = @{}
foreach ($r in $table.Rows) {
    $t = [string]$r['ELEMENT_TYPE']; $types[$t] = 1 + [int]$types[$t]
    $chains[[string]$r['CHAIN_SEQ']] = 1
    $id = [string]$r['PLACED_ELEMENT_ID']
    if ($unique.ContainsKey($id)) { continue }
    $unique[$id] = 1
    $n = [int]$r['POINT_COUNT']; $points.Add($n)
    $bin = $r['POINTS_BIN']; if ($bin -is [byte[]]) { $bytes += $bin.Length }
    if ([string]$r['CLOSED_FLAG'] -eq '1') { $closed++ }
    if ($t -eq 'PATH') {
        $pt = [string]$r['PATH_TYPE']; $pathTypes[$pt] = 1 + [int]$pathTypes[$pt]
        if ($r['PATH_WIDTH'] -ne [DBNull]::Value) { $widths.Add([double]$r['PATH_WIDTH']) }
    }
}
function Quant($list, $q) {
    if ($list.Count -eq 0) { return '-' }
    $a = $list.ToArray(); [Array]::Sort($a); return $a[[Math]::Min($a.Length - 1, [int][Math]::Floor($q * ($a.Length - 1)))]
}
$memberRows = $table.Rows.Count
$uniqueCount = $unique.Count
$avgPoints = ($points | Measure-Object -Average).Average
# Cache 크기 예상: 점 16 B + 도형당 고정 약 96 B(Bounds / 폭 / 종류 / ID 참조)
$cacheMb = ($bytes + 96L * $uniqueCount) / 1MB

Write-Host ("Device {0} / {1}: Member 행 {2:N0} / 고유 도형 {3:N0} / Chain {4} / 조회 {5:0.0}초" -f $Factory, $Device, $memberRows, $uniqueCount, $chains.Count, $seconds)
Write-Host ("도형 종류(Member 행 기준): " + (($types.GetEnumerator() | Sort-Object Name | ForEach-Object { "$($_.Name) $($_.Value)" }) -join ' / '))
Write-Host ("PATH_TYPE(고유 PATH): " + (($pathTypes.GetEnumerator() | Sort-Object Name | ForEach-Object { "Type $($_.Name) $($_.Value)" }) -join ' / '))
Write-Host ("PATH 폭 µm: 최소 {0} / 중앙 {1} / 90% {2} / 최대 {3} (고유 PATH {4})" -f (Quant $widths 0), (Quant $widths 0.5), (Quant $widths 0.9), (Quant $widths 1), $widths.Count)
Write-Host ("점 수(고유 도형): 평균 {0:0.0} / 중앙 {1} / 90% {2} / 최대 {3} / 닫힌 도형 {4}" -f $avgPoints, (Quant $points 0.5), (Quant $points 0.9), (Quant $points 1), $closed)
Write-Host ("좌표 바이트 {0:N0} -> Cache 예상 약 {1:0.0} MB" -f $bytes, $cacheMb)
Write-Host ("GDS_KLARF_SHAPE_STATS=PASS (member={0} unique={1} cacheMB={2:0.0} sec={3:0.0})" -f $memberRows, $uniqueCount, $cacheMb, $seconds)
