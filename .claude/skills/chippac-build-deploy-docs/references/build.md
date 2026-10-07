# 빌드 명령과 오류 해결

출처: `DMS/Assistant/빌드_검증_가이드.md`, `변경이력/2026-10-02.md`, `Verify-*.ps1` (2026-10-02 기준).
명령이 실패하면 아래 해결책을 적용해 보고, 새로 알게 된 내용은 `빌드_검증_가이드.md`의 "자주 겪은 문제"에 추가한다.

## 공통 변수

```powershell
$ms   = 'C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe'
# 경로가 다르면: & "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -products * -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe'
$dms  = 'D:\1_프로젝트 문서\18.스테츠칩팩_POC\ChippacPOC\DMS'
$out  = "$env:TEMP\ChippacBuild\<작업ID>"      # 작업마다 새 폴더
```

`obj` 폴더도 git으로 관리되고 있다. TEMP 빌드에서는 `IntermediateOutputPath`도 TEMP로 지정해서 저장소의 `obj` 파일이 바뀌지 않게 한다.

## GdsMapControl (.NET 4.8 / WinExe)

```powershell
& $ms "$dms\06_CoreLibSource\GdsMapControl\NexplantQMS.GdsMap.csproj" -t:Build -p:Configuration=Debug -v:minimal
```
- 출력: `GdsMapControl\bin\Debug\NexplantQMS.GdsMap.exe`. DMS는 `100_ClientBin\GdsMap`의 EXE를 실행하므로 화면 변경을 확인하려면 배포해야 한다. 화면 변경이 없는 작업(Probe용 로직만 바꾼 경우)이면 배포하지 않아도 되고, 그 사실을 이력에 적는다.
- 참조 HintPath는 `..\..\00_Solution\DACrux2015wc_ChippacPOC\packages\...` 기준이다. 기본 빌드는 `bin\Debug`에 이미 있는 OpenTK / GLControl DLL이 잡혀서 통과한다.
- OutDir를 다른 곳으로 바꾸면 `CS0246 (Vector2, GLControl)`이 난다. OpenTK / OpenTK.GLControl DLL을 새 OutDir에 먼저 복사한다.
- 실행 중인 GDS 뷰어나 DACrux가 EXE를 잡고 있으면 빌드가 실패한다.
- SEMDMS RO / Interface를 참조하므로 그 DLL들을 바꿨다면 GdsMap도 다시 빌드한다.
- 루트의 `GdsMapControl`, `GdsMapControl_chain`, `GdsMapControl_0922`는 예전 사본이다. 수정이나 빌드는 `DMS\06_CoreLibSource\GdsMapControl`에서만 한다.

## SEMDMS 서버 / 클라이언트 DLL (.NET 4.0)

csproj의 Debug 출력 경로(2026-10-02 확인): Interface / RO / ENGUI는 `..\..\100_ClientBin\`, DSL / BSL은 `..\..\101_ServerBin\`. 기본 빌드는 곧 운영 폴더 배포다. 다른 세션이나 VS가 이렇게 빌드한 흔적(`obj\Debug` 갱신, 운영 DLL 시각 변경)이 보이면 되돌리지 말고 기록만 한다.

의존 순서대로 빌드한다: **Interface -> DSL -> BSL -> RO**. 바꾸지 않은 프로젝트는 빌드하지 않아도 되지만, Interface를 바꿨다면 그것을 참조하는 BSL / RO도 다시 빌드해서 함께 배포한다.

```powershell
function Build-Temp($proj, $refDir) {
  $name = [IO.Path]::GetFileNameWithoutExtension($proj)
  & $ms $proj /t:Build /p:Configuration=Debug /p:Platform=AnyCPU `
    "/p:OutputPath=$out\" "/p:IntermediateOutputPath=$out\obj\$name\" `
    "/p:ReferencePath=$refDir" /v:minimal /nologo
}
Build-Temp "$dms\02_DMS\DACrux.SEMDMS.Interface\DACrux.SEMDMS.Interface.csproj" "$dms\101_ServerBin"
Build-Temp "$dms\02_DMS\DACrux.SEMDMS.DSL\DACrux.SEMDMS.DSL.csproj"             "$dms\101_ServerBin"
Build-Temp "$dms\02_DMS\DACrux.SEMDMS.BSL\DACrux.SEMDMS.BSL.csproj"             "$dms\101_ServerBin"
Build-Temp "$dms\02_DMS\DACrux.SEMDMS.RO\DACrux.SEMDMS.RO.csproj"               "$dms\100_ClientBin"
```
- DSL / BSL은 `ReferencePath=101_ServerBin`을 주지 않으면 Miracom 참조에서 `CS0246`이 난다(플랫폼 조건이 붙은 HintPath 때문으로 보임).
- 프로젝트 참조 DLL을 찾지 못하면 `101_ServerBin`(서버 DLL) 또는 `100_ClientBin`(클라이언트 DLL)에서 같은 이름의 DLL을 `$out`에 먼저 복사한다(2026-10-02 BSL 빌드 때 이 방법으로 통과함).
- 새 클래스를 추가했다면 4개 csproj 각각에 `<Compile Include>`가 있는지 확인한다.
- 새 Query XML은 `08_DBQuery\ORACLE`에 둔다. 기존 XML에 쿼리를 추가할 때는 기존 쿼리를 바꾸지 말고, 원래 파일의 BOM과 줄바꿈 형식을 유지한다.

## ENGUI (SEMDMS.ENGUI / TEST.ENGUI, .NET 4.0)

csproj 출력이 `100_ClientBin`이므로 반드시 TEMP로 빌드한다. `Verify-*.ps1`이 쓰는 방식과 같다.

```powershell
# 1) 프로젝트 참조 DLL(Framework / Interface / RO 등)을 ClientBin에서 OutDir로 복사
#    (2026-10-02 frmGdsMapView 빌드 때 7개 필요. 어떤 DLL이 필요한지는 csproj의 ProjectReference를 보고 정한다)
# 2) 빌드
& $ms "$dms\02_DMS\DACrux.SEMDMS.ENGUI\DACrux.SEMDMS.ENGUI.csproj" /t:Build /p:Configuration=Debug /p:Platform=AnyCPU `
  /p:BuildProjectReferences=false "/p:OutputPath=$out\" "/p:IntermediateOutputPath=$out\obj\ENGUI\" /v:minimal /nologo
```
- 화면 컨트롤 생성과 배치는 `*.Designer.cs`의 `InitializeComponent`에 둔다.
- 새 화면은 메뉴 등록이 두 곳 필요하다: `01_WinApp\DACruxV5\MenuFile.mnu`와 `100_ClientBin\MenuFile.mnu`. **맨 뒤에 행을 추가만 하고 기존 행 순서는 바꾸지 않는다.** 2026-10-02에 파일을 다시 저장하면서 행 순서가 바뀌어 DMS 왼쪽 Wafer 조회가 사라진 적이 있다(CLAUDE-010).

## 오류별 해결

| 증상 | 원인 | 해결 |
| --- | --- | --- |
| `CS0246` Vector2 / GLControl | OutDir 변경으로 OpenTK HintPath가 풀리지 않음 | OpenTK DLL 2개를 OutDir에 복사 |
| `CS0246` Miracom.* | 플랫폼 조건 HintPath | `/p:ReferencePath=<101_ServerBin>` |
| `CS0246` DACrux.* (프로젝트 참조) | `BuildProjectReferences=false` 또는 TEMP OutDir | ClientBin / ServerBin의 DLL을 OutDir에 복사 |
| 새 클래스가 없다는 오류 | non-SDK csproj에 등록 누락 | `<Compile Include="...">` 추가 |
| `MSB3021` / `MSB3027` 파일 잠김 | DACrux / 서비스 / 뷰어 실행 중 | 사용자에게 종료 요청. 강제 종료하지 않는다 |
| csproj 저장 `EPERM` | 관리자 권한 Visual Studio가 열고 있음 | VS의 Reload 안내를 닫거나 VS 종료를 요청 |
| C# 문법 오류(최신 문법) | 언어 버전 7.3 | 패턴 매칭 확장, `using` 선언 등 C# 8 이상 문법을 쓰지 않는다 |
| Probe에서 `Add-Type` 참조 실패 | `.exe`는 참조로 쓸 수 없음 | GDS EXE를 TEMP에 `.dll` 이름으로 복사해서 참조 |
