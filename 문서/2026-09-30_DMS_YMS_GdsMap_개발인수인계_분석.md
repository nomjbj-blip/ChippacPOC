# DMS / YMS / GdsMap 개발 인수인계 분석

- 작성일: 2026-09-30 (Claude Code 분석)
- 목적: Codex로 진행 중인 개발을 이어서 할 수 있도록 현재 구조와 진행 상태, 남은 작업을 정리한다.
- 관련 문서: `문서/2026-09-23_Chain_Setup_기능_컨셉안.md` (Chain 설계와 구현 이력 17.1~17.44절), `문서/NexplantQMS_DMSYMS_Function_List_v2_20260914.xlsx`, `문서/GDS TABLE.xlsx`

---

## 0. 프로젝트 개요

- 고객 요구 (SCK R&D, `AOI-ET Correlation Automation_r01.pdf`): AOI KLARF Defect 좌표가 GDS CAD의 Daisy Chain(Bump/RDL 배선) 위치와 겹치는지 확인하고, 겹치면 정확한 Bump/RDL 위치를 제공한다. ET Resistance(Open) Fail Chain과 연관 분석을 한다.
- 일정 (WBS V1.0): 2026-10-01 착수, 10-12~10-23 구축(RDS Map, 좌표 변환, Chain, Offset/Tolerance, Correlation Logic), 10-19~21 YMS/DMS 환경 Setting, 10-22~27 검증, 10-30 종료 보고
- POC 포함 기능 (Function List에서 O 표시): Map Setup, Bin Setup, Program Parameter Setup, Defect Setup / Type Setup, **GDS Data Registration, Chain Name Setup, Chain Data Setup, Defect/Test Correlation View**, Defect/E-Test Manual Input, Defect Map/Image Gallery, Chart Analysis, Bin Map View/Gallery, Wafer Cumulative, Bin Stack, Yield Report(Lot/Wafer), Test Map View/Gallery, Multi Parameter, Test Standard, BoxPlot

## 1. 저장소 구성

| 경로 | 내용 |
|---|---|
| `DMS/` | Nexplant QMSplus(DACrux) 레거시 소스. DMS와 YMS(=09_TEST) 모두 여기에 있다 |
| `GdsMapControl/` | **현재 개발 중인** GDSII 파서와 OpenGL Map 컨트롤(.NET 4.8, WinExe) |
| `GdsMapControl_0922/`, `GdsMapControl_chain/` | 과거 스냅샷(git 미추적). `_chain` = 커밋 90bb624 시점 |
| `SampleFile/` | `OMM all layer2.gds`, `.dwg` 샘플 |
| `문서/` | 요구사항, WBS, 설계 문서 |

POC 솔루션: `DMS/00_Solution/DACrux2015wc_ChippacPOC/DACrux2015wc.sln`. 06_GDS 솔루션 폴더에 GdsMap 프로젝트가 들어 있다.

## 2. DMS/YMS 레거시 구조

### 2.1 호출 구조

```
DACruxV5.exe → DACrux.Framework.DACruxMain (Infragistics MDI)
  → ENGUI 폼 → Control → RO
  → [.NET Remoting TCP] → BSL (Windows 서비스) → DSL (Miracom.Middleware.QueryComponent)
  → 08_DBQuery/ORACLE/*.xml → Oracle
```

- 기술: .NET Framework **4.0**(GdsMap만 4.8), Infragistics v12.1, FarPoint Spread 5.0, Miracom.Middleware 3.5
- 서비스 포트: DACruxCore 7510, DACruxSEMDMS(DMS) 7515, DACruxTEST(YMS) 7513. Debug 빌드는 WinForm으로 뜬다. 멀티 시작 프로필: Framework.Service → SEMDMS.Service → TEST.Service → DACruxV5
- 빌드 산출물은 `100_ClientBin`(클라이언트)과 `101_ServerBin`(서버)으로 바로 나간다. 참조가 대부분 HintPath DLL이라서 **코어부터 빌드하지 않으면 오래된 DLL로 컴파일된다.**
- DB 접속 정보는 레지스트리 `HKLM\SOFTWARE\Miracom\Middleware\Connections\{DMSMGR|QMSMGR|MESMGR}`에 있다(ConnectString, QueryPath). app.config의 `*_CONNECT_ID`는 DMSMGR를 가리킨다.
- 클라이언트 DEBUG 빌드는 ADMIN으로 자동 로그인한다(`DACruxClientMain.cs`).

### 2.2 쿼리 XML 규칙

- DSL: `InitQueryComponent(AppSettings["QMS_CONNECT_ID"], "TQC_EQUIP.xml")` → `GetDataTable("QUERY_ID", string[] 동적조각, object[] 바인딩)`
- XML: `<query queryid=".."><commandtext><![CDATA[... :PARAM ... {0} ]]></commandtext><parameters>...`
- `{0}`은 문자열 치환이므로 SQL 인젝션에 주의한다.

### 2.3 화면 추가 절차

1. ENGUI에 `DACrux.Framework.Base.DACruxUXBasic01`을 상속한 폼을 만든다. 결함 연동 화면은 `DACruxUXBasicDefectLink`를 상속한다.
2. 서버 호출이 필요하면 Interface → BSL → RO → DSL → 쿼리 XML을 추가한다. BSL 클래스가 새로 생기면 서비스 exe.config에 `<wellknown>`으로 등록한다.
3. `01_WinApp/DACruxV5/MenuFile.mnu`(DataSet XML)에 `Table1` 행을 추가하고 **`100_ClientBin/MenuFile.mnu`에도 반영**한다. 권한은 FUNCTION_CODE와 TQC_FUNCTION_LIST로 관리한다.

### 2.4 YMS(09_TEST)와 DMS 데이터 흐름

- YMS 화면: frmSetupMap, frmSetupBin, frmYieldReport, frmWaferBinReport, frmProbeBinMapGallery, frmTESTMapGallery, frmPCM*, frmCumMapReport 등. BSL은 ProbeAdmin, ProbeMapAnalysis, DataSelect 등이고 테이블은 TQP_*이다.
- 공용 Map 컨트롤: `06_CoreLibSource/DACrux.Map/WaferMap.cs`(GDI+), `EditWaferMap`, `DefectMap`
- DMS 적재 흐름: PreConvertingService(KLARF/TFF 판별, TFF2KLA 변환, 웨이퍼 분할) → Inspection.DataService(TQC_EQUIP.HANDLER 값으로 Handler 클래스를 생성하고 TQD_* Step 01~13 저장)
- YMS Map 파서: `09_TEST/DACrux.TEST.MAPParser/Parser.cs` (배포본: `101_AppSrvBin_Release/YMSFileMonitor`)
- Defect 좌표: TQD_DEFECT의 X/Y, XREL/YREL(다이 내부 상대 좌표), XINDEX/YINDEX, XSIZE/YSIZE. 다이 원점과 피치는 TQD_SETUP/TQD_PRODUCT에서 가져온다. **GDS 좌표 변환은 XREL/YREL을 기준으로 한다.**

### 2.5 알려진 결함과 위험

- `08_DBQuery/ORACLE/TQC_EQUIP.xml`의 INSERT_DATA에 `:QTY_5, , :QTY_6` 문법 오류가 있다.
- TEST.Service App.config의 wellknown에 존재하지 않는 BSL 클래스가 등록되어 있다.
- 서비스 bat 파일의 경로와 서비스 이름이 실제와 맞지 않는다(DBHitek 경로 등).
- 설비별 DataService exe(`*_ILS201.exe` 등)에 대응하는 `.exe.config`가 없다.
- git에 bin/obj/.vs/ClientBin DLL이 커밋되어 있어서 변경 파일이 많이 표시된다.

## 3. GDS 연동 전 해결 과제

1. GdsMap은 net4.8, DMS는 net4.0이다. DMS 쪽 Target을 올리거나 별도 프로세스로 띄워야 한다.
2. GdsMap이 WinExe이므로 컨트롤 부분을 Class Library로 분리해야 한다.
3. `Defect`/`DefectList` 이름이 `DACrux.Base`와 충돌한다.
4. GdsMap의 `Oracle/*.cs`에 접속 문자열이 **평문 하드코딩**되어 있다. Middleware/BSL 구조로 옮겨야 한다.
5. 권장 연동 방식: `DACrux.SEMDMS.Control`에 GdsMap Wrapper UserControl을 두고 ENGUI 폼(`DACruxUXBasicDefectLink`)과 메뉴를 등록한다. 기존 Defect Viewer에서는 `ShowForm(menuKey, DefectList)`로 넘긴다.

## 4. GdsMapControl 현재 상태

### 4.1 구조

- `Gds/` 파서(`GdsReader.Read`), `Elements/`(Boundary/Path/Text/SRef/ARef — **AREF는 화면에 펼치지 않는다**)
- `Controls/GdsMapControl*.cs` (partial): Flatten → GPU 버퍼 → OpenGL 표시, Layer, Label(GDI+), Mouse, Chain, ChainOverlay, Persistence
- `Chain/`: `ChainCandidateTracer`(Layer 필터 + 공간 격자 + BFS), `ChainGeometryOverlap`, `ChainSimilarGroupFinder`, `ChainSimilarBatchEdit`
- 테스트 폼: `GdsMapTestForm*.cs` 6개 partial(Chain, ChainEdit, ChainExample, ChainList, ChainSimilarReview)
- `Persistence/`: Oracle 스키마 초안(실행 안 함), 배치 빌더(2,000도형/8MiB), 바이너리 본문 v1(`GMP1`, SHA-256) 직렬화와 복원
- `Probes/*.ps1` 9개: Debug exe를 로드해서 모델 API를 검증한다. 실행: `powershell -ExecutionPolicy Bypass -File Probes\ChainLayerFilteredProbe.ps1`
- 빌드: `dotnet build NexplantQMS.GdsMap.csproj --no-restore -p:LangVersion=7.3`. **C# 7.3 호환 코드를 유지한다.** packages HintPath가 `..\packages`(없는 경로)를 가리킨다.

### 4.2 Chain 진행 상태

- 구현됨: Chain 여러 개(A~Z) 관리, Input 1개 필수, Output 0~1개, 지도 클릭 지정, 후보 경로 탐색(열린 경로 포함), 수동 추가/제외, 연결 검사, 유사 묶음 검색과 일괄 제외, 되돌리기(1회), OpenGL Overlay, 배치 ID `PlacedElementId`
- **Chain은 저장되지 않는다.** 폼 메모리(`_chains`)에만 있다.
- 미구현:
  - Device/Revision 선택
  - Map/Chain DB 저장과 재조회 (Chain 테이블 DDL 없음)
  - Chain Part/Sequence
  - Element → Chain 역조회와 Grid 표시
  - AREF Flatten
  - 배치 ID 재파싱 안정성 검증
  - Service 업로드
  - AOI 좌표 변환과 Defect Hit, ET 연계
- **미커밋 파일이 많다.** `GdsMapTestForm_ChainList.cs`(ChainDefinition) 등이 untracked 상태라서 HEAD만 받으면 빌드가 깨질 수 있다.
- 위험 요소:
  - `Task.Run`에서 컨트롤 `AllItems`를 순회한다(스레드 안전성).
  - 5천만 정점을 선할당한다.
  - 클릭 후보를 전체 Scene 순회로 찾는다.
  - 호출되지 않는 폼이 2개 있다(`ChainLayerRuleEditorForm`, `ChainElementCandidateForm`).

### 4.3 다음 단계 권장 순서

1. 현재 작업을 커밋한다(스냅샷 폴더와 .vs는 제외).
2. 배치 ID 안정성 Probe를 만들고 AREF를 지원한다.
3. Chain을 로컬(JSON)에 저장/복원해서 먼저 검증한다.
4. Chain DDL(CHAIN_MASTER, CHAIN_ELEMENT_MEMBER, CHAIN_LAYER_FILTER, CHAIN_ELEMENT_EDIT)을 작성하고 Oracle 저장을 구현한다.
5. Map 업로드와 DB 단독 재조회를 구현한다.
6. AOI 좌표 계약(컨셉안 17.38절 마지막)을 확정하고 Defect Overlay와 역조회를 구현한다.

## 5. File Map Setup 화면 (Codex 작성, 미커밋)

### 5.1 파일

| 파일 | 역할 |
|---|---|
| `DMS/09_TEST/DACrux.TEST.ENGUI/Admin/frmStandardMapSetup.cs` / `.Designer.cs` / `.resx` | 화면 (`DACruxUXBasic01` 상속) |
| `DMS/09_TEST/DACrux.TEST.ENGUI/Admin/StandardMapFile.cs` | 칩팩 텍스트 Map 파서 (internal) |
| `DMS/09_TEST/DACrux.TEST.ENGUI/Verification/` | `Hybrid.map` 샘플, Parser 검사 22건, Form Smoke, `Verify-StandardMapSetup.ps1` |
| `DACrux.TEST.ENGUI.csproj` | 위 파일 등록, Infragistics HintPath 정리 |
| `MenuFile.mnu` (WinApp, ClientBin 둘 다) | `MNU_STANDARD_MAP_FILE`, 부모 `MNU_SETUP_TEST`, 순서 8, `F02302` |

### 5.2 동작

1. 파일 선택 또는 다시 읽기 → `StandardMapFile.Load`
   - 헤더는 `KEY:VALUE` 형식이다. 필수 항목: DEVICE, ROWCT, COLCT, FNLOC(0/90/180/270), DUTMS(mm/mil), XDIES, YDIES. 선택 항목: LOT, WAFER, REFPX/REFPY(기본 1)
   - `RowData:` 행 수는 ROWCT, 셀 수는 COLCT와 같아야 한다. `___`/`__`는 빈 위치이고, 숫자(000 포함)는 모두 사용 칩이다. 그 외 값은 오류로 처리한다.
   - 좌표: X는 왼쪽부터 1, Y는 아래쪽부터 1(첫 RowData가 맨 위, LeftBottom 기준)이다. FNLOC는 NotchAngle에만 넣고 회전하지 않는다.
   - Center = (Cols/2 − REFPX + 1)·PitchX, Y도 같은 방식이다. 원점 초기값으로 쓴다.
2. `DrawFileMap`: `EditWaferMap`(m_wMap)에 파일의 칩만 `AddDie`한다(USECODE=1). DieMin=1, DieMax=Cols/Rows, FirstDie=(1,1), OriginIndex=REFPX/REFPY, Notch=Flat, Edge=0
3. Wafer 크기는 75~300mm 중에서 사용자가 선택해야 한다. 선택 전에는 대각선×1.05 크기로 표시만 한다.
4. 새 Map 저장 → `ProbeAdmin.CheckDuplicateMapID` → `SetMapDef(...)` → `CreateUseMap(Factory, mapId, dies)`. **두 호출이 한 트랜잭션이 아니라서** 정의만 저장될 수 있다(메시지로 안내함).
5. 저장 Map 열기 → 기존 `frmSetupMap.LoadMap(savedMapId)`를 모달리스로 연다.

### 5.3 확인된 문제와 수정 후보

- `MenuFile.mnu`의 신규 항목에 `<VISIBLE>True</VISIBLE>`가 **중복**되어 있다(WinApp과 ClientBin 두 곳). DataSet.ReadXml에서 문제가 될 수 있다.
- 저장 비원자성: SetMapDef 성공 후 CreateUseMap이 실패하면 반쪽 Map이 남는다. 기존 frmSetupMap도 같은 구조다.
- Map ID 기본값은 DEVICE다. 같은 Device를 두 번 등록하면 중복 오류가 나므로 ID 규칙이 필요하다.
- Wafer 크기가 칩 배치보다 작아도 검증하지 않는다.
- FNLOC 90/270일 때 Pitch X/Y와 좌표를 회전하지 않는 정책이 맞는지 고객 확인이 필요하다.
- Bin 값을 버린다(모든 숫자가 사용 칩). BCEQU 헤더는 읽지 않는다.
- 폰트와 레이아웃: Designer가 `AutoScaleDimensions 10x25`, 맑은 고딕 9pt이고, 컨트롤 좌표가 고DPI 기준이다. 다른 DACrux 화면과 스타일(Infragistics)이 다르다.
- `btnOpenMap`은 MDI가 아니라 `form.Show(this)`로 연다. 메뉴 체계(`ShowForm`)를 쓰지 않는다.
- 검증: `DMS/09_TEST/DACrux.TEST.ENGUI/Verification/Verify-StandardMapSetup.ps1` (TEMP에 빌드하고 Parser 22건, Form Smoke, 미리보기 PNG를 만든다. DB는 호출하지 않는다)
