# 검증 리포트 — CLAUDE-218 POC 사용 기한(2027-01-31)

| 항목 | 내용 |
| --- | --- |
| 판정 | **DONE_WITH_CONCERNS** |
| 실행 시각 | 2026-10-29 14:59 ~ 15:10 (KST) |
| 대상 | 소스(커밋 `440af08`, 이후 대상 파일 변경 없음) + 운영 폴더 `101_ServerBin` / `100_ClientBin`(10-27 16:52 빌드) |
| 계획 | [plans/CLAUDE-218.plan.md](../../plans/CLAUDE-218.plan.md) — Standard |
| 집계 | PASS 6 / FAIL 0 / BLOCKED 0 / SKIPPED 0 / 미실행 0 (+ 화면 확인 대기 1) |
| 최고 확인 수준 | Probe 확인(운영 ServerBin DLL + 로컬 DB 조회) |

## 1. 결과 요약

- 기한 확인은 소스와 운영 폴더 모두에 들어 있다. 운영 ServerBin DSL 의 `WorkRule.LastWorkDay` = 2027-01-31, 확인 호출 31곳(SEMDMS.BSL 27 + TEST.BSL 4), ClientBin 에 메뉴 안내(`MenuWorkRule`) / `GetWorkState`.
- 운영 DLL 로 Probe: 기한 안 PASS (10) — DB 한국 시간 = PC 시각, `WORK_OPEN=Y DAYS_LEFT=95`. 마지막 날을 어제로 바꾼 TEMP DSL + 운영 BSL 로 PASS (11) — 31개 저장 함수 모두 첫 줄에서 막히고 조회는 정상. DB 일시를 못 읽으면 막음(PASS).
- 우려: 원격 호출에서 빠진 서버 내부 함수 `GdsEtResult.RemapEtFiles`(Map 좌표 다시 계산)에 기한 확인이 없다(화면 경로 없음, Probe 만 사용). 검증 중 Probe 안전장치 결함을 발견해 고침(아래 3절, DB 변경 없음 확인).

## 2. 시나리오별 결과
| ID | 분류 | 결과 | 확인 수준 | 증거 |
| --- | --- | --- | --- | --- |
| SCN-01 | 정상 | PASS | 정적 검토 + Probe 확인 | `LastWorkDay = new DateTime(2027, 1, 31)`, DSL / DACruxV5 csproj 등록 각 1, `MenuWorkRule.Attach` 2곳, MenuFile(소스 / ClientBin) `MNU_POC_SETUP` · `MNU_POC_UPLOAD` 각 1. `evidence/01_WorkRule_ServerBin.stdout.txt` 소스 31 / 31, `DeleteEtFile` · `GdsMapView` 확인 없음 |
| SCN-02 | 부정 | PASS (우려 1) | 정적 검토 | POC 서비스 public 쓰기 이름 함수 스캔: Confirm 있음 24(+ SEMConfiguration 3 · ProbeAdmin 4 = 31), 없음 = Cancel 2 / 배치 중간 5 / AddImage / DeleteEtFile(설계상 제외) / 보조 함수 3 / **`RemapEtFiles`** |
| SCN-03 | 정상 | PASS | Probe 확인 | `01_WorkRule_ServerBin` `WORK_RULE_PROBE=PASS (10)`, DB 2026-10-29 15:00:57 = PC 15:00:57, `WORK_OPEN=Y LAST_DAY=2027-01-31 DAYS_LEFT=95`, Confirm 예외 없음. 재실행 `07_WorkRule_ServerBin_After` PASS (10) |
| SCN-04 | 경계 | PASS | Probe 확인 | 2026-09-30 23:59:59 차단 / 10-01 00:00:00 통과 / 2027-01-31 23:59:59 통과 / 02-01 00:00:00 차단, 남은 일수 1 / 0 / 14 |
| SCN-05 | 부정 | PASS | Probe 확인 | TEMP DSL(`LastWorkDay = 2026-10-28`, 저장소 / 운영 DSL `e7c3035a` 그대로) + 운영 BSL: `02_WorkRule_Expired` · `06_WorkRule_Expired_Guarded` PASS (11) — 31 / 31 "POC 사용 기간(2026-10-28 까지)이 지나 ...", 조회 Device 10건. red: `05_WorkRule_RedCheck_Guarded` 정상 빌드에 기한 지남 기대 → FAIL (4 / 10), 저장 함수 호출 안 함 |
| SCN-06 | 예외 | PASS | Probe 확인 | `.testagent/probes/WorkRuleDbFailCheck.ps1`(config 사본 `DMS_CONNECT_ID` 만 변경) `04_WorkRule_DbFail` PASS — `InvalidOperationException: 서버 일시를 확인할 수 없어 저장할 수 없습니다. (...)` |
| SCN-07 | 정상 | PASS | 정적 검토(리플렉션) | 운영 ServerBin(10-27 16:52) DSL `WorkRule` 2027-01-31 / Interface · BSL `GetWorkState` / Confirm 호출 SEMDMS.BSL 27 · TEST.BSL 4. ClientBin Interface · RO `GetWorkState`, `DACruxV5.MenuWorkRule`. MD5: Interface ca9bb30f(Server = Client) / DSL e7c3035a / BSL 7cde8585 / TEST.BSL 55d360d5 / RO 0ff03957 / DACruxV5 b41db0a9 |
| 화면 | 정상 | 화면 확인 대기 | - | 메뉴 / 제목 / 오류 창 문장은 DMS 실행 필요(사용자) |

## 3. 결함 (FAIL)

검증 대상(제품)의 FAIL 은 없다. 아래는 검증 중 발견한 테스트 결함과 제품 우려다.

1. **테스트 결함 — Probe 안전장치 없음 (고침)**
   - 재현: `WorkRuleProbe.ps1 -BslDir 101_ServerBin -ExpectClosed`(기한 안 빌드에 기한 지남 기대, red 확인용) → `evidence/03_WorkRule_RedCheck.stdout.txt`.
   - 실제: Confirm 이 통과해 31개 저장 함수가 빈 인자로 본문까지 실행됨. 대부분 입력 검사에서 멈췄으나 `SEMConfiguration.DeleteDefectType(null)` 과 `ProbeAdmin.UpdateMapDef(빈 값)` 은 DB 문까지 감.
   - 판정: 테스트 결함. 두 SQL 은 `WHERE CLASSNUMBER = :classnumber` / `WHERE MAPID = :mapid` 라 NULL 과 일치하는 행이 없다. DB 확인: TQP_MAPDEF 최근 1시간 수정 0건(마지막 수정 2019), MAPID NULL 0건, TQD_DEFECT_TYPE 75건 · CLASSNUMBER NULL 0건 → **변경 없음**.
   - 조치: Probe 4단계에 "WORK_OPEN=N 일 때만 저장 함수 호출" 안전장치 추가, red 재실행 `05` 에서 호출 없이 FAIL 확인, `06` 기한 지남 PASS 재확인. 함정 목록(gotchas)에 추가.
2. **제품 우려 — `GdsEtResult.RemapEtFiles` 기한 확인 없음** (심각도 Low)
   - 2026-10-15 화면 기능과 원격 호출 계약(`iGdsEtResult`)에서 빠졌고 지금은 서버 Probe 만 BSL 을 직접 부른다. 제품 화면 / 원격 경로로는 호출할 수 없다.
   - 그래도 저장(Map 좌표 / YMS 다시 반영)을 하는 public 함수라, 원칙("저장 함수 첫 줄 확인")대로라면 1줄 추가 대상. 사용자 결정 필요.

## 4. BLOCKED / SKIPPED
| ID | 사유 | 필요한 조치 (누가) |
| --- | --- | --- |
| - | 없음 | - |

## 5. 검증하지 않은 것

- DMS 화면: 기한 안 메뉴 그대로 / 저장 정상, 기한 후 `Setup` · `Data Upload` 묶음 비활성 · 제목 문장 · 화면 오류 창 문장(사용자 화면 확인).
- 서비스(Remoting) 경유 호출: 이번 Probe 는 BSL 을 같은 프로세스에서 직접 불렀다. 서비스 config 의 `DMS_CONNECT_ID`(SEMDMS / TEST 모두 DMSMGR, 10-23 확인)와 같은 설정 사본을 썼다.
- 기한 안에서 각 저장 기능의 실제 저장 회귀(기존 기능 Probe) — 첫 줄이 통과만 하므로 동작 변화는 없다고 판단했으나 이번에 다시 돌리지 않았다.
- `FIXED_DATE` 영향(DB 설정 변경이 필요해 하지 않음).
- 운영 폴더 DLL 이 커밋 `440af08` 과 같은 소스로 빌드됐는지 바이트 단위 대조는 못 함(빌드마다 MD5 가 다름). 리플렉션으로 기한 상수 / 확인 호출 수 / 새 함수 존재만 확인.

## 6. 다음 조치

1. (사용자) DMS 에서 저장 몇 가지(E-Test Para Setup / GDS Chain 저장 등)가 평소처럼 되는지 확인 → 화면 확인.
2. (사용자 결정) `RemapEtFiles` 첫 줄 확인 추가 여부.
3. Probe 안전장치 수정분(`WorkRuleProbe.ps1`)은 미커밋 — 다음 커밋에 포함.
