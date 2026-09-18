using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using Miracom.Middleware;

namespace DACrux.TEST.DSL
{
    public class T_PCM : Miracom.Middleware.QueryComponent
    {
        public T_PCM()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "T_PCM.xml");
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

        public DataTable GetPCMData(string[] strParams, string program, int[] waferSeq)
        {
            DataTable dt = null;
            try
            {
                string[] strWaferSeq = new string[waferSeq.Length];
                for (int a = 0; a < strWaferSeq.Length; a++) strWaferSeq[a] = string.Format("{0}", waferSeq[a]);
                for (int a = 0; a < strParams.Length; a++) strParams[a] = strParams[a].ToUpper().Replace("/", "_").Replace("+", "_").Replace("-", "_").Replace(".", "_").Replace(" ", "_").ToUpper();

                string[] dynamic = new string[3];
                dynamic[0] = string.Join("\", \"", strParams);
                dynamic[0] = string.Format("\"{0}\"", dynamic[0]);
                dynamic[1] = string.Format("{0}", program);
                dynamic[2] = string.Join(", ", strWaferSeq);
                dt = this.GetDataTable("GET_PCM_DATA", dynamic, null);
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

        public DataTable GetPCMData(
            string tableName,
            string[] waferseq
            )
        {
            return this.GetDataTable(
                "GET_PCM_DATA",
                new string[] { tableName, string.Format("{0}", string.Join(",", waferseq)) },
                null
                );
        }

        public DataTable GetPcmData4Report(string[] item, string program, int[] waferSeq)
        {
            DataTable dt = null;
            try
            {
                string[] strWaferSeq = new string[waferSeq.Length];
                for (int a = 0; a < strWaferSeq.Length; a++) strWaferSeq[a] = string.Format("{0}", waferSeq[a]);
                for (int a = 0; a < item.Length; a++) item[a] = item[a].Replace("/", "_").Replace("+", "_").Replace("-", "_").Replace(".", "_").Replace(" ", "_").ToUpper();

                string[] dynamic = new string[4];
                dynamic[0] = string.Join("\", \"", item);
                dynamic[0] = string.Format("\"{0}\"", dynamic[0]);
                dynamic[1] = dynamic[0];
                dynamic[2] = string.Format("{0}", program);
                dynamic[3] = string.Join(", ", strWaferSeq);
                dt = this.GetDataTable("PCM_REPORT", dynamic, null);
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

        public DataTable GetPcmDataWithYield(string[] orgItem, string program, string[] lots, string itemCal)
        {
            DataTable dt = null;
            try
            {
                string strLots = "'" + String.Join("','", lots) + "'";

                string[] item = new string[orgItem.Length];
                for (int a = 0; a < item.Length; a++) item[a] = orgItem[a].Replace("/", "_").Replace("+", "_").Replace("-", "_").Replace(".", "_").Replace(" ", "_").ToUpper(); ;

                string[] dynamic = new string[5];

                dynamic[0] = string.Join("\", \"", item);
                dynamic[0] = string.Format("\"{0}\"", dynamic[0]);

                for (int a = 0; a < item.Length; a++) item[a] = string.Format("{0}(\"{1}\") \"{2}\"", itemCal, item[a], item[a]);
                dynamic[1] = string.Join(", ", item);
                dynamic[2] = string.Format("{0}", program);
                dynamic[3] = strLots;
                dynamic[4] = strLots;
                dt = this.GetDataTable("YIELD_PCM_DATA", dynamic, null);
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

        public int ZoneCnt(string product, string user, string zone)
        {
            DataTable dt = null;
            try
            {
                string[] para = new string[] { product, user, zone };
                dt = this.GetDataTable("ZONE_CNT", null, para);
                return int.Parse(string.Format("{0}", dt.Rows[0]["CNT"]));
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

        public void ProductProgramByLot(string[] lot, ref string product, ref string testProgram)
        {
            DataTable dt = null;
            try
            {
                string[] dynamic = new string[] { string.Format("'{0}'", string.Join("','", lot)) };
                dt = this.GetDataTable("PRODUCT_PROGRAM_BY_LOT", dynamic, null);
                product = string.Format("{0}", dt.Rows[0]["PRODUCT"]);
                testProgram = string.Format("{0}", dt.Rows[0]["PROGRAM"]);
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

        public DataTable GetPcmDataWithYield(string[] orgItem, string program, string[] lots,
            string user, string zone, int[] bin)
        {
            DataTable dt = null;
            try
            {
                int zoneCnt = 0;
                string product = string.Empty;
                string testProgram = string.Empty;

                ProductProgramByLot(lots, ref product, ref testProgram);
                zoneCnt = ZoneCnt(product, user, zone);

                string strLots = "'" + String.Join("','", lots) + "'";

                string[] item = new string[orgItem.Length];
                for (int a = 0; a < item.Length; a++) item[a] = orgItem[a].Replace("/", "_").Replace("+", "_").Replace("-", "_").Replace(".", "_").Replace(" ", "_").ToUpper();

                string[] dynamic = new string[8];

                dynamic[0] = string.Join("\", \"", item);
                dynamic[0] = string.Format("\"{0}\"", dynamic[0]);
                dynamic[1] = dynamic[0];
                dynamic[2] = string.Format("{0}", program);
                dynamic[3] = strLots;
                dynamic[4] = string.Format("{0}", zoneCnt);
                dynamic[5] = testProgram;
                dynamic[6] = strLots;
                string[] strBin = new string[bin.Length];
                for (int a = 0; a < strBin.Length; a++) strBin[a] = string.Format("{0}", bin[a]);
                dynamic[7] = string.Join(",", strBin);

                string[] para = new string[] { product, user, zone };

                dt = this.GetDataTable("YIELD_PCM_DATA_ZONE", dynamic, para);
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

        public DataTable GetPcmBoxPlot(string program, string item, int[] waferSeq)
        {
            DataTable dt = null;
            try
            {
                string[] strWaferSeq = new string[waferSeq.Length];
                for (int a = 0; a < strWaferSeq.Length; a++) strWaferSeq[a] = string.Format("{0}", waferSeq[a]);

                string[] dynamic = new string[] {
													 string.Format("\"{0}\"", item)
													 , program
													 , string.Join(", ", strWaferSeq)
												 };
                dt = this.GetDataTable("PCM_BOX_PLOT", dynamic, null);
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

        public DataTable GetPcmDataDay(string program, string item, int[] waferSeq)
        {
            DataTable dt = null;
            try
            {
                item = item.Replace("/", "_").Replace("+", "_").Replace("-", "_").Replace(".", "_").Replace(" ", "_").ToUpper();
                string[] strWaferSeq = new string[waferSeq.Length];
                for (int a = 0; a < strWaferSeq.Length; a++) strWaferSeq[a] = string.Format("{0}", waferSeq[a]);

                string[] dynamic = new string[] {
													 string.Format("\"{0}\"", item)
													 , program
													 , string.Join(", ", strWaferSeq)
												 };
                dt = this.GetDataTable("PCM_DATA_DAY", dynamic, null);
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

        public DataTable GetPcmScatter(string program, string item, int[] waferSeq)
        {
            DataTable dt = null;
            try
            {
                string[] strWaferSeq = new string[waferSeq.Length];
                for (int a = 0; a < strWaferSeq.Length; a++) strWaferSeq[a] = string.Format("{0}", waferSeq[a]);

                string[] dynamic = new string[] {
													 string.Format("\"{0}\"", item)
													 , program
													 , string.Join(", ", strWaferSeq)
												 };
                dt = this.GetDataTable("PCM_SCATTER", dynamic, null);
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

        public int GetPcmSiteCnt(string program, int[] waferSeq)
        {
            DataTable dt = null;
            try
            {
                string[] strWaferSeq = new string[waferSeq.Length];
                for (int a = 0; a < strWaferSeq.Length; a++) strWaferSeq[a] = string.Format("{0}", waferSeq[a]);

                string[] dynamic = new string[] {program
                                                     , string.Join(", ", strWaferSeq)
												 };
                dt = this.GetDataTable("PCM_SITE_CNT", dynamic, null);
                int siteCnt = (int)(decimal)dt.Rows[0]["CNT"];
                return siteCnt;
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

        public DataTable GetPcmBoxPlotPivot(string program, string item, int[] waferSeq)
        {
            DataTable orgDt = null;
            DataTable dt = null;
            try
            {
                item = item.Replace("/", "_").Replace("+", "_").Replace("-", "_").Replace(".", "_").Replace(" ", "_").ToUpper();
                orgDt = GetPcmBoxPlot(program, item, waferSeq);

                dt = new DataTable("PCM_DATA_PIVOT");
                for (int a = 0; a < waferSeq.Length; a++)
                {
                    dt.Columns.Add((string)orgDt.Rows[a]["LOT_ID"] + "_" + orgDt.Rows[a]["WAFER_ID"], typeof(decimal));
                    //dt.Columns.Add(a.ToString(), typeof(decimal));
                }

                for (int a = 0; a < orgDt.Rows.Count / waferSeq.Length; a++)
                {
                    object[] obj = new object[waferSeq.Length];
                    for (int b = 0; b < waferSeq.Length; b++)
                    {
                        obj[b] = orgDt.Rows[waferSeq.Length * a + b][item];
                    }

                    dt.Rows.Add(obj);
                }

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                if (orgDt != null) orgDt.Dispose();
            }
        }

        public object PcmValue(DataTable dt, int waferSeq, int dieId, string item)
        {
            DataRow[] dr = dt.Select(string.Format("WAFER_SEQ={0} AND DIEID={1}", waferSeq, dieId));
            if (dr == null) return DBNull.Value;
            else if (dr.Length == 0) return DBNull.Value;
            else return dr[0][item];
        }

        public DataTable GetPcmScatterPivot(string program, string item, int[] waferSeq)
        {
            DataTable orgDt = null;
            DataTable dt = null;
            try
            {
                item = item.Replace("/", "_").Replace("+", "_").Replace("-", "_").Replace(".", "_").Replace(" ", "_").ToUpper();
                orgDt = GetPcmScatter(program, item, waferSeq);

                dt = new DataTable("PCM_DATA_PIVOT");
                dt.Columns.Add("LOT_ID", typeof(string));
                dt.Columns.Add("WAFER_ID", typeof(string));

                int siteCnt = GetPcmSiteCnt(program, waferSeq);

                for (int a = 0; a < siteCnt; a++)
                    dt.Columns.Add(string.Format("{0}", a + 1), typeof(decimal));

                for (int a = 0; a < waferSeq.Length; a++)
                {
                    object[] obj = new object[siteCnt + 2];
                    obj[0] = orgDt.Rows[a]["LOT_ID"];
                    obj[1] = orgDt.Rows[a * siteCnt]["WAFER_ID"];
                    for (int b = 0; b < siteCnt; b++)
                    {
                        obj[b + 2] = PcmValue(orgDt, waferSeq[a], b + 1, item);
                    }

                    dt.Rows.Add(obj);
                }

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                if (orgDt != null) orgDt.Dispose();
            }
        }

        public DataTable GetYieldTrend(string by, string[] waferSeq)
        {
            DataTable dt = null;
            try
            {
                string[] dynamic = new string[] { string.Join(",", waferSeq) };
                string queryId = string.Format("YIELD_TREND_BY_{0}", by);

                dt = this.GetDataTable(queryId, dynamic, null);
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

        public string[] BinList(string[] lotId)
        {
            DataTable dt = null;
            try
            {
                for (int a = 0; a < lotId.Length; a++) lotId[a] = string.Format("'{0}'", lotId[a]);
                string[] dynamic = new string[] {
												  string.Join(",", lotId)
												  , string.Join(",", lotId)};

                dt = this.GetDataTable("BIN_LIST", dynamic, null);

                string[] bin = new string[dt.Rows.Count];
                for (int a = 0; a < bin.Length; a++)
                {
                    bin[a] = string.Format("{0}", dt.Rows[a]["BIN"]);
                }

                return bin;
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

        public DataTable GetMapDef(string MapID)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_MAPDEF", null, new string[] { MapID });
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }

        public DataTable GetMapIDList()
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_MAPIDLIST", null, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Dispose();
                dt = null;
            }
        }
    }
}
