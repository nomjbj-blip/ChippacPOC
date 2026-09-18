using System;
using System.IO;
using System.Text;
using System.Data; 
using System.Collections;

namespace FDll
{
	/// <summary>
	/// BinaryConvert에 대한 요약 설명입니다.
	/// </summary>
	internal class BinaryConvert
	{
		private ASCIIEncoding m_Ae = null;			// byte배열을 strig으로 변환하는 Class
		private DataSet m_Ds = null;				// Data 정보 Data	
		private int m_DataKind = 0;					// CPU ALU 처리방식 0 : U~L, 1 : L~U

		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="ds">DataSet</param>
		/// <param name="dataKind">CPU ALU 처리방식 0 : U~L, 1 : L~U</param>
		public BinaryConvert(ref DataSet ds, int dataKind)
		{
			m_Ae = new ASCIIEncoding();
			m_Ds = ds;
			m_DataKind = dataKind;
		}

		/// <summary>
		/// 소멸자
		/// </summary>
		~BinaryConvert()
		{
			m_Ae = null;
		}

		#region -------------------------------------------------------------------------------- File Handing

		/// <summary>
		/// Data File을 읽는다
		/// </summary>
		/// <param name="filePath">읽을 Data File Full Path Name</param>
		/// <returns>성공여부(0:실패, 1:성공)</returns>
		public int ReadBinaryDataFile(string filePath)
		{
			FileStream fs = null;
			BinaryReader br = null;

			try
			{
				fs = new FileStream(filePath, System.IO.FileMode.Open);	// File Open
				br = new BinaryReader(fs); 

				ParseHeadBinaryToDataSet(ref br);
				ParseTestDataBinaryToDataSet(ref br);
			}
			catch
			{
				return 0;
			}
			finally
			{
				if(br != null) br.Close();
				if(fs != null) fs.Close();
			}

			return 1;
		}

        public int ReadBinaryDataFileGMS(string filePath, string LotWafer)
        {
            FileStream fs = null;
            BinaryReader br = null;

            try
            {
                fs = new FileStream(filePath, System.IO.FileMode.Open);	// File Open
                br = new BinaryReader(fs);

                // Header Info Parse
                ParseHeadBinaryToDataSetGMS(ref br);

                // Mapdata Parse
                if(LotWafer == "WAFER")
                    ParseTestDataBinaryToDataSetGMS(ref br);

                //ParseTestDataBinaryToDataSet(ref br);
            }
            catch
            {
                return 0;
            }
            finally
            {
                if (br != null) br.Close();
                if (fs != null) fs.Close();
            }

            return 1;
        }

        public DataSet GetDataTable()
        {
            return m_Ds;
        }

		/// <summary>
		/// Data File을 만든다
		/// </summary>
		/// <param name="filePath">기록할 File Full Path Name</param>
		/// <returns>성공여부(0:실패, 1:성공)</returns>
		public int WriteBinaryDataFile(string filePath)
		{
			FileStream fs = null;
			BinaryWriter bw = null;

			try
			{
				fs = new FileStream(filePath, System.IO.FileMode.Create );	// File Create
				bw = new BinaryWriter(fs); 

				ParseHeadDataSetToBinary(ref bw);
				ParseTestDataDataSetToBinary(ref bw);
			}
			catch
			{
				return 0;
			}
			finally
			{
				if(bw != null) bw.Close();
				if(fs != null) fs.Close();
			}

			return 1;
		}

		#endregion -------------------------------------------------------------------------------- File Handing

		#region -------------------------------------------------------------------------------- Binary Head

		/// <summary>
		/// Header ByteData File을 분석 DataSet에 입력
		/// </summary>
		/// <param name="br">BinaryReader</param></param>
		private void ParseHeadBinaryToDataSet(ref BinaryReader br)
		{
			int num;
			string type;
			int size;

			foreach(DataRow dr2 in m_Ds.Tables["Header"].Rows)
			{
				num = Convert.ToInt32(dr2["NO"]); 
				type = Convert.ToString(dr2["TYPE"]);
				size = Convert.ToInt32(dr2["SIZE"]);
				
				dr2["VALUE"] = ValueParseBinaryToType(ref br, type, size);
			}
		}

        private void ParseHeadBinaryToDataSetGMS(ref BinaryReader br)
        {
            int num;
            string type;
            int size;

            foreach (DataRow dr2 in m_Ds.Tables["Header"].Rows)
            {
                num = Convert.ToInt32(dr2["NO"]);
                type = Convert.ToString(dr2["TYPE"]);
                size = Convert.ToInt32(dr2["SIZE"]);

                dr2["VALUE"] = ValueParseBinaryToTypeGMS(ref br, type, size);
            }
        }


		/// <summary>
		/// DataSet을 분석 Header ByteData File에 기록
		/// </summary>
		/// <param name="bw"></param>
		private void ParseHeadDataSetToBinary(ref BinaryWriter bw)
		{
			string type;
			string strValue;
			int size;
			
			foreach(DataRow dr in m_Ds.Tables["Header"].Rows)
			{
				type = Convert.ToString(dr["TYPE"]);
				size = Convert.ToInt32(dr["SIZE"]);
				strValue = Convert.ToString(dr["VALUE"]);

				bw.Write(ValueParseTypeToBinary(strValue, type, size));
			}
		}

		#endregion -------------------------------------------------------------------------------- Binary Head
	
		#region -------------------------------------------------------------------------------- Binary Test Data

		/// <summary>
		/// Test ByteData File을 분석 DataSet에 입력
		/// </summary>
		/// <param name="br">BinaryReader</param></param>
		private void ParseTestDataBinaryToDataSet(ref BinaryReader br)
		{
			int num;
			string type;
			int size;
			int rowNum = 0;

			DataTable dt = m_Ds.Tables["DataData"]; 
			DataRow dr;
			dt.Rows.Clear(); 

			try
			{
				while(true)
				{	
					dr = dt.NewRow(); 
					rowNum++;
					dr[0] = rowNum;
					foreach(DataRow dr2 in m_Ds.Tables["DataSchma"].Rows)
					{
						num = Convert.ToInt32(dr2["NO"]); 
						type = Convert.ToString(dr2["TYPE"]);
						size = Convert.ToInt32(dr2["SIZE"]);
				
						dr[num] = ValueParseBinaryToType(ref br, type, size);
					}
					dt.Rows.Add(dr);
				}
			}
			catch
			{
			} 
		}

        private void ParseTestDataBinaryToDataSetGMS(ref BinaryReader br)
        {
            
            string[] arrBin = null;
            int size = 0;
            string type = string.Empty;
            string title = string.Empty;

            int iMinXstartPoint = 0;

            try
            {
                // Row Count Select (총 Row Count)
                DataRow[] drSelect = m_Ds.Tables["Header"].Select("TITLE = 'Records'");
                int rowCnt = Convert.ToInt32(drSelect[0]["VALUE"].ToString());
                
                byte[] arrByte = null;
                int[] arrXStartPoint = null;

                arrBin = new string[rowCnt];
                arrXStartPoint = new int[rowCnt];

                int binCnt = 0;
                for (int i = 0; i < rowCnt; i++)
                {
                    ///  X의 Data 시작좌표 : arrXStartPoint
                    ///  한 Row의 Die 개수 : binCnt
                    foreach (DataRow dr2 in m_Ds.Tables["DataData"].Rows)
                    {
                        type = Convert.ToString(dr2["TYPE"]);  
                        size = Convert.ToInt32(dr2["SIZE"]);
                        title = Convert.ToString(dr2["TITLE"]);

                        dr2["VALUE"] = ValueParseBinaryToTypeGMS(ref br, type, size);

                        switch (title.ToUpper())
                        {
                            // First X Die Position (얼마나 앞을 띄어야 하는지)
                            case "FIRSTX":
                                arrXStartPoint[i] = Convert.ToInt32(dr2["VALUE"].ToString());
                                if (arrXStartPoint[i] > 1000)
                                {
                                    arrXStartPoint[i] = 0;
                                }

                                if (iMinXstartPoint > arrXStartPoint[i])
                                    iMinXstartPoint = arrXStartPoint[i];

                                break;
                            // Bin Count on 1 Row (몇개의 Bin 이 한 Row 에 있는지)
                            case "NUMBEROFDIES":
                                binCnt = Convert.ToInt32(dr2["VALUE"].ToString());
                                break;
                        }
                    }

                    // Bin Data put into Row
                    for (int j = 0; j < binCnt; j++)
                    {
                        arrByte = (byte[])(ValueParseBinaryToTypeGMS(ref br, "byte", 2));
                        char binChar2 = Convert.ToChar(arrByte[0]);
                        arrBin[i] += binChar2.ToString();
                    }
                }

                // Add Blank to Bin Data Row
                for (int i = 0; i < arrBin.Length; i++)
                {
                    for (int j = iMinXstartPoint; j < arrXStartPoint[i]; j++)
                        arrBin[i] = " " + arrBin[i];

                    arrBin[i] = arrBin[i].Replace("Y", "#");
                    //arrBin[i] = arrBin[i].Replace("#", " ");
                }

                //string strBin2 = string.Empty;
                //for (int i = 0; i < arrBin.Length; i++)
                //{
                //    strBin2 += (arrBin[i] + "\n");
                //}

                m_Ds.Tables["DataData"].Clear();

                for (int i = 0; i < arrBin.Length; i++)
                {
                    DataRow dr =  m_Ds.Tables["DataData"].NewRow();
                    dr["VALUE"] = arrBin[i];
                    m_Ds.Tables["DataData"].Rows.Add(dr);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

		/// <summary>
		/// DataSet을 분석 Test ByteData File에 기록
		/// </summary>
		/// <param name="bw">BinaryWrite</param>
		private void ParseTestDataDataSetToBinary(ref BinaryWriter bw)
		{
			int rownum;
			int colnum;
			string type;
			string strValue;
			int size;

			DataTable dt = m_Ds.Tables["DataData"]; 

			rownum = 0;

			try
			{
				while(true)
				{
					foreach(DataRow dr2 in m_Ds.Tables["DataSchma"].Rows)
					{
						colnum = Convert.ToInt32(dr2["NO"]); 
						type = Convert.ToString(dr2["TYPE"]);
						size = Convert.ToInt32(dr2["SIZE"]);
						strValue = Convert.ToString(dt.Rows[rownum][colnum]);
				
						bw.Write(ValueParseTypeToBinary(strValue, type, size));
					}
					rownum++;
				}
			}
			catch
			{
			}
		}

		#endregion -------------------------------------------------------------------------------- Binary Test Data

		#region -------------------------------------------------------------------------------- Convert Function

		/// <summary>
		/// FileStream을 읽어 해당 정보를 반환한다.
		/// </summary>
		/// <param name="br">File BinaryReader</param>
		/// <param name="type">Data Type</param>
		/// <param name="size">Data Size</param>
		/// <returns></returns>
		private object ValueParseBinaryToType(ref BinaryReader br,  string type, int size)
		{
			int i;
	
			byte[] byteBuffer = br.ReadBytes(size);

			if(type == "string")
			{
				return m_Ae.GetString(byteBuffer, 0, byteBuffer.Length);
			}

			if(m_DataKind == 1 && size > 1)	
			{
                byte[] byteBufferTemp = new byte[size];
                for (i = 0; i < size; i++) byteBufferTemp[i] = byteBuffer[size - 1 - i];
                for (i = 0; i < size; i++) byteBuffer[i] = byteBufferTemp[i];
                byteBufferTemp = null;
			}
			
			switch(type)
			{
				case "byte":
                {
                    if(byteBuffer.Length == 1)
                        return byteBuffer[0];
                    else
					    return byteBuffer;
                }
				case "UInt16":
					return BitConverter.ToUInt16(byteBuffer, 0);

				case "UInt32":
					return BitConverter.ToUInt32(byteBuffer, 0);

				case "UInt64":
					return BitConverter.ToUInt64(byteBuffer, 0);

				case "Int16":
					return BitConverter.ToInt16(byteBuffer, 0);

				case "Int32":
					return BitConverter.ToInt32(byteBuffer, 0);

				case "Int64":
					return BitConverter.ToInt64(byteBuffer, 0);

				case "Single":
					return BitConverter.ToSingle(byteBuffer, 0);
				
				case "double":
					return BitConverter.ToDouble(byteBuffer, 0);

                case "bcd":
                    long result = 0;
                    foreach (byte b in byteBuffer)
                    {
                        int digit1 = b >> 4; 
                        int digit2 = b & 0x0F;
                        result = (result * 100) + digit1 * 10 + digit2;
                    }
                    return result;
			}

			return 0;
		}

        private object ValueParseBinaryToTypeGMS(ref BinaryReader br, string type, int size)
        {
            //int i;

            byte[] byteBuffer = br.ReadBytes(size);

            if (type == "string")
            {
                string strTemp = m_Ae.GetString(byteBuffer, 0, byteBuffer.Length);

                return strTemp;
            }
            if (type == "bcd")
            {
                string str = string.Empty;
                for (int j = 0; j < byteBuffer.Length; j++)
                {
                    str = str + byteBuffer[j].ToString();
                }
                return str;
                //return m_Ae.GetChars(byteBuffer);
            }
            //if (m_DataKind == 1 && size > 1)
            //{
            //    byte[] byteBufferTemp = new byte[size];
            //    for (i = 0; i < size; i++) byteBufferTemp[i] = byteBuffer[size - 1 - i];
            //    for (i = 0; i < size; i++) byteBuffer[i] = byteBufferTemp[i];
            //    byteBufferTemp = null;
            //}

            switch (type)
            {
                case "byte":
                {
                    if (byteBuffer.Length == 1)
                        return byteBuffer[0];
                    else
                        return byteBuffer;
                }

                case "UInt16":
                    return BitConverter.ToUInt16(byteBuffer, 0);

                case "UInt32":
                    return BitConverter.ToUInt32(byteBuffer, 0);

                case "UInt64":
                    return BitConverter.ToUInt64(byteBuffer, 0);

                case "Int16":
                    return BitConverter.ToInt16(byteBuffer, 0);

                case "Int32":
                    return BitConverter.ToInt32(byteBuffer, 0);

                case "Int64":
                    return BitConverter.ToInt64(byteBuffer, 0);

                case "Single":
                    return BitConverter.ToSingle(byteBuffer, 0);

                case "double":
                    return BitConverter.ToDouble(byteBuffer, 0);
                //case "bcd":
                //    //StringBuilder bcd = new StringBuilder(byteBuffer.Length * 2);
                //    //long result = 0;
                //    int result = 0;
                //    foreach (byte bcdByte in byteBuffer)
                //    {
                //        //int idHigh = bcdByte >> 4;
                //        //int idLow = bcdByte & 0x0f;
                //        //int idHigh = (bcdByte & 0x0f) >> 4;
                //        //int idLow = bcdByte & 0xf;
                //        int digit1 = bcdByte >> 4;
                //        int digit2 = bcdByte & 0x0f;
                //        //bcd.Append(string.Format("{0}{1}", idHigh, idLow));
                //        result = (result * 100) + digit1 * 10 + digit2;
                //    }
                //    //decimal dc = Convert.ToDecimal(bcd.ToString());
                //    //return Convert.ToDateTime(dc);
                //    return result;
            }

            return 0;
        }

		/// <summary>
		/// DataSet을 읽어 byte[]로 반환
		/// </summary>
		/// <param name="strValue">Value</param>
		/// <param name="type">Data Type</param>
		/// <param name="size">Data Size</param>
		/// <returns></returns>
		private byte[] ValueParseTypeToBinary(string strValue, string type, int size)
		{
			int i;
			byte[] byteBuffer =  null;
	
			if(type == "string")
			{
				strValue = strValue.PadRight(size, ' '); 
				return m_Ae.GetBytes(strValue);
			}

			if(strValue == "") strValue = "0";
		
			switch(type)
			{
				case "byte":
					byteBuffer = new byte[1];
					byteBuffer[0] = Convert.ToByte(strValue);
					break;

				case "UInt16":
					byteBuffer = BitConverter.GetBytes(Convert.ToUInt16(strValue));
					break;

				case "UInt32":
					byteBuffer = BitConverter.GetBytes(Convert.ToUInt32(strValue));
					break;

				case "UInt64":
					byteBuffer = BitConverter.GetBytes(Convert.ToUInt64(strValue));
					break;

				case "Int16":
					byteBuffer = BitConverter.GetBytes(Convert.ToInt16(strValue));
					break;
				
				case "Int32":
					byteBuffer = BitConverter.GetBytes(Convert.ToInt32(strValue));
					break;
				
				case "Int64":
					byteBuffer = BitConverter.GetBytes(Convert.ToInt64(strValue));
					break;

				case "Single":
					byteBuffer = BitConverter.GetBytes(Convert.ToSingle(strValue));
					break;

				case "double":
					byteBuffer = BitConverter.GetBytes(Convert.ToDouble(strValue));
					break;
			}

			if(m_DataKind == 1 && size > 1)	
			{
				byte[] byteBufferTemp = new byte[size];
				for(i = 0; i < size; i++) byteBufferTemp[i] = byteBuffer[size - 1 - i];
				for(i = 0; i < size; i++) byteBuffer[i] = byteBufferTemp[i];
			}

			return byteBuffer;
		}

		#endregion -------------------------------------------------------------------------------- Convert Function

	}
}
