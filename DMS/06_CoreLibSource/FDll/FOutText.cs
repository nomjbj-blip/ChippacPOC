using System;
using System.IO;
using System.Text;
using System.Data; 

namespace FDll
{
	/// <summary>
	/// FOutText에 대한 요약 설명입니다.
	/// </summary>
	public abstract class FOutText
	{
		protected string m_FormatFile = null;
		protected string m_OutFile = null;
		protected DataSet m_Ds = null;				// Data 정보 Data	

		public abstract void Act();
        public abstract DataSet Act1();

        public FOutText()
        {
        }

		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="formatFile">Format 형식 File Full Path</param>
		/// <param name="OutFile">Out File Full Path</param>
		/// <param name="ds">DataSet</param>
		public FOutText(string formatFile, string OutFile, ref DataSet ds)
		{
			m_FormatFile = formatFile;
			m_OutFile = OutFile;
			m_Ds = ds;
		}

		/// <summary>
		/// 소멸자
		/// </summary>
		~FOutText()
		{
			m_Ds = null;
		}

		#region -------------------------------------------------------------------------------- Process

		protected string GetHeaderValue(string name)
		{
			try
			{

				DataRow[] dr = m_Ds.Tables["Header"].Select(string.Format("TITLE='{0}'",name));
				return Convert.ToString(dr[0]["VALUE"]);
			}
			catch
			{	
				return "";
			}
		}

		protected void SetHeaderValue(string name, string strValue)
		{
			try
			{
				DataRow[] dr = m_Ds.Tables["Header"].Select(string.Format("TITLE='{0}'",name));
				dr[0]["VALUE"] = strValue;
			}
			catch
			{
			}
		}

		#endregion -------------------------------------------------------------------------------- Process
	}
}
