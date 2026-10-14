# Test 헤더 파싱 & Spec Rule 관리

헤더 2,755개 항목을 구성 요소로 분해하고, 값이 비슷한 항목을 묶은 **Rule 38개**(적용 36, 검토 대기 2)로 Spec을 관리한다.
Spec은 데이터로 계산한 **평균 ± 3σ** 초안이다.

- 작업 ID: CLAUDE-054 (F07 E-Test). DMS 소스 / DB는 수정하지 않은 분석 도구다.
- 데이터: 같은 폴더의 `CAI_OMM_ATV_A0_TSVET_20260609161940_RH-HS2615_RH-HS2615-02.xlsx` 표를 붙여넣은 `data/raw_test_data.tsv` (24 Die × 2,755 항목). 이 문서와 출력의 **Unit = Die**(`#3`, `#4` …).
- 연계 후보: E-Test Limit 설정(CLAUDE-029, `TQP_GDS_ET_LIMIT` / `frmEtLimitSetup`)에 이 Rule의 LSL / USL을 넣을지는 사용자 결정 대기.

## 1. 헤더 구조

```
<TEST>[_<MODE>][_<조건>=<값><단위>]@<NET>[:<NET>]
NET = <STRUCT_TYPE>_<ID>_<PIN>          PIN = FH | FL
```

TEST 뒤에 붙는 조건은 모두 `COND_NAME` 하나로 관리한다.
- `=`가 있는 조건(`iforce=8.0mA`)은 이름·값·단위로 나눈다 → COND_NAME=iforce, COND_VALUE=8.0, COND_UNIT=mA
- `=`가 없는 조건(Leakage의 `Parallel`, `Serial`)은 이름만 넣는다 → COND_NAME=Parallel

| 헤더 예 | TEST | COND_NAME | COND_VALUE | COND_UNIT | STRUCT_TYPE | ITEM_ID (세부 분해) | PIN |
|---|---|---|---|---|---|---|---|
| `Connectivity_iforce=8.0mA@C4DC_207_73_FH` | Connectivity | iforce | 8.0 | mA | C4DC | `207_73` (X=207, Y=73) | FH |
| `Resistance_iforce=8.0mA@TSVDC_ASIC1_P_FH` | Resistance | iforce | 8.0 | mA | TSVDC | `ASIC1_P` (KIND=ASIC, IDX=1, POL=P) | FH |
| `Resistance_iforce=1.0mA@PIC_SERP1_M3_FH` | Resistance | iforce | 1.0 | mA | PIC | `SERP1_M3` (KIND=SERP, IDX=1, SUB=M3) | FH |
| `Leakage_Parallel@C4DC_212_2_FL` | Leakage | Parallel | | | C4DC | `212_2` | FL |
| `Leakage_Serial@C4DC_136_37_FL:C4DC_137_37_FH` | Leakage | Serial | | | C4DC | `136_37:137_37` (NET 쌍) | FL:FH |

조건이 2개 이상인 헤더(예: `_Serial_vforce=1V`)는 각 컬럼에 `;`로 이어 붙이고 PARSE_NOTE에 표시한다.

현재 데이터 구성 (Unit 24개 × 항목 2,755개)

| TEST | COND_NAME | 항목 수 | 구조 |
|---|---|---|---|
| Continuity | iforce 8.0mA | 1 | C4DC 210_3 |
| Connectivity | iforce 8.0mA / 1.0mA | 865 / 10 | C4DC 837, TSVDC 28, PIC 10 |
| Resistance | iforce 8.0mA / 1.0mA | 865 / 10 | 위와 동일 (값 = Connectivity ÷ 전류) |
| Leakage | Parallel | 848 | C4DC 814, TSVDC 20, PIC 14 |
| Leakage | Serial | 156 | C4DC 139, TSVDC 9, PIC 8 |

- 구조 유형: **좌표형** C4DC(X_Y) / **이름형** TSVDC(TR·TL·BR·BL, R·L·TOP2·BOT1·ASIC1~4·ASIC_HBM·HBM0·HBM1·HBM_C × P/N), PIC(EDM_R/T/L/B, SERP1~2_M2~M4, COMB11~12_M3~M4)
- 첫 열 `Continuity…C4DC_210_3`은 `Connectivity…C4DC_210_3`과 값이 같고 이름만 다르다 → 별도 TEST로 관리
- Leakage 헤더에는 인가 전압과 단위가 없다 → UNIT=`TBD`

## 2. Spec Rule 테이블 (`spec_rules.csv`)

**주요 컬럼** (앞쪽)

| 컬럼 | 의미 |
|---|---|
| RULE_ID | CNT/CON/RES/LKP/LKS-번호 |
| TEST, COND_NAME, STRUCT_TYPE | 헤더 분해값과 비교. 빈칸 또는 `*` = 전체 |
| ID_PATTERN | ITEM_ID 와일드카드(`*`, `?`). 여러 개는 `\|`로 연결 (예: `ASIC?_N\|ASIC?_P\|TOP2_N\|TOP2_P`) |
| ITEM_CNT | 그룹에 묶인 항목(헤더) 수 |
| MEAN, STDEV | 평균·표준편차 (이상치 제외) |
| MAX, MIN | 최대·최소 (이상치 포함 전체 데이터) |
| LSL, USL | Spec = MEAN ∓ 3·STDEV. 빈칸이면 그쪽은 검사하지 않음 |

**보조 컬럼** (뒤쪽, Excel에서 숨겨도 됨)

| 컬럼 | 의미 |
|---|---|
| UNIT | V / ohm / TBD(Leakage) |
| COND_VALUE, COND_UNIT | 인가 조건 값·단위 (8.0 / mA). COND_VALUE는 숫자로 비교한다 (`8` = `8.0`). 여러 값은 `\|` |
| ENABLED | Y=적용, N=보류(검토 후보). 컬럼이 없으면 Y |
| PRIORITY | 숫자가 작을수록 우선. 10 개별 항목 / 30 영역(권장) / 50 이름형 구조 / 90 구조 기본 |
| PIN, X_MIN~Y_MAX | 핀(FH/FL), 좌표 범위 (좌표형 C4DC 전용, Serial은 첫 NET 기준) |
| N, OUTLIER_CNT | 계산에 쓴 측정값 수, 평균·표준편차 계산에서 뺀 이상치 수 |
| LIMIT_BASIS, NOTE | 근거·메모 (초안은 `DRAFT`로 시작, 통합/분리 사유) |

- 필수 컬럼은 RULE_ID, TEST, STRUCT_TYPE, ID_PATTERN, LSL, USL이다. 나머지 매칭 컬럼은 지워도 '전체'로 보고 동작한다.
- 단, 보조 컬럼은 **지우지 말고 숨기기**를 권장한다.
  - ENABLED를 지우면 비활성 Rule(48_37)까지 적용된다.
  - PRIORITY를 지우면 겹치는 Rule이 AMBIGUOUS 오류로 보고된다.

**매칭 규칙**: ENABLED=Y Rule 중 조건이 모두 맞는 것 → PRIORITY가 가장 작은 1개 적용.
같은 PRIORITY에서 2개 이상이 맞으면 `AMBIGUOUS`, 하나도 없으면 `UNMATCHED` 오류로 보고한다 (종료코드 2).

영역 Rule 예 (C4DC 상단 Edge만 다른 Spec):
```
RES-101,Resistance,iforce,C4DC,*,,,,,,4.28,20,ohm,8.0,mA,Y,30,*,85,150,73,73,,,고객 Spec,상단 Edge 라우팅 길이 차이
```

## 3. 사용법 (Python 3.8+, 추가 패키지 없음)

```powershell
python spec_rule_tool.py parse --data data\raw_test_data.tsv                          # 헤더 분해만
python spec_rule_tool.py draft --data data\raw_test_data.tsv --rules spec_rules.csv   # Rule 초안 생성 (기존 파일 있으면 --force 필요)
python spec_rule_tool.py apply --data data\raw_test_data.tsv --rules spec_rules.csv   # 매칭·검증·판정
```

`apply` 결과 (`out\`)

| 파일 | 내용 |
|---|---|
| `item_spec_map.csv` | 항목별 분해값 + 적용 RULE_ID + LSL/MEAN/USL (MES Spec 적재용) |
| `rule_stats.csv` | Rule별 실제 매칭 수(MATCHED_CNT), 데이터 중앙값/최소/최대, Fail 수, Fail 상위 항목 (Limit 보정용) |
| `judge_unit_summary.csv` | Unit별 PASS/FAIL_HIGH/FAIL_LOW/NO_DATA 건수와 Fail Rule |
| `judge_fail_list.csv` | Fail 상세 (Unit, 항목, 값, Spec) |
| `validation_report.txt` | 미매칭·중복매칭·미사용 Rule, ITEM_CNT와 실제 매칭 수 차이 |

운영 순서: `draft`(최초 1회) → 엔지니어가 LSL/USL·ENABLED 확정 → `apply` → `validation_report`가 0건인지 확인 → `item_spec_map.csv` 반영.
헤더가 바뀌거나 새 항목이 추가되면 `apply`가 UNMATCHED로 알려주므로, 해당 Rule만 추가한다.

## 4. 초안 생성 로직 (`draft`)

1. **기본 단위**: 좌표형(C4DC)은 구조 전체 1개, 이름형(TSVDC/PIC)은 ID별 1개. Leakage는 구조별 1개(PIC만 COMB/SERP 분리).
2. **값 기준 통합**: 같은 TEST·조건·구조 안에서 값 순으로 정렬한 뒤, 합쳐도 기준을 넘지 않는 인접 그룹부터 반복해서 묶는다.
   - 저항·전압형: ID별 중앙값 편차 (최대−최소)/중앙 ≤ 10% (`--merge-pct`, 0이면 통합 안 함).
     - 예: ASIC1~4_P·N과 TOP2_P·N(216~227 Ω, 편차 4.8%) → 10개를 1개 Rule로 통합.
     - 예: HBM0_N(307 Ω)은 가장 가까운 그룹과 11.8% 차이라서 **이름은 비슷하지만 Spec을 분리**했다.
   - Leakage: σ 비율 ≤ 1.5배이고 평균 차이 ≤ 0.5σ (`--leak-sd-ratio`, `--leak-mean-sigma`).
   - 구조 번호만 다른 ID가 같은 구조 안에서 전부 묶이면 `?`로 줄인다 (ASIC1~4_P → `ASIC?_P`).
3. **Spec = 평균 ± 3σ** (모든 Test 공통, k는 `--sigma`)
   - 평균·표준편차는 Q1/Q3 ∓ 3·IQR 밖 이상치를 빼고 계산한다 (`--fence-k`, 뺀 수는 OUTLIER_CNT).
     - 이상치 예: Clamp값 5.75 V, 747, -951, C4DC 48_37.
     - 빼지 않으면 PIC COMB Leakage의 σ가 110이 되어 Spec이 ±330이 된다.
   - MAX/MIN은 이상치를 포함한 전체 데이터다.
   - 저항·전압형은 LSL이 음수로 나오면 0으로 둔다.
   - 3σ는 정상 분포에서도 약 0.3%가 벗어나는 관리 한계다. 넓히려면 `--sigma 4`.
4. **개별 Spec 후보**: 그룹 안에서 모든 Unit 값이 같은 방향으로 Spec을 벗어나는 항목은 `ENABLED=N` Override Rule로 만들어 둔다.
   - 해당 항목: C4DC 48_37 (CON-015 / RES-015).
   - 다른 구조로 확인되면 Y로 바꾸고, 아니면 불량 분석 대상으로 둔다.

옵션: `--sigma`, `--fence-k`, `--merge-pct`, `--leak-sd-ratio`, `--leak-mean-sigma`

## 5. 그룹 구성

Connectivity(CON, 단위 V)는 Resistance와 그룹·ITEM_CNT가 같아서 아래 표에서 생략했다. 전체는 Rule 파일 참조.

| RULE_ID | TEST | COND_NAME | STRUCT_TYPE | ID_PATTERN | ITEM_CNT | MEAN | STDEV | MAX | MIN | LSL | USL |
|---|---|---|---|---|---|---|---|---|---|---|---|
| CNT-001 | Continuity | iforce | C4DC | `*` | 1 | 0.07405 | 0.00583 | 0.0876 | 0.0668 | 0.05656 | 0.09155 |
| RES-001 | Resistance | iforce | PIC | `SERP?_M3\|SERP?_M4` | 4 | 72.63 | 1.33 | 257 | 69.6 | 68.62 | 76.63 |
| RES-002 | Resistance | iforce | PIC | `EDM_B\|EDM_T\|SERP?_M2` | 4 | 156.12 | 4.8 | 6400 | 148 | 141.72 | 170.51 |
| RES-003 | Resistance | iforce | PIC | `EDM_L\|EDM_R` | 2 | 188.26 | 4.66 | 679 | 182 | 174.28 | 202.24 |
| RES-004 | Resistance | iforce | C4DC | `*` | 837 | 9.72 | 1.22 | 117 | 7.8 | 6.06 | 13.38 |
| RES-005 | Resistance | iforce | TSVDC | `TL\|TR` | 2 | 11.804 | 0.522 | 12.9 | 10.9 | 10.239 | 13.37 |
| RES-006 | Resistance | iforce | TSVDC | `BL\|BR` | 2 | 13.61 | 1.11 | 19.4 | 11.7 | 10.29 | 16.93 |
| RES-007 | Resistance | iforce | TSVDC | `HBM_C_N` | 1 | 65.4 | 1.7 | 67.6 | 61.9 | 60.31 | 70.49 |
| RES-008 | Resistance | iforce | TSVDC | `HBM_C_P` | 1 | 73.46 | 1.75 | 76.9 | 70.4 | 68.21 | 78.71 |
| RES-009 | Resistance | iforce | TSVDC | `L_P\|R_N` | 2 | 90.51 | 2.07 | 719 | 86.5 | 84.31 | 96.71 |
| RES-010 | Resistance | iforce | TSVDC | `ASIC_HBM_N\|ASIC_HBM_P\|BOT1_N\|BOT1_P` | 4 | 111.85 | 2.72 | 719 | 106 | 103.68 | 120.02 |
| RES-011 | Resistance | iforce | TSVDC | `L_N\|R_P` | 2 | 128.15 | 3.86 | 718 | 77 | 116.56 | 139.74 |
| RES-012 | Resistance | iforce | TSVDC | `ASIC?_N\|ASIC?_P\|TOP2_N\|TOP2_P` | 10 | 219.27 | 6.68 | 239 | 207 | 199.22 | 239.31 |
| RES-013 | Resistance | iforce | TSVDC | `HBM0_N` | 1 | 307.21 | 7.92 | 324 | 291 | 283.45 | 330.96 |
| RES-014 | Resistance | iforce | TSVDC | `HBM1_N\|HBM?_P` | 3 | 339.3 | 10.5 | 719 | 314 | 307.7 | 370.9 |
| LKP-001 | Leakage | Parallel | C4DC | `*` | 814 | -0.0139 | 0.0922 | 0.313 | -0.343 | -0.2906 | 0.2628 |
| LKP-002 | Leakage | Parallel | TSVDC | `*` | 20 | -0.0194 | 0.0777 | 0.197 | -0.176 | -0.2526 | 0.2138 |
| LKP-003 | Leakage | Parallel | PIC | `COMB*` | 8 | -0.004 | 0.143 | 747 | -951 | -0.433 | 0.425 |
| LKP-004 | Leakage | Parallel | PIC | `SERP*` | 6 | -0.0001 | 0.0608 | 0.156 | -0.563 | -0.1824 | 0.1823 |
| LKS-001 | Leakage | Serial | C4DC | `*` | 139 | -0.128 | 0.139 | 0.345 | -0.56 | -0.547 | 0.29 |
| LKS-002 | Leakage | Serial | TSVDC | `*` | 9 | -0.252 | 0.179 | 0.0611 | -0.746 | -0.79 | 0.286 |
| LKS-003 | Leakage | Serial | PIC | `COMB*\|SERP*` | 8 | -0.319 | 0.26 | -0.00172 | -747 | -1.099 | 0.461 |
| RES-015 (비활성) | Resistance | iforce | C4DC | `48_37` | 1 | 100.59 | 8.98 | 117 | 48.1 | 73.64 | 127.55 |

- 통합되지 않은 그룹: HBM_C_N / HBM_C_P (합치면 편차 11.5%), HBM0_N (가장 가까운 그룹과 11.8%), Leakage Parallel PIC COMB / SERP (σ 비율 2.35배).

## 6. 확인 필요 사항

- **Spec 값**: 모든 Limit은 24개 Unit 데이터로 만든 초안(`DRAFT`)이다. 고객/설계 Spec으로 교체해야 한다.
- **Leakage 단위·인가 조건**: 헤더에 없다. Spec 값은 데이터와 같은 단위다.
- **Leakage 고정 편차 Net**: C4DC 5_38_FH·4_3_FL(평균 약 +0.27), 2_72_FL(약 -0.31), 1_46_FH(약 -0.27)는 24개 Unit 모두 같은 방향으로 치우쳐 있다.
  - C4DC Parallel 3σ Fail 70건 중 60건이 이 4개에서 나온다.
  - Unit과 무관하게 같은 값이라 측정 Channel Offset일 가능성이 크다. Offset 보정 또는 개별 Rule 검토 필요.
- **C4DC 48_37**: 모든 Unit에서 약 100 Ω (다른 좌표는 9.4 Ω). 설계 차이인지 확인 필요.
- **C4DC 상단 Edge(Y=73, X 85~150)**: 고저항이고 Unit마다 크게 흔들린다 (109_73: 8.5~60 Ω).
  - 3σ 기준 C4DC 저항 USL은 13.38 Ω이다. C4DC 저항 Fail 870건 중 520건이 이 영역에서 나온다.
  - 구조 차이로 확인되면 영역 Rule(PRIORITY 30)을 추가한다. 아니면 Probe 접촉·Bump 불량을 확인한다.
- **초안 기준 판정**: 24개 Unit 모두 1건 이상 Fail이다. 대표 사례:
  - #44: TSVDC 6개가 5.75 V Clamp(Open).
  - #28: TSVDC R_P 저값 77 Ω.
  - #4: PIC EDM_T 6.4 V.
  - #44: PIC SERP2_M3·M4 76.7 / 77.3 Ω (USL 76.63).
  - #5/#14/#20/#42: PIC COMB12_M4 Leakage(±747 Clamp 포함).
  - #20/#22/#31: PIC COMB `-` 미측정 17건. NO_DATA는 FAIL로 처리했다.
