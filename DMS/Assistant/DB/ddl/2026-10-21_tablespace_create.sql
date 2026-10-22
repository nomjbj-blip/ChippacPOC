-- 영역별 테이블스페이스 만들기 + 대상 사용자 QUOTA (2026-10-21)
-- 구성(테이블 / 인덱스를 나눈다, 적용 규칙은 2026-10-21_tablespace_apply.sql 과 같다)
--   TS_COM_TBL  / TS_COM_IDX  : TQC_*            공통(설정 / 사용자 / 권한 / 설비 / MES Lot 이력 / 로그)
--   TS_DMS_TBL  / TS_DMS_IDX  : TQD_*            Defect(KLARF 적재 / 이미지 / Setup)
--   TS_TEST_TBL / TS_TEST_IDX : 그 밖(TQP_* / TD_* / G_* 등) YMS. E-Test 업로드가 TD_* 를 만들 때 이 이름을 쓴다(09_TEST/DACrux.TEST.DSL/TD_TABLE.cs)
--   TS_GDS_TBL  / TS_GDS_IDX  : TQP_GDS_*        GDS 도면 / Chain / E-Test 결과(가장 크다, BLOB 도 TS_GDS_TBL)
-- 실행 계정: CREATE TABLESPACE / ALTER USER 권한이 있는 계정(DBA). 대상 사용자는 아래 DEFINE TARGET_USER(기본 TRAINING).
-- 데이터 파일 위치: db_create_file_dest 가 있으면 그 위치(OMF), 없으면 USERS(없으면 SYSTEM) 데이터 파일 폴더에 <이름>.dbf
--   (ASM 이면 같은 디스크 그룹). DBA 가 정한 위치가 있으면 아래 DEFINE FILE_DIR 에 폴더(끝에 / 또는 \)를 넣는다.
-- 크기: 아래 표의 시작 크기, NEXT 50M 자동 증가, 최대 UNLIMITED. 로컬 기준 DB 크기(2026-10-21)를 참고했다.
-- 이미 있는 테이블스페이스는 만들지 않고 QUOTA 만 준다(앞서 만든 TS_TEST_TBL / TS_TEST_IDX 포함). 여러 번 실행해도 된다.
-- 실행 기록: 현재 폴더의 tablespace_create_run.log
-- 실행 순서: 이 파일(DBA) -> 2026-10-21_tablespace_apply.sql(대상 사용자)

SET SERVEROUTPUT ON SIZE UNLIMITED
SPOOL tablespace_create_run.log
SET DEFINE ON
SET VERIFY OFF
DEFINE TARGET_USER = TRAINING
DEFINE FILE_DIR = ''
WHENEVER SQLERROR EXIT SQL.SQLCODE

DECLARE
    v_user VARCHAR2(128) := UPPER('&TARGET_USER');
    v_dir  VARCHAR2(600) := '&FILE_DIR';
    v_omf  VARCHAR2(512);
    v_file VARCHAR2(600);
    v_loc  VARCHAR2(600);
    n      NUMBER;

    PROCEDURE make_ts(p_name VARCHAR2, p_size_mb NUMBER) IS
        v_sql VARCHAR2(2000);
    BEGIN
        SELECT COUNT(*) INTO n FROM DBA_TABLESPACES WHERE TABLESPACE_NAME = p_name;
        IF n > 0 THEN
            DBMS_OUTPUT.PUT_LINE('exists : ' || p_name);
        ELSE
            IF v_dir IS NOT NULL THEN
                v_sql := 'CREATE TABLESPACE ' || p_name || ' DATAFILE ''' || v_dir || p_name || '.dbf'' SIZE ' || p_size_mb || 'M';
            ELSIF v_omf IS NOT NULL THEN
                v_sql := 'CREATE TABLESPACE ' || p_name || ' DATAFILE SIZE ' || p_size_mb || 'M';
            ELSIF v_loc LIKE '+%' THEN
                v_sql := 'CREATE TABLESPACE ' || p_name || ' DATAFILE ''' || v_loc || ''' SIZE ' || p_size_mb || 'M';
            ELSE
                v_sql := 'CREATE TABLESPACE ' || p_name || ' DATAFILE ''' || v_loc || p_name || '.dbf'' SIZE ' || p_size_mb || 'M';
            END IF;
            v_sql := v_sql || ' AUTOEXTEND ON NEXT 50M MAXSIZE UNLIMITED'
                           || ' EXTENT MANAGEMENT LOCAL AUTOALLOCATE SEGMENT SPACE MANAGEMENT AUTO';
            DBMS_OUTPUT.PUT_LINE(v_sql || ';');
            EXECUTE IMMEDIATE v_sql;
            DBMS_OUTPUT.PUT_LINE('created: ' || p_name);
        END IF;
        v_sql := 'ALTER USER "' || v_user || '" QUOTA UNLIMITED ON ' || p_name;
        DBMS_OUTPUT.PUT_LINE(v_sql || ';');
        EXECUTE IMMEDIATE v_sql;
    END;
BEGIN
    SELECT COUNT(*) INTO n FROM DBA_USERS WHERE USERNAME = v_user;
    IF n = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'STOP: user ' || v_user || ' not found. Check DEFINE TARGET_USER.');
    END IF;

    SELECT VALUE INTO v_omf FROM V$PARAMETER WHERE NAME = 'db_create_file_dest';
    IF v_dir IS NULL AND v_omf IS NULL THEN
        SELECT FILE_NAME INTO v_file
          FROM (SELECT FILE_NAME FROM DBA_DATA_FILES
                 WHERE TABLESPACE_NAME IN ('USERS', 'SYSTEM')
                 ORDER BY DECODE(TABLESPACE_NAME, 'USERS', 1, 2), FILE_ID)
         WHERE ROWNUM = 1;
        IF v_file LIKE '+%' THEN
            v_loc := SUBSTR(v_file, 1, INSTR(v_file || '/', '/') - 1);
        ELSE
            v_loc := SUBSTR(v_file, 1, GREATEST(INSTR(v_file, '/', -1), INSTR(v_file, '\', -1)));
        END IF;
    END IF;
    DBMS_OUTPUT.PUT_LINE('datafile location: ' || CASE WHEN v_dir IS NOT NULL THEN v_dir WHEN v_omf IS NOT NULL THEN 'OMF ' || v_omf ELSE v_loc END);

    -- 이름 / 시작 크기(MB)
    make_ts('TS_COM_TBL', 200);
    make_ts('TS_COM_IDX', 200);
    make_ts('TS_DMS_TBL', 100);
    make_ts('TS_DMS_IDX', 100);
    make_ts('TS_TEST_TBL', 100);
    make_ts('TS_TEST_IDX', 200);
    make_ts('TS_GDS_TBL', 1024);
    make_ts('TS_GDS_IDX', 300);
END;
/

PROMPT == 확인 (8개 ONLINE / 대상 사용자 QUOTA -1(UNLIMITED)) ==
SET PAGESIZE 50 LINESIZE 200
COLUMN TABLESPACE_NAME FORMAT A15
COLUMN FILE_NAME FORMAT A70
SELECT T.TABLESPACE_NAME, T.STATUS, F.FILE_NAME, ROUND(F.BYTES / 1024 / 1024) MB, F.AUTOEXTENSIBLE, Q.MAX_BYTES QUOTA
  FROM DBA_TABLESPACES T
  JOIN DBA_DATA_FILES F ON F.TABLESPACE_NAME = T.TABLESPACE_NAME
  LEFT JOIN DBA_TS_QUOTAS Q ON Q.TABLESPACE_NAME = T.TABLESPACE_NAME AND Q.USERNAME = UPPER('&TARGET_USER')
 WHERE T.TABLESPACE_NAME IN ('TS_COM_TBL', 'TS_COM_IDX', 'TS_DMS_TBL', 'TS_DMS_IDX', 'TS_TEST_TBL', 'TS_TEST_IDX', 'TS_GDS_TBL', 'TS_GDS_IDX')
 ORDER BY 1;
SPOOL OFF
