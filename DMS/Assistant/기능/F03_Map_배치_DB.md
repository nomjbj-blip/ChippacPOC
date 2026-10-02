# F03 / Map 배치와 DB 저장 기반

확인일: 2026-10-02 / 상태: 클라이언트 배치 소스 확인 / DB 적용은 과거 문서 기록

## 목적과 파일

큰 GDS 도면을 제한된 배치로 저장하고 같은 Map 개정본의 도형 / Chain을 복원할 기반을 만든다.

| 파일 | 역할 |
| --- | --- |
| [GdsMapControl_Persistence.cs](../../06_CoreLibSource/GdsMapControl/Controls/GdsMapControl_Persistence.cs) | 배치 도형 추출 / 취소 / Map 변경 검사 |
| [MapPlacedElementBatchBuilder.cs](../../06_CoreLibSource/GdsMapControl/Persistence/MapPlacedElementBatchBuilder.cs) | 도형 수와 추정 크기로 배치 제한 |
| [MapPlacedBatchPayload.cs](../../06_CoreLibSource/GdsMapControl/Persistence/MapPlacedBatchPayload.cs) | GMP1 v2 직렬화 / SHA-256 |
| [MapPlacedBatchReader.cs](../../06_CoreLibSource/GdsMapControl/Persistence/MapPlacedBatchReader.cs) | 배치 검증 / 복원 |
| [MapPlacedSceneItemFactory.cs](../../06_CoreLibSource/GdsMapControl/Persistence/MapPlacedSceneItemFactory.cs) | 복원한 도형을 GlSceneItem으로 변환 |
| [Map DDL](../../06_CoreLibSource/GdsMapControl/Persistence/2026-10-01_tqp_gds_map_schema.sql) / [Chain DDL](../../06_CoreLibSource/GdsMapControl/Persistence/2026-10-01_tqp_gds_chain_schema.sql) | 신규 테이블 정의 |
| [DB 설계 / 적용 기록](../../../문서/2026-10-01_Device_Map_Chain_DB_스키마_설계.md) | 키 / 상태 / 제약 / 과거 적용 결과 |

## 구현과 설계 경계

- `VisitPlacedElementBatchesAsync`는 기본 2,000개 도형 / 추정 8 MiB로 배치를 나누며 취소와 Map 버전 변경을 검사한다. 실제 전송 제한은 직렬화된 바이트 길이로 검증한다.
- 현재 배치 형식 상수는 `FormatVersion = 2`다. 과거 v1 설명을 그대로 사용하지 않는다.
- `GMP1`은 여러 도형을 포함하는 전송 배치 형식이다. DB의 `POINTS_BIN`은 도형 한 행의 X/Y double 배열 형식이므로 둘을 그대로 같은 BLOB으로 취급하지 않는다.
- `MapPlacedSceneItemFactory`가 있어도 DB 데이터만으로 Layer / 원본 Grid / 전체 Map을 복원하는 기능이 완료된 것은 아니다.
- 원본 SOURCE 전송 형식과 중복 제외 원본 행의 영구 ID가 남아 있다. 전체 Grid 복원은 이 계약까지 필요하다.

## DB 이름과 저장 기준

테이블별 키 / 관계 / 생성 SQL / 감사 컬럼 변경 이력은 [DB 스키마 생성 현황](../DB/스키마_생성_현황.md)을 함께 읽는다.

현재 설계의 물리 테이블은 다음 13개다.

| 구분 | 테이블 |
| --- | --- |
| Map | TQP_GDS_MAP_REV / TQP_GDS_MAP_ACTIVE / TQP_GDS_LAYER / TQP_GDS_SOURCE_EL / TQP_GDS_PLACED_EL |
| Import | TQP_GDS_IMPORT_JOB / TQP_GDS_IMPORT_BATCH |
| Chain | TQP_GDS_CHAIN / TQP_GDS_CHAIN_VER / TQP_GDS_CHAIN_MEMBER / TQP_GDS_CHAIN_LAYER / TQP_GDS_CHAIN_RULE / TQP_GDS_CHAIN_EDIT |

- Device 마스터 대응은 미확정이다. Factory / Device 키를 기존 운영 키와 대조해야 한다.
- Map 개정본은 DRAFT -> 검증 후 READY로 공개하는 설계다. READY 전환과 전체 도형 적재 Service는 개정본 헤더 등록과 별도다.
- Chain ID는 유지하고 확정 변경은 새 VERSION_NO로 저장한다. 분석 결과에는 사용한 Map Revision / Chain ID / Version을 남긴다.
- 도형의 배치 ID는 개정본 안에서 유효하다. Source ID는 반복 배치 도형과 원본 Grid를 연결한다.
- 감사 컬럼은 설계서 기준 DATE / 사용자 ID 30 BYTE다. 실제 대상 DB와 대조한다.

## 검증 기록의 범위

10/01 설계서에는 DMSMGR에 13개 테이블 적용과 당시 0건 확인, 감사 컬럼 변경 완료가 기록되어 있다. **이번 작업은 DB에 접속하지 않았으므로 현재 행 수 / 제약 / 적용 상태는 미확인**이다. Migration과 resume SQL은 적용 이력이므로 재실행하지 않는다.

[MapPlacedElementBatchProbe.ps1](../../06_CoreLibSource/GdsMapControl/Probes/MapPlacedElementBatchProbe.ps1) / [MapPlacedBatchPayloadProbe.ps1](../../06_CoreLibSource/GdsMapControl/Probes/MapPlacedBatchPayloadProbe.ps1)이 존재한다. 현재 Debug EXE와 형식 v2 기준으로 다시 실행해야 현재 검증 결과로 기록할 수 있다.

## 다음 단계

**2026-10-02 설계:** GDS Map Setup 화면의 DB 저장 설계는 [GDS Map DB 저장 설계](../../../문서/2026-10-02_GDS_Map_DB저장_설계.md)를 따른다. 요점: 전송은 GMP1 대신 행 DTO 배열(BSL이 .NET 4.0이라 4.8 Reader 참조 불가) / 행 INSERT는 키 중복 시 건너뛰고 배치 기록은 마지막 / BLOB은 Middleware `Execute1(object[])` 검증이 1단계 / 파서 중복 행 Source ID는 `<Structure>/D<순번>` / AREF 도면은 저장 차단 / MAP_ACTIVE와 Chain 저장은 제외.

**2026-10-02 구현 1단계:** `08_DBQuery/ORACLE/TQP_GDS_IMPORT.xml`(LAYER / SOURCE / PLACED INSERT, 좌표 재조회) 추가. `Probes/GdsImportBlobProbe.ps1`로 로컬 DB에서 BLOB / Double / CLOB 왕복과 중복 건너뛰기 PASS. Middleware SQL 로그가 byte[]를 처리하지 못해 이 XML은 `logging="FALSE"`. 행당 약 4.4ms / 자동 Commit.

**2026-10-02 Bulk 비교 / 불러오기 형태 검토:** 설계 문서 11 / 12절. 작은 도형은 Middleware `ExecuteMultiple` + `HEXTORAW` 배열(`CREATE_PLACED_EL_MULTI`, 행당 0.03~0.07ms, 호출 단위 원자적), 큰 도형만 `Execute1`. 좌표 double은 G17 문자열 + `TO_BINARY_DOUBLE`(`dbtype="Double"`은 15자리 반올림). 전체 Map 표시는 행 조회보다 개정본당 스냅샷 / 원본 GDS BLOB이 유리하다는 측정 결과를 보고했다.

**2026-10-02 컨셉 수정(사용자 결정, 설계 13절):** DB 방식으로 확정. GDS 전체를 행으로 저장하고 조회 시 Chain만 / 도면만 / 둘 다 옵션. KLARF 비교는 Chain Member 영역만. 스냅샷 / 원본 BLOB 보관은 채택 안 함. Middleware `GetDataTable`의 BLOB 조회는 행당 0.013ms로 빠르므로 조회는 BLOB 그대로, 큰 도면은 범위 나눔 조회.

SOURCE 전송 계약과 ID를 정리한 뒤 Layer -> Source -> Placed 순서의 제한 배치를 연결한다. 해시 / 중복 배치 / 중단 재개 / 취소 / 최대 메모리 / 저장 후 재조회는 각각 검사한다. UI 스레드의 동기 도형 순회에 네트워크 I/O를 넣지 않는다.


**2026-10-02 구현 2단계(CLAUDE-007):** 저장 Job 서비스(`iGdsMapImport`: BeginMapImport / GetImportJob / CancelImportJob / GetImportBatch)를 기존 DefectDefine 방식(string[] / DataTable, 테이블별 DSL / XML)으로 구현하고 101_ServerBin / 100_ClientBin에 배포. `Probes/GdsImportJobProbe.ps1` PASS. 상세는 설계 14절.


**2026-10-02 구현 3단계(CLAUDE-008):** `CreateLayerBatch` / `CreateSourceBatch` + 클라이언트 `GdsMapSourceRowBuilder`(SOURCE_PAYLOAD v1 확정). 샘플 SOURCE 636,970행 68초 저장 / 재전송 / 변조 거부 PASS. 완료 확정은 누적값이 아니라 DB COUNT로 검증해야 함. 상세는 설계 15절.

**2026-10-02 구현 4단계(CLAUDE-009):** `CreatePlacedBatch` / `CompleteMapImport` + 클라이언트 `GdsMapPlacedRowBuilder`. 샘플 GDS 전체(SOURCE 636,970 / PLACED 605,080 / 좌표 8,292,978)를 저장하고 READY 확정, 좌표 재조회 일치, 누락 시 MISMATCH 확인(`GdsImportFullProbe.ps1` PASS). 전체 약 5.5분, PLACED 행당 0.386ms -> 생성 튜닝 후보. 상세는 설계 16절.


**2026-10-02 키 전환(CLAUDE-011):** GUID 키를 시퀀스(MAP_REV_SEQ / IMPORT_JOB_SEQ / CHAIN_SEQ / BATCH_EDIT_SEQ)로 전환, 로컬 DB 13개 테이블 재생성. 위 설명의 `MAP_REVISION_ID` / `JOB_ID`는 현재 `MAP_REV_SEQ` / `IMPORT_JOB_SEQ`다. Probe 5개 PASS, 전체 저장 SOURCE 53초 / PLACED 180초.


**2026-10-02 구현 5단계(CLAUDE-012):** GDS Map Setup 화면에 DB 저장 연결(`GdsMapForm_DbSave.cs`, RO `GdsMapImport` 호출). 빌드 / ClientBin 배포 완료, DMS 화면 실저장 확인 대기. 설계 18절.

**2026-10-02 조회 화면 설계(CLAUDE-013):** 등록 화면과 분리한 조회 전용 화면 [GDS Map 조회 화면 설계](../../../문서/2026-10-02_GDS_Map_조회화면_설계.md). 1차는 READY 개정본의 도면만(LAYER + PLACED, Layer별 keyset 페이지 조회) / 신규 읽기 서비스 `iGdsMapView` / `GdsMapControl.ShowPlacedElements` 추가 / 호스트 `frmGdsMapView`. Chain 옵션은 Chain 저장 후. 설계만, 사용자 결정 대기.

**2026-10-02 조회 구현 1단계(CLAUDE-014):** 읽기 전용 `iGdsMapView`(Device / Revision / Layer / PLACED keyset 페이지) + XML SELECT 6개. BSL 직접 Probe PASS(605,080행 66페이지 약 16초, 누락 / 중복 없음). 페이지는 `RemotingFormat = Binary`(10,000행 7.7MB -> 2.9MB). DLL 배포 대기. 상세는 조회 설계 11절.

**2026-10-02 조회 구현 2·3단계(CLAUDE-015):** `GdsMapPlacedRowReader` + `GdsMapControl.ShowPlacedElements`. 파일 Flatten과 DB 복원이 도형 605,080건 / 좌표 비트 단위까지 일치(`GdsMapDbRestoreProbe.ps1` PASS), DB 경로 합계 18.7초. CLAUDE-014 DLL 15:40 배포 완료. 조회 설계 11~12절.


**2026-10-02 리비전 자동 처리(CLAUDE-016):** 화면에서 Revision 입력 제거, 서버가 자동 Code / 같은 파일 이어받기 / 같은 READY 파일 거부 처리. 확정 시 이전 READY는 RETIRED, `TQP_GDS_MAP_ACTIVE` 자동 지정(신규 XML / DSL). 사용 우선순위: ACTIVE -> 최신 READY. 설계 19절 / 조회 설계 13절.

**2026-10-02 조회 구현 4단계(CLAUDE-016):** `GdsMapViewForm` + 호스트 `frmGdsMapView` + 메뉴 `MNU_DMS_GDS_MAP_VIEW`. 빌드 / 배포 완료, DMS 화면 확인 대기. 조회 설계 13절.

**2026-10-02 조회 화면 리비전 제거 반영(CLAUDE-018):** 조회 키 FACTORY + DEVICE_ID, Revision 콤보 삭제. 새 구조에서 복원 Probe(605,080건 완전 일치, DB 경로 25.9초) / 조회 Probe PASS, 16:55 배포. DMS 화면 확인 대기. 조회 설계 15절.


**2026-10-02 리비전 관리 제거(CLAUDE-017, 최신 기준):** 키 FACTORY + DEVICE_ID(Device당 Map 1개, 헤더 TQP_GDS_MAP). 기존 데이터가 있으면 화면이 삭제 후 재등록 여부를 묻고 `DeleteMap`(CHAIN -> PLACED -> SOURCE -> MAP). MAP_REV_SEQ / MAP_ACTIVE / RETIRED / 리비전 자동 처리 제거. 설계 20절 / DB 스키마 현황 마지막 절.
