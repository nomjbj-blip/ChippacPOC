# GDS Map 조회 화면 레이아웃 Smoke (2026-10-12)
# 목적: 화면 배치 규칙을 확인한다.
#       1) 도구 막대 한 줄(넓은 창) / 막대를 한 줄 폭보다 좁게 하면 다음 줄로 넘어가고 잘리지 않음(2026-10-15 고정 900px 대신)
#       2) Map 정보 Label 없음, 문구는 Device 콤보 툴팁
#       3) 도면 전: Chain 작업 줄 / 유사 묶음 숨김, 도면 후: Chain 작업 줄 보임  4) Chain 선택 시 작업 줄 제목
#       5) 도면 우클릭 메뉴 없음(2026-10-13)  6) [Map ▾] 메뉴(원점 표시, 호스트 모드 Map 삭제. Sub Device 설정은 2026-10-15 제거)
#       7) Label 일치 방식 콤보 없음 / 버튼 순서 번호 없음 / Output 지정 숨김 / 도형 추가·제외 오른쪽 끝 / 성능 문구 없음 (2026-10-12)
#          도형 추가 / 도형 제외 버튼 이름 (2026-10-19)
#       8) Chain Code(A, B, C) 화면 숨김: 목록 / 작업 줄 제목은 이름, 새 Chain 기본 이름 "새 Chain n" (2026-10-12)
#       9) 상태 문구는 아래 상태 줄 한 곳: Chain 안내 줄 / Label 결과 글자 없음, Chain 안내는 한 문장, 목록 행은 색 견본(색 코드 없음) (2026-10-13)
#          목록 행 = 이름 + 저장 상태, 도형 수 없음 (2026-10-27)
#      10) 상태 줄 작업 중 표시: 조회 중 "작업 중..." + 모래시계, 끝나면 숨김 (처리 시간 표시 없음, 2026-10-15)
#      11) [Map 복사] 버튼: 호스트 모드에서 보이고 Map 선택 전 비활성, [Map ▾] 바로 왼쪽 (2026-10-15)
#      12) Label 검색 줄(구분선 / Label / 입력 / 검색 / 전체 보기 / 결과 콤보) 숨김, 도면 후에도 숨김 (2026-10-15)
#       화면은 화면 밖에 띄우고 DB / Service 를 쓰지 않는다(빈 도면 ShowPlacedElements). 배치 그림을 PNG 로 남긴다(OpenGL 도면 영역은 검게 나온다).
# 실행: powershell -NoProfile -STA -ExecutionPolicy Bypass -File GdsViewLayoutSmoke.ps1 [-GdsExe <빌드한 NexplantQMS.GdsMap.dll>] [-OutDir <PNG 폴더>]
param(
    [string]$GdsExe = '',
    [string]$OutDir = ''
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
if ($GdsExe -eq '') { $GdsExe = Join-Path $env:TEMP 'chippac\gdsmap\NexplantQMS.GdsMap.dll' }
if ($OutDir -eq '') { $OutDir = Split-Path $GdsExe }
# PNG 폴더가 없으면 Bitmap.Save 가 GDI+ 오류를 내므로 먼저 만든다 (2026-10-15)
New-Item -ItemType Directory -Force $OutDir | Out-Null
$probeDir = Join-Path $env:TEMP 'chippac\gdsviewlayoutsmoke'
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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using NexplantQMS.GdsMap;
using NexplantQMS.GdsMap.Persistence;

public static class ViewLayoutSmoke
{
    const BindingFlags NP = BindingFlags.NonPublic | BindingFlags.Instance;
    static object F(object o, string name) { FieldInfo f = o.GetType().GetField(name, NP); return f == null ? null : f.GetValue(o); }
    static object Call(object o, string name, params object[] args) { return o.GetType().GetMethod(name, NP).Invoke(o, args); }
    static void Pump() { for (int i = 0; i < 20; i++) { Application.DoEvents(); System.Threading.Thread.Sleep(10); } }

    static void Snap(Form form, string path)
    {
        using (var bmp = new Bitmap(form.Width, form.Height))
        {
            form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));
            bmp.Save(path, System.Drawing.Imaging.ImageFormat.Png);
        }
    }

    /// <summary>검사 결과 "이름=값" 목록. PNG 는 outDir 에 layout_wide.png / layout_narrow.png</summary>
    public static List<string> Run(string outDir)
    {
        var r = new List<string>();
        using (var form = new GdsMapViewForm(true))
        {
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(-32000, -32000);
            form.Size = new Size(1730, 1194);
            form.Show();
            Pump();
            var toolbar = (Control)F(form, "pnlCondition");
            var chainBar = (Control)F(form, "tblChainBar");
            var similar = (Control)F(form, "chainSimilarListPanel");
            var tabs = (TabControl)F(form, "tabRight");
            var map = (GdsMapControl)F(form, "map");
            var device = (Control)F(form, "cboDevice");
            var tip = (ToolTip)F(form, "toolTipView");
            var status = (ToolStripStatusLabel)F(form, "lblStatus");
            r.Add("mapInfoLabel=" + (F(form, "lblMapInfo") == null && F(form, "grpMapInfo") == null));
            r.Add("singleStatus=" + (F(form, "_lblChainSetupStatus") == null && F(form, "lblLabelResult") == null));
            r.Add("toolbarHeightWide=" + toolbar.Height);
            r.Add("tabs=" + string.Join(",", tabs.TabPages.Cast<TabPage>().Select(p => p.Text).ToArray()));
            r.Add("beforeMap.chainBar=" + chainBar.Visible + "|similar=" + similar.Visible);
            // Map 정보는 Device 콤보 툴팁으로 보인다: 도면 전에는 비어 있고, SetMapInfo 로 넣은 값이 그대로 툴팁이 된다 (2026-10-15).
            string tipBefore = tip.GetToolTip(device);
            form.GetType().GetMethod("SetMapInfo", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(form, new object[] { "Factory / Device: SMOKE / DEV" });
            r.Add("mapInfoTooltip=" + (tipBefore.Length == 0) + "|" + tip.GetToolTip(device).Replace("\r\n", " / "));
            form.GetType().GetMethod("SetMapInfo", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(form, new object[] { string.Empty });
            r.Add("mapContextMenu=" + (map.ContextMenuStrip == null && F(form, "mapMenu") == null && F(form, "mnuCtxZoomFit") == null));
            r.Add("labelMatchCombo=" + (F(form, "cboLabelMatch") != null));
            var copy = (Button)F(form, "btnCopyMap");
            r.Add("copyButton=" + copy.Visible + "|" + copy.Enabled + "|"
                + (toolbar.Controls.GetChildIndex(copy) + 1 == toolbar.Controls.GetChildIndex((Control)F(form, "btnMapMenu"))) + "|" + copy.Text);

            map.ShowPlacedElements("LAYOUT_SMOKE", new Dictionary<int, Color>(), new List<MapPlacedElementData>());
            Call(form, "UpdateChainSelectionButtons");
            Pump();
            r.Add("afterMap.chainBar=" + chainBar.Visible + "|similar=" + similar.Visible);
            // Label 검색 줄은 화면에 두지 않는다. 검색 코드(GdsMapViewForm_LabelSearch)는 남아 있고 도면을 그린 뒤에도 보이지 않아야 한다 (2026-10-15).
            var labelSearch = new[] { "sepTool2", "lblLabel", "txtLabel", "btnLabelSearch", "btnLabelAll", "cboLabelResults", "sepTool3" };
            var labelShown = labelSearch.Where(n => ((Control)F(form, n)).Visible).ToArray();
            r.Add("labelSearchHidden=" + (labelShown.Length == 0) + "|" + string.Join(",", labelShown));
            r.Add("barTitleNoChain=" + ((Label)F(form, "lblChainBarTitle")).Text);
            r.Add("barOneRow=" + (chainBar.Height <= 60) + "(" + chainBar.Height + ")");
            r.Add("statusNoPerf=" + !(status.Text.Contains("GPU") || status.Text.Contains("정점") || status.Text.Contains("ms") || status.Text.Contains("초"))
                + "|tooltip=" + string.IsNullOrEmpty(status.ToolTipText) + "|" + status.Text);
            var input = (Button)F(form, "_btnChainInput");
            var output = (Button)F(form, "_btnChainOutput");
            var trace = (Button)F(form, "_btnChainTrace");
            var find = (Button)F(form, "_btnChainFindSimilar");
            var add = (Button)F(form, "_btnChainAdd");
            var remove = (Button)F(form, "_btnChainRemove");
            r.Add("buttonTexts=" + input.Text + "," + output.Text + "," + trace.Text);
            r.Add("editButtonTexts=" + add.Text + "," + remove.Text);
            r.Add("outputVisible=" + output.Visible + "|inputVisible=" + input.Visible);
            Func<Control, Rectangle> box = c => chainBar.RectangleToClient(c.RectangleToScreen(c.ClientRectangle));
            r.Add("addRemoveRight=" + (add.Parent.Name == "chainEditRow" && box(add).Left > box(find).Right && box(remove).Left > box(add).Left
                && chainBar.ClientSize.Width - box(remove).Right <= 20) + "(" + box(find).Right + "<" + box(add).Left + ", 오른쪽 여백 " + (chainBar.ClientSize.Width - box(remove).Right) + ")");

            Call(form, "BtnChainNew_Click", form, EventArgs.Empty);
            Pump();
            r.Add("barTitleChain=" + ((Label)F(form, "lblChainBarTitle")).Text);
            r.Add("statusNewChain=" + status.Text);
            var chainList = (CheckedListBox)F(form, "_chainList");
            r.Add("chainListText=" + chainList.Items[0].ToString());
            var swatchList = chainList as LayerColorCheckedListBox;
            r.Add("chainListSwatch=" + (swatchList != null && chainList.Items[0] is IColorListItem && !swatchList.SelectOnRightClick));
            Call(form, "BtnChainInput_Click", form, EventArgs.Empty);
            Pump();
            r.Add("statusInputCapture=" + status.Text);
            Call(form, "BtnChainCancel_Click", form, EventArgs.Empty);
            Pump();
            r.Add("statusCancel=" + status.Text);

            var mapMenuStrip = (ContextMenuStrip)F(form, "mnuMap");
            Call(form, "btnMapMenu_Click", form, EventArgs.Empty);
            Pump();
            r.Add("mapMenuItems=" + string.Join(",", mapMenuStrip.Items.Cast<ToolStripItem>().Where(i => !(i is ToolStripSeparator) && i.Visible).Select(i => i.Text + (i.Enabled ? "" : "(off)")).ToArray()));
            mapMenuStrip.Close();
            var origin = (ToolStripMenuItem)F(form, "mnuOriginCross");
            origin.PerformClick();
            bool off = map.ShowOriginCross;
            origin.PerformClick();
            r.Add("originToggle=" + (!off && map.ShowOriginCross));
            // 상태 줄 작업 중 표시(2026-10-15): 조회 중 "작업 중..." + 모래시계, 끝나면 숨김 + 커서 원래대로. 처리 시간은 보이지 않는다.
            var elapsed = (ToolStripStatusLabel)F(form, "lblElapsed");
            Call(form, "SetLoading", true);
            for (int i = 0; i < 50; i++) { Application.DoEvents(); System.Threading.Thread.Sleep(10); }
            string during = elapsed.Text;
            bool duringShown = elapsed.Visible, duringWait = form.UseWaitCursor;
            Call(form, "SetLoading", false);
            r.Add("busyStatus=" + (duringShown && duringWait && during == "작업 중..." && !elapsed.Visible && !form.UseWaitCursor && elapsed.Text.Length == 0)
                + "|" + during + "|" + duringWait + "|" + elapsed.Visible + "|" + form.UseWaitCursor);

            Snap(form, System.IO.Path.Combine(outDir, "layout_wide.png"));
            form.Size = new Size(900, 800);
            Pump();
            var buttons = new Control[] { input, (Control)F(form, "_btnChainApply"), (Control)F(form, "_btnChainCancel"), trace, (Control)F(form, "_btnChainExample"), find, add, remove };
            bool inside = buttons.All(c => chainBar.ClientRectangle.Contains(box(c)));
            bool noOverlap = buttons.Take(6).All(c => !box(c).IntersectsWith(box(add)) && !box(c).IntersectsWith(box(remove)));
            r.Add("narrowBar=" + (inside && noOverlap) + "(" + chainBar.Height + ")");
            Snap(form, System.IO.Path.Combine(outDir, "layout_narrow.png"));
            // 좁은 창 도구 막대: 보이는 도구 막대 컨트롤을 한 줄로 놓은 폭보다 막대를 40px 좁게 해서 줄바꿈 / 잘림을 본다.
            // 고정 900px 는 Label 검색 줄을 숨긴 뒤 막대가 한 줄에 다 들어가 줄바꿈을 확인하지 못했다. 버튼 구성이 바뀌어도 기준이 따라간다 (2026-10-15).
            var toolItems = toolbar.Controls.Cast<Control>().Where(c => c.Visible).ToList();
            int oneLine = toolItems.Sum(c => c.Width + c.Margin.Horizontal) + toolbar.Padding.Horizontal;
            form.Size = new Size(form.Width - toolbar.Width + oneLine - 40, 800);
            Pump();
            bool toolInside = toolItems.All(c => toolbar.ClientRectangle.Contains(c.Bounds));
            r.Add("toolbarOneLine=" + oneLine);
            r.Add("toolbarHeightNarrow=" + toolbar.Height + "|" + toolInside + "|" + toolbar.Width);
            Snap(form, System.IO.Path.Combine(outDir, "layout_narrow_toolbar.png"));
            // 시험용으로 만든 새 Chain 을 버려 닫을 때 "저장하지 않은 Chain" 확인 창이 뜨지 않게 한다.
            form.GetType().GetField("_activeChain", NP).SetValue(form, null);
            ((System.Collections.IList)F(form, "_chains")).Clear();
            form.Close();
        }
        return r;
    }
}
"@
Add-Type -TypeDefinition $helper -ReferencedAssemblies @((Join-Path $probeDir 'NexplantQMS.GdsMap.dll'), (Join-Path $probeDir 'OpenTK.dll'),
    (Join-Path $probeDir 'OpenTK.GLControl.dll'), 'System.Windows.Forms', 'System.Drawing', 'System.Core')

$r = [ViewLayoutSmoke]::Run($OutDir)
$v = @{}; foreach ($line in $r) { Write-Host "  $line"; $k, $val = $line.Split('=', 2); $v[$k] = $val }
$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
Check 'Map 정보 Label 제거' ($v['mapInfoLabel'] -eq 'True') $v['mapInfoLabel']
Check '상태 표시는 상태 줄 한 곳 (Chain 안내 줄 / Label 결과 글자 없음)' ($v['singleStatus'] -eq 'True') $v['singleStatus']
Check '넓은 창: 도구 막대 한 줄' ([int]$v['toolbarHeightWide'] -le 60) $v['toolbarHeightWide']
$narrow = $v['toolbarHeightNarrow'].Split('|')
Check '좁은 창(막대 < 한 줄 폭): 도구 막대 줄바꿈 / 잘림 없음' ([int]$narrow[0] -gt [int]$v['toolbarHeightWide'] -and $narrow[1] -eq 'True') `
    "높이 $($narrow[0]) / 잘림 없음 $($narrow[1]) / 막대 $($narrow[2]) < 한 줄 $($v['toolbarOneLine'])"
Check '오른쪽 탭 = Chain, Layer' ($v['tabs'] -eq 'Chain,Layer') $v['tabs']
Check '도면 전: 작업 줄 / 유사 묶음 숨김' ($v['beforeMap.chainBar'] -eq 'False|similar=False') $v['beforeMap.chainBar']
Check 'Map 정보 = Device 콤보 툴팁 (도면 전 안내 문구 없음)' ($v['mapInfoTooltip'] -eq 'True|Factory / Device: SMOKE / DEV') $v['mapInfoTooltip']
Check '도면 우클릭 메뉴 없음' ($v['mapContextMenu'] -eq 'True') $v['mapContextMenu']
Check 'Label 일치 방식 콤보 없음' ($v['labelMatchCombo'] -eq 'False') $v['labelMatchCombo']
Check '[Map 복사] 버튼(호스트): 보임 / Map 선택 전 비활성 / [Map ▾] 바로 왼쪽' ($v['copyButton'] -eq 'True|False|True|Map 복사') $v['copyButton']
Check '도면 후: 작업 줄 보임, 유사 묶음 숨김' ($v['afterMap.chainBar'] -eq 'True|similar=False') $v['afterMap.chainBar']
Check 'Label 검색 줄 숨김(도면 후)' ($v['labelSearchHidden'] -eq 'True|') $v['labelSearchHidden']
Check '작업 줄 한 줄(넓은 창)' ($v['barOneRow'].StartsWith('True')) $v['barOneRow']
Check '작업 줄 제목: Chain 미선택' ($v['barTitleNoChain'] -eq 'Chain 선택 필요') $v['barTitleNoChain']
Check '작업 줄 제목: 새 Chain 선택(Code 없이 이름)' ($v['barTitleChain'] -eq '새 Chain 1 편집') $v['barTitleChain']
Check 'Chain 목록 행: 이름 + 저장 상태 (도형 수 / Code / 색 코드 없음, 도형 수는 2026-10-27 삭제)' ($v['chainListText'] -eq '새 Chain 1  (저장 안 됨)') $v['chainListText']
Check 'Chain 목록 행 색 견본 / 우클릭은 선택을 바꾸지 않음' ($v['chainListSwatch'] -eq 'True') $v['chainListSwatch']
Check '새 Chain 안내 한 문장(계산 값 / 구분자 없음)' ($v['statusNewChain'].StartsWith('새 Chain 1 을(를) 만들었습니다.') -and -not $v['statusNewChain'].Contains(' / ')) $v['statusNewChain']
Check 'Input 지정 상태(사용법 안내 없음)' ($v['statusInputCapture'] -eq 'Input 지정 중') $v['statusInputCapture']
Check '선택 취소 안내(Input / Output 개수 없음)' ($v['statusCancel'] -eq '선택을 취소했습니다.') $v['statusCancel']
Check '버튼 순서 번호 없음' ($v['buttonTexts'] -eq 'Input 지정,Output 지정,후보 경로 찾기') $v['buttonTexts']
Check 'Output 지정 숨김 / Input 보임' ($v['outputVisible'] -eq 'False|inputVisible=True') $v['outputVisible']
Check '도형 추가 / 제외 오른쪽 끝' ($v['addRemoveRight'].StartsWith('True')) $v['addRemoveRight']
Check 'Chain 편집 버튼 이름 = 도형 추가 / 도형 제외' ($v['editButtonTexts'] -eq '도형 추가,도형 제외') $v['editButtonTexts']
Check '좁은 창: 작업 줄 버튼 겹침 / 잘림 없음' ($v['narrowBar'].StartsWith('True')) $v['narrowBar']
Check '상태 줄 성능 문구 / 툴팁 없음' ($v['statusNoPerf'].StartsWith('True|tooltip=True')) $v['statusNoPerf']
Check '[Map ▾] 메뉴(호스트): 원점 / Map 삭제 (Sub Device 설정 없음)' ($v['mapMenuItems'] -eq '원점 표시,Map 삭제...') $v['mapMenuItems']
Check '원점 표시: [Map ▾] 켜기 / 끄기' ($v['originToggle'] -eq 'True') $v['originToggle']
Check '상태 줄 작업 중 표시: 조회 중 "작업 중..." + 모래시계 -> 끝나면 숨김 / 커서 원래대로' ($v['busyStatus'] -like 'True*') $v['busyStatus']
Write-Host "PNG: $(Join-Path $OutDir 'layout_wide.png') / layout_narrow.png / layout_narrow_toolbar.png"
$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host "GDS_VIEW_LAYOUT_SMOKE=PASS ($($results.Count))" } else { Write-Host "GDS_VIEW_LAYOUT_SMOKE=FAIL ($fail)"; exit 1 }
