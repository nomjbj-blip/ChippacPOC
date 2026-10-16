# -*- coding: utf-8 -*-
"""
POC 화면 버튼 아이콘 목록과 도구 (2026-10-16)
기준 문서: 문서/설계/06_환경_운영/2026-10-16_POC_화면_버튼_아이콘_레이아웃_기준.md

같은 기능의 버튼은 화면이 달라도 같은 아이콘을 쓴다. 아이콘은 기존 DACrux 화면이 쓰는
99_Resource 16x16 아이콘(ICON / 16x16Icon / ByModule) 중 기존 화면에서 가장 많이 쓴 것을 골랐고,
이 폴더에 BTN_<KEY>.png(16x16) 로 복사해 둔다. 화면은 기존 방식대로 화면 .resx 에 PNG 를 넣고
Designer 에서 resources.GetObject("<버튼>.Image") 로 읽는다.

명령
  python button_icons.py build                        원본에서 BTN_<KEY>.png 와 목록 그림(BUTTON_ICON_CATALOG.png)을 다시 만든다
  python button_icons.py set <화면.resx> 버튼=KEY ...  화면 resx 에 아이콘을 넣는다(같은 이름이 있으면 바꾼다, resx 가 없으면 만든다)
  python button_icons.py audit <폴더 또는 Designer.cs> ...
                                                      Button / RadioButton(버튼 모양) 마다 아이콘 KEY / 크기 / 빠진 아이콘을 표로 출력한다
  python button_icons.py list                         KEY 목록 출력
"""
import base64
import glob
import io
import os
import re
import sys

from PIL import Image

HERE = os.path.dirname(os.path.abspath(__file__))
RES = os.path.normpath(os.path.join(HERE, '..', '..', '..'))   # DMS/99_Resource

# KEY, 기능 이름, 쓰는 버튼 Text 예, 원본(99_Resource 기준)
# 공통 기능: 화면이 달라도 이 KEY 를 쓴다. 화면 고유 기능은 아래 SCREEN 묶음.
COMMON = [
    ('SEARCH',      '조회',              '조회',                                   r'ICON\Search2.png'),
    ('FIND',        '찾기 / 검색',       '찾기, 검색, 직접 찾기, 유사 묶음 찾기',   r'16x16Icon\magnifier.png'),
    ('SAVE',        '저장',              '저장, DB 저장, Map 저장, Chain 저장',     r'ICON\save.png'),
    ('SAVE_AS',     '다른 이름으로 저장', '다른 이름으로 저장, 그림 저장',           r'16x16Icon\page_save.png'),
    ('DELETE',      '삭제 (DB / 파일)',  '삭제, 선택 삭제, 선택 파일 삭제',         r'ICON\Delete.png'),
    ('REMOVE',      '제외 (목록 / 화면)', '제거, 경로 제외, 목록에서 빼기',          r'16x16Icon\delete.png'),
    ('ADD',         '추가 / 신규',       '추가, 새 Chain, 경로 추가, 하위 추가',     r'16x16Icon\add.png'),
    ('EDIT',        '수정',              '수정, 이름 수정',                         r'16x16Icon\pencil.png'),
    ('COPY',        '복사 / 복제',       '복사, Map 복사, 복제',                    r'16x16Icon\page_copy.png'),
    ('PASTE',       '붙여넣기',          'Spec 표 붙여넣기',                        r'16x16Icon\paste_plain.png'),
    ('OPEN_FILE',   '파일 열기',         '파일 선택, 파일 열기, 그림 열기',         r'16x16Icon\folder_open.gif'),
    ('OPEN_FOLDER', '폴더 선택',         '폴더 선택',                               r'16x16Icon\folder.png'),
    ('REFRESH',     '다시 읽기 / 다시 조회', '다시 읽기, 다시 조회, Redraw',        r'16x16Icon\arrow_refresh.png'),
    ('EXCEL',       '엑셀 내보내기',     'Excel, CSV 내보내기',                     r'ByModule\DACruxSTAT\084excel.gif'),
    ('OK',          '확인 / 적용',       '확인, 선택 적용',                         r'16x16Icon\accept.png'),
    ('CANCEL',      '취소',              '취소, 선택 취소, 미리보기 취소',          r'16x16Icon\cancel.png'),
    ('CLOSE',       '닫기',              '닫기',                                    r'ICON\Close.png'),
    ('STOP',        '중지',              '중지, 저장 중지',                         r'16x16Icon\stop.png'),
    ('UNDO',        '되돌리기 / 복원',   '되돌리기, 복원, 되살리기',                r'16x16Icon\arrow_undo.png'),
    ('CLEAR',       '해제 / 지우기',     '필터 해제, 선택 해제, 전체 미체크, 지우기', r'ICON\Erase.png'),
    ('CHECK_ALL',   '전체 선택',         '전체 선택, 전체 체크',                    r'ICON\CheckBoxHS.png'),
    ('FILTER',      '조건 선택',         'RoughBin: 전체 ▼, Chain: 전체 ▼',        r'ICON\Filter.png'),
    ('FIT',         '전체 보기',         '전체 보기(화면 맞춤)',                    r'16x16Icon\arrow_out.png'),
    ('ANALYZE',     '분석',              '분석',                                    r'16x16Icon\chart_bar.png'),
    ('VALIDATE',    '검증',              '검증',                                    r'ICON\CheckGrammar.png'),
    ('MOVE_UP',     '위로',              '▲',                                       r'16x16Icon\arrow_top.png'),
    ('MOVE_DOWN',   '아래로',            '▼',                                       r'16x16Icon\arrow_down.png'),
    ('EXPAND',      '모두 펼치기',       '모두 펼치기',                             r'ICON\Expand_large.png'),
    ('COLLAPSE',    '모두 접기',         '모두 접기',                               r'ICON\Collapse_large.png'),
]
SCREEN = [
    ('MAP',         'Map 열기 / Map 메뉴', '저장 Map 열기, Map ▾',                  r'16x16Icon\map.png'),
    ('GENERATE',    '자동 생성 / 다시 그리기', '다시 그리기(Map 원형 생성)',        r'16x16Icon\wand.png'),
    ('VIEW',        '보기 모드',         '보기',                                    r'16x16Icon\eye.png'),
    ('VALUE_FROM',  '값 가져오기',       'KLARF 값으로',                            r'16x16Icon\arrow_back.png'),
    ('KEEP_INSPECTED', '검사 Die 만 남기기', '검사 Die 외 제거',                    r'16x16Icon\shape_square.png'),
    ('CUT_OUTSIDE', '바깥 잘라내기',     'Wafer 밖 제거',                           r'16x16Icon\cut.png'),
    ('CHAIN_INPUT', 'Chain 시작 단자',   'Input 지정',                              r'ICON\112_RightArrowLong_Green_16x16_72.png'),
    ('CHAIN_OUTPUT', 'Chain 끝 단자',    'Output 지정',                             r'ICON\112_RightArrowShort_Orange_16x16_72.png'),
    ('TRACE',       '경로 찾기',         '후보 경로 찾기',                          r'16x16Icon\arrow_branch.png'),
    ('EXAMPLE',     '예시 묶음',         '예시 묶음 지정',                          r'16x16Icon\shape_group.png'),
]
CATALOG = COMMON + SCREEN
KEYS = {k: (name, use, src) for k, name, use, src in CATALOG}


def icon_path(key):
    return os.path.join(HERE, 'BTN_%s.png' % key)


def load_16(path):
    """원본을 16x16 RGBA 로. 13x13 같은 작은 그림은 가운데에 놓는다."""
    im = Image.open(path).convert('RGBA')
    if im.size == (16, 16):
        return im
    if im.width <= 16 and im.height <= 16:
        canvas = Image.new('RGBA', (16, 16), (0, 0, 0, 0))
        canvas.paste(im, ((16 - im.width) // 2, (16 - im.height) // 2), im)
        return canvas
    return im.resize((16, 16), Image.LANCZOS)


def build():
    for key, _, _, src in CATALOG:
        load_16(os.path.join(RES, src)).save(icon_path(key))
    sheet(os.path.join(HERE, 'BUTTON_ICON_CATALOG.png'))
    print('아이콘 %d 개 생성: %s' % (len(CATALOG), HERE))


def sheet(out):
    from PIL import ImageDraw, ImageFont
    font = ImageFont.truetype(r'C:\Windows\Fonts\malgun.ttf', 13)
    bold = ImageFont.truetype(r'C:\Windows\Fonts\malgunbd.ttf', 14)
    W, H, cols = 330, 56, 3
    groups = [('공통 기능 (화면이 달라도 같은 아이콘)', COMMON), ('화면 고유 기능', SCREEN)]
    rows = sum((len(g) + cols - 1) // cols + 1 for _, g in groups)
    img = Image.new('RGB', (W * cols, H * rows), 'white')
    d = ImageDraw.Draw(img)
    y = 0
    for title, items in groups:
        d.text((8, y + 18), title, fill='#1F4E79', font=bold)
        y += H
        for i, (key, name, use, _) in enumerate(items):
            x, yy = (i % cols) * W, y + (i // cols) * H
            d.rectangle([x + 2, yy + 2, x + W - 3, yy + H - 3], outline='#D0D0D0')
            im = Image.open(icon_path(key))
            big = im.resize((32, 32), Image.NEAREST)
            img.paste(big, (x + 10, yy + 12), big)
            img.paste(im, (x + 50, yy + 20), im)
            d.text((x + 76, yy + 6), '%s  (%s)' % (name, key), fill='black', font=font)
            d.text((x + 76, yy + 28), use[:26], fill='#666666', font=font)
        y += ((len(items) + cols - 1) // cols) * H
    img.save(out)


RESX_HEAD = '''<?xml version="1.0" encoding="utf-8"?>
<root>
  <xsd:schema id="root" xmlns="" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata">
    <xsd:import namespace="http://www.w3.org/XML/1998/namespace" />
    <xsd:element name="root" msdata:IsDataSet="true">
      <xsd:complexType>
        <xsd:choice maxOccurs="unbounded">
          <xsd:element name="metadata">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" />
              </xsd:sequence>
              <xsd:attribute name="name" use="required" type="xsd:string" />
              <xsd:attribute name="type" type="xsd:string" />
              <xsd:attribute name="mimetype" type="xsd:string" />
              <xsd:attribute ref="xml:space" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="assembly">
            <xsd:complexType>
              <xsd:attribute name="alias" type="xsd:string" />
              <xsd:attribute name="name" type="xsd:string" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="data">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
                <xsd:element name="comment" type="xsd:string" minOccurs="0" msdata:Ordinal="2" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" use="required" msdata:Ordinal="1" />
              <xsd:attribute name="type" type="xsd:string" msdata:Ordinal="3" />
              <xsd:attribute name="mimetype" type="xsd:string" msdata:Ordinal="4" />
              <xsd:attribute ref="xml:space" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name="resheader">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
              </xsd:sequence>
              <xsd:attribute name="name" type="xsd:string" use="required" />
            </xsd:complexType>
          </xsd:element>
        </xsd:choice>
      </xsd:complexType>
    </xsd:element>
  </xsd:schema>
  <resheader name="resmimetype">
    <value>text/microsoft-resx</value>
  </resheader>
  <resheader name="version">
    <value>2.0</value>
  </resheader>
  <resheader name="reader">
    <value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <resheader name="writer">
    <value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <assembly alias="System.Drawing" name="System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" />
</root>
'''


def data_block(name, png, nl):
    b64 = base64.b64encode(png).decode('ascii')
    lines = [b64[i:i + 80] for i in range(0, len(b64), 80)]
    body = nl.join('        ' + l for l in lines)
    return ('  <data name="%s.Image" type="System.Drawing.Bitmap, System.Drawing" '
            'mimetype="application/x-microsoft.net.object.bytearray.base64">%s    <value>%s%s%s    </value>%s  </data>%s'
            % (name, nl, nl, body, nl, nl, nl))


def set_icons(resx, pairs):
    """resx 의 <버튼>.Image 를 목록 아이콘으로 넣거나 바꾼다. 원래 줄바꿈(CRLF / LF)과 BOM 을 유지한다."""
    if os.path.exists(resx):
        raw = open(resx, 'rb').read()
    else:
        raw = RESX_HEAD.replace('\n', '\r\n').encode('utf-8')
        print('새 resx: %s (csproj 에 EmbeddedResource + DependentUpon 추가 필요)' % resx)
    bom = raw.startswith(b'\xef\xbb\xbf')
    text = raw.decode('utf-8-sig')
    nl = '\r\n' if '\r\n' in text else '\n'
    if 'name="System.Drawing"' not in text:
        text = text.replace('</root>', '  <assembly alias="System.Drawing" name="System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" />' + nl + '</root>')
    for pair in pairs:
        name, key = pair.split('=', 1)
        if key not in KEYS:
            raise SystemExit('모르는 KEY: %s (list 로 확인)' % key)
        buf = io.BytesIO()
        Image.open(icon_path(key)).save(buf, 'PNG')
        block = data_block(name, buf.getvalue(), nl)
        rx = re.compile(r'  <data name="%s\.Image"[^>]*>.*?</data>\r?\n' % re.escape(name), re.S)
        if rx.search(text):
            text = rx.sub(lambda m: block, text, count=1)
        else:
            text = text.replace('</root>', block + '</root>')
    out = text.encode('utf-8')
    open(resx, 'wb').write((b'\xef\xbb\xbf' if bom else b'') + out)
    print('%s: %d 개' % (os.path.basename(resx), len(pairs)))


def _pixels(im):
    im = im.convert('RGBA')
    return im.size, im.tobytes()


def audit(paths):
    """Designer 의 Button / 버튼 모양 RadioButton / CheckBox 마다 resx 아이콘이 목록의 어느 KEY 인지 출력한다."""
    known = {}
    for key in KEYS:
        if os.path.exists(icon_path(key)):
            known.setdefault(_pixels(Image.open(icon_path(key))), []).append(key)
    files = []
    for p in paths:
        files += glob.glob(os.path.join(p, '**', '*.Designer.cs'), recursive=True) if os.path.isdir(p) else [p]
    missing = 0
    for des in sorted(files):
        text = open(des, encoding='utf-8-sig', errors='ignore').read()
        buttons = re.findall(r'this\.(\w+) = new System\.Windows\.Forms\.(Button|RadioButton|CheckBox)\(\);', text)
        buttons = [(n, t) for n, t in buttons
                   if t == 'Button' or re.search(r'this\.%s\.Appearance = System\.Windows\.Forms\.Appearance\.Button' % n, text)]
        if not buttons:
            continue
        resx = des.replace('.Designer.cs', '.resx')
        rtext = open(resx, encoding='utf-8-sig').read() if os.path.exists(resx) else ''
        print('\n## %s' % os.path.relpath(des, os.path.join(RES, '..')))
        for n, _ in buttons:
            m = re.search(r'this\.%s\.Text = "([^"]*)"' % n, text)
            s = re.search(r'this\.%s\.Size = new System\.Drawing\.Size\((\d+), (\d+)\)' % n, text)
            img = re.search(r'<data name="%s\.Image"[^>]*>\s*<value>([^<]+)</value>' % n, rtext)
            if img:
                im = Image.open(io.BytesIO(base64.b64decode(''.join(img.group(1).split()))))
                key = '/'.join(known.get(_pixels(im), ['목록 밖 %dx%d' % im.size]))
            else:
                key = '없음'
                missing += 1
            print('  %-28s %-24s %-10s %s' % (n, (m.group(1) if m else '').strip(), '%sx%s' % s.groups() if s else '', key))
    print('\n아이콘 없는 버튼: %d' % missing)


if __name__ == '__main__':
    cmd = sys.argv[1] if len(sys.argv) > 1 else ''
    if cmd == 'build':
        build()
    elif cmd == 'set' and len(sys.argv) > 3:
        set_icons(sys.argv[2], sys.argv[3:])
    elif cmd == 'audit':
        audit(sys.argv[2:])
    elif cmd == 'list':
        for k, name, use, src in CATALOG:
            print('%-15s %-22s %s' % (k, name, use))
    else:
        print(__doc__)
