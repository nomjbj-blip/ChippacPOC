using System;
using System.IO;
using System.Text;
using System.Data; 
using System.Collections;
using System.Windows.Forms;
using System.Configuration;

namespace FDll
{
	/// <summary>
	/// Fdll
	/// </summary>
	public abstract class Fdll
	{
		protected DataSet m_Ds = null;				// Data 정보 Data	

		protected HeaderData m_HeaderData;
		protected RowData[] m_RowDatas = null;

		protected abstract void DataSetToStruct();
        protected abstract void DataSetToStruct(string LotWafer);
		protected abstract void StructToDataSet();

		protected abstract int OutTxtFile(string formatFilePath, string outFilePath);
        protected abstract DataSet OutTxtFile1(string formatFilePath, string outFilePath);
		protected abstract int InputTxtFile(string regFilePath, string inputFile);

		static public string sFormatDataPath;

        private DataSet GMS_DataSet = null;

		/// <summary>
		/// 생성자
		/// </summary>
		public Fdll()
		{
			m_Ds = new DataSet();
            sFormatDataPath = Application.StartupPath + @"\FormatData\";
			m_HeaderData = new HeaderData(); 
		}

		/// <summary>
		/// 소멸자
		/// </summary>
		~Fdll()
		{
			if(m_Ds != null) m_Ds.Dispose(); 
			m_Ds = null;
		}

		#region -------------------------------------------------------------------------------- Interface Value

		public DataSet Data
		{
			get
			{
				return m_Ds; 
			}
		}

		public HeaderData HeaderData
		{
			get
			{
				DataSetToStruct();
				return m_HeaderData;
			}
			set
			{
				m_HeaderData = value;
				StructToDataSet();
			}
		}

		public RowData[] RowDatas
		{
			get
			{
				DataSetToStruct();
				return m_RowDatas;
			}
			set
			{
				m_RowDatas = value;
				StructToDataSet();
			}
		}

		public RowData GetRowData(int index)
		{
			try
			{
				return m_RowDatas[index];
			}
			catch
			{	
				return new RowData();
			}
		}

		public void SetMapRowData(int index, RowData rowData)
		{
			try
			{
				m_RowDatas[index] = rowData;
			}
			catch
			{	
			}
		}

        

		public int RowDataCount
		{
			get
			{
				if(m_RowDatas != null) return 0;
				return m_RowDatas.Length;
			}
		}

		public string Operator		
		{
			get{return m_HeaderData.Operator;}
			set{m_HeaderData.Operator = value;}
		}

		public string Device
		{
			get{return m_HeaderData.Device;}
			set{m_HeaderData.Device = value;}
		}

		public string TestProgram
		{
			get{return m_HeaderData.TestProgram;}
			set{m_HeaderData.TestProgram = value;}
		}

		public string TesterID
		{
			get{return m_HeaderData.TesterID;}
			set{m_HeaderData.TesterID = value;}
		}

		public int StationNo
		{
			get{return m_HeaderData.StationNo;}
			set{m_HeaderData.StationNo = value;}
		}

		public string HandlerProber
		{
			get{return m_HeaderData.HandlerProber;}
			set{m_HeaderData.HandlerProber = value;}
		}

		public string ProberCard
		{
			get{return m_HeaderData.ProberCard;}
			set{m_HeaderData.ProberCard = value;}
		}

		public int CassetteNo
		{
			get{return m_HeaderData.CassetteNo;}
			set{m_HeaderData.CassetteNo = value;}
		}

		public string LotNo
		{
			get{return m_HeaderData.LotNo;}
			set{m_HeaderData.LotNo = value;}
		}

		public string MotherLotNo
		{
			get{return m_HeaderData.MotherLotNo;}
			set{m_HeaderData.MotherLotNo = value;}
		}
	
		public int SlotNo
		{
			get{return m_HeaderData.SlotNo;}
			set{m_HeaderData.SlotNo = value;}
		}

		public string WaferID
		{
			get{return m_HeaderData.WaferID;}
			set{m_HeaderData.WaferID = value;}
		}

		public string StartTime
		{
			get{return m_HeaderData.StartTime;}
			set{m_HeaderData.StartTime = value;}
		}

		public string EndTime
		{
			get{return m_HeaderData.EndTime;}
			set{m_HeaderData.EndTime = value;}
		}

		public double WaferSize
		{
			get{return m_HeaderData.WaferSize;}
			set{m_HeaderData.WaferSize = value;}
		}

		public int Angle
		{
			get{return m_HeaderData.Angle;}
			set{m_HeaderData.Angle = value;}
		}

		public int NotchType
		{
			get{return m_HeaderData.NotchType;}
			set{m_HeaderData.NotchType = value;}
		}

		public double EdgeSize
		{
			get{return m_HeaderData.EdgeSize;}
			set{m_HeaderData.EdgeSize = value;}
		}

		public int TestDies
		{
			get{return m_HeaderData.TestDies;}
			set{m_HeaderData.TestDies = value;}
		}

		public int PassDies
		{
			get{return m_HeaderData.PassDies;}
			set{m_HeaderData.PassDies = value;}
		}

		public int FailDies
		{
			get{return m_HeaderData.FailDies;}
			set{m_HeaderData.FailDies = value;}
		}

		public double Pass
		{
			get{return m_HeaderData.Pass;}
			set{m_HeaderData.Pass = value;}
		}

		public double Fail
		{
			get{return m_HeaderData.Fail;}
			set{m_HeaderData.Fail = value;}
		}

		public int XDies
		{
			get{return m_HeaderData.XDies;}
			set{m_HeaderData.XDies = value;}
		}

		public int YDies
		{
			get{return m_HeaderData.YDies;}
			set{m_HeaderData.YDies = value;}
		}

		public int DieIndexMinX
		{
			get{return m_HeaderData.DieIndexMinX;}
			set{m_HeaderData.DieIndexMinX = value;}
		}

		public int DieIndexMaxX
		{
			get{return m_HeaderData.DieIndexMaxX;}
			set{m_HeaderData.DieIndexMaxX = value;}
		}

		public int DieIndexMinY
		{
			get{return m_HeaderData.DieIndexMinY;}
			set{m_HeaderData.DieIndexMinY = value;}
		}

		public int DieIndexMaxY
		{
			get{return m_HeaderData.DieIndexMaxY;}
			set{m_HeaderData.DieIndexMaxY = value;}
		}

		public double ChipSizeX
		{
			get{return m_HeaderData.ChipSizeX;}
			set{m_HeaderData.ChipSizeX = value;}
		}

		public double ChipSizeY
		{
			get{return m_HeaderData.ChipSizeY;}
			set{m_HeaderData.ChipSizeY = value;}
		}

		public double OriginX
		{
			get{return m_HeaderData.OriginX;}
			set{m_HeaderData.OriginX = value;}
		}

		public double OriginY
		{
			get{return m_HeaderData.OriginY;}
			set{m_HeaderData.OriginY = value;}
		}

		public int OriginDieX
		{
			get{return m_HeaderData.OriginDieX;}
			set{m_HeaderData.OriginDieX = value;}
		}

		public int OriginDieY
		{
			get{return m_HeaderData.OriginDieY;}
			set{m_HeaderData.OriginDieY = value;}
		}
	
		public double TargetX
		{
			get{return m_HeaderData.TargetX;}
			set{m_HeaderData.TargetX = value;}
		}

		public double TargetY
		{
			get{return m_HeaderData.TargetY;}
			set{m_HeaderData.TargetY = value;}
		}

		public int TargetDieX
		{
			get{return m_HeaderData.TargetDieX;}
			set{m_HeaderData.TargetDieX = value;}
		}

		public int TargetDieY
		{
			get{return m_HeaderData.TargetDieY;}
			set{m_HeaderData.TargetDieY = value;}
		}

		public int FirstDieX
		{
			get{return m_HeaderData.FirstDieX;}
			set{m_HeaderData.FirstDieX = value;}
		}

		public int FirstDieY
		{
			get{return m_HeaderData.FirstDieY;}
			set{m_HeaderData.FirstDieY = value;}
		}

		public int XYDirection
		{
			get{return m_HeaderData.XYDirection;}
			set{m_HeaderData.XYDirection = value;}
		}

		public int ProbingStartPosition
		{
			get{return m_HeaderData.ProbingStartPosition;}
			set{m_HeaderData.ProbingStartPosition = value;}
		}

		public int ProbingDirection
		{
			get{return m_HeaderData.ProbingDirection;}
			set{m_HeaderData.ProbingDirection = value;}
		}
		
		public int XDirection
		{
			get{return m_HeaderData.XDirection;}
			set{m_HeaderData.XDirection = value;}
		}

		public int YDirection
		{
			get{return m_HeaderData.YDirection;}
			set{m_HeaderData.YDirection = value;}
		}

		public int TestCount
		{
			get{return m_HeaderData.TestCount;}
			set{m_HeaderData.TestCount = value;}
		}

		public string WaferLoadingStartTime
		{
			get{return m_HeaderData.WaferLoadingStartTime;}
			set{m_HeaderData.WaferLoadingStartTime = value;}
		}

		public string WaferUnloadingStartTime
		{
			get{return m_HeaderData.WaferUnloadingStartTime;}
			set{m_HeaderData.WaferUnloadingStartTime = value;}
		}

		public int MachineNo1
		{
			get{return m_HeaderData.MachineNo1;}
			set{m_HeaderData.MachineNo1 = value;}
		}

		public int MachineNo2
		{
			get{return m_HeaderData.MachineNo2;}
			set{m_HeaderData.MachineNo2 = value;}
		}

		public int SpecialWord
		{
			get{return m_HeaderData.SpecialWord;}
			set{m_HeaderData.SpecialWord = value;}
		}

		public int TestingFinishStatus
		{
			get{return m_HeaderData.TestingFinishStatus;}
			set{m_HeaderData.TestingFinishStatus = value;}
		}

		public int ReferenceDieSetting
		{
			get{return m_HeaderData.ReferenceDieSetting;}
			set{m_HeaderData.ReferenceDieSetting = value;}
		}

		public int AreaStartAdress
		{
			get{return m_HeaderData.AreaStartAdress;}
			set{m_HeaderData.AreaStartAdress = value;}
		}

		public int LineCatData
		{
			get{return m_HeaderData.LineCatData;}
			set{m_HeaderData.LineCatData = value;}
		}

		public int LineCatAddress
		{
			get{return m_HeaderData.LineCatAddress;}
			set{m_HeaderData.LineCatAddress = value;}
		}

		public int MapFile
		{
			get{return m_HeaderData.MapFile;}
			set{m_HeaderData.MapFile = value;}
		}

		public int MultiSite
		{
			get{return m_HeaderData.MultiSite;}
			set{m_HeaderData.MultiSite = value;}
		}

		public int Category
		{
			get{return m_HeaderData.Category;}
			set{m_HeaderData.Category = value;}
		}

		public string MachineNo
		{
			get{return m_HeaderData.MachineNo;}
			set{m_HeaderData.MachineNo = value;}
		}

		public int LastEditEquipKind
		{
			get{return m_HeaderData.LastEditEquipKind;}
			set{m_HeaderData.LastEditEquipKind = value;}
		}

		public int MapVersion
		{
			get{return m_HeaderData.MapVersion;}
			set{m_HeaderData.MapVersion = value;}
		}

		public int MapDataKind
		{
			get{return m_HeaderData.MapDataKind;}
			set{m_HeaderData.MapDataKind = value;}
		}

		public int DContact
		{
			get{return m_HeaderData.DContact;}
			set{m_HeaderData.DContact = value;}
		}

		public string Title
		{
			get{return m_HeaderData.Title;}
			set{m_HeaderData.Title = value;}
		}

		public int MC
		{
			get{return m_HeaderData.MC;}
			set{m_HeaderData.MC = value;}
		}

        public int WaferNo
        {
            get { return m_HeaderData.WaferNo; }
            set { m_HeaderData.WaferNo = value; }
        }
        public int TotalDies
        {
            get { return m_HeaderData.TotalDies; }
            set { m_HeaderData.TotalDies = value; }
        }

		#endregion -------------------------------------------------------------------------------- Interface Value

		#region -------------------------------------------------------------------------------- Process Function
		/// <summary>
		/// Title에 해당 하는 값을 가져온다
		/// </summary>
		/// <param name="name">Column Name</param>
		/// <returns>Value</returns>
		protected string GetHeaderValue(string name)
		{
			try
			{
				DataRow[] dr = m_Ds.Tables["Header"].Select(string.Format("TITLE='{0}'",name));
				return Convert.ToString(dr[0]["VALUE"]).ToUpper();	
			}
			catch
			{
				return "";
			}
		}

        protected string GetHeaderValue(string name, int No)
        {
            try
            {
                DataRow[] dr = m_Ds.Tables["Header"].Select(string.Format("TITLE='{0}'{1}", name, ",NO='"+No.ToString()+"'"));
                return Convert.ToString(dr[0]["VALUE"]).ToUpper();
            }
            catch
            {
                return "";
            }
        }

		/// <summary>
		/// Title에 해당 하는 값을 가져온다
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Title에 해당 하는 값을 가져온다
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Title에 해당 하는 값을 셋팅한다
		/// </summary>
		/// <param name="name">Title 명</param>
		/// <param name="strValue">Value</param>
		protected void SetHeaderValue(string name, string strValue)
		{
			try
			{
				DataRow[] dr = m_Ds.Tables["Header"].Select(string.Format("TITLE='{0}'",name));
				dr[0]["VALUE"] = strValue.ToUpper();
			}
			catch
			{
			}
		}

		#endregion -------------------------------------------------------------------------------- Process Function

		#region -------------------------------------------------------------------------------- Xml File Handing

		/// <summary>
		/// DataSet을 xml file로 부터 읽어온다.
		/// </summary>
		/// <param name="xmlfilePath">읽을 xml file Full Path</param>
		public void SetDataSetXmlFile(string xmlfilePath, bool valueDataInit)
		{
			try
			{
				if(valueDataInit == true)
				{
					m_Ds.Clear();
                    m_Ds = new DataSet();
				}
				m_Ds.ReadXml(xmlfilePath); 
			}
			catch
			{
			}
		}

		/// <summary>
		/// DataSet을 xml file로 내보낸다.
		/// </summary>
		/// <param name="xmlfilePath">기록할 xml file Full Path</param>
		public void OutDataSetXmlFile(string xmlfilePath)
		{
			m_Ds.WriteXml(xmlfilePath);
		}

		#endregion -------------------------------------------------------------------------------- Xml File Handing

		#region -------------------------------------------------------------------------------- Binary, Read, Write

		/// <summary>
		/// Data File을 읽는다
		/// </summary>
		/// <param name="filePath">읽을 Data File Full Path Name</param>
		/// <returns>성공여부(0:실패, 1:성공)</returns>
		protected int ReadBinaryDataFile(string filePath)
		{
			BinaryConvert bc;

			try
			{
				bc = new BinaryConvert(ref m_Ds, 1);
				bc.ReadBinaryDataFile(filePath); 
			}
			catch
			{
			}
			finally
			{
				bc = null;
			}
			return 1;
		}

        protected int ReadBinaryDataFileGMS(string filePath, string LotWafer)
        {
            BinaryConvert bc;

            try
            {
                bc = new BinaryConvert(ref m_Ds, 1);
                bc.ReadBinaryDataFileGMS(filePath, LotWafer);

                GMS_DataSet = bc.GetDataTable();
            }
            catch
            {
            }
            finally
            {
                bc = null;
            }
            return 1;
        }

        protected DataSet GetGMSDataSet()
        {
            return GMS_DataSet;
        }
		
		/// <summary>
		/// Data File을 만든다
		/// </summary>
		/// <param name="filePath">기록할 File Full Path Name</param>
		/// <returns>성공여부(0:실패, 1:성공)</returns>
		protected int WriteBinaryDataFile(string filePath)
		{
			BinaryConvert bc;

			try
			{
				bc = new BinaryConvert(ref m_Ds, 1);
				bc.WriteBinaryDataFile(filePath); 
			}
			catch
			{
			}
			finally
			{
				bc = null;
			}
			return 1;
		}

		#endregion -------------------------------------------------------------------------------- Binary, Read, Write
	}
}
