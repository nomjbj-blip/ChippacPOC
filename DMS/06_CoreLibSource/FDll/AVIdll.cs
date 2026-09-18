using System;
using System.IO;
using System.Text;
using System.Data; 
using System.Collections;

namespace FDll
{
	/// <summary>
	/// AVIdll
	/// </summary>
	public class AVIdll : Fdll
	{

		/// <summary>
		/// 생성자
		/// </summary>
		public AVIdll()
		{
			InitData();
		}

		/// <summary>
		/// 소멸자
		/// </summary>
		~AVIdll()
		{
		}

		#region -------------------------------------------------------------------------------- Interface Function

		public void InitData()
		{
			SetDataSetXmlFile(sFormatDataPath + "avi.xml", true);
			DataSetToStruct();
		}

		/// <summary>
		/// AVI Text File Read
		/// </summary>
		/// <param name="xmlFilePath">Xml</param>
		/// <param name="regFilePath">정규식</param>
		/// <param name="aviInputFile">Input Text File</param>
		public void ReadTextFile(string inputFile)
		{
			SetDataSetXmlFile(sFormatDataPath + "avi.xml", true);
			InputTxtFile(sFormatDataPath + "avi.rege", inputFile);
			DataSetToStruct();
		}

		#endregion -------------------------------------------------------------------------------- Interface Function

		#region -------------------------------------------------------------------------------- DataConvert

		protected override void DataSetToStruct()
		{
			try
			{
				m_HeaderData.LotNo = GetHeaderValue("LotNo");
				m_HeaderData.Device = GetHeaderValue("Device");
				m_HeaderData.WaferID = GetHeaderValue("WaferID");
				m_HeaderData.ChipSizeX = GetHeaderValueDouble("ChipSizeX") / 1000;
				m_HeaderData.ChipSizeY = GetHeaderValueDouble("ChipSizeY") / 1000; 
				m_HeaderData.TestDies = GetHeaderValueInt("TestDies");
				m_HeaderData.FailDies = GetHeaderValueInt("FailDies"); 
	
				int i;

				i = m_Ds.Tables["DataData"].Rows.Count;
				
				if(m_RowDatas != null) m_RowDatas = null;
				m_RowDatas = new RowData[i];

				i = 0;

				foreach(DataRow dr in m_Ds.Tables["DataData"].Rows)
				{
					 m_RowDatas[i].DieX = Convert.ToInt32(dr["DieX"]);
					 m_RowDatas[i].DieY = Convert.ToInt32(dr["DieY"]);
					 m_RowDatas[i].AviBin = Convert.ToInt32(dr["AviBin"]);
			
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
				SetHeaderValue("LotNo", m_HeaderData.LotNo);
				SetHeaderValue("Device", m_HeaderData.Device);
				SetHeaderValue("WaferID", m_HeaderData.WaferID);
				SetHeaderValue("ChipSizeX", Convert.ToString(m_HeaderData.ChipSizeX * 1000));
				SetHeaderValue("ChipSizeY", Convert.ToString(m_HeaderData.ChipSizeY * 1000));
				SetHeaderValue("TestDies", m_HeaderData.TestDies.ToString());
				SetHeaderValue("FailDies", m_HeaderData.FailDies.ToString());

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
					dr["AviBin"] = m_RowDatas[i].AviBin.ToString();

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
			try
			{
				AVIInputText ait = new AVIInputText(regFilePath, inputFile, ref m_Ds);
				ait.Act(); 
			}
			catch
			{
				return 0;
			}
			return 1;
		}

		#endregion -------------------------------------------------------------------------------- Text File Read, Write

	}
}
