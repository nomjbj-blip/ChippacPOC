using System;
using System.IO;
using System.Text;
using System.Data; 
using System.Text.RegularExpressions;

namespace FDll
{
	/// <summary>
	/// TSKTestInputText에 대한 요약 설명입니다.
	/// </summary>
	internal class TestMapInputText : FInputText
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="regFile">정규식 형식 File Full Path</param>
		/// <param name="inputTextFile">Input File Full Path</param>
		/// <param name="ds">DataSet</param>
		public TestMapInputText(string regFile, string inputTextFile, ref DataSet ds)  : base(regFile, inputTextFile, ref ds)
		{
			m_RegFile = regFile;
			m_InputTextFile = inputTextFile;
			m_Ds = ds;
		}

		/// <summary>
		/// 소멸자
		/// </summary>
		~TestMapInputText()
		{
		}

		#region -------------------------------------------------------------------------------- Act

		/// <summary>
		/// Act
		/// </summary>
		public override void Act()
		{
			ParseInputFile();
		}

		#endregion -------------------------------------------------------------------------------- Act

		#region -------------------------------------------------------------------------------- Process

		/// <summary>
		/// Input File Parse To DataSet
		/// </summary>
		private void ParseInputFile()
		{
			string strData;
			string strReg;

			FileStream fs = null;
			StreamReader sr = null;

			try
			{
				fs= new FileStream(m_RegFile, System.IO.FileMode.Open);
				sr = new StreamReader(fs);

				strReg = sr.ReadToEnd(); 
				strReg = strReg.Replace("\n", "");
				strReg = strReg.Replace("\r", "");

				sr.Close();
				fs.Close(); 
				sr = null;
				fs = null;

				fs = new FileStream(m_InputTextFile, System.IO.FileMode.Open);
				sr = new StreamReader(fs);

				strData = sr.ReadToEnd(); 
				strData = strData.Replace("\r", ""); 
				
				Compare(ref strReg, ref strData);
			}
			catch
			{

			}
			finally
			{
				if(sr != null) sr.Close();
				if(fs != null) fs.Close(); 
				sr = null;
				fs = null;
			}
		}

		/// <summary>
		/// 문자열을 정규식에 맟추어 분석
		/// </summary>
		/// <param name="regstr">지정된 정규식</param>
		/// <param name="textstr">입력된 Text</param>
		private void Compare(ref string regstr, ref string textstr)
		{
			int i;
			int j;
			int k;
			int iXCount;
			int iYCount;
			int iXMin;
			int iYMin;
			int iX;
			int iY;
			string str;

			DataTable dt = null;
			DataRow dr = null;

			CaptureCollection ccDie = null;
			CaptureCollection ccValue = null;
			Regex r = null;
			Match m = null;

			try
			{
				r = new Regex(regstr);
				m = r.Match(textstr);

				SetHeaderValue("TesterID", m.Groups["TesterID"].Value);
				SetHeaderValue("StationNo", m.Groups["StationNo"].Value);
				SetHeaderValue("HandlerProber", m.Groups["HandlerProber"].Value);
				SetHeaderValue("TestProgram", m.Groups["TestProgram"].Value);
				SetHeaderValue("Device", m.Groups["Device"].Value);
				SetHeaderValue("LotNo", m.Groups["LotNo"].Value);
				SetHeaderValue("WaferID", m.Groups["WaferID"].Value);

				SetHeaderValue("Title", m.Groups["Title"].Value);
				SetHeaderValue("MC", m.Groups["MC"].Value);
				SetHeaderValue("ProberCard", m.Groups["ProberCard"].Value);
				SetHeaderValue("Operator", m.Groups["Operator"].Value);

				SetHeaderValue("StartTime", m.Groups["StartTime"].Value);
				SetHeaderValue("EndTime", m.Groups["EndTime"].Value);
				SetHeaderValue("TestDies", m.Groups["TestDies"].Value);
				SetHeaderValue("PassDies", m.Groups["PassDies"].Value);
				SetHeaderValue("Pass", m.Groups["Pass"].Value);
				SetHeaderValue("FailDies", m.Groups["FailDies"].Value);
				SetHeaderValue("Fail", m.Groups["Fail"].Value);
				SetHeaderValue("DieIndexMinX", m.Groups["DieIndexMinX"].Value);
				SetHeaderValue("DieIndexMinY", m.Groups["DieIndexMinY"].Value);
				SetHeaderValue("DieIndexMaxX", m.Groups["DieIndexMaxX"].Value);
				SetHeaderValue("DieIndexMaxY", m.Groups["DieIndexMaxY"].Value);

				SetHeaderValue("DContact", m.Groups["DContact"].Value);

				iXMin = Convert.ToInt32(m.Groups["DieIndexMinX"].Value);
				iYMin = Convert.ToInt32(m.Groups["DieIndexMinY"].Value);
				iXCount = Convert.ToInt32(m.Groups["DieIndexMaxX"].Value) - Convert.ToInt32(m.Groups["DieIndexMinX"].Value) + 1;
				iYCount = Convert.ToInt32(m.Groups["DieIndexMaxY"].Value) - Convert.ToInt32(m.Groups["DieIndexMinY"].Value) + 1;

				dt = m_Ds.Tables["DataData"];
				dt.Rows.Clear();  

				ccDie = m.Groups["Die"].Captures;   
				ccValue = m.Groups["Value"].Captures;   

				j = 0;

				for(i = 0; i <  ccDie.Count; i++)
				{
					dr = dt.NewRow(); 
					str = ccDie[i].ToString();

					if(str != " ")
					{
						j++;
						iX = (int)(i % iXCount) + iXMin;
						iY = (int)(i / iXCount) + iYMin;
						dr["NO"] = j;
						dr["DIEX"] = iX;
						dr["DIEY"] = iY; 
						dr["CHARBIN"] = str;

						for(k = 0; k < ccValue.Count; k++)
						{
							if(ccValue[k].ToString() == str) 
							{
								dr["BIN"] = k;
								break;
							}
						}

						dt.Rows.Add(dr);
					}
				}
			}
			catch
			{
			}
			finally
			{
				r = null;
				m = null;
				ccDie = null;
				ccValue = null;
			}
		}

		#endregion -------------------------------------------------------------------------------- Process
	}
}
