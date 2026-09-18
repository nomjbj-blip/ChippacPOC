
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Globalization;

namespace DACrux.SEMDMS.DSL
{
    /// <summary>
    /// TQD_IMAGES에 대한 요약 설명입니다.
    /// </summary>


    public class TQD_IMAGES : Miracom.Middleware.QueryComponent
    {

        public TQD_IMAGES()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["DMS_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQD_IMAGES.xml");
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

        public int InsertDataNonQuery(string sqlName, string[,] paras)
        {
            return this.ExecuteMultiple(
                sqlName,
                paras
                );
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

        public void CreateImages(string[] ImageInfo)
        {
            try
            {
                this.Execute("CREATE_IMAGE", null, ImageInfo);
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        public int CreateImages(
            string[,] imageParams
            )
        {
            return this.ExecuteMultiple(
                "CREATE_IMAGES",
                imageParams
                );
        }

        public DataTable GetImageCount(string[] ImageInfo)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("GET_IMAGE_COUNT", null, ImageInfo);
            }
            catch (Exception ex)
            {
                //예외처리
                throw this.ProcessErr(ex);
            }

            return dt;
        }

        public DataTable GetDefectImage(long step_seq, int[] defectids)
        {
            DataTable dt = null;
            try
            {
                if (defectids == null || defectids.Length == 0)
                {
                    dt = this.GetDataTable("GET_DEFECT_IMAGE_OF_STEP"
                        , null
                        , new string[] { step_seq.ToString() });
                }
                else
                {
                    dt = this.GetDataTable("SELECT_DEFECT_INFO"
                                          , new string[] { Miracom.Middleware.Helper.ConvertList(defectids, false) }
                                          , new string[] { step_seq.ToString() });

                    //dt = this.GetDataTable("GET_DEFECT_IMAGE_OF_STEP_DEFECTID"
                    //                        , new string[] { Miracom.Middleware.Helper.ConvertList(defectids, false) }
                    //                        , new string[] { step_seq.ToString() });
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDefectImageInfo(long step_seq)
        {
            DataTable dt = null;
            try
            {
                dt = this.GetDataTable("SELECT_IMAGE_LIST_OF_STEP", null, new string[] { step_seq.ToString() });
                return dt;
            }
            catch (Exception ex)
            {
                //예외처리
                throw this.ProcessErr(ex);
            }
        }

        public DataTable GetImagePath(
            long stepsseq,
            long waferseq,
            int defectid,
            int imageid
            )
        {
            return GetDataTable(
                "GET_IMAGE_PATH",
                null,
                new string[] { stepsseq.ToString(), waferseq.ToString(), defectid.ToString(), imageid.ToString() }
                );

        }

        public int DeleteDefectImage(
            long stepseq, 
            long waferseq
            )
        {
            return ExecuteNonQuery(
                "DELETE_IMAGES",
                null,
                new string[] { stepseq.ToString(), waferseq.ToString() }
                );
        }

        public void UpdateDefectSeq(string strNewStepSeq, string strNewWaferSeq, string strOriStepSeq, string strOriWaferSeq)
        {
            try
            {
                this.Execute("UPDATE_MAINT_SEQ", null, new string[] { strNewStepSeq, strNewWaferSeq, strOriStepSeq, strOriWaferSeq });
            }
            catch (Exception ex)
            {
                throw this.ProcessErr(ex);
            }
        }

        public DataTable GetDefectImagePath01(long[] stepSeqArr)
        {
            return GetDataTable("GET_IMAGE_PATH01", new string[] { String.Join(",", stepSeqArr) }, null);
        }

        public int DeleteDefectImage(string[,] Params)
        {
            return ExecuteMultiple("DELETE_DEFECT_IMAGES", Params);
        }

        public DataTable GetDefectImagePath02(string stepSeq)
        {
            return GetDataTable("GET_IMAGE_PATH02", null, new string[] { stepSeq });
        }
    }
}
