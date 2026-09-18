using System;
using System.IO;
using System.Data;
using System.Globalization;
using System.Text;
//using DACrux.Component.Socket;

namespace DACrux.SEMDMS.KtoD
{
	/// <summary>
	/// 작성일 : 글쎄...
	/// 작성자 : 임영신
	/// 내  용 : Log를 남기는 놈
	/// </summary>
	public class Logging
	{
		private static int m_LogLevel = 0;

		private static string m_strProcessFile = string.Empty;
		private static string m_strUpdateHistFile = string.Empty;
		public static bool Opened = false;
		private static string otmLogDttm = string.Empty;
        //private static DACrux.Component.Socket.SocketClient m_scClient = null;

		public Logging()
		{
			/// 생성자 Static인데... 생성자가 필요한가?
		}

		public static void SocketOpen(string host,int socketport)
		{
            //try
            //{
            //    m_scClient = new SocketClient();
            //    m_scClient.Connect(host,socketport);
            //}
            //catch(Exception ex)
            //{
            //    if(m_scClient != null) m_scClient = null;
            //    WriteLog(string.Format("Socket Open Error Message : {0}",ex.Message),"ERROR", 0);
            //}
            //finally
            //{
            //}
		}

		public static void SocketClose()
		{
            //try
            //{
            //    if(m_scClient != null)
            //    {
            //        m_scClient.Disconnect();
            //        m_scClient = null;
            //    }
            //}
            //catch(Exception ex)
            //{
            //    WriteLog(string.Format("Socket Closs Error Message : {0}",ex.Message),"ERROR", 0);
            //}
            //finally
            //{
            //}
		}

		public static void Start(string process_file , string updatehist_file,int loglevel)
		{
			DateTime oDtNow = DateTime.Now;
			DateTime oDtOld = oDtNow.AddDays(-5);
			string strOldProcessFile = string.Empty;
			string strOldUpdateHistFile = string.Empty;

			try
			{
				/// 2004년 12월 06일 Log File로 인해 Full이 났음
				/// 5일전의 Log는 지움 ㅋㅋ!
				/// ============================================================================================
				try 
				{
					strOldProcessFile = string.Format(@"{0}\{1}_{2}{3}",Path.GetDirectoryName(process_file)
						,Path.GetFileNameWithoutExtension(process_file)
						,oDtOld.ToString("yyyy-MM-dd")
						,Path.GetExtension(process_file));

					strOldUpdateHistFile = string.Format(@"{0}\{1}_{2}{3}",Path.GetDirectoryName(updatehist_file)
						,Path.GetFileNameWithoutExtension(updatehist_file)
						,oDtOld.ToString("yyyy-MM-dd")
						,Path.GetExtension(updatehist_file));

					if(File.Exists(strOldProcessFile)) File.Delete(strOldProcessFile);
					if(File.Exists(strOldUpdateHistFile)) File.Delete(strOldUpdateHistFile);
				}
				catch
				{
					/// Old File 지우다가 Error가 발생하지는 않을것 같음 
					/// 하지만 만에 하나 발생시 다른 작업에 영향이 없이 Exception을 무시함
					/// 할일없음
				}
				/// ============================================================================================

				otmLogDttm = oDtNow.ToString("yyyy-MM-dd",DateTimeFormatInfo.InvariantInfo);

				m_strProcessFile	=	string.Format(@"{0}\{1}_{2}{3}",Path.GetDirectoryName(process_file)
					,Path.GetFileNameWithoutExtension(process_file)
					,otmLogDttm
					,Path.GetExtension(process_file));

				m_strUpdateHistFile	=	string.Format(@"{0}\{1}_{2}{3}",Path.GetDirectoryName(updatehist_file)
					,Path.GetFileNameWithoutExtension(updatehist_file)
					,otmLogDttm
					,Path.GetExtension(updatehist_file));
				SetLogLevel(loglevel);
				Opened = true;
				WriteLog(" ",false,"PROC",loglevel);
			}
			catch
			{
				/// Log를 남기다 Error가 발생하지는 않을것 같음 
				/// 하지만 만에 하나 발생시 다른 작업에 영향이 없이 Exception을 무시함
				/// 할일없음
			}
			finally
			{
				/// 얘도 할일없음
			}
		}

		public static void WriteLog(string log,string logType,int loglevel)
		{
			try
			{
				WriteLog(log,true,logType,loglevel);
			}
			catch
			{
				/// Exeption을 무시했으니 절대 여기로 안떨어짐
			}
			finally
			{
				/// 얘도 할일없음			
			}
		}

		public static void WriteLog(string log,bool writeTime,string logType,int loglevel)
		{
			if(loglevel > m_LogLevel && !Opened)
			{
				return;
			}

			DateTime	dtLoggingTime = DateTime.Now;
			string strWorkFile = string.Empty;
			StreamWriter swLog = null;
			try
			{
                //if(m_scClient !=null && m_scClient.ServerOn && logType == "SENDDATA")
                //{
                //    try
                //    {
                //        m_scClient.SendData(log);
                //    }
                //    catch
                //    {
                //    }
                //    return;
                //}

				switch(logType)
				{
					case "PROC":
					case "ERROR":
						strWorkFile = m_strProcessFile;
						break;
					case "UPHIST":
						strWorkFile = m_strUpdateHistFile;
						break;
				}

				// 로그 파일이 비대하게 커지므로 history.log 파일만 로그남김
				// 2005-03-30 수정
//				if(loglevel == 0 && logType == "UPHIST")
//				{
					swLog  = new StreamWriter(strWorkFile,true,System.Text.Encoding.Default  );
					if(writeTime)
					{
						swLog.WriteLine("{0} | [L{1}]-{2}",dtLoggingTime.ToString("G",DateTimeFormatInfo.InvariantInfo),loglevel,log);
						System.Console.WriteLine("{0} | {1}",dtLoggingTime.ToString("G",DateTimeFormatInfo.InvariantInfo),log);
					}
					else
					{
						swLog.WriteLine(" | {0}",log.PadLeft(dtLoggingTime.ToString("G",DateTimeFormatInfo.InvariantInfo).Length));
						System.Console.WriteLine(" | {0}",log.PadLeft(dtLoggingTime.ToString("G",DateTimeFormatInfo.InvariantInfo).Length));
					}
//				}
			}
			catch
			{
			}
			finally
			{
				if(swLog != null) 
				{
					swLog.Close();
					swLog = null;

				}
			}
		}
		public static void End()
		{
            //if(m_scClient != null) m_scClient.Disconnect();
			//WriteLog("###################################################################",false,"PROC",0);
			Opened = false;
		}
		public static void SetLogLevel(int loglevel)
		{
			if(loglevel>5) loglevel = 5;
			m_LogLevel = loglevel;
		}
	}
}
