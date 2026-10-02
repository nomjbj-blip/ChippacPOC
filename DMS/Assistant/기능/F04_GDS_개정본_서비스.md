# F04 / GDS 개정본 서비스

확인일: 2026-10-02 / 상태: 건수 조회 / DRAFT 등록 / 재조회 코드 확인 / 서비스 실행 미검증

## 목적과 파일

전체 Map 도형 저장에 앞서 Factory / Device / Revision별 GDS 메타데이터 한 건을 등록하고 조회한다.

| 계층 | 파일 |
| --- | --- |
| 시험 화면 | [frmGdsMapRevisionCount.cs](../../09_TEST/DACrux.TEST.ENGUI/Analysis/frmGdsMapRevisionCount.cs) / 동일 이름 Designer |
| RO | [GdsMapRegistration.cs](../../02_DMS/DACrux.SEMDMS.RO/GdsMapRegistration.cs) |
| 계약 / DTO | [iGdsMapRegistration.cs](../../02_DMS/DACrux.SEMDMS.Interface/iGdsMapRegistration.cs) |
| BSL | [GdsMapRegistration.cs](../../02_DMS/DACrux.SEMDMS.BSL/GdsMapRegistration.cs) |
| DSL | [TQP_GDS_MAP_REV.cs](../../02_DMS/DACrux.SEMDMS.DSL/TQP_GDS_MAP_REV.cs) |
| Query XML | [TQP_GDS_MAP_REV.xml](../../08_DBQuery/ORACLE/TQP_GDS_MAP_REV.xml) |

## 현재 계약

| 함수 | 역할 / 현재 처리 |
| --- | --- |
| GetMapRevisionCount(factory, deviceId) | SELECT_MAP_REVISION_COUNT / 첫 행 첫 컬럼을 int로 변환 |
| CreateMapRevision(request) | 필수 문자열 / SHA-256 / 단위를 검증하고 GUID의 DRAFT 헤더 한 건 생성 |
| GetMapRevision(factory, deviceId, revisionCode) | SELECT_MAP_REVISION / 없으면 null / 중복 결과는 오류 |

화면 이름에 Count가 남아 있지만 현재 코드는 등록 / 재조회 버튼도 처리한다. 기능을 건수 조회만 구현된 것으로 기록하지 않는다.

## 중요한 로직

- `SELECT COUNT(*)` 결과의 `Rows.Count`는 조회 결과 행 수다. 업무 건수는 `dt.Rows[0][0]`을 읽는다.
- RO는 `typeof(iGdsMapRegistration)`으로 프록시를 만들고 `DACrux.SEMDMS.BSL.GdsMapRegistration.bin`을 호출한다.
- `101_ServerBin/App.config`와 `DACrux.SEMDMS.Service.exe.config`에 같은 objectUri 등록이 있다. 배포할 때 실제 실행 EXE의 config와 DLL 일치를 확인한다.
- DSL은 `DMS_CONNECT_ID`와 `TQP_GDS_MAP_REV.xml`을 사용한다. Query ID / 파라미터 순서 / 길이를 BSL과 함께 맞춘다.
- 요청은 GDS 해시 64자리 16진수 / 유한한 양수 단위 / 필수 문자열 길이를 검증한다. 일부 길이는 UTF-8 바이트 기준이다.
- 현재 등록 대상은 개정본 헤더다. Layer / Source / Placed / Chain의 저장 또는 READY 전환이 아니다.

## 알려진 문제 (2026-10-02 Claude 확인)

`CreateMapRevision`은 `UserUnit` / `DatabaseUnit`을 `ToString("R")`로 넘긴다. 1e-9는 `"1E-09"`가 되고 Middleware(Oracle.ManagedDataAccess)의 Decimal 변환에서 `FormatException`이 난다. 같은 쿼리에 `"0.000000001"`을 넣으면 성공했다. 일반 GDS의 DB 단위로 등록이 실패하므로 지수 표기를 쓰지 않는 문자열 변환으로 바꿔야 한다. 기존 함수 변경이므로 사용자 승인 후 수정한다.

## 정상 동작 확인 순서

1. 실제 서비스 EXE / 설정 / BSL / DSL / Interface / Query XML의 배포 버전을 확인한다.
2. 승인된 테스트 환경에서 Factory / Device 건수를 조회한다.
3. 새 Revision Code로 DRAFT 한 건 등록 후 같은 키로 재조회한다.
4. 반환된 ID / DRAFT 상태 / GDS 해시 / 단위를 대조한다.
5. 같은 Revision Code 재등록 / 잘못된 해시 / 비정상 단위 입력도 확인한다.

이번 문서 작성에서는 위 실행을 하지 않았다. 과거 DB 스키마 적용 결과를 이 서비스의 저장 성공 근거로 사용하지 않는다.


## 2026-10-02 / 저장 Job 서비스와의 관계

신규 `GdsMapImport.BeginMapImport`가 개정본 생성 / 조회에 기존 `CreateMapRevision` / `GetMapRevision`을 그대로 호출한다. 기존 함수는 단위 변환 수정(CLAUDE-005) 외에 변경하지 않았다.


## 2026-10-02 / 개정본 번호 시퀀스 전환 (CLAUDE-011)

`CreateMapRevision`은 GUID 대신 `SELECT_NEXT_MAP_REV_SEQ`로 번호를 받아 `MAP_REV_SEQ`에 넣고 그 번호(문자열)를 반환한다. `GetMapRevision`은 `MAP_REV_SEQ`를 `MapRevisionId`에 담는다(DTO 형식 유지). TEST 화면 `frmGdsMapRevisionCount`는 이 DTO를 그대로 쓰므로 수정하지 않았다.


## 2026-10-02 / 리비전 자동 처리 (CLAUDE-016)

화면은 Revision Code를 보내지 않는다. `GdsMapImport.BeginMapImport`가 빈 Code를 자동 처리하고, 확정 시 같은 Device의 다른 READY를 RETIRED로 바꾸고 MAP_ACTIVE를 지정한다. 기존 `GdsMapRegistration` 함수는 이번에 바꾸지 않았다. 상세는 저장 설계 19절.


## 2026-10-02 / 리비전 관리 제거 (CLAUDE-017)

`TQP_GDS_MAP_REV` 테이블이 없어지고 헤더는 `TQP_GDS_MAP`(Factory + Device 1:1)이 되었다. 이 문서의 `GdsMapRegistration` / `frmGdsMapRevisionCount`(Codex 작성)는 더 이상 동작하지 않는다. 저장 서비스 `GdsMapImport`는 이것을 쓰지 않는다. 소스 삭제 여부는 사용자 결정 대기.

**2026-10-02 17:12 삭제 완료(CLAUDE-020):** `GdsMapRegistration`(Interface / RO / BSL), `TQP_GDS_MAP_REV`(DSL / XML), TEST 화면 `frmGdsMapRevisionCount`와 메뉴, Service 등록을 모두 삭제했다. 이 문서는 이력으로만 남긴다.
