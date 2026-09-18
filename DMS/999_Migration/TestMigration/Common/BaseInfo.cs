using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Oracle.ManagedDataAccess.Client;

namespace FabTwoToMigrationTools
{
    public class BaseInfo : MarshalByRefObject
    {
        string _connectionString;

        protected BaseInfo()
        {
        }

        private void SetFactory()
        {
            string DataSource;
            string UserID;
            string Password;

            if (Factory == "FAB1")
            {
                DataSource = "F1DMSDB";
                UserID = "TPSMGR";
                Password = "tpsm9r0805!";// "tpsm9r0430!";
            }
            else if (Factory == "FAB2")
            {
                DataSource = "F2DMSDB";
                UserID = "TPSMGR";
                Password = "tpsm9r0805!";// "tpsm9r0430!";
            }
            else
            {
                throw new Exception("알수 없는 Factory : " + Factory);
            }

            SetConnectionString(UserID, Password, DataSource);
        }

        protected void SetConnectionString(
            string userid,
            string password,
            string connectionname
            )
        {
            _connectionString = string.Format(
                Constract.ConnectionString,
                userid,
                password,
                connectionname
                );
        }


        //-----------------------------------------------------------------------------------

        protected OracleConnection CreateConnection()
        {
            if (String.IsNullOrEmpty(_connectionString))
                SetFactory();

            return new OracleConnection(_connectionString);
        }
        
        //-----------------------------------------------------------------------------------

        public DataTable ExecuteQuery(
            string sQuery,
            Dictionary<string, Parameter> dicParams = null
            )
        {
            return ExecuteQuery(
                string.Empty,
                sQuery,
                dicParams
                );
        }

        public DataTable ExecuteQuery(
            string sFuncName,
            string sQuery,
            Dictionary<string, Parameter> dicParams
            )
        {
            using (OracleConnection oConnection = CreateConnection())
            {
                using (OracleCommand oCommand = oConnection.CreateCommand())
                {
                    oCommand.CommandText = sQuery;
                    oCommand.CommandType = CommandType.Text;

                    if (dicParams != null)
                    {
                        foreach (KeyValuePair<string, Parameter> pk in dicParams)
                        {
                            OracleParameter oParams = new OracleParameter();
                            Parameter p = pk.Value;
                            oParams.ParameterName = pk.Key;
                            oParams.OracleDbType = GetOracleDbTypeByName(p.Type);
                            oParams.Value = p.Value;

                            oCommand.Parameters.Add(oParams);
                        }
                    }

                    using (OracleDataAdapter oAdapter = new OracleDataAdapter(oCommand))
                    {
                        DataTable dt = new DataTable();
                        oAdapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public object ExecuteScalar(
            string sQuery
            )
        {
            return ExecuteScalar(sQuery, null);
        }

        public object ExecuteScalar(
            string sQuery,
            Dictionary<string, Parameter> dicParams
            )
        {
            OracleParameter oParams = null;
            using (OracleConnection oConnection = CreateConnection())
            {
                using (OracleCommand oCommand = oConnection.CreateCommand())
                {
                    oCommand.CommandText = sQuery;
                    oCommand.CommandType = CommandType.Text;

                    if (dicParams != null)
                    {
                        foreach (KeyValuePair<string, Parameter> pk in dicParams)
                        {
                            oParams = new OracleParameter();
                            Parameter p = pk.Value;
                            oParams.ParameterName = pk.Key;
                            oParams.OracleDbType = GetOracleDbTypeByName(p.Type);
                            oParams.Value = p.Value;

                            oCommand.Parameters.Add(oParams);
                        }
                    }

                    oConnection.Open();
                    return oCommand.ExecuteScalar();
                }
            }
        }

        //-----------------------------------------------------------------------------------

        public int ExecuteNonQuery(
            string sQuery
            )
        {
            using (OracleConnection oConnection = CreateConnection())
            {
                using (OracleCommand oCommand = oConnection.CreateCommand())
                {
                    oCommand.CommandText = sQuery;
                    oCommand.CommandType = System.Data.CommandType.Text;

                    oConnection.Open();
                    return oCommand.ExecuteNonQuery();
                }
            }
        }

        //-----------------------------------------------------------------------------------

        public int ExecuteNonQuery(
            string sQuery,
            Dictionary<string, Parameter> dicParams
            )
        {
            return ExecuteNonQuery(
                string.Empty,
                sQuery,
                dicParams
                );
        }

        public int ExecuteNonQuery(
            string sFunctionName,
            string sQuery,
            Dictionary<string, Parameter> dicParams
            )
        {
            using (OracleConnection oConnection = CreateConnection())
            {
                using (OracleCommand oCommand = oConnection.CreateCommand())
                {
                    oCommand.CommandText = sQuery;
                    oCommand.CommandType = System.Data.CommandType.Text;

                    foreach (KeyValuePair<string, Parameter> p in dicParams)
                    {
                        OracleParameter oParams = new OracleParameter();
                        Parameter dbp = p.Value;
                        oParams.OracleDbType = GetOracleDbTypeByName(dbp.Type);
                        oParams.Value = dbp.Value;
                        oParams.ParameterName = p.Key;
                        oCommand.Parameters.Add(oParams);
                    }

                    oConnection.Open();
                    return oCommand.ExecuteNonQuery();
                }
            }
        }

        //-----------------------------------------------------------------------------------
        public int ExecuteNonQuery(
            string sQuery,
            int iBindCnt,
            Dictionary<string, DataTableParameters> dicParams
            )
        {
            return ExecuteNonQuery(
                string.Empty,
                sQuery,
                iBindCnt,
                dicParams
                );
        }

        public int ExecuteNonQuery(
            string sFunctionName,
            string sQuery,
            int iBindCnt,
            Dictionary<string, DataTableParameters> dicParams
            )
        {
            using (OracleConnection oConnection = CreateConnection())
            {
                using (OracleCommand oCommand = oConnection.CreateCommand())
                {
                    oCommand.CommandText = sQuery;
                    oCommand.CommandType = System.Data.CommandType.Text;
                    //oCommand.ArrayBindCount = iBindCnt;

                    foreach (KeyValuePair<string, DataTableParameters> p in dicParams)
                    {
                        OracleParameter oParams = new OracleParameter();
                        DataTableParameters dbp = p.Value;
                        oParams.Value = dbp.Values.ToArray();
                        oParams.ParameterName = p.Key;
                        oCommand.Parameters.Add(oParams);
                    }

                    oConnection.Open();
                    return oCommand.ExecuteNonQuery();
                }
            }
        }

        //-----------------------------------------------------------------------------------

        public DbType GetDbTypeByName(
            Type type
            )
        {
            SqlParameter sqlParam = new SqlParameter();
            TypeConverter tc = TypeDescriptor.GetConverter(sqlParam.DbType);
            if (tc.CanConvertFrom(type))
            {
                sqlParam.DbType = (DbType)tc.ConvertFrom(type.Name);
            }
            else
            {
                sqlParam.DbType = (DbType)tc.ConvertFrom(type.Name);
            }

            return sqlParam.DbType;
        }

        public OracleDbType GetOracleDbTypeByName(
            DbType dbType
            )
        {
            OracleParameter oraParamConvert = new OracleParameter();
            oraParamConvert.DbType = dbType;
            return oraParamConvert.OracleDbType;
        }

        public void WriteQuery(
            string sInfo
            )
        {
#if DEBUG
            Debug.WriteLine(string.Format("Query: {0}", sInfo));
#endif
        }

        public static string Factory
        {
            get;
            set;
        }
    }
}
