# 새 파일 Map Setup의 빌드/Parsing/실제 컨트롤 검사를 실행한다.
# 운영 DLL은 바꾸지 않으며 DB 저장과 공통 화면 접속 이력 호출도 실행하지 않는다.
$ErrorActionPreference = 'Stop'
$projectDir = Split-Path $PSScriptRoot -Parent
$dmsDir = Split-Path (Split-Path $projectDir -Parent) -Parent
$buildRoot = Join-Path $env:TEMP 'ChippacMapSetupVerification'
$outputDir = Join-Path $buildRoot 'bin'
$intermediateDir = Join-Path $buildRoot 'obj'
$vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
$msbuild = & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
$csc = Join-Path $env:WINDIR 'Microsoft.NET\Framework\v4.0.30319\csc.exe'
if (-not $msbuild -or -not (Test-Path $csc)) { throw 'Visual Studio MSBuild / .NET Framework C# compiler가 필요합니다.' }
New-Item -ItemType Directory -Force $outputDir, $intermediateDir | Out-Null

# 프로젝트 참조는 현재 클라이언트 DLL로 확인하고 새 ENG​​UI만 별도 위치에 빌드한다.
$dependencies = @('DACrux.Common.RO.dll', 'DACrux.Data.Handler.dll', 'DACrux.Data.Parser.dll',
    'DACrux.SEMDMS.Control.dll', 'DACrux.SEMDMS.RO.dll', 'DACrux.TEST.Control.dll',
    'DACrux.TEST.Interface.dll', 'DACrux.TEST.RO.dll')
foreach ($dependency in $dependencies) {
    Copy-Item -LiteralPath (Join-Path "$dmsDir\100_ClientBin" $dependency) -Destination (Join-Path $outputDir $dependency)
}
# DACrux.SEMDMS.Control: 2026-10-14 공통 진행 표시줄(ProgressStatusBar)이 들어 있어 운영 폴더 DLL 대신 새로 빌드한다(참조 DLL 은 없는 것만 운영 폴더에서 복사)
Get-ChildItem "$dmsDir\100_ClientBin" -Filter '*.dll' | Where-Object { -not (Test-Path (Join-Path $outputDir $_.Name)) } | ForEach-Object { Copy-Item $_.FullName $outputDir }
& $msbuild "$dmsDir\02_DMS\DACrux.SEMDMS.Control\DACrux.SEMDMS.Control.csproj" /t:Build /p:Configuration=Debug /p:Platform=AnyCPU /p:BuildProjectReferences=false `
    "/p:OutputPath=$outputDir\" "/p:IntermediateOutputPath=$outputDir\..\obj_control\" /v:minimal /nologo *> "$outputDir\..\build_control.log"
if ($LASTEXITCODE -ne 0) { Get-Content "$outputDir\..\build_control.log" -Tail 30; throw 'DACrux.SEMDMS.Control 빌드 실패' }
& $msbuild "$projectDir\DACrux.TEST.ENGUI.csproj" /t:Build /p:Configuration=Debug /p:Platform=AnyCPU /p:BuildProjectReferences=false "/p:OutputPath=$outputDir\" "/p:IntermediateOutputPath=$intermediateDir\" /v:minimal /nologo *> "$buildRoot\build.log"
if ($LASTEXITCODE -ne 0) { Get-Content "$buildRoot\build.log" -Tail 30; throw '프로젝트 빌드 실패' }

# 파일 Parser는 .NET Framework 4 컴파일러로 독립 실행해 좌표/형식 오류를 검사한다.
& $csc /nologo /target:exe "/out:$buildRoot\ParserTests.exe" /r:System.Drawing.dll /r:System.Core.dll "$projectDir\Admin\StandardMapFile.cs" "$PSScriptRoot\StandardMapFileTests.cs"
if ($LASTEXITCODE -ne 0) { throw 'Parser 검사 빌드 실패' }
& "$buildRoot\ParserTests.exe" "$PSScriptRoot\Hybrid.map"
if ($LASTEXITCODE -ne 0) { throw 'Parser 검사 실패' }

# 실제 m_wMap을 사용한 형상/저장 데이터/Designer 이벤트/렌더링을 확인한다.
& $csc /nologo /target:exe /platform:x86 "/out:$outputDir\FormSmoke.exe" /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.Data.dll "/r:$outputDir\DACrux.TEST.ENGUI.dll" "/r:$outputDir\DACrux.Framework.Base.dll" "/r:$outputDir\DACrux.Map.dll" "/r:$outputDir\DACrux.Base.dll" "$PSScriptRoot\StandardMapFormSmoke.cs"
if ($LASTEXITCODE -ne 0) { throw '화면 검사 빌드 실패' }
& "$outputDir\FormSmoke.exe" "$outputDir\DACrux.TEST.ENGUI.dll" "$PSScriptRoot\Hybrid.map" "$buildRoot\form-preview.png"
if ($LASTEXITCODE -ne 0) { throw '화면 검사 실패' }
Write-Output "빌드/검사 완료: $buildRoot"
