# KLARF 기준 Map Setup 구현 1단계 검사 (KlarfMapSource / KlarfMapRule).
# DB / Service / 화면을 쓰지 않는다. TEMP 폴더에 별도 EXE 를 만들어 실행하고 운영 DLL 은 바꾸지 않는다.
# 결과 마지막 줄: KLARF_MAP_PROBE=PASS / FAIL
$ErrorActionPreference = 'Stop'
$projectDir = Split-Path $PSScriptRoot -Parent
$dmsDir = Split-Path (Split-Path $projectDir -Parent) -Parent
$sampleDir = Join-Path (Split-Path $dmsDir -Parent) 'SampleFile\MIRACOM_KLAF file'
$buildRoot = Join-Path $env:TEMP 'ChippacKlarfMapVerification'
$csc = Join-Path $env:WINDIR 'Microsoft.NET\Framework\v4.0.30319\csc.exe'
if (-not (Test-Path $csc)) { throw '.NET Framework 4 C# 컴파일러가 필요합니다.' }
New-Item -ItemType Directory -Force $buildRoot | Out-Null

# Parser / Base 는 현재 클라이언트 DLL 을 복사해서 쓴다 (실행 위치 옆에 있어야 로드된다).
foreach ($dll in @('DACrux.Base.dll', 'DACrux.Data.Parser.dll')) {
    Copy-Item -LiteralPath (Join-Path "$dmsDir\100_ClientBin" $dll) -Destination (Join-Path $buildRoot $dll) -Force
}

& $csc /nologo /target:exe /platform:anycpu "/out:$buildRoot\KlarfMapRuleTests.exe" /r:System.Drawing.dll /r:System.Core.dll /r:System.Data.dll `
    "/r:$buildRoot\DACrux.Base.dll" "/r:$buildRoot\DACrux.Data.Parser.dll" `
    "$projectDir\Admin\KlarfMapRule.cs" "$projectDir\Admin\KlarfMapSource.cs" "$PSScriptRoot\KlarfMapRuleTests.cs"
if ($LASTEXITCODE -ne 0) { throw '검사 EXE 빌드 실패' }

& "$buildRoot\KlarfMapRuleTests.exe" (Join-Path $sampleDir 'AIBIZ_SCK.KLAF.SINF') (Join-Path $sampleDir 'AIBIZ_SCK.SINF.txt')
if ($LASTEXITCODE -ne 0) { throw 'KLARF Map 검사 실패' }
