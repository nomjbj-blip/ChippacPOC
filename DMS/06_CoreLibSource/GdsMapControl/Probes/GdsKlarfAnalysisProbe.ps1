# GDS x KLARF 연관 분석 서비스 검증 스크립트 (설계: 문서/2026-10-12_GDS_KLARF_연관분석_화면_설계.md 6 / 7절)
# 목적: BSL GdsKlarfAnalysis / GdsKlarfMatchEngine 을 같은 프로세스에서 직접 호출해
#       1) 판정 엔진(좌표 변환 / 반전 / 허용오차 경계 / Defect 크기 / 큰 도형 / 중복 제거)을 합성 데이터로 확인하고
#       2) 실제 DB 의 Chain + KLARF 로 AnalyzeWafer 결과를 엔진과 별개로 만든 단순 전수 비교(모든 Member x 모든 Defect)와 대조한다.
#       DB 는 조회만 한다(저장 / 삭제 없음). 접속은 BSL -> DSL -> Miracom.Middleware 경로다(Service Remoting 경유 아님).
# 전제: -Factory / -Device 에 READY Map + READY Chain, -StepSeq 에 KLARF 업로드 Step 이 있어야 한다(기본 SCK / U9 / 501 = XD106.02).
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsKlarfAnalysisProbe.ps1 [-BinDir <Interface / DSL / BSL DLL 폴더>]
#       -BinDir 생략 시 101_ServerBin. 배포 전에는 TEMP 빌드 폴더를 넘긴다.
param(
    [string]$BinDir = '',
    [string]$Factory = 'SCK',
    [string]$Device = 'U9',
    [long]$StepSeq = 501
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$serverBin = Join-Path $dms '101_ServerBin'
if ($BinDir -eq '') { $BinDir = $serverBin }

[AppDomain]::CurrentDomain.SetData('APP_CONFIG_FILE', (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config'))
Add-Type -AssemblyName System.Configuration
$flags = [Reflection.BindingFlags]'NonPublic,Static'
$cm = [Configuration.ConfigurationManager]
$cm.GetField('s_initState', $flags).SetValue($null, 0)
$cm.GetField('s_configSystem', $flags).SetValue($null, $null)
foreach ($d in 'Oracle.ManagedDataAccess', 'Miracom.Middleware', 'DACrux.Base') {
    $p = Join-Path $serverBin "$d.dll"; if (Test-Path $p) { [void][Reflection.Assembly]::LoadFrom($p) }
}
foreach ($d in 'DACrux.SEMDMS.Interface', 'DACrux.SEMDMS.DSL', 'DACrux.SEMDMS.BSL') {
    [void][Reflection.Assembly]::LoadFrom((Join-Path $BinDir "$d.dll"))
}
Write-Host "BSL: $(Join-Path $BinDir 'DACrux.SEMDMS.BSL.dll')"

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
function Throws([scriptblock]$block) {
    try { & $block; return '' } catch { $m = $_.Exception; while ($m.InnerException) { $m = $m.InnerException }; return $m.Message }
}
function Box($seq, $id, $x1, $y1, $x2, $y2) {
    $b = New-Object DACrux.SEMDMS.BSL.GdsKlarfMemberBox
    $b.ChainSeq = $seq; $b.PlacedElementId = $id; $b.LayerId = 1; $b.RoleCode = 'MEMBER'
    $b.MinX = $x1; $b.MinY = $y1; $b.MaxX = $x2; $b.MaxY = $y2; return $b
}
function Defect($id, $xrel, $yrel, $xs, $ys) {
    $d = New-Object DACrux.SEMDMS.BSL.GdsKlarfDefectPoint
    $d.DefectId = $id; $d.XIndex = 1; $d.YIndex = 1; $d.XRel = $xrel; $d.YRel = $yrel; $d.XSize = $xs; $d.YSize = $ys; return $d
}
function Opt($tol, $useSize, $mirror) {
    $o = New-Object DACrux.SEMDMS.BSL.GdsKlarfMatchOptions
    $o.PitchX = 1000; $o.PitchY = 2000; $o.Tolerance = $tol; $o.UseDefectSize = $useSize; $o.Mirror = $mirror; return $o
}
function Hits($members, $defects, $opt) {
    $ml = New-Object 'Collections.Generic.List[DACrux.SEMDMS.BSL.GdsKlarfMemberBox]'; foreach ($m in $members) { $ml.Add($m) }
    $dl = New-Object 'Collections.Generic.List[DACrux.SEMDMS.BSL.GdsKlarfDefectPoint]'; foreach ($d in $defects) { $dl.Add($d) }
    return ,([DACrux.SEMDMS.BSL.GdsKlarfMatchEngine]::Match($ml, $dl, $opt))
}

# 엔진과 별개인 단순 전수 비교(격자 색인 없음). 결과 키 = "DefectId|PlacedElementId".
Add-Type -TypeDefinition @'
using System;
using System.Collections.Generic;
public static class KlarfBruteForce
{
    public static List<string> Hits(double[] mx1, double[] my1, double[] mx2, double[] my2, string[] ids,
        long[] did, double[] xrel, double[] yrel, double[] xs, double[] ys,
        double pitchX, double pitchY, double tol, bool useSize, bool mirror)
    {
        var r = new List<string>();
        for (int d = 0; d < did.Length; d++)
        {
            double vx = xrel[d] - pitchX / 2, vy = yrel[d] - pitchY / 2;
            double sx = mirror ? -vx : vx;
            double hw = (useSize ? Math.Abs(xs[d]) / 2 : 0) + tol, hh = (useSize ? Math.Abs(ys[d]) / 2 : 0) + tol;
            for (int m = 0; m < ids.Length; m++)
                if (mx1[m] <= sx + hw && mx2[m] >= sx - hw && my1[m] <= vy + hh && my2[m] >= vy - hh)
                    r.Add(did[d] + "|" + ids[m]);
        }
        r.Sort(StringComparer.Ordinal);
        return r;
    }
}
'@

# ===== A. 판정 엔진 (합성 데이터, DB 없음) =====
# Pitch 1000 x 2000 -> XREL 500 / YREL 1000 이 Chip 중심(0, 0)
$r = Hits @((Box 1 'C' -1 -1 1 1)) @((Defect 1 500 1000 0 0)) (Opt 0 $true $false)
Check 'A1 좌표: Pitch 칸 중심 = GDS 원점' ($r[0].ViewX -eq 0 -and $r[0].ViewY -eq 0 -and $r[0].MemberIndexes.Count -eq 1) "view=($($r[0].ViewX), $($r[0].ViewY)) hits=$($r[0].MemberIndexes.Count)"
$o = Opt 0 $true $false; $o.OffsetX = 12.5; $o.OffsetY = -3
$r = Hits @() @((Defect 1 600 900 0 0)) $o
Check 'A2 좌표 보정 X / Y' ($r[0].ViewX -eq 112.5 -and $r[0].ViewY -eq -103) "view=($($r[0].ViewX), $($r[0].ViewY))"
# 반전: Member 는 저장 기준 X 100~110. Defect 보기 기준 X = -105 (XREL 395). Mirror 면 저장 기준 +105 -> 걸림
$m = @((Box 1 'R' 100 -5 110 5))
$rm = Hits $m @((Defect 1 395 1000 0 0)) (Opt 0 $true $true)
$rn = Hits $m @((Defect 1 395 1000 0 0)) (Opt 0 $true $false)
Check 'A3 보기 기준 반전(Mirror)' ($rm[0].MemberIndexes.Count -eq 1 -and $rn[0].MemberIndexes.Count -eq 0 -and $rm[0].ViewX -eq -105) "mirror hit=$($rm[0].MemberIndexes.Count) / no-mirror hit=$($rn[0].MemberIndexes.Count) / viewX=$($rm[0].ViewX)"
# 허용오차 경계: Member X 10~20, Defect 중심 X 5 (XREL 505), 크기 0 -> 간격 5. Tol 5 걸림(경계 포함) / 4.999 안 걸림
$m = @((Box 1 'T' 10 -1 20 1))
$t5 = Hits $m @((Defect 1 505 1000 0 0)) (Opt 5 $true $false)
$t4 = Hits $m @((Defect 1 505 1000 0 0)) (Opt 4.999 $true $false)
Check 'A4 허용오차 경계 포함' ($t5[0].MemberIndexes.Count -eq 1 -and $t4[0].MemberIndexes.Count -eq 0) "tol5=$($t5[0].MemberIndexes.Count) tol4.999=$($t4[0].MemberIndexes.Count)"
# Defect 크기: XSIZE 10 -> 반폭 5 로 간격 5 를 메운다. 크기 반영 해제면 안 걸림
$s1 = Hits $m @((Defect 1 505 1000 10 2)) (Opt 0 $true $false)
$s0 = Hits $m @((Defect 1 505 1000 10 2)) (Opt 0 $false $false)
Check 'A5 Defect 크기 반영 / 해제' ($s1[0].MemberIndexes.Count -eq 1 -and $s0[0].MemberIndexes.Count -eq 0) "size=$($s1[0].MemberIndexes.Count) center=$($s0[0].MemberIndexes.Count)"
# 큰 도형(격자 4096칸 초과, 칸 200 -> 20mm x 20mm) / 여러 칸에 걸친 도형 중복 제거 / Chain 2개
$m = @((Box 7 'BIG' -10000 -10000 10000 10000), (Box 8 'LONG' -900 -1 900 1), (Box 8 'FAR' 5000 5000 5001 5001))
$r = Hits $m @((Defect 1 500 1000 400 4)) (Opt 0 $true $false)
Check 'A6 큰 도형 / 긴 도형 1회만 / 먼 도형 제외' ($r[0].MemberIndexes.Count -eq 2 -and ($r[0].MemberIndexes -join ',') -eq '0,1') "hits=$($r[0].MemberIndexes -join ',')"
# 무작위 대조: 엔진(격자 색인) vs 전수 비교, 반전 / 허용오차 / 크기 조합 4가지
$rnd = New-Object Random 53
$N = 3000; $D = 400
$mx1 = New-Object double[] $N; $my1 = New-Object double[] $N; $mx2 = New-Object double[] $N; $my2 = New-Object double[] $N; $ids = New-Object string[] $N
$ml = New-Object 'Collections.Generic.List[DACrux.SEMDMS.BSL.GdsKlarfMemberBox]'
for ($i = 0; $i -lt $N; $i++) {
    $x = $rnd.NextDouble() * 1000 - 500; $y = $rnd.NextDouble() * 2000 - 1000
    $w = $(if ($i % 50 -eq 0) { $rnd.NextDouble() * 900 } else { $rnd.NextDouble() * 20 }); $h = $rnd.NextDouble() * 20
    $mx1[$i] = $x; $my1[$i] = $y; $mx2[$i] = $x + $w; $my2[$i] = $y + $h; $ids[$i] = "M$i"
    $ml.Add((Box ($i % 5 + 1) "M$i" $x $y ($x + $w) ($y + $h)))
}
$did = New-Object long[] $D; $xr = New-Object double[] $D; $yr = New-Object double[] $D; $xs = New-Object double[] $D; $ys = New-Object double[] $D
$dl = New-Object 'Collections.Generic.List[DACrux.SEMDMS.BSL.GdsKlarfDefectPoint]'
for ($i = 0; $i -lt $D; $i++) {
    $did[$i] = $i + 1; $xr[$i] = $rnd.NextDouble() * 1000; $yr[$i] = $rnd.NextDouble() * 2000; $xs[$i] = $rnd.NextDouble() * 30; $ys[$i] = $rnd.NextDouble() * 30
    $dl.Add((Defect ($i + 1) $xr[$i] $yr[$i] $xs[$i] $ys[$i]))
}
$allSame = $true; $detail = @()
foreach ($combo in @(@(0, $true, $false), @(5, $true, $true), @(3, $false, $false), @(10, $false, $true))) {
    $o = Opt $combo[0] $combo[1] $combo[2]
    $eng = [DACrux.SEMDMS.BSL.GdsKlarfMatchEngine]::Match($ml, $dl, $o)
    $ek = New-Object Collections.Generic.List[string]
    foreach ($e in $eng) { foreach ($mi in $e.MemberIndexes) { $ek.Add([string]$e.Defect.DefectId + '|' + $ids[$mi]) } }
    $ek.Sort([StringComparer]::Ordinal)
    $bk = [KlarfBruteForce]::Hits($mx1, $my1, $mx2, $my2, $ids, $did, $xr, $yr, $xs, $ys, 1000, 2000, $combo[0], $combo[1], $combo[2])
    $same = ($ek.Count -eq $bk.Count) -and (($ek -join ';') -eq ($bk -join ';'))
    if (-not $same) { $allSame = $false }
    $detail += "tol=$($combo[0]) size=$($combo[1]) mirror=$($combo[2]) hits=$($ek.Count)/$($bk.Count)"
}
Check 'A7 무작위 3,000 Member x 400 Defect 엔진 = 전수 비교 (4조합)' $allSame ($detail -join ' | ')
Check 'A8 음수 허용오차 거부' ((Throws { Hits @() @() (Opt -1 $true $false) }) -ne '') 'ArgumentException'

# ===== B. 실제 DB (조회만) =====
$bsl = New-Object DACrux.SEMDMS.BSL.GdsKlarfAnalysis
$sw = [Diagnostics.Stopwatch]::StartNew()
$ds = $bsl.AnalyzeWafer($Factory, $Device, $StepSeq, 'BACK', 5, $true, 0, 0, $null, $null)
$sw.Stop()
$info = $ds.Tables['ANL_INFO'].Rows[0]
$names = @($ds.Tables | ForEach-Object { $_.TableName }) -join ','
Check 'B1 결과 테이블 6개 / 분석 기준' ($ds.Tables.Count -eq 6 -and $info['VIEW_SIDE'] -eq 'BACK' -and $info['DEVICE_ID'] -eq $Device -and [double]$info['TOLERANCE_UM'] -eq 5) "tables=$names side=$($info['MAP_VIEW_SIDE'])->$($info['VIEW_SIDE']) mirror=$($info['MIRROR']) pitch=$($info['PITCH_X'])x$($info['PITCH_Y'])"
Write-Host ("     Wafer {0} / {1} / Chain {2} (DRAFT {3}) / Member {4} / Defect {5} / Chain 영역 {6} / 검사 Die {7} / Defect Die {8} / 조회 {9}ms 판정 {10}ms 전체 {11}ms (호출 {12}ms)" -f `
    $info['WAFER_ID'], $info['STEP_ID'], $info['CHAIN_COUNT'], $info['DRAFT_CHAIN_COUNT'], $info['MEMBER_COUNT'], $info['DEFECT_COUNT'], `
    $info['IN_CHAIN_DEFECT_COUNT'], $info['PLAN_DIE_COUNT'], $info['DEFECT_DIE_COUNT'], $info['QUERY_MS'], $info['MATCH_MS'], $info['ELAPSED_MS'], $sw.ElapsedMilliseconds)

# 전수 비교용 원본: 같은 쿼리로 Member Bounds + Defect 를 다시 읽는다
$chainOk = @{}; foreach ($c in @($ds.Tables['CHAIN'].Rows)) { $chainOk[[long]$c['CHAIN_SEQ']] = $true }
$mrows = @((New-Object DACrux.SEMDMS.DSL.TQP_GDS_CHAIN_MEMBER).GetData('SELECT_CHAIN_MEMBER_BOUNDS_ALL', $null, [string[]]@($Factory, $Device)).Rows | Where-Object { $chainOk.ContainsKey([long]$_['CHAIN_SEQ']) })
$drows = @((New-Object DACrux.SEMDMS.DSL.TQD_DEFECT).GetDefect($StepSeq).Rows)
$n = $mrows.Count
$a1 = New-Object double[] $n; $b1 = New-Object double[] $n; $a2 = New-Object double[] $n; $b2 = New-Object double[] $n; $mid = New-Object string[] $n
for ($i = 0; $i -lt $n; $i++) { $a1[$i] = [double]$mrows[$i]['MIN_X']; $b1[$i] = [double]$mrows[$i]['MIN_Y']; $a2[$i] = [double]$mrows[$i]['MAX_X']; $b2[$i] = [double]$mrows[$i]['MAX_Y']; $mid[$i] = [string]$mrows[$i]['PLACED_ELEMENT_ID'] }
$k = $drows.Count
$q1 = New-Object long[] $k; $q2 = New-Object double[] $k; $q3 = New-Object double[] $k; $q4 = New-Object double[] $k; $q5 = New-Object double[] $k
for ($i = 0; $i -lt $k; $i++) { $q1[$i] = [long]$drows[$i]['DEFECTID']; $q2[$i] = [double]$drows[$i]['XREL']; $q3[$i] = [double]$drows[$i]['YREL']; $q4[$i] = [double]$drows[$i]['XSIZE']; $q5[$i] = [double]$drows[$i]['YSIZE'] }
$mirror = $info['MIRROR'] -eq 'Y'
$brute = [KlarfBruteForce]::Hits($a1, $b1, $a2, $b2, $mid, $q1, $q2, $q3, $q4, $q5, [double]$info['PITCH_X'], [double]$info['PITCH_Y'], 5, $true, $mirror)
$svc = @($ds.Tables['DEFECT_HIT'].Rows | ForEach-Object { [string]$_['DEFECTID'] + '|' + [string]$_['PLACED_ELEMENT_ID'] }) | Sort-Object -CaseSensitive
$svcList = New-Object Collections.Generic.List[string]; foreach ($s in $svc) { $svcList.Add($s) }; $svcList.Sort([StringComparer]::Ordinal)
Check 'B2 DEFECT_HIT = 전수 비교 (Member x Defect)' ((($svcList -join ';') -eq ($brute -join ';'))) "service=$($svcList.Count) brute=$($brute.Count) members=$n defects=$k"

# 집계 일관성
$dieT = $ds.Tables['DIE']; $dcT = $ds.Tables['DIE_CHAIN']; $defT = $ds.Tables['DEFECT']; $hitT = $ds.Tables['DEFECT_HIT']
$inChain = @($defT.Rows | Where-Object { $_['IN_CHAIN'] -eq 'Y' }).Count
$hitDefects = @($hitT.Rows | ForEach-Object { [long]$_['DEFECTID'] } | Sort-Object -Unique).Count
$dieIn = 0; $dieAll = 0; foreach ($d in @($dieT.Rows)) { $dieIn += [int]$d['IN_CHAIN_COUNT']; $dieAll += [int]$d['DEFECT_COUNT'] }
Check 'B3 Defect 수 / Chain 영역 수 일관' ($defT.Rows.Count -eq [int]$info['DEFECT_COUNT'] -and $inChain -eq [int]$info['IN_CHAIN_DEFECT_COUNT'] -and $inChain -eq $hitDefects -and $dieIn -eq $inChain -and $dieAll -eq $defT.Rows.Count) "defect=$($defT.Rows.Count) inChain=$inChain hitDefects=$hitDefects dieSum=$dieIn/$dieAll"
# (Die, Chain) 수 = DEFECT_HIT 의 (Die, Chain, Defect) 중복 제거 수
$pairs = @{}
foreach ($h in @($hitT.Rows)) {
    $d = @($defT.Select("DEFECTID = $($h['DEFECTID'])"))[0]
    $pairs["$($d['XINDEX']),$($d['YINDEX'])|$($h['CHAIN_SEQ'])|$($h['DEFECTID'])"] = 1
}
$dcSum = 0; foreach ($r in @($dcT.Rows)) { $dcSum += [int]$r['DEFECT_COUNT'] }
Check 'B4 DIE_CHAIN 합 = (Die, Chain, Defect) 수' ($dcSum -eq $pairs.Count) "dieChainSum=$dcSum pairs=$($pairs.Count) rows=$($dcT.Rows.Count)"
$planN = @($dieT.Rows | Where-Object { $_['IN_PLAN'] -eq 'Y' }).Count
Check 'B5 검사 Die 목록' ($planN -eq [int]$info['PLAN_DIE_COUNT'] -and $planN -gt 0) "plan=$planN dieRows=$($dieT.Rows.Count) (계획 밖 Defect Die $($dieT.Rows.Count - $planN))"
$first = $dieT.Rows[0]; $sorted = $true
for ($i = 1; $i -lt $dieT.Rows.Count; $i++) { if ([int]$dieT.Rows[$i]['IN_CHAIN_COUNT'] -gt [int]$dieT.Rows[$i - 1]['IN_CHAIN_COUNT']) { $sorted = $false } }
Check 'B6 Die 목록 Chain 영역 내림차순' $sorted "first=($($first['XINDEX']),$($first['YINDEX'])) in=$($first['IN_CHAIN_COUNT']) top=$($first['TOP_CHAINS'])"

# 보기 기준 FRONT: 반전 없음, GDS_X 부호는 같고(보기 기준 좌표는 KLARF 그대로) 판정만 달라질 수 있다
$dsF = $bsl.AnalyzeWafer($Factory, $Device, $StepSeq, 'FRONT', 5, $true, 0, 0, $null, $null)
$iF = $dsF.Tables['ANL_INFO'].Rows[0]
$gx1 = [double]$defT.Rows[0]['GDS_X']; $gx2 = [double]$dsF.Tables['DEFECT'].Rows[0]['GDS_X']
Check 'B7 FRONT 보기 기준' ($iF['MIRROR'] -ne $info['MIRROR'] -and $gx1 -eq $gx2) "mirror=$($iF['MIRROR']) inChain FRONT=$($iF['IN_CHAIN_DEFECT_COUNT']) / BACK=$($info['IN_CHAIN_DEFECT_COUNT']) gdsX=$gx1"
# Class 필터: 첫 Defect 의 Class 만
$cls = [int]$defT.Rows[0]['CLASSNUMBER']
$expectCls = @($defT.Rows | Where-Object { [int]$_['CLASSNUMBER'] -eq $cls }).Count
$dsC = $bsl.AnalyzeWafer($Factory, $Device, $StepSeq, 'BACK', 5, $true, 0, 0, $null, [int[]]@($cls))
Check 'B8 Class 필터' ([int]$dsC.Tables['ANL_INFO'].Rows[0]['DEFECT_COUNT'] -eq $expectCls -and $dsC.Tables['ANL_INFO'].Rows[0]['CLASS_FILTER'] -eq [string]$cls) "class=$cls defect=$($dsC.Tables['ANL_INFO'].Rows[0]['DEFECT_COUNT']) expect=$expectCls"
# Chain 필터: 첫 Chain 만
$seq1 = [string]$ds.Tables['CHAIN'].Rows[0]['CHAIN_SEQ']
$dsH = $bsl.AnalyzeWafer($Factory, $Device, $StepSeq, 'BACK', 5, $true, 0, 0, [string[]]@($seq1), $null)
$other = @($dsH.Tables['DEFECT_HIT'].Rows | Where-Object { [string]$_['CHAIN_SEQ'] -ne $seq1 }).Count
Check 'B9 Chain 필터' ($dsH.Tables['CHAIN'].Rows.Count -eq 1 -and $other -eq 0) "chain=$seq1 rows=$($dsH.Tables['CHAIN'].Rows.Count) hits=$($dsH.Tables['DEFECT_HIT'].Rows.Count)"
# 큰 허용오차일수록 Chain 영역 Defect 수가 줄지 않는다
$ds50 = $bsl.AnalyzeWafer($Factory, $Device, $StepSeq, 'BACK', 50, $true, 0, 0, $null, $null)
Check 'B10 허용오차 증가 시 단조 증가' ([int]$ds50.Tables['ANL_INFO'].Rows[0]['IN_CHAIN_DEFECT_COUNT'] -ge [int]$info['IN_CHAIN_DEFECT_COUNT']) "tol5=$($info['IN_CHAIN_DEFECT_COUNT']) tol50=$($ds50.Tables['ANL_INFO'].Rows[0]['IN_CHAIN_DEFECT_COUNT'])"
# 오류: 없는 Step / 음수 허용오차 / 없는 Device / Device 빈 값(= KLARF PRODUCT)
$e1 = Throws { $bsl.AnalyzeWafer($Factory, $Device, 999999999, 'BACK', 5, $true, 0, 0, $null, $null) }
$e2 = Throws { $bsl.AnalyzeWafer($Factory, $Device, $StepSeq, 'BACK', -1, $true, 0, 0, $null, $null) }
$e3 = Throws { $bsl.AnalyzeWafer($Factory, 'NO_SUCH_DEVICE_PROBE', $StepSeq, 'BACK', 5, $true, 0, 0, $null, $null) }
$e4 = Throws { $bsl.AnalyzeWafer($Factory, '', $StepSeq, 'BACK', 5, $true, 0, 0, $null, $null) }
Check 'B11 오류 안내' ($e1 -like '*KLARF Wafer*' -and $e2 -like '*허용오차*' -and $e3 -like '*READY*' -and ($e4 -eq '' -or $e4 -like '*READY*')) "step=[$e1] tol=[$e2] device=[$e3] product=[$e4]"

$fail = @($results | Where-Object { $_ -like 'FAIL*' }).Count
Write-Host ''
Write-Host ("GDS_KLARF_ANALYSIS_PROBE={0} ({1}/{2})" -f $(if ($fail -eq 0) { 'PASS' } else { 'FAIL' }), ($results.Count - $fail), $results.Count)
if ($fail -ne 0) { exit 1 }
