# F07 데이터 검증 K11 / K11b 용 TQD_INSP_INFO 덤프 (2026-10-19)
# 목적: KLARF STEP 하나의 검사 요약 행을 CSV 로 받는다. DB 쓰기 없음(SELECT 만).
#   k_step_info.csv       = Defect Map 정보 창 / Chart 가 쓰는 GetStepInfo(GET_INSP_INFO_MULTI_SEQ, STEP 당 한 행)
#   k_insp_info_tests.csv = 같은 STEP 의 Test 별 행(TEST / DEFECTS / INSPECTED_DIE / DEFECTIVE_DIE)
# 전제: 로컬 docker oracle19c-DMS 기동. DSL / config 는 101_ServerBin 것을 읽기만 한다. 쿼리 XML 은 config 가 가리키는 08_DBQuery 를 읽는다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File f07data_insp_info_dump.ps1 -StepSeq 582 -OutDir <db csv 폴더>
#       이어서 python -I -X utf8 f07data_klarf_check.py <KLARF 파일> <db csv 폴더>
# 출력: 마지막 줄 INSP_INFO_DUMP=OK (k_step_info n행 / k_insp_info_tests n행)
param(
    [Parameter(Mandatory = $true)][long]$StepSeq,
    [Parameter(Mandatory = $true)][string]$OutDir,
    [string]$BinDir = ''
)
$ErrorActionPreference = 'Stop'
$root = (Resolve-Path (Join-Path $PSScriptRoot '..\..')).Path
if ($BinDir -eq '') { $BinDir = Join-Path $root 'DMS\101_ServerBin' }
New-Item -ItemType Directory -Force $OutDir | Out-Null

# 운영 config 는 고치지 않고 TEMP 사본을 읽게 한다
$tmp = Join-Path $env:TEMP 'chippac\insp_info_dump'
New-Item -ItemType Directory -Force $tmp | Out-Null
$cfg = Join-Path $tmp 'dump.exe.config'
Copy-Item (Join-Path $BinDir 'DACrux.SEMDMS.Service.exe.config') $cfg -Force
Add-Type -AssemblyName System.Configuration
[AppDomain]::CurrentDomain.SetData('APP_CONFIG_FILE', $cfg)
$flags = [Reflection.BindingFlags]'NonPublic,Static'
$cm = [Configuration.ConfigurationManager]
$cm.GetField('s_initState', $flags).SetValue($null, 0)
$cm.GetField('s_configSystem', $flags).SetValue($null, $null)
foreach ($d in 'Oracle.ManagedDataAccess', 'Miracom.Middleware', 'DACrux.Base', 'DACrux.SEMDMS.DSL') {
    $p = Join-Path $BinDir "$d.dll"; if (Test-Path $p) { [void][Reflection.Assembly]::LoadFrom($p) }
}

# 미들웨어가 호출 클래스를 스택에서 찾으므로 NoInlining 래퍼 안에서 호출한다
$helper = @'
using System.Data;
using System.Runtime.CompilerServices;
public static class InspInfoDumpHelper
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static DataTable StepInfo(long step) { return new DACrux.SEMDMS.DSL.TQD_INSP_INFO().GetStepInfo(new long[] { step }); }
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static DataTable Select(string sql) { return new DACrux.SEMDMS.DSL.TQD_INSP_INFO().GetData("DYNAMIC", new string[] { sql }, null); }
}
'@
Add-Type -TypeDefinition $helper -ReferencedAssemblies @((Join-Path $BinDir 'DACrux.SEMDMS.DSL.dll'), (Join-Path $BinDir 'Miracom.Middleware.dll'), 'System.Data', 'System.Xml')

# DataTable 은 변수에 담아 넘긴다(괄호 식으로 넘기면 PowerShell 이 행 배열로 풀 수 있다)
function Save([Data.DataTable]$dt, $name) {
    $cols = @($dt.Columns | ForEach-Object { $_.ColumnName })
    $dt.Rows | Select-Object -Property $cols | Export-Csv -Path (Join-Path $OutDir "$name.csv") -NoTypeInformation -Encoding UTF8
    return $dt.Rows.Count
}

$stepInfo = [InspInfoDumpHelper]::StepInfo($StepSeq)
$n1 = Save $stepInfo 'k_step_info'
$sql = "SELECT STEP_SEQ, TEST, DEFECTS, INSPECTED_DIE, DEFECTIVE_DIE, ""COMMENT"" CMT FROM TQD_INSP_INFO WHERE STEP_SEQ = $StepSeq ORDER BY TEST"
$tests = [InspInfoDumpHelper]::Select($sql)
$n2 = Save $tests 'k_insp_info_tests'
Write-Host "INSP_INFO_DUMP=OK (k_step_info $($n1)행 / k_insp_info_tests $($n2)행)"
