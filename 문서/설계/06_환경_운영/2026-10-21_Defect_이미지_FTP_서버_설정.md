# Defect 이미지 FTP 서버 설정 (10.86.253.162 / `/DMS_IMAGE`)

작성: 2026-10-21 Claude (CLAUDE-174) / 상태: **코드 + 스크립트 완료, 서버 / TRAINING 적용 대기**

요청: "image 관련 FTP 를 아래로 변경 10.86.253.162 21 POC/POC, /DMS_IMAGE 경로" -> "KlarfUpload 에 KLARF_IMAGE_FTP_BASE 설정을 추가하고 서버 설정 방법을 알려줘"

## 1. 이미지가 오가는 길

| 단계 | 어디 | 값 |
| --- | --- | --- |
| 업로드 저장 | 서버 SEMDMS Service (`BSL/KlarfUpload`) | 파일 = `KLARF_IMAGE_ROOT\yyyyMMdd\Lot`, DB `TQD_IMAGES.IMAGE_PATH` = FTP 상대 경로 |
| 화면 표시 | 클라이언트 (`Control/DefectMapDraw.fnFTPImageDownload`) | `ftp://IP:PORT` + `DEFECT_FTP.MAIN_PATH` + `IMAGE_PATH` + 파일 이름 |

FTP 상대 경로 규칙 (2026-10-21 추가):

- `KLARF_IMAGE_FTP_BASE` 없음 = 지금까지 규칙. 드라이브 루트를 뺀 경로 (`D:\FTP\SCK\20261021\Lot` -> `FTP/SCK/20261021/Lot`). 로컬 개발 환경 / 기존 데이터는 이 규칙 그대로.
- `KLARF_IMAGE_FTP_BASE` 있음 = 그 폴더 기준 경로. 값은 `MAIN_PATH` 가 가리키는 실제 폴더.
  - 예: FTP `/DMS_IMAGE` = `C:\FTP\DMS_IMAGE` -> `C:\FTP\DMS_IMAGE\20261021\Lot` -> `20261021/Lot` -> 화면 요청 `/DMS_IMAGE/20261021/Lot/파일`.
  - 저장 폴더가 기준 폴더 아래가 아니거나 기준이 상대 경로면, 파일을 복사하기 전에 업로드가 오류로 멈춘다.

## 2. 서버(10.86.253.162) 설정 순서

1. **`/DMS_IMAGE` 의 실제 폴더 확인**: IIS 관리자 > FTP 사이트 > 기본 설정의 실제 경로(예 `C:\FTP`) 아래 `DMS_IMAGE`, 또는 `DMS_IMAGE` 가 가상 디렉터리면 그 실제 경로. 아래 예는 `C:\FTP\DMS_IMAGE`.
2. **권한**: SEMDMS Service 실행 계정 = 그 폴더 쓰기 / FTP 계정 POC = 읽기.
3. **서버 `DACrux.SEMDMS.Service.exe.config`** 의 `<appSettings>` (이미 있는 `KLARF_IMAGE_ROOT` 는 값만 바꾼다):

   ```xml
   <add key="KLARF_IMAGE_ROOT" value="C:\FTP\DMS_IMAGE" />
   <add key="KLARF_IMAGE_FTP_BASE" value="C:\FTP\DMS_IMAGE" />
   ```

4. **서버 DLL**: 이 변경이 든 `DACrux.SEMDMS.BSL.dll` 을 서버에 넣는다. 지금 BSL 소스는 CLAUDE-171 변경(Interface `EtYmsRule` 새 함수 사용)도 함께 있어 `DACrux.SEMDMS.Interface.dll` 도 같은 빌드로 같이 넣어야 한다(클라이언트 Interface 와도 맞춘다).
5. **SEMDMS Service 재시작** (config / DLL 적용).
6. **DB(TRAINING)**: `DMS/Assistant/DB/sql/2026-10-21_defect_ftp_set.sql` 을 TRAINING 으로 실행, 암호 입력. DEFECT_FTP = 10.86.253.162 / 21 / POC / `/DMS_IMAGE`. 기록 `defect_ftp_set.log`. 새 DB 는 기본 데이터 스크립트(`ddl/2026-10-21_DMSMGR_base_data.sql`, 기본값 `/DMS_IMAGE`)로 같이 들어간다.
7. **망**: 클라이언트 PC -> 서버 21 + Passive 데이터 포트 허용. 이 PC 에서는 2026-10-21 목록 조회가 됐다.

## 3. 확인 방법

1. DMS > KLARF 업로드에서 이미지가 있는 KLARF 저장 -> 결과 표 IMAGE_PATH = `yyyyMMdd/Lot` (예 `20261021/PFIC_RH-HS2615_C4BUMP`).
2. 서버 `C:\FTP\DMS_IMAGE\yyyyMMdd\Lot` 에 KLARF / 이미지 / `thumb_` 파일.
3. Defect Map Viewer 에서 Defect 를 골라 이미지 카드가 그림으로 나오는지(실패면 "이미지를 불러오지 못했습니다").

## 4. 확인한 것 / 안 한 것

- 이 PC -> `ftp://10.86.253.162:21` POC 로그인, `/` = `DMS_IMAGE`(비어 있음) + `SetupFile` (목록 조회만, 2026-10-21).
- 경로 규칙: `BSL/Probes/KlarfImageFtpBaseProbe.ps1` -> `KLARF_IMAGE_FTP_BASE_PROBE=PASS (14/14)` (TEMP 빌드, DB / FTP 없음): 기준 없음 = 기존 규칙, 기준 = 날짜/Lot, 끝 `\` · 대소문자, FTP 루트 기준, 기준 밖 · 이름 앞부분만 같음 · 상대 경로 오류, 화면 경로 조합, 실제 KLARF + 이미지 15장 저장소 복사.
- 안 한 것: 서버 config / DLL / Service, TRAINING 실행, FTP 에 파일 올리기, 실제 업로드 -> 화면 표시.
