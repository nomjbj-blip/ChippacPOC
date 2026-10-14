param([string]$OutDir)
# 아이콘 없는 메뉴 4개(GDS Map Setup / GDS Map View / E-Test Result Upload / E-Test Limit Setup)의 아이콘을 128px 로 그린 뒤 32 / 16 으로 줄여 PNG 로 저장한다.
# 만든 PNG 는 공백 구분 16진수 문자열로 바꿔 상위 폴더 MenuFile.mnu 의 ICON32 / ICON16 에 넣는다 (DACrux.Base.Convert.StringToImage 형식). (2026-10-08)
# 실행: powershell -File make_icons.ps1 -OutDir <폴더>
Add-Type -AssemblyName System.Drawing
$ErrorActionPreference = 'Stop'
New-Item -ItemType Directory -Force $OutDir | Out-Null
$S = 128

function C($a, $r, $g, $b) { [Drawing.Color]::FromArgb($a, $r, $g, $b) }
function RR([float]$x, [float]$y, [float]$w, [float]$h, [float]$r) {
    $p = New-Object Drawing.Drawing2D.GraphicsPath
    $d = $r * 2
    $p.AddArc($x, $y, $d, $d, 180, 90); $p.AddArc($x + $w - $d, $y, $d, $d, 270, 90)
    $p.AddArc($x + $w - $d, $y + $h - $d, $d, $d, 0, 90); $p.AddArc($x, $y + $h - $d, $d, $d, 90, 90)
    $p.CloseFigure(); return $p
}
function NewG($bmp) {
    $g = [Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = 'AntiAlias'; $g.PixelOffsetMode = 'HighQuality'; $g.InterpolationMode = 'HighQualityBicubic'
    return $g
}
function LinBrush($x, $y, $w, $h, $c1, $c2, $ang = 90) {
    New-Object Drawing.Drawing2D.LinearGradientBrush((New-Object Drawing.RectangleF($x, $y, $w, $h)), $c1, $c2, [float]$ang)
}

# 문서(페이지) 바탕 - 기존 Map Define 류 아이콘처럼 흰 종이 + 접힌 모서리
function Page($g, $x, $y, $w, $h) {
    $fold = 22
    $p = New-Object Drawing.Drawing2D.GraphicsPath
    $p.AddLines([Drawing.PointF[]]@(
        (New-Object Drawing.PointF($x, $y)), (New-Object Drawing.PointF(($x + $w - $fold), $y)),
        (New-Object Drawing.PointF(($x + $w), ($y + $fold))), (New-Object Drawing.PointF(($x + $w), ($y + $h))),
        (New-Object Drawing.PointF($x, ($y + $h)))))
    $p.CloseFigure()
    $g.FillPath((LinBrush $x $y $w $h (C 255 255 255 255) (C 255 226 232 240)), $p)
    $g.DrawPath((New-Object Drawing.Pen((C 255 120 130 145), 3)), $p)
    $f = New-Object Drawing.Drawing2D.GraphicsPath
    $f.AddLines([Drawing.PointF[]]@((New-Object Drawing.PointF(($x + $w - $fold), $y)), (New-Object Drawing.PointF(($x + $w - $fold), ($y + $fold))), (New-Object Drawing.PointF(($x + $w), ($y + $fold)))))
    $f.CloseFigure()
    $g.FillPath((New-Object Drawing.SolidBrush((C 255 200 208 220))), $f)
    $g.DrawPath((New-Object Drawing.Pen((C 255 120 130 145), 3)), $f)
}

# 톱니바퀴 - 기존 Setup 아이콘의 회색 기어
function Gear($g, $cx, $cy, $r) {
    $p = New-Object Drawing.Drawing2D.GraphicsPath
    $teeth = 8; $pts = New-Object 'System.Collections.Generic.List[Drawing.PointF]'
    for ($i = 0; $i -lt $teeth * 4; $i++) {
        $a = [Math]::PI * 2 * $i / ($teeth * 4)
        $rr = if (($i % 4) -lt 2) { $r } else { $r * 0.78 }
        $pts.Add((New-Object Drawing.PointF(($cx + $rr * [Math]::Cos($a)), ($cy + $rr * [Math]::Sin($a)))))
    }
    $p.AddPolygon($pts.ToArray())
    $g.FillPath((LinBrush ($cx - $r) ($cy - $r) ($r * 2) ($r * 2) (C 255 205 210 218) (C 255 105 112 125)), $p)
    $g.DrawPath((New-Object Drawing.Pen((C 255 70 76 88), 3)), $p)
    $h = $r * 0.36
    $g.FillEllipse((New-Object Drawing.SolidBrush((C 255 245 247 250))), ($cx - $h), ($cy - $h), ($h * 2), ($h * 2))
    $g.DrawEllipse((New-Object Drawing.Pen((C 255 70 76 88), 3)), ($cx - $h), ($cy - $h), ($h * 2), ($h * 2))
}

# GDS Layout 도형 - 여러 Layer 색의 사각 / L 자 Polygon
function Layout($g, $x, $y, $w, $h, $alpha) {
    $cols = @((C $alpha 40 110 220), (C $alpha 30 170 90), (C $alpha 240 140 30), (C $alpha 210 50 60))
    $rects = @(
        @(0.05, 0.08, 0.40, 0.14, 0), @(0.55, 0.08, 0.40, 0.14, 1), @(0.05, 0.30, 0.14, 0.60, 2),
        @(0.27, 0.30, 0.46, 0.12, 0), @(0.81, 0.30, 0.14, 0.60, 3), @(0.27, 0.50, 0.20, 0.40, 1),
        @(0.53, 0.50, 0.20, 0.18, 3), @(0.53, 0.74, 0.20, 0.16, 2))
    foreach ($r in $rects) {
        $rx = $x + $w * $r[0]; $ry = $y + $h * $r[1]; $rw = $w * $r[2]; $rh = $h * $r[3]
        $g.FillRectangle((New-Object Drawing.SolidBrush($cols[$r[4]])), [float]$rx, [float]$ry, [float]$rw, [float]$rh)
        $g.DrawRectangle((New-Object Drawing.Pen((C 255 30 40 60), 1.6)), [float]$rx, [float]$ry, [float]$rw, [float]$rh)
    }
}

# 웨이퍼 - 기존 Map 아이콘처럼 원판 + 색 Die 격자
function Wafer($g, $cx, $cy, $r) {
    $clip = New-Object Drawing.Drawing2D.GraphicsPath
    $clip.AddEllipse(($cx - $r), ($cy - $r), ($r * 2), ($r * 2))
    $g.FillPath((LinBrush ($cx - $r) ($cy - $r) ($r * 2) ($r * 2) (C 255 250 250 252) (C 255 190 196 206) 45), $clip)
    $old = $g.Clip; $g.SetClip($clip)
    $n = 7; $cell = ($r * 2 * 0.86) / $n; $ox = $cx - $cell * $n / 2; $oy = $cy - $cell * $n / 2
    $rand = New-Object Random 7
    $pal = @((C 255 255 200 40), (C 255 250 150 40), (C 255 235 90 120), (C 255 120 200 80), (C 255 255 220 90))
    for ($i = 0; $i -lt $n; $i++) { for ($j = 0; $j -lt $n; $j++) {
        $dx = $ox + $i * $cell + $cell / 2 - $cx; $dy = $oy + $j * $cell + $cell / 2 - $cy
        if ([Math]::Sqrt($dx * $dx + $dy * $dy) -gt $r * 0.92) { continue }
        $g.FillRectangle((New-Object Drawing.SolidBrush($pal[$rand.Next($pal.Count)])), [float]($ox + $i * $cell + 1.5), [float]($oy + $j * $cell + 1.5), [float]($cell - 3), [float]($cell - 3))
    } }
    $g.Clip = $old
    $g.DrawEllipse((New-Object Drawing.Pen((C 255 90 70 120), 4)), ($cx - $r), ($cy - $r), ($r * 2), ($r * 2))
    # 광택
    $gl = New-Object Drawing.Drawing2D.GraphicsPath
    $gl.AddEllipse(($cx - $r * 0.8), ($cy - $r * 0.95), ($r * 1.6), ($r * 0.9))
    $g.FillPath((LinBrush ($cx - $r) ($cy - $r) ($r * 2) ($r) (C 120 255 255 255) (C 0 255 255 255)), $gl)
}

function Badge($g, $cx, $cy, $r, $c1, $c2) {
    $g.FillEllipse((LinBrush ($cx - $r) ($cy - $r) ($r * 2) ($r * 2) $c1 $c2), ($cx - $r), ($cy - $r), ($r * 2), ($r * 2))
    $g.DrawEllipse((New-Object Drawing.Pen((C 255 255 255 255), 4)), ($cx - $r), ($cy - $r), ($r * 2), ($r * 2))
}

function Save($bmp, $name) {
    foreach ($sz in 32, 16) {
        $o = New-Object Drawing.Bitmap $sz, $sz
        $g = NewG $o
        $g.DrawImage($bmp, 0, 0, $sz, $sz)
        $g.Dispose()
        $o.Save((Join-Path $OutDir "$name`_$sz.png"), [Drawing.Imaging.ImageFormat]::Png)
    }
}

# 1. GDS Map Setup : Layout 이 그려진 문서 + 톱니바퀴 (기존 Map Define 류와 같은 문서 + 설정 구성)
$b = New-Object Drawing.Bitmap $S, $S; $g = NewG $b
Page $g 10 4 84 108
Layout $g 20 26 64 70 255
Gear $g 92 94 30
$g.Dispose(); Save $b 'GDS_MAP_SETUP'

# 2. GDS Map View : Chip(Die) 위 Layout + 돋보기
$b = New-Object Drawing.Bitmap $S, $S; $g = NewG $b
$chip = RR 6 6 100 100 10
$g.FillPath((LinBrush 6 6 100 100 (C 255 60 72 95) (C 255 22 28 42)), $chip)
$g.DrawPath((New-Object Drawing.Pen((C 255 15 18 28), 3)), $chip)
Layout $g 16 16 80 80 235
# 돋보기
$g.DrawLine((New-Object Drawing.Pen((C 255 70 50 30), 14)), 98, 98, 120, 120)
$g.DrawLine((New-Object Drawing.Pen((C 255 160 110 50), 9)), 99, 99, 119, 119)
$g.FillEllipse((New-Object Drawing.SolidBrush((C 150 220 240 255))), 58, 58, 46, 46)
$g.DrawEllipse((New-Object Drawing.Pen((C 255 50 60 80), 7)), 58, 58, 46, 46)
$g.Dispose(); Save $b 'GDS_MAP_VIEW'

# 3. E-Test Result Upload : 웨이퍼 + 녹색 위 화살표 (KLARF Upload 의 웨이퍼 계열과 맞춤)
$b = New-Object Drawing.Bitmap $S, $S; $g = NewG $b
Wafer $g 56 56 50
Badge $g 96 96 28 (C 255 110 210 90) (C 255 25 140 45)
$arrow = [Drawing.PointF[]]@(
    (New-Object Drawing.PointF(96, 76)), (New-Object Drawing.PointF(114, 96)), (New-Object Drawing.PointF(103, 96)),
    (New-Object Drawing.PointF(103, 114)), (New-Object Drawing.PointF(89, 114)), (New-Object Drawing.PointF(89, 96)),
    (New-Object Drawing.PointF(78, 96)))
$g.FillPolygon([Drawing.Brushes]::White, $arrow)
$g.Dispose(); Save $b 'ET_RESULT_UPLOAD'

# 4. E-Test Limit Setup : Chart(데이터 선 + 빨간 USL / LSL 점선) + 톱니바퀴
$b = New-Object Drawing.Bitmap $S, $S; $g = NewG $b
$panel = RR 4 10 104 92 8
$g.FillPath((LinBrush 4 10 104 92 (C 255 255 255 255) (C 255 222 230 242)), $panel)
$g.DrawPath((New-Object Drawing.Pen((C 255 90 105 130), 3)), $panel)
$ax = New-Object Drawing.Pen((C 255 90 100 115), 3)
$g.DrawLine($ax, 16, 22, 16, 90); $g.DrawLine($ax, 16, 90, 98, 90)
$lim = New-Object Drawing.Pen((C 255 220 40 40), 4); $lim.DashStyle = 'Dash'
$g.DrawLine($lim, 18, 34, 98, 34); $g.DrawLine($lim, 18, 76, 98, 76)
$pts = [Drawing.PointF[]]@((New-Object Drawing.PointF(20, 60)), (New-Object Drawing.PointF(34, 46)), (New-Object Drawing.PointF(48, 62)),
    (New-Object Drawing.PointF(62, 50)), (New-Object Drawing.PointF(76, 66)), (New-Object Drawing.PointF(92, 52)))
$g.DrawLines((New-Object Drawing.Pen((C 255 30 100 210), 5)), $pts)
foreach ($p in $pts) { $g.FillEllipse((New-Object Drawing.SolidBrush((C 255 30 100 210))), ($p.X - 4), ($p.Y - 4), 8, 8) }
Gear $g 96 96 28
$g.Dispose(); Save $b 'ET_LIMIT_SETUP'

# ---- 메뉴 바(ROOT) 아이콘 3개 (2026-10-08) ----

# 5. Setup : 큰 파란 톱니 + 작은 주황 톱니
function GearC($g, $cx, $cy, $r, $c1, $c2, $edge) {
    $p = New-Object Drawing.Drawing2D.GraphicsPath
    $teeth = 8; $pts = New-Object 'System.Collections.Generic.List[Drawing.PointF]'
    for ($i = 0; $i -lt $teeth * 4; $i++) {
        $a = [Math]::PI * 2 * $i / ($teeth * 4)
        $rr = if (($i % 4) -lt 2) { $r } else { $r * 0.76 }
        $pts.Add((New-Object Drawing.PointF(($cx + $rr * [Math]::Cos($a)), ($cy + $rr * [Math]::Sin($a)))))
    }
    $p.AddPolygon($pts.ToArray())
    $g.FillPath((LinBrush ($cx - $r) ($cy - $r) ($r * 2) ($r * 2) $c1 $c2 45), $p)
    $g.DrawPath((New-Object Drawing.Pen($edge, 4)), $p)
    $h = $r * 0.34
    $g.FillEllipse((New-Object Drawing.SolidBrush((C 255 250 250 252))), ($cx - $h), ($cy - $h), ($h * 2), ($h * 2))
    $g.DrawEllipse((New-Object Drawing.Pen($edge, 4)), ($cx - $h), ($cy - $h), ($h * 2), ($h * 2))
}
$b = New-Object Drawing.Bitmap $S, $S; $g = NewG $b
GearC $g 52 54 46 (C 255 120 175 245) (C 255 25 85 185) (C 255 20 55 120)
GearC $g 96 96 28 (C 255 255 200 90) (C 255 225 120 20) (C 255 140 70 10)
$g.Dispose(); Save $b 'ROOT_SETUP'

# 6. DMS Analysis : Defect Map (웨이퍼 위 빨강 / 주황 Defect 점) + 돋보기
$b = New-Object Drawing.Bitmap $S, $S; $g = NewG $b
$g.FillEllipse((LinBrush 6 6 104 104 (C 255 235 242 252) (C 255 160 180 215) 45), 6, 6, 104, 104)
$grid = New-Object Drawing.Pen((C 90 90 110 150), 2)
for ($k = 22; $k -le 94; $k += 18) { $g.DrawLine($grid, $k, 12, $k, 104); $g.DrawLine($grid, 12, $k, 104, $k) }
$g.DrawEllipse((New-Object Drawing.Pen((C 255 40 60 110), 5)), 6, 6, 104, 104)
$rand = New-Object Random 11
for ($k = 0; $k -lt 26; $k++) {
    $a = $rand.NextDouble() * [Math]::PI * 2; $rr = [Math]::Sqrt($rand.NextDouble()) * 44
    $dx = 58 + $rr * [Math]::Cos($a); $dy = 58 + $rr * [Math]::Sin($a)
    $col = if ($k % 3 -eq 0) { C 255 245 160 20 } else { C 255 225 30 40 }
    $g.FillEllipse((New-Object Drawing.SolidBrush($col)), ($dx - 5), ($dy - 5), 10, 10)
}
$g.DrawLine((New-Object Drawing.Pen((C 255 70 50 30), 14)), 100, 100, 122, 122)
$g.DrawLine((New-Object Drawing.Pen((C 255 160 110 50), 9)), 101, 101, 121, 121)
$g.FillEllipse((New-Object Drawing.SolidBrush((C 170 225 245 255))), 62, 62, 46, 46)
$g.DrawEllipse((New-Object Drawing.Pen((C 255 50 60 80), 7)), 62, 62, 46, 46)
$g.Dispose(); Save $b 'ROOT_DMS'

# 7. TEST Analysis : Bin Map 웨이퍼(녹색 Pass / 빨강 Fail Die) + 막대 Chart
$b = New-Object Drawing.Bitmap $S, $S; $g = NewG $b
$clip = New-Object Drawing.Drawing2D.GraphicsPath; $clip.AddEllipse(4, 4, 100, 100)
$g.FillPath((New-Object Drawing.SolidBrush((C 255 235 240 235))), $clip)
$old = $g.Clip; $g.SetClip($clip)
$rand = New-Object Random 5; $cell = 12.5
for ($i = 0; $i -lt 8; $i++) { for ($j = 0; $j -lt 8; $j++) {
    $x = 4 + $i * $cell; $y = 4 + $j * $cell
    $col = if ($rand.Next(9) -eq 0) { C 255 225 40 45 } elseif ($rand.Next(12) -eq 0) { C 255 250 200 40 } else { C 255 60 185 80 }
    $g.FillRectangle((New-Object Drawing.SolidBrush($col)), [float]($x + 1.2), [float]($y + 1.2), [float]($cell - 2.4), [float]($cell - 2.4))
} }
$g.Clip = $old
$g.DrawEllipse((New-Object Drawing.Pen((C 255 30 90 50), 5)), 4, 4, 100, 100)
$panel = RR 70 66 54 56 6
$g.FillPath((LinBrush 70 66 54 56 (C 255 255 255 255) (C 255 220 228 240)), $panel)
$g.DrawPath((New-Object Drawing.Pen((C 255 60 75 100), 4)), $panel)
$g.FillRectangle((LinBrush 78 96 10 20 (C 255 120 175 245) (C 255 25 85 185)), 78, 96, 10, 20)
$g.FillRectangle((LinBrush 92 82 10 34 (C 255 120 175 245) (C 255 25 85 185)), 92, 82, 10, 34)
$g.FillRectangle((LinBrush 106 74 10 42 (C 255 255 200 90) (C 255 225 120 20)), 106, 74, 10, 42)
$g.Dispose(); Save $b 'ROOT_TEST'

# 8. POC : 실험 플라스크(초록 / 파랑 액체 + 거품) + 반짝임 별 - 새로 검증하는 화면 묶음이라는 뜻 (2026-10-08)
$b = New-Object Drawing.Bitmap $S, $S; $g = NewG $b
$fl = New-Object Drawing.Drawing2D.GraphicsPath
$fl.AddLines([Drawing.PointF[]]@(
    (New-Object Drawing.PointF(46, 10)), (New-Object Drawing.PointF(74, 10)), (New-Object Drawing.PointF(74, 46)),
    (New-Object Drawing.PointF(110, 108)), (New-Object Drawing.PointF(104, 118)), (New-Object Drawing.PointF(16, 118)),
    (New-Object Drawing.PointF(10, 108)), (New-Object Drawing.PointF(46, 46))))
$fl.CloseFigure()
$g.FillPath((LinBrush 10 10 100 108 (C 200 245 250 255) (C 200 210 225 240) 0), $fl)
$old = $g.Clip; $g.SetClip($fl)
$g.FillRectangle((LinBrush 0 66 128 62 (C 255 90 220 140) (C 255 20 120 200) 90), 0, 66, 128, 62)
$bub = New-Object Drawing.SolidBrush((C 200 255 255 255))
$g.FillEllipse($bub, 40, 84, 12, 12); $g.FillEllipse($bub, 62, 96, 8, 8); $g.FillEllipse($bub, 74, 78, 10, 10)
$g.Clip = $old
$g.DrawPath((New-Object Drawing.Pen((C 255 40 60 100), 6)), $fl)
$g.DrawLine((New-Object Drawing.Pen((C 255 40 60 100), 7)), 40, 10, 80, 10)
$star = [Drawing.PointF[]]@(
    (New-Object Drawing.PointF(104, 6)), (New-Object Drawing.PointF(109, 21)), (New-Object Drawing.PointF(124, 26)),
    (New-Object Drawing.PointF(109, 31)), (New-Object Drawing.PointF(104, 46)), (New-Object Drawing.PointF(99, 31)),
    (New-Object Drawing.PointF(84, 26)), (New-Object Drawing.PointF(99, 21)))
$g.FillPolygon((LinBrush 84 6 40 40 (C 255 255 230 80) (C 255 245 150 20)), $star)
$g.DrawPolygon((New-Object Drawing.Pen((C 255 170 100 10), 3)), $star)
$g.Dispose(); Save $b 'ROOT_POC'

# ---- 같은 아이콘을 함께 쓰던 메뉴를 성격별로 나눈 아이콘 7개 (2026-10-08) ----

# 공통: 오른쪽 아래 돋보기
function Magnifier($g, $x, $y, $d) {
    $g.DrawLine((New-Object Drawing.Pen((C 255 70 50 30), 14)), ($x + $d * 0.85), ($y + $d * 0.85), ($x + $d * 1.32), ($y + $d * 1.32))
    $g.DrawLine((New-Object Drawing.Pen((C 255 160 110 50), 9)), ($x + $d * 0.87), ($y + $d * 0.87), ($x + $d * 1.30), ($y + $d * 1.30))
    $g.FillEllipse((New-Object Drawing.SolidBrush((C 170 225 245 255))), $x, $y, $d, $d)
    $g.DrawEllipse((New-Object Drawing.Pen((C 255 50 60 80), 7)), $x, $y, $d, $d)
}
# 공통: 오른쪽 아래 녹색 업로드 배지
function UploadBadge($g) {
    Badge $g 96 96 28 (C 255 110 210 90) (C 255 25 140 45)
    $arrow = [Drawing.PointF[]]@(
        (New-Object Drawing.PointF(96, 76)), (New-Object Drawing.PointF(114, 96)), (New-Object Drawing.PointF(103, 96)),
        (New-Object Drawing.PointF(103, 114)), (New-Object Drawing.PointF(89, 114)), (New-Object Drawing.PointF(89, 96)),
        (New-Object Drawing.PointF(78, 96)))
    $g.FillPolygon([Drawing.Brushes]::White, $arrow)
}
# 공통: KLARF 문서 (문서 + 위쪽 남색 띠 "KLARF" + Defect 목록 줄 / 빨간 점)
function KlarfDoc($g) {
    Page $g 8 4 88 112
    $band = RR 14 14 72 26 4
    $g.FillPath((LinBrush 14 14 72 26 (C 255 60 90 160) (C 255 20 40 95)), $band)
    $sf = New-Object Drawing.StringFormat; $sf.Alignment = 'Center'; $sf.LineAlignment = 'Center'
    $g.TextRenderingHint = 'AntiAliasGridFit'
    $g.DrawString('KLARF', (New-Object Drawing.Font('Arial Black', 15, [Drawing.FontStyle]::Bold, [Drawing.GraphicsUnit]::Pixel)), [Drawing.Brushes]::White, (New-Object Drawing.RectangleF(14, 14, 72, 26)), $sf)
    $ln = New-Object Drawing.Pen((C 255 150 160 175), 5)
    $dot = New-Object Drawing.SolidBrush((C 255 220 35 45))
    foreach ($yy in 54, 70, 86, 102) { $g.FillEllipse($dot, 18, ($yy - 5), 10, 10); $g.DrawLine($ln, 34, $yy, 78, $yy) }
}

# 9. Map Define (Setup > TEST, frmSetupMap) : 빈 Die 격자 웨이퍼 + 연필 (Map 모양을 정의)
$b = New-Object Drawing.Bitmap $S, $S; $g = NewG $b
$clip = New-Object Drawing.Drawing2D.GraphicsPath; $clip.AddEllipse(6, 6, 100, 100)
$g.FillPath((LinBrush 6 6 100 100 (C 255 245 250 255) (C 255 190 210 235) 45), $clip)
$old = $g.Clip; $g.SetClip($clip)
$gp = New-Object Drawing.Pen((C 255 70 110 180), 3)
for ($k = 18; $k -le 100; $k += 14) { $g.DrawLine($gp, $k, 0, $k, 128); $g.DrawLine($gp, 0, $k, 128, $k) }
$g.FillRectangle((New-Object Drawing.SolidBrush((C 255 90 170 245))), 46, 46, 14, 14)
$g.Clip = $old
$g.DrawEllipse((New-Object Drawing.Pen((C 255 30 60 120), 5)), 6, 6, 100, 100)
$st = $g.Save(); $g.TranslateTransform(86, 84); $g.RotateTransform(45)
$g.FillRectangle((LinBrush -9 -36 18 50 (C 255 255 215 80) (C 255 235 160 20) 0), -9, -36, 18, 50)
$g.DrawRectangle((New-Object Drawing.Pen((C 255 130 80 10), 3)), -9, -36, 18, 50)
$g.FillRectangle((New-Object Drawing.SolidBrush((C 255 240 130 150))), -9, -46, 18, 10)
$g.DrawRectangle((New-Object Drawing.Pen((C 255 130 80 10), 3)), -9, -46, 18, 10)
$tip = [Drawing.PointF[]]@((New-Object Drawing.PointF(-9, 14)), (New-Object Drawing.PointF(9, 14)), (New-Object Drawing.PointF(0, 32)))
$g.FillPolygon((New-Object Drawing.SolidBrush((C 255 240 210 170))), $tip)
$g.FillPolygon((New-Object Drawing.SolidBrush((C 255 40 40 40))), [Drawing.PointF[]]@((New-Object Drawing.PointF(-3, 26)), (New-Object Drawing.PointF(3, 26)), (New-Object Drawing.PointF(0, 32))))
$g.Restore($st)
$g.Dispose(); Save $b 'MAP_DEFINE'

# 10. Defect Coloring : 물감 팔레트(Defect 색 지정)
$b = New-Object Drawing.Bitmap $S, $S; $g = NewG $b
$pal = New-Object Drawing.Drawing2D.GraphicsPath
$pal.AddEllipse(6, 14, 116, 96)
$hole = New-Object Drawing.Drawing2D.GraphicsPath; $hole.AddEllipse(72, 72, 20, 18)
$reg = New-Object Drawing.Region($pal); $reg.Exclude($hole)
$g.FillRegion((LinBrush 6 14 116 96 (C 255 250 225 185) (C 255 205 155 95) 45), $reg)
$g.DrawPath((New-Object Drawing.Pen((C 255 120 80 35), 5)), $pal)
$g.DrawEllipse((New-Object Drawing.Pen((C 255 120 80 35), 4)), 72, 72, 20, 18)
$cols = @(@(24, 40, (C 255 225 35 45)), @(50, 26, (C 255 250 190 30)), @(80, 30, (C 255 50 170 70)), @(98, 52, (C 255 40 110 225)), @(28, 70, (C 255 150 60 200)))
foreach ($c in $cols) {
    $g.FillEllipse((New-Object Drawing.SolidBrush($c[2])), $c[0], $c[1], 22, 22)
    $g.DrawEllipse((New-Object Drawing.Pen((C 160 60 40 20), 2)), $c[0], $c[1], 22, 22)
}
$g.Dispose(); Save $b 'DEFECT_COLORING'

# 11. Defect Group Define : 묶음 상자 안에 여러 색 Defect 모양 + 톱니 (Type 을 Group 으로 묶음)
$b = New-Object Drawing.Bitmap $S, $S; $g = NewG $b
$box = RR 6 10 96 86 10
$g.FillPath((LinBrush 6 10 96 86 (C 255 240 246 255) (C 255 190 205 230)), $box)
$dp = New-Object Drawing.Pen((C 255 60 90 150), 5); $dp.DashStyle = 'Dash'
$g.DrawPath($dp, $box)
$g.FillEllipse((New-Object Drawing.SolidBrush((C 255 225 35 45))), 18, 22, 28, 28)
$g.FillRectangle((New-Object Drawing.SolidBrush((C 255 40 110 225))), 58, 22, 28, 28)
$tri = [Drawing.PointF[]]@((New-Object Drawing.PointF(32, 58)), (New-Object Drawing.PointF(48, 86)), (New-Object Drawing.PointF(16, 86)))
$g.FillPolygon((New-Object Drawing.SolidBrush((C 255 245 170 20))), $tri)
foreach ($sh in 1) { $g.DrawEllipse((New-Object Drawing.Pen((C 255 120 20 25), 3)), 18, 22, 28, 28); $g.DrawRectangle((New-Object Drawing.Pen((C 255 20 60 140), 3)), 58, 22, 28, 28); $g.DrawPolygon((New-Object Drawing.Pen((C 255 150 95 10), 3)), $tri) }
Gear $g 96 96 28
$g.Dispose(); Save $b 'DEFECT_GROUP_DEFINE'

# 12. Defect Type Define : 이름표(Tag) + 빨간 Defect 점 + 톱니 (Defect 종류 이름 / 코드 정의)
$b = New-Object Drawing.Bitmap $S, $S; $g = NewG $b
$tag = New-Object Drawing.Drawing2D.GraphicsPath
$tag.AddLines([Drawing.PointF[]]@(
    (New-Object Drawing.PointF(30, 14)), (New-Object Drawing.PointF(104, 14)), (New-Object Drawing.PointF(104, 84)),
    (New-Object Drawing.PointF(30, 84)), (New-Object Drawing.PointF(6, 49))))
$tag.CloseFigure()
$g.FillPath((LinBrush 6 14 98 70 (C 255 255 235 150) (C 255 245 190 60)), $tag)
$g.DrawPath((New-Object Drawing.Pen((C 255 150 100 10), 5)), $tag)
$g.FillEllipse([Drawing.Brushes]::White, 20, 42, 14, 14); $g.DrawEllipse((New-Object Drawing.Pen((C 255 150 100 10), 3)), 20, 42, 14, 14)
$g.FillEllipse((LinBrush 44 30 36 36 (C 255 250 90 90) (C 255 180 15 25) 45), 44, 30, 36, 36)
$g.DrawEllipse((New-Object Drawing.Pen((C 255 120 10 15), 3)), 44, 30, 36, 36)
Gear $g 96 96 28
$g.Dispose(); Save $b 'DEFECT_TYPE_DEFINE'

# 13. File Map Setup : 문서 + 작은 Bin Map 격자(파일에서 읽은 Map) + 톱니
$b = New-Object Drawing.Bitmap $S, $S; $g = NewG $b
Page $g 10 4 84 108
$rand = New-Object Random 3
for ($i = 0; $i -lt 6; $i++) { for ($j = 0; $j -lt 6; $j++) {
    if (($i -eq 0 -or $i -eq 5) -and ($j -eq 0 -or $j -eq 5)) { continue }
    $col = if ($rand.Next(7) -eq 0) { C 255 225 40 45 } else { C 255 60 185 80 }
    $g.FillRectangle((New-Object Drawing.SolidBrush($col)), [float](20 + $i * 11), [float](26 + $j * 11), 9, 9)
} }
Gear $g 92 94 30
$g.Dispose(); Save $b 'FILE_MAP_SETUP'

# 14. KLARF Upload : KLARF 문서 + 녹색 업로드 배지 (E-Test Result Upload 와 같은 배지)
$b = New-Object Drawing.Bitmap $S, $S; $g = NewG $b
KlarfDoc $g
UploadBadge $g
$g.Dispose(); Save $b 'KLARF_UPLOAD'

# 15. KLARF File View : KLARF 문서 + 돋보기
$b = New-Object Drawing.Bitmap $S, $S; $g = NewG $b
KlarfDoc $g
Magnifier $g 62 62 42
$g.Dispose(); Save $b 'KLARF_FILE_VIEW'

Get-ChildItem $OutDir -Filter *.png | Select-Object Name, Length
