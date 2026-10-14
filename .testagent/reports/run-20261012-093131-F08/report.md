# 검증 리포트 — F08 KLARF 업로드 / DB 적재

| 항목 | 내용 |
| --- | --- |
| 판정 | **BLOCKED** — 새 저장·재등록·삭제 경로는 사용자 업로드 데이터 보호로 이번에 실행하지 못함 (제품 결함은 발견 없음) |
| 실행 시각 | 2026-10-12 09:31 ~ 09:35 (KST) |
| 대상 | ChippacPOC / F08 / 운영 배포본 `DMS/101_ServerBin` BSL·Interface (DLL 2026-10-08 12:22, 읽기만) + ENGUI·RO 등 %TEMP% 빌드 (`ChippacKlarfUploadVerification`) |
| 계획 | [plans/F08.plan.md](../../plans/F08.plan.md) — Standard (시나리오 10) |
| 집계 | PASS 2 / PASS_PARTIAL 2 / FAIL 0 / SKIPPED 3 / NEEDS_CONTEXT 1 / 미실행 1 / 화면 확인 대기 1 |
| 최고 확인 수준 | DB 확인 (2026-10-08 사용자 업로드 데이터 STEP_SEQ 501 재조회) / 화면 확인 대기 |

## 1. 결과 요약

- 로컬 DB에 사용자 ADMIN이 같은 샘플(XD106.02)을 이미 올려 둔 상태(STEP_SEQ 501)였다. 기존 `KlarfUploadProbe.ps1`은 재등록 과정에서 이 데이터를 지우게 되므로 입력 검증 3건만 실행하고 스스로 멈췄다(SKIPPED, 정상 동작).
- 그 대신 DB를 쓰지 않는 읽기 전용 Probe(`.testagent/probes/KlarfUploadReadOnlyProbe.ps1`)를 새로 만들었다. 기존 Defect 조회 BSL로 STEP 501을 다시 읽어 **11개 검사가 모두 PASS**했다: Defect 15, Defect 1 좌표, CLASSNUMBER 54(D3), 이미지 경로가 상대 경로(D4), SETUP_ID 46자(D2), 이미지 행 15.
  - 이 Probe가 실제로 실패할 수 있는지도 확인했다. 기대값을 일부러 틀리게 넣으면 FAIL이 났다(red-green).
- 화면 Smoke는 TEMP 빌드에서 읽기·표시 검사 18건이 PASS했다. 저장 검사(5~7)는 같은 이유로 건너뛰었으므로 PASS_PARTIAL이다. 스크린샷을 직접 열어 확인했다(2절 SCN-09).
- **이번에 새로 저장해 보지 못한 것**: 현재 배포 DLL로 저장, 재등록(Y/N), 저장소 설정이 없을 때의 오류, 업로드 STEP 삭제, 기존 쿼리 회귀.
- 운영 폴더 `100_ClientBin`과 `101_ServerBin`은 실행 전후 최종 수정 시각이 2026-10-08 17:11:56으로 같다. 변경 없음.

## 2. 시나리오별 결과

| ID | 분류 | 결과 | 확인 수준 | 증거 |
| --- | --- | --- | --- | --- |
| SCN-01 | 정상 | PASS (기존 업로드 데이터 기준) | DB 확인 | `evidence/03_KlarfUploadReadOnlyProbe.stdout.txt` — 왼쪽 Wafer 조회 표시, MAP 1 / DIE 26, Defect 15, X -23229.419 / Y 86770.97, CLASS 54, Viewer 예외 없음 |
| SCN-02 | 입력검증 | PASS_PARTIAL (3/4) | Probe 확인 | `evidence/01_KlarfUploadProbe.stdout.txt` — 항목 수 부족 / 경로 섞인 파일명 / 잘못된 업로드 키 거부. '이미지 이름에 경로'는 SKIP 지점 뒤라 미실행 |
| SCN-03 | 예외 | SKIPPED | — | 01 Probe가 사용자 데이터 보호로 중단 |
| SCN-04 | 권한/상태 | SKIPPED | — | 위와 같음 (재등록이 STEP 501을 지우는 시나리오) |
| SCN-05 | 경계 | PASS | DB 확인 | `03` — `TQD_SETUP.SETUP_ID 46자 그대로 (D2)` 46자 `ROOT-CAI-OMM_Hybrid-OMM_Hybrid_C4Bump_Silterra` |
| SCN-06 | 경계 | NEEDS_CONTEXT | — | 129자 이상 SetupID를 거부할지 자를지 결정 없음 |
| SCN-07 | 경계 | 미실행 | — | Defect 0건 합성 KLARF 없음, 신규 Probe 필요 |
| SCN-08 | 부정 | SKIPPED | — | 01 Probe 중단 (기존 쿼리 회귀·삭제 검사 미도달) |
| SCN-09 | 정상(화면 Smoke) | PASS_PARTIAL (읽기 18 PASS, 저장 건너뜀) | Probe 확인 (화면 밖 폼) | `evidence/04_VerifyKlarfUploadSmoke.stdout.txt`, `evidence/04_screen_upload.png` (직접 열어 본 내용: XD106.02 읽기 완료, Wafer 1 / Defect 15 / 이미지 15/15, Grid 15행, Defect 1 X -23,229.42 Class 54 RDLshort, Map 미리보기에 Defect 색 표시, 상태 '읽음(미저장)') |
| SCN-10 | 정상(실사용) | 화면 확인 대기 | — | 실제 DMS 메뉴 업로드 → Defect Map Viewer는 사용자 확인 필요 |

보조 증거:
- `evidence/00_precheck.txt`: docker `oracle19c Up (healthy)`
- `evidence/02_ReadOnly_RedCheck.stdout.txt`: 기대값을 16으로 바꾸면 `FAIL / Defect 16 건 / 15`, `=FAIL (1 / 11)`
- `results.json`: 실행 4건의 상태·종료 코드·소요 시간

## 3. 결함 (FAIL)

제품 결함은 없다. 테스트 쪽에서 아래 두 가지를 찾았다.

### T-01 [Low / 테스트 결함] KlarfUploadProbe가 실제 샘플 Wafer 키를 그대로 사용
- 현상: 사용자가 화면에서 같은 샘플을 올려 두면 Probe가 항상 SKIPPED가 되어, 저장·재등록·삭제 회귀를 돌릴 수 없다.
- 근거: `KlarfUploadProbe.ps1`의 '사용자 데이터 보호' 분기(미리보기 결과 EXISTS + USER가 PROBE/SMOKE가 아니면 종료 코드 2), 이번 실행 결과 `KLARF_UPLOAD_PROBE=SKIPPED`.
- 제안: 샘플 KLARF를 TEMP에 복사한 뒤 Lot ID나 WaferID를 시험 전용 값(예: `PROBE_` 접두어)으로 바꿔 쓴다. 그러면 사용자 데이터와 키가 겹치지 않는다. 화면 Smoke(`Verify-KlarfUpload.ps1`)에도 같은 문제가 있다.

### T-02 [Low / 테스트 결함] Verify-KlarfUpload.ps1이 일부를 건너뛰고도 `=PASS`로 끝남
- 현상: 저장 검사를 건너뛰었는데 결과 키가 `PASS`이고 종료 코드가 0이다. 종료 코드만 보는 자동화는 이것을 전체 통과로 읽는다.
- 제안: 이런 경우 `=PASS_PARTIAL`로 쓰거나 종료 코드 2로 끝낸다. 지금은 program-verify 실행기가 SKIP 줄을 보고 `PASS_PARTIAL`로 다시 판정한다.

## 4. BLOCKED / SKIPPED

| ID | 사유 | 필요한 조치 (누가) |
| --- | --- | --- |
| SCN-03, 04, 08, 02 일부, 09 저장 | 샘플 Wafer에 사용자 업로드 STEP 501(USER=ADMIN)이 있음. Probe가 실행하면 이 데이터가 지워짐 | (사용자) ① 화면에서 STEP 501을 지워도 되면 지운 뒤 재실행, 또는 ② T-01처럼 시험 전용 키를 쓰는 Probe로 바꾸는 작업을 승인 |
| SCN-06 | 기대값을 정할 근거 없음 | (사용자) SetupID가 128자를 넘을 때 거부할지 자를지 결정 |
| SCN-07 | Defect 0건 샘플 없음 | (Claude) 합성 KLARF + 신규 Probe 작성 (②와 함께) |

## 5. 검증하지 않은 것

- **현재 배포 DLL(2026-10-08 12:22)로 새로 저장하는 경로.**
  - STEP 501은 2026-10-08 저장 데이터다(이미지 경로 `FTP/SCK/20261008/...`). 그것을 저장한 DLL이 지금 배포본과 같은지는 확인하지 않았다.
  - 따라서 SCN-01과 SCN-05는 "저장된 데이터가 규칙에 맞다"까지만 확인한 것이다. "지금 코드가 그렇게 저장한다"는 확인하지 않았다.
- 재등록(D1), 저장소 설정 오류 시 롤백, 업로드 STEP 삭제, 기존 SELECT_SETUP_SEQ와 SELECT_STEP_INFO 회귀.
- 실제 DMS 메뉴에서의 사용 흐름, Remoting 서비스를 거친 호출, Image Gallery 썸네일. 썸네일은 D4 상대 경로 때문에 표시되지 않을 수 있다는 점이 설계 문서에 기존 제한으로 적혀 있다.
- 고객 DB, 이미지 FTP(`DEFECT_FTP`) 접속, 대용량 KLARF, 동시 업로드.
- 이번 실행에서는 `Invoke-OracleQuery.ps1`(직접 SQL 사전·사후 비교)을 쓰지 않았다. DB 쓰기가 없었고, 조회는 앱과 같은 BSL 경로로 했다.

## 6. 다음 조치

- [ ] (사용자) 4절 ① 또는 ② 선택. ②를 고르면 Claude가 시험 전용 키를 쓰는 Probe로 SCN-03·04·07·08을 이어서 실행
- [ ] (사용자) SCN-06 기대 동작 결정
- [ ] (사용자) SCN-10 DMS 화면 확인 후 결과 알려 주기 → 화면 확인으로 갱신
- [ ] (사용자 확인 후) `DMS/Assistant/기능/F08_KLARF_업로드_DB적재.md`·변경 이력에 이번 확인 수준 기록 — Test Agent는 공유 문서를 임의로 고치지 않음
