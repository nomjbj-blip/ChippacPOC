# -*- coding: utf-8 -*-
"""
F07 데이터 검증 1-E: E-Test 원본 xlsx(DUT Summary) -> DB(TQP_GDS_ET_FILE / DIE / VALUE) + Spec 판정 대조.
원본은 openpyxl 로 직접 읽는다(제품 Parser 를 쓰지 않음). 판정은 DB Spec 표의 원문 규칙(SPEC_TEXT)으로 다시 계산한다.
인자: <xlsx> <db csv 폴더>
"""
import csv, io, os, re, sys
from collections import Counter
import openpyxl

xlsx, db = sys.argv[1], sys.argv[2]
fails = 0
checks = 0


def check(name, ok, detail=''):
    global fails, checks
    checks += 1
    if not ok:
        fails += 1
    print('%s / %s / %s' % ('PASS' if ok else 'FAIL', name, detail))


def rows(name):
    with io.open(os.path.join(db, name + '.csv'), encoding='utf-8-sig', newline='') as f:
        return list(csv.DictReader(f))


def num(v):
    try:
        return float(v)
    except (TypeError, ValueError):
        return None


# ---------- 원본 ----------
wb = openpyxl.load_workbook(xlsx, read_only=True, data_only=True)
ws = wb['DUT Summary']
grid = [list(r) for r in ws.iter_rows(values_only=True)]
head = [('' if v is None else str(v)).strip() for v in grid[0]]
fixed = head[:11]
test_no, upper, lower, unit = grid[1], grid[2], grid[3], grid[4]
duts = [r for r in grid[5:] if r[0] is not None and str(r[0]).startswith('#')]
col_of = {}
dup_header = Counter(head[11:])
for c in range(11, len(head)):
    col_of.setdefault(head[c], c)
content_wafers = Counter(str(r[head.index('Wafer ID')]) for r in duts)
print('원본: 열 %d (고정 %d + 측정 %d, 같은 이름 머리 %d) / DUT %d / Wafer ID 열 %s' % (len(head), 11, len(head) - 11,
      sum(1 for k, v in dup_header.items() if v > 1), len(duts), dict(content_wafers)))

# ---------- DB ----------
f = rows('e_file')[0]
die = rows('e_die')
val = rows('e_value')
item = rows('e_item')
lim = dict((r['ITEM_NAME'], r) for r in rows('e_limit'))
name = os.path.basename(xlsx)

# E1 파일 정보
check('E1 파일 정보: 파일 이름 / 공정 TSVET / 시각(파일 이름) / DUT 수 / 머리 수', f['FILE_NAME'] == name and f['TEST_PROGRAM'] == 'TSVET'
      and f['TEST_TIME'] == '20261002161846' and int(f['DIE_COUNT']) == len(duts) and int(f['HEADER_COUNT']) == len(head) - 11,
      'DB %s / %s / %s / DUT %s / 머리 %s' % (f['FILE_NAME'], f['TEST_PROGRAM'], f['TEST_TIME'], f['DIE_COUNT'], f['HEADER_COUNT']))
check('E2 Wafer: DB WAFER_ID(파일 이름 Lot-Wafer No) = 내용의 Wafer ID 열', set(content_wafers) == {f['WAFER_ID']},
      'DB %s(LOT %s, SLOT %s) / 내용 %s -> 이 파일은 내용상 다른 Wafer 의 E-Test' % (f['WAFER_ID'], f['LOT_ID'], f['SLOT_NO'], dict(content_wafers)))

# E3 Die 행
dbdie = dict((int(r['DUT_NO']), r) for r in die)
bad = Counter()
ex = {}
for r in duts:
    dut = int(str(r[0]).lstrip('#'))
    d = dbdie.get(dut)
    if d is None:
        bad['없음'] += 1
        continue
    m = re.match(r'\((-?\d+),\s*(-?\d+)\)', str(r[head.index('(X, Y)')]))
    flag = str(r[head.index('DUT Flag')])
    exp = {
        'DIE_X': m.group(1), 'DIE_Y': m.group(2),
        'HW_BIN': str(r[head.index('Hardware Bin')]).replace('Bin ', ''),
        'SW_BIN': str(r[head.index('Software Bin')]).replace('Bin ', ''),
        'PASS_FLAG': 'P' if flag.lower().startswith('pass') else 'F',
        'TEST_TIME_MS': str(r[head.index('Test Time')]).replace(' ms', ''),
        'TESTS_EXECUTED': str(r[head.index('Tests Executed')]),
        'HEAD_SITE': str(r[head.index('Test Head - Site')]),
    }
    for k, v in exp.items():
        if d[k] != v:
            bad[k] += 1
            ex.setdefault(k, (dut, v, d[k]))
check('E3 Die 73행: (X, Y) / HW Bin / SW Bin / DUT Flag(P/F) / Test 시각 / Tests Executed / Head-Site 1:1', not bad and len(dbdie) == len(duts),
      '다른 값 %s 예 %s / DB Die %d' % (dict(bad), ex, len(dbdie)))
file_pass = sum(1 for r in duts if str(r[head.index('DUT Flag')]).lower().startswith('pass'))
# 2026-10-19 검증 중 정정: 처음 기대값(파일 DUT Flag 집계)은 개념이 틀렸다. TQP_GDS_ET_FILE.PASS / FAIL_DIE_COUNT 와 [E-Test] 기준 표 "Fail Die" 는
# DMS Spec 판정(Die JUDGE_FLAG) 수다(E7 에서 다시 계산). 파일 DUT Flag 집계는 참고로만 보인다.

# E5 값: 저장 항목(85)마다 HEADER_TEXT 로 열을 찾아 73 DUT 값 비교
items = dict((r['ET_ITEM_SEQ'], r) for r in item)
stored = sorted(set(v['ET_ITEM_SEQ'] for v in val))
miss_col = []
vbad = Counter()
vex = []
by_key = dict(((int(v['DUT_NO']), v['ET_ITEM_SEQ']), v) for v in val)
header_of = {}
for v in val:
    header_of.setdefault(v['ET_ITEM_SEQ'], v['HEADER_TEXT'].strip())
for seq in stored:
    h = header_of[seq]
    c = col_of.get(h)
    if c is None:
        miss_col.append(h)
        continue
    for r in duts:
        dut = int(str(r[0]).lstrip('#'))
        v = by_key.get((dut, seq))
        cell = r[c]
        if v is None:
            vbad['DB 값 없음'] += 1
            continue
        a, b = num(cell), num(v['MEAS_VALUE'])
        if a is None and b is None:
            continue
        if a is None or b is None or abs(a - b) > 1e-9 * max(1.0, abs(a)):
            vbad[h] += 1
            if len(vex) < 4:
                vex.append((dut, h, cell, v['MEAS_VALUE'], v['VALUE_STATUS']))
check('E5 측정 값: 저장 항목 %d x DUT %d = %d 값이 원본 칸과 같음(HEADER_TEXT 로 열 찾기)' % (len(stored), len(duts), len(stored) * len(duts)),
      not miss_col and not vbad and len(val) == len(stored) * len(duts) == int(f['VALUE_COUNT']),
      '열 못 찾음 %s / 다른 값 %d 예 %s / DB 값 %d / VALUE_COUNT %s' % (miss_col[:3], sum(vbad.values()), vex, len(val), f['VALUE_COUNT']))
dup_used = [header_of[s] for s in stored if dup_header.get(header_of[s], 0) > 1]
check('E5b 저장 항목의 머리 원문이 파일 안에서 한 번만 나옴(같은 이름 머리를 잘못 고르지 않음)', not dup_used, '중복 머리 %s' % dup_used[:3])

# E6 Spec 판정 다시 계산: DB Spec 표(사용 Y) 하한 / 상한, '-' 나 빈 값은 Fail. Spec 없는 항목은 판정 없음
patterns = Counter(re.sub(r'-?\d+(\.\d+)?', 'N', l['SPEC_TEXT']) for l in lim.values())
jbad = Counter()
jex = []
die_fail = Counter()
for v in val:
    it = items.get(v['ET_ITEM_SEQ'])
    l = lim.get(v['ITEM_NAME'])
    if l is None or l['ENABLE_FLAG'] != 'Y':
        exp = ''
    else:
        x = num(v['MEAS_VALUE'])
        lo, up = num(l['LOWER_LIMIT']), num(l['UPPER_LIMIT'])
        exp = 'F' if x is None or (lo is not None and x < lo) or (up is not None and x > up) else 'P'
    if exp == 'F':
        die_fail[int(v['DUT_NO'])] += 1
    if v['JUDGE_FLAG'] != exp:
        jbad[v['ITEM_NAME']] += 1
        if len(jex) < 4:
            jex.append((v['DUT_NO'], v['ITEM_NAME'], v['MEAS_VALUE'], l and (l['LOWER_LIMIT'], l['UPPER_LIMIT']), v['JUDGE_FLAG'], exp))
check('E6 값 판정(JUDGE_FLAG) = DB Spec(하한 ≤ 값 ≤ 상한, \'-\' / 빈 값 Fail) 다시 계산 %d 값' % len(val), not jbad,
      '다른 판정 %d 예 %s / Spec 문구 모양 %s' % (sum(jbad.values()), jex, dict(patterns)))
dbad = [(int(d['DUT_NO']), d['JUDGE_FLAG'], d['FAIL_COUNT'], die_fail.get(int(d['DUT_NO']), 0)) for d in die
        if d['JUDGE_FLAG'] != ('F' if die_fail.get(int(d['DUT_NO']), 0) > 0 else 'P') or int(d['FAIL_COUNT']) != die_fail.get(int(d['DUT_NO']), 0)]
check('E7 Die 판정(JUDGE_FLAG / FAIL_COUNT) = 그 Die 의 Fail 값 수(1개 이상이면 F)', not dbad,
      '다른 Die %s / Spec Fail Die %d' % (dbad[:4], sum(1 for d in die if d['JUDGE_FLAG'] == 'F')))
spec_fail = sum(1 for n in die_fail.values() if n > 0)
check('E4 DB Pass / Fail Die 수(TQP_GDS_ET_FILE, [E-Test] 표 Fail Die) = 다시 계산한 Spec 판정 Die 수', int(f['FAIL_DIE_COUNT']) == spec_fail
      and int(f['PASS_DIE_COUNT']) == len(duts) - spec_fail,
      'DB Pass %s / Fail %s / 다시 계산 Pass %d / Fail %d (참고: 파일 DUT Flag Pass %d / Fail %d)'
      % (f['PASS_DIE_COUNT'], f['FAIL_DIE_COUNT'], len(duts) - spec_fail, spec_fail, file_pass, len(duts) - file_pass))
diff_file = sum(1 for d in die if d['JUDGE_FLAG'] != d['PASS_FLAG'])
print('참고 / DMS Spec 판정과 파일 DUT Flag 가 다른 Die %d 개 (Spec Out F %d / 파일 Fail %d)' % (diff_file, sum(1 for d in die if d['JUDGE_FLAG'] == 'F'),
      sum(1 for d in die if d['PASS_FLAG'] == 'F')))

# E8 파일 Spec(Upper / Lower 행)과 DB Spec 비교 (같은 항목)
sbad = []
for seq in stored:
    h = header_of[seq]
    c = col_of[h]
    nm = items[seq]['ITEM_NAME']
    l = lim.get(nm)
    fu, fl = num(upper[c]), num(lower[c])
    if l is None:
        continue
    if (fu is not None and num(l['UPPER_LIMIT']) is not None and abs(fu - num(l['UPPER_LIMIT'])) > 1e-9) or \
       (fl is not None and num(l['LOWER_LIMIT']) is not None and abs(fl - num(l['LOWER_LIMIT'])) > 1e-9):
        sbad.append((nm, (fl, fu), (l['LOWER_LIMIT'], l['UPPER_LIMIT'])))
print('참고 / 파일 Upper·Lower 행과 DB Spec 이 다른 항목 %d 개 예 %s' % (len(sbad), sbad[:3]))

# E9 Map 좌표: E-Test (X, Y) -> DMS Map (MAP_X, MAP_Y). 기대값 = 고객 대응표(U9_Die좌표_대응표 E-Test -> Back-side KLARF)
#    + KLARF 파일 번호 -> DMS 번호 이동(1-K K7: +7, +5). 2026-10-19 처음 버전은 회전 / 이동 4가지만 시험해 대응표의 변환을 빠뜨렸다(테스트 결함).
table = {}
with io.open(sys.argv[3], encoding='utf-8-sig') as t:
    next(t)
    for line in t:
        cells = line.rstrip('\n').split('\t')
        e = tuple(int(v) for v in re.findall(r'-?\d+', cells[0]))
        b = tuple(int(v) for v in re.findall(r'-?\d+', cells[1]))
        table[e] = b
shift_x, shift_y = int(sys.argv[4]), int(sys.argv[5])
pairs = [((int(d['DIE_X']), int(d['DIE_Y'])), (int(d['MAP_X']), int(d['MAP_Y']))) for d in die]
mbad = []
for e, m in pairs:
    b = table.get(e)
    exp = None if b is None else (b[0] + shift_x, b[1] + shift_y)
    if exp != m:
        mbad.append((e, m, exp))
plan = set((int(r['XINDEX']), int(r['YINDEX'])) for r in rows('a_DIE'))
outside = [m for _, m in pairs if m not in plan]
check('E9 E-Test 좌표 -> DMS Map 좌표 = 고객 대응표(Back-side KLARF) + KLARF->DMS 이동(%d, %d), 모두 KLARF 검사 Die 안' % (shift_x, shift_y),
      not mbad and not outside and len(set(m for _, m in pairs)) == len(pairs),
      '대응표와 다른 Die %d 예 %s / 대응표 행 %d / 검사 Die 밖 %s / 겹침 %d' % (len(mbad), mbad[:3], len(table), outside[:4], len(pairs) - len(set(m for _, m in pairs))))

print()
print('ETEST_CHECK_PROBE=%s (%d/%d)' % ('PASS' if fails == 0 else 'FAIL', checks - fails, checks))
sys.exit(0 if fails == 0 else 1)
