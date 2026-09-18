/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : iMainForm.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2015.01.14
--  Description     : DACrux Mainframe Interface
--  History         : Created by YSIM at 2014.11.14
 * ********************************************************************************************************
 * 2015 년 DACrux V5 History Start
 *  DACrux의 Stat 기능으로 Data전송
 *  Main Form에 Progress 전송
 ----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.Framework.Interface
{
    public interface iMainForm
    {
        void SendToStat(DataTable dt);
        void SendToStat(DataTable dt, string workSheetName);
        void SetMainStatusBarMsg(string strMessage);
        void SetMainStatusBarProgress(int iValue, int iMaxValue = 100);
        void SetStatusMessage(string message);
        void ShowForm(string menuKey, object parameter);
    }
}
