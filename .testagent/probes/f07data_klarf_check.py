# -*- coding: utf-8 -*-
"""
F07 데이터 검증 1-K: KLARF 원본(TEST.SINF) -> DB(TQD_DEFECT / 준비 목록 / 분석 Die) 대조.
원본은 이 스크립트가 직접 파싱한다(제품 Parser 를 쓰지 않음). DB 값은 DataDump.exe 가 BSL 로 읽은 CSV.
출력: PASS / 이름 / 상세 줄 + 마지막 KLARF_CHECK_PROBE=PASS|FAIL
인자: <KLARF 파일> <db csv 폴더>
"""
import csv, io, os, sys
from collections import Counter

klarf, db = sys.argv[1], sys.argv[2]
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


# ---------- 원본 파싱 ----------
text = io.open(klarf, encoding='cp949', errors='replace').read()
tokens_lines = text.splitlines()
head = {}
tests = []          # [(test no, [(x, y)...], area)]
spec = []
defects = []
summary = []
i = 0
n = len(tokens_lines)
while i < n:
    line = tokens_lines[i].strip()
    if not line:
        i += 1
        continue
    key = line.split()[0]
    if key in ('LotID', 'WaferID', 'DeviceID', 'StepID', 'SetupID', 'ResultTimestamp', 'FileTimestamp', 'Slot', 'DiePitch', 'DieOrigin',
               'SampleCenterLocation', 'OrientationMarkLocation', 'SampleSize', 'OrientationInstructions', 'CoordinatesMirrored', 'FileVersion'):
        head[key] = line[len(key):].strip().rstrip(';').strip()
    if key == 'InspectionTest':
        tests.append([int(line.split()[1].rstrip(';')), [], None])
    if key == 'SampleTestPlan':
        cnt = int(line.split()[1].rstrip(';'))
        pts = []
        j = i + 1
        while len(pts) < cnt:
            parts = tokens_lines[j].replace(';', ' ').split()
            for k in range(0, len(parts) - 1, 2):
                pts.append((int(parts[k]), int(parts[k + 1])))
            j += 1
        tests[-1][1] = pts
        i = j
        continue
    if key == 'AreaPerTest':
        tests[-1][2] = float(line.split()[1].rstrip(';'))
    if key == 'DefectRecordSpec':
        spec = line.rstrip(';').split()[2:]
    if key == 'SummaryList':
        j = i + 1
        while True:
            parts = tokens_lines[j].replace(';', ' ').split()
            if parts:
                summary.append(parts)
            if ';' in tokens_lines[j]:
                break
            j += 1
        i = j + 1
        continue
    if line[0] in '-0123456789' and spec and not summary:
        parts = line.rstrip(';').split()
        if len(parts) >= len(spec) - 1:
            defects.append(dict(zip(spec, parts)))
    i += 1

pitch_x, pitch_y = [float(v) for v in head['DiePitch'].split()]
center_x, center_y = [float(v) for v in head['SampleCenterLocation'].split()]
print('원본: Lot %s / Wafer %s / Slot %s / Device %s / Step %s / Pitch %s x %s / Center (%s, %s) / Defect %d / Test %s / Summary %s'
      % (head['LotID'], head['WaferID'], head['Slot'], head['DeviceID'], head['StepID'], pitch_x, pitch_y, center_x, center_y,
         len(defects), [(t[0], len(t[1]), t[2]) for t in tests], summary))

# ---------- DB ----------
step = rows('k_step_info')[0]
dbd = rows('k_defect')
ready = rows('r_READY_KLARF')[0]
info = rows('a_ANL_INFO')[0]
die = rows('a_DIE')

# K1 머리 정보
raw_wafer = head['WaferID'].strip('"')
lot = head['LotID'].strip('"')
check('K1 머리: Lot / Step / Slot / 검사 시각 / 파일 시각', step['LOT_ID'] == lot and step['STEP_ID'] == head['StepID'].strip('"') and step['SLOT_ID'] == head['Slot']
      and step['RESULTTIMESTAMP'] == '2026/09/28 21:31:50' and head['ResultTimestamp'] == '09-28-26 21:31:50'
      and step['FILETIMESTAMP'] == '2026/09/29 22:30:32' and head['FileTimestamp'] == '09-29-26 22:30:32',
      'DB %s / %s / slot %s / %s / %s' % (step['LOT_ID'], step['STEP_ID'], step['SLOT_ID'], step['RESULTTIMESTAMP'], step['FILETIMESTAMP']))
check('K2 Wafer ID 규칙(LOT_CORE_SLOT): 원본 WaferID "%s" + Slot %s -> DMS "%s-%02d"' % (raw_wafer, head['Slot'], lot, int(head['Slot'])),
      step['WAFER_ID'] == '%s-%02d' % (lot, int(head['Slot'])), 'DB ' + step['WAFER_ID'])
check('K3 Product: 원본 DeviceID %s / SetupID 안 OMM_TCB -> DB PRODUCT(업로드 화면 입력 우선)' % head['DeviceID'], step['PRODUCT'] == 'OMM_TCB', 'DB ' + step['PRODUCT'])
check('K4 Pitch / Pitch 출처 = KLARF', float(info['PITCH_X']) == pitch_x and float(info['PITCH_Y']) == pitch_y and ready['PITCH_SOURCE'] == 'KLARF',
      '%s x %s' % (info['PITCH_X'], info['PITCH_Y']))

# K5 Defect 수: 목록 줄 수 / 원본 Summary / DB
ids = [int(d['DEFECTID']) for d in defects]
test_dist = Counter(d['TEST'] for d in defects)
sum_by_test = dict((int(s[0]), int(s[1])) for s in summary)
check('K5 Defect 행 수: 원본 DefectList = DB TQD_DEFECT = 분석 DEFECT_COUNT', len(defects) == len(dbd) == int(info['DEFECT_COUNT']) == int(ready['DEFECTS']),
      '원본 %d (DEFECTID %d..%d, 중복 %d) / DB %d / 분석 %s / 준비 %s / 원본 TEST 분포 %s'
      % (len(defects), min(ids), max(ids), len(ids) - len(set(ids)), len(dbd), info['DEFECT_COUNT'], ready['DEFECTS'], dict(test_dist)))
check('K5b 원본 파일 Summary NDEFECT 와 DefectList 줄 수(파일 자체 일관성)', all(sum_by_test.get(int(t), 0) == c for t, c in test_dist.items()),
      'Summary %s / DefectList TEST 분포 %s' % (sum_by_test, dict(test_dist)))

# K6 Defect 값 1:1 (DEFECTID 키)
dbmap = dict((int(r['DEFECTID']), r) for r in dbd)
cols = [('XREL', 'XREL'), ('YREL', 'YREL'), ('XSIZE', 'XSIZE'), ('YSIZE', 'YSIZE'), ('DEFECTAREA', 'DEFECTAREA'), ('DSIZE', 'DSIZE'),
        ('CLASSNUMBER', 'CLASSNUMBER'), ('TEST', 'TEST'), ('CLUSTERNUMBER', 'CLUSTERNUMBER'), ('ROUGHBINNUMBER', 'ROUGHBINNUMBER'), ('FINEBINNUMBER', 'FINEBINNUMBER')]
bad = Counter()
examples = {}
for d in defects:
    r = dbmap.get(int(d['DEFECTID']))
    if r is None:
        bad['없음'] += 1
        continue
    for fc, dc in cols:
        if abs(float(d[fc]) - float(r[dc])) > 0.0051:
            bad[fc] += 1
            examples.setdefault(fc, (d['DEFECTID'], d[fc], r[dc]))
check('K6 Defect 값 11개 열(XREL / YREL / XSIZE / YSIZE / DEFECTAREA / DSIZE / CLASS / TEST / CLUSTER / ROUGHBIN / FINEBIN) 1122건 1:1 (소수 2자리)', not bad,
      '다른 값 %s 예 %s' % (dict(bad), examples))

# K7 Die 번호 변환: 원본 (XINDEX, YINDEX) -> DB 가 한 가지 이동(또는 반전)으로 맞는지 + Wafer 좌표 X / Y
shift = Counter()
for d in defects:
    r = dbmap[int(d['DEFECTID'])]
    shift[(int(r['XINDEX']) - int(d['XINDEX']), int(r['YINDEX']) - int(d['YINDEX']))] += 1
mirror = Counter()
for d in defects:
    r = dbmap[int(d['DEFECTID'])]
    mirror[(int(r['XINDEX']) + int(d['XINDEX']), int(r['YINDEX']) - int(d['YINDEX']))] += 1
check('K7 Die 번호: 원본 -> DB = 한 가지 평행 이동(Map Setup 원점 맞춤, 왼쪽 아래 기준)', len(shift) == 1,
      '이동 %s / (반전 가정 %s)' % (dict(shift), dict(list(mirror.items())[:3])))
dx = Counter()
for d in defects:
    r = dbmap[int(d['DEFECTID'])]
    calc_x = int(d['XINDEX']) * pitch_x + float(d['XREL']) - center_x
    calc_y = int(d['YINDEX']) * pitch_y + float(d['YREL']) - center_y
    dx[(round(float(r['X']) - float(d['X']), 2), round(float(r['Y']) - float(d['Y']), 2), abs(float(r['X']) - calc_x) < 0.006 and abs(float(r['Y']) - calc_y) < 0.006)] += 1
check('K8 Wafer 좌표 X / Y: DB = 원본 Die 번호 x Pitch + XREL / YREL - 중심 (원본 X / Y 와 차이 0.01 이내)', all(k[2] and abs(k[0]) <= 0.011 and abs(k[1]) <= 0.011 for k in dx),
      '차이(DB-원본 X, Y, 재계산 일치) 분포 %s' % dict(dx.most_common(4)))

# K9 검사 Die(Test 3 계획) -> 분석 Die 표(검사 계획 Die 76) 같은 집합(K7 이동 적용)
(sx, sy), = shift.keys() if len(shift) == 1 else [(0, 0)]
plan3 = [t for t in tests if t[0] == 3][0][1]
plan_db = set((int(r['XINDEX']), int(r['YINDEX'])) for r in die if r['IN_PLAN'] == 'Y')
plan_file = set((x + sx, y + sy) for x, y in plan3)
check('K9 검사 Die: 원본 Test 3 SampleTestPlan 76 = 분석 Die 표(IN_PLAN) 76 = 준비 INSPECTED_DIE', plan_file == plan_db and len(plan_db) == int(ready['INSPECTED_DIE']) == int(info['PLAN_DIE_COUNT']),
      '원본 %d / 분석 %d / 준비 %s / 원본에만 %s / 분석에만 %s' % (len(plan_file), len(plan_db), ready['INSPECTED_DIE'], sorted(plan_file - plan_db)[:5], sorted(plan_db - plan_file)[:5]))

# K10 Die 별 Defect 수 = 원본에서 센 수
cnt_file = Counter((int(d['XINDEX']) + sx, int(d['YINDEX']) + sy) for d in defects)
diff = [(k, cnt_file.get(k, 0), int(r['DEFECT_COUNT'])) for r in die for k in [(int(r['XINDEX']), int(r['YINDEX']))] if cnt_file.get(k, 0) != int(r['DEFECT_COUNT'])]
outside = [k for k in cnt_file if k not in plan_db]
check('K10 Die 별 Defect 수(분석 DIE.DEFECT_COUNT) = 원본에서 센 수 / Defect Die 수 = 원본 Summary NDEFDIE', not diff and not outside
      and sum(1 for v in cnt_file.values() if v > 0) == int(info['DEFECT_DIE_COUNT']) == int([s for s in summary if s[0] == '3'][0][4]),
      '다른 Die %s / 계획 밖 Defect Die %s / Defect Die 원본 %d / 분석 %s / Summary %s'
      % (diff[:5], outside[:5], len(cnt_file), info['DEFECT_DIE_COUNT'], [s for s in summary if s[0] == '3'][0][4]))

# K11 / K11b TQD_INSP_INFO 검사 요약 행 (2026-10-19 기대값 변경)
#   Test 별 기대값 = 원본 DefectList 에서 그 Test 의 줄 수 / 그 Test SampleTestPlan 수 / 그 Test Defect 의 (XINDEX, YINDEX) 수.
#   DMS 는 DEFECTS 를 Summary NDEFECT 가 아니라 DefectList 로 세므로(K5b) DEFECTIVE_DIE 도 같은 기준으로 본다. Summary 값은 참고로만 출력.
#   이전 K11 은 첫 행을 Test 80 으로 보고 원본 Test 80 Summary(NDEFDIE 1)와 비교했다.
def test_values(no):
    ds = [d for d in defects if int(d['TEST']) == no]
    plan = [t for t in tests if t[0] == no][0][1]
    return len(ds), len(plan), len(set((int(d['XINDEX']), int(d['YINDEX'])) for d in ds))


test_count = Counter(int(d['TEST']) for d in defects)
main_test = sorted((-test_count.get(t[0], 0), t[0]) for t in tests)[0][1]
summ_by_test = dict((int(s[0]), s) for s in summary)
expect_main = test_values(main_test)
got_main = (int(step['DEFECTS']), int(step['INSPECTED_DIE']), int(step['DEFECTIVE_DIE']))
s_main = summ_by_test.get(main_test, ['-'] * 5)
# K11 Defect Map 정보 창 / Chart 행(GetStepInfo = GET_INSP_INFO_MULTI_SEQ, STEP 당 한 행) = 주 Test(Defect 가 가장 많은 Test, 같으면 작은 번호)
check('K11 정보 창 행(GetStepInfo) = 주 Test %d: DEFECTS / INSPECTED_DIE / DEFECTIVE_DIE = 그 Test 의 DefectList 줄 / SampleTestPlan / Defect Die' % main_test,
      int(step['TEST']) == main_test and got_main == expect_main,
      'DB TEST %s / %s / %s / %s(%s%%) vs 원본 %s (Summary NDEFECT / NDIE / NDEFDIE %s / %s / %s)'
      % (step['TEST'], got_main[0], got_main[1], got_main[2], step['DEFECTIVE_DIE_PERCENTAGES'], expect_main, s_main[1], s_main[3], s_main[4]))

# K11b Test 별 행(f07data_insp_info_dump.ps1 이 만드는 k_insp_info_tests.csv). 파일이 없으면 건너뛴다(검사 수에 넣지 않음).
if os.path.exists(os.path.join(db, 'k_insp_info_tests.csv')):
    got_tests = dict((int(r['TEST']), (int(r['DEFECTS']), int(r['INSPECTED_DIE']), int(r['DEFECTIVE_DIE']))) for r in rows('k_insp_info_tests'))
    expect_tests = dict((t[0], test_values(t[0])) for t in tests)
    check('K11b TQD_INSP_INFO Test 별 행: DEFECTS / INSPECTED_DIE / DEFECTIVE_DIE = 그 Test 의 DefectList 줄 / SampleTestPlan / Defect Die', got_tests == expect_tests,
          'DB %s vs 원본 %s (Summary NDEFDIE %s)' % (got_tests, expect_tests, dict((k, s[4]) for k, s in summ_by_test.items())))
else:
    print('SKIP / K11b TQD_INSP_INFO Test 별 행 / k_insp_info_tests.csv 없음 (f07data_insp_info_dump.ps1 로 만듦)')

print()
print('KLARF_CHECK_PROBE=%s (%d/%d)' % ('PASS' if fails == 0 else 'FAIL', checks - fails, checks))
sys.exit(0 if fails == 0 else 1)
