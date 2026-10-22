# GDS x KLARF x E-Test 연관 분석 화면 레이아웃 시안 이미지 만들기 (화면 A 준비 / 결과 탭, [직접 찾기] 창, 화면 B 역추적)
# 하는 일:
#  1) 운영 폴더(100_ClientBin / 101_ServerBin)는 바꾸지 않고 %TEMP%\chippac\GdsKlarfLayoutPreview 에 Interface / DSL / BSL / RO / Control / ENGUI / GdsMap 을 빌드한다.
#  2) 실제 KLARF Wafer(로컬 DB, 조회만)로 화면을 그린 뒤, 서버 Ready / E-Test 결과 자리는 결과 표 계약(설계 10.2 / 16.4절) 모양의 예시 값으로 채운다.
#  3) 화면 이미지(png)를 -OutDir 에 저장한다. 화면 B 는 OpenGL 도면이라 화면에 잠깐 띄워 캡처한다.
# 설계: 문서/설계/05_연관분석/2026-10-15_GDS_KLARF_ETest_통합분석_데이터_설계.md 8 / 15 / 16절
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File Preview-GdsKlarfLayout.ps1 [-StepSeq 501] [-Factory SCK] [-Device OMM_Hybrid] [-OutDir 경로] [-BuildOnly]
param([long]$StepSeq = 501, [string]$Factory = 'SCK', [string]$Device = 'OMM_Hybrid', [string]$OutDir = '', [switch]$BuildOnly)
$ErrorActionPreference = 'Stop'
$projectDir = Split-Path $PSScriptRoot -Parent
$dmsDir = Split-Path (Split-Path $projectDir -Parent) -Parent
$clientBin = Join-Path $dmsDir '100_ClientBin'
$serverBin = Join-Path $dmsDir '101_ServerBin'
$buildRoot = Join-Path $env:TEMP 'chippac\GdsKlarfLayoutPreview'
$out = Join-Path $buildRoot 'bin'
if (-not $OutDir) { $OutDir = Join-Path $buildRoot 'png' }
$vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
$msbuild = & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
$csc = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'
if (-not $msbuild -or -not (Test-Path $csc)) { throw 'Visual Studio MSBuild / .NET Framework C# compiler 가 필요합니다.' }
if (Test-Path $buildRoot) { Remove-Item $buildRoot -Recurse -Force }
New-Item -ItemType Directory -Force $out | Out-Null
New-Item -ItemType Directory -Force $OutDir | Out-Null

function Build-Temp($proj, $refDir) {
    $name = [IO.Path]::GetFileNameWithoutExtension($proj)
    & $msbuild $proj /t:Build /p:Configuration=Debug /p:Platform=AnyCPU /p:BuildProjectReferences=false `
        "/p:OutputPath=$out\" "/p:IntermediateOutputPath=$buildRoot\obj\$name\" "/p:ReferencePath=$out%3B$refDir" /v:minimal /nologo *> "$buildRoot\build_$name.log"
    if ($LASTEXITCODE -ne 0) { Get-Content "$buildRoot\build_$name.log" | Select-String ': error ' | Select-Object -First 30 | ForEach-Object { $_.Line }; throw "빌드 실패: $name" }
    $warn = @(Select-String -Path "$buildRoot\build_$name.log" -Pattern ': warning ' | Where-Object { $_.Line -notmatch 'MSB3' })
    Write-Output "빌드: $name (경고 $($warn.Count))"
}

# 0) 코어 DLL (71_CoreLib, BuildProjectReferences=false 라 OutDir 에 먼저 둔다)
$coreLib = Join-Path $dmsDir '71_CoreLib'
if (Test-Path $coreLib) { Get-ChildItem $coreLib -Filter '*.dll' | ForEach-Object { Copy-Item $_.FullName $out -Force } }
foreach ($d in 'DACrux.Data.Parser', 'DACrux.Common.DSL', 'DACrux.TEST.DSL', 'DACrux.TEST.BSL', 'DACrux.Base', 'Miracom.Middleware') {
    $p = Join-Path $serverBin "$d.dll"; if (Test-Path $p) { Copy-Item $p $out -Force }
}
foreach ($d in 'DACrux.Common.RO', 'DACrux.Common.Interface', 'DACrux.Utility') { Copy-Item (Join-Path $clientBin "$d.dll") $out -Force }

# 1) 서버 / 원격 호출 / 화면 DLL
Build-Temp "$dmsDir\02_DMS\DACrux.SEMDMS.Interface\DACrux.SEMDMS.Interface.csproj" $serverBin
Build-Temp "$dmsDir\02_DMS\DACrux.SEMDMS.DSL\DACrux.SEMDMS.DSL.csproj" $serverBin
Build-Temp "$dmsDir\02_DMS\DACrux.SEMDMS.BSL\DACrux.SEMDMS.BSL.csproj" $serverBin
Build-Temp "$dmsDir\02_DMS\DACrux.SEMDMS.RO\DACrux.SEMDMS.RO.csproj" $clientBin
foreach ($d in 'DACrux.Data.Handler', 'DACrux.TEST.Control', 'DACrux.TEST.ENGUI', 'DACrux.TEST.Interface', 'DACrux.TEST.RO') {
    Copy-Item (Join-Path $clientBin "$d.dll") $out -Force
}
Build-Temp "$dmsDir\02_DMS\DACrux.SEMDMS.Control\DACrux.SEMDMS.Control.csproj" $clientBin
Build-Temp "$projectDir\DACrux.SEMDMS.ENGUI.csproj" $clientBin

# 2) GdsMap (.NET 4.8). OpenTK 등 GDS 전용 DLL 은 100_ClientBin\GdsMap 에서 가져온다
Get-ChildItem (Join-Path $clientBin 'GdsMap') -Filter '*.dll' | Where-Object { -not (Test-Path (Join-Path $out $_.Name)) } | ForEach-Object { Copy-Item $_.FullName $out }
$gdsProj = "$dmsDir\06_CoreLibSource\GdsMapControl\NexplantQMS.GdsMap.csproj"
& $msbuild $gdsProj /t:Build /p:Configuration=Debug /p:BuildProjectReferences=false "/p:OutputPath=$out\" "/p:IntermediateOutputPath=$buildRoot\obj\GdsMap\" "/p:ReferencePath=$out%3B$clientBin" /v:minimal /nologo *> "$buildRoot\build_GdsMap.log"
if ($LASTEXITCODE -ne 0) { Get-Content "$buildRoot\build_GdsMap.log" | Select-String ': error ' | Select-Object -First 30 | ForEach-Object { $_.Line }; throw '빌드 실패: GdsMap' }
Write-Output "빌드: NexplantQMS.GdsMap (경고 $(@(Select-String -Path "$buildRoot\build_GdsMap.log" -Pattern ': warning ' | Where-Object { $_.Line -notmatch 'MSB3' }).Count))"
if ($BuildOnly) { return }

# 실행에 필요한 나머지 DLL: 이미 있는 것은 덮어쓰지 않는다(방금 빌드한 DLL 보존)
foreach ($dir in $clientBin, $serverBin) {
    Get-ChildItem $dir -Filter '*.dll' | Where-Object { -not (Test-Path (Join-Path $out $_.Name)) } | ForEach-Object { Copy-Item $_.FullName $out }
}

# 3) 시안 실행 파일 + 설정 (Service config 복사, Remoting 서버 등록은 뺀다)
& $csc /nologo /target:exe /platform:anycpu "/out:$out\GdsKlarfLayoutPreview.exe" /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.Data.dll /r:System.Core.dll `
    "/r:$out\DACrux.SEMDMS.ENGUI.dll" "/r:$out\DACrux.Framework.Base.dll" "/r:$out\DACrux.Base.dll" "/r:$out\DACrux.Map.dll" `
    "/r:$out\DACrux.SEMDMS.RO.dll" "/r:$out\DACrux.SEMDMS.Interface.dll" "/r:$out\DACrux.SEMDMS.BSL.dll" "/r:$out\Miracom.Middleware.dll" `
    "/r:$out\NexplantQMS.GdsMap.dll" "/r:$out\OpenTK.dll" "/r:$out\OpenTK.GLControl.dll" "$PSScriptRoot\GdsKlarfLayoutPreview.cs"
if ($LASTEXITCODE -ne 0) { throw '시안 실행 파일 빌드 실패' }
[xml]$x = Get-Content (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config') -Encoding UTF8
$remoting = $x.configuration.'system.runtime.remoting'
if ($remoting) { [void]$x.configuration.RemoveChild($remoting) }
$x.Save("$out\GdsKlarfLayoutPreview.exe.config")

& "$out\GdsKlarfLayoutPreview.exe" $OutDir $StepSeq $Factory $Device
if ($LASTEXITCODE -ne 0) { throw '레이아웃 시안 실행 실패' }
Get-ChildItem $OutDir -Filter '*.png' | ForEach-Object { Write-Output ("이미지: {0} ({1:N0} bytes)" -f $_.FullName, $_.Length) }
