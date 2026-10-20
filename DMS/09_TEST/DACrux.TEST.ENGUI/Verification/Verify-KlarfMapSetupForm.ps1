# File Map Setup(KLARF 기준) 화면 검사 (구현 2단계).
# TEST.ENGUI 를 TEMP 에 빌드하고 실제 폼 / Map 컨트롤을 화면 밖에서 만들어 원형 생성 결과를 확인한다.
# 운영 폴더(100_ClientBin) DLL 은 바꾸지 않으며 DB 저장은 호출하지 않는다.
# 결과 마지막 줄: KLARF_FORM_SMOKE=PASS / FAIL, 이미지: $buildRoot\form-preview.png / form-preview.map.png
$ErrorActionPreference = 'Stop'
$projectDir = Split-Path $PSScriptRoot -Parent
$dmsDir = Split-Path (Split-Path $projectDir -Parent) -Parent
$sample = Join-Path (Split-Path $dmsDir -Parent) 'SampleFile\MIRACOM_KLAF file\AIBIZ_SCK.KLAF.SINF'
$buildRoot = Join-Path $env:TEMP 'ChippacKlarfMapFormVerification'
$outputDir = Join-Path $buildRoot 'bin'
$vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
$msbuild = & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
$csc = Join-Path $env:WINDIR 'Microsoft.NET\Framework\v4.0.30319\csc.exe'
if (-not $msbuild -or -not (Test-Path $csc)) { throw 'Visual Studio MSBuild / .NET Framework C# 컴파일러가 필요합니다.' }
New-Item -ItemType Directory -Force $outputDir | Out-Null

# 프로젝트 참조 DLL 은 현재 클라이언트 DLL 을 복사하고 TEST.ENGUI 만 새로 빌드한다.
foreach ($name in 'DACrux.Common.RO', 'DACrux.Data.Handler', 'DACrux.Data.Parser', 'DACrux.SEMDMS.Control', 'DACrux.SEMDMS.Interface',
    'DACrux.SEMDMS.RO', 'DACrux.TEST.Control', 'DACrux.TEST.Interface', 'DACrux.TEST.RO') {
    Copy-Item -LiteralPath (Join-Path "$dmsDir\100_ClientBin" "$name.dll") -Destination $outputDir -Force
}
# DACrux.SEMDMS.Control: 2026-10-14 공통 진행 표시줄(ProgressStatusBar)이 들어 있어 운영 폴더 DLL 대신 새로 빌드한다(참조 DLL 은 없는 것만 운영 폴더에서 복사)
Get-ChildItem "$dmsDir\100_ClientBin" -Filter '*.dll' | Where-Object { -not (Test-Path (Join-Path $outputDir $_.Name)) } | ForEach-Object { Copy-Item $_.FullName $outputDir }
& $msbuild "$dmsDir\02_DMS\DACrux.SEMDMS.Control\DACrux.SEMDMS.Control.csproj" /t:Build /p:Configuration=Debug /p:Platform=AnyCPU /p:BuildProjectReferences=false `
    "/p:OutputPath=$outputDir\" "/p:IntermediateOutputPath=$outputDir\..\obj_control\" /v:minimal /nologo *> "$outputDir\..\build_control.log"
if ($LASTEXITCODE -ne 0) { Get-Content "$outputDir\..\build_control.log" -Tail 30; throw 'DACrux.SEMDMS.Control 빌드 실패' }
& $msbuild "$projectDir\DACrux.TEST.ENGUI.csproj" /t:Build /p:Configuration=Debug /p:Platform=AnyCPU /p:BuildProjectReferences=false `
    "/p:OutputPath=$outputDir\" "/p:IntermediateOutputPath=$buildRoot\obj\" /v:minimal /nologo *> "$buildRoot\build.log"
if ($LASTEXITCODE -ne 0) { Get-Content "$buildRoot\build.log" -Tail 30; throw 'TEST.ENGUI 빌드 실패' }

& $csc /nologo /target:exe /platform:x86 "/out:$outputDir\KlarfMapFormSmoke.exe" /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.Data.dll `
    "/r:$outputDir\DACrux.TEST.ENGUI.dll" "/r:$outputDir\DACrux.Framework.Base.dll" "/r:$outputDir\DACrux.Map.dll" "/r:$outputDir\DACrux.Base.dll" `
    "$PSScriptRoot\KlarfMapFormSmoke.cs"
if ($LASTEXITCODE -ne 0) { throw '화면 검사 EXE 빌드 실패' }

# 파일 읽기 실패 시 화면이 MessageBox 를 띄워 멈출 수 있으므로 120초 제한으로 실행한다.
$process = Start-Process -FilePath "$outputDir\KlarfMapFormSmoke.exe" -ArgumentList @("`"$outputDir\DACrux.TEST.ENGUI.dll`"", "`"$sample`"", "`"$buildRoot\form-preview.png`"") `
    -WorkingDirectory $outputDir -NoNewWindow -PassThru -RedirectStandardOutput "$buildRoot\smoke.log"
# Handle 을 먼저 읽어야 종료 후 ExitCode 가 채워진다 (Start-Process -PassThru 의 알려진 동작).
$null = $process.Handle
if (-not $process.WaitForExit(120000)) { $process.Kill(); throw '화면 검사 시간 초과 (MessageBox 대기 가능성)' }
Get-Content "$buildRoot\smoke.log"
if ($process.ExitCode -ne 0) { throw '화면 검사 실패' }
