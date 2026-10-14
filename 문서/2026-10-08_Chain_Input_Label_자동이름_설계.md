# Chain Input 지정 시 근처 Label로 Chain 이름 자동 지정 설계

작성: 2026-10-08 Claude / 작업 ID: CLAUDE-049 (다른 세션이 043~048 사용) / 상태: 설계 (소스 수정 없음)

## 1. 요청과 판단

사용자 요청: GDS Map 조회 화면에서 Chain을 만들고 Input을 지정하면, Input 근처 Label(GDS TEXT) 이름을 Chain 이름으로 넣는다. "가장 가까운" 또는 "같이 연결된" Label을 찾는 방식으로 하고, Label이 없으면 이름을 바꾸지 않는다. 리소스를 많이 쓰면 안 된다.

예시 화면: Input(가운데 십자 PATH)에 이어진 두 PATH 끝의 원형 Pad 위에 `TSVDC_ASIC1_P_FL` Label이 있다. 이 Label이 Chain 이름이 되어야 한다.

**판단: 간단한 편이다(작은 기능 하나, 약 반나절).**

| 근거 | 내용 |
| --- | --- |
| Label 좌표가 이미 메모리에 있다 | 지도 컨트롤 `_textLabels`(TEXT 30,992개, 화면 기준 월드 좌표 = Front / Back 반전 반영)를 그대로 쓴다. DB 조회 / 서비스 변경이 없다 |
| 연결 판정 함수가 이미 있다 | Chain 탐색이 쓰는 `ChainGeometryOverlap.Intersects`(도형끼리 실제 겹침)와 `ContainsPoint`(점이 도형 안) |
| 계산 범위를 작게 묶을 수 있다 | Input 주변 작은 창(Window) 안의 도형만 한 번 걸러서 그 안에서만 연결을 따라간다. 전체 도면 탐색(Chain 탐색)과 달리 몇 hop만 본다 |
| 바뀌는 곳이 적다 | 지도 컨트롤 함수 1개 추가 + 조회 화면 Input 확정 처리에 호출 1곳 + 이름 자동 여부 표시 |

## 2. 찾는 방법

Input 확정 버튼을 누른 직후(UI 스레드, 한 번만) 실행한다.

```text
1) 창 만들기: Input 도형 Bounds를 R만큼 넓힌 사각형 W (R 기본 = Input 도형 크기의 3배, 최소 / 최대 제한)
2) 후보 도형: 화면 도형 중 Bounds가 W와 겹치는 BOUNDARY / PATH만 모은다 (전체 목록 1회 순회, 사각형 비교만)
3) 연결 따라가기(BFS): Input 도형에서 시작해 후보 안에서 실제로 겹치는 도형으로 최대 H hop(기본 3)까지 간다
4) Label 후보: W 안의 TEXT Label만 모은다 (Label 목록 1회 순회)
5) 1순위 = 연결된 도형 안에 있는 Label: Label 점이 도달한 도형 안(ContainsPoint)이면 (hop 수, Input 클릭점과의 거리) 순으로 고른다
6) 2순위 = 가까운 Label: 1순위가 없으면 W 안에서 Input 클릭점과 가장 가까운 Label (거리 R 이내)
7) 둘 다 없으면 이름을 바꾸지 않는다
```

- 예시 화면은 Input(십자) -> PATH(1 hop) -> Pad(2 hop) 안의 Label이라 1순위로 찾는다. 양쪽 Pad Label이 같은 이름이면 그 이름 하나를 쓴다.
- Layer: 연결은 지금 화면에 보이는 Layer의 도형만 본다(Input 후보 선택과 같은 기준). Label은 Layer와 무관하게 본다(Label Layer가 도형 Layer와 다른 경우가 많음).
- 상태 줄에 근거를 남긴다. 예: `A / 이름: TSVDC_ASIC1_P_FL (Input에서 2단계 연결된 도형의 Label)` 또는 `(가장 가까운 Label, 연결 확인 안 됨)`.

## 3. 리소스

| 항목 | 예상 (샘플 60만 도형 / Label 3.1만) |
| --- | --- |
| 후보 도형 걸러내기 | 사각형 비교 60만 번 1회, 수 ms~수십 ms |
| BFS | 창 안 도형 수십~수백 개 x hop 3, 무시할 수준 |
| Label 걸러내기 | 3.1만 번 비교 1회 |
| 메모리 | 창 안 후보 목록만 잠깐 사용. 색인을 새로 만들거나 보관하지 않음 |
| 실행 시점 | Input 확정 때 1회. 마우스 이동 / 다시 그리기와 무관 |

상한: 창 안 후보가 많으면(예: 5,000개 초과) BFS를 멈추고 2순위(가까운 Label)만 쓴다. 화면이 멈추지 않게 하기 위한 안전장치다.

## 4. 수정 예정 파일

| 파일 | 내용 |
| --- | --- |
| `06_CoreLibSource/GdsMapControl/Controls/GdsMapControl_Chain.cs` | 공개 함수 `FindChainEndpointLabel(inputKeys, clickPoint, ...)` 추가 -> 이름 / 근거(연결 hop 또는 가까움) / 거리 반환 |
| `GdsMapViewForm_Chain.cs` | Input 확정(`ApplyChainEndpointSelection`) 뒤 호출, 이름 반영 / 상태 줄 |
| `GdsMapViewForm_ChainList.cs` | Chain에 "이름 자동" 여부 보관(사용자가 이름을 고치면 자동 변경 중지), Chain 목록 갱신 |
| `Probes/` 신규 Probe | 샘플 GDS에서 알려진 Input으로 이름 / 시간 확인 |

DB / 서비스 / XML 변경 없음. Chain 이름은 기존처럼 [Chain 저장(DB)] 때 저장된다.

## 5. 결정이 필요한 사항

| 번호 | 항목 | 권장 |
| --- | --- | --- |
| 1 | 언제 이름을 바꾸나 | **기본 이름("Chain A")이거나 자동으로 붙인 이름일 때만.** 사용자가 [이름 수정]으로 바꾼 이름은 덮어쓰지 않는다. Input을 다시 지정하면 자동 이름은 다시 계산 |
| 2 | 이름 형식 | **Label 문자열 그대로**(예: `TSVDC_ASIC1_P_FL`). Chain Code(A, B…)는 따로 남는다 |
| 3 | 연결이 확인되지 않을 때 가까운 Label 사용 | **사용(2순위)**, 단 거리 R 이내만. 상태 줄에 "연결 확인 안 됨" 표시 |
| 4 | Input이 여러 개이고 Label이 서로 다를 때 | **가장 가까운(1순위 점수가 가장 좋은) 하나만** 사용. 다른 이름은 상태 줄에 함께 표시 |
| 5 | 다른 Chain과 이름이 같을 때 | **그대로 허용**(DB 고유 제약은 Chain Code만). 상태 줄에 "같은 이름의 Chain 있음" 표시 |
| 6 | 탐색 범위 기본값 | hop 3 / 창 = Input 크기 x 3 (샘플로 확인 후 조정) |

### 5.1 2026-10-08 사용자 결정

권장안 6건 모두 승인("진행").

## 6. 구현 (CLAUDE-049)

| 파일 | 내용 |
| --- | --- |
| 신규 `Controls/GdsMapControl_ChainLabel.cs` (csproj 등록) | `FindChainEndpointLabel(endpoints, maxHops = 3, windowFactor = 3, maxCandidates = 2000)` + 결과 `ChainEndpointLabelResult`(Text / Hops / Distance / OtherTexts / CandidateCount / CandidateLimitHit / ElapsedMs) |
| `GdsMapViewForm_Chain.cs` | Input 확정 뒤 `ApplyInputLabelName` 호출. 실패해도 Input 확정은 유지 |
| `GdsMapViewForm_ChainList.cs` | `ChainDefinition.NameAuto`(새 Chain true / [이름 수정] 시 false). DB에서 불러온 Chain은 이름이 기본값일 때만 자동 지정 |

- 후보 상한을 설계 3절의 5,000에서 **2,000**으로 낮췄다. 연결 따라가기가 후보끼리 비교(후보 수의 제곱)라서 큰 값이면 한 번에 수천만 번 비교가 될 수 있기 때문이다.
- 상태 줄 예: `Input 1개 확정 / ... / 이름: TSVDC_ASIC1_P_FL (Input에서 1단계 연결된 도형의 Label)`.

**검증 (`Probes/GdsChainInputLabelProbe.ps1`, 샘플 `OMM all layer2.gds`, 파일 로드 / 모든 Layer 표시):** Label이 들어 있는 가장 작은 도형과 실제로 겹치는 이웃 도형을 Input으로 두는 표본 60개 -> 60개 모두 연결 Label로 결정, 평균 64.5ms / 최대 176.8ms, 없는 도형은 null. 화면(Back side 보기, 일부 Layer 숨김, 상태 줄 문구)은 DMS 확인 대기.

### 6.1 2026-10-12 기본 이름 변경 (CLAUDE-059)

- Chain Code(A, B, C)를 화면에서 숨기면서 새 Chain 기본 이름을 "Chain A" -> "새 Chain 1"(Code 순번)로 바꿨다. 자동 이름 대상 판정(`IsDefaultChainName`)은 두 형식을 모두 기본값으로 본다(예전에 "Chain A" 로 저장된 DB Chain 도 Input 을 다시 지정하면 Label 이름으로 바뀜).
