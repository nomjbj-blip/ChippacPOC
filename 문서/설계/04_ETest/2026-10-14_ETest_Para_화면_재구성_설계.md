# E-Test Para 설정 화면 재구성 설계 (Spec 통합 / GDS Chain 직접 입력 / 대표 Chain 삭제)

작성: 2026-10-14 Claude (CLAUDE-095) / 상태: **결정 확정 — 구현 완료(빌드 / Probe / Smoke 확인, 14:21). 배포 · DMS 화면 확인 대기**
값 통계 열 삭제(같은 날 CLAUDE-096 추가 요청): 3.3절의 파일 수 ~ Fail 열과 서버 계산(SELECT_ET_PARA_SETUP_LIST 의 값 CTE)은 지웠다.
화면 나눔(같은 날 CLAUDE-096): 이 문서의 [파일 헤더] 탭은 **E-Test Para 등록** 화면, [등록 Parameter] 탭은 **E-Test Para 설정** 화면으로 나뉘었다(3.1절 위 줄의 파일 열기 / 닫기 · 콤보 잠금은 없어짐). [화면 분리 설계](2026-10-14_ETest_Para_등록_설정_화면_분리_설계.md).
기존 설계: [E-Test Para Setup 설계](2026-10-14_ETest_Para_Setup_설계.md) 6·7·8.4절(대표 Chain / GDS 연결)과 [Limit 설정 설계](2026-10-07_ETest_Limit_설정_설계.md)의 화면 부분을 이 문서가 대체한다. Parameter 등록 규칙(5절 이름 변환)과 Spec 판정 규칙(Upload 재설계 9.2절)은 그대로다.

## 1. 요청과 해석

| 번호 | 요청 (2026-10-14) | 해석 |
| --- | --- | --- |
| 1 | 파일 헤더 탭 = Parsing 결과와 등록된 Para 를 보는 탭. 최상단 [등록] 버튼을 이 탭 위로 | 파일 헤더 탭에 [등록] 을 옮긴다. 헤더 행에 등록된 Parameter 의 GDS Chain / Spec 도 읽기 전용으로 보인다 |
| 2 | 등록 Parameter 탭에 대표 Chain 입력 기능: 더블클릭해서 GDS Chain 을 사용자가 직접 입력. 대표 Chain 기능은 없어짐. Spec 도 이 탭에서 더블클릭으로 수정. 수정된 행만 저장 | 등록 Parameter 탭 = **Parameter 한 행에 사용 / GDS Chain / Spec(Lower · Upper · Enable) / 비고를 함께 편집**하는 표. 수정 칸 노란색, 저장 = 수정된 행만. [대표 Chain] 탭과 대표 Chain / GDS 연결 테이블은 없앤다 |
| 3 | 최상단 Device / 공정은 파일을 연 상태에서만 읽기 전용, 풀리면 골라서 DB 조회 | 파일을 열면 파일명 값으로 고정(읽기 전용), [파일 닫기] 하면 콤보에서 골라 바로 DB 조회 |
| 4 | E-Test setup 화면 기능이 2번으로 옮겨짐 | **E-Test Limit 설정**(Spec) 화면의 기능(조회 / 수정 / 삭제 / Spec 표 붙여넣기 / 거르기 / 값 통계 / 다시 판정)을 등록 Parameter 탭으로 옮기고 Limit 설정 메뉴 / 화면은 없앤다 |
| 5 | POC 는 최대한 단순하게 | 화면 1개 / 탭 2개 / 저장 1개. 자동 묶기 · 자동 채움 없이 사람이 입력한다 |

## 2. 결정 (2026-10-14 사용자 답)

| 번호 | 내용 | 결정 |
| --- | --- | --- |
| D1 | GDS Chain 칸 수 | **Chain 1(첫 번째 Chain) 기준 1칸**. Chain 이 2개인 Leakage 등도 Chain 1 의 GDS Chain 을 넣는다 |
| D2 | GDS Chain 값 / 입력 도움 | **등록 Parameter 탭 오른쪽에 지금 Device(의 GDS 대표 Device)에 등록된 GDS Setup Chain 목록을 보이고 이름을 복사**할 수 있게 한다. 값은 이름 문자열로 저장, 목록에 없는 이름은 주황색으로만 알림 |
| D3 | 대표 Chain 테이블 / 서비스 | **삭제** (`TQP_GDS_ET_REP_CHAIN` / `TQP_GDS_ET_CHAIN_LINK` DROP, 서비스 11개 / XML / DSL 삭제) |
| D4 | E-Test Limit 설정 화면 | **삭제** (메뉴 행 + 화면 소스 + Smoke) |
| D5 | 값 통계 / 참고 정보 | **표 열로 모두 보인다** (값 수 / '-' 수 / 최소 / 중앙값 / 99% / 최대 / Fail, 측정 / 조건 / Spec 원문 / 등록 파일). 툴팁에 숨기지 않는다 → **2026-10-14 사용자 요청으로 값 통계 열(파일 수 / 값 수 / '-' 수 / 최소 / 중앙값 / 99% / 최대 / Fail) 삭제**("setup 화면에서 필요 없는 기능"). 참고 정보 열은 그대로 |
| D6 | Device / 공정 콤보 | 권장안: **고르면 바로 조회**(수정 중이면 먼저 묻기), 직접 입력 허용 |
| D7 | 편집 방법 | 권장안: **글자 칸 더블클릭(F2), 체크 칸(사용 / Enable) 클릭** |
| D8 | 같은 Chain 행 한꺼번에 입력 | **넣지 않는다** (우선 제외) |
| D9 | [Spec 표로 고르기] (파일 헤더 탭) | **제거**. 등록할 헤더는 사람이 체크한다 |

## 3. 화면 설계

### 3.1 위 조건 줄

```text
┌ Factory [SCK ]  Device [OMM      ▼]  공정 [TSVET ▼]  [조회] │ [결과 파일 열기...] [파일 닫기] ───────────────┐
│ 파일: CAI_OMM_ATV_A0_TSVET_..._RH-HS2615-02.xlsx (Device / 공정은 파일명 값으로 고정)                        │
```

| 상태 | Device / 공정 | 동작 |
| --- | --- | --- |
| 파일 없음 | 콤보 사용 가능. Device 목록 = 그 Factory 에서 등록 Parameter 가 있는 Device (`GetEtParaProgramList`), 공정 목록 = 그 Device 의 공정. 직접 입력도 됨(새 Device 에 Spec 표를 붙여넣을 때) | 콤보를 고르면 바로 조회. [조회] = 입력한 값으로 다시 읽기 |
| 파일 열림 | 파일명의 Device / Test Type 으로 채우고 **읽기 전용**(콤보 잠금). 파일 이름을 안내 줄에 표시 | [파일 닫기] = 파일 헤더 탭을 비우고 콤보를 다시 연다(등록 Parameter 탭 내용은 유지) |

- 저장하지 않은 수정이 있는 상태에서 Device / 공정을 바꾸거나 [조회] / [결과 파일 열기] 하면 "수정 n행을 버릴까요?" 를 묻는다.
- 위 줄 버튼은 조건 / 파일 관련만 둔다. 등록 / 저장 / 삭제 / 붙여넣기는 각 탭 위로 옮긴다.

### 3.2 [파일 헤더] 탭 — Parsing 결과 + 등록 여부 (파일을 열었을 때만 내용)

```text
측정 [전체▼] Chain 종류 [전체▼] 찾기 [        ] □신규만 │ [보이는 신규 선택] [선택 해제] [등록]
헤더 2755 / 신규 2718 / 등록됨 37 / 등록 불가 0 / 목록에서 뺌 0
열 │☑│상태  │Parameter(헤더 원문)                 │저장 이름│측정│조건│Chain 1│Leg│Chain 2│Leg│단위│Test No│GDS Chain   │Spec     │메시지
 9 │ │등록됨│Resistance_iforce=8.0mA@TSVDC_TR_FH    │...      │    │    │       │   │       │   │Ohm │       │TSVDC_TR_FH │~ 715    │
```

- 지금 표(행 머리글 = 열, 드래그 / 우클릭 / Del 로 목록에서 빼기, 되살리기, 거르기, 체크)를 그대로 쓰고 **[등록] 을 이 탭 위로** 옮긴다. [Spec 표로 고르기] 는 없앤다(D9).
- [대표 Chain] 열 대신 **GDS Chain / Spec(읽기 전용)** 열: 등록된 헤더는 등록 Parameter 탭의 값을 보인다.
- 파일이 없으면 표는 비우고 "[결과 파일 열기] 로 파일을 열면 헤더와 등록 여부가 나옵니다" 안내.

### 3.3 [등록 Parameter] 탭 — 등록 Parameter + GDS Chain + Spec 편집 (Limit 설정 화면 대체)

```text
찾기 [        ] □Spec 있는 것만 □GDS Chain 없는 것만 │ [Spec 표 붙여넣기] [선택 삭제] │ [저장]      │ GDS Device [U9 ▼]
등록 37 / 사용 37 / GDS Chain 2 (목록에 없음 0) / Spec 37 / 데이터 있음 37 / Fail 있는 Parameter 6 / 수정 2행 (저장 전)
  │Parameter(헤더 원문)│저장 이름│측정│조건│Chain 1│Chain 2│단위│사용│GDS Chain ✎│Lower ✎│Upper ✎│Enable│ ┃ GDS Setup Chain (U9) 15
  │                    │         │    │    │       │       │    │    │           │       │       │      │ ┃ Code│Chain 이름      │사용 수
* │Resistance_..._R_N_FH│...     │    │    │R_N_FH │       │Ohm │ ☑ │TSVDC_R_N_FH│      │[720]  │ ☑    │ ┃ N   │TSVDC_TR_FH     │ 1
  │ ... 파일 수 / 값 수 / '-' 수 / 최소 / 중앙값 / 99% / 최대 / Fail / 비고 ✎ / Spec 원문 / 출처 / 등록 파일 / 수정  │ ┃ S   │TSVDC_ASIC1_P_FH│ 0
                                                                                                              ┃ (더블클릭 / Ctrl+C = 이름 복사)
```

| 항목 | 내용 |
| --- | --- |
| 목록 | Device + 공정의 등록 Parameter 전체(2,755 행도 가능). 열: 헤더 원문 / 저장 이름 / 측정 / 조건 / Chain 1 / Chain 2 / 단위 / **사용** / **GDS Chain** / **Lower** / **Upper** / **Enable** / **비고** / Spec 원문 / 출처 / 등록 파일 / 수정(사람 · 시각). 굵은 열만 편집(D5: 모두 표 열) |
| 편집 | 글자 칸(GDS Chain / Lower / Upper / 비고)은 **더블클릭(또는 F2)으로 편집**, 체크 칸(사용 / Enable)은 클릭(D7). 편집 칸에서는 Ctrl+V 로 복사한 이름을 붙여넣는다. 사용 안 함(N) 행은 회색 글자, 편집은 됨 |
| 수정 표시 | 행 Tag 에 조회 당시 값을 두고 셀 값이 바뀌면 그 칸 노란색 + 행 머리글 `*`, 탭 정보 줄에 "수정 n행 (저장 전)". 원래 값으로 되돌리면 표시도 사라진다 |
| 입력 확인 | Lower / Upper 는 숫자, Lower ≤ Upper, Enable 이면 Lower / Upper 중 하나는 있어야 함 -> 어긋나면 그 칸 붉은색(저장 거부). Spec 이 없던 행에 Lower / Upper 를 처음 넣으면 Enable 자동 체크(한 번). GDS Chain 은 앞뒤 공백 제거, 200자 이하 |
| GDS Chain (D1 / D2) | Chain 1 기준 1칸. 오른쪽 **GDS Setup Chain 목록**(GDS Device 콤보 = E-Test Device 의 대표 Device, Sub Device 연결): Chain 이름 / 사용 수(지금 표에서 그 이름을 쓰는 Parameter 수). Code 열은 숨김(2026-10-14 사용자 요청). 목록 행 더블클릭 또는 Ctrl+C = 이름을 클립보드로 복사. 표의 GDS Chain 이 목록에 없으면 주황 글자(저장은 됨) |
| [저장] | **수정된 행만** 서버로 보낸다(`SaveEtParameterSetup`). Lower / Upper 를 모두 지운 행은 Spec 삭제(저장 전에 한 번 묻는다). 저장 뒤 다시 조회. Spec 이 바뀌었고 그 Device + 공정에 READY 업로드 파일이 있으면 "다시 판정할까요?"(지금 Limit 화면과 같음) |
| 우클릭 | [선택 행 Spec 비우기](Lower / Upper / Enable 을 비워 수정 표시 -> 저장 때 Spec 삭제) / [선택 행 수정 취소(원래 값)] |
| [Spec 표 붙여넣기] | 지금 Limit 화면과 같다: 고객 표(클립보드) -> 공정별 건수 / 화면 Device / 새로 등록할 수 질문 -> 없는 Parameter 는 출처 SPEC 으로 등록 -> Spec 저장(Enable Y) -> 다시 판정 질문. 저장 안 한 수정이 있으면 버릴지 먼저 묻는다 |
| [선택 삭제] | Parameter 삭제(Spec 같이). 값이 저장된 Parameter 는 서버가 거부(지금과 같음). 저장 안 한 수정이 있으면 먼저 묻는다 |
| 거르기 | 찾기(헤더 원문 / 저장 이름 / Chain / GDS Chain 글자) / Spec 있는 것만 / GDS Chain 없는 것만. 다시 조회하지 않는다 |
| 진행 표시 | 조회 / 저장 / 붙여넣기 / 삭제 / 다시 판정은 화면 아래 진행 표시줄(CLAUDE-094) 그대로 |

### 3.4 화면에서 없어지는 것

- [대표 Chain] 탭 전체(E-Test Chain / 대표 Chain / GDS Setup Chain 3목록, 묶기 · 빼기 · 이름 바꾸기 · 삭제, GDS 이름 / 선택 연결 · 해제, 대표 선택 시 Parameter / Spec).
- 위 줄의 [Spec 표로 고르기] / [등록] / [저장] / [선택 삭제] ([등록] / [저장] / [선택 삭제] 는 각 탭으로 이동, [Spec 표로 고르기] 는 삭제).
- POC 메뉴 **E-Test Limit 설정**(`MNU_DMS_ET_LIMIT`, `frmEtLimitSetup`).

## 4. 데이터 / 서버 변경

### 4.1 DDL (`GdsMapControl/Persistence/2026-10-14_tqp_gds_et_para_gds_chain.sql`)

| 순서 | 내용 |
| --- | --- |
| 1 | `TQP_GDS_ET_ITEM ADD GDS_CHAIN_NAME VARCHAR2(200 CHAR) NULL` (Chain 1 의 GDS Setup Chain 이름, `TQP_GDS_CHAIN_VER.CHAIN_NAME` 과 같은 길이) + 주석 |
| 2 | 대표 Chain 데이터가 남아 있으면 옮긴다: Chain 1 이 든 대표의 연결 GDS Chain 이름 -> `GDS_CHAIN_NAME` (로컬은 0행) |
| 3 | `DROP TABLE TQP_GDS_ET_CHAIN_LINK`, `DROP TABLE TQP_GDS_ET_REP_CHAIN` (PURGE 없이). 앞뒤 행 수 출력 |

`TQP_GDS_ET_LIMIT` / `TQP_GDS_ET_ITEM` 의 다른 컬럼, FK(Spec -> 등록 Parameter)는 그대로다.

### 4.2 쿼리 / 서비스 (`iGdsEtResult`)

| 구분 | 메서드 | 내용 |
| --- | --- | --- |
| 신규 | `GetEtParameterSetupList(factory, device, program)` | 등록 Parameter + GDS_CHAIN_NAME + Spec + READY 파일 값 통계(파일 수 / 값 수 / '-' 수 / 최소 / 중앙값 / 99% / 최대 / Fail = 지금 Spec 으로 계산). 지금 `SELECT_ET_PARA_LIST` + `SELECT_ET_LIMIT_LIST` 를 합친 쿼리 1개(대표 Chain 조인 제거) (값 통계는 같은 날 삭제) |
| 신규 | `SaveEtParameterSetup(aKey{factory, device, 공정}, aRows[n, 7]{저장 이름, 사용, 비고, GDS Chain, Lower, Upper, Enable}, user)` | 수정된 행만 받는다. 처리: 값 확인(모두 등록된 Parameter / 같은 행 두 번 / 숫자 / Lower ≤ Upper / Enable 이면 Limit 1개 이상) -> 하나라도 틀리면 아무것도 저장 안 함 -> 배열 UPDATE(사용 / 비고 / GDS Chain) -> Limit 있는 행 배열 MERGE(Spec, 단위 = 기존 Spec 단위 또는 등록 단위, Spec 원문 / Spec 비고 유지) -> Limit 모두 빈 행 배열 DELETE. 반환 1행: UPDATED / SPEC_SAVED / SPEC_DELETED |
| 신규 | `GetEtGdsChainList(factory, gdsDevice)` | 오른쪽 목록: GDS Device 의 Setup Chain(ACTIVE + 현재 READY 버전) CHAIN_SEQ / CHAIN_CODE / CHAIN_NAME |
| 신규 | `GetEtGdsDevice(factory, device)` | E-Test Device 의 GDS 대표 Device(Sub Device 연결 -> 같은 이름 READY Map -> 그대로). 화면 GDS Device 콤보 기본값 |
| 변경 | `GetEtParameterList` | 대표 Chain 조인 제거, `GDS_CHAIN_NAME` = ITEM 컬럼(업로드 계획 / Fail Chain 열이 쓴다). REP_CHAIN_NAME / 2, GDS_CHAIN_NAME2 열 삭제 |
| 변경 | `GetEtChainSpecInfo(factory, device, gdsChainName)` (인계 API) | GDS Chain 이름 -> 그 이름을 GDS Chain 으로 넣은 모든 공정 등록 Parameter + Spec. device 는 E-Test Device 또는 GDS Device(Sub Device 로 찾음). 열에서 REP_CHAIN_NAME / MATCH_POSITION 삭제 |
| 유지 | `RegisterEtParameters` / `DeleteEtParameters`(묶음) / `SaveEtLimit`(붙여넣기) / `GetEtLimit` / `RejudgeEtFiles` / `GetEtParaProgramList` | 그대로 |
| 삭제 | `SaveEtParameters`(-> SaveEtParameterSetup), `GetEtLimitList`(Limit 화면 전용), 대표 Chain / 연결 11개(`GetEtChainList` / `GetEtRepChainList` / `GetEtGdsChainLinkList` / `SaveEtRepChain` / `RenameEtRepChain` / `RemoveEtRepChainMembers` / `DeleteEtRepChain` / `LinkEtRepChainsByName` / `SaveEtChainLink` / `DeleteEtChainLink` / `GetEtRepChainSpecInfo`) | XML `TQP_GDS_ET_REP_CHAIN.xml` / `TQP_GDS_ET_CHAIN_LINK.xml`, DSL 2개(csproj Compile 행) 삭제 |

### 4.3 다른 화면 / 검사 영향

| 대상 | 변경 |
| --- | --- |
| E-Test 결과 업로드 | Fail Chain 패널의 [대표 Chain] 열 삭제, [GDS Chain] = Parameter 의 GDS_CHAIN_NAME. `EtUploadPlan.Param.Rep` 삭제 |
| E-Test Limit 설정 | 메뉴 행 삭제(`01_WinApp/DACruxV5/MenuFile.mnu` + `100_ClientBin/MenuFile.mnu`, 다른 행 순서는 그대로), `frmEtLimitSetup`(.cs / .Designer.cs) ENGUI csproj 에서 제외 + 파일 삭제. `EtSpecTable.cs` 는 붙여넣기에서 계속 쓴다 |
| GDS / 연관 분석 화면 | 영향 없음(대표 Chain / 연결 테이블을 쓰지 않음, 2026-10-14 검색 확인) |
| Smoke / Probe | `EtParaSetupFormSmoke` 다시 작성(GDS Chain 셀 입력 / 오른쪽 목록 복사 / Spec 셀 편집 / 수정 행만 저장 / 붙여넣기 / 다시 판정 / Device · 공정 콤보), `EtParaSetupProbe` 대표 Chain 검사 -> GDS Chain 저장 / 인계 API 검사, `EtLimitSetupFormSmoke.cs` + `Verify-EtLimitSetup.ps1` 삭제(검사 항목은 Para Smoke 로 옮김). `EtResultUploadFormSmoke` 는 대표 Chain 열을 보지 않아 고칠 것 없음(구현 때 확인) |
| 문서 | [테스트 시나리오](2026-10-14_ETest_Para_Spec_Upload_테스트_시나리오.md) 2·3절을 새 화면 기준으로, F07 / 빌드 검증 가이드 / 메뉴 표 |

## 5. 구현 단계

| 단계 | 내용 | 확인 |
| --- | --- | --- |
| 1 | DDL(로컬 적용) + 쿼리 / 서비스(신규 4 / 변경 2 / 삭제) + Probe | `EtParaSetupProbe` (GDS Chain 저장 · 수정 행만 저장 · 잘못된 값 전체 거부 · Spec MERGE / 삭제 · 인계 API), 업로드 Probe 회귀 -> **완료**: `ET_PARA_SETUP_PROBE=PASS` 26/26, `ET_RESULT_PROBE=PASS` 18/18 |
| 2 | Para 화면 재구성(위 줄 / 파일 헤더 탭 / 등록 Parameter 탭 편집 · 수정 표시 · 저장 · 붙여넣기 · 삭제 · 다시 판정 · 오른쪽 GDS 목록 / 대표 Chain 탭 삭제) | `Verify-EtParaSetup.ps1` 다시 작성 -> **완료**: `ET_PARA_FORM_SMOKE=PASS` 37/37 |
| 3 | Limit 설정 메뉴 / 화면 삭제, 업로드 Fail Chain 열 정리, 문서 / 시나리오 갱신 | `Verify-EtResultUpload.ps1`, 메뉴 파일 XML 읽기 확인 -> **완료**: `ET_UPLOAD_FORM_SMOKE=PASS` 30 + `DIE_COORD_PROBE=PASS` 11, 메뉴 2곳 XML 파싱 확인 |

## 6. 검증 기준 (기대값, 샘플 TSVET + 고객 Spec 표)

| 검사 | 기대값 |
| --- | --- |
| 위 줄 | 파일 없음 -> Device 콤보에 등록 Device, 고르면 조회 / 파일 열기 -> Device OMM · 공정 TSVET 잠금 / [파일 닫기] -> 다시 콤보 |
| 파일 헤더 탭 | [등록] 이 탭 위에 있음, [Spec 표로 고르기] 없음, 등록됨 행에 GDS Chain / Spec 표시 |
| 셀 편집 | GDS Chain 입력 -> 노란색 + `*` + "수정 1행", 원래 값으로 되돌리면 표시 없어짐 / Lower > Upper -> 붉은색 + 저장 거부 / Spec 없던 행에 Upper -> Enable 자동 |
| 저장 | 수정 2행만 서버로(나머지 행 UPDATE_TIME 그대로) / Lower · Upper 모두 지운 행 -> 질문 -> Spec 삭제 / READY 파일 있으면 다시 판정 질문 -> #44 Fail 6 -> 4 (R_N Enable 해제 + L_P Upper 720) |
| GDS Chain | 오른쪽 목록 = GDS Device 의 Setup Chain, 더블클릭 -> 클립보드에 이름 / 목록에 있는 이름 -> 보통 글자, 없는 이름 -> 주황 / 인계 API `GetEtChainSpecInfo(PROBE, 시험 Device, 소문자 GDS Chain 이름)` -> 그 GDS Chain 을 넣은 모든 공정 Parameter + Spec |
| 붙여넣기 | 고객 표 119 -> 새로 등록 82(TSVET 37 이 이미 있으면) / Spec 119, 화면 Device 기준 |
| 삭제된 것 | [대표 Chain] 탭 없음, POC 메뉴에 E-Test Limit 설정 없음, `TQP_GDS_ET_REP_CHAIN` / `_CHAIN_LINK` 없음 |

## 7. 확인 수준

- 소스 확인: `frmEtParaSetup`(.cs / .Designer.cs), `frmEtLimitSetup`, `SELECT_ET_PARA_LIST` / `SELECT_ET_LIMIT_LIST`, 대표 Chain / 연결 사용처 검색(E-Test 화면 / 서비스 / Probe 만, GDS · 연관 분석 화면은 사용 안 함), `TQP_GDS_CHAIN_VER.CHAIN_NAME` 200 CHAR.
- DB 확인(로컬, 조회만): SCK E-Test 등록 Parameter / Spec / 대표 Chain 0행(12:04), SCK U9 GDS Setup Chain 15개.
- 구현 확인(14:21, TEMP 빌드 / 화면 밖 / 로컬 DB): 빌드 오류 0 · 새 경고 0 / Probe · Smoke 위 5절 결과 / 소스 AI 흔적 0건. 배포 안 함, DMS 화면 직접 확인과 Service Remoting 경유는 미확인.
- DDL 적용 때 사용자가 13:12 에 만든 SCK / U9 대표 `PIC_SERP1_M2_FL`(Chain 6, GDS 연결 없음)이 테이블과 함께 지워졌다(휴지통에 남음). SCK OMM / TSVET 등록 Parameter 22개는 그대로.

### 7.1 구현하면서 정한 세부 동작

| 항목 | 동작 |
| --- | --- |
| Spec 저장 수 | 서버가 저장된 Spec 과 비교해 Lower / Upper(숫자 비교) / Enable 이 바뀐 행만 MERGE 한다. GDS Chain / 사용 / 비고만 고친 행은 Spec 수정자 · 시각이 그대로라 "Spec 저장" 수에 들어가지 않는다 |
| Limit 을 모두 지움 | Enable 이 자동으로 꺼진다(그래서 "Enable 인데 Limit 없음" 오류가 아니라 Spec 삭제로 처리). Enable 을 직접 켠 채 Limit 이 없으면 Enable 칸이 붉은색 |
| 오류 칸 | 숫자 아님 = 그 칸, Lower > Upper = Lower 칸, Enable 인데 Limit 없음 = Enable 칸, 200자 초과 = 그 칸. 행 머리글 풍선 도움말에 오류 문구 |
| 키 | 글자 칸 F2 = 편집, Ctrl+V = 지금 칸에 클립보드 첫 칸(탭 / 줄바꿈 앞) 넣기, 체크 칸 Space = 바꾸기. 오른쪽 목록 Ctrl+C 는 행 전체가 아니라 Chain 이름만 복사 |
| 고정 열 / 오른쪽 폭 | 헤더 원문 열만 고정(저장 이름까지 고정하면 편집 열이 안 보임), 오른쪽 GDS 목록 약 350px |
| 질문 순서 | [Spec 표 붙여넣기]: 표 해석 -> 저장 안 한 수정 버릴지 -> 저장 질문 -> 다시 판정. [등록] / [선택 삭제]: 저장 안 한 수정이 있으면 확인 질문에 "n행은 버려집니다" 를 붙인다 |
| 수정 행 표시 | 거르기(찾기 / Spec 있는 것만 / GDS Chain 없는 것만)를 해도 수정한 행은 늘 보인다 |
| GDS Device 바꾸기 | 콤보를 바꾸면 목록만 다시 읽고 표 값은 그대로, 주황 표시 / 사용 수만 다시 계산 |
