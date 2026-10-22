# E-Test Para Setup 화면 설계 (Header -> Parameter 등록 / 대표 Chain 등록 / GDS Chain 연결)

작성: 2026-10-14 Claude (CLAUDE-086 설계 / CLAUDE-090 구현) / 상태: **설계 v2 확정 + 이름 변환 규칙(5.1절) — 구현 1~5단계 완료(서버 / Para 화면 / 메뉴 / Limit 화면 정리 + 2단계 DDL / Upload 재설계 연결, Probe 34/34 · Para Smoke 27/27 · Limit Smoke 43/43 · Upload Smoke 19 · Upload Probe 18/18, 배포 / DMS 화면 확인 대기, 직접 시험 = [테스트 시나리오](2026-10-14_ETest_Para_Spec_Upload_테스트_시나리오.md)) (CLAUDE-090)**. 결정 P1 ~ P10 모두 확정 (P3 · P9 · P10 사용자 지시, 나머지는 "설계 넣고 개발 시작" 지시로 권장안)

> 2026-10-14 (CLAUDE-095): 화면 재구성 설계 [E-Test Para 화면 재구성](2026-10-14_ETest_Para_화면_재구성_설계.md) 가 확정되면 이 문서의 6절(대표 Chain) / 7절(GDS Chain 연결) / 8.4절([대표 Chain] 탭)과 대표 Chain 테이블 2개는 **Parameter 별 GDS Chain 이름 한 칸**으로 대체되고, Spec 편집은 [등록 Parameter] 탭으로 옮겨진다(E-Test Limit 설정 화면 삭제).

사용자 지시 (2026-10-14):
- "e Test 결과 File 기반으로 Header 를 Parameter 로 등록하는 화면에 대해 설계 진행한다. e test Limit Setup 화면에 이어서 작업하면 된다."
- "회의록 내용 참고하면 Header 에 대한 전체 Full Name 으로 등록하면 된다."
- "Paraname 에서 chain 을 추출해서 GDS Chain 과 Mapping 할 수 있는 구조로 만들어야 한다. Mapping 기준 정보는 회의록 내용 기반으로 우선 찾아서 설계해줘."
- (v1 검토 후) "TOP1_P_FH / TOP1_N_FL 양 끝단자인데 GDS 에서는 TOP1_P_FH 또는 TOP1_N_FL 로 등록이 되니 대표 chain 에 대한 등록 기능이 필요하다."
- (v2 검토 후) "묶는 방식은 사용자가 이름을 입력하는 방식으로 한다. 자동으로 묶는 방식은 본 프로젝트에서 작업할 예정이다."
- (구현 지시) "parameter 명 및 chain 관련된 이름은 DB 및 소스에서 문제 없이 사용할 수 있도록 변환해서 넣어야 한다. 단, 엔지니어가 확인할 수 있도록 크게 벗어나지 않는 한에서.. 설계 넣고 개발 시작 하자"
- (화면 확인 후) "디바이스, 공정은 파일에서 가져오므로 선택이 아니라 readonly다. Data가 많기 때문에 파일헤더 grid 에서 드래그 후 오른쪽 마우스로 삭제 기능 넣어줘. 열은 좌측 헤더로 이동"
- (용어) "단자1은 결국 chain 이름이다."

수정 이력:
- v1: Chain 이름에서 Force leg(`_FH` / `_FL`)를 뗀 Chain Key 규칙으로 Chain 을 자동으로 묶고 GDS Chain 과 연결.
- v2: 한 Chain 의 양 끝 Chain 이름은 Target region 까지 다를 수 있고(`TOP1_P_FH` / `TOP1_N_FL`), GDS 는 Input 쪽 Chain 이름 하나로 Chain 을 등록한다 -> **대표 Chain 등록**. 처음에는 GDS 영역 Label / 이름 규칙 제안을 넣었다.
- v2 결정 반영: 묶기는 **사용자가 Chain 을 고르고 대표 이름을 입력**한다. 자동 묶기(제안)는 **본 프로젝트**로 미룬다(12절). GDS 연결은 이름이 똑같을 때만 잇는 비교라 그대로 둔다(7절).
- **구현 시작 (이 문서)**: Parameter / Chain / 대표 Chain 이름을 **DB · 소스에서 그대로 쓸 수 있는 이름으로 변환해 저장**한다(5.1절). 원문 헤더는 따로 보관한다. 1단계 DDL 은 기존 업로드 / Limit 화면이 계속 동작하도록 **추가만** 한다(4.7절).
- 화면 레이아웃 / 용어 (10-14, 구현 3단계 뒤): Device / 공정은 파일명에서 가져오는 **읽기 전용** 칸(콤보 삭제). [파일 헤더] 표는 행 단위 드래그 선택 + **우클릭 / Del 로 목록에서 빼기**(화면에서만), "열" 은 **왼쪽 행 머리글**로. "단자" 를 **Chain**(E-Test Chain 이름)으로 통일: 컬럼 `TQP_GDS_ET_REP_CHAIN.TERMINAL_NAME` -> `CHAIN_NAME`(로컬 DB RENAME), 서비스 `GetEtTerminalList` -> `GetEtChainList`, 쿼리 `SELECT_ET_TERMINAL_LIST` -> `SELECT_ET_CHAIN_LIST`.

선행 문서:
- [E-Test Limit(Spec) 설정](2026-10-07_ETest_Limit_설정_설계.md) 12 · 13 · 15절 (Spec 키 / 대표 Chain 첫 안 / Chain 묶기 / Setup Chain 으로 찾기)
- [E-Test Upload 재설계](2026-10-13_ETest_Upload_재설계.md) 9절 (정의 Header 만 저장, 구현 대기)
- [E-Test Upload YMS 적재](2026-10-14_ETest_Upload_YMS적재_설계.md) (같은 `TQP_GDS_ET_ITEM` 에 YMS 이름 추가 예정)
- [E-Test Parsing / DB](2026-10-07_ETest_결과파일_Parsing_DB설계.md) 10절 (헤더 분해 / `TQP_GDS_ET_ITEM`)
- [Chain Input Label 자동 이름](../02_GDS/2026-10-08_Chain_Input_Label_자동이름_설계.md) (GDS Chain 이름 = Input Label)
- [GDS 대표 Device / Sub Device](../02_GDS/2026-10-12_GDS_대표Device_SubDevice_설계.md) (OMM / OMM_Hybrid -> U9)

---

## 1. 결론

1. **Para Setup 화면**(`frmEtParaSetup`)에서 결과 파일을 열면 헤더 전체를 보여 주고, 고른 헤더를 **Parameter(헤더 전체 이름)** 로 등록한다. 등록 단위는 Device + 공정 + Parameter 다.
2. Parameter 이름의 `@` 뒤를 **E-Test Chain 이름**(예 `TSVDC_TOP1_P_FH`)으로 꺼낸다.
3. 사용자가 한 물리 Chain 의 Chain 들을 고르고 **대표 Chain 이름을 입력**해서 묶는다(예: Chain `TSVDC_TOP1_P_FH` + `TSVDC_TOP1_N_FL` -> 대표 `TSVDC_TOP1`). 자동 묶기는 하지 않는다.
4. **대표 Chain ↔ GDS Chain 은 1:1** 이다. GDS Chain 이름이 대표 이름 또는 **구성 Chain 이름 중 하나와 똑같으면** 연결된다. 그래서 GDS 가 `TOP1_P_FH` 로 등록하든 `TOP1_N_FL` 로 등록하든 같은 대표 Chain 에 붙는다. 이름이 안 맞으면 사용자가 GDS Chain 을 골라 연결한다.
5. 작업 순서는 **Para Setup -> Limit(Spec) 설정 -> 결과 업로드** 다. Fail 값은 Parameter -> Chain -> 대표 Chain -> GDS Chain 으로 찾아간다.

## 2. Mapping 기준

### 2.1 근거

| 일자 / 출처 | 내용 | 설계 반영 |
| --- | --- | --- |
| 10-02 상견례 회의록 | E-Test Excel 에는 Chain 이름과 측정 결과가 있고, GDS Label 텍스트에도 같은 Chain 이름이 있어 두 데이터를 연결할 수 있다. Chain 이름은 **Input Chain 에만** 표기된다. Fail Chain 은 다른 색으로 표시 | Chain 이름 = GDS Label (M3), GDS 이름 = Input Chain (M5) |
| 10-06 데이터 기준 협의 (10-12 메일 10-06 정리 3번) | E-Test 는 Chain 별 Limit 화면 필요. **Fail Chain 이름은 GDS 설정 시 생성되는 Chain 과 1:1 Mapping 필요** | 1:1 연결 (M6) |
| 10-08 회의 질문 3 (10-12 메일 회신) | 질문: In / Out 별로 Chain 이름이 2개씩 생겨 어느 이름으로 정의할지 모호함 (`TSVDC_TOP1_P_FH`, `TSVDC_TOP1_N_FL` 예). 회신: RES = Measurement type + Input current force + **Chain type + Target region + Force leg**, LKG = Measurement type + Connection type + Target region 1_Force leg 1 : Target region 2_Force leg 2 | Chain 이름 분해 (5절) |
| 10-08 회의 회신 (10-12 메일) | U9 Measurement type 12종: TSVDC / C4DC / ASICDC / HBMDC RES·LKG, PIC_EDM RES, PIC_SERP RES·LKG, PIC_COMB LKG | Chain type 목록 (5절) |
| 10-08 질문 4 회신 | 지금은 TSVDC Chain 용 Layer(3 / 5 / 775)만 반영. ASICDC / HBMDC / PIC 등은 Layer 추가 필요 | GDS Chain 이 없는 대표 Chain 이 생긴다 -> "GDS 없음" 상태 |
| 10-12 사용자 요청 (Limit 설계 13.1) | "현재 Setup 되는 Chain 은 In 값에 해당하는 Chain 정보를 대표로 두고 있다. Spec 설정 시 2개의 Chain 을 대표 Chain 으로 묶는 기능이 필요" | 대표 Chain 개념 (M4) |
| 10-13 후속 협의 회의록 | **헤더의 Parameter 이름을 그대로 등록**, 고객이 준 약 120개만 Spec. **Spec Out 이 나면 해당 Chain 을 찾고 GDS Chain 정보로 Correlation**. 본 프로젝트 Spec 키 Device + 공정 + Test Type + Parameter 제안 | 등록 단위 (M1), Parameter -> Chain -> GDS Chain 흐름 |
| 10-13 후속 협의 회의록 | KLARF 의 OMM, OMM_Hybrid = U9 한 Device | Device = Sub Device (M6) |
| 10-12 사용자 결정 (Limit 설계 12절) | 헤더 원문을 Parameter 로, 끝 `#번호` 는 지우고, 등록은 대문자 | 이름 정리 (M1) |
| **10-14 사용자 지시** | `TOP1_P_FH` / `TOP1_N_FL` 은 양 끝 Chain 이고 GDS 에서는 둘 중 하나로 등록된다 -> 대표 Chain 등록 기능. **묶기는 사용자가 이름 입력, 자동 묶기는 본 프로젝트** | M4 / M5, 12절 |
| CLAUDE-049 / 055 구현 | GDS Chain 이름 = Input 근처 Label 자동 지정(`TSVDC_TR_FH`). Sub Device 연결 `TQP_GDS_MAP_SUB_DEVICE` (SCK: OMM -> U9, OMM_Hybrid -> U9) | |

### 2.2 기준 (M1 ~ M6)

| 번호 | 기준 |
| --- | --- |
| M1 | **Parameter = 헤더 전체 이름.** 앞뒤 공백 제거 / 연속 공백 1칸 / 끝 ` #번호` 제거 / 대문자 (예 `RESISTANCE_IFORCE=8.0MA@TSVDC_TR_FH`). 화면에는 처음 등록한 원문을 보여 준다 |
| M2 | **Chain 이름 추출**: `@` 뒤. `:` 가 있으면 Chain 1 / Chain 2 (Leakage_Serial) |
| M3 | **E-Test Chain 이름 = GDS Label 텍스트** (Force leg 포함 그대로, 예 `TSVDC_TOP1_N_FL`) |
| M4 | **대표 Chain** = 한 물리 Chain 의 Chain 묶음 + 대표 이름. **사용자가 Chain 을 고르고 이름을 입력**한다. Chain 하나는 대표 Chain 하나에만 들어간다 |
| M5 | **GDS 연결**: GDS Chain 이름(대문자 정리)이 대표 이름과 같거나 **구성 Chain 이름 중 하나와 같으면** 연결. 아니면 사용자가 고른다 |
| M6 | **1:1**: 대표 Chain 1개 ↔ GDS Chain 1개. 대표 Chain 과 연결은 GDS 대표 Device(U9) 단위 (E-Test Device OMM -> Sub Device -> U9) |

### 2.3 대표 Chain 을 등록하는 이유

| 이유 | 내용 |
| --- | --- |
| 양 끝 Chain 이름이 Target region 까지 다를 수 있다 | 사용자 확인: `TSVDC_TOP1_P_FH` / `TSVDC_TOP1_N_FL` 이 한 Chain 의 양 끝. Force leg 만 떼는 규칙으로는 `TOP1_P` / `TOP1_N` 으로 갈린다 (v1 규칙의 한계) |
| GDS 이름은 Input Chain 하나다 | Chain 을 그릴 때 Input 을 어느 끝에 두느냐에 따라 `TOP1_P_FH` 또는 `TOP1_N_FL` 이 된다. 대표 Chain 에 두 Chain 이 모두 들어 있으면 어느 쪽이든 연결된다 |
| 공정마다 Chain 조합이 다르다 | Leakage_Serial: TSVET `TSVDC_ASIC1_N_FH:TSVDC_ASIC1_P_FL` / FT `TSVDC_ASIC1_N_FL:TSVDC_ASIC1_P_FH #421`. Chain 단위로 대표를 정해 두면 공정과 관계없이 같은 GDS Chain 으로 간다 |
| 고객 Naming 이 아직 정리 중이다 | C4DC / ASICDC / HBMDC / PIC Naming 과 Layer 는 연구소가 정리 중(10-08 회신). POC 는 사람이 정한 값으로 관리하고 규칙 / 자동화는 본 프로젝트에서 한다 |

### 2.4 이전 설계와의 관계

| 시점 | 방식 | 이번 설계 |
| --- | --- | --- |
| Limit 설계 13절 (10-12) | E-Test Chain -> 대표 Chain 이름(`TQP_GDS_ET_CHAIN_MAP`, Device = OMM) | 대표 Chain 개념을 다시 쓴다. 다른 점: 단위 = GDS 대표 Device(U9), **GDS Chain 과 1:1 연결 테이블** |
| Limit 설계 15절 (10-12, 지금 구현) | 대표 이름 없이 묶음 번호(`TQP_GDS_ET_CHAIN_GROUP`) + FH / FL 자동 묶기 + Setup Chain 이름 규칙 찾기(EXACT / SUFFIX / GROUP) | 대표 Chain 으로 대체 (P8). FH / FL 자동 묶기도 본 프로젝트로. 로컬 0행 |
| 이 문서 v1 / v2 초안 (10-14) | Chain Key 규칙 자동 묶기 / GDS 영역 Label · 이름 규칙 제안 | 본 프로젝트로 (12절) |

## 3. 로컬 데이터로 본 결과 (조회 / 계산만)

| 대상 | 결과 |
| --- | --- |
| 샘플 TSVET 파일 헤더 (`SCK / OMM`, 2,755개) | Chain 이름 1,306개 (FH 879 / FL 427, Force leg 없는 Chain 0). 종류 C4DC / TSVDC / PIC |
| 고객 Spec 표 (`Verification/EtSpecTable_U9_20261012.txt`, TSVET 37 / COWET 40 / FT 42 = 119행) | 분해 실패 0 / **Chain 이름 52개** (모두 TSVDC) |
| 로컬 GDS U9 Setup Chain (현재 READY 15개) | 이름에 `_FH` 붙음 10 / 안 붙음 5 (`TSVDC_TOP1_P` / `TSVDC_TOP1_N` / `TSVDC_L_N` / `PIC_COMB12_M3` / `PIC_COMB12_M4`). 모두 Input 만 있고 Output 없음 |
| Spec 표 Chain 52개 중 GDS Chain 이름과 똑같은 Chain | 10개 (`TSVDC_ASIC1_N_FH` / `ASIC1_P_FH` / `ASIC2_N_FH` / `ASIC2_P_FH` / `ASIC4_N_FH` / `ASIC4_P_FH` / `HBM0_N_FH` / `L_P_FH` / `TOP2_P_FH` / `TR_FH`). 대표 Chain 에 이 Chain 이 들어가면 바로 연결된다 |
| 이름이 안 맞는 GDS Chain | `TSVDC_TOP1_P` / `TSVDC_TOP1_N` / `TSVDC_L_N` (사람이 고친 이름) -> 대표 이름을 같게 입력하거나 [GDS 선택 연결] |

## 4. 데이터 구조

### 4.1 관계

```text
E-Test 쪽 (Device = 파일명 Device, 예 OMM)          GDS 대표 Device 쪽 (예 U9)
TQP_GDS_ET_ITEM  Parameter 마스터                    TQP_GDS_ET_REP_CHAIN (신규)              TQP_GDS_CHAIN / _VER
  (Device + 공정 + Parameter)                          Chain 이름 -> 대표 Chain 이름 (사용자 입력)   (Setup Chain)
  CHAIN_NAME / CHAIN_NAME2 (Chain) ── 이름 비교 ──>   TQP_GDS_ET_CHAIN_LINK (신규) ── FK CASCADE ──> CHAIN_SEQ
  ▲ FK                                                 대표 Chain 이름 -> CHAIN_SEQ (1:1)
TQP_GDS_ET_LIMIT  Spec                  E-Test Device -> TQP_GDS_MAP_SUB_DEVICE -> GDS 대표 Device
TQP_GDS_ET_VALUE  측정값 (ET_ITEM_SEQ)
```

찾아가는 순서: Fail 값 -> `ET_ITEM`(Chain 이름) -> Sub Device 로 U9 -> `ET_REP_CHAIN`(대표 Chain) -> `ET_CHAIN_LINK` -> GDS Chain (도형 / Member).

### 4.2 `TQP_GDS_ET_ITEM` 을 Parameter 마스터로 재정의 (P1)

지금 `TQP_GDS_ET_ITEM` 은 업로드가 헤더마다 만드는 "측정 항목 마스터"(Device 단위)이고 측정값(`ET_VALUE`)이 이 번호를 참조한다. Upload 재설계 S4 에서 이미 "정의 Parameter 만 등록" 으로 좁혔다. 여기서 **Para Setup 이 등록하는 Parameter 마스터**로 쓴다. 새 테이블을 만들면 같은 헤더 정보가 두 곳에 생긴다.

| 컬럼 | 지금 | 바꾼 뒤 |
| --- | --- | --- |
| ET_ITEM_SEQ | PK, 시퀀스 | 그대로 (`ET_VALUE` 가 참조) |
| FACTORY / DEVICE_ID | | 그대로 (E-Test Device = 파일명 Device) |
| **TEST_PROGRAM** | 없음 | 추가. 공정 TSVET / COWET / FT, `VARCHAR2(50 CHAR)` NOT NULL, 대문자 |
| ITEM_NAME | 헤더 원문 | **Parameter 이름 = 5.1 변환 이름** (예 `RESISTANCE_IFORCE_8P0MA_TSVDC_TR_FH`, 50자 이하). Spec(`TQP_GDS_ET_LIMIT.ITEM_NAME`) / YMS 컬럼 이름과 같은 값 |
| **HEADER_TEXT** | 없음 | 추가. 처음 등록한 헤더 원문 (공백 정리 / 끝 `#번호` 제거, 대소문자 유지, 화면 표시용) `VARCHAR2(200 CHAR)` |
| PARA_NAME / COND_VALUE | `=` 앞 / 뒤 | PARA_NAME 은 변환 이름(`RESISTANCE_IFORCE`), COND_VALUE 는 값 그대로 대문자(`8.0MA`, 화면 표시용) |
| **MEAS_TYPE** | 없음 | 추가. Measurement type (`RESISTANCE` / `CONNECTIVITY` / `CONTINUITY` / `LEAKAGE`) `VARCHAR2(30 CHAR)` |
| CHAIN_NAME / CHAIN_NAME2 | Chain 원문 | **Chain 이름 1 / 2** = 5.1 변환 이름 (Force leg 포함, 예 `TSVDC_TOP1_N_FL`) |
| **CHAIN_TYPE** | 없음 | 추가. Chain type (`TSVDC` / `C4DC` / `PIC_SERP` …, Chain 1 기준) `VARCHAR2(30 CHAR)` — 화면 거르기용 |
| **FORCE_LEG / FORCE_LEG2** | 없음 | 추가. `FH` / `FL` / NULL (`VARCHAR2(4 CHAR)`) — 화면 표시용 |
| UNIT / TEST_NO / DISPLAY_ORDER | | 그대로 (등록한 파일 기준) |
| **USE_FLAG** | 없음 | 추가. `Y` / `N` (기본 Y). N 이면 다음 업로드부터 값을 저장 / 판정하지 않는다. 이미 저장된 값은 보관 |
| **REG_SOURCE / REG_FILE_NAME** | 없음 | 추가. 등록 출처 `FILE`(결과 파일) / `SPEC`(고객 Spec 표 붙여넣기) + 등록에 쓴 파일 이름 |
| **REMARK** | 없음 | 추가 `VARCHAR2(200 CHAR)` |
| 감사 컬럼 | | 그대로 |

- 키: `UQ (FACTORY, DEVICE_ID, ITEM_NAME)` -> **`UQ (FACTORY, DEVICE_ID, TEST_PROGRAM, ITEM_NAME)`** (같은 헤더라도 공정마다 따로 등록, P2).
- **등록 Parameter = TEST_PROGRAM 이 있는 행.** 지금 업로드가 만든 예전 행(TEST_PROGRAM NULL, ITEM_NAME 원문)은 Upload 재설계 구현 때 로컬 데이터 정리(S5)와 함께 지우고 TEST_PROGRAM 을 NOT NULL 로 바꾼다. 그 전까지는 두 종류가 같은 테이블에 있다(원문에는 `@` 가 있어 변환 이름과 겹치지 않음).
- CHECK (등록 행만): ITEM_NAME / CHAIN_NAME / CHAIN_NAME2 = `^[A-Z][A-Z0-9_]*$`, ITEM_NAME 50자 이하, TEST_PROGRAM 대문자, HEADER_TEXT / REG_SOURCE 필수. 공통: `USE_FLAG IN ('Y','N')` / `REG_SOURCE IN ('FILE','SPEC')` / `FORCE_LEG IN ('FH','FL')` (NULL 허용).
- 색인: `(FACTORY, DEVICE_ID, CHAIN_NAME)` 은 이미 있음 + `(FACTORY, DEVICE_ID, CHAIN_NAME2)` 추가 — Chain 이름으로 Parameter 찾기.

### 4.3 `TQP_GDS_ET_REP_CHAIN` (신규, Chain -> 대표 Chain)

| 컬럼 | 형식 | 설명 |
| --- | --- | --- |
| FACTORY | VARCHAR2(30 CHAR) PK | |
| DEVICE_ID | VARCHAR2(50 CHAR) PK | **GDS 대표 Device** (U9). OMM / OMM_Hybrid 가 같이 쓴다 (P4) |
| CHAIN_NAME | VARCHAR2(64 CHAR) PK | E-Test Chain 이름 (5.1 변환 이름, CHECK `^[A-Z][A-Z0-9_]*$`) |
| REP_CHAIN_NAME | VARCHAR2(64 CHAR) NOT NULL | 대표 Chain 이름 (**사용자 입력을 5.1 규칙으로 변환**, 같은 CHECK). 같은 값끼리 한 물리 Chain |
| REMARK | VARCHAR2(200 CHAR) | |
| 감사 컬럼 | | CREATE_USER / CREATE_TIME / UPDATE_USER / UPDATE_TIME |

- 색인 `(FACTORY, DEVICE_ID, REP_CHAIN_NAME)`.
- PK 가 Chain 이름이라 **Chain 하나는 대표 하나에만** 들어간다. 대표 Chain 은 Chain 이 1개 이상 있을 때만 존재한다(따로 마스터 행 없음).
- Chain 이름만 저장하므로 Parameter 를 지워도 대표 Chain 은 남는다(같은 Chain 을 다시 등록하면 그대로 이어진다).
- 묶은 방법(수동 / 자동) 컬럼은 두지 않는다. 자동 묶기를 넣는 본 프로젝트에서 추가한다.

### 4.4 `TQP_GDS_ET_CHAIN_LINK` (신규, 대표 Chain -> GDS Chain)

| 컬럼 | 형식 | 설명 |
| --- | --- | --- |
| FACTORY / DEVICE_ID | PK | GDS 대표 Device |
| REP_CHAIN_NAME | VARCHAR2(64 CHAR) PK | 대표 Chain 이름 |
| CHAIN_SEQ | NUMBER NOT NULL | GDS Chain 번호. **`UQ (FACTORY, DEVICE_ID, CHAIN_SEQ)`** -> GDS Chain 하나에 대표 하나 (1:1, P5). FK -> `TQP_GDS_CHAIN (FACTORY, DEVICE_ID, CHAIN_SEQ)` **ON DELETE CASCADE** |
| GDS_CHAIN_NAME | VARCHAR2(200 CHAR) | 연결할 때의 GDS Chain 이름 (이름이 바뀐 것을 알아내는 용도) |
| LINK_TYPE | VARCHAR2(10 CHAR) | `NAME`(이름 일치) / `MANUAL`(사용자가 고름). CHECK |
| REMARK / 감사 컬럼 | | |

- 연결이 없는 대표 Chain 은 행이 없다. 상태(GDS 없음 / 후보 여러 개)는 조회 때 계산한다.
- FK CASCADE 라서 GDS 쪽에서 Chain 을 지우거나 Map 을 지우고 다시 등록하면 연결만 지워지고 대표 Chain 은 남는다. GDS 서비스 소스는 바꾸지 않는다.
- 대표 Chain 의 Chain 이 모두 빠지면 서비스가 연결 행도 지운다(대표 마스터가 없어 FK 를 걸지 않음).
- 이름 `TQP_GDS_ET_CHAIN_MAP` 은 10-12 에 쓰다 없앤 테이블과 헷갈리므로 쓰지 않는다.

### 4.5 `TQP_GDS_ET_LIMIT` (Spec) 와의 관계 (P6)

- Spec 키 `(FACTORY, DEVICE_ID, TEST_PROGRAM, ITEM_NAME)` 가 Parameter 마스터 키와 같아진다 -> **FK 로 묶는다** (`ON DELETE CASCADE`). Spec 은 등록된 Parameter 에만 둘 수 있다.
- Limit 설계 4절의 "측정 항목과 FK 로 묶지 않는다" 는 Para 단위 Spec 일 때의 판단이다. 지금은 키가 같아서 묶는 편이 어긋나지 않는다.
- 로컬 `TQP_GDS_ET_LIMIT` 은 0행이라 옮길 데이터가 없다.
- **적용 (10-14, 4단계)**: `FK_GDS_ET_LIMIT_PARA` (`Persistence/2026-10-14_tqp_gds_et_limit_fk.sql`). `SaveEtLimit` / `DeleteEtLimit` 은 Parameter 로 저장 이름과 헤더 원문(`@` 포함 -> `EtParameterNameRule` 로 저장 이름)을 모두 받고, 등록되지 않은 Parameter 는 서비스가 먼저 모아서 거부한다(메시지에 Para 설정 안내). 단위가 비면 등록 단위.

### 4.6 `TQP_GDS_ET_CHAIN_GROUP` (Chain 묶기) 정리 (P8)

대표 Chain 이 묶음 역할을 하므로 테이블 / 쿼리 XML / DSL 을 지우고, Limit 화면의 [Chain 묶기] / [Setup Chain 으로 찾기] 탭은 Para Setup [대표 Chain] 탭으로 옮긴다. 로컬 0행.

**완료 (10-14, 4단계)**: 로컬 DB `TQP_GDS_ET_CHAIN_GROUP` / `ET_CHAIN_GROUP_SEQ` DROP(PURGE 없이), `TQP_GDS_ET_CHAIN_GROUP.xml` / `.cs`(DSL csproj Compile 1행) 삭제, 서비스 5개 삭제(`GetEtChainGroupList` / `GetEtGdsChainList` / `CreateEtChainGroups` / `DeleteEtChainGroup` / `GetEtChainMatch`), `SELECT_ET_LIMIT_BY_CHAIN` 삭제, Limit 화면 탭 2개 삭제.

### 4.7 DDL (단계별)

지금 배포된 업로드 / Limit 화면은 예전 `ET_ITEM`(원문 헤더) 과 `ET_CHAIN_GROUP` 을 쓴다. 중간 리뷰 전에 이 화면들이 멈추지 않도록 DB 변경을 둘로 나눈다.

| 단계 | 파일 | 내용 | 기존 화면 영향 |
| --- | --- | --- | --- |
| 1 (이번) | `Persistence/2026-10-14_tqp_gds_et_para_rep_chain.sql` | `ET_ITEM` 컬럼 추가(NULL 허용, USE_FLAG 기본 Y) / UQ 교체 / 등록 행 CHECK / CHAIN_NAME2 색인 + `ET_REP_CHAIN` / `ET_CHAIN_LINK` 생성 | 없음 (추가만. 예전 업로드 MergeEtItem 은 TEST_PROGRAM NULL 행을 계속 만든다) |
| 2a (Limit 화면 정리, **10-14 로컬 적용**) | `Persistence/2026-10-14_tqp_gds_et_limit_fk.sql` | 사전 확인(등록 Parameter 없는 Spec / Chain 묶음 행이 있으면 아무것도 바꾸지 않고 멈춤) -> `ET_LIMIT` FK -> `ET_CHAIN_GROUP` / 시퀀스 DROP | 이전 버전 Limit 화면(Chain 묶기 탭 / 헤더 원문 키 저장)은 동작하지 않는다 -> 서버 / 클라이언트를 같이 배포 |
| 2b (Upload 재설계 구현 때, **10-14 로컬 적용**) | `Persistence/2026-10-14_tqp_gds_et_local_reset.sql` -> `2026-10-14_tqp_gds_et_upload_registered.sql` | 로컬 E-Test 데이터 정리(S5: 파일 / Die / 값 + 예전 측정 항목 13,775) -> `ET_ITEM.TEST_PROGRAM` NOT NULL + Upload 재설계 컬럼 | 업로드 화면을 같이 바꾼다(완료) |

- Upload 재설계 DDL(`2026-10-13_tqp_gds_et_defined_only.sql`)과 YMS 적재 DDL(`2026-10-14_tqp_gds_et_yms.sql`)은 2b 와 같은 날 **Para 2b -> Upload 재설계 -> YMS** 순서로 적용한다(데이터 정리 한 번, YMS UQ 는 TEST_PROGRAM 이 NOT NULL 이 된 뒤).

## 5. Parameter 이름 분해 규칙

규칙은 **클래스 1개**(신규 정적 클래스 `EtParameterNameRule`, `02_DMS/DACrux.SEMDMS.Interface/ETest/`)에 둔다. `DieCoordinateMapper` / `EtYmsRule`(YMS 적재 설계)과 같은 방식으로, 화면 미리보기와 서버 등록이 같은 클래스를 쓴다. 서버는 등록 때 화면 값을 믿지 않고 이 클래스로 다시 분해해 저장한다. 헤더 정리(`NormalizeHeader`)는 지금 업로드 파서(ENGUI)와 BSL 에 따로 있다. 이 클래스로 모으는 것은 기존 함수 변경이라 구현 때 확인한다.

| 순서 | 처리 | 결과 / 오류 |
| --- | --- | --- |
| 1 | 정리 (M1): 앞뒤 공백 제거 / 연속 공백 1칸 / 끝 `\s*#\S*\s*$` 제거 / 대문자 | Parameter 이름 (ITEM_NAME) |
| 2 | 나누기: 기존 정규식 `^(?<para>[^=@]+)(?:=(?<cond>[^@]+))?@(?<c1>[^:]+)(?::(?<c2>.+))?$` | PARA_NAME / COND_VALUE / Chain 1 / Chain 2. `@` 가 없으면 **오류**(등록 불가) |
| 3 | Measurement type = PARA_NAME 의 첫 `_` 앞. 화면의 "조건" = 나머지 (+ `=COND_VALUE`) | `RESISTANCE` + `iforce=8.0mA` / `LEAKAGE` + `Serial` |
| 4 | Force leg = Chain 끝 `_FH` / `_FL` | 없으면 NULL + **경고** "Force leg 없음" (등록은 가능) |
| 5 | Chain type = Chain 이름이 아래 목록으로 시작하면 그 값(긴 것 먼저), 아니면 첫 `_` 앞 | 목록 (10-12 메일 U9 Measurement type): `PIC_EDM` / `PIC_SERP` / `PIC_COMB` / `TSVDC` / `C4DC` / `ASICDC` / `HBMDC` |

예 (화면은 원문 대소문자로 보인다)

| 헤더 원문 | 측정 종류 | 조건 | Chain 1 / Leg | Chain 2 / Leg | Chain type |
| --- | --- | --- | --- | --- | --- |
| `Resistance_iforce=8.0mA@TSVDC_TR_FH` | Resistance | iforce=8.0mA | `TSVDC_TR_FH` / FH | | TSVDC |
| `Leakage_Parallel@TSVDC_ASIC1_P_FL` | Leakage | Parallel | `TSVDC_ASIC1_P_FL` / FL | | TSVDC |
| `Leakage_Serial@TSVDC_ASIC1_N_FH:TSVDC_ASIC1_P_FL` (TSVET) | Leakage | Serial | `TSVDC_ASIC1_N_FH` / FH | `TSVDC_ASIC1_P_FL` / FL | TSVDC |
| `Leakage_Serial@TSVDC_ASIC1_N_FL:TSVDC_ASIC1_P_FH #421` (FT) | Leakage | Serial | `TSVDC_ASIC1_N_FL` / FL | `TSVDC_ASIC1_P_FH` / FH | TSVDC |
| `Resistance_iforce=8.0mA@TSVDC_HBM_C_P_FH #717` (FT) | Resistance | iforce=8.0mA | `TSVDC_HBM_C_P_FH` / FH | | TSVDC |
| `Resistance_iforce=1.0mA@PIC_EDM_R_FH` | Resistance | iforce=1.0mA | `PIC_EDM_R_FH` / FH | | PIC_EDM |
| `Leakage_Serial@PIC_SERP1_M2_FL:PIC_SERP1_M3_FL` | Leakage | Serial | `PIC_SERP1_M2_FL` / FL | `PIC_SERP1_M3_FL` / FL | PIC_SERP |
| `Connectivity_iforce=8.0mA@C4DC_148_73_FH` | Connectivity | iforce=8.0mA | `C4DC_148_73_FH` / FH | | C4DC |

- 같은 요청 안에서 정리 후 같은 Parameter 가 두 번 나오면(`#` 만 다른 헤더 등) **오류**.
- Chain 이 어느 대표 Chain 에 속하는지는 이름으로 정하지 않는다(6절, 사용자가 등록).

### 5.1 이름 변환 (DB / 소스에서 그대로 쓰는 이름)

요구: Parameter 이름과 Chain 관련 이름을 DB(컬럼 이름 / 바인딩 / LIKE) 와 소스(식별자 / 파일 이름 / XML)에서 문제없이 쓸 수 있게 바꾸되, 엔지니어가 원래 헤더를 알아볼 수 있어야 한다.

원래 헤더의 문제 문자: `=` `@` `:` `.` 공백 `#` (예 `Resistance_iforce=8.0mA@TSVDC_TR_FH`). 이 문자들만 바꾸고 나머지 글자 순서는 그대로 둔다.

| 대상 | 규칙 | 예 |
| --- | --- | --- |
| **Parameter 이름** (ITEM_NAME) | **`EtYmsRule.ToParamName` 그대로** (YMS 적재 설계 4.3, 같은 Interface 에 이미 있음): 공백 정리 / 끝 `#번호` 제거 / 대문자 -> `.` -> `P` -> `A~Z 0~9 _` 밖 문자는 `_`, 연속 `_` 하나로, 앞뒤 `_` 제거 -> 숫자로 시작하거나 예약 이름이면 앞에 `P_` -> 50자를 넘으면 측정 종류를 고객 약어로(LEAKAGE -> LKG, RESISTANCE -> RES …) -> 그래도 넘으면 41자 + `_` + 해시 8자 | `Resistance_iforce=8.0mA@TSVDC_TR_FH` -> `RESISTANCE_IFORCE_8P0MA_TSVDC_TR_FH` / `Leakage_Serial@TSVDC_ASIC1_N_FL:TSVDC_ASIC1_P_FH #421` -> `LEAKAGE_SERIAL_TSVDC_ASIC1_N_FL_TSVDC_ASIC1_P_FH` / `Leakage_Serial@TSVDC_ASIC_HBM_N_FH:TSVDC_ASIC_HBM_P_FL`(54자) -> `LKG_SERIAL_TSVDC_ASIC_HBM_N_FH_TSVDC_ASIC_HBM_P_FL` |
| **Chain 이름** (ET_ITEM.CHAIN_NAME / CHAIN_NAME2 / ET_REP_CHAIN.CHAIN_NAME) | 헤더를 `@` / `:` 로 나눈 뒤 각 Chain 에 같은 문자 규칙(대문자 / `.` -> `P` / 그 밖 `_` / 연속 `_` 하나 / 앞뒤 `_` 제거). 영문자로 시작해야 하고 64자 이하 | `TSVDC_TOP1_N_FL` (그대로) |
| **대표 Chain 이름** (REP_CHAIN_NAME) | 사용자 입력에 Chain 이름과 같은 규칙. 입력 창 아래에 바뀐 이름을 바로 보여 주고 그 이름으로 저장 | `tsvdc top1` -> `TSVDC_TOP1` |
| **GDS Chain 이름 비교** (7.1) | GDS 이름에 Chain 이름과 같은 규칙을 적용한 뒤 비교 (GDS 테이블 값은 바꾸지 않음) | `TSVDC_TR_FH` |
| PARA_NAME / MEAS_TYPE | Chain 이름과 같은 규칙 | `RESISTANCE_IFORCE` / `RESISTANCE` |
| COND_VALUE | 이름이 아니라 값이라 대문자만 (화면 표시용) | `8.0MA` |

- **같은 규칙을 한 곳만 쓴다.** Parameter 이름은 YMS 컬럼 이름 규칙과 같게 해서 같은 헤더가 E-Test 테이블과 YMS 테이블에서 같은 이름이 되게 한다(YMS 설계의 `YMS_PARAM_NAME` 은 ITEM_NAME 과 같은 값이 된다). Chain 이름 규칙은 `EtParameterNameRule.ToSafeName`.
- **원문은 버리지 않는다.** `HEADER_TEXT` 에 원문을 두고 화면은 원문과 변환 이름을 나란히 보여 준다.
- **충돌**: 같은 Device + 공정 안에서 서로 다른 헤더가 같은 변환 이름이 되면 등록을 거부한다. 로컬 확인: 샘플 TSVET 헤더 2,755개 충돌 0 / 최대 50자 / 약어 1개 / 해시 0, 고객 Spec 표 119행 충돌 0 / 약어 3행 / 해시 0.
- **DB 가 막는다**: 등록 행의 ITEM_NAME / Chain / 대표 이름은 CHECK `^[A-Z][A-Z0-9_]*$` 로 저장된다(4.2 ~ 4.4).
- Spec(`TQP_GDS_ET_LIMIT.ITEM_NAME`)은 지금 원문 정리 이름이다. Limit 화면 정리(구현 4단계) 때 같은 변환 이름으로 바꾼다(로컬 0행).

## 6. 대표 Chain 등록 (사용자 이름 입력)

### 6.1 GDS 대표 Device와 Chain 목록

E-Test Device(OMM) -> 기존 `RO.GdsMapView.GetMapDeviceBySub(factory, E-Test Device)` -> U9. 연결이 없으면 화면이 "GDS Map 조회 화면에서 Sub Device 를 등록하세요" 를 보이고 GDS Device 콤보에서 직접 고르게 한다.

Chain 목록 = U9 의 Sub Device 들(OMM / OMM_Hybrid)에 등록된 Parameter(USE_FLAG Y)의 Chain 1 / 2 (공정은 합침, 물리 Chain 은 공정과 무관).

### 6.2 묶기 / 수정

| 동작 | 내용 |
| --- | --- |
| **[선택 Chain 묶기]** | Chain 목록에서 1개 이상 고름(Ctrl / Shift) -> **대표 이름 입력 창** -> 저장. 입력 칸은 빈 칸으로 시작하고, 아래 목록(고른 Chain 이름 / U9 GDS Setup Chain 이름 / 이미 있는 대표 이름)에서 골라 넣거나 직접 친다. 대문자로 저장 |
| 이미 있는 대표 이름을 입력 | "대표 X 에 Chain n 개를 더합니다" 확인 -> 그 대표에 들어간다 (나중에 Chain 을 추가하는 방법) |
| 다른 대표에 있던 Chain 을 고름 | "Chain A 는 대표 Y 에 있습니다. 옮길까요?" 확인 |
| [대표에서 빼기] | 고른 Chain 을 대표에서 뺀다. 대표의 Chain 이 0 이 되면 그 대표와 GDS 연결도 지운다 |
| [대표 이름 바꾸기] | 두 테이블(`ET_REP_CHAIN` / `ET_CHAIN_LINK`)을 같이 바꾼다. GDS 연결은 유지. 다른 대표와 같은 이름이면 거부 |
| [대표 삭제] | Chain + GDS 연결 삭제 |

- 입력 검사: 5.1 규칙으로 바꾼 이름을 입력 칸 아래에 보여 주고 그 이름으로 저장한다. 바꾼 결과가 비거나 영문자로 시작하지 않거나 64자를 넘으면 거부.
- 저장 직후 그 대표만 7.1 의 이름 비교를 해서 GDS 연결 상태를 보여 준다.
- 대표에 들지 않은 Chain 은 "대표 미등록" 으로 보인다. 그 Chain 의 Fail 은 GDS Chain 으로 찾아가지 못한다.

## 7. GDS Chain 연결

### 7.1 이름 일치 연결 (`LinkEtRepChainsByName`)

묶기를 자동으로 하지 않고, 사용자가 만든 대표 Chain 과 GDS Chain 을 **이름이 똑같을 때만** 잇는다.

1. 대상 = U9 의 대표 Chain 중 `MANUAL` 연결이 없는 것 (저장한 대표 1개 또는 [GDS 이름 연결] 로 전체).
2. GDS Chain = U9 Setup Chain (`TQP_GDS_CHAIN` ACTIVE + `TQP_GDS_CHAIN_VER` IS_CURRENT = 1 / READY). 이름 정리 = 앞뒤 공백 제거 + 대문자.
3. GDS 이름이 **대표 이름과 같거나 구성 Chain 이름 중 하나와 같은** GDS Chain 을 찾는다. 예: 대표 `TSVDC_TOP1` = Chain `TSVDC_TOP1_P_FH` + `TSVDC_TOP1_N_FL` -> GDS 이름이 둘 중 무엇이든 찾음.
4. 후보가 **1개**이고 그 GDS Chain 이 다른 대표에 연결돼 있지 않으면 `NAME` 연결 (있으면 이름 갱신).
5. **2개 이상**이면 연결하지 않는다 -> "후보 여러 개" (사용자가 고름).
6. **0개**면 -> "GDS 없음".
7. 이전 `NAME` 연결 중 지금 이름이 맞지 않는 것(GDS 이름이 바뀜, Chain 이 빠짐)은 지우고 3 ~ 6 으로 다시 정한다.

### 7.2 사용자 연결 / 해제

- [GDS 선택 연결]: 대표 1개 + GDS Chain 1개 -> `MANUAL`. 이름이 안 맞는 GDS Chain(예 로컬 `TSVDC_TOP1_P`, 사람이 고친 이름)에 쓴다. 그 GDS Chain 이 다른 대표에 연결돼 있으면 "옮길까요?" 를 묻고, 서비스는 이전 연결을 지우고 새로 넣는다(1:1 유지).
- [연결 해제]: 연결 행 삭제. `NAME` 연결이었던 대표는 다음 이름 비교 때 다시 연결된다(해제 상태를 붙잡아 두는 기능은 POC 에서 넣지 않음).

### 7.3 상태

| 대상 | 상태 | 조건 | 화면 |
| --- | --- | --- | --- |
| Chain | 대표 미등록 | `ET_REP_CHAIN` 에 없음 | 주황색 |
| 대표 | 연결 (이름) | `NAME`, 지금 GDS 이름이 대표 이름 또는 구성 Chain 과 같음 | 기본 색 |
| 대표 | 연결 (수동) | `MANUAL` | 파란 글자 |
| 대표 | GDS 이름 바뀜 | 연결의 GDS_CHAIN_NAME ≠ 지금 GDS Chain 이름 | 노란색. [GDS 이름 연결] 을 다시 하면 `NAME` 은 다시 계산 |
| 대표 | 후보 여러 개 | 7.1-3 후보 2개 이상, 연결 없음 | 주황색 |
| 대표 | GDS 없음 | 후보 0, 연결 없음 | 회색 (GDS Map 조회에서 Chain Setup 필요) |
| Chain | Force leg 없음 | Chain 끝이 FH / FL 아님 | 경고 아이콘 |

### 7.4 GDS 쪽이 바뀌었을 때

| GDS 변경 | 대표 Chain / 연결 |
| --- | --- |
| Chain 이름 수정 (다른 Chain 이름으로 바꿈 등) | "GDS 이름 바뀜" 표시 -> [GDS 이름 연결]. 새 이름이 구성 Chain 이면 다시 `NAME` |
| Chain 다시 저장 (덮어쓰기, CHAIN_SEQ 유지) | 그대로 |
| Chain 삭제 / Map 전체 삭제 / Map 재등록 | 연결만 FK CASCADE 로 삭제, 대표 Chain 은 남음 -> [GDS 이름 연결] (수동 연결은 다시 지정) |

## 8. 화면 설계 — `frmEtParaSetup` (신규)

위치: `02_DMS/DACrux.SEMDMS.ENGUI/ETest/frmEtParaSetup.cs` / `.Designer.cs`. 컨트롤 배치는 Designer, 동작은 실행 파일 (공통 개발 지침).

메뉴: `MNU_DMS_ET_PARA` / "E-Test Para Setup" / "E-Test Para 설정" / F03125 / POC 메뉴에서 **E-Test Limit 설정 바로 위** (작업 순서 Para -> Limit -> 업로드). 메뉴 행은 파일 맨 뒤에 추가하고 `MNU_ORDER` 로 순서를 정한다(CLAUDE-010 기준, 기존 행 순서 유지).

```text
┌ Factory [SCK ]  Device [OMM   ](읽기 전용)  공정 [TSVET](읽기 전용)  [조회] │ [결과 파일 열기...] [Spec 표로 고르기] │ [등록] [저장] [삭제] ┐
├ [파일 헤더] [등록 Parameter] [대표 Chain] ───────────────────────────────────────────────────────────────────────────┤
│ 파일: CAI_OMM_ATV_A0_TSVET_20260609161940_RH-HS2615_RH-HS2615-02.xlsx   헤더 2,755 / 신규 n / 등록됨 n / 등록 불가 0 / 목록에서 뺌 n │
│                                              측정 [전체 ▼]  Chain 종류 [전체 ▼]  찾기 [            ]  □ 신규만        │
│열│ ☑│상태  │Parameter                                        │측정        │조건        │Chain 1          │Chain 2          │단위│대표 Chain   │
│ 1│ ☐│등록됨│Resistance_iforce=8.0mA@TSVDC_TR_FH             │Resistance  │iforce=8.0mA│TSVDC_TR_FH     │                │Ohm │TSVDC_TR     │
│ 2│ ☑│신규  │Leakage_Serial@TSVDC_ASIC1_N_FH:TSVDC_ASIC1_P_FL│Leakage     │Serial      │TSVDC_ASIC1_N_FH│TSVDC_ASIC1_P_FL│uA  │미등록 / 미등록│
│ 3│ ☐│신규  │Connectivity_iforce=8.0mA@C4DC_148_73_FH         │Connectivity│iforce=8.0mA│C4DC_148_73_FH  │                │V   │미등록       │
├─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────┤
│ 상태: 체크 1 / [등록] 하면 Device OMM · 공정 TSVET 에 등록. 행 드래그 -> 우클릭 [선택 행 목록에서 삭제 (n)] / Del                                                            │
└─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────┘
```

### 8.1 위 조건 줄

- Factory = 입력 칸, 기본값 로그인 Factory (`DACrux.Base.GlobalVariable.Factory`, Limit 설정 화면과 같은 방식).
- Device / 공정: **읽기 전용**. 결과 파일을 열면 파일명의 Device / Test Type 이 들어간다(고르거나 칠 수 없음, 10-14 변경). 파일을 열지 않으면 비어 있고 [조회] / [등록] 은 "먼저 [결과 파일 열기]" 를 안내한다.
- [조회]: 열어 둔 파일의 Device / 공정으로 [등록 Parameter] / [대표 Chain] 탭과 헤더 상태를 다시 읽는다. 저장 안 된 사용 / 비고 변경이 있으면 [조회] / [결과 파일 열기] 전에 버릴지 묻는다.

### 8.2 [파일 헤더] 탭 — 등록 후보

| 항목 | 내용 |
| --- | --- |
| [결과 파일 열기] | xlsx 선택 -> 기존 `EtResultFileParser.Parse` (파일명 8칸 / 헤더 / 단위 / Test No) -> `EtParameterNameRule` 로 분해 -> `GetEtParameterList`(등록 여부 / 단위) + `GetEtChainList`(Chain 의 대표 Chain)와 맞춰 상태를 정한다 |
| 표 | **행 머리글 = 열**(파일의 측정 열 순서 1 ~ n, 왼쪽 위 칸 "열") / ☑ / 상태 / Parameter(원문) / 저장 이름 / 측정 / 조건 / Chain 1 / Leg / Chain 2 / Leg / 단위 / Test No / 대표 Chain / 메시지. 읽기 전용, 체크만 가능. 행 단위 선택(FullRowSelect, 여러 행) |
| 목록에서 빼기 | 행을 드래그(또는 Ctrl / Shift)로 고르고 우클릭 **[선택 행 목록에서 삭제 (n)]** 또는 **Del**. 고른 행 밖에서 우클릭하면 그 행만 고른다. 화면에서만 숨기고 체크를 푼다(DB 와 무관). 표를 다시 만들지 않으므로 다른 행 체크는 유지. 같은 파일을 보는 동안([조회] / [등록] 뒤에도) 유지, 새 파일을 열면 비운다. 우클릭 **[삭제한 행 모두 되살리기 (n)]**. 파일 정보 줄에 "목록에서 뺌 n", 탭 제목은 남은 행 수 |
| 상태 | **신규** / **등록됨**(체크 불가, 회색) / **단위 다름**(등록된 Parameter 와 파일 단위가 다름, 노란색, 등록 불가) / **오류**(분해 실패, 빨간색, 체크 불가) / **중복**(정리 후 같은 이름, 빨간색) |
| 거르기 | 측정 종류 / Chain 종류 / 글자 찾기 / 신규만. 고른 행 전체 체크 / 해제는 머리글 체크 |
| [Spec 표로 고르기] | 고객 Spec 표 붙여넣기 창 (기존 `EtSpecTable` 해석 재사용) -> 화면 공정과 같은 Test Type 행의 Parameter 와 일치하는 헤더를 체크. 표에 있는데 파일에 없는 Parameter 는 목록으로 알림, 목록에서 뺀 행은 건너뛰고 수를 알림 (등록은 [등록]) |
| [등록] | 체크한 신규 행 -> `RegisterEtParameters` (출처 FILE + 파일 이름). 끝나면 대표 미등록 Chain 수를 상태 줄에 보이고 [대표 Chain] 탭에서 묶도록 안내 |

### 8.3 [등록 Parameter] 탭

| 항목 | 내용 |
| --- | --- |
| 표 | Parameter(원문) / 측정 / 조건 / Chain 1 · 2 / 대표 Chain 1 · 2 / 단위 / **Spec**(Lower ~ Upper, Enable, 없으면 빈 칸) / **사용** / 값 있는 파일 수 / GDS Chain / 출처(파일 · Spec 표) / 등록 파일 / 비고 / 수정 |
| 입력 | 사용 / 비고 만. 바뀐 행 노란색 -> [저장] (`SaveEtParameters`) |
| [삭제] | 고른 Parameter. **값이 있는 Parameter 는 지우지 않고** "사용 안 함으로 바꾸세요" 안내. Spec 이 있으면 "Spec 도 같이 지웁니다" 를 묻는다 |
| 더블클릭 | Limit 설정 화면을 같은 Device / 공정 / Parameter 로 연다 |

### 8.4 [대표 Chain] 탭

```text
GDS Device [U9 ▼] (OMM 의 대표 Device)
[선택 Chain 묶기] [대표에서 빼기] [대표 이름 바꾸기] [대표 삭제] │ [GDS 이름 연결] [GDS 선택 연결] [연결 해제]
Chain 52 / 대표 미등록 n / 대표 n / GDS 연결 n (이름 n · 수동 n) / GDS 없음 n / 후보 여러 개 n
┌ E-Test Chain ──────────────────────────────┐┌ 대표 Chain ──────────────────────────────────────────────────┐┌ GDS Setup Chain (U9) ───────────┐
│Chain 이름        │Leg│공정    │Para│대표 Chain ││대표 Chain │Chain                              │GDS Chain       │방식│상태  ││이름            │연결 대표   │
│TSVDC_TOP1_P_FH  │FH │COWET·FT│ 2  │TSVDC_TOP1 ││TSVDC_TOP1 │TSVDC_TOP1_P_FH, TSVDC_TOP1_N_FL  │TSVDC_TOP1_P_FH │이름│연결  ││TSVDC_TOP1_P_FH │TSVDC_TOP1  │
│TSVDC_TOP1_N_FL  │FL │FT      │ 1  │TSVDC_TOP1 ││TSVDC_BL   │TSVDC_BL_FH                       │                │    │GDS 없음││PIC_COMB12_M3   │            │
│TSVDC_BL_FH      │FH │전 공정 │ 3  │TSVDC_BL   ││           │                                  │                │    │      ││                │            │
└───────────────────────────────────────────┘└──────────────────────────────────────────────────────────────┘└─────────────────────────────────┘
┌ 선택 대표 Chain 의 Parameter / Spec : 공정 / Parameter / Chain / 위치(1 · 2) / Lower / Upper / Enable / 단위 ─────────────────────────────────┐
```

- Chain 목록은 "대표 미등록만" 거르기와 글자 찾기를 둔다.
- 가운데 대표 Chain 을 고르면 왼쪽 구성 Chain 과 오른쪽 연결 GDS Chain 이 선택되고, 아래에 그 대표의 **모든 공정 Parameter / Spec** 이 나온다(`GetEtRepChainSpecInfo`). Limit 화면 [Setup Chain 으로 찾기] 의 역할을 여기로 옮긴다(P8).
- 오른쪽 GDS Chain 을 고르면 연결된 대표가 선택된다. 연결 안 된 GDS Chain 은 회색.
- 대표 / 연결 변경은 바로 저장한다(따로 [저장] 없음, 지금 [Chain 묶기] 탭과 같은 방식).

## 9. 서비스 (`iGdsEtResult` 에 추가, 새 wellknown 없음)

| 메서드 | 쿼리 (XML) | 내용 |
| --- | --- | --- |
| `GetEtParaProgramList(factory)` | `TQP_GDS_ET_ITEM.SELECT_ET_PARA_PROGRAM_LIST` | 등록 Parameter 가 있는 Device + 공정 (화면 콤보) |
| `RegisterEtParameters(aKey{factory, device, 공정, 출처 FILE · SPEC, 파일 이름}, aRows{원문, 단위, TestNo, 열 순서}, user)` | `TQP_GDS_ET_ITEM.CREATE_ET_PARA` | 서버가 다시 정리 / 분해해 INSERT. 이미 있는 Parameter 는 건너뜀. 오류 / 중복 / 단위 다름이 하나라도 있으면 전체 거부. 반환: 등록 / 건너뜀 수 |
| `GetEtParameterList(factory, device, 공정)` | `SELECT_ET_PARA_LIST` | 등록 Parameter + Spec 요약 + 값 있는 파일 수 + 대표 Chain / GDS 연결 |
| `SaveEtParameters(aKey{factory, device, 공정}, aRows{Parameter, 사용, 비고}, user)` | `UPDATE_ET_PARA` | |
| `DeleteEtParameters(aKey{factory, device, 공정}, aNames, user)` | `SELECT_ET_PARA_VALUE_COUNT` / `DELETE_ET_PARA` | 값이 있으면 거부. Spec 은 FK CASCADE |
| `GetEtChainList(factory, gdsDevice)` | `TQP_GDS_ET_REP_CHAIN.SELECT_ET_CHAIN_LIST` | Chain (Sub Device 등록 Parameter 합침) + Leg / 종류 / 공정 / Parameter · Spec 수 + 대표 Chain |
| `GetEtRepChainList(factory, gdsDevice)` | `SELECT_ET_REP_CHAIN_MEMBER_LIST` / `TQP_GDS_ET_CHAIN_LINK.SELECT_ET_CHAIN_LINK_LIST` / `SELECT_ET_GDS_SETUP_CHAIN` | 대표 Chain + 구성 Chain + GDS 연결 / 방식 / 상태. 상태(7.3)는 서버가 이름 규칙(`ToSafeName`)으로 계산 |
| `GetEtGdsChainLinkList(factory, gdsDevice)` | 위와 같은 3개 조회 | U9 Setup Chain + 비교 이름 + 연결된 대표 |
| `SaveEtRepChain(aKey{factory, gdsDevice, 대표 이름}, aChains, user)` | `MERGE_ET_REP_CHAIN` | Chain 을 대표에 넣음(다른 대표에서 옮김). Chain 이 0 이 된 이전 대표의 연결 삭제. 저장한 대표만 7.1 이름 비교 |
| `RenameEtRepChain(aKey{factory, gdsDevice, 이전 이름, 새 이름}, user)` | `UPDATE_ET_REP_CHAIN_NAME` / `UPDATE_ET_CHAIN_LINK_NAME` | 새 이름이 다른 대표와 같으면 거부 |
| `RemoveEtRepChainMembers(aKey{factory, gdsDevice}, aChains, user)` | `DELETE_ET_REP_CHAIN_MEMBER` | |
| `DeleteEtRepChain(aKey{factory, gdsDevice, 대표 이름}, user)` | `DELETE_ET_REP_CHAIN` / `DELETE_ET_CHAIN_LINK` | |
| `LinkEtRepChainsByName(factory, gdsDevice, user)` | `MERGE_ET_CHAIN_LINK` / `DELETE_ET_CHAIN_LINK` | 7.1절 (전체). 반환: 연결 / 갱신 / 지움 / GDS 없음 / 후보 여러 개 수 |
| `SaveEtChainLink(aKey{factory, gdsDevice}, aRows{대표 이름, CHAIN_SEQ}, user)` | `DELETE_ET_CHAIN_LINK_BY_SEQ` / `MERGE_ET_CHAIN_LINK` | `MANUAL`. 같은 GDS Chain 의 다른 대표 연결은 지우고 옮김 |
| `DeleteEtChainLink(aKey{factory, gdsDevice, 대표 이름}, user)` | `DELETE_ET_CHAIN_LINK` | |
| `GetEtRepChainSpecInfo(factory, gdsDevice, 대표 이름)` | `SELECT_ET_PARA_BY_REP_CHAIN` | 구성 Chain 이 Chain 1 / 2 로 들어간 모든 공정 Parameter + Spec |
| (변경, 10-14 구현) `GetEtChainSpecInfo(factory, device, setupChain)` | `SELECT_ET_PARA_BY_REP_CHAIN` | **인계 API 이름 / 인자 유지**, 안을 바꾼다: device(E-Test / GDS Device) -> GDS 대표 Device -> 비교 이름이 같은 GDS Setup Chain -> 연결 대표 -> 구성 Chain -> 위 조회. 열 = 위 열 + `REP_CHAIN_NAME` / `GDS_CHAIN_NAME`, 연결 없으면 0행. 이름 규칙 찾기(EXACT / SUFFIX / GROUP)는 없앴다 |
| (변경, 10-14) `GetEtLimitList(factory, device, 공정)` | `TQP_GDS_ET_LIMIT.SELECT_ET_LIMIT_LIST` | 등록 Parameter 기준(Spec 없는 Parameter 포함) + Spec + 대표 / GDS Chain + READY 파일 값 통계. 값은 등록 행 + 예전 업로드 행(TEST_PROGRAM 없음, `UPPER(ITEM_NAME) = UPPER(HEADER_TEXT)`) |
| (변경, 10-14) `SaveEtLimit` / `DeleteEtLimit` / `GetEtLimit` / `GetEtTestProgramList` | `MERGE_ET_LIMIT` / `DELETE_ET_LIMIT` / `SELECT_ET_LIMIT` / `SELECT_ET_TEST_PROGRAM_LIST` | Spec 키 = 저장 이름(헤더 원문도 받아 바꿈), 등록 안 된 Parameter 거부, 공정은 `EtParameterNameRule.ToTestProgram`. 공정 목록에 등록 Parameter 공정 추가 |
| (삭제, 10-14) Chain 묶기 서비스 5개 | `TQP_GDS_ET_CHAIN_GROUP.xml` / `SELECT_ET_LIMIT_BY_CHAIN` | 4.6절 |

- 신규 XML / DSL: `TQP_GDS_ET_REP_CHAIN.xml` / `.cs`, `TQP_GDS_ET_CHAIN_LINK.xml` / `.cs`. `TQP_GDS_ET_ITEM.xml` 에 쿼리 추가.
- 분해 규칙 클래스: `02_DMS/DACrux.SEMDMS.Interface/ETest/EtParameterNameRule.cs` (Interface csproj Compile 1행). 파일 헤더 분석은 화면이 이 클래스로 하므로 분석 전용 서비스는 두지 않는다.

## 10. 다른 화면 / 설계에 미치는 영향

| 대상 | 바뀌는 점 |
| --- | --- |
| Limit(Spec) 설정 `frmEtLimitSetup` | **완료 (10-14, 4단계)**. Spec 목록 = **등록 Parameter 기준**(Parameter 마스터 LEFT JOIN Spec). [Spec 표 붙여넣기] 는 등록 안 된 Parameter 를 **출처 SPEC 으로 자동 등록**한 뒤 Spec 저장 (FT 는 결과 파일이 없어 표로만 등록 가능, P6). [Spec] 탭 열 `묶음` / `Setup Chain` -> `대표 Chain` / `GDS Chain`. [Chain 묶기] / [Setup Chain 으로 찾기] 탭 삭제 (P8). 구현에서 더한 것: 열 "Parameter (헤더 원문)" + "저장 이름", 거르기 줄([Spec 있는 것만] / 찾기 = 헤더 원문 · 저장 이름 · 대표 · GDS), Spec 없던 행에 Lower / Upper 를 넣으면 Enable 자동(한 번), Spec 없는 행에서 Limit 없이 바뀌면 "Lower / Upper 를 입력해야 저장" 오류, [선택 행 Spec 삭제] 는 Spec 만 지우고 등록 Parameter 는 남김, 사용 안 함 Parameter 는 회색. Spec 표로 등록할 때 HEADER_TEXT 는 표의 원문 대소문자(`EtSpecRow.HeaderText`) |
| Upload 재설계 (9절, 구현 대기) | 저장 대상 = **등록 Parameter(사용 Y)** (지금 설계는 Spec 정의만, P7). S1 "Spec 없으면 업로드 중단" -> "등록 Parameter 없으면 중단". S4 `MergeEtItem` 은 등록하지 않고 찾기만(등록 안 된 헤더는 건너뜀 수로 표시). 판정은 그대로 Spec + Enable 인 값만, Spec 없는 등록 Parameter 는 값만 저장(JUDGE_FLAG NULL). "정의됐는데 파일에 없음" -> "등록됐는데 파일에 없음". Fail Chain 패널에 대표 Chain / GDS Chain 열 추가 |
| Spec 판정 쿼리 (`SELECT_ET_LIMIT_LIST` 등) | 측정 항목과 조인할 때 TEST_PROGRAM 을 같이 건다 (같은 헤더가 공정마다 따로 있으므로). `SELECT_ET_LIMIT_LIST` 는 10-14 등록 Parameter 기준으로 다시 작성(반영) |
| YMS 적재 설계 (`2026-10-14_ETest_Upload_YMS적재_설계.md`, CLAUDE-085) | 같은 `TQP_GDS_ET_ITEM` 에 `YMS_PARAM_NAME` + UQ 를 추가할 예정이다. P1 / P2 로 ITEM 이 공정마다 따로 생기면 **UQ 에 TEST_PROGRAM 을 넣어야 한다** (YMS 설계 14.2 에 이미 반영). YMS 이름은 Parameter 를 등록할 때(Para Setup) 만들고, "YMS 이름" 열은 [등록 Parameter] 탭에 둔다. DDL 은 Para Setup -> Upload 재설계 -> YMS 순서. PARAM_DESC 원문은 `HEADER_TEXT` 를 쓸 수 있다 |
| GDS × KLARF 연관 분석 / Wafer Map | E-Test Fail -> Parameter -> Chain -> 대표 Chain -> GDS Chain 영역. 다음 단계에서 연결 테이블을 쓴다 (이번 설계 범위 밖) |
| GDS 화면 / 서비스 | 소스 변경 없음. GDS Chain 삭제 시 연결 행만 FK CASCADE 로 지워진다 |

## 11. 결정 (권장 먼저)

번호는 v1 과 같게 두었다(다른 문서에서 P6 / P7 / P8 로 부름). **2026-10-14 "설계 넣고 개발 시작" 지시로 아래 권장안을 모두 확정했다.**

| 번호 | 항목 | 결정 | 다른 안 |
| --- | --- | --- | --- |
| P1 | Parameter 마스터를 어디에 둘지 | **`TQP_GDS_ET_ITEM` 재정의** (측정값이 이미 참조, 헤더 정보 한 곳) | 신규 `TQP_GDS_ET_PARA` (ITEM 은 업로드용으로 남김, 같은 정보 두 곳) |
| P2 | 등록 단위 | **Device + 공정 + 헤더 전체** (고객 Spec 표가 공정별, 10-13 본 프로젝트 키와 같은 방향) | Device + 헤더 (공정 공통) |
| P3 | Chain 묶는 방식 | **확정 (10-14): 대표 Chain 등록** | — |
| P4 | 대표 Chain / 연결의 Device | **GDS 대표 Device (U9)** — OMM / OMM_Hybrid 가 같이 씀 | E-Test Device (OMM / OMM_Hybrid 따로) |
| P5 | 1:1 | **양쪽 모두 1:1** (대표 PK + CHAIN_SEQ UNIQUE, 10-06 요구) | 대표 하나에 GDS Chain 여러 개 허용 |
| P6 | Spec 과 Parameter | **Spec 은 등록 Parameter 에만 (FK)** + Spec 표 붙여넣기는 없는 Parameter 를 자동 등록 | 지금처럼 따로 (Spec 만 있는 Parameter 허용) |
| P7 | 업로드 저장 대상 | **등록 Parameter (사용 Y)** — Spec 이 없어도 값은 저장, 판정은 Spec 있는 것만 | Spec 정의 Parameter 만 (Upload 재설계 지금 안) |
| P8 | Chain 묶기 / Setup Chain 으로 찾기 | **대표 Chain 으로 대체**: `TQP_GDS_ET_CHAIN_GROUP` 삭제, Limit 화면 탭 2개 삭제, 찾기는 Para Setup [대표 Chain] 탭으로 | 그대로 두고 대표 Chain 을 추가 |
| P9 | 대표 이름 | **확정 (10-14): 사용자가 입력** (빈 칸 시작, Chain / GDS / 기존 대표 이름 목록에서 골라 넣기 가능) | — |
| P10 | 자동 묶기 | **확정 (10-14): 본 프로젝트** (12절) | — |

## 12. 본 프로젝트로 미룬 것 (자동 묶기)

v2 초안에서 검토한 자동 묶기 방법과 로컬 시험 결과를 본 프로젝트 참고로 남긴다. POC 에는 넣지 않는다.

| 방법 | 내용 | 로컬 시험 결과 (조회만) |
| --- | --- | --- |
| ① GDS Chain 영역 안 Chain Label | U9 Setup Chain 의 Member 도형(`TQP_GDS_CHAIN_MEMBER` -> `TQP_GDS_PLACED_EL` 경계 사각형) 안에 점이 있는 TEXT 중 Chain 이름인 것 = 그 Chain 의 Chain. 대표 이름 = GDS Chain 이름 | U9 15개 Chain 1.5초. TSVDC 13개 Chain 은 각각 `X_FH` + `X_FL`, PIC_COMB 2개는 `_FH` 1개. U7 2개도 같은 형태. Spec 표 Chain 52 중 22개 -> 대표 13 (= GDS Chain 13) |
| ② 이름 규칙 | Force leg 를 뗀 이름이 같은 `X_FH` + `X_FL` 을 한 대표로 (Limit 15절 FH / FL 자동 묶기와 같은 규칙) | ① 에서 남은 30개 -> 대표 17 (짝 13 / Chain 1개 4) |
| 그 밖 | 묶은 방법 컬럼(`MEMBER_SOURCE`) / 경계 사각형 근사의 오탐(큰 도형) 처리 / 고객 Naming 표가 나오면 규칙 표 관리 | |

참고: 로컬 Setup Chain 17개(U9 15 / U7 2)의 영역 안 Label 은 모두 `X_FH` + `X_FL`(같은 Target region)이었다. `P_FH` + `N_FL` 짝은 로컬 데이터에서는 보이지 않았다. POC 는 사용자가 묶으므로 설계에는 영향이 없다.

## 13. 구현 단계 (결정 후)

| 단계 | 내용 |
| --- | --- |
| 1 | **완료 (2026-10-14 CLAUDE-090)**: 1단계 DDL(4.7, 추가만, 로컬 적용) + `EtParameterNameRule`(Interface) + Parameter 등록 서비스 + 신규 Probe `GdsMapControl/Probes/EtParaSetupProbe.ps1` |
| 2 | **완료**: 대표 Chain / GDS 연결 서비스 + 같은 Probe (`ET_PARA_SETUP_PROBE=PASS` 32/32) |
| 3 | **완료**: 화면 `frmEtParaSetup` + 메뉴(소스 `MenuFile.mnu`, `MNU_DMS_ET_PARA` F03125, POC 순서 5) + Smoke `Verification/Verify-EtParaSetup.ps1` / `EtParaSetupFormSmoke.cs` (`ET_PARA_FORM_SMOKE=PASS` 18/18). 배포 / DMS 화면 확인은 대기 |
| 3-1 | **완료 (10-14)**: 화면 레이아웃 변경(Device / 공정 읽기 전용, 헤더 표 우클릭 / Del 로 목록에서 빼기 + 되살리기, "열" -> 행 머리글) + 용어 "단자" -> "Chain"(컬럼 / 서비스 / 쿼리 이름 포함, 로컬 DB 컬럼 RENAME). Probe 32/32 / Smoke 24/24 |
| 4 | **완료 (10-14)**: Limit 화면 정리(10절) + 2단계 DDL 일부(4.7 2a: Spec FK / Chain 묶음 DROP, 로컬 적용) + 인계 API `GetEtChainSpecInfo` 새 구현 + Para 쿼리의 예전 키 Spec 조인 제거. Smoke `Verification/Verify-EtLimitSetup.ps1`(신규, `Verify-EtResultUpload.ps1` 3절에서 옮김) `ET_LIMIT_FORM_SMOKE=PASS` 41/41, Probe 34/34 |
| 5 | **완료 (10-14)**: Upload 재설계 구현(저장 대상 = 등록 Parameter, P7) + 2b DDL. 결과는 [Upload 재설계](2026-10-13_ETest_Upload_재설계.md) 10.3절. 같은 날 CLAUDE-094: Parameter 삭제 / 사용 저장 서버 묶음 처리(2,755개 삭제 서버 0.1초) + 화면 아래 진행 표시줄 |
| 6 | 연관 분석 / Wafer Map 에서 연결 테이블 사용 (별도 설계) |

## 14. 검증 기준 (기대값)

| 검사 | 기대값 |
| --- | --- |
| 분해 (샘플 TSVET 2,755) | 오류 0 / Chain 1,306 / Force leg 없음 0 |
| 분해 (Spec 표 119) | 오류 0 / Chain 52 / `#번호` 제거 / FT `...N_FL:...P_FH #421` 은 Chain `TSVDC_ASIC1_N_FL` + `TSVDC_ASIC1_P_FH` |
| 등록 | TSVET 헤더 37개(Spec 표로 고르기) 등록 -> 다시 열면 37 등록됨 / 같은 파일 다시 등록 -> 건너뜀 37 / 대문자 저장 · 원문 보관 / `@` 없는 헤더 거부 |
| 묶기 | Chain 2개 + 이름 `tsvdc top1` 입력 -> 대표 `TSVDC_TOP1` / 바꾼 결과가 빈 이름 · 숫자로 시작 거부 / 이미 있는 대표 이름 입력 -> 그 대표에 더함 / 다른 대표의 Chain -> 옮김 질문 / 대표의 Chain 을 모두 빼면 연결도 삭제 / 이름 바꾸기 후 연결 유지 |
| **사용자 예 (양 끝 이름이 다른 Chain)** | 대표 `TSVDC_TOP1` = Chain `TSVDC_TOP1_P_FH` + `TSVDC_TOP1_N_FL`. 시험 GDS Chain 이름 `TSVDC_TOP1_N_FL` -> 이름 연결됨 / GDS 이름을 `TSVDC_TOP1_P_FH` 로 바꾸고 [GDS 이름 연결] -> 다시 연결됨 |
| 로컬 U9 이름 연결 | Chain 이름과 똑같은 GDS Chain 10개는 그 Chain 이 든 대표에 바로 연결 / `TSVDC_TOP1_P` 같은 고친 이름은 대표 이름을 같게 하거나 [GDS 선택 연결] |
| 1:1 | 이미 연결된 GDS Chain 을 다른 대표에 수동 연결 -> 이전 연결 지워짐 / 같은 CHAIN_SEQ 를 두 대표에 동시 요청 -> 거부 / 한 대표의 Chain 두 개가 서로 다른 GDS Chain 이름과 같으면 "후보 여러 개" |
| CASCADE | 시험 GDS Chain 삭제 -> 연결만 삭제, 대표 Chain 남음 |
| 삭제 | 값 있는 Parameter 삭제 거부 / Spec 있는 Parameter 삭제 -> Spec 같이 삭제 |
| 화면 레이아웃 | 파일 없이 [조회] -> 파일 열기 안내 / Device · 공정 칸 읽기 전용 / 행 머리글 1 ~ 2,755 / 고른 6행 위 우클릭 -> 선택 유지, 메뉴 "(6)" -> 2,749행 / 뺀 행은 Spec 표로 고르기에서 건너뜀(36 + 건너뜀 1) / 고른 행 밖 우클릭 -> 그 행만 / Del -> 7행 / 다른 행 체크 유지 / 되살리기 -> 2,755 |
| Setup Chain 찾기 (인계 API) | `GetEtChainSpecInfo(SCK, OMM, <연결된 GDS Chain 이름>)` -> 그 대표의 Chain 이 들어간 TSVET / COWET / FT Parameter + Spec |
| Limit 화면 (4단계, Smoke 확인) | 등록 Parameter 없으면 0행 + 안내 / 고객 표 붙여넣기 -> 119개 Spec 표 출처로 등록(원문 대소문자) + Spec 119 / TSVET 37행 모두 데이터 있음(예전 업로드 값) / Fail 6 / 대표 · GDS 열 / FT 42 데이터 없음 / Spec 없는 Parameter 도 목록(38행) / Spec 있는 것만 37 / 찾기 / Enable 자동 / [선택 행 Spec 삭제] 는 행 유지 / 서비스: 헤더 원문 키 변환 · 등록 안 된 Parameter 거부 · FK ORA-02291 |

시험 데이터는 Factory `PROBE` 로 만들고 끝나면 지운다(사용자 데이터 보호, CLAUDE-031 교훈).

## 15. 확인 필요 (고객)

| 번호 | 내용 |
| --- | --- |
| C2 | C4DC / ASICDC / HBMDC / PIC Chain 의 GDS Label 과 Layer (10-08 회신: Layer 추가 필요). 지금 C4DC Chain 은 GDS Chain 이 없다 |
| C3 | ASICDC / HBMDC 는 Measurement type 목록에는 있지만 샘플 TSVET 헤더에는 없다. 다른 공정 파일에서 나오는지 |

(C1 `P_FH` / `N_FL` 짝 확인은 사용자가 묶는 방식으로 정해져 닫음. 로컬 데이터 참고는 12절.)

## 16. 확인 수준

- **소스 확인**: `EtResultFileParser.NormalizeHeader` / `HeaderRegex`, `iGdsEtResult` 메서드 목록, `iGdsMapView.GetMapDeviceBySub`, `frmEtLimitSetup` Factory 칸, Interface 공용 규칙 클래스 선례(`Coordinate/DieCoordinateMapper.cs`), 메뉴 `MNU_DMS_ET_LIMIT`(F03121, 최대 기능 코드 F03124), Limit / Upload / Parsing / YMS 적재 설계 문서
- **DB 확인 (로컬 docker `oracle19c-DMS`, 조회만)**: `TQP_GDS_CHAIN` PK `(FACTORY, DEVICE_ID, CHAIN_SEQ)`, `TQP_GDS_CHAIN_VER` / `TQP_GDS_CHAIN_MEMBER` / `TQP_GDS_PLACED_EL` 컬럼, SCK GDS Chain 17개(U9 15 / U7 2, 모두 Input 만), Chain 영역 안 Chain Label 조회(12절), `TQP_GDS_MAP_SUB_DEVICE`(OMM / OMM_Hybrid -> U9), `TQP_GDS_ET_ITEM` 컬럼 / 제약, `TQP_GDS_ET_LIMIT` 0행, `TQP_GDS_ET_CHAIN_GROUP` 0행, Chain 시험 계산(3절)
- **회의록 / 메일 확인**: 10-02 / 10-06 / 10-13 회의록 json · 녹취, 10-12 메일 회신 docx, Limit 설계 13.1 (10-12 사용자 요청)
- **Probe / Smoke 확인 (구현 후)**: `ET_PARA_SETUP_PROBE=PASS` 34/34 (BSL 직접 호출, 로컬 DB), `ET_PARA_FORM_SMOKE=PASS` 27/27 (진행 표시줄 3건 포함) · `ET_LIMIT_FORM_SMOKE=PASS` 43/43 (화면 -> RO -> BSL -> 로컬 DB, TEMP 빌드), `ET_RESULT_PROBE=PASS` 18/18 (5단계 Upload 재설계로 다시 작성)
- **미확인**: 배포 / Service Remoting 경유 / DMS 화면 직접 조작, C2 / C3, COWET / FT 실제 파일
