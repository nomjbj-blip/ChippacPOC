/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2015 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : ApplicationUnit
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2015.01.30
--  Description     : ApplicationUnit
--  History         : Created by YSIM at 2015.01.30
 * ********************************************************************************************************
----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;

namespace DACrux.Base
{
    public enum ApplicationUnit
    {
        //-------DACRUX COMMON--------------------------------------------------------------------
        MIRACOM_QMS_COMMON = 0,

        //-------DACRUX--------------------------------------------------------------------
        MIRACOM_DACRUX_RMS = 1,
        MIRACOM_DACRUX_PMS = 2,

        MIRACOM_DACRUX_PRB = 3,              //old : MIRACOM_DACRUX_TEST = 7413,
        MIRACOM_DACRUX_PKG = 4,              //PKT REPORT UI   //old : MIRACOM_QMS_TEST = 7414,
        MIRACOM_DACRUX_DMS = 5,
        MIRACOM_DACRUX_PCM = 6,
        MIRACOM_DACRUX_SPC = 7,

        MIRACOM_DACRUX_TMS = 8,          // TAMS
        MIRACOM_DACRUX_YMS = 9,

        //-------EMS--------------------------------------------------------------------
        MIRACOM_EMS_DA = 10,
        MIRACOM_EMS_DAOPER = 11,
        MIRACOM_EMS_WB = 12,                 //old : MIRACOM_EMS_WB = 7412,
        MIRACOM_EMS_DP = 13,
        MIRACOM_EMS_ENGUI = 14

        // Default Port
        //-----------------------------------------------------------------------------------
        //-------DACRUX COMMON---------------------------------------------------------------
        //MIRACOM_QMS_COMMON = 7231,

        ////-------DACRUX--------------------------------------------------------------------
        //MIRACOM_DACRUX_RMS = 7411,
        //MIRACOM_DACRUX_PMS = 7412,

        //MIRACOM_DACRUX_PRB = 7413,              //old : MIRACOM_DACRUX_TEST = 7413,
        //MIRACOM_DACRUX_PKG = 7414,              //PKT REPORT UI   //old : MIRACOM_QMS_TEST = 7414,
        //MIRACOM_DACRUX_DMS = 7415,
        //MIRACOM_DACRUX_PCM = 7416,
        //MIRACOM_DACRUX_SPC = 7417,

        //MIRACOM_DACRUX_TMS = 7418,              // TAMS
        //MIRACOM_DACRUX_YMS = 7419,

        ////-------EMS------------------------------------------------------------------------
        //MIRACOM_EMS_DA = 7420,
        //MIRACOM_EMS_DAOPER = 7421,
        //MIRACOM_EMS_WB = 7422,                  //old : MIRACOM_EMS_WB = 7412,
        //MIRACOM_EMS_DP = 7423
    }
}
