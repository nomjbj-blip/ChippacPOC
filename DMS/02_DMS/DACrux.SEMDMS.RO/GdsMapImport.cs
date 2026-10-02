using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.SEMDMS.RO
{
    /// <summary>기존 DMS 원격 호출 방식으로 GDS Map DB 저장(Import) 서비스를 화면에 제공한다.</summary>
    public class GdsMapImport
    {
        DACrux.SEMDMS.Interface.iGdsMapImport m_OBJ;

        public GdsMapImport()
        {
            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_DACRUX_DMS);
            object obj = Activator.GetObject(typeof(DACrux.SEMDMS.Interface.iGdsMapImport),
                        strUrl + "/DACrux.SEMDMS.BSL.GdsMapImport.bin");
            m_OBJ = obj as DACrux.SEMDMS.Interface.iGdsMapImport;
        }

        #region [TQP_GDS_MAP]

        public DataTable GetMapInfo(string strFactory, string strDeviceID)
        {
            try
            {
                return m_OBJ.GetMapInfo(strFactory, strDeviceID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteMap(string strFactory, string strDeviceID, string strUpdateUser)
        {
            try
            {
                return m_OBJ.DeleteMap(strFactory, strDeviceID, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [TQP_GDS_IMPORT_JOB]

        public DataTable BeginMapImport(string[] ImportInfo)
        {
            try
            {
                return m_OBJ.BeginMapImport(ImportInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetImportJob(string strImportJobSeq)
        {
            try
            {
                return m_OBJ.GetImportJob(strImportJobSeq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CancelImportJob(string strImportJobSeq, string strUpdateUser)
        {
            try
            {
                m_OBJ.CancelImportJob(strImportJobSeq, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable CompleteMapImport(string strImportJobSeq, string strUpdateUser)
        {
            try
            {
                return m_OBJ.CompleteMapImport(strImportJobSeq, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [TQP_GDS_IMPORT_BATCH]

        public DataTable GetImportBatch(string strImportJobSeq)
        {
            try
            {
                return m_OBJ.GetImportBatch(strImportJobSeq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [TQP_GDS_LAYER]

        public int CreateLayerBatch(string strImportJobSeq, long lBatchNo, string[,] aParas, string strUpdateUser)
        {
            try
            {
                return m_OBJ.CreateLayerBatch(strImportJobSeq, lBatchNo, aParas, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [TQP_GDS_SOURCE_EL]

        public int CreateSourceBatch(string strImportJobSeq, long lBatchNo, string[,] aParas, string strUpdateUser)
        {
            try
            {
                return m_OBJ.CreateSourceBatch(strImportJobSeq, lBatchNo, aParas, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [TQP_GDS_PLACED_EL]

        public int CreatePlacedBatch(string strImportJobSeq, long lBatchNo, string[,] aParas, string strUpdateUser)
        {
            try
            {
                return m_OBJ.CreatePlacedBatch(strImportJobSeq, lBatchNo, aParas, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
