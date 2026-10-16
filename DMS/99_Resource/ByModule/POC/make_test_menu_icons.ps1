param([string]$OutDir = (Join-Path $PSScriptRoot 'TEST_Menu'))
# TEST Analysis 메뉴 아이콘 (2026-10-14).
# 그림 방식은 make_analysis_icons.ps1 과 같다: 128px 로 그린 뒤 32 / 16 px PNG 로 줄인다.
# 색 규칙: Bin 계열 = Pass 초록 / Fail 빨강 / Not Tested 주황, Parameter 계열 = 파랑 -> 청록 -> 노랑 -> 빨강 값 색.
#   MNU_TEST_MAP_VIEW    : Wafer 1장 + Die 격자(Bin 색)             -> Wafer Map 1장 보기
#   MNU_BIN_MAP_GALLERY  : 작은 Wafer 4장(Bin 색)                    -> Bin Map 여러 장
#   MNU_TEST_MAP_GALLERY : 작은 Wafer 4장(Parameter 값 색)           -> Parameter Map 여러 장
#   MNU_CUM_REPORT       : 겹친 Wafer 3장 + 앞 Wafer 누적 Fail 색   -> Wafer 누적
#   MNU_BIN_WAFER_REPORT : Bin 누적 막대 3개                         -> Bin Stack
#   MNU_YIELD_REPORT     : 수율 추이 선 + % 표시                     -> 수율
#   MNU_PCM_ANALYSIS     : 분포 막대 + 분포 곡선 + LSL / USL 선      -> Parameter 분석
#   MNU_STANDARD_REPORT  : 표가 있는 문서                            -> Parameter 통계 표
#   MNU_TEST_REPORT      : BoxPlot 3개                               -> Parameter 추이 / BoxPlot
# 만든 PNG 는 MenuFile.mnu 의 ICON32 / ICON16 에 16진수 문자열로 넣는다.
# 실행: powershell -File make_test_menu_icons.ps1 [-OutDir <폴더>]
Add-Type -AssemblyName System.Drawing
$ErrorActionPreference = 'Stop'
New-Item -ItemType Directory -Force $OutDir | Out-Null
$S = 128

function C($a, $r, $g, $b) { [Drawing.Color]::FromArgb($a, $r, $g, $b) }
function Brush($c) { New-Object Drawing.SolidBrush($c) }
function NewPen($c, $w) {
    $p = New-Object Drawing.Pen($c, [float]$w)
    $p.LineJoin = 'Round'; $p.StartCap = 'Round'; $p.EndCap = 'Round'
    return $p
}
function NewG($bmp) {
    $g = [Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = 'AntiAlias'; $g.PixelOffsetMode = 'HighQuality'; $g.InterpolationMode = 'HighQualityBicubic'
    return $g
}
function NewCanvas() {
    $b = New-Object Drawing.Bitmap $S, $S
    return @($b, (NewG $b))
}
function LinBrush($x, $y, $w, $h, $c1, $c2, $ang = 90) {
    New-Object Drawing.Drawing2D.LinearGradientBrush((New-Object Drawing.RectangleF($x, $y, $w, $h)), $c1, $c2, [float]$ang)
}
function Pt($x, $y) { New-Object Drawing.PointF([float]$x, [float]$y) }
function Save($bmp, $name) {
    foreach ($sz in 32, 16) {
        $o = New-Object Drawing.Bitmap $sz, $sz
        $g = NewG $o
        $g.DrawImage($bmp, 0, 0, $sz, $sz)
        $g.Dispose()
        $o.Save((Join-Path $OutDir "$name`_$sz.png"), [Drawing.Imaging.ImageFormat]::Png)
        $o.Dispose()
    }
    $bmp.Dispose()
}

# 공통 색
$cPass = C 255 46 173 75
$cFail = C 255 229 56 59
$cNt = C 255 245 165 36
$cEdge = C 255 70 85 110
$cAxis = C 255 70 85 110
$cWaferA = C 255 236 241 248
$cWaferB = C 255 198 208 224
$heat = @((C 255 47 111 214), (C 255 43 179 192), (C 255 242 194 48), (C 255 229 56 59))

# Wafer 원 + 격자. $cellColor 는 (열, 행) -> Color 를 돌려준다. 원 밖 격자는 잘라낸다
function DrawWaferMap($g, $x, $y, $d, $n, $cellColor, $edgeW) {
    $path = New-Object Drawing.Drawing2D.GraphicsPath
    $path.AddEllipse([float]$x, [float]$y, [float]$d, [float]$d)
    $g.FillPath((LinBrush $x $y $d $d $cWaferA $cWaferB), $path)
    $old = $g.Clip
    $g.SetClip($path)
    $cell = $d / $n
    for ($c = 0; $c -lt $n; $c++) {
        for ($r = 0; $r -lt $n; $r++) {
            $col = & $cellColor $c $r
            if ($col -ne $null) {
                $g.FillRectangle((Brush $col), [float]($x + $c * $cell), [float]($y + $r * $cell), [float]$cell, [float]$cell)
            }
        }
    }
    $gridPen = New-Object Drawing.Pen((C 255 255 255 255), [float]([Math]::Max(1.5, $d / 40)))
    for ($i = 1; $i -lt $n; $i++) {
        $g.DrawLine($gridPen, [float]($x + $i * $cell), [float]$y, [float]($x + $i * $cell), [float]($y + $d))
        $g.DrawLine($gridPen, [float]$x, [float]($y + $i * $cell), [float]($x + $d), [float]($y + $i * $cell))
    }
    $g.Clip = $old
    $g.DrawEllipse((NewPen $cEdge $edgeW), [float]$x, [float]$y, [float]$d, [float]$d)
    # Notch
    $nw = $d / 7
    $g.FillPie([Drawing.Brushes]::White, [float]($x + $d / 2 - $nw / 2), [float]($y + $d - $nw / 2), [float]$nw, [float]$nw, 180, 180)
    $path.Dispose()
}

# 값 색이 들어간 Wafer (가운데에서 바깥으로 색이 바뀜)
function DrawHeatWafer($g, $x, $y, $d, $centerColor, $edgeColor, $cx, $cy, $edgeW) {
    $path = New-Object Drawing.Drawing2D.GraphicsPath
    $path.AddEllipse([float]$x, [float]$y, [float]$d, [float]$d)
    $pg = New-Object Drawing.Drawing2D.PathGradientBrush($path)
    $pg.CenterPoint = Pt ($x + $d * $cx) ($y + $d * $cy)
    $pg.CenterColor = $centerColor
    $pg.SurroundColors = [Drawing.Color[]]@($edgeColor)
    $g.FillPath($pg, $path)
    $g.DrawEllipse((NewPen $cEdge $edgeW), [float]$x, [float]$y, [float]$d, [float]$d)
    $pg.Dispose(); $path.Dispose()
}

function DrawAxes($g) {
    $p = NewPen $cAxis 8
    $g.DrawLines($p, [Drawing.PointF[]]@((Pt 12 8), (Pt 12 116), (Pt 122 116)))
}

# 1. Wafer Map 보기: Wafer 1장, 5 x 5 Die, 대부분 Pass + Fail / Not Tested 몇 개
$fail = @{ '1,1' = $cFail; '3,1' = $cNt; '0,2' = $cFail; '3,3' = $cFail; '2,4' = $cNt }
$cv = NewCanvas; $b = $cv[0]; $g = $cv[1]
DrawWaferMap $g 6 6 116 5 { param($c, $r) $k = "$c,$r"; if ($fail.ContainsKey($k)) { $fail[$k] } else { $cPass } } 7
$g.Dispose(); Save $b 'MNU_TEST_MAP_VIEW'

# 2. Bin Map Gallery: 작은 Wafer 4장, Fail 위치가 서로 다름
$cv = NewCanvas; $b = $cv[0]; $g = $cv[1]
$galleryFail = @(
    @{ '0,0' = $cFail; '2,2' = $cFail },
    @{ '2,0' = $cFail; '0,2' = $cNt },
    @{ '1,1' = $cFail },
    @{ '0,1' = $cFail; '2,1' = $cFail; '1,2' = $cNt })
for ($i = 0; $i -lt 4; $i++) {
    $gx = 4 + ($i % 2) * 64; $gy = 4 + [Math]::Floor($i / 2) * 64
    $map = $galleryFail[$i]
    DrawWaferMap $g $gx $gy 56 3 { param($c, $r) $k = "$c,$r"; if ($map.ContainsKey($k)) { $map[$k] } else { $cPass } }.GetNewClosure() 5
}
$g.Dispose(); Save $b 'MNU_BIN_MAP_GALLERY'

# 3. Test Map Gallery: 작은 Wafer 4장, Parameter 값 색(가운데 / 가장자리 색이 서로 다름)
$cv = NewCanvas; $b = $cv[0]; $g = $cv[1]
$heatSet = @(
    @($heat[3], $heat[0], 0.5, 0.5),
    @($heat[2], $heat[0], 0.35, 0.4),
    @($heat[0], $heat[2], 0.5, 0.5),
    @($heat[1], $heat[3], 0.6, 0.55))
for ($i = 0; $i -lt 4; $i++) {
    $gx = 4 + ($i % 2) * 64; $gy = 4 + [Math]::Floor($i / 2) * 64
    $h = $heatSet[$i]
    DrawHeatWafer $g $gx $gy 56 $h[0] $h[1] $h[2] $h[3] 5
}
$g.Dispose(); Save $b 'MNU_TEST_MAP_GALLERY'

# 4. Wafer 누적: 겹친 Wafer 3장 + 앞 Wafer 는 가장자리 Fail 이 많은 누적 색
$cv = NewCanvas; $b = $cv[0]; $g = $cv[1]
foreach ($o in @(@(38, 4, 150), @(21, 21, 200))) {
    $g.FillEllipse((Brush (C 255 $o[2] ($o[2] + 15) ([Math]::Min(255, $o[2] + 35)))), [float]$o[0], [float]$o[1], 86, 86)
    $g.DrawEllipse((NewPen $cEdge 5), [float]$o[0], [float]$o[1], 86, 86)
}
DrawWaferMap $g 4 38 86 5 {
    param($c, $r)
    $dist = [Math]::Sqrt([Math]::Pow($c - 2, 2) + [Math]::Pow($r - 2, 2))
    if ($dist -ge 2) { $cFail } elseif ($dist -ge 1.4) { $cNt } else { $cPass }
} 6
$g.Dispose(); Save $b 'MNU_CUM_REPORT'

# 5. Bin Stack: Bin 누적 막대 3개 (아래 Pass, 위 Not Tested / Fail)
$cv = NewCanvas; $b = $cv[0]; $g = $cv[1]
DrawAxes $g
$bars = @(@(26, 64, 14, 12), @(58, 78, 10, 8), @(90, 52, 20, 18))
$bh = 104
foreach ($bar in $bars) {
    $x = $bar[0]; $y = $bh
    foreach ($seg in @(@($bar[1], $cPass), @($bar[2], $cNt), @($bar[3], $cFail))) {
        $y -= $seg[0]
        $g.FillRectangle((Brush $seg[1]), [float]$x, [float]$y, 24, [float]$seg[0])
    }
    $g.DrawRectangle((NewPen $cEdge 3), [float]$x, [float]$y, 24, [float]($bh - $y))
}
$g.Dispose(); Save $b 'MNU_BIN_WAFER_REPORT'

# 6. 수율: 올라가는 추이 선 + 점 + 왼쪽 위 % 표시
$cv = NewCanvas; $b = $cv[0]; $g = $cv[1]
DrawAxes $g
$pts = [Drawing.PointF[]]@((Pt 26 94), (Pt 50 74), (Pt 72 82), (Pt 96 50), (Pt 116 30))
$g.DrawLines((NewPen $cPass 9), $pts)
foreach ($p in $pts) {
    $g.FillEllipse([Drawing.Brushes]::White, [float]($p.X - 8), [float]($p.Y - 8), 16, 16)
    $g.DrawEllipse((NewPen (C 255 30 130 55) 5), [float]($p.X - 8), [float]($p.Y - 8), 16, 16)
}
# % 는 글꼴 대신 도형으로 그린다(16px 에서도 보이게)
$g.FillEllipse((Brush (C 255 47 111 214)), 16, 0, 54, 54)
$g.DrawLine((NewPen (C 255 255 255 255) 8), 55, 13, 31, 41)
$g.FillEllipse([Drawing.Brushes]::White, 24, 9, 14, 14)
$g.FillEllipse([Drawing.Brushes]::White, 48, 31, 14, 14)
$g.Dispose(); Save $b 'MNU_YIELD_REPORT'

# 7. Parameter 분석: 분포 막대(파랑) + 분포 곡선 + 빨간 LSL / USL 선
$cv = NewCanvas; $b = $cv[0]; $g = $cv[1]
DrawAxes $g
$hist = @(14, 34, 62, 80, 58, 30, 12)
for ($i = 0; $i -lt $hist.Count; $i++) {
    $x = 22 + $i * 14; $h = $hist[$i]
    $g.FillRectangle((LinBrush $x (110 - $h) 13 $h $heat[1] $heat[0]), [float]$x, [float](110 - $h), 12, [float]$h)
}
$curve = New-Object 'System.Collections.Generic.List[Drawing.PointF]'
for ($x = 18; $x -le 122; $x += 4) {
    $z = ($x - 70) / 18.0
    $curve.Add((Pt $x (108 - 84 * [Math]::Exp(-0.5 * $z * $z))))
}
$g.DrawCurve((NewPen (C 255 25 45 90) 6), $curve.ToArray())
$spec = NewPen $cFail 6
$g.DrawLine($spec, 20, 14, 20, 110); $g.DrawLine($spec, 120, 14, 120, 110)
$g.Dispose(); Save $b 'MNU_PCM_ANALYSIS'

# 8. 통계 표 문서: 접힌 귀퉁이 문서 + 파란 머리글 + 표 줄
$cv = NewCanvas; $b = $cv[0]; $g = $cv[1]
$doc = New-Object Drawing.Drawing2D.GraphicsPath
$doc.AddLines([Drawing.PointF[]]@((Pt 18 4), (Pt 86 4), (Pt 112 30), (Pt 112 124), (Pt 18 124)))
$doc.CloseFigure()
$g.FillPath((LinBrush 18 4 94 120 (C 255 255 255 255) (C 255 232 238 246)), $doc)
$g.DrawPath((NewPen $cEdge 6), $doc)
$g.FillPolygon((Brush (C 255 200 210 225)), [Drawing.PointF[]]@((Pt 86 4), (Pt 86 30), (Pt 112 30)))
$g.DrawLines((NewPen $cEdge 4), [Drawing.PointF[]]@((Pt 86 6), (Pt 86 30), (Pt 110 30)))
$g.FillRectangle((Brush $heat[0]), 30, 40, 70, 16)
$row = NewPen (C 255 150 165 190) 4
foreach ($y in 70, 86, 102) { $g.DrawLine($row, 30, $y, 100, $y) }
$g.DrawLine($row, 58, 56, 58, 112)
$g.FillRectangle((Brush $heat[2]), 64, 76, 30, 8)
$g.Dispose(); Save $b 'MNU_STANDARD_REPORT'

# 9. BoxPlot: 상자 3개 + 수염 + 중앙값
$cv = NewCanvas; $b = $cv[0]; $g = $cv[1]
DrawAxes $g
$boxes = @(@(30, 30, 50, 80, 100, 66), @(62, 16, 34, 62, 84, 46), @(94, 40, 58, 86, 104, 72))
$whisker = NewPen $cEdge 5
foreach ($bx in $boxes) {
    $x = $bx[0]; $cx = $x + 12
    $g.DrawLine($whisker, $cx, $bx[1], $cx, $bx[4])
    $g.DrawLine($whisker, $cx - 7, $bx[1], $cx + 7, $bx[1])
    $g.DrawLine($whisker, $cx - 7, $bx[4], $cx + 7, $bx[4])
    $g.FillRectangle((LinBrush $x $bx[2] 24 ($bx[3] - $bx[2]) $heat[1] $heat[0]), [float]$x, [float]$bx[2], 24, [float]($bx[3] - $bx[2]))
    $g.DrawRectangle((NewPen $cEdge 4), [float]$x, [float]$bx[2], 24, [float]($bx[3] - $bx[2]))
    $g.DrawLine((NewPen (C 255 255 255 255) 5), $x + 3, $bx[5], $x + 21, $bx[5])
}
$g.Dispose(); Save $b 'MNU_TEST_REPORT'

Get-ChildItem $OutDir -Filter 'MNU_*.png' | Sort-Object Name | Select-Object Name, Length
