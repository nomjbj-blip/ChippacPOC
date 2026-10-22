# GDS Map 복사 화면 Smoke (2026-10-15)
# 목적: 1) 복사 창(MapCopyForm): 빈 이름 / 원본과 같은 이름은 창에서 막고, 서버 오류(이미 있는 Device)는 창을 닫지 않고 사유를 보이며,
#          새 이름이면 복사 후 OK 로 끝나 결과(새 Device / 도형 수 / Chain 수)를 돌려주는지
#       2) GDS Map 조회 화면: 복사 뒤 Device 목록만 다시 채우고 지금 고른 Device / Map 은 그대로인지, 단독 실행(호스트 아님)에서는 [Map 복사]가 숨는지
#       서비스는 원격 호출 대신 같은 프로세스 BSL 을 RO 에 넣는다. 화면은 화면 밖에 띄운다.
# 데이터: Factory PROBE 의 LAYER_FILTER_PROBE(읽기만)를 COPY_UI_PROBE 로 복사하고 끝나면 지운다. 사용자 데이터는 건드리지 않는다.
# 실행: powershell -NoProfile -STA -ExecutionPolicy Bypass -File GdsMapCopyUiSmoke.ps1 -SrvDir <BSL/DSL/Interface/RO 폴더> -GdsDir <GdsMap EXE 폴더>
param(
    [Parameter(Mandatory = $true)][string]$SrvDir,
    [Parameter(Mandatory = $true)][string]$GdsDir
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$serverBin = Join-Path $dms '101_ServerBin'
[AppDomain]::CurrentDomain.SetData('APP_CONFIG_FILE', (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config'))
Add-Type -AssemblyName System.Configuration, System.Windows.Forms, System.Drawing
$flags = [Reflection.BindingFlags]'NonPublic,Static'
$cm = [Configuration.ConfigurationManager]
$cm.GetField('s_initState', $flags).SetValue($null, 0)
$cm.GetField('s_configSystem', $flags).SetValue($null, $null)

# 시험용 임시 폴더: GdsMap EXE 는 .dll 이름으로 복사해 참조한다(Add-Type 은 .exe 참조 불가).
$probeDir = Join-Path $env:TEMP 'chippac\gdsmapcopyui'
New-Item -ItemType Directory -Force $probeDir | Out-Null
Copy-Item (Join-Path $GdsDir 'NexplantQMS.GdsMap.dll') (Join-Path $probeDir 'NexplantQMS.GdsMap.dll') -Force
$searchDirs = @($SrvDir, $GdsDir, $probeDir, $serverBin)
[AppDomain]::CurrentDomain.add_AssemblyResolve([ResolveEventHandler] {
    param($s, $e)
    $name = (New-Object Reflection.AssemblyName($e.Name)).Name
    foreach ($d in $searchDirs) { foreach ($ext in '.dll', '.exe') { $p = Join-Path $d ($name + $ext); if (Test-Path $p) { return [Reflection.Assembly]::LoadFrom($p) } } }
    return $null
})
foreach ($p in @("$serverBin\Oracle.ManagedDataAccess.dll", "$serverBin\Miracom.Middleware.dll", "$SrvDir\DACrux.Base.dll", "$SrvDir\DACrux.SEMDMS.Interface.dll",
        "$SrvDir\DACrux.SEMDMS.DSL.dll", "$SrvDir\DACrux.SEMDMS.BSL.dll", "$SrvDir\DACrux.SEMDMS.RO.dll", "$GdsDir\OpenTK.dll", "$GdsDir\OpenTK.GLControl.dll",
        "$probeDir\NexplantQMS.GdsMap.dll")) { [void][Reflection.Assembly]::LoadFrom($p) }

$helper = @"
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

public static class MapCopyUiSmoke
{
    public const string F = "PROBE";
    public const string Source = "LAYER_FILTER_PROBE";
    public const string Target = "COPY_UI_PROBE";
    const BindingFlags NP = BindingFlags.NonPublic | BindingFlags.Instance;

    /// <summary>RO 를 원격 연결 없이 만들고 내부 m_OBJ 에 같은 프로세스 BSL 을 넣는다.</summary>
    static T Ro<T>(object bsl)
    {
        object ro = FormatterServices.GetUninitializedObject(typeof(T));
        typeof(T).GetField("m_OBJ", NP).SetValue(ro, bsl);
        return (T)ro;
    }

    static object Field(object o, string name) { return o.GetType().GetField(name, NP).GetValue(o); }
    static void SetField(object o, string name, object value) { o.GetType().GetField(name, NP).SetValue(o, value); }

    static void Pump(Func<bool> done, int ms)
    {
        var until = DateTime.Now.AddMilliseconds(ms);
        while (!done() && DateTime.Now < until) { Application.DoEvents(); System.Threading.Thread.Sleep(20); }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Cleanup()
    {
        var imp = new DACrux.SEMDMS.BSL.GdsMapImport();
        if (imp.GetMapInfo(F, Target).Rows.Count > 0) imp.DeleteMap(F, Target, "probe");
    }

    /// <summary>복사 창: 이름을 넣고 [복사]. 끝날 때까지 기다린다(화면 스레드 메시지 루프 안). 반환 "DialogResult|상태 문구|입력 가능"</summary>
    static async Task<string> TryAsync(NexplantQMS.GdsMap.MapCopyForm form, string name)
    {
        var text = (TextBox)Field(form, "txtTarget");
        var status = (Label)Field(form, "lblStatus");
        var copy = (Button)Field(form, "btnCopy");
        text.Text = name;
        form.GetType().GetMethod("btnCopy_Click", NP).Invoke(form, new object[] { null, EventArgs.Empty });
        var until = DateTime.Now.AddSeconds(60);
        while (((bool)Field(form, "_busy") || (form.DialogResult != DialogResult.OK && status.Text.Length == 0)) && DateTime.Now < until)
            await Task.Delay(20);
        return form.DialogResult + "|" + status.Text + "|" + (text.Enabled && copy.Enabled);
    }

    /// <summary>
    /// 반환: 원본 표시 / 빈 이름 / 같은 이름 / 이미 있는 이름 / 새 이름 결과와 복사 결과 "새 Device|도형|Chain", DB 헤더 상태.
    /// 실제 화면처럼 Application.Run 메시지 루프 안에서 진행한다. 루프 밖에서 DoEvents 로 돌리면 WinForms 동기화 컨텍스트가 풀려
    /// 창의 await 뒤 처리가 다른 스레드에서 돌기 때문이다. 새 STA 스레드에서 실행한다.
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string[] Dialog()
    {
        string[] result = null;
        Exception error = null;
        var thread = new Thread(() =>
        {
            using (var form = new NexplantQMS.GdsMap.MapCopyForm(F, Source))
            {
                form.ImportService = Ro<DACrux.SEMDMS.RO.GdsMapImport>(new DACrux.SEMDMS.BSL.GdsMapImport());
                form.StartPosition = FormStartPosition.Manual;
                form.Location = new System.Drawing.Point(-32000, -32000);
                form.Shown += async (s, e) =>
                {
                    try { result = await DialogSteps(form); }
                    catch (Exception ex) { error = ex; }
                    form.Close();
                };
                Application.Run(form);
            }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();
        if (error != null) throw new InvalidOperationException("복사 창 시험 실패", error);
        return result;
    }

    static async Task<string[]> DialogSteps(NexplantQMS.GdsMap.MapCopyForm form)
    {
        string source = ((TextBox)Field(form, "txtSource")).Text + "|" + ((TextBox)Field(form, "txtSource")).ReadOnly;
        string empty = await TryAsync(form, "  ");
        string same = await TryAsync(form, Source.ToLowerInvariant());
        string exists = await TryAsync(form, "view_probe");
        string ok = await TryAsync(form, Target);
        string result = form.TargetDeviceId + "|" + form.PlacedCount + "|" + form.ChainCount;
        return new[] { source, empty, same, exists, ok, result, MapState(Target) };
    }

    /// <summary>DB 헤더 "상태|도형 수" 또는 NONE</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    static string MapState(string device)
    {
        DataTable info = new DACrux.SEMDMS.BSL.GdsMapImport().GetMapInfo(F, device);
        return info.Rows.Count == 0 ? "NONE" : info.Rows[0]["MAP_STATUS"] + "|" + info.Rows[0]["PLACED_COUNT"];
    }
    /// <summary>
    /// GDS Map 조회 화면(호스트 모드): 복사 뒤 Device 목록 다시 채우기. 지금 Device 텍스트 / 지금 Map 이 그대로이고 새 Device 가 목록에 들어가는지.
    /// 반환 "목록에 새 Device|텍스트|같은 Map|목록 수"
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string ViewRefresh()
    {
        using (var form = new NexplantQMS.GdsMap.GdsMapViewForm(true))
        {
            var combo = (ComboBox)Field(form, "cboDevice");
            SetField(form, "_factory", F);
            SetField(form, "_restoringCondition", true);
            combo.Items.Add(Source);
            combo.Items.Add("VIEW_PROBE");
            combo.SelectedIndex = 0;
            SetField(form, "_restoringCondition", false);
            Type mapItemType = form.GetType().GetField("_currentMap", NP).FieldType;
            object current = FormatterServices.GetUninitializedObject(mapItemType);
            SetField(form, "_currentMap", current);
            SetField(form, "_deviceTable", new DACrux.SEMDMS.BSL.GdsMapView().GetMapDeviceList());
            form.GetType().GetMethod("RefreshDeviceItems", NP).Invoke(form, null);
            bool listed = combo.Items.Cast<object>().Any(i => i.ToString() == Target);
            bool same = ReferenceEquals(Field(form, "_currentMap"), current);
            return listed + "|" + combo.Text + "|" + same + "|" + combo.Items.Count;
        }
    }

    /// <summary>단독 실행(호스트 아님)에서 [Map 복사] 표시 여부</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string StandaloneButton()
    {
        using (var form = new NexplantQMS.GdsMap.GdsMapViewForm())
        {
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new System.Drawing.Point(-32000, -32000);
            form.Show();
            Pump(() => false, 200);
            bool visible = ((Button)Field(form, "btnCopyMap")).Visible;
            form.Close();
            return visible.ToString();
        }
    }
}
"@
Add-Type -TypeDefinition $helper -ReferencedAssemblies @("$SrvDir\DACrux.SEMDMS.BSL.dll", "$SrvDir\DACrux.SEMDMS.Interface.dll", "$SrvDir\DACrux.SEMDMS.RO.dll",
    "$probeDir\NexplantQMS.GdsMap.dll", "$serverBin\Miracom.Middleware.dll",
    'System.Data', 'System.Xml', 'System.Core', 'System.Windows.Forms', 'System.Drawing', 'System.Runtime.Serialization')

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
$T = [MapCopyUiSmoke]::Target
try {
    [MapCopyUiSmoke]::Cleanup()
    $r = [MapCopyUiSmoke]::Dialog()
    Check '복사 창: 원본 Device 읽기 전용' ($r[0] -eq 'LAYER_FILTER_PROBE|True') $r[0]
    Check '빈 이름 -> 창에서 막음' ($r[1] -eq 'None|새 Device 이름을 입력하세요.|True') $r[1]
    Check '원본과 같은 이름(대소문자 무시) -> 창에서 막음' ($r[2] -eq 'None|원본과 다른 이름을 입력하세요.|True') $r[2]
    Check '이미 있는 Device -> 서버 사유 표시, 창 유지 / 다시 입력 가능' ($r[3].StartsWith('None|이미 있는 Device 입니다: VIEW_PROBE') -and $r[3].EndsWith('|True') -and -not $r[3].Contains('Parameter')) $r[3]
    Check '새 이름 -> 복사 후 OK' ($r[4].StartsWith('OK|')) $r[4]
    Check '복사 결과 = 새 Device / 도형 29,871 / Chain 0' ($r[5] -eq "$T|29871|0") $r[5]
    Check 'DB: 새 Device READY / 도형 수' ($r[6] -eq 'READY|29871') $r[6]

    $r = [MapCopyUiSmoke]::ViewRefresh()
    $p = $r.Split('|')
    Check '조회 화면: 목록 다시 채움 -> 새 Device 보임, 지금 Device / Map 그대로' ($p[0] -eq 'True' -and $p[1] -eq 'LAYER_FILTER_PROBE' -and $p[2] -eq 'True') $r
    $r = [MapCopyUiSmoke]::StandaloneButton()
    Check '단독 실행(호스트 아님): [Map 복사] 숨김' ($r -eq 'False') $r
}
finally {
    [MapCopyUiSmoke]::Cleanup()
}
$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host "GDS_MAP_COPY_UI_SMOKE=PASS ($($results.Count))" } else { Write-Host "GDS_MAP_COPY_UI_SMOKE=FAIL ($fail)"; exit 1 }
