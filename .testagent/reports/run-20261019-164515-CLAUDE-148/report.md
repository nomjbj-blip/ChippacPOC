# 검증 리포트 — CLAUDE-148 상세보기(화면 B) 기능 탭 배치

| 항목 | 내용 |
| --- | --- |
| 판정 | DONE |
| 실행 시각 | 2026-10-19 16:45 ~ 16:56 (KST) |
| 대상 | GdsMap `06_CoreLibSource/GdsMapControl/GdsDieAnalysisForm.cs` · `.Designer.cs`. TEMP 빌드 `%TEMP%\ChippacGdsKlarfAnalysisVerification\bin`(GdsMap md5 09c2ed9a). 운영 폴더 배포 안 함 |
| 계획 | 설계 `문서/설계/05_연관분석/2026-10-19_화면A_레이아웃_후보.md` 10절 (배치 10.1 / 문구 · 용어 10.2) — Standard |
| 집계 | 시나리오 3 = PASS 3 / FAIL 0 / BLOCKED 0 / SKIPPED 0 (red-green 1 은 기대대로 FAIL) |
| 최고 확인 수준 | Probe 확인 (TEMP 빌드 화면을 실제 로컬 데이터로 띄워 값 대조 + 화면 그림 직접 확인). DMS(DACrux) 화면 확인 대기 |

## 1. 결과 요약

상세보기를 화면 A 와 같은 구조(위 = 분석 기준 요약 3칸 + Die 줄, 왼쪽 = GDS 도면, 오른쪽 = 기능 탭 GDS / KLARF / E-Test / 종합 (Die))로 바꿨다.
화면 Smoke 68/68(기존 61 + 화면 B 새 검사 4 + 여러 창 검사 갱신), 실제 데이터(SCKLOTIDA-01 / STEP 582 / OMM_TCB / Die (4, 6), E-Test 있음) 대조 9/9.
탭을 처음 열 때 표가 첫 행을 저절로 골라 도면이 확대되던 문제를 그림으로 찾아 고쳤고, 고치기 전 빌드에서 그 검사(B7)가 FAIL 하는 것을 확인했다.

## 2. 시나리오별 결과

| ID | 분류 | 결과 | 확인 수준 | 증거 |
| --- | --- | --- | --- | --- |
| SCN-S 화면 A + B Smoke(로컬 STEP 528 / OMM_Hybrid, E-Test 없음) | 정상 / 회귀 | PASS 68/68 | Probe 확인 | evidence/01_VerifyGdsKlarfAnalysis.stdout.txt `GDS_KLARF_FORM_SMOKE=PASS (68/68)` |
| SCN-B 상세보기 실제 데이터(STEP 582 / OMM_TCB / Die (4, 6), E-Test TSVET) | 정상 | PASS 9/9 | Probe 확인 + 화면 그림 | evidence/02_DetailDump.stdout.txt `DETAIL_DUMP=PASS`, evidence/03_DetailCheck.stdout.txt `DETAIL_CHECK_PROBE=PASS (9/9)`, evidence/ui/B_*.png |
| RG 탭 전환 자동 선택 검사 red | 부정 | 기대대로 FAIL (고치기 전 빌드) | Probe 확인 | evidence/04_RG_DetailCheck_beforeFix.stdout.txt `B7 FAIL`(KLARF 탭을 열자 Defect #442 선택 · 상태줄 바뀜, E-Test 탭 첫 행 선택) |

새 Smoke 검사(화면 B): 배치(기능 탭 / 표 위치 / 기준 제목 / 분석 기준 3칸 문구 / 도면 머리 보기 면), 용어(Chain 표 열 · 분류 '-' 없음 · Defect ID · RoughBin), [종합 (Die)](4분류 수 합 = Chain 수 / 표 행 / Die 요약), 첫 탭(Die 전체 = 종합 / 역추적 = KLARF + "(역추적)").

실제 데이터 대조 항목(B0 ~ B7): 상세 결과 = Wafer 결과 / 배치 · 문구 / [KLARF] · [E-Test] 기준 / [GDS] Chain 19행 / [KLARF] Defect 50행 / [E-Test] 값 85행 / [종합] 요약 · 4분류 수(일치 2 · Fail만 2 · Defect만 3 · 정상 10 · 비교 불가 2) · 표 7행 / 역추적으로 열기 / 탭 전환 자동 선택 없음.

## 3. 결함 (FAIL)

없음. 아래는 작업 중 찾아 고친 것.

- 화면 결함(고침): 숨은 탭의 표가 처음 보일 때 첫 행을 저절로 골라 Defect / E-Test 값 행 선택 처리가 돌았다(도면 확대 · 상태줄 바뀜). 탭 전환 동안 선택 처리를 건너뛰고, 전환 전 고른 행이 없던 표는 선택을 지운다(`tabMain_Selecting` / `tabMain_SelectedIndexChanged`). red-green: RG FAIL -> SCN-B B7 PASS.
- 화면 결함(고침): 150% 화면에서 기준 제목이 기준 문구 첫 줄 왼쪽을 가렸다 -> 제목 / 문구 간격과 묶음 높이 조정. [종합 (Die)] 표 머리글이 세로로 꺾임 -> 머리글 한 줄.
- 테스트 결함(고침): Smoke "새 창은 앞 창 Chain 고른 행 · 스크롤 이어받지 않음" 이 숨은 탭 표의 첫 표시 행 -1 을 스크롤로 봐서 FAIL(선택 0) -> -1 도 스크롤 없음. 실제 데이터 대조 B0 / B1b / B2 는 Wafer 결과를 기대값으로 써서 FAIL(상세 결과는 설계 21.2절대로 연결 Chain 전부를 보냄 · 분석 시각은 다시 분석한 시각) -> 상세 결과 기준(B0 에서 Wafer 결과와 같음 확인). 처음 결과는 evidence 에 `_v1` ~ `_v3` 이름으로 남김.
- 확인 도구 결함(고침): 화면 복사가 화면 배율을 무시해 다른 창을 찍음 -> Preview 와 같은 배율 계산.

## 4. BLOCKED / SKIPPED

| ID | 사유 | 필요한 조치 (누가) |
| --- | --- | --- |
| 없음 | | |

## 5. 검증하지 않은 것

- DMS(DACrux) 안 상세보기 창(호스트 `frmGdsDieAnalysis`, MDI 탭)에서 사람이 보는 화면: TEMP 빌드를 따로 띄워 확인했다. 운영 `100_ClientBin/GdsMap` 배포는 하지 않았다.
- 화면 A E-Test Fail 탭 더블클릭으로 역추적 상세를 여는 실제 경로는 Smoke 의 여러 창 검사(OpenDetail) 수준만 확인했다. 처음 탭 = [KLARF] 는 ShowDie 직접 호출로 확인.
- Image 보기(Defect 더블클릭 -> 호스트 FTP 이미지)는 이번 변경과 무관해 다시 확인하지 않았다.
- 1920 보다 좁은 화면 / 100% 배율 화면의 줄바꿈(그림은 150% 배율에서 찍음).

## 6. 다음 조치

- [ ] (사용자) VS 로 `NexplantQMS.GdsMap` 빌드 -> `100_ClientBin/GdsMap` 반영 -> DACrux 다시 시작 -> 연관 분석 [분석] -> Die 더블클릭(상세보기) 화면 확인.
