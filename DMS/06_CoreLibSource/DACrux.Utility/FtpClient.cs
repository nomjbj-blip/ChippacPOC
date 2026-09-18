using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Net;

namespace DACrux.Utility
{
	public struct FtpItemInfo
	{
		public bool Folder;
		public bool File;
		public string Name;
	}

	public class FtpClient
	{
		string ftpServerIP;
        string ftpPort;
		string ftpUserID;
		string ftpPassword;

        string ftpBasePath = "FTP_ROOT/MAP/";


        string path;
        bool connected = false;
        int m_timeOut = 5000;

        public bool Connected
        {
            get { return connected; }
        }
        public int TimeOut
        {
            set { m_timeOut = value; }
        }

        public FtpClient()
        {
            ftpServerIP = "127.0.0.1";
            ftpPort = "21";
            ftpUserID = "miracom";
            ftpPassword = "micron";

            path = "ftp://" + ftpServerIP + ":" + ftpPort;
        }

        public FtpClient(string ip, string user, string pass)
        {
            ftpServerIP = ip;
            ftpUserID = user;
            ftpPassword = pass;
            ftpPort = "21";

            path = "ftp://" + ftpServerIP + ":" + ftpPort;
        }

        public FtpClient(string ip, string port, string user, string pass)
        {
            ftpServerIP = ip;
            ftpPort = port;
            ftpUserID = user;
            ftpPassword = pass;

            path = "ftp://" + ftpServerIP + ":" + ftpPort;
        }

        public void Connect(string ip)
        {
            this.ftpServerIP = ip;

            path = "ftp://" + ftpServerIP;
        }

        public void Connect(string ip, string port)
        {
            this.ftpServerIP = ip;
            this.ftpPort = port;

            path = "ftp://" + ftpServerIP + ":" + ftpPort;
        }

        public void ChangeCurrentFolder(string fullPath)
        {
            path = "ftp://" + ftpServerIP + fullPath;
        }

        public string GetCurrentFolder()
        {
            return path.Replace("ftp://" + ftpServerIP, "");
        }

        public void Disconnect()
        {
        }

        public void Login(string user, string pass)
        {
            this.ftpUserID = user;
            this.ftpPassword = pass;

            FtpWebRequest reqFTP = null;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(path));
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.KeepAlive = false;

                this.connected = true;
            }
            catch (Exception)
            {
                this.connected = false;
            }
            finally
            {
                if (reqFTP != null) reqFTP.Abort();
            }
        }

        public void DeleteFTP(string fileName)
        {
            StreamReader sr = null;
            Stream datastream = null;
            FtpWebResponse response = null;
            try
            {
                string uri = string.Empty;

                if (ftpPort == string.Empty)
                    uri = "ftp://" + ftpServerIP + "/" + ftpBasePath + fileName;
                else
                    uri = "ftp://" + ftpServerIP + ":" + ftpPort + "/" + ftpBasePath + fileName;

                //FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + fileName));
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                reqFTP.UsePassive = false;

                string result = String.Empty;
                response = (FtpWebResponse)reqFTP.GetResponse();
                long size = response.ContentLength;
                datastream = response.GetResponseStream();
                sr = new StreamReader(datastream);
                result = sr.ReadToEnd();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sr != null) sr.Close();
                if (datastream != null) datastream.Close();
                if (response != null) response.Close();
            }
        }

        string[] GetFilesDetailList()
        {
            WebResponse response = null;
            StreamReader reader = null;

            try
            {
                StringBuilder result = new StringBuilder();
                FtpWebRequest ftp;
                ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri(this.path));
                ftp.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                response = ftp.GetResponse();
                reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }

                ftp.Abort();

                if (result.Length == 0) return null;

                result.Remove(result.ToString().LastIndexOf("\n"), 1);
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null) reader.Close();
                if (response != null) response.Close();
            }
        }

        public FtpItemInfo[] GetFolderContents()
        {
            string[] file = GetFilesDetailList();
            if (file == null) return null;

            System.Collections.ArrayList al = new System.Collections.ArrayList();
            int idxName = 39;
            for (int a = 0; a < file.Length; a++)
            {
                FtpItemInfo fii;
                if (file[a].IndexOf("<DIR>") != -1)
                {
                    fii.Folder = true;
                    fii.File = false;
                }
                else
                {
                    fii.Folder = false;
                    fii.File = true;
                }

                if (file[a].Length > idxName)
                {
                    fii.Name = file[a].Substring(idxName, file[a].Length - idxName);
                    al.Add(fii);
                }
            }

            return al.ToArray(typeof(FtpItemInfo)) as FtpItemInfo[];
        }

        /*
        public string[] GetFileList()
        {
            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFTP;

            WebResponse response = null;
            StreamReader reader = null;

            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/"));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                response = reqFTP.GetResponse();
                reader = new StreamReader(response.GetResponseStream());
                //MessageBox.Show(reader.ReadToEnd());
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf('\n'), 1);

                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader!=null) reader.Close();
                if (response != null) response.Close();
            }
        }
        */
        public bool FileExists(string fileName)
        {
            string result = null;

            try
            {
                string uri = string.Empty;
                if (ftpPort == string.Empty)
                    uri = "ftp://" + ftpServerIP + "/" + ftpBasePath + fileName;
                else
                    uri = "ftp://" + ftpServerIP + ":" + ftpPort + "/" + ftpBasePath + fileName;

                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = System.Net.WebRequestMethods.Ftp.GetFileSize;

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

                if (result != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                if (ex is System.Net.WebException)
                {
                    if (ex.Message.Contains("550"))
                    {
                        return false;
                    }
                    else
                    {
                        throw ex;
                    }
                }
                else
                {
                    throw ex;
                }
            }
        }

        public void CreateDirectory(string dirpath)
        {
            string[] folders = dirpath.Split('\\');
            string createFolder = string.Empty;

            for (int i = 0; i < folders.Length; i++)
            {
                createFolder += folders[i];
                bool bResult = FtpCreateDirectory(createFolder);
                createFolder += "\\";
            }
        }

        private bool FtpCreateDirectory(string dirpath)
        {
            try
            {
                string uri = string.Empty;
                if (ftpPort == string.Empty)
                    uri = "ftp://" + ftpServerIP + "/" + ftpBasePath + dirpath;
                else
                    uri = "ftp://" + ftpServerIP + ":" + ftpPort + "/" + ftpBasePath + dirpath;

                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
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

        private void Download(string filePath, string fileName)
        {
            FtpWebRequest reqFTP;
            FileStream outputStream = null;
            FtpWebResponse response = null;
            Stream ftpStream = null;
            try
            {
                //filePath = <<The full path where the file is to be created.>>, 
                //fileName = <<Name of the file to be created(Need not be the name of the file on FTP server).>>
                outputStream = new FileStream(filePath + "\\" + fileName, FileMode.Create);

                string uri = string.Empty;
                if (ftpPort == string.Empty)
                    uri = "ftp://" + ftpServerIP + "/" + ftpBasePath + fileName;
                else
                    uri = "ftp://" + ftpServerIP + ":" + ftpPort + "/" + ftpBasePath + fileName;

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.UsePassive = false;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                response = (FtpWebResponse)reqFTP.GetResponse();
                ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (ftpStream != null) ftpStream.Close();
                if (outputStream != null) outputStream.Close();
                if (response != null) response.Close();
            }
        }

        public bool Upload(string remoteFolder, string localfile)
        {
            FileInfo oFile = new FileInfo(localfile);
            return Upload(remoteFolder, oFile);
        }

        public bool Upload(string remoteFolder, FileInfo localfile)
        {
            FtpWebRequest reqFTP;

            try
            {
                //1. check source
                if (!localfile.Exists)
                    return false;

                //2. check target
                string uri = string.Empty;
                if (ftpPort == string.Empty)
                    uri = "ftp://" + ftpServerIP + "/" + ftpBasePath + remoteFolder + "/" + localfile.Name;
                else
                    uri = "ftp://" + ftpServerIP + ":" + ftpPort + "/" + ftpBasePath + remoteFolder + "/" + localfile.Name;

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                reqFTP.UseBinary = false;
                reqFTP.UsePassive = false;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                reqFTP.KeepAlive = false;
                reqFTP.ContentLength = localfile.Length;

                int bufferSize = 2048;
                byte[] buffer = new byte[bufferSize];
                int readCount;

                using (FileStream fs = localfile.OpenRead())
                {
                    using (Stream rs = reqFTP.GetRequestStream())
                    {
                        do
                        {
                            readCount = fs.Read(buffer, 0, bufferSize);
                            rs.Write(buffer, 0, readCount);
                        } while (!(readCount < bufferSize));
                        rs.Close();
                    }
                    fs.Close();
                }

                buffer = null;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void ReceiveFile(string remoteFullFile, string localfile)
        {
            FtpWebRequest reqFTP;
            FileStream outputStream = null;
            FtpWebResponse response = null;
            Stream ftpStream = null;

            try
            {
                //filePath = <<The full path where the file is to be created.>>, 
                //fileName = <<Name of the file to be created(Need not be the name of the file on FTP server).>>
                outputStream = new FileStream(localfile, FileMode.Create);


                string uri = string.Empty;
                if (ftpPort == string.Empty)
                    uri = "ftp://" + ftpServerIP + "/" + remoteFullFile;
                else
                    //uri = "ftp://" + ftpServerIP + ":" + ftpPort + remoteFullFile;
                    //2011-12-21-이태훈- 경로 수정 // ftpPort 다음에  / 있어야 한다.
                    uri = "ftp://" + ftpServerIP + ":" + ftpPort + "/" + remoteFullFile;
				reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
				reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
				reqFTP.UseBinary = true;
				reqFTP.UsePassive = false;
				reqFTP.Timeout = m_timeOut;
				reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
				response = (FtpWebResponse)reqFTP.GetResponse();
				ftpStream = response.GetResponseStream();
				long cl = response.ContentLength;
				int bufferSize = 2048;
				int readCount;
				byte[] buffer = new byte[bufferSize];

				readCount = ftpStream.Read(buffer, 0, bufferSize);
				while (readCount > 0)
				{
					outputStream.Write(buffer, 0, readCount);
					readCount = ftpStream.Read(buffer, 0, bufferSize);
				}
                outputStream.Close();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (ftpStream != null) ftpStream.Close();
				if (outputStream != null) outputStream.Close();
				if (response != null) response.Close();
			}
		}

	}
}
