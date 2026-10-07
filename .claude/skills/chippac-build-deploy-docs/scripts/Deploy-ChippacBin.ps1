<#
.SYNOPSIS
  ChippacPOC build outputs -> DMS\100_ClientBin / DMS\101_ServerBin / DMS\100_ClientBin\GdsMap deploy helper.

.DESCRIPTION
  Why: deploying overwrites the DLLs the user actually runs. Every past deploy repeated the same
  manual steps (check running processes, back up, copy, record MD5 before/after). This script does
  them in one place so nothing is skipped and the change log gets exact numbers.

  Flow:
    1. Resolve target folders under <ProjectRoot>\DMS.
    2. Stop (exit 2, nothing copied) if DACrux client / services / GDS viewer are running,
       because Windows locks loaded DLLs and a half-copied set breaks the client.
    3. Print plan: file, target, MD5 before -> MD5 after (first 8 chars, same style as the change log).
    4. -DryRun: stop here. Otherwise back up existing files, copy, and verify the copied MD5.
    5. Print markdown lines that can be pasted into DMS\Assistant\변경이력\YYYY-MM-DD.md.

  Matching .pdb files next to each source file are copied too (disable with -NoPdb).
  The script is ASCII-only on purpose so Windows PowerShell 5.1 reads it without a BOM.

.EXAMPLE
  powershell -NoProfile -ExecutionPolicy Bypass -File Deploy-ChippacBin.ps1 `
    -ProjectRoot 'D:\...\ChippacPOC' -Tag CLAUDE-019 -DryRun `
    -ServerBin "$env:TEMP\b\DACrux.SEMDMS.BSL.dll" -ClientBin "$env:TEMP\b\DACrux.SEMDMS.RO.dll"

  Exit codes: 0 ok / 1 input error / 2 blocking process running / 3 MD5 verify failed
#>
param(
    [string]$ProjectRoot = '',
    [string[]]$ClientBin = @(),
    [string[]]$ServerBin = @(),
    [string[]]$GdsMap = @(),
    [string]$Tag = 'deploy',
    [string]$BackupRoot = (Join-Path $env:TEMP 'claude\deploy_backup'),
    [switch]$DryRun,
    [switch]$NoPdb
)

$ErrorActionPreference = 'Stop'

# "powershell -File" passes "a.dll,b.dll" as ONE string (only "&" / -Command build real arrays).
# Split on commas so both call styles work.
function Split-List([string[]]$items) { @($items | ForEach-Object { $_ -split ',' } | ForEach-Object { $_.Trim() } | Where-Object { $_ -ne '' }) }
$ClientBin = Split-List $ClientBin
$ServerBin = Split-List $ServerBin
$GdsMap    = Split-List $GdsMap

function Get-Md5Short([string]$path) {
    if (-not (Test-Path -LiteralPath $path)) { return 'NEW' }
    return (Get-FileHash -LiteralPath $path -Algorithm MD5).Hash.Substring(0, 8).ToLower()
}

# --- 1. project root: explicit parameter, otherwise walk up from the current folder ---
if ($ProjectRoot -eq '') {
    $dir = (Get-Location).Path
    while ($dir -and -not (Test-Path -LiteralPath (Join-Path $dir 'DMS\100_ClientBin'))) {
        $dir = Split-Path $dir -Parent
    }
    $ProjectRoot = $dir
}
if (-not $ProjectRoot -or -not (Test-Path -LiteralPath (Join-Path $ProjectRoot 'DMS\100_ClientBin'))) {
    Write-Host 'ERROR: ProjectRoot not found (folder that contains DMS\100_ClientBin). Pass -ProjectRoot.'
    exit 1
}

$targets = [ordered]@{
    'ServerBin' = @{ Dir = (Join-Path $ProjectRoot 'DMS\101_ServerBin');       Files = $ServerBin }
    'ClientBin' = @{ Dir = (Join-Path $ProjectRoot 'DMS\100_ClientBin');       Files = $ClientBin }
    'GdsMap'    = @{ Dir = (Join-Path $ProjectRoot 'DMS\100_ClientBin\GdsMap'); Files = $GdsMap }
}

# --- build the plan (source -> destination), adding .pdb next to each file ---
$plan = @()
foreach ($key in $targets.Keys) {
    $t = $targets[$key]
    foreach ($src in $t.Files) {
        if (-not (Test-Path -LiteralPath $src)) { Write-Host "ERROR: source not found: $src"; exit 1 }
        # Runtime configs in ClientBin / ServerBin differ from build outputs (e.g. GdsMap exe.config 1620B vs 177B).
        # Overwriting them breaks the service / client settings, so they are never deployed by this script.
        if ($src -like '*.config') { Write-Host "ERROR: .config files are not deployed by this script: $src (edit the target config by hand)"; exit 1 }
        $sources = @($src)
        if (-not $NoPdb) {
            $pdb = [IO.Path]::ChangeExtension($src, '.pdb')
            if (Test-Path -LiteralPath $pdb) { $sources += $pdb }
        }
        foreach ($s in $sources) {
            $name = Split-Path $s -Leaf
            $dst = Join-Path $t.Dir $name
            $plan += [pscustomobject]@{
                Target = $key; File = $name; Source = $s; Dest = $dst
                Before = (Get-Md5Short $dst); New = (Get-Md5Short $s)
            }
        }
    }
}
if ($plan.Count -eq 0) { Write-Host 'ERROR: nothing to deploy. Use -ClientBin / -ServerBin / -GdsMap.'; exit 1 }

# --- 2. running processes lock DLLs; never kill them here, the user decides ---
$running = Get-Process -ErrorAction SilentlyContinue |
    Where-Object { $_.ProcessName -like 'DACrux*' -or $_.ProcessName -like 'NexplantQMS.GdsMap*' }

# --- 3. plan output ---
Write-Host "ProjectRoot : $ProjectRoot"
Write-Host "Tag         : $Tag"
Write-Host ''
$plan | Format-Table Target, File, Before, New -AutoSize | Out-String | Write-Host
$same = @($plan | Where-Object { $_.Before -eq $_.New })
if ($same.Count -gt 0) { Write-Host "NOTE: already identical (copy has no effect): $(($same | ForEach-Object { $_.File }) -join ', ')" }

if ($running) {
    Write-Host 'BLOCKED: these processes are running. Ask the user to close them, then run again:'
    $running | ForEach-Object { Write-Host ("  {0} (PID {1})" -f $_.ProcessName, $_.Id) }
    Write-Host 'DEPLOY=BLOCKED'
    exit 2
}
Write-Host 'Process check: no DACrux / GdsMap process running.'
# Visual Studio / MSBuild do not lock ClientBin / ServerBin DLLs, but a Debug build inside VS writes
# straight into those folders (csproj OutputPath). Report them so the user knows another build may overwrite this deploy.
$builders = Get-Process -ErrorAction SilentlyContinue | Where-Object { $_.ProcessName -eq 'devenv' -or $_.ProcessName -eq 'MSBuild' }
if ($builders) { Write-Host ("NOTE: build tools running ({0}). Another session or VS build may also write to ClientBin / ServerBin." -f (($builders | ForEach-Object { $_.ProcessName } | Sort-Object -Unique) -join ', ')) }

if ($DryRun) {
    Write-Host 'DRY RUN: nothing copied. Show this plan to the user and get approval before running without -DryRun.'
    Write-Host 'DEPLOY=DRYRUN'
    exit 0
}

# --- 4. backup, copy, verify ---
$stamp = Get-Date -Format 'yyyyMMdd_HHmmss'
$backupDir = Join-Path $BackupRoot "${stamp}_$Tag"
foreach ($p in $plan) {
    if ($p.Before -ne 'NEW') {
        $bdir = Join-Path $backupDir $p.Target
        New-Item -ItemType Directory -Force -Path $bdir | Out-Null
        Copy-Item -LiteralPath $p.Dest -Destination (Join-Path $bdir $p.File) -Force
    }
}
$failed = @()
foreach ($p in $plan) {
    Copy-Item -LiteralPath $p.Source -Destination $p.Dest -Force
    $after = Get-Md5Short $p.Dest
    $p | Add-Member -NotePropertyName After -NotePropertyValue $after
    if ($after -ne $p.New) { $failed += $p.File }
}

# --- 5. lines for the change log ---
$time = Get-Date -Format 'yyyy-MM-dd HH:mm'
Write-Host ''
Write-Host '--- change log lines ---'
Write-Host "- Deploy $time ($Tag, no DACrux process running). Backup: ``$backupDir``"
foreach ($key in $targets.Keys) {
    $rows = @($plan | Where-Object { $_.Target -eq $key -and $_.File -notlike '*.pdb' })
    if ($rows.Count -eq 0) { continue }
    $parts = $rows | ForEach-Object { "{0} ``{1}`` -> ``{2}``" -f $_.File, $_.Before, $_.After }
    Write-Host ("  - {0}: {1}" -f $key, ($parts -join ' / '))
}

if ($failed.Count -gt 0) {
    Write-Host "ERROR: MD5 mismatch after copy: $($failed -join ', '). Restore from $backupDir if needed."
    Write-Host 'DEPLOY=FAIL'
    exit 3
}
Write-Host 'DEPLOY=PASS'
exit 0
