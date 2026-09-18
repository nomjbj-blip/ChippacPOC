using System;

namespace DACrux.MESIF.DSL
{
    public class MES_MWIPSLTSTS : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public MES_MWIPSLTSTS()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["MES_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "MES_MWIPSLTSTS.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
               
        // ----------------------------------------------------------------------------------------------------------------------

        public System.Data.DataTable SELECT_SLOT_MAP(
            string factory, 
            string lot_id,
            string crr_id
            )
        {
            try
            {
                return this.GetDataTable("SELECT_SLOT_MAP", null, new string[] { factory, lot_id, crr_id });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // ----------------------------------------------------------------------------------------------------------------------
    }
}
