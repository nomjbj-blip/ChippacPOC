using System;

namespace DACrux.MESIF.DSL
{
    public class MES_MRASTOLDEF : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public MES_MRASTOLDEF()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["MES_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "MES_MRASTOLDEF.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
               
        // ----------------------------------------------------------------------------------------------------------------------

        public System.Data.DataTable SELECT_RETICLE_ID(
            string factory, 
            string resId
            )
        {
            try
            {
                return this.GetDataTable("SELECT_RETICLE_ID", null, new string[] { factory, resId });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // ----------------------------------------------------------------------------------------------------------------------
    }
}
