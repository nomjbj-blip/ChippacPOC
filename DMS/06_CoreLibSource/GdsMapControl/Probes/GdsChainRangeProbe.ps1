# Chain 상세 범위 조회 검증 스크립트 (2026-10-21). 설계: 문서/설계/02_GDS/2026-10-21_U8_Chain_View_조회속도_분석.md S1
# 목적: BSL GdsChain.GetChainLayersRange / GetChainRulesRange / GetChainMembersRange / GetChainEditsRange 가
#       기존 Chain 단위 조회(GetChainLayers / Rules / Members / Edits)와 같은 행 / 값을 돌려주는지, Binary 전송 형식인지 본다.
#       Service 를 띄우지 않고 BSL 을 직접 호출한다(DB: 로컬 docker DMSMGR). 조회만 하고 DB 를 바꾸지 않는다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsChainRangeProbe.ps1 [-BinDir <BSL/Interface 폴더>] [-Factory SCK] [-Devices U8,OMM_TCB] [-Sample 300]
param(
    [string]$BinDir = '',
    [string]$Factory = 'SCK',
    [string[]]$Devices = @('U8', 'OMM_TCB'),
    [int]$Sample = 300,
    [int]$RangeSize = 5000
)
$ErrorActionPreference = 'Stop'
# -File 로 실행하면 'U8,OMM_TCB' 가 문자열 하나로 들어오므로 쉼표로 나눈다.
$Devices = @($Devices | ForEach-Object { $_ -split ',' } | Where-Object { $_ -ne '' })
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$serverBin = Join-Path $dms '101_ServerBin'
if ($BinDir -eq '') { $BinDir = $serverBin }
[AppDomain]::CurrentDomain.SetData('APP_CONFIG_FILE', (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config'))
Add-Type -AssemblyName System.Configuration
$flags = [Reflection.BindingFlags]'NonPublic,Static'
$cm = [Configuration.ConfigurationManager]
$cm.GetField('s_initState', $flags).SetValue($null, 0)
$cm.GetField('s_configSystem', $flags).SetValue($null, $null)
foreach ($d in 'Oracle.ManagedDataAccess', 'Miracom.Middleware', 'DACrux.Base') { $p = Join-Path $serverBin "$d.dll"; if (Test-Path $p) { [void][Reflection.Assembly]::LoadFrom($p) } }
foreach ($d in 'DACrux.SEMDMS.Interface', 'DACrux.SEMDMS.DSL', 'DACrux.SEMDMS.BSL') {
    $p = Join-Path $BinDir "$d.dll"; if (-not (Test-Path $p)) { $p = Join-Path $serverBin "$d.dll" }
    [void][Reflection.Assembly]::LoadFrom($p)
}
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

public static class ChainRangeProbe
{
    static readonly CultureInfo IC = CultureInfo.InvariantCulture;
    public static List<string> Lines = new List<string>();

    static string RowText(DataRow row, IEnumerable<string> columns)
    {
        return string.Join("|", columns.Select(c => Convert.ToString(row[c], IC)));
    }

    /// <summary>장치 하나 검사. 반환: 실패 수(0 이면 통과). 결과 문장은 Lines 에 쌓는다.</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static int Run(string f, string d, int sample, int rangeSize)
    {
        int fail = 0;
        var chain = new DACrux.SEMDMS.BSL.GdsChain();
        DataTable list = chain.GetChainList(f, d);
        List<long> seqs = list.Rows.Cast<DataRow>().Select(r => Convert.ToInt64(r["CHAIN_SEQ"], IC)).OrderBy(s => s).ToList();
        if (seqs.Count == 0) { Lines.Add("SKIP / " + d + " / Chain 없음"); return 0; }

        // 1) 범위 조회 전체(5,000개씩) - 시간 / 행 수 / 전송 형식
        string[] kinds = { "Layers", "Rules", "Members", "Edits" };
        var rows = kinds.ToDictionary(k => k, k => new Dictionary<long, List<DataRow>>());
        var ms = kinds.ToDictionary(k => k, k => 0L);
        bool binary = true; int calls = 0;
        for (int start = 0; start < seqs.Count; start += rangeSize)
        {
            string from = seqs[start].ToString(IC), to = seqs[Math.Min(start + rangeSize, seqs.Count) - 1].ToString(IC);
            foreach (string k in kinds)
            {
                var sw = Stopwatch.StartNew();
                DataTable t = k == "Layers" ? chain.GetChainLayersRange(f, d, from, to)
                    : k == "Rules" ? chain.GetChainRulesRange(f, d, from, to)
                    : k == "Members" ? chain.GetChainMembersRange(f, d, from, to)
                    : chain.GetChainEditsRange(f, d, from, to);
                ms[k] += sw.ElapsedMilliseconds; calls++;
                if (t.RemotingFormat != SerializationFormat.Binary) binary = false;
                foreach (DataRow r in t.Rows)
                {
                    long seq = Convert.ToInt64(r["CHAIN_SEQ"], IC);
                    List<DataRow> l; if (!rows[k].TryGetValue(seq, out l)) rows[k][seq] = l = new List<DataRow>();
                    l.Add(r);
                }
            }
        }
        Lines.Add(string.Format(IC, "INFO / {0} / Chain {1:N0}개, 범위 호출 {2}번 / Layer {3:N0}행 {4}ms / 규칙 {5:N0}행 {6}ms / 포함 도형 {7:N0}행 {8}ms / 편집 {9:N0}행 {10}ms",
            d, seqs.Count, calls, rows["Layers"].Values.Sum(l => l.Count), ms["Layers"], rows["Rules"].Values.Sum(l => l.Count), ms["Rules"],
            rows["Members"].Values.Sum(l => l.Count), ms["Members"], rows["Edits"].Values.Sum(l => l.Count), ms["Edits"]));
        long memberTotal = list.Rows.Cast<DataRow>().Sum(r => Convert.ToInt64(r["MEMBER_COUNT"], IC));
        long rangeMembers = rows["Members"].Values.Sum(l => l.Count);
        bool countOk = rangeMembers == memberTotal;
        if (!countOk) fail++;
        Lines.Add((countOk ? "PASS" : "FAIL") + " / " + d + " / 포함 도형 행 수 = 목록 MEMBER_COUNT 합계 / " + rangeMembers.ToString("N0", IC) + " = " + memberTotal.ToString("N0", IC));
        if (!binary) fail++;
        Lines.Add((binary ? "PASS" : "FAIL") + " / " + d + " / 범위 조회 결과 Binary 전송 형식");

        // 2) 표본 Chain 은 Chain 단위 조회와 같은 행 / 값 / 순서
        var rnd = new Random(20261021);
        var pick = seqs.Count <= sample ? seqs : seqs.OrderBy(s => rnd.Next()).Take(sample).OrderBy(s => s).ToList();
        int diff = 0; string firstDiff = null; var sw2 = Stopwatch.StartNew();
        foreach (long seq in pick)
        {
            string s = seq.ToString(IC);
            foreach (string k in kinds)
            {
                DataTable one = k == "Layers" ? chain.GetChainLayers(f, d, s) : k == "Rules" ? chain.GetChainRules(f, d, s)
                    : k == "Members" ? chain.GetChainMembers(f, d, s) : chain.GetChainEdits(f, d, s);
                var columns = one.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
                List<DataRow> range; if (!rows[k].TryGetValue(seq, out range)) range = new List<DataRow>();
                var a = one.Rows.Cast<DataRow>().Select(r => RowText(r, columns)).ToList();
                var b = range.Select(r => RowText(r, columns)).ToList();
                if (!a.SequenceEqual(b)) { diff++; if (firstDiff == null) firstDiff = k + " CHAIN_SEQ " + s + ": " + a.Count + " / " + b.Count + "행"; }
            }
        }
        bool same = diff == 0;
        if (!same) fail++;
        Lines.Add(string.Format(IC, "{0} / {1} / 표본 Chain {2}개 x 4종 = Chain 단위 조회와 같음 / 다른 곳 {3}{4} (Chain 단위 {5}ms)",
            same ? "PASS" : "FAIL", d, pick.Count, diff, firstDiff == null ? "" : " - " + firstDiff, sw2.ElapsedMilliseconds));
        return fail;
    }

    /// <summary>잘못된 범위는 거부한다(시작 > 끝 / 숫자 아님).</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static int Invalid(string f, string d)
    {
        int fail = 0;
        var chain = new DACrux.SEMDMS.BSL.GdsChain();
        foreach (var pair in new[] { new[] { "10", "5" }, new[] { "A", "5" }, new[] { "0", "5" } })
        {
            bool rejected = false;
            try { chain.GetChainMembersRange(f, d, pair[0], pair[1]); } catch (ArgumentException) { rejected = true; }
            if (!rejected) fail++;
            Lines.Add((rejected ? "PASS" : "FAIL") + " / 잘못된 범위 거부 / " + pair[0] + " ~ " + pair[1]);
        }
        return fail;
    }
}
'@
Add-Type -TypeDefinition $helper -ReferencedAssemblies @((Join-Path $BinDir 'DACrux.SEMDMS.BSL.dll'), (Join-Path $serverBin 'Miracom.Middleware.dll'), (Join-Path $BinDir 'DACrux.SEMDMS.Interface.dll'), 'System.Data', 'System.Core', 'System.Xml')

$fail = 0
foreach ($device in $Devices) { $fail += [ChainRangeProbe]::Run($Factory, $device, $Sample, $RangeSize) }
$fail += [ChainRangeProbe]::Invalid($Factory, $Devices[0])
foreach ($line in [ChainRangeProbe]::Lines) { Write-Host $line }
$count = @([ChainRangeProbe]::Lines | Where-Object { $_ -match '^(PASS|FAIL)' }).Count
if ($fail -eq 0) { Write-Host "GDS_CHAIN_RANGE_PROBE=PASS ($count)" } else { Write-Host "GDS_CHAIN_RANGE_PROBE=FAIL ($fail)"; exit 1 }
