# GDS Map 복사 검증 스크립트 (2026-10-15)
# 목적: BSL GdsMapImport.CopyMap 이 설계대로 동작하는지 본다. 설계: 문서/설계/02_GDS/2026-10-15_GDS_Map_복사_설계.md (C1~C5)
#       복사한 이름으로 KLARF Product / E-Test Device 가 GDS Device 에 연결되는지(이름이 같은 READY Map, 대소문자 무시, 6절)도 본다.
#       Service 를 띄우지 않고 BSL 을 직접 호출한다 (DB: 로컬 docker DMSMGR).
# 데이터: 원본은 읽기만 한다(기본 SCK / U9, Chain 이 있는 Map). 새 Device COPY_PROBE_* 를 만들고 끝나면 모두 지운다.
#         -FailTarget 을 주면 그 이름으로 복사가 중간에 실패할 때(DB 쪽에서 오류를 일으켜 둔 경우) 새 Device 데이터가 남지 않는지 본다.
#         -KeepCopy 를 주면 복사본(COPY_PROBE_<원본>)을 지우지 않고 남긴다(좌표 BLOB 까지 SQL 로 비교할 때). 다음 실행이 처음에 지운다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsMapCopyProbe.ps1 [-BinDir <BSL/DSL/Interface 폴더>] [-Factory SCK] [-Source U9] [-FailTarget <이름>] [-KeepCopy]
param(
    [string]$BinDir = '',
    [string]$Factory = 'SCK',
    [string]$Source = 'U9',
    [string]$FailTarget = '',
    [switch]$KeepCopy
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
$helper = @"
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

public static class MapCopyProbe
{
    /// <summary>복사로 달라지는 열(새 Device / 새 Chain 번호 / 복사한 사용자 / 시각)은 비교에서 뺀다.</summary>
    static readonly HashSet<string> Skip = new HashSet<string>(new[] { "FACTORY", "DEVICE_ID", "CHAIN_SEQ", "IMPORT_JOB_SEQ", "CREATE_USER", "CREATE_TIME", "UPDATE_USER", "UPDATE_TIME" });

    /// <summary>"OK:Layer/도형/Chain" 또는 "ERROR:메시지"</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string Copy(string f, string src, string dst)
    {
        try
        {
            DataRow r = new DACrux.SEMDMS.BSL.GdsMapImport().CopyMap(f, src, dst, "probe").Rows[0];
            return "OK:" + r["LAYER_COUNT"] + "/" + r["PLACED_COUNT"] + "/" + r["CHAIN_COUNT"] + ":" + r["TARGET_DEVICE_ID"];
        }
        catch (Exception ex) { return "ERROR:" + ex.Message; }
    }

    /// <summary>헤더 / 저장 작업 비교 문자열. 없으면 NONE</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string Info(string f, string d)
    {
        DataTable dt = new DACrux.SEMDMS.BSL.GdsMapImport().GetMapInfo(f, d);
        if (dt.Rows.Count == 0) return "NONE";
        DataRow r = dt.Rows[0];
        return string.Join("|", new[] { "TOP_STRUCTURE", "GDS_FILE_NAME", "MAP_STATUS", "VIEW_SIDE", "JOB_STATUS", "PLACED_COUNT", "POINT_COUNT" }.Select(c => r[c].ToString()).ToArray());
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string Layers(string f, string d) { return Digest(new DACrux.SEMDMS.BSL.GdsMapView().GetMapLayerList(f, d)); }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string ChainList(string f, string d) { return Digest(new DACrux.SEMDMS.BSL.GdsChain().GetChainList(f, d)); }

    /// <summary>Chain 번호 목록(정렬)</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string[] ChainSeqs(string f, string d)
    {
        return new DACrux.SEMDMS.BSL.GdsChain().GetChainList(f, d).Rows.Cast<DataRow>().Select(r => r["CHAIN_SEQ"].ToString()).OrderBy(s => s, StringComparer.Ordinal).ToArray();
    }

    /// <summary>Chain Code 순서로 Chain 마다 탐색 Layer / 연결 규칙 / 포함 도형 / 편집 기록을 요약한다.</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string ChainDetail(string f, string d)
    {
        var svc = new DACrux.SEMDMS.BSL.GdsChain();
        var sb = new StringBuilder();
        foreach (DataRow r in svc.GetChainList(f, d).Rows.Cast<DataRow>().OrderBy(x => x["CHAIN_CODE"].ToString(), StringComparer.Ordinal))
        {
            string seq = r["CHAIN_SEQ"].ToString();
            sb.Append(r["CHAIN_CODE"]).Append(" L").Append(Digest(svc.GetChainLayers(f, d, seq)))
              .Append(" R").Append(Digest(svc.GetChainRules(f, d, seq)))
              .Append(" M").Append(Digest(svc.GetChainMembers(f, d, seq)))
              .Append(" E").Append(Digest(svc.GetChainEdits(f, d, seq))).Append('\n');
        }
        return sb.ToString();
    }

    /// <summary>원본과 결과의 Chain 상세 요약이 같은지, 다르면 다른 첫 줄</summary>
    public static string FirstDiff(string a, string b)
    {
        string[] x = a.Split('\n'), y = b.Split('\n');
        for (int i = 0; i < Math.Max(x.Length, y.Length); i++)
        {
            string p = i < x.Length ? x[i] : "(없음)", q = i < y.Length ? y[i] : "(없음)";
            if (p != q) return p + " <> " + q;
        }
        return "SAME";
    }

    /// <summary>행 수 + 내용 해시(비교 제외 열 빼고, 행 정렬). 큰 결과도 짧게 비교한다.</summary>
    static string Digest(DataTable dt)
    {
        var cols = dt.Columns.Cast<DataColumn>().Where(c => !Skip.Contains(c.ColumnName.ToUpperInvariant())).ToArray();
        var rows = dt.Rows.Cast<DataRow>().Select(r => string.Join("\t", cols.Select(c => Text(r[c])).ToArray())).OrderBy(s => s, StringComparer.Ordinal).ToArray();
        using (var sha = SHA256.Create())
        {
            byte[] h = sha.ComputeHash(Encoding.UTF8.GetBytes(string.Join("\n", rows)));
            return rows.Length + "#" + BitConverter.ToString(h, 0, 6).Replace("-", "");
        }
    }

    static string Text(object o)
    {
        byte[] b = o as byte[];
        return b != null ? Convert.ToBase64String(b) : Convert.ToString(o, System.Globalization.CultureInfo.InvariantCulture);
    }

    /// <summary>도형 없는 DRAFT Map(저장 시작만 하고 확정하지 않음)을 만든다.</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string CreateDraft(string f, string d)
    {
        var imp = new DACrux.SEMDMS.BSL.GdsMapImport();
        if (imp.GetMapInfo(f, d).Rows.Count > 0) imp.DeleteMap(f, d, "probe");
        imp.BeginMapImport(new[] { f, d, "TOP", "probe.gds", "", "0.001", "1E-09", "probe", "0", "1", "1" });
        return imp.GetMapInfo(f, d).Rows[0]["MAP_STATUS"].ToString();
    }

    /// <summary>KLARF Product -> GDS Device (BSL GdsKlarfAnalysis.ResolveMapDevice, 이름이 같은 READY Map)</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string KlarfDevice(string f, string product)
    {
        MethodInfo m = typeof(DACrux.SEMDMS.BSL.GdsKlarfAnalysis).GetMethod("ResolveMapDevice", BindingFlags.NonPublic | BindingFlags.Static);
        return (string)m.Invoke(null, new object[] { f, product });
    }

    /// <summary>E-Test Device -> GDS Device (BSL GdsEtResult.GetEtGdsDevice, 이름이 같은 READY Map)</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string EtDevice(string f, string device)
    {
        return new DACrux.SEMDMS.BSL.GdsEtResult().GetEtGdsDevice(f, device);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void DeleteMap(string f, string d)
    {
        var imp = new DACrux.SEMDMS.BSL.GdsMapImport();
        if (imp.GetMapInfo(f, d).Rows.Count > 0) imp.DeleteMap(f, d, "probe");
    }
}
"@
Add-Type -TypeDefinition $helper -ReferencedAssemblies @((Join-Path $BinDir 'DACrux.SEMDMS.BSL.dll'), (Join-Path $BinDir 'DACrux.SEMDMS.Interface.dll'),
    (Join-Path $serverBin 'Miracom.Middleware.dll'), 'System.Data', 'System.Xml', 'System.Core')

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
$T = 'COPY_PROBE_' + $Source.ToUpperInvariant()
$D = 'COPY_PROBE_DRAFT'
try {
    foreach ($n in $T, $D, "${D}_2") { [MapCopyProbe]::DeleteMap($Factory, $n) }
    if ($FailTarget -ne '') { [MapCopyProbe]::DeleteMap($Factory, $FailTarget) }

    $srcInfo = [MapCopyProbe]::Info($Factory, $Source)
    Check "원본 $Factory / $Source READY" ($srcInfo -ne 'NONE' -and $srcInfo.Split('|')[2] -eq 'READY') $srcInfo
    $srcLayers = [MapCopyProbe]::Layers($Factory, $Source)
    $srcChainList = [MapCopyProbe]::ChainList($Factory, $Source)
    $srcChainDetail = [MapCopyProbe]::ChainDetail($Factory, $Source)
    $srcSeqs = [MapCopyProbe]::ChainSeqs($Factory, $Source)

    $sw = [Diagnostics.Stopwatch]::StartNew()
    $r = [MapCopyProbe]::Copy($Factory, $Source, $T)
    $sw.Stop()
    $parts = $r.Split(':')
    Check "복사 $Source -> $T" ($r.StartsWith('OK:') -and $parts[2] -eq $T) "$r / $([int]$sw.Elapsed.TotalSeconds)초"
    Check '반환 Chain 수 = 원본 Chain 수' ($r.StartsWith('OK:') -and [int]$parts[1].Split('/')[2] -eq $srcSeqs.Count) "$($parts[1]) / 원본 Chain $($srcSeqs.Count)"

    $dstInfo = [MapCopyProbe]::Info($Factory, $T)
    Check '헤더 / 저장 작업 / 도형·좌표 수 같음 + READY (C1, C5)' ($dstInfo -eq $srcInfo) "$dstInfo"
    $v = [MapCopyProbe]::Layers($Factory, $T); Check 'Layer 색 / 이름 같음' ($v -eq $srcLayers) "$srcLayers -> $v"
    $v = [MapCopyProbe]::ChainList($Factory, $T); Check 'Chain 목록(Code / 이름 / 색 / 상태 / 포함·편집 수) 같음 (C2)' ($v -eq $srcChainList) "$srcChainList -> $v"
    $v = [MapCopyProbe]::ChainDetail($Factory, $T)
    Check 'Chain 마다 탐색 Layer / 규칙 / 포함 도형 / 편집 기록 같음' ($v -eq $srcChainDetail) ([MapCopyProbe]::FirstDiff($srcChainDetail, $v))
    $dstSeqs = [MapCopyProbe]::ChainSeqs($Factory, $T)
    $shared = @($dstSeqs | Where-Object { $srcSeqs -contains $_ }).Count
    Check '새 Chain 번호 사용 (원본 번호와 겹치지 않음)' ($dstSeqs.Count -eq $srcSeqs.Count -and $shared -eq 0) "원본 $($srcSeqs -join ',') / 복사 $($dstSeqs -join ',')"

    $lower = $T.ToLowerInvariant()
    $k = [MapCopyProbe]::KlarfDevice($Factory, $lower); $e = [MapCopyProbe]::EtDevice($Factory, $lower)
    Check '복사한 이름(대소문자 무시) -> KLARF Product / E-Test Device 연결' ($k -eq $T -and $e -eq $T) "KLARF $k / E-Test $e"
    $k = [MapCopyProbe]::KlarfDevice($Factory, 'COPY_PROBE_NO_SUCH'); $e = [MapCopyProbe]::EtDevice($Factory, 'COPY_PROBE_NO_SUCH')
    Check '같은 이름 Map 없음 -> 입력 이름 그대로' ($k -eq 'COPY_PROBE_NO_SUCH' -and $e -eq 'COPY_PROBE_NO_SUCH') "KLARF $k / E-Test $e"

    Check '원본 그대로 (헤더 / Chain 번호 / Chain 상세)' (([MapCopyProbe]::Info($Factory, $Source) -eq $srcInfo) -and (([MapCopyProbe]::ChainSeqs($Factory, $Source) -join ',') -eq ($srcSeqs -join ',')) -and ([MapCopyProbe]::ChainDetail($Factory, $Source) -eq $srcChainDetail)) 'Info / Seq / Detail'

    $before = [MapCopyProbe]::Info($Factory, $T)
    $r = [MapCopyProbe]::Copy($Factory, $Source, $T.ToLowerInvariant())
    Check '이미 있는 이름(대소문자만 다름) -> 오류, 덮어쓰지 않음 (C3)' ($r.StartsWith('ERROR:') -and $r.Contains('이미 있는 Device') -and [MapCopyProbe]::Info($Factory, $T) -eq $before -and [MapCopyProbe]::Info($Factory, $T.ToLowerInvariant()) -eq 'NONE') $r
    $r = [MapCopyProbe]::Copy($Factory, $Source, $Source.ToLowerInvariant())
    Check '원본과 같은 이름 -> 오류 (C3)' ($r.StartsWith('ERROR:') -and $r.Contains('원본과 같습니다')) $r
    $r = [MapCopyProbe]::Copy($Factory, $Source, '   ')
    Check '빈 이름 -> 오류 (C3)' ($r.StartsWith('ERROR:')) $r
    $r = [MapCopyProbe]::Copy($Factory, $Source, ('X' * 51))
    Check '50자 초과 -> 오류 (C3)' ($r.StartsWith('ERROR:') -and $r.Contains('50')) $r
    $r = [MapCopyProbe]::Copy($Factory, 'COPY_PROBE_NO_SUCH_MAP', "${D}_2")
    Check '원본 없음 -> 오류' ($r.StartsWith('ERROR:') -and $r.Contains('원본 GDS Map 이 없습니다') -and [MapCopyProbe]::Info($Factory, "${D}_2") -eq 'NONE') $r

    $s = [MapCopyProbe]::CreateDraft($Factory, $D); Check '저장 중(DRAFT) 원본 준비' ($s -eq 'DRAFT') $s
    $k = [MapCopyProbe]::KlarfDevice($Factory, $D.ToLowerInvariant()); $e = [MapCopyProbe]::EtDevice($Factory, $D.ToLowerInvariant())
    Check 'DRAFT Map 은 연결하지 않음 (입력 그대로)' ($k -eq $D.ToLowerInvariant() -and $e -eq $D.ToLowerInvariant()) "KLARF $k / E-Test $e"
    $r = [MapCopyProbe]::Copy($Factory, $D, "${D}_2")
    Check 'DRAFT 원본 -> 오류, 새 Device 없음 (C4)' ($r.StartsWith('ERROR:') -and $r.Contains('DRAFT') -and [MapCopyProbe]::Info($Factory, "${D}_2") -eq 'NONE') $r

    if ($FailTarget -ne '') {
        $r = [MapCopyProbe]::Copy($Factory, $Source, $FailTarget)
        Check "복사 중간 실패 -> 오류 + 새 Device 데이터 정리 (C5)" ($r.StartsWith('ERROR:') -and [MapCopyProbe]::Info($Factory, $FailTarget) -eq 'NONE' -and [MapCopyProbe]::ChainSeqs($Factory, $FailTarget).Count -eq 0) $r
    }

    if ($KeepCopy) { Write-Host "KEEP: $Factory / $T" }
    else {
        [MapCopyProbe]::DeleteMap($Factory, $T)
        Check '복사한 Device 삭제 -> 남은 데이터 없음' ([MapCopyProbe]::Info($Factory, $T) -eq 'NONE' -and [MapCopyProbe]::ChainSeqs($Factory, $T).Count -eq 0) 'Map 삭제'
        Check '복사본 삭제 뒤에도 원본 그대로' ([MapCopyProbe]::Info($Factory, $Source) -eq $srcInfo -and [MapCopyProbe]::ChainDetail($Factory, $Source) -eq $srcChainDetail) $srcInfo
    }
}
finally {
    foreach ($n in $T, $D, "${D}_2") { if ($KeepCopy -and $n -eq $T) { continue }; try { [MapCopyProbe]::DeleteMap($Factory, $n) } catch { } }
    if ($FailTarget -ne '') { try { [MapCopyProbe]::DeleteMap($Factory, $FailTarget) } catch { } }
}

$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host "GDS_MAP_COPY_PROBE=PASS ($($results.Count))" } else { Write-Host "GDS_MAP_COPY_PROBE=FAIL ($fail)"; exit 1 }
