# GDS Map 조회 화면 Chain 목록 / 유사 묶음 목록, Setup 화면 Layer 목록 체크 동작 Smoke (2026-10-13)
# 목적: 한 번 클릭 = 행 선택만(체크 그대로), 더블클릭 = 체크 토글을 실제 창 메시지(WM_LBUTTONDOWN / UP / DBLCLK, Space)로 확인한다.
#       - Chain 목록: 글자 클릭 / 이미 선택된 행 다시 클릭 / 체크 칸 클릭 / Space -> 체크 그대로, 더블클릭 -> 토글 + Chain 표시(Visible) 반영
#       - 유사 묶음 목록: 클릭 -> 체크 그대로, 더블클릭 -> 토글, 단자 보호 묶음은 더블클릭해도 체크 안 됨
#       - Setup 화면 Layer 목록: 클릭 / 다시 클릭 / 체크 칸 / Space -> 그대로, 더블클릭 -> 토글 + 도면 Layer 표시 반영,
#         [전체 미체크] / [전체 체크] 버튼은 그대로 동작, 우클릭은 행 선택만(색 변경 대상)
#       화면은 화면 밖에 띄우고 DB / Service 를 쓰지 않는다(빈 도면 + 시험 Chain 2개 + 시험 유사 묶음 3개 / Setup 은 샘플 GDS 읽기).
# 실행: powershell -NoProfile -STA -ExecutionPolicy Bypass -File GdsChainListDoubleClickSmoke.ps1 -GdsExe <빌드한 NexplantQMS.GdsMap.exe>
param(
    [Parameter(Mandatory = $true)][string]$GdsExe,
    [string]$GdsPath = ''
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
if ($GdsPath -eq '') { $GdsPath = Join-Path (Split-Path $dms -Parent) 'SampleFile\OMM all layer2.gds\OMM all layer2.gds' }
$probeDir = Join-Path $env:TEMP 'chippac\gdschaindblclicksmoke'
New-Item -ItemType Directory -Force $probeDir | Out-Null
Copy-Item $GdsExe (Join-Path $probeDir 'NexplantQMS.GdsMap.dll') -Force
foreach ($d in 'OpenTK.dll', 'OpenTK.GLControl.dll') { Copy-Item (Join-Path (Split-Path $GdsExe) $d) $probeDir -Force }
$clientBin = Join-Path $dms '100_ClientBin'
[AppDomain]::CurrentDomain.add_AssemblyResolve([ResolveEventHandler] {
    param($s, $e)
    $name = (New-Object Reflection.AssemblyName($e.Name)).Name
    foreach ($d in @($probeDir, $clientBin)) { $p = Join-Path $d ($name + '.dll'); if (Test-Path $p) { return [Reflection.Assembly]::LoadFrom($p) } }
    return $null
})
foreach ($d in 'OpenTK.dll', 'OpenTK.GLControl.dll', 'NexplantQMS.GdsMap.dll') { [void][Reflection.Assembly]::LoadFrom((Join-Path $probeDir $d)) }
Add-Type -AssemblyName System.Windows.Forms, System.Drawing

$helper = @"
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using NexplantQMS.GdsMap;
using NexplantQMS.GdsMap.Chain;
using NexplantQMS.GdsMap.Persistence;

public static class ChainDblClickSmoke
{
    const BindingFlags NP = BindingFlags.NonPublic | BindingFlags.Instance;
    [DllImport("user32.dll")] static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
    const int WM_KEYDOWN = 0x100, WM_KEYUP = 0x101, WM_CHAR = 0x102, WM_LBUTTONDOWN = 0x201, WM_LBUTTONUP = 0x202, WM_LBUTTONDBLCLK = 0x203,
        WM_RBUTTONDOWN = 0x204, WM_RBUTTONUP = 0x205;

    static object F(object o, string name) { FieldInfo f = o.GetType().GetField(name, NP); return f == null ? null : f.GetValue(o); }
    static object Call(object o, string name, params object[] args) { return o.GetType().GetMethod(name, NP).Invoke(o, args); }
    static void Pump() { for (int i = 0; i < 15; i++) { Application.DoEvents(); System.Threading.Thread.Sleep(10); } }
    static IntPtr Pt(Point p) { return (IntPtr)((p.Y << 16) | (p.X & 0xFFFF)); }

    /// <summary>창 메시지로 한 번 클릭(누름 / 뗌).</summary>
    static void Click(Control c, Point p)
    {
        SendMessage(c.Handle, WM_LBUTTONDOWN, (IntPtr)1, Pt(p)); SendMessage(c.Handle, WM_LBUTTONUP, IntPtr.Zero, Pt(p)); Pump();
    }

    /// <summary>창 메시지로 더블클릭: Windows 가 실제로 보내는 순서(누름 / 뗌 / 더블클릭 / 뗌).</summary>
    static void DoubleClick(Control c, Point p)
    {
        SendMessage(c.Handle, WM_LBUTTONDOWN, (IntPtr)1, Pt(p)); SendMessage(c.Handle, WM_LBUTTONUP, IntPtr.Zero, Pt(p));
        SendMessage(c.Handle, WM_LBUTTONDBLCLK, (IntPtr)1, Pt(p)); SendMessage(c.Handle, WM_LBUTTONUP, IntPtr.Zero, Pt(p)); Pump();
    }

    static void Space(Control c)
    {
        SendMessage(c.Handle, WM_KEYDOWN, (IntPtr)0x20, IntPtr.Zero); SendMessage(c.Handle, WM_CHAR, (IntPtr)0x20, IntPtr.Zero);
        SendMessage(c.Handle, WM_KEYUP, (IntPtr)0x20, IntPtr.Zero); Pump();
    }

    static Point TextPoint(ListBox list, int index) { Rectangle r = list.GetItemRectangle(index); return new Point(r.Left + 60, r.Top + r.Height / 2); }
    static Point GlyphPoint(ListBox list, int index) { Rectangle r = list.GetItemRectangle(index); return new Point(r.Left + 8, r.Top + r.Height / 2); }

    static string State(CheckedListBox list)
    {
        var parts = new List<string>();
        for (int i = 0; i < list.Items.Count; i++) parts.Add(list.GetItemChecked(i) ? "1" : "0");
        return "sel=" + list.SelectedIndex + " chk=" + string.Join("", parts.ToArray());
    }

    /// <summary>Setup 화면 Layer 목록 검사. 샘플 GDS 를 읽어 도면 / Layer 목록을 채운다.</summary>
    public static List<string> RunSetup(string gdsPath)
    {
        var r = new List<string>();
        var lib = new GdsReader { LengthUnit = LengthUnit.Micrometer }.Read(gdsPath);
        using (var form = new GdsMapForm(false))
        {
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(-32000, -32000);
            form.Size = new Size(1500, 1100);
            form.Show();
            Pump();
            var map = (GdsMapControl)F(form, "map");
            map.ShowStructure(lib);
            Call(form, "RefreshLayerList");
            Pump();
            var layers = (CheckedListBox)F(form, "chkLayerItems");
            Func<int, bool> mapVisible = i => {
                int id = ((LayerDisplayItem)layers.Items[i]).LayerId;
                foreach (var item in map.GetLayerDisplayItems()) if (item.LayerId == id) return item.Visible;
                return false; };
            r.Add("layerCheckOnClick=" + layers.CheckOnClick);
            r.Add("layer.count=" + layers.Items.Count);
            r.Add("layer.start=" + State(layers));
            Click(layers, TextPoint(layers, 0));
            r.Add("layer.click0=" + State(layers));
            Click(layers, TextPoint(layers, 0));
            r.Add("layer.clickAgain0=" + State(layers));
            Click(layers, GlyphPoint(layers, 1));
            r.Add("layer.glyph1=" + State(layers));
            Space(layers);
            r.Add("layer.space1=" + State(layers));
            DoubleClick(layers, TextPoint(layers, 0));
            r.Add("layer.dbl0=" + State(layers) + " map0=" + mapVisible(0));
            DoubleClick(layers, TextPoint(layers, 0));
            r.Add("layer.dbl0Again=" + State(layers) + " map0=" + mapVisible(0));
            Call(form, "btnLayerCheckNone_Click", form, EventArgs.Empty); Pump();
            r.Add("layer.none=" + State(layers) + " map0=" + mapVisible(0));
            Call(form, "btnLayerCheckAll_Click", form, EventArgs.Empty); Pump();
            r.Add("layer.all=" + State(layers) + " map0=" + mapVisible(0));
            Point p2 = TextPoint(layers, 2);
            SendMessage(layers.Handle, WM_RBUTTONDOWN, (IntPtr)2, Pt(p2)); SendMessage(layers.Handle, WM_RBUTTONUP, IntPtr.Zero, Pt(p2)); Pump();
            r.Add("layer.right2=" + State(layers));
            form.Close();
        }
        return r;
    }

    /// <summary>검사 결과 "이름=값" 목록.</summary>
    public static List<string> Run()
    {
        var r = new List<string>();
        using (var form = new GdsMapViewForm(true))
        {
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(-32000, -32000);
            form.Size = new Size(1730, 1194);
            form.Show();
            Pump();
            var map = (GdsMapControl)F(form, "map");
            map.ShowPlacedElements("DBLCLICK_SMOKE", new Dictionary<int, Color>(), new List<MapPlacedElementData>());
            Call(form, "UpdateChainSelectionButtons");
            Pump();

            // Chain 목록: 시험 Chain 2개 (만들 때 체크 = 표시)
            Call(form, "BtnChainNew_Click", form, EventArgs.Empty); Pump();
            Call(form, "BtnChainNew_Click", form, EventArgs.Empty); Pump();
            var chains = (CheckedListBox)F(form, "_chainList");
            var tabs = (TabControl)F(form, "tabRight");
            if (tabs != null) { tabs.SelectedIndex = 0; Pump(); }
            r.Add("chainCheckOnClick=" + chains.CheckOnClick);
            r.Add("chain.start=" + State(chains));
            Click(chains, TextPoint(chains, 0));
            r.Add("chain.click0=" + State(chains));
            Click(chains, TextPoint(chains, 0));
            r.Add("chain.clickAgain0=" + State(chains));
            Click(chains, GlyphPoint(chains, 1));
            r.Add("chain.glyph1=" + State(chains));
            Space(chains);
            r.Add("chain.space1=" + State(chains));
            DoubleClick(chains, TextPoint(chains, 0));
            var chain0 = chains.Items[0];
            bool visible0 = (bool)chain0.GetType().GetField("Visible").GetValue(chain0);
            r.Add("chain.dbl0=" + State(chains) + " visible0=" + visible0);
            DoubleClick(chains, TextPoint(chains, 0));
            visible0 = (bool)chain0.GetType().GetField("Visible").GetValue(chain0);
            r.Add("chain.dbl0Again=" + State(chains) + " visible0=" + visible0);

            // 유사 묶음 목록: 일치(체크) / 단자 보호(체크 안 됨) / 검토 필요(체크 안 됨)
            var groups = new List<ChainSimilarGroup> {
                new ChainSimilarGroup(new[] { "SMOKE_A" }, false, false),
                new ChainSimilarGroup(new[] { "SMOKE_B" }, true, false),
                new ChainSimilarGroup(new[] { "SMOKE_C" }, false, true) };
            // 실제 흐름(유사 묶음 찾기)처럼 화면의 묶음 목록에도 넣어야 유사 묶음 패널이 보인다(UpdateViewLayout).
            var formGroups = (IList)F(form, "_chainSimilarGroups");
            formGroups.Clear();
            foreach (var g in groups) formGroups.Add(g);
            Call(form, "FillChainSimilarList", groups);
            Call(form, "UpdateChainSelectionButtons");
            Pump();
            var similarPanel = (Control)F(form, "chainSimilarListPanel");
            r.Add("similarPanelVisible=" + similarPanel.Visible);
            var similar = (CheckedListBox)F(form, "_chainSimilarList");
            r.Add("similarCheckOnClick=" + similar.CheckOnClick);
            r.Add("similar.start=" + State(similar));
            Click(similar, TextPoint(similar, 0));
            r.Add("similar.click0=" + State(similar));
            Click(similar, TextPoint(similar, 0));
            r.Add("similar.clickAgain0=" + State(similar));
            DoubleClick(similar, TextPoint(similar, 0));
            r.Add("similar.dbl0=" + State(similar));
            DoubleClick(similar, TextPoint(similar, 1));
            r.Add("similar.dbl1Protected=" + State(similar));
            DoubleClick(similar, TextPoint(similar, 2));
            r.Add("similar.dbl2=" + State(similar));

            // 시험용 Chain 을 버려 닫을 때 "저장하지 않은 Chain" 확인 창이 뜨지 않게 한다.
            form.GetType().GetField("_activeChain", NP).SetValue(form, null);
            ((IList)F(form, "_chains")).Clear();
            form.Close();
        }
        return r;
    }
}
"@
Add-Type -TypeDefinition $helper -ReferencedAssemblies @((Join-Path $probeDir 'NexplantQMS.GdsMap.dll'), (Join-Path $probeDir 'OpenTK.dll'), (Join-Path $probeDir 'OpenTK.GLControl.dll'),
    'System.Windows.Forms', 'System.Drawing', 'System.Core', 'System.Data')

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
$v = @{}
foreach ($line in [ChainDblClickSmoke]::Run()) { $i = $line.IndexOf('='); $v[$line.Substring(0, $i)] = $line.Substring($i + 1); Write-Host "  $line" }
foreach ($line in [ChainDblClickSmoke]::RunSetup($GdsPath)) { $i = $line.IndexOf('='); $v[$line.Substring(0, $i)] = $line.Substring($i + 1); Write-Host "  $line" }

# 기대값: 손으로 정한 리터럴 (Chain 2개 모두 표시 체크로 시작 / 유사 묶음 = 일치만 체크)
Check 'Chain 목록 CheckOnClick 꺼짐' ($v['chainCheckOnClick'] -eq 'False') $v['chainCheckOnClick']
Check 'Chain 시작 상태' ($v['chain.start'] -like '* chk=11') $v['chain.start']
Check 'Chain 글자 클릭 = 선택만' ($v['chain.click0'] -eq 'sel=0 chk=11') $v['chain.click0']
Check 'Chain 선택된 행 다시 클릭 = 체크 그대로' ($v['chain.clickAgain0'] -eq 'sel=0 chk=11') $v['chain.clickAgain0']
Check 'Chain 체크 칸 클릭 = 선택만' ($v['chain.glyph1'] -eq 'sel=1 chk=11') $v['chain.glyph1']
Check 'Chain Space 키 = 체크 그대로' ($v['chain.space1'] -eq 'sel=1 chk=11') $v['chain.space1']
Check 'Chain 더블클릭 = 토글 + 표시 끔' ($v['chain.dbl0'] -eq 'sel=0 chk=01 visible0=False') $v['chain.dbl0']
Check 'Chain 다시 더블클릭 = 토글 + 표시 켬' ($v['chain.dbl0Again'] -eq 'sel=0 chk=11 visible0=True') $v['chain.dbl0Again']
Check '유사 묶음 패널 보임(시험 준비)' ($v['similarPanelVisible'] -eq 'True') $v['similarPanelVisible']
Check '유사 묶음 CheckOnClick 꺼짐' ($v['similarCheckOnClick'] -eq 'False') $v['similarCheckOnClick']
Check '유사 묶음 시작 상태' ($v['similar.start'] -like '* chk=100') $v['similar.start']
Check '유사 묶음 클릭 = 선택만' ($v['similar.click0'] -eq 'sel=0 chk=100') $v['similar.click0']
Check '유사 묶음 선택된 행 다시 클릭 = 체크 그대로' ($v['similar.clickAgain0'] -eq 'sel=0 chk=100') $v['similar.clickAgain0']
Check '유사 묶음 더블클릭 = 토글' ($v['similar.dbl0'] -eq 'sel=0 chk=000') $v['similar.dbl0']
Check '유사 묶음 단자 보호는 더블클릭해도 체크 안 됨' ($v['similar.dbl1Protected'] -eq 'sel=1 chk=000') $v['similar.dbl1Protected']
Check '유사 묶음 검토 필요 더블클릭 = 체크' ($v['similar.dbl2'] -eq 'sel=2 chk=001') $v['similar.dbl2']

# Setup Layer 목록: 샘플 GDS Layer 9개, 처음에는 모두 표시(체크)
Check 'Setup Layer 목록 CheckOnClick 꺼짐' ($v['layerCheckOnClick'] -eq 'False') $v['layerCheckOnClick']
Check 'Setup Layer 9개 / 모두 체크로 시작' ($v['layer.count'] -eq '9' -and $v['layer.start'] -like '* chk=111111111') ("{0} / {1}" -f $v['layer.count'], $v['layer.start'])
Check 'Setup 글자 클릭 = 선택만' ($v['layer.click0'] -eq 'sel=0 chk=111111111') $v['layer.click0']
Check 'Setup 선택된 행 다시 클릭 = 체크 그대로' ($v['layer.clickAgain0'] -eq 'sel=0 chk=111111111') $v['layer.clickAgain0']
Check 'Setup 체크 칸 클릭 = 선택만' ($v['layer.glyph1'] -eq 'sel=1 chk=111111111') $v['layer.glyph1']
Check 'Setup Space 키 = 체크 그대로' ($v['layer.space1'] -eq 'sel=1 chk=111111111') $v['layer.space1']
Check 'Setup 더블클릭 = 토글 + 도면 Layer 숨김' ($v['layer.dbl0'] -eq 'sel=0 chk=011111111 map0=False') $v['layer.dbl0']
Check 'Setup 다시 더블클릭 = 토글 + 도면 Layer 표시' ($v['layer.dbl0Again'] -eq 'sel=0 chk=111111111 map0=True') $v['layer.dbl0Again']
Check 'Setup [전체 미체크] 버튼 동작' ($v['layer.none'] -eq 'sel=0 chk=000000000 map0=False') $v['layer.none']
Check 'Setup [전체 체크] 버튼 동작' ($v['layer.all'] -eq 'sel=0 chk=111111111 map0=True') $v['layer.all']
Check 'Setup 우클릭 = 행 선택만(색 변경 대상)' ($v['layer.right2'] -eq 'sel=2 chk=111111111') $v['layer.right2']

$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host "GDS_CHAIN_LIST_DBLCLICK_SMOKE=PASS ($($results.Count))" } else { Write-Host "GDS_CHAIN_LIST_DBLCLICK_SMOKE=FAIL ($fail)"; exit 1 }
