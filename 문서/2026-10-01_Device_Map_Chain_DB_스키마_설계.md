# Device / Map / Chain DB 스키마 설계

작성일: 2026-10-01  
상태: 신규 `TQP_GDS_*` 테이블 DDL 작성 / 2026-10-01 DMSMGR 적용 완료 / 데이터 미적재

## 1. 목적과 범위

Device로 사용 중인 Map 개정본을 찾아 DB 데이터만으로 Layer / Grid / Map 도형을 복원한다. 같은 Map 개정본의 Chain A~Z를 각각 조회 / 수정 / 논리 삭제하고, 도형에서 소속 Chain을 역조회한다. 추후 AOI Defect가 월드 좌표의 도형과 겹치면 이 역조회 결과를 연관 후보로 사용한다.

현재 클라이언트 구현 범위는 PLACED 배치 추출 / 바이너리 형식 v2 직렬화 / 복원 / `GlSceneItem` 변환까지다. Service / Oracle 저장 / DB 단독 Map 화면 복원은 아직 구현되지 않았다. 기존 `Persistence/2026-09-29_device_map_oracle_schema.sql`은 Map 일부에 대한 미실행 초안이다. 아래 설계에 맞춘 신규 `TQP_GDS_*` DDL은 8절의 별도 SQL 파일로 작성하고 DMSMGR에 적용했다.

## 2. 키와 소속 관계

```text
Device 1 -> N Map Revision, 현재 사용 Map Revision은 최대 1개
Map Revision 1 -> N Layer / Source Element / Placed Element / Chain
Source Element 1 -> N Placed Element
Chain 1 -> N Chain Version
Chain Version 1 -> N Member / Layer Filter / Layer Rule / Edit
Placed Element 1 -> N Chain Member (다른 Chain에도 공유 가능)
```

- `MAP_REVISION_ID`: Device 안에서 도면의 한 개정본을 가리키는 영구 키. 모든 도형 / Chain 참조에 포함한다.
- `SOURCE_ELEMENT_ID`: 원본 Structure의 Element ID. Grid 행과 연결한다. 반복 배치된 한 원본의 ID는 동일하다.
- `PLACED_ELEMENT_ID`: `R0/R참조순번/E원본순번` 형태의 배치 도형 ID. 같은 원본이 여러 위치에 있으면 배치 ID는 다르다. **단독 전역 키가 아니며** `MAP_REVISION_ID`와 함께 사용한다.
- `CHAIN_ID`: 하나의 Chain이 개정되어도 유지되는 GUID. `CHAIN_CODE` A~Z/AA...는 같은 Map 개정본 안에서만 고유하다.
- `VERSION_NO`: 저장 확정 때 증가하는 Chain 버전. AOI 분석 결과에는 사용한 `MAP_REVISION_ID / CHAIN_ID / VERSION_NO`를 기록해야 당시 판단 기준을 다시 찾을 수 있다.

## 3. 1차 테이블

Oracle DDL에서는 GUID를 `VARCHAR2(36 BYTE)`, 수량을 `NUMBER`, 월드 좌표를 `BINARY_DOUBLE`, 좌표 배열을 `BLOB`으로 적용했다. 2026-10-01 기존 `TQP_` 업무 테이블의 감사 컬럼을 확인한 뒤, 신규 테이블의 생성/수정 시각은 `DATE`, 사용자 ID는 `VARCHAR2(30 BYTE)`로 맞췄다. 실제 Device 마스터가 `TQP_PRODUCT.PRODUCT`인지 `TQP_PROGRAM.DEVICE`인지 확인되지 않아 신규 Revision에는 `FACTORY + DEVICE_ID`를 두고 기존 업무 테이블로의 FK는 아직 만들지 않았다.

| 테이블 | PK / 주요 컬럼 | 역할 |
| --- | --- | --- |
| `DEVICE_MAP_REVISION` | `MAP_REVISION_ID`; `DEVICE_ID`, `REVISION_CODE`, `TOP_STRUCTURE`, `GDS_SHA256`, `USER_UNIT`, `DATABASE_UNIT`, `ID_GENERATOR_VERSION`, `POINTS_FORMAT_VERSION`, `STATUS`, `CREATE_TIME/USER`, `UPDATE_TIME/USER` | Device별 Map 개정본. 적재 중 `DRAFT`, 검증 후 `READY`, 사용 종료 시 `RETIRED` |
| `DEVICE_MAP_ACTIVE` | `DEVICE_ID`; `MAP_REVISION_ID`, `CREATE_TIME/USER`, `UPDATE_TIME/USER` | Device당 현재 사용하는 READY Map 한 건을 선택. 개정본 변경이 다른 도면을 덮어쓰지 않게 함 |
| `MAP_LAYER` | (`MAP_REVISION_ID`, `LAYER_ID`); `COLOR_ARGB`, `DISPLAY_NAME` | Map의 Layer 정의와 기본색. 사용자의 임시 표시 체크는 포함하지 않음 |
| `MAP_SOURCE_ELEMENT` | (`MAP_REVISION_ID`, `SOURCE_ELEMENT_ID`); `STRUCTURE_NAME`, `SOURCE_ORDINAL`, `ELEMENT_TYPE`, `LAYER_ID`, `DATA_TYPE`, `PARSER_DUPLICATE`, `SOURCE_PAYLOAD_VERSION`, `SOURCE_PAYLOAD_BIN` | Grid 원본 행 / SREF / AREF / 원본 속성 보관. 모든 49개 Grid 열을 재구성할 원본 정보의 저장 경계 |
| `MAP_PLACED_ELEMENT` | (`MAP_REVISION_ID`, `PLACED_ELEMENT_ID`); `SOURCE_ELEMENT_ID`, `LAYER_ID`, `DATA_TYPE`, `ELEMENT_TYPE`, `CLOSED_FLAG`, `PATH_WIDTH`, `PATH_TYPE`, `MIN_X/Y`, `MAX_X/Y`, `POINT_COUNT`, `POINTS_FORMAT_VERSION`, `POINTS_BIN`, TEXT 값 / 타입 / 폰트 / 정렬 | OpenGL Map과 Chain/Defect가 공통으로 참조하는 실제 배치 도형. Chain에 없는 배경 도형도 전부 저장 |
| `MAP_IMPORT_JOB` | `JOB_ID`; `MAP_REVISION_ID`, 예상 / 수신 원본 / 배치 / 좌표 수, 상태, 생성 / 갱신 시각 | 대량 Map 적재의 진행 상태 / 중단 후 재개 |
| `MAP_IMPORT_BATCH` | (`JOB_ID`, `STREAM_KIND`, `BATCH_NO`); `FORMAT_VERSION`, `PAYLOAD_SHA256`, 건수 / 좌표 수 / 바이트 수, `COMMIT_TIME` | 배치 중복 전송 판정 / 완료 배치 추적 |
| `CHAIN_MASTER` | (`MAP_REVISION_ID`, `CHAIN_ID`); `CHAIN_CODE`, `STATUS`, `CREATE_TIME/USER`, `UPDATE_TIME/USER`, `RETIRE_TIME` | Chain의 고정 ID와 논리 삭제 상태. 같은 Map에서 Chain Code 중복 금지 |
| `CHAIN_VERSION` | (`MAP_REVISION_ID`, `CHAIN_ID`, `VERSION_NO`); `NAME`, `COLOR_ARGB`, `APPLY_LAYER_RULES`, `CELL_SIZE`, `VERSION_STATUS`, `IS_CURRENT`, `CREATE_TIME/USER`, `UPDATE_TIME/USER` | 한 번 확정한 Chain 설정 스냅샷. 이름 / 색 / 탐색 옵션도 해당 버전에 고정 |
| `CHAIN_ELEMENT_MEMBER` | (`MAP_REVISION_ID`, `CHAIN_ID`, `VERSION_NO`, `PLACED_ELEMENT_ID`); `ROLE_CODE`, `INCLUSION_SOURCE`, `MARKER_X/Y` | 최종 포함 도형. `INPUT` 1개 필수 / `OUTPUT` 0~1개 / 나머지 `MEMBER`. 클릭한 단자 Marker 위치도 복원 |
| `CHAIN_LAYER_FILTER` | (`MAP_REVISION_ID`, `CHAIN_ID`, `VERSION_NO`, `LAYER_ID`) | 후보 찾기 당시 왼쪽에서 선택한 Layer 집합 |
| `CHAIN_LAYER_RULE` | (`MAP_REVISION_ID`, `CHAIN_ID`, `VERSION_NO`, `FIRST_LAYER_ID`, `SECOND_LAYER_ID`); `TOLERANCE` | 선택적 Layer 조합 연결 규칙. 양쪽 Layer 번호를 작은 값 / 큰 값 순서로 저장해 중복 방지 |
| `CHAIN_ELEMENT_EDIT` | (`MAP_REVISION_ID`, `CHAIN_ID`, `VERSION_NO`, `EDIT_SEQ`); `PLACED_ELEMENT_ID`, `ACTION_CODE`, `BATCH_EDIT_ID`, `CREATE_USER`, `CREATE_TIME` | 수동 추가 / 제외 / 복원 내역. 최종 포함 목록과 분리하여 재탐색 때 엔지니어 결정을 재적용 |

`MAP_SOURCE_ELEMENT.SOURCE_PAYLOAD_BIN`의 형식은 아직 만들지 않았다. Grid의 이름 / 원본 Bounds / PATH / TEXT / SREF / AREF / 변환 정보와 중복 제외 행까지 복원할 수 있는 버전별 계약을 먼저 정의해야 한다. `SOURCE_ELEMENT_ID`가 없는 중복 제외 행에도 충돌하지 않는 영구 ID를 부여해야 원본 전체 건수와 Grid를 검증할 수 있다.

`MAP_PLACED_ELEMENT.POINTS_BIN`은 각 점의 X/Y `double`을 little-endian 16바이트씩 담는 **행 단위 좌표 형식**이다. 현재 클라이언트의 `GMP1` **전송 배치 형식 v2**와 구별한다. Service는 전송 배치를 검증 / 해제해 도형 행으로 적재하도록 설계한다. `PATH_TYPE`은 기존 SQL 초안에 없지만 화면 복원에 필요하다. TEXT 문자열 / 위치 / 정렬을 저장하되, 현재 배치 데이터에는 GDS 변환 전 TEXT 좌표와 Structure 경로가 없으므로 Grid의 원본 정보는 Source 데이터에서 복원한다.

## 4. FK / 고유 제약 / 상태 검증

1. `DEVICE_MAP_REVISION`: `UNIQUE(DEVICE_ID, REVISION_CODE)`를 둔다. `DEVICE_MAP_ACTIVE`는 같은 Device의 Revision만 가리키는 복합 FK를 사용하고, READY 상태인지 Service 트랜잭션에서 확인한다. 기존 Device 마스터 FK는 실제 운영 키 확인 후 붙인다.
2. `MAP_PLACED_ELEMENT`: (`MAP_REVISION_ID`, `SOURCE_ELEMENT_ID`)를 Source에, (`MAP_REVISION_ID`, `LAYER_ID`)를 Layer에 FK로 연결한다. 입력 도형의 ID/Layer/좌표 수/실제 좌표 바이트 수를 검증한다.
3. `CHAIN_VERSION` 이하 모든 참조는 `MAP_REVISION_ID`를 같이 가진 복합 FK를 사용한다. 다른 개정본의 도형을 Chain에 잘못 연결할 수 없게 한다.
4. `CHAIN_ELEMENT_MEMBER`의 `PLACED_ELEMENT_ID`만으로는 UNIQUE를 두지 않는다. Element 하나가 Chain A와 B에 동시에 속할 수 있다. 한 Chain 버전 안에서는 같은 도형을 한 번만 저장한다.
5. 한 버전의 `INPUT`은 정확히 1개, `OUTPUT`은 0~1개이며 같은 도형을 두 역할로 중복 지정할 수 없다. 역할은 `INPUT / OUTPUT / MEMBER`만 허용한다. `MEMBER`는 GDS의 `PATH` 도형 종류와 다른 **Chain 소속 역할**이다. Oracle의 역할별 고유 인덱스로 최대 건수를 제한하고, 필수 Input 1개 / Output 및 Input의 최종 포함 여부는 확정 트랜잭션에서 검사한다. Output 없는 열린 Chain은 정상이다.
6. `CHAIN_VERSION`의 READY 스냅샷은 수정하지 않는다. 새 버전의 행을 모두 만든 뒤 이전 버전의 `IS_CURRENT=0`, 새 버전의 `IS_CURRENT=1`을 한 트랜잭션에서 처리한다. Chain 수정 충돌은 이전 현재 버전 번호를 비교해 거부한다. 한 Chain의 현재 버전은 최대 1개다.
7. Chain 삭제는 `CHAIN_MASTER.STATUS=RETIRED`로 처리한다. 다른 Chain / 공용 Map 도형 / 과거 버전은 삭제하지 않는다. Chain 표시 체크는 세션 UI 상태이며 DB의 업무 상태 컬럼으로 쓰지 않는다.
8. `CHAIN_ELEMENT_EDIT`의 `EXCLUDE` 대상은 최종 `CHAIN_ELEMENT_MEMBER`에 없어도 된다. Input / Output은 최종 포함 집합에서 제외할 수 없다. `BATCH_EDIT_ID`로 유사 도형 일괄 제외를 묶는다.
9. Map은 모든 Layer / Source / Placed 배치, 중복 키, 참조, 좌표 수를 확인한 뒤 `READY`로 전환한다. 부분 적재 중인 `DRAFT`는 일반 Device 조회와 Defect 분석에서 숨긴다.

## 5. 조회 인덱스와 대량 데이터 처리

| 조회 | 인덱스 후보 |
| --- | --- |
| Device의 Map 개정본 | `DEVICE_MAP_REVISION(DEVICE_ID, STATUS, REVISION_CODE)` / `DEVICE_MAP_ACTIVE(DEVICE_ID)` |
| 원본 Grid 행 -> 배치 도형 | `MAP_PLACED_ELEMENT(MAP_REVISION_ID, SOURCE_ELEMENT_ID)` |
| Map의 Layer별 순차 재조회 | `MAP_PLACED_ELEMENT(MAP_REVISION_ID, LAYER_ID, PLACED_ELEMENT_ID)` |
| Map의 Chain 목록 | `CHAIN_MASTER(MAP_REVISION_ID, STATUS, CHAIN_CODE)` |
| Chain -> 도형 | `CHAIN_ELEMENT_MEMBER` PK 순서 |
| Defect가 겹친 도형 -> Chain | `CHAIN_ELEMENT_MEMBER(MAP_REVISION_ID, PLACED_ELEMENT_ID, CHAIN_ID, VERSION_NO)` |
| 실패 후 배치 재개 | `MAP_IMPORT_BATCH` PK (`JOB_ID`, `STREAM_KIND`, `BATCH_NO`) |

좌표 배열은 화면 픽셀 / OpenGL VBO 번호가 아니라 월드 좌표로 저장한다. 예를 들어 좌표 4,300만 개라면 좌표 본문만 약 6.9억 바이트(점당 16바이트)이고 행 / LOB / 인덱스 용량은 별도다. 따라서 단일 트랜잭션 / 무제한 `DataTable` 대신 제한된 배치 적재와 확정 후 공개를 전제로 한다. 실제 BLOB 저장 방식, LOB 테이블스페이스, 인덱스 크기, 파티션 / 공간 인덱스 필요성은 대표 Device 측정 후 정한다. Defect 영역 검색 인덱스는 AOI 좌표 정합과 검색 패턴이 확정된 뒤 설계한다.

## 6. 저장 / 재조회 절차

1. Device / Revision / 원본 파일 SHA-256 / TopStructure / 단위 / ID 생성기 버전을 고정하고 `DRAFT` Map과 Import Job을 만든다.
2. Layer -> Source -> 모든 Placed 도형 순서로 배치 적재한다. 배치 행과 배치 완료 기록은 같은 트랜잭션에서 Commit한다. 동일 Job/종류/번호/해시는 재전송 성공으로, 같은 번호/다른 해시는 오류로 처리한다.
3. 전체 건수 / 좌표 수 / FK / 중복 ID를 검증한 뒤 Map을 `READY`로 바꾼다. 사용 개정본 교체는 별도 트랜잭션으로 처리한다.
4. READY Map에 대해 Chain Version의 최종 Member / Layer Filter / Rule / Edit를 저장하고 Input / Output 제약을 검사한 뒤 현재 버전을 전환한다. 다른 Chain은 변경하지 않는다.
5. Device 조회 시 현재 READY Map의 Layer / Source / Placed를 같은 개정본에서 읽어 Map / Grid를 복원한다. 이후 현재 Chain Version의 Member를 도형 ID로 매칭하고 선택적 필터 / 수동 결정을 복원한다. 누락 참조는 임의의 다른 도형으로 대체하지 않고 오류로 표시한다.

## 7. 기존 초안 대비 수정점 / DDL 전 확인

- 기존 Map SQL 초안에는 `PATH_TYPE`, 행 좌표 형식 버전, 사용 개정본 테이블, Chain 관련 테이블 / 인덱스가 없었다. 해당 미실행 초안은 그대로 두고, 필요한 내용을 신규 `TQP_GDS_*` DDL에 반영했다. 기존 `TQP_MAPDEF` 등 업무 테이블은 수정하지 않았다.
- 현재 화면은 Input 1개 / Output 0~1개다. 오래된 문서의 복수 Input/Output 설계보다 **현재 화면 검증 로직**을 1차 저장 제약으로 사용한다.
- 기존 Grid의 중복 제외 원본 행에 영구 `SOURCE_ELEMENT_ID`가 아직 없고, Source 바이너리 형식도 없다. 이 두 가지가 해결되기 전에는 Grid 전체 재조회 완료를 주장할 수 없다.
- 현재 Map Flatten은 SREF만 처리하고 AREF 배치는 펼치지 않는다. 실제 도면에 AREF가 있으면 조용히 누락한 READY Map을 만들지 않도록 등록 전 탐지 / 지원 범위 판정이 필요하다.
- Device 마스터 실제 PK / Oracle 스키마 / 권한 / 컬럼 문자셋 / Revision 명명 규칙 / 현재 사용 개정본 교체 권한을 확인한 뒤 DDL을 확정한다.
- AOI Defect 원본 / 좌표 변환 / Element Hit 이력 테이블은 Map/Chain 왕복 복원이 확인된 다음 단계에서 분리 설계한다. 위치 겹침은 전기적 불량 원인의 확정이 아니라 연관 후보이다.

## 8. 2026-10-01 / DMSMGR 실제 적용 기록

대상은 Docker Oracle 19c의 `MESMGR` Service / `DMSMGR` 스키마다. 기존 `TQP_` 테이블 24개와 이름이 겹치지 않는 신규 테이블만 만들었다. 작업 전 동일 이름의 객체 / 제약조건은 각각 0개였으며, 실행 SQL에 기존 테이블에 대한 `ALTER / DROP / INSERT / UPDATE / DELETE`가 없는 것을 검사했다.

| 설계명 | 실제 테이블명 |
| --- | --- |
| `DEVICE_MAP_REVISION` | `TQP_GDS_MAP_REV` |
| `DEVICE_MAP_ACTIVE` | `TQP_GDS_MAP_ACTIVE` |
| `MAP_LAYER` | `TQP_GDS_LAYER` |
| `MAP_SOURCE_ELEMENT` | `TQP_GDS_SOURCE_EL` |
| `MAP_PLACED_ELEMENT` | `TQP_GDS_PLACED_EL` |
| `MAP_IMPORT_JOB` | `TQP_GDS_IMPORT_JOB` |
| `MAP_IMPORT_BATCH` | `TQP_GDS_IMPORT_BATCH` |
| `CHAIN_MASTER` | `TQP_GDS_CHAIN` |
| `CHAIN_VERSION` | `TQP_GDS_CHAIN_VER` |
| `CHAIN_ELEMENT_MEMBER` | `TQP_GDS_CHAIN_MEMBER` |
| `CHAIN_LAYER_FILTER` | `TQP_GDS_CHAIN_LAYER` |
| `CHAIN_LAYER_RULE` | `TQP_GDS_CHAIN_RULE` |
| `CHAIN_ELEMENT_EDIT` | `TQP_GDS_CHAIN_EDIT` |

적용 파일은 [Map DDL](../GdsMapControl/Persistence/2026-10-01_tqp_gds_map_schema.sql)과 [Chain DDL](../GdsMapControl/Persistence/2026-10-01_tqp_gds_chain_schema.sql)이다. [검증 SQL](../GdsMapControl/Persistence/2026-10-01_tqp_gds_verify.sql)에서 신규 테이블 13개, PK 13개, FK 18개, Unique 제약 4개, 인덱스 23개 모두 활성/유효함을 확인했다. 기존 `TQP_GDS_` 외 `TQP_` 테이블은 24개이며 새 테이블 13개의 행 수는 모두 0이다. `PATH_TYPE`과 `POINTS_BIN` 컬럼도 확인했다.

이 단계는 **빈 스키마 생성**까지다. Chain의 필수 Input 1개, Map/Chain READY 상태 전환, Device 마스터 대응, Source 원본 바이너리 형식, 대량 적재 및 DB 단독 화면 복원은 Service 구현과 함께 검증해야 한다. 확정 전에는 이 DDL을 다시 실행하지 않는다.

## 9. 2026-10-01 / 기존 TQP 감사 컬럼과 정렬

`DMSMGR`의 기존 `TQP_` 테이블 24개를 조회했다. `CREATE_TIME DATE`는 5개, `UPDATE_TIME DATE`는 6개 테이블에 있고, `CREATE_USER/UPDATE_USER`는 대부분 `VARCHAR2(30 BYTE)`이다. `TQP_MAPDEF`만 생성 쪽에 `INSERT_TIME/INSERT_USER`를 쓴다. 따라서 신규 `TQP_GDS_*`에는 다수의 기존 테이블과 같은 `CREATE_TIME/CREATE_USER/UPDATE_TIME/UPDATE_USER`를 사용한다. `CREATE_TIME`은 `SYSDATE` 기본값, 사용자 ID는 30바이트다. `COMMIT_TIME`과 `RETIRE_TIME`은 일반 수정 시각과 다른 이벤트 시각이라 별도로 남겼다. `CHAIN_STATUS`는 삭제 플래그가 아니라 `ACTIVE/RETIRED` 상태이므로 `DELETE_FLAG`로 바꾸지 않았다.

기존 신규 테이블 13개가 모두 0건인 것을 확인한 뒤, [감사 컬럼 변경 SQL](../GdsMapControl/Persistence/2026-10-01_tqp_gds_audit_migration.sql)을 적용했다. 실행 중 이미 `NOT NULL`인 컬럼에 같은 제약을 재지정한 구문이 Oracle에서 거부되어, 앞의 Map Revision 컬럼명 2개가 변경된 상태에서 중단되었다. 구문을 고친 후 [이어서 적용한 SQL](../GdsMapControl/Persistence/2026-10-01_tqp_gds_audit_migration_resume.sql)로 나머지를 완료했다. 이후 [사용자 ID 길이 변경 SQL](../GdsMapControl/Persistence/2026-10-01_tqp_gds_audit_user_length.sql)로 30 `CHAR`를 기존 업무 테이블과 같은 30 `BYTE`로 맞췄다. 이 세 파일은 적용 이력이며 재실행하지 않는다. 신규 설치용 Map/Chain CREATE 스크립트도 최종 컬럼 정의로 수정했으나 이미 만든 테이블에는 다시 실행하지 않는다.

`UPDATE_TIME`은 DB의 `DEFAULT`만으로 매 UPDATE마다 자동 변경되지 않는다. 향후 Service의 상태 전환/현재 버전 변경 트랜잭션에서 `UPDATE_USER`와 함께 명시적으로 기록한다. 불변 대량 도형 행에는 행마다 감사 컬럼을 추가하지 않았다.

[감사 컬럼 조회 SQL](../GdsMapControl/Persistence/2026-10-01_tqp_audit_columns.sql)과 [변경 후 검증 SQL](../GdsMapControl/Persistence/2026-10-01_tqp_gds_audit_verify.sql)로 최종 확인했다. 신규 테이블 13개 / 기존 `TQP_` 테이블 24개, 신규 감사 시각 `DATE` 컬럼 13개 / 30 `BYTE` 사용자 컬럼 11개 / 이전 `_AT`, `_BY` 이름 0개다. 신규 테이블의 PK 13개 / FK 18개 / UNIQUE 4개는 모두 `ENABLED/VALIDATED`이고, 인덱스는 모두 `VALID`다. 신규 테이블 행 수는 계속 0개다.
