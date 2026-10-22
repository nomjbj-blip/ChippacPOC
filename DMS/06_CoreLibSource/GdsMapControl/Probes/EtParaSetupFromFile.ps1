# E-Test 결과 파일 하나로 Para Setup(등록 + Spec + GDS Chain)을 한 번에 한다 (2026-10-22)
# 목적: U9 같은 새 Device 를 E-Test Para 등록 / 설정 화면에서 손으로 하던 순서 그대로 채운다.
#  1) 등록: 결과 파일 헤더 중 -ChainType(기본 TSVDC) 헤더를 RegisterEtParameters(출처 FILE, 등록 화면과 같은 서버 함수)로 등록. 이미 있는 것은 건너뜀
#  2) Spec: 고객 Spec 표(-SpecFile, 탭 구분: Device / Test Type / Chain name / Parameter / Spec / Unit)에서 파일 공정 행을 SaveEtLimit(Enable Y)
#     이미 Spec 이 있는 Parameter 는 -OverwriteSpec 이 없으면 그대로 둔다
#  3) GDS Chain: 비어 있는 칸만 채운다(-OverwriteGds 면 모두). 고르는 순서
#     a) -GdsFrom(기본 OMMTCB) Device 의 같은 Parameter 에 넣은 GDS Chain 이 이 Device 의 GDS Setup Chain 목록에 있으면 그 이름
#     b) 이름 규칙(Chain 1 기준, 대소문자 무시): 같은 이름 -> _FL 을 같은 위치 · 극성 _FH -> 같은 위치 다른 극성(N <-> P) -> TSVDC 모서리(TL / TR / BL / BR)끼리
#     GDS Setup Chain 목록 = 이 E-Test Device 와 같은 이름의 GDS Device(설정 화면과 같음). 없으면 GDS Chain 은 비워 둔다
#     저장은 SaveEtParameterSetup(7열) 에 사용 / 비고 / Lower / Upper / Enable 을 지금 값 그대로 보내 GDS Chain 만 바뀐다
#  4) 다시 읽어 등록 / Spec / GDS Chain 수를 확인한다
# DB 접속: BSL -> DSL -> Miracom.Middleware(Service Remoting 아님). SEMDMS Service 설정 파일을 쓰고 -ConnectId 를 주면
#          그 사본의 DMS / TEST / QMS_CONNECT_ID 만 바꾼다(서버 설정 파일은 그대로). 시작할 때 붙는 DB 연결 이름을 먼저 보여 준다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File EtParaSetupFromFile.ps1 [-File <xlsx>] [-Factory SCK] [-Device U9]
#         [-ChainType TSVDC] [-GdsFrom OMMTCB] [-ConnectId DMSMGR] [-OverwriteSpec] [-OverwriteGds] [-Apply]
#       -Apply 가 없으면 계획만 보여 주고 DB 는 읽기만 한다. -Device 를 비우면 파일 이름의 Device. -ChainType '*' = 모든 헤더.
#       -GdsFrom none = 다른 Device 를 따르지 않고 이름 규칙만.
# 결과: 마지막 줄 ET_PARA_SETUP_FROM_FILE=PLAN / PASS / FAIL
param(
    [string]$File = '',
    [string]$SpecFile = '',
    [string]$Factory = 'SCK',
    [string]$Device = '',
    [string[]]$ChainType = @('TSVDC'),
    [string]$GdsFrom = 'OMMTCB',
    [string]$ConnectId = '',
    [string]$ServerBinDir = '',
    [string]$ClientBinDir = '',
    [string]$User = 'PARA_SETUP',
    [switch]$OverwriteSpec,
    [switch]$OverwriteGds,
    [switch]$Apply
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$root = Split-Path $dms -Parent
if ($File -eq '') { $File = Join-Path $root 'SampleFile\26_10_06 Sample File\CAI_U9_ATV_A0_TSVET_20260609161940_RH-HS2615_RH-HS2615-02.xlsx' }
if ($SpecFile -eq '') { $SpecFile = Join-Path $dms '02_DMS\DACrux.SEMDMS.ENGUI\Verification\EtSpecTable_U9_20261012.txt' }
if ($ServerBinDir -eq '') { $ServerBinDir = Join-Path $dms '101_ServerBin' }
if ($ClientBinDir -eq '') { $ClientBinDir = Join-Path $dms '100_ClientBin' }
$ChainType = @($ChainType | ForEach-Object { $_.Split(',') } | ForEach-Object { $_.Trim().ToUpperInvariant() } | Where-Object { $_ -ne '' })
if ($GdsFrom -match '^(?i)(none|-)$') { $GdsFrom = '' } # powershell -File 은 빈 문자열 인수를 넘기지 못해 none 으로 받는다
foreach ($p in $File, $SpecFile) { if (-not (Test-Path $p)) { throw "파일이 없습니다: $p" } }

# 0. 설정 파일 / DLL (BSL / DSL 이 ConfigurationManager.AppSettings 의 접속 이름을 읽는다)
$serverConfig = Join-Path $ServerBinDir 'DACrux.SEMDMS.Service.exe.config'
[xml]$x = Get-Content $serverConfig -Encoding UTF8
$keys = @($x.configuration.appSettings.add | Where-Object { $_.key -in 'DMS_CONNECT_ID', 'TEST_CONNECT_ID', 'QMS_CONNECT_ID' })
$serverConnect = ($keys | Where-Object { $_.key -eq 'DMS_CONNECT_ID' } | Select-Object -First 1).value
$config = $serverConfig
if ($ConnectId -ne '' -and $ConnectId -ne $serverConnect) {
    foreach ($k in $keys) { $k.value = $ConnectId }
    $config = Join-Path $env:TEMP ('EtParaSetupFromFile_' + $ConnectId + '.config')
    $x.Save($config)
}
$useConnect = if ($ConnectId -ne '') { $ConnectId } else { $serverConnect }
[AppDomain]::CurrentDomain.SetData('APP_CONFIG_FILE', $config)
Add-Type -AssemblyName System.Configuration
$flags = [Reflection.BindingFlags]'NonPublic,Static'
$cm = [Configuration.ConfigurationManager]
$cm.GetField('s_initState', $flags).SetValue($null, 0)
$cm.GetField('s_configSystem', $flags).SetValue($null, $null)
$probeDirs = @($ServerBinDir, $ClientBinDir)
[AppDomain]::CurrentDomain.add_AssemblyResolve({
    param($s, $e)
    $n = (New-Object Reflection.AssemblyName($e.Name)).Name
    foreach ($d in $probeDirs) { $p = Join-Path $d "$n.dll"; if (Test-Path $p) { return [Reflection.Assembly]::LoadFrom($p) } }
    return $null
})
foreach ($d in 'Oracle.ManagedDataAccess', 'Miracom.Middleware', 'DACrux.Base', 'DACrux.SEMDMS.Interface', 'DACrux.SEMDMS.DSL', 'DACrux.TEST.Interface', 'DACrux.TEST.DSL', 'DACrux.TEST.BSL', 'DACrux.SEMDMS.BSL') {
    [void][Reflection.Assembly]::LoadFrom((Join-Path $ServerBinDir "$d.dll"))
}
[void][Reflection.Assembly]::LoadFrom((Join-Path $ClientBinDir 'DACrux.SEMDMS.ENGUI.dll'))
$rule = [DACrux.SEMDMS.Interface.EtParameterNameRule]
$bsl = New-Object DACrux.SEMDMS.BSL.GdsEtResult
$inv = [Globalization.CultureInfo]::InvariantCulture
Write-Host ("DB 연결 이름: {0}{1} / {2}" -f $useConnect, $(if ($useConnect -ne $serverConnect) { " (서버 설정은 $serverConnect, 이 실행만 바꿈)" } else { ' (서버 설정 그대로)' }), $(if ($Apply) { '실행(-Apply)' } else { '계획만(DB 읽기만)' }))

# 1. 결과 파일 -> 등록 대상 헤더
$f = [DACrux.SEMDMS.ENGUI.EtResultFileParser]::Parse($File)
if ($Device -eq '') { $Device = $f.FileDevice }
$program = $rule::ToTestProgram($f.TestProgram)
$targets = New-Object Collections.Generic.List[object]
for ($k = 0; $k -lt $f.ItemCount; $k++) {
    $parts = $rule::Parse($f.Items.GetValue($k, 0))
    if (-not $parts.IsValid) { throw "헤더를 나눌 수 없습니다: $($f.Items.GetValue($k, 0)) / $($parts.Error)" }
    if ($ChainType -notcontains '*' -and $ChainType -notcontains ([string]$parts.ChainType).ToUpperInvariant()) { continue }
    $targets.Add([pscustomobject]@{
        Header = $f.Items.GetValue($k, 0); Name = $parts.ParameterName; Meas = $parts.MeasType; ChainType = $parts.ChainType; Chain1 = $parts.Chain1
        Unit = $f.Items.GetValue($k, 5); TestNo = $f.Items.GetValue($k, 6); Column = $f.Items.GetValue($k, 7)
    })
}
Write-Host ("파일: {0}" -f [IO.Path]::GetFileName($File))
Write-Host ("Factory {0} / E-Test Device {1}(파일 Device {2}) / 공정 {3} / 헤더 {4} 중 등록 대상({5}) {6}" -f $Factory, $Device, $f.FileDevice, $program, $f.ItemCount, ($ChainType -join ','), $targets.Count)
$targets | Group-Object Meas | Sort-Object Name | ForEach-Object { Write-Host ("  {0} {1}" -f $_.Name, $_.Count) }
if ($targets.Count -eq 0) { throw '등록할 헤더가 없습니다(-ChainType 확인).' }

function Read-Setup { $m = @{}; foreach ($r in @($bsl.GetEtParameterSetupList($Factory, $Device, $program).Rows)) { $m[[string]$r['ITEM_NAME']] = $r }; return $m }
function Limit-Text($v) { if ($v -eq $null -or $v -is [DBNull]) { '' } else { ([double]$v).ToString('R', $inv) } }
$setup = Read-Setup
$newOnes = @($targets | Where-Object { -not $setup.ContainsKey($_.Name) })
Write-Host ("[1] 등록: 이미 등록 {0} / 새로 등록 {1}" -f ($targets.Count - $newOnes.Count), $newOnes.Count)

# 2. Spec 표
$specRows = @([DACrux.SEMDMS.ENGUI.EtSpecTable]::Parse([IO.File]::ReadAllText($SpecFile)))
$bad = @($specRows | Where-Object { $_.Error })
if ($bad.Count -gt 0) { throw "Spec 표에 해석할 수 없는 행이 $($bad.Count)개 있습니다: $($bad[0].LineNo)줄 $($bad[0].Error)" }
$specByName = @{}
foreach ($s in $specRows) { if ($rule::ToTestProgram($s.TestProgram) -eq $program) { $specByName[$rule::ToParameterName($s.HeaderText)] = $s } }
$targetNames = New-Object Collections.Generic.HashSet[string]
foreach ($t in $targets) { [void]$targetNames.Add($t.Name) }
$specPlan = New-Object Collections.Generic.List[object]
$specKeep = 0
foreach ($t in $targets) {
    if (-not $specByName.ContainsKey($t.Name)) { continue }
    $old = $setup[$t.Name]
    if ($old -ne $null -and [string]$old['HAS_SPEC'] -eq 'Y' -and -not $OverwriteSpec) { $specKeep++; continue }
    $specPlan.Add($specByName[$t.Name])
}
$specOutside = @($specByName.Keys | Where-Object { -not $targetNames.Contains($_) })
Write-Host ("[2] Spec: 표의 {0} 행 {1} / 등록 대상과 맞음 {2} / 넣을 것 {3} / 이미 있어 그대로 {4} / 등록 대상 밖(넣지 않음) {5}" -f $program, $specByName.Count, ($specPlan.Count + $specKeep), $specPlan.Count, $specKeep, $specOutside.Count)
$specPlan | Group-Object { "Lower '{0}' / Upper '{1}' {2}" -f $_.Lower, $_.Upper, $_.Unit } | ForEach-Object { Write-Host ("  {0}: {1}개" -f $_.Name, $_.Count) }

# 3. GDS Chain
$chains = @($bsl.GetEtGdsChainList($Factory, $Device).Rows | ForEach-Object { [string]$_['CHAIN_NAME'] })
$chainSet = New-Object 'Collections.Generic.Dictionary[string,string]' ([StringComparer]::OrdinalIgnoreCase)
foreach ($c in $chains) { $chainSet[$c] = $c }
$fromGds = @{}
if ($GdsFrom -ne '' -and $GdsFrom -ne $Device) {
    foreach ($r in @($bsl.GetEtParameterSetupList($Factory, $GdsFrom, $program).Rows)) { $g = [string]$r['GDS_CHAIN_NAME']; if ($g -ne '') { $fromGds[[string]$r['ITEM_NAME']] = $g } }
}
$corner = @('TL', 'TR', 'BL', 'BR')
function Split-Chain([string]$name, [string]$type) {
    $prefix = $type.ToUpperInvariant() + '_'
    $u = $name.ToUpperInvariant()
    if (-not $u.StartsWith($prefix)) { return $null }
    $m = [regex]::Match($u.Substring($prefix.Length), '^(?<loc>.+?)(?:_(?<pol>[NP]))?_(?<f>F[HL])$')
    if (-not $m.Success) { return $null }
    return [pscustomobject]@{ Loc = $m.Groups['loc'].Value; Pol = $m.Groups['pol'].Value; F = $m.Groups['f'].Value }
}
function Find-ByRule([string]$chain1, [string]$type) {
    if ($chainSet.ContainsKey($chain1)) { return @($chainSet[$chain1], '이름 같음') }
    $p = Split-Chain $chain1 $type
    if (-not $p) { return @('', '') }
    $gds = @($chains | ForEach-Object { $q = Split-Chain $_ $type; if ($q) { [pscustomobject]@{ Name = $_; Loc = $q.Loc; Pol = $q.Pol; F = $q.F } } })
    $c = @($gds | Where-Object { $_.Loc -eq $p.Loc -and $_.Pol -eq $p.Pol -and $_.F -eq 'FH' })
    if ($c.Count -gt 0) { return @($c[0].Name, 'FL -> FH') }
    $c = @($gds | Where-Object { $_.Loc -eq $p.Loc -and $_.F -eq 'FH' } | Sort-Object Name)
    if ($c.Count -gt 0) { return @($c[0].Name, '같은 위치 다른 극성') }
    if ($type -eq 'TSVDC' -and $corner -contains $p.Loc) {
        $c = @($gds | Where-Object { $corner -contains $_.Loc -and $_.Pol -eq '' } | Sort-Object Name)
        if ($c.Count -gt 0) { return @($c[0].Name, '모서리끼리') }
    }
    return @('', '')
}
$gdsPlan = New-Object Collections.Generic.List[object]
$gdsKeep = 0; $gdsNone = 0; $gdsSame = 0
foreach ($t in $targets) {
    $old = $setup[$t.Name]
    $oldGds = if ($old -ne $null) { [string]$old['GDS_CHAIN_NAME'] } else { '' }
    if ($oldGds -ne '' -and -not $OverwriteGds) { $gdsKeep++; continue }
    $name = ''; $how = ''
    if ($fromGds.ContainsKey($t.Name) -and $chainSet.ContainsKey($fromGds[$t.Name])) { $name = $fromGds[$t.Name]; $how = "$GdsFrom 와 같음" }
    else { $r = Find-ByRule $t.Chain1 $t.ChainType; $name = $r[0]; $how = $r[1] }
    if ($name -eq '') { $gdsNone++; continue }
    if ($name -eq $oldGds) { $gdsSame++; continue }
    $gdsPlan.Add([pscustomobject]@{ Name = $t.Name; Chain1 = $t.Chain1; Gds = $name; How = $how })
}
Write-Host ("[3] GDS Chain: GDS Device {0} 의 Setup Chain {1}개 / {2} 참고 {3}개 / 넣을 것 {4} / 이미 있어 그대로 {5} / 고른 값이 지금 값과 같음 {6} / 고를 것 없음 {7}" -f $Device, $chains.Count, $(if ($GdsFrom -ne '') { $GdsFrom } else { '(없음)' }), $fromGds.Count, $gdsPlan.Count, $gdsKeep, $gdsSame, $gdsNone)
$gdsPlan | Group-Object How | Sort-Object Name | ForEach-Object { Write-Host ("  {0}: {1}개" -f $_.Name, $_.Count) }
$gdsPlan | Group-Object Chain1 | Sort-Object Name | ForEach-Object { $g = $_.Group[0]; Write-Host ("    {0,-24} -> {1,-24} [{2}] {3}개" -f $g.Chain1, $g.Gds, $g.How, $_.Count) }

if (-not $Apply) {
    Write-Host '계획만 보여 줬습니다. 저장하려면 같은 명령에 -Apply 를 붙이세요.'
    Write-Host 'ET_PARA_SETUP_FROM_FILE=PLAN'
    return
}

# 4. 저장
$ok = $true
try {
    if ($newOnes.Count -gt 0) {
        $rows = New-Object 'string[,]' $newOnes.Count, 4
        for ($i = 0; $i -lt $newOnes.Count; $i++) {
            $rows[$i, 0] = $newOnes[$i].Header; $rows[$i, 1] = $newOnes[$i].Unit; $rows[$i, 2] = $newOnes[$i].TestNo; $rows[$i, 3] = $newOnes[$i].Column
        }
        $r = $bsl.RegisterEtParameters([string[]]@($Factory, $Device, $program, 'FILE', $f.FileName), $rows, $User).Rows[0]
        Write-Host ("저장 [1] 등록 {0} / 이미 있어 건너뜀 {1}" -f $r['REGISTERED'], $r['SKIPPED'])
    }
    if ($specPlan.Count -gt 0) {
        $rows = New-Object 'string[,]' $specPlan.Count, 7
        for ($i = 0; $i -lt $specPlan.Count; $i++) {
            $s = $specPlan[$i]
            $rows[$i, 0] = $rule::ToParameterName($s.HeaderText); $rows[$i, 1] = $s.Lower; $rows[$i, 2] = $s.Upper; $rows[$i, 3] = 'Y'
            $rows[$i, 4] = $s.Unit; $rows[$i, 5] = $s.SpecText; $rows[$i, 6] = ''
        }
        $n = $bsl.SaveEtLimit([string[]]@($Factory, $Device, $program), $rows, $User)
        Write-Host ("저장 [2] Spec {0}건" -f $n)
    }
    if ($gdsPlan.Count -gt 0) {
        $now = Read-Setup
        $rows = New-Object 'string[,]' $gdsPlan.Count, 7
        for ($i = 0; $i -lt $gdsPlan.Count; $i++) {
            $r = $now[$gdsPlan[$i].Name]
            $lo = Limit-Text $r['LOWER_LIMIT']; $up = Limit-Text $r['UPPER_LIMIT']
            $en = if (($lo -ne '' -or $up -ne '') -and [string]$r['ENABLE_FLAG'] -eq 'Y') { 'Y' } else { 'N' }
            $rows[$i, 0] = $gdsPlan[$i].Name; $rows[$i, 1] = $(if ([string]$r['USE_FLAG'] -eq 'N') { 'N' } else { 'Y' }); $rows[$i, 2] = [string]$r['REMARK']
            $rows[$i, 3] = $gdsPlan[$i].Gds; $rows[$i, 4] = $lo; $rows[$i, 5] = $up; $rows[$i, 6] = $en
        }
        $r = $bsl.SaveEtParameterSetup([string[]]@($Factory, $Device, $program), $rows, $User).Rows[0]
        Write-Host ("저장 [3] GDS Chain {0}행 (Spec 다시 씀 {1} / Spec 삭제 {2})" -f $r['UPDATED'], $r['SPEC_SAVED'], $r['SPEC_DELETED'])
        if ([int]$r['SPEC_SAVED'] -ne 0 -or [int]$r['SPEC_DELETED'] -ne 0) { $ok = $false; Write-Host '  확인 필요: GDS Chain 저장이 Spec 을 바꿨습니다.' }
    }
}
catch {
    $e = $_.Exception; while ($e.InnerException) { $e = $e.InnerException }
    Write-Host "저장 실패: $($e.Message)"
    Write-Host 'ET_PARA_SETUP_FROM_FILE=FAIL'
    exit 1
}

# 5. 다시 읽어 확인
$after = Read-Setup
$missing = @($targets | Where-Object { -not $after.ContainsKey($_.Name) })
$specMissing = @($specPlan | Where-Object { $n = $rule::ToParameterName($_.HeaderText); -not $after.ContainsKey($n) -or [string]$after[$n]['HAS_SPEC'] -ne 'Y' })
$gdsWrong = @($gdsPlan | Where-Object { -not $after.ContainsKey($_.Name) -or [string]$after[$_.Name]['GDS_CHAIN_NAME'] -ne $_.Gds })
$withSpec = @($targets | Where-Object { $after.ContainsKey($_.Name) -and [string]$after[$_.Name]['HAS_SPEC'] -eq 'Y' }).Count
$withGds = @($targets | Where-Object { $after.ContainsKey($_.Name) -and [string]$after[$_.Name]['GDS_CHAIN_NAME'] -ne '' }).Count
Write-Host ("다시 읽기: 등록 대상 {0} 중 등록 {1} / Spec {2} / GDS Chain {3} / 안 맞음: 등록 {4} · Spec {5} · GDS {6}" -f $targets.Count, ($targets.Count - $missing.Count), $withSpec, $withGds, $missing.Count, $specMissing.Count, $gdsWrong.Count)
if ($ok -and $missing.Count -eq 0 -and $specMissing.Count -eq 0 -and $gdsWrong.Count -eq 0) { Write-Host 'ET_PARA_SETUP_FROM_FILE=PASS' } else { Write-Host 'ET_PARA_SETUP_FROM_FILE=FAIL'; exit 1 }
