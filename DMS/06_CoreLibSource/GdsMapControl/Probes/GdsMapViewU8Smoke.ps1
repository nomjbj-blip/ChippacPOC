# GDS Map View U8 화면 Smoke (2026-10-20)
# 목적: 설계(문서/설계/02_GDS/2026-10-20_GDS_Map_View_U8_화면_설계.md 8절 검사)대로 화면이 동작하는지 본다.
#       1) U9 Chain 연결 기능(Input / Output / 후보 경로 / 도형 추가·제외 / 유사 묶음 / Map 복사·삭제)이 없음
#       2) Device 선택 -> Layer 목록 -> [조회] -> 도형 색인
#       3) 예시 지정(TSV 번호 Label 위치 클릭) -> 예시 덩어리 / 이름
#       4) 같은 형태 찾기 -> 덩어리 / 형태 일치 수가 Probe(GdsChainAutoExtractProbe)와 같음
#       5) 찾기 결과 거르기 + [목록에서 빼기] -> 지금 목록만 생성 대상 -> Chain 생성(DB) -> DB 형식(Code / 이름 / Layer / INPUT Marker / READY)
#          Net 목록 / 범위 선택 없음, [Chain 생성(DB)] 는 찾기 결과 탭 (2026-10-20)
#       6) 저장된 Chain 목록 선택 표시 -> 삭제 -> 목록 원래대로 / 단독 실행이면 DB 기능 숨김
#       7) 저장된 Chain [색 변경]: 고른 Chain 만 DB 색 / 목록 '색' 칸이 바뀌고 포함 도형 / 다른 Chain 은 그대로 (2026-10-21)
#       DB 는 로컬(DMSMGR)만 쓴다. 서버 config 가 다른 DB 를 가리키면 접속 ID 를 바꾼 TEMP 사본을 쓰고, 그래도 아니면 멈춘다.
#          '표시' 체크 칸 / [전체 미체크] / [전체 체크]: 체크한 Chain 은 고르지 않아도 지도에 그림, 다시 읽어도 체크 유지,
#          많이 체크했을 때 범위 조회로 읽은 포함 도형 = Chain 단위 조회 (2026-10-21)
#       7) 한 번의 생성(같은 Rule) = 한 색: 3개를 한 번에 / 2개를 한 번에 만들면 각 생성 안은 같은 색, 두 생성은 다른 색 (2026-10-21)
#       서비스는 원격 호출 대신 같은 프로세스 BSL 을 RO 에 넣는다. 화면은 화면 밖에 띄우고 Application.Run 메시지 루프 안에서 진행한다.
# 데이터: Factory SCK 의 U8 Map(읽기만)을 U8_UI_PROBE 로 복사해 그 Device 에서만 Chain 을 만들고 지운다. 끝나면 복사본을 지운다.
# 실행: powershell -NoProfile -STA -ExecutionPolicy Bypass -File GdsMapViewU8Smoke.ps1 -SrvDir <BSL/DSL/Interface/RO 폴더> -GdsDir <GdsMap EXE 폴더>
#       [-Factory SCK] [-Source U8] [-TemplateLabel TSV_21063] [-ExpectComponents 27413] [-ExpectShapeMatches 27341]
param(
    [Parameter(Mandatory = $true)][string]$SrvDir,
    [Parameter(Mandatory = $true)][string]$GdsDir,
    [string]$Factory = 'SCK',
    [string]$Source = 'U8',
    [string]$TemplateLabel = 'TSV_21063',
    [int]$ExpectComponents = 27413,
    [int]$ExpectShapeMatches = 27341,
    [string]$ConnectId = 'DMSMGR'
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$serverBin = Join-Path $dms '101_ServerBin'
# 서버 config(101_ServerBin)는 배포에 따라 고객 DB 를 가리킬 수 있다(DMS_CONNECT_ID). 접속 ID 를 바꾼 TEMP 사본을 쓰고,
# 로컬 DB(DMSMGR)가 아니면 실행하지 않는다 (2026-10-21).
$configDir = Join-Path $env:TEMP 'chippac\gdsmapviewu8smoke'
New-Item -ItemType Directory -Force $configDir | Out-Null
$configCopy = Join-Path $configDir 'DACrux.SEMDMS.Service.exe.config'
[xml]$cfg = [IO.File]::ReadAllText((Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config'))
foreach ($key in 'DMS_CONNECT_ID', 'TEST_CONNECT_ID', 'QMS_CONNECT_ID') {
    $node = $cfg.SelectSingleNode("//appSettings/add[@key='$key']")
    if ($node -ne $null) { $node.SetAttribute('value', $ConnectId) }
}
$cfg.Save($configCopy)
[xml]$check = [IO.File]::ReadAllText($configCopy)
$dbId = $check.SelectSingleNode("//appSettings/add[@key='DMS_CONNECT_ID']").GetAttribute('value')
if ($dbId -ne 'DMSMGR') { throw "로컬 DB(DMSMGR)가 아닌 접속($dbId)으로는 이 Smoke 를 실행하지 않습니다(시험 Map 복사 / Chain 생성 / 삭제를 한다)." }
Write-Host "DB 접속 ID: $dbId (TEMP config 사본 $configCopy)"
[AppDomain]::CurrentDomain.SetData('APP_CONFIG_FILE', $configCopy)
Add-Type -AssemblyName System.Configuration, System.Windows.Forms, System.Drawing
$flags = [Reflection.BindingFlags]'NonPublic,Static'
$cm = [Configuration.ConfigurationManager]
$cm.GetField('s_initState', $flags).SetValue($null, 0)
$cm.GetField('s_configSystem', $flags).SetValue($null, $null)

# 시험용 임시 폴더: GdsMap EXE 는 .dll 이름으로 복사해 참조한다(Add-Type 은 .exe 참조 불가).
$probeDir = Join-Path $env:TEMP 'chippac\gdsmapviewu8smoke'
New-Item -ItemType Directory -Force $probeDir | Out-Null
Copy-Item (Join-Path $GdsDir 'NexplantQMS.GdsMap.dll') (Join-Path $probeDir 'NexplantQMS.GdsMap.dll') -Force
$searchDirs = @($SrvDir, $GdsDir, $probeDir, $serverBin)
# 어셈블리 찾기는 C# 처리기로 한다. 화면 / 백그라운드 스레드에는 PowerShell Runspace 가 없어 스크립트 블록 처리기가 동작하지 않는다.
Add-Type -TypeDefinition @"
public static class U8SmokeResolver
{
    static string[] dirs;
    public static void Install(string[] d) { dirs = d; System.AppDomain.CurrentDomain.AssemblyResolve += Resolve; }
    static System.Reflection.Assembly Resolve(object sender, System.ResolveEventArgs e)
    {
        string name = new System.Reflection.AssemblyName(e.Name).Name;
        if (name.EndsWith(".resources")) return null;
        foreach (string d in dirs)
            foreach (string ext in new[] { ".dll", ".exe" })
            {
                string p = System.IO.Path.Combine(d, name + ext);
                if (System.IO.File.Exists(p)) return System.Reflection.Assembly.LoadFrom(p);
            }
        return null;
    }
}
"@
[U8SmokeResolver]::Install([string[]]$searchDirs)
foreach ($p in @("$serverBin\Oracle.ManagedDataAccess.dll", "$serverBin\Miracom.Middleware.dll", "$SrvDir\DACrux.Base.dll", "$SrvDir\DACrux.SEMDMS.Interface.dll",
        "$SrvDir\DACrux.SEMDMS.DSL.dll", "$SrvDir\DACrux.SEMDMS.BSL.dll", "$SrvDir\DACrux.SEMDMS.RO.dll", "$GdsDir\OpenTK.dll", "$GdsDir\OpenTK.GLControl.dll",
        "$probeDir\NexplantQMS.GdsMap.dll")) { [void][Reflection.Assembly]::LoadFrom($p) }

$helper = @"
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NexplantQMS.GdsMap;
using NexplantQMS.GdsMap.Persistence;

public static class U8Smoke
{
    public static string F, Source, TemplateLabel;
    public static int ExpectComponents, ExpectShapeMatches;
    public const string Target = "U8_UI_PROBE";
    const BindingFlags NP = BindingFlags.NonPublic | BindingFlags.Instance;
    const BindingFlags PB = BindingFlags.Public | BindingFlags.Instance;

    static T Ro<T>(object bsl)
    {
        object ro = FormatterServices.GetUninitializedObject(typeof(T));
        typeof(T).GetField("m_OBJ", NP).SetValue(ro, bsl);
        return (T)ro;
    }

    static object Field(object o, string name) { FieldInfo f = o.GetType().GetField(name, NP); return f == null ? null : f.GetValue(o); }
    static object Call(object o, string name, params object[] args) { return o.GetType().GetMethod(name, NP).Invoke(o, args); }
    static object Member(object o, string name)
    {
        FieldInfo f = o.GetType().GetField(name, PB | BindingFlags.NonPublic);
        if (f != null) return f.GetValue(o);
        return o.GetType().GetProperty(name, PB | BindingFlags.NonPublic).GetValue(o, null);
    }

    static async Task<bool> WaitAsync(Func<bool> done, int seconds)
    {
        var until = DateTime.Now.AddSeconds(seconds);
        while (!done())
        {
            if (DateTime.Now > until) return false;
            await Task.Delay(50);
        }
        return true;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Cleanup()
    {
        var imp = new DACrux.SEMDMS.BSL.GdsMapImport();
        if (imp.GetMapInfo(F, Target).Rows.Count > 0) imp.DeleteMap(F, Target, "probe");
    }

    /// <summary>원본 Map 을 시험 Device 로 복사한다. 반환 "도형|Chain|ms"</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string CopySource()
    {
        var sw = Stopwatch.StartNew();
        DataTable t = new DACrux.SEMDMS.BSL.GdsMapImport().CopyMap(F, Source, Target, "probe");
        DataRow r = t.Rows[0];
        return (t.Columns.Contains("PLACED_COUNT") ? r["PLACED_COUNT"] : "?") + "|" + (t.Columns.Contains("CHAIN_COUNT") ? r["CHAIN_COUNT"] : "?") + "|" + sw.ElapsedMilliseconds;
    }

    /// <summary>
    /// 복사본(시험 Device)에 원본에서 따라온 Chain 을 지운다. 원본 U8 에 이미 Chain 이 있으면 생성 대상이 0 이 되어 시험할 수 없다 (2026-10-21).
    /// 시험 Device 만 지운다. 반환: 지운 Chain 수
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static int ClearTargetChains()
    {
        if (Target == Source || !Target.EndsWith("_PROBE", StringComparison.Ordinal)) throw new InvalidOperationException("시험 Device 가 아닙니다: " + Target);
        var chain = new DACrux.SEMDMS.BSL.GdsChain();
        var seqs = chain.GetChainCodeList(F, Target).Rows.Cast<DataRow>().Select(row => row["CHAIN_SEQ"].ToString()).ToList();
        int deleted = 0;
        for (int i = 0; i < seqs.Count; i += 1000)
            deleted += chain.DeleteChainBulk(F, Target, seqs.Skip(i).Take(1000).ToArray(), "probe");
        return deleted;
    }

    /// <summary>DB 에서 Code 의 Chain 형식 확인. 반환 "이름|상태|Member|INPUT|규칙|격자|Layer 목록|INPUT Marker"</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string DbChain(string code)
    {
        var bsl = new DACrux.SEMDMS.BSL.GdsChain();
        DataRow row = bsl.GetChainList(F, Target).Rows.Cast<DataRow>().FirstOrDefault(r => r["CHAIN_CODE"].ToString() == code);
        if (row == null) return "NONE";
        string seq = row["CHAIN_SEQ"].ToString();
        DataTable members = bsl.GetChainMembers(F, Target, seq);
        DataRow input = members.Rows.Cast<DataRow>().FirstOrDefault(r => r["ROLE_CODE"].ToString() == "INPUT");
        string layers = string.Join(",", bsl.GetChainLayers(F, Target, seq).Rows.Cast<DataRow>()
            .Select(r => r["LAYER_ID"] + "/" + r["DATA_TYPE"]).OrderBy(x => x).ToArray());
        return row["CHAIN_NAME"] + "|" + row["VERSION_STATUS"] + "|" + row["MEMBER_COUNT"] + "|"
            + members.Rows.Cast<DataRow>().Count(r => r["ROLE_CODE"].ToString() == "INPUT") + "|" + row["APPLY_LAYER_RULES"] + "|"
            + Convert.ToDecimal(row["CELL_SIZE"], CultureInfo.InvariantCulture).ToString("0.###", CultureInfo.InvariantCulture) + "|" + layers + "|"
            + (input == null ? "-" : input["INCLUSION_SOURCE"] + "@" + Convert.ToDouble(input["MARKER_X"], CultureInfo.InvariantCulture).ToString("0.###", CultureInfo.InvariantCulture)
                + "," + Convert.ToDouble(input["MARKER_Y"], CultureInfo.InvariantCulture).ToString("0.###", CultureInfo.InvariantCulture));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static int DbChainCount()
    {
        return new DACrux.SEMDMS.BSL.GdsChain().GetChainCodeList(F, Target).Rows.Count;
    }

    /// <summary>DB 에 저장된 Chain 색(Code -> COLOR_ARGB).</summary>
    public static Dictionary<string, string> DbColors()
    {
        var map = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (DataRow row in new DACrux.SEMDMS.BSL.GdsChain().GetChainCodeList(F, Target).Rows)
            map[row["CHAIN_CODE"].ToString()] = row["COLOR_ARGB"].ToString();
        return map;
    }

    /// <summary>화면 시험을 새 STA 스레드의 Application.Run 안에서 진행한다(await 뒤 처리가 화면 스레드에서 돌게).</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static List<string[]> Run()
    {
        List<string[]> result = new List<string[]>();
        Exception error = null;
        var thread = new Thread(() =>
        {
            DACrux.Base.GlobalVariable.Factory = F;
            using (var form = new GdsMapViewU8Form(true))
            {
                form.MapViewService = Ro<DACrux.SEMDMS.RO.GdsMapView>(new DACrux.SEMDMS.BSL.GdsMapView());
                form.ChainService = Ro<DACrux.SEMDMS.RO.GdsChain>(new DACrux.SEMDMS.BSL.GdsChain());
                form.StartPosition = FormStartPosition.Manual;
                form.Location = new Point(-32000, -32000);
                form.Size = new Size(1600, 1000);
                form.Shown += async (s, e) =>
                {
                    try { await Steps(form, result); }
                    catch (Exception ex) { error = ex; }
                    form.Close();
                };
                Application.Run(form);
            }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();
        if (error != null) result.Add(new[] { "FAIL", "실행 오류", error.ToString() });
        return result;
    }

    static void Add(List<string[]> r, string name, bool ok, string detail) { r.Add(new[] { ok ? "PASS" : "FAIL", name, detail }); }

    static IEnumerable<Control> All(Control root)
    {
        foreach (Control c in root.Controls) { yield return c; foreach (Control x in All(c)) yield return x; }
    }

    static async Task Steps(GdsMapViewU8Form form, List<string[]> r)
    {
        // 1) U9 기능 없음
        string[] u9Fields = { "_btnChainInput", "_btnChainOutput", "_btnChainTrace", "_btnChainAdd", "_btnChainRemove", "btnCopyMap", "mnuDeleteMap", "_chainSimilarList" };
        string[] u9Texts = { "Input 지정", "Output 지정", "후보 경로", "도형 추가", "도형 제외", "유사 묶음", "Map 복사", "Map 삭제", "셀 크기", "Net 목록" };
        var texts = All(form).Select(c => c.Text ?? "").ToList();
        var foundFields = u9Fields.Where(n => form.GetType().GetField(n, NP) != null).ToList();
        var foundTexts = u9Texts.Where(t => texts.Any(x => x.Contains(t))).ToList();
        Add(r, "U9 Chain 연결 기능 없음(필드 / 버튼 글자)", foundFields.Count == 0 && foundTexts.Count == 0,
            "필드 " + string.Join(",", foundFields.ToArray()) + " / 글자 " + string.Join(",", foundTexts.ToArray()));
        var u8Buttons = new[] { "btnTemplate", "btnFind", "btnFindStop", "btnResultRemove", "btnCreate", "btnSavedRefresh", "btnSavedDelete", "btnSavedDeleteShown" };
        Add(r, "U8 작업 버튼 있음", u8Buttons.All(n => Field(form, n) is Button), string.Join(" / ", u8Buttons.Select(n => ((Button)Field(form, n)).Text).ToArray()));
        var resultTop = (Control)Field(form, "pnlResultTop");
        bool noNetList = new[] { "btnNetList", "cboScope", "lblScope" }.All(n => form.GetType().GetField(n, NP) == null);
        Add(r, "Net 목록 / 범위 없음, [목록에서 빼기] / [Chain 생성(DB)] = 찾기 결과 탭", noNetList
            && ((Control)Field(form, "btnCreate")).Parent == resultTop && ((Control)Field(form, "btnResultRemove")).Parent == resultTop, "");
        Add(r, "도면 전: U8 작업 줄 숨김", !((Control)Field(form, "pnlU8Bar")).Visible, "");

        // 2) Device 선택 -> Map 정보 / Layer 목록
        var combo = (ComboBox)Field(form, "cboDevice");
        bool listed = await WaitAsync(() => Field(form, "_deviceTable") != null && combo.Items.Count > 0, 60);
        int index = combo.Items.IndexOf(Target);
        Add(r, "Device 목록에 시험 Device", listed && index >= 0, "목록 " + combo.Items.Count + "개");
        if (index < 0) return;
        combo.SelectedIndex = index;
        bool mapReady = await WaitAsync(() => { object m = Field(form, "_currentMap"); return m != null && (string)Member(m, "DeviceId") == Target; }, 60);
        var layers = (CheckedListBox)Field(form, "chkLayerItems");
        string layerText = string.Join(", ", layers.Items.Cast<LayerDisplayItem>().Select(i => GdsLayerKey.Format(i.LayerKey)).ToArray());
        Add(r, "Map 정보 / Layer 목록(모두 체크)", mapReady && layers.Items.Count > 0 && layers.CheckedItems.Count == layers.Items.Count, layerText);

        // 3) 조회
        var sw = Stopwatch.StartNew();
        Call(form, "btnSearch_Click", null, EventArgs.Empty);
        bool loaded = await WaitAsync(() => Field(form, "_shownMap") != null && !(bool)Field(form, "_loading"), 600);
        long loadMs = sw.ElapsedMilliseconds;
        var index2 = (IDictionary)Field(form, "_elementByKey");
        var shown = (ICollection)Field(form, "_shownItems");
        var labelIndex = (GdsLabelIndex)Field(form, "_labelIndex");
        Add(r, "[조회] 도면 / 도형 색인 / Label 색인 / 작업 줄 보임", loaded && index2.Count > 0 && labelIndex != null && ((Control)Field(form, "pnlU8Bar")).Visible,
            "도형 " + (shown == null ? 0 : shown.Count).ToString("N0") + " / 색인(TEXT 제외) " + index2.Count.ToString("N0") + " / Label " + (labelIndex == null ? 0 : labelIndex.Count).ToString("N0")
            + " / " + loadMs.ToString("N0") + " ms / 상태 '" + ((ToolStripStatusLabel)Field(form, "lblStatus")).Text + "'");
        if (!loaded || labelIndex == null) return;

        // 4) 예시 지정: 예시 Label 위치를 지도 가운데로 옮기고 가운데를 클릭한 것과 같게 처리
        GdsLabelEntry entry = labelIndex.Entries.FirstOrDefault(e => e.Text == TemplateLabel);
        Add(r, "예시 Label 있음", entry != null, TemplateLabel);
        if (entry == null) return;
        var map = (GdsMapControl)Field(form, "map");
        double tx = (double)Call(form, "ToDisplayX", entry.StoredX), ty = entry.Y;
        map.FocusChainBounds(new GBox(tx - 5, ty - 5, tx + 5, ty + 5));
        var center = new Point(map.ClientSize.Width / 2, map.ClientSize.Height / 2);
        GPoint world = map.GetWorldPosition(center);
        Call(form, "SetTemplateSelecting", true);
        sw.Restart();
        Call(form, "SelectTemplateAt", center);
        bool templated = await WaitAsync(() => Field(form, "_template") != null, 120);
        string templateText = ((Label)Field(form, "lblTemplate")).Text;
        Add(r, "예시 지정 -> 예시 덩어리 / 이름", templated && templateText == "예시: " + TemplateLabel + " / 도형 10" && !(bool)Field(form, "_templateSelecting"),
            templateText + " / 클릭 위치 오차 " + Math.Abs(world.X - tx).ToString("0.###") + "," + Math.Abs(world.Y - ty).ToString("0.###") + " µm / " + sw.ElapsedMilliseconds + " ms / 예시 Layer "
            + string.Join(",", ((IEnumerable<int>)Field(form, "_templateLayers")).Select(GdsLayerKey.Format).ToArray()));
        if (!templated) return;

        // 5) 같은 형태 찾기
        sw.Restart();
        Call(form, "btnFind_Click", null, EventArgs.Empty);
        await Task.Delay(100);
        bool found = await WaitAsync(() => !(bool)Field(form, "_extractRunning") && ((ICollection)Field(form, "_results")).Count > 0, 600);
        long findMs = sw.ElapsedMilliseconds;
        var results = ((IEnumerable)Field(form, "_results")).Cast<object>().ToList();
        int shapeMatch = results.Count(o => { string m = Member(o, "Match").ToString(); return m == "Match" || m == "MatchTransformed"; });
        int isMatch = results.Count(o => (bool)Member(o, "IsMatch"));
        Add(r, "같은 형태 찾기: 덩어리 / 형태 일치 = Probe", found && results.Count == ExpectComponents && shapeMatch == ExpectShapeMatches,
            "덩어리 " + results.Count.ToString("N0") + " / 형태 일치 " + shapeMatch.ToString("N0") + " / 일치(Label 1개) " + isMatch.ToString("N0")
            + " / " + findMs.ToString("N0") + " ms / 요약 '" + ((Label)Field(form, "lblResultSummary")).Text.Replace("\r\n", " | ") + "'");
        var grid = (DataGridView)Field(form, "dgvResult");
        Add(r, "찾기 결과 목록(가상) 행 수", grid.RowCount == results.Count, grid.RowCount.ToString("N0"));
        var classCombo = (ComboBox)Field(form, "cboResultClass");
        classCombo.SelectedIndex = 2;
        int review = grid.RowCount;
        string reviewSample = review > 0 ? (string)grid.Rows[0].Cells[1].Value : "";
        classCombo.SelectedIndex = 0;
        Add(r, "분류 '검토' 거르기", review == results.Count - isMatch, review.ToString("N0") + "행 / 첫 행 " + reviewSample);

        // 6) 거르기 + [목록에서 빼기]: 생성 대상 = 지금 목록의 일치 + DB 없음
        var create = (Button)Field(form, "btnCreate");
        var removeButton = (Button)Field(form, "btnResultRemove");
        var findText = (TextBox)Field(form, "txtResultFind");
        int allTargets = (int)Call(form, "CountCreateTargets");
        Add(r, "거르기 없음: 생성 대상 = 일치 수(DB 없음) / [Chain 생성(DB)] 켜짐",
            allTargets == results.Count(o => (bool)Member(o, "IsMatch") && !(bool)Member(o, "InDb")) && create.Enabled, allTargets.ToString("N0"));
        findText.Text = "TSV_2162";
        int filtered = grid.RowCount;
        int filteredTargets = (int)Call(form, "CountCreateTargets");
        var view = ((IEnumerable)Field(form, "_resultView")).Cast<object>().ToList();
        grid.ClearSelection();
        for (int i = 0; i < view.Count; i++)
            if ((string)Member(view[i], "Code") != "TSV_2162") grid.Rows[i].Selected = true;
        int selectedCount = grid.SelectedRows.Count;
        bool removeEnabled = removeButton.Enabled;
        int removedMatches = view.Count(o => (string)Member(o, "Code") != "TSV_2162" && (bool)Member(o, "IsMatch") && !(bool)Member(o, "InDb"));
        Call(form, "btnResultRemove_Click", null, EventArgs.Empty);
        var targets = ((IEnumerable)Call(form, "CreateTargetsInView")).Cast<object>().ToList();
        int removed = (int)Field(form, "_removedCount");
        int remain = ((ICollection)Field(form, "_results")).Count;
        string summary = ((Label)Field(form, "lblResultSummary")).Text.Replace("\r\n", " | ");
        Add(r, "찾기 'TSV_2162' -> 1행만 남기고 [목록에서 빼기] -> 목록 1행 / 생성 대상 1",
            filtered > 1 && filteredTargets == filtered && removeEnabled && selectedCount == filtered - 1 && removed == filtered - 1
            && remain == results.Count - removed && grid.RowCount == 1 && targets.Count == 1 && (string)Member(targets[0], "Code") == "TSV_2162" && summary.Contains("뺀 행 " + removed),
            "거른 " + filtered + "행(대상 " + filteredTargets + ") / 뺀 " + removed + " / 남은 결과 " + remain.ToString("N0") + " / 요약 '" + summary + "'");

        // 7) Chain 생성(DB): 확인 창 없이 저장 부분만 호출(대상 = 지금 목록)
        int before = DbChainCount();
        Type resultType = form.GetType().GetNestedType("U8Result", BindingFlags.NonPublic);
        var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(resultType));
        foreach (object o in targets) list.Add(o);
        sw.Restart();
        string done = await (Task<string>)Call(form, "CreateChainsAsync", list);
        Add(r, "Chain 생성(DB) 결과", done == "Chain 생성 완료: 생성 1" && DbChainCount() == before + 1,
            (done ?? "null") + " / DB Chain " + before + " -> " + DbChainCount() + " / " + sw.ElapsedMilliseconds + " ms");
        GdsLabelEntry label2162 = labelIndex.Entries.First(e => e.Text == "TSV_2162");
        string db = DbChain("TSV_2162");
        string expect = "TSV_2162|READY|10|1|0|10|850/0,860/0,900/0|ENDPOINT@" + label2162.StoredX.ToString("0.###", CultureInfo.InvariantCulture) + "," + label2162.Y.ToString("0.###", CultureInfo.InvariantCulture);
        Add(r, "DB 형식: 이름(Net Label 없음 -> Code) / READY / 도형 10 / INPUT 1 / 규칙 0 / 격자 10 / Layer / INPUT Marker = Label 위치", db == expect, db + " (기대 " + expect + ")");
        Add(r, "생성 뒤: 행 DB '생성' / 지금 목록 생성 대상 0 / [Chain 생성(DB)] 꺼짐",
            (bool)Member(targets[0], "InDb") && (bool)Member(targets[0], "Created") && (string)grid.Rows[0].Cells[2].Value == "생성"
            && (int)Call(form, "CountCreateTargets") == 0 && !create.Enabled, "");
        findText.Text = "";
        int allAfter = (int)Call(form, "CountCreateTargets");
        Add(r, "거르기 해제 -> 생성 대상 = 처음 - 뺀 일치 - 생성 1", allAfter == allTargets - removedMatches - 1 && create.Enabled,
            allTargets.ToString("N0") + " - " + removedMatches + " - 1 = " + allAfter.ToString("N0"));

        // 8) 저장된 Chain 목록: 찾기 -> 선택 -> 지도 표시
        var savedFind = (TextBox)Field(form, "txtSavedFind");
        var saved = (DataGridView)Field(form, "dgvSaved");
        var statusLabel = (ToolStripStatusLabel)Field(form, "lblStatus");
        ((TabControl)Field(form, "tabRight")).SelectedTab = (TabPage)Field(form, "tabSaved");
        savedFind.Text = "TSV_2162";
        bool one = saved.RowCount == 1;
        if (one)
        {
            saved.ClearSelection();
            saved.Rows[0].Selected = true;
            await WaitAsync(() => statusLabel.Text.StartsWith("TSV_2162 /"), 30);
        }
        string status = statusLabel.Text;
        Add(r, "저장된 Chain 찾기 / 선택 표시(이름 = Code 면 한 번만)", one && status == "TSV_2162 / 도형 10", "행 " + saved.RowCount + " / 상태 '" + status + "'");

        // 8-1) '표시' 체크 / [전체 미체크] / [전체 체크] (2026-10-21): 체크한 Chain 은 고르지 않아도 지도에 그린다. 찾기 'TSV_2162' 로 목록 1행인 상태.
        var mapControl = (GdsMapControl)Field(form, "map");
        Func<int> overlayCount = () => ((ICollection)Field(mapControl, "_visibleChainOverlays")).Count;
        var colShow = (DataGridViewColumn)Field(form, "colSavedShow");
        var checkAll = (Button)Field(form, "btnSavedCheckAll");
        var checkNone = (Button)Field(form, "btnSavedCheckNone");
        var savedSummary = (Label)Field(form, "lblSavedSummary");
        object row2162 = ((IEnumerable)Field(form, "_savedRows")).Cast<object>().First(o => (string)Member(o, "Code") == "TSV_2162");
        Add(r, "저장된 Chain [전체 체크] / [전체 미체크] 버튼 사용 가능 / 글자 / 아이콘",
            one && checkAll.Enabled && checkNone.Enabled && checkAll.Text == "전체 체크" && checkNone.Text == "전체 미체크" && checkAll.Image != null && checkNone.Image != null,
            checkAll.Enabled + "/" + checkNone.Enabled + " " + checkAll.Text + "/" + checkNone.Text);
        Call(form, "dgvSaved_CellContentClick", saved, new DataGridViewCellEventArgs(colShow.Index, 0));
        bool shownByCheck = await WaitAsync(() => statusLabel.Text.StartsWith("지도 표시(체크) Chain 1개"), 30);
        Add(r, "'표시' 칸 클릭 -> 체크 / 칸 값 / 상태 줄 / 요약", shownByCheck && (bool)Member(row2162, "Show") && Equals(saved.Rows[0].Cells[colShow.Index].Value, true)
            && savedSummary.Text.EndsWith("지도 표시 1개"), "상태 '" + statusLabel.Text + "' / 칸 " + saved.Rows[0].Cells[colShow.Index].Value + " / 요약 '" + savedSummary.Text + "'");
        saved.ClearSelection();
        Add(r, "선택을 풀어도 체크한 Chain 은 지도에 남음", saved.SelectedRows.Count == 0 && overlayCount() == 1, "외곽선 " + overlayCount());
        checkNone.PerformClick();
        Add(r, "[전체 미체크] -> 체크 해제 / 지도 Chain 없음", !(bool)Member(row2162, "Show") && overlayCount() == 0 && statusLabel.Text == "지도에 표시한 저장된 Chain 이 없습니다.",
            "Show " + Member(row2162, "Show") + " / 외곽선 " + overlayCount() + " / 상태 '" + statusLabel.Text + "'");
        checkAll.PerformClick();
        bool allShown = await WaitAsync(() => overlayCount() == 1, 30);
        Add(r, "[전체 체크](목록 1행) -> 체크 / 지도 Chain 1개", allShown && (bool)Member(row2162, "Show") && statusLabel.Text.StartsWith("지도 표시(체크) Chain 1개"),
            "외곽선 " + overlayCount() + " / 상태 '" + statusLabel.Text + "'");
        await (Task)Call(form, "RefreshSavedChainsAsync", false);
        object again = ((IEnumerable)Field(form, "_savedRows")).Cast<object>().First(o => (string)Member(o, "Code") == "TSV_2162");
        bool reread = await WaitAsync(() => ((ICollection)Field(form, "_savedMemberKeys")).Count == 1 && overlayCount() == 1, 30);
        Add(r, "다시 읽기 뒤에도 체크 유지 / 포함 도형 다시 읽어 지도에 그림", !ReferenceEquals(again, row2162) && (bool)Member(again, "Show") && reread,
            "Show " + Member(again, "Show") + " / 읽은 Chain " + ((ICollection)Field(form, "_savedMemberKeys")).Count + " / 외곽선 " + overlayCount());

        // 8-2) 많이 체크했을 때 읽는 방식(범위 조회): 진짜 Chain 1개 + 없는 CHAIN_SEQ 20개(SavedBatchReadMin 초과)
        //      -> 진짜 Chain 은 Chain 단위 조회와 같은 도형 / 순서, 없는 Chain 은 빈 목록
        Type showType = form.GetType().GetNestedType("SavedChain", BindingFlags.NonPublic);
        var many = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(showType));
        string seq2162 = (string)Member(again, "Seq");
        many.Add(again);
        for (int i = 1; i <= 20; i++)
        {
            object fake = Activator.CreateInstance(showType, true);
            showType.GetField("Seq").SetValue(fake, (long.Parse(seq2162, CultureInfo.InvariantCulture) + 1000000 + i).ToString(CultureInfo.InvariantCulture));
            many.Add(fake);
        }
        var readMany = (IDictionary)Call(form, "ReadSavedMembers", F, Target, many);
        var chainRo = (DACrux.SEMDMS.RO.GdsChain)form.GetType().GetProperty("ChainService").GetValue(form, null);
        string[] expectKeys = chainRo.GetChainMembers(F, Target, seq2162).Rows.Cast<DataRow>().Select(row => row["PLACED_ELEMENT_ID"].ToString()).ToArray();
        string[] rangeKeys = (string[])readMany[seq2162];
        int emptyFakes = many.Cast<object>().Skip(1).Count(o => readMany.Contains(Member(o, "Seq")) && ((string[])readMany[Member(o, "Seq")]).Length == 0);
        Add(r, "많이 체크 -> 범위 조회 도형 = Chain 단위 조회 / 없는 Chain 빈 목록", readMany.Count == 21 && rangeKeys != null && rangeKeys.SequenceEqual(expectKeys) && expectKeys.Length == 10 && emptyFakes == 20,
            "결과 " + readMany.Count + " / 도형 " + (rangeKeys == null ? "null" : rangeKeys.Length.ToString()) + " = " + expectKeys.Length + " / 빈 목록 " + emptyFakes);
        checkNone.PerformClick();
        savedFind.Text = "";

        // 9) 삭제(확인 창 없이 삭제 부분만 호출) -> 목록 / DB 원래대로
        Type savedType = form.GetType().GetNestedType("SavedChain", BindingFlags.NonPublic);
        var del = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(savedType));
        foreach (object o in ((IEnumerable)Field(form, "_savedRows")).Cast<object>().Where(o => (string)Member(o, "Code") == "TSV_2162"))
            del.Add(o);
        int deleted = await (Task<int>)Call(form, "DeleteChainsAsync", del);
        int savedRows = ((ICollection)Field(form, "_savedRows")).Count;
        Add(r, "삭제 -> DB / 목록 원래대로 / 생성 대상 다시 +1", deleted == 1 && DbChainCount() == before && savedRows == before && (int)Call(form, "CountCreateTargets") == allAfter + 1,
            "삭제 " + deleted + " / DB " + DbChainCount() + " / 목록 " + savedRows);

        // 10) 한 번의 생성(같은 Rule) = 한 색 (2026-10-21): 3개를 한 번에, 이어서 2개를 한 번에 만든다.
        var pool = ((IEnumerable)Call(form, "CreateTargetsInView")).Cast<object>().Take(5).ToList();
        var run1 = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(resultType));
        var run2 = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(resultType));
        for (int i = 0; i < pool.Count; i++) (i < 3 ? run1 : run2).Add(pool[i]);
        string done1 = await (Task<string>)Call(form, "CreateChainsAsync", run1);
        string done2 = await (Task<string>)Call(form, "CreateChainsAsync", run2);
        Dictionary<string, string> colors = DbColors();
        var codes1 = run1.Cast<object>().Select(o => (string)Member(o, "Code")).ToList();
        var codes2 = run2.Cast<object>().Select(o => (string)Member(o, "Code")).ToList();
        var c1 = codes1.Select(c => colors.ContainsKey(c) ? colors[c] : "없음").Distinct().ToList();
        var c2 = codes2.Select(c => colors.ContainsKey(c) ? colors[c] : "없음").Distinct().ToList();
        Add(r, "한 번에 3개 생성 -> 3개 모두 같은 색", pool.Count == 5 && done1 == "Chain 생성 완료: 생성 3" && c1.Count == 1 && c1[0] != "없음",
            (done1 ?? "null") + " / " + string.Join(",", codes1.ToArray()) + " 색 " + string.Join(",", c1.ToArray()));
        Add(r, "다음 생성 2개 -> 2개 같은 색 / 앞 생성과 다른 색", done2 == "Chain 생성 완료: 생성 2" && c2.Count == 1 && c2[0] != "없음" && c2[0] != c1[0],
            (done2 ?? "null") + " / " + string.Join(",", codes2.ToArray()) + " 색 " + string.Join(",", c2.ToArray()));
        // 11) 저장된 Chain [색 변경] (2026-10-21): 앞 생성 3개를 골라 한 색으로 -> DB / 목록 '색' 칸 / 다른 생성 2개 / 포함 도형 그대로
        var colorButton = (Button)Field(form, "btnSavedColor");
        var colorColumn = (DataGridViewColumn)Field(form, "colSavedColor");
        var savedRowsNow = ((IEnumerable)Field(form, "_savedRows")).Cast<object>().ToList();
        var savedView = ((IEnumerable)Field(form, "_savedView")).Cast<object>().ToList();
        int viewIndex = savedView.FindIndex(o => codes1.Contains((string)Member(o, "Code")));
        saved.ClearSelection();
        bool offWithout = !colorButton.Enabled;
        if (viewIndex >= 0) saved.Rows[viewIndex].Selected = true;
        bool onWith = colorButton.Enabled;
        saved.ClearSelection();
        Add(r, "[색 변경] 버튼: 글자 / 아이콘 / '색' 칸 / 고른 행이 있을 때만 켜짐", colorButton.Text == "색 변경" && colorButton.Image != null
            && colorColumn.HeaderText == "색" && offWithout && onWith, "위치 " + colorButton.Location + " / 고르기 전 " + !offWithout + " / 고른 뒤 " + onWith);
        var colorRows = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(savedType));
        foreach (object o in savedRowsNow.Where(o => codes1.Contains((string)Member(o, "Code")))) colorRows.Add(o);
        Color newColor = Color.FromArgb(255, 18, 52, 86);
        string newArgb = newColor.ToArgb().ToString(CultureInfo.InvariantCulture);
        string memberBefore = DbChain(codes1[0]).Split('|')[2];
        int changedColor = await (Task<int>)Call(form, "ChangeSavedChainColorAsync", colorRows, newColor);
        string statusAfter = statusLabel.Text;
        Dictionary<string, string> colorsAfter = DbColors();
        bool dbChanged = codes1.All(c => colorsAfter.ContainsKey(c) && colorsAfter[c] == newArgb);
        bool othersKept = codes2.All(c => colorsAfter.ContainsKey(c) && colorsAfter[c] == c2[0]);
        bool memoryChanged = colorRows.Cast<object>().All(o => ((Color)Member(o, "Color")).ToArgb() == newColor.ToArgb());
        string[] dbAfter = DbChain(codes1[0]).Split('|');
        var cellArgs = new DataGridViewCellFormattingEventArgs(colorColumn.Index, viewIndex, null, typeof(string), new DataGridViewCellStyle());
        if (viewIndex >= 0) Call(form, "dgvSaved_CellFormatting", saved, cellArgs);
        bool cellPainted = viewIndex >= 0 && cellArgs.CellStyle.BackColor.ToArgb() == newColor.ToArgb() && "".Equals(cellArgs.Value);
        Add(r, "[색 변경] 3개 -> DB 색 / 화면 색 / '색' 칸 / 다른 생성 2개 그대로 / 포함 도형 · 상태 그대로", changedColor == 3 && dbChanged && othersKept && memoryChanged
            && cellPainted && statusAfter == "Chain 색 변경 완료: 3개" && dbAfter[2] == memberBefore && dbAfter[1] == "READY",
            "바꾼 " + changedColor + " / 새 색 " + newArgb + " / DB " + string.Join(",", codes1.Select(c => colorsAfter.ContainsKey(c) ? colorsAfter[c] : "없음").ToArray())
            + " / 다른 2개 " + string.Join(",", codes2.Select(c => colorsAfter.ContainsKey(c) ? colorsAfter[c] : "없음").ToArray())
            + " / 도형 " + memberBefore + " -> " + dbAfter[2] + " / 상태 '" + statusAfter + "'");
        var del2 = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(savedType));
        foreach (object o in ((IEnumerable)Field(form, "_savedRows")).Cast<object>().Where(o => codes1.Contains((string)Member(o, "Code")) || codes2.Contains((string)Member(o, "Code"))))
            del2.Add(o);
        int deleted2 = await (Task<int>)Call(form, "DeleteChainsAsync", del2);
        Add(r, "색 검사용 Chain 5개 삭제 -> DB 원래대로", deleted2 == 5 && DbChainCount() == before, "삭제 " + deleted2 + " / DB " + DbChainCount());
    }

    /// <summary>단독 실행(호스트 아님): DB 기능 꺼짐 / [Chain 생성(DB)] 숨김.</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string Standalone()
    {
        using (var form = new GdsMapViewU8Form())
        {
            bool enabled = (bool)Field(form, "_chainDbEnabled");
            MethodInfo state = typeof(Control).GetMethod("GetState", NP);
            bool createVisible = (bool)state.Invoke(Field(form, "btnCreate"), new object[] { 2 });
            return enabled + "|" + createVisible;
        }
    }
}
"@
Add-Type -TypeDefinition $helper -ReferencedAssemblies @("$SrvDir\DACrux.SEMDMS.BSL.dll", "$SrvDir\DACrux.SEMDMS.Interface.dll", "$SrvDir\DACrux.SEMDMS.RO.dll",
    "$SrvDir\DACrux.Base.dll", "$probeDir\NexplantQMS.GdsMap.dll", "$GdsDir\OpenTK.dll", "$GdsDir\OpenTK.GLControl.dll", "$serverBin\Miracom.Middleware.dll",
    'System.Data', 'System.Xml', 'System.Core', 'System.Windows.Forms', 'System.Drawing', 'System.Runtime.Serialization')

[U8Smoke]::F = $Factory
[U8Smoke]::Source = $Source
[U8Smoke]::TemplateLabel = $TemplateLabel
[U8Smoke]::ExpectComponents = $ExpectComponents
[U8Smoke]::ExpectShapeMatches = $ExpectShapeMatches
$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
try {
    [U8Smoke]::Cleanup()
    $c = [U8Smoke]::CopySource()
    Check "시험 Device 복사($Source -> $([U8Smoke]::Target))" $true $c
    $copied = [int]($c.Split('|')[1] -replace '[^0-9]', '')
    $cleared = [U8Smoke]::ClearTargetChains()
    Check '복사본의 기존 Chain 지움(원본은 그대로)' ($cleared -eq $copied -and [U8Smoke]::DbChainCount() -eq 0) "지움 $cleared / 복사 $copied / 남음 $([U8Smoke]::DbChainCount())"
    foreach ($row in [U8Smoke]::Run()) { Check $row[1] ($row[0] -eq 'PASS') $row[2] }
    $s = [U8Smoke]::Standalone()
    Check '단독 실행: DB 기능 꺼짐 / [Chain 생성(DB)] 숨김' ($s -eq 'False|False') $s
}
finally {
    [U8Smoke]::Cleanup()
}
$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host "GDS_MAP_VIEW_U8_SMOKE=PASS ($($results.Count))" } else { Write-Host "GDS_MAP_VIEW_U8_SMOKE=FAIL ($fail)"; exit 1 }
