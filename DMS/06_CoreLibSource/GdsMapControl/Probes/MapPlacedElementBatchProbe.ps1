# Service 연결 전에 도형/바이트 제한과 좌표 복사를 확인하는 독립 실행 검사다.
$ErrorActionPreference = 'Stop'
$assemblyPath = Join-Path $PSScriptRoot '..\bin\Debug\NexplantQMS.GdsMap.exe'
[void][System.Reflection.Assembly]::LoadFrom((Resolve-Path $assemblyPath).Path)

function New-Element([string]$id) {
    $element = [NexplantQMS.GdsMap.Persistence.MapPlacedElementData]::new()
    $element.PlacedElementId = $id
    $element.SourceElementId = 'TOP/E1'
    $element.ElementType = 'PATH'
    $element.WorldPoints = [NexplantQMS.GdsMap.GPoint[]]@(
        [NexplantQMS.GdsMap.GPoint]::new(1, 2),
        [NexplantQMS.GdsMap.GPoint]::new(3, 4))
    return $element
}

function Assert-True([bool]$condition, [string]$message) {
    if (-not $condition) { throw $message }
}

$first = New-Element 'R0/E1'
$second = New-Element 'R0/E2'
$third = New-Element 'R0/E3'
$builder = [NexplantQMS.GdsMap.Persistence.MapPlacedElementBatchBuilder]::new(2, 100000L)
Assert-True ($builder.TryAdd($first)) '첫 도형을 추가하지 못했습니다.'
Assert-True ($builder.TryAdd($second)) '두 번째 도형을 추가하지 못했습니다.'
Assert-True (-not $builder.TryAdd($third)) '도형 수 상한을 넘었습니다.'
$batch = $builder.TakeBatch(0)
Assert-True ($batch.Elements.Count -eq 2 -and $batch.PointCount -eq 4) '배치 건수/좌표 수가 다릅니다.'
$first.WorldPoints[0] = [NexplantQMS.GdsMap.GPoint]::new(99, 99)
Assert-True ($batch.Elements[0].WorldPoints[0].X -eq 1) '배치가 Map 좌표 배열을 공유합니다.'
Assert-True ($builder.TryAdd($third)) '배치를 비운 뒤 다음 도형을 추가하지 못했습니다.'

$size = [NexplantQMS.GdsMap.Persistence.MapPlacedElementBatchBuilder]::EstimateBytes($third)
$byteBuilder = [NexplantQMS.GdsMap.Persistence.MapPlacedElementBatchBuilder]::new(10, $size)
Assert-True ($byteBuilder.TryAdd($third)) '단일 도형의 정확한 크기 상한을 처리하지 못했습니다.'
Assert-True (-not $byteBuilder.TryAdd($second)) '바이트 수 상한을 넘었습니다.'
$oversize = [NexplantQMS.GdsMap.Persistence.MapPlacedElementBatchBuilder]::new(10, ($size - 1))
$rejected = $false
try { [void]$oversize.TryAdd($third) } catch [System.ArgumentException] { $rejected = $true }
Assert-True $rejected '한 도형이 상한을 넘을 때 명시적으로 거부하지 않았습니다.'

'MAP_PLACED_ELEMENT_BATCH_PROBE=PASS'
