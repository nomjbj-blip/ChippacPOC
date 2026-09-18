using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using Miracom.Middleware;

namespace DACrux.MESIF.DSL
{
    public class MES_MGCMTBLDAT : Miracom.Middleware.QueryComponent
    {
        public MES_MGCMTBLDAT()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["MES_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }
                this.InitQueryComponent(connectID, "MES_MGCMTBLDAT.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectGCMCustomer(string factory)
        {
            try
            {
                return this.GetDataTable("SELECT_GCM_CUSTOMER", null, new string[] { factory});
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectGCMTable(string factory,string table_name)
        {
            try
            {
                return this.GetDataTable("SELECT_GCM_TABLE_01", null, new string[] { factory, table_name });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable SelectGCM_Key1(string factory, string table_name, string KEY_1)
        {
            try
            {
                return this.GetDataTable("SELECT_GCM_KEY_1", null, new string[] { factory, table_name, KEY_1 });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectGCM_Key2(string factory, string table_name, string KEY_1, string KEY_2)
        {
            try
            {
                return this.GetDataTable("SELECT_GCM_KEY_2", null, new string[] { factory, table_name, KEY_1, KEY_2 });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectGCM_Common(string dynamic)
        {
            try
            {
                return this.GetDataTable("SELECT_COMMON", new string[] { dynamic }, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectGCMList(string factory)
        {
            try
            {
                return this.GetDataTable("SELECT_TABLE_LIST", null, new string[] { factory });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectGCMLossList(string factory, string oper)
        {
            try
            {
                if (oper == "ALL")
                    return this.GetDataTable("SELECT_TABLE_LOSS_LIST_01", null, new string[] { factory });
                else
                    return this.GetDataTable("SELECT_TABLE_LOSS_LIST_02", null, new string[] { factory, oper });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectGCMLossDescList(string factory, string oper, string codelist)
        {
            try
            {
                return this.GetDataTable("SELECT_LOSS_CODE_DESC", new string[] { codelist }, new string[] { factory, oper });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectColSetGrp(string factory)
        {
            try
            {
                return this.GetDataTable("SELECT_COL_SET_TYPE", null, new string[] { factory });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectPkgList()
        {
            try
            {
                return this.GetDataTable("SELECT_GCM_CONDITON", null, new string[] { });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectPackageListLot()
        {
            try
            {
                return this.GetDataTable("SELECT_GCM_CONDITON_LOT", null, new string[] { });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectMatGrpList()
        {
            try
            {
                return this.GetDataTable("SELECT_GCM_MATGRP", null, new string[] { });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // ----------------------------------------------------------------------------------------------------------------------
        // RMS 사용
        public System.Data.DataTable GetErrMsg(string factory, string strTableName, string ErrCode)
        {
            try
            {
                return this.GetDataTable("SELECT_ERROR_INFO_01", null, new string[] { factory, strTableName, ErrCode });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        // ----------------------------------------------------------------------------------------------------------------------
    }

}
