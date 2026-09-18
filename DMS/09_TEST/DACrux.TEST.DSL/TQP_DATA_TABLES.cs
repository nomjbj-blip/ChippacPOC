using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Miracom.Middleware;

namespace DACrux.TEST.DSL
{
    public class TQP_DATA_TABLES : Miracom.Middleware.QueryComponent
    {
        public TQP_DATA_TABLES()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQP_DATA_TABLES.xml");
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

        public string[] GetTableNames(string programName)
        {
            DataTable dt = GetDataTable("SELECT_TABLE_NAMES_01", null, new string[] { programName });

            if (dt == null || dt.Rows.Count == 0)
                return null;

            string[] arr = new string[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
                arr[i] = dt.Rows[i][0].ToString();

            return arr;
        }

        public string[] GetTableNames(string factory, string programName)
        {
            DataTable dt = GetDataTable("SELECT_TABLE_NAMES", null, new string[] { factory, programName });

            if (dt == null || dt.Rows.Count == 0)
                return null;

            string[] arr = new string[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
                arr[i] = dt.Rows[i][0].ToString();

            return arr;
        }

        public void InsertData(string facotry, string program, string tableName)
        {
            ExecuteNonQuery("INSERT_DATA", null, new string[] { facotry, program, tableName });
        }

        public DataTable SelectProgramAndTable(string waferSeq)
        {
            return GetDataTable("SELECT_PROGRAM_AND_TABLE", null, new string[] { waferSeq });
        }

        /// <summary>
        /// wafer seq로 테이블 목록을 가져옵니다.
        /// </summary>
        public string[] GetTableNameByWafer(string waferSeq)
        {
            DataTable dt = GetDataTable("SELECT_TABLE_BY_WAFER", null, new string[] { waferSeq });

            if (dt == null || dt.Rows.Count == 0)
                return null;

            string[] arr = new string[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
                arr[i] = dt.Rows[i][0].ToString();

            return arr;
        }

        /// <summary>
        /// lot seq로 테이블 목록을 가져옵니다.
        /// </summary>
        public string[] GetTableNameByLot(string lotSeq, string program)
        {
            DataTable dt = GetDataTable("SELECT_TABLE_BY_LOT", null, new string[] { lotSeq, program });

            if (dt == null || dt.Rows.Count == 0)
                return null;

            string[] arr = new string[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
                arr[i] = dt.Rows[i][0].ToString();

            return arr;
        }

        /// <summary>
        /// 해당 테이블명을 가진 데이터를 삭제합니다.
        /// </summary>
        public void DeleteDataByTableName(string tableName)
        {
            ExecuteNonQuery("DELETE_DATA_BY_TABLE", null, new string[] { tableName });
        }
    }
}
