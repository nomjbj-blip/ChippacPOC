using System;

namespace DACrux.MESIF.DSL
{
    public class MES_MRASCRRDEF : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public MES_MRASCRRDEF()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["MES_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "MES_MRASCRRDEF.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
               
        // ----------------------------------------------------------------------------------------------------------------------

        public System.Data.DataTable SELECT_CARRIER_STATUS(
            string factory, 
            string carrier_id
            )
        {
            try
            {
                return this.GetDataTable("SELECT_CARRIER_STATUS", null, new string[] { factory, carrier_id });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // ----------------------------------------------------------------------------------------------------------------------
    }
}
