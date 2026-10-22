# E-Test Parameter 의 YMS 이름(TQP_GDS_ET_ITEM.YMS_PARAM_NAME) 채우기 (2026-10-21)
# 목적: YMS_PARAM_NAME 컬럼을 추가하기 전에 등록된 행에 EtYmsRule.ToYmsName 값을 넣는다.
#       로컬 DB 용 1회 작업이다. 고객 DB 는 등록된 행이 없으면 필요 없다.
# 설계: 문서/설계/04_ETest/2026-10-21_ETest_YMS_컬럼이름_30자_설계.md 6절 / 7절 2단계
# 처리: 로컬 DB 컨테이너에서 빈 행 조회(sqlplus, 읽기) -> Interface DLL 규칙으로 계산
#       -> 같은 Factory + Device + 공정 안 YMS 이름 겹침 검사(이미 채운 행 포함) -> UPDATE 문 파일 -> -Apply 면 실행 후 빈 행 0 확인
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File EtYmsNameFill.ps1 [-BinDir <Interface 폴더>] [-Container oracle19c-DMS] [-Owner DMSMGR] [-Apply]
#       -BinDir 생략 시 100_ClientBin. 배포 전에는 TEMP 빌드 폴더를 넘긴다. -Apply 가 없으면 UPDATE 문 파일만 만든다.
# 결과: 마지막 줄 ET_YMS_NAME_FILL=PASS 또는 FAIL
param(
    [string]$BinDir = '',
    [string]$Container = 'oracle19c-DMS',
    [ValidatePattern('^[A-Za-z][A-Za-z0-9_$#]{0,29}$')][string]$Owner = 'DMSMGR',
    [switch]$Apply
)
$ErrorActionPreference = 'Stop'
$dms = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..')).Path
if ($BinDir -eq '') { $BinDir = Join-Path $dms '100_ClientBin' }
$Owner = $Owner.ToUpper()
[void][Reflection.Assembly]::LoadFrom((Join-Path $BinDir 'DACrux.SEMDMS.Interface.dll'))
$R = [DACrux.SEMDMS.Interface.EtYmsRule]
$work = Join-Path $env:TEMP 'chippac\etymsfill'
New-Item -ItemType Directory -Force $work | Out-Null
$utf8 = New-Object Text.UTF8Encoding($false)

function Invoke-Sql([string]$name, [string]$sql) {
    [IO.File]::WriteAllText((Join-Path $work $name), $sql, $utf8)
    docker exec $Container bash -lc 'mkdir -p /tmp/etymsfill'
    docker cp (Join-Path $work $name) "${Container}:/tmp/etymsfill/$name"
    $out = docker exec -e NLS_LANG=AMERICAN_AMERICA.AL32UTF8 $Container bash -lc "sqlplus -s / as sysdba @/tmp/etymsfill/$name"
    if ($LASTEXITCODE -ne 0) { throw "sqlplus 실패 ($name): $out" }
    return $out
}

# 1. 행 읽기 (빈 행 + 이미 채운 행)
$select = @"
WHENEVER SQLERROR EXIT SQL.SQLCODE
SET PAGESIZE 0 LINESIZE 2000 FEEDBACK OFF HEADING OFF TRIMSPOOL ON
SELECT ET_ITEM_SEQ || '|' || FACTORY || '|' || DEVICE_ID || '|' || TEST_PROGRAM || '|' || ITEM_NAME || '|' || MEAS_TYPE || '|' || PARA_NAME
       || '|' || COND_VALUE || '|' || CHAIN_NAME || '|' || CHAIN_NAME2 || '|' || YMS_PARAM_NAME
  FROM $Owner.TQP_GDS_ET_ITEM ORDER BY FACTORY, DEVICE_ID, TEST_PROGRAM, ET_ITEM_SEQ;
EXIT
"@
$rows = @(Invoke-Sql 'select.sql' $select | Where-Object { $_ -match '^\d+\|' } | ForEach-Object {
    $c = $_.Split('|')
    [pscustomobject]@{ Seq = $c[0]; Group = "$($c[1])|$($c[2])|$($c[3])"; Item = $c[4]; Meas = $c[5]; Para = $c[6]; Cond = $c[7]; Chain1 = $c[8]; Chain2 = $c[9]; Yms = $c[10].Trim() }
})
$empty = @($rows | Where-Object { $_.Yms -eq '' })
Write-Host ("행 {0} / 빈 YMS 이름 {1}" -f $rows.Count, $empty.Count)

# 2. 계산 + 검사
$updates = New-Object Collections.Generic.List[string]
$problems = New-Object Collections.Generic.List[string]
foreach ($g in ($rows | Group-Object Group)) {
    $items = New-Object Collections.Generic.List[string]
    $names = New-Object Collections.Generic.List[string]
    foreach ($row in $g.Group) {
        $name = $row.Yms
        if ($name -eq '') {
            $name = $R::ToYmsName($row.Item, $row.Meas, $row.Para, $row.Cond, $row.Chain1, $row.Chain2)
            $updates.Add("UPDATE $Owner.TQP_GDS_ET_ITEM SET YMS_PARAM_NAME = '$name' WHERE ET_ITEM_SEQ = $($row.Seq) AND YMS_PARAM_NAME IS NULL;")
        }
        $items.Add($row.Item); $names.Add($name)
    }
    foreach ($p in $R::FindYmsNameProblems($items, $names)) { $problems.Add("[$($g.Name)] $p") }
}
$file = Join-Path $work ("{0}_yms_name_fill.sql" -f $Owner)
[IO.File]::WriteAllText($file, (@('WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK', 'SET FEEDBACK OFF') + $updates + @('COMMIT;', 'EXIT')) -join "`n", $utf8)
Write-Host ("UPDATE {0}개 -> {1}" -f $updates.Count, $file)
$problems | ForEach-Object { Write-Host "문제: $_" }
if ($problems.Count -gt 0) { Write-Host 'ET_YMS_NAME_FILL=FAIL (겹침 / 형식 문제)'; exit 1 }

# 3. 실행
if (-not $Apply) { Write-Host 'ET_YMS_NAME_FILL=PASS (파일만 만듦, -Apply 없음)'; return }
if ($updates.Count -gt 0) {
    docker exec $Container bash -lc 'mkdir -p /tmp/etymsfill'
    docker cp $file "${Container}:/tmp/etymsfill/fill.sql"
    $out = docker exec -e NLS_LANG=AMERICAN_AMERICA.AL32UTF8 $Container bash -lc 'sqlplus -s / as sysdba @/tmp/etymsfill/fill.sql'
    if ($LASTEXITCODE -ne 0) { throw "UPDATE 실패: $out" }
}
$left = (Invoke-Sql 'check.sql' "SET PAGESIZE 0 FEEDBACK OFF HEADING OFF`nSELECT COUNT(*) FROM $Owner.TQP_GDS_ET_ITEM WHERE YMS_PARAM_NAME IS NULL;`nEXIT" | Where-Object { $_ -match '^\s*\d+\s*$' } | Select-Object -First 1).Trim()
docker exec $Container bash -lc 'rm -rf /tmp/etymsfill'
Write-Host ("남은 빈 행: {0}" -f $left)
Write-Host ("ET_YMS_NAME_FILL={0} (UPDATE {1})" -f $(if ($left -eq '0') { 'PASS' } else { 'FAIL' }), $updates.Count)
if ($left -ne '0') { exit 1 }
