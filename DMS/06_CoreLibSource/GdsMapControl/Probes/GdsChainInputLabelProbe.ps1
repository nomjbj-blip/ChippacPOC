# Chain Input 근처 Label 찾기 검증 스크립트
# 목적: GdsMapControl.FindChainEndpointLabel 이 Input과 연결된 도형 안의 Label을 찾는지, 시간이 짧은지 확인한다.
#       샘플 GDS의 Label을 골고루 골라 (1) Label이 들어 있는 가장 작은 도형 E(Pad 등) (2) E와 실제로 겹치는 이웃 도형 N 을 찾고,
#       N을 Input으로 두었을 때 결과가 "연결(0~1단계)"이고 이름이 그 Label인지 본다(N 안에 다른 Label이 있으면 0단계 결과도 정답).
# 대상: GdsMap EXE(기본 TEMP 빌드 %TEMP%\ChippacBuild\GdsMap) / 샘플 GDS 파일. DB / Service 사용 안 함.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsChainInputLabelProbe.ps1 [-BinDir <EXE 폴더>] [-GdsPath 경로] [-Samples 60]
param(
    [string]$BinDir = '',
    [string]$GdsPath = '',
    [int]$Samples = 60
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
if ($BinDir -eq '') { $BinDir = Join-Path $env:TEMP 'ChippacBuild\GdsMap' }
if ($GdsPath -eq '') { $GdsPath = Join-Path (Split-Path $dms -Parent) 'SampleFile\OMM all layer2.gds\OMM all layer2.gds' }
# Add-Type(CodeDom)은 .exe를 참조로 받지 못하므로 시험용 임시 폴더에 .dll 이름으로 복사해 참조 / 로드한다.
$probeDir = Join-Path $env:TEMP 'chippac\gdschainlabelprobe'
New-Item -ItemType Directory -Force $probeDir | Out-Null
Copy-Item (Join-Path $BinDir 'NexplantQMS.GdsMap.dll') (Join-Path $probeDir 'NexplantQMS.GdsMap.dll') -Force
foreach ($d in 'OpenTK.dll', 'OpenTK.GLControl.dll') { Copy-Item (Join-Path $BinDir $d) $probeDir -Force }
foreach ($d in 'OpenTK.dll', 'OpenTK.GLControl.dll', 'NexplantQMS.GdsMap.dll') { [void][Reflection.Assembly]::LoadFrom((Join-Path $probeDir $d)) }
Add-Type -AssemblyName System.Windows.Forms

$helper = @"
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NexplantQMS.GdsMap;
using NexplantQMS.GdsMap.Chain;
using NexplantQMS.GdsMap.Persistence;

public static class ChainLabelProbe
{
    public static GdsLibrary Parse(string path) { return new GdsReader { LengthUnit = LengthUnit.Micrometer }.Read(path); }

    static ChainTraceElement Trace(MapPlacedElementData d)
    {
        return new ChainTraceElement(d.PlacedElementId, d.LayerId, d.Bounds, d.ElementType, d.WorldPoints,
            d.ElementType == "PATH" ? d.PathWidth : 0, d.DataType);
    }

    static bool Contains(MapPlacedElementData d, GPoint p)
    {
        return p.X >= d.Bounds.MinX && p.X <= d.Bounds.MaxX && p.Y >= d.Bounds.MinY && p.Y <= d.Bounds.MaxY
            && ChainGeometryOverlap.ContainsPoint(p, d.WorldPoints, d.ElementType == "PATH", d.PathWidth, 0);
    }

    /// <summary>반환: [0] 시험 수 / [1] 연결로 정답 / [2] 연결이지만 다른 Label / [3] 가까움(연결 실패) / [4] 못 찾음 / [5] 최대 ms / [6] 평균 ms. detail: 실패 예</summary>
    public static double[] Run(GdsMapControl map, int samples, out string detail)
    {
        var shapes = new List<MapPlacedElementData>();
        map.VisitPlacedElements(d => { if (d.ElementType != "TEXT") shapes.Add(d); });
        var labels = map.GetTextLabelDiagnostics().Where(t => !string.IsNullOrWhiteSpace(t.Text)).ToList();
        int step = Math.Max(1, labels.Count / Math.Max(1, samples * 3));
        int tested = 0, ok = 0, other = 0, nearest = 0, none = 0; double maxMs = 0, sumMs = 0;
        var fails = new List<string>();
        for (int i = 0; i < labels.Count && tested < samples; i += step)
        {
            var label = labels[i];
            var p = new GPoint(label.TransformedX, label.TransformedY);
            MapPlacedElementData pad = shapes.Where(d => Contains(d, p))
                .OrderBy(d => (d.Bounds.MaxX - d.Bounds.MinX) * (d.Bounds.MaxY - d.Bounds.MinY)).FirstOrDefault();
            if (pad == null) continue;
            ChainTraceElement padTrace = Trace(pad);
            MapPlacedElementData input = shapes.FirstOrDefault(d => !ReferenceEquals(d, pad)
                && d.Bounds.IntersectsWith(pad.Bounds) && !Contains(d, p) && ChainGeometryOverlap.Intersects(Trace(d), padTrace));
            if (input == null) continue;
            var click = new GPoint((input.Bounds.MinX + input.Bounds.MaxX) / 2, (input.Bounds.MinY + input.Bounds.MaxY) / 2);
            ChainEndpointLabelResult r = map.FindChainEndpointLabel(new[] { new KeyValuePair<string, GPoint>(input.PlacedElementId, click) });
            tested++;
            maxMs = Math.Max(maxMs, r.ElapsedMs); sumMs += r.ElapsedMs;
            if (r.Text == null) { none++; if (fails.Count < 3) fails.Add("없음 " + label.Text); }
            else if (!r.Connected) { nearest++; if (fails.Count < 3) fails.Add("가까움 " + label.Text + " -> " + r.Text + " 후보 " + r.CandidateCount + (r.CandidateLimitHit ? " 상한" : "")); }
            else if (r.Text == label.Text.Trim() || r.OtherTexts.Contains(label.Text.Trim())) ok++;
            else if (r.Hops == 0) ok++; // Input 도형 안에 다른 Label이 있으면 그 Label이 더 가깝다(정상).
            else { other++; if (fails.Count < 3) fails.Add("다른 Label " + label.Text + " -> " + r.Text + " " + r.Hops + "단계"); }
        }
        detail = string.Join(" | ", fails);
        return new double[] { tested, ok, other, nearest, none, maxMs, tested == 0 ? 0 : sumMs / tested };
    }
}
"@
Add-Type -TypeDefinition $helper -ReferencedAssemblies @((Join-Path $probeDir 'NexplantQMS.GdsMap.dll'), (Join-Path $probeDir 'OpenTK.dll'),
    (Join-Path $probeDir 'OpenTK.GLControl.dll'), 'System.Windows.Forms', 'System.Drawing')

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}

$map = New-Object NexplantQMS.GdsMap.GdsMapControl
$map.ShowStructure([ChainLabelProbe]::Parse($GdsPath))
Write-Host "GDS: $GdsPath / EXE: $BinDir"

$detail = ''
$r = [ChainLabelProbe]::Run($map, $Samples, [ref]$detail)
Write-Host ("시험 {0} / 연결 정답 {1} / 다른 Label {2} / 가까움 {3} / 없음 {4} / 최대 {5:N1}ms / 평균 {6:N1}ms" -f $r[0], $r[1], $r[2], $r[3], $r[4], $r[5], $r[6])
if ($detail -ne '') { Write-Host "예: $detail" }
Check '시험 표본 20개 이상' ($r[0] -ge 20) "$($r[0])"
Check '연결된 Label로 이름 결정 90% 이상' ($r[0] -gt 0 -and $r[1] / $r[0] -ge 0.9) ("{0:P0}" -f ($r[1] / [Math]::Max(1, $r[0])))
Check '한 번 찾는 시간 최대 300ms 이하' ($r[5] -le 300) ("{0:N1}ms" -f $r[5])
$none = $map.FindChainEndpointLabel([Collections.Generic.KeyValuePair[string, NexplantQMS.GdsMap.GPoint][]]@(
    (New-Object 'Collections.Generic.KeyValuePair[string,NexplantQMS.GdsMap.GPoint]'('NO/SUCH/ELEMENT', (New-Object NexplantQMS.GdsMap.GPoint(0, 0))))))
Check '없는 Input 도형은 null (이름 유지)' ($none -eq $null) ''

$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host 'GDS_CHAIN_INPUT_LABEL_PROBE=PASS' } else { Write-Host "GDS_CHAIN_INPUT_LABEL_PROBE=FAIL ($fail)"; exit 1 }
