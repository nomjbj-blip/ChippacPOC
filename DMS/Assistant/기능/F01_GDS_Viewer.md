# F01 / GDS Viewer / Layer / Label / Grid

확인일: 2026-10-02 / 담당: 후속 작업 시 작업 현황에 등록 / 상태: 소스 확인

## 목적

GDSII 도면을 읽고 Layer별 도형 / TEXT를 표시한다. Chain 지정과 이후 Defect 위치 비교에서 공통 도면 역할을 한다.

## 주요 파일

아래 GDS 경로는 `DMS/06_CoreLibSource/GdsMapControl` 기준이다.

| 파일 | 역할 |
| --- | --- |
| [GdsReader.cs](../../06_CoreLibSource/GdsMapControl/Gds/GdsReader.cs) | GDS 파일 파싱 |
| [GdsMapControl.cs](../../06_CoreLibSource/GdsMapControl/Controls/GdsMapControl.cs) | Structure 펼치기 / Scene / OpenGL 표시 / 성능 지표 |
| [GdsMapControl_Label.cs](../../06_CoreLibSource/GdsMapControl/Controls/GdsMapControl_Label.cs) | TEXT 위치와 확대 배율에 따른 Label 표시 |
| [GdsMapControl_Layer.cs](../../06_CoreLibSource/GdsMapControl/Controls/GdsMapControl_Layer.cs) | Layer 표시와 색상 |
| [GdsMapTestForm.cs](../../06_CoreLibSource/GdsMapControl/GdsMapTestForm.cs) | 파일 읽기 / Grid / CSV / 상태 표시 |
| [frmGdsMapSetup.cs](../../02_DMS/DACrux.SEMDMS.ENGUI/GdsMap/frmGdsMapSetup.cs) | DMS 메뉴에서 GDS Form을 로드하는 기존 호스트 |

## 현재 구현과 중요한 로직

- 파싱한 Structure를 Flatten하여 배치 도형을 만들고 GPU 버퍼에 올려 그린다. SREF 재귀 호출과 변환 행렬 처리 경로가 있다.
- 현재 `FlattenStructure`에는 AREF를 펼치는 분기가 없다. AREF 클래스 / 파싱 지원을 화면 배치 지원과 혼동하지 않는다.
- Label은 이웃 간격의 `0.4`를 활용하고 최대 글꼴 크기는 `64px`다. 확대 시 글꼴 크기와 표시 개수는 실제 화면에서 확인해야 한다.
- `ShowOriginCross`로 원점 십자 표시를 제어한다.
- Grid는 `GridNumber`를 화면 번호로 사용하며 셀 선택 / 복사 / UTF-8 BOM CSV 내보내기 코드가 있다. `Excel Export (CSV)`는 `.xlsx` 생성 기능이 아니다.
- StatusStrip에는 읽기 진행과 Flatten / 정점 생성 / GPU 업로드 등 측정 결과를 표시한다. 비교할 때 같은 GDS / Layer / 화면 크기 / EXE를 사용한다.
- 기존 DMS 호스트는 `Application.StartupPath/GdsMap/NexplantQMS.GdsMap.exe`를 `Assembly.LoadFrom`으로 읽어 Form을 붙인다. 직접 프로젝트 참조와 다르며 실제 CLR / DLL 배포 호환성을 별도로 확인한다.
- 호스트는 `GdsMapTestForm(bool hostedByDms)` 생성자에 `true`를 넘긴다(`Activator.CreateInstance(formType, new object[] { true })`). 이때 시험용 가상 결함 2개를 그리지 않는다. 예전 직접 DB 버튼 `btnSave` / `btnLoad`와 `Oracle/OracleLoadUtil` / `OracleSaveUtil`(Middleware 없이 GDS_ELEMENT 직접 접속)은 2026-10-02 사용자 지시로 삭제했다. 화면 클래스 이름은 미커밋 변경으로 `GdsMapTestForm` -> `GdsMapForm`. 단독 실행(기본 생성자)은 기존 동작을 유지한다. 이 생성자는 DMS 내부 폴더에만 있고 루트 `GdsMapControl`에는 없다. (2026-10-02 Claude 소스 확인)
- **메모리 제한 (2026-10-02 Claude):** DMS 호스트 `DACruxV5`는 Debug 구성이 x86(32비트)이었다(같은 날 사용자가 `Debug|AnyCPU`를 AnyCPU로 변경, 이후 64비트 실행 예정). x86일 때는 GDS 화면도 32비트 메모리(약 2GB, 큰 연속 배열은 그보다 작음) 안에서 실행된다. 정점 하나가 28바이트(float 7개)이므로 `_gpuVertices`를 미리 크게 잡거나 `ToArray()`로 전체를 복사하면 OutOfMemory가 난다. 현재 `_gpuVertices`는 기본 용량으로 시작하고, `UploadAllGpuVertices`는 VBO 크기만 먼저 잡은 뒤 8192개씩 나눠 전송한다. 대형 GDS(예: 과거 측정 4,300만 정점 = 약 1.2GB)는 이 수정 후에도 32비트 호스트에서 열리지 않을 수 있다. 단독 실행 EXE는 AnyCPU / Prefer32Bit=false라 64비트로 실행된다.
- 호스트 메뉴: `MNU_DMS_GDS_MAP_SETUP` / GDS Map 설정 / `DACrux.SEMDMS.ENGUI.frmGdsMapSetup`. DMS에서 GDS 변경을 확인하려면 새 EXE를 `100_ClientBin/GdsMap`에 복사해야 한다. [빌드 / 검증 가이드](../빌드_검증_가이드.md) 참고.

## 개발 흐름과 확인 위치

| 단계 | 설명 | 변경할 때 확인할 내용 |
| --- | --- | --- |
| 파일 읽기 | GdsReader가 Library / Structure / Element를 구성 | 원본 단위 / Top Structure / 중복과 참조 |
| 배치 펼치기 | FlattenStructure가 SREF 계층과 변환을 적용 | 반복 참조의 Source / Placed ID 구분 / AREF 미지원 |
| Scene 구성 | 도형을 월드 좌표와 Layer별 표시 데이터로 구성 | 좌표 정밀도 / 도형 종류 / PATH 폭과 타입 |
| 화면 그리기 | 정점 생성 -> GPU 업로드 -> Draw | 각 단계 시간 / 메모리 / 실제 화면 |
| Label / 원점 | 도형과 별도로 TEXT와 원점 표시 | 확대 / 이동 / Layer 색상 / 표시 개수 |
| Grid | 파서 원본 정보를 행으로 표시 | 원본 행과 반복 배치 도형 수 차이 / 표시 순서 |
| Chain / 저장 | Map의 배치 도형을 공통 참조 | Grid 번호를 영구 키로 사용하지 않기 |

성능 변경은 기존 `GdsMapLoadMetrics`, `RenderProgressChanged`, `FirstFrameMeasured`로 처리 구간을 구분해 기록한다. 화면에 나온 총 시간만으로 파서 / Flatten / GPU 중 병목을 단정하지 않는다. 실제 출력 EXE가 `DMS/100_ClientBin/GdsMap`인지 독립 Debug 폴더인지 반드시 남긴다.

## 검증과 미완료

이번에는 소스만 읽었다. 현재 EXE 실행 / OpenGL 표시 / 복사 붙여넣기 / CSV 열기는 미확인이다. 과거 빌드나 Probe 기록을 현재 DMS 내부 프로젝트 실행 결과로 옮기지 않는다.

다음 확인 순서: 소스와 EXE 대응 확인 -> 대표 GDS 열기 -> Layer 표시 / 색상 -> Label 확대 / 이동 -> 원점 표시 -> Grid 복사 / CSV -> 같은 조건의 성능 측정.

변경 시 이 문서와 [작업 현황](../작업_현황.md), 당일 변경 이력을 함께 갱신한다.


## 확인 필요 (2026-10-02 Claude 발견)

샘플 `OMM all layer2.gds`에서 TEXT 30,992건 중 30,990건이 `GdsLayerList.DuplicatedItems`(파서 중복)로 분류된다. 중복 판단은 `GdsLayerList.AddElement`의 `BinarySearch`(GdsElement.CompareTo) 결과다. TEXT가 화면 Layer 목록에서 빠지는 것인지, 비교 기준이 의도와 맞는지 확인이 필요하다. 이번에는 변경하지 않았다.

-> 2026-10-15 확인(CLAUDE-120): KLayout 0.30.12 로 같은 파일을 펼쳐 대조했다. 빠진 31,890개(L775 TEXT 30,990 / PATH 899 / BOUNDARY 1)는 Layer / datatype / 좌표 / 폭 / 문자열까지 같은 복사본이라 잃은 도형은 없다. 다만 비교 기준이 Bounds 까지라 다른 내용(같은 원점의 다른 SREF 등)도 빠질 수 있어 내용까지 비교하도록 고쳤다(아래 "계층 GDS(SREF) 펼치기 규칙").


## 2026-10-02 / DB 저장 화면 (CLAUDE-012)

DMS 호스트 모드에서 상단에 Factory / Device / Revision 입력과 `DB 저장` / `저장 중지` 버튼이 보인다(단독 실행은 숨김). 로직은 `GdsMapForm_DbSave.cs`, 상세는 [GDS Map DB 저장 설계 18절](../../../문서/설계/02_GDS/2026-10-02_GDS_Map_DB저장_설계.md).


**2026-10-06 Layer 목록 도형 수 표시(CLAUDE-022):** GDS Map Setup의 Layer 목록이 `Layer 1 (15,866)`처럼 Layer별 화면 배치 도형 수(참조 펼친 인스턴스 포함, DB 저장 시 PLACED 행 수와 같음)를 표시한다. 목록 위 `lblLayerSummary`에 `선택 3 / 9 Layer · 도형 29,871 / 605,080` 합계를 체크 변경 때마다 갱신한다. 개수는 도면을 읽을 때 `GdsMapControl.GetLayerElementCounts()`로 한 번 만든다. 조회 화면(GdsMapViewForm)은 기존부터 DB 건수를 표시한다(변경 없음).

**2026-10-06 GDS 반전(Back side) 설계(CLAUDE-027, 설계만):** 회의 결정으로 GDS(Front side 원본)를 반전해 Back side 형태로 Map / Chain을 구성한다. [GDS 반전 설계](../../../문서/설계/02_GDS/2026-10-06_GDS_반전_BackSide_설계.md). 사용자 결정 대기.

**2026-10-07 Label 검색 설계(CLAUDE-032, 설계만):** View에서 Label 값(예: `C4DC_212_71`)으로 위치를 찾는다. 속성 기준 = TEXT 도형 전체(Layer 무관). 샘플 TEXT 30,992개(Layer 775). [Label 검색 설계](../../../문서/설계/02_GDS/2026-10-07_GDS_Label_검색_설계.md).

**2026-10-07 Factory 기준(CLAUDE-033, 사용자 지시):** GDS Map Setup / View 화면은 Factory를 고르거나 입력하지 않고 DACrux 로그인 Factory(`DACrux.Base.GlobalVariable.Factory`, `DmsHostContext.Factory`)를 쓴다. View Device 목록은 그 Factory의 Map만, Setup 저장 Factory도 그 값. 로그인 Factory가 없으면(단독 실행) DB 기능을 막는다. Probe `GdsFactoryContextProbe.ps1`.

**2026-10-07 Label 검색(CLAUDE-032):** View 화면 조회 조건 아래 Label 검색 줄. [Label 검색 설계](../../../문서/설계/02_GDS/2026-10-07_GDS_Label_검색_설계.md) 9절. **2026-10-15부터 화면에서 숨김**(아래 레이아웃 절 CLAUDE-122).

**2026-10-07 Label 크기 보정(CLAUDE-032 결정 A):** 한쪽 방향에만 이웃 TEXT가 있는 Label은 그 간격을 다른 방향 제한에도 사용(`CalculateTextLabelDisplayAreas`). 이웃 없는 방향이 화면 크기 제한이 되어 확대 시 64px까지 커지던 문제(예: `C4DC_HBM0_N_FH`) 수정.

**2026-10-20 U8 Label 표시 크기 설계(CLAUDE-162, 설계만):** U8 은 Bump 한 자리에 Label 4 ~ 6개(핀 번호 TEXTTYPE 3 + Net 이름 4, 2벌)가 겹쳐 같은 좌표 3개 이상이면 간격 0 -> 화면 크기 기준 -> 64px(69,084개)이 된다. 같은 좌표 건너뛰기(D1) / 사본 1개(D2) / 같은 자리 줄 나눔(D3) / 대표 간격 상한(D4) / 이름 길이 차이 2배 이내(D5) 설계, OMM 은 시뮬레이션 픽셀 차이 0. D3 / D5 사용자 결정 대기, 구현은 CLAUDE-163 뒤. [U8 Label 표시 크기 설계](../../../문서/설계/02_GDS/2026-10-20_U8_Label_표시크기_설계.md).

## GDS Map 조회 화면 레이아웃 (2026-10-12 CLAUDE-056)

- 위 도구 막대 한 줄 / Chain 작업 줄(도면 조회 뒤) / 오른쪽 탭 [Chain] [Layer] / 도면 우클릭 메뉴. Map 정보 Label 제거(Device 콤보 툴팁).
- 상세: `문서/설계/02_GDS/2026-10-02_GDS_Map_조회화면_설계.md` 16절.
- 2026-10-12 CLAUDE-057: Label 검색 포함 고정(콤보 제거) / 우클릭 = 전체 보기만 / Output 지정 숨김 / 경로 추가·제외 오른쪽 끝 / 성능 확인용 표시 제거 (설계 16.1절).
- 2026-10-13 CLAUDE-071: 상태 문구는 아래 상태 줄 한 곳(Chain 안내 줄 / Label 결과 글자 제거), 계산·확인용 값 제거, Chain 목록 색 코드 대신 색 견본 (설계 17절).
- 2026-10-15 Label 검색 줄 숨김(VS 디자이너에서 변경, 10-15 사용자가 의도한 변경으로 확인, CLAUDE-122): `GdsMapViewForm.Designer.cs` 의 `sepTool2` / `lblLabel` / `txtLabel` / `btnLabelSearch` / `btnLabelAll` / `cboLabelResults` / `sepTool3` 가 `Visible = false` 이고, 다시 보이게 하는 코드는 없다. 검색 코드(`GdsMapViewForm_LabelSearch.cs`)와 색인 조회는 남아 있다.
  - 지금 도구 막대 = Device / 조회 / 중지 / 보기 / [Map 복사](호스트) / [Map ▾]. 한 줄 폭 686px(96 DPI, 비호스트 605px). 이 폭보다 좁으면 [Map ▾] 부터 다음 줄로 넘어간다.
  - `GdsViewLayoutSmoke` 는 좁은 창 폭을 고정 900px 대신 막대 한 줄 폭 - 40px 로 계산해 줄바꿈 / 잘림을 보고, Label 검색 줄 숨김(도면 후)도 확인한다 -> PASS 28 (설계 18절).
  - `GdsLabelSearchProbe` 는 컨트롤 표시 여부를 보지 않으므로 숨긴 상태에서도 PASS 한다.

**2026-10-13 Setup Layer 목록 체크는 더블클릭(CLAUDE-080):** GDS Map Setup Layer 목록은 한 번 클릭 = 선택만, 더블클릭 = 체크 토글. [전체 체크] / [전체 미체크] / 우클릭 색 변경 대상 선택은 그대로. Smoke `GdsChainListDoubleClickSmoke.ps1`.

**2026-10-15 Setup Layer 버튼 Click 연결 복구(CLAUDE-106):** 10-14 VS 디자이너에서 Layer 영역을 분할 창 오른쪽으로 옮길 때(커밋 be513d5) [전체 체크] / [전체 미체크] / [Redraw] 의 Click 연결 3줄이 빠져 버튼이 동작하지 않았다. `GdsMapForm.Designer.cs` 에 3줄만 되돌림(더블클릭 체크 방식은 유지). Smoke 는 처리 함수 직접 호출 대신 실제 버튼 `PerformClick` 으로 확인(Redraw 검사 추가) -> 운영 EXE(10-14 17:12) FAIL 3 / 고친 TEMP EXE `GDS_CHAIN_LIST_DBLCLICK_SMOKE=PASS (30)`. 배포 안 함.

**2026-10-20 [Layer 적용] 버튼 제거 반영(CLAUDE-161):** 10-19 VS 에서 GDS Map Setup 의 [Layer 적용](`btnLayer`, 예전 [Redraw]) 버튼이 빠졌다(Designer / resx / `.cs` Enabled 줄 / `panel2` 높이 139 -> 110, 처리 함수 `btnLayer_Click` 만 연결 없이 남음). Layer 체크(더블클릭 / [전체 체크] / [전체 미체크])는 바뀔 때마다 바로 도면에 반영되므로 남은 버튼은 2개다. Smoke `GdsChainListDoubleClickSmoke.ps1` 은 이 버튼을 찾다가 RunSetup NullReference 로 끝났다 -> [Redraw] 검사를 빼고 필수 필드가 없으면 이름을 알려 주게 고침. 운영 EXE / TEMP 빌드 `GDS_CHAIN_LIST_DBLCLICK_SMOKE=PASS (28)`.

## 계층 GDS(SREF) 펼치기 규칙 (2026-10-15 CLAUDE-120)

- **행렬 순서**: `combined = m × parent` (`parent.Clone()` + `Multiply(m, Prepend)`). `System.Drawing.Matrix` 는 점을 행벡터로 바꾸므로 자식 SREF 배치(m)를 먼저, 부모 배치를 나중에 적용한다.
  - 예전 `Append`(parent × m)는 2단 이상 중첩에 회전 / 반전 / 배율이 섞이면 틀렸다.
  - 예: 부모 회전 90 @ (1000, 2000) + 자식 이동 (100, 0) -> 예전 (1100, 2000), 올바름 (1000, 2100).
- **PATH 폭** = 원본 폭 × 누적 SREF 배율. `FlattenStructure` 인자 `parentMagnification`(double)로 넘긴다. float 행렬에서 거꾸로 구하지 않는 이유는 회전만 있어도 폭이 미세하게 바뀌기 때문이다.
  - 음수 폭(GDS 절대 폭)은 배율을 적용하지 않는다.
  - 저장 `MIN_X ~ MAX_Y`(꼭짓점 ± 폭 / 2)도 이 폭으로 계산된다.
- **같은 셀 반복 배치**: `GlSceneItem.CompareTo` = 원본 요소 비교 후 같으면 배치 ID(`PlacedElementId`) 비교.
  - 예전에는 원본만 비교해 `GlSceneLayerList.AddSceneItem` 이 음수 위치에 넣다 예외가 났다(같은 셀을 두 번 배치한 GDS 를 열 수 없었음).
  - 선택 목록(`_selectedItems` BinarySearch)도 같은 셀의 다른 배치를 같은 도형으로 봤다.
  - `AddSceneItem` 은 같은 순서 값이 있어도 그 자리에 넣는다.
- **읽기 중복 제외**: `GdsElement.CompareTo` 가 Layer -> 종류 -> Bounds 가 같으면 내용까지 비교한다.
  - 내용 = DataType / 변환 / SREF 구조 이름·원점 / AREF 구조·행열·벡터 / PATH 폭·종류·점 / BOUNDARY 점 / TEXT 문자열·종류·글꼴·기준점·위치.
  - 내용까지 같은 요소만 `DuplicatedItems` 로 뺀다. 예전에는 같은 원점의 다른 SREF, 같은 위치의 다른 문자열 TEXT, Bounds 가 같은 다른 모양도 빠졌다.
  - Bounds 가 다른 요소의 순서는 그대로라 기존 배치 ID(`/E` 번호)는 바뀌지 않는다(샘플로 확인).
- **실제 샘플 / 저장 Map 영향 없음**: `OMM all layer2.gds` 는 구조 1개, SREF 0 / AREF 0. 고친 뒤에도 도형 605,080 / 읽기 중복 31,890 / 배치 ID·좌표·폭·Bounds·문자열 SHA-256 이 변경 전 빌드와 같다. DB 의 Map 9개(모두 이 파일 TOP)는 재저장하지 않는다.
- **검증**: `Probes/GdsSrefTransformProbe.ps1 -GdsExe <EXE> [-SampleGds <경로>]` -> `GDS_SREF_TRANSFORM_PROBE=PASS (8)`. 합성 GDS 3개를 스크립트가 만들고 KLayout 0.30.12 로 펼친 좌표 / 폭과 대조한다. 변경 전 빌드(10-15 15:12)는 FAIL 5.
- **남은 것**: AREF 미전개(저장 차단 유지), 행렬 float 계산(계층 파일에서만 약 0.002 µm, 허용오차 5 µm 대비 무시).

## GDS 단위 (2026-10-16 CLAUDE-125 / 126)

- 파일: UNITS = DB 단위 1개 1e-9 m(1 nm), 사용자 단위 1 µm(DB 단위 1,000개). 좌표 / PATH 폭은 정수(nm).
- 읽기: `GdsReader { LengthUnit = Micrometer }` 가 정수 x `DatabaseUnit`(0.001) 로 µm double 을 만든다. DB `TQP_GDS_PLACED_EL` MIN_X ~ MAX_Y / PATH_WIDTH / POINTS_BIN 은 µm.
- `TQP_GDS_MAP.USER_UNIT / DATABASE_UNIT` = 파일 UNITS 원값(0.001 / 1E-9). 2026-10-16 수정 전에는 화면 저장이 µm 환산 뒤 값(1E+3 / 1E-3)을 넣었다. 지금은 `GdsLibrary.FileUserUnit / FileDatabaseUnit`(파일 원값)을 보낸다(`GdsMapForm_DbSave`, `GdsMapDbRestoreProbe`). 로컬 DB 기존 7행(SCK 6 + PROBE VIEW_PROBE)은 1E-3 / 1E-9 로 UPDATE 했다.
- 이 두 컬럼은 저장 / 복사에만 쓰이고 좌표 계산에는 쓰지 않는다. 회귀 `GDS_SREF_TRANSFORM_PROBE=PASS (8)`(샘플 배치 결과 SHA-256 같음).

**2026-10-20 U8 GDS 로딩 차이(CLAUDE-158, 분석 / 설계만):** Setup 화면은 파일의 첫 Structure 를 그린다(`GdsMapControl.ShowStructure(lib)` -> `lib.Structures[0]`). U8 파일은 첫 Structure 가 하위 Cell(`U8_PIC_C4`)이라 KLayout(최상위 Cell 기준)과 다르게 보인다. 제품 최상위 Cell(`OMM_PFIC_PIC_PKG_A0`, 펼친 BOUNDARY 161만)은 `BuildGpuBuffers` 정점 목록 1개가 2GB 한도를 넘어 열리지 않는다. 설계: `문서/설계/02_GDS/2026-10-20_U8_GDS_로딩_원인분석_설계.md`(최상위 Cell 선택 / 펼치기 전 Layer 선택 / GPU 정점 구조 개선).

**2026-10-20 U8 대응 구현(CLAUDE-158):** 파일을 열면 펼친 도형이 가장 많은 최상위 Cell 을 그린다(`GdsLibrary.GetTopStructures`), Setup 화면 [Cell] 목록으로 다른 최상위 Cell(예 U8 `TOP`)을 골라 펼치고 그 Cell 로 DB 저장. 그리기 버퍼는 Layer 별 VAO / VBO / EBO + 12바이트 정점 + 인덱스 채우기 + Layer 일괄 그리기(`LayerGpuBuffer`). 평행 이동은 GDI+ 와 비트 단위로 같은 직접 계산, 펼친 뒤 한 번 정렬 -> 펼친 결과(배치 ID / 좌표) 해시가 이전과 같다(`Probes/GdsPlacedHashProbe.ps1`). U8 제품 Cell 18초 / 다시 그리기 0.5~1초(남은 과제).

**2026-10-20 Layer = 번호 + DataType(CLAUDE-163):** KLayout 처럼 Layer 를 (번호, DataType)으로 나눈다. TEXT 는 TEXTTYPE 을 DataType 자리에 쓴다(U8 Layer 18 = `18/0` 도형 / `18/3` 핀 번호 Label / `18/4` Net 이름 Label). 키 `Gds/GdsLayerKey.cs`(번호 x 65536 + DataType, 정수 순서 = 번호 -> DataType 순서), 화면 표시 `Layer 18/3`. Layer 목록 / 색 / 표시 / 도형 수 / GPU Layer 버퍼 / Label 표시 / 저장 필터가 키를 쓰고, `GlSceneItem.LayerID` / `MapPlacedElementData.LayerId` 는 GDS 번호 그대로. 기본 색은 번호 색에서 DataType 만큼 이동. U8 제품 Cell Layer 17개가 KLayout 목록과 같다(합계 1,817,538 그대로). Probe 해시 기준값은 방문 순서 / TEXT DataType 만 바뀌어 새 값으로 바꿈. 설계 `문서/설계/02_GDS/2026-10-20_U8_GDS_로딩_원인분석_설계.md` 9절. 배포(GdsMap EXE, 서버 / DDL 과 함께) / DMS 화면 확인 대기.
