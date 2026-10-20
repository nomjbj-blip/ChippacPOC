# F08 / KLARF 업로드 / DB 적재

최종 갱신: 2026-10-07 KST / 작성 도구: Claude (CLAUDE-031)
상태: **구현 완료 + 배포(2026-10-07 12:12)** / Probe·Smoke 확인 / **DMS 화면 확인 대기** / 설계 확정 (2026-10-07 사용자 결정: D1 삭제 후 재등록 / D2 SETUP_ID VARCHAR2(128) 확대 / D3 RoughBin -> CLASSNUMBER(**2026-10-15 폐기, KLARF 표준대로 파일 값 그대로**) / D4 이미지 경로 FTP 상대 경로 / D5 미질의 -> 신규 화면 권장안. 소스 / DB 변경 없음)

D4 영향: Map Viewer 이미지 목록(FTP 다운로드)은 동작, Image Gallery 썸네일(`fnFTPGetStream` 이 DB 경로 직접 사용)은 상대 경로라 안 보일 수 있음(자동 적재 데이터와 같은 제한, 기존 소스 수정 안 함). 이미지 저장 폴더는 FTP 공개 폴더 아래여야 하고, 로컬 `DEFECT_FTP`(172.16.15.100) 접속 가능 여부 확인 필요.

## 목적

엔지니어가 KLARF 파일과 Defect 이미지를 화면에서 올리면 기존 DMS 자동 적재(Inspection.DataService)와 같은 `TQD_*` 테이블 / 같은 값 규칙으로 저장한다. 그래서 기존 Defect 조회 화면(왼쪽 Wafer 조회 -> Defect Map Viewer / Gallery / Image Gallery / Chart)과 POC Correlation 이 별도 수정 없이 같은 데이터를 읽는다.

상세 설계: [2026-10-07 KLARF 업로드 / DB 적재 설계](../../../문서/설계/03_KLARF_Map/2026-10-07_KLARF_Upload_DB적재_설계.md)

## 파일과 호출 흐름 (설계)

```text
frmKlarfUpload (신규, SEMDMS.ENGUI/DefectMapAnalysis)
 -> RO.KlarfUpload (신규) -> iKlarfUpload (신규)
 -> BSL.KlarfUpload (신규, SEMDMS Service 프로세스)
     -> 기존 Parser (FileAnalyzer / ParserKlarf / ParserKlarf_18, 수정 없음)
     -> 기존 BSL DefectDefine / DMReview public 함수 (수정 없음)
     -> TQD_SETUP / SETUP_MAP / PRODUCT / LOT / WAFER / STEP / DEFECT / INSP_INFO / IMAGES / INSP_SUM
```

참조 구현(정답 순서): `02_DMS/DACrux.SEMDMS.Inspection.DataService/InspectionDataService.cs:155~586` (Step 1~13).

## 중요한 로직 (설계 기준)

- 새 테이블 없음. 기존 화면은 모두 `STEP_SEQ` 로 `TQD_INSP_INFO` / `TQD_SETUP` / `TQD_SETUP_MAP` / `TQD_DEFECT` / `TQD_IMAGES` 를 읽는다.
- 파싱 / 저장은 서버에서 한다(A안). 화면은 미리보기와 파일 전송만 한다. 이미지는 서버 작업 폴더에 KLARF 와 한 폴더로 모아 기존 `ImageMapper` 가 그대로 연결하게 한다.
- 고객 샘플 `XD106.02` 대조 결과 기존 자동 적재 규칙으로는 실패한다: SetupID 46자 > `TQD_SETUP.SETUP_ID` 40자 / Step ID 8자리 규칙 예외 / 이미지가 하위 폴더. 업로드 쪽 규칙으로 흡수하고 기존 소스는 고치지 않는다.
- 업로드 데이터 구분: `TQD_STEP.COMMENT` = `UPLOAD;USER=..;RAW_WAFER_ID=..` / `TQD_INSP_INFO.COMMENT` = `Upload` / `TQD_IMAGES.UPLOAD` = `UPLOAD`. 이 값으로 거르는 기존 쿼리 / 화면 코드는 없다(grep 확인).
- DataService 와 의도적으로 다른 점: Step ID 8자리 규칙 / LotID 앞 'A' 제거(FAB1 규칙) 미적용, ANGLE 은 Parser 각도 사용, SETUP_MAP 은 Test 별 계획으로 작성, INSP_INFO / STEP 의 IMAGES 를 실제 이미지 수로 저장, 이미지 파일 없는 행 미생성, Alarm 메일 미실행, `TQD_INSP_INFO.DEFECTIVE_DIE` 를 Test 별로 저장(2026-10-19 CLAUDE-147).
- `TQD_INSP_INFO` 는 (STEP_SEQ, TEST) 행이다. DEFECTS / CLASSIFIED / RANDOM / DEFECTIVE_DIE 모두 그 Test 의 DefectList 로 센다(원본 SummaryList NDEFECT / NDEFDIE 는 쓰지 않음). DataService 는 DEFECTIVE_DIE 에 STEP 전체 Defect Die 수를 모든 Test 행에 넣어, Test 가 2개인 파일에서 Defect 없는 Test 행에도 같은 값이 들어갔다(F07DATA 검증 D-02).
- Defect Map 정보 창 / Chart 는 `GET_INSP_INFO_MULTI_SEQ` 로 STEP 당 한 행만 읽는다. 2026-10-19 부터 주 Test(Defect 가 가장 많은 Test, 같으면 작은 번호) 행을 고른다(기존 쿼리 변경, 업로드 / DataService 데이터 모두 해당). 이전에는 Test 행 순서가 정해지지 않아 Defect 0 인 Test 80 행이 골라졌다.

## 구현 상태 (2026-10-07 구현 1단계: 서버, 이미지 제외)

| 파일 | 내용 |
| --- | --- |
| [iKlarfUpload.cs](../../02_DMS/DACrux.SEMDMS.Interface/iKlarfUpload.cs) | BeginUpload / AddImage / CommitUpload / CancelUpload / DeleteUploadedStep |
| [RO KlarfUpload.cs](../../02_DMS/DACrux.SEMDMS.RO/KlarfUpload.cs) | `DACrux.SEMDMS.BSL.KlarfUpload.bin` 원격 호출 |
| [BSL KlarfUpload.cs](../../02_DMS/DACrux.SEMDMS.BSL/KlarfUpload.cs) | 작업 폴더(`KLARF_UPLOAD_WORK_PATH`, 없으면 %TEMP%\DACrux_KlarfUpload) / 파싱(Origin Index 조회 델리게이트를 size 128 쿼리로 잠시 교체, lock) / Step 1~9, 11~12 / 재등록 삭제 / 실패 시 새 STEP 정리 |
| 쿼리 XML (추가만) | `TQD_SETUP.xml` SELECT_SETUP_SEQ_ / CREATE_SETUP_INFO_ / SELECT_ORIGIN_INDEX_KLARF_UPLOAD, `TQD_INSP_INFO.xml` CREATE_INSP_INFO_KLARF_UPLOAD, `TQD_INSP_SUM.xml` DELETE_INSP_SUM_KLARF_UPLOAD, `TQD_STEP.xml` SELECT_STEP_KLARF_UPLOAD |
| DDL | [2026-10-07_tqd_setup_setup_id_128.sql](../DB/sql/2026-10-07_tqd_setup_setup_id_128.sql) 로컬 적용, 고객 DB 미적용 |
| Probe | [KlarfUploadProbe.ps1](../../02_DMS/DACrux.SEMDMS.BSL/Probes/KlarfUploadProbe.ps1) (`-BslDir`, `-Keep`) |

실측으로 확인한 기존 경로와의 차이 (업로드 쪽에서 흡수):
- 기존 `CREATE_SETUP_INFO` / `SELECT_ORIGIN_INDEX` 는 SETUP_ID 를 size 40 으로 바인딩해 46자가 **오류 없이 40자로 잘림** -> size 128 추가 쿼리.
- `TQD_STEP.REVIEW_EQ` 에 `""` 는 NULL 이라 ORA-01400 -> `' '`. `TQD_INSP_INFO.SUMMARY_FLAG` 를 기존 INSERT 가 넣지 않아 ORA-01400 -> `' '` 추가 쿼리.

### 2단계 이미지 (2026-10-07)

- CommitUpload 가 DB 저장 전에 `KLARF_IMAGE_ROOT \ yyyy\MM\dd\{Lot}`(기존 `parser.GetPathForBackup()`)에 KLARF / 이미지 원본 / `thumb_` 썸네일을 저장한다(HandlerInsp.Backup 과 같은 함수). DB 경로 = 드라이브 루트를 뗀 '/' 상대 경로(D4).
- TQD_IMAGES 는 GetImage2DArray 와 같은 열(UPLOAD = 'UPLOAD'). **이번 업로드에서 복사한 파일만** 행을 만든다(같은 폴더에 이전 파일이 남아 있을 수 있음).
- INSP_INFO IMAGES / CLASSIFIED 는 기존 `UPDATE_INSP_INFO[REVIEW_PARSER]`, STEP IMAGES 는 추가 쿼리 `UPDATE_STEP_IMAGES_KLARF_UPLOAD`. 그래서 왼쪽 조회 "Include Image"에도 걸린다.
- 설정이 없는데 이미지가 있으면 DB 저장 전에 오류.

### 3~4단계 화면 / 배포 (2026-10-07)

- 화면 [frmKlarfUpload.cs](../../02_DMS/DACrux.SEMDMS.ENGUI/DefectMapAnalysis/frmKlarfUpload.cs) / Designer, 메뉴 `MNU_KLARF_UPLOAD`(DMS > KLARF 업로드, F03119). [분석] 은 DB 를 쓰지 않고, [DB 저장] 이 Begin -> 재등록 확인 -> 이미지 전송 -> Commit 을 호출한다. 이미지 폴더는 비워 두면 KLARF 폴더 / `Defect Images` 중 더 많이 찾는 곳을 쓴다.
- Smoke: [Verify-KlarfUpload.ps1](../../02_DMS/DACrux.SEMDMS.ENGUI/Verification/Verify-KlarfUpload.ps1) (화면 -> RO -> BSL 을 Service 없이 한 프로세스에서, 로컬 DB 사용, `-Keep`).
- Service config: wellknown `KlarfUpload` + `KLARF_IMAGE_ROOT = D:\FTP\SCK`. 저장 폴더 = 루트 \ yyyyMMdd(저장일) \ Lot (2026-10-07 사용자 지시). DB 이미지 경로 예: `FTP/SCK/20261007/PFIC_RH-HS2615_C4BUMP`.
- FTP: Docker 컨테이너 `dms-ftp`(vsftpd, 127.0.0.1:21 / Passive 21000~21010, 계정 dacrux, D:\FTP -> /FTP 읽기 전용). TQC_CONFIG DEFECT_FTP = IP 127.0.0.1 / PORT 21 / MAIN_PATH /%2f/ / PASS 는 새 시험 값(문서에 적지 않음). 기존 HFtpClient 로 다운로드 확인(2026-10-07).
- 배포 DLL 은 E-Test(CLAUDE-029) 진행 중 파일을 뺀 임시 csproj 로 빌드했다. E-Test 쪽이 나중에 전체 소스로 배포하면 KLARF 업로드 코드도 함께 들어간다(정상).

- 이미지 폴더 선택은 탐색기 형식 창(`FolderPickerDialog.cs`, IFileOpenDialog 폴더 선택 모드, 실패 시 기존 창). 이미지 폴더 자동 찾기는 사용자가 화면 코드에서 뺐다(2026-10-07) -> [폴더 선택]으로 직접 지정.
- Probe / Smoke 는 샘플 Wafer 에 사용자가 올린 데이터(STEP COMMENT USER 가 PROBE / SMOKE 아님)가 있으면 바꾸지 않고 종료 코드 2 로 멈춘다(재등록으로 사용자 데이터를 지운 사고 후 추가).

### 화면 재배치 (2026-10-08 CLAUDE-048)

- [분석] 버튼 / 처리 기록 없음. [파일 선택] / [폴더 선택] 을 누르면 바로 읽는다(입력란은 읽기 전용).
- 위: 파일 묶음 + 저장 값 묶음 + 큰 [DB 저장], 그 아래 KLARF 요약 한 줄, 본문: Wafer Map | Wafer 목록 + KLARF Defect Grid(저장 Class / 분류명 / 이미지 여부 / 좌표 / Bin). 2026-10-14 CLAUDE-100 기능 최소화: 행 선택 -> Map 강조 / 더블클릭 -> 이미지 열기 / Map 아래 정보 글 삭제.
- 속도 (2026-10-14 CLAUDE-100): Defect Grid 는 열 자동 너비(AllCells)를 쓰지 않는다. 행 1개마다 전체 행 너비를 다시 재서 1,122건 파일(1107236219.SINF)이 239초 걸렸다. 행 한 번에 추가 + 보이는 행 기준 너비 1회 -> 1.4초.
- 버튼 아이콘: `DMS/99_Resource/ByModule/POC/BTN_*.png`(make_button_icons.ps1, 메뉴 아이콘과 같은 그림 방식) 를 `frmKlarfUpload.resx` 에 이미지(base64)로 넣어 사용 (2026-10-14 확인, 빌드 때 PNG 파일을 읽지 않음).

- Map Setup 확인(2026-10-13): Product(기본 KLARF DeviceID)에 `TQP_MAPDEF`(FACTORY + MAPID) Map 이 없으면 미리보기 INVALID / 저장 ERROR. Map Define 또는 File Map Setup 에서 Map ID = Device 로 먼저 등록해야 한다.
- **Map Setup 기준 저장 (2026-10-13 CLAUDE-083, 사용자 결정)**: Defect 조회 화면은 Map Setup(`TQP_MAPDEF` / `TQP_USEMAP`, Map ID = Device) Map 을 그대로 보이고 KLARF 는 Defect 정보만 쓴다. KLARF 검사 Die(SampleTestPlan) / Pitch 로 Map 을 만들지 않는다.
  - `TQD_SETUP_MAP` = Test 마다 Map Setup 사용 Die(USECODE 1) 전체 (LeftBottom 번호). `TQD_SETUP` 의 WAFER_SIZE / DIE_PITCH / DIE_ORIGIN / ORIGIN = Map Setup 값. 새 `TQD_PRODUCT` NETDIE = Map 사용 Die 수. `TQD_INSP_INFO.INSPECTED_DIE` 는 실제 검사 Die 수 그대로.
  - 번호 변환: Map Setup 저장 방향(XY_DIRECTION) -> LeftBottom 은 File Map Setup 과 같은 식(`KlarfMapRule.FromDirection` / `StoredMapLayout.ToLeftBottomOrigin`, BSL 은 TEST.ENGUI 를 참조 못 해 `KlarfUpload` 안에 같은 식). 파싱 뒤 Defect / 검사 Die 번호를 Map Setup 원점으로 옮긴다 (OMM_Hybrid 는 KLARF 원점과 같은 (7, 5) 라 변화 없음).
  - 같은 Setup(SETUP_ID + STEP_ID + SETUP_TIME)이 예전 규칙으로 있으면 저장할 때 Setup 값 UPDATE / Setup Map 지우고 다시 넣음 (재업로드 시 교체). 이 Setup 을 쓰는 다른 STEP 도 같이 바뀐다.
  - 미리보기 / 결과 표: `MAP_DIES`(저장할 Die 수), `MAP_NOTE`(Pitch / 원점 다름, Map 밖 검사 Die / Defect 수, MESSAGE 에도 표시). Map 이 없거나 삭제 / 단위(um 아님) / XY 방향 오류 / 사용 Die 0 이면 INVALID.
  - 쿼리 추가: `TQD_SETUP.xml` UPDATE_SETUP_INFO_KLARF_UPLOAD / `TQD_SETUP_MAP.xml` DELETE_SETUP_MAP_KLARF_UPLOAD. DSL `DeleteData` / `UpdateData` 는 내부가 GetDataTable 이라 `*NonQuery` 사용.
  - 참고: 기존 메뉴 Defect Bare Map View(`GetBareDefectMapViewer`)도 Map Setup Map + Defect 개념이지만 Die 를 `TQP_MAPDEF_DIE` 에서만 읽고(File Map Setup 은 TQP_USEMAP 저장, OMM_Hybrid 0행) 원점 번호를 저장값 그대로 써 OMM_Hybrid 에는 맞지 않는다 (화면 미확인). 사용자 결정: 일반 Defect Map View 를 쓰고 업로드에서 맞춘다.
  - 검증: `02_DMS/DACrux.SEMDMS.BSL/Probes/KlarfUploadSetupMapProbe.ps1` -> `KLARF_SETUPMAP_PROBE=PASS (18)` (TEMP BSL). **DMS 화면 확인 완료** (2026-10-13 사용자 재업로드: XD106.02 / HS2617B_TBDB, Setup Map 76). 이미 올린 데이터는 덮어쓰기 재업로드 때만 바뀐다(건너뜀이면 그대로).

### 분류 = KLARF 표준 (2026-10-15 CLAUDE-112, 결정 D3 폐기)

- 사용자 지시 "표준대로 작업 해줘". KLARF 표준에서 ClassLookup 은 CLASSNUMBER 의 이름표다.
- 서버 `BSL/KlarfUpload.cs`: RoughBin -> CLASSNUMBER 복사(`ApplyClassSource`) 삭제. CLASSNUMBER / ROUGHBINNUMBER / FINEBINNUMBER 를 파일 값 그대로 저장 (기존 자동 적재와 같음). UploadInfo 7개 계약은 유지하고 6번은 예약(빈 값, 받기만 하고 무시 - 옛 화면이 ROUGHBIN 을 보내도 표준대로 저장). STEP COMMENT 의 `CLASS_SRC` / 결과 표 `CLASS_SOURCE` 열 삭제.
- 화면 `frmKlarfUpload`: "분류 기준" 콤보 / "저장 Class" 열 삭제. Defect 목록 = Class(CLASSNUMBER) + 분류명(ClassLookup[CLASSNUMBER]), Map 색 = CLASS.
- 영향: 고객 샘플(CLASSNUMBER 전부 0, 분류 코드는 ROUGHBIN)은 DMS Class 가 모두 0(Unclassified)으로 저장된다. ROUGHBINNUMBER 는 TQD_DEFECT 에 그대로 남는다.
- 이미 올린 데이터(STEP COMMENT 에 `CLASS_SRC=ROUGHBIN`, 예: STEP_SEQ 528 / 541)는 예전 규칙(CLASSNUMBER = ROUGHBIN) 그대로다. 표준으로 맞추려면 화면에서 재등록(예).
- 검증: TEMP 빌드 오류 0, `KLARF_UPLOAD_FORM_SMOKE=PASS`(읽기 검사, 저장 검사는 사용자 STEP 528 보호로 건너뜀), `KLARF_SETUPMAP_PROBE=PASS (19)`(저장 Defect CLASSNUMBER 0 / ROUGHBIN 53,54 검사 추가). `KlarfUploadProbe` 는 사용자 데이터 보호로 저장 검사 건너뜀(기대값만 표준으로 수정).

## 설계만 된 부분과 미완료

- 이미지 화면 표시: Map Viewer 이미지 목록은 FTP(`TQC_CONFIG DEFECT_FTP`) 경유라 POC PC FTP 환경 결정 필요. `KLARF_IMAGE_ROOT` 는 FTP 루트(MAIN_PATH `/%2f/` = 드라이브 루트) 아래여야 한다.
- 결정 완료(설계 문서 10.1절). 남은 확인: POC PC 의 FTP 환경(Map Viewer 이미지 확인용), D2 DDL 적용 승인.
- 고객 확인: KLARF 1.8 사용 여부, KLARF DeviceID 와 GDS Device ID 의 대응.

## 검증

- 2026-10-07 설계: 소스 확인 + 로컬 docker DMSMGR 조회.
- 2026-10-07 구현 1단계: **빌드 확인**(TEMP, 오류 0 / 경고 0) + **Probe 확인**(`KLARF_UPLOAD_PROBE=PASS (30)`, TEMP BSL, 로컬 docker DMSMGR): 저장 후 기존 화면 BSL(`GetDMSStepWaferList` / `GetDefectMapInfo` / `GetDefectData` / `GetDefectMapViewer_Info`)로 재조회 일치. 화면 확인 / 배포 / Service Remoting 경유는 미확인.
- 2026-10-07 구현 2단계: 빌드 확인 + **Probe 확인 `KLARF_UPLOAD_PROBE=PASS (38)`** (이미지 15 저장 / 파일 / 경로 / GetDefectImagePath01 / Include Image / 14장 처리 / 설정 없음 오류). 로컬 DB 확인용 STEP_SEQ 436 남김.

## 다음 단계

사용자: SEMDMS Service + DACrux 재시작 -> DMS > KLARF 업로드 화면에서 XD106.02 저장 -> 왼쪽 Wafer 조회(Lot PFIC_RH-HS2615_C4BUMP / 기간 2026-06-05) -> Map Viewer / Gallery 확인. Map Viewer 이미지 표시까지 확인. 관련: [작업 현황](../작업_현황.md) CLAUDE-031 / [변경 이력](../변경이력/2026-10-07.md).
- 2026-10-14 CLAUDE-094: 화면 아래 공통 진행 표시줄(파일 읽기 / 원본 WaferID / 이미지 찾기, 저장 = 서버 확인 / 이미지 전송 n / 전체 / DB 저장을 작업 스레드에서, 확인 창은 그 사이). `Verify-KlarfUpload.ps1` 은 읽기 검사만 PASS(저장 경로는 원래 Smoke 가 건너뜀 -> 저장 진행 표시는 DMS 화면 확인 대기). 검증 스크립트에 TEST.BSL 복사 추가(BSL 이 TEST.BSL 참조). [POC 화면 진행 표시줄](../../../문서/설계/06_환경_운영/2026-10-14_POC_화면_진행표시줄.md)

## 2026-10-16 단위 확인 / KLARF 1.8 DSIZE 수정 (CLAUDE-125 / 126)

- 단위: KLARF 1.2 는 DiePitch / SampleCenter / X / Y / XREL / YREL / XSIZE / YSIZE / DSIZE = µm, DEFECTAREA = µm², SampleSize = mm. 파서 / 업로드 / `TQD_DEFECT` 모두 변환 없이 µm 로 저장한다(DB 값 = 파일 값 확인).
- KLARF 1.8 은 파서(`06_CoreLibSource/DACrux.Data.Parser/Klarf/ParserKlarf_18.cs`)가 nm 로 보고 ÷1000(면적 ÷1e6) 한다. 그런데 DSIZE 만 ÷100000 이어서 1/100 크기로 저장됐다.
- 수정(2026-10-16, CoreLib 예외 승인): DSIZE 도 ÷1000. 같은 파서를 기존 DMS 1.8 적재(`InspectionDataService`, `HandlerInsp`)도 쓰므로 함께 바뀐다. 로컬 DB 에 1.8 적재 데이터는 없다.
- 검증: 1.2 샘플 Defect 30개를 nm 값의 1.8 형식으로 옮긴 합성 파일 -> `FileAnalyzer` 1.8 판별, 수정 전 DLL DSIZE 0.4526(1/100) / 수정 후 45.26 = max(XSIZE, YSIZE), XREL / XSIZE / AREA 30건 모두 1.2 값과 같음. 1.8 실제 파일은 아직 없다(받으면 nm 가정 자체를 다시 확인).
- 배포 안 함: `71_CoreLib/DACrux.Data.Parser.dll` 은 VS 빌드로 바뀐다.

## 2026-10-19 DEFECTIVE_DIE Test 별 / 정보 창 주 Test (CLAUDE-147)

- 수정: `KlarfUpload.cs` Step 08 DEFECTIVE_DIE = 그 Test Defect 의 (XINDEX, YINDEX) 수(Step 07 끝 STEP 전체 Die 조회 삭제). `TQD_INSP_INFO.xml` `GET_INSP_INFO_MULTI_SEQ` RN 순서 = `B.MAPID, A.DEFECTS DESC NULLS LAST, A.TEST`.
- 확인 수준: BSL **빌드 확인**(TEMP, 오류 0 / 경고 0). 쿼리 **Probe 확인**(SELECT 만): STEP 582 정보 창 행 Test 3 = 1122 / 76 / 51(67.11%), 337 / 364 / 541 / 563 도 Defect 최다 Test. 리포트 `.testagent/reports/run-20261019-113337-D02/report.md`.
- 배포 후 **DB 확인**(13:02): 사용자 VS 빌드(BSL bd371122) + 서비스 재시작 + TEST.SINF 재업로드 -> 새 STEP 601 Test 80 행 = 0 / 761 / 0, `f07data_klarf_check.py` K11 / K11b PASS. 쿼리 XML 은 운영 경로 파일이라 Service 재시작 때 적용됐다.
- 주의: 업로드의 기존 STEP 찾기는 Lot + Product 기준이다. 화면은 파일을 고르면 Product 칸을 KLARF DeviceID 로 자동으로 채운다(`frmKlarfUpload.cs:142-144`). 재업로드 때 처음과 다른 Product(OMMTCB)로 저장하면 기존 STEP 을 지우지 않고 새로 만든다.
- 17:15 **DB 확인**: Product OMM_TCB 로 재업로드 -> 582 / 541 삭제, 새 STEP 603(SCKLOTIDA-01) / 602(PFIC_U9-FS2635_C4BUMP-01) Test 80 = 0 / 761 / 0. `f07data_klarf_check.py` 12/13(K3 / K11 / K11b PASS, K5b = 원본 파일 D-01).
- OMMTCB 중복 STEP 601(SCKLOTIDA-01) / 563(PFIC_U9-FS2635_C4BUMP-01)은 지금은 그대로 두기로 결정(2026-10-19). 연관 분석 [준비]에서 두 Wafer 의 KLARF Step 이 두 줄로 보일 수 있고, 분석은 OMM_TCB 쪽(603 / 602)을 쓴다. 563 의 Test 80 DEFECTIVE_DIE 는 예전 값 51. 정리할 때는 Data Maint 삭제 후 DB 재조회로 확인한다(이번 삭제는 반영되지 않았음).
- 남은 것: DMS 화면 확인. DataService 적재분(337 / 364)의 DEFECTIVE_DIE 는 그대로(기존 코드 미수정).

## 2026-10-20 Defect Image 카드 단순화 (CLAUDE-159)

- 업로드한 이미지를 보는 Defect Map Viewer 이미지 목록과 상세보기 이미지 창의 카드를 `Control/DPUCDefectImageCard`(컨트롤 1개 / 원본 비율 / 2줄 `Defect / Die`, `Class / Bin / 크기` / 더블클릭 = 기존 ImagePopUp + 제목 줄 전체 정보)로 바꿨다. 기존 `DPUCDefectInfo` 는 Defect Source Analysis / TEST Wafer Map Matching 이 계속 쓴다.
- Bin 을 카드에 보이는 이유: 표준 업로드(CLAUDE-112) 이후 고객 데이터는 CLASSNUMBER 0, 분류 코드는 ROUGHBIN 이다.
- 확인: TEMP 빌드 + 카드 확인 16/16(STEP 528). 연관 분석 Image 탭은 CLAUDE-160 뒤에 교체. 배포 / DMS 화면 확인 대기. 설계: `문서/설계/03_KLARF_Map/2026-10-20_Defect_Image_카드_단순화_설계.md`
