using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using Miracom.Middleware;

namespace DACrux.MESIF.DSL
{
    public class MES_CQCMABNMNG : Miracom.Middleware.QueryComponent
    {
        public MES_CQCMABNMNG()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["MES_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }
                this.InitQueryComponent(connectID, "MES_CQCMABNMNG.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MES_CQCMABNMNG(string connectID)
        {
            try
            {
                this.InitQueryComponent(connectID, "MES_CQCMABNMNG.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    
/// Appand Query ID's Function
        public void CreateAbn(string abn_code, string sample_count, string lot_id, string mat_id, string customer_lot_id, string customer, string customer_device, string bump_type, string cassette_id, string oper, string factory, string flow, string res_id, string user_id, string department)
        {
            try
            {
                this.Execute("CREATE_ABN", null, new string[] { abn_code, sample_count, lot_id, mat_id, customer_lot_id, customer, customer_device, bump_type, cassette_id, oper, factory, flow, res_id, user_id, department });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectAbn(string abn_seq)
        {
            try
            {
                return this.GetDataTable("SELECT_ABN", null, new string[] { abn_seq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectAbnToDayList(string strABNType)
        {
            try
            {
                return this.GetDataTable("SELECT_ABN_TODAY_LIST_01", null, new string[] { strABNType });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectAbnByCode(string strABN_CODE)
        {
            try
            {
                return this.GetDataTable("SELECT_ABN_BY_CODE_01", null, new string[] { strABN_CODE });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateAbn(string abn_seq, string abn_type, string abn_code, string abn_status, string fail_count, string sample_count, string loss_count, string rework_count, string good_count, string reject_count, string return_count, string create_time, string tran_time, string lot_id, string mat_id, string customer_lot_id, string customer, string customer_device, string bump_type, string cassette_id, string oper, string factory, string flow, string res_id, string user_id, string department, string delete_flag)
        {
            try
            {
                return this.ExecuteNonQuery("UPDATE_ABN", null, new string[] { abn_seq, abn_type, abn_code, abn_status, fail_count, sample_count, loss_count, rework_count, good_count, reject_count, return_count, create_time, tran_time, lot_id, mat_id, customer_lot_id, customer, customer_device, bump_type, cassette_id, oper, factory, flow, res_id, user_id, department, delete_flag });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteAbn(string abn_seq)
        {
            try
            {
                return this.ExecuteNonQuery("DELETE_ABN", null, new string[] { abn_seq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    } //EndOfClass
} //EndOfNamespace