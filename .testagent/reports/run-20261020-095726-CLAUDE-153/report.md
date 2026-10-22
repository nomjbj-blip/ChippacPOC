# 검증 리포트 — CLAUDE-153 화면 A Wafer Map E-Test 표식 2종

| 항목 | 내용 |
| --- | --- |
| 판정 | DONE |
| 실행 시각 | 2026-10-20 09:57 ~ 10:05 (KST) |
| 대상 | ENGUI `DefectMapAnalysis/GdsKlarfAnalysisMap.cs` · `frmGdsKlarfAnalysis.cs` · `.Designer.cs`. TEMP 빌드 `%TEMP%\ChippacGdsKlarfAnalysisVerification\bin`(ENGUI md5 6b87fa6b). 운영 폴더 배포 안 함 |
| 계획 | 설계 `문서/설계/05_연관분석/2026-10-20_Map_ETest_표식_설계.md` 5 · 6절(안 1, E1 ~ E4 권장안) — Standard |
| 집계 | 시나리오 2 = PASS 2 / FAIL 0 / BLOCKED 0 / SKIPPED 0 (red-green 1 은 기대대로 FAIL) |
| 최고 확인 수준 | Probe 확인(TEMP 빌드 화면을 화면 밖에서 실제 로컬 데이터로 띄워 값 대조 + Map / 팝업 그림 직접 확인). DMS(DACrux) 화면 확인 대기 |

## 1. 결과 요약

Spec Out(DMS Spec 판정 F) = Die 왼쪽 위 빨간 삼각형, Fail Die(결과 파일 DUT Flag F) = 오른쪽 아래 작은 남색 삼각형을 함께 그리고, [E-Test] 탭 체크 2개로 따로 끈다.
화면 Smoke 70/70(라디오 검사 -> 체크 2개 검사로 바꿈), 실제 데이터 SCKLOTIDA-01(STEP 603 / E-Test 331) 대조 5/5: Spec Out 25 / Fail Die 73 = 결과 DIE 표 = 2026-10-19 검증 리터럴.

## 2. 시나리오별 결과

| ID | 분류 | 결과 | 확인 수준 | 증거 |
| --- | --- | --- | --- | --- |
| SCN-S 화면 A + B Smoke(로컬 STEP 528, E-Test 없는 결과에 예시 판정을 넣은 복사본) | 정상 / 회귀 | PASS 70/70 | Probe 확인 | evidence/01_VerifyGdsKlarfAnalysis.stdout.txt `GDS_KLARF_FORM_SMOKE=PASS (70/70)` |
| SCN-M 실제 데이터 표식(SCKLOTIDA-01 / OMM_TCB / STEP 603 / E-Test 331) | 정상 / 경계(끄기) | PASS 5/5 | Probe 확인 + 그림 | evidence/02_MarkDump.stdout.txt `MARK_DUMP=PASS`, evidence/03_MarkCheck.stdout.txt `MARK_CHECK_PROBE=PASS (5/5)`, evidence/ui/m_map_*.png, evidence/band_fail_mark.png |
| RG 기준값 틀리게(Spec Out 24) | 부정 | 기대대로 FAIL | Probe 확인 | evidence/04_RG_MarkCheck_wrongLiteral.stdout.txt `M2 FAIL` |

실제 데이터 항목: M1 체크 2개 문구 · 기본 켬 / M2 표식 Die 집합 = DIE.ET_STATE F 25 · ET_FILE_FLAG F 73 / M3 팝업 표 끝 2행 / M4 끄고 켜기 4단계 / M5 상태줄 "E-Test Spec Out (Fail Chain 4, 일치 3) · 결과 파일 Fail Die", "Spec 통과 (…) · 결과 파일 Fail Die", E-Test 없는 Die 는 E-Test 문구 없음.

## 3. 결함 (FAIL)

없음. 작업 중 고친 것:
- 화면(고침): 색 구간 팝업 FROM 칸에서 "Fail Die (결과 파일)" 이 잘림 -> FROM 칸 폭 비율 늘림(그림 확인).
- 테스트 결함(고침): M5 가 상태줄 첫 줄(분석 기준 " / E-Test <Wafer> <공정>")까지 봐서 E-Test 없는 Die 를 FAIL -> Die 줄만 봄. 첫 결과 `03_MarkCheck_v1_테스트결함M5.stdout.txt`.

## 4. BLOCKED / SKIPPED

| ID | 사유 | 필요한 조치 (누가) |
| --- | --- | --- |
| 없음 | | |

## 5. 검증하지 않은 것

- DMS(DACrux) 안 화면: TEMP 빌드를 화면 밖에서 띄워 확인했다. 운영 `100_ClientBin` 배포는 하지 않았다.
- 진한 구간 색(#C70039 / #900C3F) 위 빨간 삼각형은 원래도 대비가 약하다. 흰 1px 테두리를 더했지만 작은 Die(확대 전)에서는 여전히 약하게 보인다(그림 A1).
- 결과 파일 Fail 이 대부분인 이 Wafer 외에, Spec Out 만(결과 파일 Pass) 인 Die 가 있는 실제 데이터는 로컬에 없어 Smoke 예시 값으로만 확인했다.
- 작업 시작 때 다른 세션 CLAUDE-152 가 같은 Smoke 파일을 고치는 중이라 그 세션 완료(Smoke 70/70) 뒤에 Smoke 를 고쳤다.

## 6. 다음 조치

- [ ] (사용자) VS 로 `DACrux.SEMDMS.ENGUI` 빌드 -> `100_ClientBin` 반영 -> DACrux 다시 시작 -> 연관 분석 [분석] -> Map 모서리 표식 / [E-Test] 탭 체크 / [색 구간 설정] 팝업 확인. (CLAUDE-149 / 150 의 BSL + ENGUI 와 함께 배포)
