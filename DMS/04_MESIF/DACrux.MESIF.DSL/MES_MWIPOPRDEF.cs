using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using Miracom.Middleware;

namespace DACrux.MESIF.DSL
{
    public class MES_MWIPOPRDEF : Miracom.Middleware.QueryComponent
    {
        public MES_MWIPOPRDEF()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["MES_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }
                this.InitQueryComponent(connectID, "MES_MWIPOPRDEF.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectOperation(string factory, string oper)
        {
            return this.GetDataTable("SELECT_OPERATION", null, new string[] { factory, oper});
        }

        public DataTable SelectOper(string factory)
        {
            return this.GetDataTable("SELECT_OPER", null, new string[] { factory });
        }

        public DataTable SelectOper(
            string factory,
            string matId
            )
        {
            return this.GetDataTable("SELECT_OPER_BY_MATID", null, new string[] { factory, matId });
        }

        public DataTable SelectOperList(
            string factory
            )
        {
            return this.GetDataTable("SELECT_OPER_LIST", null, new string[] { factory });
        }

        public DataTable SelectOperByGroup(
            string factory, 
            string strOperGroup
            )
        {
            try
            {
                return this.GetDataTable("SELECT_OPER_BY_GROUP", new string[] { strOperGroup, strOperGroup }, new string[]{ factory });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetOperList(string factory, string sSoper, string sEoper)
        {
            return this.GetDataTable("SELECT_OPER_COND", null, new string[] { factory, sSoper, sEoper });
        }

        public System.Data.DataTable SelectOperGroupByOper(
           string factory,
           string oper
           )
        {
            try
            {
                return this.GetDataTable("SELECT_OPER_GROUP_BY_OPER", null, new string[] { factory, oper });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }

}
