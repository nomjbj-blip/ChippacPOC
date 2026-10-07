# GDS Chain DB 저장 설계

작성: 2026-10-06 Claude (CLAUDE-024) / 상태: **사용자 결정 반영(10.1절) / 1단계 서비스 구현 + Probe PASS / 운영 폴더 반영 대기**

관련 문서: [GDS Map DB 저장 설계](2026-10-02_GDS_Map_DB저장_설계.md) 20 / 21절, [GDS Map 조회 화면 설계](2026-10-02_GDS_Map_조회화면_설계.md) 3.4절, [Device / Map / Chain DB 스키마 설계](2026-10-01_Device_Map_Chain_DB_스키마_설계.md), `DMS/Assistant/기능/F02_Chain_탐색_편집.md`

## 1. 목적과 범위

GDS Map Setup 화면에서 만든 Chain(Input / Output / 포함 도형 / 탐색 Layer / Layer 규칙 / 수동 추가·제외)을 DB에 저장하고, 다시 불러와 이어서 편집하거나 조회 화면 / KLARF 연관분석에서 사용할 수 있게 한다.

| 포함 | 제외 (후속) |
| --- | --- |
| Setup 화면의 Chain 저장 / 불러오기 / 삭제(RETIRED) | 조회 화면의 Chain 표시(조회 설계 3.4절, 5단계) |
| Chain 저장 서비스(Interface / RO / BSL / DSL / XML) | KLARF Defect와 Chain Member 연관분석 |
| 저장 전 검사(Map 일치 / Layer 범위 / Input) | 유사 묶음 일괄 제외의 되돌리기 이력 저장 |

## 2. 현재 상태 (2026-10-06 소스 기준)

| 구분 | 상태 |
| --- | --- |
| 화면 Chain | `GdsMapForm`(partial `GdsMapTestForm_Chain*.cs`)의 `ChainDefinition` 목록. **메모리 전용**, 도면을 다시 열거나 화면을 닫으면 사라진다 |
| 도형 키 | Chain의 모든 도형 키(`ElementKey`, Input / Output / ManualAdded / ManualExcluded)는 `GlSceneItem.PlacedElementId`다. DB `TQP_GDS_PLACED_EL.PLACED_ELEMENT_ID`와 같은 값이다(파일 Flatten과 DB 복원이 605,080건 완전 일치, CLAUDE-015 / 018 Probe) |
| DB 테이블 | `TQP_GDS_CHAIN` / `_VER` / `_MEMBER` / `_LAYER` / `_RULE` / `_EDIT` 6개가 키 FACTORY + DEVICE_ID로 생성되어 있다(`2026-10-02_tqp_gds_schema_device.sql`). 시퀀스 `CHAIN_SEQ` / `BATCH_EDIT_SEQ` 존재(schema_seq.sql, device 재생성 때 삭제하지 않음) |
| 서비스 | `TQP_GDS_CHAIN.xml`의 `DELETE_CHAIN` 하나뿐(Map 삭제 후 재등록 때 사용). 저장 / 조회 없음 |
| Map 저장 | Device당 Map 1개, **체크된 Layer만 저장**(설계 21절). 재등록하면 그 Device의 Chain도 삭제 |

**스키마는 바꾸지 않는다.** 6개 테이블의 컬럼으로 화면 상태를 모두 표현할 수 있다(4절).

## 3. 전체 흐름

```text
[GDS Map Setup]
 GDS 파일 열기 -> Layer 체크 -> (DB 저장: Map READY)
 -> Chain 만들기 / 탐색 / 수동 편집          (지금과 같음, 메모리)
 -> [Chain 저장]  -- 저장 전 검사(6절) --> Chain마다 새 버전 저장 -> READY / 현재 버전 전환
 -> [Chain 불러오기] (같은 GDS 파일을 다시 연 뒤) -> DB 현재 버전을 ChainDefinition으로 복원 -> 이어서 편집

[GDS Map 조회]  (후속 5단계)
 도면만 / Chain만 / 둘 다 -> 현재 버전 Member를 PLACED_ELEMENT_ID로 강조
```

Chain 편집은 **파일로 연 Setup 화면에서만** 한다. 조회 화면은 읽기 전용으로 둔다(조회 설계 결정 유지).

## 4. 화면 상태 -> 테이블 매핑

| 화면(ChainDefinition) | 테이블 / 컬럼 | 규칙 |
| --- | --- | --- |
| (신규 Chain) | `TQP_GDS_CHAIN.CHAIN_SEQ` | 시퀀스 `CHAIN_SEQ`. 화면 GUID(`Id`)는 저장하지 않고, 저장 / 불러오기 후 `ChainDefinition`에 `ChainSeq`를 보관한다 |
| `Code` (A, B, … AA) | `TQP_GDS_CHAIN.CHAIN_CODE` (32자, Device 안 UNIQUE) | 불러온 Chain이 있으면 새 Chain 번호는 기존 최대 Code 다음부터 |
| 삭제 | `TQP_GDS_CHAIN.CHAIN_STATUS = RETIRED` / `RETIRE_TIME` | 과거 버전 / 공용 도형은 지우지 않는다 |
| `Name` / `Color` / `ApplyLayerRules` / `CellSize` | `TQP_GDS_CHAIN_VER.CHAIN_NAME` / `COLOR_ARGB` / `APPLY_LAYER_RULES` / `CELL_SIZE` | CellSize는 화면에서 숨겼지만 사용한 값(기본 10)을 그대로 기록 |
| 저장 1회 | `TQP_GDS_CHAIN_VER.VERSION_NO` = 이전 최대 + 1 | DRAFT로 만들고 확정 시 READY + IS_CURRENT=1, 이전 현재 버전은 SUPERSEDED + IS_CURRENT=0 |
| `Inputs` (1개) | `TQP_GDS_CHAIN_MEMBER` ROLE=INPUT, SOURCE=ENDPOINT, MARKER_X/Y = 클릭 좌표 | 필수 1개 |
| `Outputs` (0~1개) | ROLE=OUTPUT, SOURCE=ENDPOINT, MARKER_X/Y | 없으면 열린 Chain |
| 자동 탐색 결과(`LastResult` 후보) | ROLE=MEMBER, SOURCE=TRACE | 화면 `GetChainKeys`와 같은 계산: 자동 후보 ∪ ManualAdded − ManualExcluded ∪ Input / Output |
| `ManualAdded` | ROLE=MEMBER, SOURCE=MANUAL + `TQP_GDS_CHAIN_EDIT` ACTION=ADD | 자동 결과에 이미 있으면 TRACE로 둔다 |
| `ManualExcluded` | `TQP_GDS_CHAIN_EDIT` ACTION=EXCLUDE (MEMBER에는 없음) | 다시 불러와 재탐색할 때 엔지니어의 제외 결정을 재적용하기 위해 남긴다 |
| `LayerIds` | `TQP_GDS_CHAIN_LAYER` | |
| `LayerRules` | `TQP_GDS_CHAIN_RULE` (FIRST ≤ SECOND로 정렬, TOLERANCE) | |
| `LastSimilarUndo`, 표시 체크(`Visible`), 탐색 경로 순서(Path) | 저장 안 함 | UI 세션 상태 |

`EDIT_SEQ`는 현재 메모리에 편집 순서가 없으므로 ADD -> EXCLUDE 순서로 1부터 매긴다. `BATCH_EDIT_SEQ`(유사 묶음 일괄 제외)는 1차에서 NULL.

## 5. 서버 계약 (신규 `iGdsChain`, 기존 DefectDefine 방식)

Interface / RO / BSL `GdsChain`, DSL과 XML은 테이블별(`TQP_GDS_CHAIN` 기존 파일에 쿼리 추가, 나머지 5개 신규). 인자는 `string[]` / `string[,]`, 반환은 `DataTable` / `int`. `101_ServerBin/DACrux.SEMDMS.Service.exe.config`에 wellknown 1행 추가.

| 함수 | 내용 |
| --- | --- |
| `DataTable GetChainList(string strFactory, string strDeviceID)` | ACTIVE Chain의 현재 버전: CHAIN_SEQ / CODE / NAME / COLOR / VERSION_NO / APPLY_LAYER_RULES / CELL_SIZE / Member 수 / 수정 일시·사용자 |
| `DataTable GetChainLayers(...)` / `GetChainRules(...)` / `GetChainEdits(...)` | (Factory, Device, ChainSeq, VersionNo) 단위 |
| `DataTable GetChainMembers(...)` | PLACED_ELEMENT_ID / ROLE / SOURCE / MARKER. Member가 많으면 키 순서 페이지 조회(PLACED와 같은 keyset 방식) |
| `DataTable BeginChainVersion(string[] Info, string[,] Layers, string[,] Rules)` | Info: Factory / Device / ChainSeq(신규면 빈 값) / Code / Name / Color / ApplyRules / CellSize / 이전 현재 버전(충돌 검사) / 예상 Member 수 / 예상 Edit 수 / User. Map이 READY인지 확인 -> 신규면 CHAIN INSERT -> CHAIN_VER(DRAFT) + LAYER + RULE를 한 호출(=한 트랜잭션)로 저장. 반환: CHAIN_SEQ / VERSION_NO |
| `int CreateChainMemberBatch(string[] Key, string[,] Rows, string strUser)` | Key: Factory / Device / ChainSeq / VersionNo. 1만 행 단위(CLAUDE-023과 같은 크기), `ExecuteMultiple` |
| `int CreateChainEditBatch(string[] Key, string[,] Rows, string strUser)` | 같은 방식 |
| `DataTable CompleteChainVersion(string[] Key, string strUser)` | DB 건수 = 예상 건수, INPUT 1개 / OUTPUT ≤ 1개, Input / Output이 Member에 포함됐는지 확인 -> 이전 현재 버전 SUPERSEDED + 새 버전 READY / IS_CURRENT=1 (한 트랜잭션). 반환: RESULT(OK / MISMATCH / CONFLICT) / MESSAGE |
| `int RetireChain(string strFactory, string strDeviceID, string strChainSeq, string strUser)` | CHAIN_STATUS=RETIRED |
| `int DeleteDraftVersions(...)` | 중단된 DRAFT 버전 정리(BeginChainVersion 시작 시 같은 Chain의 DRAFT를 먼저 지움) |

- **충돌 검사**: 불러온 뒤 다른 사람이 같은 Chain을 저장했다면 `이전 현재 버전`이 DB와 달라 CONFLICT로 거부한다(스키마 설계 6항).
- 저장 단위는 **Chain 1개 = Begin -> Member 배치 -> Edit 배치 -> Complete**. Chain마다 독립적으로 확정되므로 중간 실패 시 그 Chain만 DRAFT로 남고 다른 Chain은 영향이 없다.

## 6. 저장 전 검사 (클라이언트)

| 순서 | 검사 | 실패 시 |
| --- | --- | --- |
| 1 | Factory / Device 입력, 도면이 열려 있음 | 안내 후 중단 |
| 2 | DB Map이 있고 READY (`GetMapInfo`) | "먼저 DB 저장(Map)을 완료하세요" |
| 3 | DB Map의 `GDS_SHA256` = 지금 연 파일의 SHA-256 | **저장 차단.** 다른 파일이면 PLACED_ELEMENT_ID가 같은 도형을 가리킨다는 보장이 없다 |
| 4 | Chain마다 Input 1개, Output ≤ 1개 | 해당 Chain 이름과 함께 안내 / 그 Chain만 제외하고 계속할지 질문 |
| 5 | Chain의 탐색 Layer / 규칙 Layer / Member 도형 Layer가 모두 DB Map Layer(`GetMapLayerList`) 안 | **저장 차단 + 빠진 Layer 번호 표시.** Map을 필터 저장(21절)할 때 그 Layer를 뺀 경우다. 서버 FK로도 막히지만 메시지가 불친절해서 먼저 검사한다 |
| 6 | 변경된 Chain만 저장 | `ChainDefinition.Dirty`(편집 / 이름 / 색 / 탐색 / Layer 변경 시 true). 확인창: "저장 n개 / 변경 없음 m개 / 삭제(RETIRED) k개" |

## 7. 불러오기와 편집 재개

1. Setup 화면에서 같은 GDS 파일을 열고 Factory / Device 입력 -> [Chain 불러오기].
2. 6절 2 / 3 검사(READY, 같은 파일). 화면에 메모리 Chain이 있으면 "현재 화면 Chain을 버리고 불러올까요?" 확인.
3. `GetChainList` -> Chain마다 Layer / Rule / Member / Edit 조회 -> `ChainDefinition` 생성:
   - Input / Output = ROLE INPUT / OUTPUT + MARKER 좌표(클릭 위치 Marker 복원)
   - 자동 결과 = SOURCE=TRACE Member 키 집합 -> 신규 필드 `SavedTraceKeys`
   - ManualAdded = EDIT ADD, ManualExcluded = EDIT EXCLUDE
   - `ChainSeq` / `VersionNo` 보관, `Dirty = false`
4. 표시: `LastResult`가 없으므로 `GetChainKeys` / `GetAutomaticChainKeys`가 `SavedTraceKeys`를 자동 결과로 쓰게 한다(두 메서드에 분기 1개). [후보 탐색]을 다시 누르면 지금처럼 새 탐색 결과가 `SavedTraceKeys`를 대신한다.
5. DB의 PLACED_ELEMENT_ID가 지금 도면에 없으면(이론상 3번 검사로 막힘) 다른 도형으로 대체하지 않고 오류로 표시한다.

## 8. 화면 변경 (Setup, Designer 배치)

- Chain 목록 버튼 영역(새로 만들기 / 이름 / 삭제 옆)에 **[Chain 저장]**, **[Chain 불러오기]** 추가. DB 저장 컨트롤처럼 DMS 호스트 모드에서만 보인다(단독 실행은 Service 연결 정보가 없음).
- Chain 목록 항목에 저장 상태 표시: `A / Chain A (v3)`, 변경되면 `A / Chain A (v3*)`, 새 Chain은 `(새)`.
- 삭제 버튼: DB에 있는 Chain이면 "DB에서도 삭제(RETIRED)됩니다. 저장할 때 반영" 안내 후 목록에서 제거하고 삭제 대기 목록에 넣는다.
- Map 재등록(삭제 후 다시 등록) 확인창의 "Chain 설정도 함께 삭제" 문구는 그대로 둔다.

## 9. 구현 단계 (작은 단위, 단계마다 확인)

| 단계 | 내용 | 확인 |
| --- | --- | --- |
| 1 | DSL 5개 + XML(INSERT / SELECT / UPDATE), Interface / BSL / RO `GdsChain`, Service config 1행 | Probe: 샘플 Map에 Chain 2개 저장 -> 재조회 일치 / 두 번째 저장 시 버전 증가 / SUPERSEDED / CONFLICT / Layer 범위 밖 Member 거부 / RETIRED |
| 2 | Setup 화면 [Chain 저장] (6절 검사 + Chain별 저장 + Dirty) | Probe(화면 로직과 같은 행 생성기) + DMS 화면 확인 |
| 3 | [Chain 불러오기] + `SavedTraceKeys` 표시 / 편집 재개 | 저장 -> 화면 닫기 -> 다시 열기 -> 불러오기 -> Member / Marker / 수동 편집 일치 |
| 4 | 삭제(RETIRED) / 목록 저장 상태 표시 | DMS 화면 확인 |
| 5 | (후속) 조회 화면 Chain만 / 둘 다 | 조회 설계 3.4절 |

수정 예정 파일: 신규 `02_DMS/DACrux.SEMDMS.Interface/iGdsChain.cs`, `RO/GdsChain.cs`, `BSL/GdsChain.cs`, DSL `TQP_GDS_CHAIN_VER.cs` / `_MEMBER` / `_LAYER` / `_RULE` / `_EDIT`, XML 같은 이름 5개 + `TQP_GDS_CHAIN.xml` 쿼리 추가, csproj 4개, Service config, `GdsMapControl`의 `GdsMapTestForm_ChainList.cs` / 신규 `GdsMapForm_ChainDb.cs` / `GdsMapForm.Designer.cs` / 행 생성기 `Persistence/GdsChainRowBuilder.cs`. CLAUDE-023이 수정 중인 `GdsMapForm_DbSave.cs` / BSL `GdsMapImport.cs`는 건드리지 않는다.

## 10. 결정이 필요한 사항

| # | 질문 | 권장 | 다른 안 |
| --- | --- | --- | --- |
| 1 | Chain 저장 시 버전 이력을 남길까? | **남긴다.** 저장마다 VERSION_NO 증가, 이전은 SUPERSEDED. KLARF 분석 결과에 "어느 버전 Chain으로 판단했는지"를 남길 수 있다. 스키마가 이미 이 구조다 | 덮어쓰기(항상 버전 1, 지우고 다시 저장). Map처럼 단순하지만 과거 판단 근거가 사라진다 |
| 2 | 저장 단위 | **[Chain 저장] 한 번에 변경된 Chain 전체** | 선택한 Chain 1개만 |
| 3 | 화면에서 Chain 삭제 시 DB 처리 | **RETIRED(논리 삭제)** | 물리 삭제 |
| 4 | 다른 GDS 파일(SHA 다름)로 연 상태에서 저장 | **차단** | 경고 후 허용(위험: 다른 도형에 연결) |
| 5 | Chain Layer가 DB Map에 없는 경우 | **차단 + 빠진 Layer 안내**(Map을 그 Layer 포함해 재등록해야 함) | 해당 Chain만 건너뛰고 나머지 저장 |
| 6 | 단독 실행(DMS 밖)에서도 Chain 저장 | **DMS 호스트 모드만**(Map DB 저장과 같은 규칙) | — |

### 10.1 2026-10-06 사용자 결정 (최신 기준, 4 / 5 / 7 / 8절보다 우선)

| # | 결정 | 설계 반영 |
| --- | --- | --- |
| 1 | **버전 이력 없음, 덮어쓰기** | `VERSION_NO`는 항상 1. 저장 = 같은 Chain의 `TQP_GDS_CHAIN_VER`(-> MEMBER / LAYER / RULE / EDIT CASCADE) 삭제 후 VERSION 1을 DRAFT로 다시 저장 -> 확정 시 READY / IS_CURRENT=1. `TQP_GDS_CHAIN` 행(CHAIN_SEQ / CODE)은 유지해 Chain 번호가 바뀌지 않는다. SUPERSEDED / 충돌(CONFLICT) 검사는 쓰지 않는다(마지막 저장이 이김) |
| 2 | 저장 단위 = 변경된 Chain 전체 | 6절 6번 그대로 |
| 3 | **삭제는 실제 삭제** | `DeleteChain` = `TQP_GDS_CHAIN` 행 DELETE(하위 VER / MEMBER / LAYER / RULE / EDIT는 CASCADE). RETIRED / RETIRE_TIME은 쓰지 않는다 |
| 4 | 다른 GDS 파일이면 차단 | 6절 3번 |
| 5 | DB Map에 없는 Layer를 쓰는 Chain은 차단 + 빠진 Layer 안내 | 6절 5번 |
| 6 | DMS 호스트 모드만 | 8절 |

**중간 실패**: 덮어쓰기이므로 저장 도중 실패한 Chain은 VERSION 1이 DRAFT로 남는다. 목록 / 불러오기에서 "저장 미완료"로 표시하고 다시 저장하면 지우고 처음부터 저장한다(Map 저장과 같은 방식).

**서버 계약 변경(5절 대체)**

| 함수 | 내용 |
| --- | --- |
| `DataTable GetChainList(strFactory, strDeviceID)` | Chain별 CHAIN_SEQ / CODE / NAME / COLOR / APPLY_LAYER_RULES / CELL_SIZE / VERSION_STATUS(READY / DRAFT) / Member 수 / 수정 일시·사용자 |
| `DataTable GetChainLayers / GetChainRules / GetChainEdits / GetChainMembers(strFactory, strDeviceID, strChainSeq)` | VERSION_NO=1 고정이라 키에서 뺀다 |
| `DataTable BeginChainSave(string[] ChainInfo, string[,] aLayers, string[,] aRules)` | ChainInfo 9개: Factory / Device / ChainSeq(신규면 빈 값) / Code / Name / Color / ApplyRules / CellSize / User. Map READY 확인 -> 신규면 Code 중복 확인 + CHAIN INSERT(시퀀스) / 기존이면 Code 일치 확인 + CHAIN UPDATE + VER 삭제 -> VER(1, DRAFT) + LAYER + RULE INSERT. 기존 BSL과 같이 문장마다 자동 Commit(트랜잭션 아님). 반환 CHAIN_SEQ / CHAIN_CODE |
| `int CreateChainMemberBatch / CreateChainEditBatch(string[] Key, string[,] Rows, strUser)` | Key: Factory / Device / ChainSeq |
| `DataTable CompleteChainSave(string[] Key, strUser)` | 예상 건수 = DB 건수, INPUT 1개 / OUTPUT ≤ 1개 확인 -> READY / IS_CURRENT=1. RESULT OK / MISMATCH |
| `int DeleteChain(strFactory, strDeviceID, strChainSeq, strUser)` | 실제 삭제 |

예상 건수는 `TQP_GDS_CHAIN_VER`에 컬럼이 없으므로 BeginChainSave가 받은 값을 Complete에서 다시 받는다(Key에 예상 Member / Edit 수 추가). 스키마는 그대로다.


## 11. 2026-10-06 / 1단계 구현 (서비스 + Probe)

**구현 파일**: Interface `iGdsChain.cs` / RO `GdsChain.cs` / BSL `GdsChain.cs` / DSL `TQP_GDS_CHAIN_VER` `_MEMBER` `_LAYER` `_RULE` `_EDIT` (.cs) / XML 같은 이름 5개 신규 + `TQP_GDS_CHAIN.xml`에 쿼리 7개 추가(기존 `DELETE_CHAIN` 그대로) / csproj 4개 Compile 추가 / Probe `GdsChainProbe.ps1`.

**10.1절 대비 구현 기준**
- 트랜잭션은 쓰지 않는다(기존 GdsMapImport와 같이 문장 자동 Commit). Begin 도중 실패(예: Map에 없는 Layer -> FK 오류)하면 그 Chain은 DRAFT로 남고, 다시 저장하면 지우고 새로 만든다.
- Layer 배열 / Member / Edit 배치는 `InsertExecuteMultiple`(호출 단위 원자적), 배치 최대 10,000행.
- 신규 저장(ChainSeq 빈 값)인데 같은 Code가 있으면 거부한다. 기존 Chain의 Code는 바꿀 수 없다.
- 규칙은 서버가 작은 Layer 번호를 FIRST로 정리한다(DB CHECK). 허용 오차 / Marker 좌표는 G17 문자열 + `TO_BINARY_DOUBLE`, 격자 크기는 `TO_NUMBER(..., NLS 고정)`.
- PLACED_ELEMENT_ID는 Trim하지 않는다(화면 / PLACED_EL과 정확히 같아야 함).
- 확정(`CompleteChainSave`) 조건: Member 수 / 편집 수 = 예상, INPUT 1개, OUTPUT 0~1개. 아니면 MISMATCH + DRAFT 유지.

**검증**: `GdsChainProbe.ps1 -BinDir <TEMP 빌드>` 14 PASS (로컬 docker DB, Map PROBE / LAYER_FILTER_PROBE). 신규 저장·확정 / 목록(격자 크기 `10` 표기) / Layer·규칙 정리 / Member·Marker 좌표 비트 일치 / 편집 기록 / 같은 Code 거부 / 확정 후 배치 거부 / 덮어쓰기(번호 유지, 저장 중 DRAFT, 이전 내용 삭제, VERSION 1) / INPUT 없음·건수 불일치 MISMATCH / Map에 없는 Layer(FK_GDS_CHAIN_LAYER_MAP)·도형(FK_GDS_CHAIN_MEMBER_EL) 거부 / 실제 삭제 CASCADE. Service Remoting 경유는 미확인.

## 12. 2026-10-06 / 2단계 구현 (Setup 화면 [Chain 저장(DB)])

- 버튼: Chain 목록의 [새 Chain / 이름 수정 / 삭제] 아래 **[Chain 저장(DB)]**. DMS 호스트 모드에서만 보인다.
- 검사 순서: Factory / Device 입력 -> 도면 열림 -> Input·Output 지정 / 수동 편집 중 아님 -> DB Map READY -> **같은 GDS 파일(SHA-256)** -> Chain별 저장 행 생성 -> 도형이 지금 도면에 있는지 + 사용 Layer(탐색 / 규칙 / Member / 편집 도형)가 DB Map Layer 안인지.
- 차단 / 제외: DB Map에 없는 Layer를 쓰는 변경 Chain이 하나라도 있으면 **전체 저장 차단 + Chain별 빠진 Layer 안내**(결정 5). Input 1개 아님 / Output 2개 이상 / 포함 도형 없음 / 지금 도면에 없는 도형은 **그 Chain만 제외**하고 확인창에 이유를 보여 준다.
- 변경 판단: 저장 내용 지문(이름 / 색 / 옵션 / Layer / 규칙 / Member / 편집 기록의 SHA-256)을 마지막 저장 지문과 비교. 같으면 "변경 없음"으로 보내지 않는다(결정 2).
- 삭제: 목록에서 DB에 저장된 Chain을 지우면 삭제 대기로 두고 다음 [Chain 저장(DB)] 때 실제 삭제(결정 3).
- 불러오기(3단계) 전 임시 대응: 새 Chain의 Code가 DB에 이미 있으면 확인창에 알리고 그 Chain을 덮어쓴다.
- 목록 표시: 저장된 Chain은 ` (DB)`, 확정 실패는 ` (DB 미완료)`.


## 13. 2026-10-06 / 화면 역할 재정의로 12절 대체

사용자 지시: Setup 화면은 GDS 읽기 + Layer 필터 + Map DB 저장까지, Chain 설정과 Chain DB 반영은 View 화면에서 한다. 상세는 [GDS Map 화면 역할 재정의 설계](2026-10-06_GDS_Map_화면역할_재정의_설계.md). 12절(Setup의 [Chain 저장(DB)])은 컴파일만 했고 운영 폴더에 반영하지 않았으며, View로 옮겨 재사용한다(파일 SHA-256 / Layer 범위 검사는 View에서 불필요). 1단계 서비스(11절)는 그대로 쓴다.
