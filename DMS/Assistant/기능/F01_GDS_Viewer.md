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


## 2026-10-02 / DB 저장 화면 (CLAUDE-012)

DMS 호스트 모드에서 상단에 Factory / Device / Revision 입력과 `DB 저장` / `저장 중지` 버튼이 보인다(단독 실행은 숨김). 로직은 `GdsMapForm_DbSave.cs`, 상세는 [GDS Map DB 저장 설계 18절](../../../문서/2026-10-02_GDS_Map_DB저장_설계.md).
