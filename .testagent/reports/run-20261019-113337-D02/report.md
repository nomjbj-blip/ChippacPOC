# 검증 리포트: F08 D-02 TQD_INSP_INFO DEFECTIVE_DIE Test 별 / 정보 창 주 Test (CLAUDE-147)

실행: 2026-10-19 11:33 ~ / 대상: 로컬 docker oracle19c-DMS, STEP_SEQ 582(`SampleFile/26_10_14 Sample File/TEST.SINF`) 외 다중 Test STEP 337 / 364 / 541 / 563
판정: **DONE_WITH_CONCERNS** — 배포한 BSL 로 Product OMM_TCB 재업로드한 STEP 603 에서 K3 / K11 / K11b PASS, `KLARF_CHECK_PROBE` 12/13(남은 FAIL = 원본 파일 K5b)(5절). 남은 것: OMMTCB 중복 STEP 601 / 563 이 DB 에 남음(Data Maint 삭제 미반영), DMS 화면 확인.

## 1. 바꾼 것

| 파일 | 내용 |
| --- | --- |
| `DMS/02_DMS/DACrux.SEMDMS.BSL/KlarfUpload.cs` | Step 08 DEFECTIVE_DIE = 그 Test Defect 의 (XINDEX, YINDEX) 수. Step 07 끝 STEP 전체 Die 수 조회(`GetDefectDataCount`) 삭제 |
| `DMS/08_DBQuery/ORACLE/TQD_INSP_INFO.xml` | `GET_INSP_INFO_MULTI_SEQ` RN 순서 `B.MAPID, A.DEFECTS DESC NULLS LAST, A.TEST` (주 Test) + 이유 주석 |
| `.testagent/probes/f07data_klarf_check.py` | K11 = 정보 창 행이 주 Test 이고 값이 그 Test 의 DefectList 기준 / K11b 신규 = Test 별 행 |
| `.testagent/probes/f07data_insp_info_dump.ps1` (신규) | `k_step_info.csv`(GetStepInfo) / `k_insp_info_tests.csv`(Test 별 행) 덤프, SELECT 만 |

## 2. 실행과 결과

| 단계 | 명령 / 대상 | 결과 | 증거 |
| --- | --- | --- | --- |
| 빌드 | MSBuild BSL, OutputPath `%TEMP%\ChippacBuild\CLAUDE-147`, BuildProjectReferences=false, 참조 DLL 은 101_ServerBin 복사 | 오류 0 / 경고 0, TEMP BSL md5 6a7be68c (운영 0feae8f7 그대로) | 대화 기록 |
| 덤프 | `f07data_insp_info_dump.ps1 -StepSeq 582` | `INSP_INFO_DUMP=OK (k_step_info 1행 / k_insp_info_tests 2행)` | `evidence/01_InspInfoDump.stdout.txt`, `evidence/db/k_step_info.csv`, `k_insp_info_tests.csv` |
| KLARF Probe | `f07data_klarf_check.py TEST.SINF evidence/db` (나머지 CSV 는 run-20261019-103914-F07DATA 복사) | `KLARF_CHECK_PROBE=FAIL (11/13)` — K11 PASS, K11b FAIL(예상), K5b FAIL(원본 파일 D-01, 기존과 같음) | `evidence/02_KlarfCheck.stdout.txt` |
| 정보 창 행 | GetStepInfo(SELECT) 337 / 364 / 541 / 563 / 582 | 모두 Defect 최다 Test: 337 = Test 1(이전 Test 2), 364 = Test 1, 541 / 563 / 582 = Test 3 (1122 / 76 / 51, 67.11%) | 대화 기록 |

- K11 PASS: 정보 창 행 TEST 3 / 1122 / 76 / 51 = 원본 Test 3 DefectList 1122 줄 / SampleTestPlan 76 / Defect Die 51. 변경 전 같은 검사는 Test 80 행(0 / 761 / 51, 6.70%)이었다(run-20261019-103914-F07DATA).
- K11b FAIL: DB Test 80 행 DEFECTIVE_DIE 51 vs 기대 0. 이 행은 변경 전 BSL 로 저장된 데이터라 재업로드 전에는 FAIL 이 맞다(red). 재업로드 후 PASS 여야 한다.

## 3. 검증하지 않은 것

- 운영 DMS 화면(Defect Map 정보 창 / Chart / 왼쪽 Step List) 표시.
- STEP 541 / 563(`1107236219.SINF`) 재업로드 결과.
- DataService(기존 자동 적재) 경로: 바꾸지 않음. STEP 337 / 364 의 DEFECTIVE_DIE 는 STEP 전체 값 그대로.

## 4. 배포 후 재업로드 확인 (13:02)

- 상태: 운영 `101_ServerBin/DACrux.SEMDMS.BSL.dll` 11:43:53 md5 bd371122(사용자 VS 빌드). DACrux.SEMDMS.Service / DACruxV5 13:01:20 시작. 사용자가 KLARF 업로드 화면에서 TEST.SINF 저장(SAVED).
  - 12:59 첫 확인 때는 DB 에 새 STEP 이 없고 서비스 프로세스도 없었다(사용자가 서비스를 띄운 뒤 다시 저장).
- 새 STEP: 601(SCKLOTIDA-01, PRODUCT **OMMTCB**). Test 3 = 1122 / 76 / 51, Test 80 = 0 / 761 / **0**.
- 명령: `f07data_insp_info_dump.ps1 -StepSeq 601 -OutDir evidence/db_step601`(나머지 CSV 는 run-20261019-103914-F07DATA 복사) -> `f07data_klarf_check.py TEST.SINF evidence/db_step601`
- 결과: `KLARF_CHECK_PROBE=FAIL (11/13)` — **K11 PASS / K11b PASS**, K3 FAIL(PRODUCT OMMTCB, 기대 OMM_TCB = 업로드 화면 입력값 차이), K5b FAIL(원본 파일 D-01). 증거 `evidence/03_InspInfoDump_step601.stdout.txt`, `evidence/04_KlarfCheck_step601.stdout.txt`.
  - K4 ~ K10 은 STEP 582 덤프 CSV(Defect / 분석 표)를 그대로 쓴 결과다. 601 의 Defect 행은 이번에 다시 덤프하지 않았다.
- DB 전체(SELECT): 다중 Test 업로드 STEP 4개 중 601 만 Test 80 행이 맞고, 예전 BSL 로 저장된 541 / 563 / 582 는 여전히 51.
- 582 가 남은 이유: 업로드의 기존 STEP 찾기(`KlarfUpload.FindExistingStep`)는 Lot + **Product** + Wafer + Step + 검사 시각으로 찾는다. Product 를 OMMTCB 로 넣어 다른 Lot 으로 보고 새로 등록했다(설계된 동작). SCKLOTIDA-01 에 KLARF STEP 이 582(OMM_TCB) / 601(OMMTCB) 2개다.
  - 업로드 화면은 파일을 고르면 Product 칸을 KLARF DeviceID(TEST.SINF = OMMTCB)로 자동으로 채운다(`frmKlarfUpload.cs:142-144`).
- 16:04 확인: DB 변화 없음(사용자 재저장 / 601 삭제가 반영되지 않음, DMS 16:03:20 재시작). Probe 실행 안 함.

## 5. Product OMM_TCB 재업로드 확인 (17:15)

- 사용자: Product 를 OMM_TCB 로 고쳐 TEST.SINF 저장(582 삭제 후 재등록, SAVED) + Data Maint 에서 601 삭제.
- DB(SELECT): 582 / 541 은 STEP / INSP_INFO / DEFECT / IMAGES / INSP_SUM 0행(삭제됨). 새 STEP **603**(SCKLOTIDA-01, OMM_TCB, 17:15:09) / **602**(PFIC_U9-FS2635_C4BUMP-01, `1107236219.SINF`, OMM_TCB, 17:14:36 = 541 재업로드) 모두 Test 80 행 = 0 / 761 / 0. **601 은 그대로**(STEP 1 / INSP_INFO 2 / DEFECT 1122 / INSP_SUM 1행).
- 명령: `f07data_insp_info_dump.ps1 -StepSeq 603 -OutDir evidence/db_step603` -> `f07data_klarf_check.py TEST.SINF evidence/db_step603`
- 결과: `KLARF_CHECK_PROBE=FAIL (12/13)` — **K3 / K11 / K11b PASS**, K5b FAIL(원본 파일 D-01). 증거 `evidence/05_InspInfoDump_step603.stdout.txt`, `evidence/06_KlarfCheck_step603.stdout.txt`. K4 ~ K10 은 STEP 582 덤프 CSV 를 쓴 결과(603 Defect 행은 1122 건 저장만 확인).
- DB 전체: 업로드 다중 Test STEP 563 / 601 / 602 / 603 중 Test 80 행이 예전 값(51)인 것은 563(OMMTCB, 10-15 등록) 하나. 정보 창 행은 6 STEP 모두 주 Test(Test 1 또는 3). DataService 337 / 364 는 그대로.
- 남은 중복: SCKLOTIDA-01 = 603(OMM_TCB) + 601(OMMTCB), PFIC_U9-FS2635_C4BUMP-01 = 602(OMM_TCB) + 563(OMMTCB). 사용자 결정으로 지금은 그대로 둔다(정리는 나중).
