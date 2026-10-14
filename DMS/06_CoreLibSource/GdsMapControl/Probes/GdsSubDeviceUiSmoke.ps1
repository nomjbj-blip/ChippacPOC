# GDS Sub Device 화면 Smoke (2026-10-12)
# 목적: 1) Sub Device 설정 창(SubDeviceSetupForm)이 목록을 읽고(첫 행 대표) 행 추가 후 [저장]하면 DB 에 반영되는지
#       2) GDS Map 조회 화면(GdsMapViewForm)의 [Map ▾] 메뉴에 Sub Device 설정 항목이 있는지 (2026-10-12 버튼 -> 메뉴)
#       3) GDS-KLARF 연관 분석 화면이 KLARF Product(Sub Device)로 대표 GDS Device 를 자동 선택하는지
#       서비스는 원격 호출 대신 같은 프로세스 BSL 을 RO 에 넣는다(기존 GdsKlarfAnalysisFormSmoke 와 같은 방식). 화면은 화면 밖에 띄운다.
# 데이터: Factory PROBE 의 SUBDEV_UI_REP 빈 Map 헤더 + Sub 설정을 만들고 끝나면 지운다. 사용자 데이터는 건드리지 않는다.
# 실행: powershell -NoProfile -STA -ExecutionPolicy Bypass -File GdsSubDeviceUiSmoke.ps1 [-BuildDir %TEMP%\chippac\gdsmap-build] [-GdsDir <GdsMap EXE 폴더>]
param(
    [string]$BuildDir = '',
    [string]$GdsDir = ''
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$serverBin = Join-Path $dms '101_ServerBin'
if ($BuildDir -eq '') { $BuildDir = Join-Path $env:TEMP 'chippac\gdsmap-build' }
$srv = Join-Path $BuildDir 'srv'; $ui = Join-Path $BuildDir 'ui'; $gds = $(if ($GdsDir -ne '') { $GdsDir } else { Join-Path $BuildDir 'GdsMap' })
[AppDomain]::CurrentDomain.SetData('APP_CONFIG_FILE', (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config'))
Add-Type -AssemblyName System.Configuration, System.Windows.Forms, System.Drawing
$flags = [Reflection.BindingFlags]'NonPublic,Static'
$cm = [Configuration.ConfigurationManager]
$cm.GetField('s_initState', $flags).SetValue($null, 0)
$cm.GetField('s_configSystem', $flags).SetValue($null, $null)

# 시험용 임시 폴더: GdsMap EXE 는 .dll 이름으로 복사해 참조한다(Add-Type 은 .exe 참조 불가).
$probeDir = Join-Path $env:TEMP 'chippac\gdssubdeviceui'
New-Item -ItemType Directory -Force $probeDir | Out-Null
Copy-Item (Join-Path $gds 'NexplantQMS.GdsMap.exe') (Join-Path $probeDir 'NexplantQMS.GdsMap.dll') -Force
# 같은 이름 DLL 은 srv(새 서버 코드) > ui(새 RO / ENGUI) > ServerBin 순으로 찾는다.
$searchDirs = @($srv, $ui, $gds, $probeDir, $serverBin)
[AppDomain]::CurrentDomain.add_AssemblyResolve([ResolveEventHandler] {
    param($s, $e)
    $name = (New-Object Reflection.AssemblyName($e.Name)).Name
    foreach ($d in $searchDirs) { foreach ($ext in '.dll', '.exe') { $p = Join-Path $d ($name + $ext); if (Test-Path $p) { return [Reflection.Assembly]::LoadFrom($p) } } }
    return $null
})
foreach ($p in @("$serverBin\Oracle.ManagedDataAccess.dll", "$serverBin\Miracom.Middleware.dll", "$srv\DACrux.SEMDMS.Interface.dll", "$srv\DACrux.SEMDMS.DSL.dll",
        "$srv\DACrux.SEMDMS.BSL.dll", "$ui\DACrux.Base.dll", "$srv\DACrux.SEMDMS.RO.dll", "$gds\OpenTK.dll", "$gds\OpenTK.GLControl.dll", "$probeDir\NexplantQMS.GdsMap.dll",
        "$ui\DACrux.SEMDMS.ENGUI.dll")) { [void][Reflection.Assembly]::LoadFrom($p) }

$helper = @"
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows.Forms;

public static class SubDeviceUiSmoke
{
    public const string F = "PROBE";
    public const string Rep = "SUBDEV_UI_REP";

    /// <summary>RO 를 원격 연결 없이 만들고 내부 m_OBJ 에 같은 프로세스 BSL 을 넣는다.</summary>
    static T Ro<T>(object bsl)
    {
        object ro = FormatterServices.GetUninitializedObject(typeof(T));
        typeof(T).GetField("m_OBJ", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(ro, bsl);
        return (T)ro;
    }

    static object Field(object o, string name) { return o.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(o); }

    static void Pump(Func<bool> done, int ms)
    {
        var until = DateTime.Now.AddMilliseconds(ms);
        while (!done() && DateTime.Now < until) { Application.DoEvents(); System.Threading.Thread.Sleep(20); }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Setup()
    {
        var imp = new DACrux.SEMDMS.BSL.GdsMapImport();
        if (imp.GetMapInfo(F, Rep).Rows.Count > 0) { imp.SaveSubDeviceList(F, Rep, new string[0], null, "probe"); imp.DeleteMap(F, Rep, "probe"); }
        DataTable job = imp.BeginMapImport(new[] { F, Rep, "TOP", "probe.gds", "", "0.001", "1E-09", "probe", "0", "0", "0" });
        imp.CompleteMapImport(job.Rows[0]["IMPORT_JOB_SEQ"].ToString(), "probe");
        imp.SaveSubDeviceList(F, Rep, new[] { "OMM_Hybrid_UI" }, new[] { "KLARF Product" }, "probe");
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Cleanup()
    {
        var imp = new DACrux.SEMDMS.BSL.GdsMapImport();
        if (imp.GetMapInfo(F, Rep).Rows.Count == 0) return;
        imp.SaveSubDeviceList(F, Rep, new string[0], null, "probe");
        imp.DeleteMap(F, Rep, "probe");
    }

    /// <summary>설정 창: 읽기 -> 행 추가 -> 저장. 반환 "첫행|행수|저장후 DB 목록|SubDeviceText|상태"</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string SetupDialog()
    {
        var view = new DACrux.SEMDMS.BSL.GdsMapView();
        using (var form = new NexplantQMS.GdsMap.SubDeviceSetupForm(F, Rep))
        {
            form.ViewService = Ro<DACrux.SEMDMS.RO.GdsMapView>(view);
            form.ImportService = Ro<DACrux.SEMDMS.RO.GdsMapImport>(new DACrux.SEMDMS.BSL.GdsMapImport());
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new System.Drawing.Point(-32000, -32000);
            form.Show();
            var grid = (DataGridView)Field(form, "gridSub");
            Pump(() => grid.Rows.Cast<DataGridViewRow>().Count(r => !r.IsNewRow) >= 2, 5000);
            string first = Convert.ToString(grid.Rows[0].Cells[0].Value);
            bool firstReadOnly = grid.Rows[0].ReadOnly;
            int loaded = grid.Rows.Cast<DataGridViewRow>().Count(r => !r.IsNewRow);
            grid.Rows.Add("OMM_UI", "E-Test 파일명");
            form.GetType().GetMethod("btnSave_Click", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(form, new object[] { null, EventArgs.Empty });
            Pump(() => form.Saved && ((Label)Field(form, "lblStatus")).Text.StartsWith("저장 완료"), 5000);
            string db = string.Join(",", view.GetSubDeviceList(F, Rep).Rows.Cast<DataRow>().Select(r => r["SUB_DEVICE_ID"].ToString()).ToArray());
            string status = ((Label)Field(form, "lblStatus")).Text;
            // 대표 행 삭제 시도는 무시되어야 한다.
            grid.ClearSelection(); grid.Rows[0].Selected = true;
            form.GetType().GetMethod("btnDeleteRow_Click", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(form, new object[] { null, EventArgs.Empty });
            string afterDelete = Convert.ToString(grid.Rows[0].Cells[0].Value);
            form.GetType().GetField("_dirty", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(form, false);
            form.Close();
            return first + (firstReadOnly ? "(RO)" : "(RW)") + "|" + loaded + "|" + db + "|" + form.SubDeviceText + "|" + status + "|" + afterDelete;
        }
    }

    /// <summary>GDS Map 조회 화면을 DMS 호스트 모드로 만들고 [Map ▾] 의 Sub Device 설정 항목 상태를 본다. 반환 "Text|Enabled|메뉴"</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string ViewFormButton()
    {
        using (var form = new NexplantQMS.GdsMap.GdsMapViewForm(true))
        {
            var button = (ToolStripMenuItem)Field(form, "mnuSubDevice");
            return button.Text + "|" + button.Enabled + "|" + (button.Owner == null ? "" : button.Owner.Name);
        }
    }

    /// <summary>KLARF 연관 분석 화면: Product(Sub)로 대표 Device 자동 선택. 반환 "선택|안내"</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string KlarfSelect(string product)
    {
        DACrux.Base.GlobalVariable.Factory = F;
        using (var form = new DACrux.SEMDMS.ENGUI.frmGdsKlarfAnalysis())
        {
            form.MapService = Ro<DACrux.SEMDMS.RO.GdsMapView>(new DACrux.SEMDMS.BSL.GdsMapView());
            Type t = form.GetType();
            t.GetField("_product", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(form, product);
            string note = (string)t.GetMethod("SelectDeviceForProduct", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(form, null);
            var combo = (ComboBox)Field(form, "cboDevice");
            return Convert.ToString(combo.SelectedItem) + "|" + note;
        }
    }

    /// <summary>BSL GdsKlarfAnalysis 의 Product -> 대표 Device 찾기(Device 인자를 비웠을 때).</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string BslResolve(string product)
    {
        MethodInfo m = typeof(DACrux.SEMDMS.BSL.GdsKlarfAnalysis).GetMethod("ResolveMapDevice", BindingFlags.NonPublic | BindingFlags.Static);
        return (string)m.Invoke(null, new object[] { F, product });
    }
}
"@
Add-Type -TypeDefinition $helper -ReferencedAssemblies @("$srv\DACrux.SEMDMS.BSL.dll", "$srv\DACrux.SEMDMS.Interface.dll", "$srv\DACrux.SEMDMS.RO.dll",
    "$ui\DACrux.SEMDMS.ENGUI.dll", "$ui\DACrux.Base.dll", "$ui\DACrux.Framework.Base.dll", "$probeDir\NexplantQMS.GdsMap.dll", "$serverBin\Miracom.Middleware.dll",
    'System.Data', 'System.Xml', 'System.Core', 'System.Windows.Forms', 'System.Drawing', 'System.Runtime.Serialization')

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
$rep = [SubDeviceUiSmoke]::Rep
try {
    [SubDeviceUiSmoke]::Setup()
    $r = [SubDeviceUiSmoke]::SetupDialog(); $p = $r.Split('|')
    Check '설정 창 첫 행 = 대표(읽기 전용)' ($p[0] -eq "$rep(RO)") $p[0]
    Check '설정 창 읽기 = 대표 + Sub 1' ($p[1] -eq '2') $p[1]
    Check '행 추가 후 [저장] -> DB 반영' ($p[2] -eq 'OMM_Hybrid_UI,OMM_UI') $p[2]
    Check '저장 후 SubDeviceText' ($p[3] -eq 'OMM_Hybrid_UI, OMM_UI') $p[3]
    Check '상태 줄 저장 완료' ($p[4].StartsWith('저장 완료')) $p[4]
    Check '대표 행은 [행 삭제]로 안 지워짐' ($p[5] -eq $rep) $p[5]

    $r = [SubDeviceUiSmoke]::ViewFormButton()
    Check 'GDS Map 조회 화면 [Map ▾] Sub Device 설정(Map 선택 전 비활성)' ($r -eq 'Sub Device 설정...|False|mnuMap') $r

    $r = [SubDeviceUiSmoke]::KlarfSelect('omm_hybrid_ui')
    Check 'KLARF Product(Sub) -> 대표 자동 선택' ($r.StartsWith("$rep|") -and $r.Contains('Sub Device')) $r
    $r = [SubDeviceUiSmoke]::KlarfSelect($rep)
    Check 'KLARF Product = 대표 이름 -> 그대로 선택' ($r -eq "$rep|") $r
    $r = [SubDeviceUiSmoke]::BslResolve('OMM_UI'); Check 'BSL 분석 Device 비움 -> Product(Sub)로 대표' ($r -eq $rep) $r
    $r = [SubDeviceUiSmoke]::BslResolve('NO_SUCH_PRODUCT'); Check 'BSL 연결 없음 -> Product 그대로' ($r -eq 'NO_SUCH_PRODUCT') $r
}
finally {
    [SubDeviceUiSmoke]::Cleanup()
}
$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host "GDS_SUB_DEVICE_UI_SMOKE=PASS ($($results.Count))" } else { Write-Host "GDS_SUB_DEVICE_UI_SMOKE=FAIL ($fail)"; exit 1 }
