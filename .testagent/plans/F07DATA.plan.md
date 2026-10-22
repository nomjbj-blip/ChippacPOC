# F07DATA 연관 분석 데이터 검증 테스트 계획 (SCKLOTIDA-01 / OMM_TCB / TSVET)

- 대상 표면: DB(원본 -> 저장 값), .NET(BSL Analyze 결과, 화면 A 표시)
- 범위 단계: Standard
- 근거 문서: `문서/설계/05_연관분석/2026-10-15_GDS_KLARF_ETest_통합분석_데이터_설계.md` 4 ~ 7절 / 18 ~ 19절 / 21.2절, KLARF 1.2 형식, `GdsMapPlacedRowBuilder.cs`(POINTS_BIN v1)
- 변경 범위: 새 기능 변경 검증이 아니라 운영 데이터 1 Wafer 의 적재 / 분석 / 표시 대조. 소스 수정 없음
- 기준 데이터:
  - KLARF `SampleFile/26_10_14 Sample File/TEST.SINF` (STEP_SEQ 582, Step OMM_C4Bump_TCB_FullSilterra)
  - E-Test `SampleFile/26_10_14 Sample File/CAI_OMMTCB_ATV_A0_TSVET_20261002161846_SCKLOTIDA_SCKLOTIDA-01$Silterra$.xlsx` (SRC_FILE_SEQ 331)
  - GDS `SampleFile/OMM all layer2.gds/OMM all layer2.gds` (SCK / OMM_TCB, READY Chain 19)
  - E-Test Die 대응 `SampleFile/U9_Die좌표_대응표_20261012.tsv` + KLARF -> DMS 이동 (7, 5)
- DB 쓰기: 없음(BSL 조회 함수만). 덤프 도구는 `%TEMP%\ChippacGdsKlarfAnalysisVerification\bin` 의 TEMP 빌드 DLL 사용
- 기존 검증 자산: `GdsKlarfAnalysisFormSmoke.cs`(화면 흐름, 다른 Step 기준) - 이 Wafer 값 대조는 하지 않아 새 Probe 를 만든다
- 작성/갱신: 2026-10-19

## 시나리오

### SCN-K [정상] KLARF 원본 -> TQD_STEP / TQD_DEFECT / TQD_INSP_INFO
- 단계: 원본을 Probe 가 직접 읽고(cp949) DB 덤프와 비교
  - expect: Wafer ID SCKLOTIDA-01(LOT_CORE_SLOT) / Product OMM_TCB(업로드 화면 입력 우선) / Pitch 23832 x 30058, 출처 KLARF / Defect 1122 행, 11개 열 1:1 / Die 번호 = 원본 + (7, 5) / Wafer X·Y 재계산 0.01 이내 / 검사 Die(Test 3 Plan) 76 (원본 KLARF)
- 실행 수단: `probes/f07data_klarf_check.py` 키 `KLARF_CHECK_PROBE`
- 실패 의미: 업로드 파서 / Die 원점 맞춤 / Wafer ID 규칙 결함

### SCN-E [정상] E-Test xlsx -> TQP_GDS_SRC_FILE / ET_DIE / ET_VALUE
- expect: Die 73 / 값 85 x 73 = 6205 원본 칸과 같음 / 판정 = DB Spec(SPEC_TEXT) 재계산 / Die 판정 Pass 48 · Fail 25 / MAP_X·Y = 고객 대응표 + (7, 5)
- 실행 수단: `probes/f07data_etest_check.py` 키 `ETEST_CHECK_PROBE`
- 실패 의미: 파서 열 찾기 / 판정 SQL / Die 좌표 대응 결함

### SCN-G [정상] GDS 원본 -> TQP_GDS_MAP / LAYER / PLACED / Chain Member
- expect: 저장 Layer 3 / 5 / 775 의 도형 수 = 원본에서 완전히 같은 도형을 한 번 센 수(파서 `GdsLayerList.AddElement` 규칙), 점 수 = 원본(BOUNDARY 닫는 점 포함) / Member 62609 도형 = 원본 요소(nm 좌표 전체, 폭, 타입) / Chain 19 Member 수
- 실행 수단: `probes/f07data_gds_check.py`(원본 읽기 `f07data_gds_lib.py`) 키 `GDS_CHECK_PROBE`
- 실패 의미: GDS 저장 / 좌표 단위 / Member 연결 결함

### SCN-A [정상] 분석 결과 = 설계 규칙으로 따로 다시 계산한 값
- expect: 반전 규칙(18절) / gx·gy(5.2) / 모양 Gap(4.3) / 걸림 Gap <= 5 / 허용오차 표(5.5) / ET 이름 연결(6.5) / (Die, Chain) 상태 · 4분류(6.2, 7.1) / Chain 별 Die 수 · Fail률(7.2) / Die 표 / ANL_INFO 수
- 실행 수단: `probes/f07data_analysis_check.py` 키 `ANALYSIS_CHECK_PROBE` (제품 C# 를 옮기지 않고 문서 규칙으로 새로 씀)
- 실패 의미: 판정 엔진 / 결과 집계 결함

### SCN-D [정상] 화면 A 표시 = 분석 결과 + 화면 규칙 + 사용자 사진 값
- 단계: `F07DisplayDump.exe` 로 화면 A 를 화면 밖에 띄워 [분석] -> 범례 / 구간 표 / Map 채움색 · 숫자 · 삼각형 / 탭 제목 / 표 7종 / Die 76개 하나씩 선택 시 Chain · Defect 탭을 CSV 로 받음
  - expect: 범례 1~7 19 / 8~14 12 / 15~21 7 / 22~28 3 / 29 이상 2 / 기준 미만 8 / Defect 없음 25 / Spec Out Die 25, 탭 Defect (472) / E-Test Fail (149) (사용자 사진)
- 실행 수단: `probes/F07DisplayDump.cs` + `probes/f07data_display_check.py` 키 `DISPLAY_CHECK_PROBE`
- 실패 의미: 화면 바인딩 / 색 구간 / 4분류 문구 결함

### SCN-RG [부정] Probe 가 틀린 값을 잡는가 (red-green)
- 단계: 덤프 사본 5곳을 바꿈(KLARF XREL / E-Test 값 / Member 좌표 / 4분류 / Map 숫자) 후 같은 Probe 실행
  - expect: K6 / E5 / G3 / A1·A2·A5·A7 / D3·D9·D11 FAIL
- 데이터 정리: 사본은 세션 임시 폴더, DB 쓰기 없음

## 해당 없음
- [입력검증] [예외] [권한/상태] [경계] 해당 없음 — 업로드 / 분석 기능 변경 검증이 아니라 이미 저장된 1 Wafer 데이터 대조다. 업로드 입력 검증은 기존 KLARF / E-Test 업로드 Probe 범위

## 추적표
| 시나리오 | 근거 | 실행 수단 | 확인 수준 목표 |
| --- | --- | --- | --- |
| SCN-K | KLARF 1.2 / KlarfUpload 규칙 | f07data_klarf_check.py | DB 확인 |
| SCN-E | E-Test 파서 / B12 판정 | f07data_etest_check.py | DB 확인 |
| SCN-G | GDS Map DB 저장 / 4.1절 | f07data_gds_check.py | DB 확인 |
| SCN-A | 통합분석 설계 4 ~ 7 / 18 / 19 / 21.2절 | f07data_analysis_check.py | Probe 확인 |
| SCN-D | 화면 A 규칙(색 구간 / 4분류 / 표) | F07DisplayDump + f07data_display_check.py | Probe 확인(화면 밖 실행) |
