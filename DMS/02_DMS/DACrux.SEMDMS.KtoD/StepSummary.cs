using System;
using System.Text;
using System.IO;
using System.Data;
using DACrux.SEMDMS.DSL;
using System.Runtime.InteropServices;

namespace DACrux.SEMDMS.KtoD
{
	/// <summary>
	/// StepSummary에 대한 요약 설명입니다.
	/// </summary>
	public class StepSummary : Miracom.Middleware.BaseComponent
	{
		public string m_XMLFile = string.Empty;
		public string m_XMLPath = @"L:\EDB_TQD_DATA";
		public string m_XMLFormatFile = @"D:\Miracom\YES\180_Console\EDB_DATA_FROM_TQD_STEP_SUM.xml";

		public StepSummary()
		{
			
		}

		public void RunStepSummary(long step_seq)
		{
			TQD_STEP_SUM oYesStepSum =null;
			TQD_CONFIG oYesConfig = null;
			DataTable  dt = null;
			try
			{
				oYesConfig  = new TQD_CONFIG();
				try
				{
					dt = oYesConfig.Get(new string[] {"INTERFACE","TOEDBXMLPATH"});
					if(dt.Rows.Count>0)
					{
						m_XMLPath = dt.Rows[0]["VALUE"].ToString().Trim();
					}
				}
				catch{}

				oYesStepSum = new TQD_STEP_SUM();
				
				oYesStepSum.Create(new string[1]{step_seq.ToString()});
				Logging.WriteLog("STEP Summary Success.","PROC",0);
			}
			catch(Exception ex)
			{
				Logging.WriteLog(string.Format("STEP Summary Fail. [ERROR:{0}]",ex.Message),"PROC",0);
				throw this.ProcessErr(ex);
			}
			finally
			{
				oYesConfig.Dispose();
				oYesStepSum.Dispose();
				if(dt != null) dt.Dispose(); dt= null;
			}
		}
		
		public void RunStepDCount(long step_seq)
		{
			TQD_DCOUNT oYesDCount = null;
			try
			{
				oYesDCount = new TQD_DCOUNT();
				oYesDCount.Create(new string[1]{step_seq.ToString()});
				Logging.WriteLog("Defect Type Count Success.","PROC",0);

			}
			catch(Exception ex)
			{
				Logging.WriteLog(string.Format("Defect Type Count Fail. [ERROR:{0}]",ex.Message),"PROC",0);
				//ContextUtil.SetAbort();
				throw this.ProcessErr(ex);
			}
			finally
			{
				//ContextUtil.SetComplete();
				oYesDCount.Dispose();
			}
		}

		public void GetConfig()
		{
			TQD_CONFIG oYesConfig = new TQD_CONFIG();
			DataTable  dt = null;
			try
			{
				dt = oYesConfig.GetCategory("EDB");
				for(int i=0;i<dt.Rows.Count;i++)
				{
					if(dt.Rows[i]["NAME"].ToString().Equals("STEPSUM_XML_FORMAT"))
					{
						m_XMLFormatFile = dt.Rows[i]["VALUE"].ToString();
					}

					if(dt.Rows[i]["NAME"].ToString().Equals("STEPSUM_XML_PATH"))
					{
						m_XMLPath = dt.Rows[i]["VALUE"].ToString();
					}
				}
			}
			catch(Exception ex)
			{
				throw this.ProcessErr(ex);
			}
			finally
			{
			}
		}

		public void WriteStepSumXML(long step_seq)
		{
			StreamReader oStrReader = null;
			TQD_STEP_SUM oYesStepSum = null;
			TQD_DCOUNT oYesDCount = null;
			string strDt = string.Empty;
			string strSR = string.Empty;
			StringBuilder sboXML = null;
            DataTable dtYesStepInfo = null;
            DataTable dtYesStepSum = null;
            DataTable dtYesDCount = null;
			StreamWriter oStrWriter = null;

			try
			{
				oYesStepSum	= new TQD_STEP_SUM();
				oYesDCount	= new TQD_DCOUNT();
				oStrReader	= new StreamReader(m_XMLFormatFile);

				strSR = oStrReader.ReadToEnd();
				oStrReader.Close();

				sboXML = new StringBuilder(strSR);

				/// XML에서 Header 부분의 Data를 가져온다.
				/// =================================================================================================================
                dtYesStepInfo = oYesStepSum.GetStepInfo(new string[1] { step_seq.ToString() });
				m_XMLFile = string.Format(@"{0}\{1}",m_XMLPath,dtYesStepInfo.Rows[0]["XMLFILE"].ToString().ToUpper());

                for (int i = 0; i < dtYesStepInfo.Columns.Count; i++)
				{
                    sboXML.Replace(string.Format("_{0}_", dtYesStepInfo.Columns[i].ColumnName)
                        , dtYesStepInfo.Rows[0].ItemArray[i].ToString());
				}
				/// =================================================================================================================
				///


				/// XML에서 STEP_SUM 부분의 Data를 가져온다.
				/// =================================================================================================================
				dtYesStepSum = oYesStepSum.Get(new string[1]{step_seq.ToString()});

                for (int i = 0; i < dtYesStepSum.Columns.Count; i++)
				{
                    sboXML.Replace(string.Format("_{0}_", dtYesStepSum.Columns[i].ColumnName)
                        , dtYesStepSum.Rows[0].ItemArray[i].ToString());
				}
				/// =================================================================================================================
				/// 


				/// XML에서 DCOUNT 부분의 Data를 가져온다.
				/// =================================================================================================================
				dtYesDCount = oYesDCount.Get(new string[1]{step_seq.ToString()});

                for (int i = 0; i < dtYesDCount.Columns.Count; i++)
				{
                    sboXML.Replace(string.Format("_{0}_", dtYesDCount.Columns[i].ColumnName)
                        , dtYesDCount.Rows[0][i].ToString());
				}
				/// =================================================================================================================


				oStrWriter = new StreamWriter(m_XMLFile);

				oStrWriter.Write(sboXML.ToString());
				oStrWriter.Close();
				Logging.WriteLog(string.Format("EDB Send XML File [{0}] Write Success.",m_XMLFile),"PROC",0);
				Logging.WriteLog(string.Format("EDB Send XML File End [{0}]",m_XMLFile),"PROC",0);
			}
			catch(Exception ex)
			{
				Logging.WriteLog(ex.Message,"ERROR",3);
				Logging.WriteLog(string.Format("EDB Send XML File Write Fail [ERROR:{0}].\n\r\t\tData Upload Success, this Error skip.",ex.Message),"PROC",0);
				//throw this.ProcessErr(ex);  //Error 무시되어야 함
			}
			finally
			{
				oYesStepSum.Dispose();
				oYesDCount.Dispose();
				sboXML = null;
			}
		}
	}
}
