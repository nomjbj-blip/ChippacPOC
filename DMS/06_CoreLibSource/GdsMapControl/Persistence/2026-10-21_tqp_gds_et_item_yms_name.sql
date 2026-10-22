-- 2026-10-21: TQP_GDS_ET_ITEM.YMS_PARAM_NAME (E-Test Parameter 의 YMS TD 컬럼 이름, 30자)
-- 설계: 문서/설계/04_ETest/2026-10-21_ETest_YMS_컬럼이름_30자_설계.md 6절
-- 이유: 고객 DB(Oracle 11.2.0.4)는 테이블 / 컬럼 이름이 30바이트까지라 Parameter 이름(ITEM_NAME, 최대 50자)을 TD 컬럼 이름으로 그대로 쓸 수 없다.
--       등록 때 EtYmsRule.ToYmsName 으로 한 번 만들어 저장하고 이후 자동으로 바꾸지 않는다.
-- 실행: 객체를 가진 사용자로 접속해 실행한다(로컬 DMSMGR / 고객 TRAINING). 스키마 이름을 붙이지 않는다. 여러 번 실행해도 된다.
--       sysdba 로 실행할 때는 먼저 ALTER SESSION SET CURRENT_SCHEMA = <사용자>; 를 실행한다.
-- 순서: 1 컬럼 추가 -> 2 빈 값 확인 -> 3 NOT NULL / CHECK / UQ
--       등록된 행이 있는 DB 는 2 에서 멈춘다. Probes/EtYmsNameFill.ps1 로 기존 행을 채운 뒤 이 파일을 다시 실행한다.

WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK
SET SERVEROUTPUT ON FEEDBACK OFF

-- 1. 컬럼
DECLARE
    v_owner VARCHAR2(128) := SYS_CONTEXT('USERENV', 'CURRENT_SCHEMA');
    n       NUMBER;
BEGIN
    SELECT COUNT(*) INTO n FROM ALL_TAB_COLUMNS
     WHERE OWNER = v_owner AND TABLE_NAME = 'TQP_GDS_ET_ITEM' AND COLUMN_NAME = 'YMS_PARAM_NAME';
    IF n = 0 THEN
        EXECUTE IMMEDIATE 'ALTER TABLE TQP_GDS_ET_ITEM ADD (YMS_PARAM_NAME VARCHAR2(30 CHAR))';
        DBMS_OUTPUT.PUT_LINE('1. YMS_PARAM_NAME 컬럼 추가');
    ELSE
        DBMS_OUTPUT.PUT_LINE('1. YMS_PARAM_NAME 컬럼 있음');
    END IF;
END;
/
COMMENT ON COLUMN TQP_GDS_ET_ITEM.YMS_PARAM_NAME IS 'YMS TD 컬럼 이름(30자 이하). ITEM_NAME 이 30자 이하면 같은 값, 넘으면 측정 약어 + 조건 + Chain 으로 줄인 이름. 등록 때 만들고 자동으로 바꾸지 않는다. Para Setup 에서 고칠 수 있다(YMS 반영 전만)';

-- 2. 빈 값 확인
DECLARE
    n NUMBER;
BEGIN
    SELECT COUNT(*) INTO n FROM TQP_GDS_ET_ITEM WHERE YMS_PARAM_NAME IS NULL;
    IF n > 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'YMS_PARAM_NAME 이 빈 행 ' || n || '개. Probes/EtYmsNameFill.ps1 로 채운 뒤 다시 실행한다.');
    END IF;
    DBMS_OUTPUT.PUT_LINE('2. 빈 값 없음');
END;
/

-- 3. NOT NULL / CHECK / UQ
DECLARE
    v_owner VARCHAR2(128) := SYS_CONTEXT('USERENV', 'CURRENT_SCHEMA');
    v_null  VARCHAR2(1);
    n       NUMBER;
BEGIN
    SELECT NULLABLE INTO v_null FROM ALL_TAB_COLUMNS
     WHERE OWNER = v_owner AND TABLE_NAME = 'TQP_GDS_ET_ITEM' AND COLUMN_NAME = 'YMS_PARAM_NAME';
    IF v_null = 'Y' THEN
        EXECUTE IMMEDIATE 'ALTER TABLE TQP_GDS_ET_ITEM MODIFY (YMS_PARAM_NAME NOT NULL)';
        DBMS_OUTPUT.PUT_LINE('3. NOT NULL');
    END IF;

    SELECT COUNT(*) INTO n FROM ALL_CONSTRAINTS WHERE OWNER = v_owner AND CONSTRAINT_NAME = 'CK_GDS_ET_ITEM_YMS';
    IF n = 0 THEN
        EXECUTE IMMEDIATE 'ALTER TABLE TQP_GDS_ET_ITEM ADD CONSTRAINT CK_GDS_ET_ITEM_YMS'
            || ' CHECK (REGEXP_LIKE(YMS_PARAM_NAME, ''^[A-Z][A-Z0-9_]*$'') AND LENGTH(YMS_PARAM_NAME) <= 30)';
        DBMS_OUTPUT.PUT_LINE('3. CK_GDS_ET_ITEM_YMS');
    END IF;

    SELECT COUNT(*) INTO n FROM ALL_CONSTRAINTS WHERE OWNER = v_owner AND CONSTRAINT_NAME = 'UQ_GDS_ET_ITEM_YMS';
    IF n = 0 THEN
        EXECUTE IMMEDIATE 'ALTER TABLE TQP_GDS_ET_ITEM ADD CONSTRAINT UQ_GDS_ET_ITEM_YMS'
            || ' UNIQUE (FACTORY, DEVICE_ID, TEST_PROGRAM, YMS_PARAM_NAME)';
        DBMS_OUTPUT.PUT_LINE('3. UQ_GDS_ET_ITEM_YMS');
    END IF;
    DBMS_OUTPUT.PUT_LINE('완료');
END;
/

EXIT
