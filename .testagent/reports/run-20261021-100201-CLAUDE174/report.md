# 검증 리포트: KLARF 업로드 이미지 FTP 기준 폴더(KLARF_IMAGE_FTP_BASE) (CLAUDE-174)

실행: 2026-10-21 10:02 / 대상: TEMP 빌드 `%TEMP%\ChippacBuild\CLAUDE-174` (Interface + BSL 소스 빌드, 오류 0 / 경고 0, BSL md5 d707dcf1)
판정: **DONE_WITH_CONCERNS** - 경로 규칙 Probe 14/14 PASS. 서버 / TRAINING / 실제 업로드 화면은 미확인.

- 변경: `DMS/02_DMS/DACrux.SEMDMS.BSL/KlarfUpload.cs` (`ToFtpRelativePath` 신규, `SaveFilesToImageStore` 가 Service 설정 `KLARF_IMAGE_FTP_BASE` 사용) / 신규 `DMS/02_DMS/DACrux.SEMDMS.BSL/Probes/KlarfImageFtpBaseProbe.ps1`
- 명령: `powershell -NoProfile -ExecutionPolicy Bypass -File DMS\02_DMS\DACrux.SEMDMS.BSL\Probes\KlarfImageFtpBaseProbe.ps1 -BinDir %TEMP%\ChippacBuild\CLAUDE-174`
- 결과: `KLARF_IMAGE_FTP_BASE_PROBE=PASS (14/14)` - `evidence/01_KlarfImageFtpBaseProbe.stdout.txt`
- 첫 실행은 Probe 결함(HashSet 을 ICollection 으로 변환)으로 E1 에서 멈춤 -> Probe 수정 후 PASS.
- 빌드 주의: BSL 소스에 CLAUDE-171 진행분이 있어 운영 Interface DLL 로는 컴파일되지 않는다(EtYmsRule 새 함수). Interface 도 소스로 빌드했다.
- 미확인: 서버 설정 / DLL 배포, TRAINING `2026-10-21_defect_ftp_set.sql` 실행, FTP 에 실제 업로드 후 화면 표시.
