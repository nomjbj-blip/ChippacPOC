# E-Test 결과 Upload 재설계

작성: 2026-10-13 Claude (CLAUDE-029 12단계) / 상태: **확정 설계 (권장안 전부 채택, 9절이 최신) — 구현 대기**. 4·7·8절은 검토 과정

> 2026-10-14 (CLAUDE-086): [E-Test Para Setup 설계](2026-10-14_ETest_Para_Setup_설계.md) P7 이 결정되면 저장 대상이 "Spec 정의 Parameter" -> "등록 Parameter(사용 Y)" 로 바뀐다 (S1 / S4 변경, 9절 구현 전 반영). DDL 도 Para Setup DDL 과 같은 날 적용한다.

추가 설계 (2026-10-14, CLAUDE-085): 업로드 저장 때 YMS 테이블(`TQP_LOT` / `TQP_WAFER` / `TD_<PROGRAM>` / `TQP_PARASPEC` / 요약)에도 적재 -> [E-Test Upload YMS 적재 설계](2026-10-14_ETest_Upload_YMS적재_설계.md). 9.4 서비스 / 9.6 화면 / 9.9 구현 순서에 더해진다 (결정 D1 ~ D9 대기)

사용자 지시 (2026-10-13): "회의록 및 작업 내용들 토대로 Upload 기능에 대한 재설계 시작"

범위: E-Test 결과 파일 업로드 화면 `frmEtResultUpload` 와 저장 서비스(`GdsEtResult`). KLARF 업로드는 다른 작업 범위라 제외한다.

근거:
- 회의록 `2026-10-02_POC_상견례` / `2026-10-06_데이터기준_협의` / `2026-10-12 메일(10-08 회의 회신)` / `2026-10-13_데이터기준_후속협의`
- 설계 [Parsing / DB](2026-10-07_ETest_결과파일_Parsing_DB설계.md) / [Spec](2026-10-07_ETest_Limit_설정_설계.md) / [Die 좌표](2026-10-13_Die좌표_대응_Setup_설계.md)

---

## 1. 요구사항 (회의록 기준)

| 번호 | 요구 | 출처 |
| --- | --- | --- |
| R1 | POC 는 **엔지니어 수동 업로드**. 자동 수집 / MES 연동은 본 프로젝트 | 10-02, 10-13 |
| R2 | 파일명을 `_` 로 잘라 **순서대로 기준 정보**로 쓴다 (Customer / Device / Wafer Type / Wafer Rev / Test Type / Test Date / Lot / Lot-Wafer No). **Lot-Wafer No 가 Wafer 식별에 중요** | 10-12 메일, 10-13 |
| R3 | `$Silterra$` 표기는 **기준 정보에 넣지 않는다** | 10-13 |
| R4 | Test Type(TSVET -> COWET -> FT) = 공정. 같은 Wafer 의 3단계를 따로 보관 | 10-12 메일 |
| R5 | 파일에는 **Pass / Fail 결과가 없다** -> 시스템이 Spec(정의된 약 120개 Parameter)으로 판정. 정의된 항목의 '-' 는 Fail | 10-06, 10-12, 10-13 |
| R6 | 업로드할 때 **Fail Chain 을 다른 색(빨간색 등)으로 표시** | 10-02 |
| R7 | Hardware Bin / Software Bin 열은 **무시** | 10-06 |
| R8 | E-Test 파일은 **변환 없이 그대로** 쓴다. Back / Front side 와 Step 은 **분석할 때 엔지니어가 선택** | 10-06, 10-13 |
| R9 | Die 인덱스 대응(E-Test / KLARF / Map)은 **시스템 내부에서만** 처리, 사용자 표시 불필요 | 10-13 |
| R10 | 제품 정보는 업로드 때 파일명에서 넣는다. **Map Setup 이 먼저**, Map 이 없으면 없는 대로 진행 | 10-13 사용자 결정 |
| R11 | Full(76) / Hybrid(26) 는 같은 Device, Map 1개. OMM / OMM_Hybrid = U9 | 10-06, 10-13 |
| R12 | KLARF 와 연결할 **Wafer ID 매칭 규칙은 고객 정리 대기** (E-Test 본문에 Wafer ID 없음) | 10-06, 10-12 |
| R13 | FT 는 좌표 없이 Part ID = Die# (POC 에 FT 실제 파일은 아직 없음) | 10-12 메일, 10-13 확인 |

## 2. 현재 구현 (As-Is)

| 구분 | 내용 |
| --- | --- |
| 화면 | 파일 1개 선택 -> 분석 -> 정보 칸 16개 + 경고 / Die 미리보기 (DUT / X / Y / HW Bin / SW Bin / Pass·Fail / Flag / 시험 시간 / '-' 값) / Slot 입력 + WAFER_ID 미리보기 / [DB 저장] / Device 파일 목록 / [선택 파일 삭제] / 목록 더블클릭 -> 조회 창 |
| 기준 정보 | 파일명 8칸 파싱 (`PARSE_RULE_VER 2`), Device = 파일명 2칸, Slot = Lot-Wafer No 끝 숫자(없으면 입력), 중복 키 Factory + WAFER_ID + 공정 |
| 저장 | BeginEtFile(LOADING) -> 항목 Merge -> Die -> 측정값 1만 건씩 -> CompleteEtFile(READY, 요약 기록). 66,120 값 약 1초 |
| 판정 | Spec 판정은 Spec 설정 화면에서만 (업로드 화면 / 조회 창에는 없음) |

## 3. 요구사항 대비 차이 (Gap)

| 번호 | 현재 | 문제 | 요구 |
| --- | --- | --- | --- |
| G1 | 정보 칸 "Pass 20 / Fail 4", 목록 "Fail Die", 미리보기 "Pass/Fail" = **파일 DUT Flag 기준** | 고객 기준(파일에 Pass/Fail 없음, Spec 으로 판정)과 다르다. 샘플은 DUT Flag Fail 4개(DUT #20 / #22 / #31 / #42, PIC_COMB Leakage — Spec 정의 밖) 인데 Spec 판정 Fail 은 DUT #44 1개(TSVDC RES 6항목 718~719 > 715) | R5 |
| G2 | 업로드 화면에 Spec 판정 / Fail Chain 표시 없음 | 엔지니어가 올리면서 Fail 을 볼 수 없다 | R5, R6 |
| G3 | `$Silterra$` -> FAB_NAME 컬럼 / 정보 칸 "Fab" | 기준 정보에 넣지 않기로 함 | R3 |
| G4 | VIEW_SIDE = 'BACK' 고정 저장 | 면은 분석 때 엔지니어가 선택 | R8 |
| G5 | 미리보기에 HW Bin / SW Bin | 무시하기로 함 | R7 |
| G6 | 파일 1개씩 업로드 | Lot 1개 = Wafer 25장 x 공정 3 = 최대 75개 파일. 수동 업로드라도 여러 파일을 한 번에 올려야 실용적 | R1 |
| G7 | Map 확인 없음 | Map 이 있으면 Die 좌표가 Map 안에 드는지 내부 확인 필요 (없으면 그냥 진행) | R9, R10 |
| G8 | FT (좌표 없음) 업로드 불가 (`DIE_X / DIE_Y` NOT NULL, 파서 (X, Y) 필수) | FT 파일이 오면 막힌다 | R13 |
| G9 | KLARF 매칭 키 없음 | 고객 규칙 대기. Lot-Wafer No 원문은 이미 저장(`LOT_WAFER_NO`)이라 규칙이 오면 다시 올리지 않고 계산 가능 | R12 |

## 4. 재설계안 (To-Be)

### 4.1 업로드 흐름 (8.2 로 대체)

```text
[파일 추가] (여러 개 / 폴더)                       -> 대기 목록에 행 추가
  └ 파일마다 분석 (백그라운드, 순서대로)
      1) 파일명 8칸 파싱 -> 기준 정보 (Device / 공정 / Lot / Lot-Wafer No -> Slot). $...$ 는 무시
      2) 시트 파싱 (헤더 '#번호' 제거, Die / 측정값)
      3) Spec 판정 미리보기: Device + 공정의 Spec 으로 Fail Die / Fail Parameter / Fail Chain 계산 (Spec 없으면 "Spec 없음")
      4) Map 확인 (내부): Map ID = Device. 있으면 Die 좌표가 Map 사용 Die 안인지 확인 -> 밖이면 경고. 없으면 "Map 없음" 안내만
      5) 중복 확인: 같은 Factory + WAFER_ID + 공정 이 이미 있는지
  └ 상태: 준비 / 경고 / 오류 / 중복
[저장] -> 고른 행(또는 전체)을 순서대로 저장, 행마다 결과(파일 번호 / 오류) 표시
         중복 행은 저장 전에 한 번에 묻는다: 덮어쓰기 / 건너뛰기
```

- 저장 서비스는 지금 것(Begin -> Item -> Die -> Value -> Complete)을 그대로 쓴다. 파일 단위로 성공 / 실패가 나뉘고, 실패한 파일은 LOADING 으로 남지 않게 지운다(지금 화면과 같음).
- ~~Spec 판정은 저장하지 않는다~~ -> 8절: 판정을 Die / 값에 저장하고 Spec 이 바뀌면 다시 판정한다.

### 4.2 화면 (안, 9.6 으로 대체)

| 영역 | 내용 |
| --- | --- |
| 위 | Factory / [파일 추가] [폴더 추가] [목록 비우기] / [저장] [선택 저장] / 진행 막대 |
| 대기 목록 (위 가운데) | 파일명 / 상태(준비·경고·오류·중복·저장됨) / Device / 공정 / Lot / Lot-Wafer No / Slot(수정 가능) / Test Date / Die 수 / **Spec Fail Die** / 경고·오류 / 저장 결과 |
| 선택 파일 상세 (아래 왼쪽) | 기준 정보 칸 (Customer / Device / Wafer Type / Wafer Rev / 공정 / Test Date / Lot / Lot-Wafer No / Slot / Sheet / 항목 / 측정값 / '-' 값) — Fab 칸 제거 |
| Die 판정 (아래 가운데) | Die 목록: DUT / X / Y / **Spec 판정(Pass / Fail / Spec 없음)** / **Fail Parameter 수** / '-' 값 / DUT Flag(참고, 회색). HW / SW Bin 제거. Fail Die 행 빨간색 |
| Fail Chain (아래 오른쪽) | 고른 Die(또는 파일 전체)의 Fail Parameter: 공정 / Parameter / Chain / 값 / Lower / Upper — **Fail 빨간색** (R6). 더블클릭 -> Spec 화면 이동 |
| 등록 목록 (탭) | 지금 목록 유지 (Device 별 파일, 더블클릭 -> 조회 창, 삭제). "Fail Die" 열은 Spec 판정 Fail Die 로 바꾼다 |

조회 창(`frmEtResultView`)도 같은 기준으로: Die 열 머리글 / 셀 색을 Spec 판정으로 (DUT Flag 는 참고 표시).

### 4.3 데이터 변경 (최소, 8.3 으로 대체)

| 대상 | 변경 | 이유 |
| --- | --- | --- |
| `TQP_GDS_SRC_FILE.FAB_NAME` | 새 저장에서 채우지 않음 (컬럼은 남김, 기존 행 그대로) | R3 |
| `TQP_GDS_SRC_FILE.VIEW_SIDE` | 채우지 않음 / 의미 "원본 그대로" (면은 분석 때 선택) | R8 |
| `TQP_GDS_SRC_FILE.PASS_DIE_COUNT / FAIL_DIE_COUNT` | **DUT Flag 기준**임을 명확히: 이름 유지하고 화면 표시는 "DUT Flag Fail(참고)" 로만, Spec Fail Die 는 조회 때 계산 | R5 (판정은 Spec) |
| `TQP_GDS_ET_DIE` HW_BIN / SW_BIN | 저장은 유지(원본 보관), 화면에서 숨김 | R7 |
| FT 대비 (G8) | `TQP_GDS_ET_DIE.PART_ID` 추가, `DIE_X / DIE_Y` NULL 허용, 파서가 (X, Y) 없으면 Part ID 사용 — **FT 실제 파일을 받으면** 진행 | R13 |
| Spec 판정 조회 | 신규 쿼리 1개: 파일 1개의 Die 별 Spec Fail 수 / Fail Parameter (조회 창 / 등록 목록 / 업로드 미리보기 공용). 업로드 미리보기는 저장 전이라 화면이 Spec(`GetEtLimit`)을 받아 메모리에서 같은 규칙으로 계산 | R5, R6 |

### 4.4 내부 Die 좌표 확인 (R9, R10)

- Map ID = 파일명 Device. Map 이 있으면 `DieCoordinateMapper`(ETest 종류)로 Die (X, Y) -> 시스템 좌표 -> 사용 Die 인지 확인. 밖이면 경고(저장은 가능).
- Map 이 없으면 확인 없이 진행하고 대기 목록에 "Map 없음 (File Map Setup 필요)" 안내만.
- Die# / 시스템 좌표는 화면에 보이지 않는다(R9). KLARF 원점이 필요 없는 ETest 종류만 쓰므로 원점 문제(CLAUDE-075)와 무관.
- Map 조회 서비스가 필요하다 (`TQP_USEMAP` 사용 Die). 기존 TEST DSL 조회를 BSL 에 메서드 1개로 연다.

### 4.5 KLARF Wafer 매칭 (R12)

- 이번 재설계에서는 키를 만들지 않는다. 고객 규칙이 오면 `LOT_ID` / `LOT_WAFER_NO` / `SLOT_NO` 원문으로 계산하는 쿼리 / 함수만 추가한다(다시 업로드 불필요).

## 5. 바뀌지 않는 것

- 파일명 8칸 규칙 / 중복 키(Factory + WAFER_ID + 공정) / 저장 서비스와 배치 크기 / Spec 테이블 / Chain 묶음 / 조회 창 더블클릭 흐름 / 기존 데이터.

## 6. 단계 (결정 후, 9.9 로 대체)

| 단계 | 내용 |
| --- | --- |
| 1 | 판정 기준 정리: Spec 판정 쿼리 + 업로드 / 등록 목록 / 조회 창의 Pass·Fail 을 Spec 기준으로, DUT Flag 는 참고, Fab / HW·SW Bin 숨김, FAB_NAME·VIEW_SIDE 저장 중지 |
| 2 | 여러 파일 업로드: 대기 목록 / 행별 분석·상태 / 일괄 저장 / 중복 일괄 처리 + Fail Chain 패널 |
| 3 | 내부 Map 확인 (Map 조회 메서드 + `DieCoordinateMapper`) |
| 4 | FT (실제 파일 수령 후) |

## 7. 결정 필요 (권장안 먼저, 9.1 에서 확정)

| 번호 | 항목 | 권장 | 다른 안 |
| --- | --- | --- | --- |
| U1 | 화면의 Pass / Fail 기준 | **Spec 판정** (DUT Flag 는 "참고"로 회색 표시) | DUT Flag 그대로 |
| U2 | 업로드 때 Spec 판정 / Fail Chain 빨간색 미리보기 | **넣는다** (10-02 요청) | 업로드는 저장만, 판정은 Spec / 조회 화면 |
| U3 | 여러 파일 한 번에 업로드 | **넣는다** (파일 추가 / 폴더 추가 + 대기 목록) | 지금처럼 1개씩 |
| U4 | 중복(같은 Wafer + 공정) 일괄 처리 | **저장 전에 한 번에 묻기 (덮어쓰기 / 건너뛰기)** | 파일마다 묻기 |
| U5 | `$Silterra$` (FAB_NAME) | **저장 / 표시 중지** (원문은 파일명에 남음) | 저장만 하고 숨김 |
| U6 | VIEW_SIDE 'BACK' 고정 | **저장 중지** (분석 때 선택) | 그대로 |
| U7 | HW / SW Bin | **화면에서 숨김, 저장은 유지** | 저장도 중지 |
| U8 | 내부 Map 확인 | **넣는다** (Map 없으면 안내만) | 넣지 않음 |
| U9 | FT 대비 | **FT 실제 파일 수령 후** | 지금 구조만 먼저 변경 |

## 8. 사용자 결정 반영: Spec 에 정의된 Header 만 저장 + Die 별 저장 -> Map 표시 (2026-10-13, 4절 일부 대체)

사용자 지시: "CSV 에 있는 Header 에서 정의된 정보에 대해서만 저장하도록 설계한다. Spec 에 대한 정의는 Spec Setup 화면이 따로 있다. 해당 정보 기준으로 각 Die 별로 Data 가 저장하여 나중에 Map 에서 보여 줄 수 있도록 한다."

### 8.1 개념

```text
Setup 먼저 (Setup 우선)
  - Spec Setup 화면: Device + 공정 + Parameter(헤더 전체) Spec  ── 저장 대상 목록
  - File Map Setup: Device Map                                  ── Map 좌표
업로드
  파일 헤더 2,755개 ─(정의된 Parameter 만 고름)→ 37개 (TSVET)
  Die 24개 마다: 정의 Parameter 값 + 값 판정(P / F) + Die 판정(Fail 수 / P·F) + Map 좌표(시스템 X, Y)
  → 나중에 Wafer Map 에서 Die 색(Pass / Fail / Fail 수 / 고른 Parameter 값)으로 바로 표시
```

- **정의 안 된 헤더는 저장하지 않는다** (값 / 측정 항목 모두). 파일의 전체 헤더 수만 기록한다.
- 저장량: TSVET 샘플 1개 = 66,120 값 -> **888 값** (37 x 24).
- 판정은 Spec 설정 화면의 Spec 그대로: 값 < Lower 또는 > Upper 이면 Fail, 정의 항목의 '-' 는 Fail, Enable = N 은 판정 안 함(값은 저장).

### 8.2 업로드 흐름 (4.1 대체)

| 순서 | 처리 | 없거나 다를 때 |
| --- | --- | --- |
| 1 | 파일명 8칸 파싱 -> Device / 공정 / Lot / Lot-Wafer No / Slot | 오류 (지금과 같음) |
| 2 | **Spec 확인**: Device + 공정 의 Spec 목록 | **없으면 업로드 중단** "Spec 설정에서 먼저 등록" (Setup 우선) |
| 3 | 헤더 대조: 파일 헤더('#번호' 제거, 대문자) ↔ Spec Parameter | 정의됐는데 파일에 없는 Parameter = 경고 목록 (저장은 진행) / 정의 안 된 헤더 = 건너뜀 (수만 표시) |
| 4 | Die 별 판정 계산 (값 판정 / Die Fail 수 / Die 판정) | |
| 5 | **Map 확인**: Map ID = Device. 있으면 E-Test (X, Y) -> 시스템 Map 좌표 (`DieCoordinateMapper`), Map 사용 Die 밖이면 경고 | **Map 없으면 Map 좌표 비우고 진행** + 안내 (나중에 [Map 좌표 다시 계산]) |
| 6 | 중복 확인 (Factory + WAFER_ID + 공정) | 덮어쓰기 / 건너뛰기 |
| 7 | 저장: 파일 -> 정의 항목 -> Die(판정 + Map 좌표) -> 값(판정) -> 완료 | 실패하면 그 파일만 지움 |

### 8.3 데이터 (4.3 대체)

| 테이블 | 변경 | 내용 |
| --- | --- | --- |
| `TQP_GDS_SRC_FILE` | 컬럼 추가 | `HEADER_COUNT`(파일 전체 헤더) / `DEFINED_COUNT`(업로드 때 Device + 공정 Spec 수) / `MATCHED_COUNT`(저장한 Parameter 수) / `MISSING_COUNT`(정의됐는데 파일에 없음) / `JUDGE_TIME`(판정 시각) / `MAP_ID`(좌표 계산에 쓴 Map, 없으면 NULL). `PASS_DIE_COUNT / FAIL_DIE_COUNT` 는 **Spec 판정 기준**으로 의미 변경 (DUT Flag 기준 수는 버림) |
| `TQP_GDS_ET_ITEM` | 사용 범위 | **정의된 Parameter 만** 등록 (지금은 헤더 2,755개 전부) |
| `TQP_GDS_ET_DIE` | 컬럼 추가 | `MAP_X / MAP_Y`(시스템 Map 좌표, Map 없으면 NULL) / `JUDGE_FLAG`(P / F) / `FAIL_COUNT`(Fail Parameter 수, '-' 포함) / `NOT_TESTED_COUNT`(정의 항목 '-' 수). 기존 `PASS_FLAG`(DUT Flag) / HW·SW Bin 은 원본 참고로 남김(화면 숨김) |
| `TQP_GDS_ET_VALUE` | 컬럼 추가 | `JUDGE_FLAG`(P / F / 빈 값 = Enable N 이라 판정 안 함). 정의 Parameter 값만 들어간다 |

Map 표시용 조회 (다음 단계 Map 화면에서 사용):
- Die 판정 Map: 파일(또는 Wafer + 공정) -> `TQP_GDS_ET_DIE` (MAP_X, MAP_Y, JUDGE_FLAG, FAIL_COUNT)
- Parameter 값 Map: 파일 + Parameter -> `TQP_GDS_ET_VALUE` 조인 Die (MAP_X, MAP_Y, MEAS_VALUE, JUDGE_FLAG)
- 같은 Wafer 의 TSVET / COWET / FT 비교: WAFER_ID + 공정

### 8.4 Setup 이 바뀌면

| 바뀐 것 | 처리 |
| --- | --- |
| Spec 의 Lower / Upper / Enable | 저장된 값으로 **다시 판정** 가능: 서비스 `RejudgeEtFile`(파일 단위) / Spec 화면 저장 후 "이 Device + 공정 파일 다시 판정할까요?" |
| Spec 에 Parameter 추가 | 그 값은 저장돼 있지 않다 -> **파일을 다시 업로드** 해야 한다 (Spec 화면에 안내). 삭제한 Parameter 는 다시 판정 때 판정에서 빠진다 |
| Map 을 나중에 Setup | 서비스 `RemapEtFile` 로 저장된 E-Test (X, Y) 에서 Map 좌표 다시 계산 |

### 8.5 다른 기능 영향

| 기능 | 영향 / 조치 |
| --- | --- |
| Spec 설정 화면 | 통계 / Fail 수는 정의 Parameter 값만 쓰므로 그대로 동작. "데이터 있음 N" 은 "Spec 추가 후 재업로드 필요" 뜻이 된다 (안내 문구) |
| Chain 묶기 / Setup Chain 찾기 | Chain 목록이 측정 항목(전체 헤더 1,306 Chain) 기준 -> **정의 Parameter 의 Chain 만** 나온다 (TSVET / COWET / FT 정의 기준). "Spec 없는 측정 항목" 은 더 이상 없다 |
| 조회 창 (파일 더블클릭) | 행 = 정의 Parameter 만 (2,755 -> 37행), 셀 색 = 값 판정, Die 열 머리글 = Die 판정 |
| 지금 로컬 데이터 | 전체 헤더로 저장돼 있다 (SCK / OMM 파일 94 등). **지우고 다시 업로드** (POC 시험 데이터) |
| Smoke / Probe | 업로드 / Spec / 서비스 Probe 를 새 기준으로 고친다 (Spec 먼저 등록 -> 업로드) |

### 8.6 결정 필요 (9.1 에서 확정)

| 번호 | 항목 | 권장 | 다른 안 |
| --- | --- | --- | --- |
| S1 | Spec 이 없는 Device + 공정 업로드 | **중단** (Setup 우선) | 파일 / Die 만 저장하고 값은 없음 |
| S2 | 판정 결과 저장 | **저장 (값 / Die) + 다시 판정 서비스** (Map 바로 표시) | 저장 안 하고 볼 때 계산 |
| S3 | Map 좌표 저장 | **업로드 때 계산해 저장 + Map 없으면 비움, 나중에 다시 계산** | 볼 때마다 계산 |
| S4 | 측정 항목 마스터(`TQP_GDS_ET_ITEM`) | **정의 Parameter 만** | 헤더 전부 등록(값은 정의만) — Chain 목록 / 헤더 확인용 |
| S5 | 기존 로컬 데이터 | **지우고 다시 업로드** | 그대로 두고 새 업로드부터 적용 |
| S6 | 정의됐는데 파일에 없는 Parameter | **경고 후 저장** | 업로드 중단 |

## 9. 확정 설계 (2026-10-13 사용자 "권장안으로 설계")

### 9.1 확정한 결정

| 번호 | 결정 |
| --- | --- |
| U1 / U2 | 화면 Pass / Fail = **Spec 판정**. 업로드 때 판정 미리보기 + **Fail Chain 빨간색** |
| U3 / U4 | **여러 파일 업로드** (파일 추가 / 폴더 추가 + 대기 목록). 중복(Wafer + 공정)은 **저장 전에 한 번에** 덮어쓰기 / 건너뛰기 |
| U5 / U6 / U7 | `$Silterra$`(FAB_NAME)·VIEW_SIDE **저장 / 표시 중지**. HW / SW Bin **화면 숨김, 저장 유지** |
| U8 | 내부 Map 확인 (Map 없으면 안내만) |
| U9 | FT 는 **실제 파일 수령 후** |
| S1 | Device + 공정 Spec 이 없으면 **업로드 중단** (Setup 우선) |
| S2 | 판정을 **값 / Die / 파일에 저장** + Spec 변경 때 **다시 판정** |
| S3 | Map 좌표를 **업로드 때 저장**, Map 없으면 비움 + 나중에 **좌표 다시 계산** |
| S4 | 측정 항목 마스터 = **정의 Parameter 만** |
| S5 | 기존 로컬 E-Test 데이터 **지우고 다시 업로드** |
| S6 | 정의됐는데 파일에 없는 Parameter = **경고 후 저장** |

### 9.2 판정 규칙 (한 곳에 정의, 서버 SQL / 화면 미리보기 같은 규칙)

| 대상 | 규칙 |
| --- | --- |
| 값 (`TQP_GDS_ET_VALUE.JUDGE_FLAG`) | Spec Enable = N -> NULL (판정 안 함) / VALUE_STATUS N('-') -> **F** / E(빈 칸) -> NULL (판정 불가, 업로드 경고에 빈 칸 수) / V -> Lower 가 있고 값 < Lower 또는 Upper 가 있고 값 > Upper 이면 **F**, 아니면 **P** (경계값 Pass) |
| Die (`TQP_GDS_ET_DIE`) | FAIL_COUNT = 값 F 수 / NOT_TESTED_COUNT = 정의 항목 '-' 수 / JUDGE_FLAG = FAIL_COUNT > 0 이면 **F**, 아니면 **P** |
| 파일 (`TQP_GDS_SRC_FILE`) | PASS_DIE_COUNT / FAIL_DIE_COUNT = Die JUDGE_FLAG 수 (Spec 판정 기준으로 의미 변경), JUDGE_TIME = 판정 시각 |

- 서버: 값 저장 뒤 SQL UPDATE 3개(값 -> Die -> 파일)로 판정한다. 같은 SQL 을 다시 판정에 쓴다.
- 화면 미리보기: 신규 `ENGUI/ETest/EtJudgeRule.cs` (Spec 행 + 값 -> P / F / NULL). 서버 SQL 과 같은 규칙이고 저장하지 않는다.

### 9.3 DB (DDL `Persistence/2026-10-13_tqp_gds_et_defined_only.sql`)

| 테이블 | 변경 |
| --- | --- |
| `TQP_GDS_SRC_FILE` | 추가: `HEADER_COUNT NUMBER(10)`(파일 전체 헤더) / `DEFINED_COUNT NUMBER(10)`(업로드 때 Device + 공정 Spec 수) / `MISSING_COUNT NUMBER(10)`(정의됐는데 파일에 없음) / `JUDGE_TIME DATE` / `MAP_ID VARCHAR2(50 CHAR)`(좌표에 쓴 Map, 없으면 NULL) / `MAP_TIME DATE`. 의미 변경: `ITEM_COUNT` = 저장한 정의 Parameter 수, `PASS_DIE_COUNT / FAIL_DIE_COUNT` = Spec 판정. `PARSE_RULE_VER` = 3 (정의 Header 만 저장). 새 저장부터 `FAB_NAME / VIEW_SIDE` 는 NULL |
| `TQP_GDS_ET_DIE` | 추가: `MAP_X NUMBER(10)` / `MAP_Y NUMBER(10)` / `JUDGE_FLAG CHAR(1)` (CHECK P / F) / `FAIL_COUNT NUMBER(10)` / `NOT_TESTED_COUNT NUMBER(10)`. 기존 `PASS_FLAG`(DUT Flag) / HW·SW Bin 은 원본 참고로 유지 |
| `TQP_GDS_ET_VALUE` | 추가: `JUDGE_FLAG CHAR(1)` (CHECK P / F, NULL 허용) |
| `TQP_GDS_ET_ITEM` | 구조 그대로. 새 저장부터 정의 Parameter 만 들어온다 |
| 로컬 데이터 (S5) | 별도 스크립트 `2026-10-13_tqp_gds_et_local_reset.sql`: E-Test 파일 / Die / 값 / 측정 항목 전부 삭제 (로컬 DB 만, 고객 DB 는 데이터 없음). 대량 삭제라 사용자가 직접 실행할 수도 있음 |

### 9.4 쿼리 / 서비스 (`iGdsEtResult`, 새 wellknown 없음)

| 메서드 | 변경 |
| --- | --- |
| `BeginEtFile(FileInfo)` | FileInfo **21항목**: 0~17 지금과 같음(17 Fab 은 받되 저장 안 함) + 18 HEADER_COUNT / 19 DEFINED_COUNT / 20 MISSING_COUNT. **Device + 공정 Spec 이 0건이면 거부 (S1)**. PARSE_RULE_VER 3, FAB_NAME / VIEW_SIDE NULL. 이전 12 / 18항목 호출은 거부 (화면 / Probe 같이 바꿈) |
| `MergeEtItem` | 받은 항목이 모두 그 Device + 공정 Spec 에 있는지 확인(대문자 / '#' 제거 규칙), 아니면 거부 (S4) |
| `CreateEtDieBatch / CreateEtValueBatch` | 그대로 (값은 정의 Parameter 만 온다) |
| `CompleteEtFile` | 개수 확인(값 = Die x 저장 항목) 뒤 **판정(9.2) + Map 좌표(9.5)** 를 하고 READY. 반환에 SPEC_FAIL_DIE / MAP_ID / MAP_DIE(좌표 찾은 Die) / OUT_OF_MAP 추가 |
| 신규 `RejudgeEtFiles(aKey{factory, device, 공정})` | 그 Device + 공정의 READY 파일 다시 판정 (Spec 화면 저장 뒤 호출). 반환: 파일 수 |
| 신규 `RemapEtFiles(aKey{factory, device})` | 그 Device 파일의 Map 좌표 다시 계산 (Map 을 나중에 Setup 했을 때). 반환: 파일 수 / Map 밖 Die 수 |
| 신규 `GetEtMapDieList(factory, mapId)` | Map 사용 Die (X / Y / USECODE) — 업로드 미리보기의 Map 확인용 (`TQP_USEMAP`, 우리 XML 에 조회 1개) |
| `GetEtDieList` | 열 추가: MAP_X / MAP_Y / JUDGE_FLAG / FAIL_COUNT / NOT_TESTED_COUNT (= Map 표시용 Die 데이터) |
| `GetEtValueList` | 열 추가: JUDGE_FLAG (= Map 표시용 Parameter 값) |
| `GetEtFileList / GetEtFile` | 열 추가: HEADER / DEFINED / MISSING / MAP_ID / JUDGE_TIME |

### 9.5 Map 좌표 (서버, `DieCoordinateMapper` 재사용)

1. MAP_ID = 파일 DEVICE_ID. `TQP_USEMAP`(Factory + MAPID) 사용 Die 를 읽는다. 없으면 MAP_ID / MAP_X / MAP_Y NULL 로 끝.
2. `DieCoordinateMapper.FromDieTable(사용 Die, 원점)` 의 **ETest 종류**로 (DIE_X, DIE_Y) -> 시스템 좌표. KLARF 원점은 쓰지 않으므로 (0, 0) 을 넘긴다.
3. Map 사용 Die 가 아니면 MAP_X / MAP_Y NULL + OUT_OF_MAP 수에 센다 (경고, 저장은 유지).
4. Die 별 UPDATE 는 ExecuteMultiple 한 번.

### 9.6 화면

**업로드 화면 `frmEtResultUpload` (다시 작성)**

| 영역 | 내용 |
| --- | --- |
| 위 | Factory / [파일 추가] [폴더 추가] [목록 비우기] / [모두 저장] [선택 저장] / 진행 막대 |
| 대기 목록 | 파일명 / 상태(분석 중 · 준비 · 경고 · 오류 · 중복 · 저장됨 · 실패) / Device / 공정 / Lot / Lot-Wafer No / Slot(수정 가능) / Test Date / Die / 정의·저장·누락 Parameter / **Spec Fail Die** / Map(있음 · 없음 · 밖 n) / 메시지 / 파일 번호 |
| 선택 파일: 기준 정보 | Customer / Device / Wafer Type / Wafer Rev / 공정 / Test Date / Lot / Lot-Wafer No / Slot / 헤더 수 / 정의 / 저장 / 누락 / '-' 값 / 빈 칸 (Fab / Pass·Fail(DUT Flag) 칸 제거) |
| 선택 파일: Die 판정 | DUT / X / Y / **판정** / **Fail 수** / '-' 수 / DUT Flag(회색 참고). Fail 행 빨간색 |
| 선택 파일: Fail Chain | 고른 Die(없으면 파일 전체)의 Fail Parameter: Parameter / Chain / 값 / Lower / Upper / 사유('-' · 범위 밖) — **빨간색** |
| 탭 [등록 파일] | Device 별 등록 목록 (Spec Fail Die / Map 열 추가), 더블클릭 -> 조회 창, [선택 파일 삭제], **[Map 좌표 다시 계산]** |

- 분석은 파일마다 순서대로 백그라운드(BackgroundWorker)로 하고, 대기 목록에 바로 반영한다.
- 저장 전 확인 한 번: "저장 n / 중복 m (덮어쓰기 · 건너뛰기) / 오류 k 제외".
- Spec 없음(S1) 파일은 상태 "오류: Spec 없음 (Spec 설정에서 Device + 공정 등록)" 이고 저장 대상에서 빠진다.

**조회 창 `frmEtResultView`**: 행 = 저장된 정의 Parameter, 셀 색 = 값 판정(F 빨간색), Die 열 머리글 = Die 판정, 정보 줄 = Spec 판정 Pass / Fail.

**Spec 설정 화면 `frmEtLimitSetup`**: [저장] / [Spec 표 붙여넣기] 뒤 "이 Device + 공정 의 업로드 파일 n 개를 다시 판정할까요?" -> `RejudgeEtFiles`. Parameter 를 새로 추가했으면 "추가한 Parameter 값은 파일을 다시 업로드해야 저장됩니다" 안내.

### 9.7 파서

- `EtResultFileParser.Parse` 는 지금처럼 전체를 읽는다 (헤더 대조에 필요).
- 신규 `EtResultFileParser.SelectDefined(file, specNames)`: 헤더(정리 + 대문자) ↔ Spec Parameter 대조 -> 저장할 Items / Values 만 남긴 결과 + 누락 목록 + 건너뛴 헤더 수.
- `$...$` 는 파싱만 하고(경고 판단용) 저장 / 표시하지 않는다.

### 9.8 검증

| 검사 | 기대값 (TSVET 샘플, Spec 표 TSVET 37) |
| --- | --- |
| 서비스 Probe | Spec 없으면 BeginEtFile 거부 / 정의 밖 항목 Merge 거부 / 저장 후 항목 37 · 값 888 / Die 판정 F = DUT #44 1개 (FAIL_COUNT 6), 나머지 P / 파일 FAIL_DIE 1 · PASS_DIE 23 / Map: 시험 Map(PROBE 에 OMM_Hybrid 스냅샷으로 MAPID = Device 등록 후 삭제) 으로 24 Die 모두 좌표, DUT #20 -> (9, 2) / Map 없으면 좌표 NULL / Spec 바꾸고 RejudgeEtFiles -> 판정 바뀜 / Map 등록 후 RemapEtFiles -> 좌표 채움 |
| 업로드 Smoke | 파일 3개(TSVET / COWET 이름 / 7칸 오류) 추가 -> 상태(준비 · Spec 없음 · 오류) / 중복 일괄 질문 / Fail Chain 빨간색 / 등록 목록 Spec Fail Die / 조회 창 37행 |
| Spec Smoke | 저장 뒤 다시 판정 질문 -> 판정 반영, Chain 묶기 목록 = 정의 Chain |
| Die 좌표 Probe | 그대로 |

### 9.9 구현 순서

| 단계 | 내용 |
| --- | --- |
| 1 | DDL + 판정 SQL + Map 좌표 + 서비스 변경 / 신규 3개 + Probe |
| 2 | 파서 SelectDefined + EtJudgeRule + 업로드 화면 다시 작성 + Smoke |
| 3 | 조회 창 / Spec 화면(다시 판정) + Smoke |
| 4 | 로컬 데이터 정리 (S5) + 샘플 다시 업로드 |
| 5 | FT (파일 수령 후) |

