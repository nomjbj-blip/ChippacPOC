/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : MES_MWIPCALDEF.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2015.4.3
--  Description     : DACrux V5 MES IF DSL Class
--  History         : Created by YSIM at 2015.4.3
 * ********************************************************************************************************
 * 2014 년 DACrux V4 History Reset
 * 
 ----------------------------------------------------------------------------------------------------------*/
using System;

namespace DACrux.MESIF.DSL
{
    public class MES_MWIPCALDEF : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public MES_MWIPCALDEF()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["MES_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "MES_MWIPCALDEF.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
               
        // ----------------------------------------------------------------------------------------------------------------------

        public System.Data.DataTable SELECT_SYS_DATE(
            string startTime, 
            string endTime
            )
        {
            try
            {
                return this.GetDataTable("SELECT_SYS_DATE", null, new string[] { startTime, endTime });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // ----------------------------------------------------------------------------------------------------------------------
    }
}
