# F07 / AOI / E-Test / GDS Correlation

확인일: 2026-10-02 / 상태: 기존 요구 검토 문서와 선행 기능 기준 / 통합 실행 미확인

## 목적과 근거

AOI KLARF의 Defect 위치가 GDS Chain을 구성하는 Bump / RDL 영역과 관련되는지 확인하고 E-Test Fail Chain과 연결한다.

근거는 [9/30 문서 검토 종합](../../../문서/설계/01_계획_관리/2026-09-30_문서검토_종합.md)과 [Chain Setup 컨셉안](../../../문서/설계/02_GDS/2026-09-23_Chain_Setup_기능_컨셉안.md)이다. 이번 정리에서는 고객 원본 PDF / XLSX를 새로 분석하지 않았다. 확정 고객 요구로 인용할 때 원본과 최신 협의 내용을 다시 확인한다.

## 현재 구분

| 항목 | 현재 확인 범위 |
| --- | --- |
| GDS 도면 / Chain | Viewer와 메모리 기반 Chain 편집 코드 존재 |
| AOI 파일 확인 | KLARF 파일 조회 화면 코드 존재 |
| 영구 Map / Chain | 배치 기반과 스키마 존재 / 전체 왕복 저장 미완료 |
| AOI -> GDS 좌표 변환 | 원점 / 방향 / 미러 / 회전 / 단위 / 허용오차 계약 확인 필요 |
| E-Test -> Chain | 고객 ET 항목명과 Chain Code 연결 키 확인 필요 |
| 최종 판정 / 통합 화면 | 이번 조사 범위에서 통합 완료 근거 없음 |

## 이어갈 때 지켜야 할 기준

- 고객 검토 문서의 판정 사례 단위는 `(Die, Fail Chain)`이다. Die 전체 하나의 Yes/No로 단순화하지 않는다.
- GDS Layer와 Chain은 같은 개념이 아니다. 하나의 Defect와 여러 Chain의 관계가 생길 수 있다.
- 사각 영역 / Bounding Box 비교와 정밀 Polygon 교차는 서로 다르다. 사용할 판정 방식과 Tolerance를 고객 샘플로 확정한다.
- KLARF의 절대 좌표 / Die 상대 좌표 / Die Index를 구분한다. 화면에서 맞아 보인다는 이유로 미러 / 회전을 자동 적용하지 않는다.
- 비교 결과에는 어떤 Map Revision / Chain Version과 좌표 변환 기준을 사용했는지 남겨야 한다.

## 다음 단계

동일 Device / Wafer / Die의 KLARF, GDS, E-Test 샘플을 기준으로 좌표와 Chain 매핑 표부터 만든다. 확정되지 않은 부분은 추정 / 확인 필요로 표시한다. 그 뒤 한 개 Die / Chain 사례를 왕복 검증하고 범위를 넓힌다.


## 2026-10-02 사용자 결정

KLARF Defect와의 연관분석은 Chain 영역(현재 Chain Version의 Member 도형)만 대상으로 한다. 배경 도면 도형은 비교하지 않는다. 1차 후보는 Defect 좌표 +/- Tolerance와 Member 도형 Bounds 겹침. 상세는 [GDS Map DB 저장 설계 13.3절](../../../문서/설계/02_GDS/2026-10-02_GDS_Map_DB저장_설계.md).


## 2026-10-07 E-Test 결과 파일 설계 (CLAUDE-029)

샘플 `SampleFile/26_10_06 Sample File/CAI_OMM_ATV_A0_TSVET_..._RH-HS2615-02.xlsx` 분석 결과 헤더 형식은 `{Para}[_{조건}={값}]@{Chain}[:{Chain2}]` (Leakage_Serial 은 Chain 2개). 파일 Limit 은 쓰지 않고 별도 Limit 테이블 / 화면으로 판정한다 (Connectivity / Resistance 는 파일 Limit 이 N/A 라 Open 판정에 Limit 화면 필수). E-Test Chain 이름(최대 19자)과 GDS `CHAIN_CODE` 는 FK 없이 이름으로 대응. 상세와 결정 대기 항목 D1~D8은 [E-Test Parsing / DB 설계](../../../문서/설계/04_ETest/2026-10-07_ETest_결과파일_Parsing_DB설계.md). 설계만 됨 / 구현 없음.

KLARF `XD106.02` 연결 분석 (같은 날): Wafer 는 (Lot 핵심 문자열, Slot) 으로 대응, Die 는 시계 90도 + 이동 (4, 5) 잠정 (Defect Die 4 중 3 이 E-Test Fail Die). 설계 문서 8절, 연구소 좌표 대응표로 확정 필요.

범위 조정 (같은 날 사용자 지시): 먼저 파일 간 기준 정보 관계만 설계 -> [파일 간 기준 정보 관계 설계](../../../문서/설계/03_KLARF_Map/2026-10-07_파일간_기준정보_관계_설계.md). 연결 규칙 / Die 좌표 / 결과 Data 는 나중.

E-Test 결과 저장 구현 (CLAUDE-029, 2026-10-07): 테이블 `TQP_GDS_SRC_FILE` / `TQP_GDS_ET_ITEM` / `TQP_GDS_ET_DIE` / `TQP_GDS_ET_VALUE` (로컬 DB 적용), 서비스 `GdsEtResult`, 화면 DMS > E-Test 결과 업로드 (`ENGUI/ETest/frmEtResultUpload`). 13:11 배포, Probe 16/16 / Smoke 17 PASS, DMS 화면 확인 대기. Limit 판정 / KLARF 연결은 나중.

E-Test 중복 기준 변경 (같은 날 사용자 지시): Device 중복 허용, UNIQUE = FACTORY + WAFER_ID, Slot No 는 업로드 화면에서 입력 (설계 12절). 로컬 DB 적용, 배포 승인 대기.

E-Test Spec (CLAUDE-029 2026-10-12, CLAUDE-090 2026-10-14 변경): 키 = Device + 공정 + **등록 Parameter 저장 이름**(E-Test Para 설정에서 등록, 헤더 원문을 넣어도 서버가 저장 이름으로 바꿈, DB FK 로 등록 Parameter 에만). 고객 Spec 표 붙여넣기는 등록되지 않은 Parameter 를 Spec 표 출처로 먼저 등록한 뒤 저장. 정의된 항목만 판정('-' 도 정의 항목만 Fail). Spec 편집은 2026-10-14 CLAUDE-095 부터 **E-Test Para 설정 화면**에서 한다(E-Test Limit 설정 화면 / 메뉴 삭제, CLAUDE-096 부터 결과 파일 등록은 E-Test Para 등록 화면으로 분리): 등록 Parameter 전체(Spec 없는 것 포함) + GDS Chain(값 통계 열은 2026-10-14 사용자 요청으로 삭제), Lower / Upper / Enable 더블클릭 · 클릭 수정, 수정 행만 저장(`SaveEtParameterSetup`), [Spec 표 붙여넣기], 다시 판정 질문. 예전 Chain 묶음 `TQP_GDS_ET_CHAIN_GROUP` / 대표 Chain(`TQP_GDS_ET_REP_CHAIN` / `TQP_GDS_ET_CHAIN_LINK`)은 2026-10-14 삭제 -> Parameter 별 `GDS_CHAIN_NAME` + 인계 API `GetEtChainSpecInfo`(GDS Setup Chain 이름 -> 그 이름을 넣은 모든 공정 Parameter / Spec). [Limit 설계](../../../문서/설계/04_ETest/2026-10-07_ETest_Limit_설정_설계.md) 12절 / [Para Setup 설계](../../../문서/설계/04_ETest/2026-10-14_ETest_Para_Setup_설계.md) 4.5 · 10절.

통합 Die# / 좌표 대응 (CLAUDE-081, 2026-10-13): 고객 Die# 1~76 (BS top view 왼쪽 위부터) 와 E-Test (X,Y) / BS·FS KLARF (X,Y) 대응은 규칙식. Setup 없이 Logic 으로 처리 (사용자 결정): `DACrux.SEMDMS.Interface` 의 `DieCoordinateMapper` (Map 사용 Die + KLARF 원점으로 E-Test / KLARF Back·Front / Die# <-> 시스템 좌표). [Die 좌표 설계](../../../문서/설계/03_KLARF_Map/2026-10-13_Die좌표_대응_Setup_설계.md) 9절. 쓰는 곳 연결은 다음 단계.

E-Test Upload 재설계 (CLAUDE-029 12단계, 2026-10-13): 회의록 기준으로 Pass / Fail 을 Spec 판정으로, 여러 파일 업로드, 업로드 때 Fail Chain 빨간색, $Silterra$ / VIEW_SIDE 저장 중지, 내부 Map 확인 -> [Upload 재설계](../../../문서/설계/04_ETest/2026-10-13_ETest_Upload_재설계.md) (권장안 전부 확정, **2026-10-14 CLAUDE-090 5단계로 구현** — 10.3절).

E-Test 결과 Upload / View 분리 (CLAUDE-113, 2026-10-15): 공통 결과 Control 하나로 두 화면이 같은 모양 -> [설계](../../../문서/설계/04_ETest/2026-10-15_ETest_Upload_View_분리_설계.md) (결정 V1~V6 대기).

## 2026-10-12 GDS × KLARF 연관 분석 화면 설계 (CLAUDE-053)

화면 A(ENGUI `frmGdsKlarfAnalysis`, 기존 DMSearch + Defect Count Color 방식 구간 색) / 화면 B(GdsMap `GdsDieAnalysisForm`, Die 상세) / 읽기 서비스 `GdsKlarfAnalysis.AnalyzeWafer` 설계. 1차 좌표 가정 `gx = XREL - PitchX/2`, `gy = YREL - PitchY/2` (GDS 원점 = Chip 중심), 보기 기준 기본 Back, 결과 저장 안 함, 결과 키 `(Die, Chain)`으로 E-Test 확장 자리. 설계만 됨 / 결정 A1 ~ A10 대기. 상세는 [연관 분석 화면 설계](../../../문서/설계/05_연관분석/2026-10-12_GDS_KLARF_연관분석_화면_설계.md).

연관 분석 구현 (CLAUDE-053, 같은 날, 사용자 "우선 권장한대로 진행"): 서비스 `GdsKlarfAnalysis.AnalyzeWafer`(+ `GdsKlarfMatchEngine`) / 화면 A POC > GDS-KLARF 연관 분석(`frmGdsKlarfAnalysis`, RESV_04 = DM) / 화면 B Die 상세(`GdsDieAnalysisForm` + 호스트 `frmGdsDieAnalysis`, 숨김 메뉴). Probe `GdsKlarfAnalysisProbe` 19/19, Smoke `Verify-GdsKlarfAnalysis` 29/29 (반전 일치 10/10). 실제 데이터 SCK / U9 x XD106.02: Back / 5 µm -> Chain 영역 Defect 7 / Die 2. 배포 / DMS 화면 확인은 사용자. 운영 Service config wellknown 미등록(배포 때 추가). 상세: 설계 문서 13절.

## 2026-10-15 GDS × KLARF × E-Test 통합 분석 데이터 설계 (CLAUDE-119)

세 데이터를 불러오는 방식 / GDS Element 폭 / KLARF 허용오차 / E-Test Chain 연동 / Die 수가 많을 때의 리소스 설계. 소스 수정 없음, 결정 B1 ~ B14 대기. 상세: [통합 분석 데이터 설계](../../../문서/설계/05_연관분석/2026-10-15_GDS_KLARF_ETest_통합분석_데이터_설계.md).

- GDS: Chain Member 모양을 Device마다 한 번 읽어 서버 Cache(Die 수와 무관), Bounds 1차 후보 + 실제 모양(BOUNDARY 다각형 / PATH 폭 · PATH_TYPE 끝 · 꺾임) 2차 거리.
- KLARF: 판정을 거리(Gap)로 바꾸고 허용오차는 거르기 값. 기본 5 µm 유지(무작위 오차 합 약 4.4 µm), Scribe / Seal ring 같은 한 방향 오차는 보정 X / Y. 허용오차 0 ~ 50 µm 표.
- E-Test: `GDS_CHAIN_NAME`이 있는 Parameter만 (Die, Chain) 판정으로 SQL 집계. Wafer는 엔지니어가 파일 선택, Die는 시스템 Die 좌표, Chain은 이름으로 연결. (Die, Chain) 4분류(일치 / Fail만 / Defect만 / 정상).
- 리소스: 결과 축소(Member 단위 행 없앰), Die 상세는 `AnalyzeDie`로 따로, DataSet Binary 전송.
- E-Test Fail 역추적(15절): 1차 = 화면 A "E-Test Fail" 탭(가장 가까운 Defect 거리 포함) + 화면 B 역추적 열기. 2차 = 2 Chain Short 후보. 결정 B15 ~ B17.
- 2026-10-16 (CLAUDE-123): 결정 B1 ~ B20 권장안 확정, 1차 화면 레이아웃 시안 완료(화면만, 서버는 화면 확인 뒤). 화면 A [준비] / 허용오차 / E-Test 연결 / E-Test Fail 탭, 색 기준 4개, Fail Die 모서리 표식, [직접 찾기] 창 / 화면 B 역추적 / E-Test 값 탭. 시안 이미지 `문서/설계/05_연관분석/2026-10-16_1차화면_시안/`, 재생성 `ENGUI/Verification/Preview-GdsKlarfLayout.ps1 -StepSeq 528 -Device OMM_Hybrid`. Smoke 36/36(지금 서버). 설계 문서 17절.
- Wafer 중심 Ready 컨셉(16절, 같은 날 변경): DMSearch 에서 Wafer 를 고르면 Wafer Key + Device 로 KLARF Step 전체 / E-Test 공정별 파일 / GDS 상태를 목록만 모아 [준비] 탭에 보임 -> 체크 / 선택 후 [분석]. Wafer Key 규칙은 고객 협의 대상이라 `WaferKeyRule` 한 파일(잠정 Lot 핵심 + Slot), 못 찾으면 [직접 찾기]. 여러 Step 1차 포함(Step별 보기 기준). 결정 B18 ~ B20.
- 확인 필요(0단계): `GdsMapControl.FlattenStructure`의 중첩 SREF 행렬 순서(부모 변환 먼저 적용), SREF 배율이 PATH 폭에 미반영.

## 2026-10-12 E-Test 헤더 파싱 / 항목 Spec Rule 초안 (CLAUDE-054)

분석 도구 `SampleFile/ETestParsing/` (Python, DMS 소스 / DB 수정 없음). 샘플 RH-HS2615-02 (24 Die × 2,755 항목) 헤더를 TEST / COND_NAME(iforce · Parallel · Serial) / COND_VALUE · UNIT / STRUCT_TYPE(C4DC 좌표형, TSVDC · PIC 이름형) / ID / PIN 으로 나누고, 값이 비슷한 ID_PATTERN 을 묶어 Rule 38개로 관리(Spec = 이상치 제외 평균 ± 3σ 초안). 같은 Resistance 안에서도 구조 / ID 별 값이 9.4 Ω ~ 340 Ω 로 달라, 현재 `TQP_GDS_ET_LIMIT` (키 FACTORY + DEVICE_ID + PARA_NAME) 로는 판정할 수 없다. Limit 키 확장(조건 + 구조 + ID 패턴) 여부는 사용자 결정 대기. 사용법과 그룹 표는 `SampleFile/ETestParsing/README.md`.

## 2026-10-13 Scribe 오프셋 (CLAUDE-074 에서 정리, 고객 확인 대상)

File Map Setup 은 Die / Scribe 크기를 받지 않고 Die Pitch 만 조정한다 (사용자 결정). KLARF / DMS / Map 은 모두 Pitch 칸 기준이고, Scribe 는 KLARF Pitch 칸 -> GDS Chip 좌표로 바꿀 때만 필요하다. 연관 분석은 `TQD_SETUP.DIE_PITCH_X / Y` (KLARF 값)를 쓰므로 Map Setup 에서 바꾼 CHIP_SIZE 와 무관하다 (`GdsKlarfAnalysis.cs` 확인). (2026-10-15 정정: Map Setup 필수(CLAUDE-078 / 083) 이후 `TQD_SETUP` Pitch 는 Map Setup 값이다. KLARF Pitch 와 다르면 Chip 중심이 ΔPitch / 2 밀린다. 대책은 통합 분석 데이터 설계 B6.)

- 현재 변환 `gx = XREL - PitchX / 2 + OffsetX` 는 Chip 이 Pitch 칸 가운데 있다 (Scribe 양쪽 균등, 샘플 X 72 / Y 75 µm 차이 -> 약 36 µm 씩) 는 가정이다.
- Scribe 가 한쪽으로 몰려 있으면 그 차이가 `OffsetX / OffsetY` (화면 보정값, 기본 0) 가 되며, 이것이 Scribe 오프셋 설정이다. 별도 Die / Scribe 입력은 만들지 않는다.
- 고객 확인 대상: Chip 이 Pitch 칸 안 어디에 놓이는지 (가운데 여부), Scribe 폭 (X / Y), GDS 원점 = Chip 중심 여부. 답이 오면 보정 기본값을 정한다 (연관 분석 설계 A5).
- 사용자 판단 (2026-10-13): Map Setup 과 KLARF 가 같은 Pitch 칸 기준이므로 Scribe 는 문제 없을 것으로 본다. GDS 쪽 위치 차이는 연관 분석의 Tolerance(허용오차, `GdsKlarfMatchEngine.Tolerance`) 기능 활용을 검토 중이며 아직 결정하지 않음. 연관 분석 단계에서 다시 검토한다.

## 2026-10-14 E-Test -> YMS 화면 연계 검토 (CLAUDE-085)

YMS Function List 11개 화면(Bin Map View·Gallery / Wafer Cumulative / Bin Stack / Yield Lot·Wafer / Test Map View·Gallery / Multi Parameter / Test Standard / BoxPlot)은 E-Test 데이터로 모두 동작 가능 (구조 검토, 소스 / DB 변경 없음).

- YMS Test 데이터는 `TD_<PROGRAM>`(Die 1행 x Parameter 컬럼, BIN 포함) 범용 구조다.
- E-Test 를 기존 YMS 적재 함수로 옮기는 브리지 1개와 메뉴 / TESTAREA 설정만 있으면 된다.
- 권장 TESTAREA 는 공정명(TSVET / COWET / FT)이다. PARAMETRIC 이면 Map 모양이 나오지 않고 Cum / Bin Stack / Test Report 에서 빠진다.
- 실제 E-Test 파일이 1개라 여러 Wafer 화면은 데이터가 부족하다.
- 상세: [E-Test YMS 화면 연계 검토](../../../문서/설계/04_ETest/2026-10-14_ETest_YMS_화면연계_검토.md).
- 확정 설계 (2026-10-14 사용자 "권장안으로 설계"): 업로드 저장 때 YMS 테이블에도 적재한다 -> [E-Test Upload YMS 적재 설계](../../../문서/설계/04_ETest/2026-10-14_ETest_Upload_YMS적재_설계.md) 14절. 구현 대기.
  - 저장 흐름: `CompleteEtFile` 뒤 별도 호출 `PublishEtFileToYms` 가 기존 YMS 적재 함수를 순서대로 부른다(SaveWaferData -> TestDataManager.SaveData -> SummaryData).
  - 이름: PROGRAM = Device_공정, TESTAREA = 공정, BIN 1 / 2 / 3.
  - 상태 / 키: `TQP_GDS_SRC_FILE.YMS_*`, `TQP_GDS_ET_ITEM.YMS_PARAM_NAME`.
  - 동작: Map 없음 = 보류, 실패 = E-Test 유지 + 다시 반영.
  - Para Setup(CLAUDE-086) P1 ~ P8 결과와 관계없이 E-Test 쪽 저장 결과를 따라간다(14.2절).
- 구현 1차 (2026-10-14, CLAUDE-087, 설계 15절): 규칙 `EtYmsRule`(Interface) / 적재 핵심 `EtYmsPublisher`(BSL, 입력 모델을 받아 기존 YMS 함수로 적재) / 쿼리 `TQP_GDS_ET_YMS.xml` / DDL 파일(미적용).
  - 확인 수준: 빌드 확인 + Probe 확인 (`ET_YMS_RULE_PROBE=PASS 24/24`, `ET_YMS_PUBLISH_PROBE=PASS 21/21` 로컬 DB).
  - 아직 하지 않은 것: `GdsEtResult` 연결 / 업로드 화면 / 메뉴. 배포 안 함.
  - 구현 때 정정: YMS 컬럼 이름 한도 50자(`TQP_PARASPEC.PARAM_NAME`), 넘으면 약어 -> 해시.
  - 기존 `TestDataMaint.DeleteWaferData` 는 Lot 의 마지막 Wafer 를 지울 때 ORA-01400 으로 멈췄다(빈 Lot 요약).
- 2차 정리 (같은 날, 사용자 "진행", 설계 15.5): 기존 YMS 소스 최대 활용.
  - 조회는 기존 쿼리로 바꿨다: `GET_WAFER_INFO [WSQ]` / `GET_WAFER_LOT_INFO` / `SELECT_BIN_DESC`.
  - Probe 전용 쿼리를 제품 XML 에서 뺐다. 새 쿼리는 `UPDATE_YMS_PARASPEC` 1개다. 기존 PARASPEC 저장은 LSL / USL 을 넣지 않기 때문이다.
  - 기존 `TestDataMaint.DeleteWaferData` 빈 Lot 버그를 고쳤고, `DeleteWafer` 는 그 함수를 그대로 부른다.
  - 검증: `ET_YMS_PUBLISH_PROBE=PASS 22/22` (같은 Lot 2장 중 1장 삭제 / 마지막 Wafer 삭제 포함).
  - 배포 안 함.
- 3차: 업로드 서비스 연결 (같은 날, 사용자 "Setup 은 내가, 지금은 프로그램이 돌아갈 수 있게만", 설계 16절).
  - 반영 시점: `CompleteEtFile` / `RejudgeEtFiles` / `RemapEtFiles` 가 YMS 에 자동 반영한다.
  - 삭제: `DeleteEtFile` 은 YMS 도 지운다.
  - Map 없음: WAIT. Map 을 만든 뒤 [Map 좌표 다시 계산] 을 하면 반영된다.
  - 메뉴: YMS 분석 메뉴를 열었다(VISIBLE / RESV_03 / `SELECT_TESTAREA`). 화면 수정은 없다.
  - YMS 컬럼 이름은 Parameter 저장 이름 그대로다(`YMS_PARAM_NAME` 컬럼 안 만듦). -> 2026-10-21 CLAUDE-171 로 바뀜(아래).
    - 2026-10-21 설계(CLAUDE-170): 고객 DB 가 Oracle 11.2.0.4 라 이름 30자 제한. 등록 Parameter 194행(109종)이 모두 31 ~ 50자라 고객 DB 에서 TD 컬럼을 만들 수 없었다. [설계](../../../문서/설계/04_ETest/2026-10-21_ETest_YMS_컬럼이름_30자_설계.md)
    - 현재 동작(2026-10-21 CLAUDE-171 구현, 권장안 Y1 ~ Y9): YMS 컬럼 이름 = `TQP_GDS_ET_ITEM.YMS_PARAM_NAME`(30자 이하, NOT NULL / CK / 같은 Device + 공정 안 UNIQUE). ITEM_NAME(50자)은 그대로.
      - 등록 때 한 번 정해 저장하고 자동으로 바꾸지 않는다: 30자 이하 ITEM_NAME 은 그대로, 넘으면 측정 약어 + 조건 + Chain 1 + Chain 2 끝으로 조립, 그래도 넘으면 21자 + `_` + 해시 8자리(`EtYmsRule.ToYmsName`). 이미 등록된 것이나 같은 요청 안의 다른 Parameter 와 같으면 등록 오류.
      - E-Test Para 설정 [등록 Parameter] 에 "YMS 이름" 열(저장 이름 오른쪽). 더블클릭 / F2 / Ctrl+V 로 고친다(대문자, 형식 / 표 안 겹침은 붉은 칸). 서버는 `SaveEtParameterSetup` 8번째 열로 받아 바뀐 행만 고친다(형식 / YMS 반영된 이름 수정 거부 / YMS 에 이미 있는 컬럼 이름 거부 / Device + 공정 전체 겹침 거부). Interface / RO 함수는 그대로.
      - YMS 반영 여부(`YMS_PUBLISHED`) = 그 이름이 YMS Program(Device_공정)의 `TQP_PARASPEC` 에 있음. 반영된 Parameter 는 회색 글자 + 편집 안 됨. 바꾸려면 그 Program 의 파일 YMS 반영을 모두 지우고(`Probes/EtYmsRepublish.ps1`) 이름을 고친 뒤 다시 반영한다.
      - Program(Device_공정)은 26자 이하(`TD_<Program>` 30자). 업로드 화면 분석 단계에서 넘으면 "이름 오류"로 저장하지 않는다. 업로드 화면에는 YMS 탭이 없다.
      - 로컬 DB: 기존 194행은 규칙 값으로 채웠고(해시 2), SCK OMM / OMMTCB 파일 3개의 YMS 반영을 지우고 다시 반영해 `TD_OMM_TSVET` / `TD_OMMTCB_TSVET` 컬럼과 Parameter 정의가 모두 30자 이하가 됐다.
      - 고객 DB(TRAINING): `06_CoreLibSource/GdsMapControl/Persistence/2026-10-21_tqp_gds_et_item_yms_name.sql` 실행 필요(미실행).
  - 검증: `ET_YMS_SERVICE_PROBE=PASS 9/9`.
  - 운영 반영: 14:33 사용자 VS 빌드로 운영 폴더에 들어갔다. DMS 화면 확인 대기.
- TEST Analysis 메뉴 정리 (CLAUDE-097, 같은 날, 설계 16.6): CP 하위 메뉴를 없애고 Map / Bin·수율 / Parameter 3묶음(구분선)으로 나열, 9개 메뉴 아이콘 새로 그림(`99_Resource/ByModule/POC/make_test_menu_icons.ps1`). Yield Report 를 사용 가능(ENABLE True)으로 바꾸고 캡션 오타(Yeild) 수정. DACrux 다시 시작 후 화면 확인 대기.

## 2026-10-14 E-Test Para Setup / E-Test Chain ↔ GDS Chain 연결 기준 (CLAUDE-086)

E-Test 쪽 "ET Chain 매핑 기준"을 회의록 근거로 정했다 (설계 v2만, 결정 P1 · P2 · P4 ~ P8 대기. 대표 Chain 은 사용자가 E-Test Chain 을 고르고 이름을 입력해 묶고, 자동 묶기는 본 프로젝트 — 2026-10-14 사용자 결정).

- Parameter = 결과 파일 헤더 전체(공백 정리 / 끝 `#번호` 제거 / 대문자), Device + 공정 단위로 Para Setup 화면에서 등록.
- E-Test Chain 이름 = 헤더 `@` 뒤 Chain 이름(Force leg 포함) = GDS Label. 한 Chain 의 양 끝 이름은 다를 수 있고(`TOP1_P_FH` / `TOP1_N_FL`, 2026-10-14 사용자 지시) GDS 는 Input 쪽 이름 하나로 등록하므로, **대표 Chain 을 등록**한다 (`TQP_GDS_ET_REP_CHAIN`, U9 + Chain 이름 -> 대표 이름). 2026-10-14 부터 "단자" 대신 **Chain** 으로 부른다(사용자: "단자1은 결국 chain 이름이다").
- 연결 테이블 `TQP_GDS_ET_CHAIN_LINK` (U9 + 대표 Chain -> GDS CHAIN_SEQ, 양쪽 1:1). GDS 이름이 대표 이름 또는 구성 Chain 중 하나와 똑같으면 연결(이름 비교), 아니면 사용자가 고름. Fail 값 -> Parameter -> Chain -> 대표 Chain -> GDS Chain 영역으로 연관 분석에 쓴다.
- 로컬: Spec 표 Chain 52개 중 10개가 U9 GDS Chain 이름과 똑같다. 자동 묶기 방법(① GDS Chain 영역 안 Chain Label / ② `X_FH` + `X_FL`)과 시험 결과는 본 프로젝트 참고로 설계 12절에 남김.
- 상세: [E-Test Para Setup 설계](../../../문서/설계/04_ETest/2026-10-14_ETest_Para_Setup_설계.md).
- 구현 (CLAUDE-090, 2026-10-14): 이름 변환(Parameter = `EtYmsRule.ToParamName`, Chain·대표 = `EtParameterNameRule.ToSafeName`, 원문 HEADER_TEXT) / 로컬 DB 1단계 DDL(추가만) / 서비스 16개 / 화면 POC > E-Test Para 설정(`frmEtParaSetup`, 탭 파일 헤더 · 등록 Parameter · 대표 Chain). 화면 동작: Device / 공정은 파일명에서 가져오는 읽기 전용 칸, 파일 헤더 표는 "열" 을 왼쪽 행 머리글로 보이고 드래그로 고른 행을 우클릭 / Del 로 목록에서 뺀다(화면에서만, 되살리기 가능). Probe `ET_PARA_SETUP_PROBE=PASS` 32/32, Smoke `ET_PARA_FORM_SMOKE=PASS` 24/24. **배포 / DMS 화면 확인 대기**. 남음: Limit 화면 정리 + 2단계 DDL, Upload 재설계 연결, 연관 분석에서 연결 테이블 사용.
- 4단계 (CLAUDE-090, 2026-10-14): Limit(Spec) 화면 정리 — 위 E-Test Spec 문단 기준으로 바뀜. 로컬 DB 2단계 일부 적용(`2026-10-14_tqp_gds_et_limit_fk.sql`: Spec -> 등록 Parameter FK, Chain 묶음 테이블 DROP). Smoke `ET_LIMIT_FORM_SMOKE=PASS` 41/41 (`Verify-EtLimitSetup.ps1`), Probe 34/34. **배포 / DMS 화면 확인 대기**. 남음: Upload 재설계 연결(저장 대상 = 등록 Parameter) + 2단계 나머지(TEST_PROGRAM NOT NULL).
- 5단계 (CLAUDE-090, 2026-10-14): Upload 재설계 구현 — 저장 대상 = 등록 Parameter(사용 Y), 판정 = Spec(Enable) 을 서버 SQL 과 화면 미리보기(`EtJudgeRule`)가 같은 규칙으로 저장(값 -> Die -> 파일), Map ID = 파일 Device 의 사용 Die 로 시스템 좌표 저장, Spec 을 바꾸면 다시 판정 질문(`RejudgeEtFiles`), Map 을 나중에 만들면 [Map 좌표 다시 계산](`RemapEtFiles`). 업로드 화면 / 조회 창 다시 작성. 로컬 DB 2b 적용(데이터 정리 -> `TEST_PROGRAM` NOT NULL + 판정 / Map 컬럼). 자동 검사: Upload Probe 18/18 / Upload Smoke 19 / Limit 43 / Para 27 / Para Probe 34. **배포 / DMS 화면 확인 대기** — 직접 시험은 [테스트 시나리오](../../../문서/설계/04_ETest/2026-10-14_ETest_Para_Spec_Upload_테스트_시나리오.md). 남음: YMS 적재 연결(`PublishEtFileToYms`).
- 진행 표시줄 / 대량 처리 (CLAUDE-094, 2026-10-14): Para 설정에서 2,755개 삭제가 한 건씩 DB 왕복(약 8,300번)이라 오래 걸려 멈춘 것처럼 보였다. 서버 삭제 / 사용 저장을 묶음 처리로 바꾸고(2,755개 삭제 서버 0.1초), POC 화면 아래에 공통 진행 표시줄(`DACrux.SEMDMS.Control.ProgressStatusBar`)을 넣었다. 서버 호출은 작업 스레드에서 돌고 작업 이름 / 경과 시간 / 막대가 움직이며, 작업 중 그 화면 입력과 닫기는 막는다. 상세: [POC 화면 진행 표시줄](../../../문서/설계/06_환경_운영/2026-10-14_POC_화면_진행표시줄.md).
- 화면 재구성 (CLAUDE-095, 2026-10-14, **구현 완료 / 배포 · DMS 화면 확인 대기**): 대표 Chain 대신 Parameter 별 **GDS Chain** 한 칸(`TQP_GDS_ET_ITEM.GDS_CHAIN_NAME`, Chain 1 의 GDS Setup Chain 이름, 사용자가 직접 입력 / 오른쪽 GDS Setup Chain 목록에서 복사, 목록에 없는 이름은 주황 경고). Para 설정 화면 하나에서 등록([파일 헤더] 탭 [등록]) + GDS Chain + Spec 편집([등록 Parameter] 탭, 수정 행만 저장, Spec 값이 바뀐 행만 다시 씀). Device / 공정은 파일을 연 동안만 잠그고 그 밖에는 콤보로 골라 바로 조회. 삭제: [대표 Chain] 탭 / 대표 Chain 테이블 2개(로컬 DB DROP) / 서비스 11개 / E-Test Limit 설정 화면 · 메뉴 / 업로드 Fail Chain 의 대표 Chain 열 / [Spec 표로 고르기]. 연관 분석이 쓸 연결은 Fail 값 -> Parameter -> GDS_CHAIN_NAME -> GDS Setup Chain(이름, 대소문자 무시). `ET_PARA_SETUP_PROBE=PASS` 26 / `ET_PARA_FORM_SMOKE=PASS` 37 / `ET_RESULT_PROBE=PASS` 18 / `ET_UPLOAD_FORM_SMOKE=PASS` 30. [Para 화면 재구성 설계](../../../문서/설계/04_ETest/2026-10-14_ETest_Para_화면_재구성_설계.md) / [직접 시험 시나리오](../../../문서/설계/04_ETest/2026-10-14_ETest_Para_Spec_Upload_테스트_시나리오.md).
- 결과 Upload / View 화면 분리 (CLAUDE-113, 2026-10-15, **구현 완료 / 배포 · DMS 화면 확인 대기**): POC > **E-Test 결과 업로드**(`frmEtResultUpload`) = 파일 올리기만(**파일 하나씩**, 2026-10-15 CLAUDE-114: [파일 열기] / [다시 읽기] / Slot / [저장] + 결과 확인 + 저장 수 확인 "Parameter p x Die d = 값 v" -> 저장), [등록 파일] 탭 / 팝업 조회 창 제거. 다중 파일은 본 프로젝트(참고 사본 `문서/설계/04_ETest/참고소스_다중파일_업로드_20261015`). POC > **E-Test 결과 조회**(`frmEtResultView`, 신규 메뉴 F03127 순서 8) = Device / 공정 / Lot 으로 저장 파일 목록(추가 Para / Spec 바뀜 / Map / YMS 열) + 삭제 ([Map 좌표 다시 계산] 은 2026-10-15 CLAUDE-116 삭제: 업로드 때 Map(Map ID = Device) 이 없으면 오류, Map 을 고치면 다시 업로드). 두 화면의 결과는 **같은 Control**(`EtResultDataControl` + 모델 `EtResultData`): 정보 표(파일명 ~ 등록 11행, 업로드 저장 전에는 Device / 공정 / Lot / Lot-Wafer No 더블클릭 수정 -> 다시 분석) / 1단계 Die(DUT · 좌표 · Map · 판정 · Fail 수) / 2단계 정의 Para 값(한 행 = Die x Parameter, 고른 Die · Fail 만 · '-' 만 · 찾기로 거름), Parameter 순서 = 표시 순서 -> 저장 이름. 나중에 등록한 Parameter 는 다시 업로드해야 보인다(조회 목록 "추가 Para" = 업로드 뒤 등록·변경되고 파일에 없는 수). 서버 = 조회 쿼리 / 메서드 1개, DDL 없음. `ET_UPLOAD_FORM_SMOKE=PASS` 39(업로드 미리보기와 조회 888행 같음 포함). 1단계 '-' 수는 정의 Para 기준(Spec 무관) / `ET_RESULT_PROBE=PASS` 18. [Upload / View 분리 설계](../../../문서/설계/04_ETest/2026-10-15_ETest_Upload_View_분리_설계.md).
- 등록 / 설정 화면 분리 (CLAUDE-096, 2026-10-14, **구현 완료 / 배포 · DMS 화면 확인 대기**): POC > **E-Test Para 등록**(`frmEtParaRegister`, 순서 5, 새 아이콘) = 결과 파일을 열어 Factory 만 입력하고 Device / 공정 / 파일은 파일명에서(읽기 전용), 헤더 체크 후 [등록]만. POC > **E-Test Para 설정**(`frmEtParaSetup`, 순서 6) = 등록된 Device / 공정을 콤보로 골라(직접 입력 없음) 조회 / GDS Chain · Spec · 사용 수정 / Spec 표 붙여넣기 / 삭제. 서버 / DB 변경 없음. `ET_PARA_FORM_SMOKE=PASS` 41 / `ET_UPLOAD_FORM_SMOKE=PASS` 30. [화면 분리 설계](../../../문서/설계/04_ETest/2026-10-14_ETest_Para_등록_설정_화면_분리_설계.md).
- 설정 화면 필터 콤보 (CLAUDE-121, 2026-10-15, **구현 완료 / 배포 · DMS 화면 확인 대기**): 표 위 측정 / 조건 / Chain 1 / GDS Chain / 사용 / Enable 콤보. 펼치면 `(전체)` + 값(건수, 다른 콤보 통과 행 기준), 고르면 그 값 행만(AND, 수정 행은 보임). 같은 Device / 공정 다시 조회는 유지, 바뀌면 (전체). [필터 해제]. 서버 변경 없음. `ET_PARA_FORM_SMOKE=PASS` 47. [화면 분리 설계 5절](../../../문서/설계/04_ETest/2026-10-14_ETest_Para_등록_설정_화면_분리_설계.md). 같은 날 오른쪽 GDS Device 는 고르지 않고 위 E-Test Device 이름으로 조회(읽기 전용, 없으면 빈 목록).

- YMS Bin 기준 (2026-10-14, CLAUDE-102): YMS BIN = E-Test 결과 파일 Hardware Bin (Software Bin 은 Bin 정의 이름에), 양품 = 파일 DUT 결과. POC Limit 판정과 별개이며 Measure 는 Setup Parameter + Setup LSL/USL. 설계 `문서/설계/04_ETest/2026-10-14_ETest_Upload_YMS적재_설계.md` 18절.
- YMS Map Die 표시 (2026-10-14, CLAUDE-103): Map 사용 Die 와 결과 파일 Die 가 **둘 다 있는 Die 만** 보인다. CLAUDE-098 의 "측정 안 한 Map Die = 하늘색 Mark Die" 표시는 취소(서버 Mark Die 조회 원래대로 + `TPUCMapView` 원래대로, 원점 LeftBottom 환산은 유지). 결과 파일에만 있는 Die 는 Map 좌표가 없어 원래 YMS 에 안 들어간다. 수율 분모(NETDIE = Map 사용 Die 수)는 결정 대기. 검토 문서 `문서/설계/04_ETest/2026-10-14_ETest_YMS_화면연계_검토.md` 10.2절.

## 2026-10-16 단위 확인 / E-Test 업로드 단위 비교 (CLAUDE-125 / 126)

- 길이 단위: GDS / KLARF / Map Setup 모두 시스템 안은 µm(GDS 파일은 nm 정수, KLARF SampleSize / Map EDGE_SIZE 만 mm). 연관 분석 허용오차 / 보정 / Gap 도 µm. E-Test 는 길이 값이 없다(DUT = Die 번호).
- E-Test 측정값 단위는 파일 Unit 행(V / Ohm / uA)을 `TQP_GDS_ET_ITEM.UNIT` 에 그대로 두고 값은 환산하지 않는다.
- 2026-10-16 수정: 업로드 계획 `EtUploadPlan.Build`(`ENGUI/ETest/EtJudgeRule.cs`)가 파일 Unit 과 등록 단위를 비교한다(둘 다 있을 때, 대소문자 구분 = Para 등록과 같은 규칙). 다르면 그 Parameter 는 저장 / 판정하지 않고 상태 경고 "단위 다름 n 건너뜀 (예: 저장 이름: 파일 nA / 등록 uA)"(`frmEtResultUpload`). 누락으로는 세지 않는다. 서버 변경 없음.
- 검증: `Verify-EtResultUpload.ps1` -> `ET_UPLOAD_FORM_SMOKE=PASS`(새 검사: 첫 Parameter 단위를 nOhm 으로 바꾸면 저장 36 / 누락 0 / 경고) + `DIE_COORD_PROBE=PASS (11/11)`, TEMP 빌드 오류 0, PROBE 시험 데이터 정리됨.
- 화면 정리 (2026-10-22, CLAUDE-194):
  - E-Test Para 설정(`frmEtParaSetup`): 표의 YMS 이름 / 조건 / Spec 원문 / 출처 열 숨김(`Visible = false`, 값은 채움). Chain 1 / Chain 2 열은 한 번 숨겼다가 같은 날 다시 보이게 함. 필터 줄의 조건 콤보도 숨기고 Chain 1 / GDS Chain / 사용 / Enable / [필터 해제] 를 왼쪽으로 당겼다(숨긴 콤보는 (전체)로 남아 거르지 않음, 필터 처리 목록에는 그대로). 여러 칸 붙여넣기는 보이는 열에만 붙으므로 YMS 이름은 화면에서 고칠 수 없다.
  - E-Test 결과 업로드(`frmEtResultUpload`): 위 둘째 줄 상태 / 메시지(`lblState` / `lblMessage`) 숨김, 위 막대 Designer 100 -> 60(실행 40). 상태는 결과 Control 제목, 메시지(오류 이유 / 경고)는 분석 뒤 아래 상태 표시줄 뒤에 붙인다(`AutoEllipsis`).
  - 확인: TEMP 빌드 오류 0 + 화면 밖 띄워 보기(`POC_BUTTON_LAYOUT=PASS`, 4화면) / Smoke 미실행 / 배포는 사용자 VS 빌드.

## 2026-10-16 Scribe 위치 / GDS Chip 크기 / Pitch 비교 (CLAUDE-128, 결정 대기)

- 지금: KLARF `XREL / YREL` 을 Die 칸(Die + Scribe) 왼쪽 아래 기준으로 보고 `X = XREL − Pitch / 2 + 보정 X` 로 Chip 중심 좌표로 옮긴다. Chip 이 칸 가운데(Scribe 양쪽 균등)라는 가정이다. Scribe 정의 / GDS Chip 크기 / Pitch 비교는 시스템에 없다(DMS `TQD_SETUP.STREET_X / Y` 는 Glass 용, 모두 0).
- KLARF 규격은 칸 안 Scribe 위치를 정하지 않는다. 검사 Recipe 의 Die 기준점(Scribe 교차점 중앙 = 가운데 / Chip 왼쪽 아래 모서리 = 오른쪽 위)에 따른다. 샘플(STEP 541)로는 정하지 못했다.
- 실측(OMM): KLARF Pitch 23,832 × 30,060, GDS Chip(전체 Layer) 23,760 × 29,985(원점 = 중심), 고객 Die(Scribe 제외) 23,680 × 29,905. 한쪽 차이 Pitch − GDS 36 / 37.5, GDS − Die 40 / 40. Layer 일부만 저장한 Map 은 저장 도형 범위가 Chip 크기와 다르다(U9 X −11,810 ~ +11,793.6).
- 설계 추가: 통합 분석 설계 20절, 결정 B23(Device 별 Scribe 위치 가운데 / 오른쪽 위 / 직접 입력 + Step 별 변경) / B24(GDS Chip 크기 저장, `TQP_GDS_MAP` 컬럼) / B25([준비] 점검 Pitch 비교 + Chip 밖 Defect). 고객 확인 질문 4개는 20.6.

## 통합 연관 분석 서버 개발 (2026-10-16, CLAUDE-127)

- 상태: **완료(코드) / TEMP 빌드 + Probe · Smoke 확인 / 배포 안 함 / 사용자 DMS 화면 확인 대기**. 상세: 통합 분석 설계 21절, 변경이력 2026-10-16 CLAUDE-127.
- 서비스(`iGdsKlarfAnalysis`): `GetWaferReady`(Wafer Key 로 같은 Wafer KLARF Step / E-Test 파일 + GDS 상태 + 점검) / `Analyze(GdsKlarfAnalysisRequest)`(실제 모양 Gap 판정, 여러 Step, Chain 면 / 검사 면, 허용오차 표, E-Test 4분류 · Fail률) / `AnalyzeDie`(Die 1개 + Member 단위 + E-Test 값). `AnalyzeWafer` 는 호환용으로 남김.
- 모양 판정: BOUNDARY 다각형 / PATH 폭 · 끝 처리 · 꺾임, 서버 Cache(Device 별, LRU 3). OMM_Hybrid 첫 읽기 1.3초, 이후 0초.
- Wafer Key 규칙: `Interface/Coordinate/WaferKeyRule.cs` 잠정(LOT 가운데 조각 + Slot 2자리), 고객 규칙이 오면 이 파일만 바꾼다.
- KLARF 업로드: Step COMMENT 에 원본 Die Pitch(`RAW_DIE_PITCH`), 분석이 먼저 쓴다(이전에 올린 Step 은 Map Setup Pitch).
- 검증 스크립트: `02_DMS/DACrux.SEMDMS.BSL/Probes/GdsKlarfShapeProbe.ps1`(33) / `GdsKlarfAnalyzeProbe.ps1`(14) / `GdsKlarfEtProbe.ps1`(13, 시험 데이터 PROBE 만들고 삭제) / `ENGUI/Verification/Verify-GdsKlarfAnalysis.ps1`(42).
- 남은 것: 배포(서버 · 화면 함께), 통합 설계 20절 B23 ~ B25(Scribe / Chip 크기 / Pitch 비교) 결정 뒤 반영, 면(FS / BS) 확인(STEP 528 은 반전 없이 Gap 0 Defect 6개).
- 2026-10-16 CLAUDE-132: Ready 조회 KLARF Step 은 Step 당 한 줄이다(`SELECT_KLARF_STEP_BY_WAFER` GROUP BY, Defect = 검사 Test 합). Test 가 2개인 KLARF(1107236219)가 두 줄로 나와 [분석]이 "같은 KLARF Step 이 두 번" 으로 멈추던 문제. 화면 `ReadySteps` / [직접 찾기]도 같은 Step 을 한 번만 보낸다.

## 2026-10-16 [준비] 탭 KLARF Step 목록 (CLAUDE-134, 결정 B26 / B27)

- 같은 파일 다시 올림은 업로드가 지우고 다시 쓰므로 목록에서 중복 처리하지 않는다(사용자 결정). Product 만 다르게 올린 같은 파일은 남아 있을 수 있어 Ready 는 고른 Step 과 같은 Product 만 모은다(B26, 쿼리 `SELECT_KLARF_STEP_BY_WAFER_PRODUCT`).
- 같은 Wafer + 같은 Step ID 재검사는 `TQD_STEP.TEST_ORDER`(새 업로드마다 +1, 작을수록 최신) 순으로 놓고 Step ID 마다 한 행만 기본 체크한다(고른 Step 우선, 없으면 최신). 다른 행은 "이전 검사" / "이후 검사"(B27, `GdsKlarfReadyStepRule`).
- 한 파일 안의 검사 Test(InspectionTest 3 / 80)는 CLAUDE-132 에서 Step 당 한 줄로 묶음. 상세: 통합 분석 설계 22절.

- 2026-10-16 CLAUDE-135: 연관 분석 화면 A / B 는 Chain 을 이름으로만 보인다(코드 A / B / AA .. 화면에 없음). [Die] 탭 X / Y 열, Defect 전체 / Chain In / Chain Out, 주요 Chain 열 삭제. 설계 23절.
- 2026-10-16 CLAUDE-139 (설계, 결정 대기): Die 상세 여러 화면 + 도면 재사용. 측정 = 창마다 도면 데이터 약 6초 + 도면 만들기 약 4초 / 도면 컨트롤 하나 약 1.85 GB. 권장 = 도면 하나를 보이는 상세보기 탭으로 옮겨 씀 + [분석] 때 미리 읽기. [설계](../../../문서/설계/05_연관분석/2026-10-16_Die상세_여러화면_GDS재사용_설계.md)
- 2026-10-16 CLAUDE-140: 화면 A 색 구간 표 끝에 "E-Test Fail Die" 행(Map 왼쪽 위 빨간 삼각형 범례 + Die 수). E-Test 판정이 있을 때만 보이고 색 기준과 상관없다. 표식 Die 는 위 구간 행에도 함께 세어진다(합계에서 뺀다). `GDS_KLARF_FORM_SMOKE=PASS (47/47)`, 배포 안 함.
- 2026-10-19 CLAUDE-145: 화면 A 빨간 삼각형 기준 라디오 "Spec Out"(기본, DMS Spec 판정) / "Fail Die"(결과 파일 DUT Flag). 서버 Analyze DIE 표에 `ET_FILE_FLAG` 열 추가(끝 열, 예전 화면은 무시). `GDS_KLARF_ET_PROBE=PASS (16/16)`, `GDS_KLARF_FORM_SMOKE=PASS (61/61)`, 배포 안 함(BSL + ENGUI).
- 2026-10-20 CLAUDE-151 / 152: 상세보기(화면 B) [GDS] + [KLARF] 탭 -> [GDS / KLARF] 탭 하나. 한 행 = (Chain, Defect), Chain 칸은 묶음처럼 그림, 걸린 Defect 없는 Chain 은 빈 1행, 두 Chain Defect 는 두 묶음 모두, 맨 아래 "Chain 밖 Defect"(가까운 거리 순). Defect 범위 콤보 대신 [Chain 밖 Defect] 체크. 서버 변경 없음. `GDS_KLARF_FORM_SMOKE=PASS (70/70)`, 실제 Die (4, 6) 52행 확인. 배포 안 함(GdsMap EXE).
- 2026-10-20 CLAUDE-155: 상세보기 [GDS / KLARF] 표 행 선택 = Defect 로 이동 / 강조만(노란 Chain 칠하기 없음, Chain 칸 클릭도 칠하지 않음). 두 Chain 에 걸린 Defect 는 두 묶음 모두 + "함께 걸림"(SCKLOTIDA-01 STEP 603 에 6개, 대부분 P / N 쌍). Smoke 70/70.
- 2026-10-20 CLAUDE-156: 상세보기 [GDS / KLARF] 표에서 두 Chain 에 걸린 Defect 는 Defect ID 굵게, "가까운 Chain" 열을 Gap 바로 뒤로(가로 스크롤 없이 "함께 걸림" 보임). Smoke 71/71.
- 2026-10-16 CLAUDE-139 (구현, 결정 D1 ~ D6 권장안 / D7 도면 메모리 줄이기는 CLAUDE-142 추후): Die 상세 = Die 마다 상세보기 창(같은 Die 는 그 창, 역추적 / 재분석이면 그 창에 새 결과), GDS 상세 화면(도면) 하나를 활성 창으로 옮겨 씀(창마다 결과 / 보기 상태 저장 / 복원, 없는 창은 안내 문구), [분석] 뒤 도면 데이터 미리 읽기(`GdsDieMapCache`), 화면 A 와 상세보기 창을 모두 닫으면 해제. Smoke 54/54: 첫 상세보기 11.6 -> 5.2초(미리 읽기 7.4초 백그라운드), 두 번째 Die 창 0.94초, DB 다시 읽기 0 / 도면 핸들 같음. 배포 안 함 / DMS 화면 확인 대기. 상세: 설계 9절
- 2026-10-16 CLAUDE-139 (창끼리 간섭 4건 수정): 읽는 중 같은 키 창으로 넘어가면 늦게 끝난 읽기가 덮던 버그 수정 / 새 창은 앞 창 상태줄 · Chain 고른 행을 이어받지 않음(상태줄도 창마다 저장) / 미리 읽기 캐시 2칸(조건 다른 창 전환 8.7 ~ 10 -> 2.4 ~ 4.2초) / 재분석 뒤 이전 결과 창 탭 제목 "(이전 분석)". Smoke 61/61(메시지 루프 안 검사 포함). 상세: 설계 10절
- 2026-10-19 CLAUDE-146 (설계, 결정 L1 ~ L4 대기): 화면 A 레이아웃 후보 3개 - 안 1 기능 탭 = 기준 + 결과(권장, 색 구간 = Map 아래 범례 + 팝업) / 안 2 기준 탭 · 결과 탭 2단 / 안 3 단계 탭 + 색 구간 팝업. [설계](../../../문서/설계/05_연관분석/2026-10-19_화면A_레이아웃_후보.md)
- 2026-10-19 CLAUDE-146 (구현, 결정 "권장안으로 하되 색구간설정은 팝업으로"): 화면 A = 위쪽 "지금 기준" 요약 줄 / 왼쪽 Map + [색 구간 설정] 팝업 + Map 왼쪽 아래 범례 / 오른쪽 기능 탭 [GDS] [KLARF] [E-Test] [종합 (Die)](탭마다 기준 묶음 + 접기 + 결과 하위 탭), [준비] 탭 없앰. Wafer 받으면 KLARF, 첫 분석 결과면 종합 (Die). Smoke 64/64, 배포 안 함 / DMS 화면 확인 대기. [설계](../../../문서/설계/05_연관분석/2026-10-19_화면A_레이아웃_후보.md) 8절
- 2026-10-19 데이터 검증 (SCKLOTIDA-01 / OMM_TCB / TSVET, 판정 DONE_WITH_CONCERNS, 검사 47 중 PASS 44): 원본 -> DB 는 **DB 확인**(KLARF 10/12 · E-Test 8/9 · GDS 5/5), 분석 결과는 설계 규칙으로 따로 다시 계산한 값과 모두 같음 **Probe 확인**(9/9: 걸림 472 / (Defect, Chain) 478 / (Die, Chain) 298 4분류 / 허용오차 표 / E-Test 연결 17 / Chain Fail률), 화면 A 표시는 화면 밖 실행으로 분석 결과 · 사용자 사진과 같음 **Probe 확인**(12/12: 범례 19 / 12 / 7 / 3 / 2 / 8 / 25 / Spec Out 25, Defect (472), E-Test Fail (149)). 운영 DMS 화면은 사진 숫자 대조만. 다른 값 3건은 연관 분석 영향 없음: KLARF 원본 Summary 1098 vs 목록 1122(원본 문제) / TQD_INSP_INFO Test 80 DEFECTIVE_DIE 51(원본 1, 기존 업로드 동작, 원인 조사 중) / E-Test 파일 내용 Wafer ID U9-HS2635-01(이름만 바꾼 시험 파일로 보임). GDS 저장은 완전히 같은 도형을 한 번만 저장한다(파서 규칙, 설계 문서에 없음). 반전 · 보정 · Scribe 가 실제 Wafer 에 맞는지는 고객 확인 대상으로 남음. 리포트 `.testagent/reports/run-20261019-103914-F07DATA/report.md`, 변경이력 2026-10-19
- 2026-10-19 CLAUDE-148 (상세보기 기능 탭, 결정 "1안으로 하되 해당 화면에 컨셉에 맞게 문구나 용어를 잘 확인"): 상세보기(화면 B) = 위 분석 기준 요약 3칸(화면 A 문구) + Die 줄 / 왼쪽 GDS 도면 / 오른쪽 기능 탭 [GDS] Chain 표 · [KLARF] Defect · [E-Test] E-Test 값 · [종합 (Die)] Die 요약 + 4분류 수 + 일치 · Fail만 · Defect만 Chain 표. 처음 열면 종합, 역추적이면 KLARF(탭 제목 "(역추적)"). 용어 화면 A 와 맞춤(비교 불가 / Defect 수 / Defect ID / RoughBin / Spec Out · Fail Die). Smoke 68/68 + 실제 데이터 Probe 9/9, 배포 안 함(GdsMap EXE) / DMS 화면 확인 대기. [설계](../../../문서/설계/05_연관분석/2026-10-19_화면A_레이아웃_후보.md) 9 · 10절

## 화면 A 정리 (2026-10-20, CLAUDE-149)

- 위쪽 "지금 기준" 요약 3칸과 Map 왼쪽 아래 범례를 없앴다. 색 구간 / Die 수는 [색 구간 설정] 팝업 표에서만 본다.
- GDS Chain 면 기본값을 BS 로 바꿨다(화면 콤보 / 서버 Ready / Analyze 요청이 비었을 때). 기본 조건(Chain BS · 검사 BS)은 반전 없이 판정한다.
- 상태: 완료(코드) / TEMP 빌드 + Smoke 67 · EtProbe 17 / 배포 안 함 / DMS 화면 확인 대기.

## 화면 A KLARF 탭 Step 정보 / Wafer 1개만 분석 (2026-10-20, CLAUDE-150)

- KLARF 탭의 Step 표와 KLARF [직접 찾기]를 없앴다. 고른 Step 정보 두 줄(Step / Lot, 검사 시각 / Defect / 검사 Die) + 검사 면 [BS / FS](기본 = Ready 값 BS) + "Pitch 출처" 문구만 보인다. [분석]은 고른 Step 1개 + 이 검사 면으로 요청한다.
- 여러 KLARF Step 합치기(결정 B16)는 2차로 되돌렸다. 서버 Analyze 의 `StepSeqs[]` / `ViewSides[]` 기능은 그대로다. 상세: 통합 분석 설계 11절 B16 / 15.4 / 16.6 / 16.7, 화면 A 레이아웃 설계 12절.
- GDS / KLARF / E-Test 탭 위쪽 제목에 "(Wafer 1개만 분석)", Wafer 여러 개 선택 오류 = "1개 Wafer 만 분석할 수 있습니다. Wafer 를 하나만 선택하세요. (선택 n개)".
- 상태: 완료(코드, ENGUI 만) / TEMP 빌드 + Smoke 69/69 / 배포 안 함 / DMS 화면 확인 대기.
- 2026-10-20 CLAUDE-153 (Map E-Test 표식 2종, 결정 "안 1, E1 ~ E4 권장안"): 화면 A Wafer Map 에 Spec Out(DMS Spec 판정 F) = Die 왼쪽 위 빨간 삼각형, Fail Die(결과 파일 DUT Flag F) = 오른쪽 아래 작은 남색 삼각형을 함께 그린다. [E-Test] 탭 라디오 -> 체크 2개(기본 켬), 색 구간 팝업 끝 2행, 상태줄 "E-Test Spec Out (…) · 결과 파일 Fail Die". Smoke 70/70 + 실제 데이터(SCKLOTIDA-01 25 / 73) Probe 5/5, 배포 안 함(ENGUI) / DMS 화면 확인 대기. [설계](../../../문서/설계/05_연관분석/2026-10-20_Map_ETest_표식_설계.md)
- 2026-10-20 CLAUDE-154 (결정 B28): KLARF 검사 면 = FS 고정, 화면에 보이지 않음(KLARF 탭 검사 면 콤보 삭제). 고르는 것은 GDS Chain 면만(기본 BS). 반전 규칙(Chain 면 ≠ 검사 면)은 그대로라 기본 조건 Chain BS · 검사 FS = 반전, STEP 528 Chain In 1 / 15(Chain FS 로 바꾸면 8 / 15). KLARF / E-Test Die 는 그대로: 서버가 검사 면 FS 일 때 KLARF Die 번호를 좌우로 바꾸던 것을 없앴다(ToSystem / FromSystem = Index 그대로). 서버 Ready VIEW_SIDE = FRONT, 요청 검사 면이 비면 FRONT. BSL + ENGUI, TEMP 빌드 + Smoke 70/70 + EtProbe 17/17 + AnalyzeProbe 14/14, 배포 안 함 / DMS 화면 확인 대기. 설계: 통합 분석 B28 / 6.4 / 18절, 화면 A 레이아웃 13절.
- 2026-10-20 CLAUDE-157: 화면 A 상태줄과 상세보기 "분석 기준" GDS 칸 · GDS / KLARF 기준 줄에서도 검사 면을 뺐다(KLARF 검사 면 FS 고정, B28). 상태줄 "BACK 보기 (Chain BS -> 반전)", 상세보기 "GDS · Chain BS -> 반전". ENGUI + GdsMap EXE, TEMP 빌드 + Smoke 71/71, 배포 안 함 / DMS 화면 확인 대기.
- 2026-10-20 CLAUDE-160 (E-Test 목록 조건, 결정 B29 = 권장안 3개): 화면 A [E-Test] 탭 목록은 Wafer Key 같음 + E-Test Device = 분석 GDS Device 인 파일만(서버 Ready), 표 위 Step 콤보(E-Test Step = 파일 Step ID = 공정)로 고른 Step 파일만 보이고 1개 선택. Device 가 다른 같은 Wafer 파일은 상태줄 "E-Test n개는 Device 가 GDS Device X 와 달라 목록에서 뺌(…)", [직접 찾기]로는 더할 수 있다(찾기 Device 기본 = GDS Device). 실제 데이터: STEP 601(OMMTCB) -> 331 / 603(OMM_TCB)·528(OMM_Hybrid) -> 0개. BSL + ENGUI, Probe 18/18 + Smoke 75/75, 배포 안 함 / DMS 화면 확인 대기. [설계](../../../문서/설계/05_연관분석/2026-10-20_ETest_목록_조건_설계.md)
- 2026-10-20 CLAUDE-167: 화면 A 배치 검사 실패 2건 수정(Designer 위치만). KLARF 탭 Pitch 출처 글자가 패널 아래로 넘침 -> 단계 정보 높이 48 -> 46 / Pitch 출처 Y 62 -> 56, E-Test 탭 Spec Out 과 Fail Die(결과 파일) 체크 겹침 -> Fail Die X 300 -> 316. `Verify-PocButtonLayout` PASS. 배포 대기.

## U8 처럼 Chain 이 많은 Device 대응 (2026-10-21, CLAUDE-179 / 180)

- 확인(CLAUDE-179): U8(Chain 27,342개)은 판정 계산은 그대로지만 상세보기 열기(Chain 마다 Member 조회), 상세보기 표 전체 Chain 표시, 서버 모양 Cache 한도 초과, 화면 A Chain 이름 찾기에서 느려졌다. [영향 확인](../../../문서/설계/05_연관분석/2026-10-21_U8_연관분석_영향_확인.md)
- 현재 동작(CLAUDE-180, 결정 = 권장안):
  - 상세보기는 Chain Member 를 Chain 번호 5,000개 범위로 묶어 읽는다(`GetChainMembersRange`). 서버에 범위 조회가 없으면 Chain 마다 읽는다.
  - 서버 모양 Cache 는 Device 1개 점 2,000만 개까지 둔다(U8 1,613만 점, 상주 약 370 MB, 최근 3 Device).
  - 화면 A 는 결과가 바뀔 때 Chain 찾기 사전을 1번 만든다.
  - (2026-10-28 CLAUDE-225 로 삭제) ~~Chain 이 1,000개를 넘는 Device 의 상세보기 표는 그 Die 의 DIE_CHAIN Chain 만~~ -> 지금은 Chain 수와 상관없이 모든 Chain. 대신 상세보기 [걸린 / Fail Chain 만]이 기본 켬이라 처음에는 걸린 / E-Test Fail Chain 만 보이고(없는 Die 는 모든 Chain), 끄면 U8 도 27,342개 전부(로컬 13.4초 / Private 3.8 GB).
- 효과(로컬 측정): 상세보기 67.7 ~ 75.9초 -> 20.0초, [분석] 두 번째부터 7.5초 -> 0.6초, 화면 A Defect 탭 2.3초 -> 8ms, [걸린 / Fail Chain 만] 끄기 11.6초 / 27,352행 -> 0.16초 / 93행.
- 상태: 완료(코드) / TEMP 빌드 + Smoke 76/76 / 운영 폴더는 13:20 VS 빌드로 이미 반영됨(Service / DACrux 다시 시작 필요) / DMS 화면 확인 대기. U8 KLARF / E-Test 업로드 뒤 재측정은 남음.

## GDS-KLARF 연관 분석 Simple 화면 설계 (2026-10-22, CLAUDE-192)

- 요청: 화면 A 보다 단순한 새 화면. Wafer 1개를 받으면 기본 설정으로 1차 분석(KLARF Defect 가 GDS Chain 위에 올라갔는지, Defect 크기 그대로), Die 마다 Chain 위 Defect 수, E-Test 없음(삼각형 없음), 색 구간 그대로, 오른쪽 탭 2개(설정 + 간단한 정보 / Die 종합), 좌표 보정 제외, Die 더블클릭 상세는 2단계.
- 설계: 새 화면 `frmGdsKlarfSimple`(ENGUI, 화면 A 보존), 같은 서버 `Analyze` 를 `SIZE_TOL` + 허용오차 0 + 보정 0 + `EtSrcFileSeq` 0 으로 부른다. **서버 / Interface / DB 변경 없음**. `GetWaferReady` 는 부르지 않는다.
- 판정 뜻(소스 확인): Defect 좌표 중심 XSIZE × YSIZE 사각형과 Chain Member 실제 모양의 Gap ≤ 허용오차(0 = 겹치거나 닿음). 크기 0 은 점. DSIZE / DEFECTAREA 는 쓰지 않음.
- 오른쪽 탭: [설정 / 정보] = 분석 정보 / Defect 영역 설정(+ Chain 면, [분석]) / 선택 Die 의 Chain 위 Defect 이미지 카드(`DPUCDefectImageCard`) / Chain 목록, [종합 (Die)] = 화면 A Die 표에서 E-Test 열 뺀 것.
  - 2026-10-22 CLAUDE-217: 탭을 없애고 한 페이지로 바꿨다. 위에서부터 분석 정보 / Defect 영역 설정 / KLARF 위치 조정 + [다시 분석] / 종합(Die 표) / Defect 이미지(맨 아래). Chain 목록과 체크 2개는 뺐다.
  - 2026-10-22 CLAUDE-219: Die Index(= 고객 통합 Die#, Factory + Product 의 TQP_USEMAP 사용 Die 를 위 행부터 · 같은 행 왼쪽부터 번호, `ENGUI/DefectMapAnalysis/GdsDieIndex`) 표시. Wafer Map 머리 = 마우스 Die "X / Y / Index", 종합 표 맨 왼쪽 Index 열, 상세보기 Die 목록 No 다음 Index 열. 서버 변경 없음(기존 `GetEtMapDieList`). Simple 설계 23절.
- 결정(같은 날): "권장안 S1 ~ S10 으로 1단계 구현 진행".
- 구현(1단계, 설계 12절): POC > **GDS-KLARF 연관 분석 (Simple)** (`ENGUI/DefectMapAnalysis/frmGdsKlarfSimple`, 메뉴 `MNU_GDS_KLARF_SIMPLE` / F03129 / RESV_04 = DM, 소스 MenuFile 에만) + 색 구간 팝업 `GdsKlarfBandPopup`(화면 A 와 같은 동작, 새 화면 전용) + `GdsKlarfAnalysisMap.ShowDetailMenu`(기본 true, Simple 은 false). Wafer 받으면 자동 분석, 설정 바꾸면 "설정 바뀜" -> [분석], Die 클릭 = 그 Die 의 Chain 위 Defect 이미지 카드 / Chain 목록, Die 더블클릭 = Die 선택만(2단계 자리).
- 검증: TEMP 빌드(새 경고 없음) + `Verify-GdsKlarfSimple.ps1` -> `GDS_KLARF_SIMPLE_FORM_SMOKE=PASS (17/17)` STEP 528(Defect 15, 허용오차 0 / 5 = Chain 위 1 / 1) · STEP 603(1,122, 437 / 472). Smoke 는 DB 연결 이름을 DMSMGR(로컬)로 바꿔 쓴다(서버 config 는 지금 실제 앱 DB CPKMESDV). 버튼 배치 Simple 화면 FAIL 0. 이미지 카드 실제 FTP 읽기는 Smoke 안에서 실패(원인 추정: 원격 설정 없음) -> DMS 화면 확인.
- POC 문구 규칙 적용(같은 날 요청 "Simple 화면에도 CLAUDE-193 과 같은 문구 규칙 적용", 설계 13절): 탭 "종합", 라디오 "Defect 크기 + 허용오차", 입력란 옆 `µm` Label 삭제, 분석 정보 / 상태줄 기준 "Defect 크기 그대로 / 크기 + 허용오차 n" (단위 없음), 이름 글자 설명 괄호 삭제. 값 괄호(개수 / 좌표 / 비율 / 시간, 상태줄)와 창 제목 / 메뉴 "(Simple)" 은 그대로. Smoke 18/18(문구 규칙 검사 추가) · 528 / 603.
- 레이아웃 정리(같은 날 요청 4건, 설계 14절): Map 위 "색 = Chain 위 Defect 수" 삭제 / 아래 상태줄 삭제(알림은 분석 정보 "알림" 줄, 진행 표시는 위 막대 오른쪽에 작업 중에만) / 버튼 "다시 분석" / 이미지 = 고른 Die 의 모든 Defect(Chain 위 · 밖 묶음과 체크 삭제, 분석 결과와 무관, 결정 S5 바뀜). "분석 버튼 아래 살짝 보이는 것" = [Chain 밖 Defect 도 보기] 체크였고 함께 없앰. Smoke 17/17 · 528 / 603, 버튼 배치 Simple FAIL 0.
- 2단계 Die 상세보기(같은 날 요청, 설계 15절): Map Die 더블클릭 / 우클릭 [Detail Analysis] / [종합] 행 더블클릭 -> 화면 A 상세보기(`frmGdsDieAnalysis` + `GdsDieAnalysisForm`)를 마지막 분석 요청 그대로 연다. Simple 결과(`DETAIL_VIEW = SIMPLE`)일 때만: 위쪽 분석 기준 칸 · [GDS / KLARF] 체크 줄 숨김, 모든 Chain + Chain 밖 묶음, 처음 탭 GDS / KLARF, 도면 Defect = Chain 안 빨강 / 밖 노랑 채움(흰 점선 · #번호 없음, `GdsMapControl.DefectStyle = Filled`), 크기 = 분석 설정, E-Test 를 분석하지 않아 비는 분류 / ET / Fail Para 열과 [E-Test] / [종합] 탭 숨김(사용자 결정, 설계 15.7절). 화면 A 상세보기는 그대로. Smoke 27/27 · 528 / 603. 표 머리글 목록은 설계 15.5절.
- 기능별 구분선(CLAUDE-198, 설계 16절): [설정 / 정보] 탭 기능 사이 전체 폭 선 3개. 별도 TEMP 빌드 + 배치 검사 PASS, Smoke 미실행(동작 변경 없음).
- KLARF 위치 조정(CLAUDE-198, 구현 완료(코드, K1 ~ K8 권장안) / Smoke 33/33 · 528 / 603 / 배포 안 함 / DMS 화면 확인 대기): Defect 영역 설정 아래 X / Y + [◀][▲][▼][▶] 1 씩 + [초기화] + [다시 분석]. 화살표는 값만 바꾸고 [다시 분석] 때 반영, Wafer 를 바꿔도 값 유지, 0 이 아니면 분석 정보 기준 줄에 표시(2026-10-22 CLAUDE-209 에서 분석 정보 5줄을 빼 지금은 숫자칸에서만 보임). 서버 보정값이 반전 전 KLARF 좌표계에서 더해지므로(= REL 조정) 서버 변경 없음. 화면 A 보정값은 그대로. [설계](../../../문서/설계/05_연관분석/2026-10-22_KLARF_위치조정_설계.md)
- Die 상세 화면 DLL 로딩 + 왼쪽 Die 목록(CLAUDE-200, 구현 완료(코드) / Smoke 화면 A 81/81 · Simple 35/35 · 528 / 603 / 배포 안 함 / DMS 화면 확인 대기):
  - GdsMap 을 EXE 에서 DLL(`NexplantQMS.GdsMap.dll`, 같은 GdsMap 폴더)로 바꿨다. GDS 호스트 4개는 `GdsMapAssembly` 로 그 DLL 을 한 번만 읽는다.
  - 상세보기 창은 전체에서 하나다. 열려 있으면 다른 Die 를 더블클릭해도 그 창 목록에서 고른다.
  - 왼쪽 목록 열: No / X / Y / Defect / Chain 위 / E-Test Fail. 고르면 0.3초 뒤 호스트가 `AnalyzeDie` 로 계산하고, Die 별 결과 / 보기 상태를 기억한다.
  - 분석 화면이 결과를 반영할 때마다 상세보기도 같은 Die 를 새 요청으로 다시 계산한다(`SourceAnalyzed`). 보기 상태는 이어 간다.
  - 공유 도면 / 미리 읽기는 그대로다. [설계](../../../문서/설계/05_연관분석/2026-10-22_Die상세_DLL로딩_Die목록_설계.md)
- 상태: **구현 완료(코드) / TEMP 빌드 + Smoke 확인 / 배포 안 함 / DMS 화면 확인 대기**. 배포 = ENGUI DLL + `NexplantQMS.GdsMap.exe`(100_ClientBin/GdsMap) + `MenuFile.mnu`(소스 -> 100_ClientBin) + DACrux 재시작(서버 변경 없음). [설계](../../../문서/설계/05_연관분석/2026-10-22_GDS_KLARF_연관분석_Simple_화면_설계.md)

## 상세보기 오른쪽 한 패널 설계 (2026-10-22, CLAUDE-197)

- 요청: 오른쪽 탭 3개([GDS / KLARF] [E-Test] [종합])를 한 패널로. GDS / KLARF 표가 주, 고른 행 Chain 의 이 Die E-Test Parameter 를 아래 표로([E-Test 값] 체크로 보이기 / 숨기기), 종합 정보는 위 표에 엮기, "가까운 Chain" 정보 삭제.
- 설계: 종합은 4분류 수 칸 + 분류 글자색만 위 표로(나머지는 위쪽 Die 줄 / 위 표 / 아래 표와 겹침), 가까운 Chain 열 삭제(Chain 밖 Gap 은 남김). 공유 화면이라 화면 A · Simple 상세보기가 함께 바뀜. 서버 변경 없음, 배포는 GdsMap EXE.
- 구현(결정 P1 ~ P9 권장안): `GdsDieAnalysisForm` 오른쪽 = 기준 묶음(+ E-Test 줄) / 4분류 수 칸 / 머리 줄([걸린 / Fail Chain 만] [Chain 밖 Defect] [E-Test 값]) / 가로 나누기 위 GDS / KLARF 표 + 아래 E-Test 값 표(아래 35%). 아래 표 = 고른 행 Chain(없음 = Die 전체, Chain 밖 = 빈 표). E-Test 없는 결과 · Simple 은 체크 · 아래 표 · 4분류 칸 숨김. `Verify-GdsKlarfAnalysis.ps1` 에 `-ConnectId`(기본 DMSMGR) / `-SkipBuild` / `-Capture`.
- 검증: 화면 A Smoke 528 82/82 · 603 81/82(무관 1건: 화면 A [KLARF] 탭 Pitch 출처 기대값), E-Test 연결(같은 Wafer 파일 361 / 331)로 아래 표 범위 · 접기 · 분류 색 확인, Simple 27/27, 버튼 배치 상세 FAIL 0.
- 상태: **구현 완료(코드) / TEMP 빌드 + Smoke 확인 / 배포 안 함(`NexplantQMS.GdsMap.exe`) / DMS 화면 확인 대기**. [설계](../../../문서/설계/05_연관분석/2026-10-22_상세보기_오른쪽_한패널_설계.md)
- 두 화면 같은 모양 + Simple E-Test 자동 연결(같은 날 CLAUDE-199, 설계 12절): 요청 "두 화면에서 동일하게 / 컨셉대로 전부", 결정 "완전히 같게(두 요청 합침)" + "Simple E-Test = 같은 Wafer 최신 파일 자동". Simple 보기 표시(DETAIL_VIEW) 삭제 -> 두 화면 공통 = 분석 기준 칸 · 표 위 요약 숨김, [걸린 / Fail Chain 만] 기본 끔, Defect 빨강 / 노랑 채움(점선 · 번호 없음), 한 패널(E-Test 있으면 체크 · 아래 표 · 4분류 칸 · E-Test 열). Simple 은 GetWaferReady 로 같은 Wafer + 같은 Device 최신 READY E-Test 자동 연결 + 분석 정보 E-Test 줄. Simple Smoke 33/33, 화면 A 82/82(528). 배포 = GdsMap EXE + ENGUI DLL.
- 상세보기 [다시 분석] + 정리(같은 날 CLAUDE-205, 설계 13절): 위쪽 오른쪽 [다시 분석] = 연 분석 화면의 [다시 분석] / [분석](지금 설정) -> SourceAnalyzed 로 상세보기 새 결과. [Chain 밖 Defect] 체크 삭제(늘 보임), 4분류 상태 칸 5개 삭제, 기준의 최종 저장 / 분석 시각 삭제. [걸린 / Fail Chain 만] 은 유지(기본 끔). 화면 A 80/80(528), Simple 36/36. 배포 = GdsMap DLL + ENGUI DLL.
- 도면 머리 줄 좁은 폭(같은 날 CLAUDE-206, 설계 14절): Die 목록 옆에서 도면 칸이 좁으면(약 190px) 보기 / [배경 도면] / [원점 표시] / [전체 보기] 가 겹치던 것을 FlowLayoutPanel 로 바꿔 다음 줄로 넘김(넓으면 한 줄 32, 190px = 4줄). [전체 보기] 는 [원점 표시] 뒤. 화면 A 81/81(528), Simple 36/36(603). 배포 = GdsMap DLL.
- 버튼 · 열 · 문구 정리(같은 날 CLAUDE-208, 설계 15절): [역추적 해제](체크 줄 아래) 실행 112 x 28, Member / Gap / GDS X / GDS Y 열 숨김, [전체 보기] 삭제, [배경 도면] -> [전체 도면], 기준 묶음 E-Test 줄 늘 숨김. 사용자 VS 150% 저장으로 눌린 아래 E-Test 값 표를 35% 로 맞춤. 화면 A 81/81(528), Simple 36/36(603). 배포 = GdsMap DLL.
- 아래 E-Test 값 행 선택 칠하기 삭제(같은 날 CLAUDE-211, 설계 16절): 행을 골라도 도면 Chain 을 노랗게 칠하지 않고 상태줄만(위 표는 2026-10-20 에 이미 뺌). 화면 A 81/81(528), Simple 36/36(603). 배포 = GdsMap DLL.
- 표 더블클릭 삭제 / 이미지 = 오른쪽 클릭(같은 날 CLAUDE-216, 설계 17절): 더블클릭 역추적에서 회색 행이 빠지고 자동 스크롤돼 줄이 사라져 보이던 것(원인 분석) 뒤 요청으로 더블클릭 = 아무 일 없음. 클릭 = Defect 따라감, 표시 체크 그대로, Defect 행 오른쪽 클릭 = [Defect 이미지 보기]. 역추적은 분석 화면 Chain 지정 열기만. 화면 A 82/82(528), Simple 36/36(603). 배포 = GdsMap DLL.

## E-Test Bin 설정 화면 설계 (2026-10-22, CLAUDE-201)

- 요청: 기존 `TEST.ENGUI/Admin/frmSetupBin`(TestArea -> Program -> Bin 0 ~ 150 고정 표)을 POC 성격에 맞게 하나 더. 기준은 바꿔도 됨.
- 설계: 신규 `SEMDMS.ENGUI/ETest/frmEtBinSetup`. 고르는 기준 = E-Test Device / 공정(YMS Program = `EtYmsRule.ToProgramName`, 읽기 전용). 행 = Bin 정의 + 파일 Die 의 Hardware Bin. 고치는 값 = 이름 / 양품 / 색 / 설명, 참고 열 = Software Bin / Die / Pass / Fail / 파일 / 상태. 저장 뒤 양품이 바뀌면 기존 `RejudgeEtFiles` 로 YMS 수율 다시 계산 질문. 서버 = `GdsEtResult` 메서드 4개 + `TQP_GDS_ET_YMS.xml` 쿼리 2개, Bin 정의는 기존 TEST DSL `TQP_BINDESC` 함수(DDL 없음). 기존 frmSetupBin 은 그대로.
- 로컬 DB(읽기): OMM_TSVET 파일 Bin 1 / 5, 정의 1 / 2 / 4 / 5(2 / 4 는 파일에 없음), 수율 83.33. OMMTCB_TSVET 정의 2 / 4, 양품 없음.
- 구현(같은 날, "진행" = B1 ~ B9 권장안): 화면 `frmEtBinSetup` + 메뉴 `MNU_DMS_ET_BIN`(POC, E-Test Para Setup 바로 뒤) + 서버 `GdsEtResult_Bin` + `TQP_GDS_ET_YMS.xml` 쿼리 5개. 기존 `MERGE_BIN` 이 설명을 40자에서 잘라(실측) Bin 정의 읽기 / 쓰기 / 삭제는 전용 쿼리로 바꿈(4열만 UPDATE, 숨긴 열 유지).
- 검증: `ET_BIN_SETUP_PROBE=PASS (15/15)` / `ET_BIN_SETUP_FORM_SMOKE=PASS` (17) / 버튼 배치 새 화면 OK. 양품 변경 -> 수율 다시 계산 83.33 -> 100 -> 83.33 확인.
- 상태: **구현 완료(코드) / TEMP 빌드 + Probe + 화면 밖 Smoke 확인 / 배포 안 함 / DMS 화면 확인 대기**. [설계](../../../문서/설계/04_ETest/2026-10-22_ETest_Bin_Setup_화면_설계.md) 13절
- 후속(CLAUDE-215): 화면 확인 뒤 요청으로 Die / Pass / Fail / 파일 / 상태 열 숨김(값은 채움). 보이는 열 = Bin / 이름 / 양품 / 색 / 설명 / Software Bin. Smoke PASS, ENGUI DLL 만 바뀜.
- 후속(CLAUDE-207): 기존 `TQP_BINDESC.xml` `MERGE_BIN` / `SELECT_BIN_EDITABLE` 바인딩 size 40 / 25 -> 0. 기존 frmSetupBin / frmSetupProgram 도 긴 설명 / 26자 Program 이 잘리지 않음(Probe 확인, TEST Service 경유 화면은 미확인).
