using System;

namespace DACrux.MESIF.DSL
{
    public class MES_MWIPCRRLOT : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public MES_MWIPCRRLOT()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["MES_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "MES_MWIPCRRLOT.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
               
        // ----------------------------------------------------------------------------------------------------------------------

        public System.Data.DataTable SELECT_LOT_STATUS(
            string factory, 
            string carrier_id
            )
        {
            try
            {
                return this.GetDataTable("SELECT_LOT_STATUS", null, new string[] { factory, carrier_id });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // ----------------------------------------------------------------------------------------------------------------------
    }
}
