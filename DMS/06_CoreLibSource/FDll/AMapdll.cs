using System;
using System.IO;
using System.Text;
using System.Data; 
using System.Collections;

namespace FDll
{
	/// <summary>
	/// AMapdll에 대한 요약 설명입니다.
	/// </summary>
	public class AMapdll : Fdll
	{
		public AMapdll()
		{
			InitData();
		}

		~AMapdll()
		{
		}

		#region -------------------------------------------------------------------------------- Interface Function

		public void InitData()
		{
			SetDataSetXmlFile(sFormatDataPath + "amap.xml", true);
			DataSetToStruct();
		}

		/// <summary>
		/// TSK Binary File -> DataSet -> Text로 출력
		/// </summary>
		/// <param name="tskBinaryFile">TSK Data File</param>
		/// <param name="outFilePath">Text File</param>
		public void OutTextFile(string outFile)
		{
			StructToDataSet();
			OutTxtFile(sFormatDataPath + "amap.tf", outFile);
		}
        
		#endregion -------------------------------------------------------------------------------- Interface Function

		#region -------------------------------------------------------------------------------- DataConvert

		protected override void DataSetToStruct()
		{
			try
			{

				m_HeaderData.TestProgram = GetHeaderValue("TestProgram");
				m_HeaderData.WaferID = GetHeaderValue("WaferID");
				m_HeaderData.StartTime = GetHeaderValue("StartTime");
				m_HeaderData.TestDies = Convert.ToInt32(GetHeaderValue("TestDies"));
				m_HeaderData.PassDies = Convert.ToInt32(GetHeaderValue("PassDies"));
				m_HeaderData.Pass = Convert.ToDouble(GetHeaderValue("Pass"));
				m_HeaderData.FailDies = Convert.ToInt32(GetHeaderValue("FailDies")); 
				m_HeaderData.Fail = Convert.ToDouble(GetHeaderValue("Fail"));
				m_HeaderData.DieIndexMinX = Convert.ToInt32(GetHeaderValue("DieIndexMinX"));
				m_HeaderData.DieIndexMinY = Convert.ToInt32(GetHeaderValue("DieIndexMinY"));
				m_HeaderData.DieIndexMaxX = Convert.ToInt32(GetHeaderValue("DieIndexMaxX"));
				m_HeaderData.DieIndexMaxY = Convert.ToInt32(GetHeaderValue("DieIndexMaxY"));
				m_HeaderData.DContact = Convert.ToInt32(GetHeaderValue("DContact"));
	
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
					m_RowDatas[i].DieTestResult = Convert.ToInt32(dr["DieTestResult"]);
					m_RowDatas[i].DieAttribute = Convert.ToInt32(dr["DieAttribute"]);
			
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
				SetHeaderValue("TestProgram", m_HeaderData.TestProgram);
				SetHeaderValue("WaferID", m_HeaderData.WaferID);
				SetHeaderValue("StartTime", m_HeaderData.StartTime);
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
					dr["DieTestResult"] = m_RowDatas[i].DieTestResult.ToString();
					dr["DieAttribute"] = m_RowDatas[i].DieAttribute.ToString();

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
			try
			{
				AMapOutText tot = new AMapOutText(formatFilePath, outFilePath, ref m_Ds);
				tot.Act(); 
			}
			catch
			{
				return 0;
			}
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
			return 1;
		}

		#endregion -------------------------------------------------------------------------------- Text File Read, Write
	}
}
