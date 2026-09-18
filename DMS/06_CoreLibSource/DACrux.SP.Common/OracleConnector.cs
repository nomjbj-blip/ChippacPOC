using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using System.Windows.Forms;

using DACrux.SP.Common;

namespace DACrux.SP.Common
{
    public class OracleConnector
    {
        #region " Member Field "

        ConnectionInformation connInfo = null;
        OracleConnection connection = null;

        #endregion

        #region " Creator "

        public OracleConnector(ConnectionInformation connInfo)
        {
            if (connInfo == null)
            {
                Utility.ShowMessageBox("DataBase Connection Information is not specified.", MessageBoxIcon.Information);
                return;
            }

            this.connInfo = connInfo;
            this.connection = GetOracleConnection(((connInfo.HostType == HostType.IP) ? connInfo.HostIP : connInfo.HostName),
                connInfo.Port,
                connInfo.DatabaseName,
                connInfo.UserID,
                connInfo.Password);
        }

        #endregion

        #region " Method "

        #region [ Static ]

        private static OracleConnection GetOracleConnection(string host, string port, string database, string id, string password)
        {
            try
            {
                OracleConnectionStringBuilder sb = new OracleConnectionStringBuilder();

                sb.DataSource = string.Format("{0}:{1}/{2}", host.Replace("\"", "\"\""), port, database.Replace("\"", "\"\""));
                sb.UserID = id.Replace("\"", "\"\"");
                sb.Password = password.Replace("\"", "\"\"");

                return new OracleConnection(sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static OracleConnection GetOracleConnection(ConnectionInformation connInfo)
        {
            try
            {
                return GetOracleConnection(
                    ((connInfo.HostType == HostType.IP) ? connInfo.HostIP : connInfo.HostName),
                    connInfo.Port,
                    connInfo.DatabaseName,
                    connInfo.UserID,
                    connInfo.Password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetDataTable(string host, string port, string database, string id, string password, string query)
        {
            OracleConnection oc = null;
            try
            {
                oc = GetOracleConnection(host, port, database, id, password);

                return GetDataTable(oc, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oc != null)
                    oc.Close();
            }
        }

        public static DataTable GetDataTable(ConnectionInformation connInfo, string query)
        {
            OracleConnection oc = null;
            try
            {
                oc = GetOracleConnection(
                    ((connInfo.HostType == HostType.IP) ? connInfo.HostIP : connInfo.HostName),
                    connInfo.Port,
                    connInfo.DatabaseName,
                    connInfo.UserID,
                    connInfo.Password);

                return GetDataTable(oc, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oc != null)
                    oc.Close();
            }
        }

        public static DataTable GetDataTable(ConnectionInformation connInfo, string query, params OracleParameter[] parameters)
        {
            OracleConnection oc = null;
            try
            {
                oc = GetOracleConnection(
                    ((connInfo.HostType == HostType.IP) ? connInfo.HostIP : connInfo.HostName),
                    connInfo.Port,
                    connInfo.DatabaseName,
                    connInfo.UserID,
                    connInfo.Password);

                return GetDataTable(oc, query, parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oc != null)
                    oc.Close();
            }
        }

        private static DataTable GetDataTable(OracleConnection connection, string query, params OracleParameter[] parameters)
        {
            OracleCommand oc = null;
            DataTable dt = new DataTable();

            try
            {
                if (connection == null && string.IsNullOrEmpty(query))
                    return null;

                oc = new OracleCommand(query, connection);

                if (parameters != null && parameters.Length > 0)
                    oc.Parameters.AddRange(parameters);

                using (OracleDataAdapter oda = new OracleDataAdapter(oc))
                {
                    oda.Fill(dt);
                }

                return dt;
            }
            catch (System.Threading.ThreadAbortException te)
            {
                MessageBox.Show(te.Message);
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        private static DataSet GetDataSet(OracleConnection connection, string query, params OracleParameter[] parameters)
        {
            OracleCommand oc = null;
            DataSet ds = new DataSet();

            try
            {
                if (connection == null && string.IsNullOrEmpty(query))
                    return null;

                oc = new OracleCommand(query, connection);

                if (parameters != null && parameters.Length > 0)
                    oc.Parameters.AddRange(parameters);

                using (OracleDataAdapter oda = new OracleDataAdapter(oc))
                {
                    oda.Fill(ds);
                }

                return ds;
            }
            catch (System.Threading.ThreadAbortException te)
            {
                MessageBox.Show(te.Message);
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public static int Excute(OracleConnection connection, string query, params OracleParameter[] parameters)
        {
            OracleCommand oc = null;

            try
            {
                if (connection == null && string.IsNullOrEmpty(query))
                    return -1;

                oc = new OracleCommand(query, connection);

                if (parameters != null && parameters.Length > 0)
                {
                    oc.Parameters.AddRange(parameters);
                    //oc.CommandType = CommandType.StoredProcedure;
                }

                connection.Open();
                
                return oc.ExecuteNonQuery();
            }
            catch (System.Threading.ThreadAbortException te)
            {
                MessageBox.Show(te.Message);
                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        #endregion

        #region [ General ]

        public DataTable GetDataTable(string query)
        {
            try
            {
                return GetDataTable(connection, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDataTable(string query, params OracleParameter[] parameters)
        {
            try
            {
                return GetDataTable(connection, query, parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Excute(string query, params OracleParameter[] parameters)
        {
            try
            {
                return Excute(connection, query, parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ Pre-Defined ]

        public DataTable GetTableList()
        {
            if (connection == null)
                return null;

            try
            {
                OracleParameter userID = new OracleParameter("userID", OracleType.VarChar, 30, "OWNER");
                //userID.Value = connInfo.UserID;

                return GetDataTable(connection,
                          @"SELECT TABLE_NAME
                                 , COLUMN_NAME
                                 , DATA_TYPE
                                 , DATA_LENGTH
                                 , NULLABLE
                                 , COLUMN_ID
                                 , DATA_DEFAULT
                                 , (SELECT OBJECT_TYPE FROM USER_OBJECTS WHERE OBJECT_NAME = TABLE_NAME) TABLE_TYPE      
                              FROM DBA_TAB_COLUMNS 
                             WHERE OWNER = :userID 
                             ORDER BY TABLE_NAME, COLUMN_ID"
                            , userID);
            }
            catch (System.Threading.ThreadAbortException)
            {
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public DataTable GetTableSheme(string tableName)
        {
            if (connection == null)
                return null;

            try
            {
                return GetDataTable(connection, "SELECT * FROM " + tableName + " WHERE 1=2");
            }
            catch (System.Threading.ThreadAbortException)
            {
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        #endregion

        #endregion
    }
}
