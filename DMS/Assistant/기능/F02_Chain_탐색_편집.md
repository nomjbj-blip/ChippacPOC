# F02 / Chain 탐색과 편집

확인일: 2026-10-02 / 상태: 소스와 기존 분석 문서 확인 / 실행 재검증 필요

## 목적과 파일

GDS의 연결 후보를 찾고 작업자가 포함 도형을 조정하여 Chain을 정의한다. Layer 한 개를 Chain 한 개로 간주하지 않는다.

| 파일 | 역할 |
| --- | --- |
| [ChainCandidateTracer.cs](../../06_CoreLibSource/GdsMapControl/Chain/ChainCandidateTracer.cs) | Layer / 공간 후보 / 연결 탐색 |
| [ChainGeometryOverlap.cs](../../06_CoreLibSource/GdsMapControl/Chain/ChainGeometryOverlap.cs) | 도형 연결 판단 |
| [ChainSimilarGroupFinder.cs](../../06_CoreLibSource/GdsMapControl/Chain/ChainSimilarGroupFinder.cs) | 유사 도형 묶음 후보 |
| [ChainSimilarBatchEdit.cs](../../06_CoreLibSource/GdsMapControl/Chain/ChainSimilarBatchEdit.cs) | 유사 묶음 일괄 편집 |
| [GdsMapTestForm_ChainList.cs](../../06_CoreLibSource/GdsMapControl/GdsMapTestForm_ChainList.cs) | 다중 Chain과 선택 상태 |
| [GdsMapTestForm_ChainEdit.cs](../../06_CoreLibSource/GdsMapControl/GdsMapTestForm_ChainEdit.cs) | 수동 추가 / 제외 / 연결 확인 |
| [GdsMapControl_ChainOverlay.cs](../../06_CoreLibSource/GdsMapControl/Controls/GdsMapControl_ChainOverlay.cs) | 도면 위 Chain 표시 |

## 현재 기능과 기준

- 다중 Chain 관리 / Input 지정 / 선택적 Output 지정 / 후보 경로 탐색 / 수동 추가와 제외 / 유사 묶음 편집 코드가 있다.
- Input은 1개 필수, Output은 0~1개다. Output이 없는 열린 Chain도 처리 대상이다.
- 후보 연결은 Layer 필터와 공간 후보 검색을 사용한다. 자동 탐색 결과를 고객의 전기적 Netlist와 동일하다고 단정하지 않는다.
- Chain UI 상태는 메모리에서 관리된다. Chain DB 테이블이 존재해도 화면 저장 / 재조회가 완성된 것은 아니다.
- 영구 소속 키는 `MAP_REVISION_ID + PLACED_ELEMENT_ID` 기준이다. Grid 번호 / 화면 선택 순서 / `SCENE:n`을 DB 식별자로 쓰지 않는다.
- 하나의 도형이 여러 Chain에 포함될 수 있다. 다른 Chain의 공용 도형을 삭제하는 방식으로 제외를 처리하지 않는다.

## Chain 설정 순서

1. GDS를 읽고 탐색할 Layer를 체크한다.
2. 새 Chain을 만든다. Code / 이름 / 색상 / 표시 상태를 Chain별로 관리한다.
3. 목록에서 편집할 Chain을 선택한다. 목록의 선택은 편집 대상이고 체크는 Map 표시 여부다. 두 상태를 구분한다.
4. Map에서 Input 도형을 한 개 지정한다. 필요하면 Output을 한 개 지정한다. 도형 키와 클릭한 월드 좌표를 함께 보관한다.
5. 선택한 Layer와 필요한 Layer 연결 규칙 / 공간 Cell 크기를 확인하고 후보 탐색을 실행한다.
6. 후보를 확인해 수동 추가 / 제외한다. 유사 묶음 검색은 목록에서 확인한 뒤 일괄 제외하며 직전 일괄 제외는 되돌릴 수 있다.
7. 다른 Chain으로 이동하기 전 현재 편집 상태를 메모리에 보관하고, 선택한 Chain의 상태를 복원한다. 프로그램 종료 후 DB 복원과는 다른 처리다.

## 핵심 클래스와 함수

| 구성 | 역할과 구현 방식 |
| --- | --- |
| ChainDefinition / `_chains` | GUID / Code / 이름 / 색상 / Input / Output / ManualAdded / ManualExcluded / Layer / 규칙 / LastResult / 되돌리기 상태를 Chain별로 보관 |
| CaptureActiveChain / RestoreActiveChain | 편집 중인 화면 상태와 선택 Chain의 상태를 교환 |
| TryCreateChainTraceInput | 도면 / 표시 가능한 Chain 선택 / Input 1개 / Output 최대 1개 / Layer 최소 1개 / 단자 Layer 포함 / 규칙 적용 조건을 검사 |
| ChainTraceRequest | 대상 도형 / 단자 키 / Layer / Layer 규칙 / 공간 Cell 크기 / 작업 영역을 탐색기에 전달 |
| ChainCandidateTracer | 선택 Layer와 작업 영역으로 후보를 제한하고 공간 격자 색인과 BFS로 연결 후보를 탐색 |
| ChainGeometryOverlap | 실제 도형 연결 판단을 담당. 공간 Bounds 후보 검색만으로 최종 연결을 확정하지 않음 |
| ChainTraceResult | Path / VisitedElements / 겹침 / 분기 / 단자 등 탐색 결과를 분리해 반환 |
| InvalidateChainTraceResult | Layer 조건 변경 시 이전 결과를 해제하고 확정 단자를 유지해 재탐색 준비 |
| ApplyCheckedChainSimilarGroups | 선택한 유사 묶음과 연결성 검사를 거쳐 일괄 제외 |
| UndoLastChainSimilarRemoval | 마지막 유사 묶음 제외 1회를 복원. 일반적인 무제한 Undo 기능은 아님 |

화면의 단자 제한과 탐색기 자체의 다중 시작점 API는 구분한다. Layer 규칙을 적용할 때 같은 Layer끼리의 연결도 규칙 대상으로 본다. 허용오차가 모든 도형에 같은 방식으로 작동한다고 가정하지 말고 실제 도형 좌표와 `AreElementsConnected` 경로를 확인한다.

## 저장 기능과 연결할 내용

현재 ChainDefinition은 세션 초안이다. DB 저장 시에는 고정 Chain ID / Version / 최종 Member / Layer Filter / Layer Rule / Edit 이력으로 변환해야 한다. UI의 Visible 체크는 업무 삭제 상태가 아니다. 화면의 Chain 삭제와 DB의 RETIRED 처리도 별도 구현이다.

## 검증 방법

[Probes 폴더](../../06_CoreLibSource/GdsMapControl/Probes)에는 Layer 필터, 열린 경로, 연결성, 유사 묶음 관련 검사 스크립트가 있다. Debug EXE를 읽으므로 먼저 현재 소스로 만든 EXE인지 확인한다.

```powershell
Set-Location 'D:\1_프로젝트 문서\18.스테츠칩팩_POC\ChippacPOC\DMS\06_CoreLibSource\GdsMapControl'
powershell -NoProfile -ExecutionPolicy Bypass -File '.\Probes\ChainLayerFilteredProbe.ps1'
powershell -NoProfile -ExecutionPolicy Bypass -File '.\Probes\ChainOpenEndedProbe.ps1'
```

위 명령은 후속 검증용 예시이며 이번에는 실행하지 않았다. 성공 종료와 스크립트 결과를 기록하고 실제 Map 클릭 / 강조 표시 / 편집 후 상태도 따로 확인한다.

## 다음 단계

현재 소스와 EXE 기준의 모델 검사부터 수행한다. 영구 저장은 [F03](F03_Map_배치_DB.md)의 Map / Chain 버전 계약과 함께 작은 단위로 연결한다. AREF와 배치 ID 재파싱 안정성도 남은 확인 대상이다.
