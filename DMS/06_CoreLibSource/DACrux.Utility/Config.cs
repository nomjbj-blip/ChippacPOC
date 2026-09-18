using System;
using System.IO;
using System.Windows.Forms;
using System.Reflection;

namespace DACrux.Utility
{
    /// <summary>
    /// Name       : Config<br/>
    /// Summary    : Dacrux Environment Definition<br/>
    /// Author     : Miracom David, Kwak<br/>
    /// First Date : 2009-02-23<br/>
    /// Description: <br/>
    /// History    : 2012-11-07 YSIM Class Naming Á¤¸®<br/>
    /// </summary>
    public struct Config
	{
		#region Class Member

        static private string strCompany = string.Empty;
        static private string strCustomer = string.Empty;
        static private string strAdminMail = string.Empty;
        static private string strCompanyTelNumber = string.Empty;
        static private string strCustomerTelNumber = string.Empty;
        static private string strProductName = string.Empty;
        static private string strVersion = string.Empty;
		static private string strTempFolder = string.Empty;

		#endregion

		#region Properties

		/// <summary>
		/// Get Manufacturer Name
		/// </summary>
		public string Company
		{
			get{	return strCompany;	}
		}

		/// <summary>
		/// Get Customer
		/// </summary>
		public string Customer
		{
			get{	return strCustomer;	}
		}

		/// <summary>
		/// Get Admin Email Address
		/// </summary>
		public string AdminMail
		{
			get{	return strAdminMail;	}
		}

		/// <summary>
		/// Get Manufacturer Tel No
		/// </summary>
		public string CompanyTelNumber
		{
			get{	return strCompanyTelNumber;	}
		}

		/// <summary>
		/// Get Customer Tel No
		/// </summary>
		public string CustomerTelNumber
		{
			get{	return strCustomerTelNumber;	}
		}

		/// <summary>
		/// Get Product Name
		/// </summary>
		public string ProductName
		{
			get{	return strProductName;	}
		}

		/// <summary>
		/// Get Assembly Version
		/// </summary>
		public string Version
		{
			get{	return strVersion;	}
		}

		/// <summary>
		/// Get Temporary Folder
		/// </summary>
		public string TempFolder
		{
			get{	return strTempFolder;	}
		}

		#endregion

        #region SetConfig - INI
        /// <summary>
		/// Read Default Information From INI File
		/// </summary>
		public void SetConfig()
		{

            string strCurFolder = string.Empty; 

			try
			{
                strCurFolder = Application.StartupPath.ToString();

				IniHandle Ini = new IniHandle(strCurFolder + "\\DACrux.ini");

                FileInfo iniFile = new FileInfo(strCurFolder + "\\DACrux.ini");

				strCompany           = Ini.IniReadValue("DACrux","Company");
				strCustomer			 = Ini.IniReadValue("DACrux","Customer");
				strAdminMail		 = Ini.IniReadValue("DACrux","AdminMail");
				strCompanyTelNumber	 = Ini.IniReadValue("DACrux","CompanyTelNumber");
				strCustomerTelNumber = Ini.IniReadValue("DACrux","CustomerTelNumber");
				strProductName		 = Ini.IniReadValue("DACrux","ProductName");
				strTempFolder		 = Ini.IniReadValue("DACrux","TempFolder");
				strVersion			 = GetAssemblyVersion();
			}
			catch(Exception ex)
			{
                throw ex;
			}
		}

		#endregion

		#region GetAssemblyVersion

		/// <summary>
		/// Return Assembly Version
		/// </summary>
		/// <returns>Version</returns>
		public string GetAssemblyVersion()
		{
			Assembly assembly = Assembly.LoadFrom(Application.ExecutablePath);
			string [] arrTmp = assembly.FullName.Split(',');
			return arrTmp[1].ToLower().Replace("version=","");
		}

		#endregion
	}
	
}
