using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using Miracom.Middleware;

namespace DACrux.MESIF.DSL
{
    public class MES_CQCMABFAIL : Miracom.Middleware.QueryComponent
    {
        public MES_CQCMABFAIL()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["MES_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }
                this.InitQueryComponent(connectID, "MES_CQCMABFAIL.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MES_CQCMABFAIL(string connectID)
        {
            try
            {
                this.InitQueryComponent(connectID, "MES_CQCMABFAIL.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// Appand Query ID's Function
        public int CreateQcmabfail(long abn_seq, string abn_code, string priority, string mat_id, string lot_id, string cassette_id, string oper, string res_id, string user_id)
        {
            try
            {
                return this.ExecuteNonQuery("CREATE_QCMABFAIL", null, new string[] {abn_seq.ToString(), abn_code, priority, mat_id, lot_id, cassette_id, oper, res_id, user_id });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectQcmabfail()
        {
            try
            {
                return this.GetDataTable("SELECT_QCMABFAIL", null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateQcmabfail()
        {
            try
            {
                return this.ExecuteNonQuery("UPDATE_QCMABFAIL", null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteQcmabfail()
        {
            try
            {
                return this.ExecuteNonQuery("DELETE_QCMABFAIL", null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    } //EndOfClass
} //EndOfNamespace