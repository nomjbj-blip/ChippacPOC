/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : IniHandle.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2014.11.14
--  Description     : DACrux Framework::DACrux 의 Login Window
--  History         : Created by YSIM at 2014.11.14
 * ********************************************************************************************************
 * 2014 년 DACrux V4 History Reset

----------------------------------------------------------------------------------------------------------*/

using System.Runtime.InteropServices;
using System.Text;

namespace DACrux.Base
{
    public static class IniHandle
    {

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);


        #region ini 파일에 쓰기
        /// <summary>
        /// Ini File 쓰기
        /// </summary>
        /// <param name="Section">Section name</param>
        /// <param name="Key">Key Name</param>
        /// <param name="Value">Value Name</param>
        public static void IniWriteValue(string Section, string Key, string Value, string IniFile)
        {
            WritePrivateProfileString(Section, Key, Value, IniFile);
        }
        #endregion

        #region ini 파일 읽기
        /// <summary>
        /// Ini File 읽기
        /// </summary>
        /// <param name="Section">Section</param>
        /// <param name="Key">Key</param>
        /// <returns></returns>
        public static string IniReadValue(string Section, string Key, string IniFile)
        {
            StringBuilder temp = new StringBuilder(1024);
            int i = GetPrivateProfileString(Section, Key, "", temp, 1024, IniFile);
            return temp.ToString();
        }
        #endregion



    }
}
