# Chain 일괄 저장 서비스 검증 스크립트 (2026-10-20)
# 목적: BSL GdsChain.CreateChainBulk / GetChainCodeList / DeleteChainBulk 가 설계대로 동작하는지 본다.
#       설계: 문서/설계/02_GDS/2026-10-20_GDS_Map_View_U8_화면_설계.md 6 ~ 7절 (D4 Code / D5 같은 Code 건너뜀)
#       Service 를 띄우지 않고 BSL 을 직접 호출한다 (DB: 로컬 docker DMSMGR).
# 데이터: Factory PROBE / Device LAYER_FILTER_PROBE(READY Map) 에 시험 Chain(Code ZZBULK_*)을 만들고 끝나면 모두 지운다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsChainBulkProbe.ps1 [-BinDir <BSL/DSL/Interface 폴더>]
param(
    [string]$BinDir = '',
    [string]$Factory = 'PROBE',
    [string]$Device = 'LAYER_FILTER_PROBE'
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
Write-Host "BIN: $BinDir"

# Middleware 가 호출 클래스를 스택에서 찾으므로 BSL 호출은 NoInlining 래퍼 안에서 한다.
$helper = @'
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

public static class ChainBulkProbe
{
    public static string F, D, LayerId, DataType;
    public static List<string> Ids = new List<string>();

    /// <summary>시험 Map 의 첫 Layer 와 도형 ID 를 읽는다. 반환 "Layer / 도형 수"</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string Prepare(string factory, string device)
    {
        F = factory; D = device;
        var view = new DACrux.SEMDMS.BSL.GdsMapView();
        DataRow layer = view.GetMapLayerList(F, D).Rows[0];
        LayerId = layer["LAYER_ID"].ToString();
        DataType = layer.Table.Columns.Contains("DATA_TYPE") ? layer["DATA_TYPE"].ToString() : "0";
        DataTable page = view.GetPlacedElementPage(F, D, LayerId + "/" + DataType, "", 50);
        foreach (DataRow r in page.Rows) Ids.Add(r["PLACED_ELEMENT_ID"].ToString());
        return LayerId + "/" + DataType + " / 도형 " + Ids.Count;
    }

    /// <summary>Chain count 개 x 도형 per 개(첫 도형 INPUT). 도형 ID 는 시험 Map 의 앞쪽 도형을 돌려 쓴다.</summary>
    static void Build(string prefix, int start, int count, int per, out string[,] chains, out string[,] members)
    {
        chains = new string[count, 3];
        members = new string[count * per, 6];
        for (int c = 0; c < count; c++)
        {
            chains[c, 0] = prefix + (start + c);
            chains[c, 1] = "BULK " + (start + c);
            chains[c, 2] = (-16711936).ToString(CultureInfo.InvariantCulture);
            for (int m = 0; m < per; m++)
            {
                int row = c * per + m;
                members[row, 0] = c.ToString(CultureInfo.InvariantCulture);
                members[row, 1] = Ids[(c + m) % Ids.Count];
                members[row, 2] = m == 0 ? "INPUT" : "MEMBER";
                members[row, 3] = m == 0 ? "ENDPOINT" : "TRACE";
                members[row, 4] = m == 0 ? "1.5" : "";
                members[row, 5] = m == 0 ? "2.5" : "";
            }
        }
    }

    static string[] Key() { return new[] { F, D, "0", "10" }; }
    static string[,] Layers() { return new[,] { { LayerId, DataType } }; }

    /// <summary>"CREATED n / SKIPPED m / ms" 또는 "ERROR:메시지"</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string Create(string prefix, int start, int count, int per)
    {
        string[,] chains, members;
        Build(prefix, start, count, per, out chains, out members);
        return Run(chains, members);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static string Run(string[,] chains, string[,] members)
    {
        try
        {
            var sw = Stopwatch.StartNew();
            DataTable r = new DACrux.SEMDMS.BSL.GdsChain().CreateChainBulk(Key(), chains, Layers(), members, "probe");
            return "CREATED " + r.Rows.Cast<DataRow>().Count(x => (string)x["RESULT"] == "CREATED")
                + " / SKIPPED " + r.Rows.Cast<DataRow>().Count(x => (string)x["RESULT"] == "SKIPPED") + " / " + sw.ElapsedMilliseconds + " ms";
        }
        catch (Exception ex) { while (ex.InnerException != null) ex = ex.InnerException; return "ERROR:" + ex.Message.Split('\n')[0]; }
    }

    /// <summary>잘못된 입력 종류별 호출. kind: noinput / dupcode / badindex / badelement</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string Bad(string kind, string prefix, int start)
    {
        string[,] chains, members;
        Build(prefix, start, 2, 3, out chains, out members);
        if (kind == "noinput") members[0, 2] = "MEMBER";
        if (kind == "dupcode") chains[1, 0] = chains[0, 0];
        if (kind == "badindex") members[5, 0] = "9";
        if (kind == "badelement") members[4, 1] = "R0/ZZ_NO_SUCH_ELEMENT";
        return Run(chains, members);
    }

    /// <summary>Code 접두어로 DB 목록. 반환 "개수 / READY 수"</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string List(string prefix)
    {
        var rows = new DACrux.SEMDMS.BSL.GdsChain().GetChainCodeList(F, D).Rows.Cast<DataRow>().Where(r => r["CHAIN_CODE"].ToString().StartsWith(prefix, StringComparison.Ordinal)).ToList();
        return rows.Count + " / READY " + rows.Count(r => r["VERSION_STATUS"].ToString() == "READY");
    }

    /// <summary>Chain 하나의 저장 내용(기존 조회 서비스로): "도형 n / INPUT m / Marker / Layer k / 목록 MEMBER_COUNT"</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string Detail(string code)
    {
        var chain = new DACrux.SEMDMS.BSL.GdsChain();
        DataRow row = chain.GetChainCodeList(F, D).Rows.Cast<DataRow>().First(r => r["CHAIN_CODE"].ToString() == code);
        string seq = row["CHAIN_SEQ"].ToString();
        DataTable members = chain.GetChainMembers(F, D, seq);
        DataRow input = members.Rows.Cast<DataRow>().First(r => r["ROLE_CODE"].ToString() == "INPUT");
        DataRow listRow = chain.GetChainList(F, D).Rows.Cast<DataRow>().First(r => r["CHAIN_SEQ"].ToString() == seq);
        return "도형 " + members.Rows.Count + " / INPUT " + members.Rows.Cast<DataRow>().Count(r => r["ROLE_CODE"].ToString() == "INPUT")
            + " / Marker " + Convert.ToDouble(input["MARKER_X"], CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture)
            + "," + Convert.ToDouble(input["MARKER_Y"], CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture)
            + " / Layer " + chain.GetChainLayers(F, D, seq).Rows.Count + " / 목록 " + listRow["MEMBER_COUNT"] + " " + listRow["VERSION_STATUS"] + " " + row["CHAIN_NAME"];
    }

    /// <summary>Code 접두어 Chain 을 일괄 삭제. 반환 지운 수</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static int DeleteAll(string prefix)
    {
        var chain = new DACrux.SEMDMS.BSL.GdsChain();
        string[] seqs = chain.GetChainCodeList(F, D).Rows.Cast<DataRow>().Where(r => r["CHAIN_CODE"].ToString().StartsWith(prefix, StringComparison.Ordinal))
            .Select(r => r["CHAIN_SEQ"].ToString()).ToArray();
        int total = 0;
        for (int i = 0; i < seqs.Length; i += 10000)
            total += chain.DeleteChainBulk(F, D, seqs.Skip(i).Take(10000).ToArray(), "probe");
        return total;
    }
}
'@
Add-Type -TypeDefinition $helper -ReferencedAssemblies @((Join-Path $BinDir 'DACrux.SEMDMS.BSL.dll'), (Join-Path $BinDir 'DACrux.SEMDMS.Interface.dll'),
    (Join-Path $serverBin 'Miracom.Middleware.dll'), 'System.Data', 'System.Xml', 'System.Core')

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
$P = 'ZZBULK_'
Write-Host ("준비: " + [ChainBulkProbe]::Prepare($Factory, $Device))
try {
    [void][ChainBulkProbe]::DeleteAll($P)
    $r = [ChainBulkProbe]::Create($P, 1, 3, 3); Check '새 Chain 3개 저장' ($r.StartsWith('CREATED 3 / SKIPPED 0')) $r
    $r = [ChainBulkProbe]::List($P); Check 'DB 3개 모두 READY' ($r -eq '3 / READY 3') $r
    $r = [ChainBulkProbe]::Detail("${P}2"); Check '저장 형식: 도형 3 / INPUT 1 / Marker / 탐색 Layer 1 / 기존 목록 조회(GetChainList)에서도 보임' ($r -eq '도형 3 / INPUT 1 / Marker 1.5,2.5 / Layer 1 / 목록 3 READY BULK 2') $r
    $r = [ChainBulkProbe]::Create($P, 1, 3, 3); Check '같은 Code 다시 저장 -> 모두 건너뜀(D5)' ($r.StartsWith('CREATED 0 / SKIPPED 3') -and [ChainBulkProbe]::List($P) -eq '3 / READY 3') $r
    $r = [ChainBulkProbe]::Create($P, 3, 2, 3); Check '있는 Code + 새 Code 섞임 -> 1 건너뜀 / 1 저장' ($r.StartsWith('CREATED 1 / SKIPPED 1') -and [ChainBulkProbe]::List($P) -eq '4 / READY 4') $r
    $r = [ChainBulkProbe]::Bad('noinput', $P, 10); Check 'INPUT 없는 Chain -> 거부(아무것도 저장 안 함)' ($r.StartsWith('ERROR:') -and $r.Contains('INPUT') -and [ChainBulkProbe]::List($P) -eq '4 / READY 4') $r
    $r = [ChainBulkProbe]::Bad('dupcode', $P, 10); Check '같은 호출 안 Code 중복 -> 거부' ($r.StartsWith('ERROR:') -and $r.Contains('두 번')) $r
    $r = [ChainBulkProbe]::Bad('badindex', $P, 10); Check 'CHAIN_INDEX 범위 밖 -> 거부' ($r.StartsWith('ERROR:') -and $r.Contains('CHAIN_INDEX')) $r
    $r = [ChainBulkProbe]::Bad('badelement', $P, 20); Check '없는 도형(FK 오류) -> 오류 + 이번 호출 Chain 정리' ($r.StartsWith('ERROR:') -and [ChainBulkProbe]::List($P) -eq '4 / READY 4') $r
    $r = [ChainBulkProbe]::Create("${P}PERF_", 1, 1000, 10); Check '1,000개 x 도형 10(1만 행) 한 번 저장' ($r.StartsWith('CREATED 1000 / SKIPPED 0')) $r
    $r = [ChainBulkProbe]::List("${P}PERF_"); Check '1,000개 모두 READY' ($r -eq '1000 / READY 1000') $r
    $sw = [Diagnostics.Stopwatch]::StartNew(); $n = [ChainBulkProbe]::DeleteAll($P); $sw.Stop()
    Check '일괄 삭제 -> 남은 시험 Chain 없음' ($n -eq 1004 -and [ChainBulkProbe]::List($P) -eq '0 / READY 0') ("지운 수 $n / $($sw.ElapsedMilliseconds) ms")
}
finally {
    try { [void][ChainBulkProbe]::DeleteAll($P) } catch { }
}
$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host "GDS_CHAIN_BULK_PROBE=PASS ($($results.Count))" } else { Write-Host "GDS_CHAIN_BULK_PROBE=FAIL ($fail)"; exit 1 }
