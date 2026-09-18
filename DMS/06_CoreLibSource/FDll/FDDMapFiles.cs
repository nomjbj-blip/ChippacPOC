using System;
using System.IO;
using System.Collections;
using System.Text;
using System.Windows.Forms;

namespace FDll
{
	/// <summary>
	/// FDDMapFiles에 대한 요약 설명입니다.
	/// </summary>
	public class FDDMapFiles
	{
		private string m_LotID = null;
		private ArrayList m_Wafers = null;
		private ArrayList m_FileList = null;
		/// <summary>
		/// 생성자
		/// </summary>
		public FDDMapFiles(string lotID, ArrayList wafers, ArrayList fileList)
		{
			m_LotID = lotID;
			m_Wafers = wafers;
			m_FileList = fileList;			  
		}

		/// <summary>
		/// 소멸자
		/// </summary>
		~FDDMapFiles()
		{
			m_LotID = null;
			m_Wafers = null;
		}

		#region -------------------------------------------------------------------------------- Act

		/// <summary>
		/// Fdd에 전송될 File들을 만든다.
		/// </summary>
		public void MakeFiles(string filePath)
		{
			MakeMapIDXFile(filePath);
		}

		/// <summary>
		/// File이 옮겨질 path
		/// </summary>
		/// <param name="path"></param>
		public void MoveFiles(string path, bool copy)
		{
			MoveFilesAct(path, copy);
		}

		#endregion -------------------------------------------------------------------------------- Act

		#region -------------------------------------------------------------------------------- Process

		/// <summary>
		/// File들을 이동 시킨다.
		/// </summary>
		/// <param name="path"></param>
		private void MoveFilesAct(string path, bool copy)
		{
			string inputFile;
			string outFile;
			int i;
		
			try
			{
				for(i = 0; i < m_Wafers.Count; i++)
				{
					inputFile = m_FileList[i].ToString(); 
					outFile = path + @"\MAP_" + m_Wafers[i].ToString().PadLeft(4, '0') +  @".DAT";
				
					if(copy)
						File.Copy(inputFile, outFile, true);
					else
						File.Move(inputFile, outFile); 
				}
			}
			catch
			{		
			}
			
		}

		/// <summary>
		/// Index File을 만든다
		/// </summary>
		private void MakeMapIDXFile(string filePath)
		{
			int i;
			string str;
			UInt16 ui16;
			ASCIIEncoding ae = new ASCIIEncoding();
			FileStream fs = null;
			BinaryWriter bw = null;
			
			try
			{
				fs = new FileStream(filePath, System.IO.FileMode.Create);
				bw = new BinaryWriter(fs); 

				byte[] byteBuffer;
				byte[] b = new byte[250];  
				byte[] b2 = new byte[]{0x20, 0x20, 0x20, 0x20, 0x20, 
										  0x20, 0x20, 0x20, 0x20, 0x20,
										  0x20, 0x20, 0x20, 0x20, 0x20,
										  0x20, 0x20, 0x20, 0x20, 0x20,
										  0x20, 0x00, 0x30, 0x20, 0x20, 
										  0x20, 0x20 };

				for(i = 0; i < m_Wafers.Count; i++)
				{
					ui16 = (UInt16)(Convert.ToUInt16(m_Wafers[i]) - 1);	
					b[(UInt16)(ui16 / 8)] = (byte)(b[(UInt16)(ui16 / 8)] | (0x01 << (UInt16)(ui16 % 8)));
				}

				b[0] = (byte)(0xFF);
			
				ui16 = (UInt16)(m_Wafers.Count);
				ui16 = (UInt16)(((ui16 >> 8) & 0xFF) | ((ui16 & 0xFF) << 8)); 

				bw.Write((UInt16)(0x0001));
				bw.Write((UInt16)(0xc800));
				bw.Write((UInt16)(ui16));
				bw.Write(b);

				for(i = 0; i < m_Wafers.Count; i++)
				{
					str = m_LotID + "-" + Convert.ToString(m_Wafers[i]).PadLeft(2, '0'); 
					str = str.PadRight(20, ' ') + "\00"; 
					str = str + Convert.ToString(m_Wafers[i]).PadLeft(4, '0'); 	

					byteBuffer =  ae.GetBytes(str); 
				
					bw.Write(byteBuffer);
				}

				for(i = 0; i < 200; i++)
				{
					bw.Write(b2); 
				}
			}
			catch
			{
			}
			finally
			{	
				if(bw != null)bw.Close();
				if(fs != null)fs.Close();
				bw = null;
				fs = null;
			}
		}

		#endregion -------------------------------------------------------------------------------- Process
	}
}
