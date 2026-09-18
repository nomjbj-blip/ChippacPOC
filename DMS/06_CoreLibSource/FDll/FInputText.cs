using System;
using System.IO;
using System.Text;
using System.Data; 

namespace FDll
{
	/// <summary>
	/// FInputText에 대한 요약 설명입니다.
	/// </summary>
	public abstract class FInputText
	{
		protected string m_RegFile = null;
		protected string m_InputTextFile = null;
		protected DataSet m_Ds = null;	

		public abstract void Act();

		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="regFile">정규식 형식 File Full Path</param>
		/// <param name="inputTextFile">Input File Full Path</param>
		/// <param name="ds">DataSet</param>
		public FInputText(string regFile, string inputTextFile, ref DataSet ds)
		{	
			m_RegFile = regFile;
			m_InputTextFile = inputTextFile;
			m_Ds = ds;
		}	

		/// <summary>
		/// 소멸자
		/// </summary>
		~FInputText()
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

		public int GetHeaderValueInt(string name)
		{
			try
			{
				DataRow[] dr = m_Ds.Tables["Header"].Select(string.Format("TITLE='{0}'",name));
				return Convert.ToInt32(dr[0]["VALUE"]);	
			}
			catch
			{
				return 0;
			}
		}

		public double GetHeaderValueDouble(string name)
		{
			try
			{
				DataRow[] dr = m_Ds.Tables["Header"].Select(string.Format("TITLE='{0}'",name));
				return Convert.ToDouble(dr[0]["VALUE"]);	
			}
			catch
			{
				return 0;
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
