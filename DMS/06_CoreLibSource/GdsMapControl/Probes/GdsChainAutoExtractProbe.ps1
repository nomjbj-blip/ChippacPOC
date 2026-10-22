# 같은 형태 Chain 자동 추출 검증 스크립트 (2026-10-20)
# 목적: 예시 Chain(Input + 후보 경로 찾기로 만든 연결 덩어리)과 같은 형태의 Chain 을 도면 전체에서 한 번에 찾을 수 있는지 본다.
#       1) 탐색 Layer 도형 전체를 공간 격자에 넣고, 닿는 도형끼리 묶어(Union-Find) 연결 덩어리를 모두 구한다.
#          연결 판정은 화면 [후보 경로 찾기]와 같다(외곽 상자 접촉 -> ChainGeometryOverlap.Intersects, Layer 규칙 끔).
#       2) Label(예: Layer 50/3 TSV 번호)이 들어 있는 도형의 덩어리에 Label 이름을 붙인다(같은 자리 2벌은 하나로).
#       3) 예시 Label 의 덩어리를 기준으로 도형 구성 / 상대 위치를 비교해 일치 / 회전·대칭 일치 / 구성 같음·배치 다름 / 구성 다름으로 나눈다.
#       4) 일부 덩어리는 제품 추적기(ChainCandidateTracer, Output 없음)로 다시 찾아 결과가 같은지 확인한다.
#       DB 는 조회만 한다(서비스를 띄우지 않고 BSL 직접 호출). 결과 CSV 는 OutDir 에 남긴다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsChainAutoExtractProbe.ps1 [-Factory SCK] [-Device U8]
#       [-Layers 850/0,860/0,900/0] [-LabelLayer 50/3] [-LabelPrefix TSV_] [-TemplateLabel TSV_21063] [-VerifySamples 300] [-SizeStep 5]
#       [-BinDir <BSL/DSL/Interface 폴더>] [-GdsDir <GdsMap EXE 폴더>] [-OutDir <결과 폴더>]
param(
    [string]$Factory = 'SCK',
    [string]$Device = 'U8',
    [string]$Layers = '850/0,860/0,900/0',
    [string]$LabelLayer = '50/3',
    [string]$LabelPrefix = 'TSV_',
    [string]$TemplateLabel = 'TSV_21063',
    [int]$VerifySamples = 300,
    [double]$SizeStep = 5,
    [string]$BinDir = '',
    [string]$GdsDir = '',
    [string]$OutDir = ''
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$serverBin = Join-Path $dms '101_ServerBin'
if ($BinDir -eq '') { $BinDir = $serverBin }
if ($GdsDir -eq '') { $GdsDir = Join-Path $dms '100_ClientBin\GdsMap' }
if ($OutDir -eq '') { $OutDir = Join-Path $env:TEMP 'chippac\chainautoextract' }
New-Item -ItemType Directory -Force $OutDir | Out-Null
[AppDomain]::CurrentDomain.SetData('APP_CONFIG_FILE', (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config'))
Add-Type -AssemblyName System.Configuration
$flags = [Reflection.BindingFlags]'NonPublic,Static'
$cm = [Configuration.ConfigurationManager]
$cm.GetField('s_initState', $flags).SetValue($null, 0)
$cm.GetField('s_configSystem', $flags).SetValue($null, $null)

# GdsMap EXE 는 .dll 이름으로 복사해 참조한다(Add-Type 은 .exe 참조 불가).
$probeDir = Join-Path $env:TEMP 'chippac\chainautoextractbin'
New-Item -ItemType Directory -Force $probeDir | Out-Null
Copy-Item (Join-Path $GdsDir 'NexplantQMS.GdsMap.dll') (Join-Path $probeDir 'NexplantQMS.GdsMap.dll') -Force
$searchDirs = @($BinDir, $GdsDir, $probeDir, $serverBin)
[AppDomain]::CurrentDomain.add_AssemblyResolve([ResolveEventHandler] {
    param($s, $e)
    $name = (New-Object Reflection.AssemblyName($e.Name)).Name
    foreach ($d in $searchDirs) { foreach ($ext in '.dll', '.exe') { $p = Join-Path $d ($name + $ext); if (Test-Path $p) { return [Reflection.Assembly]::LoadFrom($p) } } }
    return $null
})
foreach ($p in @("$serverBin\Oracle.ManagedDataAccess.dll", "$serverBin\Miracom.Middleware.dll", "$serverBin\DACrux.Base.dll", "$BinDir\DACrux.SEMDMS.Interface.dll",
        "$BinDir\DACrux.SEMDMS.DSL.dll", "$BinDir\DACrux.SEMDMS.BSL.dll", "$probeDir\NexplantQMS.GdsMap.dll")) { [void][Reflection.Assembly]::LoadFrom($p) }

$helper = @'
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using NexplantQMS.GdsMap;
using NexplantQMS.GdsMap.Chain;
using NexplantQMS.GdsMap.Persistence;

public static class ChainAutoExtract
{
    public sealed class Label { public string Text; public double X; public double Y; public int Root = -1; }
    public sealed class Part { public string Key; public double Dx; public double Dy; }
    public sealed class Component
    {
        public int Root; public List<int> Members = new List<int>(); public List<string> Labels = new List<string>();
        public string Histogram; public string Class; public int Transform = -1; public GBox Bounds;
    }

    static readonly List<ChainTraceElement> Elements = new List<ChainTraceElement>();
    static readonly List<Label> Labels = new List<Label>();
    static readonly Dictionary<long, List<int>> Grid = new Dictionary<long, List<int>>();
    static int[] Parent;
    static double CellSize = 100;
    /// <summary>구성 키의 폭 / 높이 묶음 단위(µm). 1 µm 반올림은 114.5 같은 경계 값에서 같은 도형이 갈린다.</summary>
    public static double SizeStep = 5;
    public static readonly List<Component> Components = new List<Component>();
    public static Component Template;
    public static readonly StringBuilder Log = new StringBuilder();

    static string F(double v) { return v.ToString("0.###", CultureInfo.InvariantCulture); }
    static long CellKey(long cx, long cy) { return (cx << 32) ^ (cy & 0xFFFFFFFFL); }
    static long Cell(double v) { return (long)Math.Floor(v / CellSize); }

    /// <summary>탐색 Layer 도형과 Label 을 BSL 페이지 조회로 읽는다. 반환 "도형 수 / Label 수 / ms"</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string Load(string factory, string device, string[] layerKeys, string labelLayer, string labelPrefix)
    {
        var sw = Stopwatch.StartNew();
        var svc = new DACrux.SEMDMS.BSL.GdsMapView();
        foreach (string key in layerKeys.Concat(new[] { labelLayer }))
        {
            bool labelPage = key == labelLayer;
            string after = string.Empty;
            while (true)
            {
                DataTable page = svc.GetPlacedElementPage(factory, device, key, after, 50000);
                foreach (DataRow row in page.Rows)
                {
                    MapPlacedElementData m = GdsMapPlacedRowReader.Read(row);
                    bool text = string.Equals(m.ElementType, "TEXT", StringComparison.OrdinalIgnoreCase);
                    if (labelPage)
                    {
                        if (text && m.Text != null && m.Text.StartsWith(labelPrefix, StringComparison.Ordinal))
                            Labels.Add(new Label { Text = m.Text, X = m.Bounds.MinX, Y = m.Bounds.MinY });
                    }
                    else if (!text && m.WorldPoints != null && m.WorldPoints.Length > 0)
                        Elements.Add(new ChainTraceElement(m.PlacedElementId, m.LayerId, m.Bounds, m.ElementType, m.WorldPoints, m.PathWidth, m.DataType));
                }
                int n = page.Rows.Count;
                if (n > 0) after = page.Rows[n - 1]["PLACED_ELEMENT_ID"].ToString();
                page.Dispose();
                if (n < 50000) break;
            }
        }
        // 같은 자리 같은 이름 Label(2벌)은 하나로 본다.
        var seen = new HashSet<string>(StringComparer.Ordinal);
        int before = Labels.Count;
        Labels.RemoveAll(l => !seen.Add(l.Text + "@" + Math.Round(l.X, 2) + "," + Math.Round(l.Y, 2)));
        return Elements.Count + " / Label " + Labels.Count + " (같은 자리 중복 " + (before - Labels.Count) + " 제외) / " + sw.ElapsedMilliseconds + " ms";
    }

    static int Find(int x) { while (Parent[x] != x) { Parent[x] = Parent[Parent[x]]; x = Parent[x]; } return x; }

    static bool BoundsTouch(GBox a, GBox b)
    {
        return a.MaxX >= b.MinX && b.MaxX >= a.MinX && a.MaxY >= b.MinY && b.MaxY >= a.MinY;
    }

    /// <summary>격자 색인 + 닿는 도형 묶기. 반환 "검사 쌍 / 묶음 / 덩어리 수 / ms"</summary>
    public static string Connect(double cellSize)
    {
        var sw = Stopwatch.StartNew();
        CellSize = cellSize;
        int n = Elements.Count;
        Parent = new int[n];
        for (int i = 0; i < n; i++) Parent[i] = i;
        for (int i = 0; i < n; i++)
        {
            GBox b = Elements[i].Bounds;
            for (long cx = Cell(b.MinX); cx <= Cell(b.MaxX); cx++)
                for (long cy = Cell(b.MinY); cy <= Cell(b.MaxY); cy++)
                {
                    List<int> list;
                    long k = CellKey(cx, cy);
                    if (!Grid.TryGetValue(k, out list)) Grid[k] = list = new List<int>();
                    list.Add(i);
                }
        }
        long tested = 0, unions = 0;
        foreach (List<int> list in Grid.Values)
            for (int a = 0; a < list.Count; a++)
                for (int c = a + 1; c < list.Count; c++)
                {
                    int i = list[a], j = list[c];
                    int ri = Find(i), rj = Find(j);
                    if (ri == rj) continue;
                    if (!BoundsTouch(Elements[i].Bounds, Elements[j].Bounds)) continue;
                    tested++;
                    if (!ChainGeometryOverlap.Intersects(Elements[i], Elements[j])) continue;
                    Parent[ri] = rj;
                    unions++;
                }
        var byRoot = new Dictionary<int, Component>();
        for (int i = 0; i < n; i++)
        {
            int r = Find(i);
            Component comp;
            if (!byRoot.TryGetValue(r, out comp)) { comp = new Component { Root = r }; byRoot[r] = comp; Components.Add(comp); }
            comp.Members.Add(i);
        }
        foreach (Component comp in Components)
        {
            double minX = double.MaxValue, minY = double.MaxValue, maxX = double.MinValue, maxY = double.MinValue;
            foreach (int i in comp.Members)
            {
                GBox b = Elements[i].Bounds;
                minX = Math.Min(minX, b.MinX); minY = Math.Min(minY, b.MinY); maxX = Math.Max(maxX, b.MaxX); maxY = Math.Max(maxY, b.MaxY);
            }
            comp.Bounds = new GBox(minX, minY, maxX, maxY);
            comp.Histogram = string.Join(" + ", comp.Members.GroupBy(i => ShapeKey(Elements[i])).OrderBy(g => g.Key, StringComparer.Ordinal)
                .Select(g => g.Count() + "x[" + g.Key + "]").ToArray());
        }
        return tested + " / " + unions + " / 덩어리 " + Components.Count + " / " + sw.ElapsedMilliseconds + " ms";
    }

    /// <summary>도형 구성 키: Layer/DataType, 종류, 꼭짓점 수(닫힌 점 중복 제외), 폭 x 높이(µm 반올림).</summary>
    static string ShapeKey(ChainTraceElement e)
    {
        return ShapeKey(e, false);
    }

    static string ShapeKey(ChainTraceElement e, bool swap)
    {
        int pc = e.WorldPoints.Length;
        bool closed = !string.Equals(e.ElementType, "PATH", StringComparison.OrdinalIgnoreCase);
        if (closed && pc > 1 && Math.Abs(e.WorldPoints[0].X - e.WorldPoints[pc - 1].X) < 1e-6 && Math.Abs(e.WorldPoints[0].Y - e.WorldPoints[pc - 1].Y) < 1e-6) pc--;
        double w = Math.Round((e.Bounds.MaxX - e.Bounds.MinX) / SizeStep, MidpointRounding.AwayFromZero) * SizeStep;
        double h = Math.Round((e.Bounds.MaxY - e.Bounds.MinY) / SizeStep, MidpointRounding.AwayFromZero) * SizeStep;
        if (swap) { double t = w; w = h; h = t; }
        return GdsLayerKey.Format(e.LayerKey) + " " + e.ElementType.ToUpperInvariant() + " " + pc + "점 " + F(w) + "x" + F(h);
    }

    static GPoint Center(ChainTraceElement e) { return new GPoint((e.Bounds.MinX + e.Bounds.MaxX) / 2, (e.Bounds.MinY + e.Bounds.MaxY) / 2); }

    /// <summary>Label 이 들어 있는 도형의 덩어리에 이름을 붙인다. 반환 "붙은 Label / 도형 밖 Label"</summary>
    public static string AttachLabels()
    {
        int attached = 0, outside = 0;
        var byRoot = Components.ToDictionary(c => c.Root);
        foreach (Label l in Labels)
        {
            List<int> list;
            if (!Grid.TryGetValue(CellKey(Cell(l.X), Cell(l.Y)), out list)) { outside++; continue; }
            var p = new GPoint(l.X, l.Y);
            foreach (int i in list)
            {
                ChainTraceElement e = Elements[i];
                if (l.X < e.Bounds.MinX || l.X > e.Bounds.MaxX || l.Y < e.Bounds.MinY || l.Y > e.Bounds.MaxY) continue;
                bool path = string.Equals(e.ElementType, "PATH", StringComparison.OrdinalIgnoreCase);
                if (!ChainGeometryOverlap.ContainsPoint(p, e.WorldPoints, path, e.PathWidth, 0)) continue;
                l.Root = Find(i);
                break;
            }
            if (l.Root < 0) { outside++; continue; }
            attached++;
            Component comp = byRoot[l.Root];
            if (!comp.Labels.Contains(l.Text)) comp.Labels.Add(l.Text);
        }
        return attached + " / " + outside;
    }

    /// <summary>덩어리의 기준점: Label 위치(있으면), 없으면 가장 드문 구성 키 도형의 중심.</summary>
    static bool Anchor(Component comp, string anchorKey, bool swap, out GPoint anchor)
    {
        foreach (int i in comp.Members)
            if (ShapeKey(Elements[i], swap) == anchorKey) { anchor = Center(Elements[i]); return true; }
        anchor = new GPoint(0, 0);
        return false;
    }

    static List<Part> Parts(Component comp, GPoint anchor)
    {
        return comp.Members.Select(i => new Part { Key = ShapeKey(Elements[i]), Dx = Center(Elements[i]).X - anchor.X, Dy = Center(Elements[i]).Y - anchor.Y }).ToList();
    }

    /// <summary>8 방향 변형(회전 0/90/180/270 x 좌우 대칭) 중 t 를 예시 상대 위치에 적용한다.</summary>
    static void Transform(int t, double x, double y, out double ox, out double oy)
    {
        double rx = x, ry = y;
        if ((t & 4) != 0) rx = -rx;               // 좌우 대칭
        switch (t & 3)
        {
            case 0: ox = rx; oy = ry; break;
            case 1: ox = -ry; oy = rx; break;      // 90
            case 2: ox = -rx; oy = -ry; break;     // 180
            default: ox = ry; oy = -rx; break;     // 270
        }
    }

    static string SwapKey(string key)
    {
        int s = key.LastIndexOf(' ');
        string[] wh = key.Substring(s + 1).Split('x');
        return key.Substring(0, s + 1) + wh[1] + "x" + wh[0];
    }

    /// <summary>예시와 비교해 분류한다. 반환 분류별 개수 문자열.</summary>
    public static string Classify(string templateLabel, double tolerance)
    {
        Template = Components.FirstOrDefault(c => c.Labels.Contains(templateLabel));
        if (Template == null) return "예시 Label 을 가진 덩어리가 없습니다: " + templateLabel;
        // 기준 도형 = 예시에서 가장 드문 구성 키(같은 개수면 이름 순 첫 번째)
        string anchorKey = Template.Members.GroupBy(i => ShapeKey(Elements[i])).OrderBy(g => g.Count()).ThenBy(g => g.Key, StringComparer.Ordinal).First().Key;
        GPoint tAnchor;
        Anchor(Template, anchorKey, false, out tAnchor);
        List<Part> tParts = Parts(Template, tAnchor);
        var templateMultiset = Template.Members.Select(i => ShapeKey(Elements[i])).OrderBy(k => k, StringComparer.Ordinal).ToList();
        Log.AppendLine("예시 " + templateLabel + ": 도형 " + Template.Members.Count + "개 / 기준 도형 [" + anchorKey + "] / 구성 " + Template.Histogram);
        foreach (Component comp in Components)
        {
            var multiset = comp.Members.Select(i => ShapeKey(Elements[i])).OrderBy(k => k, StringComparer.Ordinal).ToList();
            bool sameParts = multiset.SequenceEqual(templateMultiset);
            int matched = -1;
            for (int t = 0; t < 8 && matched < 0; t++)
            {
                bool swap = (t & 1) == 1;
                // 90 / 270 도 회전이면 예시 도형의 폭 / 높이가 바뀐다.
                var expectKeys = templateMultiset.Select(k => swap ? SwapKey(k) : k).OrderBy(k => k, StringComparer.Ordinal).ToList();
                if (!multiset.SequenceEqual(expectKeys)) continue;
                GPoint anchor;
                if (!Anchor(comp, swap ? SwapKey(anchorKey) : anchorKey, false, out anchor)) continue;
                List<Part> parts = Parts(comp, anchor);
                var used = new bool[parts.Count];
                bool all = true;
                foreach (Part tp in tParts)
                {
                    double ex, ey;
                    Transform(t, tp.Dx, tp.Dy, out ex, out ey);
                    string key = swap ? SwapKey(tp.Key) : tp.Key;
                    int hit = -1;
                    for (int k = 0; k < parts.Count; k++)
                        if (!used[k] && parts[k].Key == key && Math.Abs(parts[k].Dx - ex) <= tolerance && Math.Abs(parts[k].Dy - ey) <= tolerance) { hit = k; break; }
                    if (hit < 0) { all = false; break; }
                    used[hit] = true;
                }
                if (all) matched = t;
            }
            comp.Transform = matched;
            if (matched == 0) comp.Class = "일치";
            else if (matched > 0) comp.Class = "회전·대칭 일치";
            else if (sameParts) comp.Class = "구성 같음·배치 다름";
            else if (comp.Members.Count > Template.Members.Count) comp.Class = "구성 다름(도형 더 많음)";
            else comp.Class = "구성 다름(도형 더 적음)";
        }
        return string.Join(" / ", Components.GroupBy(c => c.Class).OrderByDescending(g => g.Count()).Select(g => g.Key + " " + g.Count()).ToArray());
    }

    /// <summary>Label 개수별 분포(일치 / 회전·대칭 일치 덩어리만).</summary>
    public static string LabelStats()
    {
        var ok = Components.Where(c => c.Transform >= 0).ToList();
        return "Label 0개 " + ok.Count(c => c.Labels.Count == 0) + " / 1개 " + ok.Count(c => c.Labels.Count == 1) + " / 2개 이상 " + ok.Count(c => c.Labels.Count > 1)
            + " / 회전·대칭 방향 " + string.Join(",", ok.GroupBy(c => c.Transform).OrderBy(g => g.Key).Select(g => "t" + g.Key + "=" + g.Count()).ToArray());
    }

    /// <summary>일치가 아닌 덩어리의 구성 종류 상위 n 개.</summary>
    public static string[] OtherGroups(int top)
    {
        return Components.Where(c => c.Transform < 0).GroupBy(c => c.Class + " | 도형 " + c.Members.Count + " | " + c.Histogram)
            .OrderByDescending(g => g.Count()).Take(top)
            .Select(g => g.Count() + "개 | " + g.Key + " | 예: " + (g.First().Labels.Count > 0 ? g.First().Labels[0] : "(Label 없음)")
                + " (" + F(g.First().Bounds.MinX) + ", " + F(g.First().Bounds.MinY) + ")").ToArray();
    }

    /// <summary>
    /// 제품 추적기(ChainCandidateTracer, Output 없음 / Layer 규칙 끔)로 다시 찾아 덩어리와 같은지 본다.
    /// Input = Label 이 들어 있는 도형. 덩어리 외곽 + margin 안의 도형만 넘긴다(그 밖 도형은 닿을 수 없다).
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string VerifyWithTracer(int samples, double margin, double cellSize)
    {
        var rnd = new Random(20261020);
        var pool = Labels.Where(l => l.Root >= 0).ToList();
        var byRoot = Components.ToDictionary(c => c.Root);
        int same = 0, diff = 0;
        string firstDiff = null;
        var tracer = new ChainCandidateTracer();
        for (int s = 0; s < Math.Min(samples, pool.Count); s++)
        {
            Label l = pool[rnd.Next(pool.Count)];
            Component comp = byRoot[l.Root];
            var area = new GBox(comp.Bounds.MinX - margin, comp.Bounds.MinY - margin, comp.Bounds.MaxX + margin, comp.Bounds.MaxY + margin);
            var local = new HashSet<int>();
            for (long cx = Cell(area.MinX); cx <= Cell(area.MaxX); cx++)
                for (long cy = Cell(area.MinY); cy <= Cell(area.MaxY); cy++)
                {
                    List<int> list;
                    if (Grid.TryGetValue(CellKey(cx, cy), out list)) foreach (int i in list) local.Add(i);
                }
            var p = new GPoint(l.X, l.Y);
            int input = comp.Members.First(i => ChainGeometryOverlap.ContainsPoint(p, Elements[i].WorldPoints,
                string.Equals(Elements[i].ElementType, "PATH", StringComparison.OrdinalIgnoreCase), Elements[i].PathWidth, 0));
            var request = new ChainTraceRequest(local.Select(i => Elements[i]), new[] { Elements[input].ElementKey }, new string[0],
                new ChainLayerConnectionRule[0], cellSize) { ApplyLayerRules = false };
            ChainTraceResult r = tracer.Trace(request);
            var traced = new HashSet<string>(r.VisitedElements.Select(e => e.ElementKey), StringComparer.Ordinal);
            var expect = new HashSet<string>(comp.Members.Select(i => Elements[i].ElementKey), StringComparer.Ordinal);
            if (traced.SetEquals(expect)) same++;
            else { diff++; if (firstDiff == null) firstDiff = l.Text + " 추적 " + traced.Count + " / 덩어리 " + expect.Count; }
        }
        return "같음 " + same + " / 다름 " + diff + (firstDiff == null ? "" : " (예: " + firstDiff + ")");
    }

    /// <summary>
    /// 화면 기능이 쓰는 제품 클래스(ChainComponentExtractor / ChainShapeTemplate / ChainNetNameRule)로 다시 계산해
    /// 위 독립 계산과 같은지 본다. 반환: 결과 문자열(덩어리 / 분류 / 구성원 다름 / 예시 덩어리 / 시간).
    /// </summary>
    public static string ProductCheck(string templateLabel)
    {
        var sw = Stopwatch.StartNew();
        var extractor = new ChainComponentExtractor(Elements, ChainComponentExtractor.DefaultCellSize);
        IList<ChainComponent> comps = extractor.ExtractAll(null, System.Threading.CancellationToken.None);
        var rule = new ChainNetNameRule();
        extractor.AttachLabels(comps, Labels.Select(l => new ChainComponentLabel(l.Text, rule.NumberLabelLayerKey, new GPoint(l.X, l.Y))));
        ChainComponent example = comps.FirstOrDefault(c => rule.CodeOf(c) == templateLabel);
        if (example == null) return "제품 클래스: 예시 덩어리 없음";
        var template = new ChainShapeTemplate(example.Elements);
        var counts = new Dictionary<string, int>();
        foreach (ChainComponent c in comps)
        {
            int t;
            ChainShapeMatch m = template.Compare(c.Elements, out t);
            string key = ChainShapeTemplate.Text(m) + (rule.NumberLabelCount(c) != 1 ? "(Label " + rule.NumberLabelCount(c) + ")" : "");
            int n; counts.TryGetValue(key, out n); counts[key] = n + 1;
        }
        // 구성원 비교: 독립 계산의 덩어리(Root)와 도형 집합이 같은지
        var probeRoot = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (Component c in Components) foreach (int i in c.Members) probeRoot[Elements[i].ElementKey] = c.Root;
        int diff = 0;
        foreach (ChainComponent c in comps)
        {
            var roots = new HashSet<int>(c.Elements.Select(e => probeRoot[e.ElementKey]));
            if (roots.Count != 1) { diff++; continue; }
            Component pc = Components.First(x => x.Root == roots.First());
            if (pc.Members.Count != c.Elements.Count) diff++;
        }
        // 예시 지정과 같은 경로: 예시 Label 도형에서 ExtractFrom
        Label tl = Labels.First(l => l.Text == templateLabel);
        ChainTraceElement start = extractor.FindElementAt(new GPoint(tl.X, tl.Y));
        ChainComponent from = start == null ? null : extractor.ExtractFrom(start.ElementKey);
        bool sameFrom = from != null && new HashSet<string>(from.Elements.Select(e => e.ElementKey)).SetEquals(example.Elements.Select(e => e.ElementKey));
        return "덩어리 " + comps.Count + " / " + string.Join(" / ", counts.OrderByDescending(p => p.Value).Select(p => p.Key + " " + p.Value).ToArray())
            + " / 독립 계산과 구성원 다름 " + diff + " / 예시 지정(ExtractFrom) 같음 " + sameFrom + " (" + (from == null ? 0 : from.Elements.Count) + "개)"
            + " / 이름 " + rule.NameOf(example) + " / " + sw.ElapsedMilliseconds + " ms";
    }

    /// <summary>덩어리 전체를 CSV 로 쓴다.</summary>
    public static int WriteCsv(string path)
    {
        var sb = new StringBuilder();
        sb.AppendLine("COMPONENT,CLASS,TRANSFORM,MEMBERS,LABELS,MIN_X,MIN_Y,MAX_X,MAX_Y,HISTOGRAM");
        int n = 0;
        foreach (Component c in Components.OrderBy(c => c.Bounds.MinY).ThenBy(c => c.Bounds.MinX))
        {
            sb.AppendLine(string.Join(",", new[] { (n++).ToString(CultureInfo.InvariantCulture), c.Class, c.Transform.ToString(CultureInfo.InvariantCulture),
                c.Members.Count.ToString(CultureInfo.InvariantCulture), "\"" + string.Join(" ", c.Labels.ToArray()) + "\"",
                F(c.Bounds.MinX), F(c.Bounds.MinY), F(c.Bounds.MaxX), F(c.Bounds.MaxY), "\"" + c.Histogram + "\"" }));
        }
        System.IO.File.WriteAllText(path, sb.ToString(), new UTF8Encoding(true));
        return n;
    }
}
'@
Add-Type -TypeDefinition $helper -ReferencedAssemblies @("$probeDir\NexplantQMS.GdsMap.dll", "$BinDir\DACrux.SEMDMS.BSL.dll", "$BinDir\DACrux.SEMDMS.Interface.dll",
    "$serverBin\Miracom.Middleware.dll", 'System.Data', 'System.Xml', 'System.Core')

$total = [Diagnostics.Stopwatch]::StartNew()
$layerKeys = [string[]]@($Layers.Split(',') | ForEach-Object { $_.Trim() })
Write-Host "대상: $Factory / $Device / 탐색 Layer $($layerKeys -join ', ') / Label $LabelLayer '$LabelPrefix*' / 예시 $TemplateLabel"
[ChainAutoExtract]::SizeStep = $SizeStep
Write-Host ("읽기: 도형 " + [ChainAutoExtract]::Load($Factory, $Device, $layerKeys, $LabelLayer, $LabelPrefix))
$sw = [Diagnostics.Stopwatch]::StartNew()
Write-Host ("연결(검사 쌍 / 묶음 / 덩어리): " + [ChainAutoExtract]::Connect(100))
Write-Host ("Label 붙이기(붙음 / 도형 밖): " + [ChainAutoExtract]::AttachLabels())
Write-Host ("분류: " + [ChainAutoExtract]::Classify($TemplateLabel, 0.5))
$sw.Stop()
Write-Host ([ChainAutoExtract]::Log.ToString().TrimEnd())
Write-Host ("일치 덩어리 Label: " + [ChainAutoExtract]::LabelStats())
Write-Host "일치가 아닌 구성 상위 10:"
foreach ($g in [ChainAutoExtract]::OtherGroups(10)) { Write-Host "  $g" }
Write-Host ("계산 시간(연결 + Label + 분류): {0:N1} 초" -f $sw.Elapsed.TotalSeconds)
Write-Host ("제품 클래스 재계산: " + [ChainAutoExtract]::ProductCheck($TemplateLabel))
if ($VerifySamples -gt 0) { Write-Host ("제품 추적기 비교(무작위 Label $VerifySamples 개): " + [ChainAutoExtract]::VerifyWithTracer($VerifySamples, 200, 10)) }
$csv = Join-Path $OutDir ("{0}_{1}_components.csv" -f $Factory, $Device)
Write-Host ("CSV: $csv (" + [ChainAutoExtract]::WriteCsv($csv) + " 행)")
Write-Host ("전체 시간: {0:N1} 초" -f $total.Elapsed.TotalSeconds)
