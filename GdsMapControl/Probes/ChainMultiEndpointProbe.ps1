# 여러 Input/Output 중 실제 연결된 단자 쌍을 찾고 단일 지정 호출도 유지하는지 검사한다.
$ErrorActionPreference = 'Stop'
$exePath = if ($env:CHAIN_PROBE_EXE) { $env:CHAIN_PROBE_EXE }
    else { Join-Path $PSScriptRoot '..\bin\Debug\NexplantQMS.GdsMap.exe' }
[void][System.Reflection.Assembly]::LoadFrom((Resolve-Path $exePath).Path)

function New-Element([string]$key, [int]$layer, [double]$left, [double]$right) {
    $points = [NexplantQMS.GdsMap.GPoint[]]@(
        [NexplantQMS.GdsMap.GPoint]::new($left, 0),
        [NexplantQMS.GdsMap.GPoint]::new($right, 0),
        [NexplantQMS.GdsMap.GPoint]::new($right, 2),
        [NexplantQMS.GdsMap.GPoint]::new($left, 2))
    return [NexplantQMS.GdsMap.Chain.ChainTraceElement]::new(
        $key, $layer, [NexplantQMS.GdsMap.GBox]::new($left, 0, $right, 2),
        'BOUNDARY', $points, 0.0)
}

$elements = [NexplantQMS.GdsMap.Chain.ChainTraceElement[]]@(
    (New-Element 'A' 2 0 2),
    (New-Element 'B' 1 10 12),
    (New-Element 'C' 1 11 15),
    (New-Element 'D' 1 14 16),
    (New-Element 'E' 1 30 32))
$rules = [NexplantQMS.GdsMap.Chain.ChainLayerConnectionRule[]]@()
$tracer = [NexplantQMS.GdsMap.Chain.ChainCandidateTracer]::new()

$request = [NexplantQMS.GdsMap.Chain.ChainTraceRequest]::new(
    $elements, [string[]]@('A', 'B'), [string[]]@('D', 'E'), $rules, 5.0)
$request.ApplyLayerRules = $false
$result = $tracer.Trace($request)
$path = ($result.Path | ForEach-Object ElementKey) -join ','
if (-not $result.IsConnected -or $path -ne 'B,C,D') {
    throw "복수 단자 경로가 예상과 다릅니다: $path"
}
$endpoints = (($result.EndpointElements | ForEach-Object ElementKey) | Sort-Object) -join ','
$offPath = (($result.OffPathEndpointElements | ForEach-Object ElementKey) | Sort-Object) -join ','
$candidates = (($result.CandidateElements | ForEach-Object ElementKey) | Sort-Object) -join ','
if ($endpoints -ne 'A,B,D,E' -or $offPath -ne 'A,E' -or $candidates -ne 'A,B,C,D,E') {
    throw "경로 밖 Input/Output이 후보에서 빠졌습니다: endpoints=$endpoints offPath=$offPath candidates=$candidates"
}

$disconnected = [NexplantQMS.GdsMap.Chain.ChainTraceRequest]::new(
    $elements, 'A', 'E', $rules, 5.0)
$disconnected.ApplyLayerRules = $false
$disconnectedResult = $tracer.Trace($disconnected)
$disconnectedCandidates = (($disconnectedResult.CandidateElements | ForEach-Object ElementKey) | Sort-Object) -join ','
if ($disconnectedResult.IsConnected -or $disconnectedCandidates -ne 'A,E') {
    throw "경로 실패 시 지정 단자가 결과에서 빠졌습니다: $disconnectedCandidates"
}

$request.SelectedLayerIds = [System.Collections.Generic.HashSet[int]]::new()
[void]$request.SelectedLayerIds.Add(1)
$missing = $tracer.Trace($request)
if ($missing.IsConnected -or $missing.Message -notlike '*Input Element 일부*') {
    throw '필터에서 빠진 Input을 무시하고 경로를 계산했습니다.'
}

$legacy = [NexplantQMS.GdsMap.Chain.ChainTraceRequest]::new(
    $elements, 'B', 'D', $rules, 5.0)
$legacy.ApplyLayerRules = $false
if (-not $tracer.Trace($legacy).IsConnected) {
    throw '기존 단일 Input/Output 탐색이 실패했습니다.'
}

Write-Output 'CHAIN_MULTI_ENDPOINT_PROBE=PASS'
