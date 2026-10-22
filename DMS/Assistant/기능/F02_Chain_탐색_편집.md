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


**2026-10-06 격자 크기 입력 숨김(CLAUDE-022, 사용자 결정 "소스상에 넣어줘. 필요시 꺼내자"):** 격자 크기는 Chain 탐색 공간 색인(`ChainSpatialIndex`)의 칸 크기다. 후보만 줄이고 연결은 실제 형상으로 판단하므로 결과가 아니라 속도 / 메모리에만 영향을 준다(동일 길이 경로가 여럿이면 표시 경로 순서만 달라질 수 있음). 화면 입력(`chainCellSizeLabel` / `_numChainCellSize`)은 Designer에서 `Visible = false`로 숨기고 `GdsMapTestForm_ChainList.cs`의 `DefaultChainCellSize = 10m`(µm)를 사용한다. 다시 꺼낼 때는 두 컨트롤의 Visible만 true로 바꾼다. 고정값이라 도형 크기 / 밀도가 크게 다른 GDS에서는 느려질 수 있다(후속 후보: 선택 Layer 도형 크기 기반 자동 계산). Chain DB 저장 시 `TQP_GDS_CHAIN_VER.CELL_SIZE`에는 사용한 값을 그대로 기록한다.

**2026-10-06 Chain DB 저장 설계(CLAUDE-024, 설계만):** [GDS Chain DB 저장 설계](../../../문서/설계/02_GDS/2026-10-06_GDS_Chain_DB저장_설계.md). 요점: 스키마 변경 없이 기존 6개 테이블 사용 / 도형 키 = PLACED_ELEMENT_ID(화면 ElementKey와 같음) / 신규 서비스 `iGdsChain`(Chain 1개 = Begin -> Member·Edit 배치 -> Complete) / 저장 전 검사: DB Map READY + 같은 GDS 파일(SHA-256) + Chain Layer가 DB Map Layer 안 / 불러오기는 TRACE Member를 `SavedTraceKeys`로 복원해 편집 재개 / 삭제는 RETIRED. 사용자 결정 6건(버전 이력 여부 등) 대기.

**2026-10-06 Chain 저장 서비스 1단계(CLAUDE-024):** 사용자 결정(덮어쓰기 / 실제 삭제 등, 설계 10.1절) 반영. `iGdsChain`(GetChainList / BeginChainSave / CreateChainMemberBatch / CreateChainEditBatch / CompleteChainSave / DeleteChain / 상세 조회 4개) 구현, `GdsChainProbe.ps1` 14 PASS(TEMP 빌드 BSL 직접 호출). 화면 연결(저장 / 불러오기)은 2·3단계. 운영 폴더 반영 대기.

**2026-10-06 화면 역할 재정의(CLAUDE-026, 설계만):** 사용자 지시로 Chain 설정 / Chain DB 반영은 **GDS Map View(조회) 화면**에서 한다. Setup 화면은 GDS 읽기 + Layer 필터 + Map DB 저장까지. Chain 화면 코드(`GdsMapTestForm_Chain*.cs`, `GdsMapForm_ChainDb.cs`)를 `GdsMapViewForm` partial로 옮기는 설계: [화면 역할 재정의 설계](../../../문서/설계/02_GDS/2026-10-06_GDS_Map_화면역할_재정의_설계.md). DB 도형으로도 Chain 탐색 가능(Source 재생성 확인). 사용자 결정 5건 대기.

**2026-10-06 Chain 화면 이동 1단계(CLAUDE-026):** Chain 설정 화면 코드가 `GdsMapViewForm_Chain*.cs` / `GdsMapViewForm_ChainDb.cs`(GDS Map View 화면)로 옮겨졌다. 위 표의 `GdsMapTestForm_*` 파일 이름은 이전 이름이다. DB 도형으로 탐색한 결과가 파일 도형과 같음을 `GdsChainDbTraceProbe.ps1`로 확인. DMS 화면 확인 대기.

**2026-10-07 Front / Back 보기(CLAUDE-030):** View 보기 기준을 바꿔도 Chain 소속(도형 ID)은 같다. Marker는 저장 기준 좌표로 저장되고 화면에서만 반전. 전환 처리 `GdsMapViewForm_ChainSide.cs`(`MirrorChainsForSideChange` / `ToStoredX` / `ToDisplayX`). Probe `GdsFrontBackViewProbe.ps1`.

**2026-10-07 Label 검색(CLAUDE-032):** View 화면 조회 조건 아래 Label 검색 줄. TEXT 도형 전체(Layer 무관) 색인을 [조회] 때 받아 정확히 / 시작 / 포함 검색 -> 결과 목록 콤보 + 지도 표시(노란 원 + 십자) + 300µm 이동. Chain 연결 표시는 2단계. 서비스 `iGdsMapView.GetMapTextIndex`, 로직 `Persistence/GdsLabelIndex.cs`, Probe `GdsLabelSearchProbe.ps1`. [Label 검색 설계](../../../문서/설계/02_GDS/2026-10-07_GDS_Label_검색_설계.md) 9절.

## 2026-10-08 / Chain 색상 구분 + 색상 변경 + IN·OUT 표시 제거 (CLAUDE-038, GDS Map View)

- **새 Chain 색:** 고정 8색 순환 대신 `Chain/ChainColorPicker.Pick`이 화면의 모든 Chain 색(DB에서 불러온 Chain 포함)과 CIE Lab 거리가 가장 먼 후보를 고른다. 지도 Layer 색과의 거리는 작은 가중치(최대 40 x 0.3)로만 반영한다. 후보는 색상환 24 x 3단계 + 흰색이며, 어두운 배경에서 안 보이는 색(Lab L < 50, 예: #0000FF)은 제외한다. 같은 입력이면 항상 같은 색이다.
- **색상 변경:** Chain 목록 우클릭 -> "색상 변경..."(ColorDialog, 추천 16색 제공). 우클릭한 행의 Chain 색을 바꾸며 편집 중인 Chain이 아니어도 된다. 다른 작업(Input / Output 지정, 편집, 탐색, DB 저장) 중에는 목록이 비활성이라 메뉴가 열리지 않는다. 색은 DB 저장 지문에 포함되므로 [Chain 저장(DB)] 때 변경 Chain으로 저장된다. 다른 Chain과 ΔE 15 미만이면 상태 줄에 알린다.
- **IN / OUT 표시 제거:** View 화면이 `GdsMapControl.SetChainEndpointPoints`에 항상 빈 목록을 넘긴다(2곳). 지정한 Input / Output 도형은 노란 선택 강조로 보인다. 공통 컨트롤의 표시 기능 자체는 남겨 두었다(다른 화면 호출 없음).
- 검증: `Probes/GdsChainColorProbe.ps1` 7 PASS(예전 8색 최소 ΔE 27.6 -> 51.2, 기존 초록 / 하늘색 다음 색 #FF0000 ΔE 148, 배경 대비 최소 ΔE 58.6). 화면 동작(우클릭 메뉴 / IN·OUT 미표시 / 다시 조회 후 잔여 없음)은 DMS 화면 확인 대기.

## 2026-10-08 / Input 근처 Label로 Chain 이름 자동 지정 (CLAUDE-049, GDS Map View)

- Input 확정 직후 `GdsMapControl.FindChainEndpointLabel`로 Input 주변 창(Input 크기 x 3) 안에서만 계산한다. 1순위 = Input에서 3단계 이내로 실제 겹쳐 이어진 도형 안의 Label(단계 수 -> 클릭점 거리), 2순위 = 창 안 가장 가까운 Label, 없으면 이름 유지. 창 안 도형이 2,000개를 넘으면 연결 탐색을 건너뛰고 2순위만 쓴다.
- 이름은 기본값("Chain " + Code)이거나 자동으로 붙인 이름일 때만 바꾼다. [이름 수정]으로 바꾼 이름은 그대로 둔다. Label 문자열을 그대로 쓰고, 같은 이름의 Chain이 있으면 상태 줄에만 알린다.
- 연결은 지금 보이는 Layer의 도형만 따라간다. Label은 Layer와 무관하며 화면 기준 좌표(Front / Back 반영)를 쓴다. DB / 서비스 변경 없음.
- 검증: `Probes/GdsChainInputLabelProbe.ps1` 4 PASS(표본 60 모두 정답, 평균 64.5ms / 최대 176.8ms). 화면 확인 대기.

## 2026-10-12 화면 표시 정리

- CLAUDE-059: Chain Code(A, B, C)는 화면에서 숨김(DB 키로만 사용). 새 Chain 기본 이름 "새 Chain n", Input Label 을 찾으면 그 이름으로 바뀜.
- CLAUDE-060: 자동 Chain 색에서 흰색 / 흰색 근처 제외(흰색 = 유사 묶음 찾기 결과 표시 색).

**2026-10-13 목록 체크는 더블클릭(CLAUDE-079):** 조회 화면 Chain 목록(표시) / 유사 묶음 목록(제외)은 한 번 클릭 = 선택만, 더블클릭 = 체크 토글. Smoke `GdsChainListDoubleClickSmoke.ps1`.
- 2026-10-14 CLAUDE-094: GDS Map 조회 화면 상태 줄에 경과 시간 칸(`ElapsedStatusTicker`, 조회 / 삭제 / Chain 저장 중 `경과 n.n초` -> `걸린 시간`) 추가, Chain DB 저장 중에도 진행 막대(Marquee)를 켠다. 배포(GdsMap EXE) / DMS 화면 확인 대기. [POC 화면 진행 표시줄](../../../문서/설계/06_환경_운영/2026-10-14_POC_화면_진행표시줄.md)
- 2026-10-19 CLAUDE-143: GDS Map 조회 화면 Chain 편집 버튼 이름 "경로 추가 / 경로 제외" -> **"도형 추가 / 도형 제외"**(지도에서 도형 하나씩 Chain 에 넣고 뺌, 경로는 [후보 경로 찾기]). 상태 줄 / 겹친 도형 메뉴도 "도형" 으로 통일. 배포(GdsMap EXE) / DMS 화면 확인 대기.
- 2026-10-20 CLAUDE-163: Chain 탐색 Layer / 연결 규칙 / 미조회 판단이 Layer 키(번호 + DataType, `GdsLayerKey`)를 쓴다(`ChainTraceRequest.SelectedLayerIds`, `ChainLayerConnectionRule.First/SecondLayerId`, `ChainTraceElement.LayerKey`). DB 저장은 탐색 Layer 2열(LAYER_ID / DATA_TYPE) / 규칙 5열(첫 번호 / DataType / 둘째 번호 / DataType / 허용 오차), 서버가 (번호, DataType) 작은 쪽을 앞에 둔다. 예전 열 수(1열 / 3열)는 DataType 0. 기존 Chain 은 DDL 로 775 -> 775/237 변환(로컬 57개). 미조회 문구 "Layer 2/0 미조회". 검증 `GdsChainProbe` / `GdsChainLoadProbe`(새 형식) / `GdsChainDbTraceProbe` / `GdsFrontBackViewProbe` / Chain 탐색 5종 PASS. 배포 / DMS 화면 확인 대기.
- 2026-10-20 CLAUDE-165: U8 같은 형태 Chain 자동 추출 검증(Probe `GdsChainAutoExtractProbe.ps1`): 연결 덩어리 일괄(Union-Find) + 예시 Chain 비교로 TSV_21063 과 같은 형태 27,341개(전체 27,413 중), 제품 추적기와 300 / 300 같음, 34초. 제품 기능은 범위 결정 대기. [설계](../../../문서/설계/02_GDS/2026-10-20_U8_같은형태_Chain_자동추출_설계.md)
- 2026-10-20 CLAUDE-166: U8 전용 새 화면 GDS Map View U8(POC 메뉴 "GDS Map 조회 U8", F03128) **구현 완료 / 배포 대기**. 기존 화면 그대로, U9 연결 알고리즘 기능 제외. [예시 지정](도형 클릭 -> 연결 덩어리) / [같은 형태 찾기](Union-Find + 구성·배치 비교) / 찾기 결과 가상 목록 / 범위(E-Test Net 목록 · 전체 일치) / [Chain 생성(DB)](신규 일괄 저장 `CreateChainBulk`) / 저장된 Chain 목록 · 삭제. Smoke PASS 20(U8 복사본: 같은 형태 27,341 = Probe, E-Test Net 2개 생성 · 삭제). D1 ~ D8 권장안. [설계](../../../문서/설계/02_GDS/2026-10-20_GDS_Map_View_U8_화면_설계.md)

**2026-10-21 U8 Chain 많은 Map 의 GDS Map View 조회 속도 분석(CLAUDE-176, 분석만):** 저장 Chain 27,342개인 U8 은 기존 View [조회]가 약 6분 20초(Chain 마다 서비스 4번 = 341초가 약 90%), 조회 뒤 Chain 클릭마다 약 2.2초. 개선안 S1 장치 단위 일괄 읽기 / S2 목록 BeginUpdate / S3 외곽선 Chain 별 저장 / S4 큰 표 Binary 전송(기존 함수 변경 승인 필요) -> 약 30초(S4 포함 약 20초). 결정 대기. [조회 속도 분석](../../../문서/설계/02_GDS/2026-10-21_U8_Chain_View_조회속도_분석.md).

**2026-10-21 U8 Chain 색(CLAUDE-178):** GDS Map View U8 의 [Chain 생성(DB)] 한 번(같은 Rule)으로 만든 Chain 은 모두 한 색(이미 저장된 Chain 색 / Layer 색과 가장 먼 색). 이미 저장된 Chain 색은 바꾸지 않음. Smoke `GdsMapViewU8Smoke.ps1`.

**2026-10-21 GDS Map View Chain 불러오기 속도 개선(CLAUDE-176 구현):** Chain 상세를 Chain 마다 4번 읽던 것을 Chain 번호 범위(5,000개)마다 한 번씩 읽는다(서버 `GdsChain.Get...Range` 4개 추가, Binary 전송). 목록 넣기 BeginUpdate, Chain 별 외곽선 저장 + Layer 별 그리기 구간 합치기. 불러온 Chain 이 1,000개를 넘으면 지도 표시(체크)는 끈 채로 불러온다(더블클릭으로 표시). U8 27,342개 상세 읽기 약 5분 -> 약 3초, 예상 [조회] 약 6분 20초 -> 약 30초. 서버 + 클라이언트 배포 / 화면 확인 대기. Probe `GdsChainRangeProbe.ps1`.

**2026-10-21 Chain 목록 [전체 체크] / [전체 미체크](CLAUDE-181):** 기존 View Chain 탭에 [전체 체크] / [전체 미체크](모든 Chain 지도 표시 켜기 / 끄기, 선택 행 그대로, 1,000개 초과 [전체 체크]는 확인). U8 저장된 Chain 목록에 '표시' 체크 칸 + [전체 체크](찾기로 거른 목록의 행) / [전체 미체크](전체) 추가, 체크한 Chain 은 고르지 않아도 지도에 그림(목록 선택 표시는 그대로, 21개 이상은 CHAIN_SEQ 범위 조회). Smoke `GdsChainListDoubleClickSmoke.ps1` / `GdsMapViewU8Smoke.ps1`. 배포 / 화면 확인 대기.

**2026-10-21 Chain 목록 체크는 체크 박스로만(CLAUDE-184, CLAUDE-079 대체):** GDS Map View Chain 목록(표시) / 유사 묶음 목록(제외)은 행 왼쪽 체크 박스를 눌렀을 때만 체크가 바뀐다. 행 클릭 / 더블클릭 / Space 는 선택만. 공용 `LayerColorCheckedListBox.CheckOnCheckBoxOnly`. Smoke `GdsChainListDoubleClickSmoke.ps1`.

**2026-10-20 U8 Net 목록 삭제 / 생성 버튼 이동(CLAUDE-168):** GDS Map View U8 의 [Net 목록] / 범위 콤보 / E-Test Net 거르기를 뺐다. [Chain 생성(DB)] 는 찾기 결과 탭에서 지금 목록(분류 / 찾기로 거르고 [목록에서 빼기]로 뺀 뒤) 중 일치 + DB 없음만 만든다. 이름 = Net Label + 번호, 없으면 TSV Code.

**2026-10-21 U8 저장된 Chain 색 변경(CLAUDE-187):** 저장된 Chain 탭 [색 변경] - 고른 Chain(여러 개 가능)의 색을 색 선택 창으로 정해 DB 에 바로 저장(서버 `UpdateChainColorBulk`, COLOR_ARGB 만), 목록 "색" 칸. Smoke PASS 35. 배포 대기(서버 + 클라이언트).

**2026-10-21 Chain 을 다른 DB 의 같은 GDS Map 으로 복사(CLAUDE-188):** `Assistant/DB/sql/make_u9_chain_copy.py` 로 로컬 Chain 묶음(예 OMM_TCB 19개)을 SQL 로 만들어 같은 GDS / 같은 Layer 로 저장된 Map(예 고객 TRAINING U9)에 넣는다. PLACED_ELEMENT_ID(R0/E<n>)는 같은 GDS 면 같다. 사전 검사(시작 도형 Layer · 위치 비교) / 같은 이름 건너뜀 / Code 겹침 시 빈 Code / 건수 확인 후 COMMIT.
