# POC License 설계 (Setup 3개월 사용 / Report 계속 조회 / 정식 License 확장 구조)

작성: 2026-10-14 Claude (CLAUDE-089) / 상태: **설계 v2 — L1 확정, 결정 대기 L2 ~ L5** (소스 / DB 변경 없음)

사용자 지시 (2026-10-14):
- "POC에서는 3개월간 Setup 관련된 기능은 활성화하고 report는 그냥 계속 볼수 있도록 해야 한다."
- "POC 이기 때문에 소스상에서 Limit을 걸어야 한다."
- "나중에 개발이 완료 되고 해당 기능을 넣어야 하니 미리 설계를 잘 해놓자."
- (v1 검토 후) "L1 : 1월 말까지."
- (v1 검토 후) "여기 IT 팀이 DACrux를 할 줄 알기 때문에 소스에 해당 기능을 녹여야 한다."
- (v1 검토 후) "DB에서 현재 일자를 확인하는 방법으로 해야 한다. PC에서 시간을 바꿀수 있다."
- (v1 검토 후) "service에서 확인해서 일자가 충족이 되면 다음 step으로 넘어가는 방식이 좋을것같다."

근거: 2026-10-02 상견례 녹취 — POC 결과물을 3개월 정도 사용해 본 뒤 확장하려면 실제 라이선스를 구매하는 흐름으로 안내함 (`회의록/2026-10-02_POC_상견례_및_업무협의/녹취원문.txt` 451행).

수정 이력:
- v1 (2026-10-14): 공용 DLL(SEMDMS.Interface)에 판정 클래스, 메뉴 / 화면은 PC 시간, 서버는 DB 시간.
- **v2 (2026-10-14, 이 문서)**: 기한 = **2027-01-31 까지**. **PC 시간 판정 전부 제거** — 판정은 서비스(서버)만 하고 시간은 DB 일시만 쓴다. 판정 코드를 서버 전용 DLL(`DACrux.SEMDMS.DSL`)의 업무 흐름 단계로 넣고, DB 일시 SQL 도 Query XML 이 아닌 컴파일된 코드에 둔다. 저장 흐름은 "1단계 일자 확인 → 통과해야 다음 단계" 로 바꾼다. 클라이언트는 서비스가 준 결과로 메뉴만 막는다(L5).

---

## 1. 결론

1. 기능을 **Report(조회) / Setup(기준정보 저장) / Upload(데이터 적재)** 로 나눈다. Setup / Upload 는 **DB 일시 기준 2027-01-31 까지**(2027-02-01 00:00:00 부터 차단), Report 는 기한 없이 허용한다.
2. 판정은 **서비스(서버)에서만** 한다. 시간은 **DB 일시**(`SYSTIMESTAMP`, 한국 시간으로 변환)만 쓴다. PC 시간은 어디에도 쓰지 않는다.
3. 서비스의 저장 흐름 **첫 단계가 일자 확인**이다. 통과해야 다음 단계(작업 키 발급 / 저장)로 넘어간다. 여러 번 나눠 부르는 흐름(Begin → 배치 → Complete)은 Begin 에서 키를 받지 못하면 다음 단계를 진행할 수 없고, Complete 에서도 한 번 더 확인한다.
4. **소스에 녹인다**: 기한 / 판정 / DB 일시 SQL 은 모두 서버 전용 DLL(`DACrux.SEMDMS.DSL`) 안 컴파일된 코드에 있다. 설정 파일 / `MenuFile.mnu` / Query XML / DB 테이블 / License 파일 어디에도 기한이나 끄는 스위치가 없다. 클라이언트 PC 에는 판정 코드가 배포되지 않는다.
5. 클라이언트는 시작할 때 서비스에 한 번 물어 Setup / Upload 메뉴를 막는다(L5). 이 확인은 안내용이고, 실제 차단은 3번의 서비스 단계다.
6. 정식 License 는 같은 단계(`WorkPeriod.Confirm`)의 **기한 출처만** 상수에서 License 정보로 바꾼다(9절). 서비스 함수의 첫 단계 호출 / 메뉴 목록은 그대로 쓴다.

```text
 클라이언트(PC 시간 안 씀)                         서버 Service (판정은 여기만)
 ─────────────────────────                       ─────────────────────────────────────────
 시작 → GetWorkPeriod() ───────────────────────▶  WorkPeriod.GetState()  ── DB 일시
      ◀── 기간 안 / 밖 ── 메뉴 막기(안내용)

 저장 버튼 → BeginXxx() ───────────────────────▶  1단계 WorkPeriod.Confirm()  ── DB 일시
                                                   ├ 기간 밖 → 예외(키 없음) → 흐름 끝
                                                   └ 기간 안 → 2단계 작업 키 발급 / 저장
           → 배치 ... → CompleteXxx() ─────────▶  1단계 Confirm() → 2단계 확정
```

## 2. 현재 소스 확인 결과

| 항목 | 확인 내용 | 설계 반영 |
| --- | --- | --- |
| 기존 License | `06_CoreLibSource/DACrux.Framework.BSL/License.cs` → `Miracom.Middleware.BaseComponent.ChkLicense(IP)`. 로그인 / 메인 Load 호출은 주석 처리(미사용). Middleware 4.0.0.7 에 `GetLicense(파일, 키)` → `LicenseInfo { ServerIP, DueDate, Users }` 와 설정 키 `LICENSEFILE` / `LICENSEKEY` / `CLIENTLICENSE` 가 있으나 서버 / 클라이언트 설정 어디에도 없음 | POC 에서는 쓰지 않음. 정식 때 기한 출처 후보(9.2) |
| 메뉴 클릭 | Core `DACruxMain.utbManager_ToolClick` 이 `MenuFile.mnu` 행으로 화면을 만든다. 취소할 수 있는 확장 지점 없음. Tool Key = `MENU_KEY`, `SharedProps.Enabled`(`MenuHelper.cs:136`) | 클라이언트는 메뉴 Tool 만 비활성(L5) |
| 고객이 바꿀 수 있는 것 | `MenuFile.mnu`(ENABLE / VISIBLE), 서비스 config, `08_DBQuery/ORACLE/*.xml`(서버 폴더의 텍스트), DB 권한 표(FUNCTION_CODE) | 기한 / 판정을 이 중 어디에도 두지 않는다 |
| DB 일시 조회 | 기존 `TQC_LOT_STS.xml` 의 `SELECT_SYS_DATE` 는 XML 이라 고정 날짜로 바꿀 수 있다. Middleware `QueryComponent.GetDataTable(ConnectID, Query)` 로 **코드 안 SQL** 실행 가능 | 코드 안 SQL 로 조회 |
| 서버 전용 DLL | `DACrux.SEMDMS.DSL` 은 `101_ServerBin` 에만 있고 `100_ClientBin` 에 없음. SEMDMS.BSL / TEST.BSL 이 둘 다 참조. 두 서비스 config 모두 `DMS_CONNECT_ID=DMSMGR` | 판정 클래스 위치 |
| 저장 흐름 | GDS Import / Chain / E-Test / KLARF 는 Begin → 배치 → Complete(Commit) 단계, Limit / Sub Device / Map 삭제는 한 번 호출 | 첫 단계 = 일자 확인 |
| 조회 화면 안 저장 | GDS Map View 에 Chain DB 저장(`BtnChainDbSave_Click`, 안에서 `DeleteChain`) / Map 삭제 / Sub Device 저장. 오류는 `MessageBox.Show(ex.Message)` 로 표시(`GdsMapViewForm_ChainDb.cs:174`) | 화면 코드 수정 없이 서비스 문장이 그대로 보임 |
| 클라이언트 → 서비스 | RO 가 코드에서 URL 을 만들고(`RemoteConfig.url` + objectUri), 서버 config 에 wellknown 1줄. 기존 POC 서비스에 함수를 더하면 config 변경 없음 | 클라이언트 확인 함수는 기존 `iGdsMapView` 에 추가(L5) |

## 3. 기능 분류

### 3.1 메뉴 / 화면 / 서비스 단계

목록에 없는 메뉴는 Report 로 본다. 새 Setup / Upload 화면을 만들면 **서비스 저장 함수 첫 단계 + 메뉴 목록**에 함께 등록한다(공통 개발 지침에 규칙 추가, 11절 5단계).

| 메뉴 (MENU_KEY) | 화면 | 종류 | 모듈 | 서비스 첫 단계 확인 함수 |
| --- | --- | --- | --- | --- |
| GDS Map Setup (`MNU_DMS_GDS_MAP_SETUP`) | frmGdsMapSetup → GdsMapForm | Setup | Gds | `GdsMapImport.BeginMapImport` / `CompleteMapImport` / `DeleteMap` / `SaveSubDeviceList` |
| GDS Map View (`MNU_DMS_GDS_MAP_VIEW`) | frmGdsMapView → GdsMapViewForm | **Report** (안의 저장 동작만 Setup) | Gds | `GdsChain.BeginChainSave` / `CompleteChainSave` / `DeleteChain`, `GdsMapImport.DeleteMap` / `SaveSubDeviceList` |
| KLARF File Upload (`MNU_KLARF_UPLOAD`) | frmKlarfUpload | Upload | Klarf | `KlarfUpload.BeginUpload` / `CommitUpload` / `DeleteUploadedStep` |
| E-Test Result Upload (`MNU_DMS_ET_RESULT_UPLOAD`) | frmEtResultUpload | Upload | ETest | `GdsEtResult.BeginEtFile` / `CompleteEtFile` / `DeleteEtFile` / `MergeEtItem` (+ 설계된 `PublishEtFileToYms`) |
| E-Test Limit Setup (`MNU_DMS_ET_LIMIT`) | frmEtLimitSetup | Setup | ETest | `GdsEtResult.SaveEtLimit` / `DeleteEtLimit` / `CreateEtChainGroups` / `DeleteEtChainGroup` |
| File Map Setup (`MNU_STANDARD_MAP_FILE`) | frmStandardMapSetup | Setup | WaferMap | TEST `ProbeAdmin.SetMapDef` / `CreateUseMap` / `DeleteMapDef` — **기존 함수(L4)** |
| Wafer Map View / GDS-KLARF Analysis / DMS Analysis 4개 / (숨김) GDS Die Analysis | | Report | | 없음 |
| Setup > DM 3개 (Defect Coloring / Group / Type Define) | 기존 DMS 화면 | Setup | Dms | 없음 — 기존 화면 / 기존 함수(L3) |
| (설계 중) E-Test Para Setup (CLAUDE-086) / Die 좌표 대응 Setup (CLAUDE-081) | 신규 예정 | Setup | ETest / WaferMap | 새 저장 함수 첫 단계 |

### 3.2 GDS Map View 안의 동작

| 동작 | 기한 후 |
| --- | --- |
| Map 조회 / Layer / Label 검색 / Chain 목록 / Chain 추적 / 유사 묶음 찾기 | 그대로 사용 |
| Chain 새로 만들기 / 이름 변경 / 추가 / 빼기 (메모리, 저장 안 함) | 그대로 사용 |
| Chain DB 저장 / Map 삭제 / Sub Device 저장 | 서비스 첫 단계에서 막힘 → 화면이 기존 오류 창으로 서비스 문장을 보여 준다 |

화면 코드는 고치지 않는다(v1 의 클릭 전 확인은 PC 시간을 써서 제거).

### 3.3 서비스 함수 중 확인하지 않는 것

| 함수 | 이유 |
| --- | --- |
| `Get*` / `AnalyzeWafer` 등 읽기 | Report |
| `CancelUpload` / `CancelImportJob` | 실패 작업 정리. 막으면 미완료 작업이 남는다 |
| 배치 중간 함수 `CreateLayerBatch` / `CreatePlacedBatch` / `CreateChainMemberBatch` / `CreateChainEditBatch` / `CreateEtDieBatch` / `CreateEtValueBatch` / `AddImage` | Begin 에서 받은 작업 키가 있어야만 부를 수 있고, Complete 에서 다시 확인하므로 결과가 확정되지 않는다. 호출 수가 많아 매번 DB 일시를 읽지 않는다 |

기한 직전에 시작한 작업이 기한 뒤에 Complete 를 부르면 확정되지 않고 미완료로 남는다. 기존 미완료 작업 처리(취소 / 재시도)와 같다.

## 4. 사용 기한과 DB 일시

| 항목 | 내용 |
| --- | --- |
| 마지막 날 | **2027-01-31** (L1 확정). DB 일시 2027-02-01 00:00:00 부터 Setup / Upload 차단 |
| 판정 | `FirstValidDay(2026-10-01) <= DB 일시 < 2027-02-01` 이면 통과 |
| DB 일시 | `SELECT TO_CHAR(SYSTIMESTAMP AT TIME ZONE '+09:00', 'YYYYMMDDHH24MISS') FROM DUAL` — 코드 안 SQL. DB 서버 시간대 설정과 관계없이 한국 시간 |
| `SYSTIMESTAMP` 를 쓰는 이유 | Oracle `FIXED_DATE` 설정은 `SYSDATE` 만 고정한다. DBA 가 `SYSDATE` 를 과거로 고정해도 영향이 없다(구현 때 로컬 DB 에서 확인) |
| 하한(`FirstValidDay`) | DB 일시가 2026-10-01 보다 앞서면 시각이 잘못된 것으로 보고 막는다 |
| DB 일시 조회 실패 | 막는다. DB 가 안 되면 저장도 어차피 실패한다. 서버 PC 시간으로 대신하지 않는다 |
| 기한 변경(연장) | 상수 변경 → `DACrux.SEMDMS.DSL` 다시 빌드 → `101_ServerBin` 배포 + SEMDMS / TEST 서비스 재시작. 클라이언트 재배포는 필요 없다 |

시작일을 첫 실행일로 저장해 계산하는 방식은 쓰지 않는다. 저장 위치(DB / 레지스트리)를 지우면 기간이 다시 시작된다.

## 5. 소스에 녹이는 방법

| 원칙 | 방법 |
| --- | --- |
| 바꿀 수 있는 곳에 두지 않는다 | 기한 / 판정 / DB 일시 SQL 을 설정 파일 · `MenuFile.mnu` · Query XML · DB 테이블 · License 파일에 두지 않는다. 컴파일된 서버 DLL 안에만 있다 |
| 떼어 낼 수 있는 부품으로 만들지 않는다 | 별도 DLL(예: `DACrux.License.dll`)을 만들지 않는다. 같은 이름의 빈 DLL 로 바꿔치기하면 끝나기 때문이다. 업무 쿼리가 모여 있는 `DACrux.SEMDMS.DSL` 안 클래스로 둔다 |
| 클라이언트에 두지 않는다 | DSL 은 `101_ServerBin` 에만 배포된다. 클라이언트 PC 의 DLL 을 고쳐도 판정에 영향이 없다 |
| 업무 흐름의 한 단계로 둔다 | 저장 함수의 첫 단계 "작업 일시 확인" 으로 부른다. 통과해야 작업 키 발급 / 저장 단계로 넘어간다 |
| 이름 | 코드 이름은 업무 용어(`WorkPeriod` / `Confirm` / `WorkModule` / `WorkKind`)를 쓴다. 디컴파일 결과에서 "License" 로 검색해 바로 찾지 못하게 한다. 설명 주석은 공통 개발 지침대로 남긴다(주석은 DLL 에 들어가지 않는다) |

## 6. 서버 설계

### 6.1 판정 클래스 (`02_DMS/DACrux.SEMDMS.DSL/WorkPeriod.cs`, 신규)

```csharp
namespace DACrux.SEMDMS.DSL
{
    /// 작업 모듈. 정식 License 에서 모듈별 기한을 줄 때 그대로 쓴다. POC 는 모듈과 관계없이 같은 기한.
    public enum WorkModule { Dms, Gds, Klarf, ETest, WaferMap }

    /// 작업 종류. 조회는 확인하지 않으므로 저장 종류만 둔다.
    public enum WorkKind { Setup, Upload }

    /// 서비스 저장 흐름의 첫 단계. DB 일시를 읽어 작업 가능 기간이면 그 일시를 돌려주고,
    /// 기간 밖이면 예외로 흐름을 멈춘다. 일시 SQL 은 Query XML 이 아닌 이 코드에 둔다.
    public class WorkPeriod : Miracom.Middleware.QueryComponent
    {
        static readonly DateTime LastWorkDay = new DateTime(2027, 1, 31);   // 이 날까지 작업 가능
        static readonly DateTime FirstValidDay = new DateTime(2026, 10, 1); // 이보다 앞선 DB 일시는 잘못된 시각

        /// 1단계: DB 일시 확인. 통과하면 DB 일시를 돌려준다.
        public DateTime Confirm(WorkModule module, WorkKind kind);

        /// 클라이언트 안내용: DB_TIME / LAST_DAY / SETUP_OPEN / UPLOAD_OPEN / DAYS_LEFT 한 행.
        public DataTable GetState();

        /// 판정만 하는 함수(시험용으로 DB 일시를 넣어 확인).
        internal static bool IsOpen(WorkModule module, WorkKind kind, DateTime dbNow);

        /// SYSTIMESTAMP AT TIME ZONE '+09:00' 을 GetDataTable(DMS_CONNECT_ID, SQL) 로 조회. 실패하면 예외.
        DateTime ReadDbNow();
    }
}
```

- 막힐 때 문장: `POC 사용 기간(2027-01-31 까지)이 지나 저장할 수 없습니다. 조회 화면은 계속 사용할 수 있습니다.` — `InvalidOperationException`. 기존 BSL 이 던지는 예외와 같은 계열이라 기존 화면 오류 표시가 그대로 동작한다.
- 시각 오류 / 조회 실패 문장: `서버 일시를 확인할 수 없어 저장할 수 없습니다.` (원인은 서버 로그에만)
- 구현 때 확인: `GetDataTable(ConnectID, Query)` 가 `InitQueryComponent` 없이 동작하는지 Probe 로 먼저 본다. 필요하면 생성자에서 기존 XML 로 초기화만 하고 SQL 은 코드 문자열을 그대로 쓴다.

### 6.2 서비스 흐름 (첫 단계 = 일자 확인)

```csharp
public DataTable BeginEtFile(string[] FileInfo)
{
    // 1단계: 작업 일시 확인. 기간 밖이면 여기서 끝나고 작업 키가 만들어지지 않는다.
    new DACrux.SEMDMS.DSL.WorkPeriod().Confirm(WorkModule.ETest, WorkKind.Upload);

    // 2단계: 기존 처리 (인자 검사 → 파일 행 생성 → 작업 키 반환)
    ...
}
```

| 흐름 | 1단계 확인 | 다음 단계 |
| --- | --- | --- |
| KLARF Upload | `BeginUpload` / `CommitUpload` | 키 발급 → 이미지 추가 → 반영 |
| E-Test Upload | `BeginEtFile` / `CompleteEtFile` (+ `MergeEtItem`) | 파일 행 → Die / Value 배치 → 확정 |
| GDS Import | `BeginMapImport` / `CompleteMapImport` | 작업 키 → Layer / Placed 배치 → 확정 |
| GDS Chain 저장 | `BeginChainSave` / `CompleteChainSave` | 작업 키 → Member / Edit 배치 → 확정 |
| 한 번 호출 | `SaveEtLimit` / `DeleteEtLimit` / `CreateEtChainGroups` / `DeleteEtChainGroup` / `DeleteMap` / `SaveSubDeviceList` / `DeleteChain` / `DeleteEtFile` / `DeleteUploadedStep` | 저장 / 삭제 |
| File Map (L4) | `ProbeAdmin.SetMapDef` / `CreateUseMap` / `DeleteMapDef` | 저장 |

- 확인은 인자 검사보다 앞, DB 작업 전에 둔다. 읽기 쿼리 1번이 늘 뿐 트랜잭션 / 저장 흐름은 바뀌지 않는다.
- TEST.BSL 은 이미 SEMDMS.DSL 을 참조하므로 참조 추가 없이 같은 클래스를 부른다(L4).

## 7. 클라이언트 (안내용, L5)

판정은 하지 않는다. 서비스가 준 결과로 메뉴만 막는다.

1. `iGdsMapView` / RO / BSL 에 읽기 함수 `GetWorkPeriod()` 1개 추가 → BSL 은 `WorkPeriod.GetState()` 결과를 그대로 반환. 기존 wellknown 을 쓰므로 config 변경 없음.
2. `01_WinApp/DACruxV5/MenuWorkPeriod.cs`(신규): Setup / Upload 메뉴 키 목록 + 적용.
   - Main 이 보인 뒤 **백그라운드로** `GetWorkPeriod()` 호출(서버가 꺼져 있어도 시작이 늦어지지 않게) → 결과가 오면 UI 스레드에서 막힌 메뉴의 `SharedProps.Enabled = false` + 제목 문장.
   - 호출 실패(서버 꺼짐 / 옛 서버) → 아무것도 바꾸지 않는다. 저장은 서비스가 막는다.
   - 제목 문장: D-14 부터 `[Setup 사용 기한 2027-01-31, 7일 남음]`, 기한 후 `[Setup 사용 기간 종료 - 조회 전용]`.
3. `DACruxClientMain.cs`: DEBUG / Release 두 곳에서 `ApplyMenuSize(mMain)` 다음에 `MenuWorkPeriod.Attach(mMain)` 1줄.

메뉴 목록:

| MENU_KEY | 종류 |
| --- | --- |
| `MNU_DMS_GDS_MAP_SETUP` / `MNU_DMS_ET_LIMIT` / `MNU_STANDARD_MAP_FILE` | Setup |
| `MNU_KLARF_UPLOAD` / `MNU_DMS_ET_RESULT_UPLOAD` | Upload |
| `MNU_DEFECT_COLORING` / `MNU_DEFECT_GROUP_DEFINE` / `MNU_DEFECT_TYPE_DEFINE` | Setup (L3) |

메뉴 파일 ENABLE 이 이미 False 인 메뉴는 켜지 않는다. 막는 쪽으로만 바꾼다.

## 8. 우회 가능성과 한계

| 방법 | 결과 |
| --- | --- |
| PC 시간 변경 | 영향 없음(PC 시간을 쓰지 않음) |
| `MenuFile.mnu` 수정 / 옛 클라이언트 / `NexplantQMS.GdsMap.exe` 단독 실행 | 화면은 열려도 서비스 첫 단계에서 막힌다 |
| 서비스 config 수정 | 기한 관련 키가 없다. `DMS_CONNECT_ID` 를 바꾸면 업무 쿼리 전체가 같이 멈춘다 |
| Query XML 수정(`SELECT_SYS_DATE` 고정 등) | 영향 없음(일시 SQL 은 코드 안) |
| 서버 서비스 PC 시간 변경 | 영향 없음(DB 일시만 사용) |
| DBA 가 `FIXED_DATE` 로 `SYSDATE` 고정 | 영향 없음(`SYSTIMESTAMP` 사용, 구현 때 확인) |
| DB 서버 OS 시간을 과거로 변경 | 2026-10-01 이전이면 막힌다. 그 사이 날짜는 막지 못한다. 같은 DB 를 쓰는 다른 업무가 함께 틀어지는 조작이라 받아들인다 |
| 서버 DLL 디컴파일 후 수정(dnSpy 등) | 막지 못한다. 설정 / 파일 수정이 아니라 프로그램 변조이고, 소스 머리말의 저작권 조항 위반이다. 난독화 도구는 Remoting 형식 이름을 바꿀 위험이 있어 POC 에서는 쓰지 않는다 |

## 9. 정식 License 확장 (나중에 넣을 때)

### 9.1 바꾸는 것 / 그대로 두는 것

| 구분 | POC | 정식 |
| --- | --- | --- |
| 기한 출처 | `WorkPeriod` 안 상수 | License 정보(9.2) — `WorkPeriod` 내부만 변경 |
| 서비스 첫 단계 호출 / `GetWorkPeriod()` / 메뉴 목록 | 6 ~ 7절 | **그대로** |
| 기준 시간 | DB 일시 | DB 일시 (+ 9.3 되돌림 감지) |
| 모듈별 기한 | 모두 같음 | `WorkModule` 별로 다르게 — 호출 코드는 이미 모듈을 넘기고 있음 |

### 9.2 License 정보 출처 (정식 개발 때 결정)

- **후보 1 — 기존 Middleware License**: `BaseComponent.GetLicense(파일, 키)` → `LicenseInfo { ServerIP, DueDate, Users }`, 설정 키 `LICENSEFILE` / `LICENSEKEY` / `CLIENTLICENSE`. 회사 공통 방식이면 발급 도구 / 키 관리가 이미 있을 수 있다. 다만 모듈별 기한은 없다(DueDate 1개). 발급 방법과 파일 형식은 프레임워크 담당에게 확인 필요.
- **후보 2 — 서명된 License 파일**: 서버의 XML 파일에 고객 / Site / 설치 대상(DB 이름 또는 서버 이름) / 모듈 x 종류별 마지막 날 / 사용자 수를 넣고, 미라콤 개인 키로 서명(RSA-SHA256), 서버 DLL 의 공개 키로 검증. 기한 연장은 파일만 다시 발급한다.
- 어느 쪽이든 서명 / 파일이 없거나 틀리면 Report 만 허용한다.

### 9.3 그 밖에 정식 때 검토할 것

- 시간 되돌림 감지: 마지막으로 본 DB 일시를 저장해 두고 그보다 과거면 막는다.
- PC / 동시 사용자 제한: Middleware `ChkLicense(IP)` / `LicenseInfo.Users` 와 합칠지 검토.
- 확인 지점을 원격 호출 공통 지점(서버 채널 Sink)으로 모으는 방법은 함수 이름 기반이라 디버깅이 어렵다. 함수 첫 단계 방식을 유지하는 것을 권장한다.

## 10. 결정

| ID | 내용 | 상태 / 권장 | 대안 |
| --- | --- | --- | --- |
| L1 | 사용 기한 | **확정: 2027-01-31 까지** (사용자 2026-10-14) | - |
| L2 | KLARF / E-Test **Upload** 도 기한 대상 | 권장: 예 — 기한 후 새 데이터 적재는 운영 사용. 기한 전 데이터는 계속 조회 | Upload 는 기한 없이 허용 |
| L3 | 기존 DMS Setup 3개 메뉴(Defect Coloring / Group / Type Define) | 권장: 메뉴만 막는다(서비스 결과 기준, 기존 화면 / 함수 수정 없음) | 제외(계속 사용) |
| L4 | File Map Setup — TEST `ProbeAdmin.SetMapDef` / `CreateUseMap` / `DeleteMapDef` 는 **기존 함수**(옛 frmSetupMap 도 사용) | 권장: 기존 함수 첫 단계에 확인 1줄 추가(기존 함수 변경 — 승인 필요) | File Map Setup 화면 저장 전에 `GetWorkPeriod()` 확인(기존 함수 수정 없음, 옛 화면 경로는 못 막음) |
| L5 | 클라이언트 메뉴 막기 | 권장: 시작 후 백그라운드로 `GetWorkPeriod()` 1번 → 메뉴 비활성 + 제목 문장 | 메뉴는 그대로 두고 저장 때 서비스 문장만 |
| ~~L6~~ | ~~GDS View 클릭 전 확인~~ | v2 에서 제거 — PC 시간 판정이라 지시와 맞지 않음. 서비스 문장으로 대신 | |
| ~~L7~~ | ~~판정 클래스 위치~~ | v2 에서 정함 — 서버 전용 `DACrux.SEMDMS.DSL`(5절) | |

## 11. 구현 단계 (결정 후)

CLAUDE-087(진행 중)이 `DACrux.SEMDMS.DSL.csproj` / `DACrux.SEMDMS.BSL.csproj` / `DACrux.SEMDMS.Interface.csproj` 와 BSL 파일을 수정 중이다. CLAUDE-087 종료 후 또는 작업 현황에서 범위를 나눈 뒤 진행하고, 편집 직전에 파일을 다시 읽는다.

1. **판정 단계**: `SEMDMS.DSL/WorkPeriod.cs` + DSL csproj Compile 1행 + Probe `WorkPeriodProbe.ps1`(① 코드 안 SQL 로 DB 일시 조회 ② `IsOpen` 에 일시를 넣어 판정).
2. **서비스 첫 단계**: SEMDMS.BSL 4개 파일(`GdsMapImport` / `GdsChain` / `GdsEtResult` / `KlarfUpload`) 6.2 표 함수 첫 줄. L4 채택 시 TEST.BSL `ProbeAdmin` 3함수.
3. **클라이언트 안내(L5)**: `iGdsMapView` / RO / BSL `GetWorkPeriod()` + `DACruxV5/MenuWorkPeriod.cs` + `DACruxClientMain.cs` 2곳 + DACruxV5 csproj(SEMDMS.RO / Interface 참조, Compile 1행).
4. **배포**: DSL / BSL / Interface → `101_ServerBin`, RO / Interface / DACruxV5 → `100_ClientBin`, SEMDMS / TEST 서비스 재시작.
5. **문서**: 공통 개발 지침에 "새 Setup / Upload 저장 함수 = 첫 단계 `WorkPeriod.Confirm` + 메뉴 목록 등록" 규칙 / 빌드 가이드에 기한 변경 절차.

## 12. 검증 기준 (기대값)

| 항목 | 방법 | 기대값 |
| --- | --- | --- |
| DB 일시 | Probe 에서 `ReadDbNow` | 로컬 DB 의 `SYSTIMESTAMP` 한국 시간과 같음 |
| 판정 | Probe 에서 `IsOpen` 에 일시 입력 | 2026-09-30 23:59:59 막힘 / 2026-10-01 00:00:00 통과 / 2027-01-31 23:59:59 통과 / 2027-02-01 00:00:00 막힘 |
| `FIXED_DATE` 영향 | 로컬 docker DB 에서만 `FIXED_DATE` 설정 → `ReadDbNow` → 원복 (DB 설정 변경이라 실행 전 사용자 확인) | `SYSDATE` 는 고정 날짜, `ReadDbNow` 는 실제 시각 |
| 첫 단계 누락 | 6.2 표 함수의 첫 줄을 소스 검사 | 누락 0 |
| 기한 전 회귀 | 기존 Probe / Smoke (KLARF / E-Test / GDS 저장) | 지금과 같은 결과 |
| 기한 후 서비스 | 로컬 환경에서만 `LastWorkDay` 를 어제로 바꾼 임시 DSL 빌드 → 저장 호출 | 서비스 문장 예외 / DB 변경 없음 / 키 미발급. 확인 후 상수 원복 · 재빌드(임시 빌드는 배포 안 함) |
| 기한 후 화면 | 같은 임시 빌드로 DMS 실행 | Setup / Upload 메뉴 비활성, 조회 메뉴 정상, 제목 문장, GDS View 저장 시 서비스 문장 |
| 서버 꺼짐 상태 시작 | 서비스 중지 후 DMS 실행 | 시작이 늦어지지 않음 / 메뉴 그대로 |

시험용으로 기한을 바꾸는 설정(config 등)은 만들지 않는다. 고객 환경에 남으면 그대로 우회 수단이 된다.

## 13. 확인 필요

- 기존 DMS 조회 화면(Defect Map Viewer 등) 안에 저장 기능이 있는지 — 이번 설계는 화면 단위 Report 로 보고 막지 않는다.
- 정식 License 방식(9.2): 기존 Middleware License 의 발급 방법 / 회사 표준 여부.
- 클라이언트 설치 파일 설계(CLAUDE-088)와 배포 순서 — 이번 변경은 대부분 서버 배포다.

## 14. 확인 수준

- 소스 확인: 메뉴 생성 / 클릭 경로, 기존 License 코드, Middleware 공개 함수 / `LicenseInfo` (리플렉션), 화면별 서비스 쓰기 호출(grep), 프로젝트 참조, DSL 배포 위치, 서비스 config 의 CONNECT_ID, 화면 오류 표시.
- 빌드 / Probe / 화면 / DB 확인 없음. 소스 / DB 변경 없음.
