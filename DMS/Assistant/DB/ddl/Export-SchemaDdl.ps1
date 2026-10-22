# 스키마 DDL 파일 만들기 (2026-10-21)
# 로컬 DB 컨테이너의 스키마(기본 DMSMGR)에서 데이터 없이 DDL 만 뽑아 .sql 파일로 저장한다(읽기 전용, extract_schema_ddl.sql).
# 결과 파일에는 스키마 이름 / 테이블스페이스 / 저장 옵션이 없다. 대상 DB 에서 객체를 가질 사용자로 접속해 실행하면 그 사용자 아래에 만들어진다.
#   대상 사용자에 권한이 없으면 DBA 가 먼저 grant_target_user.sql 을 실행한다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File Export-SchemaDdl.ps1 [-Owner DMSMGR] [-Container oracle19c-DMS] [-OutFile <파일>]
# 기본 출력: 이 폴더의 <오늘>_<Owner>_schema_ddl.sql (같은 이름이 있으면 덮어쓴다, UTF-8 / BOM 없음)
# 대상 DB 에서 실행할 때는 주석의 한글이 깨지지 않게 NLS_LANG 을 UTF-8 로 지정한다.  예) set NLS_LANG=AMERICAN_AMERICA.AL32UTF8
param(
    [ValidatePattern('^[A-Za-z][A-Za-z0-9_$#]{0,127}$')][string]$Owner = 'DMSMGR',
    [string]$Container = 'oracle19c-DMS',
    [string]$OutFile = ''
)
$ErrorActionPreference = 'Stop'
$Owner = $Owner.ToUpper()
if ($OutFile -eq '') { $OutFile = Join-Path $PSScriptRoot ("{0}_{1}_schema_ddl.sql" -f (Get-Date -Format 'yyyy-MM-dd'), $Owner) }

# 1) 추출
docker exec $Container bash -lc 'mkdir -p /tmp/schemaddl && rm -f /tmp/schemaddl/schema_ddl.sql'
if ($LASTEXITCODE -ne 0) { throw "DB 컨테이너($Container)에 접속하지 못했습니다." }
docker cp (Join-Path $PSScriptRoot 'extract_schema_ddl.sql') "${Container}:/tmp/schemaddl/extract_schema_ddl.sql"
if ($LASTEXITCODE -ne 0) { throw '추출 SQL 을 컨테이너로 복사하지 못했습니다.' }
docker exec -e NLS_LANG=AMERICAN_AMERICA.AL32UTF8 $Container bash -lc "sqlplus -s / as sysdba @/tmp/schemaddl/extract_schema_ddl.sql $Owner"
if ($LASTEXITCODE -ne 0) { throw "DDL 추출 실패 (sqlplus 종료 코드 $LASTEXITCODE)" }

# 2) 가져오기 (TEMP 를 거쳐 한글 / 공백 경로 문제를 피한다)
$tmp = Join-Path $env:TEMP ("schema_ddl_{0}.sql" -f $Owner)
docker cp "${Container}:/tmp/schemaddl/schema_ddl.sql" $tmp
if ($LASTEXITCODE -ne 0) { throw '결과 파일을 가져오지 못했습니다.' }
Move-Item -LiteralPath $tmp -Destination $OutFile -Force

# 3) 결과 점검
$text = [IO.File]::ReadAllText($OutFile, [Text.Encoding]::UTF8)
$count = [ordered]@{
    'CREATE SEQUENCE'   = ([regex]::Matches($text, '(?m)^\s*CREATE SEQUENCE ')).Count
    'CREATE TABLE'      = ([regex]::Matches($text, '(?m)^\s*CREATE (GLOBAL TEMPORARY )?TABLE ')).Count
    'COMMENT ON'        = ([regex]::Matches($text, '(?m)^\s*COMMENT ON ')).Count
    'CREATE INDEX'      = ([regex]::Matches($text, '(?m)^\s*CREATE (UNIQUE |BITMAP )?INDEX ')).Count
    'ALTER TABLE PK/UK' = ([regex]::Matches($text, '(?m)^\s*ALTER TABLE [^\n]*(PRIMARY KEY|UNIQUE)')).Count
    'ALTER TABLE FK'    = ([regex]::Matches($text, '(?m)^\s*ALTER TABLE [^\n]*FOREIGN KEY')).Count
    'CREATE OR REPLACE' = ([regex]::Matches($text, '(?m)^\s*CREATE OR REPLACE ')).Count
}
Write-Host "저장: $OutFile ($([Math]::Round((Get-Item -LiteralPath $OutFile).Length / 1KB)) KB)"
$count.GetEnumerator() | ForEach-Object { Write-Host ("  {0,-18} {1}" -f $_.Key, $_.Value) }
$head = ($text -split "`n" | Where-Object { $_ -match '^-- (제외한 테이블|원본 개수)' }) -join "`n"
if ($head) { Write-Host $head }
$warn = @()
if ($text -match ('"' + [regex]::Escape($Owner) + '"\.')) { $warn += "스키마 이름(`"$Owner`".)이 남아 있습니다." }
if ($text -match '(?m)\bTABLESPACE\s+"') { $warn += 'TABLESPACE 절이 남아 있습니다.' }
if ($text -match '"SYS_C\d+"') { $warn += '자동 생성 이름(SYS_C...)이 남아 있습니다.' }
if ($text -match 'ORA-\d{5}|SP2-\d{4}') { $warn += '추출 결과에 오류 문구(ORA- / SP2-)가 있습니다.' }
$warn | ForEach-Object { Write-Warning $_ }
Write-Host '완료'
