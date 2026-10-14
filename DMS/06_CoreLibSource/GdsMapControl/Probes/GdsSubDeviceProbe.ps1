# GDS 대표 Device / Sub Device 검증 스크립트 (2026-10-12)
# 목적: BSL GdsMapImport.SaveSubDeviceList / GdsMapView.GetSubDeviceList / GetMapDeviceBySub 가 설계대로 동작하는지 본다.
#       설계: 문서/2026-10-12_GDS_대표Device_SubDevice_설계.md (D1 대표 자신 저장 안 함 / D2 Sub 1:1 / D3 다른 Map 대표 이름 거부 / D4 대소문자 무시 / D5 Map 삭제 후에도 설정 유지)
#       Service 를 띄우지 않고 BSL 을 직접 호출한다 (DB: 로컬 docker DMSMGR, 2026-10-12_tqp_gds_map_sub_device.sql 적용 필요).
# 데이터: Factory PROBE 의 SUBDEV_REP_A / SUBDEV_REP_B 빈 Map 헤더(도형 없음)를 만들고 끝나면 Sub 설정과 함께 모두 지운다. 사용자 데이터는 건드리지 않는다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsSubDeviceProbe.ps1 [-BinDir <BSL/DSL/Interface 폴더>]
param(
    [string]$BinDir = ''
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$serverBin = Join-Path $dms '101_ServerBin'
if ($BinDir -eq '') { $BinDir = Join-Path $env:TEMP 'chippac\gdsmap-srv' }
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
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;

public static class SubDeviceProbe
{
    public const string F = "PROBE";

    /// <summary>도형 없는 READY Map 헤더를 만든다(예상 건수 0으로 시작 -> 확정). 반환: MAP_STATUS</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string CreateMap(string device)
    {
        var imp = new DACrux.SEMDMS.BSL.GdsMapImport();
        if (imp.GetMapInfo(F, device).Rows.Count > 0) imp.DeleteMap(F, device, "probe");
        DataTable job = imp.BeginMapImport(new[] { F, device, "TOP", "probe.gds", "", "0.001", "1E-09", "probe", "0", "0", "0" });
        imp.CompleteMapImport(job.Rows[0]["IMPORT_JOB_SEQ"].ToString(), "probe");
        return imp.GetMapInfo(F, device).Rows[0]["MAP_STATUS"].ToString();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void DeleteMap(string device)
    {
        var imp = new DACrux.SEMDMS.BSL.GdsMapImport();
        if (imp.GetMapInfo(F, device).Rows.Count > 0) imp.DeleteMap(F, device, "probe");
    }

    /// <summary>저장 결과 "OK:수" 또는 "ERROR:메시지"</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string Save(string device, string[] subs)
    {
        try
        {
            string[] remarks = subs.Select(s => "probe " + s).ToArray();
            return "OK:" + new DACrux.SEMDMS.BSL.GdsMapImport().SaveSubDeviceList(F, device, subs, remarks, "probe");
        }
        catch (Exception ex) { return "ERROR:" + ex.Message; }
    }

    /// <summary>Sub 목록을 대문자 정렬해 "A,B" 로</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string List(string device)
    {
        DataTable dt = new DACrux.SEMDMS.BSL.GdsMapView().GetSubDeviceList(F, device);
        return string.Join(",", dt.Rows.Cast<DataRow>().Select(r => r["SUB_DEVICE_ID"].ToString()).ToArray());
    }

    /// <summary>"대표:MATCH_TYPE" 또는 "NONE"</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string Resolve(string any)
    {
        DataTable dt = new DACrux.SEMDMS.BSL.GdsMapView().GetMapDeviceBySub(F, any);
        return dt.Rows.Count == 0 ? "NONE" : dt.Rows[0]["DEVICE_ID"] + ":" + dt.Rows[0]["MATCH_TYPE"];
    }
}
"@
Add-Type -TypeDefinition $helper -ReferencedAssemblies @((Join-Path $BinDir 'DACrux.SEMDMS.BSL.dll'), (Join-Path $BinDir 'DACrux.SEMDMS.Interface.dll'),
    (Join-Path $serverBin 'Miracom.Middleware.dll'), 'System.Data', 'System.Xml', 'System.Core')

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
$A = 'SUBDEV_REP_A'; $B = 'SUBDEV_REP_B'
try {
    $s = [SubDeviceProbe]::CreateMap($A); Check '대표 A Map READY 준비' ($s -eq 'READY') $s
    $s = [SubDeviceProbe]::CreateMap($B); Check '대표 B Map READY 준비' ($s -eq 'READY') $s
    [void][SubDeviceProbe]::Save($A, @()); [void][SubDeviceProbe]::Save($B, @())

    $r = [SubDeviceProbe]::Save($A, @('OMM', ' OMM_Hybrid ', 'subdev_rep_a', ''))
    Check 'A 저장: 대표 자신 / 빈 값 제외 (D1)' ($r -eq 'OK:2') $r
    $r = [SubDeviceProbe]::List($A); Check 'A 목록 = OMM, OMM_Hybrid (공백 정리)' ($r -eq 'OMM,OMM_Hybrid') $r
    $r = [SubDeviceProbe]::Resolve('omm_hybrid'); Check 'omm_hybrid -> A (Sub, 대소문자 무시 D4)' ($r -eq "${A}:SUB") $r
    $r = [SubDeviceProbe]::Resolve('subdev_rep_b'); Check 'subdev_rep_b -> B (같은 이름 Map)' ($r -eq "${B}:SELF") $r
    $r = [SubDeviceProbe]::Resolve('NO_SUCH_DEVICE'); Check '없는 이름 -> 0행' ($r -eq 'NONE') $r

    $r = [SubDeviceProbe]::Save($B, @('omm')); Check 'B 에 A 의 Sub(omm) 저장 -> 오류 (D2)' ($r.StartsWith('ERROR:') -and $r.Contains('OMM')) $r
    $r = [SubDeviceProbe]::List($B); Check 'B 목록 그대로 비어 있음' ($r -eq '') $r
    $r = [SubDeviceProbe]::Save($A, @('X1', 'x1')); Check '대소문자만 다른 중복 -> 오류' ($r.StartsWith('ERROR:')) $r
    $r = [SubDeviceProbe]::List($A); Check '오류 뒤 A 목록 유지 (검사 후 삭제)' ($r -eq 'OMM,OMM_Hybrid') $r
    $r = [SubDeviceProbe]::Save($A, @($B)); Check '다른 Map 대표 이름 -> 오류 (D3)' ($r.StartsWith('ERROR:') -and $r.Contains($B)) $r
    $r = [SubDeviceProbe]::Save('SUBDEV_NO_MAP', @('Z')); Check 'Map 없는 대표 -> 오류' ($r.StartsWith('ERROR:')) $r
    $r = [SubDeviceProbe]::Save($A, @('OMM', 'OMM_Hybrid', 'U9_TEST')); Check 'A 다시 저장(교체) 3개' ($r -eq 'OK:3') $r

    [SubDeviceProbe]::DeleteMap($A)
    $r = [SubDeviceProbe]::Resolve('OMM'); Check 'A Map 삭제 -> OMM 찾기 0행 (READY 없음)' ($r -eq 'NONE') $r
    $s = [SubDeviceProbe]::CreateMap($A)
    $r = [SubDeviceProbe]::Resolve('OMM'); Check 'A Map 재등록 -> Sub 설정 유지로 다시 연결 (D5)' ($r -eq "${A}:SUB") $r

    $r = [SubDeviceProbe]::Save($A, @()); Check 'A 전체 해제' ($r -eq 'OK:0' -and [SubDeviceProbe]::List($A) -eq '') $r
}
finally {
    try { [void][SubDeviceProbe]::Save($A, @()) } catch { }
    try { [void][SubDeviceProbe]::Save($B, @()) } catch { }
    [SubDeviceProbe]::DeleteMap($A); [SubDeviceProbe]::DeleteMap($B)
}

$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host "GDS_SUB_DEVICE_PROBE=PASS ($($results.Count))" } else { Write-Host "GDS_SUB_DEVICE_PROBE=FAIL ($fail)"; exit 1 }
