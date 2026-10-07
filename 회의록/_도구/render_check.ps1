# 회의록 렌더링 점검: Word로 PDF 변환 후 페이지별 PNG 생성
# 사용법: powershell -File render_check.ps1 <회의록.docx> <출력폴더>
param([string]$Docx, [string]$OutDir)
$ErrorActionPreference = 'Stop'
New-Item -ItemType Directory -Force $OutDir | Out-Null
$OutDir = [string](Resolve-Path $OutDir).Path
[object]$pdfPath = [string](Join-Path $OutDir 'check.pdf')

# 1) Word COM 으로 PDF 변환 (읽기 전용으로 열기, 실패해도 Word 종료)
$w = New-Object -ComObject Word.Application; $w.Visible = $false
try {
  $doc = $w.Documents.Open([string]$Docx, $false, $true)
  [object]$fmt = 17
  $doc.SaveAs([ref]$pdfPath, [ref]$fmt)
  $pages = $doc.ComputeStatistics(2)
  $doc.Close([ref]0)
} finally { $w.Quit() }

# 2) Windows 내장 PDF 렌더러로 페이지별 PNG 생성
Add-Type -AssemblyName System.Runtime.WindowsRuntime
$ext = [System.WindowsRuntimeSystemExtensions].GetMethods()
$asTask = ($ext | ? { $_.Name -eq 'AsTask' -and $_.GetParameters().Count -eq 1 -and $_.GetParameters()[0].ParameterType.Name -eq 'IAsyncOperation`1' })[0]
$asAct  = ($ext | ? { $_.Name -eq 'AsTask' -and $_.GetParameters().Count -eq 1 -and $_.GetParameters()[0].ParameterType.Name -eq 'IAsyncAction' })[0]
function Await($op, [Type]$t) { $task = $asTask.MakeGenericMethod($t).Invoke($null, @($op)); $task.Wait(); $task.Result }
[void][Windows.Storage.StorageFile, Windows.Storage, ContentType = WindowsRuntime]
[void][Windows.Data.Pdf.PdfDocument, Windows.Data.Pdf, ContentType = WindowsRuntime]
$f   = Await ([Windows.Storage.StorageFile]::GetFileFromPathAsync($pdfPath)) ([Windows.Storage.StorageFile])
$pdf = Await ([Windows.Data.Pdf.PdfDocument]::LoadFromFileAsync($f)) ([Windows.Data.Pdf.PdfDocument])
$folder = Await ([Windows.Storage.StorageFolder]::GetFolderFromPathAsync($OutDir)) ([Windows.Storage.StorageFolder])
for ($i = 0; $i -lt $pdf.PageCount; $i++) {
  $file = Await ($folder.CreateFileAsync("page$($i + 1).png", [Windows.Storage.CreationCollisionOption]::ReplaceExisting)) ([Windows.Storage.StorageFile])
  $s = Await ($file.OpenAsync([Windows.Storage.FileAccessMode]::ReadWrite)) ([Windows.Storage.Streams.IRandomAccessStream])
  $opt = New-Object Windows.Data.Pdf.PdfPageRenderOptions; $opt.DestinationWidth = 1100
  $asAct.Invoke($null, @($pdf.GetPage($i).RenderToStreamAsync($s, $opt))).Wait(); $s.Dispose()
}
"pages=$pages rendered=$($pdf.PageCount) -> $OutDir"
