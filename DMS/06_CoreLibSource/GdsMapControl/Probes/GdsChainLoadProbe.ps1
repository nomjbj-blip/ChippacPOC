# GDS Map View 화면 DB Chain 불러오기 왕복 검증 스크립트 (3단계, 설계: 문서/설계/02_GDS/2026-10-06_GDS_Map_화면역할_재정의_설계.md 4.3절)
# 목적: DB 도형으로 실제 탐색한 Chain을 BSL GdsChain으로 저장(INPUT / TRACE / 수동 추가 MANUAL / 수동 제외 EXCLUDE)한 뒤,
#       View 화면의 복원 코드(CreateChainFromDb, private)를 리플렉션으로 호출해 화면 Chain을 만들고,
#       저장 행 생성 코드(BuildChainSaveData)가 다시 만든 Member / 편집 행이 처음 저장한 행과 같은지(왕복 일치) 확인한다.
#       Layer 일부만 조회한 지도에서는 DbIncomplete(저장 제외)로 표시되는지도 확인한다.
#       Service를 띄우지 않고 BSL을 직접 호출한다(화면의 RO 원격 호출은 DMS 화면에서 확인).
# 전제: FACTORY=PROBE / DEVICE_ID=LAYER_FILTER_PROBE READY Map(Layer 1, 2, 6). Chain Code ZZLOAD1을 만들었다가 마지막에 삭제한다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsChainLoadProbe.ps1 -GdsExe <빌드한 NexplantQMS.GdsMap.exe> [-BinDir <BSL/DSL/Interface 폴더>]
#       -BinDir 생략 시 101_ServerBin. 배포 전에는 TEMP 빌드 폴더를 넘긴다.
# Layer 는 번호 + DataType 키다. 저장은 화면과 같은 탐색 Layer 2열(LAYER_ID / DATA_TYPE) / 규칙 5열로 한다 (2026-10-20).
param(
    [Parameter(Mandatory = $true)][string]$GdsExe,
    [string]$Factory = 'PROBE',
    [string]$Device = 'LAYER_FILTER_PROBE',
    [string]$BinDir = ''
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$serverBin = Join-Path $dms '101_ServerBin'
if ($BinDir -eq '') { $BinDir = $serverBin }
[AppDomain]::CurrentDomain.SetData('APP_CONFIG_FILE', (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config'))
Add-Type -AssemblyName System.Configuration
$flags = [Reflection.BindingFlags]'NonPublic,Static'
$cm = [Configuration.ConfigurationManager]
$cm.GetField('s_initState', $flags).SetValue($null, 0)
$cm.GetField('s_configSystem', $flags).SetValue($null, $null)
foreach ($d in 'Oracle.ManagedDataAccess', 'Miracom.Middleware', 'DACrux.Base') {
    $p = Join-Path $serverBin "$d.dll"; if (Test-Path $p) { [void][Reflection.Assembly]::LoadFrom($p) }
}
foreach ($d in 'DACrux.SEMDMS.Interface', 'DACrux.SEMDMS.DSL', 'DACrux.SEMDMS.BSL') { [void][Reflection.Assembly]::LoadFrom((Join-Path $BinDir "$d.dll")) }
Write-Host "BSL: $(Join-Path $BinDir 'DACrux.SEMDMS.BSL.dll')"
[void][Reflection.Assembly]::LoadFrom((Join-Path $dms '100_ClientBin\DACrux.SEMDMS.RO.dll'))
$probeDir = Join-Path $env:TEMP 'chippac\gdschainloadprobe'
New-Item -ItemType Directory -Force $probeDir | Out-Null
Copy-Item $GdsExe (Join-Path $probeDir 'NexplantQMS.GdsMap.dll') -Force
foreach ($d in 'OpenTK.dll', 'OpenTK.GLControl.dll') { Copy-Item (Join-Path (Split-Path $GdsExe) $d) $probeDir -Force }
$gdsDll = Join-Path $probeDir 'NexplantQMS.GdsMap.dll'
[void][Reflection.Assembly]::LoadFrom((Join-Path $probeDir 'OpenTK.dll'))
[void][Reflection.Assembly]::LoadFrom((Join-Path $probeDir 'OpenTK.GLControl.dll'))
[void][Reflection.Assembly]::LoadFrom($gdsDll)
Add-Type -AssemblyName System.Windows.Forms

$helper = @"
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using NexplantQMS.GdsMap;
using NexplantQMS.GdsMap.Chain;
using NexplantQMS.GdsMap.Persistence;

public static class ChainLoadProbe
{
    const BindingFlags NP = BindingFlags.NonPublic | BindingFlags.Instance;

    public static void DrawDb(GdsMapControl map, string factory, string device, int[] layers)
    {
        var bsl = new DACrux.SEMDMS.BSL.GdsMapView();
        var items = new List<MapPlacedElementData>();
        foreach (int layer in layers)
        {
            string after = string.Empty;
            while (true)
            {
                DataTable page = bsl.GetPlacedElementPage(factory, device, GdsLayerKey.Format(layer), after, 10000);
                foreach (DataRow row in page.Rows) items.Add(GdsMapPlacedRowReader.Read(row));
                if (page.Rows.Count > 0) after = page.Rows[page.Rows.Count - 1]["PLACED_ELEMENT_ID"].ToString();
                if (page.Rows.Count < 10000) break;
            }
        }
        map.ShowPlacedElements("TOP", layers.ToDictionary(id => id, id => Color.Gray), items);
        map.RebuildChainTraceItems();
    }

    static string G(double v) { return v.ToString("G17", CultureInfo.InvariantCulture); }

    /// <summary>
    /// Layer 2의 첫 도형을 Input으로 Layer 2를 탐색하고, 결과로 저장 행을 만들어 BSL로 저장한다.
    /// TRACE = 탐색 후보(단자 제외) 중 마지막 1개를 뺀 나머지(EXCLUDE 기록), MANUAL = Layer 6의 첫 도형(ADD 기록).
    /// 반환: CHAIN_SEQ|저장 Member 행(정렬, 0x1E 구분)|저장 편집 행
    /// </summary>
    public static string SaveSample(GdsMapControl map, string factory, string device)
    {
        string input = null, manual = null;
        map.VisitPlacedElements(new HashSet<int> { GdsLayerKey.Make(2, 0), GdsLayerKey.Make(6, 0) }, d =>
        {
            if (d.ElementType != "BOUNDARY" && d.ElementType != "PATH") return;
            if (d.LayerId == 2 && input == null) input = d.PlacedElementId;
            if (d.LayerId == 6 && manual == null) manual = d.PlacedElementId;
        });
        ChainTraceResult r = map.TraceChainCandidate(new[] { input }, new string[0], new[] { GdsLayerKey.Make(2, 0) }, new ChainLayerConnectionRule[0], false, 10.0);
        List<string> auto = (r.IsConnected ? r.CandidateElements : r.VisitedElements.Concat(r.EndpointElements))
            .Select(e => e.ElementKey).Where(k => k != input).Distinct().OrderBy(k => k, StringComparer.Ordinal).ToList();
        string excluded = auto.Last();
        auto.Remove(excluded);
        // 화면 클릭 좌표는 ScreenToWorld -> new GPoint에서 소수 5자리로 반올림되므로 같은 형식의 값을 쓴다.
        double mx = new GPoint(Math.PI * 100, -1.0 / 7).X, my = new GPoint(Math.PI * 100, -1.0 / 7).Y;
        var members = new List<string[]> { new[] { input, "INPUT", "ENDPOINT", G(mx), G(my) } };
        members.AddRange(auto.Select(k => new[] { k, "MEMBER", "TRACE", "", "" }));
        members.Add(new[] { manual, "MEMBER", "MANUAL", "", "" });
        members = members.OrderBy(m => m[0], StringComparer.Ordinal).ToList();
        var edits = new List<string[]> { new[] { "1", manual, "ADD", "" }, new[] { "2", excluded, "EXCLUDE", "" } };

        var bsl = new DACrux.SEMDMS.BSL.GdsChain();
        foreach (DataRow row in bsl.GetChainList(factory, device).Rows)
            if (row["CHAIN_CODE"].ToString().StartsWith("ZZLOAD")) bsl.DeleteChain(factory, device, row["CHAIN_SEQ"].ToString(), "probe");
        string[] info = { factory, device, "", "ZZLOAD1", "Load Probe", Color.Orange.ToArgb().ToString(), "1", "12.5", "probe" };
        // 화면 저장과 같은 형식: 탐색 Layer = LAYER_ID / DATA_TYPE, 규칙 = 첫 Layer / DataType / 둘째 Layer / DataType / 허용 오차 (2026-10-20)
        string seq = bsl.BeginChainSave(info, To2D(new List<string[]> { new[] { "2", "0" } }, 2),
            To2D(new List<string[]> { new[] { "2", "0", "1", "0", "0.25" } }, 5)).Rows[0]["CHAIN_SEQ"].ToString();
        string[] key = { factory, device, seq };
        bsl.CreateChainMemberBatch(key, To2D(members, 5), "probe");
        bsl.CreateChainEditBatch(key, To2D(edits, 4), "probe");
        DataRow c = bsl.CompleteChainSave(new[] { factory, device, seq, members.Count.ToString(), edits.Count.ToString() }, "probe").Rows[0];
        if (c["RESULT"].ToString() != "OK") throw new Exception("저장 확정 실패: " + c["MESSAGE"]);
        return seq + "|" + Join(members) + "|" + Join(edits);
    }

    static string Join(IEnumerable<string[]> rows) { return string.Join("\u001E", rows.Select(r => string.Join("\u001F", r))); }

    static string[,] To2D(List<string[]> rows, int cols)
    {
        var a = new string[rows.Count, cols];
        for (int i = 0; i < rows.Count; i++) for (int c = 0; c < cols; c++) a[i, c] = rows[i][c];
        return a;
    }

    /// <summary>
    /// 화면 복원 코드로 Chain을 만들고 저장 행을 다시 만든다.
    /// 반환: Member 행|편집 행|DbIncomplete|SavedFingerprint==Fingerprint|Problems|LayerIds|규칙|이름/색/옵션/격자
    /// </summary>
    public static string LoadBack(GdsMapViewForm form, string factory, string device, string seq)
    {
        Type ft = typeof(GdsMapViewForm);
        var bsl = new DACrux.SEMDMS.BSL.GdsChain();
        DataRow header = bsl.GetChainList(factory, device).Rows.Cast<DataRow>().First(r => r["CHAIN_SEQ"].ToString() == seq);
        Type dataType = ft.GetNestedType("ChainDbData", BindingFlags.NonPublic);
        object data = Activator.CreateInstance(dataType, true);
        dataType.GetField("Header").SetValue(data, header);
        dataType.GetField("Layers").SetValue(data, bsl.GetChainLayers(factory, device, seq));
        dataType.GetField("Rules").SetValue(data, bsl.GetChainRules(factory, device, seq));
        dataType.GetField("Members").SetValue(data, bsl.GetChainMembers(factory, device, seq));
        dataType.GetField("Edits").SetValue(data, bsl.GetChainEdits(factory, device, seq));
        object chain = ft.GetMethod("CreateChainFromDb", NP).Invoke(form, new[] { data });
        object save = ft.GetMethod("BuildChainSaveData", NP).Invoke(form, new[] { chain });
        Type ct = chain.GetType(), st = save.GetType();
        var members = ((List<string[]>)st.GetField("Members").GetValue(save)).OrderBy(m => m[0], StringComparer.Ordinal);
        var edits = (List<string[]>)st.GetField("Edits").GetValue(save);
        string fp = (string)st.GetField("Fingerprint").GetValue(save);
        string saved = (string)ct.GetField("SavedFingerprint").GetValue(chain);
        var problems = (List<string>)st.GetField("Problems").GetValue(save);
        var layers = ((HashSet<int>)ct.GetField("LayerIds").GetValue(chain)).OrderBy(i => i);
        var rules = (List<ChainLayerConnectionRule>)ct.GetField("LayerRules").GetValue(chain);
        string opt = ct.GetField("Name").GetValue(chain) + "/" + ((Color)ct.GetField("Color").GetValue(chain)).ToArgb() + "/"
            + ct.GetField("ApplyLayerRules").GetValue(chain) + "/" + ct.GetField("CellSize").GetValue(chain);
        return Join(members) + "|" + Join(edits) + "|" + (ct.GetField("DbIncomplete").GetValue(chain) ?? "") + "|" + (fp == saved)
            + "|" + string.Join(";", problems) + "|" + string.Join(",", layers.Select(GdsLayerKey.Format)) + "|"
            + string.Join(",", rules.Select(x => GdsLayerKey.Format(x.FirstLayerId) + "-" + GdsLayerKey.Format(x.SecondLayerId) + ":" + x.Tolerance.ToString(CultureInfo.InvariantCulture))) + "|" + opt;
    }
}
"@
Add-Type -TypeDefinition $helper -ReferencedAssemblies @($gdsDll, (Join-Path $BinDir 'DACrux.SEMDMS.BSL.dll'), (Join-Path $BinDir 'DACrux.SEMDMS.Interface.dll'),
    (Join-Path $serverBin 'Miracom.Middleware.dll'), (Join-Path $probeDir 'OpenTK.GLControl.dll'), (Join-Path $probeDir 'OpenTK.dll'),
    'System.Core', 'System.Data', 'System.Windows.Forms', 'System.Drawing', 'System.Xml')

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
$inst = [Reflection.BindingFlags]'NonPublic,Instance'

# 1. 전체 Layer로 그린 View 화면 지도에서 탐색한 Chain 저장
$form = New-Object NexplantQMS.GdsMap.GdsMapViewForm($false)
$map = [NexplantQMS.GdsMap.GdsMapViewForm].GetField('map', $inst).GetValue($form)
$key = [NexplantQMS.GdsMap.GdsLayerKey]
[ChainLoadProbe]::DrawDb($map, $Factory, $Device, [int[]]@($key::Make(1, 0), $key::Make(2, 0), $key::Make(6, 0)))
$saved = ([ChainLoadProbe]::SaveSample($map, $Factory, $Device)).Split('|')
$seq = $saved[0]
$memberCount = $saved[1].Split([char]0x1E).Count
Check '저장(INPUT / TRACE / MANUAL / EXCLUDE)' ($memberCount -gt 2) "seq=$seq member=$memberCount"

# 2. 같은 지도에서 화면 복원 -> 저장 행 재생성 = 처음 저장한 행
$back = ([ChainLoadProbe]::LoadBack($form, $Factory, $Device, $seq)).Split('|')
$diffRows = @(Compare-Object @($saved[1].Split([char]0x1E)) @($back[0].Split([char]0x1E)) | ForEach-Object { $_.SideIndicator + ' ' + ($_.InputObject -replace [char]0x1F, ' ') })
Check '복원 후 Member 행 왕복 일치(역할 / 출처 / Marker 비트)' ($back[0] -eq $saved[1]) ("member=$($back[0].Split([char]0x1E).Count) " + ($diffRows -join ' / '))
Check '복원 후 편집 행 왕복 일치(ADD / EXCLUDE)' ($back[1] -eq $saved[2]) ($back[1] -replace [char]0x1F, ' ' -replace [char]0x1E, ' / ')
Check '완전 복원 / 변경 없음 지문 / 저장 문제 없음' ($back[2] -eq '' -and $back[3] -eq 'True' -and $back[4] -eq '') "incomplete='$($back[2])' fp=$($back[3]) problems='$($back[4])'"
Check 'Layer / 규칙 / 이름·색·옵션·격자' ($back[5] -eq '2/0' -and $back[6] -eq '1/0-2/0:0.25' -and $back[7] -eq ('Load Probe/' + [Drawing.Color]::Orange.ToArgb() + '/True/12.5')) "layers=$($back[5]) rule=$($back[6]) opt=$($back[7])"
$form.Dispose()

# 3. Layer 1만 그린 지도에서 복원 -> Input / TRACE(Layer 2) 미복원 -> DbIncomplete + 저장 제외 (수동 추가 도형은 ID만 보존하므로 미복원으로 세지 않음)
$form2 = New-Object NexplantQMS.GdsMap.GdsMapViewForm($false)
$map2 = [NexplantQMS.GdsMap.GdsMapViewForm].GetField('map', $inst).GetValue($form2)
[ChainLoadProbe]::DrawDb($map2, $Factory, $Device, [int[]]@($key::Make(1, 0)))
$part = ([ChainLoadProbe]::LoadBack($form2, $Factory, $Device, $seq)).Split('|')
Check 'Layer 일부만 조회하면 미복원 표시 + 저장 제외' ($part[2] -like 'Layer 2/0 미조회*' -and $part[4] -like '*미조회*') "incomplete='$($part[2])'"
$form2.Dispose()

# 4. 정리
[void](New-Object DACrux.SEMDMS.BSL.GdsChain).DeleteChain($Factory, $Device, $seq, 'probe')
Write-Host "정리: Chain $seq 삭제"

$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host 'GDS_CHAIN_LOAD_PROBE=PASS' } else { Write-Host "GDS_CHAIN_LOAD_PROBE=FAIL ($fail)"; exit 1 }
