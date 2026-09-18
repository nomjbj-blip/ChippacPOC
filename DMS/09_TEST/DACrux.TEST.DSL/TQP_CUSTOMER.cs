using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Miracom.Middleware;
using System.Data;

namespace DACrux.TEST.DSL
{
    public class TQP_CUSTOMER : Miracom.Middleware.QueryComponent
    {
        public TQP_CUSTOMER()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQP_CUSTOMER.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region [Common Select]
        public DataTable GetData(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                return this.GetDataTable(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [Common Insert]
        public void InsertData(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                this.GetDataTable(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertDataNonQuery(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                return this.ExecuteNonQuery(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertExecuteMultiple(string sqlName, string[,] paras)
        {
            try
            {
                this.ExecuteMultiple(sqlName, null, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [Common Update]
        public void UpdateData(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                this.GetDataTable(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateDataNonQuery(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                return this.ExecuteNonQuery(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [Common Delete]
        public void DeleteData(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                this.GetDataTable(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteDataNonQuery(string sqlName, string[] dynamic, string[] paras)
        {
            try
            {
                return this.ExecuteNonQuery(sqlName, dynamic, paras);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public DataTable GetCustomerList()
		{
            DataTable dt = null;
			try
			{
				dt = this.GetDataTable("GET_CUSTLIST",null,null);
				return dt;
			}
			catch(Exception ex)
			{
				throw ex;
			}

		}

		public void CreateCustomer(string CustomerID
			,string Customer
            ,string CustomerDesc
			,bool isDataService
			,int UserCount
			,string ExpireDate
			,string FtpSite
			,string FtpUser
			,string FtpPassword
			,string FtpPath
			,string SendTime
			,string AVIFormat
			,string EDSFormat
			,string InklessFormat
			,string RawDataFormat
            ,string CustCode
            ,string SprName
            ,string MapRcvDir
            ,string BackupDir
            ,string ErrorDir
            ,string LogDir)
		{
			try
			{
				this.Execute("CREATE_CUSTOMER",null,new string[] {CustomerID
																	,Customer
																	,CustomerDesc
																	,isDataService?"1":"0"
																	,UserCount.ToString()
																	,ExpireDate.ToString()
																	,FtpSite
																	,FtpUser
																	,FtpPassword
																	,FtpPath
																    ,SendTime
																	,AVIFormat
																	,EDSFormat
																	,InklessFormat
																	,RawDataFormat
                                                                    ,CustCode
                                                                    ,SprName
                                                                    ,MapRcvDir
                                                                    ,BackupDir
                                                                    ,ErrorDir
                                                                    ,LogDir
																	});


			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public void DeleteCustomer(string CustomerID)
		{
			try
			{
				this.Execute("DELETE_CUSTOMER",null,new string[] {CustomerID});
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

        public void UpdateCustomer(
            string Customer
            , string CustomerDesc
            , bool isDataService
            , int UserCount
            , string ExpireDate
            , string FtpSite
            , string FtpUser
            , string FtpPassword
            , string FtpPath
            , string SendTime
            , string AVIFormat
            , string EDSFormat
            , string InklessFormat
            , string RawDataFormat
            , string CustCode
            , string SprName
            , string MapRcvDir
            , string BackupDir
            , string ErrorDir
            , string LogDir
            , string CustomerID)
        {
            try
            {
                this.Execute("UPDATE_CUSTOMER", null, new string[] {Customer
																	,CustomerDesc
																	,isDataService?"1":"0"
																	,UserCount.ToString()
																	,ExpireDate.ToString()
																	,FtpSite
																	,FtpUser
																	,FtpPassword
																	,FtpPath
																	,SendTime
																	,AVIFormat
																	,EDSFormat
																	,InklessFormat
																	,RawDataFormat
                                                                    ,CustCode
                                                                    ,SprName
                                                                    ,MapRcvDir
                                                                    ,BackupDir
                                                                    ,ErrorDir
                                                                    ,LogDir
																	,CustomerID});
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetCustomerList(string map_dir)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_CUSTLIST_BYMAPDIR", null, new string[] { map_dir });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
