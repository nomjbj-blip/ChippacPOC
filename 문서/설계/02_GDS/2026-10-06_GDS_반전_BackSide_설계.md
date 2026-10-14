# GDS 반전(Back side 보기) 설계

작성: 2026-10-06 Claude (CLAUDE-027) / 상태: **2026-10-07 [Front / Back 전환 설계](2026-10-07_GDS_FrontBack_전환_설계.md)로 대체됨**(Setup 반전 제거 / View에서 선택). 9절 구현은 대체 설계 1단계에서 정리

근거: [2026-10-06 데이터기준 협의 회의록](../../../회의록/2026-10-06_데이터기준_협의/2026-10-06_데이터기준_협의_회의록.docx) "Front side와 Back side 기준" 안건, 녹취 원문 245~261행.
관련: [GDS Map DB 저장 설계](2026-10-02_GDS_Map_DB저장_설계.md) 21절, [화면 역할 재정의 설계](2026-10-06_GDS_Map_화면역할_재정의_설계.md), `DMS/Assistant/기능/F07_Correlation.md`

## 1. 회의 결정과 요구

| 구분 | 내용 |
| --- | --- |
| 사실 | GDS 원본은 **Front side(Top view)** 기준이다. 이번에 받을 KLARF와 E-Test(CSV)는 **Back side** 기준이다. 공정은 Back side부터 시작한 뒤 뒤집어 Front side를 진행한다 |
| 결정 | POC에서는 **GDS를 반전해 Back side 형태로 Map을 만들고, 그 위에 Chain을 설정**한다. KLARF / E-Test는 변환 없이 그대로 쓴다 (정병주 프로) |
| 순서 | Back side 단계 데이터를 먼저 셋업하고 Front side 단계는 이후 |
| 후속 검토 | 공정 단계에 따라 엔지니어가 Front / Back을 선택하는 방식(정병주 프로 검토). 단계별 Front / Back 구분 테이블은 연구소(서승환 책임)가 전달 예정 |
| 범위 밖 | E-Test Notch 3시 방향 회전(180도 등) / 다이 인덱스 대응은 **좌표 대응 테이블**(연구소 전달 예정)로 별도 처리한다. 이 설계는 GDS 도형 반전만 다룬다 |

## 2. 현재 소스에서 확인한 사실 (2026-10-06)

- 화면 도형 좌표는 `GdsMapControl.ShowStructure` -> `FlattenStructure`에서 만든다. SREF / AREF 배치 변환은 `System.Drawing.Drawing2D.Matrix`(**float**)로 계산하고, 변환이 없는 최상위 도형은 **원본 좌표 배열을 그대로 공유**한다(`TransformPoints`가 src 반환).
  - 반전을 이 행렬에 넣으면 모든 도형이 float 계산을 거쳐 정밀도가 떨어진다(µm 소수 5자리 좌표 손실). -> **Flatten 이후 double로 반전**해야 한다.
  - 원본 배열을 직접 바꾸면 원본 Grid / SOURCE 저장(원본 GDS 기록)까지 반전된다. -> **새 배열**로 반전해야 한다.
- DB 저장(`GdsMapForm_DbSave`)은 화면 도형(PLACED: WorldPoints / Bounds)과 원본(SOURCE: 파서 결과 Payload)을 따로 보낸다. 반전은 PLACED에만 적용되고 SOURCE는 원본 그대로 남는다.
- View(`ShowPlacedElements`)와 Chain(탐색 / 저장 / 불러오기)은 PLACED 좌표와 ID만 쓴다. PLACED가 반전되어 있으면 추가 변경 없이 Back side로 보이고 동작한다(도형 ID는 반전과 무관).
- 도형 겹침 판단(Chain 탐색)은 반전에 대해 대칭이라 결과(연결 집합)가 같아야 한다(구현 후 Probe로 확인).
- 반전하면 다각형 꼭짓점 순서(시계 / 반시계)가 뒤집힌다. 채우기 삼각형 분할이 방향에 의존하는지는 구현 1단계에서 화면 / Probe로 확인한다.

## 3. 반전 정의 (안)

```text
좌우 반전(Y축 기준, 권장 기본):  X' = (MinX + MaxX) - X ,  Y' = Y
상하 반전(X축 기준):             X' = X ,  Y' = (MinY + MaxY) - Y
MinX..MaxX / MinY..MaxY = 최상위 Structure 전체 도형 범위(펼친 뒤 화면 도형 전체의 Bounds)
```

- **기준점을 도면 범위 중심**으로 두어 반전 후에도 도면이 같은 좌표 범위에 그대로 남는다(음수 좌표 / 화면 위치 이동 없음). KLARF 다이 내부 좌표와 맞출 때 다이 범위가 그대로라서 대응 테이블 적용이 단순하다.
- 적용 대상: 도형 좌표(WorldPoints), Bounds(재계산), TEXT 위치. TEXT 글자 모양은 뒤집지 않는다(읽을 수 있게 위치만 반전).
- 원본 Grid 탭은 원본 GDS 좌표 그대로 보여 준다(SOURCE와 같음). 화면 상태줄 마우스 좌표는 반전된 좌표.

## 4. 처리 흐름

```text
[GDS Map Setup]
 GDS 파일 열기 -> 보기 기준 선택(Front 원본 / Back 좌우 반전 / Back 상하 반전, 기본 = 7절 결정 1)
 -> Flatten(원본) -> 반전(double, 새 배열) -> 화면 표시 / Layer 도형 수
 -> [DB 저장]: PLACED = 반전 좌표, SOURCE = 원본, Map 헤더에 보기 기준 / 축 / 기준점 기록
[GDS Map View]
 Map 정보에 "보기 기준: Back side(좌우 반전)" 표시 -> 도형 / Chain 그대로(이미 반전된 PLACED)
```

- 보기 기준을 바꾸면 같은 파일을 다시 Flatten + 반전한다(샘플 약 9초). 저장 중에는 변경을 막는다.
- 이미 저장된 Device와 보기 기준이 다르면 저장 전 기존 데이터 확인창에 "DB: Front / 지금: Back"을 함께 보여 준다(삭제 후 재등록 = Chain도 삭제).

## 5. 저장 / 스키마

| 항목 | 안 |
| --- | --- |
| PLACED_EL | 반전된 좌표 / Bounds 저장(컬럼 변경 없음) |
| SOURCE_EL | 원본 그대로(변경 없음) |
| TQP_GDS_MAP | 컬럼 추가(ALTER, 기본값으로 기존 행 호환): `VIEW_SIDE` VARCHAR2(10) DEFAULT 'FRONT' (FRONT / BACK), `MIRROR_AXIS` VARCHAR2(1) NULL (X = 좌우 / Y = 상하), `MIRROR_CENTER` BINARY_DOUBLE NULL (반전 기준 좌표: 좌우면 (MinX+MaxX)/2) |
| 서비스 | `BeginMapImport` ImportInfo 11개 -> 14개(보기 기준 / 축 / 기준점), `GetMapInfo` / View `GetMapInfo` 반환 열 추가. 기존 11개 호출은 FRONT로 받는다(Probe 호환) |

- 반전 정보를 헤더에 남겨야 나중에 KLARF / E-Test 좌표 대응, Front 단계 데이터 대응, "이 Map은 원본과 어떻게 다른가"를 판단할 수 있다.
- **주의(작업 조정)**: `GdsMapImport`(BSL / Interface / RO)와 `GdsMapForm_DbSave.cs`는 CLAUDE-023(DB 저장 성능)이 수정한 파일이다. 구현 전 CLAUDE-023 진행 여부를 확인한다.

## 6. 구현 단계 (작은 단위)

| 단계 | 내용 | 확인 |
| --- | --- | --- |
| 1 | `GdsMapControl`: 반전 옵션(`ShowStructure(lib, name, mirror)`), Flatten 후 double 반전(새 배열 / Bounds / TEXT), 반전 정보 속성 | Probe: 반전 좌표 = 기대식, 원본 배열 불변, 두 번 반전 = 원본, 도형 수 / 면적 동일, 반전 전후 Chain 탐색 연결 집합 동일 / 화면 채우기 확인 |
| 2 | Setup 화면 보기 기준 선택(Designer) + 다시 그리기 | DMS 화면 |
| 3 | DB: TQP_GDS_MAP ALTER(사용자 승인), XML / BSL / Interface / RO, 저장 시 헤더 기록 | Probe: 저장 -> GetMapInfo 보기 기준 / 축 / 기준점, PLACED 반전 좌표 왕복 |
| 4 | View Map 정보에 보기 기준 표시 | DMS 화면 |
| 5 | 문서 / F07 Correlation에 좌표 기준 연결 메모 | — |

## 7. 결정이 필요한 사항

| # | 질문 | 권장 | 다른 안 / 비고 |
| --- | --- | --- | --- |
| 1 | 반전 축 | **좌우 반전(Y축 기준, X' = MinX+MaxX−X)을 기본으로, Setup에서 상하도 고를 수 있게** | 상하 반전 고정. 웨이퍼를 어느 축으로 뒤집는지는 연구소 기준 확인 필요 [확인 필요] — 그래서 고정하지 않고 선택으로 둔다 |
| 2 | 반전 기준점 | **도면 전체 범위의 중심**(반전 후 같은 좌표 범위 유지) | 원점(0) 기준(X' = −X, 음수 좌표). KLARF 다이 좌표 대응 테이블이 원점 기준이면 바꿀 수 있게 헤더에 기준점을 기록 |
| 3 | 저장 방식 | **반전된 좌표로 PLACED 저장 + 헤더에 보기 기준 기록**(회의 결정 "반전한 Map 위에 Chain 설정"과 일치, View / Chain 무변경) | 원본 좌표 저장 + View에서 표시할 때 반전(나중 Front / Back 전환이 쉬우나 Chain Marker / 분석 좌표 공간이 섞임) |
| 4 | Map 헤더 컬럼 추가(ALTER TQP_GDS_MAP) | **추가**(VIEW_SIDE / MIRROR_AXIS / MIRROR_CENTER, 기존 행은 FRONT) | 스키마 변경 없이 화면에서만 반전(저장된 Map이 Front인지 Back인지 DB로 알 수 없음) |
| 5 | Setup 기본 보기 기준 | **Back side(결정 1의 축)** — POC 데이터가 Back side 기준 | Front(원본) 기본 |
| 6 | 이미 저장된 Map(Front 좌표) | **그대로 두고**, Back으로 쓰려면 삭제 후 재등록(Chain 삭제 안내) | 일괄 변환 기능(범위 밖) |

Device당 Map 1개(키 FACTORY + DEVICE_ID) 구조라서 같은 Device의 Front / Back Map을 동시에 둘 수는 없다. 회의의 후속 검토(단계별 Front / Back 선택)가 확정되면 키 확장 또는 표시 시 반전 방식으로 별도 설계한다.

## 8. 2026-10-06 사용자 결정 반영 (최신 기준)

사용자 지시: "GDS File은 Front이며 이 부분을 Back, Front 바꿔가며 처리 가능하도록 해야 한다. 좌우 반전만 하면 된다." / 반전 기준 = "GDS 원점 X=0" / "Map Setup 화면에 Loading되는 File은 Front 기준이다. DB 저장 시 combo box로 선택해서 저장할 수 있도록 해줘. front or back side".

| 항목 | 확정 |
| --- | --- |
| 반전 | **좌우 반전만**, **GDS 원점 기준 X' = −X**, Y' = Y. 부호만 바뀌므로 두 번 반전하면 비트 단위로 원본과 같고, 저장 / 조회 Layer 범위와 무관하다. 반전 기준점 저장 불필요 |
| Setup 화면 | 불러온 GDS는 **Front 기준 그대로 표시**(Layer 필터 / 원본 Grid 변화 없음). DB 저장 영역에 **콤보박스 [저장 기준: Front side(원본) / Back side(좌우 반전)]** |
| DB 저장 | 선택이 Back이면 **PLACED 도형 좌표 / Bounds / TEXT 위치를 X 부호 반전해 저장**(새 배열, 화면 도형과 원본은 그대로). SOURCE(원본 Grid)는 항상 원본. 선택한 기준을 Map 헤더 `VIEW_SIDE`(FRONT / BACK)에 기록 |
| Device 기준 | Device당 Map 1개이므로 Device마다 저장할 때 고른 기준 하나로 저장된다. 기준을 바꾸려면 기존 Map 삭제 후 다른 기준으로 재등록(기존 확인창에 "DB: Front / 이번 선택: Back" 표시, Chain 삭제 안내) |
| View 화면 | 저장된 좌표를 그대로 그린다(Back으로 저장한 Map은 Back으로 보임). Map 정보에 "저장 기준: Back side(좌우 반전)" 표시. Chain은 그 기준 위에서 설정 / 저장(추가 변경 없음) |
| Chain 탐색 | 좌우 반전은 겹침 판단에 대칭이라 같은 연결 결과(Probe로 확인) |

**구현 위치**
- 반전: `GdsMapControl_Persistence.cs` 저장용 추출(`VisitPlacedElements` / `VisitPlacedElementBatchesAsync` -> `CreatePersistenceData`)에 `mirrorX` 옵션. WorldPoints는 새 배열(−X), Bounds = (−MaxX, MinY, −MinX, MaxY), TEXT는 WorldPoints[0]이 위치. 화면 도형은 바꾸지 않는다.
- Setup: `GdsMapForm.Designer.cs`에 콤보(`cboDbSide`, DB 저장 컨트롤 옆, 호스트 모드에서만), `GdsMapForm_DbSave.cs`에서 선택값으로 예상 건수 / 배치 추출 / 저장 정보 / 확인창 문구.
- DB: `ALTER TABLE TQP_GDS_MAP ADD VIEW_SIDE VARCHAR2(10) DEFAULT 'FRONT' NOT NULL` + CHECK(FRONT / BACK). 기존 행은 FRONT. **DDL은 사용자 승인 후 로컬 시험 DB에 적용**하고 SQL 파일을 `Persistence/`에 남긴다.
- 서비스: `BeginMapImport` ImportInfo 11개(기존, FRONT로 처리) 또는 12개(12번째 = VIEW_SIDE). XML `CREATE_MAP` / `SELECT_MAP_INFO` / 조회 화면 Map 정보 쿼리에 VIEW_SIDE. 메서드 시그니처 변경 없음(기존 Probe 호환).
- View: `GdsMapViewForm` Map 정보 문구에 저장 기준.

**구현 단계**: ① 저장용 반전 추출 + Probe(반전식 / 원본 불변 / 두 번 반전 = 원본 / 반전 Map 탐색 결과 동일) -> ② DDL + XML / BSL(헤더 기록) + Probe(Back 저장 -> PLACED = −X / SOURCE 원본 / VIEW_SIDE) -> ③ Setup 콤보 + 확인창 -> ④ View 표시 -> 빌드 / 반영 / 문서.

**작업 조정**: ② ③은 CLAUDE-023이 수정한 `GdsMapImport`(BSL) / `GdsMapForm_DbSave.cs`를 바꾼다. CLAUDE-023 행은 12:25 이후 갱신이 없고 화면 확인까지 기록되어 있어 진행 중이 아닌 것으로 보지만, 구현 시작 전 파일 수정 시각을 다시 확인한다.

## 9. 2026-10-06 / 구현 (CLAUDE-027)

**DB**: `Persistence/2026-10-06_tqp_gds_map_view_side.sql` -> 로컬 docker DMSMGR 적용(사용자 승인 "승인"). `TQP_GDS_MAP.VIEW_SIDE VARCHAR2(10) DEFAULT 'FRONT' NOT NULL` + `CK_GDS_MAP_VIEW_SIDE`(FRONT / BACK). 기존 Map 9건 FRONT. 고객 DB 적용은 별도.

**소스**
- `Controls/GdsMapControl_Persistence.cs`: `VisitPlacedElements(layerIds, mirrorX, visitor)` / `VisitPlacedElementBatchesAsync(layerIds, mirrorX, ...)` 오버로드(기존 시그니처는 mirrorX = false로 위임), `CreatePersistenceData(item, mirrorX)`, `MirrorPoints`(새 배열, X 부호만 변경 - GPoint 생성자 반올림을 거치지 않음) / `MirrorBounds`(-MaxX, MinY, -MinX, MaxY). TEXT는 위치만 반전.
- `GdsMapForm.Designer.cs`: DB 저장 영역 왼쪽에 `lblDbSide`("저장 기준") / `cboDbSide`(DropDownList: "Front side(원본)" / "Back side(좌우 반전)"). `GdsMapForm_DbSave.cs`: 기본값 Back side(`InitializeDbSaveState`), 호스트 모드에서만 표시 / 저장 중 비활성, 저장 시작 시 선택을 한 번 읽어 PLACED 배치 추출에 mirrorX, `BeginMapImport` 12번째 항목 VIEW_SIDE, 기존 데이터 확인창에 "저장 기준: DB … / 이번 선택 …", 완료 메시지 / 상태줄에 저장 기준. 예상 건수 / SOURCE는 반전과 무관.
- `02_DMS/DACrux.SEMDMS.BSL/GdsMapImport.cs`: `BeginMapImport` 11개(FRONT) 또는 12개(VIEW_SIDE 검증 FRONT / BACK) 항목, `CREATE_MAP`에 VIEW_SIDE 전달. `iGdsMapImport.cs`는 주석만(시그니처 동일, DLL 재빌드 불필요).
- `08_DBQuery/ORACLE/TQP_GDS_MAP.xml`: `SELECT_MAP_INFO`에 `M.VIEW_SIDE`, `CREATE_MAP`에 VIEW_SIDE 열 / `:view_side` 파라미터. **새 XML은 새 BSL(9개 인자)과 함께 써야 한다**(예전 BSL + 새 XML이면 Map 저장 실패) -> BSL을 바로 반영함.
- `GdsMapViewForm.cs`: Map 정보에 "저장 기준: Front side(원본) / Back side(좌우 반전)". 도형은 저장 좌표 그대로 그린다.

**검증**: BSL / GdsMap TEMP 빌드 성공. 신규 `Probes/GdsImportBackSideProbe.ps1` 6 PASS(TEMP BSL, 반영 후 운영 BSL로 재실행 PASS): Layer 2·6 도형 14,005 / 좌표 28,351의 반전 X 비트 일치(X / Y / Bounds 불일치 0), 반전 추출 후 화면 도형 불변 / 두 번 반전 = 원본, Back 저장 확정 OK + VIEW_SIDE = BACK + SOURCE 14,008 / PLACED 14,005 일치(1.6초), DB 재조회 좌표 = 반전 좌표(불일치 0), Back DB 도형 Chain 탐색 = Front 파일 탐색(11 = 11), 기존 11개 호출 = FRONT.

**반영**: 17:13 ServerBin BSL `4a811ffb` -> `b71931e4`, GdsMap EXE `aaa9e6cc` -> `f68c22d6`(3단계 진행 표시 포함). 반영 전 MD5가 이전 기록과 달라 VS 빌드(출력 경로 운영 폴더)가 있었던 것으로 본다. SEMDMS Service / DACrux 재시작 후 적용.

**미확인**: DMS 화면(콤보 배치 / Back 저장 / View 저장 기준 표시 / Back Map에서 Chain 설정), 화면 채우기(꼭짓점 방향 반전)는 Probe로 확인하지 않음(View에서 Back Map 표시로 확인 필요), Service Remoting 경유.
