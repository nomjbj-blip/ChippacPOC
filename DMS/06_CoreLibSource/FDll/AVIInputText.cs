using System;
using System.IO;
using System.Text;
using System.Data; 
using System.Text.RegularExpressions;

namespace FDll
{
	/// <summary>
	/// AVIInputText에 대한 요약 설명입니다.
	/// </summary>
	internal class AVIInputText : FInputText
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="regFile">정규식 형식 File Full Path</param>
		/// <param name="inputTextFile">Input File Full Path</param>
		/// <param name="ds">DataSet</param>
		public AVIInputText(string regFile, string inputTextFile, ref DataSet ds)  : base(regFile, inputTextFile, ref ds)
		{
			m_RegFile = regFile;
			m_InputTextFile = inputTextFile;
			m_Ds = ds;
		}

		/// <summary>
		/// 소멸자
		/// </summary>
		~AVIInputText()
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

			DataTable dt = null;
			DataRow dr = null;

			CaptureCollection ccX = null;
			CaptureCollection ccY = null;
			CaptureCollection ccB = null;
			Regex r = null;
			Match m = null;

			try
			{
				r = new Regex(regstr);
				m = r.Match(textstr);

				SetHeaderValue("LotNo",	m.Groups["LotNo"].Value);
				SetHeaderValue("Device", m.Groups["Device"].Value);
				SetHeaderValue("WaferID", m.Groups["WaferID"].Value);
				SetHeaderValue("ChipSizeX", m.Groups["ChipSizeX"].Value);
				SetHeaderValue("ChipSizeY", m.Groups["ChipSizeY"].Value);
				SetHeaderValue("TestDies", m.Groups["TestDies"].Value);
				SetHeaderValue("FailDies",	m.Groups["FailDies"].Value);

				dt = m_Ds.Tables["DataData"];
				dt.Rows.Clear();  
				ccX = m.Groups["DieX"].Captures;   
				ccY = m.Groups["DieY"].Captures;   
				ccB = m.Groups["AviBin"].Captures;  

				for(i = 0; i <  ccX.Count; i++)
				{
					dr = dt.NewRow(); 
					dr["NO"] = i + 1;
					dr["DIEX"] = ccX[i].Value;
					dr["DIEY"] = ccY[i].Value; 
					dr["AVIBIN"] = ccB[i].Value;
					dt.Rows.Add(dr);
				}
			}
			catch
			{
			}
			finally
			{
				r = null;
				m = null;
				ccX = null;
				ccY = null;
				ccB = null;
			}
		}

		#endregion -------------------------------------------------------------------------------- Process
	}
}
