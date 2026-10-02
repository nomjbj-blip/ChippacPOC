# 일괄 제외와 직전 되돌리기가 기존 수동 제외 기록을 건드리지 않는지 검사한다.
$ErrorActionPreference = 'Stop'
$exePath = if ($env:CHAIN_PROBE_EXE) { $env:CHAIN_PROBE_EXE }
    else { Join-Path $PSScriptRoot '..\bin\EndpointQA\NexplantQMS.GdsMap.exe' }
[void][System.Reflection.Assembly]::LoadFrom((Resolve-Path $exePath).Path)
$automatic = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
$added = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
$excluded = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
[void]$automatic.Add('AUTO-A')
[void]$automatic.Add('AUTO-B')
[void]$added.Add('MANUAL-C')
[void]$excluded.Add('AUTO-B')
$change = [NexplantQMS.GdsMap.Chain.ChainSimilarBatchEdit]::Apply(
    [string[]]@('AUTO-A', 'MANUAL-C'), $automatic, $added, $excluded)
if ($change.ChangedCount -ne 2 -or $added.Count -ne 0 -or
    -not $excluded.Contains('AUTO-A') -or -not $excluded.Contains('AUTO-B')) {
    throw '자동 후보와 수동 추가 객체의 일괄 제외 상태가 잘못되었습니다.'
}
$change.Restore($added, $excluded)
if (-not $added.Contains('MANUAL-C') -or $added.Count -ne 1 -or
    $excluded.Contains('AUTO-A') -or -not $excluded.Contains('AUTO-B') -or
    $excluded.Count -ne 1) {
    throw '되돌리기가 기존 수동 제외 상태를 보존하지 못했습니다.'
}
Write-Output 'CHAIN_SIMILAR_BATCH_PROBE=PASS'
