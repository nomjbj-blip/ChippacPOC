# GDS Map View U8 화면 설계

작성: 2026-10-20 / 작업 ID: CLAUDE-166 / 상태: 결정 완료(D1 ~ D8 권장안, 사용자 2026-10-20 15:42) / 구현 완료(2026-10-20 16:30) / 배포 · DMS 화면 확인 대기

## 1. 요청과 원칙

- 요청: 기존 GDS Map View 는 그대로 두고 **GDS Map View U8** 화면을 하나 더 만들어 U8 개발을 그 화면에서 한다. 기존 U9 Chain 연결 알고리즘 기능은 빼고 **U8 에 필요한 알고리즘 기능만** 넣는다.
- 원칙
  - 기존 `GdsMapViewForm`(GDS Map 조회) / `frmGdsMapView`(DMS 호스트) / `GdsMapControl`(지도) / 기존 서버 메서드는 **고치지 않는다.** 새 화면은 새 파일로만 만든다.
  - 지도 / Layer 목록 / 좌표 해석 / 연결 판정 같은 공용 부품은 **그대로 가져다 쓴다**(수정 없이 호출).
  - Chain 은 기존 테이블(`TQP_GDS_CHAIN` / `_VER` / `_LAYER` / `_MEMBER`)에 **기존과 같은 형식**으로 저장해 KLARF 연관 분석 / Die 분석 / E-Test Para 가 그대로 읽게 한다.
- 10/19 결정("배열 Net 자동 추출 같은 새 Rule 은 본 프로젝트")은 이번 요청으로 **U8 은 POC 에서 별도 화면으로 진행**하는 것으로 갱신한다.

## 2. 근거 (확인한 사실)

| 항목 | 내용 | 출처 |
| --- | --- | --- |
| U8 구조 | TSV(850 원) - 연결 도형(860 사각) - 끝점(860 원 + 900 원) 한 묶음이 반복. 다이 전체 연결 덩어리 27,413개 | `2026-10-20_U8_같은형태_Chain_자동추출_설계.md` |
| 같은 형태 | 예시 `TSV_21063`(도형 10개)과 같은 형태 **27,341개**, 제품 추적기와 300 / 300 같음, 계산 34초 | 같은 문서 5절 |
| E-Test 연결 | U8 E-Test 샘플 408 항목 = Net **200개**(`VDD_2162`, `VSS_15661` ...). **200개 번호 모두 U8 TSV 번호(`TSV_2162` ...)와 같다**(2026-10-20 확인). 그 200개 TSV 덩어리는 **모두 예시와 같은 형태** | 이 문서 작성 중 확인(샘플 xlsx 항목 이름 + DB Label) |
| GDS Net Label | TSV 자리마다 Net 이름 Label(50/4, 18/4: `VSS`, `VDD` ...) + TSV 번호 Label(50/3: `TSV_nnnnn`). 지금 DB 의 U8 Map 에는 **50/3 만** 저장됨 | `2026-10-20_U8_Label_표시크기_설계.md` 11.2절 |
| 가설 | E-Test Net 이름 = GDS Net Label + `_` + TSV 번호 (예 `VDD` + `2162` -> `VDD_2162`). 번호 일치는 200 / 200 확인, Net Label 부분은 50/4 저장 후 확인 | - |
| 기존 화면 부담 | 기존 GDS Map View 는 Chain 1개마다 서버 호출 2번(포함 도형 / 편집 기록)으로 읽는다 -> Chain 2.7만 개면 U8 을 기존 화면에서 열 때 매우 느려진다. KLARF 연관 분석은 Device 전체를 한 번에 읽는다 | `GdsMapViewForm_ChainLoad.cs`, `GdsKlarfAnalysis.cs` |

## 3. 기능 범위

### 3.1 넣는 기능

| 구분 | 기능 | 비고 |
| --- | --- | --- |
| 조회 | Device(로그인 Factory) / [조회] / [중지] / 보기 기준(Front / Back) / 진행 표시 / 상태 줄 / 좌표 | 기존 화면과 같은 방식(Layer 별 1만 개 페이지) |
| Layer | Layer 목록(체크 박스로만 체크, 색 견본, Layer 키 `850/0` 형식) / 전체 체크 / 전체 해제 | 공용 `LayerColorCheckedListBox` |
| Label | Label 검색(포함, 대소문자 무시) / 결과로 이동 | TSV 번호 / Net 이름 찾기 |
| **예시 지정** | 지도에서 도형 하나 클릭 -> 체크된 Layer 에서 **그 도형의 연결 덩어리**를 예시로 강조, 이름 미리보기 | U9 의 Input / Output / 경로 대신 "덩어리 한 번 클릭" |
| **같은 형태 찾기** | 도면 전체 연결 덩어리 -> 예시와 비교(구성 + 상대 위치, 8 방향) -> 분류(일치 / 회전·대칭 / 검토) | Probe 로직을 화면 기능으로 옮김. 백그라운드 + 진행률 + 중지 |
| **찾기 결과** | 요약(일치 n / 검토 m) / 목록(이름 / 분류 / DB 상태 / 위치) / 검색 / 행 선택 -> 지도 이동 + 강조 | 2.7만 행이라 가상 목록(DataGridView VirtualMode) |
| **Chain 생성(DB)** | 범위(D2) 안의 일치 덩어리를 Chain 으로 **일괄 저장** | 신규 일괄 저장 서비스 |
| **저장된 Chain** | DB 의 U8 Chain 목록(가상 목록, 검색) / 선택 Chain 지도 강조 / 선택 삭제 / 이 화면에서 만든 Chain 전체 삭제 / '표시' 체크 칸 + [전체 체크] / [전체 미체크](체크한 Chain 은 고르지 않아도 지도에 그림, 2026-10-21 추가) | Chain 이름 / 색 수정은 넣지 않음 |

### 3.2 빼는 기능 (U9 Chain 연결 알고리즘)

Input 지정 / Output 지정 / 선택 적용 · 취소 / 후보 경로 찾기 / 셀 크기 / (U9 의미의) 예시 묶음 지정 / 유사 묶음 찾기 · 적용 · 되돌리기 / 도형 추가 · 제외 / 연결 경고 / 단자 Marker 편집 / Chain 1개 저장(DB) / 새 Chain · 이름 수정 · 색 변경 / Map 복사 · Map 삭제(기존 화면에서 한다, D7).

## 4. 화면 배치 (개념)

```
┌ 도구 막대 ───────────────────────────────────────────────────────────────────────────┐
│ Device [U8 ▾] [조회] [중지] │ 보기 [Front side(원본) ▾] │ Label [________] [검색] [전체 보기]  │
├ U8 작업 줄 (도면 조회 후) ──────────────────────────────────────────────────────────┤
│ 예시: TSV_21063 / 도형 10  [예시 지정] [같은 형태 찾기] [중지] │ 범위 [E-Test Net ▾] [Chain 생성(DB)] │
├──────────────────────────────────────────────────────┬──────────────────────────────┤
│                                                      │ [찾기 결과] [저장된 Chain] [Layer] │
│                                                      │ 일치 27,341 / 검토 72           │
│                    도면 (GdsMapControl)                │ 찾기 [________] 분류 [전체 ▾]    │
│        예시 = 노란 외곽선 / 선택 결과 = 흰 외곽선         │ 이름        분류   DB    위치     │
│                                                      │ VDD_2162    일치   새로  (..)    │
│                                                      │ ...  (가상 목록)                 │
├──────────────────────────────────────────────────────┴──────────────────────────────┤
│ 진행률 │ 상태 │ 좌표                                                                    │
└────────────────────────────────────────────────────────────────────────────────────┘
```

- 작업 순서: [조회] -> [예시 지정] 후 도형 클릭 -> [같은 형태 찾기] -> 결과 확인(검토 목록은 위치로 이동해 봄) -> 범위 선택 -> [Chain 생성(DB)] -> [저장된 Chain] 탭에서 확인.
- 강조 표시는 공용 지도 기능만 쓴다: 예시 `SetChainExampleElements`, 선택 결과 `HighlightChainElements` / `SetVisibleChainOverlays`, 이동 `FocusChainBounds`, 클릭 도형 `GetChainElementCandidates`. 지도 컨트롤 수정 없음.

## 5. 알고리즘 (화면 기능)

1. **덩어리 계산(예시 지정)**: 클릭한 도형에서 체크된 Layer 의 도형만 대상으로 공간 격자 + 실제 접촉(`ChainGeometryOverlap.Intersects`) BFS -> 연결 덩어리. 이름 = 덩어리 안 Label(D3 규칙).
2. **같은 형태 찾기**: 체크된 Layer 도형 전체를 100 µm 격자에 넣고 Union-Find 로 모든 덩어리 -> Label 붙이기 -> 예시와 비교.
   - 구성: Layer / 종류 / 꼭짓점 수 / 크기(5 µm 묶음) 목록이 같은지.
   - 배치: 가장 드문 도형을 기준점으로 모든 도형 상대 위치가 0.5 µm 안(8 방향 변형 포함).
   - 분류: 일치 / 회전·대칭 일치 / 검토(구성 같고 배치 다름, 도형 더 많음 = 이웃과 붙음, 도형 더 적음, Label 0개 또는 2개 이상).
3. 도형 데이터는 [조회]로 이미 받은 화면 도형(`MapPlacedElementData`)을 그대로 쓴다(서버 다시 조회 없음). U8 은 탐색 Layer 3개 + Label Layer 만 조회하면 된다.
4. 계산 시간(Probe 기준): 연결 22.5초. 화면에서는 진행률 + 중지. 필요하면 다음 단계에서 원 / 사각 전용 빠른 접촉 판정 검토.

## 6. Chain 저장 형식 (기존 테이블, 기존 화면이 읽는 형식)

| 테이블 | 값 |
| --- | --- |
| CHAIN | FACTORY / DEVICE / 새 CHAIN_SEQ / **CHAIN_CODE = TSV 번호 Label(`TSV_2162`, D4)** / ACTIVE |
| CHAIN_VER | VERSION 1 / CHAIN_NAME = D3 규칙 이름 / 색 = 한 번의 [Chain 생성(DB)](같은 Rule) 전체에 한 색(이미 저장된 Chain 색 / 지도 Layer 색과 가장 먼 색, 흰색 계열 제외, 2026-10-21 변경 - 이전에는 Chain 마다 추천 색을 차례로) / APPLY_LAYER_RULES 0 / CELL_SIZE 10 / READY / IS_CURRENT |
| CHAIN_LAYER | 예시 지정 때 체크된 탐색 Layer(번호 + DataType) |
| CHAIN_MEMBER | Label 이 든 도형 = `INPUT` / `ENDPOINT` + Marker(Label 위치, 저장 기준 X), 나머지 = `MEMBER` / `TRACE` |
| CHAIN_RULE / CHAIN_EDIT | 없음 |

- 기존 화면에서 만든 Chain 과 구별 없이 읽힌다(KLARF 연관 분석 / Die 분석 / E-Test Para GDS Chain 목록).
- 같은 Device 에 같은 CODE 가 있으면 D5 에 따라 건너뛰거나 바꾼다.

## 7. 일괄 저장 서비스 (신규, DefectDefine 방식)

지금 Chain 저장은 1개당 서버 호출 3번(시작 / 포함 도형 / 확정) -> 200개는 600번, 2.7만 개는 8만 번이라 일괄 저장이 필요하다.

| 계층 | 추가 |
| --- | --- |
| Interface `iGdsChain` / RO / BSL `GdsChain` | `CreateChainBulk(string[] aKey, string[,] aChains, string[,] aLayers, string[,] aMembers)` -> 행: CHAIN_CODE / CHAIN_SEQ / RESULT(CREATED / SKIPPED). `GetChainCodeList(factory, device)`(CODE / SEQ / 이름 / 상태, 포함 도형 수 계산 없음 - 2.7만 행에서도 빠르게). `DeleteChainBulk(factory, device, string[] aChainSeqs)` |
| BSL 처리 | 입력 검사 -> 같은 CODE 확인(D5) -> CHAIN_SEQ n개 한 번에 채번 -> CHAIN / VER(DRAFT) / LAYER / MEMBER 를 `InsertExecuteMultiple` (1만 행씩) -> VER READY 일괄 변경. 중간 실패면 이번 호출에서 만든 SEQ 를 지우고 오류 |
| 호출 단위 | 화면이 포함 도형 1만 행 이하가 되도록 Chain 을 나눠 보냄(U8 은 약 900 Chain / 호출) -> 200개 = 1번, 2.7만 개 = 약 30번 |
| XML | `TQP_GDS_CHAIN.xml`(SEQ 여러 개 채번, CODE 목록 조회), `TQP_GDS_CHAIN_VER.xml`(READY 일괄 변경) 쿼리 추가만. 기존 `CREATE_*` / `DELETE_CHAIN_ONE` 은 그대로 재사용 |

## 8. 파일 (예정)

| 구분 | 파일 | 내용 |
| --- | --- | --- |
| DMS 호스트 | 신규 `ENGUI/GdsMap/frmGdsMapViewU8.cs` + `.Designer.cs`, ENGUI csproj 2행 | `frmGdsMapView` 와 같은 방식으로 GdsMap EXE 의 `GdsMapViewU8Form(true)` 를 붙임 |
| 메뉴 | `01_WinApp/DACruxV5/MenuFile.mnu` 1행(끝에 추가) | `MNU_DMS_GDS_MAP_VIEW_U8` / `GDS Map View U8` / `GDS Map 조회 U8` / `F03128` / 순서 D1 |
| 화면 | 신규 `GdsMapControl/GdsMapViewU8Form.cs` + `.Designer.cs` + 부분 파일(`_Load` 조회 / `_Template` 예시 / `_Extract` 같은 형태 / `_ChainDb` 저장·목록) | Designer 배치, 기존 `GdsMapViewForm` 코드는 복사하지 않고 필요한 조회 흐름만 새로 작성 |
| 알고리즘 | 신규 `GdsMapControl/Chain/ChainComponentExtractor.cs`(격자 + Union-Find + 덩어리 BFS), `ChainShapeMatcher.cs`(구성 / 배치 비교, 8 방향), `ChainNameRule.cs`(D3) | Probe `GdsChainAutoExtractProbe` 로직을 정리해 옮김 |
| 서버 | `Interface/iGdsChain.cs`, `RO/GdsChain.cs`, `BSL/GdsChain.cs`(메서드 추가만), XML 2개(쿼리 추가만) | 7절 |
| 검사 | 신규 `Probes/GdsChainBulkProbe.ps1`(일괄 저장 / 건너뜀 / 실패 정리 / 삭제), `Probes/GdsMapViewU8Smoke.ps1`(화면: 빠진 U9 버튼 없음 / 예시 지정 / 찾기 결과 = Probe 와 같은 27,341 / 생성 / 삭제), `GdsChainAutoExtractProbe` 는 알고리즘 클래스 호출로 바꿔 화면과 같은 코드 검증 | - |
| 고치지 않음 | `GdsMapViewForm*`, `GdsMapControl*`, `frmGdsMapView`, 기존 BSL / XML 쿼리 | - |

## 9. 결정할 것 (권장안)

| 번호 | 항목 | 권장 | 다른 안 |
| --- | --- | --- | --- |
| D1 | 메뉴 위치 | **GDS Map 조회 바로 아래**(순서 2, 이후 항목 순서 +1) | 맨 끝(순서 12) |
| D2 | 생성 범위 기본값 | **E-Test 측정 Net**(E-Test Para 에 등록된 U8 Parameter 의 Net 또는 붙여넣은 Net 목록, 샘플 기준 200개) | 전체 일치 2.7만 개(기존 GDS Map View 에서 U8 열기 / E-Test Para GDS Chain 목록이 매우 느려짐) / 화면 영역 |
| D3 | Chain 이름 | **E-Test 형식 `<Net>_<TSV 번호>`**(예 `VDD_2162`): Net = 같은 자리 Net Label(50/4). U8 Map 을 50/4 포함해 다시 저장해야 함 | E-Test 목록 이름을 번호로 연결 / TSV 번호 그대로(`TSV_2162`) |
| D4 | Chain Code | **TSV 번호 Label**(`TSV_2162`, Device 안에서 유일 -> 다시 만들 때 같은 TSV 판별) | 자동 번호 |
| D5 | 같은 Code 가 이미 있음 | **건너뜀**(결과에 "DB 있음") | 지우고 다시 만듦 |
| D6 | 검토 대상(72개) | **생성 대상에서 빼고 목록에만 표시**. 특수 형태 64개는 그 덩어리를 예시로 다시 찾으면 같은 방식으로 생성 | 검토 대상도 생성 |
| D7 | Map 복사 / 삭제 | **U8 화면에 넣지 않음**(기존 GDS Map View 에서) | 넣음 |
| D8 | 조회 Layer | **예시 / 탐색 Layer + Label Layer 만 조회**(U8: 850 / 860 / 900 + 50/3, 50/4) | 기존처럼 체크한 Layer 전부 |

## 10. 구현 단계 (작은 단위)

1. 화면 뼈대: 호스트 + 메뉴 + `GdsMapViewU8Form` 조회 / Layer / 보기 / Label 검색. Smoke(빠진 U9 기능 없음).
2. 알고리즘 클래스 + 예시 지정 + 같은 형태 찾기 + 찾기 결과 목록. Probe 와 같은 수(27,341)인지 확인.
3. 일괄 저장 서비스 + Probe -> [Chain 생성(DB)] + 저장된 Chain 목록 / 삭제.
4. 연계 확인: E-Test Net 200개 Chain 기준으로 KLARF 연관 분석 / Die 분석 / E-Test Para GDS Chain / 기존 GDS Map View 에서 U8 열기.

- 앞선 준비(사용자): D3 를 권장안으로 하면 U8 Map 을 50/4 Layer 포함해 다시 저장(GDS Map Setup).
- 10/21 회의 확인: E-Test Net 이름 = Net Label + TSV 번호 규칙, U8 셋업 Net 범위.

## 11. 구현 결과 (2026-10-20)

### 11.1 파일

| 구분 | 파일 | 비고 |
| --- | --- | --- |
| 화면 | `GdsMapControl/GdsMapViewU8Form.cs` / `.Designer.cs` / `.resx` / `_Label.cs` / `_Template.cs` / `_Extract.cs` / `_ChainDb.cs` | 8절의 `_Load` 는 본체 파일에 넣음 |
| 창 | `GdsMapControl/Chain/ChainNetListForm.cs` / `.Designer.cs` / `.resx` | E-Test Net 목록 붙여넣기(D2) |
| 알고리즘 | `Chain/ChainComponentExtractor.cs` / `ChainShapeTemplate.cs` / `ChainNetNameRule.cs` | 8절 예정 이름 `ChainShapeMatcher` / `ChainNameRule` 대신 이 이름 |
| 서버 | Interface / RO / BSL `GdsChain`, DSL `TQP_GDS_CHAIN` / `TQP_GDS_CHAIN_VER`, XML `TQP_GDS_CHAIN.xml` | 추가만. READY 변경은 기존 `UPDATE_CHAIN_VER_READY` 다건 재사용(VER XML 수정 없음) |
| 호스트 / 메뉴 | ENGUI `GdsMap/frmGdsMapViewU8`, ENGUI csproj 2행, `01_WinApp/DACruxV5/MenuFile.mnu` 1행 + POC 순서 | F03128, 순서 2 |
| 검사 | `Probes/GdsChainBulkProbe.ps1`, `Probes/GdsMapViewU8Smoke.ps1`, `Verify-PocButtonLayout.ps1` 목록 2행 | - |

### 11.2 화면 동작

- 작업 순서: [조회] -> [예시 지정] 후 도형 클릭(Esc 끝) -> [같은 형태 찾기] -> 찾기 결과 확인(분류 "검토" 로 거르기, 행 선택 = Cyan + 이동) -> 범위 / [Net 목록] -> [Chain 생성(DB)] -> [저장된 Chain] 탭.
- 지도 표시: 예시 자주색 / 생성 대상 흰색(5만 도형 이하일 때만) / 선택 행 Cyan / 저장된 Chain 은 Chain 색(선택한 것 최대 20개 + '표시' 체크한 것 전체, 2026-10-21).
- 예시 Layer = 예시를 지정할 때 체크된 도형 Layer. 같은 형태 찾기와 저장 탐색 Layer 가 이 Layer 다.
- 보기 기준 변경 / 다시 조회 -> 예시 / 찾기 결과 지움. DB Chain 은 그대로이고 Marker 는 저장 기준 X 로 저장한다.
- DACrux 메뉴로 열 때만 DB 기능([Chain 생성(DB)] / 저장된 Chain 버튼)을 쓴다.

### 11.3 검증 (Smoke PASS 20, U8 복사본 U8_UI_PROBE)

| 항목 | 결과 |
| --- | --- |
| U9 기능 | Input / Output / 후보 경로 / 도형 추가·제외 / 유사 묶음 / Map 복사·삭제 없음 |
| 조회 | 도형 329,114 / 도형 색인(TEXT 제외) 274,280 / Label 54,834, 16.8초 |
| 예시 지정 | TSV_21063 도형 10, 1.2초(덩어리 계산기 처음 구성 포함) |
| 같은 형태 찾기 | 덩어리 27,413 / 형태 일치 27,341(Probe 와 같음) / 검토 72, 25.0초 |
| 범위 | E-Test Net 3개(도면 2) -> 생성 대상 2, 이름 VDD_2162 / VSS_15661(목록 이름). 전체 일치 = 27,341 |
| 생성 / DB | 2개 2.2초. READY / 도형 10 / INPUT 1(ENDPOINT, Marker = TSV Label 위치) / 규칙 0 / 격자 10 / Layer 850/0 · 860/0 · 900/0 |
| 저장된 Chain | 찾기 / 선택 표시 / 삭제 2 -> 목록 · DB 원래대로 |

### 11.4 남은 일

1. 사용자 배포 후 DMS 화면 확인(POC > GDS Map 조회 U8).
2. D3: U8 Map 을 50/4 포함해 다시 저장하면 이름이 `VDD_2162` 형식이 된다(지금은 E-Test 목록 이름 -> TSV Label).
3. E-Test Para 에 등록된 Net 을 범위로 자동으로 읽기(지금은 [Net 목록] 붙여넣기).
4. 10절 4단계 연계 확인(E-Test Net Chain 기준 KLARF 연관 분석 / Die 분석 / E-Test Para GDS Chain / 기존 GDS Map View 에서 U8 열기).

## 12. 이후 변경

- 2026-10-20 (CLAUDE-168): [Net 목록] / 범위 콤보 / 분류 "E-Test Net" 삭제(D2 대체). [Chain 생성(DB)] 는 찾기 결과 탭으로 옮겨 지금 목록(분류 / 찾기로 거르고 [목록에서 빼기]로 뺀 뒤) 중 일치 + DB 없음만 만든다. 이름 = Net Label + 번호, 없으면 TSV Code(D3 의 E-Test 목록 이름 단계 삭제). 11.4 의 2 / 3 은 이에 맞춰 "Net Label(50/4) 저장 후 이름" 만 남는다.
- 2026-10-21 (CLAUDE-178): 한 번의 [Chain 생성(DB)] 로 만든 Chain 은 한 색(6절 색 규칙).
- 2026-10-21 (CLAUDE-181): 저장된 Chain 목록 "표시" 체크 칸 / [전체 체크] / [전체 미체크].
- 2026-10-21 (CLAUDE-187): 저장된 Chain [색 변경] - 고른 Chain(여러 개 가능)의 색을 DB 에 바로 저장(서버 `UpdateChainColorBulk`, 쿼리 `UPDATE_CHAIN_VER_COLOR`, VERSION 1 의 COLOR_ARGB 만), 목록 "색" 칸.
