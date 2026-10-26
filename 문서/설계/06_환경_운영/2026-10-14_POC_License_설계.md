# POC License 설계 (데이터 생성·수정 사용 기한 / Device 5개 / 조회 계속 / 정식 License 확장 구조)

작성: 2026-10-14 Claude (CLAUDE-089) / 갱신: 2026-10-19 v3, 2026-10-22 v4 · v4.1, 2026-10-23 구현(일자), **2026-10-26 v5 Device (CLAUDE-218)** / 상태: **일자 제한 구현 완료(16절, 빌드 · Probe 확인, 배포는 사용자) / **Device 5개 제한: 하지 않음(2026-10-26 사용자 "5limit은 하지 말자")** — 5절 v5 는 기록으로만 남김. 관련 규칙(GDS 에 없으면 오류 / GDS Map View [Map 삭제] 제거)도 하지 않음(같은 날 결정)**

사용자 지시:
- (10-14) "POC에서는 3개월간 Setup 관련된 기능은 활성화하고 report는 그냥 계속 볼수 있도록 해야 한다."
- (10-14) "POC 이기 때문에 소스상에서 Limit을 걸어야 한다."
- (10-14) "나중에 개발이 완료 되고 해당 기능을 넣어야 하니 미리 설계를 잘 해놓자."
- (10-14, v1 검토 후) "L1 : 1월 말까지."
- (10-14, v1 검토 후) "여기 IT 팀이 DACrux를 할 줄 알기 때문에 소스에 해당 기능을 녹여야 한다."
- (10-14, v1 검토 후) "DB에서 현재 일자를 확인하는 방법으로 해야 한다. PC에서 시간을 바꿀수 있다."
- (10-14, v1 검토 후) "service에서 확인해서 일자가 충족이 되면 다음 step으로 넘어가는 방식이 좋을것같다."
- (10-19) "등록되는 device는 5개 Limit이 있다. 소스상에서 5개까지만 등록 가능하도록 설계 진행"
- **(10-22) "현재 Open 된 화면들중에 Data 생성 및 수정 하는 화면들에 대해 27년 01월 31일 까지 사용할 수 있도록 기능 추가 해야 한다. 일반 조회 화면들은 그대로 둔다. 대상 화면 및 기능에 대한 조사 / 일자 Limit 처리할 설계"**
- **(10-22, 대상 화면 그림 검토 후) Setup 6개 / Data Upload 2개 / GDS Map View(Map 삭제 · 복사 · Chain 저장) 목록을 골라 "해당 화면만 대상으로 하는것으로 하자"**
- **(10-26) "디바이스는 5개 제한이다. 어느 화면에서 디바이스에 대한 등록을 Limit 을 어떻게 거는것이 좋겠나" -> "GDS에서 막아버리면 안되나? 나머지는 디바이스가 없으면 Error" -> "GDS Map 삭제 하고 다시 등록 하면 안되니. 삭제 기능은 제거. 중간에 멈춘거는 다시 Upload 시 등록 가능. 해당 부분도 설계안에 넣어줘"**

근거: 2026-10-02 상견례 녹취 — POC 결과물을 3개월 정도 사용해 본 뒤 확장하려면 실제 라이선스를 구매하는 흐름으로 안내함 (`회의록/2026-10-02_POC_상견례_및_업무협의/녹취원문.txt` 451행).

수정 이력:
- v1 (2026-10-14): 공용 DLL(SEMDMS.Interface)에 판정 클래스, 메뉴 / 화면은 PC 시간, 서버는 DB 시간.
- v2 (2026-10-14): 기한 2027-01-31 까지. PC 시간 판정 제거, 판정은 서버 전용 `DACrux.SEMDMS.DSL` 의 업무 흐름 단계, DB 일시 SQL 은 코드 안, 저장 흐름 "1단계 일자 확인 → 다음 단계".
- v3 (2026-10-19): 등록 Device 5개 제한(5절). 판정 클래스 `WorkPeriod` → `WorkRule`(기한 + Device 수).
- v4 (2026-10-22): 지금 메뉴에 열려 있는 **화면 23개 전수 조사**(3절) — 데이터를 만들거나 고치는 화면 8개 + 조회 화면 안의 쓰기 동작 4개 화면을 찾음(기존 DMS Chart Analysis / Defect Image Gallery 의 Reclassify · 이미지 삭제 포함). 서버 함수 목록을 지금 소스로 다시 맞춤(E-Test Bin 설정, Chain 일괄 함수, Defect Type 정의). 코드 안 SQL 실행 방법 확인(2절, 지난번 오류는 시험 방식 문제) / 로컬 DB OS 시간이 UTC 라 한국 시간 변환 SQL 확정(4절). 클라이언트는 메뉴 묶음 2개만 막도록 단순화(8절). L2 확정, L9 · L10 추가.
- v4.1 (2026-10-22): **대상 확정** — Setup 6개 + Data Upload 2개 화면 전체 + GDS Map View 의 Map 삭제 / Map 복사 / Chain DB 저장. E-Test Result View 파일 삭제 / Chart Analysis Reclassify / Defect Image Gallery Reclassify · 이미지 삭제는 **대상 아님**(L10 = 그대로 둠). 그래서 E-Test Result View 와 같이 쓰는 `DeleteEtFile` 은 서버에서 막지 않는다(7.2).
- **v5 (2026-10-26, 이 문서)**: Device 5개를 **GDS Map 등록에서만** 센다(GDS Map Setup 저장 / [Map 복사]). Wafer Map / KLARF / E-Test 저장은 GDS Map 에 그 Device 가 없으면 오류. GDS Map View [Map 삭제] 기능 제거(지우고 다시 등록 방지). 중간에 멈춘 DRAFT 도 1개로 세고 같은 이름 다시 Upload 는 통과. v3 의 "5개 표 어디서든 새 이름 확인" 안과 L6 ~ L8 은 이 안으로 대체(5절, 11절 D1 · D2).

---

## 1. 결론

1. 지금 열려 있는 화면 23개 중 **데이터를 만들거나 고치는 기능**만 **DB 일시 기준 2027-01-31 까지** 쓸 수 있게 한다(2027-02-01 00:00:00 부터 차단). **조회 화면과 조회 기능은 그대로 둔다** — 화면 코드를 고치지 않고, 화면 안의 저장 / 삭제 동작만 서버에서 막는다.
2. 대상은 **데이터 생성·수정 화면 8개**(Setup 6 + Upload 2)와 **GDS Map View 의 쓰기 동작 3개**(Map 삭제 / Map 복사 / Chain DB 저장)다(v4.1 확정). 조사에서 찾은 다른 조회 화면 안의 쓰기(E-Test Result View 파일 삭제 / Chart Analysis · Defect Image Gallery 의 Reclassify · 이미지 삭제)는 대상에서 뺀다. 나머지 11개 조회 화면은 DB 쓰기가 없다(3절).
3. 판정은 **서비스(서버)에서만** 한다. 시간은 **DB 일시를 한국 시간으로 바꾼 값**(`SYS_EXTRACT_UTC(SYSTIMESTAMP) + 9/24`)만 쓴다. PC 시간은 쓰지 않는다.
4. 서비스 저장 흐름의 **첫 단계가 `WorkRule.Confirm`** 이다. 통과해야 다음 단계(작업 키 발급 / 저장)로 넘어간다. Begin → 배치 → Complete 흐름은 Begin 에서 키를 받지 못하면 진행할 수 없고, Complete 에서 다시 확인한다.
5. **소스에 녹인다**: 기한 / 판정 / 조회 SQL 은 모두 서버 전용 DLL(`DACrux.SEMDMS.DSL`) 안 컴파일된 코드에 있다. 설정 파일 / `MenuFile.mnu` / Query XML / DB 테이블 어디에도 값이나 끄는 스위치가 없다.
6. 클라이언트는 시작 후 서비스에 한 번 물어, 기한이 지나면 **POC 메뉴 묶음 `Setup` / `Data Upload` 두 개만** 비활성으로 바꾼다(L5). 조회 화면 안의 저장 동작은 서버 문장으로 안내한다.
7. **Device 5개(v5, 5절)**: GDS Map 이 Device 등록부다. 5개 제한은 GDS 에 새 Device 가 생기는 GDS Map Setup 저장 / [Map 복사] 에서만 걸고, Wafer Map / KLARF / E-Test 저장은 GDS Map 에 그 Device 가 없으면 오류로 막는다. GDS Map View [Map 삭제] 는 없앤다. 같은 저장 첫 단계(`WorkRule.Confirm`)에 Device 이름을 넘겨 확인한다.

```text
 클라이언트(판정 안 함)                              서버 Service (판정은 여기만)
 ─────────────────────                           ─────────────────────────────────────────────
 시작 → GetWorkState() ───────────────────────▶  WorkRule.GetState()   ── DB 일시(한국 시간)
      ◀── 기간 안 / 밖 ── 메뉴 묶음 2개 막기(안내용, L5)

 저장 → BeginXxx() / SaveXxx() ───────────────▶  1단계 WorkRule.Confirm(모듈, 종류[, Device])
                                                   ├ 기한 밖 → 예외, 흐름 끝 (화면은 기존 오류 창에 문장 표시)
                                                   └ 통과   → 2단계 작업 키 발급 / 저장
      → 배치 ... → CompleteXxx() ─────────────▶  1단계 Confirm() → 2단계 확정
```

## 2. 현재 소스 확인 결과

| 항목 | 확인 내용 | 설계 반영 |
| --- | --- | --- |
| 기존 License | `06_CoreLibSource/DACrux.Framework.BSL/License.cs` → `Miracom.Middleware.BaseComponent.ChkLicense(IP)`. 로그인 / 메인 Load 호출은 주석 처리(미사용). Middleware 4.0.0.7 에 `GetLicense(파일, 키)` → `LicenseInfo { ServerIP, DueDate, Users }` 와 설정 키 `LICENSEFILE` / `LICENSEKEY` / `CLIENTLICENSE` 가 있으나 설정 어디에도 없음 | POC 에서는 쓰지 않음. 정식 때 값 출처 후보(10.2) |
| 메뉴 | 2026-10-22 16:10 `MenuFile.mnu`(소스 / `100_ClientBin` 같음): 보이는 화면 23개 = POC 12(묶음 `MNU_POC_SETUP` 6 / `MNU_POC_UPLOAD` 2 / `MNU_POC_VIEW` 4) + DMS Analysis 4 + TEST Analysis 7 | 3절 전수 조사 |
| 메뉴 클릭 | Core `DACruxMain.utbManager_ToolClick` 이 `MenuFile.mnu` 행으로 화면을 만든다. 취소할 수 있는 확장 지점 없음. Tool Key = `MENU_KEY`, `SharedProps.Enabled`(`MenuHelper.cs:136`). 묶음(Popup)도 Tool 이라 비활성으로 바꾸면 하위 메뉴를 열 수 없다 | 클라이언트는 묶음 2개만 비활성(L5) |
| 고객이 바꿀 수 있는 것 | `MenuFile.mnu`(ENABLE / VISIBLE), 서비스 config, `08_DBQuery/ORACLE/*.xml`(서버 폴더의 텍스트), DB 권한 표(FUNCTION_CODE), 로그인 Factory | 값 / 판정을 이 중 어디에도 두지 않는다 |
| 코드 안 SQL | Middleware `QueryComponent.GetDataTable(ConnectID, Query)` 로 Query XML 없이 SQL 실행. 2026-10-22 시험: **컴파일된 코드에서 부르면 정상**(InitQueryComponent 없이도 됨, SQL 안 `:` 도 문제 없음). 10-21 의 NullReferenceException 은 PowerShell 이 한 줄짜리 메서드를 직접 부를 때 JIT 인라인으로 Middleware 가 읽는 호출 메서드 정보(`StackFrame` → `ReflectedType`)가 비어 생긴 시험 방식 문제 — `[MethodImpl(NoInlining)]` 이면 PowerShell 에서도 정상 | DB 일시는 코드 안 SQL. Probe 는 BSL 을 거치거나 NoInlining 메서드로 부른다 |
| DB OS 시간대 | 로컬 DB(`DMS`): `SYSDATE` / `SYSTIMESTAMP` = 08:18(UTC), 서버 PC = 17:18(KST). `SYS_EXTRACT_UTC(SYSTIMESTAMP) + 9/24` / `AT TIME ZONE '+09:00'` / `'Asia/Seoul'` 모두 17:18 | 한국 시간으로 바꿔 판정(4절). 바꾸지 않으면 로컬에서 9시간 늦게 막힌다 |
| 서버 전용 DLL | `DACrux.SEMDMS.DSL` 은 `101_ServerBin` 에만 있음. SEMDMS.BSL / TEST.BSL 이 둘 다 참조. 두 서비스 config 모두 `DMS_CONNECT_ID=DMSMGR` | 판정 클래스 위치 |
| 연결 | Middleware 는 쿼리마다 `OracleConnection` Open / Close, Pooling 기본값이라 세션은 Pool 에 남는다(2026-10-21 확인) | 첫 단계 조회 1번은 Pool 연결을 다시 쓰므로 부담이 작다 |
| 사용 기록 | 모든 메뉴 화면이 열 때 `TQC_USER_HITLOG` INSERT, 닫을 때 UPDATE(공통 부모 `DACruxUXBasic01`, Core) | 데이터 생성·수정 기능이 아니라 대상 아님(3.4) |
| 클라이언트 → 서비스 | RO 가 코드에서 URL 을 만들고 서버 config 에 wellknown 1줄. 기존 POC 서비스에 함수를 더하면 config 변경 없음 | 안내용 함수는 기존 `iGdsMapView` 에 추가(L5) |

## 3. 대상 화면 조사 (2026-10-22 메뉴 기준, 23개)

조사 방법: 화면 / 컨트롤 / 팝업이 부르는 RO 호출 → BSL → DSL → Query XML `statement` 까지 확인. 버튼 / 탭이 실제로 보이는지는 Designer 코드로 판단(실행 확인 안 함). DMS 4개 / YMS 7개는 같은 방법으로 별도 조사.

### 3.1 데이터 생성·수정 화면 — 8개 (화면 전체가 대상)

| 메뉴 묶음 | 메뉴 (MENU_KEY) | 화면 | 하는 일 | 서버 함수 (첫 단계 확인) | 쓰는 표 | 종류 / 모듈 |
| --- | --- | --- | --- | --- | --- | --- |
| Setup | GDS Map Setup (`MNU_DMS_GDS_MAP_SETUP`) | frmGdsMapSetup → GdsMapForm | GDS 파일을 읽어 DB 저장(같은 Device 는 지우고 다시 저장) | `GdsMapImport.BeginMapImport` / `CompleteMapImport` / `DeleteMap` | TQP_GDS_MAP / LAYER / PLACED_EL / IMPORT_* | Setup / Gds |
| Setup | Wafer Map Setup (`MNU_STANDARD_MAP_FILE`) | frmStandardMapSetup → MapCreationService | Wafer Map 정의 / 사용 Die 저장 | TEST `ProbeAdmin.SetMapDef` / `UpdateMapDef` / `CreateUseMap` — **기존 함수(L4)** | TQP_MAPDEF / TQP_USEMAP | Setup / WaferMap |
| Setup | E-Test Para Register (`MNU_DMS_ET_PARA_REG`) | frmEtParaRegister | 결과 파일 헤더를 Parameter 로 등록 / 삭제(삭제 때 업로드 파일도 지움) | `GdsEtResult.RegisterEtParameters` / `DeleteEtParameters` (`DeleteEtFile` 은 서버에서 막지 않음, 7.2) | TQP_GDS_ET_ITEM / SRC_FILE 등 | Setup / ETest |
| Setup | E-Test Para Setup (`MNU_DMS_ET_PARA`) | frmEtParaSetup | Parameter 설정 / Spec 저장 / 재판정 | `SaveEtParameterSetup` / `RegisterEtParameters` / `DeleteEtParameters` / `SaveEtLimit` / `RejudgeEtFiles` | TQP_GDS_ET_ITEM / ET_LIMIT / 판정 결과 | Setup / ETest |
| Setup | E-Test Bin Setup (`MNU_DMS_ET_BIN`) | frmEtBinSetup | YMS Bin 정의(이름 / 양품 / 색 / 설명) 저장 / 삭제 / 재판정 | `GdsEtResult.SaveEtBins` / `DeleteEtBins` / `RejudgeEtFiles` | TQP_BINDESC | Setup / ETest |
| Setup | Defect Type Define (`MNU_DEFECT_TYPE_DEFINE`) | frmDefectTypeDefine_Property (기존 DMS 화면) | Defect Type 추가 / 수정 / 삭제 | `SEMConfiguration.InsertDefectType` / `UpdateDefectType` / `DeleteDefectType` — **기존 함수(L9)** | TQD_DEFECT_TYPE | Setup / Dms |
| Data Upload | KLARF File Upload (`MNU_KLARF_UPLOAD`) | frmKlarfUpload | KLARF / 이미지 적재 | `KlarfUpload.BeginUpload` / `CommitUpload` (+ `DeleteUploadedStep`, 지금 화면 호출 없음) | TQD_* | Upload / Klarf |
| Data Upload | E-Test Result Upload (`MNU_DMS_ET_RESULT_UPLOAD`) | frmEtResultUpload | 결과 파일 적재 / 삭제 / YMS 반영 | `GdsEtResult.BeginEtFile` / `CompleteEtFile` / `PublishEtFileToYms` (`DeleteEtFile` 은 서버에서 막지 않음, 7.2) | TQP_GDS_SRC_FILE / ET_DIE / ET_VALUE / YMS TD_* | Upload / ETest |

L2 확정: "Data 생성 및 수정 하는 화면" 에 Upload 2개가 들어간다(2026-10-22 지시).

### 3.2 조회 화면 안의 쓰기 동작 — 조사 4개 화면, **대상은 GDS Map View 1개** (v4.1)

| 메뉴 | 화면 | 쓰기 동작 (보이는지) | 서버 함수 | 쓰는 표 | v4.1 결정 |
| --- | --- | --- | --- | --- | --- |
| GDS Map View (`MNU_DMS_GDS_MAP_VIEW`) | GdsMapViewForm | [Map 삭제] `mnuDeleteMap_Click` / [Map 복사] `MapCopyForm` / [Chain DB 저장] `BtnChainDbSave_Click`(안에서 `DeleteChain`) — 모두 보임 | `GdsMapImport.DeleteMap` / `CopyMap`, `GdsChain.BeginChainSave` / `CompleteChainSave` / `DeleteChain` | TQP_GDS_MAP 이하 / TQP_GDS_CHAIN 이하 | **대상** — 기한 후 서비스 문장 표시(기존 `MessageBox.Show(ex.Message)`), 조회 / 메모리 Chain 편집은 그대로. **[Map 삭제] 는 v5 에서 화면 기능 제거**(5.6) |
| E-Test Result View (`MNU_DMS_ET_RESULT_VIEW`) | frmEtResultView | [파일 삭제] — 보임 | `GdsEtResult.DeleteEtFile` | TQP_GDS_SRC_FILE 이하 / YMS | **대상 아님** — 기한 후에도 삭제 가능 |
| Chart Analysis (`MNU_CHART_ANALYSIS`, 기존 DMS) | frmChartAnalysisNew → DPUCChartAnalysisDefectList | RECLASSIFY 탭 [Apply] — 보임(`DPUCChartAnalysisDefectList.Designer.cs:483, 510-530`) | `DefectMapAnalysis.SetReclassifyDefectList_Comp` (기존 함수) | TQD_DEFECT.CLASSNUMBER / TQD_INSP_INFO.CLASSIFIED_DEFECTS (UPDATE) | **대상 아님**(L10) — 그대로 |
| Defect Image Gallery (`MNU_DEFECT_IMAGE_GALLERY`, 기존 DMS) | frmDefectImageGallery → DPUCImageGalleryGrid | Reclassify 탭 [Apply] / Remove Image 탭 [Delete] — 둘 다 보임(`DPUCImageGalleryGrid.Designer.cs:92-150, 207-215`) | `DefectMapAnalysis.SetReclassifyDefectImageList` / `SetRemoveDefectImageList` (기존 함수) | TQD_DEFECT / TQD_INSP_INFO (UPDATE), TQD_IMAGES (DELETE) + TQD_DATA_MAINT_HIS (INSERT) | **대상 아님**(L10) — 그대로 |

- 참고(조사 기록): 위 기존 DMS 기능은 권한 속성(`IsReclassifiedPermesion` / `IsImageDeletePermission`)이 있으나 부르는 곳이 없어 누구나 쓸 수 있다. 이미지 삭제는 DB 행만 지우고 FTP 원본은 지우지 않는다.

### 3.3 DB 쓰기가 없는 조회 화면 — 11개 (그대로 둠)

| 메뉴 | 화면 | 확인 내용 |
| --- | --- | --- |
| Wafer Map View | frmStandardMapView | 서버 쓰기 호출 없음 |
| GDS-KLARF-ET Correlation | frmGdsKlarfSimple | `GdsKlarfAnalysis.Analyze` / `GetWaferReady` / `GetMapDeviceList` 만. 서버 분석은 메모리 Cache 만 씀(GdsKlarf* BSL 에 DSL 쓰기 없음) |
| Defect Map Viewer | frmDefectMapViewer | Reclassify / RD 탭이 닫혀 있음(`DPUCDefecMapViewer.cs:1074-1082`, 2026-10-16 처리). 다시 여는 코드 없음 |
| Defect Map Gallery | frmDefectMapGallery | 읽기 RO 만. 옵션 / Zone 설정은 로컬 파일 |
| TEST Map View / Bin Map Gallery / Test Map Gallery / Wafer Cum Report / Wafer Bin Report / Yield Report / Param Analysis | YMS 7개 | 부르는 조회 RO 의 queryid 24개 모두 Select. 같은 RO 클래스의 쓰기 함수(`CreateParaSpec` / `UpdateMapConfig` 등)는 이 화면들에서 부르지 않음. Dut Analysis 버튼 / Recipe 저장은 숨김 |
| (검색 패널) DPUCStepSelect / TPUCSelectWafer | 메인 왼쪽 | 조회만. 조건 / Recipe 는 로컬 파일 |

### 3.4 대상에서 빼는 것

| 항목 | 이유 |
| --- | --- |
| 화면 열기 / 닫기 사용 기록(`TQC_USER_HITLOG`) | 데이터 생성·수정 기능이 아니라 사용 기록이다. 막으면 조회 화면이 열리지 않는다. Core(`DACruxUXBasic01`) 동작 |
| 로컬 파일 저장(검색 조건 / 화면 옵션 / Recipe / Excel / 이미지 임시 파일) | DB 데이터가 아니다 |
| `CancelUpload` / `CancelImportJob` | 실패 작업 정리. 막으면 미완료 작업이 남는다 |
| 배치 중간 함수 `CreateLayerBatch` / `CreatePlacedBatch` / `CreateChainMemberBatch` / `CreateChainEditBatch` / `CreateEtDieBatch` / `CreateEtValueBatch` / `AddImage` | Begin 에서 받은 작업 키가 있어야만 부를 수 있고 Complete 에서 다시 확인한다. 호출 수가 많아 매번 DB 를 읽지 않는다 |
| 읽기 함수(`Get*` / `Analyze*` / `GetWaferReady`) | 조회 |

### 3.5 메뉴에 없는 화면의 서버 쓰기 함수

화면이 메뉴에 없어도 서버 함수는 다른 경로(옛 화면 / 메뉴 파일 수정 / GDS EXE 단독 실행)로 부를 수 있으므로, **POC 서비스의 쓰기 함수는 모두** 첫 단계 확인을 넣는다: `GdsChain.CreateChainBulk` / `DeleteChainBulk` / `UpdateChainColorBulk`(GDS Map View U8, 메뉴 없음), `GdsEtResult.DeleteEtLimit`, `KlarfUpload.DeleteUploadedStep`. **예외: `GdsEtResult.DeleteEtFile`** — 대상 아닌 E-Test Result View 가 같이 쓰므로 막지 않는다(v4.1). 기존 제품 함수는 대상 화면이 쓰는 것만 다룬다(L4 / L9).

## 4. 사용 기한과 DB 일시

| 항목 | 내용 |
| --- | --- |
| 마지막 날 | **2027-01-31** (L1 확정). 한국 시간 2027-02-01 00:00:00 부터 데이터 생성·수정 차단 |
| 판정 | `FirstValidDay(2026-10-01) <= DB 한국 시간 < 2027-02-01` 이면 통과 |
| DB 일시 SQL | `SELECT TO_CHAR(SYS_EXTRACT_UTC(SYSTIMESTAMP) + 9/24, 'YYYYMMDDHH24MISS') KST, SYS_CONTEXT('USERENV','DBID') DB_ID FROM DUAL` — 코드 안 SQL. DB OS 시간대(로컬은 UTC)와 관계없이 한국 시간. 시간대 파일 / 이름이 필요 없다 |
| `SYSTIMESTAMP` 를 쓰는 이유 | Oracle `FIXED_DATE` 설정은 `SYSDATE` 만 고정한다. DBA 가 `SYSDATE` 를 과거로 고정해도 영향이 없다(구현 때 로컬 DB 에서 확인) |
| 하한(`FirstValidDay`) | DB 일시가 2026-10-01 보다 앞서면 시각이 잘못된 것으로 보고 막는다 |
| DB 조회 실패 | 막는다. 서버 PC 시간으로 대신하지 않는다 |
| 기한 변경(연장) | 상수 변경 → `DACrux.SEMDMS.DSL` 다시 빌드 → `101_ServerBin` 배포 + SEMDMS / TEST 서비스 재시작. 클라이언트 재배포 없음 |

## 5. 등록 Device 5개 (v5, 2026-10-26)

> **2026-10-26 결정: Device 5개 제한은 하지 않는다**(사용자 "권장안대로 개발 시작" 직후 "5limit은 하지 말자"). 같은 날 관련 규칙(Wafer Map / KLARF / E-Test 는 GDS 에 없으면 오류, GDS Map View [Map 삭제] 제거)도 하지 않기로 했다(5개 제한을 지키려고 만든 규칙이라 함께 뺌). 아래 v5 내용은 구현하지 않고 기록으로만 남긴다. 소스 변경 없음. POC 제한은 일자(2027-01-31)만 남는다.

사용자 결정(2026-10-26): "GDS 에서 막아버리면 안되나? 나머지는 디바이스가 없으면 Error" / "GDS Map 삭제 하고 다시 등록 하면 안되니. 삭제 기능은 제거" / "중간에 멈춘거는 다시 Upload 시 등록 가능". v3 안(5개 표 어디서든 새 이름이면 확인)은 이 방식으로 바꾼다.

### 5.1 원칙

| 구분 | 내용 |
| --- | --- |
| Device 등록부 | **GDS Map**(`TQP_GDS_MAP.DEVICE_ID`). 연관 분석이 이미 GDS Device 이름으로 KLARF Product(예 `OMM_TCB`) / E-Test Device(예 `OMM`)를 찾으므로 등록부를 GDS 하나로 둔다 |
| 5개 제한을 거는 곳 | GDS 에 **새 Device 가 생기는 곳만**: GDS Map Setup 저장 / GDS Map View [Map 복사] |
| 나머지 | Device 를 받는 저장은 **GDS Map 에 그 Device 가 없으면 오류** |
| 삭제 | **GDS Map View [Map 삭제] 기능 제거**. 한 번 등록한 Device 는 화면에서 지울 수 없다(지우고 다른 Device 로 다시 등록하는 것을 막기 위해) |
| 중간에 멈춘 저장 | DRAFT Map 도 등록 1개로 센다. 같은 이름으로 다시 Upload 하면 등록된 Device 라 통과한다(지금 동작: 다음 저장 때 기존 DRAFT 삭제 여부를 묻고 다시 올림) |
| 이름 비교 | `UPPER(TRIM())` — `OMM_Hybrid` = `OMM_HYBRID`. Factory 는 보지 않는다(Factory 를 새로 만들어 5개를 더 쓰는 것을 막기 위해) |
| 작업 순서 | GDS Map 을 먼저 등록해야 Wafer Map / KLARF / E-Test 를 등록할 수 있다. E-Test Device(`OMM`)는 U9 를 [Map 복사]해 `OMM` 을 만든다. 예: `U9` / `OMM` / `OMM_TCB` / `OMM_Hybrid` = 4개 |

### 5.2 함수별 처리

| 구분 | 화면 | 서버 함수 (첫 줄 `Confirm` 에 Device 를 넘김) | 처리 |
| --- | --- | --- | --- |
| **등록 (5개 확인)** | GDS Map Setup 저장 | `GdsMapImport.BeginMapImport`(Device = 저장 정보) / `CompleteMapImport`(작업의 Device, 다시 셈) | GDS 에 있는 이름(READY / DRAFT)이면 통과 = 다시 Upload. 없는 이름이면 등록 수 4개 이하일 때만 통과. Begin 에서 막으므로 도형을 올리기 전에 끝난다. Complete 에서 다시 세어 동시에 두 사람이 새 Device 를 올려 6개가 되는 경우를 막는다 |
| | GDS Map View [Map 복사] | `GdsMapImport.CopyMap`(Device = 새 이름) | 새 이름이 6번째면 막는다. 원본은 등록된 Device |
| **등록 Device 만 (없으면 오류)** | Wafer Map Setup | TEST `ProbeAdmin.SetMapDef` / `UpdateMapDef` / `CreateUseMap`(Device = Map ID) | GDS Map 에 없으면 오류 |
| | KLARF File Upload | `KlarfUpload.BeginUpload`(Device = Product) | GDS Map 에 없으면 오류(지금의 Wafer Map 필수 검사 앞) |
| | E-Test Para Register / Para Setup / Bin Setup / Result Upload | `GdsEtResult.RegisterEtParameters` / `SaveEtParameterSetup` / `SaveEtLimit` / `SaveEtBins` / `BeginEtFile`(Device = 키 / 파일 정보) | GDS Map 에 없으면 오류 |
| 확인 없음 | GDS Map Setup 같은 Device 다시 저장 전 삭제 | `GdsMapImport.DeleteMap` | 서버 함수는 남긴다(덮어쓰기 / Probe 정리용). 화면 메뉴만 없앤다. 기간 확인은 그대로 |
| | 나머지 저장 / 삭제 / 조회 | Chain 저장, Complete 류, 재판정, YMS 반영, 각종 삭제, 조회 | 이미 등록된 Device 의 데이터만 다루므로 Device 는 보지 않는다 |

### 5.3 세는 기준 (D1)

- 등록 수 = **GDS Map Device 이름**(READY / DRAFT 모두) **+ 그 이름으로 이미 저장된 Wafer Map / E-Test 데이터의 Device 이름**(`TQP_MAPDEF.MAPID` / `TQP_GDS_ET_ITEM` / `TQP_GDS_ET_LIMIT` / `TQP_GDS_SRC_FILE` 의 DEVICE_ID) 합집합.
- 이유: 화면 삭제는 없앴지만 (a) DB 에서 GDS Map 행을 직접 지우거나 (b) 같은 Device 다시 저장 중 삭제 직후 끊기면 GDS 에서 이름이 빠진다. 다른 데이터에 이름이 남아 있으면 계속 세어 자리가 비지 않게 한다. 새 규칙에서는 다른 표에 들어가는 이름도 모두 GDS 를 거쳐 등록된 이름이라 숫자는 같다.
- "GDS 에 없으면 오류" 확인은 GDS Map 만 본다(등록부).
- 대안: GDS Map 이름만 센다(더 단순, 위 (a)(b) 경우 자리가 빈다).

### 5.4 판정 / 문장

| 경우 | 결과 / 문장 |
| --- | --- |
| 등록: GDS 에 있는 이름 | 통과 (다시 Upload / 같은 Device 다시 저장) |
| 등록: 새 이름 + 등록 수 4 이하 | 통과 |
| 등록: 새 이름 + 등록 수 5 이상 | `POC 는 Device 를 5개까지 등록할 수 있습니다. 등록된 Device(5): OMM, OMM_HYBRID, OMM_TCB, U9, U7. 등록된 Device 로만 다시 저장할 수 있습니다.` |
| 사용: GDS 에 있음 | 통과 |
| 사용: GDS 에 없음 | `Device OMM 이 GDS Map 에 없습니다. GDS Map Setup 또는 GDS Map View [Map 복사] 로 먼저 등록하세요.` |

모두 `InvalidOperationException`. 확인 순서는 기간 → Device.

### 5.5 판정 클래스 변경 (`WorkRule`, 구현은 16절 기간 확인에 이어서)

```csharp
/// Device 를 어떻게 쓰는 저장인지. New = GDS 에 새로 등록할 수 있음(5개 확인), Registered = GDS 에 등록된 Device 만.
public enum DeviceUse { New, Registered }

/// 1단계: 기간 → Device 순서로 확인. 기존 Confirm(module, kind) 은 기간만 본다.
public DateTime Confirm(WorkModule module, WorkKind kind, string deviceId, DeviceUse use);

const int MaxDeviceCount = 5;
const string DevDatabaseId = "3988091916";   // D2: 개발 DB 에서는 Device 확인을 건너뜀(기간은 확인)
```

- SQL 은 기간 SQL 과 같이 코드 안에 둔다(Query XML 은 고칠 수 있음).
  - 등록부: `SELECT 1 FROM TQP_GDS_MAP WHERE UPPER(TRIM(DEVICE_ID)) = :1` 형태(이름 하나 확인)
  - 등록 수: `SELECT D FROM (SELECT UPPER(TRIM(DEVICE_ID)) D FROM TQP_GDS_MAP UNION SELECT UPPER(TRIM(MAPID)) FROM TQP_MAPDEF UNION ... ) WHERE D IS NOT NULL` (5.3, 목록은 막힐 때 문장에 넣음)
- DBID 는 기간 SQL 에서 `SYS_CONTEXT('USERENV','DBID')` 로 같이 읽는다.

### 5.6 GDS Map 삭제 기능 제거 (화면)

| 파일 | 변경 |
| --- | --- |
| `06_CoreLibSource/GdsMapControl/GdsMapViewForm.Designer.cs` | [Map ▾] 메뉴의 `mnuDeleteMap`("Map 삭제...") 항목 / 선언 / 이벤트 연결 삭제 |
| `GdsMapViewForm.cs` | `mnuDeleteMap.Visible = hostedByDms` 줄과 `mnuDeleteMap_Click` 처리 삭제 |
| `GdsMapViewForm_Layout.cs` | [Map ▾] 설명 주석에서 Map 삭제 제거 |
| `Probes/GdsViewLayoutSmoke.ps1` / `GdsMapViewU8Smoke.ps1` | [Map ▾] 메뉴 기대값을 "원점 표시" 만으로 / 필드 목록에서 `mnuDeleteMap` 제거 |

- 서버 `DeleteMap` 은 남긴다: GDS Map Setup 이 같은 Device 를 다시 저장할 때(덮어쓰기) 쓰고, Probe 가 시험 데이터 정리에 쓴다.
- 배포: GdsMap DLL(`100_ClientBin/GdsMap/NexplantQMS.GdsMap.dll`) 다시 배포 + DACrux 재시작.

### 5.7 개발 DB / 전달 DB (D2)

| 구분 | 처리 |
| --- | --- |
| 개발 DB (로컬 `DMS`) | 권장: **DBID 가 개발 DB(3988091916)일 때 Device 확인(5개 / GDS 에 없음)을 둘 다 건너뛴다.** 기간은 확인. 로컬은 GDS Device 8개(2026-10-19, Probe 3 포함) + Wafer Map 샘플이 있고, Probe 가 임시 Device 를 만든다 |
| 고객 DB | Data Pump 로 옮긴 DB 는 새 DBID 를 가진다(DBNEWID 는 값을 지정할 수 없음) → 제한 적용 |
| 전달 DB | 대상 Device(연구소 확정) 외 데이터를 지운 사본을 Data Pump 로 보낸다. 데이터 파일 / docker 볼륨 통째 복사 금지(DBID 가 넘어감). 전달 DB 등록 수 5 이하를 Probe 로 확인 |
| 대안 | 예외 없음 — 개발 DB 를 4 ~ 5개만 남기고 Probe 를 기존 이름만 쓰게 고침 |

### 5.8 한계

| 방법 | 결과 |
| --- | --- |
| 화면에서 Device 지우고 새로 등록 | 막힘(삭제 메뉴 없음) |
| 다른 Factory 로 등록 / 대소문자만 다른 이름 | 같은 등록 수로 셈 |
| Wafer Map / KLARF / E-Test 를 GDS 없이 등록 | 오류 |
| DB 에서 GDS Map 행 직접 삭제 | D1 권장안이면 다른 데이터에 이름이 남아 자리가 비지 않음. 다른 데이터까지 모두 지우면 막지 못함(데이터를 버리는 조작) |
| 서버 DLL 수정 | 막지 못함(9절과 같음) |

## 6. 소스에 녹이는 방법

| 원칙 | 방법 |
| --- | --- |
| 바꿀 수 있는 곳에 두지 않는다 | 기한 / Device 수(5) / 판정 / 조회 SQL 을 설정 파일 · `MenuFile.mnu` · Query XML · DB 테이블 · License 파일에 두지 않는다. 컴파일된 서버 DLL 안에만 있다 |
| 떼어 낼 수 있는 부품으로 만들지 않는다 | 별도 DLL(예: `DACrux.License.dll`)을 만들지 않는다. 같은 이름의 빈 DLL 로 바꿔치기하면 끝나기 때문이다. 업무 쿼리가 모여 있는 `DACrux.SEMDMS.DSL` 안 클래스로 둔다 |
| 클라이언트에 두지 않는다 | DSL 은 `101_ServerBin` 에만 배포된다. 클라이언트 PC 의 DLL 을 고쳐도 판정에 영향이 없다 |
| 업무 흐름의 한 단계로 둔다 | 저장 함수의 첫 단계 "작업 조건 확인" 으로 부른다. 통과해야 작업 키 발급 / 저장 단계로 넘어간다 |
| 셀 값을 따로 저장하지 않는다 | 기한은 상수, Device 수는 별도 등록 표 없이 실제 업무 데이터에서 매번 센다 |
| 이름 | 코드 이름은 업무 용어(`WorkRule` / `Confirm` / `WorkModule` / `WorkKind`)를 쓴다. 디컴파일 결과에서 "License" 로 검색해 바로 찾지 못하게 한다. 설명 주석은 공통 개발 지침대로 남긴다(주석은 DLL 에 들어가지 않는다) |

## 7. 서버 설계

### 7.1 판정 클래스 (`02_DMS/DACrux.SEMDMS.DSL/WorkRule.cs`, 신규)

```csharp
namespace DACrux.SEMDMS.DSL
{
    /// 작업 모듈. 정식 License 에서 모듈별 조건을 줄 때 그대로 쓴다. POC 는 모듈과 관계없이 같은 조건.
    public enum WorkModule { Dms, Gds, Klarf, ETest, WaferMap }

    /// 작업 종류. 조회는 확인하지 않으므로 생성·수정 종류만 둔다. (구현 때 Edit 는 쓰는 곳이 없어 뺐다. GDS Map View 3동작도 기준정보라 Setup)
    public enum WorkKind { Setup, Upload }

    /// 서비스 저장 흐름의 첫 단계. DB 일시(한국 시간)를 읽어 작업 조건(기한 / Device 수)을 확인한다.
    /// 통과하면 DB 일시를 돌려주고, 아니면 예외로 흐름을 멈춘다. 조회 SQL 은 Query XML 이 아닌 이 코드에 둔다.
    public class WorkRule : Miracom.Middleware.QueryComponent
    {
        static readonly DateTime LastWorkDay = new DateTime(2027, 1, 31);   // 이 날까지 작업 가능
        static readonly DateTime FirstValidDay = new DateTime(2026, 10, 1); // 이보다 앞선 DB 일시는 잘못된 시각
        const int MaxDeviceCount = 5;                                       // 등록 Device 수(v5, 5.5절)
        const string DevDatabaseId = "3988091916";                          // 개발 DB(L8). Device 수 확인만 건너뜀

        /// 1단계: 기한 → (Device 수) 순서로 확인. deviceId 가 비면 기한만 본다. 통과하면 DB 한국 시간.
        [MethodImpl(MethodImplOptions.NoInlining)]
        public DateTime Confirm(WorkModule module, WorkKind kind, string deviceId = null);

        /// 클라이언트 안내용 한 행: DB_TIME / LAST_DAY / WORK_OPEN / DAYS_LEFT (+ Device 결정 뒤 DEVICE_COUNT / DEVICE_MAX / DEVICE_LIST).
        [MethodImpl(MethodImplOptions.NoInlining)]
        public DataTable GetState();

        /// 판정만 하는 함수(시험용으로 값을 넣어 확인).
        internal static bool IsPeriodOpen(WorkModule module, WorkKind kind, DateTime dbNowKst);
        internal static bool IsDeviceAllowed(string deviceId, IList<string> registered);

        /// 4절 SQL 을 GetDataTable(DMS_CONNECT_ID, SQL) 로 조회. 실패하면 예외.
        [MethodImpl(MethodImplOptions.NoInlining)]
        void ReadDbState(out DateTime dbNowKst, out string dbId);
    }
}
```

- 기한 문장: `POC 사용 기간(2027-01-31 까지)이 지나 저장 / 수정 / 삭제할 수 없습니다. 조회 화면은 계속 사용할 수 있습니다.`
- 시각 오류 / 조회 실패: `서버 일시를 확인할 수 없어 저장할 수 없습니다.` (원인은 서버 로그에만)
- 모두 `InvalidOperationException`. 기존 BSL 이 던지는 예외와 같은 계열이라 기존 화면 오류 표시가 그대로 동작한다.
- `NoInlining`: Middleware 가 호출 메서드 정보를 `StackFrame` 으로 읽으므로, Probe 가 PowerShell 에서 직접 불러도 실패하지 않게 붙인다(2절).

### 7.2 서비스 첫 단계 (함수 목록)

```csharp
public DataTable BeginEtFile(string[] FileInfo)
{
    // 1단계: 작업 조건 확인. 기한이 지나면 여기서 끝나고 작업 키가 만들어지지 않는다.
    new DACrux.SEMDMS.DSL.WorkRule().Confirm(WorkModule.ETest, WorkKind.Upload, FileInfo[1]);

    // 2단계: 기존 처리 (인자 검사 → 파일 행 생성 → 작업 키 반환)
    ...
}
```

| 구분 | 파일 | 함수 (모두 첫 줄에 `Confirm`) | 종류 / 모듈 |
| --- | --- | --- | --- |
| POC 서비스 (신규 함수, 수정 범위 안) | `SEMDMS.BSL/GdsMapImport.cs` | `BeginMapImport` / `CompleteMapImport` / `DeleteMap` / `CopyMap` | Setup / Gds |
| | `SEMDMS.BSL/GdsChain.cs` | `BeginChainSave` / `CompleteChainSave` / `DeleteChain` / `CreateChainBulk` / `DeleteChainBulk` / `UpdateChainColorBulk` | Setup / Gds |
| | `SEMDMS.BSL/GdsEtResult.cs` | `BeginEtFile` / `CompleteEtFile` / `RejudgeEtFiles` / `SaveEtLimit` / `DeleteEtLimit` (**`DeleteEtFile` 제외** — E-Test Result View 와 같이 씀) | Upload · Setup / ETest |
| | `SEMDMS.BSL/GdsEtResult_Yms.cs` | `PublishEtFileToYms` | Upload / ETest |
| | `SEMDMS.BSL/GdsEtResult_Para.cs` | `RegisterEtParameters` / `SaveEtParameterSetup` / `DeleteEtParameters` | Setup / ETest |
| | `SEMDMS.BSL/GdsEtResult_Bin.cs` | `SaveEtBins` / `DeleteEtBins` (`TQP_GDS_ET_YMS.xml` 의 `MERGE_ET_BIN` / `DELETE_ET_BIN` → TQP_BINDESC) | Setup / ETest |
| | `SEMDMS.BSL/KlarfUpload.cs` | `BeginUpload` / `CommitUpload` / `DeleteUploadedStep` | Upload / Klarf |
| 기존 제품 함수 (대상 확정, 1줄 추가는 구현 착수 때 확인) | `TEST.BSL/ProbeAdmin.cs` (L4) | `SetMapDef` / `UpdateMapDef` / `CreateUseMap` | Setup / WaferMap |
| | `SEMDMS.BSL/SEMConfiguration.cs` (L9) | `InsertDefectType` / `UpdateDefectType` / `DeleteDefectType` | Setup / Dms |
| 막지 않음 (v4.1) | `SEMDMS.BSL/GdsEtResult.cs` / `DefectMapAnalysis.cs` | `DeleteEtFile` / `SetReclassifyDefectList_Comp` / `SetReclassifyDefectImageList` / `SetRemoveDefectImageList` | 대상 아닌 조회 화면 기능(L10) |

- `DeleteEtFile` 을 막지 않으므로, E-Test Result Upload / E-Test Para Register 화면의 파일 삭제는 기한 후 **메뉴 묶음 비활성(8절)으로만** 막힌다. 메뉴를 거치지 않는 경로(메뉴 파일 수정 등)로 이 두 화면을 열면 파일 삭제는 된다. 파일 삭제는 새 데이터를 만들지 않으므로 받아들인다.

- 확인은 인자 검사보다 앞, DB 작업 전에 둔다. 읽기 쿼리 1번이 늘 뿐 트랜잭션 / 저장 흐름은 바뀌지 않는다.
- TEST.BSL 은 이미 SEMDMS.DSL 을 참조하므로 참조 추가 없이 같은 클래스를 부른다.
- 기존 제품 함수는 함수 본문을 바꾸지 않고 **첫 줄 1줄만** 넣는다. 다른 화면(옛 frmSetupMap / Admin 화면 등)이 같은 함수를 쓰면 그 화면의 저장도 같이 막힌다(POC 기한 후에는 의도와 같다).

## 8. 클라이언트 (안내용, L5)

판정은 하지 않는다. 서비스가 준 결과로 메뉴만 막는다.

1. `iGdsMapView` / RO / BSL 에 읽기 함수 `GetWorkState()` 1개 추가 → BSL 은 `WorkRule.GetState()` 결과를 그대로 반환. 기존 wellknown 을 쓰므로 config 변경 없음.
2. `01_WinApp/DACruxV5/MenuWorkRule.cs`(신규):
   - Main 이 보인 뒤 **백그라운드로** `GetWorkState()` 호출(서버가 꺼져 있어도 시작이 늦어지지 않게) → 결과가 오면 UI 스레드에서 처리.
   - 기한이 지났으면 메뉴 묶음 **`MNU_POC_SETUP` / `MNU_POC_UPLOAD`** 의 `SharedProps.Enabled = false`. 묶음 아래 화면 8개(3.1)를 한 번에 막고, 나중에 이 묶음에 넣는 화면도 따로 등록할 필요가 없다.
   - 제목 문장: D-14 부터 `[데이터 등록 / 수정 기한 2027-01-31, 7일 남음]`, 기한 후 `[데이터 등록 / 수정 기간 종료 - 조회 전용]`.
   - 호출 실패(서버 꺼짐 / 옛 서버) → 아무것도 바꾸지 않는다. 저장은 서비스가 막는다.
   - `View / Analysis` 묶음과 DMS / TEST Analysis 메뉴는 건드리지 않는다. GDS Map View 의 Map 삭제 / 복사 / Chain 저장(3.2)은 누를 때 서비스 문장으로 안내한다.
3. `DACruxClientMain.cs`: DEBUG / Release 두 곳에서 `ApplyMenuSize(mMain)` 다음에 `MenuWorkRule.Attach(mMain)` 1줄.

메뉴 파일 ENABLE 이 이미 False 인 메뉴는 켜지 않는다. 막는 쪽으로만 바꾼다. 메뉴 파일에서 화면을 다른 묶음으로 옮기면 메뉴는 열리지만 저장은 서비스가 막는다.

## 9. 우회 가능성과 한계

| 방법 | 결과 |
| --- | --- |
| PC 시간 변경 | 영향 없음(PC 시간을 쓰지 않음) |
| `MenuFile.mnu` 수정 / 옛 클라이언트 / `NexplantQMS.GdsMap.exe` 단독 실행 | 화면은 열려도 서비스 첫 단계에서 막힌다 |
| 서비스 config 수정 | 기한 관련 키가 없다. `DMS_CONNECT_ID` 를 바꾸면 업무 쿼리 전체가 같이 멈춘다 |
| Query XML 수정(`SELECT_SYS_DATE` 고정 등) | 영향 없음(조회 SQL 은 코드 안) |
| DB 서버 OS 시간대 변경 | 영향 없음(UTC 로 바꾼 뒤 9시간을 더함) |
| 서버 서비스 PC 시간 변경 / DBA 의 `FIXED_DATE` | 영향 없음(`SYSTIMESTAMP` 사용, 구현 때 확인) |
| DB 서버 OS 시간을 과거로 변경 | 2026-10-01 이전이면 막힌다. 그 사이 날짜는 막지 못한다 |
| 기존 제품 함수에 1줄을 넣지 않는 경우(L4 / L9) | Wafer Map Setup / Defect Type Define 은 메뉴 묶음 비활성(L5)으로만 막힌다. 메뉴를 거치지 않는 경로로 열면 저장된다 |
| 대상 아닌 조회 화면 기능(L10) | E-Test Result View 파일 삭제 / Chart Analysis · Defect Image Gallery 의 Reclassify · 이미지 삭제는 기한 후에도 된다(결정) |
| 서버 DLL 디컴파일 후 수정(dnSpy 등) | 막지 못한다. 설정 / 파일 수정이 아니라 프로그램 변조이고, 소스 머리말의 저작권 조항 위반이다. 난독화 도구는 Remoting 형식 이름을 바꿀 위험이 있어 POC 에서는 쓰지 않는다 |

Device 관련 우회 항목은 5.8절(v5)과 5.7절(개발 DB 파일째 복사 금지)을 본다.

## 10. 정식 License 확장 (나중에 넣을 때)

### 10.1 바꾸는 것 / 그대로 두는 것

| 구분 | POC | 정식 |
| --- | --- | --- |
| 기한 / Device 수 출처 | `WorkRule` 안 상수 | License 정보(10.2) — `WorkRule` 내부만 변경 |
| 서비스 첫 단계 호출 / `GetWorkState()` / 메뉴 묶음 | 7 ~ 8절 | **그대로** |
| 기준 시간 | DB 한국 시간 | DB 한국 시간 (+ 10.3 되돌림 감지) |
| 모듈별 조건 | 모두 같음 | `WorkModule` / `WorkKind` 별 기한 / Device 수 — 호출 코드는 이미 모듈과 종류를 넘기고 있음 |
| 개발 DB 예외 | DBID 상수 | 개발용 License 파일로 대체하고 상수 삭제 |

### 10.2 License 정보 출처 (정식 개발 때 결정)

- **후보 1 — 기존 Middleware License**: `BaseComponent.GetLicense(파일, 키)` → `LicenseInfo { ServerIP, DueDate, Users }`, 설정 키 `LICENSEFILE` / `LICENSEKEY` / `CLIENTLICENSE`. 회사 공통 방식이면 발급 도구 / 키 관리가 이미 있을 수 있다. 다만 모듈별 기한 / Device 수 항목은 없다. 발급 방법과 파일 형식은 프레임워크 담당에게 확인 필요.
- **후보 2 — 서명된 License 파일**: 서버의 XML 파일에 고객 / Site / 설치 대상(DB 이름 또는 서버 이름) / 모듈 x 종류별 마지막 날 / Device 수 / 사용자 수를 넣고, 미라콤 개인 키로 서명(RSA-SHA256), 서버 DLL 의 공개 키로 검증. 기한 연장 / Device 추가는 파일만 다시 발급한다.
- 어느 쪽이든 서명 / 파일이 없거나 틀리면 조회만 허용한다.

### 10.3 그 밖에 정식 때 검토할 것

- 시간 되돌림 감지: 마지막으로 본 DB 일시를 저장해 두고 그보다 과거면 막는다.
- Device 동시 등록: 확인과 저장을 같은 트랜잭션 + DB 잠금(`DBMS_LOCK` 또는 잠금 행)으로 묶는다.
- 기존 제품 쓰기 기능의 권한 속성(`IsReclassifiedPermesion` 등)을 실제로 연결할지.
- PC / 동시 사용자 제한: Middleware `ChkLicense(IP)` / `LicenseInfo.Users` 와 합칠지 검토.
- 확인 지점을 원격 호출 공통 지점(서버 채널 Sink)으로 모으는 방법은 함수 이름 기반이라 디버깅이 어렵다. 함수 첫 단계 방식을 유지하는 것을 권장한다.

## 11. 결정

| ID | 내용 | 상태 / 권장 | 대안 |
| --- | --- | --- | --- |
| L1 | 사용 기한 | **확정: 2027-01-31 까지** (2026-10-14, 10-22 다시 지시) | - |
| L2 | Upload 도 기한 대상 | **확정: 예** — 10-22 지시 "Data 생성 및 수정 하는 화면" | - |
| ~~L3~~ | ~~기존 DMS Setup 3개 메뉴~~ | v3 에서 해소. v4 에서 `Defect Type Define` 이 POC Setup 묶음에 다시 보여 L9 로 다룸 | |
| L4 | Wafer Map Setup — TEST `ProbeAdmin.SetMapDef` / `UpdateMapDef` / `CreateUseMap` 은 **기존 함수** | **대상 확정**(v4.1 선택). 방법 권장: 첫 줄에 확인 1줄 추가 — 기존 함수 변경이라 구현 착수 때 확인 | 메뉴 묶음 비활성(L5)만. 메뉴를 거치지 않는 경로는 못 막음 |
| L5 | 클라이언트 메뉴 막기 | 권장: 시작 후 백그라운드로 `GetWorkState()` 1번 → 기한 후 묶음 `MNU_POC_SETUP` / `MNU_POC_UPLOAD` 비활성 + 제목 문장 | 메뉴는 그대로 두고 저장 때 서비스 문장만 |
| L9 (v4) | Defect Type Define — `SEMConfiguration.InsertDefectType` / `UpdateDefectType` / `DeleteDefectType` 는 **기존 함수** | **대상 확정**(v4.1 선택). 방법 권장: 첫 줄에 확인 1줄 추가 — 구현 착수 때 확인 | 메뉴 묶음 비활성(L5)만 |
| L10 (v4) | 조회 화면 안의 다른 쓰기 — E-Test Result View [파일 삭제] / Chart Analysis [Reclassify Apply] / Defect Image Gallery [Reclassify Apply] · [Remove Image Delete] | **확정: 대상 아님(그대로)** — 2026-10-22 대상 목록 선택. `DeleteEtFile` 도 서버에서 막지 않음 | - |
| ~~L6 ~ L8~~ | ~~Device 5개(세는 기준 / 삭제 시 자리 / 개발 DB)~~ | v5 로 대체: 등록 = GDS 에서만 5개 / 나머지 GDS 에 없으면 오류 / 삭제 기능 제거 / DRAFT 포함 + 다시 Upload 가능 (2026-10-26 확정) | |
| ~~D1 (v5)~~ | ~~등록 수 세는 기준~~ — 5개 제한 안 함(10-26)으로 해소 | 권장: GDS Map Device + 그 이름으로 저장된 Wafer Map / E-Test 데이터의 Device 합집합(DB 에서 GDS 행을 지워도 자리가 비지 않게, 5.3) | GDS Map 이름만 |
| ~~D2 (v5)~~ | ~~개발 DB~~ — 5개 제한 안 함(10-26)으로 해소 | 권장: DBID 가 개발 DB 일 때 Device 확인 2가지를 건너뜀(기간은 확인) + 전달은 Data Pump 로만 + 전달 DB 는 대상 Device 만(5.7) | 예외 없음(개발 DB 정리 + Probe 수정) |
| ~~L6 (v1)~~ / ~~L7 (v1)~~ | ~~GDS View 클릭 전 확인 / 판정 클래스 위치~~ | v2 에서 해소 | |

## 12. 구현 단계 (결정 후, 일자 먼저)

편집 직전에 대상 파일을 다시 읽고, 작업 현황에서 같은 파일을 수정 중인 작업이 있는지 확인한다.

1. **판정 단계**: `SEMDMS.DSL/WorkRule.cs` + DSL csproj Compile 1행 + Probe `WorkRuleProbe.ps1`(① 코드 안 SQL 로 DB 한국 시간 / DBID 조회 ② `IsPeriodOpen` 에 값을 넣어 판정). Device 확인은 L6 ~ L8 결정 뒤 켠다.
2. **POC 서비스 첫 단계**: 7.2 표 POC 서비스 함수 첫 줄.
3. **기존 제품 함수(확인 후)**: L4 `ProbeAdmin` 3함수 / L9 `SEMConfiguration` 3함수 첫 줄.
4. **클라이언트 안내(L5)**: `iGdsMapView` / RO / BSL `GetWorkState()` + `DACruxV5/MenuWorkRule.cs` + `DACruxClientMain.cs` 2곳 + DACruxV5 csproj(SEMDMS.RO / Interface 참조, Compile 1행).
5. **배포**: DSL / BSL / Interface → `101_ServerBin`, RO / Interface / DACruxV5 → `100_ClientBin`, SEMDMS / TEST 서비스 재시작.
6. **문서**: 공통 개발 지침에 "새 데이터 생성·수정 서비스 함수 = 첫 단계 `WorkRule.Confirm`, 화면은 메뉴 묶음 `Setup` / `Data Upload` 에 둔다" 규칙 / 빌드 가이드에 기한 변경 절차.

(1 ~ 4, 6 은 2026-10-23 완료 — 16절. 5 배포는 사용자.)

**Device 5개 (v5) — 2026-10-26 하지 않기로 함, 아래 7 ~ 11 단계는 진행하지 않음**

7. **판정**: `WorkRule` 에 `DeviceUse` / `Confirm(module, kind, deviceId, use)` / 등록부 확인 · 등록 수 SQL(코드 안) / `MaxDeviceCount` / `DevDatabaseId` 추가(5.5).
8. **서버 첫 줄 인자 추가**: 등록(New) = `BeginMapImport` / `CompleteMapImport` / `CopyMap`, 사용(Registered) = `ProbeAdmin.SetMapDef` / `UpdateMapDef` / `CreateUseMap`, `KlarfUpload.BeginUpload`, `RegisterEtParameters` / `SaveEtParameterSetup` / `SaveEtLimit` / `SaveEtBins` / `BeginEtFile`(5.2). 이미 있는 첫 줄의 인자만 바꾼다.
9. **화면**: GDS Map View [Map 삭제] 제거 + Smoke 2개 기대값(5.6). GdsMap DLL 빌드.
10. **Probe**: `WorkRuleProbe.ps1` 에 Device 항목 — 등록 수 / 등록부 조회, 개발 DB 예외, 임시 빌드(`DevDatabaseId` 를 다른 값으로)로 6번째 새 이름 막힘 · 등록된 이름 통과 · GDS 없는 Device 의 Wafer Map / KLARF / E-Test 저장 오류(첫 줄에서 멈춰 DB 변경 없음).
11. **배포(사용자)**: Interface 는 바뀌지 않으므로 서버는 DSL / SEMDMS.BSL / TEST.BSL, 클라이언트는 GdsMap DLL. 전달 DB 정리(5.7).

## 13. 검증 기준 (기대값)

| 항목 | 방법 | 기대값 |
| --- | --- | --- |
| DB 일시 / DBID | Probe 에서 `ReadDbState`(NoInlining) | 로컬 DB(OS UTC)에서도 한국 시간 = 서버 PC 시각과 같음 / DBID 3988091916 |
| 기한 판정 | `IsPeriodOpen` 에 일시 입력 | 2026-09-30 23:59:59 막힘 / 2026-10-01 00:00:00 통과 / 2027-01-31 23:59:59 통과 / 2027-02-01 00:00:00 막힘 |
| `FIXED_DATE` 영향 | 로컬 docker DB 에서만 `FIXED_DATE` 설정 → `ReadDbState` → 원복 (DB 설정 변경이라 실행 전 사용자 확인) | `SYSDATE` 는 고정 날짜, `ReadDbState` 는 실제 시각 |
| 첫 단계 누락 | 7.2 표 함수의 첫 줄을 소스 검사 | 누락 0 |
| 기한 전 회귀 | 기존 Probe / Smoke (GDS Import / 복사 / Chain / E-Test Para · Bin · 업로드 / KLARF) | 지금과 같은 결과 |
| 기한 후 서비스 | 로컬에서만 `LastWorkDay` 를 어제로 바꾼 임시 DSL 빌드 → 7.2 표 함수 호출 | 기한 문장 예외 / DB 변경 없음 / 작업 키 미발급. 조회 함수는 정상. 확인 후 상수 원복 · 재빌드(임시 빌드는 배포 안 함) |
| 기한 후 화면 | 같은 임시 빌드로 DMS 실행 | `Setup` / `Data Upload` 묶음 비활성, `View / Analysis` / DMS / TEST 메뉴 정상, 제목 문장, GDS Map View 의 Map 삭제 / 복사 / Chain 저장에서 기한 문장. E-Test Result View 파일 삭제 / Chart Analysis · Image Gallery 의 Reclassify · 이미지 삭제는 기한 후에도 동작(대상 아님) |
| 서버 꺼짐 상태 시작 | 서비스 중지 후 DMS 실행 | 시작이 늦어지지 않음 / 메뉴 그대로 |
| Device 등록 (v5) | 개발 DB 예외를 끈 임시 빌드 + 등록 수 4 / 5 상태를 만든 시험 Device | 4개 + 새 이름 통과 / 5개 + 새 이름 "5개까지" 문장 / 5개 + 등록된 이름(대소문자 · 공백 다름 포함) 통과 / DRAFT 이름 다시 Upload 통과 / Complete 에서 다시 셈 |
| Device 사용 (v5) | 같은 임시 빌드 | GDS 에 없는 Device 로 Wafer Map / KLARF / E-Test 저장 → "GDS Map 에 없습니다" 문장, DB 변경 없음 / GDS 에 있는 Device 는 통과 |
| 개발 DB (v5) | 정식 빌드로 로컬 DB | Device 확인 건너뜀(기존 Probe 회귀 그대로), 기간 확인은 동작 |
| 삭제 제거 (v5) | GdsViewLayoutSmoke / GdsMapViewU8Smoke | [Map ▾] 메뉴에 Map 삭제 없음 |

시험용으로 기한 / Device 수를 바꾸는 설정(config 등)은 만들지 않는다. 고객 환경에 남으면 그대로 우회 수단이 된다.

## 14. 확인 필요

- 고객 DB 서버의 OS 시간대 — 한국 시간으로 바꿔 판정하므로 결과에는 영향이 없으나, 다른 기존 쿼리의 `SYSDATE` 기록 시각이 UTC 로 남는지는 별도 문제(로컬 DB 는 UTC).
- Infragistics 에서 닫힌 Pane(Defect Map Viewer 의 Reclassify / RD)을 사용자가 기본 UI 로 다시 열 수 있는지 — 코드에는 다시 여는 경로가 없다. L10 = 대상 아님으로 정해 이번 범위에서는 영향 없음.
- 고객 DB 가 전달 DB 인지 기존 DACrux DB 인지, 연구소 확정 대상 Device 목록(5절).
- 정식 License 방식(10.2): 기존 Middleware License 의 발급 방법 / 회사 표준 여부.

## 15. 확인 수준

- 소스 확인: 2026-10-22 16:10 메뉴(23개), POC 화면 12개의 서비스 쓰기 호출(grep) / GdsMapControl 핸들러, 기존 DMS 4개 · YMS 7개 화면과 검색 패널의 RO → BSL → DSL → Query XML statement(별도 조사), POC 서비스 Interface 함수 목록, 기존 License 코드, Middleware 공개 함수 / IL(리플렉션).
- 실행 확인(로컬, 조회만): Middleware `GetDataTable(ConnectID, Query)` 를 컴파일된 코드 / PowerShell 에서 호출해 성공 · 실패 조건 확인, DB 한국 시간 변환 SQL 3가지 비교, DBID 조회. 이전: 로컬 DB Device 목록(10-19), Connection Pool 세션 재사용(10-21).
- 빌드 / Probe / 화면 확인 없음. 소스 / DB 변경 없음.

## 16. 구현 결과 (2026-10-23, CLAUDE-218 — 일자 제한만)

| 구분 | 파일 | 내용 |
| --- | --- | --- |
| 판정 | 신규 `02_DMS/DACrux.SEMDMS.DSL/WorkRule.cs` + DSL csproj 1행 | `Confirm(모듈, 종류)` / `GetState()` / `IsOpen` / `DaysLeft`. 상수 `LastWorkDay = 2027-01-31`, `FirstValidDay = 2026-10-01`. DB 일시 SQL 코드 안(`SYS_EXTRACT_UTC(SYSTIMESTAMP) + 9/24`, `DMS_CONNECT_ID`). DB 일시 실패 시 Oracle 예외는 InnerException 으로 넘기지 않고 문장만 붙임(클라이언트 Remoting 역직렬화 실패 방지). Device 인자 / DBID 는 이번에 넣지 않음(5절 결정 대기) |
| 서버 첫 줄 | SEMDMS.BSL `GdsMapImport`(4) · `GdsChain`(6) · `GdsEtResult`(5) · `GdsEtResult_Yms`(1) · `GdsEtResult_Para`(3) · `GdsEtResult_Bin`(2) · `KlarfUpload`(3) · `SEMConfiguration`(3, 기존) / TEST.BSL `ProbeAdmin`(4 = SetMapDef / UpdateMapDef / CreateUseMap 2개, 기존) | 31곳. 각 함수 여는 괄호 다음에 주석 1줄 + `new WorkRule().Confirm(...)` 1줄. 함수 본문은 바꾸지 않음. `DeleteEtFile` / 조회 / 배치 중간 / Cancel 은 넣지 않음 |
| 메뉴 안내 | `iGdsMapView` / RO · BSL `GdsMapView.GetWorkState()`(새 wellknown 없음) + 신규 `01_WinApp/DACruxV5/MenuWorkRule.cs` + `DACruxClientMain.cs` 2줄 + DACruxV5 csproj(Compile 1행, SEMDMS.Interface / RO 프로젝트 참조) | Main Shown 뒤 작업 스레드에서 1번 호출. 기한 후 `MNU_POC_SETUP` / `MNU_POC_UPLOAD` 와 그 아래 Tool 비활성 + 제목 `[데이터 등록 / 수정 기간 종료 - 조회 전용]`, D-14 부터 남은 일수. 호출 실패 시 아무것도 바꾸지 않음 |
| 검증 | 신규 `02_DMS/DACrux.SEMDMS.BSL/Probes/WorkRuleProbe.ps1` | 기간 안 10항목 / `-ExpectClosed` 11항목 |

검증 결과 (TEMP 빌드 `%TEMP%\ChippacBuild\CLAUDE-218\server` / `client`, 로컬 DB):
- 빌드: Interface / DSL / TEST.BSL / SEMDMS.BSL / RO / DACruxV5 오류 0. 경고는 기존 것만(TEST.BSL CS0219 `dt`, DACruxV5 MSB3274 4.8 GDS DLL 참조).
- `WorkRuleProbe.ps1 -BslDir server` = **PASS (10)**: 소스 첫 줄 31 / 31, `DeleteEtFile` · `GdsMapView` 확인 없음, 기간 경계 4개, 남은 일수, DB 한국 시간 = PC 시각(로컬 DB OS 는 UTC), `WORK_OPEN=Y LAST_DAY=2027-01-31 DAYS_LEFT=101`, BSL `GetWorkState`, `Confirm` 통과.
- 저장소 소스를 바꾸지 않고 DSL 을 TEMP 로 복사해 `LastWorkDay = 2026-10-22` 로 만든 임시 빌드(`expired`, 배포 안 함)로 `-ExpectClosed` = **PASS (11)**: `WORK_OPEN=N DAYS_LEFT=0`, 대상 31개 함수 모두 첫 단계에서 기간 문장으로 멈춤(DB 쓰기 전), 조회 `GetMapDeviceList` 정상.
- 미확인: Service Remoting 경유 호출 / DMS 화면(메뉴 비활성 / 제목 / 화면 오류 창 문장) / 기존 기능 회귀 Probe(기간 안이라 첫 줄이 통과만 하므로 동작 변화 없음 — 소스 검사와 `Confirm` 통과로 갈음).
