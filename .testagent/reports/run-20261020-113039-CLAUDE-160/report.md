# 검증 리포트 — CLAUDE-160 E-Test 목록 조건 (Wafer ID · Device · Step)

| 항목 | 내용 |
| --- | --- |
| 판정 | DONE |
| 실행 시각 | 2026-10-20 11:16 ~ 11:40 (KST) |
| 대상 | BSL `GdsKlarfReady.cs`(E-Test Device 조건 + 점검 `ET_OTHER_DEVICE`), ENGUI `frmGdsKlarfAnalysis`([E-Test] 탭 Step 콤보), `frmGdsKlarfSourceFind`(문구) |
| 계획 | 별도 plan 파일 없음(이 리포트 2절이 시나리오) — Standard |
| 집계 | PASS 11 / FAIL 0 / BLOCKED 0 / SKIPPED 0 / 미실행 0 |
| 최고 확인 수준 | Probe 확인(로컬 DB 실제 데이터 조회 + PROBE 시험 데이터) + 화면 그림(오프스크린 실행). DMS 화면 확인 대기 |

## 1. 결과 요약

- 서버 Ready 의 E-Test 목록은 Wafer Key 가 같고 Device 가 분석 GDS Device 와 같은 파일만 담는다. Device 가 다른 같은 Wafer 파일은 점검 정보로만 남는다.
- 화면 [E-Test] 탭은 Step 콤보로 Step 을 고르면 그 Step 파일만 보이고, 체크는 1개다. 파일이 없으면 콤보가 꺼진다.
- 실제 데이터: STEP 528(OMM_Hybrid) / 603(OMM_TCB)은 E-Test 0개(Device 다름 1개씩 점검 정보), STEP 601(OMMTCB)은 331 1개(선택 Y).

## 2. 시나리오별 결과

| ID | 분류 | 결과 | 확인 수준 | 증거 |
| --- | --- | --- | --- | --- |
| S1 | 정상 | PASS — Probe T7: PROBE 시험 파일(OMM_Hybrid / TSVET / RH-HS2615-02)을 같은 Device 로 조회하면 목록 1행, 선택 Y, 점검 `ET_OTHER_DEVICE` 없음 | Probe | `evidence/probe_run3_final.log` |
| S2 | 부정 | PASS — Probe T7-1: 다른 Device(PROBE_OTHER_DEVICE)로 조회하면 목록 0행 + 점검 정보 "… 뺌(OMM_Hybrid TSVET)" | Probe | 같은 파일 |
| S3 | 경계 | PASS — Probe T7-1: Device 빈 값이면 Product 이름(OMM_Hybrid)을 분석 Device 로 써서 1행 | Probe | 같은 파일 |
| S4 | 정상(실데이터) | PASS — Probe R2: STEP 528 E-Test 0개, 점검 `ET_OTHER_DEVICE` | Probe(DB 조회) | 같은 파일 |
| S5 | 정상(실데이터) | PASS — 최종 BSL 직접 조회: 601 -> `331:OMMTCB:TSVET:Y`, 603 -> 0개 + "OMMTCB TSVET" 뺌, 563 / 602 -> 0개(Wafer Key 다름, 전과 같음) | DB 조회 | `evidence/ready_real_steps.log` |
| S6 | 정상(화면) | PASS — Smoke "E-Test Step 콤보": Step = TSVET, COWET / TSVET 표 [9001*, 9003] / COWET [9002*] / 다시 TSVET [9001*, 9003] / 9003 체크하면 9001 꺼짐, 열 머리 "Step" | 오프스크린 화면 | `evidence/smoke_run2_green.log` |
| S7 | 경계(화면) | PASS — Smoke "E-Test 파일 없음": 콤보 꺼짐 / Step 0 / 표 0행 / 분석 E-Test 0 | 오프스크린 화면 | 같은 파일 |
| S8 | 정상(화면) | PASS — Smoke "목록 = 분석 GDS Device 파일만 / 상태줄": STEP 528 상태줄 끝 "E-Test 1개는 Device 가 GDS Device OMM_Hybrid 와 달라 목록에서 뺌(OMM TSVET)", "[" 없음 | 오프스크린 화면 | 같은 파일 |
| S9 | 외관 | PASS — Smoke "머리 줄 배치": 제목 4~245 < Step 255~286 < 콤보 290~382 < [직접 찾기] 522 | 오프스크린 화면 | 같은 파일 |
| S10 | 회귀 | PASS — Smoke 전체 75/75(기존 71 + 새 4), Probe 전체 18/18(기존 17 + 새 1) | Probe / 오프스크린 화면 | 같은 파일들 |
| S11 | 외관 | PASS — 미리보기 12장, A3 그림에서 제목 · Step 콤보 · [직접 찾기] 한 줄, 표 = TSVET 2행 | 그림 | `evidence/A3_ETest탭.png`, `evidence/preview.log` |

- red-green:
  - S2: 같은 Probe 를 운영 폴더의 예전 BSL(md5 6b71d3eb)로 바꿔 실행하면 T7-1 FAIL("다른 Device 1행 / 점검 없음"). R2 도 예전에는 "E-Test 1개"(`evidence/probe_red_old_bsl.log`).
  - S9: 처음 고정 위치(x 330)로 만든 그림에서 제목 끝 "선택"이 Step 라벨에 가려진 것을 보고 실행 중 자리 잡기로 고친 뒤 검사를 넣음.
- 빌드: 경고 Control 12 / ENGUI 4 / GdsMap 0(기존과 같은 Control / ENGUI 수). TEMP md5 BSL 2c810bcb / ENGUI bd786962(Smoke 빌드), Probe 빌드 BSL 60483d8d(같은 소스, 빌드 폴더만 다름).

## 3. 결함 (FAIL)

- 제품 결함: 미리보기 그림에서 제목과 Step 라벨이 겹침(Low, 외관) -> 고침(S9).
- 테스트 결함: 첫 Probe 실행 T7-1 이 "Device 빈 값 = 0행" 을 기대(`evidence/probe_run1_test_defect.log`). 서버는 Device 가 비면 Product 이름을 분석 Device 로 쓰므로(`ResolveMapDevice`) 1행이 규칙대로다 -> 기대값을 고침(S3).
- 테스트 결함: Smoke 지역 변수 이름 충돌(CS0136) -> 이름 변경.

## 4. BLOCKED / SKIPPED

| ID | 사유 | 필요한 조치 (누가) |
| --- | --- | --- |
| - | 없음 | - |

## 5. 검증하지 않은 것

- DMS 화면(Service Remoting 경유) 확인. 배포 안 함(BSL `101_ServerBin` + ENGUI `100_ClientBin` 함께 필요, 사용자 VS 빌드 / 배포).
- [직접 찾기]로 다른 Device 파일을 더해 분석하는 흐름(창 Device 기본값 = GDS Device, 더한 파일 Step 으로 콤보 바뀜)은 코드만 바꿨고 Smoke 로 누르지 않았다. 미리보기 A6 는 창 모양만 확인.
- Smoke 의 Step 콤보 검사는 예전 화면에 콤보가 없어 red 실행을 하지 않았다(기대값은 손으로 정한 리터럴).
- 150% 배율 실제 화면에서의 머리 줄 배치(오프스크린 실행 배율에서만 확인).

## 6. 다음 조치

- 사용자: VS 에서 BSL + ENGUI 빌드 / 배포 -> SEMDMS Service · DACrux 다시 시작 -> DMSearch 에서 STEP 601(OMMTCB) 고르고 [E-Test] 탭에 Step [TSVET] + 331 1행 체크, STEP 603(OMM_TCB) 이면 표 비고 상태줄 끝에 "Device 가 … 달라 목록에서 뺌(OMMTCB TSVET)".
