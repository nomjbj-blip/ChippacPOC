# KlarfImageFtpBase Probe (2026-10-21)
# 목적: KLARF 업로드 이미지 경로 규칙(KlarfUpload.ToFtpRelativePath / SaveFilesToImageStore)을 DB 없이 확인한다.
#   - KLARF_IMAGE_FTP_BASE 없음 = 지금까지 규칙(드라이브 루트를 뺀 경로, 예 FTP/SCK/20261021/Lot)
#   - 있음 = 그 폴더 기준 경로(예 FTP /DMS_IMAGE = C:\FTP\DMS_IMAGE -> 20261021/Lot), 화면 경로 = MAIN_PATH + 이 경로
#   - 기준 폴더 밖 / 상대 경로 / 이름 앞부분만 같은 폴더는 파일 복사 전에 오류
#   - 저장 폴더 = 날짜 \ Device \ Lot (2026-10-21), 썸네일 = 'thumb_' + 이름의 JPEG(가로세로 절반), 여러 장 TIFF 프레임 / 읽을 수 없는 이미지 처리
# 전제: 샘플 SampleFile\26_10_06 Sample File\XD106.02 + Defect Images(15장). DB / FTP 접속 없음.
# 결과 데이터: %TEMP%\chippac\ftpbase_probe 아래에만 파일을 만든다(매 실행 시 비움). 운영 config 는 읽지 않는다.
# 실행: powershell -NoProfile -ExecutionPolicy Bypass -File KlarfImageFtpBaseProbe.ps1 [-BinDir <BSL / Interface 폴더>]
# 출력: 'PASS / 이름 / 상세' 줄들 + 마지막 KLARF_IMAGE_FTP_BASE_PROBE=PASS|FAIL (n/m), 종료 코드 0 / 1
param([string]$BinDir = '')
$ErrorActionPreference = 'Stop'
$root = (Resolve-Path (Join-Path $PSScriptRoot '..\..\..\..')).Path
$serverBin = Join-Path $root 'DMS\101_ServerBin'
if ($BinDir -eq '') { $BinDir = $serverBin }
$sampleDir = Join-Path $root 'SampleFile\26_10_06 Sample File'

$results = New-Object Collections.Generic.List[string]
$script:fail = 0
function Check($name, [bool]$ok, $detail) {
    if (-not $ok) { $script:fail++ }
    $line = ($(if ($ok) { 'PASS' } else { 'FAIL' })) + " / $name / $detail"; $results.Add($line); Write-Host $line
}

# ---- 작업 폴더 / 시험용 config ----
$work = Join-Path $env:TEMP 'chippac\ftpbase_probe'
if (Test-Path $work) { Remove-Item $work -Recurse -Force }
$upload = Join-Path $work 'upload'
New-Item -ItemType Directory -Force $upload | Out-Null
Copy-Item (Join-Path $sampleDir 'XD106.02') $upload
Get-ChildItem (Join-Path $sampleDir 'Defect Images') -File | ForEach-Object { Copy-Item $_.FullName $upload }
$klarf = Join-Path $upload 'XD106.02'
$config = Join-Path $work 'probe.exe.config'

function Write-Config([hashtable]$settings) {
    $lines = @('<?xml version="1.0" encoding="utf-8"?>', '<configuration>', '  <appSettings>')
    foreach ($k in $settings.Keys) { $lines += ('    <add key="{0}" value="{1}" />' -f $k, [Security.SecurityElement]::Escape($settings[$k])) }
    $lines += @('  </appSettings>', '</configuration>')
    [IO.File]::WriteAllLines($config, $lines, (New-Object Text.UTF8Encoding($false)))
    [Configuration.ConfigurationManager]::RefreshSection('appSettings')
}

Add-Type -AssemblyName System.Configuration
Write-Config @{}
[AppDomain]::CurrentDomain.SetData('APP_CONFIG_FILE', $config)
$flags = [Reflection.BindingFlags]'NonPublic,Static'
$cm = [Configuration.ConfigurationManager]
$cm.GetField('s_initState', $flags).SetValue($null, 0)
$cm.GetField('s_configSystem', $flags).SetValue($null, $null)

foreach ($d in 'Oracle.ManagedDataAccess', 'Miracom.Middleware', 'DACrux.Base', 'DACrux.Data.Parser', 'DACrux.SEMDMS.DSL') {
    $p = Join-Path $BinDir "$d.dll"; if (-not (Test-Path $p)) { $p = Join-Path $serverBin "$d.dll" }
    if (Test-Path $p) { [void][Reflection.Assembly]::LoadFrom($p) }
}
$ifDll = Join-Path $BinDir 'DACrux.SEMDMS.Interface.dll'
$bslDll = Join-Path $BinDir 'DACrux.SEMDMS.BSL.dll'
[void][Reflection.Assembly]::LoadFrom($ifDll)
[void][Reflection.Assembly]::LoadFrom($bslDll)
Write-Host "BSL: $bslDll"

$helper = @'
using System;
using System.Drawing;
using System.Reflection;
using DACrux.Data.Parser.Klarf;
public static class FtpBaseHelper
{
    static readonly Type T = typeof(DACrux.SEMDMS.BSL.KlarfUpload);
    const BindingFlags F = BindingFlags.NonPublic | BindingFlags.Static;

    /// <summary>경로 계산만. 오류면 "ERR:" + 메시지.</summary>
    public static string Rel(string folder, string ftpBase)
    {
        try { return (string)T.GetMethod("ToFtpRelativePath", F).Invoke(null, new object[] { folder, ftpBase }); }
        catch (TargetInvocationException ex) { return "ERR:" + ex.InnerException.Message; }
    }

    /// <summary>KLARF 를 읽어 저장소에 복사한다(SaveFilesToImageStore, Device = KLARF DeviceID). 결과 = 폴더 | DB 경로 | 저장 이미지 수 | Device, 오류면 "ERR:" + 메시지.</summary>
    public static string Store(string klarfPath)
    {
        ParserKlarf.GetOriginDieIndex = delegate(string setupId, string stepId, DateTime setupTime) { return Point.Empty; };
        ParserKlarf parser = new ParserKlarf(klarfPath);
        string device = (parser.DeviceID ?? "").Trim();
        try
        {
            object store = T.GetMethod("SaveFilesToImageStore", F).Invoke(null, new object[] { parser, klarfPath, device });
            Type st = store.GetType();
            string folder = (string)st.GetField("Folder").GetValue(store);
            string rel = (string)st.GetField("RelativePath").GetValue(store);
            int saved = ((System.Collections.Generic.ICollection<string>)st.GetField("SavedNames").GetValue(store)).Count;
            return folder + "|" + rel + "|" + saved + "|" + device;
        }
        catch (TargetInvocationException ex) { return "ERR:" + ex.InnerException.Message; }
    }

    /// <summary>파일 앞 바이트로 실제 형식(JPEG / PNG / TIFF / 기타).</summary>
    public static string Kind(string path)
    {
        byte[] h = new byte[4];
        using (System.IO.FileStream fs = System.IO.File.OpenRead(path)) fs.Read(h, 0, 4);
        if (h[0] == 0xFF && h[1] == 0xD8) return "JPEG";
        if (h[0] == 0x89 && h[1] == 0x50) return "PNG";
        if ((h[0] == 0x49 && h[1] == 0x49) || (h[0] == 0x4D && h[1] == 0x4D)) return "TIFF";
        return "OTHER";
    }

    /// <summary>이미지 가로 x 세로.</summary>
    public static string Size(string path)
    {
        using (Image img = Image.FromFile(path)) return img.Width + "x" + img.Height;
    }

    /// <summary>프레임 2장(200x100 빨강, 200x100 파랑) TIFF 를 만든다.</summary>
    public static void MakeTwoFrameTiff(string path)
    {
        using (Bitmap a = new Bitmap(200, 100)) using (Bitmap b = new Bitmap(200, 100))
        {
            using (Graphics g = Graphics.FromImage(a)) g.Clear(Color.Red);
            using (Graphics g = Graphics.FromImage(b)) g.Clear(Color.Blue);
            System.Drawing.Imaging.ImageCodecInfo tiff = null;
            foreach (System.Drawing.Imaging.ImageCodecInfo c in System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders())
                if (c.FormatID == System.Drawing.Imaging.ImageFormat.Tiff.Guid) tiff = c;
            System.Drawing.Imaging.EncoderParameters ps = new System.Drawing.Imaging.EncoderParameters(1);
            ps.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)System.Drawing.Imaging.EncoderValue.MultiFrame);
            a.Save(path, tiff, ps);
            ps.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)System.Drawing.Imaging.EncoderValue.FrameDimensionPage);
            a.SaveAdd(b, ps);
            ps.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)System.Drawing.Imaging.EncoderValue.Flush);
            a.SaveAdd(ps);
        }
    }

    /// <summary>SaveFrameImagesAndThumbnails 호출. 결과 = 저장 이름들(',' 구분), 오류면 "ERR:" + 메시지.</summary>
    public static string Frames(string imagePath, int[] seqs, string folder)
    {
        try { return string.Join(",", ((System.Collections.Generic.List<string>)T.GetMethod("SaveFrameImagesAndThumbnails", F).Invoke(null, new object[] { imagePath, seqs, folder })).ToArray()); }
        catch (TargetInvocationException ex) { return "ERR:" + ex.InnerException.Message; }
    }

    /// <summary>SaveThumbnailFromFile 호출. 오류면 "ERR:" + 메시지.</summary>
    public static string Thumb(string imagePath)
    {
        try { T.GetMethod("SaveThumbnailFromFile", F).Invoke(null, new object[] { imagePath }); return "OK"; }
        catch (TargetInvocationException ex) { return "ERR:" + ex.InnerException.Message; }
    }

    /// <summary>화면(DefectMapDraw.fnFTPImageDownload)과 같은 식으로 FTP 요청 경로를 만든다.</summary>
    public static string ScreenPath(string mainPath, string imagePath, string fileName)
    {
        return System.IO.Path.Combine(mainPath, imagePath + "/" + fileName).Replace(@"\", "/");
    }
}
'@
$refs = @($bslDll, $ifDll, 'System.Drawing', 'System.Data', 'System.Xml')
foreach ($d in 'DACrux.Data.Parser', 'DACrux.Base', 'Miracom.Middleware') {
    $p = Join-Path $BinDir "$d.dll"; if (-not (Test-Path $p)) { $p = Join-Path $serverBin "$d.dll" }
    $refs += $p
}
Add-Type -TypeDefinition $helper -ReferencedAssemblies $refs

# ---- 1) 경로 계산 ----
$day = (Get-Date).ToString('yyyyMMdd')
Check 'P1 기준 없음 = 지금 규칙(드라이브 루트 뺌)' ([FtpBaseHelper]::Rel("D:\FTP\SCK\$day\LOT_A", $null) -eq "FTP/SCK/$day/LOT_A") ([FtpBaseHelper]::Rel("D:\FTP\SCK\$day\LOT_A", $null))
Check 'P1b 기준 빈 칸 = 지금 규칙' ([FtpBaseHelper]::Rel("D:\FTP\SCK\$day\LOT_A", '  ') -eq "FTP/SCK/$day/LOT_A") ([FtpBaseHelper]::Rel("D:\FTP\SCK\$day\LOT_A", '  '))
Check 'P2 기준 = DMS_IMAGE 폴더 -> 날짜/Lot' ([FtpBaseHelper]::Rel("C:\FTP\DMS_IMAGE\$day\LOT_A", 'C:\FTP\DMS_IMAGE') -eq "$day/LOT_A") ([FtpBaseHelper]::Rel("C:\FTP\DMS_IMAGE\$day\LOT_A", 'C:\FTP\DMS_IMAGE'))
Check 'P3 기준 끝 \ / 앞뒤 공백' ([FtpBaseHelper]::Rel("C:\FTP\DMS_IMAGE\$day\LOT_A", ' C:\FTP\DMS_IMAGE\ ') -eq "$day/LOT_A") ([FtpBaseHelper]::Rel("C:\FTP\DMS_IMAGE\$day\LOT_A", ' C:\FTP\DMS_IMAGE\ '))
Check 'P4 대소문자 다름' ([FtpBaseHelper]::Rel("C:\FTP\DMS_IMAGE\$day\LOT_A", 'c:\ftp\dms_image') -eq "$day/LOT_A") ([FtpBaseHelper]::Rel("C:\FTP\DMS_IMAGE\$day\LOT_A", 'c:\ftp\dms_image'))
Check 'P5 기준 = FTP 루트(C:\FTP) -> DMS_IMAGE/날짜/Lot' ([FtpBaseHelper]::Rel("C:\FTP\DMS_IMAGE\$day\LOT_A", 'C:\FTP') -eq "DMS_IMAGE/$day/LOT_A") ([FtpBaseHelper]::Rel("C:\FTP\DMS_IMAGE\$day\LOT_A", 'C:\FTP'))
$r6 = [FtpBaseHelper]::Rel("D:\FTP\SCK\$day\LOT_A", 'C:\FTP\DMS_IMAGE')
Check 'P6 저장 폴더가 기준 밖 -> 오류' ($r6.StartsWith('ERR:') -and $r6 -match 'KLARF_IMAGE_FTP_BASE') $r6
$r7 = [FtpBaseHelper]::Rel("C:\FTP\DMS_IMAGE\$day\LOT_A", 'C:\FTP\DMS')
Check 'P7 이름 앞부분만 같은 기준(C:\FTP\DMS) -> 오류' ($r7.StartsWith('ERR:')) $r7
$r8 = [FtpBaseHelper]::Rel("C:\FTP\DMS_IMAGE\$day\LOT_A", 'FTP\DMS_IMAGE')
Check 'P8 기준이 상대 경로 -> 오류' ($r8.StartsWith('ERR:') -and $r8 -match '절대 경로') $r8
$s1 = [FtpBaseHelper]::ScreenPath('/DMS_IMAGE', "$day/LOT_A", 'a.jpg')
$s2 = [FtpBaseHelper]::ScreenPath('/DMS_IMAGE/', "$day/LOT_A", 'a.jpg')
$s3 = [FtpBaseHelper]::ScreenPath('/%2f/', "FTP/SCK/$day/LOT_A", 'a.jpg')
Check 'P9 화면 FTP 경로 = MAIN_PATH + DB 경로 + 파일' ($s1 -eq "/DMS_IMAGE/$day/LOT_A/a.jpg" -and $s2 -eq $s1 -and $s3 -eq "/%2f/FTP/SCK/$day/LOT_A/a.jpg") "$s1 | $s2 | $s3"

# ---- 2) 저장소 복사(SaveFilesToImageStore) ----
$ftpRoot = Join-Path $work 'FTP'
$dmsImage = Join-Path $ftpRoot 'DMS_IMAGE'
Write-Config @{ KLARF_IMAGE_ROOT = $dmsImage; KLARF_IMAGE_FTP_BASE = $dmsImage }
$e1 = [FtpBaseHelper]::Store($klarf)
$p = $e1 -split '\|'
$copied = if ($p.Count -ge 1 -and (Test-Path $p[0])) { @(Get-ChildItem $p[0] -File).Count } else { 0 }
Check 'E1 ROOT = BASE = DMS_IMAGE 폴더: DB 경로 = 날짜/Device/Lot, 이미지 15 + KLARF 복사' (-not $e1.StartsWith('ERR:') -and $p.Count -ge 4 -and $p[3].Length -gt 0 -and $p[1] -eq "$day/$($p[3])/PFIC_RH-HS2615_C4BUMP" -and [int]$p[2] -eq 15 -and $copied -ge 16) "DB 경로 $($p[1]) / 저장 이미지 $($p[2]) / 폴더 파일 $copied"
$screen = [FtpBaseHelper]::ScreenPath('/DMS_IMAGE', $p[1], 'XD106.02_-1_1_0_12_RDLopen_53.jpg')
$local = Join-Path $ftpRoot ($screen.TrimStart('/').Replace('/', '\'))
Check 'E1b 화면 경로(/DMS_IMAGE + DB 경로)가 실제 파일을 가리킴(FTP 루트 = 작업\FTP 로 가정)' (Test-Path $local) "$screen -> $local"

$scck = Join-Path $work 'D_FTP\SCK'
Write-Config @{ KLARF_IMAGE_ROOT = $scck }
$e2 = [FtpBaseHelper]::Store($klarf)
$q = $e2 -split '\|'
$expect2 = (Join-Path $scck "$day\$($q[3])\PFIC_RH-HS2615_C4BUMP"); $expect2 = $expect2.Substring([IO.Path]::GetPathRoot($expect2).Length).Replace('\', '/')
Check 'E2 BASE 없음 = 지금 규칙(드라이브 루트 뺌), 날짜/Device/Lot' (-not $e2.StartsWith('ERR:') -and $q[1] -eq $expect2) "DB 경로 $($q[1])"

$other = Join-Path $work 'OTHER\DMS_IMAGE'
Write-Config @{ KLARF_IMAGE_ROOT = $other; KLARF_IMAGE_FTP_BASE = $dmsImage }
$e3 = [FtpBaseHelper]::Store($klarf)
Check 'E3 ROOT 가 BASE 밖 -> 복사 전에 오류(폴더 안 만듦)' ($e3.StartsWith('ERR:') -and -not (Test-Path $other)) "$e3 / 폴더 있음 $(Test-Path $other)"

# ---- 3) 썸네일 = JPEG / 절반 크기 / 원본보다 작음, 여러 장 TIFF, 읽을 수 없는 이미지 (2026-10-21) ----
$origs = @(Get-ChildItem $p[0] -Filter '*.jpg' | Where-Object { -not $_.Name.StartsWith('thumb_') })
$bad = @()
foreach ($o in $origs) {
    $t = Join-Path $p[0] ('thumb_' + $o.Name)
    if (-not (Test-Path $t)) { $bad += "$($o.Name) 썸네일 없음"; continue }
    $os = [FtpBaseHelper]::Size($o.FullName) -split 'x'
    $ts = [FtpBaseHelper]::Size($t) -split 'x'
    $kind = [FtpBaseHelper]::Kind($t)
    $half = ([int]$ts[0] -eq [Math]::Floor([int]$os[0] / 2)) -and ([int]$ts[1] -eq [Math]::Floor([int]$os[1] / 2))
    if ($kind -ne 'JPEG' -or -not $half -or (Get-Item $t).Length -ge $o.Length) { $bad += "$($o.Name) $kind $($ts -join 'x') $((Get-Item $t).Length)B" }
}
$o1 = $origs | Select-Object -First 1
$t1 = Join-Path $p[0] ('thumb_' + $o1.Name)
Check "T1 썸네일 $($origs.Count)장 = JPEG / 가로세로 절반 / 원본보다 작음" (($origs.Count -eq 15) -and ($bad.Count -eq 0)) "예 $($o1.Name) $((Get-Item $o1.FullName).Length)B $([FtpBaseHelper]::Size($o1.FullName)) -> $((Get-Item $t1).Length)B $([FtpBaseHelper]::Size($t1)) $([FtpBaseHelper]::Kind($t1)) $($bad -join ', ')"

$multi = Join-Path $work 'multi'
New-Item -ItemType Directory -Force $multi | Out-Null
$tif = Join-Path $work 'two.tif'
[FtpBaseHelper]::MakeTwoFrameTiff($tif)
$fr = [FtpBaseHelper]::Frames($tif, [int[]]@(1, 2, 3), $multi)
$f1 = Join-Path $multi 'two_0001.tif'
$f2 = Join-Path $multi 'two_0002.tif'
$ok2 = ($fr -eq 'two_0001.tif,two_0002.tif') -and (Test-Path $f1) -and (Test-Path $f2) -and ([FtpBaseHelper]::Kind($f1) -eq 'TIFF')
$ok2 = $ok2 -and ([FtpBaseHelper]::Kind((Join-Path $multi 'thumb_two_0001.tif')) -eq 'JPEG') -and ([FtpBaseHelper]::Size((Join-Path $multi 'thumb_two_0002.tif')) -eq '100x50')
Check 'T2 여러 장 TIFF: 프레임 1·2 = 이름_0001 / _0002 (원본 형식), 썸네일 JPEG 100x50, 없는 프레임 3 건너뜀' $ok2 "$fr / $(@(Get-ChildItem $multi).Count) 파일"

$broken = Join-Path $work 'broken.jpg'
[IO.File]::WriteAllText($broken, 'not an image')
$tb = [FtpBaseHelper]::Thumb($broken)
$tbf = Join-Path $work 'thumb_broken.jpg'
Check 'T3 읽을 수 없는 이미지 -> 오류 없이 원본을 썸네일로 복사' (($tb -eq 'OK') -and (Test-Path $tbf) -and ((Get-Item $tbf).Length -eq (Get-Item $broken).Length)) "$tb"

Write-Host ''
$total = $results.Count
Write-Host ("KLARF_IMAGE_FTP_BASE_PROBE={0} ({1}/{2})" -f $(if ($script:fail -eq 0) { 'PASS' } else { 'FAIL' }), ($total - $script:fail), $total)
exit $(if ($script:fail -eq 0) { 0 } else { 1 })
