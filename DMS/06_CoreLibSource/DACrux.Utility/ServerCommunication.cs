using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

namespace DACrux.Utility
{
    /*
     * 사용법
        string[] pathArr = new string[]
        {
            "/EQUIP/TEST/PCM/AA001",
            "/EQUIP/TEST/PCM/AA002",
            "/EQUIP/TEST/PCM/AA002/(전산_DM)DM2 client setting방법.xls"
        };

        string result;

        using (ServerCommunication comm = new ServerCommunication())
        {
            comm.Server = "172.16.15.100";
            comm.Port = 21;
            comm.UserID = "dacrux";
            comm.Password = "dacrux";

            result = comm.ChangeChmod(pathArr, 777);
        }
     */
    // Server와 소켓 통신을 통해 명령어를 전송 2019.08.13 Taihi,Kim.
    public class ServerCommunication : IDisposable
    {
        #region 멤버 변수

        public static string ANONYMOUS = "anonymous";
        public static readonly int READ_BUFFER_LEN = 8192;
        byte[] _readBuffer;

        private Socket _socket;
        private StringBuilder _sb;

        #endregion

        #region 생성자

        public ServerCommunication()
        {
            Encoding = Encoding.Default;
            _sb = new StringBuilder();
            _readBuffer = new byte[READ_BUFFER_LEN];
        }

        public ServerCommunication(string server, int port, string userID, string password)
            : this()
        {
            Server = server;
            Port = port;
            UserID = userID;
            Password = password;
        }

        public ServerCommunication(string server, int port)
            : this(server, port, null, null)
        {
        }

        #endregion

        #region 메서드

        public bool TryConnect(out string message)
        {
            try
            {
                GetSocket();
            }
            catch
            {
                message = GetReplyMessage();
                int index = Math.Min(message.LastIndexOf(Environment.NewLine, message.Length - 1), 0);

                if (index > 0)
                    message = message.Substring(index);

                return false;
            }

            message = null;
            return true;
        }

        /// <summary>
        /// chmod 명령을 전송합니다.
        /// </summary>
        /// <param name="pathArray">파일 또는 디렉토리 경로 배열</param>
        /// <param name="number">명령 숫자값</param>
        public void ChangeChmod(string[] pathArray, short number)
        {
            foreach (string path in pathArray)
                ChangeChmod(path, number);
        }

        /// <summary>
        /// chmod 명령을 전송합니다.
        /// </summary>
        /// <param name="pathArray">파일 또는 디렉토리 경로 배열</param>
        /// <param name="number">명령 숫자값</param>
        public ReplyMessage ChangeChmod(string path, short number)
        {
            return SendMessage("SITE", String.Format("chmod {0} {1}", number, path));
        }

        /// <summary>
        /// 명령어를 전송합니다.
        /// </summary>
        public ReplyMessage SendMessage(string cmd)
        {
            return SendMessage(cmd, null, false);
        }

        /// <summary>
        /// 명령어를 전송합니다.
        /// </summary>
        public ReplyMessage SendMessage(string cmd, bool ignoreError)
        {
            return SendMessage(cmd, null, ignoreError);
        }

        /// <summary>
        /// 명령어를 전송합니다.
        /// </summary>
        public ReplyMessage SendMessage(string cmd, string argument)
        {
            return SendMessage(cmd, argument, false);
        }

        /// <summary>
        /// 명령어를 전송hn  nb
        /// ?;합니다.
        /// </summary>
        public ReplyMessage SendMessage(string cmd, string argument, bool ignoreError)
        {
            if (String.IsNullOrEmpty(cmd))
                return ReplyMessage.Empty;

            string cmdString = String.IsNullOrEmpty(argument) ? cmd : String.Format("{0} {1}", cmd.Trim(), argument.Trim());

            byte[] bytes = StringToBytes(cmdString);
            _sb.AppendLine(cmdString);
            GetSocket().Send(bytes, 0, bytes.Length, SocketFlags.None);
            //System.Diagnostics.Debug.WriteLine("*REQUEST: " + cmdString);
            ReplyMessage reply = ReadStream();

            if (!ignoreError && reply.IsFailCode)
                throw new Exception(String.Format("명령({0}) 실패 : {1}", cmdString, reply.AllText));

            return reply;
        }

        public string GetReplyMessage()
        {
            string reply = _sb.ToString();
            _sb.Clear();
            return reply;
        }

        protected ReplyMessage ReadStream()
        {
            int read = READ_BUFFER_LEN;
            StringBuilder sb = new StringBuilder();

            int max = 100;
            int cnt = 0;

            while (GetSocket().Available == 0)
            {
                Thread.Sleep(10);
                //System.Diagnostics.Debug.WriteLine("ReadStream() WAIT : {0} ms", cnt * 100);

                if (cnt++ >= max)
                    return ReplyMessage.Empty;// throw new Exception("명령 결과를 받을 수 없습니다.");
            }

            while (GetSocket().Available > 0)
            {
                read = GetSocket().Receive(_readBuffer, 0, _readBuffer.Length, SocketFlags.None);
                sb.Append(Encoding.GetString(_readBuffer, 0, read));
            }

            string msg = sb.ToString();

            // 이전 메시지 REPLY 가 같이 넘어올 수 있음
            string[] arr = msg.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            //System.Diagnostics.Debug.WriteLine("*RESPONSE: " + msg);
            //System.Diagnostics.Debug.WriteLineIf(arr.Length > 1, String.Format("MESSAGE LENGTH = {0}, CHECK ERROR : ", arr.Length, msg));
            msg = arr[arr.Length - 1];

            _sb.AppendLine(msg);
            return new ReplyMessage(msg);
        }

        private byte[] StringToBytes(string text)
        {
            return Encoding.GetBytes(text + Environment.NewLine);
        }

        private Socket GetSocket()
        {
            if (_socket == null)
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }

            if (!_socket.Connected)
            {
                _socket.Connect(Server, Port);
                ReadStream();

                SendMessage("AUTH", "TLS", true);
                SendMessage("AUTH", "SSL", true);

                if (String.IsNullOrEmpty(UserID) && String.IsNullOrEmpty(Password))
                    UserID = Password = ANONYMOUS;

                if (!String.IsNullOrEmpty(UserID))
                    SendMessage("USER", UserID);

                if (!String.IsNullOrEmpty(Password))
                {
                    ReplyMessage msg = SendMessage("PASS", Password);

                    if (msg.Code != 230)
                        throw new Exception(String.Format("Login failed. ({0}/{1})", UserID, Password));
                }
            }

            return _socket;
        }

        public void Dispose()
        {
            if (_socket != null)
            {
                if (_socket.Connected)
                {
                    SendMessage("QUIT");
                    ReadStream();
                }

                _socket.Dispose();
            }

            if (_sb != null)
                _sb.Clear();

            _socket = null;
            _sb = null;
        }

        #endregion

        #region 프로퍼티

        public string Server
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

        public string UserID
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public Encoding Encoding
        {
            get;
            set;
        } 

        #endregion
    }

    /// <summary>
    /// 응답 메시지
    /// </summary>
    public class ReplyMessage
    {
        public static readonly ReplyMessage Empty;

        private short _code;
        private string _message;
        private string _allText;

        static ReplyMessage()
        {
            Empty = new ReplyMessage(null);
        }

        public ReplyMessage(string allText)
        {
            if (String.IsNullOrEmpty(allText))
                return;

            _allText = allText.Trim();

            int idx = allText.IndexOf(' ');

            if (idx < 0)
                return;
            
            if (!Int16.TryParse(allText.Substring(0, idx), out _code))
                _code = 0;

            _message = allText.Substring(idx + 1);
        }

        public bool IsSuccessCode
        {
            get { return _code < 400; }
        }

        public bool IsFailCode
        {
            get { return !IsSuccessCode; }
        }

        public short Code
        {
            get { return _code; }
        }

        public string Message
        {
            get { return _message; }
        }

        public string AllText
        {
            get { return _allText; }
        }
    }
}
