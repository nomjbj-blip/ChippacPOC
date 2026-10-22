# -*- coding: utf-8 -*-
"""
F07 데이터 검증 3: 연관 분석 결과(GdsKlarfAnalysis.Analyze)를 설계 개념대로 따로 다시 계산해 대조한다.
제품 C# 코드를 옮기지 않고 설계 문서(문서/설계/05_연관분석/2026-10-15_GDS_KLARF_ETest_통합분석_데이터_설계.md)의 규칙으로 새로 쓴다.
  - 5.2 gx = XREL - PitchX/2 + 보정, gy = YREL - PitchY/2 + 보정 (Pitch = KLARF 원본 DiePitch, K4 에서 DB = 파일 확인)
  - 18  반전 = (Chain 면 != 검사 면) XOR (Map 저장 기준 = BACK). 반전이면 Defect x 를 -x 로 (거리는 반전에 대해 같다)
  - 4.3 모양 모델: BOUNDARY 다각형 / PATH 선분 사각형 + 끝 연장(Type 2, 4) + 둥근 끝(Type 1) + 꺾임 Miter(2W 넘으면 삼각형)
        Gap = Defect 사각형(중심 ± 크기/2)과 도형 사이 최단 거리, 겹치면 0. 걸림 = Gap <= 허용오차(5). 탐색 거리 50
  - 6.2 (Die, Chain) E-Test 상태 F / P / N, 6.5 이름 연결 UPPER(TRIM), 7.1 4분류, 7.2 Chain 별 Die 수 / Fail률
입력(모두 이번 덤프): k_defect(KLARF 원시), g_member_shapes / g_members / g_chains(GDS), e_die / e_value / e_para(E-Test),
      a_DIE.IN_PLAN(K9 에서 원본 SampleTestPlan 과 같음을 확인한 검사 Die 집합). 비교 대상: a_*
인자: <db csv 폴더>
"""
import csv, io, math, os, struct, sys
from collections import Counter, defaultdict

db = sys.argv[1]
PITCH_X, PITCH_Y = 23832.0, 30058.0      # KLARF TEST.SINF DiePitch (2.3832e+04 3.0058e+04)
OFFSET_X = OFFSET_Y = 0.0                # 화면 보정 X / Y 기본값
TOL = 5.0                                # 화면 허용오차 기본값 (설계 5.4 A3 / B4)
SEARCH = 50.0                            # 설계 4.2 탐색 거리 = max(허용오차, 50)
LARGE = 1000.0                           # 설계 5.3 한 변 1,000 µm 이상 = 큰 Defect
CURVE = [0.0, 2.0, 5.0, 10.0, 20.0, 50.0]
EPS = 0.0015                             # DB 값은 소수 3자리
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


def fnum(s):
    return None if s == '' else float(s)


# ---------------- 기하 ----------------
def pt_seg(px, py, ax, ay, bx, by):
    dx, dy = bx - ax, by - ay
    l2 = dx * dx + dy * dy
    t = 0.0 if l2 == 0 else max(0.0, min(1.0, ((px - ax) * dx + (py - ay) * dy) / l2))
    return math.hypot(px - (ax + t * dx), py - (ay + t * dy))


def pt_rect(px, py, r):
    x0, y0, x1, y1 = r
    dx = max(x0 - px, 0.0, px - x1)
    dy = max(y0 - py, 0.0, py - y1)
    return math.hypot(dx, dy)


def seg_hits_rect(ax, ay, bx, by, r):
    """Liang-Barsky: 선분이 닫힌 사각형과 닿으면 True"""
    x0, y0, x1, y1 = r
    t0, t1 = 0.0, 1.0
    dx, dy = bx - ax, by - ay
    for p, q in ((-dx, ax - x0), (dx, x1 - ax), (-dy, ay - y0), (dy, y1 - ay)):
        if p == 0:
            if q < 0:
                return False
        else:
            t = q / p
            if p < 0:
                if t > t1:
                    return False
                t0 = max(t0, t)
            else:
                if t < t0:
                    return False
                t1 = min(t1, t)
    return t0 <= t1


def seg_rect(ax, ay, bx, by, r):
    if seg_hits_rect(ax, ay, bx, by, r):
        return 0.0
    x0, y0, x1, y1 = r
    return min(pt_rect(ax, ay, r), pt_rect(bx, by, r),
               pt_seg(x0, y0, ax, ay, bx, by), pt_seg(x1, y0, ax, ay, bx, by),
               pt_seg(x0, y1, ax, ay, bx, by), pt_seg(x1, y1, ax, ay, bx, by))


def inside(px, py, poly):
    c = False
    n = len(poly)
    j = n - 1
    for i in range(n):
        xi, yi = poly[i]
        xj, yj = poly[j]
        if (yi > py) != (yj > py) and px < (xj - xi) * (py - yi) / (yj - yi) + xi:
            c = not c
        j = i
    return c


def poly_rect(poly, r):
    if inside((r[0] + r[2]) / 2, (r[1] + r[3]) / 2, poly):
        return 0.0
    best = float('inf')
    n = len(poly)
    for i in range(n):
        ax, ay = poly[i]
        bx, by = poly[(i + 1) % n]
        d = seg_rect(ax, ay, bx, by, r)
        if d == 0.0:
            return 0.0
        best = min(best, d)
    return best


def path_model(pts, w, ptype):
    """반환: [('poly', [...]) | ('circle', (x, y, r)) | ('seg', (ax, ay, bx, by))]"""
    clean = [pts[0]]
    for p in pts[1:]:
        if p != clean[-1]:
            clean.append(p)
    h = w / 2.0
    if w == 0:
        if len(clean) == 1:
            return [('seg', (clean[0][0], clean[0][1], clean[0][0], clean[0][1]))]
        return [('seg', (a[0], a[1], b[0], b[1])) for a, b in zip(clean, clean[1:])]
    if len(clean) == 1:
        x, y = clean[0]
        if ptype == 1:
            return [('circle', (x, y, h))]
        return [('poly', [(x - h, y - h), (x + h, y - h), (x + h, y + h), (x - h, y + h)])]
    out = []
    dirs = []
    n = len(clean)
    for i in range(n - 1):
        (ax, ay), (bx, by) = clean[i], clean[i + 1]
        L = math.hypot(bx - ax, by - ay)
        dx, dy = (bx - ax) / L, (by - ay) / L
        dirs.append((dx, dy))
        nx, ny = -dy, dx
        if ptype in (2, 4) and i == 0:
            ax, ay = ax - dx * h, ay - dy * h
        if ptype in (2, 4) and i == n - 2:
            bx, by = bx + dx * h, by + dy * h
        out.append(('poly', [(ax + nx * h, ay + ny * h), (bx + nx * h, by + ny * h), (bx - nx * h, by - ny * h), (ax - nx * h, ay - ny * h)]))
    for i in range(1, n - 1):
        (d1x, d1y), (d2x, d2y) = dirs[i - 1], dirs[i]
        cross = d1x * d2y - d1y * d2x
        if abs(cross) < 1e-12 and d1x * d2x + d1y * d2y > 0:
            continue
        s = -1.0 if cross > 0 else 1.0           # 왼쪽으로 꺾이면 바깥은 오른쪽(-법선)
        o1 = (-d1y * s, d1x * s)
        o2 = (-d2y * s, d2x * s)
        px, py = clean[i]
        a = (px + o1[0] * h, py + o1[1] * h)
        b = (px + o2[0] * h, py + o2[1] * h)
        ux, uy = o1[0] + o2[0], o1[1] + o2[1]
        l2 = ux * ux + uy * uy
        if l2 < 1e-12:
            out.append(('poly', [(px, py), a, b]))
            continue
        m = (px + ux * 2 * h / l2, py + uy * 2 * h / l2)
        if math.hypot(m[0] - px, m[1] - py) > 2 * w:
            out.append(('poly', [(px, py), a, b]))
        else:
            out.append(('poly', [(px, py), a, m, b]))
    if ptype == 1:
        out.append(('circle', (clean[0][0], clean[0][1], h)))
        out.append(('circle', (clean[-1][0], clean[-1][1], h)))
    return out


def gap(model, r):
    best = float('inf')
    for kind, g in model:
        if kind == 'poly':
            d = poly_rect(g, r)
        elif kind == 'circle':
            d = max(0.0, pt_rect(g[0], g[1], r) - g[2])
        else:
            d = seg_rect(g[0], g[1], g[2], g[3], r)
        if d < best:
            best = d
            if d == 0.0:
                break
    return best


# ---------------- 입력 ----------------
info = rows('a_ANL_INFO')[0]
gmap = rows('g_map_info')[0]
ready_gds = rows('r_READY_GDS')[0]
chain_side, insp_side = ready_gds['CHAIN_SIDE'], 'BACK'      # 검사 면 기본 BS (설계 18 / A6)
mirror = (chain_side != insp_side) != (gmap['VIEW_SIDE'] == 'BACK')
check('A0 반전 규칙: Chain 면 %s / 검사 면 %s / Map 저장 %s -> 반전 %s = 분석 MIRROR / CHAIN_SIDE / STEP_SIDES' % (chain_side, insp_side, gmap['VIEW_SIDE'], mirror),
      info['MIRROR'] == ('Y' if mirror else 'N') and info['CHAIN_SIDE'] == chain_side and info['STEP_SIDES'] == insp_side
      and float(info['TOLERANCE_UM']) == TOL and float(info['HIT_GAP_UM']) == TOL and float(info['SEARCH_UM']) == SEARCH and info['AREA_MODE'] == 'SIZE_TOL',
      'MIRROR %s / TOL %s / HIT_GAP %s / SEARCH %s / %s' % (info['MIRROR'], info['TOLERANCE_UM'], info['HIT_GAP_UM'], info['SEARCH_UM'], info['AREA_MODE']))

chains = dict((c['CHAIN_SEQ'], c) for c in rows('g_chains') if c['VERSION_STATUS'] == 'READY')
member_chain = defaultdict(list)
for m in rows('g_members'):
    if m['CHAIN_SEQ'] in chains:
        member_chain[m['PLACED_ELEMENT_ID']].append(m['CHAIN_SEQ'])

CELL = 200.0
grid = defaultdict(list)
models = {}
for s in rows('g_member_shapes'):
    if s['PLACED_ELEMENT_ID'] not in member_chain or s['ELEMENT_TYPE'] == 'TEXT':
        continue
    b = bytes.fromhex(s['POINTS_BIN'])
    v = struct.unpack('<' + 'd' * (len(b) // 8), b)
    pts = list(zip(v[0::2], v[1::2]))
    if s['ELEMENT_TYPE'] == 'BOUNDARY':
        if len(pts) > 1 and pts[0] == pts[-1]:
            pts = pts[:-1]
        model = [('poly', pts)]
        margin = 0.0
    else:
        w = abs(float(s['PATH_WIDTH'] or 0))
        model = path_model(pts, w, int(s['PATH_TYPE'] or 0))
        margin = 2 * w + 1
    xs = [p[0] for p in pts]
    ys = [p[1] for p in pts]
    bb = (min(xs) - margin, min(ys) - margin, max(xs) + margin, max(ys) + margin)
    pid = s['PLACED_ELEMENT_ID']
    models[pid] = (model, bb)
    for gx in range(int(math.floor((bb[0] - SEARCH) / CELL)), int(math.floor((bb[2] + SEARCH) / CELL)) + 1):
        for gy in range(int(math.floor((bb[1] - SEARCH) / CELL)), int(math.floor((bb[3] + SEARCH) / CELL)) + 1):
            grid[(gx, gy)].append(pid)

kd = rows('k_defect')
# Defect 마다: Chain 별 최소 Gap(탐색 거리 안) / 가장 가까운 Member / 걸린 Member 수
res = {}
for d in kd:
    gx = float(d['XREL']) - PITCH_X / 2 + OFFSET_X
    gy = float(d['YREL']) - PITCH_Y / 2 + OFFSET_Y
    cx = -gx if mirror else gx
    hx, hy = float(d['XSIZE']) / 2, float(d['YSIZE']) / 2
    r = (cx - hx, gy - hy, cx + hx, gy + hy)
    cand = set()
    for ix in range(int(math.floor(r[0] / CELL)), int(math.floor(r[2] / CELL)) + 1):
        for iy in range(int(math.floor(r[1] / CELL)), int(math.floor(r[3] / CELL)) + 1):
            cand.update(grid.get((ix, iy), ()))
    per_chain = {}
    hit_members = 0
    for pid in cand:
        model, bb = models[pid]
        lb = math.hypot(max(bb[0] - r[2], 0.0, r[0] - bb[2]), max(bb[1] - r[3], 0.0, r[1] - bb[3]))
        if lb > SEARCH:
            continue
        g = gap(model, r)
        if g > SEARCH:
            continue
        if g <= TOL:
            hit_members += 1
        for cs in member_chain[pid]:
            if cs not in per_chain or g < per_chain[cs][0]:
                per_chain[cs] = (g, pid)
    res[int(d['DEFECTID'])] = dict(d=d, gx=gx, gy=gy, rect=r, chains=per_chain, hit_members=hit_members,
                                   die=(int(d['XINDEX']), int(d['YINDEX'])), large=max(float(d['XSIZE']), float(d['YSIZE'])) >= LARGE)

# ---------------- 3-1 Defect 표 ----------------
ad = dict((int(r['DEFECTID']), r) for r in rows('a_DEFECT'))
bad = Counter()
ex = {}
border = 0


def miss(key, val):
    bad[key] += 1
    ex.setdefault(key, val)


for did, x in res.items():
    a = ad.get(did)
    if a is None:
        miss('행 없음', did)
        continue
    if abs(float(a['GDS_X']) - x['gx']) > 0.006 or abs(float(a['GDS_Y']) - x['gy']) > 0.006:
        miss('GDS_X/Y', (did, a['GDS_X'], round(x['gx'], 3), a['GDS_Y'], round(x['gy'], 3)))
    if (int(a['SYS_X']), int(a['SYS_Y'])) != x['die'] or (int(a['XINDEX']), int(a['YINDEX'])) != x['die']:
        miss('Die 번호(BACK 검사 = 시스템 그대로)', (did, a['SYS_X'], a['SYS_Y'], x['die']))
    hits = sorted(cs for cs, (g, p) in x['chains'].items() if g <= TOL)
    if any(abs(g - TOL) < 0.01 for g, p in x['chains'].values()):
        border += 1
    if (a['IN_CHAIN'] == 'Y') != bool(hits):
        miss('IN_CHAIN', (did, a['IN_CHAIN'], [(chains[c]['CHAIN_CODE'], round(x['chains'][c][0], 3)) for c in x['chains']]))
    if sorted(a['CHAIN_CODES'].split(',')) != sorted(chains[c]['CHAIN_CODE'] for c in hits) and not (a['CHAIN_CODES'] == '' and not hits):
        miss('CHAIN_CODES', (did, a['CHAIN_CODES'], [chains[c]['CHAIN_CODE'] for c in hits]))
    if int(a['HIT_MEMBER_COUNT']) != x['hit_members']:
        miss('HIT_MEMBER_COUNT', (did, a['HIT_MEMBER_COUNT'], x['hit_members']))
    mg = min([g for g, p in x['chains'].values()], default=None)
    if (mg is None) != (a['GAP'] == '') or (mg is not None and abs(float(a['GAP']) - mg) > EPS):
        miss('GAP', (did, a['GAP'], mg))
    if mg is not None and a['NEAR_CHAIN_SEQ'] and abs(x['chains'].get(a['NEAR_CHAIN_SEQ'], (1e9,))[0] - mg) > EPS:
        miss('NEAR_CHAIN', (did, a['NEAR_CHAIN_CODE'], mg))
    if (a['LARGE'] == 'Y') != x['large']:
        miss('LARGE', (did, a['LARGE'], a['XSIZE'], a['YSIZE']))
n_in = sum(1 for x in res.values() if any(g <= TOL for g, p in x['chains'].values()))
check('A1 Defect %d개: Chip 좌표(gx, gy) / Die 번호 / 걸림(Gap <= 5) / 걸린 Chain / 걸린 Member 수 / 최소 Gap(50 안) / 가장 가까운 Chain / 큰 Defect' % len(res),
      not bad and len(ad) == len(res), '다른 값 %s 예 %s / 다시 계산 걸림 %d (분석 %s) / Gap 이 5 µm ± 0.01 인 경계 Defect %d'
      % (dict(bad), ex, n_in, info['IN_CHAIN_DEFECT_COUNT'], border))

# ---------------- 3-2 Defect x Chain 표 ----------------
adc = dict(((int(r['DEFECTID']), r['CHAIN_SEQ']), r) for r in rows('a_DEFECT_CHAIN'))
mine = dict(((did, cs), v) for did, x in res.items() for cs, v in x['chains'].items() if v[0] <= TOL)
bad = Counter()
ex = {}
for k in set(adc) | set(mine):
    a, m = adc.get(k), mine.get(k)
    if a is None or m is None:
        miss('한쪽에만 있음', (k, a and a['GAP'], m))
        continue
    if abs(float(a['GAP']) - m[0]) > EPS:
        miss('GAP', (k, a['GAP'], m[0]))
    # 가장 가까운 Member: 같은 Gap 의 Member 가 여럿일 수 있어 ID 대신 "그 Chain Member 이고 Gap 이 같다"로 본다
    np_ = a['NEAR_PLACED_ELEMENT_ID']
    if np_ not in models or k[1] not in member_chain[np_] or abs(gap(models[np_][0], res[k[0]]['rect']) - m[0]) > EPS:
        miss('NEAR_PLACED_ELEMENT_ID', (k, np_, m[1]))
check('A2 Defect x Chain 걸림 %d 쌍 = 분석 DEFECT_CHAIN %d 행 (쌍 / Gap / 가장 가까운 Member)' % (len(mine), len(adc)), not bad,
      '다른 값 %s 예 %s / Member ID 까지 같은 쌍 %d' % (dict(bad), ex, sum(1 for k in mine if k in adc and adc[k]['NEAR_PLACED_ELEMENT_ID'] == mine[k][1])))

# ---------------- 3-3 허용오차 표 ----------------
curve = dict((float(r['TOL_UM']), r) for r in rows('a_TOL_CURVE'))
mine_curve = []
for t in CURVE:
    hit = [x for x in res.values() if any(g <= t for g, p in x['chains'].values())]
    mine_curve.append((t, len(hit), len(set(x['die'] for x in hit))))
check('A3 허용오차 표 0 / 2 / 5 / 10 / 20 / 50 µm: Chain 영역 Defect 수 / 걸린 Die 수 (5.5)',
      sorted(curve) == CURVE and all(int(curve[t]['IN_CHAIN_DEFECT']) == n and int(curve[t]['HIT_DIE']) == dn for t, n, dn in mine_curve),
      '다시 계산 %s / 분석 %s' % ([(int(t), n, dn) for t, n, dn in mine_curve], [(r['TOL_UM'], r['IN_CHAIN_DEFECT'], r['HIT_DIE']) for r in rows('a_TOL_CURVE')]))

# ---------------- 3-4 E-Test 이름 연결 ----------------
para = [p for p in rows('e_para') if p['USE_FLAG'] != 'N' and p['GDS_CHAIN_NAME'].strip()]
by_name = defaultdict(list)
for c in chains.values():
    by_name[c['CHAIN_NAME'].strip().upper()].append(c)
keys = defaultdict(list)
for p in para:
    keys[p['GDS_CHAIN_NAME'].strip().upper()].append(p)
link = {}
for k, ps in keys.items():
    cs = by_name.get(k, [])
    link[k] = dict(status='LINKED' if len(cs) == 1 else ('DUPLICATE' if cs else 'NOT_FOUND'), chain=cs[0]['CHAIN_SEQ'] if len(cs) == 1 else '',
                   paras=ps, chain2=sum(1 for p in ps if p['CHAIN_NAME2'].strip()))
ael = dict((r['GDS_CHAIN_KEY'], r) for r in rows('a_ET_LINK'))
bad = Counter()
ex = {}
for k in set(ael) | set(link):
    a, m = ael.get(k), link.get(k)
    if a is None or m is None:
        miss('한쪽에만 있음', (k, a and a['STATUS'], m and m['status']))
        continue
    if a['STATUS'] != m['status'] or a['CHAIN_SEQ'] != m['chain'] or int(a['PARA_COUNT']) != len(m['paras']) or int(a['CHAIN2_PARA_COUNT']) != m['chain2']:
        miss('값', (k, a['STATUS'], a['CHAIN_SEQ'], a['PARA_COUNT'], a['CHAIN2_PARA_COUNT'], m['status'], m['chain'], len(m['paras']), m['chain2']))
check('A4 E-Test Chain 이름 연결: 등록 Parameter(사용 N 제외, GDS Chain 이름 있음) %d개 -> 이름 %d개 = 분석 ET_LINK %d 행 (상태 / Chain / Parameter 수 / 2 Chain 항목 수)'
      % (len(para), len(link), len(ael)), not bad, '다른 값 %s 예 %s / 상태 %s / 이름 없는 Parameter %d / 사용 N %d'
      % (dict(bad), ex, dict(Counter(m['status'] for m in link.values())), sum(1 for p in rows('e_para') if not p['GDS_CHAIN_NAME'].strip()),
         sum(1 for p in rows('e_para') if p['USE_FLAG'] == 'N')))

# ---------------- 3-5 (Die, Chain) E-Test 상태 / 4분류 ----------------
edie = rows('e_die')
et_at = dict(((int(e['MAP_X']), int(e['MAP_Y'])), e) for e in edie if e['MAP_X'] != '')
para_chain = {}
for k, m in link.items():
    if m['status'] == 'LINKED':
        for p in m['paras']:
            para_chain[p['ET_ITEM_SEQ']] = (m['chain'], p['ITEM_NAME'])
judge = defaultdict(lambda: dict(n=0, f=0, p=0, first=None))
for v in rows('e_value'):
    pc = para_chain.get(v['ET_ITEM_SEQ'])
    if pc is None:
        continue
    j = judge[(v['DUT_NO'], pc[0])]
    j['n'] += 1
    if v['JUDGE_FLAG'] == 'F':
        j['f'] += 1
        j['first'] = pc[1] if j['first'] is None else min(j['first'], pc[1])
    elif v['JUDGE_FLAG'] == 'P':
        j['p'] += 1
linked_chains = sorted(set(m['chain'] for m in link.values() if m['status'] == 'LINKED'))
plan = set((int(r['SYS_X']), int(r['SYS_Y'])) for r in rows('a_DIE') if r['IN_PLAN'] == 'Y')     # K9 확인 집합
klarf_dies = plan | set(x['die'] for x in res.values())


def et_status(die, cs):
    e = et_at.get(die)
    if e is None or cs not in linked_chains:
        return '', None
    j = judge.get((e['DUT_NO'], cs))
    if j is None or j['n'] == 0:
        return 'N', j
    return ('F' if j['f'] else ('P' if j['p'] else 'N')), j


die_hit = defaultdict(lambda: defaultdict(list))      # die -> chain -> [(gap, defect)]
die_near = defaultdict(dict)                          # die -> chain -> (min gap 50 안, defect)
for did, x in res.items():
    for cs, (g, p) in x['chains'].items():
        if g <= TOL:
            die_hit[x['die']][cs].append((g, did))
        if cs not in die_near[x['die']] or g < die_near[x['die']][cs][0]:
            die_near[x['die']][cs] = (g, did)


def class4(die, cs):
    st, j = et_status(die, cs)
    if die not in klarf_dies or die not in et_at or st not in ('P', 'F'):
        return 'NA'
    h = bool(die_hit[die].get(cs))
    return {(True, 'F'): 'BOTH', (False, 'F'): 'FAIL', (True, 'P'): 'DEFECT', (False, 'P'): 'OK'}[(h, st)]


mine_dc = {}
for die in klarf_dies | set(et_at):
    for cs in chains:
        st, j = et_status(die, cs)
        if die_hit[die].get(cs) or st in ('F', 'N'):
            mine_dc[(die, cs)] = (st, j, class4(die, cs))
adcr = dict((((int(r['SYS_X']), int(r['SYS_Y'])), r['CHAIN_SEQ']), r) for r in rows('a_DIE_CHAIN'))
bad = Counter()
ex = {}
for k in set(adcr) | set(mine_dc):
    a, m = adcr.get(k), mine_dc.get(k)
    if a is None or m is None:
        miss('한쪽에만 있음', (k, a and (a['ET_STATUS'], a['DEFECT_COUNT']), m and m[0]))
        continue
    st, j, c4 = m
    hits = sorted(die_hit[k[0]].get(k[1], []))
    if a['ET_STATUS'] != st:
        miss('ET_STATUS', (k, a['ET_STATUS'], st))
    if a['CLASS4'] != c4:
        miss('CLASS4', (k, a['CLASS4'], c4))
    if int(a['DEFECT_COUNT'] or 0) != len(hits) or sorted(int(i) for i in a['DEFECT_IDS'].split(',') if i) != sorted(i for g, i in hits):
        miss('DEFECT_COUNT/IDS', (k, a['DEFECT_COUNT'], len(hits)))
    near = hits[0] if hits else die_near[k[0]].get(k[1])
    if (near is None) != (a['MIN_GAP'] == '') or (near is not None and abs(float(a['MIN_GAP']) - near[0]) > EPS):
        miss('MIN_GAP', (k, a['MIN_GAP'], near))
    if j is not None and st:
        if int(a['ET_PARA_COUNT'] or 0) != j['n'] or int(a['ET_FAIL_COUNT'] or 0) != j['f'] or (a['ET_FIRST_FAIL_ITEM'] or None) != j['first']:
            miss('ET_PARA/FAIL/FIRST', (k, a['ET_PARA_COUNT'], a['ET_FAIL_COUNT'], a['ET_FIRST_FAIL_ITEM'], j))
check('A5 (Die, Chain) 표: 걸린 Defect 가 있거나 E-Test F / N 인 칸 %d = 분석 DIE_CHAIN %d (E-Test 상태 / 4분류 / Defect 수 · ID / 최소 Gap / Parameter · Fail 수 / 첫 Fail 항목)'
      % (len(mine_dc), len(adcr)), not bad, '다른 값 %s 예 %s / 4분류 %s / 상태 %s' % (dict(bad), ex, dict(Counter(m[2] for m in mine_dc.values())),
                                                                       dict(Counter(m[0] or '-' for m in mine_dc.values()))))

# ---------------- 3-6 Die 표 ----------------
adie = dict(((int(r['SYS_X']), int(r['SYS_Y'])), r) for r in rows('a_DIE'))
bad = Counter()
ex = {}
die_defects = defaultdict(list)
for did, x in res.items():
    die_defects[x['die']].append(x)
for die in set(adie) | klarf_dies | set(et_at):
    a = adie.get(die)
    if a is None:
        miss('행 없음', die)
        continue
    ds = die_defects.get(die, [])
    nin = sum(1 for x in ds if any(g <= TOL for g, p in x['chains'].values()))
    cnt = dict((cs, len(v)) for cs, v in die_hit[die].items() if v)
    if int(a['DEFECT_COUNT']) != len(ds) or int(a['IN_CHAIN_COUNT']) != nin or int(a['OUT_CHAIN_COUNT']) != len(ds) - nin or int(a['HIT_CHAIN_COUNT']) != len(cnt):
        miss('Defect 수', (die, a['DEFECT_COUNT'], a['IN_CHAIN_COUNT'], a['OUT_CHAIN_COUNT'], a['HIT_CHAIN_COUNT'], len(ds), nin, len(cnt)))
    top = [(t.split('(')[0], int(t.split('(')[1].rstrip(')'))) for t in a['TOP_CHAINS'].split()] if a['TOP_CHAINS'] else []
    code_cnt = dict((chains[cs]['CHAIN_CODE'], n) for cs, n in cnt.items())
    if len(top) != min(3, len(cnt)) or any(code_cnt.get(c) != n for c, n in top) or [n for c, n in top] != sorted(cnt.values(), reverse=True)[:3]:
        miss('TOP_CHAINS', (die, a['TOP_CHAINS'], sorted(code_cnt.items(), key=lambda kv: -kv[1])[:4]))
    if (a['IN_PLAN'] == 'Y') != (die in plan):
        miss('IN_PLAN', die)
    e = et_at.get(die)
    if (a['ET_STATE'] or None) != (e['JUDGE_FLAG'] if e else None) or (a['ET_FILE_FLAG'] or None) != (e['PASS_FLAG'] if e else None):
        miss('ET_STATE/ET_FILE_FLAG', (die, a['ET_STATE'], a['ET_FILE_FLAG'], e and (e['JUDGE_FLAG'], e['PASS_FLAG'])))
    nf = sum(1 for cs in chains if et_status(die, cs)[0] == 'F')
    nb = sum(1 for cs in chains if class4(die, cs) == 'BOTH')
    if int(a['ET_FAIL_CHAIN_COUNT'] or 0) != nf or int(a['BOTH_CHAIN_COUNT'] or 0) != nb:
        miss('ET_FAIL/BOTH_CHAIN_COUNT', (die, a['ET_FAIL_CHAIN_COUNT'], a['BOTH_CHAIN_COUNT'], nf, nb))
check('A6 Die 표 %d = KLARF 검사 Die %d ∪ E-Test Die %d (Defect / Chain 영역 / 밖 / 걸린 Chain 수 / 상위 Chain 3 / 검사 계획 / E-Test 판정 · 파일 Flag / Fail Chain 수 / 일치 Chain 수)'
      % (len(adie), len(klarf_dies), len(et_at)), not bad and len(adie) == len(klarf_dies | set(et_at)), '다른 값 %s 예 %s' % (dict(bad), ex))

# ---------------- 3-7 Chain 표 ----------------
achain = dict((r['CHAIN_SEQ'], r) for r in rows('a_CHAIN'))
bad = Counter()
ex = {}
for cs, c in chains.items():
    a = achain.get(cs)
    if a is None:
        miss('행 없음', c['CHAIN_CODE'])
        continue
    hit_dies = [die for die in die_hit if die_hit[die].get(cs)]
    hit_def = sum(1 for x in res.values() if x['chains'].get(cs, (1e9,))[0] <= TOL)
    st = dict((die, et_status(die, cs)[0]) for die in klarf_dies)
    judged = [die for die in klarf_dies if die in et_at and st[die] in ('P', 'F')]
    dd = [die for die in judged if die_hit[die].get(cs)]
    df = [die for die in judged if st[die] == 'F']
    db_ = [die for die in dd if st[die] == 'F']
    rate_d = round(len(db_) / len(dd), 4) if dd else None
    rate_c = round((len(df) - len(db_)) / (len(judged) - len(dd)), 4) if len(judged) > len(dd) else None
    m = link.get(c['CHAIN_NAME'].strip().upper())
    status = 'LINKED' if m and m['status'] == 'LINKED' else 'NONE'
    got = (int(a['HIT_DIE_COUNT']), int(a['HIT_DEFECT_COUNT']), a['ET_LINK_STATUS'], int(a['ET_PARA_COUNT'] or 0), int(a['DIE_JUDGED'] or 0),
           int(a['DIE_DEFECT'] or 0), int(a['DIE_FAIL'] or 0), int(a['DIE_BOTH'] or 0), fnum(a['FAIL_RATE_DEFECT']), fnum(a['FAIL_RATE_CLEAN']))
    want = (len(hit_dies), hit_def, status, len(m['paras']) if status == 'LINKED' else 0, len(judged), len(dd), len(df), len(db_), rate_d, rate_c)
    if got[:8] != want[:8] or any((g is None) != (w is None) or (g is not None and abs(g - w) > 0.00006) for g, w in zip(got[8:], want[8:])):
        miss('값', (c['CHAIN_CODE'], got, want))
    if int(a['MEMBER_COUNT']) != int(c['MEMBER_COUNT']):
        miss('MEMBER_COUNT', c['CHAIN_CODE'])
check('A7 Chain 표 %d: 걸린 Die / 걸린 Defect / E-Test 연결 · Parameter 수 / 대상 Die / Defect Die / Fail Die / 일치 Die / Fail률 2개 (7.2)' % len(chains),
      not bad and len(achain) == len(chains), '다른 값 %s 예 %s' % (dict(bad), ex))

# ---------------- 3-8 분석 요약 ----------------
want = dict(IN_CHAIN_DEFECT_COUNT=n_in, DEFECT_COUNT=len(res), DEFECT_TOTAL=len(res), PLAN_DIE_COUNT=len(plan),
            DEFECT_DIE_COUNT=len(set(x['die'] for x in res.values())), LARGE_DEFECT_COUNT=sum(1 for x in res.values() if x['large']),
            CHAIN_COUNT=len(chains), MEMBER_COUNT=sum(int(c['MEMBER_COUNT']) for c in chains.values()),
            ET_DIE_COUNT=len(et_at), ET_OUT_OF_MAP_COUNT=sum(1 for e in edie if e['MAP_X'] == ''))
diff = dict((k, (info[k], v)) for k, v in want.items() if int(info[k]) != v)
check('A8 분석 요약(ANL_INFO) 수: %s' % ', '.join('%s %s' % kv for kv in want.items()), not diff, '다른 값 %s' % diff)

print()
print('ANALYSIS_CHECK_PROBE=%s (%d/%d)' % ('PASS' if fails == 0 else 'FAIL', checks - fails, checks))
sys.exit(0 if fails == 0 else 1)
