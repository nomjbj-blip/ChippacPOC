# 검증 리포트: Defect Image 카드 단순화 구현 (CLAUDE-159)

실행: 2026-10-20 11:2x ~ 11:31 / 대상: TEMP 빌드 `%TEMP%\ChippacBuild\CLAUDE-159\bin` (DACrux.SEMDMS.Control md5 ea956ec3 / ENGUI md5 7ad51194), 로컬 이미지 `D:\FTP\SCK\20261013\PFIC_RH-HS2615_C4BUMP` + 로컬 DB STEP_SEQ 528 Defect 15행(SELECT 만)
판정: **DONE_WITH_CONCERNS** — 카드 / 호출 2곳 빌드 확인 + 카드 확인 16/16 PASS. 연관 분석 Image 탭은 CLAUDE-160 진행 중이라 교체하지 않음. FTP 경로와 DMS 화면은 미확인.

## 1. 바꾼 것

| 파일 | 내용 |
| --- | --- |
| 신규 `DMS/02_DMS/DACrux.SEMDMS.Control/DPUCDefectImageCard.cs` + Control csproj Compile 1행 | 컨트롤 1개 카드(원본 비율 / 글자 2줄 / 선택 테두리 / 더블클릭 = 기존 ImagePopUp + 제목 줄), 축소본만 보관, 이미지 읽기 대기열 1개 |
| `DMS/02_DMS/DACrux.SEMDMS.Control/DPUCDefecMapViewer.cs` (cp949 유지) | 이미지 목록 카드 교체, `Thread.Sleep(10)` 삭제, 폭만 지정(`ImageCardWidth`), 크기 변경 때 높이 누적 제거, Excel 내보내기 형식 비교, 새 Map 읽을 때 카드 해제 |
| `DMS/02_DMS/DACrux.SEMDMS.ENGUI/GdsMap/frmGdsDieAnalysis.cs` | 상세보기 이미지 창 카드 교체(Defect ID 만) |

## 2. 실행과 결과

| 단계 | 명령 / 대상 | 결과 |
| --- | --- | --- |
| 빌드 | scratchpad `build159.ps1`(Verify-GdsKlarfAnalysis.ps1 와 같은 순서, 전용 폴더) | Control / ENGUI 오류 0. 경고 Control 12 / ENGUI 4 = 모두 기존 파일(새 카드 0) |
| 카드 확인 | scratchpad `CardCheck.exe`(TEMP Control DLL 로딩 확인 C0) | `IMAGE_CARD_CHECK=PASS (16/16)` — `evidence/01_CardCheck.stdout.txt` |
| 그림 | 같은 프로그램 | `evidence/05_구현_DefectMapViewer_목록.png`, `06_구현_상세보기창_실패상태.png` |

주요 항목: 15장 대기열 읽기 완료 / 글자 2줄 = DB 값 15장 / Defect 12 제목 줄 `Defect 12 / Die (6, 6) / Class 53 / Bin 53 / Size 3.23 x 3.23 µm / REL (5521.58, 1284.69) µm / Cluster 0` / 자식 컨트롤 0 / 높이 = 폭 x 비율 + 42 / 클릭 단일 선택 / 더블클릭 ImagePopUp 열기·다시 열기 / 실패 상태 / Defect ID 만 카드 / 읽는 중 30장 해제 오류 없음(USER +4).

측정(카드 30장, 실제 카드): USER +34 / GDI +6 / Private +24 MB / 목록 다시 그리기 5회 182 ms. 같은 조건 기존 카드(설계 2절): USER +741 / GDI +207 / +272 MB / 3.6 초.

## 3. 검증하지 않은 것

- FTP 다운로드 경로(`DefectMapDraw.fnFTPImageDownload`, 기존 함수 그대로): 시험 프로세스에 원격 설정이 없어 실패 상태만 확인. 실제 FTP 는 DMS 화면 확인 대기.
- DMS 화면(Defect Map Viewer 이미지 목록 / 상세보기 이미지 창 / Excel 내보내기 그림).
- 연관 분석 화면 A Image 탭(`frmGdsKlarfAnalysis.ShowImages`): CLAUDE-160 이 같은 파일 진행 중이라 교체 보류.

## 4. 2026-10-21 연관 분석 Image 탭 교체

- CLAUDE-160 완료(10-20 11:35) 뒤 `frmGdsKlarfAnalysis.ShowImages` 를 `DPUCDefectImageCard(d, path)` 로 교체. 이것으로 설계 D1 의 3곳 모두 교체.
- `Verify-GdsKlarfAnalysis.ps1`(TEMP 를 `%TEMP%\ChippacBuild\CLAUDE-159-smoke` 로 바꿔 실행, 조회만): 빌드 경고 Control 12 / ENGUI 4(기존 파일만) -> `GDS_KLARF_FORM_SMOKE=PASS (75/75)`. 증거 `evidence/02_VerifyGdsKlarfAnalysis_20261021.stdout.txt`. 이 Smoke 는 Image 탭을 검사하지 않는다.
- 운영 `100_ClientBin` Control / ENGUI 는 10-20 16:36 VS 빌드로 바뀌었고 두 DLL 안에 `DPUCDefectImageCard` 가 있다(Viewer / 상세보기 반영으로 보임). Image 탭 교체는 다음 ENGUI 빌드부터.
