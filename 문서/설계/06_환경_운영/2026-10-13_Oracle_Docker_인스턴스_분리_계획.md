# Oracle 19c Docker 인스턴스 분리 계획 / 결과 (MESMGR / DMSMGR)

작성일: 2026-10-13 / 범위: 로컬 docker `oracle19c` 컨테이너, 클라이언트 tnsnames, 테스트 설정 / 상태: **로컬 적용 완료 (9절). 남은 것: tnsnames.ora 관리자 권한 반영 / DBeaver 접속 변경**

## 1. 결론 요약

- 지금은 Oracle 인스턴스 1개(DB 이름 / SID / 서비스 `MESMGR`) 안에 스키마 DMSMGR(3.5GB)과 MESMGR(317MB)가 같이 있다.
- **메모리 5.4GB의 원인은 MESMGR 스키마가 아니라 인스턴스 메모리 설정이다.** SGA가 4,720MB로 고정되어 있고(PGA 목표 1,574MB 별도), 세션이 없는 스키마는 메모리를 거의 쓰지 않는다. 이미지의 `createDB.sh`가 CPU가 8개를 넘으면(이 PC는 22개) dbca 기본값인 "가용 메모리의 40%"로 DB를 만들었기 때문이다.
- 그래서 **분리만 하면 메모리는 줄지 않는다.** 두 인스턴스를 같이 띄우면 오히려 늘어난다. 메모리를 줄이는 것은 아래 두 가지다.
  1. DMS 인스턴스의 SGA / PGA를 줄인다 (SGA 2GB / PGA 1GB 제안 -> 컨테이너 약 2.5 ~ 3GB 예상).
  2. MES 컨테이너는 평소 중지해 두고 필요할 때만 띄운다.
- 두 스키마 사이에 의존성 / 시노님 / 권한 / DB Link / Job이 하나도 없어 분리에 막히는 것은 없다.
- 앱(Middleware)은 docker의 MESMGR 스키마를 쓰지 않는다. 레지스트리의 MESMGR 접속은 원격 `MES_NX300_DEV`(1.209.89.182)를 가리킨다.
- 추천 방식: **새 DMS 컨테이너를 만들어 DMSMGR를 Data Pump로 옮기고, 기존 컨테이너는 MES 전용으로 남긴다(방식 A).** 기존 DB는 전환 전까지 손대지 않으므로 되돌리기가 쉽다.

## 2. 현재 상태 (2026-10-13 조회)

| 항목 | 값 |
| --- | --- |
| 컨테이너 | `oracle19c` / 이미지 `doctorkirk/oracle-19c:latest` / 2026-08-06 `docker run`으로 생성(compose 없음) / restart `no` |
| 포트 / 볼륨 | `1521:1521` / `oracle19c-data` -> `/opt/oracle/oradata` (20.1GB, 그중 `oradata/MESMGR` 19GB) |
| DB | non-CDB (`ORACLE_PDB=MESMGRPDB` 환경변수는 있지만 PDB 없음), NOARCHIVELOG, AL32UTF8, 시간대 UTC |
| 메모리 | `sga_target` = `sga_max_size` = 4,720MB, `pga_aggregate_target` 1,574MB (할당 554MB), `processes` 1,760 / 컨테이너 5.4GB, `vmmemWSL` 6.3GB |
| Redo | 1GB x 4 (2026-10-06 GDS 대량 저장 때 200MB x 3에서 변경) |
| Data Pump 폴더 | `DATA_PUMP_DIR` = `/opt/oracle/admin/MESMGR/dpdump/` -> **볼륨이 아니라 컨테이너 안**이다. 오늘 백업 `factory_sck_20261013.dmp`(348MB)가 여기 있다 |
| 디스크 | docker 디스크 여유 919GB |

| 스키마 | 크기 | 객체 | 테이블스페이스 / 권한 |
| --- | --- | --- | --- |
| DMSMGR | 3,489MB | 테이블 81 / 인덱스 145 / 시퀀스 24 / LOB 3 / 함수 1 | USERS(대부분) / TS_TEST_TBL / TS_TEST_IDX, RESOURCE |
| MESMGR | 317MB | 테이블 457 / 인덱스 589 / LOB 59 / 시퀀스 1 / 함수 1 | `MIC_*` 테이블스페이스 62개(대부분 MIC_QMS_DAT_TS 262MB), DBA |

접속 경로:

- 앱: `app.config`의 `*_CONNECT_ID` = DMSMGR -> 레지스트리 `HKLM\SOFTWARE\Miracom\Middleware\Connections\DMSMGR` (`Data Source=DMS`) -> `D:\Oracle11gClient\product\11.2.0\client_1\network\admin\tnsnames.ora`의 `DMS` = `127.0.0.1:1521 / SERVICE_NAME=MESMGR`.
- 레지스트리 MESMGR / QMSMGR 접속은 원격 서버(`MES_NX300_DEV` / `QMS_NX300_DEV`)라 이번 작업과 무관하다.
- 조회 시점 접속 세션: DBeaver(DMSMGR) 3개뿐. Windows에 Oracle 서비스 없음.
- 컨테이너 이름 `oracle19c`를 쓰는 곳: `.testagent/config.json`(`dockerContainer`), `.testagent/probes/KlarfUploadReadOnlyProbe.ps1`, `DMS/Assistant/빌드_검증_가이드.md`, program-verify 스킬 `references/gotchas.md`. 소스 코드 / app.config / 레지스트리는 바꿀 필요 없다.

## 3. 목표 구성

| 항목 | DMS (신규) | MES (기존 컨테이너 재생성) |
| --- | --- | --- |
| 컨테이너 | `oracle19c-dms` | `oracle19c-mes` |
| 볼륨 | `oracle19c-dms-data` (신규) | `oracle19c-data` (기존 그대로) |
| SID / 서비스 | `DMS` | `MESMGR` (그대로) |
| 호스트 포트 | 1521 | 1522 |
| 메모리 | SGA 2GB / PGA 1GB / processes 300 | SGA 1GB / PGA 512MB로 축소 |
| 기동 | 평소 사용 | 평소 중지, 필요할 때만 |
| 스키마 | DMSMGR | MESMGR (+ 전환 확인 후 남는 DMSMGR 사본은 보관 기간 뒤 삭제) |
| Redo | 1GB x 4 (기존과 같게) | 그대로 |

관리는 `docker-compose.yml` 하나로 한다. MES는 `profiles: ["mes"]`로 두어 `docker compose up -d`에는 DMS만 뜨고, MES는 `docker compose --profile mes up -d mes`로 따로 띄운다. 비밀번호가 들어가므로 저장소 밖(예: `D:\docker\oracle19c\`)에 `.env`와 함께 둔다.

```yaml
# D:\docker\oracle19c\docker-compose.yml (안)
name: oracle19c
services:
  dms:
    image: doctorkirk/oracle-19c:latest
    container_name: oracle19c-dms
    environment:
      ORACLE_SID: DMS
      ORACLE_PWD: ${DMS_SYS_PWD}
      ORACLE_CHARACTERSET: AL32UTF8
    ports: ["1521:1521"]
    volumes: ["dms-data:/opt/oracle/oradata"]
    restart: "no"
  mes:
    image: doctorkirk/oracle-19c:latest
    container_name: oracle19c-mes
    profiles: ["mes"]
    environment:            # 기존 컨테이너와 같은 값이어야 기존 DB로 기동한다
      ORACLE_SID: MESMGR
      ORACLE_PDB: MESMGRPDB
      ORACLE_PWD: ${MES_SYS_PWD}
      ORACLE_CHARACTERSET: AL32UTF8
    ports: ["1522:1521"]
    volumes: ["mes-data:/opt/oracle/oradata"]
    restart: "no"
volumes:
  dms-data:
    name: oracle19c-dms-data
  mes-data:
    name: oracle19c-data
    external: true          # compose down -v 로 지워지지 않게 외부 볼륨으로 둔다
```

### 다른 방식 (방식 B)

기존 컨테이너를 DMS로 그대로 쓰고 MESMGR(317MB)만 새 컨테이너로 옮긴 뒤 기존 DB에서 MESMGR를 지운다. 옮길 데이터가 작아 빠르지만, DMS가 계속 이름이 `MESMGR`인 DB에 남고, 운영 중인 DMS DB를 직접 바꾸며, 19GB 데이터파일이 그대로 남는다. 그래서 A를 추천한다.

## 4. 작업 단계 (방식 A)

DMS 로컬 DB가 멈추는 시간은 3 ~ 5단계 동안(약 1시간 추정)이다. 이 동안 DMS Service / DBeaver / Probe를 쓰지 않는다.

| 단계 | 내용 | 확인 | 되돌리기 |
| --- | --- | --- | --- |
| 0. 준비 | 사용자 결정(7절) 확인. `docker inspect oracle19c` 결과 / `CREATE PFILE FROM SPFILE` / tnsnames.ora 사본을 `%TEMP%\chippac\oracle_split\`에 저장. **`dpdump`의 `factory_sck_20261013.dmp`를 `docker cp`로 꺼냄** | 파일 존재 / 크기 | - |
| 1. 기준값 수집 | 기존 DB의 DMSMGR 객체 수(종류별) / 테이블별 정확한 건수(`COUNT(*)`) / 시퀀스 현재값 / 제약 상태를 파일로 저장 | 81 / 145 / 24 / 3 / 1 | - |
| 2. 쓰기 중지 + Export | DMS Service / DBeaver 종료 -> `expdp schemas=DMSMGR flashback_time=systimestamp` (백업용으로 MESMGR도 별도 export) -> 덤프를 `docker cp`로 꺼냄 | expdp 로그 오류 0 / 행 수가 1단계와 같음 | 기존 DB 변경 없음 |
| 3. 신규 DB 생성 | `docker compose up -d dms`. 이때 1521을 쓰려면 먼저 `docker stop oracle19c`. 첫 기동은 dbca로 DB를 만들어 20 ~ 40분 걸린다(추정) | `docker logs`에 `DATABASE IS READY TO USE!` / healthy | `docker compose rm dms` 후 `docker start oracle19c` |
| 4. 메모리 / Redo 설정 | `sga_max_size` / `sga_target` 2G, `pga_aggregate_target` 1G, `pga_aggregate_limit` 2G, `processes` 300 (`SCOPE=SPFILE`) -> 재기동. Redo 1GB x 4 추가 후 기본 그룹 삭제. TS_TEST_TBL / TS_TEST_IDX 생성 | `show parameter sga` / `docker stats` 약 3GB 이하 / `v$log` | 3단계와 같음 |
| 5. Import | 덤프를 새 컨테이너 `dpdump`로 복사 -> `impdp schemas=DMSMGR` -> `utlrp` 재컴파일 -> 통계 수집 | impdp 오류 0 / 1단계와 객체 수 / 테이블별 건수 / 시퀀스 값 일치 / INVALID 0 / 비활성 제약 0 | 3단계와 같음 |
| 6. 접속 전환 | tnsnames.ora `DMS`의 `SERVICE_NAME = MESMGR` -> `DMS` 한 줄 수정(포트 1521 그대로). DBeaver 접속 서비스 이름 변경 | `tnsping DMS` / Probe `KlarfUploadReadOnlyProbe.ps1`(조회만) PASS / 사용자 DMS 화면 확인 | tnsnames 원복 + `oracle19c-dms` 중지 + `oracle19c` 기동 |
| 7. MES 컨테이너 정리 | `docker stop oracle19c` -> `docker rm oracle19c`(볼륨 유지) -> `docker compose --profile mes up -d mes`(1522) -> 기존 DB로 열리는지 확인 -> SGA 1G / PGA 512M로 줄이고 재기동 -> 중지 | `v$database` NAME=MESMGR / MESMGR 객체 수 그대로 / 기동 후 중지 | 같은 환경변수로 `docker run`을 다시 하면 볼륨의 DB로 기동한다 |
| 8. 마무리 | `.testagent/config.json` / Probe / 빌드_검증_가이드 / program-verify gotchas의 컨테이너 이름 변경, 스키마_생성_현황에 접속 정보 기록. 보관 기간 뒤 MES DB의 DMSMGR 사본 `DROP USER ... CASCADE` | 문서 반영 | - |

## 5. 주의할 점

- **기존 컨테이너를 지우면 `dpdump` 안의 덤프도 같이 지워진다.** 0단계에서 꺼내 둔다. 새 컨테이너도 같은 구조라 덤프는 쓰고 나서 바로 꺼내 둔다.
- 새 DB도 그냥 만들면 같은 이유로 가용 메모리 40%를 잡는다. 4단계 메모리 설정을 빠뜨리면 분리 효과가 없다.
- 2단계 Export 이후 기존 DB에 쓴 데이터는 새 DB에 없다. Export부터 6단계 전환까지 쓰기를 멈춘다.
- 새 DB의 Redo 기본값은 200MB라 GDS 대량 저장 때 `log file switch` 대기가 다시 생긴다. 4단계에서 1GB x 4로 맞춘다.
- 새 DB도 시간대가 UTC다(같은 이미지). SYSDATE 비교 주의는 그대로다.
- 7단계 재생성은 볼륨의 `oradata/dbconfig/MESMGR`(spfile / orapw / listener / tnsnames 사본)로 기존 DB를 다시 연다. `ORACLE_SID`가 다르면 새 DB를 만들려고 하므로 환경변수를 기존과 똑같이 둔다.
- 두 컨테이너를 같이 띄우면 메모리는 DMS 약 3GB + MES 약 1.5GB가 된다.
- 비밀번호는 compose / `.env`에만 두고 저장소에 커밋하지 않는다.

## 6. 변경 대상

| 대상 | 변경 | 위치 |
| --- | --- | --- |
| docker | 신규 `oracle19c-dms` + 볼륨, 기존 `oracle19c` -> `oracle19c-mes`(1522) | 로컬 docker |
| compose | 신규 `docker-compose.yml` / `.env` | 저장소 밖 (`D:\docker\oracle19c\` 안) |
| tnsnames.ora | `DMS` 서비스 이름 1줄 | `D:\Oracle11gClient\...\network\admin` (저장소 밖) |
| 테스트 설정 | 컨테이너 이름 | `.testagent/config.json`, `.testagent/probes/KlarfUploadReadOnlyProbe.ps1` |
| 문서 | 컨테이너 / 서비스 이름 | `DMS/Assistant/빌드_검증_가이드.md`, `DB/스키마_생성_현황.md`, program-verify `gotchas.md` |
| 소스 / app.config / 레지스트리 | 변경 없음 | - |

## 7. 사용자 결정 필요

1. 방식: **A 신규 DMS 생성 + DMSMGR 이관 (추천)** / B 기존을 DMS로 유지 + MESMGR 이관
2. DMS 메모리: **SGA 2GB / PGA 1GB (추천)** / 다른 값
3. 이름 / 포트: **`oracle19c-dms` 1521, `oracle19c-mes` 1522 (추천)** / DMS가 기존 이름 `oracle19c`를 이어 씀(테스트 설정 수정 불필요)
4. MES DB에 남는 DMSMGR 사본: **2주 보관 후 삭제 (추천)** / 바로 삭제 / 계속 보관
5. compose 위치: **저장소 밖 `D:\docker\oracle19c\` (추천)** / 다른 위치
6. tnsnames.ora / DBeaver 수정: Claude가 백업 후 수정 / 사용자가 직접
7. 작업 시간: DMS 로컬 DB가 약 1시간 멈춰도 되는 시간대

## 8. 사용자 결정 (2026-10-13)

- 방식 A 로 진행. SGA 는 "현재 상황에 맞게" -> 기존 인스턴스의 `v$sga_target_advice` / `v$pga_target_advice` 로 정함.
- 컨테이너 이름에 인스턴스를 드러냄 + **SID 도 변경**: DMS = SID `DMS`, MES = SID `MES`. 그래서 MES 도 기존 DB 를 재사용하지 않고 새로 만들어 MESMGR 를 Data Pump 로 옮김 (3절 표의 "기존 컨테이너 재생성"을 대체).

## 9. 실행 결과 (2026-10-13)

| 항목 | DMS | MES |
| --- | --- | --- |
| 컨테이너 | `oracle19c-DMS` (평소 사용) | `oracle19c-MES` (중지 상태로 둠, `cpuset 0-7`) |
| SID / 서비스 / 포트 | DMS / DMS / 1521 | MES / MES / 1522 |
| 볼륨 | `oracle19c-dms-data` | `oracle19c-mes-data` |
| 메모리 | SGA 3G / PGA 768M / limit 2G / processes 300 | SGA 1G / PGA 512M / limit 2G / processes 300 |
| Redo | 1GB x 4 (기본 200MB x 3 삭제) | 기본 200MB x 3 |
| 테이블스페이스 | USERS(3G, 256M 확장) / TS_TEST_TBL / TS_TEST_IDX | MIC_* 94개 (기존과 같은 이름 / 크기) |

메모리 근거 (기존 인스턴스, 기동 후 약 1일): SGA 4,720MB -> 2,950MB 일 때 예상 DB Time +0.6%, 2,360MB 일 때 +3.1% / 물리 읽기 2배. PGA 는 394MB 에서도 적중률 100%. 그래서 DMS 는 SGA 3G / PGA 768M.
결과: DMS 컨테이너 5.4GB -> 3.6GB (cgroup: shmem 3.0GB + anon 0.34GB, 나머지는 파일 캐시).

검증:

- DMSMGR: 객체 수(테이블 81 / 인덱스 145 / 시퀀스 24 / LOB 3 / 함수 1) / INVALID 0 / 제약(C 469 / P 65 / R 18 / U 7, 모두 ENABLED) / 시퀀스 24개 값 / 테이블 81개 행 수 **기존과 동일** (`D:\docker\oracle19c\backup\base_old_DMSMGR.txt` vs `base_new_DMSMGR.txt`).
- MESMGR: 테이블 457 / 인덱스 589 / 제약 / 시퀀스 / 행 수 동일. LOB 59 -> 52 는 기존 DB 휴지통(삭제된 테이블)의 LOB 7개라 정상 (살아 있는 LOB 52 = 새 DB 52).
- 앱 계정: 레지스트리 DMSMGR 비밀번호로 `//localhost:1521/DMS` 로그인 성공 (DB_NAME=DMS, 테이블 81).
- 컨테이너를 compose 정의로 다시 만든 뒤에도 기존 DB 로 기동, SPFILE 메모리 값 유지 확인.

Import 중 발견 / 처리:

- PK 가 같은 컬럼 구성의 일반 인덱스를 대신 쓰게 됨 (Data Pump 가 인덱스를 먼저 만들고 PK 를 붙이면서 재사용). DMSMGR `PK_GDS_CHAIN_MEMBER` 1건, MESMGR `MEPTINDDEF_PK` / `QRTZ_TRIGGERS_PK`(FK 4개 참조) / `MWMSULDDEF_PK` / `MQBMWAFDAT_PK` 4건 -> 제약 이름과 같은 고유 인덱스로 다시 만들어 기존과 같게 함 (FK 는 DDL 보관 후 재생성). DMSMGR 에 같은 형태가 3건 더 있지만 기존 DB 도 같은 구성(인덱스 수 145 동일)이라 그대로 둠.
- `ORA-39346` (DMSMGR.DBH_DATE 문자셋 변환 손실): 기존 DB 에서도 이 함수의 한글 주석이 이미 깨진 문자(U+FFFD)로 저장돼 있었음. 새 DB 소스와 바이트 단위로 같음 (24줄 / 1,093바이트). 기능 영향 없음.
- DMSMGR 휴지통 18개(오늘 삭제한 백업 테이블)는 옮기지 않음.
- 기존 `oracle19c` 는 `docker stop` 기본 10초에 강제 종료(137)됨 -> 임시 기동으로 정상 OPEN 확인. compose 에 `stop_grace_period: 2m` 추가.

파일 위치 (`D:\docker\oracle19c\`, 저장소 밖):

- `docker-compose.yml` / `.env`(SYS 비밀번호 DMS_SYS_PWD / MES_SYS_PWD) / `tnsnames.ora.new`(적용할 tnsnames)
- `backup\`: 기존 컨테이너 inspect / tnsnames 원본 / 스키마별 기준값(old / new)
- `dump\`: `dmsmgr_20261013_*.dmp` / `mesmgr_20261013_*.dmp` / `factory_sck_20261013.dmp` / 기존 pfile / export·import 로그

남은 것:

1. `tnsnames.ora` 반영: 관리자 권한 파일이라 Claude 수정 불가(EPERM). `D:\docker\oracle19c\tnsnames.ora.new` 를 관리자 권한으로 복사 (`DMS` 의 SERVICE_NAME MESMGR -> DMS, `MES` 항목 추가). 반영 전까지 앱 / Probe 는 DMS DB 에 접속하지 못함.
2. DBeaver 접속: 서비스 이름 MESMGR -> DMS (포트 1521 그대로). 실행 중인 DACrux 서비스(Framework / TEST)는 재기동.
3. ~~기존 볼륨 `oracle19c-data` 삭제~~ -> 2026-10-13 사용자 결정("지우자")으로 사용자가 `docker volume rm oracle19c-data` 실행, 삭제 확인(20.1GB 확보). 옛 DB(SID MESMGR)는 덤프(`D:\docker\oracle19c\dump\`)로만 복원 가능.
