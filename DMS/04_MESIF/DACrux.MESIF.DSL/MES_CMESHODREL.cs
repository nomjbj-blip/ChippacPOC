using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using Miracom.Middleware;

namespace DACrux.MESIF.DSL
{
    public class MES_CMESHODREL : Miracom.Middleware.QueryComponent
    {
        public MES_CMESHODREL()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["MES_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }
                this.InitQueryComponent(connectID, "TQS_CMESHODREL.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertCMESHODREL(string factory, string lot_id, string res_id, string tran_gbn, string cmf_1, string cmf_2, string cmf_3, string cmf_4, string cmf_5, string create_time, string create_user_id)
        {
            try
            {
                this.Execute("CREATE_CMESHODREL", null, new string[] { factory, lot_id, res_id, tran_gbn, cmf_1, cmf_2, cmf_3, cmf_4, cmf_5, create_time, create_user_id });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable selectCMESHODREL(string factory, string lot_id, string res_id, string tran_gbn)
        {
            try
            {
                return this.GetDataTable("SELECT_CMESHODREL", null, new string[] { factory, lot_id, res_id, tran_gbn });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void deleteCMESHODREL(string factory, string lot_id, string res_id, string tran_gbn)
        {
            try
            {
                this.Execute("DELETE_CMESHODREL", null, new string[] { factory, lot_id, res_id, tran_gbn });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
