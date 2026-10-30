# CLAUDE-218 POC 데이터 생성 / 수정 사용 기한(2027-01-31) 테스트 계획

- 대상 표면: .NET(DSL / BSL / RO / DACruxV5), DB(DB 일시 조회만)
- 범위 단계: Standard
- 근거 문서: `문서/설계/06_환경_운영/2026-10-14_POC_License_설계.md` 4 · 7 · 8 · 16절, `DMS/Assistant/공통_개발_지침.md` "데이터 생성 / 수정 작업 기간"
- 변경 범위: 커밋 `440af08`(10-27) — `DACrux.SEMDMS.DSL/WorkRule.cs`, SEMDMS.BSL 7파일 + TEST.BSL `ProbeAdmin.cs` 첫 줄 31곳, `iGdsMapView` / RO / BSL `GdsMapView.GetWorkState`, `DACruxV5/MenuWorkRule.cs` · `DACruxClientMain.cs`. 이후 대상 파일 변경 없음(git log / status)
- 기준 데이터: 없음(저장하지 않음). 로컬 DB `oracle19c-DMS` 는 DB 일시 / 조회만
- DB 쓰기: 없음. 기한 지남 시험은 저장소 소스를 바꾸지 않은 TEMP 복사본 DSL(`LastWorkDay` 과거) — 함수가 첫 줄에서 멈춰 DB 에 쓰지 않음
- 기존 검증 자산: `DMS/02_DMS/DACrux.SEMDMS.BSL/Probes/WorkRuleProbe.ps1` (SCN-01 일부 / 03 / 04 / 05)
- 작성/갱신: 2026-10-29

## 시나리오

### SCN-01 [정상] 소스에 기한 확인이 빠짐없이 있다
- 사전조건: 현재 작업 트리
- 단계:
  1. `WorkRule.cs` 상수 / csproj 등록 / 메뉴 묶음 키 확인
     - expect: `LastWorkDay = new DateTime(2027, 1, 31)`, DSL csproj `WorkRule.cs` 1행, DACruxV5 csproj `MenuWorkRule.cs` 1행, `DACruxClientMain.cs` `MenuWorkRule.Attach` 2곳, MenuFile.mnu(소스 / ClientBin) 에 `MNU_POC_SETUP` / `MNU_POC_UPLOAD`
  2. Probe 소스 검사
     - expect: 대상 31 / 31 첫 줄 Confirm, `DeleteEtFile` / `GdsMapView` 에 없음
- 실행 수단: grep + `WorkRuleProbe.ps1` 소스 항목
- 실패 의미: 기한 확인이 빠진 저장 경로가 있다
- 데이터 정리: 쓰기 없음

### SCN-02 [부정] 확인 줄이 없는 새 저장 함수가 생기지 않았다
- 사전조건: 현재 작업 트리
- 단계:
  1. POC 서비스 BSL(`GdsMapImport` / `GdsChain` / `GdsEtResult*` / `KlarfUpload`) 의 public 함수 중 DB 를 바꾸는 함수 전체를 목록화하고 첫 줄 Confirm 여부를 본다
     - expect: Confirm 없는 쓰기 함수 = 설계상 제외 목록(Cancel* / 배치 중간 Create*Batch / AddImage / DeleteEtFile) 만
- 실행 수단: 소스 스캔(PowerShell)
- 실패 의미: 10-23 이후 추가된 저장 함수가 기한 확인 없이 열려 있다
- 데이터 정리: 쓰기 없음

### SCN-03 [정상] 기한 안에서는 저장 흐름이 그대로다
- 사전조건: 로컬 DB healthy
- 단계:
  1. `WorkRuleProbe.ps1 -BslDir <운영 ServerBin 또는 TEMP 빌드>`
     - expect: `WORK_OPEN=Y`, `LAST_DAY=2027-01-31`, DB 한국 시간과 PC 시각 차이 5분 미만, `Confirm` 예외 없음, `WORK_RULE_PROBE=PASS`
- 실행 수단: Probe `WORK_RULE_PROBE`
- 실패 의미: 기한 안인데 저장이 막히거나, DB 시각을 잘못 읽는다
- 데이터 정리: 쓰기 없음

### SCN-04 [경계] 마지막 날 23:59:59 통과 / 다음 날 0시 차단
- 단계:
  1. Probe 판정 항목
     - expect: 2026-09-30 23:59:59 차단 / 2026-10-01 00:00:00 통과 / 2027-01-31 23:59:59 통과 / 2027-02-01 00:00:00 차단, 남은 일수 마지막 날 1 · 다음 날 0 · 13일 전 14
- 실행 수단: Probe `판정: 기간 경계` / `판정: 남은 일수`
- 실패 의미: 하루 일찍 / 늦게 막힌다
- 데이터 정리: 쓰기 없음

### SCN-05 [부정] 기한이 지나면 31개 저장 함수가 모두 첫 단계에서 멈추고 조회는 된다
- 사전조건: 현재 소스를 TEMP 로 복사해 `LastWorkDay = 2026-10-28` 로 빌드한 DSL + 현재 BSL
- 단계:
  1. `WorkRuleProbe.ps1 -BslDir <expired> -ExpectClosed`
     - expect: `WORK_OPEN=N DAYS_LEFT=0`, 31 / 31 "POC 사용 기간" 문장, `GetMapDeviceList` 정상, `WORK_RULE_PROBE=PASS`
  2. red 확인: 정상 빌드에 `-ExpectClosed` 를 주면 FAIL 이 나야 한다
- 실행 수단: Probe
- 실패 의미: 기한 후에도 저장되는 경로가 있다 / Probe 가 실패를 잡지 못한다
- 데이터 정리: 쓰기 없음(첫 줄에서 멈춤), TEMP 빌드 배포 안 함

### SCN-06 [예외] DB 일시를 읽지 못하면 저장을 막는다
- 사전조건: Probe 설정 사본의 `DMS_CONNECT_ID` 를 없는 접속 ID 로 바꾼 별도 프로세스
- 단계:
  1. `WorkRule.Confirm`
     - expect: "서버 일시를 확인할 수 없어 저장할 수 없습니다." 로 시작하는 문장, 서버 PC 시간으로 통과하지 않음
- 실행 수단: 임시 PowerShell(설정 사본만 변경)
- 실패 의미: DB 를 못 읽을 때 PC 시간으로 판정하거나 예외가 엉뚱하게 난다
- 데이터 정리: 쓰기 없음

### SCN-07 [정상] 운영 폴더에 배포된 상태
- 단계:
  1. `101_ServerBin` / `100_ClientBin` 대상 파일 MD5 와 리플렉션
     - expect: 배포했다면 ServerBin DSL 에 `WorkRule`, BSL `GdsMapView.GetWorkState`, ClientBin `DACruxV5.MenuWorkRule` / Interface `GetWorkState`. 배포 전이면 "미배포"로 기록
- 실행 수단: MD5 / 리플렉션
- 실패 의미: 소스는 있으나 실행 폴더에는 적용되지 않았다
- 데이터 정리: 쓰기 없음

## 해당 없음
- [입력검증] 해당 없음 — 기한 확인은 화면 입력을 받지 않는다(모듈 / 종류 enum 만)
- [권한/상태] 해당 없음 — 사용자 권한과 무관하게 서버 DB 일시로만 판정
- 화면 확인(메뉴 비활성 / 제목 문장 / 오류 창 문장) — Claude 가 DMS 를 띄우지 않으므로 `화면 확인 대기`
