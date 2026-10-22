# Factory FAB1 / FAB2 -> SCK 통합 점검

작성일: 2026-10-13 / 범위: 로컬 DB(docker `oracle19c`, 스키마 DMSMGR / MESMGR) + `DMS` 소스 / 상태: **로컬 DB 적용 완료 (6절), 고객 DB 미적용**

## 1. 결론 요약

- 프로그램 설정은 이미 SCK 다. 클라이언트 `DACruxConfig.ini`(Factory / FactoryList = SCK)와 서비스 6개 `App.config`(FACTORY = SCK). 그래서 지금 화면 / 서비스는 FAB1 / FAB2 데이터를 조회하지 못하고, 새로 들어오는 데이터만 SCK 로 쌓인다.
- 바꿀 대상은 DMSMGR 의 FACTORY 컬럼(20개 테이블)과 FACTORY 와 같은 의미로 쓰는 FACILITY 컬럼(3개 테이블)이다. MESMGR 는 `MRC` 만 있어 대상이 아니다.
- 단순 `UPDATE ... SET FACTORY='SCK'` 로는 안 된다. FAB1 과 FAB2 에 같은 키가 있는 테이블 5개는 PK 충돌 / 중복 설정이 생기므로 먼저 어느 쪽을 남길지 정해야 한다.
- `HandlerPcm_Fab2` 같은 Handler / Parser 클래스 이름은 Factory 가 아니므로 바꾸면 안 된다(문자열 일괄 치환 금지).

## 2. DB 대상 (실제 건수, 2026-10-13 조회)

| 테이블 | 현재 값 | 키(PK / UK) | 비고 |
| --- | --- | --- | --- |
| TQP_MAPDEF_DIE | FAB2 5,596,185 | FACTORY, MAPID, DIE_TYPE, DIE_NO | 가장 큼, PK 에 FACTORY |
| TQC_LOT_HIS | FAB1 2,394,482 | 없음 | FAB1 전용 이력 |
| TQP_PARASPEC | FAB2 255,452 | FACTORY, PROGRAM, PROGRAM_REV, PARAM_NAME | |
| TQP_USEMAP | FAB2 19,201 / SCK 216 | FACTORY, MAPID, PROGRAM_REV, X, Y | 겹치는 키 0 |
| TQP_PROGRAM | FAB1 1 / FAB2 1,525 | FACTORY, PROGRAM, TESTAREA, DEVICE | **겹침 1** (BY22_DBH_WQ_P02R00 / MULTIPROBE / BY22) |
| TQP_PRODUCT | FAB1 1 / FAB2 737 | FACTORY, PRODUCT | **겹침 1** (BY22) |
| TQC_CONFIG_USER | FAB1 60 / FAB2 459 | 없음 | **겹침 14** (ADMIN 색상 / 옵션, 값 다른 것 있음) |
| TQP_MAPDEF | FAB2 405 | - | |
| TQC_SERVICE_LOG | FAB2 269 | FACTORY, SERVICE_NAME, TRAN_KEY | 로그 |
| TQP_CHANGE_HIS | FAB2 176 | - | 이력 |
| TQP_WAFER | FAB2 154 | FACTORY, WAFER_SEQ | |
| TQP_LOT | FAB2 103 | FACTORY, LOT_SEQ | |
| TQD_INSP_INFO | FAB2 81 / SCK 3 | 없음 | 겹치는 LOT / WAFER / STEP 0 |
| TQC_EQUIP | FAB1 36 / FAB2 36 | FACTORY, EQUIP_ID | **36대 전부 겹침**, 내용(HANDLER / MODEL / AREA / OPER / CMF1~3) 동일 |
| TQC_EQUIP_MODEL | FAB1 17 / FAB2 17 | FACTORY, EQUIP_MODEL | **17개 전부 겹침** |
| TQD_PRODUCT | FAB2 10 / SCK 3 | 없음 | 제품 이름 겹침 없음 |
| TQC_OPER | FAB2 7 | FACTORY, AREA, OPER | |
| TQP_DATA_TABLES | FAB2 4 | FACTORY, PROGRAM, TABLE_NAME | |
| TQD_INSP_INFO_BAK / TQD_PRODUCT_BAK2 | FAB2 82 / 10 | 없음 | 백업 테이블 |

FACILITY 컬럼(FACTORY 와 같은 의미로 사용): TQC_LOT_STS 47,965 / TQP_LOT_SUM 103 / TQP_WAFER_SUM 49 건 모두 FAB2.
`TestCommon.cs` 데이터 복사 기능이 FACILITY 값을 그대로 TQP_PRODUCT / TQP_PROGRAM 의 FACTORY 로 다시 등록하므로 같이 바꿔야 한다.

대상 아님: GDS 테이블의 `PROBE`(Probe 시험 데이터), MESMGR `MRC`, `TQC_EQUIP.HANDLER` / `TQC_SERVICE_LOG.HANDLER` 의 `HandlerPcm_Fab2` 등(클래스 이름), `TQC_SERVICE_LOG.MESSAGE`(과거 로그 문구).
뷰 / 프로시저 / 트리거 안의 FAB1 / FAB2 하드코딩 없음. 시퀀스는 Factory 와 무관. 이미지 경로(TQD_IMAGES / TQD_INSP_INFO.IMAGES)에 FAB 없음.

## 3. 소스 대상

| 구분 | 위치 | 내용 | 처리 제안 |
| --- | --- | --- | --- |
| 동작 분기 | `09_TEST/DACrux.TEST.BSL/TestCommon.cs:1301` | `factory == "FAB2"` 일 때만 NETDIE 로 수율 계산 | SCK 에서도 같은 계산이 필요하면 `"SCK"` 로 변경 (현재 SCK 데이터는 이 계산을 타지 않음) |
| 클라이언트 | `01_WinApp/DACruxV5_FAB2` (솔루션 포함) | FAB2 전용 실행 파일 / `Factory = FAB2` | 솔루션에서 제외 또는 삭제 |
| 클라이언트 | `01_WinApp/DACruxV5/DACruxV5.csproj` | ProductName `DACruxV5_FAB1`, 아이콘 `FAB1.ico` | 이름만 정리(선택) |
| 설정 | `101_ServerBin/*.vshost.exe.config` 2개 | FACTORY = FAB2 | VS 디버그 잔여 파일, 삭제 |
| 설정 | `10_Migration` / `103_Migration` `DACruxConfig.ini` | Factory = FAB1 | Migration 도구, 그대로 |
| 쿼리 XML | TQC_EQUIP / TQD_INSP_INFO / TQP_CHANGE_HIS / TQP_PCM_INPUT_HIS / TQP_WAFER | param 기본값 `value="FAB1/FAB2"` | 쿼리 도구 시험 기본값, 동작 영향 없음(선택 변경) |
| 이름 / 주석 | Parser / Handler `*_Fab1`, `*_Fab2`, `*_Fab2_MEMS`, 주석의 "FAB1 규칙" | PCM 파일 형식 / 설비 규칙 이름 | **변경 금지** (DB HANDLER 값과 연결) |

## 4. 문제될 소지

1. **PK 충돌**: TQC_EQUIP 36 / TQC_EQUIP_MODEL 17 / TQP_PRODUCT 1 / TQP_PROGRAM 1 은 FAB1 · FAB2 를 모두 SCK 로 바꾸면 PK 위반으로 UPDATE 가 실패한다. 한쪽(FAB1 사본) 을 지우고 나머지를 바꿔야 한다.
2. **중복 사용자 설정**: TQC_CONFIG_USER 는 PK 가 없어 그대로 바꾸면 ADMIN 의 같은 설정 14개가 두 줄씩 생기고 값이 다른 것(배경색 등)은 어느 값이 읽힐지 정해지지 않는다. FAB2 값 유지 / FAB1 삭제 권장.
3. **FAB1 데이터 성격**: FAB1 은 TQC_LOT_HIS 239만 건 / 설비 · 모델 복제본 / 사용자 설정 60 / BY22 1건뿐이다. SCK 로 합칠지, FAB1 을 지울지(또는 그대로 둘지) 결정 필요.
4. **문자열 일괄 치환 위험**: `HandlerPcm_Fab2` 등은 서비스가 Handler 클래스를 찾는 이름이다. FACTORY / FACILITY 컬럼만 바꾼다.
5. **대용량 UPDATE**: TQP_MAPDEF_DIE 560만 / TQC_LOT_HIS 239만 건. PK 인덱스에 FACTORY 가 들어 있어 인덱스도 함께 갱신된다. Undo / Redo 와 시간이 크므로 테이블 단위로 나눠 실행하고 사전 백업(Export 또는 CTAS) 필요.
6. **동시 작업**: 2026-10-13 점검 중 다른 세션이 `TQP_MAPDEF_DIE` 대량 DELETE 를 실행하고 있었다(현재 종료). 변경 시 서비스 / 화면 / 다른 작업 세션을 멈추고 진행해야 한다.
7. **동작 변화**: `TestCommon.cs` 의 FAB2 수율 계산 분기를 SCK 로 바꾸지 않으면, 바뀐 데이터로 Summary 를 다시 만들 때 수율 계산 방식이 달라질 수 있다.
8. **통계 정보**: 일부 테이블 통계가 오래됨(TQP_USEMAP 통계 557만 / 실제 1.9만). 변경 후 `DBMS_STATS` 재수집 권장.
9. **백업 테이블**: `_BAK` / `_BAK2` / `_1106` 테이블 포함 여부 결정 필요(권장: 그대로 두기).

## 5. 진행 순서 제안 (승인 후)

1. 결정: FAB1 처리(삭제 / 합침 / 유지), 겹치는 키는 FAB2 우선, 백업 테이블 제외 여부, TestCommon 분기 변경 여부.
2. 서비스 / DACrux 종료, 다른 작업 세션 DB 작업 중지 확인.
3. 대상 테이블 백업(CTAS 또는 Data Pump).
4. 충돌 정리: TQC_EQUIP / TQC_EQUIP_MODEL / TQP_PRODUCT / TQP_PROGRAM / TQC_CONFIG_USER 의 FAB1 사본 삭제(또는 결정 방식).
5. FACTORY / FACILITY UPDATE: 작은 테이블 -> 큰 테이블 순, 테이블마다 COMMIT, 건수 확인.
6. 확인: FAB1 / FAB2 잔여 0건, PK 위반 0, 통계 재수집, DMS 화면에서 Lot / Wafer / Map 조회.
7. 소스: TestCommon 분기 / FAB2 클라이언트 프로젝트 / vshost config 정리 후 빌드.

## 6. 실행 결과 (2026-10-13)

결정: FAB1 / FAB2 모두 SCK / 겹치는 키는 FAB2 우선(SCK > FAB2 > FAB1) / 백업 테이블 삭제 / TestCommon 수율 분기는 FAB2 방식으로 통일.

- 사전 백업: Data Pump `factory_sck_20261013.dmp` (컨테이너 `DATA_PUMP_DIR` = `/opt/oracle/admin/MESMGR/dpdump/`), 대상 21개 + 백업 18개 테이블, 오류 0.
  - 참고: 점검 이후 다른 작업(미사용 Map 삭제)으로 TQP_MAPDEF 405 -> 13, TQP_USEMAP -> 19,417, TQP_MAPDEF_DIE -> 21,321 건이 된 상태에서 실행.
- 실행 스크립트: `DMS/Assistant/DB/sql/2026-10-13_factory_sck.sql`
  - 겹치는 키 삭제: TQC_EQUIP 36 / TQC_EQUIP_MODEL 17 / TQP_PRODUCT 1 / TQP_PROGRAM 1 / TQC_CONFIG_USER 14 (모두 FAB1 사본).
  - SCK 변경: TQC_LOT_HIS 2,394,482 / TQP_PARASPEC 255,452 / TQC_LOT_STS.FACILITY 47,965 / TQP_MAPDEF_DIE 21,321 / TQP_USEMAP 19,201 / TQP_PROGRAM 1,525 / TQP_PRODUCT 737 / TQC_CONFIG_USER 505 / TQC_SERVICE_LOG 269 / TQP_CHANGE_HIS 176 / TQP_WAFER 154 / TQP_LOT 103 / TQP_LOT_SUM.FACILITY 103 / TQD_INSP_INFO 81 / TQP_WAFER_SUM.FACILITY 49 / TQC_EQUIP 36 / TQC_EQUIP_MODEL 17 / TQP_MAPDEF 13 / TQD_PRODUCT 10 / TQC_OPER 7 / TQP_DATA_TABLES 4.
  - 백업 테이블 18개 DROP (휴지통에 남음, `FLASHBACK TABLE ... TO BEFORE DROP` 로 복구 가능).
  - 확인: 새 세션 재조회에서 FACTORY / FACILITY 의 FAB1 / FAB2 0건, 무효 객체 0, 통계 재수집(GATHER STALE).
- 소스: `TestCommon.cs` 수율 계산을 Factory 조건 없이 NETDIE 기준으로 통일, `101_ServerBin/*.vshost.exe.config` 2개 FACTORY = SCK, 쿼리 XML 5개 param 기본값 SCK. TEST.BSL 빌드(TEMP 사본) 오류 0.
- 그대로 둔 것: `DACruxV5_FAB2` 클라이언트 프로젝트(Factory = FAB2 설정), Migration 도구 설정(FAB1), Handler / Parser `_Fab1` / `_Fab2` 이름.
- 미확인: DMS 화면 조회(Lot / Wafer / Map / Defect), 서비스 재시작 후 신규 적재.

## 2026-10-14 추가: 컬럼 기본값 FAB2 (CLAUDE-101)

- 증상: File Map Setup 으로 OMM_TCB Map 을 만들었는데 KLARF Upload [DB 저장] 이 "Device OMM_TCB 의 Map 이 없습니다 (Factory SCK)". DB 의 `TQP_MAPDEF` 행이 FACTORY = FAB2.
- 원인: 10-13 스크립트는 데이터만 바꾸고 컬럼 기본값은 그대로였다. `TQP_MAPDEF.FACTORY DEFAULT 'FAB2'` + Map Define / File Map Setup 저장 쿼리 `TQP_MAPDEF.xml` CREATE_MAP 이 FACTORY 를 넣지 않음 -> 10-13 이후 새로 만든 OMM / OMM_TCB 가 FAB2 (사용 Die `TQP_USEMAP` 은 로그인 Factory SCK 로 저장되어 둘이 어긋남).
- 점검: DMSMGR 의 FACTORY 컬럼 기본값은 TQP_MAPDEF('FAB2') / TQC_SERVICE_LOG · TQD_PRODUCT(' ') 뿐. SCK 가 아닌 FACTORY 값은 TQP_MAPDEF FAB2 2건 + 시험 데이터 PROBE(GDS 4개 테이블 / TQP_PRODUCT)뿐.
- 조치: `DMS/Assistant/DB/sql/2026-10-14_tqp_mapdef_factory_sck.sql` 로컬 실행 = 기본값 'SCK' + FAB2 2건(OMM, OMM_TCB) -> SCK (키 충돌 없음 확인 후). 결과 FAB1 / FAB2 0건, `ExistsMapID(SCK, OMM_TCB / OMM / OMM_Hybrid)` 모두 True.
- 소스는 바꾸지 않음 (기존 Map Define 저장 경로는 FACTORY 를 DB 기본값에 맡기는 구조). **고객 DB 도 기본값이 FAB2 면 같은 스크립트 필요.**
