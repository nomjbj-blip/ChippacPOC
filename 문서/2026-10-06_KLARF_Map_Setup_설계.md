# KLARF 기준 Map Setup 설계 (2026-10-06)

대상 화면: `DMS/09_TEST/DACrux.TEST.ENGUI/Admin/frmStandardMapSetup.cs` (메뉴 "File Map Setup", 신규 화면)
이전 설계: [2026-10-02 파일 Map Setup 원형 생성 설계](2026-10-02_파일Map_Setup_원형생성_설계.md) - 입력 파일을 SINF txt 에서 KLARF 로 바꾸며 이 문서가 우선한다. 원형 생성 / 작업자 제거 / 보조 버튼 / USECODE=-1 / 화면 분리 원칙은 그대로 이어받는다.
작업 ID: CLAUDE-025 / 상태: 설계 확정 (2026-10-06 사용자 결정 3건 반영) / 구현 1~2단계 완료 (Probe / 화면 검사 PASS, DMS 화면 확인 대기)

---

## 1. 결정 배경

- 사용자 결정 (2026-10-06): "klarf 로 하자". 이유는 이후 GDS <-> KLARF Overlap 이다.
- SINF txt 는 KLARF 검사 Die 를 격자로 옮긴 파일이다. 샘플에서 `SampleTestPlan` 26개와 SINF RowData 26개가 정확히 같은 위치다 (`X = 열 - 6`, `Y = 4 - 행`).
- KLARF 만 Wafer 크기 / 정밀 Die Pitch / 실제 Wafer 중심 오프셋 / Notch 방향을 준다. SINF 는 이 값이 없어서 가정해야 한다.

## 2. 확인한 근거 (2026-10-06)

| 항목 | 확인 내용 | 근거 |
| --- | --- | --- |
| 기존 Map 단위 | 로컬 DB `TQP_MAPDEF` 402건 전부 **µm** (WAFER_SIZE 200000, CHIP_SIZE 670~23306, 컬럼명 `ORIGIN_MICRO_X`) | docker oracle19c DMSMGR 조회 |
| 기존 Map 방향 | 402건 전부 `XY_DIRECTION = 0` (LeftTop), ANGLE 0 | 같은 조회 |
| 기존 0 / 음수 인덱스 | Map 정의 3건, Die 3건에 0 인덱스 존재 (드묾) | 같은 조회 |
| USECODE | 1 = 5,569,408건 / -1 = 2건 | 같은 조회 |
| KLARF Parser | `ParserKlarf` 가 SampleSize / DiePitch / DieOrigin / SampleCenterLocation / OrientationMarkLocation / SampleTestPlan 을 읽는다 | `06_CoreLibSource/DACrux.Data.Parser/Klarf/ParserKlarf.cs` |
| Defect 물리 좌표 | `X = XINDEX * PitchX + XREL - SampleCenterX` (Wafer 중심 기준 µm). 샘플 Defect 1번으로 계산 일치 | `Klarf/ParserKlarf.cs:255` |
| DMS 저장 인덱스 | Parser 가 원본 인덱스(가운데 Die = 0,0)를 **좌하단 첫 Die = (1,1)** 로 옮긴다. 옮기는 양은 `TQD_SETUP.DIE_ORIGIN_X/Y` (Setup / Step / SetupTime 별 저장값) 가 있으면 그 값, 없으면 `WaferDieCalculator` 계산값 | `Klarf/WaferList.cs:64` `UpdateDieIndex` / `DefectDefine.GetOriginDieIndex` / `TQD_SETUP.xml SELECT_ORIGIN_INDEX` |
| 기존 KLARF 화면 | `frmKlarfFileViewer` 가 Map 컨트롤에 WaferSize = SampleSize*1000 / DieSize = Pitch / Origin = SampleCenter / OriginIndex = 옮긴 양 / LeftBottom / `DieCalculation(true)` 로 그린다 | `02_DMS/.../frmKlarfFileViewer.cs:192~233` |
| 프로젝트 참조 | `DACrux.TEST.ENGUI` 는 이미 `DACrux.Data.Parser` 프로젝트를 참조한다 (.NET 4.0) | TEST.ENGUI csproj |

`TQP_MAPDEF` 는 TEST(ET) 쪽 Map 이다. DMS(Defect) 는 이 테이블을 쓰지 않고 `TQD_SETUP` 원점으로 인덱스를 정한다. 그래서 KLARF 로 `TQP_MAPDEF` 를 만들면 **ET Map 과 AOI Defect 가 같은 Die 격자 / 같은 인덱스**를 쓰게 된다. 이것이 GDS Overlap 의 공통 기준이 된다.

### 샘플 계산 결과 (`WaferDieCalculator` 를 같은 식으로 계산)

입력: Wafer 300 mm / Pitch 23,834 x 30,060.1 µm / SampleCenter (-29, 15,191) µm

```
옮기는 양(OriginIndex) = (7, 5)   -> 원본 (0,0) Die 가 Map (7,5)
격자 = X 1~12 / Y 1~9, 계산기 기준 Die 79개
Y=9 :         5 6 7 8
Y=8 :     3 4 5 6 7 8 9 10
Y=7 :   2 3 4 5 6 7 8 9 10 11
Y=6 : 1 2 3 4 5 6 7 8 9 10 11 12
Y=5 : 1 2 3 4 5 6 7 8 9 10 11 12      <- 원점 행
Y=4 : 1 2 3 4 5 6 7 8 9 10 11
Y=3 :   2 3 4 5 6 7 8 9 10 11
Y=2 :     3 4 5 6 7 8 9 10
Y=1 :         5 6 7 8
검사 Die 26개 -> X 2~11 / Y 2~8 로 옮겨지고 모두 격자 안
```

- 인덱스가 1부터 시작하므로 10/02 설계의 "음수 인덱스 저장" 위험이 없어진다.

## 3. Map 생성 Rule (KLARF -> Map 컨트롤 -> TQP_MAPDEF)

| Map 값 | 계산 | 샘플 |
| --- | --- | --- |
| 단위 | **µm** (기존 402건과 같음, KLARF 원값 그대로) | |
| WaferSize | `SampleSize * 1000`. 화면 콤보로 변경 가능 (기본 = 파일 값) | 300,000 |
| DieSizeX / Y | `DiePitch` | 23,834 / 30,060.1 |
| XYDirect | 작업자 선택, **기본 LeftBottom** (DMS 저장 인덱스와 같은 방향) - 8절 결정 1 | 1 |
| OriginIndexX / Y | DMS 와 같은 "옮기는 양" (TQD_SETUP 저장값 우선, 없으면 계산기) | (7, 5) |
| OriginX / Y | `SampleCenterLocation` | -29 / 15,191 |
| DieMin / Max | 계산기 `IndexXMin~Max` / `IndexYMin~Max` (Wafer 전체 격자) | X 1~12 / Y 1~9 |
| FirstDie | DieMin | (1, 1) |
| NotchType | `SampleOrientationMarkType` (NOTCH -> Notch, FLAT -> Flat) | Notch |
| NotchAngle | `parser.GetAngle()` (OrientationMarkLocation: DOWN 0 / LEFT 90 / UP 180 / RIGHT 270, KLARF File View 와 같음) | 0 (DOWN) |
| EdgeSize | 화면 입력, 기본 0 | 0 |
| 사용 Die | `DieCalculation(true)` 로 원 안 = 1, 원 밖 = -1 -> 작업자 제거 | |
| 파일 Die (보조 버튼 기준) | 모든 InspectionTest 의 `SampleTestPlan` 합집합 (옮긴 인덱스) | 26개 |

생성 순서는 10/02 설계와 같다: 설정 -> Redraw -> `DieCalculation(true)` -> Redraw (Notch 영역이 그리기 중 계산되기 때문).

## 4. 화면 설계 변경점 (10/02 설계 대비)

### 4.1 입력

- 파일 선택 필터를 KLARF(`*.000;*.klarf;*.KLAF;*.SINF` 등 + 모든 파일)로 바꾼다.
- 파일 분석은 `frmKlarfFileViewer` 와 같은 방식으로 Parser 를 고른다 (`FileAnalyzer` -> `ParserKlarf` / `ParserKlarf_18`). `ErrorFlag` / Wafer 0개 / Pitch 0 / SampleSize 0 이면 생성하지 않는다.
- 파일 안에 Wafer 가 여러 개면 Wafer 콤보로 고른다 (기본 첫 Wafer). Map 은 Wafer 공통이지만 SampleCenterLocation 은 Wafer 별 값이라 선택한 Wafer 값을 쓴다.
- Map ID 초기값 = `DeviceID`.

### 4.2 설정 / 생성 / 편집 / 저장

| 항목 | 10/02 설계 | KLARF 설계 |
| --- | --- | --- |
| Wafer 크기 | 작업자 선택 필수 | 파일 값 기본, 변경 가능 |
| Die 크기 | SINF mm 값 | KLARF µm 값 (화면 표시는 µm, mm 병기) |
| 원점 | 격자 중심 가정 (Rule 계산) | SampleCenterLocation 그대로. 입력칸은 표시용 + 미세 조정 |
| 격자 확장 | 파일 격자 대칭 확장 계산 | 계산기 결과가 이미 Wafer 전체 격자 (확장 계산 불필요) |
| XY 방향 | 선택 가능, 기본 LeftTop | 선택 가능, **기본 LeftBottom**. 형상은 그대로 두고 번호만 바꾼다: Right 계열 `X' = MinX + MaxX - X`, Top 계열 `Y' = MinY + MaxY - Y`. LeftBottom 이 아니면 DMS Defect 인덱스와 번호가 다르다는 안내를 정보창에 표시 |
| [파일 빈칸 제거] | SINF 밑줄 칸 | **[검사 Die 외 제거]**: SampleTestPlan 에 없는 Die 를 -1 로. 격자 전체가 대상 |
| [Wafer 밖 제거] | 그대로 | 그대로 (`StandardMapEditMap.IsInsideWafer`) |
| 저장 | `MapCreationService.Create` | 그대로. µm 값이 그대로 `SetMapDef` 로 간다 |

### 4.3 Notch 각도

- 빌드되는 Parser 는 `Klarf/ParserKlarf.cs` 다. `GetAngle()` 은 `OrientationMarkLocation` 을 `OrientationMark` enum (DOWN 0 / LEFT 90 / UP 180 / RIGHT 270) 으로 바꾼다. 이 값을 그대로 쓴다.
- 정정 (2026-10-06): 처음 작성 때 "GetAngle 이 Mark Type 을 비교하는 버그" 라고 적은 것은 빌드에 포함되지 않는 예전 파일 `DACrux.Data.Parser/ParserKlarf.cs` (루트) 를 읽은 착오였다. csproj 는 `Klarf\ParserKlarf.cs` 만 Compile 한다.
- 화면 확인은 구현 2단계에서 KLARF File View 와 Notch 위치를 나란히 비교한다.

### 4.4 마우스 정보 (GDS Overlap 준비)

`frmKlarfFileViewer` 의 마우스 정보와 같은 식으로 표시한다: Map 인덱스 / KLARF 원본 인덱스 (= Map 인덱스 - 옮기는 양) / Die 안 위치 (XREL, YREL µm). 이후 GDS 좌표 변환에 쓸 값이다.

## 5. 클래스 / 파일 구조

| 파일 | 구분 | 역할 |
| --- | --- | --- |
| `Admin/KlarfMapSource.cs` | 신규 | KLARF 파일을 읽어 Map 생성에 필요한 값만 모은다 (Wafer 목록 / Pitch / SampleSize / Center / Notch / 옮기는 양 / 검사 Die 집합). Parser 선택과 오류 검사 포함 |
| `Admin/KlarfMapRule.cs` | 신규 | 3절 계산만 하는 static class. 격자 범위 (`WaferDieCalculator`) / XY 방향 번호 변환. 화면과 분리해 Rule 만 따로 검증 |
| `Admin/StandardMapEditMap.cs` | 신규 (10/02 설계 그대로) | `EditWaferMap` 상속, protected `UseDie` 감싼 `IsInsideWafer` |
| `Admin/frmStandardMapSetup.cs` / `.Designer.cs` | 수정 (신규 화면) | KLARF 입력 / Wafer 콤보 / 생성 / 편집 / 보조 버튼 / 저장 |
| `Admin/StandardMapFile.cs` | 사용 중지 | SINF 파서. 바로 삭제하지 않고 csproj 에서 뺄지 사용자 확인 (8절 결정 3) |
| `Admin/MapCreationService.cs` | 변경 없음 | 저장 |
| 8절 결정 2 가 "TQD_SETUP 조회" 면 | Function 추가 | SEMDMS Interface / RO 에 `GetOriginDieIndex` 노출 (BSL / DSL / XML 은 이미 있음). 기존 함수 변경 없음 |
| 기존 화면 / Core | 변경 없음 | `frmKlarfFileViewer` / `ParserKlarf` / `WaferDieCalculator` 는 호출만 |

## 6. 단계별 구현 계획

각 단계는 빌드 + 검증 후 다음 단계. 화면 단계는 사용자 DMS 화면 확인까지.

1. `KlarfMapSource` / `KlarfMapRule` + Probe: 샘플에서 옮기는 양 (7,5) / 격자 X 1~12, Y 1~9 / 검사 Die 26개가 모두 격자 안 / Defect 109건이 검사 Die 안에 있는지 PASS.
2. `StandardMapEditMap` + 화면 생성 동작 교체 (SINF -> KLARF). 샘플로 원형 Map 표시 / Notch 위치 / 검사 Die 위치를 KLARF File View 와 나란히 비교.
3. 편집 모드 (보기 / 제거 / 복원) + 보조 버튼 2개 + 정보창.
4. (결정 2 가 조회면) Interface / RO `GetOriginDieIndex` 추가 + ServerBin 반영.
5. 저장 -> 기존 Map Define 화면에서 열어 형상 / 인덱스 / µm 값 확인.

## 7. 1차 범위 제외

- KLARF 1.8 형식 상세 검증 (Parser 는 있으나 샘플은 1.2).
- 같은 Device 의 여러 KLARF 를 합쳐 검사 Die 를 늘리는 기능.
- GDS Overlap 자체 (F07).

## 8. 사용자 결정 (2026-10-06 확정)

- 결정 1: XY 방향은 작업자가 선택, 기본 LeftBottom.
- 결정 2: 옮기는 양은 TQD_SETUP 저장값 조회 우선, 없으면 계산기 (SEMDMS Interface / RO 에 조회 함수 추가).
- 결정 3: SINF txt 지원 중지 (KLARF 만). SINF 코드는 파일만 남기고 화면에서 뺀다.
- 참고 (사용자 질문): GDS 도 µm 기준이다. 샘플 `OMM all layer2.gds` UNITS = DB 단위 1 nm / User 단위 1 µm (정수 좌표 x 0.001 = µm). KLARF / TQP_MAPDEF / GDS 가 모두 µm 로 맞는다.

### 결정 전 검토 내용

1. **XY 방향**: KLARF 기준 Map 은 DMS Defect 인덱스와 같도록 **LeftBottom 고정**을 권장한다. 10/02 결정 (선택 가능, 기본 LeftTop) 은 SINF 기준이었다. 기존 TEST Map 은 모두 LeftTop 이므로, ET 데이터가 LeftTop 인덱스로 들어오면 ET 쪽 변환이 필요해진다.
2. **옮기는 양의 출처**: DMS 와 100% 같게 하려면 `TQD_SETUP` 저장값을 먼저 조회해야 한다 (SEMDMS Interface / RO 함수 추가 필요). 권장: 조회 추가 (없으면 계산기). 대안: 계산기만 사용 (서비스 변경 없음, 이미 업로드된 Setup 과 다를 수 있음).
3. **SINF txt 지원**: 사용 중지 (KLARF 만) 권장. SINF 코드는 파일만 남기고 화면에서 뺀다.
