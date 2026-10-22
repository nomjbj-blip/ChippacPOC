# GDS 화면 Factory 기준 검증 스크립트 (2026-10-07 요청: Factory는 DACrux.Base.GlobalVariable.Factory 사용, 화면 Factory 선택 제거)
# 목적: GdsMap이 DACrux가 로드한 DACrux.Base의 GlobalVariable.Factory를 그대로 읽는지(DmsHostContext),
#       View 화면 Device 목록이 그 Factory의 Map만 보여 주는지, Factory 입력 컨트롤이 없어졌는지 확인한다.
#       Device 목록은 BSL GetMapDeviceList(직접 호출)로 채우고 화면은 띄우지 않는다. DB를 바꾸지 않는다(조회만).
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsFactoryContextProbe.ps1 -GdsExe <빌드한 NexplantQMS.GdsMap.dll> [-BinDir <BSL 폴더>]
param(
    [Parameter(Mandatory = $true)][string]$GdsExe,
    [string]$BinDir = '',
    [string]$Factory = 'PROBE'
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
$probeDir = Join-Path $env:TEMP 'chippac\gdsfactoryprobe'
New-Item -ItemType Directory -Force $probeDir | Out-Null
Copy-Item $GdsExe (Join-Path $probeDir 'NexplantQMS.GdsMap.dll') -Force
foreach ($d in 'OpenTK.dll', 'OpenTK.GLControl.dll') { Copy-Item (Join-Path (Split-Path $GdsExe) $d) $probeDir -Force }
$gdsDll = Join-Path $probeDir 'NexplantQMS.GdsMap.dll'
foreach ($d in 'OpenTK.dll', 'OpenTK.GLControl.dll') { [void][Reflection.Assembly]::LoadFrom((Join-Path $probeDir $d)) }
$gds = [Reflection.Assembly]::LoadFrom($gdsDll)
Add-Type -AssemblyName System.Windows.Forms

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
$np = [Reflection.BindingFlags]'NonPublic,Instance'
$ctxProp = $gds.GetType('NexplantQMS.GdsMap.DmsHostContext', $true).GetProperty('Factory', [Reflection.BindingFlags]'Public,Static')

# 1. GlobalVariable.Factory 읽기 (앞뒤 공백 제거 / 비어 있음)
[DACrux.Base.GlobalVariable]::Factory = " $Factory "
Check 'GlobalVariable.Factory 읽기' ($ctxProp.GetValue($null) -eq $Factory) "GlobalVariable=' $Factory ' -> '$($ctxProp.GetValue($null))'"
[DACrux.Base.GlobalVariable]::Factory = ''
Check '로그인 Factory 없음' ($ctxProp.GetValue($null) -eq '') "빈 값 -> '$($ctxProp.GetValue($null))'"

# 2. View 화면: Factory 컨트롤 없음 + Device 목록이 로그인 Factory만
$viewType = $gds.GetType('NexplantQMS.GdsMap.GdsMapViewForm', $true)
$setupType = $gds.GetType('NexplantQMS.GdsMap.GdsMapForm', $true)
$left = @(@($viewType, 'cboFactory'), @($viewType, 'lblFactory'), @($setupType, 'txtDbFactory'), @($setupType, 'lblDbFactory') | Where-Object { $_[0].GetField($_[1], $np) -ne $null } | ForEach-Object { $_[1] })
Check 'Factory 입력 컨트롤 제거' ($left.Count -eq 0) $(if ($left.Count -eq 0) { 'View cboFactory / Setup txtDbFactory 없음' } else { '남음: ' + ($left -join ', ') })

$all = (New-Object DACrux.SEMDMS.BSL.GdsMapView).GetMapDeviceList()
$expected = @($all.Rows | Where-Object { $_['FACTORY'].ToString() -eq $Factory } | ForEach-Object { $_['DEVICE_ID'].ToString() })
$others = @($all.Rows | Where-Object { $_['FACTORY'].ToString() -ne $Factory }).Count
$form = [Activator]::CreateInstance($viewType)
$cboDevice = $viewType.GetField('cboDevice', $np).GetValue($form)
# Device 선택 이벤트는 RO(원격) 호출로 Map 정보를 읽으므로 이 검사에서는 뗀다.
$cboDevice.remove_SelectedIndexChanged([Delegate]::CreateDelegate([EventHandler], $form, 'cboDevice_SelectedIndexChanged'))
[DACrux.Base.GlobalVariable]::Factory = $Factory
$viewType.GetField('_factory', $np).SetValue($form, $ctxProp.GetValue($null))
$viewType.GetField('_deviceTable', $np).SetValue($form, $all)
$viewType.GetMethod('FillDeviceList', $np).Invoke($form, $null)
$listed = @($cboDevice.Items | ForEach-Object { $_.ToString() })
Check 'Device 목록 = 로그인 Factory' ($listed.Count -gt 0 -and ($listed -join ',') -eq ($expected -join ',')) ("Factory $Factory -> " + ($listed -join ', ') + " / 전체 Map $($all.Rows.Count)건 중 다른 Factory $($others)건 제외")
$viewType.GetField('_factory', $np).SetValue($form, 'NO_SUCH_FACTORY')
$viewType.GetMethod('FillDeviceList', $np).Invoke($form, $null)
Check '다른 Factory면 목록 없음' ($cboDevice.Items.Count -eq 0) "NO_SUCH_FACTORY -> $($cboDevice.Items.Count)건"
$form.Dispose()

$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host "GDS_FACTORY_CONTEXT_PROBE=PASS ($($results.Count))" } else { Write-Host "GDS_FACTORY_CONTEXT_PROBE=FAIL ($fail)"; exit 1 }
