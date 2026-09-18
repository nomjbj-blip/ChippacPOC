using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using Miracom.Middleware;

namespace DACrux.TEST.DSL
{
    public class T_AVI_TABLE : Miracom.Middleware.QueryComponent
    {
        public T_AVI_TABLE()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "T_AVI_TABLE.xml");
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

        public long GetNextLotSeq()
        {
            DataTable dt = null;
            long lNextLotSeq = 0;
            try
            {
                dt = this.GetDataTable("DYNAMIC", new string[] { "SELECT MAX(LOT_SEQ) AS MAX_LSEQ FROM T_TPS_LOT" }, null);
                lNextLotSeq = long.Parse(dt.Rows[0]["MAX_LSEQ"].ToString()) + 1;
                return lNextLotSeq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public long InsertLot(string FACILITY, string TESTAREA, string PRODUCT, string PROGRAM, string LOTID, string FABLOT, string START_TIME, string END_TIME)
        {
            long lLotSeq = 0;
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_LOT_SEQ", null, new string[] { TESTAREA, PRODUCT, PROGRAM, LOTID });

                if (dt != null && dt.Rows.Count > 0) //扁粮俊 乐阑锭
                {
                    lLotSeq = long.Parse(dt.Rows[0]["LOT_SEQ"].ToString());
                }
                else  //扁粮俊 绝阑锭
                {
                    this.Execute("INSERT_LOT_INFO", null, new string[] { PRODUCT, TESTAREA, PROGRAM, LOTID, LOTID, "", FABLOT, "", FACILITY, "", START_TIME, END_TIME, "", "1" });
                    dt = this.GetDataTable("GET_LOT_SEQ", null, new string[] { TESTAREA, PRODUCT, PROGRAM, LOTID });
                    lLotSeq = long.Parse(dt.Rows[0]["LOT_SEQ"].ToString());
                }

                return lLotSeq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long InsertWafer(string LOT_SEQ
                            , string WAFER_ID
                            , string TESTER
                            , string OPERATOR
                            , string TESTED_DIE
                            , string START_TIME
                            , string LOSS_DIE)
        {
            long lWaferSeq = 0;
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_WAFER_SEQ", null, new string[] { LOT_SEQ, WAFER_ID });

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] drs = dt.Select("", "PROBE_CNT DESC");
                    for (int i = 0; i < drs.Length; i++)
                    {
                        this.Execute("UPDATE_PROBE_CNT", null, new string[] { LOT_SEQ, drs[i]["WAFER_SEQ"].ToString() });
                        this.Execute("UPDATE_PROBE_CNT_SUM", null, new string[] { LOT_SEQ, drs[i]["WAFER_SEQ"].ToString() });
                    }
                }

                this.Execute("INSERT_WAFER_INFO", null, new string[] { LOT_SEQ, WAFER_ID, TESTER, TESTER, OPERATOR, TESTED_DIE, START_TIME, LOSS_DIE });

                dt = this.GetDataTable("GET_WAFER_SEQ", null, new string[] { LOT_SEQ, WAFER_ID });
                DataRow[] drs1 = dt.Select("PROBE_CNT=0");
                lWaferSeq = long.Parse(drs1[0]["WAFER_SEQ"].ToString());

                return lWaferSeq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// YSIM 2015-0-16
        /// => 수정
        /// </summary>
        /// <param name="Product">AVI는 Product 별 Table에 저장</param>
        /// <param name="strValue">1개 Data [WAFER_SEQ,DIEID,X,Y,CAT,QC]</param>
        public void InsertFailDie(string Product, string[] strValue)
        {
            try
            {
                this.Execute("INSERT_RAWDATA", new string[] { string.Format("T_AVI_{0}", Product.Replace(".", "_").Replace("-", "_")) }, strValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        /// <summary>
        /// YSIM 2015-0-16
        /// => Override Function 추가 
        ///    여러개의 Data를 한번에 처리하는 함수
        /// </summary>
        /// <param name="Product">AVI는 Product 별 Table에 저장</param>
        /// <param name="strValue">string[,]</param>
        public void InsertFailDie(string Product, string[,] strValue)
        {
            try
            {
                this.ExecuteMultiple("INSERT_RAWDATA", new string[] { string.Format("T_AVI_{0}", Product.Replace(".", "_").Replace("-", "_")) }, strValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        /// <summary>
        /// YSIM 2015-04-16
        /// => 1개씩 Update
        /// </summary>
        /// <param name="Product">AVI는 Product 별 Table에 저장</param>
        /// <param name="strValue">QC, WAFER_SEQ, X, Y</param>
        public void UpdateFailDie(string Product, string[] strValue)
        {
            try
            {
                this.Execute("UPDATE_RAWDATA", new string[] { string.Format("T_AVI_{0}", Product.Replace(".", "_").Replace("-", "_")) }, strValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        /// <summary>
        /// YSIM 2015-04-16
        /// => 여러개 동시에 Update
        /// </summary>
        /// <param name="Product">AVI는 Product 별 Table에 저장</param>
        /// <param name="strValue">QC, WAFER_SEQ, X, Y</param>
        public void UpdateFailDie(string Product, string[,] strValue)
        {
            try
            {
                this.ExecuteMultiple("UPDATE_RAWDATA", new string[] { string.Format("T_AVI_{0}", Product.Replace(".", "_").Replace("-", "_")) }, strValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public DataTable GetMapData(string TESTAREA, string PRODUCT, string PROGRAM, string LOT_ID, string WAFER_ID)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_MAPDATA", new string[] { PRODUCT.Replace('-', '_').ToString() }, new string[] { TESTAREA, PRODUCT, PROGRAM, LOT_ID, WAFER_ID });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAVIMapData(string TESTAREA, string PRODUCT, string LOT_ID, string WAFER_ID)
        {
            DataTable dt = null;
            string tableNameProduct = string.Empty;
            try
            {
                
                if (PRODUCT != null) tableNameProduct = PRODUCT.ToUpper().Replace("-", "_").Replace(" ", "").Replace(".", "_");

                if (TESTAREA == "AVI")
                {
                    dt = this.GetDataTable("GET_AVI_MAPDATA", new string[] { tableNameProduct }, new string[] { TESTAREA, PRODUCT, LOT_ID, WAFER_ID });
                }
                else
                {
                    dt = this.GetDataTable("GET_IQC_MAPDATA", new string[] { tableNameProduct }, new string[] { TESTAREA, PRODUCT, LOT_ID, WAFER_ID });
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAVIMapData(string TESTAREA, string PRODUCT, string LOT_ID, string WAFER_ID, string PROGRAM)
        {
            DataTable dt = null;
            string tableNameProduct = string.Empty;
            try
            {

                tableNameProduct = PROGRAM.ToUpper().Replace("-", "_").Replace(" ", "").Replace(".", "_");

                dt = this.GetDataTable("GET_IQC_MAPDATA_01", new string[] { tableNameProduct }, new string[] { TESTAREA, PRODUCT, LOT_ID, WAFER_ID });
                
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAVIMapData(string TESTAREA, string PRODUCT, long WaferSeq)
        {
            DataTable dt = null;
            string tableNameProduct = string.Empty;
            try
            {

                if (PRODUCT != null) tableNameProduct = PRODUCT.ToUpper().Replace("-", "_").Replace(" ", "").Replace(".", "_");

                if (TESTAREA == "AVI")
                {
                    dt = this.GetDataTable("GET_AVI_MAPDATA(BY_WAFER_SEQ)", new string[] { tableNameProduct }, new string[] {  PRODUCT, WaferSeq.ToString() });
                }
                else
                {
                    dt = this.GetDataTable("GET_IQC_MAPDATA(BY_WAFER_SEQ)", new string[] { tableNameProduct }, new string[] {  PRODUCT, WaferSeq.ToString() });
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetWaferList(string TestArea, string StartTime, string EndTime, string LikeDevice, string LikeLot, string LikeWafer)
        {
            DataTable dt = null;
            string strLike = string.Empty;
            try
            {
                if (LikeDevice.Length > 0)
                {
                    strLike = strLike + string.Format(" AND PRODUCT LIKE ('%{0}%') ", LikeDevice);
                }
                if (LikeLot.Length > 0)
                {
                    strLike = strLike + string.Format(" AND LOT_ID LIKE ('%{0}%') ", LikeLot);
                }
                if (LikeWafer.Length > 0)
                {
                    strLike = strLike + string.Format(" AND WAFER_ID = '{0}' ", LikeWafer);
                }

                dt = this.GetDataTable("GET_WAFER_LIST", new string[] { strLike }, new string[] { TestArea, StartTime, EndTime });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public DataTable GetTestAreaList()
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_TESTAREA_LIST", null, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public void DeleteAVIZero(string PRODUCT, string WAFER_SEQ)
        {
            try
            {
                this.Execute("DELETE_ZERO", new string[] { PRODUCT.Replace("-", "_").Replace(".", "_") }, new string[] { WAFER_SEQ });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RunWaferSummary(string PRODUCT, string WAFER_SEQ)
        {
            try
            {
                this.ExecuteScalar("PROC_RUN_AVI_SUMMAY", null, new string[] { PRODUCT, WAFER_SEQ });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Map Parser DSL ----------------------------------------------------------

        public DataTable SelectBinInfo(string str, string WaferID, string Product, string Program, string LotId)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_BININFO"
                    , new string[] { str }
                    , new string[] { WaferID, Product, Program, LotId });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        #endregion

        public void CreateCleanMap(string Product, long lWaferSeq)
        {
            try
            {
                this.ExecuteNonQuery("INSERT_CLEAN_MAP", new string[] { Product }, new string[] { lWaferSeq.ToString(), Product });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetBinDistribution(string Product, long WaferSeq, string TestArea)
        {
            try
            {
                if (TestArea == "AVI")
                {
                    return this.GetDataTable("GET_BIN_DISTRIBUTION_AVI", new string[] { Product }, new string[] { TestArea, WaferSeq.ToString() });
                }
                else
                {
                    return this.GetDataTable("GET_BIN_DISTRIBUTION_IQC", new string[] { Product }, new string[] { TestArea, WaferSeq.ToString() });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
