# E-Test Limit(Spec) 설정 화면 설계

작성: 2026-10-07 Claude (CLAUDE-029) / 상태: **2026-10-12 Parameter 단위 Spec + Chain 묶기 / Setup Chain 으로 찾기 구현 완료 (12 · 14 · 15절이 우선, 13절 대표 Chain 은 15절로 대체)** — 4 ~ 11절은 이전 설계

> 2026-10-14 후속 설계 (CLAUDE-086): [E-Test Para Setup](2026-10-14_ETest_Para_Setup_설계.md) — 결과 파일 Header 를 Parameter 로 등록하는 화면 분리 + **대표 Chain 등록**(단자 이름 묶음, 13절 개념을 다시 씀) ↔ GDS Chain 1:1 연결. 결정(P6 / P8)되면 Spec 은 등록 Parameter 에만 두고, 15절 Chain 묶기 / Setup Chain 으로 찾기는 대표 Chain 으로 대체한다.

사용자 지시 (2026-10-07): "Para 에 대한 Limit 설정 화면을 만들자. 현재까지 내용 확인해서 설계안 도출"
선행: [E-Test Parsing / DB 설계](2026-10-07_ETest_결과파일_Parsing_DB설계.md) (9 ~ 12절 구현 완료, 3.3절 Limit 참고안)

---

## 1. 지금까지 정해진 것 (근거)

| 항목 | 내용 | 근거 |
| --- | --- | --- |
| 파일의 Upper / Lower Limit | 읽지 않고 저장하지 않는다. 판정 기준은 우리가 만드는 Limit 화면에서 관리 | 사용자 지시 (최초 요청) |
| Para 정의 | 헤더 `=` 앞 = Para (예: `Continuity_iforce`), `=` 뒤 = 조건 (`8.0mA`), `@` 뒤 = Chain | 사용자 정의, 설계 10.2절 |
| 측정 항목 | `TQP_GDS_ET_ITEM` (Factory + Device 단위, PARA_NAME / COND_VALUE / CHAIN_NAME / CHAIN_NAME2 / UNIT) | 구현 완료 |
| 측정값 | `TQP_GDS_ET_VALUE` (V 숫자 / N 미측정 `-` / E 빈 값). Pass / Fail 은 저장하지 않고 Limit 과 비교해 계산 | 설계 10.3절 |
| 결과 화면 | 업로드 화면 / 조회 창 (값만 표시, 판정 색은 "Limit 화면과 함께 나중") | 설계 11절 |
| 배포 | 사용자가 VS 디버깅으로 직접 | 2026-10-07 사용자 지시 |

## 2. 샘플 데이터로 본 Para 별 값 (RH-HS2615-02, 24 Die)

| Para | 조건 | 단위 | 항목 수 | 파일 Limit (Lower / Upper) | 최소 | 중앙값 | 99% | 최대 |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| Continuity_iforce | 8.0mA | V | 1 | 0 / 5.7 | 0.0668 | 0.0724 | 0.0876 | 0.0876 |
| Connectivity_iforce | 8.0mA | V | 865 | N/A | 0.0624 | 0.0762 | 1.76 | 5.75 |
| Connectivity_iforce | 1.0mA | V | 10 | N/A | 0.0696 | 0.152 | 0.237 | 6.4 |
| Resistance_iforce | 8.0mA | Ohm | 865 | N/A | 7.8 | 9.5 | 221 | 719 |
| Resistance_iforce | 1.0mA | Ohm | 10 | N/A | 69.6 | 152 | 257 | 6400 |
| Leakage_Parallel | - | uA | 848 | -50 / 50 | -951 | -0.016 | 0.205 | 747 |
| Leakage_Serial | - | uA | 156 | -50 / 50 | -747 | -0.144 | 0.208 | 0.345 |

알게 된 점

1. **같은 Para 라도 조건(iforce) 이 다르면 값 범위가 다르다.** Connectivity 8.0mA 와 1.0mA 는 따로 정해야 한다 -> Limit 키에 조건 포함.
2. **Open 판정에 필요한 Connectivity / Resistance 는 파일 Limit 이 모두 N/A** 다. 이 화면이 없으면 Open Fail 을 판정할 수 없다.
3. **같은 Para 안에서도 Chain 마다 정상 값이 크게 다르다.** Resistance_iforce=8.0mA 의 Chain 별 중앙값: C4DC 8.5 ~ 100.5 Ohm, TSVDC 11.4 ~ 347 Ohm. 그래서 Para 공통 Upper 하나로는 **완전 Open (수백 ~ 수천 Ohm) 만** 잡힌다. 평소보다 3배 높아진 Chain (예: C4DC_148_73_FH 중앙값 12 -> 최대 39 Ohm, 이런 Chain 14개) 은 놓친다 -> Chain 개별 Limit 이 나중에 필요할 수 있다 (L1).

## 3. 판정 규칙

| 측정값 상태 | 판정 |
| --- | --- |
| V (숫자) | Lower 가 있으면 `값 < Lower` 이면 Fail, Upper 가 있으면 `값 > Upper` 이면 Fail. 경계값은 Pass (`Lower <= 값 <= Upper`, 파일 Limit 관례와 같음) |
| N (미측정 `-`) | Fail 로 세지 않고 **미측정**으로 따로 센다 (L7) |
| E (빈 값) | 판정 안 함 |
| Limit 없음 / 사용 안 함 | 판정 안 함 (`NO_LIMIT`) |

Limit 찾는 순서: (Para, 조건, Chain) 개별 Limit -> (Para, 조건, `*`) Para 공통 Limit -> 없음. 1차 구현은 Para 공통만 (L1).

## 4. DB 설계 — `TQP_GDS_ET_LIMIT` (신규)

| 컬럼 | 형식 | 설명 |
| --- | --- | --- |
| FACTORY | VARCHAR2(30 CHAR) PK | |
| DEVICE_ID | VARCHAR2(50 CHAR) PK | 측정 항목과 같은 Device 단위 (L3) |
| PARA_NAME | VARCHAR2(50 CHAR) PK | `Resistance_iforce` |
| COND_VALUE | VARCHAR2(20 CHAR) PK | `8.0mA`, 조건 없으면 `-` (PK 라 NULL 대신 `-`) |
| CHAIN_NAME | VARCHAR2(130 CHAR) PK | `*` = Para 공통. Chain 개별이면 Chain 이름 (Leakage_Serial 은 `A:B`) — 1차는 `*` 만 |
| LOWER_LIMIT | BINARY_DOUBLE | NULL = 하한 없음 |
| UPPER_LIMIT | BINARY_DOUBLE | NULL = 상한 없음 |
| UNIT | VARCHAR2(10 CHAR) | 저장 시 측정 항목 단위와 같아야 함 |
| USE_FLAG | CHAR(1) DEFAULT 'Y' | N 이면 판정 안 함 (값은 보관) |
| REMARK | VARCHAR2(200 CHAR) | 근거 메모 (예: "Open 판정 1000 Ohm") |
| 감사 컬럼 | | CREATE_USER / CREATE_TIME / UPDATE_USER / UPDATE_TIME |

제약: `CHECK (LOWER_LIMIT IS NOT NULL OR UPPER_LIMIT IS NOT NULL)`, `CHECK (LOWER_LIMIT IS NULL OR UPPER_LIMIT IS NULL OR LOWER_LIMIT <= UPPER_LIMIT)`, `CHECK (USE_FLAG IN ('Y','N'))`.

측정 항목과 FK 로 묶지 않는다 (Para 는 항목 테이블의 키가 아니라 속성이라서). 저장 때 "그 Device 에 그 Para + 조건 항목이 있는지" 를 서비스가 확인한다.

## 5. 서비스 (기존 `GdsEtResult` 에 추가)

같은 E-Test 업무이고, 새 BSL 을 만들면 Service config wellknown 을 또 등록해야 하므로 **기존 Interface / RO / BSL `GdsEtResult` 에 메서드를 추가**한다. DSL / XML 은 기존 방식대로 테이블별 `TQP_GDS_ET_LIMIT` 1쌍.

| 메서드 | 내용 |
| --- | --- |
| `GetEtLimitList(factory, device)` | Device 의 Para + 조건 목록 (측정 항목에서 묶음) + 항목 수 / 단위 + 저장된 Limit + 업로드 값 통계 (값 수 / 최소 / 중앙값 / 99% / 최대 / 현재 Limit 기준 범위 밖 수). READY 파일만 통계에 넣음 |
| `SaveEtLimit(aKey, aRows, user)` | 바뀐 행만 저장 (MERGE). 열: PARA / 조건 / CHAIN(`*`) / Lower / Upper / 사용 / 비고. 숫자 / Lower <= Upper / 단위 / 존재하는 Para 검사 |
| `DeleteEtLimit(aKey, para, cond, chain, user)` | Limit 1행 삭제 (판정 안 함으로 돌아감) |
| `GetEtJudgeSummary(srcFileSeq)` | (조회 창 연동) 파일의 Para 별 / Die 별 Fail 수 / 미측정 수 |

통계 계산은 SQL (`PERCENTILE_CONT`, `MIN` / `MAX`) 로 한다. 화면이 측정값 전체를 내려받지 않는다.

## 6. 화면 설계 — `frmEtLimitSetup` (신규 메뉴 "E-Test Limit 설정")

```text
┌ Factory [SCK ]  Device [OMM_Hybrid ▼]  [조회]   [파일 Limit 가져오기...]   [저장]                         ┐
├ Para Limit (Device 기준)                                                                                  ┤
│ Para               │조건 │단위│항목│값 수 │최소  │중앙값│99%  │최대 │ Lower │ Upper │사용│범위 밖│비고│수정  │
│ Continuity_iforce  │8.0mA│V   │1   │24    │0.0668│0.0724│...  │...  │ 0     │ 5.7   │ ☑ │0      │    │SCK.. │
│ Resistance_iforce  │8.0mA│Ohm │865 │20,760│7.8   │9.5   │221  │719  │       │ 1000  │ ☑ │0      │Open│      │
│ Resistance_iforce  │1.0mA│Ohm │10  │240   │69.6  │152   │257  │6400 │       │ 1000  │ ☑ │1      │Open│      │
│ Leakage_Parallel   │-    │uA  │848 │...   │      │      │     │     │ -50   │ 50    │ ☑ │...    │    │      │
├───────────────────────────────────────────────────────────────────────────────────────────────────────────┤
│ 상태: 저장 안 된 변경 2행 (노란색) / 마지막 조회 ...                                                        │
└───────────────────────────────────────────────────────────────────────────────────────────────────────────┘
```

- 입력 칸: Lower / Upper / 사용 / 비고 만. 나머지는 읽기 전용. 바뀐 행은 노란색, Lower > Upper 같은 잘못된 값은 붉은색 + 저장 막음.
- **범위 밖** 칸: 저장된 Limit 기준으로 서버가 센 값 (READY 파일 전체). 1차는 [저장] 후 다시 조회할 때 갱신한다. 입력하는 즉시 미리 계산하는 기능은 필요하면 추가.
- **파일 Limit 가져오기** (L4): xlsx 를 고르면 3 / 4행 (Upper / Lower) 을 읽어 Para + 조건 별로 값이 하나로 같을 때만 **빈 칸에** 채운다 (N/A 는 건너뜀, 이미 입력된 값은 덮지 않음). 저장은 사용자가 [저장] 을 눌러야 된다. 샘플이면 Continuity 0 / 5.7, Leakage ±50 이 채워지고 Connectivity / Resistance 는 비어 있음.
- 업로드 화면 / 조회 창에서 이 화면을 여는 버튼은 2차.

## 7. 조회 창 연동 (2단계)

| 위치 | 추가 |
| --- | --- |
| 조회 창 측정값 표 | Limit 밖 값 = 붉은 배경, 항목 행에 Lower / Upper 열, 거르기 "Limit 밖 있는 항목만" |
| 조회 창 Die 목록 | Die 별 Limit Fail 항목 수 (DUT Flag 와 나란히) |
| 업로드 화면 등록 목록 | 파일별 Limit Fail Die 수 |

이 단계가 끝나면 (Die, Fail Chain) 목록이 나오므로 F07 Correlation (KLARF Defect 와 연결) 의 E-Test 쪽 입력이 된다.

## 8. 결정 필요

| ID | 내용 | 제안 |
| --- | --- | --- |
| L1 | Limit 단위: Para(+조건) 공통만 vs Chain 개별까지 | **1차 Para 공통**. 테이블은 CHAIN_NAME (`*`) 칸을 미리 둬서 2차에 Chain 개별 추가 (2절 3번 근거) |
| L2 | 같은 Para 의 조건(iforce) 별로 따로 정하나 | **따로** (8.0mA / 1.0mA 값 범위 다름) |
| L3 | Limit 적용 범위 | **Factory + Device** (측정 항목과 같음). 시험 프로그램별 분리는 필요할 때 |
| L4 | 파일 Limit 가져오기 버튼 | **넣음** (빈 칸만 채움, 저장은 사용자) |
| L5 | 경계값 | **Pass** (`Lower <= 값 <= Upper`) |
| L6 | Limit 변경 이력 | **POC 는 보관 안 함** (수정자 / 수정 시각만). 판정 재현이 필요해지면 이력 테이블 추가 |
| L7 | 미측정 (`-`) | **Fail 이 아닌 "미측정" 으로 따로 셈** (샘플 Fail Die #22 / #31 은 Limit 밖 값 없이 미측정만 있음 -> 화면에서 구분해 보여 줌) |

## 9. 구현 단계 (결정 후)

1. DDL `TQP_GDS_ET_LIMIT` + 로컬 DB 적용
2. 서비스: DSL / XML `TQP_GDS_ET_LIMIT` + `GdsEtResult` 메서드 (`GetEtLimitList` / `SaveEtLimit` / `DeleteEtLimit`) + Probe
3. 화면 `frmEtLimitSetup` + 메뉴 (`MNU_DMS_ET_LIMIT`, F03121) + Smoke
4. 조회 창 / 업로드 화면 판정 표시 (7절) + `GetEtJudgeSummary`
5. (L1 2차) Chain 개별 Limit


---

## 10. 사용자 결정과 1차 구현 (2026-10-07)

사용자 결정: "그냥 para 별로 사용자가 구간 선택할 수 있도록 한다 / device 별로 설정 / 대소문자가 약간 다를 수 있으므로 등록 시에는 Upper / 사용자가 사용하고자 하는 enable 기능 / Limit 는 한쪽만 있을 수 있다."

| 항목 | 반영 (4 ~ 8절 설계안과 다른 점) |
| --- | --- |
| 키 | **FACTORY + DEVICE_ID + PARA_NAME** (조건 / Chain 구분 없음, L1 / L2) |
| 대소문자 | Limit PARA_NAME 은 **대문자로 저장** (CHECK `PARA_NAME = UPPER(PARA_NAME)`), 측정 항목과 `UPPER(PARA_NAME)` 으로 비교. 화면 Para 목록도 대문자로 묶음 |
| Enable | `ENABLE_FLAG` Y / N. N 이면 값은 보관, 판정 안 함 |
| 한쪽 Limit | Lower / Upper 각각 NULL 허용, 둘 다 NULL 은 막음 (CHECK) |
| 그 밖 | 경계값 Pass, 미측정 별도, 변경 이력 없음 (수정자 / 시각만), 파일 Limit 가져오기 버튼 **없음** (단순화) |

구현

- DDL `DMS/06_CoreLibSource/GdsMapControl/Persistence/2026-10-07_tqp_gds_et_limit.sql` (로컬 DB 적용): `TQP_GDS_ET_LIMIT` (FACTORY, DEVICE_ID, PARA_NAME PK / LOWER_LIMIT / UPPER_LIMIT BINARY_DOUBLE / UNIT / ENABLE_FLAG / REMARK / 감사).
- XML `TQP_GDS_ET_LIMIT.xml` (SELECT_ET_LIMIT_LIST: Para 묶음 + Limit + READY 파일 값 통계 (값 수 / 미측정 / 최소 / 중앙값 / 99% / 최대) + Enable Limit 범위 밖 수, SELECT_ET_LIMIT, MERGE_ET_LIMIT, DELETE_ET_LIMIT), `TQP_GDS_ET_ITEM.xml` 에 SELECT_ET_ITEM_DEVICE_LIST 추가. DSL `TQP_GDS_ET_LIMIT`.
- 서비스 `GdsEtResult` 추가 메서드: GetEtDeviceList / GetEtLimitList / GetEtLimit / SaveEtLimit (Para 대문자, 숫자 / 한쪽 이상 / Lower <= Upper / Enable Y·N / Device 에 있는 Para / 단위 섞임 검사, 단위는 서버가 채움, MERGE) / DeleteEtLimit. 새 wellknown 불필요.
- 화면: 메뉴 **DMS > E-Test Limit 설정** (`MNU_DMS_ET_LIMIT`, F03121, 순서 24) / `ENGUI/ETest/frmEtLimitSetup`. Factory / Device (측정 항목 있는 Device 목록 + 직접 입력) -> [조회] -> Para 별 통계 + Lower / Upper / Enable / 비고 입력 (바뀐 행 노란색, 잘못된 값 붉은색, Limit 없던 Para 에 값 입력 시 Enable 자동 체크) -> [저장] (바뀐 행만, 값 모두 지운 행은 삭제 질문) / [선택 Limit 삭제].
- 검증: `ENGUI/Verification/Verify-EtResultUpload.ps1` 에 Limit Smoke (`EtLimitSetupFormSmoke.cs`) 추가 -> `ET_LIMIT_FORM_SMOKE=PASS` 22항목 (샘플 기대값 일치: Para 5 / RESISTANCE 값 21,000 최대 6400 / Upper 1000 -> 범위 밖 1 / LEAKAGE_PARALLEL ±50 -> 4 / LEAKAGE_SERIAL Enable 해제 -> 0, 소문자 등록 -> 대문자 / Y, 거부 3종, 삭제 2종). 업로드 Smoke 도 같이 PASS. 화면 이미지 `%TEMP%\ChippacEtResultUploadVerification\limit.png`.
- 다음: 7절 조회 창 / 업로드 화면 판정 표시 (Limit 밖 값 붉은색, Die / 파일별 Fail 수).

---

## 11. 고객 Spec 표 (2026-10-12) 와 지금 Limit 구조 재검토

사용자 지시 (2026-10-12): 고객 Spec 표 (Device U9 / Test Type / Chain name / Parameter / Spec / Unit, TSVET 37 + COWET 39 + FT 43행) 를 주며 "여기 있는 Para 만 정의한다. 나머지는 정의하지 않는다." 그리고 "요청한 내용으로 처리하면 문제 없는지, 현재까지 작업 컨셉에 맞는지 재확인".

### 11.1 표 분석

| 항목 | 내용 |
| --- | --- |
| 정의 단위 | **항목 (헤더 전체) 단위**: `Resistance_iforce=8.0mA@TSVDC_TR_FH`, `Leakage_Serial@TSVDC_ASIC1_N_FH:TSVDC_ASIC1_P_FL` |
| 대상 | TSVDC Resistance (8.0mA) + TSVDC Leakage_Serial 만. Connectivity / Leakage_Parallel / C4DC / PIC / Continuity 는 정의 없음 |
| Spec | Resistance: val > 715 Fail (Upper 715) / Leakage: val = '-' 또는 abs(val) > 50 Fail (Lower -50, Upper 50, '-' Fail). 공정과 관계없이 같은 값 |
| 공정별 목록 | TSVET 37 / COWET 39 (TOP1_P·N RES, TOP2 LKG 추가) / FT 43 — 공정마다 정의 항목이 다름 |
| FT 이름 | `...@TSVDC_HBM_C_P_FH #717` 처럼 뒤에 ` #번호` 가 붙고, Leakage_Serial 은 `N_FL:P_FH` (TSVET / COWET 은 `N_FH:P_FL`) |
| Device | U9 (우리 DEVICE_ID 는 파일명 OMM) |

샘플 (TSVET RH-HS2615-02) 대조: 표의 TSVET 37개가 샘플 헤더에 **모두 그대로 있음** (2,755 항목 중 37개 정의). 이 37개로 판정하면 Fail Die 는 **#44 하나** (TSVDC R_N / L_P / L_N / BOT1_P / BOT1_N / HBM1_N Resistance 718~719 > 715), DUT Flag Fail Die #20 / #22 / #31 / #42 (PIC_COMB Leakage) 는 정의 항목 밖이라 Fail 아님.

### 11.2 지금 구조 (10절, Para 단위) 와 맞지 않는 점

- 10절 Limit 키는 Device + Para. `RESISTANCE_IFORCE` 에 Upper 715 를 넣으면 **Resistance 875개 전부** (C4DC / PIC 포함) 를 판정한다 -> "나머지는 정의하지 않는다" 와 반대.
- 공정마다 정의 항목이 다르다 (COWET 에만 TOP1 RES 등) -> 공정 구분이 필요.
- 14절 결정 "'-' = Fail" 은 정의 항목에만 적용하는 것이 표와 맞다 (정의 안 된 항목은 판정 자체를 안 함).

### 11.3 제안: 항목 단위 Spec (10절 Para 단위를 대체)

| 컬럼 | 내용 |
| --- | --- |
| FACTORY / DEVICE_ID / TEST_PROGRAM(공정) / ITEM_NAME | PK. ITEM_NAME = 헤더 원문을 공백 정리 + 대문자 (등록 시 Upper, 측정 항목과 `UPPER(TRIM(ITEM_NAME))` 비교) |
| PARA_NAME / CHAIN_NAME / CHAIN_NAME2 | 헤더에서 분해 (조회 / 거르기용) |
| LOWER_LIMIT / UPPER_LIMIT | 한쪽만 가능 |
| ENABLE_FLAG | Y / N |
| UNIT / REMARK / SPEC_TEXT | 고객 Spec 원문 보관 |

- 판정: **정의 + Enable 항목만** 판정. 값 < Lower 또는 > Upper 또는 '-' 이면 Fail. 정의 안 된 항목은 "미정의" (판정 안 함).
- 등록: Limit 화면에 **고객 표 붙여넣기** (Excel 에서 6열 복사 -> 붙여넣기) -> Spec 문장 해석 (`val > N` -> Upper N / `abs(val) > N` -> -N ~ N / `"-"` -> '-' Fail) -> 미리보기 -> 저장. 이후 행 단위로 Lower / Upper / Enable 수정 / 삭제.
- Device: 표의 U9 를 붙여넣을 때 지금 Device (OMM) 로 넣는다 (U9 처리는 사용자 검토 중).
- 화면 목록: Device + 공정 고르면 정의 항목 + 업로드 값 통계 + Fail 수 (항목 단위).

### 11.4 확인 필요

- FT 실제 파일 헤더에 ` #717` 이 그대로 들어 있는지 (FT 샘플 필요). 들어 있으면 헤더 원문 그대로 맞추면 되고, GDS Chain 연결 때는 ` #번호` 를 떼야 한다.
- 정의 안 된 항목의 '-' 는 Fail 로 세지 않는 것으로 할지 (제안: 안 셈).


## 12. 사용자 결정과 구현: Parameter 단위 Spec (2026-10-12, 10절 Para 단위를 대체)

### 12.1 사용자 결정 (11.4 / 미정 항목 답변)

| 번호 | 결정 |
| --- | --- |
| 1 | Spec 키 = **디바이스 + 공정(TSVET / COWET / FT) + Parameter**. "전체 Header 정보가 Parameter가 된다" (예: `Resistance_iforce=8.0mA@TSVDC_TL_FH`) |
| 2 | **정의된 항목의 '-' 만 Fail** (정의 안 된 항목은 판정 안 함) |
| 3 | 헤더 원문 그대로 항목명으로 쓰고 **'#' 은 지운다** (FT `...@TSVDC_HBM_C_P_FH #717` -> `...@TSVDC_HBM_C_P_FH`) |
| 4 | DEVICE_ID(U9 / OMM) 는 보류 -> 지금은 파일명 Device(OMM). Spec 표의 Device 열은 화면 Device 로 바꿔 저장 |
| (이전) | 고객 표에 있는 Parameter 만 정의 / 등록 시 대문자 / Enable / Limit 한쪽만 가능 (10절 결정 유지) |

표 건수 정정: 사용자가 준 표는 **TSVET 37 / COWET 40 / FT 42 = 119행** 이다(11.1 의 39 / 43 은 잘못 센 값). 표 원문은 검증용으로 `ENGUI/Verification/EtSpecTable_U9_20261012.txt` 에 저장했다.

### 12.2 DB — `TQP_GDS_ET_LIMIT` 재생성

`06_CoreLibSource/GdsMapControl/Persistence/2026-10-12_tqp_gds_et_limit_item.sql` (로컬 적용 완료, 기존 0행 확인 후 DROP / CREATE)

| 컬럼 | 내용 |
| --- | --- |
| FACTORY / DEVICE_ID / TEST_PROGRAM / ITEM_NAME | PK. ITEM_NAME VARCHAR2(200 CHAR) = 헤더 전체 (연속 공백 1칸 / 끝 `#번호` 제거 / 대문자). CHECK: ITEM_NAME / TEST_PROGRAM 대문자 |
| LOWER_LIMIT / UPPER_LIMIT | BINARY_DOUBLE, 한쪽만 가능 (CHECK 하나 이상 / Lower <= Upper) |
| UNIT | 표 Unit (비어 있으면 측정 항목 단위) |
| ENABLE_FLAG | Y / N |
| SPEC_TEXT | 고객 Spec 원문 (`If val > 715, Fail; otherwise, Pass`) |
| REMARK / 감사 컬럼 | |

측정 항목(TQP_GDS_ET_ITEM) 과는 `UPPER(ITEM_NAME)` 으로 비교한다. 아직 업로드되지 않은 Parameter 도 저장한다(표를 먼저 등록할 수 있게).

### 12.3 판정 / 통계 (쿼리 `SELECT_ET_LIMIT_LIST`)

- 대상: Device + 공정의 **정의 Parameter 만**. 통계는 그 공정의 READY 파일 값.
- Fail 수 = Enable 이면 (값 < Lower 또는 값 > Upper) + '-' 값. 경계값은 Pass. Enable N 이면 0.
- 데이터 = 그 공정 READY 파일에 값이 있으면 Y (측정 항목 마스터는 Device 단위라 공정 구분이 없어서 값 기준으로 판단).
- 업로드 / 조회 화면의 '-' 표시는 Fail 이 아니라 **'-' 값** 으로 되돌렸다(결정 2: 정의 안 된 항목의 '-' 는 Fail 아님). 조회 창 '-' 셀은 회색.

### 12.4 Spec 표 붙여넣기 (`ENGUI/ETest/EtSpecTable.cs`)

- 입력: Excel 에서 복사한 탭 구분 텍스트. 첫 줄이 제목(Test Type / Chain name 포함)이면 제목으로 열을 찾고, 아니면 Device / Test Type / Chain name / Parameter / Spec / Unit 순서.
- Spec 문장 해석 (공백 / 대소문자 무시, "Fail" 앞 조건만, `or` 로 여러 조건):

| 문장 | 결과 |
| --- | --- |
| `val > N` | Upper N |
| `val < N` | Lower N |
| `abs(val) > N` | Lower -N / Upper N |
| `val = "-"` | 별도 Limit 없음 ('-' 는 정의 항목이면 항상 Fail) |
| 그 밖(`>=` 등) | 오류 행 -> **하나라도 있으면 저장하지 않음** |

- Chain name -> Parameter: `EtResultFileParser.NormalizeHeader`(공백 / `#번호` 제거) + 대문자. 서버 BSL 도 같은 규칙으로 다시 정리한다.
- 같은 공정에 같은 Parameter 가 두 번 나오면 오류 행.
- 저장: 공정별 SaveEtLimit(MERGE, Enable Y). 이미 있는 Parameter 는 Lower / Upper / 단위 / Spec 원문을 덮어쓰고 비고는 지운다(확인 질문에 표시).

### 12.5 서비스 (`iGdsEtResult` 변경)

| 메서드 | 내용 |
| --- | --- |
| `GetEtTestProgramList(factory, device)` | 신규. 업로드 파일 + Spec 이 있는 공정 |
| `GetEtLimitList(factory, device, 공정)` | 공정 인자 추가. 열: ITEM_NAME / CHAIN_NAME / CHAIN_NAME2 / REP_CHAIN_NAME / REP_CHAIN_NAME2 / UNIT / ITEM_UNIT / IN_DATA / VALUE_COUNT / NOT_TESTED_COUNT / MIN / MEDIAN / P99 / MAX / FAIL_COUNT / LOWER / UPPER / ENABLE / SPEC_TEXT / REMARK / 수정 |
| `GetEtLimit(factory, device, 공정)` | 판정용 |
| `SaveEtLimit(aKey{factory, device, 공정}, aRows 7열)` | 0 Parameter / 1 Lower / 2 Upper / 3 Enable / 4 Unit / 5 Spec 원문 / 6 비고. '@' 없으면 거부, 같은 Parameter 두 번('#' 만 다른 경우 포함) 거부 |
| `DeleteEtLimit(aKey{factory, device, 공정, Parameter})` | 4항목 |

10절 Para 단위 메서드 / 쿼리(키 2개, 5열)는 없앴다. 그 화면은 배포 전이었고 테이블도 다시 만들었다.

### 12.6 화면 (`frmEtLimitSetup`, 메뉴 MNU_DMS_ET_LIMIT 그대로)

- 위: Factory / Device / **공정** / [조회] [저장] [선택 행 삭제] [Spec 표 붙여넣기]
- [Spec (Parameter)] 탭: Parameter / 대표 Chain / 단위 / 데이터 / 값 수 / '-' 수 / 최소 / 중앙값 / 99% / 최대 / **Lower / Upper / Enable** / Fail / Spec 원문 / **비고** / 수정. 바뀐 행 노란색, 잘못된 값 붉은색, Lower / Upper 를 모두 지우면 삭제 질문. 데이터 없음 행은 회색.
- [Chain 묶기 (대표 Chain)] 탭: 13절.

## 13. Chain 묶기 — E-Test Chain -> 대표 Chain (2026-10-12)

> **대체됨 (같은 날 사용자 지시 "대표 chain 보다는 그냥 묶는 기능")** -> 15절. 아래는 기록용이다.

### 13.1 사용자 요청

"E test 결과에서 chain 정보를 추출할 수 있다. 현재 Setup 되는 Chain은 In 값에 해당하는 Chain 정보를 대표로 두고 있다. 따라서 E Test Spec 설정 시 2개의 Chain을 대표 Chain으로 묶는 기능이 필요하다."

### 13.2 확인한 데이터

| 구분 | 이름 예 | 비고 |
| --- | --- | --- |
| E-Test Chain (헤더 `@` 뒤 / `:` 뒤) | `TSVDC_ASIC1_P_FH`, `TSVDC_ASIC1_P_FL` | 샘플 1,306개 = `_FH` 879 + `_FL` 427. 한 물리 Chain 의 양 끝 Pad 이름 |
| GDS Chain (TQP_GDS_CHAIN_VER.CHAIN_NAME) | `TSVDC_TOP1_P`, `TSVDC_L_N` | Input Label 기준 이름 (2026-10-08 CLAUDE-049 자동 이름) |

-> E-Test Chain 2개(`_FH` / `_FL`)를 GDS Chain 이름 1개(대표 Chain)로 묶는다. Leakage_Serial 처럼 Chain 이 2개인 항목(`N_FH:P_FL`)은 각 Chain 의 대표 Chain 을 `대표1 : 대표2` 로 보인다.

### 13.3 DB — `TQP_GDS_ET_CHAIN_MAP` (신규)

`Persistence/2026-10-12_tqp_gds_et_chain_map.sql` (로컬 적용 완료)

| 컬럼 | 내용 |
| --- | --- |
| FACTORY / DEVICE_ID / CHAIN_NAME | PK. CHAIN_NAME = E-Test Chain(대문자, 64 CHAR = ET_ITEM.CHAIN_NAME) |
| REP_CHAIN_NAME | 대표 Chain = GDS Chain 이름 (200 CHAR = CHAIN_VER.CHAIN_NAME). 문자열 연결, FK 없음 |
| REMARK / 감사 컬럼 | |

- 같은 REP_CHAIN_NAME 을 가진 행끼리 "묶음". 보통 2개지만 개수를 막지는 않는다(2개가 아니면 화면이 묻는다).
- **Device 단위, 공정 무관** (물리 Chain 은 공정이 달라도 같다).
- 키가 E-Test Device(OMM) 이고 GDS Device 는 U9 라(결정 4 보류) GDS Chain 과는 이름으로만 맞춘다. 화면은 대표 Chain 이 Factory 의 GDS Chain(현재 READY 버전)에 있으면 그 GDS Device 를 보여 준다.
- 대표 Chain 후보: CLAUDE-055 Sub Device 연결 `RO.GdsMapView.GetMapDeviceBySub(factory, E-Test Device)` 로 GDS 대표 Device(예: OMM -> U9)를 찾아 그 Device 의 GDS Chain 만 보인다. 서비스 미배포 / 연결 없음이면 Factory 전체 GDS Chain (화면 안내 문구로 구분).

### 13.4 서비스 / 쿼리 (`TQP_GDS_ET_CHAIN_MAP.xml`)

| 메서드 | 쿼리 | 내용 |
| --- | --- | --- |
| `GetEtChainMapList(factory, device)` | SELECT_ET_CHAIN_MAP_LIST | ET_ITEM 의 CHAIN_NAME / CHAIN_NAME2(대문자) + 저장된 대표 Chain + 묶음 수 + GDS Device |
| `GetEtGdsChainList(factory)` | SELECT_ET_GDS_CHAIN_LIST | 대표 Chain 후보 (GDS Chain 이름) |
| `SaveEtChainMap(aKey{factory, device}, aRows{Chain, 대표, 비고})` | MERGE_ET_CHAIN_MAP | Chain 대문자, ':' / '@' 거부, 대표 Chain 필수 |
| `DeleteEtChainMap(aKey{factory, device, Chain})` | DELETE_ET_CHAIN_MAP | 묶음 해제 |

### 13.5 화면 ([Chain 묶기] 탭)

- [찾기] 글자로 Chain 거르기 -> Chain 2개 고르기(Ctrl / Shift) -> 대표 Chain 칸에 **제안 이름(끝 `_FH` / `_FL` 을 뗀 공통 이름, GDS 후보에 있으면 그 표기)** 자동 입력 -> [선택 Chain 에 지정] -> [Chain 저장].
- [FH / FL 짝 제안]: 대표 Chain 이 없는 Chain 전체에 뗀 이름을 채운다(저장 전, 확인 후 [Chain 저장]).
- [선택 묶음 해제]: 대표 Chain 을 비우고 [Chain 저장] 하면 삭제.
- 저장하면 [Spec] 탭의 대표 Chain 열도 다시 읽는다.

## 14. 검증 (2026-10-12)

| 검사 | 결과 |
| --- | --- |
| TEMP 빌드 (Interface / DSL / BSL / RO / ENGUI) | 성공. ENGUI 경고 4건은 기존 화면(frmCommonalityStudy / frmDieStackGallery / frmDefectMapGallery) |
| 업로드 화면 Smoke (`EtResultUploadFormSmoke`) | PASS ('-' 문구 되돌림 반영) |
| Spec 화면 Smoke (`EtLimitSetupFormSmoke`, 다시 작성) | PASS: 표 119행 해석 / `>=` 거부 / 공정 COWET·FT·TSVET / TSVET 37행 모두 데이터 있음 / **Fail 6항목 (#44 RES 718~719)** / FT 42행 '#' 없음·데이터 없음 / Enable 해제·Upper 720 -> Fail 0 / 행 삭제 / 서비스 거부 6종 / Chain 2개 묶기·제안 TSVDC_ASIC1_P·Spec 탭 `? : TSVDC_ASIC1_P` / FH·FL 짝 제안 1,304개 / 묶음 해제 |
| 서비스 Probe (`GdsEtResultProbe.ps1 -BinDir <TEMP bin>`) | 19/19 PASS. `-BinDir` 없이 돌리면 101_ServerBin(배포 전 DLL)을 써서 COWET 단계에서 실패한다 |

## 15. Chain 묶기 + Setup Chain 으로 찾기 (2026-10-12, 13절 대표 Chain 을 대체)

### 15.1 사용자 지시와 컨셉

"대표 chain 보다는 그냥 묶는 기능으로 하고 Setup 된 Chain에서 찾으면 묶인 Chain에 대한 Limit 등의 정보를 전부 찾는다. 컨셉 확인 후 설계 하고 개발"

| 구분 | 컨셉 |
| --- | --- |
| 묶기 | 대표 이름 없이 E-Test Chain 여러 개(보통 한 물리 Chain 의 양 끝 `_FH` + `_FL`)를 **같은 묶음 번호**로 둔다. Chain 1개는 묶음 1개에만 들어간다. Device 단위, 공정 무관 |
| Setup Chain | GDS Map 에서 Setup(저장)한 Chain = `TQP_GDS_CHAIN_VER` 현재 READY 버전 이름. E-Test Device(OMM)의 GDS Device(U9)는 CLAUDE-055 Sub Device 연결로 찾는다 |
| 찾기 규칙 | ① **같은 이름**의 E-Test Chain (Input Label 이 그대로 Chain 이름인 경우, 예: `TSVDC_ASIC1_P_FL`) ② ①이 없을 때만 **이름 + `_FH` / `_FL`** (예: Setup `PIC_COMB12_M4` -> `PIC_COMB12_M4_FH` / `_FL`) ③ 찾은 Chain 이 묶여 있으면 **같은 묶음의 Chain 전부** |
| 찾는 정보 | 찾은 Chain 이 Chain 1 / Chain 2 로 들어간 **모든 공정의 Spec**(Lower / Upper / Enable / Spec 원문) + **Spec 없는 측정 항목** + READY 파일 값 통계(파일 수 / 값 수 / '-' 수 / 최소 / 최대) + **Fail 수**(Enable Spec 범위 밖 + '-') |

로컬 데이터 확인: SCK / U9 Setup Chain 11개 중 `PIC_COMB12_M4` 는 E-Test `PIC_COMB12_M4_FH` / `_FL`, `TSVDC_L_N` 은 `TSVDC_L_N_FH` 로 ②규칙에 걸린다. Sub Device 등록 OMM / OMM_Hybrid -> U9 있음.

### 15.2 DB — `TQP_GDS_ET_CHAIN_GROUP` (13절 `TQP_GDS_ET_CHAIN_MAP` 대체)

`Persistence/2026-10-12_tqp_gds_et_chain_group.sql` (로컬 적용 완료: CHAIN_MAP DROP(0행, 없으면 건너뜀) + 시퀀스 `ET_CHAIN_GROUP_SEQ` + 테이블)

| 컬럼 | 내용 |
| --- | --- |
| FACTORY / DEVICE_ID / CHAIN_NAME | PK. CHAIN_NAME = E-Test Chain (대문자 CHECK, 64 CHAR) |
| GROUP_SEQ | 묶음 번호 (같은 값끼리 한 묶음, 묶음마다 2개 이상). 색인 FACTORY + DEVICE_ID + GROUP_SEQ |
| REMARK | 묶은 방법 (`수동 묶음` / `FH-FL 자동 묶음`) |
| 감사 컬럼 | |

`2026-10-12_tqp_gds_et_chain_map.sql` 은 머리에 "대체됨" 표시만 남겼다(다시 실행하지 않음). `TQP_GDS_ET_CHAIN_MAP.xml` / DSL `TQP_GDS_ET_CHAIN_MAP.cs` 는 지우고 `TQP_GDS_ET_CHAIN_GROUP.xml` / `.cs` 로 바꿨다(배포 전).

### 15.3 쿼리 / 서비스

| 메서드 (`iGdsEtResult`) | 쿼리 | 내용 |
| --- | --- | --- |
| `GetEtChainGroupList(factory, device)` | SELECT_ET_CHAIN_GROUP_LIST | E-Test Chain(측정 항목 CHAIN_NAME / CHAIN_NAME2) + 묶음 번호 / 묶음 Chain 목록(`A + B`) / 묶음 수 |
| `GetEtGdsChainList(factory)` | SELECT_ET_GDS_CHAIN_LIST | Setup Chain 목록 (GDS Device 거르기는 화면) |
| `CreateEtChainGroups(aKey{factory, device}, aRows{묶음 구분값, Chain, 비고})` | SELECT_NEXT_ET_CHAIN_GROUP_SEQ / MERGE_ET_CHAIN_GROUP / DELETE_ET_CHAIN_GROUP_SINGLE | 구분값마다 새 묶음 번호. 이미 묶인 Chain 은 옮기고, 1개만 남은 이전 묶음은 지운다. 묶음당 2개 미만 / ':' '@' / 같은 Chain 두 번 거부 |
| `DeleteEtChainGroup(aKey{factory, device, Chain})` | DELETE_ET_CHAIN_GROUP_BY_CHAIN | Chain 이 든 묶음 전체 해제 |
| `GetEtChainMatch(factory, device, setupChain)` | SELECT_ET_CHAIN_MATCH + SELECT_ET_CHAIN_GROUP_MEMBER | 15.1 찾기 규칙. 열 CHAIN_NAME / MATCH_TYPE(EXACT / SUFFIX / GROUP) / GROUP_SEQ. E-Test Chain 후보 = 측정 항목 / 묶음 / Spec 의 Chain |
| `GetEtChainSpecInfo(factory, device, setupChain)` | SELECT_ET_LIMIT_BY_CHAIN (Chain 마다) | 찾은 Chain 마다 조회 후 같은 (공정, Parameter) 는 한 행으로 합침(MATCH_CHAIN 에 Chain 추가). 정렬: Spec 있는 행(공정, Parameter) -> Spec 없는 측정 항목 |

- `SELECT_ET_LIMIT_LIST` 에서 대표 Chain 조인을 뺐다(바인딩 f1 d1 t1 f2 d2 f3 d3 t3). Spec 탭의 묶음 / Setup Chain 열은 화면이 묶음 목록으로 계산한다.
- **다른 화면 인계**: GDS Map 조회 / KLARF 연관 분석 등에서 Setup Chain 의 E-Test 정보가 필요하면 `RO.GdsEtResult.GetEtChainSpecInfo(factory, E-Test Device, Setup Chain 이름)` 하나로 찾는다. E-Test Device 는 GDS 대표 Device 의 Sub Device 목록(`GdsMapView.GetSubDeviceList`)으로 정한다. 이번에는 GDS 화면(다른 세션 소유)을 고치지 않았다.

### 15.4 화면 (`frmEtLimitSetup`)

| 탭 | 내용 |
| --- | --- |
| Spec (Parameter) | 열 `대표 Chain` -> **`묶음`**(Chain 이 묶였으면 `A + B`, Chain 2개 항목은 `Chain1 쪽 : Chain2 쪽`) + **`Setup Chain`**(연결되는 GDS Setup Chain). 상태 줄 "Setup Chain 없음 N" |
| Chain 묶기 | [찾기] 거르기 / **[선택 Chain 묶기]**(2개 이상, 바로 저장. 이미 묶인 Chain 이 있거나 2개가 아니면 묻는다) / **[FH / FL 짝 자동 묶기]**(안 묶인 `이름_FH` + `이름_FL` 짝 전부, 묻고 한 번에 저장) / **[선택 묶음 해제]**. 열: E-Test Chain / 항목 수 / 묶음 Chain / 묶음 수 / Setup Chain / 비고 / 수정 (읽기 전용) |
| **Setup Chain 으로 찾기** (신규) | 왼쪽 Setup Chain 목록(GDS Device 의 Setup Chain) 클릭 또는 이름 입력 + [찾기] -> 위 안내: `Setup Chain X -> E-Test Chain: A (같은 이름), B (묶음) / Spec n건 (공정들) / Spec 없는 측정 항목 m건 / Fail 합`. 표: 공정 / Parameter / 찾은 Chain / 정의 / Lower / Upper / 단위 / Enable / Fail / 파일 / 값 수 / '-' 수 / 최소 / 최대 / Spec 원문. Spec 행 더블클릭 -> [Spec] 탭 그 공정 / Parameter 로 이동 |

### 15.5 검증 (2026-10-12)

| 검사 | 결과 |
| --- | --- |
| TEMP 빌드 | 성공 (ENGUI 경고 4 = 기존 화면) |
| 쿼리 (로컬 SCK / OMM) | 6개 문법 / 바인딩 순서 확인. Setup `PIC_COMB12_M4` -> `_FH` / `_FL` SUFFIX 2행 |
| Spec Smoke (`EtLimitSetupFormSmoke`) | **PASS**. 추가 검사: 묶기 전 `TSVDC_ASIC1_P_FL` 찾기 = 같은 이름 1 / Spec 2 (COWET, TSVET) / Spec 없는 항목 1 -> 2개 묶기(질문 없음) -> Setup Chain 열 _FH 도 연결 -> 같은 찾기 = 같은 이름 + 묶음 / **Spec 6 (COWET, FT, TSVET)** / Spec 없는 항목 2 (Connectivity _FH / Leakage_Parallel _FL) / FT Leakage `N_FL:P_FH` 는 묶인 _FH 로 찾음 / 행 더블클릭 이동(같은 공정, 다른 공정) / `PIC_COMB12_M4` SUFFIX / 없는 이름 0행 / 서비스 결과 = 화면 / **FH-FL 자동 426쌍** / 이미 묶인 Chain 옮기기 + 1개 남은 묶음 정리 / 해제 / 서비스 거부 3종. 끝나고 PROBE 데이터 정리 확인 |
| 업로드 Smoke / Probe | PASS / 19/19 (`-BinDir` TEMP bin) |
