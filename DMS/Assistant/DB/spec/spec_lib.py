# -*- coding: utf-8 -*-
"""
테이블 명세서 공통 함수 (2026-10-15): 서식 / 셀 쓰기 / 행 높이 / 데이터 타입 표기 / CSV 읽기.
build_area_spec.py(Common / DMS / YMS)가 쓴다. build_table_spec.py(POC GDS)는 같은 서식을 자체 코드로 가지고 있다.
"""
import csv
import os
import re

from openpyxl.styles import Alignment, Border, Font, PatternFill, Side

FONT = '맑은 고딕'
_thin = Side(style='thin', color='A6A6A6')
BORDER = Border(left=_thin, right=_thin, top=_thin, bottom=_thin)
HEAD_FILL = PatternFill('solid', fgColor='1F4E79')
LABEL_FILL = PatternFill('solid', fgColor='DDEBF7')
PK_FILL = PatternFill('solid', fgColor='FFF2CC')
GROUP_FILL = PatternFill('solid', fgColor='EDEDED')
SECTION_FILL = PatternFill('solid', fgColor='BDD7EE')
WRAP = Alignment(wrap_text=True, vertical='center')
CENTER = Alignment(horizontal='center', vertical='center', wrap_text=True)
LEFT_TOP = Alignment(vertical='center')
FORMULA_PREFIX = ('=COUNTA(', '=SUM(', '=COUNTIF(', "='")   # 마지막 = 다른 시트 참조 ('시트'!A1)


def load_csv(folder, name):
    """sqlplus CSV('|' 구분, 첫 줄 머리글)를 행 목록으로 읽는다. 휴지통(BIN$) 객체는 뺀다."""
    path = os.path.join(folder, name)
    rows = []
    with open(path, encoding='utf-8') as f:
        for r in csv.reader(f, delimiter='|'):
            if not r or r[0] in ('TABLE_NAME', 'SEQUENCE_NAME', 'DEVICE_ID') or r[0].startswith('BIN$'):
                continue
            rows.append(r)
    return rows


def load_defaults(folder):
    """def.txt (테이블|컬럼|기본값) 를 읽는다. 'NULL' 기본값은 빈 값으로 본다."""
    result = {}
    with open(os.path.join(folder, 'def.txt'), encoding='utf-8') as f:
        for line in f:
            p = line.rstrip('\n').split('|', 2)
            if len(p) == 3 and not p[0].startswith('BIN$'):
                value = p[2].strip()
                result[(p[0], p[1])] = '' if value.upper() == 'NULL' else value
    return result


def data_type(t, clen, used, prec, scale):
    """DB 컬럼 형식을 명세서 표기로: VARCHAR2(30 CHAR) / VARCHAR2(30) / NUMBER(10,2) / DATE ..."""
    if t in ('VARCHAR2', 'CHAR', 'NVARCHAR2', 'NCHAR'):
        return '%s(%s%s)' % (t, clen, ' CHAR' if used == 'C' else '')
    if t == 'NUMBER':
        if not prec:
            return 'NUMBER'
        return 'NUMBER(%s)' % prec if scale in ('', '0') else 'NUMBER(%s,%s)' % (prec, scale)
    return t


def clean(s):
    return re.sub(r'\s+', ' ', s or '').strip()


def keep_text(c, value):
    """'=' 로 시작하는 설명 문자열이 수식으로 저장되지 않게 문자열로 고정한다."""
    if isinstance(value, str) and value.startswith('=') and not value.startswith(FORMULA_PREFIX):
        c.data_type = 's'


def text_lines(text, width):
    """병합 셀처럼 자동 높이가 안 되는 칸의 줄 수 추정 (한글 2칸, 그 밖 1칸, 열 너비 1 = 약 1.15칸)."""
    lines = 0
    for part in str(text or '').split('\n'):
        vis = sum(2 if ord(ch) > 0x2E80 else 1 for ch in part)
        lines += max(1, -(-vis // int(width * 1.15)))
    return lines


def fit(ws, row, *pairs):
    """pairs = (글자, 열 너비 합). 가장 많은 줄 수에 맞춰 행 높이를 정한다."""
    ws.row_dimensions[row].height = max(16, 14 * max(text_lines(t, w) for t, w in pairs) + 4)


def font(bold=False, size=10, color='000000', underline=None):
    return Font(name=FONT, size=size, bold=bold, color=color, underline=underline)


def cell(ws, ref, value, bold=False, fill=None, align=WRAP, border=True, size=10, color='000000'):
    c = ws[ref]
    c.value = value
    keep_text(c, value)
    c.font = font(bold, size, color)
    c.alignment = align
    if fill:
        c.fill = fill
    if border:
        c.border = BORDER
    return c


def merge(ws, rng, value, **kw):
    first = rng.split(':')[0]
    ws.merge_cells(rng)
    c = cell(ws, first, value, **kw)
    if kw.get('border', True):
        for row in ws[rng]:
            for x in row:
                x.border = BORDER
    return c


def header_row(ws, row, titles, start_col=1):
    for i, title in enumerate(titles):
        c = ws.cell(row=row, column=start_col + i, value=title)
        c.font = font(True, 10, 'FFFFFF')
        c.fill = HEAD_FILL
        c.alignment = CENTER
        c.border = BORDER


def write_row(ws, row, values, start_col=1, fill=None, centers=()):
    for i, v in enumerate(values):
        c = ws.cell(row=row, column=start_col + i, value=v)
        keep_text(c, v)
        c.font = font()
        c.alignment = CENTER if (start_col + i) in centers else WRAP
        c.border = BORDER
        if fill:
            c.fill = fill


def link(c, sheet):
    c.hyperlink = "#'%s'!A1" % sheet
    c.font = font(False, 10, '0563C1', 'single')


def setup_print(ws, landscape=True):
    ws.page_setup.orientation = 'landscape' if landscape else 'portrait'
    ws.page_setup.paperSize = ws.PAPERSIZE_A4
    ws.page_setup.fitToWidth = 1
    ws.page_setup.fitToHeight = 0
    ws.sheet_properties.pageSetUpPr.fitToPage = True
    ws.print_options.horizontalCentered = True
    ws.sheet_view.showGridLines = False


def title(ws, rng, text):
    merge(ws, rng, text, bold=True, size=14, align=Alignment(vertical='center'), border=False)
    ws.row_dimensions[int(re.sub(r'[A-Z]', '', rng.split(':')[0]))].height = 26
