using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using Miracom.Middleware;

namespace DACrux.TEST.DSL
{
    public class T_PRB_TABLE : Miracom.Middleware.QueryComponent
    {
        public T_PRB_TABLE()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "T_PRB_TABLE.xml");
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

        /// <summary>
        /// 해당하는 WaferSeq의 Map의 전체 Column을 조회한다.
        /// 특정 Column만 조회하는 속도보다는 느리다
        /// </summary>
        /// <param name="WaferSeq">WaferSeq</param>
        /// <returns>WAFER_SEQ,DIEID,XY,BIN,HBIN,CBIN,SITE,IQC,AVI,OQC,FVI</returns>
        public DataTable GetMapData(string Program, long WaferSeq)
        {
            try
            {
                return this.GetDataTable("GET_MAPDATA_01", new string[] { Program }, new string[] { WaferSeq.ToString() });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 해당하는 WaferSeq의 Map의 주어진 Column을 조회한다.
        /// 특정 Column만 조회하여 속도는 비교적 빠르다
        /// </summary>
        /// <param name="Program">Test Program Name</param>
        /// <param name="Columns">조회하고자 하는 Column Name</param>
        /// <param name="WaferSeq">WaferSeq</param>
        /// <returns>WAFER_SEQ,DIEID,XY,[BIN,HBIN,CBIN,SITE,IQC,AVI,OQC,FVI]</returns>
        public DataTable GetMapData(string Program, string Columns, long WaferSeq)
        {
            try
            {
                return this.GetDataTable("GET_MAPDATA_02", new string[] { Columns, Program }, new string[] { WaferSeq.ToString() });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
