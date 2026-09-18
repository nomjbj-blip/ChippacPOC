using System;

namespace DACrux.MESIF.DSL
{
    public class MES_MRASRESMFO : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public MES_MRASRESMFO()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["MES_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "MES_MRASRESMFO.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
               
        // ----------------------------------------------------------------------------------------------------------------------

        public System.Data.DataTable SELECT_MFO_RESOURCE(
            string factory, 
            string oper,
            string res_id
            )
        {
            try
            {
                return this.GetDataTable("SELECT_MFO_RESOURCE", null, new string[] { factory, oper, res_id });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // ----------------------------------------------------------------------------------------------------------------------
    }
}
