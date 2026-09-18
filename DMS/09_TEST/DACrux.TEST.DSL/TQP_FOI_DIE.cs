using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Miracom.Middleware;

namespace DACrux.TEST.DSL
{
    public class TQP_FOI_DIE : Miracom.Middleware.QueryComponent
    {
        #region 생성자

        public TQP_FOI_DIE()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQP_FOI_DIE.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        #endregion

        public void CreateFOIDiesMulti(string[,] Paras)
        {
            this.ExecuteMultiple("INSERT_TQP_FOI_DIE", null, Paras);
        }

        public DataTable SelectFOIBinList(string strWaferSeq)
        {
            return this.GetDataTable("SELECT_FOI_BIN_SUM", null, new string[] { strWaferSeq });
        }

        public DataTable SelectFOIMap(string strWaferSeq, string strProbeCnt)
        {
            return this.GetDataTable("SELECT_FOI_MAP", null, new string[] { strWaferSeq, strProbeCnt });
        }

        public DataTable SelectFOIBinSummary(string strWaferSeq)
        {
            return this.GetDataTable("SELECT_AVI_BIN_SUMMARY", null, new string[] { strWaferSeq });
        }

        public DataTable SelectFOIBinRaw(string strWaferSeq)
        {
            return this.GetDataTable("SELECT_WAFER_RAW", null, new string[] { strWaferSeq });
        }

        public void DeleteFOIDiesMulti(string strWaferSeq)
        {
            this.Execute("DELETE_TQP_FOI_DIE", null, new string[] { strWaferSeq });
        }

        public DataTable SelectScopeRawData(string strLotID, string strWaferID)
        {
            return this.GetDataTable("SELECT_SCOPE_RAW", null, new string[] { strLotID, strWaferID });
        }

        public DataTable SelectFOIDataMulti(string[] strWaferSeq)
        {
            string strSequences = string.Format("'{0}'", string.Join("','", strWaferSeq));
            return this.GetDataTable("SELECT_FOI_MAP_MULTI", new string[] { strSequences }, null);
        }

        public DataTable SelectFOIDataMultiImage(string[] strWaferSeq)
        {
            string strSequences = string.Format("'{0}'", string.Join("','", strWaferSeq));
            return this.GetDataTable("SELECT_FOI_MAP_MULTI_IMAGE", new string[] { strSequences }, null);
        }

        public DataTable SelectFOIDataMultiIRaw(string[] strWaferSeq)
        {
            string strSequences = string.Format("'{0}'", string.Join("','", strWaferSeq));
            return this.GetDataTable("SELECT_FOI_MAP_MULTI_RAW", new string[] { strSequences }, null);
        }

        public void UpdateAVIBinValue(string strBin, string strWaferseq, string strDieNum)
        {
            try
            {
                this.Execute("UPDATE_TQP_FOI_DIE_BIN", null, new string[] { strBin, strWaferseq, strDieNum });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
