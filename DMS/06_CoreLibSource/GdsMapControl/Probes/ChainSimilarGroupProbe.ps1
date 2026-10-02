# 현재 Chain의 평행 이동 묶음만 찾고, 단일 유사 도형과 보호 단자를 구분한다.
$ErrorActionPreference = 'Stop'
$exePath = if ($env:CHAIN_PROBE_EXE) { $env:CHAIN_PROBE_EXE }
    else { Join-Path $PSScriptRoot '..\bin\EndpointQA\NexplantQMS.GdsMap.exe' }
[void][System.Reflection.Assembly]::LoadFrom((Resolve-Path $exePath).Path)

function New-Element([string]$key, [double]$x, [double]$y,
    [string]$kind, [int]$dataType) {
    if ($kind -eq 'PATH') {
        $points = [NexplantQMS.GdsMap.GPoint[]]@(
            [NexplantQMS.GdsMap.GPoint]::new($x, $y),
            [NexplantQMS.GdsMap.GPoint]::new($x + 1, $y + 1),
            [NexplantQMS.GdsMap.GPoint]::new($x + 2, $y))
        $bounds = [NexplantQMS.GdsMap.GBox]::new($x, $y, $x + 2, $y + 1)
        return [NexplantQMS.GdsMap.Chain.ChainTraceElement]::new(
            $key, 5, $bounds, $kind, $points, 0.2, $dataType)
    }
    $points = [NexplantQMS.GdsMap.GPoint[]]@(
        [NexplantQMS.GdsMap.GPoint]::new($x, $y),
        [NexplantQMS.GdsMap.GPoint]::new($x + 1, $y),
        [NexplantQMS.GdsMap.GPoint]::new($x + 1, $y + 1),
        [NexplantQMS.GdsMap.GPoint]::new($x, $y + 1))
    $bounds = [NexplantQMS.GdsMap.GBox]::new($x, $y, $x + 1, $y + 1)
    return [NexplantQMS.GdsMap.Chain.ChainTraceElement]::new(
        $key, 5, $bounds, $kind, $points, 0.0, $dataType)
}

$elements = [NexplantQMS.GdsMap.Chain.ChainTraceElement[]]@(
    (New-Element 'A1' 0 0 'BOUNDARY' 1),
    (New-Element 'A2' 3 0 'BOUNDARY' 1),
    (New-Element 'A3' 1 2 'PATH' 1),
    (New-Element 'B1' 20 0 'BOUNDARY' 1),
    (New-Element 'B2' 23 0 'BOUNDARY' 1),
    (New-Element 'B3' 21 2 'PATH' 1),
    (New-Element 'C1' 40 0 'BOUNDARY' 1),
    (New-Element 'C2' 43 0 'BOUNDARY' 1),
    (New-Element 'C3' 41 2 'PATH' 1),
    (New-Element 'SINGLE' 60 0 'BOUNDARY' 1),
    (New-Element 'OTHER-DATATYPE' 63 0 'BOUNDARY' 2),
    (New-Element 'OUTSIDE-CHAIN' 80 0 'BOUNDARY' 1))
# 같은 Boundary/PATH라도 꼭짓점 시작점이나 진행 방향이 달라질 수 있다.
$reversedBoundary = [NexplantQMS.GdsMap.GPoint[]]@(
    [NexplantQMS.GdsMap.GPoint]::new(21, 1),
    [NexplantQMS.GdsMap.GPoint]::new(21, 0),
    [NexplantQMS.GdsMap.GPoint]::new(20, 0),
    [NexplantQMS.GdsMap.GPoint]::new(20, 1))
$reversedPath = [NexplantQMS.GdsMap.GPoint[]]@(
    [NexplantQMS.GdsMap.GPoint]::new(23, 2),
    [NexplantQMS.GdsMap.GPoint]::new(22, 3),
    [NexplantQMS.GdsMap.GPoint]::new(21, 2))
$elements[3] = [NexplantQMS.GdsMap.Chain.ChainTraceElement]::new('B1', 5,
    [NexplantQMS.GdsMap.GBox]::new(20, 0, 21, 1), 'BOUNDARY', $reversedBoundary, 0.0, 1)
$elements[5] = [NexplantQMS.GdsMap.Chain.ChainTraceElement]::new('B3', 5,
    [NexplantQMS.GdsMap.GBox]::new(21, 2, 23, 3), 'PATH', $reversedPath, 0.2, 1)
$chain = [NexplantQMS.GdsMap.Chain.ChainTraceElement[]]@(
    $elements | Where-Object { $_.ElementKey -ne 'OUTSIDE-CHAIN' })
$finder = [NexplantQMS.GdsMap.Chain.ChainSimilarGroupFinder]::new()
$groups = $finder.Find($chain, [string[]]@('A1', 'A2', 'A3'),
    [string[]]@('C2'), 0.0001, [Threading.CancellationToken]::None)
if ($groups.Count -ne 3) { throw "예상한 세 묶음 대신 $($groups.Count)개를 찾았습니다." }
$identities = @($groups | ForEach-Object { $_.ElementKeys -join ',' } | Sort-Object)
if (($identities -join ';') -ne 'A1,A2,A3;B1,B2,B3;C1,C2,C3') {
    throw "묶음 구성 오류: $($identities -join ';')"
}
if (@($groups | Where-Object IsProtected).Count -ne 1) {
    throw 'Input/Output을 포함한 묶음을 보호하지 못했습니다.'
}
if (@($groups | Where-Object IsAmbiguous).Count -ne 0) {
    throw '서로 겹치지 않는 묶음을 모호한 결과로 분류했습니다.'
}
Write-Output 'CHAIN_SIMILAR_GROUP_PROBE=PASS'
