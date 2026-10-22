# 기본 데이터 INSERT 파일 만들기 (2026-10-21)
# 로컬 DB 컨테이너의 스키마(기본 DMSMGR)에서 로그인 후 화면 사용에 필요한 기본 데이터만 INSERT 문으로 뽑는다(읽기 전용, extract_base_data.sql).
# 대상과 제외 범위는 extract_base_data.sql 머리말을 본다. POC 데이터는 넣지 않는다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File Export-BaseData.ps1 [-Owner DMSMGR] [-Container oracle19c-DMS] [-OutFile <파일>]
# 기본 출력: 이 폴더의 <오늘>_<Owner>_base_data.sql (같은 이름이 있으면 덮어쓴다, UTF-8 / BOM 없음)
# 대상 DB 에서는 스키마 DDL 을 먼저 실행하고, NLS_LANG 을 UTF-8 로 지정한 뒤 실행한다.  예) set NLS_LANG=AMERICAN_AMERICA.AL32UTF8
param(
    [ValidatePattern('^[A-Za-z][A-Za-z0-9_$#]{0,127}$')][string]$Owner = 'DMSMGR',
    [string]$Container = 'oracle19c-DMS',
    [string]$OutFile = ''
)
$ErrorActionPreference = 'Stop'
$Owner = $Owner.ToUpper()
if ($OutFile -eq '') { $OutFile = Join-Path $PSScriptRoot ("{0}_{1}_base_data.sql" -f (Get-Date -Format 'yyyy-MM-dd'), $Owner) }

# 1) 추출
docker exec $Container bash -lc 'mkdir -p /tmp/basedata && rm -f /tmp/basedata/base_data.sql'
if ($LASTEXITCODE -ne 0) { throw "DB 컨테이너($Container)에 접속하지 못했습니다." }
docker cp (Join-Path $PSScriptRoot 'extract_base_data.sql') "${Container}:/tmp/basedata/extract_base_data.sql"
if ($LASTEXITCODE -ne 0) { throw '추출 SQL 을 컨테이너로 복사하지 못했습니다.' }
docker exec -e NLS_LANG=AMERICAN_AMERICA.AL32UTF8 $Container bash -lc "sqlplus -s / as sysdba @/tmp/basedata/extract_base_data.sql $Owner"
if ($LASTEXITCODE -ne 0) { throw "기본 데이터 추출 실패 (sqlplus 종료 코드 $LASTEXITCODE)" }

# 2) 가져오기 (TEMP 를 거쳐 한글 / 공백 경로 문제를 피한다)
$tmp = Join-Path $env:TEMP ("base_data_{0}.sql" -f $Owner)
docker cp "${Container}:/tmp/basedata/base_data.sql" $tmp
if ($LASTEXITCODE -ne 0) { throw '결과 파일을 가져오지 못했습니다.' }
Move-Item -LiteralPath $tmp -Destination $OutFile -Force

# 3) 결과 점검: 표별 건수 / INSERT 수 / 오류 문구
$text = [IO.File]::ReadAllText($OutFile, [Text.Encoding]::UTF8)
Write-Host "저장: $OutFile ($([Math]::Round((Get-Item -LiteralPath $OutFile).Length / 1KB)) KB)"
($text -split "`n") | Where-Object { $_ -match '^-- (TQC|TQD)_\S+ \d+건' } | ForEach-Object { Write-Host ('  ' + $_.TrimEnd().Substring(3)) }
Write-Host ("  INSERT 문 {0}개" -f ([regex]::Matches($text, '(?m)^INSERT INTO ')).Count)
if ($text -match 'ORA-\d{5}|SP2-\d{4}') { Write-Warning '추출 결과에 오류 문구(ORA- / SP2-)가 있습니다.' }
Write-Host '완료'
