# E-Test Para Limit 설정 화면 설계

작성: 2026-10-07 Claude (CLAUDE-029) / 상태: **사용자 결정 반영 / 1차 구현 완료 (10절)** — 4 ~ 8절은 결정 전 설계안 (10절이 우선)

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
