-- TD_* 테이블 / 인덱스를 원본과 같은 테이블스페이스로 옮기기 (2026-10-21)
-- 원본(로컬 DMSMGR) 배치: TD_* 테이블은 TS_TEST_TBL, TD_* 테이블의 인덱스(PK)는 TS_TEST_IDX, 나머지 객체는 사용자 기본 테이블스페이스.
--   E-Test 업로드가 새 TD_* 테이블을 만들 때도 같은 규칙을 쓴다(09_TEST/DACrux.TEST.DSL/TD_TABLE.cs).
-- 스키마 DDL 은 테이블스페이스 절 없이 만들어지므로, DDL 실행 뒤 이 파일로 TD_* 만 옮긴다(나머지는 그대로).
-- 실행 계정: 객체를 가진 사용자(예: TRAINING). 먼저 2026-10-21_ts_test_tablespace.sql 로 두 테이블스페이스와 QUOTA 가 있어야 한다.
-- 데이터가 없는 상태에서 실행한다고 보고 MOVE / REBUILD 를 바로 실행한다. 여러 번 실행해도 된다(이미 옮긴 것은 건너뜀).

SET SERVEROUTPUT ON SIZE UNLIMITED
WHENEVER SQLERROR EXIT SQL.SQLCODE

DECLARE
    v_owner VARCHAR2(128) := SYS_CONTEXT('USERENV', 'CURRENT_SCHEMA');
    v_cnt   NUMBER := 0;

    PROCEDURE run(p_sql VARCHAR2) IS
    BEGIN
        DBMS_OUTPUT.PUT_LINE(p_sql || ';');
        EXECUTE IMMEDIATE p_sql;
        v_cnt := v_cnt + 1;
    END;
BEGIN
    DBMS_OUTPUT.PUT_LINE('대상 스키마: ' || v_owner);

    -- 1) TD_* 테이블 -> TS_TEST_TBL
    FOR t IN (SELECT TABLE_NAME
                FROM ALL_TABLES
               WHERE OWNER = v_owner
                 AND TABLE_NAME LIKE 'TD\_%' ESCAPE '\'
                 AND NVL(TABLESPACE_NAME, '-') <> 'TS_TEST_TBL'
               ORDER BY TABLE_NAME)
    LOOP
        run('ALTER TABLE "' || v_owner || '"."' || t.TABLE_NAME || '" MOVE TABLESPACE TS_TEST_TBL');
    END LOOP;

    -- 2) TD_* 테이블의 인덱스 -> TS_TEST_IDX (테이블을 옮겨 UNUSABLE 이 된 인덱스도 다시 만든다)
    FOR i IN (SELECT INDEX_NAME
                FROM ALL_INDEXES
               WHERE OWNER = v_owner
                 AND TABLE_NAME LIKE 'TD\_%' ESCAPE '\'
                 AND INDEX_TYPE <> 'LOB'
                 AND (NVL(TABLESPACE_NAME, '-') <> 'TS_TEST_IDX' OR STATUS = 'UNUSABLE')
               ORDER BY INDEX_NAME)
    LOOP
        run('ALTER INDEX "' || v_owner || '"."' || i.INDEX_NAME || '" REBUILD TABLESPACE TS_TEST_IDX');
    END LOOP;

    DBMS_OUTPUT.PUT_LINE('실행한 문장: ' || v_cnt);
END;
/

PROMPT == 확인 (TD_* 테이블은 TS_TEST_TBL, 인덱스는 TS_TEST_IDX / VALID 이어야 한다) ==
SET PAGESIZE 100 LINESIZE 200
COLUMN TABLE_NAME FORMAT A30
COLUMN TBL_TS FORMAT A15
COLUMN INDEX_NAME FORMAT A30
COLUMN IDX_TS FORMAT A15
SELECT T.TABLE_NAME, T.TABLESPACE_NAME TBL_TS, I.INDEX_NAME, I.TABLESPACE_NAME IDX_TS, I.STATUS
  FROM ALL_TABLES T
  LEFT JOIN ALL_INDEXES I ON I.TABLE_OWNER = T.OWNER AND I.TABLE_NAME = T.TABLE_NAME AND I.INDEX_TYPE <> 'LOB'
 WHERE T.OWNER = SYS_CONTEXT('USERENV', 'CURRENT_SCHEMA')
   AND T.TABLE_NAME LIKE 'TD\_%' ESCAPE '\'
 ORDER BY 1, 3;
