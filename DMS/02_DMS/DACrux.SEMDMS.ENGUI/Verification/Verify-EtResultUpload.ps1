# E-Test 결과 업로드 화면(frmEtResultUpload) + 결과 조회 화면(frmEtResultView) Smoke 검사 (2026-10-07 작성, 2026-10-14 Upload 재설계, 2026-10-15 Upload / View 분리 + 업로드 파일 하나씩으로 다시 작성)
# Smoke = 화면을 화면 밖에서 실제로 띄워 파일 하나씩 열기 -> 상태 / Slot 입력 -> 공통 결과 Control(Die / 정의 Para 값 거르기) -> 저장 확인 문구 -> 저장 -> 중복 아니요 · 예(덮어쓰기) -> [다시 읽기]
#         -> 조회 화면 목록(추가 Para / Spec 바뀜) / 업로드 미리보기와 같은 결과 / Map 다시 계산 / 삭제 흐름이 깨지지 않는지 확인하는 자동 검사. Verify-KlarfUpload.ps1 과 같은 구조.
# 하는 일:
#  1) 운영 폴더(100_ClientBin / 101_ServerBin)는 바꾸지 않고 %TEMP% 에 Interface / DSL / BSL / RO / Control / ENGUI 를 따로 빌드한다.
#  2) Service(Remoting) 대신 BSL 을 같은 프로세스에 붙여 화면 -> RO -> BSL -> 로컬 DB 를 실제로 거친다.
#  3) 샘플: SampleFile\26_10_06 Sample File\CAI_OMM_ATV_A0_TSVET_20260609161940_RH-HS2615_RH-HS2615-02.xlsx (Excel 에 열려 있어도 됨)
#  4) 시험 데이터: FACTORY=PROBE / Device OMM(파일명) / TSVET 등록 Parameter 37(고객 Spec 표 Verification\EtSpecTable_U9_20261012.txt) + Spec. 끝나면 지운다.
#     업로드는 Map 이 필수(2026-10-15)라 로컬 docker oracle19c-DMS 에 시험 Map(PROBE / MAPID = OMM, SCK OMM_Hybrid 사용 Die 복사)을 넣고 끝나면 지운다.
#  5) 결과 화면 이미지: %TEMP%\ChippacEtResultUploadVerification\upload.png (업로드) / view.png (조회)
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File Verify-EtResultUpload.ps1 [-Keep]
#   -Keep: 저장 파일 / 등록 Parameter / 시험 Map(PROBE / OMM)을 지우지 않는다.
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
# DACrux.TEST.BSL: 2026-10-14 SEMDMS.BSL 이 YMS 적재 때문에 참조한다(BuildProjectReferences=false 빌드에 DLL 이 필요)
foreach ($d in 'DACrux.Data.Parser', 'DACrux.Common.DSL', 'DACrux.TEST.DSL', 'DACrux.TEST.BSL', 'DACrux.Base', 'Miracom.Middleware') {
    $p = Join-Path $serverBin "$d.dll"; if (Test-Path $p) { Copy-Item $p $out -Force }
}
# DACrux.Utility: 2026-10-08 RO csproj 에 프로젝트 참조가 생겨(빌드 경로 정리 작업) BuildProjectReferences=false 빌드에 DLL 이 필요하다
foreach ($d in 'DACrux.Common.RO', 'DACrux.Common.Interface', 'DACrux.Utility') { Copy-Item (Join-Path $clientBin "$d.dll") $out -Force }
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

# 실행에 필요한 나머지 DLL: 이미 있는 것은 덮어쓰지 않는다(방금 빌드한 Interface / BSL / RO / ENGUI 보존)
foreach ($dir in $clientBin, $serverBin) {
    Get-ChildItem $dir -Filter '*.dll' | Where-Object { -not (Test-Path (Join-Path $out $_.Name)) } | ForEach-Object { Copy-Item $_.FullName $out }
}

# 2) Smoke 실행 파일 + 설정 (Service config 복사, Remoting 서버 등록은 뺀다)
& $csc /nologo /target:exe /platform:anycpu "/out:$out\EtResultUploadSmoke.exe" /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.Data.dll /r:System.Core.dll `
    "/r:$out\DACrux.SEMDMS.ENGUI.dll" "/r:$out\DACrux.SEMDMS.Control.dll" "/r:$out\DACrux.Framework.Base.dll" "/r:$out\DACrux.Base.dll" `
    "/r:$out\DACrux.SEMDMS.RO.dll" "/r:$out\DACrux.SEMDMS.Interface.dll" "/r:$out\DACrux.SEMDMS.BSL.dll" "/r:$out\Miracom.Middleware.dll" "$PSScriptRoot\EtResultUploadFormSmoke.cs"
if ($LASTEXITCODE -ne 0) { throw 'Smoke 빌드 실패' }
[xml]$x = Get-Content (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config') -Encoding UTF8
$remoting = $x.configuration.'system.runtime.remoting'
if ($remoting) { [void]$x.configuration.RemoveChild($remoting) }
$x.Save("$out\EtResultUploadSmoke.exe.config")

$sample = Join-Path $rootDir 'SampleFile\26_10_06 Sample File\CAI_OMM_ATV_A0_TSVET_20260609161940_RH-HS2615_RH-HS2615-02.xlsx'
$keepArg = if ($Keep) { 'keep' } else { '' }
# 시험 Map: 로컬 docker DB 에 SQL 실행 (GdsEtResultProbe.ps1 과 같은 방식)
function Sql([string]$text) {
    $script = "WHENEVER SQLERROR EXIT 1`nSET FEEDBACK OFF`n" + $text + "`nCOMMIT;`nEXIT`n"
    $result = $script | docker exec -i -e NLS_LANG=AMERICAN_AMERICA.AL32UTF8 oracle19c-DMS bash -lc 'sqlplus -s / as sysdba'
    if ($LASTEXITCODE -ne 0) { throw "SQL 실패: $result" }
}
$deleteMap = "DELETE FROM DMSMGR.TQP_USEMAP WHERE FACTORY = 'PROBE' AND MAPID = 'OMM';"
Sql ($deleteMap + "`nINSERT INTO DMSMGR.TQP_USEMAP (FACTORY, MAPID, PROGRAM_REV, X, Y, USECODE, DIE_NUM) SELECT 'PROBE', 'OMM', PROGRAM_REV, X, Y, USECODE, DIE_NUM FROM DMSMGR.TQP_USEMAP WHERE FACTORY = 'SCK' AND MAPID = 'OMM_Hybrid';")
try {
    & "$out\EtResultUploadSmoke.exe" $sample "$buildRoot\upload.png" "$buildRoot\work" "$PSScriptRoot\EtSpecTable_U9_20261012.txt" $keepArg
    $smokeExit = $LASTEXITCODE
}
finally {
    if (-not $Keep) { Sql $deleteMap }
}
if ($smokeExit -ne 0) { throw 'E-Test 업로드 / 조회 화면 Smoke 실패' }

# 3) Spec 편집 화면 Smoke 는 2026-10-14 Verify-EtParaSetup.ps1 에 있다(E-Test Limit 설정 화면을 Para 설정 [등록 Parameter] 탭으로 합침).

# 4) Die 좌표 변환(Interface DieCoordinateMapper) 검사 (2026-10-13, DB 사용 안 함)
#    Map 스냅샷: Verification\U9_OMM_Hybrid_UseMap_20261013.tsv (로컬 DB SCK / OMM_Hybrid 의 TQP_USEMAP)
#    고객 대응표: SampleFile\U9_Die좌표_대응표_20261012.tsv (E-Test / KLARF Back / KLARF Front / Die#, 76행)
& $csc /nologo /target:exe /platform:anycpu "/out:$out\DieCoordinateProbe.exe" /r:System.Data.dll /r:System.Core.dll `
    "/r:$out\DACrux.SEMDMS.Interface.dll" "/r:$out\DACrux.SEMDMS.ENGUI.dll" "$PSScriptRoot\DieCoordinateProbe.cs"
if ($LASTEXITCODE -ne 0) { throw 'Die 좌표 Probe 빌드 실패' }
& "$out\DieCoordinateProbe.exe" "$PSScriptRoot\U9_OMM_Hybrid_UseMap_20261013.tsv" (Join-Path $rootDir 'SampleFile\U9_Die좌표_대응표_20261012.tsv') $sample
if ($LASTEXITCODE -ne 0) { throw 'Die 좌표 Probe 실패' }
Write-Output "검사 완료: $buildRoot (화면 이미지 upload.png / view.png)"
