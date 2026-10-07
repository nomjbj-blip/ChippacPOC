# 공유 문서 기록 양식과 예시

원본 양식: `DMS/Assistant/양식/작업_기록_양식.md`, `양식/기능_문서_양식.md`. 원본 양식이 바뀌었으면 원본을 따른다.

## 변경 이력 항목 (`변경이력/YYYY-MM-DD.md` 끝에 추가)

```markdown
## CLAUDE-0nn / F0n / 짧은 제목

- 시각: YYYY-MM-DD HH:mm KST
- 담당: Claude
- 상태: 진행 중 / 부분 완료 / 검증 대기 / 완료 / 중단
- 목적과 사용자 요청:
- 수정 파일 / 주요 함수:
- 변경한 동작과 이유:
- 결정 사항 / 근거 문서:
- 검증 명령 / EXE / 샘플:
- 실제 결과 / 로그:
- 미실행 / 미확인 항목:
- 남은 오류 / 중단 이유:
- 다음 작업 한 단계:
- 갱신한 기능 문서 / 작업 현황:
```

해당 사항이 없는 칸은 빼도 된다. 다만 **미실행 / 미확인**과 **다음 작업 한 단계**는 항상 쓴다. 다음 작업자가 가장 먼저 찾는 정보이기 때문이다.

## 좋은 예시 (2026-10-02 실제 기록을 줄인 것)

```markdown
## CLAUDE-014 / F03 / 조회 서비스 iGdsMapView

- 신규: `02_DMS/DACrux.SEMDMS.Interface/iGdsMapView.cs`, `DACrux.SEMDMS.RO/GdsMapView.cs`, `DACrux.SEMDMS.BSL/GdsMapView.cs`, `Probes/GdsMapViewProbe.ps1`.
- 수정: csproj 3개 Compile 1줄씩, XML 3개에 SELECT 추가(기존 쿼리 변경 없음, BOM / 줄바꿈 유지), `101_ServerBin/DACrux.SEMDMS.Service.exe.config`에 `GdsMapView.bin` wellknown 1줄.
- 빌드: TEMP OutDir(`%TEMP%\GdsMapViewBuild`) Interface / DSL / BSL / RO 성공. DSL / BSL은 `-p:ReferencePath=101_ServerBin` 필요.
- 검증: `GdsMapViewProbe.ps1 -BslDir %TEMP%\GdsMapViewBuild` PASS. 605,080행 66페이지 15.5~17.3초, 중복 / 누락 없음.
- 배포(사용자 승인 15:40, DACrux 미실행 확인, 백업 scratchpad `backup_CLAUDE-014`): ServerBin Interface `5de775bf` -> `55773844` / BSL `9b00d262` -> `81ec03e6`, ClientBin Interface `5de775bf` -> `55773844` / RO `41027fbb` -> `de286a47`. 배포 전 MD5가 15:02 기록과 달라 그 사이 다른 배포가 있었던 것으로 보임.
- 미확인: Service Remoting 경유 호출.
- 다음 작업 한 단계: 행 해석기 + `GdsMapControl.ShowPlacedElements`.
```

(이 예시는 스크립트가 생기기 전의 기록이라 백업 위치가 scratchpad로 되어 있다. 지금은 스크립트가 출력하는 `%TEMP%\claude\deploy_backup\<시각>_<Tag>`를 적는다.)

좋은 점:
- 무엇을 실행했고 무엇을 실행하지 않았는지가 분리되어 있다.
- 숫자(행 수 / 시간 / MD5)가 있어 다음에 비교할 수 있다.
- 예상과 다른 사실(MD5 불일치)을 숨기지 않았다.

## 중단 기록 예시

```markdown
- 배포 실패(중단): 14:59 DACruxV5.exe / DACrux.SEMDMS.Service 실행 중이라 DLL 잠김. 복사된 파일 없음.
  현재 실행 중인 Service는 이전 BSL이라 새 스키마에서 저장 호출이 실패한다.
- 재개할 첫 단계: 사용자가 DACrux와 서비스 종료 -> DLL 배포 -> Probe 5개 재실행.
```

## 작업 현황 표 행

```markdown
| CLAUDE-0nn | Claude | F03 짧은 설명 | 수정 파일 범위(신규는 "신규" 표시) | 완료 / 16:15 배포 / Probe PASS / DMS 화면 확인 대기 | 2026-10-02 16:20 |
```
- 상태 칸에는 확인 수준이 드러나게 쓴다. "완료"만 쓰지 말고 어디까지 확인했는지 붙인다.
- 다른 작업자의 행은 임의로 종료하거나 지우지 않는다.

## 기능 문서 (`기능/F0n_*.md`)

- 현재 상태 표 / 중요한 로직 / 미완료 항목을 **현재 기준으로 고쳐 쓴다**. 날짜별 경과는 변경 이력에 남기고, 기능 문서에는 지금의 사실만 둔다.
- 설계만 있는 내용과 구현된 내용을 구분해 쓴다(`설계` / `소스 확인` / `빌드 확인` / `Probe 확인` / `화면 확인` / `DB 확인`).
- 새 클래스, 서비스 계약, DB 키처럼 다음 작업자가 소스를 찾는 데 필요한 경로와 이름을 남긴다.
- 바뀐 내용이 없으면 날짜만 고치는 갱신은 하지 않는다.
