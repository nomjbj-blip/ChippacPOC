param([string]$OutDir = $PSScriptRoot)
# GDS x KLARF 연관 분석 화면 버튼 / 메뉴 아이콘 (2026-10-12).
# 그림 방식은 make_button_icons.ps1과 같다: 128px 로 그린 뒤 48 / 32 / 24 / 16 px PNG 로 줄인다.
#   BTN_GDS_ANALYZE : Wafer 원 + Die 격자(Chain 영역 Defect 수 구간 색, 기존 Defect Count Color 색 순서) -> [분석] 버튼 / 연관 분석 메뉴
#   BTN_DIE_DETAIL  : Die(칩) + Chain 배선 2개 + 빨간 Defect + 돋보기 -> [상세 보기] 버튼 / Die 상세 메뉴
# 화면 .resx 는 이 PNG 를 base64 로 넣는다(make 후 frmGdsKlarfAnalysis.resx 재생성).
# 실행: powershell -File make_analysis_icons.ps1 [-OutDir <폴더>]
Add-Type -AssemblyName System.Drawing
$ErrorActionPreference = 'Stop'
New-Item -ItemType Directory -Force $OutDir | Out-Null
$S = 128

function C($a, $r, $g, $b) { [Drawing.Color]::FromArgb($a, $r, $g, $b) }
function HexColor($html) { [Drawing.ColorTranslator]::FromHtml($html) }
function NewG($bmp) {
    $g = [Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = 'AntiAlias'; $g.PixelOffsetMode = 'HighQuality'; $g.InterpolationMode = 'HighQualityBicubic'
    return $g
}
function LinBrush($x, $y, $w, $h, $c1, $c2, $ang = 90) {
    New-Object Drawing.Drawing2D.LinearGradientBrush((New-Object Drawing.RectangleF($x, $y, $w, $h)), $c1, $c2, [float]$ang)
}
function Save($bmp, $name) {
    foreach ($sz in 48, 32, 24, 16) {
        $o = New-Object Drawing.Bitmap $sz, $sz
        $g = NewG $o
        $g.DrawImage($bmp, 0, 0, $sz, $sz)
        $g.Dispose()
        $o.Save((Join-Path $OutDir "$name`_$sz.png"), [Drawing.Imaging.ImageFormat]::Png)
        $o.Dispose()
    }
}

# 1. 연관 분석: Wafer(회색 원 + Notch) 안 5 x 5 Die 격자, 가운데 Die 들을 구간 색으로 칠한다
$b = New-Object Drawing.Bitmap $S, $S; $g = NewG $b
$g.FillEllipse((LinBrush 4 4 120 120 (C 255 235 240 248) (C 255 200 210 225)), 4, 4, 120, 120)
$g.DrawEllipse((New-Object Drawing.Pen((C 255 90 105 130), 5)), 4, 4, 120, 120)
$g.FillPie([Drawing.Brushes]::White, 56, 112, 16, 16, 180, 180)
$cells = @(
    @(1, 0, ''), @(2, 0, '#DAF7A6'), @(3, 0, ''),
    @(0, 1, ''), @(1, 1, '#FFC300'), @(2, 1, '#C70039'), @(3, 1, '#DAF7A6'), @(4, 1, ''),
    @(0, 2, '#DAF7A6'), @(1, 2, '#FF5733'), @(2, 2, '#900C3F'), @(3, 2, '#FFC300'), @(4, 2, ''),
    @(0, 3, ''), @(1, 3, '#DAF7A6'), @(2, 3, '#FF5733'), @(3, 3, ''), @(4, 3, ''),
    @(1, 4, ''), @(2, 4, '#DAF7A6'), @(3, 4, ''))
$grid = New-Object Drawing.Pen((C 255 110 120 140), 2.5)
foreach ($c in $cells) {
    $x = 19 + $c[0] * 18; $y = 19 + $c[1] * 18
    $fill = if ($c[2] -eq '') { [Drawing.Brushes]::White } else { New-Object Drawing.SolidBrush((HexColor $c[2])) }
    $g.FillRectangle($fill, $x, $y, 18, 18)
    $g.DrawRectangle($grid, $x, $y, 18, 18)
}
$g.Dispose(); Save $b 'BTN_GDS_ANALYZE'

# 2. Die 상세: 칩(진한 남색 사각형) + Chain 배선 2개(청록 / 보라) + 빨간 Defect + 오른쪽 아래 돋보기
$b = New-Object Drawing.Bitmap $S, $S; $g = NewG $b
$g.FillRectangle((LinBrush 6 6 96 112 (C 255 40 55 85) (C 255 20 28 48)), 6, 6, 96, 112)
$g.DrawRectangle((New-Object Drawing.Pen((C 255 120 140 175), 4)), 6, 6, 96, 112)
$teal = New-Object Drawing.Pen((C 255 40 200 160), 7); $teal.LineJoin = 'Round'
$violet = New-Object Drawing.Pen((C 255 160 140 250), 7); $violet.LineJoin = 'Round'
$g.DrawLines($teal, [Drawing.PointF[]]@((New-Object Drawing.PointF(18, 30)), (New-Object Drawing.PointF(60, 30)), (New-Object Drawing.PointF(60, 62)), (New-Object Drawing.PointF(90, 62))))
$g.DrawLines($violet, [Drawing.PointF[]]@((New-Object Drawing.PointF(18, 96)), (New-Object Drawing.PointF(44, 96)), (New-Object Drawing.PointF(44, 74)), (New-Object Drawing.PointF(80, 74))))
$red = New-Object Drawing.SolidBrush((C 255 235 45 45))
$g.FillRectangle($red, 53, 23, 14, 14); $g.DrawRectangle([Drawing.Pens]::White, 53, 23, 14, 14)
$g.FillRectangle($red, 38, 89, 12, 12); $g.DrawRectangle([Drawing.Pens]::White, 38, 89, 12, 12)
$lens = New-Object Drawing.Pen((C 255 255 255 255), 9)
$g.FillEllipse((New-Object Drawing.SolidBrush((C 120 200 230 255))), 70, 70, 40, 40)
$g.DrawEllipse((New-Object Drawing.Pen((C 255 40 70 120), 7)), 70, 70, 40, 40)
$handle = New-Object Drawing.Pen((C 255 40 70 120), 12); $handle.StartCap = 'Round'; $handle.EndCap = 'Round'
$g.DrawLine($handle, 104, 104, 120, 120)
$g.Dispose(); Save $b 'BTN_DIE_DETAIL'

Get-ChildItem $OutDir -Filter 'BTN_GDS_ANALYZE_*.png' | Select-Object Name, Length
Get-ChildItem $OutDir -Filter 'BTN_DIE_DETAIL_*.png' | Select-Object Name, Length
