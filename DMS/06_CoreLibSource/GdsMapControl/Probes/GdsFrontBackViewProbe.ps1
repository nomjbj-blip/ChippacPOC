# GDS Map View Front / Back 보기 전환 검증 스크립트 (설계: 문서/설계/02_GDS/2026-10-07_GDS_FrontBack_전환_설계.md 4 / 5.2절)
# 목적: DB는 저장 기준(FRONT) 1벌이고 View에서 로직으로 Back을 보여 주는 방식이 맞게 동작하는지 확인한다.
#       1) GdsSideTransform: 두 번 반전 = 원본(비트), 반전 X = -X, NeedsMirror 규칙
#       2) Back 보기(DB 도형 반전)에서 Chain 탐색 도형 ID 집합 = Front 보기 탐색 집합
#       3) Front 기준으로 저장한 Chain을 Back 보기 화면에서 불러오기(CreateChainFromDb): Marker X = -저장값, 탐색 결과 도형 좌표 = 화면 도형
#          -> 저장 행 재생성(BuildChainSaveData): Marker = 저장값(비트) / 저장 지문 = "변경 없음"
#       4) 보기 전환 처리(MirrorChainsForSideChange + 보기 기준 FRONT): Marker = 저장값, 저장 행 / 지문 동일
#       화면 private 메서드를 리플렉션으로 호출한다. BSL 직접 호출(Service Remoting 아님).
# 전제: FACTORY=PROBE / DEVICE_ID=LAYER_FILTER_PROBE READY Map(Layer 1, 2, 6, VIEW_SIDE FRONT). 없으면 GdsImportLayerFilterProbe.ps1 -Keep.
#       Chain Code ZZSIDE1을 만들었다가 마지막에 삭제한다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsFrontBackViewProbe.ps1 -GdsExe <빌드한 NexplantQMS.GdsMap.exe>
param(
    [Parameter(Mandatory = $true)][string]$GdsExe,
    [string]$Factory = 'PROBE',
    [string]$Device = 'LAYER_FILTER_PROBE',
    [string]$BinDir = ''
)
# -BinDir 생략 시 101_ServerBin. 화면 / 탐색 / 페이지 조회는 Layer 키(번호 + DataType)로 다룬다 (2026-10-20)
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
$probeDir = Join-Path $env:TEMP 'chippac\gdsfrontbackprobe'
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
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using NexplantQMS.GdsMap;
using NexplantQMS.GdsMap.Chain;
using NexplantQMS.GdsMap.Persistence;

public static class FrontBackProbe
{
    const BindingFlags NP = BindingFlags.NonPublic | BindingFlags.Instance;
    static long B(double v) { return BitConverter.DoubleToInt64Bits(v); }

    public static List<MapPlacedElementData> ReadDb(string factory, string device, int[] layers)
    {
        var view = new DACrux.SEMDMS.BSL.GdsMapView();
        var items = new List<MapPlacedElementData>();
        foreach (int layer in layers)
        {
            string after = string.Empty;
            while (true)
            {
                DataTable page = view.GetPlacedElementPage(factory, device, GdsLayerKey.Format(layer), after, 10000);
                foreach (DataRow row in page.Rows) items.Add(GdsMapPlacedRowReader.Read(row));
                if (page.Rows.Count > 0) after = page.Rows[page.Rows.Count - 1]["PLACED_ELEMENT_ID"].ToString();
                if (page.Rows.Count < 10000) break;
            }
        }
        return items;
    }

    /// <summary>1) 변환식. 반환: 도형|좌표|두번반전불일치|X불일치|NeedsMirror(F,F)(F,B)(B,F)(B,B)(null,B)</summary>
    public static string CheckTransform(List<MapPlacedElementData> items)
    {
        List<MapPlacedElementData> once = GdsSideTransform.MirrorAll(items), twice = GdsSideTransform.MirrorAll(once);
        long pts = 0, badTwice = 0, badX = 0;
        for (int i = 0; i < items.Count; i++)
            for (int k = 0; k < items[i].WorldPoints.Length; k++)
            {
                pts++;
                if (B(twice[i].WorldPoints[k].X) != B(items[i].WorldPoints[k].X) || B(twice[i].WorldPoints[k].Y) != B(items[i].WorldPoints[k].Y)) badTwice++;
                if (B(once[i].WorldPoints[k].X) != B(-items[i].WorldPoints[k].X)) badX++;
            }
        string rule = "" + GdsSideTransform.NeedsMirror("FRONT", "FRONT") + GdsSideTransform.NeedsMirror("FRONT", "BACK")
            + GdsSideTransform.NeedsMirror("BACK", "FRONT") + GdsSideTransform.NeedsMirror("BACK", "BACK") + GdsSideTransform.NeedsMirror(null, "BACK");
        return items.Count + "|" + pts + "|" + badTwice + "|" + badX + "|" + rule;
    }

    public static string Trace(GdsMapControl map, string input, int layer)
    {
        return string.Join(",", map.TraceChainCandidate(new[] { input }, new string[0], new[] { layer }, new ChainLayerConnectionRule[0], false, 10.0)
            .VisitedElements.Select(e => e.ElementKey).OrderBy(k => k, StringComparer.Ordinal));
    }

    static string G(double v) { return v.ToString("G17", CultureInfo.InvariantCulture); }
    static string[,] To2D(List<string[]> rows) { int c = rows[0].Length; var a = new string[rows.Count, c]; for (int i = 0; i < rows.Count; i++) for (int j = 0; j < c; j++) a[i, j] = rows[i][j]; return a; }

    /// <summary>Front 지도에서 Layer 2 탐색 결과로 Chain을 저장(Marker = 클릭 좌표 형식). 반환: seq|input|markerX|markerY</summary>
    public static string SaveFrontChain(GdsMapControl front, string factory, string device)
    {
        string input = null;
        int layer2 = GdsLayerKey.Make(2, 0);
        front.VisitPlacedElements(new HashSet<int> { layer2 }, false, d => { if (input == null && (d.ElementType == "BOUNDARY" || d.ElementType == "PATH")) input = d.PlacedElementId; });
        var trace = front.TraceChainCandidate(new[] { input }, new string[0], new[] { layer2 }, new ChainLayerConnectionRule[0], false, 10.0);
        GPoint marker = new GPoint(1234.56789, -98.76543);
        var members = new List<string[]> { new[] { input, "INPUT", "ENDPOINT", G(marker.X), G(marker.Y) } };
        members.AddRange(trace.VisitedElements.Select(e => e.ElementKey).Where(k => k != input).Distinct().Select(k => new[] { k, "MEMBER", "TRACE", "", "" }));
        var bsl = new DACrux.SEMDMS.BSL.GdsChain();
        foreach (DataRow row in bsl.GetChainList(factory, device).Rows)
            if (row["CHAIN_CODE"].ToString().StartsWith("ZZSIDE")) bsl.DeleteChain(factory, device, row["CHAIN_SEQ"].ToString(), "probe");
        string seq = bsl.BeginChainSave(new[] { factory, device, "", "ZZSIDE1", "Side Probe", Color.Gold.ToArgb().ToString(), "0", "10", "probe" },
            To2D(new List<string[]> { new[] { "2" } }), null).Rows[0]["CHAIN_SEQ"].ToString();
        bsl.CreateChainMemberBatch(new[] { factory, device, seq }, To2D(members), "probe");
        DataRow c = bsl.CompleteChainSave(new[] { factory, device, seq, members.Count.ToString(), "0" }, "probe").Rows[0];
        if (c["RESULT"].ToString() != "OK") throw new Exception(c["MESSAGE"].ToString());
        return seq + "|" + input + "|" + G(marker.X) + "|" + G(marker.Y);
    }

    /// <summary>
    /// 3·4) Back 보기 화면에서 불러오기 -> 저장 행 / 지문 -> 보기 전환(FRONT) -> 저장 행 / 지문.
    /// 반환: back화면MarkerX|저장행MarkerX(back)|지문같음(back)|탐색결과좌표=화면도형|전환후MarkerX|저장행MarkerX(front)|지문같음(front)|저장행동일
    /// </summary>
    public static string LoadInBack(GdsMapViewForm form, GdsMapControl map, string factory, string device, string seq, string input)
    {
        Type ft = typeof(GdsMapViewForm);
        var bsl = new DACrux.SEMDMS.BSL.GdsChain();
        var mapInfo = new DACrux.SEMDMS.BSL.GdsMapView().GetMapInfo(factory, device).Rows[0];
        Type mapItemType = ft.GetNestedType("MapItem", BindingFlags.NonPublic);
        object mapItem = Activator.CreateInstance(mapItemType, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { mapInfo }, null);
        ft.GetField("_shownMap", NP).SetValue(form, mapItem);
        ft.GetField("_displaySide", NP).SetValue(form, "BACK");
        map.RebuildChainTraceItems();

        DataRow header = bsl.GetChainList(factory, device).Rows.Cast<DataRow>().First(r => r["CHAIN_SEQ"].ToString() == seq);
        Type dataType = ft.GetNestedType("ChainDbData", BindingFlags.NonPublic);
        object data = Activator.CreateInstance(dataType, true);
        dataType.GetField("Header").SetValue(data, header);
        dataType.GetField("Layers").SetValue(data, bsl.GetChainLayers(factory, device, seq));
        dataType.GetField("Rules").SetValue(data, bsl.GetChainRules(factory, device, seq));
        dataType.GetField("Members").SetValue(data, bsl.GetChainMembers(factory, device, seq));
        dataType.GetField("Edits").SetValue(data, bsl.GetChainEdits(factory, device, seq));
        object chain = ft.GetMethod("CreateChainFromDb", NP).Invoke(form, new[] { data });
        Type ct = chain.GetType();

        Func<double> markerX = () =>
        {
            var inputs = (System.Collections.IDictionary)ct.GetField("Inputs").GetValue(chain);
            object pick = inputs[input];
            return ((GPoint)pick.GetType().GetProperty("Point").GetValue(pick)).X;
        };
        Func<string[]> saveRows = () =>
        {
            object save = ft.GetMethod("BuildChainSaveData", NP).Invoke(form, new[] { chain });
            var members = (List<string[]>)save.GetType().GetField("Members").GetValue(save);
            string fp = (string)save.GetType().GetField("Fingerprint").GetValue(save);
            string[] inputRow = members.First(m => m[1] == "INPUT");
            return new[] { inputRow[3], (fp == (string)ct.GetField("SavedFingerprint").GetValue(chain)).ToString(), string.Join(";", members.Select(m => string.Join(",", m))) };
        };

        double backX = markerX();
        string[] backRows = saveRows();
        // 탐색 결과 도형 좌표가 지금(Back) 화면 도형과 같은지
        var result = (ChainTraceResult)ct.GetField("LastResult").GetValue(chain);
        var shown = map.GetChainTraceElements(result.VisitedElements.Select(e => e.ElementKey)).ToDictionary(e => e.ElementKey, StringComparer.Ordinal);
        bool sameGeometry = result.VisitedElements.All(e => shown.ContainsKey(e.ElementKey) && e.WorldPoints.Length == shown[e.ElementKey].WorldPoints.Length
            && e.WorldPoints.Select((p, i) => B(p.X) == B(shown[e.ElementKey].WorldPoints[i].X)).All(x => x));

        // 보기 전환: Chain 좌표 사본 반전 + 보기 기준 FRONT (지도는 화면 처리에서 다시 그리지만 저장 행 계산에는 영향 없음)
        var chains = (System.Collections.IList)ft.GetField("_chains", NP).GetValue(form);
        chains.Add(chain);
        ft.GetField("_displaySide", NP).SetValue(form, "FRONT");
        ft.GetMethod("MirrorChainsForSideChange", NP).Invoke(form, null);
        double frontX = markerX();
        string[] frontRows = saveRows();
        return G(backX) + "|" + backRows[0] + "|" + backRows[1] + "|" + sameGeometry + "|" + G(frontX) + "|" + frontRows[0] + "|" + frontRows[1] + "|" + (backRows[2] == frontRows[2]);
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
$key = [NexplantQMS.GdsMap.GdsLayerKey]
$layers = [int[]]@($key::Make(1, 0), $key::Make(2, 0), $key::Make(6, 0))
$info = (New-Object DACrux.SEMDMS.BSL.GdsMapView).GetMapInfo($Factory, $Device)
if ($info.Rows.Count -eq 0 -or $info.Rows[0]['MAP_STATUS'] -ne 'READY' -or $info.Rows[0]['VIEW_SIDE'] -ne 'FRONT') { Write-Host "전제 실패: $Factory / $Device FRONT READY Map 없음"; exit 2 }

$items = [FrontBackProbe]::ReadDb($Factory, $Device, $layers)
$t = ([FrontBackProbe]::CheckTransform($items)).Split('|')
Check '변환식: 두 번 반전 = 원본 / X = -X / 규칙' ($t[2] -eq '0' -and $t[3] -eq '0' -and $t[4] -eq 'FalseTrueTrueFalseTrue') "도형 $($t[0]) / 좌표 $($t[1]) / 두번반전 불일치 $($t[2]) / X 불일치 $($t[3]) / 규칙 $($t[4])"

$inst = [Reflection.BindingFlags]'NonPublic,Instance'
$formFront = New-Object NexplantQMS.GdsMap.GdsMapViewForm($false)
$front = [NexplantQMS.GdsMap.GdsMapViewForm].GetField('map', $inst).GetValue($formFront)
$colors = New-Object 'Collections.Generic.Dictionary[int,Drawing.Color]'
foreach ($l in $layers) { $colors[$l] = [Drawing.Color]::Gray }
$front.ShowPlacedElements('TOP', $colors, $items)
$saved = ([FrontBackProbe]::SaveFrontChain($front, $Factory, $Device)).Split('|')
$seq = $saved[0]; $input = $saved[1]

$formBack = New-Object NexplantQMS.GdsMap.GdsMapViewForm($false)
$back = [NexplantQMS.GdsMap.GdsMapViewForm].GetField('map', $inst).GetValue($formBack)
$sw = [Diagnostics.Stopwatch]::StartNew()
$mirrored = [NexplantQMS.GdsMap.Persistence.GdsSideTransform]::MirrorAll($items)
$back.ShowPlacedElements('TOP', $colors, $mirrored)
$switchMs = $sw.ElapsedMilliseconds
$a = [FrontBackProbe]::Trace($front, $input, $key::Make(2, 0)); $b = [FrontBackProbe]::Trace($back, $input, $key::Make(2, 0))
Check 'Back 보기 Chain 탐색 = Front 탐색' ($a -eq $b -and $a.Length -gt 0) ("방문 {0}개 / 반전 + 다시 그리기 {1:N0} ms (도형 {2:N0})" -f $a.Split(',').Count, $switchMs, $items.Count)

$r = ([FrontBackProbe]::LoadInBack($formBack, $back, $Factory, $Device, $seq, $input)).Split('|')
$negX = (-[double]::Parse($saved[2], [Globalization.CultureInfo]::InvariantCulture)).ToString('G17', [Globalization.CultureInfo]::InvariantCulture)
Check 'Back 보기 불러오기: Marker X = -저장값 / 탐색 결과 좌표 = 화면 도형' ($r[0] -eq $negX -and $r[3] -eq 'True') "화면 Marker X $($r[0]) (저장 $($saved[2])) / 좌표 일치 $($r[3])"
Check 'Back 보기 저장 행: Marker = 저장 기준 값 / 변경 없음' ($r[1] -eq $saved[2] -and $r[2] -eq 'True') "저장 행 X $($r[1]) / 변경 없음 $($r[2])"
Check '보기 전환(Front) 후: Marker = 저장값 / 저장 행·지문 동일' ($r[4] -eq $saved[2] -and $r[5] -eq $saved[2] -and $r[6] -eq 'True' -and $r[7] -eq 'True') "화면 X $($r[4]) / 저장 행 X $($r[5]) / 변경 없음 $($r[6]) / 저장 행 동일 $($r[7])"

[void](New-Object DACrux.SEMDMS.BSL.GdsChain).DeleteChain($Factory, $Device, $seq, 'probe')
$formFront.Dispose(); $formBack.Dispose()
Write-Host "정리: Chain $seq 삭제"
$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host 'GDS_FRONT_BACK_VIEW_PROBE=PASS' } else { Write-Host "GDS_FRONT_BACK_VIEW_PROBE=FAIL ($fail)"; exit 1 }
