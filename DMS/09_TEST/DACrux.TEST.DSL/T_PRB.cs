using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DACrux.TEST.DSL
{
    public class T_PRB : Miracom.Middleware.QueryComponent
    {
        public T_PRB()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "T_PRB.xml");
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

        public DataTable Dynamic(string strQuery)
        {
            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("DYNAMIC_QUERY", new string[] { strQuery }, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertPrb(string strTable
                                , string Wafer_Seq
                                , string DieId
                                , string X
                                , string Y
                                , string Bin
                                , string hBin
                                , string CharBin
                                , string Site
                                , string VisualInsp
                                , string Avi)
        {

            try
            {
                strTable = @"TD_" + strTable.Replace("-", "_").ToString();
                this.Execute("INSERT_PRB"
                    , new string[] { strTable }  // Dynamic query
                    , new string[] { Wafer_Seq
                                        , DieId
                                        , X
                                        , Y
                                        , Bin
                                        , hBin
                                        , CharBin
                                        , Site
                                        , VisualInsp
                                        , Avi});
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CopyPrb(string str1, string str2
                                , string Wafer_Seq, string PreWafer_Seq)
        {
            try
            {
                this.Execute("COPY_PRB"
                    , new string[] { str1, str2 } // Dynamic query
                    , new string[] { Wafer_Seq, PreWafer_Seq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdatePrb(string strTable
                                , string Wafer_Seq, string X, string Y, string Bin, string Hbin, string CharBin)
        {
            try
            {
                this.Execute("UPDATE_PRB"
                    , new string[] { strTable } // Dynamic query
                    , new string[] { Wafer_Seq, X, Y, Bin, Hbin, CharBin });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectXY(string strTable, string Wafer_Seq, string X, string Y)
        {
            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("SELECT_XY"
                                        , new string[] { strTable }
                                        , new string[] { Wafer_Seq, X, Y });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        public void UpdateAvi(string strTable,
                                string Wafer_Seq, string X, string Y, string Bin)
        {
            try
            {
                this.Execute("UPDATE_AVI"
                                , new string[] { strTable } // Dynamic query
                                , new string[] { Wafer_Seq, X, Y, Bin });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectLossDie(string strTable, string Wafer_Seq, string Program, string Product)
        {
            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("SELECT_LOSSDIE"
                                        , new string[] { strTable }
                                        , new string[] { Wafer_Seq, Program, Product });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        public DataTable SelectFailDie(string strTable, string Wafer_Seq, string Program, string Product)
        {
            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("SELECT_FAILDIE"
                                        , new string[] { strTable }
                                        , new string[] { Wafer_Seq, Program, Product });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        public DataTable SelectTestDie(string strTable, string Wafer_Seq)
        {
            DataTable dt = null;

            try
            {
                strTable = @"TD_" + strTable.Replace("-", "_").ToString();
                dt = this.GetDataTable("SELECT_TESTDIE"
                                        , new string[] { strTable }
                                        , new string[] { Wafer_Seq });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        public DataTable SelectTableName(string program)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_TABLENAME", null, new string[] { program });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectInfo(string Table, string WaferSeq)
        {
            try
            {
                return this.GetDataTable("SELECT_INFO", new string[] { Table }, new string[] { WaferSeq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectInfoMergeVI(string Table, string WaferSeq)
        {
            try
            {
                return this.GetDataTable("SELECT_INFO_MERGE_VI", new string[] { Table }, new string[] { WaferSeq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectInfoAVIMergeVI(string LotID, string WaferID, string TestArea)
        {
            try
            {
                return this.GetDataTable("SELECT_INFO_AVI_MERGE_VI", null, new string[] { LotID, WaferID, TestArea });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateTemporaryTable(string strTempTable, string strTable)
        {
            try
            {
                this.Execute("CREATE_TEMP_TABLE", new string[] { strTempTable, strTable }, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CopyTemporaryTableToItem(string strTable, string strTempTable)
        {
            try
            {
                this.Execute("COPY_TEMP_TABLE_TO_ITEM", new string[] { strTable, strTempTable }, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DropTemporaryTable(string strTempTable)
        {
            try
            {
                this.Execute("DROP_TEMP_TABLE", new string[] { strTempTable }, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CopyTemporaryTable(string strTable, string strTempTable)
        {
            try
            {
                this.Execute("COPY_TEMP_TABLE", new string[] { strTable, strTempTable }, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Sum Data 사용---------------------------------------------

        public DataTable SelectBinData(string strTable, string Wafer_Seq)
        {
            DataTable dt = null;

            try
            {
                dt = this.GetDataTable("SELECT_BINDATA"
                                        , new string[] { strTable }
                                        , new string[] { Wafer_Seq });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
            }
        }

        #endregion --------------------------------------------------------

        public void CopyRowDataNotPrb(string ToTable, string FromTable, string WaferSeq, string PreWaferSeq)
        {
            try
            {
                this.Execute("COPY_ROW_DATA_NOT_PRB", new string[] { ToTable, FromTable }, new string[] { WaferSeq, PreWaferSeq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectRowDataWithGec(string WaferSeq)
        {
            try
            {
                return this.GetDataTable("SELECT_ROW_DATA_WITH_GEC", null, new string[] { WaferSeq });
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        public void CopyRowDataForAOI(string ToTable, string FromTable, string WaferSeq, string PreWaferSeq)
        {
            try
            {
                this.Execute("COPY_ROW_DATA_FOR_AOI", new string[] { ToTable, FromTable }, new string[] { WaferSeq, PreWaferSeq });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable SelectCount(string table, string waferSeq)
        {
            try
            {
                return this.GetDataTable("SELECT_VI_COUNT", new string[] { table }, new string[] { waferSeq });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SelectMaxWaferSeq()
        {
            try
            {
                return this.GetDataTable("SELECT_MAX_WAFERSEQ", null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CallProcedure(string testArea, string product, string program, string waferSeq, string strGecBin)
        {
            try
            {
                this.Execute("CALL_PROCEDURE", null, new string[] { testArea, product, program, waferSeq, strGecBin });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void InsertPrb(string strTable, string wafer_seq, string mapid)
        {

            try
            {
                strTable = @"TD_" + strTable.Replace("-", "_").ToString();
                this.Execute("INSERT_PRB_BYUSEMAP", new string[] { strTable }, new string[] { wafer_seq, mapid });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
