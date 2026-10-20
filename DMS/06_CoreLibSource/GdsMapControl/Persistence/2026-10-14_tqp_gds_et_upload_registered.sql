-- 2026-10-14: E-Test Upload 재설계 구현 (등록 Parameter 만 저장 + Spec 판정 저장 + Map 좌표) + Para Setup 2단계 나머지(2b)
-- 설계: 문서/설계/04_ETest/2026-10-13_ETest_Upload_재설계.md 9.3절 / 10절, 문서/설계/04_ETest/2026-10-14_ETest_Para_Setup_설계.md 4.7절 2b
--
-- 전제: 로컬은 2026-10-14_tqp_gds_et_local_reset.sql 을 먼저 실행한다. 예전 업로드 측정 항목(TEST_PROGRAM NULL)이 남아 있으면 0) 에서 멈춘다.
-- 내용
--  1) TQP_GDS_ET_ITEM.TEST_PROGRAM NOT NULL (모든 측정 항목 = 등록 Parameter)
--  2) TQP_GDS_SRC_FILE: HEADER_COUNT(파일 전체 헤더) / DEFINED_COUNT(업로드 때 Device + 공정 등록 Parameter(사용 Y) 수) /
--     MISSING_COUNT(등록됐는데 파일에 없음) / JUDGE_TIME / MAP_ID(좌표에 쓴 Map, 없으면 NULL) / MAP_TIME 추가.
--     VIEW_SIDE NULL 허용 + 기본값('BACK') 제거(새 저장부터 채우지 않음, 면은 분석 때 선택). 의미 변경: ITEM_COUNT = 저장한 등록 Parameter 수,
--     PASS_DIE_COUNT / FAIL_DIE_COUNT = Spec 판정 기준, PARSE_RULE_VER 3 = 등록 Parameter 만 저장.
--  3) TQP_GDS_ET_DIE: MAP_X / MAP_Y(시스템 Map 좌표) / JUDGE_FLAG(P / F) / FAIL_COUNT / NOT_TESTED_COUNT 추가. PASS_FLAG(DUT Flag) / HW·SW Bin 은 원본 참고로 남김.
--  4) TQP_GDS_ET_VALUE: JUDGE_FLAG(P / F, NULL = Spec 없음 · Enable N · 빈 칸이라 판정 안 함) 추가.
-- 이 파일을 적용하면 이전 버전 업로드 화면(헤더 전체 저장 / MergeEtItem)은 동작하지 않는다. 서버 / 클라이언트를 같이 배포한다.
-- 적용: 로컬 docker oracle19c-DMS DMSMGR. 고객 DB 적용 여부는 별도 확인.

WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK
SET ECHO OFF FEEDBACK ON SERVEROUTPUT ON

-- 0) 사전 확인
DECLARE
    n_old NUMBER;
BEGIN
    SELECT COUNT(*) INTO n_old FROM DMSMGR.TQP_GDS_ET_ITEM WHERE TEST_PROGRAM IS NULL;
    DBMS_OUTPUT.PUT_LINE('예전 업로드 측정 항목(TEST_PROGRAM 없음) ' || n_old || '행');
    IF n_old > 0 THEN
        RAISE_APPLICATION_ERROR(-20001, '예전 업로드 측정 항목이 ' || n_old || '행 있습니다. 로컬은 2026-10-14_tqp_gds_et_local_reset.sql 을 먼저 실행하세요.');
    END IF;
END;
/

-- 1) 측정 항목 = 등록 Parameter
ALTER TABLE DMSMGR.TQP_GDS_ET_ITEM MODIFY (TEST_PROGRAM NOT NULL);

-- 2) 파일
ALTER TABLE DMSMGR.TQP_GDS_SRC_FILE ADD (
        HEADER_COUNT     NUMBER(10),
        DEFINED_COUNT    NUMBER(10),
        MISSING_COUNT    NUMBER(10),
        JUDGE_TIME       DATE,
        MAP_ID           VARCHAR2(50 CHAR),
        MAP_TIME         DATE
);
ALTER TABLE DMSMGR.TQP_GDS_SRC_FILE MODIFY (VIEW_SIDE DEFAULT NULL NULL);

COMMENT ON COLUMN DMSMGR.TQP_GDS_SRC_FILE.HEADER_COUNT IS '파일 전체 측정 헤더 수';
COMMENT ON COLUMN DMSMGR.TQP_GDS_SRC_FILE.DEFINED_COUNT IS '업로드 때 Device + 공정 등록 Parameter(사용 Y) 수';
COMMENT ON COLUMN DMSMGR.TQP_GDS_SRC_FILE.MISSING_COUNT IS '등록됐는데 파일에 없는 Parameter 수';
COMMENT ON COLUMN DMSMGR.TQP_GDS_SRC_FILE.JUDGE_TIME IS 'Spec 판정 시각 (업로드 / 다시 판정)';
COMMENT ON COLUMN DMSMGR.TQP_GDS_SRC_FILE.MAP_ID IS 'Die 좌표 계산에 쓴 Map ID (TQP_USEMAP, = DEVICE_ID). 없으면 NULL';
COMMENT ON COLUMN DMSMGR.TQP_GDS_SRC_FILE.MAP_TIME IS 'Map 좌표 계산 시각';
COMMENT ON COLUMN DMSMGR.TQP_GDS_SRC_FILE.ITEM_COUNT IS '저장한 등록 Parameter 수 (PARSE_RULE_VER 3)';
COMMENT ON COLUMN DMSMGR.TQP_GDS_SRC_FILE.PASS_DIE_COUNT IS 'Spec 판정 Pass Die 수 (PARSE_RULE_VER 3)';
COMMENT ON COLUMN DMSMGR.TQP_GDS_SRC_FILE.FAIL_DIE_COUNT IS 'Spec 판정 Fail Die 수 (PARSE_RULE_VER 3)';

-- 3) Die
ALTER TABLE DMSMGR.TQP_GDS_ET_DIE ADD (
        MAP_X            NUMBER(10),
        MAP_Y            NUMBER(10),
        JUDGE_FLAG       CHAR(1),
        FAIL_COUNT       NUMBER(10),
        NOT_TESTED_COUNT NUMBER(10),
        CONSTRAINT CK_GDS_ET_DIE_JUDGE CHECK (JUDGE_FLAG IS NULL OR JUDGE_FLAG IN ('P', 'F'))
);

COMMENT ON COLUMN DMSMGR.TQP_GDS_ET_DIE.MAP_X IS '시스템 Map 좌표 X (Map 사용 Die 가 아니거나 Map 이 없으면 NULL)';
COMMENT ON COLUMN DMSMGR.TQP_GDS_ET_DIE.MAP_Y IS '시스템 Map 좌표 Y';
COMMENT ON COLUMN DMSMGR.TQP_GDS_ET_DIE.JUDGE_FLAG IS 'Spec 판정 P / F (Fail 값이 하나라도 있으면 F)';
COMMENT ON COLUMN DMSMGR.TQP_GDS_ET_DIE.FAIL_COUNT IS 'Fail 값 수 (범위 밖 + Enable Spec 의 ''-'')';
COMMENT ON COLUMN DMSMGR.TQP_GDS_ET_DIE.NOT_TESTED_COUNT IS '저장한 Parameter 의 ''-'' 값 수';
COMMENT ON COLUMN DMSMGR.TQP_GDS_ET_DIE.PASS_FLAG IS '파일 DUT Flag (원본 참고, 판정 아님)';

-- 4) 값
ALTER TABLE DMSMGR.TQP_GDS_ET_VALUE ADD (
        JUDGE_FLAG       CHAR(1),
        CONSTRAINT CK_GDS_ET_VALUE_JUDGE CHECK (JUDGE_FLAG IS NULL OR JUDGE_FLAG IN ('P', 'F'))
);

COMMENT ON COLUMN DMSMGR.TQP_GDS_ET_VALUE.JUDGE_FLAG IS 'Spec 판정 P / F. NULL = Spec 없음 / Enable N / 빈 칸';
