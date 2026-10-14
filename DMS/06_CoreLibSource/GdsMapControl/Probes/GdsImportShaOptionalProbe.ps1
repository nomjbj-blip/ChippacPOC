# GDS 파일 SHA-256 제거 검증 스크립트 (작성 / 컬럼 제거로 다시 씀)
# 배경: 2026-10-06 화면 계산 제거(컬럼 NULL 허용) -> 2026-10-08 결정으로 TQP_GDS_MAP.GDS_SHA256 컬럼 제거.
# 목적: BSL GdsMapImport 가
#       1) GetMapInfo 결과에 GDS_SHA256 열이 없다 (SELECT_MAP_INFO 에서 제거)
#       2) BeginMapImport ImportInfo[4](예전 SHA 자리)는 빈 값 / null / 예전 형식 / 틀린 형식 모두 무시하고 저장을 받아들인다
#          (자리는 이미 배포된 화면 / Probe와 순서를 맞추려고 남겼다. 예전에는 틀린 형식을 거부했다)
#       Service 를 띄우지 않고 BSL 을 직접 호출한다 (DB: 로컬 docker DMSMGR, 2026-10-08_tqp_gds_map_drop_sha.sql 적용 필요).
# 결과: PROBE / SHA_OPTIONAL_PROBE 헤더만 만들었다가 매번 삭제한다 (도형 데이터는 넣지 않음).
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsImportShaOptionalProbe.ps1 [-BslDir <BSL 폴더>]
param(
    [string]$BslDir = ''
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$serverBin = Join-Path $dms '101_ServerBin'
if ($BslDir -eq '') { $BslDir = $serverBin }
[AppDomain]::CurrentDomain.SetData('APP_CONFIG_FILE', (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config'))
Add-Type -AssemblyName System.Configuration
$flags = [Reflection.BindingFlags]'NonPublic,Static'
$cm = [Configuration.ConfigurationManager]
$cm.GetField('s_initState', $flags).SetValue($null, 0)
$cm.GetField('s_configSystem', $flags).SetValue($null, $null)
foreach ($d in 'Oracle.ManagedDataAccess', 'Miracom.Middleware', 'DACrux.Base', 'DACrux.SEMDMS.Interface', 'DACrux.SEMDMS.DSL') {
    $p = Join-Path $serverBin "$d.dll"; if (Test-Path $p) { [void][Reflection.Assembly]::LoadFrom($p) }
}
$bslDll = Join-Path $BslDir 'DACrux.SEMDMS.BSL.dll'
[void][Reflection.Assembly]::LoadFrom($bslDll)
Write-Host "BSL: $bslDll"

# Middleware 가 호출 클래스를 스택에서 찾으므로 BSL 호출은 NoInlining 래퍼 안에서 한다.
$helper = @"
using System;
using System.Data;
using System.Runtime.CompilerServices;

public static class ShaOptionalProbe
{
    /// <summary>헤더를 만들고 "OK:상태:열 여부"를 돌려준다(GDS_SHA256 열이 없으면 NO_SHA_COLUMN). 오류면 "ERROR:" + 메시지. 항상 지운다.</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string Begin(string factory, string device, string sha, bool twelve)
    {
        var bsl = new DACrux.SEMDMS.BSL.GdsMapImport();
        if (bsl.GetMapInfo(factory, device).Rows.Count > 0) bsl.DeleteMap(factory, device, "probe");
        try
        {
            string[] info = twelve
                ? new[] { factory, device, "TOP", "sha.gds", sha, "0.001", "1E-09", "probe", "0", "0", "0", "BACK" }
                : new[] { factory, device, "TOP", "sha.gds", sha, "0.001", "1E-09", "probe", "0", "0", "0" };
            bsl.BeginMapImport(info);
            DataTable dt = bsl.GetMapInfo(factory, device);
            if (dt.Rows.Count != 1) return "ERROR:헤더가 만들어지지 않았습니다.";
            return "OK:" + dt.Rows[0]["MAP_STATUS"] + (dt.Columns.Contains("GDS_SHA256") ? ":HAS_SHA_COLUMN" : ":NO_SHA_COLUMN");
        }
        catch (Exception ex)
        {
            return "ERROR:" + ex.Message;
        }
        finally
        {
            if (bsl.GetMapInfo(factory, device).Rows.Count > 0) bsl.DeleteMap(factory, device, "probe");
        }
    }
}
"@
Add-Type -TypeDefinition $helper -ReferencedAssemblies @($bslDll, (Join-Path $serverBin 'DACrux.SEMDMS.Interface.dll'),
    (Join-Path $serverBin 'Miracom.Middleware.dll'), 'System.Data', 'System.Xml')

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
$f = 'PROBE'; $dev = 'SHA_OPTIONAL_PROBE'
$hex = ('0123456789abcdef' * 4)

$ok = 'OK:DRAFT:NO_SHA_COLUMN'
$r = [ShaOptionalProbe]::Begin($f, $dev, '', $true)
Check '4번 빈 값 (12개 / BACK) -> 저장 / SHA 열 없음' ($r -eq $ok) $r
$r = [ShaOptionalProbe]::Begin($f, $dev, $null, $true)
Check '4번 null -> 저장' ($r -eq $ok) $r
$r = [ShaOptionalProbe]::Begin($f, $dev, $hex, $false)
Check '4번 예전 64자리 값 (11개) -> 무시하고 저장' ($r -eq $ok) $r
$r = [ShaOptionalProbe]::Begin($f, $dev, 'ABC', $true)
Check '4번 예전 기준 틀린 값 -> 무시하고 저장 (예전에는 거부)' ($r -eq $ok) $r
$r = [ShaOptionalProbe]::Begin($f, $dev, ('G' * 64), $true)
Check '4번 16진수 아님 -> 무시하고 저장 (예전에는 거부)' ($r -eq $ok) $r

$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host 'GDS_IMPORT_SHA_REMOVED_PROBE=PASS' } else { Write-Host "GDS_IMPORT_SHA_REMOVED_PROBE=FAIL ($fail)"; exit 1 }
