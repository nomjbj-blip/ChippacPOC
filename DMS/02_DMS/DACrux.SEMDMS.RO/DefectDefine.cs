using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.SEMDMS.RO
{
    public class DefectDefine
    {
        DACrux.SEMDMS.Interface.iDefectDefine m_OBJ;

        public DefectDefine()
        {
            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_DACRUX_DMS);
            object obj = Activator.GetObject(typeof(DACrux.SEMDMS.Interface.iDefectDefine),
                        strUrl + "/DACrux.SEMDMS.BSL.DefectDefine.bin");
            m_OBJ = obj as DACrux.SEMDMS.Interface.iDefectDefine;
        }

        #region [TQD_SETUP]

        public long GetSetupInfo(string SetupID)
        {
            try
            {
                return m_OBJ.GetSetupInfo(SetupID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long GetSetupInfo(string SetupID, string strX, string strY)
        {
            try
            {
                return m_OBJ.GetSetupInfo(SetupID, strX, strY);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateSetupData(string strSetupSeq, string strStreetX, string strStreetY)
        {
            try
            {
                m_OBJ.UpdateSetupData(strSetupSeq, strStreetX, strStreetY);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateSetupInfo(string[] SetupInfo)
        {
            try
            {
                m_OBJ.CreateSetupInfo(SetupInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [TQD_LOT]


        public long GetLotSeq(string[] LotInfo)
        {
            try
            {
                return m_OBJ.GetLotSeq(LotInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateLotInfo(string[] LotInfo)
        {
            try
            {
                m_OBJ.CreateLotInfo(LotInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [TQD_WAFER]

        public long GetWaferSeq(string[] WaferInfo)
        {
            try
            {
                return m_OBJ.GetWaferSeq(WaferInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateWaferInfo(string[] WaferInfo)
        {
            try
            {
                m_OBJ.CreateWaferInfo(WaferInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [TQD_STEP]

        public long GetStepSeq(string[] StepInfo)
        {
            try
            {
                return m_OBJ.GetStepSeq(StepInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateStepInfo(string[] StepInfo)
        {
            try
            {
                m_OBJ.CreateStepInfo(StepInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [TQD_INSP_INFO]

        public void CreateInspInfo(string[] InspInfo)
        {
            try
            {
                m_OBJ.CreateInspInfo(InspInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetInspInfo(string strStepSeq)
        {
            try
            {
                return m_OBJ.GetInspInfo(strStepSeq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [TQD_DEFECT]

        public int GetDefectCount(string strWaferSeq, string strStep)
        {
            try
            {
                return m_OBJ.GetDefectCount( strWaferSeq, strStep);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateDefect(string[,] aParas)
        {
            try
            {
                m_OBJ.CreateDefect(aParas);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void DeleteDefectInfo(string strWaferSeq, string strStep)
        {
            try
            {
                m_OBJ.DeleteDefectInfo(strWaferSeq, strStep);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region [TQD_IMAGES]

        //public void CreateImages(string[] ImageInfo)
        //{
        //    try
        //    {
        //        m_OBJ.CreateImages(ImageInfo);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void CreateImagesMulti(string[,] ImageInfo)
        {
            try
            {
                m_OBJ.CreateImagesMulti(ImageInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void DeleteImages(string strStepSeq, string strWaferSeq)
        {
            try
            {
                m_OBJ.DeleteImages(strStepSeq, strWaferSeq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [TQD_PRODUCT]

        public DataTable GetProductInfo(string strProduct)
        {
            try
            {
                return m_OBJ.GetProductInfo(strProduct);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void CreateProduct(string[] ProductInfo)
        {
            try
            {
                m_OBJ.CreateProduct(ProductInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [TQD_SETUP_MAP]

        public int GetSetupMapCount(long setup_seq)
        {
            try
            {
                return m_OBJ.GetSetupMapCount(setup_seq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateSetupMap(string[,] aParas)
        {
            try
            {
                m_OBJ.CreateSetupMap(aParas);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
