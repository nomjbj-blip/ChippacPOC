# 연관 분석 1단계 확인: 실제 모양 Gap / 모양 판정 엔진 / 서버 Cache (설계: 문서/설계/05_연관분석/2026-10-15_GDS_KLARF_ETest_통합분석_데이터_설계.md 4.2 / 4.3 / 4.5절)
# 하는 일:
#  1) 운영 폴더(100_ClientBin / 101_ServerBin)는 바꾸지 않고 %TEMP%\ChippacGdsKlarfShapeProbe 에 Interface / DSL / BSL 을 따로 빌드한다.
#  2) GdsKlarfShapeProbe.cs 를 그 BSL 에 붙여 실행 파일로 만들고 실행한다(손 계산 비교 / 전수 비교 / Cache / 실제 DB 조회).
# DB 는 조회만 한다. 전제: -Factory / -Device 에 READY GDS Map + READY Chain, -StepSeq 에 KLARF 업로드 Step.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsKlarfShapeProbe.ps1 [-Factory SCK] [-Device OMM_Hybrid] [-StepSeq 528]
param([string]$Factory = 'SCK', [string]$Device = 'OMM_Hybrid', [long]$StepSeq = 528)
$ErrorActionPreference = 'Stop'
$dmsDir = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$serverBin = Join-Path $dmsDir '101_ServerBin'
$buildRoot = Join-Path $env:TEMP 'ChippacGdsKlarfShapeProbe'
$out = Join-Path $buildRoot 'bin'
$vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
$msbuild = & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
$csc = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'
if (-not $msbuild -or -not (Test-Path $csc)) { throw 'Visual Studio MSBuild / .NET Framework C# compiler 가 필요합니다.' }
if (Test-Path $buildRoot) { Remove-Item $buildRoot -Recurse -Force }
New-Item -ItemType Directory -Force $out | Out-Null

function Build-Temp($proj) {
    $name = [IO.Path]::GetFileNameWithoutExtension($proj)
    & $msbuild $proj /t:Build /p:Configuration=Debug /p:Platform=AnyCPU /p:BuildProjectReferences=false `
        "/p:OutputPath=$out\" "/p:IntermediateOutputPath=$buildRoot\obj\$name\" "/p:ReferencePath=$out%3B$serverBin" /v:minimal /nologo *> "$buildRoot\build_$name.log"
    if ($LASTEXITCODE -ne 0) { Get-Content "$buildRoot\build_$name.log" -Tail 20; throw "빌드 실패: $name" }
    $warn = @(Select-String -Path "$buildRoot\build_$name.log" -Pattern ': warning ' | Where-Object { $_.Line -notmatch 'MSB3' })
    Write-Output "빌드: $name (경고 $($warn.Count))"
}

# 코어 DLL 을 먼저 둔다(BuildProjectReferences=false)
$coreLib = Join-Path $dmsDir '71_CoreLib'
if (Test-Path $coreLib) { Get-ChildItem $coreLib -Filter '*.dll' | ForEach-Object { Copy-Item $_.FullName $out -Force } }
foreach ($d in 'DACrux.Data.Parser', 'DACrux.Common.DSL', 'DACrux.TEST.DSL', 'DACrux.TEST.BSL', 'DACrux.Base', 'Miracom.Middleware') {
    $p = Join-Path $serverBin "$d.dll"; if (Test-Path $p) { Copy-Item $p $out -Force }
}
Build-Temp "$dmsDir\02_DMS\DACrux.SEMDMS.Interface\DACrux.SEMDMS.Interface.csproj"
Build-Temp "$dmsDir\02_DMS\DACrux.SEMDMS.DSL\DACrux.SEMDMS.DSL.csproj"
Build-Temp "$dmsDir\02_DMS\DACrux.SEMDMS.BSL\DACrux.SEMDMS.BSL.csproj"
Get-ChildItem $serverBin -Filter '*.dll' | Where-Object { -not (Test-Path (Join-Path $out $_.Name)) } | ForEach-Object { Copy-Item $_.FullName $out }

& $csc /nologo /target:exe /platform:anycpu "/out:$out\GdsKlarfShapeProbe.exe" /r:System.Data.dll /r:System.Core.dll /r:System.Xml.dll `
    "/r:$out\DACrux.SEMDMS.BSL.dll" "/r:$out\DACrux.SEMDMS.DSL.dll" "/r:$out\DACrux.SEMDMS.Interface.dll" "/r:$out\Miracom.Middleware.dll" `
    "/r:$out\DACrux.Base.dll" "$PSScriptRoot\GdsKlarfShapeProbe.cs"
if ($LASTEXITCODE -ne 0) { throw 'Probe 빌드 실패' }
[xml]$x = Get-Content (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config') -Encoding UTF8
$remoting = $x.configuration.'system.runtime.remoting'
if ($remoting) { [void]$x.configuration.RemoveChild($remoting) }
$x.Save("$out\GdsKlarfShapeProbe.exe.config")

& "$out\GdsKlarfShapeProbe.exe" $Factory $Device $StepSeq
$code = $LASTEXITCODE
Write-Output ("DACrux.SEMDMS.BSL.dll md5={0}" -f (Get-FileHash (Join-Path $out 'DACrux.SEMDMS.BSL.dll') -Algorithm MD5).Hash.Substring(0, 8).ToLower())
if ($code -ne 0) { throw '모양 판정 Probe 실패' }
