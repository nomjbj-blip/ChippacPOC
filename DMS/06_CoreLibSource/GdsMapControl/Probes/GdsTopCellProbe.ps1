# 최상위 Cell 선택 검증 (2026-10-20). 설계: 문서/설계/02_GDS/2026-10-20_U8_GDS_로딩_원인분석_설계.md D1
# 확인: GdsLibrary.GetTopStructures / GetDefaultTopStructureName 이 첫 Structure 가 하위 Cell 인 U8 파일에서 제품 Cell 을 고르고,
#       최상위 Cell 이 하나인 기존 파일은 지금과 같은 Cell 을 고르는지. 펼친 도형 수가 원본 레코드 분석 값(완전 중복 제외)과 같은지.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsTopCellProbe.ps1 [-BinDir <NexplantQMS.GdsMap.dll 폴더>]
param([string]$BinDir = '')
if ($BinDir -eq '') { $BinDir = Join-Path (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path '100_ClientBin\GdsMap' }
$ErrorActionPreference = 'Stop'
$root = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..\..\SampleFile')).Path
$work = Join-Path $env:TEMP 'chippac\topcellprobe'
New-Item -ItemType Directory -Force $work | Out-Null
Copy-Item (Join-Path $BinDir 'NexplantQMS.GdsMap.dll') (Join-Path $work 'NexplantQMS.GdsMap.dll') -Force
foreach ($d in 'OpenTK.dll', 'OpenTK.GLControl.dll') { Copy-Item (Join-Path $BinDir $d) $work -Force }
foreach ($d in 'OpenTK.dll', 'OpenTK.GLControl.dll', 'NexplantQMS.GdsMap.dll') { [void][Reflection.Assembly]::LoadFrom((Join-Path $work $d)) }
$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) { $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line }

foreach ($case in @(
    @{ File = 'OMM all layer2.gds\OMM all layer2.gds'; Expect = $null },
    @{ File = '26_10_20 Sample File U8\OMM_PFIC_PRODUCT_PIC_PKG_A0_0410_v1.gds'; Expect = 'OMM_PFIC_PIC_PKG_A0' })) {
    $reader = New-Object NexplantQMS.GdsMap.GdsReader
    $reader.LengthUnit = [NexplantQMS.GdsMap.LengthUnit]::Micrometer
    $lib = $reader.Read((Join-Path $root $case.File))
    $tops = $lib.GetTopStructures()
    $def = $lib.GetDefaultTopStructureName()
    $first = $lib.Structures[0].Name
    "[{0}] Structure {1}개 / 첫 Structure {2} / 최상위 {3}" -f (Split-Path $case.File -Leaf), $lib.Structures.Count, $first, (($tops | ForEach-Object { "$($_.Name)=$($_.ElementCount)" }) -join ', ')
    if ($case.Expect -eq $null) {
        Check "$(Split-Path $case.File -Leaf) 기본 Cell = 첫 Structure(기존 동작 유지)" ($def -eq $first) "기본 $def / 첫 $first"
    } else {
        Check "$(Split-Path $case.File -Leaf) 기본 Cell" ($def -eq $case.Expect) "기본 $def"
        $names = @($tops | ForEach-Object { $_.Name })
        Check 'U8 최상위 Cell 3개(순서: 제품 / TOP / mark_cell)' (($names -join ',') -eq 'OMM_PFIC_PIC_PKG_A0,TOP,mark_cell') ($names -join ',')
        $cnt = @{}; foreach ($t in $tops) { $cnt[$t.Name] = $t.ElementCount }
        # 원본 레코드 분석(제품 1,885,539 / TOP 68,288)에서 파서가 빼는 완전 중복을 뺀 값:
        # 제품 = 1,885,539 - 제품 자체 6,586 - backside_all 6,582 - TDV$2 1 - (TDV + TDV$1) 2 x TSV 27,416 = 1,817,538 / TOP = 68,288 - Pkg_C4 SREF 중복 17,072 = 51,216
        $dupTop = $lib.Structures | Where-Object { $_.Name -eq 'TOP' } | ForEach-Object { $_.Layers.DuplicatedItems.Count }
        Check 'U8 TOP 중복 = 같은 자리 Pkg_C4 SREF 17,072' ($dupTop -eq 17072) "중복 $dupTop"
        Check 'U8 펼친 도형 수 = 원본 - 완전 중복' ($cnt['OMM_PFIC_PIC_PKG_A0'] -eq 1817538 -and $cnt['TOP'] -eq 51216 -and $cnt['mark_cell'] -eq 0) ("제품 {0} / TOP {1} / mark {2}" -f $cnt['OMM_PFIC_PIC_PKG_A0'], $cnt['TOP'], $cnt['mark_cell'])
        # 작은 Cell 은 실제로 펼쳐 화면 항목 수와 비교
        $map = New-Object NexplantQMS.GdsMap.GdsMapControl
        $map.ShowStructure($lib, 'TOP')
        $n = $map.VisitPlacedElements([Action[NexplantQMS.GdsMap.Persistence.MapPlacedElementData]]{ param($d) })
        Check 'TOP Cell 펼치기 = 계산값(BOUNDARY + TEXT 배치 도형)' ($n -eq 51216) "배치 도형 $n / Structure $($map.Structure.Name)"
        $map.Dispose()
    }
}
$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { 'GDS_TOP_CELL_PROBE=PASS (' + $results.Count + ')' } else { "GDS_TOP_CELL_PROBE=FAIL ($fail)"; exit 1 }
