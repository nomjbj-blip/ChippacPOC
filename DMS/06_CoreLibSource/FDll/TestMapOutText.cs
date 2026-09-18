using System;
using System.IO;
using System.Text;
using System.Data; 
using System.Text.RegularExpressions;

namespace FDll
{
	/// <summary>
	/// TestMapOutText에 대한 요약 설명입니다.
	/// </summary>
	public class TestMapOutText : FOutText
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="formatFile"></param>
		/// <param name="OutFile"></param>
		/// <param name="ds"></param>
		public TestMapOutText(string formatFile, string OutFile, ref DataSet ds) : base(formatFile, OutFile, ref ds)
		{
			m_FormatFile = formatFile;
			m_OutFile = OutFile;
			m_Ds = ds;
		}

		/// <summary>
		/// 소멸자
		/// </summary>
		~TestMapOutText()
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
			int i;
			int j;
			int[,] iValue;
			char[] charBins = {'.', '1', '2', '3', '4', '5', '6', '7', '8', '9',
							   'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
			                   'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 
							   'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd',
			                   'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
							   'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
			                   'y', 'z'};

			iXMin = Convert.ToInt32(GetHeaderValue("DieIndexMinX"));
			iYMin = Convert.ToInt32(GetHeaderValue("DieIndexMinY"));
			iXMax = Convert.ToInt32(GetHeaderValue("DieIndexMaxX"));
			iYMax = Convert.ToInt32(GetHeaderValue("DieIndexMaxY"));

			strHead = strHead.Replace("%TesterID%", GetHeaderValue("TesterID"));  // TesterID
			strHead = strHead.Replace("%StationNo%", GetHeaderValue("StationNo"));	// StationNo
			strHead = strHead.Replace("%HandlerProber%", GetHeaderValue("HandlerProber"));	// HandlerProber
			strHead = strHead.Replace("%TestProgram%", GetHeaderValue("TestProgram"));	// TestProgram
			strHead = strHead.Replace("%Device%", GetHeaderValue("Device"));	// Device
			strHead = strHead.Replace("%LotNo%", GetHeaderValue("LotNo"));	// LotNo
			strHead = strHead.Replace("%WaferID%", GetHeaderValue("WaferID"));	// WaferID
			strHead = strHead.Replace("%Title%", GetHeaderValue("Title"));	// Title
			strHead = strHead.Replace("%MC%", GetHeaderValue("MC"));	// MC
			strHead = strHead.Replace("%ProberCard%", GetHeaderValue("ProberCard"));	// ProberCard
			strHead = strHead.Replace("%Operator%", GetHeaderValue("Operator"));	// Operator
			strHead = strHead.Replace("%StartTime%", GetHeaderValue("StartTime"));		// StartTime
			strHead = strHead.Replace("%EndTime%", GetHeaderValue("EndTime"));		// EndTime

			strHead = strHead.Replace("%Test%", GetHeaderValue("TestDies").PadLeft(6, ' '));		// TestDies
			strHead = strHead.Replace("%PassD%", GetHeaderValue("PassDies").PadLeft(7, ' '));		// PassDies
			strHead = strHead.Replace("%####Pass%", GetHeaderValue("Pass").PadLeft(10, ' '));		// Pass
			strHead = strHead.Replace("%FailDi%", GetHeaderValue("FailDies").PadLeft(8, ' '));		// FailDies
			strHead = strHead.Replace("%###Fail%", GetHeaderValue("Fail").PadLeft(9, ' '));		// Fail
			strHead = strHead.Replace("%DieXmi%", iXMin.ToString().PadLeft(8, ' '));		// DieIndexMinX
			strHead = strHead.Replace("%DieY%", iYMin.ToString().PadLeft(6, ' '));		// DieIndexMinY
			strHead = strHead.Replace("%DieX%", iXMax.ToString().PadLeft(6, ' '));		// DieIndexMaxX
			strHead = strHead.Replace("%Diem%", iYMax.ToString().PadLeft(6, ' '));		// DieIndexMaxY
			strHead = strHead.Replace("%###DContac%", GetHeaderValue("DContact").PadLeft(12, ' '));		// DContact

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
					if(Convert.ToString(dr["CharBin"]) == "1" ) iValue[iy - iYMin, ix - iXMin] = iBin;
				}
				catch
				{
				}
			}

			str = string.Empty;

			str = str + "     ";

			j = 0;

			for(i = iXMin; i <= iXMax; i++)
			{
				j++;
				if(i % 10 == 0)
				{
					if(j % 10 == 0)	
						str = str + i.ToString().PadLeft(10, ' ');
					else
						str = str + " ".PadLeft(j, ' ');
					j = 0;
				}	
			}

			str = str + "\r\n     ";

			for(i = iXMin; i <= iXMax; i++)
			{
				if(i % 5 == 0) 
					str = str + "+";
				else
					str = str + "-";
			}

			str = str + "\r\n";

			for(i = iYMin; i <= iYMax; i++)
			{
				
				if(i % 5 == 0) 
					//str = str + "    +";
					str = str + "    |";
				else
					str = str + "    |";
				
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
							if(iValue[i - iYMin, j - iXMin] > 61)
								str = str + "?";
							else
								str = str + charBins[iValue[i - iYMin, j - iXMin]].ToString(); 
							break;
					}
				}
			
				if(i != iYMax) str = str + "\r\n";
			}

			strHead = strHead.Replace("%LoopDataLoop%", str);

			return strHead;
		}

		#endregion -------------------------------------------------------------------------------- Process
	}
}
