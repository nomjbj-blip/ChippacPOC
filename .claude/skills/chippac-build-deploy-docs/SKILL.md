---
name: chippac-build-deploy-docs
description: 스테츠칩팩 ChippacPOC(DACrux DMS / GdsMapControl) 프로젝트에서 코드를 고친 뒤 빌드 -> Probe / Verify 검증 -> 100_ClientBin / 101_ServerBin 배포 -> DMS/Assistant 공유 문서(작업_현황.md / 기능 문서 / 변경이력) 갱신, 그리고 하루 마감 인계 정리까지 처리하는 절차. ChippacPOC에서 "빌드해줘", "배포해줘", "ClientBin에 복사", "ServerBin 반영", "DMS에서 보이게 해줘", "Probe 돌려줘", "작업 현황 / 변경 이력 갱신", "오늘은 여기까지", "인계 정리", "MSBuild 오류(CS0246 / EPERM / 파일 잠김)"가 나오거나 BSL / DSL / RO / Interface / ENGUI / GdsMap 소스를 수정해 결과를 확인하고 기록해야 하는 모든 경우에 사용한다. 사용자가 빌드나 문서를 직접 언급하지 않아도 ChippacPOC 코드 변경 작업을 마무리할 때는 이 스킬을 따른다.
---

# ChippacPOC 빌드 / 배포 / 공유 문서 갱신

프로젝트 루트: `D:\1_프로젝트 문서\18.스테츠칩팩_POC\ChippacPOC` (아래 경로는 이 기준).
이 프로젝트는 Claude(메인)와 Codex(보조), 그리고 여러 Claude 세션이 같은 PC에서 동시에 개발하고 `DMS/Assistant` 문서로 인계한다.
그래서 빌드 결과보다 **무엇을 어디까지 확인했는지 정확히 남기는 것**이 더 중요하다. 다음 작업자는 대화가 아니라 문서만 보고 이어간다.

## 0. 시작 전 확인

빌드 전에 아래를 확인한다. 실제 시험에서 사용자가 "추가했다"고 한 코드가 디스크에 없거나, 배포할 DLL에 다른 세션의 미완성 코드가 섞여 있던 경우가 있었다. 이런 상태로 빌드하고 배포하면 시간만 쓰고 운영 화면이 깨진다.

1. **사용자가 말한 변경이 저장되어 있는가.** 말한 메서드 / 컨트롤 / 쿼리 ID를 grep으로 찾는다. 없으면 Visual Studio(devenv)가 실행 중인지 보고, 저장하지 않은 것 같다고 사용자에게 알린 뒤 멈춘다.
2. **누가 이 파일을 다루고 있는가.** `DMS/Assistant/작업_현황.md`의 "진행 중인 작업" 표를 읽는다.
   - 다른 도구나 세션이 `진행 중`인 파일이면 수정하지 않는다.
   - `중단`인 작업의 파일이면, 그 행을 이어받을지 사용자에게 묻는다. 중단된 작업의 반쯤 고친 코드가 내 빌드에 같이 들어가기 때문이다.
   - 기록된 시각 이후에 파일 수정 시각이 바뀌었다면(`Get-Item <파일>).LastWriteTime`) 문서에 없는 작업이 있었다는 뜻이다. 사용자에게 알린다.
   - 최근 몇 분 안에 소스나 `obj` 파일이 바뀌고 devenv / MSBuild가 실행 중이면 다른 세션이 작업 중일 가능성이 높다. 같은 파일과 문서를 건드리기 전에 사용자에게 확인한다.
3. **git 상태.** `git status`로 확인한다. GDS 관련 파일은 상당수가 untracked(`??`)라서 `git diff`로는 변경이 보이지 않는다. 이런 파일은 수정 시각과 grep으로 비교한다. 미커밋 파일도 누군가의 작업물이므로 되돌리지 않는다.
4. **내 작업 행 추가.** 형식: `| CLAUDE-0nn | Claude | 기능 | 수정 파일 범위 | 진행 중 | YYYY-MM-DD HH:mm |`. 번호는 표에서 가장 큰 번호 + 1로 한다. 표에는 이미 중복 ID(CLAUDE-005 두 개, 016 / 016B)가 있으므로 새 번호가 표에 없는지 grep으로 확인한다.

## 1. 빌드

**`02_DMS`와 `09_TEST`의 모든 csproj는 Debug 출력 경로가 운영 폴더다.** Interface / RO / ENGUI는 `100_ClientBin`, DSL / BSL은 `101_ServerBin`으로 나간다. Visual Studio에서 그냥 빌드해도 사용자가 쓰는 DLL이 확인 없이 바뀐다. 그래서 Claude는 항상 TEMP OutDir로 빌드하고, 배포는 3절 스크립트로만 한다.

| 고친 곳 | 빌드 방식 | 배포 대상 |
| --- | --- | --- |
| `DMS/06_CoreLibSource/GdsMapControl` | 기본 빌드(`bin/Debug`) | `100_ClientBin/GdsMap` (EXE + pdb, **config는 제외**) |
| `02_DMS/DACrux.SEMDMS.Interface` | TEMP OutDir | ServerBin **과** ClientBin 둘 다 (csproj 출력은 ClientBin뿐이라 ServerBin을 빠뜨리기 쉽다) |
| `02_DMS/DACrux.SEMDMS.DSL` / `BSL` | TEMP OutDir + `ReferencePath=<101_ServerBin>` | `101_ServerBin` |
| `02_DMS/DACrux.SEMDMS.RO` | TEMP OutDir | `100_ClientBin` |
| `02_DMS/DACrux.SEMDMS.ENGUI` / `09_TEST/DACrux.TEST.ENGUI` | TEMP OutDir + `BuildProjectReferences=false` + 참조 DLL 미리 복사 | `100_ClientBin` |

명령과 오류별 해결은 [references/build.md](references/build.md)를 본다. 자주 겪는 문제:
- `CS0246`(Vector2 / GLControl / Miracom / DACrux 형식): 참조가 풀리지 않은 것이다. 참조 DLL을 OutDir에 복사하거나 `ReferencePath`를 지정한다. DSL을 고치지 않고 BSL만 빌드할 때도 `101_ServerBin`의 DSL DLL이 OutDir에 있어야 한다.
- 새 `.cs` 파일이 빌드되지 않음: non-SDK csproj라서 `<Compile Include>`를 직접 추가해야 한다.
- 파일 잠김 / `EPERM`: DACrux / 서비스 / GDS 뷰어 / 관리자 권한 Visual Studio가 파일을 잡고 있다. 강제로 처리하지 말고 사용자에게 종료를 요청한다.
- C# 7.3 문법만 쓴다(.NET Framework 4.0 / 4.8 혼재).

Interface를 바꾸면 그것을 참조하는 RO / BSL / GdsMap도 다시 빌드한다. 그런데 GdsMap이 다른 작업 때문에 지금 빌드되지 않는 상태라면, GdsMap은 배포 묶음에서 빼고 그 사실을 사용자에게 알리고 이력에 남긴다. 고장 난 다른 작업을 내 판단으로 고치지 않는다.

빌드 후 경고 수를 본다. 기존 경고 `CS0649 (GlDefectItem.LineVertexCount)` 1건은 정상이다. 새 경고가 생기면 보고한다.

## 2. 검증

- GdsMap / DB 저장 / 조회 로직: `DMS/06_CoreLibSource/GdsMapControl/Probes/*.ps1`. 결과는 `XXX_PROBE=PASS` 형식이다. 배포 전에 시험할 때는 Probe가 TEMP 빌드를 보도록 인자(`-BslDir`, `-BinDir` 등)를 넘긴다. Probe마다 인자 이름이 다르므로 파일 첫 주석을 읽는다.
- **Probe가 새 기능을 실제로 호출하는지 확인한다.** 기존 Probe는 기존 메서드만 부른다. 새 메서드를 추가했다면 Probe에 검사 항목을 추가하거나, 추가하지 않았다면 "새 메서드는 Probe 미확인"으로 기록한다. DB 구조가 바뀐 뒤 아직 고쳐지지 않은 Probe도 있으니(예: 리비전 키를 쓰는 예전 Probe) 실패하면 Probe가 최신인지부터 본다.
- F05 / F06 화면: `Verification/Verify-*.ps1` (`DMS/Assistant/빌드_검증_가이드.md` 표 참고).
- 실행: `powershell -NoProfile -ExecutionPolicy Bypass -File <ps1>`. Windows PowerShell 5.1을 쓰고, 한글이 들어간 ps1은 UTF-8 BOM으로 저장한다.

**확인 수준을 섞지 않는다.** 빌드가 성공하면 `빌드 확인`, Probe를 통과하면 `Probe 확인`이다. 사용자가 DMS 화면에서 확인해야 `화면 확인`, 저장한 뒤 재조회까지 해야 `DB 확인`이다. Claude가 DMS 화면을 직접 띄워 보지 않았다면 "화면 확인 대기"로 쓴다. 문서에 과장된 상태가 남으면 다음 작업자가 확인되지 않은 기능을 믿고 그 위에 작업을 쌓게 된다.

## 3. 배포

배포는 사용자가 실제로 쓰는 실행 폴더를 바꾸는 일이므로 **복사 전에 항상 사용자 승인을 받는다.** 앞에서 받은 승인은 그 배포에만 해당한다.

[scripts/Deploy-ChippacBin.ps1](scripts/Deploy-ChippacBin.ps1)이 하는 일:
- DACrux / 서비스 / GDS 뷰어가 실행 중이면 복사하지 않고 종료 코드 2로 끝낸다.
- devenv / MSBuild가 실행 중이면 경고만 표시한다.
- 배포 전 / 후 MD5(앞 8자리)를 표로 보여 준다.
- `%TEMP%\claude\deploy_backup\<시각>_<Tag>\`에 백업한 뒤 복사하고, 복사본의 MD5를 확인한다.
- 같은 이름의 `.pdb`는 함께 복사한다. `.config`는 거부한다. 운영 config는 빌드 산출물과 내용이 다르기 때문이다.

```powershell
$deploy = 'D:\1_프로젝트 문서\18.스테츠칩팩_POC\ChippacPOC\.claude\skills\chippac-build-deploy-docs\scripts\Deploy-ChippacBin.ps1'
$b = "$env:TEMP\ChippacBuild\CLAUDE-0nn"
# 1) 계획만 확인 (복사 안 함). 결과 표를 사용자에게 보여 주고 승인을 받는다
& $deploy -ProjectRoot 'D:\1_프로젝트 문서\18.스테츠칩팩_POC\ChippacPOC' -Tag CLAUDE-0nn -DryRun `
  -ServerBin "$b\DACrux.SEMDMS.Interface.dll","$b\DACrux.SEMDMS.BSL.dll" `
  -ClientBin "$b\DACrux.SEMDMS.Interface.dll","$b\DACrux.SEMDMS.RO.dll"
# 2) 승인을 받으면 -DryRun을 빼고 같은 명령을 실행
```
`powershell -File`로 부를 때는 `-ServerBin "a.dll,b.dll"`처럼 쉼표로 이어 쓴 문자열 하나로 넘겨도 된다.

승인을 요청할 때 함께 알릴 것:
- **배포 전 MD5가 마지막 기록값과 다를 때**: 그 사이 누군가 배포했거나 VS에서 빌드한 것이다.
- **내 배포 묶음에 다른 작업의 코드가 섞일 때**: 같은 DLL을 다른 세션도 수정한 경우다. 어느 작업 ID의 코드인지 적는다.
- **클라이언트와 서버가 어긋날 때**: ServerBin과 ClientBin의 Interface MD5가 다르거나, RO만 새로 배포되고 BSL은 예전 버전이면 화면에서 원격 호출이 실패할 수 있다. 화면이 동작할 거라고 말하기 전에 양쪽을 맞춘다.
- **배포 후 다시 시작해야 하는 것**:
  - 서버 DLL을 배포했다면 SEMDMS Service를 다시 시작해야 적용된다.
  - GdsMap EXE를 배포했다면 DACrux를 다시 시작해야 한다. DACrux가 EXE를 `Assembly.LoadFrom`으로 한 번 읽으면, 같은 프로세스에서는 교체되지 않는다.

`101_ServerBin`에 새 서비스 클래스를 추가했다면 `DACrux.SEMDMS.Service.exe.config`의 wellknown 등록도 필요하다. 실제 실행 설정은 이 파일이며 `App.config`가 아니다. 이 파일은 손으로 한 줄만 추가하고, 추가한 뒤 XML이 올바르게 파싱되는지 확인한다.

## 4. 공유 문서 갱신

결정, 구현 단위 완료, 검증, 오류나 중단이 생기면 그때마다 바로 갱신한다. 대화가 끊겨도 문서가 현재 상태를 말해 줘야 하기 때문이다. 모든 파일은 **편집 직전에 다시 읽고** 다른 작업자의 항목은 보존한다.

| 문서 | 갱신 내용 |
| --- | --- |
| `DMS/Assistant/작업_현황.md` | 내 행의 상태 칸(완료 범위 / 배포 시각 / 확인 수준 / 남은 일), 상단 `최종 갱신` 시각 |
| `DMS/Assistant/기능/F0n_*.md` | 현재 동작 / 중요한 로직의 이유 / 미완료 항목. 바뀐 이유는 이력에 남긴다 |
| `DMS/Assistant/변경이력/YYYY-MM-DD.md` | 오늘 날짜 파일 끝에 추가한다. 없으면 `# YYYY-MM-DD 작업 이력` 헤더로 새로 만든다. 과거 항목은 고치지 않는다 |

이력 형식과 실제 예시는 [references/doc-templates.md](references/doc-templates.md)에 있다. 꼭 남길 것:
- 실제 실행한 명령, 대상 DLL / EXE, 결과(PASS 개수 / 건수 / 시간)
- 배포한 파일별 MD5 `이전 -> 이후`, 백업 위치, 프로세스 미실행 확인 여부. 스크립트가 출력하는 "change log lines"를 한국어로 옮기면 된다
- **미실행 / 미확인 항목**(예: "Service Remoting 경유 미확인", "DMS 화면 확인 대기")
- **문서에 없던 변경을 발견한 경우**: 운영 폴더 MD5가 기록과 다름, 다른 세션이 `obj/Debug`에 바로 빌드함 등. 사실만 적고 되돌리지 않는다
- 다음 작업 한 단계

비밀번호, 접속 문자열, 고객 원시 데이터는 문서에 붙여 넣지 않는다.

## 5. 하루 마감 / 인계 ("오늘은 여기까지", "인계 정리")

다음 작업자(Codex 또는 새 Claude 세션)가 문서만 보고 첫 단계를 시작할 수 있게 한다. 아래 순서로 확인하고 기록한다.

1. 인계 대상 작업 ID마다 문서 기록 시각 이후 바뀐 파일을 0절 방법으로 찾는다. 문서와 실제 파일이 다르면 실제 파일을 기준으로 적는다.
2. 운영 폴더 상태를 확인한다. 관련 DLL / EXE의 MD5와 시각, ServerBin / ClientBin Interface가 같은지, 빌드만 하고 배포하지 않은 산출물이 있는지 본다.
3. 실행 중인 프로세스(DACrux / Service / devenv / MSBuild)를 확인한다.
4. 이력에 작업 ID별로 인계 절을 추가한다. 내용은 현재 상태, 확인 수준, 남은 위험(빌드 불가 / 클라이언트와 서버 불일치 / 미배포), **내일 첫 단계 한 가지**다.
5. 작업 현황의 각 행 상태 칸과 상단 `최종 갱신` 시각을 고친다.

**다른 세션의 행을 인계할 때**: 그 행의 상태를 `완료`나 `중단`으로 바꾸지 않는다. 그 세션이 아직 작업 중일 수 있다. 상태 칸 끝에 `/ 인계 메모: 변경이력 참고(Claude HH:mm)`만 붙이고, 자세한 내용은 이력의 인계 절에 쓴다. 사용자가 그 세션이 끝났다고 확인해 주면 그때 상태를 고친다.

## 6. 사용자 보고

사용자는 C# 개발자이고 한국어 설명을 원한다. 다음 순서로 짧게 보고한다.
**목적 / 수정한 파일 / 코드 설명 / 실행 명령어 / 정상 실행 확인 방법 / 다음 단계**

- 0절에서 멈췄다면(변경이 없음 / 다른 작업과 겹침), 발견한 사실과 사용자에게 받아야 할 결정을 먼저 쓴다. 결정은 번호를 붙인 질문으로 묻는다.
- 정상 실행 확인 방법에는 사용자가 DMS에서 직접 해 볼 동작을 적는다. 다시 시작할 프로그램, 메뉴 이름, 클릭 순서, 기대 결과가 들어간다.
- 마지막에 갱신한 문서를 한 줄로 알린다.
