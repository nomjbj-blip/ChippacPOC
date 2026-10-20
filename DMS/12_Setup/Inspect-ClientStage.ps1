# 클라이언트 Setup 에 넣을 폴더(stage)를 검사한다. Build-ClientSetup.ps1 이 별도 프로세스로 부른다.
# (어셈블리를 읽으면 프로세스가 끝날 때까지 파일이 잡히므로 빌드 스크립트 프로세스와 분리한다)
#
# 검사 내용
#   1. 실행 파일(-ExeName, 설치본 이름)에 DEBUG 전용 자동 로그인 코드가 없는지 (DEBUG 빌드는 로그인 없이 ADMIN 으로 들어간다)
#      #if DEBUG 안에만 있는 메서드 이름(mMain_Load)이 메타데이터에 없으면 DEBUG 기호 없이 빌드된 것이다.
#   1-1. 실행 파일 아이콘이 -IconFile(MainIcon.ico)인지
#   2. x86 전용 .NET DLL / EXE. DACrux 는 64비트로 실행되므로 이런 DLL 은 로드할 수 없다.
#      알려진 것(NMath 커널, 지금도 쓰이지 않음)과 새로 생긴 것을 나눠 출력한다.
#   3. 참조 어셈블리 중 stage 에도 .NET Framework 4.8 기본 구성에도 없는 것
#      알려진 것(POC 에서 쓰지 않는 기능)과 새로 생긴 것을 나눠 출력한다.
# 출력: KEY=값 줄. 빌드 스크립트가 읽어 판정한다.
param(
    [Parameter(Mandatory = $true)][string]$StageDir,
    [string]$ExeName = 'DACruxV5.exe',
    [string]$IconFile
)

$ErrorActionPreference = 'Stop'

# 2026-10-14 확인: 개발 PC 에도 없고 POC 에서 쓰지 않는 기능의 참조 (통계 / 영상 캡처 / 펜 입력 / ClosedXML 셀 표시 형식)
$known = @('NMathCoreMath', 'NMathCoreShared', 'NMathStatsMain', 'AForge', 'AForge.Imaging', 'Microsoft.Ink', 'ExcelNumberFormat')
# 2026-10-14 확인: 운영 100_ClientBin 에도 있는 x86 전용 DLL (통계 커널, 64비트 실행에서 지금도 쓰이지 않음)
$knownX86 = @('NMathCoreKernel.dll', 'NMathStatsKernel.dll')

# 1. 자동 로그인 코드
$exeBytes = [IO.File]::ReadAllBytes((Join-Path $StageDir $ExeName))
$exeText = [Text.Encoding]::ASCII.GetString($exeBytes)
"AUTO_LOGIN_CODE=" + $exeText.Contains('mMain_Load')

# 1-1. 프로그램 아이콘: 실행 파일의 Windows 아이콘(탐색기 / 바로 가기에 보이는 것)을 32x32 로 꺼내 ico 파일과 픽셀 비교한다.
#      창 아이콘용 관리 리소스(Resources.MainIcon)와 구분하려고 바이트 검색이 아니라 Windows 가 보여 주는 아이콘을 비교한다.
if ($IconFile) {
    Add-Type -AssemblyName System.Drawing
    $exeIcon = ([System.Drawing.Icon]::ExtractAssociatedIcon((Join-Path $StageDir $ExeName))).ToBitmap()
    $want = (New-Object System.Drawing.Icon($IconFile, 32, 32)).ToBitmap()
    $diff = 0
    for ($x = 0; $x -lt 32; $x++) { for ($y = 0; $y -lt 32; $y++) { if ($exeIcon.GetPixel($x, $y).ToArgb() -ne $want.GetPixel($x, $y).ToArgb()) { $diff++ } } }
    "ICON_MATCH=" + ($diff -eq 0)
}

# 2. x86 전용 .NET DLL / EXE
foreach ($f in Get-ChildItem $StageDir -Recurse -Include *.dll, *.exe -File) {
    try { $arch = [Reflection.AssemblyName]::GetAssemblyName($f.FullName).ProcessorArchitecture } catch { continue }
    if ($arch -ne 'X86') { continue }
    $kind = if ($knownX86 -contains $f.Name) { 'KNOWN' } else { 'NEW' }
    "X86_${kind}=" + $f.FullName.Substring($StageDir.TrimEnd('\').Length + 1)
}

# 3. 참조 검사
$files = Get-ChildItem $StageDir -Recurse -Include *.dll, *.exe -File
$local = @{}
foreach ($f in $files) { $local[[IO.Path]::GetFileNameWithoutExtension($f.Name).ToLower()] = $true }
$fwDirs = @(
    "${env:ProgramFiles(x86)}\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8",
    "${env:ProgramFiles(x86)}\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8\Facades",
    "$env:WINDIR\Microsoft.NET\Framework64\v4.0.30319",
    "$env:WINDIR\Microsoft.NET\Framework64\v4.0.30319\WPF"
)
$missing = @{}
foreach ($f in $files) {
    try { $a = [Reflection.Assembly]::ReflectionOnlyLoadFrom($f.FullName) } catch { continue }
    foreach ($r in $a.GetReferencedAssemblies()) {
        if ($local.ContainsKey($r.Name.ToLower())) { continue }
        $inFw = $false
        foreach ($d in $fwDirs) { if (Test-Path (Join-Path $d "$($r.Name).dll")) { $inFw = $true; break } }
        if ($inFw) { continue }
        if (-not $missing.ContainsKey($r.Name)) { $missing[$r.Name] = New-Object System.Collections.Generic.List[string] }
        $missing[$r.Name].Add($f.Name)
    }
}
foreach ($name in ($missing.Keys | Sort-Object)) {
    $kind = if ($known -contains $name) { 'KNOWN' } else { 'NEW' }
    "REF_${kind}=$name <- " + (($missing[$name] | Sort-Object -Unique) -join ', ')
}
