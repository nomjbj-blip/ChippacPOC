# GDS Map 조회 서비스 검증 스크립트 (CLAUDE-014 작성 / CLAUDE-018 Factory + Device 키로 변경)
# 목적: DACrux.SEMDMS.BSL.GdsMapView(읽기 전용 조회 서비스)를 Service 없이 같은 프로세스에서 직접 호출해
#       Device / Map 정보 / Layer 목록과 PLACED 키 순서 페이지 조회가 누락 / 중복 없이 끝까지 읽히는지 확인한다.
#       한 페이지 DataTable의 Remoting 직렬화 크기(XML 기본 / Binary)도 함께 측정한다(조회 설계 7~8절).
#       2026-10-02 리비전 관리 제거(CLAUDE-017)로 Map은 Factory + Device당 1개다.
# 대상: DACrux.SEMDMS.Service.exe.config의 DMS_CONNECT_ID(DMSMGR) -> 레지스트리 Middleware 연결 (로컬 docker 시험 DB)
# 선행: READY Map이 1건 이상 있어야 한다(없으면 GdsMapDbRestoreProbe.ps1이 PROBE / VIEW_PROBE로 저장한다).
# 결과: 조회는 DB를 바꾸지 않는다. 단, DRAFT 거부 확인용으로 FACTORY=PROBE / DEVICE_ID=VIEW_DRAFT DRAFT Map 1건을 만든다(실행마다 지우고 다시 만듦).
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsMapViewProbe.ps1 [-Factory PROBE] [-Device VIEW_PROBE] [-BslDir <BSL / DSL / Interface DLL 폴더>] [-PageRows 10000]
param(
    [string]$Factory = 'PROBE',
    [string]$Device = 'VIEW_PROBE',
    [string]$BslDir = '',
    [int]$PageRows = 10000
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$serverBin = Join-Path $dms '101_ServerBin'
if ($BslDir -eq '') { $BslDir = $serverBin }

# BSL / DSL이 ConfigurationManager.AppSettings["DMS_CONNECT_ID"]를 읽으므로 Service 설정 파일을 이 프로세스 설정으로 지정한다.
[AppDomain]::CurrentDomain.SetData('APP_CONFIG_FILE', (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config'))
Add-Type -AssemblyName System.Configuration
$flags = [Reflection.BindingFlags]'NonPublic,Static'
$cm = [Configuration.ConfigurationManager]
$cm.GetField('s_initState', $flags).SetValue($null, 0)
$cm.GetField('s_configSystem', $flags).SetValue($null, $null)
if ([Configuration.ConfigurationManager]::AppSettings['DMS_CONNECT_ID'] -ne 'DMSMGR') { throw 'DMS_CONNECT_ID를 읽지 못했습니다.' }

foreach ($d in 'Oracle.ManagedDataAccess', 'Miracom.Middleware') { [void][Reflection.Assembly]::LoadFrom((Join-Path $serverBin "$d.dll")) }
foreach ($d in 'DACrux.SEMDMS.Interface', 'DACrux.SEMDMS.DSL', 'DACrux.SEMDMS.BSL') { [void][Reflection.Assembly]::LoadFrom((Join-Path $BslDir "$d.dll")) }
Write-Host "BSL 위치: $BslDir / 페이지 행 수: $PageRows"

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
function Fails([scriptblock]$action) {
    try { & $action; return $null } catch { $e = $_.Exception; while ($e.InnerException -ne $null) { $e = $e.InnerException }; return $e.Message }
}
# DataTable을 Remoting과 같은 BinaryFormatter로 직렬화한 바이트 수 / 시간
function Measure-Serialize([Data.DataTable]$dt, [Data.SerializationFormat]$format) {
    $old = $dt.RemotingFormat; $dt.RemotingFormat = $format
    $ms = New-Object IO.MemoryStream; $bf = New-Object Runtime.Serialization.Formatters.Binary.BinaryFormatter
    $sw = [Diagnostics.Stopwatch]::StartNew(); $bf.Serialize($ms, $dt); $ser = $sw.ElapsedMilliseconds
    $ms.Position = 0; $sw.Restart(); [void]$bf.Deserialize($ms); $de = $sw.ElapsedMilliseconds
    $dt.RemotingFormat = $old
    return "{0:N0} bytes / 직렬화 {1}ms / 역직렬화 {2}ms" -f $ms.Length, $ser, $de
}

$view = New-Object DACrux.SEMDMS.BSL.GdsMapView

# 1. Device 목록 (READY Map만)
$devices = $view.GetMapDeviceList()
Check 'READY Device 목록' ($devices.Rows.Count -gt 0) "rows=$($devices.Rows.Count)"
if ($devices.Rows.Count -eq 0) { Write-Host 'READY Map이 없어 중단합니다. GdsMapDbRestoreProbe.ps1을 먼저 실행하세요.'; exit 1 }

# 2. Map 정보: 목록의 Device는 모두 READY여야 한다. 대상은 지정한 시험 Device(다른 세션의 시험 데이터와 간섭하지 않게 고정).
$allReady = $true
foreach ($d in $devices.Rows) {
    $info = $view.GetMapInfo([string]$d['FACTORY'], [string]$d['DEVICE_ID'])
    if ($info.Rows.Count -eq 1 -and $info.Rows[0]['MAP_STATUS'] -ne 'READY') { $allReady = $false }
}
$inList = @($devices.Rows | Where-Object { $_['FACTORY'] -eq $Factory -and $_['DEVICE_ID'] -eq $Device }).Count -eq 1
$best = $view.GetMapInfo($Factory, $Device).Rows[0]
$factory = $Factory; $device = $Device
Check 'Device 목록은 READY Map만 / 대상 Device 포함' ($allReady -and $inList) "대상 $factory/$device 파일=$($best['GDS_FILE_NAME']) PLACED=$($best['PLACED_COUNT']) POINT=$($best['POINT_COUNT']) CREATE=$($best['CREATE_TIME'])"

# 3. Layer 목록: Layer별 합계 = Map 건수
$sw = [Diagnostics.Stopwatch]::StartNew()
$layers = $view.GetMapLayerList($factory, $device)
$layerMs = $sw.ElapsedMilliseconds
$sumPlaced = 0L; $sumPoint = 0L
foreach ($l in $layers.Rows) { $sumPlaced += [long]$l['PLACED_COUNT']; $sumPoint += [long]$l['POINT_COUNT'] }
Check 'Layer 합계 = Map PLACED / 좌표 수' ($layers.Rows.Count -gt 0 -and $sumPlaced -eq [long]$best['PLACED_COUNT'] -and $sumPoint -eq [long]$best['POINT_COUNT']) "layers=$($layers.Rows.Count) placed=$sumPlaced point=$sumPoint ${layerMs}ms"

# 4. 전체 페이지 조회: 누락 / 중복 / 순서 / 좌표 바이트 길이
$ids = New-Object 'Collections.Generic.HashSet[string]'
$totalRows = 0L; $totalPoint = 0L; $pages = 0; $orderOk = $true; $binOk = $true; $layerOk = $true
$firstPage = $null; $maxPageMs = 0L
$swAll = [Diagnostics.Stopwatch]::StartNew()
foreach ($l in $layers.Rows) {
    $layerId = [string]$l['LAYER_ID']; $after = ''; $layerRows = 0L
    while ($true) {
        $swPage = [Diagnostics.Stopwatch]::StartNew()
        $page = $view.GetPlacedElementPage($factory, $device, $layerId, $after, $PageRows)
        if ($swPage.ElapsedMilliseconds -gt $maxPageMs) { $maxPageMs = $swPage.ElapsedMilliseconds }
        $pages++
        $prev = $after
        foreach ($row in $page.Rows) {
            $id = [string]$row['PLACED_ELEMENT_ID']
            if ($prev -ne '' -and [string]::CompareOrdinal($prev, $id) -ge 0) { $orderOk = $false }
            if (-not $ids.Add($id)) { $orderOk = $false }
            if ([string]$row['LAYER_ID'] -ne $layerId) { $layerOk = $false }
            $pc = [long]$row['POINT_COUNT']; $totalPoint += $pc
            if (([byte[]]$row['POINTS_BIN']).Length -ne $pc * 16) { $binOk = $false }
            $prev = $id
        }
        $layerRows += $page.Rows.Count; $totalRows += $page.Rows.Count
        if ($firstPage -eq $null -and $page.Rows.Count -eq $PageRows) { $firstPage = $page } else { if ($page -ne $firstPage) { $page.Dispose() } }
        if ($page.Rows.Count -lt $PageRows) { break }
        $after = $prev
    }
    if ($layerRows -ne [long]$l['PLACED_COUNT']) { $layerOk = $false; Write-Host "  Layer $layerId 행 수 불일치: $layerRows / $($l['PLACED_COUNT'])" }
}
$allMs = $swAll.ElapsedMilliseconds
Check '전체 페이지 행 수 = PLACED 수 (중복 없음)' ($totalRows -eq $sumPlaced -and $ids.Count -eq $totalRows) "rows=$totalRows unique=$($ids.Count) pages=$pages"
Check '페이지 순서 증가 / 중복 없음' $orderOk ''
Check 'Layer별 행 수 / LAYER_ID 일치' $layerOk ''
Check '좌표 합계 일치 / POINTS_BIN 길이 = 점 수 x 16' ($binOk -and $totalPoint -eq $sumPoint) "point=$totalPoint"
Write-Host ("측정: 전체 {0:N1}초 / 페이지 {1}개 / 최대 페이지 {2}ms / 행당 {3:N4}ms" -f ($allMs / 1000.0), $pages, $maxPageMs, ($allMs / [Math]::Max(1, $totalRows)))

# 5. 한 페이지 직렬화 크기 (Remoting 전달 부담 측정)
if ($firstPage -ne $null) {
    Check '페이지 반환 형식 Binary' ($firstPage.RemotingFormat -eq [Data.SerializationFormat]::Binary) "$($firstPage.RemotingFormat)"
    Write-Host ("직렬화 ({0}행) XML 기본: {1}" -f $firstPage.Rows.Count, (Measure-Serialize $firstPage ([Data.SerializationFormat]::Xml)))
    Write-Host ("직렬화 ({0}행) Binary : {1}" -f $firstPage.Rows.Count, (Measure-Serialize $firstPage ([Data.SerializationFormat]::Binary)))
}

# 6. 입력 / 상태 검증
$m1 = Fails { $view.GetPlacedElementPage($factory, $device, '0', '', 0) }
Check '페이지 행 수 0 거부' ($m1 -ne $null) "$m1"
$m2 = Fails { $view.GetPlacedElementPage($factory, $device, '0', '', 50001) }
Check '페이지 행 수 상한 초과 거부' ($m2 -ne $null) "$m2"
$m3 = Fails { $view.GetPlacedElementPage($factory, $device, '-1', '', 10) }
Check '음수 Layer 거부' ($m3 -ne $null) "$m3"
$m4 = Fails { $view.GetMapLayerList('PROBE', 'NO_SUCH_DEVICE') }
Check '없는 Map 거부' ($m4 -ne $null -and $m4.Contains('없습니다')) "$m4"
$m5 = Fails { $view.GetMapInfo('', 'X') }
Check 'Factory 빈 값 거부' ($m5 -ne $null) "$m5"
Check '없는 Map 정보는 0행' ($view.GetMapInfo('PROBE', 'NO_SUCH_DEVICE').Rows.Count -eq 0) ''

# DRAFT Map 거부: 저장 서비스로 DRAFT Map 1건을 만든 뒤 조회한다(이전 실행분은 먼저 지운다).
$import = New-Object DACrux.SEMDMS.BSL.GdsMapImport
if ($import.GetMapInfo('PROBE', 'VIEW_DRAFT').Rows.Count -gt 0) { [void]$import.DeleteMap('PROBE', 'VIEW_DRAFT', 'probe') }
[void]$import.BeginMapImport([string[]]@('PROBE', 'VIEW_DRAFT', 'TOP', 'draft.gds', ('A' * 64), '0.001', '0.000000001', 'probe', '1', '1', '1'))
$m6 = Fails { $view.GetPlacedElementPage('PROBE', 'VIEW_DRAFT', '0', '', 10) }
Check 'DRAFT Map 조회 거부' ($m6 -ne $null -and $m6.Contains('READY')) "$m6"
$draftInList = @($view.GetMapDeviceList().Rows | Where-Object { $_['DEVICE_ID'] -eq 'VIEW_DRAFT' }).Count
Check 'DRAFT Map은 Device 목록에 없음' ($draftInList -eq 0) ''

$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host 'GDS_MAP_VIEW_PROBE=PASS' } else { Write-Host "GDS_MAP_VIEW_PROBE=FAIL ($fail)"; exit 1 }
