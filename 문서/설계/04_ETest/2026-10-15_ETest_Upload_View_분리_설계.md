# E-Test 결과 Upload / View 화면 분리 + 공통 결과 Control 설계

작성: 2026-10-15 Claude (CLAUDE-113) / 상태: **9절 권장안 확정 (V1~V6) -> 구현 완료 (코드 / TEMP 빌드 / Smoke · Probe PASS, 배포 안 함 / DMS 화면 확인 대기)** — 구현 결과는 11절. **2026-10-15 업로드는 파일 하나씩으로 변경 (12절, 4절 대기 목록 / 저장 확인 표는 12절로 대체). Map 필수 · [Map 좌표 다시 계산] 삭제 · 정보 표 축소 / 수정 (13절)**

사용자 지시 (2026-10-15):
> 현재 frmEtResultUpload 화면에서 Upload 및 조회를 다 하고 있는데 Upload / View 나눠서 볼 수 있도록 나눠줘.
> 결과 정보는 동일하게 보여지도록 해줘 (분석해서 나눠진 Data) - 하나의 Control 에서 관리해서 하는 것도 방법이다. 동일한 결과 Data 를 보기 때문에 One 소스 관리.
> 결과 Data 는 현재와 비슷한 컨셉으로 1단계 DUT, 좌표, 결과, 정보들이 보이고 2단계로 정의된 Para 에 대해서만 Filter 되어 Data 가 보인다.
> Upload / View 전부 정의된 Para 에 대해서만 저장하고 몇 개가 저장되는지 확인 한번 시켜주고 저장한다.
> 추가된 정보를 보려면 다시 Upload 해야 한다는 컨셉으로 진행한다.

관련 설계: [Upload 재설계](2026-10-13_ETest_Upload_재설계.md) (9·10절 구현됨) / [Para 등록·설정 분리](2026-10-14_ETest_Para_등록_설정_화면_분리_설계.md) / [YMS 적재](2026-10-14_ETest_Upload_YMS적재_설계.md)

---

## 1. 이해한 컨셉

| 번호 | 컨셉 |
| --- | --- |
| C1 | **Upload 화면**은 파일을 올리는 일만 한다 (파일 추가 -> 분석 -> 결과 확인 -> 저장 수 확인 -> 저장). 등록된 파일 목록 / 조회는 하지 않는다 |
| C2 | **View 화면**은 저장된 결과를 찾아 보는 일만 한다 (Device / 공정 -> 파일 목록 -> 결과) |
| C3 | 두 화면의 결과는 **같은 Control 하나**가 같은 모양으로 보여 준다. 다른 것은 데이터 원천뿐이다 (Upload = 분석한 메모리 데이터 / View = DB) |
| C4 | 결과는 **1단계 = Die (DUT / 좌표 / 판정 / 정보)**, **2단계 = 정의된 Para 의 값만** (1단계에서 고른 Die 로 거름) |
| C5 | **정의된 Para = 등록 Parameter(사용 Y)** (Para 등록 / 설정 화면). 저장도 표시도 이것만 한다 |
| C6 | 저장 전에 **"몇 개가 저장되는지"** (파일별 Parameter 수 x Die 수 = 값 수) 를 한 번 보여 주고 확인받는다 |
| C7 | 나중에 Para 를 더 등록해도 이미 올린 파일에는 그 값이 없다. **보려면 다시 Upload** 한다. View 는 "추가 Para n개 — 다시 Upload 필요" 를 보여 준다 |

## 2. 현재 상태 (2026-10-15 기준)

| 구분 | 현재 |
| --- | --- |
| `frmEtResultUpload` | 탭 [업로드] + [등록 파일]. [업로드] = 대기 목록(여러 파일, 18열) + 아래 3칸(기준 정보 / Die 판정 8열 / Fail Chain). [등록 파일] = Device 목록 / 삭제 / Map 좌표 다시 계산 / 더블클릭 -> 조회 창 |
| `frmEtResultView` | 메뉴 없는 팝업 Form (등록 파일 더블클릭으로만 열림). 정보 2줄 + 왼쪽 Die 표(8열) + 오른쪽 Parameter x Die 표 + 필터 4개 |
| 저장 대상 | 이미 **등록 Parameter(사용 Y)만** (화면 `EtUploadPlan` + 서버 BeginEtFile / CreateEtValueBatch 가 거부) |
| 저장 전 확인 | 있음. 문구는 "파일 n개를 저장합니다 (경고 n개 포함) / 오류 n개 제외" + 중복이면 덮어쓰기 / 건너뛰기 — **저장 개수(Parameter / 값)는 안 보임** |
| 결과 표시 | 두 곳이 따로 만들어짐 (Upload = 메모리 Die 표 + Fail 목록 / View = DB Parameter x Die 표), 열 이름도 다름 |
| 서버 | 저장 4단계 + 완료 때 판정 / Map 좌표 / YMS 적재, 다시 판정 / 다시 계산 — **화면을 나누는 데 서버 / DDL 변경은 거의 없다** |

## 3. 공통 결과 Control

### 3.1 구성

```text
EtResultData (데이터 모델, 화면과 무관)
  ├─ Info   : 항목 / 값 목록 (기준 정보 + 개수 + 안내)
  ├─ Dies   : DUT / X / Y / Map X·Y / 판정 / Fail 수 / '-' 수 / DUT Flag
  ├─ Params : 저장 이름 / 헤더 원문 / Chain / GDS Chain / 단위 / Lower / Upper / Spec 있음 / Enable
  └─ Values : (Die, Param) -> 값 / 상태(V·N·E) / 판정(P·F·없음)

변환기 2개 (같은 EtResultData 를 만든다)
  ├─ EtResultData.FromUpload(EtResultFile, EtUploadPlan, slot)   ← Upload 화면 (저장 전, 미리보기 판정 EtJudgeRule)
  └─ EtResultData.FromDb(file, dies, values, params)             ← View 화면 (저장된 판정)

EtResultDataControl (UserControl) : ShowData(EtResultData) / Clear()
```

- 파일: 신규 `ENGUI/ETest/EtResultData.cs` (모델 + 변환기) / `EtResultDataControl.cs` + `.Designer.cs`.
- 판정 규칙은 지금처럼 `EtJudgeRule`(화면) = 서버 판정 SQL 과 같다. Control 은 판정을 계산하지 않고 받은 값만 그린다.

### 3.2 화면 모양

```text
┌ 정보 ─────────────┐┌ 1단계: Die ───────────────────────────────────────┐
│ 항목 │ 값          ││ DUT │ X │ Y │ Map(X,Y) │ 판정 │ Fail 수 │ '-' 수 │ DUT Flag(참고) │
│ Wafer / Lot / Slot ││ (Fail 행 빨간색, 여러 행 선택 가능)                 │
│ Device / 공정 / Date│└──────────────────────────────────────────────────┘
│ 헤더 / 등록 / 저장 ││┌ 2단계: 정의 Para 값 ─ [Fail 만] ['-' 만] [찾기____] ┐
│ 누락 / 건너뜀       │││ DUT │ (X,Y) │ Parameter │ 헤더 원문 │ Chain │ GDS Chain │ 단위 │ 값 │ Lower │ Upper │ 판정 │ 사유 │
│ Pass / Fail Die    │││ (1단계에서 고른 Die 만, 고르지 않으면 전체 Die)       │
│ 안내 (추가 Para 등) │││ (판정 F 빨간색, '-' 회색)                           │
└────────────────────┘└──────────────────────────────────────────────────┘
```

- **정보**는 항목 / 값 표로 보인다(툴팁 없음). Upload 와 View 가 같은 항목을 같은 순서로 보인다. View 만 있는 항목(파일 번호 / 판정 시각 / Map / YMS / 등록자)은 Upload 에서 빈 값.
- **1단계 Die**: 지금 Die 판정 표와 같은 8열. 행을 고르면 2단계가 그 Die 로 거른다.
- **2단계 정의 Para 값**: 지금의 Fail Chain 목록(Upload)과 Parameter x Die 표(View)를 **긴 표 하나**로 합친다. 한 행 = Die 1개 x Parameter 1개. 정의 Para 만 있으므로 TSVET 샘플 전체 Die = 888행, Die 1개 = 37행.
- 사유: '-' / 범위 밖(Lower / Upper) / 빈 칸.

### 3.3 Lower / Upper 표시 기준

- 지금 Spec 값을 보인다 (판정 당시 값은 저장하지 않음 — DB 변경 없음).
- View 에서 **Spec 수정 시각 > 판정 시각** 이면 정보 표 안내: "Spec 이 판정 뒤 바뀜 — Para 설정 화면에서 다시 판정". 셀 색(저장된 판정)과 Lower / Upper 가 어긋나는 이유를 알 수 있게 한다.

## 4. Upload 화면 (`frmEtResultUpload` 정리)

| 영역 | 변경 |
| --- | --- |
| 위 / 대기 목록 | 그대로 (파일 추가 / 폴더 추가 / 목록 비우기 / 모두 저장 / 선택 저장, 대기 목록 18열, Slot 수정) |
| 아래 | 기준 정보 / Die 판정 / Fail Chain 3칸 -> **공통 Control 하나** (대기 목록에서 고른 파일) |
| [등록 파일] 탭 | **제거** (View 화면으로 이동) |
| 저장 확인 | 저장 개수를 보인다 (아래) |

저장 확인 문구 (C6):

```text
E-Test 결과 파일 3개를 저장합니다. 등록 Parameter(사용 Y)만 저장합니다.

  파일                                        공정   Parameter   Die   값
  CAI_OMM_..._RH-HS2615-02.xlsx               TSVET     37        24    888
  CAI_OMM_..._RH-HS2615-03.xlsx               TSVET     37        24    888
  CAI_OMM_..._HS2615-02.xlsx                  COWET     40        24    960
  합계                                                          72  2,736

등록되지 않은 헤더는 저장하지 않습니다 (파일마다 약 2,700개).
나중에 등록한 Parameter 는 이 파일을 다시 Upload 해야 보입니다.
경고 1개 포함 / 오류 1개 제외
중복 1개: 예 = 기존 파일 지우고 다시 저장 / 아니요 = 건너뛰기 / 취소 = 저장 안 함
```

- 파일이 많으면 10줄까지 보이고 "외 n개" + 합계.
- 숫자는 분석 결과(`EtUploadPlan.Matched` 수 x Die 수)로 계산한다. 서버 저장 결과와 다르면 지금처럼 MISMATCH 로 실패한다.

## 5. View 화면 (`frmEtResultView` 를 메뉴 화면으로 다시 작성)

| 영역 | 내용 |
| --- | --- |
| 위 | Factory / Device / 공정(전체 · TSVET · COWET · FT) / Lot 찾기 / [조회] / [선택 파일 삭제] / [Map 좌표 다시 계산] |
| 파일 목록 | Wafer ID / Lot / Slot / 공정 / Test Date / Die / 저장 Para / **추가 Para** / Spec Fail Die / Map Die / 판정 시각 / YMS / 등록자 / 등록 시각 / 파일명 |
| 아래 | 공통 Control (목록에서 고른 파일, 고르면 바로 조회) |

- 메뉴 신규: `MNU_DMS_ET_RESULT_VIEW` (F03127, POC 메뉴에서 결과 업로드 바로 다음). `DACruxUXBasic01` 기반.
- 팝업 조회 창은 없앤다 (Upload 의 [등록 파일] 더블클릭도 없어짐). 결과 보기는 View 화면 하나.
- **추가 Para** = 지금 등록 Parameter(사용 Y, 이 Device + 공정) 중 **업로드 뒤 등록 / 변경되었고**(등록 Parameter 수정 시각 > 파일 등록 시각) 이 파일에 저장 안 된 수. 0 보다 크면 노란색 + 정보 표 안내 "추가 Para n개 — 다시 Upload 해야 보임" (C7).
  업로드 때 이미 파일에 없던 Parameter(누락, MISSING_COUNT)는 다시 올려도 생기지 않으므로 시각 비교로 뺀다 (구현 중 보정).
- 삭제 확인 문구에 "YMS 에 올린 데이터도 함께 지웁니다" 추가 (서버는 이미 함께 지움).

## 6. 서버 / 쿼리 (작게, DDL 없음)

| 대상 | 변경 |
| --- | --- |
| `TQP_GDS_SRC_FILE.xml` | 신규 `SELECT_SRC_FILE_BROWSE` (구현 이름, Factory / Device / 공정 선택 / Lot 글자): 지금 열 + `ADDED_PARA_COUNT` + `SPEC_CHANGED`(Spec 수정 시각 > 판정 시각) + YMS 상태 열. 지금 쿼리는 그대로 둔다 (배포된 서비스 호환) |
| `iGdsEtResult` / RO / BSL | 신규 `GetEtFileBrowseList(factory, device, testProgram, lot)` 1개. 나머지(GetEtFile / GetEtDieList / GetEtValueList / 등록 Parameter 조회 / Delete / Remap)는 그대로 |

## 7. 소스 구성 / 영향

| 파일 | 변경 |
| --- | --- |
| 신규 `ENGUI/ETest/EtResultData.cs`, `EtResultDataControl.cs` / `.Designer.cs` | 공통 모델 + 변환기 2개 + Control |
| `frmEtResultUpload.cs` / `.Designer.cs` | 아래 3칸 -> Control, [등록 파일] 탭 제거, 저장 확인 문구 |
| `frmEtResultView.cs` / `.Designer.cs` | 메뉴 화면으로 다시 작성 (파일 목록 + Control) |
| `TQP_GDS_SRC_FILE.xml`, `iGdsEtResult.cs` / RO / BSL `GdsEtResult.cs` | 쿼리 1개 / 메서드 1개 추가 |
| `01_WinApp/DACruxV5/MenuFile.mnu` | View 메뉴 1행 추가 |
| ENGUI csproj | Compile 3행 추가 |
| Smoke | `EtResultUploadFormSmoke.cs` 다시 작성 (컨트롤 이름이 바뀜), 신규 View Smoke, `Verify-EtResultUpload.ps1` |

동시 편집 주의:
- `frmEtResultUpload` / `frmEtResultView` 는 10-15 오전 CLAUDE-104 / 105 / 107 이 고쳤고 DMS 화면 확인 대기다. 구현 때 최신 파일을 다시 읽고 그 위에 작업한다.
- `MenuFile.mnu`(13:32) / `frmEtParaRegister`(13:47) 는 작업 현황에 기록 없는 오늘 변경이 있다. 메뉴 편집 전에 확인한다 (`frmEtParaRegister` 는 이 작업에서 손대지 않음).

## 8. 바뀌지 않는 것

- 저장 대상(등록 Parameter 사용 Y) / 저장 순서 / 서버 판정 · Map 좌표 · YMS 적재 / 다시 판정(Para 설정 화면) / 중복 키 / DB 구조.

## 9. 결정 (2026-10-15 권장안으로 확정)

| 번호 | 항목 | 권장 | 다른 안 |
| --- | --- | --- | --- |
| V1 | 2단계 표 모양 | **긴 표 하나** (행 = Die x Parameter, 1단계 Die 선택으로 거름) | Parameter x Die 표(지금 조회 창) |
| V2 | Lower / Upper | **지금 Spec + "판정 뒤 Spec 바뀜" 안내** | 판정 당시 값 저장 (DB 변경) |
| V3 | 팝업 조회 창 | **없애고 View 화면 하나로** | 팝업도 남김 (View 와 Upload 에서 같이 사용) |
| V4 | View 버튼 | **[선택 파일 삭제] / [Map 좌표 다시 계산]** (지금 등록 파일 탭과 같음) | + [다시 판정] (지금은 Para 설정 화면에만 있음) |
| V5 | View 목록의 추가 Para / YMS / Spec 바뀜 열 | **넣는다** | 넣지 않음 |
| V6 | 저장 확인 문구 | **파일별 Parameter / Die / 값 표 + 합계** | 합계만 |

## 10. 구현 순서 (결정 후)

| 단계 | 내용 |
| --- | --- |
| 1 | `EtResultData` + 변환기 2개 + `EtResultDataControl` |
| 2 | Upload 화면: 아래 3칸 -> Control, [등록 파일] 탭 제거, 저장 확인 문구 |
| 3 | 서버 쿼리 / 메서드 1개 + View 화면(메뉴 화면) + 메뉴 |
| 4 | Smoke (Upload 다시 작성 / View 신규) + 빌드 / 검증 |

## 11. 구현 결과 (2026-10-15)

| 대상 | 내용 |
| --- | --- |
| 공통 | 신규 `ENGUI/ETest/EtResultData.cs` (모델 + `FromUpload` / `FromDb`), `EtResultDataControl.cs` / `.Designer.cs` (정보 표 / 1단계 Die / 2단계 정의 Para 긴 표 + [Fail 만] ['-' 만] 찾기, DataView 거르기) |
| Parameter 순서 | 두 화면 모두 **표시 순서 -> 저장 이름** (업로드 미리보기도 파일 열 순서 대신 조회와 같은 순서). `EtUploadPlan.Param.DisplayOrder` 추가 |
| Upload | `frmEtResultUpload` 아래 3칸 -> 공통 Control, [등록 파일] 탭 / 조회 창 열기 제거, 저장 확인 문구 = 파일별 "파일 / 공정 / Parameter p x Die d = 값 v" (10줄까지) + 합계 + 등록 안 된 헤더 수 + 다시 업로드 안내 + 경고 / 오류 수. 저장 뒤 안내에 "결과는 E-Test 결과 조회 화면에서" |
| View | `frmEtResultView` 를 메뉴 화면(`DACruxUXBasic01`)으로 다시 작성: Factory / Device / 공정((전체) + 목록) / Lot 글자 / [조회] / [선택 파일 삭제] / [Map 좌표 다시 계산] + 파일 목록 + 공통 Control. 목록 색: Spec Fail Die 빨강 / 추가 Para 노란 배경 / Spec 바뀜 주황 / Map 없음 회색 |
| 서버 | `SELECT_SRC_FILE_BROWSE` + `GetEtFileBrowseList` (Interface / RO / BSL). DDL 없음 |
| 메뉴 | `MNU_DMS_ET_RESULT_VIEW` "E-Test 결과 조회" (F03127, POC 메뉴 순서 8 = 결과 업로드 다음). 숨김 메뉴 3개(KLARF 분석 / Die 분석 / KLARF File View) 순서 8~10 -> 9~11 |
| 검증 | `Verify-EtResultUpload.ps1` (Upload + View 한 Smoke, 34 검사) **PASS**: 미리보기 888행 / #44 Fail 6 / 저장 확인 문구 `Parameter 37 x Die 24 = 값 888` x 2 + 합계 1,776 / 조회 목록 열 / **업로드 미리보기와 조회 888행 같음(행 순서 · 값 · 판정 · 사유)** / 추가 Para(등록 전 파일 1, 등록 뒤 파일 0) / Spec 수정 뒤 Spec 바뀜 Y + 안내 2줄 / 삭제. `GdsEtResultProbe.ps1` 18/18, `DieCoordinateProbe` 11/11 PASS |
| 화면 이미지 | `%TEMP%\ChippacEtResultUploadVerification\upload.png` / `view.png` |

## 12. 업로드 파일 하나씩 (2026-10-15 변경, CLAUDE-114)

사용자 지시:
> Upload는 단일 파일 즉, 하나씩만 올리는 컨셉으로 한다. - 멀티는 본프로젝트 할때..

| 대상 | 내용 |
| --- | --- |
| 화면 | 위 1줄: Factory / [파일 열기...] / [다시 읽기] / 파일 경로(읽기 전용) / Slot / [저장]. 2줄: 상태(색: 준비 초록 · 경고 / 중복 주황 · 오류 / 실패 빨강 · 저장됨 파랑) + 메시지. 아래: 공통 결과 Control |
| 없앤 것 | 대기 목록 18열 / [폴더 추가] / [목록 비우기] / Del 로 빼기 / [모두 저장] · [선택 저장] / 백그라운드 분석 / 목록 안 중복 / 등록 Parameter · Map 캐시(파일을 열 때마다 새로 읽음) |
| Slot | 파일명에서 못 읽으면 `오류(Slot 을 입력하세요)` -> Slot 칸 입력(Enter 또는 칸을 벗어나면) -> WAFER_ID / DB 중복 다시 확인 |
| 저장 확인 | "파일: 이름 / WAFER_ID w / 공정 p / 저장: Parameter p x Die d = 값 v" + 등록 안 된 헤더 수 + 다시 업로드 안내 + 경고. 중복이면 예 = 덮어쓰기 / 아니요 = 저장 안 함 |
| 저장 뒤 | 상태 `저장됨` + 메시지 `저장 완료 / 파일 번호 n / ...`, 결과 제목 `업로드 결과 - 저장됨 (파일 번호 n)`, 정보 표 파일 번호, [저장] 꺼짐 / Slot 잠김 |
| [다시 읽기] | 같은 파일을 다시 분석 (등록 Parameter / Spec / Map 을 바꾼 뒤, "분석 뒤 등록 Parameter 바뀜" 실패 뒤) |
| 다중 파일 | 본 프로젝트에서 쓰려고 직전 버전 사본을 [참고소스_다중파일_업로드_20261015](참고소스_다중파일_업로드_20261015/README.md) 에 둠 (빌드 제외) |
| 검증 | `Verify-EtResultUpload.ps1` Smoke 35/35 PASS (파일 하나씩 열기 오류 3종 / Slot 입력 / 저장 확인 문구 / 저장 뒤 상태 / 중복 아니요 · 예 / [다시 읽기] + 조회 화면 검사 그대로), `DieCoordinateProbe` 11/11 |

### 12.1 1단계 '-' 수 확인 (사용자 질문, 2026-10-15)

- 1단계 Die 표의 `'-' 수` 는 **정의 Para(저장 대상 등록 Parameter)의 '-' 값 수**다. 2단계 표와 같은 기준이며 Spec 등록과는 관계없다(Spec 이 없어도 센다).
- `SampleFile\26_10_14 Sample File\CAI_OMM_ATV_A0_TSVET_20261002161846_U9-HS2635_U9-HS2635-01$Silterra$.xlsx` 확인: 파일 전체 '-' 554개가 26개 헤더(Leakage_Parallel / Leakage_Serial 의 C4DC_1xx · PIC_COMB11/12 계열, Die 69개)에만 있고, 로컬 DB 등록 Para(SCK / OMM / TSVET 109개)에는 '-' 가 0개다(저장값 7,957개 모두 V). 그래서 0 이 맞다.

## 13. Map 필수 / [Map 좌표 다시 계산] 삭제 / 정보 표 축소 · 수정 (2026-10-15, CLAUDE-116)

사용자 지시:
> 업로드할 때 OMM Map이 없으면 그냥 Error 처리 하는 걸로 바꿔줘. 그리고 나서 해당 기능은 제거
> 디바이스, 공정, Lot, WaferNo은 더블클릭해서 수정 가능 하도록 해줘. 저장 Parameter 부터 아래는 안보이게 해줘.

| 대상 | 내용 |
| --- | --- |
| Map 필수 | 업로드 분석 때 File Map Setup 에 Map ID = Device(사용 Die 있는 Map)가 없으면 `오류`: `Map 없음: File Map Setup 에 Map ID OMM (사용 Die 가 있는 Map) 을 먼저 만드세요` -> [저장] 꺼짐. Map 밖 Die 는 지금처럼 경고 |
| [Map 좌표 다시 계산] 삭제 | 조회 화면 버튼 / 처리, 원격 호출(iGdsEtResult / RO `RemapEtFiles`), 메뉴 툴팁 문구 삭제. Map 을 나중에 만들거나 고쳤으면 그 파일을 다시 업로드한다. 서버 BSL `RemapEtFiles` 는 검사 스크립트(GdsEtResultProbe / EtYmsServiceProbe)가 Map 좌표 계산 · YMS 다시 반영 확인에 직접 쓰므로 남김(화면에서는 못 부름). YMS 보류(WAIT) 안내 문구도 "다시 업로드" 로 바꿈 |
| 정보 표 축소 | 업로드 / 조회 공통: 파일명 / Device / 공정 / Lot / Lot-Wafer No / Slot / WAFER_ID / Test Date / Customer · Wafer Type · Rev / Die / 헤더 / 등록 (사용 Y) 11행만. 저장 Parameter · 저장 값 · 누락 · 건너뜀 · '-' 값 · Spec Pass / Fail · Map · 상태 / 메시지 · 파일 번호 · 판정 시각 · YMS · 등록 · 충돌 · 안내 행 삭제 (상태 / 메시지는 업로드 화면 위 상태 줄, 추가 Para / Spec 바뀜 / YMS 는 조회 목록 열, 개수는 저장 확인창 / 2단계 건수 표시) |
| 정보 표 수정 | 업로드 화면(저장 전)만: Device / 공정 / Lot / Lot-Wafer No 값 칸(옅은 파란색)을 더블클릭해 고친다 -> 파일을 다시 읽지 않고 고친 값으로 다시 분석(등록 Parameter / Map / Slot / DB 중복). 공정은 대문자 이름 규칙(`tsvet` -> `TSVET`), Lot-Wafer No 를 고치면 Slot 도 끝 '-숫자' 로 다시 정함, 빈 값은 거부. 저장에는 고친 값(DEVICE_ID / TEST_PROGRAM / LOT_ID / LOT_WAFER_NO / WAFER_ID), FILE_NAME 은 원래 이름. [다시 읽기] 는 파일명 값으로 돌아감. 저장 뒤 / 조회 화면은 고칠 수 없음 |
| Slot 칸 | 사용자가 VS 디자이너에서 위 Slot 칸(lblSlot / txtSlot)을 숨김(Visible = false). Slot 은 Lot-Wafer No 수정으로 정한다. Slot 없음 오류 문구: `Slot 없음: Lot-Wafer No ... 끝에 '-숫자(1~99)' 가 없습니다. 정보 표의 Lot-Wafer No 를 더블클릭해 고치세요.` |
| 소스 | `EtResultData`(InfoDevice / InfoProgram / InfoLot / InfoWaferNo, EditableInfo, FromUpload editable 인자, FromDb listRow 인자 삭제), `EtResultDataControl`(정보 표 EditProgrammatically + 더블클릭 / 편집 끝 -> InfoEdited 이벤트, InfoEditedEventArgs), `frmEtResultUpload`(Map 필수, AnalyzeFile 분리, resultView_InfoEdited), `EtResultFileParser.SlotFromWaferNo` |
| 검증 | `Verify-EtResultUpload.ps1` 이 PROBE / OMM 시험 Map(SCK OMM_Hybrid 사용 Die 복사)을 넣고 끝나면 지움. Smoke 39/39 PASS (Map 없음 오류 -> Device 수정 / 공정 수정 / Lot-Wafer No · Lot 수정 / 정보 표 11행 / 저장 뒤 잠김 / DB 에 고친 값 / 조회 화면 수정 불가 · 버튼 없음), `GdsEtResultProbe` 18/18, `DieCoordinateProbe` 11/11 |
