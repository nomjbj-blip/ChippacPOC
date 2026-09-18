using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;

namespace DACrux.Utility
{
    public class HFtpClient
    {
        public delegate void DelgUpDown(int writeByte);
        public event DelgUpDown OnUpDown;

        string m_user = "eap";
        string m_pw = "eap_hana0";
        string m_ip = "12.230.55.27";
        int m_port = 21;
        string m_dir = "";

        public enum HFtpContentType { FILE, DIRECTORY, SHOT_CUT, UNKNOWN };
        public struct HFtpContent
        {
            public string name;
            public HFtpContentType type;
            public DateTime dateTime;
            public long length;
        }

        public void Dispose()
        {
            Disconnect();
        }

        public void Disconnect()
        {
            m_user = "";
            m_pw = "";
            m_port = -1;
            m_dir = "";
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="ip">IP 주소</param>
        /// <param name="port">Port 번호</param>
        /// <param name="user">User ID</param>
        /// <param name="pw">User Password</param>
        public HFtpClient(string ip, int port, string user, string pw)
        {
            m_ip = ip;
            m_port = port;
            m_user = user;
            m_pw = pw;
        }

        public HFtpClient(string ip, int port, string user, string pw, string DefaultPath)
        {
            m_ip = ip;
            m_port = port;
            m_user = user;
            m_pw = pw;
            m_dir = DefaultPath;
        }

        /// <summary>
        /// Directory와 File을 return 한다
        /// Root Directory가 아닐 경우 .. 도 포함되어 있다
        /// </summary>
        /// <returns>HFtpContent[] Type을 return</returns>
        public HFtpContent[] Dir()
        {
            WebResponse response = null;
            Stream stream = null;
            System.Text.StringBuilder sb = null;
            FtpWebRequest ftp = null;
            Uri uri = null;
            byte[] body = null;
            byte[] dataBody = null;
            ArrayList alContent = null;
            ArrayList alBody = null;
            try
            {

                uri = new Uri(string.Format("ftp://{0}:{1}{2}", m_ip, m_port, m_dir));

                ftp = (FtpWebRequest)WebRequest.Create(uri);
                ftp.Credentials = new NetworkCredential(m_user, m_pw);
                ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                response = ftp.GetResponse();
                stream = response.GetResponseStream();

                int readByte = 0;
                sb = new StringBuilder();
                alBody = new ArrayList();
                body = new byte[4096];
                while (true)
                {
                    readByte = stream.Read(body, 0, body.Length);
                    if (readByte == 0) break;
                    dataBody = new byte[readByte];
                    Array.Copy(body, dataBody, readByte);
                    alBody.AddRange(dataBody);
                    dataBody = null;
                }

                byte[] allBody = (byte[])alBody.ToArray(typeof(byte));
                string[] item = Encoding.Default.GetString(allBody).Replace("\r\n", "\r").Split('\r');
                alContent = new ArrayList();

                HFtpContent ftpContent;
                ftpContent.name = null;
                ftpContent.dateTime = DateTime.MinValue;
                ftpContent.type = HFtpContentType.UNKNOWN;
                ftpContent.length = -1;

                // root 가 아니면 .. 도 추가
                if (m_dir.Length != 0)
                {
                    ftpContent.name = "..";
                    ftpContent.dateTime = DateTime.Now;
                    ftpContent.type = HFtpContentType.DIRECTORY;
                    alContent.Add(ftpContent);
                }

                for (int a = 0; a < item.Length; a++)
                {
                    if (item[a].Trim().Length == 0) continue;

                    ftpContent.name = null;
                    ftpContent.dateTime = DateTime.MinValue;
                    ftpContent.type = HFtpContentType.UNKNOWN;
                    ftpContent.length = -1;

                    if (item[a].IndexOf("<DIR>") > -1)
                    {
                        ftpContent.name = item[a].Substring(39, item[a].Length - 39).Trim();
                        ftpContent.type = HFtpContentType.DIRECTORY;
                        ftpContent.dateTime = DateTime.ParseExact(item[a].Substring(0, 17)
                                                    , "MM-dd-yy  hh:mmtt", System.Globalization.CultureInfo.InvariantCulture);
                        ftpContent.length = -1;
                    }
                    else
                    {
                        ftpContent.name = item[a].Substring(39, item[a].Length - 39).Trim();
                        ftpContent.type = HFtpContentType.FILE;
                        ftpContent.dateTime = DateTime.ParseExact(item[a].Substring(0, 17)
                            , "MM-dd-yy  hh:mmtt", System.Globalization.CultureInfo.InvariantCulture);
                        ftpContent.length = DACrux.Base.Convert.longParse(item[a].Substring(17, 21).Trim());
                    }

                    alContent.Add(ftpContent);
                }

                HFtpContent[] ftpContentList = (HFtpContent[])alContent.ToArray(ftpContent.GetType());
                return ftpContentList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dataBody = null;
                body = null;
                if (uri != null) uri = null;
                if (sb != null) sb.Remove(0, sb.Length);
                if (stream != null)
                {
                    stream.Dispose();
                    stream = null;
                }
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (ftp != null) ftp = null;
                if (alContent != null)
                {
                    alContent.Clear();
                    alContent = null;
                }
                if (alBody != null)
                {
                    alBody.Clear();
                    alBody = null;
                }
            }
        }

        /// <summary>
        /// 현재 directory를 return
        /// </summary>
        /// <returns>string type의 현재 directory을 return</returns>
        public string GetCurrentDir()
        {
            return m_dir;
        }

        /// <summary>
        /// ftp의 현재 directory를 설정한다
        /// </summary>
        /// <param name="directory">설정하려는 directory 이름</param>
        public void SetCurrentDir(string directory)
        {
            if (directory == null) return;
            if (directory.Length == 0) return;
            if (directory.Equals(".")) return;
            if (directory.Equals(".."))
            {
                int idx = m_dir.LastIndexOf('/');
                m_dir = m_dir.Substring(0, idx);
                return;
            }
            if (directory.IndexOf('\\') > -1) throw new Exception(@"Directory name can not contain '\'.");
            directory = directory.Replace('\\', '/');
            if (directory[0] == '/') m_dir = directory;
            else m_dir = m_dir + "/" + directory;
        }

        /// <summary>
        /// 파일을 down한다
        /// </summary>
        /// <param name="remoteFile">다운하고자 하는 파일(FTP상에 있는 파일)</param>
        /// <param name="localFile">Local로 받고자 하는 파일 이름</param>
        public void Down(string remoteFile, string localFile)
        {
            WebResponse response = null;
            Stream stream = null;
            FtpWebRequest ftp = null;
            Uri uri = null;
            byte[] body = null;
            FileStream fs = null;
            try
            {
                if (remoteFile.Length == 0) return;
                if (remoteFile.IndexOf('\\') > -1) throw new Exception(@"Remote file name can not contain '\'.");

                string forDownFile;
                if (remoteFile[0] == '/') forDownFile = remoteFile;
                else
                {
                    if (m_dir.Length == 0) forDownFile = "/" + remoteFile;
                    else forDownFile = m_dir + "/" + remoteFile;
                }
                uri = new Uri(string.Format("ftp://{0}:{1}{2}", m_ip, m_port, forDownFile));

                ftp = (FtpWebRequest)WebRequest.Create(uri);
                ftp.UseBinary = true;
                ftp.Method = WebRequestMethods.Ftp.DownloadFile;
                ftp.Credentials = new NetworkCredential(m_user, m_pw);

                response = ftp.GetResponse();
                stream = response.GetResponseStream();

                int readByte = 0;
                fs = new FileStream(localFile, FileMode.Create);
                body = new byte[4096];
                while (true)
                {
                    readByte = stream.Read(body, 0, body.Length);
                    if (readByte == 0) break;
                    fs.Write(body, 0, readByte);
                    if (OnUpDown != null) OnUpDown(readByte);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Dispose();
                    fs = null;
                }
                body = null;
                if (uri != null) uri = null;
                if (stream != null)
                {
                    stream.Dispose();
                    stream = null;
                }
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (ftp != null) ftp = null;
            }
        }

        /// <summary>
        /// ftp로 파일을 올릴다
        /// </summary>
        /// <param name="localFile">올리고자 하는 파일, 로컬에 있는 파일</param>
        /// <param name="remoteFile">ftp에 저장될 파일 이름</param>
        public void Upload(string localFile, string remoteFile)
        {
            Stream stream = null;
            FtpWebRequest ftp = null;
            Uri uri = null;
            byte[] body = null;
            FileStream fs = null;
            try
            {
                if (remoteFile.Length == 0) return;
                if (remoteFile.IndexOf('\\') > -1) throw new Exception(@"Remote file name can not contain '\'.");

                string forUpFile = null;
                string forUpDir = null;
                if (remoteFile[0] == '/')
                {
                    int idx = remoteFile.LastIndexOf('/');
                    forUpDir = remoteFile.Substring(0, idx);
                    forUpFile = remoteFile.Replace(forUpDir, "");
                }
                else
                {
                    forUpDir = m_dir + "/";
                    forUpFile = remoteFile;
                }
                uri = new Uri(string.Format("ftp://{0}:{1}{2}{3}", m_ip, m_port, forUpDir, forUpFile));

                ftp = (FtpWebRequest)WebRequest.Create(uri);
                ftp.UseBinary = true;
                ftp.Method = WebRequestMethods.Ftp.UploadFile;
                ftp.Credentials = new NetworkCredential(m_user, m_pw);

                stream = ftp.GetRequestStream();

                int readByte = 0;
                fs = new FileStream(localFile, FileMode.Open);
                body = new byte[4096];
                while (true)
                {
                    readByte = fs.Read(body, 0, body.Length);
                    if (readByte == 0) break;
                    stream.Write(body, 0, readByte);
                    if (OnUpDown != null) OnUpDown(readByte);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Dispose();
                    fs = null;
                }
                body = null;
                if (uri != null) uri = null;
                if (stream != null)
                {
                    stream.Dispose();
                    stream = null;
                }
                if (ftp != null) ftp = null;
            }
        }

        /// <summary>
        /// Directory 를 생성한다
        /// </summary>
        /// <param name="dir">ftp에 생성하고자 하는 directory 이름</param>
        public void MakeDir(string dir)
        {
            FtpWebRequest ftp = null;
            Uri uri = null;
            WebResponse response = null;
            try
            {
                if (dir.Length == 0) return;
                if (dir.IndexOf('\\') > -1) throw new Exception(@"Remote file name can not contain '\'.");

                string directory = null;
                if (dir[0] == '/') directory = dir;
                else directory = m_dir + "/" + dir;
                uri = new Uri(string.Format("ftp://{0}:{1}{2}", m_ip, m_port, directory));

                ftp = (FtpWebRequest)WebRequest.Create(uri);
                ftp.Method = WebRequestMethods.Ftp.MakeDirectory;
                ftp.Credentials = new NetworkCredential(m_user, m_pw);

                response = ftp.GetResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (uri != null) uri = null;
                if (ftp != null) ftp = null;
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
            }
        }

        /// <summary>
        /// 파일을 지운다
        /// </summary>
        /// <param name="remoteFile">지우고자하는 파일, ftp 에 있는 파일</param>
        public void DeleteFolder(string remoteFolder)
        {
            FtpWebRequest ftp = null;
            Uri uri = null;
            WebResponse response = null;
            try
            {
                if (remoteFolder.Length == 0) return;
                if (remoteFolder.IndexOf('\\') > -1) throw new Exception(@"Remote folder name can not contain '\'.");

                string deleteFile = null;
                if (remoteFolder[0] == '/') deleteFile = remoteFolder;
                else deleteFile = m_dir + "/" + remoteFolder;
                uri = new Uri(string.Format("ftp://{0}:{1}{2}", m_ip, m_port, deleteFile));

                ftp = (FtpWebRequest)WebRequest.Create(uri);
                ftp.Method = WebRequestMethods.Ftp.RemoveDirectory;
                ftp.Credentials = new NetworkCredential(m_user, m_pw);

                response = ftp.GetResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (uri != null) uri = null;
                if (ftp != null) ftp = null;
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
            }
        }

        /// <summary>
        /// 파일을 지운다
        /// </summary>
        /// <param name="remoteFile">지우고자하는 파일, ftp 에 있는 파일</param>
        public void Delete(string remoteFile)
        {
            FtpWebRequest ftp = null;
            Uri uri = null;
            WebResponse response = null;
            try
            {
                if (remoteFile.Length == 0) return;
                if (remoteFile.IndexOf('\\') > -1) throw new Exception(@"Remote file name can not contain '\'.");

                string deleteFile = null;
                if (remoteFile[0] == '/') deleteFile = remoteFile;
                else deleteFile = m_dir + "/" + remoteFile;
                uri = new Uri(string.Format("ftp://{0}:{1}{2}", m_ip, m_port, deleteFile));

                ftp = (FtpWebRequest)WebRequest.Create(uri);
                ftp.Method = WebRequestMethods.Ftp.DeleteFile;
                ftp.Credentials = new NetworkCredential(m_user, m_pw);

                response = ftp.GetResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (uri != null) uri = null;
                if (ftp != null) ftp = null;
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
            }
        }

        /// <summary>
        /// directory를 지운다
        /// </summary>
        /// <param name="dir">지우고자하는 directory, ftp에 있는 directory</param>
        public void RemoveDir(string dir)
        {
            FtpWebRequest ftp = null;
            Uri uri = null;
            WebResponse response = null;
            try
            {
                if (dir.Length == 0) return;
                if (dir.IndexOf('\\') > -1) throw new Exception(@"Remote file name can not contain '\'.");

                string directory = null;
                if (dir[0] == '/') directory = dir;
                else directory = m_dir + "/" + dir;
                uri = new Uri(string.Format("ftp://{0}:{1}{2}", m_ip, m_port, directory));

                ftp = (FtpWebRequest)WebRequest.Create(uri);
                ftp.Method = WebRequestMethods.Ftp.RemoveDirectory;
                ftp.Credentials = new NetworkCredential(m_user, m_pw);

                response = ftp.GetResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (uri != null) uri = null;
                if (ftp != null) ftp = null;
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
            }
        }

                
        /// <summary>
        /// FTP 접속이 가능한지 테스트한다.
        /// </summary>
        /// <returns>true이면 접속가능, false이면 실패</returns>
        public bool LoginTest()
        {
            FtpWebRequest ftp = null;
            Uri uri = null;

            try
            {
                uri = new Uri(string.Format("ftp://{0}:{1}", m_ip, m_port));
                ftp = (FtpWebRequest)WebRequest.Create(uri);
                ftp.Credentials = new NetworkCredential(m_user, m_pw);
                ftp.KeepAlive = false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (uri != null) uri = null;
                if (ftp != null) ftp = null;
            }
        }

        public void CreateDirectory(string dirpath)
        {
            string[] folders = dirpath.Split(new char[] {'\\', '/'});
            string createFolder = string.Empty;

            for (int i = 0; i < folders.Length; i++)
            {
                createFolder += folders[i];
                bool bResult = FtpCreateDirectory(createFolder);
                createFolder += "/";
            }
        }

        private bool FtpCreateDirectory(string dirpath)
        {
            try
            {
                string uri = string.Empty;
                if (m_port == -1 || m_port == 0)
                    uri = "ftp://" + m_ip + "/" + m_dir + dirpath;
                else
                    uri = "ftp://" + m_ip + ":" + m_port + m_dir + "/" + dirpath;

                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                reqFTP.Credentials = new NetworkCredential(m_user, m_pw);
                reqFTP.KeepAlive = false;
                reqFTP.Method = System.Net.WebRequestMethods.Ftp.MakeDirectory;

                string result = string.Empty;
                using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
                {
                    long size = response.ContentLength;
                    using (Stream datastream = response.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(datastream))
                        {
                            result = sr.ReadToEnd();
                            sr.Close();
                        }
                        datastream.Close();
                    }
                    response.Close();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
