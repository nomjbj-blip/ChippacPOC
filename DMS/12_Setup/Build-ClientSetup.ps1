# DACrux SCK POC 클라이언트 Setup 만들기
#
# 운영 폴더(100_ClientBin / 101_ServerBin / 71_CoreLib)와 Git 추적 obj 를 바꾸지 않도록 DMS 빌드 대상 폴더를
# %TEMP%\chippac\setup_build\<버전> 에 복사한 뒤 그 복사본에서 솔루션을 다시 빌드한다.
#
# 빌드 구성 (-Configuration, 기본 Debug)
#   - Debug  : Debug 구성 + DEBUG 기호 제외 (/p:Configuration=Debug /p:DefineConstants=TRACE).
#              100_ClientBin 의 Debug 빌드는 로그인 없이 ADMIN 으로 들어가므로(#if DEBUG) 그대로 줄 수 없어 DEBUG 기호만 뺀다.
#              모든 프로젝트의 Debug 기호가 DEBUG;TRACE 이므로 TRACE 만 남기면 #if DEBUG 코드(자동 로그인 / 스택 표시 등)만 빠진다.
#              개발 PC 에서 시험한 것과 같은 참조 / 플랫폼, 최적화 없음.
#   - Release: Release 구성 그대로 (/p:Configuration=Release, 기호는 프로젝트별 Release 값). 최적화된 배포용 빌드.
#              2026-10-14 에 Release 의 참조 / 플랫폼(Miracom.Middleware / DACrux.ProjectManager / 서버 서비스 6개)을 Debug 와 맞췄다.
#              클라이언트는 Debug 와 같은 코드 경로(DEBUG 없음)이고, 서버 묶음의 데이터 서비스는 SINGLE_EQUIP(설비별 Windows 서비스)로 빌드된다.
#   - 어느 쪽이든 5단계 검사가 자동 로그인 코드 / x86 전용 DLL / Middleware 판을 확인한다.
#
# 단계
#   1. 도구 확인 (MSBuild / ISCC / .NET 4.8 설치 파일 서명)
#   2. DMS 복사 -> 솔루션 다시 빌드 (-SkipBuild 면 같은 버전의 이전 빌드 결과 사용)
#   3. 설치 점검 도구(EnvCheck) 빌드
#   4. stage 구성: 복사본 100_ClientBin - 제외 규칙 + 점검 도구 + 새 DACruxConfig.ini
#   5. stage 검사: 자동 로그인 코드 없음 / x86 전용 DLL / Middleware / 빌드로 갱신되지 않은 DLL / 없는 참조 어셈블리
#   6. 버전정보.txt / 파일목록.txt (SHA-256)
#   7. ISCC 로 Setup 생성 -> DMS\104_ClientSetup\<버전>\
#   8. 서버 묶음: 같은 빌드가 만든 서버 DLL / EXE ZIP (config 제외, 짝 맞추기용)
#
# 예)
#   powershell -NoProfile -ExecutionPolicy Bypass -File Build-ClientSetup.ps1 -ServerIP 10.10.1.20
#   powershell -NoProfile -ExecutionPolicy Bypass -File Build-ClientSetup.ps1 -Version 2026.10.14.1 -ServerIP LOCALHOST
#   powershell -NoProfile -ExecutionPolicy Bypass -File Build-ClientSetup.ps1 -Configuration Release -ServerIP 10.86.253.162
# 결과 마지막 줄: CLIENT_SETUP_BUILD=PASS <Setup 경로> 또는 CLIENT_SETUP_BUILD=FAIL <이유>
param(
    [string]$Version,
    [string]$ServerIP = 'LOCALHOST',
    [ValidateSet('Debug', 'Release')][string]$Configuration = 'Debug',
    [switch]$NoPdb,
    [switch]$NoDotNet,
    [switch]$NoServerBundle,
    [switch]$TestMode,
    [switch]$SkipBuild,
    [string]$BuildRoot
)

$ErrorActionPreference = 'Stop'
$SetupDir = $PSScriptRoot
$DmsRoot = Split-Path $SetupDir -Parent
$RepoRoot = Split-Path $DmsRoot -Parent
$OutRoot = Join-Path $DmsRoot '104_ClientSetup'
$Utf8Bom = New-Object System.Text.UTF8Encoding($true)
$Warnings = New-Object System.Collections.Generic.List[string]

function Step([string]$text) { Write-Host ''; Write-Host "== $text" -ForegroundColor Cyan }
function Warn([string]$text) { $Warnings.Add($text); Write-Host "  [경고] $text" -ForegroundColor Yellow }
function Fail([string]$text) { Write-Host "CLIENT_SETUP_BUILD=FAIL $text" -ForegroundColor Red; exit 1 }

# ---- 버전 / 경로 ---------------------------------------------------------------
if (-not $Version) {
    $today = Get-Date
    $prefix = '{0}.{1}.{2}' -f $today.Year, $today.Month, $today.Day
    $n = 1
    while (Test-Path (Join-Path $OutRoot "$prefix.$n")) { $n++ }
    $Version = "$prefix.$n"
}
if ($Version -notmatch '^\d{4}\.\d{1,2}\.\d{1,2}\.\d{1,5}$') { Fail "버전 형식은 yyyy.M.d.N 입니다: $Version" }
if (-not $BuildRoot) { $BuildRoot = Join-Path $env:TEMP "chippac\setup_build\$Version" }
$OutDir = if ($TestMode) { Join-Path $OutRoot "_test\$Version" } else { Join-Path $OutRoot $Version }
$CopyDms = Join-Path $BuildRoot 'DMS'
$Stage = Join-Path $BuildRoot 'stage'
$Sln = Join-Path $CopyDms '00_Solution\DACrux2015wc_ChippacPOC\DACrux2015wc.sln'
$DotNetFile = Join-Path $SetupDir 'Prerequisites\NDP48-x86-x64-AllOS-ENU.exe'
$ProductName = 'DACrux SCK POC'
$FilePrefix = 'DACrux_SCK_POC'
# 설치 폴더의 실행 파일 이름. 개발 / 운영 폴더는 DACruxV5.exe 그대로 두고 stage 에서만 바꾼다 (2026-10-14 결정)
$AppExeName = 'DACrux SCK POC.exe'
# 프로그램 아이콘 (실행 파일 / Setup / 점검 도구 공통, 2026-10-14 통일)
$MainIcon = Join-Path $DmsRoot '01_WinApp\DACruxV5\Resources\MainIcon.ico'

# 솔루션 빌드 구성 (머리말 참고)
$ConfigText = if ($Configuration -eq 'Release') { 'Release 구성 (최적화, 프로젝트별 Release 기호)' } else { 'Debug 구성 + DEBUG 기호 제외' }
# 인수가 하나여도 배열로 남도록 @() 로 감싼다(그대로 두면 문자열 하나가 되어 @ConfigArgs 가 글자 단위로 펼쳐진다)
$ConfigArgs = @(if ($Configuration -eq 'Release') { '/p:Configuration=Release' } else { '/p:Configuration=Debug', '/p:DefineConstants=TRACE' })

Write-Host "$ProductName 클라이언트 Setup 빌드"
Write-Host "  버전       : $Version"
Write-Host "  빌드 구성  : $ConfigText"
Write-Host "  서버 기본값: $ServerIP"
Write-Host "  빌드 폴더  : $BuildRoot"
Write-Host "  출력 폴더  : $OutDir"

# ---- 1. 도구 확인 --------------------------------------------------------------
Step '1. 도구 확인'
$vswhere = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"
$MsBuild = & $vswhere -latest -products * -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
if (-not $MsBuild -or -not (Test-Path $MsBuild)) { Fail 'MSBuild 를 찾지 못했습니다 (Visual Studio 설치 확인)' }
Write-Host "  MSBuild : $MsBuild"

$Iscc = @(
    "$env:LOCALAPPDATA\Programs\Inno Setup 7\ISCC.exe",
    "${env:ProgramFiles}\Inno Setup 7\ISCC.exe",
    "${env:ProgramFiles(x86)}\Inno Setup 7\ISCC.exe"
) | Where-Object { Test-Path $_ } | Select-Object -First 1
if (-not $Iscc) { Fail 'Inno Setup 7 (ISCC.exe) 이 없습니다. winget install JRSoftware.InnoSetup.7 --scope user' }
Write-Host "  ISCC    : $Iscc"

$IncludeDotNet = -not $NoDotNet
if ($IncludeDotNet) {
    if (-not (Test-Path $DotNetFile)) { Fail ".NET 4.8 설치 파일이 없습니다: $DotNetFile (넣지 않으려면 -NoDotNet)" }
    $sig = Get-AuthenticodeSignature $DotNetFile
    if ($sig.Status -ne 'Valid' -or $sig.SignerCertificate.Subject -notmatch 'O=Microsoft Corporation') {
        Fail ".NET 설치 파일 서명이 올바르지 않습니다: $($sig.Status) / $($sig.SignerCertificate.Subject)"
    }
    Write-Host "  .NET 4.8: $DotNetFile (Microsoft 서명 확인)"
} else {
    Write-Host '  .NET 4.8: 넣지 않음 (Setup 이 없을 때 안내 후 중단)'
}

# ---- 2. DMS 복사 / 솔루션 빌드 --------------------------------------------------
$BuildStartFile = Join-Path $BuildRoot 'build_start.txt'
$BuildConfigFile = Join-Path $BuildRoot 'build_config.txt'
if ($SkipBuild) {
    Step '2. 솔루션 빌드 건너뜀 (-SkipBuild)'
    if (-not (Test-Path $BuildStartFile)) { Fail "이전 빌드 결과가 없습니다: $BuildRoot" }
    # 구성 기록이 없는 이전 빌드는 Debug 로 만든 것이다
    $prevConfig = if (Test-Path $BuildConfigFile) { (Get-Content $BuildConfigFile).Trim() } else { 'Debug' }
    if ($prevConfig -ne $Configuration) { Fail "이전 빌드는 $prevConfig 구성입니다. -Configuration $prevConfig 로 실행하거나 -SkipBuild 없이 다시 빌드하세요" }
    $BuildStart = [datetime]::ParseExact((Get-Content $BuildStartFile), 'yyyy-MM-dd HH:mm:ss', $null)
} else {
    Step "2. DMS 복사 -> 솔루션 빌드 ($ConfigText)"
    if (Test-Path $CopyDms) { Remove-Item $CopyDms -Recurse -Force }
    New-Item -ItemType Directory -Force $CopyDms | Out-Null

    # 솔루션 프로젝트 / HintPath 가 있는 폴더 + 작은 리소스 폴더. INSP_BACKUP / Migration / Assistant 는 필요 없다.
    $folders = '00_Solution', '01_WinApp', '02_DMS', '03_STAT', '04_MESIF', '05_RefLib', '06_CoreLibSource', '07_CoreLib',
               '08_DBQuery', '09_TEST', '71_CoreLib', '99_Resource', '100_ClientBin', '101_ServerBin'
    foreach ($f in $folders) {
        $src = Join-Path $DmsRoot $f
        if (-not (Test-Path $src)) { continue }
        & robocopy $src (Join-Path $CopyDms $f) /E /XD obj .vs TestResults /NFL /NDL /NJH /NJS /NP /MT:16 /R:1 /W:1 | Out-Null
        if ($LASTEXITCODE -ge 8) { Fail "복사 실패($LASTEXITCODE): $f" }
    }
    Write-Host "  복사 완료: $CopyDms"

    # 솔루션의 모든 프로젝트가 복사본에 있는지 먼저 확인한다.
    $slnDir = Split-Path $Sln
    $missingProj = Select-String -Path $Sln -Pattern '^Project\("\{[^}]+\}"\) = "[^"]+", "([^"]+\.csproj)"' |
        ForEach-Object { $_.Matches[0].Groups[1].Value } |
        Where-Object { -not (Test-Path (Join-Path $slnDir $_)) }
    if ($missingProj) { Fail ('복사본에 없는 프로젝트: ' + ($missingProj -join ', ')) }

    $BuildStart = (Get-Date).AddSeconds(-2)
    Set-Content -Path $BuildStartFile -Value $BuildStart.ToString('yyyy-MM-dd HH:mm:ss') -Encoding ASCII
    Set-Content -Path $BuildConfigFile -Value $Configuration -Encoding ASCII
    $log = Join-Path $BuildRoot 'build_solution.log'
    Write-Host "  솔루션 다시 빌드 중: $ConfigText (몇 분 걸림)..."
    # 모든 프로젝트가 같은 출력 폴더로 복사되므로 병렬 빌드의 파일 경합을 피해 순서대로 빌드한다.
    & $MsBuild $Sln /t:Rebuild @ConfigArgs "/p:Platform=Any CPU" /m:1 /nr:false /nologo /v:quiet "/fl" "/flp:LogFile=$log;Verbosity=normal;Encoding=UTF-8" | Out-Host
    $code = $LASTEXITCODE
    $summary = Select-String -Path $log -Pattern '^\s+\d+ (Warning|Error)\(s\)|^\s+\d+개 경고|^\s+\d+개 오류|경고 \d+개|오류 \d+개' | ForEach-Object { $_.Line.Trim() }
    Write-Host ('  결과: ' + ($summary -join ' / '))
    if ($code -ne 0) { Fail "솔루션 빌드 실패 (로그: $log)" }
}

# ---- 3. 설치 점검 도구 빌드 ------------------------------------------------------
Step '3. 설치 점검 도구 빌드'
$envOut = Join-Path $BuildRoot 'envcheck'
& $MsBuild (Join-Path $SetupDir 'EnvCheck\SCK.EnvCheck.csproj') /t:Rebuild /p:Configuration=Release "/p:OutDir=$envOut\bin\\" "/p:IntermediateOutputPath=$envOut\obj\\" /nologo /v:minimal | Out-Host
if ($LASTEXITCODE -ne 0) { Fail '설치 점검 도구 빌드 실패' }

# ---- 4. stage 구성 -------------------------------------------------------------
Step '4. 설치 파일 모으기 (stage)'
$clientBin = Join-Path $CopyDms '100_ClientBin'
if (Test-Path $Stage) { Remove-Item $Stage -Recurse -Force }
New-Item -ItemType Directory -Force $Stage | Out-Null

$rules = Get-Content (Join-Path $SetupDir 'ClientSetup.exclude.txt') -Encoding UTF8 |
    ForEach-Object { $_.Trim() } | Where-Object { $_ -and -not $_.StartsWith('#') }

function Test-Excluded([string]$rel, [string]$full) {
    $name = Split-Path $rel -Leaf
    if ($name -eq 'DACruxConfig.ini' -and -not $rel.Contains('\')) { return $true }
    if ($NoPdb -and $name -like '*.pdb') { return $true }
    if ($name -like '*.xml') {
        $base = [IO.Path]::Combine((Split-Path $full), [IO.Path]::GetFileNameWithoutExtension($name))
        if ((Test-Path "$base.dll") -or (Test-Path "$base.exe")) { return $true }
    }
    foreach ($r in $rules) {
        if ($r.EndsWith('\')) { if ($rel -like ($r + '*')) { return $true } }
        elseif ($r.Contains('\')) { if ($rel -like $r) { return $true } }
        elseif ($name -like $r) { return $true }
    }
    return $false
}

$picked = 0; $skipped = 0
foreach ($f in Get-ChildItem $clientBin -Recurse -File) {
    $rel = $f.FullName.Substring($clientBin.Length + 1)
    if (Test-Excluded $rel $f.FullName) { $skipped++; continue }
    $dst = Join-Path $Stage $rel
    New-Item -ItemType Directory -Force (Split-Path $dst) | Out-Null
    Copy-Item -LiteralPath $f.FullName -Destination $dst
    $picked++
}
Write-Host "  100_ClientBin: 넣음 $picked / 뺌 $skipped"

foreach ($n in 'SCK_EnvCheck.exe', 'SCK_EnvCheck.exe.config', 'SCK_EnvCheck.pdb') {
    if ($NoPdb -and $n -like '*.pdb') { continue }
    Copy-Item (Join-Path $envOut "bin\$n") $Stage
}

# DACruxConfig.ini: 원본의 [APPLICATION UNIT] 포트 줄만 옮겨 ASCII 로 새로 만든다.
# (DACrux 는 모르는 포트 이름이 있으면 ini 전체를 기본값으로 다시 쓰므로 원본 줄을 그대로 쓴다. 서버 주소는 Setup 이 기록한다)
$srcIni = Get-Content (Join-Path $clientBin 'DACruxConfig.ini') -Encoding Default
$portLines = New-Object System.Collections.Generic.List[string]
$section = ''
foreach ($line in $srcIni) {
    $t = $line.Trim()
    if ($t.StartsWith('[')) { $section = $t.ToUpper(); continue }
    if ($section -eq '[APPLICATION UNIT]' -and $t -match '^([A-Z_]+)\s*=\s*(\d+)$') { $portLines.Add("$($Matches[1]) = $($Matches[2])") }
}
if ($portLines.Count -eq 0) { Fail '원본 DACruxConfig.ini 에서 포트 설정을 찾지 못했습니다' }
$ini = @('# DACrux Configuration ini file', "# Service port define of DACrux's Application module", '[APPLICATION UNIT]') + $portLines + @(
    '', '[CONFIG MODE]', '# Method of Application Configuration', '# Configuration mode has two option that "Registry" or "INIFile"', 'CONFIG_STOCK = INIFile',
    '', '[LAST OPTION]', 'UserID = ', "ServerIP = $ServerIP", "EAIServerIP = $ServerIP", 'EAIServerPort = 10101', 'EAIChannel = /QMS',
    'Language = English', 'LanguageFile = Global.Lng', 'Site = ', 'Factory = SCK', 'FactoryList = SCK', 'RibbonMenuEnable = False')
[IO.File]::WriteAllLines((Join-Path $Stage 'DACruxConfig.ini'), [string[]]$ini, [Text.Encoding]::ASCII)
Write-Host "  DACruxConfig.ini: 포트 $($portLines.Count)줄 + 기본 설정 (ServerIP = $ServerIP)"

# 실행 파일 이름을 설치본 이름으로 바꾼다. .NET EXE 는 파일 이름이 달라도 그대로 실행된다(PDB 는 원래 이름 DACruxV5.pdb 로 찾음).
Rename-Item -LiteralPath (Join-Path $Stage 'DACruxV5.exe') -NewName $AppExeName
if (Test-Path -LiteralPath (Join-Path $Stage 'DACruxV5.exe.config')) { Rename-Item -LiteralPath (Join-Path $Stage 'DACruxV5.exe.config') -NewName "$AppExeName.config" }
Write-Host "  실행 파일 이름: DACruxV5.exe -> $AppExeName"

# ---- 5. stage 검사 -------------------------------------------------------------
Step '5. 설치 파일 검사'
$inspect = & powershell.exe -NoProfile -ExecutionPolicy Bypass -File (Join-Path $SetupDir 'Inspect-ClientStage.ps1') -StageDir $Stage -ExeName $AppExeName -IconFile $MainIcon
if ($LASTEXITCODE -ne 0) { Fail ('설치 파일 검사 실패: ' + ($inspect -join ' ')) }
$inspect | ForEach-Object { Write-Host "  $_" }
if (-not ($inspect -contains 'ICON_MATCH=True')) { Fail "$AppExeName 아이콘이 MainIcon.ico 가 아닙니다 (DACruxV5.csproj ApplicationIcon 확인)" }
if (-not ($inspect -contains 'AUTO_LOGIN_CODE=False')) { Fail "$AppExeName 에 자동 로그인(DEBUG) 코드가 들어 있습니다" }
$x86New = $inspect | Where-Object { $_ -like 'X86_NEW=*' }
if ($x86New) { Fail ('64비트 실행에서 로드할 수 없는 x86 전용 DLL: ' + (($x86New | ForEach-Object { $_.Substring(8) }) -join ', ')) }
$inspect | Where-Object { $_ -like 'REF_NEW=*' } | ForEach-Object { Warn ('새로 생긴 없는 참조: ' + $_.Substring(8)) }

# Miracom.Middleware 는 개발 PC / 운영 폴더와 같은 파일이어야 한다 (2단계 참조 맞춤 확인)
$mwStage = (Get-FileHash (Join-Path $Stage 'Miracom.Middleware.dll') -Algorithm MD5).Hash
$mwDev = (Get-FileHash (Join-Path $DmsRoot '07_CoreLib\Miracom.Middleware.dll') -Algorithm MD5).Hash
if ($mwStage -ne $mwDev) { Fail "Miracom.Middleware.dll 이 개발용 파일과 다릅니다 (stage $($mwStage.Substring(0,8)) / 07_CoreLib $($mwDev.Substring(0,8)))" }
Write-Host "  Miracom.Middleware.dll: 개발용과 같음 ($($mwDev.Substring(0,8)))"

# 이번 빌드에서 갱신되지 않은 DLL / EXE. 운영 100_ClientBin 과 같으면 개발 PC 에서 쓰는 것과 같은 파일이다.
# 이름이 DACrux / NexplantQMS 인데 운영 폴더와 다르면 빌드 순서 / 출력 경로 문제일 수 있어 경고한다.
$sameAsDev = 0; $diffNames = New-Object System.Collections.Generic.List[string]
foreach ($s in (Get-ChildItem $Stage -Recurse -Include *.dll, *.exe -File | Where-Object { $_.LastWriteTime -lt $BuildStart })) {
    $rel = $s.FullName.Substring($Stage.Length + 1)
    $dev = Join-Path $DmsRoot "100_ClientBin\$rel"
    if ((Test-Path -LiteralPath $dev) -and (Get-FileHash -LiteralPath $dev -Algorithm MD5).Hash -eq (Get-FileHash -LiteralPath $s.FullName -Algorithm MD5).Hash) { $sameAsDev++; continue }
    if ($s.Name -match '^(DACrux|NexplantQMS|SCK_)') { Warn "이번 빌드로 갱신되지 않았고 운영 폴더와도 다름: $rel" } else { $diffNames.Add($rel) }
}
Write-Host "  빌드 대상이 아닌 DLL / EXE: 운영 100_ClientBin 과 같음 $sameAsDev 개"
if ($diffNames.Count -gt 0) { Write-Host ('  운영 폴더와 다른 외부 DLL: ' + ($diffNames -join ', ')) }

# ---- 6. 버전정보 / 파일목록 ------------------------------------------------------
Step '6. 버전정보.txt / 파일목록.txt'
Push-Location $RepoRoot
try {
    $commit = (& git rev-parse --short HEAD 2>$null)
    $dirty = @(& git status --porcelain -- DMS 2>$null).Count
} finally { Pop-Location }
$info = @(
    "제품 = $ProductName 클라이언트",
    "버전 = $Version",
    ('빌드 시각 = ' + (Get-Date).ToString('yyyy-MM-dd HH:mm:ss')),
    $(if ($Configuration -eq 'Release') { '빌드 구성 = Release 구성(최적화, 프로젝트별 Release 기호, 참조 / 플랫폼은 Debug 와 같음)' } else { '빌드 구성 = Debug 구성(개발 PC 와 같은 참조 / 플랫폼) + DEBUG 기호 제외(로그인 필요)' }),
    "Git commit = $commit (DMS 미커밋 변경 $dirty 개 포함)",
    "빌드 PC = $env:COMPUTERNAME",
    "서버 주소 기본값 = $ServerIP",
    ('.NET 4.8 포함 = ' + $(if ($IncludeDotNet) { '예' } else { '아니오' }))
)
[IO.File]::WriteAllLines((Join-Path $Stage '버전정보.txt'), [string[]]$info, $Utf8Bom)

$listLines = New-Object System.Collections.Generic.List[string]
$listLines.Add("# $ProductName 클라이언트 설치 파일 목록 (상대 경로 / 크기 / SHA-256)")
$listLines.Add("# 버전 $Version")
foreach ($f in (Get-ChildItem $Stage -Recurse -File | Sort-Object FullName)) {
    $rel = $f.FullName.Substring($Stage.Length + 1)
    $hash = (Get-FileHash -LiteralPath $f.FullName -Algorithm SHA256).Hash
    $listLines.Add("$rel`t$($f.Length)`t$hash")
}
[IO.File]::WriteAllLines((Join-Path $Stage '파일목록.txt'), $listLines.ToArray(), $Utf8Bom)
$stageFiles = Get-ChildItem $Stage -Recurse -File
Write-Host ("  stage: {0}개 / {1:N1} MB" -f $stageFiles.Count, (($stageFiles | Measure-Object Length -Sum).Sum / 1MB))

# ---- 7. Setup 생성 -------------------------------------------------------------
Step '7. Setup 생성 (ISCC)'
New-Item -ItemType Directory -Force $OutDir | Out-Null
$isccArgs = @('/Qp', "/DAppVersion=$Version", "/DStageDir=$Stage", "/DOutputDir=$OutDir", "/DDefaultServerIP=$ServerIP")
$icon = $MainIcon
if (Test-Path $icon) { $isccArgs += "/DIconFile=$icon" }
if ($IncludeDotNet) { $isccArgs += "/DDotNetInstaller=$DotNetFile" }
if ($TestMode) { $isccArgs += '/DTestMode' }
$isccArgs += "/DAppExe=$AppExeName"
$isccArgs += (Join-Path $SetupDir 'DACrux_SCK_POC_Client.iss')
& $Iscc @isccArgs | Out-Host
if ($LASTEXITCODE -ne 0) { Fail "ISCC 실패 ($LASTEXITCODE)" }
$setupFile = Get-ChildItem $OutDir -Filter '*.exe' | Sort-Object LastWriteTime -Descending | Select-Object -First 1
Copy-Item (Join-Path $Stage '버전정보.txt'), (Join-Path $Stage '파일목록.txt') $OutDir -Force

# ---- 8. 서버 묶음 --------------------------------------------------------------
if (-not $NoServerBundle -and -not $TestMode) {
    Step '8. 서버 묶음 (같은 빌드의 서버 DLL / EXE)'
    $serverBin = Join-Path $CopyDms '101_ServerBin'
    $bundle = Join-Path $BuildRoot 'server_bundle'
    if (Test-Path $bundle) { Remove-Item $bundle -Recurse -Force }
    New-Item -ItemType Directory -Force $bundle | Out-Null
    $count = 0
    foreach ($f in Get-ChildItem $serverBin -Recurse -Include *.dll, *.exe, *.pdb -File) {
        if ($f.LastWriteTime -lt $BuildStart -or $f.Name -like '*.vshost.exe') { continue }
        if ($NoPdb -and $f.Name -like '*.pdb') { continue }
        $rel = $f.FullName.Substring($serverBin.Length + 1)
        $dst = Join-Path $bundle $rel
        New-Item -ItemType Directory -Force (Split-Path $dst) | Out-Null
        Copy-Item -LiteralPath $f.FullName -Destination $dst
        $count++
    }
    $readme = @(
        "$ProductName 서버 묶음 $Version",
        '',
        "포함: 이 버전 클라이언트 Setup 과 같은 소스 / 같은 빌드($ConfigText)로 만든 서버 DLL / EXE (/ PDB).",
        '제외: *.config (DB 접속 / wellknown / KLARF_IMAGE_ROOT), 외부 DLL, Query XML. 서버에 있는 것을 그대로 둔다.',
        '',
        '배포: 서비스를 멈춘 뒤 101_ServerBin 에 덮어쓰고 버전정보.txt 도 같이 둔다. 그 다음 서비스를 다시 시작한다.',
        '주의: DEBUG 기호 없이 빌드해 서비스 시작 / 중지 로그를 DB 에 남긴다 (개발 PC 의 Debug 서버와 다른 점).'
    )
    if ($Configuration -eq 'Release') {
        $readme += '주의: Release 구성이라 데이터 서비스(Inspection / PreConverting / Review / CP / PCM)는 SINGLE_EQUIP(설비별 Windows 서비스 이름)으로 빌드된다.'
        $readme += '      DACrux.SEMDMS.BSL 은 Release 구성에도 DEBUG 기호가 있다(그 소스에 DEBUG 분기 / Debug 호출 없음, 2026-10-21 확인).'
    }
    [IO.File]::WriteAllLines((Join-Path $bundle '읽어보기.txt'), [string[]]$readme, $Utf8Bom)
    Copy-Item (Join-Path $Stage '버전정보.txt') $bundle
    $zip = Join-Path $OutDir "${FilePrefix}_Server_${Version}_DLL.zip"
    if (Test-Path $zip) { Remove-Item $zip -Force }
    Add-Type -AssemblyName System.IO.Compression.FileSystem
    [IO.Compression.ZipFile]::CreateFromDirectory($bundle, $zip, [IO.Compression.CompressionLevel]::Optimal, $false)
    Write-Host ("  {0}: 파일 {1}개 / {2:N1} MB" -f (Split-Path $zip -Leaf), $count, ((Get-Item $zip).Length / 1MB))
}

# ---- 결과 ----------------------------------------------------------------------
Step '결과'
$setupHash = (Get-FileHash $setupFile.FullName -Algorithm SHA256).Hash
$report = @(
    "Setup = $($setupFile.FullName)",
    ('크기 = {0:N1} MB' -f ($setupFile.Length / 1MB)),
    "SHA-256 = $setupHash",
    "빌드 구성 = $ConfigText",
    "stage 파일 수 = $($stageFiles.Count)",
    "빌드 폴더 = $BuildRoot",
    ('경고 = ' + $Warnings.Count)
) + ($Warnings | ForEach-Object { "  - $_" })
[IO.File]::WriteAllLines((Join-Path $OutDir '빌드기록.txt'), [string[]]($info + @('') + $report), $Utf8Bom)
$report | ForEach-Object { Write-Host "  $_" }
Write-Host "CLIENT_SETUP_BUILD=PASS $($setupFile.FullName)" -ForegroundColor Green
