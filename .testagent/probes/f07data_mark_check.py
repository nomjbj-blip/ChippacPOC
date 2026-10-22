# -*- coding: utf-8 -*-
"""
화면 A Wafer Map E-Test 표식(2026-10-20 안 1 두 모서리) 확인: F07MarkCheck.exe 가 받은 값(m_meta.tsv / m_data_die.csv) 대조.
설계 문서/설계/05_연관분석/2026-10-20_Map_ETest_표식_설계.md 5절:
  Spec Out = 결과 DIE.ET_STATE F, Fail Die = DIE.ET_FILE_FLAG F, 체크 2개(기본 둘 다 켬), 색 구간 팝업 표 끝 2행,
  상태줄 "E-Test Spec Out (…) · 결과 파일 Fail Die".
인자: <ui 폴더> [Spec Out 수 리터럴] [Fail Die 수 리터럴]  (SCKLOTIDA-01 은 25 / 73, 2026-10-19 F07DATA 검증 값)
"""
import csv, io, os, sys

ui = sys.argv[1]
lit_spec = int(sys.argv[2]) if len(sys.argv) > 2 else None
lit_file = int(sys.argv[3]) if len(sys.argv) > 3 else None
fails = 0
checks = 0


def check(name, ok, detail=''):
    global fails, checks
    checks += 1
    if not ok:
        fails += 1
    print('%s / %s / %s' % ('PASS' if ok else 'FAIL', name, detail))


meta = {}
with io.open(os.path.join(ui, 'm_meta.tsv'), encoding='utf-8-sig') as f:
    for line in f:
        p = line.rstrip('\r\n').split('\t')
        meta.setdefault(p[0], []).append(p[1:])
die = list(csv.DictReader(io.open(os.path.join(ui, 'm_data_die.csv'), encoding='utf-8-sig')))
want_spec = set('%s %s' % (r['X'], r['Y']) for r in die if r['ET_STATE'] == 'F')
want_file = set('%s %s' % (r['X'], r['Y']) for r in die if r['ET_FILE_FLAG'] == 'F')


def marks(tag, kind):
    for p in meta['MARK']:
        if p[0] == tag and p[1] == kind:
            return set(x for x in p[2].split(';') if x)
    return None


def band(tag):
    return [(p[2], p[3]) for p in meta.get('BAND', []) if p[0] == tag]


chk = dict((p[0], (p[1], p[2])) for p in meta['CHECK'])
lab = dict((p[0], p[1]) for p in meta['LABEL'])
check('M1 [E-Test] 기준 = "Map E-Test 표식" + 체크 2개 Spec Out / Fail Die (결과 파일), 기본 둘 다 켬',
      lab['lblFailMark'] == 'Map E-Test 표식' and chk['chkMarkSpec'] == ('Spec Out', 'True') and chk['chkMarkFile'] == ('Fail Die (결과 파일)', 'True'), '%s / %s' % (lab, chk))

lit = (lit_spec is None or len(want_spec) == lit_spec) and (lit_file is None or len(want_file) == lit_file)
check('M2 둘 다 켬: Spec Out 표식 Die = DIE.ET_STATE F %d / Fail Die 표식 Die = DIE.ET_FILE_FLAG F %d (리터럴 %s / %s)' % (len(want_spec), len(want_file), lit_spec, lit_file),
      marks('BOTH_ON', 'SPEC') == want_spec and marks('BOTH_ON', 'FILE') == want_file and lit,
      '화면 %d / %d' % (len(marks('BOTH_ON', 'SPEC')), len(marks('BOTH_ON', 'FILE'))))
check('M3 색 구간 팝업 표 끝 2행 = Spec Out Die n / Fail Die (결과 파일) n',
      band('BOTH_ON') == [('Spec Out Die', str(len(want_spec))), ('Fail Die (결과 파일)', str(len(want_file)))], '%s' % band('BOTH_ON'))
steps = [('SPEC_ONLY', want_spec, set(), [('Spec Out Die', str(len(want_spec)))]),
         ('NONE', set(), set(), []),
         ('FILE_ONLY', set(), want_file, [('Fail Die (결과 파일)', str(len(want_file)))]),
         ('BOTH_AGAIN', want_spec, want_file, [('Spec Out Die', str(len(want_spec))), ('Fail Die (결과 파일)', str(len(want_file)))])]
bad = [(tag, len(marks(tag, 'SPEC')), len(marks(tag, 'FILE')), band(tag)) for tag, s, fi, b in steps
       if marks(tag, 'SPEC') != s or marks(tag, 'FILE') != fi or band(tag) != b]
check('M4 체크 끄고 켜기: Fail Die 끔 -> Spec Out 만 / 둘 다 끔 -> 표식 · 행 없음 / Fail Die 만 / 다시 둘 다 (표식 Die 와 표 행이 같이 바뀜)', not bad, '다른 단계 %s' % bad)

bad = []
for x, y, spec, file, text in meta.get('STATUS', []):
    # 상태줄 = 첫 줄(분석 기준, " / E-Test <Wafer> <공정>" 포함) + " | " + 둘째 줄(Die). Die 줄만 본다
    # (처음 버전은 첫 줄까지 봐서 E-Test 없는 Die 를 FAIL 로 봤다 - 테스트 결함, 2026-10-20)
    text = text.split(' | ', 1)[1] if ' | ' in text else text
    want = ''
    if spec or file:
        want = ' / E-Test ' + ('Spec Out' if spec == 'F' else 'Spec 통과' if spec == 'P' else spec or '-')
        if file:
            want_file_text = ' · 결과 파일 ' + ('Fail Die' if file == 'F' else 'Pass' if file == 'P' else file)
        else:
            want_file_text = ''
        ok = want in text and (want_file_text == '' or want_file_text in text)
    else:
        ok = ' / E-Test ' not in text
    if not ok:
        bad.append((x, y, spec, file, text[-120:]))
check('M5 상태줄(Die 위 마우스): E-Test Spec Out / Spec 통과 (Fail Chain, 일치) · 결과 파일 Fail Die / Pass, E-Test 없는 Die 는 E-Test 문구 없음 (%d 종류)' % len(meta.get('STATUS', [])),
      not bad and len(meta.get('STATUS', [])) >= 2, '다른 Die %s / 예 %s' % (bad[:2], [p[4][-90:] for p in meta.get('STATUS', [])[:2]]))

print()
print('MARK_CHECK_PROBE=%s (%d/%d)' % ('PASS' if fails == 0 else 'FAIL', checks - fails, checks))
sys.exit(0 if fails == 0 else 1)
