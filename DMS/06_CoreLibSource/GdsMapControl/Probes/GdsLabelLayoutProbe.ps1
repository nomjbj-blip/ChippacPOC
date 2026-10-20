# GDS Label 배치 검증 (2026-10-20). 설계: 문서/설계/02_GDS/2026-10-20_U8_Label_표시크기_설계.md
# 목적: GdsMapControl.LayoutTextLabels(글자 / 위치 / 크기 계산)를 비트맵 Graphics 로 불러 확인한다. 화면은 띄우지 않는다.
#   - 글자 크기는 배율마다 화면 전체에서 하나(Label 마다 이웃 거리로 바꾸지 않음), 배율이 2배면 글자도 약 2배.
#   - OMM all layer2: 이전 규칙(아래 OldLayout, 2026-10-20 이전 계산을 그대로 옮김)과 같은 Label 이 보이고, 크기는 1종으로 맞춰진다(18 화면).
#   - U8 제품 Cell: 같은 내용 사본은 그리지 않음 / 화면 크기 기준 칸 거의 없음 / 멀리서는 안 보임 / TSV 구역(빈 자리 옆) 크기 1종 /
#     18/3(핀 번호) / 18/4(Net 이름)를 따로 켜고 끔 / 둘 다 켜면 Net 위 / 핀 번호 아래 두 줄
# 실행: powershell -NoProfile -STA -ExecutionPolicy Bypass -File GdsLabelLayoutProbe.ps1 -GdsExe <빌드한 NexplantQMS.GdsMap.exe> [-SkipU8]
param(
    [Parameter(Mandatory = $true)][string]$GdsExe,
    [switch]$SkipU8
)
$ErrorActionPreference = 'Stop'
$sample = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..\..\SampleFile')).Path
$work = Join-Path $env:TEMP 'chippac\gdslabellayout'
New-Item -ItemType Directory -Force $work | Out-Null
Copy-Item $GdsExe (Join-Path $work 'NexplantQMS.GdsMap.dll') -Force
foreach ($d in 'OpenTK.dll', 'OpenTK.GLControl.dll') { Copy-Item (Join-Path (Split-Path $GdsExe) $d) $work -Force }
foreach ($d in 'OpenTK.dll', 'OpenTK.GLControl.dll', 'NexplantQMS.GdsMap.dll') { [void][Reflection.Assembly]::LoadFrom((Join-Path $work $d)) }
Add-Type -AssemblyName System.Windows.Forms, System.Drawing

$helper = @"
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using NexplantQMS.GdsMap;

public static class LabelLayoutProbe
{
    const BindingFlags NP = BindingFlags.NonPublic | BindingFlags.Instance;
    static readonly CultureInfo IC = CultureInfo.InvariantCulture;

    static object F(object o, string name) { return o.GetType().GetField(name, NP).GetValue(o); }
    static object P(object o, string name) { return o.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(o, null); }

    /// <summary>화면 중심(월드 좌표)과 배율(px/µm)을 지정한다.</summary>
    public static void SetView(GdsMapControl map, double cx, double cy, double scale)
    {
        typeof(GdsMapControl).GetField("_scale", NP).SetValue(map, scale);
        typeof(GdsMapControl).GetField("_offset", NP).SetValue(map, new OpenTK.Vector2((float)cx, (float)cy));
    }

    public static double FitScale(GdsMapControl map) { return (double)F(map, "_labelFitScale"); }

    /// <summary>Layer 키 조건에 맞는 Layer 만 켠다.</summary>
    public static void SetVisible(GdsMapControl map, Func<int, bool> visible)
    {
        foreach (object layer in (IEnumerable)F(map, "_layerList"))
            layer.GetType().GetProperty("Visible").SetValue(layer, visible((int)P(layer, "LayerKey")), null);
    }

    public static string Keys(GdsMapControl map)
    {
        var keys = new List<string>();
        foreach (object layer in (IEnumerable)F(map, "_layerList")) keys.Add(GdsLayerKey.Format((int)P(layer, "LayerKey")));
        return string.Join(" ", keys);
    }

    static IEnumerable<object> Labels(GdsMapControl map) { foreach (object l in (IEnumerable)F(map, "_textLabels")) yield return l; }

    public static int Count(GdsMapControl map) { return Labels(map).Count(); }
    public static int Drawable(GdsMapControl map) { return Labels(map).Count(l => !(bool)P(l, "SkipDraw")); }
    public static int NoNeighbor(GdsMapControl map)
    {
        return Labels(map).Count(l => !(bool)P(l, "SkipDraw") && (double)P(l, "AvailableWidth") <= 0 && (double)P(l, "AvailableHeight") <= 0);
    }

    /// <summary>새 계산(GdsMapControl.LayoutTextLabels) 결과를 "글자|X|Y|W|H|px" 목록으로.</summary>
    public static List<string> NewLayout(GdsMapControl map, Graphics g, int w, int h)
    {
        var list = (IList)typeof(GdsMapControl).GetMethod("LayoutTextLabels", NP).Invoke(map, new object[] { g, w, h });
        var r = new List<string>();
        foreach (object p in list)
        {
            var loc = (PointF)P(p, "Location"); var size = (SizeF)P(p, "Size");
            r.Add(string.Format(IC, "{0}|{1:R}|{2:R}|{3:R}|{4:R}|{5}", P(p, "Text"), loc.X, loc.Y, size.Width, size.Height, P(p, "FontPixels")));
        }
        return r;
    }

    // ---- 이전 규칙(2026-10-20 이전 GdsMapControl_Label.cs 의 CalculateTextLabelDisplayAreas / DrawTextLabels)을 그대로 옮긴 비교용 계산 ----
    class Old { public int LayerId, Key; public string Text; public double X, Y, W, H; public string HP, VP; }

    public static List<string> OldLayout(GdsMapControl map, Graphics g, int w, int h)
    {
        var labels = new List<Old>();
        foreach (object l in Labels(map))
        {
            var pos = (GPoint)P(l, "Position");
            labels.Add(new Old { LayerId = (int)P(l, "LayerId"), Key = (int)P(l, "LayerKey"), Text = (string)P(l, "Text"), X = pos.X, Y = pos.Y,
                HP = P(l, "HorizontalPresentation").ToString(), VP = P(l, "VerticalPresentation").ToString() });
        }
        foreach (var row in labels.GroupBy(l => new { l.LayerId, l.Y })) Assign(row.OrderBy(l => l.X).ToList(), true);
        foreach (var col in labels.GroupBy(l => new { l.LayerId, l.X })) Assign(col.OrderBy(l => l.Y).ToList(), false);
        foreach (var l in labels) { if (l.W <= 0 && l.H > 0) l.W = l.H; else if (l.H <= 0 && l.W > 0) l.H = l.W; }

        var visible = new HashSet<int>();
        foreach (object layer in (IEnumerable)F(map, "_layerList")) if ((bool)P(layer, "Visible")) visible.Add((int)P(layer, "LayerKey"));
        double scale = (double)F(map, "_scale"), fit = FitScale(map);
        double ratio = 10.0 * (scale / fit);
        float maxPx = (float)Math.Max(6.0, Math.Min(64.0, ratio));
        var r = new List<string>();
        var occupied = new Dictionary<long, List<RectangleF>>();
        var fonts = new Dictionary<int, Font>();
        var offset = (OpenTK.Vector2)F(map, "_offset");
        using (var maximumFont = new Font("Segoe UI", maxPx, FontStyle.Regular, GraphicsUnit.Pixel))
        {
            foreach (var l in labels)
            {
                if (!visible.Contains(l.Key)) continue;
                float sx = (float)((l.X - offset.X) * scale + map.Width / 2.0), sy = (float)(map.Height / 2.0 - (l.Y - offset.Y) * scale);
                if (sx < 0 || sy < 0 || sx > w || sy > h) continue;
                float aw = l.W > 0 ? (float)(l.W * scale) - 6f : w - 12f;
                float ah = l.H > 0 ? (float)(l.H * scale) - 6f : h - 12f;
                if (aw <= 0 || ah <= 0) continue;
                SizeF ms = g.MeasureString(l.Text, maximumFont);
                float fr = Math.Min(1f, Math.Min(aw * 0.4f / ms.Width, ah * 0.4f / ms.Height));
                int px = (int)Math.Floor(maximumFont.Size * fr);
                if (px < 6) continue;
                Font font; if (!fonts.TryGetValue(px, out font)) { font = new Font("Segoe UI", px, FontStyle.Regular, GraphicsUnit.Pixel); fonts.Add(px, font); }
                SizeF ts = g.MeasureString(l.Text, font);
                if (ts.Width > aw || ts.Height > ah) continue;
                float dx = sx, dy = sy;
                if (l.HP == "Center") dx -= ts.Width / 2f; else if (l.HP == "Right") dx -= ts.Width;
                if (l.VP == "Middle") dy -= ts.Height / 2f; else if (l.VP == "Bottom") dy -= ts.Height;
                var b = new RectangleF(dx, dy, ts.Width, ts.Height);
                if (b.Left < 0 || b.Top < 0 || b.Right > w || b.Bottom > h || Collides(b, occupied)) continue;
                r.Add(string.Format(IC, "{0}|{1:R}|{2:R}|{3:R}|{4:R}|{5}", l.Text, dx, dy, ts.Width, ts.Height, px));
                Reserve(b, occupied);
            }
        }
        foreach (var f in fonts.Values) f.Dispose();
        return r;
    }

    static void Assign(List<Old> s, bool hz)
    {
        for (int i = 0; i < s.Count; i++)
        {
            double c = hz ? s[i].X : s[i].Y, a = double.MaxValue;
            if (i > 0) { double p = hz ? s[i - 1].X : s[i - 1].Y; if (c > p) a = Math.Min(a, c - p); }
            if (i < s.Count - 1) { double n = hz ? s[i + 1].X : s[i + 1].Y; if (n > c) a = Math.Min(a, n - c); }
            if (a == double.MaxValue) continue;
            if (hz) s[i].W = a; else s[i].H = a;
        }
    }

    static bool Collides(RectangleF b, Dictionary<long, List<RectangleF>> occ)
    {
        foreach (long c in Cells(b, true)) { List<RectangleF> list; if (occ.TryGetValue(c, out list)) foreach (var x in list) if (x.IntersectsWith(b)) return true; }
        return false;
    }

    static void Reserve(RectangleF b, Dictionary<long, List<RectangleF>> occ)
    {
        foreach (long c in Cells(b, false)) { List<RectangleF> list; if (!occ.TryGetValue(c, out list)) { list = new List<RectangleF>(); occ.Add(c, list); } list.Add(b); }
    }

    static IEnumerable<long> Cells(RectangleF b, bool around)
    {
        int x0 = (int)Math.Floor(b.Left / 64), x1 = (int)Math.Floor((b.Right - 1) / 64), y0 = (int)Math.Floor(b.Top / 64), y1 = (int)Math.Floor((b.Bottom - 1) / 64);
        if (around) { x0--; x1++; y0--; y1++; }
        for (int x = x0; x <= x1; x++) for (int y = y0; y <= y1; y++) yield return ((long)x << 32) ^ (uint)y;
    }

    /// <summary>라벨 좌표 범위(진단 목록 기준).</summary>
    public static double[] Bounds(GdsMapControl map)
    {
        var d = map.GetTextLabelDiagnostics();
        return new[] { d.Min(x => x.TransformedX), d.Max(x => x.TransformedX), d.Min(x => x.TransformedY), d.Max(x => x.TransformedY) };
    }
}
"@
Add-Type -TypeDefinition $helper -ReferencedAssemblies @((Join-Path $work 'NexplantQMS.GdsMap.dll'), (Join-Path $work 'OpenTK.dll'), (Join-Path $work 'OpenTK.GLControl.dll'),
    'System.Windows.Forms', 'System.Drawing', 'System.Core')

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
$W = 1500; $H = 956
$bmp = New-Object Drawing.Bitmap $W, $H
$g = [Drawing.Graphics]::FromImage($bmp)
$g.TextRenderingHint = [Drawing.Text.TextRenderingHint]::ClearTypeGridFit

function Open-Map($gds, $cell) {
    $reader = New-Object NexplantQMS.GdsMap.GdsReader
    $reader.LengthUnit = [NexplantQMS.GdsMap.LengthUnit]::Micrometer
    $lib = $reader.Read((Join-Path $sample $gds))
    $map = New-Object NexplantQMS.GdsMap.GdsMapControl
    $map.Size = New-Object Drawing.Size $W, $H
    $map.ShowStructure($lib, $cell)
    $map.ZoomToFit()
    return $map
}
function Layout($map, $cx, $cy, $s) { [LabelLayoutProbe]::SetView($map, $cx, $cy, $s); return , [LabelLayoutProbe]::NewLayout($map, $g, $W, $H) }
function Sizes($list) { return @($list | ForEach-Object { ($_ -split '\|')[-1] } | Sort-Object -Unique) }

# 1) OMM: 이전 규칙과 배치 같음
$omm = Open-Map 'OMM all layer2.gds\OMM all layer2.gds' 'TOP'
$fit = [LabelLayoutProbe]::FitScale($omm)
$b = [LabelLayoutProbe]::Bounds($omm)
$centers = @(@((($b[0] + $b[1]) / 2), (($b[2] + $b[3]) / 2)), @(($b[0] + 2000), ($b[2] + 1500)), @(($b[1] - 1500), ($b[3] - 1200)))
$same = 0; $views = 0; $drawn = 0; $firstDiff = ''; $oneSize = 0; $oldSizes = 0
foreach ($c in $centers) {
    foreach ($k in 1, 2.38, 4.77, 10, 20, 40) {
        [LabelLayoutProbe]::SetView($omm, $c[0], $c[1], $fit * $k)
        $old = [LabelLayoutProbe]::OldLayout($omm, $g, $W, $H)
        $new = [LabelLayoutProbe]::NewLayout($omm, $g, $W, $H)
        $views++; $drawn += $old.Count
        $oldTexts = (@($old | ForEach-Object { ($_ -split '\|')[0] }) | Sort-Object) -join ','
        $newTexts = (@($new | ForEach-Object { ($_ -split '\|')[0] }) | Sort-Object) -join ','
        if ($oldTexts -eq $newTexts) { $same++ } elseif ($firstDiff -eq '') { $firstDiff = ("x{0} old {1} / new {2}" -f $k, $old.Count, $new.Count) }
        if ($new.Count -eq 0 -or (Sizes $new).Count -eq 1) { $oneSize++ }
        if ((Sizes $old).Count -gt 1) { $oldSizes++ }
    }
}
Check 'OMM Label 수 / 사본 없음' ([LabelLayoutProbe]::Count($omm) -eq 30992 -and [LabelLayoutProbe]::Drawable($omm) -eq 30992) ("{0} / 그리기 {1}" -f [LabelLayoutProbe]::Count($omm), [LabelLayoutProbe]::Drawable($omm))
Check 'OMM 이전 규칙과 같은 Label 이 보임' ($same -eq $views -and $drawn -gt 0) ("{0}/{1} 화면, 그린 Label {2} {3}" -f $same, $views, $drawn, $firstDiff)
Check 'OMM 화면마다 글자 크기 1종' ($oneSize -eq $views) ("{0}/{1} 화면 (이전 규칙은 {2} 화면에서 여러 크기)" -f $oneSize, $views, $oldSizes)
$omm.Dispose(); [GC]::Collect()

if (-not $SkipU8) {
    # 2) U8 제품 Cell
    $u8 = Open-Map '26_10_20 Sample File U8\OMM_PFIC_PRODUCT_PIC_PKG_A0_0410_v1.gds' 'OMM_PFIC_PIC_PKG_A0'
    Write-Host ('  U8 Layer 키: ' + [LabelLayoutProbe]::Keys($u8))
    Check 'U8 사본은 그리지 않음' ([LabelLayoutProbe]::Count($u8) -eq 274162 -and [LabelLayoutProbe]::Drawable($u8) -eq 137081) ("{0} / 그리기 {1}" -f [LabelLayoutProbe]::Count($u8), [LabelLayoutProbe]::Drawable($u8))
    $nn = [LabelLayoutProbe]::NoNeighbor($u8)
    Check 'U8 화면 크기 기준 칸 20개 이하' ($nn -le 20) "$nn"
    $k183 = [NexplantQMS.GdsMap.GdsLayerKey]::Make(18, 3); $k184 = [NexplantQMS.GdsMap.GdsLayerKey]::Make(18, 4)
    $k503 = [NexplantQMS.GdsMap.GdsLayerKey]::Make(50, 3); $k504 = [NexplantQMS.GdsMap.GdsLayerKey]::Make(50, 4)
    $cx = 16051.0; $cy = 18537.0
    [LabelLayoutProbe]::SetVisible($u8, [Func[int, bool]] { param($k) $k -eq $k183 -or $k -eq $k184 })
    $far = Layout $u8 $cx $cy 0.12; $s034 = Layout $u8 $cx $cy 0.34; $s1 = Layout $u8 $cx $cy 1.0; $s2 = Layout $u8 $cx $cy 2.0
    Check 'U8 멀리(0.12 px/µm) 안 보임' ($far.Count -eq 0) "$($far.Count)"
    Check 'U8 0.34 px/µm(화면 1) 큰 글씨 없음' (@($s034 | Where-Object { [int](($_ -split '\|')[-1]) -gt 12 }).Count -eq 0) ("{0}개 / 크기 {1}" -f $s034.Count, ((Sizes $s034) -join ','))
    Check 'U8 1.0 px/µm 글자 크기 1종' ($s1.Count -gt 50 -and (Sizes $s1).Count -eq 1) ("{0}개 / 크기 {1}" -f $s1.Count, ((Sizes $s1) -join ','))
    Check 'U8 2.0 px/µm 글자 크기 1종' ($s2.Count -gt 10 -and (Sizes $s2).Count -eq 1) ("{0}개 / 크기 {1}" -f $s2.Count, ((Sizes $s2) -join ','))
    $px1 = [int]@(Sizes $s1)[0]; $px2 = [int]@(Sizes $s2)[0]
    Check 'U8 배율 2배면 글자도 약 2배' ([Math]::Abs($px2 - 2 * $px1) -le 2) ("1.0 -> {0}px / 2.0 -> {1}px" -f $px1, $px2)
    $two = @($s2 | Where-Object { $_.Contains("`n") })
    $order = @($two | Where-Object { $parts = ($_ -split '\|')[0] -split "`n"; $parts.Count -eq 2 -and -not $parts[0].StartsWith('C4_') -and $parts[1].StartsWith('C4_') })
    Check 'U8 18/3 + 18/4 = Net 위 / 핀 번호 아래' ($two.Count -gt 0 -and $order.Count -eq $two.Count) ("두 줄 {0} / 순서 맞음 {1} / 예: {2}" -f $two.Count, $order.Count, (($two | Select-Object -First 1) -replace "`n", ' / '))
    [LabelLayoutProbe]::SetVisible($u8, [Func[int, bool]] { param($k) $k -eq $k183 })
    $pin = Layout $u8 $cx $cy 2.0
    Check 'U8 18/3 만 켜면 핀 번호만' ($pin.Count -gt 0 -and @($pin | Where-Object { -not $_.StartsWith('C4_') }).Count -eq 0) "$($pin.Count)개"
    [LabelLayoutProbe]::SetVisible($u8, [Func[int, bool]] { param($k) $k -eq $k184 })
    $net = Layout $u8 $cx $cy 2.0
    Check 'U8 18/4 만 켜면 Net 이름만' ($net.Count -gt 0 -and @($net | Where-Object { $_.Contains('C4_') }).Count -eq 0) "$($net.Count)개"
    [LabelLayoutProbe]::SetVisible($u8, [Func[int, bool]] { param($k) $k -eq $k183 -or $k -eq $k184 -or $k -eq $k503 -or $k -eq $k504 })
    $both = Layout $u8 $cx $cy 1.0
    $dupLine = @($both | Where-Object { $p = ($_ -split '\|')[0] -split "`n"; ($p | Sort-Object -Unique).Count -ne $p.Count })
    $tsv = @($both | Where-Object { $_.Contains('TSV_') })
    Check 'U8 18 + 50 함께: 같은 Net 은 한 줄, TSV 번호 줄 있음' ($both.Count -gt 0 -and $dupLine.Count -eq 0 -and $tsv.Count -gt 0) ("{0}개 / 같은 줄 반복 {1} / TSV 줄 {2} / 크기 {3}" -f $both.Count, $dupLine.Count, $tsv.Count, ((Sizes $both) -join ','))
    # 화면 3 위치(TSV 번호, 빈 자리 옆 Label 이 크게 나오던 곳)
    [LabelLayoutProbe]::SetVisible($u8, [Func[int, bool]] { param($k) $k -eq $k503 })
    $tsvView = Layout $u8 13300.0 21450.0 1.0
    Check 'U8 TSV 구역(50/3) 글자 크기 1종' ($tsvView.Count -gt 20 -and (Sizes $tsvView).Count -eq 1) ("{0}개 / 크기 {1}" -f $tsvView.Count, ((Sizes $tsvView) -join ','))
    $u8.Dispose(); [GC]::Collect()
}
$g.Dispose(); $bmp.Dispose()
$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host "GDS_LABEL_LAYOUT_PROBE=PASS ($($results.Count))" } else { Write-Host "GDS_LABEL_LAYOUT_PROBE=FAIL ($fail)"; exit 1 }
