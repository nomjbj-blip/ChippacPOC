# E-Test Bin 설정 화면(frmEtBinSetup) Smoke 검사 (2026-10-22)
# Smoke = 화면을 화면 밖에서 실제로 띄워 Device 고르기 -> 조회 -> 이름 / 양품 / 색 수정 -> Bin 추가 -> 저장 + 수율 다시 계산 -> 삭제 흐름이
# 깨지지 않는지 확인하는 자동 검사. 설계: 문서/설계/04_ETest/2026-10-22_ETest_Bin_Setup_화면_설계.md 11절
# 하는 일 (Verify-EtParaSetup.ps1 과 같은 구조):
#  1) 운영 폴더(100_ClientBin / 101_ServerBin)는 바꾸지 않고 %TEMP% 에 Interface / DSL / BSL / RO / Control / ENGUI 를 따로 빌드한다.
#  2) Service(Remoting) 대신 BSL 을 같은 프로세스에 붙여 화면 -> RO -> BSL -> 로컬 DB 를 실제로 거친다.
#  3) 샘플: SampleFile\26_10_06 Sample File\CAI_OMM_ATV_A0_TSVET_20260609161940_RH-HS2615_RH-HS2615-02.xlsx / 고객 Spec 표 Verification\EtSpecTable_U9_20261012.txt
#  4) 시험 데이터: FACTORY=PROBE / Device PRBBINUI / 공정 TSVET / Slot 7. 시험 Map(TQP_USEMAP)은 SCK OMM_Hybrid 사용 Die 를 복사했다가 끝나면 지운다.
#  5) 결과 화면 이미지: %TEMP%\chippac\EtBinSetupVerification\bin_setup.png
#  6) DB 연결 이름: 서버 config 의 DMS / TEST / QMS_CONNECT_ID 가 다른 DB(예: CPKMESDV)여도 Smoke 설정에는 -ConnectId(기본 DMSMGR = 로컬)를 넣는다.
#     Smoke 는 시험 데이터를 만들고 지우므로 고객 DB 에서 돌면 안 된다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File Verify-EtBinSetup.ps1 [-Keep] [-ConnectId DMSMGR] [-SkipBuild]
param([switch]$Keep, [string]$ConnectId = 'DMSMGR', [switch]$SkipBuild)
$ErrorActionPreference = 'Stop'
$projectDir = Split-Path $PSScriptRoot -Parent
$dmsDir = Split-Path (Split-Path $projectDir -Parent) -Parent
$rootDir = Split-Path $dmsDir -Parent
$clientBin = Join-Path $dmsDir '100_ClientBin'
$serverBin = Join-Path $dmsDir '101_ServerBin'
$buildRoot = Join-Path $env:TEMP 'chippac\EtBinSetupVerification'
$out = Join-Path $buildRoot 'bin'
$vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
$msbuild = & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
$csc = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'
if (-not $msbuild -or -not (Test-Path $csc)) { throw 'Visual Studio MSBuild / .NET Framework C# compiler 가 필요합니다.' }

function Build-Temp($proj, $refDir) {
    $name = [IO.Path]::GetFileNameWithoutExtension($proj)
    & $msbuild $proj /t:Build /p:Configuration=Debug /p:Platform=AnyCPU /p:BuildProjectReferences=false `
        "/p:OutputPath=$out\" "/p:IntermediateOutputPath=$buildRoot\obj\$name\" "/p:ReferencePath=$out%3B$refDir" /v:minimal /nologo *> "$buildRoot\build_$name.log"
    if ($LASTEXITCODE -ne 0) { Get-Content "$buildRoot\build_$name.log" -Tail 20; throw "빌드 실패: $name" }
    $warn = @(Select-String -Path "$buildRoot\build_$name.log" -Pattern ': warning ' | Where-Object { $_.Line -notmatch 'MSB3' })
    Write-Output "빌드: $name (경고 $($warn.Count))"
    $warn | ForEach-Object { Write-Output ('  ' + ($_.Line -replace '^.*\\02_DMS\\', '')) }
}

# 시험 Map: 로컬 docker DB 에 SQL 실행. PowerShell 이 보내는 글자 앞 BOM 때문에 첫 줄은 버리는 REM 으로 둔다.
function Sql([string]$text) {
    $script = "REM`nWHENEVER SQLERROR EXIT 1`nSET FEEDBACK OFF`n" + $text + "`nCOMMIT;`nEXIT`n"
    $sqlOut = $script | docker exec -i -e NLS_LANG=AMERICAN_AMERICA.AL32UTF8 oracle19c-DMS bash -lc 'sqlplus -s / as sysdba'
    if ($LASTEXITCODE -ne 0) { throw "SQL 실패: $sqlOut" }
}
function MapOn() {
    Sql "DELETE FROM DMSMGR.TQP_USEMAP WHERE FACTORY = 'PROBE' AND MAPID = 'PRBBINUI';`nINSERT INTO DMSMGR.TQP_USEMAP (FACTORY, MAPID, PROGRAM_REV, X, Y, USECODE, DIE_NUM) SELECT 'PROBE', 'PRBBINUI', PROGRAM_REV, X, Y, USECODE, DIE_NUM FROM DMSMGR.TQP_USEMAP WHERE FACTORY = 'SCK' AND MAPID = 'OMM_Hybrid';"
}
function MapOff() { Sql "DELETE FROM DMSMGR.TQP_USEMAP WHERE FACTORY = 'PROBE' AND MAPID = 'PRBBINUI';" }

if (-not $SkipBuild) {
    if (Test-Path $buildRoot) { Remove-Item $buildRoot -Recurse -Force }
    New-Item -ItemType Directory -Force $out | Out-Null

    # 0) 코어 DLL (71_CoreLib) -> OutDir (BuildProjectReferences=false TEMP 빌드에 필요)
    $coreLib = Join-Path $dmsDir '71_CoreLib'
    if (Test-Path $coreLib) { Get-ChildItem $coreLib -Filter '*.dll' | ForEach-Object { Copy-Item $_.FullName $out -Force } }

    # 1) 서버 / 원격 호출 DLL -> ENGUI
    foreach ($d in 'DACrux.Data.Parser', 'DACrux.Common.DSL', 'DACrux.TEST.DSL', 'DACrux.TEST.BSL', 'DACrux.Base', 'Miracom.Middleware') {
        $p = Join-Path $serverBin "$d.dll"; if (Test-Path $p) { Copy-Item $p $out -Force }
    }
    foreach ($d in 'DACrux.Common.RO', 'DACrux.Common.Interface', 'DACrux.Utility') { Copy-Item (Join-Path $clientBin "$d.dll") $out -Force }
    Build-Temp "$dmsDir\02_DMS\DACrux.SEMDMS.Interface\DACrux.SEMDMS.Interface.csproj" $serverBin
    Build-Temp "$dmsDir\02_DMS\DACrux.SEMDMS.DSL\DACrux.SEMDMS.DSL.csproj" $serverBin
    Build-Temp "$dmsDir\02_DMS\DACrux.SEMDMS.BSL\DACrux.SEMDMS.BSL.csproj" $serverBin
    Build-Temp "$dmsDir\02_DMS\DACrux.SEMDMS.RO\DACrux.SEMDMS.RO.csproj" $clientBin
    foreach ($d in 'DACrux.Data.Handler', 'DACrux.TEST.Control', 'DACrux.TEST.ENGUI', 'DACrux.TEST.Interface', 'DACrux.TEST.RO') {
        Copy-Item (Join-Path $clientBin "$d.dll") $out -Force
    }
    Build-Temp "$dmsDir\02_DMS\DACrux.SEMDMS.Control\DACrux.SEMDMS.Control.csproj" $clientBin
    Build-Temp "$projectDir\DACrux.SEMDMS.ENGUI.csproj" $clientBin

    # 실행에 필요한 나머지 DLL: 이미 있는 것은 덮어쓰지 않는다(방금 빌드한 Interface / DSL / BSL / RO / ENGUI 보존)
    foreach ($dir in $clientBin, $serverBin) {
        Get-ChildItem $dir -Filter '*.dll' | Where-Object { -not (Test-Path (Join-Path $out $_.Name)) } | ForEach-Object { Copy-Item $_.FullName $out }
    }
}

# 2) Smoke 실행 파일 + 설정 (Service config 복사, Remoting 서버 등록은 뺀다)
& $csc /nologo /target:exe /platform:anycpu "/out:$out\EtBinSetupSmoke.exe" /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.Data.dll /r:System.Core.dll `
    "/r:$out\DACrux.SEMDMS.ENGUI.dll" "/r:$out\DACrux.SEMDMS.Control.dll" "/r:$out\DACrux.Framework.Base.dll" "/r:$out\DACrux.Base.dll" "/r:$out\DACrux.SEMDMS.DSL.dll" `
    "/r:$out\DACrux.SEMDMS.RO.dll" "/r:$out\DACrux.SEMDMS.Interface.dll" "/r:$out\DACrux.SEMDMS.BSL.dll" "/r:$out\DACrux.TEST.DSL.dll" "/r:$out\DACrux.TEST.BSL.dll" `
    "/r:$out\Miracom.Middleware.dll" "$PSScriptRoot\EtBinSetupFormSmoke.cs"
if ($LASTEXITCODE -ne 0) { throw 'Smoke 빌드 실패' }
[xml]$x = Get-Content (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config') -Encoding UTF8
$remoting = $x.configuration.'system.runtime.remoting'
if ($remoting) { [void]$x.configuration.RemoveChild($remoting) }
$connectKeys = @($x.configuration.appSettings.add | Where-Object { $_.key -in 'DMS_CONNECT_ID', 'TEST_CONNECT_ID', 'QMS_CONNECT_ID' })
if ($connectKeys.Count -eq 0) { throw 'Service config 에 DMS_CONNECT_ID 가 없습니다.' }
foreach ($k in $connectKeys) {
    if ($k.value -ne $ConnectId) { Write-Output "DB 연결 이름: $($k.key) $($k.value) -> $ConnectId (Smoke 설정만, 서버 config 는 그대로)" }
    $k.value = $ConnectId
}
$x.Save("$out\EtBinSetupSmoke.exe.config")

$sample = Join-Path $rootDir 'SampleFile\26_10_06 Sample File\CAI_OMM_ATV_A0_TSVET_20260609161940_RH-HS2615_RH-HS2615-02.xlsx'
$keepArg = if ($Keep) { 'keep' } else { '' }
MapOn
try {
    & "$out\EtBinSetupSmoke.exe" $sample "$buildRoot\bin_setup.png" "$PSScriptRoot\EtSpecTable_U9_20261012.txt" $keepArg
    $smokeExit = $LASTEXITCODE
} finally {
    if (-not $Keep) { MapOff }
}
if ($smokeExit -ne 0) { throw 'E-Test Bin 설정 화면 Smoke 실패' }
Write-Output "검사 완료: $buildRoot (화면 이미지 bin_setup.png)"
