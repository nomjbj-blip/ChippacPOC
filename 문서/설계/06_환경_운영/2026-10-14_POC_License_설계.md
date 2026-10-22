# POC License 설계 (Setup 사용 기한 / Device 5개 / Report 계속 조회 / 정식 License 확장 구조)

작성: 2026-10-14 Claude (CLAUDE-089) / 갱신: 2026-10-19 v3 / 상태: **설계 v3 — L1 확정, 결정 대기 L2 · L4 · L5 · L6 ~ L8** (소스 / DB 변경 없음, 로컬 DB 조회만)

사용자 지시:
- (10-14) "POC에서는 3개월간 Setup 관련된 기능은 활성화하고 report는 그냥 계속 볼수 있도록 해야 한다."
- (10-14) "POC 이기 때문에 소스상에서 Limit을 걸어야 한다."
- (10-14) "나중에 개발이 완료 되고 해당 기능을 넣어야 하니 미리 설계를 잘 해놓자."
- (10-14, v1 검토 후) "L1 : 1월 말까지."
- (10-14, v1 검토 후) "여기 IT 팀이 DACrux를 할 줄 알기 때문에 소스에 해당 기능을 녹여야 한다."
- (10-14, v1 검토 후) "DB에서 현재 일자를 확인하는 방법으로 해야 한다. PC에서 시간을 바꿀수 있다."
- (10-14, v1 검토 후) "service에서 확인해서 일자가 충족이 되면 다음 step으로 넘어가는 방식이 좋을것같다."
- **(10-19) "등록되는 device는 5개 Limit이 있다. 소스상에서 5개까지만 등록 가능하도록 설계 진행"**

근거: 2026-10-02 상견례 녹취 — POC 결과물을 3개월 정도 사용해 본 뒤 확장하려면 실제 라이선스를 구매하는 흐름으로 안내함 (`회의록/2026-10-02_POC_상견례_및_업무협의/녹취원문.txt` 451행).

수정 이력:
- v1 (2026-10-14): 공용 DLL(SEMDMS.Interface)에 판정 클래스, 메뉴 / 화면은 PC 시간, 서버는 DB 시간.
- v2 (2026-10-14): 기한 2027-01-31 까지. PC 시간 판정 제거, 판정은 서버 전용 `DACrux.SEMDMS.DSL` 의 업무 흐름 단계(`WorkPeriod`), DB 일시 SQL 은 코드 안, 저장 흐름 "1단계 일자 확인 → 다음 단계".
- **v3 (2026-10-19, 이 문서)**: **등록 Device 5개 제한** 추가(5절). 판정 클래스를 `WorkPeriod` → **`WorkRule`**(기간 + Device 수)로 넓힘. 같은 첫 단계에서 Device 이름도 확인한다. 10-14 이후 바뀐 메뉴 / 서비스 함수(Sub Device 제거, Map 복사, E-Test Para 등록 / 설정, E-Test 결과 Upload / View 분리)를 3절에 반영. L3 해소(Setup > DM 메뉴 숨김).

---

## 1. 결론

1. 기능을 **Report(조회) / Setup(기준정보 저장) / Upload(데이터 적재)** 로 나눈다. Setup / Upload 는 **DB 일시 기준 2027-01-31 까지**(2027-02-01 00:00:00 부터 차단), Report 는 기한 없이 허용한다.
2. **등록 Device 는 5개까지**다. GDS Map / Wafer Map / E-Test Parameter / E-Test Spec / E-Test 결과 파일에 들어 있는 Device 이름을 **모두 합쳐(같은 이름은 1개)** 센다. 이미 등록된 Device 는 계속 수정 / 업로드 / 삭제할 수 있고, **6번째 새 이름**을 만드는 저장만 막는다.
3. 판정은 **서비스(서버)에서만** 한다. 시간은 **DB 일시**(`SYSTIMESTAMP`, 한국 시간으로 변환)만 쓰고, Device 수도 DB 에서 직접 센다. PC 시간 / 클라이언트 값은 판정에 쓰지 않는다.
4. 서비스 저장 흐름의 **첫 단계가 `WorkRule.Confirm`**(기한 → Device 수 순서)이다. 통과해야 다음 단계(작업 키 발급 / 저장)로 넘어간다. Begin → 배치 → Complete 흐름은 Begin 에서 키를 받지 못하면 진행할 수 없고, Complete 에서 다시 확인한다.
5. **소스에 녹인다**: 기한 / Device 수(5) / 판정 / 조회 SQL 은 모두 서버 전용 DLL(`DACrux.SEMDMS.DSL`) 안 컴파일된 코드에 있다. 설정 파일 / `MenuFile.mnu` / Query XML / DB 테이블 / License 파일 어디에도 값이나 끄는 스위치가 없다.
6. 정식 License 는 `WorkRule` 안에서 **값의 출처만** 상수에서 License 정보로 바꾼다(10절). 서비스 함수의 첫 단계 호출 / 메뉴 목록은 그대로 쓴다.

```text
 클라이언트(판정 안 함)                              서버 Service (판정은 여기만)
 ─────────────────────                           ─────────────────────────────────────────────
 시작 → GetWorkState() ───────────────────────▶  WorkRule.GetState()   ── DB 일시 / 등록 Device
      ◀── 기간 안 / 밖 ── 메뉴 막기(안내용, L5)

 저장 → BeginXxx(Device) ─────────────────────▶  1단계 WorkRule.Confirm(모듈, 종류, Device)
                                                   ├ ① 기한 밖            → 예외, 흐름 끝
                                                   ├ ② 새 Device + 이미 5개 → 예외, 흐름 끝
                                                   └ 통과 → 2단계 작업 키 발급 / 저장
      → 배치 ... → CompleteXxx() ─────────────▶  1단계 Confirm(같은 Device) → 2단계 확정
```

## 2. 현재 소스 확인 결과

| 항목 | 확인 내용 | 설계 반영 |
| --- | --- | --- |
| 기존 License | `06_CoreLibSource/DACrux.Framework.BSL/License.cs` → `Miracom.Middleware.BaseComponent.ChkLicense(IP)`. 로그인 / 메인 Load 호출은 주석 처리(미사용). Middleware 4.0.0.7 에 `GetLicense(파일, 키)` → `LicenseInfo { ServerIP, DueDate, Users }` 와 설정 키 `LICENSEFILE` / `LICENSEKEY` / `CLIENTLICENSE` 가 있으나 서버 / 클라이언트 설정 어디에도 없음. Device 수 항목은 없음 | POC 에서는 쓰지 않음. 정식 때 값 출처 후보(10.2) |
| 메뉴 클릭 | Core `DACruxMain.utbManager_ToolClick` 이 `MenuFile.mnu` 행으로 화면을 만든다. 취소할 수 있는 확장 지점 없음. Tool Key = `MENU_KEY`, `SharedProps.Enabled`(`MenuHelper.cs:136`) | 클라이언트는 메뉴 Tool 만 비활성(L5) |
| 고객이 바꿀 수 있는 것 | `MenuFile.mnu`(ENABLE / VISIBLE), 서비스 config, `08_DBQuery/ORACLE/*.xml`(서버 폴더의 텍스트), DB 권한 표(FUNCTION_CODE), 로그인 Factory | 값 / 판정을 이 중 어디에도 두지 않는다. Device 는 Factory 와 관계없이 센다 |
| 코드 안 SQL | 기존 `TQC_LOT_STS.xml` 의 `SELECT_SYS_DATE` 는 XML 이라 고정 날짜로 바꿀 수 있다. Middleware `QueryComponent.GetDataTable(ConnectID, Query)` 로 **코드 안 SQL** 실행 가능 | DB 일시 / Device 수 모두 코드 안 SQL |
| 서버 전용 DLL | `DACrux.SEMDMS.DSL` 은 `101_ServerBin` 에만 있고 `100_ClientBin` 에 없음. SEMDMS.BSL / TEST.BSL 이 둘 다 참조. 두 서비스 config 모두 `DMS_CONNECT_ID=DMSMGR` | 판정 클래스 위치 |
| Device 가 들어가는 표 | `TQP_GDS_MAP.DEVICE_ID` / `TQP_MAPDEF.MAPID` / `TQP_GDS_ET_ITEM.DEVICE_ID` / `TQP_GDS_ET_LIMIT.DEVICE_ID` / `TQP_GDS_SRC_FILE.DEVICE_ID`. GDS Chain 표는 `TQP_GDS_MAP` 의 자식(FK). KLARF 업로드는 Product 의 `TQP_MAPDEF` 가 없으면 오류(CLAUDE-078), E-Test 업로드도 Map 필수(CLAUDE-116) | 5개 표를 합쳐 센다(5.1) |
| 로컬 DB Device 수 (2026-10-19 조회) | 합집합 **21개** = POC 이름 7(OMM / OMMTCB / OMM_TCB / OMM_Hybrid / Hybrid / U7 / U9) + Probe 3(FULL_PROBE / LAYER_FILTER_PROBE / VIEW_PROBE, Factory PROBE) + 2019 년 YMS 샘플 Map 11(`TQP_MAPDEF`, INSERT_TIME 2019) | 개발 DB / 전달 DB 처리 필요(5.5) |
| Probe | GDS Import / Map 복사 / E-Test / KLARF Probe 약 15개가 `JOB_PROBE` / `ET_PROBE` / 복사 대상 같은 임시 Device 를 만들고 지운다 | 개발 DB 예외(L8) |
| DB 식별값 | `SYS_CONTEXT('USERENV','DBID')` 는 권한 없이 읽힌다. 로컬 `DMS`(non-CDB, 2026-10-13 생성) DBID = 3988091916 | 개발 DB 예외 조건(L8) |
| 조회 화면 안 저장 | GDS Map View: Chain DB 저장 / Map 삭제 / [Map 복사](`MapCopyForm` → `CopyMap`). E-Test Result View: 파일 삭제(`DeleteEtFile`). 오류는 `MessageBox.Show(ex.Message)` 로 표시 | 화면 코드 수정 없이 서비스 문장이 그대로 보임 |
| 클라이언트 → 서비스 | RO 가 코드에서 URL 을 만들고(`RemoteConfig.url` + objectUri), 서버 config 에 wellknown 1줄. 기존 POC 서비스에 함수를 더하면 config 변경 없음 | 클라이언트 확인 함수는 기존 `iGdsMapView` 에 추가(L5) |

## 3. 기능 분류

### 3.1 메뉴 / 화면 / 서비스 첫 단계 (2026-10-19 메뉴 기준)

목록에 없는 메뉴는 Report 로 본다. 새 Setup / Upload 화면을 만들면 **서비스 저장 함수 첫 단계 + 메뉴 목록**에 함께 등록한다(공통 개발 지침에 규칙 추가, 12절 5단계). 표의 **(D)** 는 첫 단계에서 Device 이름도 넘기는 함수다.

| 메뉴 (MENU_KEY) | 화면 | 종류 | 모듈 | 서비스 첫 단계 확인 함수 |
| --- | --- | --- | --- | --- |
| GDS Map Setup (`MNU_DMS_GDS_MAP_SETUP`) | frmGdsMapSetup → GdsMapForm | Setup | Gds | `GdsMapImport.BeginMapImport`(D) / `CompleteMapImport`(D, 작업에서 읽음) / `DeleteMap`(D) |
| GDS Map View (`MNU_DMS_GDS_MAP_VIEW`) | frmGdsMapView → GdsMapViewForm | **Report** (안의 저장 동작만 Setup) | Gds | `GdsChain.BeginChainSave`(D) / `CompleteChainSave`(D) / `DeleteChain`(D), `GdsMapImport.CopyMap`(D = 새 이름) / `DeleteMap`(D) |
| Wafer Map Setup (`MNU_STANDARD_MAP_FILE`) | frmStandardMapSetup → MapCreationService | Setup | WaferMap | TEST `ProbeAdmin.SetMapDef`(D) / `UpdateMapDef`(D) / `CreateUseMap`(D) / `DeleteMapDef`(D) — **기존 함수(L4)** |
| Wafer Map View (`MNU_STANDARD_MAP_VIEW`) | frmStandardMapView | Report | WaferMap | 없음 |
| E-Test Para Register (`MNU_DMS_ET_PARA_REG`) | frmEtParaRegister | Setup | ETest | `GdsEtResult.RegisterEtParameters`(D) |
| E-Test Para Setup (`MNU_DMS_ET_PARA`) | frmEtParaSetup | Setup | ETest | `RegisterEtParameters`(D) / `SaveEtParameterSetup`(D) / `DeleteEtParameters`(D) / `SaveEtLimit`(D) / `DeleteEtLimit`(D) / `RejudgeEtFiles`(D) |
| KLARF File Upload (`MNU_KLARF_UPLOAD`) | frmKlarfUpload | Upload | Klarf | `KlarfUpload.BeginUpload`(D = Product) / `CommitUpload` / `DeleteUploadedStep` |
| E-Test Result Upload (`MNU_DMS_ET_RESULT_UPLOAD`) | frmEtResultUpload | Upload | ETest | `GdsEtResult.BeginEtFile`(D) / `CompleteEtFile` / `DeleteEtFile` / `PublishEtFileToYms` |
| E-Test Result View (`MNU_DMS_ET_RESULT_VIEW`) | frmEtResultView | **Report** (안의 파일 삭제만 Upload) | ETest | `DeleteEtFile` |
| GDS-KLARF Analysis (`MNU_GDS_KLARF_ANALYSIS`) | frmGdsKlarfAnalysis | Report | Gds | 없음 |
| DMS Analysis 4개 / TEST Analysis 7개 | 기존 DMS / YMS 화면 | Report | Dms | 없음 |
| (숨김) Setup > DM 3개 / GDS Die Analysis | | - | | 숨김 메뉴라 대상 아님. 다시 보이게 하면 이 표에 추가 |

### 3.2 조회 화면 안의 동작

| 화면 | 동작 | 기한 후 / Device 초과 |
| --- | --- | --- |
| GDS Map View | Map 조회 / Layer / Label 검색 / Chain 목록 / 추적 / 유사 묶음 찾기 / 메모리 Chain 편집 | 그대로 사용 |
| GDS Map View | Chain DB 저장 / Map 삭제 / [Map 복사] | 서비스 첫 단계에서 막힘 → 기존 오류 창에 서비스 문장. [Map 복사]는 새 이름이 6번째면 막힘 |
| E-Test Result View | 결과 조회 | 그대로 사용 |
| E-Test Result View | 파일 삭제 | 기한 후 막힘 |

### 3.3 서비스 함수 중 확인하지 않는 것

| 함수 | 이유 |
| --- | --- |
| `Get*` / `AnalyzeWafer` 등 읽기 | Report |
| `CancelUpload` / `CancelImportJob` | 실패 작업 정리. 막으면 미완료 작업이 남는다 |
| 배치 중간 함수 `CreateLayerBatch` / `CreatePlacedBatch` / `CreateChainMemberBatch` / `CreateChainEditBatch` / `CreateEtDieBatch` / `CreateEtValueBatch` / `AddImage` | Begin 에서 받은 작업 키가 있어야만 부를 수 있고, Complete 에서 다시 확인한다. 호출 수가 많아 매번 DB 를 읽지 않는다 |

## 4. 사용 기한과 DB 일시

| 항목 | 내용 |
| --- | --- |
| 마지막 날 | **2027-01-31** (L1 확정). DB 일시 2027-02-01 00:00:00 부터 Setup / Upload 차단 |
| 판정 | `FirstValidDay(2026-10-01) <= DB 일시 < 2027-02-01` 이면 통과 |
| DB 일시 | `SYSTIMESTAMP AT TIME ZONE '+09:00'` — 코드 안 SQL. DB 서버 시간대 설정과 관계없이 한국 시간 |
| `SYSTIMESTAMP` 를 쓰는 이유 | Oracle `FIXED_DATE` 설정은 `SYSDATE` 만 고정한다. DBA 가 `SYSDATE` 를 과거로 고정해도 영향이 없다(구현 때 로컬 DB 에서 확인) |
| 하한(`FirstValidDay`) | DB 일시가 2026-10-01 보다 앞서면 시각이 잘못된 것으로 보고 막는다 |
| DB 조회 실패 | 막는다. 서버 PC 시간으로 대신하지 않는다 |
| 기한 변경(연장) | 상수 변경 → `DACrux.SEMDMS.DSL` 다시 빌드 → `101_ServerBin` 배포 + SEMDMS / TEST 서비스 재시작. 클라이언트 재배포 없음 |

## 5. 등록 Device 5개 (v3)

### 5.1 무엇을 "등록 Device" 로 세나 (L6)

아래 5개 표의 Device 이름을 **합집합**으로 센다. 이름은 `UPPER(TRIM())` 로 비교하고(`OMM_Hybrid` = `OMM_HYBRID`), **Factory 는 보지 않는다**(Factory 를 새로 만들어 5개를 더 쓰는 것을 막기 위해).

| 표 | 열 | 이 이름이 생기는 저장 |
| --- | --- | --- |
| `TQP_GDS_MAP` | DEVICE_ID | GDS Map Setup 저장(`BeginMapImport`) / GDS Map View [Map 복사](`CopyMap`) |
| `TQP_MAPDEF` | MAPID | Wafer Map Setup 저장(TEST `SetMapDef`) |
| `TQP_GDS_ET_ITEM` | DEVICE_ID | E-Test Para 등록(`RegisterEtParameters`) |
| `TQP_GDS_ET_LIMIT` | DEVICE_ID | E-Test Spec 저장(`SaveEtParameterSetup` / `SaveEtLimit`) |
| `TQP_GDS_SRC_FILE` | DEVICE_ID | E-Test 결과 업로드(`BeginEtFile`) |

- KLARF Product 는 `TQP_MAPDEF` 가 있어야 업로드되므로 따로 세지 않아도 이미 들어 있다. YMS Product 는 E-Test Device 를 따른다.
- GDS / Wafer Map / E-Test 에서 이름이 같으면 1개다. 이름이 다르면 각각 1개다. 예: GDS `U9` + Map 복사 `OMM_Hybrid` / `OMM_TCB` + E-Test `OMM` = **4개**.
- 표 하나만 세면(예: GDS Map) Wafer Map Setup 으로 Map 을 계속 만들고 KLARF 를 올려 Device 제한 없이 DMS 분석을 쓸 수 있다. 그래서 Device 가 들어가는 표를 모두 센다.

### 5.2 판정 규칙

| 경우 | 결과 |
| --- | --- |
| 넘긴 Device 가 이미 등록된 이름 | 통과 — 수정 / 재업로드 / 삭제는 항상 가능(기한 안에서) |
| 새 이름 + 등록 수 4개 이하 | 통과 — 저장하면 등록 수가 1 늘어난다 |
| 새 이름 + 등록 수 5개 이상 | 막는다 |
| Device 를 넘기지 않는 함수(`CompleteEtFile(파일 번호)` 등) | 기한만 확인. 이 함수들은 이미 등록된 Device 의 작업만 다룬다 |

- 막힐 때 문장: `POC 에서는 Device 를 5개까지 등록할 수 있습니다. 등록된 Device(5): OMM, OMM_HYBRID, OMM_TCB, U9, U7. 새 Device 를 쓰려면 쓰지 않는 Device 의 Map / Parameter / Spec / 결과를 삭제하세요.`
- **삭제하면 자리가 빈다**(L7). 5개 표에서 그 이름의 행이 모두 없어져야 빈다. 기한이 지나면 삭제도 막히므로 그때부터 등록 수는 바뀌지 않는다.

### 5.3 여러 단계 저장과 동시 등록

- GDS Import 는 `BeginMapImport` 에서 확인하고, `CompleteMapImport` 에서 **다시 세어** 5개를 넘으면 확정하지 않는다. 두 사람이 동시에 서로 다른 새 Device 를 올려 6개가 되는 경우를 Complete 에서 막는다(그 작업은 미완료로 남고 사용자가 취소).
- 한 번 호출로 끝나는 함수(`CopyMap` / `RegisterEtParameters` / `SetMapDef` 등)는 확인과 저장 사이가 짧아 그대로 둔다. POC 사용자 수로 보아 동시에 새 Device 를 만드는 일은 드물다. 정식 때는 DB 잠금으로 확인과 저장을 묶는다(10.3).

### 5.4 조회 SQL (코드 안)

```sql
SELECT D FROM (
    SELECT UPPER(TRIM(DEVICE_ID)) D FROM TQP_GDS_MAP
    UNION SELECT UPPER(TRIM(MAPID))     FROM TQP_MAPDEF
    UNION SELECT UPPER(TRIM(DEVICE_ID)) FROM TQP_GDS_ET_ITEM
    UNION SELECT UPPER(TRIM(DEVICE_ID)) FROM TQP_GDS_ET_LIMIT
    UNION SELECT UPPER(TRIM(DEVICE_ID)) FROM TQP_GDS_SRC_FILE)
 WHERE D IS NOT NULL
 ORDER BY D
```

목록을 받아 C# 에서 개수와 포함 여부를 본다(목록은 막힐 때 문장에 넣는다). Query XML 에 두지 않는 이유는 DB 일시와 같다(XML 을 고치면 0건으로 만들 수 있다).

### 5.5 개발 DB 와 전달 DB (L8)

| 구분 | 상황 | 처리 |
| --- | --- | --- |
| 개발 DB (로컬 `DMS`) | 이미 21개. Probe 약 15개가 임시 Device 를 만들고 지운다. 제한을 그대로 걸면 새 Device 를 만드는 Probe 가 모두 실패한다 | 권장: **DBID 가 개발 DB(3988091916)일 때만 Device 수 확인을 건너뛴다**(기한은 그대로 확인). DBID 는 `SYS_CONTEXT('USERENV','DBID')` 로 DB 일시와 같은 SQL 에서 읽는다 |
| 고객 DB | Data Pump 로 옮긴 DB 는 새 DBID 를 가진다. Oracle DBNEWID 는 새 값을 임의로 만들므로 고객이 개발 DB 값으로 맞출 수 없다 | 제한 적용 |
| 전달 DB | 설치 설계(CLAUDE-088) 기준 "서버 확보 후 설치 파일과 DB 전달". 지금 개발 DB 를 그대로 보내면 21개라 처음부터 막힌다 | 전달 전에 **연구소가 확정한 대상 Device(RnR "대상 Device 기준정보 확정") 외 데이터를 지운 사본**을 만들고 Data Pump 로 보낸다. Probe Device / 2019 샘플 Map 은 지운다. 전달 DB 의 등록 수가 5 이하인지 Probe 로 확인 |

- 조건: DB 는 **Data Pump(expdp / impdp) 또는 스크립트로만 전달**한다. 데이터 파일 / docker 볼륨을 통째로 복사하면 DBID 가 같이 넘어가 고객 DB 도 예외가 된다. 서버 / DB 전달 절차서에 이 조건을 적는다.
- 로컬 DB 를 다시 만들면(컨테이너 재생성) DBID 가 바뀌므로 상수를 고친다.
- 대안(예외 없음): 개발 DB 도 대상 Device 4 ~ 5개만 남기고, 새 Device 를 만드는 Probe 는 기존 이름을 쓰도록 고친다. 2019 샘플 Map 과 연결된 YMS 시험 데이터도 같이 정리해야 한다.

## 6. 소스에 녹이는 방법

| 원칙 | 방법 |
| --- | --- |
| 바꿀 수 있는 곳에 두지 않는다 | 기한 / Device 수(5) / 판정 / 조회 SQL 을 설정 파일 · `MenuFile.mnu` · Query XML · DB 테이블 · License 파일에 두지 않는다. 컴파일된 서버 DLL 안에만 있다 |
| 떼어 낼 수 있는 부품으로 만들지 않는다 | 별도 DLL(예: `DACrux.License.dll`)을 만들지 않는다. 같은 이름의 빈 DLL 로 바꿔치기하면 끝나기 때문이다. 업무 쿼리가 모여 있는 `DACrux.SEMDMS.DSL` 안 클래스로 둔다 |
| 클라이언트에 두지 않는다 | DSL 은 `101_ServerBin` 에만 배포된다. 클라이언트 PC 의 DLL 을 고쳐도 판정에 영향이 없다 |
| 업무 흐름의 한 단계로 둔다 | 저장 함수의 첫 단계 "작업 조건 확인" 으로 부른다. 통과해야 작업 키 발급 / 저장 단계로 넘어간다 |
| 셀 값을 따로 저장하지 않는다 | Device 수는 별도 등록 표 없이 **실제 업무 데이터에서 매번 센다.** 등록 표가 있으면 그 행만 지워 자리를 만들 수 있다 |
| 이름 | 코드 이름은 업무 용어(`WorkRule` / `Confirm` / `WorkModule` / `WorkKind`)를 쓴다. 디컴파일 결과에서 "License" 로 검색해 바로 찾지 못하게 한다. 설명 주석은 공통 개발 지침대로 남긴다(주석은 DLL 에 들어가지 않는다) |

## 7. 서버 설계

### 7.1 판정 클래스 (`02_DMS/DACrux.SEMDMS.DSL/WorkRule.cs`, 신규)

```csharp
namespace DACrux.SEMDMS.DSL
{
    /// 작업 모듈. 정식 License 에서 모듈별 조건을 줄 때 그대로 쓴다. POC 는 모듈과 관계없이 같은 조건.
    public enum WorkModule { Dms, Gds, Klarf, ETest, WaferMap }

    /// 작업 종류. 조회는 확인하지 않으므로 저장 종류만 둔다.
    public enum WorkKind { Setup, Upload }

    /// 서비스 저장 흐름의 첫 단계. DB 일시와 등록 Device 를 읽어 작업 조건(기한 / Device 수)을 확인한다.
    /// 통과하면 DB 일시를 돌려주고, 아니면 예외로 흐름을 멈춘다. 조회 SQL 은 Query XML 이 아닌 이 코드에 둔다.
    public class WorkRule : Miracom.Middleware.QueryComponent
    {
        static readonly DateTime LastWorkDay = new DateTime(2027, 1, 31);   // 이 날까지 작업 가능
        static readonly DateTime FirstValidDay = new DateTime(2026, 10, 1); // 이보다 앞선 DB 일시는 잘못된 시각
        const int MaxDeviceCount = 5;                                       // 등록 Device 수
        const string DevDatabaseId = "3988091916";                          // 개발 DB(L8). Device 수 확인만 건너뜀

        /// 1단계: 기한 → Device 수 순서로 확인. deviceId 가 비면 기한만 본다. 통과하면 DB 일시.
        public DateTime Confirm(WorkModule module, WorkKind kind, string deviceId);

        /// 클라이언트 안내용 한 행: DB_TIME / LAST_DAY / SETUP_OPEN / UPLOAD_OPEN / DAYS_LEFT / DEVICE_COUNT / DEVICE_MAX / DEVICE_LIST.
        public DataTable GetState();

        /// 판정만 하는 함수(시험용으로 값을 넣어 확인).
        internal static bool IsPeriodOpen(WorkModule module, WorkKind kind, DateTime dbNow);
        internal static bool IsDeviceAllowed(string deviceId, IList<string> registered);

        /// SYSTIMESTAMP AT TIME ZONE '+09:00' + SYS_CONTEXT('USERENV','DBID') 한 번에 조회. 실패하면 예외.
        void ReadDbState(out DateTime dbNow, out string dbId);

        /// 5.4 SQL. 대문자 Device 이름 목록.
        List<string> ReadRegisteredDevices();
    }
}
```

- 기한 문장: `POC 사용 기간(2027-01-31 까지)이 지나 저장할 수 없습니다. 조회 화면은 계속 사용할 수 있습니다.`
- Device 문장: 5.2 절. 시각 오류 / 조회 실패: `서버 일시를 확인할 수 없어 저장할 수 없습니다.`
- 모두 `InvalidOperationException`. 기존 BSL 이 던지는 예외와 같은 계열이라 기존 화면 오류 표시가 그대로 동작한다.
- 구현 때 확인: `GetDataTable(ConnectID, Query)` 가 `InitQueryComponent` 없이 동작하는지 Probe 로 먼저 본다.

### 7.2 서비스 흐름 (첫 단계 = 작업 조건 확인)

```csharp
public DataTable BeginEtFile(string[] FileInfo)
{
    // 1단계: 작업 조건 확인(기한 / Device 수). 막히면 여기서 끝나고 작업 키가 만들어지지 않는다.
    new DACrux.SEMDMS.DSL.WorkRule().Confirm(WorkModule.ETest, WorkKind.Upload, FileInfo[1]);

    // 2단계: 기존 처리 (인자 검사 → 파일 행 생성 → 작업 키 반환)
    ...
}
```

| 흐름 | 1단계 확인 | 다음 단계 |
| --- | --- | --- |
| GDS Import | `BeginMapImport`(D) / `CompleteMapImport`(D, 다시 셈) | 작업 키 → Layer / Placed 배치 → 확정 |
| GDS Map 복사 | `CopyMap`(D = 새 이름) | DRAFT 복사 → READY |
| GDS Chain 저장 | `BeginChainSave`(D) / `CompleteChainSave`(D) | 작업 키 → Member / Edit 배치 → 확정 |
| E-Test Para / Spec | `RegisterEtParameters` / `SaveEtParameterSetup` / `DeleteEtParameters` / `SaveEtLimit` / `DeleteEtLimit` / `RejudgeEtFiles` (모두 D) | 저장 |
| E-Test Upload | `BeginEtFile`(D) / `CompleteEtFile` / `PublishEtFileToYms` | 파일 행 → Die / Value 배치 → 확정 → YMS |
| KLARF Upload | `BeginUpload`(D = Product) / `CommitUpload` | 키 발급 → 이미지 추가 → 반영 |
| 삭제 | `DeleteMap`(D) / `DeleteChain`(D) / `DeleteEtFile` / `DeleteUploadedStep` | 삭제 |
| Wafer Map (L4) | `ProbeAdmin.SetMapDef` / `UpdateMapDef` / `CreateUseMap` / `DeleteMapDef` (모두 D) | 저장 |

- 확인은 인자 검사보다 앞, DB 작업 전에 둔다. 읽기 쿼리 1 ~ 2번이 늘 뿐 트랜잭션 / 저장 흐름은 바뀌지 않는다.
- TEST.BSL 은 이미 SEMDMS.DSL 을 참조하므로 참조 추가 없이 같은 클래스를 부른다(L4).

## 8. 클라이언트 (안내용, L5)

판정은 하지 않는다. 서비스가 준 결과로 메뉴만 막는다.

1. `iGdsMapView` / RO / BSL 에 읽기 함수 `GetWorkState()` 1개 추가 → BSL 은 `WorkRule.GetState()` 결과를 그대로 반환. 기존 wellknown 을 쓰므로 config 변경 없음.
2. `01_WinApp/DACruxV5/MenuWorkRule.cs`(신규): Setup / Upload 메뉴 키 목록 + 적용.
   - Main 이 보인 뒤 **백그라운드로** `GetWorkState()` 호출(서버가 꺼져 있어도 시작이 늦어지지 않게) → 결과가 오면 UI 스레드에서 기한이 지난 메뉴의 `SharedProps.Enabled = false` + 제목 문장.
   - 호출 실패(서버 꺼짐 / 옛 서버) → 아무것도 바꾸지 않는다. 저장은 서비스가 막는다.
   - 제목 문장: D-14 부터 `[Setup 사용 기한 2027-01-31, 7일 남음]`, 기한 후 `[Setup 사용 기간 종료 - 조회 전용]`.
   - Device 수는 메뉴를 막지 않는다(등록된 Device 는 계속 Setup 할 수 있으므로). 6번째 새 이름은 저장 때 서비스 문장으로 안내한다.
3. `DACruxClientMain.cs`: DEBUG / Release 두 곳에서 `ApplyMenuSize(mMain)` 다음에 `MenuWorkRule.Attach(mMain)` 1줄.

메뉴 목록:

| MENU_KEY | 종류 |
| --- | --- |
| `MNU_DMS_GDS_MAP_SETUP` / `MNU_STANDARD_MAP_FILE` / `MNU_DMS_ET_PARA_REG` / `MNU_DMS_ET_PARA` | Setup |
| `MNU_KLARF_UPLOAD` / `MNU_DMS_ET_RESULT_UPLOAD` | Upload |

메뉴 파일 ENABLE 이 이미 False 인 메뉴는 켜지 않는다. 막는 쪽으로만 바꾼다.

## 9. 우회 가능성과 한계

| 방법 | 결과 |
| --- | --- |
| PC 시간 변경 | 영향 없음(PC 시간을 쓰지 않음) |
| `MenuFile.mnu` 수정 / 옛 클라이언트 / `NexplantQMS.GdsMap.exe` 단독 실행 | 화면은 열려도 서비스 첫 단계에서 막힌다 |
| 서비스 config 수정 | 기한 / Device 관련 키가 없다. `DMS_CONNECT_ID` 를 바꾸면 업무 쿼리 전체가 같이 멈춘다 |
| Query XML 수정(`SELECT_SYS_DATE` 고정 등) | 영향 없음(조회 SQL 은 코드 안) |
| Factory 를 새로 만들어 등록 | 영향 없음(Device 는 Factory 와 관계없이 셈) |
| 대소문자 / 앞뒤 공백만 다른 이름 | 같은 Device 로 셈 |
| 쓰던 Device 를 지우고 새 Device 등록 | 허용(L7). 5개를 넘겨 동시에 쓸 수는 없다 |
| DB 에서 업무 데이터 행을 직접 지워 자리 만들기 | 막지 못한다. 그 Device 의 데이터를 잃는 조작이라 받아들인다 |
| 서버 서비스 PC 시간 변경 / DBA 의 `FIXED_DATE` | 영향 없음(`SYSTIMESTAMP` 사용, 구현 때 확인) |
| DB 서버 OS 시간을 과거로 변경 | 2026-10-01 이전이면 막힌다. 그 사이 날짜는 막지 못한다 |
| 개발 DB 를 파일째 복사해 고객 서버에서 기동 | DBID 가 같아 Device 수 확인이 빠진다 → 전달은 Data Pump 로만(5.5) |
| 서버 DLL 디컴파일 후 수정(dnSpy 등) | 막지 못한다. 설정 / 파일 수정이 아니라 프로그램 변조이고, 소스 머리말의 저작권 조항 위반이다. 난독화 도구는 Remoting 형식 이름을 바꿀 위험이 있어 POC 에서는 쓰지 않는다 |

## 10. 정식 License 확장 (나중에 넣을 때)

### 10.1 바꾸는 것 / 그대로 두는 것

| 구분 | POC | 정식 |
| --- | --- | --- |
| 기한 / Device 수 출처 | `WorkRule` 안 상수 | License 정보(10.2) — `WorkRule` 내부만 변경 |
| 서비스 첫 단계 호출 / `GetWorkState()` / 메뉴 목록 | 7 ~ 8절 | **그대로** |
| 기준 시간 | DB 일시 | DB 일시 (+ 10.3 되돌림 감지) |
| 모듈별 조건 | 모두 같음 | `WorkModule` 별 기한 / Device 수 — 호출 코드는 이미 모듈과 Device 를 넘기고 있음 |
| 개발 DB 예외 | DBID 상수 | 개발용 License 파일로 대체하고 상수 삭제 |

### 10.2 License 정보 출처 (정식 개발 때 결정)

- **후보 1 — 기존 Middleware License**: `BaseComponent.GetLicense(파일, 키)` → `LicenseInfo { ServerIP, DueDate, Users }`, 설정 키 `LICENSEFILE` / `LICENSEKEY` / `CLIENTLICENSE`. 회사 공통 방식이면 발급 도구 / 키 관리가 이미 있을 수 있다. 다만 모듈별 기한 / Device 수 항목은 없다. 발급 방법과 파일 형식은 프레임워크 담당에게 확인 필요.
- **후보 2 — 서명된 License 파일**: 서버의 XML 파일에 고객 / Site / 설치 대상(DB 이름 또는 서버 이름) / 모듈 x 종류별 마지막 날 / Device 수 / 사용자 수를 넣고, 미라콤 개인 키로 서명(RSA-SHA256), 서버 DLL 의 공개 키로 검증. 기한 연장 / Device 추가는 파일만 다시 발급한다.
- 어느 쪽이든 서명 / 파일이 없거나 틀리면 Report 만 허용한다.

### 10.3 그 밖에 정식 때 검토할 것

- 시간 되돌림 감지: 마지막으로 본 DB 일시를 저장해 두고 그보다 과거면 막는다.
- Device 동시 등록: 확인과 저장을 같은 트랜잭션 + DB 잠금(`DBMS_LOCK` 또는 잠금 행)으로 묶는다.
- Device 목록을 License 에 이름으로 넣을지(지정 Device 만 허용) 개수만 넣을지.
- PC / 동시 사용자 제한: Middleware `ChkLicense(IP)` / `LicenseInfo.Users` 와 합칠지 검토.
- 확인 지점을 원격 호출 공통 지점(서버 채널 Sink)으로 모으는 방법은 함수 이름 기반이라 디버깅이 어렵다. 함수 첫 단계 방식을 유지하는 것을 권장한다.

## 11. 결정

| ID | 내용 | 상태 / 권장 | 대안 |
| --- | --- | --- | --- |
| L1 | 사용 기한 | **확정: 2027-01-31 까지** (사용자 2026-10-14) | - |
| L2 | KLARF / E-Test **Upload** 도 기한 대상 | 권장: 예 — 기한 후 새 데이터 적재는 운영 사용. 기한 전 데이터는 계속 조회 | Upload 는 기한 없이 허용 |
| ~~L3~~ | ~~기존 DMS Setup 3개 메뉴~~ | v3 에서 해소 — 지금 숨김 메뉴라 대상 아님 | |
| L4 | Wafer Map Setup — TEST `ProbeAdmin.SetMapDef` / `UpdateMapDef` / `CreateUseMap` / `DeleteMapDef` 는 **기존 함수**(옛 frmSetupMap 도 사용). v3 에서는 Device 수 때문에 더 필요하다(Map 을 제한 없이 만들면 KLARF 업로드도 제한 없음) | 권장: 기존 함수 첫 단계에 확인 1줄 추가(기존 함수 변경 — 승인 필요) | `MapCreationService` 저장 전에 `GetWorkState()` 확인(기존 함수 수정 없음, 옛 화면 / 다른 경로는 못 막음) |
| L5 | 클라이언트 메뉴 막기 | 권장: 시작 후 백그라운드로 `GetWorkState()` 1번 → 기한 후 메뉴 비활성 + 제목 문장 | 메뉴는 그대로 두고 저장 때 서비스 문장만 |
| L6 | 등록 Device 세는 기준 | 권장: 5개 표 합집합 / Factory 무관 / 이름 대소문자·공백 무시(5.1) | 화면별(GDS / Map / E-Test) 각각 5개 — 이름이 다르면 최대 15개 |
| L7 | 삭제하면 자리가 비는지 | 권장: 예 — 지금 등록된 수 기준. 별도 저장값 없이 실제 데이터에서 셈 | 한 번 등록한 이름은 계속 셈 — 이력 표가 필요하고 그 표를 지우면 무력화 |
| L8 | 개발 DB 처리 | 권장: 개발 DB DBID 일 때만 Device 수 확인 건너뜀 + 전달은 Data Pump 로만 + 전달 DB 는 대상 Device 만 남김(5.5) | 예외 없음 — 개발 DB 를 5개 이하로 정리하고 Probe 를 기존 이름만 쓰게 변경 |
| ~~L6 (v1)~~ / ~~L7 (v1)~~ | ~~GDS View 클릭 전 확인 / 판정 클래스 위치~~ | v2 에서 해소 | |

## 12. 구현 단계 (결정 후)

편집 직전에 대상 파일을 다시 읽고, 작업 현황에서 같은 파일을 수정 중인 작업이 있는지 확인한다.

1. **판정 단계**: `SEMDMS.DSL/WorkRule.cs` + DSL csproj Compile 1행 + Probe `WorkRuleProbe.ps1`(① 코드 안 SQL 로 DB 일시 / DBID / 등록 Device 조회 ② `IsPeriodOpen` / `IsDeviceAllowed` 에 값을 넣어 판정).
2. **서비스 첫 단계**: SEMDMS.BSL(`GdsMapImport` / `GdsChain` / `GdsEtResult` / `GdsEtResult_Para` / `GdsEtResult_Yms` / `KlarfUpload`) 7.2 표 함수 첫 줄. L4 채택 시 TEST.BSL `ProbeAdmin` 4함수.
3. **클라이언트 안내(L5)**: `iGdsMapView` / RO / BSL `GetWorkState()` + `DACruxV5/MenuWorkRule.cs` + `DACruxClientMain.cs` 2곳 + DACruxV5 csproj(SEMDMS.RO / Interface 참조, Compile 1행).
4. **배포**: DSL / BSL / Interface → `101_ServerBin`, RO / Interface / DACruxV5 → `100_ClientBin`, SEMDMS / TEST 서비스 재시작.
5. **문서**: 공통 개발 지침에 "새 Setup / Upload 저장 함수 = 첫 단계 `WorkRule.Confirm`(Device 키가 있으면 넘김) + 메뉴 목록 등록" 규칙 / 빌드 가이드에 기한 변경 절차 / 서버·DB 전달 절차에 "Data Pump 로만 + 대상 Device 만 남김"(L8).
6. **전달 DB 준비(L8)**: 대상 Device 확정 후 사본 정리 스크립트 + 등록 수 확인 Probe.

## 13. 검증 기준 (기대값)

| 항목 | 방법 | 기대값 |
| --- | --- | --- |
| DB 일시 / DBID | Probe 에서 `ReadDbState` | 로컬 `SYSTIMESTAMP` 한국 시간과 같음 / DBID 3988091916 |
| 등록 Device 조회 | Probe 에서 `ReadRegisteredDevices` | 2026-10-19 로컬 기준 21개(2절)와 같은 목록 |
| 기한 판정 | `IsPeriodOpen` 에 일시 입력 | 2026-09-30 23:59:59 막힘 / 2026-10-01 00:00:00 통과 / 2027-01-31 23:59:59 통과 / 2027-02-01 00:00:00 막힘 |
| Device 판정 | `IsDeviceAllowed` 에 목록 입력 | 4개 + 새 이름 통과 / 5개 + 새 이름 막힘 / 5개 + 등록된 이름(`omm_hybrid ` 처럼 대소문자·공백 다름 포함) 통과 / 빈 이름은 기한만 |
| `FIXED_DATE` 영향 | 로컬 docker DB 에서만 `FIXED_DATE` 설정 → `ReadDbState` → 원복 (DB 설정 변경이라 실행 전 사용자 확인) | `SYSDATE` 는 고정 날짜, `ReadDbState` 는 실제 시각 |
| 첫 단계 누락 | 7.2 표 함수의 첫 줄을 소스 검사 | 누락 0 |
| 개발 DB 회귀 | 기존 Probe / Smoke (GDS Import / 복사 / E-Test / KLARF) | 지금과 같은 결과(개발 DB 예외로 임시 Device 생성 가능) |
| 제한 적용 시 서비스 | 로컬에서만 `DevDatabaseId` 를 다른 값으로 바꾼 임시 DSL 빌드 → 새 이름으로 `CopyMap` / `RegisterEtParameters` / `BeginMapImport` | Device 문장 예외 / DB 변경 없음. 등록된 이름은 통과. 확인 후 상수 원복 · 재빌드(임시 빌드는 배포 안 함) |
| 기한 후 서비스 / 화면 | 로컬에서만 `LastWorkDay` 를 어제로 바꾼 임시 DSL 빌드 → 저장 호출 / DMS 실행 | 기한 문장 예외 / 키 미발급, Setup / Upload 메뉴 비활성, 조회 메뉴 정상, 제목 문장 |
| 동시 등록 | 등록 4개 상태에서 서로 다른 새 Device 2개를 GDS Import 로 동시에 Begin → 차례로 Complete | 둘 중 적어도 하나는 Complete 에서 막힘 / 등록 수 5 이하 |
| 서버 꺼짐 상태 시작 | 서비스 중지 후 DMS 실행 | 시작이 늦어지지 않음 / 메뉴 그대로 |

시험용으로 기한 / Device 수를 바꾸는 설정(config 등)은 만들지 않는다. 고객 환경에 남으면 그대로 우회 수단이 된다.

## 14. 확인 필요

- **고객 DB 가 우리가 전달하는 DB 인지**, 고객이 이미 쓰는 DACrux DB 에 설치하는지. 기존 DB 라면 `TQP_MAPDEF` 에 고객의 기존 Map 이 있어 처음부터 5개를 넘을 수 있다(그 경우 `TQP_MAPDEF` 를 세는 방법을 다시 정한다).
- 연구소가 확정할 POC 대상 Device 목록(RnR 운영 환경 구성 단계) — GDS / Map 복사 / E-Test 이름을 합쳐 5개 안에 드는지. 예: `U9` / `OMM` / `OMM_Hybrid` / `OMM_TCB` = 4개.
- 기존 DMS 조회 화면(Defect Map Viewer 등) 안의 저장 기능 — 이번 설계는 화면 단위 Report 로 보고 막지 않는다.
- 정식 License 방식(10.2): 기존 Middleware License 의 발급 방법 / 회사 표준 여부.

## 15. 확인 수준

- 소스 확인: 메뉴(2026-10-19 `MenuFile.mnu`) / 클릭 경로, 기존 License 코드, Middleware 공개 함수 / `LicenseInfo`(리플렉션), 화면별 서비스 쓰기 호출(grep), 프로젝트 참조, DSL 배포 위치, 서비스 config 의 CONNECT_ID, 화면 오류 표시, Probe 의 임시 Device 이름.
- 로컬 DB 조회(읽기만, 2026-10-19): Device 가 들어가는 5개 표의 이름 목록 / 합집합 21개, `TQP_MAPDEF` 컬럼 / INSERT_TIME, `SYS_CONTEXT('USERENV','DBID')` / `V$DATABASE`.
- 빌드 / Probe / 화면 확인 없음. 소스 / DB 변경 없음.
