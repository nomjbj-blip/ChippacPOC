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

        #endregion

        #region [TQD_INSP_INFO]

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


        #endregion

        #region [TQD_IMAGES]


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
