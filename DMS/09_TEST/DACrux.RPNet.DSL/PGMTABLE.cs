using System;
using System.Data;

namespace DACrux.RPNet.DSL
{
    public class PGMTABLE : Miracom.Middleware.QueryComponent
    {
        public PGMTABLE()
        {
            string connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];

            if (String.IsNullOrEmpty(connectID))
            {
                throw new Exception("The connect ID nothing. Please, check app.config.");
            }

            this.InitQueryComponent(connectID, "RPNet.PGMTABLE.xml");
        }

        public DataTable GetMapData(string Program, string WaferSeq)
        {
            DataTable dataTable = (DataTable)null;
            try
            {
                dataTable = this.GetDataTable("GET_MAPDATA", new string[1]
        {
          Program
        }, new string[1] { WaferSeq });
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

        public DataTable GetBinDistribution(string Program, string WaferSeq)
        {
            DataTable dataTable = (DataTable)null;
            try
            {
                dataTable = this.GetDataTable("GET_BIN_DISTRIBUTION", new string[1]
        {
          Program
        }, new string[2] { Program, WaferSeq });
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

        public DataTable GetBinDistribution(string Program, string[] WaferSeq)
        {
            DataTable dataTable = (DataTable)null;
            try
            {
                dataTable = this.GetDataTable("GET_BIN_DISTRIBUTION_MULTI_WAFER", new string[2]
        {
          Program,
          string.Join(",", WaferSeq)
        }, new string[1] { Program });
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
