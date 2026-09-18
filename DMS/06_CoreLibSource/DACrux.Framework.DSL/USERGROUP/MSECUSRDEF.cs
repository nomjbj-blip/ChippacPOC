using System;
using System.Data;

namespace DACrux.Framework.DSL
{
    public class MSECUSRDEF : Miracom.Middleware.QueryComponent
    {
        #region Class Creator

        /// <summary>
        /// Initialize Class
        /// </summary>
        public MSECUSRDEF()
        {
            string connectID = string.Empty;

            connectID = System.Configuration.ConfigurationManager.AppSettings["MES_CONNECT_ID"];
            if (connectID.Equals(string.Empty))
            {
                throw new Exception("The connect ID nothing. Please, check app.config.");
            }

            this.InitQueryComponent(connectID, "MES_MSECUSRDEF.xml");
        }

        #endregion

        public DataTable GetUserInfo(string Factory, string UserID)
        {
            return this.GetDataTable("SELECT_MES_USER_INFO", null, new string[] { Factory, UserID });
        }
    }
}
