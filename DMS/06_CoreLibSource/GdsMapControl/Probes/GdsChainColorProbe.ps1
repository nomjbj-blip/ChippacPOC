# Chain 색 자동 선택 검증 스크립트
# 목적: ChainColorPicker가 기존 Chain 색과 최대한 다른 색을 고르는지 확인한다.
#       예전 방식(고정 8색 순환)과 새 방식으로 Chain 8개를 만들 때 "가장 비슷한 두 색의 차이(ΔE)"를 비교한다.
#       ΔE는 CIE Lab 거리이며 약 10 이하면 화면에서 같은 계열로 보이기 쉽다.
# 대상: GdsMapControl\bin\Debug\NexplantQMS.GdsMap.exe (DB / Service 사용 안 함)
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsChainColorProbe.ps1 [-BinDir <EXE 폴더>]
param([string]$BinDir = '')
$ErrorActionPreference = 'Stop'
if ($BinDir -eq '') { $BinDir = Join-Path $PSScriptRoot '..\bin\Debug' }
Add-Type -AssemblyName System.Drawing
# EXE를 잠그지 않도록 바이트로 읽어 로드한다.
$asm = [Reflection.Assembly]::Load([IO.File]::ReadAllBytes((Join-Path $BinDir 'NexplantQMS.GdsMap.exe')))
$picker = $asm.GetType('NexplantQMS.GdsMap.Chain.ChainColorPicker', $true)
function Pick($used, $layers) { return $picker.GetMethod('Pick').Invoke($null, @([Drawing.Color[]]$used, [Drawing.Color[]]$layers)) }
function Get-ColorDiff($a, $b) { return [double]$picker.GetMethod('Difference').Invoke($null, @($a, $b)) }
function MinPair($colors) {
    $min = [double]::MaxValue; $pair = ''
    for ($i = 0; $i -lt $colors.Count; $i++) { for ($j = $i + 1; $j -lt $colors.Count; $j++) {
        $d = Get-ColorDiff $colors[$i] $colors[$j]; if ($d -lt $min) { $min = $d; $pair = "$($colors[$i].Name) / $($colors[$j].Name)" } } }
    return @($min, $pair)
}
function Html($c) { return [Drawing.ColorTranslator]::ToHtml($c) }

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}

# 1. 예전 고정 8색의 가장 가까운 쌍
$old = [Drawing.Color[]]@([Drawing.Color]::Gold, [Drawing.Color]::DeepSkyBlue, [Drawing.Color]::LimeGreen, [Drawing.Color]::OrangeRed,
    [Drawing.Color]::Violet, [Drawing.Color]::Cyan, [Drawing.Color]::HotPink, [Drawing.Color]::White)
$oldMin = MinPair $old
Write-Host ("예전 8색 최소 ΔE = {0:N1} ({1})" -f $oldMin[0], $oldMin[1])

# 2. 새 방식으로 Chain 8개를 차례로 만든다(Layer 색 없음)
$new = New-Object 'Collections.Generic.List[Drawing.Color]'
for ($k = 0; $k -lt 8; $k++) { $new.Add((Pick $new.ToArray() @())) }
$newMin = MinPair $new.ToArray()
Write-Host ("새 방식 8색: " + (($new | ForEach-Object { Html $_ }) -join ' '))
Check '새 방식 8색 최소 ΔE > 예전 / 30 이상' ($newMin[0] -gt $oldMin[0] -and $newMin[0] -ge 30) ("{0:N1} ({1})" -f $newMin[0], $newMin[1])

# 3. 화면 예시: DB에서 불러온 초록 / 하늘색 Chain이 있을 때 다음 색
$existing = [Drawing.Color[]]@([Drawing.Color]::LimeGreen, [Drawing.Color]::DeepSkyBlue)
$next = Pick $existing @()
$d = [Math]::Min((Get-ColorDiff $next $existing[0]), (Get-ColorDiff $next $existing[1]))
Check '기존 초록 / 하늘색과 다른 색' ($d -ge 50) ("{0} / 최소 ΔE {1:N1}" -f (Html $next), $d)

# 4. 같은 입력이면 같은 색 (항상 같은 결과)
$again = Pick $existing @()
Check '같은 입력 같은 결과' ($again.ToArgb() -eq $next.ToArgb()) (Html $again)

# 5. Chain이 없을 때 Layer 색(초록 / 올리브 계열 배경)과 다른 색
$layers = [Drawing.Color[]]@([Drawing.Color]::FromArgb(80, 180, 80), [Drawing.Color]::FromArgb(128, 128, 64), [Drawing.Color]::FromArgb(70, 200, 200))
$first = Pick @() $layers
$dl = ($layers | ForEach-Object { Get-ColorDiff $first $_ } | Measure-Object -Minimum).Minimum
Check '첫 Chain은 Layer 색과 다른 색' ($dl -ge 40) ("{0} / Layer 최소 ΔE {1:N1}" -f (Html $first), $dl)

# 6. Chain 30개까지 계속 만들어도 오류 없이 불투명 색을 준다
$many = New-Object 'Collections.Generic.List[Drawing.Color]'
for ($k = 0; $k -lt 30; $k++) { $many.Add((Pick $many.ToArray() $layers)) }
Check 'Chain 30개 생성' ($many.Count -eq 30 -and @($many | Where-Object { $_.A -ne 255 }).Count -eq 0) ("최소 ΔE {0:N1}" -f (MinPair $many.ToArray())[0])

# 7. 어두운 지도 배경(#18181C)에서 보이는 색만 고른다
$background = [Drawing.Color]::FromArgb(24, 24, 28)
$minBg = ($many | ForEach-Object { Get-ColorDiff $_ $background } | Measure-Object -Minimum).Minimum
Check '배경과 충분히 다른 색만 (ΔE 50 이상)' ($minBg -ge 50) ("최소 ΔE {0:N1}" -f $minBg)

# 8. 흰색 제외 (2026-10-12 요청: 흰색은 유사 묶음 찾기 결과 표시 색)
$white = [Drawing.Color]::White
$lots = New-Object 'Collections.Generic.List[Drawing.Color]'
for ($k = 0; $k -lt 80; $k++) { $lots.Add((Pick $lots.ToArray() $layers)) }
$minWhite = ($lots | ForEach-Object { Get-ColorDiff $_ $white } | Measure-Object -Minimum).Minimum
Check '자동 색 80개에 흰색 / 흰색 근처 없음 (ΔE 25 이상)' ($minWhite -ge 25) ("최소 ΔE {0:N1}" -f $minWhite)
$suggested = [Drawing.Color[]]$picker.GetField('SuggestedColors').GetValue($null)
$suggestedWhite = @($suggested | Where-Object { (Get-ColorDiff $_ $white) -lt 25 }).Count
Check '색 선택 창 추천 16색에 흰색 없음' ($suggested.Count -eq 16 -and $suggestedWhite -eq 0) ("흰색 근처 $suggestedWhite 개")
$nearWhite = [bool]$picker.GetMethod('IsNearSimilarPreviewColor').Invoke($null, @([Drawing.Color]::FromArgb(250, 250, 245)))
$notWhite = [bool]$picker.GetMethod('IsNearSimilarPreviewColor').Invoke($null, @([Drawing.Color]::Gold))
Check '직접 고른 색 안내 판정 (거의 흰색 = 예 / Gold = 아니오)' ($nearWhite -and -not $notWhite) "$nearWhite / $notWhite"

$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host 'GDS_CHAIN_COLOR_PROBE=PASS' } else { Write-Host "GDS_CHAIN_COLOR_PROBE=FAIL ($fail)"; exit 1 }
