using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace DACrux.Utility
{
    /// <summary>
    /// Class Name : IniHandle<br/>
    /// Summary    : INI File Handling Class<br/>
    /// Author     : Miracom David, Kwak<br/>
    /// First Date : 2009-02-23<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class IniHandle
	{
		#region Class Member

		public string strPath;

		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section,string key,string val,string filePath);
		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section,string key,string def,StringBuilder retVal,int size,string filePath);

		#endregion

		#region Creator

		/// <summary>
		/// Initalize Class
		/// </summary>
		/// <param name="INIPath">INI File Path</param>
		public IniHandle(string INIPath)
		{
            strPath = INIPath;
		}

		#endregion

        #region IniWriteValue
        /// <summary>
		/// Write on Ini File
		/// </summary>
        /// <param name="strSection">Section name</param>
        /// <param name="strKey">Key Name</param>
        /// <param name="strValue">Value Name</param>
		public void IniWriteValue(string strSection,string strKey,string strValue)
		{
            WritePrivateProfileString(strSection, strKey, strValue, this.strPath);
		}
		#endregion

        #region IniReadValue

        /// <summary>
		/// Read Ini File
		/// </summary>
        /// <param name="strSection">Section</param>
        /// <param name="strKey">Key</param>
		/// <returns></returns>
        public string IniReadValue(string strSection, string strKey)
		{
			StringBuilder sbTemp = new StringBuilder(1024);

            int i = GetPrivateProfileString(strSection, strKey, "", sbTemp, 1024, this.strPath);
            return sbTemp.ToString();
		}

		#endregion
	}
}
