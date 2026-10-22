# GDS Map Setup 화면 배치 Smoke (2026-10-13)
# 목적: Map(왼쪽 Tab) / Layer 목록(오른쪽) 배치에서, DACrux 호스트처럼 작은 창에 붙였다가 크게 키워도
#       Layer 목록은 설계 폭을 유지하고 Map Tab 이 남는 폭을 모두 쓰는지 확인한다.
#       - 호스트 1000 x 700 에 붙인 뒤 1900 x 1000 으로 키움 (frmGdsMapSetup 기본 크기)
#       - 호스트 500 x 400 처럼 아주 작게 붙인 뒤 1900 x 1000 으로 키움
#       - 단독 실행(TopLevel) 1900 x 1000
#       화면은 화면 밖에 띄우고 DB / Service / GDS 파일을 쓰지 않는다.
# 실행: powershell -NoProfile -STA -ExecutionPolicy Bypass -File GdsSetupLayoutSmoke.ps1 -GdsExe <빌드한 NexplantQMS.GdsMap.dll>
param(
    [Parameter(Mandatory = $true)][string]$GdsExe
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$probeDir = Join-Path $env:TEMP 'chippac\gdssetuplayoutsmoke'
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
using System.Reflection;
using System.Windows.Forms;
using NexplantQMS.GdsMap;

public static class SetupLayoutSmoke
{
    const BindingFlags NP = BindingFlags.NonPublic | BindingFlags.Instance;
    static object F(object o, string name) { FieldInfo f = o.GetType().GetField(name, NP); return f == null ? null : f.GetValue(o); }
    static void Pump() { for (int i = 0; i < 20; i++) { Application.DoEvents(); System.Threading.Thread.Sleep(10); } }

    static string Measure(GdsMapForm form)
    {
        var split = (SplitContainer)F(form, "GDSContainer");
        var tab = (Control)F(form, "tabControl1");
        return "total=" + split.ClientSize.Width + " tab=" + tab.Width + " layer=" + split.Panel2.Width;
    }

    /// <summary>DACrux 호스트(frmGdsMapSetup)처럼 Panel 에 TopLevel=false 로 붙인 뒤 호스트를 키운다.</summary>
    static string Hosted(Size start, Size end)
    {
        using (var host = new Form())
        {
            host.StartPosition = FormStartPosition.Manual;
            host.Location = new Point(-32000, -32000);
            host.ClientSize = start;
            var panel = new Panel { Dock = DockStyle.Fill };
            host.Controls.Add(panel);
            host.Show();
            Pump();
            var form = new GdsMapForm(true);
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            panel.Controls.Add(form);
            form.Show();
            Pump();
            string first = Measure(form);
            host.ClientSize = end;
            Pump();
            string last = Measure(form);
            host.Close();
            return first + " -> " + last;
        }
    }

    public static List<string> Run()
    {
        var r = new List<string>();
        using (var probe = new GdsMapForm(true))
        {
            var split = (SplitContainer)F(probe, "GDSContainer");
            r.Add("design=" + Measure(probe) + " fixed=" + split.FixedPanel);
        }
        r.Add("host1000=" + Hosted(new Size(1000, 700), new Size(1900, 1000)));
        r.Add("host500=" + Hosted(new Size(500, 400), new Size(1900, 1000)));
        using (var form = new GdsMapForm(true))
        {
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(-32000, -32000);
            form.Size = new Size(1900, 1000);
            form.Show();
            Pump();
            r.Add("standalone=" + Measure(form));
            form.Close();
        }
        return r;
    }
}
"@
Add-Type -TypeDefinition $helper -ReferencedAssemblies @((Join-Path $probeDir 'NexplantQMS.GdsMap.dll'), (Join-Path $probeDir 'OpenTK.dll'), (Join-Path $probeDir 'OpenTK.GLControl.dll'),
    'System.Windows.Forms', 'System.Drawing', 'System.Core')

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
function Nums($text) { $h = @{}; foreach ($m in [regex]::Matches($text, '(\w+)=(\d+)')) { $h[$m.Groups[1].Value] = [int]$m.Groups[2].Value }; return $h }

$v = @{}
foreach ($line in [SetupLayoutSmoke]::Run()) { $i = $line.IndexOf('='); $v[$line.Substring(0, $i)] = $line.Substring($i + 1); Write-Host "  $line" }

# 기대값(리터럴): Layer 목록 폭 = Designer 의 Layer 패널 최소 폭 400px(배율과 무관, 오차 10px),
#                Map Tab = 나머지 전부(전체 - Layer - 분할선 / 테두리, 오차 20px)
$layerWant = 400
$design = Nums $v['design']
Check '화면 생성 직후 Layer 폭 400 / Map Tab 나머지' (([Math]::Abs($design['layer'] - $layerWant) -le 10) -and $design['tab'] -ge ($design['total'] - $layerWant - 20)) $v['design']
foreach ($k in 'host1000', 'host500') {
    $last = Nums ($v[$k].Split('>')[1])
    $okLayer = [Math]::Abs($last['layer'] - $layerWant) -le 10
    $okTab = $last['tab'] -ge ($last['total'] - $layerWant - 20)
    Check "$k 호스트를 키운 뒤 Map Tab 이 남는 폭을 모두 씀" ($okLayer -and $okTab) $v[$k]
}
$s = Nums $v['standalone']
Check '단독 실행 1900 폭에서 Map Tab 이 남는 폭을 모두 씀' (([Math]::Abs($s['layer'] - $layerWant) -le 10) -and $s['tab'] -ge ($s['total'] - $layerWant - 20)) $v['standalone']

$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host "GDS_SETUP_LAYOUT_SMOKE=PASS ($($results.Count))" } else { Write-Host "GDS_SETUP_LAYOUT_SMOKE=FAIL ($fail)"; exit 1 }
