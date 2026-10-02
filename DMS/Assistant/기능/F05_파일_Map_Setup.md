# F05 / 파일 Map Setup

확인일: 2026-10-02 / 상태: 파일 배치 표시 코드 확인 / 원형 생성 설계는 구현 전

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
