# 기본 경로의 직접 겹침, 한 단계 밖의 미포함, 분기 후보와 Layer 필터를 검사한다.
$ErrorActionPreference = 'Stop'
$exePath = if ($env:CHAIN_PROBE_EXE) { $env:CHAIN_PROBE_EXE }
    else { Join-Path $PSScriptRoot '..\bin\Debug\NexplantQMS.GdsMap.exe' }
[void][System.Reflection.Assembly]::LoadFrom((Resolve-Path $exePath).Path)

function New-Element([string]$key, [int]$layer, [double]$left, [double]$bottom,
    [double]$right, [double]$top) {
    $points = [NexplantQMS.GdsMap.GPoint[]]@(
        [NexplantQMS.GdsMap.GPoint]::new($left, $bottom),
        [NexplantQMS.GdsMap.GPoint]::new($right, $bottom),
        [NexplantQMS.GdsMap.GPoint]::new($right, $top),
        [NexplantQMS.GdsMap.GPoint]::new($left, $top))
    return [NexplantQMS.GdsMap.Chain.ChainTraceElement]::new(
        $key, $layer, [NexplantQMS.GdsMap.GBox]::new($left, $bottom, $right, $top),
        'BOUNDARY', $points, 0.0)
}

$elements = [NexplantQMS.GdsMap.Chain.ChainTraceElement[]]@(
    (New-Element 'A' 1 0 0 2 2),
    (New-Element 'B' 1 2 0 4 2),
    (New-Element 'C' 1 4 0 6 2),
    (New-Element 'D' 1 2.1 1.5 2.5 2.5),
    (New-Element 'E' 2 3.5 1.5 3.8 3),
    (New-Element 'F' 2 3.5 2.8 3.8 4))
$rules = [NexplantQMS.GdsMap.Chain.ChainLayerConnectionRule[]]@()
$request = [NexplantQMS.GdsMap.Chain.ChainTraceRequest]::new(
    $elements, 'A', 'C', $rules, 2.0)
$request.ApplyLayerRules = $false
$request.SelectedLayerIds = [System.Collections.Generic.HashSet[int]]::new()
[void]$request.SelectedLayerIds.Add(1)
[void]$request.SelectedLayerIds.Add(2)
$tracer = [NexplantQMS.GdsMap.Chain.ChainCandidateTracer]::new()
$result = $tracer.Trace($request)
$path = ($result.Path | ForEach-Object ElementKey) -join ','
$overlaps = (($result.OverlappingElements | ForEach-Object ElementKey) | Sort-Object) -join ','
$branches = ($result.BranchCandidates | ForEach-Object ElementKey) -join ','
$candidates = (($result.CandidateElements | ForEach-Object ElementKey) | Sort-Object) -join ','
if (-not $result.IsConnected -or $path -ne 'A,B,C' -or $overlaps -ne 'D,E' -or $branches -ne 'E' -or $candidates -ne 'A,B,C,D,E') {
    throw "직접 겹침/분기 결과가 다릅니다: path=$path overlap=$overlaps branch=$branches candidates=$candidates"
}

[void]$request.SelectedLayerIds.Remove(2)
$filtered = $tracer.Trace($request)
$filteredOverlaps = ($filtered.OverlappingElements | ForEach-Object ElementKey) -join ','
if (-not $filtered.IsConnected -or $filteredOverlaps -ne 'D' -or $filtered.BranchCandidates.Count -ne 0) {
    throw "선택 Layer만 남긴 결과가 다릅니다: $filteredOverlaps"
}

# 같은 좌표에 놓인 다른 Element 두 개가 하나로 합쳐지지 않는지 검사한다.
$sameLocationElements = [NexplantQMS.GdsMap.Chain.ChainTraceElement[]]@(
    (New-Element 'A' 1 0 0 2 2),
    (New-Element 'B' 1 2 0 4 2),
    (New-Element 'C' 1 4 0 6 2),
    (New-Element 'H' 1 3 1.5 3.5 2.5),
    (New-Element 'I' 2 3 1.5 3.5 2.5))
$sameLocation = [NexplantQMS.GdsMap.Chain.ChainTraceRequest]::new(
    $sameLocationElements, 'A', 'C', $rules, 2.0)
$sameLocation.ApplyLayerRules = $false
$sameLocation.SelectedLayerIds = [System.Collections.Generic.HashSet[int]]::new()
[void]$sameLocation.SelectedLayerIds.Add(1)
[void]$sameLocation.SelectedLayerIds.Add(2)
$sameResult = $tracer.Trace($sameLocation)
$sameOverlaps = (($sameResult.OverlappingElements | ForEach-Object ElementKey) | Sort-Object) -join ','
if ($sameOverlaps -ne 'H,I') {
    throw "같은 좌표의 Element가 모두 포함되지 않았습니다: $sameOverlaps"
}

Write-Output 'CHAIN_DIRECT_OVERLAP_PROBE=PASS'
