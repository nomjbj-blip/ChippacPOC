/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : DCMH.cs 
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2014.11.14
--  Description     : DACrux Message Helper Framework::DACrux Message 및 Err에 대해 통일된 처리 Class
--  History         : Created by YSIM at 2014.11.14
 * ********************************************************************************************************
 * 2014 년 DACrux V4 History Reset

----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;

namespace DACrux.Framework
{
    /// <summary>
    /// Class Name : MsgHelper<br/>
    /// Summary    : DACrux Common Error Message Process<br/>
    /// Author     : Miracom YoungShin, Lim<br/>
    /// First Date : 2008-10-22<br/>
    /// Description: <br/>
    /// History    : <br/>
    /// </summary>
    public static class DCMH
    {
        public static void DspError(Exception ex)
        {
#if DEBUG
            DspError(ex.Message + Environment.NewLine + ex.StackTrace);
#else
            DspError(ex.Message);
#endif
        }

        private static void DspError(string strError)
        {
            frmErrorBox oErrorForm = null;

            try
            {
                if (strError.StartsWith("ORA"))
                {
                    MakeOracleMessage(ref strError);
                }
                oErrorForm = new frmErrorBox(strError);
                oErrorForm.ShowDialog();
            }
            finally
            {
                if (oErrorForm != null) oErrorForm.Dispose();
            }
        }

        private static void MakeOracleMessage(ref string strError)
        {
            switch (strError.Substring(0, 9))
            {
                case "ORA-00001":
                    strError = string.Format("같은 값이 존재합니다.\n\r[ERR=>{0}]", strError);
                    break;
            }
        }

        public static void DspMessage(string strMsg)
        {
            frmMsgBox oMsgForm = null;

            try
            {
                oMsgForm = new frmMsgBox(strMsg);
                oMsgForm.ShowDialog();
            }
            finally
            {
                if (oMsgForm != null) 
                    oMsgForm.Dispose();
            }
        }
    }

}
