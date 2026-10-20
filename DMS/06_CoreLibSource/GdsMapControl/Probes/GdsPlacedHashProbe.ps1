# 펼친 배치 결과 해시 회귀 검증 (2026-10-20). 설계: 문서/설계/02_GDS/2026-10-20_U8_GDS_로딩_원인분석_설계.md D3
# 해시 대상: 배치 ID / Layer / 종류 / 좌표 double 비트 / PATH 폭 / TEXT. 그리기 버퍼 / 펼치기 속도를 바꿔도 펼친 결과는 같아야 한다.
# 기준값은 2026-10-20 이전 운영 EXE(평행 이동을 GDI+ Matrix 로 계산)로 만든 값이다. 값이 바뀌면 좌표나 순서가 바뀐 것이다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsPlacedHashProbe.ps1 [-ExePath <NexplantQMS.GdsMap.exe>] [-WithU8Product]
param(
    [string]$ExePath = '',
    [switch]$WithU8Product,
    [string]$WorkName = 'placedhash'
)
if ($ExePath -eq '') { $ExePath = Join-Path (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path '100_ClientBin\GdsMap\NexplantQMS.GdsMap.exe' }
$sample = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..\..\SampleFile')).Path
$cases = @(
    @{ Gds = 'OMM all layer2.gds\OMM all layer2.gds'; Cell = 'TOP'; Count = 605080; Hash = '73FB0E30F9C58191' },
    # U8 TOP 은 Layer 18 TEXT 가 18/3 / 18/4 Layer 로 나뉘어 방문 순서만 바뀌었다(이전 C383FBDEA6715B57, 순서 무관 비교 같음) (2026-10-20)
    @{ Gds = '26_10_20 Sample File U8\OMM_PFIC_PRODUCT_PIC_PKG_A0_0410_v1.gds'; Cell = 'TOP'; Count = 51216; Hash = 'ABC03DD4EB485D89' },
    @{ Gds = '26_10_20 Sample File U8\OMM_PFIC_PRODUCT_PIC_PKG_A0_0410_v1.gds'; Cell = 'U8_PIC_C4'; Count = 82242; Hash = '6CE19A57F7A52570' })
# 제품 Cell 은 이전 EXE 로 열리지 않아(배열 한도) 새 방식의 첫 결과를 기준으로 둔다.
# Layer/DataType 분리로 방문 순서만 바뀌었다(이전 2D9CADF443B64E6B, 순서 무관 비교 같음) (2026-10-20)
if ($WithU8Product) { $cases += @{ Gds = '26_10_20 Sample File U8\OMM_PFIC_PRODUCT_PIC_PKG_A0_0410_v1.gds'; Cell = 'OMM_PFIC_PIC_PKG_A0'; Count = 1817538; Hash = '73D03815DE2980C8' } }
$ErrorActionPreference = 'Stop'
$work = Join-Path $env:TEMP ('chippac\' + $WorkName)
New-Item -ItemType Directory -Force $work | Out-Null
Copy-Item $ExePath (Join-Path $work 'NexplantQMS.GdsMap.dll') -Force
foreach ($d in 'OpenTK.dll', 'OpenTK.GLControl.dll') { Copy-Item (Join-Path (Split-Path $ExePath) $d) $work -Force }
foreach ($d in 'OpenTK.dll', 'OpenTK.GLControl.dll', 'NexplantQMS.GdsMap.dll') { [void][Reflection.Assembly]::LoadFrom((Join-Path $work $d)) }
Add-Type -AssemblyName System.Windows.Forms
Add-Type -ReferencedAssemblies (Join-Path $work 'NexplantQMS.GdsMap.dll'), (Join-Path $work 'OpenTK.dll'), (Join-Path $work 'OpenTK.GLControl.dll'), System.Windows.Forms, System.Drawing -TypeDefinition @"
using System; using System.IO; using System.Text; using System.Security.Cryptography;
using NexplantQMS.GdsMap; using NexplantQMS.GdsMap.Persistence;
public static class PlacedHash {
    public static string Run(GdsMapControl map, out int count, out long ms) {
        var sha = SHA256.Create(); var buf = new MemoryStream(); var w = new BinaryWriter(buf, Encoding.UTF8);
        int n = 0;
        var sw = System.Diagnostics.Stopwatch.StartNew();
        map.VisitPlacedElements(d => {
            w.Write(d.PlacedElementId ?? ""); w.Write(d.LayerId); w.Write(d.ElementType ?? ""); w.Write(d.PathWidth); w.Write(d.Text ?? "");
            foreach (var p in d.WorldPoints) { w.Write(BitConverter.DoubleToInt64Bits(p.X)); w.Write(BitConverter.DoubleToInt64Bits(p.Y)); }
            n++;
            if (buf.Length > (1 << 20)) { w.Flush(); var a = buf.ToArray(); sha.TransformBlock(a, 0, a.Length, null, 0); buf.SetLength(0); }
        });
        w.Flush(); var last = buf.ToArray(); sha.TransformFinalBlock(last, 0, last.Length);
        count = n; ms = sw.ElapsedMilliseconds;
        return BitConverter.ToString(sha.Hash).Replace("-", "").Substring(0, 16);
    }
}
"@
$results = New-Object Collections.Generic.List[string]
$libs = @{}
foreach ($case in $cases) {
    if (-not $libs.ContainsKey($case.Gds)) {
        $reader = New-Object NexplantQMS.GdsMap.GdsReader
        $reader.LengthUnit = [NexplantQMS.GdsMap.LengthUnit]::Micrometer
        $libs.Clear(); [GC]::Collect()
        $libs[$case.Gds] = $reader.Read((Join-Path $sample $case.Gds))
    }
    $map = New-Object NexplantQMS.GdsMap.GdsMapControl
    $sw = [Diagnostics.Stopwatch]::StartNew()
    $map.ShowStructure($libs[$case.Gds], $case.Cell)
    $show = $sw.ElapsedMilliseconds
    $n = 0; $ms = 0L
    $hash = [PlacedHash]::Run($map, [ref]$n, [ref]$ms)
    $ok = ($n -eq $case.Count -and $hash -eq $case.Hash)
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + (" / {0} {1} / 배치 {2:N0} (기준 {3:N0}) / 해시 {4} (기준 {5}) / {6:N1}초" -f (Split-Path $case.Gds -Leaf), $case.Cell, $n, $case.Count, $hash, $case.Hash, ($show / 1000))
    $results.Add($line); Write-Host $line
    $map.Dispose(); [GC]::Collect()
}
$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host ('GDS_PLACED_HASH_PROBE=PASS (' + $results.Count + ')') } else { Write-Host "GDS_PLACED_HASH_PROBE=FAIL ($fail)"; exit 1 }