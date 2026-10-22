# POC 화면 버튼 아이콘 / 크기 / 배치 기준 검사 (2026-10-16)
# 기준 문서: 문서/설계/06_환경_운영/2026-10-16_POC_화면_버튼_아이콘_레이아웃_기준.md
# 하는 일:
#  1) 운영 폴더(100_ClientBin)는 바꾸지 않고 %TEMP%\chippac\buttonlayout 에 SEMDMS.Control / TEST.ENGUI / SEMDMS.ENGUI / GdsMap DLL / MenuFileEditor 를 따로 빌드한다.
#  2) PocButtonLayoutCheck.cs 로 POC 화면을 화면 밖에 띄워(Load 처리 없이, DB / Service 사용 안 함) 실행 크기로 버튼 아이콘 / 높이 / 글자 잘림 / 컨트롤 겹침을 확인한다.
#  3) 결과: %TEMP%\chippac\buttonlayout\report.md, 마지막 줄 POC_BUTTON_LAYOUT=PASS / FAIL (n), 화면 그림 images\<화면>.png
# 새 POC 화면을 만들면 아래 $forms 목록에 한 줄 추가한다.
# 실행: powershell -NoProfile -STA -ExecutionPolicy Bypass -File Verify-PocButtonLayout.ps1 [-SkipBuild]
param([switch]$SkipBuild)
$ErrorActionPreference = 'Stop'
$projectDir = Split-Path $PSScriptRoot -Parent
$dmsDir = Split-Path (Split-Path $projectDir -Parent) -Parent
$clientBin = Join-Path $dmsDir '100_ClientBin'
$iconDir = Join-Path $dmsDir '99_Resource\ByModule\POC\Button'
$buildRoot = Join-Path $env:TEMP 'chippac\buttonlayout'
$out = Join-Path $buildRoot 'bin'
$gdsOut = Join-Path $buildRoot 'gdsmap'
$menuOut = Join-Path $buildRoot 'menueditor'
$vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
$msbuild = & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
$csc = Join-Path $env:WINDIR 'Microsoft.NET\Framework\v4.0.30319\csc.exe'
if (-not $msbuild -or -not (Test-Path $csc)) { throw 'Visual Studio MSBuild / .NET Framework C# 컴파일러가 필요합니다.' }

function Build-Temp($proj, $outDir, $name) {
    & $msbuild $proj /t:Build /p:Configuration=Debug /p:Platform=AnyCPU /p:BuildProjectReferences=false `
        "/p:OutputPath=$outDir\" "/p:IntermediateOutputPath=$buildRoot\obj\$name\" /v:minimal /nologo *> "$buildRoot\build_$name.log"
    if ($LASTEXITCODE -ne 0) { Get-Content "$buildRoot\build_$name.log" -Tail 20; throw "빌드 실패: $name" }
    $warn = @(Select-String -Path "$buildRoot\build_$name.log" -Pattern ': warning ' | Where-Object { $_.Line -notmatch 'MSB3' })
    Write-Output "빌드: $name (경고 $($warn.Count))"
}

if (-not $SkipBuild) {
    if (Test-Path $buildRoot) { Remove-Item $buildRoot -Recurse -Force }
    New-Item -ItemType Directory -Force $out, $gdsOut, $menuOut | Out-Null
    # 참조 DLL 은 운영 클라이언트 폴더에서 복사하고, 이번에 바꾼 화면 프로젝트만 새로 빌드한다
    Get-ChildItem $clientBin -Filter '*.dll' | ForEach-Object { Copy-Item $_.FullName $out -Force }
    # 화면이 새 원격 메서드를 부르면 운영 폴더 RO / Interface 로는 빌드되지 않아 같이 빌드한다 (2026-10-22)
    Build-Temp "$dmsDir\02_DMS\DACrux.SEMDMS.Interface\DACrux.SEMDMS.Interface.csproj" $out 'SEMDMS.Interface'
    Build-Temp "$dmsDir\02_DMS\DACrux.SEMDMS.RO\DACrux.SEMDMS.RO.csproj" $out 'SEMDMS.RO'
    Build-Temp "$dmsDir\02_DMS\DACrux.SEMDMS.Control\DACrux.SEMDMS.Control.csproj" $out 'SEMDMS.Control'
    Build-Temp "$dmsDir\09_TEST\DACrux.TEST.ENGUI\DACrux.TEST.ENGUI.csproj" $out 'TEST.ENGUI'
    Build-Temp "$projectDir\DACrux.SEMDMS.ENGUI.csproj" $out 'SEMDMS.ENGUI'
    Build-Temp "$dmsDir\06_CoreLibSource\GdsMapControl\NexplantQMS.GdsMap.csproj" $gdsOut 'GdsMap'
    Build-Temp "$dmsDir\01_WinApp\MenuFileEditor\MenuFileEditor.csproj" $menuOut 'MenuFileEditor'
}

& $csc /nologo /target:exe /platform:x86 "/out:$out\PocButtonLayoutCheck.exe" /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.Core.dll "$PSScriptRoot\PocButtonLayoutCheck.cs"
if ($LASTEXITCODE -ne 0) { throw '검사 프로그램 빌드 실패' }

$semdms = Join-Path $out 'DACrux.SEMDMS.ENGUI.dll'
$test = Join-Path $out 'DACrux.TEST.ENGUI.dll'
$gds = Join-Path $gdsOut 'NexplantQMS.GdsMap.dll'
$menu = Join-Path $menuOut 'MenuFileEditor.exe'
$forms = @(
    "$semdms|DACrux.SEMDMS.ENGUI.frmKlarfUpload",
    "$semdms|DACrux.SEMDMS.ENGUI.frmKlarfFileViewer",
    "$semdms|DACrux.SEMDMS.ENGUI.frmGdsKlarfAnalysis",
    "$semdms|DACrux.SEMDMS.ENGUI.frmGdsKlarfSourceFind",
    "$semdms|DACrux.SEMDMS.ENGUI.frmGdsKlarfSimple",
    "$semdms|DACrux.SEMDMS.ENGUI.frmEtParaRegister",
    "$semdms|DACrux.SEMDMS.ENGUI.frmEtParaSetup",
    "$semdms|DACrux.SEMDMS.ENGUI.frmEtResultUpload",
    "$semdms|DACrux.SEMDMS.ENGUI.frmEtResultView",
    "$semdms|DACrux.SEMDMS.ENGUI.frmEtBinSetup",
    "$test|DACrux.TEST.ENGUI.frmStandardMapSetup",
    "$test|DACrux.TEST.ENGUI.frmStandardMapView",
    "$gds|NexplantQMS.GdsMap.GdsMapForm",
    "$gds|NexplantQMS.GdsMap.GdsMapViewForm",
    "$gds|NexplantQMS.GdsMap.GdsDieAnalysisForm",
    "$gds|NexplantQMS.GdsMap.MapCopyForm",
    "$gds|NexplantQMS.GdsMap.GdsMapViewU8Form",
    "$menu|MenuFileEditor.MenuEditorForm",
    "$menu|MenuFileEditor.IconEditForm"
)
$list = Join-Path $buildRoot 'forms.txt'
[IO.File]::WriteAllLines($list, $forms, (New-Object Text.UTF8Encoding($false)))
& "$out\PocButtonLayoutCheck.exe" $iconDir $list (Join-Path $buildRoot 'report.md') (Join-Path $buildRoot 'images')
$code = $LASTEXITCODE
Write-Output "결과: $(Join-Path $buildRoot 'report.md')"
exit $code
