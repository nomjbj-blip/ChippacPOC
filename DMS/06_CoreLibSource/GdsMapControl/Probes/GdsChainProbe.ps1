# GDS Chain DB 저장 서비스 검증 스크립트 (설계: 문서/설계/02_GDS/2026-10-06_GDS_Chain_DB저장_설계.md 10.1절)
# 목적: BSL GdsChain을 같은 프로세스에서 직접 호출해 Chain 저장 / 조회 / 덮어쓰기 / 확정 검사 / 실제 삭제를 확인한다.
#       DB 접속은 BSL -> DSL -> Miracom.Middleware 경로다(Service Remoting 경유 아님).
# 전제: FACTORY=PROBE / DEVICE_ID=LAYER_FILTER_PROBE 에 READY Map이 있어야 한다(GdsImportLayerFilterProbe.ps1 -Keep 으로 생성, Layer 1,2,6).
#       Map에 없는 Layer(기본 3)는 FK 거부 확인에 쓴다.
# 결과: Chain Code ZZPROBE1 / ZZPROBE2 를 만들었다가 마지막에 모두 삭제한다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File GdsChainProbe.ps1 [-BinDir <BSL/DSL/Interface DLL 폴더>] [-MissingLayer 3]
#       -BinDir 생략 시 101_ServerBin. 배포 전에는 TEMP 빌드 폴더를 넘긴다.
param(
    [string]$BinDir = '',
    [string]$Factory = 'PROBE',
    [string]$Device = 'LAYER_FILTER_PROBE',
    [int]$MissingLayer = 3
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
$serverBin = Join-Path $dms '101_ServerBin'
if ($BinDir -eq '') { $BinDir = $serverBin }

# BSL / DSL이 ConfigurationManager.AppSettings["DMS_CONNECT_ID"]를 읽으므로 Service 설정 파일을 이 프로세스 설정으로 지정한다.
[AppDomain]::CurrentDomain.SetData('APP_CONFIG_FILE', (Join-Path $serverBin 'DACrux.SEMDMS.Service.exe.config'))
Add-Type -AssemblyName System.Configuration
$flags = [Reflection.BindingFlags]'NonPublic,Static'
$cm = [Configuration.ConfigurationManager]
$cm.GetField('s_initState', $flags).SetValue($null, 0)
$cm.GetField('s_configSystem', $flags).SetValue($null, $null)
foreach ($d in 'Oracle.ManagedDataAccess', 'Miracom.Middleware', 'DACrux.Base') {
    $p = Join-Path $serverBin "$d.dll"; if (Test-Path $p) { [void][Reflection.Assembly]::LoadFrom($p) }
}
foreach ($d in 'DACrux.SEMDMS.Interface', 'DACrux.SEMDMS.DSL', 'DACrux.SEMDMS.BSL') {
    [void][Reflection.Assembly]::LoadFrom((Join-Path $BinDir "$d.dll"))
}
Write-Host "BSL: $(Join-Path $BinDir 'DACrux.SEMDMS.BSL.dll')"

$results = New-Object Collections.Generic.List[string]
function Check($name, [bool]$ok, $detail) {
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}
function Throws([scriptblock]$block) {
    try { & $block; return '' } catch { $m = $_.Exception; while ($m.InnerException) { $m = $m.InnerException }; return $m.Message }
}
function Rows([object[]]$rows, [int]$cols) {
    $a = New-Object 'string[,]' $rows.Count, $cols
    for ($i = 0; $i -lt $rows.Count; $i++) { for ($c = 0; $c -lt $cols; $c++) { $a[$i, $c] = [string]$rows[$i][$c] } }
    return ,$a
}

$bsl = New-Object DACrux.SEMDMS.BSL.GdsChain
$import = New-Object DACrux.SEMDMS.BSL.GdsMapImport

# 0. 전제: READY Map / Layer 1, 2의 배치 도형 ID
$info = $import.GetMapInfo($Factory, $Device)
if ($info.Rows.Count -eq 0 -or $info.Rows[0]['MAP_STATUS'] -ne 'READY') { Write-Host "전제 실패: $Factory / $Device READY Map 없음. GdsImportLayerFilterProbe.ps1 -Keep 먼저 실행"; exit 2 }
$placed = New-Object DACrux.SEMDMS.DSL.TQP_GDS_PLACED_EL
$ids1 = @($placed.GetData('SELECT_PLACED_EL_PAGE', $null, [string[]]@($Factory, $Device, '1', ' ', '6')).Rows | ForEach-Object { [string]$_['PLACED_ELEMENT_ID'] })
$ids2 = @($placed.GetData('SELECT_PLACED_EL_PAGE', $null, [string[]]@($Factory, $Device, '2', ' ', '3')).Rows | ForEach-Object { [string]$_['PLACED_ELEMENT_ID'] })
Check '전제: READY Map / 도형 ID' ($ids1.Count -eq 6 -and $ids2.Count -eq 3) "Layer1 $($ids1.Count)개 / Layer2 $($ids2.Count)개"

# 이전 실행 잔여 정리
foreach ($r in @($bsl.GetChainList($Factory, $Device).Rows)) { if ([string]$r['CHAIN_CODE'] -like 'ZZPROBE*') { [void]$bsl.DeleteChain($Factory, $Device, [string]$r['CHAIN_SEQ'], 'probe') } }

# 1. 신규 Chain 저장: Input / Output(Marker) / TRACE 3 / MANUAL 1, 편집 ADD 1 / EXCLUDE 1, Layer 1·2, 규칙 (2,1) -> (1,2)
$mx = [Math]::PI * 1000; $my = -1 / 3
$info1 = [string[]]@($Factory, $Device, '', 'ZZPROBE1', 'Probe Chain 1', '-16711936', '1', '10', 'probe')
$layers = Rows @(@('1'), @('2')) 1
$rules = Rows @(,@('2', '1', '0.5')) 3
$seq = [string]$bsl.BeginChainSave($info1, $layers, $rules).Rows[0]['CHAIN_SEQ']
$members = Rows @(
    @($ids1[0], 'INPUT', 'ENDPOINT', $mx.ToString('R', [Globalization.CultureInfo]::InvariantCulture), $my.ToString('R', [Globalization.CultureInfo]::InvariantCulture)),
    @($ids2[0], 'OUTPUT', 'ENDPOINT', '12.5', '-7.25'),
    @($ids1[1], 'MEMBER', 'TRACE', '', ''), @($ids1[2], 'MEMBER', 'TRACE', '', ''), @($ids2[1], 'MEMBER', 'TRACE', '', ''),
    @($ids1[3], 'MEMBER', 'MANUAL', '', '')) 5
$edits = Rows @(@('1', $ids1[3], 'ADD', ''), @('2', $ids1[4], 'EXCLUDE', '')) 4
$key = [string[]]@($Factory, $Device, $seq)
$nm = $bsl.CreateChainMemberBatch($key, $members, 'probe')
$ne = $bsl.CreateChainEditBatch($key, $edits, 'probe')
$c = $bsl.CompleteChainSave([string[]]@($Factory, $Device, $seq, '6', '2'), 'probe').Rows[0]
Check '신규 저장 / 확정 OK' ($c['RESULT'] -eq 'OK' -and $c['VERSION_STATUS'] -eq 'READY' -and $nm -eq 6 -and $ne -eq 2) "seq=$seq member=$nm edit=$ne $($c['MESSAGE'])"

# 2. 재조회 일치
$row = @($bsl.GetChainList($Factory, $Device).Rows | Where-Object { [string]$_['CHAIN_SEQ'] -eq $seq })[0]
Check '목록 조회' ($row['CHAIN_CODE'] -eq 'ZZPROBE1' -and $row['CHAIN_NAME'] -eq 'Probe Chain 1' -and [int]$row['COLOR_ARGB'] -eq -16711936 -and [int]$row['APPLY_LAYER_RULES'] -eq 1 -and [string]$row['CELL_SIZE'] -eq '10' -and $row['VERSION_STATUS'] -eq 'READY' -and [int]$row['MEMBER_COUNT'] -eq 6 -and [int]$row['EDIT_COUNT'] -eq 2) "code=$($row['CHAIN_CODE']) cell=$($row['CELL_SIZE']) status=$($row['VERSION_STATUS']) member=$($row['MEMBER_COUNT']) edit=$($row['EDIT_COUNT'])"
$ly = @($bsl.GetChainLayers($Factory, $Device, $seq).Rows | ForEach-Object { [int]$_['LAYER_ID'] }) -join ','
$ru = @($bsl.GetChainRules($Factory, $Device, $seq).Rows)
Check 'Layer / 규칙(작은 번호 먼저)' ($ly -eq '1,2' -and $ru.Count -eq 1 -and [int]$ru[0]['FIRST_LAYER_ID'] -eq 1 -and [int]$ru[0]['SECOND_LAYER_ID'] -eq 2 -and [double]$ru[0]['TOLERANCE'] -eq 0.5) "layers=$ly rule=$($ru[0]['FIRST_LAYER_ID'])-$($ru[0]['SECOND_LAYER_ID']) tol=$($ru[0]['TOLERANCE'])"
$mem = @($bsl.GetChainMembers($Factory, $Device, $seq).Rows)
$in = @($mem | Where-Object { $_['ROLE_CODE'] -eq 'INPUT' })[0]
$mm = @($mem | Where-Object { $_['ROLE_CODE'] -eq 'MEMBER' })
Check 'Member / Marker 좌표 비트 일치' ($mem.Count -eq 6 -and [string]$in['PLACED_ELEMENT_ID'] -eq $ids1[0] -and [double]$in['MARKER_X'] -eq $mx -and [double]$in['MARKER_Y'] -eq $my -and @($mm | Where-Object { -not ($_['MARKER_X'] -is [DBNull]) }).Count -eq 0) "rows=$($mem.Count) input=$($in['PLACED_ELEMENT_ID']) x=$($in['MARKER_X']) y=$($in['MARKER_Y'])"
$ed = @($bsl.GetChainEdits($Factory, $Device, $seq).Rows)
Check '편집 기록' ($ed.Count -eq 2 -and $ed[0]['ACTION_CODE'] -eq 'ADD' -and $ed[1]['ACTION_CODE'] -eq 'EXCLUDE' -and [string]$ed[1]['PLACED_ELEMENT_ID'] -eq $ids1[4] -and $ed[0]['BATCH_EDIT_SEQ'] -is [DBNull]) "rows=$($ed.Count)"

# 3. 같은 Code 신규 저장 거부 / 확정 후 배치 거부
$e1 = Throws { $bsl.BeginChainSave($info1, $layers, $rules) }
Check '같은 Code 신규 거부' ($e1 -like '*이미 있습니다*') $e1
$e2 = Throws { $bsl.CreateChainMemberBatch($key, $members, 'probe') }
Check '확정 후 배치 거부' ($e2 -like '*READY*') $e2

# 4. 덮어쓰기: 같은 Chain 번호로 Member 3개만 다시 저장 -> 번호 유지 / 이전 내용 삭제 / VERSION 1
$info1b = [string[]]@($Factory, $Device, $seq, 'ZZPROBE1', 'Probe Chain 1 수정', '-65536', '0', '12.5', 'probe')
$seq2 = [string]$bsl.BeginChainSave($info1b, (Rows @(,@('1')) 1), $null).Rows[0]['CHAIN_SEQ']
$st = @($bsl.GetChainList($Factory, $Device).Rows | Where-Object { [string]$_['CHAIN_SEQ'] -eq $seq })[0]
$okDraft = ($st['VERSION_STATUS'] -eq 'DRAFT' -and [int]$st['MEMBER_COUNT'] -eq 0)
[void]$bsl.CreateChainMemberBatch($key, (Rows @(@($ids1[0], 'INPUT', 'ENDPOINT', '1', '2'), @($ids1[1], 'MEMBER', 'TRACE', '', ''), @($ids1[2], 'MEMBER', 'TRACE', '', '')) 5), 'probe')
$c2 = $bsl.CompleteChainSave([string[]]@($Factory, $Device, $seq, '3', '0'), 'probe').Rows[0]
$st = @($bsl.GetChainList($Factory, $Device).Rows | Where-Object { [string]$_['CHAIN_SEQ'] -eq $seq })[0]
$ver = (New-Object DACrux.SEMDMS.DSL.TQP_GDS_CHAIN_VER).GetData('SELECT_CHAIN_VER', $null, [string[]]@($Factory, $Device, $seq)).Rows[0]
Check '덮어쓰기(번호 유지 / 저장 중 DRAFT / 이전 내용 삭제)' ($seq2 -eq $seq -and $okDraft -and $c2['RESULT'] -eq 'OK' -and [int]$st['MEMBER_COUNT'] -eq 3 -and [int]$st['EDIT_COUNT'] -eq 0 -and $st['CHAIN_NAME'] -eq 'Probe Chain 1 수정' -and [string]$st['CELL_SIZE'] -eq '12.5' -and @($bsl.GetChainRules($Factory, $Device, $seq).Rows).Count -eq 0 -and [int]$ver['VERSION_NO'] -eq 1 -and [int]$ver['IS_CURRENT'] -eq 1) "seq=$seq2 member=$($st['MEMBER_COUNT']) cell=$($st['CELL_SIZE']) ver=$($ver['VERSION_NO'])"

# 5. 확정 검사: INPUT 없음 -> MISMATCH, DRAFT 유지
$info2 = [string[]]@($Factory, $Device, '', 'ZZPROBE2', 'Probe Chain 2', '-1', '0', '10', 'probe')
$seqB = [string]$bsl.BeginChainSave($info2, (Rows @(,@('1')) 1), $null).Rows[0]['CHAIN_SEQ']
[void]$bsl.CreateChainMemberBatch([string[]]@($Factory, $Device, $seqB), (Rows @(,@($ids1[5], 'MEMBER', 'TRACE', '', '')) 5), 'probe')
$m = $bsl.CompleteChainSave([string[]]@($Factory, $Device, $seqB, '1', '0'), 'probe').Rows[0]
Check 'INPUT 없음은 MISMATCH / DRAFT 유지' ($m['RESULT'] -eq 'MISMATCH' -and $m['VERSION_STATUS'] -eq 'DRAFT') "$($m['MESSAGE'])"
$m2 = $bsl.CompleteChainSave([string[]]@($Factory, $Device, $seqB, '5', '0'), 'probe').Rows[0]
Check '건수 불일치 MISMATCH' ($m2['RESULT'] -eq 'MISMATCH' -and ([string]$m2['MESSAGE']).Contains('예상 5')) "$($m2['MESSAGE'])"

# 6. Map에 없는 Layer / 도형은 DB FK로 거부
$info2b = [string[]]@($Factory, $Device, $seqB, 'ZZPROBE2', 'Probe Chain 2', '-1', '0', '10', 'probe')
$e3 = Throws { $bsl.BeginChainSave($info2b, (Rows @(@('1'), @("$MissingLayer")) 1), $null) }
Check "Map에 없는 Layer($MissingLayer) 거부" ($e3 -ne '' -and $e3 -notlike '*이미 있습니다*') ($e3 -replace "`r?`n", ' ')
[void]$bsl.BeginChainSave($info2b, (Rows @(,@('1')) 1), $null)
$e4 = Throws { $bsl.CreateChainMemberBatch([string[]]@($Factory, $Device, $seqB), (Rows @(,@('NO_SUCH/E0', 'MEMBER', 'TRACE', '', '')) 5), 'probe') }
Check 'Map에 없는 도형 거부' ($e4 -ne '') ($e4 -replace "`r?`n", ' ')

# 7. 실제 삭제(하위 테이블 CASCADE)
$d1 = $bsl.DeleteChain($Factory, $Device, $seq, 'probe')
$d2 = $bsl.DeleteChain($Factory, $Device, $seqB, 'probe')
$left = @($bsl.GetChainList($Factory, $Device).Rows | Where-Object { [string]$_['CHAIN_CODE'] -like 'ZZPROBE*' }).Count
$leftMem = @($bsl.GetChainMembers($Factory, $Device, $seq).Rows).Count + @($bsl.GetChainLayers($Factory, $Device, $seq).Rows).Count
Check '실제 삭제 / 하위 행 CASCADE' ($d1 -eq 1 -and $d2 -eq 1 -and $left -eq 0 -and $leftMem -eq 0) "deleted=$d1,$d2 left=$left child=$leftMem"

$fail = @($results | Where-Object { $_.StartsWith('FAIL') }).Count
if ($fail -eq 0) { Write-Host 'GDS_CHAIN_PROBE=PASS' } else { Write-Host "GDS_CHAIN_PROBE=FAIL ($fail)"; exit 1 }
