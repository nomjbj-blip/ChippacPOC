# F07 / AOI / E-Test / GDS Correlation

확인일: 2026-10-02 / 상태: 기존 요구 검토 문서와 선행 기능 기준 / 통합 실행 미확인

## 목적과 근거

AOI KLARF의 Defect 위치가 GDS Chain을 구성하는 Bump / RDL 영역과 관련되는지 확인하고 E-Test Fail Chain과 연결한다.

근거는 [9/30 문서 검토 종합](../../../문서/2026-09-30_문서검토_종합.md)과 [Chain Setup 컨셉안](../../../문서/2026-09-23_Chain_Setup_기능_컨셉안.md)이다. 이번 정리에서는 고객 원본 PDF / XLSX를 새로 분석하지 않았다. 확정 고객 요구로 인용할 때 원본과 최신 협의 내용을 다시 확인한다.

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

KLARF Defect와의 연관분석은 Chain 영역(현재 Chain Version의 Member 도형)만 대상으로 한다. 배경 도면 도형은 비교하지 않는다. 1차 후보는 Defect 좌표 +/- Tolerance와 Member 도형 Bounds 겹침. 상세는 [GDS Map DB 저장 설계 13.3절](../../../문서/2026-10-02_GDS_Map_DB저장_설계.md).


## 2026-10-07 E-Test 결과 파일 설계 (CLAUDE-029)

샘플 `SampleFile/26_10_06 Sample File/CAI_OMM_ATV_A0_TSVET_..._RH-HS2615-02.xlsx` 분석 결과 헤더 형식은 `{Para}[_{조건}={값}]@{Chain}[:{Chain2}]` (Leakage_Serial 은 Chain 2개). 파일 Limit 은 쓰지 않고 별도 Limit 테이블 / 화면으로 판정한다 (Connectivity / Resistance 는 파일 Limit 이 N/A 라 Open 판정에 Limit 화면 필수). E-Test Chain 이름(최대 19자)과 GDS `CHAIN_CODE` 는 FK 없이 이름으로 대응. 상세와 결정 대기 항목 D1~D8은 [E-Test Parsing / DB 설계](../../../문서/2026-10-07_ETest_결과파일_Parsing_DB설계.md). 설계만 됨 / 구현 없음.

KLARF `XD106.02` 연결 분석 (같은 날): Wafer 는 (Lot 핵심 문자열, Slot) 으로 대응, Die 는 시계 90도 + 이동 (4, 5) 잠정 (Defect Die 4 중 3 이 E-Test Fail Die). 설계 문서 8절, 연구소 좌표 대응표로 확정 필요.

범위 조정 (같은 날 사용자 지시): 먼저 파일 간 기준 정보 관계만 설계 -> [파일 간 기준 정보 관계 설계](../../../문서/2026-10-07_파일간_기준정보_관계_설계.md). 연결 규칙 / Die 좌표 / 결과 Data 는 나중.

E-Test 결과 저장 구현 (CLAUDE-029, 2026-10-07): 테이블 `TQP_GDS_SRC_FILE` / `TQP_GDS_ET_ITEM` / `TQP_GDS_ET_DIE` / `TQP_GDS_ET_VALUE` (로컬 DB 적용), 서비스 `GdsEtResult`, 화면 DMS > E-Test 결과 업로드 (`ENGUI/ETest/frmEtResultUpload`). 13:11 배포, Probe 16/16 / Smoke 17 PASS, DMS 화면 확인 대기. Limit 판정 / KLARF 연결은 나중.

E-Test 중복 기준 변경 (같은 날 사용자 지시): Device 중복 허용, UNIQUE = FACTORY + WAFER_ID, Slot No 는 업로드 화면에서 입력 (설계 12절). 로컬 DB 적용, 배포 승인 대기.

E-Test Spec (CLAUDE-029, 2026-10-12): 키 = Device + 공정 + Parameter(헤더 전체, '#번호' 제거, 대문자), 고객 Spec 표 붙여넣기로 등록, 정의된 항목만 판정('-' 도 정의 항목만 Fail). E-Test Chain 묶음 `TQP_GDS_ET_CHAIN_GROUP` (대표 이름 없음, 보통 _FH + _FL, Device 단위) + Setup Chain(GDS) 으로 찾기: 같은 이름 -> 없으면 이름_FH / _FL -> 묶인 Chain 전부의 Spec / 측정 항목 (`GetEtChainSpecInfo`). [Limit 설계](../../../문서/2026-10-07_ETest_Limit_설정_설계.md) 12 · 14 · 15절.

통합 Die# / 좌표 대응 (CLAUDE-081, 2026-10-13): 고객 Die# 1~76 (BS top view 왼쪽 위부터) 와 E-Test (X,Y) / BS·FS KLARF (X,Y) 대응은 규칙식. Setup 없이 Logic 으로 처리 (사용자 결정): `DACrux.SEMDMS.Interface` 의 `DieCoordinateMapper` (Map 사용 Die + KLARF 원점으로 E-Test / KLARF Back·Front / Die# <-> 시스템 좌표). [Die 좌표 설계](../../../문서/2026-10-13_Die좌표_대응_Setup_설계.md) 9절. 쓰는 곳 연결은 다음 단계.

E-Test Upload 재설계 (CLAUDE-029 12단계, 2026-10-13): 회의록 기준으로 Pass / Fail 을 Spec 판정으로, 여러 파일 업로드, 업로드 때 Fail Chain 빨간색, $Silterra$ / VIEW_SIDE 저장 중지, 내부 Map 확인 -> [Upload 재설계](../../../문서/2026-10-13_ETest_Upload_재설계.md) (결정 U1~U9 대기).

## 2026-10-12 GDS × KLARF 연관 분석 화면 설계 (CLAUDE-053)

화면 A(ENGUI `frmGdsKlarfAnalysis`, 기존 DMSearch + Defect Count Color 방식 구간 색) / 화면 B(GdsMap `GdsDieAnalysisForm`, Die 상세) / 읽기 서비스 `GdsKlarfAnalysis.AnalyzeWafer` 설계. 1차 좌표 가정 `gx = XREL - PitchX/2`, `gy = YREL - PitchY/2` (GDS 원점 = Chip 중심), 보기 기준 기본 Back, 결과 저장 안 함, 결과 키 `(Die, Chain)`으로 E-Test 확장 자리. 설계만 됨 / 결정 A1 ~ A10 대기. 상세는 [연관 분석 화면 설계](../../../문서/2026-10-12_GDS_KLARF_연관분석_화면_설계.md).

연관 분석 구현 (CLAUDE-053, 같은 날, 사용자 "우선 권장한대로 진행"): 서비스 `GdsKlarfAnalysis.AnalyzeWafer`(+ `GdsKlarfMatchEngine`) / 화면 A POC > GDS-KLARF 연관 분석(`frmGdsKlarfAnalysis`, RESV_04 = DM) / 화면 B Die 상세(`GdsDieAnalysisForm` + 호스트 `frmGdsDieAnalysis`, 숨김 메뉴). Probe `GdsKlarfAnalysisProbe` 19/19, Smoke `Verify-GdsKlarfAnalysis` 29/29 (반전 일치 10/10). 실제 데이터 SCK / U9 x XD106.02: Back / 5 µm -> Chain 영역 Defect 7 / Die 2. 배포 / DMS 화면 확인은 사용자. 운영 Service config wellknown 미등록(배포 때 추가). 상세: 설계 문서 13절.

## 2026-10-12 E-Test 헤더 파싱 / 항목 Spec Rule 초안 (CLAUDE-054)

분석 도구 `SampleFile/ETestParsing/` (Python, DMS 소스 / DB 수정 없음). 샘플 RH-HS2615-02 (24 Die × 2,755 항목) 헤더를 TEST / COND_NAME(iforce · Parallel · Serial) / COND_VALUE · UNIT / STRUCT_TYPE(C4DC 좌표형, TSVDC · PIC 이름형) / ID / PIN 으로 나누고, 값이 비슷한 ID_PATTERN 을 묶어 Rule 38개로 관리(Spec = 이상치 제외 평균 ± 3σ 초안). 같은 Resistance 안에서도 구조 / ID 별 값이 9.4 Ω ~ 340 Ω 로 달라, 현재 `TQP_GDS_ET_LIMIT` (키 FACTORY + DEVICE_ID + PARA_NAME) 로는 판정할 수 없다. Limit 키 확장(조건 + 구조 + ID 패턴) 여부는 사용자 결정 대기. 사용법과 그룹 표는 `SampleFile/ETestParsing/README.md`.

## 2026-10-13 Scribe 오프셋 (CLAUDE-074 에서 정리, 고객 확인 대상)

File Map Setup 은 Die / Scribe 크기를 받지 않고 Die Pitch 만 조정한다 (사용자 결정). KLARF / DMS / Map 은 모두 Pitch 칸 기준이고, Scribe 는 KLARF Pitch 칸 -> GDS Chip 좌표로 바꿀 때만 필요하다. 연관 분석은 `TQD_SETUP.DIE_PITCH_X / Y` (KLARF 값)를 쓰므로 Map Setup 에서 바꾼 CHIP_SIZE 와 무관하다 (`GdsKlarfAnalysis.cs` 확인).

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
- 상세: [E-Test YMS 화면 연계 검토](../../../문서/2026-10-14_ETest_YMS_화면연계_검토.md).
- 확정 설계 (2026-10-14 사용자 "권장안으로 설계"): 업로드 저장 때 YMS 테이블에도 적재한다 -> [E-Test Upload YMS 적재 설계](../../../문서/2026-10-14_ETest_Upload_YMS적재_설계.md) 14절. 구현 대기.
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

## 2026-10-14 E-Test Para Setup / E-Test Chain ↔ GDS Chain 연결 기준 (CLAUDE-086)

E-Test 쪽 "ET Chain 매핑 기준"을 회의록 근거로 정했다 (설계 v2만, 결정 P1 · P2 · P4 ~ P8 대기. 대표 Chain 은 사용자가 단자를 고르고 이름을 입력해 묶고, 자동 묶기는 본 프로젝트 — 2026-10-14 사용자 결정).

- Parameter = 결과 파일 헤더 전체(공백 정리 / 끝 `#번호` 제거 / 대문자), Device + 공정 단위로 Para Setup 화면에서 등록.
- 단자 이름 = 헤더 `@` 뒤 Chain 이름(Force leg 포함) = GDS Label. 한 Chain 의 양 끝 단자 이름은 다를 수 있고(`TOP1_P_FH` / `TOP1_N_FL`, 2026-10-14 사용자 지시) GDS 는 Input 단자 이름 하나로 등록하므로, **대표 Chain 을 등록**한다 (`TQP_GDS_ET_REP_CHAIN`, U9 + 단자 -> 대표 이름).
- 연결 테이블 `TQP_GDS_ET_CHAIN_LINK` (U9 + 대표 Chain -> GDS CHAIN_SEQ, 양쪽 1:1). GDS 이름이 대표 이름 또는 구성 단자 중 하나와 똑같으면 연결(이름 비교), 아니면 사용자가 고름. Fail 값 -> Parameter -> 단자 -> 대표 Chain -> GDS Chain 영역으로 연관 분석에 쓴다.
- 로컬: Spec 표 단자 52개 중 10개가 U9 GDS Chain 이름과 똑같다. 자동 묶기 방법(① GDS Chain 영역 단자 Label / ② `X_FH` + `X_FL`)과 시험 결과는 본 프로젝트 참고로 설계 12절에 남김.
- 상세: [E-Test Para Setup 설계](../../../문서/2026-10-14_ETest_Para_Setup_설계.md).
