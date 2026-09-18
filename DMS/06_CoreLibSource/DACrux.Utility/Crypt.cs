using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using Microsoft.Win32;
using System.Diagnostics;

namespace DACrux.Utility
{
    /// <summary>
    /// Class Name : Crypt<br/>
    /// Summary    : Crypt Encoding / Decoding Handling Class<br/>
    /// Author     : Miracom David, Kwak<br/>
    /// First Date : 2009-02-23<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public class Crypt
    {
        #region Class Member

        private const string CRYPTOKEY = "MIRACOMC";

        #endregion

        #region Password Encoding / Decoding

        /// <summary>
        /// Return crypt string for password
        /// </summary>
        /// <param name="strPasswd">Password</param>
        /// <returns>crypt String</returns>
        public string GetEncoding(string strPasswd)
        {
            return EncryptData(CRYPTOKEY, strPasswd);
        }

        /// <summary>
        /// Return original string for password
        /// </summary>
        /// <param name="strPasswd">Password</param>
        /// <returns>Original string</returns>
        public string GetDecoding(string strPasswd)
        {
            return DecryptData(CRYPTOKEY, strPasswd);
        }


       

        #endregion

        #region EncryptData(string strKey, string strData)

        /// <summary>
        /// Encrypt Data
        /// </summary>
        /// <param name="strKey">Key</param>
        /// <param name="strData">String</param>
        /// <returns>Crypt Data</returns>
        private static string EncryptData(string strKey, string strData)
        {
            string strResult;

            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();

            descsp.Key = ASCIIEncoding.ASCII.GetBytes(strKey);
            descsp.IV = ASCIIEncoding.ASCII.GetBytes(strKey);

            ICryptoTransform desEncrypt = descsp.CreateEncryptor();

            MemoryStream mOut = new MemoryStream();
            CryptoStream encryptStream = new CryptoStream(mOut, desEncrypt, CryptoStreamMode.Write);

            byte[] rbData = UnicodeEncoding.Unicode.GetBytes(strData);

            try
            {
                encryptStream.Write(rbData, 0, rbData.Length);
            }
            catch
            {
                // Catch it
            }

            encryptStream.FlushFinalBlock();

            if (mOut.Length == 0)
                strResult = "";
            else
            {
                byte[] buff = mOut.ToArray();
                strResult = Convert.ToBase64String(buff, 0, buff.Length);
            }

            try
            {
                encryptStream.Close();
            }
            catch
            {

            }

            return strResult;
        }

        #endregion

        #region DecryptData

        /// <summary>
        /// Decryp tData
        /// </summary>
        /// <param name="strKey">Key</param>
        /// <param name="strData">Crypt Data</param>
        /// <returns>Original String</returns>
        private static string DecryptData(string strKey, string strData)
        {
            string strResult;

            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();

            descsp.Key = ASCIIEncoding.ASCII.GetBytes(strKey);
            descsp.IV = ASCIIEncoding.ASCII.GetBytes(strKey);

            ICryptoTransform desDecrypt = descsp.CreateDecryptor();

            MemoryStream mOut = new MemoryStream();
            CryptoStream decryptStream = new CryptoStream(mOut, desDecrypt, CryptoStreamMode.Write);
            char[] carray = strData.ToCharArray();
            byte[] rbData = Convert.FromBase64CharArray(carray, 0, carray.Length);

            try
            {
                decryptStream.Write(rbData, 0, rbData.Length);
            }
            catch
            {

            }

            decryptStream.FlushFinalBlock();

            UnicodeEncoding aEnc = new UnicodeEncoding();
            strResult = aEnc.GetString(mOut.ToArray());

            try
            {
                decryptStream.Close();
            }
            catch
            {

            }

            return strResult;
        }

        #endregion

    }
}
