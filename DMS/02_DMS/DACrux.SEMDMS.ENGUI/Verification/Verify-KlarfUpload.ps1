# KLARF 업로드 화면(frmKlarfUpload) Smoke 검사 (F08 구현 3단계)
# Smoke = 화면을 화면 밖에서 실제로 띄워 [분석] -> [DB 저장] 흐름이 깨지지 않는지 빠르게 확인하는 자동 검사.
# 하는 일:
#  1) 운영 폴더(100_ClientBin / 101_ServerBin)는 바꾸지 않고 %TEMP% 에 Interface / BSL / RO / ENGUI 를 따로 빌드한다.
#  2) Service(Remoting) 대신 BSL 을 같은 프로세스에 붙여 화면 -> RO -> BSL -> 로컬 DB 를 실제로 거친다.
#  3) 샘플: SampleFile\26_10_06 Sample File\XD106.02 (+ Defect Images 15장). 이미지 저장소는 %TEMP% 아래 시험 폴더.
#  4) 결과 화면 이미지: %TEMP%\ChippacKlarfUploadVerification\upload.png
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File Verify-KlarfUpload.ps1 [-Keep]
#   -Keep: 마지막 저장 Wafer 를 지우지 않는다(DMS 화면에서 확인할 때).
param([switch]$Keep)
$ErrorActionPreference = 'Stop'
$projectDir = Split-Path $PSScriptRoot -Parent
$dmsDir = Split-Path (Split-Path $projectDir -Parent) -Parent
$rootDir = Split-Path $dmsDir -Parent
$clientBin = Join-Path $dmsDir '100_ClientBin'
$serverBin = Join-Path $dmsDir '101_ServerBin'
$buildRoot = Join-Path $env:TEMP 'ChippacKlarfUploadVerification'
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
    $warn = @(Select-String -Path "$buildRoot\build_$name.log" -Pattern ': warning ' | Where-Object { $_.Line -notmatch 'MSB3' }).Count
    Write-Output "빌드: $name (경고 $warn)"
}

# 1) 서버 / 원격 호출 DLL (새 iKlarfUpload / KlarfUpload) -> ENGUI
# DACrux.TEST.BSL: 2026-10-14 SEMDMS.BSL 이 YMS 적재 때문에 참조한다(BuildProjectReferences=false 빌드에 DLL 이 필요)
foreach ($d in 'DACrux.Data.Parser', 'DACrux.Common.DSL', 'DACrux.TEST.DSL', 'DACrux.TEST.BSL', 'DACrux.Base', 'Miracom.Middleware') {
    $p = Join-Path $serverBin "$d.dll"; if (Test-Path $p) { Copy-Item $p $out -Force }
}
foreach ($d in 'DACrux.Common.RO', 'DACrux.Common.Interface') { Copy-Item (Join-Path $clientBin "$d.dll") $out -Force }
# RO / ENGUI 의 프로젝트 참조(Base / Framework / Map / Utility 등, 다른 작업으로 늘어남)는 ClientBin DLL 로 맞춘다. 이미 있는 DLL(방금 빌드한 것)은 덮어쓰지 않는다.
Get-ChildItem $clientBin -Filter '*.dll' | Where-Object { -not (Test-Path (Join-Path $out $_.Name)) } | ForEach-Object { Copy-Item $_.FullName $out }
Build-Temp "$dmsDir\02_DMS\DACrux.SEMDMS.Interface\DACrux.SEMDMS.Interface.csproj" $serverBin
# BSL 에는 다른 작업(E-Test 등)이 새 DSL 클래스를 참조하는 코드가 함께 있을 수 있어 DSL 도 현재 소스로 빌드한다(운영 DSL 은 바꾸지 않음)
Build-Temp "$dmsDir\02_DMS\DACrux.SEMDMS.DSL\DACrux.SEMDMS.DSL.csproj" $serverBin
Build-Temp "$dmsDir\02_DMS\DACrux.SEMDMS.BSL\DACrux.SEMDMS.BSL.csproj" $serverBin
Build-Temp "$dmsDir\02_DMS\DACrux.SEMDMS.RO\DACrux.SEMDMS.RO.csproj" $clientBin
foreach ($d in 'DACrux.Data.Handler', 'DACrux.TEST.Control', 'DACrux.TEST.ENGUI', 'DACrux.TEST.Interface', 'DACrux.TEST.RO') {
    Copy-Item (Join-Path $clientBin "$d.dll") $out -Force
}
# DACrux.SEMDMS.Control: 2026-10-14 공통 진행 표시줄(ProgressStatusBar)이 들어 있어 운영 폴더 DLL 대신 새로 빌드한다
Build-Temp "$dmsDir\02_DMS\DACrux.SEMDMS.Control\DACrux.SEMDMS.Control.csproj" $clientBin
Build-Temp "$projectDir\DACrux.SEMDMS.ENGUI.csproj" $clientBin

# 실행에 필요한 나머지 DLL: 이미 있는 것은 덮어쓰지 않는다(방금 빌드한 Interface / BSL / RO / ENGUI 보존)
foreach ($dir in $clientBin, $serverBin) {
    Get-ChildItem $dir -Filter '*.dll' | Where-Object { -not (Test-Path (Join-Path $out $_.Name)) } | ForEach-Object { Copy-Item $_.FullName $out }
}

# 2) Smoke 실행 파일 + 설정 (Service config 복사 + 시험 이미지 저장소 / 작업 폴더)
& $csc /nologo /target:exe /platform:anycpu "/out:$out\KlarfUploadSmoke.exe" /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.Data.dll `
    "/r:$out\DACrux.SEMDMS.ENGUI.dll" "/r:$out\DACrux.Framework.Base.dll" "/r:$out\DACrux.Map.dll" "/r:$out\DACrux.Base.dll" `
    "/r:$out\DACrux.SEMDMS.RO.dll" "/r:$out\DACrux.SEMDMS.Interface.dll" "/r:$out\DACrux.SEMDMS.BSL.dll" "/r:$out\Miracom.Middleware.dll" "/r:$out\DACrux.SEMDMS.DSL.dll" "$PSScriptRoot\KlarfUploadFormSmoke.cs"
if ($LASTEXITCODE -ne 0) { throw 'Smoke 빌드 실패' }
[xml]$x = Get-Content (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config') -Encoding UTF8
foreach ($kv in @(@('KLARF_IMAGE_ROOT', (Join-Path $buildRoot 'FTP\SCK')), @('KLARF_UPLOAD_WORK_PATH', (Join-Path $buildRoot 'work')))) {
    $old = $x.configuration.appSettings.add | Where-Object { $_.key -eq $kv[0] }
    if ($old) { [void]$x.configuration.appSettings.RemoveChild($old) }
    $n = $x.CreateElement('add'); $n.SetAttribute('key', $kv[0]); $n.SetAttribute('value', $kv[1])
    [void]$x.configuration.appSettings.AppendChild($n)
}
# Service config 의 Remoting 서버 등록(system.runtime.remoting)은 Smoke 에 필요 없고 포트를 열 수 있어 뺀다
$remoting = $x.configuration.'system.runtime.remoting'
if ($remoting) { [void]$x.configuration.RemoveChild($remoting) }
$x.Save("$out\KlarfUploadSmoke.exe.config")

$sample = Join-Path $rootDir 'SampleFile\26_10_06 Sample File\XD106.02'
$keepArg = if ($Keep) { 'keep' } else { '' }
& "$out\KlarfUploadSmoke.exe" "$out\DACrux.SEMDMS.ENGUI.dll" $sample "$buildRoot\upload.png" $keepArg
if ($LASTEXITCODE -eq 2) { Write-Output 'Smoke 건너뜀: 샘플 Wafer 에 사용자가 올린 데이터가 있어 지우지 않았습니다.'; exit 2 }
if ($LASTEXITCODE -ne 0) { throw 'KLARF 업로드 화면 Smoke 실패' }
Write-Output "검사 완료: $buildRoot (화면 이미지 upload.png)"
