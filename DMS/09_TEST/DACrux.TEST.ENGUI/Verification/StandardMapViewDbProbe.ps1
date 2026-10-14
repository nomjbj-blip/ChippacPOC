# Wafer Map 조회 화면 DB 확인 (읽기 전용).
# 1. 101_ServerBin 의 TEST 서버 DLL(ProbeAdmin BSL)로 화면과 같은 조회 3개(GetMapIDAllList / GetMapDef / GetDies)를 실행해 XML 로 저장한다.
#    접속 설정은 ProbeAdmin 을 올리는 DACrux.TEST.Service.exe.config 를 그대로 쓴다. Remoting(서비스) 경유는 확인하지 않는다.
# 2. Verify-StandardMapView.ps1 이 만든 TEMP 빌드의 화면 검사 EXE 에 저장한 폴더를 넘겨, 조회 화면에 실제 DB 값을 넣고
#    Map 그리기 / 마우스 판정 / 데이터 확인 결과를 출력한다.
# DB 는 SELECT 만 한다. 실행 전 Verify-StandardMapView.ps1 을 한 번 실행해 TEMP 빌드를 만들어 둔다.
# 결과 마지막 줄: STANDARD_MAP_VIEW_SMOKE=PASS / FAIL (DB 항목은 "DB " 로 시작)
param([int]$MaxMaps = 100)
$ErrorActionPreference = 'Stop'
$projectDir = Split-Path $PSScriptRoot -Parent
$dmsDir = Split-Path (Split-Path $projectDir -Parent) -Parent
$serverBin = Join-Path $dmsDir '101_ServerBin'
$sample = Join-Path (Split-Path $dmsDir -Parent) 'SampleFile\MIRACOM_KLAF file\AIBIZ_SCK.KLAF.SINF'
$buildRoot = Join-Path $env:TEMP 'chippac\StandardMapViewVerification'
$outputDir = Join-Path $buildRoot 'bin'
$dataDir = Join-Path $buildRoot 'db'
if (-not (Test-Path "$outputDir\StandardMapViewSmoke.exe")) { throw '먼저 Verify-StandardMapView.ps1 을 실행하세요 (TEMP 빌드 없음).' }
Remove-Item -LiteralPath $dataDir -Recurse -Force -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Force $dataDir | Out-Null

# 1. 서버 DLL 로 조회 -> XML (스키마 포함, Oracle NUMBER 형식 유지)
[AppDomain]::CurrentDomain.SetData('APP_CONFIG_FILE', (Join-Path $serverBin 'DACrux.TEST.Service.exe.config'))
Add-Type -AssemblyName System.Configuration
$flags = [Reflection.BindingFlags]'NonPublic,Static'
$cm = [Configuration.ConfigurationManager]
$cm.GetField('s_initState', $flags).SetValue($null, 0)
$cm.GetField('s_configSystem', $flags).SetValue($null, $null)
foreach ($d in 'Oracle.ManagedDataAccess', 'Miracom.Middleware', 'DACrux.Base', 'DACrux.TEST.Interface', 'DACrux.TEST.DSL', 'DACrux.TEST.BSL') {
    $p = Join-Path $serverBin "$d.dll"; if (Test-Path $p) { [void][Reflection.Assembly]::LoadFrom($p) }
}
$helper = @"
using System.Data;
using System.Runtime.CompilerServices;

public static class StandardMapViewDbReader
{
    // Middleware 가 호출 클래스를 스택에서 찾으므로 인라인하지 않는다.
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static DataTable MapList() { return new DACrux.TEST.BSL.ProbeAdmin().GetMapIDAllList(); }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static DataTable MapDef(string mapId) { return new DACrux.TEST.BSL.ProbeAdmin().GetMapDef(mapId); }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static DataTable Dies(string mapId) { return new DACrux.TEST.BSL.ProbeAdmin().GetDies(mapId); }
}
"@
$refs = @('System.Data', 'System.Xml', (Join-Path $serverBin 'DACrux.TEST.BSL.dll'), (Join-Path $serverBin 'DACrux.TEST.Interface.dll'), (Join-Path $serverBin 'Miracom.Middleware.dll'))
Add-Type -TypeDefinition $helper -ReferencedAssemblies $refs

$list = [StandardMapViewDbReader]::MapList()
$list.TableName = 'MAP_LIST'
$list.WriteXml((Join-Path $dataDir 'list.xml'), [Data.XmlWriteMode]::WriteSchema)
"Map 목록 (GetMapIDAllList): $($list.Rows.Count) 건"
$index = 0
foreach ($row in $list.Rows) {
    if ($index -ge $MaxMaps) { break }
    $mapId = [string]$row['MAPID']
    $def = [StandardMapViewDbReader]::MapDef($mapId)
    $dies = [StandardMapViewDbReader]::Dies($mapId)
    $def.TableName = 'MAP_DEF'
    $dies.TableName = 'DIES'
    $def.WriteXml((Join-Path $dataDir ('map{0:000}_def.xml' -f $index)), [Data.XmlWriteMode]::WriteSchema)
    $dies.WriteXml((Join-Path $dataDir ('map{0:000}_dies.xml' -f $index)), [Data.XmlWriteMode]::WriteSchema)
    $index++
}
"정의 / Die 조회 (GetMapDef / GetDies): $index 건 -> $dataDir"

# 2. 화면 검사 EXE 에 DB 데이터 폴더를 넘긴다 (화면 검사 전체 + DB 항목).
$process = Start-Process -FilePath "$outputDir\StandardMapViewSmoke.exe" -ArgumentList @("`"$outputDir\DACrux.TEST.ENGUI.dll`"", "`"$sample`"", "`"$buildRoot\view-preview.png`"", "`"$dataDir`"") `
    -WorkingDirectory $outputDir -NoNewWindow -PassThru -RedirectStandardOutput "$buildRoot\db-smoke.log"
$null = $process.Handle
if (-not $process.WaitForExit(300000)) { $process.Kill(); throw '화면 검사 시간 초과 (MessageBox 대기 가능성)' }
Get-Content "$buildRoot\db-smoke.log" | Where-Object { $_ -match '^(PASS DB|DB |FAIL|ERROR|STANDARD_MAP_VIEW_SMOKE)' -or $_ -match '^   ' }
if ($process.ExitCode -ne 0) { throw '화면 검사 실패' }
