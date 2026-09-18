using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Miracom.Middleware;

namespace DACrux.TEST.DSL
{
    public class TQP_FOI_IMAGES : Miracom.Middleware.QueryComponent
    {
        #region 생성자

        public TQP_FOI_IMAGES()
        {
            string connectID = string.Empty;
            try
            {
                connectID = System.Configuration.ConfigurationManager.AppSettings["TEST_CONNECT_ID"];
                if (connectID.Equals(string.Empty))
                {
                    throw new Exception("The connect ID nothing. Please, check app.config.");
                }

                this.InitQueryComponent(connectID, "TQP_FOI_IMAGES.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        #endregion

        public void CreateFOIImagesMulti(string[,] Paras)
        {
            this.ExecuteMultiple("INSERT_TQP_FOI_IMAGES", null, Paras);
        }

        public DataTable SelectFOIImageList(string strWaferSeq)
        {
            return this.GetDataTable("SELECT_TQP_FOI_IMAGES", null, new string[] { strWaferSeq });
        }

        public DataTable SelectFOIImageListMulti(string[] strWaferSeq)
        {
            string strSequences = string.Format("'{0}'", string.Join("','", strWaferSeq));
            return this.GetDataTable("SELECT_TQP_FOI_IMAGES_MULTI", new string[] { strSequences }, null);
        }

        public void DeleteFOIImageList(string strWaferSeq)
        {
            this.Execute("DELETE_TQP_FOI_IMAGES", null, new string[] { strWaferSeq });
        }
    }
}
