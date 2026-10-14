# Wafer Map 조회 화면(frmStandardMapView) 검사.
# TEST.ENGUI 를 TEMP 에 빌드하고 File Map Setup 화면 / 조회 화면 / 기존 Map Define 화면을 화면 밖에서 만들어 확인한다.
# 운영 폴더(100_ClientBin) DLL 은 바꾸지 않으며 DB / 원격 서비스는 호출하지 않는다.
# 결과 마지막 줄: STANDARD_MAP_VIEW_SMOKE=PASS / FAIL, 이미지: $buildRoot\view-preview.png / view-preview.map.png
$ErrorActionPreference = 'Stop'
$projectDir = Split-Path $PSScriptRoot -Parent
$dmsDir = Split-Path (Split-Path $projectDir -Parent) -Parent
$sample = Join-Path (Split-Path $dmsDir -Parent) 'SampleFile\MIRACOM_KLAF file\AIBIZ_SCK.KLAF.SINF'
$buildRoot = Join-Path $env:TEMP 'chippac\StandardMapViewVerification'
$outputDir = Join-Path $buildRoot 'bin'
$vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
$msbuild = & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
$csc = Join-Path $env:WINDIR 'Microsoft.NET\Framework\v4.0.30319\csc.exe'
if (-not $msbuild -or -not (Test-Path $csc)) { throw 'Visual Studio MSBuild / .NET Framework C# 컴파일러가 필요합니다.' }
New-Item -ItemType Directory -Force $outputDir | Out-Null

# 참조 DLL 은 현재 클라이언트 DLL 을 복사하고 TEST.ENGUI 만 새로 빌드한다.
# OutputPath 를 바꾸면 프로젝트 참조도 같은 폴더에서 찾으므로 Core DLL 까지 복사한다 (빈 폴더에서 CS0006 방지).
foreach ($name in 'DACrux.Base', 'DACrux.Framework.Base', 'DACrux.Framework.Controls', 'DACrux.Framework.Interface', 'DACrux.Framework.RO',
    'DACrux.Map', 'DACrux.SP.Common', 'DACrux.Utility', 'DACrux.Common.RO', 'DACrux.Data.Handler', 'DACrux.Data.Parser',
    'DACrux.SEMDMS.Control', 'DACrux.SEMDMS.Interface', 'DACrux.SEMDMS.RO', 'DACrux.TEST.Control', 'DACrux.TEST.Interface', 'DACrux.TEST.RO') {
    Copy-Item -LiteralPath (Join-Path "$dmsDir\100_ClientBin" "$name.dll") -Destination $outputDir -Force
}
& $msbuild "$projectDir\DACrux.TEST.ENGUI.csproj" /t:Build /p:Configuration=Debug /p:Platform=AnyCPU /p:BuildProjectReferences=false `
    "/p:OutputPath=$outputDir\" "/p:IntermediateOutputPath=$buildRoot\obj\" /v:minimal /nologo *> "$buildRoot\build.log"
if ($LASTEXITCODE -ne 0) { Get-Content "$buildRoot\build.log" -Tail 30; throw 'TEST.ENGUI 빌드 실패' }
$warnings = @(Select-String -Path "$buildRoot\build.log" -Pattern 'warning CS' | ForEach-Object { $_.Line } | Sort-Object -Unique)
"TEST.ENGUI 빌드 성공 / 경고 $($warnings.Count)"
$warnings | Where-Object { $_ -match 'StandardMapView|StoredMapLayout' } | ForEach-Object { "새 파일 경고: $_" }

& $csc /nologo /target:exe /platform:x86 "/out:$outputDir\StandardMapViewSmoke.exe" /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.Data.dll `
    "/r:$outputDir\DACrux.TEST.ENGUI.dll" "/r:$outputDir\DACrux.Framework.Base.dll" "/r:$outputDir\DACrux.Map.dll" "/r:$outputDir\DACrux.Base.dll" `
    "$PSScriptRoot\StandardMapViewSmoke.cs"
if ($LASTEXITCODE -ne 0) { throw '화면 검사 EXE 빌드 실패' }

# 화면이 MessageBox 를 띄우면 멈출 수 있으므로 180초 제한으로 실행한다.
$process = Start-Process -FilePath "$outputDir\StandardMapViewSmoke.exe" -ArgumentList @("`"$outputDir\DACrux.TEST.ENGUI.dll`"", "`"$sample`"", "`"$buildRoot\view-preview.png`"") `
    -WorkingDirectory $outputDir -NoNewWindow -PassThru -RedirectStandardOutput "$buildRoot\smoke.log"
# Handle 을 먼저 읽어야 종료 후 ExitCode 가 채워진다 (Start-Process -PassThru 의 알려진 동작).
$null = $process.Handle
if (-not $process.WaitForExit(180000)) { $process.Kill(); throw '화면 검사 시간 초과 (MessageBox 대기 가능성)' }
Get-Content "$buildRoot\smoke.log"
if ($process.ExitCode -ne 0) { throw '화면 검사 실패' }
