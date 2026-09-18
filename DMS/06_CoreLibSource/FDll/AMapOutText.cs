using System;
using System.IO;
using System.Text;
using System.Data; 
using System.Text.RegularExpressions;

namespace FDll
{
	/// <summary>
	/// AMapOutText에 대한 요약 설명입니다.
	/// </summary>
	internal class AMapOutText : FOutText
	{

		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="formatFile">Format 형식 File Full Path</param>
		/// <param name="OutFile">Out File Full Path</param>
		/// <param name="ds">DataSet</param>
		public AMapOutText(string formatFile, string OutFile, ref DataSet ds) : base(formatFile, OutFile, ref ds)
		{
			m_FormatFile = formatFile;
			m_OutFile = OutFile;
			m_Ds = ds;
		}

		/// <summary>
		/// 소멸자
		/// </summary>
		~AMapOutText()
		{	
		}

		
		#region -------------------------------------------------------------------------------- Act

		/// <summary>
		/// Act
		/// </summary>
		public override void Act()
		{
			File.Copy(m_FormatFile, m_OutFile, true); 

			File.SetAttributes(m_OutFile, System.IO.FileAttributes.Archive);
	
			InsertOutFiles();
		}

        public override DataSet Act1()
        {
            DataSet ds = null;
            return ds;
        }
		#endregion -------------------------------------------------------------------------------- Act


		#region -------------------------------------------------------------------------------- Process

		/// <summary>
		/// 출력 Text File을 만든다.
		/// </summary>
		private void InsertOutFiles()
		{
			string strHead;

			FileStream fs = null;
			StreamReader sr = null;
			StreamWriter sw = null;
			
			try
			{
				fs = new FileStream(m_OutFile, System.IO.FileMode.Open);
				sr = new StreamReader(fs);
			
				strHead = sr.ReadToEnd(); 
			
				strHead = InsertData(strHead);

				sr.Close();
				fs.Close(); 
				sr = null;
				fs = null;

				fs = new FileStream(m_OutFile, System.IO.FileMode.Create);
				sw = new StreamWriter(fs);

				sw.Write(strHead);

				sw.Close();
				fs.Close();
				sw = null;
				fs = null;
			}
			catch
			{
			}
			finally
			{
				if(sr != null) sr.Close();
				if(sw != null) sr.Close();
				if(fs != null) sr.Close();

				sr = null;
				sw = null;
				fs = null;
			}
		}

		/// <summary>
		/// Header Txet를 구성한다
		/// </summary>
		/// <param name="strHead">File에서 읽은 String</param>
		/// <returns>변환된 String</returns>
		private string InsertData(string strHead)
		{
			string str;
			int iXMin;
			int iYMin;
			int iXMax;
			int iYMax;
			int ix;
			int iy;
			int iBin;
			int iResult;
			int iAttribute;
			int i;
			int j;
			int[,] iValue;

			iXMin = Convert.ToInt32(GetHeaderValue("DieIndexMinX"));
			iYMin = Convert.ToInt32(GetHeaderValue("DieIndexMinY"));
			iXMax = Convert.ToInt32(GetHeaderValue("DieIndexMaxX"));
			iYMax = Convert.ToInt32(GetHeaderValue("DieIndexMaxY"));
			
			strHead = strHead.Replace("%TestProgram%", GetHeaderValue("TestProgram"));				// TestProgram
			strHead = strHead.Replace("%WaferID#####%", GetHeaderValue("WaferID").PadRight(14, ' '));	// WaferID 
			strHead = strHead.Replace("%StartTime%", GetHeaderValue("StartTime"));					// StartTime

			strHead = strHead.Replace("%##TestDies%", GetHeaderValue("TestDies").PadLeft(12, ' '));		// TestDies
			strHead = strHead.Replace("%PassDie%", GetHeaderValue("PassDies").PadLeft(9, ' '));		// PassDies
			strHead = strHead.Replace("%###Pass%", GetHeaderValue("Pass").PadLeft(9, ' '));		// Pass
			strHead = strHead.Replace("%FailDie%", GetHeaderValue("FailDies").PadLeft(9, ' '));		// FailDies
			strHead = strHead.Replace("%###Fail%", GetHeaderValue("Fail").PadLeft(9, ' '));		// Fail
			strHead = strHead.Replace("%DieXmin%", iXMin.ToString().PadLeft(9, ' '));		// DieIndexMinX
			strHead = strHead.Replace("%DieYmin%", iYMin.ToString().PadLeft(9, ' '));		// DieIndexMinY
			strHead = strHead.Replace("%DieXmax%", iXMax.ToString().PadLeft(9, ' '));		// DieIndexMaxX
			strHead = strHead.Replace("%DieYmax%", iYMax.ToString().PadLeft(9, ' '));		// DieIndexMaxY
			strHead = strHead.Replace("%#DContact%", GetHeaderValue("DContact").PadLeft(11, ' '));		// DContact

			iValue = new int[iYMax - iYMin + 1, iXMax - iXMin + 1];

			for(i = 0; i <= iYMax - iYMin; i++)
				for(j = 0; j <= iXMax - iXMin; j++) iValue[i, j] = -100;

			foreach(DataRow dr in m_Ds.Tables["DataData"].Rows)
			{
				try
				{
					ix = Convert.ToInt32(dr["DIEX"]);
					iy = Convert.ToInt32(dr["DIEY"]); 
					iBin = Convert.ToInt32(dr["Bin"]);
					iResult = Convert.ToInt32(dr["DieTestResult"]);
					iAttribute = Convert.ToInt32(dr["DieAttribute"]); 

					if(iResult > 0)
						iValue[iy - iYMin, ix - iXMin] = iBin;
				}
				catch
				{
				}
			}

			str = string.Empty;

			for(i = iYMin; i <= iYMax; i++)
			{
				str = str + i.ToString().PadRight(3, ' ');
				
				for(j = iXMin; j <= iXMax; j++)
				{
					switch(iValue[i - iYMin, j - iXMin])
					{
						case -100:
							str = str + " ";
							break;

						case 0:
							str = str + ".";
							break;

						default:
							str = str + "V";
							break;
					}
				}
			
				if(i != iYMax) str = str + "\r\n";
			}

			strHead = strHead.Replace("%LoopDataLoop%", str);

			str = "-";
			str = str.PadLeft(iXMax - iXMin, '-');
			strHead = strHead.Replace("%Loop-Loop%", str);

			return strHead;
		}

		#endregion -------------------------------------------------------------------------------- Process
	}
}
