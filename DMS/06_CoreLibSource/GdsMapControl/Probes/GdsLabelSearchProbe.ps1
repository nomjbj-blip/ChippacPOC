# GDS Map View Label 검색 검증 스크립트 (CLAUDE-032, 설계: 문서/2026-10-07_GDS_Label_검색_설계.md 6절 1~2단계)
# 목적: 서비스 GetMapTextIndex(BSL 직접 호출)로 받은 Label 색인이 샘플 GDS 파일의 배치 TEXT와 같은지,
#       검색 규칙(정확히 / 시작 / 포함, 대소문자 무시, 앞뒤 공백 제거)과 View 화면 검색(결과 목록 / 첫 건 위치로 이동 / Back 보기 X 반전)을 확인한다.
#       2026-10-07 지도 검색 표시(과녁 / 글자) 제거 -> 이동 확인은 지도 화면 중심(GdsMapControl._offset)으로 한다.
#       화면은 띄우지 않고 GdsMapViewForm을 만들어 private 검색 메서드를 리플렉션으로 호출한다(RO 원격 호출 / 실제 그리기는 DMS 화면에서 확인).
# 전제: FACTORY=PROBE / DEVICE_ID=FULL_PROBE READY Map(GdsImportFullProbe.ps1로 샘플 전체 Layer 등록). DB를 바꾸지 않는다(조회만).
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsLabelSearchProbe.ps1 -GdsExe <빌드한 NexplantQMS.GdsMap.exe> [-BinDir <BSL/DSL/Interface 폴더>]
#       -BinDir 생략 시 101_ServerBin. 배포 전에는 TEMP 서버 빌드 폴더를 넘긴다.
param(
    [Parameter(Mandatory = $true)][string]$GdsExe,
    [string]$BinDir = '',
    [string]$Factory = 'PROBE',
    [string]$Device = 'FULL_PROBE',
    [string]$GdsPath = ''
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$serverBin = Join-Path $dms '101_ServerBin'
if ($BinDir -eq '') { $BinDir = $serverBin }
if ($GdsPath -eq '') { $GdsPath = Join-Path $dms '..\SampleFile\OMM all layer2.gds\OMM all layer2.gds' }
$GdsPath = (Resolve-Path $GdsPath).Path

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
$probeDir = Join-Path $env:TEMP 'claude\gdslabelsearchprobe'
New-Item -ItemType Directory -Force $probeDir | Out-Null
Copy-Item $GdsExe (Join-Path $probeDir 'NexplantQMS.GdsMap.dll') -Force
foreach ($d in 'OpenTK.dll', 'OpenTK.GLControl.dll') { Copy-Item (Join-Path (Split-Path $GdsExe) $d) $probeDir -Force }
$gdsDll = Join-Path $probeDir 'NexplantQMS.GdsMap.dll'
foreach ($d in 'OpenTK.dll', 'OpenTK.GLControl.dll') { [void][Reflection.Assembly]::LoadFrom((Join-Path $probeDir $d)) }
[void][Reflection.Assembly]::LoadFrom($gdsDll)
Add-Type -AssemblyName System.Windows.Forms
Write-Host "BSL: $(Join-Path $BinDir 'DACrux.SEMDMS.BSL.dll') / GdsMap: $GdsExe"

$helper = @"
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using NexplantQMS.GdsMap;
using NexplantQMS.GdsMap.Persistence;

public static class LabelSearchProbe
{
    const BindingFlags NP = BindingFlags.NonPublic | BindingFlags.Instance;

    static string G(double v) { return Math.Round(v, 6).ToString("0.######", CultureInfo.InvariantCulture); }

    public static DataTable ReadIndex(string factory, string device)
    {
        return new DACrux.SEMDMS.BSL.GdsMapView().GetMapTextIndex(factory, device);
    }

    /// <summary>파일의 배치 TEXT를 색인과 같은 형식(값|Layer|X|Y, 정렬)으로 만든다.</summary>
    public static List<string> FileTexts(string path)
    {
        var lib = new GdsReader { LengthUnit = LengthUnit.Micrometer }.Read(path);
        var map = new GdsMapControl();
        map.ShowStructure(lib);
        var list = new List<string>();
        map.VisitPlacedElements(null, false, d => { if (d.ElementType == "TEXT") list.Add((d.Text ?? "") + "|" + d.LayerId + "|" + G(d.WorldPoints[0].X) + "|" + G(d.WorldPoints[0].Y)); });
        list.Sort(StringComparer.Ordinal);
        return list;
    }

    public static List<string> IndexTexts(GdsLabelIndex index)
    {
        var list = index.Entries.Select(e => e.Text + "|" + e.LayerId + "|" + G(e.StoredX) + "|" + G(e.Y)).ToList();
        list.Sort(StringComparer.Ordinal);
        return list;
    }

    public static string Describe(IEnumerable<GdsLabelEntry> list)
    {
        return string.Join(" / ", list.Select(e => e.Text + " L" + e.LayerId + " (" + G(e.StoredX) + ", " + G(e.Y) + ")"));
    }

    /// <summary>
    /// View 화면 검색: 보기 기준 side로 Map을 그렸다고 두고(_shownMap / _displaySide) 색인을 넣은 뒤 txtLabel = text로 RunLabelSearch.
    /// 반환: 결과 목록 문구(0x1E 구분) + "#" + 이동 후 지도 화면 중심 + "#" + 선택 index + "#" + lblLabelResult + "#" + 상태줄
    /// </summary>
    public static string FormSearch(DataRow mapRow, GdsLabelIndex index, string side, string text, int matchIndex)
    {
        using (var form = new GdsMapViewForm())
        {
            Type t = typeof(GdsMapViewForm);
            Type mapItemType = t.GetNestedType("MapItem", BindingFlags.NonPublic);
            object item = Activator.CreateInstance(mapItemType, BindingFlags.Public | BindingFlags.Instance, null, new object[] { mapRow }, null);
            t.GetField("_shownMap", NP).SetValue(form, item);
            t.GetField("_displaySide", NP).SetValue(form, side);
            t.GetMethod("ReplaceLabelIndex", NP).Invoke(form, new object[] { index, null });
            ((TextBox)t.GetField("txtLabel", NP).GetValue(form)).Text = text;
            ((ComboBox)t.GetField("cboLabelMatch", NP).GetValue(form)).SelectedIndex = matchIndex;
            t.GetMethod("RunLabelSearch", NP).Invoke(form, null);
            var results = (ComboBox)t.GetField("cboLabelResults", NP).GetValue(form);
            var map = (GdsMapControl)t.GetField("map", NP).GetValue(form);
            object offset = typeof(GdsMapControl).GetField("_offset", NP).GetValue(map);
            float cx = (float)offset.GetType().GetField("X").GetValue(offset), cy = (float)offset.GetType().GetField("Y").GetValue(offset);
            int selected = results.SelectedIndex;
            string status = ((ToolStripStatusLabel)t.GetField("lblStatus", NP).GetValue(form)).Text;
            string resultText = ((Label)t.GetField("lblLabelResult", NP).GetValue(form)).Text;
            return string.Join("\u001e", results.Items.Cast<object>().Select(o => o.ToString())) + "#"
                + (results.Items.Count == 0 ? "" : "(" + Math.Round(cx, 2).ToString(CultureInfo.InvariantCulture) + "," + Math.Round(cy, 2).ToString(CultureInfo.InvariantCulture) + ")")
                + "#" + selected + "#" + resultText + "#" + status;
        }
    }
}
"@
$refs = @($gdsDll, (Join-Path $probeDir 'OpenTK.dll'), (Join-Path $probeDir 'OpenTK.GLControl.dll'), (Join-Path $BinDir 'DACrux.SEMDMS.BSL.dll'),
    (Join-Path $BinDir 'DACrux.SEMDMS.Interface.dll'), (Join-Path $serverBin 'Miracom.Middleware.dll'), 'System.Windows.Forms', 'System.Drawing', 'System.Data', 'System.Core', 'System.Xml')
Add-Type -ReferencedAssemblies $refs -TypeDefinition $helper

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}

# 1. 서비스 색인 조회(BSL 직접)
$watch = [Diagnostics.Stopwatch]::StartNew()
$table = [LabelSearchProbe]::ReadIndex($Factory, $Device)
$readMs = $watch.Elapsed.TotalMilliseconds
$index = [NexplantQMS.GdsMap.Persistence.GdsLabelIndex]::FromTable($table)
$layers = ($index.Entries | ForEach-Object { $_.LayerId } | Sort-Object -Unique) -join ','
Check '색인 조회' ($index.Count -eq 30992) ("TEXT {0:N0}행 / Layer {1} / 조회 {2:N0}ms / 열 {3}" -f $index.Count, $layers, $readMs, (($table.Columns | ForEach-Object { $_.ColumnName }) -join ','))

# 2. 파일 TEXT와 같은지(값 / Layer / 위치)
$fileTexts = [LabelSearchProbe]::FileTexts($GdsPath)
$dbTexts = [LabelSearchProbe]::IndexTexts($index)
$diff = 0; for ($i = 0; $i -lt [Math]::Min($fileTexts.Count, $dbTexts.Count); $i++) { if ($fileTexts[$i] -ne $dbTexts[$i]) { $diff++; if ($diff -le 3) { Write-Host "  다름: 파일 $($fileTexts[$i]) / DB $($dbTexts[$i])" } } }
Check '파일 TEXT 일치' ($fileTexts.Count -eq $dbTexts.Count -and $diff -eq 0) ("파일 {0:N0} / DB {1:N0} / 다른 행 {2}" -f $fileTexts.Count, $dbTexts.Count, $diff)

# 3. 검색 규칙
$exact = $index.Search('C4DC_212_71', [NexplantQMS.GdsMap.Persistence.GdsLabelMatchMode]::Exact)
$exactText = [LabelSearchProbe]::Describe($exact)
Check '정확히 C4DC_212_71' ($exact.Count -eq 2 -and $exactText -eq 'C4DC_212_71 L775 (10800, 14582.208) / C4DC_212_71 L775 (10960, 14582.208)') $exactText
$lower = $index.Search('  c4dc_212_71 ', [NexplantQMS.GdsMap.Persistence.GdsLabelMatchMode]::Exact)
Check '대소문자 / 공백 무시' ([LabelSearchProbe]::Describe($lower) -eq $exactText) "'  c4dc_212_71 ' -> $($lower.Count)건"
# C4DC_212_7 은 실제로 있는 다른 Label 이므로, 형식상 없는 앞부분(C4DC_212)으로 확인한다.
$partial = $index.Search('C4DC_212', [NexplantQMS.GdsMap.Persistence.GdsLabelMatchMode]::Exact)
$partialFile = @($fileTexts | Where-Object { $_.Split('|')[0] -eq 'C4DC_212' }).Count
Check '정확히는 앞부분만으로 찾지 않음' ($partial.Count -eq 0 -and $partialFile -eq 0) "C4DC_212 -> 색인 $($partial.Count)건 / 파일 $($partialFile)건"
$prefixKey = 'C4DC_212_'
$prefix = $index.Search($prefixKey, [NexplantQMS.GdsMap.Persistence.GdsLabelMatchMode]::Prefix)
$prefixFile = @($fileTexts | Where-Object { $_.StartsWith($prefixKey, [StringComparison]::OrdinalIgnoreCase) }).Count
Check '시작 C4DC_212_' ($prefix.Count -eq $prefixFile -and $prefix.Count -gt 2) "색인 $($prefix.Count)건 / 파일 $($prefixFile)건"
$contains = $index.Search('htr_stk', [NexplantQMS.GdsMap.Persistence.GdsLabelMatchMode]::Contains)
$containsFile = @($fileTexts | Where-Object { $_.Split('|')[0].IndexOf('HTR_STK', [StringComparison]::OrdinalIgnoreCase) -ge 0 }).Count
Check '포함 htr_stk' ($contains.Count -eq $containsFile -and $contains.Count -ge 4) "색인 $($contains.Count)건 / 파일 $($containsFile)건"
$watch.Restart(); for ($i = 0; $i -lt 100; $i++) { [void]$index.Search('C4DC_212_71', [NexplantQMS.GdsMap.Persistence.GdsLabelMatchMode]::Contains) }
$searchMs = $watch.Elapsed.TotalMilliseconds / 100
Check '검색 시간' ($searchMs -lt 200) ("포함 검색 1회 {0:0.0}ms (3만 건)" -f $searchMs)

# 4. View 화면 검색(Front / Back 보기)
$mapRow = (New-Object DACrux.SEMDMS.BSL.GdsMapView).GetMapInfo($Factory, $Device).Rows[0]
$front = ([LabelSearchProbe]::FormSearch($mapRow, $index, 'FRONT', 'C4DC_212_71', 0)).Split('#')
Write-Host "  Front 목록: $($front[0] -replace [string][char]0x1e, ' || ')"
Check 'Front 보기 결과 / 첫 건 이동' ($front[1] -eq '(10800,14582.21)' -and $front[2] -eq '0' -and $front[0].Split([char]0x1e).Count -eq 2) "화면 중심 $($front[1]) / 선택 $($front[2]) / $($front[3]) / 상태: $($front[4])"
$back = ([LabelSearchProbe]::FormSearch($mapRow, $index, 'BACK', 'C4DC_212_71', 0)).Split('#')
Check 'Back 보기 X 반전' ($back[1] -eq '(-10800,14582.21)' -and $back[0].Contains('(-10800, 14582.208)')) "화면 중심 $($back[1]) / 목록 $($back[0].Split([char]0x1e)[0])"
$none = ([LabelSearchProbe]::FormSearch($mapRow, $index, 'FRONT', 'NO_SUCH_LABEL_X', 0)).Split('#')
Check '0건 안내' ($none[1] -eq '' -and $none[3].Contains('0건') -and $none[4].Contains('시작 / 포함')) "$($none[3]) / 상태: $($none[4])"
$many = ([LabelSearchProbe]::FormSearch($mapRow, $index, 'FRONT', 'C4DC', 1)).Split('#')
$listCount = $many[0].Split([char]0x1e).Count
Check '많은 결과 목록 제한' ($listCount -eq 1000 -and $many[3].Contains('외')) "목록 $($listCount)건 / $($many[3])"

$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host "GDS_LABEL_SEARCH_PROBE=PASS ($($results.Count))" } else { Write-Host "GDS_LABEL_SEARCH_PROBE=FAIL ($fail)"; exit 1 }
