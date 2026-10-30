# U8 Chain 생성 뒤 GDS Map View 조회 속도 분석 (2026-10-21)

- 요청: "U8에 대한 chain 생성하고 GDS MAp View 조회시 상당한 시간이 걸리다. chain이 대한 목록이 매우 많고 element가 많아서 인것 같다.. 속도 올릴수 있는 방법 한번 찾아줘"
- 대상: 로컬 DB(docker DMSMGR) `SCK / U8` 실제 데이터, 실행 중인 DMS Service(`tcp://localhost:7515`)
- 작업: CLAUDE-176 / 원인 분석 + 개선안(설계). 제품 소스 수정 없음, DB 는 조회만. 측정 스크립트는 저장소 밖(9절).

## 1. 결론

로컬 `SCK / U8` 에 저장 Chain 이 **27,342개**(GDS Map View U8 화면의 같은 형태 Chain 일괄 생성, Chain 마다 도형 10개 = 273,420행) 있다. 기존 GDS Map View 는 [조회] 때 이 Chain 을 **하나씩 서비스로 읽어서** 시간이 대부분 여기서 나간다.

| 화면 | 지금(로컬 측정 / 추정) | 가장 큰 원인 |
| --- | --- | --- |
| 기존 GDS Map View [조회] | **약 6분 20초** | Chain 상세 읽기 341초(약 90%): Chain 마다 서비스 4번 x 27,342 = 109,368번 호출 |
| 기존 GDS Map View 조회 뒤 Chain 하나 클릭 | 약 2.2초 / 클릭 | 클릭마다 모든 Chain(27,342개) 외곽선을 다시 만든다 |
| GDS Map View U8 [조회] | 약 20초 | 도형 조회 13.5초 + Label 색인 3.7초 + 도면 구성 2 ~ 3초(Chain 상세는 읽지 않음) |

개선안(효과는 로컬 측정 기준):

| 순위 | 개선 | 바뀌는 곳 | 효과 | 기존 영향 |
| --- | --- | --- | --- | --- |
| S1 | Chain 상세(Layer / 규칙 / 포함 도형 / 편집)를 **장치 단위로 한 번에** 읽기, Binary 전송 | 서버 함수 **추가**(Interface / RO / BSL / DSL / XML) + 기존 View `ReadDbChains` | 341초 -> 약 4 ~ 5초 | 기존 함수는 그대로. 서버 배포 필요 |
| S2 | Chain 목록(CheckedListBox) 넣기를 `BeginUpdate / EndUpdate` 로 감싸기 | `GdsMapViewForm_ChainLoad.cs` | 약 11 ~ 15초 -> 약 2초 | 동작 같음 |
| S3 | 외곽선을 Chain 별로 저장해 두고 바뀐 Chain 만 다시 계산 | `GdsMapViewForm_ChainList.cs` / `Controls/GdsMapControl_ChainOverlay.cs` | 클릭 2.2초 -> 바뀐 Chain 만(예상 수십 ms) | 표시 같음, 구현 후 측정 |
| S4 | 큰 조회 결과(도형 페이지 / Label 색인 / Chain 목록)를 Binary 형식으로 전송 | 기존 BSL 조회 함수(전송 형식 한 줄) | 도형 12.4초 -> 약 5 ~ 6초, Label 3.7초 -> 약 1초 | **기존 함수 변경**(승인 필요). 두 화면 모두 빨라짐 |
| S5 | (운영) Chain 이 매우 많은 Map 은 U8 화면으로 보고, 기존 View 는 많은 Chain 을 "목록만" 받기 | 결정 | - | 동작 변경 |

- S1 + S2 + S3: 기존 View 약 6분 20초 -> **약 30초**, 클릭 2.2초 -> 바뀐 Chain 만.
- S4 까지: 기존 View 약 20초, U8 화면 약 20초 -> 약 12초.
- 고객 서버(네트워크 너머)는 호출 1번 지연이 커서 S1 효과가 더 크다(예: 호출당 20ms 면 지금 방식 Chain 상세만 약 36분).

## 2. 측정 방법

- 데이터: 로컬 docker DMSMGR `SCK / U8`(사용자가 만든 Chain 그대로). Map 도형 329,114개(Layer 키 4개: 저장 때 고른 Layer), 저장 Chain 27,342개, 포함 도형 합계 273,420행.
- 서비스: 실행 중인 `DACrux.SEMDMS.Service` 를 클라이언트와 같은 Remoting(`tcp://localhost:7515`, `Activator.GetObject`)으로 호출.
- 화면 쪽: 현재 소스 TEMP 빌드 GdsMap 의 `GdsMapViewForm` 을 화면 밖에 띄우고, `LoadChainsFromDb` 와 같은 함수를 같은 순서로 불러 단계별 시간을 쟀다(리플렉션). Chain 은 3,000개까지 실제로 복원해 비례를 확인하고 27,342개는 비례로 추정했다.
- DB 변경 없음(조회만). 같은 PC 라 네트워크 지연은 거의 없는 조건이다.

## 3. 기존 GDS Map View [조회] 단계별 시간

처리 순서는 `GdsMapViewForm.btnSearch_Click` 그대로다.

| 단계 | 하는 일 | 시간 | 근거 |
| --- | --- | --- | --- |
| 1 | Chain 목록(`GetChainList`) 1번 | 0.7 ~ 2.7초 | 측정(27,342행) |
| 2 | **Chain 마다** `GetChainLayers` / `GetChainRules` / `GetChainMembers` / `GetChainEdits` | **약 341초** | Chain 300개 12.65ms / 3,000개 12.47ms -> 27,342개 비례 |
| 3 | Label 색인(`GetMapTextIndex`) | 3.7초 | 측정(54,834행) |
| 4 | 도형 페이지(`GetPlacedElementPage`, 1만 행 x 35번) + 행 해석 | 12.4 + 1.1초 | 측정(329,114개) |
| 5 | 도면 구성(`ShowPlacedElements`) | 2.2 ~ 3.1초 | 측정 |
| 6 | 화면 Chain 만들기(`CreateChainFromDb`) | 약 1.3초 | 3,000개 141ms 비례 |
| 7 | Chain 목록에 하나씩 넣기(`_chainList.Items.Add`) | 약 11 ~ 15초 | 3,000개 1.7초 비례 / 27,342개 단독 실험 10.7초 |
| 8 | 모든 Chain 외곽선 만들기(`RefreshChainDisplay`) | 약 2.4초 | 3,000개 261ms 비례 |
| | 합계 | **약 6분 20초** | |

조회 뒤: 목록에서 Chain 을 고르거나 체크를 바꿀 때마다 8단계(`RefreshChainDisplay`)가 다시 돌아 약 2.2초씩 걸린다(3,000개 241ms 측정 -> 비례).

## 4. GDS Map View U8 [조회]

`GdsMapViewU8Form.btnSearch_Click`: Label 색인(3.7초) -> 도형 페이지(13.5초) -> 도면 구성(2 ~ 3초) -> 도형 색인 -> 저장 Chain Code 목록(`GetChainCodeList` 1번, 0.6초, 가상 표). Chain 상세는 고른 Chain 만(최대 20개) 읽는다. CLAUDE-166 Smoke 기록 조회 16.8초와 같은 수준(약 20초)이다. 이 화면은 S4 만 효과가 있다.

## 5. 원인 상세

### 5.1 Chain 마다 서비스 4번 (가장 큼)

`GdsMapViewForm_ChainLoad.cs` `ReadDbChains`: 목록을 받은 뒤 Chain 마다 4번 부른다. 호출 1번은 약 3ms(Remoting 왕복 + DB)인데 27,342 x 4 = 109,368번이다. 같은 내용을 장치 단위로 한 번에 읽으면:

| 방식 | 포함 도형 273,420행 |
| --- | --- |
| 지금(Chain 마다) | Member 만 1,000 Chain 4.3초 -> 27,342개 약 118초(4종 합계 약 341초) |
| 장치 단위 한 번(`SELECT_CHAIN_MEMBER_BOUNDS_ALL` 과 같은 조건, 연관 분석이 이미 사용) | DB 2.7 ~ 3.6초 + 전송 XML 107MB(보내기 + 받기 약 4.3초) / **Binary 22MB(약 0.6초)** |

PK(`FACTORY, DEVICE_ID, CHAIN_SEQ, VERSION_NO, PLACED_ELEMENT_ID`) 앞부분으로 범위 조회가 되어 장치 단위 조회가 빠르다.

### 5.2 DataTable XML 전송

Remoting 은 DataTable 을 기본 XML 형식으로 보낸다. 같은 표를 Binary 로 보내면(보내기 + 받기, 네트워크 제외):

| 표 | XML | Binary |
| --- | --- | --- |
| 도형 한 페이지(1만 행 x 20열) | 199ms / 7.7MB | 45ms / 1.9MB |
| Label 색인(54,834행) | 586ms / 15.7MB | 131ms / 3.4MB |
| Chain 목록(27,342행) | 510ms / 13.8MB | 205ms / 2.8MB |
| 장치 Member(273,420행) | 약 4.3초 / 107MB | 약 0.6초 / 22MB |

크기가 1/4 ~ 1/5 이라 고객 서버처럼 네트워크를 거치면 차이가 더 크다. 클라이언트는 받는 쪽 코드를 바꿀 필요가 없다(DataTable 이 형식을 스스로 판별).

### 5.3 CheckedListBox 하나씩 넣기

`LoadChainsFromDb` 가 `_chainList.Items.Add(chain, visible)` 를 Chain 마다 부른다. 넣을 때마다 목록을 다시 계산해 27,342개 단독 실험에서 10.7초, `BeginUpdate / EndUpdate` 로 감싸면 2.1초였다(`AddRange` 3.4초).

### 5.4 클릭마다 모든 외곽선 다시 만들기

`GdsMapViewForm_ChainList.cs` `RefreshChainDisplay`: 보이는 Chain 전체에 대해 표시 키 계산 -> `GetChainTraceElements` -> `SetVisibleChainOverlays`(정점 전체 다시 생성)를 한다. Chain 선택 / 체크 변경 / 이름 변경 때마다 불린다.

## 6. 개선안 상세

### S1. Chain 상세 장치 단위 일괄 읽기 (권장, 효과 가장 큼)

- 서버(기존 방식: Interface / RO / BSL / DSL / XML, 기존 함수는 그대로 두고 **추가만**):
  - XML(`TQP_GDS_CHAIN_LAYER` / `_RULE` / `_MEMBER` / `_EDIT`): 기존 Chain 단위 조회와 같은 열 + `CHAIN_SEQ`, 조건은 `FACTORY / DEVICE_ID` (`CHAIN_SEQ` 조건 없음), `ORDER BY CHAIN_SEQ`. Member 는 기존 `SELECT_CHAIN_MEMBER` 와 같은 Join(도형 Layer).
  - BSL `GdsChain`: `GetChainLayersAll` / `GetChainRulesAll` / `GetChainMembersAll` / `GetChainEditsAll`(이름은 구현 때 확정). 돌려주기 전 `RemotingFormat = Binary`(새 함수라 기존 호출에 영향 없음).
  - 아주 큰 Map 대비: Member 는 `CHAIN_SEQ` 범위로 나눠 받을 수 있게 `after CHAIN_SEQ` + 최대 행 수 인자(도형 페이지와 같은 방식).
- 화면(기존 View): `ReadDbChains` = 목록 1번 + 위 4개(Member 는 페이지) -> `CHAIN_SEQ` 로 나눠 지금의 `ChainDbData` 를 만든다. `CreateChainFromDb` 이후는 그대로.
- 효과: 341초 -> 약 4 ~ 5초(Member DB 3초 + 전송 0.6초 + 나누기). 서버 호출 109,369번 -> 5 ~ 30번.
- 다른 화면: KLARF 연관 분석은 이미 장치 단위 한 번 조회라 해당 없음. Die 분석 / E-Test Para 가 Chain 을 Chain 마다 읽는지는 구현 때 확인하고 같은 함수를 쓸 수 있다.

### S2. 목록 한 번에 넣기 (권장, 작음)

- `LoadChainsFromDb` 의 넣기 반복을 `_chainList.BeginUpdate()` / `EndUpdate()` 로 감싼다. 약 11 ~ 15초 -> 약 2초.

### S3. 외곽선 다시 만들기 줄이기 (권장)

- Chain 별 외곽선(표시 키 + 도형)을 저장해 두고, 표시 키가 바뀐 Chain(편집 중 Chain) / 보이기가 바뀐 Chain 만 다시 계산한다. 정점 묶음도 Chain 별로 두어 바뀐 것만 다시 만든다.
- 처음 [조회] 때 한 번은 전체를 만든다(약 2.4초 그대로). 클릭 2.2초 -> 바뀐 Chain 만(구현 후 측정).
- 바꾸는 곳: `GdsMapViewForm_ChainList.cs` `RefreshChainDisplay`, `Controls/GdsMapControl_ChainOverlay.cs` `SetVisibleChainOverlays` / `BuildChainOverlayVertices`(GdsMapControl 은 공용이라 U8 화면 / Setup 화면 회귀 확인 필요).

### S4. 큰 조회 결과 Binary 전송 (결정 필요)

- 기존 BSL 조회 함수가 돌려주는 DataTable 에 `RemotingFormat = SerializationFormat.Binary` 한 줄: `GdsMapView.GetPlacedElementPage` / `GetMapTextIndex`, `GdsChain.GetChainList` / `GetChainCodeList`.
- 효과: 도형 페이지 1번 약 300ms -> 약 150ms(전송 부분 1/4), U8 329,114개 기준 12.4초 -> 약 5 ~ 6초. Label 색인 3.7초 -> 약 1초. 두 화면 / Die 분석 모두 빨라진다.
- 기존 함수 변경이라 승인이 필요하다. 받는 쪽은 바꿀 필요가 없다. 대안은 같은 내용의 Binary 함수를 추가하고 화면 호출만 바꾸는 것(파일 수 증가).

### S5. 운영 / 동작 변경 (참고)

- 같은 형태 Chain 수만 개가 있는 Map 은 U8 화면(목록 + 고른 Chain 만 표시)으로 보는 것이 맞다. 기존 View 는 Chain 하나하나를 편집 / 저장하는 화면이라 수만 개를 한꺼번에 열 이유가 적다.
- 원하면 기존 View 도 "Chain 이 1,000개를 넘으면 목록만 받고 체크한 Chain 만 상세를 읽기"로 바꿀 수 있다(편집 / 저장 흐름 영향이 커서 별도 설계).

## 7. 예상 효과 (로컬, 기존 View)

| 단계 | 지금 | S1 + S2 + S3 | + S4 |
| --- | --- | --- | --- |
| Chain 목록 | 0.7 ~ 2.7초 | 그대로 | 약 0.3초 |
| Chain 상세 | 약 341초 | 약 4 ~ 5초 | 그대로 |
| Label 색인 | 3.7초 | 그대로 | 약 1초 |
| 도형 조회 + 해석 | 13.5초 | 그대로 | 약 6 ~ 7초 |
| 도면 구성 | 2 ~ 3초 | 그대로 | 그대로 |
| 화면 Chain + 목록 + 외곽선 | 약 15 ~ 19초 | 약 6초 | 그대로 |
| 합계 | **약 6분 20초** | **약 30초** | **약 20초** |
| Chain 클릭 1번 | 약 2.2초 | 바뀐 Chain 만 | 그대로 |

추정치는 구현 후 같은 측정 스크립트로 다시 잰다.

## 8. 결정 필요 (-> 10절에서 결정됨)

| # | 항목 | 권장 |
| --- | --- | --- |
| 1 | 진행 범위 | S1 + S2 + S3 (서버 함수 추가 + 기존 View 수정). U8 화면 파일은 건드리지 않는다(CLAUDE-168 진행 중) |
| 2 | S4 기존 조회 함수 4개 Binary 전송 | 승인 시 함께 진행(두 화면 모두 빨라짐) |
| 3 | S5 | 운영으로 우선 안내, 기존 View 동작 변경은 보류 |

## 9. 측정 스크립트 (저장소 밖)

scratchpad `perf/` 폴더.

- `DbScan.ps1`: Map 목록 / 장치별 Chain 수(BSL 직접 호출, 조회만).
- `ViewPerf.ps1` / `.cs`: 서비스(Remoting)로 Chain 목록 / 상세 / 도형 페이지를 읽고, `GdsMapViewForm` 을 화면 밖에 띄워 도면 구성 / Chain 복원 단계별 시간.
- `Experiments.ps1` / `.cs`: Label 색인 / Code 목록 / 한 페이지 서비스 시간, DataTable XML / Binary 비교, CheckedListBox 넣기 방식 비교.
- `BulkProbe.ps1`: 장치 단위 Member 조회(`SELECT_CHAIN_MEMBER_BOUNDS_ALL`) DB 시간과 전송 크기.

## 10. 결정 / 구현 결과 (2026-10-21 13:16)

### 10.1 결정

- 사용자: "S1 장치 단위 일괄 읽기 + S2 목록 BeginUpdate + S3 외곽선 Chain 별 저장으로 구현 진행해줘. S4 는 보류."
- 구현 중 추가 결정: 외곽선 정점이 Chain 1개당 약 1,180개(Bump 원 33 ~ 65점)라 U8 27,342개를 모두 보이게 하면 정점 약 3,200만 개(불러오기 약 15초 + 메모리 1.6GB 이상)다. 질문 -> 사용자 선택 **"1,000개 넘으면 표시 끔"**: 불러온 Chain 이 1,000개를 넘으면 목록에는 모두 넣고 지도 표시(체크)는 끈 채로 불러온다. 더블클릭으로 체크한 Chain 만 외곽선을 그린다. 1,000개 이하(OMM_TCB 19개 등)는 지금처럼 모두 표시.

### 10.2 수정 파일

- 서버(기존 함수는 그대로, **추가만**):
  - `08_DBQuery/ORACLE/TQP_GDS_CHAIN_LAYER.xml` / `_RULE.xml` / `_MEMBER.xml` / `_EDIT.xml`: `SELECT_CHAIN_LAYER_RANGE` / `SELECT_CHAIN_RULE_RANGE` / `SELECT_CHAIN_MEMBER_RANGE` / `SELECT_CHAIN_EDIT_RANGE`(기존 Chain 단위 쿼리와 같은 열 + `CHAIN_SEQ`, 조건 `CHAIN_SEQ BETWEEN :from_seq AND :to_seq`, `ORDER BY CHAIN_SEQ`).
  - `DACrux.SEMDMS.Interface/iGdsChain.cs` / `RO/GdsChain.cs` / `BSL/GdsChain.cs`: `GetChainLayersRange` / `GetChainRulesRange` / `GetChainMembersRange` / `GetChainEditsRange`(Factory, Device, 시작 번호, 끝 번호). BSL 은 범위 검사(시작 > 끝 / 숫자 아님 거부) 후 `RemotingFormat = Binary` 로 돌려준다. DSL 은 범용 `GetData` 를 써서 바꾸지 않았다.
- 화면(GdsMap, U8 화면 파일은 수정 안 함):
  - `GdsMapViewForm_ChainLoad.cs`: `ChainDbData` 상세를 행 목록(`List<DataRow>`)으로, `ReadDbChains` = 목록 1번 + Chain 번호 순 5,000개씩 범위 조회 4종 -> `SplitByChain` 으로 나눠 담음. 범위 조회가 없는 이전 서버면(`RemotingException` / `MissingMethodException`, 첫 범위에서만) Chain 마다 읽는 예전 방식으로 바꾼다. `LoadChainsFromDb`: 목록 넣기를 `BeginUpdate / EndUpdate` 로 감쌈(S2), 1,000개 초과면 표시 끔(`ChainLoadVisibleLimit`).
  - `GdsMapViewForm_ChainList.cs` `RefreshChainDisplay`: Chain 별 외곽선 저장(`_chainOverlayCache`, 표시 키 / Code / 색이 같으면 다시 찾지 않음, 지도를 다시 그리면 비움).
  - `Controls/GdsMapControl_ChainOverlay.cs`: 보이는 Chain 외곽선을 Layer 별로 모아 Layer 마다 그리기 구간 1개(`AppendVisibleChainOverlays`), 같은 외곽선 객체가 같은 순서로 오면 정점을 다시 만들지 않음(`SameChainOverlays`). 선분 / 점 만들기는 `AppendChainOverlayLines` / `AppendChainOverlayPoint` 로 나눔(결과 같음).
  - `GdsMapViewForm.cs`: 조회 결과 문구에 "Chain 이 많아 지도 표시는 꺼 둠(더블클릭으로 표시)".
- Probe: 신규 `Probes/GdsChainRangeProbe.ps1`, `GdsChainLoadProbe.ps1`(범위 조회로 복원) / `GdsFrontBackViewProbe.ps1`(`ChainDbData` 새 형식) 수정.

### 10.3 검증 (TEMP 빌드: 서버 `%TEMP%\chippac\CLAUDE-176\build\srv` Interface `42a89042` / BSL `992ebc1d` / RO `dcd42887`, GdsMap `%TEMP%\chippac\CLAUDE-176\build\gdsmap` EXE `cd3e7a3d`, 모두 경고 0)

- `GdsChainRangeProbe.ps1` `GDS_CHAIN_RANGE_PROBE=PASS (9)`(BSL 직접, 조회만):
  - U8: Chain 27,342개를 범위 호출 24번으로 읽음, DB 약 2.9초(Layer 82,027행 0.16초 / 포함 도형 273,420행 2.7초). 포함 도형 행 수 = 목록 MEMBER_COUNT 합계, Binary 형식, 표본 300개 x 4종이 Chain 단위 조회와 같음(행 / 값 / 순서).
  - OMM_TCB(19개, 편집 34,810행) / U9(1개)도 같음. 범위 0.74초 대 Chain 단위 0.84초(캐시가 찬 뒤)로 Chain 이 적어도 느려지지 않음. 잘못된 범위 3종 거부.
- 화면 검사(scratchpad `ClientCheck`, BSL 직접 + TEMP GdsMap, 조회만):
  - 같은 Chain 을 예전(Chain 단위) / 새(범위 -> `SplitByChain`) 데이터로 `CreateChainFromDb` -> Code / 이름 / Layer / 규칙 / 단자 / 수동 추가 / 제외 / 탐색 결과 / 미복원 / 저장 지문 비교: U8 표본 500 + 200 + 10개, OMM_TCB 19개 모두 같음.
  - U8 27,342개: 범위 읽기 + 나누기 3.0초, `LoadChainsFromDb` 7.2초(화면 Chain 1.3 ~ 1.6초 + 목록 넣기 3.3 ~ 4.0초, 표시 끔이라 외곽선 0), 관리 메모리 +336MB.
  - U8 6,000개 모두 표시: 외곽선 정점 708만 개, 그리기 구간 3개(이전 18,000개), 바뀐 것 없는 클릭 14ms, Chain 1개 표시 끔 322ms. OMM_TCB: 클릭 181 -> 8ms.
- 회귀(같은 TEMP EXE): `GdsChainLoadProbe` PASS(PROBE 시험 Chain ZZLOAD1 만들고 지움) / `GdsFrontBackViewProbe` PASS / `GdsChainDbTraceProbe` PASS / `GdsChainListDoubleClickSmoke` PASS (38) / `GdsViewLayoutSmoke` PASS (29) / `GdsSetupLayoutSmoke` PASS (4) / `GdsChainInputLabelProbe` PASS.
- 실행 못 한 것:
  - DMS Service(Remoting) 경유 `ReadDbChains` 실제 호출: 측정 중 Service 가 꺼져 BSL 직접 호출로 대신했다. Remoting 전송은 Binary 22MB 기준 약 0.6초 + 네트워크 추가 예상.
  - 이전 서버 대체 경로(범위 조회 없음).
  - `GdsMapViewU8Smoke`: DB 쓰기 + CLAUDE-168 진행 중 파일이라 실행 안 함(U8 화면도 `SetVisibleChainOverlays` 를 쓰므로 화면 확인 필요).
- 예상(로컬, 기존 View U8 [조회]): Chain 목록 약 1초 + 상세 약 4초 + Label 색인 3.7초 + 도형 13.5초 + 도면 구성 약 3초 + Chain 복원 약 7초 = **약 30초**(이전 약 6분 20초). 화면에서 확인 필요.

### 10.4 배포 / 남은 일

- 배포는 사용자가 한다. 서버(Interface + BSL, XML 4개는 `08_DBQuery/ORACLE`)와 클라이언트(Interface + RO + GdsMap EXE)를 함께 반영하고 Service 를 다시 시작한다. TEMP 빌드는 지금 소스 전체(다른 작업의 미배포 변경 포함)로 만든 것이다.
- 남은 선택: 목록 넣기 3 ~ 4초(CheckedListBox 자체 비용)는 가상 목록으로 바꿔야 줄어든다(S5 와 함께 판단). S4(기존 조회 함수 Binary)는 보류.

## 11. 표시 예외 삭제 (2026-10-28, CLAUDE-225)

- 요청 "다 보여줘라. u8에 대해 예외 처리 된 부분도 전부": 1,000개 초과면 지도 표시(체크)를 끈 채로 불러오던 `ChainLoadVisibleLimit`(10.2절 / S5 성격), [전체 체크] 확인 창, 상태줄 "Chain 이 많아 지도 표시는 꺼 둠" 을 삭제했다. 저장된 표시 상태 그대로 불러온다.
- U8 화면: 목록에서 고른 Chain 을 지도에 앞 20개만 그리던 제한과 [전체 체크] 1,000개 확인 창 삭제. 20개 넘게 읽을 때 범위 조회 + 모래시계는 그대로(`SavedBatchReadMin`).
- 영향: U8 27,342개를 모두 표시로 불러오면 외곽선 정점 약 3,200만 개(10.1절 측정 기준 불러오기 약 15초 + 메모리 1.6 GB 이상)가 그대로 든다. S1 ~ S3(범위 조회 / 목록 한 번에 넣기 / 외곽선 재사용)는 그대로.
- Smoke `GdsViewLayoutSmoke.ps1` 29 / `GdsMapViewU8Smoke.ps1` 35 / `GdsChainListDoubleClickSmoke.ps1` 43 PASS. 배포 / 화면 확인 대기.
