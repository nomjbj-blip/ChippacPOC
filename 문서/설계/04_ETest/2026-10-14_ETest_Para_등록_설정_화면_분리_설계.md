# E-Test Para 등록 / 설정 화면 분리 설계

작성: 2026-10-14 Claude (CLAUDE-096) / 상태: **구현 완료(빌드 / Smoke 확인, 14:59). 배포 · DMS 화면 확인 대기** (사용자: "설계 하고 바로 나누고 작업 한다")
앞 문서: [Para 화면 재구성 설계](2026-10-14_ETest_Para_화면_재구성_설계.md) — 한 화면의 탭 2개를 이 문서대로 화면 2개로 나눈다. 서버 / DB / 판정 규칙은 그대로다.

## 1. 요청

1. **E-Test File 로 등록하는 화면**: Factory 외에는 전부 파일에서 가져와 기준 정보를 만든다(읽기 전용). DB 에 등록만 한다 = 지금 [파일 헤더] 탭.
2. **E-Test Para setup 화면**: 등록된 data 를 DB 에서 조회 / 수정 / 삭제한다. 전부 등록된 data 를 기준으로 골라서 조회한다 = 지금 [등록 Parameter] 탭.
3. 두 화면으로 나누고 메뉴 아이콘도 따로 쓴다.

## 2. 화면

| | E-Test Para 등록 (신규) | E-Test Para 설정 (기존 화면 정리) |
| --- | --- | --- |
| 메뉴 | `MNU_DMS_ET_PARA_REG` / POC 순서 5 / F03126 / 새 아이콘(표 문서 + 초록 '+') | `MNU_DMS_ET_PARA` / POC 순서 6(지운 Limit 메뉴 자리) / F03125 / 지금 아이콘(차트 + 톱니) |
| 클래스 | `DACrux.SEMDMS.ENGUI.frmEtParaRegister` | `DACrux.SEMDMS.ENGUI.frmEtParaSetup` |
| 위 줄 | Factory(입력) / Device · 공정 · 파일(읽기 전용, 파일명에서) / [결과 파일 열기...] | Factory(입력) / Device▼ · 공정▼(목록에서만 고름 = 등록된 Device · 공정) / [조회] |
| 본문 | 헤더 표(상태 / 원문 / 저장 이름 / 측정 / 조건 / Chain · Leg / 단위 / Test No / GDS Chain · Spec(읽기 전용, 참고) / 메시지), 거르기, 목록에서 빼기, 체크 + [등록] | 등록 Parameter 표(GDS Chain / Lower / Upper / Enable / 사용 / 비고 편집) + 오른쪽 GDS Setup Chain 목록, [Spec 표 붙여넣기] / [선택 삭제] / [저장] |
| DB | 등록(`RegisterEtParameters`, 출처 FILE)만. 조회는 상태 계산용(`GetEtParameterList`) | 조회 / 저장 / 삭제 / Spec 붙여넣기 / 다시 판정 |

- 등록 화면은 수정 / 삭제가 없다. 등록한 뒤의 GDS Chain / Spec / 사용 / 삭제는 설정 화면에서 한다.
- 설정 화면의 Device / 공정 콤보는 **목록에서만 고른다**(직접 입력 없음). 처음 쓰는 Device 는 등록 화면에서 결과 파일로 먼저 등록한다. [Spec 표 붙여넣기]는 고른 Device 로 저장하고, 표에만 있는 공정(COWET / FT 등)의 Parameter 는 지금처럼 출처 SPEC 으로 먼저 등록한다.
- 두 화면은 서로 부르지 않는다. 다른 화면에서 바꾼 내용은 등록 화면은 파일을 다시 열 때, 설정 화면은 [조회] 할 때 보인다.
- 저장하지 않은 수정 확인 / 수정 행만 저장 / 진행 표시줄은 재구성 설계 그대로 설정 화면에 있다. 등록 화면은 확인 질문 1개([등록])만 있다.

## 3. 변경 파일

| 파일 | 변경 |
| --- | --- |
| `ENGUI/ETest/frmEtParaRegister.cs` / `.Designer.cs` | 신규. 지금 Para 화면의 파일 헤더 부분(파일 읽기 / 헤더 분해 · 상태 / 거르기 / 목록에서 빼기 / 체크 / 등록)을 옮김 |
| `ENGUI/ETest/frmEtParaSetup.cs` / `.Designer.cs` | 파일 열기 / 닫기 / 헤더 표 / 탭 삭제. 콤보 DropDownList |
| `ENGUI/DACrux.SEMDMS.ENGUI.csproj` | Compile 2행 추가 |
| `01_WinApp/DACruxV5/MenuFile.mnu`, `100_ClientBin/MenuFile.mnu` | `MNU_DMS_ET_PARA_REG` 블록 1개 추가(Para 블록 앞), `MNU_DMS_ET_PARA` 는 MNU_ORDER 5 -> 6 과 설명(TOOL_TIP)만. 다른 메뉴 행은 그대로 |
| `ENGUI/Verification/EtParaSetupFormSmoke.cs` / `Verify-EtParaSetup.ps1` | 두 화면을 차례로 검사(등록 화면 -> 설정 화면 -> 등록 화면 다시 열어 GDS Chain / Spec 열 확인) |

## 4. 검증 기준

| 검사 | 기대값 |
| --- | --- |
| 등록 화면 | Device / 공정 / 파일 칸 읽기 전용, 탭 없음, 편집 표 없음. 샘플 열기 -> 헤더 2,755 신규, 직접 체크 37 -> [등록] -> 등록됨 37 |
| 설정 화면 | 탭 없음, 파일 열기 버튼 없음, 콤보 DropDownList. Device 목록에 등록 Device, 고르면 공정 첫 값으로 바로 조회 37행 |
| 설정 화면 기능 | 재구성 설계 6절 그대로(값 통계 열은 사용자 요청으로 삭제, 붙여넣기 82 등록 + Spec 119 / 수정 행만 저장 / 다시 판정 #44 Fail 6 -> 4 / Spec 비우기 / 거르기 / GDS 목록 복사 / 삭제 거부 / 공정 바꿀 때 수정 버림 질문) |
| 등록 화면 다시 열기 | 설정 화면에서 넣은 GDS Chain / Spec 이 헤더 표 GDS Chain / Spec 열에 보임 |
| 메뉴 | POC: 4 E-Test 결과 업로드 / 5 E-Test Para 등록(새 아이콘) / 6 E-Test Para 설정 / 7 파일 Map Setup ..., XML 파싱 정상 |

## 5. 필터 콤보 (2026-10-15, CLAUDE-121)

요청: "frmEtParaSetup 상에 조건 컬럼 별로 Filter 기능 넣어줘" -> (보충) "콤보박스에서 정보가 나오고 선택해서 data 를 필터하는 것". 처음에 만든 머리글 ▼ 값 고르기 창(Excel 자동 필터 방식)은 요청과 달라 지웠다.

| 항목 | 내용 |
| --- | --- |
| 위치 | 표 위 줄(paraBar) 왼쪽부터 `측정` / `조건` / `Chain 1` / `GDS Chain` / `사용` / `Enable` 콤보(DropDownList, 목록에서만 고름) + [필터 해제]. 오른쪽 [저장] 은 그대로 |
| 목록 | `(전체)` + 그 열 값(2026-10-22 건수 표시 삭제). 빈 값은 `(빈 값)` 으로 맨 끝. 사용 / Enable 은 `Y` / `N`. 글자 순(대소문자 무시) |
| 목록 범위 | 다른 콤보에서 고른 값을 통과한 행의 값만 보인다(예: 조건 = IFORCE=8.0MA 를 고르면 GDS Chain 목록은 그 28행 값만). 콤보를 펼칠 때 다시 모으므로 수정 중인 값도 들어간다 |
| 거르기 | 고른 값과 같은 행만 보인다. 여러 콤보는 모두 맞아야(AND) 보인다. 수정한 행(노란 칸 / `*`)은 필터와 상관없이 보인다(기존 거르기 규칙과 같음) |
| 유지 / 해제 | 같은 Device / 공정을 다시 조회(저장 뒤 포함)하면 고른 값을 유지한다(값이 없어지면 `(0)` 으로 남김). Device / 공정이 바뀌면 모두 `(전체)`. [필터 해제] = 모두 `(전체)`, 고른 값이 없으면 버튼이 꺼진다 |
| 상태 줄 | 고를 때 `필터 조건=IFORCE=8.0MA, GDS Chain (Chain 1)=(빈 값) / 표시 26 / 전체 37`, 조회 뒤 `... / 필터 조건=IFORCE=8.0MA 표시 28` |

- 서버 / DB 변경 없음. 화면 코드만(`frmEtParaSetup.cs` `[필터 콤보]` 구역, `.Designer.cs` 라벨 6 + 콤보 6 + [필터 해제]).
- 2026-10-15 사용자가 VS 디자이너에서 바꾼 상태를 그대로 두었다: 찾기 / Spec 있는 것만 / GDS Chain 없는 것만 / [Spec 표 붙여넣기] / [선택 삭제] 숨김(Visible = false), 정보 줄(`lblParaInfo`) / 우클릭 메뉴(Spec 비우기 · 수정 취소 · GDS 이름 복사) 삭제, Factory 칸 읽기 전용(등록 화면 포함). 필터 콤보는 숨긴 찾기 / 체크 자리에 놓였다(디자이너에서는 겹쳐 보임).
- 검증: `Verify-EtParaSetup.ps1` -> `ET_PARA_FORM_SMOKE=PASS` 47/47 (필터 콤보 검사 7개 포함, 지운 정보 줄 / 우클릭 메뉴를 쓰던 검사는 표에서 직접 세도록 바꿈).
- 오른쪽 GDS Device (같은 날, 사용자 지시 "선택이 아닌 상단의 e test device 로 조회 한다. 없으면 없는대로 보여준다"): GDS Device 콤보를 읽기 전용 칸(`txtGdsDevice`)으로 바꾸고, 조회 때 위 E-Test Device 이름 그대로 `GetEtGdsChainList(Factory, E-Test Device)` 를 부른다. GDS Device 찾기(`GetEtGdsDevice`) / GDS Device 목록 / 바꾸기 처리는 화면에서 뺐다(서버 API 는 그대로). 그 이름의 GDS 가 없으면 목록 0개 그대로(`GDS Setup Chain 0개`), 표의 GDS Chain 은 주황 글자. Smoke: 시험 GDS(Map / Chain)를 지우고 다시 조회 -> 목록 0 확인.

## 6. 등록 화면 정리 (2026-10-21, CLAUDE-190)

- 헤더 표에서 Test No 오른쪽 열(GDS Chain / Spec / 메시지)을 숨겼다(Visible = false, 값은 그대로 채움). 2절 표의 "GDS Chain · Spec(읽기 전용, 참고) / 메시지" 는 화면에 보이지 않는다.
- 위쪽 줄의 파일 칸 아래에 파일 이름 규칙을 보인다: `규칙 [Customer]_[Device]_[Wafer Type]_[Wafer Rev]_[Test Type]_[Test Date]_[Lot]_[Lot-Wafer No]$Fab$`. 글자는 파일을 열 때 쓰는 `EtResultFileParser.FileNameRule` 을 그대로 넣어 규칙과 화면이 어긋나지 않게 한다.
