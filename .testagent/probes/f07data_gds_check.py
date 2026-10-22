# -*- coding: utf-8 -*-
"""
F07 데이터 검증 1-G: GDS 원본 -> DB(TQP_GDS_MAP / LAYER / PLACED, Chain Member) 대조.
원본은 f07data_gds_lib 로 직접 읽는다. DB 도형 좌표(POINTS_BIN)는 이 스크립트가 직접 푼다(little-endian double x, y 쌍).
인자: <gds> <db csv 폴더>
"""
import csv, io, os, struct, sys
from collections import Counter
sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
import f07data_gds_lib as g

gds, db = sys.argv[1], sys.argv[2]
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


def points(hexs):
    b = bytes.fromhex(hexs)
    v = struct.unpack('<' + 'd' * (len(b) // 8), b)
    return list(zip(v[0::2], v[1::2]))


units, structs = g.read(gds)
um = units[1] * 1e6          # DB 단위 -> µm
els = structs['TOP']
print('원본: 단위 %s m (= %s µm) / 구조 %d / TOP 요소 %d' % (units[1], um, len(structs), len(els)))
info = rows('g_map_info')[0]
layers = dict((r['LAYER_ID'], r) for r in rows('g_layers'))

# G1 Map / Layer 건수: 원본에서 완전히 같은 도형을 하나로 친 수
check('G1 Map 정보: 파일 이름 / 최상위 구조 / 저장 기준 FRONT / READY', info['GDS_FILE_NAME'] == os.path.basename(gds) and info['TOP_STRUCTURE'] == 'TOP'
      and info['VIEW_SIDE'] == 'FRONT' and info['MAP_STATUS'] == 'READY', '%s / %s / %s' % (info['GDS_FILE_NAME'], info['TOP_STRUCTURE'], info['VIEW_SIDE']))
detail = []
ok = True
for lid, r in sorted(layers.items(), key=lambda kv: int(kv[0])):
    sel = [e for e in els if e.get('layer') == int(lid)]
    uniq = len(set((e['kind'], e.get('datatype'), e.get('width'), e.get('pathtype'), e.get('string'), e['xy']) for e in sel))
    pts = 0
    seen = set()
    for e in sel:
        k = (e['kind'], e.get('datatype'), e.get('width'), e.get('pathtype'), e.get('string'), e['xy'])
        if k in seen:
            continue
        seen.add(k)
        # BOUNDARY 끝의 닫는 점(첫 점 반복)도 DB 에 그대로 남는다(2026-10-19 확인: 처음 기대값은 닫는 점을 뺀 수라 L3 차이 = BOUNDARY 31,000개)
        pts += len(e['xy']) // 2
    ok = ok and int(r['PLACED_COUNT']) == uniq and int(r['POINT_COUNT']) == pts
    detail.append('L%s 원본 %d(같은 도형 빼면 %d, 점 %d) / DB %s(점 %s)' % (lid, len(sel), uniq, pts, r['PLACED_COUNT'], r['POINT_COUNT']))
check('G2 Layer 3 / 5 / 775 도형 수 / 점 수 = 원본(완전히 같은 도형은 한 번) ', ok, ' ; '.join(detail))
check('G2b Map 합계 = Layer 합계', int(info['PLACED_COUNT']) == sum(int(r['PLACED_COUNT']) for r in layers.values())
      and int(info['POINT_COUNT']) == sum(int(r['POINT_COUNT']) for r in layers.values()), '%s / %s' % (info['PLACED_COUNT'], info['POINT_COUNT']))

# G3 Chain Member 도형 62,609 = 원본에 같은 Layer / 종류 / 좌표의 요소가 있음
# E 번호는 파일 순서가 아니라 파서가 Layer -> 종류 -> Bounds -> 내용으로 정렬한 순번(GdsMapControl.FlattenStructure)이라
# 번호로 찾지 않고 좌표(nm 정수)로 찾는다. 처음 버전은 파일 순서 번호로 찾아 62,609개 모두 다른 요소와 비교했다(테스트 결함, 2026-10-19).
index = {}
for e in els:
    if 'xy' in e and e.get('layer') in (3, 5):
        index.setdefault((e['layer'], e['kind'], e['xy']), []).append(e)
shapes = rows('g_member_shapes')
bad = Counter()
ex = {}
for s in shapes:
    p = points(s['POINTS_BIN'])
    nm = []
    for x, y in p:
        ix, iy = round(x / um), round(y / um)
        if abs(x / um - ix) > 1e-6 or abs(y / um - iy) > 1e-6:
            bad['nm 정수 아님'] += 1
            ex.setdefault('nm 정수 아님', (s['SOURCE_ELEMENT_ID'], x, y))
        nm += [ix, iy]
    found = index.get((int(s['LAYER_ID']), s['ELEMENT_TYPE'], tuple(nm)))
    if len(p) != int(s['POINT_COUNT']):
        bad['점 수'] += 1
        ex.setdefault('점 수', (s['SOURCE_ELEMENT_ID'], len(p), s['POINT_COUNT']))
    if not found:
        bad['원본에 없음'] += 1
        ex.setdefault('원본에 없음', (s['SOURCE_ELEMENT_ID'], s['LAYER_ID'], s['ELEMENT_TYPE'], p[:2]))
        continue
    e = found[0]
    if int(s['DATA_TYPE']) != e.get('datatype', 0):
        bad['DataType'] += 1
        ex.setdefault('DataType', (s['SOURCE_ELEMENT_ID'], e.get('datatype'), s['DATA_TYPE']))
    if e['kind'] == 'PATH':
        w = e.get('width', 0) * um
        if s['PATH_WIDTH'] == '' or abs(float(s['PATH_WIDTH']) - w) > 1e-6 or int(s['PATH_TYPE'] or 0) != e.get('pathtype', 0):
            bad['PATH 폭/타입'] += 1
            ex.setdefault('PATH 폭/타입', (s['SOURCE_ELEMENT_ID'], w, e.get('pathtype'), s['PATH_WIDTH'], s['PATH_TYPE']))
    if e['kind'] == 'BOUNDARY':
        xs = [q[0] for q in p]
        ys = [q[1] for q in p]
        if max(abs(min(xs) - float(s['MIN_X'])), abs(min(ys) - float(s['MIN_Y'])), abs(max(xs) - float(s['MAX_X'])), abs(max(ys) - float(s['MAX_Y']))) > 0.0011:
            bad['Bounds'] += 1
            ex.setdefault('Bounds', (s['SOURCE_ELEMENT_ID'], min(xs), s['MIN_X'], max(ys), s['MAX_Y']))
check('G3 Chain Member 도형 %d개 = 원본 요소(같은 Layer / 종류 / nm 좌표 전체 / DataType / PATH 폭 · 타입, BOUNDARY Bounds)' % len(shapes),
      not bad, '다른 값 %s 예 %s / 종류 %s / 같은 모양 원본이 2개 이상인 Member %d'
      % (dict(bad), ex, dict(Counter(s['ELEMENT_TYPE'] for s in shapes)),
         sum(1 for s in shapes if len(index.get((int(s['LAYER_ID']), s['ELEMENT_TYPE'], tuple(round(v / um) for q in points(s['POINTS_BIN']) for v in q)), [])) > 1)))

members = rows('g_members')
chains = rows('g_chains')
cnt = Counter(m['CHAIN_SEQ'] for m in members)
anl = dict((c['CHAIN_SEQ'], c) for c in rows('a_CHAIN'))
cbad = [(c['CHAIN_NAME'], c['MEMBER_COUNT'], cnt[c['CHAIN_SEQ']], anl.get(c['CHAIN_SEQ'], {}).get('MEMBER_COUNT')) for c in chains
        if int(c['MEMBER_COUNT']) != cnt[c['CHAIN_SEQ']] or anl.get(c['CHAIN_SEQ'], {}).get('MEMBER_COUNT') != c['MEMBER_COUNT']]
shape_ids = set(s['PLACED_ELEMENT_ID'] for s in shapes)
check('G4 Chain 19개: 목록 MEMBER_COUNT = Member 행 수 = 분석 CHAIN.MEMBER_COUNT, Member 가 모두 Map 도형에 있음', not cbad and len(chains) == 19
      and all(m['PLACED_ELEMENT_ID'] in shape_ids for m in members), '다른 Chain %s / READY %d' % (cbad[:3], sum(1 for c in chains if c['VERSION_STATUS'] == 'READY')))

print()
print('GDS_CHECK_PROBE=%s (%d/%d)' % ('PASS' if fails == 0 else 'FAIL', checks - fails, checks))
sys.exit(0 if fails == 0 else 1)
