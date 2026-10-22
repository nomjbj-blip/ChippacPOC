# -*- coding: utf-8 -*-
"""GDSII 파일 직접 읽기(제품 GdsReader 를 쓰지 않는 검증용). 레코드 단위로 구조 / 도형을 읽는다."""
import struct

REC = {0x00: 'HEADER', 0x01: 'BGNLIB', 0x02: 'LIBNAME', 0x03: 'UNITS', 0x04: 'ENDLIB', 0x05: 'BGNSTR', 0x06: 'STRNAME', 0x07: 'ENDSTR',
       0x08: 'BOUNDARY', 0x09: 'PATH', 0x0A: 'SREF', 0x0B: 'AREF', 0x0C: 'TEXT', 0x0D: 'LAYER', 0x0E: 'DATATYPE', 0x0F: 'WIDTH',
       0x10: 'XY', 0x11: 'ENDEL', 0x12: 'SNAME', 0x13: 'COLROW', 0x15: 'NODE', 0x16: 'TEXTTYPE', 0x17: 'PRESENTATION', 0x19: 'STRING',
       0x1A: 'STRANS', 0x1B: 'MAG', 0x1C: 'ANGLE', 0x21: 'PATHTYPE', 0x2D: 'BOX', 0x2E: 'BOXTYPE'}


def real8(b):
    if b == b'\x00' * 8:
        return 0.0
    sign = -1.0 if b[0] & 0x80 else 1.0
    exp = (b[0] & 0x7F) - 64
    mant = int.from_bytes(b[1:], 'big') / float(1 << 56)
    return sign * mant * (16.0 ** exp)


def read(path):
    """반환: (units(user, meters), {구조이름: [요소 dict]}). 요소: kind / layer / datatype / width / pathtype / xy / sname / strans / mag / angle / colrow / string"""
    data = open(path, 'rb').read()
    pos = 0
    n = len(data)
    units = None
    structs = {}
    cur = None
    el = None
    while pos + 4 <= n:
        length, rtype, dtype = struct.unpack('>HBB', data[pos:pos + 4])
        if length < 4:
            break
        body = data[pos + 4:pos + length]
        pos += length
        name = REC.get(rtype)
        if name == 'UNITS':
            units = (real8(body[0:8]), real8(body[8:16]))
        elif name == 'BGNSTR':
            cur = None
        elif name == 'STRNAME':
            cur = body.rstrip(b'\x00').decode('latin-1')
            structs[cur] = []
        elif name in ('BOUNDARY', 'PATH', 'SREF', 'AREF', 'TEXT', 'BOX', 'NODE'):
            el = {'kind': name}
        elif el is not None:
            if name == 'LAYER':
                el['layer'] = struct.unpack('>h', body)[0]
            elif name in ('DATATYPE', 'TEXTTYPE', 'BOXTYPE'):
                el['datatype'] = struct.unpack('>h', body)[0]
            elif name == 'WIDTH':
                el['width'] = struct.unpack('>i', body)[0]
            elif name == 'PATHTYPE':
                el['pathtype'] = struct.unpack('>h', body)[0]
            elif name == 'XY':
                cnt = len(body) // 8
                el['xy'] = struct.unpack('>' + 'i' * (cnt * 2), body)
            elif name == 'SNAME':
                el['sname'] = body.rstrip(b'\x00').decode('latin-1')
            elif name == 'STRANS':
                el['strans'] = struct.unpack('>H', body)[0]
            elif name == 'MAG':
                el['mag'] = real8(body)
            elif name == 'ANGLE':
                el['angle'] = real8(body)
            elif name == 'COLROW':
                el['colrow'] = struct.unpack('>hh', body)
            elif name == 'STRING':
                el['string'] = body.rstrip(b'\x00').decode('latin-1')
            elif name == 'ENDEL':
                structs[cur].append(el)
                el = None
    return units, structs
