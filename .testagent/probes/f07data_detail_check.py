# -*- coding: utf-8 -*-
"""
상세보기(화면 B) 기능 탭 배치 확인: F07DetailCheck.exe 가 받은 화면 값(b_ui_*, b_meta_*) = 그 Die 의 AnalyzeDie 결과(b_data_*) + 화면 규칙,
그리고 Wafer 분석 덤프(a_*, f07data_analysis_check.py 로 설계 규칙 재계산과 같음을 확인한 표)와 같은 Die 값인지 본다.
인자: <ui 폴더> <db csv 폴더> <DieX> <DieY>
"""
import csv, io, os, re, sys
from collections import Counter

ui, db, X, Y = sys.argv[1], sys.argv[2], sys.argv[3], sys.argv[4]
fails = 0
checks = 0
CLASS = {'BOTH': '일치', 'FAIL': 'Fail만', 'DEFECT': 'Defect만', 'OK': '정상'}
ORDER = {'BOTH': 0, 'FAIL': 1, 'DEFECT': 2, 'OK': 3, 'NA': 4}


def check(name, ok, detail=''):
    global fails, checks
    checks += 1
    if not ok:
        fails += 1
    print('%s / %s / %s' % ('PASS' if ok else 'FAIL', name, detail))


def rows(folder, name):
    with io.open(os.path.join(folder, name + '.csv'), encoding='utf-8-sig', newline='') as f:
        return list(csv.DictReader(f))


def meta(name):
    m = {}
    with io.open(os.path.join(ui, name), encoding='utf-8-sig') as f:
        for line in f:
            p = line.rstrip('\r\n').split('\t')
            m.setdefault(p[0], []).append(p[1:])
    return m


def gap(s):
    return '' if s == '' else '%.1f' % float(s)


me = meta('b_meta_die.tsv')
lab = dict((p[0], p[1]) for p in me['LABEL'])
tabs = [(p[0], p[1]) for p in me['TAB']]
info = rows(db, 'a_ANL_INFO')[0]
die = [r for r in rows(db, 'a_DIE') if (r['XINDEX'], r['YINDEX']) == (X, Y)][0]
chains = dict((c['CHAIN_SEQ'], c) for c in rows(db, 'a_CHAIN'))
adc = dict((r['CHAIN_SEQ'], r) for r in rows(db, 'a_DIE_CHAIN') if (r['XINDEX'], r['YINDEX']) == (X, Y))
bdc = dict((r['CHAIN_SEQ'], r) for r in rows(ui, 'b_data_die_chain') if (r['XINDEX'], r['YINDEX']) == (X, Y))
defects = [r for r in rows(db, 'a_DEFECT') if (r['XINDEX'], r['YINDEX']) == (X, Y)]
n_in = sum(1 for r in defects if r['IN_CHAIN'] == 'Y')
hit = sum(1 for r in adc.values() if int(r['DEFECT_COUNT'] or 0) > 0)


def class_of(seq):
    r = adc.get(seq)
    if r is not None and r['CLASS4']:
        return r['CLASS4']
    return 'OK' if die['ET_STATE'] in ('P', 'F') and chains[seq]['ET_LINK_STATUS'] == 'LINKED' else 'NA'


# B0 상세 결과(AnalyzeDie) 의 그 Die (Die, Chain) = Wafer 분석 결과(같은 조건).
# 상세 결과는 설계 21.2절대로 "탐색 거리 안 Chain 전부 + E-Test 연결 Chain" 을 보내 Wafer 결과(걸림 또는 F / N 칸만)보다 행이 많다.
# 처음 버전은 행 집합이 같다고 기대했다(테스트 결함, 2026-10-19). Wafer 행은 모두 같은 값, 더 있는 행은 Defect 0 · 정상(OK) 또는 비교 불가 · E-Test F 아님.
same = set(adc) <= set(bdc) and all((adc[k]['CLASS4'], adc[k]['DEFECT_COUNT'], adc[k]['ET_STATUS']) == (bdc[k]['CLASS4'], bdc[k]['DEFECT_COUNT'], bdc[k]['ET_STATUS']) for k in adc)
extra = [bdc[k] for k in set(bdc) - set(adc)]
extra_ok = all(int(r['DEFECT_COUNT'] or 0) == 0 and r['CLASS4'] in ('OK', 'NA') and r['ET_STATUS'] != 'F' for r in extra)
check('B0 상세 결과 (Die, Chain) %d 행 ⊇ Wafer 분석 결과 같은 Die %d 행(분류 / Defect 수 / ET 같음), 더 있는 %d 행 = Defect 0 · 정상' % (len(bdc), len(adc), len(extra)),
      same and extra_ok, '더 있는 행 분류 %s' % dict(Counter(r['CLASS4'] for r in extra)))
adc_wafer = adc
adc = bdc      # 화면 B 는 상세 결과로 그린다: 아래 표 기대값은 상세 결과 기준(B0 에서 Wafer 결과와 같은 값임을 확인)

# B1 배치 / 문구
want_tabs = [('tabGds', 'GDS'), ('tabKlarf', 'KLARF'), ('tabEt', 'E-Test'), ('tabDie', '종합 (Die)')]
die_line = 'Die (%s, %s)  Defect 전체 %d / Chain In %d / Chain Out %d / 걸린 Chain %d / E-Test %s (Fail Chain %s, 일치 %s)' % (
    X, Y, len(defects), n_in, len(defects) - n_in, hit, die['ET_STATE'], die['ET_FAIL_CHAIN_COUNT'], die['BOTH_CHAIN_COUNT'])
want = {
    'lblSumTitle': '분석 기준',
    'lblSumGds': 'GDS · Chain FS · 검사 BS -> 반전',
    'lblSumKlarf': 'KLARF · Step 1개 · 크기 + 허용오차 5 µm · 보정 (0, 0)',
    'lblSumEt': 'E-Test · ' + info['ET_TEST_PROGRAM'],
    'lblMapTitle': 'GDS 도면', 'lblMapSide': '보기 = Back side(좌우 반전)',
    'lblGdsCriteria': 'GDS 기준', 'lblKlarfCriteria': 'KLARF 기준', 'lblEtCriteria': 'E-Test 기준',
    'lblDieInfo': die_line,
    'lblGdsText': '보기 Back side(좌우 반전) · Chain 면 FS · 검사 면 BS · Chain %s (최종 저장 %s)' % (info['CHAIN_COUNT'], info['CHAIN_LAST_SAVED']),
}
diff = dict((k, (lab.get(k), v)) for k, v in want.items() if lab.get(k) != v)
check('B1 배치 / 문구: 기능 탭 GDS / KLARF / E-Test / 종합 (Die), 처음 = 종합 (Die), 분석 기준 3칸 · 기준 제목 · Die 줄 = 화면 A 문구',
      tabs == want_tabs and me['SELECTED_TAB'][0][0] == 'tabDie' and not diff, '다른 값 %s / 탭 %s / 처음 %s' % (diff, tabs, me['SELECTED_TAB']))
check('B1b [KLARF] 기준 = Step · Defect 영역 · 좌표 보정 · 분석 시각 / [E-Test] 기준 = 공정 · E-Test Wafer · 판정 시각 + 이 Die Spec 판정(Spec Out) · 결과 파일(Fail Die)',
      # 분석 시각은 상세 결과를 만든 시각(이 실행)이라 형식만 본다
      re.match(r'^Step %s · Defect 영역 크기 \+ 허용오차 5 µm · 좌표 보정 \(0, 0\) · 분석 \d{4}-\d\d-\d\d \d\d:\d\d:\d\d$' % re.escape(info['STEP_IDS']), lab['lblKlarfText']) is not None
      and lab['lblEtText'].startswith('공정 %s · E-Test Wafer %s · 판정 ' % (info['ET_TEST_PROGRAM'], info['ET_WAFER_ID']))
      and lab['lblEtText'].endswith('이 Die: Spec 판정 %s%s · 결과 파일 %s%s' % (die['ET_STATE'], ' (Spec Out)' if die['ET_STATE'] == 'F' else '', die['ET_FILE_FLAG'],
                                                                           ' (Fail Die)' if die['ET_FILE_FLAG'] == 'F' else '')),
      '%s | %s' % (lab['lblKlarfText'], lab['lblEtText']))

# B2 [GDS] Chain 표 19 행
ch = rows(ui, 'b_ui_chain')
bad = []
for g in ch:
    seq = g['ROW_TAG']
    k = class_of(seq)
    r = adc.get(seq) or {}
    et = r.get('ET_STATUS', 'P' if k == 'OK' else '')
    fp = '' if not r.get('ET_FAIL_COUNT') else '%s / %s' % (r['ET_FAIL_COUNT'], r['ET_PARA_COUNT'])
    w = [chains[seq]['CHAIN_NAME'], r.get('DEFECT_COUNT') or '0', CLASS.get(k, '비교 불가'), et, fp, chains[seq]['MEMBER_COUNT']]
    v = [g['Chain'], g['Defect 수'], g['분류'], g['ET'], g['Fail Para'], g['Member']]
    if v != w:
        bad.append((v, w))
order = [ORDER[class_of(g['ROW_TAG'])] for g in ch]
check('B2 [GDS] Chain 표 %d 행 = Chain %d (Chain 이름 / Defect 수 / 분류(정상 · 비교 불가 포함) / ET / Fail Para / Member), 정렬 일치 -> Fail만 -> Defect만 -> 정상 -> 비교 불가' % (len(ch), len(chains)),
      not bad and len(ch) == len(chains) and order == sorted(order) and lab['lblChainTitle'] == 'Chain (%d)' % len(chains),
      '다른 행 %s / 분류 %s' % (bad[:2], dict(Counter(g['분류'] for g in ch))))

# B3 [KLARF] Defect 표(Defect 전체)
de = rows(ui, 'b_ui_defect')
code_name = dict((c['CHAIN_CODE'], c['CHAIN_NAME']) for c in chains.values())
dmap = dict((r['DEFECTID'], r) for r in defects)
bad = []
for g in de:
    r = dmap.get(g['Defect ID'])
    names = ', '.join(code_name[c] for c in r['CHAIN_CODES'].split(',')) if r and r['IN_CHAIN'] == 'Y' else '(밖)'
    if r is None or g['Chain'] != names or g['Gap(µm)'] != gap(r['GAP']) or g['RoughBin'] != r['ROUGHBINNUMBER'] or g['Die'] != '(%s, %s)' % (X, Y):
        bad.append((g['Defect ID'], g['Chain'], names, g['Gap(µm)'], r and r['GAP']))
check('B3 [KLARF] Defect 표 %d 행 = 그 Die Defect %d (Defect ID / Chain 이름 / Gap / RoughBin / Die), 범위 Defect 전체, 머리 "Defect (n)  Chain In / Chain Out"' % (len(de), len(defects)),
      not bad and len(de) == len(defects) and dict(me['COMBO'])['cboScope'] == 'Defect 전체'
      and lab['lblDefectCount'] == 'Defect (%d)  Chain In %d / Chain Out %d' % (len(defects), n_in, len(defects) - n_in),
      '다른 행 %s / %s' % (bad[:2], lab['lblDefectCount']))

# B4 [E-Test] E-Test 값 표 = 그 Die DUT 의 등록 Parameter 값(원시 덤프 e_value / e_para)
edie = [e for e in rows(db, 'e_die') if (e['MAP_X'], e['MAP_Y']) == (X, Y)]
et = rows(ui, 'b_ui_et')
if edie:
    para = dict((p['ET_ITEM_SEQ'], p) for p in rows(db, 'e_para'))
    vals = dict((v['ITEM_NAME'], v) for v in rows(db, 'e_value') if v['DUT_NO'] == edie[0]['DUT_NO'])
    bad = []
    for g in et:
        v = vals.get(g['Parameter'])
        p = para.get(v['ET_ITEM_SEQ']) if v else None
        if v is None or g['판정'] != v['JUDGE_FLAG'] or g['GDS Chain'] != p['GDS_CHAIN_NAME']:
            bad.append((g['Parameter'], g['판정'], v and v['JUDGE_FLAG']))
        else:
            try:
                if abs(float(g['값']) - float(v['MEAS_VALUE'])) > 1e-6:
                    bad.append((g['Parameter'], g['값'], v['MEAS_VALUE']))
            except ValueError:
                if g['값'] != v['MEAS_VALUE']:
                    bad.append((g['Parameter'], g['값'], v['MEAS_VALUE']))
    nf = sum(1 for g in et if g['판정'] == 'F')
    first_f = [g['판정'] for g in et]
    check('B4 [E-Test] E-Test 값 %d 행 = DUT %s 등록 Parameter %d (판정 / 값 / GDS Chain), Fail 먼저, 머리 "E-Test 값 (n)  Fail m"' % (len(et), edie[0]['DUT_NO'], len(vals)),
          not bad and len(et) == len(vals) and first_f == sorted(first_f, key=lambda s: s != 'F')
          and lab['lblEtCount'] == 'E-Test 값 (%d)%s' % (len(et), '  Fail %d' % nf if nf else ''),
          '다른 행 %s / %s' % (bad[:2], lab['lblEtCount']))

# B5 [종합 (Die)]
su = rows(ui, 'b_ui_summary')
want_rows = sorted([s for s in adc if adc[s]['CLASS4'] in ('BOTH', 'FAIL', 'DEFECT')],
                   key=lambda s: (ORDER[adc[s]['CLASS4']], -int(adc[s]['DEFECT_COUNT'] or 0), float(adc[s]['MIN_GAP']) if adc[s]['MIN_GAP'] else 1e18))
got = [(g['분류'], g['Chain'], g['Defect 수'], g['가까운 거리(µm)'], g['ET'], g['Fail Para'], g['Fail Parameter']) for g in su]
exp = [(CLASS[adc[s]['CLASS4']], chains[s]['CHAIN_NAME'], adc[s]['DEFECT_COUNT'], gap(adc[s]['MIN_GAP']), adc[s]['ET_STATUS'],
        '' if not adc[s]['ET_FAIL_COUNT'] else '%s / %s' % (adc[s]['ET_FAIL_COUNT'], adc[s]['ET_PARA_COUNT']), adc[s]['ET_FIRST_FAIL_ITEM']) for s in want_rows]
cnt = Counter(class_of(s) for s in chains)
chips = [lab['lblClassBoth'], lab['lblClassFail'], lab['lblClassDefect'], lab['lblClassOk'], lab['lblClassNa']]
want_chips = ['일치 %d' % cnt['BOTH'], 'Fail만 %d' % cnt['FAIL'], 'Defect만 %d' % cnt['DEFECT'], '정상 %d' % cnt['OK'], '비교 불가 %d' % cnt['NA']]
check('B5 [종합 (Die)] 요약 · 4분류 수(%s) · 일치 / Fail만 / Defect만 Chain 표 %d 행(분류 / Chain / Defect 수 / 가까운 거리 / ET / Fail Para / Fail Parameter, 정렬)' % (', '.join(want_chips), len(exp)),
      got == exp and chips == want_chips and lab['lblDieCriteria'] == 'Die (%s, %s) 요약' % (X, Y)
      and lab['lblDieText'] == 'Defect 전체 %d / Chain In %d / Chain Out %d / 걸린 Chain %d  ·  E-Test %s (Fail Chain %s, 일치 %s)' % (
          len(defects), n_in, len(defects) - n_in, hit, die['ET_STATE'], die['ET_FAIL_CHAIN_COUNT'], die['BOTH_CHAIN_COUNT']),
      '화면 %s / %s / %s' % (chips, got[:2], lab['lblDieText']))

# B6 역추적으로 열기 = [KLARF] + 제목 (역추적), Defect = 그 Chain 까지 거리 순(상세 결과 DEFECT_CHAIN)
mf = meta('b_meta_focus.tsv')
labf = dict((p[0], p[1]) for p in mf['LABEL'])
tabsf = dict((p[0], p[1]) for p in mf['TAB'])
seq = mf['FOCUS_SEQ'][0][0]
dcf = [r for r in rows(ui, 'b_data_defect_chain') if r['CHAIN_SEQ'] == seq and r['DEFECTID'] in dmap]
gaps = sorted(float(r['GAP'] or 0) for r in dcf)
fd = rows(ui, 'b_ui_defect_focus')
check('B6 역추적(Chain %s)으로 열기: 처음 탭 [KLARF] · 탭 제목 KLARF / E-Test (역추적) · Defect = 그 Chain 까지 거리 순 %d 행 · 위쪽 역추적 Chain' % (chains[seq]['CHAIN_NAME'], len(dcf)),
      mf['SELECTED_TAB'][0][0] == 'tabKlarf' and tabsf['tabKlarf'] == 'KLARF (역추적)' and tabsf['tabEt'] == 'E-Test (역추적)'
      and [float(g['Gap(µm)']) for g in fd] == [round(v, 1) for v in gaps] and labf['lblFocus'].startswith('역추적: Chain ' + chains[seq]['CHAIN_NAME']),
      '탭 %s / %s / 행 %d / %s' % (mf['SELECTED_TAB'], tabsf, len(fd), labf['lblFocus']))

# B7 탭을 처음 열어도 표가 첫 행을 저절로 고르지 않는다(도면 확대 / 상태줄 그대로)
sw = meta('b_meta_switch.tsv')['SWITCH']
bad = [p for p in sw if p[0] in ('tabKlarf', 'tabEt', 'tabDie') and (p[1] != '0' or p[2] != p[3])]
check('B7 탭 전환(종합 -> GDS -> KLARF -> E-Test): Defect / E-Test 값 / 종합 표가 첫 행을 저절로 고르지 않음, 상태줄 그대로', not bad and len(sw) == 4,
      '%s' % [(p[0], '선택 ' + p[1], p[3]) for p in sw])

print()
print('DETAIL_CHECK_PROBE=%s (%d/%d)' % ('PASS' if fails == 0 else 'FAIL', checks - fails, checks))
sys.exit(0 if fails == 0 else 1)
