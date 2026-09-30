# Output 없이 Input에서 연결된 도형을 끝까지 찾고, 기존 Output 탐색은 유지하는지 검사한다.
$ErrorActionPreference = 'Stop'
$exePath = if ($env:CHAIN_PROBE_EXE) { $env:CHAIN_PROBE_EXE }
    else { Join-Path $PSScriptRoot '..\bin\Debug\NexplantQMS.GdsMap.exe' }
[void][System.Reflection.Assembly]::LoadFrom((Resolve-Path $exePath).Path)

function New-Rectangle([string]$key, [int]$layer, [double]$left, [double]$right) {
    $points = [NexplantQMS.GdsMap.GPoint[]]@(
        [NexplantQMS.GdsMap.GPoint]::new($left, 0),
        [NexplantQMS.GdsMap.GPoint]::new($right, 0),
        [NexplantQMS.GdsMap.GPoint]::new($right, 4),
        [NexplantQMS.GdsMap.GPoint]::new($left, 4))
    $bounds = [NexplantQMS.GdsMap.GBox]::new($left, 0, $right, 4)
    return [NexplantQMS.GdsMap.Chain.ChainTraceElement]::new(
        $key, $layer, $bounds, 'BOUNDARY', $points, 0.0)
}

function Assert-Condition([bool]$condition, [string]$message) {
    if (-not $condition) { throw $message }
}

$elements = [NexplantQMS.GdsMap.Chain.ChainTraceElement[]]@(
    (New-Rectangle 'A' 1 0 4),
    (New-Rectangle 'B' 1 3 7),
    (New-Rectangle 'C' 2 6 10),
    (New-Rectangle 'D' 1 30 34))
$rules = [NexplantQMS.GdsMap.Chain.ChainLayerConnectionRule[]]@()
$inputs = [string[]]@('A')
$withoutOutput = [NexplantQMS.GdsMap.Chain.ChainTraceRequest]::new(
    $elements, $inputs, [string[]]@(), $rules, 5.0)
$withoutOutput.ApplyLayerRules = $false
$tracer = [NexplantQMS.GdsMap.Chain.ChainCandidateTracer]::new()
$result = $tracer.Trace($withoutOutput)
Assert-Condition $result.IsOpenEnded 'Output 없는 탐색 결과로 표시하지 않았습니다.'
Assert-Condition (-not $result.IsConnected) 'Output 없이 연결 완료로 표시했습니다.'
Assert-Condition ($result.VisitedElements.Count -eq 3) 'Input 연결 성분 전체를 찾지 못했습니다.'
Assert-Condition ($result.CandidateElements.Count -eq 3) '후보에 연결된 도형 전체가 없습니다.'
Assert-Condition (-not @($result.CandidateElements | Where-Object ElementKey -eq 'D').Count) `
    '떨어진 도형 D를 후보에 포함했습니다.'

$withoutOutput.SelectedLayerIds = [System.Collections.Generic.HashSet[int]]::new()
[void]$withoutOutput.SelectedLayerIds.Add(1)
$filtered = $tracer.Trace($withoutOutput)
Assert-Condition ($filtered.VisitedElements.Count -eq 2) 'Layer 필터를 벗어난 C를 따라갔습니다.'

$withOutput = [NexplantQMS.GdsMap.Chain.ChainTraceRequest]::new(
    $elements, $inputs, [string[]]@('C'), $rules, 5.0)
$withOutput.ApplyLayerRules = $false
$bounded = $tracer.Trace($withOutput)
Assert-Condition ($bounded.IsConnected -and -not $bounded.IsOpenEnded) `
    '기존 Input/Output 경로 탐색이 바뀌었습니다.'
Write-Output 'CHAIN_OPEN_ENDED_PROBE=PASS'
