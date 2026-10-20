# Layer / DataType 분리 검증 (2026-10-20). 설계: 문서/설계/02_GDS/2026-10-20_U8_GDS_로딩_원인분석_설계.md 9절
# 확인: U8 제품 Cell / TOP Cell 의 Layer 목록이 KLayout 과 같은 (번호, DataType) 키인지, 키별 도형 수 / 전체 도형 수,
#       저장 데이터(VisitPlacedElements)가 고른 키만 방문하고 TEXT 의 DataType = TEXTTYPE 인지. DB 없음.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsLayerKeyProbe.ps1 [-ExePath <NexplantQMS.GdsMap.exe>]
param([string]$ExePath = '', [string]$WorkName = 'layerkeyprobe')
if ($ExePath -eq '') { $ExePath = Join-Path (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path '100_ClientBin\GdsMap\NexplantQMS.GdsMap.exe' }
$ErrorActionPreference = 'Stop'
$sample = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..\..\SampleFile')).Path
$work = Join-Path $env:TEMP ('chippac\' + $WorkName)
New-Item -ItemType Directory -Force $work | Out-Null
Copy-Item $ExePath (Join-Path $work 'NexplantQMS.GdsMap.dll') -Force
foreach ($d in 'OpenTK.dll', 'OpenTK.GLControl.dll') { Copy-Item (Join-Path (Split-Path $ExePath) $d) $work -Force }
foreach ($d in 'OpenTK.dll', 'OpenTK.GLControl.dll', 'NexplantQMS.GdsMap.dll') { [void][Reflection.Assembly]::LoadFrom((Join-Path $work $d)) }
Add-Type -AssemblyName System.Windows.Forms
$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) { $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line }
$key = [NexplantQMS.GdsMap.GdsLayerKey]

# 키 변환
$k = $key::Make(18, 3)
Check '키 변환 Make / Layer / DataType / Format / Parse' ($key::Layer($k) -eq 18 -and $key::DataType($k) -eq 3 -and $key::Format($k) -eq '18/3' -and $key::Parse('18/3') -eq $k -and $key::Parse('18') -eq $key::Make(18, 0) -and $key::Make(18, 0) -lt $key::Make(18, 3) -and $key::Make(18, 65535) -lt $key::Make(19, 0)) "18/3 = $k"

$reader = New-Object NexplantQMS.GdsMap.GdsReader
$reader.LengthUnit = [NexplantQMS.GdsMap.LengthUnit]::Micrometer
$lib = $reader.Read("$sample\26_10_20 Sample File U8\OMM_PFIC_PRODUCT_PIC_PKG_A0_0410_v1.gds")
foreach ($cell in 'OMM_PFIC_PIC_PKG_A0', 'TOP') {
    $map = New-Object NexplantQMS.GdsMap.GdsMapControl
    $map.ShowStructure($lib, $cell)
    $items = $map.GetLayerDisplayItems()
    $counts = $map.GetLayerElementCounts()
    $names = ($items | ForEach-Object { $key::Format($_.LayerKey) }) -join ','
    Write-Host "[$cell] Layer $($items.Count)개: $(($items | ForEach-Object { '{0}={1}' -f $key::Format($_.LayerKey), $counts[$_.LayerKey] }) -join ' ')"
    if ($cell -eq 'OMM_PFIC_PIC_PKG_A0') {
        Check '제품 Cell Layer 17개(KLayout 과 같은 Layer/DataType, 51/0 은 TOP 에만 있음)' ($names -eq '1/0,11/0,17/0,18/0,18/3,18/4,50/0,50/3,50/4,55/0,91/0,99/0,850/0,860/0,890/0,900/0,1500/0') $names
        # 원본 분석(완전 중복 제외): 18/3 = 27,427 x 2 + 27,403 x 2, 18/4 = 27,405 x 2 + 12 x 2 / 50/3 = 27,417 x 2, 50/4 = (27,405 + 12) x 2
        $t183 = $counts[$key::Make(18, 3)]; $t184 = $counts[$key::Make(18, 4)]; $t503 = $counts[$key::Make(50, 3)]; $t504 = $counts[$key::Make(50, 4)]
        Check 'Label Layer 수: 18/3 / 18/4 / 50/3 / 50/4' ($t183 -eq 109660 -and $t184 -eq 54834 -and $t503 -eq 54834 -and $t504 -eq 54834) "18/3 $t183 / 18/4 $t184 / 50/3 $t503 / 50/4 $t504"
        $sum = 0; foreach ($v in $counts.Values) { $sum += $v }
        Check '전체 도형 수 = 이전(Layer 번호 묶음)과 같음' ($sum -eq 1817538) "합계 $sum"
        # 저장 데이터: TEXT DataType = TEXTTYPE, 도형은 DATATYPE
        $bad = 0; $seen = @{}
        $set = New-Object 'Collections.Generic.HashSet[int]'; [void]$set.Add($key::Make(18, 3)); [void]$set.Add($key::Make(18, 0))
        [void]$map.VisitPlacedElements($set, [Action[NexplantQMS.GdsMap.Persistence.MapPlacedElementData]]{ param($d)
            $id = "{0} {1} {2}" -f $d.ElementType, $d.LayerId, $d.DataType; $seen[$id] = 1 + [int]$seen[$id]
            if ($d.ElementType -eq 'TEXT' -and $d.DataType -ne $d.TextType) { $script:bad++ } })
        Check '저장 데이터: 18/3 / 18/0 만 방문, TEXT 의 DataType = TEXTTYPE' ($bad -eq 0 -and $seen.Count -eq 2 -and $seen['TEXT 18 3'] -eq 109660) (($seen.GetEnumerator() | ForEach-Object { "$($_.Key)=$($_.Value)" }) -join ' / ')
    } else {
        Check 'TOP Cell Layer 3개' ($names -eq '18/3,18/4,51/0') $names
    }
    $map.Dispose(); [GC]::Collect()
}
$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host ('GDS_LAYER_KEY_PROBE=PASS (' + $results.Count + ')') } else { Write-Host "GDS_LAYER_KEY_PROBE=FAIL ($fail)"; exit 1 }
