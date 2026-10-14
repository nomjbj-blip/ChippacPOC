# KLARF 업로드 / DB 적재 설계 (2026-10-07)

작업 ID: CLAUDE-031 / 기능: F08 KLARF 업로드 / 상태: **설계 확정** (2026-10-07 사용자 결정 D1~D4 반영, 10.1절. D4 는 권장안 대신 FTP 상대 경로) / 소스 / DB 수정 없음 (로컬 docker DB 조회만)

---

## 1. 목적과 결론

엔지니어가 KLARF 파일과 Defect 이미지를 화면에서 직접 올리면, 그 데이터를 **기존 DMS 자동 적재(Inspection.DataService)와 같은 테이블 / 같은 값 규칙**으로 저장한다. 그래서 기존 Defect 조회 화면(왼쪽 Wafer 조회 -> Defect Map Viewer / Map Gallery / Image Gallery / Chart / Die Stack 등)과 POC 화면(GDS Correlation)이 별도 수정 없이 같은 데이터를 조회한다.

결론(권장안):

1. **새 테이블을 만들지 않는다.** 기존 `TQD_SETUP / SETUP_MAP / PRODUCT / LOT / WAFER / STEP / DEFECT / INSP_INFO / IMAGES / INSP_SUM`에 넣는다. 기존 화면은 모두 `STEP_SEQ` 하나로 이 테이블들을 읽기 때문이다(3절).
2. **파싱과 저장은 서버(BSL)에서 한다.** 신규 BSL `KlarfUpload`가 기존 Parser(`ParserKlarf` / `ParserKlarf_18`)로 파일을 읽고, 기존 BSL `DefectDefine` / `DMReview`의 public 함수를 **수정 없이 호출**해 DataService Step 1~12와 같은 순서로 저장한다. 화면은 미리보기와 파일 전송만 한다.
3. 기존 DataService에 파일을 떨어뜨려 처리시키는 방식은 쓰지 않는다. 고객 샘플이 현재 DataService 규칙으로는 **적재에 실패**한다(4절).
4. 고객 샘플 기준으로 기존 규칙과 맞지 않는 항목 5개(SETUP_ID 길이 / Step ID 규칙 / 분류 컬럼 / 이미지 하위 폴더 / 원본 WaferID)는 **기존 코드를 고치지 않고 업로드 쪽 규칙으로 흡수**한다. 사용자 결정이 필요한 항목은 10절에 모았다.

## 2. 확인한 근거 (2026-10-07)

| 항목 | 확인 내용 | 근거 |
| --- | --- | --- |
| 자동 적재 경로 | `Inspection.DataService` 가 Handler(`TQC_EQUIP.HANDLER`) -> Parser -> Wafer 마다 Step 1~13 저장. **트랜잭션 없음**(Step 별 자동 커밋, 재처리 시 "있으면 건너뜀") | `02_DMS/DACrux.SEMDMS.Inspection.DataService/InspectionDataService.cs:52~651` 직접 읽음 |
| 재사용할 BSL | Step 1~12 가 쓰는 `DefectDefine` 함수 20개 / `DMReview.InsertDefectImage` 모두 **public** | `02_DMS/DACrux.SEMDMS.BSL/DefectDefine.cs:19~569` / `DMReview.cs:121` |
| 원격 인터페이스 | `iDefectDefine` 원격 계약은 위 함수와 이름 / 시그니처가 다르다(GetSetupInfo, CreateStepInfo ...). DataService 는 BSL 을 **같은 프로세스에서 직접** 부른다 | `02_DMS/DACrux.SEMDMS.Interface/iDefectDefine.cs` |
| BSL 참조 | `DACrux.SEMDMS.BSL` 이 이미 `DACrux.Data.Parser` 프로젝트를 참조한다(.NET 4.0) | BSL csproj 124행 |
| 화면이 읽는 테이블 | 왼쪽 Wafer 조회는 `TQD_INSP_INFO` **단독**(`SELECT_DMS_WAFER_LIST`). Map 은 `TQD_INSP_INFO` + `TQD_SETUP`(INNER) + `TQD_SETUP_MAP`(SETUP_SEQ + TEST) + `TQD_DEFECT`. 이미지는 `TQD_IMAGES` + 파일 | `DPUCStepSelect.cs:1445~1834` / BSL `DefectMapAnalysis.cs:726, 1073, 1294~1395` / `TQD_INSP_INFO.xml:688, 804` |
| 로컬 DB 구조 | PK: DEFECT(STEP_SEQ, DEFECTID) / IMAGES(STEP_SEQ, DEFECTID, IMAGE_ID) / INSP_INFO(STEP_SEQ, TEST) / SETUP_MAP(SETUP_SEQ, TEST, INDEX_X, INDEX_Y). FK 없음. 시퀀스 SETUP_SEQ / LOT_SEQ / WAFER_SEQ / STEP_SEQ | 로컬 docker oracle19c `DBA_CONSTRAINTS` 조회 |
| 문자열 길이 | `TQD_SETUP.SETUP_ID / STEP_ID`, `TQD_STEP.STEP_ID`, `TQD_LOT.LOT_ID`, `TQD_WAFER.WAFER_ID`, `TQD_INSP_INFO.*_ID` 모두 **VARCHAR2(40)** / `TQD_STEP.COMMENT` 500 | 같은 조회 |
| 기존 이미지 행 | 로컬 `TQD_IMAGES` 427건의 IMAGE_PATH 가 `C:/QMSClient/INSP_BACKUP/2019/05/20/195J64` 같은 **절대 경로** / THUMB 동일 / UPLOAD='INSP' | 같은 DB 데이터 조회 |
| 이미지 열기 | (a) Map Viewer 이미지 목록: `fnFTPImageDownload` = `TQC_CONFIG DEFECT_FTP`(MAIN_PATH) + IMAGE_PATH 를 FTP 로 받음. (b) Image Gallery 썸네일 / 팝업: `fnFTPGetStream` 이 **DB 경로를 그대로** `WebRequest.Create` (260223 Chris 변경) | `02_DMS/DACrux.SEMDMS.Control/DefectMapDraw.cs:805~926` |
| 로컬 FTP 설정 | `DEFECT_FTP` IP 172.16.15.100 / MAIN_PATH `/%2f/` (POC PC 에서 접속 가능 여부 미확인) | 로컬 DB `TQC_CONFIG` |
| 다른 설계와의 관계 | E-Test 설계(CLAUDE-029)는 KLARF 가 `TQD_LOT.LOT_ID / TQD_WAFER(SLOT)` 에 적재된다는 전제로 연결 키(Lot 핵심 + Slot)를 잡았다 | `문서/설계/04_ETest/2026-10-07_ETest_결과파일_Parsing_DB설계.md` 8절 |

### 2.1 미커밋 변경 (이번 설계에서 수정하지 않음)

- `InspectionDataService.cs` / `ReviewDataService.cs`: FAB1 조건 제거. Step ID 8자리 규칙이 **모든 Factory 에 적용**된다. `app.config` FACTORY = SCK. 작성자 / 의도 미확인.
- 이 변경 때문에 고객 샘플은 자동 적재에서 실패한다(4절). 업로드 기능은 이 파일을 쓰지 않으므로 영향이 없다. 자동 적재 경로를 고칠지는 별도 결정이다.

## 3. 기존 화면에 보이기 위한 최소 데이터

모든 화면은 `STEP_SEQ` 로 이어진다. `WAFER_SEQ` 도 테이블 간 같아야 한다.

| 목적 | 필요한 행 | 주의 |
| --- | --- | --- |
| 왼쪽 Wafer 목록 | `TQD_INSP_INFO` (STEP_SEQ, TEST 단위) | 마스터 JOIN 없음. 기간 조건 = RESULTTIMESTAMP (기본 7일). DEFECTS / DEFECTIVE_DIE NULL 금지. "Include Image" 는 `IMAGES > 0` |
| Map 표시 | `TQD_SETUP` (INSP_INFO.SETUP_SEQ) + `TQD_SETUP_MAP` (같은 SETUP_SEQ + **INSP_INFO.TEST**) + `TQD_DEFECT` | 없으면 BSL 예외. STEP_SEQ 는 Int32 범위 (RO 가 intParse) |
| 이미지 | `TQD_IMAGES` + `TQD_DEFECT.IMAGECOUNT > 0` + 파일 접근 경로 | Image Gallery 는 INSP_INFO / DEFECT / IMAGES 의 STEP_SEQ, WAFER_SEQ, DEFECTID 가 모두 일치해야 함 |
| Class 이름 / 색 | `TQD_DEFECT_TYPE` (CLASSNUMBER) | 없으면 번호만 표시 |
| 이력 / 추세 / New Defect | `TQD_PRODUCT / LOT / WAFER / STEP`, `TQD_INSP_SUM` | 화면 표시에는 직접 필요 없지만 DataService 와 같게 넣는다 |

## 4. 고객 샘플 대조 결과 (`SampleFile/26_10_06 Sample File/XD106.02`)

KLARF 1.2 / Wafer 1 (WaferID `XD106.02`, Slot 2) / InspectionTest 3 / SampleTestPlan 26 Die / Defect 15건 / `TiffFileName` 15개 (jpg 는 하위 폴더 `Defect Images\` 에 있음). 두 번째 샘플 `MIRACOM_KLAF file/AIBIZ_SCK.KLAF.SINF` 도 같은 형식.

| # | 항목 | 샘플 값 | 기존 규칙으로 저장하면 | 업로드 설계 |
| --- | --- | --- | --- | --- |
| S1 | SetupID | `ROOT-CAI-OMM_Hybrid-OMM_Hybrid_C4Bump_Silterra` (**46자**) | `TQD_SETUP.SETUP_ID` VARCHAR2(40) 초과 -> ORA-12899 로 Step 1 실패 | 결정 D2 |
| S2 | StepID | `OMM_Hybrid_C4Bump_Silterra` (26자) | 8자리가 아니면 SetupID 를 `_` 로 나눠 정확히 2조각이어야 함 -> 5조각이라 **예외**. AIBIZ 샘플은 2조각이라 STEP_ID 가 `TBDB` 로 바뀜 | KLARF StepID 그대로 저장 (8자리 규칙은 FAB1 규칙, 업로드에는 적용 안 함). 화면에서 수정 가능 |
| S3 | 분류 | CLASSNUMBER 전부 0, 실제 분류는 **ROUGHBINNUMBER** (53 RDLopen / 54 RDLshort). ClassLookup 42개 | 기존 화면 Class 집계 / 색상이 모두 0(Uncategorized). 로컬 `TQD_DEFECT_TYPE` 53 은 `STI Dimple` (예전 FAB 데이터) 로 이름이 다름 | 결정 D3 |
| S4 | 이미지 위치 | KLARF 와 다른 하위 폴더 `Defect Images\` | `ImageMapper` 는 KLARF 와 **같은 폴더**의 파일만 등록 -> 이미지 0건 | 서버 작업 폴더에 KLARF 와 이미지를 한 폴더로 모은 뒤 파싱 (6.3절) |
| S5 | WaferID | `XD106.02` | Parser 규칙으로 `PFIC_RH-HS2615_C4BUMP-02` (`{LotID}-{Slot:D2}`, LotID 대문자). 원본 값은 어디에도 남지 않음 | DMS 규칙 유지(기존 화면 / E-Test 연결과 같은 키). 원본 WaferID 는 `TQD_STEP.COMMENT` 에 기록 (6.2절) |
| S6 | 방향 | `OrientationMarkLocation DOWN` / `OrientationInstructions "Front"` | ANGLE 0, 회전 없음 | 같은 규칙. Front / Back 해석은 F07 좌표 계약에서 다룬다 |
| S7 | 설비 | `InspectionStationID "KLA" "LDS CIRCL" "Tool"` -> Equip = `Tool` | DataService 는 폴더 이름(EQUIP_ID)으로 덮어씀 | 화면에서 `TQC_EQUIP` 목록 선택 또는 직접 입력 (기본 = 파일 값). `INSPECTION_EQ` 는 왼쪽 조회의 기본 조건이다 |
| S8 | 시각 | ResultTimestamp `06-05-26 12:01:42` -> 2026-06-05 | 왼쪽 조회 기본 기간(7일) 밖 | 저장 완료 창에 "조회 기간을 2026-06-05 포함으로" 안내 |

S1 / S2 때문에 "DataService 폴더에 파일을 복사해 자동 적재" 방식은 현재 소스로는 동작하지 않는다. 이 방식을 쓰려면 기존 DataService 소스와 DB 컬럼을 고쳐야 하므로 기존 소스 영향 금지 원칙과 맞지 않는다.

## 5. 처리 방식 비교

| 안 | 내용 | 장점 | 단점 |
| --- | --- | --- | --- |
| **A (권장)** | 화면: 파일 선택 / 미리보기 / 전송. 서버 신규 BSL `KlarfUpload`: 기존 Parser 로 파싱 + 기존 `DefectDefine` 함수로 Step 1~12 저장 + 이미지 / 썸네일 저장 | DataService 와 같은 Parser / 같은 BSL 함수 / 같은 계산(Origin Index, New Defect, INSP_INFO 집계). 저장 규칙이 서버 한 곳에 있음. 클라이언트에 이미지 저장소 쓰기 권한 불필요 | 파일 바이트를 Remoting 으로 보냄 (이미지 많으면 여러 번 호출) |
| B | 화면에서 파싱해 `string[,]` 로 보내고 서버는 INSERT 만 | 전송량 작음 | 좌표 / 집계 / New Defect 계산이 클라이언트에 복제되어 DataService 와 어긋날 위험. 이미지 파일 저장을 따로 해결해야 함 |
| C | 화면이 DataService 입력 폴더(`DATA_PATH\{EQUIP_ID}`)에 복사 | 신규 저장 코드 거의 없음 | 고객 샘플 적재 실패(S1 / S2). DataService 실행 / TQC_EQUIP 등록 필요. 결과를 화면에서 바로 확인 못 함 |

## 6. 상세 설계 (A안)

### 6.1 전체 흐름

```text
[화면 frmKlarfUpload]
 1. KLARF 파일 선택 -> 클라이언트에서 Parser 로 미리보기 (Wafer / Defect / Map / 이미지 찾기 결과 / 저장될 키 값)
 2. 저장 값 확인 / 수정 (STEP_ID / EQUIP / PRODUCT / 분류 기준) -> 기존 등록 여부 조회
 3. [DB 저장]
      RO.KlarfUpload.BeginUpload(KLARF 바이트 + 옵션)       -> 서버 작업 폴더 생성, UPLOAD_KEY 반환
      RO.KlarfUpload.AddImage(UPLOAD_KEY, 파일명, 바이트) x N   (이미지 파일마다 1회)
      RO.KlarfUpload.CommitUpload(UPLOAD_KEY, 옵션)         -> 서버에서 파싱 + 저장, Wafer 별 결과 DataTable 반환
 4. 결과 표시 (Wafer 별 STEP_SEQ / Defect 수 / 이미지 수 / 오류) + 조회 안내

[서버 BSL KlarfUpload.CommitUpload]
 - 작업 폴더의 KLARF 를 FileAnalyzer -> ParserKlarf / ParserKlarf_18 로 파싱
   (파싱 전에 ParserKlarf.GetOriginDieIndex = DefectDefine.GetOriginDieIndex, DataService 67행과 같음)
 - ErrorFlag / Wafer 0 / Pitch 0 / SampleSize 0 검사
 - GetAngle() > 0 이면 RotateAndRecalculate(360 - angle, CenterToLowerLeft)  (HandlerInsp.Run 140행과 같음)
 - Wafer 마다 Step 1~12 (6.2절)
 - 이미지 저장소로 원본 + 썸네일 복사 (6.3절) -> 작업 폴더 삭제
```

### 6.2 저장 규칙 (DataService 대비)

| Step | 테이블 / 호출 (기존 BSL 함수, 수정 없음) | 업로드에서의 값 / 차이 |
| --- | --- | --- |
| 1 | `TQD_SETUP` / `GetSetupDuplicate` -> `CreateSetupData` | DataService 와 같음. SETUP_ID 길이는 결정 D2. ANGLE 은 `GetAngle()` 값 사용 (DataService 의 "TOP" 비교는 Parser 의 "UP" 과 달라 180도가 0 으로 저장되는 문제가 있어, 업로드는 Parser 각도를 그대로 쓴다) |
| 2 | `TQD_SETUP_MAP` / `GetSetupTestData` -> `CreateSetupMap` | Test 마다 그 Test 의 SampleTestPlan 으로 배열을 만든다 (DataService 는 합친 목록 `oIndex[ir]` 를 써서 Test 가 2개 이상이면 어긋날 수 있음. 업로드 코드는 신규 코드이므로 올바르게 작성) |
| 3 | `TQD_PRODUCT` / `GetProductInfo` -> `CreateProduct` | PRODUCT = 화면 값(기본 KLARF DeviceID). FACILITY = 로그인 Factory. 있으면 건너뜀 (DataService 와 같음) |
| 4 | `TQD_LOT` / `GetLotSeq` -> `CreateLotInfo` | LOT_ID = Parser 값(대문자). **앞 'A' 제거는 하지 않음** (HandlerInsp 주석 "FAB1은 LOT 앞에 A가 붙으면 제거") |
| 5 | `TQD_WAFER` / `GetWaferSeq` -> `CreateWaferInfo` | WAFER_ID = Parser DMS 규칙 값 / SLOT_ID |
| 6 | `TQD_STEP` / `GetStepInfo` -> `CreateStepSeq` | STEP_ID = 화면 값(기본 KLARF StepID). INSPECTION_EQ / MAIN_EQ = 화면 설비. INSP_FILENAME = 원본 파일명. INSP_FILEPATH = 이미지 저장 경로(6.3). COMMENT = `UPLOAD;USER=<ID>;RAW_WAFER_ID=<원본>;CLASS_SRC=<ROUGHBIN 등>` (자동 적재 `Parser` 와 구분) |
| 7 | `TQD_DEFECT` / `GetDefectDataCount` -> `GetLastestStepDefectXY` + `DefectHelper.CalculateNewDefect`(허용오차 100um, DataService 기본값) -> `CreateDefect` | 좌표 / Index / 크기는 Parser 값 그대로 (X = XINDEX*Pitch + XREL - SampleCenter, Index 는 좌하단 (1,1) 기준). Cluster 는 파일 값 유지 (설비 CMF3 기본 N 과 같음). 분류는 결정 D3 |
| 8 | `TQD_INSP_INFO` / `GetInspInfoDuple` -> `CreateInspData` | DataService 와 같은 집계식. FACTORY = 로그인 Factory. **IMAGES = 실제 저장 이미지 수** (DataService 는 0 으로 넣어 "Include Image" 조회에서 빠짐). COMMENT = `Upload` |
| 9 | `UpdateStepTestNo` | 새 STEP 일 때만 (같음) |
| 10 | `TQD_IMAGES` / `DMReview.InsertDefectImage` | 행 구성은 `HandlerInsp.GetImage2DArray` 와 같음 (UPLOAD = `UPLOAD`, 자동 적재 `INSP` 와 구분). 경로 형식은 결정 D4 |
| 11 | `UpdateImageCount` | 같음 |
| 12 | `CreateInspSum` | 같음 |
| 13 | Alarm (메일) | **하지 않음** (수동 업로드에서 알람 메일을 보내지 않음) |

- `TQD_STEP.IMAGES` 도 저장 이미지 수로 맞춘다. 이 두 IMAGES 갱신은 기존 `UPDATE_INSP_INFO[REVIEW_PARSER]` 등 기존 쿼리를 쓸 수 있는지 구현 단계에서 확인하고, 맞지 않으면 신규 쿼리를 **추가**한다(기존 쿼리 수정 안 함).
- `UPLOAD` / `COMMENT` 값을 조건으로 쓰는 쿼리 / 화면 코드는 없다 (`08_DBQuery/ORACLE/*.xml` 과 SEMDMS Control / ENGUI / RO / BSL grep, 2026-10-07). 그래서 `UPLOAD` / `Upload` 로 구분해도 기존 화면 조회에서 빠지지 않는다.
- 원본 WaferID 는 `TQD_WAFER.COMMENT` 대신 `TQD_STEP.COMMENT` 에 남긴다. 기존 `CREATE_WAFER` 쿼리가 COMMENT 를 `''` 로 고정하기 때문이다.

### 6.3 이미지 처리

1. 화면: KLARF 의 `TiffFileName`(1.8 은 `Images` 블록) 이름을 모은다. 찾는 순서 = KLARF 폴더 -> `Defect Images` 하위 폴더 -> 사용자가 지정한 폴더. 찾은 수 / 없는 파일 목록을 미리보기에 표시한다.
2. 전송: 이미지 파일마다 `AddImage` 1회 (파일 1개 = 호출 1회. 큰 다중 TIFF 도 1개씩 보내 Remoting 메시지 크기를 제한한다).
3. 서버: 작업 폴더(`KLARF_UPLOAD_WORK_PATH\<UPLOAD_KEY>\`)에 KLARF 와 이미지를 **한 폴더**로 둔다 -> 기존 `ImageMapper` 가 그대로 이미지를 Defect 에 연결한다 (S4 해결, Parser 수정 없음).
4. 서버: 이미지 저장소 `KLARF_IMAGE_ROOT\yyyy\MM\dd\{LotID}\` (DataService 백업 규칙과 같은 하위 구조, 날짜 = ResultTimestamp) 에 원본 복사 + `ImageManager.SaveThumbnailImage`(썸네일 `thumb_` 접두) / 다중 TIFF 는 `SaveFrameImageAndThumbnail` (HandlerInsp.Backup 187~219행과 같은 함수).
5. KLARF 원본도 같은 폴더에 복사한다 (`INSP_FILEPATH` 가 가리키는 위치).
6. 이미지 파일이 없는 Defect 는 `TQD_IMAGES` 행을 만들지 않는다 (DataService 는 파일이 없어도 행을 만든다. 업로드는 화면에서 빠진 파일을 먼저 보여 주므로 빈 행을 만들 이유가 없다).

`KLARF_UPLOAD_WORK_PATH` / `KLARF_IMAGE_ROOT` 는 SEMDMS Service 설정(`DACrux.SEMDMS.Service.exe.config` appSettings)에 추가한다. POC PC 에서는 Service 와 DACrux 가 같은 PC 에 있다. 고객 환경에서는 클라이언트가 읽을 수 있는 공유 경로여야 한다(결정 D4).

### 6.4 재등록 / 실패 처리

- 기존 등록 판정 키는 DataService 와 같다: `(WAFER_SEQ = LOT_ID + WAFER_ID, STEP_ID, RESULTTIMESTAMP)` (`GetStepInfo`). 미리보기 단계에서 조회해 Wafer 별로 "신규 / 이미 등록됨(STEP_SEQ)" 을 표시한다.
- 이미 등록된 Wafer 는 결정 D1 에 따라 처리한다. 삭제는 기존 Data Maint 삭제 순서(`DefectMapAnalysis.cs:1860~1870`: STEP -> DEFECT -> IMAGES -> INSP_INFO) + `TQD_INSP_SUM` 을 따른다. SETUP / PRODUCT / LOT / WAFER 는 다른 Step 이 함께 쓰므로 지우지 않는다.
- 기존 DataService 처럼 Step 별 자동 커밋이다. Wafer 저장 중 오류가 나면 **이번에 새로 만든 STEP_SEQ 의 행을 삭제**(위 삭제 순서)하고 오류를 돌려준다. 그래서 반쯤 저장된 Wafer 가 남지 않고 같은 파일을 다시 올릴 수 있다. SETUP / PRODUCT / LOT / WAFER 처럼 재사용되는 행은 남아도 다음 저장에서 그대로 재사용된다.
- 작업 폴더는 Commit 성공 / 실패 모두 삭제한다. 하루 지난 작업 폴더는 BeginUpload 때 정리한다(화면이 중간에 닫힌 경우).

### 6.5 화면 설계 (`frmKlarfUpload`, 메뉴 "KLARF Upload")

위치: `02_DMS/DACrux.SEMDMS.ENGUI/DefectMapAnalysis/` (Defect 화면 기준 프로젝트, F06 KLARF 파일 조회와 같은 폴더). `DACruxUXBasic01` 상속, 컨트롤은 Designer 에 배치.

```text
+--------------------------------------------------------------------------------------------+
| KLARF 파일 [..................................] [찾기]  이미지 폴더 [............] [찾기]  [분석] |
+---------------------------------+------------------------------------+---------------------+
| Wafer 목록 (체크 = 저장 대상)      | Map 미리보기 (KlarfDefectMap 재사용)  | 저장 값              |
| 원본WaferID / DMS WaferID / Slot  |  - 검사 Die / Defect / 색상 기준      | Factory  (로그인)    |
| Defect 수 / 이미지 찾음/전체        |                                    | Product  [Hybrid_OMM]|
| 상태: 신규 / 등록됨(STEP_SEQ)      |                                    | Lot      PFIC_...   |
|                                 |                                    | Step ID  [OMM_...]  |
|                                 |                                    | Setup ID ROOT-...(46)|
|                                 |                                    | 설비     [Tool  v]   |
|                                 |                                    | 분류 기준 [RoughBin v]|
|                                 |                                    | 검사시각 2026-06-05  |
+---------------------------------+------------------------------------+---------------------+
| 확인 메시지 (길이 초과 / 없는 이미지 / ClassLookup 과 TQD_DEFECT_TYPE 이름 차이 / 기존 등록)        |
|                                                         [DB 저장]  [결과 Wafer 조회 안내] |
+--------------------------------------------------------------------------------------------+
```

- 미리보기는 저장과 같은 Parser 와 같은 규칙(회전 / Origin Index 조회는 `RO.DefectDefine.GetSetupInfo` 등 기존 원격 함수로 가능한지 구현 때 확인, 안 되면 계산기 값으로 표시하고 "저장 시 DB 기준 적용" 표기)으로 저장될 키 값을 보여 준다.
- 저장 결과 창: Wafer 별 STEP_SEQ / Defect / 이미지 / 소요 시간. "왼쪽 Wafer 조회에서 Lot = PFIC_RH-HS2615_C4BUMP, 기간 2026-06-05 포함" 같은 조회 안내를 표시한다.
- F06 KLARF 파일 조회 화면은 DB 없는 파일 확인용으로 그대로 둔다(결정 D5).

### 6.6 서비스 계약 (기존 DefectDefine 방식: string[] / string[,] / DataTable, DTO 없음)

| 계층 | 신규 파일 | 내용 |
| --- | --- | --- |
| Interface | `DACrux.SEMDMS.Interface/iKlarfUpload.cs` | `DataTable GetUploadedStep(string[] keys)` / `string BeginUpload(string[] info, byte[] klarf)` / `void AddImage(string uploadKey, string fileName, byte[] data)` / `DataTable CommitUpload(string uploadKey, string[] options)` / `void CancelUpload(string uploadKey)` |
| RO | `DACrux.SEMDMS.RO/KlarfUpload.cs` | `Activator.GetObject(url + "/DACrux.SEMDMS.BSL.KlarfUpload.bin")`, 메서드마다 try / throw |
| BSL | `DACrux.SEMDMS.BSL/KlarfUpload.cs` | `BaseComponent` 상속. 기존 `DefectDefine` / `DMReview` 를 같은 프로세스에서 호출. 파일 / 이미지 처리 |
| DSL / XML | 필요 시 기존 `TQD_INSP_INFO.xml` / `TQD_STEP.xml` 등에 **조회 / IMAGES 갱신 / 삭제 쿼리 추가만** | 기존 쿼리 ID 수정 없음. 신규 테이블 없음 |
| 설정 | `101_ServerBin/DACrux.SEMDMS.Service.exe.config` + `02_DMS/DACrux.SEMDMS.Service/App.config` | wellknown 1행 + appSettings 2행 (`KLARF_UPLOAD_WORK_PATH` / `KLARF_IMAGE_ROOT`) |
| 메뉴 | `MenuFile.mnu` 2곳 | `MNU_KLARF_UPLOAD` 맨 뒤 1행 (행 순서 주의: CLAUDE-010) |

`ParserKlarf.GetOriginDieIndex` 는 static 델리게이트다. SEMDMS Service 프로세스 안에서 업로드 시 설정해도 DataService(별도 프로세스)에는 영향이 없다. Service 안의 다른 코드가 이 값을 쓰는지는 구현 단계에서 grep 으로 확인한다.

### 6.7 파일 간 기준 정보 설계와의 관계

같은 날 작성된 [파일 간 기준 정보 관계 설계](2026-10-07_파일간_기준정보_관계_설계.md) (결정 R1~R5 대기) 는 KLARF 를 기존 `TQD_*` 에 두고, 신규 공통 등록 테이블 `TQP_SRC_FILE` 이 `STEP_SEQ` 로 가리키는 구조다. 이 업로드 설계와 충돌하지 않는다.

- 업로드가 만든 `TQD_STEP` 은 그 설계의 "DMS 에 이미 있는 KLARF" 와 같은 형태라 R4 (조회 때 미등록 STEP 자동 등록) 로 그대로 등록된다.
- `TQP_SRC_FILE` 이 확정되면 업로드 BSL 이 Commit 끝에 바로 등록할 수 있다. 그때 원문 WaferID(`XD106.02`) / SetupID 전체 / 원본 파일명을 RAW 칸에 넣으면 6.2 절의 `TQD_STEP.COMMENT` 기록은 보조 수단이 된다. 이 연결은 R1~R5 결정 후 별도 단계로 추가한다 (이번 설계 범위 밖).
- 참고: 그 문서 1.2 절은 기존 적재를 "KtoD" 로 적었으나, 현재 소스의 적재 경로는 `Inspection.DataService` 이고 KtoD 는 예전 경로다 (2절 근거). 저장 테이블은 같다.

### 6.8 구현 1단계에서 확인한 사실 (2026-10-07, Probe 실측)

- 기존 `CREATE_SETUP_INFO` / `SELECT_ORIGIN_INDEX` 는 `:setup_id` 를 size 40 으로 바인딩해 46자 SetupID 를 **오류 없이 40자로 잘라** 저장 / 비교한다. 그래서 D2 DDL 만으로는 부족하고, 업로드는 size 128 쿼리 3개를 `TQD_SETUP.xml` 에 추가해 쓴다.
- `TQD_STEP.REVIEW_EQ`(NOT NULL, DEFAULT ' ')에 DataService 처럼 `""` 를 넘기면 Oracle 에서 NULL 이 되어 ORA-01400. 업로드는 기존 데이터와 같은 `' '` 를 넣는다.
- `TQD_INSP_INFO.SUMMARY_FLAG`(NOT NULL, 기본값 없음)를 기존 `CREATE_INSP_INFO_PARSE` 가 넣지 않아 ORA-01400. 같은 INSERT 에 `' '` 를 더한 `CREATE_INSP_INFO_KLARF_UPLOAD` 를 추가했다.
- 위 두 건은 현재 DataService 코드도 로컬 스키마에서 같은 오류를 낼 수 있다는 뜻이다(기존 데이터는 다른 경로 / 이전 스키마로 들어간 것으로 추정). 자동 적재 경로는 이번 범위가 아니라 수정하지 않았다.
- 재사용: 기존 DSL 의 범용 `GetData / InsertData / DeleteDataNonQuery`, `TQD_INSP_SUM` 은 QueryComponent public `ExecuteNonQuery`. DSL / 기존 BSL 함수 수정 없음.

## 7. 수정 / 신규 파일 범위 (구현 시)

| 구분 | 파일 | 기존 파일 영향 |
| --- | --- | --- |
| 신규 | Interface / RO / BSL `KlarfUpload` 3개, `frmKlarfUpload.cs` / `.Designer.cs`, Probe `KlarfUploadProbe.ps1`, 화면 Smoke | 없음 |
| 추가만 | 각 csproj `<Compile Include>` 1행, 쿼리 XML 에 쿼리 추가(필요 시), Service config 2곳, MenuFile.mnu 2곳 | 기존 행 / 쿼리 / 함수 변경 없음 |
| DDL | 결정 D2 에서 컬럼 확대를 고르면 `TQD_SETUP.SETUP_ID` 1건 (`MODIFY VARCHAR2(40) -> 넓게`) | 기존 값 / 쿼리 영향 없음 (확대만) |
| 수정 안 함 | `InspectionDataService.cs`, `HandlerInsp.cs`, `DACrux.Data.Parser/*`, 기존 BSL / RO / Interface 함수, 기존 화면 | - |

## 8. 구현 단계 (작은 단위)

1. **서버 1단계**: Interface / RO / BSL `KlarfUpload` 의 Begin / AddImage / Commit (이미지 제외, Step 1~9 + 11~12) -> Probe 로 샘플 `XD106.02` 저장 후 `TQD_*` 재조회, DataService 와 같은 값인지 대조 (INSP_INFO 집계 / Index / X,Y).
2. **서버 2단계**: 이미지 저장 + `TQD_IMAGES` + IMAGES 갱신 / 재등록 삭제 / 실패 시 정리 -> Probe.
3. **화면 1단계**: 파일 선택 / 미리보기 / 저장 값 / 확인 메시지 (DB 저장 없음) -> 화면 Smoke.
4. **화면 2단계**: [DB 저장] 연결 / 결과 창 / 메뉴 -> ClientBin / ServerBin 반영.
5. **기존 화면 확인 (사용자)**: 왼쪽 Wafer 조회 -> Defect Map Viewer / Map Gallery / Image Gallery / Chart 에서 샘플 Wafer 확인.

## 9. 검증 방법

| 확인 | 방법 | 기대 결과 (XD106.02) |
| --- | --- | --- |
| 키 / 개수 | Probe 가 저장 후 `TQD_INSP_INFO` / `TQD_DEFECT` / `TQD_SETUP_MAP` / `TQD_IMAGES` 재조회 | INSP_INFO 1행 (TEST 3) / DEFECT 15 / SETUP_MAP 26 / IMAGES 15 / INSP_INFO.IMAGES 15 |
| 좌표 | Defect 1 의 X, Y / XINDEX, YINDEX | X = -23229.42 (파일 X 와 같음) / Index = 원본 + Origin Index |
| 기존 쿼리 통과 | Probe 가 기존 BSL `GetDMSStepWaferList` / `GetDefectMapInfo` 를 같은 STEP_SEQ 로 호출 | 예외 없이 행 반환 |
| 재등록 | 같은 파일 2회 저장 | 결정 D1 대로 동작, DEFECT / IMAGES 중복 0 |
| 실패 정리 | 잘못된 이미지 / 강제 예외 | 새 STEP_SEQ 행 0, 작업 폴더 삭제 |
| 화면 | 사용자가 DMS 왼쪽 Wafer 조회 -> Map Viewer / Image Gallery | Map / Defect 15 / 이미지 썸네일 표시 |

## 10. 사용자 결정 필요

| ID | 항목 | 선택지 | 권장 |
| --- | --- | --- | --- |
| D1 | 같은 Wafer / Step / 검사시각을 다시 올릴 때 | (1) 기존 삭제 후 재등록 (화면에서 확인) / (2) 건너뜀(기존 DataService 동작) / (3) 오류로 막음 | (1) — GDS Map / E-Test 와 같은 정책 |
| D2 | SETUP_ID 가 40자를 넘을 때 (샘플 46자) | (1) `TQD_SETUP.SETUP_ID` 를 VARCHAR2(128) 로 확대 (DDL 1건, 기존 데이터 영향 없음, 고객 DB 에도 적용 필요) / (2) 앞 40자로 자르고 원본은 STEP.COMMENT 에 기록 / (3) 오류로 막음 | (1). (2) 는 다른 Setup 이 같은 40자로 합쳐질 위험 |
| D3 | 분류 컬럼 (샘플은 CLASSNUMBER 0, ROUGHBIN 에 실제 분류) | (1) 화면 "분류 기준" 기본 RoughBin: CLASSNUMBER = ROUGHBINNUMBER 로 저장, 원래 ROUGHBIN 도 그대로 저장 / (2) 파일 그대로 (CLASSNUMBER 0) | (1). Class 이름(TQD_DEFECT_TYPE)은 자동 등록하지 않고, 화면에서 ClassLookup 과 다른 코드를 보여 주며 기존 Defect Type Define 화면에서 등록 |
| D4 | 이미지 경로 형식 / 저장 위치 | (1) 절대 경로(공유 폴더 / UNC) 저장: Image Gallery(DB 경로 직접 사용) 동작, 로컬 기존 데이터와 같은 형식 / (2) FTP 상대 경로 저장: Map Viewer 이미지 목록(FTP) 동작, DataService 와 같은 형식, FTP 서버 필요 | (1) + POC 는 `D:\DMS_IMAGE` 같은 Service PC 폴더. Map Viewer 이미지 목록(FTP 경유)은 기존 데이터도 같은 제한이 있어 별도 확인 |
| D5 | 화면 형태 | (1) 신규 화면 `KLARF Upload` (F06 조회 화면은 그대로) / (2) F06 KLARF 파일 조회 화면에 [DB 저장] 추가 | (1). 조회와 저장의 역할을 나누고 F06 Smoke 검증을 그대로 유지 |

### 10.1 2026-10-07 사용자 결정

| ID | 결정 | 설계 반영 |
| --- | --- | --- |
| D1 | 삭제 후 재등록 | 6.4 절대로. 미리보기에서 "등록됨" Wafer 는 저장 전 확인창 -> 기존 STEP 행 삭제 후 저장 |
| D2 | `TQD_SETUP.SETUP_ID` 를 VARCHAR2(128) 로 확대 | 구현 1단계 전에 DDL 1건 작성 -> 로컬 DB 적용(승인 후). 고객 DB 에도 같은 DDL 필요 (배포 목록에 기록) |
| D3 | RoughBin 을 Class 로 | 화면 "분류 기준" 기본 RoughBin (CLASSNUMBER = ROUGHBINNUMBER, ROUGHBIN 원값 유지). 선택지 Class(파일 그대로) / FineBin. Class 이름은 자동 등록 안 함 |
| D4 | **FTP 상대 경로** (권장안과 다름) | 6.3 절 아래 "D4 반영" 참고 |
| D5 | (미질의) | 권장안 (1) 신규 화면으로 진행. 다르게 원하면 구현 3단계 전에 변경 |

**D4 반영 (FTP 상대 경로):**

- 이미지 저장 위치 `KLARF_IMAGE_ROOT` 는 **FTP 서버가 공개하는 폴더 아래**여야 한다. DB 에는 DataService 와 같은 규칙(`Handler.GetImageRelativePath`: 드라이브 루트를 떼고 `\` -> `/`)의 상대 경로를 넣는다. 예: `D:\DMS_FTP\KLARF_UPLOAD\2026\06\05\PFIC_RH-HS2615_C4BUMP` -> `DMS_FTP/KLARF_UPLOAD/2026/06/05/PFIC_RH-HS2615_C4BUMP`. 화면은 `TQC_CONFIG DEFECT_FTP.MAIN_PATH` + 이 경로로 받는다. 그래서 FTP 루트 = 드라이브 루트(`/%2f/` 가 이 의미) 인 설정과 맞아야 한다. 직접 상대 경로 규칙을 바꾸지 않고 기존 함수와 같은 결과를 낸다.
- **동작하는 화면**: Defect Map Viewer 이미지 목록 / `DPUCDefectImgGallery` (FTP 다운로드 `fnFTPImageDownload`).
- **제한**: Image Gallery 썸네일 / 팝업(`fnFTPGetStream`, 260223 Chris 변경)은 DB 경로를 그대로 `WebRequest.Create` 에 넘긴다. 상대 경로는 URI 가 아니라서 이 화면에서는 이미지가 보이지 않을 수 있다. DataService 가 넣는 자동 적재 데이터도 같은 제한을 갖는다(같은 형식). 이 화면 코드는 기존 소스라 수정하지 않는다. 화면 확인 때 결과를 기록하고, 필요하면 별도 결정으로 다룬다.
- **환경 확인 필요**: 로컬 DB 의 `DEFECT_FTP` 는 172.16.15.100 이다. POC PC 에서 이 FTP 에 접속되는지, 아니면 POC PC 에 FTP 를 두고 `TQC_CONFIG` 값을 바꿀지 결정해야 Map Viewer 이미지를 확인할 수 있다 (TQC_CONFIG 변경은 DB 데이터 변경이라 승인 후).

추가 확인 (고객 / 협의): 고객 KLARF 1.8 파일 사용 여부 (저장소에 1.8 샘플 없음, `ParserKlarf_18` 은 예외 처리가 없어 업로드 BSL 이 감싸서 오류 메시지로 돌려준다) / KLARF DeviceID(`Hybrid_OMM`) 와 GDS Device ID 가 같은 이름인지 (다르면 화면 Product 값으로 맞춘다).

## 11. 위험과 제한

- 자동 적재(DataService)와 수동 업로드가 같은 Wafer / Step / 시각을 동시에 넣으면 키 중복이 생길 수 있다. POC 에서는 DataService 를 운영하지 않는다는 전제다(확인 필요).
- Remoting 으로 보내는 이미지 1개 크기에 제한을 둔다 (예: 50MB 초과 파일은 화면에서 제외 안내). 실제 고객 이미지 크기는 샘플 jpg 기준 수십 KB.
- `TQD_DEFECT_TYPE` 은 Factory 구분이 없는 전역 코드다. 고객 코드(53 RDLopen)와 로컬 예전 데이터(53 STI Dimple)가 충돌한다. 고객 DB 는 새로 쓰는지 확인이 필요하다.
- 업로드 데이터는 COMMENT(`Upload`) / UPLOAD(`UPLOAD`) 로 구분된다. 기존 Data Maint 화면의 삭제 기능으로도 지울 수 있다.
