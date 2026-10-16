param([string]$OutDir = $PSScriptRoot)
# POC 화면 버튼 아이콘 (2026-10-08, KLARF Upload 화면 버튼).
# 그림 방식과 색은 메뉴 아이콘 생성 스크립트(01_WinApp/DACruxV5/MenuIcons/make_icons.ps1,~047)와 같게 맞춘다:
#   128px 로 그린 뒤 48 / 24 / 16 px 로 줄여 PNG 로 저장(48 = 큰 버튼, 24 = 일반 버튼, 16 = 작은 버튼 / 메뉴). 화면은 .resx 의 ResXFileRef 로 이 폴더 PNG 를 참조한다(복사하지 않음).
# 실행: powershell -File make_button_icons.ps1 [-OutDir <폴더>]
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

# 문서(페이지) - make_icons.ps1 의 Page 와 같음
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

# KLARF 문서 - make_icons.ps1 의 KlarfDoc 와 같음 (위쪽 남색 띠 "KLARF" + Defect 목록 줄 / 빨간 점)
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

function Badge($g, $cx, $cy, $r, $c1, $c2) {
    $g.FillEllipse((LinBrush ($cx - $r) ($cy - $r) ($r * 2) ($r * 2) $c1 $c2), ($cx - $r), ($cy - $r), ($r * 2), ($r * 2))
    $g.DrawEllipse((New-Object Drawing.Pen((C 255 255 255 255), 4)), ($cx - $r), ($cy - $r), ($r * 2), ($r * 2))
}

# 녹색 업로드 배지 - make_icons.ps1 의 UploadBadge 와 같음 (KLARF Upload / E-Test Upload 메뉴와 같은 표시)
function UploadBadge($g) {
    Badge $g 96 96 28 (C 255 110 210 90) (C 255 25 140 45)
    $arrow = [Drawing.PointF[]]@(
        (New-Object Drawing.PointF(96, 76)), (New-Object Drawing.PointF(114, 96)), (New-Object Drawing.PointF(103, 96)),
        (New-Object Drawing.PointF(103, 114)), (New-Object Drawing.PointF(89, 114)), (New-Object Drawing.PointF(89, 96)),
        (New-Object Drawing.PointF(78, 96)))
    $g.FillPolygon([Drawing.Brushes]::White, $arrow)
}

# 노란 폴더 (뒤판 + 탭 + 앞판). open 이면 앞판을 아래로 기울여 열린 모양
function Folder($g, $x, $y, $w, $h, [bool]$open) {
    $edge = New-Object Drawing.Pen((C 255 160 110 20), 4)
    $back = New-Object Drawing.Drawing2D.GraphicsPath
    $tabW = $w * 0.38
    $back.AddLines([Drawing.PointF[]]@(
        (New-Object Drawing.PointF($x, ($y + 10))), (New-Object Drawing.PointF(($x + 4), $y)), (New-Object Drawing.PointF(($x + $tabW), $y)),
        (New-Object Drawing.PointF(($x + $tabW + 8), ($y + 10))), (New-Object Drawing.PointF(($x + $w), ($y + 10))),
        (New-Object Drawing.PointF(($x + $w), ($y + $h))), (New-Object Drawing.PointF($x, ($y + $h)))))
    $back.CloseFigure()
    $g.FillPath((LinBrush $x $y $w $h (C 255 255 214 110) (C 255 230 160 40)), $back)
    $g.DrawPath($edge, $back)
    $front = New-Object Drawing.Drawing2D.GraphicsPath
    if ($open) {
        $front.AddLines([Drawing.PointF[]]@(
            (New-Object Drawing.PointF(($x + 12), ($y + 26))), (New-Object Drawing.PointF(($x + $w + 8), ($y + 26))),
            (New-Object Drawing.PointF(($x + $w - 6), ($y + $h))), (New-Object Drawing.PointF($x, ($y + $h)))))
    } else {
        $front.AddRectangle((New-Object Drawing.RectangleF($x, ($y + 22), $w, ($h - 22))))
    }
    $front.CloseFigure()
    $g.FillPath((LinBrush $x ($y + 22) $w ($h - 22) (C 255 255 230 140) (C 255 245 180 50)), $front)
    $g.DrawPath($edge, $front)
}

# 사진(산 + 해) - 이미지 폴더 표시
function Picture($g, $x, $y, $w, $h) {
    $g.FillRectangle([Drawing.Brushes]::White, $x, $y, $w, $h)
    $g.FillRectangle((LinBrush $x $y $w $h (C 255 150 205 250) (C 255 220 240 255)), ($x + 5), ($y + 5), ($w - 10), ($h - 10))
    $g.FillEllipse((New-Object Drawing.SolidBrush((C 255 255 200 40))), ($x + $w * 0.62), ($y + 9), ($w * 0.2), ($w * 0.2))
    $m = [Drawing.PointF[]]@((New-Object Drawing.PointF(($x + 5), ($y + $h - 5))), (New-Object Drawing.PointF(($x + $w * 0.38), ($y + $h * 0.38))),
        (New-Object Drawing.PointF(($x + $w * 0.62), ($y + $h * 0.7))), (New-Object Drawing.PointF(($x + $w * 0.75), ($y + $h * 0.55))),
        (New-Object Drawing.PointF(($x + $w - 5), ($y + $h - 5))))
    $g.FillPolygon((New-Object Drawing.SolidBrush((C 255 60 160 80))), $m)
    $g.DrawRectangle((New-Object Drawing.Pen((C 255 90 100 115), 4)), $x, $y, $w, $h)
}

# DB 원통 (파란 Database)
function Database($g, $x, $y, $w, $h) {
    $eh = $h * 0.24
    $edge = New-Object Drawing.Pen((C 255 25 60 120), 4)
    $body = New-Object Drawing.Drawing2D.GraphicsPath
    $body.AddArc($x, ($y + $h - $eh), $w, $eh, 0, 180)
    $body.AddLine($x, ($y + $h - $eh / 2), $x, ($y + $eh / 2))
    $body.AddArc($x, $y, $w, $eh, 180, -180)
    $body.CloseFigure()
    $g.FillPath((LinBrush $x $y $w $h (C 255 80 140 220) (C 255 30 80 170) 0), $body)
    $g.DrawPath($edge, $body)
    foreach ($k in 1, 2) {
        $yy = $y + ($h - $eh) * $k / 3
        $g.DrawArc((New-Object Drawing.Pen((C 255 200 225 255), 4)), $x, $yy, $w, $eh, 0, 180)
    }
    $g.FillEllipse((LinBrush $x $y $w $eh (C 255 190 220 255) (C 255 120 170 235)), $x, $y, $w, $eh)
    $g.DrawEllipse($edge, $x, $y, $w, $eh)
}

function Save($bmp, $name) {
    foreach ($sz in 48, 24, 16) {
        $o = New-Object Drawing.Bitmap $sz, $sz
        $g = NewG $o
        $g.DrawImage($bmp, 0, 0, $sz, $sz)
        $g.Dispose()
        $o.Save((Join-Path $OutDir "$name`_$sz.png"), [Drawing.Imaging.ImageFormat]::Png)
    }
}

# 1. KLARF 파일 선택 : KLARF 문서 + 앞쪽 열린 폴더
$b = New-Object Drawing.Bitmap $S, $S; $g = NewG $b
KlarfDoc $g
Folder $g 48 66 70 56 $true
$g.Dispose(); Save $b 'BTN_KLARF_OPEN'

# 2. 이미지 폴더 선택 : 노란 폴더 + 위로 나온 사진 (순서: 폴더 뒤판 -> 사진 -> 폴더 앞판, 사진 윗부분만 보이게)
$b = New-Object Drawing.Bitmap $S, $S; $g = NewG $b
Folder $g 6 30 116 88 $false
Picture $g 30 10 70 56
$front = New-Object Drawing.Drawing2D.GraphicsPath
$front.AddRectangle((New-Object Drawing.RectangleF(6, 62, 116, 56)))
$g.FillPath((LinBrush 6 62 116 56 (C 255 255 230 140) (C 255 245 180 50)), $front)
$g.DrawPath((New-Object Drawing.Pen((C 255 160 110 20), 4)), $front)
$g.Dispose(); Save $b 'BTN_IMAGE_FOLDER'

# 3. DB 저장 : 파란 DB 원통 + 녹색 업로드 배지 (KLARF Upload 메뉴 아이콘과 같은 배지)
$b = New-Object Drawing.Bitmap $S, $S; $g = NewG $b
Database $g 10 8 82 104
UploadBadge $g
$g.Dispose(); Save $b 'BTN_DB_SAVE'

Get-ChildItem $OutDir -Filter 'BTN_*.png' | Select-Object Name, Length
