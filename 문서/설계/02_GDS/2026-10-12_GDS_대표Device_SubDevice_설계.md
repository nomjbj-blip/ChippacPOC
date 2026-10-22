# GDS 대표 Device / Sub Device 설계

작성: 2026-10-12 Claude / 작업 ID: CLAUDE-055 / 상태: 설계 + 구현 시작

> **폐기 (2026-10-15, CLAUDE-109):** Sub Device 기능은 GDS Map 복사로 대체되어 제거했다. KLARF Product / E-Test Device 는 이름이 같은(대소문자 무시) READY GDS Map 에 연결된다. [2026-10-15_GDS_Map_복사_설계.md](2026-10-15_GDS_Map_복사_설계.md) 참고. 아래 내용은 이력이다.

## 1. 요청

사용자 지시: "GDS View 화면에서 대표 디바이스를 U9으로 하고 사용가능한 Sub 디바이스를 U9, OMM, OMM Hybrid 등등 메뉴얼로 Setup 할 수 있도록 기능 구현. 다른 디바이스 조회 및 연동하는 부분을 Sub Device 로 연동되도록."

지금 같은 제품이 데이터마다 이름이 다르다.

| 데이터 | Device 이름 (샘플) | 출처 |
| --- | --- | --- |
| GDS Map | `U9` | `TQP_GDS_MAP.DEVICE_ID` (Map 1개 = 대표) |
| KLARF | `OMM_Hybrid` | `TQD_INSP_INFO.PRODUCT` |
| E-Test | `OMM` | 파일명 Device (`TQP_GDS_SRC_FILE.DEVICE_ID`) |

그래서 GDS-KLARF 연관 분석은 Device를 손으로 골라야 했고(연관분석 설계 "이름 대응 규칙은 미정"), E-Test는 "GDS Device(U9)와 E-Test Device(OMM) 연결 보류"(ET Chain Map DDL 주석) 상태다.

## 2. 개념

- **대표 Device** = GDS Map이 있는 Device(`TQP_GDS_MAP`). 도형 / Chain은 대표 Device에만 있다.
- **Sub Device** = 대표 Map을 같이 쓰는 다른 이름. 사용자가 GDS Map 조회 화면에서 직접 등록한다.
- 대표 Device 자신은 언제나 자기 Sub로 취급한다(따로 저장하지 않는다). 화면 목록 첫 줄에 "(대표)"로 보여 준다.
- 다른 데이터의 Device로 GDS Map을 찾을 때 순서: ① Sub 등록 -> 그 대표 ② 같은 이름의 READY Map ③ 없음. 대소문자는 무시한다.

## 3. DB

신규 `TQP_GDS_MAP_SUB_DEVICE` (DDL `06_CoreLibSource/GdsMapControl/Persistence/2026-10-12_tqp_gds_map_sub_device.sql`)

| 컬럼 | 형식 | 설명 |
| --- | --- | --- |
| FACTORY | VARCHAR2(30 CHAR) PK | |
| SUB_DEVICE_ID | VARCHAR2(50 CHAR) PK | 입력한 그대로 저장. `UPPER` 고유 색인으로 대소문자만 다른 중복을 막는다 |
| DEVICE_ID | VARCHAR2(50 CHAR) NOT NULL | 대표 Device (`TQP_GDS_MAP.DEVICE_ID`) |
| REMARK | VARCHAR2(200 CHAR) | 예: KLARF Product / E-Test 파일명 |
| CREATE_USER / CREATE_TIME | | 감사 |

- Sub Device 1개는 대표 1개에만 속한다(PK = FACTORY + SUB_DEVICE_ID).
- **FK 없음**: Map은 "삭제 후 재등록"으로 다시 만드는데(리비전 없음), FK CASCADE면 재등록 때 Sub 설정이 사라진다. Map이 없거나 READY가 아니면 찾기에서 빠질 뿐이다.

## 4. 서비스 (DefectDefine 방식, 새 wellknown 없음)

| 서비스 | 메서드 | 내용 |
| --- | --- | --- |
| `GdsMapView` (조회) | `GetSubDeviceList(factory, device)` | 대표의 Sub 목록. 열 SUB_DEVICE_ID / REMARK / CREATE_USER / CREATE_TIME |
| `GdsMapView` (조회) | `GetMapDeviceBySub(factory, anyDevice)` | 다른 데이터 Device -> 대표 Device 1행 (DEVICE_ID / MATCH_TYPE = SUB 또는 SELF). READY Map만, 없으면 0행 |
| `GdsMapImport` (저장) | `SaveSubDeviceList(factory, device, subs[], remarks[], user)` | 대표 Map 확인 -> 입력 정리(공백 / 대표 자신 제외 / 대소문자 중복 오류) -> 다른 대표에 등록된 Sub, 다른 Map의 대표 Device 이름이면 오류 -> 기존 삭제 후 일괄 등록(InsertExecuteMultiple). 등록 수 반환 |

DSL / XML: `TQP_GDS_MAP_SUB_DEVICE` (테이블별 1개).

## 5. 화면

### 5.1 GDS Map 조회 (GdsMapViewForm)

- 조회 조건 줄에 **[Sub Device]** 버튼 (DMS 호스트 모드에서만 보임, Map 삭제와 같은 조건). 누르면 Sub Device 설정 창.
- Sub Device 설정 창(`SubDeviceSetupForm`, Designer): 대표 Device 표시 / 목록(Sub Device, 비고) / 입력 + [추가] / [삭제] / [저장] / [닫기]. 첫 줄은 대표(수정 불가).
- Map 정보에 `Sub Device: OMM, OMM_Hybrid` 줄 추가.
- Device 콤보에 목록에 없는 이름(예: `OMM_Hybrid`)을 입력하고 Enter -> Sub 등록이면 대표(`U9`)를 연다. 상태 줄 "Sub Device OMM_Hybrid -> 대표 U9".

### 5.2 연동 (다른 Device 조회)

| 화면 / 서비스 | 지금 | 변경 |
| --- | --- | --- |
| GDS-KLARF 연관 분석 화면 `frmGdsKlarfAnalysis` (CLAUDE-053) | KLARF Product와 같은 이름의 GDS Device만 자동 선택 | 같은 이름이 없으면 `GetMapDeviceBySub(Product)`로 대표 Device 자동 선택 + 상태 줄 표시 |
| `GdsKlarfAnalysis.AnalyzeWafer` (BSL) | Device 인자가 비면 Product 이름 그대로 | Product -> 대표 Device로 찾고 없으면 Product 그대로 |
| E-Test (CLAUDE-029 진행 중) | GDS 연결 보류 | **이번에는 손대지 않는다**(다른 세션이 같은 파일 수정 중). 연결할 때 `GetMapDeviceBySub(E-Test Device)`를 쓰도록 E-Test 설계 / 작업 현황에 메모 |

## 6. 결정 (권장안으로 진행, 바꿀 수 있음)

| 번호 | 항목 | 적용 |
| --- | --- | --- |
| D1 | 대표 자신 저장 여부 | 저장 안 함, 언제나 포함 |
| D2 | Sub 1개가 여러 대표에 속할 수 있나 | 안 됨(1:1) |
| D3 | Sub 이름이 다른 Map의 대표 Device와 같을 때 | 저장 오류(어느 Map을 쓸지 모호해짐) |
| D4 | 대소문자 | 무시하고 비교, 입력한 그대로 표시 |
| D5 | Map 삭제 시 Sub 설정 | 남긴다(재등록하면 다시 연결) |
| D6 | 비고 컬럼 | 둔다(어느 데이터 이름인지 적는 용도) |

## 7. 구현 순서

1. DDL + 로컬 DB 적용
2. DSL / XML / Interface / RO / BSL + Probe
3. GDS Map 조회 화면 (Sub Device 설정 창 / Map 정보 / Enter 찾기)
4. KLARF 연관 분석 연동
5. 빌드 / Probe / 문서

## 8. 구현 결과 (2026-10-12)

| 구분 | 파일 |
| --- | --- |
| DB | `Persistence/2026-10-12_tqp_gds_map_sub_device.sql` **로컬 DB 적용 완료** (PK FACTORY + SUB_DEVICE_ID / UPPER 고유 색인 / 대표 자신 금지 CHECK / 대표 색인) |
| XML / DSL | 신규 `08_DBQuery/ORACLE/TQP_GDS_MAP_SUB_DEVICE.xml`(SELECT_SUB_DEVICE_LIST / SELECT_SUB_DEVICE_FACTORY / SELECT_MAP_DEVICE_BY_SUB / DELETE_SUB_DEVICE_BY_MAP / CREATE_SUB_DEVICE) / `DSL/TQP_GDS_MAP_SUB_DEVICE.cs` + csproj 1행 |
| 서비스 | Interface·RO·BSL `GdsMapView` + `GetSubDeviceList` / `GetMapDeviceBySub`, `GdsMapImport` + `SaveSubDeviceList` (새 wellknown 없음) |
| GDS Map 조회 | 신규 `SubDeviceSetupForm.cs` / `.Designer.cs`, 신규 `GdsMapViewForm_SubDevice.cs`, `GdsMapViewForm.cs`(호스트 생성자 / LoadMapInfo / ClearMapInfo / SetLoading / MapItem.SubDevices), `.Designer.cs`(`btnSubDevice` 1310, [원점 표시] 1470으로 이동), csproj 3항목 |
| KLARF 연관 분석 | `ENGUI/DefectMapAnalysis/frmGdsKlarfAnalysis.cs` `SelectDeviceForProduct`(같은 이름 없으면 Sub 로 대표 선택, 상태 줄 안내) + `FindRepresentativeDevice` / BSL `GdsKlarfAnalysis.AnalyzeWafer` Device 비었을 때 `ResolveMapDevice` |
| 검증 | `Probes/GdsSubDeviceProbe.ps1` **17/17 PASS** (D1~D5 / 오류 뒤 기존 설정 유지 / Map 재등록 후 다시 연결), `Probes/GdsSubDeviceUiSmoke.ps1` **11/11 PASS** (설정 창 읽기·추가·저장·대표 행 보호 / 조회 화면 버튼 / KLARF 자동 선택 / BSL 찾기). Probe 데이터(Factory PROBE) 정리 확인 |

- 화면 사용: GDS Map 조회 -> Device `U9` 선택 -> [Sub Device] -> `OMM`, `OMM_Hybrid` 입력 -> [저장]. Map 정보에 `Sub Device: OMM, OMM_Hybrid` 표시. Device 콤보에 `OMM_Hybrid` 입력 + Enter -> `U9` Map 이 열림.
- KLARF 연관 분석: Wafer Product `OMM_Hybrid` -> GDS Device `U9` 자동 선택, 상태 줄 "Sub Device OMM_Hybrid -> GDS Device U9".
- **E-Test (CLAUDE-029) 인계**: E-Test Device(파일명 `OMM`) -> GDS 대표 Device 는 `RO.GdsMapView.GetMapDeviceBySub(factory, etDevice)` 1행(DEVICE_ID)으로 찾는다. E-Test 파일은 이번에 수정하지 않았다.
- 배포(사용자): DB DDL(대상 DB), ServerBin `Interface` / `DSL` / `BSL`, ClientBin `Interface` / `RO` / `ENGUI`, GdsMap EXE. 빌드한 BSL / ENGUI 소스에는 다른 세션의 미커밋 변경(E-Test CLAUDE-029 등)도 들어 있다.
