# Input/Output, Layer 필터와 실제 도형 겹침 판정을 검증하는 독립 Probe다.
# 화면을 띄우지 않고 Debug 실행 파일의 Chain 모델을 직접 호출한다.
$ErrorActionPreference = 'Stop'
$exePath = if ($env:CHAIN_PROBE_EXE) { $env:CHAIN_PROBE_EXE }
    else { Join-Path $PSScriptRoot '..\bin\Debug\NexplantQMS.GdsMap.exe' }
[void][System.Reflection.Assembly]::LoadFrom((Resolve-Path $exePath).Path)

function New-Point([double]$x, [double]$y) {
    return [NexplantQMS.GdsMap.GPoint]::new($x, $y)
}

function New-RectangleElement([string]$key, [int]$layer, [double]$left,
    [double]$bottom, [double]$right, [double]$top) {
    $points = [NexplantQMS.GdsMap.GPoint[]]@(
        (New-Point $left $bottom), (New-Point $right $bottom),
        (New-Point $right $top), (New-Point $left $top))
    $box = [NexplantQMS.GdsMap.GBox]::new($left, $bottom, $right, $top)
    return [NexplantQMS.GdsMap.Chain.ChainTraceElement]::new(
        $key, $layer, $box, 'BOUNDARY', $points, 0.0)
}

function Assert-Condition([bool]$condition, [string]$message) {
    if (-not $condition) { throw $message }
}

$a = New-RectangleElement 'A' 1 0 0 4 4
$b = New-RectangleElement 'B' 2 3 0 7 4
$c = New-RectangleElement 'C' 3 6 0 10 4
Assert-Condition ([NexplantQMS.GdsMap.Chain.ChainGeometryOverlap]::Intersects($a, $b)) '겹치는 BOUNDARY를 찾지 못했습니다.'
Assert-Condition (-not [NexplantQMS.GdsMap.Chain.ChainGeometryOverlap]::Intersects($a, $c)) '떨어진 BOUNDARY를 연결했습니다.'

$triangleA = [NexplantQMS.GdsMap.Chain.ChainTraceElement]::new(
    'T1', 1, [NexplantQMS.GdsMap.GBox]::new(0.0, 0.0, 4.0, 4.0), 'BOUNDARY',
    [NexplantQMS.GdsMap.GPoint[]]@((New-Point 0 0), (New-Point 4 0), (New-Point 0 4)), 0.0)
$triangleB = [NexplantQMS.GdsMap.Chain.ChainTraceElement]::new(
    'T2', 2, [NexplantQMS.GdsMap.GBox]::new(2.5, 2.5, 4.0, 4.0), 'BOUNDARY',
    [NexplantQMS.GdsMap.GPoint[]]@((New-Point 4 4), (New-Point 2.5 4), (New-Point 4 2.5)), 0.0)
Assert-Condition (-not [NexplantQMS.GdsMap.Chain.ChainGeometryOverlap]::Intersects($triangleA, $triangleB)) `
    'Bounding Box만 겹치는 삼각형을 잘못 연결했습니다.'

$horizontalPath = [NexplantQMS.GdsMap.Chain.ChainTraceElement]::new(
    'P1', 1, [NexplantQMS.GdsMap.GBox]::new(0.0, -1.0, 10.0, 1.0), 'PATH',
    [NexplantQMS.GdsMap.GPoint[]]@((New-Point 0 0), (New-Point 10 0)), 2.0)
$touchingPath = [NexplantQMS.GdsMap.Chain.ChainTraceElement]::new(
    'P2', 2, [NexplantQMS.GdsMap.GBox]::new(4.0, 1.0, 6.0, 6.0), 'PATH',
    [NexplantQMS.GdsMap.GPoint[]]@((New-Point 5 2), (New-Point 5 5)), 2.0)
$distantPath = [NexplantQMS.GdsMap.Chain.ChainTraceElement]::new(
    'P3', 2, [NexplantQMS.GdsMap.GBox]::new(4.0, 3.0, 6.0, 6.0), 'PATH',
    [NexplantQMS.GdsMap.GPoint[]]@((New-Point 5 4), (New-Point 5 5)), 2.0)
Assert-Condition ([NexplantQMS.GdsMap.Chain.ChainGeometryOverlap]::Intersects($horizontalPath, $touchingPath)) `
    'PATH 폭을 적용한 접촉을 찾지 못했습니다.'
Assert-Condition (-not [NexplantQMS.GdsMap.Chain.ChainGeometryOverlap]::Intersects($horizontalPath, $distantPath)) `
    '떨어진 PATH를 연결했습니다.'
$boundaryTouchingPath = [NexplantQMS.GdsMap.Chain.ChainTraceElement]::new(
    'P4', 2, [NexplantQMS.GdsMap.GBox]::new(4.0, 1.0, 9.0, 3.0), 'PATH',
    [NexplantQMS.GdsMap.GPoint[]]@((New-Point 5 2), (New-Point 8 2)), 2.0)
Assert-Condition ([NexplantQMS.GdsMap.Chain.ChainGeometryOverlap]::Intersects($a, $boundaryTouchingPath)) `
    'BOUNDARY와 PATH 폭의 접촉을 찾지 못했습니다.'

$elements = [NexplantQMS.GdsMap.Chain.ChainTraceElement[]]@($a, $b, $c)
$noRules = [NexplantQMS.GdsMap.Chain.ChainLayerConnectionRule[]]@()
$request = [NexplantQMS.GdsMap.Chain.ChainTraceRequest]::new($elements, 'A', 'C', $noRules, 5.0)
$request.ApplyLayerRules = $false
$request.SelectedLayerIds = [System.Collections.Generic.HashSet[int]]::new()
[void]$request.SelectedLayerIds.Add(1)
[void]$request.SelectedLayerIds.Add(3)
$tracer = [NexplantQMS.GdsMap.Chain.ChainCandidateTracer]::new()
Assert-Condition (-not $tracer.Trace($request).IsConnected) `
    '체크하지 않은 중간 Layer를 통과했습니다.'

[void]$request.SelectedLayerIds.Add(2)
Assert-Condition $tracer.Trace($request).IsConnected '체크한 Layer의 연결 경로를 찾지 못했습니다.'
$request.ApplyLayerRules = $true
$request = [NexplantQMS.GdsMap.Chain.ChainTraceRequest]::new(
    $elements, 'A', 'C',
    [NexplantQMS.GdsMap.Chain.ChainLayerConnectionRule[]]@(
        [NexplantQMS.GdsMap.Chain.ChainLayerConnectionRule]::new(1, 2, 0.0)), 5.0)
$request.SelectedLayerIds = [System.Collections.Generic.HashSet[int]]::new()
[void]$request.SelectedLayerIds.Add(1)
[void]$request.SelectedLayerIds.Add(2)
[void]$request.SelectedLayerIds.Add(3)
Assert-Condition (-not $tracer.Trace($request).IsConnected) `
    'Layer 규칙에 없는 2-3 연결을 통과했습니다.'

$request = [NexplantQMS.GdsMap.Chain.ChainTraceRequest]::new(
    $elements, 'A', 'C',
    [NexplantQMS.GdsMap.Chain.ChainLayerConnectionRule[]]@(
        [NexplantQMS.GdsMap.Chain.ChainLayerConnectionRule]::new(1, 2, 0.0),
        [NexplantQMS.GdsMap.Chain.ChainLayerConnectionRule]::new(2, 3, 0.0)), 5.0)
$request.SelectedLayerIds = [System.Collections.Generic.HashSet[int]]::new()
[void]$request.SelectedLayerIds.Add(1)
[void]$request.SelectedLayerIds.Add(2)
[void]$request.SelectedLayerIds.Add(3)
Assert-Condition $tracer.Trace($request).IsConnected '등록된 Layer 규칙 경로를 찾지 못했습니다.'

$large = New-RectangleElement 'LARGE' 1 0 0 1000 1000
$inside = New-RectangleElement 'INSIDE' 1 995 995 999 999
$largeRequest = [NexplantQMS.GdsMap.Chain.ChainTraceRequest]::new(
    [NexplantQMS.GdsMap.Chain.ChainTraceElement[]]@($large, $inside),
    'LARGE', 'INSIDE', $noRules, 5.0)
$largeRequest.ApplyLayerRules = $false
Assert-Condition $tracer.Trace($largeRequest).IsConnected `
    '큰 도형을 격자 밖에 보관한 뒤 내부 도형을 찾지 못했습니다.'

Write-Output 'CHAIN_LAYER_FILTERED_PROBE=PASS'
