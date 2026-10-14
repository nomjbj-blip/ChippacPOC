# 검증 리포트 — CLAUDE-070 GDS 조회 우클릭 전체 보기 제거

| 항목 | 내용 |
| --- | --- |
| 판정 | **BLOCKED** |
| 실행 시각 | 2026-10-13 11:24 ~ 11:27 (KST) |
| 대상 | `DMS/06_CoreLibSource/GdsMapControl/GdsMapViewForm.Designer.cs` / `GdsMapViewForm_Layout.cs` / `Probes/GdsViewLayoutSmoke.ps1` |
| 계획 | [plans/CLAUDE-070.plan.md](../../plans/CLAUDE-070.plan.md) — Standard |
| 집계 | PASS 2 / FAIL 0 / BLOCKED 6 / SKIPPED 0 / 미실행 0 |
| 최고 확인 수준 | 정적 검토 (빌드 / Smoke 는 BLOCKED) |

## 1. 결과 요약

- 정적 확인(SCN-05 / SCN-08)은 PASS: 삭제한 메뉴 이름을 참조하는 코드 0건, `map.ContextMenuStrip` 대입 0건, 줄바꿈 / BOM 원본과 같음, 고친 줄 AI 흔적 0.
- 빌드부터 BLOCKED: 같은 화면 파일을 **CLAUDE-071(GDS Map 조회 화면 상태 표시 통합, 진행 중)** 이 11:21~11:25 에 고치는 중이라 현재 소스가 컴파일되지 않는다(`GdsMapViewForm_LabelSearch.cs` CS0103 `lblLabelResult` 없음 - CLAUDE-071 이 Designer 에서 제거한 컨트롤). 이번 변경(CLAUDE-070) 때문이 아니다.
- CLAUDE-070 변경 내용은 CLAUDE-071 편집 뒤에도 그대로 남아 있음(`mapMenu` / `mnuCtxZoomFit` 0건).
- 참고(이번 실행 이전 증거, 판정에 넣지 않음): 11:15 CLAUDE-071 편집 전 소스로 TEMP 빌드 성공 + `GDS_VIEW_LAYOUT_SMOKE=PASS` 20/20.

## 2. 시나리오별 결과
| ID | 분류 | 결과 | 확인 수준 | 증거 |
| --- | --- | --- | --- | --- |
| SCN-01 | 정상 | BLOCKED | - | `evidence/01_Build.stdout.txt` (CS0103 `lblLabelResult` - CLAUDE-071 진행 중 편집) |
| SCN-02 | 정상 | BLOCKED | - | SCN-01 EXE 없음 (`02_LayoutSmokeNew.stderr.txt`) |
| SCN-03 | 부정(red) | BLOCKED | - | 변경 전 EXE 실행은 PNG 저장 단계 GDI+ 오류로 중단(`03_LayoutSmokeOldExeRed.stderr.txt`, `png_old` 폴더 문제로 추정) - 다음 실행에서 OutDir 확인 후 재실행 |
| SCN-04 | 회귀 | BLOCKED | - | SCN-01 EXE 없음 |
| SCN-05 | 부정 | PASS | 정적 검토 | `evidence/05_RemovedNames.txt` (COUNT=0, ContextMenuStrip 대입 0) |
| SCN-06 | 회귀 | BLOCKED | - | SCN-01 EXE 없음 |
| SCN-07 | 회귀 | BLOCKED | - | SCN-01 EXE 없음 |
| SCN-08 | 정적 | PASS | 정적 검토 | `evidence/08_Static.txt` (LF / BOM 원본과 같음, AI 흔적 0) |

## 3. 결함 (FAIL)

없음.

## 4. BLOCKED / SKIPPED
| ID | 사유 | 필요한 조치 (누가) |
| --- | --- | --- |
| SCN-01 ~ 04, 06, 07 | CLAUDE-071 이 같은 화면 파일(Designer / Layout / Chain / LabelSearch 참조)을 고치는 중이라 소스가 컴파일되지 않음 | CLAUDE-071 편집이 끝난 뒤 같은 계획으로 재실행 (Claude, 작업 현황 CLAUDE-071 행 변경 감시) |

## 5. 검증하지 않은 것

- 빌드 / 화면 밖 Smoke / 회귀 Probe 전부 (위 BLOCKED).
- DMS 실제 화면에서 도면 우클릭 시 메뉴가 뜨지 않는지 — 화면 확인 대기(사용자).
- CLAUDE-071 과 합쳐진 최종 소스의 동작.

## 6. 다음 조치

1. CLAUDE-071 완료 기록 확인 -> 같은 계획으로 SCN-01 ~ 07 재실행.
2. 사용자: VS 빌드 후 DMS 에서 도면 우클릭 확인.
