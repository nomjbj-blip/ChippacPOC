// 회의록 Word 생성기
// 규칙 원본: D:\1_프로젝트 문서\98.AI 관련 정보\DocumentAgent\회의록 전용 Agent\AGENTS.md
// 사용법: node make_minutes.js <회의록_내용.json> <출력.docx>
//   docx 패키지가 없으면 NODE_PATH 로 설치 위치를 지정한다.
const fs = require("fs");
const {
  Document, Packer, Paragraph, TextRun, Table, TableRow, TableCell,
  AlignmentType, LevelFormat, LevelSuffix, WidthType, BorderStyle, ShadingType, VerticalAlign,
} = require("docx");

const [, , inPath, outPath] = process.argv;
if (!inPath || !outPath) {
  console.error("usage: node make_minutes.js <content.json> <out.docx>");
  process.exit(1);
}
const data = JSON.parse(fs.readFileSync(inPath, "utf8"));

const FONT = "맑은 고딕";
const IN = 1440; // 1인치 = 1440 DXA
const RED = "C00000";
// 모든 회의록 문단에서 페이지 제어 속성을 해제한다 (왼쪽 검은 사각형 방지)
const NO_PAGE_CTRL = { keepNext: false, keepLines: false, pageBreakBefore: false };

// "[확인 필요]", "[원문 확인 필요]", "[확인 필요: ...]" 부분만 붉은색 문자 스타일로 분리한다
function runs(text, extra = {}) {
  const out = [];
  const re = /\[(?:원문 )?확인 필요[^\]]*\]/g;
  let last = 0, m;
  while ((m = re.exec(text)) !== null) {
    if (m.index > last) out.push(new TextRun({ text: text.slice(last, m.index), ...extra }));
    out.push(new TextRun({ text: m[0], style: "CheckNeeded", ...extra }));
    last = m.index + m[0].length;
  }
  if (last < text.length) out.push(new TextRun({ text: text.slice(last), ...extra }));
  return out;
}

const p = (style, text, opts = {}) =>
  new Paragraph({ style, children: runs(text), ...NO_PAGE_CTRL, ...opts });

// ---- 번호 목록: 수준별 독립 단일 단계 목록 ----
const numbering = {
  config: [
    {
      reference: "big",
      levels: [{ level: 0, format: LevelFormat.UPPER_ROMAN, text: "%1.", alignment: AlignmentType.LEFT, suffix: LevelSuffix.SPACE, // II. III. 폭 차이로 탭 위치가 밀리지 않게 공백 사용
        style: { paragraph: { indent: { left: 0.2 * IN, hanging: 0.2 * IN } } } }],
    },
    {
      reference: "mid",
      levels: [{ level: 0, format: LevelFormat.DECIMAL, text: "%1.", alignment: AlignmentType.LEFT,
        style: { paragraph: { indent: { left: 0.6 * IN, hanging: 0.2 * IN } } } }],
    },
    {
      reference: "sub",
      levels: [{ level: 0, format: LevelFormat.DECIMAL_ENCLOSED_CIRCLE, text: "%1", alignment: AlignmentType.LEFT,
        style: { paragraph: { indent: { left: 0.95 * IN, hanging: 0.2 * IN } } } }],
    },
    {
      reference: "bullet",
      levels: [{ level: 0, format: LevelFormat.BULLET, text: "\u2022", alignment: AlignmentType.LEFT,
        style: { paragraph: { indent: { left: 0.2 * IN, hanging: 0.2 * IN } } } }],
    },
  ],
};

// ---- 스타일: 모두 Normal 기반 ----
const base = (id, name, size, bold, para = {}) => ({
  id, name, basedOn: "Normal", next: "Normal", quickFormat: true,
  run: { font: FONT, size: size * 2, bold, color: "000000" },
  paragraph: { ...NO_PAGE_CTRL, ...para },
});
const styles = {
  default: {
    document: {
      run: { font: FONT, size: 20, color: "000000" },
      paragraph: { spacing: { line: 300, before: 0, after: 60 } },
    },
  },
  paragraphStyles: [
    base("MinutesTitle", "회의록 제목", 20, true, { alignment: AlignmentType.CENTER, spacing: { after: 240 } }),
    base("SummaryTitle", "요약 제목", 14, true, { spacing: { before: 240, after: 120 } }),
    base("SummaryItem", "내용 요약 항목", 10, false, { spacing: { line: 300, after: 60 } }),
    base("BigAgenda", "큰 안건", 14, true, { spacing: { before: 280, after: 120 } }),
    base("MidAgenda", "세부 안건", 12, true, { spacing: { before: 160, after: 80 } }),
    base("SubAgenda", "하위 안건", 10, false, { spacing: { line: 300, after: 60 } }),
    base("Body", "본문", 10, false, { spacing: { line: 300, after: 60 } }),
    base("TableText", "표 본문", 10, false, { spacing: { line: 260, before: 40, after: 40 } }),
  ],
  characterStyles: [
    { id: "CheckNeeded", name: "확인 필요", basedOn: "DefaultParagraphFont", run: { color: RED } },
  ],
};

// ---- 표 ----
const border = { style: BorderStyle.SINGLE, size: 4, color: "808080" };
const borders = { top: border, bottom: border, left: border, right: border };
const margins = { top: 60, bottom: 60, left: 100, right: 100 };
function cell(text, width, { header = false, center = false } = {}) {
  return new TableCell({
    borders, margins, width: { size: width, type: WidthType.DXA }, verticalAlign: VerticalAlign.CENTER,
    shading: header ? { fill: "E7E6E6", type: ShadingType.CLEAR, color: "auto" } : undefined,
    children: String(text).split("\n").map((line) => new Paragraph({
      style: "TableText", ...NO_PAGE_CTRL,
      alignment: center ? AlignmentType.CENTER : AlignmentType.LEFT,
      children: runs(line, header ? { bold: true } : {}),
    })),
  });
}
function table(widths, rows, header = false) {
  const total = widths.reduce((a, b) => a + b, 0);
  return new Table({
    width: { size: total, type: WidthType.DXA }, columnWidths: widths,
    rows: rows.map((r, i) => new TableRow({ children: r, tableHeader: i === 0 && header })),
  });
}

// ---- 본문 구성 ----
const PAGE_W = 11906, MARGIN = 1134; // A4, 여백 2cm
const CONTENT_W = PAGE_W - MARGIN * 2;
const children = [];

children.push(p("MinutesTitle", data.title || "회의록"));

const info = data.info;
const infoW = [1800, CONTENT_W - 1800];
children.push(table(infoW, [
  ["회의명", info.name], ["일시", info.datetime], ["장소", info.place],
  ["참석자", info.attendees], ["작성자", info.writer],
].map(([k, v]) => [cell(k, infoW[0], { header: true, center: true }), cell(v || "[확인 필요]", infoW[1])])));

children.push(p("SummaryTitle", "[내용 요약]"));
data.summary.forEach((s) => children.push(p("SummaryItem", s, { numbering: { reference: "bullet", level: 0 } })));

let midInst = 0, subInst = 0;
const subList = (items) => {
  subInst++;
  items.forEach((t) => children.push(p("SubAgenda", t, { numbering: { reference: "sub", level: 0, instance: subInst } })));
};
data.agendas.forEach((a) => {
  children.push(p("BigAgenda", a.title, { numbering: { reference: "big", level: 0 } }));
  if (a.items) subList(a.items); // 세부 안건 없이 바로 하위 내용
  if (a.sections) {
    midInst++;
    a.sections.forEach((s) => {
      children.push(p("MidAgenda", s.title, { numbering: { reference: "mid", level: 0, instance: midInst } }));
      if (s.items) subList(s.items);
    });
  }
});

// 결정 및 후속 조치
children.push(p("BigAgenda", "결정 및 후속 조치", { numbering: { reference: "big", level: 0 } }));
const actW = [CONTENT_W - 1900 - 1700 - 1300, 1900, 1700, 1300];
children.push(table(actW, [
  ["조치 내용", "담당", "기한", "상태"].map((h, i) => cell(h, actW[i], { header: true, center: true })),
  ...data.actions.map((x) => [
    cell(x.task, actW[0]), cell(x.owner || "[확인 필요]", actW[1], { center: true }),
    cell(x.due || "[확인 필요]", actW[2], { center: true }), cell(x.status || "", actW[3], { center: true }),
  ]),
], true)); // 페이지가 넘어가면 머리행 반복
if (data.notes && data.notes.length) {
  // 빈 문단 대신 첫 참고 문단 위 간격으로 표와 띄운다 (빈 줄로 페이지가 넘어가는 것 방지)
  data.notes.forEach((n, i) => children.push(p("Body", n, i === 0 ? { spacing: { before: 200 } } : {})));
}

const doc = new Document({
  creator: info.writer || "", title: info.name || "회의록",
  styles, numbering,
  sections: [{
    properties: { page: { size: { width: PAGE_W, height: 16838 },
      margin: { top: MARGIN, bottom: MARGIN, left: MARGIN, right: MARGIN } } },
    children,
  }],
});
Packer.toBuffer(doc).then((buf) => { fs.writeFileSync(outPath, buf); console.log("saved:", outPath); });
