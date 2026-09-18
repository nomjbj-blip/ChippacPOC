using System;
using System.IO;
using System.Text;
using Microsoft.Win32;

using System.Security;
using System.Security.Cryptography;
using System.Diagnostics;

namespace DACrux.Utility
{
    /// <summary>
    /// Class Name : Regedit<br/>
    /// Summary    : Registry Handling Class<br/>
    /// Author     : Miracom David, Kwak<br/>
    /// First Date : 2009-02-23<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class Regedit
	{
		#region Class Member

		private const string COMPANY = "Miracom";
		private const string PRODUCT = "DACrux";
		
		#endregion

		#region Properties

        #region Server
        
        /// <summary>
		/// Get or Set Server IP Address
		/// </summary>
		public static string Server
		{
			get
			{
                return GetRegKey("Server");
			}
			set
			{
                SetRegKey("Server", value);
			}
		}

		#endregion

        #region SmartUpdateInfo

        /// <summary>
		/// Get or Set Smart Update Information
		/// </summary>
        public static string SmartUpdateInfo
		{
			get
			{
                return GetRegKey("SmartUpdate");
			}
			set
			{
                SetRegKey("SmartUpdate", value);
			}
		}

		#endregion

        #region ComPort

        /// <summary>
        /// Get or Set Last Login User ID
        /// </summary>
        public static string ComPort
        {
            get
            {
                return GetRegKey("ComPort");
            }
            set
            {
                SetRegKey("ComPort", value);
            }
        }

        public static string JigFontSize
        {
            get
            {
                return GetRegKey("JigFontSize");
            }
            set
            {
                SetRegKey("JigFontSize", value);
            }
        }

        public static string JigXPosition
        {
            get
            {
                return GetRegKey("JigXPosition");
            }
            set
            {
                SetRegKey("JigXPosition", value);
            }
        }

        public static string JigYPosition
        {
            get
            {
                return GetRegKey("JigYPosition");
            }
            set
            {
                SetRegKey("JigYPosition", value);
            }
        }
        #endregion

		#region LastUser

		/// <summary>
		/// Get or Set Last Login User ID
		/// </summary>
		public static string LastUser
		{
			get
			{
				return GetRegKey("LastUser");
			}
			set
			{
				SetRegKey("LastUser", value);
			}
		}

        /// <summary>
        /// Get or Set Last Login User Name
        /// </summary>
        public static string LastUserName
        {
            get
            {
                return GetRegKey("LastUserName");
            }
            set
            {
                SetRegKey("LastUserName", value);
            }
        }

		#endregion

		#endregion

		#region Set Registry Value

		/// <summary>
		/// Set Registry Value
		/// </summary>
		/// <param name="strKey">Key</param>
		/// <param name="strValue">Value</param>
		private static void SetRegKey(string strKey, string strValue)
		{
			RegistryKey HKLM          = Registry.LocalMachine ;
			RegistryKey HKSoftware    = HKLM.CreateSubKey( "SOFTWARE" ) ;
			RegistryKey HKMiracom     = HKSoftware.CreateSubKey( COMPANY ) ;
			RegistryKey HKDACrux      = HKMiracom.CreateSubKey( PRODUCT ) ;
            RegistryKey HKUserOption = HKDACrux.CreateSubKey("UserOption");

            HKUserOption.SetValue(strKey, strValue);

            HKUserOption.Close();
			HKDACrux.Close() ;
			HKMiracom.Close() ;
			HKSoftware.Close() ;
			HKLM.Close() ;
		}

		#endregion

		#region Get Registry Value

		/// <summary>
        /// Get Registry Value
		/// </summary>
        /// <param name="strKey">Key</param>
        private static string GetRegKey(string strKey)
		{
            string strValue = string.Empty;

			RegistryKey HKLM          = Registry.LocalMachine ;
			RegistryKey HKSoftware    = HKLM.CreateSubKey( "SOFTWARE" ) ;
			RegistryKey HKMiracom     = HKSoftware.CreateSubKey( COMPANY ) ;
			RegistryKey HKDACrux      = HKMiracom.CreateSubKey( PRODUCT ) ;
            RegistryKey HKUserOption = HKDACrux.CreateSubKey("UserOption");

            strValue = (string)HKUserOption.GetValue(strKey);

            HKUserOption.Close();
			HKMiracom.Close() ;
			HKDACrux.Close() ;
			HKSoftware.Close() ;
			HKLM.Close() ;

            return strValue;
		}

		#endregion
	}
}
