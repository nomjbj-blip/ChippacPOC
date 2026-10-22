# E-Test Upload 시 YMS 테이블 적재 설계

작성: 2026-10-14 Claude (CLAUDE-085) / 상태: **확정 설계 (권장안 D1 ~ D9 채택, 14절이 최신)**. 1 ~ 13절은 검토 과정이다(14절과 다르면 14절을 따른다).

구현 1차 완료 (CLAUDE-087, 15절):
- 범위: 규칙 클래스 / YMS 적재 핵심 / DDL 파일.
- 검증: Probe 2개 PASS.
- 정정: 컬럼 이름 한도 50자.
- 2차 정리(15.5): 조회 / 삭제를 기존 YMS 쿼리·함수로 바꾸고 기존 `TestDataMaint.DeleteWaferData` 빈 Lot 버그를 고쳤다. 새 쿼리는 `UPDATE_YMS_PARASPEC` 1개만 남았다.
- ~~`GdsEtResult` 연결과 화면은 Para Setup 결정 / Upload 재설계 1단계 뒤에 한다.~~ -> **3차(16절): 업로드 서비스에 연결 완료.**
  - 저장 확정 / 다시 판정 / Map 좌표 다시 계산 때 자동 반영되고, 파일을 지우면 YMS 도 같이 지운다.
  - YMS 메뉴를 열었다. 화면(ENGUI) 수정은 없다.
  - 로컬 DB DDL 을 적용했다.
  - 서비스 Probe PASS.

사용자 지시 (2026-10-14):

> E-Test File Upload 화면 지금 설계 개발 전 상태인데 해당 화면에서 Upload 시 필요한 항목에 대해 Parsing 된 결과를 YMS Table 상에 넣어주는 Logic을 넣는 방향으로 설계 해줘

선행 문서:
- [E-Test Upload 재설계](2026-10-13_ETest_Upload_재설계.md) 9절: 확정 설계, 구현 대기. **이 문서는 9절에 YMS 적재를 더한다.**
- [E-Test YMS 화면 연계 검토](2026-10-14_ETest_YMS_화면연계_검토.md): YMS 11개 화면 가능 여부 / TESTAREA 비교.
- [Die 좌표 대응 설계](../03_KLARF_Map/2026-10-13_Die좌표_대응_Setup_설계.md) 9절: `DieCoordinateMapper`.

---

## 1. 목표와 범위

- 업로드 화면에서 파일을 저장하면 Parsing 결과가 **두 곳에 함께 들어가게** 한다.
  - E-Test 테이블(`TQP_GDS_*`): 지금 설계대로.
  - YMS 테이블(`TQP_LOT` / `TQP_WAFER` / `TD_<PROGRAM>` / `TQP_PARASPEC` / `TQP_BINDESC` / `TQP_WAFER_SUM` / `TQP_LOT_SUM`)
- 저장이 끝나면 YMS 기존 화면에서 바로 조회된다. 대상 화면: TEST Map View / Bin·Test Map Gallery / Wafer Cum / Bin Stack / Yield / Param Analysis / Standard Report / Test Report.
- Function List YMS "E-Test Data Manual Input" 이 이 업로드 화면으로 충족된다.

| 포함 | 제외 |
| --- | --- |
| 업로드 저장 흐름에 YMS 적재 단계 추가 (서버) | YMS 화면 C# 수정 |
| 업로드 화면에 YMS 미리보기 / 상태 / [YMS 다시 반영] | KLARF 와 Test 매칭(`DieDefectMatching`) — Wafer 매칭 규칙 고객 대기 |
| 삭제 / 재판정 / 좌표 재계산 때 YMS 쪽도 같이 갱신 | FT (파일 수령 후, 재설계 U9) |
| YMS 화면을 열기 위한 메뉴 / 조건 설정 (마지막 단계) | YMS 자동 수집(DataService) 경로 |

**E-Test 테이블은 그대로 둔다.** 연관 분석 / Spec 화면 / Chain 묶기가 `TQP_GDS_ET_*` 를 쓰기 때문이다. YMS 테이블은 YMS 화면용 사본이고, 원본은 E-Test 테이블이다(결정 D1).

## 2. 기존 YMS 적재 방식 (재사용 근거)

YMS 는 설비 파일(CP / PCM)을 아래 순서로 적재한다(`09_TEST/DACrux.TEST.PCM.DataService/PCMDataService.cs:62-291`). E-Test 적재도 **같은 함수를 같은 순서로** 부른다.

| 순서 | 기존 함수 (`DACrux.TEST.BSL`) | 하는 일 | 확인한 내용 |
| --- | --- | --- | --- |
| 1 | `TestCommon.InsertProductInfo(factory, product, mapid)` | `TQP_PRODUCT` 없으면 INSERT | MERGE, 있으면 그대로 |
| 2 | `TestCommon.InsertProgramInfo(factory, program, testarea, device)` | `TQP_PROGRAM` 없으면 INSERT | MERGE |
| 3 | `TestCommon.GetMaxProgramRev(factory, program)` | Program Rev | 없으면 0 (`SELECT_MAX_PROGRAM_REV`) |
| 4 | `TestCommon.SaveWaferData(...)` (31개 인자판, `TestCommon.cs:833`) | `TQP_LOT` / `TQP_WAFER` MERGE -> LOT_SEQ / WAFER_SEQ | Lot 키 = Factory + TestArea + Lot + Program. 같은 Wafer + 같은 시작 시간이면 기존 WAFER_SEQ 재사용, 다르면 새 행 + 이전 행은 PROBE_CNT +1 (재측정 이력) |
| 5 | `TestDataManager.SaveData(factory, program, rawDt, waferSeq)` (`TestDataManager.cs:40`) | TD 테이블 / 컬럼 / `TQP_PARASPEC` / `TQP_DATA_TABLES` 자동 생성 + 그 Wafer 행 지우고 다시 넣기 | 컬럼 이름 검사 / 변환 없음 (`TD_TABLE.cs:197-237`). 빈 값 -> NULL, 숫자 아닌 값 -> decimal.MinValue / MaxValue (`:317-320`, `:497-518`). DIE_NUM = 행 순서 |
| 6 | `TestCommon.SummaryData(factory, lotSeq, waferSeq, waferId)` | `TQP_WAFER_SUM` / `TQP_LOT_SUM` 다시 만듦 | CUSTOMER = 'DB' 고정, NETDIE = `TQP_MAPDEF`(Factory + DEVICE_ALIAS), BIN < 100 집계, Good = `TQP_BINDESC.HIGH_GEC='Y'` (없으면 BIN1) |
| 삭제 | `TestDataMaint.DeleteWaferData(waferSeq)` (`TestDataMaint.cs:212`) | Wafer / Wafer 요약 / TD 행 삭제, 빈 TD 테이블 정리, Lot 요약 재생성, 빈 Lot 삭제 | 2026-10-08 수정본 |

부르지 않는 함수:
- `GetLotStatusInfo` / `GetProductInfo`: MES `TQC_LOT_STS` 에서 Device 를 찾는다. E-Test 는 파일명에서 받는다.
- `UpdateParsingInfo`: 설비 파싱 정보다.
- `DieDefectMatching`: KLARF 매칭 규칙이 정해진 뒤에 검토한다.

서버 위치:
- E-Test 서비스 `DACrux.SEMDMS.BSL.GdsEtResult` 에 메서드를 추가한다.
- `DACrux.SEMDMS.BSL` 에 `DACrux.TEST.BSL` 프로젝트 참조 1개를 추가한다. `TEST.BSL` 은 `SEMDMS.DSL` 만 참조하므로 순환 참조가 생기지 않는다.
- SEMDMS Service 설정에 `TEST_CONNECT_ID = DMSMGR` 이 이미 있어(`101_ServerBin/DACrux.SEMDMS.Service.exe.config:53`) TEST DSL 이 같은 DB 로 붙는다.
- 다른 BSL 안에서 `new TestCommon()` 으로 직접 부르는 기존 예가 있다(`TestDataMaint.cs:285`).
- **새 wellknown 은 없다.**

## 3. 업로드 저장 흐름 (재설계 8.2 / 9.4 에 단계 추가)

```text
[화면] 파일 추가 -> 분석(백그라운드)
         1) 파일명 8칸 -> 기준 정보          2) Spec 확인 (없으면 중단)
         3) 헤더 ↔ Spec 대조 (정의 항목만)   4) Die 판정 미리보기
         5) Map 확인 (Map 좌표 미리 계산)     6) 중복 확인
         7) (신규) YMS 미리보기: Program / TestArea / Bin 1·2·3 수 / 예상 수율 / YMS 컬럼 이름 검사
[저장] 파일마다
   ├ BeginEtFile -> MergeEtItem -> CreateEtDieBatch -> CreateEtValueBatch -> CompleteEtFile(판정 + Map 좌표 + READY)   ← 재설계 9.4
   └ (신규) PublishEtFileToYms(파일 번호)                                                                              ← 이 문서
         결과를 대기 목록 행에 표시: "저장됨 / YMS 반영됨 (Wafer n, 수율 95.8%)" 또는 "저장됨 / YMS 실패: 사유"
```

- YMS 적재는 `CompleteEtFile` 안에 넣지 않고 **별도 호출**로 한다(결정 D9).
  - 서버 호출 1번이 길어지지 않는다.
  - YMS 가 실패해도 E-Test 저장 결과는 남는다.
  - 같은 메서드를 [YMS 다시 반영] / 재판정 / 좌표 재계산에 다시 쓴다.
- **하나의 트랜잭션이 아니다.** TD 테이블 생성 / 컬럼 추가가 DDL(자동 커밋)이고 기존 함수들이 각각 커밋한다. 그래서 상태 컬럼으로 결과를 남기고, 다시 반영하면 같은 결과가 나오게(멱등) 만든다(5절).

## 4. Parsing 결과 -> YMS 테이블 매핑

### 4.1 이름 / 키 규칙

| 항목 | 규칙 | 샘플 (`CAI_OMM_ATV_A0_TSVET_20260609161940_RH-HS2615_RH-HS2615-02.xlsx`) |
| --- | --- | --- |
| PROGRAM | 파일 Device + `_` + 공정, 대문자, `[A-Z0-9_]` 밖 문자는 `_` | `OMM_TSVET` -> TD 테이블 `TD_OMM_TSVET` |
| TESTAREA | 공정 (파일명 5칸 Test Type) | `TSVET` |
| DEVICE (= PRODUCT) | 파일 Device (`DEVICE_ID`) | `OMM` |
| DEVICE_ALIAS (= Map 연결) | 업로드 때 쓴 `MAP_ID` (= Device) | `OMM` (File Map Setup 으로 등록 필요, 로컬에는 아직 없음) |
| LOT_ID / WAFER_ID | `LOT_ID` / `WAFER_ID` (Lot + 2자리 Slot) | `RH-HS2615` / `RH-HS2615-02` |
| 시간 | 파일명 Test Date | 2026-06-09 16:19:40 (START = END) |

### 4.2 테이블별

| YMS 테이블 | 컬럼 | 값 (Parsing / 업로드 결과) |
| --- | --- | --- |
| `TQP_PRODUCT` | FACTORY / PRODUCT / MAPID | 로그인 Factory / DEVICE_ID / MAP_ID. 이미 있으면 그대로 (기존 MERGE) |
| `TQP_PROGRAM` | FACTORY / PROGRAM / TESTAREA / DEVICE | 4.1 |
| `TQP_LOT` | LOT_ID / TEST_AREA / PROGRAM / DEVICE / DEVICE_ALIAS / START·END_TIME / WAFER_DIAM | 4.1. WAFER_DIAM = Map `WAFER_SIZE / 1000` (mm, 없으면 300) |
| `TQP_WAFER` | WAFER_ID / START·END_TIME / PROGRAM / PROGRAM_REV | 4.1 / Rev = 3번 결과 |
|  | MAX_X / MAX_Y | YMS 에 넣는 Die 의 MAP_X / MAP_Y 최대값 |
|  | NUM_DIE | YMS 에 넣는 Die 수 |
|  | DATA_SOURCE | 원본 파일명 |
|  | COMMENTS | `Lot-Wafer No` 원문 (KLARF Wafer 매칭 규칙이 오면 참고) |
|  | TESTER / PROBER / PROBE_CARD / OPERATOR / TEMP | 비움 (파일에 없음) |
| `TD_OMM_TSVET` (자동 생성) | WAFER_SEQ / DIE_NUM / DIEPROBE_CNT | 기존 함수가 채움. 행 순서는 고객 Die# 순으로 넘긴다 -> DIE_NUM 순서 = Die# 순서 (Die# 값 자체는 넣지 않음, R9) |
|  | X / Y | `TQP_GDS_ET_DIE.MAP_X / MAP_Y` (시스템 좌표, LeftBottom (1,1)). YMS Map 은 이 값을 변환 없이 Map Index 로 쓴다 |
|  | BIN | Die 판정: **1 = Pass / 2 = Spec Fail (범위 밖 값이 있음) / 3 = 미측정('-') 때문에만 Fail** (결정 D6) | → **2026-10-14 변경 (18절): YMS BIN = 결과 파일 Hardware Bin, E-Test Spec 판정과 별개**
|  | Parameter 컬럼 | Spec 정의 Parameter 만 (재설계 S4). 컬럼 이름 = 4.3 규칙. 값 = `MEAS_VALUE` (InvariantCulture 문자열), **'-' / 빈 칸은 빈 문자열 -> NULL** (decimal.MinValue 방지) |
| `TQP_PARASPEC` (자동 생성 후 보정) | PARAM_NAME / TABLE_NAME / PARAM_INDEX | 기존 함수가 등록 |
|  | PARAM_DESC | 헤더 원문 (Spec ITEM_NAME) |
|  | UOM | Spec UNIT |
|  | LSL / USL | Spec Lower / Upper |
|  | PARAM_TYPE | X / Y / BIN = `INT`, 나머지 `FLOAT` |
|  | 보정 방법 | **신규 쿼리**로 갱신. 기존 YMS XML 은 고치지 않고 E-Test XML 에 추가 |
| `TQP_DATA_TABLES` | FACTORY / PROGRAM / TABLE_NAME | 기존 함수 자동 |
| `TQP_BINDESC` | PROGRAM / BIN / BIN_NAME / HIGH_GEC / COLOR | Program 마다 1 `PASS`(Y, 녹색) / 2 `SPEC_FAIL`(N, 빨강) / 3 `NOT_TESTED`(N, 주황). **없을 때만 등록** — Bin Define 화면에서 고친 값은 덮어쓰지 않는다(기존 `EXISTS_BIN_INFO` / `CREATE_BIN`) | → **2026-10-14 변경 (18절): YMS BIN = 결과 파일 Hardware Bin, E-Test Spec 판정과 별개**
| `TQP_WAFER_SUM` / `TQP_LOT_SUM` | BINn / TESTED_DIE / GEC / YIELD / NETDIE | 기존 `SummaryData`. 샘플: BIN1 23 / BIN2 1 / TESTED 24 / NETDIE 76 -> 1.5배 넘게 차이라 측정 Die 수로 나눔 -> **YIELD 95.83** |

YMS 에 넣지 않는 Parsing 결과(E-Test 테이블에만 저장):
- 파일명 Customer / Wafer Type / Wafer Rev
- 정의 안 된 헤더(재설계 S4: 저장 안 함)
- HW / SW Bin / DUT Flag / Die 별 시험 시간
- `$Silterra$`(저장 안 함)

### 4.3 YMS 컬럼 이름 규칙 (Parameter)

분석 화면은 Parameter 를 **컬럼 이름 그대로** 보여준다(`SELECT_PARA_ITEM` 이 `ALL_TAB_COLUMNS` 와 이름 조인). 사람이 읽을 수 있게 바꾼다.

| 순서 | 규칙 | 예 |
| --- | --- | --- |
| 1 | Spec ITEM_NAME (대문자, '#번호' 제거된 값)에서 시작 | `RESISTANCE_IFORCE=8.0MA@C4DC_28_37_FH` |
| 2 | `.` -> `P` (기존 Fab2 PCM 파서와 같은 방식) | `RESISTANCE_IFORCE=8P0MA@C4DC_28_37_FH` |
| 3 | `[A-Z0-9_]` 밖 문자(`=` `@` `:` `-` 공백 등) -> `_`, 연속 `_` 는 1개 | `RESISTANCE_IFORCE_8P0MA_C4DC_28_37_FH` |
| 4 | 숫자로 시작하면 앞에 `P_` | |
| 5 | 예약 이름(X / Y / BIN / WAFER_SEQ / DIE_NUM / DIEPROBE_CNT)과 같으면 앞에 `P_` | |
| 6 | 길이 한도 넘으면 오류 | 로컬 19c = 128바이트 (헤더 최대 54자라 통과). **고객 DB 가 12.1 이하면 30바이트** -> 고객 DB 버전 확인 후 한도 상수 결정. **(2026-10-14 구현 때 정정) 실제 한도는 50자다.** `TQP_PARASPEC.PARAM_NAME` 이 VARCHAR2(50)이고 컬럼 이름과 같아야 한다. 50자를 넘으면 약어로 줄이고, 그래도 넘으면 해시를 붙인다 (15절) |
| 7 | 같은 Device + 공정 안에서 서로 다른 헤더가 같은 이름이 되면 오류 | `A:B` 형식(Leakage_Serial)은 `A_B` 가 되어 구분 유지 |

- 규칙은 클래스 1개(`EtYmsRule`, `DACrux.SEMDMS.Interface`)에 둔다. `DieCoordinateMapper` 와 같은 방식이다.
  - 화면 미리보기와 서버 적재가 같은 클래스를 쓴다.
  - Program 이름 / Bin 판정 규칙도 이 클래스에 둔다.
- 처음 만든 이름은 `TQP_GDS_ET_ITEM.YMS_PARAM_NAME` 에 저장한다(결정 D7). 규칙이 나중에 바뀌어도 이미 만든 TD 컬럼과 어긋나지 않게 하기 위함이다.
- Spec 설정 화면에도 "YMS 이름" 열을 보여 주고, 저장할 때 6 / 7 을 미리 검사한다(업로드 전에 걸러짐).

## 5. 서비스 설계 (`iGdsEtResult` 추가 / 변경)

| 메서드 | 내용 |
| --- | --- |
| 신규 `PublishEtFileToYms(srcFileSeq, user)` | 6절 순서. 반환 1행: YMS_STATUS / PROGRAM / LOT_SEQ / WAFER_SEQ / DIE_COUNT / BIN1 / BIN2 / BIN3 / YIELD / SKIP_DIE(Map 밖) / MESSAGE |
| `DeleteEtFile` (변경) | E-Test 파일을 지우기 전에 `YMS_WAFER_SEQ` 가 있으면 `TestDataMaint.DeleteWaferData` |
| `RejudgeEtFiles` (재설계 9.4 신규, 변경) | 다시 판정한 파일마다 `PublishEtFileToYms` (BIN / 요약 / LSL·USL 갱신) |
| `RemapEtFiles` (재설계 9.4 신규, 변경) | 좌표 다시 계산한 파일마다 `PublishEtFileToYms` (X / Y 갱신, Map 이 생겨 보류가 풀린 파일 포함) |
| `GetEtFileList` / `GetEtFile` (변경) | 열 추가: YMS_STATUS / YMS_WAFER_SEQ / YMS_TIME / YMS_MESSAGE |

### 6. `PublishEtFileToYms` 처리 순서

```text
0. 읽기: 파일 헤더(READY 확인) / Die(MAP_X·Y, JUDGE_FLAG, FAIL_COUNT, NOT_TESTED_COUNT) / 값(정의 Parameter) / Spec / 항목 YMS 이름
1. 확인
   - MAP_ID 없음 -> YMS_STATUS = 'WAIT' ("Map 없음, Map 등록 후 좌표 다시 계산하면 반영") 하고 끝 (결정 D3)
   - Map 밖 Die (MAP_X NULL) -> YMS 에서 제외, SKIP_DIE 수로 알림
2. 이전 반영 정리: YMS_WAFER_SEQ 가 있으면 TestDataMaint.DeleteWaferData(이전 WAFER_SEQ)   ← 다시 반영해도 결과가 같게
3. InsertProductInfo / InsertProgramInfo / GetMaxProgramRev
4. SaveWaferData -> LOT_SEQ / WAFER_SEQ  => 바로 SRC_FILE 에 YMS_WAFER_SEQ 기록 (중간 실패해도 다음에 정리 가능)
5. rawDt 만들기: 열 X / Y / BIN / Parameter(YMS 이름), 행 = Die (Die# 순), 값은 문자열('-' / 빈 칸 = "")
6. TestDataManager.SaveData(factory, program, rawDt, waferSeq)  -> TD / PARASPEC / DATA_TABLES
7. PARASPEC 보정 (신규 쿼리): PARAM_DESC / UOM / LSL / USL / PARAM_TYPE
8. BINDESC: Program 의 Bin 1 / 2 / 3 이 없으면 등록
9. SummaryData(factory, lotSeq, waferSeq, waferId)
10. SRC_FILE: YMS_STATUS = 'DONE', YMS_LOT_SEQ / YMS_PROGRAM / YMS_TIME, YMS_MESSAGE = NULL
실패: 그 단계 예외 메시지를 YMS_STATUS = 'FAIL' / YMS_MESSAGE 에 기록하고 E-Test 데이터는 그대로 둔다
```

- 덮어쓰기(같은 Factory + Wafer + 공정 재업로드)는 재설계대로 이전 E-Test 파일을 지운다. 이때 `DeleteEtFile` 이 YMS Wafer 도 지우므로 YMS 에는 새 파일 1개만 남는다(재측정 이력 없음, 결정 D8).
- 2번 정리 덕분에 `SaveWaferData` 가 새 행을 만들어도 PROBE_CNT 는 0 이 된다.
- 성능: 파일당 Die 24 ~ 76 x Parameter 약 40 -> 수천 값이다. TD 테이블 생성(DDL)은 Program 마다 처음 한 번뿐이다.

## 7. 데이터 변경 (DDL `Persistence/2026-10-14_tqp_gds_et_yms.sql`, 재설계 9.3 DDL 과 같이 적용)

| 테이블 | 추가 컬럼 | 내용 |
| --- | --- | --- |
| `TQP_GDS_SRC_FILE` | `YMS_STATUS VARCHAR2(10)` | NULL = 미반영 / `DONE` / `WAIT`(Map 없음) / `FAIL` (CHECK) |
|  | `YMS_PROGRAM VARCHAR2(50 CHAR)` / `YMS_LOT_SEQ NUMBER` / `YMS_WAFER_SEQ NUMBER` | 반영한 YMS 키 (삭제 / 다시 반영용) |
|  | `YMS_TIME DATE` / `YMS_MESSAGE VARCHAR2(500 CHAR)` | 반영 시각 / 실패 사유 |
| `TQP_GDS_ET_ITEM` | `YMS_PARAM_NAME VARCHAR2(128 CHAR)` + UQ (FACTORY, DEVICE_ID, YMS_PARAM_NAME) | 4.3 |

쿼리 (E-Test XML 에 추가만, YMS 기존 XML 은 수정 없음):
- `TQP_GDS_SRC_FILE.xml`: `UPDATE_SRC_FILE_YMS`
- 신규 `TQP_GDS_ET_YMS.xml`: `UPDATE_YMS_PARASPEC` (FACTORY + PROGRAM + PROGRAM_REV + PARAM_NAME 으로 PARAM_DESC / UOM / LSL / USL / PARAM_TYPE)
- `TQP_GDS_ET_ITEM.xml`: MERGE 에 YMS_PARAM_NAME 추가

## 8. 업로드 화면 변경 (재설계 9.6 에 추가)

| 영역 | 추가 내용 |
| --- | --- |
| 대기 목록 | 열 **YMS**: 분석 후 "반영 예정 OMM_TSVET" / "Map 없음 - 보류" / "이름 오류" -> 저장 후 "반영됨 (수율 95.8%)" / "실패" |
| 선택 파일: 기준 정보 | YMS 칸: Program / TestArea / Lot / Wafer / Map(DEVICE_ALIAS) / YMS Die 수 / Bin 1·2·3 수 / 예상 수율 |
| 선택 파일: 탭 추가 [YMS 항목] | 헤더 원문 / YMS 컬럼 이름 / LSL / USL / 단위 / 검사 결과(길이 초과 · 이름 겹침 빨간색) |
| 저장 | 파일마다 "E-Test 저장 -> YMS 반영" 두 단계 진행 표시. YMS 실패는 그 행만 "실패" (다른 파일 계속) |
| 탭 [등록 파일] | 열 추가: YMS 상태 / YMS Wafer / 수율 / 반영 시각. 버튼 **[YMS 다시 반영]** (고른 파일). [선택 파일 삭제] 확인 문구에 "YMS 데이터도 삭제" |

- YMS 이름 오류(4.3 의 6 / 7)는 분석 단계에서 "오류" 상태로 저장 대상에서 뺀다. 두 테이블이 어긋나지 않게 하기 위함이다.
- 미리보기 계산은 `EtYmsRule` + 재설계 `EtJudgeRule` 로 화면에서 한다. 저장 전이라 서버 호출은 없다.

## 9. YMS 화면 열기 (마지막 단계, 화면 코드 수정 없음)

| 대상 | 변경 | 이유 |
| --- | --- | --- |
| `01_WinApp/DACruxV5/MenuFile.mnu` VISIBLE | `MNU_TEST` / `MNU_TEST_CP` / `MNU_PCM_ANALYSIS` / `MNU_STANDARD_REPORT` / `MNU_CUM_REPORT` / `MNU_BIN_WAFER_REPORT` / `MNU_YIELD_REPORT` -> True | 지금 숨김 |
| 같은 파일 RESV_03 | `MNU_CUM_REPORT` / `MNU_BIN_WAFER_REPORT` / `MNU_YIELD_REPORT` / `MNU_TEST_REPORT` 에 `,TSVET,COWET,FT` 추가 | Cum / Bin Stack 은 목록에 없는 TestArea Wafer 를 거부 (`frmCumMapReport.cs:624`, `frmWaferBinReport.cs:50`) |
| `08_DBQuery/ORACLE/TQP_PROGRAM.xml` `SELECT_TESTAREA` | 고정 6개에 TSVET / COWET / FT 3줄 추가 | Standard Report TestArea 목록 |

행 순서는 바꾸지 않고 값만 고친다(CLAUDE-010 교훈).

## 10. 결정 필요 (권장 먼저)

| 번호 | 항목 | 권장 | 다른 안 |
| --- | --- | --- | --- |
| D1 | 저장 위치 | **E-Test 테이블 + YMS 테이블 둘 다** (E-Test 가 원본) | YMS 만 — 연관 분석 / Spec / Chain 화면이 E-Test 테이블을 써서 다시 만들어야 함 |
| D2 | YMS 반영 시점 | **업로드 저장 때 항상** | 화면 체크박스로 선택 / [YMS 반영] 버튼으로만 |
| D3 | Map 없는 파일 | **YMS 반영 보류(WAIT)**, Map 등록 후 [Map 좌표 다시 계산] 때 자동 반영 | E-Test 원본 좌표로 반영 (YMS 가 가상 격자로 그림, 방향이 Map 과 다름) |
| D4 | TESTAREA | **공정명 TSVET / COWET / FT** (+ 9절 설정) | MULTIPROBE 빌림 / PARAMETRIC (Map 모양 안 나옴) |
| D5 | PROGRAM | **Device_공정** (`OMM_TSVET`) | 파일 Test Type 그대로 (`TSVET`, Device 간 섞임) |
| D6 | BIN | **1 Pass / 2 Spec Fail / 3 미측정만 Fail**, 기존 Bin 정의는 덮어쓰지 않음 | 1 Pass / 2 Fail | → **2026-10-14 변경 (18절): YMS BIN = 결과 파일 Hardware Bin, E-Test Spec 판정과 별개**
| D7 | YMS 컬럼 이름 | **4.3 규칙 + ET_ITEM 에 저장** | 매번 규칙으로 계산 / 일련번호(ET0001) |
| D8 | 같은 Wafer + 공정 재업로드 | **YMS 도 교체** (E-Test 와 같게, 재측정 이력 없음) | YMS 에는 재측정 이력(PROBE_CNT)으로 남김 |
| D9 | YMS 실패 처리 | **E-Test 저장은 유지 + YMS_STATUS FAIL + [YMS 다시 반영]** | 파일 전체 실패 (E-Test 도 지움) |

## 11. 구현 순서 (재설계 9.9 와 합침)

| 단계 | 내용 | 비고 |
| --- | --- | --- |
| 1 | 재설계 9.9 의 1단계 (판정 / Map 좌표 DDL + 서비스) | 선행 |
| 2 | DDL(7절) + `EtYmsRule` + `PublishEtFileToYms` / 삭제 · 재판정 · 재계산 연동 + 참조 1개 + Probe | Probe: 샘플 1파일 -> YMS 테이블 행 수 / 값 / '-' NULL / 수율 95.83, 다시 반영 후 행 수 같음, 삭제 후 YMS 정리, Map 없음 WAIT |
| 3 | 재설계 9.9 의 2단계 업로드 화면 + 8절 YMS 칸 / 탭 / 버튼 + Smoke | |
| 4 | Spec 화면 YMS 이름 열 / 검사 (재설계 9.9 의 3단계와 같이) | |
| 5 | 9절 메뉴 / 조건 + 사용자 DMS 에서 YMS 11개 화면 확인 | Map 원점(검토 문서 5절 4) 화면 확인 포함 |

## 12. 확인 필요

| 항목 | 내용 |
| --- | --- |
| 고객 DB 버전 | 12.1 이하면 컬럼 이름 30바이트 -> 4.3 규칙 6 의 한도와 짧은 이름 규칙 필요 |
| 고객 DB 테이블스페이스 | TD 테이블 생성 DDL 이 `TS_TEST_TBL` / `TS_TEST_IDX` 를 쓴다(`TD_TABLE.cs:13-14`). 로컬은 있음, 고객 서버 설치 때 확인 |
| `OMM` Map | Map ID = 파일 Device(`OMM`). 로컬에는 `OMM_Hybrid` / `Hybrid` 만 있어 File Map Setup 으로 `OMM` 등록 필요 |
| Map 원점 | LeftTop Map 이 YMS 화면에서 X 1칸 밀려 보일 가능성 (검토 문서 5절 4) — 5단계 화면 확인 |
| Wafer 목록 기간 | YMS Wafer 선택은 END_TIME 기간으로 거른다. E-Test 시간은 파일의 Test Date(샘플 2026-06-09)라 조회 기간을 그에 맞춰야 보인다 |

## 13. 확인 수준

- 설계: 이 문서 전체.
- 소스 확인:
  - `PCMDataService.ProcessingDefaultPcmData`
  - `TestCommon`: `InsertProductInfo` / `InsertProgramInfo` / `GetMaxProgramRev` / `SaveWaferData` / `SummaryData`
  - `TestDataManager`: `SaveData` / `SetData` / `GetDecimalValue` / `CreateNewTable` / `CheckColumns`
  - `TD_TABLE`: `CreateNewTable` / `CreateTDTable` / `AppendColumn`
  - `TestDataMaint.DeleteWaferData`
  - 쿼리 `GET_WAFER_SUMMARY_INFO [WSEQ]` / `MERGE_DATA`(PRODUCT / PROGRAM / WAFER) / `SELECT_GECBIN` / `GET_RAW_DATA` / `MERGE_BIN` / `CREATE_BIN` / `SELECT_MAX_PROGRAM_REV`
  - `iGdsEtResult` 메서드 목록 / `DACrux.SEMDMS.BSL.csproj` · `DACrux.TEST.BSL.csproj` 참조 / ServerBin 설정 연결 키
- DB 확인 (로컬, 조회만):
  - 테이블스페이스 `TS_TEST_TBL` / `TS_TEST_IDX` ONLINE
  - `TQP_MAPDEF` FACTORY 전부 SCK (NETDIE 조인 가능)
  - `TQP_PRODUCT` 에 OMM 없음
  - `TQP_BINDESC` 표본 (FACTORY 컬럼 없음, PROGRAM 키)
- 미확인:
  - 실제 실행 전부 (구현 전)
  - (14절 작성 때 확인함) 시간 문자열 형식은 `yyyy-MM-dd HH:mm:ss` 이다(`Handler.TO_DATE_FORMAT`, `TQP_WAFER` / `TQP_LOT` MERGE 의 `TO_DATE(..., 'YYYY-MM-DD HH24:MI:SS')`).
  - (14절 작성 때 확인함) Bin 등록은 TEST DSL `TQP_BINDESC.ExistsBinInfo` / `InsertBinInfo` 가 있다.
  - (14절 작성 때 확인함) Bin 색상은 HTML 형식(`ColorTranslator.FromHtml`, `TPUCMapView.cs:2264`)이다.

---

## 14. 확정 설계 (2026-10-14 사용자 "권장안으로 설계")

### 14.1 확정한 결정

| 번호 | 결정 |
| --- | --- |
| D1 | 저장 위치: **E-Test 테이블(원본) + YMS 테이블(YMS 화면용 사본)** |
| D2 | YMS 반영 시점: **업로드 저장 때 항상** (선택 체크박스 없음) |
| D3 | Map 없는 파일: **YMS 반영 보류(`WAIT`)**. Map 등록 후 [Map 좌표 다시 계산] 때 자동 반영 |
| D4 | TESTAREA: **공정명 TSVET / COWET / FT** |
| D5 | PROGRAM: **Device_공정** (`OMM_TSVET`) |
| D6 | BIN: **1 Pass / 2 Spec Fail / 3 미측정만 Fail**. Program 에 Bin 정의가 이미 있으면 덮어쓰지 않음 | → **2026-10-14 변경 (18절): YMS BIN = 결과 파일 Hardware Bin, E-Test Spec 판정과 별개**
| D7 | YMS 컬럼 이름: **4.3 규칙으로 만들어 `TQP_GDS_ET_ITEM.YMS_PARAM_NAME` 에 저장** |
| D8 | 같은 Wafer + 공정 재업로드: **YMS 도 교체** (재측정 이력 없음) |
| D9 | YMS 실패: **E-Test 저장 유지 + `YMS_STATUS = FAIL` + [YMS 다시 반영]** |

같이 정한 것 ([화면 연계 검토](2026-10-14_ETest_YMS_화면연계_검토.md) 7절 권장안):
- Y6 메뉴: 기존 TEST Analysis 메뉴를 표시한다(9절).
- Y7 Map: `OMM` Map 을 File Map Setup 으로 등록한다. 등록은 사용자가 한다.
- Y8 리뷰 데이터: 고객에게 1 Lot 전체 E-Test 파일을 요청한다. 요청은 사용자가 한다.

### 14.2 Para Setup 설계(CLAUDE-086)와의 관계

[E-Test Para Setup 설계](2026-10-14_ETest_Para_Setup_설계.md)의 결정 P1 ~ P8 은 아직 대기 중이다. YMS 적재는 어느 쪽으로 정해져도 바꾸지 않도록 아래처럼 **E-Test 쪽 저장 결과를 그대로 따라간다**.

| 항목 | 규칙 | P1 / P2 / P7 채택 시 | 채택 안 할 때 |
| --- | --- | --- | --- |
| YMS Parameter 컬럼 | 그 파일이 `TQP_GDS_ET_VALUE` 에 저장한 Parameter 전부 | 등록 Parameter(사용 Y) | Spec 정의 Parameter |
| Spec 없는 Parameter | 값은 넣고 LSL / USL 은 NULL. BIN 계산에서 빠짐 (값 판정 NULL) | 생길 수 있음 | 생기지 않음 |
| `YMS_PARAM_NAME` UQ | `TQP_GDS_ET_ITEM` 이름 UQ 와 같은 범위 | `(FACTORY, DEVICE_ID, TEST_PROGRAM, YMS_PARAM_NAME)` | `(FACTORY, DEVICE_ID, YMS_PARAM_NAME)` |
| 이름을 만드는 때 | `TQP_GDS_ET_ITEM` 행을 만들 때 한 번 | `RegisterEtParameters` | `MergeEtItem` |
| "YMS 이름" 열 위치 | Parameter 를 등록 / 정의하는 화면 | Para Setup [등록 Parameter] 탭 | Spec 설정 화면 |
| PARASPEC `PARAM_DESC` | 헤더 원문 | `HEADER_TEXT` | `ITEM_NAME` |

- DDL 은 **Para Setup -> Upload 재설계 -> YMS** 순서로 같은 날 적용한다(Para Setup 설계 4.6절과 같음).
- `TEST_PROGRAM` 이 생긴 뒤에 YMS UQ 를 만든다.

### 14.3 규칙 클래스 `EtYmsRule` (신규, `02_DMS/DACrux.SEMDMS.Interface/ETest/EtYmsRule.cs`)

화면 미리보기 / 서버 적재 / Parameter 등록이 같은 클래스를 쓴다. System 만 쓰고 DB 조회는 없다(`DieCoordinateMapper` 와 같은 방식).

| 멤버 | 내용 |
| --- | --- |
| `const int MaxParamNameLength = 50` | (구현 때 정정, 처음 128) `TQP_PARASPEC.PARAM_NAME` VARCHAR2(50). 고객 DB 가 12.1 이하이면 30 으로 바꾼다 (12절 확인 후) |
| `string ToProgramName(string deviceId, string testProgram)` | `{DEVICE}_{공정}` 대문자, `[A-Z0-9_]` 밖은 `_` |
| `string ToTestArea(string testProgram)` | 공정 대문자 (TSVET / COWET / FT) |
| `string ToParamName(string itemName)` | 4.3 규칙 1 ~ 5. 같은 입력이면 항상 같은 결과 |
| `string[] FindNameProblems(IEnumerable<string> itemNames)` | 4.3 규칙 6 (길이) / 7 (겹침) 위반 목록. 비면 통과 |
| `int ToBin(string dieJudge, int rangeFailCount, int notTestedFailCount)` | P -> 1 / F 이고 rangeFail > 0 -> 2 / 그 밖의 F -> 3 |
| `const int BinPass = 1, BinSpecFail = 2, BinNotTested = 3` + 이름 / 색 | `PASS` `#00C000` / `SPEC_FAIL` `#FF0000` / `NOT_TESTED` `#FFA500` |
| `static readonly string[] ReservedNames` | X / Y / BIN / WAFER_SEQ / DIE_NUM / DIEPROBE_CNT |
| `string ToYmsValue(object measValue, string valueStatus)` | V -> InvariantCulture `"R"` 문자열 / N · E -> `""` (NULL 저장) |
| `double ExpectedYield(int good, int dieCount, int? netDie)` | YMS `SummaryData` 와 같은 분모 규칙 (14.8), 소수 2자리 |

BIN 의 `rangeFailCount` / `notTestedFailCount` 정의:
- `rangeFailCount` = 그 Die 의 값 중 `JUDGE_FLAG = 'F' AND VALUE_STATUS = 'V'` 수 (범위 밖)
- `notTestedFailCount` = `JUDGE_FLAG = 'F' AND VALUE_STATUS = 'N'` 수 ('-')
- Die 판정 규칙(재설계 9.2)은 그대로다. BIN 은 그 결과를 나눠 적는 것뿐이다.

### 14.4 데이터 (DDL `Persistence/2026-10-14_tqp_gds_et_yms.sql`)

```sql
-- 1. 파일 헤더: YMS 반영 상태
ALTER TABLE DMSMGR.TQP_GDS_SRC_FILE ADD (
    YMS_STATUS     VARCHAR2(10)
        CONSTRAINT CK_GDS_SRC_FILE_YMS CHECK (YMS_STATUS IN ('DONE', 'WAIT', 'FAIL')),
    YMS_PROGRAM    VARCHAR2(50 CHAR),
    YMS_LOT_SEQ    NUMBER,
    YMS_WAFER_SEQ  NUMBER,
    YMS_TIME       DATE,
    YMS_MESSAGE    VARCHAR2(500 CHAR)
);
-- 2. Parameter: YMS 컬럼 이름 (UQ 범위는 14.2)
ALTER TABLE DMSMGR.TQP_GDS_ET_ITEM ADD (YMS_PARAM_NAME VARCHAR2(50 CHAR));   -- 구현 때 128 -> 50 정정
ALTER TABLE DMSMGR.TQP_GDS_ET_ITEM ADD CONSTRAINT UQ_GDS_ET_ITEM_YMS
    UNIQUE (FACTORY, DEVICE_ID, /* P2 채택 시 TEST_PROGRAM, */ YMS_PARAM_NAME);
```

- NULL 인 `YMS_STATUS` 는 "반영 안 함"을 뜻한다(이 기능 이전 파일).
- 기존 로컬 E-Test 데이터는 재설계 S5 에 따라 지우고 다시 올린다. 그래서 기존 행의 이름을 채우는 UPDATE 는 두지 않는다.
- YMS 테이블(`TQP_LOT` 등)의 구조는 바꾸지 않는다.

### 14.5 쿼리 (E-Test XML 에 추가만, 기존 YMS XML 수정은 9절의 `SELECT_TESTAREA` 1개뿐)

| XML | 쿼리 | 내용 |
| --- | --- | --- |
| `TQP_GDS_SRC_FILE.xml` | `UPDATE_SRC_FILE_YMS` | SRC_FILE_SEQ 로 YMS_STATUS / PROGRAM / LOT_SEQ / WAFER_SEQ / TIME(SYSDATE) / MESSAGE / UPDATE_USER |
| `TQP_GDS_SRC_FILE.xml` | `SELECT_SRC_FILE_YMS_TARGET` | 재판정 / 재계산 대상 파일 (Device [+ 공정], READY) + YMS 상태 |
| `TQP_GDS_ET_DIE.xml` | `SELECT_ET_DIE_FOR_YMS` | DUT_NO / MAP_X / MAP_Y / JUDGE_FLAG / rangeFail / notTestedFail (값과 조인해 집계), `MAP_X IS NOT NULL`, **ORDER BY MAP_Y DESC, MAP_X** |
| `TQP_GDS_ET_VALUE.xml` | `SELECT_ET_VALUE_FOR_YMS` | DUT_NO / YMS_PARAM_NAME / MEAS_VALUE / VALUE_STATUS (항목과 조인). C# 에서 Die x Parameter 로 펼침 |
| 신규 `TQP_GDS_ET_YMS.xml` | `UPDATE_YMS_PARASPEC` | `TQP_PARASPEC` 을 FACTORY + PROGRAM + PROGRAM_REV + PARAM_NAME 으로 PARAM_DESC / UOM / LSL / USL / PARAM_TYPE 갱신 (ExecuteMultiple) |
| 신규 `TQP_GDS_ET_YMS.xml` | `SELECT_ET_YMS_SPEC` | 그 파일 Parameter 의 YMS 이름 + 원문 + 단위 + Spec Lower / Upper (Spec 없으면 NULL) |
| `TQP_GDS_ET_ITEM.xml` | 기존 INSERT / MERGE | `YMS_PARAM_NAME` 추가 (P1 채택 시 `INSERT_ET_PARA`) |

**행 순서.** ORDER BY `MAP_Y DESC, MAP_X` 는 시스템 좌표(LeftBottom) 기준 "위 행부터, 왼쪽부터" 순서다. 이것이 고객 통합 Die# 순서(`DieNumberRule`, LeftTop 행 우선)와 같다. 그래서 YMS `DIE_NUM` 이 Die# 순서로 붙는다. Die# 값은 넣지 않는다(R9).

### 14.6 서비스 (`iGdsEtResult`, 새 wellknown 없음)

| 메서드 | 변경 | 계약 |
| --- | --- | --- |
| `DataTable PublishEtFileToYms(string strSrcFileSeq, string strUpdateUser)` | 신규 | 반환 1행: SRC_FILE_SEQ / YMS_STATUS / YMS_PROGRAM / YMS_TESTAREA / YMS_LOT_SEQ / YMS_WAFER_SEQ / DIE_COUNT / SKIP_DIE_COUNT / PARAM_COUNT / BIN1_COUNT / BIN2_COUNT / BIN3_COUNT / YIELD / YMS_MESSAGE. **예외를 던지지 않고** 실패도 이 행으로 돌려준다(FAIL + 사유) |
| `int DeleteEtFile(...)` | 내부 변경 | YMS_WAFER_SEQ 가 있으면 먼저 `TestDataMaint.DeleteWaferData`. **YMS 삭제가 실패하면 E-Test 삭제도 하지 않고 오류** (YMS 에 고아 데이터를 남기지 않음) |
| `RejudgeEtFiles` / `RemapEtFiles` (재설계 9.4) | 내부 변경 | 처리한 파일마다 `PublishEtFileToYms`. 반환 열에 `YMS_FAIL_COUNT` 추가 |
| `GetEtFileList` / `GetEtFile` | 열 추가 | YMS_STATUS / YMS_PROGRAM / YMS_WAFER_SEQ / YMS_TIME / YMS_MESSAGE + 수율(`TQP_WAFER_SUM.YIELD`, YMS_WAFER_SEQ 로 LEFT JOIN) |

프로젝트 참조: `DACrux.SEMDMS.BSL.csproj` 에 `DACrux.TEST.BSL` ProjectReference 1행.
- 배포 영향: SEMDMS Service 가 `101_ServerBin` 의 `DACrux.TEST.BSL.dll` 을 함께 읽는다. 두 서비스가 같은 폴더를 쓰므로 파일 복사 추가는 없다.
- TEST BSL 을 바꾸면 SEMDMS Service 도 다시 시작해야 한다.

### 14.7 `PublishEtFileToYms` 처리 (확정)

```text
try
  0. 파일 헤더 읽기. LOAD_STATUS != READY -> FAIL "READY 아님"
  1. MAP_ID 가 NULL -> UPDATE_SRC_FILE_YMS(WAIT, "Map 없음 - Map 등록 후 [Map 좌표 다시 계산]") 반환
  2. YMS_WAFER_SEQ 가 있으면 TestDataMaint.DeleteWaferData(이전 값) -> SRC_FILE 의 YMS_* 키 비움
  3. program = EtYmsRule.ToProgramName / testArea = ToTestArea
     TestCommon.InsertProductInfo(factory, DEVICE_ID, MAP_ID)
     TestCommon.InsertProgramInfo(factory, program, testArea, DEVICE_ID)
     rev = TestCommon.GetMaxProgramRev(factory, program)
  4. Die = SELECT_ET_DIE_FOR_YMS. 0개 -> FAIL "Map 안 Die 없음"
     TestCommon.SaveWaferData(31개 인자판):
       TEST_AREA testArea / DEVICE DEVICE_ID / DEVICE_ALIAS MAP_ID / LOT_ID / WAFER_ID
       START = END = TEST_TIME ("yyyy-MM-dd HH:mm:ss") / WAFER_DIAM = Map WAFER_SIZE/1000 (없으면 300)
       PROGRAM program / PROGRAM_REV rev / START_QTY -1 / MAX_X·MAX_Y = Die 최대
       WAF_FLAT "0" / COMMENTS LOT_WAFER_NO / DATA_SOURCE FILE_NAME / NUM_DIE Die 수
       나머지 NULL / bIncreaseProbeCnt = true
     -> UPDATE_SRC_FILE_YMS(키만: PROGRAM / LOT_SEQ / WAFER_SEQ, 상태는 그대로)   ← 이후 단계가 실패해도 다음에 2번이 정리
  5. rawDt: 열 X, Y, BIN, (YMS 이름들) / 행 = 4번 Die 순서
     X·Y = MAP_X·MAP_Y, BIN = EtYmsRule.ToBin(...), 값 = EtYmsRule.ToYmsValue(...)
  6. TestDataManager.SaveData(factory, program, rawDt, waferSeq)
  7. UPDATE_YMS_PARASPEC (SELECT_ET_YMS_SPEC 결과 + X/Y/BIN = INT)
  8. Bin 1 / 2 / 3: TQP_BINDESC.ExistsBinInfo == 0 이면 InsertBinInfo (HIGH_GEC 1 만 Y)
  9. TestCommon.SummaryData(factory, lotSeq, waferSeq, WAFER_ID)
 10. UPDATE_SRC_FILE_YMS(DONE, MESSAGE NULL) / 반환 행(수율 = TQP_WAFER_SUM.YIELD)
catch (Exception ex)
  UPDATE_SRC_FILE_YMS(FAIL, 단계 이름 + ex.Message 앞 500자) / 반환 행
```

- 4번에서 키를 바로 기록한다. 그래서 5 ~ 9번에서 실패해도 [YMS 다시 반영] 의 2번이 반쯤 들어간 YMS 데이터를 지운다.
- 6번이 처음 만나는 Program 이면 TD 테이블을 만든다(DDL). DMSMGR 계정에 CREATE TABLE 권한이 있어야 한다. 기존 YMS 적재와 같은 계정이다.

### 14.8 업로드 화면 (8절 확정, 재설계 9.6 과 함께 다시 작성)

| 영역 | 확정 내용 |
| --- | --- |
| 대기 목록 열 `YMS` | 분석 후: `반영 예정 (OMM_TSVET)` / `Map 없음 - 보류` / `이름 오류 n` (오류는 저장 대상 제외). 저장 후: `반영됨 (수율 95.8%)` / `보류` / `실패: 사유` |
| 기준 정보 YMS 칸 | Program / TestArea / Lot / Wafer / Map / YMS Die (Map 밖 n 제외) / Bin 1·2·3 / 예상 수율 |
| 탭 [YMS 항목] | 헤더 원문 / YMS 컬럼 이름 / LSL / USL / 단위 / 검사(길이 초과 · 이름 겹침 빨간색) |
| 저장 진행 | 파일마다 `E-Test 저장` -> `YMS 반영` 두 줄. YMS 실패는 그 파일만 표시하고 다음 파일 계속 |
| 탭 [등록 파일] | 열 YMS 상태 / YMS Wafer / 수율 / 반영 시각, **[YMS 다시 반영]** (고른 파일, `FAIL` · `WAIT` · `DONE` 모두 가능). 삭제 확인 문구: "E-Test 와 YMS 데이터를 모두 지웁니다" |

- 예상 수율은 YMS `SummaryData` 와 같은 규칙으로 계산한다(`TestCommon.cs:1302-1316`).
  - Map NETDIE 와 YMS Die 수가 1.5배 안쪽이면 Bin 1 수 / NETDIE, 아니면 Bin 1 수 / YMS Die 수.
  - 예: Hybrid 24 Die 는 측정 Die 수로 나누고, Full 76 Die 는 NETDIE 76 으로 나눈다. 60 / 76 처럼 일부만 측정한 Wafer 는 NETDIE 로 나눈다.
  - 이 계산은 `EtYmsRule.ExpectedYield(int good, int dieCount, int? netDie)` 에 둔다.

### 14.9 검증 기대값 (Probe `GdsEtResultProbe.ps1` 에 YMS 항목 추가 / 업로드 Smoke)

Probe 는 실제 데이터와 섞이지 않게 Factory `PROBE` + Device `PRBOMM`(Program `PRBOMM_TSVET`)을 쓴다. 끝나면 전부 지운다.
- 지우는 범위: YMS Wafer 삭제 -> 빈 TD 테이블 정리(기존 `CheckTableEmpty`) -> Probe Program 의 BINDESC / PROGRAM / PRODUCT / PARASPEC / DATA_TABLES 행.
- 2026-10-07 Smoke 가 사용자 데이터를 지운 사고 후 정한 보호 규칙을 따른다.

| 검사 | 기대값 (TSVET 샘플 + Spec TSVET 37, Probe Map 76 Die) |
| --- | --- |
| 규칙 단위 | `Resistance_iforce=8.0mA@C4DC_28_37_FH` -> `RESISTANCE_IFORCE_8P0MA_C4DC_28_37_FH`<br>`Leakage_Serial@A_FH:B_FL` -> `LEAKAGE_SERIAL_A_FH_B_FL`<br>숫자 시작 -> `P_`, `BIN` -> `P_BIN`<br>129자 -> 길이 오류, 겹침 -> 오류 |
| 반영 | `TQP_LOT` 1행 (TEST_AREA TSVET / PROGRAM PRBOMM_TSVET / DEVICE_ALIAS = Map)<br>`TQP_WAFER` 1행 (PROBE_CNT 0 / NUM_DIE 24 / DATA_SOURCE 파일명) |
| TD | `TD_PRBOMM_TSVET` 24행 / Parameter 컬럼 37 / BIN 1 = 23, 2 = 1 (DUT #44), 3 = 0<br>MIN(값) > -1E20 (MinValue 없음)<br>DIE_NUM 1 = Die# 가 가장 작은 Die |
| PARASPEC | 40행 (X / Y / BIN INT + 37 FLOAT), LSL / USL = Spec, PARAM_DESC = 원문 |
| BINDESC | 3행. 두 번째 반영 때 COLOR 를 바꿔 둔 값이 유지됨 |
| 요약 | `TQP_WAFER_SUM` TESTED_DIE 24 / GEC 23 / YIELD 95.83 / NETDIE 76 |
| 다시 반영 | 행 수 같음, PROBE_CNT 0 행 1개, WAFER_SEQ 새 값 |
| 실패 복구 | 6번 직후 일부러 예외 -> FAIL + 키 기록 -> 다시 반영 -> DONE, 남은 행 없음 |
| Map 없음 | WAIT, YMS 행 0 -> Map 등록 + `RemapEtFiles` -> DONE |
| 재판정 | Spec Upper 를 바꿔 `RejudgeEtFiles` -> BIN / 수율 / USL 변경 |
| 삭제 | `DeleteEtFile` -> YMS Wafer / 요약 / TD 행 0, Lot 없어짐 |
| 업로드 Smoke | 대기 목록 YMS 열, [YMS 항목] 탭, 저장 후 "반영됨 (수율 95.8%)", [YMS 다시 반영] |
| DMS (사용자) | TEST Map View / Gallery / Cum / Bin Stack / Yield / Param Analysis / Standard / Test Report 에서 `OMM_TSVET` 조회 (Map 원점 확인 포함) |

### 14.10 구현 순서 (Para Setup / Upload 재설계와 합침)

| 단계 | 내용 |
| --- | --- |
| 1 | DDL 같은 날 순서대로: Para Setup(결정 시) -> Upload 재설계 9.3 -> 14.4 |
| 2 | `EtYmsRule` + 규칙 단위 Probe |
| 3 | 재설계 9.9 의 1단계 서비스(판정 / Map 좌표) + 14.5 쿼리 + 14.6 서비스 + BSL 참조 + Probe 14.9 |
| 4 | 업로드 화면(재설계 9.6 + 14.8) + Smoke |
| 5 | Spec / Para Setup 화면 YMS 이름 열 + 재판정 연결 |
| 6 | 9절 메뉴 / RESV_03 / `SELECT_TESTAREA` + 사용자 빌드 / 배포 / DMS 화면 확인 |

### 14.11 사용자 / 고객 조치

| 항목 | 담당 |
| --- | --- |
| `OMM` Map 을 File Map Setup 으로 등록 (Y7) | 사용자 |
| 고객에게 1 Lot 전체 E-Test 파일 요청 (Y8) | 사용자 |
| 고객 DB 버전 / 테이블스페이스(`TS_TEST_TBL` / `TS_TEST_IDX`) 확인 | 사용자 (서버 설치 때) |
| Para Setup P1 ~ P8 결정 | 사용자 (14.2 는 어느 쪽이든 맞게 작성) |

## 15. 구현 결과 1차 (2026-10-14, CLAUDE-087)

사용자 "구현시작". 이번 범위는 Para Setup 결정(CLAUDE-086) / Upload 재설계 구현(CLAUDE-029)과 **무관한 부분**이다: 규칙 클래스 / YMS 적재 핵심 / DDL 파일.
- `GdsEtResult.PublishEtFileToYms` 연결 / 업로드 화면 / 메뉴는 아직 하지 않았다.
- 연결하려면 E-Test Die 판정 / Map 좌표 컬럼(재설계 9.3)이 필요하다.

### 15.1 파일

| 구분 | 파일 | 내용 |
| --- | --- | --- |
| 규칙 | 신규 `02_DMS/DACrux.SEMDMS.Interface/ETest/EtYmsRule.cs` + Interface csproj Compile 1행 | 14.3 멤버 전부 + `NormalizeParameterName` / `IsValidParamName` / `BinName` / `BinColor` / `BinDescription` |
| 적재 | 신규 `02_DMS/DACrux.SEMDMS.BSL/EtYmsPublisher.cs` + BSL csproj Compile 1행 + **`DACrux.TEST.BSL` ProjectReference 1개** | 입력 모델 `EtYmsWafer` / `EtYmsParameter` / `EtYmsDie`, 결과 `EtYmsPublishResult`(`ToDataTable` = 14.6 반환 열), `EtYmsPublisher.Publish` / `DeleteWafer` / `GetWafer` / `GetWaferSummary` |
| 쿼리 | 신규 `08_DBQuery/ORACLE/TQP_GDS_ET_YMS.xml` + DSL `02_DMS/DACrux.SEMDMS.DSL/TQP_GDS_ET_YMS.cs` + DSL csproj 1행 | 1차 7개 -> **2차(15.5)에서 `UPDATE_YMS_PARASPEC` 1개만** 남김 |
| DDL | 신규 `06_CoreLibSource/GdsMapControl/Persistence/2026-10-14_tqp_gds_et_yms.sql` | **적용 안 함**. 14.4 내용 + ET_ITEM UQ 2안(P2 채택 / 미채택) |
| Probe | 신규 `06_CoreLibSource/GdsMapControl/Probes/EtYmsRuleProbe.ps1` / `EtYmsPublishProbe.ps1` | 15.3 |

- 업로드 화면이나 E-Test 테이블에 의존하지 않도록 적재 핵심은 **입력 모델**을 받는다.
- 나중에 `GdsEtResult.PublishEtFileToYms` 는 E-Test 테이블(헤더 / Die 판정·Map 좌표 / 값 / Spec / YMS 이름)을 읽어 모델을 채우고, 결과를 `YMS_*` 컬럼에 기록하기만 하면 된다.

### 15.2 설계와 달라진 점 (구현하며 확인한 사실)

| 항목 | 설계 | 구현 | 이유 |
| --- | --- | --- | --- |
| YMS 컬럼 이름 한도 | 128 (DB 식별자) | **50** | `TQP_PARASPEC.PARAM_NAME` VARCHAR2(50), 분석 화면은 PARAM_NAME = 컬럼 이름으로 조인 (로컬 DB 컬럼 길이 확인) |
| 50자 초과 이름 | 오류 | **약어 -> 해시** | 1단계 LEAKAGE→LKG / RESISTANCE→RES / CONNECTIVITY→CONN / CONTINUITY→CONT, 2단계 PARALLEL→PAR / SERIAL→SER / IFORCE→IF, 그래도 넘으면 41자 + `_` + FNV-1a 8자리 |
| 다른 YMS 길이 | - | 확인 추가 | LOT_ID 20 / WAFER_ID 30 / DEVICE·DEVICE_ALIAS 30 / TEST_AREA 30 / Program 45 (TD_ + Program + 분할 접미어가 TABLE_NAME 50 안) |
| 이전 반영 / 삭제 | 기존 `TestDataMaint.DeleteWaferData` 재사용 | **같은 순서를 `DeleteWafer` 가 직접 수행, Lot 요약만 다르게** | 기존 함수는 Lot 의 마지막 Wafer 를 지울 때 `INSERT_LOT_SUMMARY`(GROUP BY 없는 집계)가 빈 행을 넣다가 **ORA-01400 (TQP_LOT_SUM.CUSTOMER NULL)** 으로 멈추고 Lot 행이 남는다 (Probe 실행 중 발견). E-Test 는 Lot 에 Wafer 1장인 경우가 많다. `DeleteWafer` 는 Wafer 가 남으면 요약을 다시 만들고, 없으면 Lot 을 지운다 |
| 쿼리 | 14.5 의 E-Test 조회 쿼리 4개 포함 | 이번에는 YMS 쪽 쿼리만 | E-Test 쪽 조회(`SELECT_ET_DIE_FOR_YMS` 등)는 재설계 9.3 컬럼이 생긴 뒤 `GdsEtResult` 연결 때 추가 |

참고: 기존 `TestDataMaint.DeleteWaferData` 버그는 YMS TEST Data Maint 화면에서 Lot 의 마지막 Wafer 를 지울 때도 같다. 기존 함수 수정은 이번 범위가 아니므로 고치지 않았다(결정 필요).

### 15.3 검증

**빌드 확인**
- TEMP `%TEMP%\chippac\CLAUDE-087`: Interface / DSL / BSL(+ TEST.BSL / TEST.DSL / TEST.Interface 함께) 오류 0.
- BSL Rebuild 경고 6건은 모두 기존 파일이다(`DACrux.Data.Parser` 3 / `DACrux.Utility` 2 / `TEST.BSL VisualInspection` 1). 새 파일 경고는 0.
- `TEST.Interface` 직접 참조 없이 컴파일된다.

**Probe 확인**
- `EtYmsRuleProbe.ps1 -BinDir <TEMP>` -> **`ET_YMS_RULE_PROBE=PASS (24/24)`**. DB 를 쓰지 않는다.
  - 이름 예 9개 / 줄인 이름 50자 / 해시 이름(같은 입력 같은 값) / 이름 검사 / 겹침 검사
  - Program / Bin / 값('-' -> '') / 예상 수율 5경우
  - 고객 Spec TSVET 37: 문제 0, 최대 50자, 줄인 이름 1
  - 샘플 파일 측정 헤더 2,755: 문제 0, 최대 50자, 서로 다른 이름 2,755
- `EtYmsPublishProbe.ps1 -BinDir <TEMP>` -> **`ET_YMS_PUBLISH_PROBE=PASS (21/21)`**. 로컬 docker `oracle19c-DMS`, Factory PROBE / Device PRBOMM / Map OMM_Hybrid.
  - 반영: DONE 433 ms / Die 24 / Param 37 / Bin 23-1-0 / 콜백 WAFER_SEQ = 결과
  - TQP_LOT·TQP_WAFER: TEST_AREA TSVET / DEVICE PRBOMM / DEVICE_ALIAS OMM_Hybrid / PROBE_CNT 0 / NUM_DIE 24 / COMMENTS / DATA_SOURCE / PROGRAM_REV 0
  - `TD_PRBOMM_TSVET`: 24행 43열 / BIN1 23 BIN2 1 / 최소값 -0.746 (MinValue 없음) / 값 일치 719
    - DIE_NUM 1 = (4, 8) = Die#6 = 최소 Die#
    - DUT #20 (9,2) BIN 1 / DUT #44 (6,6) BIN 2
  - TQP_PARASPEC: 40행 (INT 3 / FLOAT 37), RES USL 715 · LSL NULL · Ohm · 원문 설명, 줄인 LKG 이름 LSL -50 USL 50
  - TQP_BINDESC: 3행 (1 PASS Y #00C000 / 2 / 3)
  - TQP_WAFER_SUM: TESTED 24 / GEC 23 / YIELD 95.83. Factory PROBE 라 Map(SCK) NETDIE 가 조인되지 않아 측정 Die 수로 나눔.
  - 다시 반영: WAFER_SEQ 새 값, 이전 Wafer 0, Program Wafer 1 (PROBE_CNT 0), TD 24 / Bin Define 에서 바꾼 BIN2 색 유지
  - Map 밖 Die 1: DONE / Die 23 / Skip 1 / TD 23
  - 입력 오류(이름 `1BAD`) / Map ID 없음: FAIL "입력 확인", 이전 반영 그대로
  - 삭제: True, 다시 삭제 False, TD 테이블 / PARASPEC / DATA_TABLES 정리
  - 끝 정리 후 PROBE 행 0 (LOT / LOT_SUM / WAFER / WAFER_SUM / PARASPEC / DATA_TABLES / BINDESC / PROGRAM / PRODUCT / TD 테이블). 기존 데이터 수(TQP_DATA_TABLES 4 / TQP_LOT 103)는 시작 전과 같음.
- Probe 를 만들며 첫 실행에서 실패한 것 2가지는 모두 스크립트 쪽 문제였다.
  - PowerShell 변수 대소문자 충돌(`$R` / `$r`)
  - DataTable 반환이 행으로 풀린 것
- 세 번째 실행에서 위 삭제 버그를 발견했다. 그때 남은 PROBE Lot 1행은 다음 실행의 시작 정리에서 지웠다.

**미확인**
- Service Remoting 경유 호출
- DMS YMS 화면 표시 (메뉴 숨김 / `GdsEtResult` 연결 전)
- 배포 (운영 폴더 변경 없음)
- DDL 적용

### 15.4 다음 단계

| 순서 | 내용 | 막힌 이유 |
| --- | --- | --- |
| 1 | Para Setup P1 ~ P8 결정 | 사용자 |
| 2 | DDL 적용 (Para -> Upload 재설계 -> 이 파일, 같은 날) | 1 |
| 3 | 재설계 9.9 1단계 (Die 판정 / Map 좌표 서비스) | 2 |
| 4 | `GdsEtResult.PublishEtFileToYms` (14.6 / 14.7: E-Test 조회 쿼리 4개 + 모델 채우기 + `YMS_*` 기록) / `DeleteEtFile` · 재판정 · 재계산 연결 + Probe | 3 |
| 5 | 업로드 화면 YMS 칸 / 탭 / 버튼 + 메뉴 VISIBLE / RESV_03 / `SELECT_TESTAREA` | 4 |
| - | ~~(선택) 기존 `TestDataMaint.DeleteWaferData` 빈 Lot 버그 수정~~ | 2차(15.5)에서 소스 수정 완료, 배포 대기 |

### 15.5 2차 정리: 기존 YMS 소스 최대 활용 (2026-10-14, 사용자 "진행")

사용자 질문 "기존 YMS 소스를 최대한 활용하고 있는가?" 에 대해 새로 만든 코드를 하나씩 기존 YMS 소스와 대조했다. 바꿀 수 있는 것은 모두 기존 것으로 바꿨다.

| 1차에 새로 만든 것 | 2차 | 근거 |
| --- | --- | --- |
| `SELECT_YMS_WAFER` | 기존 `TQP_WAFER.xml` **`GET_WAFER_INFO [WSQ]`** (`TQP_WAFER.GetData`) | 같은 조회 (WAFER_SEQ 1건) |
| `SELECT_YMS_WAFER_SUM` | 기존 `TQP_WAFER_SUM.xml` **`GET_WAFER_LOT_INFO`** | TEST Map View 의 Wafer 정보와 같은 조회 (TQP_WAFER + TQP_WAFER_SUM, YIELD / TESTED_DIE / GEC / NETDIE / BINn) |
| `SELECT_YMS_BINDESC` | 기존 `TQP_BINDESC.xml` **`SELECT_BIN_DESC`** (Probe) | Map 화면이 쓰는 Bin 조회 |
| `SELECT_YMS_WAFER_BY_PROGRAM` / `SELECT_YMS_EMPTY_LOT_BY_PROGRAM` / `DELETE_YMS_PRODUCT_UNUSED` | **제품 XML 에서 삭제**. Probe 는 기존 `SELECT_LOT_SEQ_01` / `SELECT_WAFER_BY_LOT` / `TQP_LOT.DeleteLot` / `TQP_LOT_SUM.DeleteLot` / `UPDATE_DELETE_FLAG` 사용 | Probe 정리에만 쓰던 쿼리. Product 는 기존 삭제 쿼리가 없어 숨김(DELETE_FLAG Y)만 한다 |
| `EtYmsPublisher.DeleteWafer` 의 삭제 순서 직접 구현 | **기존 `TestDataMaint.DeleteWaferData` 호출** + 기존 함수의 빈 Lot 버그 수정 | 아래 |
| `UPDATE_YMS_PARASPEC` | **유지** | 기존 `TQP_PARASPEC.UpdateProgramParaSpec` 은 Program 행 전체를 지우고 `CREATE_PARASPEC` 으로 다시 넣는데 LSL / USL 컬럼이 없다 |

기존 함수 수정 (`09_TEST/DACrux.TEST.BSL/TestDataMaint.cs` `DeleteWaferData`, +5 / -2줄):
- `lotSum.InsertLotSummary(lotSeq)` 를 Lot 에 Wafer 가 남아 있을 때만 부른다(`waf.GetWaferCount`).
- 빈 Lot 은 원래 있던 `DeleteLotSeqIfEmpty` 가 Lot 요약 / Lot 을 지운다.
- 같은 파일의 다른 `InsertLotSummary` 2곳은 Wafer 를 옮겨 넣은 새 Lot 이라 비지 않아서 그대로 두었다.
- YMS TEST Data Maint 화면도 이 함수를 쓰므로 같이 고쳐진다 (배포 후).

그대로 둔 새 코드와 이유:
- `EtYmsRule.ToParamName`: 기존 Fab2 PCM 파서 치환은 파서 내부 전용이고 `= @ :` 를 다루지 않는다.
- `EtYmsRule.ExpectedYield`: 저장 전 미리보기용이다. 실제 수율은 기존 `SummaryData` 가 계산한다.
- `DieCoordinateMapper`: 기존 YMS Map 보정(`TQP_MAPCFG`)은 YMS 파서 안에서만 적용되고, E-Test 좌표 규칙은 이미 결정했다(Die 좌표 설계 9절).

검증 (TEMP `%TEMP%\chippac\CLAUDE-087`, Rebuild):
- 빌드: 오류 0, 경고 6 (1차와 같은 기존 파일). CLAUDE-090 이 Interface csproj 에 추가한 `EtParameterNameRule.cs` 도 함께 컴파일됨.
- `ET_YMS_RULE_PROBE=PASS (24/24)`
- `ET_YMS_PUBLISH_PROBE=PASS (22/22)`. 1차 21 항목 + 기존 삭제 함수 두 경로:
  - 같은 Lot 2장 중 1장 삭제 -> Lot 요약 유지 (`GetIsLot` True)
  - 마지막 Wafer 삭제 -> Lot / Lot 요약 없음 (수정 전에는 ORA-01400)
- 끝난 뒤 PROBE 행 0. 기존 TQP_DATA_TABLES 4 / TQP_LOT 103 그대로. Product `PROBE / PRBOMM` 은 DELETE_FLAG Y 로 남는다.
- 미확인: 배포 (TEST.BSL / SEMDMS.BSL / DSL 운영 폴더 변경 없음), Service Remoting, YMS TEST Data Maint 화면에서 마지막 Wafer 삭제.

## 16. 구현 3차: 업로드 서비스 연결 (2026-10-14, CLAUDE-087)

사용자: "setup 은 내가 작업 하더라도 지금은 프로그램이 돌아 갈수 있게만 진행."
- 선행 조건이 다른 작업으로 풀렸다.
  - Para Setup 결정: 저장 대상 = 등록 Parameter.
  - Upload 재설계 구현: Die 판정 / Map 좌표 / DDL (CLAUDE-090).
  - Para 화면 재구성 (CLAUDE-095).
- Setup 데이터(OMM Map / Parameter / Spec)는 사용자가 만든다. 이번에는 업로드한 파일이 YMS 에 들어가 YMS 화면에서 보이는 데까지만 했다.

### 16.1 14절과 달라진 점

| 항목 | 14절 | 구현 | 이유 |
| --- | --- | --- | --- |
| YMS 컬럼 이름 저장 | `TQP_GDS_ET_ITEM.YMS_PARAM_NAME` 추가 | **추가 안 함** | CLAUDE-090 이 Parameter 저장 이름(`ITEM_NAME`)을 `EtYmsRule.ToParamName` 으로 만들어 이미 YMS 컬럼 이름과 같다. UQ(FACTORY, DEVICE_ID, TEST_PROGRAM, ITEM_NAME)도 이미 있다 |
| 반영 호출 | 화면이 `CompleteEtFile` 뒤 `PublishEtFileToYms` 를 따로 호출 (D9) | **서버 `CompleteEtFile` 끝에서 자동 호출** | 화면 수정 없이 동작. 업로드 화면은 CLAUDE-095 가 같은 날 고치고 있어 겹치지 않게 했다. YMS 실패는 예외 없이 결과 열(YMS_STATUS 등)로만 알려 확정은 그대로다 (D9 의 "E-Test 유지" 는 같음) |
| 화면 YMS 열 / [YMS 다시 반영] | 업로드 화면에 추가 | **이번에는 안 함** | "프로그램이 돌아갈 수 있게만". 다시 반영은 [등록 파일] 탭 [Map 좌표 다시 계산] 이 대신한다 (그 서비스가 YMS 도 다시 반영) |
| E-Test 읽기 쿼리 | E-Test XML 에 추가 | **`TQP_GDS_ET_YMS.xml` 에 둠** | 다른 작업이 고치는 E-Test XML 을 건드리지 않게 |

### 16.2 파일

| 구분 | 파일 | 내용 |
| --- | --- | --- |
| DDL | `06_CoreLibSource/GdsMapControl/Persistence/2026-10-14_tqp_gds_et_yms.sql` | `TQP_GDS_SRC_FILE` YMS 6컬럼만 (ET_ITEM 부분 삭제). **로컬 DB 적용** |
| 서버 | 신규 `02_DMS/DACrux.SEMDMS.BSL/GdsEtResult_Yms.cs` (partial) + BSL csproj Compile 1행 | `PublishEtFileToYms`(원격) / `PublishToYms`(본체, 예외 없음) / `BuildYmsWafer`(E-Test -> EtYmsWafer) / `DeleteYmsOfFile` / `AppendYmsResult` |
| 서버 | `GdsEtResult.cs` 호출 4곳 | `DeleteEtFile` 앞 YMS 삭제(실패하면 파일도 안 지움) / `CompleteEtFile` 끝 반영 + 결과 열 5개(YMS_STATUS / YMS_MESSAGE / YMS_PROGRAM / YMS_WAFER_SEQ / YMS_YIELD) / `RejudgeEtFiles` 파일마다 반영 / `RemapEtFiles` 파일마다 반영 + 결과 열 YMS_FAIL |
| 원격 | `Interface/iGdsEtResult.cs` / `RO/GdsEtResult.cs` | `PublishEtFileToYms` 1개 |
| 쿼리 | `TQP_GDS_ET_YMS.xml` + DSL `TQP_GDS_ET_YMS.cs`(GetData / UpdateDataNonQuery 추가) | `SELECT_ET_YMS_FILE` / `SELECT_ET_YMS_PARAM`(파일에 값 있는 등록 Parameter + 헤더 원문 / 단위 + 사용 Y Spec) / `SELECT_ET_YMS_DIE`(Map 좌표 / 판정 / 범위 밖·'-' Fail 수) / `SELECT_ET_YMS_VALUE` / `UPDATE_SRC_FILE_YMS_KEY` / `UPDATE_SRC_FILE_YMS` |
| 메뉴 | `01_WinApp/DACruxV5/MenuFile.mnu` + `100_ClientBin/MenuFile.mnu` (두 파일 같음) | VISIBLE True 7곳 (`MNU_TEST` / `MNU_TEST_CP` / `MNU_PCM_ANALYSIS` / `MNU_STANDARD_REPORT` / `MNU_CUM_REPORT` / `MNU_BIN_WAFER_REPORT` / `MNU_YIELD_REPORT`), RESV_03 에 `,TSVET,COWET,FT` 4곳 (`MNU_CUM_REPORT` / `MNU_BIN_WAFER_REPORT` / `MNU_YIELD_REPORT` / `MNU_TEST_REPORT`). 다른 값 / 행 순서 변경 없음 |
| 쿼리 | `TQP_PROGRAM.xml` `SELECT_TESTAREA` | TSVET / COWET / FT 3줄 추가 (Standard Report TestArea 목록) |
| Probe | 신규 `06_CoreLibSource/GdsMapControl/Probes/EtYmsServiceProbe.ps1` | 16.3 |

메뉴 권한: 화면을 열 때 `UXUtil.AccessCheck` 가 검사한다. ADMIN / 1111(개발 자동 로그인)은 통과한다. 다른 사용자는 기존 POC 메뉴처럼 기능 코드 권한 등록이 필요하다(이번에 안 함).

### 16.3 검증 (TEMP `%TEMP%\chippac\CLAUDE-087`, 로컬 docker `oracle19c-DMS`)

- 빌드: BSL Rebuild(Interface / DSL / TEST.* 포함) + RO 오류 0, 경고 6 / 5 (모두 기존 파일).
- **`EtYmsServiceProbe.ps1 -BinDir <TEMP>` -> `ET_YMS_SERVICE_PROBE=PASS (9/9)`**
  - 실행 조건: Factory PROBE / Device PRBOMM, 화면과 같은 파서(ClientBin ENGUI)로 업로드 서비스만 호출.
  - Map 없이 업로드 -> 확정 OK + YMS WAIT (YMS 데이터 없음)
  - 시험 Map(TQP_USEMAP 복사) + [Map 좌표 다시 계산] -> YMS DONE: TD 24 / Bin 23-1 / 수율 95.83 / TESTAREA TSVET / DEVICE_ALIAS PRBOMM (610 ms)
  - `PublishEtFileToYms` 다시 반영 -> 새 Wafer 1장, 이전 Wafer 없음
  - Spec 완화(715 -> 800) + 다시 판정 -> YMS 수율 100 / 되돌리면 95.83
  - 파일 삭제 -> YMS Wafer / Lot 삭제 (Lot 의 마지막 Wafer, 2차에서 고친 기존 함수 사용)
  - Map 있는 상태로 다시 업로드 -> 확정 결과에 바로 YMS DONE / 수율 95.83
  - Map 삭제 + 다시 계산 -> YMS WAIT + YMS 데이터 삭제
  - 끝 정리 0
- 회귀:
  - `EtYmsRuleProbe` 24/24 / `EtYmsPublishProbe` 22/22
  - CLAUDE-090 의 `GdsEtResultProbe.ps1 -BinDir <TEMP>` -> `ET_RESULT_PROBE=PASS (18/18)`. 이 Probe 의 좌표 다시 계산 단계에서 YMS 반영 / 정리도 지나간다.
  - 그 Probe 는 YMS 를 몰라서 `ET_PROBE_TSVET` Bin 3 / Program 1 / Product 1 행을 남긴다 -> 기존 DSL 쿼리로 정리함.
- 로컬 DB: PROBE 행 0, 기존 TQP_DATA_TABLES 4 / TQP_LOT 103 그대로.
- **운영 반영 (Claude 배포 아님)**: 14:33 사용자 Visual Studio 빌드가 이 소스로 운영 폴더에 나갔다.
  - 101_ServerBin: `DACrux.SEMDMS.BSL.dll` 33eb9b4e / `DSL` 3ad40683 / `Interface` 7253012f / `DACrux.TEST.BSL.dll` 1a7e85eb (2차 삭제 함수 수정 포함)
  - 100_ClientBin: `Interface` 7253012f / `RO` d60dd069
  - 14:34 DACrux / SEMDMS / TEST / Framework Service 가 다시 시작되어 지금 실행 중인 DMS 에 반영되어 있다.
  - DMS 화면 확인은 아직 하지 않았다.
- 미확인: DMS 화면에서 업로드 -> YMS 화면 조회 (사용자 Setup 후), Service Remoting 경유 호출 시간, 여러 Wafer / Lot.

### 16.4 사용자 확인 순서 (Setup 은 사용자)

1. POC > File Map Setup: 샘플 KLARF 를 읽고 **Map ID 를 `OMM`** 으로 바꿔 저장한다 (업로드 Map ID = 파일 Device).
2. E-Test Para 설정: OMM / TSVET Parameter 등록 + Spec.
3. E-Test 결과 업로드: 샘플 파일 저장.
   - 확정 결과에 YMS 가 붙는다(화면 표시 없음).
   - Map 을 나중에 만들었으면 [등록 파일] 탭 [Map 좌표 다시 계산] 을 누른다.
4. TEST Analysis > TEST Map View: Wafer 선택에서 기간을 2026-06-09 가 들어가게 하고 Program `OMM_TSVET` / TestArea `TSVET` 를 고른다.
   - Bin Map: 녹색 23 / 빨강 1.
   - Parameter 를 고르면 값 Map, Spec 체크면 범위 밖 빨강.
5. 같은 데이터를 Bin / Test Map Gallery / Wafer Cum Report / Wafer Bin Report / Yield Report / Param Analysis / Standard Report / Test Report 에서 확인한다.
   - Standard Report 의 TestArea 목록에 TSVET 이 보여야 한다.

### 16.5 남은 것

- 업로드 화면 YMS 열 / [YMS 다시 반영] 버튼 (필요하면).
- 기능 코드 권한 등록 (ADMIN 외 사용자).
- 고객 DB 적용 때 DDL 2개: Upload 재설계 DDL + 이 파일.
- `GdsEtResultProbe` 정리에 YMS 마스터 행 지우기 추가 (CLAUDE-090 파일이라 이번에는 안 고침).

### 16.6 TEST Analysis 메뉴 정리 (2026-10-14, CLAUDE-097)

요청: CP 로 나누지 말고 성격에 맞게 나열, 아이콘도 성격에 맞게.

| 순서 | 묶음 | 메뉴 (MENU_KEY) | 아이콘 |
| --- | --- | --- | --- |
| 0 | Wafer Map | TEST Map View (`MNU_TEST_MAP_VIEW`) | Wafer 1장 + Die 격자(Bin 색) |
| 1 | | Bin Map Gallery (`MNU_BIN_MAP_GALLERY`) | 작은 Wafer 4장(Bin 색) |
| 2 | | Test Map Gallery (`MNU_TEST_MAP_GALLERY`) | 작은 Wafer 4장(Parameter 값 색) |
| 3 | | Wafer Cum Report (`MNU_CUM_REPORT`) | 겹친 Wafer 3장 + 누적 Fail 색 |
| 4 | Bin / 수율 (구분선) | Wafer Bin Report (`MNU_BIN_WAFER_REPORT`, Bin Stack) | Bin 누적 막대 |
| 5 | | Yield Report (`MNU_YIELD_REPORT`) | 수율 추이 선 + % |
| 6 | Parameter (구분선) | Param Analysis (`MNU_PCM_ANALYSIS`) | 분포 막대 + 곡선 + LSL / USL |
| 7 | | Standard Report (`MNU_STANDARD_REPORT`) | 통계 표 문서 |
| 8 | | Test Report (`MNU_TEST_REPORT`, BoxPlot) | BoxPlot 3개 |

- 메뉴 파일 2곳(`01_WinApp/DACruxV5/MenuFile.mnu`, `100_ClientBin/MenuFile.mnu`, 같은 내용)의 TEST 블록만 바꿨다. 행 순서는 그대로다.
  - CP 하위 4개: `POPUP_MENU` -> `MNU_TEST`, `MNU_LEVEL` 3 -> 2.
  - `MNU_TEST` 하위: `MNU_ORDER` 0 ~ 14 다시 매김(숨김 메뉴 5개와 `MNU_TEST_CP` 는 뒤로).
  - 구분선: `IS_START` True = Wafer Bin Report / Param Analysis (`MenuHelper.InitMenu` 가 Button 에만 `IsFirstInGroup` 적용).
  - `MNU_TEST_CP`: VISIBLE False. 하위가 없어져 Button 으로 만들어지지만 숨김이다.
  - `MNU_YIELD_REPORT`: ENABLE False -> True (회색이라 열 수 없었음, 검토 문서 5 / 6번 화면), 캡션 / 툴팁 `Yeild` -> `Yield`.
  - ICON32 / ICON16: 9개 모두 새 PNG(32 / 16px). 그림 스크립트 `99_Resource/ByModule/POC/make_test_menu_icons.ps1`, PNG 와 미리보기 `99_Resource/ByModule/POC/TEST_Menu/`.
- 기능 코드(`FUNCTION_CODE`) / 화면 / `RESV_03` 은 바꾸지 않았다.
- 확인: DataSet 으로 두 파일을 읽어 107행, `MNU_TEST` 하위 순서 / 구분선 / 아이콘 크기(32 / 16), `MNU_TEST_CP` 하위 0 확인. DACrux 를 다시 시작해야 보인다. 화면 확인 대기.

## 18. Bin 기준 변경: 결과 파일 Hardware Bin (2026-10-14, CLAUDE-102)

계기: U9-HS2635-01 (OMM / TSVET) 업로드 후 YMS Map 이 전부 PASS(초록). 이 Device 의 등록 Parameter 109 개에 Spec 이 없어 E-Test 값 판정이 모두 NULL -> Die 판정 P -> BIN 1. 파일은 73 Die 모두 Fail (HW 2 / SW 2001 = 5, HW 4 / SW 4001 = 68).

사용자 결정:
- YMS 는 E-Test Limit 판정과 별개로 결과 파일의 Hardware / Software Bin 으로 보여 준다. 파싱해 올릴 때 YMS 의 Map / Measure 와 POC E-Test 기능은 따로 보고, 공용 부분(파싱 / Map 좌표 / 적재 순서)만 같이 쓴다.
- Measure 는 Setup(등록)된 Parameter 만, LSL / USL 도 등록 Spec (14절 그대로).
- Die BIN = **Hardware Bin** ("우선 하드웨어 빈으로"). Software Bin 을 BIN 으로 쓰는 안은 YMS 제약으로 불가: 요약 쿼리 `TD_TABLE.xml GET_RAW_DATA` 가 `BIN < 100` 만 세고 `TQP_WAFER_SUM` / `TQP_LOT_SUM` 은 BIN0 ~ BIN150 열만 있다.
- 양품 = 파일 DUT 결과 Pass. 예전 판정 Bin 정의(1 PASS / 2 SPEC_FAIL / 3 NOT_TESTED)는 지우고 파일 Bin 으로 다시 만든다 (사용자가 고친 정의는 유지).

| 항목 | 규칙 |
| --- | --- |
| TD BIN | `TQP_GDS_ET_DIE.HW_BIN` (0 ~ 99, 없거나 범위 밖이면 입력 확인 단계 FAIL, 메시지에 DUT 번호) |
| Bin 정의 (`TQP_BINDESC`) | 이 Wafer 의 Hardware Bin 중 정의가 없는 것만 등록 (기존 `EXISTS_BIN_INFO` / `CREATE_BIN`). BIN_NAME `HW2 SW2001` (Software Bin 여러 개면 `HW2 SW2001,2002`, 30자), DESCRIPTION `E-Test file Hardware Bin 2 / Software Bin 2001 / Fail` |
| 양품 (HIGH_GEC) | 그 Hardware Bin 의 Die 가 모두 파일 결과 Pass 면 Y. 섞여 있으면 N + 설명 "Pass and Fail mixed" |
| 색 | 양품 `#00C000`, 불량은 Hardware Bin 끝자리로 고정 (2 빨강 / 3 파랑 / 4 자주 / 5 금색 ...) |
| 예전 정의 정리 | BIN / BIN_NAME / DESCRIPTION 이 예전 규칙 값과 모두 같을 때만 삭제 (기존 `SELECT_PGM_BIN` / `DELETE_PROGRAM_BIN`) |
| 결과 표 열 | `BIN1_COUNT / BIN2_COUNT / BIN3_COUNT` -> `GOOD_DIE_COUNT / FAIL_DIE_COUNT / BIN_COUNTS` (예 `1:20, 5:4`) |
| E-Test 쪽 | 값 / Die 판정(JUDGE_FLAG)과 POC 화면은 그대로. 다시 판정(Rejudge)해도 YMS Bin / 수율은 바뀌지 않는다 (YMS 반영은 계속 다시 한다) |

변경 파일: `Interface/ETest/EtYmsRule.cs` (Bin 규칙) / `BSL/EtYmsPublisher.cs` (Die 모델 / 입력 확인 / Bin 정의) / `BSL/GdsEtResult_Yms.cs` (Die 읽기) / `08_DBQuery/ORACLE/TQP_GDS_ET_YMS.xml` `SELECT_ET_YMS_DIE` (값 판정 집계 대신 HW_BIN / SW_BIN / PASS_FLAG) / `Interface/iGdsEtResult.cs` (주석) / Probe 3개. 새 쿼리 / DDL 없음.

검증 (TEMP 빌드 `%TEMP%\chippac\CLAUDE-102\bin`, 오류 0): `ET_YMS_RULE_PROBE=PASS (27/27)` / `ET_YMS_PUBLISH_PROBE=PASS (23/23)` (샘플 RH-HS2615-02: Bin 1 Pass 20 / Bin 5 Fail 4 -> 수율 83.33, 예전 정의 2 삭제 / 고친 3 유지, HW Bin 없음 / 100 -> FAIL) / `ET_YMS_SERVICE_PROBE=PASS (9/9)` (업로드 -> YMS 83.33, Spec 완화 다시 판정 후에도 83.33) / 회귀 `ET_RESULT_PROBE=PASS (18/18)`.
