# E-Test 결과 업로드 화면(frmEtResultUpload) Smoke 검사 (2026-10-07)
# Smoke = 화면을 화면 밖에서 실제로 띄워 [분석] -> [DB 저장] -> 재저장 -> 삭제 흐름이 깨지지 않는지 확인하는 자동 검사. Verify-KlarfUpload.ps1 과 같은 구조.
# 하는 일:
#  1) 운영 폴더(100_ClientBin / 101_ServerBin)는 바꾸지 않고 %TEMP% 에 Interface / DSL / BSL / RO / ENGUI 를 따로 빌드한다.
#  2) Service(Remoting) 대신 BSL 을 같은 프로세스에 붙여 화면 -> RO -> BSL -> 로컬 DB 를 실제로 거친다.
#  3) 샘플: SampleFile\26_10_06 Sample File\CAI_OMM_ATV_A0_TSVET_20260609161940_RH-HS2615_RH-HS2615-02.xlsx (Excel 에 열려 있어도 됨)
#  4) 결과 화면 이미지: %TEMP%\ChippacEtResultUploadVerification\upload.png
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File Verify-EtResultUpload.ps1 [-Keep]
#   -Keep: 마지막 저장 파일(PROBE / ET_SMOKE)을 지우지 않는다.
param([switch]$Keep)
$ErrorActionPreference = 'Stop'
$projectDir = Split-Path $PSScriptRoot -Parent
$dmsDir = Split-Path (Split-Path $projectDir -Parent) -Parent
$rootDir = Split-Path $dmsDir -Parent
$clientBin = Join-Path $dmsDir '100_ClientBin'
$serverBin = Join-Path $dmsDir '101_ServerBin'
$buildRoot = Join-Path $env:TEMP 'ChippacEtResultUploadVerification'
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
    $warn | ForEach-Object { Write-Output ('  ' + ($_.Line -replace '^.*\\02_DMS\\', '')) }
}

# 0) 코어 DLL: 2026-10-08 빌드 경로 정리 이후 06_CoreLibSource 산출물은 71_CoreLib 에 있고, 프로젝트 참조라
#    BuildProjectReferences=false TEMP 빌드에서는 OutDir 에 먼저 있어야 한다(빌드_검증_가이드.md 1절).
$coreLib = Join-Path $dmsDir '71_CoreLib'
if (Test-Path $coreLib) { Get-ChildItem $coreLib -Filter '*.dll' | ForEach-Object { Copy-Item $_.FullName $out -Force } }

# 1) 서버 / 원격 호출 DLL (iGdsEtResult / GdsEtResult) -> ENGUI
foreach ($d in 'DACrux.Data.Parser', 'DACrux.Common.DSL', 'DACrux.TEST.DSL', 'DACrux.Base', 'Miracom.Middleware') {
    $p = Join-Path $serverBin "$d.dll"; if (Test-Path $p) { Copy-Item $p $out -Force }
}
# DACrux.Utility: 2026-10-08 RO csproj 에 프로젝트 참조가 생겨(빌드 경로 정리 작업) BuildProjectReferences=false 빌드에 DLL 이 필요하다
foreach ($d in 'DACrux.Common.RO', 'DACrux.Common.Interface', 'DACrux.Utility') { Copy-Item (Join-Path $clientBin "$d.dll") $out -Force }
Build-Temp "$dmsDir\02_DMS\DACrux.SEMDMS.Interface\DACrux.SEMDMS.Interface.csproj" $serverBin
Build-Temp "$dmsDir\02_DMS\DACrux.SEMDMS.DSL\DACrux.SEMDMS.DSL.csproj" $serverBin
Build-Temp "$dmsDir\02_DMS\DACrux.SEMDMS.BSL\DACrux.SEMDMS.BSL.csproj" $serverBin
Build-Temp "$dmsDir\02_DMS\DACrux.SEMDMS.RO\DACrux.SEMDMS.RO.csproj" $clientBin
foreach ($d in 'DACrux.Data.Handler', 'DACrux.SEMDMS.Control', 'DACrux.TEST.Control', 'DACrux.TEST.ENGUI', 'DACrux.TEST.Interface', 'DACrux.TEST.RO') {
    Copy-Item (Join-Path $clientBin "$d.dll") $out -Force
}
Build-Temp "$projectDir\DACrux.SEMDMS.ENGUI.csproj" $clientBin

# 실행에 필요한 나머지 DLL: 이미 있는 것은 덮어쓰지 않는다(방금 빌드한 Interface / BSL / RO / ENGUI 보존)
foreach ($dir in $clientBin, $serverBin) {
    Get-ChildItem $dir -Filter '*.dll' | Where-Object { -not (Test-Path (Join-Path $out $_.Name)) } | ForEach-Object { Copy-Item $_.FullName $out }
}

# 2) Smoke 실행 파일 + 설정 (Service config 복사, Remoting 서버 등록은 뺀다)
& $csc /nologo /target:exe /platform:anycpu "/out:$out\EtResultUploadSmoke.exe" /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.Data.dll `
    "/r:$out\DACrux.SEMDMS.ENGUI.dll" "/r:$out\DACrux.Framework.Base.dll" "/r:$out\DACrux.Base.dll" `
    "/r:$out\DACrux.SEMDMS.RO.dll" "/r:$out\DACrux.SEMDMS.Interface.dll" "/r:$out\DACrux.SEMDMS.BSL.dll" "/r:$out\Miracom.Middleware.dll" "$PSScriptRoot\EtResultUploadFormSmoke.cs"
if ($LASTEXITCODE -ne 0) { throw 'Smoke 빌드 실패' }
[xml]$x = Get-Content (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config') -Encoding UTF8
$remoting = $x.configuration.'system.runtime.remoting'
if ($remoting) { [void]$x.configuration.RemoveChild($remoting) }
$x.Save("$out\EtResultUploadSmoke.exe.config")

$sample = Join-Path $rootDir 'SampleFile\26_10_06 Sample File\CAI_OMM_ATV_A0_TSVET_20260609161940_RH-HS2615_RH-HS2615-02.xlsx'
$keepArg = if ($Keep) { 'keep' } else { '' }
& "$out\EtResultUploadSmoke.exe" $sample "$buildRoot\upload.png" "$buildRoot\work" $keepArg
if ($LASTEXITCODE -ne 0) { throw 'E-Test 업로드 화면 Smoke 실패' }

# 3) Spec 설정 화면 Smoke (2026-10-07 추가, 2026-10-12 Parameter 단위 + Spec 표 붙여넣기 + Chain 묶기로 다시 작성, FACTORY=PROBE / DEVICE_ID=ET_LIMIT_SMOKE / Slot 4)
#    고객 Spec 표: Verification\EtSpecTable_U9_20261012.txt (2026-10-12 고객 Spec 표, 119행)
& $csc /nologo /target:exe /platform:anycpu "/out:$out\EtLimitSetupSmoke.exe" /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.Data.dll `
    "/r:$out\DACrux.SEMDMS.ENGUI.dll" "/r:$out\DACrux.Framework.Base.dll" "/r:$out\DACrux.Base.dll" `
    "/r:$out\DACrux.SEMDMS.RO.dll" "/r:$out\DACrux.SEMDMS.Interface.dll" "/r:$out\DACrux.SEMDMS.BSL.dll" "/r:$out\Miracom.Middleware.dll" "$PSScriptRoot\EtLimitSetupFormSmoke.cs"
if ($LASTEXITCODE -ne 0) { throw 'Limit Smoke 빌드 실패' }
Copy-Item "$out\EtResultUploadSmoke.exe.config" "$out\EtLimitSetupSmoke.exe.config" -Force
& "$out\EtLimitSetupSmoke.exe" $sample "$buildRoot\limit.png" "$PSScriptRoot\EtSpecTable_U9_20261012.txt" $keepArg
if ($LASTEXITCODE -ne 0) { throw 'E-Test Spec 설정 화면 Smoke 실패' }

# 4) Die 좌표 변환(Interface DieCoordinateMapper) 검사 (2026-10-13, DB 사용 안 함)
#    Map 스냅샷: Verification\U9_OMM_Hybrid_UseMap_20261013.tsv (로컬 DB SCK / OMM_Hybrid 의 TQP_USEMAP)
#    고객 대응표: SampleFile\U9_Die좌표_대응표_20261012.tsv (E-Test / KLARF Back / KLARF Front / Die#, 76행)
& $csc /nologo /target:exe /platform:anycpu "/out:$out\DieCoordinateProbe.exe" /r:System.Data.dll /r:System.Core.dll `
    "/r:$out\DACrux.SEMDMS.Interface.dll" "/r:$out\DACrux.SEMDMS.ENGUI.dll" "$PSScriptRoot\DieCoordinateProbe.cs"
if ($LASTEXITCODE -ne 0) { throw 'Die 좌표 Probe 빌드 실패' }
& "$out\DieCoordinateProbe.exe" "$PSScriptRoot\U9_OMM_Hybrid_UseMap_20261013.tsv" (Join-Path $rootDir 'SampleFile\U9_Die좌표_대응표_20261012.tsv') $sample
if ($LASTEXITCODE -ne 0) { throw 'Die 좌표 Probe 실패' }
Write-Output "검사 완료: $buildRoot (화면 이미지 upload.png / view.png / limit.png)"
