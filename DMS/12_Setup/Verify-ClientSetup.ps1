# DACrux SCK POC 클라이언트 Setup 자동 검사
#
# Build-ClientSetup.ps1 이 만든 stage 폴더로 시험용 Setup 2개를 따로 컴파일해 실제 설치 흐름을 돌려 본다.
# 시험용 Setup 은 다른 AppId / 이름 "(시험)" 으로 만들어 이 PC 의 실제 설치와 섞이지 않는다.
#   A (1.0.0.1): stage + 지난 버전에만 있는 표시 파일(zz_verify_old.dll)
#   B (1.0.0.2): stage 그대로 + .NET 4.8 설치 파일 포함 (이 PC 에는 .NET 이 있으므로 설치 단계는 건너뛰는 경로)
#
# 흐름
#   1. A 무음 설치 (/SERVERIP 지정) -> 파일 / ini / 등록 / 바로 가기 / 점검 결과 확인
#   2. 사용 흉내: ini UserID 변경, 임시 파일 생성, 사용자 파일 생성
#   3. B 무음 설치 (서버 주소 지정 안 함) -> 기존 버전 삭제 후 설치 / 설정 유지 / 지난 파일 삭제 / 사용자 파일 유지 확인
#   4. 설치 폴더에서 실행 파일(DACrux SCK POC.exe) 실행 -> 창 표시 + ini 다시 쓰기(설치 폴더 쓰기) 확인 후 종료
#   5. 무음 삭제 -> 등록 / 프로그램 파일 / 바로 가기 삭제, 사용자 파일 유지 확인
# 결과 마지막 줄: CLIENT_SETUP_VERIFY=PASS n/n 또는 CLIENT_SETUP_VERIFY=FAIL 실패수/전체
#
# 예) powershell -NoProfile -ExecutionPolicy Bypass -File Verify-ClientSetup.ps1 -StageDir %TEMP%\chippac\setup_build\2026.10.14.1\stage
param(
    [Parameter(Mandatory = $true)][string]$StageDir,
    [string]$WorkRoot = (Join-Path $env:TEMP ('chippac\setup_verify\' + (Get-Date -Format 'yyyyMMdd_HHmmss'))),
    [switch]$NoDotNetVariant
)

$ErrorActionPreference = 'Stop'
$SetupDir = $PSScriptRoot
$TestGuid = '8258B0F2-110A-49EC-AA22-52F18492815A'
$TestAppName = 'DACrux SCK POC (시험)'
$AppExeName = 'DACrux SCK POC.exe'
$UninstallKey = "HKCU:\Software\Microsoft\Windows\CurrentVersion\Uninstall\{$TestGuid}_is1"
$AppDir = Join-Path $WorkRoot 'app'
$TestServer = '10.1.2.3'
$script:Pass = 0
$script:FailCount = 0

function Check([string]$name, [bool]$ok, [string]$detail = '') {
    if ($ok) { $script:Pass++; Write-Host "  [PASS] $name $detail" -ForegroundColor Green }
    else { $script:FailCount++; Write-Host "  [FAIL] $name $detail" -ForegroundColor Red }
}

function Step([string]$text) { Write-Host ''; Write-Host "== $text" -ForegroundColor Cyan }

# DACrux 와 같은 규칙으로 [LAST OPTION] 값 읽기
function Get-LastOption([string]$iniPath, [string]$key) {
    $section = ''
    foreach ($line in Get-Content $iniPath -Encoding Default) {
        $t = $line.Trim()
        if ($t.StartsWith('[')) { $section = $t.ToUpper(); continue }
        $eq = $t.IndexOf('=')
        if ($section -eq '[LAST OPTION]' -and $eq -gt 0 -and $t.Substring(0, $eq).Trim() -ieq $key) { return $t.Substring($eq + 1).Trim() }
    }
    return $null
}

function Set-LastOption([string]$iniPath, [string]$key, [string]$value) {
    $lines = Get-Content $iniPath -Encoding Default
    $section = ''
    for ($i = 0; $i -lt $lines.Count; $i++) {
        $t = $lines[$i].Trim()
        if ($t.StartsWith('[')) { $section = $t.ToUpper(); continue }
        $eq = $t.IndexOf('=')
        if ($section -eq '[LAST OPTION]' -and $eq -gt 0 -and $t.Substring(0, $eq).Trim() -ieq $key) { $lines[$i] = "$key = $value" }
    }
    [IO.File]::WriteAllLines($iniPath, [string[]]$lines, [Text.Encoding]::ASCII)
}

# 파일목록.txt 대비 설치 폴더 비교 (ini 는 있는지만). 틀린 파일 수를 돌려준다.
function Compare-FileList([string]$dir) {
    $bad = 0; $total = 0
    foreach ($line in Get-Content (Join-Path $dir '파일목록.txt') -Encoding UTF8) {
        if (-not $line -or $line.StartsWith('#')) { continue }
        $cols = $line.Split("`t")
        $total++
        $full = Join-Path $dir $cols[0]
        if (-not (Test-Path -LiteralPath $full)) { $bad++; Write-Host "    누락: $($cols[0])"; continue }
        if ($cols[0] -like '*.ini') { continue }
        if ((Get-FileHash -LiteralPath $full -Algorithm SHA256).Hash -ne $cols[2]) { $bad++; Write-Host "    변경: $($cols[0])" }
    }
    return @($bad, $total)
}

function Invoke-Setup([string]$exe, [string[]]$extra, [string]$logName) {
    $log = Join-Path $WorkRoot $logName
    $setupArgs = @('/VERYSILENT', '/SUPPRESSMSGBOXES', '/NORESTART', "/DIR=`"$AppDir`"", "/LOG=`"$log`"") + $extra
    $p = Start-Process -FilePath $exe -ArgumentList $setupArgs -Wait -PassThru
    return @($p.ExitCode, $log)
}

function Wait-KeyGone([int]$seconds) {
    for ($i = 0; $i -lt ($seconds * 2); $i++) { if (-not (Test-Path $UninstallKey)) { return $true }; Start-Sleep -Milliseconds 500 }
    return -not (Test-Path $UninstallKey)
}

# ---- 준비 ----------------------------------------------------------------------
Step '0. 준비'
$Iscc = @("$env:LOCALAPPDATA\Programs\Inno Setup 7\ISCC.exe", "${env:ProgramFiles}\Inno Setup 7\ISCC.exe", "${env:ProgramFiles(x86)}\Inno Setup 7\ISCC.exe") |
    Where-Object { Test-Path $_ } | Select-Object -First 1
if (-not $Iscc) { Write-Host 'CLIENT_SETUP_VERIFY=FAIL ISCC 없음'; exit 1 }
if (-not (Test-Path (Join-Path $StageDir $AppExeName))) { Write-Host "CLIENT_SETUP_VERIFY=FAIL stage 가 아님: $StageDir"; exit 1 }
New-Item -ItemType Directory -Force $WorkRoot | Out-Null

# 이전 검사가 중간에 멈춰 시험 설치가 남아 있으면 먼저 지운다.
if (Test-Path $UninstallKey) {
    $old = (Get-ItemProperty $UninstallKey).UninstallString.Trim('"')
    Write-Host "  이전 시험 설치 삭제: $old"
    if (Test-Path $old) { Start-Process $old -ArgumentList '/VERYSILENT', '/SUPPRESSMSGBOXES', '/NORESTART' -Wait; [void](Wait-KeyGone 60) }
}

# A: stage + 지난 버전 표시 파일 (파일목록에도 추가)
$stageA = Join-Path $WorkRoot 'stageA'
& robocopy $StageDir $stageA /E /NFL /NDL /NJH /NJS /NP /MT:16 | Out-Null
$marker = Join-Path $stageA 'zz_verify_old.dll'
[IO.File]::WriteAllText($marker, 'verify marker')
$mHash = (Get-FileHash $marker -Algorithm SHA256).Hash
Add-Content -Path (Join-Path $stageA '파일목록.txt') -Value ("zz_verify_old.dll`t" + (Get-Item $marker).Length + "`t" + $mHash) -Encoding UTF8

$outSetup = Join-Path $WorkRoot 'setups'
$common = @('/Qp', "/DOutputDir=$outSetup", '/DTestMode', '/DDefaultServerIP=LOCALHOST', "/DAppExe=$AppExeName")
& $Iscc @common '/DAppVersion=1.0.0.1' "/DStageDir=$stageA" (Join-Path $SetupDir 'DACrux_SCK_POC_Client.iss') | Out-Null
Check '시험 Setup A 컴파일' ($LASTEXITCODE -eq 0)
$argsB = @('/DAppVersion=1.0.0.2', "/DStageDir=$StageDir")
$dotNet = Join-Path $SetupDir 'Prerequisites\NDP48-x86-x64-AllOS-ENU.exe'
if (-not $NoDotNetVariant -and (Test-Path $dotNet)) { $argsB += "/DDotNetInstaller=$dotNet" }
& $Iscc @common @argsB (Join-Path $SetupDir 'DACrux_SCK_POC_Client.iss') | Out-Null
Check '시험 Setup B 컴파일' ($LASTEXITCODE -eq 0) $(if ($argsB.Count -gt 2) { '(.NET 포함)' } else { '' })
$setupA = Join-Path $outSetup 'DACrux_SCK_POC_Client_Setup_TEST_1.0.0.1.exe'
$setupB = Join-Path $outSetup 'DACrux_SCK_POC_Client_Setup_TEST_1.0.0.2.exe'

# ---- 1. A 설치 -----------------------------------------------------------------
Step '1. 처음 설치 (A, 서버 주소 지정)'
$r = Invoke-Setup $setupA @("/SERVERIP=$TestServer") 'install_A.log'
Check 'A 설치 종료 코드 0' ($r[0] -eq 0) "(코드 $($r[0]))"
Check 'A 등록 버전' ((Get-ItemProperty $UninstallKey -ErrorAction SilentlyContinue).DisplayVersion -eq '1.0.0.1')
$cmp = Compare-FileList $AppDir
Check 'A 파일목록 일치' ($cmp[0] -eq 0) "($($cmp[1])개 중 틀림 $($cmp[0]))"
$ini = Join-Path $AppDir 'DACruxConfig.ini'
Check 'A ini ServerIP' ((Get-LastOption $ini 'ServerIP') -eq $TestServer) "($(Get-LastOption $ini 'ServerIP'))"
Check 'A ini EAIServerIP' ((Get-LastOption $ini 'EAIServerIP') -eq $TestServer)
Check 'A ini Factory / FactoryList' (((Get-LastOption $ini 'Factory') -eq 'SCK') -and ((Get-LastOption $ini 'FactoryList') -eq 'SCK'))
Check 'A DEFECT_IMAGE 폴더' (Test-Path (Join-Path $AppDir 'DEFECT_IMAGE'))
$lnkPath = Join-Path $env:APPDATA "Microsoft\Windows\Start Menu\Programs\$TestAppName\$TestAppName.lnk"
$lnkOk = $false
if (Test-Path -LiteralPath $lnkPath) {
    $lnk = (New-Object -ComObject WScript.Shell).CreateShortcut($lnkPath)
    $lnkOk = ($lnk.TargetPath -ieq (Join-Path $AppDir $AppExeName)) -and ($lnk.WorkingDirectory.TrimEnd('\') -ieq $AppDir)
}
Check 'A 시작 메뉴 바로 가기 (대상 / 시작 위치)' $lnkOk
$report = Join-Path $AppDir '설치점검_결과.txt'
$reportText = if (Test-Path -LiteralPath $report) { Get-Content -LiteralPath $report -Encoding UTF8 -Raw } else { '' }
Check 'A 설치 점검 결과 파일' ($reportText -match '요약 :')
Check 'A 점검: 설치 파일 일치' ($reportText -match '\[정상\] 설치 파일 :')
Check 'A 점검: 설치 폴더 쓰기' ($reportText -match '\[정상\] 설치 폴더 쓰기')
Check 'A 점검: 시험 서버 연결 실패 표시' ($reportText -match "\[실패\] 서버 연결 - 공통 / 로그인 : $([regex]::Escape($TestServer))")
Check 'A 지난 버전 표시 파일 설치됨' (Test-Path (Join-Path $AppDir 'zz_verify_old.dll'))

# ---- 2. 사용 흉내 -------------------------------------------------------------
Step '2. 사용 흉내 (설정 변경 / 임시 파일 / 사용자 파일)'
Set-LastOption $ini 'UserID' 'VERIFYUSER'
Set-Content (Join-Path $AppDir 'DEFECT_IMAGE\a.jpg') 'x'
Set-Content (Join-Path $AppDir 'excel_temp123.xls') 'x'
Set-Content (Join-Path $AppDir 'temp_chart1.bmp') 'x'
$userDir = Join-Path $AppDir '내캡처'
New-Item -ItemType Directory -Force $userDir | Out-Null
Set-Content (Join-Path $userDir 'a.png') 'x'
Check 'UserID 변경' ((Get-LastOption $ini 'UserID') -eq 'VERIFYUSER')

# ---- 3. B 설치 -----------------------------------------------------------------
Step '3. 새 버전 설치 (B, 서버 주소 지정 안 함 -> 기존 버전 삭제 후 설치)'
$r = Invoke-Setup $setupB @() 'install_B.log'
Check 'B 설치 종료 코드 0' ($r[0] -eq 0) "(코드 $($r[0]))"
$logB = Get-Content $r[1] -Encoding Default -Raw
Check 'B 로그: 기존 설치 인식' ($logB -match '1\.0\.0\.1')
Check 'B 등록 버전' ((Get-ItemProperty $UninstallKey -ErrorAction SilentlyContinue).DisplayVersion -eq '1.0.0.2')
Check 'B 지난 버전 전용 파일 삭제' (-not (Test-Path (Join-Path $AppDir 'zz_verify_old.dll')))
$cmp = Compare-FileList $AppDir
Check 'B 파일목록 일치' ($cmp[0] -eq 0) "($($cmp[1])개 중 틀림 $($cmp[0]))"
Check 'B ServerIP = 고정값 (기존 설치 값을 쓰지 않음)' ((Get-LastOption $ini 'ServerIP') -eq 'LOCALHOST') "($(Get-LastOption $ini 'ServerIP'))"
Check 'B UserID 유지' ((Get-LastOption $ini 'UserID') -eq 'VERIFYUSER') "($(Get-LastOption $ini 'UserID'))"
Check 'B 임시 파일 삭제 (excel_temp / temp_chart)' (-not (Test-Path (Join-Path $AppDir 'excel_temp123.xls')) -and -not (Test-Path (Join-Path $AppDir 'temp_chart1.bmp')))
Check 'B DEFECT_IMAGE 비움' ((Test-Path (Join-Path $AppDir 'DEFECT_IMAGE')) -and -not (Test-Path (Join-Path $AppDir 'DEFECT_IMAGE\a.jpg')))
Check 'B 사용자 파일 유지' (Test-Path (Join-Path $userDir 'a.png'))

# ---- 4. 실행 ---------------------------------------------------------------------
Step '4. 설치 폴더에서 프로그램 실행'
$iniBefore = (Get-Item $ini).LastWriteTime
# 로그인 창은 메인 창 없이 대화 상자로 뜨므로 Process.MainWindowHandle 은 0 이다. 프로세스의 보이는 최상위 창을 직접 찾는다.
if (-not ('VerifyWin' -as [type])) {
    Add-Type -TypeDefinition @"
using System; using System.Collections.Generic; using System.Runtime.InteropServices; using System.Text;
public static class VerifyWin {
    public delegate bool EnumProc(IntPtr h, IntPtr l);
    [DllImport("user32.dll")] public static extern bool EnumWindows(EnumProc f, IntPtr l);
    [DllImport("user32.dll")] public static extern uint GetWindowThreadProcessId(IntPtr h, out uint pid);
    [DllImport("user32.dll")] public static extern bool IsWindowVisible(IntPtr h);
    [DllImport("user32.dll", CharSet = CharSet.Unicode)] public static extern int GetWindowText(IntPtr h, StringBuilder s, int n);
    public static List<string> VisibleTitles(uint pid) {
        var r = new List<string>();
        EnumWindows((h, l) => { uint p; GetWindowThreadProcessId(h, out p); if (p == pid && IsWindowVisible(h)) { var sb = new StringBuilder(256); GetWindowText(h, sb, 256); if (sb.Length > 0) r.Add(sb.ToString()); } return true; }, IntPtr.Zero);
        return r;
    }
}
"@
}
$proc = Start-Process -FilePath (Join-Path $AppDir $AppExeName) -WorkingDirectory $AppDir -PassThru
$titles = @()
for ($i = 0; $i -lt 60; $i++) {
    Start-Sleep -Milliseconds 500
    if ($proc.HasExited) { break }
    $titles = @([VerifyWin]::VisibleTitles([uint32]$proc.Id))
    if ($titles -contains 'Login') { break }
}
Start-Sleep -Seconds 2
$alive = -not $proc.HasExited
Check '실행 후 로그인 창 표시 (종료되지 않음)' ($alive -and ($titles -contains 'Login')) "(보이는 창: $($titles -join ', '))"
$iniText = Get-Content $ini -Raw
Check '프로그램이 설치 폴더 ini 를 다시 씀 (쓰기 권한)' (((Get-Item $ini).LastWriteTime -gt $iniBefore) -and ($iniText -match '# Create Time'))
Check '프로그램이 쓴 ini 의 ServerIP (고정값)' ((Get-LastOption $ini 'ServerIP') -eq 'LOCALHOST')
$logDir = Join-Path $env:LOCALAPPDATA 'DACrux\Log'
$newErr = @(Get-ChildItem $logDir -File -ErrorAction SilentlyContinue | Where-Object { $_.LastWriteTime -gt $iniBefore })
Check '실행 중 오류 로그 없음' ($newErr.Count -eq 0) $(if ($newErr.Count) { '(' + ($newErr.Name -join ', ') + ')' } else { '' })
if (-not $proc.HasExited) {
    $proc.Kill()
    [void]$proc.WaitForExit(5000)
}

# ---- 5. 삭제 ---------------------------------------------------------------------
Step '5. 삭제'
$unins = Join-Path $AppDir 'unins000.exe'
Check '삭제 프로그램 있음' (Test-Path $unins)
if (Test-Path $unins) {
    Start-Process $unins -ArgumentList '/VERYSILENT', '/SUPPRESSMSGBOXES', '/NORESTART' -Wait
    Check '등록 삭제' (Wait-KeyGone 60)
    Start-Sleep -Seconds 1
    $leftBin = @(Get-ChildItem $AppDir -Recurse -Include *.dll, *.exe -File -ErrorAction SilentlyContinue)
    Check '프로그램 파일 삭제' ($leftBin.Count -eq 0) "(남은 DLL / EXE $($leftBin.Count)개)"
    Check 'ini / 점검 결과 삭제' (-not (Test-Path $ini) -and -not (Test-Path -LiteralPath $report))
    Check '시작 메뉴 바로 가기 삭제' (-not (Test-Path -LiteralPath (Split-Path $lnkPath)))
    Check '사용자 파일 유지' (Test-Path (Join-Path $userDir 'a.png'))
}

# ---- 결과 ----------------------------------------------------------------------
$total = $script:Pass + $script:FailCount
Write-Host ''
Write-Host "작업 폴더: $WorkRoot (설치 로그 install_A.log / install_B.log)"
if ($script:FailCount -eq 0) { Write-Host "CLIENT_SETUP_VERIFY=PASS $script:Pass/$total" -ForegroundColor Green; exit 0 }
Write-Host "CLIENT_SETUP_VERIFY=FAIL $script:FailCount/$total" -ForegroundColor Red
exit 1
