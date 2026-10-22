# E-Test Para 등록 / 설정(Spec 포함) / 결과 업로드 + 진행 표시줄 — 직접 시험 시나리오

작성: 2026-10-14 (CLAUDE-090 5단계 / CLAUDE-094, 14:21 CLAUDE-095 화면 재구성, 14:58 CLAUDE-096 등록 / 설정 화면 분리 반영) / 대상: DMS 화면에서 사용자가 직접 확인
근거: [Para 등록 / 설정 화면 분리 설계](2026-10-14_ETest_Para_등록_설정_화면_분리_설계.md) / [Para 화면 재구성 설계](2026-10-14_ETest_Para_화면_재구성_설계.md) / [Para Setup 설계](2026-10-14_ETest_Para_Setup_설계.md) 5절(이름 변환) / [Upload 재설계](2026-10-13_ETest_Upload_재설계.md) 9·10절 / [POC 화면 진행 표시줄](../06_환경_운영/2026-10-14_POC_화면_진행표시줄.md)

- 순서: 1 서버(선택) → 2.1 Para 등록(결과 파일) → 2.2 Para 설정(GDS Chain + Spec) → 4 결과 업로드. 앞 단계의 등록 / Spec 이 뒤 단계의 기대값을 만든다.
- E-Test Limit 설정 화면은 없어졌다. Spec 은 E-Test Para 설정 화면에서 고친다(3절).
- 각 행의 **기대 결과**는 같은 샘플로 자동 검사(Smoke / Probe)에서 확인한 값이다. 다르면 "확인" 칸에 실제 값을 적는다.
- 모든 화면 아래 오른쪽에 **진행 표시줄**이 있다. 각 단계에서 "진행" 열의 문구가 보이는지도 같이 본다(공통 확인은 0.4절).

## 0. 시험 전 준비

### 0.1 프로그램 반영 (Claude 는 배포 안 함)

**먼저 다시 빌드한다.** 로컬 DB(대표 Chain 테이블 삭제, GDS_CHAIN_NAME 추가)와 쿼리 XML(`08_DBQuery`)은 이미 바뀌었기 때문에, 예전 DLL 로는 E-Test Para / 업로드 화면이 오류를 낸다.

| 순서 | 프로젝트 | 출력 |
| --- | --- | --- |
| 1 | `02_DMS/DACrux.SEMDMS.Interface` | ClientBin (ServerBin 에도 같은 DLL 필요) |
| 2 | `02_DMS/DACrux.SEMDMS.DSL`, `DACrux.SEMDMS.BSL` | ServerBin |
| 3 | `02_DMS/DACrux.SEMDMS.RO`, `DACrux.SEMDMS.Control` (공통 진행 표시줄) | ClientBin |
| 4 | `02_DMS/DACrux.SEMDMS.ENGUI`, `09_TEST/DACrux.TEST.ENGUI` | ClientBin |
| 5 | `06_CoreLibSource/GdsMapControl` (GDS 화면 경과 시간 칸) | `100_ClientBin/GdsMap` |

- 그다음 **SEMDMS Service 다시 시작 → DACrux 다시 시작**한다. GdsMap EXE 는 DACrux 를 다시 시작해야 바뀐다.
- 참고(14:21): 운영 폴더 DLL 은 14:16 에 다시 빌드됐다(새 Para 화면 포함, BSL 의 "같은 Spec 은 다시 쓰지 않음" 14:17 변경은 빠짐). Setup 2026.10.14.2(14:10)에는 그보다 앞선 중간 소스가 들어 있다. 위 프로젝트를 한 번 더 빌드하면 맞는다.
- 메뉴 파일(`01_WinApp/DACruxV5/MenuFile.mnu`, `100_ClientBin/MenuFile.mnu`): **E-Test Limit 설정** 메뉴 삭제, **E-Test Para 등록** 메뉴 추가(POC 5, 새 아이콘), E-Test Para 설정은 POC 6.
- 참고(14:58): 등록 / 설정 화면 분리(CLAUDE-096)는 운영 폴더 / Setup 에 아직 없다. ENGUI 를 다시 빌드해야 E-Test Para 등록 메뉴가 열린다.

### 0.2 로컬 DB 상태 (2026-10-14 14:20 확인)

| 항목 | 상태 |
| --- | --- |
| DDL | `2026-10-14_tqp_gds_et_para_rep_chain.sql` / `_limit_fk.sql` / `_local_reset.sql` / `_upload_registered.sql` / **`_para_gds_chain.sql`**(GDS_CHAIN_NAME 추가, `TQP_GDS_ET_REP_CHAIN` / `TQP_GDS_ET_CHAIN_LINK` 삭제) 모두 적용 |
| SCK E-Test | 등록 Parameter OMM / TSVET **22개**(13:12 ADMIN), Spec 0, 업로드 파일 0. 13:12 에 만든 대표 Chain `PIC_SERP1_M2_FL` 은 테이블과 함께 지워짐(DB 휴지통에 남음) |
| GDS Device | Sub Device `OMM` → `U9`. U9 GDS Setup Chain 15개 (예: `TSVDC_ASIC1_P_FH`, `TSVDC_TR_FH`, `TSVDC_L_P_FH`, `TSVDC_TOP1_P`) |
| Map | `OMM_Hybrid` 만 있음. E-Test Map ID(= 파일 Device) `OMM` 은 없음 → 2026-10-15 부터 업로드에 Map 이 필수라 U-00 에서 먼저 만든다 |

### 0.3 시험 파일

- 결과 파일: `SampleFile\26_10_06 Sample File\CAI_OMM_ATV_A0_TSVET_20260609161940_RH-HS2615_RH-HS2615-02.xlsx` (TSVET, Die 24, 헤더 2,755)
- 고객 Spec 표(탭 구분 텍스트): `DMS\02_DMS\DACrux.SEMDMS.ENGUI\Verification\EtSpecTable_U9_20261012.txt` — 메모장으로 열고 전체 선택(Ctrl+A) → 복사(Ctrl+C). 119행(TSVET 37 / COWET 40 / FT 42).
- 여러 파일 시험용 복사본(4절): 시험 폴더(예: `D:\ETestTest`)에 샘플을 아래 이름으로 복사한다.

| 복사본 이름 | 의도 | 기대 상태 |
| --- | --- | --- |
| `CAI_OMM_ATV_A0_TSVET_20260609161940_RH-HS2615_RH-HS2615-02.xlsx` | 원본 (Slot 2) | 준비 |
| `CAI_OMM_ATV_A0_TSVET_20260609161940_RH-HS2615_RH-HS2615-03.xlsx` | 다른 Slot (3) | 준비 |
| `CAI_OMM_ATV_A0_TSVET_20260610090000_RH-HS2615_RH-HS2615-02.xlsx` | 같은 Wafer, 다른 시험 시각 | 오류: 목록 안 중복 |
| `CAI_OMM_ATV_A0_TSVET_20260609161940_RH-HS2615_RH-HS2615-XX.xlsx` | Slot 없음 | 오류: Slot 입력 → 고치면 준비 |
| `CAI_ZZTEST_ATV_A0_TSVET_20260609161940_RH-HS2615_RH-HS2615-07.xlsx` | 등록 안 된 Device | 오류: 등록 Parameter 없음 |
| `CAI_OMM_ATV_A0_TSVET_20260609161940_RH-HS2615.xlsx` | 파일명 칸 부족 (7칸) | 오류: 파일 읽기 실패 |

### 0.4 공통 확인 — 화면 아래 진행 표시줄

| 번호 | 조작 | 기대 결과 | 확인 |
| --- | --- | --- | --- |
| C-01 | 아무 화면이나 열기 | 아래 줄: 왼쪽 = 기존 상태 문구, 오른쪽 = 진행 문구 "대기" + 빈 막대 | ☐ |
| C-02 | 서버 조회 / 저장 / 삭제 버튼 | 작업 중: 오른쪽에 `작업 이름 중 · 1.2초 · 단계` 가 0.2초마다 늘고, 막대가 움직인다(전체 수를 알면 n / 전체 로 찬다). 마우스 커서 모래시계 | ☐ |
| C-03 | 작업 중 그 화면의 버튼 / 표 클릭, 키 입력 | 반응 없음(무시). 다른 메뉴 화면 / DACrux 메뉴는 사용 가능 | ☐ |
| C-04 | 작업 중 그 화면 닫기(X / 탭 닫기) | 닫히지 않고 오른쪽 문구가 `... 중 · 끝난 뒤 닫을 수 있습니다` | ☐ |
| C-05 | 작업이 끝남 | `작업 이름 완료 · n.n초 (HH:mm:ss)` 회색 + 막대 가득. 실패면 빨간 `실패`, 중간 단계만 실패하면 `끝 (오류 있음)` | ☐ |

## 1. 서버 (자동 검사, 선택)

화면 시험 전에 서버만 따로 확인하려면 실행한다. 로컬 DB 에 Factory `PROBE` 시험 데이터를 만들고 끝나면 지운다(SCK 데이터는 건드리지 않음). 배포 전이면 `-BinDir` 에 TEMP 빌드 폴더를 넘긴다(생략하면 운영 폴더 DLL).

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File 'D:\1_프로젝트 문서\18.스테츠칩팩_POC\ChippacPOC\DMS\06_CoreLibSource\GdsMapControl\Probes\EtParaSetupProbe.ps1'
powershell -NoProfile -ExecutionPolicy Bypass -File 'D:\1_프로젝트 문서\18.스테츠칩팩_POC\ChippacPOC\DMS\06_CoreLibSource\GdsMapControl\Probes\GdsEtResultProbe.ps1'
```

| 번호 | 기대 결과 | 확인 |
| --- | --- | --- |
| S-01 | `ET_PARA_SETUP_PROBE=PASS (26/26)` (등록 / 수정 행만 저장 / 틀리면 전체 거부 / Spec 값이 같으면 다시 안 씀 / Spec 삭제 / GDS 목록 / 인계 API) | ☐ |
| S-02 | `ET_RESULT_PROBE=PASS (18/18)` (등록 37 저장 / 서버 판정 = 화면 미리보기 888개 / 다시 판정 / Map 좌표 / 거부 5종) | ☐ |

## 2. E-Test Para 등록 / 설정

### 2.1 E-Test Para 등록 (POC > E-Test Para 등록) — 결과 파일로 등록만

| 번호 | 조작 | 기대 결과 | 진행 표시줄 | 확인 |
| --- | --- | --- | --- | --- |
| R-01 | DACrux 다시 시작 → POC 메뉴 | 4 E-Test 결과 업로드 / **5 E-Test Para 등록**(표 문서 + 초록 '+' 아이콘) / **6 E-Test Para 설정**(차트 + 톱니 아이콘) / 7 파일 Map Setup ... `E-Test Limit 설정` 없음 | — | ☐ |
| R-02 | E-Test Para 등록 열기 | Factory SCK(입력 가능). Device / 공정 / 파일 칸 회색(읽기 전용). 탭 / 저장 / 삭제 버튼 없음, 버튼은 [결과 파일 열기...] / [보이는 신규 선택] / [선택 해제] / [등록] 만 | — | ☐ |
| R-03 | [결과 파일 열기...] → 샘플 | Device `OMM` / 공정 `TSVET` / 파일 이름 표시. 정보 줄 `헤더 2755 / 신규 2733 / 등록됨 22 / 등록 불가 0 (Factory SCK / Device OMM / 공정 TSVET)`(등록됨 = 이미 등록한 수). 행 머리글 1 ~ 2755(왼쪽 위 칸 "열") | `결과 파일 열기 중 · 파일 읽기 / 등록 Parameter 조회` → `결과 파일 열기 완료` | ☐ |
| R-04 | 1~6행 드래그 → 우클릭 | 메뉴 `선택 행 목록에서 삭제 (6)` → 정보 줄 `목록에서 뺌 6` | — | ☐ |
| R-05 | 다른 행 하나 클릭 → Del 키 / 우클릭 [삭제한 행 모두 되살리기] | 7행 뺌 → 되살리면 2755. 뺐던 행의 체크는 풀려 있다 | — | ☐ |
| R-06 | 찾기 `tsvdc` + [신규만] → 등록할 행을 직접 체크(또는 [보이는 신규 선택]) → **[등록]** → 확인 | 질문 `Factory SCK / OMM / TSVET 에 Parameter n개를 등록합니다.` → `등록 n개 / 이미 있어 건너뜀 0개 -> GDS Chain / Spec / 사용은 E-Test Para 설정 화면에서 입력`. 체크한 행 `등록됨`(회색, 체크 불가) | `등록 (n개) 중 · 서버 저장` → `등록 (n개) 완료` | ☐ |
| R-07 | (2.2 를 한 뒤) 이 화면에서 파일 다시 열기 | 설정 화면에서 등록한 행은 상태 `등록됨`. (2026-10-21 부터 GDS Chain / Spec / 메시지 열은 숨김이라 화면에서 보지 않는다.) 파일 칸 아래 `규칙 [Customer]_[Device]_[Wafer Type]_[Wafer Rev]_[Test Type]_[Test Date]_[Lot]_[Lot-Wafer No]$Fab$` 이 보임 | — | ☐ |

### 2.2 E-Test Para 설정 (POC > E-Test Para 설정) — 등록된 Parameter 조회 / Spec 표 붙여넣기 / GDS Chain · Spec 수정

> 2026-10-15 화면 변경(사용자 VS 디자이너): [Spec 표 붙여넣기] / [선택 삭제] / 찾기 / Spec 있는 것만 / GDS Chain 없는 것만은 숨김, 정보 줄과 우클릭 메뉴(Spec 비우기 / 수정 취소)는 삭제. 그래서 P-03 · P-04 · P-12 · P-14 · P-18 · P-19 와 P-06 · P-08 의 정보 줄 / 우클릭 부분은 화면에서 할 수 없다. 걸러 보기는 P-20 필터 콤보로 한다. 수정 취소는 칸을 원래 값으로 다시 입력하거나 [조회] -> 버림(예).

| 번호 | 조작 | 기대 결과 | 진행 표시줄 | 확인 |
| --- | --- | --- | --- | --- |
| P-01 | E-Test Para 설정 열기 | Factory SCK. Device 콤보 목록 `OMM`(등록된 Device 만, **목록에서만 고름 — 직접 입력 안 됨**), 공정 비어 있음. 탭 / 파일 열기 버튼 없음 | `Device 목록 완료` | ☐ |
| P-02 | Device 콤보 `OMM` 고르기 | 공정 `TSVET` 이 채워지고 **바로 조회**: 상태 `조회 완료 / Device OMM / 공정 TSVET / 등록 Parameter 22+n / Spec 0 / GDS Device OMM`. 오른쪽 GDS Device `OMM`(위 Device 그대로, 읽기 전용 - 고르지 않음), `GDS Setup Chain 15개`(사용 수 0) | `조회 완료` | ☐ |
| P-03 | [Spec 표 붙여넣기] (Spec 표 복사 상태) | 질문: `Spec 119건을 저장합니다. 공정별: COWET 40 / FT 42 / TSVET 37`, `Device: OMM (표의 Device U9 대신)`, `등록되지 않은 Parameter k개는 Spec 표 출처로 먼저 등록합니다(...)` (k = COWET 40 + FT 42 + TSVET 중 아직 등록 안 된 수) | — | ☐ |
| P-04 | 질문에 [예] | `Spec 표 저장 완료 / COWET 40 / FT 42 / TSVET 37 / 새로 등록한 Parameter k / Device OMM / 지금 공정 TSVET`. 공정 콤보 `COWET / FT / TSVET`. 업로드 파일이 없어 다시 판정 질문은 없음 | `Spec 표 저장 (119건) 중 · COWET 등록 / Spec 저장 (n / 3)` → 완료 | ☐ |
| P-05 | TSVET 행 확인 | `Resistance_iforce=8.0mA@TSVDC_R_N_FH`: Upper 715 / Lower 빈 칸 / Enable 체크 / 단위 Ohm / Spec 원문 `If val > 715, Fail; otherwise, Pass`. Leakage 행 ±50 uA. 값 통계 열(파일 수 ~ Fail)은 없다. | — | ☐ |
| P-06 | 오른쪽 목록 `TSVDC_TR_FH` 더블클릭 → 표 `..._TSVDC_TR_FH` 행 **GDS Chain** 칸 클릭 → Ctrl+V | 아래 줄 `복사: TSVDC_TR_FH -> ...` → 칸에 `TSVDC_TR_FH`, 노란색 + 행 머리글 `*`, 정보 줄 `수정 1행 (저장 전)`, 목록 사용 수 1 | — | ☐ |
| P-07 | `..._TSVDC_ASIC1_P_FH` 행 GDS Chain 칸 **더블클릭** → `tsvdc_asic1_p_fh` 입력 → Enter | 대소문자만 다르면 목록에 있는 이름으로 봄(보통 글자), 목록 `TSVDC_ASIC1_P_FH` 사용 수 1 | — | ☐ |
| P-08 | 다른 행 GDS Chain 에 `ZZ_NONE` 입력 → 그 행 우클릭 [선택 행 수정 취소] | 입력 직후 **주황 글자** + 정보 줄 `(목록에 없음 1)` → 수정 취소하면 빈 칸, 노란색 / `*` 없어짐 | — | ☐ |
| P-09 | TR 행 Lower 더블클릭 → `800` → [저장] | Lower 칸 붉은색 → 안내 `입력을 확인하세요(붉은 칸) ... Lower 가 Upper 보다 큽니다` → 아무것도 저장 안 함. 그 뒤 Lower 를 다시 비운다(더블클릭 → 지우기 → Enter) | — | ☐ |
| P-10 | TR / ASIC1_P 가 아닌 다른 한 행: [사용] 클릭(해제) + 비고 더블클릭 입력 | 사용 해제 행은 회색 글자. 바뀐 칸 노란색 | — | ☐ |
| P-11 | [저장] | `저장 완료 / 수정 3행 / Spec 저장 0 / Spec 삭제 0 / Device OMM / 공정 TSVET`(GDS Chain / 사용 / 비고만 바꿔 Spec 은 다시 쓰지 않음). 다시 읽은 표에 노란색 / `*` 없음, GDS Chain 유지. **시험 뒤 [사용] 은 다시 켜고 저장**(4절 기대값 기준) | `저장 (3행) 중 · 서버 저장` → `저장 (3행) 완료` | ☐ |
| P-12 | Spec 행 우클릭 [선택 행 Spec 비우기] → 우클릭 [선택 행 수정 취소] | Lower / Upper 빈 칸 + Enable 해제 + 노란색 → 취소하면 원래 값 | — | ☐ |
| P-13 | R-06 에서 등록만 한(Spec 없는) 행 Upper 더블클릭 → `5` → 수정 취소 | Upper 를 넣으면 **Enable 자동 체크** → 취소하면 원래대로 | — | ☐ |
| P-14 | [Spec 있는 것만] / [GDS Chain 없는 것만] / 찾기 `asic1_p` | 다시 조회하지 않고 거름. 찾기 = 헤더 원문 / 저장 이름 / Chain 1 · 2 / GDS Chain 글자. 수정 중인 행은 걸러도 보임 | — | ☐ |
| P-15 | 한 칸 고친 뒤 [조회] → [아니요] | 질문 `저장하지 않은 수정 1행이 있습니다. 버리고 계속할까요?` → 아니요면 그대로. (예 = 버리고 다시 읽음) | — | ☐ |
| P-16 | (2026-10-15 바뀜) GDS Setup Chain 이 없는 Device(예: 등록만 하고 GDS Map / Chain 이 없는 Device)를 위에서 고르기 | 오른쪽 GDS Device 칸 = 그 Device 이름(입력 / 선택 불가), 목록 0개 `GDS Setup Chain 0개`, 표의 GDS Chain 값은 모두 주황(목록에 없음). 표 / Spec 수정 / 저장은 그대로 된다 | `조회 완료` | ☐ |
| P-17 | 수정이 남은 상태에서 공정 콤보 `COWET` 고르기 → [아니요] → 다시 고르기 → [예] | 아니요 = 공정 TSVET 로 되돌아가고 수정 유지. 예 = 바로 조회 40행 / `Spec 40` | `조회 완료` | ☐ |
| P-18 | COWET 2행 선택 → [선택 삭제] → 확인 | `Parameter 2개를 삭제합니다. Spec 이 있는 2개는 Spec 도 같이 지웁니다.` → `삭제 2개`, 38행. (되돌리려면 Spec 표를 다시 붙여넣는다) | `삭제 (2개) 완료` | ☐ |
| P-19 | (선택, 마지막에) **대량 삭제 속도**: 등록 화면에서 [보이는 신규 선택] → [등록] → 설정 화면 TSVET Ctrl+A → [선택 삭제] | 진행 표시줄이 계속 움직이다 `삭제 (n개) 완료`(로컬 측정: 2,755개 서버 0.1초). 값이 저장된 Parameter 가 섞이면 전체 거부. **등록 / Spec 이 모두 지워지므로 4절 뒤에 하거나 생략** | `삭제 (n개) 중 · 서버 삭제` → 완료 | ☐ |
| P-20 | **필터 콤보** (표 위 측정 / 조건 / Chain 1 / GDS Chain / 사용 / Enable): 조건 콤보 펼치기 → `IFORCE=8.0MA (n)` 고르기 → GDS Chain 콤보 펼치기 → `(빈 값)` 고르기 → [필터 해제] | 펼친 목록 = `(전체)` + 값(건수 없음, 다른 콤보에서 고른 값을 통과한 행 기준, (빈 값)은 끝). 고르면 그 값 행만 보이고 상태 줄 `필터 조건=... / 표시 n / 전체 m`. 두 콤보는 AND. 수정 중인 행은 걸러도 보임. 같은 공정 [조회] 는 고른 값 유지, 공정을 바꾸면 모두 (전체). [필터 해제] = 모두 (전체) + 전체 행 | — | ☐ |

## 3. E-Test Limit 설정 — 없어짐

2026-10-14 화면 재구성으로 E-Test Para 설정 화면(2.2절)에 합쳤다.

| 번호 | 조작 | 기대 결과 | 확인 |
| --- | --- | --- | --- |
| L-01 | DACrux 다시 시작 후 POC 메뉴 (R-01 과 같이 확인) | `E-Test Limit 설정` 메뉴 없음. Spec 붙여넣기 / 수정 / 삭제 / 다시 판정은 E-Test Para 설정에서 한다 | ☐ |

## 4. E-Test 결과 업로드 (POC > E-Test 결과 업로드)

| 번호 | 조작 | 기대 결과 | 진행 표시줄 | 확인 |
| --- | --- | --- | --- | --- |
| U-00 | **Map 먼저** (2026-10-15 Map 필수): [파일 열기...] → 원본 샘플 → 오류 확인 → 파일 Map Setup 에서 OMM_Hybrid 를 만든 KLARF 로 Map ID 를 `OMM` 으로 저장 → 업로드 [다시 읽기] | 처음 `오류`: `Map 없음: File Map Setup 에 Map ID OMM (사용 Die 가 있는 Map) 을 먼저 만드세요`, [저장] 꺼짐 → Map 저장 뒤 [다시 읽기] 하면 `준비`, 1단계 Map (X,Y): DUT #20 → (9,2) | `분석 완료` | ☐ |
| U-01 | [파일 열기...] → 원본 샘플 1개 (2026-10-15 업로드는 파일 하나씩) | 위 둘째 줄 상태 `준비`(초록), 메시지 `저장할 수 있습니다.`. 정보 표 11행: 파일명 / Device OMM / 공정 TSVET / Lot RH-HS2615 / Lot-Wafer No RH-HS2615-02 / Slot / WAFER_ID `2 / RH-HS2615-02` / Test Date / Customer · Wafer Type · Rev / Die 24 / 헤더 2,755 / 등록(사용 Y). Device · 공정 · Lot · Lot-Wafer No 값 칸은 옅은 파란색(더블클릭 수정). 저장 Parameter 아래 행은 없음 | `분석 중` → `분석 완료` | ☐ |
| U-02 | 아래 결과 보기 | 아래 결과(2026-10-15 공통 결과 Control): 정보 표 / 1단계 Die 24행: DUT #44 Fail 6(빨간 행), #20 Pass(DUT Flag F 는 참고) / 2단계 정의 Para 값 `표시 n / 전체 n (Parameter n x Die 24)`. [Fail 만] → 6행 — 예: `Resistance_iforce=8.0mA@TSVDC_BOT1_N_FH` = 719 > Upper 715, 사유 `Upper 초과`. 열: Chain / GDS Chain | — | ☐ |
| U-03 | [Fail 만] 켠 채 1단계 Die 표에서 #44 / #20 클릭 (Ctrl 로 여러 Die) | 2단계 6행 / 0행, 제목 `2단계: 정의 Para 값 (DUT 44)`. [Fail 만] 끄면 #44 = Parameter 수만큼. ['-' 만] / 찾기 칸(이름 · 헤더 · Chain 글자)도 거름 | — | ☐ |
| U-04 | [파일 열기] 로 0.3 시험 폴더 파일을 하나씩: 다른 시각 / XX / ZZTEST / 7칸 | 다른 시각 `준비`(원본을 아직 저장 안 했으므로, 저장 뒤 열면 `중복`), XX `오류`(Slot 없음: Lot-Wafer No RH-HS2615-XX 끝에 '-숫자(1~99)' 가 없습니다. 정보 표의 Lot-Wafer No 를 더블클릭해 고치세요.), ZZTEST `오류`(등록 Parameter 없음: ... Device ZZTEST ...), 7칸 `오류`(파일 읽기 실패 ..., 아래 결과 비움). 오류면 [저장] 꺼짐 | `분석 완료` | ☐ |
| U-05 | XX 파일을 연 채 정보 표 Lot-Wafer No 칸(옅은 파란색) 더블클릭 → `RH-HS2615-06` 입력 → Enter. (같은 방법으로 Device / 공정 / Lot 도 고칠 수 있음: 예 ZZTEST 파일의 Device 를 `OMM` 으로) | 다시 분석 → 상태 `준비`, Slot / WAFER_ID `6 / RH-HS2615-06`, [저장] 켜짐. 저장하면 DB 에 고친 값(Lot-Wafer No / WAFER_ID), 파일명은 원래 이름. 저장 뒤에는 고칠 수 없음. 위 Slot 칸은 숨김 | `다시 분석 완료` | ☐ |
| U-06 | [저장] → 확인. 원본 / Slot3 / XX(Slot 6) 를 하나씩 열어 같은 방법으로 3개 저장 | 질문 `E-Test 결과 파일을 저장합니다. 등록 Parameter(사용 Y)만 저장합니다.` + `파일: 이름` + `WAFER_ID RH-HS2615-02 / 공정 TSVET` + `저장: Parameter n x Die 24 = 값 n` + `등록되지 않은 헤더는 저장하지 않습니다 (n개). 나중에 등록한 Parameter 는 이 파일을 다시 업로드해야 보입니다.` → 상태 `저장됨`(파랑), 메시지 `저장 완료 / 파일 번호 n / WAFER_ID ... / Spec Fail Die 1 / 값 n / Map OMM 좌표 Die 24`, 결과 제목 `업로드 결과 - 저장됨 (파일 번호 n)`, [저장] 꺼짐. 안내 끝 `저장한 결과는 E-Test 결과 조회 화면에서 봅니다.` | `저장 중 · 서버 저장` → `저장 완료` | ☐ |
| U-07 | POC > **E-Test 결과 조회**(2026-10-15 신규 메뉴, 결과 업로드 다음) → Device `OMM` 고르기 | 공정 콤보 `(전체)` / `TSVET`, 바로 조회: 파일 3건, Spec Fail Die 1(빨간 글자), 추가 Para 0, Spec 바뀜 N, Map `OMM`, Map 좌표 Die 24. 첫 행 결과가 아래에 보임. 업로드 화면에는 [등록 파일] 탭이 없음 | `파일 목록 조회 완료` → `결과 조회 (파일 번호 n) 완료` | ☐ |
| U-08 | 목록에서 다른 한 건 클릭 / Lot 칸에 `hs2615` + Enter | 아래 결과가 업로드 화면과 같은 모양 · 같은 값(정보 항목 순서 / Die 24 / 2단계 행 순서). 제목 `저장된 결과 - 파일 번호 n`. #44 + [Fail 만] 6행. Lot 글자는 대소문자 무시 | `결과 조회 (파일 번호 n) 완료` | ☐ |
| U-09 | 원본을 다시 [파일 열기] → [저장] → [아니요] | 상태 `중복`(메시지 `DB 에 같은 Wafer + 공정 파일 있음 (파일 번호 n ...)`) → 질문 끝 `예 = 기존 파일을 지우고 다시 저장(덮어쓰기) / 아니요 = 저장하지 않음` → [아니요] 면 그대로 `중복` | — | ☐ |
| U-10 | 다시 [저장] → [예] | 덮어쓰기: 기존 파일 지우고 새 파일 번호로 `저장됨` | `저장 완료` | ☐ |
| U-11 | **다시 판정**: E-Test Para 설정(OMM / TSVET) → R_N 행 Enable 클릭(해제) + `..._L_P_FH` Upper 더블클릭 `720` → [저장] | 질문 `Device OMM / 공정 TSVET 의 업로드 파일 3개를 바뀐 Spec 으로 다시 판정할까요?` → [예] → `저장 완료 / 수정 2행 / Spec 저장 2 / Spec 삭제 0 / ... / 다시 판정: TSVET 3개`. E-Test 결과 조회 #44 Fail 4 (다시 판정했으므로 Spec 바뀜 N) | `저장 (2행) 중 · 서버 저장 / 다시 판정 (TSVET 3개)` → `저장 (2행) 완료` | ☐ |
| U-12 | 같은 변경을 되돌리고(Enable 켜기, Upper 715) 저장 → [아니요] → 다시 한 칸 바꿔 저장 후 [예] | [아니요] 면 판정 그대로(#44 Fail 4) + 조회 목록 Spec 바뀜 `Y`(주황), [예] 면 #44 Fail 6 + Spec 바뀜 N | — | ☐ |
| U-13 | **사용 안 함**: E-Test Para 설정에서 R_N 행 [사용] 해제 → 저장 → 업로드에서 Slot 3 파일 [파일 열기](또는 [다시 읽기]) | 2단계 건수 표시 `Parameter n x Die 24` 가 1 줄고 메시지에 `사용 안 함 1 저장 안 함` (사용 안 함 Parameter 는 저장 / 판정 대상이 아님). 확인 뒤 사용으로 되돌린다 | — | ☐ |
| U-14 | **분석 뒤 등록이 바뀜**: 파일을 열어(분석) 둔 채 E-Test Para 등록 화면에서 신규 헤더 1개 더 등록 → 업로드 [저장] | 그 파일 `실패`: `... 등록 Parameter 가 분석한 뒤 바뀌었습니다(분석 n / 지금 n+1). 파일을 다시 분석하세요.` (저장 도중 만든 파일은 지움, [저장] 꺼짐) → [다시 읽기] 하면 `준비`. 그 전에 올린 파일은 E-Test 결과 조회 목록 추가 Para `1`(노란 배경). 시험 뒤 그 Parameter 는 지운다(값이 저장됐으면 파일 먼저 삭제) | — | ☐ |
| U-15 | E-Test Para 설정에서 값이 저장된 Parameter 를 [선택 삭제] | 거부: `삭제 실패: ... 측정값 n개가 저장된 Parameter(사용 안 함으로 바꾸세요)` | `삭제 (1개) 끝 (오류 있음)` | ☐ |
| U-16 | **Map 좌표 다시 계산 없음** (2026-10-15 삭제): E-Test 결과 조회 화면 위 버튼 확인 | [조회] / [선택 파일 삭제] 만 있음. Map 을 고쳤으면 그 파일을 업로드 화면에서 다시 올린다(중복 → [예] 덮어쓰기) | — | ☐ |
| U-17 | E-Test 결과 조회에서 한 건 [선택 파일 삭제] | 질문에 `YMS 에 올린 데이터도 함께 삭제` → 목록에서 빠짐. 등록 Parameter 는 남음 | `파일 삭제 완료` | ☐ |

## 5. 진행 표시줄 — 다른 POC 화면

| 번호 | 화면 | 조작 | 기대 결과 | 확인 |
| --- | --- | --- | --- | --- |
| G-01 | KLARF File 업로드 | [파일 선택] → KLARF | 아래 오른쪽 `KLARF 읽기 중 · 파일 읽기 / 원본 WaferID 읽기 / 이미지 찾기` → `KLARF 읽기 완료`. 이미지 폴더를 고르면 `이미지 찾기 (n개)` | ☐ |
| G-02 | KLARF File 업로드 | [DB 저장] | `서버에서 파일 확인` → (질문) → `이미지 전송 (n / 전체)` → `DB 저장 완료`. 기존 상태 문구 / 결과 안내는 그대로 | ☐ |
| G-03 | KLARF 파일 조회 | [파일 선택] / [다시 읽기] | `KLARF 읽기 완료 · n초` | ☐ |
| G-04 | 파일 Map Setup | [읽기] (KLARF) / [DB 저장] | `KLARF 읽기 / 원형 Map 만들기 완료`, 저장 `Map ID 확인` → (질문) → `Map 저장`(또는 `Map 덮어쓰기`) 완료 | ☐ |
| G-05 | Wafer Map 조회 | [조회] / 목록에서 Map 고르기 | `Map 목록 조회 완료`, `Map 조회 (Map ID) 완료` | ☐ |
| G-06 | GDS-KLARF 연관 분석 | 왼쪽 Wafer [Search] / [분석] | `Wafer 조회 완료`, `분석 (U9) 중 · n초` → 완료(분석 중 이 화면 입력은 막힘). 2줄 상태 문구는 그대로 | ☐ |
| G-07 | GDS Map 설정 | [파일 열기] / [DB 저장] | 기존 진행 막대 옆에 `경과 n.n초`(0.2초마다) → 끝나면 `걸린 시간 n.n초` | ☐ |
| G-08 | GDS Map 조회 | [조회] / Chain [DB 저장] | 조회: 진행 막대 + `경과 n.n초`. Chain 저장: 이제 막대(Marquee)도 움직임 + 경과 시간 | ☐ |
| G-09 | Die 별 분석(상세) | Chip 더블클릭으로 상세 열기 | 아래 상태 줄 진행 막대 + `경과 n.n초` → `걸린 시간` | ☐ |

## 6. 시험 데이터 정리 / 결과 기록

- 업로드 파일은 E-Test 결과 조회 → [선택 파일 삭제]. 등록 Parameter / Spec 은 다음 시험(YMS 적재 등)에 다시 쓸 수 있으므로 남겨도 된다. 지우려면 E-Test Para 설정 → 전체 선택 → [선택 삭제](값이 있으면 파일을 먼저 지움).
- 확인 칸에 ☐ → ☑, 다르면 실제 값과 화면 캡처 경로를 적고 Claude 에게 전달한다.
- 자동 검사 결과(2026-10-15 15:16, CLAUDE-116 Map 필수 / 정보 표 수정): `ET_UPLOAD_FORM_SMOKE=PASS` 39(업로드 + 조회 화면) / `ET_RESULT_PROBE=PASS` 18 / `DIE_COORD_PROBE=PASS` 11. 14:51 CLAUDE-114: Smoke 35. 14:30 CLAUDE-113: `ET_UPLOAD_FORM_SMOKE=PASS` 34 / `ET_RESULT_PROBE=PASS` 18 / `DIE_COORD_PROBE=PASS` 11. 자동 검사 결과(2026-10-14 14:58, CLAUDE-096 TEMP 빌드): `ET_PARA_FORM_SMOKE=PASS` 41(등록 화면 + 설정 화면) / `ET_UPLOAD_FORM_SMOKE=PASS` 30 + `DIE_COORD_PROBE=PASS` 11. 14:20 CLAUDE-095: `ET_PARA_SETUP_PROBE=PASS` 26 / `ET_RESULT_PROBE=PASS` 18 / `ET_UPLOAD_FORM_SMOKE=PASS` 30 + `DIE_COORD_PROBE=PASS` 11. 그 전(CLAUDE-094): `KLARF_UPLOAD_FORM_SMOKE=PASS`(읽기만) / `GDS_KLARF_FORM_SMOKE=PASS` 35 / KLARF 조회 / `STANDARD_MAP_VIEW_SMOKE=PASS` / `KLARF_FORM_SMOKE=PASS` / `GDS_SETUP_LAYOUT_SMOKE=PASS` 4 / `GDS_VIEW_LAYOUT_SMOKE=PASS` 26. 화면 밖 자동 검사이며 DMS 화면 직접 확인은 이 시나리오로 한다.
