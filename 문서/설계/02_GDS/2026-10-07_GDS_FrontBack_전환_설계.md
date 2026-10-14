# GDS Front / Back side 전환 설계 (DB 2벌 저장 vs 로직 반전 비교)

작성: 2026-10-07 Claude (CLAUDE-030) / 상태: **사용자 결정 반영(8.1절) / 구현 1~4단계 완료(9절) / DMS 화면 확인 대기**

이 문서는 [GDS 반전 설계](2026-10-06_GDS_반전_BackSide_설계.md) 8~9절(Setup에서 저장 기준을 골라 PLACED를 반전 저장)을 대체한다.
관련: [화면 역할 재정의 설계](2026-10-06_GDS_Map_화면역할_재정의_설계.md), [Chain DB 저장 설계](2026-10-06_GDS_Chain_DB저장_설계.md), [2026-10-06 데이터기준 협의 회의록](../../../회의록/2026-10-06_데이터기준_협의)

## 1. 요구 (2026-10-07 사용자 지시)

- GDS Map Setup 화면에서 **반전 기능을 제거**한다(GDS 원본 = Front side 그대로 등록).
- GDS Map View 화면에서 Chain까지 저장되면 **Front / Back side 두 가지를 모두 쓸 수 있게** 하고, 분석할 때 엔지니어가 **선택**한다.
- 두 가지를 **DB에 2벌 저장할지, 로직에서 반전할지** 효율을 분석해 설계한다.
- (회의 결정 유지) 좌우 반전만, GDS 원점 기준 X' = -X. KLARF / E-Test는 Back side 기준이고 변환 없이 쓴다.

## 2. 실측 (2026-10-07, 로컬 docker DB / 샘플 GDS)

| 항목 | 값 | 비고 |
| --- | --- | --- |
| PLACED 테이블 | 1,088MB / 약 213만 행(전 Device) -> 행당 약 0.5KB | `dba_segments` |
| PLACED 인덱스 3개 | PK 160MB + IX_GDS_PLACED_SOURCE 160MB + IX_GDS_PLACED_LAYER 152MB | |
| Device 1개(60만 도형) 환산 | PLACED 약 310MB + 인덱스 약 135MB = **약 440MB** | SOURCE(원본 Grid)는 반전과 무관 |
| Chain Member | 전체 2MB(Chain 6개, 약 3.4만 행) | Chain은 도형 ID만 참조 |
| 화면에서 전체 반전 사본 생성 | **0.2초**(605,080 도형 / 좌표 8,292,978) | `VisitPlacedElements(mirrorX)` 측정 |
| 같은 도면 화면 구성(Flatten + GPU 정점) | 4.6초 | 반전 후 GPU 버퍼 다시 만들기의 상한 추정 |
| DB에서 같은 Device 다시 조회 | 약 20~25초(전체 Layer) | CLAUDE-018 측정 |
| Map DB 저장(PLACED) | 35만 행 24.5초(CLAUDE-023 튜닝 후) | 2벌이면 그만큼 추가 |
| 현재 BACK으로 저장된 Map | **SCK / 001, SCK / OMM_Hybrid** (2026-10-06 Setup 저장 기준 콤보) | 나머지 9건 FRONT |

## 3. 두 방식 비교

| 기준 | A. DB에 Front / Back 2벌 저장 | B. DB 1벌 + 로직 반전 (권장) |
| --- | --- | --- |
| 저장 용량 | Device당 PLACED + 인덱스 **약 440MB 추가**(2배) | 추가 없음 |
| 저장 시간 | PLACED 저장 2배(60만 도형 기준 약 40초 추가) | 변화 없음 |
| 스키마 | Map 키가 FACTORY + DEVICE_ID(1:1)라서 **MAP / LAYER / PLACED / CHAIN 6개 테이블 키에 SIDE 추가** 또는 Device 이름 규칙 분리 필요. FK / 인덱스 / 서비스 / Probe 전반 수정 | 변경 없음(기존 `VIEW_SIDE` = 저장된 좌표 기준만 사용) |
| Chain | Front / Back 각각 저장 또는 한쪽 저장 후 복제 -> **수정할 때마다 두 벌 동기화**, 어긋나면 분석 결과가 달라질 위험 | Chain 1벌(도형 ID 기반이라 기준과 무관). Marker 좌표만 표시할 때 변환 |
| 화면 Front / Back 전환 | 다른 Map을 DB에서 다시 조회(약 20초) | 메모리에서 반전 + GPU 버퍼 재생성(**수 초 이내** 추정, 반전 계산 0.2초) |
| KLARF / E-Test 분석 | 분석 기준 쪽 Map을 골라 그대로 비교 | Defect / Die 좌표(점 / 사각형 몇 개)를 X 부호만 바꿔 비교(도형 60만 개를 바꿀 필요 없음). DB 영역 질의도 범위만 [-x2, -x1]로 바꾸면 됨 |
| 외부 SQL 사용자 | Back 좌표를 그대로 조회 | 반전 규칙(X' = -X)을 알아야 함 -> 필요 시 Bounds를 반전한 DB VIEW 추가(POINTS_BIN은 로직에서) |
| 정합성 | 같은 도형이 두 곳에 있어 한쪽만 바뀌는 오류 가능 | 원본 1곳, 변환식 1곳(부호 반전, 두 번 하면 비트 단위로 원본) |
| 구현량 | 큼(스키마 / 서비스 / 저장 / 조회 / Chain 동기화) | 작음(화면 전환 + 좌표 변환 함수 + Chain Marker 변환) |

**결론: B(DB 1벌 + 로직 반전)를 권장한다.** 반전이 부호 하나만 바꾸는 무손실 연산이고(측정 0.2초), 분석에서는 도형이 아니라 Defect / Die 좌표 몇 개만 바꾸면 되므로, 2벌 저장의 용량(Device당 약 440MB) / 저장 시간 / 키 구조 변경 / Chain 동기화 위험을 감수할 이유가 없다.

## 4. 좌표 공간 규칙 (B안)

```text
저장 기준(stored) = TQP_GDS_MAP.VIEW_SIDE (앞으로 Setup 저장은 항상 FRONT. 기존 BACK 2건은 그대로 인정)
보기 / 분석 기준(view) = 엔지니어가 View에서 선택한 FRONT / BACK
반전 필요 = (stored != view)  ->  X' = -X (GDS 원점 기준 좌우 반전, Y 그대로)
```

- DB의 PLACED / Chain Marker는 **항상 저장 기준 좌표**로 둔다. 화면에 보여 줄 때와 분석할 때만 보기 기준으로 바꾼다.
- 변환은 한 곳(`GdsSideTransform`: 점 / Bounds / 점 배열, 새 배열 / 부호만 변경)에 모아 화면 / Chain / 분석이 같은 식을 쓴다(어제 만든 `MirrorPoints` / `MirrorBounds`를 옮겨 사용).
- KLARF / E-Test(Back 기준)를 Front 보기와 비교할 때는 그 좌표를 X 반전한다. 즉 "입력 데이터 기준 != 보기 기준이면 입력 좌표 반전". 다이 인덱스 / Notch 회전 대응은 연구소 좌표 대응 테이블(별도)로 한다.

## 5. 화면 설계

### 5.1 GDS Map Setup (반전 제거)
- 저장 기준 콤보(`lblDbSide` / `cboDbSide`) 삭제. 저장은 항상 원본 좌표(FRONT), `BeginMapImport` 12번째 항목 FRONT(또는 11개 호출).
- 저장용 반전 오버로드(`mirrorX`)는 Setup에서 쓰지 않는다(Probe / 변환 함수로 이동).

### 5.2 GDS Map View (Front / Back 선택)
- 조회 조건 영역에 **[보기 기준: Front side / Back side]** 콤보. 기본값은 8절 결정.
- [조회] 시: DB 좌표(저장 기준)로 그린 뒤 보기 기준과 다르면 화면 도형 전체를 반전(새 배열) -> GPU 버퍼 재생성.
- 조회 후 콤보 변경: DB를 다시 읽지 않고 메모리에서 반전 + GPU 재생성 + Chain 표시 재연결.
- Map 정보: "저장 기준: Front / 보기 기준: Back".
- Chain
  - 탐색 / 겹침 판단은 반전에 대칭이라 어느 보기에서 해도 같은 도형 ID 집합(어제 Probe: Back 저장 Map 탐색 = Front 탐색).
  - Input / Output Marker: 저장은 저장 기준 좌표, 화면은 보기 기준 좌표. 저장 / 불러오기 / 보기 전환 때 변환.
  - 탐색 결과(`LastResult`)에 들어 있는 도형 좌표 사본은 보기 전환 때 도형 ID로 지금 지도 도형에서 다시 만든다(유사 묶음 / 연결 검사가 지금 좌표를 쓰도록).
- (분석, F07 연동 시) 분석 화면 / 서비스에 보기 기준을 인자로 넘기고 4절 규칙을 적용한다.

## 6. 구현 단계 (작은 단위)

| 단계 | 내용 | 확인 |
| --- | --- | --- |
| 1 | Setup 반전 제거(콤보 / 저장 반전 호출 삭제, 항상 FRONT) | 빌드 / BackSide Probe를 FRONT 저장 확인으로 갱신 |
| 2 | `GdsSideTransform` + `GdsMapControl.ApplyViewSide(stored, view)`(화면 도형 / Bounds / TEXT 반전, GPU 재생성, 탐색 캐시 재연결) | Probe: 반전 전후 좌표 / 두 번 전환 = 원본 / 도형 수 / Chain 탐색 집합 동일 / 전환 시간 |
| 3 | View 보기 기준 콤보 + 조회 / 전환 흐름 + Map 정보 | DMS 화면 |
| 4 | Chain Marker 저장 / 불러오기 / 전환 변환 + `LastResult` 좌표 재구성 | Probe: Back 보기에서 저장 -> Front 보기에서 불러오기 Marker 일치(저장 기준 좌표) |
| 5 | 문서 / F07에 좌표 기준 규칙 | — |

수정 예정 파일: `GdsMapForm.Designer.cs` / `GdsMapForm_DbSave.cs`(CLAUDE-028이 어제 17:36 수정, 시작 전 재확인), `Controls/GdsMapControl_Persistence.cs` / 신규 `Controls/GdsMapControl_ViewSide.cs` / 신규 `Persistence/GdsSideTransform.cs`, `GdsMapViewForm.cs` / `.Designer.cs` / `GdsMapViewForm_Chain*.cs`(Marker 변환), Probe. 서버 / DB 변경 없음.

## 7. 기존 데이터

- FRONT 9건: 그대로.
- BACK 2건(SCK / 001, SCK / OMM_Hybrid): 4절 규칙(stored = BACK)으로 그대로 Front / Back 둘 다 볼 수 있다. 이 Map에 이미 저장된 Chain Marker는 BACK 좌표이므로 같은 규칙으로 맞는다. 원본과 같게 정리하려면 Front로 삭제 후 재등록(Chain도 삭제) — 8절 결정 3.

## 8. 결정이 필요한 사항

| # | 질문 | 권장 | 다른 안 |
| --- | --- | --- | --- |
| 1 | 저장 방식 | **B. DB 1벌(원본 Front) + 로직 반전** (3절) | A. DB에 Front / Back 2벌(용량 2배 / 키 변경 / Chain 동기화) |
| 2 | View 보기 기준 기본값 | **Back side**(KLARF / E-Test가 Back) | Front side / 마지막 선택 기억 |
| 3 | 기존 BACK 저장 Map 2건 | **그대로 둔다**(VIEW_SIDE로 구분해 정상 동작) | Front로 삭제 후 재등록(Chain 삭제) |
| 4 | `TQP_GDS_MAP.VIEW_SIDE` 컬럼 | **유지**(= 저장된 좌표 기준. 앞으로 항상 FRONT, 기존 BACK 2건 구분에 필요) | 삭제(BACK 2건을 먼저 재등록해야 함) |
| 5 | 외부 SQL용 Back 좌표 DB VIEW | **지금은 만들지 않음**(필요해지면 Bounds 반전 VIEW) | 지금 생성 |

### 8.1 2026-10-07 사용자 결정 (최신 기준)

"View 보기 기준 기본값은 원본대로 front side로 간다. 기존 data들은 전부 삭제하고 처음부터 다시 만들자. 나머지는 권장안대로."

| # | 확정 |
| --- | --- |
| 1 | **B. DB 1벌(원본 Front) + 로직 반전** |
| 2 | View 보기 기준 기본값 **Front side**(원본) |
| 3 | **기존 GDS 데이터(Map 11건 + Chain 등) 전부 삭제 후 다시 등록**(로컬 시험 DB). 이후 모든 Map은 VIEW_SIDE = FRONT |
| 4 | `VIEW_SIDE` 컬럼 유지(저장된 좌표 기준, 4절 규칙의 stored) |
| 5 | 외부 SQL용 Back 좌표 DB VIEW는 지금 만들지 않음 |

## 9. 2026-10-07 / 구현 (CLAUDE-030)

**1단계 Setup 반전 제거**: `GdsMapForm.Designer.cs`에서 `lblDbSide` / `cboDbSide` 삭제. `GdsMapForm_DbSave.cs`에서 저장 기준 선택 / 반전 추출 / 확인창·완료 문구의 저장 기준 삭제. 저장은 원본 좌표, `BeginMapImport` 12번째 항목 `"FRONT"` 고정. (`GdsMapControl_Persistence.cs`의 `mirrorX` 오버로드와 서버의 BACK 허용은 그대로 둔다 - 쓰는 곳 없음, 서버 호환 유지)

**2단계 변환 함수**: 신규 `Persistence/GdsSideTransform.cs`(NeedsMirror / Normalize / Text / MirrorX / MirrorPoint / MirrorPoints / MirrorBounds / MirrorData / MirrorAll). `GdsMapControl_Persistence.cs`의 MirrorPoints / MirrorBounds는 이 클래스로 위임. 화면 전환은 `GdsMapControl`을 고치지 않고 **화면 도형 목록을 반전한 사본으로 `ShowPlacedElements`를 다시 호출**하는 방식(TEXT 라벨 / GPU / Layer 목록을 기존 코드로 다시 만든다).

**3단계 View 보기 기준**: `GdsMapViewForm.Designer.cs` 조건 영역에 `lblViewSide` / `cboViewSide`("Front side(원본)" / "Back side(좌우 반전)", 기본 Front). `GdsMapViewForm.cs`
- [조회]: DB 좌표(저장 기준) -> 보기 기준과 다르면 `MirrorAll` -> 그리기. 화면 도형 목록 / 색상 / 보기 기준을 보관.
- 콤보 변경(도면 있음): `CaptureActiveChain` -> 목록 반전 -> `ShowPlacedElements` -> Layer 표시 체크 다시 적용 -> `RebuildChainTraceItems` -> `MirrorChainsForSideChange` -> `ReattachChainsAfterMapReload`. DB를 다시 읽지 않는다. 도면 없으면 다음 [조회]에 적용.
- Map 정보: "저장 기준 / 보기 기준". 조회 / Chain 저장 중에는 콤보 비활성.

**4단계 Chain 좌표** (신규 `GdsMapViewForm_ChainSide.cs`)
- 저장 행 / 저장 지문의 Marker X는 `ToStoredX`(저장 기준 좌표) -> 보기 기준만 바꿔도 "변경 없음" 유지.
- 불러오기 Marker는 `ToDisplayX`(GPoint 생성자 반올림을 피해 필드에 직접).
- 보기 전환 시 모든 Chain의 Marker / 단자 후보 Bounds / 탐색 결과(경로 / 방문 / 겹침 / 분기 / 단자) 좌표를 **ID로 다시 찾지 않고 좌표만 반전**(조회하지 않은 Layer 도형도 Member로 남게).

**기존 데이터**: 로컬 시험 DB의 GDS Map 11건 / Chain 6건을 BSL `DeleteMap`으로 전부 삭제(사용자 결정 3).

**검증 (2026-10-07)**
- 기존 데이터 삭제: Map 11건(VIEW_DRAFT 0.5초 ~ TEST_DEVICE 60만 도형 236초), 삭제 후 MAP / PLACED / SOURCE / LAYER / CHAIN / MEMBER / IMPORT_JOB 모두 0건.
- 시험 Map 재생성: `GdsImportLayerFilterProbe.ps1 -Keep` -> PROBE / LAYER_FILTER_PROBE(Layer 1·2·6, FRONT) PASS.
- 빌드: GdsMap TEMP(`%TEMP%\ChippacBuild\CLAUDE-030_111154`) 성공, 경고 기존 1건.
- 신규 `Probes/GdsFrontBackViewProbe.ps1` 5 PASS: 변환식(29,871 도형 / 169,875 좌표 두 번 반전 불일치 0, X 불일치 0, NeedsMirror 규칙) / Back 보기 Chain 탐색 = Front(11) / 반전 + 다시 그리기 179ms / Front 저장 Chain을 Back 보기에서 불러오면 Marker X = -1234.56789, 탐색 결과 좌표 = 화면 도형 / 저장 행 Marker = 1234.56789(저장 기준) + 변경 없음 / 보기 전환 후 Marker = 저장값, 저장 행·지문 동일.
- 회귀: GdsChainProbe 14 / GdsChainLoadProbe 6 / GdsChainDbTraceProbe 6 PASS.
- 반영: 11:23 GdsMap EXE `c03d6c8e` -> `280a005a`(반영 전 MD5 = CLAUDE-028 기록). 서버 / DB 변경 없음.
- 미확인: DMS 화면(콤보 배치 / 전환 속도(60만 도형) / 전환 후 Layer 표시 / Chain 편집·저장), Service Remoting.
