# DACrux SCK POC 클라이언트 Setup 한 번에 만들기 (2026-10-21)
#
# Build-ClientSetup.ps1(복사본 빌드 -> stage 검사 -> Setup 생성)을 실행하고,
# 통과하면 Verify-ClientSetup.ps1(시험 설치 -> 재설치 -> 실행 -> 삭제)까지 이어서 돌린다.
#   - 묻는 과정 없이 매개변수(기본값)대로 끝까지 진행한다.
#   - VS 의 DACrux_SCK_Setup 프로젝트(같은 폴더)를 빌드하면 이 스크립트가 실행된다. 설정은 SetupSettings.props.
#   - 결과: DMS\104_ClientSetup\<버전>\ (Setup.exe / 서버 DLL zip / 버전정보 / 파일목록 / 빌드기록)
#   - 복사 시점의 작업 폴더(미커밋 포함)를 빌드한다. 다른 작업이 소스를 고치는 도중이면 실패할 수 있으니 메시지를 보고 다시 빌드한다.
#   - 솔루션 빌드 경고(수백 줄)는 화면에 내지 않고 개수만 보여 준다. 전체는 %TEMP%\chippac\setup_build\<버전>\build_solution.log
#
# 예) powershell -NoProfile -ExecutionPolicy Bypass -File Make-ClientSetup.ps1
#     powershell -NoProfile -ExecutionPolicy Bypass -File Make-ClientSetup.ps1 -ServerIP 10.86.253.162 -NoVerify
#     powershell -NoProfile -ExecutionPolicy Bypass -File Make-ClientSetup.ps1 -Configuration Release
# 결과 마지막 줄: CLIENT_SETUP=PASS <Setup 경로> 또는 CLIENT_SETUP=FAIL <이유>
param(
    [string]$ServerIP = '10.86.253.162',
    # 솔루션 빌드 구성: Debug(개발 PC 와 같은 빌드, DEBUG 기호만 뺌) / Release(최적화된 배포용) - Build-ClientSetup.ps1 머리말 참고
    [ValidateSet('Debug', 'Release')][string]$Configuration = 'Debug',
    [string]$Version,
    [switch]$NoDotNet,
    [switch]$NoPdb,
    [switch]$NoServerBundle,
    [switch]$NoVerify,
    [switch]$OpenFolder,
    [switch]$ShowWarnings,
    # 시험용: 같은 버전의 이전 복사본 빌드 결과를 다시 쓰거나(-SkipBuild) 결과를 104_ClientSetup\_test 로 보낸다(-TestMode)
    [switch]$SkipBuild,
    [switch]$TestMode
)

$ErrorActionPreference = 'Stop'
$SetupDir = $PSScriptRoot

# 출력은 UTF-8 로 낸다(VS 출력 창 한글). 콘솔 코드 페이지가 함께 UTF-8 로 바뀌므로 같은 콘솔을 쓰는 자식 PowerShell 출력도 UTF-8 로 읽는다.
[Console]::OutputEncoding = New-Object System.Text.UTF8Encoding($false)
$ChildEncoding = New-Object System.Text.UTF8Encoding($false)

function Say([string]$text) { [Console]::Out.WriteLine($text) }
function Finish([bool]$ok, [string]$text)
{
    if ($ok) { Say "CLIENT_SETUP=PASS $text"; exit 0 }
    Say "CLIENT_SETUP=FAIL $text"; exit 1
}

# 자식 스크립트를 실행하고 출력 줄을 그대로 보여 준다. 솔루션 빌드 경고 줄은 세기만 한다.
function Invoke-Child([string]$ScriptName, [string[]]$ScriptArgs)
{
    $psi = New-Object System.Diagnostics.ProcessStartInfo
    $psi.FileName = 'powershell.exe'
    $psi.Arguments = '-NoProfile -ExecutionPolicy Bypass -File "' + (Join-Path $SetupDir $ScriptName) + '" ' + ($ScriptArgs -join ' ')
    $psi.WorkingDirectory = $SetupDir
    $psi.UseShellExecute = $false
    $psi.RedirectStandardOutput = $true
    $psi.RedirectStandardError = $true
    $psi.StandardOutputEncoding = $ChildEncoding
    $psi.StandardErrorEncoding = $ChildEncoding
    $p = [System.Diagnostics.Process]::Start($psi)
    $errTask = $p.StandardError.ReadToEndAsync()
    $lines = New-Object System.Collections.Generic.List[string]
    $warnings = 0
    while ($null -ne ($line = $p.StandardOutput.ReadLine()))
    {
        $lines.Add($line)
        if (-not $ShowWarnings -and $line -match ': warning [A-Z]+\d+') { $warnings++; continue }
        Say $line
    }
    $p.WaitForExit()
    $err = $errTask.Result
    if ($err) { Say $err.TrimEnd() }
    if ($warnings -gt 0) { Say "  (빌드 경고 $warnings 줄은 화면에서 생략)" }
    return [pscustomobject]@{ ExitCode = $p.ExitCode; Lines = $lines }
}

Say "DACrux SCK POC 클라이언트 Setup 만들기 (빌드 + 자동 검사)"
Say "  빌드 구성        : $Configuration"
Say "  서버 주소 기본값 : $ServerIP"
Say ("  버전             : " + $(if ($Version) { $Version } else { '오늘 날짜 + 순번 (자동)' }))
Say ("  자동 검사        : " + $(if ($NoVerify) { '안 함' } else { '함' }))

# ---- 1. Setup 빌드 ---------------------------------------------------------------
$buildArgs = @('-ServerIP', $ServerIP, '-Configuration', $Configuration)
if ($Version) { $buildArgs += @('-Version', $Version) }
if ($NoDotNet) { $buildArgs += '-NoDotNet' }
if ($NoPdb) { $buildArgs += '-NoPdb' }
if ($NoServerBundle) { $buildArgs += '-NoServerBundle' }
if ($SkipBuild) { $buildArgs += '-SkipBuild' }
if ($TestMode) { $buildArgs += '-TestMode' }

$build = Invoke-Child 'Build-ClientSetup.ps1' $buildArgs
$passLine = $build.Lines | Where-Object { $_ -like 'CLIENT_SETUP_BUILD=PASS *' } | Select-Object -Last 1
if ($build.ExitCode -ne 0 -or -not $passLine)
{
    $failLine = $build.Lines | Where-Object { $_ -like 'CLIENT_SETUP_BUILD=FAIL*' } | Select-Object -Last 1
    $reason = if ($failLine) { $failLine.Substring('CLIENT_SETUP_BUILD=FAIL'.Length).Trim() } else { "Build-ClientSetup.ps1 종료 코드 $($build.ExitCode)" }
    Finish $false "Setup 빌드 실패: $reason"
}
$setupPath = $passLine.Substring('CLIENT_SETUP_BUILD=PASS'.Length).Trim()
if (-not (Test-Path -LiteralPath $setupPath -PathType Leaf)) { Finish $false "만든 Setup 파일을 찾지 못했습니다: $setupPath" }
$builtVersion = Split-Path (Split-Path $setupPath -Parent) -Leaf
$stageDir = Join-Path $env:TEMP "chippac\setup_build\$builtVersion\stage"

# ---- 2. 자동 검사 ----------------------------------------------------------------
$verifyText = '건너뜀 (-NoVerify)'
if (-not $NoVerify)
{
    Say ''
    Say "== 자동 검사 (시험용 이름으로 설치 -> 재설치 -> 실행 -> 삭제, 로그인 창이 몇 초 떴다 닫힘)"
    if (-not (Test-Path $stageDir)) { Finish $false "자동 검사용 stage 폴더가 없습니다: $stageDir" }
    $verify = Invoke-Child 'Verify-ClientSetup.ps1' @('-StageDir', ('"' + $stageDir + '"'))
    $vLine = $verify.Lines | Where-Object { $_ -like 'CLIENT_SETUP_VERIFY=*' } | Select-Object -Last 1
    if ($verify.ExitCode -ne 0 -or -not $vLine -or $vLine -notlike 'CLIENT_SETUP_VERIFY=PASS*')
    {
        $why = if ($vLine) { $vLine } else { "Verify-ClientSetup.ps1 종료 코드 $($verify.ExitCode)" }
        Finish $false "Setup 은 만들었으나 자동 검사 실패 ($why) - $setupPath"
    }
    $verifyText = $vLine.Substring('CLIENT_SETUP_VERIFY='.Length)
}

# ---- 3. 결과 ---------------------------------------------------------------------
$file = Get-Item -LiteralPath $setupPath
$hash = (Get-FileHash -LiteralPath $setupPath -Algorithm SHA256).Hash
Say ''
Say '== 결과'
Say "  Setup     : $setupPath"
Say "  빌드 구성 : $Configuration"
Say ("  크기      : {0:N1} MB" -f ($file.Length / 1MB))
Say "  SHA-256   : $hash"
$zip = Join-Path (Split-Path $setupPath -Parent) "DACrux_SCK_POC_Server_${builtVersion}_DLL.zip"
if (Test-Path -LiteralPath $zip) { Say "  서버 묶음 : $zip" }
Say "  자동 검사 : $verifyText"
if ($OpenFolder) { Start-Process explorer.exe "/select,`"$setupPath`"" }
Finish $true $setupPath
