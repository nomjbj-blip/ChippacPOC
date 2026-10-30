# WorkRule: DB 일시를 읽지 못하면 저장을 막는지 확인 (2026-10-29, SCN-06)
# 서비스 config 사본의 DMS_CONNECT_ID 만 없는 접속 ID 로 바꿔 WorkRule.Confirm 을 부른다. DB 에 쓰지 않는다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File WorkRuleDbFailCheck.ps1 [-BslDir <폴더>]
param([string]$BslDir = '')
$ErrorActionPreference = 'Stop'
$root = (Resolve-Path (Join-Path $PSScriptRoot '..\..')).Path
$serverBin = Join-Path $root 'DMS\101_ServerBin'
if ($BslDir -eq '') { $BslDir = $serverBin }
$tmp = Join-Path $env:TEMP 'ChippacWorkRuleDbFail'
New-Item -ItemType Directory -Force $tmp | Out-Null
$cfg = Join-Path $tmp 'probe.exe.config'
[xml]$x = Get-Content (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config') -Encoding UTF8
$n = $x.configuration.appSettings.add | Where-Object { $_.key -eq 'DMS_CONNECT_ID' }
$n.SetAttribute('value', 'NO_SUCH_CONNECT_ID')
$x.Save($cfg)
Add-Type -AssemblyName System.Configuration
[AppDomain]::CurrentDomain.SetData('APP_CONFIG_FILE', $cfg)
$f = [Reflection.BindingFlags]'NonPublic,Static'; $cm = [Configuration.ConfigurationManager]
$cm.GetField('s_initState', $f).SetValue($null, 0); $cm.GetField('s_configSystem', $f).SetValue($null, $null)
foreach ($d in 'Oracle.ManagedDataAccess', 'Miracom.Middleware', 'DACrux.Base') { [void][Reflection.Assembly]::LoadFrom((Join-Path $serverBin "$d.dll")) }
$dsl = [Reflection.Assembly]::LoadFrom((Join-Path $BslDir 'DACrux.SEMDMS.DSL.dll'))
$t = $dsl.GetType('DACrux.SEMDMS.DSL.WorkRule', $true)
$rule = [Activator]::CreateInstance($t)
$mod = [Enum]::Parse($dsl.GetType('DACrux.SEMDMS.DSL.WorkModule'), 'Gds'); $kind = [Enum]::Parse($dsl.GetType('DACrux.SEMDMS.DSL.WorkKind'), 'Setup')
$msg = ''; $type = ''
try { [void]$t.GetMethod('Confirm').Invoke($rule, @($mod, $kind)) } catch { $e = $_.Exception; while ($e.InnerException) { $e = $e.InnerException }; $msg = $e.Message; $type = $e.GetType().Name }
$ok = ($type -eq 'InvalidOperationException' -and $msg -like '서버 일시를 확인할 수 없어 저장할 수 없습니다.*')
Write-Host ("[{0}] DB 일시 실패 -> 저장 막음 {1}: {2}" -f $(if ($ok) { 'PASS' } else { 'FAIL' }), $type, $(if ($msg) { $msg } else { '예외 없음(통과함)' }))
if ($ok) { Write-Host 'WORK_RULE_DBFAIL_PROBE=PASS (1)' } else { Write-Host 'WORK_RULE_DBFAIL_PROBE=FAIL (1 / 1)'; exit 1 }
