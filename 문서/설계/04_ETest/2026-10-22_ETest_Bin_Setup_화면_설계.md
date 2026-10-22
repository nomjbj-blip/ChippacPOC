# E-Test Bin 설정 화면 설계 (frmSetupBin 의 POC 판)

작성: 2026-10-22 Claude (CLAUDE-201) / 상태: **구현 완료(코드, 결정 B1 ~ B9 권장안) / TEMP 빌드 + Probe + 화면 밖 Smoke 확인 / 배포 안 함 / DMS 화면 확인 대기** (13절). DDL 없음

관련 문서: [E-Test Upload YMS 적재 설계](2026-10-14_ETest_Upload_YMS적재_설계.md) 18절(Bin 기준 = 결과 파일 Hardware Bin) / [E-Test Para 화면 재구성](2026-10-14_ETest_Para_화면_재구성_설계.md) / [POC 버튼 아이콘 / 레이아웃 기준](../06_환경_운영/2026-10-16_POC_화면_버튼_아이콘_레이아웃_기준.md)

## 1. 요청

| 날짜 | 요청 |
| --- | --- |
| 2026-10-22 | "frmSetupBin 에 대한 화면이 있다. 해당 화면을 POC 성격에 맞게 하나 더 만들어줘. 해당 화면은 TestArea 에 Program 이다. 다른 성격으로 바꿔도 된다. 설계" |

## 2. 기존 화면 frmSetupBin (소스 확인)

| 항목 | 내용 |
| --- | --- |
| 위치 | `DMS/09_TEST/DACrux.TEST.ENGUI/Admin/frmSetupBin.cs` / 메뉴 `01_WinApp/DACruxV5/MenuFile.mnu:1086` / 화면 글자 "Setup by Bin Info" |
| 구성 | 왼쪽 TESTAREA 목록 -> PROGRAM 목록 -> 오른쪽 FarPoint 표. 표는 Bin 0 ~ 150 고정 151행 |
| 열 | PROGRAM / BIN / BIN_NAME / CHAR_BIN / HIGH_GEC / DISPLAY / UPPER_LIMIT_CNT / LOWER_LIMIT_CNT / COLOR / DESCRIPTION |
| 서버 | `TEST.RO.ProbeAdmin`: `GetTestAreaList` / `GetTestProgramList` / `GetBinListEditable`(`SELECT_BIN_EDITABLE`) / `MergeBinInfo`(`MERGE_BIN`, 행마다) / `CreateCopyBin`(`CREATE_COPY_BIN`) |
| 버튼 | Search / Save / Copy / New Program |

POC 에서 쓰기 불편한 점:

- TestArea / Program 은 E-Test 업로드가 만든 값이다(공정 `TSVET`, Program `OMM_TSVET` = Device_공정). 사용자는 E-Test 화면에서 늘 Device / 공정으로 고르는데 이 화면만 YMS 이름으로 찾아야 한다.
- 151행이 늘 나와 실제 쓰는 Bin(로컬 DB 2 ~ 4개)을 찾기 어렵다.
- 그 Bin 의 Die 가 몇 개인지, 파일 결과가 Pass 인지 Fail 인지 안 보인다. 양품(HIGH_GEC)을 정할 근거가 화면에 없다.
- 삭제가 없다. 지운 파일의 Bin 정의가 남는다(로컬 `OMM_TSVET` Bin 2 / 4 는 지금 파일에 없음).
- 색 칸을 선택만 해도 색 창이 뜬다(`SelectionChanged`).
- 저장 뒤 결과 안내 / 다시 조회가 없다. 양품을 바꿔도 YMS 수율(`TQP_WAFER_SUM`)은 다시 계산되지 않는다.
- 버튼이 POC 기준(16x16 아이콘 / 실행 높이 28 / ImageBeforeText)이 아니다.

## 3. 방향

| 항목 | 새 화면 |
| --- | --- |
| 화면 | 신규 `frmEtBinSetup` (화면 글자 "E-Test Bin 설정", 메뉴 "E-Test Bin Setup"). **기존 frmSetupBin 은 그대로 둔다** |
| 고르는 기준 | TestArea / Program 대신 **E-Test Device / 공정**. YMS Program = `EtYmsRule.ToProgramName(Device, 공정)`(업로드와 같은 규칙), 화면에 읽기 전용으로 보인다 |
| 행 | 그 Program 의 Bin 정의 + 업로드 파일 Die 에 나온 Hardware Bin (합집합, Bin 순). 고정 행 없음 |
| 고치는 값 | 이름 / 양품 / 색 / 설명 4개. Map 기호 / 표시 / Limit 수는 화면에 없고 저장 때 기존 값을 그대로 둔다 |
| 판단 근거 | Software Bin / Die / Pass / Fail / 파일 / 상태를 **표 열**로 보인다(툴팁 아님) |
| 자동 처리 | 없음. 양품 / 이름은 사람이 고친다. 상태 열은 알려 주기만 한다 |
| 저장 대상 | 같은 `TQP_BINDESC`. 기존 화면과 YMS 화면이 같은 값을 본다 |

로컬 DB 현황 (2026-10-22 13:50, `oracle19c-DMS` 읽기만):

| Device / 공정 | YMS Program | 파일 | 파일 Die 의 Hardware Bin | Bin 정의 |
| --- | --- | --- | --- | --- |
| OMM / TSVET | OMM_TSVET | 1 | 1 (SW 1, Die 20 모두 Pass) / 5 (SW 5001, Die 4 모두 Fail) | 1 양품 / 2 / 4 / 5. 2 / 4 는 지금 파일에 없음 |
| OMMTCB / TSVET | OMMTCB_TSVET | 2 | 2 (SW 2001, Die 10 Fail) / 4 (SW 4001, Die 136 Fail) | 2 / 4. 양품 Bin 없음 |

## 4. 화면 구성

- 위치: `DMS/02_DMS/DACrux.SEMDMS.ENGUI/ETest/frmEtBinSetup.cs` / `.Designer.cs` / `.resx` (E-Test 화면과 같은 폴더 / 같은 서비스 `RO.GdsEtResult`)
- 화면 단위 7x12 / 굴림 9 (새 화면 기준). 버튼은 기준 문서 2절(아이콘 / 높이 28 / 막대 40 / 저장 오른쪽 끝 / 주 기능 Bold).

```text
+-----------------------------------------------------------------------------------------------+
| Factory [SCK ] Device [OMM      v] 공정 [TSVET v] YMS Program [OMM_TSVET   ] [조회]       [저장] |  막대 1 (40)
| [Bin 추가]                 [선택 삭제]                                                          |  막대 2 (40)
+-----+-------------+-----+---------+-------------------------------+---------+----+----+----+----+-------------+
| Bin | 이름        | 양품 | 색      | 설명                          | Software| Die|Pass|Fail|파일| 상태        |
|     |             |     |         |                               | Bin     |    |    |    |    |             |
+-----+-------------+-----+---------+-------------------------------+---------+----+----+----+----+-------------+
|   1 | HW1 SW1     | [v] | #00C000 | E-Test file Hardware Bin 1 /. | 1       | 20 | 20 |  0 |  1 |             |
|   2 | HW2 SW2001  | [ ] | #FF0000 | E-Test file Hardware Bin 2 /. |         |  0 |  0 |  0 |  0 | 파일에 없음 |
|   4 | HW4 SW4001  | [ ] | #C000C0 | E-Test file Hardware Bin 4 /. |         |  0 |  0 |  0 |  0 | 파일에 없음 |
|   5 | HW5 SW5001  | [ ] | #FFD700 | E-Test file Hardware Bin 5 /. | 5001    |  4 |  0 |  4 |  1 |             |
+-----+-------------+-----+---------+-------------------------------+---------+----+----+----+----+-------------+
| busyBar (진행 표시줄)                                                                             |
+-----------------------------------------------------------------------------------------------+
```

| 컨트롤 | 이름 | 버튼 KEY / 비고 |
| --- | --- | --- |
| Factory | `txtFactory` 읽기 전용 | 로그인 Factory |
| Device / 공정 | `cboDevice` / `cboTestProgram` (DropDownList) | 목록 = `GetEtBinProgramList` |
| YMS Program | `txtYmsProgram` 읽기 전용 | `EtYmsRule.ToProgramName` |
| 조회 | `btnSearch` | SEARCH |
| 저장 | `btnSave` (Bold, 막대 1 오른쪽 끝) | SAVE |
| Bin 추가 | `btnAdd` | ADD |
| 선택 삭제 | `btnDelete` (추가와 떨어뜨림) | DELETE |
| 표 | `dgvBin` (DataGridView, Fill) | E-Test 화면과 같은 표 종류 |
| 진행 표시 | `busyBar` (ProgressStatusBar) | 다른 POC 화면과 같음 |

새 버튼 KEY 는 없다(모두 기준 문서 3.1절 공통 KEY).

## 5. 표 열

| 열 | 값 | 편집 | 비고 |
| --- | --- | --- | --- |
| Bin | `BIN` = Hardware Bin | 새 행만 입력, 0 ~ 99 | `EtYmsRule.MaxBin` 과 같음(YMS 요약이 BIN < 100 만 센다) |
| 이름 | `BIN_NAME` | 더블클릭 / F2, 30자 이하 | |
| 양품 | `HIGH_GEC` Y / N | 클릭(체크) | YMS 수율에서 양품으로 세는 Bin |
| 색 | `COLOR` `#RRGGBB` | 더블클릭 / Enter -> 색 창 | 칸 배경 = 그 색, 글자 = 값. 빈 값이면 YMS Map 기본 색(`TPUCMapView.cs:2260`) |
| 설명 | `DESCRIPTION` | 더블클릭 / F2, 100자 이하 | |
| Software Bin | 그 Hardware Bin Die 의 Software Bin (작은 번호부터 `,`) | 읽기 | 참고 |
| Die | 그 Bin 의 Die 수 (READY 파일 전체) | 읽기 | 참고 |
| Pass / Fail | 파일 DUT 결과(`PASS_FLAG`) P / F Die 수 | 읽기 | 양품 판단 근거 |
| 파일 | 그 Bin Die 가 있는 파일 수 | 읽기 | 참고 |
| 상태 | 아래 표 | 읽기 | 알려 주기만 |

| 상태 글자 | 조건 | 할 수 있는 일 |
| --- | --- | --- |
| 정의 없음 | 파일에는 있는데 `TQP_BINDESC` 에 없음 (YMS 반영 실패 / 아직 안 함) | 값을 넣고 저장하면 정의가 생긴다. 그대로 두면 다음 YMS 반영 때 업로드 규칙 기본값으로 생긴다 |
| 파일에 없음 | 정의는 있는데 지금 READY 파일 Die 에 없음 | 필요 없으면 [선택 삭제] |
| 파일 결과와 다름 | 양품 체크인데 Fail Die 가 있음, 또는 양품이 아닌데 Fail 0 / Pass 1 이상 | 사람이 판단. 체크를 바꾸면 바로 다시 계산해 보인다 |
| YMS 에 못 넣음 | Hardware Bin 이 없거나 0 ~ 99 밖 | 편집 불가. 그 파일은 YMS 반영이 이미 FAIL 이다 |
| (빈 값) | 그 밖 | |

Map 기호(`CHAR_BIN`) / 표시(`DISPLAY`) / Limit 수(`UPPER_LIMIT_CNT` / `LOWER_LIMIT_CNT`)는 화면에 열을 만들지 않는다. 서버가 저장 때 기존 값을 읽어 그대로 넣는다(7.3절).

## 6. 동작

| 순서 | 동작 |
| --- | --- |
| 열기 | 로그인 Factory 를 넣고 `GetEtBinProgramList` 로 Device / 공정 목록을 채운다. 첫 Device / 공정을 고르고 조회한다 |
| Device / 공정 바꿈 | 바로 조회. 저장 안 한 수정이 있으면 먼저 묻는다: 예 = 버리고 바꿈 / 아니요 = 콤보를 원래대로 (frmEtParaSetup 과 같음) |
| 조회 | `GetEtBinList(Factory, Device, 공정)`. 표를 채우고 행 Tag 에 조회 당시 값을 둔다 |
| 고치기 | 바뀐 칸 노란색(`LightYellow`) + 행 머리글 `*`. 원래 값으로 돌리면 표시가 없어진다. 형식이 틀린 칸은 붉은색(`MistyRose`) |
| 색 | 색 칸 더블클릭 / Enter -> `ColorDialog`(지금 색에서 시작). 확인하면 칸 배경과 값(`#RRGGBB`)을 바꾼다. 선택만으로는 열리지 않는다 |
| Bin 추가 | 표 끝에 새 행을 넣고 Bin 칸 편집을 시작한다. Bin 번호는 0 ~ 99, 표 안 중복 불가. 양품 / 색 / 이름 / 설명은 빈 값으로 시작한다 |
| 선택 삭제 | 고른 행 중 정의가 있는 행의 Bin 정의를 지운다(확인창: "Bin 2, 4 정의를 지웁니다. 파일에 있는 Bin 은 다음 YMS 반영 때 기본값으로 다시 만들어집니다."). 새 행 / 정의 없는 행은 표에서만 뺀다. 저장 안 한 다른 수정이 있으면 "먼저 저장하세요" 로 막는다 |
| 저장 | 바뀐 행 + 새 행만 `SaveEtBins` 로 보낸다. 보내기 전 화면 검사(Bin 범위 / 중복, 이름 30자, 설명 100자, 색 형식). 저장 뒤 같은 Device / 공정을 다시 조회한다 |
| 수율 다시 계산 | 저장 결과 `GOOD_CHANGED = Y` 이고 YMS 반영 파일이 있으면 묻는다: "양품 Bin 이 바뀌었습니다. YMS 수율을 다시 계산할까요? (파일 n개)". 예 = 기존 `RejudgeEtFiles(Factory, Device, 공정)` (판정 + YMS 다시 반영) |
| 진행 표시 | 서버 호출은 busyBar 로 묶는다(다른 POC 화면과 같음) |

이름 / 색 / 설명만 바뀌면 수율 질문을 하지 않는다. YMS 화면은 열 때 `TQP_BINDESC` 를 읽는다. 수율만 `TQP_WAFER_SUM` / `TQP_LOT_SUM` 에 저장된 값이다(기존 `TestCommon.SummaryData`).

예: OMM / TSVET 에서 Bin 5 를 양품으로 바꾸고 다시 계산하면 Wafer RH-HS2615-02 수율 83.33 -> 100 이 된다(측정 Die 24 = Bin 1 20 + Bin 5 4, 로컬 `TQP_WAFER_SUM` 확인).

## 7. 서버

`iGdsEtResult` partial 인터페이스 / `GdsEtResult` BSL partial 에 Bin 전용 파일을 더한다(Para 와 같은 방식). 새 wellknown / DDL 은 없다.

### 7.1 메서드

| 메서드 | 구분 | 내용 |
| --- | --- | --- |
| `DataTable GetEtBinProgramList(string strFactory)` | 신규 | READY 파일이 있는 Device / 공정. 열 DEVICE_ID / TEST_PROGRAM / YMS_PROGRAM / FILE_COUNT / YMS_FILE_COUNT |
| `DataTable GetEtBinList(string[] aKey)` | 신규 | aKey = Factory / Device / 공정. 열 BIN / BIN_NAME / HIGH_GEC / COLOR / DESCRIPTION / HAS_DEF / SW_BINS / DIE_COUNT / PASS_COUNT / FAIL_COUNT / FILE_COUNT / IN_RANGE |
| `DataTable SaveEtBins(string[] aKey, string[,] aRows, string strUpdateUser)` | 신규 | aRows 열 5개: BIN / BIN_NAME / HIGH_GEC(Y / N) / COLOR / DESCRIPTION. 반환 1행 SAVED_COUNT / NEW_COUNT / GOOD_CHANGED / YMS_FILE_COUNT |
| `int DeleteEtBins(string[] aKey, string[] aBins, string strUpdateUser)` | 신규 | 지운 정의 수 |
| `int RejudgeEtFiles(string[] aKey, string strUpdateUser)` | 기존 그대로 | 수율 다시 계산에 쓴다 |

YMS Program 은 서버가 `EtYmsRule.ToProgramName(Device, 공정)` 으로 만든다. 화면이 Program 이름을 보내지 않는다.

### 7.2 쿼리 (`08_DBQuery/ORACLE/TQP_GDS_ET_YMS.xml` 에 2개 추가)

```sql
-- SELECT_ET_BIN_PROGRAM_LIST : READY 파일이 있는 Device + 공정
SELECT F.DEVICE_ID, F.TEST_PROGRAM, COUNT(*) AS FILE_COUNT,
       SUM(CASE WHEN F.YMS_STATUS = 'DONE' THEN 1 ELSE 0 END) AS YMS_FILE_COUNT
  FROM TQP_GDS_SRC_FILE F
 WHERE F.FACTORY = :factory
   AND F.LOAD_STATUS = 'READY'
 GROUP BY F.DEVICE_ID, F.TEST_PROGRAM
 ORDER BY F.DEVICE_ID, F.TEST_PROGRAM

-- SELECT_ET_BIN_USE : Device + 공정 READY 파일의 Die 를 Hardware / Software Bin / 파일로 묶음
SELECT D.HW_BIN, D.SW_BIN, D.SRC_FILE_SEQ, COUNT(*) AS DIE_COUNT,
       SUM(CASE WHEN D.PASS_FLAG = 'P' THEN 1 ELSE 0 END) AS PASS_COUNT,
       SUM(CASE WHEN D.PASS_FLAG = 'F' THEN 1 ELSE 0 END) AS FAIL_COUNT
  FROM TQP_GDS_ET_DIE D
  JOIN TQP_GDS_SRC_FILE F ON F.SRC_FILE_SEQ = D.SRC_FILE_SEQ
 WHERE F.FACTORY = :factory
   AND F.DEVICE_ID = :device_id
   AND F.TEST_PROGRAM = :test_program
   AND F.LOAD_STATUS = 'READY'
 GROUP BY D.HW_BIN, D.SW_BIN, D.SRC_FILE_SEQ
```

- `LISTAGG(DISTINCT ...)` 는 Oracle 19c 부터라 쓰지 않는다(고객 DB 버전 미확인). 행이 적어(파일 x Bin) BSL 에서 Hardware Bin 별로 Die / Pass / Fail 합, Software Bin 목록, 파일 수를 만든다.
- YMS_STATUS `DONE` = `EtYmsPublisher.StatusDone` (소스 확인). 쿼리에 글자로 쓰지 않고 BSL 이 상수로 비교해도 된다.

### 7.3 Bin 정의 읽기 / 쓰기 (기존 TEST DSL 함수만, `EtYmsPublisher.RegisterBins` 와 같은 방식)

> 구현에서 바뀜(13.2): 기존 `SELECT_BIN_EDITABLE` / `MERGE_BIN` 바인딩 크기 때문에 `TQP_GDS_ET_YMS.xml` 에 Bin 전용 쿼리를 두었다. 아래 표는 설계 당시 안이다.

| 단계 | 기존 함수 (`DACrux.TEST.DSL.TQP_BINDESC`) | 쿼리 |
| --- | --- | --- |
| 정의 읽기 | `GetBinListEditable(program)` | `SELECT_BIN_EDITABLE` (HIGH_GEC 는 `True` / `False` 로 온다 -> Y / N 으로 바꿔 돌려준다) |
| 기존 값 유지 | `SelectProgramBin(program, bin)` | `SELECT_PGM_BIN` -> CHAR_BIN / DISPLAY / UPPER_LIMIT_CNT / LOWER_LIMIT_CNT 를 읽어 그대로 넣는다 |
| 쓰기 | `MergeBinInfo(string[])` | `MERGE_BIN`. 새 Bin 은 CHAR_BIN 빈 값 / DISPLAY `Y` / Limit 빈 값(업로드 자동 등록과 같음) |
| 삭제 | `DeleteBinInfo(program, bin, user)` | `DELETE_PROGRAM_BIN` |

- 저장은 모든 행을 먼저 검사하고(Bin 0 ~ 99 / 중복, 이름 30자, 설명 100자, 색 빈 값 또는 `#RRGGBB`, HIGH_GEC Y / N) 하나라도 틀리면 아무것도 쓰지 않고 오류를 돌려준다. 쓰기는 행마다 MERGE(기존 화면 / 업로드와 같음, 트랜잭션 없음).
- GOOD_CHANGED 는 쓰기 전 HIGH_GEC 와 비교해 서버가 정한다.
- `TQP_BINDESC` 에는 FACTORY / 수정자 / 수정 시각 열이 없다. strUpdateUser 는 다른 E-Test 메서드처럼 필수 검사만 한다.

## 8. 바꿀 파일 (구현 때)

| 구분 | 파일 |
| --- | --- |
| 신규 화면 | `02_DMS/DACrux.SEMDMS.ENGUI/ETest/frmEtBinSetup.cs` / `.Designer.cs` / `.resx` |
| 신규 서버 | `02_DMS/DACrux.SEMDMS.Interface/iGdsEtResult_Bin.cs` / `02_DMS/DACrux.SEMDMS.BSL/GdsEtResult_Bin.cs` |
| 수정 RO | `02_DMS/DACrux.SEMDMS.RO/GdsEtResult.cs` 또는 신규 partial `GdsEtResult_Bin.cs` (구현 때 RO partial 여부 확인) |
| 수정 쿼리 | `08_DBQuery/ORACLE/TQP_GDS_ET_YMS.xml` 쿼리 2개 추가 (DSL 은 기존 `GetData`) |
| csproj | Interface / BSL / (RO) / ENGUI Compile 행, ENGUI resx EmbeddedResource 1행 |
| 메뉴 | `01_WinApp/DACruxV5/MenuFile.mnu` `MNU_DMS_ET_BIN` (POC 메뉴, E-Test Para Setup 다음) |
| 검사 | 신규 Probe `06_CoreLibSource/GdsMapControl/Probes/EtBinSetupProbe.ps1` / 신규 `ENGUI/Verification/EtBinSetupFormSmoke.cs` / `Verify-PocButtonLayout.ps1` `$forms` 1행 |
| 바꾸지 않음 | `frmSetupBin` / TEST.RO · BSL · DSL / `TQP_BINDESC.xml` / `EtYmsPublisher` / DDL |

배포 영향: 클라이언트 `DACrux.SEMDMS.ENGUI.dll` + 서버 SEMDMS Interface / BSL / RO dll + `TQP_GDS_ET_YMS.xml` (`101_ServerBin`, 서비스 재시작) + 메뉴 파일.

## 9. 결정 필요 (권장 먼저)

| ID | 항목 | 권장 | 다른 안 |
| --- | --- | --- | --- |
| B1 | 고르는 기준 | **E-Test Device / 공정** (YMS Program 은 읽기 전용 표시) | 기존처럼 TestArea / Program |
| B2 | 행 범위 | **Bin 정의 + 파일에 나온 Hardware Bin** | Bin 0 ~ 99 고정 100행 |
| B3 | 고치는 열 | **이름 / 양품 / 색 / 설명** (나머지는 기존 값 유지) | 기존 10열 모두 |
| B4 | 참고 열 / 상태 | **Software Bin / Die / Pass / Fail / 파일 / 상태를 표 열로** | 정의 열만 |
| B5 | Bin 추가 / 삭제 | **둘 다 넣음** (삭제는 확인창, 추가와 떨어뜨림) | 추가 없음(업로드 자동 등록만) / 삭제 없음 |
| B6 | 수율 다시 계산 | **저장 뒤 양품이 바뀌었을 때만 질문 -> 기존 `RejudgeEtFiles`** | 별도 버튼 / 하지 않음(다음 업로드 때) |
| B7 | 다른 Program 에서 복사(기존 Copy) | **넣지 않음** (Device 수가 적고 업로드가 정의를 만든다) | 다른 Device / 공정을 골라 복사 |
| B8 | 서버 경로 | **SEMDMS `GdsEtResult` 메서드 4개 + 쿼리 2개, Bin 정의는 BSL 이 기존 TEST DSL 함수 호출** | 화면이 `TEST.RO.ProbeAdmin` 을 직접 부르고 SEMDMS 는 Bin 분포 조회 1개만 |
| B9 | 위치 / 메뉴 | **SEMDMS.ENGUI/ETest, POC 메뉴 E-Test Para Setup 다음** | TEST.ENGUI/Admin (frmSetupBin 옆) |

B8 권장 이유: E-Test 화면이 모두 `RO.GdsEtResult` 하나만 부른다. 정의와 파일 Bin 합치기 / 입력 검사 / 기존 값 유지를 서버 한 곳에서 해 Probe 로 확인할 수 있다. 업로드(`EtYmsPublisher`)도 같은 TEST DSL 함수로 Bin 정의를 쓴다.

## 10. 주의할 경우

- 양품 Bin 이 하나도 없으면 YMS 요약은 BIN1 을 양품으로 본다(`SummaryData`, 적재 설계 2절). 로컬 `OMMTCB_TSVET` 이 이 경우다(Bin 1 Die 없음 -> 로컬 수율 0, Wafer 2개).
- 이름 규칙상 다른 Device 가 같은 YMS Program 이 될 수 있다(`[A-Z0-9_]` 밖 글자는 `_`). 그러면 Bin 정의를 같이 쓴다. 업로드 규칙과 같은 문제라 이번 범위 밖이고 화면 YMS Program 칸으로 확인한다.
- 기존 frmSetupBin 에서 같은 Program 을 같이 고치면 나중 저장이 남는다(잠금 없음).
- "정의 없음" 행을 고치지 않고 두면 저장하지 않는다. 다음 YMS 반영이 기본값으로 만든다.

## 11. 구현 순서와 검증

1. 쿼리 2개 + Interface / BSL / RO + Probe -> TEMP 빌드 + `ET_BIN_SETUP_PROBE=PASS`
   - 목록: OMM / TSVET, OMMTCB / TSVET
   - 합치기: OMM_TSVET = Bin 1 / 5 파일에 있음, 2 / 4 "파일에 없음". Software Bin / Die / Pass / Fail 이 3절 표와 같음
   - 저장 거부: 이름 31자 / 색 `red` / Bin 100 / 중복 Bin -> 아무것도 쓰지 않음
   - 저장: 숨긴 열(CHAR_BIN / DISPLAY / Limit) 기존 값 유지, 새 Bin = DISPLAY Y
   - 양품 변경: GOOD_CHANGED = Y -> `RejudgeEtFiles` 뒤 `TQP_WAFER_SUM.YIELD` 83.33 -> 100 -> 되돌림
   - 삭제 후 다시 조회
   - 시험 데이터는 Probe 가 만들고 지운다. 기존 OMM / OMMTCB 정의는 시험 뒤 원래 값으로 되돌리고 수를 비교한다
2. 화면 Designer + 동작 + 메뉴 + Smoke -> `ET_BIN_SETUP_FORM_SMOKE=PASS` / `POC_BUTTON_LAYOUT=PASS` / 소스 흔적 검사 0건
3. 배포는 사용자 VS 빌드 -> DMS 화면 확인 (조회 / 색 / 양품 바꾸고 수율 다시 계산 / YMS Map 색 확인)

## 12. 확인 수준

| 항목 | 수준 |
| --- | --- |
| 기존 frmSetupBin 구조 / 서버 함수 / 쿼리 | 소스 확인 |
| E-Test 업로드의 Bin 자동 등록 규칙 (`EtYmsPublisher.RegisterBins`) | 소스 확인 |
| 로컬 DB Bin 분포 / 정의 (3절 표) | DB 확인 (읽기만, 2026-10-22 13:50) |
| YMS Map 빈 색 처리 (`TPUCMapView.cs:2260`) | 소스 확인 |
| 새 화면 / 서버 / 쿼리 | 빌드 확인 / Probe 확인 / 화면 밖 Smoke 확인 (13.3). DMS 화면 확인은 배포 뒤 |

## 13. 구현 결과 (2026-10-22, 사용자 "진행" = B1 ~ B9 권장안)

### 13.1 파일

| 구분 | 파일 |
| --- | --- |
| 신규 화면 | `02_DMS/DACrux.SEMDMS.ENGUI/ETest/frmEtBinSetup.cs` / `.Designer.cs` / `.resx`(버튼 아이콘 SEARCH / SAVE / ADD / DELETE) |
| 신규 서버 | `02_DMS/DACrux.SEMDMS.Interface/iGdsEtResult_Bin.cs` / `02_DMS/DACrux.SEMDMS.BSL/GdsEtResult_Bin.cs` / `02_DMS/DACrux.SEMDMS.RO/GdsEtResult_Bin.cs` (RO 도 partial) |
| 쿼리 | `08_DBQuery/ORACLE/TQP_GDS_ET_YMS.xml` 5개 추가: `SELECT_ET_BIN_PROGRAM_LIST` / `SELECT_ET_BIN_USE` / `SELECT_ET_BIN_DEF` / `MERGE_ET_BIN` / `DELETE_ET_BIN` |
| csproj | Interface / RO / BSL Compile 1행씩, ENGUI Compile 2블록 + EmbeddedResource 1블록 |
| 메뉴 | `01_WinApp/DACruxV5/MenuFile.mnu` `MNU_DMS_ET_BIN` "E-Test Bin Setup" (POC, MNU_ORDER 6 = E-Test Para Setup 바로 뒤, F03130, 아이콘은 Para Setup 과 같음). `100_ClientBin` 사본은 사용자 VS 빌드 때 복사 |
| 검사 | 신규 `06_CoreLibSource/GdsMapControl/Probes/EtBinSetupProbe.ps1` / `ENGUI/Verification/EtBinSetupFormSmoke.cs` / `Verify-EtBinSetup.ps1`, `Verify-PocButtonLayout.ps1` (`$forms` 1행 + SEMDMS Interface / RO TEMP 빌드 2행) |
| 바꾸지 않음 | `frmSetupBin` / TEST.RO · BSL · DSL / `TQP_BINDESC.xml` / `EtYmsPublisher` / DDL |

### 13.2 설계와 달라진 점

| 항목 | 설계 | 구현 | 이유 |
| --- | --- | --- | --- |
| Bin 정의 읽기 / 쓰기 | 기존 TEST DSL `GetBinListEditable` / `SelectProgramBin` / `MergeBinInfo` / `DeleteBinInfo` | `TQP_GDS_ET_YMS.xml` 의 `SELECT_ET_BIN_DEF` / `MERGE_ET_BIN` / `DELETE_ET_BIN` (SEMDMS DSL `GetData` / `UpdateExecuteMultiple`) | 기존 `MERGE_BIN` 은 바인딩이 모두 `size="40"` 이라 **설명이 40자에서 잘린다**(Probe 실측: 60자 -> 40자). 업로드가 만든 설명이 50 ~ 53자라 이름만 고쳐도 설명이 잘린다. `SELECT_BIN_EDITABLE` 은 `:program` 이 25 라 26자 Program 이 잘린다. 기존 쿼리는 다른 화면이 쓰므로 고치지 않았다 |
| 숨긴 열 유지 | 저장 전 `SELECT_PGM_BIN` 으로 읽어 그대로 넣기 | `MERGE_ET_BIN` 이 이름 / 양품 / 색 / 설명 4열만 UPDATE | 읽고 다시 쓰는 단계가 없어 더 단순하다. 새 정의는 표시 Y / Map 기호 · Limit 없음(업로드 자동 등록과 같음) |
| 색 고르기 키 | 더블클릭 / Enter | 더블클릭 / F2 | 표가 Enter 를 다음 행 이동으로 먼저 처리해 KeyDown 까지 오지 않는다 |
| 상태 글자 | 정의 없음 / 파일에 없음 / 파일 결과와 다름 / YMS 에 못 넣음 | + `새 Bin`(추가한 행), 입력 오류가 있으면 오류 문장(붉은 글자) | 새 행과 오류 이유를 같은 칸에서 보이게 |
| 표 열 폭 | 설명 320 | 설명 = 남는 폭 채우기(최소 200), 상태 140 | 1100 폭에서 가로 스크롤 / '파일' 머리글 접힘을 없앰 |
| 진행 표시 | busyBar | 목록 / Bin 조회는 `RunHere`(콤보 변경에서도 부름), 저장 / 삭제 / 수율 다시 계산은 `Run` | 빌드 가이드 기준(콤보 변경 이벤트 안에서는 RunHere) |

같은 날 후속(CLAUDE-207): 기존 `TQP_BINDESC.xml` `MERGE_BIN` / `SELECT_BIN_EDITABLE` 바인딩도 size 0 으로 고쳤다(frmSetupBin / frmSetupProgram 도 긴 설명 / 26자 Program 이 잘리지 않음, 컬럼보다 긴 값은 저장 오류). 이 화면은 4열만 바꾸는 `MERGE_ET_BIN` 을 그대로 쓴다.

DB 연결: `MERGE_ET_BIN` 은 `DMS_CONNECT_ID`(SEMDMS DSL)로 `TQP_BINDESC` 를 쓴다. 업로드의 자동 등록은 `TEST_CONNECT_ID`(TEST DSL)다. 지금 서버 설정은 두 이름이 같은 DB 이고, 같은 XML 의 `UPDATE_YMS_PARASPEC` 도 같은 방식으로 YMS 테이블을 쓴다.

### 13.3 검증 (TEMP 빌드 `%TEMP%\chippac\EtBinSetupVerification`, 로컬 docker `oracle19c-DMS`)

| 검사 | 결과 |
| --- | --- |
| TEMP 빌드 (Interface / DSL / BSL / RO / Control / ENGUI) | 오류 0. 새 파일 경고 0 (Control 12 / ENGUI 4 는 기존 파일 경고) |
| `EtBinSetupProbe.ps1 -BinDir <TEMP>` | `ET_BIN_SETUP_PROBE=PASS (15/15)`: 목록 / 정의 + 분포 합치기 / 업로드 설명 50자 그대로 / 새 Bin 기본값 / 저장 거부 6종(아무것도 안 씀) / 숨긴 열 유지 + 설명 100자 / 기존 MERGE_BIN 60 -> 40자 잘림 실측(CLAUDE-207 에서 고친 뒤 7단계 = 60자 그대로 · 101자 ORA-12899 · 26자 Program 조회) / 양품 변경 -> 다시 판정 수율 83.33 -> 100 -> 83.33 / 삭제 거부 · 삭제 / 정의 삭제 Bin 이 다시 반영 때 기본값으로 다시 생김 / 끝 정리 / 다른 Program 정의 그대로 (108행 해시 같음) |
| `Verify-EtBinSetup.ps1` (화면 밖 Smoke) | `ET_BIN_SETUP_FORM_SMOKE=PASS` 17항목: 구성 / 조회 / 색 칸 / 읽기 칸 편집 막음 / 이름 · 양품 · 색 수정 표시 / F2 색 창 / Bin 추가 중복 · 길이 오류 / 저장 + 수율 질문 -> 100 / 이름만 저장(질문 없음) / 수정 중 조회 · 삭제 막음 / 삭제 / 양품 되돌림 83.33 / 새 행 표에서만 빼기. 그림 `bin_setup.png` |
| `Verify-PocButtonLayout.ps1` | frmEtBinSetup 버튼 4개 모두 OK. 전체는 `FAIL (2)` = 기존 `frmEtParaRegister`(결과 파일 열기 높이 24 / Device 삭제 아이콘 없음, 이번 범위 밖) |
| 소스 흔적 검사 | 새 / 수정 파일 0건 |
| 시험 데이터 | Probe / Smoke 끝난 뒤 PRBBIN* Bin 정의 / 파일 / 등록 Parameter / 시험 Map / YMS Program / Lot / TD 테이블 0건 |

DB 확인 범위: Factory PROBE 시험 데이터만 만들고 지웠다. 다른 Program 의 Bin 정의(108행)는 시험 전 / 후 같다. 고객 DB(`CPKMESDV`)에는 붙지 않았다(Probe / Smoke 설정 사본의 연결 이름 = DMSMGR).

### 13.4 배포 / 사용자 확인 순서

1. VS 에서 솔루션 빌드: 클라이언트 `DACrux.SEMDMS.ENGUI.dll` / `DACrux.SEMDMS.RO.dll` / `DACrux.SEMDMS.Interface.dll` + 메뉴 파일, 서버 `DACrux.SEMDMS.BSL.dll` / `DACrux.SEMDMS.Interface.dll` + `08_DBQuery/ORACLE/TQP_GDS_ET_YMS.xml`, SEMDMS Service 재시작. 새 wellknown 은 없다.
2. DACrux 를 다시 열고 POC 메뉴 > E-Test Bin Setup (E-Test Para Setup 바로 아래).
3. OMM / TSVET: Bin 1 / 2 / 4 / 5 가 보이고 2 / 4 는 "파일에 없음" 인지 본다.
4. Bin 5 양품 체크 -> [저장] -> 수율 다시 계산 "예" -> YMS 화면(TEST Map View / Yield) 수율 100 확인 -> 다시 양품 해제 + 저장 + "예" 로 되돌린다.
5. 색을 바꾸고 YMS Map 색이 바뀌는지 본다(수율 다시 계산 없이 바로 반영).
