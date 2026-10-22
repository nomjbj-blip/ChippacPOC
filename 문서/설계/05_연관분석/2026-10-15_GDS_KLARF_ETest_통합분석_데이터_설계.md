# GDS × KLARF × E-Test 통합 연관 분석 데이터 설계

작성: 2026-10-15 Claude (CLAUDE-119) / 상태: **결정 확정 (2026-10-16 "설계안 대로 작업 시작" = B1 ~ B20 권장안 전부, B21 / B22 추가)** / **B23 ~ B25 결정 대기 (20절, 2026-10-16 추가)** / **B26 / B27 확정 + 코드 반영 (22절, 배포 대기)** / 2026-10-15 Wafer 중심 Ready 컨셉으로 변경 (16절) / **1차 화면 확정(17 ~ 19절) / 서버 구현 + 화면 연결 완료, 배포 전(21절, 2026-10-16)** / **2026-10-20 B16(여러 KLARF Step) = 2차로 되돌림: 화면은 Wafer 1개 · 고른 Step 1개만 분석, 서버 여러 Step 기능은 유지** / **2026-10-20 B28: KLARF 검사 면 = FS 고정(화면에 보이지 않음), 고르는 것은 GDS Chain 면만(기본 BS) -> 기본 조건 반전, KLARF / E-Test Die 좌표는 그대로 (18절)**

사용자 지시 (2026-10-15):

> GDS, Klarf, E TEST 결과가 다 있어야 연관 분석이 가능하니 각각의 Data를 어떤 방식으로 불러 와서 분석하는게 좋을지 잘 고민 해봐
> GDS는 element의 폭도 잘 계산 해야 하고 Klarf는 Defect의 위치에서 사각형으로 얼마나 Tol을 줄지 고민해야 한다.
> Etest 결과에 대한 부분도 chain에 대해 Mapping 설계가 되었으니 어떻게 연동해서 보여 줄지...
> Die 개수가 많으면 많을 수록 리소스가 많이 먹을 것으로 예상이 된다. 해당 부분 잘 고민 해서 설계 해줘

앞 문서: [GDS × KLARF 연관 분석 화면 설계 (1차)](2026-10-12_GDS_KLARF_연관분석_화면_설계.md) (화면 A `frmGdsKlarfAnalysis` / 화면 B `GdsDieAnalysisForm`, 구현됨)
근거 문서: [Die 좌표 대응 설계 9절](../03_KLARF_Map/2026-10-13_Die좌표_대응_Setup_설계.md) / [E-Test Para Setup 설계](../04_ETest/2026-10-14_ETest_Para_Setup_설계.md) / [E-Test Para 화면 재구성](../04_ETest/2026-10-14_ETest_Para_화면_재구성_설계.md) / [E-Test Upload 재설계](../04_ETest/2026-10-13_ETest_Upload_재설계.md) / [GDS Map DB 저장 설계 13절](../02_GDS/2026-10-02_GDS_Map_DB저장_설계.md) / [Chain Setup 컨셉안 10.2 / 17.8](../02_GDS/2026-09-23_Chain_Setup_기능_컨셉안.md)

---

## 0. 결론 요약

1. **GDS 도형은 Chip 1개 기준이라 Die 수와 상관이 없다.** Chain Member 도형을 Device마다 한 번 읽어 서버 메모리에 색인(Cache)하고, 모든 Die의 Defect를 Chip 좌표로 옮겨 같은 색인에서 찾는다. Die가 76개든 3,000개든 GDS 쪽 비용은 같다.
2. **Element 폭**: 저장된 `MIN_X ~ MAX_Y`는 PATH 폭의 절반까지 넓힌 값이라 1차 후보 거르기에 그대로 쓴다. 2차로 실제 모양과의 거리를 계산한다. 실제 모양은 BOUNDARY 다각형, 그리고 PATH의 선분별 폭 사각형 + 끝 처리(`PATH_TYPE`) + 꺾임 메움이다. 화면 쪽 `ChainGeometryOverlap` 계산을 서버(.NET 4.0)로 옮겨 쓴다.
3. **허용오차는 "걸림 여부"가 아니라 "거리(Gap, µm)"로 계산한다.** Defect 사각형(XSIZE × YSIZE)과 Chain 도형 사이 거리가 허용오차 이하면 걸림이다.
   - 기본값은 5 µm를 유지한다. 근거는 AOI 화소 3.23 µm 등 무작위 위치 오차의 합 약 4.4 µm다.
   - Scribe / Seal ring 위치 같은 한 방향 오차(최대 약 ±36 µm)는 허용오차로 덮지 않고 보정 X / Y로 맞춘다.
   - 허용오차 0 / 2 / 5 / 10 / 20 / 50 µm별 걸림 수 표를 함께 보여 엔지니어가 값을 정하게 한다.
4. **E-Test는 Wafer 1장의 (Die, GDS Chain) 판정만 서버 SQL로 묶어 가져온다.** 대상은 `GDS_CHAIN_NAME`을 넣은 Parameter뿐이다. 측정값 전체는 Die 상세를 열 때 그 Die 것만 읽는다.
5. **세 데이터를 잇는 키는 (시스템 Die 좌표, Chain 이름)이다.**
   - KLARF Index는 보기 기준(Back / Front)에 따라 시스템 좌표로 바꾼다. E-Test는 저장된 `MAP_X / MAP_Y`를 쓴다.
   - Chain은 `CHAIN_SEQ`가 아니라 이름으로 잇는다. Map 복사로 Device마다 `CHAIN_SEQ`가 다르기 때문이다.
6. **Wafer 중심으로 모은다 (16절).**
   - DMS 조회 화면에서 Wafer를 고르면 Wafer Key와 Device로 세 데이터를 자동으로 모아 준비(Ready) 상태를 만든다.
     - KLARF: 같은 Wafer의 모든 Step
     - E-Test: 같은 Wafer의 공정별 파일
     - GDS: Device의 READY Map / Chain
   - Ready 단계는 목록만 가볍게 읽는다. 무거운 데이터는 [분석] 때만 읽는다.
   - Wafer Key 규칙은 고객사와 협의해 정한다. 규칙은 `WaferKeyRule` 한 파일에만 두고, 그 전까지는 잠정 규칙(Lot 핵심 + Slot)을 쓴다.
   - 규칙으로 못 찾는 데이터는 [직접 찾기]로 더한다.
7. **(Die, Chain)을 4가지로 나눈다**: 일치(Defect + Fail) / Fail만 / Defect만 / 정상. Chain 탭에는 "Defect 있는 Die의 Fail률"과 "Defect 없는 Die의 Fail률"을 열로 보인다.
8. **리소스 대책**: 서버 Cache, 집계 위주 결과(Member 단위 행 없앰), Die 상세는 열 때 따로 계산, DataSet Binary 전송, Map 칸이 작으면 숫자 생략.
9. **E-Test Fail 역추적 (15절, 추가)**: E-Test Fail (Die, Chain) 목록에서 출발한다.
   - 그 Chain에 가장 가까운 Defect의 거리를 허용오차 밖까지(탐색 거리 50 µm) 보인다.
   - 행을 고르면 화면 B가 그 Chain만 강조해 연다.
   - 여러 KLARF Step은 Wafer 중심 컨셉으로 1차에 포함한다(16절). 2 Chain 항목의 Short 후보 찾기는 2차로 둔다.

## 1. 지금 상태 (근거)

| 데이터 | 저장 위치 (확인) | 지금 연관 분석이 쓰는 것 | 없는 것 |
| --- | --- | --- | --- |
| GDS | `TQP_GDS_PLACED_EL`(`POINTS_BIN` / `MIN_X ~ MAX_Y` / `PATH_WIDTH` / `PATH_TYPE`), `TQP_GDS_CHAIN` / `_CHAIN_VER` / `_CHAIN_MEMBER` | `SELECT_CHAIN_MEMBER_BOUNDS_ALL`로 Bounds만. 분석마다 DB에서 읽음 | 실제 모양 판정, Cache |
| KLARF | `TQD_DEFECT`(XINDEX / YINDEX = Map Setup 원점 기준, XREL / YREL, XSIZE / YSIZE, CLASSNUMBER, ROUGHBINNUMBER), `TQD_SETUP`(Pitch = Map Setup 값) | `GET_DEFECT`(28 컬럼 전체), Class 필터 = CLASSNUMBER | KLARF 원본 Pitch, RoughBin 필터 |
| E-Test | `TQP_GDS_SRC_FILE` / `_ET_DIE`(MAP_X / MAP_Y, JUDGE_FLAG) / `_ET_VALUE`(JUDGE_FLAG) / `_ET_ITEM`(GDS_CHAIN_NAME) / `_ET_LIMIT` | 연결 없음 | Wafer 연결, (Die, Chain) 집계, Chain 이름 해석 |
| Die 좌표 | `Interface/Coordinate/DieCoordinateMapper.cs` (Probe 11/11) | E-Test 업로드만 사용 | KLARF 쪽 적용 |

기록된 크기 / 속도:

- U9(SCK) 도면은 도형 204,898개, Chain 15개, Member 66,489개다. Member Bounds는 폭 중앙값 104 µm(최대 1,325), 높이 중앙값 71 µm(최대 155)다 (변경이력 10-13 / 10-15).
- 지금 판정 속도는 Member 66,489개 기준으로 Defect 1천 11 ~ 15 ms, 1만 29 ~ 64 ms, 10만 134 ~ 576 ms다. Member Bounds 쿼리는 약 1.5 s다 (2026-10-13 측정).
- E-Test는 24 Die × 37 Parameter = 888값(RH-HS2615-02), 73 Die × 109 = 7,957값(U9-HS2635-01)이다. 파일 헤더 2,755개 중 등록 Parameter만 저장한다.
- KLARF 샘플 3개의 Defect 수는 15 / 1,122 / 109개다.
  - XSIZE 중앙값은 6.5 ~ 16 µm, 최댓값은 5,043 µm다.
  - 크기 값이 3.23 µm(C4Bump) / 1.29 µm(TBDB) 간격으로만 나온다. AOI 화소 크기로 본다.

## 2. 설계 원칙

| 원칙 | 내용 | 이유 |
| --- | --- | --- |
| P1 Chip 기준 색인 | GDS 도형은 Chip 1개 좌표계에서 한 번만 색인하고, Defect를 Chip 좌표로 옮겨 찾는다 | 모든 Die가 같은 도면이다. Die 수와 GDS 비용을 떼어 놓는다 |
| P2 계산은 서버, 화면은 집계 | Wafer 화면은 Die / (Die, Chain) / Chain 집계만 받는다. Member 단위는 Die 상세에서 그 Die만 | 결과 행이 Defect × Member로 커지는 것을 막는다 |
| P3 거리로 판정 | 걸림 = Gap ≤ 허용오차 | 허용오차의 영향을 표로 보고, Defect마다 얼마나 가까운지 본다 |
| P4 이름 / 시스템 좌표로 연결 | Die = 시스템 Die 좌표, Chain = Chain 이름 | Device마다 Map / `CHAIN_SEQ`가 따로다 |
| P5 저장 안 함 | 결과 테이블 없음(A7 유지). E-Test 판정은 저장된 `JUDGE_FLAG` 사용 | DDL이 없고, 판정 규칙이 한 곳에 있다 |
| P6 모으는 건 자동, 고르는 건 사람 | KLARF Step / E-Test 파일은 Wafer Key로 자동으로 모아 표로 보인다. 쓸지는 엔지니어가 체크로 정한다. 보기 기준 / 보정 X / Y / 허용오차는 엔지니어가 입력 | 무엇이 묶였는지 표로 보이고 바꿀 수 있어야 POC 단순 원칙에 맞다 |
| P7 Wafer 규칙은 한 곳 | Wafer Key 규칙은 `WaferKeyRule` 한 파일에만 둔다 | 고객 협의로 규칙이 바뀌어도 그 파일만 고친다 |

## 3. 전체 흐름

```text
[화면 A] Wafer 선택(DMSearch)
   |
   v  GetWaferReady(factory, stepSeq)   (목록만, 16.4)
 0) Wafer Key 계산 -> 같은 Key의 KLARF Step 목록 / E-Test 파일 목록 / GDS Device 상태 / 점검
   |
   v  [준비] 탭: Step 체크 + Step별 보기 기준 / E-Test 공정 선택 / 주의 항목 -> 허용오차 / 보정 / 분류 -> [분석]
   |
   v  Analyze(request)   (서버 BSL GdsKlarfAnalysis)
 1) GDS    : Map 정보 + Chain 목록 -> Cache 확인(저장 시각) -> 없거나 바뀌었으면 Member 모양을 한 번 읽고 색인
 2) KLARF  : 체크한 Step마다 Defect(필요 컬럼만) -> Step의 Pitch / 보기 기준으로 Chip 좌표 + 시스템 Die 좌표 -> 합쳐서 1차 Bounds 후보 -> 2차 모양 거리(Gap)
 3) E-Test : (Die, GDS Chain 이름) 판정 SQL 집계 -> 이름 -> 이 Device의 CHAIN_SEQ -> 시스템 Die 좌표로 KLARF Die와 맞춤
 4) 합치기 : DIE / CHAIN / DIE_CHAIN(4분류) / DEFECT(Chain 영역만) / DEFECT_CHAIN / TOL_CURVE / ET_LINK
   |
   v  DataSet (Binary 전송)
[화면 A] 색 기준 콤보 / E-Test Fail Die 테두리 / 탭 표
   |  Die 선택 -> Detail Analysis
   v  AnalyzeDie(request, x, y)   (같은 Cache, Die 1개)
[화면 B] 그 Die의 Defect 전체 + Member 단위 걸림 + E-Test 값 -> 도면 위 표시
```

## 4. GDS: 불러오기와 폭 계산

### 4.1 저장된 값 (소스로 확인)

| 항목 | 내용 | 근거 |
| --- | --- | --- |
| 좌표 | µm, SREF를 펼친 월드 좌표, 항상 FRONT 원본 기준 | `GdsMapForm_DbSave.cs:121` |
| `POINTS_BIN` | X, Y double little-endian 반복, 점당 16 B. BOUNDARY는 마지막 점 = 첫 점 포함(`CLOSED_FLAG` 1). PATH는 열린 중심선 | `GdsMapPlacedRowBuilder.cs:69-82` |
| `PATH_WIDTH` | 전체 폭(µm). `Math.Abs`를 적용해 음수(절대 폭) 정보는 사라짐 | `GdsMapControl.cs:702` |
| `MIN_X ~ MAX_Y` | 꼭짓점 범위 ± `PATH_WIDTH / 2`. `PATH_TYPE`은 반영하지 않음 | `GlSceneItem.cs:44-51` |
| 끝 처리 | Type 0은 Bounds가 끝에서 w/2 넓게 잡힌다. Type 2 대각선 끝 모서리는 Bounds 밖으로 최대 (√2−1)·w/2 나간다 | 위 계산 |
| Type 4 | 연장 길이(BGNEXTN / ENDEXTN)를 읽지 않는다 | `GdsReader.cs:67-93` |
| SREF 배율 | 좌표에는 적용되지만 PATH 폭에는 적용되지 않는다 | `GdsMapControl.cs:702` |
| 수치 오차 | float 행렬, 15,000 µm에서 약 0.001 µm. 무시 | `GdsGeometry.cs:61-75` |

### 4.2 2단계 판정

| 단계 | 대상 | 방법 |
| --- | --- | --- |
| 1차 후보 | 모든 Member | Member Bounds를 탐색 거리 S만큼 넓힌 사각형과 Defect 사각형의 겹침. 균일 격자 색인(칸 200 µm, 지금과 같음). PATH는 사각 끝 대각선 몫으로 w/2를 더 넓힌다 |
| 2차 모양 | 1차 후보 Member만 | 실제 모양과 Defect 사각형 사이 거리 Gap (4.3) |

- 탐색 거리 S = max(허용오차, 50 µm)다. 50 µm는 허용오차 표(5.5)의 가장 큰 단계다.
- 정밀 계산은 후보에만 하므로 판정 시간은 Defect 수와 후보 수에 비례하고, Member 전체 수와는 거의 상관없다.

### 4.3 도형별 모양 모델

| 도형 | 판정에 쓰는 모양 | Gap 계산 |
| --- | --- | --- |
| BOUNDARY | 닫힌 다각형(마지막 중복 점 제거). 오목 다각형 가능 | 변끼리 교차하거나 한쪽이 다른 쪽 안에 있으면 0. 아니면 변-변 최소 거리 |
| PATH Type 0 (평평한 끝) | 선분마다 폭 w 사각형. 끝 연장 없음 | 조각(볼록 사각형)마다 분리축(SAT)으로 겹침이면 0, 아니면 변-변 최소 거리. 조각들의 최솟값 |
| PATH Type 2 (사각 연장) | Type 0 + 첫 / 끝 선분을 w/2 연장 | 같음 |
| PATH Type 4 (사용자 연장) | 연장 값을 읽지 않으므로 Type 2로 본다 | 같음 |
| PATH Type 1 (둥근 끝) | Type 0 + 양 끝에 반지름 w/2 원 | 원은 "중심과 사각형 거리 − w/2" |
| PATH 꺾임 (모든 Type) | 두 선분 사각형 바깥쪽 틈을 Miter 조각(꺾임 점, 바깥 모서리 2개, Miter 점)으로 메운다. 급히 꺾여 Miter 점이 꺾임 점에서 2w보다 멀면 바깥 모서리 2개를 잇는 삼각형으로 자른다 | 볼록 조각과 같음 |
| PATH 점 1개 | 한 변 w 정사각형(Type 1은 원) | 같음 |
| PATH 폭 0 | 중심선 | 선분-사각형 거리 |
| TEXT | 판정 제외 | - |

- **Gap**은 Defect 사각형과 도형 사이 가장 가까운 거리(유클리드)이고, 겹치면 0이다.
- **Gap(Defect, Chain)**은 그 Chain Member들 중 최솟값이다. 가장 가까운 Member ID도 같이 남긴다.
- "Gap ≤ 허용오차"는 Defect 사각형을 허용오차만큼 **둥글게** 넓혀 도형과 닿는다는 뜻이다.
  - 지금 구현은 **네모로** 넓힌다. 바뀌면 모서리 방향에서만 최대 (√2−1) × 허용오차만큼 좁아진다(5 µm면 2 µm).
- 화면 그리기(`AddPathFillVertices`, `GdsMapControl.cs:430-489`)는 꺾임을 삼각형 하나로 메워 위 규칙과 조금 다르다. 판정은 이 표를 기준으로 한다.

### 4.4 계산 코드 위치

- 화면 쪽 `06_CoreLibSource/GdsMapControl/Chain/ChainGeometryOverlap.cs`(.NET 4.8, 약 170줄)를 옮긴다.
  - 옮길 함수: `PointSegmentDistance` / `SegmentsIntersect` / `PointInPolygon` / `PolygonsIntersect` / `PolygonPathIntersect`.
  - 대상은 BSL 신규 `GdsKlarfShapeGeometry.cs`(.NET 4.0)이고, 점은 double 배열로 바꾼다.
  - 4.0 BSL은 4.8 어셈블리를 참조할 수 없다 (GDS Map DB 저장 설계).
- PATH 조각 만들기(선분 사각형 / 끝 연장 / Miter)는 4.3 규칙으로 새로 쓴다.
- `POINTS_BIN` 읽기는 `GdsMapPlacedRowReader`의 검사(`POINT_COUNT × 16 = 길이`)를 따른다.

### 4.5 서버 Cache

| 항목 | 내용 |
| --- | --- |
| 위치 | BSL static(서비스 프로세스 메모리). SingleCall 객체가 바뀌어도 유지된다 |
| 키 | FACTORY + DEVICE_ID |
| 바뀜 확인 | Map 저장 시각 + READY Chain 수 + Chain 저장 시각 최댓값. 분석 1단계에서 이미 읽는 `SELECT_MAP_INFO` / `SELECT_CHAIN_LIST` 결과로 만든다(쿼리 추가 없음). 다르면 다시 읽는다 |
| 내용 | READY Chain 전체 Member의 `CHAIN_SEQ` / `PLACED_ELEMENT_ID` / Layer / 종류 / 폭 / `PATH_TYPE` / 점 배열 / Bounds + 격자 색인. 화면에서 고른 Chain 거르기는 찾을 때 적용한다 (Cache는 Device 하나에 하나) |
| 개수 | 최근 3 Device(LRU). 점 수 합계가 400만 점(약 64 MB)을 넘는 Device는 Cache하지 않고 매번 읽는다 |
| 동시성 | 같은 키는 lock 하나로 한 번만 만든다. 다 만든 Cache는 읽기 전용 |
| 결과 표시 | `ANL_INFO.CACHE_HIT`(Y / N)와 GDS 읽기 시간 |

메모리 크기 (U9 Member 66,489개):

- Bounds만: Member당 double 6개(48 B), 약 3 MB.
- 모양 포함: Member당 점 k개 × 16 B가 더해진다.
  - k = 10이면 약 13 MB, k = 30이면 약 35 MB다. 도면 전체 평균은 30.6점이다.
  - Chain Member의 실제 점 수는 0단계 Probe로 잰다.
- 읽는 쿼리는 신규 `SELECT_CHAIN_MEMBER_SHAPE_ALL`이다.
  - 지금의 `SELECT_CHAIN_MEMBER_BOUNDS_ALL`에 `ELEMENT_TYPE` / `PATH_WIDTH` / `PATH_TYPE` / `POINT_COUNT` / `POINTS_BIN`을 더한 것이다.
  - BLOB 읽기는 행당 0.013 ms라는 기록이 있다 (GDS Map DB 저장 설계 13.4). 실제 시간은 0단계에서 잰다.

### 4.6 먼저 확인할 것 (0단계, 읽기 전용)

1. **Chain Member 통계**: 종류(BOUNDARY / PATH) 비율, PATH 폭 분포, `PATH_TYPE` 분포, Member당 점 수. Cache 크기와, 모양 판정이 결과를 얼마나 바꾸는지 미리 본다.
2. **중첩 SREF 변환 순서**: `GdsMapControl.FlattenStructure`(731-734행)는 `parent.Clone()`에 자식 행렬을 `Append`로 붙인다.
   - 이 순서는 점에 부모 변환을 먼저 적용한다.
   - 2단 이상 중첩에 회전 / 반전 / 배율이 섞이면 좌표가 틀어질 수 있다. 평행 이동만이면 결과가 같다.
   - U9 / OMM_TCB 도면에 그런 중첩이 있는지, 있으면 다른 GDS 도구 좌표와 맞는지 확인한다.
   - 틀리면 저장된 모든 좌표에 영향이 있으므로 별도 작업으로 다룬다.
3. **SREF 배율 ≠ 1이 있는지**: 있으면 PATH 폭이 틀린다.
4. **Type 4 PATH가 있는지.**

### 4.7 0단계 / S1 결과 (2026-10-16, CLAUDE-127)

SCK / OMM_Hybrid(READY Chain 15) 기준 측정. 도구: `BSL/Probes/GdsKlarfShapeStatsProbe.ps1`, `BSL/Probes/GdsKlarfShapeProbe.ps1`(30/30 PASS).

| 항목 | 값 | 설계에 주는 의미 |
| --- | --- | --- |
| Member | 66,489 (BOUNDARY 35,781 / PATH 30,708), 같은 도형이 여러 Chain 에 든 경우 0 | Member 행 = 고유 도형 |
| PATH | `PATH_TYPE` 모두 0, 폭 25 ~ 45 µm | Type 4 없음(4.6-4). DB Bounds 는 끝을 W/2 넓게 잡아 PATH Bounds 면적이 27.3% 크다 -> 모양 판정 필요 |
| 점 수 | 2,397,409점(평균 36.1, 중앙 65 = 원형 Pad) | 400만 점 상한 안 -> Cache 대상 |
| 읽기 | 조회 1,009 ms + 모양 만들기 323 ms, 두 번째 호출 0 ms(Cache) | 4.5 Cache 효과 확인 |
| Miter | DB Bounds 밖으로 나간 조각 0 | 이 도면은 직각 / 직선 배선 |
| STEP 528 (Defect 15) | 보기 BACK(반전) Bounds 판정 1 -> 모양 판정 1 / FRONT 9 -> 8 (Gap 0 이 6, 2 µm 이내 8) | 면 선택에 따라 결과가 크게 다르다. 어느 면이 맞는지는 S2 이후 확인 |

- 구현 위치: `GdsKlarfShapeGeometry`(모양 / Gap), `GdsKlarfGeometryCache`(격자 색인 + Cache), `GdsKlarfMatchEngine.MatchShapes`(탐색 거리 안 Gap 전부, 걸림 = Gap ≤ 허용오차, 중심 사각형은 Gap 0).
- 4.6-2 중첩 SREF 변환 순서 / 4.6-3 SREF 배율은 2026-10-15 CLAUDE-120 에서 확인했다(샘플 GDS 는 SREF / AREF 0 이라 저장 Map 영향 없음, 20절 끝 참고).

## 5. KLARF: 불러오기와 허용오차

### 5.1 읽기

- 신규 `TQD_DEFECT.xml SELECT_DEFECT_FOR_GDS`로 필요한 컬럼만 읽는다.
  - 조건: STEP_SEQ.
  - 컬럼: DEFECTID / XINDEX / YINDEX / XREL / YREL / XSIZE / YSIZE / CLASSNUMBER / ROUGHBINNUMBER / IMAGECOUNT.
  - 지금 쓰는 `GET_DEFECT`는 28 컬럼을 모두 읽는다.
- 분류 거르기는 서버에서 하고(지금과 같음), 기준을 `CLASSNUMBER` 또는 `ROUGHBINNUMBER` 중에서 고르게 한다 (B7).
  - 2026-10-15 업로드 분류 표준화 이후 고객 파일은 CLASSNUMBER가 모두 0이고, 실제 분류(RDLopen / RDLshort)는 RoughBin에 있다.
  - 이미 올린 STEP 528 / 541은 예전 규칙(Class = RoughBin)이라 두 기준이 섞여 있다.

### 5.2 Chip 좌표 변환

1차 식은 앞 문서 6.1을 그대로 쓴다.

```text
gx = XREL - PitchX / 2 + 보정X
gy = YREL - PitchY / 2 + 보정Y
저장 기준(FRONT) ≠ 보기 기준이면 gx = -gx   (Member 대신 Defect를 반전)
```

바꿀 것은 **Pitch 출처**다.

- 지금 Pitch는 `TQD_SETUP.DIE_PITCH_X / Y`다. 2026-10-13 Map Setup 필수 이후로 이 값은 File Map Setup 값이다.
- XREL은 KLARF 파일의 Pitch 칸 기준이다. 두 Pitch가 다르면 Chip 중심이 ΔPitch / 2만큼 밀린다.
  - 예: OMM_TCB 파일 Y 30,058, Map 30,060이면 1 µm 밀린다.
  - Map Setup에서 Pitch를 손으로 고친 경우에는 더 크게 밀릴 수 있다.
- 앞 문서 6.1의 "Pitch는 KLARF 값" 문장은 지금 동작과 맞지 않는다.
- 권장(B6):
  - KLARF 업로드가 원본 DiePitch를 `TQD_STEP.COMMENT`에 `RAW_DIE_PITCH=x,y`로 남긴다. `RAW_WAFER_ID`를 이미 같은 방식으로 남기고 있어 DDL이 필요 없다.
  - 분석은 이 값을 먼저 쓴다.
  - 값이 없으면(예전 업로드) `TQD_SETUP` 값을 쓰고, `ANL_INFO.PITCH_SOURCE = MAP_SETUP`으로 표시한다.

### 5.3 Defect 사각형

- 중심은 (gx, gy), 크기는 XSIZE × YSIZE다. [Defect 크기 반영]을 끄면 점으로 본다 (A4).
- 크기가 큰 Defect가 있다. 1107236219 파일에 XSIZE 5,043 µm가 있다.
  - 이런 사각형은 여러 Chain을 덮어 걸림 수를 부풀린다.
  - 판정은 그대로 하되, 한 변이 1,000 µm 이상이면 `DEFECT.LARGE = Y`로 표시한다.
  - 조건 줄에 [큰 Defect 제외] 체크(기본 해제)를 둔다 (B5).

### 5.4 허용오차: 얼마로, 왜

허용오차는 **Defect 위치를 GDS 위에 올릴 때의 무작위 위치 오차**만 덮는 값으로 정의한다. Defect 크기는 이미 사각형으로 반영했고, 한 방향으로 밀리는 **계통 오차**는 보정 X / Y로 맞춘다.

오차 예산:

| 원인 | 크기 (µm) | 성격 | 처리 |
| --- | --- | --- | --- |
| AOI 위치 측정 (화소 1개) | C4Bump 3.23 / TBDB 1.29 | 무작위 | 허용오차 |
| Defect 크기 값 끊김 (화소 반쪽) | 1.6 / 0.65 | 무작위 | 허용오차 |
| Die 안 회전 잔차 | 모름. 0.01°라면 Chip 끝(15,000 µm)에서 2.6 | Die 위치마다 다름 | 허용오차 |
| GDS 모양 근사 | Bounds만 쓰면 수십 ~ 수백, 모양 판정 후 0 | 모델 | 4절 모양 판정 |
| Pitch 출처 차이 | 0 ~ 1 (ΔPitch / 2) | 계통 (파일별) | B6 원본 Pitch |
| Chip 위치 (Scribe / Seal ring 비대칭) | 0 ~ ±36 (X) / ±37.5 (Y) | 계통 (Device별) | 보정 X / Y |
| 좌표 계산 (float) | 0.001 | - | 무시 |
| 보기 기준(Back / Front) 잘못 고름 | 수 mm | 오류 | 허용오차로 덮을 수 없음. 보기 기준 확인 |

- Chip 위치 오차 크기의 근거: KLARF Pitch(23,832 × 30,060)와 GDS Chip(23,760 × 29,985)의 차이는 X 72 / Y 75 µm다. Chip이 Pitch 칸 한쪽으로 붙어 있으면 최대 그 절반만큼 밀린다.
- 무작위 항목을 합친 값(제곱합의 제곱근)은 C4Bump √(3.23² + 1.6² + 2.6²) ≈ 4.4 µm, TBDB ≈ 3.0 µm다. 그래서 **기본 5 µm를 유지한다 (A3, B4).**
- 계통 오차를 허용오차로 덮지 않는 이유:
  - 리뷰 이미지 눈금으로 잰 RDL 폭은 약 17 ~ 20 µm, Bump pad 간격은 약 160 µm다(눈 측정, 확정값 아님).
  - 허용오차를 40 µm로 잡으면 Defect 영역이 80 µm 넘게 넓어진다. 그러면 옆 배선 / 옆 Chain까지 걸려 "Chain 위 Defect" 구분이 사라진다.
  - 결과 행도 크게 는다. Defect 10만 개 × 200 µm에서 265만 행이 나온 기록이 있다.

### 5.5 허용오차 표 (민감도)

[분석] 한 번에 Gap을 탐색 거리(50 µm)까지 구해 두고 단계별로 센다. 다시 계산하지 않는다.

| 허용오차 (µm) | 0 | 2 | 5 | 10 | 20 | 50 |
| --- | --- | --- | --- | --- | --- | --- |
| Chain 영역 Defect 수 | | | | | | |
| 걸린 Die 수 | | | | | | |

읽는 법 (엔지니어 판단용):

- Chain 위에 실제로 있는 결함(RDLopen / RDLshort)은 좌표가 맞으면 Gap 0 ~ 5 µm에 몰린다.
- 우연히 가까운 결함은 허용오차가 커질수록 천천히 는다.
- 5 → 20 µm 사이에서 수가 크게 뛰면 좌표가 한쪽으로 밀렸다는 신호다. 보정 X / Y를 바꿔 다시 [분석]하고, Gap 0 수가 느는 쪽을 고른다 (수동).
- 보정값 자동 추정은 2차로 둔다.
- Defect 탭에 Gap 열을 두어 Defect마다 거리(µm)를 본다.

### 5.6 결과 행 줄이기

Defect 하나가 Member 수십 개에 걸려도 결과는 (Defect, Chain)당 1행이다. 이 행에는 Gap 최솟값과 가장 가까운 Member를 남긴다. Member 단위 행은 Die 상세(`AnalyzeDie`)에서만 만든다.

## 6. E-Test: 불러오기와 Chain 연결

### 6.1 대상과 판정 값

- `TQP_GDS_ET_ITEM.GDS_CHAIN_NAME`이 들어간 Parameter만 쓴다. 로컬 SCK / OMMTCB / TSVET은 85개가 입력돼 있다. 이름이 없는 Parameter는 연관 분석에서 보지 않는다.
- 판정은 저장된 `TQP_GDS_ET_VALUE.JUDGE_FLAG`를 쓴다 (B12). 이 값은 업로드 / 재판정 때 서버 SQL로 계산된 것이다.
- 판정 뒤 Spec이 바뀌었으면(`SPEC_CHANGED`) 상태줄에 경고한다. 판정 당시 Lower / Upper는 저장되지 않는다.

### 6.2 (Die, Chain) 판정 집계: 서버 SQL 1번

신규 `TQP_GDS_ET_VALUE.xml SELECT_ET_DIE_CHAIN_JUDGE` (인자 src_file_seq):

```sql
SELECT D.DUT_NO, D.MAP_X, D.MAP_Y, D.JUDGE_FLAG AS DIE_JUDGE,
       UPPER(TRIM(I.GDS_CHAIN_NAME))                            AS GDS_CHAIN_KEY,
       COUNT(*)                                                 AS PARA_COUNT,
       SUM(CASE WHEN V.JUDGE_FLAG = 'F' THEN 1 ELSE 0 END)      AS FAIL_COUNT,
       SUM(CASE WHEN V.JUDGE_FLAG = 'P' THEN 1 ELSE 0 END)      AS PASS_COUNT,
       MIN(CASE WHEN V.JUDGE_FLAG = 'F' THEN I.ITEM_NAME END)   AS FIRST_FAIL_ITEM
  FROM TQP_GDS_ET_DIE D
  JOIN TQP_GDS_ET_VALUE V ON V.SRC_FILE_SEQ = D.SRC_FILE_SEQ AND V.DUT_NO = D.DUT_NO
  JOIN TQP_GDS_ET_ITEM  I ON I.ET_ITEM_SEQ = V.ET_ITEM_SEQ
 WHERE D.SRC_FILE_SEQ = :src_file_seq
   AND I.GDS_CHAIN_NAME IS NOT NULL
 GROUP BY D.DUT_NO, D.MAP_X, D.MAP_Y, D.JUDGE_FLAG, UPPER(TRIM(I.GDS_CHAIN_NAME))
```

(Die, Chain) E-Test 상태:

| 상태 | 조건 |
| --- | --- |
| F | FAIL_COUNT ≥ 1 (값이 '-'이면 판정 규칙상 F) |
| P | FAIL_COUNT = 0이고 PASS_COUNT ≥ 1 |
| N | 판정 값 없음 (Spec 없음 / Enable N / 빈 칸) |
| - | 이 Chain에 연결된 Parameter 없음 |

결과 행 수는 Die × 연결 Chain이다(U9는 73 × 15 이하). 값 행(Die × Parameter)은 화면으로 보내지 않는다.

### 6.3 Wafer 연결: Wafer Key로 자동 수집 (B8, 16절)

- 2026-10-15 컨셉 변경으로 "엔지니어가 목록에서 파일 선택"을 바꿨다. Wafer를 고르면 같은 Wafer Key의 E-Test 파일을 공정별로 모아 [준비] 탭에 보인다(16.4 / 16.6).
- 공정(TSVET / COWET / FT)은 한 번에 1개를 쓴다. 기본값은 Test 시각이 가장 최근인 공정이다 (B19).
- 규칙으로 못 찾으면 [직접 찾기]로 E-Test 파일 목록 창을 열어 더한다. 창 위에는 비교용으로 KLARF Lot / Slot / Wafer Key를 보이고, 아래 표는 기존 `GetEtFileBrowseList`다.
- 지금 샘플 기준(잠정 규칙):
  - 1쌍째 `PFIC_RH-HS2615_C4Bump` Slot 2 → `RH-HS2615-02`는 E-Test와 맞는다.
  - 2쌍째 `PFIC_U9-FS2635_C4Bump` Slot 1 → `U9-FS2635-01`은 E-Test `U9-HS2635-01`과 맞지 않는다. 고객 규칙이 오기 전에는 [직접 찾기]로 더한다.
- 묶은 결과는 저장하지 않는다 (A7). `ANL_INFO`와 상태줄에 남긴다.

### 6.4 Die 연결: 시스템 Die 좌표 (B9)

> **2026-10-20 변경 (B28)**: KLARF Die 번호는 검사 면과 상관없이 그대로 시스템 Die 좌표로 쓴다(= 아래 Back 규칙, 고객 대응표 U9 76 Die 와 같음). 검사 면(FS 고정)은 GDS 반전에만 쓴다. 아래 Front 규칙(MinX + MaxX − x)은 서버에서 쓰지 않으며, 여러 Step(B16, 2차) 때 면이 다른 Step 을 합칠 때 다시 본다.

- KLARF `TQD_DEFECT.XINDEX / YINDEX`는 이미 Map Setup 원점을 더한 왼쪽 아래 기준 번호다.
  - 보기 기준 Back(BS 검사): 시스템 = (x, y).
  - 보기 기준 Front(FS 검사): 시스템 = (MinX + MaxX − x, y).
  - 계산은 `DieCoordinateMapper.FromDieTable(KLARF Setup Die 목록, new DiePoint(0, 0))`과 `KlarfBack` / `KlarfFront`로 한다. 원점을 이미 더해 저장했으므로 원점은 (0, 0)이다.
  - Step마다 보기 기준 하나가 그 Step Defect의 GDS 반전과 KLARF Die 반전을 함께 정한다. 둘 다 "이 Step을 어느 면에서 검사했나"라는 같은 질문이다. 보기 기준은 [준비] 탭 Step 표의 열이다 (16.6).
  - 여러 Step을 합칠 때 Die는 시스템 좌표로 모은다. Step마다 Back / Front 반전이 달라서 저장 Index를 그대로 더하면 다른 Die가 섞인다.
- E-Test는 `TQP_GDS_ET_DIE.MAP_X / MAP_Y`를 쓴다. 업로드 때 E-Test Map으로 계산해 저장한 값이다.
  - `MAP_X`가 NULL(Map 밖)인 Die는 빼고, 그 수를 상태줄에 보인다.
- 두 Map(KLARF Product Map, E-Test Device Map)의 사용 Die 범위(MinX ~ MaxY)와 Die 수를 비교한다. 다르면 "Map 형상 다름"으로 경고한다. U9 → OMM / OMM_Hybrid는 같은 Map의 복사라 같다.
- 한쪽에만 있는 Die는 4분류에서 빼고 회색으로 보인다.
  - KLARF에만 있음 = "ET 없음".
  - E-Test에만 있음 = "검사 안 함".
- 검증 사례: 10-06 회의의 "27번 Die Fail + Defect"가 이 규칙으로 같은 Die에 모이는지 대조한다.
- 앞 문서 9절의 잠정식 `ET_X = KLARF_YI + 4, ET_Y = 5 − KLARF_XI`는 폐기한다. Die 좌표 대응 설계 9절의 규칙식으로 대체됐다.

### 6.5 Chain 연결: 이름 (B10)

- E-Test `GDS_CHAIN_NAME`은 E-Test Device(예: OMM)와 이름이 같은 GDS Map의 Setup Chain 이름이다.
- 분석 GDS Device(예: OMM_Hybrid)는 Map 복사로 Chain 이름 / Code / 색은 같고 `CHAIN_SEQ`만 다르다.
- 그래서 `UPPER(TRIM(이름))`으로 분석 Device의 READY Chain(`CHAIN_VER.CHAIN_NAME`, `IS_CURRENT = 1`)과 맞춘다.
- 결과 `ET_LINK` 표(화면 A "E-Test 연결" 탭):

| 열 | 내용 |
| --- | --- |
| E-Test Chain 이름 | `GDS_CHAIN_NAME` 원문 |
| 연결 Chain | CHAIN_CODE / 이름 |
| 상태 | 연결 / GDS에 없음 / 이름 중복 |
| Parameter 수 | 이 이름을 쓰는 Parameter 수 |
| 2 Chain 항목 수 | 그중 Chain 2가 있는 항목 수 |

- `CHAIN_NAME`은 UNIQUE가 아니다. 이름이 중복되면 연결하지 않고 표에 표시한다. 엔지니어가 GDS Chain 이름이나 E-Test 쪽 이름을 고친다.

### 6.6 Chain 2개 항목 (Leakage_Serial 등) (B11)

- `GDS_CHAIN_NAME`은 Chain 1 기준 1칸뿐이다.
- 1차는 Chain 1에만 붙인다. `ET_LINK`에 2 Chain 항목 수를 보이고, 화면 B E-Test 값 탭에 `A : B` 원문을 보인다.
- 두 Chain 사이 누설은 두 Chain 사이 Short Defect와 바로 연결되는 항목이다. 2차에 `GDS_CHAIN_NAME2` 컬럼(DDL 1개)을 검토한다.

## 7. 세 데이터 합치기

### 7.1 (Die, Chain) 4분류

| 분류 | Defect (Gap ≤ 허용오차) | E-Test | 뜻 | 색 |
| --- | --- | --- | --- | --- |
| 일치 | 있음 | F | Defect가 Fail 원인 후보 | `#C70039` |
| Fail만 | 없음 | F | 보이지 않는 결함 / 다른 Step / 좌표 문제 | `#FF5733` |
| Defect만 | 있음 | P | 전기적으로 영향이 없는 Defect | `#FFC300` |
| 정상 | 없음 | P | - | 흰색 |
| 비교 불가 | - | N / - / ET 없음 / 검사 안 함 | 비교할 수 없음 | `#E0E0E0` |

색은 앞 문서 A2의 Defect Count Color 색에서 골랐다.

### 7.2 Chain별 Die 수 (Chain 탭 열)

대상 Die = KLARF 검사 Die이면서 그 Chain의 E-Test 상태가 P / F인 Die.

| 열 | 정의 |
| --- | --- |
| Defect Die | 그 Chain에 Defect가 걸린 Die 수 |
| Fail Die | 그 Chain이 E-Test F인 Die 수 |
| 일치 Die | 둘 다인 Die 수 |
| Fail률 (Defect 있음) | 일치 Die / Defect Die |
| Fail률 (Defect 없음) | Fail만 Die / (대상 Die − Defect Die) |

- 두 Fail률의 차이가 크면 그 Chain은 Defect가 Fail로 이어진다는 신호다.
- Die 수가 적어(U9 76개) 통계 판정은 하지 않고 숫자만 보인다.

## 8. 화면 표시

### 8.1 화면 A (Wafer)

조건 줄:

- 더할 것: 분류 기준 (Class / RoughBin) / [큰 Defect 제외] / 색 기준.
- 옮길 것 (16.6): 보기 기준은 [준비] 탭 Step 표의 열로 옮긴다(Step마다 면이 다르다). E-Test 파일 선택도 [준비] 탭으로 옮긴다.

**색 기준 콤보 (신규)**:

| 색 기준 | Die 색 |
| --- | --- |
| Chain Defect 수 (기본, 지금 동작) | 구간 색 (A2) |
| E-Test Fail Chain 수 | 같은 구간 색, 숫자 = F Chain 수 |
| 일치 Chain 수 | 같은 구간 색, 숫자 = 일치 Chain 수 |
| 선택 Chain 4분류 | Chain 탭에서 고른 Chain 하나의 7.1 색 |

- [준비] 탭에서 E-Test 공정을 골랐으면, 색 기준과 상관없이 Die 판정이 F인 Die에 표식을 겹쳐 그린다.
  - 2026-10-16 시안: 선택 Die 빨간 테두리(기존 동작)와 겹치지 않게, 테두리 대신 Die 왼쪽 위 모서리 빨간 삼각형으로 그린다(17절). 그리는 곳은 `GdsKlarfAnalysisMap.WaferMap_Paint`다.
  - 2026-10-16 (CLAUDE-140): 오른쪽 위 색 구간 표 끝에 "E-Test Fail Die" 행(색 칸 = 흰 Die 왼쪽 위 빨간 삼각형, Die 수 = 표식 Die 수)을 더한다. E-Test 판정이 있을 때만 보이고 색 기준과 상관없다. 표식 Die 는 위 구간 행에도 함께 세어진다.
  - 2026-10-19 (CLAUDE-145): 빨간 삼각형 기준을 구간 표 머리의 라디오로 고른다. **Spec Out**(기본) = DMS Spec 판정(Die 의 등록 Parameter 중 하나라도 Spec 밖 또는 '-', `DIE.ET_STATE`), **Fail Die** = 결과 파일의 DUT Pass / Fail Flag(`TQP_GDS_ET_DIE.PASS_FLAG` -> 서버 `DIE.ET_FILE_FLAG`). 바꾸면 서버를 다시 부르지 않고 표식과 구간 표 행("Spec Out Die" / "Fail Die (결과 파일)")만 다시 그린다.

**탭과 열**:

| 탭 | 더할 것 |
| --- | --- |
| 준비 (신규, 맨 앞) | 16.6 Wafer Key / GDS / KLARF Step / E-Test 표 |
| Die | ET 판정 / ET Fail Chain 수 / 일치 Chain 수 / 시스템 (X, Y) |
| Chain | E-Test 연결 / Parameter 수 / Defect Die / Fail Die / 일치 Die / Fail률 2개 |
| Defect | Step / Gap(µm) / 가까운 Chain / 크기 / 큰 Defect / RoughBin. **Chain 영역 Defect만** 보인다 (전체 Defect는 Map에 이미 그려져 있다) |
| 허용오차 (신규) | 5.5 표 |
| E-Test 연결 (신규) | 6.5 표 |
| E-Test Fail (신규) | 15.3 역추적 목록 |

**상태줄**: Wafer Key(규칙 잠정 / 확정) / 분석 Step 수 / E-Test 파일(WAFER_ID / 공정 / 판정 시각) / Spec 바뀜 경고 / Map 형상 다름 경고 / Pitch 출처 / Cache 여부.

```text
+-------------------------------------------------------------------------------------------------------+
| GDS Device [OMM_Hybrid v] 허용오차 [5] µm  보정 X [0] Y [0]  분류 [RoughBin v]  [큰 Defect 제외]        |
| 색 기준 [일치 Chain 수 v]                                                                  [분석]      |
+------------------------------------------------------+------------------------------------------------+
|         Wafer Map                                    | [준비][구간][Die][Chain][Defect][허용오차]...   |
|   [ ][1][ ][ ]                                       |  Chain | 연결 | Defect Die | Fail Die | 일치   |
|   [ ][2][#][ ]   # = 빨간 테두리 (E-Test Fail Die)   |  G     | 연결 |     2      |    3     |  2     |
|   [ ][ ][1][ ]                                       |  ...                                           |
+------------------------------------------------------+------------------------------------------------+
| Wafer Key RH-HS2615-02 (규칙 잠정) / Step 2 / E-Test TSVET 판정 10-14 / Spec 바뀜 없음 / GDS Cache Y   |
+-------------------------------------------------------------------------------------------------------+
```

### 8.2 화면 B (Die 상세)

- 열 때 호스트(`frmGdsDieAnalysis`)가 `AnalyzeDie`로 그 Die만 다시 받는다. 화면 A의 Wafer 결과 전체는 넘기지 않는다.
- Chain 목록 열을 더한다: ET 상태 / Fail Para 수 / Para 수 / 4분류.
- ~~E-Test F Chain은 도면 Overlay에서 빨간 굵은 외곽선으로 그린다.~~ 2026-10-16 변경(사용자 요청 "각 Chain에 맞는 색으로"): 모든 Chain 을 저장 색으로 그린다. 빨간색으로 덮으면 어느 Chain 인지 구분되지 않는다. Fail 여부는 Chain 목록 ET / 분류 열로 본다.
- Chain 행 더블클릭(역추적)해도 목록 순서 / 스크롤 위치는 그대로 두고 그 행만 선택한다(2026-10-16).
- 신규 "E-Test 값" 탭: 그 Die의 연결 Parameter를 보인다.
  - 열: Parameter / 헤더 원문 / Chain(`A : B`) / GDS Chain / 값 / Lower / Upper / 판정.
  - 행을 고르면 그 Chain을 강조한다.
- Defect 목록 열을 더한다: Gap / 가까운 Chain. Defect를 고르면 가장 가까운 Member를 강조한다(지금 동작 유지).

## 9. Die / Defect가 많을 때

### 9.1 무엇이 무엇에 비례하나

| 데이터 | 비례 대상 | U9 기록 | 큰 Wafer 예 (가정) | 대책 |
| --- | --- | --- | --- | --- |
| GDS Chain Member 모양 | Chip 도면 (Die 수와 무관) | Member 66,489 | 같음 | 서버 Cache 1회, 모든 Die 공유 (4.5) |
| Ready 목록 | Wafer 1장의 Step 수 + E-Test 공정 수 | 수 행 | 수십 행 | 목록만 읽음. 후보 쿼리는 기간 조건으로 범위 제한 (16.8) |
| Defect | Wafer 면적 × 결함 밀도 × 체크한 Step 수 | 15 ~ 1,122 | 100,000 | 필요 컬럼만, 판정은 Defect 수에 비례. 상한은 Step 합계 기준 |
| DIE 행 | Die 수 | 76 | 3,000 | 그대로 |
| DIE_CHAIN 행 | Die × Chain | ≤ 1,140 | 3,000 × 50 = 150,000 | Defect 0이고 ET P인 칸은 보내지 않음 |
| Defect-Chain 행 | 걸린 Defect × Chain | 수십 | 수십만 | Member 단위 행을 없애고 Chain당 1행 |
| E-Test 값 | Die × 연결 Parameter | 73 × 109 = 7,957 | 3,000 × 300 = 900,000 | SQL에서 (Die, Chain)으로 묶음. 값은 Die 상세 때 1 Die만 |
| Map 그리기 | Die 수 | 76 | 3,000 | Die 칸이 24 px보다 작으면 숫자 생략, 색만 |
| 화면 B 도면 | Chip 도면 | 도형 204,898 | 같음 | Chain Layer만(A8), 같은 Device는 다시 읽지 않음(`LoadedMapKey`) |

"큰 Wafer 예"는 작은 Die 제품을 가정한 값이고 측정값이 아니다.

### 9.2 대책

1. GDS 서버 Cache (4.5).
2. 2단계 판정: 모양 계산은 Bounds 후보에만 한다 (4.2).
3. 결과 축소: `DEFECT`는 Chain 영역 Defect만 보내고, Member 단위 `DEFECT_HIT` 대신 `DEFECT_CHAIN`을 쓴다. `DIE_CHAIN`은 의미 있는 칸만 보낸다.
4. Die 상세는 `AnalyzeDie`로 그 Die만 계산한다 (Cache 사용).
5. 서버에서 돌려주기 전에 DataSet `RemotingFormat = SerializationFormat.Binary`로 바꾼다. 기본 XML 형식보다 작고 빠르다.
6. E-Test는 SQL에서 묶는다 (6.2).
7. 상한: Defect가 300,000개를 넘으면 오류로 멈추고 분류 거르기를 쓰라고 안내한다. 값은 Probe 측정 후 조정한다.
8. 측정: `ANL_INFO`에 단계별 시간(GDS 읽기 / Defect 읽기 / 판정 / E-Test / 결과 만들기)과 행 수를 남긴다. 지금은 `ELAPSED_MS` 하나뿐이다.

### 9.3 측정 계획 (Probe)

- 합성 Defect 1만 / 10만 / 30만 개(U9 Chip 안 무작위 + Chain 위) × 허용오차 5 / 50 µm로 판정 시간, 결과 행 수, DataSet 크기를 잰다.
- Cache가 있을 때와 없을 때의 분석 시간을 잰다.
- U9 STEP 501 / 541에서 모양 판정 후 Chain 영역 Defect 수가 Bounds 결과보다 얼마나 줄어드는지 본다.

## 10. 서비스 / 결과 변경

### 10.1 Interface

```csharp
// DACrux.SEMDMS.Interface (신규 요청 클래스, [Serializable])
public class GdsKlarfAnalysisRequest
{
    public string Factory;
    public string DeviceId;            // 비면 Product와 이름이 같은 READY Device
    public string WaferKey;            // Ready 단계에서 계산한 값 (기록용)
    public long[] StepSeqs;            // [준비] 탭에서 체크한 KLARF Step
    public string ChainSide;           // "FRONT" / "BACK" - GDS Chain 이 그려진 면(분석에 1개, 2026-10-16 B21)
    public string[] ViewSides;         // StepSeqs와 같은 순서. "BACK" / "FRONT" - 그 Step 의 검사 면. GDS 반전 = (ChainSide != 검사 면), KLARF Die 반전 = 검사 면
    public long DisplayStepSeq;        // Map을 그리는 기준 Step (DMSearch에서 고른 Step)
    public double ToleranceUm;
    public bool UseDefectSize;
    public bool ExcludeLargeDefect;
    public double OffsetXUm;
    public double OffsetYUm;
    public string[] ChainSeqs;         // null = READY 전체
    public string ClassBasis;          // "CLASS" / "ROUGHBIN"
    public int[] ClassValues;          // null = 전체
    public long EtSrcFileSeq;          // 0 = E-Test 없음
}

public interface iGdsKlarfAnalysis
{
    DataSet GetWaferReady(string factory, long stepSeq, string deviceId);        // Ready 목록 (16.4). deviceId 비면 Product와 같은 이름
    DataSet AnalyzeWafer(...);                                                    // 기존 유지 (Probe / 호환)
    DataSet Analyze(GdsKlarfAnalysisRequest request);                             // Wafer 집계
    DataSet AnalyzeDie(GdsKlarfAnalysisRequest request, int xIndex, int yIndex,
                       string focusChainSeq);                                     // Die 1개 상세. focusChainSeq = 역추적 기준 Chain (null = 없음, 15.3)
}
```

인자가 늘어 요청 클래스로 묶는다. Interface DLL은 서버와 화면이 같이 쓴다 (`DiePoint`와 같은 방식).

### 10.2 결과 DataSet

| 테이블 | 변경 |
| --- | --- |
| `READY` 계열 (신규, `GetWaferReady`) | 16.4 표 5개: `WAFER_KEY` / `READY_GDS` / `READY_KLARF` / `READY_ET` / `READY_CHECK` |
| `ANL_INFO` | + WAFER_KEY / WAFER_KEY_RULE(잠정 / 확정) / STEP_SEQS / VIEW_SIDES / DISPLAY_STEP_SEQ / CLASS_BASIS / SEARCH_UM / PITCH_SOURCE / CACHE_HIT / 단계별 시간 / ET_SRC_FILE_SEQ / ET_WAFER_ID / ET_TEST_PROGRAM / ET_JUDGE_TIME / ET_SPEC_CHANGED / ET_DIE_COUNT / ET_OUT_OF_MAP_COUNT / MAP_SHAPE_SAME |
| `CHAIN` | + ET_LINK_STATUS / ET_PARA_COUNT / DIE_JUDGED / DIE_DEFECT / DIE_FAIL / DIE_BOTH / FAIL_RATE_DEFECT / FAIL_RATE_CLEAN |
| `DIE` | + SYS_X / SYS_Y / ET_STATE (P / F / N / 없음) / ET_FAIL_CHAIN_COUNT / BOTH_CHAIN_COUNT |
| `DIE_CHAIN` | + MIN_GAP / NEAR_DEFECT_ID / ET_STATUS / ET_PARA_COUNT / ET_FAIL_COUNT / ET_FIRST_FAIL_ITEM / ET_FIRST_FAIL_VALUE / CLASS4. 보내는 행 = Defect ≥ 1 또는 ET 상태 ≠ P. MIN_GAP은 허용오차와 상관없이 탐색 거리(50 µm) 안의 그 Die Defect 중 최솟값이고, 없으면 NULL (15.3) |
| `DEFECT` | Chain 영역 + Chip 밖 Defect만. + STEP_SEQ / STEP_ID / GAP / NEAR_CHAIN_SEQ / ROUGHBINNUMBER / LARGE. Defect 키 = (STEP_SEQ, DEFECTID) |
| `DEFECT_CHAIN` (신규, `DEFECT_HIT` 대신) | STEP_SEQ / DEFECTID / CHAIN_SEQ / GAP / NEAR_PLACED_ELEMENT_ID |
| `TOL_CURVE` (신규) | TOL_UM / IN_CHAIN_DEFECT / HIT_DIE |
| `ET_LINK` (신규) | GDS_CHAIN_KEY / CHAIN_SEQ / CHAIN_CODE / STATUS / PARA_COUNT / CHAIN2_PARA_COUNT |
| `AnalyzeDie` 결과 | 위와 같은 모양을 Die 1개로. `DEFECT`는 그 Die 전체, `DEFECT_HIT`(Member 단위, Gap ≤ 허용오차), `ET_VALUE`(그 Die 연결 Parameter의 값 / Lower / Upper / 판정) |

### 10.3 쿼리 (추가만, 기존 쿼리 수정 없음)

| XML | queryid | 용도 |
| --- | --- | --- |
| `TQP_GDS_CHAIN_MEMBER.xml` | `SELECT_CHAIN_MEMBER_SHAPE_ALL` | Member 모양 (Cache 만들 때) |
| `TQD_DEFECT.xml` | `SELECT_DEFECT_FOR_GDS` | Defect 필요 컬럼 |
| `TQP_GDS_ET_VALUE.xml` | `SELECT_ET_DIE_CHAIN_JUDGE` | (Die, Chain) 판정 |
| `TQP_GDS_ET_VALUE.xml` | `SELECT_ET_VALUE_OF_DIE` | Die 1개의 연결 Parameter 값 + Spec |
| `TQD_INSP_INFO.xml` | `SELECT_KLARF_STEP_BY_WAFER` | Ready: 같은 Wafer 후보 KLARF Step (16.4) |
| `TQP_GDS_SRC_FILE.xml` | `SELECT_ET_FILE_BY_WAFER` | Ready: 같은 Wafer 후보 E-Test 파일 (16.4) |

E-Test 파일 1건 / 목록은 기존 `GetEtFile` / `GetEtFileBrowseList`, Map Die 목록(형상 비교)은 기존 `SELECT_ET_MAP_DIE`를 다시 쓴다. [직접 찾기] 창도 기존 목록 서비스를 쓴다.

### 10.4 파일

| 구분 | 파일 |
| --- | --- |
| Interface | `iGdsKlarfAnalysis.cs` (메서드 3개 + 요청 클래스), 신규 `Coordinate/WaferKeyRule.cs` (Wafer Key 규칙, 16.2) |
| RO | `GdsKlarfAnalysis.cs` (메서드 3개) |
| BSL | `GdsKlarfAnalysis.cs`(흐름), 신규 `GdsKlarfReady.cs`(Ready 목록 / 점검), `GdsKlarfMatchEngine.cs`(Gap / 2단계), 신규 `GdsKlarfShapeGeometry.cs`(모양 거리), 신규 `GdsKlarfGeometryCache.cs`, 신규 `GdsKlarfEtLink.cs`(E-Test 집계 / 이름 / Die 연결) |
| DSL / XML | 10.3 쿼리 6개 |
| 화면 A | `frmGdsKlarfAnalysis`(조건 / 콤보 / [준비] 탭 / 결과 탭), `GdsKlarfAnalysisMap`(테두리 / 4분류 색 / 숫자 생략), 신규 [직접 찾기] 목록 창 (KLARF Step / E-Test 파일) |
| 화면 B | `frmGdsDieAnalysis`(`AnalyzeDie` 호출), `GdsDieAnalysisForm`(열 / E-Test 값 탭 / Fail 외곽선) |
| KLARF 업로드 | `KlarfUpload.cs`가 COMMENT에 `RAW_DIE_PITCH` 1줄 (B6 승인 시) |
| 검증 | `GdsKlarfAnalysisProbe.ps1` 보강, 신규 모양 거리 Probe, Smoke 보강 |

DDL은 없다. 예외: B23 / B24 승인 시 `TQP_GDS_MAP` 컬럼 7개 (20.4).

## 11. 결정 항목 (권장안)

| ID | 결정 | 권장안 | 다른 안 |
| --- | --- | --- | --- |
| B1 | GDS 판정 정밀도 | Bounds 후보 + 실제 모양 거리 (PATH 폭 / 끝 처리 / 꺾임) | Bounds만 (지금) |
| B2 | GDS 읽기 | 서버 Cache (Device별, 저장 시각 확인, 최근 3개) | 분석마다 DB |
| B3 | 판정 방식 | 거리(Gap) 계산 + 허용오차 표(0 ~ 50 µm) | 걸림 여부만 (지금) |
| B4 | 허용오차 기본 | 5 µm 유지, 계통 오차는 보정 X / Y | 크게(20 µm 이상) 잡아 덮기 |
| B5 | 큰 Defect | 한 변 1,000 µm 이상 표시 + [큰 Defect 제외] 체크(기본 해제) | 그대로 / 크기 상한 강제 |
| B6 | Pitch 출처 | KLARF 원본 Pitch를 업로드 때 COMMENT에 남겨 먼저 사용 | Map Setup Pitch 그대로 (수 µm 밀릴 수 있음) |
| B7 | 분류 거르기 | Class / RoughBin 선택, 기본 RoughBin | CLASSNUMBER만 (지금) |
| B8 | Wafer 연결 (2026-10-15 변경) | Wafer Key로 KLARF Step / E-Test 파일을 자동 수집해 [준비] 탭에 표로 보이고, 엔지니어가 체크 / 선택. 못 찾으면 [직접 찾기] | 엔지니어가 E-Test 파일만 목록에서 선택 (이전 권장안) |
| B9 | Die 연결 | 시스템 Die 좌표 (보기 기준으로 KLARF Back / Front 반전) | E-Test 좌표로 맞춤 |
| B10 | Chain 연결 | 이름(대소문자 / 앞뒤 공백 무시). 중복 / 없음은 표시만 | CHAIN_CODE |
| B11 | 2 Chain 항목 | Chain 1에만 붙이고 표시. `GDS_CHAIN_NAME2`는 2차 | 지금 DDL 추가 |
| B12 | E-Test 판정 | 저장된 `JUDGE_FLAG` + Spec 바뀜 경고 | 분석 때 다시 판정 |
| B13 | 화면 A 표시 | 색 기준 콤보 4개 + Fail Die 빨간 테두리 + 탭 열 추가 / 신규 탭 2개 | 별도 화면 |
| B14 | 대량 처리 | 결과 축소 + `AnalyzeDie` + Binary 전송 + 숫자 생략 + Defect 300,000 상한 | 지금 구조 유지 |
| B15 | E-Test Fail 역추적 | 1차 포함: 화면 A "E-Test Fail" 탭(가장 가까운 Defect 거리 포함) + 화면 B 역추적 열기 (15.3) | Chain 탭 4분류로만 보기 |
| B16 | 여러 KLARF Step (2026-10-15 변경 -> **2026-10-20 2차**) | **2차 (2026-10-20 사용자: "KLARF 탭의 Step 표를 없애고 … 설계 B16 은 2차로 돌려 기록")**: 화면 A 는 DMSearch 에서 고른 Step 1개만 분석한다. KLARF 탭 = 고른 Step 정보 + 검사 면 [BS / FS] + Pitch 출처 (16.6). 서버 Analyze 의 `StepSeqs[]` / `ViewSides[]` 합치기는 그대로 둔다 (16.7). 이전 1차안(2026-10-15 ~ 10-19): [준비] 탭에서 체크한 Step을 합쳐 판정, Step마다 보기 기준, 결과에 Step 열 | 1차 포함 (2026-10-15 ~ 10-19 안) |
| B17 | 2 Chain 항목 Short 후보 | 2차: B11 `GDS_CHAIN_NAME2`와 함께, 두 Chain 모두 허용오차 안인 Defect 표시 (15.5) | 1차 포함 (DDL 필요) |
| B18 | Wafer Key 규칙 | `WaferKeyRule` 한 파일에 둔다. 잠정 규칙 = Lot 핵심 + Slot, 고객 협의 규칙이 오면 이 파일만 바꾼다 (16.2) | 규칙 확정 전에는 수동 연결만 |
| B19 | E-Test 공정 | 한 번에 1개, 기본 = Test 시각 최신 (16.6). **2026-10-20 B29: 공정 = E-Test Step(파일 Step ID), [E-Test] 탭 Step 콤보로 고르고 그 Step 파일 중 1개** | 여러 공정 판정 합침 (어느 공정 Fail인지 흐려짐) |
| B20 | Step 간 같은 결함 | 1차는 Step별로 따로 세고 Step 열로 구분. 같은 결함 묶기는 2차 (16.7). **2026-10-20: B16 이 2차라 1차 화면에는 생기지 않음** | 1차에 ±100 µm로 묶기 |
| B22 | 색 기준 콤보 (2026-10-16) | **숨김**, 기능 유지(기본 Chain Defect 수) | 표시 |
| B21 | Chain 면 (2026-10-16 사용자 선택) | **분석에 1개**: [준비] 탭 GDS 줄 "Chain 면 [FS / BS]"(기본 FS -> **2026-10-20 사용자 "Default로 BS" 로 기본 BS**). Step 표의 "보기 기준" 은 "검사 면" 으로 바꿔 Step 쪽 면만 뜻한다(Step 표는 2026-10-20 삭제 -> KLARF 탭 "검사 면 [BS / FS]" 콤보). 좌우 반전 = Chain 면 ≠ 검사 면 (18절). **2026-10-20 B28: 검사 면은 FS 고정, 화면에서 고르지 않음** | Chain 마다 FS / BS |
| B23 | Scribe 위치 (2026-10-16 추가, 결정 대기) | Device 별 기준정보 "가운데(기본) / 오른쪽 위 / 직접 입력" + [준비] 탭 Step 별 변경, Scribe 보정 X / Y 기본값 자동 계산 (20.3) | 조건 줄 보정 X / Y 손 입력만 (지금) |
| B24 | GDS Chip 크기 저장 (2026-10-16 추가, 결정 대기) | Map 저장 때 전체 Layer 외곽을 `TQP_GDS_MAP` 에 기록 (컬럼 4개), 기존 Map 은 보정 SQL (20.4) | 분석 때 저장 도형 범위로 계산 (저장 Layer 에 따라 달라짐) |
| B25 | Pitch 비교 점검 (2026-10-16 추가, 결정 대기) | [준비] 점검에 Pitch 와 Chip 크기 비교(정보 / 오류 / 주의) + Chip 밖 Defect 를 Chip 외곽 기준 `OUT_CHIP` 으로 표시 (20.5) | 비교 없음 (지금) |
| B26 | KLARF Ready 의 Product (2026-10-16 **확정**) | 고른 Step 과 같은 Product 의 KLARF 만 모은다 (22절) | Product 무관(지금, 같은 파일을 다른 Product 로 올리면 Defect 를 두 번 셈) |
| B28 | KLARF 검사 면 (2026-10-20 **사용자 확정**) | **FS 고정, 화면에 보이지 않음**. 고르는 것은 GDS Chain 면만(기본 BS). 사용자: "Klarf는 검사면이 FS가 Default다. GDS만 선택가능하고 Default나 BS다. Klarf에서는 검사면을 보여주지마라. Klarf, E-test는 그대로 사용한다." 반전 규칙(Chain 면 ≠ 검사 면)은 그대로라 기본 조건 Chain BS · 검사 FS 는 반전. 검사 면은 반전에만 쓰고 KLARF Die 번호 / E-Test Die 는 그대로(6.4 / 18절). 서버 Ready `VIEW_SIDE` = FRONT, Analyze 요청 검사 면이 비면 FRONT | 검사 면 콤보(BS 기본, 결정 A6, 2026-10-16 ~ 10-20 오전) |
| B27 | 같은 Step 재검사 (2026-10-16 **확정**) | `TQD_STEP.TEST_ORDER` 순서로 정렬, 같은 Step ID 는 최신(가장 작은 값)만 기본 체크 (22절) | 모두 체크 |
| B29 | E-Test 목록 조건 (2026-10-20 **사용자 확정**) | 사용자: "Wafer ID, Device, Step이 동일한 것만 E Test 결과 List에 나와야 한다." -> Wafer Key 같음 + E-Test `DEVICE_ID` = 분석 GDS Device + Step 콤보(E-Test Step = `TEST_PROGRAM`, 원본 `Step ID:` 칸과 같음)로 고른 Step 파일만, 그 안에서 1개. Device 가 다른 같은 Wafer 파일은 점검 `ET_OTHER_DEVICE`(상태줄)로만 알리고 [직접 찾기]로 더할 수 있다. 상세: `2026-10-20_ETest_목록_조건_설계.md` | Wafer ID 글자 일치 / KLARF Product 비교 / KLARF Step 대응표 |

## 12. 구현 순서 (승인 후)

0. **확인 Probe (읽기 전용)**: Chain Member 도형 통계 / 점 수, 중첩 SREF 회전·배율, Type 4 여부 (4.6).
1. **GDS 모양 거리 + Cache + Gap + 결과 축소** (B1 ~ B4, B14).
   - Probe: PATH Type 0 / 1 / 2와 꺾임 합성 도형의 거리를 손계산과 대조한다.
   - Probe: U9 STEP 501 / 541에서 Bounds 결과와 비교하고, Cache 적중을 확인한다.
2. **KLARF 쪽**: 필요 컬럼 쿼리 / 분류 기준 / 큰 Defect / Pitch 출처 / 여러 Step 합치기 (B5 ~ B7, B16, B20).
3. **Ready + E-Test 연결**: `WaferKeyRule` + `GetWaferReady` + 쿼리 2개, E-Test 쿼리 2개 + 이름 / Die 연결 + 4분류 + Chain Fail률 (B8 ~ B12, B18, B19).
   - Probe: `WaferKeyRule` 잠정 규칙을 샘플 2쌍으로 확인한다(1쌍 맞음 / 2쌍 다름이 기대값).
   - Probe: RH-HS2615-02 ↔ XD106.02로 10-06 회의 "27번 Die" 사례를 대조한다.
4. **화면**: 화면 A([준비] 탭 / [직접 찾기], B13, B15 E-Test Fail 탭) → 화면 B(`AnalyzeDie`, E-Test 값 탭, Fail 외곽선, 역추적 열기).
5. **합성 대량 Probe** (9.3) → 사용자 DMS 화면 확인 (배포는 사용자 직접).
6. **Scribe / Chip 크기 / Pitch 점검** (B23 ~ B25, 결정 후): DDL → Map 저장 / 복사 + 기존 Map 보정 SQL → Ready 점검 → Analyze 보정 / Chip 밖 → 화면 (20.7). 3번(Ready)과 함께 해도 된다.

## 13. 확인 수준 / 고객 확인

- **소스 / 문서로 확인함**: GDS 저장 값 형식, E-Test 테이블 / 판정 / Chain 이름 매핑, Die 대응 규칙(Probe 11/11), 샘플 KLARF 값 범위.
- **가정**: 화소 = 크기 값 간격, Die 안 회전 잔차 2.6 µm, RDL 폭 / pad 간격(리뷰 이미지 눈금으로 눈 측정).
- **고객 확인 필요**:
  - 매칭 허용오차 값 (10-02 회의 "고객사와 논의된 범위", 수치 미정)
  - Wafer ID 매칭 규칙
  - Step별 Back / Front
  - Chip 위치(Scribe / Seal ring) 오프셋 -> 질문 문안 20.6 (Recipe Die 기준점 / Scribe 폭 / GDS 외곽과 Die 차이 / 위치 아는 Defect)
  - AOI 위치 정확도

## 14. 앞 문서와 달라지는 것

| 앞 문서 | 바뀌는 것 |
| --- | --- |
| 6.1 "Pitch는 KLARF 값" | 지금은 Map Setup 값이다. B6 |
| 6.3 겹침 판정 | 4.2 2단계 + Gap. 허용오차가 네모 확장에서 거리(둥근 확장)로 바뀐다 |
| 7.1 인자 | 요청 클래스 + `Analyze` / `AnalyzeDie` (10.1) |
| 7.2 `DEFECT_HIT` | `DEFECT_CHAIN` + `AnalyzeDie`의 Member 단위 |
| 7.3 화면 B가 Wafer 결과를 받음 | `AnalyzeDie` 결과를 받음 |
| 9절 E-Test 확장 자리 / Die 잠정식 | 이 문서 6 ~ 8절. 잠정식 폐기 |

## 15. E-Test Fail 역추적 (2026-10-15 추가)

사용자 질문 (2026-10-15): "E Test 결과에 대한 Fail 발생된 Chain 기준으로 역 추적 할 수 있는 기능도 포함인가?"

역추적은 **E-Test Fail (Die, Chain)에서 출발해 그 Chain 위 / 근처의 Defect와 위치를 찾는 흐름**이다. E-Test는 Chain이 끊겼거나(Open) 붙었다(Short)는 것만 알려 주고 위치는 모른다. 그래서 위치 후보는 그 Chain 근처의 KLARF Defect다.

### 15.1 1 ~ 14절 설계로 되는 것

- Chain 탭에서 Chain을 고르고 색 기준을 "선택 Chain 4분류"로 두면, 일치 / Fail만 Die가 보인다.
- 그 Die를 고르면 화면 B Chain 목록에 Fail Chain(ET F / 분류)이 보이고 도면에는 Chain 색으로 그려지며(2026-10-16 변경), E-Test 값 탭에 Fail Parameter가 보인다.

### 15.2 빠진 것

| 빠진 것 | 이유 |
| --- | --- |
| Fail 목록에서 출발하는 표 | Chain 하나씩 바꿔 가며 봐야 한다. Wafer 전체 Fail (Die, Chain)을 한 번에 훑을 수 없다 |
| "Fail만" Die의 가까운 Defect | 결과 축소(9.2)로 허용오차 안 Defect만 보낸다. 허용오차보다 조금 멀리 있는 Defect(좌표가 밀린 경우)가 안 보인다 |
| 여러 KLARF Step | 분석은 KLARF Step 1개 기준이다. 같은 Wafer의 SiN / RDL / C4 Bump 검사 Defect를 같이 볼 수 없다 |
| 2 Chain 항목(누설 / Short) | `GDS_CHAIN_NAME`이 1칸이라 두 Chain 사이에 걸친 Defect를 찾을 수 없다 |

### 15.3 1차에 넣을 것 (B15)

**화면 A 신규 탭 "E-Test Fail"**: 행 = E-Test 상태가 F인 (Die, Chain).

| 열 | 내용 |
| --- | --- |
| Die (X, Y) | 시스템 Die 좌표 |
| Chain | Code / 이름 / 색 |
| Fail Parameter | 첫 Fail 항목 이름 / 값 / Lower ~ Upper |
| Fail Para 수 | Fail 수 / 연결 Parameter 수 |
| 분류 | 일치 / Fail만 |
| 걸린 Defect | 허용오차 안 Defect 수 |
| 가장 가까운 Defect | Gap(µm) / DEFECTID. 탐색 거리 50 µm 안에 없으면 '-' |

- 기본 정렬: 일치 → Fail만 순서, 그 안에서는 가장 가까운 Gap 순서.
- "Fail만"인데 가장 가까운 Defect가 6 ~ 50 µm에 있으면, 허용오차나 보정 X / Y를 다시 볼 대상이다. Defect가 아예 없으면 다른 Step 결함이거나 검사로 안 보인 결함일 수 있다.
- 데이터: `DIE_CHAIN`은 이미 ET 상태 ≠ P인 행을 모두 보낸다(10.2). 그래서 F 행은 다 있다. 2단계 판정이 탐색 거리 50 µm까지 Gap을 이미 구하므로, `MIN_GAP` / `NEAR_DEFECT_ID`를 그 값으로 채운다. 쿼리 / 계산 추가는 없다.

**화면 B 역추적 열기**: E-Test Fail 탭에서 행을 더블클릭하거나 Map 우클릭 [Detail Analysis]를 쓴다.

- 그 Chain만 진하게, 나머지 Chain은 흐리게 그린다. 화면은 Chain 범위로 맞춘다(지금 `FocusChainBounds`).
- Defect 목록은 그 Chain까지의 Gap 순서로 보이고, 허용오차 밖(50 µm까지)도 포함한다. 행을 고르면 그 Defect와 가장 가까운 Member를 강조한다(지금 동작).
- E-Test 값 탭은 그 Chain의 Parameter만 먼저 보인다.
- `AnalyzeDie`에 "기준 Chain" 인자를 더해, 그 Chain에 대한 Defect별 Gap을 돌려준다. Die 1개라 부담이 없다.

### 15.4 여러 KLARF Step (B16, 2026-10-15 변경: 1차 포함 -> 2026-10-20 2차)

- **2026-10-20 2차로 변경**: 화면 A 는 Wafer 1개 · 고른 Step 1개만 분석한다(KLARF 탭 Step 표 삭제). 아래는 2차 때 참고할 내용이며, 서버 Analyze 의 여러 Step 합치기는 남아 있다.
- Wafer 중심 컨셉(16절)으로 같은 Wafer Key의 KLARF Step을 [준비] 탭에 모으므로 1차에 포함한다.
- 체크한 Step의 Defect를 합쳐 판정한다. 역추적 목록의 "가장 가까운 Defect"에는 Step ID가 붙는다.
- KLARF Lot ID에 Step 이름이 들어 있어(예: `PFIC_RH-HS2615_C4Bump`) DMS Wafer 키(LOT_ID + WAFER_ID)로는 Step끼리 묶이지 않는다. 그래서 묶는 기준은 `WaferKeyRule`이다(16.2).

### 15.5 2 Chain 항목 Short 후보 (B17, 2차)

- B11의 `GDS_CHAIN_NAME2`가 생기면, 누설 항목이 Fail일 때 두 Chain 모두에 Gap ≤ 허용오차인 Defect를 "Short 후보"로 표시한다.
- 한쪽 Chain에만 가까운 Defect는 일반 후보로 둔다.

## 16. Wafer 중심 Ready 컨셉 (2026-10-15 변경)

사용자 지시 (2026-10-15):

> 3개의 Data는 Wafer ID 로 묶을 수 있다. DMS의 조회 화면에서 Wafer를 선택해서 조회 하면 각각의 정보들은 Wafer ID 및 Device 정보 기반으로 물고 들어와서 상세 분석전의 Ready 상태로 만들 수 있지 않나?
> WaferID 에 대한 Rule은 고객사와 협의해서 맞출거다. 해당 컨셉으로 설계를 해줘.

### 16.1 바뀌는 것

| 항목 | 이전 (KLARF Step 중심) | 변경 (Wafer 중심) |
| --- | --- | --- |
| 출발점 | DMSearch에서 고른 KLARF Step 1개 | DMSearch에서 고른 Step의 **Wafer** |
| KLARF | 고른 Step 1개 | 같은 Wafer Key의 모든 Step을 모아 표로 보이고, 체크한 Step을 함께 분석 |
| E-Test | 엔지니어가 목록에서 파일 선택 | 같은 Wafer Key의 파일을 공정별로 모아 표로 보이고, 공정 1개 선택 |
| GDS | Product와 이름이 같은 Device | 같음 (Device 기준) |
| 면 선택 | 보기 기준 화면에 1개 | Chain 면(분석에 1개, GDS 줄) + 검사 면(Step 마다, Step 표의 열) (18절) |
| 분석 전 | 바로 [분석] | Ready 조회(목록만) → [준비] 탭에서 상태 확인 → [분석] |

GDS 판정(4절), 허용오차(5절), E-Test 집계 / Chain 연결(6절), 4분류(7절), 리소스(9절), 역추적(15절)은 그대로다.

### 16.2 Wafer Key 규칙 (고객 협의 대상, B18)

- **Wafer Key**는 세 데이터가 같은 실물 Wafer인지 판단하는 문자열이다. 규칙은 고객사와 협의해 정한다.
- 규칙은 신규 `02_DMS/DACrux.SEMDMS.Interface/Coordinate/WaferKeyRule.cs` 한 파일에만 둔다.
  - `DieCoordinateMapper`와 같은 방식이다. System만 쓰고, 서버와 화면이 같이 쓴다.
  - 규칙이 바뀌면 이 파일만 고친다.

```csharp
public static class WaferKeyRule
{
    public const string RuleName = "LOT_CORE_SLOT";   // 잠정 규칙 이름. 고객 규칙으로 바꿀 때 함께 바꾼다
    public const bool Confirmed = false;              // 고객 협의 전 = false. 화면 상태줄에 "규칙 잠정" 표시

    /// KLARF 1행(TQD_INSP_INFO + TQD_STEP.COMMENT의 RAW_WAFER_ID)으로 Wafer Key를 만든다. 만들 수 없으면 null
    public static string FromKlarf(string lotId, string waferId, string slotId, string rawWaferId);

    /// E-Test 파일 1행(TQP_GDS_SRC_FILE)으로 Wafer Key를 만든다
    public static string FromETest(string lotId, string slotNo, string lotWaferNo);

    /// DB 후보 조건(넓게). 같은지 판단은 FromKlarf / FromETest 결과 비교로 한다
    public static WaferKeySearch SearchOf(string waferKey);   // { LotLike, Slot }
}
```

- DB에서는 넓게 후보를 거르고(Lot 글자 + Slot), 같은지는 C# 규칙으로 판단한다. 그래서 규칙이 바뀌어도 쿼리는 대부분 그대로다.
  - 후보 쿼리는 규칙이 쓸 수 있는 컬럼(LOT_ID / WAFER_ID / SLOT / 원본 Wafer ID / LOT_WAFER_NO)을 모두 돌려준다.
  - 고객 규칙이 다른 컬럼(예: 원본 Wafer ID)으로 찾게 되면 `SearchOf`와 후보 쿼리 조건 1줄만 바뀐다.
- **잠정 규칙** (고객 규칙 전, 샘플 기준):
  - KLARF: LOT_ID를 `_`로 나눠 3조각 이상이면, 첫 조각(접두어)과 마지막 조각(Step 이름)을 뺀 가운데를 Lot 핵심으로 본다. 3조각 미만이면 LOT_ID 전체를 쓴다. Key = 대문자(Lot 핵심) + `-` + Slot 2자리.
  - E-Test: Key = 대문자(LOT_ID) + `-` + Slot 2자리 (= 저장된 `WAFER_ID`).
- 샘플 확인값 (Probe 기대값):

| KLARF | Wafer Key | E-Test | 결과 |
| --- | --- | --- | --- |
| `PFIC_RH-HS2615_C4Bump` Slot 2 | `RH-HS2615-02` | `RH-HS2615-02` | 맞음 |
| `PFIC_U9-FS2635_C4Bump` Slot 1 | `U9-FS2635-01` | `U9-HS2635-01` | 다름 → [직접 찾기] |

### 16.3 Device

- GDS Device는 KLARF Product와 이름이 같은 READY Map이다(지금 동작). 없으면 GDS Device 콤보에서 직접 고르거나 [Map 복사]를 안내한다.
- E-Test는 Wafer Key로 찾으므로 Device 이름이 달라도 된다(예: KLARF `OMM_Hybrid` / E-Test `OMM`). [준비] 탭 E-Test 표에 E-Test Device를 열로 보인다.
- Chain 연결은 E-Test Device의 Parameter 등록(`GDS_CHAIN_NAME`)을 분석 GDS Device의 Chain 이름에 맞춘다(6.5). 연결 수는 Ready 단계에서 미리 센다.
- Die 연결용 Map은 KLARF Setup Die 목록과 E-Test Map(`MAP_ID` = E-Test Device)을 쓰고, 형상이 다르면 [준비] 탭에 주의로 보인다(6.4).

### 16.4 Ready 조회 `GetWaferReady` (목록만)

DMSearch에서 Wafer를 고르거나 GDS Device를 바꾸면 서버를 한 번 부른다.

| 순서 | 내용 | 쿼리 |
| --- | --- | --- |
| 1 | 고른 Step 정보 → Wafer Key 계산 | 기존 `SELECT_DMS_WAFER_INFO` + `TQD_STEP.COMMENT`(RAW_WAFER_ID) |
| 2 | 같은 Wafer의 KLARF Step: 후보를 읽고 `FromKlarf`로 거름 | 신규 `TQD_INSP_INFO.xml SELECT_KLARF_STEP_BY_WAFER` |
| 3 | 같은 Wafer의 E-Test 파일: 후보를 읽고 `FromETest`로 거름 | 신규 `TQP_GDS_SRC_FILE.xml SELECT_ET_FILE_BY_WAFER` |
| 4 | GDS Device / READY Map / READY · DRAFT Chain 수 / Chain 저장 시각 | 기존 `SELECT_MAP_INFO` / `SELECT_CHAIN_LIST` |
| 5 | E-Test Chain 이름 연결 수 (공정별) | 기존 `GetEtParameterList` 결과의 `GDS_CHAIN_NAME`을 4의 Chain 이름과 비교 |
| 6 | Die Map 형상 비교 | 기존 `SELECT_X_Y`(KLARF Setup) / `SELECT_ET_MAP_DIE` |

후보 쿼리 (조건만, 컬럼은 구현 때 확정):

```sql
-- SELECT_KLARF_STEP_BY_WAFER
SELECT A.STEP_SEQ, A.LOT_ID, A.WAFER_ID, A.SLOT_ID, A.STEP_ID, A.PRODUCT, A.RESULTTIMESTAMP,
       A.DEFECTS, A.INSPECTED_DIE, A.SETUP_SEQ, S.COMMENT
  FROM TQD_INSP_INFO A, TQD_STEP S
 WHERE S.STEP_SEQ = A.STEP_SEQ
   AND A.FACTORY = :factory
   AND UPPER(A.LOT_ID) LIKE :lot_like          -- WaferKeyRule.SearchOf
   AND A.SLOT_ID = :slot_id
   AND A.RESULTTIMESTAMP BETWEEN :from_time AND :to_time   -- 고른 Step 검사 시각 ± 기간 (16.8)

-- SELECT_ET_FILE_BY_WAFER
SELECT SRC_FILE_SEQ, WAFER_ID, LOT_ID, SLOT_NO, LOT_WAFER_NO, DEVICE_ID, TEST_PROGRAM, TEST_TIME,
       DIE_COUNT, PASS_DIE_COUNT, FAIL_DIE_COUNT, JUDGE_TIME, MAP_ID, (Spec 바뀜: SELECT_SRC_FILE_BROWSE와 같은 식)
  FROM TQP_GDS_SRC_FILE
 WHERE FACTORY = :factory
   AND UPPER(LOT_ID) LIKE :lot_like
   AND SLOT_NO = :slot_no
   AND LOAD_STATUS = 'READY'
```

- 무거운 데이터(GDS Member 모양, Defect, E-Test 값)는 읽지 않는다. Defect 수는 업로드 때 저장된 `TQD_INSP_INFO.DEFECTS`를 쓴다.
- 결과 DataSet:

| 테이블 | 행 | 주요 열 |
| --- | --- | --- |
| `WAFER_KEY` | 1 | WAFER_KEY / RULE_NAME / CONFIRMED / 고른 STEP_SEQ / FACTORY |
| `READY_GDS` | 1 | DEVICE_ID / MAP_STATUS / READY_CHAIN / DRAFT_CHAIN / CHAIN_LAST_SAVED / CACHE_HIT(이미 Cache에 있는지) |
| `READY_KLARF` | Step | STEP_SEQ / STEP_ID / LOT_ID / RESULTTIMESTAMP / DEFECTS / INSPECTED_DIE / VIEW_SIDE(기본 BACK) / PITCH_SOURCE / ADDED_BY(규칙 / 직접) |
| `READY_ET` | 파일 | SRC_FILE_SEQ / TEST_PROGRAM / WAFER_ID / DEVICE_ID / TEST_TIME / DIE_COUNT / FAIL_DIE_COUNT / JUDGE_TIME / SPEC_CHANGED / LINKED_CHAIN / UNLINKED_NAME / MAP_SHAPE_SAME / ADDED_BY |
| `READY_CHECK` | 점검 | CODE / LEVEL(오류 / 주의 / 정보) / MESSAGE |

### 16.5 Ready 상태와 점검

| 점검 | 수준 | 조건 | 결과 |
| --- | --- | --- | --- |
| GDS Map 없음 | 오류 | Device의 READY Map 없음 | [분석] 불가. Device 직접 선택 / [Map 복사] 안내 |
| GDS Chain 없음 | 오류 | READY Chain 0개 | [분석] 불가 |
| DRAFT Chain | 주의 | 저장 미완료 Chain 있음 | 분석에서 빠짐 (개수 표시) |
| KLARF Step 1개 | 정보 | 같은 Wafer Step을 1개만 찾음 | 규칙이 잠정이면 [직접 찾기] 확인 권장 |
| E-Test 없음 | 정보 | 같은 Wafer 파일 없음 | Defect만 분석 (E-Test 열은 '-') |
| Spec 바뀜 | 주의 | 판정 뒤 Spec 변경 | 판정 다시 하도록 안내 |
| Chain 이름 미연결 | 주의 | `GDS_CHAIN_NAME`이 GDS에 없음 / 중복 | E-Test 연결 탭에서 확인 |
| Map 형상 다름 | 주의 | KLARF Map / E-Test Map 사용 Die 범위 / 수 다름 | Die 연결 결과 확인 필요 |
| 규칙 잠정 | 정보 | `WaferKeyRule.Confirmed = false` | 상태줄 표시 |
| Pitch 출처 | 정보 | KLARF 원본 Pitch 없음 | Map Setup Pitch 사용 (B6) |

- **[분석] 가능 조건** = 오류 없음 + 체크한 KLARF Step ≥ 1. E-Test는 없어도 된다. (2026-10-20: Step 은 Map 을 그린 Step 1개로 정해져 체크 조건 없음)

### 16.6 화면 A [준비] 탭

> **2026-10-20 변경 (B16 2차)**: KLARF Step 표 / KLARF [직접 찾기] 삭제. KLARF 탭 위쪽 = 고른 Step 정보 두 줄(Step / Lot, 검사 시각 / Defect / 검사 Die) + "Pitch 출처: Map Setup / KLARF" (같은 날 검사 면 [BS / FS] 콤보도 삭제, KLARF 검사 면 FS 고정 = B28). GDS / KLARF / E-Test 탭 제목 = "(Wafer 1개만 분석)", Wafer 여러 개 선택 오류 = "1개 Wafer 만 분석할 수 있습니다. Wafer 를 하나만 선택하세요. (선택 n개)". [준비] 탭 자체는 2026-10-19 기능 탭(GDS / KLARF / E-Test / 종합 (Die))으로 바뀌었다([화면 A 레이아웃 후보](2026-10-19_화면A_레이아웃_후보.md)). 아래 그림은 2026-10-16 안이다.

Wafer를 고르면 오른쪽 탭 맨 앞의 [준비] 탭이 열린다. 분석 결과가 나오면 Chain 탭으로 넘어간다.

```text
[준비] [구간] [Die] [Chain] [Defect] [허용오차] [E-Test 연결] [E-Test Fail]
 Wafer Key  RH-HS2615-02   (규칙 잠정 · Lot 핵심 + Slot)                         [다시 조회]
 GDS        OMM_Hybrid  READY / Chain 15 (DRAFT 0) / 저장 10-15 14:30
 KLARF Step                                                              [직접 찾기]
   [v] C4Bump_Silterra  PFIC_RH-HS2615_C4Bump  10-06 09:12  Defect 15  [Back v]  Pitch KLARF
   [v] RDL              PFIC_RH-HS2615_RDL     10-04 18:40  Defect 42  [Back v]  Pitch Map Setup
 E-Test                                                                  [직접 찾기]
   (o) TSVET  RH-HS2615-02  OMM  10-07  Die 24  Fail 3  판정 10-14  Spec 그대로  Chain 연결 5 / 미연결 2
   ( ) COWET  RH-HS2615-02  OMM  10-09  Die 24  Fail 1  판정 10-14  Spec 바뀜
   ( ) E-Test 없이
 점검  주의: Chain 이름 미연결 2 (DC_P9, BR_N3) / 정보: 규칙 잠정
```

(RDL Step / COWET 행은 설명용 예시다. 실제 샘플에는 C4Bump / TSVET만 있다.)

- GDS 줄: Device / Map 상태 / READY · DRAFT Chain / 저장 시각 / Cache + **Chain 면 [FS / BS] 콤보(기본 FS, 18절 -> 2026-10-20 기본 BS)**.
- KLARF Step 표: [체크] / Step ID / Lot ID / 검사 시각 / Defect 수 / 검사 Die / 검사 면(BS / FS 콤보, 기본 BS) / Pitch 출처 / 추가 방식(규칙 / 직접).
- E-Test 표: (선택) / 공정 / WAFER_ID / Device / Test 시각 / Die / Fail Die / 판정 시각 / Spec 바뀜 / Chain 연결 / 추가 방식. 맨 아래에 "E-Test 없이" 행이 있다. 기본 선택은 Test 시각이 가장 최근인 공정이다 (B19).
  - **2026-10-20 B29**: 목록 = Wafer Key 같음 + Device = 분석 GDS Device 인 파일만. 표 위 Step 콤보(E-Test Step = 공정)로 Step 을 고르면 그 Step 파일만 보이고 1개 선택. 열 머리 "공정" -> "Step". 상세: `2026-10-20_ETest_목록_조건_설계.md`.
- [직접 찾기]: 규칙으로 못 찾은 Step / 파일을 목록 창에서 더한다(기존 목록 서비스). 더한 행은 "직접"으로 표시한다. 저장하지 않으므로 Wafer를 다시 고르면 사라진다.
- 체크 / 선택 / 보기 기준을 바꾸면 [분석]을 다시 누른다. GDS Cache가 있어 다시 분석해도 GDS는 다시 읽지 않는다.
- 조건 줄에서 보기 기준 콤보와 E-Test [선택] / [해제]는 없앤다(이 탭으로 옮김).

### 16.7 분석 (여러 Step 합치기)

- **2026-10-20**: 화면은 `StepSeqs` / `ViewSides` 에 고른 Step 1개 + 검사 면 FRONT(FS 고정, B28)만 보낸다. 시스템 Die 좌표는 검사 면과 상관없이 KLARF Index 그대로(6.4). 서버의 여러 Step 합치기는 그대로 두고(BSL `GdsKlarfAnalyzeProbe` 로 확인), 화면 연결은 2차(B16)다.
- 요청은 `StepSeqs[]` / `ViewSides[]` / `DisplayStepSeq` / `EtSrcFileSeq`다 (10.1).
- Step마다 Defect를 읽어 그 Step의 Pitch / 검사 면으로 Chip 좌표(반전 = Chain 면 ≠ 검사 면)와 시스템 Die 좌표(검사 면)를 계산한 뒤, 합쳐서 판정한다. GDS Cache는 하나다.
- Die 집계는 시스템 Die 좌표로 한다. 화면 A Map은 DMSearch에서 고른 Step(`DisplayStepSeq`)의 Index로 그린다. 다른 Step의 Die는 시스템 좌표를 거쳐 그 Index로 바꾼다(`DieCoordinateMapper.Convert`).
- Map 위의 Defect 점은 지금처럼 고른 Step 것이다. Die 색 / 숫자는 체크한 Step 합계다. 상태줄에 "색 = Step n개 합"을 보인다.
- Defect 키는 (STEP_SEQ, DEFECTID)다. Defect / E-Test Fail 탭과 화면 B Defect 목록에 Step 열을 둔다.
- **Step 간 같은 결함 (B20)**: 같은 결함이 다음 Step 검사에도 잡히면 두 번 센다.
  - 1차는 그대로 세고 Step 열로 구분한다.
  - 같은 결함 묶기는 2차다. 기존 New Defect 판정(Wafer X / Y ±100 µm, `DefectHelper`)과 같은 방식을 검토한다.

### 16.8 리소스

- Ready는 목록만 읽는다(Step 수 + 공정 수 행). 무거운 읽기는 [분석] 때만이다.
- 후보 쿼리의 `LIKE '%Lot 핵심%'`는 인덱스를 못 탄다. 범위를 줄이려고 FACTORY + SLOT + 검사 시각 기간(고른 Step 기준 ± 180일, 설정값)을 같이 건다. 고객 규칙이 정확히 같은 값을 비교하는 방식이면 `=` 조건으로 바꾼다.
- Step을 여러 개 체크하면 Defect 수가 Step 수만큼 는다. 대책은 9절과 같고, Defect 상한은 Step 합계 기준이다.
- GDS Cache는 Step 수와 상관없이 Device당 하나다.
- Ready 때 GDS Cache를 미리 만들면 첫 [분석]이 빨라지지만, 그만큼 Ready가 느려진다. 1차에서는 하지 않고, `READY_GDS.CACHE_HIT`로 Cache가 이미 있는지만 보인다.

## 17. 1차 화면 레이아웃 시안 (2026-10-16)

사용자 지시 (2026-10-16): "설계안 대로 작업 시작. 1차 화면에 대한 레이아웃 시안이 만들어지면 알려줘. 화면을 보고 server 개발 시작하자"

- 결정 B1 ~ B20 은 권장안으로 확정한다.
- 이번 단계는 **화면만** 바꿨다. 서버(BSL / RO / Interface / Query)는 화면 확인 뒤 시작한다.
- 화면은 10.2 / 16.4절 결과 표 계약을 읽게 만들었다. 열이나 표가 없으면(지금 서버 결과) 그 칸을 비워 두므로, 지금 서버로도 기존 기능은 그대로 동작한다(Smoke 36/36).

### 17.1 시안 이미지

폴더: [2026-10-16_1차화면_시안](2026-10-16_1차화면_시안/)

| 이미지 | 화면 | 보이는 것 |
| --- | --- | --- |
| A1_준비탭_분석전 | 화면 A [준비] 탭 | Wafer Key(규칙 잠정) / GDS 1행 / KLARF Step 표(사용 체크, Step 별 보기 기준 콤보, Pitch 출처, 추가 방식) / E-Test 표(공정 1개 선택, Spec 바뀜 주황) / 점검 표, [다시 조회] / [직접 찾기] 2개 |
| A2_결과_Chain탭 | 화면 A 결과 | 색 기준 = 일치 Chain 수, E-Test Fail Die 모서리 표식, 구간 표에 "일치 없음 / 비교 불가" 행, Chain 탭 = E-Test 연결 / Defect · Fail · 일치 Die / Fail률 2개 |
| A3_결과_ETestFail탭 | 화면 A 결과 | 색 기준 = 선택 Chain 4분류(구간 표 = 일치 / Fail만 / Defect만 / 정상 / 비교 불가), E-Test Fail 탭(일치 먼저, 가까운 Defect 거리 순) |
| A4_결과_허용오차탭 | 화면 A 결과 | 색 기준 = Chain Defect 수(기존), 허용오차 탭(0 / 2 / 5 / 10 / 20 / 50 µm) |
| A5_결과_ETest연결탭 | 화면 A 결과 | 색 기준 = E-Test Fail Chain 수, E-Test 연결 탭(연결 / GDS 에 없음 / 이름 중복, 2 Chain 항목 수) |
| A6_직접찾기_ETest | [직접 찾기] 창 | 비교용 KLARF 정보 / Device · 공정 · Lot 조건 / 목록 / [추가] |
| B1_상세_역추적_Defect탭 | 화면 B 역추적 | 위쪽 "역추적: Chain R ... - E-Test Fail" + [역추적 해제], Chain 목록 분류 / ET / Fail Para, 그 Chain 만 빨간 외곽선, Defect 를 그 Chain 까지 거리 순 |
| B2_상세_역추적_ETest값탭 | 화면 B 역추적 | E-Test 값 탭(판정 / Parameter / 값 / Lower / Upper / GDS Chain / Chain / 헤더 원문) |
| B3_상세_Die전체 | 화면 B | [역추적 해제] 뒤 Die 전체 보기 |

- 이미지는 `02_DMS/DACrux.SEMDMS.ENGUI/Verification/Preview-GdsKlarfLayout.ps1 -StepSeq 528 -Device OMM_Hybrid` 로 다시 만든다(TEMP 빌드, 로컬 DB 조회만).
- **실제 값**: KLARF Wafer(STEP 528 = XD106.02, Defect 15), GDS 도면 / Chain(OMM_Hybrid 15개), Chain 영역 판정, 허용오차 탭 숫자(서버를 허용오차별로 불러 센 값).
- **예시 값(레이아웃 확인용, 판정 결과 아님)**: Wafer Key, 예시 RDL Step / COWET 행, E-Test 판정(P / F) / 4분류 / Fail률, E-Test 연결 표, E-Test 값, 허용오차 밖 가까운 Defect 거리, [직접 찾기] 목록(로컬 DB 에 E-Test 파일이 없어 예시 3행).
- 참고: XD106.02 는 OMM_Hybrid Chain 에서 허용오차 0 ~ 50 µm 모두 Chain 영역 Defect 1개다(U9 Chain 으로 7개였던 10-12 결과와 다름, Chain 구성 차이로 보임. 서버 개발 때 확인).

### 17.2 바뀐 소스 (화면만)

| 파일 | 내용 |
| --- | --- |
| `ENGUI/DefectMapAnalysis/frmGdsKlarfAnalysis.Designer.cs` | 조건 줄: 보기 기준 콤보 제거(→ [준비] 탭 Step 표 열), 분류 기준 / 큰 Defect 제외 / 색 기준 추가. 탭: 준비(맨 앞) / 허용오차 / E-Test 연결 / E-Test Fail 추가 |
| `ENGUI/DefectMapAnalysis/frmGdsKlarfAnalysis.cs` | `ShowReady` / `NewReadySet`(16.4 표 틀) / `BuildLocalReady`(서버 Ready 전: 고른 Step 1개), 분류 기준 RoughBin / Class, 색 기준 4개, Fail 표식, 새 탭 3개, E-Test Fail 행 → 역추적 상세, `ShowAnalysisResult`. 분석 호출은 지금 서버(`AnalyzeWafer`) 그대로이고, 여러 Step / E-Test / 큰 Defect / RoughBin 일부 거르기는 상태줄 / 안내로 "서버 반영 후" 를 알린다 |
| 신규 `ENGUI/DefectMapAnalysis/frmGdsKlarfSourceFind.cs` / `.Designer.cs` | [직접 찾기] 창. KLARF = 기존 `GetDMSStepWaferList`(DMSearch 와 같은 쿼리), E-Test = 기존 `GetEtFileBrowseList` |
| `ENGUI/DefectMapAnalysis/GdsKlarfAnalysisMap.cs` | E-Test Fail Die 모서리 삼각형(`SetFailDies`) |
| `ENGUI/DefectMapAnalysis/GdsKlarfColorBands.cs` | 4분류 색 / 문구, `GdsKlarfColorBasis` |
| `ENGUI/GdsMap/frmGdsDieAnalysis.cs` | `ShowDie(DataSet, x, y, 역추적 Chain)` (이전 GDS EXE 면 3인자로) |
| `06_CoreLibSource/GdsMapControl/GdsDieAnalysisForm.cs` / `.Designer.cs` | 역추적 표시 / [역추적 해제], Chain 목록 분류 / ET / Fail Para, Fail Chain 빨간 외곽선, 오른쪽 Defect / E-Test 값 탭, Defect Step / Gap 열, Chain 행 더블클릭 = 역추적 |
| `ENGUI/Verification/` | 신규 `Preview-GdsKlarfLayout.ps1` / `GdsKlarfLayoutPreview.cs`(시안 이미지), `GdsKlarfAnalysisFormSmoke.cs`(분류 기본 RoughBin / 메서드 오버로드), `Verify-GdsKlarfAnalysis.ps1`(기본 Step 528 / OMM_Hybrid) |

### 17.3 서버 개발 때 바꿀 화면 연결

- `BuildLocalReady` → `GetWaferReady` 결과로 바꾼다(같은 표 모양).
- `BuildRequest` / `Call` → `Analyze(GdsKlarfAnalysisRequest)` 로 바꾸고 "서버 반영 후" 안내를 지운다.
- 화면 B 는 `AnalyzeDie` 결과(그 Die 의 Defect 전체 / DEFECT_HIT / DEFECT_CHAIN / ET_VALUE)를 받는다.

## 18. Chain 면 / 검사 면 분리 (2026-10-16, 결정 B21)

> **2026-10-20 변경 (결정 B28)**: 사용자 "Klarf는 검사면이 FS가 Default다. GDS만 선택가능하고 Default나 BS다. Klarf에서는 검사면을 보여주지마라. Klarf, E-test는 그대로 사용한다."
> - KLARF 검사 면 = **FS 고정**, KLARF 탭에 검사 면을 보이지 않는다. 고르는 것은 GDS Chain 면 콤보 하나(기본 BS).
> - GDS 반전 규칙은 그대로(Chain 면 ≠ 검사 면). 기본 조건 Chain BS · 검사 FS = **반전**(BACK 보기), Chain FS 로 바꾸면 반전 없음(FRONT 보기).
> - KLARF Die 번호 / E-Test Die 는 면과 상관없이 그대로(아래 "KLARF Die 번호 반전 = 검사 면" 줄은 더 이상 맞지 않음, 6.4).
> - 서버: Ready `READY_KLARF.VIEW_SIDE` = FRONT, Analyze 요청 검사 면이 비면 FRONT, `ToSystem` / `FromSystem` = KLARF Index 그대로. 화면 A 상태줄 / 상세보기 "분석 기준" 칸 · GDS / KLARF 기준 줄도 검사 면을 보이지 않는다: "Chain BS -> 반전" / "Chain FS" / "· Chain 면 BS" (같은 날 사용자 "빼줘", CLAUDE-157).
> - STEP 528 / OMM_Hybrid / 5 µm: 기본(Chain BS · 검사 FS) 반전 Chain In **1 / 15**, Chain FS · 검사 FS 반전 없음 8 / 15. 2026-10-20 오전 기본(Chain BS · 검사 BS, 반전 없음 8 / 15)과 반대다.

사용자 지시 (2026-10-16): "분석 시에 chain 에 대한 FS/BS 선택 기능이 있어야 한다." -> 선택지 중 "분석에 1개" 선택.

근거: 10-13 회의 결정 "각 파일(GDS, KLARF, E-Test)이 Back side 인지 Front side 인지 분석 시 엔지니어가 Step 별로 선택한다". 1차 시안은 KLARF Step 의 "보기 기준" 하나로 GDS 반전과 KLARF Die 반전을 같이 정해 GDS(Chain) 쪽 면을 따로 고를 수 없었다.

| 선택 | 위치 | 기본값 | 뜻 |
| --- | --- | --- | --- |
| Chain 면 | [준비] 탭 GDS 줄 콤보, 분석에 1개 | ~~FS (GDS 원본은 Front side 로 그려짐)~~ **BS (2026-10-20 사용자 "GDS 관련 Chain 면은 Default로 BS")**. 화면 콤보 / 서버 Ready(READY_GDS.CHAIN_SIDE) / Analyze 요청에 Chain 면이 비었을 때 모두 BACK | GDS Chain 이 그려진 면 |
| 검사 면 | [준비] 탭 KLARF Step 표 열, Step 마다 | BS (결정 A6) | 그 KLARF Step 을 검사한 면 |

- **GDS 좌우 반전 = Chain 면 ≠ 검사 면.** 기본(FS / BS)은 반전이라 이전 동작(Back side 보기)과 같다.
- **KLARF Die 번호 반전(시스템 Die 좌표) = 검사 면만**(6.4절 KlarfBack / KlarfFront). GDS 반전과 따로 정해진다.
- E-Test 는 저장된 시스템 Die 좌표(MAP_X / MAP_Y)를 쓰므로 면 선택이 없다. FT 등 면이 다른 E-Test 가 생기면 E-Test 표에 같은 방식의 열을 더한다.
- 서버 계약: 요청 `ChainSide` + Step 별 `ViewSides`(검사 면). 서버는 Step 마다 "Defect 반전 = (ChainSide ≠ 검사 면) XOR (Map 저장 기준 VIEW_SIDE = BACK)" 로 계산한다(Map 은 원본 FRONT 저장이 원칙, 예전 BACK 저장 Map 도 같은 식으로 맞음). 결과 `ANL_INFO` 에 CHAIN_SIDE / STEP_SIDES 를 남긴다.
- 화면 B 는 Map 을 그리는 Step(DisplayStepSeq)의 반전으로 도면을 그리고, 다른 면 Step 의 Defect 좌표는 서버가 같은 보기 좌표로 바꿔 보낸다.
- 지금 서버(`AnalyzeWafer`, 반전 = Map 저장 기준 ≠ 보기 기준) 연결: 화면이 보기 기준 = (Chain 면 ≠ 검사 면 ? BACK : FRONT) 로 바꿔 넘긴다. 상태줄에 "(Chain FS · 검사 BS -> 반전)" 처럼 보인다.

확인 결과 (Smoke, STEP 528 = XD106.02 / OMM_Hybrid / 5 µm):

| Chain 면 / 검사 면 | 반전 | Chain 영역 Defect |
| --- | --- | --- |
| FS / BS (기본) | 예 | 1 / 15 |
| BS / BS | 아니요 | 9 / 15 |

- 10-12 에 U9(같은 도면, 10-15 OMM_Hybrid 로 복사) + STEP 501 + Back 으로 7 / 15 였다. 그 뒤 KLARF 가 Map Setup 필수로 다시 올라가(STEP 528) Pitch 출처가 Map Setup 값으로 바뀌었다(5.2절). 어느 면이 맞는지와 Pitch 영향은 서버 개발 때 허용오차 표 / 보정 X·Y 와 함께 확인한다(판단은 엔지니어, 고객 확인 대상).

변경 소스: `frmGdsKlarfAnalysis.Designer.cs`(GDS 줄 `cboChainSide`, Step 열 "검사 면" BS / FS), `frmGdsKlarfAnalysis.cs`(`ChainSide` / `ServerViewSide` / 상태줄, `READY_GDS.CHAIN_SIDE` 가 오면 기본값으로 사용), `Verification/GdsKlarfAnalysisFormSmoke.cs`(면 규칙 검사 추가, 37/37). 시안 이미지 A1 다시 만듦.

## 19. 1차 화면 정리 (2026-10-16 요청)

요청: 1) Wafer 단일 선택, 여러 개면 오류, Wafer 콤보 제거 / 2) GDS Device 는 Wafer 의 Device 를 따르고 없으면 분석 불가 / 3) 분류 기준 · Chain 거르기 · Die Index 표시는 지금 기준으로 두고 숨김 / 4) Defect 크기 + 허용오차 또는 Defect 중심 사각형을 라디오로 선택 / 5) 큰 Defect 제외는 기능은 두고 숨김 / 6) 색 기준 설명 후 결정 / 추가: 우클릭 메뉴의 Defect Select / Die Select 제거, 기본 Die 선택.

| 항목 | 반영 |
| --- | --- |
| Wafer | 조건 줄 Wafer 콤보 제거 -> "분석 Wafer" 읽기 전용 문구(WaferID / StepID / Product). DMSearch 에서 Step 이 2개 이상이면 "분석할 Wafer 는 하나만 선택하세요" 오류(메시지 창 + 상태줄), 지금 화면은 그대로. 같은 Wafer 의 다른 Step 은 [준비] 탭에서 모은다 |
| GDS Device | Device 콤보 제거 -> 읽기 전용 문구. KLARF Product 와 같은 이름(대소문자 무시)의 READY Map, 없으면 "없음 - 분석 불가"(빨강) + [준비] 점검 오류 + [분석] 안내("GDS Map 조회 화면 [Map 복사]") |
| 숨김 | 분류 기준(RoughBin) / 분류 메뉴(전체) / Chain 메뉴(READY 전체) / Die Index 표시(끔) / 큰 Defect 제외(끔). 컨트롤과 기능은 그대로 두고 Visible 만 끔 |
| Defect 영역 | 라디오 2개: "Defect 크기 + 허용오차 [5] µm"(기본) / "Defect 중심 사각형 (한 변) [10] µm". 고른 쪽 입력칸만 쓸 수 있다. 중심 사각형은 Defect 크기를 쓰지 않고 중심에서 한 변 길이의 정사각형으로 판정한다(지금 서버: 크기 끔 + 허용오차 = 한 변 / 2). 상태줄 / 화면 B 분석 기준 문구와 화면 B Defect 표시(정사각형, 점선 없음)도 같은 모양 |
| 우클릭 메뉴 | Free Zoom / Fit Size / Center Cross / Detail Analysis. Map 은 처음부터 Die 선택 모드(`DefectMap.DieSelectMode`) |
| 색 기준 | **숨김(결정 B22)**: 기능은 두고 기본 "Chain Defect 수" 로 고정. E-Test Fail 모서리 표식 / E-Test Fail 탭 / Chain 탭 Fail률은 그대로 |
| [준비] 탭 점검 표 | **삭제(2026-10-16 요청 "우측 하단에 점검 내용은 삭제")**: 라벨 / 표를 지우고 Step / E-Test 표가 자리를 나눈다. 서버 `READY_CHECK` 는 그대로 돌려주고 화면은 서버 준비 목록 조회 실패만 상태줄로 알린다. 다른 점검 내용은 이미 다른 칸에 보인다(GDS 없음 = Device 문구 빨강, DRAFT Chain = GDS 줄, Spec 바뀜 / Chain 미연결 = E-Test 표 열, 규칙 잠정 = Wafer Key 줄, Pitch 출처 = Step 표 열). Map 형상 다름 경고만 화면에 보이지 않는다 |

서버 계약 영향: 요청에 Defect 영역 방식을 넣는다 - `AreaMode`("SIZE_TOL" / "SQUARE") + `ToleranceUm` / `SquareUm`. 거리(Gap) 판정에서 SQUARE 는 Defect 사각형 = 중심 ± 한 변 / 2, 걸림 = Gap ≤ 0(허용오차 없음). 허용오차 표(5.5절)는 SIZE_TOL 에서만 의미가 있어 SQUARE 면 탭을 비운다.

색 기준(6번) 설명: Wafer Map Die 색을 무엇으로 칠할지 고르는 콤보.

| 색 기준 | Die 색 / 숫자 | 언제 쓰나 |
| --- | --- | --- |
| Chain Defect 수 (기본) | Die 안 Chain 영역 Defect 수 구간 색 (예전 동작) | KLARF Defect 가 Chain 위에 얼마나 있나 |
| E-Test Fail Chain 수 | Die 의 E-Test Fail Chain 수 구간 색, E-Test 판정 없는 Die 는 회색 | 전기적으로 Fail 인 Die 분포 |
| 일치 Chain 수 | Defect 가 걸렸고 E-Test 도 Fail 인 Chain 수 | Defect 가 Fail 원인 후보인 Die |
| 선택 Chain 4분류 | Chain 탭 / E-Test Fail 탭에서 고른 Chain 하나의 일치 / Fail만 / Defect만 / 정상 / 비교 불가 색 | Chain 하나 기준 역추적 |

- 색 기준과 별도로, E-Test Fail Die 모서리 표식은 E-Test 를 고르면 항상 그린다. 색 구간 표(오른쪽 위)는 색 기준에 따라 행이 바뀐다.
- 콤보를 없애면 "Chain Defect 수" 고정 + Fail 모서리 표식만 남고, 나머지 3개(E-Test 를 Map 색으로 보는 방법)는 쓸 수 없다. E-Test Fail 탭 / Chain 탭 표는 그대로 남는다.

## 20. Scribe 위치 / GDS Chip 크기 / Pitch 비교 (2026-10-16 추가, 결정 B23 ~ B25 대기)

사용자 질문 / 지시 (2026-10-16):

> GDS는 Die 기반이고 Klarf는 Die + scribe line 포함으로 알고 있다 우리 시스템은 이부분 고려가 된건가?
> klarf 의 표준이나 일반적으로 scribe line은 중점으로 기준으로 겹쳐지나 아니면 상단, 우측으로 scribe line을 만드나? 이부분을 정해야 좌표 분석시에 scribe line에 대한 조정이 가능 할 것 같다.

-> Device 별 Scribe 위치, GDS Chip 크기 저장, Pitch 비교 점검을 결정 항목(B23 ~ B25)으로 두고 고객 확인 질문(20.6)을 남긴다.

### 20.1 지금 상태 (확인한 사실)

| 항목 | 지금 | 근거 |
| --- | --- | --- |
| 좌표 식 | `X = XREL − PitchX / 2 + 보정 X` (Y 같음). Chip 이 KLARF Die 칸 한가운데(Scribe 양쪽 균등)이고 GDS 원점 = Chip 중심이라는 가정 | `GdsKlarfMatchEngine.cs` 69 ~ 91행 |
| Scribe 정의 | 없음. KLARF 에 Scribe 항목이 없다(`DiePitch` 만). DMS `TQD_SETUP.STREET_X / Y` 는 Glass Q-Panel 간격(mm)용 컬럼이고 55행 모두 0. File Map Setup 은 Pitch 만 받는다(Die / Scribe 크기 입력 없음, 2026-10-06 KLARF Map Setup 설계). `TQP_MAPDEF.CHIP_SIZE` 는 Pitch 값이다 | 로컬 DB 조회 / `TQP_MAPDEF.xml`(`CHIP_SIZE_X AS DIE_PITCH_X`) |
| 보정 X / Y | 조건 줄 입력, 저장 안 함(분석마다 손으로 넣음) | `frmGdsKlarfAnalysis` `numOffsetX / Y` |
| GDS Chip 크기 | 저장 안 함(`TQP_GDS_MAP` 에 크기 컬럼 없음). 저장 도형 범위는 저장 Layer 에 따라 다르다: 전체 Layer ±11,880 / ±14,992.5, Layer 일부만 저장한 U9 / OMM_Hybrid 는 X −11,810 ~ +11,793.6 | `TQP_GDS_PLACED_EL` MIN / MAX 집계 |
| GDS 와 Pitch 비교 | 없음. 아래 실측 값은 설계 때 사람이 잰 값 | - |
| Chip 밖 Defect | 표시 없음(Chain 밖으로만 센다). 10.2 `DEFECT` 에 "Chip 밖" 이 있으나 어느 외곽으로 판단하는지 정해지지 않았다 | `GdsKlarfAnalysis.cs` |

실측 (OMM, µm):

| 항목 | X | Y |
| --- | --- | --- |
| KLARF Die Pitch (Die + Scribe) | 23,832 | 30,060 (OMM_TCB 30,058) |
| GDS Chip 외곽 (전체 Layer, 원점 = 중심) | 23,760 | 29,985 |
| 고객 Die (Scribe 제외, 고객 자료 Scribe 150) | 23,680 | 29,905 |
| Pitch − GDS, 한쪽 | 36 | 37.5 (OMM_TCB 36.5) |
| GDS − 고객 Die, 한쪽 | 40 | 40 |

### 20.2 Scribe 위치는 KLARF 규격이 정하지 않는다

- KLARF 는 Die 칸 크기(`DiePitch`), 칸 기준점(`DieOrigin`, 샘플 4개 모두 `0 0` = 칸 왼쪽 아래), 기준점에서 Defect 까지 거리(`XREL / YREL`)만 갖는다. 칸 왼쪽 아래가 실제 Wafer 에서 어디인지는 검사 장비 Recipe 에서 Die 를 정렬할 때 잡은 기준점에 따른다.
- 현장에서 쓰는 두 가지:

| Recipe 기준점 | Die 칸 안 Chip 위치 | Scribe |
| --- | --- | --- |
| Scribe 교차점 중앙 | 가운데 | 양쪽 균등 (한쪽 = Scribe / 2) |
| Chip 왼쪽 아래 모서리 | 왼쪽 아래에 붙음 | 오른쪽과 위에 몰림 |
| Chip 왼쪽 위 모서리 (KLA 23XX 매뉴얼 절차와 같은 모양, 동부하이텍 KLA 2139 KLARF 주요 제품 실측, 20.8) | 왼쪽 위에 붙음 | 오른쪽과 아래에 몰림 |

- 샘플로는 정하지 못했다 (STEP 541 OMM_C4Bump_TCB, 고유 Defect 1,122):
  - 가장자리 여백: 왼쪽 55 / 오른쪽 207, 아래 156 / 위 258 µm. 오른쪽과 위가 더 비어 있지만 가장자리 100 µm 안 Defect 가 3개뿐이라 근거가 약하다.
  - GDS 도형 맞춤 이동량 시험(Layer 1 ~ 8, X / Y −152 ~ +152 µm, 4 µm 간격, FS / BS): Layer 마다 최고점이 제각각(dx −108 ~ +128, dy −152 ~ +68)이고 우연 수준(z ≤ 5)이다. 이 Step 의 Defect 는 GDS 도형과 위치 상관이 거의 없다.
- 그래서 고객 확인(20.6)으로 정하고, 시스템은 Device 별 설정값으로 둔다.

### 20.3 B23 Scribe 위치 (Device 별 기준정보)

| 선택 | 뜻 | Scribe 보정 X 기본값 | Scribe 보정 Y 기본값 |
| --- | --- | --- | --- |
| 가운데 (기본) | Chip 이 칸 가운데 | Chip 보기 중심 X | Chip 중심 Y |
| 오른쪽 위 | Chip 이 칸 왼쪽 아래에 붙고 Scribe 가 오른쪽과 위 | PitchX / 2 + Chip 보기 MinX | PitchY / 2 + Chip MinY |
| 직접 입력 | 고객 확인 값 | 입력값 | 입력값 |

- Chip 보기 외곽 = B24 Chip 외곽을 그 Step 의 GDS 반전(18절, Chain 면 ≠ 검사 면)에 맞춘 값이다. 반전이면 보기 MinX = −CHIP_MAX_X.
  - 원점이 Chip 중심인 GDS(OMM)는 가운데 = (0, 0), 오른쪽 위 = (+36, +37.5), OMM_TCB Pitch 면 (+36, +36.5).
  - 고객 답이 "기준점 = 고객 Die(Scribe 제외) 모서리" 이면 GDS 외곽이 Die 보다 한쪽 40 µm 커서 (+76, +77.5) 가 된다. 이 경우 직접 입력.
  - 원점이 Chip 왼쪽 아래인 GDS 도 Chip 외곽이 있으면 같은 식으로 맞는다(지금 식은 원점 = 중심일 때만 맞음).
- 식: `X = XREL − PitchX / 2 + Scribe 보정 X + 조건 줄 보정 X` (Y 같음). 조건 줄 보정 X / Y 는 모든 Step 공통 추가 보정(기본 0)으로 남긴다. 보정은 지금처럼 KLARF 보기 좌표(반전 전)에서 더하므로 "오른쪽 위" 는 검사 이미지 기준이다.
- 저장: `TQP_GDS_MAP` + `SCRIBE_POS`(CENTER / UPPER_RIGHT / MANUAL, NULL = CENTER) / `SCRIBE_OFFSET_X / Y`(µm, MANUAL 때). 입력: GDS Map 조회 화면 [Map ▾] -> "Chip / Scribe 설정" 작은 창(호스트 모드, Chip 크기는 읽기 전용으로 함께 보임). Map 복사 때 같이 복사한다.
- Step 별: [준비] 탭 KLARF Step 표에 "Scribe" 열(가운데 / 오른쪽 위 / 직접), 기본 = Device 값. Recipe 가 Step 마다 다르면 여기서 바꾼다(검사 면 열과 같은 방식, 저장 안 함).
- 결과: `ANL_INFO` 에 Step 별 SCRIBE_POS 와 적용한 보정 X / Y 를 남긴다.
- 다른 안: 지금처럼 조건 줄 보정 X / Y 만(분석마다 손 입력, 기록 없음).
- 참고 (2026-10-16, 20.8): 동부하이텍 KLA 2139 검사 KLARF 는 주요 제품(MA65 / WO02_4X2 / SW73)에서 Scribe 가 오른쪽과 아래에 있었다. 제품(Recipe)마다 다를 수 있다. 이 경우 Scribe 보정 X = PitchX / 2 + Chip 보기 MinX, Scribe 보정 Y = Chip MaxY − PitchY / 2 다. OMM 에 넣으면 (+36, −37.5), 고객 Die 기준이면 (+76, −77.5). 위 선택에 "오른쪽 아래" 를 더할지는 고객 답(20.6) 뒤 B23 결정 때 정한다.

### 20.4 B24 GDS Chip 크기 저장

- GDS Map 저장 때 Flatten 결과 전체(저장 Layer 체크와 무관, TEXT 제외)의 외곽을 `TQP_GDS_MAP` + `CHIP_MIN_X / CHIP_MIN_Y / CHIP_MAX_X / CHIP_MAX_Y`(µm, 저장 기준 FS 좌표)에 남긴다.
  - 이유: Layer 일부만 저장하면 `PLACED_EL` 범위로 Chip 크기를 알 수 없다(20.1).
  - 값은 화면이 이미 가진 배치 도형 `WorldBounds` 합으로 구한다. 읽기 단계 `GdsStructure.Bounds` 는 SREF 를 원점 한 점으로만 넣어 계층 GDS 에서 틀리므로 쓰지 않는다.
- 저장 경로: 기존 함수는 바꾸지 않고 BSL `GdsMapImport` 에 Chip 외곽 기록 함수를 더해 저장 완료 직후 부른다. Map 복사(`CopyMap`) 쿼리에 컬럼을 더한다.
- 기존 Map 9개(모두 `OMM all layer2.gds` TOP): 보정 SQL 1회(−11,880 / −14,992.5 / 11,880 / 14,992.5). 다른 파일 Map 이 생기면 다시 저장하거나 같은 방식으로 채운다. 값이 없으면 B25 점검에서 "Chip 크기 없음".
- DDL: `TQP_GDS_MAP` 컬럼 7개(B23 3 + B24 4), 모두 NULL 허용. 10.4 "DDL 없음" 의 예외다.
- 다른 안: 분석 때 `PLACED_EL` 범위로 계산(저장 Layer 에 따라 달라지고 Chip 밖 판단 불가).

### 20.5 B25 Pitch 비교 점검 + Chip 밖 Defect

16.5 점검 표에 더한다(`GetWaferReady`, Step 마다, Pitch 는 B6 출처):

| 점검 | 수준 | 조건 | 결과 |
| --- | --- | --- | --- |
| Chip / Pitch | 정보 | 항상 | "Pitch 23,832 × 30,060 / Chip 23,760 × 29,985 / Scribe 추정 72 × 75 (한쪽 36 × 37.5) / Scribe 위치 가운데" |
| Chip 이 Pitch 보다 큼 | 오류 | Chip 폭 또는 높이 > Pitch + 1 µm | 다른 제품 GDS 또는 단위 오류. [분석] 불가 |
| Scribe 추정 큼 | 주의 | Pitch − Chip > 1,000 µm (X 또는 Y, 설정값) | GDS 가 Chip 일부만 담았거나 Pitch 를 잘못 고친 경우 |
| Chip 크기 없음 | 정보 | `CHIP_*` NULL | 비교 생략. 오른쪽 위 보정 계산 불가(가운데 = GDS 원점 기준, 지금과 같음) |

- Chip 밖 Defect: 보정까지 적용한 Defect 중심(보기 좌표)이 Chip 보기 외곽 밖이면 `DEFECT.OUT_CHIP = Y`. Chain 판정은 그대로 하고, 수를 `ANL_INFO.OUT_CHIP_COUNT` 에 남긴다. Scribe 에 떨어진 Defect 이거나 Scribe 위치 / Pitch 가 틀렸다는 신호다.
  - 현재 샘플은 가운데 가정에서 0개다(STEP 541 고유 1,122 / 528 15 / 529 109, XREL / YREL 범위로 계산).
- 다른 안: 비교 없음(지금).

### 20.6 고객 확인 질문

1. Step(검사 Recipe) 별로 Die 기준점(KLARF `DieOrigin 0 0`)을 Scribe 교차점 중앙에 잡았는지, Chip 왼쪽 아래 모서리에 잡았는지 (C4 Bump / TBDB / RDL / uPAD 등).
2. Scribe 폭 X / Y(150 / 150 µm 맞는지)와 Chip 이 Scribe 사이 가운데에 있는지.
3. GDS 외곽(23,760 × 29,985)이 Die(Scribe 제외, 23,680 × 29,905)보다 한쪽 40 µm 큰 부분(Seal ring 등)이 무엇인지, "Chip 밖" 기준을 GDS 외곽으로 써도 되는지.
4. 위치를 아는 Defect 1 ~ 2개(리뷰 이미지에서 Pad / 배선 위로 보이는 Defect 의 Wafer / Step / DEFECTID). 보정 값 확인용.
5. 검사 Recipe 의 Die 원점(모서리) 위치 (2026-10-16 추가, 20.8): Recipe 에서 Die 크기를 정할 때 찍은 모서리가 Chip 의 어느 모서리인지(왼쪽 위 / 왼쪽 아래 / 그 밖), Scribe Line 영역을 Recipe 에 정의했는지(폭 / 위치), 정한 뒤 Die 원점을 옮겼는지. 1번을 검사 장비 Recipe 화면 기준으로 묻는 질문이다.

### 20.7 영향 / 순서

- 서버(CLAUDE-127 진행 중 범위): `GetWaferReady` 점검 행, `Analyze` 의 Step 별 Scribe 보정 / `OUT_CHIP`. 요청 계약에 Step 별 `ScribePos` / 직접 입력 X / Y 를 더한다.
- 화면(1차 시안 확정분에 추가): [준비] 탭 Step 표 "Scribe" 열 1개, GDS 줄에 Chip 크기 / Scribe 위치 문구, GDS Map 조회 [Map ▾] 설정 창.
- Map 저장 / 복사: GdsMap `GdsMapForm_DbSave`, BSL `GdsMapImport`, 쿼리 `TQP_GDS_MAP.xml`.
- 순서: 결정 -> DDL -> Map 저장 / 복사 + 기존 Map 보정 SQL -> Ready 점검 -> Analyze 보정 / OUT_CHIP -> 화면. 고객 답이 오기 전 기본은 "가운데"(= 지금 동작)라 결과가 바뀌지 않는다.
- 관련: 4.6-2 / 4.6-3(중첩 SREF 변환 순서 / SREF 배율)은 2026-10-15 CLAUDE-120 에서 확인했다. 샘플 GDS 는 SREF 0 / AREF 0 이라 저장 Map 영향이 없고, 코드 수정(행렬 Prepend / PATH 폭 배율 / 반복 배치 / 읽기 중복)은 완료했다(변경이력 2026-10-15).
- 결정 시점 (2026-10-16 사용자): **고객 답(20.6)을 받은 뒤 B23 ~ B25 를 결정한다.** 그때까지 대기이며 서버 / 화면에 넣지 않는다.

### 20.8 참고: 동부하이텍 자료의 Scribe / Pitch 계산 (2026-10-16 조사, CLAUDE-136)

B23 판단 근거로 `D:\1_프로젝트 문서\12.동부하이텍` 자료를 찾았다(읽기만). **B23 ~ B25 결정은 그대로 대기다.**

| 자료 | 내용 |
| --- | --- |
| KLA 23XX 사용자 매뉴얼 (`!문서/I01.분석단계(ANA)/기존 시스템 메뉴얼/KLA2360 Manual/23XX_UserManual.pdf` 3-54 ~ 3-59 / 3-66 / 9-63쪽) | Die 크기: 이웃 Die 의 같은 모양 모서리 3곳을 찍어 구하며 화면에 "die size (pitch)" 로 나온다(= Die + Scribe). 고른 모서리가 Die 원점이다. 매뉴얼 절차는 왼쪽 위 모서리이고, 모서리가 아니어도 되며 Adjust Die Corner / Adjust Die Origin 으로 옮길 수 있다. Scribe Lines: 오른쪽 위 Die 의 왼쪽 아래 모서리와 왼쪽 아래 Die 의 오른쪽 위 모서리를 찍어 폭과 위치를 정한다. Scribe 영역은 검사하지 않는다. 정의하지 않으면 경고만 나온다 |
| DMS-ESDA 6.3.2 매뉴얼 (`!문서/I01.분석단계(ANA)/시스템구성/DMS-ESDA 6.3.2 Release/esda63-vol2.pdf` 100 ~ 107쪽) | `device.insp` = Street Width / Height + Die X / Y Period(µm). 그림: X period = die width + street width. Street 값은 DMS 가 더 이상 쓰지 않아 0 으로 둬도 된다. Leica Review 용 `devicename.drv` 도 STREET_WIDTH / HEIGHT 를 0 으로 둘 수 있다 |
| 기존 Klarity DB 참고 Query (`!문서/I01.분석단계(ANA)/DBH_통합_DMS_FAB2_WAFERMAP_20190409.xlsx`, `!문서/P01.의사소통(COM)/회의내용 및 정리_20190403.txt`) | `INSP_RECIPE.DIE_SIZE_X` 에 DiePitch 를 저장. `XREL = (WAFER_X − DIE_SIZE_X × INDEX_X) / 1000`, KLARF Index = `INDEX_X − ORIGIN_INDEX_X`. Scribe 항 없음 |
| ESDA 이관 코드 (`DMS/10_Migration/MigrationTools/Main.cs` 243 ~ 282 / 425 ~ 426행) | Pitch = `X_period`, `XREL = Xmicron − Pitch × (DIE_X − 1)`. Street 폭은 `TQD_SETUP.STREET_X / Y` 에 저장만 한다(우리 DB 55행은 모두 0, 20.1) |
| Rudolph NSX map (`!문서/I01.분석단계(ANA)/FOI/9056430/AK9056430-01.map`) | Pitch 1.78 × 2.33 mm 와 Die 1.7 × 2.25 mm 를 따로 저장(차이 = Street 80 µm) |

로컬 DB 의 동부하이텍 제품 KLARF (XREL / YREL 양 끝 빈 폭 = 0.1 / 99.9 백분위, µm). 검사 장비는 모두 `INSPECTION_EQ = IPS201`(TL09B 1 Step 만 IPS204)이다. 동부하이텍 KLARF 원본 머리글 `InspectionStationID "KLA2139" "IPS201"` 로 IPS201 = KLA 2139 를 확인했다(설비 리스트상 IPS204 = KLA 2360).

| 제품 | Pitch | Defect | X 작은 쪽 | X 큰 쪽 | Y 작은 쪽 | Y 큰 쪽 | 끝 50 µm 안 Defect (X 작은 / X 큰 / Y 작은 / Y 큰) |
| --- | --- | --- | --- | --- | --- | --- | --- |
| MA65 | 5,043.9 × 4,253.3 | 110,643 | 35 | 106 | 115 | 27 | 519 / 0 / 0 / 1,259 |
| WO02_4X2 | 1,487.4 × 933.0 | 6,374 | 21 | 81 | 80 | 16 | 441 / 0 / 0 / 573 |
| SW73 | 2,939.6 × 2,973.4 | 5,764 | 36 | 75 | 213 | 65 | 180 / 0 / 0 / 229 |
| MA45 | 3,387.5 × 3,384.1 | 7,688 | 107 | 243 | 165 | 253 | 0 / 0 / 0 / 0 |
| SI57 | 3,410.9 × 3,017.0 | 1,366 | 168 | 220 | 249 | 144 | - |
| TL09B | 16,580.1 × 560.0 | 1,446 | 107 | 35 | 111 | 32 | - |
| OMM_TCB (우리 고객) | 23,832 × 30,058 | 2,244 | 65 | 210 | 165 | 281 | 0 / 0 / 0 / 0 |

정리:

- 동부하이텍 기존 시스템은 Scribe 를 Pitch 칸에 넣은 채 Pitch 와 Index 만으로 Die 안 좌표를 계산했다. Scribe 폭으로 좌표를 옮기는 계산은 없고 Street 폭은 저장만 했다.
- Scribe 가 칸의 어디에 있는지는 KLARF 규격이 아니라 검사 Recipe 에서 고른 Die 원점(모서리)으로 정해진다(20.2 와 같음). 회사 규칙으로 정한 문서는 찾지 못했다.
- 주요 제품 3개(MA65 / WO02_4X2 / SW73)는 XREL 큰 쪽과 YREL 작은 쪽에만 Defect 없는 띠(약 75 ~ 115 µm)가 있다. 검사하지 않는 Scribe 가 KLARF 좌표로 오른쪽 / 아래에 있다고 본다.
- 한계:
  - 데이터 장비는 KLA 2139 이고 매뉴얼은 KLA 23XX 용이다. "왼쪽 위 모서리" 절차와 모양이 같을 뿐 같은 장비의 근거는 아니다.
  - MA45 / SI57 은 네 변이 모두 비어 판정할 수 없고, TL09B 는 왼쪽과 아래가 더 비어 있다. 제품(Recipe)마다 다르다.
  - 빈 띠가 Scribe 가 아니라 검사 영역(Care Area) 여백일 수도 있다. 띠 폭이 흔한 Scribe 폭(NSX map 80 µm 등)과 비슷하다는 정도다.
  - "오른쪽 / 아래" 는 KLARF 좌표(XREL 오른쪽 +, YREL 위 +) 기준이다. Notch 방향이나 검사 면에 따라 실제 Wafer 에서 보는 방향과 다를 수 있다.
- OMM_TCB 는 네 변이 모두 비어 있어 같은 방법으로 판정할 수 없다. 장비 / Recipe 가 동부하이텍과 같다는 근거도 없어 고객 확인(20.6 의 1번, 5번)이 필요하다.

## 21. 서버 구현 (2026-10-16, CLAUDE-127)

1차 화면 확정 뒤 12절 순서(0 ~ 5)대로 서버를 만들고 화면에 연결했다. 운영 폴더 배포는 하지 않았다(사용자 VS 빌드 / 배포).

### 21.1 구현 위치

| 단계 | 파일 | 내용 |
| --- | --- | --- |
| 0 | 쿼리 `TQP_GDS_CHAIN_MEMBER.SELECT_CHAIN_MEMBER_SHAPE_ALL`, `BSL/Probes/GdsKlarfShapeStatsProbe.ps1` | Member 모양 통계 (4.7) |
| S1 | BSL 신규 `GdsKlarfShapeGeometry.cs` / `GdsKlarfGeometryCache.cs`, `GdsKlarfMatchEngine.MatchShapes` | 모양 Gap (4.3) / 격자 색인 + Cache (4.5) / 탐색 거리 안 Gap 전부 (4.2, 5.5) |
| S2 | Interface 신규 `GdsKlarfAnalysisRequest.cs` + `Analyze` / `AnalyzeDie`, RO, BSL 신규 `GdsKlarfAnalysis_Analyze.cs`(partial), 쿼리 `TQD_DEFECT.SELECT_DEFECT_FOR_GDS` | 결과 표 (10.2), 여러 Step (16.7), Chain 면 / 검사 면 (18), Defect 영역 (19) |
| S3 | Interface 신규 `Coordinate/WaferKeyRule.cs` + `GetWaferReady`, BSL 신규 `GdsKlarfReady.cs`, 쿼리 `TQD_INSP_INFO.SELECT_KLARF_STEP_BY_WAFER` / `TQP_GDS_SRC_FILE.SELECT_ET_FILE_BY_WAFER` | Ready (16.2 ~ 16.5) |
| S4 | BSL 신규 `GdsKlarfEtLink.cs`, 쿼리 `TQP_GDS_SRC_FILE.SELECT_ET_FILE_FOR_GDS` / `TQP_GDS_ET_VALUE.SELECT_ET_DIE_CHAIN_JUDGE` · `SELECT_ET_VALUE_OF_DIE` | 이름 연결 (6.5) / Die 판정 (6.2, 6.4) / 4분류 · Fail률 (7) / 역추적 (15.3) |
| S5 | 화면 `frmGdsKlarfAnalysis` | [준비] 탭 = `GetWaferReady`(실패하면 고른 Step 만 + 사유 점검 행), [분석] = `Analyze`, 상세 화면 = `AnalyzeDie`(그 Die 만 다시 계산), 임시 안내 문구 제거 |
| S6 | BSL `KlarfUpload` | Step COMMENT 에 `RAW_DIE_PITCH=x,y`. 분석 / Ready 가 먼저 쓰고 없으면 Map Setup (5.2, B6) |
| S7 | `GdsKlarfShapeProbe.ps1` D4 | 대량 합성 측정 (9.3) |

### 21.2 구현하며 정한 점 (설계 표와 다른 곳 포함)

- **DEFECT 표**: "Chain 영역 + Chip 밖" 대신 Defect 20,000개 이하면 모두, 넘으면 탐색 거리(50 µm) 안 Defect 만 보낸다(`ANL_INFO.DEFECT_ROWS` = ALL / NEAR). Chip 범위가 아직 저장되지 않아(20절 B24) Chip 밖을 가를 수 없고, 화면 Defect 탭 [Chain 밖] 범위를 살리기 위해서다. B24 / B25 결정 뒤 `OUT_CHIP` 을 더한다.
- **TOL_CURVE**: 0 / 2 / 5 / 10 / 20 / 50 + 지금 허용오차(목록에 없으면) 행. 중심 사각형 방식이면 빈 표(19절).
- **ANL_INFO.TOLERANCE_UM**: 중심 사각형이면 한 변 / 2, `USE_DEFECT_SIZE` = N (화면 B 가 정사각형을 이 값 x 2 로 그린다). 실제 걸림 기준은 `HIT_GAP_UM`(중심 사각형 0).
- **ET_LINK**: `GDS_CHAIN_NAME`(등록 원문) 열을 더했다. 화면 E-Test 연결 탭은 원문을 보인다. 사용 N Parameter 는 뺀다.
- **측정값 / Limit 문구**: DB 가 BINARY_DOUBLE 이라 SQL `TO_CHAR` 를 쓰면 `1.5E+002` 로 나온다. 쿼리는 숫자 + 값 상태를 돌려주고 BSL 이 "150" / '-' 측정 = "-" 로 바꾼다.
- **DIE 표**: E-Test 에만 있는 Die 도 행으로 보낸다(IN_PLAN N, 4분류 NA). 4분류의 "KLARF 검사 Die" = 어느 Step 이든 검사 Die 목록에 있거나 Defect 가 있는 Die.
- **DIE_CHAIN (Wafer)**: 걸린 Defect 가 있거나 E-Test 상태가 F / N 인 칸. Defect 0 + P 칸은 보내지 않는다(화면이 OK 로 본다). Fail 칸의 `MIN_GAP` = 탐색 거리 안 가장 가까운 그 Die Defect.
- **AnalyzeDie**: DIE_CHAIN / DEFECT_CHAIN 은 탐색 거리 안 Chain 전부 + E-Test 연결 Chain, DEFECT_HIT 은 Member 단위(Gap ≤ 걸림 기준).
- **Cache Stamp**: `SELECT_MAP_INFO` 의 CREATE_TIME / IMPORT_JOB_SEQ / VIEW_SIDE + READY Chain 별 UPDATE_TIME / MEMBER_COUNT. 화면 Chain 거르기는 Cache 와 상관없이 판정 때 적용.
- **PATH 꺾임**: Miter 길이 > 2W 면 바깥 모서리 2개를 잇는 삼각형(4.3 표).
- **Wafer Key 후보 쿼리**: KLARF Slot 은 앞 0 을 뺀 문자열(SLOT_ID '2'), E-Test 는 SLOT_NO 숫자. 검사 시각 기간 ± 180일.
- **결과 전송**: DataSet `RemotingFormat = Binary` (STEP 541 결과 417 KB, Xml 885 KB).
- 서버와 화면은 함께 배포해야 한다. 이전 서버에 새 화면을 붙이면 [준비] 탭에 "서버 준비 목록 조회 실패" 점검 행이 나오고 [분석] 은 오류로 끝난다.

### 21.3 검증 (TEMP 빌드, 로컬 DB)

| 검사 | 결과 | 내용 |
| --- | --- | --- |
| `BSL/Probes/GdsKlarfShapeProbe.ps1` | 33/33 | 손 계산 거리 11 / 엔진 무작위 800 Member x 600 Defect 전수 비교 / Cache(LRU, 동시 호출 1회 읽기) / 실제 DB / 대량 |
| `BSL/Probes/GdsKlarfAnalyzeProbe.ps1` | 14/14 | 결과 계약 / 면 규칙 = 예전 보기 기준 / 엔진 직접 판정과 같은 수 / 표 합계 / 허용오차 표 / 중심 사각형 / 3 Step 합치기(528 + 529 FS + 541) / Die 상세 / 거르기 / 큰 Defect / Cache / Binary / 오류 / 원본 Pitch 기록 · 읽기 |
| `BSL/Probes/GdsKlarfEtProbe.ps1` | 13/13 | Wafer Key 샘플 2쌍 / Ready / 시험 E-Test(PROBE / OMM_Hybrid, Lot RH-HS2615 Slot 2, 끝나면 삭제)로 이름 연결 4종 / Die 판정 / 일치 · Fail만 · 비교 불가 · Defect만 / Fail률 / E-Test 값 |
| `ENGUI/Verification/Verify-GdsKlarfAnalysis.ps1` | 42/42 | 화면 A: 서버 Ready / 서버 Analyze 와 같은 수 / 상태줄 면 문구 / 상세 데이터 = AnalyzeDie, 화면 B: AnalyzeDie 결과로 도면 / Defect / 반전 일치 |

대량 합성 (9.3, OMM_Hybrid Member 66,489, 합성 Defect 절반은 Chip 범위 무작위 / 절반은 Member 꼭짓점 위, 크기 0 ~ 20 µm):

| Defect 수 | 판정 시간 (허용오차 5 / 50 µm) | 걸린 Defect | 탐색 거리 안 Member 쌍 | 결과 메모리 |
| --- | --- | --- | --- | --- |
| 10,000 | 0.44 / 0.41 초 | 6,862 / 7,332 | 46,761 | 약 4 MB |
| 100,000 | 4.3 / 4.2 초 | 68,356 / 73,405 | 467,013 | 약 33 MB |
| 300,000 (상한) | 12.9 / 12.9 초 | 205,142 / 220,235 | 1,404,366 | 약 99 MB |

- Defect 1개당 약 43 µs. 실제 Wafer(STEP 528 + 529 + 541, Defect 1,246)는 판정 25 ms / 전체 0.3초(GDS Cache 있을 때), 첫 호출은 GDS 읽기 1.3초가 더해진다.

### 21.4 STEP 528 결과 메모 (면 / Pitch)

| Chain 면 / 검사 면 | 반전 | 모양 판정 Chain 영역 (Bounds 판정) | 허용오차 표 0 / 2 / 5 / 10 / 20 / 50 µm |
| --- | --- | --- | --- |
| FS / BS (기본) | 예 | 1 / 15 (1) | 1 / 1 / 1 / 1 / 1 / 1 |
| BS / BS | 아니요 | 8 / 15 (9) | 6 / 8 / 8 / 8 / 9 / 9 |

- 반전 없이 Gap 0 Defect 가 6개로 많다. 어느 면이 맞는지는 엔지니어 판단 / 고객 확인 대상이다(18절).
- STEP 528 은 원본 Pitch 기록(S6) 전에 올린 Step 이라 Pitch 출처가 Map Setup 이다. 다시 올리면 KLARF 원본 Pitch(23,832 x 30,060)로 분석한다.

### 21.5 남은 것

- 배포(사용자): Interface / DSL / BSL / RO / ENGUI DLL + 쿼리 XML 5개(`TQP_GDS_CHAIN_MEMBER` / `TQD_DEFECT` / `TQD_INSP_INFO` / `TQP_GDS_SRC_FILE` / `TQP_GDS_ET_VALUE`). 화면 B(GdsMap EXE)는 10-16 시안 변경분이 함께 나간다. 운영 `101_ServerBin` 의 GdsKlarfAnalysis wellknown 등록 확인.
- 20절 B23 ~ B25 결정 뒤 Scribe 보정 / Chip 밖(OUT_CHIP) / Pitch 비교 점검.
- 시안 이미지 도구(`Preview-GdsKlarfLayout.ps1`)는 표본을 아직 AnalyzeWafer 로 만든다(레이아웃 확인용, 판정 결과 아님).
- 사용자 DMS 화면 확인.

## 22. KLARF Step 목록 규칙 (2026-10-16 사용자 결정, B26 / B27 확정)

사용자 결정 (2026-10-16):

> 1. Klarf 에서 같은 파일 중복 처리시 기존거는 삭제 하고 엎어 친다. Lot / Slot / Step ID / 검사 시각이 같은 기록을 굳이 중복처리할 필요는 없다.
> 2. 검사 시각이 다른 경우는 아마 Test 순서관련 컬럼이 있을텐데 이부분 찾아서 순서에 맞게 처리하면 될거다.

### 22.1 같은 파일 다시 올림: 중복 처리 안 함 (결정)

- KLARF 업로드는 같은 파일을 다시 올리면 기존 Step 을 지우고 다시 쓴다. 같은 파일 키 = Product + Lot + Wafer + Step ID + 검사 시각(`KlarfUpload.FindExistingStep`, `KlarfUpload.cs` 255 ~ 268행).
- 그래서 [준비] 탭 KLARF 표에서 Lot / Slot / Step ID / 검사 시각이 같은 기록을 따로 중복 처리하지 않는다.
- 남는 경우: 같은 파일을 **Product 만 다르게** 올리면 키가 달라 둘 다 남는다. 예: `1107236219.SINF` = STEP 541(Product OMM_TCB) / 563(OMMTCB). Wafer Key 는 Product 를 보지 않으므로 둘 다 Ready 에 나오고, 함께 체크하면 같은 Defect 를 두 번 센다(CLAUDE-132 확인: DEFECT 2,244 = 1,122 × 2).
- **B26 (2026-10-16 확정, 권장안)**: Ready KLARF 수집을 고른 Step 과 같은 Product 로 한정한다(`SELECT_KLARF_STEP_BY_WAFER` 에 PRODUCT 조건 1줄). GDS Device 도 Product 를 따르므로(19절) 다른 Product 의 Step 은 다른 도면 기준이다. E-Test 는 그대로(Device 가 달라도 Wafer Key 로 찾음, 16.3).

### 22.2 검사 시각이 다른 같은 Step (재검사): TEST_ORDER 순서 (결정)

- 순서 컬럼: `TQD_STEP.TEST_ORDER` (DMS 기존 규칙).
  - 같은 WAFER_SEQ + STEP_ID 에 새 KLARF 가 오면 새 행을 0 으로 넣고 같은 Wafer / Step 의 모든 행을 +1 한다(`TQD_STEP.xml` `UPDATE_STEP_TEST_ORDER`, KLARF 업로드 Step 09 / DataService 같음).
  - 그래서 **가장 작은 값이 최신**이다. 지금 데이터는 최신 = 1(예: Wafer 123 Step `3039M4DM` 16:19 = 1, 16:00 = 2). `KlarfUpload.cs` 511행 주석 "최신이 0" 은 실제 값과 다르다.
  - WAFER_SEQ 는 Product 별이라 TEST_ORDER 도 Product 안에서만 매겨진다(22.1 B26 과 맞물림).
- **B27 (2026-10-16 확정, 권장안)**: Ready 쿼리가 TEST_ORDER 를 함께 돌려주고, 표는 Step ID -> TEST_ORDER 순으로 놓는다. 같은 Step ID 는 가장 작은 TEST_ORDER(최신)만 기본 체크하고, 이전 검사는 표에 남기되 체크를 끈다("추가" 열에 "이전 검사").
- 한 KLARF 파일 안의 검사 Test(KLARF `InspectionTest`, `TQD_INSP_INFO.TEST`, 예: Test 3 / 80)는 TEST_ORDER 와 다르다. 이 경우는 CLAUDE-132 에서 Step 당 한 줄로 묶었다(Defect 합, 검사 Die / TEST = Defect 가 가장 많은 Test).
- 결정 (2026-10-16 사용자): B26 / B27 권장안으로 확정. 코드는 CLAUDE-132 / 133 의 같은 파일 작업(`SELECT_KLARF_STEP_BY_WAFER` / `frmGdsKlarfAnalysis.cs`)이 끝난 뒤 반영한다.

### 22.3 구현 (2026-10-16, CLAUDE-134)

| 파일 | 내용 |
| --- | --- |
| `08_DBQuery/ORACLE/TQD_INSP_INFO.xml` | 신규 `SELECT_KLARF_STEP_BY_WAFER_PRODUCT` = 기존 `SELECT_KLARF_STEP_BY_WAFER`(CLAUDE-132 판, 그대로 둠) + `AND UPPER(TRIM(A.PRODUCT)) = UPPER(TRIM(:product))` + `S.TEST_ORDER`. 매개변수가 하나 늘어 새 ID 로 두었다: 기존 ID 를 쓰는 예전 서버 DLL 이 서비스 재시작 뒤에도 그대로 동작한다 |
| `BSL/GdsKlarfReady.cs` | `GetWaferReady` 가 새 쿼리에 고른 Step 의 Product 를 넘긴다(B26). 같은 파일 끝 신규 `GdsKlarfReadyStepRule`(B27): `Arrange` = Step ID 순 묶음, 묶음 안 최신 먼저(TEST_ORDER 작은 값 -> 검사 시각 늦은 것 -> STEP_SEQ 큰 것), Step ID 마다 기본 체크 1행(고른 Step 이 있으면 그것, 없으면 최신), 나머지 USE_FLAG N + ADDED_BY PREV(이전 검사) / NEXT(이후 검사). `ParseOrder` = TEST_ORDER 값 해석 |
| `ENGUI/DefectMapAnalysis/frmGdsKlarfAnalysis.cs` | `AddedText`: PREV -> "이전 검사", NEXT -> "이후 검사" ("추가" 열) |
| `BSL/Probes/GdsKlarfEtProbe.cs` | R5(같은 Product: 541 -> 541 한 줄, 563 -> 563 한 줄) / R6(재검사 정리 규칙, 합성 값) 추가 |

- 검증: `GdsKlarfEtProbe.ps1` -> `GDS_KLARF_ET_PROBE=PASS (15/15)`(Interface / DSL / BSL TEMP 빌드 경고 0), `Verify-GdsKlarfAnalysis.ps1` -> `GDS_KLARF_FORM_SMOKE=PASS (43/43)`(경고 기존 Control 12 / ENGUI 4 / GdsMap 1). 새 쿼리 조건은 DB 에서 직접 실행해 541 / 563 각 1행(Defect 1,122 / Die 76 / TEST_ORDER 1) 확인.
- 로컬 DB 에는 같은 Wafer + 같은 Step ID 재검사 Step 이 없어(POC 샘플) B27 은 합성 값(R6)으로만 확인했다. 실제 재검사 데이터가 생기면 [준비] 탭에서 다시 본다.
- 배포 안 함: BSL(101_ServerBin) / ENGUI(100_ClientBin) 반영 + SEMDMS Service / DACrux 재시작이 필요하다. 쿼리 XML 은 서비스 재시작 때 읽힌다.

## 23. 화면 표시 이름 (2026-10-16 사용자 지시, CLAUDE-135)

- 지시: "Die -> X, Y 컬럼 나눠서 / 전체 -> Defect 전체 / Chain 영역 -> Chain In / Chain 밖 -> Chain Out / 주요 Chain 필요 없음 / Chain Name을 넣어라. A, B, C ... 이거는 어디서도 쓰지 마라".
- 규칙:
  - Chain 은 화면 어디서나 Chain 이름(`CHAIN_NAME`)으로 보인다. Chain 코드(`CHAIN_CODE`, A / B / AA ..)는 서버 결과와 내부 정렬 / 대응에만 쓰고 화면에 보이지 않는다.
  - 서버 `DEFECT.CHAIN_CODES` 는 그대로 두고, 화면이 같은 결과의 `CHAIN` 표(코드 -> 이름)로 바꿔 보인다. 서버 재배포 없이 화면만 바뀐다.
  - 이름: Defect 전체 = Die 의 Defect 수(분류 / 큰 Defect 거르기 뒤) = Chain In + Chain Out. Chain In = Chain 에 걸린 Defect, Chain Out = 걸리지 않은 Defect.
- 화면 A(`frmGdsKlarfAnalysis`): [Die] 탭 열 = X / Y / Defect 전체 / Chain In / Chain Out / Chain 수 / ET 판정 / Fail Chain / 일치 Chain / 검사(주요 Chain 열 없음). [Chain] / [E-Test Fail] 탭 첫 Chain 열 = 이름(뒤쪽 이름 열 없음), [Defect] 탭 Chain = 이름 목록, [E-Test 연결] 연결 Chain = 이름, Chain 고르기 메뉴 = 이름 + Member 수, Defect 범위 콤보 = Chain In / Chain Out / Defect 전체, 허용오차 탭 = Chain In Defect, 상태줄 = "Defect 전체 n / Chain In n / Chain Out n / 걸린 Chain n".
- 화면 B(`GdsDieAnalysisForm`): Chain 목록 Chain 열 = 이름(이름 열 숨김), 역추적 = "Chain 이름", Defect 목록 Chain = 이름, Die 정보 / Defect 수 문구 = Defect 전체 / Chain In / Chain Out, 범위 콤보 = Chain In / Chain Out / Defect 전체.
