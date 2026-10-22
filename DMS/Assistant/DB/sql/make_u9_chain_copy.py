"""
로컬 DMSMGR 의 GDS Chain 묶음을 다른 DB(고객 TRAINING 등)의 같은 GDS Map 으로 옮기는 SQL 을 만든다 (2026-10-21).

입력(로컬에서 sqlplus spool 로 받은 '|' 구분 파일, 폴더 = 첫 번째 인자):
  chains.txt  : CHAIN_SEQ|CHAIN_CODE|CHAIN_STATUS|CHAIN_NAME|COLOR_ARGB|APPLY_LAYER_RULES|CELL_SIZE|VERSION_STATUS|IS_CURRENT|CHAIN_CREATE_USER|VER_CREATE_USER
  layers.txt  : CHAIN_SEQ|VERSION_NO|LAYER_ID|DATA_TYPE
  members.txt : CHAIN_SEQ|VERSION_NO|PLACED_ELEMENT_ID|ROLE_CODE|INCLUSION_SOURCE|MARKER_X_HEX|MARKER_Y_HEX
  edits.txt   : CHAIN_SEQ|VERSION_NO|EDIT_SEQ|PLACED_ELEMENT_ID|ACTION_CODE|BATCH_EDIT_SEQ|CREATE_USER
  inputs.txt  : PLACED_ELEMENT_ID|LAYER_ID|DATA_TYPE|MIN_X_HEX|MIN_Y_HEX  (각 Chain 시작 도형의 위치, 같은 GDS 인지 확인용)
출력: 두 번째 인자 경로의 SQL (Oracle 11g 이상, 스키마 이름 없음, 한 트랜잭션).

SQL 동작
  0) 사전 검사: 대상 READY Map / 필요한 Layer / 시작 도형 19개의 Layer·위치가 원본과 같은지. 다르면 아무것도 쓰지 않고 멈춘다.
  1) Chain 마다: 같은 이름이 있으면 건너뜀. Code 만 겹치면 비어 있는 다음 Code(A, B, ... Z, AA ...)를 쓴다.
     CHAIN / CHAIN_VER(1) / CHAIN_LAYER / INPUT 행(Marker = 원본 비트 그대로) 생성.
  2) 포함 도형 / 수동 편집 기록: 300행씩 INSERT ... SELECT. 대상 Map 에 없는 도형이면 FK 오류로 멈추고 전부 되돌린다.
  3) 건수 확인(원본과 다르면 되돌림) -> COMMIT.
"""
import os
import sys

src = sys.argv[1]
out = sys.argv[2]
CHUNK = 300


def rows(name):
    with open(os.path.join(src, name), encoding='utf-8') as f:
        return [line.rstrip('\r\n').split('|') for line in f if line.strip()]


def q(text):
    return "'" + text.replace("'", "''") + "'"


chains = rows('chains.txt')
layers = rows('layers.txt')
members = rows('members.txt')
edits = rows('edits.txt')
inputs = rows('inputs.txt')

seq_ids = [c[0] for c in chains]
var = {seq: 'S_' + c[1] for seq, c in zip(seq_ids, chains)}
layers_by = {}
for r in layers:
    layers_by.setdefault(r[0], []).append(r)
members_by = {}
for r in members:
    members_by.setdefault(r[0], []).append(r)
edits_by = {}
for r in edits:
    edits_by.setdefault(r[0], []).append(r)
need_layers = sorted({(int(r[2]), int(r[3])) for r in layers})

L = []
w = L.append
w('-- GDS Chain 묶음 복사: 로컬 DMSMGR SCK / OMM_TCB (2026-10-15 DWG Chain 도면 기준) -> 대상 DB 의 &factory / &device Map (2026-10-21)')
w('-- Chain %d / 포함 도형 %d / 수동 편집 기록 %d. 생성: DMS/Assistant/DB/sql/make_u9_chain_copy.py' % (len(chains), len(members), len(edits)))
w('-- 대상 Map 은 같은 GDS(OMM all layer2.gds) + 같은 Layer(%s)로 저장되어 있어야 한다.' % ', '.join('%d/%d' % k for k in need_layers))
w('-- 실행: 대상 사용자(예 TRAINING)로 접속해 @이 파일. 한 트랜잭션이라 오류가 나면 모두 되돌린다(ROLLBACK). 다시 실행하면 이미 만든 이름은 건너뛴다.')
w('SET DEFINE ON')
w("DEFINE factory = 'SCK'")
w("DEFINE device = 'U9'")
w('SET SERVEROUTPUT ON SIZE UNLIMITED')
w('SET FEEDBACK OFF VERIFY OFF ECHO OFF')
w('WHENEVER SQLERROR EXIT FAILURE ROLLBACK')
w('WHENEVER OSERROR EXIT FAILURE ROLLBACK')
w('SPOOL u9_chain_copy_run.log')
w("PROMPT === 대상: &factory / &device")
w('')
w('-- 0) 사전 검사')
w('DECLARE')
w('  n NUMBER;')
w('  PROCEDURE need_layer(p_layer NUMBER, p_type NUMBER) IS c NUMBER; BEGIN')
w("    SELECT COUNT(*) INTO c FROM TQP_GDS_LAYER WHERE FACTORY = '&factory' AND DEVICE_ID = '&device' AND LAYER_ID = p_layer AND DATA_TYPE = p_type;")
w("    IF c = 0 THEN RAISE_APPLICATION_ERROR(-20002, 'Layer ' || p_layer || '/' || p_type || ' 가 대상 Map 에 없습니다.'); END IF;")
w('  END;')
w('  PROCEDURE same_el(p_id VARCHAR2, p_layer NUMBER, p_type NUMBER, p_x VARCHAR2, p_y VARCHAR2) IS c NUMBER; BEGIN')
w("    SELECT COUNT(*) INTO c FROM TQP_GDS_PLACED_EL WHERE FACTORY = '&factory' AND DEVICE_ID = '&device' AND PLACED_ELEMENT_ID = p_id")
w('      AND LAYER_ID = p_layer AND DATA_TYPE = p_type')
w('      AND MIN_X = UTL_RAW.CAST_TO_BINARY_DOUBLE(HEXTORAW(p_x)) AND MIN_Y = UTL_RAW.CAST_TO_BINARY_DOUBLE(HEXTORAW(p_y));')
w("    IF c = 0 THEN RAISE_APPLICATION_ERROR(-20003, '시작 도형 ' || p_id || ' 이 없거나 위치 / Layer 가 원본과 다릅니다(다른 GDS 또는 다른 Layer 로 만든 Map).'); END IF;")
w('  END;')
w('BEGIN')
w("  SELECT COUNT(*) INTO n FROM TQP_GDS_MAP WHERE FACTORY = '&factory' AND DEVICE_ID = '&device' AND MAP_STATUS = 'READY';")
w("  IF n <> 1 THEN RAISE_APPLICATION_ERROR(-20001, 'READY 상태 GDS Map 이 없습니다: &factory / &device'); END IF;")
for k in need_layers:
    w('  need_layer(%d, %d);' % k)
for r in inputs:
    w('  same_el(%s, %s, %s, %s, %s);' % (q(r[0]), r[1], r[2], q(r[3]), q(r[4])))
w("  SELECT COUNT(*) INTO n FROM TQP_GDS_PLACED_EL WHERE FACTORY = '&factory' AND DEVICE_ID = '&device';")
w("  DBMS_OUTPUT.PUT_LINE('사전 검사 통과: 대상 도형 ' || n || '개 (원본 Map 204898개) / Layer %d개 / 시작 도형 %d개 위치 같음');" % (len(need_layers), len(inputs)))
w('END;')
w('/')
w('')
for seq in seq_ids:
    w('VARIABLE %s NUMBER' % var[seq])
w('')
w('-- 1) Chain / 버전 / 탐색 Layer / 시작 도형(INPUT)')
for seq, c in zip(seq_ids, chains):
    _, code, status, name, color, rules, cell, vstatus, cur, cuser, vuser = c
    v = var[seq]
    w('DECLARE')
    w('  n NUMBER; s NUMBER; c VARCHAR2(128); i NUMBER := 0;')
    w('  FUNCTION code_of(p NUMBER) RETURN VARCHAR2 IS x NUMBER := p; r VARCHAR2(10) := NULL; BEGIN')
    w("    LOOP r := CHR(65 + MOD(x, 26)) || r; x := TRUNC(x / 26) - 1; EXIT WHEN x < 0; END LOOP; RETURN r; END;")
    w('BEGIN')
    w('  SELECT COUNT(*) INTO n FROM TQP_GDS_CHAIN_VER WHERE FACTORY = \'&factory\' AND DEVICE_ID = \'&device\' AND CHAIN_NAME = %s;' % q(name))
    w('  IF n > 0 THEN')
    w('    :%s := NULL;' % v)
    w("    DBMS_OUTPUT.PUT_LINE('건너뜀(같은 이름 있음): ' || %s);" % q(name))
    w('  ELSE')
    w('    c := %s;' % q(code))
    w("    LOOP SELECT COUNT(*) INTO n FROM TQP_GDS_CHAIN WHERE FACTORY = '&factory' AND DEVICE_ID = '&device' AND CHAIN_CODE = c;")
    w('      EXIT WHEN n = 0; c := code_of(i); i := i + 1; END LOOP;')
    w('    SELECT CHAIN_SEQ.NEXTVAL INTO s FROM DUAL;')
    w('    :%s := s;' % v)
    w('    INSERT INTO TQP_GDS_CHAIN (FACTORY, DEVICE_ID, CHAIN_SEQ, CHAIN_CODE, CHAIN_STATUS, CREATE_USER, CREATE_TIME)')
    w("      VALUES ('&factory', '&device', s, c, %s, %s, SYSDATE);" % (q(status), q(cuser)))
    w('    INSERT INTO TQP_GDS_CHAIN_VER (FACTORY, DEVICE_ID, CHAIN_SEQ, VERSION_NO, CHAIN_NAME, COLOR_ARGB, APPLY_LAYER_RULES, CELL_SIZE, VERSION_STATUS, IS_CURRENT, CREATE_USER, CREATE_TIME)')
    w("      VALUES ('&factory', '&device', s, 1, %s, %s, %s, %s, %s, %s, %s, SYSDATE);" % (q(name), color, rules, cell, q(vstatus), cur, q(vuser)))
    for r in layers_by.get(seq, []):
        w("    INSERT INTO TQP_GDS_CHAIN_LAYER (FACTORY, DEVICE_ID, CHAIN_SEQ, VERSION_NO, LAYER_ID, DATA_TYPE) VALUES ('&factory', '&device', s, %s, %s, %s);" % (r[1], r[2], r[3]))
    for r in members_by.get(seq, []):
        if r[5]:
            w('    INSERT INTO TQP_GDS_CHAIN_MEMBER (FACTORY, DEVICE_ID, CHAIN_SEQ, VERSION_NO, PLACED_ELEMENT_ID, ROLE_CODE, INCLUSION_SOURCE, MARKER_X, MARKER_Y)')
            w("      VALUES ('&factory', '&device', s, %s, %s, %s, %s, UTL_RAW.CAST_TO_BINARY_DOUBLE(HEXTORAW(%s)), UTL_RAW.CAST_TO_BINARY_DOUBLE(HEXTORAW(%s)));"
              % (r[1], q(r[2]), q(r[3]), q(r[4]), q(r[5]), q(r[6])))
    w("    DBMS_OUTPUT.PUT_LINE('생성: ' || %s || ' (Code ' || c || ')');" % q(name))
    w('  END IF;')
    w('END;')
    w('/')
w('')
w('-- 2) 포함 도형(INPUT 외) / 수동 편집 기록')
for seq in seq_ids:
    v = var[seq]
    plain = [r for r in members_by.get(seq, []) if not r[5]]
    for i in range(0, len(plain), CHUNK):
        part = plain[i:i + CHUNK]
        w('INSERT INTO TQP_GDS_CHAIN_MEMBER (FACTORY, DEVICE_ID, CHAIN_SEQ, VERSION_NO, PLACED_ELEMENT_ID, ROLE_CODE, INCLUSION_SOURCE)')
        w("SELECT '&factory', '&device', :%s, x.v, x.e, x.r, x.s FROM (" % v)
        for j, r in enumerate(part):
            w('SELECT %s v, %s e, %s r, %s s FROM DUAL%s' % (r[1], q(r[2]), q(r[3]), q(r[4]), ' UNION ALL' if j < len(part) - 1 else ''))
        w(') x WHERE :%s IS NOT NULL;' % v)
    ed = edits_by.get(seq, [])
    for i in range(0, len(ed), CHUNK):
        part = ed[i:i + CHUNK]
        w('INSERT INTO TQP_GDS_CHAIN_EDIT (FACTORY, DEVICE_ID, CHAIN_SEQ, VERSION_NO, EDIT_SEQ, PLACED_ELEMENT_ID, ACTION_CODE, BATCH_EDIT_SEQ, CREATE_USER, CREATE_TIME)')
        w("SELECT '&factory', '&device', :%s, x.v, x.q, x.e, x.a, x.b, x.u, SYSDATE FROM (" % v)
        for j, r in enumerate(part):
            b = r[5] if r[5] else 'NULL'
            w('SELECT %s v, %s q, %s e, %s a, %s b, %s u FROM DUAL%s'
              % (r[1], r[2], q(r[3]), q(r[4]), 'CAST(%s AS NUMBER)' % b, q(r[6]), ' UNION ALL' if j < len(part) - 1 else ''))
        w(') x WHERE :%s IS NOT NULL;' % v)
w('')
w('-- 3) 건수 확인 후 COMMIT (원본과 다르면 되돌린다)')
w('DECLARE')
w('  made NUMBER := 0; skipped NUMBER := 0;')
w('  PROCEDURE chk(p_seq NUMBER, p_name VARCHAR2, p_mem NUMBER, p_edit NUMBER, p_layer NUMBER) IS m NUMBER; e NUMBER; l NUMBER; BEGIN')
w('    IF p_seq IS NULL THEN skipped := skipped + 1; RETURN; END IF;')
w("    SELECT COUNT(*) INTO m FROM TQP_GDS_CHAIN_MEMBER WHERE FACTORY = '&factory' AND DEVICE_ID = '&device' AND CHAIN_SEQ = p_seq;")
w("    SELECT COUNT(*) INTO e FROM TQP_GDS_CHAIN_EDIT WHERE FACTORY = '&factory' AND DEVICE_ID = '&device' AND CHAIN_SEQ = p_seq;")
w("    SELECT COUNT(*) INTO l FROM TQP_GDS_CHAIN_LAYER WHERE FACTORY = '&factory' AND DEVICE_ID = '&device' AND CHAIN_SEQ = p_seq;")
w("    IF m <> p_mem OR e <> p_edit OR l <> p_layer THEN RAISE_APPLICATION_ERROR(-20010, p_name || ' 건수 다름: 도형 ' || m || '/' || p_mem || ' 편집 ' || e || '/' || p_edit || ' Layer ' || l || '/' || p_layer); END IF;")
w("    made := made + 1; DBMS_OUTPUT.PUT_LINE('확인: ' || p_name || ' 도형 ' || m || ' / 편집 ' || e || ' / Layer ' || l);")
w('  END;')
w('BEGIN')
for seq, c in zip(seq_ids, chains):
    w('  chk(:%s, %s, %d, %d, %d);' % (var[seq], q(c[3]), len(members_by.get(seq, [])), len(edits_by.get(seq, [])), len(layers_by.get(seq, []))))
w("  DBMS_OUTPUT.PUT_LINE('결과: 생성 ' || made || ' / 건너뜀 ' || skipped);")
w('END;')
w('/')
w('COMMIT;')
w("PROMPT === 완료(COMMIT)")
w('SPOOL OFF')
w('EXIT')
with open(out, 'w', encoding='utf-8', newline='\n') as f:
    f.write('\n'.join(L) + '\n')
print('chains', len(chains), 'members', len(members), 'edits', len(edits), 'lines', len(L), 'bytes', os.path.getsize(out))
