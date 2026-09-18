using System;
using System.Data;

namespace DACrux.RPNet.DSL
{
    public class PROGRAM_SETUP_REV : Miracom.Middleware.QueryComponent
    {
        public PROGRAM_SETUP_REV()
        {
            string connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];

            if (String.IsNullOrEmpty(connectID))
            {
                throw new Exception("The connect ID nothing. Please, check app.config.");
            }

            this.InitQueryComponent(connectID, "RPNet.PROGRAM_SETUP_REV.xml");
        }

        public DataTable GetProgramSetup(string Program, string lotSeq)
        {
            DataTable dataTable = (DataTable)null;
            try
            {
                dataTable = this.GetDataTable("GET_PGM_SETUP", (string[])null, new string[]
                {
                  Program,
                  lotSeq
                });
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dataTable != null)
                    dataTable.Dispose();
            }
        }
    }
}
