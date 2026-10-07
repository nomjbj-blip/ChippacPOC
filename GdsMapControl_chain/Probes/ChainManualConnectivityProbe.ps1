# 수동 제외로 중간 연결이 끊기거나 떨어진 객체가 추가될 때 연결 검증 결과를 검사한다.
$ErrorActionPreference = 'Stop'
$exePath = if ($env:CHAIN_PROBE_EXE) { $env:CHAIN_PROBE_EXE }
    else { Join-Path $PSScriptRoot '..\bin\EndpointQA\NexplantQMS.GdsMap.exe' }
[void][System.Reflection.Assembly]::LoadFrom((Resolve-Path $exePath).Path)

function New-Element([string]$key, [double]$left, [double]$bottom,
    [double]$right, [double]$top) {
    $points = [NexplantQMS.GdsMap.GPoint[]]@(
        [NexplantQMS.GdsMap.GPoint]::new($left, $bottom),
        [NexplantQMS.GdsMap.GPoint]::new($right, $bottom),
        [NexplantQMS.GdsMap.GPoint]::new($right, $top),
        [NexplantQMS.GdsMap.GPoint]::new($left, $top))
    return [NexplantQMS.GdsMap.Chain.ChainTraceElement]::new(
        $key, 1, [NexplantQMS.GdsMap.GBox]::new($left, $bottom, $right, $top),
        'BOUNDARY', $points, 0.0)
}

$a = New-Element 'A' 0 0 2 2
$b = New-Element 'B' 2 0 4 2
$c = New-Element 'C' 4 0 6 2
$d = New-Element 'D' 2.5 2 3.5 3
$e = New-Element 'E' 10 0 12 2
$rules = [NexplantQMS.GdsMap.Chain.ChainLayerConnectionRule[]]@()
$tracer = [NexplantQMS.GdsMap.Chain.ChainCandidateTracer]::new()

function Check([object[]]$elements) {
    $request = [NexplantQMS.GdsMap.Chain.ChainTraceRequest]::new(
        [NexplantQMS.GdsMap.Chain.ChainTraceElement[]]$elements,
        [string[]]@('A'), [string[]]@('C'), $rules, 2.0)
    $request.ApplyLayerRules = $false
    return $tracer.CheckSelectionConnectivity($request)
}

$connected = Check @($a, $b, $c, $d)
if (-not $connected.IsConnected -or $connected.ReachedOutputCount -ne 1) {
    throw '전체 후보가 연결된 사례를 단절로 판단했습니다.'
}

$broken = Check @($a, $c, $d)
$brokenKeys = ($broken.DisconnectedKeys | Sort-Object) -join ','
if ($broken.IsConnected -or $broken.ReachedOutputCount -ne 0 -or $brokenKeys -ne 'C,D') {
    throw '중간 객체를 제외한 단절을 찾지 못했습니다.'
}

$isolated = Check @($a, $b, $c, $d, $e)
$isolatedKeys = ($isolated.DisconnectedKeys | Sort-Object) -join ','
if ($isolated.IsConnected -or $isolated.ReachedOutputCount -ne 1 -or $isolatedKeys -ne 'E') {
    throw '떨어진 수동 추가 객체를 찾지 못했습니다.'
}

Write-Output 'CHAIN_MANUAL_CONNECTIVITY_PROBE=PASS'
