# 테이블 명세서(xlsx) 만들기 (2026-10-15)
# 대상: POC(TQP_GDS_*) / Common(TQC_*) / DMS(TQD_*) / YMS(TQP_* + TD_*). 기본은 네 개 모두.
# 순서: 1) 로컬 DB 컨테이너에서 메타데이터 추출(읽기 전용, extract_table_meta.sql)
#       2) POC = build_table_spec.py, Common / DMS / YMS = build_area_spec.py + desc_<영역>.json(컬럼 설명)
#       3) Excel 로 열어 수식을 다시 계산하고 저장(계산 값 / 행 높이 저장, 수식 오류 확인). Excel 이 없으면 건너뛴다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File Make-TableSpec.ps1 [-Target All|POC|Common|DMS|YMS] [-OutDir <폴더>] [-Container oracle19c-DMS] [-SkipExcel]
# 기본 출력: 문서\자료\04_시스템_개발자료\<오늘>_<POC|Common|DMS|YMS>_테이블_명세서.xlsx (같은 이름이 있으면 덮어쓴다)
# 테이블 / 컬럼이 바뀌면 설명 JSON(desc_*.json) 이나 build_table_spec.py 의 목록을 먼저 고친다(실행 결과의 '경고' 줄 참고).
param(
    [ValidateSet('All', 'POC', 'Common', 'DMS', 'YMS')][string]$Target = 'All',
    [string]$OutDir = '',
    [string]$Container = 'oracle19c-DMS',
    [switch]$SkipExcel
)
$ErrorActionPreference = 'Stop'
$root = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..\..')).Path
if ($OutDir -eq '') { $OutDir = Join-Path $root '문서\자료\04_시스템_개발자료' }
$date = Get-Date -Format 'yyyy-MM-dd'
$work = Join-Path $env:TEMP 'chippac\tablespec'
New-Item -ItemType Directory -Force $work | Out-Null

# 1) 추출
docker exec $Container bash -lc 'mkdir -p /tmp/tablespec'
if ($LASTEXITCODE -ne 0) { throw "DB 컨테이너($Container)에 접속하지 못했습니다." }
docker cp (Join-Path $PSScriptRoot 'extract_table_meta.sql') "${Container}:/tmp/tablespec/extract_table_meta.sql"
docker exec -e NLS_LANG=AMERICAN_AMERICA.AL32UTF8 $Container bash -lc 'sqlplus -s / as sysdba @/tmp/tablespec/extract_table_meta.sql' | Out-Null
if ($LASTEXITCODE -ne 0) { throw "메타데이터 추출 실패 (sqlplus 종료 코드 $LASTEXITCODE)" }
foreach ($f in 'tbl.csv', 'col.csv', 'cons.csv', 'idx.csv', 'def.txt', 'seq.csv', 'et_item.csv') {
    docker cp "${Container}:/tmp/tablespec/$f" (Join-Path $work $f)
    if ($LASTEXITCODE -ne 0) { throw "$f 를 가져오지 못했습니다." }
}
Write-Host "추출: $work"

# 2) 생성
$env:PYTHONIOENCODING = 'utf-8'
$targets = if ($Target -eq 'All') { @('POC', 'Common', 'DMS', 'YMS') } else { @($Target) }
$outputs = @()
foreach ($t in $targets) {
    $out = Join-Path $OutDir ("{0}_{1}_테이블_명세서.xlsx" -f $date, $t)
    if ($t -eq 'POC') {
        python (Join-Path $PSScriptRoot 'build_table_spec.py') $work $out
    }
    else {
        $desc = Join-Path $PSScriptRoot ("desc_{0}.json" -f $t.ToLower())
        if (-not (Test-Path $desc)) { Write-Warning "$t 설명 파일이 없어 건너뜁니다: $desc"; continue }
        python (Join-Path $PSScriptRoot 'build_area_spec.py') $work $t.ToLower() $desc $out
    }
    if ($LASTEXITCODE -ne 0) { throw "$t 명세서 생성 실패" }
    $outputs += $out
}

# 3) Excel 로 다시 계산 / 저장
if ($SkipExcel) { Write-Host '완료 (Excel 저장 건너뜀: 수식 값은 파일을 열 때 계산된다)'; return }
try { $xl = New-Object -ComObject Excel.Application } catch { Write-Warning 'Excel 이 없어 다시 계산 / 저장을 건너뜁니다.'; return }
$xl.Visible = $false
$xl.DisplayAlerts = $false
try {
    foreach ($out in $outputs) {
        $wb = $xl.Workbooks.Open($out, 0, $false)
        $xl.CalculateFull()
        $formulas = 0
        $errors = @()
        foreach ($ws in $wb.Worksheets) {
            foreach ($c in $ws.UsedRange.Cells) {
                if ($c.HasFormula) {
                    $formulas++
                    if ($c.Text -like '#*') { $errors += "$($ws.Name)!$($c.Address(0, 0)) = $($c.Text)" }
                }
            }
        }
        $sheets = $wb.Worksheets.Count
        $wb.Worksheets.Item(1).Activate()
        $wb.Save()
        $wb.Close($false)
        Write-Host ("Excel: {0} / 시트 {1}개 / 수식 {2}개 / 오류 {3}개" -f (Split-Path $out -Leaf), $sheets, $formulas, $errors.Count)
        if ($errors.Count -gt 0) { $errors | Select-Object -First 10 | ForEach-Object { Write-Warning $_ } }
    }
}
finally {
    $xl.Quit()
    [void][Runtime.InteropServices.Marshal]::ReleaseComObject($xl)
}
Write-Host '완료'
