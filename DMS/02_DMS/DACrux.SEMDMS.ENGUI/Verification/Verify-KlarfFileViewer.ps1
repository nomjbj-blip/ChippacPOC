# KLARF File View 화면의 빌드 / 파일 읽기 / 실제 컨트롤 그리기 / 마우스 정보를 검사한다.
# 운영 DLL(100_ClientBin)은 바꾸지 않고 %TEMP% 에 따로 빌드한다. DB 저장과 공통 화면 접속 이력 호출은 실행하지 않는다.
# XD074_synthetic.000 은 화면 확인용으로 만든 합성 KLARF 이다 (300mm / 검사 Die 26 / Defect 109 / FineBin 1~4 = 43/2/45/19).
$ErrorActionPreference = 'Stop'
$projectDir = Split-Path $PSScriptRoot -Parent
$dmsDir = Split-Path (Split-Path $projectDir -Parent) -Parent
$buildRoot = Join-Path $env:TEMP 'ChippacKlarfViewerVerification'
$outputDir = Join-Path $buildRoot 'bin'
$intermediateDir = Join-Path $buildRoot 'obj'
$vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
$msbuild = & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
$csc = Join-Path $env:WINDIR 'Microsoft.NET\Framework\v4.0.30319\csc.exe'
if (-not $msbuild -or -not (Test-Path $csc)) { throw 'Visual Studio MSBuild / .NET Framework C# compiler가 필요합니다.' }
New-Item -ItemType Directory -Force $outputDir, $intermediateDir | Out-Null

# 프로젝트 참조는 현재 클라이언트 DLL로 확인하고 ENGUI만 별도 위치에 빌드한다.
$dependencies = @('DACrux.Common.RO.dll', 'DACrux.Data.Handler.dll', 'DACrux.Data.Parser.dll',
    'DACrux.SEMDMS.Control.dll', 'DACrux.SEMDMS.RO.dll', 'DACrux.TEST.Control.dll',
    'DACrux.TEST.Interface.dll', 'DACrux.TEST.RO.dll')
foreach ($dependency in $dependencies) {
    Copy-Item -LiteralPath (Join-Path "$dmsDir\100_ClientBin" $dependency) -Destination (Join-Path $outputDir $dependency) -Force
}
& $msbuild "$projectDir\DACrux.SEMDMS.ENGUI.csproj" /t:Build /p:Configuration=Debug /p:Platform=AnyCPU /p:BuildProjectReferences=false "/p:OutputPath=$outputDir\" "/p:IntermediateOutputPath=$intermediateDir\" /v:minimal /nologo *> "$buildRoot\build.log"
if ($LASTEXITCODE -ne 0) { Get-Content "$buildRoot\build.log" -Tail 30; throw '프로젝트 빌드 실패' }

# 실제 폼을 화면 밖에서 띄워 합성 샘플과 실제 백업 KLARF 를 읽고 결과 이미지를 남긴다.
& $csc /nologo /target:exe /platform:x86 "/out:$outputDir\KlarfSmoke.exe" /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.Data.dll "/r:$outputDir\DACrux.SEMDMS.ENGUI.dll" "/r:$outputDir\DACrux.Framework.Base.dll" "/r:$outputDir\DACrux.Map.dll" "/r:$outputDir\DACrux.Base.dll" "$PSScriptRoot\KlarfViewerFormSmoke.cs"
if ($LASTEXITCODE -ne 0) { throw '화면 검사 빌드 실패' }
& "$outputDir\KlarfSmoke.exe" "$outputDir\DACrux.SEMDMS.ENGUI.dll" "$PSScriptRoot\XD074_synthetic.000" "$buildRoot\synthetic.png" 109 4
if ($LASTEXITCODE -ne 0) { throw '합성 샘플 화면 검사 실패' }
$realKlarf = Join-Path $dmsDir 'INSP_BACKUP\2018\02\04\181C45\181C45.000'
if (Test-Path $realKlarf) {
    & "$outputDir\KlarfSmoke.exe" "$outputDir\DACrux.SEMDMS.ENGUI.dll" $realKlarf "$buildRoot\real.png"
    if ($LASTEXITCODE -ne 0) { throw '실제 KLARF 화면 검사 실패' }
}
Write-Output "빌드/검사 완료: $buildRoot"
