# GDS x KLARF 연관 분석 화면(frmGdsKlarfAnalysis) + Die 상세 화면(GdsDieAnalysisForm) Smoke 검사
# 하는 일:
#  1) 운영 폴더(100_ClientBin / 101_ServerBin)는 바꾸지 않고 %TEMP% 에 Interface / DSL / BSL / RO / ENGUI / GdsMap 을 따로 빌드한다.
#  2) Service(Remoting) 대신 RO 원격 연결 필드를 같은 프로세스 BSL 로 바꿔 화면 -> RO -> BSL -> 로컬 DB 를 실제로 거친다(조회만).
#  3) 결과 화면 이미지: %TEMP%\ChippacGdsKlarfAnalysisVerification\analysis.png
# 전제: -Factory / -Device 에 READY GDS Map + READY Chain, -StepSeq 에 KLARF 업로드 Step (기본 SCK / OMM_Hybrid / 528 = XD106.02, 2026-10-16 재업로드 기준)
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File Verify-GdsKlarfAnalysis.ps1 [-StepSeq 528] [-Factory SCK] [-Device OMM_Hybrid]
param([long]$StepSeq = 528, [string]$Factory = 'SCK', [string]$Device = 'OMM_Hybrid')
$ErrorActionPreference = 'Stop'
$projectDir = Split-Path $PSScriptRoot -Parent
$dmsDir = Split-Path (Split-Path $projectDir -Parent) -Parent
$clientBin = Join-Path $dmsDir '100_ClientBin'
$serverBin = Join-Path $dmsDir '101_ServerBin'
$buildRoot = Join-Path $env:TEMP 'ChippacGdsKlarfAnalysisVerification'
$out = Join-Path $buildRoot 'bin'
$vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
$msbuild = & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
$csc = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'
if (-not $msbuild -or -not (Test-Path $csc)) { throw 'Visual Studio MSBuild / .NET Framework C# compiler 가 필요합니다.' }
if (Test-Path $buildRoot) { Remove-Item $buildRoot -Recurse -Force }
New-Item -ItemType Directory -Force $out | Out-Null

function Build-Temp($proj, $refDir) {
    $name = [IO.Path]::GetFileNameWithoutExtension($proj)
    & $msbuild $proj /t:Build /p:Configuration=Debug /p:Platform=AnyCPU /p:BuildProjectReferences=false `
        "/p:OutputPath=$out\" "/p:IntermediateOutputPath=$buildRoot\obj\$name\" "/p:ReferencePath=$out%3B$refDir" /v:minimal /nologo *> "$buildRoot\build_$name.log"
    if ($LASTEXITCODE -ne 0) { Get-Content "$buildRoot\build_$name.log" -Tail 20; throw "빌드 실패: $name" }
    $warn = @(Select-String -Path "$buildRoot\build_$name.log" -Pattern ': warning ' | Where-Object { $_.Line -notmatch 'MSB3' })
    Write-Output "빌드: $name (경고 $($warn.Count))"
}

# 0) 코어 DLL (이후 71_CoreLib, BuildProjectReferences=false 라 OutDir 에 먼저 둔다)
$coreLib = Join-Path $dmsDir '71_CoreLib'
if (Test-Path $coreLib) { Get-ChildItem $coreLib -Filter '*.dll' | ForEach-Object { Copy-Item $_.FullName $out -Force } }
# DACrux.TEST.BSL: 2026-10-14 SEMDMS.BSL 이 YMS 적재 때문에 참조한다(BuildProjectReferences=false 빌드에 DLL 이 필요)
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
# DACrux.SEMDMS.Control: 2026-10-14 공통 진행 표시줄(ProgressStatusBar)이 들어 있어 운영 폴더 DLL 대신 새로 빌드한다
Build-Temp "$dmsDir\02_DMS\DACrux.SEMDMS.Control\DACrux.SEMDMS.Control.csproj" $clientBin
Build-Temp "$projectDir\DACrux.SEMDMS.ENGUI.csproj" $clientBin

# 2) GdsMap (.NET 4.8). OpenTK 등 GDS 전용 DLL 은 100_ClientBin\GdsMap 에서 가져온다(build.md: OutDir 를 바꾸면 OpenTK 를 먼저 복사)
Get-ChildItem (Join-Path $clientBin 'GdsMap') -Filter '*.dll' | Where-Object { -not (Test-Path (Join-Path $out $_.Name)) } | ForEach-Object { Copy-Item $_.FullName $out }
$gdsProj = "$dmsDir\06_CoreLibSource\GdsMapControl\NexplantQMS.GdsMap.csproj"
& $msbuild $gdsProj /t:Build /p:Configuration=Debug /p:BuildProjectReferences=false "/p:OutputPath=$out\" "/p:IntermediateOutputPath=$buildRoot\obj\GdsMap\" "/p:ReferencePath=$out%3B$clientBin" /v:minimal /nologo *> "$buildRoot\build_GdsMap.log"
if ($LASTEXITCODE -ne 0) { Get-Content "$buildRoot\build_GdsMap.log" -Tail 20; throw '빌드 실패: GdsMap' }
Write-Output "빌드: NexplantQMS.GdsMap (경고 $(@(Select-String -Path "$buildRoot\build_GdsMap.log" -Pattern ': warning ' | Where-Object { $_.Line -notmatch 'MSB3' }).Count))"

# 실행에 필요한 나머지 DLL: 이미 있는 것은 덮어쓰지 않는다(방금 빌드한 DLL 보존)
foreach ($dir in $clientBin, $serverBin) {
    Get-ChildItem $dir -Filter '*.dll' | Where-Object { -not (Test-Path (Join-Path $out $_.Name)) } | ForEach-Object { Copy-Item $_.FullName $out }
}

# 3) Smoke 실행 파일 + 설정 (Service config 복사, Remoting 서버 등록은 뺀다)
& $csc /nologo /target:exe /platform:anycpu "/out:$out\GdsKlarfAnalysisSmoke.exe" /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.Data.dll /r:System.Core.dll `
    "/r:$out\DACrux.SEMDMS.ENGUI.dll" "/r:$out\DACrux.Framework.Base.dll" "/r:$out\DACrux.Base.dll" "/r:$out\DACrux.Map.dll" `
    "/r:$out\DACrux.SEMDMS.RO.dll" "/r:$out\DACrux.SEMDMS.Interface.dll" "/r:$out\DACrux.SEMDMS.BSL.dll" "/r:$out\Miracom.Middleware.dll" `
    "/r:$out\NexplantQMS.GdsMap.exe" "/r:$out\OpenTK.dll" "/r:$out\OpenTK.GLControl.dll" "$PSScriptRoot\GdsKlarfAnalysisFormSmoke.cs"
if ($LASTEXITCODE -ne 0) { throw 'Smoke 빌드 실패' }
[xml]$x = Get-Content (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config') -Encoding UTF8
$remoting = $x.configuration.'system.runtime.remoting'
if ($remoting) { [void]$x.configuration.RemoveChild($remoting) }
$x.Save("$out\GdsKlarfAnalysisSmoke.exe.config")

& "$out\GdsKlarfAnalysisSmoke.exe" "$buildRoot\analysis.png" $StepSeq $Factory $Device
if ($LASTEXITCODE -ne 0) { throw 'GDS-KLARF 연관 분석 화면 Smoke 실패' }
foreach ($n in 'DACrux.SEMDMS.Interface.dll', 'DACrux.SEMDMS.DSL.dll', 'DACrux.SEMDMS.BSL.dll', 'DACrux.SEMDMS.RO.dll', 'DACrux.SEMDMS.ENGUI.dll', 'NexplantQMS.GdsMap.exe') {
    Write-Output ("{0} md5={1}" -f $n, (Get-FileHash (Join-Path $out $n) -Algorithm MD5).Hash.Substring(0, 8).ToLower())
}
