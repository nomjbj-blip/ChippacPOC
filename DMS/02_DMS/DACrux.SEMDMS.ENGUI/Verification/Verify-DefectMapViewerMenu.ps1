# Defect Map Viewer 컨트롤(DPUCDefecMapViewer) 우클릭 메뉴 [Center Cross Show / Hide] 검사 (2026-10-13).
# SEMDMS.Control 을 TEMP 에 빌드하고(운영 100_ClientBin 은 바꾸지 않음) 컨트롤을 화면에 띄우지 않고 메뉴만 확인한다. DB 조회 없음.
# 결과 마지막 줄: DEFECT_MAP_VIEWER_MENU_SMOKE=PASS / FAIL
$ErrorActionPreference = 'Stop'
$projectDir = Split-Path $PSScriptRoot -Parent
$dmsDir = Split-Path (Split-Path $projectDir -Parent) -Parent
$clientBin = Join-Path $dmsDir '100_ClientBin'
$controlProject = Join-Path $dmsDir '02_DMS\DACrux.SEMDMS.Control\DACrux.SEMDMS.Control.csproj'
$buildRoot = Join-Path $env:TEMP 'chippac\DefectMapViewerMenuVerification'
$out = Join-Path $buildRoot 'bin'
$vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
$msbuild = & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
$csc = Join-Path $env:WINDIR 'Microsoft.NET\Framework\v4.0.30319\csc.exe'
if (-not $msbuild -or -not (Test-Path $csc)) { throw 'Visual Studio MSBuild / .NET Framework C# 컴파일러가 필요합니다.' }
New-Item -ItemType Directory -Force $out | Out-Null

# 참조 DLL 은 현재 클라이언트 DLL 을 그대로 복사하고 SEMDMS.Control 만 새로 빌드한다 (Debug 구성 = DEBUG 상수 포함).
Get-ChildItem -LiteralPath $clientBin -Filter '*.dll' | Where-Object { $_.Name -ne 'DACrux.SEMDMS.Control.dll' } |
    ForEach-Object { Copy-Item -LiteralPath $_.FullName -Destination $out -Force }
& $msbuild $controlProject /t:Build /p:Configuration=Debug /p:Platform=AnyCPU /p:BuildProjectReferences=false `
    "/p:OutputPath=$out\" "/p:IntermediateOutputPath=$buildRoot\obj\" "/p:ReferencePath=$clientBin" /v:minimal /nologo *> "$buildRoot\build.log"
if ($LASTEXITCODE -ne 0) { Get-Content "$buildRoot\build.log" -Tail 30; throw 'SEMDMS.Control 빌드 실패' }
$warnings = @(Select-String -Path "$buildRoot\build.log" -Pattern 'warning CS' | ForEach-Object { $_.Line } | Sort-Object -Unique)
"SEMDMS.Control 빌드 성공 / 경고 $($warnings.Count)"
$warnings | Where-Object { $_ -match 'DPUCDefecMapViewer' } | ForEach-Object { "DPUCDefecMapViewer 경고: $_" }

& $csc /nologo /target:exe /platform:x86 "/out:$out\DefectMapViewerMenuSmoke.exe" /r:System.Windows.Forms.dll /r:System.Drawing.dll `
    "$PSScriptRoot\DefectMapViewerMenuSmoke.cs"
if ($LASTEXITCODE -ne 0) { throw '검사 EXE 빌드 실패' }

$process = Start-Process -FilePath "$out\DefectMapViewerMenuSmoke.exe" -ArgumentList @("`"$out\DACrux.SEMDMS.Control.dll`"") `
    -WorkingDirectory $out -NoNewWindow -PassThru -RedirectStandardOutput "$buildRoot\smoke.log"
$null = $process.Handle
if (-not $process.WaitForExit(120000)) { $process.Kill(); throw '검사 시간 초과 (MessageBox 대기 가능성)' }
Get-Content "$buildRoot\smoke.log"
if ($process.ExitCode -ne 0) { throw '검사 실패' }
