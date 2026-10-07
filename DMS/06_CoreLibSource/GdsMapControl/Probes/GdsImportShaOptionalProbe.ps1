# GDS 파일 SHA-256 선택 입력 검증 스크립트 (CLAUDE-028, 사용자 지시 "빼줘": 화면 계산 제거 / 컬럼 NULL 허용)
# 목적: BSL GdsMapImport.BeginMapImport 가
#       1) SHA 빈 값이면 저장을 받아들이고 TQP_GDS_MAP.GDS_SHA256 = NULL 로 만든다 (새 화면 호출, 12개 항목 / BACK)
#       2) 64자리 16진수가 오면 지금처럼 저장한다 (이전 클라이언트 / 기존 Probe 호환, 11개 항목)
#       3) 형식이 틀린 값(64자리 아님 / 16진수 아님)은 거부한다
#       Service 를 띄우지 않고 BSL 을 직접 호출한다 (DB: 로컬 docker DMSMGR, GDS_SHA256 NULL 허용 DDL 적용 필요).
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
    /// <summary>헤더를 만들고 GDS_SHA256 값을 돌려준다 (NULL 이면 "(NULL)"). 오류면 "ERROR:" + 메시지. 항상 지운다.</summary>
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
            object value = dt.Rows[0]["GDS_SHA256"];
            return value == DBNull.Value ? "(NULL)" : value.ToString().Trim();
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

$r = [ShaOptionalProbe]::Begin($f, $dev, '', $true)
Check 'SHA 빈 값 (12개 / BACK) -> 저장 / NULL' ($r -eq '(NULL)') $r
$r = [ShaOptionalProbe]::Begin($f, $dev, $null, $true)
Check 'SHA null -> 저장 / NULL' ($r -eq '(NULL)') $r
$r = [ShaOptionalProbe]::Begin($f, $dev, $hex, $false)
Check 'SHA 64자리 16진수 (11개) -> 대문자로 저장' ($r -eq $hex.ToUpperInvariant()) $r
$r = [ShaOptionalProbe]::Begin($f, $dev, 'ABC', $true)
Check 'SHA 64자리 아님 -> 거부' ($r.StartsWith('ERROR:')) $r
$r = [ShaOptionalProbe]::Begin($f, $dev, ('G' * 64), $true)
Check 'SHA 16진수 아님 -> 거부' ($r.StartsWith('ERROR:')) $r

$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host 'GDS_IMPORT_SHA_OPTIONAL_PROBE=PASS' } else { Write-Host "GDS_IMPORT_SHA_OPTIONAL_PROBE=FAIL ($fail)"; exit 1 }
