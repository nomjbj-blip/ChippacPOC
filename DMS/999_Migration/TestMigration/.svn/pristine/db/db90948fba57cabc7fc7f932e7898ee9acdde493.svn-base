using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Miracom.Middleware;

namespace FabTwoToMigrationTools.DSL
{
    class T_TEST_MIGRATION : Miracom.Middleware.QueryComponent
    {
        public T_TEST_MIGRATION()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "T_AVI_TABLE.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
