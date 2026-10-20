using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace SCK.EnvCheck
{
    /// <summary>
    /// 클라이언트 PC 가 DACrux SCK POC 를 쓸 수 있는 상태인지 점검한다.
    /// Setup 이 대신 설치할 수 없는 조건(그래픽 / 네트워크 / Excel)과 설치 결과(파일 / 설정 / 쓰기 권한)를 한 번에 확인해
    /// 화면과 결과 파일(설치점검_결과.txt)에 남긴다. 원격 지원 때는 결과 파일만 받아 보면 된다.
    /// OpenGL 점검은 UI 스레드에서 해야 하므로 GlProbe 가 따로 하고 결과만 받아 순서대로 끼워 넣는다.
    /// </summary>
    internal sealed class EnvChecker
    {
        public const string ReportFileName = "설치점검_결과.txt";
        private const string ConfigFileName = "DACruxConfig.ini";
        private const string FileListName = "파일목록.txt";
        private const string VersionFileName = "버전정보.txt";
        private const int ConnectTimeoutMs = 3000;

        // .NET Framework 4.8 / 4.8.1 의 최소 Release 값 (Microsoft 문서 기준)
        private const int DotNet48Release = 528040;
        private const int DotNet481Release = 533320;

        private readonly string _appDir;

        public EnvChecker(string appDir)
        {
            _appDir = appDir;
        }

        public string AppDir
        {
            get { return _appDir; }
        }

        /// <summary>
        /// ini 의 서버 주소. 표시용이며 점검은 RunAll 에서 다시 읽는다.
        /// </summary>
        public string ReadServerIp()
        {
            var ini = IniFile.Read(Path.Combine(_appDir, ConfigFileName));
            return IniFile.Get(ini, "[LAST OPTION]", "ServerIP", string.Empty);
        }

        /// <summary>
        /// 버전정보.txt 의 "버전 = ..." 줄. 없으면 "알 수 없음".
        /// </summary>
        public string ReadVersionLabel()
        {
            string path = Path.Combine(_appDir, VersionFileName);
            if (!File.Exists(path)) return "알 수 없음";
            foreach (string line in File.ReadAllLines(path, Encoding.UTF8))
            {
                int eq = line.IndexOf('=');
                if (eq > 0 && line.Substring(0, eq).Trim() == "버전") return line.Substring(eq + 1).Trim();
            }
            return "알 수 없음";
        }

        /// <summary>
        /// OpenGL 을 뺀 모든 점검을 실행하고, 미리 받은 OpenGL 결과를 정해진 순서에 끼워 돌려준다.
        /// 순서: 운영체제 / .NET / 설치 폴더 쓰기 / 설치 파일 / 그래픽 / 서버 연결 3개 / FTP / Excel.
        /// 네트워크 점검은 포트별로 동시에 해서 서버가 응답하지 않아도 3초 남짓이면 끝나게 한다.
        /// </summary>
        public List<CheckResult> RunAll(CheckResult glResult)
        {
            var results = new List<CheckResult>();
            results.Add(Guard("운영체제", CheckOs));
            results.Add(Guard(".NET Framework", CheckDotNet));
            results.Add(Guard("설치 폴더 쓰기", CheckWritable));
            results.Add(Guard("설치 파일", CheckFiles));
            results.Add(glResult);
            results.AddRange(CheckNetwork());
            results.Add(Guard("Excel", CheckExcel));
            return results;
        }

        /// <summary>
        /// 결과 파일을 설치 폴더에 쓴다. 쓸 수 없으면 사용자 TEMP 폴더에 쓴다. 실제로 쓴 경로를 돌려준다.
        /// </summary>
        public string WriteReport(IList<CheckResult> results)
        {
            var sb = new StringBuilder();
            sb.AppendLine("DACrux SCK POC 설치 점검 결과");
            sb.AppendLine("점검 시각     : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
            sb.AppendLine("PC 이름       : " + Environment.MachineName);
            sb.AppendLine("설치 폴더     : " + _appDir);
            sb.AppendLine("프로그램 버전 : " + ReadVersionLabel());
            sb.AppendLine("서버 주소     : " + ReadServerIp());
            sb.AppendLine();
            foreach (CheckResult r in results)
            {
                sb.AppendLine(string.Format("[{0}] {1} : {2}", CheckResult.LevelText(r.Level), r.Item, r.Detail));
            }
            sb.AppendLine();
            sb.AppendLine("요약 : " + Summary(results));

            List<CheckResult> withLines = results.Where(r => r.Lines.Count > 0).ToList();
            if (withLines.Count > 0)
            {
                sb.AppendLine();
                sb.AppendLine("== 상세 ==");
                foreach (CheckResult r in withLines)
                {
                    sb.AppendLine("[" + r.Item + "]");
                    foreach (string line in r.Lines) sb.AppendLine("  " + line);
                }
            }

            string path = Path.Combine(_appDir, ReportFileName);
            try
            {
                File.WriteAllText(path, sb.ToString(), new UTF8Encoding(true));
            }
            catch (Exception)
            {
                path = Path.Combine(Path.GetTempPath(), "DACrux_SCK_POC_" + ReportFileName);
                File.WriteAllText(path, sb.ToString(), new UTF8Encoding(true));
            }
            return path;
        }

        /// <summary>
        /// "정상 n / 경고 n / 실패 n / 참고 n" 형식 요약.
        /// </summary>
        public static string Summary(IList<CheckResult> results)
        {
            return string.Format("정상 {0} / 경고 {1} / 실패 {2} / 참고 {3}",
                results.Count(r => r.Level == CheckLevel.Ok),
                results.Count(r => r.Level == CheckLevel.Warning),
                results.Count(r => r.Level == CheckLevel.Fail),
                results.Count(r => r.Level == CheckLevel.Info));
        }

        /// <summary>
        /// 점검 하나가 예외로 끝나도 나머지 점검은 계속하도록 감싼다.
        /// </summary>
        private static CheckResult Guard(string item, Func<CheckResult> check)
        {
            try
            {
                return check();
            }
            catch (Exception ex)
            {
                return new CheckResult(item, CheckLevel.Fail, "점검 중 오류: " + ex.GetBaseException().Message);
            }
        }

        // ---- 개별 점검 -------------------------------------------------------------

        /// <summary>
        /// Windows 이름 / 빌드 / 64비트 여부. Windows 11 도 ProductName 이 "Windows 10" 으로 나오므로 빌드 번호(22000 이상)로 고친다.
        /// 32비트는 큰 GDS 파일에서 메모리가 부족할 수 있어 경고로 둔다.
        /// </summary>
        private static CheckResult CheckOs()
        {
            const string item = "운영체제";
            string product = "Windows";
            string display = string.Empty;
            int build = 0;
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion"))
            {
                if (key != null)
                {
                    product = Convert.ToString(key.GetValue("ProductName", "Windows"));
                    display = Convert.ToString(key.GetValue("DisplayVersion", key.GetValue("ReleaseId", string.Empty)));
                    int.TryParse(Convert.ToString(key.GetValue("CurrentBuild", "0")), out build);
                }
            }
            if (build >= 22000) product = product.Replace("Windows 10", "Windows 11");

            bool is64 = Environment.Is64BitOperatingSystem;
            string text = string.Format("{0} {1} (빌드 {2}) / {3}", product, display, build, is64 ? "64비트" : "32비트").Replace("  ", " ");

            if (build > 0 && build < 10240) return new CheckResult(item, CheckLevel.Warning, text + " - Windows 10 이상 권장");
            if (!is64) return new CheckResult(item, CheckLevel.Warning, text + " - 큰 GDS 파일에서 메모리가 부족할 수 있음");
            return new CheckResult(item, CheckLevel.Ok, text);
        }

        /// <summary>
        /// .NET Framework 4.8 이상인지. GDS 화면(NexplantQMS.GdsMap)이 4.8 을 쓴다.
        /// </summary>
        private static CheckResult CheckDotNet()
        {
            const string item = ".NET Framework";
            int release = 0;
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            using (RegistryKey key = baseKey.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full"))
            {
                if (key != null) release = Convert.ToInt32(key.GetValue("Release", 0));
            }

            if (release >= DotNet481Release) return new CheckResult(item, CheckLevel.Ok, "4.8.1 이상 (Release " + release + ")");
            if (release >= DotNet48Release) return new CheckResult(item, CheckLevel.Ok, "4.8 (Release " + release + ")");
            return new CheckResult(item, CheckLevel.Fail, "4.8 미만 (Release " + release + ") - Setup 을 다시 실행하면 함께 설치됩니다");
        }

        /// <summary>
        /// 설치 폴더에 파일을 쓸 수 있는지. DACrux 는 로그인할 때마다 DACruxConfig.ini 를 실행 폴더에 다시 쓰므로
        /// 쓰기가 막히면 로그인 창을 넘어가지 못한다.
        /// </summary>
        private CheckResult CheckWritable()
        {
            const string item = "설치 폴더 쓰기";
            string probe = Path.Combine(_appDir, "~envcheck_write.tmp");
            try
            {
                File.WriteAllText(probe, "check");
                File.Delete(probe);
                return new CheckResult(item, CheckLevel.Ok, "쓰기 가능");
            }
            catch (Exception ex)
            {
                return new CheckResult(item, CheckLevel.Fail, "쓸 수 없음 - 로그인할 때 설정 파일을 저장하지 못해 로그인이 되지 않습니다 (" + ex.GetBaseException().Message + ")");
            }
        }

        /// <summary>
        /// 파일목록.txt(상대 경로 / 크기 / SHA-256)와 실제 파일을 비교한다.
        /// .ini 는 DACrux 가 로그인 때 다시 쓰므로 있는지만 본다.
        /// </summary>
        private CheckResult CheckFiles()
        {
            const string item = "설치 파일";
            string listPath = Path.Combine(_appDir, FileListName);
            if (!File.Exists(listPath)) return new CheckResult(item, CheckLevel.Warning, FileListName + " 파일이 없어 비교하지 못함");

            var missing = new List<string>();
            var changed = new List<string>();
            int total = 0;
            using (SHA256 sha = SHA256.Create())
            {
                foreach (string line in File.ReadAllLines(listPath, Encoding.UTF8))
                {
                    if (line.Length == 0 || line.StartsWith("#")) continue;
                    string[] cols = line.Split('\t');
                    if (cols.Length < 3) continue;

                    total++;
                    string rel = cols[0];
                    string full = Path.Combine(_appDir, rel);
                    if (!File.Exists(full))
                    {
                        missing.Add(rel);
                        continue;
                    }
                    if (rel.EndsWith(".ini", StringComparison.OrdinalIgnoreCase)) continue;

                    long size;
                    long.TryParse(cols[1], out size);
                    var info = new FileInfo(full);
                    if (info.Length != size || !string.Equals(HashFile(sha, full), cols[2], StringComparison.OrdinalIgnoreCase))
                    {
                        changed.Add(rel);
                    }
                }
            }

            if (missing.Count == 0 && changed.Count == 0)
            {
                return new CheckResult(item, CheckLevel.Ok, total + "개 일치");
            }

            var parts = new List<string>();
            if (missing.Count > 0) parts.Add("누락 " + missing.Count + "개: " + ShortList(missing));
            if (changed.Count > 0) parts.Add("변경 " + changed.Count + "개: " + ShortList(changed));
            var result = new CheckResult(item, CheckLevel.Fail, string.Join(" / ", parts) + " - Setup 을 다시 실행하세요");
            result.Lines.AddRange(missing.Select(m => "누락: " + m));
            result.Lines.AddRange(changed.Select(c => "변경: " + c));
            return result;
        }

        /// <summary>
        /// ini 의 ServerIP 와 [APPLICATION UNIT] 포트로 서버 서비스 3개와 FTP 에 TCP 연결해 본다.
        /// 클라이언트는 나가는 연결만 쓰므로 PC 자체 방화벽 규칙은 필요 없다. 실패하면 서버 실행 여부 / 사내망 방화벽을 본다.
        /// </summary>
        private List<CheckResult> CheckNetwork()
        {
            var ini = IniFile.Read(Path.Combine(_appDir, ConfigFileName));
            string server = IniFile.Get(ini, "[LAST OPTION]", "ServerIP", string.Empty);

            var targets = new[]
            {
                new { Item = "서버 연결 - 공통 / 로그인", Key = "MIRACOM_QMS_COMMON", Port = 7510 },
                new { Item = "서버 연결 - TEST", Key = "MIRACOM_DACRUX_PRB", Port = 7513 },
                new { Item = "서버 연결 - DMS", Key = "MIRACOM_DACRUX_DMS", Port = 7515 }
            };

            var results = new List<CheckResult>();
            if (server.Length == 0)
            {
                foreach (var t in targets) results.Add(new CheckResult(t.Item, CheckLevel.Fail, ConfigFileName + " 에 서버 주소가 없음 - Setup 을 다시 실행해 서버 주소를 입력하세요"));
                results.Add(new CheckResult("Defect 이미지 FTP", CheckLevel.Info, "서버 주소가 없어 확인하지 않음"));
                return results;
            }

            bool isLocal = server.Equals("LOCALHOST", StringComparison.OrdinalIgnoreCase) || server == "127.0.0.1";
            var tasks = new List<Task<string>>();
            var ports = new List<int>();
            foreach (var t in targets)
            {
                int port;
                if (!int.TryParse(IniFile.Get(ini, "[APPLICATION UNIT]", t.Key, t.Port.ToString(CultureInfo.InvariantCulture)), out port)) port = t.Port;
                ports.Add(port);
                int p = port;
                tasks.Add(Task.Run(() => TryConnect(server, p)));
            }
            Task<string> ftpTask = Task.Run(() => TryConnect(server, 21));
            Task.WaitAll(tasks.Concat(new[] { ftpTask }).ToArray());

            for (int i = 0; i < targets.Length; i++)
            {
                string where = server + ":" + ports[i];
                string error = tasks[i].Result;
                if (error == null)
                {
                    results.Add(new CheckResult(targets[i].Item, isLocal ? CheckLevel.Warning : CheckLevel.Ok,
                        where + " 연결됨" + (isLocal ? " - 서버 주소가 이 PC(LOCALHOST)입니다. 고객 서버를 쓰려면 Setup 을 다시 실행하세요" : string.Empty)));
                }
                else
                {
                    results.Add(new CheckResult(targets[i].Item, CheckLevel.Fail, where + " " + error));
                }
            }

            string ftpError = ftpTask.Result;
            results.Add(ftpError == null
                ? new CheckResult("Defect 이미지 FTP", CheckLevel.Ok, server + ":21 연결됨")
                : new CheckResult("Defect 이미지 FTP", CheckLevel.Info, server + ":21 " + ftpError + " - Defect 이미지 서버가 다른 주소일 수 있음. 로그인 후 이미지가 안 보이면 FTP 21 확인"));
            return results;
        }

        /// <summary>
        /// Excel 설치 여부. 없으면 일부 Download 기능만 못 쓰므로 실패가 아닌 참고로 둔다.
        /// </summary>
        private static CheckResult CheckExcel()
        {
            const string item = "Excel";
            using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"Excel.Application\CLSID"))
            {
                if (key != null) return new CheckResult(item, CheckLevel.Ok, "설치됨");
            }
            return new CheckResult(item, CheckLevel.Info, "설치 안 됨 - DM 검색 Download / Yield Report / Map View 의 Excel 저장만 쓸 수 없음 (E-Test 업로드와 대부분의 보고서는 영향 없음)");
        }

        // ---- 보조 ------------------------------------------------------------------

        /// <summary>
        /// TCP 연결을 시도한다. 성공하면 null, 실패하면 사람이 읽을 이유를 돌려준다.
        /// </summary>
        private static string TryConnect(string host, int port)
        {
            using (var client = new TcpClient())
            {
                try
                {
                    IAsyncResult ar = client.BeginConnect(host, port, null, null);
                    if (!ar.AsyncWaitHandle.WaitOne(ConnectTimeoutMs))
                    {
                        return "응답 없음(" + (ConnectTimeoutMs / 1000) + "초) - 사내망 방화벽 또는 서버 주소 확인";
                    }
                    client.EndConnect(ar);
                    return null;
                }
                catch (SocketException ex)
                {
                    switch (ex.SocketErrorCode)
                    {
                        case SocketError.ConnectionRefused: return "연결 거부 - 서버 서비스가 실행 중인지 확인";
                        case SocketError.HostNotFound:
                        case SocketError.NoData: return "서버 이름을 찾을 수 없음 - 서버 주소 확인";
                        case SocketError.TimedOut: return "응답 없음 - 사내망 방화벽 또는 서버 주소 확인";
                        default: return "연결 실패(" + ex.SocketErrorCode + ")";
                    }
                }
                catch (Exception ex)
                {
                    return "연결 실패(" + ex.GetBaseException().Message + ")";
                }
            }
        }

        private static string HashFile(SHA256 sha, string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                byte[] hash = sha.ComputeHash(fs);
                return BitConverter.ToString(hash).Replace("-", string.Empty);
            }
        }

        private static string ShortList(List<string> names)
        {
            const int max = 3;
            string head = string.Join(", ", names.Take(max));
            return names.Count > max ? head + " 외 " + (names.Count - max) + "개" : head;
        }
    }
}
