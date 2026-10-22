# -*- coding: utf-8 -*-
"""
F07 데이터 검증 2: 화면 A 에 보이는 값(F07DisplayDump.exe 가 화면 밖 실행으로 받은 ui_*.csv) = 분석 결과(a_*.csv) + 화면 규칙.
a_* 는 f07data_analysis_check.py(A1 ~ A8) 에서 설계 규칙으로 따로 다시 계산한 값과 같음을 확인한 표다.
사용자 화면 사진의 숫자(구간 Die 수 / 탭 제목 수)도 리터럴로 대조한다.
인자: <db csv 폴더> <ui 폴더>
"""
import csv, io, math, os, sys
from collections import Counter, defaultdict

db, ui = sys.argv[1], sys.argv[2]
fails = 0
checks = 0
CLASS_TEXT = {'BOTH': '일치', 'FAIL': 'Fail만', 'DEFECT': 'Defect만', 'OK': '정상'}
LINK_TEXT = {'LINKED': '연결', 'NOT_IN_GDS': 'GDS 에 없음', 'DUPLICATE': '이름 중복', 'NONE': '-'}
PALETTE = ['#DAF7A6', '#FFC300', '#FF5733', '#C70039', '#900C3F']     # 설계: Defect Count Color 기본 5색
# 사용자 화면 사진(2026-10-19, SCKLOTIDA-01 / OMM_TCB GDS 탭) 에 보인 값
PHOTO_LEGEND = ['1 ~ 7   19', '8 ~ 14   12', '15 ~ 21   7', '22 ~ 28   3', '29 이상   2', '기준 미만   8', 'Defect 없음   25', 'Spec Out Die   25']
PHOTO_TABS = {'tabDefect': 'Defect (472)', 'tabEtFail': 'E-Test Fail (149)'}


def check(name, ok, detail=''):
    global fails, checks
    checks += 1
    if not ok:
        fails += 1
    print('%s / %s / %s' % ('PASS' if ok else 'FAIL', name, detail))


def rows(folder, name):
    with io.open(os.path.join(folder, name + '.csv'), encoding='utf-8-sig', newline='') as f:
        return list(csv.DictReader(f))


def gap_text(s):
    return s if s == '' else '%.1f' % float(s)


def rate_text(s):
    return '' if s == '' else '%d%%' % round(float(s) * 100)     # 화면은 Math.Round(짝수 반올림), Python round 도 같다


meta = defaultdict(list)
with io.open(os.path.join(ui, 'ui_meta.tsv'), encoding='utf-8-sig') as f:
    for line in f:
        p = line.rstrip('\r\n').split('\t')
        meta[p[0]].append(p[1:])
tabs = dict((p[1], p[2]) for p in meta['TAB'])
labels = dict((p[0], p[1]) for p in meta['LABEL'])
info = rows(db, 'a_ANL_INFO')[0]
die = rows(db, 'a_DIE')
chains = dict((c['CHAIN_SEQ'], c) for c in rows(db, 'a_CHAIN'))
code_name = dict((c['CHAIN_CODE'], c['CHAIN_NAME']) for c in chains.values())
dc = rows(db, 'a_DIE_CHAIN')
key = lambda r: (int(r['XINDEX']), int(r['YINDEX']))

# D1 구간: 설계 규칙(표시 기준 1, 최대 N 의 폭 = ceil(N / 5), 5구간, 마지막 = 이상)
mx = max(int(r['IN_CHAIN_COUNT']) for r in die)
width = 1 if mx <= 5 else int(math.ceil(mx / 5.0))
want_bands = [(1 + i * width, 999 if i == min(5, mx) - 1 else i * width + width, PALETTE[i]) for i in range(min(5, mx))]
got_bands = [(int(p[0]), int(p[1]), p[2].upper()) for p in meta['BAND']]
check('D1 자동 색 구간 = 1 ~ 최대 N(%d) 을 5구간(폭 %d), 마지막 "이상", 색 = 기본 5색' % (mx, width), got_bands == want_bands, '화면 %s' % got_bands)

# D2 범례 / 구간 표 Die 수 = 분석 Die 표에서 센 수, 그리고 사용자 사진과 같음
cnt = []
for f_, t_, c_ in want_bands:
    cnt.append(sum(1 for r in die if int(r['DEFECT_COUNT']) > 0 and f_ <= int(r['IN_CHAIN_COUNT']) <= t_))
below = sum(1 for r in die if int(r['DEFECT_COUNT']) > 0 and int(r['IN_CHAIN_COUNT']) < 1)
empty = sum(1 for r in die if int(r['DEFECT_COUNT']) == 0)
spec = sum(1 for r in die if r['ET_STATE'] == 'F')
want_legend = ['%d ~ %d   %d' % (f_, t_, n) if t_ != 999 else '%d 이상   %d' % (f_, n) for (f_, t_, c_), n in zip(want_bands, cnt)] \
    + ['기준 미만   %d' % below, 'Defect 없음   %d' % empty, 'Spec Out Die   %d' % spec]
legend = [p[0] for p in meta['LEGEND']]
band = rows(ui, 'ui_band')
band_cells = ['%s ~ %s   %s' % (b['FROM'], b['TO'], b['Die 수']) if b['TAG'] == '' and b['TO'] != '이상' else
              ('%s 이상   %s' % (b['FROM'], b['Die 수']) if b['TAG'] == '' else '%s   %s' % (b['FROM'], b['Die 수'])) for b in band]
check('D2 Map 범례 = 구간 표 = 분석 Die 표로 센 Die 수(구간 / 기준 미만 / Defect 없음 / Spec Out = ET_STATE F) = 사용자 사진',
      legend == want_legend == band_cells == PHOTO_LEGEND and sum(cnt) + below + empty == len(die),
      '화면 %s / 계산 %s' % (legend, want_legend))

# D3 Map Die 76: 채움색 / 숫자 / 빨간 삼각형
umap = dict(((int(r['X']), int(r['Y'])), r) for r in rows(ui, 'ui_map'))
bad = []
for r in die:
    u = umap.get(key(r))
    n, total = int(r['IN_CHAIN_COUNT']), int(r['DEFECT_COUNT'])
    color = 'WHITE' if total == 0 else ('#E0E0E0' if n < 1 else [c for f_, t_, c in want_bands if f_ <= n <= t_][0])
    if u is None or u['FILL'].upper() != color or u['LABEL'] != (str(n) if total > 0 else '') or (u['FAIL_MARK'] == 'Y') != (r['ET_STATE'] == 'F'):
        bad.append((key(r), u, color, n, r['ET_STATE']))
check('D3 Map Die %d: 채움색(Defect 없음 흰색 / Chain 밖만 연회색 / 구간 색) · 숫자 = Chain In 수(Defect Die 만) · 빨간 삼각형 = Spec Out' % len(die),
      not bad and len(umap) == len(die), '다른 Die %d %s / 화면 Die %d / 삼각형 %d' % (len(bad), bad[:3], len(umap), sum(1 for u in umap.values() if u['FAIL_MARK'] == 'Y')))

# D4 탭 제목 / 요약 / 상태줄
in_chain = sum(1 for r in rows(db, 'a_DEFECT') if r['IN_CHAIN'] == 'Y')
et_fail = sum(1 for r in dc if r['ET_STATUS'] == 'F')
check('D4 탭 제목 Defect (%d) = Chain In Defect / E-Test Fail (%d) = (Die, Chain) E-Test F = 사용자 사진' % (in_chain, et_fail),
      tabs['tabDefect'] == 'Defect (%d)' % in_chain == PHOTO_TABS['tabDefect'] and tabs['tabEtFail'] == 'E-Test Fail (%d)' % et_fail == PHOTO_TABS['tabEtFail'],
      '%s / %s' % (tabs['tabDefect'], tabs['tabEtFail']))
st = labels['lblStatus']
check('D5 지금 기준 요약 / 상태줄 = 분석 조건(Chain FS · 검사 BS -> 반전 / 크기 + 허용오차 5 / Chain 19 / Defect 1122 중 Chain In 472 / E-Test 공정)',
      labels['lblSumGds'].startswith('GDS · Chain FS · 검사 BS -> 반전') and '크기 + 허용오차 5 µm' in labels['lblSumKlarf']
      and labels['lblSumEt'].startswith('E-Test · %s' % info['ET_TEST_PROGRAM']) and 'Chain %s' % info['CHAIN_COUNT'] in st
      and 'Defect %s 중 Chain In %s' % (info['DEFECT_COUNT'], info['IN_CHAIN_DEFECT_COUNT']) in st and info['ET_TEST_PROGRAM'] in st,
      '%s | %s | %s | %s' % (labels['lblSumGds'], labels['lblSumKlarf'], labels['lblSumEt'], st))

# D6 종합 (Die) 탭 = Defect 가 있거나 E-Test F 인 Die
want = dict((key(r), r) for r in die if int(r['DEFECT_COUNT']) > 0 or r['ET_STATE'] == 'F')
got = rows(ui, 'ui_die')
bad = []
for g in got:
    r = want.get((int(g['X']), int(g['Y'])))
    if r is None or [g['Defect 전체'], g['Chain In'], g['Chain Out'], g['Chain 수'], g['ET 판정'], g['Fail Chain'], g['일치 Chain'], g['검사']] != \
            [r['DEFECT_COUNT'], r['IN_CHAIN_COUNT'], r['OUT_CHAIN_COUNT'], r['HIT_CHAIN_COUNT'], r['ET_STATE'], r['ET_FAIL_CHAIN_COUNT'], r['BOTH_CHAIN_COUNT'], r['IN_PLAN']]:
        bad.append((g['X'], g['Y']))
check('D6 종합 (Die) 탭 %d 행 = Defect 있거나 E-Test F 인 Die %d (Defect 전체 / Chain In · Out / Chain 수 / ET 판정 / Fail · 일치 Chain / 검사)' % (len(got), len(want)),
      not bad and len(got) == len(want), '다른 행 %s' % bad[:5])

# D7 Defect 탭(Wafer, Chain In) 472 행
ad = dict((r['DEFECTID'], r) for r in rows(db, 'a_DEFECT'))
got = rows(ui, 'ui_defect')
bad = []
for g in got:
    r = ad.get(g['Defect ID'])
    names = ', '.join(code_name[c] for c in r['CHAIN_CODES'].split(',')) if r and r['CHAIN_CODES'] else ''
    if r is None or r['IN_CHAIN'] != 'Y' or g['Die'] != '(%s, %s)' % (r['XINDEX'], r['YINDEX']) or g['Chain'] != names or g['Gap(µm)'] != gap_text(r['GAP']) \
            or abs(float(g['GDS X']) - float(r['GDS_X'])) > 0.006 or abs(float(g['GDS Y']) - float(r['GDS_Y'])) > 0.006 or g['RoughBin'] != r['ROUGHBINNUMBER']:
        bad.append((g['Defect ID'], g['Chain'], names, g['Gap(µm)'], r and r['GAP']))
check('D7 Defect 탭 %d 행 = Chain In Defect %d (Die / 걸린 Chain 이름 / Gap 소수 1자리 / GDS X·Y / RoughBin)' % (len(got), in_chain), not bad and len(got) == in_chain,
      '다른 행 %d %s' % (len(bad), bad[:3]))

# D8 Die 선택 시 Defect 탭 행 수 = 그 Die Chain In 수
dd = dict(((int(r['X']), int(r['Y'])), r) for r in rows(ui, 'ui_defect_die'))
bad = [(key(r), dd.get(key(r))) for r in die if dd.get(key(r)) is None or int(dd[key(r)]['DEFECT_ROWS']) != int(r['IN_CHAIN_COUNT'])
       or dd[key(r)]['DEFECT_TAB'] != 'Defect (%s)' % r['IN_CHAIN_COUNT']]
check('D8 Die %d 개를 하나씩 선택: Defect 탭(Chain In) 행 수 / 제목 = 그 Die Chain In 수' % len(die), not bad, '다른 Die %s' % bad[:3])

# D9 Die 선택 시 GDS > Chain 탭 = 그 Die 의 (Die, Chain) 행
want = defaultdict(list)
for r in dc:
    c = chains[r['CHAIN_SEQ']]
    fp = '' if r['ET_FAIL_COUNT'] == '' else '%s / %s' % (r['ET_FAIL_COUNT'], r['ET_PARA_COUNT'])
    want[key(r)].append((c['CHAIN_NAME'], r['DEFECT_COUNT'], CLASS_TEXT.get(r['CLASS4'], '비교 불가') if r['CLASS4'] else '', r['ET_STATUS'],
                         gap_text(r['MIN_GAP']), fp, r['DEFECT_IDS'], c['MEMBER_COUNT']))
got = defaultdict(list)
for g in rows(ui, 'ui_chain_die'):
    got[(int(g['X']), int(g['Y']))].append((g['Chain'], g['Defect 수'], g['분류'], g['ET'], g['가까운 거리(µm)'], g['Fail Para'], g['Defect ID'], g['Member']))
bad = [k for k in set(want) | set(got) if sorted(want.get(k, [])) != sorted(got.get(k, []))]
cls = Counter(t[2] for v in got.values() for t in v)
check('D9 Die 선택 시 GDS > Chain 탭 = 그 Die 의 (Die, Chain) %d 행 (Chain 이름 / Defect 수 / 분류 / ET / 가까운 거리 / Fail Para / Defect ID / Member)' % len(dc),
      not bad and sum(len(v) for v in got.values()) == len(dc), '다른 Die %s / 분류 %s' % (bad[:3], dict(cls)))

# D10 GDS > Chain 탭 (Wafer 전체) 19 행 = Chain 표, 정렬 = 일치 Die 많은 순 -> 걸린 Defect 많은 순
got = rows(ui, 'ui_chain_wafer')
order = sorted(chains.values(), key=lambda c: (-int(c['DIE_BOTH'] or 0), -int(c['HIT_DEFECT_COUNT'])))
bad = []
for g in got:
    c = chains[g['SEQ[숨김]']]
    w = [c['CHAIN_NAME'], c['HIT_DEFECT_COUNT'], c['HIT_DIE_COUNT'], LINK_TEXT.get(c['ET_LINK_STATUS'], c['ET_LINK_STATUS']), c['DIE_DEFECT'], c['DIE_FAIL'], c['DIE_BOTH'],
         rate_text(c['FAIL_RATE_DEFECT']), rate_text(c['FAIL_RATE_CLEAN']), c['ET_PARA_COUNT'], c['MEMBER_COUNT']]
    v = [g['Chain'], g['Defect 수'], g['Die 수'], g['E-Test 연결'], g['Defect Die'], g['Fail Die'], g['일치 Die'], g['Fail률 (Defect 있음)'], g['Fail률 (Defect 없음)'], g['Para'], g['Member']]
    if v != w:
        bad.append((v, w))
same_order = [(int(chains[g['SEQ[숨김]']]['DIE_BOTH'] or 0), int(chains[g['SEQ[숨김]']]['HIT_DEFECT_COUNT'])) for g in got] == \
             [(int(c['DIE_BOTH'] or 0), int(c['HIT_DEFECT_COUNT'])) for c in order]
check('D10 GDS > Chain 탭(Wafer 전체) %d 행 = Chain 표 19 (Defect 수 / Die 수 / E-Test 연결 / Defect · Fail · 일치 Die / Fail률 %% / Para / Member, 정렬)' % len(got),
      not bad and len(got) == len(chains) and same_order, '다른 행 %s / 정렬 %s' % (bad[:2], same_order))

# D11 E-Test Fail 탭 149 행 = E-Test F (Die, Chain), 정렬 = 일치 먼저 -> 가까운 Defect 순
got = rows(ui, 'ui_etfail')
fr = dict(((int(r['XINDEX']), int(r['YINDEX']), r['CHAIN_SEQ']), r) for r in dc if r['ET_STATUS'] == 'F')
bad = []
for g in got:
    r = fr.get((int(g['X[숨김]']), int(g['Y[숨김]']), g['SEQ[숨김]']))
    if r is None or [g['Die'], g['Chain'], g['분류'], g['가까운 Defect(µm)'], g['걸린 Defect'], g['Fail Para'], g['Fail Parameter'], g['Step']] != \
            ['(%s, %s)' % (r['XINDEX'], r['YINDEX']), chains[r['CHAIN_SEQ']]['CHAIN_NAME'], CLASS_TEXT.get(r['CLASS4'], '비교 불가'), gap_text(r['MIN_GAP']),
             r['DEFECT_COUNT'], '%s / %s' % (r['ET_FAIL_COUNT'], r['ET_PARA_COUNT']), r['ET_FIRST_FAIL_ITEM'], r['NEAR_STEP_ID']]:
        bad.append(g['Die'] + ' ' + g['Chain'])
keys_ = [(0 if g['분류'] == '일치' else 1, float(g['가까운 Defect(µm)']) if g['가까운 Defect(µm)'] else 1e18) for g in got]
check('D11 E-Test Fail 탭 %d 행 = E-Test F 인 (Die, Chain) %d (Die / Chain / 분류 / 가까운 Defect / 걸린 Defect / Fail Para / 첫 Fail 항목 / Step, 정렬 일치 -> 거리)' % (len(got), len(fr)),
      not bad and len(got) == len(fr) and keys_ == sorted(keys_), '다른 행 %s / 분류 %s' % (bad[:3], dict(Counter(g['분류'] for g in got))))

# D12 허용오차 / E-Test 연결 탭
tol = [(g['허용오차(µm)'], g['Chain In Defect'], g['걸린 Die']) for g in rows(ui, 'ui_tolerance')]
link = sorted((g['E-Test Chain 이름'], g['연결 Chain'], g['상태'], g['Parameter'], g['2 Chain 항목']) for g in rows(ui, 'ui_etlink'))
want_link = sorted((r['GDS_CHAIN_NAME'], chains[r['CHAIN_SEQ']]['CHAIN_NAME'] if r['CHAIN_SEQ'] else '', LINK_TEXT.get(r['STATUS'], r['STATUS']), r['PARA_COUNT'],
                    r['CHAIN2_PARA_COUNT']) for r in rows(db, 'a_ET_LINK'))
check('D12 허용오차 탭 = TOL_CURVE 6 행 / E-Test 연결 탭 = ET_LINK 17 행(등록 원문 이름 / 연결 Chain / 상태 / Parameter 수)',
      tol == [(r['TOL_UM'], r['IN_CHAIN_DEFECT'], r['HIT_DIE']) for r in rows(db, 'a_TOL_CURVE')] and link == want_link, '허용오차 %s / 연결 %d 행' % (tol, len(link)))

print()
print('DISPLAY_CHECK_PROBE=%s (%d/%d)' % ('PASS' if fails == 0 else 'FAIL', checks - fails, checks))
sys.exit(0 if fails == 0 else 1)
