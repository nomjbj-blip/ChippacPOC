# 검증 리포트 — F07DATA 연관 분석 데이터 검증 (SCKLOTIDA-01 / OMM_TCB / TSVET)

| 항목 | 내용 |
| --- | --- |
| 판정 | DONE_WITH_CONCERNS |
| 실행 시각 | 2026-10-19 10:39 ~ 11:11 (KST) |
| 대상 | Wafer SCKLOTIDA-01 / KLARF Step OMM_C4Bump_TCB_FullSilterra (STEP_SEQ 582) / GDS SCK · OMM_TCB (Chain 19) / E-Test TSVET (SRC_FILE_SEQ 331). 로컬 DB 조회만. 분석 / 화면은 `%TEMP%\ChippacGdsKlarfAnalysisVerification\bin` TEMP 빌드(소스 10-19 10:19 = 운영 ClientBin 10:30 VS 빌드와 같은 소스) |
| 계획 | [plans/F07DATA.plan.md](../../plans/F07DATA.plan.md) — Standard |
| 집계 | 검사 항목 47 = PASS 44 / FAIL 3 (모두 원본 데이터 또는 분석이 쓰지 않는 값, 아래 3절) / BLOCKED 0 / SKIPPED 0 |
| 최고 확인 수준 | DB 확인(원본 -> DB) / Probe 확인(분석 재계산, 화면 밖 화면 A 실행). 운영 DMS 화면은 사용자 사진 값 대조만 |

## 1. 결과 요약

1. **원본 -> DB**: KLARF / E-Test / GDS 모두 형식대로 들어갔다. 다른 값 3건은 원본 파일 자체의 문제 2건(KLARF Summary 수, E-Test 파일 안의 Wafer ID)과 분석이 쓰지 않는 KLARF 요약 행 1건이다.
2. **분석 결과**: 설계 문서 규칙으로 따로 짠 계산(제품 C# 미사용)이 Defect 1122개 · (Defect, Chain) 478쌍 · (Die, Chain) 298칸 · Die 76 · Chain 19 · 허용오차 표 · E-Test 연결 17을 **모두 같은 값**으로 냈다.
3. **화면 표시**: 화면 A 를 화면 밖에 띄워 받은 범례 · Map 76 Die · 표 7종 · Die 76개 하나씩 선택한 Chain / Defect 탭이 분석 결과와 같고, 사용자 사진 숫자(19 / 12 / 7 / 3 / 2 / 8 / 25 / Spec Out 25, Defect (472), E-Test Fail (149))와도 같다.
4. 새 Probe 5개는 덤프 사본 5곳을 일부러 바꾸면 해당 항목이 FAIL 했다(red-green).

## 2. 시나리오별 결과

| ID | 분류 | 결과 | 확인 수준 | 증거 |
| --- | --- | --- | --- | --- |
| SCN-K KLARF -> DB | 정상 | FAIL 2 / PASS 10 (원본 데이터 · 미사용 값) | DB 확인 | evidence/02_KlarfCheck.stdout.txt `KLARF_CHECK_PROBE=FAIL (10/12)` |
| SCN-E E-Test -> DB | 정상 | FAIL 1 / PASS 8 (원본 데이터) | DB 확인 | evidence/03_EtestCheck.stdout.txt `ETEST_CHECK_PROBE=FAIL (8/9)` |
| SCN-G GDS -> DB | 정상 | PASS 5/5 | DB 확인 | evidence/04_GdsCheck.stdout.txt `GDS_CHECK_PROBE=PASS (5/5)` |
| SCN-A 분석 재계산 | 정상 | PASS 9/9 | Probe 확인 | evidence/05_AnalysisCheck.stdout.txt `ANALYSIS_CHECK_PROBE=PASS (9/9)` |
| SCN-D 화면 표시 | 정상 | PASS 12/12 | Probe 확인(화면 밖 실행) | evidence/06_DisplayDump.stdout.txt `DISPLAY_DUMP=PASS`, evidence/07_DisplayCheck.stdout.txt `DISPLAY_CHECK_PROBE=PASS (12/12)`, evidence/ui/*.csv · ui_form.png |
| SCN-RG red-green | 부정 | 기대대로 FAIL 5/5 | Probe 확인 | evidence/08_redgreen_mutation.txt, 08a ~ 08e (K6 / E5 / G3 / A1·A2·A5·A7 / D3·D9·D11 FAIL) |

### 확인한 주요 값

| 구분 | 원본 / 다시 계산 | DB / 분석 / 화면 |
| --- | --- | --- |
| KLARF Wafer ID | 원본 WaferID "SCKLOTIDA", Slot 1 -> 규칙 LOT_CORE_SLOT | SCKLOTIDA-01 |
| KLARF Product | 원본 DeviceID OMMTCB, 업로드 화면 입력 우선 | OMM_TCB |
| KLARF Pitch | 원본 DiePitch 23832 x 30058 | 같음, 출처 KLARF |
| Defect | DefectList 1122 줄, 11개 열 | TQD_DEFECT 1122, 1:1 (소수 2자리) |
| Die 번호 | 원본 + (7, 5) 한 가지 이동(Map Setup 원점) | 1122건 모두 |
| 검사 Die | Test 3 SampleTestPlan 76 | 분석 IN_PLAN 76 |
| E-Test | Die 73, 값 85 x 73 = 6205 | 같음. 판정 = DB Spec 재계산, Pass 48 / Fail 25 |
| E-Test Die 좌표 | 고객 대응표 + (7, 5) | MAP_X / MAP_Y 73개 같음, 모두 검사 Die 안 |
| GDS Layer 3 / 5 / 775 | 원본 62,133 / 112,605 / 61,982 (같은 도형 한 번 = 62,133 / 111,773 / 30,992) | 62,133 / 111,773 / 30,992, 점 수도 같음 |
| Chain Member | 62,609 도형(BOUNDARY 34,728 / PATH 27,881) | 원본 요소와 nm 좌표 · 폭 · 타입 모두 같음 |
| 반전 | Chain 면 FS / 검사 면 BS / Map 저장 FRONT -> 반전 | MIRROR Y |
| Chain 영역 Defect | Gap <= 5 µm: 472 (Gap 5 ± 0.01 경계 0개) | 472, 걸린 Die 43 |
| 허용오차 표 0 / 2 / 5 / 10 / 20 / 50 | 437 / 450 / 472 / 511 / 574 / 637 Defect, Die 43 / 43 / 43 / 43 / 43 / 44 | 같음 |
| (Die, Chain) 4분류 | 일치 26 / Fail만 123 / Defect만 106 / 비교 불가 43 (연결 안 된 Chain 2개) | 같음, E-Test F 칸 149 = 탭 E-Test Fail (149) |
| Spec Out Die | e_die 판정 F 25 (= Fail Chain 1개 이상 Die 25) | 빨간 삼각형 25 |

## 3. 결함 (FAIL)

### D-01 [Low] KLARF 원본 Summary 의 Defect 수가 DefectList 줄 수와 다름 (K5b)
- 재현: `python -I -X utf8 .testagent/probes/f07data_klarf_check.py "SampleFile/26_10_14 Sample File/TEST.SINF" <evidence/db>`
- 기대 / 실제: Summary Test 3 NDEFECT = DefectList 줄 수 / NDEFECT 1098, DefectList 1122 줄(모두 TEST 3)
- 판정: 원본 데이터 — 근거: 고객 원본 `1107236219.SINF` 도 같은 값. DMS 는 DefectList 1122 줄을 모두 저장했고 분석도 1122 를 쓴다.
- 영향: 없음(DMS 결함 아님). 고객 파일 확인 대상.

### D-02 [Low] TQD_INSP_INFO Test 80 행의 DEFECTIVE_DIE 51 (원본 Test 80 은 1) (K11)
- 기대 / 실제: 원본 Test 80 Summary NDEFECT 0 / NDIE 1 / NDEFDIE 1 / DB DEFECTS 0 / INSPECTED_DIE 761 / DEFECTIVE_DIE 51 (6.70%)
- 판정: 제품 동작 의심(기존 KLARF 업로드의 검사 요약 행) — 근거: 51 = Test 3 Defect Die 수, 761 = Test 80 SampleTestPlan 수. 원인 코드는 확인하지 않았다.
- 영향: 연관 분석은 이 표를 쓰지 않는다. KLARF 요약 화면 / Chart 에서 Test 80 불량률이 6.70% 로 보일 수 있다.

### D-03 [Low] E-Test 파일 이름의 Wafer 와 파일 안 Wafer ID 가 다름 (E2)
- 기대 / 실제: DB WAFER_ID(파일 이름) = 내용 'Wafer ID' 열 / DB SCKLOTIDA-01, 내용 73행 모두 U9-HS2635-01
- 판정: 원본 데이터(시험용 이름 바꾼 복사본으로 보임) — 근거: U9-HS2635-01 원본 파일과 크기가 같고 내용의 Wafer ID 가 U9-HS2635-01 이다(바이트 비교는 파일 잠김으로 하지 않음). 파서는 파일 이름을 쓰고 내용이 다르면 경고만 한다(설계된 동작).
- 영향: 이 Wafer 의 E-Test 결과는 실제로는 U9-HS2635-01 측정값이다. 데이터 해석 시 주의.

### 테스트 결함으로 고친 것 (제품 결함 아님)
- E4: 처음 기대값을 파일 DUT Flag 수로 잡았으나 DB Pass / Fail 수는 DMS Spec 판정 수다. Spec 재계산 기대값으로 고침(첫 결과 `03_EtestCheck_v1_테스트결함E4E9.stdout.txt`).
- E9: 좌표 변환 후보에 고객 대응표가 빠져 있었다. 대응표 + (7, 5) 로 고침.
- G2 / G3: BOUNDARY 닫는 점을 빼고 셌고, Member 의 E 번호를 파일 순서로 보았다. DB 는 닫는 점을 포함하고 E 번호는 파서 정렬 순번이다. 좌표로 찾도록 고침(첫 결과 `04_GdsCheck_v1_테스트결함G2G3.stdout.txt`).

## 4. BLOCKED / SKIPPED

| ID | 사유 | 필요한 조치 (누가) |
| --- | --- | --- |
| 없음 | | |

## 5. 검증하지 않은 것

- **운영 DMS 화면 자체**: 화면 값은 같은 소스의 TEMP 빌드를 화면 밖에서 띄워 확인했고, 운영 화면(Service Remoting 경유)은 사용자 사진에 보인 숫자만 대조했다.
- **물리 정렬이 맞는지**: 반전(Chain FS / 검사 BS), 보정 (0, 0), Chip 중심 = Pitch 중심은 규칙대로 계산됨만 확인했다. 실제 Wafer 에 맞는 면 / Scribe 보정인지는 리뷰 이미지 등 고객 확인 대상(설계 18 / 20절 B23 ~ B25).
- **Chain 정의**: Member 로 고른 도형이 실제 전기 Chain 과 맞는지는 확인하지 않았다(사용자 / 고객 정의).
- **E-Test Spec 값의 타당성**: DB Spec 이 파일 Upper / Lower 와 28 항목 다르다(예: RESISTANCE 파일 0 ~ 700, DB ~ 715). 판정이 DB Spec 대로임만 확인했다.
- **GDS 미저장 Layer**: 원본 Layer 1 / 2 / 4 / 6 / 7 / 8 요소 400,250개는 저장되지 않았다(저장 시 보이는 Layer 만 저장). Chain 은 Layer 3 / 5 만 써서 이번 분석 영향은 없다. 완전히 같은 도형 제외(Layer 5 에서 832, Layer 775 TEXT 에서 30,990)는 파서 규칙(`GdsLayerList.AddElement`)이고 설계 문서에는 적혀 있지 않다.
- **모양 규칙 일부**: 이 도면 Member PATH 는 모두 Type 0(폭 45 / 25 µm), 꺾임 76곳뿐이다. PATH Type 1 / 2 / 4, Miter 2W 초과 자르기, 폭 0 PATH 는 이 데이터로 확인되지 않았다.
- **화면 B (Die 상세)** 의 표시 값, Image 탭(이미지 미업로드, IMAGECOUNT 0), 여러 Step 합치기 / FS 검사 Step / 중심 사각형 방식.
- D-02 의 원인 코드.

## 6. 다음 조치

- [ ] (사용자) D-03: 실제 SCKLOTIDA-01 E-Test 파일이 있으면 다시 올려 비교. 지금 결과는 U9-HS2635-01 측정값임을 감안.
- [ ] (사용자 / 고객) D-01: 고객 KLARF Summary NDEFECT 1098 의 의미 확인.
- [ ] (개발) D-02: KLARF 업로드 검사 요약 행(Test 80) DEFECTIVE_DIE 계산 원인 확인 — 별도 작업.
- [ ] (사용자 / 고객) 반전 / 보정이 맞는지 리뷰 이미지로 확인(설계 18 / 20절).
