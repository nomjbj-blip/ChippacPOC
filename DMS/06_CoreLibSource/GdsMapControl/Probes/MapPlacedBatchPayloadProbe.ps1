# Service로 보낼 바이너리 본문 형식/재전송 해시/실제 크기 제한을 확인한다.
$ErrorActionPreference = 'Stop'
$assemblyPath = Join-Path $PSScriptRoot '..\bin\Debug\NexplantQMS.GdsMap.exe'
[void][System.Reflection.Assembly]::LoadFrom((Resolve-Path $assemblyPath).Path)

function Assert-True([bool]$condition, [string]$message) {
    if (-not $condition) { throw $message }
}

function Read-Text([System.IO.BinaryReader]$reader) {
    $length = $reader.ReadInt32()
    if ($length -eq -1) { return $null }
    return [Text.Encoding]::UTF8.GetString($reader.ReadBytes($length))
}

function Get-Sha256([byte[]]$bytes) {
    $sha = [Security.Cryptography.SHA256]::Create()
    try { return [BitConverter]::ToString($sha.ComputeHash($bytes)).Replace('-', '').ToLowerInvariant() }
    finally { $sha.Dispose() }
}

$element = [NexplantQMS.GdsMap.Persistence.MapPlacedElementData]::new()
$element.PlacedElementId = 'R0/E7'
$element.SourceElementId = 'TOP/E7'
$element.LayerId = 5
$element.DataType = 2
$element.ElementType = 'TEXT'
$element.Text = '입력 단자'
$element.TextType = 0
$element.TextFont = 3
$element.TextHorizontal = 'Center'
$element.TextVertical = 'Top'
$precisePoint = [NexplantQMS.GdsMap.GPoint]::new(0, 0)
$precisePoint.X = 1.123456789
$precisePoint.Y = -2.5
$element.WorldPoints = [NexplantQMS.GdsMap.GPoint[]]@($precisePoint)
$element.Bounds = [NexplantQMS.GdsMap.GBox]::new(1.123456789, -2.5, 1.123456789, -2.5)
$builder = [NexplantQMS.GdsMap.Persistence.MapPlacedElementBatchBuilder]::new(10, 100000L)
Assert-True ($builder.TryAdd($element)) '검사 도형을 배치에 추가하지 못했습니다.'
$batch = $builder.TakeBatch(2)
$jobId = '11995adc-bd4f-47d9-9b4a-0fd90fb087c9'
$revisionId = 'edb7aa55-a8da-4127-90e9-af1f90338b6c'
$payload = [NexplantQMS.GdsMap.Persistence.MapPlacedBatchPayload]::Create(
    $batch, $jobId, $revisionId, 100000)
$repeated = [NexplantQMS.GdsMap.Persistence.MapPlacedBatchPayload]::Create(
    $batch, $jobId, $revisionId, 100000)
Assert-True ($payload.PayloadSha256 -eq $repeated.PayloadSha256) '같은 배치의 해시가 달라졌습니다.'
Assert-True ($payload.StreamKind -eq 'PLACED' -and $payload.BatchNo -eq 2) '배치 메타데이터가 다릅니다.'

$sha = [Security.Cryptography.SHA256]::Create()
try { $actualHash = [BitConverter]::ToString($sha.ComputeHash($payload.Payload)).Replace('-', '').ToLowerInvariant() }
finally { $sha.Dispose() }
Assert-True ($payload.PayloadSha256 -eq $actualHash) '본문 SHA-256이 다릅니다.'

$stream = [IO.MemoryStream]::new($payload.Payload)
$reader = [IO.BinaryReader]::new($stream, [Text.Encoding]::UTF8)
try {
    Assert-True ([Text.Encoding]::ASCII.GetString($reader.ReadBytes(4)) -eq 'GMP1') '본문 형식 식별자가 다릅니다.'
    Assert-True ($reader.ReadInt32() -eq 2) '형식 버전이 다릅니다.'
    Assert-True ($reader.ReadInt32() -eq 2) '배치 번호가 다릅니다.'
    Assert-True ($reader.ReadInt32() -eq 1) '도형 수가 다릅니다.'
    Assert-True ($reader.ReadInt64() -eq 1) '좌표 수가 다릅니다.'
    Assert-True ((Read-Text $reader) -eq 'R0/E7') '배치 ID가 다릅니다.'
    Assert-True ((Read-Text $reader) -eq 'TOP/E7') '원본 ID가 다릅니다.'
    Assert-True ($reader.ReadInt32() -eq 5 -and $reader.ReadInt32() -eq 2) 'Layer/DataType이 다릅니다.'
    Assert-True ((Read-Text $reader) -eq 'TEXT') '도형 종류가 다릅니다.'
    [void]$reader.ReadBoolean()
    [void]$reader.ReadDouble()
    Assert-True ($reader.ReadByte() -eq 0) 'TEXT의 PATH 끝 모양 기본값이 다릅니다.'
    1..4 | ForEach-Object { [void]$reader.ReadDouble() }
    Assert-True ((Read-Text $reader) -eq '입력 단자') 'UTF-8 문자열이 다릅니다.'
    Assert-True ($reader.ReadBoolean() -and $reader.ReadInt32() -eq 0) 'TEXT Type이 다릅니다.'
    Assert-True ($reader.ReadBoolean() -and $reader.ReadInt32() -eq 3) 'TEXT Font가 다릅니다.'
    Assert-True ((Read-Text $reader) -eq 'Center') 'TEXT 가로 정렬이 다릅니다.'
    Assert-True ((Read-Text $reader) -eq 'Top') 'TEXT 세로 정렬이 다릅니다.'
    Assert-True ($reader.ReadInt32() -eq 1) '도형 좌표 개수가 다릅니다.'
    Assert-True ($reader.ReadDouble() -eq 1.123456789 -and $reader.ReadDouble() -eq -2.5) '월드 좌표가 다릅니다.'
    Assert-True ($stream.Position -eq $stream.Length) '읽고 남은 본문이 있습니다.'
}
finally { $reader.Dispose(); $stream.Dispose() }

$rejected = $false
try {
    [void][NexplantQMS.GdsMap.Persistence.MapPlacedBatchPayload]::Create(
        $batch, $jobId, $revisionId, ($payload.Payload.Length - 1))
} catch [System.InvalidOperationException] { $rejected = $true }
Assert-True $rejected '실제 요청 바이트 상한을 넘었는데 통과했습니다.'

# 파일/DB에서 읽은 바이트가 원래 Map 도형 정보로 돌아오는지 확인한다.
$restored = [NexplantQMS.GdsMap.Persistence.MapPlacedBatchReader]::Read(
    $payload.Payload, $payload.PayloadSha256, 100000, 10)
Assert-True ($restored.BatchNo -eq 2 -and $restored.PointCount -eq 1) '복원된 배치 헤더가 다릅니다.'
$restoredElement = $restored.Elements[0]
Assert-True ($restoredElement.PlacedElementId -eq 'R0/E7' -and $restoredElement.SourceElementId -eq 'TOP/E7') '복원된 도형 ID가 다릅니다.'
Assert-True ($restoredElement.Text -eq '입력 단자' -and $restoredElement.TextType -eq 0 -and $restoredElement.TextFont -eq 3) '복원된 TEXT 값이 다릅니다.'
Assert-True ($restoredElement.WorldPoints[0].X -eq 1.123456789 -and $restoredElement.WorldPoints[0].Y -eq -2.5) '복원된 좌표가 다릅니다.'

$changed = [byte[]]$payload.Payload.Clone()
$changed[$changed.Length - 1] = $changed[$changed.Length - 1] -bxor 1
$hashRejected = $false
try { [void][NexplantQMS.GdsMap.Persistence.MapPlacedBatchReader]::Read($changed, $payload.PayloadSha256, 100000, 10) }
catch [System.IO.InvalidDataException] { $hashRejected = $true }
Assert-True $hashRejected '변조된 본문을 해시 검사에서 거부하지 않았습니다.'

$versionChanged = [byte[]]$payload.Payload.Clone()
$versionChanged[4] = 1
$versionRejected = $false
try { [void][NexplantQMS.GdsMap.Persistence.MapPlacedBatchReader]::Read($versionChanged, (Get-Sha256 $versionChanged), 100000, 10) }
catch [System.IO.InvalidDataException] { $versionRejected = $true }
Assert-True $versionRejected '지원하지 않는 형식 버전을 거부하지 않았습니다.'

$truncated = [byte[]]::new($payload.Payload.Length - 1)
[Array]::Copy($payload.Payload, $truncated, $truncated.Length)
$truncatedRejected = $false
try { [void][NexplantQMS.GdsMap.Persistence.MapPlacedBatchReader]::Read($truncated, (Get-Sha256 $truncated), 100000, 10) }
catch [System.IO.InvalidDataException] { $truncatedRejected = $true }
Assert-True $truncatedRejected '끝이 잘린 본문을 거부하지 않았습니다.'

$duplicateBuilder = [NexplantQMS.GdsMap.Persistence.MapPlacedElementBatchBuilder]::new(10, 100000L)
Assert-True ($duplicateBuilder.TryAdd($element)) '중복 검사 첫 도형 추가 실패'
Assert-True ($duplicateBuilder.TryAdd($element)) '중복 검사 둘째 도형 추가 실패'
$duplicatePayload = [NexplantQMS.GdsMap.Persistence.MapPlacedBatchPayload]::Create(
    $duplicateBuilder.TakeBatch(3), $jobId, $revisionId, 100000)
$duplicateRejected = $false
try { [void][NexplantQMS.GdsMap.Persistence.MapPlacedBatchReader]::Read($duplicatePayload.Payload, $duplicatePayload.PayloadSha256, 100000, 10) }
catch [System.IO.InvalidDataException] { $duplicateRejected = $true }
Assert-True $duplicateRejected '중복 배치 Element ID를 거부하지 않았습니다.'

# 복원된 DTO를 기존 OpenGL/Chain SceneItem으로 바꿀 때 PATH/TEXT 속성을 유지한다.
$assembly = [Reflection.Assembly]::LoadFrom((Resolve-Path $assemblyPath).Path)
$factoryType = $assembly.GetType('NexplantQMS.GdsMap.Persistence.MapPlacedSceneItemFactory', $true)
$factory = $factoryType.GetMethod('Create', [Reflection.BindingFlags]'Static, NonPublic')
$textItem = $factory.Invoke($null, [object[]]@($restoredElement))
$textSource = $textItem.GetType().GetProperty('Source').GetValue($textItem)
Assert-True ($textItem.PlacedElementId -eq 'R0/E7' -and $textItem.LayerID -eq 5) '복원 TEXT의 Scene ID/Layer가 다릅니다.'
Assert-True ($textSource.Text -eq '입력 단자' -and $textSource.HorizontalPresentation.ToString() -eq 'Center') '복원 TEXT 표시 속성이 다릅니다.'

$path = [NexplantQMS.GdsMap.Persistence.MapPlacedElementData]::new()
$path.PlacedElementId = 'R0/E8'
$path.SourceElementId = 'TOP/E8'
$path.LayerId = 6
$path.ElementType = 'PATH'
$path.PathWidth = 4.5
$path.PathType = 1
$path.WorldPoints = [NexplantQMS.GdsMap.GPoint[]]@([NexplantQMS.GdsMap.GPoint]::new(3, 4))
$path.Bounds = [NexplantQMS.GdsMap.GBox]::new(0.75, 1.75, 5.25, 6.25)
$pathBuilder = [NexplantQMS.GdsMap.Persistence.MapPlacedElementBatchBuilder]::new(10, 100000L)
Assert-True ($pathBuilder.TryAdd($path)) 'PATH 배치 추가 실패'
$pathPayload = [NexplantQMS.GdsMap.Persistence.MapPlacedBatchPayload]::Create(
    $pathBuilder.TakeBatch(4), $jobId, $revisionId, 100000)
$restoredPath = [NexplantQMS.GdsMap.Persistence.MapPlacedBatchReader]::Read(
    $pathPayload.Payload, $pathPayload.PayloadSha256, 100000, 10).Elements[0]
$pathItem = $factory.Invoke($null, [object[]]@($restoredPath))
$pathSource = $pathItem.GetType().GetProperty('Source').GetValue($pathItem)
Assert-True ($restoredPath.PathType -eq 1 -and $pathSource.PathType -eq 1) 'PATH 끝 모양이 복원되지 않았습니다.'
Assert-True ($pathItem.Width -eq 4.5 -and $pathItem.WorldPoints[0].X -eq 3) 'PATH 폭/좌표가 다릅니다.'

'MAP_PLACED_BATCH_PAYLOAD_PROBE=PASS'
