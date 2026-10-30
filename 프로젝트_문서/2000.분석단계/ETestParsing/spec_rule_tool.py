#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Test 헤더 파싱 + Spec Rule 관리 도구 (Python 3.8+, 표준 라이브러리만 사용)

  python spec_rule_tool.py parse --data data/raw_test_data.tsv
  python spec_rule_tool.py draft --data data/raw_test_data.tsv --rules spec_rules.csv
  python spec_rule_tool.py apply --data data/raw_test_data.tsv --rules spec_rules.csv

헤더 문법:  <TEST>[_<MODE>][_<COND>=<VALUE><UNIT>]@<NET>[:<NET>]
  NET   :=  <STRUCT_TYPE>_<ID>_<PIN>      PIN = FH | FL
  예) Resistance_iforce=8.0mA@TSVDC_ASIC1_P_FH   → COND_NAME=iforce, COND_VALUE=8.0, COND_UNIT=mA
      Leakage_Serial@C4DC_136_37_FL:C4DC_137_37_FH → COND_NAME=Serial (MODE도 COND_NAME으로 공통 관리)
"""
import argparse
import csv
import fnmatch
import math
import os
import re
import statistics
import sys
from collections import Counter, defaultdict

# ---------------------------------------------------------------- 설정
CHAIN_TESTS = ("Continuity", "Connectivity", "Resistance")   # 저항/전압형 (양수 → LSL 하한 0)
LEAK_TESTS = ("Leakage",)                                     # 누설형
UNIT_BY_TEST = {"Continuity": "V", "Connectivity": "V", "Resistance": "ohm", "Leakage": "TBD"}
TEST_ORDER = ["Continuity", "Connectivity", "Resistance", "Leakage"]
STRUCT_ORDER = ["C4DC", "TSVDC", "PIC"]
LEAK_SPLIT_BY_KIND = {"PIC"}     # Leakage 기본 단위를 KIND(COMB/SERP)별로 나누는 구조

PRIO_OVERRIDE, PRIO_NAMED, PRIO_DEFAULT = 10, 50, 90

PARSE_FIELDS = ["COL_NO", "RAW_HEADER", "TEST", "COND_NAME", "COND_VALUE", "COND_UNIT",
                "STRUCT_TYPE", "ITEM_ID", "PIN", "BASE", "POL", "KIND", "IDX", "SUB", "X", "Y",
                "NET_A", "NET_B", "PARSE_NOTE"]
# 앞쪽 = 보는 용도의 주요 컬럼, 뒤쪽 = 매칭·관리용 보조 컬럼 (Excel에서 숨겨도 됨)
RULE_MAIN = ["RULE_ID", "TEST", "COND_NAME", "STRUCT_TYPE", "ID_PATTERN", "ITEM_CNT",
             "MEAN", "STDEV", "MAX", "MIN", "LSL", "USL"]
RULE_AUX = ["UNIT", "COND_VALUE", "COND_UNIT", "ENABLED", "PRIORITY", "PIN", "X_MIN", "X_MAX", "Y_MIN", "Y_MAX",
            "N", "OUTLIER_CNT", "LIMIT_BASIS", "NOTE"]
RULE_FIELDS = RULE_MAIN + RULE_AUX
RULE_REQUIRED = ["RULE_ID", "TEST", "STRUCT_TYPE", "ID_PATTERN", "LSL", "USL"]   # 나머지는 없으면 빈칸(=전체)
RULE_DEFAULTS = {"ENABLED": "Y"}

NET_RE = re.compile(r"^(?P<stype>[A-Za-z0-9]+?)_(?P<body>.+?)(?:_(?P<pin>FH|FL))?$")


# ---------------------------------------------------------------- 공통 유틸
def num(v):
    try:
        x = float(v)
    except (TypeError, ValueError):
        return None
    return None if math.isnan(x) else x


def sig(x, n=3):
    if x == 0:
        return 0.0
    return round(x, -int(math.floor(math.log10(abs(x)))) + (n - 1))


def fmt(x):
    """지수 표기 없이 출력 (-6.59e-05 → -0.0000659)"""
    if x is None:
        return ""
    s = f"{x:.10f}".rstrip("0").rstrip(".")
    return "0" if s in ("", "-0") else s


def open_text(path):
    for enc in ("utf-8-sig", "cp949"):
        try:
            with open(path, encoding=enc) as f:
                f.read()
            return open(path, encoding=enc, newline="")
        except UnicodeDecodeError:
            continue
    raise SystemExit(f"인코딩을 읽을 수 없습니다: {path}")


def read_data(path):
    """첫 행 = 헤더, 첫 열 = Unit ID. '@'가 없는 헤더 열은 메타 열로 보고 건너뛴다."""
    with open_text(path) as f:
        first = f.readline()
        f.seek(0)
        delim = "\t" if "\t" in first else ","
        rows = [r for r in csv.reader(f, delimiter=delim) if any(c.strip() for c in r)]
    return rows[0], rows[1:]


def write_csv(path, fields, rows):
    os.makedirs(os.path.dirname(os.path.abspath(path)), exist_ok=True)
    try:
        with open(path, "w", encoding="utf-8-sig", newline="") as f:
            w = csv.DictWriter(f, fieldnames=fields, extrasaction="ignore")
            w.writeheader()
            w.writerows(rows)
    except PermissionError:
        raise SystemExit(f"{path} 에 쓸 수 없습니다. Excel 등에서 열려 있으면 닫고 다시 실행하세요.")


def read_rules(path):
    with open_text(path) as f:
        rules = [{k: (v or "").strip() for k, v in r.items() if k} for r in csv.DictReader(f)]
    missing = [c for c in RULE_REQUIRED if rules and c not in rules[0]]
    if missing:
        raise SystemExit(f"Rule 파일에 필수 컬럼이 없습니다: {missing}")
    for r in rules:
        for c in RULE_FIELDS:
            if not r.get(c):
                r[c] = RULE_DEFAULTS.get(c, r.get(c, ""))
    return rules


# ---------------------------------------------------------------- 1. 헤더 파싱
def parse_net(net):
    m = NET_RE.match(net)
    d = {"stype": "", "id": net, "pin": "", "base": "", "pol": "", "kind": "", "idx": "", "sub": "",
         "x": "", "y": "", "note": ""}
    if not m:
        d["note"] = "NET 형식 불일치"
        return d
    st, body = m["stype"], m["body"]
    d.update(stype=st, id=body, pin=m["pin"] or "", base=body)
    if not d["pin"]:
        d["note"] = "PIN(FH/FL) 없음"
    xy = re.fullmatch(r"(\d+)_(\d+)", body)
    if xy:                                   # 좌표형 구조 (C4DC_210_3)
        d.update(x=int(xy[1]), y=int(xy[2]), base="")
        return d
    tok = body.split("_")                    # 이름형 구조 (TSVDC_ASIC1_P, PIC_SERP1_M3)
    if len(tok) > 1 and tok[-1] in ("P", "N"):
        d["pol"], tok = tok[-1], tok[:-1]
        d["base"] = "_".join(tok)
    km = re.fullmatch(r"([A-Za-z]+)(\d*)", tok[0])
    if km:
        d["kind"], d["idx"] = km[1], km[2]
    d["sub"] = "_".join(tok[1:])
    return d


def parse_header(raw, col_no=""):
    r = dict.fromkeys(PARSE_FIELDS, "")
    r["COL_NO"], r["RAW_HEADER"] = col_no, raw
    if "@" not in raw:
        r["PARSE_NOTE"] = "'@' 없음"
        return r
    head, target = raw.split("@", 1)
    parts = head.split("_")
    r["TEST"] = parts[0]
    modes = [p for p in parts[1:] if "=" not in p]           # Leakage_Parallel → Parallel
    names, values, units = [], [], []
    for c in (p for p in parts[1:] if "=" in p):
        k, v = c.split("=", 1)                               # iforce=8.0mA → iforce | 8.0mA
        mv = re.fullmatch(r"([-+]?[\d.]+)\s*(\S*)", v)       # 8.0mA → 8.0 | mA
        names.append(k)
        values.append(mv[1] if mv else v)
        units.append(mv[2] if mv else "")
    r["COND_NAME"] = ";".join(["_".join(modes)] * bool(modes) + names)
    r["COND_VALUE"], r["COND_UNIT"] = ";".join(values), ";".join(units)

    nets = target.split(":")
    a = parse_net(nets[0])
    notes = [a["note"]] if a["note"] else []
    if len(names) + bool(modes) > 1:
        notes.append("조건 2개 이상(';'로 연결)")
    r.update(STRUCT_TYPE=a["stype"], ITEM_ID=a["id"], PIN=a["pin"], BASE=a["base"], POL=a["pol"],
             KIND=a["kind"], IDX=a["idx"], SUB=a["sub"], X=a["x"], Y=a["y"], NET_A=nets[0])
    if len(nets) == 2:                       # Serial: 두 NET 쌍
        b = parse_net(nets[1])
        r["ITEM_ID"] = f"{a['id']}:{b['id']}"
        r["PIN"] = f"{a['pin']}:{b['pin']}"
        r["NET_B"] = nets[1]
        if b["stype"] != a["stype"]:
            r["STRUCT_TYPE"] = f"{a['stype']}/{b['stype']}"
        if b["note"]:
            notes.append("B:" + b["note"])
    elif len(nets) > 2:
        notes.append("NET 3개 이상")
    r["PARSE_NOTE"] = "; ".join(notes)
    return r


def parse_all(hdr):
    return [parse_header(h, i) for i, h in enumerate(hdr) if "@" in h]


# ---------------------------------------------------------------- 2. Rule 매칭
def _any(p):
    return p.strip() in ("", "*")


def _glob(val, pat):
    return _any(pat) or any(fnmatch.fnmatchcase(str(val), q.strip()) for q in pat.split("|"))


def _value_eq(val, pat):
    """숫자끼리는 값으로 비교(8 = 8.0, Excel 저장 시 8.0→8 변환 대비), 그 외는 와일드카드"""
    if _any(pat):
        return True
    for q in pat.split("|"):
        a, b = num(val), num(q)
        if (a is not None and b is not None and a == b) or fnmatch.fnmatchcase(str(val), q.strip()):
            return True
    return False


def rule_matches(rule, it):
    for rf, f in (("TEST", "TEST"), ("COND_NAME", "COND_NAME"), ("COND_UNIT", "COND_UNIT"),
                  ("STRUCT_TYPE", "STRUCT_TYPE"), ("ID_PATTERN", "ITEM_ID"), ("PIN", "PIN")):
        if not _glob(it[f], rule[rf]):
            return False
    if not _value_eq(it["COND_VALUE"], rule["COND_VALUE"]):
        return False
    for lo, hi, f in (("X_MIN", "X_MAX", "X"), ("Y_MIN", "Y_MAX", "Y")):
        lo_v, hi_v = num(rule[lo]), num(rule[hi])
        if lo_v is None and hi_v is None:
            continue
        v = num(it[f])
        if v is None or (lo_v is not None and v < lo_v) or (hi_v is not None and v > hi_v):
            return False
    return True


def resolve(items, rules):
    """ENABLED=Y Rule 중 조건이 모두 맞는 것 → PRIORITY 숫자가 가장 작은 1개. 동순위 복수 매칭은 오류."""
    def prio(r):
        p = num(r["PRIORITY"])
        return 999 if p is None else p

    active = [r for r in rules if r["ENABLED"].upper() in ("Y", "YES", "1", "TRUE")]
    for it in items:
        hits = sorted((r for r in active if rule_matches(r, it)), key=prio)
        it["ALSO_MATCHED"] = ""
        if not hits:
            it["MATCH_STATUS"], it["RULE"] = "UNMATCHED", None
            continue
        top = [r for r in hits if prio(r) == prio(hits[0])]
        if len(top) > 1:
            it["MATCH_STATUS"], it["RULE"] = "AMBIGUOUS", None
            it["ALSO_MATCHED"] = "|".join(r["RULE_ID"] for r in top)
            continue
        it["MATCH_STATUS"], it["RULE"] = "OK", top[0]
        it["ALSO_MATCHED"] = "|".join(r["RULE_ID"] for r in hits[1:])
    return active


def judge(x, lsl, usl):
    if x is None:
        return "NO_DATA"
    if lsl is not None and x < lsl:
        return "FAIL_LOW"
    if usl is not None and x > usl:
        return "FAIL_HIGH"
    return "PASS"


# ---------------------------------------------------------------- 3. Rule 초안 생성
def _prefix(t, cond_name):
    if t == "Leakage":
        return "LK" + (cond_name[:1] or "X")                  # Parallel → LKP, Serial → LKS
    return {"Continuity": "CNT", "Connectivity": "CON", "Resistance": "RES"}.get(t, t[:3].upper())


def robust_mean_sd(v, fence_k):
    """Q1/Q3 ∓ fence_k·IQR 밖(Clamp값 747, -951 등)을 뺀 평균·σ → (평균, σ, 제외 건수)"""
    q1, _, q3 = statistics.quantiles(v, n=4) if len(v) >= 4 else (min(v), 0, max(v))
    iqr = q3 - q1
    keep = [x for x in v if q1 - fence_k * iqr <= x <= q3 + fence_k * iqr] or v
    sd = statistics.stdev(keep) if len(keep) > 1 else 0.0
    return statistics.mean(keep), sd, len(v) - len(keep)


def sigma_spec(t, v, k_sigma, fence_k):
    """평균 ± k·σ (이상치 제외 후). 저항/전압형은 LSL 하한 0. MAX/MIN은 전체 데이터 기준."""
    mu, sd, ex = robust_mean_sd(v, fence_k)
    d = 2 - int(math.floor(math.log10(sd))) if sd > 0 else 4     # σ 유효숫자 3자리 수준으로 반올림
    lsl = mu - k_sigma * sd
    clipped = t in CHAIN_TESTS and lsl < 0
    return dict(MEAN=fmt(round(mu, d)), STDEV=fmt(sig(sd)), MAX=fmt(max(v)), MIN=fmt(min(v)),
                LSL=fmt(0.0 if clipped else round(lsl, d)), USL=fmt(round(mu + k_sigma * sd, d)),
                N=len(v), OUTLIER_CNT=ex,
                LIMIT_BASIS=(f"DRAFT 평균±{k_sigma:g}σ (이상치 {ex}건 제외: Q1/Q3∓{fence_k:g}·IQR 밖, n={len(v)})"
                             + (", LSL 음수→0" if clipped else "")))


def _family(item_id):
    """구조 번호만 '?'로: ASIC1_P → ASIC?_P, SERP1_M3 → SERP?_M3 (Layer 번호는 유지). 번호 없으면 None"""
    m = re.fullmatch(r"([A-Za-z]+)(\d+)(.*)", item_id)
    return m[1] + "?" * len(m[2]) + m[3] if m else None


def compress_ids(ids, universe):
    """ID 목록 → ID_PATTERN. 번호만 다른 ID가 같은 구조 안에서 전부 포함되면 '?'로 줄인다 (ASIC1~4_P → ASIC?_P)."""
    fams = defaultdict(set)
    for i in ids:
        fams[_family(i) or i].add(i)
    parts = []
    for f, mem in fams.items():
        if len(mem) > 1 and {u for u in universe if fnmatch.fnmatchcase(u, f)} == mem:
            parts.append(f)
        else:
            parts.extend(mem)
    return "|".join(sorted(parts))


def merge_adjacent(cells, cost):
    """값 순으로 정렬된 cells에서 합쳐도 되는 인접 쌍(cost → (점수, 허용))을 점수가 낮은 것부터 반복해서 합친다."""
    cl = [[c] for c in cells]
    while len(cl) > 1:
        cand = [(sc, i) for i in range(len(cl) - 1) for sc, ok in [cost(cl[i] + cl[i + 1])] if ok]
        if not cand:
            break
        _, i = min(cand)
        cl[i:i + 2] = [cl[i] + cl[i + 1]]
    return cl


def build_draft(items, data, k_sigma=3.0, fence_k=3.0, merge_pct=10.0, leak_sd_ratio=1.5, leak_mean_sigma=0.5):
    vals = {it["COL_NO"]: [x for x in (num(r[it["COL_NO"]]) for r in data) if x is not None] for it in items}

    # (1) 기본 단위(cell): 좌표형 → 구조 전체 1개 / 이름형 → ID별 / Leakage → 구조(PIC은 KIND)별
    cells = defaultdict(list)
    for it in items:
        t, st = it["TEST"], it["STRUCT_TYPE"]
        if t in CHAIN_TESTS:
            pat = "*" if it["X"] != "" else it["ITEM_ID"]
        elif t in LEAK_TESTS:
            pat = it["KIND"] + "*" if st in LEAK_SPLIT_BY_KIND and it["KIND"] else "*"
        else:
            pat = "*"
        cells[((t, (it["COND_NAME"], it["COND_VALUE"], it["COND_UNIT"]), st), pat)].append(it)

    def cell_vals(cs):
        return [x for c in cs for it in cells[c] for x in vals[it["COL_NO"]]]

    stat = {}
    for c in cells:
        v = cell_vals([c])
        if v:
            mu, sd, _ = robust_mean_sd(v, fence_k)
            stat[c] = dict(med=statistics.median(v), mu=mu, sd=sd)

    # (2) 같은 TEST·조건·구조 안에서 값 범위가 비슷한 ID_PATTERN끼리 통합
    #     저항/전압형: ID별 중앙값 편차 (최대-최소)/중앙 ≤ merge_pct
    #     Leakage   : σ 비율 ≤ leak_sd_ratio 이고 평균 차이 ≤ leak_mean_sigma·σ
    def chain_cost(cs):
        meds = [stat[c]["med"] for c in cs]
        center = statistics.median(meds)
        sp = (max(meds) - min(meds)) / center * 100 if center > 0 else math.inf
        return sp, merge_pct > 0 and sp <= merge_pct

    def leak_cost(cs):
        sds, mus = [stat[c]["sd"] for c in cs], [stat[c]["mu"] for c in cs]
        ratio = max(sds) / min(sds) if min(sds) > 0 else math.inf
        dm = (max(mus) - min(mus)) / max(sds) if max(sds) > 0 else math.inf
        return dm + ratio - 1, ratio <= leak_sd_ratio and dm <= leak_mean_sigma

    def describe(cs):
        if cs[0][0][0] in LEAK_TESTS:
            sds, mus = [stat[c]["sd"] for c in cs], [stat[c]["mu"] for c in cs]
            return f"σ 비율 {max(sds) / min(sds):.2f}, 평균 차이 {(max(mus) - min(mus)) / max(sds):.2f}σ"
        return f"중앙값 편차 {chain_cost(cs)[0]:.1f}%"

    by_part = defaultdict(list)
    for c in cells:
        by_part[c[0]].append(c)
    groups = []                                   # (part, pattern, cells, note, 정렬값)
    for part, cs in by_part.items():
        t = part[0]
        fixed = [c for c in cs if c[1] == "*" or c not in stat]
        groups += [(part, c[1], [c], "", -math.inf) for c in fixed]
        key = "mu" if t in LEAK_TESTS else "med"
        named = sorted((c for c in cs if c not in fixed), key=lambda c: stat[c][key])
        if not named:
            continue
        if t in CHAIN_TESTS:
            clusters, rule_txt = merge_adjacent(named, chain_cost), f"기준 {merge_pct:g}%"
        elif t in LEAK_TESTS:
            clusters = merge_adjacent(named, leak_cost)
            rule_txt = f"기준 σ 비율 ≤ {leak_sd_ratio:g}, 평균 차이 ≤ {leak_mean_sigma:g}σ"
        else:
            clusters, rule_txt = [[c] for c in named], ""
        universe = {it["ITEM_ID"] for c in cs for it in cells[c]}
        pats = ["|".join(sorted(c[1] for c in cl)) if t in LEAK_TESTS else compress_ids([c[1] for c in cl], universe)
                for cl in clusters]
        for k, cl in enumerate(clusters):
            if len(cl) > 1:
                note = f"값이 비슷한 {len(cl)}개 ID_PATTERN 통합: {', '.join(c[1] for c in cl)} – {describe(cl)} ({rule_txt} 충족)"
            else:
                cost = leak_cost if t in LEAK_TESTS else chain_cost
                nb = [(cost(cl + clusters[j])[0], j) for j in (k - 1, k + 1) if 0 <= j < len(clusters)]
                if nb and t in CHAIN_TESTS + LEAK_TESTS:
                    j = min(nb)[1]
                    note = f"가장 가까운 그룹({pats[j]})과 합치면 {describe(cl + clusters[j])} ({rule_txt} 초과) → 개별 Spec"
                else:
                    note = ""
            groups.append((part, pats[k], cl, note, statistics.median(stat[c][key] for c in cl)))

    def order(g):
        (t, cond, st), _, _, _, sv = g
        return (TEST_ORDER.index(t) if t in TEST_ORDER else 99, cond,
                STRUCT_ORDER.index(st) if st in STRUCT_ORDER else 99, st, sv)

    rules, seq = [], Counter()

    def add(part, pat, cnt, prio, enabled, spec, note):
        t, cond, st = part
        p = _prefix(t, cond[0])
        seq[p] += 1
        rules.append(dict(RULE_ID=f"{p}-{seq[p]:03d}", TEST=t, COND_NAME=cond[0], STRUCT_TYPE=st, ID_PATTERN=pat,
                          ITEM_CNT=cnt, UNIT=UNIT_BY_TEST.get(t, "TBD"), COND_VALUE=cond[1], COND_UNIT=cond[2],
                          ENABLED=enabled, PRIORITY=prio, PIN="*", NOTE=note, **spec))

    unit_note = "단위·인가전압이 헤더에 없음 → Spec은 데이터와 같은 단위"
    overrides = []
    for part, pat, cl, note, _ in sorted(groups, key=order):
        t = part[0]
        members = [it for c in cl for it in cells[c]]
        v = cell_vals(cl)
        if not v:
            add(part, pat, len(members), PRIO_DEFAULT, "Y", {"LIMIT_BASIS": "데이터 없음"}, "Spec 미정")
            continue
        spec = sigma_spec(t, v, k_sigma, fence_k)
        if t in CHAIN_TESTS and pat == "*":
            note = note or f"좌표형 구조 공통 Spec ({len(members)}개 좌표)"
        if t in LEAK_TESTS:
            note = f"{note}; {unit_note}" if note else unit_note
        add(part, pat, len(members), PRIO_DEFAULT if pat == "*" or t in LEAK_TESTS else PRIO_NAMED, "Y", spec, note)
        # 그룹 안에서 '모든 Unit이 같은 방향으로 Spec 이탈'하는 항목 → 개별 Spec 후보(비활성)
        if t in CHAIN_TESTS and len(members) > 1:
            lsl, usl = num(spec["LSL"]), num(spec["USL"])
            for it in members:
                iv = vals[it["COL_NO"]]
                if len(iv) >= 3 and (all(x > usl for x in iv) or all(x < lsl for x in iv)):
                    overrides.append((part, it, iv))

    for part, it, iv in overrides:
        spec = sigma_spec(part[0], iv, k_sigma, fence_k)
        add(part, it["ITEM_ID"], 1, PRIO_OVERRIDE, "N", spec,
            f"모든 Unit이 그룹 Spec 이탈(평균 {spec['MEAN']}) – 설계상 다른 구조면 ENABLED=Y, 아니면 불량 분석 대상")
    return rules


# ---------------------------------------------------------------- 명령
def cmd_parse(a):
    hdr, _ = read_data(a.data)
    items = parse_all(hdr)
    out = os.path.join(a.out, "header_parsed.csv")
    write_csv(out, PARSE_FIELDS, items)
    print(f"[parse] 테스트 항목 {len(items)}개 → {out}")
    for key in ("TEST", "COND_NAME", "COND_VALUE", "COND_UNIT", "STRUCT_TYPE", "PIN"):
        print(f"  {key:12s}", dict(Counter(it[key] for it in items).most_common()))
    named = sorted({it["ITEM_ID"] for it in items if it["X"] == "" and ":" not in it["ITEM_ID"]})
    print(f"  이름형 ID {len(named)}개:", ", ".join(named))
    bad = [it for it in items if it["PARSE_NOTE"]]
    print(f"  파싱 경고 {len(bad)}건" + "".join(f"\n    {b['COL_NO']}: {b['RAW_HEADER']} ({b['PARSE_NOTE']})" for b in bad[:20]))


def cmd_draft(a):
    if os.path.exists(a.rules) and not a.force:
        raise SystemExit(f"{a.rules} 가 이미 있습니다. 덮어쓰려면 --force (관리 중인 Rule이 지워집니다)")
    hdr, data = read_data(a.data)
    items = parse_all(hdr)
    rules = build_draft(items, data, a.sigma, a.fence_k, a.merge_pct, a.leak_sd_ratio, a.leak_mean_sigma)
    write_csv(a.rules, RULE_FIELDS, rules)
    print(f"[draft] 항목 {len(items)}개 → Rule {len(rules)}개 (비활성 후보 "
          f"{sum(r['ENABLED'] == 'N' for r in rules)}개) → {a.rules}")
    print(f"  {'RULE_ID':8s} {'TEST':12s} {'COND_NAME':9s} {'STRUCT':6s} {'ID_PATTERN':36s} {'CNT':>4s} "
          f"{'MEAN':>9s} {'STDEV':>7s} {'MAX':>7s} {'MIN':>7s} {'LSL':>9s} {'USL':>9s}")
    for r in rules:
        print(f"  {r['RULE_ID']:8s} {r['TEST']:12s} {r['COND_NAME']:9s} {r['STRUCT_TYPE']:6s} {r['ID_PATTERN']:36s} "
              f"{r['ITEM_CNT']:>4} {r.get('MEAN', ''):>9s} {r.get('STDEV', ''):>7s} {r.get('MAX', ''):>7s} "
              f"{r.get('MIN', ''):>7s} {r.get('LSL', ''):>9s} {r.get('USL', ''):>9s}" + ("  (비활성)" * (r['ENABLED'] != 'Y')))


def cmd_apply(a):
    hdr, data = read_data(a.data)
    items = parse_all(hdr)
    rules = read_rules(a.rules)
    active = resolve(items, rules)
    os.makedirs(a.out, exist_ok=True)

    # 항목별 Spec 맵 (MES 적재용 평탄화 결과)
    for it in items:
        r = it["RULE"] or {}
        it.update(RULE_ID=r.get("RULE_ID", ""), PRIORITY=r.get("PRIORITY", ""), UNIT=r.get("UNIT", ""),
                  LSL=r.get("LSL", ""), MEAN=r.get("MEAN", ""), USL=r.get("USL", ""))
    map_fields = PARSE_FIELDS + ["RULE_ID", "PRIORITY", "UNIT", "LSL", "MEAN", "USL", "MATCH_STATUS",
                                 "ALSO_MATCHED"]
    write_csv(os.path.join(a.out, "item_spec_map.csv"), map_fields, items)

    # 판정
    fails, unit_sum = [], []
    stat = defaultdict(lambda: {"vals": [], "items": 0, "FAIL_HIGH": 0, "FAIL_LOW": 0, "NO_DATA": 0,
                                "fail_units": set(), "fail_items": Counter()})
    for it in items:
        if it["RULE"]:
            stat[it["RULE_ID"]]["items"] += 1
    for row in data:
        uid = row[0]
        cnt, by_rule = Counter(), Counter()
        for it in items:
            if not it["RULE"]:
                continue
            raw = row[it["COL_NO"]] if it["COL_NO"] < len(row) else ""
            x = num(raw)
            res = judge(x, num(it["LSL"]), num(it["USL"]))
            cnt[res] += 1
            s = stat[it["RULE_ID"]]
            if x is not None:
                s["vals"].append(x)
            if res != "PASS":
                s[res] += 1
                s["fail_units"].add(uid)
                s["fail_items"][it["ITEM_ID"]] += 1
                by_rule[it["RULE_ID"]] += 1
                fails.append(dict(UNIT_ID=uid, COL_NO=it["COL_NO"], RAW_HEADER=it["RAW_HEADER"],
                                  RULE_ID=it["RULE_ID"], VALUE=raw, LSL=it["LSL"], USL=it["USL"], RESULT=res))
        unit_sum.append(dict(UNIT_ID=uid, TOTAL=sum(cnt.values()), PASS=cnt["PASS"], FAIL_HIGH=cnt["FAIL_HIGH"],
                             FAIL_LOW=cnt["FAIL_LOW"], NO_DATA=cnt["NO_DATA"],
                             RESULT="PASS" if sum(cnt.values()) == cnt["PASS"] else "FAIL",
                             FAIL_RULES=" ".join(f"{k}({v})" for k, v in by_rule.most_common())))
    write_csv(os.path.join(a.out, "judge_fail_list.csv"),
              ["UNIT_ID", "COL_NO", "RAW_HEADER", "RULE_ID", "VALUE", "LSL", "USL", "RESULT"], fails)
    write_csv(os.path.join(a.out, "judge_unit_summary.csv"),
              ["UNIT_ID", "TOTAL", "PASS", "FAIL_HIGH", "FAIL_LOW", "NO_DATA", "RESULT", "FAIL_RULES"], unit_sum)

    # Rule별 통계 (Limit 보정용, 현재 Rule 기준 실제 매칭 결과)
    rs = []
    for r in rules:
        s = stat.get(r["RULE_ID"])
        v = sorted(s["vals"]) if s else []
        rs.append(dict(r, MATCHED_CNT=s["items"] if s else 0, VALUE_CNT=len(v),
                       DATA_MEDIAN=fmt(sig(statistics.median(v))) if v else "",
                       DATA_MIN=fmt(v[0]) if v else "", DATA_MAX=fmt(v[-1]) if v else "",
                       FAIL_HIGH=s["FAIL_HIGH"] if s else 0, FAIL_LOW=s["FAIL_LOW"] if s else 0,
                       NO_DATA=s["NO_DATA"] if s else 0, FAIL_UNIT_CNT=len(s["fail_units"]) if s else 0,
                       TOP_FAIL_ITEMS=" ".join(f"{k}({c})" for k, c in s["fail_items"].most_common(5)) if s else ""))
    write_csv(os.path.join(a.out, "rule_stats.csv"),
              RULE_FIELDS + ["MATCHED_CNT", "VALUE_CNT", "DATA_MEDIAN", "DATA_MIN", "DATA_MAX", "FAIL_HIGH",
                             "FAIL_LOW", "NO_DATA", "FAIL_UNIT_CNT", "TOP_FAIL_ITEMS"], rs)

    # 검증 리포트
    st = Counter(it["MATCH_STATUS"] for it in items)
    unused = [r["RULE_ID"] for r in active if not stat.get(r["RULE_ID"]) or stat[r["RULE_ID"]]["items"] == 0]
    lines = [f"테스트 항목: {len(items)}  /  Rule: 전체 {len(rules)}, 활성 {len(active)}",
             f"매칭: OK {st['OK']}, UNMATCHED {st['UNMATCHED']}, AMBIGUOUS {st['AMBIGUOUS']}",
             f"미사용 활성 Rule: {', '.join(unused) or '없음'}"]
    lines += [f"  UNMATCHED  {it['COL_NO']}: {it['RAW_HEADER']}" for it in items if it["MATCH_STATUS"] == "UNMATCHED"]
    lines += [f"  AMBIGUOUS  {it['COL_NO']}: {it['RAW_HEADER']} → {it['ALSO_MATCHED']}"
              for it in items if it["MATCH_STATUS"] == "AMBIGUOUS"]
    lines += [f"  참고 ITEM_CNT {r['RULE_ID']}: Rule {r['ITEM_CNT']} → 실제 {stat[r['RULE_ID']]['items']} "
              f"(우선순위 높은 Rule로 일부 이동했거나 헤더 변경)"
              for r in active if num(r["ITEM_CNT"]) is not None and num(r["ITEM_CNT"]) != stat[r["RULE_ID"]]["items"]]
    lines.append(f"Unit 판정: {sum(u['RESULT'] == 'PASS' for u in unit_sum)} PASS / "
                 f"{sum(u['RESULT'] == 'FAIL' for u in unit_sum)} FAIL (Unit {len(unit_sum)}개, NO_DATA는 FAIL 처리)")
    with open(os.path.join(a.out, "validation_report.txt"), "w", encoding="utf-8-sig") as f:
        f.write("\n".join(lines) + "\n")
    print("[apply] " + "\n        ".join(lines[:3] + lines[3:-1][:10] + lines[-1:]))
    print(f"        결과 → {a.out}\\ (item_spec_map, rule_stats, judge_unit_summary, judge_fail_list)")
    return 2 if st["UNMATCHED"] or st["AMBIGUOUS"] else 0


def main():
    ap = argparse.ArgumentParser(description="Test 헤더 파싱 / Spec Rule 관리")
    sub = ap.add_subparsers(dest="cmd", required=True)
    for name in ("parse", "draft", "apply"):
        p = sub.add_parser(name)
        p.add_argument("--data", required=True, help="측정 데이터 (TSV/CSV, 1행=헤더, 1열=Unit ID)")
        p.add_argument("--out", default="out", help="결과 폴더")
        if name in ("draft", "apply"):
            p.add_argument("--rules", default="spec_rules.csv", help="Spec Rule 파일")
        if name == "draft":
            p.add_argument("--sigma", type=float, default=3, help="Spec = 평균 ± k·σ 의 k (기본 3)")
            p.add_argument("--fence-k", type=float, default=3,
                           help="평균·σ 계산 전 이상치 제외 기준 Q1/Q3 ∓ k·IQR (기본 3)")
            p.add_argument("--merge-pct", type=float, default=10,
                           help="저항/전압형 ID_PATTERN 통합 기준: ID별 중앙값 편차 %% (기본 10, 0이면 통합 안 함)")
            p.add_argument("--leak-sd-ratio", type=float, default=1.5, help="Leakage 통합 기준: σ 비율 (기본 1.5)")
            p.add_argument("--leak-mean-sigma", type=float, default=0.5,
                           help="Leakage 통합 기준: 평균 차이 (σ 배수, 기본 0.5)")
            p.add_argument("--force", action="store_true", help="기존 Rule 파일 덮어쓰기")
    a = ap.parse_args()
    sys.exit({"parse": cmd_parse, "draft": cmd_draft, "apply": cmd_apply}[a.cmd](a) or 0)


if __name__ == "__main__":
    main()
