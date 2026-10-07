# F08 / KLARF 업로드 / DB 적재

최종 갱신: 2026-10-07 KST / 작성 도구: Claude (CLAUDE-031)
상태: **구현 완료 + 배포(2026-10-07 12:12)** / Probe·Smoke 확인 / **DMS 화면 확인 대기** / 설계 확정 (2026-10-07 사용자 결정: D1 삭제 후 재등록 / D2 SETUP_ID VARCHAR2(128) 확대 / D3 RoughBin -> CLASSNUMBER / D4 이미지 경로 FTP 상대 경로 / D5 미질의 -> 신규 화면 권장안. 소스 / DB 변경 없음)

D4 영향: Map Viewer 이미지 목록(FTP 다운로드)은 동작, Image Gallery 썸네일(`fnFTPGetStream` 이 DB 경로 직접 사용)은 상대 경로라 안 보일 수 있음(자동 적재 데이터와 같은 제한, 기존 소스 수정 안 함). 이미지 저장 폴더는 FTP 공개 폴더 아래여야 하고, 로컬 `DEFECT_FTP`(172.16.15.100) 접속 가능 여부 확인 필요.

## 목적

엔지니어가 KLARF 파일과 Defect 이미지를 화면에서 올리면 기존 DMS 자동 적재(Inspection.DataService)와 같은 `TQD_*` 테이블 / 같은 값 규칙으로 저장한다. 그래서 기존 Defect 조회 화면(왼쪽 Wafer 조회 -> Defect Map Viewer / Gallery / Image Gallery / Chart)과 POC Correlation 이 별도 수정 없이 같은 데이터를 읽는다.

상세 설계: [2026-10-07 KLARF 업로드 / DB 적재 설계](../../../문서/2026-10-07_KLARF_Upload_DB적재_설계.md)

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
- DataService 와 의도적으로 다른 점: Step ID 8자리 규칙 / LotID 앞 'A' 제거(FAB1 규칙) 미적용, ANGLE 은 Parser 각도 사용, SETUP_MAP 은 Test 별 계획으로 작성, INSP_INFO / STEP 의 IMAGES 를 실제 이미지 수로 저장, 이미지 파일 없는 행 미생성, Alarm 메일 미실행.

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
