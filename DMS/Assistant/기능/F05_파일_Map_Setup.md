# F05 / 파일 Map Setup

확인일: 2026-10-06 / 상태: 입력을 KLARF 로 변경 / KLARF 기준 설계 확정 / 구현 2단계 완료 (화면이 KLARF 로 원형 생성, Probe / 화면 검사 PASS, 미배포 / DMS 화면 확인 대기)

## 2026-10-06 KLARF 기준으로 변경 (CLAUDE-025)

- 사용자 결정: "klarf 로 하자" (이후 GDS <-> KLARF Overlap 기준을 맞추기 위함). 설계: [KLARF 기준 Map Setup 설계](../../../문서/2026-10-06_KLARF_Map_Setup_설계.md). 좌표 / 격자 / 입력은 이 설계가 10/02 설계보다 우선한다.
- 확인 근거: 로컬 DB `TQP_MAPDEF` 402건 모두 µm / LeftTop / ANGLE 0. DMS 는 KLARF 원본 인덱스를 `TQD_SETUP.DIE_ORIGIN` 또는 `WaferDieCalculator` 값만큼 옮겨 좌하단 (1,1) 로 저장한다. 샘플 옮기는 양 (7,5), 격자 X 1~12 / Y 1~9, 검사 Die 26개 모두 격자 안 (음수 인덱스 없음).
- Notch 각도는 `Klarf/ParserKlarf.GetAngle()` (OrientationMarkLocation -> DOWN 0 / LEFT 90 / UP 180 / RIGHT 270) 을 그대로 쓴다. (처음 "버그" 로 적은 것은 빌드 제외된 루트 `ParserKlarf.cs` 를 읽은 착오, 정정함)
- 구현 1단계 (2026-10-06): `Admin/KlarfMapSource.cs` (FileAnalyzer -> ParserKlarf / _18, 원점 조회 함수를 받으면 읽는 동안만 `ParserKlarf.GetOriginDieIndex` 에 넣고 되돌림, Wafer 별 Center / 옮긴 양 / 검사 Die / Defect) + `Admin/KlarfMapRule.cs` (`CalculateGrid`: WaferDieCalculator 격자를 Parser 옮긴 양 기준으로 맞춤, Edge 는 격자에 넣지 않음 / `ToDirection`·`FromDirection`: 형상 유지 번호 변환). 검증 `Verification/Verify-KlarfMapRule.ps1` -> `KLARF_MAP_PROBE=PASS` (샘플 Shift (7,5) / 격자 X 1~12 Y 1~9 / 검사 Die 26 = SINF 26 / Defect 109 모두 검사 Die 안 / 물리 좌표식 일치 / 방향 4종 왕복).
- 구현 2단계 (2026-10-06): 화면 입력을 KLARF 로 교체 (SINF `StandardMapFile` 은 파일 / csproj 에 남기고 화면에서 사용 안 함). `GenerateCircleMap` = `CalculateGrid` -> `MapOriginIndex` (방향별) -> Map 설정 -> Redraw -> DieClear -> 격자 전체 `AddDie` -> `StandardMapEditMap.IsInsideWafer` (protected `UseDie` 재사용) 로 1 / -1. `DieCalculation(true)` 는 쓰지 않는다: LeftBottom 등에서 `AddDie` 와 Die 위치가 한 칸 다르다 (Core 코드 확인). `AddDie` 는 방향마다 원점 Die 모서리가 달라 `MapOriginIndex` 로 맞춘다 (LeftTop 은 X 가 s-1). 화면: KLARF Wafer 콤보 / Wafer 크기(기본 SampleSize) / Edge(µm) / XY 방향(기본 LeftBottom) / 원점(µm, SampleCenterLocation) / [원형 생성] / 정보창 / 좌표(선택 방향 번호 / LeftBottom / KLARF 원본 / 검사 Die 여부). 설정 변경은 [원형 생성] 으로 반영, Die 편집 후 다시 생성하면 확인창. 원점 조회(TQD_SETUP)는 아직 null (4단계).
- 검증: `Verification/Verify-KlarfMapSetupForm.ps1` -> `KLARF_FORM_SMOKE=PASS` (실제 폼 / Map 컨트롤, 샘플 300 mm: 격자 108 / 사용 79 / 방향 4종 모두 Die 좌하단 = KLARF 물리 위치, 사용 Die 수 같음, 검사 Die 26 모두 사용 / USECODE 1·-1 / 좌표 표시 KLARF 원본 0,0 / Edge 5000 -> 76 / Wafer 200 -> 40 중 28 / 경로 변경 시 저장 막힘). 기존 `Verify-StandardMapSetup.ps1` 은 SINF 화면용이라 더 이상 맞지 않음 (빌드 단계에서도 SEMDMS.Interface 복사 누락).
- 남은 범위: 3단계 편집 모드(보기 / 제거 / 복원, 기본 EditMode 가 ADD 라 지금은 클릭하면 원 밖 Die 가 살아남) + 보조 버튼 2개 / 4단계 TQD_SETUP 조회 / 5단계 저장 후 Map Define 에서 확인.
- 저장 규칙 (2026-10-06, Map Define 불러오기 실패 원인 수정): EDGE_SIZE 는 mm (기존 데이터 2.5~5, Map Define 입력칸 최대 5) -> 화면 µm / 1000, 입력 최대 5000 µm. ORIGIN_MICRO_Y 는 DieSizeY - OriginY 로 저장 (조회 쿼리 GET_MAPDEF 등이 CHIP_SIZE_Y - 값 으로 되돌림). `MapCreationService.ToStoredValues`. Shot Map View 는 DELETE_FLAG N Map 의 Die 를 `TQP_MAPDEF_DIE` 에서만 읽는다 (이 테이블 INSERT 코드는 소스에 없음, Map Define 으로 만든 Map 도 표시 안 됨) - 사용자 결정 2026-10-06: 지원 안 함.
- 저장 (2026-10-06): 같은 Map ID 가 있으면 덮어쓰기 확인 후 `MapCreationService.Update` (UpdateMapDef + CreateUseMap, Map Define Save As 덮어쓰기와 같은 서비스). DELETE_FLAG = Y 인 ID 는 덮어쓰지 않음.
- 개수 표시 (2026-10-07): Map 위 `lblDieCount` 에 Net Die (사용 Die = 저장 NETDIE) / 생성 Die (격자 전체) / 제거 / 기타 / 검사 Die 중 사용.
- 사용자 결정 (2026-10-06): XY 방향 작업자 선택 + 기본 LeftBottom / 옮기는 양은 TQD_SETUP 조회 우선 + 없으면 계산기 (Interface / RO 함수 추가) / SINF txt 지원 중지. GDS 도 µm (샘플 UNITS DB 1 nm / User 1 µm).


## 목적과 파일

고객 텍스트 Map을 읽어 Die 위치를 확인하고 기존 Map 저장 서비스를 이용해 새 Map ID를 만든다.

| 파일 | 역할 |
| --- | --- |
| [frmStandardMapSetup.cs](../../09_TEST/DACrux.TEST.ENGUI/Admin/frmStandardMapSetup.cs) / [Designer](../../09_TEST/DACrux.TEST.ENGUI/Admin/frmStandardMapSetup.Designer.cs) | 화면 / 미리보기 / 좌표 표시 / 저장 |
| [StandardMapFile.cs](../../09_TEST/DACrux.TEST.ENGUI/Admin/StandardMapFile.cs) | 헤더와 RowData 파싱 / 형식 검사 |
| [MapCreationService.cs](../../09_TEST/DACrux.TEST.ENGUI/Admin/MapCreationService.cs) | 기존 ProbeAdmin 저장 호출 / X,Y,USECODE 변환 |
| [Verify-StandardMapSetup.ps1](../../09_TEST/DACrux.TEST.ENGUI/Verification/Verify-StandardMapSetup.ps1) | 별도 출력 위치 빌드 / 파서 / Form 검사 |
| [원형 생성 설계](../../../문서/2026-10-02_파일Map_Setup_원형생성_설계.md) | 사용자 결정 / 원형 격자 / 편집 / 구현 순서 |

## 현재 소스에서 확인한 동작

- `StandardMapFile.Load`로 헤더와 RowData를 읽고 숫자 셀을 Die로 만든다. 밑줄 셀은 빈 위치다.
- 파서는 첫 RowData의 왼쪽을 `(1,1)`로 만든다. 현재 화면은 `XYDirection.LeftTop`이다. 9/30 문서의 LeftBottom 설명은 현재와 다르다.
- `DrawFileMap`, `CalculateFitScale`, `SnapshotDies`, `RebuildDies`가 존재한다. 현재는 파일 Die 표시와 Wafer 맞춤 배율을 사용하는 단계다.
- `RebuildDies`는 현재 DieProp을 보존하며 바뀐 크기에 맞춰 Die를 다시 넣는다. 파일 CenterDie의 선택 테두리도 설정한다.
- 마우스 Die 정보는 `OnChangeCurrentDie` 이벤트에서 처리한다. FNLOC를 좌표 회전 규칙으로 임의 해석하지 않는다.
- 새 Map 저장은 `MapCreationService.Create` -> `SetMapDef` -> `CreateUseMap` 순서다. X/Y/USECODE는 현재 `DieProp`을 옮긴다.
- 두 원격 호출이 하나의 트랜잭션은 아니다. 정의만 저장된 부분 실패 여부를 실제 저장 시험에서 확인해야 한다.
- 기존 `frmSetupMap`은 현재 자체 저장 로직을 유지한다. 공통 서비스라는 과거 주석만 보고 기존 화면을 다시 수정하지 않는다.

## 확정 설계 / 아직 미구현

10/02 설계서는 다음 방식으로 바꾸기로 기록되어 있다. `StandardMapCircleRule.cs`, `StandardMapEditMap.cs`와 원형 생성 동작은 이번 조사 시 현재 구현으로 확인되지 않았다.

1. 실제 Die 크기를 사용하고 작업자가 Wafer 크기 / Edge / 방향을 선택한다.
2. 생성 버튼으로 원 안 전체 격자를 만든다. 필요하면 파일 범위 밖 음수 / 0 / 초과 인덱스까지 대칭 확장한다.
3. XY 방향 변경 시 형상은 유지하고 번호를 변환한다. 기본은 LeftTop이다.
4. 불필요한 Die는 제거 / 복원하고 제거 Die도 `USECODE=-1`로 저장한다.
5. 기존 Map 컨트롤은 수정하지 않고 화면 전용 상속과 계산 규칙을 사용한다.

과거의 빈 원점 표시 요구와 이번 원형 생성 설계는 구분한다. 새 설계를 구현할 때 현재 원점 / 중심 Die 표시를 어떻게 이어갈지 설계서와 함께 확인한다.

## 검증 명령과 확인 범위

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File 'D:\1_프로젝트 문서\18.스테츠칩팩_POC\ChippacPOC\DMS\09_TEST\DACrux.TEST.ENGUI\Verification\Verify-StandardMapSetup.ps1'
```

스크립트는 TEMP의 `ChippacMapSetupVerification`을 사용하며 현재 ClientBin 의존 DLL이 필요하다. 빌드 로그 / Parser 결과 / FormSmoke 결과 / `form-preview.png`를 확인한다. DB 저장은 검사하지 않는다. 이번 작업에서 이 명령은 실행하지 않았다.

다음 구현은 설계서의 좌표 / 격자 계산과 파서 데이터 확장부터 진행한다. 원형 생성 이후에는 음수 인덱스 / 방향 4종 / 제거 저장 / 저장 후 다시 열기를 별도로 확인한다. Shot Map은 `TQP_MAPDEF_DIE` 경로이므로 `TQP_USEMAP` 저장만으로 동일 표시를 보장하지 않는다.
