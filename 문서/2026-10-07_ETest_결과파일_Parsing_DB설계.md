# E-Test 결과 파일 분석 / Parsing / DB 저장 설계

작성: 2026-10-07 Claude (CLAUDE-029) / 상태: **설계안 (소스 / DB 변경 없음, 사용자 결정 대기)**

> **범위 (2026-10-07 사용자 지시, 최신):** 이 세션은 **E-Test 만** 다룬다. KLARF 파일은 다른 세션에서 다룬다. DDL / 설계도 E-Test 결과에 대해서만 진행한다.
> - 현재 확정 / DDL 대상: **9절 E-Test 파일 기준 정보 (`TQP_GDS_SRC_FILE`)**
> - 1 ~ 2절 (파일 분석) 은 근거. 3.2 ~ 3.6 / 4 ~ 5절 (항목 / Limit / Die / 측정값) 은 **나중 설계 참고안**, 결정 D1 ~ D8 도 그때 다시 묻는다.
> - 8절 (KLARF 연결) 은 **KLARF 세션 참고 기록**이며 이 세션 범위가 아니다.

근거 파일: `SampleFile/26_10_06 Sample File/CAI_OMM_ATV_A0_TSVET_20260609161940_RH-HS2615_RH-HS2615-02.xlsx`
분석 방법: Python openpyxl 로 전체 셀을 읽어 집계 (아래 숫자는 모두 실측값). 추정은 **(추정)** 으로 표시한다.

사용자 지시 (2026-10-07)

- 헤더 `Continuity_iforce=8.0mA@C4DC_210_3_FH` 에서 `Continuity_iforce` = Para 이름, `C4DC_210_3_FH` = Chain 이름.
- Upper Limit / Lower Limit 은 파일에서 쓰지 않는다. 추후 우리가 **Limit 설정 화면**을 만든다.
- 정형화해서 DB 에 저장한다.

---

## 1. 파일 구조 분석

### 1.1 전체 모양

| 항목 | 실측 |
| --- | --- |
| 시트 | 1개, 이름 `DUT Summary` |
| 크기 | 29행 x 2,766열 |
| 고정 열 | A ~ K (11개) |
| 측정 열 | L ~ 끝 **2,755개** (`Tests Executed` 값 2755 와 일치) |
| Die 행 | 6 ~ 29행, **24 Die** |
| 측정값 | 66,120셀 = 숫자 66,103 + 문자 `-` 17 |

### 1.2 행 구성 (1 ~ 5행은 머리 정보, 6행부터 Die)

| 행 | A열 | 고정 열(B~K) | 측정 열(L~) |
| --- | --- | --- | --- |
| 1 | 빈 값 | 고정 열 이름 | **측정 항목 헤더** (`Para@Chain`) |
| 2 | `Test Number` | 빈 값 | Test 번호 (항목 그룹 번호, 열마다 유일하지 않음) |
| 3 | `Upper Limit` | 빈 값 | 상한 (숫자 또는 `N/A`) -> **저장 안 함** |
| 4 | `Lower Limit` | 빈 값 | 하한 (숫자 또는 `N/A`) -> **저장 안 함** |
| 5 | `Unit` | 빈 값 | 단위 `V` / `Ohm` / `uA` |
| 6~ | `#3` 형식 DUT 번호 | Die 정보 | 측정값 |

### 1.3 고정 열 (Die 정보)

| 열 | 헤더 | 샘플 값 | 분석 | 저장 |
| --- | --- | --- | --- | --- |
| A | (없음) | `#3`, `#4` ... | DUT 번호. 24행 모두 다름 | O `DUT_NO` = 3 |
| B | File ID | 0 (전부) | 의미 없음 | X |
| C | Part ID | 빈 값 (전부) | 제품 정보 없음 -> Device 는 업로드 화면에서 선택 | X |
| D | Test Head - Site | `Head 1 - Site 1` (전부) | 장비 Site | O (원문, 추후 다중 Site 대비) |
| E | Tests Executed | 2755 (전부) | 수행 항목 수 | O |
| F | Test Time | `9469 ms` | Die 별 시험 시간 | O `TEST_TIME_MS` = 9469 |
| G | Hardware Bin | `Bin 1` / `Bin 5` | | O `HW_BIN` = 1 |
| H | Software Bin | `Bin 1` / `Bin 5001` | | O `SW_BIN` = 5001 |
| I | Wafer ID | `RH-HS2615` (전부) | 파일명 기준 **Lot ID** 와 같은 값 (2.1 참고) | 파일 헤더에서 비교만 |
| J | (X, Y) | `(3, 1)` | Die 좌표. X 1~7 / Y 1~10 사용 | O `DIE_X`, `DIE_Y` |
| K | DUT Flag | `Pass - 0x00` / `Failed - 0x08` | Pass 20 / Fail 4 | O `PASS_FLAG` (P/F) + `FLAG_CODE` (`0x08`) |

### 1.4 측정 항목 헤더 형식

형식: `{Para}[_{조건이름}={조건값}]@{Chain1}[:{Chain2}]`

| Para | 조건 | Chain | 열 수 | Test No | 단위 | 파일 Limit (Upper / Lower) |
| --- | --- | --- | --- | --- | --- | --- |
| Continuity | iforce=8.0mA | 1개 | 1 | 1 | V | 5.7 / 0 |
| Connectivity | iforce=8.0mA | 1개 | 865 | 4 | V | N/A |
| Connectivity | iforce=1.0mA | 1개 (PIC_EDM_*) | 10 | 5 | V | N/A |
| Resistance | iforce=8.0mA | 1개 | 865 | 2 | Ohm | N/A |
| Resistance | iforce=1.0mA | 1개 (PIC_EDM_*) | 10 | 3 | Ohm | N/A |
| Leakage_Parallel | 없음 | 1개 | 848 | 6 | uA | 50 / -50 |
| Leakage_Serial | 없음 | **2개 (`A:B`)** | 156 | 8 ~ 17 | uA | 50 / -50 |

확인 결과

- 정규식 `^(?<para>[^=@]+?)(?:_(?<cond>[A-Za-z]+)=(?<cval>[^@]+))?@(?<chain>.+)$` 로 **2,755개 모두 분해됨** (실패 0). Para + 조건 + Chain 조합 중복 0.
- 사용자 표현의 Para 이름 `Continuity_iforce` 는 **Para `Continuity` + 조건 `iforce=8.0mA`** 로 나눠 저장한다. 같은 Para 가 8.0mA / 1.0mA 두 조건으로 측정되기 때문 (Connectivity / Resistance 각각 10개). 원문 헤더는 그대로 함께 저장해 사용자가 아는 형태로도 찾을 수 있게 한다.
- `Leakage_Parallel`, `Leakage_Serial` 은 조건이 없다. Para 이름 안에 `_` 가 있으므로 `_` 로 나누면 안 되고 `=` 가 있을 때만 조건으로 본다.
- **Leakage_Serial 은 Chain 2개** (`PIC_COMB11_M4_FH:PIC_COMB11_M4_FL`) — 두 Chain 사이 누설. 두 번째 Chain 을 별도 컬럼으로 저장한다.
- 열 순서: Continuity 1 -> Connectivity 875 -> Resistance 875 -> Leakage_Parallel 848 -> Leakage_Serial 156 (그룹별 연속).

### 1.5 Chain 이름

| 항목 | 실측 |
| --- | --- |
| 서로 다른 Chain 이름 | 1,306개 (헤더 2,755개 안에서) |
| 접두어 | C4DC 1,244 / TSVDC 38 / PIC 24 |
| 접미어 | `_FH` 879 / `_FL` 427 (FL 이 있는 427개는 모두 FH 짝이 있음) |
| 최대 길이 | 19자 -> 기존 `TQP_GDS_CHAIN.CHAIN_CODE` VARCHAR2(32) 안에 들어감 |
| Connectivity Chain vs Leakage_Parallel Chain | 875 / 848, 서로 다른 Chain 이 섞임 (같은 목록 아님) |

`_FH` / `_FL` 의 뜻 (Force High / Force Low 단자 또는 Front 면 구분 등) 은 **확인 필요**. 이번 설계는 접미어를 떼지 않고 **Chain 이름 전체**를 그대로 저장한다.

### 1.6 측정값 특성

| Para | 최소 ~ 최대 | 비고 |
| --- | --- | --- |
| Continuity | 0.0668 ~ 0.0876 V | |
| Connectivity | 0.0624 ~ 6.4 V | 6.4 V 1건 = #4 PIC_EDM_T_FH (1.0mA). 전압 한계값으로 보임 **(추정 Open)** |
| Resistance | 7.8 ~ 6400 Ohm | 정수 값 1,462셀 섞임 -> 숫자형으로 통일 |
| Leakage_Parallel | -951 ~ 747 uA | |
| Leakage_Serial | -747 ~ 0.345 uA | |

- Resistance 와 Connectivity / iforce 비교: 21,000쌍 중 20,943쌍 (99.7%) 일치, 57쌍은 차이 있음 -> **계산으로 대체하지 않고 두 값 모두 원본 저장**.
- 문자 `-` 17셀: Fail Die #20 / #22 / #31 의 `PIC_COMB11/12_M4` Leakage 항목에만 있음 -> 미측정 (중단) **(추정)**. 값은 NULL + 상태 `NOT_TESTED` 로 저장.
- Fail Die 4개를 파일 Limit 으로 다시 판정하면 #20 2개 / #42 3개 항목이 Limit 밖, #22 / #31 은 Limit 밖 항목이 없고 `-` 만 있다. 즉 **DUT Flag 의 Fail 원인이 값만으로는 다 설명되지 않는다** -> DUT Flag / Bin 은 원본 그대로 저장하고, 항목별 Pass/Fail 은 우리 Limit 으로 다시 판정한다 (5절).
- Connectivity / Resistance 는 파일 Limit 이 모두 `N/A` 라서 **현재 파일만으로는 Open Fail 판정이 불가능**하다. Correlation (Die, Fail Chain) 을 하려면 Limit 설정 화면이 반드시 필요하다.

---

## 2. 파일명 / 파일 헤더

### 2.1 파일명 분해

`CAI_OMM_ATV_A0_TSVET_20260609161940_RH-HS2615_RH-HS2615-02.xlsx`

| 위치 | 값 | 해석 | 확정 여부 |
| --- | --- | --- | --- |
| 1~4 | `CAI_OMM_ATV_A0` | 사이트 / 장비 / 단계 코드 | **확인 필요** -> `FILE_PREFIX` 원문 저장 |
| 5 | `TSVET` | 시험 프로그램 (TSV E-Test) | (추정) `TEST_PROGRAM` |
| 6 | `20260609161940` | 시험 일시 2026-06-09 16:19:40 | (추정) `TEST_START_TIME` |
| 7 | `RH-HS2615` | Lot ID (I열 Wafer ID 값과 같음) | (추정) `LOT_ID` |
| 8 | `RH-HS2615-02` | Wafer ID (Lot + `-02` 슬롯) | (추정) `WAFER_ID` |

파일명을 `_` 로 자르면 Lot / Wafer 에 `_` 가 들어올 때 깨진다. **뒤에서부터** 자른다: 마지막 = Wafer, 그 앞 = Lot, 그 앞 = 14자리 일시, 그 앞 = 프로그램, 나머지 = 접두어. 14자리 일시 형식이 아니면 업로드를 막는다.

I열 Wafer ID 값이 파일명 Lot ID 와 다르면 경고만 표시한다 (샘플에서는 같음).

### 2.2 Device / Factory

파일에 제품 정보가 없다 (Part ID 빈 값). 업로드 화면에서 **Factory / Device 를 선택**하게 한다. GDS Map (`TQP_GDS_MAP`, PK FACTORY + DEVICE_ID) 과 같은 키를 쓰므로 Chain 연결이 그대로 된다.

### 2.3 좌표 기준

2026-10-06 데이터기준 협의 결과 E-Test 는 **Back side** 기준이고, Notch 회전 / Die 인덱스 대응은 연구소 좌표 대응 테이블로 별도 처리한다. 이 설계는 파일의 (X, Y) 를 **변환 없이 원본 그대로** 저장한다.

---

## 3. DB 설계

### 3.1 전체 관계

```text
TQP_GDS_MAP (FACTORY, DEVICE_ID)              <- 기존
  ├─ TQP_GDS_CHAIN (CHAIN_CODE)               <- 기존, 이름으로만 대응 (FK 없음)
  │
  ├─ TQP_ET_ITEM   측정 항목 마스터 (Para + 조건 + Chain)   <- 신규
  │     └─ TQP_ET_LIMIT  Limit 설정 (추후 화면)              <- 신규
  │
  └─ TQP_ET_FILE   파일 1개 = Wafer 1장 시험 1회            <- 신규
        └─ TQP_ET_DIE    Die 정보 (Bin / Flag / 좌표)       <- 신규
              └─ TQP_ET_VALUE  Die x 항목 측정값            <- 신규
```

- 이름 접두어 `TQP_ET_` (기존 `TQP_GDS_` 와 같은 규칙). 키는 기존 방식대로 NUMBER + 같은 이름 SEQUENCE (`ET_FILE_SEQ`, `ET_ITEM_SEQ`).
- 측정값은 **세로형 (Die x 항목 1행)** 으로 저장한다. Chain / Para 로 조회 / 집계 / Limit 판정이 SQL 로 바로 된다. 행 수는 파일당 66,120 (24 Die 기준), 전체 Wafer (예: 76 Die) 라도 약 21만 행으로 기존 GDS Bulk 저장 (35만 행 24.5초) 범위 안이다.
- E-Test Chain 이름과 GDS Chain 은 FK 로 묶지 않는다. GDS Chain 이 아직 없어도 E-Test 는 올라가야 하기 때문. 연결은 조회 때 `TQP_ET_ITEM.CHAIN_NAME = TQP_GDS_CHAIN.CHAIN_CODE` 로 한다.

### 3.2 TQP_ET_ITEM (측정 항목 마스터)

Device 단위로 항목을 한 번만 등록하고 파일마다 재사용한다. 새 파일에 없던 헤더가 나오면 추가한다.

| 컬럼 | 형식 | 설명 / 샘플 |
| --- | --- | --- |
| FACTORY | VARCHAR2(30 CHAR) PK | |
| DEVICE_ID | VARCHAR2(50 CHAR) PK | |
| ET_ITEM_SEQ | NUMBER PK | 시퀀스 |
| ITEM_NAME | VARCHAR2(200 CHAR) NOT NULL | 원문 헤더 `Continuity_iforce=8.0mA@C4DC_210_3_FH` (최대 54자 실측). UNIQUE (FACTORY, DEVICE_ID, ITEM_NAME) |
| PARA_NAME | VARCHAR2(50 CHAR) NOT NULL | `Continuity` / `Leakage_Serial` |
| COND_NAME | VARCHAR2(20 CHAR) | `iforce` (없으면 NULL) |
| COND_VALUE | VARCHAR2(20 CHAR) | `8.0mA` 원문 |
| COND_NUM | BINARY_DOUBLE | 8.0 (숫자 부분, 정렬 / 계산용) |
| COND_UNIT | VARCHAR2(10 CHAR) | `mA` |
| CHAIN_NAME | VARCHAR2(64 CHAR) NOT NULL | `C4DC_210_3_FH` |
| CHAIN_NAME2 | VARCHAR2(64 CHAR) | Leakage_Serial 두 번째 Chain, 없으면 NULL |
| UNIT | VARCHAR2(10 CHAR) | `V` / `Ohm` / `uA` |
| TEST_NO | NUMBER(10) | 2행 Test Number |
| DISPLAY_ORDER | NUMBER(10) | 처음 등록한 파일의 열 순서 |
| 감사 컬럼 | | CREATE_USER / CREATE_TIME / UPDATE_USER / UPDATE_TIME (기존 규칙) |

인덱스: (FACTORY, DEVICE_ID, CHAIN_NAME), (FACTORY, DEVICE_ID, PARA_NAME).

같은 ITEM_NAME 인데 단위 (5행) 가 이전 등록과 다르면 업로드를 막고 메시지를 보여준다.

### 3.3 TQP_ET_LIMIT (Limit 설정, 화면은 추후)

파일의 Upper / Lower Limit 은 쓰지 않고 이 테이블만 판정 기준으로 쓴다.

| 컬럼 | 형식 | 설명 |
| --- | --- | --- |
| FACTORY / DEVICE_ID | PK | |
| PARA_NAME | VARCHAR2(50 CHAR) PK | `Resistance` |
| COND_VALUE | VARCHAR2(20 CHAR) PK | `8.0mA` / 조건 없음은 `-` |
| CHAIN_NAME | VARCHAR2(64 CHAR) PK | 특정 Chain 또는 `*` (Para 공통) |
| LOWER_LIMIT | BINARY_DOUBLE | NULL = 하한 없음 |
| UPPER_LIMIT | BINARY_DOUBLE | NULL = 상한 없음 |
| UNIT | VARCHAR2(10 CHAR) | 항목 단위와 같아야 함 |
| USE_FLAG | CHAR(1) DEFAULT 'Y' | |
| 감사 컬럼 | | |

판정 우선순위: Chain 지정 Limit -> Para 공통 (`*`) Limit -> 없으면 판정 안 함 (`NO_LIMIT`).
`CHAIN_NAME2` 가 있는 Leakage_Serial 은 CHAIN_NAME 에 `A:B` 원문을 넣어 지정한다 **(화면 설계 때 재확인)**.

### 3.4 TQP_ET_FILE (파일 헤더)

| 컬럼 | 형식 | 설명 / 샘플 |
| --- | --- | --- |
| ET_FILE_SEQ | NUMBER PK | 시퀀스 |
| FACTORY / DEVICE_ID | NOT NULL | 업로드 화면에서 선택 |
| LOT_ID | VARCHAR2(50 CHAR) NOT NULL | `RH-HS2615` |
| WAFER_ID | VARCHAR2(50 CHAR) NOT NULL | `RH-HS2615-02` |
| TEST_PROGRAM | VARCHAR2(50 CHAR) NOT NULL | `TSVET` |
| TEST_START_TIME | DATE NOT NULL | 2026-06-09 16:19:40 |
| FILE_PREFIX | VARCHAR2(100 CHAR) | `CAI_OMM_ATV_A0` |
| FILE_NAME | VARCHAR2(260 CHAR) NOT NULL | 원본 파일명 |
| SHEET_NAME | VARCHAR2(50 CHAR) | `DUT Summary` |
| VIEW_SIDE | VARCHAR2(10) DEFAULT 'BACK' | 2.3 근거 |
| DIE_COUNT | NUMBER(10) | 24 |
| ITEM_COUNT | NUMBER(10) | 2755 |
| VALUE_COUNT | NUMBER(10) | 66120 (저장 후 검증용) |
| FAIL_DIE_COUNT | NUMBER(10) | 4 (DUT Flag 기준) |
| LOAD_STATUS | VARCHAR2(20) | `LOADING` / `READY` (GDS 와 같이 마지막에 확정) |
| 감사 컬럼 | | |

UNIQUE (FACTORY, DEVICE_ID, LOT_ID, WAFER_ID, TEST_PROGRAM, TEST_START_TIME).
같은 키 파일을 다시 올리면 **기존 행 삭제 후 재등록** (하위 테이블 ON DELETE CASCADE) — GDS Map 과 같은 정책 (결정 필요 D1).

### 3.5 TQP_ET_DIE

| 컬럼 | 형식 | 설명 |
| --- | --- | --- |
| ET_FILE_SEQ | PK, FK -> TQP_ET_FILE CASCADE | |
| DUT_NO | NUMBER(10) PK | `#3` -> 3 |
| DIE_X / DIE_Y | NUMBER(10) NOT NULL | `(3, 1)` |
| HW_BIN / SW_BIN | NUMBER(10) | 1 / 5001 |
| PASS_FLAG | CHAR(1) | `P` / `F` |
| FLAG_CODE | VARCHAR2(10) | `0x00` / `0x08` |
| TEST_TIME_MS | NUMBER(10) | 9469 |
| TESTS_EXECUTED | NUMBER(10) | 2755 |
| HEAD_SITE | VARCHAR2(30 CHAR) | `Head 1 - Site 1` |

UNIQUE (ET_FILE_SEQ, DIE_X, DIE_Y) — 같은 좌표가 두 번 나오면 업로드를 막는다 (재시험 행이 섞인 파일이 오면 정책 재결정).

### 3.6 TQP_ET_VALUE

| 컬럼 | 형식 | 설명 |
| --- | --- | --- |
| ET_FILE_SEQ | PK | |
| DUT_NO | PK | FK -> TQP_ET_DIE CASCADE |
| ET_ITEM_SEQ | NUMBER PK | TQP_ET_ITEM 참조 (FACTORY / DEVICE_ID 는 FILE 에서) |
| VALUE | BINARY_DOUBLE | 측정값, `-` / 빈 값이면 NULL |
| VALUE_STATUS | CHAR(1) | `V` 숫자 / `N` 미측정 (`-`) / `E` 빈 값 |

인덱스: (ET_FILE_SEQ, ET_ITEM_SEQ) — 한 Chain 의 Wafer 내 분포 조회용.

판정 결과 (Pass / Fail) 는 **저장하지 않고** 조회 때 TQP_ET_LIMIT 과 비교해 계산한다. Limit 을 바꿔도 다시 올릴 필요가 없다. 이후 Correlation 성능 문제가 생기면 판정 결과 테이블을 따로 만든다.

---

## 4. Parsing 규칙

| 순서 | 규칙 | 실패 시 |
| --- | --- | --- |
| 1 | 시트 `DUT Summary` 를 찾는다 | 없으면 업로드 중단 |
| 2 | 1행에서 고정 열을 **이름으로** 찾는다 (`File ID` ~ `DUT Flag`). 위치가 바뀌어도 동작 | 필수 열 (`(X, Y)`, `DUT Flag`) 없으면 중단 |
| 3 | 1행에서 `@` 가 들어간 열 = 측정 열 | 0개면 중단 |
| 4 | A열이 `Test Number` / `Upper Limit` / `Lower Limit` / `Unit` 인 행을 찾는다 (행 번호 고정 안 함). Limit 두 행은 읽지 않는다 | Unit 행 없으면 중단 |
| 5 | A열이 `#숫자` 인 행 = Die 행 | 0행이면 중단 |
| 6 | 측정 헤더를 1.4절 정규식으로 분해, `:` 가 있으면 Chain 2개 | 분해 실패 헤더가 1개라도 있으면 중단 (목록 표시) |
| 7 | 값: 숫자 -> VALUE / `-` -> NULL + `N` / 빈 값 -> NULL + `E` / 그 외 문자 -> 중단 | |
| 8 | `Bin 5001` -> 5001, `9469 ms` -> 9469, `(3, 1)` -> 3, 1, `Failed - 0x08` -> F + `0x08` | 형식 다르면 중단 |
| 9 | 파일명 2.1절 규칙 (뒤에서부터 분해) | 14자리 일시 아니면 중단 |
| 10 | 저장 전 확인 창: Lot / Wafer / 프로그램 / 일시 / Die 수 / 항목 수 / 신규 항목 수 / 미측정 셀 수 / 기존 같은 파일 존재 여부 | |

---

## 5. 처리 흐름과 구현 위치 (기존 방식 따름)

```text
[ENGUI 업로드 화면] 파일 선택 + Factory / Device 선택
   -> ClosedXML (ClientBin 에 이미 있음) 로 읽기 + 4절 Parsing (클라이언트)
   -> 확인 창
   -> Service 호출 (RO / Interface / BSL / DSL / XML 기존 DefectDefine 방식, DTO 금지)
        1) BeginEtFile : 같은 키 기존 FILE 삭제(CASCADE) -> FILE INSERT (LOADING) -> ET_FILE_SEQ 반환
        2) MergeEtItem : 신규 항목만 INSERT, 전체 ITEM_NAME -> ET_ITEM_SEQ 목록 반환
        3) SaveEtDie   : Die 행 Bulk INSERT
        4) SaveEtValue : 값 Bulk INSERT (GDS 와 같은 10,000행 배치)
        5) CompleteEtFile : 건수 검증 (VALUE_COUNT = Die x 항목) -> READY
```

- DB 접속은 Miracom.Middleware 만 사용, 테이블별 DSL / XML 분리 (기존 규칙).
- 신규 화면 / 신규 Interface / 신규 BSL 만 추가하고 기존 화면 / Core 는 수정하지 않는다.
- 저장 확인용 Probe: 샘플 파일로 FILE 1 / DIE 24 / ITEM 2,755 / VALUE 66,120 / 미측정 17 / Fail Die 4 가 되는지 확인.

---

## 6. 결정 필요 항목

| ID | 내용 | 제안 |
| --- | --- | --- |
| D1 | 같은 Lot / Wafer / 프로그램 / 일시 파일 재업로드 | 삭제 후 재등록 (GDS 와 같음) |
| D2 | Device 지정 방법 (파일에 Part ID 없음) | 업로드 화면에서 Factory / Device 선택 |
| D3 | 파일명 해석 (접두어 4개 / Lot / Wafer 위치) | 2.1절 추정대로 구현, 고객 확인 후 보정 |
| D4 | 파일 Limit 처리 | 저장 안 함 (사용자 지시). Limit 화면에 "파일 값으로 초기 채우기" 버튼은 넣을지 결정 |
| D5 | Para 와 조건 분리 | `Continuity` + `iforce=8.0mA` 로 나누고 원문 헤더 함께 보관 |
| D6 | `_FH` / `_FL` 의미, GDS Chain Code 와의 이름 규칙 | 전체 이름 저장, GDS Chain Code 를 E-Test Chain 이름과 같게 등록하는 규칙 제안 (고객 확인) |
| D7 | Die 항목 저장 범위 (File ID / Part ID 제외) | 1.3절 표대로 |
| D8 | `-` 미측정의 의미 | 미측정 (N) 으로 저장, 고객 확인 |

## 7. 다음 단계

1. 사용자 D1 ~ D8 결정
2. DDL 작성 + 로컬 DB 적용 (승인 후)
3. Interface / RO / BSL / DSL / XML + Probe (샘플 파일 저장 건수 확인)
4. ENGUI 업로드 화면
5. Limit 설정 화면 설계 (TQP_ET_LIMIT)
6. Correlation: Fail (Die, Chain) -> GDS Chain Member -> KLARF Defect 겹침 (F07)


---

## 8. KLARF (XD106.02) 연결 / Mapping 확인 (2026-10-07 추가, **KLARF 세션 참고 기록 / 이 세션 범위 밖**)

대상: 같은 폴더 `XD106.02` (KLARF 1.2, Defect 15건) + `Defect Images/*.jpg` 15개. 두 파일을 Python 으로 전수 대조했다.

### 8.1 Wafer 식별 정보 대응

| 항목 | KLARF | E-Test | 대응 |
| --- | --- | --- | --- |
| Lot | `LotID "PFIC_RH-HS2615_C4Bump"` | 파일명 `RH-HS2615` / I열 `RH-HS2615` | KLARF Lot 안에 E-Test Lot 이 들어 있음. 앞 `PFIC_` / 뒤 `_C4Bump` (StepID 의 C4Bump 와 같음) 를 빼면 일치 **(규칙 추정)** |
| Wafer | `WaferID "XD106.02"` / `Slot 2` | 파일명 `RH-HS2615-02` | Wafer ID 문자열은 서로 다름. **Slot 2 = `-02`** 로만 연결됨. `XD106` 의 의미 확인 필요 |
| Device | `DeviceID "Hybrid_OMM"` / SetupID `ROOT-CAI-OMM_Hybrid-...` | 파일명 접두어 `CAI_OMM_ATV_A0` | `CAI` / `OMM` 공통. 직접 같은 문자열은 없음 -> 업로드 시 Device 선택 (D2) 유지 |
| 시점 | FileTimestamp `06-05-26` (MM-DD-YY, SetupID 날짜 `05-26-26` 로 형식 확인) = 2026-06-05 | 2026-06-09 16:19:40 | AOI (C4Bump) 4일 뒤 E-Test. 순서 맞음 |
| Die 크기 | DiePitch 23,832 x 30,060 µm | 없음 | |
| 방향 | Notch `DOWN` / `OrientationInstructions "Front"` / Mirrored NO | 없음 | 10-06 회의 내용 (KLARF Back side) 과 파일 표기 `Front` 가 다름 -> **확인 필요** |

DMS 기존 적재 키: `TQD_LOT.LOT_ID` (= KLARF LotID) / `TQD_WAFER.WAFER_ID, SLOT_ID`. 따라서 E-Test -> KLARF 연결 키는 **(Lot 핵심 문자열, Slot)** 이다. 현재는 문자열 규칙이 추정이므로 E-Test 업로드 화면에서 연결할 DMS Lot / Wafer 를 **선택 + 자동 추천**하는 방식을 제안한다 (결정 D9).

### 8.2 Die 좌표 대응 (실측으로 찾음)

- KLARF 검사 Die (SampleTestPlan) 26개: X -5 ~ 4 (10열) x Y -3 ~ 3 (7행).
- E-Test Die 24개: X 1 ~ 7 (7열) x Y 1 ~ 10 (10행). 가로 / 세로 수가 바뀌어 있음 -> 90도 회전 관계.
- 회전 4종 x 반전 2종 x 이동량 (-20 ~ 20) 을 전부 대입: 90도 계열 4가지가 24개 모두 일치 (배치가 상하좌우 대칭이라 Die 위치만으로는 결정 불가).
- 그래서 **Defect Die 와 E-Test Fail Die 를 대조**해 한 가지로 정했다.

| 후보 변환 | KLARF Defect Die 4개 중 E-Test Fail Die 와 같은 수 |
| --- | --- |
| **ET_X = KLARF_YINDEX + 4 / ET_Y = 5 - KLARF_XINDEX** (시계방향 90도) | **3** |
| ET_X = 4 - YI / ET_Y = XI + 6 | 1 |
| 나머지 2개 (반전 포함) | 0 |

E-Test 의 Notch 3시 방향 (10-06 회의) 과 KLARF Notch DOWN 의 차이가 90도인 것과도 맞는다.

**채택 후보 변환 기준 대응표 (Defect / Fail 있는 Die)**

| KLARF (XI, YI) | E-Test (X, Y) | DUT | E-Test 결과 | KLARF Defect | E-Test Fail 항목 |
| --- | --- | --- | --- | --- | --- |
| (2, 3) | (7, 3) | #20 | Fail 0x08 | RDLshort(54) 2 | Leakage_Parallel PIC_COMB12_M3_FL / M4_FL Limit 밖 + PIC_COMB11/12_M4 미측정 |
| (1, 3) | (7, 4) | #22 | Fail 0x08 | RDLshort(54) 3 | PIC_COMB11/12_M4 Leakage 미측정 (`-`) |
| (-1, 1) | (5, 6) | #42 | Fail 0x08 | RDLopen(53) 5 | Leakage PIC_COMB12_M4_FH / FL / Serial Limit 밖 |
| (-1, 3) | (7, 6) | #40 | **Pass** | RDLshort(54) 5 | 없음 -> 불일치 1 |
| (0, -3) | (1, 5) | #31 | Fail 0x08 | **없음** | PIC_COMB11/12_M4 Leakage 미측정 -> 불일치 1 |
| (-5, 0) / (-5, 1) | (4, 10) / (5, 10) | - | **E-Test 행 없음** | 없음 | KLARF 26 - E-Test 24 = 2 |

해석: RDLshort (배선 단락) <-> Leakage (Comb 누설) Fail 이 같은 Die 에서 나와 물리적으로도 맞다. 단 대칭 배치 + 표본 4개라 **확정은 아님** — 연구소 좌표 대응 테이블 (10-06 회의에서 전달 예정) 로 최종 확인한다.

Die 안 좌표 (XREL / YREL) 도 같은 90도 회전이 필요하다 (KLARF Die 가로 23,832 -> E-Test 기준 세로). GDS Map 이 Back side 반전으로 저장되므로, KLARF Die 상대 좌표 -> GDS 좌표 변환식은 GDS Die 외곽 크기와 대조해 별도로 확정해야 한다 (F07 좌표 계약).

### 8.3 KLARF 파일 확인 사항

| 항목 | 내용 | 영향 |
| --- | --- | --- |
| Defect 분류 위치 | `CLASSNUMBER` 는 전부 0 (Unclassified). 53 / 54 는 **`ROUGHBINNUMBER`** 에 있음 | 분류명 (ClassLookup) 을 ROUGHBIN 으로 찾아야 함. 이미지 파일명 끝 `_RDLshort_54` 도 ROUGHBIN 기준 |
| 이미지 파일명 | `{WaferID}_{XINDEX}_{YINDEX}_0_{DEFECTID}_{분류명}_{ROUGHBIN}.jpg` | DEFECTID 로 Defect 와 1:1 연결 가능 (15 = 15) |
| 레코드 길이 | Spec 22개, 행은 24토큰 (IMAGELIST 가 `1 1 69` 3토큰) | 마지막 항목 가변 길이 처리 필요 |
| Summary 불일치 | SummaryList NDEFECT 13 / 실제 Defect 15 / NDEFDIE 4 (실제 4) | 건수는 DefectList 기준으로 저장 |
| 좌표식 | X = XINDEX x 23,832 + XREL - SampleCenterX(-289) 확인 (Y 동일 식) | 기존 KLARF Map Setup 분석과 같음 |

### 8.4 설계 반영 제안

1. `TQP_ET_FILE` 에 연결 컬럼 추가: `LINK_LOT_ID` (DMS `TQD_LOT.LOT_ID`), `LINK_SLOT_ID` — 업로드 시 추천 후 사용자 확인.
2. Die 좌표 대응 규칙 테이블 `TQP_ET_DIE_XFORM` (FACTORY, DEVICE_ID PK / ROTATE_DEG 0·90·180·270 / MIRROR_X Y·N / OFFSET_X / OFFSET_Y / 근거 메모). 샘플 값: 시계 90도 / 반전 N / (4, 5). 연구소 대응표가 규칙식이 아니면 Die 1:1 표 (`KLARF_XI, KLARF_YI -> ET_X, ET_Y`) 로 바꾼다.
3. `TQP_ET_DIE` 에는 원본 (X, Y) 만 저장하고 KLARF 인덱스는 조회 때 규칙으로 계산 (규칙이 바뀌어도 재업로드 불필요).
4. Correlation 단위: (Wafer, Die, Fail Chain) <- E-Test / (Wafer, Die, Defect) <- KLARF, Die 는 8.2 변환으로 연결, Chain 은 GDS Chain Member 와 Defect 위치 겹침으로 연결 (F07).

### 8.5 추가 결정 / 확인 항목

| ID | 내용 | 제안 |
| --- | --- | --- |
| D9 | E-Test <-> KLARF Wafer 연결 방식 | Lot 핵심 문자열 + Slot 자동 추천, 사용자 확인 후 저장 |
| D10 | Die 변환 시계 90도 + (4, 5) 채택 여부 | 샘플 기준 잠정 채택, 연구소 대응표로 확정 |
| Q1 (고객) | `XD106.02` 와 `RH-HS2615-02` 관계, `PFIC_` / `_C4Bump` 규칙 | |
| Q2 (고객) | KLARF `OrientationInstructions "Front"` 와 Back side 설명 차이 | |
| Q3 (고객) | #40 (Defect 5개, Pass) / #31 (Defect 없음, Fail) 이 정상 케이스인지 | |


---

## 9. E-Test 파일 기준 정보 설계 (현재 범위, DDL 작성)

결정 (2026-10-07 사용자): R1 Lot + Slot 을 붙인 WAFER_ID 를 만든다 / R3 DEVICE_ID 는 `TQP_GDS_MAP.DEVICE_ID` 와 같은 값 체계 / 테이블명 앞에 GDS (`TQP_GDS_SRC_FILE`) / RAW_ 컬럼 없음 / 한 행 = 파일 1개 / E-Test 만.

DDL: [`2026-10-07_tqp_gds_src_file.sql`](../DMS/06_CoreLibSource/GdsMapControl/Persistence/2026-10-07_tqp_gds_src_file.sql) (시퀀스 `SRC_FILE_SEQ` / 테이블 1 / UNIQUE 1 / 인덱스 1). **2026-10-07 로컬 docker oracle19c DMSMGR 적용 (사용자 승인 "진행")**. 확인: PK / UNIQUE / CHECK ENABLED, WAFER_ID 가상 컬럼 (샘플 값 INSERT -> `RH-HS2615-02`), 같은 키 재INSERT ORA-00001, WAFER_ID 직접 INSERT ORA-54013, SLOT 100 ORA-02290, 시험 행은 ROLLBACK (0행).

### 9.1 TQP_GDS_SRC_FILE

| 컬럼 | 형식 | 값 출처 / 샘플 |
| --- | --- | --- |
| SRC_FILE_SEQ | NUMBER PK | 시퀀스 |
| FILE_NAME | VARCHAR2(260 CHAR) NOT NULL | 원본 파일명 |
| FACTORY | VARCHAR2(30 CHAR) NOT NULL | 업로드 화면 선택 |
| DEVICE_ID | VARCHAR2(50 CHAR) NOT NULL | 업로드 화면 선택 (파일 Part ID 빈 값) |
| LOT_ID | VARCHAR2(50 CHAR) NOT NULL | 파일명 Lot `RH-HS2615` |
| SLOT_NO | NUMBER(3) NOT NULL | 파일명 Wafer 끝 숫자 `-02` -> 2 (1 ~ 99) |
| WAFER_ID | VARCHAR2(60 CHAR) **가상 컬럼** | `LOT_ID || '-' || 2자리 SLOT_NO` = `RH-HS2615-02` |
| TEST_PROGRAM | VARCHAR2(50 CHAR) NOT NULL | 파일명 `TSVET` |
| TEST_TIME | DATE NOT NULL | 파일명 `20260609161940` -> 2026-06-09 16:19:40 |
| FILE_PREFIX | VARCHAR2(100 CHAR) | 파일명 앞부분 `CAI_OMM_ATV_A0` (의미 확인 필요) |
| 감사 컬럼 | | CREATE_USER / CREATE_TIME / UPDATE_USER / UPDATE_TIME |

- UNIQUE (FACTORY, DEVICE_ID, LOT_ID, SLOT_NO, TEST_PROGRAM, TEST_TIME) — 같은 Wafer 의 같은 시험 1건.
- 인덱스 (FACTORY, DEVICE_ID, WAFER_ID) — Wafer 로 파일 찾기.
- WAFER_ID 는 가상 컬럼이라 INSERT / UPDATE 에 넣지 않는다 (넣으면 ORA-54013).
- E-Test 는 파일명 Wafer (`RH-HS2615-02`) 가 Lot + Slot 과 같으므로 별도 원문 칸을 두지 않는다.

### 9.2 Parsing 규칙 (기준 정보만)

| 순서 | 규칙 | 실패 시 |
| --- | --- | --- |
| 1 | 확장자 뺀 파일명을 `_` 로 나누고 **뒤에서부터** 읽는다: 마지막 = Wafer, 그 앞 = Lot, 그 앞 = 14자리 일시, 그 앞 = 시험 프로그램, 나머지 = 접두어 | 토큰 5개 미만이면 중단 |
| 2 | 일시는 `yyyyMMddHHmmss` 14자리 | 형식 다르면 중단 |
| 3 | Wafer 는 `{Lot}-{숫자}` 형식이어야 하며 숫자가 SLOT_NO | 형식 다르면 중단 (WAFER_ID 를 Lot + Slot 으로 만들기 때문) |
| 4 | 시트 `DUT Summary` 의 `Wafer ID` 열 값이 파일명 Lot 과 다르면 경고만 표시 | 저장은 계속 |
| 5 | Factory / Device 는 업로드 화면에서 선택 | 미선택 시 저장 불가 |
| 6 | 같은 UNIQUE 키 파일이 이미 있으면 확인 창 -> 삭제 후 재등록 (GDS Map 과 같은 정책) | 취소 시 저장 안 함 |

### 9.3 나중 설계에서 이 테이블을 쓰는 방식

E-Test 측정 항목 / Die / 측정값 / Limit 테이블은 나중 설계에서 `SRC_FILE_SEQ` 를 참조한다 (3.2 ~ 3.6절 참고안의 `TQP_ET_FILE` 은 이 테이블로 대체하고, 결과 테이블 이름도 `TQP_GDS_` 접두어 규칙을 따른다).


---

## 10. E-Test 결과 Data 설계 (측정 항목 / Die / 측정값)

2026-10-07 사용자 "진행" 으로 이어서 설계. DDL 초안: [`2026-10-07_tqp_gds_et_result.sql`](../DMS/06_CoreLibSource/GdsMapControl/Persistence/2026-10-07_tqp_gds_et_result.sql) — **2026-10-07 로컬 docker oracle19c DMSMGR 적용 (사용자 승인 "진행")**: 테이블 변경 1 / 생성 3 / 시퀀스 1 / 인덱스 4 / 주석 14, 제약 전부 ENABLED. ROLLBACK 시험: LOAD_STATUS 기본 LOADING / 측정값 PK 중복 ORA-00001 / V 인데 값 NULL ORA-02290 / 같은 Die 좌표 ORA-00001 / 파일 삭제 시 Die·측정값 CASCADE 삭제, 항목은 남음 / ROLLBACK 후 4개 테이블 0행. 3.2 ~ 3.6절 참고안을 이 절로 대체한다. Limit (판정 기준) 은 Limit 설정 화면과 함께 나중에 설계한다 (3.3절 참고안 유지).

### 10.1 관계

```text
TQP_GDS_SRC_FILE (파일 1개, 적용 완료)  ── 보강: SHEET_NAME / DIE_COUNT / ITEM_COUNT / VALUE_COUNT / LOAD_STATUS
  └ TQP_GDS_ET_DIE    (SRC_FILE_SEQ, DUT_NO)              ON DELETE CASCADE
      └ TQP_GDS_ET_VALUE (SRC_FILE_SEQ, DUT_NO, ET_ITEM_SEQ) ON DELETE CASCADE
TQP_GDS_ET_ITEM (Device 단위 항목 마스터) ── ET_VALUE.ET_ITEM_SEQ 가 참조
```

파일을 삭제하면 Die / 측정값이 함께 지워진다 (재등록 정책). 측정 항목은 Device 단위라 파일 삭제와 무관하게 남는다.

### 10.2 헤더 분해 (사용자 정의 그대로)

형식 `{PARA_NAME}[={COND_VALUE}]@{CHAIN_NAME}[:{CHAIN_NAME2}]`. 정규식 `^(?<para>[^=@]+)(?:=(?<cond>[^@]+))?@(?<c1>[^:]+)(?::(?<c2>.+))?$`.

| 원문 헤더 | PARA_NAME | COND_VALUE | CHAIN_NAME | CHAIN_NAME2 |
| --- | --- | --- | --- | --- |
| `Continuity_iforce=8.0mA@C4DC_210_3_FH` | `Continuity_iforce` | `8.0mA` | `C4DC_210_3_FH` | |
| `Resistance_iforce=1.0mA@PIC_EDM_R_FH` | `Resistance_iforce` | `1.0mA` | `PIC_EDM_R_FH` | |
| `Leakage_Parallel@C4DC_207_73_FH` | `Leakage_Parallel` | | `C4DC_207_73_FH` | |
| `Leakage_Serial@PIC_COMB11_M4_FH:PIC_COMB11_M4_FL` | `Leakage_Serial` | | `PIC_COMB11_M4_FH` | `PIC_COMB11_M4_FL` |

샘플 확인: 2,755개 전부 분해, (Para, 조건, Chain, Chain2) 중복 0. Para + 조건 7종 (Connectivity_iforce 8.0mA 865 / 1.0mA 10, Resistance_iforce 8.0mA 865 / 1.0mA 10, Leakage_Parallel 848, Leakage_Serial 156, Continuity_iforce 8.0mA 1). 1.4절 참고안 (Para `Continuity` + 조건 `iforce=8.0mA`) 대신 사용자 정의 (Para = `=` 앞) 를 따른다. 같은 Para 의 8.0mA / 1.0mA 는 COND_VALUE 로 구분된다.

Chain 이름은 `_FH` / `_FL` 를 떼지 않고 전체를 저장한다 (의미 확인 전).

### 10.3 테이블

**TQP_GDS_SRC_FILE 보강 (ALTER)**

| 컬럼 | 설명 / 샘플 |
| --- | --- |
| SHEET_NAME | `DUT Summary` |
| DIE_COUNT / ITEM_COUNT / VALUE_COUNT | 24 / 2,755 / 66,120 |
| LOAD_STATUS | `LOADING` -> 건수 검증 (VALUE_COUNT = DIE_COUNT x ITEM_COUNT) 후 `READY`. 조회 화면은 READY 만 보여준다 |

**TQP_GDS_ET_ITEM (측정 항목 마스터, Device 단위)**

| 컬럼 | 형식 | 설명 |
| --- | --- | --- |
| ET_ITEM_SEQ | NUMBER PK | 시퀀스 `ET_ITEM_SEQ` |
| FACTORY / DEVICE_ID | NOT NULL | 파일 헤더와 같은 값 |
| ITEM_NAME | VARCHAR2(200 CHAR) | 원문 헤더 (최대 54자 실측). UNIQUE (FACTORY, DEVICE_ID, ITEM_NAME) |
| PARA_NAME / COND_VALUE | | 10.2절 |
| CHAIN_NAME / CHAIN_NAME2 | VARCHAR2(64 CHAR) | 최대 19자 실측 |
| UNIT | | 5행 `V` / `Ohm` / `uA` |
| TEST_NO | | 2행 Test Number |
| DISPLAY_ORDER | | 처음 등록한 파일의 열 순서 |

새 파일에 없던 헤더가 나오면 추가한다. 이미 있는 헤더인데 단위가 다르면 업로드를 막는다. 파일의 Upper / Lower Limit (3 / 4행) 은 읽지 않는다.

**TQP_GDS_ET_DIE**

| 컬럼 | 파일 값 -> 저장 |
| --- | --- |
| DUT_NO | A열 `#3` -> 3 (PK 2번째) |
| DIE_X / DIE_Y | `(3, 1)` -> 3 / 1 (파일 원본 좌표, 변환 없음). UNIQUE (SRC_FILE_SEQ, DIE_X, DIE_Y) |
| HW_BIN / SW_BIN | `Bin 5` / `Bin 5001` -> 5 / 5001 |
| PASS_FLAG / FLAG_CODE | `Failed - 0x08` -> F / `0x08` |
| TEST_TIME_MS | `9469 ms` -> 9469 |
| TESTS_EXECUTED | 2755 |
| HEAD_SITE | `Head 1 - Site 1` |

저장하지 않는 열: File ID (전부 0), Part ID (전부 빈 값), Wafer ID (파일명 Lot 비교에만 사용).

**TQP_GDS_ET_VALUE (Die x 항목 1행)**

| 컬럼 | 설명 |
| --- | --- |
| SRC_FILE_SEQ / DUT_NO / ET_ITEM_SEQ | PK |
| MEAS_VALUE | BINARY_DOUBLE. 숫자만 (`VALUE` 는 Oracle 예약어 계열이라 피함) |
| VALUE_STATUS | `V` 숫자 / `N` 미측정 `-` (샘플 17셀) / `E` 빈 값. CHECK 로 V 이면 값 필수, N / E 이면 NULL |

인덱스: (SRC_FILE_SEQ, ET_ITEM_SEQ) — 한 파일에서 Chain / 항목별 Die 값 조회 / (ET_ITEM_SEQ) — 여러 Wafer 의 같은 항목 조회 + 항목 FK 검사.

Pass / Fail 은 저장하지 않는다. 나중에 Limit 테이블과 비교해 계산한다 (Limit 을 바꿔도 다시 올릴 필요 없음).

### 10.4 저장 흐름 (Service, 기존 RO / Interface / BSL / DSL / XML 방식)

1. 클라이언트: ClosedXML 로 읽기 + Parsing (9.2절 + 4절 규칙) -> 확인 창
2. BeginEtFile: 같은 UNIQUE 키 파일이 있으면 삭제 (CASCADE) -> TQP_GDS_SRC_FILE INSERT (LOADING) -> SRC_FILE_SEQ
3. MergeEtItem: 없는 항목만 INSERT -> ITEM_NAME 별 ET_ITEM_SEQ 반환 (단위 다르면 중단)
4. SaveEtDie: Die 24행 INSERT
5. SaveEtValue: 측정값 66,120행 Bulk INSERT (10,000행 배치, GDS 와 같은 방식)
6. CompleteEtFile: 건수 검증 -> 건수 / READY 갱신

### 10.4.1 구현 1단계 (서버, 2026-10-07)

Interface `iGdsEtResult` / RO·BSL `GdsEtResult` / DSL·XML 4개 (`TQP_GDS_SRC_FILE`, `TQP_GDS_ET_ITEM`, `TQP_GDS_ET_DIE`, `TQP_GDS_ET_VALUE`). Probe `GdsMapControl/Probes/GdsEtResultProbe.ps1` 16/16 PASS (아래 10.5 기대값 모두 일치, 측정값 66,120행 저장 약 1.4초). 미배포 / Service 등록 / Remoting 미확인. Excel 에 열린 파일은 공유 읽기 스트림으로 열어야 한다.

### 10.4.2 구현 2단계 (화면, 2026-10-07)

메뉴 `MNU_DMS_ET_RESULT_UPLOAD` (DMS > E-Test 결과 업로드) / 화면 `ENGUI/ETest/frmEtResultUpload` / 파서 `ENGUI/ETest/EtResultFileParser`. 화면 흐름: 파일 선택 즉시 분석 -> 요약 / Die 미리보기 / 경고 -> Factory(로그인 기본) / Device(GDS READY 목록 또는 직접 입력) -> [DB 저장] (같은 시험이면 삭제 후 재등록 질문) -> 등록 파일 목록 / [선택 파일 삭제]. Smoke `ENGUI/Verification/Verify-EtResultUpload.ps1` 17항목 PASS. **2026-10-07 13:11 배포** (ServerBin Interface / DSL / BSL + Service config wellknown, ClientBin Interface / RO / ENGUI), 운영 BSL Probe 16/16 PASS. DMS 화면 확인 대기.

### 10.5 샘플 기준 확인 값 (Probe)

| 항목 | 기대값 |
| --- | --- |
| TQP_GDS_SRC_FILE | 1행, WAFER_ID `RH-HS2615-02`, READY, DIE 24 / ITEM 2,755 / VALUE 66,120 |
| TQP_GDS_ET_ITEM | 2,755행 (처음 올릴 때), 같은 파일 재등록 시 추가 0 |
| TQP_GDS_ET_DIE | 24행, PASS_FLAG F 4행 (#20 / #22 / #31 / #42) |
| TQP_GDS_ET_VALUE | 66,120행, VALUE_STATUS N 17행 |

### 10.6 확인 / 결정 (제안대로 반영, 다르면 지시)

| ID | 내용 | 반영 |
| --- | --- | --- |
| E1 | 측정 항목을 Device 단위로 공유 (파일마다 따로 두지 않음) | 반영. 파일 2,755개 헤더를 매번 저장하지 않음 |
| E2 | Para 이름 = `=` 앞 (사용자 정의) | 반영 |
| E3 | Chain 이름 `_FH` / `_FL` 포함 전체 저장 | 반영 (의미는 고객 확인) |
| E4 | `-` = 미측정 (N) | 반영 (고객 확인) |
| E5 | File ID / Part ID 저장 안 함 | 반영 |


---

## 11. 업로드 데이터 조회 창 설계 (2026-10-07 사용자 지시)

사용자 지시: "업로드된 data 를 볼 수 있는 화면이 있어야 한다. 기존 Upload 화면에서 결과 File 더블클릭 시 확인할 수 있도록."

### 11.1 여는 방법

DMS > E-Test 결과 업로드 화면 아래 **등록된 E-Test 파일 목록에서 행을 더블클릭** -> 조회 창(`frmEtResultView`, 새 창)이 열린다. 메뉴는 추가하지 않는다. 여러 파일을 비교할 수 있게 창은 여러 개 띄울 수 있다(비모달). 상태가 LOADING 인 파일도 열 수 있다(저장된 만큼 보임, 제목에 상태 표시).

### 11.2 표 방향: 행 = 측정 항목, 열 = Die

측정 항목이 2,755개라 "행 = Die, 열 = 항목" 으로 펼치면 열이 너무 많아 볼 수 없다. **행 = 항목 (2,755행), 열 = Die (24열)** 로 놓고 Para / Chain 으로 행을 거른다.

```text
┌ 파일 정보: WAFER_ID / Lot / Slot / 시험 프로그램 / 시험 일시 / Die / 항목 / 측정값 / 미측정 / 상태 / 등록 ┐
├ 거르기: Para [전체 ▼]  Chain 포함 [        ]  [ ] 미측정 있는 항목만   표시 2,755 / 2,755 항목          ┤
├ Die 목록 (왼쪽)          │ 측정값 표 (오른쪽)                                                         ┤
│ DUT X Y HW SW P/F Flag  │ Para | 조건 | Chain | Chain2 | 단위 | Test No | #3 (3,1) | #4 (4,1) | ...  │
│ 미측정                  │   값은 파일 값 그대로, 미측정은 "-" (회색), Fail Die 열 머리글은 붉은색     │
└─────────────────────────┴──────────────────────────────────────────────────────────────────────────────┘
```

- Para 목록: `PARA_NAME` + 조건 (예: `Resistance_iforce=8.0mA`, `Leakage_Serial`) 7종 + 전체.
- Chain 포함: CHAIN_NAME / CHAIN_NAME2 에 입력 글자가 들어간 항목 (대소문자 무시).
- Die 목록에서 Die 를 더블클릭하면 측정값 표가 그 Die 열로 이동한다.
- 값 표시는 저장값 그대로 (R 형식), 판정 (Limit) 색 표시는 Limit 설정 화면과 함께 나중에 한다.

### 11.3 조회 데이터 (이미 배포된 서비스 사용, 서버 변경 없음)

| 호출 | 용도 |
| --- | --- |
| `GetEtFile(seq)` | 파일 정보 줄 |
| `GetEtDieList(seq)` | Die 목록 / 열 머리글 |
| `GetEtItemList(factory, device)` | 항목 Para / 조건 / Chain / 단위 (Device 단위) |
| `GetEtValueList(seq)` | 측정값 66,120행 -> 화면에서 항목 x Die 표로 펼침 |

샘플 1장 기준 측정값 66,120행 1회 조회. 화면은 표를 한 번 만들고 거르기는 DataView RowFilter 로 한다(재조회 없음).

### 11.4 구현 범위

신규 `ENGUI/ETest/frmEtResultView.cs` / `.Designer.cs`, `frmEtResultUpload` 에 목록 더블클릭 1개 (+ Designer 이벤트 1줄), ENGUI csproj Compile 2개, Smoke 에 조회 창 검사 추가. 서버 / DB / 메뉴 변경 없음.

**구현 (2026-10-07)**: 위 설계대로 구현, Smoke 27항목 PASS (조회 창 9항목). ENGUI 배포 승인 대기.


---

## 12. 중복 기준 변경: WAFER_ID / Slot 사용자 입력 (2026-10-07 사용자 지시)

사용자 지시: "디바이스 기준으로 중복으로 저장 가능하다. 결과 data 는 wafer 별로 Lot 별로 생성된다. 디바이스는 중복이 허용되어야 하며 중복 방지는 wafer id 기준으로 우선 정의한다. wafer 의 slot No 가 없으므로 사용자가 입력할 수 있도록 한다."

| 항목 | 이전 (9절) | 변경 |
| --- | --- | --- |
| 중복 키 (UNIQUE) | FACTORY + DEVICE_ID + LOT_ID + SLOT_NO + TEST_PROGRAM + TEST_TIME | **FACTORY + WAFER_ID** (`UQ_GDS_SRC_FILE_WAFER`) |
| Device | 키의 일부 | 키가 아님. 같은 Device 에 여러 Lot / Wafer 저장 |
| Slot | 파일명 Wafer `{Lot}-{숫자}` 에서 얻고 아니면 업로드 중단 | **업로드 화면 Slot No 입력 (1~99)**. 파일명에서 얻으면 기본값, 못 얻으면 빈 칸 (입력해야 저장) |
| WAFER_ID | Lot + '-' + 2자리 Slot (가상 컬럼) | 같음. 화면에 저장될 WAFER_ID 를 미리 보여 준다 |
| 같은 Wafer 재업로드 | 같은 시험 일시일 때만 질문 | 시험 일시 / Device 가 달라도 같은 WAFER_ID 면 "기존 삭제 후 재등록" 질문 (기존 Device / 시험 일시 표시) |

- DDL: [`2026-10-07_tqp_gds_src_file_uq_wafer.sql`](../DMS/06_CoreLibSource/GdsMapControl/Persistence/2026-10-07_tqp_gds_src_file_uq_wafer.sql) — 로컬 DB 적용 (적용 전 WAFER_ID 중복 0, 기존 1건 SCK / OMM_Hybrid / RH-HS2615-02 유지).
- 서비스: `GetEtFileByKey` 키가 3항목 (Factory / Lot / Slot) 으로 바뀜, 새 쿼리 `SELECT_SRC_FILE_BY_WAFER`. 이전 `SELECT_SRC_FILE_BY_KEY` 는 배포 전 BSL 호환용으로 그대로 둔다 (Service 가 08_DBQuery XML 을 바로 읽기 때문).
- 파서: 파일명 Wafer 가 `{Lot}-{숫자}` 가 아니어도 중단하지 않는다 (SlotNo = 0 -> 사용자 입력).
- 시험 값은 Lot / Slot 만으로 정해지므로, 같은 Wafer 의 재시험 파일을 함께 보관해야 하면 키 재정의 필요 ("우선 정의").


---

## 13. 파일명 규칙 정형화 (2026-10-12 사용자 제공 규칙)

사용자 제공 (2026-10-12):

```text
[Customer]_[Device]_[Wafer Type]_[Wafer Rev]_[Test Type]_[Test Date]_[Lot]_[Lot-Wafer No]$Silterra$
CAI_OMM_ATV_A0_TSVET_20260902141031_U9-HS2633_U9-HS2633-01$Silterra$
```

상태: **구현 완료 (2026-10-12, 사용자 "제안대로 진행") — 13.7절**

### 13.1 분석

| 칸 | 규칙 이름 | 예시 (새 규칙) | 기존 샘플 (RH-HS2615-02) | 지금 저장 위치 (9 ~ 12절) |
| --- | --- | --- | --- | --- |
| 1 | Customer | `CAI` | `CAI` | `FILE_PREFIX` 에 1~4칸이 붙어서 들어감 |
| 2 | Device | `OMM` | `OMM` | `FILE_PREFIX` (DEVICE_ID 는 사용자가 따로 선택) |
| 3 | Wafer Type | `ATV` | `ATV` | `FILE_PREFIX` |
| 4 | Wafer Rev | `A0` | `A0` | `FILE_PREFIX` |
| 5 | Test Type | `TSVET` | `TSVET` | `TEST_PROGRAM` |
| 6 | Test Date | `20260902141031` | `20260609161940` | `TEST_TIME` |
| 7 | Lot | `U9-HS2633` | `RH-HS2615` | `LOT_ID` |
| 8 | Lot-Wafer No | `U9-HS2633-01` | `RH-HS2615-02` | 저장 안 함 (Slot 기본값 `01` 에만 사용) |
| 끝 | `$Silterra$` | `Silterra` | 없음 | 없음 |

- 두 파일 모두 `$...$` 를 떼고 `_` 로 나누면 **정확히 8칸**이다. 이전 설계에서 "의미 확인 필요" 로 둔 접두어 `CAI_OMM_ATV_A0` 의 뜻이 이번 규칙으로 확정된다 (Customer / Device / Wafer Type / Wafer Rev).
- `$Silterra$` 는 `$` 로 둘러싼 꼬리표다. Silterra 는 말레이시아 파운드리 이름이라 **Wafer 제조 Fab** 으로 추정한다 (N2, 확인 필요). 없는 파일 (기존 샘플) 도 있으므로 선택 항목이다.
- **지금 파서의 문제**: 뒤에서부터 나누므로 새 파일의 마지막 칸이 `U9-HS2633-01$Silterra$` 가 된다. 그래서 Slot 기본값을 얻지 못해 매번 Slot 을 입력해야 하고, 요약에 나오는 파일명 Wafer 도 틀린다. 저장 (Lot / 시험 일시 / Test Type) 은 맞게 된다.
- 파일명 Device (`OMM`) 와 DMS / GDS Device 는 다르다 (저장된 E-Test 1건은 `OMM_Hybrid`, KLARF DeviceID `Hybrid_OMM`, GDS Map 에는 `U9` 도 있음). 그래서 파일명 Device 는 **원문으로만 보관**하고 DEVICE_ID 는 지금처럼 사용자가 고른다 (N3).
- Lot `U9-HS2633` 의 앞부분 `U9` 가 GDS Device `U9` 와 같아 보이지만 규칙으로 정해진 것이 아니므로 연결에 쓰지 않는다 (참고만).

### 13.2 파싱 규칙 (변경)

| 순서 | 규칙 | 실패 시 |
| --- | --- | --- |
| 1 | 확장자를 뺀 파일명 끝의 `$이름$` 를 떼어 Fab 꼬리표로 둔다 (`^(.*?)(?:\$([^$]+)\$)?$`). 없으면 빈 값 | - |
| 2 | 남은 이름을 `_` 로 나눠 **정확히 8칸**이어야 한다. 앞에서부터 Customer / Device / Wafer Type / Wafer Rev / Test Type / Test Date / Lot / Lot-Wafer No | 8칸이 아니면 업로드 중단, 규칙 문자열을 메시지에 표시 |
| 3 | Test Date 는 `yyyyMMddHHmmss` 14자리 | 중단 |
| 4 | 1 ~ 7칸은 빈 값이면 안 된다 | 중단 |
| 5 | Lot-Wafer No 가 `{Lot}-{숫자 1~99}` 면 숫자를 Slot **기본값**으로 쓴다 (최종 Slot 은 지금처럼 사용자 입력). Lot 으로 시작하지 않으면 경고 표시 + Slot 빈 칸 | 경고만 |
| 6 | 내용의 `Wafer ID` 열이 Lot 과 다르면 경고 (지금과 같음) | 경고만 |

8칸 고정으로 바꾸는 이유: 지금은 앞부분 칸 수가 정해지지 않아 뒤에서부터 나눴지만, 규칙이 정해졌으니 칸 수를 검사해야 잘못된 파일 (칸 누락 / `_` 가 섞인 Lot) 을 저장 전에 막을 수 있다.

### 13.3 DB (`TQP_GDS_SRC_FILE` 컬럼 추가)

| 컬럼 | 형식 | 값 (예시) |
| --- | --- | --- |
| CUSTOMER | VARCHAR2(30 CHAR) | `CAI` |
| FILE_DEVICE | VARCHAR2(50 CHAR) | `OMM` (파일명 Device 원문, DEVICE_ID 와 별개) |
| WAFER_TYPE | VARCHAR2(30 CHAR) | `ATV` |
| WAFER_REV | VARCHAR2(20 CHAR) | `A0` |
| LOT_WAFER_NO | VARCHAR2(60 CHAR) | `U9-HS2633-01` (파일명 원문) |
| FAB_NAME | VARCHAR2(50 CHAR) | `Silterra` (없으면 NULL, N2) |

- 기존 컬럼은 그대로 쓴다: Test Type = `TEST_PROGRAM`, Test Date = `TEST_TIME`, Lot = `LOT_ID` (N4). 화면 제목만 "Test Type" / "Test Date" 로 바꾼다.
- `FILE_PREFIX` (= Customer_Device_WaferType_WaferRev) 는 새 컬럼과 겹친다. 새 저장부터는 채우지 않고 남겨 둘지 / 삭제할지 결정 (N5). 기존 1건 (SCK / OMM_Hybrid / RH-HS2615-02) 은 FILE_NAME 에서 새 컬럼을 채우는 UPDATE 를 DDL 에 같이 넣는다.
- 모두 NULL 허용 (기존 행 / 꼬리표 없는 파일).

### 13.4 서비스 / 화면

- `BeginEtFile` FileInfo 를 12항목 -> **18항목** 으로 늘린다 (12 Customer / 13 File Device / 14 Wafer Type / 15 Wafer Rev / 16 Lot-Wafer No / 17 Fab). 12항목 호출도 계속 받는다 (지금 배포된 화면 호환, GdsMapImport.BeginMapImport 11 / 12항목과 같은 방식).
- 쿼리 XML `TQP_GDS_SRC_FILE.xml`: `CREATE_SRC_FILE` 은 기존 쿼리를 바꾸지 않고 새 쿼리 `CREATE_SRC_FILE_V2` 를 추가한다 (Service 가 XML 을 바로 읽으므로 배포 전 BSL 이 깨지지 않게, 12절과 같은 이유). `SELECT_SRC_FILE` / `SELECT_SRC_FILE_LIST` 는 열만 추가 (기존 BSL 은 열 이름으로 읽으므로 영향 없음).
- 업로드 화면: 요약 첫 줄을 `Customer CAI / 파일 Device OMM / Wafer Type ATV / Rev A0 / Test Type TSVET / Test Date 2026-09-02 14:10:31 / Lot U9-HS2633 / Lot-Wafer No U9-HS2633-01 / Fab Silterra` 로 바꾼다. 등록 목록에 Customer / Wafer Type / Wafer Rev / Fab 열, `TEST_PROGRAM` 제목을 "Test Type" 으로.
- 조회 창 파일 정보 줄에 같은 값 추가.
- 중복 키는 그대로 FACTORY + WAFER_ID (12절). 단 N1 참고.

### 13.5 결정 필요

| ID | 내용 | 제안 |
| --- | --- | --- |
| N1 | **같은 Wafer 의 다른 Test Type** (예: TSVET 와 다른 시험) 을 함께 저장해야 하나. 지금 키 (FACTORY + WAFER_ID) 로는 두 번째 파일이 첫 파일을 "삭제 후 재등록" 한다 | Test Type 이 여러 개 오는 경우가 있으면 키를 **FACTORY + WAFER_ID + TEST_PROGRAM(Test Type)** 으로 넓힌다. 확실하지 않으면 지금 키 유지 |
| N2 | `$Silterra$` 의 의미 / 컬럼 이름 | Wafer 제조 Fab 으로 보고 `FAB_NAME`. 다른 뜻이면 이름만 바꿈 |
| N3 | 파일명 Device (`OMM`) 와 DMS Device 의 관계 | 원문 `FILE_DEVICE` 로 보관, DEVICE_ID 는 계속 사용자 선택. 대응 규칙이 정해지면 자동 추천 추가 |
| N4 | Test Type 을 `TEST_PROGRAM` 그대로 쓸지 / `TEST_TYPE` 로 이름을 바꿀지 | **그대로** (XML / BSL / 화면 / Limit 쿼리 등 여러 곳이 쓰고 있어 바꾸면 손댈 곳이 많다). 화면 제목만 "Test Type" |
| N5 | `FILE_PREFIX` 처리 | 새 저장부터 채우지 않고 컬럼은 남김 (나중에 정리할 때 삭제) |

### 13.6 구현 단계 (결정 후)

1. DDL: 컬럼 6개 추가 + 기존 행 채우기 (+ N1 이면 UNIQUE 변경)
2. XML (`CREATE_SRC_FILE_V2`, SELECT 열 추가) / BSL `BeginEtFile` 18항목 / Interface 주석
3. 파서 `ParseFileName` 8칸 규칙 + Fab 꼬리표 / 업로드 화면 요약 / 등록 목록 열 / 조회 창 정보 줄
4. Probe / Smoke: 새 규칙 예시 이름 (`...U9-HS2633-01$Silterra$.xlsx`, 샘플 사본) 과 기존 샘플 둘 다, 칸 수 틀린 이름 거부

### 13.7 구현 (2026-10-12)

결정: N1 키 유지 (FACTORY + WAFER_ID) / N2 `FAB_NAME` / N3 `FILE_DEVICE` 원문 / N4 `TEST_PROGRAM` 유지 (화면 제목 Test Type) / N5 `FILE_PREFIX` 새 저장부터 미사용. 추가 요청 (같은 날 "추가로 넣을 정보" 제안 채택): `PASS_DIE_COUNT` / `FAIL_DIE_COUNT` / `NOT_TESTED_COUNT` (READY 확정 때 기록), `PARSE_RULE_VER` (1 = 뒤에서부터, 2 = 8칸 규칙), `VIEW_SIDE` (BACK).

- DDL `DMS/06_CoreLibSource/GdsMapControl/Persistence/2026-10-12_tqp_gds_src_file_name_rule.sql` 로컬 적용 (컬럼 11개 + CHECK, 기존 1건 FILE_NAME 에서 채움 -> CAI / OMM / ATV / A0 / RH-HS2615-02, Pass 20 / Fail 4 / 미측정 17, 규칙 1).
- XML `TQP_GDS_SRC_FILE.xml`: 신규 `CREATE_SRC_FILE_V2` / `UPDATE_SRC_FILE_READY_V2` (기존 쿼리 유지), `SELECT_SRC_FILE` / `SELECT_SRC_FILE_LIST` 열 추가.
- BSL `BeginEtFile` 12 / 18항목 (18 = V2), `CompleteEtFile` 은 V2 로 요약 기록. Interface 주석.
- 파서 `ParseFileName` 8칸 고정 + `$Fab$` 꼬리표 + 빈 칸 / Test Date 검사 + Slot 기본값 실패 경고. 업로드 화면 요약 2줄 (파일명 칸 / Lot-Wafer No), 18항목 전송, 목록 열 제목 (Test Type / Test Date / Customer / 파일 Device / Wafer Type / Wafer Rev / Fab / Pass Die / Fail Die / 미측정). 조회 창 정보 3줄 (Lot-Wafer No / 면 / 파일명 칸 / Pass·Fail).
- 검증: `Verify-EtResultUpload.ps1` -> `ET_UPLOAD_FORM_SMOKE=PASS` (추가: 요약 파일명 8칸, DB 파일명 칸 / 규칙 2 / BACK / FILE_PREFIX NULL, DB 요약 20 / 4 / 17, 목록 열 제목, 조회 창 정보, 새 규칙 이름 `...U9-HS2633-01$Silterra$.xlsx` 분석 (Fab Silterra / Slot 기본값 1 / WAFER_ID U9-HS2633-01 / 내용 Wafer ID 다름 경고) + 저장 (FAB_NAME / LOT / WAFER_ID / Test Date / 규칙 2), 7칸 이름 거부) + `ET_LIMIT_FORM_SMOKE=PASS` (12항목 이전 경로 업로드 포함) + `GdsEtResultProbe` 18/18 (12항목 호출 호환).

### 13.8 화면 정리 / Device = 파일명 Device (2026-10-12 사용자 지시)

사용자 지시: "문자열로 보여 주지 말고 text box 로 정리해서 보여줘. device 는 선택이 아니라 파일 이름에 있는 device 로 한다."

- **N3 변경**: DEVICE_ID = 파일명 2칸 Device (`OMM`). 업로드 화면 Device 칸은 읽기 전용 표시 (선택 / GDS Device 목록 조회 제거). FILE_DEVICE 와 DEVICE_ID 가 같은 값으로 저장된다. 이전에 사용자가 고른 Device 로 저장된 파일 (SCK / `OMM_Hybrid` / RH-HS2615-02) 은 그대로이며, 등록 목록 (Factory + Device 기준) 에는 `OMM` 으로 조회할 때 나오지 않는다. Limit 설정 (Device 단위) 도 `OMM` 으로 다시 정해야 한다.
- 업로드 화면 분석 결과: 여러 줄 문자열 대신 "파일 정보" 그룹에 항목별 읽기 전용 칸 4열 x 4줄 (Customer / Wafer Type / Wafer Rev / Fab, Test Type / Test Date / Lot / Lot-Wafer No, Die / Pass Die / Fail Die / 시트, 측정 항목 / 측정값 / 미측정 / 빈 칸) + 경고 칸 (없으면 "경고 없음", 있으면 붉은 글씨). Limit 안내는 그룹 제목으로.
- 검증: Smoke `ET_UPLOAD_FORM_SMOKE=PASS` / `ET_LIMIT_FORM_SMOKE=PASS` (Device 칸 = OMM 읽기 전용, 정보 칸 값 16개, 경고 칸, Factory 미입력 막음, 저장 Device = OMM, 새 규칙 파일 정보 칸 / 경고). 화면 이미지 확인.

---

## 14. 고객 메일 (10-08 회의 답변, 2026-10-12) 반영

사용자 결정 (2026-10-12): 1) "TSVET -> COWET -> FT 자체가 공정이므로 공정으로 두고 조합해서 Key" 2) "'-' 측정 Fail 로 두고 그냥 Fail" 4) "OMM = 디바이스. U9 는 자재 코드 느낌, GDS 에서만 사용 중 - 고민" (3) Spec 단위 / '-' 범위는 15절).

- 중복 키: UNIQUE (FACTORY, WAFER_ID, TEST_PROGRAM) — DDL `Persistence/2026-10-12_tqp_gds_src_file_uq_wafer_test.sql` 로컬 적용. 같은 Wafer 의 TSVET / COWET / FT 는 따로 저장, 같은 공정이면 재등록 질문. `GetEtFileByKey` 3항목 (Wafer 전체 공정) / 4항목 (+ 공정) 둘 다 받음, 새 쿼리 `SELECT_SRC_FILE_BY_WAFER_TEST`. 화면 제목 "공정 (Test Type)".
- '-' = Fail: Limit 목록 `FAIL_COUNT` (Enable Limit 범위 밖 + '-', Limit 없어도 '-' 는 Fail), Limit 화면 열 "Fail", 업로드 / 조회 창 표시 "'-' (Fail)", 조회 창 '-' 셀 붉은 배경. DB 컬럼 (NOT_TESTED_COUNT / VALUE_STATUS N) 은 그대로.
- Slot 기본값: Lot-Wafer No 끝의 `-숫자` (고객 예시 COWET / FT `HS2606-02` 처럼 Lot 으로 시작하지 않아도 됨).
- 검증: Smoke 업로드 PASS (추가: 같은 Wafer COWET / FT 따로 저장, FT 꼬리표 없음, 같은 공정 재저장 질문, 삭제 단계 보정) / Limit PASS (Fail = 범위 밖 + '-') / Probe 19/19 (같은 Wafer 다른 공정 저장).


## 15. 미정 항목 결정 반영 (2026-10-12)

| 번호 | 사용자 결정 | 반영 |
| --- | --- | --- |
| Spec 단위 | 디바이스 + 공정 + Parameter(헤더 전체) | [Limit 설계](2026-10-07_ETest_Limit_설정_설계.md) 12절 |
| '-' | **정의된 항목의 '-' 만 Fail** (14절 "'-' = Fail" 을 좁힘) | 업로드 / 조회 창 표시를 "'-' 값" 으로 되돌림, 조회 창 '-' 셀 회색. Fail 은 Spec 화면(정의 + Enable 항목)에서만 센다 |
| '#' | 헤더 원문을 항목명으로 쓰고 끝 `#번호` 는 지운다 | `EtResultFileParser.NormalizeHeader` (연속 공백 1칸 + `\s*#\S*\s*$` 제거, 대소문자 유지). 정리 후 같은 헤더가 두 번 나오면 기존 규칙대로 "중복" 오류. 지금까지 저장된 항목에는 '#' 이 없었다(0건 확인) |
| DEVICE_ID | U9 / OMM 보류 -> **2026-10-13 OMM 으로 종결** | 파일명 Device(OMM) 유지. GDS 대표 Device(U9)와는 Sub Device 연결(CLAUDE-055)로 잇는다 |
| 6 Chain 연결 | GDS Chain 은 Input 값의 Chain 을 대표로 둔다 -> Spec 설정 시 2개 Chain 을 묶는 기능. 이어서 "대표 chain 보다는 그냥 묶는 기능, Setup 된 Chain 에서 찾으면 묶인 Chain 의 Limit 등 정보를 전부 찾는다" | [Limit 설계](2026-10-07_ETest_Limit_설정_설계.md) 15절 (TQP_GDS_ET_CHAIN_GROUP, 13절 대표 Chain 은 대체) |
| 7 이전 데이터 | **A. 삭제하고 새 규칙으로 다시 올림** | 로컬 DB: SCK / OMM_Hybrid 파일 번호 25(규칙 1) 삭제 + 고아가 된 OMM_Hybrid 측정 항목 2,755행 삭제 -> 같은 샘플을 업로드 화면과 같은 순서(18항목)로 다시 저장: **파일 번호 94 / SCK / Device OMM / WAFER_ID RH-HS2615-02 / TSVET / 규칙 2 / READY / Die 24 (Pass 20 / Fail 4) / 측정값 66,120 ('-' 17)**. 등록자는 이전과 같은 ADMIN |
| 8 DB 시간 | **A. 그대로 두고 고객 DB(KST)에서만 확인** | 로컬 docker DB 는 UTC(PC 보다 9시간 늦음). 코드 / 쿼리는 SYSDATE 그대로, 고객 DB 에서 등록 시각만 확인 |
| 5 | $Silterra$ 의미 | 2026-10-13 회의록으로 정리: 파일에 Silterra Die 만 있다는 표시 (FT 는 Silterra 만 측정해 생략). Die# 대응은 [Die 좌표 Setup 설계](2026-10-13_Die좌표_대응_Setup_설계.md) (CLAUDE-081) |
| 8(측정 종류) | 다른 측정 종류 Spec | 답변 대기 (지금 상태 유지) |
