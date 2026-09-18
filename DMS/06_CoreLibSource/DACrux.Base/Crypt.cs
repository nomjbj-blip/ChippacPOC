/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : Crypt.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2014.11.14
--  Description     : DACrux Core::Password Encoding / Decoding
--  History         : Created by YSIM at 2014.11.14
 * ********************************************************************************************************
 * 2014 년 DACrux V4 History Reset

----------------------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace DACrux.Base
{
    public static class Crypt
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
        public static string GetEncoding(string strPasswd)
        {
            return EncryptData(CRYPTOKEY, strPasswd);
        }

        /// <summary>
        /// Return original string for password
        /// </summary>
        /// <param name="strPasswd">Password</param>
        /// <returns>Original string</returns>
        public static string GetDecoding(string strPasswd)
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
                strResult = System.Convert.ToBase64String(buff, 0, buff.Length);
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
            byte[] rbData = System.Convert.FromBase64CharArray(carray, 0, carray.Length);

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
