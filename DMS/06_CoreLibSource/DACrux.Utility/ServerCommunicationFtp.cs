using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Text;

namespace DACrux.Utility
{
    /* 사용법
     ServerCommunicationFtp comm = new ServerCommunicationFtp();
            comm.Server = "10.148.1.36";// "172.16.15.100";
            comm.Port = 21;
            comm.UserID = "dacrux";
            comm.Password = "dacrux";
            comm.ChmodValue = 777;
     
     comm.ReceiveFile("/TAIHI/KIM/HELLO/WORLD/ftp_test.xlsx", "c:\\ftp_test1.xlsx");
            txtMessage.AppendText(comm.GetResultMessage() + Environment.NewLine);
     
     comm.SendFile("c:\\ftp_test.xlsx", "/TAIHI/KIM/HELLO/WORLD/ftp_test.xlsx");
            txtMessage.AppendText(comm.GetResultMessage() + Environment.NewLine);
     */
    // Socket 통신을 이용한 FTP 구현 2019.08.13 Taihi,Kim.
    // Unix FTP인 경우 chmod 를 이용하여 파일/폴더 권한 설정 가능
    public class ServerCommunicationFtp : ServerCommunication
    {
        public static readonly char DIR_SEPARATOR = '/';
        public static readonly int FILE_BUFFER_LEN = 128 * 1024; // 128kB

        private int _availablePort;
        private IPAddress _localAddress;
        private byte[] _portBytes;

        public ServerCommunicationFtp()
        {
            Port = 21;
            _availablePort = GetAvaliablePort();
            _localAddress = GetValidIPAddress();

            List<byte> list = new List<byte>();
            list.AddRange(_localAddress.GetAddressBytes());
            list.Add((byte)(_availablePort >> 8));
            list.Add((byte)(_availablePort & 0xFF));
            _portBytes = list.ToArray();
        }

        #region 메서드

        public string Normalize(string path)
        {
            if (String.IsNullOrEmpty(path))
                return String.Empty;

            return path.Replace('\\', DIR_SEPARATOR).Trim(DIR_SEPARATOR);
        }

        /// <summary>
        /// 최상위 폴더로 이동합니다.
        /// </summary>
        public void SetTop()
        {
            SendMessage("CWD");
        }

        /// <summary>
        /// 현재 디렉토리를 가져옵니다.
        /// </summary>
        public string GetCurrentDirectory()
        {
            string message = SendMessage("PWD").Message;
            string search = "\"";
            int idx1 = message.IndexOf(search);
            int idx2 = -1;

            if (idx1 >= 0)
                idx2 = message.IndexOf(search, idx1 + search.Length);

            if (idx2 > 0)
                return message.Substring(idx1 + search.Length, idx2 - idx1 - search.Length);

            return message;
        }

        /// <summary>
        /// 현재 디렉토리를 설정합니다.
        /// </summary>
        public void SetCurrentDirectory(string fullPath)
        {
            SendMessage("CWD");
            SendMessage("CWD", fullPath);
        }

        /// <summary>
        /// 현재 디렉토리에서 하위 디렉토리로 이동합니다.
        /// </summary>
        public void MoveDownDirectory(string relativePath)
        {
            SendMessage("CWD", relativePath);
        }

        /// <summary>
        /// 현재 디렉토리에서 상위 디렉토리로 이동합니다.
        /// </summary>
        public void MoveUpDirectory()
        {
            SendMessage("CDUP");
        }

        /// <summary>
        /// 디렉토리 내의 파일/디렉토리 목록을 가져옵니다.
        /// </summary>
        [Obsolete("LIST 지연현상으로 사용 금지", true)]
        private string GetListToString(string serverPath)
        {
            serverPath = Normalize(serverPath);

            if (!String.IsNullOrEmpty(serverPath))
            {
                ReplyMessage reply = SendMessage("CWD", serverPath);

                if (reply.IsFailCode)
                    throw new Exception(String.Format("해당 디렉토리({0})에 접근할 수 없습니다. {1}", serverPath, reply.AllText));
            }

            SendMessage("TYPE", "I");
            SetPort();

            StringBuilder sb = new StringBuilder();

            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Bind(new IPEndPoint(_localAddress, _availablePort));
                socket.Listen(10000);

                SendMessage("LIST"); // NLST
                ReadStream(); // LIST는 Reply를 2번 받아야 하므로 별도로 ReadStream() 한번 더 호출

                using (Socket newSocket = socket.Accept())
                {
                    using (MemoryStream ms = new MemoryStream(FILE_BUFFER_LEN))
                    {
                        byte[] buffer = new byte[FILE_BUFFER_LEN];
                        int read = buffer.Length;

                        while (read > 0)
                        {
                            read = newSocket.Receive(buffer);
                            sb.Append(Encoding.GetString(buffer, 0, read));
                        }
                    }
                }
            }

            SendMessage("TYPE", "A");

            return sb.ToString();
        }
        
        public List<string> GetDirectoryList(string serverPath)
        {
            string url = String.Format("ftp://{0}:{1}/{2}", Server, Port, Normalize(serverPath));

            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            
            if (!String.IsNullOrEmpty(UserID))
                request.Credentials = new NetworkCredential(UserID, Password);

            using (WebResponse response = request.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                List<string> list = new List<string>();

                while (reader.Peek() > 0)
                {
                    string text = reader.ReadLine();

                    if (text[0] == 'd')
                        list.Add(GetFileOrDirectoryName(text));

                    System.Threading.Thread.Sleep(10);
                }

                return list;
            }
        }

        /// <summary>
        /// 파일/디렉토리의 이름을 변경합니다.
        /// </summary>
        public void Rename(string path, string oldName, string newName)
        {
            path = Normalize(path);
            SendMessage("CWD", path);
            SendMessage("RNFR", oldName);
            SendMessage("RNTO", newName);
        }

        /// <summary>
        /// 디렉토리를 생성합니다. 생성 후 위치는 해당 디렉토리로 변경됩니다.
        /// </summary>
        public void CreateDirectory(string serverPath)
        {
            serverPath = Normalize(serverPath);
            string curr = GetCurrentDirectory();

            foreach (string path in serverPath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (SendMessage("MKD", path, true).IsSuccessCode)
                    SetChmod(path);

                SendMessage("CWD", path);
            }

            SetCurrentDirectory(curr);
        }

        /// <summary>
        /// 디렉토리를 삭제합니다.
        /// </summary>
        public void DeleteDirectory(string serverPath)
        {
            SendMessage("RMD", serverPath);
        }

        /// <summary>
        /// 파일을 삭제합니다.
        /// </summary>
        public void DeleteFile(string serverFile)
        {
            SendMessage("DELE", serverFile);
        }

        /// <summary>
        /// 파일을 서버로 전송 합니다. <para/>
        /// CHMOD 값이 설정된 경우 파일이나 폴더가 해당 권한으로 설정됩니다.
        /// </summary>
        public void SendFile(string localFile)
        {
            SendFile(localFile, Path.GetFileName(localFile));
        }

        public void SendFileNew(string localFile, string serverPath)
        {
            string url = String.Format("ftp://{0}:{1}/{2}", Server, Port, Normalize(serverPath));

            FileInfo file = new FileInfo(localFile);

            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.UseBinary = false;
            request.UsePassive = false;
            request.KeepAlive = false;
            request.ContentLength = file.Length;

            if (!String.IsNullOrEmpty(UserID))
                request.Credentials = new NetworkCredential(UserID, Password);

            using (FileStream fs = File.OpenRead(localFile))
            {
                byte[] buffer = new byte[10240];

                using (Stream rs = request.GetRequestStream())
                {
                    int read = 0;

                    while (true)
                    {
                        read = fs.Read(buffer, 0, buffer.Length);

                        if (read <= 0)
                            break;

                        rs.Write(buffer, 0, read);
                    }

                }
            }
        }

        /// <summary>
        /// 파일을 서버로 전송 합니다. <para/>
        /// CHMOD 값이 설정된 경우 파일이나 폴더가 해당 권한으로 설정됩니다.
        /// </summary>
        public void SendFile(string localFile, string serverFile)
        {
            string fileName;
            string serverPath;
            GetPathAndFileName(Normalize(serverFile), out serverPath, out fileName);

            if (!String.IsNullOrEmpty(serverPath))
            {
                ReplyMessage reply = SendMessage("CWD", serverPath);

                if (reply.IsFailCode)
                    throw new Exception(String.Format("해당 디렉토리({0})에 접근할 수 없습니다. {1}", serverPath, reply.AllText));
            }

            SendMessage("TYPE", "I");
            SetPort();

            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Bind(new IPEndPoint(_localAddress, _availablePort));
                socket.Listen(10000);

                SendMessage("STOR", fileName);

                using (Socket newSocket = socket.Accept())
                {
                    newSocket.SendFile(localFile);
                }
            }

            SetChmod(fileName);
            SendMessage("TYPE", "A");
        }

        /// <summary>
        /// 파일을 서버로 부터 받습니다.
        /// </summary>
        public void ReceiveFile(string serverFile, string localFile)
        {
            using (FileStream fs = new FileStream(localFile, FileMode.Create))
            {
                WriteToStream(fs, serverFile);
            }
        }

        /// <summary>
        /// 서버로 부터 파일을 MemoryStream 으로 받습니다.
        /// </summary>
        public MemoryStream ReceiveStream(string serverFile)
        {
            MemoryStream ms = new MemoryStream();
            WriteToStream(ms, serverFile);
            return ms;
        }

        /// <summary>
        /// 파일을 Stream 에 기록합니다.
        /// </summary>
        private void WriteToStream(Stream stream, string serverFile)
        {
            string fileName;
            string serverPath;
            GetPathAndFileName(Normalize(serverFile), out serverPath, out fileName);

            if (!String.IsNullOrEmpty(serverPath))
            {
                ReplyMessage reply = SendMessage("CWD", serverPath);

                if (reply.IsFailCode)
                    throw new Exception(String.Format("해당 디렉토리({0})에 접근할 수 없습니다. {1}", serverPath, reply.AllText));
            }

            SendMessage("TYPE", "I");
            SetPort();

            Socket socket = null;

            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket.Bind(new IPEndPoint(_localAddress, _availablePort));
                socket.Listen(10000);

                SendMessage("RETR", fileName);

                using (Socket newSocket = socket.Accept())
                {
                    byte[] buffer = new byte[FILE_BUFFER_LEN];
                    int read = buffer.Length;

                    while (read > 0)
                    {
                        read = newSocket.Receive(buffer);
                        stream.Write(buffer, 0, read);
                    }
                }
            }
            finally
            {
                SendMessage("TYPE", "A");
            }
        }

        public static string Combine(params string[] pathArr)
        {
            if (pathArr == null || pathArr.Length == 0)
                return null;

            for (int i = 0; i < pathArr.Length; i++)
            {
                pathArr[i] = pathArr[i].Trim('/');
            }

            return String.Join("/", pathArr);
        }

        private void SetPort()
        {
            SendMessage("PORT", String.Format("{0},{1},{2},{3},{4},{5}", _portBytes[0], _portBytes[1], _portBytes[2], _portBytes[3], _portBytes[4], _portBytes[5]));
        }

        private int GetAvaliablePort()
        {
            return 60000;
            int startPort = 1024;

            for (int i = startPort; i < 65536; i++)
            {
                TcpConnectionInformation[] connInfos = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections();

                bool exists = false;

                foreach (var connInfo in connInfos)
                {
                    if (connInfo.LocalEndPoint.Port == i)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    return i;
                }
            }

            return 0;
        }

        private void SetChmod(string path)
        {
            if (ChmodValue > 0)
                ChangeChmod(path, ChmodValue);
        }

        private void GetPathAndFileName(string fullPath, out string path, out string fileName)
        {
            int idx = fullPath.LastIndexOf(DIR_SEPARATOR);

            if (idx < 0)
            {
                path = String.Empty;
                fileName = fullPath;
            }
            else
            {
                path = fullPath.Substring(0, idx);
                fileName = fullPath.Substring(idx + 1);
            }
        }

        private IPAddress GetValidIPAddress()
        {
            foreach (var addr in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (addr.AddressFamily == AddressFamily.InterNetwork)
                    return addr;
            }

            throw new Exception("유효한 IP 주소를 찾을 수 없습니다.");
        }
        
        // LIST 명령어를 통해 가져온 파일/디렉토리 정보를 FileSystemInfo 리스트로 변환
        private List<FileSystemInfo> ToSystemFileInfoList(string ftpDirFileInfoText, bool includeFile, bool includeDir)
        {
            List<FileSystemInfo> list = new List<FileSystemInfo>();

            if (String.IsNullOrEmpty(ftpDirFileInfoText))
                return list;

            string[] arr = ftpDirFileInfoText.Split(Environment.NewLine.ToCharArray());

            foreach (string infoText in arr)
            {
                if (String.IsNullOrEmpty(infoText))
                    continue;

                string name = GetFileOrDirectoryName(infoText);

                if (includeFile && infoText[0] == '-') // file
                    list.Add(new FileInfo(name));
                else if (includeDir && infoText[0] == 'd') // directory
                    list.Add(new DirectoryInfo(name));
            }

            return list;
        }

        private string GetFileOrDirectoryName(string infoText)
        {
            // -rw-r-----   1 110        107            275 Aug 14 21:20 WindowsUpdate.log
            // 콜론 다음 공백 뒤가 파일/폴더명

            int idx = infoText.IndexOf(':');

            if (idx > 0)
                idx = infoText.IndexOf(' ', idx);

            return (idx > 0) ? infoText.Substring(idx + 1) : infoText.Substring(infoText.LastIndexOf(' ') + 1);
        }

        #endregion

        #region 프로퍼티

        /// <summary>
        /// 파일이나 폴더를 생성하는 경우 UNIX FTP의 파일/폴더 권한을 나타냅니다.
        /// 0 인 경우 권한을 변경하지 않습니다.
        /// </summary>
        public short ChmodValue
        {
            get;
            set;
        }

        #endregion
    }
}
