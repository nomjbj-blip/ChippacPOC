# 테스트 계획 — CLAUDE-070 GDS Map 조회 화면 도면 우클릭 메뉴 "전체 보기" 제거

- 범위 단계: Standard (변경 작음 → 정상 + 부정 + 회귀, DB 쓰기 없음)
- 변경 파일: `DMS/06_CoreLibSource/GdsMapControl/GdsMapViewForm.Designer.cs`, `GdsMapViewForm_Layout.cs`, `Probes/GdsViewLayoutSmoke.ps1`
- 표면: .NET (WinForms). DB 는 회귀 Probe 의 읽기 전용 조회만.
- 화면 확인: DMS 실제 화면 우클릭은 사용자 확인 필요(화면 확인 대기).

| ID | 분류 | 사전조건 | 단계 | expect | 실행 수단 | 데이터 정리 |
| --- | --- | --- | --- | --- | --- | --- |
| SCN-01 | 정상 | 소스 저장됨 | GdsMap csproj 를 TEMP 로 빌드 | 오류 0, 경고는 알려진 CS0649 1건뿐 | MSBuild | 없음 |
| SCN-02 | 정상 | SCN-01 EXE | 화면을 화면 밖에 띄워 `map.ContextMenuStrip` / `mapMenu` / `mnuCtxZoomFit` 확인 | 모두 null (`mapContextMenu=True`) | `GdsViewLayoutSmoke.ps1` | PNG 만 TEMP |
| SCN-03 | 부정(red) | 변경 전 EXE(`SOURCE_DROP_gds_164119`) | 같은 Smoke 를 변경 전 EXE 로 실행 | '도면 우클릭 메뉴 없음' FAIL (검사가 실패할 수 있음을 확인) | `GdsViewLayoutSmoke.ps1` | 없음 |
| SCN-04 | 회귀 | SCN-01 EXE | 레이아웃 나머지 항목([Map ▾] 메뉴 / 원점 표시 / Chain 작업 줄 / Label 콤보 없음 등) | Smoke 20항목 FAIL 0 | `GdsViewLayoutSmoke.ps1` | 없음 |
| SCN-05 | 부정 | 소스 | 삭제한 이름(mapMenu / mnuCtxZoomFit / mapMenu_Opening / mnuCtxZoomFit_Click)을 참조하는 코드 남음 여부 | 0건 (Probe 검사 문자열 제외) | grep | 없음 |
| SCN-06 | 회귀 | SCN-01 EXE, DB 실행 중, PROBE/FULL_PROBE | 조회 화면 Label 검색(화면 객체 생성 + 검색 + 이동) | `GDS_LABEL_SEARCH_PROBE=PASS` | `GdsLabelSearchProbe.ps1` (읽기 전용) | 없음 |
| SCN-07 | 회귀 | SCN-01 EXE | Sub Device 설정 화면 / 조회 화면 연동 Smoke | `GDS_SUB_DEVICE_UI_SMOKE=PASS` | `GdsSubDeviceUiSmoke.ps1` | Smoke 가 만든 데이터 정리 확인 |
| SCN-08 | 정적 | 소스 | 고친 줄 AI 흔적 / 줄바꿈·BOM 원본 유지 | 흔적 0, 원본과 같은 LF / BOM 상태 | grep / python | 없음 |

입력검증 / 권한·상태 / 경계: 해당 없음 — 입력이나 권한, 경계값이 없는 UI 항목 제거.
예외: 우클릭 시 동작 = 지도 컨트롤에 오른쪽 버튼 처리 코드 없음(`Controls/GdsMapControl_Mouse.cs` 는 Left / Middle 만) → SCN-02 로 메뉴 없음 확인.
