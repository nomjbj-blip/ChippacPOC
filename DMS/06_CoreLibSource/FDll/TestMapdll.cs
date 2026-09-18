using System;
using System.IO;
using System.Text;
using System.Data; 
using System.Collections;

namespace FDll
{

	/// <summary>
	/// TSKTestdll
	/// </summary>
	public class TestMapdll : Fdll
	{
		/// <summary>
		/// 생성자
		/// </summary>
		public TestMapdll()
		{
			InitData();
		}

		/// <summary>
		/// 소멸자
		/// </summary>
		~TestMapdll()
		{
		}

		#region -------------------------------------------------------------------------------- Interface Function

		public void InitData()
		{
			SetDataSetXmlFile(sFormatDataPath + "testMap.xml", true);
			DataSetToStruct();
		}

		/// <summary>
		/// TSK Test Text File Read
		/// </summary>
		/// <param name="inputFile">Input Text File</param>
		public void ReadTextFile(string inputFile)
		{
			SetDataSetXmlFile(sFormatDataPath + "testMap.xml", true);
			InputTxtFile(sFormatDataPath + "testMap.rege", inputFile);
			DataSetToStruct();
		}

		/// <summary>
		/// DataSet -> Text로 출력
		/// </summary>
		/// <param name="tskBinaryFile">TSK Data File</param>
		/// <param name="outFilePath">Text File</param>
		public void OutTextFile(string outFile)
		{
			StructToDataSet();
			OutTxtFile(sFormatDataPath + "testMap.tf", outFile); 
		}

		#endregion -------------------------------------------------------------------------------- Interface Function
		
		#region -------------------------------------------------------------------------------- DataConvert

		protected override void DataSetToStruct()
		{
			try
			{
				m_HeaderData.TesterID = GetHeaderValue("TesterID");
				m_HeaderData.StationNo = GetHeaderValueInt("StationNo");
				m_HeaderData.HandlerProber = GetHeaderValue("HandlerProber");
				m_HeaderData.TestProgram = GetHeaderValue("TestProgram");
				m_HeaderData.Device = GetHeaderValue("Device");
				m_HeaderData.LotNo = GetHeaderValue("LotNo");
				m_HeaderData.MotherLotNo = GetHeaderValue("LotNo");	// Dummy Data
				m_HeaderData.WaferID = GetHeaderValue("WaferID");

				m_HeaderData.Title = GetHeaderValue("Title");
				m_HeaderData.MC = GetHeaderValueInt("MC");
				m_HeaderData.ProberCard = GetHeaderValue("ProberCard");
				m_HeaderData.Operator = GetHeaderValue("Operator");

				m_HeaderData.StartTime = GetHeaderValue("StartTime");
				m_HeaderData.EndTime = GetHeaderValue("EndTime");
				m_HeaderData.TestDies = GetHeaderValueInt("TestDies");
				m_HeaderData.PassDies =GetHeaderValueInt("PassDies");
				m_HeaderData.Pass = GetHeaderValueDouble("Pass");
				m_HeaderData.FailDies = GetHeaderValueInt("FailDies"); 
				m_HeaderData.Fail = GetHeaderValueDouble("Fail");
				m_HeaderData.DieIndexMinX = GetHeaderValueInt("DieIndexMinX");
				m_HeaderData.DieIndexMinY = GetHeaderValueInt("DieIndexMinY");
				m_HeaderData.DieIndexMaxX = GetHeaderValueInt("DieIndexMaxX");
				m_HeaderData.DieIndexMaxY = GetHeaderValueInt("DieIndexMaxY");

				m_HeaderData.DContact = GetHeaderValueInt("DContact");
	
				int i;

				i = m_Ds.Tables["DataData"].Rows.Count;
				
				if(m_RowDatas != null) m_RowDatas = null;
				m_RowDatas = new RowData[i];

				i = 0;

				foreach(DataRow dr in m_Ds.Tables["DataData"].Rows)
				{
					m_RowDatas[i].DieX = Convert.ToInt32(dr["DieX"]);
					m_RowDatas[i].DieY = Convert.ToInt32(dr["DieY"]);
					m_RowDatas[i].Bin = Convert.ToInt32(dr["Bin"]);
					m_RowDatas[i].CharBin = Convert.ToString(dr["CharBin"]);
			
					i++;
				}
			}
			catch
			{
			}
			finally
			{
			}
		}

        protected override void DataSetToStruct(string LotWafer)
        {
        }

		protected override void StructToDataSet()
		{
			try
			{
				SetHeaderValue("TesterID", m_HeaderData.TesterID);
				SetHeaderValue("StationNo", m_HeaderData.StationNo.ToString());
				SetHeaderValue("HandlerProber", m_HeaderData.HandlerProber);
				SetHeaderValue("TestProgram", m_HeaderData.TestProgram);
				SetHeaderValue("Device", m_HeaderData.Device);
				SetHeaderValue("LotNo", m_HeaderData.LotNo);
				SetHeaderValue("WaferID", m_HeaderData.WaferID);

				SetHeaderValue("Title", m_HeaderData.Title);
				SetHeaderValue("MC", m_HeaderData.MC.ToString());
				SetHeaderValue("ProberCard", m_HeaderData.ProberCard);
				SetHeaderValue("Operator", m_HeaderData.Operator);

				SetHeaderValue("StartTime", m_HeaderData.StartTime);
				SetHeaderValue("EndTime", m_HeaderData.EndTime);
				SetHeaderValue("TestDies", m_HeaderData.TestDies.ToString());
				SetHeaderValue("PassDies", m_HeaderData.PassDies.ToString());
				SetHeaderValue("Pass", m_HeaderData.Pass.ToString());
				SetHeaderValue("FailDies", m_HeaderData.FailDies.ToString());
				SetHeaderValue("Fail", m_HeaderData.Fail.ToString());
				SetHeaderValue("DieIndexMinX", m_HeaderData.DieIndexMinX.ToString());
				SetHeaderValue("DieIndexMinY", m_HeaderData.DieIndexMinY.ToString());
				SetHeaderValue("DieIndexMaxX", m_HeaderData.DieIndexMaxX.ToString());
				SetHeaderValue("DieIndexMaxY", m_HeaderData.DieIndexMaxY.ToString());

				SetHeaderValue("DContact", m_HeaderData.DContact.ToString());

				DataTable dt = m_Ds.Tables["DataData"];
				DataRow dr;
				dt.Rows.Clear();  

				int i;
	
				for(i = 0; i < m_RowDatas.Length; i++) 
				{
					dr = dt.NewRow(); 
				
					dr["NO"] = Convert.ToString(i + 1);
					dr["DieX"] = m_RowDatas[i].DieX.ToString();
					dr["DieY"] = m_RowDatas[i].DieY.ToString();
					dr["Bin"] = m_RowDatas[i].Bin.ToString();
					dr["CharBin"] = m_RowDatas[i].CharBin;
					dt.Rows.Add(dr);  
				}
			}
			catch
			{
			}
			finally
			{
			}
		}

		#endregion -------------------------------------------------------------------------------- DataConvert

		#region -------------------------------------------------------------------------------- Text File Read, Write

		/// <summary>
		/// DataSet으로 Text File을 만든다
		/// </summary>
		/// <param name="formatFilePath">Text 형식 File</param>
		/// <param name="outFilePath">만들어질 파일의 Full Path</param>
		/// <returns>성공 여부</returns>
		protected override int OutTxtFile(string formatFilePath, string outFilePath)
		{
			TestMapOutText tmot = new TestMapOutText(formatFilePath, outFilePath, ref m_Ds);
			tmot.Act(); 
			return 1;
		}

        protected override DataSet OutTxtFile1(string formatFilePath, string outFilePath)
        {
            DataSet ds = null;
            return ds;
        }

		/// <summary>
		/// Text File을 읽는다
		/// </summary>
		/// <param name="regFilePath">정규식 File</param>
		/// <param name="inputFile">읽을 File</param>
		/// <returns>성공 여부</returns>
		protected override int InputTxtFile(string regFilePath, string inputFile)
		{
			TestMapInputText tit = new TestMapInputText(regFilePath, inputFile, ref m_Ds);
			tit.Act();
			return 1;
		}

		#endregion -------------------------------------------------------------------------------- Text File Read, Write
	}
}
